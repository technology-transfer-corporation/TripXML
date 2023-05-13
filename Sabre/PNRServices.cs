using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using TripXMLMain;
using System.Collections.Generic;

namespace Sabre
{
    /// <summary>
    /// PNR Services Methods
    /// </summary>
    public class PNRServices : SabreBase
    {
        /// <summary>
        /// Read PNR Method
        /// </summary>
        public string PNRRead()
        {

            string strResponse;

            // *****************************************************************
            // Transform OTA PNRRead Request into Native Sabre Request     *
            // ***************************************************************** 

            try
            {
                Version = "v03";
                string strRequest = SetRequest("Sabre_PNRReadRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 

                try
                {
                    var tagToReplace = Version == "v04" ? "</GetReservationRS>" : "</TravelItineraryReadRS>";
                    string dqbResponse = "";
                    strResponse = Version == "v04_"
                        ? ttSA.SendMessage(strRequest, "GetReservationRQ", "GetReservationRQ", ConversationID)
                        : ttSA.SendMessage(strRequest, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);

                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(Request);
                    var oRoot = oDoc.DocumentElement;
                    string strPNR = oRoot.SelectSingleNode("UniqueID/@ID").InnerText;

                    var initDoc = new XmlDocument();
                    initDoc.LoadXml(strResponse);
                    XmlElement initRoot = initDoc.DocumentElement;

                    // Check for Errors
                    if (strResponse.Contains("Success"))
                    {
                        #region *DQB
                        if (oRoot.Attributes["CheckIssuedTicket"] != null)
                        {
                            if (oRoot.Attributes["CheckIssuedTicket"].Value.ToLower() == "true")
                            {
                                dqbResponse = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>DQB*</HostCommand></Request></SabreCommandLLSRQ>";
                                CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "DQB*", "", ProviderSystems.LogUUID);
                                dqbResponse = ttSA.SendMessage(dqbResponse, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                            }
                        }
                        #endregion

                        #region *PQS
                        //string cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*PQS</HostCommand></Request></SabreCommandLLSRQ>";
                        //CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "PQS", "", ProviderSystems.LogUUID);
                        //cryptic = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                        string pricerq = ttSA.SendMessage("<DisplayPriceQuoteRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Version=\"2.5.1\"><AirItineraryPricingInfo><Summary Ind=\"true\"/></AirItineraryPricingInfo></DisplayPriceQuoteRQ>", "FareType", "DisplayPriceQuoteLLSRQ", ConversationID);

                        string strFaretype = "<DisplayPriceQuoteRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.3.0\"><AirItineraryPricingInfo><Record/></AirItineraryPricingInfo></DisplayPriceQuoteRQ>";
                        CoreLib.SendTrace(ProviderSystems.UserID, "FareType", "PD", strFaretype, ProviderSystems.LogUUID);
                        strFaretype = ttSA.SendMessage(strFaretype, "FareType", "DisplayPriceQuoteLLSRQ", ConversationID);
                        //strResponse = strResponse.Replace(tagToReplace, $"{cryptic}{strFaretype}<TimeStamp>{DateTime.Now.ToString("yyyy-MM-dd")}</TimeStamp>{dqbResponse}{tagToReplace}");
                        #endregion

                        var fareDoc = new XmlDocument();
                        fareDoc.LoadXml(pricerq);
                        var priceDate = DateTime.Now.ToString("yyyy-MM-dd");
                        if (fareDoc.DocumentElement.SelectSingleNode("PriceQuoteSummary/@CreateDate") != null)
                            priceDate = DateTime.Now.ToString("yyyy-") + fareDoc.DocumentElement.SelectSingleNode("PriceQuoteSummary/@CreateDate").Value;

                        //Reprice call. Collect Contolling Carrier & Global Ind
                        string strFareDetails = $"<OTA_AirPriceRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Version=\"2.17.0\"><PriceRequestInformation Retain=\"false\"><OptionalQualifiers><PricingQualifiers><BuyingDate>{priceDate}</BuyingDate></PricingQualifiers></OptionalQualifiers></PriceRequestInformation></OTA_AirPriceRQ>";
                        //CoreLib.SendTrace(ProviderSystems.UserID, "FareType", "PD", strFaretype, ProviderSystems.LogUUID);
                        strFareDetails = ttSA.SendMessage(strFareDetails, "Price", "OTA_AirPriceLLSRQ", ConversationID);
                        strResponse = strResponse.Replace(tagToReplace, $"{strFaretype}{pricerq}{strFareDetails}<TimeStamp>{DateTime.Now.ToString("yyyy-MM-dd")}</TimeStamp>{dqbResponse}{tagToReplace}");

                        #region *H
                        List<string> lstFOP = new List<string>();
                        string strDisplayHI = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*H</HostCommand></Request></SabreCommandLLSRQ>";
                        CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "HDK", "", ProviderSystems.LogUUID);
                        string strHI = ttSA.SendMessage(strDisplayHI, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                        int iStart = strHI.IndexOf("<Response>");
                        int iEnd = strHI.IndexOf("</Response>");
                        strHI = strHI.Substring(iStart, iEnd - iStart).Replace("<Response>", "");
                        var lstLines = strHI.Split(new string[] { "<![CDATA[", "]]>", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();


                        var sbH = new StringBuilder("<PNR_HDK>");
                        foreach (string line in lstLines.GetRange(1, lstLines.Count - 1))
                        {
                            var strline = line.Trim().Replace(")&gt;", "").Replace("&gt;", "");
                            if (strline.StartsWith("ADK") | strline.StartsWith("R-"))
                                continue;

                            bool isGood = false;
                            if (strline.StartsWith("A"))
                            {
                                isGood = Regex.IsMatch(strline, @"(A[0-9]{1}F\s)|(AFP\s)");
                                if (isGood)
                                {
                                    if (!strline.Contains("CHECK") && !strline.Contains("CASH"))
                                    {
                                        if (strline.Length < 20)
                                        {
                                            var index = lstLines.IndexOf(line);
                                            strline += lstLines[index + 1];
                                        }
                                    }
                                    var fopElems = SetFOP(strline);
                                    var isCanced = lstLines.Exists(l => l.StartsWith(strline.Contains("CHECK") ? $"XFP  {fopElems.CCNumber}" : $"XFP  *{fopElems.CCType}{fopElems.CCNumber}"));
                                    var histLine = $"<PNR_HDK_FOP CCType=\"{fopElems.CCType}\" Exp=\"{fopElems.Exp}\" Active=\"{!isCanced}\">{fopElems.CCNumber}</PNR_HDK_FOP>";

                                    if (!lstFOP.Exists(l => l.Equals(histLine)))
                                        lstFOP.Add(histLine);
                                }
                            }

                            isGood = Regex.IsMatch(strline, @"[a-zA-Z0-9]{4}\s[a-zA-Z0-9]{4}\*[A-Z0-9]{3}\s[0-9]{4}\/[a-zA-Z0-9]{5}");

                            // If Not String.IsNullOrEmpty(line) AndAlso Not line.Trim.StartsWith("R-") AndAlso line.Trim.Contains("*") Then
                            if (isGood)
                            {
                                var elems = strline.Split(new string[] { " ", "/", "*" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                string pcc = elems.First();
                                sbH.Append($"<Line PCC='{pcc}'>{strline}</Line>");
                            }
                        }

                        sbH.Append("</PNR_HDK>");
                        if (lstFOP.Count > 0)
                        {
                            sbH.Append(string.Join("\r\n", lstFOP));
                        }
                        strResponse = strResponse.Replace(tagToReplace, $"{sbH}{tagToReplace}");
                        #endregion
                    }
                    else
                    {
                        string cryptic = "";
                        strResponse = strResponse.Replace(tagToReplace, $"{cryptic}<TimeStamp>{DateTime.Now.ToString("yyyy-MM-dd")}</TimeStamp>{tagToReplace}");
                    }

                    // ----------------------------------
                    // Get Branded Fares
                    // ----------------------------------
                    // strResponse = GetBrandedFaresRequest(strResponse, strPNR, ConversationID, ttSA)
                    // ----------------------------------
                    // This is possible future developemnt
                    // ----------------------------------
                    // If strResponse.Contains("Ticketing") Then
                    // Dim ticketDoc = GetTicketDocument(strResponse, ConversationID, ttSA)
                    // strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", String.Format("{0}</OTA_TravelItineraryRS>", ticketDoc))
                    // End If

                    // *****************************************************************
                    // Transform Native Sabre PNRRead Response into OTA Response   *
                    // ***************************************************************** 
                    CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response size", strResponse.Length.ToString(), ProviderSystems.LogUUID);
                    if (strResponse.Length > 5500)
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response I", strResponse.Substring(0, (int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response II", strResponse.Substring((int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                    }
                    else
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response I", strResponse, ProviderSystems.LogUUID);
                    }


                    strResponse = strResponse.Replace(" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\"", "").Replace(" Version=\"2.0.0\"", "");
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_PNRReadRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttSA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception exx)
            {
                AddLog($"<M>{Request}<BL/>", ProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRRead, exx.Message, ProviderSystems);
            }
            return strResponse;
        }

        private (string AFP, string CCType, string CCNumber, string Exp) SetFOP(string strline)
        {
            try
            {
                //AFP  *VI4482330035908250/0123
                //A5F  -*VI4147202552404582\u008707/27
                //AFP  CHECK
                if (strline.Contains("\u0087"))
                    strline = strline.Replace("/", "").Replace("\u0087", "/").Replace("-", "");

                var elems = strline.Split(new[] { " ", "*", "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                return strline.Contains("CHECK") || strline.Contains("CASH")
                    ? (AFP: strline, CCType: elems[1].Substring(0, 2), CCNumber: elems[1], Exp: "")
                    : (AFP: strline, CCType: elems[1].Substring(0, 2), CCNumber: elems[1].Substring(2), Exp: elems.Last());
            }
            catch (Exception ex)
            {
                AddLog($"<Error>{ex.Message}</Error>", ProviderSystems.UserID);
            }
            return (AFP: strline, CCType: string.Empty, CCNumber: string.Empty, Exp: string.Empty);
        }

        private string GetTicketDocument(string response, string ConversationID, SabreAdapter ttSA)
        {
            string strResponse;
            try
            {
                var resp = new StringBuilder("<TicketingCoupons>");
                XmlDocument oDoc;
                XmlElement oRoot;
                response = response.Replace("xmlns:stl=\"http://services.sabre.com/STL/v01\" xmlns:msxsl=\"urn:schemas-microsoft-com:xslt\"", "").Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Replace("<?xml version=\"1.0\"?>", "");
                oDoc = new XmlDocument();
                oDoc.LoadXml(response);
                oRoot = oDoc.DocumentElement;

                foreach (XmlNode oNodeTicket in oRoot.SelectNodes("./TravelItinerary/ItineraryInfo/Ticketing[@eTicketNumber!='']"))
                {
                    var arElements = oNodeTicket.SelectSingleNode("@eTicketNumber").Value.Split(new string[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    // Dim strTicketCoupon As String = String.Format("<eTicketCouponRQ xmlns=""http://webservices.sabre.com/sabreXML/2011/10"" Version=""2.0.0""><Ticketing eTicketNumber=""{0}""/></eTicketCouponRQ>", arElements.Item(1))
                    string strTicketCoupon = string.Format("<eTicketCouponRQ Version=\"2.0.0\"><Ticketing eTicketNumber=\"{0}\"/></eTicketCouponRQ>", arElements[1]);
                    CoreLib.SendTrace(ProviderSystems.UserID, "TicketCoupon", "WETR", strTicketCoupon, ProviderSystems.LogUUID);
                    strTicketCoupon = ttSA.SendMessage(strTicketCoupon, "TicketCoupon", "eTicketCouponRQ", ConversationID);
                    resp.Append(strTicketCoupon);
                }

                resp.Append("</TicketingCoupons>");

                strResponse = resp.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                strResponse = string.Empty;
            }

            return strResponse;
        }

        private string GetBrandedFaresRequest(string response, string rl, string ConversationID, SabreAdapter ttSA)
        {
            string strResponse;
            try
            {
                // Branded Fares Check
                string strBrandedFare = $"<GetReservationRQ xmlns=\"http://webservices.sabre.com/pnrbuilder/v1_19\" Version=\"1.19.0\"><Locator>{rl}</Locator><RequestType>Stateful</RequestType><ReturnOptions PriceQuoteServiceVersion = \"4.0.0\" ><SubjectAreas><SubjectArea>PRICE_QUOTE</SubjectArea></SubjectAreas></ReturnOptions></GetReservationRQ>";
                CoreLib.SendTrace(ProviderSystems.UserID, "PRICE_QUOTE", "PQ", strBrandedFare, ProviderSystems.LogUUID);
                strBrandedFare = ttSA.SendMessage(strBrandedFare, "PRIVE_QUOTE", "GetReservationRQ", ConversationID);
                strBrandedFare = CleanSabreReply(strBrandedFare);
                var oDoc = new XmlDocument();
                XmlElement oRoot;
                oDoc.LoadXml(strBrandedFare);
                oRoot = oDoc.DocumentElement;
                string pq = oRoot.SelectSingleNode("PriceQuote").InnerXml.Replace(" xmlns=\"http://www.sabre.com/ns/Ticketing/pqs/1.0\"", "");

                // In Case if pricing command was WPE request Brand Infomation by FareBasis Code
                // pq = ReCheckBrandedFareByFareBasisCode(pq,ConversationID,ttSA)
                var resp = new StringBuilder("<FareFamily>");
                resp.Append($"{pq}");
                resp.Append("</FareFamily>");

                strResponse = response.Replace("</TravelItineraryReadRS>", $"{resp}</TravelItineraryReadRS>");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
            }

            return strResponse;
        }

        private string ReCheckBrandedFareByFareBasisCode(string xmlPriceQuote, string ConversationID, SabreAdapter ttSA)
        {
            try
            {
                // -- SABRE documentation  ------------
                // https://developer.sabre.com/docs/soap_apis/air/book/air_fare_by_city_pairs
                // Format: FQ{from}{to}{travelDate:MM-yy}‡Q{FareBasis}/{TicketDesignator}-{airline}
                // Equivalent Sabre host command: FQIADLHR21FEB‡QKL287NCV-UA
                // ------------------------------------
                // Reload XML document with clean data
                var oDoc = new XmlDocument();
                XmlElement oRoot;
                oDoc.LoadXml(xmlPriceQuote);
                oRoot = oDoc.DocumentElement;
                var segments = oRoot.SelectNodes("Details/SegmentInfo");
                string strFareRQ = string.Empty;
                string strFare = string.Empty;
                foreach (XmlNode seg in segments)
                {
                    if (!seg.SelectSingleNode("BrandedFare").HasChildNodes)
                    {
                        // 1. Call FareLLSRQ
                        // 2. Get BrandFare node from response
                        // 3. Replace old node in strBrandFare with one from new response 
                        var elFareBasis = seg.SelectSingleNode("FareBasis").InnerText.Split('/').ToList();
                        string mCarrier = seg.SelectSingleNode("Flight/MarketingFlight").InnerText;
                        string depDate = Convert.ToDateTime(seg.SelectSingleNode("Flight/Departure/DateTime").InnerText).ToString("MM-yy");
                        string depAirport = seg.SelectSingleNode("Flight/Departure/CityCode").InnerText;
                        string arAirport = seg.SelectSingleNode("Flight/Arrival/CityCode").InnerText;
                        string strFRQ = $"<FareRQ Version=\"2.9.0\" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><OptionalQualifiers><FlightQualifiers><VendorPrefs><Airline Code=\"{mCarrier}\"/></VendorPrefs></FlightQualifiers><PricingQualifiers><FareBasis Code=\"";
                        if (elFareBasis.Count.Equals(1))
                        {
                            strFRQ += $"{elFareBasis[0]}\"";
                        }
                        else
                        {
                            strFRQ += $"{elFareBasis[0]}\" TicketDesignator=\"{elFareBasis[1]}\"";
                        }

                        strFRQ += $" /></PricingQualifiers><TimeQualifiers><TravelDateOptions Start=\"{depDate}\"/></TimeQualifiers></OptionalQualifiers><OriginDestinationInformation><FlightSegment><DestinationLocation LocationCode=\"{arAirport}\"/><OriginLocation LocationCode=\"{depAirport}\"/></FlightSegment></OriginDestinationInformation></FareRQ>";
                        if (!string.Compare(strFareRQ, strFRQ).Equals(0))
                        {
                            strFareRQ = strFRQ;
                            CoreLib.SendTrace(ProviderSystems.UserID, "FareLLSRQ", "Fare", strFareRQ, ProviderSystems.LogUUID);
                            strFare = ttSA.SendMessage(strFareRQ, "FareRQ", "FareLLSRQ", ConversationID);
                            strFare = CleanSabreReply(strFare);
                        }

                        var oFDoc = new XmlDocument();
                        XmlElement oFRoot;
                        oFDoc.LoadXml(strFare);
                        oFRoot = oFDoc.DocumentElement;
                        XmlNode xmlBrand;
                        if (elFareBasis.Count.Equals(1))
                        {
                            xmlBrand = oFRoot.SelectSingleNode($"FareBasis[@Code='{elFareBasis[0]}']");
                        }
                        else
                        {
                            xmlBrand = oFRoot.SelectSingleNode($"FareBasis[@Code='{elFareBasis[0]}/{elFareBasis[1]}']");
                        }

                        if (xmlBrand == null)
                        {
                            continue;
                        }

                        var brand = new XElement("BrandedFare", new XAttribute("code", xmlBrand.SelectSingleNode("AdditionalInformation/Brand/@BrandCode").InnerText), new XAttribute("description", xmlBrand.SelectSingleNode("AdditionalInformation/Brand/@BrandName").InnerText), new XAttribute("programCode", xmlBrand.SelectSingleNode("AdditionalInformation/Brand/@ProgramCode").InnerText), new XAttribute("programDescription", xmlBrand.SelectSingleNode("AdditionalInformation/Brand/@ProgramName").InnerText));
                        seg.InnerXml = seg.InnerXml.Replace("<BrandedFare />", brand.ToString());
                    }
                }

                return oRoot.InnerXml;
            }
            catch (Exception ex)
            {
                //throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                return xmlPriceQuote;
            }
        }

        private XmlNode CreateNewXMLNode(string xmlInputString)
        {
            if (string.IsNullOrEmpty(xmlInputString.Trim()))
            {
                throw new ArgumentNullException("xmlInputString");
            }

            var xd = new XmlDocument();
            xd.LoadXml(xmlInputString);
            return xd;
        }

        private string CleanSabreReply(string sabreReply)
        {
            try
            {
                var lstNS = sabreReply.Split(' ').ToList();
                foreach (string elem in lstNS.FindAll(l => l.StartsWith("xmlns:")))
                {
                    var strNS = elem.Split(new char[] { ':', '=' }).ToList();
                    sabreReply = sabreReply.Replace(elem, "").Replace(strNS[1] + ":", "");
                }
            }
            catch (Exception ex)
            {
                return "";
            }

            return sabreReply;
        }

        /// <summary>
        /// Cancel PNR Method
        /// </summary>
        public string PNRCancel()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA PNRCancel Request into Native Sabre Request     *
            // ***************************************************************** 

            try
            {
                string strRead;
                string strCancel;
                string strEndTransaction;
                string strRequest = SetRequest("Sabre_PNRCancelRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // ********************
                // Get All Requests  * 
                // ********************

                var oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                var oRoot = oDoc.DocumentElement;
                if (oRoot.SelectSingleNode("Read") is null)
                {
                    throw new Exception("Request is missing mandatory Read elements.");
                }
                else
                {
                    strRead = oRoot.SelectSingleNode("Read").InnerXml.Replace(" xmlns=\"\"", "");
                }

                if (oRoot.SelectSingleNode("Cancel") is null)
                {
                    throw new Exception("Request is missing mandatory Cancel elements.");
                }
                else
                {
                    strCancel = oRoot.SelectSingleNode("Cancel").InnerXml.Replace(" xmlns=\"\"", "");
                }

                if (oRoot.SelectSingleNode("ET") is null)
                {
                    throw new Exception("Request is missing mandatory ET elements.");
                }
                else
                {
                    strEndTransaction = oRoot.SelectSingleNode("ET").InnerXml.Replace(" xmlns=\"\"", "");
                }


                // *******************
                // Create Session    *
                // *******************                
                var ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 

                CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "ReadPNR", "", ProviderSystems.LogUUID);
                strResponse = ttSA.SendMessage(strRead, "OTA_TravelItineraryReadRQ", "OTA_TravelItineraryReadRQ", ConversationID);

                // Check for Errors
                if (strResponse.Contains("Success"))
                {
                    CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "strResponse", strResponse, ProviderSystems.LogUUID);
                    CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "Cancel Segments", "", ProviderSystems.LogUUID);
                    strResponse = ttSA.SendMessage(strCancel, "Cancel", "OTA_CancelLLSRQ", ConversationID);

                    // Check for Errors
                    if (strResponse.Contains("Success"))
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "strResponse", strResponse, ProviderSystems.LogUUID);
                        CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "ET", "", ProviderSystems.LogUUID);
                        strResponse = ttSA.SendMessage(strEndTransaction, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                    }
                }

                // *****************************************************************
                // Transform Native Sabre PNRCancel Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var strToReplace = "</OTA_CancelLLSRS>";

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{strToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_PNRCancelRS.xsl");

                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttSA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRCancel, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        /// <summary>
        /// Read PNR Method
        /// </summary>
        public string PNRReprice()
        {
            string strResponse;
            // *****************************************************************
            // Transform OTA PNRRead Request into Native Sabre Request         *
            // ***************************************************************** 
            try
            {
                //var bCloseSession = true;
                bool bStoreFare = true;
                string strRepriceResp = "";
                string strPaxCombined = "";

                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                XmlNodeList nodesToDel = oDoc.SelectNodes("//FareSegments/AirSegments[text()='VOID']");
                for (int i = nodesToDel.Count - 1; i >= 0; i--)
                {
                    nodesToDel[i].ParentNode.RemoveChild(nodesToDel[i]);
                }
                Request = oDoc.OuterXml;

                string strRequest = SetRequest("Sabre_PNRRepriceRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                var oRoot = oDoc.DocumentElement;

                var oDocT = new XmlDocument();
                oDocT.LoadXml(strRequest);
                var oRootT = oDocT.DocumentElement;

                var strRead = oRootT.SelectSingleNode("PNRRead").InnerXml;
                var strRedisplay = oRootT.SelectSingleNode("PNRRedisplay").InnerXml;
                var strPriceCombined = oRootT.SelectSingleNode("PriceCombined").InnerXml;

                var strPrice = Request.Contains("TicketDesignator") ? oRootT.SelectSingleNode("Price").InnerXml : oRootT.SelectSingleNode("Price").OuterXml;

                // Bug 1310
                // strPrice = strPrice.Replace("RPH=""/""", "").Replace("<FareBasis Code=""/"" />", "")

                var strPQS = oRootT.SelectSingleNode("Cryptic").InnerXml;
                if (oRoot.SelectSingleNode("@StoreFare") != null)
                {
                    if (oRoot.SelectSingleNode("@StoreFare").InnerText == "false")
                    {
                        bStoreFare = false;
                    }
                }

                strRequest = !inSession ? strRead : strRedisplay;

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                try
                {
                    var strReadResp = ttSA.SendMessage(strRequest, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                    strReadResp = strReadResp.Replace(" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\"", "").Replace(" Version=\"2.17.0\"", "");
                    // strReadResp = strReadResp.Replace("<or:", "<").Replace("</or:", "</").Replace("xsi:type=""or:", "type=""")

                    var oDocResp = new XmlDocument();
                    oDocResp.LoadXml(strReadResp);
                    var oRootResp = oDocResp.DocumentElement;

                    var validatingCarrier = string.Empty;
                    if (oRootResp.SelectSingleNode("TravelItinerary/ItineraryInfo/ItineraryPricing/PriceQuote[MiscInformation/SignatureLine/@Status='ACTIVE']") != null)
                    {
                        var arlns = oRootResp.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item/FlightSegment/MarketingAirline").Cast<XmlNode>().ToList().Select(x => x.Attributes["Code"].Value).ToList();
                        if (arlns.Any() && !arlns.TrueForAll(a => a.Equals(arlns.First())))
                        {
                            validatingCarrier = oRootResp.SelectSingleNode("TravelItinerary/ItineraryInfo/ItineraryPricing/PriceQuote[MiscInformation/SignatureLine/@Status='ACTIVE']/PricedItinerary/@ValidatingCarrier").InnerText;
                        }
                    }

                    if (oRootResp.SelectSingleNode("TravelItinerary/ItineraryInfo/ItineraryPricing/PriceQuote") is null)
                    {
                        strRepriceResp = "<Error>No stored fare exist in PNR</Error>";
                    }
                    else if (Request.Contains("TicketDesignator"))
                    {
                        strPQS = GetPQS(ttSA, strPQS);

                        if (bStoreFare)
                        {
                            string strPQDel = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>PQD-ALL</HostCommand></Request></SabreCommandLLSRQ>";
                            ttSA.SendMessage(strPQDel, "PQD-ALL", "SabreCommandLLSRQ", ConversationID);
                        }

                        var bMulty = IsMultiplePrice(strPrice);
                        var oDocPrice = new XmlDocument();
                        oDocPrice.LoadXml(bMulty ? $"<Price>{strPrice}</Price>" : strPrice);
                        var oRootPrice = oDocPrice.DocumentElement;
                        var nsmgr = new XmlNamespaceManager(oDocPrice.NameTable);
                        nsmgr.AddNamespace("sx", "http://webservices.sabre.com/sabreXML/2011/10");

                        if (bMulty)
                        {
                            strPrice = "";
                            int i = 1;
                            foreach (XmlNode oNodePricer in oRootPrice.SelectNodes("sx:OTA_AirPriceRQ", nsmgr))
                            {
                                string strRepriceReq = oNodePricer.OuterXml;
                                var pDoc = new XmlDocument();
                                pDoc.LoadXml(strRepriceReq);
                                XmlNodeList pRoot = pDoc.GetElementsByTagName("PassengerType");
                                List<string> strPQ = new List<string>();

                                foreach (XmlNode pr in pRoot)
                                {
                                    string vAl = pr.Attributes["Code"].Value;
                                    if (!strPQ.Exists(v => v.Equals(vAl)))
                                        strPQ.Add(pr.Attributes["Code"].Value);
                                }
                                string strPassengers = GetPassangerInfo(strPQS, strPQ);

                                strRepriceReq = strRepriceReq.Replace("<NameSelect>NS</NameSelect>", strPassengers);

                                if (!string.IsNullOrEmpty(validatingCarrier))
                                    strRepriceReq = AddValidatingCarrier(strRepriceReq, validatingCarrier);

                                strRepriceResp += ttSA.SendMessage(strRepriceReq, "Price", "OTA_AirPriceLLSRQ", ConversationID);
                                if (strRepriceResp.Contains("NO COMBINABLE FARES FOR CLASS USED") || strRepriceResp.Contains("NEED MORE PSGR TYPES OR NAME SELECT") || strRepriceResp.Contains("USE INF PSGR TYPE CODE FOR I"))
                                    break;
                            }
                        }
                        else
                        {

                            string strRepriceReq = oRootPrice.OuterXml;
                            var pDoc = new XmlDocument();
                            pDoc.LoadXml(strRepriceReq);
                            XmlNodeList pRoot = pDoc.GetElementsByTagName("PassengerType");
                            List<string> strPQ = new List<string>();

                            foreach (XmlNode pr in pRoot)
                            {
                                string vAl = pr.Attributes["Code"].Value;
                                if (!strPQ.Exists(v => v.Equals(vAl)))
                                    strPQ.Add(pr.Attributes["Code"].Value);
                            }

                            string strPassengers = GetPassangerInfo(strPQS, strPQ);
                            strPrice = strPrice.Replace("<NameSelect>NS</NameSelect>", strPassengers);

                            if (!string.IsNullOrEmpty(validatingCarrier))
                                strPrice = AddValidatingCarrier(strPrice, validatingCarrier);

                            strRepriceResp = ttSA.SendMessage(strPrice, "Price", "OTA_AirPriceLLSRQ", ConversationID);
                        }

                        strRepriceResp = strRepriceResp.Replace("<OTA_AirPriceRS Version=\"2.17.0\">", "").Replace("</OTA_AirPriceRS>", "");
                    }
                    else
                    {
                        strPQS = GetPQS(ttSA, strPQS);

                        // Dim oNodeSF As XmlNode = Nothing
                        var oDocPrice = new XmlDocument();
                        oDocPrice.LoadXml(strPrice);
                        var oRootPrice = oDocPrice.DocumentElement;
                        var nsmgr = new XmlNamespaceManager(oDocPrice.NameTable);
                        nsmgr.AddNamespace("sx", "http://webservices.sabre.com/sabreXML/2011/10");

                        int i = 1;

                        if (bStoreFare)
                        {
                            string strPQDel = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>PQD-ALL</HostCommand></Request></SabreCommandLLSRQ>";
                            ttSA.SendMessage(strPQDel, "PQD-ALL", "SabreCommandLLSRQ", ConversationID);
                        }

                        if (!strPrice.Contains("<Brand "))
                        {
                            foreach (XmlNode oNodePricer in oRootPrice.SelectNodes("sx:OTA_AirPriceRQ", nsmgr))
                            {
                                string strPQ = oRoot.SelectSingleNode($"StoredFare[position()={i}]/@RPH").InnerText;
                                string strPassengers = GetPassangerInfo(strPQS, strPQ);
                                string strRepriceReq = oNodePricer.OuterXml;
                                strRepriceReq = strRepriceReq.Replace("<NameSelect>NS</NameSelect>", strPassengers);
                                strPaxCombined += strPassengers;

                                // here we get fare basis codes from PNR to include in reprice command when we have reprice with ticket designator and/or discount
                                if (Request.Contains("<Discount") | Request.Contains("<TicketDesignator>"))
                                {
                                    string cmdPrice = "";
                                    string itinOption = "<ItineraryOptions>";
                                    foreach (XmlNode oNodeResp in oRootResp.SelectNodes($"TravelItinerary/ItineraryInfo/ItineraryPricing/PriceQuote[@RPH='{i} ']/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/FlightSegment[FareBasis/@Code!='' and FareBasis/@Code!='VOID']"))
                                    {
                                        cmdPrice += $"<CommandPricing RPH=\"{oNodeResp.SelectSingleNode("@SegmentNumber").InnerText}\">";
                                        if (oRoot.SelectSingleNode($"StoredFare[@RPH='{strPQ}']/Discount") != null)
                                        {
                                            var oNodeDiscount = oRoot.SelectSingleNode($"StoredFare[@RPH='{strPQ}']/Discount");
                                            if (oNodeDiscount.SelectSingleNode("@Amount") != null)
                                            {
                                                cmdPrice += $"<Discount Amount=\"{oNodeDiscount.SelectSingleNode("@Amount").InnerText}\"";
                                            }
                                            else
                                            {
                                                cmdPrice += $"<Discount Percent=\"{oNodeDiscount.SelectSingleNode("@Percent").InnerText}\"";
                                            }

                                            if (oRoot.SelectSingleNode($"StoredFare[@RPH='{strPQ}']/TicketDesignator") != null)
                                            {
                                                cmdPrice += $" AuthCode=\"{oRoot.SelectSingleNode($"StoredFare[@RPH='{strPQ} ']/TicketDesignator").InnerText}\"/>";
                                            }
                                            else
                                            {
                                                cmdPrice += "/>";
                                            }
                                        }

                                        if (Request.Contains("<TicketDesignator>"))
                                        {
                                            cmdPrice += $"<FareBasis Code=\"{oNodeResp.SelectSingleNode("FareBasis/@Code").InnerText}";
                                            cmdPrice += oNodeResp.SelectSingleNode("FareBasis/@Code").InnerText.Contains("/") ? "\"/>" : "/\"/>";
                                        }

                                        cmdPrice += "</CommandPricing>";
                                        itinOption += $"<SegmentSelect Number=\"{oNodeResp.SelectSingleNode("@SegmentNumber").InnerText}\" RPH=\"{oNodeResp.SelectSingleNode("@SegmentNumber").InnerText}\"/>";
                                    }

                                    itinOption = itinOption + "</ItineraryOptions>";
                                    CoreLib.SendTrace(ProviderSystems.UserID, "cmdPrice", "cmdPrice", cmdPrice, ProviderSystems.LogUUID);
                                    CoreLib.SendTrace(ProviderSystems.UserID, "itinOption", "itinOption", itinOption, ProviderSystems.LogUUID);
                                    strRepriceReq = strRepriceReq.Replace("<CommandPricing>CP</CommandPricing>", cmdPrice + itinOption);
                                    CoreLib.SendTrace(ProviderSystems.UserID, "strRepriceReq", "strRepriceReq", strRepriceReq, ProviderSystems.LogUUID);
                                }

                                if (!string.IsNullOrEmpty(validatingCarrier))
                                    strRepriceReq = AddValidatingCarrier(strRepriceReq, validatingCarrier);

                                strRepriceResp += ttSA.SendMessage(strRepriceReq, "Price", "OTA_AirPriceLLSRQ", ConversationID);
                                if (strRepriceResp.Contains("Error"))
                                {
                                    strRepriceResp = strRepriceResp.Replace("<OTA_AirPriceRS Version=\"2.17.0\">", "").Replace("</OTA_AirPriceRS>", "");
                                    break;
                                }

                                strRepriceResp = strRepriceResp.Replace("<OTA_AirPriceRS Version=\"2.17.0\">", "").Replace("</OTA_AirPriceRS>", "");
                                i++;
                            }
                        }
                        else
                        {
                            var bMulty = IsMultiplePrice(strPrice);
                            if (bMulty)
                            {
                                strPrice = "";
                                i = 1;
                                foreach (XmlNode oNodePricer in oRootPrice.SelectNodes("sx:OTA_AirPriceRQ", nsmgr))
                                {
                                    string strRepriceReq = oNodePricer.OuterXml;
                                    var pDoc = new XmlDocument();
                                    pDoc.LoadXml(strRepriceReq);
                                    XmlNodeList pRoot = pDoc.GetElementsByTagName("PassengerType");
                                    List<string> strPQ = new List<string>();

                                    foreach (XmlNode pr in pRoot)
                                    {
                                        string vAl = pr.Attributes["Code"].Value;
                                        if (!strPQ.Exists(v => v.Equals(vAl)))
                                        {
                                            //We working with Child code like C09 but PQS returns CNN
                                            if (pr.Attributes["Code"].Value.StartsWith("C") && !strPQS.Contains(pr.Attributes["Code"].Value))
                                            {
                                                strPQS = strPQS.Replace("*CNN*", $"*{pr.Attributes["Code"].Value}*");
                                            }
                                            strPQ.Add(pr.Attributes["Code"].Value);
                                        }
                                    }
                                    string strPassengers = GetPassangerInfo(strPQS, strPQ);

                                    strRepriceReq = strRepriceReq.Replace("<NameSelect>NS</NameSelect>", strPassengers);

                                    if (!string.IsNullOrEmpty(validatingCarrier))
                                        strRepriceReq = AddValidatingCarrier(strRepriceReq, validatingCarrier);

                                    strRepriceResp += ttSA.SendMessage(strRepriceReq, "Price", "OTA_AirPriceLLSRQ", ConversationID);
                                }
                            }
                            else
                            {
                                strPrice = strPrice.Replace("<NameSelect>NS</NameSelect>", "").Replace("<Price>", "").Replace("</Price>", "");

                                if (!string.IsNullOrEmpty(validatingCarrier))
                                    strPrice = AddValidatingCarrier(strPrice, validatingCarrier);

                                strRepriceResp = ttSA.SendMessage(strPrice, "Price", "OTA_AirPriceLLSRQ", ConversationID);
                            }
                        }
                        { }
                    }

                    if (strRepriceResp.Contains("NO COMBINABLE FARES FOR CLASS USED") || strRepriceResp.Contains("NEED MORE PSGR TYPES OR NAME SELECT") || strRepriceResp.Contains("USE INF PSGR TYPE CODE FOR I"))
                    {
                        strPriceCombined = strPriceCombined.Replace("<NameSelect>NS</NameSelect>", strPaxCombined);

                        if (!string.IsNullOrEmpty(validatingCarrier))
                            strPriceCombined = AddValidatingCarrier(strPriceCombined, validatingCarrier);

                        strRepriceResp = ttSA.SendMessage(strPriceCombined, "Price", "OTA_AirPriceLLSRQ", ConversationID);
                        strRepriceResp = strRepriceResp.Replace("<OTA_AirPriceRS Version=\"2.17.0\">", "").Replace("</OTA_AirPriceRS>", "");
                    }

                    if (strRepriceResp.Contains("Error"))
                    {
                        string strER = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>IR</HostCommand></Request></SabreCommandLLSRQ>";
                        ttSA.SendMessage(strER, "IR", "SabreCommandLLSRQ", ConversationID);
                    }
                    else if (bStoreFare)
                    {
                        string strER = @"<SabreCommandLLSRQ xmlns=""http://webservices.sabre.com/sabreXML/2011/10"" Version=""2.0.0""><Request Output=""SCREEN"" MDRSubset=""AD01"" CDATA=""true""><HostCommand>6TRIPXML\ER</HostCommand></Request></SabreCommandLLSRQ>";
                        strResponse = ttSA.SendMessage(strER, "ER", "SabreCommandLLSRQ", ConversationID);
                        if (strResponse.Contains("*WARNING EDITS*") | strResponse.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strResponse.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED") | strResponse.Contains("END OR IGNORE PNR") | strResponse.Contains("INCORRECT TIME LIMIT - VERIFY  *PQ  DATE"))
                        {
                            strER = strER.Replace(@"6TRIPXML\ER", "ER");
                            strResponse = ttSA.SendMessage(strER, "ER", "SabreCommandLLSRQ", ConversationID);
                        }

                        if (strResponse.Contains("*WARNING EDITS*") | strResponse.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strResponse.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED") | strResponse.Contains("END OR IGNORE PNR") | strResponse.Contains("INCORRECT TIME LIMIT - VERIFY  *PQ  DATE"))
                        {
                            ttSA.SendMessage(strER, "ER", "SabreCommandLLSRQ", ConversationID);
                        }
                    }



                    // *****************************************************************
                    // Transform Native Sabre PNRRead Response into OTA Response   *
                    // ***************************************************************** 

                    var strToReplace = "</TravelItineraryReadRS>";

                    strResponse = !strRepriceResp.Contains("<OTA_AirPriceRS")
                            ? strReadResp.Replace("</TravelItineraryReadRS>", $"<OTA_AirPriceRS>{strRepriceResp}</OTA_AirPriceRS></TravelItineraryReadRS>")
                            : strReadResp.Replace("</TravelItineraryReadRS>", $"{strRepriceResp}</TravelItineraryReadRS>");

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{strToReplace}");


                    // CoreLib.SendTrace(ProviderSystems.UserID, "PNRReprice", "Final response", sb.Append("<TravelItineraryReadRS><OTA_AirPriceRS>").Append(strRepriceResp).Append("</OTA_AirPriceRS>").Append(ConversationID).Append("</TravelItineraryReadRS>").ToString(), ProviderSystems.LogUUID)
                    if (strResponse.Length > 5500)
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRReprice", "Final response I", strResponse.Substring(0, (int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRReprice", "Final response II", strResponse.Substring((int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                    }
                    else
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRReprice", "Final response I", strResponse, ProviderSystems.LogUUID);
                    }

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_PNRRepriceRS.xsl");
                    CoreLib.SendTrace(ProviderSystems.UserID, "strResponse", "Final strResponse", strResponse, ProviderSystems.LogUUID);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttSA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRReprice, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        private static string AddValidatingCarrier(string strRepriceReq, string valAL)
        {
            var oDocPriceRQ = new XmlDocument();
            oDocPriceRQ.LoadXml(strRepriceReq);
            var nsmgrRQ = new XmlNamespaceManager(oDocPriceRQ.NameTable);
            nsmgrRQ.AddNamespace("xs", "http://webservices.sabre.com/sabreXML/2011/10");
            var optQualifiersNode = oDocPriceRQ.DocumentElement.SelectSingleNode("//xs:OptionalQualifiers", nsmgrRQ);
            var alNode = oDocPriceRQ.CreateElement("FlightQualifiers", "http://webservices.sabre.com/sabreXML/2011/10");
            alNode.AppendChild(oDocPriceRQ.CreateElement("VendorPrefs", "http://webservices.sabre.com/sabreXML/2011/10"));
            alNode.FirstChild.AppendChild(oDocPriceRQ.CreateElement("Airline", "http://webservices.sabre.com/sabreXML/2011/10"));
            ((XmlElement)alNode.SelectSingleNode("//xs:Airline", nsmgrRQ)).SetAttribute("Code", valAL);
            optQualifiersNode.InsertBefore(alNode, optQualifiersNode.FirstChild);
            strRepriceReq = oDocPriceRQ.OuterXml;
            return strRepriceReq;
        }

        public string Queue()
        {
            string strResponse = "";

            // *****************************************************************
            // Transform OTA Queue Request into Native Sabre Request     *
            // ***************************************************************** 

            try
            {
                string strRequest = SetRequest("Sabre_QueueRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 

                var ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                if (strRequest.Contains("SabreCommandLLSRQ"))
                {
                    strResponse = ttSA.SendMessage(strRequest, "SabreCommandLLSRQ", "SabreCommandLLSRQ");
                    strResponse = strResponse.Replace("<![CDATA[", "<Line>").Replace("]]>", "</Line>").Replace("\r\n", "");
                }
                else if (strRequest.Contains("QueueCountRQ"))
                {
                    strResponse = ttSA.SendMessage(strRequest, "QueueCountLLSRQ", "QueueCountLLSRQ");
                }
                else if (strRequest.Contains("QueuePlaceRQ"))
                {
                    strResponse = ttSA.SendMessage(strRequest, "QueuePlaceLLSRQ", "QueuePlaceLLSRQ");
                }


                // *****************************************************************
                // Transform Native Sabre Queue Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var strToReplace = "</SabreCommandLLSRS>";
                    if (strResponse.Contains("QueueCountRS"))
                    {
                        strToReplace = "</QueueCountRS>";
                    }
                    else if (strResponse.Contains("Queue_ListReply"))
                    {
                        strToReplace = "</Queue_ListReply>";
                    }
                    else if (strResponse.Contains("QueuePlaceRS"))
                    {
                        strToReplace = "</QueuePlaceRS>";
                    }
                    else if (strResponse.Contains("Queue_MoveItemReply"))
                    {
                        strToReplace = "</Queue_MoveItemReply>";
                    }
                    else if (strResponse.Contains("Queue_RemoveItemReply"))
                    {
                        strToReplace = "</Queue_RemoveItemReply>";
                    }
                    else if (strResponse.Contains("MessagesOnly_Reply"))
                    {
                        strToReplace = "</MessagesOnly_Reply>";
                    }

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{strToReplace}");
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_QueueRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttSA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Queue, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string QueueRead()
        {
            string strResponse = "";

            // *****************************************************************
            // Transform OTA QueueRead Request into Native Sabre Request     *
            // ***************************************************************** 
            try
            {
                string cryptic;
                string strMessage;
                string strVerifyTickets = "";
                XmlDocument oDoc;
                XmlElement oRoot;
                XmlDocument oDocNative = new XmlDocument();
                XmlElement oRootNative;
                string dqbResponse;

                string strRequest = SetRequest("Sabre_QueueReadRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                oDocNative.LoadXml(strRequest);
                oRootNative = oDocNative.DocumentElement;

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                var ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                try
                {
                    var nsmgr = new XmlNamespaceManager(oDocNative.NameTable);
                    if (Request.Contains("AccessQueue"))
                    {
                        strMessage = "AccessQueue";
                        nsmgr.AddNamespace("sx", "http://webservices.sabre.com/sabreXML/2011/10");
                        strRequest = oRootNative.SelectSingleNode("sx:QueueAccessRQ", nsmgr).OuterXml;

                        if (oRootNative.SelectSingleNode("VerifyTickets") != null)
                        {
                            strVerifyTickets = oRootNative.SelectSingleNode("VerifyTickets").OuterXml;
                        }
                    }
                    else
                    {
                        if (Request.Contains("ItemOnQueue"))
                        {
                            if (Request.Contains("Redisplay"))
                            {
                                strMessage = "Redisplay";
                                nsmgr.AddNamespace("sx", "http://webservices.sabre.com/sabreXML/2011/10");
                                strRequest = oRootNative.SelectSingleNode("sx:TravelItineraryReadRQ", nsmgr).OuterXml;
                            }
                            else
                            {
                                strMessage = "ItemOnQueue";
                                nsmgr.AddNamespace("sx", "http://webservices.sabre.com/sabreXML/2011/10");
                                strRequest = oRootNative.SelectSingleNode("sx:SabreCommandLLSRQ", nsmgr).OuterXml;
                            }

                            if (oRootNative.SelectSingleNode("VerifyTickets") != null)
                            {
                                strVerifyTickets = oRootNative.SelectSingleNode("VerifyTickets").OuterXml;
                            }
                        }
                        else
                        {
                            strMessage = "ExitQueue";
                            nsmgr.AddNamespace("sx", "http://webservices.sabre.com/sabreXML/2011/10");
                            strRequest = oRootNative.SelectSingleNode("sx:SabreCommandLLSRQ", nsmgr).OuterXml;
                        }
                    }

                    if (strMessage != "Redisplay")
                    {
                        if (strRequest.Contains("QueueAccessRQ"))
                        {
                            strResponse = ttSA.SendMessage(strRequest, "QueueAccessRQ", "QueueAccessLLSRQ", ConversationID);
                            CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "strResponse", strResponse, ProviderSystems.LogUUID);
                        }
                        else if (strRequest.Contains("SabreCommandLLSRQ"))
                        {
                            strResponse = ttSA.SendMessage(strRequest, "SabreCommandLLSRQ", "SabreCommandLLSRQ", ConversationID);
                        }
                    }

                    // Check PNR or Errors in Native Response
                    if (strResponse.StartsWith("<Error"))
                    {
                        strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, "Sabre", "", false, "v03");
                        CoreLib.SendTrace(ProviderSystems.UserID, "QRead", "Error response", strResponse, ProviderSystems.LogUUID);
                        if (strMessage == "AccessQueue")
                        {
                            inSession = false;
                        }
                        else
                        {
                            strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"<ConversationID>{ConversationID}</ConversationID></OTA_TravelItineraryRS>");
                        }

                        return strResponse;
                    }

                    if (strMessage == "Redisplay" | strMessage == "AccessQueue" & (strResponse.Contains("TKT/TIME LIMIT") | strResponse.Contains("<UniqueID ID=")) | strMessage == "ItemOnQueue" & (strResponse.Contains("TKT/TIME LIMIT") | strResponse.Contains("RECEIVED FROM -") | strResponse.Contains("<UniqueID ID=")))
                    {
                        string strWarning = strResponse.Contains("QUEUE CYCLE COMPLETE") ? "<Warning Type=\"Sabre\">QUEUE CYCLE COMPLETE</Warning>" : "";

                        // Send PNR Redisplay
                        strRequest = "<TravelItineraryReadRQ Version=\"3.6.0\" xmlns=\"http://services.sabre.com/res/tir/v3_6\"><MessagingDetails><SubjectAreas><SubjectArea>FULL</SubjectArea></SubjectAreas></MessagingDetails></TravelItineraryReadRQ>";
                        strResponse = ttSA.SendMessage(strRequest, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                        strResponse = strResponse.Replace(" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\"", "").Replace(" Version=\"2.0.0\"", "");
                        if (strResponse.Contains("Error") && !strResponse.Contains("Success"))
                        {
                            ttSA.CloseSession(ConversationID);
                            ConversationID = null;
                            strResponse = CoreLib.GetNodeInnerText(strResponse, "Message", false);

                            strResponse = string.IsNullOrEmpty(strResponse)
                                ? "Cannot read PNR from queue"
                                : strResponse.Contains("NEED PNR")
                                    ? "SELECTED QUEUE WAS EMPTY"
                                    : strResponse;


                            // Ignore current PNR
                            string strER = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>I</HostCommand></Request></SabreCommandLLSRQ>";
                            ttSA.SendMessage(strER, "I", "SabreCommandLLSRQ", ConversationID);
                            strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, ProviderSystems);
                            return strResponse;
                        }

                        cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*PQS</HostCommand></Request></SabreCommandLLSRQ>";
                        CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "*PQS", "", ProviderSystems.LogUUID);
                        cryptic = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);

                        strResponse = strResponse.Replace("</TravelItineraryReadRS>", $"{cryptic}</TravelItineraryReadRS>");

                        string strFaretype = "<DisplayPriceQuoteRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.5.2\"><AirItineraryPricingInfo><Record/></AirItineraryPricingInfo></DisplayPriceQuoteRQ>";
                        CoreLib.SendTrace(ProviderSystems.UserID, "FareType", "PD", strFaretype, ProviderSystems.LogUUID);
                        strFaretype = ttSA.SendMessage(strFaretype, "FareType", "DisplayPriceQuoteLLSRQ", ConversationID);
                        strResponse = strResponse.Replace("</TravelItineraryReadRS>", $"{strFaretype}</TravelItineraryReadRS>");


                        #region *H
                        string strDisplayHI = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*H</HostCommand></Request></SabreCommandLLSRQ>";
                        CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "H", "", ProviderSystems.LogUUID);
                        string strHI = ttSA.SendMessage(strDisplayHI, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                        int iStart = strHI.IndexOf("<Response>");
                        int iEnd = strHI.IndexOf("</Response>");
                        strHI = strHI.Substring(iStart, iEnd - iStart).Replace("<Response>", "");
                        var lstLines = strHI.Split(new string[] { "<![CDATA[", "]]>", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                        // ---------------------
                        // A5H  H - Asc() - NO SCHD CHG FOUND ON PNR TO WORK 28AUG/1301
                        // R-POST BOOKING AUTOMATION
                        // 1BJC 1BJC*ASC 1301/28AUG20   ********************** need this entry ***
                        // A4S  SSR ADTK 1S TO DL BY 29AUG 2359 LGA OTHERWISE MAY BE XLD
                        // A4S  SSR ADTK 1S TO DL BY 29AUG FARE MAY NEED EARLIER TKT DTE
                        // R-HDQRMDL281748 983439B2-001 SSC
                        // PLT PLTRMDL 1248/28AUG20
                        // A5W  WT - A5C6 /ON
                        // A5H  H - NO CONTRACTS FOUND. OD. AIRLINE DL.
                        // R-TRIPXML
                        // A5C6 53UG*AW1 1143/28AUG20   ********************** need this entry ***
                        // A5F  -CHECK
                        // R-P
                        // A5C6 A5C6*ALV 1142/28AUG20   ********************** need this entry ***
                        // X6   P
                        // X5H  H - TA / 1BJC
                        // A6   TA / 1BJC
                        // K159 K159*ALS 1139/28AUG20   ********************** need this entry ***
                        // A9   NYC - 718 - 373 - 6500
                        // AN   1ADAMSKAYA/KARINA MRS
                        // AS   DL 737L 12OCT JFKMIA NN/SS1   720A 1023A /DCDL       /E
                        // A7   TAW /
                        // R - P
                        // K159 K159*ALS 1138/28AUG20   ********************** need this entry ***
                        // ---------------------

                        var sbH = new StringBuilder("<PNR_HDK>");
                        foreach (string line in lstLines.GetRange(1, lstLines.Count - 1))
                        {
                            var strline = line.Trim().Replace(")&gt;", "").Replace("&gt;", "");
                            if (strline.StartsWith("ADK") | strline.StartsWith("R-"))
                            {
                                continue;
                            }

                            bool isGood = Regex.IsMatch(strline, @"[a-zA-Z0-9]{4}\s[a-zA-Z0-9]{4}\*[A-Z0-9]{3}\s[0-9]{4}\/[a-zA-Z0-9]{5}");

                            // If Not String.IsNullOrEmpty(line) AndAlso Not line.Trim.StartsWith("R-") AndAlso line.Trim.Contains("*") Then
                            if (isGood)
                            {
                                var elems = strline.Split(new string[] { " ", "/", "*" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                string pcc = elems.First();
                                sbH.Append($"<Line PCC='{pcc}'>{strline}</Line>");
                            }
                        }

                        sbH.Append("</PNR_HDK>");
                        strResponse = strResponse.Replace("</TravelItineraryReadRS>", $"{sbH}</TravelItineraryReadRS>");

                        #endregion

                        if (!string.IsNullOrEmpty(strVerifyTickets))
                        {
                            XmlDocument oDocTkt;
                            XmlElement oRootTkt;
                            XmlDocument oDocPNR;
                            XmlElement oRootPNR;
                            XmlNodeList oNodesIssued;
                            string strTickets = "";
                            string strPNR;
                            oDocTkt = new XmlDocument();
                            oDocTkt.LoadXml(strVerifyTickets);
                            oRootTkt = oDocTkt.DocumentElement;
                            try
                            {
                                CoreLib.SendTrace(ProviderSystems.UserID, "DailySales", "DailySalesReportLLSRQ", "", ProviderSystems.LogUUID);
                                foreach (XmlNode oNodeTkt in oRootTkt)
                                    strTickets += ttSA.SendMessage(oNodeTkt.OuterXml, "DailySales", "DailySalesReportLLSRQ", ConversationID);
                                strTickets = strTickets.Replace("<DailySalesReportRS Version=\"2.0.0\">", "").Replace("</DailySalesReportRS>", "");
                                strTickets = "<DailySalesReportRS>" + strTickets + "</DailySalesReportRS>";
                                oDocPNR = new XmlDocument();
                                oDocPNR.LoadXml(strResponse);
                                oRootPNR = oDocPNR.DocumentElement;
                                strPNR = oRootPNR.SelectSingleNode("TravelItinerary/ItineraryRef/@ID").InnerText;
                                oDocTkt.LoadXml(strTickets);
                                oRootTkt = oDocTkt.DocumentElement;
                                oNodesIssued = oRootTkt.SelectNodes($"SalesReport/IssuanceData[@ItineraryRef='{strPNR}']");
                                if (oNodesIssued.Count > 0)
                                {
                                    int iVoidTkt = oRootTkt.SelectNodes($"SalesReport/IssuanceData[@ItineraryRef='{strPNR}'][@IndicatorOne!='']").Count;
                                    if (iVoidTkt != oNodesIssued.Count)
                                    {
                                        strResponse = strResponse.Replace("</TravelItineraryReadRS>", "<Ticketed/></TravelItineraryReadRS>");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                CoreLib.SendTrace(ProviderSystems.UserID, "DailySalesReportRS", string.Format("ERROR:{0}", ex.Message), strResponse.Substring(0, (int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                            }
                        }

                        // ==========================================================================================
                        // oRootNative don't have attribute "CheckIssuedTicket", the problem is in Sabre_QueueRead.xsl
                        // ==========================================================================================
                        CoreLib.SendTrace(ProviderSystems.UserID, "QueueRead", "Original Request", Request, ProviderSystems.LogUUID);
                        var oDocNative2 = new XmlDocument();
                        oDocNative2.LoadXml(Request);
                        var oRootNative2 = oDocNative2.DocumentElement;
                        // ==========================================================================================

                        if (oRootNative2.Attributes["CheckIssuedTicket"] != null)
                        {
                            if (oRootNative2.Attributes["CheckIssuedTicket"].Value == "true")
                            {
                                dqbResponse = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>DQB*</HostCommand></Request></SabreCommandLLSRQ>";
                                CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "DQB*", "", ProviderSystems.LogUUID);
                                dqbResponse = ttSA.SendMessage(dqbResponse, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                                strResponse = strResponse.Replace("</TravelItineraryReadRS>", $"{dqbResponse}</TravelItineraryReadRS>");
                            }
                            else
                            {
                                CoreLib.SendTrace(ProviderSystems.UserID, "CheckIssuedTicket", "false", "Set not to true", ProviderSystems.LogUUID);
                            }
                        }
                        else
                        {
                            CoreLib.SendTrace(ProviderSystems.UserID, "CheckIssuedTicket", "false", "", ProviderSystems.LogUUID);
                        }

                        // Transform PNR Read
                        if (Version != "v04_")
                        {
                            Version = "v03";
                        }

                        inSession = strResponse.Contains("<Errors>") ? false : true;

                        var strToReplace = "</TravelItineraryReadRS>";
                        if (inSession)
                            strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{strToReplace}");

                        if (strResponse.Length > 1500)
                        {
                            CoreLib.SendTrace(ProviderSystems.UserID, "QRead", "Final response I", strResponse.Substring(0, (int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                            CoreLib.SendTrace(ProviderSystems.UserID, "QRead", "Final response II", strResponse.Substring((int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                        }
                        else
                        {
                            CoreLib.SendTrace(ProviderSystems.UserID, "QRead", "Final response I", strResponse, ProviderSystems.LogUUID);
                        }

                        strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_PNRReadRS.xsl");


                    }
                    else
                    {
                        switch (strMessage ?? "")
                        {
                            case "AccessQueue":
                                {
                                    inSession = false;
                                    strResponse = CoreLib.GetNodeInnerText(strResponse, "Message", false);
                                    if (string.IsNullOrEmpty(strResponse))
                                    {
                                        strResponse = "Cannot read PNR from queue";
                                    }

                                    strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, ProviderSystems);
                                    break;
                                }

                            case "ExitQueue":
                                {
                                    strResponse = CoreLib.GetNodeInnerText(strResponse, "Response", false);
                                    if (strResponse.Contains("Q/TTL"))
                                    {
                                        inSession = false;
                                    }

                                    strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, ProviderSystems);
                                    if (!string.IsNullOrEmpty(ConversationID) & !string.IsNullOrEmpty(ConversationID))
                                    {
                                        inSession = false;
                                    }

                                    break;
                                }

                            case "ItemOnQueue":
                                {
                                    strResponse = strResponse.Replace("В?", "");
                                    strResponse = CoreLib.GetNodeInnerText(strResponse, "Response", false);
                                    if (strResponse.Contains("IGNORED - OFF QUEUE"))
                                    {
                                        strResponse = $"<OTA_TravelItineraryRS Version=\"1.000\"><Success/><Warnings><Warning Type=\"Queue\">{strResponse}</Warning></Warnings></OTA_TravelItineraryRS>";
                                        inSession = false;
                                    }
                                    else
                                    {
                                        strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, ProviderSystems);
                                        strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"<ConversationID>{ConversationID}</ConversationID></OTA_TravelItineraryRS>");
                                    }

                                    break;
                                }
                        }
                    }

                    CoreLib.SendTrace(ProviderSystems.UserID, "QRead", "Final response size", strResponse.Length.ToString(), ProviderSystems.LogUUID);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttSA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, ex.Message, ProviderSystems);
            }

            return strResponse;

        }

        private string GetPassangerInfo(string strPQS, string strPQ)
        {
            string strPassengers = "";
            try
            {

                //string strPQ = oRoot.SelectSingleNode($"StoredFare[position()={i}]/@RPH").InnerText;
                CoreLib.SendTrace(ProviderSystems.UserID, "strPQ", "stored fare number", strPQ, ProviderSystems.LogUUID);

                foreach (string line in strPQS.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    CoreLib.SendTrace(ProviderSystems.UserID, "strPQ", "line.Substring(9,1)", line.Substring(9, 1), ProviderSystems.LogUUID);
                    if ((strPQ ?? "") == (line.Substring(9, 1) ?? ""))
                    {
                        strPassengers += $"<NameSelect NameNumber=\"{line.Substring(1, 3)}\"/>";
                        CoreLib.SendTrace(ProviderSystems.UserID, "strPQ", "{line.Substring(2, 3)}", line.Substring(1, 3), ProviderSystems.LogUUID);
                    }
                }

                CoreLib.SendTrace(ProviderSystems.UserID, "strPQ", "strPassengers", strPassengers, ProviderSystems.LogUUID);
            }
            catch (Exception ex)
            {
                strPassengers = ex.Message;
            }
            return strPassengers;
        }

        private string GetPassangerInfo(string strPQS, List<string> strPQ)
        {
            string strPassengers = "";
            try
            {

                //string strPQ = oRoot.SelectSingleNode($"StoredFare[position()={i}]/@RPH").InnerText;
                CoreLib.SendTrace(ProviderSystems.UserID, "strPQ", "stored fare number", string.Join(",", strPQ), ProviderSystems.LogUUID);

                foreach (string line in strPQS.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    CoreLib.SendTrace(ProviderSystems.UserID, "strPQ", "line.Substring(9,1)", line.Substring(9, 1), ProviderSystems.LogUUID);

                    if (strPQ.Exists(l => line.Contains(l)))
                    {
                        strPassengers += $"<NameSelect NameNumber=\"{line.Substring(1, 3)}\"/>";
                        CoreLib.SendTrace(ProviderSystems.UserID, "strPQ", "{line.Substring(2, 3)}", line.Substring(1, 3), ProviderSystems.LogUUID);
                    }
                    else
                    {
                        //It happandes when PQS has CNN but Stylesheet has C09
                        if (strPQ.First().Equals("C09") && line.Contains("CNN"))
                        {
                            CoreLib.SendTrace(ProviderSystems.UserID, "strPQ", "line.Substring(9,1)", line.Substring(9, 1), ProviderSystems.LogUUID);
                            strPassengers += $"<NameSelect NameNumber=\"{line.Substring(1, 3)}\"/>";
                            CoreLib.SendTrace(ProviderSystems.UserID, "strPQ", "{line.Substring(2, 3)}", line.Substring(1, 3), ProviderSystems.LogUUID);
                        }
                    }

                }

                CoreLib.SendTrace(ProviderSystems.UserID, "strPQ", "strPassengers", strPassengers, ProviderSystems.LogUUID);
            }
            catch (Exception ex)
            {
                strPassengers = ex.Message;
            }
            return strPassengers;
        }

        private bool IsMultiplePrice(string priceXML)
        {
            try
            {
                var lstElems = priceXML.Split(new[] { "<OTA_AirPriceRQ", "</OTA_AirPriceRQ>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                return lstElems.Count > 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string GetPQS(SabreAdapter ttSA, string cmd)
        {
            try
            {
                var strResponse = ttSA.SendMessage(cmd, "SabreCommandLLSRQ", "SabreCommandLLSRQ", ConversationID);
                strResponse = strResponse.Replace("<![CDATA[", "").Replace("]]>", "").Replace(" ", "*").Replace("</Response></SabreCommandLLSRS>", "");
                var strPQS = strResponse.Substring(strResponse.IndexOf("1.1", StringComparison.Ordinal) - 2);
                CoreLib.SendTrace(ProviderSystems.UserID, "strPQS", "strPQS", strPQS, ProviderSystems.LogUUID);
                return strPQS;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}