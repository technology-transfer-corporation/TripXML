using System;
using System.Data;
using System.Text;
using System.Xml;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{

    public class cServiceAmadeusWS
    {

        public event GotResponseEventHandler GotResponse;

        public delegate void GotResponseEventHandler(string Response);

        #region  Properties 
        private StringBuilder sb = new StringBuilder();

        public TripXMLProviderSystems ttProviderSystems;

        public int ServiceID { get; set; }

        public string Request { get; set; } = "";

        public string Version { get; set; } = "";

        #endregion

        public void SendAirRequest()
        {
            AmadeusWS.AirServices ttService = null;
            string strResponse = "";
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlDocument oDocReq = null;
            XmlElement oRootReq = null;
            XmlDocument oDocReq1 = null;
            XmlElement oRootReq1 = null;
            XmlNode oNode = null;
            XmlNode oBLNode = null;
            XmlAttribute oAttr;
            string strMsg = "";
            string strPCC = "";
            string strBLPCC = "";
            int i;
            DataTable dt = null;

            try
            {
                ttService = new AmadeusWS.AirServices();

                {
                    ref var withBlock = ref ttService;
                    withBlock.Version = Version;
                    withBlock.XslPath = XslPath;
                    withBlock.ttProviderSystems = ttProviderSystems;

                    oDocReq = new XmlDocument();
                    oDocReq.LoadXml(Request);
                    oRootReq = oDocReq.DocumentElement;
                    oNode = oRootReq.SelectSingleNode("POS/Source");
                    // oNode1 = oRootReq.SelectSingleNode("POS/Source")


                    if (oNode.Attributes["PseudoCityCode"] is null)
                    {
                        oAttr = oDocReq.CreateAttribute("PseudoCityCode");
                        oNode.Attributes.Append(oAttr);
                    }


                    // If mintServiceID = ttService.LowFarePlus Then
                    dt = new DataTable();
                    dt.Columns.Add("Airline");
                    dt.Columns.Add("Class");
                    dt.Columns.Add("PreferLevel");

                    // Filter Classes
                    string str2 = "";
                    foreach (XmlNode oNode1 in oRootReq.SelectNodes("TravelPreferences/VendorPref[@Code!='']"))
                    {
                        string str1 = oNode1.Attributes["Code"].InnerText;

                        foreach (XmlNode aClassNode in oNode1.SelectNodes("AClasses/AClass"))
                        {
                            if (oNode1.SelectSingleNode("AClasses").Attributes["PreferLevel"] is null)
                            {
                                str2 = "";
                            }
                            else
                            {
                                str2 = oNode1.ChildNodes[0].Attributes["PreferLevel"].InnerText.ToLower();
                            }
                            var dr = dt.NewRow();
                            dr["Airline"] = str1;
                            dr["Class"] = aClassNode.InnerXml.ToString();
                            if (str2 != "unacceptable")
                            {
                                dr["PreferLevel"] = "1";
                            }
                            else
                            {
                                dr["PreferLevel"] = "0";
                            }
                            dt.Rows.Add(dr);
                        }
                    }
                    // End If


                    oNode.Attributes["PseudoCityCode"].InnerText = withBlock.ttProviderSystems.PCC;
                    Request = oRootReq.OuterXml;

                    switch (ServiceID)
                    {
                        case (int)ttServices.AirAvail:
                            {
                                strMsg = "AirAvail";
                                break;
                            }
                        case (int)ttServices.LowFare:
                            {
                                strMsg = "LowFare";
                                break;
                            }
                        case (int)ttServices.LowFarePlus:
                            {
                                strMsg = "LowFare";
                                break;
                            }
                        case (int)ttServices.LowFareSchedule:
                            {
                                strMsg = "LowFare";
                                break;
                            }
                        case (int)ttServices.AirSchedule:
                            {
                                strMsg = "AirSchedule";
                                break;
                            }
                        case (int)ttServices.LowFareMatrix:
                            {
                                strMsg = "LowFare";
                                break;
                            }
                        case (int)ttServices.LowFareFlights:
                            {
                                strMsg = "LowFareFlights";
                                break;
                            }
                        case (int)ttServices.LowOfferMatrix:
                            {
                                strMsg = "LowFare";
                                break;
                            }
                        case (int)ttServices.LowOfferSearch:
                            {
                                strMsg = "LowFare";
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Amadeus air services.");
                            }
                    }

                    bool EKBP = false;

                    if (!string.IsNullOrEmpty(ttProviderSystems.BLFile))
                    {
                        oDoc = new XmlDocument();
                        // Load Access Control List into memory
                        try
                        {
                            oDoc.Load(ttProviderSystems.BLFile);
                        }
                        catch (Exception exr)
                        {
                            CoreLib.SendTrace("", "cServiceAmadeusWS", "Error Loading business logic file", exr.Message, string.Empty);
                            throw exr;
                        }

                        oRoot = oDoc.DocumentElement;
                        // check "In" business logic
                        sb.Append("Message[@Name='").Append(strMsg).Append("'][@Direction='In']");
                        oNode = oRoot.SelectSingleNode(sb.ToString());
                        sb.Remove(0, sb.Length);

                        if (oNode is not null)
                        {

                            if (ttProviderSystems.PCC.StartsWith("*"))
                            {
                                ttProviderSystems.PCC = ttProviderSystems.PCC.Substring(1);
                                EKBP = true;
                            }

                            sb.Append("ProviderBL[@Name='Amadeus'][@System='").Append(ttProviderSystems.System).Append("'][@PCC='").Append(ttProviderSystems.PCC.ToUpper()).Append("']");
                            oNode = oNode.SelectSingleNode(sb.ToString());
                            sb.Remove(0, sb.Length);

                            if (oNode is not null)
                            {
                                sb.Append(XslPath).Append(@"BL\");
                                Request = BusinessLogicIn(Request, oNode.OuterXml, sb.ToString(), strMsg);
                                sb.Remove(0, sb.Length);
                            }
                            else
                            {
                                sb.Append("Message[@Name='").Append(strMsg).Append("'][@Direction='In']");
                                oNode = oRoot.SelectSingleNode(sb.ToString());
                                sb.Remove(0, sb.Length);
                                sb.Append("ProviderBL[@Name='Amadeus'][@System='").Append(ttProviderSystems.System).Append("'][contains(@PCC,'*')]");
                                oNode = oNode.SelectSingleNode(sb.ToString());
                                sb.Remove(0, sb.Length);

                                if (oNode is not null)
                                {
                                    strBLPCC = oNode.SelectSingleNode("@PCC").InnerText;
                                    strPCC = ttProviderSystems.PCC.ToUpper();

                                    sb.Append("Preferences/Except[PCC='").Append(strPCC).Append("']");
                                    if (oNode.SelectSingleNode(sb.ToString()) is null)
                                    {
                                        if (strBLPCC.Length == strPCC.Length)
                                        {
                                            var loopTo = strBLPCC.Length - 1;
                                            for (i = 0; i <= loopTo; i++)
                                            {
                                                if (strBLPCC.Substring(i, 1) == "*")
                                                {
                                                    strPCC = strPCC.Remove(i, 1);
                                                    strPCC = strPCC.Insert(i, "*");
                                                }
                                            }

                                            if ((strBLPCC ?? "") == (strPCC ?? ""))
                                            {

                                                if (EKBP)
                                                {
                                                    Request = Request.Replace("PseudoCityCode=\"*", "PseudoCityCode=\"");
                                                    Request = Request.Replace("OTA_AirLowFareSearchPlusRQ", "OTA_AirLowFareSearchRQ");
                                                    // mintServiceID = ttServices.LowFare
                                                }

                                                sb.Remove(0, sb.Length);
                                                sb.Append(XslPath).Append(@"BL\");
                                                Request = BusinessLogicIn(Request, oNode.OuterXml, sb.ToString(), strMsg);
                                            }
                                        }
                                    }
                                    sb.Remove(0, sb.Length);
                                }
                            }
                        }
                        oDoc = null;
                        oRoot = null;
                        oNode = null;
                    }

                    withBlock.Request = Request.Trim();

                    switch (ServiceID)
                    {
                        case (int)ttServices.AirAvail:
                            {
                                strResponse = withBlock.AirAvail();
                                break;
                            }
                        case (int)ttServices.LowFare:
                            {

                                if (EKBP & ttProviderSystems.UserID == "FlightSite" & (withBlock.Request.Contains("<VendorPref Code=\"EK\"") | withBlock.Request.Contains("<VendorPref Code=\"BP\"")))
                                {
                                    Console.Write(withBlock.Request);
                                    withBlock.Request = withBlock.Request.Replace("<VendorPref Code=\"G3\" PreferLevel=\"Unacceptable\" />", string.Empty);
                                    Console.Write(withBlock.Request);
                                    withBlock.Request = withBlock.Request.Replace("<VendorPref Code=\"FL\" PreferLevel=\"Unacceptable\" />", string.Empty);
                                    Console.Write(withBlock.Request);
                                    withBlock.Request = withBlock.Request.Replace("<VendorPref Code=\"U2\" PreferLevel=\"Unacceptable\" />", string.Empty);
                                    Console.Write(withBlock.Request);
                                }
                                else if (!EKBP & ttProviderSystems.UserID == "FlightSite" & (withBlock.Request.Contains("<VendorPref Code=\"EK\"") | withBlock.Request.Contains("<VendorPref Code=\"BP\"")))
                                {
                                    withBlock.Request = withBlock.Request.Replace("<VendorPref Code=\"BP\" PreferLevel=\"Only\" />", "<VendorPref Code=\"BP\" PreferLevel=\"Unacceptable\" />");
                                    Console.Write(withBlock.Request);
                                    withBlock.Request = withBlock.Request.Replace("<VendorPref Code=\"EK\" PreferLevel=\"Only\" />", "<VendorPref Code=\"EK\" PreferLevel=\"Unacceptable\" />");
                                    Console.Write(withBlock.Request);
                                }

                                strResponse = withBlock.LowFare();
                                break;
                            }

                        // If EKBP Then
                        // strResponse = strResponse.Replace("OTA_AirLowFareSearchRS", "OTA_AirLowFareSearchPlusRS")
                        // End If
                        case (int)ttServices.LowFarePlus:
                            {
                                // oDocReq1 = New XmlDocument
                                // oDocReq1.LoadXml(mstrRequest)
                                // oRootReq1 = oDocReq1.DocumentElement

                                if (ttProviderSystems.UserID == "FlightSite" & (withBlock.Request.Contains("<VendorPref Code=\"EK\" PreferLevel=\"Only\"") | withBlock.Request.Contains("<VendorPref Code=\"BP\" PreferLevel=\"Only\"")))
                                {
                                    withBlock.Request = Request.Replace("OTA_AirLowFareSearchPlusRQ", "OTA_AirLowFareSearchRQ");
                                    strResponse = withBlock.LowFare();
                                    strResponse = strResponse.Replace("OTA_AirLowFareSearchRS", "OTA_AirLowFareSearchPlusRS");
                                }
                                else
                                {
                                    strResponse = withBlock.LowFarePlus();
                                }

                                break;
                            }

                        // If oRootReq1.SelectNodes("OriginDestinationInformation").Count < 4 Then
                        // strResponse = .LowFarePlus()
                        // Else
                        // .Request = mstrRequest.Replace("OTA_AirLowFareSearchPlusRQ", "OTA_AirLowFareSearchRQ")
                        // strResponse = .LowFare()
                        // strResponse = strResponse.Replace("OTA_AirLowFareSearchRS", "OTA_AirLowFareSearchPlusRS")
                        // End If
                        // oDocReq1 = Nothing
                        // oRootReq1 = Nothing
                        case (int)ttServices.LowFareSchedule:
                            {
                                strResponse = withBlock.LowFareSchedule();
                                break;
                            }
                        case (int)ttServices.AirSchedule:
                            {
                                strResponse = withBlock.AirSchedule();
                                break;
                            }
                        case (int)ttServices.LowFareMatrix:
                            {
                                strResponse = withBlock.LowFareMatrix();
                                break;
                            }
                        case (int)ttServices.LowOfferMatrix:
                            {
                                strResponse = withBlock.LowOfferMatrix();
                                break;
                            }
                        case (int)ttServices.LowOfferSearch:
                            {
                                strResponse = withBlock.LowOfferSearch();
                                break;
                            }
                        case (int)ttServices.LowFareFlights:
                            {
                                strResponse = withBlock.LowFareFlights();
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Amadeus air services.");
                            }
                    }


                    // '''''''''''''''''Air line class filtering -  shashin''''''''''''''''''

                    // If dt.Rows.Count > 0 Then
                    // Select Case mintServiceID
                    // Case 6, 7
                    // strResponse = FilterAirLineClasses(strResponse, dt)
                    // Case 68
                    // strResponse = FilterAirLineClasses_LFS(strResponse, dt)
                    // End Select
                    // End If

                    if (dt.Select("PreferLevel='1'").Length > 0)
                    {
                        switch (ServiceID)
                        {
                            case 6:
                            case 7:
                                {
                                    strResponse = FilterAirLineClasses(strResponse, dt);
                                    break;
                                }
                            case 68:
                                {
                                    strResponse = FilterAirLineClasses_LFS(strResponse, dt);
                                    break;
                                }
                        }
                    }

                    if (dt.Select("PreferLevel='0'").Length > 0)
                    {
                        switch (ServiceID)
                        {
                            case 6:
                            case 7:
                                {
                                    strResponse = FilterAirLineClasses_remove(strResponse, dt);
                                    break;
                                }
                            case 68:
                                {
                                    strResponse = FilterAirLineClasses_LFS_remove(strResponse, dt);
                                    break;
                                }
                        }
                    }

                    // '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    // ttAA = .ttAPIAdapter

                    if (!string.IsNullOrEmpty(ttProviderSystems.BLFile) & ServiceID != (int)ttServices.LowFareSchedule)
                    {
                        oDoc = new XmlDocument();
                        // Load Access Control List into memory
                        try
                        {
                            oDoc.Load(ttProviderSystems.BLFile);
                        }
                        catch (Exception exr)
                        {
                            CoreLib.SendTrace("", "cServiceAmadeus", "Error Loading business logic file", exr.Message, string.Empty);
                            throw exr;
                        }

                        oRoot = oDoc.DocumentElement;
                        // check "Out" business logic
                        sb.Append("Message[@Name='").Append(strMsg).Append("'][@Direction='Out']");
                        oNode = oRoot.SelectSingleNode(sb.ToString());
                        sb.Remove(0, sb.Length);

                        if (oNode is not null)
                        {
                            // check if non ticketable flights/fares to eliminate
                            sb.Append("NoTktAirline[@Name='Amadeus'][@System='").Append(ttProviderSystems.System).Append("'][@PCC='").Append(ttProviderSystems.PCC.ToUpper()).Append("']");
                            oBLNode = oNode.SelectSingleNode(sb.ToString());
                            sb.Remove(0, sb.Length);

                            if (oBLNode is not null)
                            {
                                sb.Append(XslPath).Append(@"BL\");
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, sb.ToString(), strMsg, "NoTkt");
                                sb.Remove(0, sb.Length);
                            }

                            // check if no mix airline to eliminate
                            sb.Append("NoMixAirline[@Name='Amadeus'][@System='").Append(ttProviderSystems.System).Append("'][@PCC='").Append(ttProviderSystems.PCC.ToUpper()).Append("']");
                            oBLNode = oNode.SelectSingleNode(sb.ToString());
                            sb.Remove(0, sb.Length);

                            if (oBLNode is not null)
                            {
                                sb.Append(XslPath).Append(@"BL\");
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, sb.ToString(), strMsg, "NoMix");
                                sb.Remove(0, sb.Length);
                            }

                            // check if No Fare Type to eliminate
                            sb.Append("NoFareType[@Name='Amadeus'][@System='").Append(ttProviderSystems.System).Append("'][@PCC='").Append(ttProviderSystems.PCC.ToUpper()).Append("']");
                            oBLNode = oNode.SelectSingleNode(sb.ToString());
                            sb.Remove(0, sb.Length);

                            if (oBLNode is not null)
                            {
                                sb.Append(XslPath).Append(@"BL\");
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, sb.ToString(), strMsg, "NoFareType");
                                sb.Remove(0, sb.Length);
                            }

                            // ' add fare markup if needed
                            sb.Append("ProviderBL[@Name='Amadeus'][@System='").Append(ttProviderSystems.System).Append("'][@PCC='").Append(ttProviderSystems.PCC).Append("']");
                            oBLNode = oNode.SelectSingleNode(sb.ToString());
                            sb.Remove(0, sb.Length);

                            if (oBLNode is not null)
                            {
                                sb.Append(XslPath).Append(@"BL\");
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, sb.ToString(), strMsg, "");
                                sb.Remove(0, sb.Length);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                sb.Append("AmadeusWS").Append("-").Append(ttProviderSystems.PCC);
                strResponse = FormatErrorMessage((ttServices)ServiceID, ex.Message, sb.ToString(), "", false, Version);
                sb.Remove(0, sb.Length);
            }
            finally
            {
                if (ttService is not null)
                    ttService = null;
                oDocReq = null;
                oRootReq = null;
                GotResponse?.Invoke(strResponse);
            }

        }

        public void SendCarRequest()
        {
            CarServices ttService = null;
            string strResponse = "";

            try
            {
                ttService = new CarServices();

                {
                    ref var withBlock = ref ttService;
                    withBlock.Version = Version;
                    withBlock.XslPath = XslPath;
                    withBlock.Request = Request;
                    withBlock.ttProviderSystems = ttProviderSystems;

                    switch (ServiceID)
                    {
                        case (int)ttServices.CarAvail:
                            {
                                strResponse = withBlock.CarAvail();
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Amadeus car services.");
                            }
                    }

                    // ttAA = .ttAPIAdapter
                }
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage((ttServices)ServiceID, ex.Message, "Amadeus", "", false, Version);
            }
            finally
            {
                if (ttService is not null)
                    ttService = null;
                GotResponse?.Invoke(strResponse);
            }

        }

        public void SendHotelRequest()
        {
            HotelServices ttService = null;
            string strResponse = "";

            try
            {
                ttService = new HotelServices();

                {
                    ref var withBlock = ref ttService;
                    withBlock.Version = Version;
                    withBlock.XslPath = XslPath;
                    withBlock.Request = Request;
                    withBlock.ttProviderSystems = ttProviderSystems;

                    switch (ServiceID)
                    {
                        case (int)ttServices.HotelAvail:
                            {
                                strResponse = withBlock.HotelAvail();
                                break;
                            }
                        case (int)ttServices.HotelSearch:
                            {
                                strResponse = withBlock.HotelSearch();
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Amadeus hotel services.");
                            }
                    }

                    // ttAA = .ttAPIAdapter
                }
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage((ttServices)ServiceID, ex.Message, "Amadeus", "", false, Version);
            }
            finally
            {
                if (ttService is not null)
                    ttService = null;
                GotResponse?.Invoke(strResponse);
            }

        }

        /// <param name="strResponse">Response after converting to OTA format</param>
        /// <param name="dtClasses">DataTable of airlines and classes</param>
        /// <returns>Filtered response for given classes</returns>
        /// <remarks>By Shashin - 31/03/2010</remarks>
        private string FilterAirLineClasses_remove(string strResponse, DataTable dtClasses)
        {
            if (strResponse.IndexOf("Success") != -1)
            {
                string strResult;
                string strXML;
                // Dim inStart As Integer = strResponse.IndexOf("<soap:Body>") + 11
                // Dim iLength As Integer = strResponse.IndexOf("</soap:Body>") - (strResponse.IndexOf("<soap:Body>") + 11)
                // strXML = "<wmLowFarePlusResponse xmlns=""http://tripxml.downtowntravel.com/wsLowFarePlus"">" + strResponse '.Substring(inStart, iLength)
                // strXML = "<wmLowFarePlusResponse xmlns=""tripxml.dowtowntravel.com/tripxml/wsLowFarePlus"">" + strResponse + "</wmLowFarePlusResponse>"
                strXML = strResponse;

                var oDoc = new XmlDocument();
                oDoc.LoadXml(strXML);
                XmlElement oRoot;
                oRoot = oDoc.DocumentElement;
                XmlNode oNode;
                XmlNode oInnerNode;
                int cnt;
                var seqCounter = default(int);
                cnt = oRoot.ChildNodes[1].ChildNodes.Count;
                var sb = new StringBuilder();
                string strHeader = strXML.Substring(0, strXML.IndexOf("<PricedItineraries>") + 19);
                // sb = sb.Append("<?xml version=""1.0""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><soap:Body><wmLowFarePlusResponse xmlns=""tripxml.dowtowntravel.com/tripxml/wsLowFarePlus""><OTA_AirLowFareSearchPlusRS Version=""1.001"" TransactionIdentifier=""Amadeus-MIA1S21AV""><Success/><PricedItineraries>")

                sb = sb.Append(strHeader);
                string strFooter = strXML.Substring(strXML.IndexOf("</PricedItineraries>"), strXML.Length - strXML.IndexOf("</PricedItineraries>"));

                for (int i = 0, loopTo = cnt; i <= loopTo; i++)
                {
                    oNode = oRoot.ChildNodes[1].ChildNodes[i];
                    bool flag = false;

                    if (oNode is not null)
                    {
                        int icnt = oNode.ChildNodes[0].ChildNodes[0].ChildNodes.Count;

                        for (int j = 0, loopTo1 = icnt; j <= loopTo1; j++)
                        {
                            oInnerNode = oNode.ChildNodes[0].ChildNodes[0].ChildNodes[j];

                            if (oInnerNode is not null)
                            {
                                string strClass = oInnerNode.ChildNodes[0].Attributes["ResBookDesigCode"].Value;
                                string strAL = oInnerNode.ChildNodes[0].ChildNodes[4].Attributes["Code"].Value;

                                if (dtClasses.Select("Airline='" + strAL + "' and Class='" + strClass + "' and PreferLevel='0'").Length > 0)
                                {
                                    flag = false;
                                    j = icnt + 1;
                                }
                                else
                                {
                                    flag = true;

                                }
                            }
                        }
                        if (flag)
                        {
                            seqCounter = seqCounter + 1;
                            oNode.Attributes["SequenceNumber"].Value = seqCounter.ToString();
                            sb = sb.Append(oNode.OuterXml);
                        }
                    }
                }
                // sb = sb.Append("</PricedItineraries></OTA_AirLowFareSearchPlusRS>") '</wmLowFarePlusResponse></soap:Body></soap:Envelope>")

                if (seqCounter == 0)
                {
                    throw new Exception("NO ITINERARY FOUND FOR REQUESTED CLASSES OF SERVICE");
                }

                sb = sb.Append(strFooter);
                strResult = sb.ToString();
                sb = sb.Remove(0, sb.Length);
                return strResult;
            }
            else
            {
                return strResponse;
            }
        }

        /// <param name="strResponse">Response after converting to OTA format</param>
        /// <param name="dtClasses">DataTable of airlines and classes</param>
        /// <returns>Filtered response for given classes</returns>
        /// <remarks>By Shashin - 31/03/2010</remarks>
        private string FilterAirLineClasses(string strResponse, DataTable dtClasses)
        {
            if (strResponse.IndexOf("Success") != -1)
            {
                string strResult;
                string strXML;
                // Dim inStart As Integer = strResponse.IndexOf("<soap:Body>") + 11
                // Dim iLength As Integer = strResponse.IndexOf("</soap:Body>") - (strResponse.IndexOf("<soap:Body>") + 11)
                // strXML = "<wmLowFarePlusResponse xmlns=""tripxml.dowtowntravel.com/tripxml/wsLowFarePlus"">" + strResponse '.Substring(inStart, iLength)
                // strXML = "<wmLowFarePlusResponse xmlns=""tripxml.dowtowntravel.com/tripxml/wsLowFarePlus"">" + strResponse + "</wmLowFarePlusResponse>"
                strXML = strResponse;

                var oDoc = new XmlDocument();
                oDoc.LoadXml(strXML);
                XmlElement oRoot;
                oRoot = oDoc.DocumentElement;
                XmlNode oNode;
                XmlNode oInnerNode;
                int cnt;
                var seqCounter = default(int);
                cnt = oRoot.ChildNodes[1].ChildNodes.Count;
                var sb = new StringBuilder();
                string strHeader = strXML.Substring(0, strXML.IndexOf("<PricedItineraries>") + 19);
                // sb = sb.Append("<?xml version=""1.0""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><soap:Body><wmLowFarePlusResponse xmlns=""tripxml.dowtowntravel.com/tripxml/wsLowFarePlus""><OTA_AirLowFareSearchPlusRS Version=""1.001"" TransactionIdentifier=""Amadeus-MIA1S21AV""><Success/><PricedItineraries>")

                sb = sb.Append(strHeader);
                string strFooter = strXML.Substring(strXML.IndexOf("</PricedItineraries>"), strXML.Length - strXML.IndexOf("</PricedItineraries>"));

                for (int i = 0, loopTo = cnt; i <= loopTo; i++)
                {
                    oNode = oRoot.ChildNodes[1].ChildNodes[i];
                    bool flag = false;

                    if (oNode is not null)
                    {
                        int icnt = oNode.ChildNodes[0].ChildNodes[0].ChildNodes.Count;

                        for (int j = 0, loopTo1 = icnt; j <= loopTo1; j++)
                        {
                            oInnerNode = oNode.ChildNodes[0].ChildNodes[0].ChildNodes[j];

                            if (oInnerNode is not null)
                            {
                                string strClass = oInnerNode.ChildNodes[0].Attributes["ResBookDesigCode"].Value;
                                string strAL = oInnerNode.ChildNodes[0].ChildNodes[4].Attributes["Code"].Value;

                                if (dtClasses.Select("Airline='" + strAL + "' and Class='" + strClass + "' and PreferLevel='1'").Length > 0)
                                {
                                    flag = true;
                                }
                                else
                                {
                                    flag = false;
                                    j = icnt + 1;
                                }
                            }
                        }
                        if (flag)
                        {
                            seqCounter = seqCounter + 1;
                            oNode.Attributes["SequenceNumber"].Value = seqCounter.ToString();
                            sb = sb.Append(oNode.OuterXml);
                        }
                    }
                }
                // sb = sb.Append("</PricedItineraries></OTA_AirLowFareSearchPlusRS>") '</wmLowFarePlusResponse></soap:Body></soap:Envelope>")

                if (seqCounter == 0)
                {
                    throw new Exception("NO ITINERARY FOUND WITH REQUESTED CLASSES OF SERVICE");
                }

                sb = sb.Append(strFooter);
                strResult = sb.ToString();
                sb = sb.Remove(0, sb.Length);
                return strResult;
            }
            else
            {
                return strResponse;
            }
        }

        /// <summary> 
        /// Method to filter response for given classes for LowFareSchedule
        /// </summary>
        /// <param name="strResponse">Response after converting to OTA format</param>
        /// <param name="dtClasses">DataTable of airlines and classes</param>
        /// <returns>Filtered response for given classes</returns>
        /// <remarks>By Shashin - 05/04/2010</remarks>
        private string FilterAirLineClasses_LFS_remove(string strResponse, DataTable dtClasses)
        {
            string strResult;
            if (strResponse.IndexOf("Success") != -1)
            {
                var oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                XmlElement oRoot;
                oRoot = oDoc.DocumentElement;
                XmlNode oNode;
                XmlNode oInnerNode;
                int cnt = oRoot.ChildNodes[1].ChildNodes.Count;
                int seqCounter = 1;
                var sbResult = new StringBuilder();
                var sbFinal = new StringBuilder();
                var dtReturns = new DataTable("Results");
                dtReturns.Columns.Add("Index", Type.GetType("System.Int32"));
                dtReturns.Columns.Add("RPH", Type.GetType("System.Int32"));
                dtReturns.Columns.Add("Flag");
                string strAL = "";
                string strClass = "";
                var dtFinal = new DataTable();
                dtFinal.Columns.Add("Index");
                dtFinal.Columns.Add("RPH");
                var dc = new DataColumn[3];
                dc[0] = dtFinal.Columns["Index"];
                dc[1] = dtFinal.Columns["RPH"];
                dtFinal.PrimaryKey = dc;
                var dtIndex = new DataTable();
                dtIndex.Columns.Add("Index");
                var dc1 = new DataColumn[2];
                dc1[0] = dtIndex.Columns["Index"];
                dtIndex.PrimaryKey = dc1;
                string strHeader = strResponse.Substring(0, strResponse.IndexOf("<PricedItineraries>") + 19);
                // sbFinal = sbFinal.Append("<OTA_AirLowFareSearchPlusRS Version=""1.001"" TransactionIdentifier=""Amadeus-MIA1S21AV\""><Success/><PricedItineraries>")
                sbFinal = sbFinal.Append(strHeader);
                string strFooter = strResponse.Substring(strResponse.IndexOf("</PricedItineraries>"), strResponse.Length - strResponse.IndexOf("</PricedItineraries>"));
                for (int i = 0, loopTo = cnt - 1; i <= loopTo; i++)
                {
                    oNode = oRoot.ChildNodes[1].ChildNodes[i];
                    int innnercnt1 = oNode.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
                    int innnercnt2 = oNode.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes.Count;

                    for (int j = 0, loopTo1 = innnercnt2; j <= loopTo1; j++)
                    {
                        oInnerNode = oNode.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[j];

                        if (oInnerNode is not null)
                        {
                            strAL = oInnerNode.ChildNodes[4].Attributes["Code"].Value;
                            strClass = oInnerNode.Attributes["ResBookDesigCode"].Value;

                            for (int k = 0, loopTo2 = innnercnt1 - 1; k <= loopTo2; k++)
                            {
                                var dr = dtReturns.NewRow();
                                // dr("Index") = Integer.Parse(oInnerNode.ChildNodes(5).ChildNodes(4 + k).Attributes("Index").Value)

                                dr["Index"] = int.Parse(oInnerNode.ChildNodes[5].SelectSingleNode("OriginClass").Attributes["Index"].Value);
                                dr["RPH"] = int.Parse(oInnerNode.Attributes["RPH"].Value);

                                if (dtClasses.Select("Airline='" + strAL + "' and Class='" + strClass + "' and PreferLevel='0'").Length > 0)
                                {
                                    // strClass = oInnerNode.ChildNodes(5).ChildNodes(4 + k).InnerText
                                    dr["Flag"] = "F";
                                }
                                else
                                {

                                    strClass = oInnerNode.ChildNodes[5].SelectSingleNode("OriginClass").InnerText;
                                    strAL = oNode.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[k].ChildNodes[2].Attributes["Code"].Value;
                                    if (dtClasses.Select("Airline='" + strAL + "' and Class='" + strClass + "' and PreferLevel='0'").Length > 0)
                                    {
                                        dr["Flag"] = "F";
                                    }
                                    else
                                    {
                                        dr["Flag"] = "T";
                                    }
                                }
                                dtReturns.Rows.Add(dr);
                            }
                        }

                    }
                    DataRow[] drArray = dtReturns.Select("Flag='T'");

                    if (drArray.Length > 0)
                    {
                        foreach (var dr in drArray)
                        {
                            string strIn = dr["Index"].ToString();
                            string strRPH = dr["RPH"].ToString();

                            if (dtReturns.Select("RPH=" + strRPH + " and Flag='F'").Length == 0)
                            {
                                var drTemp = dtFinal.NewRow();
                                drTemp["RPH"] = strRPH;
                                drTemp["Index"] = strIn;
                                try
                                {
                                    dtFinal.Rows.Add(drTemp);
                                }
                                catch (Exception ex)
                                {

                                }
                                var drTemp2 = dtIndex.NewRow();
                                drTemp2["Index"] = strIn;
                                try
                                {
                                    dtIndex.Rows.Add(drTemp2);
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                        }

                        if (dtIndex.Rows.Count > 0)
                        {
                            sbResult = sbResult.Append("<PricedItinerary SequenceNumber=\"" + seqCounter.ToString() + "\"><AirItinerary><OriginDestinationOptions><OriginDestinationOption>");
                            foreach (DataRow dr1 in dtIndex.Rows)
                                sbResult = sbResult.Append(oNode.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[int.Parse(dr1["Index"].ToString()) - 1].OuterXml);
                            sbResult = sbResult.Append("</OriginDestinationOption><OriginDestinationOption>");

                            for (int j = 0, loopTo3 = innnercnt2; j <= loopTo3; j++)
                            {
                                oInnerNode = oNode.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[j];

                                if (oInnerNode is not null)
                                {
                                    strAL = oInnerNode.ChildNodes[2].Attributes["Code"].Value;
                                    strClass = oInnerNode.Attributes["ResBookDesigCode"].Value;
                                    string strIN = oInnerNode.ChildNodes[5].SelectSingleNode("OriginClass").Attributes["Index"].Value;
                                    string strRPH = oInnerNode.Attributes["RPH"].Value;

                                    if (dtFinal.Select("Index='" + strIN + "' and RPH='" + strRPH + "'").Length > 0)
                                    {
                                        sbResult = sbResult.Append(oInnerNode.OuterXml);
                                    }
                                }
                            }
                            sbResult = sbResult.Append("</OriginDestinationOption></OriginDestinationOptions></AirItinerary>");
                            sbResult = sbResult.Append(oNode.ChildNodes[1].OuterXml);
                            sbResult = sbResult.Append("</PricedItinerary>");
                            seqCounter = seqCounter + 1;
                        }
                    }
                    dtReturns.Rows.Clear();
                    dtFinal.Rows.Clear();
                    dtIndex.Clear();
                    sbFinal = sbFinal.Append(sbResult.ToString());
                    sbResult = sbResult.Remove(0, sbResult.Length);

                }

                // sbFinal = sbFinal.Append("</PricedItineraries></OTA_AirLowFareSearchPlusRS>")

                if (seqCounter == 1)
                {
                    throw new Exception("NO ITINERARY FOUND FOR REQUESTED CLASSES OF SERVICE");
                }

                sbFinal = sbFinal.Append(strFooter);
                strResult = sbFinal.ToString();
                sbFinal.Remove(0, sbFinal.Length);
                return strResult;
            }
            else
            {
                return strResponse;
            }
        }

        /// <summary> 
        /// Method to filter response for given classes for LowFareSchedule
        /// </summary>
        /// <param name="strResponse">Response after converting to OTA format</param>
        /// <param name="dtClasses">DataTable of airlines and classes</param>
        /// <returns>Filtered response for given classes</returns>
        /// <remarks>By Shashin - 05/04/2010</remarks>
        private string FilterAirLineClasses_LFS(string strResponse, DataTable dtClasses)
        {
            string strResult;
            if (strResponse.IndexOf("Success") != -1)
            {
                var oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                XmlElement oRoot;
                oRoot = oDoc.DocumentElement;
                XmlNode oNode;
                XmlNode oInnerNode;
                int cnt = oRoot.ChildNodes[1].ChildNodes.Count;
                int seqCounter = 1;
                var sbResult = new StringBuilder();
                var sbFinal = new StringBuilder();
                var dtReturns = new DataTable("Results");
                dtReturns.Columns.Add("Index", Type.GetType("System.Int32"));
                dtReturns.Columns.Add("RPH", Type.GetType("System.Int32"));
                dtReturns.Columns.Add("Flag");
                string strAL = "";
                string strClass = "";
                var dtFinal = new DataTable();
                dtFinal.Columns.Add("Index");
                dtFinal.Columns.Add("RPH");
                var dc = new DataColumn[3];
                dc[0] = dtFinal.Columns["Index"];
                dc[1] = dtFinal.Columns["RPH"];
                dtFinal.PrimaryKey = dc;
                var dtIndex = new DataTable();
                dtIndex.Columns.Add("Index");
                var dc1 = new DataColumn[2];
                dc1[0] = dtIndex.Columns["Index"];
                dtIndex.PrimaryKey = dc1;
                string strHeader = strResponse.Substring(0, strResponse.IndexOf("<PricedItineraries>") + 19);
                // sbFinal = sbFinal.Append("<OTA_AirLowFareSearchPlusRS Version=""1.001"" TransactionIdentifier=""Amadeus-MIA1S21AV\""><Success/><PricedItineraries>")
                sbFinal = sbFinal.Append(strHeader);
                string strFooter = strResponse.Substring(strResponse.IndexOf("</PricedItineraries>"), strResponse.Length - strResponse.IndexOf("</PricedItineraries>"));
                for (int i = 0, loopTo = cnt - 1; i <= loopTo; i++)
                {
                    oNode = oRoot.ChildNodes[1].ChildNodes[i];
                    int innnercnt1 = oNode.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes.Count;
                    int innnercnt2 = oNode.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes.Count;

                    for (int j = 0, loopTo1 = innnercnt2; j <= loopTo1; j++)
                    {
                        oInnerNode = oNode.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[j];

                        if (oInnerNode is not null)
                        {
                            strAL = oInnerNode.ChildNodes[4].Attributes["Code"].Value;
                            strClass = oInnerNode.Attributes["ResBookDesigCode"].Value;

                            for (int k = 0, loopTo2 = innnercnt1 - 1; k <= loopTo2; k++)
                            {
                                var dr = dtReturns.NewRow();
                                // dr("Index") = Integer.Parse(oInnerNode.ChildNodes(5).ChildNodes(4 + k).Attributes("Index").Value)

                                dr["Index"] = int.Parse(oInnerNode.ChildNodes[5].SelectSingleNode("OriginClass").Attributes["Index"].Value);
                                dr["RPH"] = int.Parse(oInnerNode.Attributes["RPH"].Value);

                                if (dtClasses.Select("Airline='" + strAL + "' and Class='" + strClass + "' and PreferLevel='1'").Length > 0)
                                {
                                    // strClass = oInnerNode.ChildNodes(5).ChildNodes(4 + k).InnerText
                                    strClass = oInnerNode.ChildNodes[5].SelectSingleNode("OriginClass").InnerText;
                                    strAL = oNode.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[k].ChildNodes[2].Attributes["Code"].Value;
                                    if (dtClasses.Select("Airline='" + strAL + "' and Class='" + strClass + "' and PreferLevel='1'").Length > 0)
                                    {
                                        dr["Flag"] = "T";
                                    }
                                    else
                                    {
                                        dr["Flag"] = "F";
                                    }
                                }
                                else
                                {
                                    dr["Flag"] = "F";
                                }
                                dtReturns.Rows.Add(dr);
                            }
                        }

                    }
                    DataRow[] drArray = dtReturns.Select("Flag='T'");

                    if (drArray.Length > 0)
                    {
                        foreach (var dr in drArray)
                        {
                            string strIn = dr["Index"].ToString();
                            string strRPH = dr["RPH"].ToString();

                            if (dtReturns.Select("RPH=" + strRPH + " and Flag='F'").Length == 0)
                            {
                                var drTemp = dtFinal.NewRow();
                                drTemp["RPH"] = strRPH;
                                drTemp["Index"] = strIn;
                                try
                                {
                                    dtFinal.Rows.Add(drTemp);
                                }
                                catch (Exception ex)
                                {

                                }
                                var drTemp2 = dtIndex.NewRow();
                                drTemp2["Index"] = strIn;
                                try
                                {
                                    dtIndex.Rows.Add(drTemp2);
                                }
                                catch (Exception ex)
                                {

                                }
                            }

                        }

                        if (dtIndex.Rows.Count > 0)
                        {
                            sbResult = sbResult.Append("<PricedItinerary SequenceNumber=\"" + seqCounter.ToString() + "\"><AirItinerary><OriginDestinationOptions><OriginDestinationOption>");
                            foreach (DataRow dr1 in dtIndex.Rows)
                                sbResult = sbResult.Append(oNode.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[int.Parse(dr1["Index"].ToString()) - 1].OuterXml);
                            sbResult = sbResult.Append("</OriginDestinationOption><OriginDestinationOption>");

                            for (int j = 0, loopTo3 = innnercnt2; j <= loopTo3; j++)
                            {
                                oInnerNode = oNode.ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[j];

                                if (oInnerNode is not null)
                                {
                                    strAL = oInnerNode.ChildNodes[2].Attributes["Code"].Value;
                                    strClass = oInnerNode.Attributes["ResBookDesigCode"].Value;
                                    string strIN = oInnerNode.ChildNodes[5].SelectSingleNode("OriginClass").Attributes["Index"].Value;
                                    string strRPH = oInnerNode.Attributes["RPH"].Value;

                                    if (dtFinal.Select("Index='" + strIN + "' and RPH='" + strRPH + "'").Length > 0)
                                    {
                                        sbResult = sbResult.Append(oInnerNode.OuterXml);
                                    }
                                }
                            }
                            sbResult = sbResult.Append("</OriginDestinationOption></OriginDestinationOptions></AirItinerary>");
                            sbResult = sbResult.Append(oNode.ChildNodes[1].OuterXml);
                            sbResult = sbResult.Append("</PricedItinerary>");
                            seqCounter = seqCounter + 1;
                        }
                    }
                    dtReturns.Rows.Clear();
                    dtFinal.Rows.Clear();
                    dtIndex.Clear();
                    sbFinal = sbFinal.Append(sbResult.ToString());
                    sbResult = sbResult.Remove(0, sbResult.Length);

                }

                // sbFinal = sbFinal.Append("</PricedItineraries></OTA_AirLowFareSearchPlusRS>")

                if (seqCounter == 1)
                {
                    throw new Exception("NO ITINERARY FOUND FOR REQUESTED CLASSES OF SERVICE");
                }

                sbFinal = sbFinal.Append(strFooter);
                strResult = sbFinal.ToString();
                sbFinal.Remove(0, sbFinal.Length);
                return strResult;
            }
            else
            {
                return strResponse;
            }
        }

        public string BusinessLogic(string strResponse, string strBusiness, string xslPath, string strMsg, string strExt)
        {

            if (strResponse.Contains("<Success />") | strResponse.Contains("<Success></Success>"))
            {
                var sb = new StringBuilder();
                sb.Append(strBusiness).Append("<Success />");
                strResponse = strResponse.Replace("<Success />", sb.ToString());
                sb.Remove(0, sb.Length);

                sb.Append(strBusiness).Append("<Success></Success>");
                strResponse = strResponse.Replace("<Success></Success>", sb.ToString());
                sb.Remove(0, sb.Length);

                sb.Append("Before ").Append(strMsg).Append(" business logic");
                CoreLib.SendTrace("", "cServiceAmadeus", sb.ToString(), strResponse, string.Empty);
                sb.Remove(0, sb.Length);

                sb.Append(Version).Append("BL_").Append(strMsg).Append(strExt).Append("RS.xsl");
                strResponse = CoreLib.TransformXML(strResponse, xslPath, sb.ToString());
                sb = null;
            }

            return strResponse;
        }

        public string BusinessLogicIn(string strRequest, string strBusiness, string xslPath, string strMsg)
        {
            var sb = new StringBuilder();
            sb.Append(strBusiness).Append("</OTA_AirLowFareSearchRQ>");
            strRequest = strRequest.Replace("</OTA_AirLowFareSearchRQ>", sb.ToString());
            sb.Remove(0, sb.Length);

            sb.Append(strBusiness).Append("</OTA_AirLowFareSearchPlusRQ>");
            strRequest = strRequest.Replace("</OTA_AirLowFareSearchPlusRQ>", sb.ToString());
            sb.Remove(0, sb.Length);

            sb.Append(strBusiness).Append("</OTA_AirLowFareSearchMatrixRQ>");
            strRequest = strRequest.Replace("</OTA_AirLowFareSearchMatrixRQ>", sb.ToString());
            sb.Remove(0, sb.Length);

            sb.Append(strBusiness).Append("</OTA_AirAvailRQ>");
            strRequest = strRequest.Replace("</OTA_AirAvailRQ>", sb.ToString());
            sb.Remove(0, sb.Length);

            sb.Append("Before ").Append(strMsg).Append(" business logic");
            CoreLib.SendTrace("", "cServiceAmadeus", sb.ToString(), strRequest, string.Empty);
            sb.Remove(0, sb.Length);

            sb.Append(Version).Append("BL_").Append(strMsg).Append("RQ.xsl");
            strRequest = CoreLib.TransformXML(strRequest, xslPath, sb.ToString());
            sb = null;

            return strRequest;
        }

    }
}