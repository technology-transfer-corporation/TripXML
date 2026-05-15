using System;
using System.Xml.Serialization;
using wsTripXML.wsTravelTalk.wmTravelItinerary;

namespace wsTripXML.wsTravelTalk.wmTravelItineraryIn_v04
{

    [XmlRoot(IsNullable = false)]
    public class AcceptedPayment
    {

        public PaymentCard PaymentCard;

        public BankAcct BankAcct;

        public DirectBill DirectBill;

        public Voucher Voucher;

        [XmlAttribute()]
        public AcceptedPaymentShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AcceptedPaymentShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CostCenterId;

        [XmlAttribute()]
        public string RPH;
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
    public class CardIssuerName
    {

        [XmlAttribute()]
        public string BankID;
    }

    [XmlRoot(IsNullable = false)]
    public class Voucher
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
        public string SeriesCode;
    }

    [XmlRoot(IsNullable = false)]
    public class DirectBill
    {

        public Address Address;

        [XmlAttribute()]
        public DirectBillShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public DirectBillShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string DirectBill_ID;
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
    public class AcceptedPayments
    {

        [XmlElement("AcceptedPayment")]
        public AcceptedPayment[] AcceptedPayment;
    }

    [XmlRoot(IsNullable = false)]
    public class Additional
    {

        public PersonName PersonName;

        [XmlElement("Telephone")]
        public Telephone[] Telephone;

        [XmlElement("Email")]
        public Email[] Email;

        [XmlElement("Address")]
        public Address[] Address;

        [XmlElement("CitizenCountryName")]
        public CitizenCountryName[] CitizenCountryName;

        [XmlElement("Document")]
        public Document[] Document;

        [XmlElement("CustLoyalty")]
        public CustLoyalty[] CustLoyalty;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public string BirthDate;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public int DecimalPlaces;

        [XmlIgnore()]
        public bool DecimalPlacesSpecified;
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
    public class CitizenCountryName
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public string Code;
    }

    [XmlRoot(IsNullable = false)]
    public class Document
    {

        public string DocHolderName;

        public string DocLimitations;

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
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;

        public PNRData PNRData;

        public CarData CarData;

        public HotelData HotelData;

        [XmlElement("PriceData")]
        public PriceData[] PriceData;

        public AgencyData AgencyData;

        [XmlAttribute()]
        public string FlightType;

        [XmlAttribute()]
        public string DateChange;

        [XmlAttribute()]
        public bool FlownIndicator;

        [XmlIgnore()]
        public bool FlownIndicatorSpecified;

        [XmlAttribute()]
        public string FOPType;

        [XmlAttribute()]
        public string ConfirmationNumber;
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
    public class PNRData
    {

        [XmlElement("Traveler")]
        public Traveler[] Traveler;

        [XmlElement("Telephone")]
        public Telephone[] Telephone;

        [XmlElement("Email")]
        public Email[] Email;

        public Address Address;

        public Ticketing Ticketing;

        [XmlElement("AccountingLine")]
        public string[] AccountingLine;

        public Queue Queue;

        [XmlElement("MCO")]
        public MCO[] MCO;

    }

    [XmlRoot(IsNullable = false)]
    public class MCO
    {

        public TravelerRefNumber TravelerRefNumber;

        public MCOFare MCOFare;

        public DateTime DepartureDate;

        public MCOType Type;

    }

    [XmlRoot(IsNullable = false)]
    public class MCOType
    {

        public string Text;

        [XmlAttribute()]
        public string Code;

    }

    [XmlRoot(IsNullable = false)]
    public class Traveler
    {

        public PersonName PersonName;

        [XmlElement("CustLoyalty")]
        public CustLoyalty[] CustLoyalty;

        [XmlElement("Document")]
        public Document[] Document;

        public TravelerRefNumber TravelerRefNumber;

        [XmlArrayItem("FlightSegmentRPH", IsNullable = false)]
        public string[] FlightSegmentRPHs;

        public string Age;

        [XmlAttribute()]
        public TravelerGender Gender;

        [XmlIgnore()]
        public bool GenderSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate;

        [XmlIgnore()]
        public bool BirthDateSpecified;

        [XmlAttribute()]
        public string PassengerTypeCode;

        [XmlAttribute()]
        public bool AccompaniedByInfant;

        [XmlIgnore()]
        public bool AccompaniedByInfantSpecified;
    }

    public enum TravelerGender
    {

        Male,

        Female,

        Unknown
    }

    [XmlRoot(IsNullable = false)]
    public class Ticketing
    {

        [XmlElement("TicketAdvisory")]
        public TicketAdvisory[] TicketAdvisory;

        [XmlAttribute()]
        public DateTime TicketTimeLimit;

        [XmlIgnore()]
        public bool TicketTimeLimitSpecified;

        [XmlAttribute()]
        public TicketingTicketType TicketType;
    }

    public enum TicketingTicketType
    {

        eTicket,

        Paper,

        None
    }

    [XmlRoot(IsNullable = false)]
    public class CarData
    {

        public CarLocation CarLocation;

        public CarRate CarRate;

        [XmlAttribute()]
        public string NumCars;
    }

    [XmlRoot(IsNullable = false)]
    public class CarLocation
    {

        [XmlAttribute()]
        public string Category;

