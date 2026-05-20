using System;
using System.Xml.Serialization;
using wsTripXML.wsTravelTalk.wmTravelItinerary;

namespace wsTripXML.wsTravelTalk.wmCruiseReadIn
{

    [XmlRoot(IsNullable = false)]
    public class Address
    {

        public StreetNmbr StreetNmbr;

        public string BldgRoom;

        [XmlElement("AddressLine")]
        public string[] AddressLine;

        public string CityName;

        public string PostalCode;

        public string County;

        public StateProv StateProv;

        public CountryName CountryName;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        [XmlAttribute()]
        public AddressShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string Type;
    }

    [XmlRoot(IsNullable = false)]
    public class AddressInfo
    {

        public StreetNmbr StreetNmbr;

        public string BldgRoom;

        [XmlElement("AddressLine")]
        public string[] AddressLine;

        public string CityName;

        public string PostalCode;

        public string County;

        public StateProv StateProv;

        public CountryName CountryName;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        [XmlAttribute()]
        public AddressInfoShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressInfoShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public string UseType;
    }

    public enum AddressInfoShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum AddressInfoShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class AirReadRequest
    {

        public POS POS;

        public AirlineRQ Airline;

        public string FlightNumber;

        public DepartureAirport DepartureAirport;

        [XmlElement(DataType = "date")]
        public DateTime DepartureDate;

        [XmlIgnore()]
        public bool DepartureDateSpecified;

        public string Name;

        public Telephone Telephone;

        public CustLoyalty CustLoyalty;

        public CreditCardInfo CreditCardInfo;

        public TicketNumber TicketNumber;

        [XmlAttribute()]
        public string SeatNumber;

        [XmlAttribute()]
        public bool IncludeFF_EquivPartnerLev;

        [XmlIgnore()]
        public bool IncludeFF_EquivPartnerLevSpecified;

        [XmlAttribute()]
        public bool ReturnFF_Number;

        [XmlIgnore()]
        public bool ReturnFF_NumberSpecified;

        [XmlAttribute()]
        public bool ReturnDownlineSeg;

        [XmlIgnore()]
        public bool ReturnDownlineSegSpecified;

        [XmlAttribute()]
        public AirReadRequestInfoToReturn InfoToReturn;

        [XmlIgnore()]
        public bool InfoToReturnSpecified;

        [XmlAttribute()]
        public AirReadRequestFF_RequestCriteria FF_RequestCriteria;

        [XmlIgnore()]
        public bool FF_RequestCriteriaSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    

    [XmlRoot(IsNullable = false)]
    public class BookingChannel
    {

        public Code.CompanyName CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public bool Primary;

        [XmlIgnore()]
        public bool PrimarySpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        [XmlElement("Provider")]
        public Provider[] Provider;
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
    public class AirlineRQ
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
    public class Telephone
    {

        [XmlAttribute()]
        public TelephoneShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public TelephoneShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string PhoneLocationType;

        [XmlAttribute()]
        public string PhoneTechType;

        [XmlAttribute()]
        public string CountryAccessCode;

        [XmlAttribute()]
        public string AreaCityCode;

        [XmlAttribute()]
        public string PhoneNumber;

        [XmlAttribute()]
        public string Extension;

        [XmlAttribute()]
        public string PIN;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;
    }

    [XmlRoot(IsNullable = false)]
    public class CustLoyalty
    {

        [XmlAttribute()]
        public CustLoyaltyShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public CustLoyaltyShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string ProgramID;

        [XmlAttribute()]
        public string MembershipID;

        [XmlAttribute()]
        public string TravelSector;

        [XmlAttribute()]
        public string LoyalLevel;

        [XmlAttribute()]
        public CustLoyaltySingleVendorInd SingleVendorInd;

        [XmlIgnore()]
        public bool SingleVendorIndSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime SignupDate;

        [XmlIgnore()]
        public bool SignupDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;

        [XmlAttribute()]
        public string RPH;
    }

    [XmlRoot(IsNullable = false)]
    public class CreditCardInfo
    {

        public string CardHolderName;

        public CardIssuerName CardIssuerName;

        public Address Address;

        [XmlAttribute()]
        public CreditCardInfoShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public CreditCardInfoShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CardType;

        [XmlAttribute()]
        public CreditCardInfoCardCode CardCode;

        [XmlIgnore()]
        public bool CardCodeSpecified;

        [XmlAttribute()]
        public string CardNumber;

        [XmlAttribute()]
        public string SeriesCode;

        [XmlAttribute()]
        public string EffectiveDate;

        [XmlAttribute()]
        public string ExpireDate;
    }

    [XmlRoot(IsNullable = false)]
    public class CardIssuerName
    {

        [XmlAttribute()]
        public string BankID;
    }

    public enum CreditCardInfoShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum CreditCardInfoShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum CreditCardInfoCardCode
    {

        AX,

        BC,

        BL,

        CB,

        DN,

        DS,

        EC,

        JC,

        MC,

        TP,

        VI
    }

    [XmlRoot(IsNullable = false)]
    public class TicketNumber
    {

