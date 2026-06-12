using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmStoredFareUpdateIn
{

    [XmlRoot(IsNullable = false)]
    public class OTA_StoredFareUpdateRQ
    {

        public POS POS;

        public UniqueIDRS UniqueID;

        public AirItineraryRQ AirItinerary;

        public OTA_StoredFareUpdateRQFare Fare;

        public OTA_StoredFareUpdateRQPassengerType PassengerType;

        [XmlElement("BankerRates")]
        public OTA_StoredFareUpdateRQBankerRates[] BankerRates;

        [XmlElement("Segments")]
        public OTA_StoredFareUpdateRQSegments[] Segments;

        public string FareCalcLine;

        [XmlElement("OtherFareInformation")]
        public OTA_StoredFareUpdateRQOtherFareInformation[] OtherFareInformation;

        [XmlElement("Status")]
        public OTA_StoredFareUpdateRQStatus[] Status;

        [XmlElement("AirlineFees")]
        public OTA_StoredFareUpdateRQAirlineFees[] AirlineFees;

        public decimal Mileage;

        [XmlIgnore()]
        public bool MileageSpecified;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_StoredFareUpdateRQTarget.Production)]
        public OTA_StoredFareUpdateRQTarget Target = OTA_StoredFareUpdateRQTarget.Production;

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
        public OTA_StoredFareUpdateRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQSalesIndicator SalesIndicator;

        [XmlIgnore()]
        public bool SalesIndicatorSpecified;
    }
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    public class OTA_StoredFareUpdateRQAirlineFeesFeeInformationTaxesDetails
    {

        [XmlAttribute()]
        public string Rate;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationTaxesDetailsType Type;
    }

    public enum OTA_StoredFareUpdateRQAirlineFeesFeeInformationTaxesDetailsType
    {

        [XmlEnum("1")]
        Item1,

        [XmlEnum("2")]
        Item2,

        [XmlEnum("3")]
        Item3,

        [XmlEnum("4")]
        Item4,

        [XmlEnum("5")]
        Item5,

        [XmlEnum("6")]
        Item6,

        [XmlEnum("7")]
        Item7,

        [XmlEnum("8")]
        Item8,

        [XmlEnum("9")]
        Item9,

        [XmlEnum("10")]
        Item10,

        [XmlEnum("11")]
        Item11,

        [XmlEnum("12")]
        Item12,

        [XmlEnum("13")]
        Item13,

        [XmlEnum("14")]
        Item14,

        [XmlEnum("15")]
        Item15,

        [XmlEnum("16")]
        Item16,

        [XmlEnum("17")]
        Item17,

        [XmlEnum("18")]
        Item18,

        [XmlEnum("19")]
        Item19,

        [XmlEnum("20")]
        Item20,

        [XmlEnum("21")]
        Item21,

        [XmlEnum("22")]
        Item22,

        [XmlEnum("23")]
        Item23,

        [XmlEnum("24")]
        Item24
    }

    public class OTA_StoredFareUpdateRQAirlineFeesFeeInformationTaxes
    {

        [XmlElement("Details")]
        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationTaxesDetails[] Details;

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationTaxesTaxCategory TaxCategory;

        [XmlIgnore()]
        public bool TaxCategorySpecified;
    }

    public enum OTA_StoredFareUpdateRQAirlineFeesFeeInformationTaxesTaxCategory
    {

        TX
    }

    public class OTA_StoredFareUpdateRQAirlineFeesFeeInformationAmountPrice
    {

        [XmlAttribute()]
        public string PriceTypeCode;

        [XmlAttribute()]
        public string Amount;

        [XmlAttribute()]
        public string RestrictedIndicator;

        [XmlAttribute()]
        public string CodeDetail;

        [XmlText()]
        public string Value;
    }

    public class OTA_StoredFareUpdateRQAirlineFeesFeeInformationAmountCurrency
    {

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationAmountCurrencyCurrencyCode CurrencyCode;

        [XmlAttribute()]
        public string DecimalPlaces;
    }

    public enum OTA_StoredFareUpdateRQAirlineFeesFeeInformationAmountCurrencyCurrencyCode
    {

        [XmlEnum("777")]
        Item777
    }

    public class OTA_StoredFareUpdateRQAirlineFeesFeeInformationAmount
    {

        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationAmountCurrency Currency;

        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationAmountPrice Price;

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationAmountQualifier Qualifier;
    }

    public enum OTA_StoredFareUpdateRQAirlineFeesFeeInformationAmountQualifier
    {

        TAX,

        TEX,

        TIN
    }

    public class OTA_StoredFareUpdateRQAirlineFeesFeeInformationFeeName
    {

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationFeeNameQualifier Qualifier;

        [XmlAttribute()]
        public string Name;
    }

    public enum OTA_StoredFareUpdateRQAirlineFeesFeeInformationFeeNameQualifier
    {

        COM
    }

    public class OTA_StoredFareUpdateRQAirlineFeesFeeInformationFeePropertiesFeeApplication
    {

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationFeePropertiesFeeApplicationCode Code;

        [XmlIgnore()]
        public bool CodeSpecified;
    }

    public enum OTA_StoredFareUpdateRQAirlineFeesFeeInformationFeePropertiesFeeApplicationCode
    {

        CM,

        NC,

        NI,

        NR
    }

    public class OTA_StoredFareUpdateRQAirlineFeesFeeInformationFeeProperties
    {

        [XmlElement("FeeApplication")]
        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationFeePropertiesFeeApplication[] FeeApplication;

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationFeePropertiesType Type;
    }

    public enum OTA_StoredFareUpdateRQAirlineFeesFeeInformationFeePropertiesType
    {

        FC1,

        FC2,

        FC3,

        FC4,

        FC5,

        FC6,

        FCA,

        [XmlEnum("FC2")]
        FC21,

        R01,

        R02,

        R03,

        R04,

        R05,

        R06,

        R07,

        R08,

        R09,

        R10,

        R11,

        R12,

        R13,

        R14,

        R15,

        R16,

        R17,

        R18,

        R19,

        R20,

        T01,

        T02,

        T03,

        T04,

        T05,

        T06,

        T07,

        T08,

        T09,

        T10,

        T11,

        T12,

        T13,

        T14,

        T15,

        T16,

        T17,

        T18,

        T19,

        T20
    }

    public class OTA_StoredFareUpdateRQAirlineFeesFeeInformation
    {

        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationFeeProperties FeeProperties;

        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationFeeName FeeName;

        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationAmount Amount;

        [XmlElement("Taxes")]
        public OTA_StoredFareUpdateRQAirlineFeesFeeInformationTaxes[] Taxes;
    }

    public class OTA_StoredFareUpdateRQAirlineFees
    {

        [XmlElement("FeeInformation")]
        public OTA_StoredFareUpdateRQAirlineFeesFeeInformation[] FeeInformation;

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQAirlineFeesFeeType FeeType;
    }

    public enum OTA_StoredFareUpdateRQAirlineFeesFeeType
    {

        OB
    }

    public class OTA_StoredFareUpdateRQStatus
    {

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQStatusType Type;

        [XmlIgnore()]
        public bool TypeSpecified;

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQStatusStatusCode StatusCode;
    }

    public enum OTA_StoredFareUpdateRQStatusType
    {

        FirstStatus,

        OtherStatus
    }

    public enum OTA_StoredFareUpdateRQStatusStatusCode
    {

        ADT,

        CNF,

        ETK,

        INF,

        INT,

        PTS
    }

    public class OTA_StoredFareUpdateRQOtherFareInformation
    {

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQOtherFareInformationType Type;

        [XmlAttribute()]
        public string Description;
    }

    public enum OTA_StoredFareUpdateRQOtherFareInformationType
    {

        FCA,

        PAY,

        SAT
    }

    public class OTA_StoredFareUpdateRQSegmentsBagAllowance
    {

        [XmlAttribute()]
        public int Quantity;

        [XmlIgnore()]
        public bool QuantitySpecified;

        [XmlAttribute()]
        public decimal Weight;

        [XmlIgnore()]
        public bool WeightSpecified;

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQSegmentsBagAllowanceType Type;

        [XmlIgnore()]
        public bool TypeSpecified;

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQSegmentsBagAllowanceUnit Unit;

        [XmlIgnore()]
        public bool UnitSpecified;
    }

    public enum OTA_StoredFareUpdateRQSegmentsBagAllowanceType
    {

        Piece,

        Weight
    }

    public enum OTA_StoredFareUpdateRQSegmentsBagAllowanceUnit
    {

        K,

        L
    }

    public class OTA_StoredFareUpdateRQSegmentsFareValidity
    {

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQSegmentsFareValidityValidityReason ValidityReason;

        [XmlIgnore()]
        public bool ValidityReasonSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ValidityDate;

        [XmlIgnore()]
        public bool ValidityDateSpecified;
    }

    public enum OTA_StoredFareUpdateRQSegmentsFareValidityValidityReason
    {

        After,

        Before
    }

    public class OTA_StoredFareUpdateRQSegmentsFareBasis
    {

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQSegmentsFareBasisMovementType MovementType;

        [XmlIgnore()]
        public bool MovementTypeSpecified;

        [XmlAttribute()]
        public string PrimaryCode;

        [XmlAttribute()]
        public string FareBasisCode;

        [XmlAttribute()]
        public string TicketDesignator;

        [XmlAttribute()]
        public string DiscountTicketDesignator;

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQSegmentsFareBasisDiscountType DiscountType;

        [XmlIgnore()]
        public bool DiscountTypeSpecified;

        [XmlAttribute()]
        public decimal DiscountAmount;

        [XmlIgnore()]
        public bool DiscountAmountSpecified;

        [XmlAttribute(DataType = "integer")]
        public string DiscountPercentage;
    }

    public enum OTA_StoredFareUpdateRQSegmentsFareBasisMovementType
    {

        HD,

        HR
    }

    public enum OTA_StoredFareUpdateRQSegmentsFareBasisDiscountType
    {

        [XmlEnum("70A")]
        Item70A,

        [XmlEnum("70B")]
        Item70B
    }

    public class OTA_StoredFareUpdateRQSegments
    {

        public OTA_StoredFareUpdateRQSegmentsFareBasis FareBasis;

        [XmlElement("FareValidity")]
        public OTA_StoredFareUpdateRQSegmentsFareValidity[] FareValidity;

        public OTA_StoredFareUpdateRQSegmentsBagAllowance BagAllowance;

        [XmlAttribute(DataType = "integer")]
        public string RPH;
    }

    public class OTA_StoredFareUpdateRQBankerRatesPrice
    {

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQBankerRatesPricePriceTypeCode PriceTypeCode;

        [XmlAttribute()]
        public decimal Amount;

        [XmlIgnore()]
        public bool AmountSpecified;

        [XmlAttribute()]
        public string RestrictedIndicator;

        [XmlAttribute()]
        public string CodeDetail;

        [XmlText()]
        public string Value;
    }

    public enum OTA_StoredFareUpdateRQBankerRatesPricePriceTypeCode
    {

        FirstRate,

        SecondRate
    }

    public class OTA_StoredFareUpdateRQBankerRatesCurrency
    {

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQBankerRatesCurrencyCurrencyCode CurrencyCode;

        [XmlAttribute()]
        public string DecimalPlaces;
    }

    public enum OTA_StoredFareUpdateRQBankerRatesCurrencyCurrencyCode
    {

        [XmlEnum("777")]
        Item777
    }

    public class OTA_StoredFareUpdateRQBankerRates
    {

        public OTA_StoredFareUpdateRQBankerRatesCurrency Currency;

        public OTA_StoredFareUpdateRQBankerRatesPrice Price;
    }

    public class OTA_StoredFareUpdateRQPassengerType
    {

        [XmlAttribute()]
        public string Code;
    }

    public class OTA_StoredFareUpdateRQFareTaxesTax
    {

        [XmlAttribute()]
        public string TaxCode;

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
    }

    public class OTA_StoredFareUpdateRQFareTaxes
    {

        [XmlElement("Tax")]
        public OTA_StoredFareUpdateRQFareTaxesTax[] Tax;

        [XmlAttribute(DataType = "integer")]
        public string Qualifier;

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQFareTaxesIdentifier Identifier;

        [XmlAttribute()]
        public string ContryCode;

        [XmlAttribute()]
        public string Nature;

        [XmlAttribute()]
        public OTA_StoredFareUpdateRQFareTaxesExemptIndicator ExemptIndicator;

        [XmlIgnore()]
        public bool ExemptIndicatorSpecified;
    }

    public enum OTA_StoredFareUpdateRQFareTaxesIdentifier
    {

        GST,

        PD,

        RFD,

        RPF,

        X
    }

    public enum OTA_StoredFareUpdateRQFareTaxesExemptIndicator
    {

        E
    }

    public class OTA_StoredFareUpdateRQFareBaseFare
    {

        [XmlAttribute()]
        public double Amount;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public int DecimalPlaces;

        [XmlIgnore()]
        public bool DecimalPlacesSpecified;
    }

    public class OTA_StoredFareUpdateRQFareEquivFare
    {

        [XmlAttribute()]
        public double Amount;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public int DecimalPlaces;

        [XmlIgnore()]
        public bool DecimalPlacesSpecified;
    }

    public class OTA_StoredFareUpdateRQFare
    {

        public OTA_StoredFareUpdateRQFareBaseFare BaseFare;

        public OTA_StoredFareUpdateRQFareEquivFare EquivFare;

        public OTA_StoredFareUpdateRQFareTaxes Taxes;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public string UniqueRef;

        [XmlAttribute()]
        public FareIdentifierRQ FareIdentifier;
    }

    public enum FareIdentifierRQ
    {

        F,

        I,

        U,

        R,

        Y,

        W
    }

    [XmlRoot(IsNullable = false)]
    public class UniqueIDRS
    {

        [XmlAttribute()]
        public UniqueIDQualifier Qualifier;

        [XmlIgnore()]
        public bool QualifierSpecified;

        [XmlAttribute()]
        public string ID;

        [XmlAttribute()]
        public string UniqueRef;
    }

    public enum UniqueIDQualifier
    {

        P,

        S,

        TST
    }

    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        public object Name;

        public string System;

        public string Userid;

        public string Password;
    }

    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions
    {

        public Provider Provider;
    }

    public enum OTA_StoredFareUpdateRQTarget
    {

        Test,

        Production
    }

    public enum OTA_StoredFareUpdateRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }

    public enum OTA_StoredFareUpdateRQSalesIndicator
    {

        II,

        IO,

        OI,

        OO
    }

    [XmlRoot(IsNullable = false)]
    public class AirItineraryRQ
    {

        [XmlArrayItem("OriginDestinationOption", IsNullable = false)]
        [XmlArrayItem(IsNullable = false, NestingLevel = 1)]
        public FlightSegment[][] OriginDestinationOptions;

        [XmlAttribute()]
        public AirItineraryDirectionInd DirectionInd;

        [XmlIgnore()]
        public bool DirectionIndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class FlightSegment
    {

        public DepartureAirportRQ DepartureAirport;

        public ArrivalAirportRQ ArrivalAirport;

        public OperatingAirlineRQ OperatingAirline;

        [XmlElement("Equipment")]
        public EquipmentRQ[] Equipment;

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

    public enum AirItineraryDirectionInd
    {

        OneWay,

        Return,

        Circle,

        OpenJaw,

        Other
    }

    [XmlRoot(IsNullable = false)]
    public class DepartureAirportRQ
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
    public class ArrivalAirportRQ
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
    public class OperatingAirlineRQ
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
        public OperatingAirlineFlightNumberRQ FlightNumber;

        [XmlIgnore()]
        public bool FlightNumberSpecified;

        [XmlText()]
        public string Value;
    }

    public enum OperatingAirlineFlightNumberRQ
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
    public class EquipmentRQ
    {

        [XmlAttribute()]
        public string AirEquipType;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool ChangeofGauge = false;

        [XmlText()]
        public string Value;
    }

    public enum FlightSegmentActionCode
    {

        OK,

        Waitlist,

        Other
    }
}
