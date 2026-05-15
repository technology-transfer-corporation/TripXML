using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmAirAvailIn_v03
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
    public class StreetNmbr
    {

        [XmlAttribute()]
        public string PO_Box;

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

    [XmlRoot(IsNullable = false)]
    public class StateProv
    {

        [XmlAttribute()]
        public string StateCode;

        [XmlText()]
        public string Value;
    }

    public enum AddressShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum AddressShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class AdvResTicketing
    {

        public AdvReservation AdvReservation;

        public AdvTicketing AdvTicketing;

        [XmlAttribute()]
        public bool AdvResInd;

        [XmlIgnore()]
        public bool AdvResIndSpecified;

        [XmlAttribute()]
        public bool AdvTicketingInd;

        [XmlIgnore()]
        public bool AdvTicketingIndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class AdvReservation
    {

        [XmlAttribute()]
        public DateTime LatestTimeOfDay;

        [XmlIgnore()]
        public bool LatestTimeOfDaySpecified;

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
        public DateTime FromResTimeOfDay;

        [XmlIgnore()]
        public bool FromResTimeOfDaySpecified;

        [XmlAttribute()]
        public string FromResPeriod;

        [XmlAttribute()]
        public AdvTicketingFromResUnit FromResUnit;

        [XmlIgnore()]
        public bool FromResUnitSpecified;

        [XmlAttribute()]
        public DateTime FromDepartTimeOfDay;

        [XmlIgnore()]
        public bool FromDepartTimeOfDaySpecified;

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

        [XmlAttribute()]
        public AirTravelerGender Gender;

        [XmlIgnore()]
        public bool GenderSpecified;

        [XmlAttribute()]
        public AirTravelerShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AirTravelerShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate;

        [XmlIgnore()]
        public bool BirthDateSpecified;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public string PassengerTypeCode;

        [XmlAttribute()]
        public bool AccompaniedByInfant;

        [XmlIgnore()]
        public bool AccompaniedByInfantSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class ProfileRef
    {

        public UniqueID UniqueID;
    }

    [XmlRoot(IsNullable = false)]
    public class UniqueID
    {

        public CompanyName CompanyName;

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

        [XmlAttribute()]
        public PersonNameShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public PersonNameShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string NameType;
    }

    public enum PersonNameShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum PersonNameShareMarketInd
    {

        Yes,

        No,

        Inherit
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

    public enum TelephoneShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum TelephoneShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class Email
    {

        [XmlAttribute()]
        public EmailShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public EmailShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public string EmailType;

        [XmlText()]
        public string Value;
    }

    public enum EmailShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum EmailShareMarketInd
    {

        Yes,

        No,

        Inherit
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

    public enum CustLoyaltyShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum CustLoyaltyShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum CustLoyaltySingleVendorInd
    {

        SingleVndr,

        Alliance
    }

    [XmlRoot(IsNullable = false)]
    public class Document
    {

        public string DocHolderName;

        [XmlElement("DocLimitations")]
        public string[] DocLimitations;

        [XmlAttribute()]
        public DocumentShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public DocumentShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string DocIssueAuthority;

        [XmlAttribute()]
        public string DocIssueLocation;

        [XmlAttribute()]
        public string DocID;

        [XmlAttribute()]
        public string DocType;

        [XmlAttribute()]
        public DocumentGender Gender;

        [XmlIgnore()]
        public bool GenderSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate;

        [XmlIgnore()]
        public bool BirthDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;
    }

    public enum DocumentShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum DocumentShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum DocumentGender
    {

        Male,

        Female,

        Unknown
    }

    [XmlRoot(IsNullable = false)]
    public class PassengerTypeQuantity
    {

        [XmlAttribute()]
        public int Age;

        [XmlIgnore()]
        public bool AgeSpecified;

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

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(true)]
        public bool Changeable = true;
    }

    [XmlRoot(IsNullable = false)]
    public class TravelerRefNumber
    {

        [XmlAttribute()]
        public string RPH;
    }

    public enum AirTravelerGender
    {

        Male,

        Female,

        Unknown
    }

    public enum AirTravelerShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum AirTravelerShareMarketInd
    {

        Yes,

        No,

        Inherit
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
        public bool CrossDateAllowedIndicator;

        [XmlIgnore()]
        public bool CrossDateAllowedIndicatorSpecified;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class BookingChannel
    {

        public CompanyName CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public bool Primary;

        [XmlIgnore()]
        public bool PrimarySpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class BookingClassPref
    {

        [XmlAttribute()]
        public string ResBookDesigCode;

        [XmlAttribute()]
        public BookingClassPrefPreferLevel PreferLevel;
    }

    public enum BookingClassPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class CabinPref
    {

        [XmlAttribute()]
        public CabinPrefPreferLevel PreferLevel;

        [XmlAttribute()]
        public CabinPrefCabin Cabin;

        [XmlIgnore()]
        public bool CabinSpecified;
    }

    public enum CabinPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum CabinPrefCabin
    {

        First,

        Business,

        Economy
    }

    [XmlRoot(IsNullable = false)]
    public class ConnectionLocation
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(true)]
        public bool Inclusive = true;

        [XmlAttribute()]
        public ConnectionLocationPreferLevel PreferLevel;

        [XmlAttribute()]
        public int MinChangeTime;

        [XmlIgnore()]
        public bool MinChangeTimeSpecified;

        [XmlAttribute()]
        public ConnectionLocationConnectionInfo ConnectionInfo;

        [XmlIgnore()]
        public bool ConnectionInfoSpecified;

        [XmlText()]
        public string Value;
    }

    public enum ConnectionLocationPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
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
        public bool CrossDateAllowedIndicator;

        [XmlIgnore()]
        public bool CrossDateAllowedIndicatorSpecified;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class DestinationLocation
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
    public class EquipPref
    {

        [XmlAttribute()]
        public string AirEquipType;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool ChangeofGauge = false;

        [XmlAttribute()]
        public EquipPrefPreferLevel PreferLevel;

        [XmlAttribute()]
        public bool WideBody;

        [XmlIgnore()]
        public bool WideBodySpecified;

        [XmlText()]
        public string Value;
    }

    public enum EquipPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class FareRestrictPref
    {

        public AdvResTicketing AdvResTicketing;

        public StayRestrictions StayRestrictions;

        public VoluntaryChanges VoluntaryChanges;

        [XmlAttribute()]
        public FareRestrictPrefPreferLevel PreferLevel;

        [XmlAttribute()]
        public string FareRestriction;
    }

    [XmlRoot(IsNullable = false)]
    public class StayRestrictions
    {

        public MinimumStay MinimumStay;

        public MaximumStay MaximumStay;

        [XmlAttribute()]
        public bool StayRestrictionsInd;

        [XmlIgnore()]
        public bool StayRestrictionsIndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class MinimumStay
    {

        [XmlAttribute()]
        public DateTime ReturnTimeOfDay;

        [XmlIgnore()]
        public bool ReturnTimeOfDaySpecified;

        [XmlAttribute()]
        public int MinStay;

        [XmlIgnore()]
        public bool MinStaySpecified;

        [XmlAttribute()]
        public MinimumStayStayUnit StayUnit;

        [XmlIgnore()]
        public bool StayUnitSpecified;

        [XmlAttribute()]
        public DateTime MinStayDate;

        [XmlIgnore()]
        public bool MinStayDateSpecified;
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
        public DateTime ReturnTimeOfDay;

        [XmlIgnore()]
        public bool ReturnTimeOfDaySpecified;

        [XmlAttribute()]
        public int MaxStay;

        [XmlIgnore()]
        public bool MaxStaySpecified;

        [XmlAttribute()]
        public MaximumStayStayUnit StayUnit;

        [XmlIgnore()]
        public bool StayUnitSpecified;

        [XmlAttribute()]
        public DateTime MaxStayDate;

        [XmlIgnore()]
        public bool MaxStayDateSpecified;
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
        public bool VolChangeInd;

        [XmlIgnore()]
        public bool VolChangeIndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class Penalty
    {

        [XmlAttribute()]
        public string PenaltyType;

        [XmlAttribute()]
        public string DepartureStatus;

        [XmlAttribute()]
        public double Amount;

        [XmlIgnore()]
        public bool AmountSpecified;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public int DecimalPlaces;

        [XmlIgnore()]
        public bool DecimalPlacesSpecified;

        [XmlAttribute()]
        public double Percent;

        [XmlIgnore()]
        public bool PercentSpecified;
    }

    public enum FareRestrictPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
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

        [XmlAttribute()]
        public FlightTypePrefPreferLevel PreferLevel;

        [XmlAttribute()]
        public FlightTypePrefFlightType FlightType;

        [XmlIgnore()]
        public bool FlightTypeSpecified;

        [XmlAttribute()]
        public int MaxConnections;

        [XmlIgnore()]
        public bool MaxConnectionsSpecified;
    }

    public enum FlightTypePrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum FlightTypePrefFlightType
    {

        Nonstop,

        Direct,

        Connection
    }

    [XmlRoot(IsNullable = false)]
    public class NegotiatedFareCode
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

        public OriginDestinationInformation OriginDestinationInformation;

        public SpecificFlightInfo SpecificFlightInfo;

        public TravelPreferences TravelPreferences;

        public TravelerInfoSummary TravelerInfoSummary;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_AirAvailRQTarget.Production)]
        public OTA_AirAvailRQTarget Target = OTA_AirAvailRQTarget.Production;

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
        public OTA_AirAvailRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public int MaxResponses;

        [XmlIgnore()]
        public bool MaxResponsesSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DirectFlightsOnly = false;

        [XmlAttribute()]
        public int NumberStops;

        [XmlIgnore()]
        public bool NumberStopsSpecified;
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

        [XmlElement("Name")]
        public Name[] Name;

        [XmlElement("System")]
        public string GDSSystem;

        public string Userid;

        public string Password;
    }

    [XmlRoot(IsNullable = false)]
    public class Name
    {

        [XmlAttribute()]
        public string PseudoCityCode;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class OriginDestinationInformation
    {

        public DepartureDateTime DepartureDateTime;

        public DatePeriod DatePeriod;
        // Public DatePeriod() As Integer

        public ArrivalDateTime ArrivalDateTime;

        public OriginLocation OriginLocation;

        public DestinationLocation DestinationLocation;

        [XmlArrayItem(IsNullable = false)]
        public ConnectionLocation[] ConnectionLocations;
    }

    [XmlRoot(IsNullable = false)]
    public class DatePeriod
    {

        [XmlAttribute()]
        public int Pages;

        [XmlText()]
        public string Value;

    }

    [XmlRoot(IsNullable = false)]
    public class OriginLocation
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
    public class SpecificFlightInfo
    {

        public string FlightNumber;

        public Airline Airline;

        [XmlElement("BookingClassPref")]
        public BookingClassPref[] BookingClassPref;
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
        [System.ComponentModel.DefaultValue(false)]
        public bool SmokingAllowed = false;

        [XmlAttribute()]
        public double OnTimeRate;

        [XmlIgnore()]
        public bool OnTimeRateSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool ETicketDesired = false;

        [XmlAttribute()]
        public int MaxStopsQuantity;

        [XmlIgnore()]
        public bool MaxStopsQuantitySpecified;
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

        [XmlAttribute()]
        public VendorPrefPreferLevel PreferLevel;

        [XmlText()]
        public string Value;
    }

    public enum VendorPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class TicketDistribPref
    {

        [XmlAttribute()]
        public TicketDistribPrefPreferLevel PreferLevel;

        [XmlAttribute()]
        public string DistribType;

        [XmlAttribute()]
        public string TicketTime;

        [XmlText()]
        public string Value;
    }

    public enum TicketDistribPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class TravelerInfoSummary
    {

        [XmlElement("SeatsRequested")]
        public int[] SeatsRequested;

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
        public bool NegotiatedFaresOnly;

        [XmlIgnore()]
        public bool NegotiatedFaresOnlySpecified;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public PriceRequestInformationPricingSource PricingSource;

        [XmlIgnore()]
        public bool PricingSourceSpecified;

        [XmlAttribute()]
        public bool Reprice;

        [XmlIgnore()]
        public bool RepriceSpecified;
    }

    public enum PriceRequestInformationPricingSource
    {

        Published,

        Private,

        Both
    }

    public enum OTA_AirAvailRQTarget
    {

        Test,

        Production
    }

    public enum OTA_AirAvailRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }
}
