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


    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsCruiseCreateBooking", Name = "wsCruiseCreateBooking", Description = "A TripXML Web Service to Process Cruise Create Booking Messages Request.")]
    public class wsCruiseCreateBooking : WebService
    {

        #region  Web Services Designer Generated Code 

        public wsCruiseCreateBooking() : base()
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

        private string DecodeCruiseCreateBooking(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            DataView ttCruiseAdvisory;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                ttCruiseAdvisory = (DataView)Application.Get("ttCruiseAdvisory");

                if (oRoot.SelectSingleNode("Errors") is not null)
                {
                    // *******************************
                    // Decode Advisory Errors Codes  *
                    // *******************************
                    foreach (XmlNode oNode in oRoot.SelectNodes("Errors/Error"))
                    {
                        if (oNode.InnerText.Length == 0)
                        {
                            string argstrCode = oNode.Attributes["Code"].Value;
                            oNode.InnerText = modMain.GetDecodeValue(ref ttCruiseAdvisory, ref argstrCode);
                            oNode.Attributes["Code"].Value = argstrCode;
                        }
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsCruiseCreateBooking", "Error *** Decoding CruiseCreateBooking Response", ex.Message, string.Empty);
            }
            return strResponse;
        }

        private string FilterCruiseCreateBooking(string strRequest, string UserID)
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
            DataView ttCruiseOccupation;
            int intQuantity = 0;
            int intMaxGuestPerCabin = 0;
            int CityFieldLength;
            int AddressFieldLength;
            int PostalCodeFieldLength;
            int FirstNameFieldLength;
            int LastNameFieldLength;
            int EmailAddressFieldLength;
            int PhoneNumberFieldLength;
            int PassportNumberFieldLength;
            int AlienNumberFieldLength;
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
                ttCruiseOccupation = (DataView)Application.Get("ttCruiseOccupation");

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
                EmailAddressFieldLength = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "emailAddressLength"));
                PhoneNumberFieldLength = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "phoneNumberLength"));
                PassportNumberFieldLength = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "passportNumberLength"));
                AlienNumberFieldLength = Conversions.ToInteger(modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "alienNumberLength"));
                GuestCitySupported = modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "guestCitySupported") == "true";

                // Check Guest Rules
                foreach (XmlNode currentONode1 in oRoot.SelectNodes("ReservationInfo/GuestDetails/GuestDetail"))
                {
                    oNode = currentONode1;
                    foreach (XmlNode currentONodeChild in oNode.SelectNodes("ContactInfo"))
                    {
                        oNodeChild = currentONodeChild;
                        // Check Guest Occupation
                        if (oNodeChild.Attributes["GuestOccupation"] is not null)
                        {
                            if (!modMain.IsCruiseFilterValue(ref ttCruiseOccupation, mstrVendorCode, oNodeChild.Attributes["GuestOccupation"].Value))
                            {
                                throw new Exception(sb.Append("Guest occupation - ").Append(oNodeChild.Attributes["GuestOccupation"].Value).Append(" not supported by this cruise line ").Append(mstrVendorCode).ToString());
                                sb.Remove(0, sb.Length);
                            }
                        }

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

                        // Check Emergency Flag
                        // If CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "emergencyContactInformation") <> "E"), Boolean) Then
                        // If CType(If(oNodeChild.Attributes("EmergencyFlag"), "") = "true", Boolean) Then
                        // Throw New Exception("Emergengy contact not supported by this cruise line.")
                        // End If
                        // End If

                        if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "emergencyContactInformation") != "E")
                        {
                            if (oNodeChild.Attributes["EmergencyFlag"] is not null && oNodeChild.Attributes["EmergencyFlag"].Value == "true")
                            {
                                throw new Exception("Emergency contact not supported by this cruise line.");
                            }
                        }

                        // Check Contact Info BirthDate
                        if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "paxBirthDateSupported") != "E")
                        {
                            if (oNodeChild.Attributes["BirthDate"] is not null && oNodeChild.Attributes["BirthDate"].Value.Length > 0)
                            {
                                throw new Exception("Guest birth date not supported by this cruise line.");
                            }
                        }

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
                                        // Emergency or Contact Address
                                        if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "emergencyContactInformation").IndexOf("C") == -1)
                                        {
                                            throw new Exception("Emergency or contact address not supported by this cruise line.");
                                        }

                                        break;
                                    }
                            }

                            // Check Contact Info Address Line Length
                            if (oNodeGrandChild.SelectSingleNode("AddressLine") is not null)
                            {
                                if (oNodeGrandChild.SelectSingleNode("AddressLine").InnerText.Length > AddressFieldLength)
                                {
                                    throw new Exception(sb.Append("Address line too long. Maximum length allow by this cruise line is ").Append(AddressFieldLength).ToString());
                                    sb.Remove(0, sb.Length);
                                }
                            }

                            // Check Contact Info Postal Code Length
                            if (oNodeGrandChild.SelectSingleNode("PostalCode") is not null)
                            {
                                if (oNodeGrandChild.SelectSingleNode("PostalCode").InnerText.Length > PostalCodeFieldLength)
                                {
                                    throw new Exception(sb.Append("Postal code too long. Maximum length allow by this cruise line is ").Append(PostalCodeFieldLength).ToString());
                                    sb.Remove(0, sb.Length);
                                }
                            }

                        }    // Address

                        foreach (XmlNode currentONodeGrandChild2 in oNodeChild.SelectNodes("Telephone"))
                        {
                            oNodeGrandChild = currentONodeGrandChild2;
                            // Check Emergency Phone Number
                            if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "emergencyContactInformation").IndexOf("P") == -1)
                            {
                                throw new Exception("Emergency or contact phone not supported by this cruise line.");
                            }
                            // Check Contact Info Telephone Length
                            if (oNodeGrandChild.InnerText.Length > PhoneNumberFieldLength)
                            {
                                throw new Exception(sb.Append("Phone number too long. Maximum length allow by this cruise line is ").Append(PhoneNumberFieldLength).ToString());
                                sb.Remove(0, sb.Length);
                            }
                        }    // Telephone

                        foreach (XmlNode currentONodeGrandChild3 in oNodeChild.SelectNodes("Email"))
                        {
                            oNodeGrandChild = currentONodeGrandChild3;
                            // Check Contact Info Email Address Length
                            if (oNodeGrandChild.InnerText.Length > EmailAddressFieldLength)
                            {
                                throw new Exception(sb.Append("Email Address too long. Maximum length allow by this cruise line is ").Append(EmailAddressFieldLength).ToString());
                                sb.Remove(0, sb.Length);
                            }
                        }    // Email

                    } // ContactInfo (oNodeChild)

                    foreach (XmlNode currentONodeChild1 in oNode.SelectNodes("TravelDocument"))
                    {
                        oNodeChild = currentONodeChild1;
                        switch (Conversions.ToInteger(modMain.IsNothing(oNodeChild.Attributes["DocType"], "0")))
                        {
                            case 2:
                                {
                                    // Check Passport Number Length
                                    if (modMain.IsNothing(oNodeChild.Attributes["DocID"], "").ToString().Length > PassportNumberFieldLength)
                                    {
                                        throw new Exception(sb.Append("Passport number too long. Maximum length allow by this cruise line is ").Append(PassportNumberFieldLength).ToString());
                                        sb.Remove(0, sb.Length);
                                    }

                                    break;
                                }
                            case 7:
                                {
                                    // Check Alien Number Length
                                    if (modMain.IsNothing(oNodeChild.Attributes["DocID"], "").ToString().Length > AlienNumberFieldLength)
                                    {
                                        throw new Exception(sb.Append("Alien registration number too long. Maximum length allow by this cruise line is ").Append(AlienNumberFieldLength).ToString());
                                        sb.Remove(0, sb.Length);
                                    }

                                    break;
                                }
                        }
                    }    // TravelDocument (oNodeChild)

                    // Check PersonName for LinkedTraveler
                    foreach (XmlNode currentONodeChild2 in oNode.SelectNodes("LinkedTraveler"))
                    {
                        oNodeChild = currentONodeChild2;
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
                        foreach (XmlNode currentONodeChild3 in oNode.SelectNodes("SelectedFareCode"))
                        {
                            oNodeChild = currentONodeChild3;
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
                        foreach (XmlNode currentONodeChild4 in oNode.SelectNodes("LoyaltyInfo"))
                        {
                            oNodeChild = currentONodeChild4;
                            if (oNodeChild.Attributes["MembershipID"] is not null)
                            {
                                throw new Exception("Past passanger number not supported for this cruise line.");
                            }
                        }
                    }

                    // Check Dining  -   ReservationInfo/GuestDetails/GuestDetail+/SelectedDining+
                    foreach (XmlNode currentONodeChild5 in oNode.SelectNodes("SelectedDining"))
                    {
                        oNodeChild = currentONodeChild5;
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
                            else if (string.Compare(Language, oNodeChild.Attributes["Language"].Value) != 0)
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
                            else if (string.Compare(DiningRoom, oNodeChild.Attributes["DiningRoom"].Value) != 0)
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
                            else if (string.Compare(TableSize, oNodeChild.Attributes["TableSize"].Value) != 0)
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
                            else if (string.Compare(AgeCode, oNodeChild.Attributes["AgeCode"].Value) != 0)
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

                    // Check Air Deviation Request
                    if (oNode.SelectSingleNode("AirDeviationRequests/AirDeviationRequest") is not null)
                    {
                        if (modMain.GetCruiseFilterValue(ref ttCruiseProfiles, mstrVendorCode, "airDeviationSupported") == "false")
                        {
                            throw new Exception("Air deviation not supported by this cruise line.");
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
                    foreach (XmlNode currentONodeGrandChild4 in oNodeChild.SelectNodes("GivenName"))
                    {
                        oNodeGrandChild = currentONodeGrandChild4;
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
                        foreach (XmlNode currentONodeGrandChild5 in oNodeChild.SelectNodes("NameTitle"))
                        {
                            oNodeGrandChild = currentONodeGrandChild5;
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
                CoreLib.SendTrace(UserID, "wsCruiseCreateBooking", "Error *** Filtering CruiseCreateBooking Request", ex.Message, string.Empty);
                throw ex;
            }
            // sb = Nothing
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

                // Validate Rules for CruiseCreateBooking
                strRequest = FilterCruiseCreateBooking(strRequest, ttCredential.UserID);

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

                strResponse = DecodeCruiseCreateBooking(strResponse, ttCredential.UserID);

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
                    CoreLib.SendTrace(ttCredential.UserID, "wsCruiseCreateBooking", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 

        [WebMethod(Description = "Process Cruise Create Booking Messages Request.")]
        public wmCruiseCreateBookingOut.OTA_CruiseCreateBookingRS wmCruiseCreateBooking(wmCruiseCreateBookingIn.OTA_CruiseCreateBookingRQ OTA_CruiseCreateBookingRQ)
        {
            string xmlMessage = "";
            wmCruiseCreateBookingOut.OTA_CruiseCreateBookingRS oCruiseCreateBookingRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmCruiseCreateBookingIn.OTA_CruiseCreateBookingRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_CruiseCreateBookingRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            xmlMessage = xmlMessage.Replace(" xmlns=\"http://tripxml.downtowntravel.com/tripxml/wsCruiseCreateBooking\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CruiseCreateBooking);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmCruiseCreateBookingOut.OTA_CruiseCreateBookingRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oCruiseCreateBookingRS = (wmCruiseCreateBookingOut.OTA_CruiseCreateBookingRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsCruiseCreateBooking", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oCruiseCreateBookingRS;

        }

        [WebMethod(Description = "Process Cruise Create Booking Xml Messages Request.")]
        public string wmCruiseCreateBookingXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.CruiseCreateBooking);
        }

        #endregion

    }

}