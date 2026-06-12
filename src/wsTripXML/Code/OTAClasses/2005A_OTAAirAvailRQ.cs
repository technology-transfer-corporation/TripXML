using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmAirAvail2005AIn
{

    [XmlRoot(IsNullable = false)]
    public class AdditionalPersonNames
    {

        [XmlElement("AdditionalPersonName")]
        public string[] AdditionalPersonName;
    }

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
        public string FormattedInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public AddressOperation Operation;

        [XmlIgnore()]
        public bool OperationSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class StreetNmbr
    {

        [XmlAttribute()]
        public string PO_Box;

        [XmlAttribute()]
        public string StreetNmbrSuffix;

        [XmlAttribute()]
        public string StreetDirection;

        [XmlAttribute()]
        public string RuralRouteNmbr;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class StateProv
    {

        [XmlAttribute()]
        public string StateCode;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class CountryName
    {

        [XmlAttribute()]
        public string Code;

        [XmlText()]
        public string Value;
    }

    public enum AddressOperation
    {

        [XmlEnum("Add-Update")]
        AddUpdate,

        Cancel,

        Delete,

        Add,

        Replace
    }

    [XmlRoot(IsNullable = false)]
    public class AdvResTicketing
    {

        public AdvReservation AdvReservation;

        public AdvTicketing AdvTicketing;

        [XmlAttribute()]
        public string AdvResInd;

        [XmlAttribute()]
        public string AdvTicketingInd;

        [XmlAttribute()]
        public string RequestedTicketingDate;
    }

    [XmlRoot(IsNullable = false)]
    public class AdvReservation
    {

        [XmlAttribute()]
        public string LatestTimeOfDay;

        [XmlAttribute()]
        public string LatestPeriod;

        [XmlAttribute()]
        public AdvReservationLatestUnit LatestUnit;

        [XmlIgnore()]
        public bool LatestUnitSpecified;
    }

    public enum AdvReservationLatestUnit
    {

        Minutes,

        Hours,

        Days,

        Months,

        MON,

        TUES,

        WED,

        THU,

        FRI,

        SAT,

        SUN
    }

    [XmlRoot(IsNullable = false)]
    public class AdvTicketing
    {

        [XmlAttribute()]
        public string FromResTimeOfDay;

        [XmlAttribute()]
        public string FromResPeriod;

        [XmlAttribute()]
        public AdvTicketingFromResUnit FromResUnit;

        [XmlIgnore()]
        public bool FromResUnitSpecified;

        [XmlAttribute()]
        public string FromDepartTimeOfDay;

        [XmlAttribute()]
        public string FromDepartPeriod;

        [XmlAttribute()]
        public AdvTicketingFromDepartUnit FromDepartUnit;

        [XmlIgnore()]
        public bool FromDepartUnitSpecified;
    }

    public enum AdvTicketingFromResUnit
    {

        Minutes,

        Hours,

        Days,

        Months,

        MON,

        TUES,

        WED,

        THU,

        FRI,

        SAT,

        SUN
    }

    public enum AdvTicketingFromDepartUnit
    {

        Minutes,

        Hours,

        Days,

        Months,

        MON,

        TUES,

        WED,

        THU,

        FRI,

        SAT,

        SUN
    }

    [XmlRoot(IsNullable = false)]
    public class AirTraveler
    {

        public ProfileRef ProfileRef;

        public PersonName PersonName;

        [XmlElement("Telephone")]
        public Telephone[] Telephone;

        [XmlElement("Email")]
        public Email[] Email;

        [XmlElement("Address")]
        public Address[] Address;

        [XmlElement("CustLoyalty")]
        public CustLoyalty[] CustLoyalty;

        [XmlElement("Document")]
        public Document[] Document;

        public PassengerTypeQuantity PassengerTypeQuantity;

        public TravelerRefNumber TravelerRefNumber;

        [XmlArrayItem("FlightSegmentRPH", IsNullable = false)]
        public string[] FlightSegmentRPHs;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string Gender;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string BirthDate;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public string PassengerTypeCode;

        [XmlAttribute()]
        public string AccompaniedByInfant;
    }

    [XmlRoot(IsNullable = false)]
    public class ProfileRef
    {

        public UniqueID UniqueID;
    }

    [XmlRoot(IsNullable = false)]
    public class UniqueID
    {

        public Code.CompanyName CompanyName;

        [XmlAttribute()]
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

    // local CompanyName removed: identical in shape to the shared wsTripXML.Code.CompanyName,
    // and both reachable in one XmlSerializer scope would collide on the XML type name
    // (the legacy ASMX WSDL also exposes a single CompanyName type)

    [XmlRoot(IsNullable = false)]
    public class PersonName
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
    public class Telephone
    {

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

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
        public string FormattedInd;

        [XmlAttribute()]
        public string DefaultInd;

        [XmlAttribute()]
        public string PhoneUseType;

        [XmlAttribute()]
        public TelephoneOperation Operation;

        [XmlIgnore()]
        public bool OperationSpecified;
    }

    public enum TelephoneOperation
    {

        [XmlEnum("Add-Update")]
        AddUpdate,

        Cancel,

        Delete,

        Add,

        Replace
    }

    [XmlRoot(IsNullable = false)]
    public class Email
    {

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string DefaultInd;

        [XmlAttribute()]
        public string EmailType;

        [XmlAttribute()]
        public EmailOperation Operation;

        [XmlIgnore()]
        public bool OperationSpecified;

        [XmlText()]
        public string Value;
    }

    public enum EmailOperation
    {

        [XmlEnum("Add-Update")]
        AddUpdate,

        Cancel,

        Delete,

        Add,

        Replace
    }

    [XmlRoot(IsNullable = false)]
    public class CustLoyalty
    {

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string ProgramID;

        [XmlAttribute()]
        public string MembershipID;

        [XmlAttribute()]
        public string TravelSector;

        [XmlAttribute()]
        public string LoyalLevel;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string SingleVendorInd;

        [XmlAttribute()]
        public string SignupDate;

        [XmlAttribute()]
        public string EffectiveDate;

        [XmlAttribute()]
        public string ExpireDate;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public string VendorCode;

        [XmlAttribute()]
        public CustLoyaltyOperation Operation;

        [XmlIgnore()]
        public bool OperationSpecified;
    }

    public enum CustLoyaltyOperation
    {

        [XmlEnum("Add-Update")]
        AddUpdate,

        Cancel,

        Delete,

        Add,

        Replace
    }

    [XmlRoot(IsNullable = false)]
    public class Document
    {

        [XmlElement("DocHolderName", typeof(string))]
        [XmlElement("DocHolderFormattedName", typeof(DocHolderFormattedName))]
        public object Item;

        [XmlElement("DocLimitations")]
        public string[] DocLimitations;

        [XmlArrayItem("AdditionalPersonName", IsNullable = false)]
        public string[] AdditionalPersonNames;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string DocIssueAuthority;

        [XmlAttribute()]
        public string DocIssueLocation;

        [XmlAttribute()]
        public string DocID;

        [XmlAttribute()]
        public string DocType;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string Gender;

        [XmlAttribute()]
        public string BirthDate;

        [XmlAttribute()]
        public string EffectiveDate;

        [XmlAttribute()]
        public string ExpireDate;

        [XmlAttribute()]
        public string DocIssueStateProv;

        [XmlAttribute()]
        public string DocIssueCountry;

        [XmlAttribute()]
        public DocumentOperation Operation;

        [XmlIgnore()]
        public bool OperationSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class DocHolderFormattedName
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

    public enum DocumentOperation
    {

        [XmlEnum("Add-Update")]
        AddUpdate,

        Cancel,

        Delete,

        Add,

        Replace
    }

    [XmlRoot(IsNullable = false)]
    public class PassengerTypeQuantity
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

        [XmlAttribute()]
        public string Changeable;
    }

    [XmlRoot(IsNullable = false)]
    public class TravelerRefNumber
    {

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public string SurnameRefNumber;
    }

    [XmlRoot(IsNullable = false)]
    public class AirTravelerAvail
    {

        [XmlElement("PassengerTypeQuantity")]
        public PassengerTypeQuantity[] PassengerTypeQuantity;

        public AirTraveler AirTraveler;
    }

    [XmlRoot(IsNullable = false)]
    public class Airline
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
    public class ArrivalDateTime
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
    public class BookingChannel
    {

        public Code.CompanyName CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public string Primary;
    }

    [XmlRoot(IsNullable = false)]
    public class BookingClassPref
    {

        [XmlAttribute()]
        public string ResBookDesigCode;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PreferLevel;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ResBookDesigCodeType;
    }

    [XmlRoot(IsNullable = false)]
    public class CabinPref
    {

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PreferLevel;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string Cabin;
    }

    [XmlRoot(IsNullable = false)]
    public class ConnectionLocation
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute()]
        public string Inclusive;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PreferLevel;

        [XmlAttribute()]
        public string MinChangeTime;

        [XmlAttribute()]
        public ConnectionLocationConnectionInfo ConnectionInfo;

        [XmlIgnore()]
        public bool ConnectionInfoSpecified;

        [XmlText()]
        public string Value;
    }

    public enum ConnectionLocationConnectionInfo
    {

        Via,

        Stop,

        Change
    }

    [XmlRoot(IsNullable = false)]
    public class ConnectionLocations
    {

        [XmlElement("ConnectionLocation")]
        public ConnectionLocation[] ConnectionLocation;
    }

    [XmlRoot(IsNullable = false)]
    public class DepartureDateTime
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
    public class DestinationLocation
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute()]
        public string MultiAirportCityInd;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class EquipPref
    {

        [XmlAttribute()]
        public string AirEquipType;

        [XmlAttribute()]
        public string ChangeofGauge;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PreferLevel;

        [XmlAttribute()]
        public string WideBody;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class FareRestrictPref
    {

        public AdvResTicketing AdvResTicketing;

        public StayRestrictions StayRestrictions;

        public VoluntaryChanges VoluntaryChanges;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PreferLevel;

        [XmlAttribute()]
        public string FareRestriction;

        [XmlAttribute()]
        public string FareDisplayCurrency;

        [XmlAttribute()]
        public string CurrencyOverride;
    }

    [XmlRoot(IsNullable = false)]
    public class StayRestrictions
    {

        public MinimumStay MinimumStay;

        public MaximumStay MaximumStay;

        [XmlAttribute()]
        public string StayRestrictionsInd;
    }

    [XmlRoot(IsNullable = false)]
    public class MinimumStay
    {

        [XmlAttribute()]
        public string ReturnTimeOfDay;

        [XmlAttribute()]
        public string MinStay;

        [XmlAttribute()]
        public MinimumStayStayUnit StayUnit;

        [XmlIgnore()]
        public bool StayUnitSpecified;

        [XmlAttribute()]
        public string MinStayDate;
    }

    public enum MinimumStayStayUnit
    {

        Minutes,

        Hours,

        Days,

        Months,

        MON,

        TUES,

        WED,

        THU,

        FRI,

        SAT,

        SUN
    }

    [XmlRoot(IsNullable = false)]
    public class MaximumStay
    {

        [XmlAttribute()]
        public MaximumStayReturnType ReturnType;

        [XmlIgnore()]
        public bool ReturnTypeSpecified;

        [XmlAttribute()]
        public string ReturnTimeOfDay;

        [XmlAttribute()]
        public string MaxStay;

        [XmlAttribute()]
        public MaximumStayStayUnit StayUnit;

        [XmlIgnore()]
        public bool StayUnitSpecified;

        [XmlAttribute()]
        public string MaxStayDate;
    }

    public enum MaximumStayReturnType
    {

        C,

        S
    }

    public enum MaximumStayStayUnit
    {

        Minutes,

        Hours,

        Days,

        Months,

        MON,

        TUES,

        WED,

        THU,

        FRI,

        SAT,

        SUN
    }

    [XmlRoot(IsNullable = false)]
    public class VoluntaryChanges
    {

        public Penalty Penalty;

        [XmlAttribute()]
        public string VolChangeInd;
    }

    [XmlRoot(IsNullable = false)]
    public class Penalty
    {

        [XmlAttribute()]
        public string PenaltyType;

        [XmlAttribute()]
        public string DepartureStatus;

        [XmlAttribute()]
        public string Amount;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public string DecimalPlaces;

        [XmlAttribute()]
        public string Percent;
    }

    [XmlRoot(IsNullable = false)]
    public class FlightSegmentRPHs
    {

        [XmlElement("FlightSegmentRPH")]
        public string[] FlightSegmentRPH;
    }

    [XmlRoot(IsNullable = false)]
    public class FlightTypePref
    {

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PreferLevel;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string FlightType;

        [XmlAttribute()]
        public string MaxConnections;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string NonScheduledFltInfo;

        [XmlAttribute()]
        public string BackhaulIndicator;

        [XmlAttribute()]
        public string GroundTransportIndicator;

        [XmlAttribute()]
        public string DirectAndNonStopOnlyInd;

        [XmlAttribute()]
        public string NonStopsOnlyInd;

        [XmlAttribute()]
        public string OnlineConnectionsOnlyInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string RoutingType;
    }

    [XmlRoot(IsNullable = false)]
    public class NegotiatedFareCode
    {

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute()]
        public string URI;

        [XmlAttribute()]
        public string Quantity;

        [XmlAttribute()]
        public string SecondaryCode;

        [XmlAttribute()]
        public string SupplierCode;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_AirAvailRQ
    {

        public POS POS;

        public TPA_Extensions TPA_Extensions;

        public ProcessingInfo ProcessingInfo;

        [XmlElement("OriginDestinationInformation")]
        public OriginDestinationInformation[] OriginDestinationInformation;

        public SpecificFlightInfo SpecificFlightInfo;

        public TravelPreferences TravelPreferences;

        public TravelerInfoSummary TravelerInfoSummary;

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
        public string DirectFlightsOnly;

        [XmlAttribute()]
        public string NumberStops;
    }
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }
    [XmlRoot(IsNullable = false)]
    public class Position
    {

        [XmlAttribute()]
        public string Latitude;

        [XmlAttribute()]
        public string Longitude;

        [XmlAttribute()]
        public string Altitude;
    }

    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;

        public string MoreIndicator;
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
    public class ProcessingInfo
    {

        [XmlAttribute(DataType = "NMTOKEN")]
        public string TargetSource;

        [XmlAttribute()]
        public string FlightSvcInfoIndicator;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string DisplayOrder;

        [XmlAttribute()]
        public string ReducedDataIndicator;

        [XmlAttribute()]
        public string BaseFaresOnlyIndicator;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string SearchType;

        [XmlAttribute()]
        public string AvailabilityIndicator;
    }

    [XmlRoot(IsNullable = false)]
    public class OriginDestinationInformation
    {

        [XmlElement("DepartureDateTime", typeof(DepartureDateTime))]
        [XmlElement("ArrivalDateTime", typeof(ArrivalDateTime))]
        public object Item;

        public OriginLocation OriginLocation;

        public DestinationLocation DestinationLocation;

        [XmlArrayItem(IsNullable = false)]
        public ConnectionLocation[] ConnectionLocations;

        public SpecificFlightInfo SpecificFlightInfo;

        public TravelPreferences TravelPreferences;

        [XmlAttribute()]
        public string SameAirportInd;
    }

    [XmlRoot(IsNullable = false)]
    public class OriginLocation
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute()]
        public string MultiAirportCityInd;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class SpecificFlightInfo
    {

        public string FlightNumber;

        public Airline Airline;

        public BookingClassPref BookingClassPref;
    }

    [XmlRoot(IsNullable = false)]
    public class TravelPreferences
    {

        [XmlElement("VendorPref")]
        public VendorPref[] VendorPref;

        [XmlElement("FlightTypePref")]
        public FlightTypePref[] FlightTypePref;

        [XmlElement("FareRestrictPref")]
        public FareRestrictPref[] FareRestrictPref;

        [XmlElement("EquipPref")]
        public EquipPref[] EquipPref;

        [XmlElement("CabinPref")]
        public CabinPref[] CabinPref;

        [XmlElement("TicketDistribPref")]
        public TicketDistribPref[] TicketDistribPref;

        [XmlAttribute()]
        public string SmokingAllowed;

        [XmlAttribute()]
        public string OnTimeRate;

        [XmlAttribute()]
        public string ETicketDesired;

        [XmlAttribute()]
        public string MaxStopsQuantity;
    }

    [XmlRoot(IsNullable = false)]
    public class VendorPref
    {

        [XmlAttribute()]
        public string CompanyShortName;

        [XmlAttribute()]
        public string TravelSector;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PreferLevel;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class TicketDistribPref
    {

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PreferLevel;

        [XmlAttribute()]
        public string DistribType;

        [XmlAttribute()]
        public string TicketTime;

        [XmlAttribute()]
        public string LastTicketDate;

        [XmlAttribute()]
        public string FirstTicketDate;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class TravelerInfoSummary
    {

        [XmlElement("SeatsRequested")]
        public string[] SeatsRequested;

        [XmlElement("AirTravelerAvail")]
        public AirTravelerAvail[] AirTravelerAvail;

        public PriceRequestInformation PriceRequestInformation;
    }

    [XmlRoot(IsNullable = false)]
    public class PriceRequestInformation
    {

        [XmlElement("NegotiatedFareCode")]
        public NegotiatedFareCode[] NegotiatedFareCode;

        [XmlAttribute()]
        public string FareQualifier;

        [XmlAttribute()]
        public string NegotiatedFaresOnly;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public PriceRequestInformationPricingSource PricingSource;

        [XmlIgnore()]
        public bool PricingSourceSpecified;

        [XmlAttribute()]
        public string Reprice;
    }

    public enum PriceRequestInformationPricingSource
    {

        Published,

        Private,

        Both
    }

}