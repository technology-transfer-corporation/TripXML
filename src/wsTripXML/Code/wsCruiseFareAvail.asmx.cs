using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{


    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsCruiseFareAvail", Name = "wsCruiseFareAvail", Description = "A TripXML Web Service to Process Cruise Fare Availibility Messages Request.")]
    public class wsCruiseFareAvail : WebService
    {

        #region  Web Services Designer Generated Code 

        public wsCruiseFareAvail() : base()
        {

            // This call is required by the Web Services Designer.
            InitializeComponent();

            // Add your own initialization code after the InitializeComponent() call

        }

        // Required by the Web Services Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Web Services Designer
        // It can be modified using the Web Services Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        protected override void Dispose(bool disposing)
        {
            // CODEGEN: This procedure is required by the Web Services Designer
            // Do not modify it using the code editor.
            if (disposing)
            {
                if (components is not null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region  Decode Function 
        private StringBuilder sb = new StringBuilder();

        private string mstrShipCode = "";
        private string mstrDepartureDate = "";
        private string mstrDuration = "";

        private string DecodeCruiseFareAvail(string strResponse, string UserID)
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

                ttCruiseAdvisory = (DataView)Application.Get("ttCruiseAdvisory");

                if (oRoot.SelectSingleNode("Errors") is null)
                {

                    ttwsCruiseCities = (DataView)Application.Get("ttwsCruiseCities");
                    ttCruiseRegions = (DataView)Application.Get("ttCruiseRegions");
                    ttCruiseLines = (DataView)Application.Get("ttCruiseLines");
                    ttCruiseShips = (DataView)Application.Get("ttCruiseShips");

                    oNode = oRoot.SelectSingleNode("SailingInfo");
                    // Departure Date and Duration
                    if (string.IsNullOrEmpty(oNode.Attributes["DepartureDate"].Value))
                    {
                        oNode.Attributes["DepartureDate"].Value = mstrDepartureDate;
                    }
                    if (string.IsNullOrEmpty(oNode.Attributes["Duration"].Value))
                    {
                        oNode.Attributes["Duration"].Value = mstrDuration;
                    }
                    // *******************************
                    // Decode CruiseLines & Ships    *
                    // *******************************
                    string argstrCode = oNode.SelectSingleNode("CruiseLine").Attributes["VendorCode"].Value;
                    oNode.SelectSingleNode("CruiseLine").Attributes["VendorName"].Value = modMain.GetDecodeValue(ref ttCruiseLines, ref argstrCode);
                    oNode.SelectSingleNode("CruiseLine").Attributes["VendorCode"].Value = argstrCode;
                    if (string.IsNullOrEmpty(oNode.SelectSingleNode("CruiseLine").Attributes["ShipCode"].Value))
                    {
                        oNode.SelectSingleNode("CruiseLine").Attributes["ShipCode"].Value = mstrShipCode;
                    }
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

                    // *******************************
                    // Decode Advisory Errors Codes  *
                    // *******************************
                    foreach (XmlNode currentONode in oRoot.SelectNodes("Warnings/Warning"))
                    {
                        oNode = currentONode;
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
                    foreach (XmlNode currentONode1 in oRoot.SelectNodes("Errors/Error"))
                    {
                        oNode = currentONode1;
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
                CoreLib.SendTrace(UserID, "wsCruiseFareAvail", "Error *** Decoding CruiseFareAvail Response", ex.Message, string.Empty);
            }
            return strResponse;
        }

        private string FilterCruiseFareAvail(string strRequest, string UserID)
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
            string VendorCode = "";
            int intQuantity = 0;
            int intMaxGuestPerCabin = 0;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;

                ttCruiseLines = (DataView)Application.Get("ttCruiseLines");
                ttCruiseMot = (DataView)Application.Get("ttCruiseMot");
                ttCruiseShips = (DataView)Application.Get("ttCruiseShips");
                ttCruiseProfiles = (DataView)Application.Get("ttCruiseProfiles");
                ttCruiseCities = (DataView)Application.Get("ttCruiseCities");
                ttCruiseCurrency = (DataView)Application.Get("ttCruiseCurrency");

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

                // Check Package StartDate
                if (oRoot.SelectSingleNode("SailingInfo/InclusivePackageOption") is not null)
                {
                    if (oRoot.SelectSingleNode("SailingInfo/InclusivePackageOption").Attributes["StartDate"] is null)
                    {
                        throw new Exception("Package Start Date is mandatory.");
                    }
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
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsCruiseFareAvail", "Error *** Filtering CruiseFareAvail Request", ex.Message, string.Empty);
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

                var argoApp = Application;
                modMain.PreServiceRequest(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

                // Validate Rules for CruiseFareAvail
                strRequest = FilterCruiseFareAvail(strRequest, ttCredential.UserID);

                switch (ttCredential.Providers[0].Name.ToLower() ?? "")
                {
                    case "amadeus":
                        {
                            break;
                        }
                    // Dim ttAA As AmadeusAPIAdapter

                    // ttAA = Application.Get(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
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
                    // Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
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

                strResponse = DecodeCruiseFareAvail(strResponse, ttCredential.UserID);

                modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers[0].Name);
            }
            finally
            {
                modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsCruiseFareAvail", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [WebMethod(Description = "Process Cruise Fare Availability Messages Request.")]
        public wmCruiseFareAvailOut.OTA_CruiseFareAvailRS wmCruiseFareAvail(wmCruiseFareAvailIn.OTA_CruiseFareAvailRQ OTA_CruiseFareAvailRQ)
        {
            string xmlMessage = "";
            wmCruiseFareAvailOut.OTA_CruiseFareAvailRS oCruiseFareAvailRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmCruiseFareAvailIn.OTA_CruiseFareAvailRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_CruiseFareAvailRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CruiseFareAvail);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmCruiseFareAvailOut.OTA_CruiseFareAvailRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oCruiseFareAvailRS = (wmCruiseFareAvailOut.OTA_CruiseFareAvailRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsCruiseFareAvail", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oCruiseFareAvailRS;

        }

        [WebMethod(Description = "Process Cruise Fare Availibility Xml Messages Request.")]
        public string wmCruiseFareAvailXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.CruiseFareAvail);
        }

        #endregion

    }

}