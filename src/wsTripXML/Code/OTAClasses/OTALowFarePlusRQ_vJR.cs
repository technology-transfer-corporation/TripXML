using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmLowFarePlusIn_vJR
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
        public string LatestTimeOfDay;

        [XmlAttribute()]
        public string LatestPeriod;

        [XmlAttribute()]
        public string LatestUnit;
    }

    [XmlRoot(IsNullable = false)]
    public class AdvTicketing
    {

        [XmlAttribute()]
        public string FromResTimeOfDay;

        [XmlAttribute()]
        public string FromResPeriod;

        [XmlAttribute()]
        public string FromResUnit;

        [XmlAttribute()]
        public string FromDepartTimeOfDay;

        [XmlAttribute()]
        public string FromDepartPeriod;

        [XmlAttribute()]
        public string FromDepartUnit;
    }

    [XmlRoot(IsNullable = false)]
    public class AirTravelerAvail
    {

        [XmlElement("PassengerTypeQuantity")]
        public PassengerTypeQuantity[] PassengerTypeQuantity;

        public AirTravelerType AirTravelerType;
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
    public class AirTravelerType
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

        [XmlArrayItem(IsNullable = false)]
        public FlightSegmentRPH[] FlightSegmentRPHs;

        [XmlAttribute()]
        public AirTravelerTypeGender Gender;

        [XmlIgnore()]
        public bool GenderSpecified;

        [XmlAttribute()]
        public AirTravelerTypeShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AirTravelerTypeShareMarketInd ShareMarketInd;

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

    // local CompanyName removed: identical to shared wsTripXML.Code.CompanyName (XML type-name collision in one serializer scope)

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
    public class TravelerRefNumber
    {

        [XmlAttribute()]
        public string RPH;
    }

    [XmlRoot(IsNullable = false)]
    public class FlightSegmentRPH
    {

        [XmlText()]
        public string[] Text;
    }

    public enum AirTravelerTypeGender
    {

        Male,

        Female,

        Unknown
    }

    public enum AirTravelerTypeShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum AirTravelerTypeShareMarketInd
    {

        Yes,

        No,

        Inherit
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
        [System.ComponentModel.DefaultValue(BookingClassPrefPreferLevel.Preferred)]
        public BookingClassPrefPreferLevel PreferLevel = BookingClassPrefPreferLevel.Preferred;
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
        [System.ComponentModel.DefaultValue(CabinPrefPreferLevel.Preferred)]
        public CabinPrefPreferLevel PreferLevel = CabinPrefPreferLevel.Preferred;

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

        Economy,

        Main,

        Premium
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
        [System.ComponentModel.DefaultValue(ConnectionLocationPreferLevel.Preferred)]
        public ConnectionLocationPreferLevel PreferLevel = ConnectionLocationPreferLevel.Preferred;

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
        [System.ComponentModel.DefaultValue(EquipPrefPreferLevel.Preferred)]
        public EquipPrefPreferLevel PreferLevel = EquipPrefPreferLevel.Preferred;

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
        [System.ComponentModel.DefaultValue(FareRestrictPrefPreferLevel.Preferred)]
        public FareRestrictPrefPreferLevel PreferLevel = FareRestrictPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string FareRestriction;
    }

    [XmlRoot(IsNullable = false)]
    public class StayRestrictions
    {

        public MinimumStay MinimumStay;

        public MaximumStay MaximumStay;

        [XmlAttribute()]
        public bool StayRestrictionInd;

        [XmlIgnore()]
        public bool StayRestrictionIndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class MinimumStay
    {

        [XmlAttribute()]
        public string ReturnTimeOfDay;

        [XmlAttribute()]
        public string MinStay;

        [XmlAttribute()]
        public string StayUnit;

        [XmlAttribute()]
        public string MinStayDate;
    }

    [XmlRoot(IsNullable = false)]
    public class MaximumStay
    {

        [XmlAttribute()]
        public string ReturnType;

        [XmlAttribute()]
        public string ReturnTimeOfDay;

        [XmlAttribute()]
        public string MaxStay;

        [XmlAttribute()]
        public string StayUnit;

        [XmlAttribute()]
        public string MaxStayDate;
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
        public string Amount;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public string DecimalPlaces;

        [XmlAttribute()]
        public string Percent;
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
        public FlightSegmentRPH[] FlightSegmentRPH;
    }

    [XmlRoot(IsNullable = false)]
    public class FlightTypePref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(FlightTypePrefPreferLevel.Preferred)]
        public FlightTypePrefPreferLevel PreferLevel = FlightTypePrefPreferLevel.Preferred;

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
    public class OTA_AirLowFareSearchPlusRQ
    {

        public POS POS;

        [XmlElement("OriginDestinationInformation")]
        public OriginDestinationInformation[] OriginDestinationInformation;

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
        [System.ComponentModel.DefaultValue(OTA_AirLowFareSearchPlusRQTarget.Production)]
        public OTA_AirLowFareSearchPlusRQTarget Target = OTA_AirLowFareSearchPlusRQTarget.Production;

        [XmlAttribute()]
        public double Version;

        [XmlIgnore()]
        public bool VersionSpecified;

        [XmlAttribute()]
        public int SequenceNmbr;

        [XmlIgnore()]
        public bool SequenceNmbrSpecified;

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
        public bool AvailableFlightsOnly;

        [XmlIgnore()]
        public bool AvailableFlightsOnlySpecified;
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

        [XmlAttribute()]
        public string ListOfSupplierCode;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class OriginDestinationInformation
    {

        public DepartureDateTime DepartureDateTime;

        public ArrivalDateTime ArrivalDateTime;

        public OriginLocation OriginLocation;

        public DestinationLocation DestinationLocation;

        [XmlArrayItem(IsNullable = false)]
        public ConnectionLocation[] ConnectionLocations;
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


        [XmlElement("AClasses")]
        public AClasses[] AClasses;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(VendorPrefPreferLevel.Preferred)]
        public VendorPrefPreferLevel PreferLevel = VendorPrefPreferLevel.Preferred;

        [XmlText()]
        public string Value;


    }

    [XmlRoot(IsNullable = false)]
    public class AClasses
    {

        [XmlAttribute()]
        public string PreferLevel;

        [XmlElement("AClass")]
        public string[] AClass;

    }

    // '<remarks/>
    // <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    // Public Class CClass
    // '<remarks/>
    // <System.Xml.Serialization.XmlElementAttribute("CClass")> _
    // Public CClass() As Char
    // End Class


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
        [System.ComponentModel.DefaultValue(TicketDistribPrefPreferLevel.Preferred)]
        public TicketDistribPrefPreferLevel PreferLevel = TicketDistribPrefPreferLevel.Preferred;

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

    public enum OTA_AirLowFareSearchPlusRQTarget
    {

        Test,

        Production
    }
}
