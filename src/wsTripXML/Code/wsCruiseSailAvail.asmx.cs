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
    public partial class wsCruiseSailAvail
    {
        private StringBuilder sb = new StringBuilder();

        private readonly modMain _modMain;

        public wsCruiseSailAvail(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Function 

        private string DecodeCruiseSailAvail(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            DataView ttwsCruiseCities;
            DataView ttCruiseRegions;
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

                    ttwsCruiseCities = (DataView)TripXMLMain.AppState.Get("ttwsCruiseCities");
                    ttCruiseRegions = (DataView)TripXMLMain.AppState.Get("ttCruiseRegions");
                    ttCruiseLines = (DataView)TripXMLMain.AppState.Get("ttCruiseLines");
                    ttCruiseShips = (DataView)TripXMLMain.AppState.Get("ttCruiseShips");

                    foreach (XmlNode currentONode in oRoot.SelectNodes("SailingOptions/SailingOption"))
                    {
                        oNode = currentONode;
                        // *******************************
                        // Decode CruiseLines & Ships    *
                        // *******************************
                        string argstrCode = oNode.SelectSingleNode("CruiseLine").Attributes["VendorCode"].Value;
                        oNode.SelectSingleNode("CruiseLine").Attributes["VendorName"].Value = modMain.GetDecodeValue(ref ttCruiseLines, ref argstrCode);
                        oNode.SelectSingleNode("CruiseLine").Attributes["VendorCode"].Value = argstrCode;
                        oNode.SelectSingleNode("CruiseLine").Attributes["ShipName"].Value = modMain.GetCruiseFilterValue(ref ttCruiseShips, oNode.SelectSingleNode("CruiseLine").Attributes["VendorCode"].Value, oNode.SelectSingleNode("CruiseLine").Attributes["ShipCode"].Value);

                        // *******************
                        // Decode Regions    *
                        // *******************
                        if (oNode.SelectSingleNode("Region") is not null)
                        {
                            string argstrCode1 = oNode.SelectSingleNode("Region").Attributes["RegionCode"].Value;
                            oNode.SelectSingleNode("Region").Attributes["RegionName"].Value = modMain.GetDecodeValue(ref ttCruiseRegions, ref argstrCode1);
                            oNode.SelectSingleNode("Region").Attributes["RegionCode"].Value = argstrCode1;
                        }

                        // ***********************
                        // Decode CruiseCities   *
                        // ***********************
                        if (oNode.SelectSingleNode("DeparturePort") is not null)
                        {
                            string argstrCode2 = oNode.SelectSingleNode("DeparturePort").Attributes["LocationCode"].Value;
                            oNode.SelectSingleNode("DeparturePort").InnerText = modMain.GetDecodeValue(ref ttwsCruiseCities, ref argstrCode2);
                            oNode.SelectSingleNode("DeparturePort").Attributes["LocationCode"].Value = argstrCode2;
                        }
                        if (oNode.SelectSingleNode("ArrivalPort") is not null)
                        {
                            string argstrCode3 = oNode.SelectSingleNode("ArrivalPort").Attributes["LocationCode"].Value;
                            oNode.SelectSingleNode("ArrivalPort").InnerText = modMain.GetDecodeValue(ref ttwsCruiseCities, ref argstrCode3);
                            oNode.SelectSingleNode("ArrivalPort").Attributes["LocationCode"].Value = argstrCode3;
                        }

                    }

                    // *******************************
                    // Decode Advisory Errors Codes  *
                    // *******************************
                    foreach (XmlNode currentONode1 in oRoot.SelectNodes("Warnings/Warning"))
                    {
                        oNode = currentONode1;
                        if (oNode.InnerText.Length == 0)
                        {
                            string argstrCode4 = oNode.Attributes["Code"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttCruiseAdvisory, ref argstrCode4);
                            oNode.Attributes["Code"].Value = argstrCode4;
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
                            string argstrCode5 = oNode.Attributes["Code"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttCruiseAdvisory, ref argstrCode5);
                            oNode.Attributes["Code"].Value = argstrCode5;
                        }
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsCruiseSailAvail", "Error *** Decoding CruiseSailAvail Response", ex.Message, string.Empty);
            }
            return strResponse;
        }

        private string FilterCruiseSailAvail(string strRequest, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            DataView ttCruiseLines;
            DataView ttCruiseRegions;
            DataView ttCruiseShips;
            DataView ttCruiseProfiles;
            int intCruiseLines;
            string VendorCode = "";
            string ShipCode = "";
            int intQuantity = 0;
            int intMaxGuestPerCabin = 0;
            int intdefaultSailingDuration;
            bool AddDuration;
            XmlAttribute oAttr;
            string GroupIndicator = "";
            string InclusiveFilter = "";

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;

                ttCruiseLines = (DataView)TripXMLMain.AppState.Get("ttCruiseLines");
                ttCruiseRegions = (DataView)TripXMLMain.AppState.Get("ttCruiseRegions");
                ttCruiseShips = (DataView)TripXMLMain.AppState.Get("ttCruiseShips");
                ttCruiseProfiles = (DataView)TripXMLMain.AppState.Get("ttCruiseProfiles");

                if (oRoot.SelectSingleNode("CruiseLinePrefs") is null)
                {
                    throw new Exception("Invalid request. Cruise Line Preferences is mandatory.");
                }

                intCruiseLines = oRoot.SelectNodes("CruiseLinePrefs/CruiseLinePref").Count;

                if (intCruiseLines > 5)
                {
                    throw new Exception("Invalid request. Maximum of 5 preferred cruise lines allowed.");
                }

                // Check SailingDuration
                if (oRoot.SelectSingleNode("SailingDateRange").Attributes["Duration"] is null)
                {
                    intdefaultSailingDuration = 0;
                    oAttr = oDoc.CreateAttribute("Duration");
                    oAttr.Value = intdefaultSailingDuration.ToString();
                    oRoot.SelectSingleNode("SailingDateRange").Attributes.Append(oAttr);
                }
                else
                {
                    intdefaultSailingDuration = Conversions.ToInteger(oRoot.SelectSingleNode("SailingDateRange").Attributes["Duration"].Value);
                }
                AddDuration = intdefaultSailingDuration == 0;

                // Get GroupIndicator
                if (oRoot.SelectSingleNode("GuestCounts") is null)
                {
                    GroupIndicator = "";
                }
                else
                {
                    GroupIndicator = modMain.IsNothing(oRoot.SelectSingleNode("GuestCounts").Attributes["GroupIndicator"], "").ToString();
                }

                if (intCruiseLines == 1)
                {
                    // Check Cruise Line Code - Vendor Code
                    oNode = oRoot.SelectSingleNode("CruiseLinePrefs/CruiseLinePref");

                    if (oNode.Attributes["VendorCode"] is null)
                    {
                        throw new Exception("Invalid request. Cruise line vendor code is mandatory.");
                    }
                    VendorCode = oNode.Attributes["VendorCode"].Value;
                    if (!modMain.IsDecodeValue(ref ttCruiseLines, ref VendorCode))
                    {
                        throw new Exception(sb.Append("Invalid Cruise Line Code - ").Append(VendorCode).ToString());
                        sb.Remove(0, sb.Length);
                    }
                    else
                    {
                        // Check ShipCode for Single Cruise Line
                        ShipCode = Conversions.ToString(modMain.IsNothing(oNode.Attributes["ShipCode"], ""));
                        // Get inclusiveFilteringSupportedForSailAvl
                        if (oNode.SelectSingleNode("InclusivePackageOption") is null)
                        {
                            InclusiveFilter = "";
                        }
                        else
                        {
                            InclusiveFilter = Conversions.ToString(modMain.IsNothing(oNode.SelectSingleNode("InclusivePackageOption").Attributes["InclusiveIndicator"], ""));
                        }
                        if (ShipCode.Length > 0)
                        {
                            if (!modMain.IsCruiseFilterValue(ref ttCruiseShips, VendorCode, ShipCode))
                            {
                                throw new Exception(sb.Append("Invalid Ship Code - ").Append(ShipCode).Append(" for cruise line ").Append(VendorCode).ToString());
                                sb.Remove(0, sb.Length);
                            }
                        }
                        else
                        {
                            // Check Region
                            if (oRoot.SelectNodes("RegionPref").Count == 0)
                            {
                                throw new Exception("Invalid request. Region code is mandatory when ship code not specified.");
                            }
                            foreach (XmlNode currentONode in oRoot.SelectNodes("RegionPref"))
                            {
                                oNode = currentONode;
                                bool localIsDecodeValue() { string argstrCode = oNode.Attributes["RegionCode"].Value; var ret = modMain.IsDecodeValue(ref ttCruiseRegions, ref argstrCode); oNode.Attributes["RegionCode"].Value = argstrCode; return ret; }

                                if (!localIsDecodeValue())
                                {
                                    throw new Exception(sb.Append("Invalid Region Code - ").Append(oNode.Attributes["RegionCode"].Value).ToString());
                                    sb.Remove(0, sb.Length);
                                }
                            }
                        }
                        // Get defaultSailingDuration
                        if (AddDuration)
                        {
                            intdefaultSailingDuration = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, VendorCode, "defaultSailingDuration"));
                        }
                        // Get maxGuestPerCabin
                        intMaxGuestPerCabin = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, VendorCode, "maxGuestPerCabin"));
                        // Check GroupIndicator
                        if (GroupIndicator.Length > 0 & modMain.GetCruiseFilterValue(ref ttCruiseProfiles, VendorCode, "groupIndicatorOnSailAvlSupported").ToLower() == "false")
                        {
                            throw new Exception("Group indicator on sailing availability not supported for this cruise line.");
                        }
                        // Check inclusiveFilteringSupportedForSailAvl
                        if (InclusiveFilter.Length > 0 & modMain.GetCruiseFilterValue(ref ttCruiseProfiles, VendorCode, "inclusiveFilteringSupportedForSailAvl").ToLower() == "false")
                        {
                            throw new Exception("Inclusive package indicator on sailing availability not supported for this cruise line.");
                        }
                    }
                }
                else
                {
                    // Check GroupIndicator
                    if (GroupIndicator.Length > 0)
                    {
                        throw new Exception("GroupIndicator not allowed if multiple cruise lines selected in the request.");
                    }
                    foreach (XmlNode currentONode1 in oRoot.SelectNodes("CruiseLinePrefs/CruiseLinePref"))
                    {
                        oNode = currentONode1;
                        // Check InclusiveFilter
                        if (oNode.SelectSingleNode("InclusivePackageOption") is null)
                        {
                            InclusiveFilter = "";
                        }
                        else
                        {
                            InclusiveFilter = Conversions.ToString(modMain.IsNothing(oNode.SelectSingleNode("InclusivePackageOption").Attributes["InclusiveIndicator"], ""));
                        }
                        if (InclusiveFilter.ToString().Length > 0)
                        {
                            throw new Exception("GroupIndicator not allowed if multiple cruise lines selected in the request.");
                        }
                        // Check Cruise Line Code
                        if (oNode.Attributes["VendorCode"] is null)
                        {
                            throw new Exception("Invalid request. Cruise line vendor code is mandatory.");
                        }
                        VendorCode = Conversions.ToString(modMain.IsNothing(oNode.Attributes["VendorCode"], ""));
                        if (!modMain.IsDecodeValue(ref ttCruiseLines, ref VendorCode))
                        {
                            throw new Exception(sb.Append("Invalid Cruise Line Code - ").Append(VendorCode).ToString());
                            sb.Remove(0, sb.Length);
                        }
                        else
                        {
                            // Check ShipCode
                            ShipCode = Conversions.ToString(modMain.IsNothing(oNode.Attributes["ShipCode"], ""));
                            if (ShipCode.Length > 0)
                            {
                                throw new Exception("Ship code not allow with more than one preferred cruise line selection.");
                            }
                            // Get defaultSailingDuration. The Max of all Cruise
                            if (AddDuration)
                            {
                                intQuantity = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, VendorCode, "defaultSailingDuration"));
                                if (intQuantity > intdefaultSailingDuration)
                                {
                                    intdefaultSailingDuration = intQuantity;
                                }
                            }
                            // Get maxGuestPerCabin. The Max of all Cruise
                            intQuantity = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, VendorCode, "maxGuestPerCabin"));
                            if (intQuantity > intMaxGuestPerCabin)
                            {
                                intMaxGuestPerCabin = intQuantity;
                            }
                        }
                    }
                    // Check Region
                    if (oRoot.SelectNodes("RegionPref").Count == 0)
                    {
                        throw new Exception("Invalid request. Region code is mandatory when ship code not specified.");
                    }
                    foreach (XmlNode currentONode2 in oRoot.SelectSingleNode("RegionPref"))
                    {
                        oNode = currentONode2;
                        bool localIsDecodeValue1() { string argstrCode1 = oNode.Attributes["RegionCode"].Value; var ret = modMain.IsDecodeValue(ref ttCruiseRegions, ref argstrCode1); oNode.Attributes["RegionCode"].Value = argstrCode1; return ret; }

                        if (!localIsDecodeValue1())
                        {
                            throw new Exception(sb.Append("Invalid Region Code - ").Append(oNode.Attributes["RegionCode"].Value).ToString());
                            sb.Remove(0, sb.Length);
                        }
                    }
                }

                // Check for Number In Party
                intQuantity = 0;
                foreach (XmlNode currentONode3 in oRoot.SelectNodes("GuestCounts/GuestCount"))
                {
                    oNode = currentONode3;
                    intQuantity += Conversions.ToInteger(modMain.IsNothing(oNode.Attributes["Quantity"], 0));
                }
                if (intQuantity > intMaxGuestPerCabin)
                {
                    throw new Exception("Maximum number of guest per cabin exceeded for specified cruise line.");
                }

                if (AddDuration)
                {
                    oRoot.SelectSingleNode("SailingDateRange").Attributes["Duration"].Value = intdefaultSailingDuration.ToString();
                    strRequest = oDoc.OuterXml;
                }
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsCruiseSailAvail", "Error *** Filtering CruiseSailAvail Request", ex.Message, string.Empty);
                throw ex;
            }
            return strRequest;
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

                // Validate Rules for CruiseSailAvail
                strRequest = FilterCruiseSailAvail(strRequest, ttCredential.UserID);

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

                strResponse = DecodeCruiseSailAvail(strResponse, ttCredential.UserID);

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsCruiseSailAvail", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 
        public wmCruiseSailAvailOut.OTA_CruiseSailAvailRS wmCruiseSailAvail(wmCruiseSailAvailIn.OTA_CruiseSailAvailRQ OTA_CruiseSailAvailRQ)
        {
            string xmlMessage = "";
            wmCruiseSailAvailOut.OTA_CruiseSailAvailRS oCruiseSailAvailRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmCruiseSailAvailIn.OTA_CruiseSailAvailRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_CruiseSailAvailRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            xmlMessage = xmlMessage.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CruiseSailAvail);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmCruiseSailAvailOut.OTA_CruiseSailAvailRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oCruiseSailAvailRS = (wmCruiseSailAvailOut.OTA_CruiseSailAvailRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsCruiseSailServices", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oCruiseSailAvailRS;

        }
        public string wmCruiseSailAvailXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.CruiseSailAvail);
        }

        #endregion

    }

}