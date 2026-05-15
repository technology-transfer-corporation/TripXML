using System.Xml.Serialization;
using wsTripXML.wsTravelTalk.wmInfoFares;

namespace wsTripXML.wsTravelTalk.wmFareInfoIn
{

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
    public class FareAccessTypePref
    {

        [XmlArrayItem(IsNullable = false)]
        public NegotiatedFareCode[] NegotiatedFareCodes;

        [XmlAttribute()]
        public FareAccessTypePrefFareAccess FareAccess;

        [XmlIgnore()]
        public bool FareAccessSpecified;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PreferLevel;
    }

    [XmlRoot(IsNullable = false)]
    public class NegotiatedFareCode
    {

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;

        [XmlText()]
        public string Value;
    }

    public enum FareAccessTypePrefFareAccess
    {

        PointToPoint,

        Through,

        Joint,

        Private,

        Negotiated,

        Net
    }

    [XmlRoot(IsNullable = false)]
    public class FareApplicationTypePref
    {

        [XmlAttribute()]
        public FareApplicationTypePrefFareApplicationType FareApplicationType;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PreferLevel;
    }

    public enum FareApplicationTypePrefFareApplicationType
    {

        OneWay,

        Return,

        HalfReturn
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
    public class FareTypePref
    {

        [XmlAttribute()]
        public string FareType;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PreferLevel;
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
    public class GlobalIndicatorPref
    {

        [XmlAttribute(DataType = "NMTOKEN")]
        public string GlobalIndicatorCode;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PreferLevel;
    }

    [XmlRoot(IsNullable = false)]
    public class NegotiatedFareCodes
    {

        [XmlElement("NegotiatedFareCode")]
        public NegotiatedFareCode[] NegotiatedFareCode;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_AirFareInfoRQ
    {

        public POS POS;

        public TPA_Extensions TPA_Extensions;

        [XmlElement("OriginDestinationInformation")]
        public OriginDestinationInformation[] OriginDestinationInformation;

        public SpecificFlightInfo SpecificFlightInfo;

        public TravelPreferences TravelPreferences;

        [XmlArrayItem(IsNullable = false)]
        public PassengerTypeQuantity[] TravelerInfoSummary;

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
        public string AvailableFlightsOnly;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string DisplayOrder;
    }

    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    

    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;
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
    public class OriginDestinationInformation
    {

        [XmlElement("DepartureDateTime", typeof(DepartureDateTime))]
        [XmlElement("ArrivalDateTime", typeof(ArrivalDateTime))]
        public object Item;

        public OriginLocation OriginLocation;

        public DestinationLocation DestinationLocation;

        [XmlArrayItem(IsNullable = false)]
        public ConnectionLocation[] ConnectionLocations;

        [XmlElement("FlightSegment")]
        public FlightSegment[] FlightSegment;

        [XmlAttribute()]
        public string RPH;
    }

    [XmlRoot(IsNullable = false)]
    public class FlightSegment
    {

        public DepartureAirport DepartureAirport;

        public ArrivalAirport ArrivalAirport;

        public OperatingAirline OperatingAirline;

        [XmlElement("Equipment")]
        public Equipment[] Equipment;

        public MarketingAirlineRQ MarketingAirline;

        public string MarriageGrp;

        [XmlAttribute()]
        public string DepartureDateTime;

        [XmlAttribute()]
        public string ArrivalDateTime;

        [XmlIgnore()]
        public bool ArrivalDateTimeSpecified;

        [XmlAttribute()]
        public int StopQuantity;

        [XmlIgnore()]
        public bool StopQuantitySpecified;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public string InfoSource;

        [XmlAttribute()]
        public string FlightNumber;

        [XmlAttribute()]
        public string TourOperatorFlightID;

        [XmlAttribute()]
        public string ResBookDesigCode;

        [XmlAttribute()]
        public FlightSegmentActionCode ActionCode;

        [XmlIgnore()]
        public bool ActionCodeSpecified;

        [XmlAttribute()]
        public int NumberInParty;

        [XmlIgnore()]
        public bool NumberInPartySpecified;
    }

    public enum FlightSegmentActionCode
    {

        OK,

        Waitlist,

        Other
    }

    [XmlRoot(IsNullable = false)]
    public class DepartureAirport
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlAttribute()]
        public string Terminal;

        [XmlAttribute()]
        public string Gate;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class ArrivalAirport
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlAttribute()]
        public string Terminal;

        [XmlAttribute()]
        public string Gate;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class OperatingAirline
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
        public OperatingAirlineFlightNumber FlightNumber;

        [XmlIgnore()]
        public bool FlightNumberSpecified;

        [XmlText()]
        public string Value;
    }

    public enum OperatingAirlineFlightNumber
    {

        OPEN,

        ARNK
    }

    [XmlRoot(IsNullable = false)]
    public class MarketingAirlineRQ
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
    public class Equipment
    {

        [XmlAttribute()]
        public string AirEquipType;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool ChangeofGauge = false;

        [XmlText()]
        public string Value;
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

        [XmlAttribute()]
        public string BookingReferenceID;
    }

    [XmlRoot(IsNullable = false)]
    public class TravelPreferences
    {

        public ValidatingAirline ValidatingAirline;

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

        public FareApplicationTypePref FareApplicationTypePref;

        [XmlElement("FareTypePref")]
        public FareTypePref[] FareTypePref;

        [XmlElement("FareAccessTypePref")]
        public FareAccessTypePref[] FareAccessTypePref;

        [XmlElement("BookingClassPref")]
        public BookingClassPref[] BookingClassPref;

        public PricingPrefs PricingPrefs;

        [XmlElement("GlobalIndicatorPref")]
        public GlobalIndicatorPref[] GlobalIndicatorPref;

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
    public class PricingPrefs
    {

        [XmlArrayItem(IsNullable = false)]
        public ExchangeRate[] ExchangeRates;

        [XmlAttribute()]
        public string IncludeTaxInd;

        [XmlAttribute()]
        public string IncludeServiceFeesInd;

        [XmlAttribute()]
        public string OverrideCurrency;

        [XmlAttribute()]
        public string AlternateCurrency;
    }

    [XmlRoot(IsNullable = false)]
    public class PassengerTypeQuantity
    {

        public ReductionPref ReductionPref;

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
    public class ReductionPref
    {

        [XmlAttribute()]
        public string Amount;

        [XmlAttribute()]
        public string Currency;

        [XmlAttribute()]
        public string Percent;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PreferLevel;
    }

    [XmlRoot(IsNullable = false)]
    public class TravelerInfoSummary
    {

        [XmlElement("PassengerTypeQuantity")]
        public PassengerTypeQuantity[] PassengerTypeQuantity;
    }
}
