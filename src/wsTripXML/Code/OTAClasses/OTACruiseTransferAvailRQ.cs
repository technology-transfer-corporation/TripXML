using System.Xml.Serialization;
using wsTripXML.wsTravelTalk.wmCruisePackageAvail;

namespace wsTripXML.wsTravelTalk.wmCruiseTransferAvailIn
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
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_CruiseTransferAvailRQ
    {

        public POS POS;

        [XmlArrayItem(IsNullable = false)]
        public GuestCount[] GuestCounts;

        public SailingInfo SailingInfo;

        [XmlElement("Guest")]
        public Guest[] Guest;

        public SearchQualifiers SearchQualifiers;

        public SelectedFare SelectedFare;

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
    public class SailingInfo
    {

        public SelectedSailing SelectedSailing;

        public TransferOption TransferOption;

        public InclusivePackageOption InclusivePackageOption;

        public Currency Currency;

        public SelectedCategory SelectedCategory;
    }

    [XmlRoot(IsNullable = false)]
    public class TransferOption
    {

        [XmlAttribute()]
        public TransferOptionPackageType TransferType;

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
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        [XmlElement("Provider")]
        public Provider[] Provider;
    }

}
