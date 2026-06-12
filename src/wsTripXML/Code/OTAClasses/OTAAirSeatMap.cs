using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmAirSeatMap
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AirTraveler
    {

        // <remarks/>
        public PersonName PersonName;

        // <remarks/>
        [XmlElement("CustLoyalty")]
        public CustLoyalty[] CustLoyalty;

        // <remarks/>
        public PassengerTypeQuantity PassengerTypeQuantity;

        // <remarks/>
        public TravelerRefNumber TravelerRefNumber;

        // <remarks/>
        [XmlAttribute()]
        public AirTravelerGender Gender;

        // <remarks/>
        [XmlIgnore()]
        public bool GenderSpecified;

        // <remarks/>
        [XmlAttribute()]
        public AirTravelerShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public AirTravelerShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate;

        // <remarks/>
        [XmlIgnore()]
        public bool BirthDateSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute()]
        public string PassengerTypeCode;

        // <remarks/>
        [XmlAttribute()]
        public bool AccompaniedByInfant;

        // <remarks/>
        [XmlIgnore()]
        public bool AccompaniedByInfantSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PersonName
    {

        // <remarks/>
        [XmlElement("NamePrefix")]
        public string[] NamePrefix;

        // <remarks/>
        [XmlElement("GivenName")]
        public string[] GivenName;

        // <remarks/>
        [XmlElement("MiddleName")]
        public string[] MiddleName;

        // <remarks/>
        public string SurnamePrefix;

        // <remarks/>
        public string Surname;

        // <remarks/>
        [XmlElement("NameSuffix")]
        public string[] NameSuffix;

        // <remarks/>
        [XmlElement("NameTitle")]
        public string[] NameTitle;

        // <remarks/>
        [XmlAttribute()]
        public PersonNameShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public PersonNameShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string NameType;
    }

    // <remarks/>
    public enum PersonNameShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum PersonNameShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TravelerRefNumber
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PassengerTypeQuantity
    {

        // <remarks/>
        [XmlAttribute()]
        public int Age;

        // <remarks/>
        [XmlIgnore()]
        public bool AgeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string URI;

        // <remarks/>
        [XmlAttribute()]
        public int Quantity;

        // <remarks/>
        [XmlIgnore()]
        public bool QuantitySpecified;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(true)]
        public bool Changeable = true;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CustLoyalty
    {

        // <remarks/>
        [XmlAttribute()]
        public CustLoyaltyShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CustLoyaltyShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string ProgramID;

        // <remarks/>
        [XmlAttribute()]
        public string MembershipID;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string LoyalLevel;

        // <remarks/>
        [XmlAttribute()]
        public CustLoyaltySingleVendorInd SingleVendorInd;

        // <remarks/>
        [XmlIgnore()]
        public bool SingleVendorIndSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime SignupDate;

        // <remarks/>
        [XmlIgnore()]
        public bool SignupDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        // <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    public enum CustLoyaltyShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum CustLoyaltyShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum CustLoyaltySingleVendorInd
    {

        // <remarks/>
        SingleVndr,

        // <remarks/>
        Alliance
    }

    // <remarks/>
    public enum AirTravelerGender
    {

        // <remarks/>
        Male,

        // <remarks/>
        Female,

        // <remarks/>
        Unknown
    }

    // <remarks/>
    public enum AirTravelerShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AirTravelerShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AirTravelers
    {

        // <remarks/>
        [XmlElement("AirTraveler")]
        public AirTraveler[] AirTraveler;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class ArrivalAirport
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class DepartureAirport
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Equipment
    {

        // <remarks/>
        [XmlAttribute()]
        public string AirEquipType;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool ChangeofGauge = false;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class FlightSegmentInfo
    {

        // <remarks/>
        public DepartureAirport DepartureAirport;

        // <remarks/>
        public ArrivalAirport ArrivalAirport;

        // <remarks/>
        public OperatingAirline OperatingAirline;

        // <remarks/>
        [XmlElement("Equipment")]
        public Equipment[] Equipment;

        // <remarks/>
        public MarketingAirline MarketingAirline;

        // <remarks/>
        [XmlAttribute()]
        public string DepartureDateTime;

        // <remarks/>
        [XmlAttribute()]
        public string ArrivalDateTime;

        // <remarks/>
        [XmlIgnore()]
        public bool ArrivalDateTimeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public int StopQuantity;

        // <remarks/>
        [XmlIgnore()]
        public bool StopQuantitySpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public string InfoSource;

        // <remarks/>
        [XmlAttribute()]
        public string FlightNumber;

        // <remarks/>
        [XmlAttribute()]
        public string TourOperatorFlightID;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class OperatingAirline
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlAttribute()]
        public OperatingAirlineFlightNumber FlightNumber;

        // <remarks/>
        [XmlIgnore()]
        public bool FlightNumberSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum OperatingAirlineFlightNumber
    {

        // <remarks/>
        OPEN,

        // <remarks/>
        ARNK
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class MarketingAirline
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class BookingReferenceID
    {

        // <remarks/>
        public CompanyName CompanyName;

        // <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string URL;

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        public string Instance;

        // <remarks/>
        [XmlAttribute()]
        public string ID;

        // <remarks/>
        [XmlAttribute()]
        public string ID_Context;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CompanyName
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum CabinClassCabinType
    {

        // <remarks/>
        First,

        // <remarks/>
        Business,

        // <remarks/>
        Economy
    }

}