        [XmlElement("TicketAdvisory")]
        public TicketAdvisory[] TicketAdvisory;

        [XmlAttribute()]
        public DateTime TicketTimeLimit;

        [XmlIgnore()]
        public bool TicketTimeLimitSpecified;

        [XmlAttribute()]
        public TicketNumberTicketType TicketType;

        [XmlAttribute()]
        public string eTicketNumber;
    }

    public enum TicketNumberTicketType
    {

        eTicket,

        Paper
    }

    public enum AirReadRequestInfoToReturn
    {

        ListofFF_StatusLevels,

        NoFF_Status,

        All
    }

    public enum AirReadRequestFF_RequestCriteria
    {

        ReturnLevelAndAbove,

        ReturnLevelAndBelow,

        ReturnOnlySpecifiedLevel
    }

    [XmlRoot(IsNullable = false)]
    public class Airport
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlAttribute()]
        public string AirportName;
    }

    [XmlRoot(IsNullable = false)]
    public class ArrivalLocation
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class AssociatedQuantity
    {

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute(DataType = "anyURI")]
        public string URI;

        [XmlAttribute()]
        public int Quantity;

        [XmlIgnore()]
        public bool QuantitySpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class DepartureLocation
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class EndLocation
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlAttribute()]
        public DateTime AssociatedDateTime;

        [XmlIgnore()]
        public bool AssociatedDateTimeSpecified;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class GlobalReservationReadRequest
    {

        public TravelerName TravelerName;

        [XmlAttribute(DataType = "date")]
        public DateTime Start;

        [XmlIgnore()]
        public bool StartSpecified;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute(DataType = "date")]
        public DateTime End;

        [XmlIgnore()]
        public bool EndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class TravelerName
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

        [XmlAttribute()]
        public TravelerNameShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public TravelerNameShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string NameType;
    }

    public enum TravelerNameShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum TravelerNameShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class GolfReadRequest
    {

        [XmlElement("Membership")]
        public Membership[] Membership;

        public string Name;

        [XmlAttribute()]
        public string ID;

        [XmlAttribute()]
        public int RoundID;

        [XmlIgnore()]
        public bool RoundIDSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime PlayDateTime;

        [XmlIgnore()]
        public bool PlayDateTimeSpecified;

        [XmlAttribute()]
        public string PackageID;
    }

    [XmlRoot(IsNullable = false)]
    public class Membership
    {

        [XmlAttribute()]
        public MembershipShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public MembershipShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string ProgramID;

        [XmlAttribute()]
        public string MembershipID;

        [XmlAttribute()]
        public string TravelSector;

        [XmlAttribute()]
        public string LoyalLevel;

        [XmlAttribute()]
        public MembershipSingleVendorInd SingleVendorInd;

        [XmlIgnore()]
        public bool SingleVendorIndSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime SignupDate;

        [XmlIgnore()]
        public bool SignupDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;

        [XmlAttribute()]
        public string RPH;
    }

    public enum MembershipShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum MembershipShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum MembershipSingleVendorInd
    {

        SingleVndr,

        Alliance
    }

    [XmlRoot(IsNullable = false)]
    public class HotelReadRequest
    {

        public string CityName;

        public Airport Airport;

        public UserID UserID;

        public Verification Verification;

        [XmlAttribute()]
        public string ChainCode;

        [XmlAttribute()]
        public string BrandCode;

        [XmlAttribute()]
        public string HotelCode;

        [XmlAttribute()]
        public string HotelCityCode;

        [XmlAttribute()]
        public string HotelName;

        [XmlAttribute()]
        public string HotelCodeContext;

        [XmlAttribute()]
        public string ChainName;

        [XmlAttribute()]
        public string BrandName;

        [XmlAttribute()]
        public string AreaID;
    }

    [XmlRoot(IsNullable = false)]
    public class UserID
    {

        public Code.CompanyName CompanyName;

        [XmlAttribute(DataType = "anyURI")]
        public string URL;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public string Instance;

        [XmlAttribute()]
        public string ID;

        [XmlAttribute()]
        public string ID_Context;

        [XmlAttribute()]
        public string PinNumber;
    }

    [XmlRoot(IsNullable = false)]
    public class Verification
    {

        public PersonName PersonName;

        public Email Email;

        public TelephoneInfo TelephoneInfo;

        public PaymentCard PaymentCard;

        public AddressInfo AddressInfo;

        [XmlElement("CustLoyalty")]
        public CustLoyalty[] CustLoyalty;

        [XmlElement("Vendor")]
        public VendorRQ[] Vendor;

        public ReservationTimeSpan ReservationTimeSpan;

        [XmlElement("AssociatedQuantity")]
        public AssociatedQuantity[] AssociatedQuantity;

        public StartLocation StartLocation;

        public EndLocation EndLocation;
    }

    [XmlRoot(IsNullable = false)]
    public class TelephoneInfo
    {

        [XmlAttribute()]
        public TelephoneInfoShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public TelephoneInfoShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string PhoneLocationType;

        [XmlAttribute()]
        public string PhoneTechType;

        [XmlAttribute()]
        public string CountryAccessCode;

        [XmlAttribute()]
        public string AreaCityCode;

        [XmlAttribute()]
        public string PhoneNumber;

        [XmlAttribute()]
        public string Extension;

        [XmlAttribute()]
        public string PIN;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public string PhoneUseType;
    }

    public enum TelephoneInfoShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum TelephoneInfoShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentCard
    {

        public string CardHolderName;

        public CardIssuerName CardIssuerName;

        public Address Address;

        [XmlAttribute()]
        public PaymentCardShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public PaymentCardShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CardType;

        [XmlAttribute()]
        public PaymentCardCardCode CardCode;

        [XmlIgnore()]
        public bool CardCodeSpecified;

        [XmlAttribute()]
        public string CardNumber;

        [XmlAttribute()]
        public string SeriesCode;

        [XmlAttribute()]
        public string EffectiveDate;

        [XmlAttribute()]
        public string ExpireDate;
    }

    [XmlRoot(IsNullable = false)]
    public class VendorRQ
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
    public class ReservationTimeSpan
    {

        [XmlAttribute(DataType = "date")]
        public DateTime Start;

        [XmlIgnore()]
        public bool StartSpecified;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute(DataType = "date")]
        public DateTime End;

        [XmlIgnore()]
        public bool EndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class StartLocation
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlAttribute()]
        public DateTime AssociatedDateTime;

        [XmlIgnore()]
        public bool AssociatedDateTimeSpecified;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_CruiseReadRQ
    {

        public POS POS;

        public UniqueIDRQ UniqueID;

        public ReadRequests ReadRequests;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_ReadRQTarget.Production)]
        public OTA_ReadRQTarget Target = OTA_ReadRQTarget.Production;

        [XmlAttribute()]
        public double Version;

        [XmlIgnore()]
        public bool VersionSpecified;

        [XmlAttribute()]
        public string TransactionIdentifier;

        [XmlAttribute()]
        public int SequenceNmbr;

        [XmlIgnore()]
        public bool SequenceNmbrSpecified;

        [XmlAttribute()]
        public OTA_ReadRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public string ReqRespVersion;

        [XmlAttribute()]
        public string ReservationType;

        [XmlAttribute()]
        public bool ReturnListIndicator;

        [XmlIgnore()]
        public bool ReturnListIndicatorSpecified;

        [XmlAttribute()]
        public int MaxResponses;

        [XmlIgnore()]
        public bool MaxResponsesSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class UniqueIDRQ
    {

        public Code.CompanyName CompanyName;

        [XmlAttribute(DataType = "anyURI")]
        public string URL;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public string Instance;

        [XmlAttribute()]
        public string ID;

        [XmlAttribute()]
        public string ID_Context;
    }

    [XmlRoot(IsNullable = false)]
    public class ReadRequests
    {

        [XmlElement("ReadRequest")]
        public ReadRequest[] ReadRequest;

        [XmlElement("GlobalReservationReadRequest")]
        public GlobalReservationReadRequest[] GlobalReservationReadRequest;

        [XmlElement("HotelReadRequest")]
        public HotelReadRequest[] HotelReadRequest;

        [XmlElement("AirReadRequest")]
        public AirReadRequest[] AirReadRequest;

        [XmlElement("PkgReadRequest")]
        public PkgReadRequest[] PkgReadRequest;

        [XmlElement("GolfReadRequest")]
        public GolfReadRequest[] GolfReadRequest;

        [XmlElement("VehicleReadRequest")]
        public VehicleReadRequest[] VehicleReadRequest;
    }

    [XmlRoot(IsNullable = false)]
    public class ReadRequest
    {

        public UniqueIDRQ UniqueID;

        public Verification Verification;
    }

    [XmlRoot(IsNullable = false)]
    public class PkgReadRequest
    {

        public string Name;

        public ArrivalLocation ArrivalLocation;

        public DepartureLocation DepartureLocation;

        [XmlAttribute()]
        public string TravelCode;

        [XmlAttribute()]
        public string TourCode;

        [XmlAttribute()]
        public string PackageID;

        [XmlAttribute(DataType = "date")]
        public DateTime Start;

        [XmlIgnore()]
        public bool StartSpecified;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute(DataType = "date")]
        public DateTime End;

        [XmlIgnore()]
        public bool EndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class VehicleReadRequest
    {

        public UniqueIDRQ UniqueID;

        public PersonName PersonName;

        public CustLoyalty CustLoyalty;

        public VehRetResRQInfo VehRetResRQInfo;
    }

    [XmlRoot(IsNullable = false)]
    public class VehRetResRQInfo
    {

        public PickUpLocation PickUpLocation;

        public Telephone Telephone;

        public VendorRQ Vendor;

        [XmlAttribute()]
        public DateTime PickUpDateTime;

        [XmlIgnore()]
        public bool PickUpDateTimeSpecified;
    }

    public enum OTA_ReadRQTarget
    {

        Test,

        Production
    }

    public enum OTA_ReadRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }
}
