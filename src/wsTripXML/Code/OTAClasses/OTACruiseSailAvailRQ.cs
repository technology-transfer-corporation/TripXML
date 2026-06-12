using System.Xml.Serialization;


namespace wsTripXML.wsTravelTalk.wmCruiseSailAvailIn
{

    [XmlRoot(IsNullable = false)]
    public class BookingChannel
    {

        public CompanyName CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public string Primary;
    }

    [XmlRoot(IsNullable = false)]
    public class CompanyName
    {

        [XmlAttribute()]
        public string CompanyShortName;

        [XmlAttribute()]
        public string TravelSector;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class Budget
    {

        [XmlAttribute()]
        public string PricingType;

        [XmlAttribute()]
        public string MinPrice;

        [XmlAttribute()]
        public string MaxPrice;

        [XmlAttribute()]
        public string GuidelinePrice;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public string DecimalPlaces;
    }

    [XmlRoot(IsNullable = false)]
    public class CruiseLinePref
    {

        public InclusivePackageOption InclusivePackageOption;

        public SearchQualifiers SearchQualifiers;

        [XmlAttribute()]
        public string VendorCode;

        [XmlAttribute()]
        public string VendorName;

        [XmlAttribute()]
        public string ShipCode;

        [XmlAttribute()]
        public string ShipName;
    }

    [XmlRoot(IsNullable = false)]
    public class InclusivePackageOption
    {

        [XmlAttribute()]
        public string CruisePackageCode;

        [XmlAttribute()]
        public string InclusiveIndicator;

        [XmlAttribute()]
        public string StartDate;
    }

    [XmlRoot(IsNullable = false)]
    public class SearchQualifiers
    {

        public Residency Residency;

        [XmlAttribute()]
        public string LoyaltyMembershipID;

        [XmlAttribute()]
        public string GroupCode;

        [XmlAttribute()]
        public string ReservationID;

        [XmlAttribute()]
        public string FareCode;

        [XmlAttribute()]
        public string BerthedCategoryCode;

        [XmlAttribute()]
        public string PricedCategoryCode;

        [XmlAttribute()]
        public string CabinNumber;

        [XmlAttribute()]
        public string CabinHeldIndicator;
    }

    [XmlRoot(IsNullable = false)]
    public class Residency
    {

        [XmlAttribute()]
        public string StateProvCode;

        [XmlAttribute()]
        public string CountryCode;
    }

    [XmlRoot(IsNullable = false)]
    public class CruiseLinePrefs
    {

        [XmlElement("CruiseLinePref")]
        public CruiseLinePref[] CruiseLinePref;
    }

    [XmlRoot(IsNullable = false)]
    public class DateWindowRange
    {

        [XmlAttribute()]
        public string WindowBefore;

        [XmlAttribute()]
        public string WindowAfter;

        [XmlAttribute()]
        public string CrossDateAllowedIndicator;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class EndDateWindow
    {

        [XmlAttribute()]
        public string EarliestDate;

        [XmlAttribute()]
        public string LatestDate;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string DOW;
    }

    [XmlRoot(IsNullable = false)]
    public class GatewayCity
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        public string CodeContext;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class Guest
    {

        public GuestName GuestName;

        [XmlElement("GuestTransportation")]
        public GuestTransportation[] GuestTransportation;

        [XmlAttribute()]
        public string GuestRPH;

        [XmlAttribute()]
        public string Gender;

        [XmlAttribute()]
        public string Age;

        [XmlAttribute()]
        public string Nationality;

        [XmlAttribute()]
        public string LoyaltyMembershipID;

        [XmlAttribute()]
        public string GuestOccupation;

        [XmlAttribute()]
        public string BirthDate;
    }

    [XmlRoot(IsNullable = false)]
    public class GuestName
    {

        [XmlElement("NamePrefix")]
        public string[] NamePrefix;

        [XmlElement("GivenName")]
        public string[] GivenName;

        [XmlElement("MiddleName")]
        public string[] MiddleName;

        public string SurnamePrefix;

        public string Surname;

        [XmlElement("NameSuffix")]
        public string[] NameSuffix;

        [XmlElement("NameTitle")]
        public string[] NameTitle;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string NameType;
    }

    [XmlRoot(IsNullable = false)]
    public class GuestTransportation
    {

        public GuestCity GuestCity;

        public GatewayCity GatewayCity;

        [XmlAttribute()]
        public string TransportationMode;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string TransportationStatus;
    }

    [XmlRoot(IsNullable = false)]
    public class GuestCity
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        public string CodeContext;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class GuestCount
    {

        [XmlAttribute()]
        public string Age;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute()]
        public string URI;

        [XmlAttribute()]
        public string Quantity;
    }

    [XmlRoot(IsNullable = false)]
    public class GuestCounts
    {

        [XmlElement("GuestCount")]
        public GuestCount[] GuestCount;

        [XmlAttribute()]
        public string GroupIndicator;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_CruiseSailAvailRQ
    {

        public POS POS;

        [XmlElement("Guest")]
        public Guest[] Guest;

        public GuestCounts GuestCounts;

        public SailingDateRange SailingDateRange;

        [XmlArrayItem(IsNullable = false)]
        public CruiseLinePref[] CruiseLinePrefs;

        [XmlElement("RegionPref")]
        public RegionPref[] RegionPref;

        [XmlArrayItem(IsNullable = false)]
        public Provider[] TPA_Extensions;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public string TimeStamp;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string Target;

        [XmlAttribute()]
        public string Version;

        [XmlAttribute()]
        public string TransactionIdentifier;

        [XmlAttribute()]
        public string SequenceNmbr;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string TransactionStatusCode;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public string MaxResponses;

        [XmlAttribute()]
        public string MoreIndicator;

        [XmlAttribute()]
        public string MoreDataEchoToken;
    }

    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }



    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        public string Name;

        public string System;

        public string Userid;

        public string Password;
    }

    [XmlRoot(IsNullable = false)]
    public class SailingDateRange
    {

        [XmlElement("DateWindowRange", typeof(DateWindowRange))]
        [XmlElement("StartDateWindow", typeof(StartDateWindow))]
        [XmlElement("EndDateWindow", typeof(EndDateWindow))]
        public object Item;

        [XmlAttribute()]
        public string Start;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute()]
        public string End;

        [XmlAttribute()]
        public string MinDuration;

        [XmlAttribute()]
        public string MaxDuration;

        [XmlAttribute()]
        public string ListOfDurations;
    }

    [XmlRoot(IsNullable = false)]
    public class StartDateWindow
    {

        [XmlAttribute()]
        public string EarliestDate;

        [XmlAttribute()]
        public string LatestDate;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string DOW;
    }

    [XmlRoot(IsNullable = false)]
    public class RegionPref
    {

        [XmlAttribute()]
        public string RegionCode;

        [XmlAttribute()]
        public string RegionName;
    }

    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        [XmlElement("Provider")]
        public Provider[] Provider;
    }
}
