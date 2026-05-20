using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCarListIn
{

    [XmlRoot(IsNullable = false)]
    public partial class OTA_VehLocSearchRQ
    {

        public POS POS;

        [XmlElement("VehLocSearchCriterion")]
        public ItemSearchCriterionType[] VehLocSearchCriterion;

        public CompanyNameType Vendor;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        public OTA_VehLocSearchRQTarget Target;

        [XmlIgnore()]
        public bool TargetSpecified;

        [XmlAttribute()]
        public string TargetName;

        [XmlAttribute()]
        public decimal Version;

        [XmlIgnore()]
        public bool VersionSpecified;

        [XmlAttribute()]
        public string TransactionIdentifier;

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string SequenceNmbr;

        [XmlAttribute()]
        public OTA_VehLocSearchRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public bool RetransmissionIndicator;

        [XmlIgnore()]
        public bool RetransmissionIndicatorSpecified;

        [XmlAttribute()]
        public string CorrelationID;

        [XmlAttribute(DataType = "positiveInteger")]
        public string MaxResponses;

        [XmlAttribute()]
        public string ReqRespVersion;

        [XmlAttribute()]
        public bool JustAddressPhone;

        [XmlIgnore()]
        public bool JustAddressPhoneSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    [XmlType()]
    public partial class UniqueID_Type
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
    public partial class CompanyNameType
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
    public partial class WrittenConfInstType
    {

        public ParagraphType SupplementalData;

        public EmailType Email;

        [XmlAttribute()]
        public string LanguageID;

        [XmlAttribute()]
        public string AddresseeName;

        [XmlAttribute()]
        public string Address;

        [XmlAttribute()]
        public string Telephone;

        [XmlAttribute()]
        public bool ConfirmInd;

        [XmlIgnore()]
        public bool ConfirmIndSpecified;
    }





    [XmlType()]
    public partial class ParagraphType
    {

        [XmlElement("Image", typeof(string))]
        [XmlElement("ListItem", typeof(ParagraphTypeListItem))]
        [XmlElement("Text", typeof(FormattedTextTextType))]
        [XmlElement("URL", typeof(string), DataType = "anyURI")]
        [XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items;

        [XmlElement("ItemsElementName")]
        [XmlIgnore()]
        public ItemsChoiceType[] ItemsElementName;

        [XmlAttribute()]
        public string Name;

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string ParagraphNumber;

        [XmlAttribute()]
        public DateTime CreateDateTime;

        [XmlIgnore()]
        public bool CreateDateTimeSpecified;

        [XmlAttribute()]
        public string CreatorID;

        [XmlAttribute()]
        public DateTime LastModifyDateTime;

        [XmlIgnore()]
        public bool LastModifyDateTimeSpecified;

        [XmlAttribute()]
        public string LastModifierID;

        [XmlAttribute(DataType = "date")]
        public DateTime PurgeDate;

        [XmlIgnore()]
        public bool PurgeDateSpecified;

        [XmlAttribute(DataType = "language")]
        public string Language;
    }





    [XmlType()]
    public partial class ParagraphTypeListItem : FormattedTextTextType
    {

        [XmlAttribute(DataType = "integer")]
        public string ListItem;
    }

    [XmlType()]
    public partial class FormattedTextTextType
    {

        [XmlAttribute()]
        public bool Formatted;

        [XmlIgnore()]
        public bool FormattedSpecified;

        [XmlAttribute(DataType = "language")]
        public string Language;

        [XmlAttribute()]
        public FormattedTextTextTypeTextFormat TextFormat;

        [XmlIgnore()]
        public bool TextFormatSpecified;

        [XmlText()]
        public string Value;
    }



    [XmlType()]
    public enum FormattedTextTextTypeTextFormat
    {

        PlainText,

        HTML
    }





    [XmlType()]
    public partial class CoverageDetailsType : FormattedTextTextType
    {

        [XmlAttribute()]
        public CoverageTextType CoverageTextType;
    }



    [XmlType()]
    public enum CoverageTextType
    {

        Supplement,

        Description,

        Limits
    }



    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        Image,

        ListItem,

        Text,

        URL
    }





    [XmlType()]
    public partial class EmailType
    {

        [XmlAttribute()]
        public AddressTypeShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressTypeShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public bool DefaultInd;

        [XmlIgnore()]
        public bool DefaultIndSpecified;

        [XmlAttribute("EmailType")]
        public string EmailType1;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public string Remark;

        [XmlText()]
        public string Value;
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
    public partial class VehicleArrivalDetailsType
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
    public partial class LocationType
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        public string CodeContext;

        [XmlText()]
        public string Value;
    }





    [XmlType()]
    public partial class VehicleSpecialReqPrefType
    {

        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        [XmlIgnore()]
        public bool PreferLevelSpecified;

        [XmlText()]
        public string Value;
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
    public partial class VehicleTourInfoType
    {

        public CompanyNameType TourOperator;

        [XmlAttribute()]
        public string TourNumber;
    }





    [XmlType()]
    public partial class TaxType
    {

        [XmlElement("TaxDescription")]
        public ParagraphType[] TaxDescription;

        [XmlAttribute()]
        public AmountDeterminationType Type;

        [XmlIgnore()]
        public bool TypeSpecified;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public decimal Percent;

        [XmlIgnore()]
        public bool PercentSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;

        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator;

        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;

        [XmlAttribute()]
        public string ChargeUnit;

        [XmlAttribute()]
        public string ChargeFrequency;

        [XmlAttribute(DataType = "positiveInteger")]
        public string ChargeUnitExempt;

        [XmlAttribute(DataType = "positiveInteger")]
        public string ChargeFrequencyExempt;

        [XmlAttribute(DataType = "positiveInteger")]
        public string MaxChargeUnitApplies;

        [XmlAttribute(DataType = "positiveInteger")]
        public string MaxChargeFrequencyApplies;
    }



    [XmlType()]
    public enum AmountDeterminationType
    {

        Inclusive,

        Exclusive,

        Cumulative
    }





    [XmlType()]
    public partial class TaxesType
    {

        [XmlElement("Tax")]
        public TaxType[] Tax;

        [XmlAttribute()]
        public decimal Amount;

        [XmlIgnore()]
        public bool AmountSpecified;
    }





    [XmlType()]
    public partial class FeeType
    {

        public TaxesType Taxes;

        [XmlElement("Description")]
        public ParagraphType[] Description;

        [XmlAttribute()]
        public bool TaxInclusive;

        [XmlIgnore()]
        public bool TaxInclusiveSpecified;

        [XmlAttribute()]
        public AmountDeterminationType Type;

        [XmlIgnore()]
        public bool TypeSpecified;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public decimal Percent;

        [XmlIgnore()]
        public bool PercentSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;

        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator;

        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;

        [XmlAttribute()]
        public bool MandatoryIndicator;

        [XmlIgnore()]
        public bool MandatoryIndicatorSpecified;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public string ChargeUnit;

        [XmlAttribute()]
        public string ChargeFrequency;

        [XmlAttribute(DataType = "positiveInteger")]
        public string ChargeUnitExempt;

        [XmlAttribute(DataType = "positiveInteger")]
        public string ChargeFrequencyExempt;

        [XmlAttribute(DataType = "positiveInteger")]
        public string MaxChargeUnitApplies;

        [XmlAttribute(DataType = "positiveInteger")]
        public string MaxChargeFrequencyApplies;

        [XmlAttribute()]
        public bool TaxableIndicator;

        [XmlIgnore()]
        public bool TaxableIndicatorSpecified;
    }

    [XmlType()]
    public partial class OperationScheduleType
    {

        [XmlArrayItem("OperationTime", IsNullable = false)]
        public OperationScheduleTypeOperationTime[] OperationTimes;

        [XmlAttribute()]
        public string Start;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute()]
        public string End;
    }





    [XmlType()]
    public partial class OperationScheduleTypeOperationTime
    {

        [XmlAttribute()]
        public bool Mon;

        [XmlIgnore()]
        public bool MonSpecified;

        [XmlAttribute()]
        public bool Tue;

        [XmlIgnore()]
        public bool TueSpecified;

        [XmlAttribute()]
        public bool Weds;

        [XmlIgnore()]
        public bool WedsSpecified;

        [XmlAttribute()]
        public bool Thur;

        [XmlIgnore()]
        public bool ThurSpecified;

        [XmlAttribute()]
        public bool Fri;

        [XmlIgnore()]
        public bool FriSpecified;

        [XmlAttribute()]
        public bool Sat;

        [XmlIgnore()]
        public bool SatSpecified;

        [XmlAttribute()]
        public bool Sun;

        [XmlIgnore()]
        public bool SunSpecified;

        [XmlAttribute()]
        public string Start;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute()]
        public string End;

        [XmlAttribute()]
        public string AdditionalOperationInfoCode;

        [XmlAttribute()]
        public string Frequency;

        [XmlAttribute()]
        public string Text;
    }





    [XmlType()]
    public partial class OperationSchedulePlusChargeType : OperationScheduleType
    {

        [XmlElement("Charge")]
        public FeeType[] Charge;
    }





    [XmlType()]
    public partial class OperationSchedulesType
    {

        [XmlElement("OperationSchedule")]
        public OperationScheduleType[] OperationSchedule;

        [XmlAttribute()]
        public string Start;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute()]
        public string End;
    }





    [XmlType()]
    public partial class VehicleWhereAtFacilityType
    {

        [XmlAttribute()]
        public string Location;

        [XmlText()]
        public string Value;
    }





    [XmlType()]
    public partial class VehicleLocationAdditionalDetailsType
    {

        [XmlArrayItem("VehRentLocInfo", IsNullable = false)]
        public VehicleLocationInformationType[] VehRentLocInfos;

        public VehicleWhereAtFacilityType ParkLocation;

        public VehicleWhereAtFacilityType CounterLocation;

        public OperationSchedulesType OperationSchedules;

        public VehicleLocationAdditionalDetailsTypeShuttle Shuttle;

        [XmlArrayItem("OneWayDropLocation", IsNullable = false)]
        public VehicleLocationAdditionalDetailsTypeOneWayDropLocation[] OneWayDropLocations;

        public TPA_Extensions TPA_Extensions;
    }





    [XmlType()]
    public partial class VehicleLocationInformationType : FormattedTextType
    {

        [XmlAttribute()]
        public string Type;
    }

    [XmlType()]
    public partial class FormattedTextType
    {

        [XmlElement("SubSection")]
        public FormattedTextSubSectionType[] SubSection;

        [XmlAttribute()]
        public string Title;

        [XmlAttribute(DataType = "language")]
        public string Language;
    }





    [XmlType()]
    public partial class FormattedTextSubSectionType
    {

        [XmlElement("Paragraph")]
        public ParagraphType[] Paragraph;

        [XmlAttribute()]
        public string SubTitle;

        [XmlAttribute()]
        public string SubCode;

        [XmlAttribute(DataType = "integer")]
        public string SubSectionNumber;
    }





    [XmlType()]
    public partial class VendorMessageType : FormattedTextType
    {

        [XmlAttribute()]
        public string InfoType;
    }





    [XmlType()]
    public partial class VehicleLocationAdditionalDetailsTypeShuttle
    {

        [XmlArrayItem("ShuttleInfo", IsNullable = false)]
        public VehicleLocationAdditionalDetailsTypeShuttleShuttleInfo[] ShuttleInfos;

        public OperationSchedulesType OperationSchedules;
    }





    [XmlType()]
    public partial class VehicleLocationAdditionalDetailsTypeShuttleShuttleInfo : FormattedTextType
    {

        [XmlAttribute()]
        public LocationDetailShuttleInfoType Type;
    }



    [XmlType()]
    public enum LocationDetailShuttleInfoType
    {

        Transportation,

        Frequency,

        PickupInfo,

        Distance,

        ElapsedTime,

        Fee,

        Miscellaneous,

        Hours
    }





    [XmlType()]
    public partial class VehicleLocationAdditionalDetailsTypeOneWayDropLocation : LocationType
    {

        [XmlAttribute()]
        public string ExtendedLocationCode;
    }

    [XmlRoot("TPA_Extensions", IsNullable = false)]
    public partial class TPA_Extensions
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

    [XmlType()]
    public partial class VehicleLocationDetailsType
    {

        [XmlElement("Address")]
        public AddressInfoType[] Address;

        [XmlElement("Telephone")]
        public VehicleLocationDetailsTypeTelephone[] Telephone;

        public VehicleLocationAdditionalDetailsType AdditionalInfo;

        [XmlAttribute()]
        public bool AtAirport;

        [XmlIgnore()]
        public bool AtAirportSpecified;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string Name;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute()]
        public string ExtendedLocationCode;

        [XmlAttribute()]
        public string[] AssocAirportLocList;
    }





    [XmlType()]
    public partial class AddressInfoType : AddressType
    {

        [XmlAttribute()]
        public bool DefaultInd;

        [XmlIgnore()]
        public bool DefaultIndSpecified;

        [XmlAttribute()]
        public string UseType;

        [XmlAttribute()]
        public string RPH;
    }

    [XmlType()]
    public partial class AddressType
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
        public bool FormattedInd;

        [XmlIgnore()]
        public bool FormattedIndSpecified;

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

        [XmlAttribute()]
        public string Remark;
    }





    [XmlType()]
    public partial class AddressTypeStreetNmbr : StreetNmbrType
    {

        [XmlAttribute()]
        public string StreetNmbrSuffix;

        [XmlAttribute()]
        public string StreetDirection;

        [XmlAttribute()]
        public string RuralRouteNmbr;
    }





    [XmlType()]
    public partial class StreetNmbrType
    {

        [XmlAttribute()]
        public string PO_Box;

        [XmlText()]
        public string Value;
    }





    [XmlType()]
    public partial class AddressTypeBldgRoom
    {

        [XmlAttribute()]
        public bool BldgNameIndicator;

        [XmlIgnore()]
        public bool BldgNameIndicatorSpecified;

        [XmlText()]
        public string Value;
    }





    [XmlType()]
    public partial class StateProvType
    {

        [XmlAttribute()]
        public string StateCode;

        [XmlText()]
        public string Value;
    }





    [XmlType()]
    public partial class CountryNameType
    {

        [XmlAttribute()]
        public string Code;

        [XmlText()]
        public string Value;
    }





    [XmlType()]
    public partial class VehicleLocationDetailsTypeTelephone
    {

        [XmlAttribute()]
        public string RPH;
    }

    [XmlType()]
    public partial class OffLocationServiceCoreType
    {

        public OffLocationServiceCoreTypeAddress Address;

        [XmlAttribute()]
        public OffLocationServiceID_Type Type;
    }





    [XmlType()]
    public partial class OffLocationServiceCoreTypeAddress : AddressType
    {

        [XmlAttribute()]
        public string SiteID;

        [XmlAttribute()]
        public string SiteName;
    }



    [XmlType()]
    public enum OffLocationServiceID_Type
    {

        CustPickUp,

        VehDelivery,

        CustDropOff,

        VehCollection,

        Exchange,

        RepairLocation
    }





    [XmlType()]
    public partial class OffLocationServiceType : OffLocationServiceCoreType
    {

        public PersonNameType PersonName;

        public OffLocationServiceTypeTelephone Telephone;

        public UniqueID_Type TrackingID;

        [XmlAttribute()]
        public string SpecInstructions;
    }





    [XmlType()]
    public partial class PersonNameType
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

        public PersonNameTypeDocument Document;

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
    public partial class PersonNameTypeDocument
    {

        [XmlAttribute()]
        public string DocID;

        [XmlAttribute()]
        public string DocType;
    }





    [XmlType()]
    public partial class OffLocationServiceTypeTelephone
    {
    }





    [XmlType()]
    public partial class OffLocationServicePricedType
    {

        public OffLocationServiceType OffLocService;

        public VehicleChargeType Charge;
    }

    [XmlType()]
    public partial class VehicleChargeType
    {

        [XmlArrayItem("TaxAmount", IsNullable = false)]
        public VehicleChargeTypeTaxAmount[] TaxAmounts;

        public VehicleChargeTypeMinMax MinMax;

        [XmlElement("Calculation")]
        public VehicleChargeTypeCalculation[] Calculation;

        [XmlAttribute()]
        public decimal Amount;

        [XmlIgnore()]
        public bool AmountSpecified;

        [XmlAttribute()]
        public bool TaxInclusive;

        [XmlIgnore()]
        public bool TaxInclusiveSpecified;

        [XmlAttribute()]
        public string Description;

        [XmlAttribute()]
        public bool GuaranteedInd;

        [XmlIgnore()]
        public bool GuaranteedIndSpecified;

        [XmlAttribute()]
        public bool IncludedInRate;

        [XmlIgnore()]
        public bool IncludedInRateSpecified;

        [XmlAttribute()]
        public bool IncludedInEstTotalInd;

        [XmlIgnore()]
        public bool IncludedInEstTotalIndSpecified;

        [XmlAttribute()]
        public bool RateConvertInd;

        [XmlIgnore()]
        public bool RateConvertIndSpecified;
    }





    [XmlType()]
    public partial class VehicleChargeTypeTaxAmount
    {

        [XmlAttribute()]
        public decimal Total;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public string TaxCode;

        [XmlAttribute()]
        public decimal Percentage;

        [XmlIgnore()]
        public bool PercentageSpecified;

        [XmlAttribute()]
        public string Description;
    }





    [XmlType()]
    public partial class VehicleChargeTypeMinMax
    {

        [XmlAttribute()]
        public decimal MaxCharge;

        [XmlIgnore()]
        public bool MaxChargeSpecified;

        [XmlAttribute()]
        public decimal MinCharge;

        [XmlIgnore()]
        public bool MinChargeSpecified;

        [XmlAttribute(DataType = "integer")]
        public string MaxChargeDays;
    }





    [XmlType()]
    public partial class VehicleChargeTypeCalculation
    {

        [XmlAttribute()]
        public decimal UnitCharge;

        [XmlIgnore()]
        public bool UnitChargeSpecified;

        [XmlAttribute()]
        public string UnitName;

        [XmlAttribute(DataType = "integer")]
        public string Quantity;

        [XmlAttribute()]
        public decimal Percentage;

        [XmlIgnore()]
        public bool PercentageSpecified;

        [XmlAttribute()]
        public VehicleChargeTypeCalculationApplicability Applicability;

        [XmlIgnore()]
        public bool ApplicabilitySpecified;

        [XmlAttribute(DataType = "integer")]
        public string MaxQuantity;

        [XmlAttribute()]
        public decimal Total;

        [XmlIgnore()]
        public bool TotalSpecified;
    }



    [XmlType()]
    public enum VehicleChargeTypeCalculationApplicability
    {

        FromPickupLocation,

        FromDropoffLocation,

        BeforePickup,

        AfterDropoff
    }





    [XmlType()]
    public partial class VehicleChargePurposeType : VehicleChargeType
    {

        [XmlAttribute()]
        public string Purpose;

        [XmlAttribute()]
        public bool RequiredInd;

        [XmlIgnore()]
        public bool RequiredIndSpecified;
    }





    [XmlType()]
    public partial class DeductibleType
    {

        [XmlAttribute()]
        public decimal Amount;

        [XmlIgnore()]
        public bool AmountSpecified;

        [XmlAttribute()]
        public decimal LiabilityAmount;

        [XmlIgnore()]
        public bool LiabilityAmountSpecified;

        [XmlAttribute()]
        public decimal ExcessAmount;

        [XmlIgnore()]
        public bool ExcessAmountSpecified;
    }





    [XmlType()]
    public partial class CoverageType
    {

        [XmlElement("Details")]
        public CoverageDetailsType[] Details;

        [XmlAttribute("CoverageType")]
        public string CoverageType1;

        [XmlAttribute()]
        public string Code;
    }





    [XmlType()]
    public partial class CoveragePricedType
    {

        public CoverageType Coverage;

        public VehicleChargeType Charge;

        public DeductibleType Deductible;

        [XmlAttribute()]
        public bool Required;

        [XmlIgnore()]
        public bool RequiredSpecified;
    }





    [XmlType()]
    public partial class MonetaryRuleType
    {

        [XmlAttribute()]
        public decimal Amount;

        [XmlIgnore()]
        public bool AmountSpecified;

        [XmlAttribute()]
        public string RuleType;

        [XmlAttribute()]
        public decimal Percent;

        [XmlIgnore()]
        public bool PercentSpecified;

        [XmlAttribute()]
        public DateTime DateTime;

        [XmlIgnore()]
        public bool DateTimeSpecified;

        [XmlAttribute()]
        public string PaymentType;

        [XmlAttribute()]
        public bool RateConvertedInd;

        [XmlIgnore()]
        public bool RateConvertedIndSpecified;

        [XmlAttribute()]
        public string AbsoluteDeadline;

        [XmlAttribute()]
        public TimeUnitType OffsetTimeUnit;

        [XmlIgnore()]
        public bool OffsetTimeUnitSpecified;

        [XmlAttribute(DataType = "integer")]
        public string OffsetUnitMultiplier;

        [XmlAttribute()]
        public VehicleRentalRateTypeRateGuaranteeOffsetDropTime OffsetDropTime;

        [XmlIgnore()]
        public bool OffsetDropTimeSpecified;

        [XmlText()]
        public string Value;
    }



    [XmlType()]
    public enum TimeUnitType
    {

        Year,

        Month,

        Week,

        Day,

        Hour,

        Second,

        FullDuration,

        Minute
    }



    [XmlType()]
    public enum VehicleRentalRateTypeRateGuaranteeOffsetDropTime
    {

        BeforeArrival,

        AfterBooking,

        AfterConfirmation,

        AfterArrival
    }





    [XmlType()]
    public partial class VehicleSegmentAdditionalInfoType
    {

        [XmlArrayItem("PaymentRule", IsNullable = false)]
        public MonetaryRuleType[] PaymentRules;

        [XmlElement("RentalPaymentAmount")]
        public PaymentDetailType[] RentalPaymentAmount;

        [XmlArrayItem("PricedCoverage", IsNullable = false)]
        public CoveragePricedType[] PricedCoverages;

        [XmlElement("PricedOffLocService")]
        public OffLocationServicePricedType[] PricedOffLocService;

        [XmlArrayItem("VendorMessage", IsNullable = false)]
        public FormattedTextType[] VendorMessages;

        [XmlElement("LocationDetails")]
        public VehicleLocationDetailsType[] LocationDetails;

        public VehicleTourInfoType TourInfo;

        [XmlElement("SpecialReqPref")]
        public VehicleSpecialReqPrefType[] SpecialReqPref;

        public VehicleArrivalDetailsType ArrivalDetails;

        public WrittenConfInstType WrittenConfInst;

        [XmlElement("Remark")]
        public ParagraphType[] Remark;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public bool WrittenConfInd;

        [XmlIgnore()]
        public bool WrittenConfIndSpecified;
    }





    [XmlType()]
    public partial class PaymentDetailType : PaymentFormType
    {

        [XmlElement("PaymentAmount")]
        public PaymentDetailTypePaymentAmount[] PaymentAmount;

        public CommissionType Commission;

        [XmlAttribute()]
        public string PaymentType;

        [XmlAttribute()]
        public bool SplitPaymentInd;

        [XmlIgnore()]
        public bool SplitPaymentIndSpecified;

        [XmlAttribute(DataType = "integer")]
        public string AuthorizedDays;

        [XmlAttribute()]
        public bool PrimaryPaymentInd;

        [XmlIgnore()]
        public bool PrimaryPaymentIndSpecified;
    }





    [XmlType()]
    public partial class PaymentDetailTypePaymentAmount
    {

        [XmlAttribute()]
        public decimal Amount;

        [XmlIgnore()]
        public bool AmountSpecified;

        [XmlAttribute()]
        public string ApprovalCode;

        [XmlAttribute()]
        public PaymentDetailTypePaymentAmountRefundCalcMethod RefundCalcMethod;

        [XmlIgnore()]
        public bool RefundCalcMethodSpecified;
    }



    [XmlType()]
    public enum PaymentDetailTypePaymentAmountRefundCalcMethod
    {

        System,

        Manual
    }





    [XmlType()]
    public partial class CommissionType
    {

        public UniqueID_Type UniqueID;

        public CommissionTypeCommissionableAmount CommissionableAmount;

        public CommissionTypePrepaidAmount PrepaidAmount;

        public CommissionTypeFlatCommission FlatCommission;

        public CommissionTypeCommissionPayableAmount CommissionPayableAmount;

        public ParagraphType Comment;

        [XmlAttribute()]
        public CommissionTypeStatusType StatusType;

        [XmlIgnore()]
        public bool StatusTypeSpecified;

        [XmlAttribute()]
        public decimal Percent;

        [XmlIgnore()]
        public bool PercentSpecified;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string DecimalPlaces;

        [XmlAttribute()]
        public string ReasonCode;

        [XmlAttribute()]
        public string BillToID;

        [XmlAttribute()]
        public string Frequency;

        [XmlAttribute(DataType = "positiveInteger")]
        public string MaxCommissionUnitApplies;

        [XmlAttribute()]
        public decimal CapAmount;

        [XmlIgnore()]
        public bool CapAmountSpecified;
    }





    [XmlType()]
    public partial class CommissionTypeCommissionableAmount
    {

        [XmlAttribute()]
        public decimal Amount;

        [XmlIgnore()]
        public bool AmountSpecified;

        [XmlAttribute()]
        public bool TaxInclusiveIndicator;

        [XmlIgnore()]
        public bool TaxInclusiveIndicatorSpecified;
    }





    [XmlType()]
    public partial class CommissionTypePrepaidAmount
    {

        [XmlAttribute()]
        public decimal Amount;

        [XmlIgnore()]
        public bool AmountSpecified;
    }





    [XmlType()]
    public partial class CommissionTypeFlatCommission
    {

        [XmlAttribute()]
        public decimal Amount;

        [XmlIgnore()]
        public bool AmountSpecified;
    }





    [XmlType()]
    public partial class CommissionTypeCommissionPayableAmount
    {

        [XmlAttribute()]
        public decimal Amount;

        [XmlIgnore()]
        public bool AmountSpecified;
    }



    [XmlType()]
    public enum CommissionTypeStatusType
    {

        Full,

        Partial,

        [XmlEnum("Non-paying")]
        Nonpaying,

        [XmlEnum("No-show")]
        Noshow,

        Adjustment,

        Commissionable
    }

    [XmlType()]
    public partial class PaymentFormType
    {

        [XmlElement("BankAcct", typeof(BankAcctType))]
        [XmlElement("Cash", typeof(PaymentFormTypeCash))]
        [XmlElement("DirectBill", typeof(DirectBillType))]
        [XmlElement("LoyaltyRedemption", typeof(PaymentFormTypeLoyaltyRedemption))]
        [XmlElement("MiscChargeOrder", typeof(PaymentFormTypeMiscChargeOrder))]
        [XmlElement("PaymentCard", typeof(PaymentCardType))]
        [XmlElement("Ticket", typeof(PaymentFormTypeTicket))]
        [XmlElement("Voucher", typeof(PaymentFormTypeVoucher))]
        public object Item;

        [XmlAttribute()]
        public AddressTypeShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressTypeShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CostCenterID;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public PaymentFormTypePaymentTransactionTypeCode PaymentTransactionTypeCode;

        [XmlIgnore()]
        public bool PaymentTransactionTypeCodeSpecified;

        [XmlAttribute()]
        public bool GuaranteeIndicator;

        [XmlIgnore()]
        public bool GuaranteeIndicatorSpecified;

        [XmlAttribute()]
        public string GuaranteeTypeCode;

        [XmlAttribute()]
        public string GuaranteeID;

        [XmlAttribute()]
        public string Remark;
    }





    [XmlType()]
    public partial class BankAcctType
    {

        public string BankAcctName;

        [XmlAttribute()]
        public AddressTypeShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressTypeShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string BankID;

        [XmlAttribute()]
        public string AcctType;

        [XmlAttribute()]
        public string BankAcctNumber;

        [XmlAttribute()]
        public bool ChecksAcceptedInd;

        [XmlIgnore()]
        public bool ChecksAcceptedIndSpecified;

        [XmlAttribute()]
        public string CheckNumber;
    }





    [XmlType()]
    public partial class PaymentFormTypeCash
    {

        [XmlAttribute()]
        public bool CashIndicator;

        [XmlIgnore()]
        public bool CashIndicatorSpecified;
    }





    [XmlType()]
    public partial class DirectBillType
    {

        public DirectBillTypeCompanyName CompanyName;

        public AddressInfoType Address;

        public EmailType Email;

        public DirectBillTypeTelephone Telephone;

        [XmlAttribute()]
        public AddressTypeShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressTypeShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string DirectBill_ID;

        [XmlAttribute()]
        public string BillingNumber;
    }





    [XmlType()]
    public partial class DirectBillTypeCompanyName : CompanyNameType
    {

        [XmlAttribute()]
        public string ContactName;
    }





    [XmlType()]
    public partial class DirectBillTypeTelephone
    {

        [XmlAttribute()]
        public string RPH;
    }





    [XmlType()]
    public partial class PaymentFormTypeLoyaltyRedemption
    {

        [XmlElement("LoyaltyCertificate")]
        public PaymentFormTypeLoyaltyRedemptionLoyaltyCertificate[] LoyaltyCertificate;

        [XmlAttribute()]
        public string CertificateNumber;

        [XmlAttribute()]
        public string MemberNumber;

        [XmlAttribute()]
        public string ProgramName;

        [XmlAttribute()]
        public string PromotionCode;

        [XmlAttribute()]
        public string[] PromotionVendorCode;

        [XmlAttribute(DataType = "positiveInteger")]
        public string RedemptionQuantity;
    }





    [XmlType()]
    public partial class PaymentFormTypeLoyaltyRedemptionLoyaltyCertificate
    {

        [XmlAttribute()]
        public string ID_Context;

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string NmbrOfNights;

        [XmlAttribute()]
        public PaymentFormTypeLoyaltyRedemptionLoyaltyCertificateFormat Format;

        [XmlIgnore()]
        public bool FormatSpecified;

        [XmlAttribute()]
        public string Status;
    }



    [XmlType()]
    public enum PaymentFormTypeLoyaltyRedemptionLoyaltyCertificateFormat
    {

        Paper,

        Electronic
    }





    [XmlType()]
    public partial class PaymentFormTypeMiscChargeOrder
    {

        [XmlAttribute()]
        public string TicketNumber;

        [XmlAttribute()]
        public string OriginalTicketNumber;

        [XmlAttribute()]
        public string OriginalIssuePlace;

        [XmlAttribute(DataType = "date")]
        public DateTime OriginalIssueDate;

        [XmlIgnore()]
        public bool OriginalIssueDateSpecified;

        [XmlAttribute()]
        public string OriginalIssueIATA;

        [XmlAttribute()]
        public string OriginalPaymentForm;

        [XmlAttribute()]
        public PaymentFormTypeMiscChargeOrderCheckInhibitorType CheckInhibitorType;

        [XmlIgnore()]
        public bool CheckInhibitorTypeSpecified;

        [XmlAttribute()]
        public string[] CouponRPHs;

        [XmlAttribute()]
        public bool PaperMCO_ExistInd;

        [XmlIgnore()]
        public bool PaperMCO_ExistIndSpecified;
    }



    [XmlType()]
    public enum PaymentFormTypeMiscChargeOrderCheckInhibitorType
    {

        CheckDigit,

        InterlineAgreement,

        Both
    }





    [XmlType()]
    public partial class PaymentCardType
    {

        public string CardHolderName;

        public PaymentCardTypeCardIssuerName CardIssuerName;

        public AddressType Address;

        [XmlElement("Telephone")]
        public PaymentCardTypeTelephone[] Telephone;

        [XmlElement("Email")]
        public EmailType[] Email;

        [XmlElement("CustLoyalty")]
        public PaymentCardTypeCustLoyalty[] CustLoyalty;

        public PaymentCardTypeSignatureOnFile SignatureOnFile;

        public PaymentCardTypeMagneticStripe MagneticStripe;

        [XmlAttribute()]
        public AddressTypeShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressTypeShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CardType;

        [XmlAttribute()]
        public string CardCode;

        [XmlAttribute()]
        public string CardNumber;

        [XmlAttribute()]
        public string SeriesCode;

        [XmlAttribute()]
        public string EffectiveDate;

        [XmlAttribute()]
        public string ExpireDate;

        [XmlAttribute()]
        public string MaskedCardNumber;

        [XmlAttribute()]
        public string CardHolderRPH;

        [XmlAttribute()]
        public bool ExtendPaymentIndicator;

        [XmlIgnore()]
        public bool ExtendPaymentIndicatorSpecified;

        [XmlAttribute()]
        public string CountryOfIssue;

        [XmlAttribute(DataType = "integer")]
        public string ExtendedPaymentQuantity;

        [XmlAttribute()]
        public bool SignatureOnFileIndicator;

        [XmlIgnore()]
        public bool SignatureOnFileIndicatorSpecified;

        [XmlAttribute()]
        public string CompanyCardReference;

        [XmlAttribute()]
        public string Remark;

        [XmlAttribute()]
        public string EncryptionKey;
    }





    [XmlType()]
    public partial class PaymentCardTypeCardIssuerName
    {

        [XmlAttribute()]
        public string BankID;
    }





    [XmlType()]
    public partial class PaymentCardTypeTelephone
    {

        [XmlAttribute()]
        public string RPH;
    }





    [XmlType()]
    public partial class PaymentCardTypeCustLoyalty
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

        [XmlAttribute()]
        public bool PrimaryLoyaltyIndicator;

        [XmlIgnore()]
        public bool PrimaryLoyaltyIndicatorSpecified;

        [XmlAttribute()]
        public string AllianceLoyaltyLevelName;

        [XmlAttribute()]
        public string CustomerType;

        [XmlAttribute()]
        public string CustomerValue;

        [XmlAttribute()]
        public string Password;
    }





    [XmlType()]
    public partial class PaymentCardTypeSignatureOnFile
    {

        [XmlAttribute()]
        public bool SignatureOnFileIndicator;

        [XmlIgnore()]
        public bool SignatureOnFileIndicatorSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;

        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator;

        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;
    }





    [XmlType()]
    public partial class PaymentCardTypeMagneticStripe
    {

        [XmlAttribute(DataType = "base64Binary")]
        public byte[] Track1;

        [XmlAttribute(DataType = "base64Binary")]
        public byte[] Track2;

        [XmlAttribute(DataType = "base64Binary")]
        public byte[] Track3;
    }





    [XmlType()]
    public partial class PaymentFormTypeTicket
    {

        [XmlElement("ConjunctionTicketNbr")]
        public PaymentFormTypeTicketConjunctionTicketNbr[] ConjunctionTicketNbr;

        [XmlAttribute()]
        public string TicketNumber;

        [XmlAttribute()]
        public string OriginalTicketNumber;

        [XmlAttribute()]
        public string OriginalIssuePlace;

        [XmlAttribute(DataType = "date")]
        public DateTime OriginalIssueDate;

        [XmlIgnore()]
        public bool OriginalIssueDateSpecified;

        [XmlAttribute()]
        public string OriginalIssueIATA;

        [XmlAttribute()]
        public string OriginalPaymentForm;

        [XmlAttribute()]
        public PaymentFormTypeMiscChargeOrderCheckInhibitorType CheckInhibitorType;

        [XmlIgnore()]
        public bool CheckInhibitorTypeSpecified;

        [XmlAttribute()]
        public string[] CouponRPHs;

        [XmlAttribute()]
        public PaymentFormTypeTicketReroutingType ReroutingType;

        [XmlIgnore()]
        public bool ReroutingTypeSpecified;

        [XmlAttribute()]
        public string ReasonForReroute;
    }





    [XmlType()]
    public partial class PaymentFormTypeTicketConjunctionTicketNbr
    {

        [XmlAttribute()]
        public string[] Coupons;

        [XmlText()]
        public string Value;
    }



    [XmlType()]
    public enum PaymentFormTypeTicketReroutingType
    {

        voluntary,

        involuntary
    }





    [XmlType()]
    public partial class PaymentFormTypeVoucher
    {

        [XmlAttribute()]
        public string SeriesCode;

        [XmlAttribute()]
        public string BillingNumber;

        [XmlAttribute()]
        public string SupplierIdentifier;

        [XmlAttribute()]
        public string Identifier;

        [XmlAttribute()]
        public string ValueType;

        [XmlAttribute()]
        public bool ElectronicIndicator;

        [XmlIgnore()]
        public bool ElectronicIndicatorSpecified;
    }



    [XmlType()]
    public enum PaymentFormTypePaymentTransactionTypeCode
    {

        charge,

        reserve,

        refund
    }





    [XmlType()]
    public partial class VehicleEquipmentType
    {

        public string Description;

        [XmlAttribute()]
        public string EquipType;

        [XmlAttribute(DataType = "positiveInteger")]
        public string Quantity;

        [XmlAttribute()]
        public EquipmentRestrictionType Restriction;

        [XmlIgnore()]
        public bool RestrictionSpecified;
    }



    [XmlType()]
    public enum EquipmentRestrictionType
    {

        OneWayOnly,

        RoundTripOnly,

        AnyReservation
    }





    [XmlType()]
    public partial class VehicleEquipmentPricedType
    {

        public VehicleEquipmentType Equipment;

        public VehicleChargeType Charge;

        [XmlAttribute()]
        public bool Required;

        [XmlIgnore()]
        public bool RequiredSpecified;
    }





    [XmlType()]
    public partial class NoShowFeeType
    {

        public NoShowFeeTypeDeadline Deadline;

        public NoShowFeeTypeGracePeriod GracePeriod;

        public NoShowFeeTypeFeeAmount FeeAmount;

        public FormattedTextTextType Description;
    }





    [XmlType()]
    public partial class NoShowFeeTypeDeadline
    {

        [XmlAttribute()]
        public string AbsoluteDeadline;

        [XmlAttribute()]
        public TimeUnitType OffsetTimeUnit;

        [XmlIgnore()]
        public bool OffsetTimeUnitSpecified;

        [XmlAttribute(DataType = "integer")]
        public string OffsetUnitMultiplier;

        [XmlAttribute()]
        public VehicleRentalRateTypeRateGuaranteeOffsetDropTime OffsetDropTime;

        [XmlIgnore()]
        public bool OffsetDropTimeSpecified;
    }





    [XmlType()]
    public partial class NoShowFeeTypeGracePeriod
    {

        [XmlAttribute()]
        public string AbsoluteDeadline;

        [XmlAttribute()]
        public TimeUnitType OffsetTimeUnit;

        [XmlIgnore()]
        public bool OffsetTimeUnitSpecified;

        [XmlAttribute(DataType = "integer")]
        public string OffsetUnitMultiplier;

        [XmlAttribute()]
        public VehicleRentalRateTypeRateGuaranteeOffsetDropTime OffsetDropTime;

        [XmlIgnore()]
        public bool OffsetDropTimeSpecified;
    }





    [XmlType()]
    public partial class NoShowFeeTypeFeeAmount
    {

        [XmlAttribute()]
        public decimal Amount;

        [XmlIgnore()]
        public bool AmountSpecified;

        [XmlAttribute()]
        public bool RateConvertedInd;

        [XmlIgnore()]
        public bool RateConvertedIndSpecified;

        [XmlAttribute()]
        public bool GuaranteeReqInd;

        [XmlIgnore()]
        public bool GuaranteeReqIndSpecified;

        [XmlAttribute()]
        public bool EmailRequiredInd;

        [XmlIgnore()]
        public bool EmailRequiredIndSpecified;
    }





    [XmlType()]
    public partial class RateQualifierType
    {

        public string PromoDesc;

        [XmlArrayItem("RateComment", IsNullable = false)]
        public RateQualifierTypeRateComment[] RateComments;

        [XmlAttribute()]
        public string TravelPurpose;

        [XmlAttribute()]
        public string RateCategory;

        [XmlAttribute()]
        public string CorpDiscountNmbr;

        [XmlAttribute()]
        public string RateQualifier;

        [XmlAttribute()]
        public RateQualifierTypeRatePeriod RatePeriod;

        [XmlIgnore()]
        public bool RatePeriodSpecified;

        [XmlAttribute()]
        public bool GuaranteedInd;

        [XmlIgnore()]
        public bool GuaranteedIndSpecified;

        [XmlAttribute()]
        public bool ArriveByFlight;

        [XmlIgnore()]
        public bool ArriveByFlightSpecified;

        [XmlAttribute()]
        public string RateAuthorizationCode;

        [XmlAttribute()]
        public string VendorRateID;
    }





    [XmlType()]
    public partial class RateQualifierTypeRateComment : FormattedTextTextType
    {

        [XmlAttribute()]
        public string Name;
    }



    [XmlType()]
    public enum RateQualifierTypeRatePeriod
    {

        Hourly,

        Daily,

        Weekly,

        Monthly,

        WeekendDay,

        Other,

        Package,

        Bundle,

        Total
    }





    [XmlType()]
    public partial class VehicleRentalRateType
    {

        [XmlElement("RateDistance")]
        public VehicleRentalRateTypeRateDistance[] RateDistance;

        [XmlArrayItem("VehicleCharge", IsNullable = false)]
        public VehicleChargePurposeType[] VehicleCharges;

        public VehicleRentalRateTypeRateQualifier RateQualifier;

        public VehicleRentalRateTypeRateRestrictions RateRestrictions;

        public VehicleRentalRateTypeRateGuarantee RateGuarantee;

        [XmlElement("PickupReturnRule")]
        public VehicleRentalRateTypePickupReturnRule[] PickupReturnRule;

        public NoShowFeeType NoShowFeeInfo;

        [XmlAttribute()]
        public string QuoteID;
    }





    [XmlType()]
    public partial class VehicleRentalRateTypeRateDistance
    {

        [XmlAttribute()]
        public bool Unlimited;

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string Quantity;

        [XmlAttribute()]
        public DistanceUnitNameType DistUnitName;

        [XmlIgnore()]
        public bool DistUnitNameSpecified;

        [XmlAttribute()]
        public VehiclePeriodUnitNameType VehiclePeriodUnitName;

        [XmlIgnore()]
        public bool VehiclePeriodUnitNameSpecified;
    }



    [XmlType()]
    public enum DistanceUnitNameType
    {

        Mile,

        Km,

        Block
    }



    [XmlType()]
    public enum VehiclePeriodUnitNameType
    {

        RentalPeriod,

        Year,

        Month,

        Week,

        Day,

        Hour,

        Weekend,

        ExtraMonth,

        Bundle,

        Package,

        ExtraDay,

        ExtraHour,

        ExtraWeek
    }





    [XmlType()]
    public partial class VehicleRentalRateTypeRateQualifier : RateQualifierType
    {

        [XmlAttribute()]
        public string TourInfoRPH;

        [XmlAttribute()]
        public string[] CustLoyaltyRPH;

        [XmlAttribute()]
        public string QuoteID;
    }





    [XmlType()]
    public partial class VehicleRentalRateTypeRateRestrictions
    {

        [XmlAttribute()]
        public bool ArriveByFlight;

        [XmlIgnore()]
        public bool ArriveByFlightSpecified;

        [XmlAttribute()]
        public bool MinimumDayInd;

        [XmlIgnore()]
        public bool MinimumDayIndSpecified;

        [XmlAttribute()]
        public bool MaximumDayInd;

        [XmlIgnore()]
        public bool MaximumDayIndSpecified;

        [XmlAttribute()]
        public bool AdvancedBookingInd;

        [XmlIgnore()]
        public bool AdvancedBookingIndSpecified;

        [XmlAttribute()]
        public bool RestrictedMileageInd;

        [XmlIgnore()]
        public bool RestrictedMileageIndSpecified;

        [XmlAttribute()]
        public bool CorporateRateInd;

        [XmlIgnore()]
        public bool CorporateRateIndSpecified;

        [XmlAttribute()]
        public bool GuaranteeReqInd;

        [XmlIgnore()]
        public bool GuaranteeReqIndSpecified;

        [XmlAttribute(DataType = "integer")]
        public string MaximumVehiclesAllowed;

        [XmlAttribute()]
        public bool OvernightInd;

        [XmlIgnore()]
        public bool OvernightIndSpecified;

        [XmlAttribute()]
        public VehicleRentalRateTypeRateRestrictionsOneWayPolicy OneWayPolicy;

        [XmlIgnore()]
        public bool OneWayPolicySpecified;

        [XmlAttribute()]
        public bool CancellationPenaltyInd;

        [XmlIgnore()]
        public bool CancellationPenaltyIndSpecified;

        [XmlAttribute()]
        public bool ModificationPenaltyInd;

        [XmlIgnore()]
        public bool ModificationPenaltyIndSpecified;

        [XmlAttribute(DataType = "integer")]
        public string MinimumAge;

        [XmlAttribute(DataType = "integer")]
        public string MaximumAge;

        [XmlAttribute()]
        public bool NoShowFeeInd;

        [XmlIgnore()]
        public bool NoShowFeeIndSpecified;
    }



    [XmlType()]
    public enum VehicleRentalRateTypeRateRestrictionsOneWayPolicy
    {

        OneWayAllowed,

        OneWayNotAllowed,

        RestrictedOneWay
    }





    [XmlType()]
    public partial class VehicleRentalRateTypeRateGuarantee
    {

        public FormattedTextTextType Description;

        [XmlAttribute()]
        public string AbsoluteDeadline;

        [XmlAttribute()]
        public TimeUnitType OffsetTimeUnit;

        [XmlIgnore()]
        public bool OffsetTimeUnitSpecified;

        [XmlAttribute(DataType = "integer")]
        public string OffsetUnitMultiplier;

        [XmlAttribute()]
        public VehicleRentalRateTypeRateGuaranteeOffsetDropTime OffsetDropTime;

        [XmlIgnore()]
        public bool OffsetDropTimeSpecified;
    }





    [XmlType()]
    public partial class VehicleRentalRateTypePickupReturnRule
    {

        [XmlAttribute()]
        public DayOfWeekType DayOfWeek;

        [XmlIgnore()]
        public bool DayOfWeekSpecified;

        [XmlAttribute()]
        public string Time;

        [XmlAttribute()]
        public VehicleRentalRateTypePickupReturnRuleRuleType RuleType;

        [XmlIgnore()]
        public bool RuleTypeSpecified;
    }



    [XmlType()]
    public enum DayOfWeekType
    {

        Mon,

        Tue,

        Wed,

        Thu,

        Fri,

        Sat,

        Sun
    }



    [XmlType()]
    public enum VehicleRentalRateTypePickupReturnRuleRuleType
    {

        EarliestPickup,

        LatestPickup,

        LatestReturn
    }

    [XmlType()]
    public partial class VehicleCoreType
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
    public partial class VehicleCoreTypeVehType
    {

        [XmlAttribute()]
        public string VehicleCategory;

        [XmlAttribute()]
        public string DoorCount;
    }





    [XmlType()]
    public partial class VehicleCoreTypeVehClass
    {

        [XmlAttribute()]
        public string Size;
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
    public partial class VehicleType : VehicleCoreType
    {

        public VehicleTypeVehMakeModel VehMakeModel;

        [XmlElement(DataType = "anyURI")]
        public string PictureURL;

        public VehicleTypeVehIdentity VehIdentity;

        [XmlAttribute()]
        public string PassengerQuantity;

        [XmlAttribute(DataType = "integer")]
        public string BaggageQuantity;

        [XmlAttribute()]
        public string VendorCarType;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute()]
        public decimal UnitOfMeasureQuantity;

        [XmlIgnore()]
        public bool UnitOfMeasureQuantitySpecified;

        [XmlAttribute()]
        public string UnitOfMeasure;

        [XmlAttribute()]
        public string UnitOfMeasureCode;

        [XmlAttribute()]
        public string Start;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute()]
        public string End;

        [XmlAttribute()]
        public DistanceUnitNameType OdometerUnitOfMeasure;

        [XmlIgnore()]
        public bool OdometerUnitOfMeasureSpecified;

        [XmlAttribute()]
        public string Description;
    }





    [XmlType()]
    public partial class VehicleTypeVehMakeModel
    {

        [XmlAttribute(DataType = "gYear")]
        public string ModelYear;
    }





    [XmlType()]
    public partial class VehicleTypeVehIdentity
    {

        [XmlAttribute()]
        public string VehicleAssetNumber;

        [XmlAttribute()]
        public string LicensePlateNumber;

        [XmlAttribute()]
        public string StateProvCode;

        [XmlAttribute()]
        public string CountryCode;

        [XmlAttribute()]
        public string VehicleID_Number;

        [XmlAttribute()]
        public string VehicleColor;
    }





    [XmlType()]
    public partial class VehiclePrefType : VehicleCoreType
    {

        public VehiclePrefTypeVehMakeModel VehMakeModel;

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
    public partial class VehiclePrefTypeVehMakeModel
    {

        [XmlAttribute(DataType = "gYear")]
        public string ModelYear;
    }





    [XmlType()]
    public partial class VehicleRentalCoreType
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
    public partial class VehicleRentalCoreTypePickUpLocation : LocationType
    {

        [XmlAttribute()]
        public string ExtendedLocationCode;

        [XmlAttribute()]
        public string CounterLocation;
    }





    [XmlType()]
    public partial class VehicleRentalCoreTypeReturnLocation : LocationType
    {

        [XmlAttribute()]
        public string ExtendedLocationCode;

        [XmlAttribute()]
        public string CounterLocation;
    }





    [XmlType()]
    public partial class VehicleSegmentCoreType
    {

        [XmlElement("ConfID")]
        public VehicleSegmentCoreTypeConfID[] ConfID;

        public CompanyNameType Vendor;

        public VehicleRentalCoreType VehRentalCore;

        public VehicleType Vehicle;

        public VehicleRentalRateType RentalRate;

        [XmlArrayItem("PricedEquip", IsNullable = false)]
        public VehicleEquipmentPricedType[] PricedEquips;

        [XmlArrayItem("Fee", IsNullable = false)]
        public VehicleChargePurposeType[] Fees;

        public VehicleSegmentCoreTypeTotalCharge TotalCharge;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute(DataType = "integer")]
        public string IndexNumber;
    }





    [XmlType()]
    public partial class VehicleSegmentCoreTypeConfID : UniqueID_Type
    {

        [XmlAttribute()]
        public string Status;
    }





    [XmlType()]
    public partial class VehicleSegmentCoreTypeTotalCharge
    {

        [XmlAttribute()]
        public decimal RateTotalAmount;

        [XmlIgnore()]
        public bool RateTotalAmountSpecified;

        [XmlAttribute()]
        public decimal EstimatedTotalAmount;

        [XmlIgnore()]
        public bool EstimatedTotalAmountSpecified;
    }





    [XmlType()]
    public partial class DocumentType
    {

        [XmlElement("DocHolderFormattedName", typeof(PersonNameType))]
        [XmlElement("DocHolderName", typeof(string))]
        public object Item;

        [XmlElement("DocLimitations")]
        public string[] DocLimitations;

        [XmlArrayItem("AdditionalPersonName", IsNullable = false)]
        public string[] AdditionalPersonNames;

        [XmlAttribute()]
        public AddressTypeShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressTypeShareMarketInd ShareMarketInd;

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
        public DocumentTypeGender Gender;

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

        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator;

        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;

        [XmlAttribute()]
        public string DocIssueStateProv;

        [XmlAttribute()]
        public string DocIssueCountry;

        [XmlAttribute()]
        public string BirthCountry;

        [XmlAttribute()]
        public string BirthPlace;

        [XmlAttribute()]
        public string DocHolderNationality;

        [XmlAttribute()]
        public string ContactName;

        [XmlAttribute()]
        public DocumentTypeHolderType HolderType;

        [XmlIgnore()]
        public bool HolderTypeSpecified;

        [XmlAttribute()]
        public string Remark;

        [XmlAttribute()]
        public string PostalCode;
    }



    [XmlType()]
    public enum DocumentTypeGender
    {

        Male,

        Female,

        Unknown,

        Male_NoShare,

        Female_NoShare
    }



    [XmlType()]
    public enum DocumentTypeHolderType
    {

        Infant,

        HeadOfHousehold
    }





    [XmlType()]
    public partial class EmployeeInfoType
    {

        [XmlAttribute()]
        public string EmployeeId;

        [XmlAttribute()]
        public string EmployeeLevel;

        [XmlAttribute()]
        public string EmployeeTitle;

        [XmlAttribute()]
        public string EmployeeStatus;

        [XmlText()]
        public string Value;
    }





    [XmlType()]
    public partial class ContactPersonType
    {

        public PersonNameType PersonName;

        [XmlElement("Telephone")]
        public ContactPersonTypeTelephone[] Telephone;

        [XmlElement("Address")]
        public AddressInfoType[] Address;

        [XmlElement("Email")]
        public EmailType[] Email;

        [XmlElement("URL")]
        public URL_Type[] URL;

        [XmlElement("CompanyNameFull")]
        public CompanyNameType[] CompanyName;

        [XmlElement("EmployeeInfo")]
        public EmployeeInfoType[] EmployeeInfo;

        [XmlAttribute()]
        public AddressTypeShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressTypeShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public bool DefaultInd;

        [XmlIgnore()]
        public bool DefaultIndSpecified;

        [XmlAttribute()]
        public string ContactType;

        [XmlAttribute()]
        public string Relation;

        [XmlAttribute()]
        public bool EmergencyFlag;

        [XmlIgnore()]
        public bool EmergencyFlagSpecified;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public string CommunicationMethodCode;

        [XmlAttribute()]
        public string DocumentDistribMethodCode;
    }





    [XmlType()]
    public partial class ContactPersonTypeTelephone
    {

        [XmlAttribute()]
        public string RPH;
    }





    [XmlType()]
    public partial class URL_Type
    {

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

        [XmlAttribute()]
        public bool DefaultInd;

        [XmlIgnore()]
        public bool DefaultIndSpecified;

        [XmlText(DataType = "anyURI")]
        public string Value;
    }





    [XmlType()]
    public partial class RelatedTravelerType
    {

        public UniqueID_Type UniqueID;

        public PersonNameType PersonName;

        [XmlAttribute()]
        public AddressTypeShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressTypeShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string Relation;

        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate;

        [XmlIgnore()]
        public bool BirthDateSpecified;
    }





    [XmlType()]
    public partial class CustomerType
    {

        [XmlElement("PersonName")]
        public PersonNameType[] PersonName;

        [XmlElement("Telephone")]
        public CustomerTypeTelephone[] Telephone;

        [XmlElement("Email")]
        public CustomerTypeEmail[] Email;

        [XmlElement("Address")]
        public CustomerTypeAddress[] Address;

        [XmlElement("URL")]
        public CustomerTypeURL[] URL;

        [XmlElement("CitizenCountryName")]
        public CustomerTypeCitizenCountryName[] CitizenCountryName;

        [XmlElement("PhysChallName")]
        public CustomerTypePhysChallName[] PhysChallName;

        [XmlElement("PetInfo")]
        public string[] PetInfo;

        [XmlElement("PaymentForm")]
        public CustomerTypePaymentForm[] PaymentForm;

        [XmlElement("RelatedTraveler")]
        public RelatedTravelerType[] RelatedTraveler;

        [XmlElement("ContactPerson")]
        public ContactPersonType[] ContactPerson;

        [XmlElement("Document")]
        public DocumentType[] Document;

        [XmlElement("CustLoyalty")]
        public CustomerTypeCustLoyalty[] CustLoyalty;

        [XmlElement("EmployeeInfo")]
        public EmployeeInfoType[] EmployeeInfo;

        public CompanyNameType EmployerInfo;

        [XmlElement("AdditionalLanguage")]
        public CustomerTypeAdditionalLanguage[] AdditionalLanguage;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public DocumentTypeGender Gender;

        [XmlIgnore()]
        public bool GenderSpecified;

        [XmlAttribute()]
        public bool Deceased;

        [XmlIgnore()]
        public bool DeceasedSpecified;

        [XmlAttribute()]
        public string LockoutType;

        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate;

        [XmlIgnore()]
        public bool BirthDateSpecified;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string DecimalPlaces;

        [XmlAttribute()]
        public bool VIP_Indicator;

        [XmlIgnore()]
        public bool VIP_IndicatorSpecified;

        [XmlAttribute()]
        public string Text;

        [XmlAttribute(DataType = "language")]
        public string Language;

        [XmlAttribute()]
        public string CustomerValue;

        [XmlAttribute()]
        public CustomerTypeMaritalStatus MaritalStatus;

        [XmlIgnore()]
        public bool MaritalStatusSpecified;

        [XmlAttribute()]
        public bool PreviouslyMarriedIndicator;

        [XmlIgnore()]
        public bool PreviouslyMarriedIndicatorSpecified;

        [XmlAttribute(DataType = "integer")]
        public string ChildQuantity;
    }





    [XmlType()]
    public partial class CustomerTypeTelephone
    {

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;

        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator;

        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public TransferActionType TransferAction;

        [XmlIgnore()]
        public bool TransferActionSpecified;

        [XmlAttribute()]
        public string ParentCompanyRef;
    }



    [XmlType()]
    public enum TransferActionType
    {

        Automatic,

        Mandatory,

        Selectable
    }





    [XmlType()]
    public partial class CustomerTypeEmail : EmailType
    {

        [XmlAttribute()]
        public TransferActionType TransferAction;

        [XmlIgnore()]
        public bool TransferActionSpecified;

        [XmlAttribute()]
        public string ParentCompanyRef;
    }





    [XmlType()]
    public partial class CustomerTypeAddress : AddressInfoType
    {

        public CompanyNameType CompanyName;

        public PersonNameType AddresseeName;

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;

        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator;

        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;

        [XmlAttribute()]
        public CustomerTypeAddressValidationStatus ValidationStatus;

        [XmlIgnore()]
        public bool ValidationStatusSpecified;

        [XmlAttribute()]
        public TransferActionType TransferAction;

        [XmlIgnore()]
        public bool TransferActionSpecified;

        [XmlAttribute()]
        public string ParentCompanyRef;
    }



    [XmlType()]
    public enum CustomerTypeAddressValidationStatus
    {

        SystemValidated,

        UserValidated,

        NotChecked
    }





    [XmlType()]
    public partial class CustomerTypeURL : URL_Type
    {

        [XmlAttribute()]
        public TransferActionType TransferAction;

        [XmlIgnore()]
        public bool TransferActionSpecified;
    }





    [XmlType()]
    public partial class CustomerTypeCitizenCountryName
    {

        [XmlAttribute()]
        public string Code;
    }





    [XmlType()]
    public partial class CustomerTypePhysChallName
    {

        [XmlAttribute()]
        public bool PhysChallInd;

        [XmlIgnore()]
        public bool PhysChallIndSpecified;

        [XmlText()]
        public string Value;
    }





    [XmlType()]
    public partial class CustomerTypePaymentForm : PaymentFormType
    {

        public CustomerTypePaymentFormAssociatedSupplier AssociatedSupplier;

        [XmlAttribute()]
        public TransferActionType TransferAction;

        [XmlIgnore()]
        public bool TransferActionSpecified;

        [XmlAttribute()]
        public bool DefaultInd;

        [XmlIgnore()]
        public bool DefaultIndSpecified;

        [XmlAttribute()]
        public string ParentCompanyRef;
    }





    [XmlType()]
    public partial class CustomerTypePaymentFormAssociatedSupplier
    {

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
    public partial class CustomerTypeCustLoyalty
    {

        public CustomerTypeCustLoyaltyMemberPreferences MemberPreferences;

        public CustomerTypeCustLoyaltySecurityInfo SecurityInfo;

        [XmlElement("SubAccountBalance")]
        public CustomerTypeCustLoyaltySubAccountBalance[] SubAccountBalance;

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

        [XmlAttribute()]
        public bool PrimaryLoyaltyIndicator;

        [XmlIgnore()]
        public bool PrimaryLoyaltyIndicatorSpecified;

        [XmlAttribute()]
        public string AllianceLoyaltyLevelName;

        [XmlAttribute()]
        public string CustomerType;

        [XmlAttribute()]
        public string CustomerValue;

        [XmlAttribute()]
        public string Password;

        [XmlAttribute()]
        public string Remark;
    }





    [XmlType()]
    public partial class CustomerTypeCustLoyaltyMemberPreferences
    {

        [XmlElement("AdditionalReward")]
        public CustomerTypeCustLoyaltyMemberPreferencesAdditionalReward[] AdditionalReward;

        [XmlElement("Offer")]
        public CustomerTypeCustLoyaltyMemberPreferencesOffer[] Offer;

        [XmlAttribute()]
        public string Awareness;

        [XmlAttribute()]
        public string PromotionCode;

        [XmlAttribute()]
        public string[] PromotionVendorCode;

        [XmlAttribute()]
        public CustomerTypeCustLoyaltyMemberPreferencesAwardsPreference AwardsPreference;

        [XmlIgnore()]
        public bool AwardsPreferenceSpecified;
    }





    [XmlType()]
    public partial class CustomerTypeCustLoyaltyMemberPreferencesAdditionalReward
    {

        public CompanyNameType CompanyName;

        public PersonNameType Name;

        [XmlAttribute()]
        public string MemberID;
    }





    [XmlType()]
    public partial class CustomerTypeCustLoyaltyMemberPreferencesOffer
    {

        [XmlElement("Communication")]
        public CustomerTypeCustLoyaltyMemberPreferencesOfferCommunication[] Communication;

        [XmlAttribute()]
        public CustomerTypeCustLoyaltyMemberPreferencesOfferType Type;

        [XmlIgnore()]
        public bool TypeSpecified;
    }





    [XmlType()]
    public partial class CustomerTypeCustLoyaltyMemberPreferencesOfferCommunication
    {

        [XmlAttribute()]
        public string DistribType;
    }



    [XmlType()]
    public enum CustomerTypeCustLoyaltyMemberPreferencesOfferType
    {

        Partner,

        Loyalty
    }



    [XmlType()]
    public enum CustomerTypeCustLoyaltyMemberPreferencesAwardsPreference
    {

        Points,

        Miles
    }





    [XmlType()]
    public partial class CustomerTypeCustLoyaltySecurityInfo
    {

        [XmlElement("PasswordHint")]
        public CustomerTypeCustLoyaltySecurityInfoPasswordHint[] PasswordHint;

        [XmlAttribute()]
        public string Username;

        [XmlAttribute()]
        public string Password;
    }





    [XmlType()]
    public partial class CustomerTypeCustLoyaltySecurityInfoPasswordHint
    {

        [XmlAttribute()]
        public CustomerTypeCustLoyaltySecurityInfoPasswordHintHint Hint;

        [XmlIgnore()]
        public bool HintSpecified;

        [XmlText()]
        public string Value;
    }



    [XmlType()]
    public enum CustomerTypeCustLoyaltySecurityInfoPasswordHintHint
    {

        Question,

        Answer
    }





    [XmlType()]
    public partial class CustomerTypeCustLoyaltySubAccountBalance
    {

        [XmlAttribute()]
        public string Type;

        [XmlAttribute(DataType = "integer")]
        public string Balance;
    }





    [XmlType()]
    public partial class CustomerTypeAdditionalLanguage
    {

        [XmlAttribute(DataType = "language")]
        public string Code;
    }



    [XmlType()]
    public enum CustomerTypeMaritalStatus
    {

        Annulled,

        [XmlEnum("Co-habitating")]
        Cohabitating,

        Divorced,

        Engaged,

        Married,

        Separated,

        Single,

        Widowed,

        Unknown
    }





    [XmlType()]
    public partial class CustomerPrimaryAdditionalType
    {

        public CustomerPrimaryAdditionalTypePrimary Primary;

        [XmlElement("Additional")]
        public CustomerPrimaryAdditionalTypeAdditional[] Additional;
    }





    [XmlType()]
    public partial class CustomerPrimaryAdditionalTypePrimary : CustomerType
    {

        public UniqueID_Type CustomerID;
    }





    [XmlType()]
    public partial class CustomerPrimaryAdditionalTypeAdditional : CustomerType
    {

        [XmlAttribute()]
        public string Start;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute()]
        public string End;

        [XmlAttribute()]
        public string CorpDiscountName;

        [XmlAttribute()]
        public string CorpDiscountNmbr;

        [XmlAttribute()]
        public CustomerPrimaryAdditionalTypeAdditionalQualificationMethod QualificationMethod;

        [XmlIgnore()]
        public bool QualificationMethodSpecified;

        [XmlAttribute(DataType = "integer")]
        public string Age;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute(DataType = "anyURI")]
        public string URI;
    }



    [XmlType()]
    public enum CustomerPrimaryAdditionalTypeAdditionalQualificationMethod
    {

        RT_AirlineTicket,

        CreditCard,

        PassportAndReturnTkt
    }





    [XmlType()]
    public partial class ItemSearchCriterionType
    {

        public ItemSearchCriterionTypePosition Position;

        public ItemSearchCriterionTypeAddress Address;

        public ItemSearchCriterionTypeTelephone Telephone;

        [XmlElement("RefPoint")]
        public ItemSearchCriterionTypeRefPoint[] RefPoint;

        public ItemSearchCriterionTypeCodeRef CodeRef;

        [XmlElement("HotelRef")]
        public ItemSearchCriterionTypeHotelRef[] HotelRef;

        public ItemSearchCriterionTypeRadius Radius;

        public ItemSearchCriterionTypeMapArea MapArea;

        [XmlArrayItem("AdditionalContent", IsNullable = false)]
        public ItemSearchCriterionTypeAdditionalContent[] AdditionalContents;

        [XmlAttribute()]
        public bool ExactMatch;

        [XmlIgnore()]
        public bool ExactMatchSpecified;

        [XmlAttribute()]
        public ItemSearchCriterionTypeImportanceType ImportanceType;

        [XmlIgnore()]
        public bool ImportanceTypeSpecified;

        [XmlAttribute(DataType = "integer")]
        public string Ranking;
    }





    [XmlType()]
    public partial class ItemSearchCriterionTypePosition
    {

        [XmlAttribute()]
        public string Latitude;

        [XmlAttribute()]
        public string Longitude;

        [XmlAttribute()]
        public string Altitude;

        [XmlAttribute()]
        public string AltitudeUnitOfMeasureCode;

        [XmlAttribute()]
        public string PositionAccuracy;
    }





    [XmlType()]
    public partial class ItemSearchCriterionTypeAddress : AddressType
    {

        [XmlAttribute()]
        public bool SameCountryInd;

        [XmlIgnore()]
        public bool SameCountryIndSpecified;

        [XmlAttribute()]
        public ItemSearchCriterionTypeAddressAddressSearchScope AddressSearchScope;

        [XmlIgnore()]
        public bool AddressSearchScopeSpecified;
    }



    [XmlType()]
    public enum ItemSearchCriterionTypeAddressAddressSearchScope
    {

        Primary,

        Alternate,

        PrimaryAndAlternate
    }





    [XmlType()]
    public partial class ItemSearchCriterionTypeTelephone
    {
    }





    [XmlType()]
    public partial class ItemSearchCriterionTypeRefPoint
    {

        [XmlAttribute()]
        public string StateProv;

        [XmlAttribute()]
        public string CountryCode;

        [XmlAttribute(DataType = "anyURI")]
        public string URI;

        [XmlAttribute()]
        public string RefPointType;

        [XmlAttribute()]
        public string Name;

        [XmlAttribute()]
        public string CityName;

        [XmlText()]
        public string Value;
    }





    [XmlType()]
    public partial class ItemSearchCriterionTypeCodeRef : LocationType
    {

        [XmlAttribute()]
        public string VicinityCode;
    }





    [XmlType()]
    public partial class ItemSearchCriterionTypeHotelRef
    {

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
        public string SegmentCategoryCode;

        [XmlAttribute()]
        public string PropertyClassCode;

        [XmlAttribute()]
        public string ArchitecturalStyleCode;

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string SupplierIntegrationLevel;

        [XmlAttribute()]
        public string LocationCategoryCode;

        [XmlAttribute()]
        public bool ExtendedCitySearchIndicator;

        [XmlIgnore()]
        public bool ExtendedCitySearchIndicatorSpecified;
    }





    [XmlType()]
    public partial class ItemSearchCriterionTypeRadius
    {

        [XmlAttribute()]
        public string Distance;

        [XmlAttribute()]
        public string DistanceMeasure;

        [XmlAttribute()]
        public string Direction;

        [XmlAttribute()]
        public string DistanceMax;

        [XmlAttribute()]
        public string UnitOfMeasureCode;
    }





    [XmlType()]
    public partial class ItemSearchCriterionTypeMapArea
    {

        [XmlAttribute()]
        public string NorthLatitude;

        [XmlAttribute()]
        public string SouthLatitude;

        [XmlAttribute()]
        public string EastLongitude;

        [XmlAttribute()]
        public string WestLongitude;
    }





    [XmlType()]
    public partial class ItemSearchCriterionTypeAdditionalContent
    {

        [XmlAttribute()]
        public string ContentGroupCode;

        [XmlAttribute()]
        public string CodeDetail;
    }



    [XmlType()]
    public enum ItemSearchCriterionTypeImportanceType
    {

        Mandatory,

        High,

        Medium,

        Low
    }





    [XmlType()]
    public partial class OperatingAirlineType : CompanyNameType
    {

        [XmlAttribute()]
        public string FlightNumber;

        [XmlAttribute()]
        public string ResBookDesigCode;
    }





    [XmlType()]
    public partial class CompanyNamePrefType : CompanyNameType
    {

        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        [XmlIgnore()]
        public bool PreferLevelSpecified;
    }





    [XmlType()]
    public partial class Position
    {

        [XmlAttribute()]
        public string Latitude;

        [XmlAttribute()]
        public string Longitude;

        [XmlAttribute()]
        public string Altitude;

        [XmlAttribute()]
        public string AltitudeUnitOfMeasureCode;

        [XmlAttribute()]
        public string PositionAccuracy;
    }





    [XmlType()]
    public partial class BookingChannel
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
    public enum OTA_VehLocSearchRQTarget
    {

        Test,

        Production
    }



    [XmlType()]
    public enum OTA_VehLocSearchRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries,

        Continuation,

        Subsequent
    }

    [XmlType()]
    [XmlRoot("VehReservation", IsNullable = false)]
    public partial class VehicleReservationType
    {

        public CustomerPrimaryAdditionalType Customer;

        public VehicleReservationTypeVehSegmentCore VehSegmentCore;

        public VehicleSegmentAdditionalInfoType VehSegmentInfo;

        [XmlAttribute()]
        public DateTime CreateDateTime;

        [XmlIgnore()]
        public bool CreateDateTimeSpecified;

        [XmlAttribute()]
        public string CreatorID;

        [XmlAttribute()]
        public DateTime LastModifyDateTime;

        [XmlIgnore()]
        public bool LastModifyDateTimeSpecified;

        [XmlAttribute()]
        public string LastModifierID;

        [XmlAttribute(DataType = "date")]
        public DateTime PurgeDate;

        [XmlIgnore()]
        public bool PurgeDateSpecified;

        [XmlAttribute()]
        public string ReservationStatus;
    }





    [XmlType()]
    public partial class VehicleReservationTypeVehSegmentCore : VehicleSegmentCoreType
    {

        [XmlAttribute()]
        public bool OptionChangeAllowedIndicator;

        [XmlIgnore()]
        public bool OptionChangeAllowedIndicatorSpecified;
    }
}
