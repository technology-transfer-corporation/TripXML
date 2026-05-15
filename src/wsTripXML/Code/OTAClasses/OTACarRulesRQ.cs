using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCarRulesIn
{

    [XmlType()]
    [XmlRoot(IsNullable = false)]
    public class OTA_VehRateRuleRQ
    {

        public POS_Type POS;

        public UniqueID_Type Reference;

        public OTA_VehRateRuleRQRentalInfo RentalInfo;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_VehRateRuleRQTarget.Production)]
        public OTA_VehRateRuleRQTarget Target = OTA_VehRateRuleRQTarget.Production;

        [XmlAttribute()]
        public decimal Version;

        [XmlAttribute()]
        public string TransactionIdentifier;

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string SequenceNmbr;

        [XmlAttribute()]
        public OTA_VehRateRuleRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public bool RetransmissionIndicator;

        [XmlIgnore()]
        public bool RetransmissionIndicatorSpecified;

        [XmlAttribute()]
        public string CompanyShortName;

        [XmlAttribute()]
        public string TravelSector;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;
    }

    [XmlType()]
    public class POS_Type : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    [XmlType()]
    public class SourceType
    {

        public SourceTypeRequestorID RequestorID;

        public SourceTypePosition Position;

        public SourceTypeBookingChannel BookingChannel;

        [XmlAttribute()]
        public string AgentSine;

        [XmlAttribute()]
        public string PseudoCityCode;

        [XmlAttribute()]
        public string ISOCountry;

        [XmlAttribute()]
        public string ISOCurrency;

        [XmlAttribute()]
        public string AgentDutyCode;

        [XmlAttribute()]
        public string AirlineVendorID;

        [XmlAttribute()]
        public string AirportCode;

        [XmlAttribute()]
        public string FirstDepartPoint;

        [XmlAttribute()]
        public string ERSP_UserID;

        [XmlAttribute()]
        public string TerminalID;
    }

    [XmlType()]
    public class SourceTypeRequestorID : UniqueID_Type
    {

        [XmlAttribute()]
        public string MessagePassword;
    }

    [XmlType()]
    [XmlInclude(typeof(SourceTypeRequestorID))]
    public class UniqueID_Type
    {

        public CompanyNameType CompanyName;

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

    [XmlType()]
    public class CompanyNameType
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
        public string Division;

        [XmlAttribute()]
        public string Department;

        [XmlText()]
        public string Value;
    }

    [XmlType()]
    public class VehicleTourInfoType
    {

        public CompanyNameType TourOperator;

        [XmlAttribute()]
        public string TourNumber;
    }

    [XmlType()]
    public class VehicleArrivalDetailsType
    {

        public LocationType ArrivalLocation;

        public CompanyNameType MarketingCompany;

        public CompanyNameType OperatingCompany;

        [XmlAttribute()]
        public string TransportationCode;

        [XmlAttribute()]
        public string Number;

        [XmlAttribute()]
        public DateTime ArrivalDateTime;

        [XmlIgnore()]
        public bool ArrivalDateTimeSpecified;
    }

    [XmlType()]
    [XmlInclude(typeof(VehicleRentalCoreTypeReturnLocation))]
    [XmlInclude(typeof(VehicleRentalCoreTypePickUpLocation))]
    public class LocationType
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlText()]
        public string Value;
    }

    [XmlType()]
    public class VehicleRentalCoreTypeReturnLocation : LocationType
    {

        [XmlAttribute()]
        public string ExtendedLocationCode;

        [XmlAttribute()]
        public string CounterLocation;
    }

    [XmlType()]
    public class VehicleRentalCoreTypePickUpLocation : LocationType
    {

        [XmlAttribute()]
        public string ExtendedLocationCode;

        [XmlAttribute()]
        public string CounterLocation;
    }

    [XmlType()]
    public class OffLocationServiceTypeTelephone
    {
    }

    [XmlType()]
    public class PersonNameType
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
        public AddressTypeShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressTypeShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string NameType;
    }

    [XmlType()]
    public enum AddressTypeShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlType()]
    public enum AddressTypeShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlType()]
    public class CountryNameType
    {

        [XmlAttribute()]
        public string Code;

        [XmlText()]
        public string Value;
    }

    [XmlType()]
    public class StateProvType
    {

        [XmlAttribute()]
        public string StateCode;

        [XmlText()]
        public string Value;
    }

    [XmlType()]
    public class AddressTypeBldgRoom
    {

        [XmlAttribute()]
        public bool BldgNameIndicator;

        [XmlIgnore()]
        public bool BldgNameIndicatorSpecified;

        [XmlText()]
        public string Value;
    }

    [XmlType()]
    [XmlInclude(typeof(AddressTypeStreetNmbr))]
    public class StreetNmbrType
    {

        [XmlAttribute()]
        public string PO_Box;

        [XmlText()]
        public string Value;
    }

    [XmlType()]
    public class AddressTypeStreetNmbr : StreetNmbrType
    {

        [XmlAttribute()]
        public string StreetNmbrSuffix;

        [XmlAttribute()]
        public string StreetDirection;

        [XmlAttribute()]
        public string RuralRouteNmbr;
    }

    [XmlType()]
    [XmlInclude(typeof(OffLocationServiceCoreTypeAddress))]
    public class AddressType
    {

        public AddressTypeStreetNmbr StreetNmbr;

        [XmlElement("BldgRoom")]
        public AddressTypeBldgRoom[] BldgRoom;

        [XmlElement("AddressLine")]
        public string[] AddressLine;

        public string CityName;

        public string PostalCode;

        public string County;

        public StateProvType StateProv;

        public CountryNameType CountryName;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        [XmlAttribute()]
        public AddressTypeShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressTypeShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string Type;
    }

    [XmlType()]
    public class OffLocationServiceCoreTypeAddress : AddressType
    {

        [XmlAttribute()]
        public string SiteID;

        [XmlAttribute()]
        public string SiteName;
    }

    [XmlType()]
    [XmlInclude(typeof(OffLocationServiceType))]
    public class OffLocationServiceCoreType
    {

        public OffLocationServiceCoreTypeAddress Address;

        [XmlAttribute()]
        public OffLocationServiceID_Type Type;
    }

    [XmlType()]
    public enum OffLocationServiceID_Type
    {

        CustPickUp,

        VehDelivery,

        CustDropOff,

        VehCollection,

        Exchange
    }

    [XmlType()]
    public class OffLocationServiceType : OffLocationServiceCoreType
    {

        public PersonNameType PersonName;

        public OffLocationServiceTypeTelephone Telephone;

        [XmlAttribute()]
        public string SpecInstructions;
    }

    [XmlType()]
    public class OTA_VehRateRuleRQRentalInfoRateQualifier
    {

        [XmlAttribute()]
        public string TravelPurpose;

        [XmlAttribute()]
        public string RateCategory;

        [XmlAttribute()]
        public string CorpDiscountNmbr;

        [XmlAttribute()]
        public string RateQualifier;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string RatePeriod;

        [XmlAttribute()]
        public bool GuaranteedInd;

        [XmlIgnore()]
        public bool GuaranteedIndSpecified;

        [XmlAttribute()]
        public string RateAuthorizationCode;

        [XmlAttribute()]
        public string VendorRateID;

        [XmlAttribute()]
        public bool RateModifiedInd;

        [XmlIgnore()]
        public bool RateModifiedIndSpecified;
    }

    [XmlType()]
    public class OTA_VehRateRuleRQRentalInfoCustLoyalty
    {

        [XmlAttribute()]
        public string ProgramID;

        [XmlAttribute()]
        public string MembershipID;

        [XmlAttribute()]
        public string TravelSector;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public string[] VendorCode;
    }

    [XmlType()]
    public class OTA_VehRateRuleRQRentalInfoSpecialEquipPref
    {

        [XmlAttribute()]
        public ActionType Action;

        [XmlIgnore()]
        public bool ActionSpecified;
    }

    [XmlType()]
    public enum ActionType
    {

        [XmlEnum("Add-Update")]
        AddUpdate,

        Cancel,

        Delete,

        Add,

        Replace
    }

    [XmlType()]
    public class VehicleCoreTypeVehClass
    {

        [XmlAttribute()]
        public string Size;
    }

    [XmlType()]
    public class VehicleCoreTypeVehType
    {

        [XmlAttribute()]
        public string VehicleCategory;

        [XmlAttribute()]
        public string DoorCount;
    }

    [XmlType()]
    [XmlInclude(typeof(VehiclePrefType))]
    public class VehicleCoreType
    {

        public VehicleCoreTypeVehType VehType;

        public VehicleCoreTypeVehClass VehClass;

        [XmlAttribute()]
        public bool AirConditionInd;

        [XmlIgnore()]
        public bool AirConditionIndSpecified;

        [XmlAttribute()]
        public VehicleTransmissionType TransmissionType;

        [XmlIgnore()]
        public bool TransmissionTypeSpecified;

        [XmlAttribute()]
        public VehicleCoreTypeFuelType FuelType;

        [XmlIgnore()]
        public bool FuelTypeSpecified;

        [XmlAttribute()]
        public VehicleCoreTypeDriveType DriveType;

        [XmlIgnore()]
        public bool DriveTypeSpecified;
    }

    [XmlType()]
    public enum VehicleTransmissionType
    {

        Automatic,

        Manual
    }

    [XmlType()]
    public enum VehicleCoreTypeFuelType
    {

        Unspecified,

        Diesel,

        Hybrid,

        Electric,

        LPG_CompressedGas,

        Hydrogen,

        MultiFuel,

        Petrol,

        Ethanol
    }

    [XmlType()]
    public enum VehicleCoreTypeDriveType
    {

        AWD,

        [XmlEnum("4WD")]
        Item4WD,

        Unspecified
    }

    [XmlType()]
    public class VehiclePrefType : VehicleCoreType
    {

        [XmlAttribute()]
        public PreferLevelType TypePref;

        [XmlIgnore()]
        public bool TypePrefSpecified;

        [XmlAttribute()]
        public PreferLevelType ClassPref;

        [XmlIgnore()]
        public bool ClassPrefSpecified;

        [XmlAttribute()]
        public PreferLevelType AirConditionPref;

        [XmlIgnore()]
        public bool AirConditionPrefSpecified;

        [XmlAttribute()]
        public PreferLevelType TransmissionPref;

        [XmlIgnore()]
        public bool TransmissionPrefSpecified;

        [XmlAttribute()]
        public string VendorCarType;

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string VehicleQty;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;
    }

    [XmlType()]
    public enum PreferLevelType
    {

        Only,

        Unacceptable,

        Preferred,

        Required,

        NoPreference
    }

    [XmlType()]
    public class VehicleRentalCoreType
    {

        [XmlElement("PickUpLocation")]
        public VehicleRentalCoreTypePickUpLocation[] PickUpLocation;

        public VehicleRentalCoreTypeReturnLocation ReturnLocation;

        [XmlAttribute()]
        public DateTime PickUpDateTime;

        [XmlIgnore()]
        public bool PickUpDateTimeSpecified;

        [XmlAttribute()]
        public DateTime ReturnDateTime;

        [XmlIgnore()]
        public bool ReturnDateTimeSpecified;

        [XmlAttribute()]
        public DateTime StartChargesDateTime;

        [XmlIgnore()]
        public bool StartChargesDateTimeSpecified;

        [XmlAttribute()]
        public DateTime StopChargesDateTime;

        [XmlIgnore()]
        public bool StopChargesDateTimeSpecified;

        [XmlAttribute()]
        public bool OneWayIndicator;

        [XmlIgnore()]
        public bool OneWayIndicatorSpecified;

        [XmlAttribute(DataType = "integer")]
        public string MultiIslandRentalDays;

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string Quantity;

        [XmlAttribute()]
        public DistanceUnitNameType DistUnitName;

        [XmlIgnore()]
        public bool DistUnitNameSpecified;
    }

    [XmlType()]
    public enum DistanceUnitNameType
    {

        Mile,

        Km,

        Block
    }

    [XmlType()]
    public class OTA_VehRateRuleRQRentalInfo
    {

        public VehicleRentalCoreType VehRentalCore;

        public VehiclePrefType VehicleInfo;

        [XmlArrayItem("SpecialEquipPref", IsNullable = false)]
        public OTA_VehRateRuleRQRentalInfoSpecialEquipPref[] SpecialEquipPrefs;

        [XmlElement("CustLoyalty")]
        public OTA_VehRateRuleRQRentalInfoCustLoyalty[] CustLoyalty;

        public OTA_VehRateRuleRQRentalInfoRateQualifier RateQualifier;

        public OffLocationServiceType OffLocService;

        public VehicleArrivalDetailsType ArrivalDetails;

        public VehicleTourInfoType TourInfo;

        public UniqueID_Type CustomerID;

        public TPA_Extensions TPA_Extensions;
    }

    [XmlType()]
    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;
    }

    [XmlType()]
    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        public Name Name;

        public string System;

        public string Userid;

        public string Password;
    }

    [XmlType()]
    [XmlRoot(IsNullable = false)]
    public class Name
    {

        [XmlAttribute()]
        public string PseudoCityCode;

        [XmlText()]
        public string Value;
    }

    [XmlType()]
    public class SourceTypeBookingChannel
    {

        public CompanyNameType CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public bool Primary;

        [XmlIgnore()]
        public bool PrimarySpecified;
    }

    [XmlType()]
    public class SourceTypePosition
    {

        [XmlAttribute()]
        public string Latitude;

        [XmlAttribute()]
        public string Longitude;

        [XmlAttribute()]
        public string Altitude;

        [XmlAttribute()]
        public string AltitudeUnitOfMeasureCode;
    }

    [XmlType()]
    public enum OTA_VehRateRuleRQTarget
    {

        Test,

        Production
    }

    [XmlType()]
    public enum OTA_VehRateRuleRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries,

        Continuation,

        Subsequent
    }
}