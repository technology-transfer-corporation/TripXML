using System.Xml.Serialization;
using wsTripXML.wsTravelTalk.wmCruiseBooking;
using wsTripXML.wsTravelTalk.wmCruiseCreateBooking;

namespace wsTripXML.wsTravelTalk.wmCruiseCreateBookingIn
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
        public string DefaultInd;

        [XmlAttribute()]
        public string UseType;
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

    [XmlRoot(IsNullable = false)]
    public class AgentInfo
    {

        [XmlAttribute()]
        public string Contact;

        [XmlAttribute()]
        public string ContactID;
    }

    [XmlRoot(IsNullable = false)]
    public class AirDeviationRequest
    {

        public DepartureCity DepartureCity;

        public ArrivalCity ArrivalCity;

        public Airline Airline;

        [XmlAttribute()]
        public string DepartureDateTime;

        [XmlAttribute()]
        public string ArrivalDateTime;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string AirlineCabinClass;

        [XmlAttribute()]
        public string Comment;
    }

    [XmlRoot(IsNullable = false)]
    public class DepartureCity
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        public string CodeContext;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class ArrivalCity
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        public string CodeContext;

        [XmlText()]
        public string Value;
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

        [XmlAttribute()]
        public AirlineFlightNumber FlightNumber;

        [XmlIgnore()]
        public bool FlightNumberSpecified;

        [XmlText()]
        public string Value;
    }

    public enum AirlineFlightNumber
    {

        OPEN,

        ARNK
    }

    [XmlRoot(IsNullable = false)]
    public class AirDeviationRequests
    {

        [XmlElement("AirDeviationRequest")]
        public AirDeviationRequest[] AirDeviationRequest;
    }

    [XmlRoot(IsNullable = false)]
    public class AirInfo
    {

        public DepartureCity DepartureCity;

        public ArrivalCity ArrivalCity;

        public Airline Airline;

        [XmlAttribute()]
        public string DepartureDateTime;

        [XmlAttribute()]
        public string ArrivalDateTime;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string AirlineCabinClass;
    }

    [XmlRoot(IsNullable = false)]
    public class BankAcct
    {

        public string BankAcctName;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string BankID;

        [XmlAttribute()]
        public string AcctType;

        [XmlAttribute()]
        public string BankAcctNumber;
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
    public class CardIssuerName
    {

        [XmlAttribute()]
        public string BankID;
    }

    [XmlRoot(IsNullable = false)]
    public class Cash
    {

        [XmlAttribute()]
        public string CashIndicator;
    }

    [XmlRoot(IsNullable = false)]
    public class Comment
    {

        [XmlElement("Text")]
        public Text[] Text;

        [XmlElement("Image")]
        public string[] Image;

        [XmlElement("URL")]
        public URL[] URL;

        [XmlElement("ListItem")]
        public wmCruiseBooking.ListItem[] ListItem;

        [XmlAttribute()]
        public string Name;

        [XmlAttribute()]
        public string ParagraphNumber;

        [XmlAttribute()]
        public string CreateDateTime;

        [XmlAttribute()]
        public string CreatorID;

        [XmlAttribute()]
        public string LastModifyDateTime;

        [XmlAttribute()]
        public string LastModifierID;

        [XmlAttribute()]
        public string Language;
    }

    [XmlRoot(IsNullable = false)]
    public class URL
    {

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public string DefaultInd;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class ContactInfo
    {

        public PersonName PersonName;

        [XmlElement("Telephone")]
        public Telephone[] Telephone;

        [XmlElement("Address")]
        public Address[] Address;

        [XmlElement("Email")]
        public Email[] Email;

        [XmlElement("URL")]
        public URL[] URL;

        [XmlElement("CompanyNameFull")]
        public Code.CompanyName[] CompanyName;

        [XmlElement("EmployeeInfo")]
        public EmployeeInfo[] EmployeeInfo;

        [XmlElement("GuestTransportation")]
        public GuestTransportation[] Items;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string DefaultInd;

        [XmlAttribute()]
        public string ContactType;

        [XmlAttribute()]
        public string Relation;

        [XmlAttribute()]
        public string EmergencyFlag;

        [XmlAttribute()]
        public string RPH;

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

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class EmployeeInfo
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

    [XmlRoot(IsNullable = false)]
    public class DeletedGuest
    {

        [XmlAttribute()]
        public string DeletedGuestReference;
    }

    [XmlRoot(IsNullable = false)]
    public class DirectBill
    {

        public Code.CompanyName CompanyName;

        public Address Address;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string DirectBill_ID;
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

    [XmlRoot(Namespace = "", IsNullable = false)]
    public class FaxDocument
    {

        [XmlAttribute()]
        public string Code;
    }

    [XmlRoot(IsNullable = false)]
    public class GuestDetail
    {

        [XmlElement("SelectedFareCode")]
        public SelectedFareCode[] SelectedFareCode;

        [XmlElement("ContactInfo")]
        public ContactInfo[] ContactInfo;

        [XmlElement("LoyaltyInfo")]
        public LoyaltyInfo[] LoyaltyInfo;

        [XmlElement("LinkedTraveler")]
        public LinkedTraveler[] LinkedTraveler;

        [XmlElement("TravelDocument")]
        public TravelDocument[] TravelDocument;

        [XmlElement("SelectedDining")]
        public SelectedDining[] SelectedDining;

        public SelectedInsurance SelectedInsurance;

        [XmlElement("SelectedOptions")]
        public SelectedOptions[] SelectedOptions;

        [XmlArrayItem(IsNullable = false)]
        public SelectedPackage[] SelectedPackages;

        [XmlArrayItem(IsNullable = false)]
        public SelectedSpecialService[] SelectedSpecialServices;

        [XmlArrayItem(IsNullable = false)]
        public AirDeviationRequest[] AirDeviationRequests;

        [XmlElement("FaxDocument")]
        public FaxDocument[] FaxDocument;

        [XmlAttribute()]
        public string GuestExistsIndicator;
    }

    [XmlRoot(IsNullable = false)]
    public class SelectedFareCode
    {

        [XmlAttribute()]
        public string FareCode;

        [XmlAttribute()]
        public string GroupCode;
    }

    [XmlRoot(IsNullable = false)]
    public class LoyaltyInfo
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
    }

    [XmlRoot(IsNullable = false)]
    public class LinkedTraveler
    {

        public UniqueID UniqueID;

        public PersonName PersonName;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string Relation;
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

    [XmlRoot(IsNullable = false)]
    public class TravelDocument
    {

        public string DocHolderName;

        public DocHolderFormattedName DocHolderFormattedName;

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
    }

    [XmlRoot(IsNullable = false)]
    public class SelectedDining
    {

        [XmlAttribute()]
        public string SmokingCode;

        [XmlAttribute()]
        public string DiningRoom;

        [XmlAttribute()]
        public string TableSize;

        [XmlAttribute()]
        public string AgeCode;

        [XmlAttribute()]
        public string Language;

        [XmlAttribute()]
        public string Sitting;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string Status;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string Preference;
    }

    [XmlRoot(IsNullable = false)]
    public class SelectedInsurance
    {

        [XmlAttribute()]
        public string InsuranceCode;

        [XmlAttribute()]
        public string SelectedOptionIndicator;

        [XmlAttribute()]
        public string DefaultIndicator;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string Status;
    }

    [XmlRoot(IsNullable = false)]
    public class SelectedOptions
    {

        public Originator Originator;

        public Message Message;

        [XmlAttribute()]
        public string OptionCode;

        [XmlAttribute()]
        public string Quantity;

        [XmlAttribute()]
        public string DeliveryDate;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string DeliveryLocation;
    }

    [XmlRoot(IsNullable = false)]
    public class Originator
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
    public class Message
    {

        [XmlElement("Text")]
        public Text[] Text;

        [XmlElement("Image")]
        public string[] Image;

        [XmlElement("URL")]
        public URL[] URL;

        [XmlElement("ListItem")]
        public wmCruiseBooking.ListItem[] ListItem;

        [XmlAttribute()]
        public string Name;

        [XmlAttribute()]
        public string ParagraphNumber;

        [XmlAttribute()]
        public string CreateDateTime;

        [XmlAttribute()]
        public string CreatorID;

        [XmlAttribute()]
        public string LastModifyDateTime;

        [XmlAttribute()]
        public string LastModifierID;

        [XmlAttribute()]
        public string Language;
    }

    [XmlRoot(IsNullable = false)]
    public class SelectedPackage
    {

        public Location Location;

        public AirInfo AirInfo;

        [XmlAttribute()]
        public string PackageTypeCode;

        [XmlAttribute()]
        public string CruisePackageCode;

        [XmlAttribute()]
        public string InclusiveIndicator;

        [XmlAttribute()]
        public string StartDate;

        [XmlAttribute()]
        public string Status;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute()]
        public string HotelRoomRPH;
    }

    [XmlRoot(IsNullable = false)]
    public class Location
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute()]
        public string LocationName;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class SelectedSpecialService
    {

        public Comment Comment;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string Type;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeName;

        [XmlAttribute()]
        public string CodeDetail;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string AssociationType;

        [XmlAttribute()]
        public string Date;
    }

    [XmlRoot(IsNullable = false)]
    public class GuestDetails
    {

        [XmlElement("GuestDetail")]
        public GuestDetail[] GuestDetail;
    }

    [XmlRoot(IsNullable = false)]
    public class ReservationInfo
    {

        [XmlElement("ReservationID")]
        public ReservationID[] ReservationID;

        [XmlArrayItem(IsNullable = false)]
        public GuestDetail[] GuestDetails;

        [XmlArrayItem(IsNullable = false)]
        public LinkedBooking[] LinkedBookings;

        [XmlArrayItem(IsNullable = false)]
        public PaymentRequest[] PaymentRequests;
    }

    [XmlRoot(IsNullable = false)]
    public class LinkedBooking
    {

        public UniqueID UniqueID;

        public PersonName PersonName;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string Relation;

        [XmlAttribute()]
        public string LinkTypeCode;
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentRequest
    {

        public PaymentCard PaymentCard;

        public BankAcct BankAcct;

        public DirectBill DirectBill;

        public Voucher Voucher;

        public LoyaltyRedemption LoyaltyRedemption;

        public MiscChargeOrder MiscChargeOrder;

        public Cash Cash;

        public PaymentAmount PaymentAmount;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string CostCenterID;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PaymentTransactionTypeCode;

        [XmlAttribute()]
        public string ExtendedIndicator;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string PaymentPurpose;

        [XmlAttribute()]
        public string ExtendedDepositDate;

        [XmlAttribute()]
        public string ReferenceNumber;
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentCard
    {

        public string CardHolderName;

        public CardIssuerName CardIssuerName;

        public Address Address;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string CardType;

        [XmlAttribute()]
        public string CardCode;

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

        [XmlAttribute()]
        public string MaskedCardNumber;

        [XmlAttribute()]
        public string CardHolderRPH;
    }

    [XmlRoot(IsNullable = false)]
    public class Voucher
    {

        [XmlAttribute()]
        public string EffectiveDate;

        [XmlAttribute()]
        public string ExpireDate;

        [XmlAttribute()]
        public string SeriesCode;
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
        public string RedemptionQuantity;
    }

    [XmlRoot(IsNullable = false)]
    public class MiscChargeOrder
    {

        [XmlAttribute()]
        public string TicketNumber;
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentAmount
    {

        [XmlAttribute()]
        public string Amount;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public string DecimalPlaces;

        [XmlAttribute()]
        public string ApprovalCode;
    }

    [XmlRoot(IsNullable = false)]
    public class LinkedBookings
    {

        [XmlElement("LinkedBooking")]
        public LinkedBooking[] LinkedBooking;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_CruiseCreateBookingRQ
    {

        public POS POS;

        public AgentInfo AgentInfo;

        [XmlElement("DeletedGuest")]
        public DeletedGuest[] DeletedGuest;

        public SailingInfo SailingInfo;

        public ReservationInfo ReservationInfo;

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
        public OTA_CruiseCreateBookingRQTransactionActionCode TransactionActionCode;

        [XmlIgnore()]
        public bool TransactionActionCodeSpecified;
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

        [XmlAttribute()]
        public string AltitudeUnitOfMeasure;
    }

    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        public string Name;

        public string System;

        public string Userid;

        public string Password;
    }

    public enum OTA_CruiseCreateBookingRQTransactionActionCode
    {

        Book,

        Quote,

        Hold,

        Initiate,

        Ignore,

        Modify,

        Commit,

        Cancel
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentRequests
    {

        [XmlElement("PaymentRequest")]
        public PaymentRequest[] PaymentRequest;
    }

    [XmlRoot(IsNullable = false)]
    public class SelectedPackages
    {

        [XmlElement("SelectedPackage")]
        public SelectedPackage[] SelectedPackage;
    }

    [XmlRoot(IsNullable = false)]
    public class SelectedSpecialServices
    {

        [XmlElement("SelectedSpecialService")]
        public SelectedSpecialService[] SelectedSpecialService;
    }

    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        [XmlElement("Provider")]
        public Provider[] Provider;
    }

}
