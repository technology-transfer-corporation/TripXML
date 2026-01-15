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


    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsCruisePriceBooking", Name = "wsCruisePriceBooking", Description = "A TripXML Web Service to Process Cruise Price Booking Messages Request.")]
    public class wsCruisePriceBooking : WebService
    {

        #region  Web Services Designer Generated Code 

        public wsCruisePriceBooking() : base()
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

        private string mstrVendorCode = "";
        private string mstrShipCode = "";
        private string mstrDepartureDate = "";
        private string mstrDuration = "";

        private string DecodeCruisePriceBooking(string strResponse, string UserID)
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

                ttCruiseAdvisory = (DataView)Application.Get("ttCruiseAdvisory");

                if (oRoot.SelectSingleNode("Errors") is null)
                {

                    ttCruiseLines = (DataView)Application.Get("ttCruiseLines");
                    ttCruiseShips = (DataView)Application.Get("ttCruiseShips");
                    ttCruisePricedItems = (DataView)Application.Get("ttCruisePricedItems");

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
                    foreach (XmlNode currentONode1 in oRoot.SelectNodes("BookingPayment/BookingPrices/BookingPrice"))
                    {
                        oNode = currentONode1;
                        if (string.IsNullOrEmpty(oNode.InnerText))
                        {
                            string argstrCode2 = oNode.Attributes["PriceTypeCode"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttCruisePricedItems, ref argstrCode2);
                            oNode.Attributes["PriceTypeCode"].Value = argstrCode2;
                        }
                    }
                    foreach (XmlNode oGpNode in oRoot.SelectNodes("BookingPayment/GuestPrices/GuestPrice"))
                    {
                        foreach (XmlNode currentONode2 in oGpNode.SelectNodes("PriceInfos/PriceInfo"))
                        {
                            oNode = currentONode2;
                            if (string.IsNullOrEmpty(oNode.InnerText))
                            {
                                string argstrCode3 = oNode.Attributes["PriceTypeCode"].Value;
                                oNode.InnerText = modMain.GetDecodeValue(ref ttCruisePricedItems, ref argstrCode3);
                                oNode.Attributes["PriceTypeCode"].Value = argstrCode3;
                            }
                        }
                    }
                }

                else
                {
                    // *******************************
                    // Decode Advisory Errors Codes  *
                    // *******************************
                    foreach (XmlNode currentONode3 in oRoot.SelectNodes("Errors/Error"))
                    {
                        oNode = currentONode3;
                        if (oNode.InnerText.Length == 0)
                        {
                            string argstrCode4 = oNode.Attributes["Code"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttCruiseAdvisory, ref argstrCode4);
                            oNode.Attributes["Code"].Value = argstrCode4;
                        }
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsCruisePriceBooking", "Error *** Decoding CruisePriceBooking Response", ex.Message, string.Empty);
            }
            return strResponse;
        }

        private string FilterCruisePriceBooking(string strRequest, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlNode oNodeChild = null;
            XmlNode oNodeGrandChild = null;
            XmlNode oNodeGGrandChild = null;
            DataView ttCruiseLines;
            DataView ttCruiseMot;
            DataView ttCruiseShips;
            DataView ttCruiseProfiles;
            DataView ttCruiseCities;
            DataView ttCruiseCurrency;
            DataView ttCruiseBedConfiguration;
            DataView ttCruisePaxTitle;
            DataView ttwsCruiseInsurance;
            DataView ttwsCreditCards;
            int intQuantity = 0;
            int intMaxGuestPerCabin = 0;
            int CityFieldLength;
            int AddressFieldLength;
            int PostalCodeFieldLength;
            int FirstNameFieldLength;
            int LastNameFieldLength;
            bool TitleRequired;
            string strName = "";
            bool GuestCitySupported;
            bool FareCodeIndicator;
            string FareCode = null;
            bool PastGuestIndicator;
            string Language = null;
            string DiningRoom = null;
            string TableSize = null;
            string AgeCode = null;
            string InsuranceCode = null;

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
                ttCruiseBedConfiguration = (DataView)Application.Get("ttCruiseBedConfiguration");
                ttCruisePaxTitle = (DataView)Application.Get("ttCruisePaxTitle");
                ttwsCruiseInsurance = (DataView)Application.Get("ttwsCruiseInsurance");
                ttwsCreditCards = (DataView)Application.Get("ttwsCreditCards");

                // Check Cruise Line Code - Vendor Code
                oNode = oRoot.SelectSingleNode("SailingInfo/SelectedSailing");

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

                // Get Some Info from the Request to Echo them back on the Response
                mstrDepartureDate = oNode.Attributes["Start"].Value;
                mstrDuration = oNode.Attributes["Duration"].Value;

                // Check Currency Code
                if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "currencyRequiredFareAvailabilityRequest") == "true")
                {
                    if (oRoot.SelectSingleNode("SailingInfo/Currency") is null)
                    {
                        throw new Exception("Currency Code is mandatory for this Cruise line.");
                    }
                    else if (string.IsNullOrEmpty(oRoot.SelectSingleNode("SailingInfo/Currency")?.Attributes["CurrencyCode"]?.Value))
                    {
                        throw new Exception("Currency Code is mandatory for this Cruise line.");
                    }
                    else if (!modMain.IsCruiseFilterValue(ref ttCruiseCurrency, mstrVendorCode, oRoot.SelectSingleNode("SailingInfo/Currency").Attributes["CurrencyCode"].Value))
                    {
                        throw new Exception(sb.Append("Currency code - ").Append(oRoot.SelectSingleNode("SailingInfo/Currency").Attributes["CurrencyCode"].Value).Append(" not supported by this cruise line ").Append(mstrVendorCode).ToString());
                        sb.Remove(0, sb.Length);
                    }
                }

                // Get maxGuestPerCabin
                intMaxGuestPerCabin = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "maxGuestPerCabin"));
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

                FareCodeIndicator = modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "fareCodeAtPassangerLevel") == "false";
                PastGuestIndicator = modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "pastGuestIndicatorSupported") == "false";
                TitleRequired = modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "guestTitleRequired") == "true";
                FirstNameFieldLength = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "firstNameFieldLength"));
                LastNameFieldLength = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "lastNameFieldLength"));
                AddressFieldLength = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "streetAddressLength"));
                CityFieldLength = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "cityNameLength"));
                PostalCodeFieldLength = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "postalCodeLength"));
                GuestCitySupported = modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "guestCitySupported") == "true";

                // Check Guest Rules
                foreach (XmlNode currentONode1 in oRoot.SelectNodes("ReservationInfo/GuestDetails/GuestDetail"))
                {
                    oNode = currentONode1;
                    foreach (XmlNode currentONodeChild in oNode.SelectNodes("ContactInfo"))
                    {
                        oNodeChild = currentONodeChild;
                        foreach (XmlNode currentONodeGrandChild in oNodeChild.SelectNodes("GuestTransportation"))
                        {
                            oNodeGrandChild = currentONodeGrandChild;
                            // Check MOT
                            if (!modMain.IsCruiseFilterValue(ref ttCruiseMot, mstrVendorCode, oNodeGrandChild.Attributes["TransportationMode"].Value))
                            {
                                throw new Exception(sb.Append("Invalid Transportation Mode - ").Append(oNodeGrandChild.Attributes["TransportationMode"].Value).Append(" for cruise line ").Append(mstrVendorCode).ToString());
                                sb.Remove(0, sb.Length);
                            }
                            // Check Gateway
                            if (!modMain.IsCruiseFilterValue(ref ttCruiseCities, mstrVendorCode, oNodeGrandChild.SelectSingleNode("GatewayCity").Attributes["LocationCode"].Value))
                            {
                                throw new Exception(sb.Append("Gateway location - ").Append(oNodeGrandChild.SelectSingleNode("GatewayCity").Attributes["LocationCode"].Value).Append(" not supported by this cruise line ").Append(mstrVendorCode).ToString());
                                sb.Remove(0, sb.Length);
                            }
                        }    // GuestTransportation

                        // Check PersonName for ContactInfo
                        oNodeGrandChild = oNodeChild.SelectSingleNode("PersonName");
                        // Check LastName Field Length
                        if (oNodeGrandChild.SelectSingleNode("Surname").InnerText.Length > LastNameFieldLength)
                        {
                            throw new Exception(sb.Append("Last name too long. Maximum length allow by this cruise line is ").Append(LastNameFieldLength).ToString());
                            sb.Remove(0, sb.Length);
                        }
                        // Check FirstName Filed Length
                        foreach (XmlNode currentONodeGGrandChild in oNodeGrandChild.SelectNodes("GivenName"))
                        {
                            oNodeGGrandChild = currentONodeGGrandChild;
                            if (oNodeGGrandChild.InnerText.Length > FirstNameFieldLength)
                            {
                                throw new Exception(sb.Append("First name too long. Maximum length allow by this cruise line is ").Append(FirstNameFieldLength).ToString());
                                sb.Remove(0, sb.Length);
                            }
                        }
                        // Check Title
                        if (TitleRequired & oNodeGrandChild.SelectSingleNode("NameTitle") is null)
                        {
                            throw new Exception("Passanger title is required by this cruise line.");
                        }
                        else
                        {
                            foreach (XmlNode currentONodeGGrandChild1 in oNodeGrandChild.SelectNodes("NameTitle"))
                            {
                                oNodeGGrandChild = currentONodeGGrandChild1;
                                if (!modMain.IsCruiseFilterValue(ref ttCruisePaxTitle, mstrVendorCode, oNodeGGrandChild.InnerText))
                                {
                                    throw new Exception(sb.Append("Passanger title - ").Append(oNodeGGrandChild.InnerText).Append(" not supported by this cruise line ").Append(mstrVendorCode).ToString());
                                    sb.Remove(0, sb.Length);
                                }
                            }
                        }

                        // Check CityName for Contact Info
                        foreach (XmlNode currentONodeGrandChild1 in oNodeChild.SelectNodes("Address"))
                        {
                            oNodeGrandChild = currentONodeGrandChild1;
                            if (oNodeGrandChild.SelectSingleNode("CityName") is null)
                            {
                                strName = "";
                            }
                            else
                            {
                                strName = oNodeGrandChild.SelectSingleNode("CityName").InnerText.Trim();
                            }
                            switch (Conversions.ToInteger(modMain.IsNothing(oNodeGrandChild.Attributes["UseType"], "0")))
                            {
                                case 5:
                                    {
                                        if (GuestCitySupported)
                                        {
                                            if (strName.Length == 0)
                                            {
                                                throw new Exception("Guest city name is mandatory for this cruise line.");
                                            }
                                            else if (!modMain.IsCruiseFilterValue(ref ttCruiseCities, mstrVendorCode, strName))
                                            {
                                                throw new Exception(sb.Append("City location - ").Append(strName).Append(" not supported by this cruise line.").ToString());
                                                sb.Remove(0, sb.Length);
                                            }
                                        }

                                        break;
                                    }
                                case 6:
                                case 10:
                                    {
                                        if (strName.Length > CityFieldLength)
                                        {
                                            throw new Exception(sb.Append("City name too long. Maximum length allow by this cruise line is ").Append(CityFieldLength).ToString());
                                            sb.Remove(0, sb.Length);
                                        }

                                        break;
                                    }
                            }

                        }    // Address

                    } // ContactInfo

                    // Check PersonName for LinkedTraveler
                    foreach (XmlNode currentONodeChild1 in oNode.SelectNodes("LinkedTraveler"))
                    {
                        oNodeChild = currentONodeChild1;
                        // Check PersonName
                        oNodeGrandChild = oNodeChild.SelectSingleNode("PersonName");
                        // Check LastName Field Length
                        if (oNodeGrandChild.SelectSingleNode("Surname").InnerText.Length > LastNameFieldLength)
                        {
                            throw new Exception(sb.Append("Last name too long. Maximum length allow by this cruise line is ").Append(LastNameFieldLength).ToString());
                            sb.Remove(0, sb.Length);
                        }
                        // Check FirstName Filed Length
                        foreach (XmlNode currentONodeGGrandChild2 in oNodeGrandChild.SelectNodes("GivenName"))
                        {
                            oNodeGGrandChild = currentONodeGGrandChild2;
                            if (oNodeGGrandChild.InnerText.Length > FirstNameFieldLength)
                            {
                                throw new Exception(sb.Append("First name too long. Maximum length allow by this cruise line is ").Append(FirstNameFieldLength).ToString());
                                sb.Remove(0, sb.Length);
                            }
                        }
                        // Check Title
                        if (TitleRequired & oNodeGrandChild.SelectSingleNode("NameTitle") is null)
                        {
                            throw new Exception("Passanger title is required by this cruise line.");
                        }
                        else
                        {
                            foreach (XmlNode currentONodeGGrandChild3 in oNodeGrandChild.SelectNodes("NameTitle"))
                            {
                                oNodeGGrandChild = currentONodeGGrandChild3;
                                if (!modMain.IsCruiseFilterValue(ref ttCruisePaxTitle, mstrVendorCode, oNodeGGrandChild.InnerText))
                                {
                                    throw new Exception(sb.Append("Passanger title - ").Append(oNodeGGrandChild.InnerText).Append(" not supported by this cruise line ").Append(mstrVendorCode).ToString());
                                    sb.Remove(0, sb.Length);
                                }
                            }
                        }
                    } // LinkedTraveler

                    // Check FareCode
                    if (FareCodeIndicator)
                    {
                        foreach (XmlNode currentONodeChild2 in oNode.SelectNodes("SelectedFareCode"))
                        {
                            oNodeChild = currentONodeChild2;
                            if (FareCode is null)
                            {
                                FareCode = oNodeChild.Attributes["FareCode"].Value;
                            }
                            else if (string.Compare(FareCode, oNodeChild.Attributes["FareCode"].Value) != 0)
                            {
                                throw new Exception("Fare codes must be identical for all guests for this cruise line.");
                            }
                        }
                    }
                    // Check Past Passenger
                    if (PastGuestIndicator)
                    {
                        foreach (XmlNode currentONodeChild3 in oNode.SelectNodes("LoyaltyInfo"))
                        {
                            oNodeChild = currentONodeChild3;
                            if (oNodeChild.Attributes["MembershipID"] is not null)
                            {
                                throw new Exception("Past passanger number not supported for this cruise line.");
                            }
                        }
                    }

                    // Check Dining  -   ReservationInfo/GuestDetails/GuestDetail+/SelectedDining+
                    foreach (XmlNode currentONodeChild4 in oNode.SelectNodes("SelectedDining"))
                    {
                        oNodeChild = currentONodeChild4;
                        // Check Language
                        if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "languageOptionSupported") == "false")
                        {
                            if (modMain.IsNothing(oNodeChild.Attributes["Language"], "").ToString().Length != 0)
                            {
                                throw new Exception("Dining language selection not supported for this cruise line.");
                            }
                        }
                        // Check DiningRoom
                        if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "diningRoomOptionSupported") == "false")
                        {
                            if (modMain.IsNothing(oNodeChild.Attributes["DiningRoom"], "").ToString().Length != 0)
                            {
                                throw new Exception("Dining room selection not supported for this cruise line.");
                            }
                        }
                        // Check TableSize
                        if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "diningTableSizeOptionSupported") == "false")
                        {
                            if (modMain.IsNothing(oNodeChild.Attributes["TableSize"], "").ToString().Length != 0)
                            {
                                throw new Exception("Table size selection not supported for this cruise line.");
                            }
                        }
                        // Check AgeCode
                        if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "ageGroupPrefForDiningSupported") == "false")
                        {
                            if (modMain.IsNothing(oNodeChild.Attributes["AgeCode"], "").ToString().Length != 0)
                            {
                                throw new Exception("Age group code selection not supported for this cruise line.");
                            }
                        }
                        // Check Dining At Passenger Level
                        if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "diningAtPassengerLevel") == "false")
                        {
                            if (Language is null)
                            {
                                if (oNodeChild.Attributes["Language"] is not null)
                                {
                                    Language = oNodeChild.Attributes["Language"].Value;
                                }
                            }
                            else if (string.Compare(Language, Conversions.ToString(modMain.IsNothing(oNodeChild.Attributes["Language"], ""))) != 0)
                            {
                                throw new Exception("Dining options must be the same for all guests for this cruise line.");
                            }
                            if (DiningRoom is null)
                            {
                                if (oNodeChild.Attributes["DiningRoom"] is not null)
                                {
                                    DiningRoom = oNodeChild.Attributes["DiningRoom"].Value;
                                }
                            }
                            else if (string.Compare(DiningRoom, Conversions.ToString(modMain.IsNothing(oNodeChild.Attributes["DiningRoom"], ""))) != 0)
                            {
                                throw new Exception("Dining options must be the same for all guests for this cruise line.");
                            }
                            if (TableSize is null)
                            {
                                if (oNodeChild.Attributes["TableSize"] is not null)
                                {
                                    TableSize = oNodeChild.Attributes["TableSize"].Value;
                                }
                            }
                            else if (string.Compare(TableSize, Conversions.ToString(modMain.IsNothing(oNodeChild.Attributes["TableSize"], ""))) != 0)
                            {
                                throw new Exception("Dining options must be the same for all guests for this cruise line.");
                            }
                            if (AgeCode is null)
                            {
                                if (oNodeChild.Attributes["AgeCode"] is not null)
                                {
                                    AgeCode = oNodeChild.Attributes["AgeCode"].Value;
                                }
                            }
                            else if (string.Compare(AgeCode, Conversions.ToString(modMain.IsNothing(oNodeChild.Attributes["AgeCode"], ""))) != 0)
                            {
                                throw new Exception("Dining options must be the same for all guests for this cruise line.");
                            }
                        }

                    }    // SelectedDining (oNodeChild)

                    // Check Insurance   -   ReservationInfo/GuestDetails/GuestDetail+/SelectedInsurance
                    if (oNode.SelectSingleNode("SelectedInsurance") is not null)
                    {
                        oNodeChild = oNode.SelectSingleNode("SelectedInsurance");
                        // Check Insurance Code
                        if (oNodeChild.Attributes["InsuranceCode"] is not null)
                        {
                            bool localIsDecodeValue() { string argstrCode = oNodeChild.Attributes["InsuranceCode"].Value; var ret = modMain.IsDecodeValue(ref ttwsCruiseInsurance, ref argstrCode); oNodeChild.Attributes["InsuranceCode"].Value = argstrCode; return ret; }

                            if (!localIsDecodeValue())
                            {
                                throw new Exception("Invalid insurance code.");
                            }
                            else if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "insuranceAtPassengerLevel") == "false")
                            {
                                // Check Insurance Association
                                if (InsuranceCode is null)
                                {
                                    InsuranceCode = oNodeChild.Attributes["InsuranceCode"].Value;
                                }
                                else if (string.Compare(InsuranceCode, oNodeChild.Attributes["InsuranceCode"].Value) != 0)
                                {
                                    throw new Exception("Insurance options must be the same for all guests for this cruise line.");
                                }
                            }
                        }
                    }

                }    // GuestDetail (oNode)

                // Check Bed Configuration
                foreach (XmlNode currentONode2 in oRoot.SelectNodes("SailingInfo/SelectedCategory/SelectedCabin"))
                {
                    oNode = currentONode2;
                    if (oNode.Attributes["BedConfigurationCode"] is not null)
                    {
                        bool localIsDecodeValue1() { string argstrCode1 = oNode.Attributes["BedConfigurationCode"].Value; var ret = modMain.IsDecodeValue(ref ttCruiseBedConfiguration, ref argstrCode1); oNode.Attributes["BedConfigurationCode"].Value = argstrCode1; return ret; }

                        if (!localIsDecodeValue1())
                        {
                            throw new Exception(sb.Append("Invalid Bed Configuration Code - ").Append(oNode.Attributes["BedConfigurationCode"].Value).ToString());
                            sb.Remove(0, sb.Length);
                        }
                    }
                }

                // Check PersonName for LinkedBooking
                foreach (XmlNode currentONode3 in oRoot.SelectNodes("ReservationInfo/LinkedBookings/LinkedBooking"))
                {
                    oNode = currentONode3;
                    // Check PersonName
                    oNodeChild = oNode.SelectSingleNode("PersonName");
                    // Check LastName Field Length
                    if (oNodeChild.SelectSingleNode("Surname").InnerText.Length > LastNameFieldLength)
                    {
                        throw new Exception(sb.Append("Last name too long. Maximum length allow by this cruise line is ").Append(LastNameFieldLength).ToString());
                        sb.Remove(0, sb.Length);
                    }
                    // Check FirstName Filed Length
                    foreach (XmlNode currentONodeGrandChild2 in oNodeChild.SelectNodes("GivenName"))
                    {
                        oNodeGrandChild = currentONodeGrandChild2;
                        if (oNodeGrandChild.InnerText.Length > FirstNameFieldLength)
                        {
                            throw new Exception(sb.Append("First name too long. Maximum length allow by this cruise line is ").Append(FirstNameFieldLength).ToString());
                            sb.Remove(0, sb.Length);
                        }
                    }
                    // Check Title
                    if (TitleRequired & oNodeChild.SelectSingleNode("NameTitle") is null)
                    {
                        throw new Exception("Passanger title is required by this cruise line.");
                    }
                    else
                    {
                        foreach (XmlNode currentONodeGrandChild3 in oNodeChild.SelectNodes("NameTitle"))
                        {
                            oNodeGrandChild = currentONodeGrandChild3;
                            if (!modMain.IsCruiseFilterValue(ref ttCruisePaxTitle, mstrVendorCode, oNodeGrandChild.InnerText))
                            {
                                throw new Exception(sb.Append("Passanger title - ").Append(oNodeGrandChild.InnerText).Append(" not supported by this cruise line ").Append(mstrVendorCode).ToString());
                                sb.Remove(0, sb.Length);
                            }
                        }
                    }
                }    // LinkedBooking (oNode)

                // Check PaymentRequest Rules
                foreach (XmlNode currentONode4 in oRoot.SelectNodes("ReservationInfo/PaymentRequests/PaymentRequest"))
                {
                    oNode = currentONode4;
                    // Check FOP
                    if (oNode.SelectSingleNode("PaymentCard") is null & oNode.SelectSingleNode("DirectBill") is null)
                    {
                        throw new Exception("Invalid form of payment.");
                    }
                    else if (oNode.SelectSingleNode("PaymentCard") is not null)
                    {
                        oNodeChild = oNode.SelectSingleNode("PaymentCard");
                        // Check Credit Card Effective Date
                        if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "creditCardFromDateList") == "false" & modMain.IsNothing(oNodeChild.Attributes["EffectiveDate"], "").ToString().Length > 0)
                        {
                            throw new Exception("Credit card effective date not supported by this cruise line.");
                        }
                        // Check Credit Card Code
                        if (oNode.Attributes["CardCode"] is not null)
                        {
                            bool localIsDecodeValue2() { string argstrCode2 = oNodeChild.Attributes["CardCode"].Value; var ret = modMain.IsDecodeValue(ref ttwsCreditCards, ref argstrCode2); oNodeChild.Attributes["CardCode"].Value = argstrCode2; return ret; }

                            if (!localIsDecodeValue2())
                            {
                                throw new Exception("Credit card vendor code not supported.");
                            }
                        }
                        // Check Address Line Mandatory
                        oNodeGrandChild = oNodeChild.SelectSingleNode("Address");
                        if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "billAddMandOnCC") == "true" & oNodeGrandChild is null)
                        {
                            throw new Exception("Billing address is mandatory for this cruise line.");
                        }
                        // Check Payment Address Line Length
                        if (oNodeGrandChild.SelectSingleNode("AddressLine") is not null)
                        {
                            if (oNodeGrandChild.SelectSingleNode("AddressLine").InnerText.Length > AddressFieldLength)
                            {
                                throw new Exception(sb.Append("Address line too long. Maximum length allow by this cruise line is ").Append(AddressFieldLength).ToString());
                                sb.Remove(0, sb.Length);
                            }
                        }
                        // Check Payment City Length
                        if (oNodeGrandChild.SelectSingleNode("CityName") is not null)
                        {
                            if (oNodeGrandChild.SelectSingleNode("CityName").InnerText.Length > CityFieldLength)
                            {
                                throw new Exception(sb.Append("City name too long. Maximum length allow by this cruise line is ").Append(CityFieldLength).ToString());
                                sb.Remove(0, sb.Length);
                            }
                        }
                        // Check Payment Postal Code Length
                        if (oNodeGrandChild.SelectSingleNode("PostalCode") is not null)
                        {
                            if (oNodeGrandChild.SelectSingleNode("PostalCode").InnerText.Length > PostalCodeFieldLength)
                            {
                                throw new Exception(sb.Append("Postal code too long. Maximum length allow by this cruise line is ").Append(PostalCodeFieldLength).ToString());
                                sb.Remove(0, sb.Length);
                            }
                        }
                    }
                    else if (oNode.SelectSingleNode("DirectBill") is not null)
                    {
                        oNodeChild = oNode.SelectSingleNode("DirectBill");
                        if (modMain.IsNothing(oNodeChild.Attributes["DirectBill_ID"], "").ToString().Length == 0)
                        {
                            throw new Exception("Invalid form of payment.");
                        }
                        else if (modMain.IsNothing(oNode.Attributes["ReferenceNumber"], "").ToString().Length == 0)
                        {
                            // Check Payment Reference
                            throw new Exception("Payment reference number is mandatory with this form of payment.");
                        }
                    }

                    // Check Extended Payment
                    if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "extendedPaymentSupported") == "false" && oNode.Attributes["ExtendedIndicator"] is not null && oNode.Attributes["ExtendedIndicator"].Value == "true")
                    {
                        throw new Exception("Extended payment not supported by this cruise line.");
                    }

                    // Check Payment Currency
                    if (oNode.SelectSingleNode("PaymentAmount") is not null)
                    {
                        if (oNode.SelectSingleNode("PaymentAmount").Attributes["CurrencyCode"] is not null)
                        {
                            if (!modMain.IsCruiseFilterValue(ref ttCruiseCurrency, mstrVendorCode, oNode.SelectSingleNode("PaymentAmount").Attributes["CurrencyCode"].Value))
                            {
                                throw new Exception(sb.Append("Currency code - ").Append(oNode.SelectSingleNode("PaymentAmount").Attributes["CurrencyCode"].Value).Append(" not supported by this cruise line.").ToString());
                                sb.Remove(0, sb.Length);
                            }
                        }
                    }

                }    // PaymentRequest (oNode)
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsCruisePriceBooking", "Error *** Filtering CruisePriceBooking Request", ex.Message, string.Empty);
                throw ex;
            }
            sb.Remove(0, sb.Length);
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

                // Validate Rules for CruisePriceBooking
                strRequest = FilterCruisePriceBooking(strRequest, ttCredential.UserID);

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

                strResponse = DecodeCruisePriceBooking(strResponse, ttCredential.UserID);

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsCruiseBooking", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [WebMethod(Description = "Process Cruise Price Booking Messages Request.")]
        public wmCruisePriceBookingOut.OTA_CruisePriceBookingRS wmCruisePriceBooking(wmCruisePriceBookingIn.OTA_CruisePriceBookingRQ OTA_CruisePriceBookingRQ)
        {
            string xmlMessage = "";
            wmCruisePriceBookingOut.OTA_CruisePriceBookingRS oCruisePriceBookingRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmCruisePriceBookingIn.OTA_CruisePriceBookingRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_CruisePriceBookingRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CruisePriceBooking);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmCruisePriceBookingOut.OTA_CruisePriceBookingRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oCruisePriceBookingRS = (wmCruisePriceBookingOut.OTA_CruisePriceBookingRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsCruiseBooking", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oCruisePriceBookingRS;

        }

        [WebMethod(Description = "Process Cruise Price Booking Xml Messages Request.")]
        public string wmCruisePriceBookingXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.CruisePriceBooking);
        }

        #endregion

    }

}