        [XmlAttribute()]
        public string Number;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class CarRate
    {

        [XmlAttribute()]
        public string Rate;

        [XmlAttribute()]
        public string Currency;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class HotelData
    {

        public string ExtraAdult;

        public string ExtraChild;

        public string Crib;

        public string RollawayAdult;

        public string RollawayChild;
    }

    [XmlRoot(IsNullable = false)]
    public class PriceData
    {

        public PublishedFares PublishedFares;

        public NegoFares NegoFares;

        [XmlElement("FareDiscount")]
        public FareDiscount[] FareDiscount;

        [XmlElement("PassengerTypeQuantity")]
        public PassengerTypeQuantity[] PassengerTypeQuantity;

        [XmlAttribute()]
        public PriceDataPriceType PriceType;

        [XmlIgnore()]
        public bool PriceTypeSpecified;

        [XmlAttribute()]
        public bool AutoTicketing;

        [XmlAttribute()]
        public string ValidatingAirlineCode;

        [XmlAttribute()]
        public string TravelerRefNumberRPHList;

        [XmlAttribute()]
        public string FlightRefNumberRPHList;

        [XmlAttribute()]
        public string PricingInstruction;

    }

    [XmlRoot(IsNullable = false)]
    public class FareDiscount
    {

        public BaseFare BaseFare;

        [XmlArrayItem(IsNullable = false)]
        public Tax[] Taxes;

        public TotalFare TotalFare;

        [XmlAttribute()]
        public string TravelerRefNumberRPHList;

        [XmlAttribute()]
        public string DiscountCode;

    }

    public class AgencyData
    {

        public AgencyDataCommission Commission;

        public AgencyDataServiceFee ServiceFee;

        [XmlElement("EndRules")]
        public EndRules[] EndRules;
    }

    [XmlRoot(IsNullable = false)]
    public class EndRules
    {

        [XmlAttribute()]
        public string RuleType;

        [XmlAttribute()]
        public string RuleName;

        [XmlText()]
        public string Value;

    }

    [XmlRoot(IsNullable = false)]
    public class PublishedFares
    {

        public FareRestrictPref FareRestrictPref;
    }

    [XmlRoot(IsNullable = false)]
    public class FareRestrictPref
    {

        public AdvResTicketing AdvResTicketing;

        public MinimumStay MinimumStay;

        public MaximumStay MaximumStay;

        public VoluntaryChanges VoluntaryChanges;
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

        [XmlText()]
        public string Value;
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

        [XmlText()]
        public string Value;
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
    }

    [XmlRoot(IsNullable = false)]
    public class NegoFares
    {

        public PriceRequestInformation PriceRequestInformation;
    }

    [XmlRoot(IsNullable = false)]
    public class PriceRequestInformation
    {

        [XmlElement("NegotiatedFareCode")]
        public NegotiatedFareCode[] NegotiatedFareCode;

        [XmlAttribute()]
        public PriceRequestInformationPricingSource PricingSource;

        [XmlIgnore()]
        public bool PricingSourceSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class NegotiatedFareCode
    {

        [XmlAttribute()]
        public string Code;

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

    public enum PriceRequestInformationPricingSource
    {

        Published,

        Private
    }

    public enum PriceDataPriceType
    {

        Published,

        Private
    }

    public class AgencyDataCommission
    {

        [XmlAttribute()]
        public decimal Amount;

        [XmlIgnore()]
        public bool AmountSpecified;

        [XmlAttribute()]
        public decimal Percent;

        [XmlIgnore()]
        public bool PercentSpecified;
    }

    public class AgencyDataServiceFee
    {

        [XmlAttribute()]
        public decimal Amount;

        [XmlIgnore()]
        public bool AmountSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class AdditionalDetail
    {

        public DetailDescription DetailDescription;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public double Amount;

        [XmlIgnore()]
        public bool AmountSpecified;

        [XmlAttribute()]
        public string CurrencyCode;
    }

    [XmlRoot(IsNullable = false)]
    public class DetailDescription
    {

        public string Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        [XmlAttribute()]
        public int ParagraphNumber;

        [XmlIgnore()]
        public bool ParagraphNumberSpecified;

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

        [XmlAttribute()]
        public string Language;
    }

    [XmlRoot(IsNullable = false)]
    public class AdditionalDetails
    {

        [XmlElement("AdditionalDetail")]
        public AdditionalDetail[] AdditionalDetail;
    }

    [XmlRoot(IsNullable = false)]
    public class AirItinerary
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
    public class MiscellaneousSegments
    {
        [XmlElement("Segment")]
        public Segment[] Segment;
    }
    [XmlRoot(IsNullable = false)]
    public class Segment
    {
        [XmlAttribute()]
        public string Type;
        [XmlAttribute()]
        public int NumberInParty;
        [XmlAttribute()]
        public string Vendor;
        [XmlAttribute()]
        public string Date;
        [XmlAttribute()]
        public string LocationCode;
        [XmlElement(IsNullable = false)]
        public string Text;
    }

    [XmlRoot(IsNullable = false)]
    public class FlightSegment
    {

        public DepartureAirport DepartureAirport;

        public ArrivalAirport ArrivalAirport;

        public OperatingAirline OperatingAirline;

        [XmlElement("Equipment")]
        public Equipment[] Equipment;

        public MarketingAirline MarketingAirline;

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

        [XmlAttribute()]
        public string FlightContext;
    }

    public enum FlightSegmentActionCode
    {

        OK,

        Waitlist,

        Other
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

        public TPA_Extensions TPA_Extensions;

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
    public class ArrivalDetails
    {

        public ArrivalLocation ArrivalLocation;

        public MarketingCompany MarketingCompany;

        public OperatingCompany OperatingCompany;

        [XmlAttribute()]
        public string TransportationCode;

        [XmlAttribute()]
        public string Number;

        [XmlAttribute()]
        public DateTime ArrivalDateTime;

        [XmlIgnore()]
        public bool ArrivalDateTimeSpecified;
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
    public class MarketingCompany
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
    public class OperatingCompany
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
    public class BaseFare
    {

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

    [XmlRoot(IsNullable = false)]
    public class BasicPropertyInfo
    {

        public Address Address;

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
        public string AreaId;
    }

    [XmlRoot(IsNullable = false)]
    public class BookingReferenceID
    {

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
    public class ChargesRules
    {

        public VoluntaryChanges VoluntaryChanges;
    }

    [XmlRoot(IsNullable = false)]
    public class Comment
    {

        public string Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        [XmlAttribute()]
        public int ParagraphNumber;

        [XmlIgnore()]
        public bool ParagraphNumberSpecified;

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

        [XmlAttribute()]
        public string Language;
    }

    [XmlRoot(IsNullable = false)]
    public class Comments
    {

        [XmlElement("Comment")]
        public Comment[] Comment;
    }

    [XmlRoot(IsNullable = false)]
    public class CoveragePref
    {

        [XmlAttribute()]
        public string CoverageType;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(CoveragePrefPreferLevel.Preferred)]
        public CoveragePrefPreferLevel PreferLevel = CoveragePrefPreferLevel.Preferred;
    }

    public enum CoveragePrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class CoveragePrefs
    {

        [XmlElement("CoveragePref")]
        public CoveragePref[] CoveragePref;
    }

    [XmlRoot(IsNullable = false)]
    public class Customer
    {

        public Primary Primary;

        [XmlElement("Additional")]
        public Additional[] Additional;
    }

    [XmlRoot(IsNullable = false)]
    public class Primary
    {

        public PersonName PersonName;

        [XmlElement("Telephone")]
        public Telephone[] Telephone;

        [XmlElement("Email")]
        public Email[] Email;

        [XmlElement("Address")]
        public Address[] Address;

        [XmlElement("CitizenCountryName")]
        public CitizenCountryName[] CitizenCountryName;

        [XmlElement("Document")]
        public Document[] Document;

        [XmlElement("CustLoyalty")]
        public CustLoyalty[] CustLoyalty;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public string BirthDate;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public int DecimalPlaces;

        [XmlIgnore()]
        public bool DecimalPlacesSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class DateWindowRange
    {

        [XmlAttribute()]
        public string WindowBefore;

        [XmlAttribute()]
        public string WindowAfter;

        [XmlText()]
        public DateTime Value;
    }

    [XmlRoot(IsNullable = false)]
    public class DeliveryAddress
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
        public DeliveryAddressShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public DeliveryAddressShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string Type;
    }

    public enum DeliveryAddressShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum DeliveryAddressShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class DepositPayments
    {

        [XmlElement("RequiredPayment")]
        public RequiredPayment[] RequiredPayment;
    }

    [XmlRoot(IsNullable = false)]
    public class RequiredPayment
    {

        [XmlArrayItem(IsNullable = false)]
        public AcceptedPayment[] AcceptedPayments;

        [XmlElement("PaymentDescription")]
        public PaymentDescription[] PaymentDescription;

        [XmlAttribute()]
        public RequiredPaymentRetributionType RetributionType;

        [XmlIgnore()]
        public bool RetributionTypeSpecified;

        [XmlAttribute()]
        public string PaymentCode;
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentDescription
    {

        public string Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        [XmlAttribute()]
        public int ParagraphNumber;

        [XmlIgnore()]
        public bool ParagraphNumberSpecified;

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

        [XmlAttribute()]
        public string Language;
    }

    public enum RequiredPaymentRetributionType
    {

        ResAutoCancelled,

        ResNotGuaranteed
    }

    [XmlRoot(IsNullable = false)]
    public class DriverType
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
    }

    [XmlRoot(IsNullable = false)]
    public class EndDateWindow
    {

        [XmlAttribute(DataType = "date")]
        public DateTime EarliestDate;

        [XmlIgnore()]
        public bool EarliestDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime LatestDate;

        [XmlIgnore()]
        public bool LatestDateSpecified;

        [XmlAttribute()]
        public EndDateWindowDOW DOW;

        [XmlIgnore()]
        public bool DOWSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class EquivFare
    {

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

    [XmlRoot(IsNullable = false)]
    public class FareInfo
    {

        public DateTime DepartureDate;

        [XmlIgnore()]
        public bool DepartureDateSpecified;

        public string FareReference;

        public RuleInfo RuleInfo;

        public FilingAirline FilingAirline;

        public MarketingAirline MarketingAirline;

        public DepartureAirport DepartureAirport;

        public ArrivalAirport ArrivalAirport;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool NegotiatedFare = false;

        [XmlAttribute()]
        public string NegotiatedFareCode;
    }

    [XmlRoot(IsNullable = false)]
    public class RuleInfo
    {

        public ResTicketingRules ResTicketingRules;

        public LengthOfStayRules LengthOfStayRules;

        public ChargesRules ChargesRules;
    }

    [XmlRoot(IsNullable = false)]
    public class ResTicketingRules
    {

        public AdvResTicketing AdvResTicketing;
    }

    [XmlRoot(IsNullable = false)]
    public class LengthOfStayRules
    {

        public MinimumStay MinimumStay;

        public MaximumStay MaximumStay;

        [XmlAttribute()]
        public bool StayRestrictionsInd;

        [XmlIgnore()]
        public bool StayRestrictionsIndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class FareInfos
    {

        [XmlElement("FareInfo")]
        public FareInfo[] FareInfo;
    }

    [XmlRoot(IsNullable = false)]
    public class Fee
    {

        [XmlAttribute()]
        public string FeeCode;

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

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class Fees
    {

        [XmlElement("Fee")]
        public Fee[] Fee;
    }

    [XmlRoot(IsNullable = false)]
    public class FlightSegmentRPHs
    {

        [XmlElement("FlightSegmentRPH")]
        public string[] FlightSegmentRPH;
    }

    [XmlRoot(IsNullable = false)]
    public class Fulfillment
    {

        [XmlArrayItem(IsNullable = false)]
        public PaymentDetail[] PaymentDetails;

        public DeliveryAddress DeliveryAddress;

        public string Name;

        public Receipt Receipt;
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentDetail
    {

        public PaymentCard PaymentCard;

        public BankAcct BankAcct;

        public DirectBill DirectBill;

        public Voucher Voucher;

        public LoyaltyRedemption LoyaltyRedemption;

        public MiscChargeOrder MiscChargeOrder;

        public TPA_Extensions TPA_Extensions;

        public PaymentAmount PaymentAmount;

        [XmlAttribute()]
        public PaymentDetailShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public PaymentDetailShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CostCenterID;

        [XmlAttribute()]
        public string RPH;
    }

    [XmlRoot(IsNullable = false)]
    public class LoyaltyRedemption
    {

        [XmlAttribute()]
        public string CertificateNumber;

        [XmlAttribute()]
        public string MemberNumber;

        [XmlAttribute()]
        public string ProgramName;

        [XmlAttribute()]
        public string PromotionCode;

        [XmlAttribute()]
        public int RedemptionQuantity;

        [XmlIgnore()]
        public bool RedemptionQuantitySpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class MiscChargeOrder
    {

        [XmlAttribute()]
        public string TicketNumber;

        [XmlAttribute()]
        public string OriginalPaymentForm;
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentAmount
    {

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

    public enum PaymentDetailShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum PaymentDetailShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class Receipt
    {

        [XmlAttribute()]
        public string DistribType;
    }

    [XmlRoot(IsNullable = false)]
    public class Guarantee
    {

        [XmlArrayItem(IsNullable = false)]
        public GuaranteeAccepted[] GuaranteesAccepted;

        [XmlArrayItem(IsNullable = false)]
        public Comment[] Comments;

        [XmlElement("GuaranteeDescription")]
        public GuaranteeDescription[] GuaranteeDescription;

        [XmlAttribute()]
        public GuaranteeRetributionType RetributionType;

        [XmlIgnore()]
        public bool RetributionTypeSpecified;

        [XmlAttribute()]
        public string GuaranteeCode;

        [XmlAttribute()]
        public string GuaranteeType;

        [XmlAttribute(DataType = "time")]
        public DateTime HoldTime;

        [XmlIgnore()]
        public bool HoldTimeSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class GuaranteeAccepted
    {

        public PaymentCard PaymentCard;

        public BankAcct BankAcct;

        public DirectBill DirectBill;

        public Voucher Voucher;

        [XmlAttribute()]
        public GuaranteeAcceptedShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public GuaranteeAcceptedShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CostCenterId;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public bool Default;

        [XmlIgnore()]
        public bool DefaultSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class GuaranteeDescription
    {

        public string Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        [XmlAttribute()]
        public int ParagraphNumber;

        [XmlIgnore()]
        public bool ParagraphNumberSpecified;

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

        [XmlAttribute()]
        public string Language;
    }

    [XmlRoot(IsNullable = false)]
    public class GuaranteesAccepted
    {

        [XmlElement("GuaranteeAccepted")]
        public GuaranteeAccepted[] GuaranteeAccepted;
    }

    [XmlRoot(IsNullable = false)]
    public class GuestCount
    {

        [XmlAttribute()]
        public string AgeQualifyingCode;

        [XmlAttribute()]
        public int Age;

        [XmlIgnore()]
        public bool AgeSpecified;

        [XmlAttribute()]
        public int Count;
    }

    [XmlRoot(IsNullable = false)]
    public class GuestCounts
    {

        [XmlElement("GuestCount")]
        public GuestCount[] GuestCount;

        [XmlAttribute()]
        public bool IsPerRoom;

        [XmlIgnore()]
        public bool IsPerRoomSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class HotelReservation
    {

        public UniqueID UniqueId;

        [XmlArrayItem(IsNullable = false)]
        public RoomStay[] RoomStays;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public bool RoomStayReservation;

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
    }

    [XmlRoot(IsNullable = false)]
    public class RoomStay
    {

        [XmlArrayItem(IsNullable = false)]
        public RoomType[] RoomTypes;

        [XmlArrayItem(IsNullable = false)]
        public RoomRate[] RoomRates;

        public GuestCounts GuestCounts;

        public TimeSpanRQ TimeSpan;

        [XmlElement("Guarantee")]
        public Guarantee[] Guarantee;

        [XmlArrayItem(IsNullable = false)]
        public RequiredPayment[] DepositPayments;

        public Total Total;

        public BasicPropertyInfo BasicPropertyInfo;

        [XmlArrayItem(IsNullable = false)]
        public ResGuestRPH[] ResGuestRPHs;

        [XmlArrayItem(IsNullable = false)]
        public Membership[] Memberships;

        [XmlArrayItem(IsNullable = false)]
        public Comment[] Comments;

        [XmlArrayItem(IsNullable = false)]
        public SpecialRequest[] SpecialRequests;

        [XmlArrayItem(IsNullable = false)]
        public ServiceRPH[] ServiceRPHs;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public string MarketCode;

        [XmlAttribute()]
        public string SourceOfBusiness;

        [XmlAttribute()]
        public string DiscountCode;

        [XmlAttribute()]
        public string PromotionCode;
    }

    [XmlRoot(IsNullable = false)]
    public class RoomType
    {

        public RoomDescription RoomDescription;

        [XmlArrayItem(IsNullable = false)]
        public AdditionalDetail[] AdditionalDetails;

        [XmlElement("Amenity")]
        public Amenity[] Amenity;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public string RoomTypeCode;

        [XmlAttribute()]
        public int NumberOfUnits;

        [XmlIgnore()]
        public bool NumberOfUnitsSpecified;

        [XmlAttribute()]
        public bool IsRoom;

        [XmlIgnore()]
        public bool IsRoomSpecified;

        [XmlAttribute()]
        public bool IsConverted;

        [XmlIgnore()]
        public bool IsConvertedSpecified;

        [XmlAttribute()]
        public bool IsAlternate;

        [XmlIgnore()]
        public bool IsAlternateSpecified;

        [XmlAttribute()]
        public string InvBlockCode;

        [XmlAttribute()]
        public string ReqdGuaranteeType;
    }

    [XmlRoot(IsNullable = false)]
    public class RoomDescription
    {

        public string Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        [XmlAttribute()]
        public int ParagraphNumber;

        [XmlIgnore()]
        public bool ParagraphNumberSpecified;

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

        [XmlAttribute()]
        public string Language;
    }

    [XmlRoot(IsNullable = false)]
    public class RoomRate
    {

        public RoomRateDescription RoomRateDescription;

        [XmlAttribute()]
        public string BookingCode;

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;

        [XmlAttribute()]
        public string RoomTypeCode;

        [XmlAttribute()]
        public string InvBlockCode;

        [XmlAttribute()]
        public int NumberOfUnits;

        [XmlIgnore()]
        public bool NumberOfUnitsSpecified;

        [XmlAttribute()]
        public string RatePlanType;

        [XmlAttribute()]
        public string RatePlanCode;

        [XmlAttribute()]
        public string RatePlanID;

        [XmlAttribute()]
        public bool RatePlanQualifier;

        [XmlIgnore()]
        public bool RatePlanQualifierSpecified;

        [XmlAttribute()]
        public string PromotionCode;
    }

    [XmlRoot(IsNullable = false)]
    public class RoomRateDescription
    {

        public string Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        [XmlAttribute()]
        public int ParagraphNumber;

        [XmlIgnore()]
        public bool ParagraphNumberSpecified;

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

        [XmlAttribute()]
        public string Language;
    }

    [XmlRoot(IsNullable = false)]
    public class TimeSpanRQ
    {

        public DateWindowRange DateWindowRange;

        public StartDateWindow StartDateWindow;

        public EndDateWindow EndDateWindow;

        [XmlAttribute()]
        public DateTime Start;

        [XmlIgnore()]
        public bool StartSpecified;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute()]
        public DateTime End;

        [XmlIgnore()]
        public bool EndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class StartDateWindow
    {

        [XmlAttribute(DataType = "date")]
        public DateTime EarliestDate;

        [XmlIgnore()]
        public bool EarliestDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime LatestDate;

        [XmlIgnore()]
        public bool LatestDateSpecified;

        [XmlAttribute()]
        public StartDateWindowDOW DOW;

        [XmlIgnore()]
        public bool DOWSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class Total
    {

        [XmlArrayItem(IsNullable = false)]
        public Tax[] Taxes;

        [XmlAttribute()]
        public double AmountBeforeTax;

        [XmlIgnore()]
        public bool AmountBeforeTaxSpecified;

        [XmlAttribute()]
        public double AmountAfterTax;

        [XmlIgnore()]
        public bool AmountAfterTaxSpecified;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public int DecimalPlaces;

        [XmlIgnore()]
        public bool DecimalPlacesSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class Tax
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

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class ResGuestRPH
    {

        [XmlAttribute()]
        public string RPH;
    }

    [XmlRoot(IsNullable = false)]
    public class Membership
    {

        [XmlAttribute()]
        public string ProgramCode;

        [XmlAttribute()]
        public string BonusCode;

        [XmlAttribute()]
        public string AccountID;

        [XmlAttribute()]
        public int PointsEarned;

        [XmlIgnore()]
        public bool PointsEarnedSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class SpecialRequest
    {

        public string Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        [XmlAttribute()]
        public int ParagraphNumber;

        [XmlIgnore()]
        public bool ParagraphNumberSpecified;

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

        [XmlAttribute()]
        public string Language;

        [XmlAttribute()]
        public string RequestCode;
    }

    [XmlRoot(IsNullable = false)]
    public class ServiceRPH
    {

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public bool IsPerRoom;

        [XmlIgnore()]
        public bool IsPerRoomSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class HotelReservations
    {

        [XmlElement("HotelReservation")]
        public HotelReservation[] HotelReservation;
    }

    [XmlRoot(IsNullable = false)]
    public class ItinTotalFare
    {

        public BaseFare BaseFare;

        public EquivFare EquivFare;

        [XmlArrayItem(IsNullable = false)]
        public Tax[] Taxes;

        [XmlArrayItem(IsNullable = false)]
        public Fee[] Fees;

        public TotalFare TotalFare;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool NegotiatedFare = false;

        [XmlAttribute()]
        public string NegotiatedFareCode;
    }

    [XmlRoot(IsNullable = false)]
    public class TotalFare
    {

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

    [XmlRoot(IsNullable = false)]
    public class MCOFare
    {

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

    [XmlRoot(IsNullable = false)]
    public class Memberships
    {

        [XmlElement("Membership")]
        public Membership[] Membership;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_AirBookRQ
    {

        public AirItinerary AirItinerary;

        public MiscellaneousSegments MiscellaneousSegments;

        public PriceInfo PriceInfo;

        public TravelerInfo TravelerInfo;

        public Fulfillment Fulfillment;

        public Ticketing Ticketing;

        [XmlElement("Queue")]
        public Queue[] Queue;

        public BookingReferenceID BookingReferenceID;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_AirBookRQTarget.Production)]
        public OTA_AirBookRQTarget Target = OTA_AirBookRQTarget.Production;

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
        public OTA_AirBookRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;
    }

    [XmlRoot(IsNullable = false)]
    public class PriceInfo
    {

        public ItinTotalFare ItinTotalFare;

        [XmlArrayItem(IsNullable = false)]
        public PTC_FareBreakdown[] PTC_FareBreakdowns;

        [XmlArrayItem(IsNullable = false)]
        public FareInfo[] FareInfos;

        [XmlAttribute()]
        public PriceInfoPricingSource PricingSource;

        [XmlIgnore()]
        public bool PricingSourceSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool RepriceRequired = false;
    }

    [XmlRoot(IsNullable = false)]
    public class PTC_FareBreakdown
    {

        public PassengerTypeQuantity PassengerTypeQuantity;

        [XmlArrayItem("FareBasisCode", IsNullable = false)]
        public string[] FareBasisCodes;

        public PassengerFare PassengerFare;

        [XmlAttribute()]
        public PTC_FareBreakdownPricingSource PricingSource;

        [XmlIgnore()]
        public bool PricingSourceSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class PassengerFare
    {

        public BaseFare BaseFare;

        public EquivFare EquivFare;

        [XmlArrayItem(IsNullable = false)]
        public Tax[] Taxes;

        [XmlArrayItem(IsNullable = false)]
        public Fee[] Fees;

        public TotalFare TotalFare;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool NegotiatedFare = false;

        [XmlAttribute()]
        public string NegotiatedFareCode;
    }

    public enum PriceInfoPricingSource
    {

        Published,

        Private,

        Both
    }

    [XmlRoot(IsNullable = false)]
    public class TravelerInfo
    {

        [XmlElement("AirTraveler")]
        public AirTraveler[] AirTraveler;

        public SpecialReqDetails SpecialReqDetails;
    }

    [XmlRoot(IsNullable = false)]
    public class SpecialReqDetails
    {

        [XmlArrayItem(IsNullable = false)]
        public SeatRequest[] SeatRequests;

        [XmlArrayItem(IsNullable = false)]
        public SpecialServiceRequest[] SpecialServiceRequests;

        [XmlArrayItem(IsNullable = false)]
        public OtherServiceInformation[] OtherServiceInformations;

        [XmlArrayItem("Remark", IsNullable = false)]
        public string[] Remarks;

        [XmlArrayItem(IsNullable = false)]
        public CategorizedRemark[] CategorizedRemarks;

        [XmlArrayItem(IsNullable = false)]
        public UniqueRemark[] UniqueRemarks;

        [XmlArrayItem(IsNullable = false)]
        public SpecialRemark[] SpecialRemarks;

    }

    [XmlRoot(IsNullable = false)]
    public class SpecialRemark
    {

        [XmlElement("TravelerRefNumber")]
        public TravelerRefNumber[] TravelerRefNumber;

        [XmlElement("FlightRefNumber")]
        public FlightRefNumber[] FlightRefNumber;

        public string Text;

        [XmlAttribute()]
        public SpecialRemarkRemarkType RemarkType;
    }

    public enum SpecialRemarkRemarkType
    {

        Air,

        Car,

        Cryptic,

        Hotel,

        Invoice,

        Endorsement,

        TourCode,

        ValidatingCarrier,

        ManualDocument,

        AutomatedTicket,

        ElectronicTicket,

        ManualTicket,

        A,

        B,

        C,

        D,

        E,

        F,

        G,

        H,

        I,

        J,

        K,

        L,

        M,

        N,

        O,

        P,

        Q,

        R,

        S,

        T,

        U,

        V,

        W,

        X,

        Y,

        Z

    }

    [XmlRoot(IsNullable = false)]
    public class CategorizedRemark
    {

        [XmlAttribute()]
        public CategorizedRemarkType Category;

        [XmlText()]
        public string Value;

    }
    public enum CategorizedRemarkType
    {
        A,

        B,

        C,

        D,

        E,

        F,

        G,

        H,

        I,

        J,

        K,

        L,

        M,

        N,

        O,

        P,

        Q,

        R,

        S,

        T,

        U,

        V,

        W,

        X,

        Y,

        Z
    }

    [XmlRoot(IsNullable = false)]
    public class Queue
    {

        [XmlAttribute()]
        public string PseudoCityCode;

        [XmlAttribute()]
        public string QueueNumber;

        [XmlAttribute()]
        public string QueueCategory;
    }

    public enum OTA_AirBookRQTarget
    {

        Test,

        Production
    }

    public enum OTA_AirBookRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_HotelResRQ
    {

        public UniqueID UniqueId;

        [XmlArrayItem(IsNullable = false)]
        public HotelReservation[] HotelReservations;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_HotelResRQTarget.Production)]
        public OTA_HotelResRQTarget Target = OTA_HotelResRQTarget.Production;

        [XmlAttribute()]
        public string Version;

        [XmlAttribute()]
        public int SequenceNmbr;

        [XmlIgnore()]
        public bool SequenceNmbrSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public OTA_HotelResRQResStatus ResStatus;

        [XmlIgnore()]
        public bool ResStatusSpecified;
    }

    public enum OTA_HotelResRQTarget
    {

        Test,

        Production
    }

    public enum OTA_HotelResRQResStatus
    {

        Book,

        Quote,

        Hold,

        Initiate,

        Ignore,

        Modify,

        Commit,

        OnRequest
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_TravelItineraryRQ
    {

        public POS POS;

        public UniqueID UniqueId;

        public OTA_AirBookRQ OTA_AirBookRQ;

        public OTA_HotelResRQ OTA_HotelResRQ;

        public OTA_VehResRQ OTA_VehResRQ;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_TravelItineraryRQTarget.Production)]
        public OTA_TravelItineraryRQTarget Target = OTA_TravelItineraryRQTarget.Production;

        [XmlAttribute()]
        public string Version;

        [XmlAttribute()]
        public int SequenceNmbr;

        [XmlIgnore()]
        public bool SequenceNmbrSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public OTA_TravelItineraryRQResStatus ResStatus;

        [XmlIgnore()]
        public bool ResStatusSpecified;

        [XmlAttribute()]
        public bool ReferenceOnly;

        [XmlIgnore()]
        public bool ReferenceOnlySpecified;
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
    public class OTA_VehResRQ
    {

        public VehResRQCore VehResRQCore;

        public VehResRQInfo VehResRQInfo;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_VehResRQTarget.Production)]
        public OTA_VehResRQTarget Target = OTA_VehResRQTarget.Production;

        [XmlAttribute()]
        public string Version;

        [XmlAttribute()]
        public int SequenceNmbr;

        [XmlIgnore()]
        public bool SequenceNmbrSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public string ReqRespVersion;
    }

    [XmlRoot(IsNullable = false)]
    public class VehResRQCore
    {

        public VehRentalCore VehRentalCore;

        public Customer Customer;

        public VendorPref VendorPref;

        public VehPref VehPref;

        [XmlElement("DriverType")]
        public DriverType[] DriverType;

        public RateQualifier RateQualifier;

        [XmlArrayItem(IsNullable = false)]
        public SpecialEquipPref[] SpecialEquipPrefs;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public VehResRQCoreStatus Status;
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
        [System.ComponentModel.DefaultValue(VendorPrefPreferLevel.Preferred)]
        public VendorPrefPreferLevel PreferLevel = VendorPrefPreferLevel.Preferred;

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
    public class VehPref
    {

        public VehType VehType;

        public VehClass VehClass;

        [XmlAttribute()]
        public bool AirConditionInd;

        [XmlIgnore()]
        public bool AirConditionIndSpecified;

        [XmlAttribute()]
        public VehPrefTransmissionType TransmissionType;

        [XmlIgnore()]
        public bool TransmissionTypeSpecified;

        [XmlAttribute()]
        public VehPrefTypePref TypePref;

        [XmlIgnore()]
        public bool TypePrefSpecified;

        [XmlAttribute()]
        public VehPrefClassPref ClassPref;

        [XmlIgnore()]
        public bool ClassPrefSpecified;

        [XmlAttribute()]
        public VehPrefAirConditionPref AirConditionPref;

        [XmlIgnore()]
        public bool AirConditionPrefSpecified;

        [XmlAttribute()]
        public VehPrefTransmissionPref TransmissionPref;

        [XmlIgnore()]
        public bool TransmissionPrefSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class VehType
    {

        [XmlAttribute()]
        public string VehicleCategory;

        [XmlAttribute()]
        public int DoorCount;

        [XmlIgnore()]
        public bool DoorCountSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class VehClass
    {

        [XmlAttribute()]
        public string Size;
    }

    public enum VehPrefTransmissionType
    {

        Automatic,

        Manual
    }

    public enum VehPrefTypePref
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum VehPrefClassPref
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum VehPrefAirConditionPref
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum VehPrefTransmissionPref
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class RateQualifier
    {

        [XmlAttribute()]
        public string TravelPurpose;

        [XmlAttribute()]
        public string RateCategory;

        [XmlAttribute()]
        public string CorpDiscountNmbr;

        [XmlAttribute()]
        public string PromotionCode;

        [XmlAttribute("RateQualifier")]
        public string RateQualifier1;

        [XmlAttribute()]
        public RateQualifierRatePeriod RatePeriod;

        [XmlIgnore()]
        public bool RatePeriodSpecified;

    }

    [XmlRoot(IsNullable = false)]
    public class SpecialEquipPref
    {

        [XmlAttribute()]
        public string EquipType;

        [XmlAttribute()]
        public int Quantity;

        [XmlIgnore()]
        public bool QuantitySpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SpecialEquipPrefPreferLevel.Preferred)]
        public SpecialEquipPrefPreferLevel PreferLevel = SpecialEquipPrefPreferLevel.Preferred;
    }

    public enum SpecialEquipPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum VehResRQCoreStatus
    {

        Available,

        Unavailable,

        OnRequest,

        Confirmed,

        All
    }

    [XmlRoot(IsNullable = false)]
    public class VehResRQInfo
    {

        [XmlElement("SpecialReqPref")]
        public SpecialReqPref[] SpecialReqPref;

        [XmlArrayItem(IsNullable = false)]
        public CoveragePref[] CoveragePrefs;

        [XmlElement("OffLocService")]
        public OffLocService[] OffLocService;

        public ArrivalDetails ArrivalDetails;

        public RentalPaymentPref RentalPaymentPref;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public int LuggageQty;

        [XmlIgnore()]
        public bool LuggageQtySpecified;

        [XmlAttribute()]
        public int PassengerQty;

        [XmlIgnore()]
        public bool PassengerQtySpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool SmokingAllowed = false;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool GasPrePay = false;
    }

    [XmlRoot(IsNullable = false)]
    public class SpecialReqPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SpecialReqPrefPreferLevel.Preferred)]
        public SpecialReqPrefPreferLevel PreferLevel = SpecialReqPrefPreferLevel.Preferred;

        [XmlText()]
        public string Value;
    }

    public enum SpecialReqPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class OffLocService
    {

        public Address Address;

        public PersonName PersonName;

        public Telephone Telephone;

        [XmlAttribute()]
        public OffLocServiceType Type;

        [XmlAttribute()]
        public string SpecInstructions;
    }

    public enum OffLocServiceType
    {

        CustPickUp,

        VehDelivery,

        CustDropOff,

        VehCollection
    }

    [XmlRoot(IsNullable = false)]
    public class RentalPaymentPref
    {

        public PaymentCard PaymentCard;

        public BankAcct BankAcct;

        public DirectBill DirectBill;

        public Voucher Voucher;

        [XmlAttribute()]
        public RentalPaymentPrefShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public RentalPaymentPrefShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CostCenterId;

        [XmlAttribute()]
        public string RPH;
    }

    public enum RentalPaymentPrefShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum RentalPaymentPrefShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum OTA_VehResRQTarget
    {

        Test,

        Production
    }

    public enum OTA_TravelItineraryRQTarget
    {

        Test,

        Production
    }

    public enum OTA_TravelItineraryRQResStatus
    {

        Book,

        Quote,

        Hold,

        Initiate,

        Ignore,

        Modify,

        Commit
    }

    [XmlRoot(IsNullable = false)]
    public class OriginDestinationOption
    {

        [XmlElement("FlightSegment")]
        public FlightSegment[] FlightSegment;
    }

    [XmlRoot(IsNullable = false)]
    public class OriginDestinationOptions
    {

        [XmlArrayItem(typeof(FlightSegment), IsNullable = false)]
        public FlightSegment[][] OriginDestinationOption;
    }

    [XmlRoot(IsNullable = false)]
    public class PTC_FareBreakdowns
    {

        [XmlElement("PTC_FareBreakdown")]
        public PTC_FareBreakdown[] PTC_FareBreakdown;
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentDetails
    {

        [XmlElement("PaymentDetail")]
        public PaymentDetail[] PaymentDetail;
    }

    [XmlRoot(IsNullable = false)]
    public class Remarks
    {

        [XmlElement("Remark")]
        public string[] Remark;
    }

    [XmlRoot(IsNullable = false)]
    public class ResGuestRPHs
    {

        [XmlElement("ResGuestRPH")]
        public ResGuestRPH[] ResGuestRPH;
    }

    [XmlRoot(IsNullable = false)]
    public class RoomRates
    {

        [XmlElement("RoomRate")]
        public RoomRate[] RoomRate;
    }

    [XmlRoot(IsNullable = false)]
    public class RoomStays
    {

        [XmlElement("RoomStay")]
        public RoomStay[] RoomStay;
    }

    [XmlRoot(IsNullable = false)]
    public class RoomTypes
    {

        [XmlElement("RoomType")]
        public RoomType[] RoomType;
    }

    [XmlRoot(IsNullable = false)]
    public class ServiceRPHs
    {

        [XmlElement("ServiceRPH")]
        public ServiceRPH[] ServiceRPH;
    }

    [XmlRoot(IsNullable = false)]
    public class SpecialEquipPrefs
    {

        [XmlElement("SpecialEquipPref")]
        public SpecialEquipPref[] SpecialEquipPref;
    }

    [XmlRoot(IsNullable = false)]
    public class SpecialRemarks
    {

        [XmlElement("SpecialRemark")]
        public SpecialRemark[] SpecialRemark;
    }

    [XmlRoot(IsNullable = false)]
    public class CategorizedRemarks
    {

        [XmlElement("CategorizedRemark")]
        public CategorizedRemark[] CategorizedRemark;
    }

    [XmlRoot(IsNullable = false)]
    public class SpecialRequests
    {

        [XmlElement("SpecialRequest")]
        public SpecialRequest[] SpecialRequest;
    }

    [XmlRoot(IsNullable = false)]
    public class Taxes
    {

        [XmlElement("Tax")]
        public Tax[] Tax;
    }

}
