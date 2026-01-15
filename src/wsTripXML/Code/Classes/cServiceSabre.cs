using System;
using System.Data;
using System.Text;
using System.Xml;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{


    public class cServiceSabre
    {

        public event GotResponseEventHandler GotResponse;

        public delegate void GotResponseEventHandler(string Response);

        #region  Properties 
        private StringBuilder sb = new StringBuilder();


        public ttServices ServiceID { get; set; }
        public string Request { get; set; } = "";
        public string Version { get; set; } = "";
        public TripXMLProviderSystems ProviderSystems { get; set; }
        public DataView ttCities { get; set; }

        #endregion

        public void SendAirRequest()
        {
            string strResponse = "";
            Sabre.AirServices ttService = null;
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            string strMsg = "";
            XmlNode oBLNode = null;
            string strPCC = "";

            XmlDocument oDocReq = null;
            XmlElement oRootReq = null;
            XmlDocument oDocReq1 = null;
            XmlElement oRootReq1 = null;

            XmlAttribute oAttr;
            DataTable dt = null;

            try
            {
                ttService = new Sabre.AirServices();

                {
                    ref var withBlock = ref ttService;
                    withBlock.Version = Version;
                    withBlock.XslPath = XslPath;
                    withBlock.ProviderSystems = ProviderSystems;
                    withBlock.Request = Request;

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
                    foreach (XmlNode oNode1 in oRootReq.SelectNodes("TravelPreferences/VendorPref"))
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
                        case ttServices.AirAvail:
                            {
                                strResponse = withBlock.AirAvail();
                                strMsg = "AirAvail";
                                break;
                            }
                        case ttServices.LowFare:
                            {
                                var argttCities = ttCities;
                                strResponse = withBlock.LowFare(ref argttCities);
                                ttCities = argttCities;
                                strMsg = "LowFare";
                                break;
                            }
                        case ttServices.LowFarePlus:
                            {
                                var argttCities1 = ttCities;
                                strResponse = withBlock.LowFarePlus(ref argttCities1);
                                ttCities = argttCities1;
                                strMsg = "LowFare";
                                break;
                            }
                        case ttServices.LowFareSchedule:
                            {
                                var argttCities2 = ttCities;
                                strResponse = withBlock.LowFareSchedule(ref argttCities2);
                                ttCities = argttCities2;
                                strMsg = "LowFare";
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Sabre air services.");
                            }
                    }

                    // '''''''''''''''''Air line class filtering -  shashin''''''''''''''''''

                    if (dt.Rows.Count > 0)
                    {
                        switch (ServiceID)
                        {
                            case ttServices.LowFare:
                            case ttServices.LowFarePlus:
                                {
                                    strResponse = FilterAirLineClasses(strResponse, dt);
                                    break;
                                }
                            case ttServices.LowFareSchedule:
                                {
                                    break;
                                }
                                // strResponse = FilterAirLineClasses_LFS(strResponse, dt)
                        }
                    }

                    // '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    if (ProviderSystems.AAAPCC.Length > 0)
                    {
                        strResponse = strResponse.Replace("TransactionIdentifier=\"Sabre", sb.Append("TransactionIdentifier=\"Sabre").Append("-").Append(ProviderSystems.AAAPCC).ToString());
                        sb.Remove(0, sb.Length);
                    }
                    else if (ProviderSystems.PCC.Length > 0)
                    {
                        strResponse = strResponse.Replace("TransactionIdentifier=\"Sabre", sb.Append("TransactionIdentifier=\"Sabre").Append("-").Append(ProviderSystems.PCC).ToString());
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
                            CoreLib.SendTrace("", "cServiceSabre", "Error Loading business logic file", exr.Message, ProviderSystems.LogUUID);
                            throw exr;
                        }

                        oRoot = oDoc.DocumentElement;
                        oNode = oRoot.SelectSingleNode(sb.Append("Message[@Name='").Append(strMsg).Append("'][@Direction='Out']").ToString());
                        sb.Remove(0, sb.Length);

                        if (oNode is not null)
                        {
                            if (!string.IsNullOrEmpty(ProviderSystems.AAAPCC))
                            {
                                strPCC = ProviderSystems.AAAPCC;
                            }
                            else
                            {
                                strPCC = ProviderSystems.PCC;
                            }
                            // check if non ticketable flights/fares to eliminate
                            sb.Append("NoTktAirline[@Name='Sabre'][@System='").Append(ProviderSystems.System).Append("'][@PCC='").Append(strPCC.ToUpper()).Append("']");
                            oBLNode = oNode.SelectSingleNode(sb.ToString());
                            sb.Remove(0, sb.Length);

                            if (oBLNode is not null)
                            {
                                sb.Append(XslPath).Append(@"BL\");
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, sb.ToString(), strMsg, "NoTkt");
                                sb.Remove(0, sb.Length);
                            }

                            // check if no mix airline to eliminate
                            sb.Append("NoMixAirline[@Name='Sabre'][@System='").Append(ProviderSystems.System).Append("'][@PCC='").Append(strPCC.ToUpper()).Append("']");
                            oBLNode = oNode.SelectSingleNode(sb.ToString());
                            sb.Remove(0, sb.Length);

                            if (oBLNode is not null)
                            {
                                sb.Append(XslPath).Append(@"BL\");
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, sb.ToString(), strMsg, "NoMix");
                                sb.Remove(0, sb.Length);
                            }

                            // check if No Fare Type to eliminate
                            sb.Append("NoFareType[@Name='Sabre'][@System='").Append(ProviderSystems.System).Append("'][@PCC='").Append(strPCC.ToUpper()).Append("']");
                            oBLNode = oNode.SelectSingleNode(sb.ToString());
                            sb.Remove(0, sb.Length);

                            if (oBLNode is not null)
                            {
                                sb.Append(XslPath).Append(@"BL\");
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, sb.ToString(), strMsg, "NoFareType");
                                sb.Remove(0, sb.Length);
                            }

                            // ' add fare markup if needed
                            oNode = oNode.SelectSingleNode(sb.Append("ProviderBL[@Name='Sabre'][@System='").Append(withBlock.ProviderSystems.System).Append("'][@PCC='").Append(strPCC.ToUpper()).Append("']").ToString());
                            sb.Remove(0, sb.Length);

                            if (oNode is not null)
                            {
                                strResponse = BusinessLogic(strResponse, oNode.OuterXml, sb.Append(XslPath).Append(@"BL\").ToString(), strMsg, "");
                                sb.Remove(0, sb.Length);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ServiceID, ex.Message, "Sabre", "", false, Version);
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
            Sabre.CarServices ttService = null;

            try
            {
                ttService = new Sabre.CarServices();

                {
                    ref var withBlock = ref ttService;
                    withBlock.Version = Version;
                    withBlock.XslPath = XslPath;
                    withBlock.ProviderSystems = ProviderSystems;
                    withBlock.Request = Request;

                    switch (ServiceID)
                    {
                        case ttServices.CarAvail:
                            {
                                strResponse = withBlock.CarAvail();
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Sabre car services.");
                            }
                    }

                }
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ServiceID, ex.Message, "Sabre", "", false, Version);
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
            Sabre.HotelServices ttService = null;

            try
            {
                ttService = new Sabre.HotelServices();

                {
                    ref var withBlock = ref ttService;
                    withBlock.Version = Version;
                    withBlock.XslPath = XslPath;
                    withBlock.ProviderSystems = ProviderSystems;
                    withBlock.Request = Request;

                    switch (ServiceID)
                    {
                        case ttServices.HotelAvail:
                            {
                                strResponse = withBlock.HotelAvail();
                                break;
                            }
                        case ttServices.HotelSearch:
                            {
                                strResponse = withBlock.HotelSearch();
                                break;
                            }

                        default:
                            {
                                throw new Exception("Invalid request or message not supported by Sabre hotel services.");
                            }
                    }

                }
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ServiceID, ex.Message, "Sabre", "", false, Version);
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


        public string BusinessLogic(string strResponse, string strBusiness, string xslPath, string strMsg, string strExt)
        {
            var sb1 = new StringBuilder();
            if (strResponse.IndexOf("<Success />") != -1 || strResponse.IndexOf("<Success></Success>") != -1)
            {
                strResponse = strResponse.Replace("<Success />", sb1.Append(strBusiness).Append("<Success />").ToString());
                sb1.Remove(0, sb1.Length);
                strResponse = strResponse.Replace("<Success></Success>", sb1.Append(strBusiness).Append("<Success></Success>").ToString());
                sb1.Remove(0, sb1.Length);
                CoreLib.SendTrace("", "cServiceSabre", sb1.Append("Before ").Append(strMsg).Append(" business logic").ToString(), strResponse, ProviderSystems.LogUUID);
                sb1.Remove(0, sb1.Length);
                strResponse = CoreLib.TransformXML(strResponse, xslPath, sb1.Append(Version).Append("BL_").Append(strMsg).Append(strExt).Append("RS.xsl").ToString());
                sb1.Remove(0, sb1.Length);
            }
            sb1 = null;
            return strResponse;
        }

    }
}