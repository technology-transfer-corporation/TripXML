using System;
using System.Threading;
using System.Xml;
using TripXMLMain;

namespace Sabre
{
    public class TravelServices : SabreBase
    {
        public string Warnings { get; set; }

        public string Errors { get; set; }

        public string Message { get; set; }

        private string strNative = "";

        public string TravelBuild()
        {
            string strResponse;

            // ****************************************************************
            // Transform OTA TravelBuild Request into Several Navite Request *
            // ****************************************************************  

            try
            {
                string strEchoToken = "";
                var nodesOther = default(XmlNodeList);
                XmlNamespaceManager nsmgr = null;
                bool MiscSegmentSell;
                string strMiscSegmentSell = "";
                bool SpecialRemark;
                string srtSpecialRemark = "";
                var nodesSpecialRemark = default(XmlNodeList);
                string RecordLocator = "";
                double dTime;
                XmlNode oNodeConfirm = null;

                string strRequest = SetRequest("Sabre_TravelBuildRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool Air;
                bool Car;
                bool Hotel;
                bool Price;
                bool Other;
                bool SpecialCI;
                bool SpecialSeat;
                bool SpecialSSR;
                bool SpecialOSI;
                bool Queue;

                string strAddInfo = "";
                string strSpecialServicesCI = "";
                string strSpecialServicesSeat = "";
                string strSpecialServicesOSI = "";
                string strAir = "";
                string strCars = "";
                string strHotels = "";
                string strPricing = "";
                string strEndTransaction = "";
                string strRead = "";
                string strIgnore = "";
                string strQueue = "";
                XmlElement oRoot;
                XmlDocument oDoc;
                // ********************
                // Get All Requests  * 
                // ********************
                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;
                if (oRoot.SelectSingleNode("AddInfo") is null)
                {
                    throw new Exception("Request is missing mandatory elements.");
                }
                else
                {
                    strAddInfo = oRoot.SelectSingleNode("AddInfo").InnerXml.Replace(" xmlns=\"\"", "");
                }

                if (oRoot.SelectSingleNode("AirBook") is null)
                {
                    Air = false;
                }
                else
                {
                    strAir = oRoot.SelectSingleNode("AirBook").InnerXml.Replace(" xmlns=\"\"", "");

                    Air = strAir.Contains("FlightSegment")
                        ? !string.IsNullOrEmpty(strAir)
                        : false;
                }

                if (oRoot.SelectSingleNode("CarBook") is null)
                {
                    Car = false;
                }
                else
                {
                    strCars = oRoot.SelectSingleNode("CarBook").InnerXml.Replace(" xmlns=\"\"", "");
                    Car = !string.IsNullOrEmpty(strCars);
                }

                if (oRoot.SelectSingleNode("HotelBook") is null)
                {
                    Hotel = false;
                }
                else
                {
                    strHotels = oRoot.SelectSingleNode("HotelBook").InnerXml.Replace(" xmlns=\"\"", "");
                    Hotel = !string.IsNullOrEmpty(strHotels);
                }

                if (oRoot.SelectSingleNode("Pricing") is null)
                {
                    Price = false;
                }
                else
                {
                    strPricing = oRoot.SelectSingleNode("Pricing").InnerXml.Replace(" xmlns=\"\"", "");
                    Price = !string.IsNullOrEmpty(strPricing);
                }

                if (oRoot.SelectSingleNode("SpecialRemarks") is null)
                {
                    SpecialRemark = false;
                }
                else
                {
                    srtSpecialRemark = oRoot.SelectSingleNode("SpecialRemarks").InnerXml.Replace(" xmlns=\"\"", "");
                    SpecialRemark = !string.IsNullOrEmpty(srtSpecialRemark);
                    nsmgr = new XmlNamespaceManager(oDoc.NameTable);
                    nsmgr.AddNamespace("sx", "http://webservices.sabre.com/sabreXML/2003/07");
                    nodesSpecialRemark = oRoot.SelectSingleNode("SpecialRemarks").SelectNodes("sx:AddRemarkRQ", nsmgr);
                }

                if (oRoot.SelectSingleNode("Remarks") is null)
                {
                    Other = false;
                }
                else
                {
                    string strOther = oRoot.SelectSingleNode("Remarks").InnerXml.Replace(" xmlns=\"\"", "");
                    Other = !string.IsNullOrEmpty(strOther);
                    nsmgr = new XmlNamespaceManager(oDoc.NameTable);
                    nsmgr.AddNamespace("sx", "http://webservices.sabre.com/sabreXML/2003/07");
                    nodesOther = oRoot.SelectSingleNode("Remarks").SelectNodes("sx:AddRemarkRQ", nsmgr);
                }

                if (oRoot.SelectSingleNode("SpecialServicesCI") is null)
                {
                    SpecialCI = false;
                }
                else
                {
                    strSpecialServicesCI = oRoot.SelectSingleNode("SpecialServicesCI").InnerXml.Replace(" xmlns=\"\"", "");
                    SpecialCI = !string.IsNullOrEmpty(strSpecialServicesCI);
                }

                if (oRoot.SelectSingleNode("SpecialServicesSeat") is null)
                {
                    SpecialSeat = false;
                }
                else
                {
                    strSpecialServicesSeat = oRoot.SelectSingleNode("SpecialServicesSeat").InnerXml.Replace(" xmlns=\"\"", "");
                    SpecialSeat = !string.IsNullOrEmpty(strSpecialServicesSeat);
                }

                if (oRoot.SelectSingleNode("SpecialServicesSSR") is null)
                {
                    SpecialSSR = false;
                }
                else
                {
                    SpecialSSR = true;
                }

                SpecialSSR = oRoot.SelectSingleNode("SpecialServicesSSR") == null
                    ? false : true;

                if (oRoot.SelectSingleNode("SpecialServicesOSI") is null)
                {
                    SpecialOSI = false;
                }
                else
                {
                    strSpecialServicesOSI = oRoot.SelectSingleNode("SpecialServicesOSI").InnerXml.Replace(" xmlns=\"\"", "");
                    SpecialOSI = !string.IsNullOrEmpty(strSpecialServicesOSI);
                }

                if (oRoot.SelectSingleNode("Queue") is null)
                {
                    Queue = false;
                }
                else
                {
                    strQueue = oRoot.SelectSingleNode("Queue").InnerXml.Replace(" xmlns=\"\"", "");
                    Queue = !string.IsNullOrEmpty(strQueue);
                }

                if (oRoot.SelectSingleNode("MiscellaneousSegments") is null)
                {
                    MiscSegmentSell = false;
                }
                else
                {
                    strMiscSegmentSell = oRoot.SelectSingleNode("MiscellaneousSegments").InnerXml.Replace(" xmlns=\"\"", "");
                    MiscSegmentSell = true;
                }

                strEndTransaction = oRoot.SelectSingleNode("ET").InnerXml.Replace(" xmlns=\"\"", "");
                strRead = oRoot.SelectSingleNode("Read").InnerXml.Replace(" xmlns=\"\"", "");
                strIgnore = oRoot.SelectSingleNode("Ignore").InnerXml.Replace(" xmlns=\"\"", "");


                // *******************
                // Create Session    *
                // *******************
                bool inSession = SetConversationID(ttSA);
                strResponse = ttSA.SendMessage(strIgnore, "IgnoreTransaction", "IgnoreTransactionLLSRQ", ConversationID);

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 

                try
                {
                    // Send Mandatory elements
                    strResponse = SendRequestSegment(ttSA, strAddInfo, "MultiElements", "TravelItineraryAddInfo", "TravelItineraryAddInfoLLSRQ");

                    // Fatal Error
                    if (strResponse.Trim().Length > 0)
                    {
                        strResponse = BuildOTAResponse(strResponse);
                        return strResponse;
                    }

                    // Send Air elements
                    if (Air)
                    {
                        strResponse = SendRequestSegment(ttSA, strAir, "Air", "AirBook", "OTA_AirBookLLSRQ");
                        // Fatal Error
                        if (strResponse.Trim().Length > 0)
                        {
                            strResponse = BuildOTAResponse(strResponse);
                            return strResponse;
                        }
                    }

                    Thread.Sleep(1000);

                    // Send Cars Request
                    if (Car)
                    {
                        strResponse = SendRequestSegment(ttSA, strCars, "Cars", "Sell Vehicle", "OTA_VehResLLSRQ");
                        if (strResponse.Length > 0 & !(Air | Hotel))
                        {
                            strResponse = BuildOTAResponse(strResponse);
                            return strResponse;
                        }
                    }

                    // Send Hotels Request
                    if (Hotel)
                    {
                        strResponse = SendRequestSegment(ttSA, strHotels, "Hotel", "Sell Hotel", "OTA_HotelResLLSRQ");
                        if (strResponse.Length > 0 & !(Air | Car))
                        {
                            strResponse = BuildOTAResponse(strResponse);
                            return strResponse;
                        }
                    }

                    // Send Remarks Request
                    if (Other)
                    {
                        int brCount = 0;
                        foreach (XmlNode otherNode in nodesOther)
                        {
                            if (otherNode.SelectNodes("sx:BasicRemark", nsmgr).Count > 0)
                            {
                                if (otherNode.SelectNodes("sx:BasicRemark", nsmgr).Count > 10)
                                {
                                    brCount = brCount + 1;
                                    string rmkReq = "";
                                    for (int index = 1, loopTo = otherNode.SelectNodes("sx:BasicRemark", nsmgr).Count; index <= loopTo; index++)
                                    {
                                        if (index % 10 == 1)
                                        {
                                            rmkReq = "<AddRemarkRQ Version=\"2003A.TsabreXML1.0.1\" xmlns=\"http://webservices.sabre.com/sabreXML/2003/07\">";
                                            rmkReq = rmkReq + otherNode.SelectSingleNode("sx:POS", nsmgr).OuterXml;
                                        }

                                        rmkReq = rmkReq + otherNode.SelectNodes("sx:BasicRemark", nsmgr)[index - 1].OuterXml;
                                        if (index % 10 == 0 | index == otherNode.SelectNodes("sx:BasicRemark", nsmgr).Count)
                                        {
                                            rmkReq = rmkReq + "</AddRemarkRQ>";
                                            strResponse = SendRequestSegment(ttSA, rmkReq, "Remarks", "AddRemark", "AddRemarkLLSRQ");
                                        }
                                    }
                                }
                                else
                                {
                                    string argstrRequest = otherNode.OuterXml;
                                    strResponse = SendRequestSegment(ttSA, argstrRequest, "Remarks", "AddRemark", "AddRemarkLLSRQ");
                                    //otherNode.OuterXml = argstrRequest;
                                }
                            }
                            else
                            {
                                string argstrRequest1 = otherNode.OuterXml;
                                strResponse = SendRequestSegment(ttSA, argstrRequest1, "Remarks", "AddRemark", "AddRemarkLLSRQ");
                                //otherNode.OuterXml = argstrRequest1;
                            }
                        }
                    }

                    // Send Special Remarks Request
                    if (SpecialRemark)
                    {
                        int brCount = 0;
                        foreach (XmlNode SpecialRemarkNode in nodesSpecialRemark)
                        {
                            if (SpecialRemarkNode.SelectNodes("sx:HistoricalRemark", nsmgr).Count > 0)
                            {
                                if (SpecialRemarkNode.SelectNodes("sx:HistoricalRemark", nsmgr).Count > 10)
                                {
                                    brCount = brCount + 1;
                                    string rmkReq = "";
                                    for (int index = 1, loopTo1 = SpecialRemarkNode.SelectNodes("sx:HistoricalRemark", nsmgr).Count; index <= loopTo1; index++)
                                    {
                                        if (index % 10 == 1)
                                        {
                                            rmkReq = "<AddRemarkRQ Version=\"2003A.TsabreXML1.0.1\" xmlns=\"http://webservices.sabre.com/sabreXML/2003/07\">";
                                            rmkReq = rmkReq + SpecialRemarkNode.SelectSingleNode("sx:POS", nsmgr).OuterXml;
                                        }

                                        rmkReq = rmkReq + SpecialRemarkNode.SelectNodes("sx:HistoricalRemark", nsmgr)[index - 1].OuterXml;
                                        if (index % 10 == 0 | index == SpecialRemarkNode.SelectNodes("sx:HistoricalRemark", nsmgr).Count)
                                        {
                                            rmkReq = rmkReq + "</AddRemarkRQ>";
                                            strResponse = SendRequestSegment(ttSA, rmkReq, "Special Remarks", "AddRemark", "AddRemarkLLSRQ");
                                        }
                                    }
                                }
                                else
                                {
                                    string argstrRequest2 = SpecialRemarkNode.OuterXml;
                                    strResponse = SendRequestSegment(ttSA, argstrRequest2, "Special Remarks", "AddRemark", "AddRemarkLLSRQ");
                                    //SpecialRemarkNode.OuterXml = argstrRequest2;
                                }
                            }
                            else
                            {
                                string argstrRequest3 = SpecialRemarkNode.OuterXml;
                                strResponse = SendRequestSegment(ttSA, argstrRequest3, "Special Remarks", "AddRemark", "AddRemarkLLSRQ");
                                //SpecialRemarkNode.OuterXml = argstrRequest3;
                            }
                        }
                    }

                    // Send Special Services Requests
                    if (SpecialCI)
                    {
                        strResponse = SendRequestSegment(ttSA, strSpecialServicesCI, "SpecialServicesCI", "SpecialService", "SpecialServiceLLSRQ");
                    }

                    if (SpecialSeat)
                    {
                        strResponse = SendRequestSegment(ttSA, strSpecialServicesSeat, "SpecialServicesSeat", "SpecialService", "SpecialServiceLLSRQ");
                    }

                    if (SpecialSSR)
                    {
                        var oNodes = oRoot.SelectNodes("SpecialServicesSSR");
                        foreach (XmlNode currentONode in oNodes)
                        {
                            var oNode = currentONode;
                            string strSpecialServicesSSR = oNode.InnerXml.Replace(" xmlns=\"\"", "");
                            strResponse = SendRequestSegment(ttSA, strSpecialServicesSSR, "SpecialServicesSSR", "SpecialService", "SpecialServiceLLSRQ");
                        }
                    }

                    if (SpecialOSI)
                    {
                        strResponse = SendRequestSegment(ttSA, strSpecialServicesOSI, "SpecialServicesOSI", "SpecialService", "SpecialServiceLLSRQ");
                    }

                    if (MiscSegmentSell)
                    {
                        strResponse = SendRequestSegment(ttSA, strMiscSegmentSell, "MiscSegmentSell", "MiscSegmentSell", "MiscSegmentSellLLSRQ");
                    }

                    // Send Pricing Request
                    if (Price)
                    {
                        strResponse = SendRequestSegment(ttSA, strPricing, "Price", "Air Price", "OTA_AirPriceLLSRQ");

                        // Fatal Error
                        if (strResponse.Trim().Length > 0)
                        {
                            strResponse = BuildOTAResponse(strResponse);
                            return strResponse;
                        }
                    }

                    // Send End Transaction Request
                    CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "ET", "", ProviderSystems.LogUUID);
                    strResponse = ttSA.SendMessage(strEndTransaction, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                    strNative += $"{strEndTransaction}{strResponse}";

                    // do we need a second end transact?
                    if (strResponse.Contains("PREVIOUS ENTRY"))
                    {
                        //dTime = DateAndTime.Timer;
                        //while (DateAndTime.Timer - dTime <= 1d)
                        //{
                        //}
                        Thread.Sleep(1000);

                        CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "ET", "", ProviderSystems.LogUUID);
                        strResponse = ttSA.SendMessage(strEndTransaction, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                        strNative += $"{strEndTransaction}{strResponse}";
                    }

                    string cryptic = "";
                    if (strResponse.Contains("*IM AND CANCEL UNABLE SEGMENTS"))
                    {
                        cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*IM</HostCommand></Request></SabreCommandLLSRQ>";
                        CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "*IM", "", ProviderSystems.LogUUID);
                        strResponse = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                        strNative += $"{cryptic}{strResponse}";
                        string curDate = DateTime.Now.ToShortDateString();
                        strResponse = strResponse.Replace("</SabreCommandLLSRS>", $"<CurrentDate>{curDate}</CurrentDate></SabreCommandLLSRS>");

                        // create fomatted error with flights that failed
                        CoreLib.SendTrace(ProviderSystems.UserID, "FailedFlights", "*IM", strResponse, ProviderSystems.LogUUID);
                    }
                    else
                    {
                        // do we need a second end transact?
                        if (strResponse.Contains("DIRECT CONNECT MESSAGES RECEIVED - ENTER *A OR *IM"))
                        {
                            cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*IM</HostCommand></Request></SabreCommandLLSRQ>";
                            CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "*IM", "", ProviderSystems.LogUUID);
                            strResponse = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                            strNative += $"{cryptic}{strResponse}";

                            string curDate = DateTime.Now.ToShortDateString();
                            strResponse = strResponse.Replace("</SabreCommandLLSRS>", $"<CurrentDate>{curDate}</CurrentDate></SabreCommandLLSRS>");
                            // Check for Errors
                            strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_TB_Errors.xsl");

                            if (!string.IsNullOrEmpty(strResponse) & strResponse.Contains("Flight "))
                            {
                                if (strResponse.Contains("<Error"))
                                {
                                    // Fatal Error
                                    return BuildOTAResponse(strResponse);
                                }
                                else if (strResponse.Contains("<Warning>"))
                                {
                                    Warnings += strResponse;
                                }
                            }

                            CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "ET", "", ProviderSystems.LogUUID);
                            strResponse = ttSA.SendMessage(strEndTransaction, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                            strNative += $"{strEndTransaction}{strResponse}";
                        }

                        if (strResponse.Contains("VERIFY ORDER OF ITINERARY SEGMENTS - MODIFY OR END TRANSACTION"))
                        {
                            cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>0AA</HostCommand></Request></SabreCommandLLSRQ>";
                            CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "0AA", "", ProviderSystems.LogUUID);
                            strResponse = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                            strNative += $"{cryptic}{strResponse}";
                            CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "ET", "", ProviderSystems.LogUUID);
                            strResponse = ttSA.SendMessage(strEndTransaction, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                            strNative += $"{strEndTransaction}{strResponse}";
                        }

                        if (strResponse.Contains("MODIFY OR END TRANSACTION"))
                        {
                            CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "ET", "", ProviderSystems.LogUUID);
                            strResponse = ttSA.SendMessage(strEndTransaction, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                            strNative += $"{strEndTransaction}{strResponse}";
                        }
                    }

                    // Check for Errors
                    strEndTransaction = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_TB_Errors.xsl");

                    if (strEndTransaction.Length > 0)
                    {
                        if (strEndTransaction.Contains("<Error"))
                        {
                            // Fatal Error
                            return BuildOTAResponse(strEndTransaction);
                        }
                        else if (strEndTransaction.Contains("<Warning>"))
                        {
                            Warnings += strEndTransaction;
                        }
                    }

                    // Retrieve the PNR
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;
                    if (oRoot.SelectSingleNode("UniqueID/@ID") != null)
                    {
                        if (Queue)
                        {
                            CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "Queue PNR", "", ProviderSystems.LogUUID);
                            strQueue = strQueue.Replace("UniqueID", $"UniqueID ID=\"{oRoot.SelectSingleNode("UniqueID/@ID").InnerText}\"");
                            strResponse = ttSA.SendMessage(strQueue, "QueuePlace", "QPlaceLLSRQ", ConversationID);
                        }

                        CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "Retreive PNR", "", ProviderSystems.LogUUID);
                        // strRead = strRead.Replace("UniqueID", sb.Append("UniqueID ID=""").Append(oRoot.SelectSingleNode("UniqueID/@ID").InnerText).Append("""").ToString())
                        strRead = "<TravelItineraryReadRQ Version=\"3.6.0\" xmlns=\"http://services.sabre.com/res/tir/v3_6\"><MessagingDetails><SubjectAreas>" +
                            "<SubjectArea>FULL</SubjectArea></SubjectAreas></MessagingDetails><UniqueID ID=\"" + oRoot.SelectSingleNode("UniqueID/@ID").InnerText + "\"/></TravelItineraryReadRQ>";

