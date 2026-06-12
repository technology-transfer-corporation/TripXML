using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsCruisePackageDesc
    {

        private readonly modMain _modMain;

        public wsCruisePackageDesc(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 

        private string mstrVoyageID = "";
        private string mstrShipCode = "";
        private string mstrDepartureDate = "";
        private string mstrDuration = "";

        private string DecodeCruisePackageDesc(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            DataView ttCruiseLines;
            DataView ttCruiseShips;
            DataView ttCruiseAdvisory;
            DataView ttCruisePricedItems;
            XmlNode oNode = null;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                ttCruiseAdvisory = (DataView)TripXMLMain.AppState.Get("ttCruiseAdvisory");

                if (oRoot.SelectSingleNode("Errors") is null)
                {

                    ttCruiseLines = (DataView)TripXMLMain.AppState.Get("ttCruiseLines");
                    ttCruiseShips = (DataView)TripXMLMain.AppState.Get("ttCruiseShips");
                    ttCruisePricedItems = (DataView)TripXMLMain.AppState.Get("ttCruisePricedItems");

                    oNode = oRoot.SelectSingleNode("SailingInfo/SelectedSailing");
                    // VoyageID, Departure Date and Duration
                    if (string.IsNullOrEmpty(oNode.Attributes["VoyageID"].Value))
                    {
                        oNode.Attributes["VoyageID"].Value = mstrVoyageID;
                    }
                    if (string.IsNullOrEmpty(oNode.Attributes["Start"].Value))
                    {
                        oNode.Attributes["Start"].Value = mstrDepartureDate;
                    }
                    if (string.IsNullOrEmpty(oNode.Attributes["Duration"].Value))
                    {
                        oNode.Attributes["Duration"].Value = mstrDuration;
                    }
                    // *******************************
                    // Decode CruiseLines & Ships    *
                    // *******************************
                    string argstrCode = oNode.Attributes["VendorCode"].Value;
                    oNode.Attributes["VendorName"].Value = modMain.GetDecodeValue(ref ttCruiseLines, ref argstrCode);
                    oNode.Attributes["VendorCode"].Value = argstrCode;
                    if (string.IsNullOrEmpty(oNode.Attributes["ShipCode"].Value))
                    {
                        oNode.Attributes["ShipCode"].Value = mstrShipCode;
                    }
                    oNode.Attributes["ShipName"].Value = modMain.GetCruiseFilterValue(ref ttCruiseShips, oNode.Attributes["VendorCode"].Value, oNode.Attributes["ShipCode"].Value);

                    // *******************************
                    // Decode Advisory Errors Codes  *
                    // *******************************
                    foreach (XmlNode currentONode in oRoot.SelectNodes("Warnings/Warning"))
                    {
                        oNode = currentONode;
                        if (oNode.InnerText.Length == 0)
                        {
                            string argstrCode1 = oNode.Attributes["Code"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttCruiseAdvisory, ref argstrCode1);
                            oNode.Attributes["Code"].Value = argstrCode1;
                        }
                    }

                    // ***************************
                    // Decode Price Type Code    *
                    // ***************************
                    foreach (XmlNode currentONode1 in oRoot.SelectNodes("PackageOptions/PackageOption"))
                    {
                        oNode = currentONode1;
                        foreach (XmlNode oNodeChild in oNode.SelectNodes("PackagePrices/PackagePrice"))
                        {
                            string argstrCode2 = oNodeChild.Attributes["PriceTypeCode"].Value;
                            oNodeChild.Attributes["CodeDetail"].Value = modMain.GetDecodeValue(ref ttCruisePricedItems, ref argstrCode2);
                            oNodeChild.Attributes["PriceTypeCode"].Value = argstrCode2;
                        }
                    }
                }

                else
                {
                    // *******************************
                    // Decode Advisory Errors Codes  *
                    // *******************************
                    foreach (XmlNode currentONode2 in oRoot.SelectNodes("Errors/Error"))
                    {
                        oNode = currentONode2;
                        if (oNode.InnerText.Length == 0)
                        {
                            string argstrCode3 = oNode.Attributes["Code"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttCruiseAdvisory, ref argstrCode3);
                            oNode.Attributes["Code"].Value = argstrCode3;
                        }
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsCruisePackageDesc", "Error *** Decoding CruisePackageDesc Response", ex.Message, string.Empty);
            }
            return strResponse;
        }

        private string FilterCruisePackageDesc(string strRequest, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlNode oNodeGt = null;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;

                oNode = oRoot.SelectSingleNode("SailingInfo/SelectedSailing");

                // Get Some Info from the Request to Echo them back on the Response
                mstrVoyageID = oNode.Attributes["VoyageID"].Value;
                mstrShipCode = Conversions.ToString(modMain.IsNothing(oNode.Attributes["ShipCode"], ""));
                mstrDepartureDate = oNode.Attributes["Start"].Value;
                mstrDuration = oNode.Attributes["Duration"].Value;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsCruisePackageDesc", "Error *** Filtering CruisePackageDesc Request", ex.Message, string.Empty);
                throw ex;
            }
            return strRequest;
        }

        #endregion

        #region  Process Service Request All GDS 
        private StringBuilder sb = new StringBuilder();

        private string ServiceRequest(string strRequest, ttServices ttServiceID)
        {
            string strResponse = "";
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            bool ValidateXSDOut;
            var StartTime = default(DateTime);
            string UUID = "";

            try
            {
                StartTime = DateTime.Now;
                _modMain.PreServiceRequest(ref strRequest, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                // Validate Rules for CruisePackageDesc
                strRequest = FilterCruisePackageDesc(strRequest, ttCredential.UserID);

                switch (ttCredential.Providers[0].Name.ToLower() ?? "")
                {
                    case "amadeus":
                        {
                            break;
                        }
                    // Dim ttAA As AmadeusAPIAdapter

                    // ttAA = TripXMLMain.AppState.Get(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                    // sb.Remove(0, sb.Length())
                    // If ttAA Is Nothing Then
                    // Throw New Exception(sb.Append("Access denied to Amadeus - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString())
                    // sb.Remove(0, sb.Length())
                    // End If

                    // If ttCredential.Providers(0).PCC.Trim.Length > 0 Then
                    // ttAA.SourcePCC = ttCredential.Providers(0).PCC
                    // End If

                    // 'Send Reuest
                    // strResponse = SendCruiseRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                    // TripXMLMain.AppState.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                    // sb.Remove(0, sb.Length())

                    case "amadeusws":
                        {

                            strResponse = modMain.SendCruiseRequestAmadeusWS(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    case "apollo":
                    case "galileo":
                        {

                            strResponse = modMain.SendCruiseRequestGalileo(ttServiceID, ref ttCredential, ref ttProviderSystems, ref strRequest);
                            break;
                        }

                    default:
                        {
                            throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
                            sb.Remove(0, sb.Length);
                            break;
                        }
                }

                strResponse = DecodeCruisePackageDesc(strResponse, ttCredential.UserID);

                modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                _modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsPackageDesc", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 
        public wmCruisePackageDescOut.OTA_CruisePackageDescRS wmCruisePackageDesc(wmCruisePackageDescIn.OTA_CruisePackageDescRQ OTA_CruisePackageDescRQ)
        {
            string xmlMessage = "";
            wmCruisePackageDescOut.OTA_CruisePackageDescRS oCruisePackageDescRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmCruisePackageDescIn.OTA_CruisePackageDescRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_CruisePackageDescRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CruisePackageDesc);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmCruisePackageDescOut.OTA_CruisePackageDescRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oCruisePackageDescRS = (wmCruisePackageDescOut.OTA_CruisePackageDescRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsPackageDesc", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oCruisePackageDescRS;

        }
        public string wmCruisePackageDescXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.CruisePackageDesc);
        }

        #endregion

    }

}