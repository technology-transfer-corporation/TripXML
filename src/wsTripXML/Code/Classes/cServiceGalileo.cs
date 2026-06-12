using System;
using System.Data;
using System.Text;
using System.Xml;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{

    public class cServiceGalileo
    {

        public event GotResponseEventHandler GotResponse;

        public delegate void GotResponseEventHandler(string Response);

        #region  Properties 
        private StringBuilder sb = new StringBuilder();
        private TripXMLProviderSystems ttProviderSystems;

        public int ServiceID { get; set; }

        public string Request { get; set; } = "";

        public string Version { get; set; } = "";

        public TripXMLProviderSystems ProviderSystems
        {
            get
            {
                return ttProviderSystems;
            }
            set
            {
                ttProviderSystems = value;
            }
        }

        #endregion

        public void SendAirRequest()
        {
            string strResponse = "";
            Galileo.AirServices ttService = null;
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            string strMsg = "";
            string strPCC = "";
            string strBLPCC = "";
            int i;

            XmlDocument oDocReq = null;
            XmlElement oRootReq = null;
            XmlDocument oDocReq1 = null;
            XmlElement oRootReq1 = null;
            XmlNode oBLNode = null;
            XmlAttribute oAttr;
            DataTable dt = null;

            try
            {
                ttService = new Galileo.AirServices();

                {
                    ref var withBlock = ref ttService;
                    withBlock.Version = Version;
                    withBlock.XslPath = XslPath;
                    withBlock.ProviderSystems = ttProviderSystems;

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

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Galileo/Apollo air services.");
                            }
                    }

                    if (!string.IsNullOrEmpty(withBlock.ProviderSystems.BLFile))
                    {
                        oDoc = new XmlDocument();

                        try
                        {
                            oDoc.Load(withBlock.ProviderSystems.BLFile);
                        }
                        catch (Exception exr)
                        {
                            CoreLib.SendTrace("", "cServiceGalileo", "Error Loading business logic file", exr.Message, ttProviderSystems.LogUUID);
                            throw exr;
                        }

                        oRoot = oDoc.DocumentElement;
                        // check "In" business logic
                        oNode = oRoot.SelectSingleNode("Message[@Name='" + strMsg + "'][@Direction='In']");

                        if (oNode is not null)
                        {
                            oNode = oNode.SelectSingleNode("ProviderBL[@Name='Galileo'][@System='" + withBlock.ProviderSystems.System + "'][@PCC='" + withBlock.ProviderSystems.PCC.ToUpper() + "']");

                            if (oNode is not null)
                            {
                                Request = BusinessLogicIn(Request, oNode.OuterXml, XslPath + @"BL\", strMsg);
                            }
                            else
                            {
                                oNode = oRoot.SelectSingleNode("Message[@Name='" + strMsg + "'][@Direction='In']");
                                oNode = oNode.SelectSingleNode("ProviderBL[@Name='Galileo'][@System='" + withBlock.ProviderSystems.System + "'][contains(@PCC,'*')]");

                                if (oNode is not null)
                                {
                                    strBLPCC = oNode.SelectSingleNode("@PCC").InnerText;
                                    strPCC = withBlock.ProviderSystems.PCC.ToUpper();

                                    if (oNode.SelectSingleNode("Preferences/Except[PCC='" + strPCC + "']") is null)
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
                                                Request = BusinessLogicIn(Request, oNode.OuterXml, XslPath + @"BL\", strMsg);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        oDoc = null;
                        oRoot = null;
                        oNode = null;
                    }

                    withBlock.Request = Request;

                    switch (ServiceID)
                    {
                        case (int)ttServices.AirAvail:
                            {
                                strResponse = withBlock.AirAvail();
                                break;
                            }
                        case (int)ttServices.LowFare:
                            {
                                strResponse = withBlock.LowFare();
                                break;
                            }
                        case (int)ttServices.LowFarePlus:
                            {
                                strResponse = withBlock.LowFarePlus();
                                break;
                            }
                        case (int)ttServices.LowFareSchedule:
                            {
                                strResponse = withBlock.LowFareSchedule();
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Galileo/Apollo air services.");
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
                                    break;
                                }
                                // strResponse = FilterAirLineClasses_LFS(strResponse, dt)
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
                                    break;
                                }
                                // strResponse = FilterAirLineClasses_LFS_remove(strResponse, dt)
                        }
                    }

                    // '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    if (ttProviderSystems.PCC.Length > 0)
                    {
                        strResponse = strResponse.Replace("TransactionIdentifier=\"Galileo", sb.Append("TransactionIdentifier=\"Galileo").Append("-").Append(ttProviderSystems.PCC).ToString());
                        sb.Remove(0, sb.Length);
                    }

                    if (!string.IsNullOrEmpty(withBlock.ProviderSystems.BLFile))
                    {
                        oDoc = new XmlDocument();
                        // Load Access Control List into memory
                        try
                        {
                            oDoc.Load(withBlock.ProviderSystems.BLFile);
                        }
                        catch (Exception exr)
                        {
                            CoreLib.SendTrace("", "cServiceGalileo", "Error Loading business logic file", exr.Message, ttProviderSystems.LogUUID);
                            throw exr;
                        }

                        oRoot = oDoc.DocumentElement;
                        oNode = oRoot.SelectSingleNode(sb.Append("Message[@Name='").Append(strMsg).Append("'][@Direction='Out']").ToString());
                        sb.Remove(0, sb.Length);

                        if (oNode is not null)
                        {
                            oNode = oNode.SelectSingleNode(sb.Append("ProviderBL[@Name='Galileo'][@System='").Append(withBlock.ProviderSystems.System).Append("'][@PCC='").Append(withBlock.ProviderSystems.PCC.ToUpper()).Append("']").ToString());
                            sb.Remove(0, sb.Length);

                            if (oNode is not null)
                            {
                                strResponse = BusinessLogic(strResponse, oNode.OuterXml, sb.Append(XslPath).Append(@"BL\").ToString(), strMsg);
                                sb.Remove(0, sb.Length);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage((ttServices)ServiceID, ex.Message, "Galileo", "", false, Version);
            }
            finally
            {
                if (ttService is not null)
                    ttService = null;
                GotResponse?.Invoke(strResponse);
            }
            sb = null;
        }

        public void SendCarRequest()
        {
            string strResponse = "";
            Galileo.CarServices ttService = null;

            try
            {
                ttService = new Galileo.CarServices();

                {
                    ref var withBlock = ref ttService;
                    withBlock.Version = Version;
                    withBlock.XslPath = XslPath;
                    withBlock.ProviderSystems = ttProviderSystems;
                    withBlock.Request = Request;

                    switch (ServiceID)
                    {
                        case (int)ttServices.CarAvail:
                            {
                                strResponse = withBlock.CarAvail();
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Galileo/Apollo car services.");
                            }
                    }

                }
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage((ttServices)ServiceID, ex.Message, "Galileo", "", false, Version);
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
            string strResponse = "";
            Galileo.HotelServices ttService = null;

            try
            {
                ttService = new Galileo.HotelServices();

                {
                    ref var withBlock = ref ttService;
                    withBlock.Version = Version;
                    withBlock.XslPath = XslPath;
                    withBlock.ProviderSystems = ttProviderSystems;
                    withBlock.Request = Request;

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
                                throw new Exception("Invalid request or message not supported by Galileo/Apollo hotel services.");
                            }
                    }

                }
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage((ttServices)ServiceID, ex.Message, "Galileo", "", false, Version);
            }
            finally
            {
                if (ttService is not null)
                    ttService = null;
                GotResponse?.Invoke(strResponse);
            }

        }

        public void SendTravelRequest()
        {
            string strResponse = "";
            Galileo.TravelServices ttService = null;

            try
            {
                ttService = new Galileo.TravelServices();

                {
                    ref var withBlock = ref ttService;
                    withBlock.Version = Version;
                    withBlock.XslPath = XslPath;
                    withBlock.ProviderSystems = ttProviderSystems;
                    withBlock.Request = Request;

                    switch (ServiceID)
                    {
                        case (int)ttServices.TravelBuild:
                            {
                                strResponse = withBlock.TravelBuild();
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Galileo/Apollo Travel services.");
                            }
                    }

                }
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage((ttServices)ServiceID, ex.Message, "Galileo", "", false, Version);
            }
            finally
            {
                if (ttService is not null)
                    ttService = null;
                GotResponse?.Invoke(strResponse);
            }

        }

        private string FilterAirLineClasses_remove(string strResponse, DataTable dtClasses)
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
                    throw new Exception("NO ITINERARY FOUND FOR REQUESTED SEGMENT 1");
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

                                if (dtClasses.Select("Airline='" + strAL + "' and Class='" + strClass + "'").Length > 0)
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
                    throw new Exception("NO ITINERARY FOUND FOR REQUESTED SEGMENT 1");
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

        public string BusinessLogic(string strResponse, string strBusiness, string xslPath, string strMsg)
        {

            // If strResponse.IndexOf("<Success />") <> -1 Or strResponse.IndexOf("<Success></Success>") Then
            // strResponse = strResponse.Replace("<Success />", sb.Append(strBusiness).Append("<Success />").ToString())
            // sb.Remove(0, sb.Length())
            // strResponse = strResponse.Replace("<Success></Success>", sb.Append(strBusiness).Append("<Success></Success>").ToString())
            // sb.Remove(0, sb.Length())
            // CoreLib.SendTrace("", "cServiceGalileo", sb.Append("Before ").Append(strMsg).Append(" business logic").ToString(), strResponse, ttProviderSystems.LogUUID)
            // sb.Remove(0, sb.Length())
            // strResponse = CoreLib.TransformXML(strResponse, xslPath, sb.Append(Version).Append("BL_").Append(strMsg).Append("RS.xsl").ToString())
            // sb.Remove(0, sb.Length())
            // End If
            if (strResponse.IndexOf("<Success />") != -1 || strResponse.IndexOf("<Success></Success>") != -1)
            {
                sb.Clear();
                sb.Append(strBusiness).Append("<Success />");
                strResponse = strResponse.Replace("<Success />", sb.ToString());

                sb.Clear();
                sb.Append(strBusiness).Append("<Success></Success>");
                strResponse = strResponse.Replace("<Success></Success>", sb.ToString());

                sb.Clear();
                sb.Append("Before ").Append(strMsg).Append(" business logic");
                CoreLib.SendTrace("", "cServiceGalileo", sb.ToString(), strResponse, ttProviderSystems.LogUUID);

                sb.Clear();
                sb.Append(Version).Append("BL_").Append(strMsg).Append("RS.xsl");
                strResponse = CoreLib.TransformXML(strResponse, xslPath, sb.ToString());

                sb.Clear();
            }

            return strResponse;
            sb = null;
        }

        // Public Function BusinessLogic(ByVal strResponse As String, ByVal strBusiness As String, ByVal xslPath As String, ByVal strMsg As String, ByVal strExt As String) As String

        // If strResponse.IndexOf("<Success />") <> -1 Or strResponse.IndexOf("<Success></Success>") Then
        // strResponse = strResponse.Replace("<Success />", strBusiness & "<Success />")
        // strResponse = strResponse.Replace("<Success></Success>", strBusiness & "<Success></Success>")
        // CoreLib.SendTrace("", "cServiceGalileo", "Before " & strMsg & " business logic", strResponse, ttProviderSystems.LogUUID)
        // strResponse = CoreLib.TransformXML(strResponse, xslPath, Version & "BL_" & strMsg & strExt & "RS.xsl")
        // End If

        // Return strResponse
        // End Function
        public string BusinessLogic(string strResponse, string strBusiness, string xslPath, string strMsg, string strExt)
        {
            if (strResponse.IndexOf("<Success />") != -1 || strResponse.IndexOf("<Success></Success>") != -1)
            {
                strResponse = strResponse.Replace("<Success />", strBusiness + "<Success />");
                strResponse = strResponse.Replace("<Success></Success>", strBusiness + "<Success></Success>");
                CoreLib.SendTrace("", "cServiceGalileo", "Before " + strMsg + " business logic", strResponse, ttProviderSystems.LogUUID);
                strResponse = CoreLib.TransformXML(strResponse, xslPath, Version + "BL_" + strMsg + strExt + "RS.xsl");
            }
            return strResponse;
        }

        public string BusinessLogicIn(string strRequest, string strBusiness, string xslPath, string strMsg)
        {

            strRequest = strRequest.Replace("</OTA_AirLowFareSearchRQ>", strBusiness + "</OTA_AirLowFareSearchRQ>");
            strRequest = strRequest.Replace("</OTA_AirLowFareSearchPlusRQ>", strBusiness + "</OTA_AirLowFareSearchPlusRQ>");
            strRequest = strRequest.Replace("</OTA_AirAvailRQ>", strBusiness + "</OTA_AirAvailRQ>");
            CoreLib.SendTrace("", "cServiceGalileo", "Before " + strMsg + " business logic", strRequest, ttProviderSystems.LogUUID);
            strRequest = CoreLib.TransformXML(strRequest, xslPath, Version + "BL_" + strMsg + "RQ.xsl");

            return strRequest;
        }

    }
}