                        strResponse = ttSA.SendMessage(strRead, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                        cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*PQS</HostCommand></Request></SabreCommandLLSRQ>";
                        CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "*PQS", "", ProviderSystems.LogUUID);
                        cryptic = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                        strResponse = strResponse.Replace("</TravelItineraryReadRS>", $"{cryptic}</TravelItineraryReadRS>");
                        strResponse = strResponse.Replace(" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\"", "").Replace(" Version=\"2.0.0\"", "");

                        oDoc.LoadXml(strResponse);
                        oRoot = oDoc.DocumentElement;
                        if (oRoot.SelectSingleNode("TravelItinerary") != null)
                        {
                            oNodeConfirm = oRoot.SelectSingleNode("TravelItinerary/ItineraryRef");
                            if (oNodeConfirm != null)
                            {
                                RecordLocator = oNodeConfirm.Attributes["ID"].Value;
                                if (string.IsNullOrEmpty(RecordLocator))
                                {
                                    // Send Retreive Request
                                    if (Air)
                                    {
                                        Thread.Sleep(3000);
                                    }

                                    strRead = "<TravelItineraryReadRQ Version=\"3.6.0\" xmlns=\"http://services.sabre.com/res/tir/v3_6\"><MessagingDetails><SubjectAreas>" +
                                        "<SubjectArea>FULL</SubjectArea></SubjectAreas></MessagingDetails><UniqueID ID=\"" + oRoot.SelectSingleNode("UniqueID/@ID").InnerText + "\"/></TravelItineraryReadRQ>";

                                    strResponse = ttSA.SendMessage(strRead, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                                    cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*PQS</HostCommand></Request></SabreCommandLLSRQ>";
                                    CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "*PQS", "", ProviderSystems.LogUUID);
                                    cryptic = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);

                                    strResponse = strResponse.Replace("</TravelItineraryReadRS>", $"{cryptic}</TravelItineraryReadRS>");
                                }
                            }
                        }
                    }

                    // ****************************************************************************
                    // Add Previous Errors and Warnings To Sabre Native End Transact Response  *
                    // ****************************************************************************
                    if (Request.Contains("EchoToken"))
                    {
                        XmlDocument oDocReq = null;
                        XmlElement oRootReq = null;
                        oDocReq = new XmlDocument();
                        oDocReq.LoadXml(Request);
                        oRootReq = oDocReq.DocumentElement;
                        strEchoToken = $"<EchoToken>{oRootReq.Attributes.GetNamedItem("EchoToken").Value}</EchoToken>";

                        oDocReq = null;
                        oRootReq = null;
                    }

                    strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"{Errors}{Warnings}{strEchoToken}</OTA_TravelItineraryRS>");

                    // *****************************************************************
                    // Transform Native Sabre TravelBuild Response into OTA Response   *
                    // ***************************************************************** 
                    var strToReplace = "</Sabre_PNRReadRS>";

                    CoreLib.SendTrace(ProviderSystems.UserID, "TravelBuild", "Final response", strResponse, ProviderSystems.LogUUID);
                    strResponse = strResponse.Replace(" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\"", "").Replace(" Version=\"2.0.0\"", "");
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_PNRReadRS.xsl");

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{ strToReplace}");

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
                        ConversationID = null;
                        ttSA = null;

                        CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "Clean up working area", "", ProviderSystems.LogUUID);
                        //strIgnore = ttSA.SendMessage(strIgnore, "IgnoreTransaction", "IgnoreTransactionLLSRQ", ConversationID);
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.TravelBuild, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string TravelBuild_V4()
        {
            string strRequest = "";
            string strResponse = "";
            string strAddInfo = "";
            string strSpecialServices = "";
            string strOther = "";
            string strAir = "";
            string strCars = "";
            string strHotels = "";
            string strPricing = "";
            string strEndTransaction = "";
            string strRead = "";
            string strIgnore = "";
            string strQueue = "";
            string cryptic = "";
            bool Air;
            bool Car;
            bool Hotel;
            bool Price;
            bool Other;
            bool Special;
            bool Queue;
            string strEnhanced_AirBookRQ = "";
            string strWarnings = string.Empty;
            string strErrors = string.Empty;
            try
            {
                strRequest = SetRequest("Sabre_TravelBuildRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);
                // ****************************************************************
                // Transform OTA TravelBuild Request into Several Navite Request *
                // ****************************************************************  
                try
                {
                    // ********************
                    // Get All Requests  * 
                    // ********************
                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(strRequest);
                    var oRoot = oDoc.DocumentElement;
                    var oDoc2 = new XmlDocument();
                    oDoc2.LoadXml(Request);
                    var oRoot2 = oDoc2.DocumentElement;

                    if (oRoot.SelectSingleNode("AddInfo") is null)
                    {
                        throw new Exception("Request is missing mandatory elements.");
                    }
                    else
                    {
                        strAddInfo = oRoot.SelectSingleNode("AddInfo").InnerXml.Replace(" xmlns=\"\"", "");
                    }

                    string strPassengerDetails = "";
                    if (oRoot2.SelectSingleNode("PassengerDetails") is null)
                    {
                        throw new Exception("Request is missing mandatory elements.");
                    }
                    else
                    {
                        strPassengerDetails = oRoot2.SelectSingleNode("PassengerDetails").InnerXml.Replace(" xmlns=\"\"", "");
                    }

                    if (oRoot2.SelectSingleNode("Enhanced_AirBook") is null)
                    {
                        Air = false;
                    }
                    else
                    {
                        strEnhanced_AirBookRQ = oRoot2.SelectSingleNode("Enhanced_AirBook").InnerXml.Replace(" xmlns=\"\"", "");
                        Air = strAir.Length > 0;
                    }

                    if (oRoot.SelectSingleNode("AirBook") is null)
                    {
                        Air = false;
                    }
                    else
                    {
                        strAir = oRoot.SelectSingleNode("AirBook").InnerXml.Replace(" xmlns=\"\"", "");
                        Air = strAir.Length > 0;
                    }

                    if (oRoot.SelectSingleNode("CarBook") is null)
                    {
                        Car = false;
                    }
                    else
                    {
                        strCars = oRoot.SelectSingleNode("CarBook").InnerXml.Replace(" xmlns=\"\"", "");
                        Car = strCars.Length > 0;
                    }

                    if (oRoot.SelectSingleNode("HotelBook") is null)
                    {
                        Hotel = false;
                    }
                    else
                    {
                        strHotels = oRoot.SelectSingleNode("HotelBook").InnerXml.Replace(" xmlns=\"\"", "");
                        Hotel = strHotels.Length > 0;
                    }

                    if (oRoot.SelectSingleNode("Pricing") is null)
                    {
                        Price = false;
                    }
                    else
                    {
                        strPricing = oRoot.SelectSingleNode("Pricing").InnerXml.Replace(" xmlns=\"\"", "");
                        Price = strPricing.Length > 0;
                    }

                    if (oRoot.SelectSingleNode("Remarks") is null)
                    {
                        Other = false;
                    }
                    else
                    {
                        strOther = oRoot.SelectSingleNode("Remarks").InnerXml.Replace(" xmlns=\"\"", "");
                        Other = strOther.Length > 0;
                    }

                    if (oRoot.SelectSingleNode("SpecialServices") is null)
                    {
                        Special = false;
                    }
                    else
                    {
                        strSpecialServices = oRoot.SelectSingleNode("SpecialServices").InnerXml.Replace(" xmlns=\"\"", "");
                        Special = strSpecialServices.Length > 0;
                    }

                    if (oRoot.SelectSingleNode("Queue") is null)
                    {
                        Queue = false;
                    }
                    else
                    {
                        strQueue = oRoot.SelectSingleNode("Queue").InnerXml.Replace(" xmlns=\"\"", "");
                        Queue = strQueue.Length > 0;
                    }

                    strEndTransaction = oRoot.SelectSingleNode("ET").InnerXml.Replace(" xmlns=\"\"", "");
                    strRead = oRoot.SelectSingleNode("Read").InnerXml.Replace(" xmlns=\"\"", "");
                    strIgnore = oRoot.SelectSingleNode("Ignore").InnerXml.Replace(" xmlns=\"\"", "");

                    // *******************************************************************************
                    // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                    // ******************************************************************************* 

                    // Send Mandatory elements
                    strResponse = SendRequestSegment(ttSA, strAddInfo, "MultiElements", "TravelItineraryAddInfo", "TravelItineraryAddInfoLLSRQ");

                    // Fatal Error
                    if (strResponse.Trim().Length > 0)
                    {
                        strResponse = BuildOTAResponse(strResponse);
                        return strResponse;
                    }

                    strResponse = SendRequestSegment(ttSA, strPassengerDetails, "PassengerDetails", "PassengerDetailsRQ", "PassengerDetailsRQ");
                    // Fatal Error
                    if (strResponse.Trim().Length > 0)
                    {
                        strResponse = BuildOTAResponse(strResponse);
                        return strResponse;
                    }

                    // Send Air elements
                    if (Air)
                    {
                        strResponse = SendRequestSegment(ttSA, strEnhanced_AirBookRQ, "Enhanced_AirBook", "Enhanced_AirBookRQ", "Enhanced_AirBookRQ");

                        // Fatal Error
                        if (strResponse.Trim().Length > 0)
                        {
                            strResponse = BuildOTAResponse(strResponse);
                            return strResponse;
                        }
                    }

                    // Send Cars Request
                    if (Car)
                    {
                        strResponse = SendRequestSegment(ttSA, strCars, "Cars", "Sell Vehicle", "OTA_VehResLLSRQ");
                        if (strResponse.Length > 0 & !(Air | Hotel))
                        {
                            strResponse = BuildOTAResponse(strResponse);
                            return strResponse;
                        }
                    }

                    // Send Hotels Request
                    if (Hotel)
                    {
                        strResponse = SendRequestSegment(ttSA, strHotels, "Hotel", "Sell Hotel", "OTA_HotelResLLSRQ");
                        if (strResponse.Length > 0 & !(Air | Car))
                        {
                            strResponse = BuildOTAResponse(strResponse);
                            return strResponse;
                        }
                    }

                    if (strResponse.Contains("*IM AND CANCEL UNABLE SEGMENTS"))
                    {
                        cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*IM</HostCommand></Request></SabreCommandLLSRQ>";
                        CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "*IM", "", ProviderSystems.LogUUID);
                        strResponse = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                        strNative = $"{strNative}{cryptic}{strResponse}";

                        var curDate = DateTime.Now;
                        strResponse = strResponse.Replace("</SabreCommandLLSRS>", "<CurrentDate>" + curDate + "</CurrentDate></SabreCommandLLSRS>");

                        // create fomatted error with flights that failed
                        CoreLib.SendTrace(ProviderSystems.UserID, "FailedFlights", "*IM", strResponse, ProviderSystems.LogUUID);
                    }
                    else
                    {
                        if (strResponse.Contains("DIRECT CONNECT MESSAGES RECEIVED - ENTER *A OR *IM"))
                        {
                            cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*IM</HostCommand></Request></SabreCommandLLSRQ>";
                            CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "*IM", "", ProviderSystems.LogUUID);
                            strResponse = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                            strNative = $"{strNative}{cryptic}{strResponse}";
                        }

                        if (strResponse.Contains("VERIFY ORDER OF ITINERARY SEGMENTS - MODIFY OR END TRANSACTION"))
                        {
                            cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>0AA</HostCommand></Request></SabreCommandLLSRQ>";
                            CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "0AA", "", ProviderSystems.LogUUID);
                            strResponse = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                            strNative = $"{strNative}{cryptic}{strResponse}";
                        }
                    }

                    // Retrieve the PNR
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;
                    if (oRoot.SelectSingleNode("UniqueID/@ID") != null)
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "Retreive PNR", "", ProviderSystems.LogUUID);
                        strRead = strRead.Replace("UniqueID", $"UniqueID ID=\"{oRoot.SelectSingleNode("UniqueID/@ID").InnerText}'\')");

                        strResponse = ttSA.SendMessage(strRead, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                        cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*PQS</HostCommand></Request></SabreCommandLLSRQ>";
                        CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "*PQS", "", ProviderSystems.LogUUID);
                        cryptic = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                        strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"{cryptic}</OTA_TravelItineraryRS>");
                    }

                    // ****************************************************************************
                    // Add Previous Errors and Warnings To Sabre Native End Transact Response  *
                    // ****************************************************************************
                    strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"{strErrors}{strWarnings}</OTA_TravelItineraryRS>");

                    // *****************************************************************
                    // Transform Native Sabre TravelBuild Response into OTA Response   *
                    // ***************************************************************** 
                    var strToReplace = "</Sabre_PNRReadRS>";
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_PNRReadRS.xsl");

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{ strToReplace}");

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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.TravelBuild, ex.Message, ProviderSystems);
            }
            return strResponse;
        }

        public string IssueTicket()
        {
            string strResponse;

            try
            {
                string eTicketNo;
                string strTemp = "";
                var strRequest = SetRequest("Sabre_IssueTicketRQ.xsl");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // *************************
                // Get Multiple Requests  *
                // *************************
                var oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                var oRoot = oDoc.DocumentElement;
                var strRead = oRoot.SelectSingleNode("PNRRead").InnerXml;
                var strEndTransact = oRoot.SelectSingleNode("ET").InnerXml;

                // ********************
                // Retrieve the PNR  *
                // ******************** 
                if (ProviderSystems.UserID.ToLower() == "alshamel")
                {
                    string printRQ = "<DesignatePrinterRQ Version=\"2.0.2\" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><POS><Source PseudoCityCode=\"" + ProviderSystems.PCC + "\"/></POS><Printers><Ticket CountryCode=\"AT\"/></Printers></DesignatePrinterRQ>";
                    CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "Send to printer", "", ProviderSystems.LogUUID);
                    SendRequestSegment(ttSA, printRQ, "SendToPrint", "SendToPrint", "DesignatePrinterLLSRQ"); // ttSA.SendMessage(printRQ, "SendToPrint", "DesignatePrinterLLSRQ ", ConversationID)
                    string hardCopyPrintRQ = "<DesignatePrinterRQ Version=\"2.0.2\" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><POS><Source PseudoCityCode=\"" + ProviderSystems.PCC + "\"/></POS><Printers><Hardcopy LineAddress=\"A7AA9B\"/></Printers></DesignatePrinterRQ>";
                    CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "Send to hard copy printer", "", ProviderSystems.LogUUID);
                    SendRequestSegment(ttSA, hardCopyPrintRQ, "HardCopyPrinter", "HardCopyPrinter", "DesignatePrinterLLSRQ");
                }

                strResponse = ttSA.SendMessage(strRead, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                strResponse = strResponse.Replace(" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\"", "").Replace(" Version=\"2.0.0\"", "");
                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;
                var validatingCarrier = oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ItineraryPricing/PriceQuote[not(contains(ResponseHeader/Text[1],'HISTORY'))][not(starts-with(PricedItinerary/@InputMessage,'WS'))][1]/PricedItinerary/@ValidatingCarrier").InnerText;
                var strPnrNo = oRoot.SelectSingleNode("TravelItinerary/ItineraryRef").Attributes["ID"].InnerText;

                // Check for Errors
                if (!strResponse.Contains("<CustomerInfo>"))
                {
                    throw new Exception("Cannot retrieve PNR to ticket");
                }

                try
                {
                    var strTicket = "<AirTicketRQ xmlns=\"http://webservices.sabre.com/sabreXML/2003/07\" Version=\"2003A.TsabreXML1.9.1\" ><POS><Source PseudoCityCode=\"" + ProviderSystems.PCC + "\"/></POS>" +
                                    "<NumResponses Count=\"1\"/><TicketingInfo TicketType=\"ETR\"/><OptionalQualifiers><MiscQualifiers><VendorPref Code=\"" + validatingCarrier + "\"/></MiscQualifiers></OptionalQualifiers></AirTicketRQ>";
                    strResponse = ttSA.SendMessage(strTicket, "Air", "AirTicketLLSRQ", ConversationID);
                    strEndTransact = strEndTransact.Replace("<GivenName>TRIPXML</GivenName>", $"<GivenName>{ProviderSystems.UserID}</GivenName>");
                    CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "ET", "", ProviderSystems.LogUUID);
                    strNative = ttSA.SendMessage(strEndTransact, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                    strNative = $"{strNative}{strEndTransact}{strResponse}";

                    // *****************************************************************
                    // Transform Native Sabre IssueTicket Response into OTA Response   *
                    // ***************************************************************** 
                    var readResp = ttSA.SendMessage(strRead, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                    readResp = readResp.Replace(" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\"", "").Replace(" Version=\"2.0.0\"", "");
                    readResp = CoreLib.TransformXML(readResp, XslPath, $"{Version}Sabre_PNRReadRS.xsl");
                    oDoc = new XmlDocument();
                    oDoc.LoadXml(readResp);
                    oRoot = oDoc.DocumentElement;
                    var issuedList = oRoot.SelectNodes("TravelItinerary/ItineraryInfo/TPA_Extensions/IssuedTickets/IssuedTicket") ??
                                     null;

                    oDoc = new XmlDocument();
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;

                    if (issuedList != null)
                    {
                        foreach (XmlNode oNodeResp in issuedList)
                            strTemp += oNodeResp.OuterXml;
                    }

                    var strIssueTicket = "<AirTicketRS>";
                    strIssueTicket += oRoot.SelectSingleNode("Errors") == null
                        ? $"<Success/>{oRoot.SelectSingleNode("Text").OuterXml}"
                        : $"{oRoot.SelectSingleNode("Errors").OuterXml}";

                    if (!string.IsNullOrEmpty(strTemp.Trim()))
                    {
                        strIssueTicket += $"<IssuedTickets>{strTemp}</IssuedTickets>";
                    }

                    strIssueTicket += "</AirTicketRS>";
                    strResponse = strIssueTicket;

                    var strToReplace = "</Sabre_IssueTicketRS>";

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{ strToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_IssueTicketRS.xsl");
                    strResponse = strResponse.Replace("<UniqueID ID=\"\" />", "<UniqueID ID=\"" + strPnrNo + "\" />");

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

                return strResponse;
            }
            catch (Exception exx)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.IssueTicket, exx.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string IssueTicketSessioned()
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            string strRequest;
            string strRead;
            string strRetrieve;
            string strTicket = "";
            string strInvoice = "";
            var strResponse = default(string);
            string validatingCarrier = "";
            string strPnrNo = "";
            string readResp = "";
            string eTicketNo = "";
            string strEndTransact = null;
            string strEndTransactAgain = null;
            XmlNodeList issuedList;
            string strTemp = "";
            string strIssueTicket;
            string strPrinter = "";
            string strTickets = "";
            try
            {
                strRequest = SetRequest("Sabre_IssueTicketSessionedRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // *************************
                // Get Multiple Requests  *
                // *************************
                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;
                strRead = oRoot.SelectSingleNode("PNRRead").InnerXml;
                strRetrieve = oRoot.SelectSingleNode("PNRRetrieve").InnerXml;
                if (oRoot.SelectSingleNode("DesignatePrinter") != null)
                {
                    strPrinter = oRoot.SelectSingleNode("DesignatePrinter").OuterXml;
                }

                if (oRoot.SelectSingleNode("AirTicket") != null)
                {
                    strTicket = oRoot.SelectSingleNode("AirTicket").InnerXml;
                }

                if (oRoot.SelectSingleNode("Invoice") != null)
                {
                    strInvoice = oRoot.SelectSingleNode("Invoice").InnerXml;
                }

                if (oRoot.SelectSingleNode("ET") != null)
                {
                    strEndTransact = oRoot.SelectSingleNode("ET").InnerXml;
                    strEndTransactAgain = oRoot.SelectSingleNode("ReET").InnerXml;
                }

                // ********************
                // Retrieve the PNR  *
                // ******************** 

                if (ProviderSystems.UserID.ToLower() == "alshamel")
                {
                    string printRQ = "<DesignatePrinterRQ Version=\"2.0.2\" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><POS><Source PseudoCityCode=\"" + ProviderSystems.PCC + "\"/></POS><Printers><Ticket CountryCode=\"AT\"/></Printers></DesignatePrinterRQ>";
                    CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "Send to printer", "", ProviderSystems.LogUUID);
                    strResponse = SendRequestSegment(ttSA, printRQ, "SendToPrint", "SendToPrint", "DesignatePrinterLLSRQ"); // ttSA.SendMessage(printRQ, "SendToPrint", "DesignatePrinterLLSRQ ", ConversationID)
                    string hardCopyPrintRQ = "<DesignatePrinterRQ Version=\"2.0.2\" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><POS><Source PseudoCityCode=\"" + ProviderSystems.PCC + "\"/></POS><Printers><Hardcopy LineAddress=\"A7AA9B\"/></Printers></DesignatePrinterRQ>";
                    CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "Send to hard copy printer", "", ProviderSystems.LogUUID);
                    strResponse = SendRequestSegment(ttSA, hardCopyPrintRQ, "HardCopyPrinter", "HardCopyPrinter", "DesignatePrinterLLSRQ");
                }
                else if (!string.IsNullOrEmpty(strPrinter))
                {
                    XmlDocument oDocPrinter;
                    XmlElement oRootPrinter;
                    oDocPrinter = new XmlDocument();
                    oDocPrinter.LoadXml(strPrinter);
                    oRootPrinter = oDocPrinter.DocumentElement;
                    var nsmgr = new XmlNamespaceManager(oDocPrinter.NameTable);
                    nsmgr.AddNamespace("sx", "http://webservices.sabre.com/sabreXML/2003/07");
                    foreach (XmlNode oNodePrinter in oRootPrinter.SelectNodes("sx:DesignatePrinterRQ", nsmgr))
                        strResponse = ttSA.SendMessage(oNodePrinter.OuterXml, "Air", "DesignatePrinterLLSRQ", ConversationID);

                    // Dim strER As String = "<SabreCommandLLSRQ xmlns=""http://webservices.sabre.com/sabreXML/2011/10"" Version=""2.0.0""><Request Output=""SCREEN"" MDRSubset=""AD01"" CDATA=""true""><HostCommand>PGHOLD</HostCommand></Request></SabreCommandLLSRQ>"
                    // strNative = ttSA.SendMessage(strER, "ER", "SabreCommandLLSRQ", ConversationID)

                }

                if (string.IsNullOrEmpty(strPrinter))
                {

                    strResponse = ttSA.SendMessage(strRead, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                    strResponse = strResponse.Replace(" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\"", "").Replace(" Version=\"2.0.0\"", "");
                    oDoc = new XmlDocument();
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;
                    // validatingCarrier = oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ItineraryPricing/PriceQuote[1]/PricedItinerary/@ValidatingCarrier").InnerText
                    strPnrNo = oRoot.SelectSingleNode("TravelItinerary/ItineraryRef").Attributes["ID"].InnerText;

                    // Check for Errors
                    if (!strResponse.Contains("<CustomerInfo>"))
                    {
                        throw new Exception("Cannot retrieve PNR to ticket");
                    }

                    string strER = @"<SabreCommandLLSRQ xmlns=""http://webservices.sabre.com/sabreXML/2011/10"" Version=""2.0.0""><Request Output=""SCREEN"" MDRSubset=""AD01"" CDATA=""true""><HostCommand>6P\ER</HostCommand></Request></SabreCommandLLSRQ>";
                    //strER = strER.Replace(@"6P\ER", "CC/PC");
                    //strNative = ttSA.SendMessage(strER, "CC/PC", "SabreCommandLLSRQ", ConversationID);
                    //strER = strER.Replace("CC/PC", @"6P\ER");
                    if (!string.IsNullOrEmpty(strTicket))
                    {
                        //    strNative = ttSA.SendMessage(strER, "ER", "SabreCommandLLSRQ", ConversationID);
                        //    if (strNative.Contains("SIMULTANEOUS CHANGE"))
                        //    {
                        //        throw new Exception("PNR IGNORED AND REDISPLAYED DUE TO SIMULTANEOUS CHANGE");
                        //    }

                        //    if (strNative.Contains("*WARNING EDITS*") | strNative.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strNative.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED")
                        //        | strNative.Contains("END OR IGNORE PNR") | strNative.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT") | strNative.Contains("FF MILEAGE AGREEMENT EXISTS, SEE PT"))
                        //    {
                        //        strER = strER.Replace(@"6P\ER", "ER");
                        //        strNative = ttSA.SendMessage(strER, "ER", "SabreCommandLLSRQ", ConversationID);
                        //        if (strNative.Contains("*WARNING EDITS*") | strNative.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strNative.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED")
                        //            | strNative.Contains("END OR IGNORE PNR") | strNative.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT") | strNative.Contains("FF MILEAGE AGREEMENT EXISTS, SEE PT"))
                        //        {
                        //            int iTry = 0;
                        //            while ((strNative.Contains("*WARNING EDITS*") | strNative.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strNative.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED")
                        //                | strNative.Contains("END OR IGNORE PNR") | strNative.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT") | strNative.Contains("FF MILEAGE AGREEMENT EXISTS, SEE PT")) & iTry < 8)
                        //            {
                        //                Thread.Sleep(1000);

                        //                strNative = ttSA.SendMessage(strER, "ER", "SabreCommandLLSRQ", ConversationID);
                        //                iTry += 1;
                        //            }
                        //        }

                        //        strER = strER.Replace("ER", @"6P\ER");
                        //        if (strNative.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT"))
                        //        {
                        //            strResponse = "<AirTicketRS><Errors><Error>INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT</Error></Errors><ConversationID>" +
                        //                "<![CDATA[" + ConversationID.Replace("<", "&lt;").Replace(">", "&gt;") + "]]></ConversationID></AirTicketRS>";
                        //            return strResponse;
                        //        }
                        //    }

                        // Check for Errors
                        if (strNative.Contains("UNABLE") | strNative.Contains("MIN CONNX TIME"))
                        {
                            string err = strNative.Substring(strNative.IndexOf("<Response>") + 10, strNative.IndexOf("</Response>") - (strNative.IndexOf("<Response>") + 10));
                            throw new Exception(err);
                        }

                        strTickets = ttSA.SendMessage(strTicket, "Air", "AirTicketLLSRQ", ConversationID);

                        if (strTickets.Contains("*WARNING EDITS*") | strTickets.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strTickets.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED")
                            | strTickets.Contains("END OR IGNORE PNR") | strTickets.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT") | strTickets.Contains("FF MILEAGE AGREEMENT EXISTS, SEE PT"))
                        {
                            strTickets = ttSA.SendMessage(strTicket, "Air", "AirTicketLLSRQ", ConversationID);
                            if (strTickets.Contains("*WARNING EDITS*") | strTickets.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strTickets.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED")
                                | strTickets.Contains("END OR IGNORE PNR") | strTickets.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT") | strTickets.Contains("FF MILEAGE AGREEMENT EXISTS, SEE PT"))
                            {
                                int iTry = 0;
                                while ((strTickets.Contains("*WARNING EDITS*") | strTickets.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strTickets.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED")
                                    | strTickets.Contains("END OR IGNORE PNR") | strTickets.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT") | strTickets.Contains("FF MILEAGE AGREEMENT EXISTS, SEE PT")) & iTry < 5)
                                {
                                    Thread.Sleep(1000);

                                    strTickets = ttSA.SendMessage(strTicket, "Air", "AirTicketLLSRQ", ConversationID);
                                    iTry += 1;
                                }
                            }
                        }

                        readResp = ttSA.SendMessage(strRead, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                        if (readResp.Contains("TICKETING IN PROGRESS"))
                        {
                            int iTry = 0;
                            while (readResp.Contains("TICKETING IN PROGRESS") & iTry < 5)
                            {
                                Thread.Sleep(1000);

                                readResp = ttSA.SendMessage(strRead, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                                iTry += 1;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(strInvoice))
                    {
                        strResponse = ttSA.SendMessage(strInvoice, "Air", "SabreCommandLLSRQ", ConversationID);
                        if (strResponse.Contains("*PAC TO VERIFY CORRECT NBR OF ACCTG LINES"))
                        {
                            strER = strER.Replace(@"6P\ER", "*PAC");
                            strResponse = ttSA.SendMessage(strER, "*PAC", "SabreCommandLLSRQ", ConversationID);
                            strER = strER.Replace("*PAC", @"6P\ER");
                            strResponse = ttSA.SendMessage(strInvoice, "Air", "SabreCommandLLSRQ", ConversationID);
                        }

                        if (strResponse.Contains("*WARNING EDITS*") | strResponse.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strResponse.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED")
                            | strResponse.Contains("END OR IGNORE PNR") | strResponse.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT") | strResponse.Contains("FF MILEAGE AGREEMENT EXISTS, SEE PT"))
                        {
                            strResponse = ttSA.SendMessage(strInvoice, "Air", "SabreCommandLLSRQ", ConversationID);
                            if (strResponse.Contains("*WARNING EDITS*") | strResponse.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strResponse.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED")
                                | strResponse.Contains("END OR IGNORE PNR") | strResponse.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT") | strResponse.Contains("FF MILEAGE AGREEMENT EXISTS, SEE PT"))
                            {
                                int iTry = 0;
                                while ((strResponse.Contains("*WARNING EDITS*") | strResponse.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strResponse.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED")
                                    | strResponse.Contains("END OR IGNORE PNR") | strResponse.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT") | strResponse.Contains("FF MILEAGE AGREEMENT EXISTS, SEE PT")) & iTry < 8)
                                {
                                    Thread.Sleep(1000);

                                    strResponse = ttSA.SendMessage(strInvoice, "Air", "SabreCommandLLSRQ", ConversationID);
                                    // TODO: Check for simultaneous changes - тут они игнорируются полностью

                                    iTry += 1;
                                }
                            }
                        }

                        while (strResponse.Contains("CTP EDITS IN PROGRESS....PLEASE WAIT"))
                        {
                            Thread.Sleep(1000);
                            strResponse = ttSA.SendMessage(strInvoice, "Air", "SabreCommandLLSRQ", ConversationID);
                        }

                        readResp = ttSA.SendMessage(strRead, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                    }

                    if (!string.IsNullOrEmpty(strEndTransact))
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "ET", "", ProviderSystems.LogUUID);
                        strResponse = ttSA.SendMessage(strEndTransact, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                        if (strResponse.Contains("*PAC TO VERIFY CORRECT NBR OF ACCTG LINES"))
                        {
                            strER = strER.Replace(@"6P\ER", "*PAC");
                            strResponse = ttSA.SendMessage(strER, "*PAC", "SabreCommandLLSRQ", ConversationID);
                            strResponse = ttSA.SendMessage(strEndTransactAgain, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                        }

                        if (strResponse.Contains("*WARNING EDITS*") | strResponse.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strResponse.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED")
                            | strResponse.Contains("END OR IGNORE PNR") | strResponse.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT") | strResponse.Contains("FF MILEAGE AGREEMENT EXISTS, SEE PT"))
                        {
                            strResponse = ttSA.SendMessage(strEndTransactAgain, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                            if (strResponse.Contains("*WARNING EDITS*") | strResponse.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strResponse.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED")
                                | strResponse.Contains("END OR IGNORE PNR") | strResponse.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT") | strResponse.Contains("FF MILEAGE AGREEMENT EXISTS, SEE PT"))
                            {
                                Thread.Sleep(1000);

                                strResponse = ttSA.SendMessage(strEndTransactAgain, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                            }
                        }

                        while (strResponse.Contains("CTP EDITS IN PROGRESS....PLEASE WAIT"))
                        {
                            Thread.Sleep(1000);

                            strResponse = ttSA.SendMessage(strEndTransactAgain, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                        }

                        strER = strER.Replace("*PAC", "IG");
                        strResponse = ttSA.SendMessage(strER, "IG", "SabreCommandLLSRQ", ConversationID);
                    }
                }
                // *****************************************************************
                // Transform Native Sabre IssueTicket Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    if (string.IsNullOrEmpty(strPrinter))
                    {
                        if (!string.IsNullOrEmpty(strEndTransact))
                        {
                            readResp = ttSA.SendMessage(strRetrieve, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                        }
                        else
                        {
                            // readResp = ttSA.SendMessage(strRead, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID)
                        }

                        readResp = readResp.Replace(" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\"", "").Replace(" Version=\"2.0.0\"", "");
                        readResp = CoreLib.TransformXML(readResp, XslPath, $"{Version}Sabre_PNRReadRS.xsl");
                        oDoc = new XmlDocument();
                        oDoc.LoadXml(readResp);
                        oRoot = oDoc.DocumentElement;
                        if (oRoot.SelectNodes("TravelItinerary/ItineraryInfo/TPA_Extensions/IssuedTickets/IssuedTicket") is null)
                        {
                            issuedList = null;
                        }
                        else
                        {
                            issuedList = oRoot.SelectNodes("TravelItinerary/ItineraryInfo/TPA_Extensions/IssuedTickets/IssuedTicket");
                        }

                        if (issuedList != null)
                        {
                            foreach (XmlNode oNodeResp in issuedList)
                                strTemp += oNodeResp.OuterXml;
                        }

                        CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "strTickets", strTickets, ProviderSystems.LogUUID);
                        CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "strTemp", $"<IssuedTickets>{strTemp}</IssuedTickets>", ProviderSystems.LogUUID);
                        strIssueTicket = "<AirTicketRS>";
                        if (!string.IsNullOrEmpty(strTickets))
                        {
                            oDoc = null;
                            oRoot = null;
                            oDoc = new XmlDocument();
                            oDoc.LoadXml(strTickets);
                            oRoot = oDoc.DocumentElement;
                            CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "oRoot", oRoot.OuterXml, ProviderSystems.LogUUID);
                            if (oRoot.SelectSingleNode("Errors") is null)
                            {
                                strIssueTicket += "<Success/>";
                            }
                            else if (oRoot.SelectSingleNode("Errors/Error/ErrorInfo/Message").InnerText.Contains("ETR MESSAGE PROCESSED"))
                            {
                                strIssueTicket += "<Success/>";
                            }
                            else
                            {
                                strIssueTicket += oRoot.SelectSingleNode("Errors").OuterXml;
                            }
                        }
                        else
                        {
                            strIssueTicket += "<Success/>";
                        }

                        if (strTemp.Trim().Length > 0)
                        {
                            strIssueTicket += $"<IssuedTickets>{strTemp}</IssuedTickets>";
                        }

                        if (inSession)
                            strIssueTicket += $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID></AirTicketRS>";

                        strResponse = strIssueTicket;

                        CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "Last XML", strResponse, ProviderSystems.LogUUID);
                        strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_IssueTicketRS.xsl");
                        strResponse = strResponse.Replace("<UniqueID ID=\"\" />", "<UniqueID ID=\"" + strPnrNo + "\" />");
                    }
                    else
                    {

                        strResponse = strResponse.Contains("Error")
                            ? $"<AirTicketRS>{strResponse}</AirTicketRS>"
                            : $"<AirTicketRS><Success/></AirTicketRS>";


                        var strToReplace = "</AirTicketRS>";

                        if (inSession)
                            strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{strToReplace}");

                        strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_IssueTicketRS.xsl");
                        strResponse = strResponse.Replace("<UniqueID ID=\"\" />", "<UniqueID ID=\"" + strPnrNo + "\" />");
                    }
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.IssueTicketSessioned, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string TicketCoupon()
        {
            string strResponse = "";

            // *****************************************************************
            // Transform OTA Queue Request into Native Sabre Request     *
            // ***************************************************************** 

            try
            {
                var strRequest = SetRequest("Sabre_TicketCouponRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                if (strRequest.Contains("eTicketCouponLLSRQ"))
                {
                    strResponse = ttSA.SendMessage(strRequest, "eTicketCouponLLSRQ", "eTicketCouponLLSRQ");
                    //strResponse = strResponse.Replace("<![CDATA[", "<Line>").Replace("]]>", "</Line>").Replace(Constants.vbLf, "");
                }
                else if (strRequest.Contains("eTicketCouponRQ"))
                {
                    strResponse = ttSA.SendMessage(strRequest, "eTicketCouponLLSRQ", "eTicketCouponLLSRQ");
                }
                else if (strRequest.Contains("eTicketCouponRQ"))
                {
                    strResponse = ttSA.SendMessage(strRequest, "eTicketCouponLLSRQ", "eTicketCouponLLSRQ");
                }

                // *****************************************************************
                // Transform Native Sabre Queue Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var strToReplace = "</eTicketCouponLLSRS>";

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{strToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_TicketCouponRS.xsl");
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
                    ttSA = null;
                }
            }
            catch (Exception exx)
            {
                AddLog($"<M>{Request}<BL/>", ProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.TicketDisplay, exx.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string Update()
        {

            string strResponse = "";

            try
            {
                string strRequest = SetRequest("Sabre_PNRReadRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *********************
                // * Create Session    *
                // *********************
                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // ****************************
                // Retrieve existing PNR     *
                // **************************** 
                string strErrEvent = "Error Transforming OTA PNRRead Request.";

                // ********************************************
                // * Get Sabre Native PNR Retrieve response *
                // ********************************************
                string strNativePNRReply = ttSA.SendMessage(strRequest, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                strErrEvent = "Error sending Sabre PNR Retrieve Request.";
                if (strNativePNRReply.Contains("<OTA_TravelItineraryRS"))
                {
                    // *******************************
                    // Load OTA Modify XML document  *
                    // ******************************* 
                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(Request);
                    var oRoot = oDoc.DocumentElement;

                    // *******************************
                    // Modify PNR - Insert elements *
                    // ******************************* 
                    strErrEvent = "Modify PNR - Insert elements Error.";
                    string strEndTransaction = "";
                    string strIgnore = "";
                    string strQueue = "";
                    string strRead = "";
                    if (oRoot.SelectSingleNode("Position/Element[@Operation='insert']") != null)
                    {
                        // *******************************************************************
                        // * Transform OTA Modify Request into Sabre Native Insert Request *
                        // *******************************************************************
                        strRequest = SetRequest("Sabre_UpdateInsertRQ.xsl");

                        // ********************
                        // Get All Requests  * 
                        // ********************

                        try
                        {
                            var oDocTemp = new XmlDocument();
                            oDocTemp.LoadXml(strRequest);
                            var oRootTemp = oDocTemp.DocumentElement;
                            if (oRootTemp.SelectSingleNode("Queue") != null)
                            {
                                strQueue = oRootTemp.SelectSingleNode("Queue").InnerXml.Replace(" xmlns=\"\"", "");
                            }

                            strRead = oRootTemp.SelectSingleNode("Read").InnerXml.Replace(" xmlns=\"\"", "");
                            strIgnore = oRootTemp.SelectSingleNode("Ignore").InnerXml.Replace(" xmlns=\"\"", "");
                            strEndTransaction = oRootTemp.SelectSingleNode("ET").InnerXml.Replace(" xmlns=\"\"", "");
                            string strSegments = "";
                            // insert flights segments if any in request
                            if (oRootTemp.SelectSingleNode("AirBook") != null)
                            {
                                strSegments = oRootTemp.SelectSingleNode("AirBook").InnerXml;
                            }

                            if (!string.IsNullOrEmpty(strSegments))
                            {
                                strResponse = SendRequestSegment(ttSA, strSegments, "Air", "AirBook", "OTA_AirBookLLSRQ");
                                // Fatal Error
                                if (strResponse.Trim().Length > 0)
                                {
                                    strResponse = BuildOTAResponse(strResponse);
                                    return strResponse;
                                }
                            }
                            string strOther = "";
                            // insert other elements if any
                            if (oRootTemp.SelectSingleNode("Remarks") != null)
                            {
                                strOther = oRootTemp.SelectSingleNode("Remarks").InnerXml;
                                strResponse = SendRequestSegment(ttSA, strOther, "Remarks", "AddRemark", "AddRemarkLLSRQ");

                                // Fatal Error
                                if (strResponse.Trim().Length > 0)
                                {
                                    strResponse = BuildOTAResponse(strResponse);
                                    return strResponse;
                                }
                            }

                            if (oRootTemp.SelectSingleNode("SpecialServices") != null)
                            {
                                strOther = oRootTemp.SelectSingleNode("SpecialServices").InnerXml;
                                strResponse = SendRequestSegment(ttSA, strOther, "SpecialServices", "SpecialService", "SpecialServiceLLSRQ");

                                // Fatal Error
                                if (strResponse.Trim().Length > 0)
                                {
                                    strResponse = BuildOTAResponse(strResponse);
                                    return strResponse;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error Loading Transformed Request XML Document.\r\n{ex.Message}");
                        }

                    }

                    // *********************************
                    // * Send End Transaction Request  *
                    // *********************************
                    strResponse = ttSA.SendMessage(strEndTransaction, "EndTransaction", "EndTransactionLLSRQ", ConversationID);

                    // Check for Errors
                    strEndTransaction = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_TB_Errors.xsl");

                    if (strEndTransaction.Length > 0)
                    {
                        if (strEndTransaction.Contains("<Error"))
                        {
                            // Fatal Error
                            return BuildOTAResponse(strEndTransaction);
                        }
                        else if (strEndTransaction.Contains("<Warning>"))
                        {
                            Warnings += strEndTransaction;
                        }
                    }

                    // do we need a second end transact?
                    if (strResponse.Contains("PREVIOUS ENTRY"))
                    {
                        Thread.Sleep(1000);

                        CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "ET", "", ProviderSystems.LogUUID);
                        strResponse = ttSA.SendMessage(strEndTransaction, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                        strNative = $"{strNative}{strEndTransaction}{strResponse}";

                    }

                    if (strResponse.Contains("*IM AND CANCEL UNABLE SEGMENTS"))
                    {
                        string cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*IM</HostCommand></Request></SabreCommandLLSRQ>";
                        CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "*IM", "", ProviderSystems.LogUUID);
                        strResponse = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                        strNative = $"{strNative}{cryptic}{strResponse}";

                        var curDate = DateTime.Now;
                        strResponse = strResponse.Replace("</SabreCommandLLSRS>", "<CurrentDate>" + curDate + "</CurrentDate></SabreCommandLLSRS>");

                        // create fomatted error with flights that failed
                        CoreLib.SendTrace(ProviderSystems.UserID, "FailedFlights", "*IM", strResponse, ProviderSystems.LogUUID);
                    }
                    else
                    {
                        // do we need a second end transact?
                        if (strResponse.Contains("DIRECT CONNECT MESSAGES RECEIVED - ENTER *A OR *IM"))
                        {
                            var cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*IM</HostCommand></Request></SabreCommandLLSRQ>";
                            CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "*IM", "", ProviderSystems.LogUUID);
                            strResponse = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                            strNative = $"{strNative}{cryptic}{strResponse}";

                            var curDate = DateTime.Now;
                            strResponse = strResponse.Replace("</SabreCommandLLSRS>", $"<CurrentDate>{curDate}</CurrentDate></SabreCommandLLSRS>");
                            // Check for Errors
                            strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_TB_Errors.xsl");

                            if (strResponse.Length > 0 & !strResponse.Contains("Flight "))
                            {
                                if (strResponse.Contains("<Error"))
                                {
                                    // Fatal Error
                                    return BuildOTAResponse(strResponse);
                                }
                                else if (strResponse.Contains("<Warning>"))
                                {
                                    Warnings += strResponse;
                                }
                            }

                            CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "ET", "", ProviderSystems.LogUUID);
                            strResponse = ttSA.SendMessage(strEndTransaction, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                            strNative = $"{strNative}{strEndTransaction}{strResponse}";
                        }

                        if (strResponse.Contains("VERIFY ORDER OF ITINERARY SEGMENTS - MODIFY OR END TRANSACTION"))
                        {
                            var cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>0AA</HostCommand></Request></SabreCommandLLSRQ>";
                            CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "0AA", "", ProviderSystems.LogUUID);
                            strResponse = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                            strNative = $"{strNative}{cryptic}{strResponse}";

                            CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "ET", "", ProviderSystems.LogUUID);
                            strResponse = ttSA.SendMessage(strEndTransaction, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                            strNative = $"{strNative}{strEndTransaction}{strResponse}";

                        }

                        if (strResponse.Contains("MODIFY OR END TRANSACTION"))
                        {
                            CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "ET", "", ProviderSystems.LogUUID);
                            strResponse = ttSA.SendMessage(strEndTransaction, "EndTransaction", "EndTransactionLLSRQ", ConversationID);
                            strNative = $"{strNative}{strEndTransaction}{strResponse}";
                        }
                    }

                    // Check for Errors
                    strEndTransaction = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_TB_Errors.xsl");

                    if (strEndTransaction.Length > 0)
                    {
                        if (strEndTransaction.Contains("<Error"))
                        {
                            // Fatal Error
                            return BuildOTAResponse(strEndTransaction);
                        }
                        else if (strEndTransaction.Contains("<Warning>"))
                        {
                            Warnings += strEndTransaction;
                        }
                    }

                    // Retrieve the PNR
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;
                    if (oRoot.SelectSingleNode("UniqueID/@ID") != null)
                    {
                        if (!string.IsNullOrEmpty(strQueue))
                        {
                            CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "Queue PNR", "", ProviderSystems.LogUUID);
                            strQueue = strQueue.Replace("UniqueID", $"UniqueID ID=\"{oRoot.SelectSingleNode("UniqueID/@ID").InnerText}'\'");

                            strResponse = ttSA.SendMessage(strQueue, "QueuePlace", "QPlaceLLSRQ", ConversationID);
                        }

                        CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "Retreive PNR", "", ProviderSystems.LogUUID);
                        strRead = strRead.Replace("UniqueID", $"UniqueID ID=\"{oRoot.SelectSingleNode("UniqueID/@ID").InnerText}'\'");

                        strResponse = ttSA.SendMessage(strRead, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                        var cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*PQS</HostCommand></Request></SabreCommandLLSRQ>";
                        CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "*PQS", "", ProviderSystems.LogUUID);
                        cryptic = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                        strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"{cryptic}</OTA_TravelItineraryRS>");

                    }

                    // Close Session
                    CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "Clean up working area", "", ProviderSystems.LogUUID);
                    strIgnore = ttSA.SendMessage(strIgnore, "IgnoreTransaction", "IgnoreTransactionLLSRQ", ConversationID);
                }

                // *****************************************************************
                // Transform Native Sabre TravelBuild Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var strToReplace = "</TravelItineraryReadRS>";
                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{strToReplace}");

                    strErrEvent = "Sabre_PNRReadRS.xsl Error.";
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Update, exx.Message, ProviderSystems);
            }
            return strResponse;
        }

        public string UpdateSessioned()
        {
            string strResponse = "";

            try
            {
                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                Request = Request.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Replace("<?xml version=\"1.0\"?>", "");
                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                var oRoot = oDoc.DocumentElement;

                // **************************** 
                // Retrieve existing PNR * 
                // **************************** 
                string strErrEvent = "Error sending Sabre PNR Display Request.";
                string strRequest = "<TravelItineraryReadRQ Version=\"3.6.0\" xmlns=\"http://services.sabre.com/res/tir/v3_6\"><MessagingDetails><SubjectAreas><SubjectArea>FULL</SubjectArea></SubjectAreas></MessagingDetails></TravelItineraryReadRQ>";
                string strEndTransaction = "";

                // ********************************************
                // * Get Sabre Native PNR Retrieve response *
                // ********************************************
                string strNativePNRReply = ttSA.SendMessage(strRequest, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);
                // strNativePNRReply = strNativePNRReply.Replace("<or:", "<").Replace("</or:", "</").Replace("xsi:type=""or:", "type=""").Replace("xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""", "")

                strErrEvent = "Error processing PNR update.";
                if (strNativePNRReply.Contains("<TravelItineraryReadRS"))
                {
                    // *******************************
                    // Load OTA Modify XML document  *
                    // ******************************* 
                    oDoc = new XmlDocument();
                    oDoc.LoadXml(Request);
                    oRoot = oDoc.DocumentElement;

                    // *******************************
                    // Modify PNR - Insert elements *
                    // ******************************* 
                    strErrEvent = "Modify PNR - Insert elements Error.";
                    if (oRoot.SelectSingleNode("Position/Element[@Operation='insert']") != null)
                    {
                        // *******************************************************************
                        // * Transform OTA Modify Request into Sabre Native Insert Request *
                        // *******************************************************************
                        strRequest = CoreLib.TransformXML($"<UpdateInsert>{Request}{strNativePNRReply}</UpdateInsert>", XslPath, $"{Version}Sabre_UpdateInsertRQ.xsl");

                        // ********************
                        // Get All Requests  * 
                        // ********************
                        try
                        {
                            string strSegments = "";
                            string strQueue = "";
                            var oDocTemp = new XmlDocument();
                            oDocTemp.LoadXml(strRequest);
                            var oRootTemp = oDocTemp.DocumentElement;
                            if (oRootTemp.SelectSingleNode("Queue") != null)
                            {
                                strQueue = oRootTemp.SelectSingleNode("Queue").InnerXml.Replace(" xmlns=\"\"", "");
                            }

                            string strRead = oRootTemp.SelectSingleNode("Read").InnerXml.Replace(" xmlns=\"\"", "");
                            string strIgnore = oRootTemp.SelectSingleNode("Ignore").InnerXml.Replace(" xmlns=\"\"", "");
                            strEndTransaction = oRootTemp.SelectSingleNode("ET").InnerXml.Replace(" xmlns=\"\"", "");

                            // insert flights segments if any in request
                            if (oRootTemp.SelectSingleNode("AirBook") != null)
                            {
                                strSegments = oRootTemp.SelectSingleNode("AirBook").InnerXml;
                            }

                            if (!string.IsNullOrEmpty(strSegments))
                            {
                                strResponse = SendRequestSegment(ttSA, strSegments, "Air", "AirBook", "OTA_AirBookLLSRQ");
                                // Fatal Error
                                if (strResponse.Trim().Length > 0)
                                {
                                    strResponse = BuildOTAResponse(strResponse);
                                    strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID></OTA_TravelItineraryRS>");

                                    return strResponse;
                                }
                            }

                            // insert other elements if any
                            if (oRootTemp.SelectSingleNode("Remarks") != null)
                            {
                                var strOther = oRootTemp.SelectSingleNode("Remarks").InnerXml;
                                strResponse = SendRequestSegment(ttSA, strOther, "Remarks", "AddRemark", "AddRemarkLLSRQ");

                                // Fatal Error
                                if (strResponse.Trim().Length > 0)
                                {
                                    strResponse = BuildOTAResponse(strResponse);
                                    strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID></OTA_TravelItineraryRS>");
                                    return strResponse;
                                }
                            }

                            if (oRootTemp.SelectSingleNode("SpecialServices") != null)
                            {
                                var strOther = oRootTemp.SelectSingleNode("SpecialServices").InnerXml;
                                strResponse = SendRequestSegment(ttSA, strOther, "SpecialServices", "SpecialService", "SpecialServiceLLSRQ");

                                // Fatal Error
                                if (strResponse.Trim().Length > 0)
                                {
                                    strResponse = BuildOTAResponse(strResponse);
                                    strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID></OTA_TravelItineraryRS>");
                                    return strResponse;
                                }
                            }

                            if (oRootTemp.SelectSingleNode("FuturePriceInfo") != null)
                            {
                                XmlDocument oDocFPI;
                                XmlElement oRootFPI;
                                oDocFPI = new XmlDocument();
                                oDocFPI.LoadXml(oRootTemp.SelectSingleNode("FuturePriceInfo").OuterXml);
                                oRootFPI = oDocFPI.DocumentElement;
                                var nsmgr = new XmlNamespaceManager(oDocFPI.NameTable);
                                nsmgr.AddNamespace("sx", "http://webservices.sabre.com/sabreXML/2003/07");

                                // Dim strPrinter As String = oRootFPI.SelectSingleNode("sx:DesignatePrinterRQ", nsmgr).OuterXml
                                // strResponse = SendRequestSegment(strPrinter, "AirTicket", "AirTicket", "DesignatePrinterLLSRQ")

                                foreach (XmlNode oNodeFPI in oRootFPI.SelectNodes("sx:SabreCommandLLSRQ", nsmgr))
                                {
                                    string argstrRequest = oNodeFPI.OuterXml;
                                    strResponse = SendRequestSegment(ttSA, argstrRequest, "FuturePriceInfo", "SabreCommand", "SabreCommandLLSRQ");

                                    // Fatal Error
                                    if (strResponse.Trim().Length > 0)
                                    {
                                        strResponse = BuildOTAResponse(strResponse);
                                        strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID></OTA_TravelItineraryRS>");
                                        return strResponse;
                                    }
                                }
                            }

                            if (oRootTemp.SelectSingleNode("RefundDocument") != null)
                            {
                                var oDocRD = new XmlDocument();
                                oDocRD.LoadXml(oRootTemp.SelectSingleNode("RefundDocument").OuterXml);
                                var oRootRD = oDocRD.DocumentElement;
                                var nsmgr = new XmlNamespaceManager(oDocRD.NameTable);
                                nsmgr.AddNamespace("sx", "http://webservices.sabre.com/sabreXML/2003/07");
                                foreach (XmlNode oNodeRd in oRootRD.SelectNodes("sx:AddAccountingLineRQ", nsmgr))
                                {
                                    string argstrRequest1 = oNodeRd.OuterXml;
                                    strResponse = SendRequestSegment(ttSA, argstrRequest1, "AddAccountingLine", "AddAccountingLine", "AddAccountingLineLLSRQ");
                                    //oNodeRd.OuterXml = argstrRequest1;

                                    // Fatal Error
                                    if (strResponse.Trim().Length > 0)
                                    {
                                        strResponse = BuildOTAResponse(strResponse);
                                        strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID></OTA_TravelItineraryRS>");
                                        return strResponse;
                                    }
                                }
                            }

                            if (oRootTemp.SelectSingleNode("AccountableDocument") != null)
                            {
                                XmlDocument oDocRD;
                                XmlElement oRootRD;
                                oDocRD = new XmlDocument();
                                oDocRD.LoadXml(oRootTemp.SelectSingleNode("AccountableDocument").OuterXml);
                                oRootRD = oDocRD.DocumentElement;
                                var nsmgr = new XmlNamespaceManager(oDocRD.NameTable);
                                nsmgr.AddNamespace("sx", "http://webservices.sabre.com/sabreXML/2003/07");
                                foreach (XmlNode oNodeRD in oRootRD.SelectNodes("sx:AddAccountingLineRQ", nsmgr))
                                {
                                    string argstrRequest2 = oNodeRD.OuterXml;
                                    strResponse = SendRequestSegment(ttSA, argstrRequest2, "AddAccountingLine", "AddAccountingLine", "AddAccountingLineLLSRQ");

                                    // Fatal Error
                                    if (strResponse.Trim().Length > 0)
                                    {
                                        strResponse = BuildOTAResponse(strResponse);
                                        strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID></OTA_TravelItineraryRS>");
                                        return strResponse;
                                    }
                                }
                            }

                            if (oRootTemp.SelectSingleNode("BulkDocument") != null)
                            {
                                XmlElement oRootBd;
                                var oDocBD = new XmlDocument();
                                oDocBD.LoadXml(oRootTemp.SelectSingleNode("BulkDocument").OuterXml);
                                oRootBd = oDocBD.DocumentElement;
                                var nsmgr = new XmlNamespaceManager(oDocBD.NameTable);
                                nsmgr.AddNamespace("sx", "http://webservices.sabre.com/sabreXML/2003/07");
                                foreach (XmlNode oNodeBD in oRootBd.SelectNodes("sx:SabreCommandLLSRQ", nsmgr))
                                {
                                    string argstrRequest3 = oNodeBD.OuterXml;
                                    strResponse = SendRequestSegment(ttSA, argstrRequest3, "BulkDocument", "SabreCommand", "SabreCommandLLSRQ");

                                    // Fatal Error
                                    if (strResponse.Trim().Length > 0 && !strResponse.Contains("*"))
                                    {
                                        strResponse = BuildOTAResponse(strResponse);
                                        strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID></OTA_TravelItineraryRS>");
                                        return strResponse;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error Loading Transformed Request XML Document.\r\n{ex.Message}");
                        }
                    }

                    if (Request.Contains("TransactionStatusCode=\"End\"") || Request.Contains("TransactionStatusCode='End'"))
                    {
                        string strER = @"<SabreCommandLLSRQ xmlns=""http://webservices.sabre.com/sabreXML/2011/10"" Version=""2.0.0""><Request Output=""SCREEN"" MDRSubset=""AD01"" CDATA=""true""><HostCommand>6TRIPXML\ER</HostCommand></Request></SabreCommandLLSRQ>";
                        strResponse = ttSA.SendMessage(strER, "*ER", "SabreCommandLLSRQ", ConversationID);
                        if (strResponse.Contains("*WARNING EDITS*") | strResponse.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strResponse.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED") | strResponse.Contains("END OR IGNORE PNR") | strResponse.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT") | strResponse.Contains("MIN CONNX TIME"))
                        {
                            strER = strER.Replace(@"6TRIPXML\ER", "ER");
                            strResponse = ttSA.SendMessage(strER, "ER", "SabreCommandLLSRQ", ConversationID);
                            if (strResponse.Contains("*WARNING EDITS*") | strResponse.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strResponse.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED") | strResponse.Contains("END OR IGNORE PNR") | strResponse.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT") | strResponse.Contains("MIN CONNX TIME"))
                            {
                                int iTry = 0;
                                while ((strResponse.Contains("*WARNING EDITS*") | strResponse.Contains("VERIFY ORDER OF ITINERARY SEGMENTS") | strResponse.Contains("TOO MANY PNR ERRORS - EDIT SUSPENDED") | strResponse.Contains("END OR IGNORE PNR") | strResponse.Contains("INFANT DETAILS REQUIRED IN SSR - ENTER 3INFT") | strResponse.Contains("MIN CONNX TIME")) & iTry < 8)
                                {
                                    Thread.Sleep(1000);

                                    strResponse = ttSA.SendMessage(strER, "ER", "SabreCommandLLSRQ", ConversationID);
                                    iTry += 1;
                                }
                            }
                        }

                        while (strResponse.Contains("CTP EDITS IN PROGRESS....PLEASE WAIT"))
                        {
                            Thread.Sleep(1000);
                            strResponse = ttSA.SendMessage(strER, "ER", "EndTransactionLLSRQ", ConversationID);
                        }
                    }

                    strRequest = "<TravelItineraryReadRQ Version=\"3.6.0\" xmlns=\"http://services.sabre.com/res/tir/v3_6\"><MessagingDetails><SubjectAreas><SubjectArea>FULL</SubjectArea></SubjectAreas></MessagingDetails></TravelItineraryReadRQ>";
                    strResponse = ttSA.SendMessage(strRequest, "TravelItineraryReadRQ", "TravelItineraryReadRQ", ConversationID);

                    // Check for Errors
                    strEndTransaction = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_TB_Errors.xsl");

                    if (strEndTransaction.Length > 0)
                    {
                        if (strEndTransaction.Contains("<Error"))
                        {
                            // Fatal Error
                            strResponse = BuildOTAResponse(strEndTransaction);
                            strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID></OTA_TravelItineraryRS>");
                            return strResponse;
                        }
                        else if (strEndTransaction.Contains("<Warning>"))
                        {
                            Warnings += strEndTransaction;
                        }
                    }
                }

                string cryptic = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>*PQS</HostCommand></Request></SabreCommandLLSRQ>";
                CoreLib.SendTrace(ProviderSystems.UserID, "SabreCommand", "*PQS", "", ProviderSystems.LogUUID);
                cryptic = ttSA.SendMessage(cryptic, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                string strFaretype = "<DisplayPriceQuoteRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.5.2\"><AirItineraryPricingInfo><Record/></AirItineraryPricingInfo></DisplayPriceQuoteRQ>";
                CoreLib.SendTrace(ProviderSystems.UserID, "FareType", "PD", strFaretype, ProviderSystems.LogUUID);
                strFaretype = ttSA.SendMessage(strFaretype, "FareType", "DisplayPriceQuoteLLSRQ", ConversationID);
                strResponse = strResponse.Replace("</TravelItineraryReadRS>", $"{Errors}{Warnings}{Request}{cryptic}{strFaretype}</TravelItineraryReadRS>");

                var reqTime = DateTime.Now;
                LogMessageToFile("Update", ref strResponse, reqTime, reqTime);

                // *****************************************************************
                // Transform Native Sabre TravelBuild Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strErrEvent = "Sabre_PNRReadRS.xsl Error.";
                    Version = "v03";

                    var strToReplace = "</TravelItineraryReadRS>";

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{strToReplace}");

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
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.UpdateSessioned, ex.Message, ProviderSystems);
            }
            return strResponse;
        }

        private string SendRequestSegment(SabreAdapter ttSA, string strRequest, string Segment, string Service, string Action)
        {
            string strResponse = "";
            if (!string.IsNullOrEmpty(strRequest.Trim()))
            {
                CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", Segment, "", ProviderSystems.LogUUID);
                strResponse = ttSA.SendMessage(strRequest, Service, Action, ConversationID);
                if (strResponse.Contains("TICKETING IN PROGRESS"))
                {
                    int iTry = 0;
                    while (strResponse.Contains("TICKETING IN PROGRESS") & iTry < 5)
                    {
                        Thread.Sleep(2000);

                        strResponse = ttSA.SendMessage(strRequest, Service, Action, ConversationID);
                        iTry += 1;
                    }
                }

                strNative += $"{strRequest}{strResponse}";

                if (Segment == "BulkDocument")
                    strResponse = strResponse.Replace("<![CDATA[", "<Line>").Replace("]]>", "</Line>").Replace("\r\n", "").Trim();

                // Check for Errors
                var transResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_TB_Errors.xsl");

                if (string.IsNullOrEmpty(transResponse) && Segment == "BulkDocument")
                    transResponse = strResponse;

                CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "strResponse", transResponse, ProviderSystems.LogUUID);
                // Log Errors
                if (transResponse.Contains("<Error") & Segment == "BulkDocument")
                {
                    Warnings += transResponse.Replace("<Error", "<Warning").Replace("</Error", "</Warning");
                    transResponse = "";
                }
                else if (transResponse.Contains("<Error"))
                {
                    Errors += transResponse;
                }
                else if (transResponse.Contains("<Warning"))
                {
                    Warnings += transResponse;
                    transResponse = "";
                }

                return transResponse;
            }
            else
            {
                return "";
            }

        }

        private string BuildOTAResponse(string strResponse)
        {
            string strEchoToken = "";
            try
            {
                if (Request.Contains(" EchoToken"))
                {
                    var oDocReq = new XmlDocument();
                    oDocReq.LoadXml(Request);
                    var oRootReq = oDocReq.DocumentElement;

                    strEchoToken = $"<EchoToken>{oRootReq.Attributes.GetNamedItem("EchoToken").Value}</EchoToken>";
                }

                strResponse = $"<PNRReply>{strResponse}{strEchoToken}</PNRReply>";
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_PNRReadRS.xsl");
                return strResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}