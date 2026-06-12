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
    public partial class wsCruiseCabinUnUnhold
    {

        private readonly modMain _modMain;

        public wsCruiseCabinUnUnhold(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 
        private StringBuilder sb = new StringBuilder();

        private string mstrVendorCode = "";
        private string mstrShipCode = "";
        private string mstrDepartureDate = "";
        private string mstrDuration = "";
        private string mstrCabinNo = "";

        private string DecodeCruiseCabinUnhold(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            DataView ttCruiseLines;
            DataView ttCruiseShips;
            DataView ttCruiseAdvisory;
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

                    oNode = oRoot.SelectSingleNode("SelectedSailing");

                    // Departure Date and Duration
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
                    if (string.IsNullOrEmpty(oNode.Attributes["VendorCode"].Value))
                    {
                        oNode.Attributes["VendorCode"].Value = mstrVendorCode;
                    }
                    if (string.IsNullOrEmpty(oNode.Attributes["ShipCode"].Value))
                    {
                        oNode.Attributes["ShipCode"].Value = mstrShipCode;
                    }
                    string argstrCode = oNode.Attributes["VendorCode"].Value;
                    oNode.Attributes["VendorName"].Value = modMain.GetDecodeValue(ref ttCruiseLines, ref argstrCode);
                    oNode.Attributes["VendorCode"].Value = argstrCode;
                    oNode.Attributes["ShipName"].Value = modMain.GetCruiseFilterValue(ref ttCruiseShips, oNode.Attributes["VendorCode"].Value, oNode.Attributes["ShipCode"].Value);

                    if (string.IsNullOrEmpty(oNode.SelectSingleNode("SelectedCabin").Attributes["CabinNumber"].Value))
                    {
                        oNode.SelectSingleNode("SelectedCabin").Attributes["CabinNumber"].Value = mstrCabinNo;
                    }

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
                }
                else
                {
                    // *******************************
                    // Decode Advisory Errors Codes  *
                    // *******************************
                    foreach (XmlNode currentONode1 in oRoot.SelectNodes("Errors/Error"))
                    {
                        oNode = currentONode1;
                        if (oNode.InnerText.Length == 0)
                        {
                            string argstrCode2 = oNode.Attributes["Code"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttCruiseAdvisory, ref argstrCode2);
                            oNode.Attributes["Code"].Value = argstrCode2;
                        }
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsCruiseCabinUnhold", "Error *** Decoding CruiseCabinUnhold Response", ex.Message, string.Empty);
            }
            return strResponse;
        }

        private string FilterCruiseCabinUnhold(string strRequest, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlNode oNodeGt = null;
            DataView ttCruiseLines;
            DataView ttCruiseShips;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;

                ttCruiseLines = (DataView)TripXMLMain.AppState.Get("ttCruiseLines");
                ttCruiseShips = (DataView)TripXMLMain.AppState.Get("ttCruiseShips");

                // Check Cruise Line Code - Vendor Code
                oNode = oRoot.SelectSingleNode("SelectedSailing");

                if (oNode.Attributes["VendorCode"] is null)
                {
                    throw new Exception("Invalid request. Cruise line vendor code is mandatory for this message.");
                }

                mstrVendorCode = oNode.Attributes["VendorCode"].Value;
                if (!modMain.IsDecodeValue(ref ttCruiseLines, ref mstrVendorCode))
                {
                    throw new Exception(sb.Append("Invalid Cruise Line Code - ").Append(mstrVendorCode).ToString());
                    sb.Remove(0, sb.Length);
                }

                // Check ShipCode
                mstrShipCode = Conversions.ToString(modMain.IsNothing(oNode.Attributes["ShipCode"], ""));
                if (mstrShipCode.Length > 0)
                {
                    if (!modMain.IsCruiseFilterValue(ref ttCruiseShips, mstrVendorCode, mstrShipCode))
                    {
                        throw new Exception(sb.Append("Invalid Ship code - ").Append(mstrShipCode).Append(" for cruise line ").Append(mstrVendorCode).ToString());
                        sb.Remove(0, sb.Length);
                    }
                }
                else
                {
                    throw new Exception("Invalid request. Ship code is mandatory for this message.");
                }

                // Get Some Info from the Request to Echo them back on the Response
                mstrDepartureDate = oNode.Attributes["Start"].Value;
                mstrDuration = oNode.Attributes["Duration"].Value;
                mstrCabinNo = oNode.SelectSingleNode("SelectedCabin").Attributes["CabinNumber"].Value;

                // Check Voyage Number
                switch (mstrVendorCode ?? "")
                {
                    case "RCC":
                    case "CEL":
                    case "ICL":
                        {
                            if (string.Compare(Conversions.ToString(modMain.IsNothing(oNode.Attributes["VoyageID"], "")), modMain.CVoyageID) != 0)
                            {
                                throw new Exception(sb.Append("Invalid VoyageID number, it must be ").Append(modMain.CVoyageID).Append(".").ToString());
                                sb.Remove(0, sb.Length);
                            }

                            break;
                        }
                }
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsCruiseCabinUnhold", "Error *** Filtering CruiseCabinUnhold Request", ex.Message, string.Empty);
                throw ex;
            }
            return strRequest;
            sb = null;
        }

        #endregion

        #region  Process Service Request All GDS 

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

                // Validate Rules for CruiseCabinUnhold
                strRequest = FilterCruiseCabinUnhold(strRequest, ttCredential.UserID);

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

                strResponse = DecodeCruiseCabinUnhold(strResponse, ttCredential.UserID);

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsCruiseCabinUnhold", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 
        public wmCruiseCabinUnholdOut.OTA_CruiseCabinUnholdRS wmCruiseCabinUnhold(wmCruiseCabinUnholdIn.OTA_CruiseCabinUnholdRQ OTA_CruiseCabinUnholdRQ)
        {
            string xmlMessage = "";
            wmCruiseCabinUnholdOut.OTA_CruiseCabinUnholdRS oCruiseCabinUnholdRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmCruiseCabinUnholdIn.OTA_CruiseCabinUnholdRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_CruiseCabinUnholdRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CruiseCabinUnhold);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmCruiseCabinUnholdOut.OTA_CruiseCabinUnholdRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oCruiseCabinUnholdRS = (wmCruiseCabinUnholdOut.OTA_CruiseCabinUnholdRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsCruiseCabinUnhold", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oCruiseCabinUnholdRS;

        }
        public string wmCruiseCabinUnholdXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.CruiseCabinUnhold);
        }

        #endregion

    }

}