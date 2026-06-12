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
    public partial class wsCruiseCabinAvail
    {

        private readonly modMain _modMain;

        public wsCruiseCabinAvail(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 
        private StringBuilder sb = new StringBuilder();

        private string mstrShipCode = "";
        private string mstrDepartureDate = "";
        private string mstrDuration = "";

        private string DecodeCruiseCabinAvail(string strResponse, string UserID)
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

                    oNode = oRoot.SelectSingleNode("SailingInfo/SelectedSailing");
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
                CoreLib.SendTrace(UserID, "wsCruiseCabinAvail", "Error *** Decoding CruiseCabinAvail Response", ex.Message, string.Empty);
            }
            return strResponse;
        }

        private string FilterCruiseCabinAvail(string strRequest, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            DataView ttCruiseLines;
            DataView ttCruiseMot;
            DataView ttCruiseShips;
            DataView ttCruiseProfiles;
            DataView ttCruiseCities;
            DataView ttCruiseCurrency;
            DataView ttCruiseCabinFilter;
            string VendorCode = "";
            int intQuantity = 0;
            int intMaxGuestPerCabin = 0;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;

                ttCruiseLines = (DataView)TripXMLMain.AppState.Get("ttCruiseLines");
                ttCruiseMot = (DataView)TripXMLMain.AppState.Get("ttCruiseMot");
                ttCruiseShips = (DataView)TripXMLMain.AppState.Get("ttCruiseShips");
                ttCruiseProfiles = (DataView)TripXMLMain.AppState.Get("ttCruiseProfiles");
                ttCruiseCities = (DataView)TripXMLMain.AppState.Get("ttCruiseCities");
                ttCruiseCurrency = (DataView)TripXMLMain.AppState.Get("ttCruiseCurrency");
                ttCruiseCabinFilter = (DataView)TripXMLMain.AppState.Get("ttCruiseCabinFilter");

                // Check Cruise Line Code - Vendor Code
                oNode = oRoot.SelectSingleNode("SailingInfo/SelectedSailing");

                if (oNode.Attributes["VendorCode"] is null)
                {
                    throw new Exception("Invalid request. Cruise line vendor code is mandatory for this message.");
                }

                VendorCode = oNode.Attributes["VendorCode"].Value;
                if (!modMain.IsDecodeValue(ref ttCruiseLines, ref VendorCode))
                {
                    throw new Exception(sb.Append("Invalid Cruise Line Code - ").Append(VendorCode).ToString());
                    sb.Remove(0, sb.Length);
                }

                // Check ShipCode
                mstrShipCode = Conversions.ToString(modMain.IsNothing(oNode.Attributes["ShipCode"], ""));
                if (mstrShipCode.Length > 0)
                {
                    if (!modMain.IsCruiseFilterValue(ref ttCruiseShips, VendorCode, mstrShipCode))
                    {
                        throw new Exception(sb.Append("Invalid Ship code - ").Append(mstrShipCode).Append(" for cruise line ").Append(VendorCode).ToString());
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

                // Check Currency Code
                if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, VendorCode, "currencyRequiredFareAvailabilityRequest") == "true")
                {
                    if (oRoot.SelectSingleNode("SailingInfo/Currency") is null)
                    {
                        throw new Exception("Currency Code is mandatory for this Cruise line.");
                    }
                    else if (string.IsNullOrEmpty(oRoot.SelectSingleNode("SailingInfo/Currency")?.Attributes["CurrencyCode"]?.Value))
                    {
                        throw new Exception("Currency Code is mandatory for this Cruise line.");
                    }
                    else if (!modMain.IsCruiseFilterValue(ref ttCruiseCurrency, VendorCode, oRoot.SelectSingleNode("SailingInfo/Currency").Attributes["CurrencyCode"].Value))
                    {
                        throw new Exception(sb.Append("Currency code - ").Append(oRoot.SelectSingleNode("SailingInfo/Currency").Attributes["CurrencyCode"].Value).Append(" not supported by this cruise line ").Append(VendorCode).ToString());
                        sb.Remove(0, sb.Length);
                    }
                }

                // Check Voyage Number
                switch (VendorCode ?? "")
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

                // Get maxGuestPerCabin
                intMaxGuestPerCabin = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, VendorCode, "maxGuestPerCabin"));
                // Check for Number In Party
                intQuantity = 0;
                foreach (XmlNode currentONode in oRoot.SelectNodes("GuestCounts/GuestCount"))
                {
                    oNode = currentONode;
                    intQuantity += Conversions.ToInteger(modMain.IsNothing(oNode.Attributes["Quantity"], 0));
                }
                if (intQuantity > intMaxGuestPerCabin)
                {
                    throw new Exception("Maximum number of guest per cabin exceeded for specified cruise line.");
                }

                // Check MOT
                foreach (XmlNode currentONode1 in oRoot.SelectNodes("Guest"))
                {
                    oNode = currentONode1;
                    foreach (XmlNode oNodeGt in oNode.SelectNodes("GuestTransportation"))
                    {
                        if (!modMain.IsCruiseFilterValue(ref ttCruiseMot, VendorCode, oNodeGt.Attributes["TransportationMode"].Value))
                        {
                            throw new Exception(sb.Append("Invalid Transportation Mode - ").Append(oNodeGt.Attributes["TransportationMode"].Value).Append(" for cruise line ").Append(VendorCode).ToString());
                            sb.Remove(0, sb.Length);
                        }
                        if (!modMain.IsCruiseFilterValue(ref ttCruiseCities, VendorCode, oNodeGt.SelectSingleNode("GatewayCity").Attributes["LocationCode"].Value))
                        {
                            throw new Exception(sb.Append("Gateway location - ").Append(oNodeGt.SelectSingleNode("GatewayCity").Attributes["LocationCode"].Value).Append(" not supported by this cruise line ").Append(VendorCode).ToString());
                            sb.Remove(0, sb.Length);
                        }
                    }
                }

                // Check Cabin Filters
                foreach (XmlNode currentONode2 in oRoot.SelectNodes("SailingInfo/SelectedCategory/CabinFilters/CabinFilter"))
                {
                    oNode = currentONode2;
                    if (oNode.Attributes["CabinFilterCode"] is not null)
                    {
                        if (!modMain.IsCruiseFilterValue(ref ttCruiseCabinFilter, VendorCode, oNode.Attributes["CabinFilterCode"].Value))
                        {
                            throw new Exception(sb.Append("Cabin filter - ").Append(oNode.Attributes["CabinFilterCode"].Value).Append(" not supported by this cruise line ").Append(VendorCode).ToString());
                            sb.Remove(0, sb.Length);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsCruiseCabinAvail", "Error *** Filtering CruiseCabinAvail Request", ex.Message, string.Empty);
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

                // Validate Rules for CruiseCabinAvail
                strRequest = FilterCruiseCabinAvail(strRequest, ttCredential.UserID);

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

                strResponse = DecodeCruiseCabinAvail(strResponse, ttCredential.UserID);

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsCabinAvail", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 
        public wmCruiseCabinAvailOut.OTA_CruiseCabinAvailRS wmCruiseCabinAvail(wmCruiseCabinAvailIn.OTA_CruiseCabinAvailRQ OTA_CruiseCabinAvailRQ)
        {
            string xmlMessage = "";
            wmCruiseCabinAvailOut.OTA_CruiseCabinAvailRS oCruiseCabinAvailRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmCruiseCabinAvailIn.OTA_CruiseCabinAvailRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_CruiseCabinAvailRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CruiseCabinAvail);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmCruiseCabinAvailOut.OTA_CruiseCabinAvailRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oCruiseCabinAvailRS = (wmCruiseCabinAvailOut.OTA_CruiseCabinAvailRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsCabinAvail", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oCruiseCabinAvailRS;

        }
        public string wmCruiseCabinAvailXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.CruiseCabinAvail);
        }

        #endregion

    }

}