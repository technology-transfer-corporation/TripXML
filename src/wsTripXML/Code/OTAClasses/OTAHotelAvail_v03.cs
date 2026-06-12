using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmHotelAvail_v03
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions
    {

        // <remarks/>
        public Provider Provider;

        // <remarks/>
        public string MultiRate;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        // <remarks/>
        [XmlElement("Name")]
        public Name[] Name;

        // <remarks/>
        [XmlElement("System")]
        public string GDSSystem;

        // <remarks/>
        public string Userid;

        // <remarks/>
        public string Password;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Name
    {

        // <remarks/>
        [XmlAttribute()]
        public string PseudoCityCode;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Access
    {

        // <remarks/>
        public AccessPerson AccessPerson;

        // <remarks/>
        public AccessComment AccessComment;

        // <remarks/>
        [XmlAttribute()]
        public AccessActionType ActionType;

        // <remarks/>
        [XmlIgnore()]
        public bool ActionTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public DateTime ActionDateTime;

        // <remarks/>
        [XmlIgnore()]
        public bool ActionDateTimeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string ID;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AccessPerson
    {

        // <remarks/>
        [XmlElement("NamePrefix")]
        public string[] NamePrefix;

        // <remarks/>
        [XmlElement("GivenName")]
        public string[] GivenName;

        // <remarks/>
        [XmlElement("MiddleName")]
        public string[] MiddleName;

        // <remarks/>
        public string SurnamePrefix;

        // <remarks/>
        public string Surname;

        // <remarks/>
        [XmlElement("NameSuffix")]
        public string[] NameSuffix;

        // <remarks/>
        [XmlElement("NameTitle")]
        public string[] NameTitle;

        // <remarks/>
        [XmlAttribute()]
        public AccessPersonShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public AccessPersonShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string NameType;
    }

    // <remarks/>
    public enum AccessPersonShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AccessPersonShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AccessComment
    {

        // <remarks/>
        [XmlAttribute()]
        public string Language;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum AccessActionType
    {

        // <remarks/>
        Create,

        // <remarks/>
        Read,

        // <remarks/>
        Update,

        // <remarks/>
        Delete
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Accesses
    {

        // <remarks/>
        [XmlElement("Access")]
        public Access[] Access;

        // <remarks/>
        [XmlAttribute()]
        public AccessesShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public AccessesShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public DateTime CreateDateTime;

        // <remarks/>
        [XmlIgnore()]
        public bool CreateDateTimeSpecified;
    }

    // <remarks/>
    public enum AccessesShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AccessesShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Affiliations
    {

        // <remarks/>
        [XmlElement("Organization")]
        public Organization[] Organization;

        // <remarks/>
        [XmlElement("Employer")]
        public Employer[] Employer;

        // <remarks/>
        [XmlElement("TravelArranger")]
        public TravelArranger[] TravelArranger;

        // <remarks/>
        [XmlElement("TravelClub")]
        public TravelClub[] TravelClub;

        // <remarks/>
        [XmlElement("Insurance")]
        public Insurance[] Insurance;

        // <remarks/>
        [XmlAttribute()]
        public AffiliationsShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public AffiliationsShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Agreements
    {

        // <remarks/>
        [XmlElement("Certification")]
        public Certification[] Certification;

        // <remarks/>
        [XmlElement("AllianceConsortium")]
        public AllianceConsortium[] AllianceConsortium;

        // <remarks/>
        [XmlElement("CommissionInfo")]
        public CommissionInfo[] CommissionInfo;

        // <remarks/>
        [XmlAttribute()]
        public AgreementsShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public AgreementsShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AirlinePref
    {

        // <remarks/>
        [XmlElement("LoyaltyPref")]
        public LoyaltyPref[] LoyaltyPref;

        // <remarks/>
        [XmlElement("VendorPref")]
        public VendorPref[] VendorPref;

        // <remarks/>
        [XmlElement("PaymentFormPref")]
        public PaymentFormPref[] PaymentFormPref;

        // <remarks/>
        [XmlElement("AirportOriginPref")]
        public AirportOriginPref[] AirportOriginPref;

        // <remarks/>
        [XmlElement("AirportRoutePref")]
        public AirportRoutePref[] AirportRoutePref;

        // <remarks/>
        [XmlElement("FareRestrictPref")]
        public FareRestrictPref[] FareRestrictPref;

        // <remarks/>
        [XmlElement("FlightTypePref")]
        public FlightTypePref[] FlightTypePref;

        // <remarks/>
        [XmlElement("EquipPref")]
        public EquipPref[] EquipPref;

        // <remarks/>
        [XmlElement("CabinPref")]
        public CabinPref[] CabinPref;

        // <remarks/>
        [XmlElement("SeatPref")]
        public SeatPref[] SeatPref;

        // <remarks/>
        [XmlElement("TicketDistribPref")]
        public TicketDistribPref[] TicketDistribPref;

        // <remarks/>
        [XmlElement("MealPref")]
        public MealPref[] MealPref;

        // <remarks/>
        [XmlElement("SpecRequestPref")]
        public SpecRequestPref[] SpecRequestPref;

        // <remarks/>
        [XmlElement("SSR_Pref")]
        public SSR_Pref[] SSR_Pref;

        // <remarks/>
        [XmlElement("MediaEntertainPref")]
        public MediaEntertainPref[] MediaEntertainPref;

        // <remarks/>
        [XmlElement("PetInfoPref")]
        public PetInfoPref[] PetInfoPref;

        // <remarks/>
        [XmlAttribute()]
        public AirlinePrefShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public AirlinePrefShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(AirlinePrefPreferLevel.Preferred)]
        public AirlinePrefPreferLevel PreferLevel = AirlinePrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool SmokingAllowed = false;

        // <remarks/>
        [XmlAttribute()]
        public string PassengerTypeCode;

        // <remarks/>
        [XmlAttribute()]
        public AirlinePrefAirTicketType AirTicketType;

        // <remarks/>
        [XmlIgnore()]
        public bool AirTicketTypeSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CommonPref
    {

        // <remarks/>
        [XmlElement("NamePref")]
        public NamePref[] NamePref;

        // <remarks/>
        [XmlElement("PhonePref")]
        public PhonePref[] PhonePref;

        // <remarks/>
        [XmlElement("AddressPref")]
        public AddressPref[] AddressPref;

        // <remarks/>
        [XmlElement("PaymentFormPref")]
        public PaymentFormPref[] PaymentFormPref;

        // <remarks/>
        [XmlElement("InterestPref")]
        public InterestPref[] InterestPref;

        // <remarks/>
        [XmlElement("InsurancePref")]
        public InsurancePref[] InsurancePref;

        // <remarks/>
        [XmlElement("SeatingPref")]
        public SeatingPref[] SeatingPref;

        // <remarks/>
        [XmlElement("TicketDistribPref")]
        public TicketDistribPref[] TicketDistribPref;

        // <remarks/>
        [XmlElement("MediaEntertainPref")]
        public MediaEntertainPref[] MediaEntertainPref;

        // <remarks/>
        [XmlElement("PetInfoPref")]
        public PetInfoPref[] PetInfoPref;

        // <remarks/>
        [XmlElement("MealPref")]
        public MealPref[] MealPref;

        // <remarks/>
        [XmlElement("LoyaltyPref")]
        public LoyaltyPref[] LoyaltyPref;

        // <remarks/>
        [XmlElement("SpecRequestPref")]
        public SpecRequestPref[] SpecRequestPref;

        // <remarks/>
        [XmlElement("RelatedTravelerPref")]
        public RelatedTravelerPref[] RelatedTravelerPref;

        // <remarks/>
        [XmlAttribute()]
        public CommonPrefShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommonPrefShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool SmokingAllowed = false;

        // <remarks/>
        [XmlAttribute()]
        public string PrimaryLangID;

        // <remarks/>
        [XmlAttribute()]
        public string AltLangID;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Customer
    {

        // <remarks/>
        public PersonName PersonName;

        // <remarks/>
        [XmlElement("Telephone")]
        public Telephone[] Telephone;

        // <remarks/>
        [XmlElement("Email")]
        public Email[] Email;

        // <remarks/>
        [XmlElement("Address")]
        public Address[] Address;

        // <remarks/>
        [XmlElement("URL")]
        public URL[] URL;

        // <remarks/>
        [XmlElement("CitizenCountryName")]
        public CitizenCountryName[] CitizenCountryName;

        // <remarks/>
        [XmlElement("PhysChallName")]
        public string[] PhysChallName;

        // <remarks/>
        [XmlElement("PetInfo")]
        public string[] PetInfo;

        // <remarks/>
        [XmlElement("PaymentForm")]
        public PaymentForm[] PaymentForm;

        // <remarks/>
        [XmlElement("RelatedTraveler")]
        public RelatedTraveler[] RelatedTraveler;

        // <remarks/>
        [XmlElement("ContactPerson")]
        public ContactPerson[] ContactPerson;

        // <remarks/>
        [XmlElement("Document")]
        public Document[] Document;

        // <remarks/>
        [XmlElement("CustLoyalty")]
        public CustLoyalty[] CustLoyalty;

        // <remarks/>
        [XmlElement("EmployeeInfo")]
        public EmployeeInfo[] EmployeeInfo;

        // <remarks/>
        [XmlAttribute()]
        public CustomerGender Gender;

        // <remarks/>
        [XmlIgnore()]
        public bool GenderSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool Deceased;

        // <remarks/>
        [XmlIgnore()]
        public bool DeceasedSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string LockoutType;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate;

        // <remarks/>
        [XmlIgnore()]
        public bool BirthDateSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute()]
        public int DecimalPlaces;

        // <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PersonName
    {

        // <remarks/>
        [XmlElement("NamePrefix")]
        public string[] NamePrefix;

        // <remarks/>
        [XmlElement("GivenName")]
        public string[] GivenName;

        // <remarks/>
        [XmlElement("MiddleName")]
        public string[] MiddleName;

        // <remarks/>
        public string SurnamePrefix;

        // <remarks/>
        public string Surname;

        // <remarks/>
        [XmlElement("NameSuffix")]
        public string[] NameSuffix;

        // <remarks/>
        [XmlElement("NameTitle")]
        public string[] NameTitle;

        // <remarks/>
        [XmlAttribute()]
        public PersonNameShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public PersonNameShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string NameType;
    }

    // <remarks/>
    public enum PersonNameShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum PersonNameShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Telephone
    {

        // <remarks/>
        [XmlAttribute()]
        public TelephoneShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TelephoneShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneLocationType;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneTechType;

        // <remarks/>
        [XmlAttribute()]
        public string CountryAccessCode;

        // <remarks/>
        [XmlAttribute()]
        public string AreaCityCode;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneNumber;

        // <remarks/>
        [XmlAttribute()]
        public string Extension;

        // <remarks/>
        [XmlAttribute()]
        public string PIN;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;
    }

    // <remarks/>
    public enum TelephoneShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum TelephoneShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Email
    {

        // <remarks/>
        [XmlAttribute()]
        public EmailShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public EmailShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        // <remarks/>
        [XmlAttribute()]
        public string EmailType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum EmailShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum EmailShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class URL
    {

        // <remarks/>
        [XmlAttribute()]
        public URLShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public URLShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        // <remarks/>
        [XmlText(DataType = "anyURI")]
        public string Value;
    }

    // <remarks/>
    public enum URLShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum URLShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class UniqueID
    {

        // <remarks/>
        public Code.CompanyName CompanyName;

        // <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string URL;

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        public string Instance;

        // <remarks/>
        [XmlAttribute()]
        public string ID;

        // <remarks/>
        [XmlAttribute()]
        public string ID_Context;
    }

    // local CompanyName removed: identical to shared wsTripXML.Code.CompanyName (XML type-name collision in one serializer scope)

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CitizenCountryName
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        // <remarks/>
        [XmlAttribute()]
        public string Code;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PaymentForm
    {

        // <remarks/>
        public PaymentCard PaymentCard;

        // <remarks/>
        public BankAcct BankAcct;

        // <remarks/>
        public DirectBill DirectBill;

        // <remarks/>
        public Voucher Voucher;

        // <remarks/>
        public LoyaltyRedemption LoyaltyRedemption;

        // <remarks/>
        public MiscChargeOrder MiscChargeOrder;

        // <remarks/>
        [XmlAttribute()]
        public PaymentFormShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public PaymentFormShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string CostCenterID;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PaymentCard
    {

        // <remarks/>
        public string CardHolderName;

        // <remarks/>
        public CardIssuerName CardIssuerName;

        // <remarks/>
        public Address Address;

        // <remarks/>
        [XmlAttribute()]
        public PaymentCardShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public PaymentCardShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string CardType;

        // <remarks/>
        [XmlAttribute()]
        public PaymentCardCardCode CardCode;

        // <remarks/>
        [XmlIgnore()]
        public bool CardCodeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string CardNumber;

        // <remarks/>
        [XmlAttribute()]
        public string SeriesCode;

        // <remarks/>
        [XmlAttribute()]
        public string EffectiveDate;

        // <remarks/>
        [XmlAttribute()]
        public string ExpireDate;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CardIssuerName
    {

        // <remarks/>
        [XmlAttribute()]
        public string BankID;
    }

    // <remarks/>
    public enum PaymentCardShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum PaymentCardShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum PaymentCardCardCode
    {

        // <remarks/> Carte Aurore
        AU,

        // <remarks/> Amex
        AX,

        // <remarks/>
        BC,

        // <remarks/>
        BL,

        // <remarks/> Carte Blanche
        CB,

        // <remarks/> Cofinoga
        CG,

        // <remarks/> Connect
        CN,

        // <remarks/> Choice
        CX,

        // <remarks/> Diners (DC)
        DN,

        // <remarks/>
        DK,

        // <remarks/> Discover
        DS,

        // <remarks/>
        EC,

        // <remarks/> Lufthansa GK Card
        GK,

        // <remarks/> JCB
        JC,

        // <remarks/> Mastercard (CA)
        MC,

        // <remarks/> Mastercard Debit
        MD,

        // <remarks/> Mastercard Maestro
        MO,

        // <remarks/> Mastercard Prepaid
        MP,

        // <remarks/> Solo
        SO,

        // <remarks/> Switch
        SW,

        // <remarks/> Torch Club
        TC,

        // <remarks/> UATP
        TP,

        // <remarks/> Visa
        VI,

        // <remarks/> Visa Debit
        VD,

        // <remarks/> Visa Electron
        VE,

        // <remarks/> ??
        VS,

        // <remarks/> Visa Delta
        VT,

        // <remarks/> ??
        WB,

        // <remarks/> Access
        XS
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class BankAcct
    {

        // <remarks/>
        public string BankAcctName;

        // <remarks/>
        [XmlAttribute()]
        public BankAcctShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public BankAcctShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string BankID;

        // <remarks/>
        [XmlAttribute()]
        public string AcctType;

        // <remarks/>
        [XmlAttribute()]
        public string BankAcctNumber;
    }

    // <remarks/>
    public enum BankAcctShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum BankAcctShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class DirectBill
    {

        // <remarks/>
        public Code.CompanyName CompanyName;

        // <remarks/>
        public Address Address;

        // <remarks/>
        [XmlAttribute()]
        public DirectBillShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public DirectBillShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string DirectBill_ID;
    }

    // <remarks/>
    public enum DirectBillShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum DirectBillShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Voucher
    {

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        // <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string SeriesCode;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class LoyaltyRedemption
    {

        // <remarks/>
        [XmlAttribute()]
        public string CertificateNumber;

        // <remarks/>
        [XmlAttribute()]
        public string MemberNumber;

        // <remarks/>
        [XmlAttribute()]
        public string PromotionCode;

        // <remarks/>
        [XmlAttribute()]
        public int RedemptionQuantity;

        // <remarks/>
        [XmlIgnore()]
        public bool RedemptionQuantitySpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class MiscChargeOrder
    {

        // <remarks/>
        [XmlAttribute()]
        public string TicketNumber;
    }

    // <remarks/>
    public enum PaymentFormShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum PaymentFormShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class RelatedTraveler
    {

        // <remarks/>
        public UniqueID UniqueID;

        // <remarks/>
        public PersonName PersonName;

        // <remarks/>
        [XmlAttribute()]
        public RelatedTravelerShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public RelatedTravelerShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Relation;
    }

    // <remarks/>
    public enum RelatedTravelerShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum RelatedTravelerShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class ContactPerson
    {

        // <remarks/>
        public PersonName PersonName;

        // <remarks/>
        [XmlElement("Telephone")]
        public Telephone[] Telephone;

        // <remarks/>
        [XmlElement("Address")]
        public Address[] Address;

        // <remarks/>
        [XmlElement("Email")]
        public Email[] Email;

        // <remarks/>
        [XmlElement("URL")]
        public URL[] URL;

        // <remarks/>
        [XmlElement("CompanyNameFull")]
        public Code.CompanyName[] CompanyName;

        // <remarks/>
        [XmlElement("EmployeeInfo")]
        public EmployeeInfo[] EmployeeInfo;

        // <remarks/>
        [XmlAttribute()]
        public ContactPersonShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public ContactPersonShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        // <remarks/>
        [XmlAttribute()]
        public string ContactType;

        // <remarks/>
        [XmlAttribute()]
        public string Relation;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool EmergencyFlag = false;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    public enum ContactPersonShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum ContactPersonShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Document
    {

        // <remarks/>
        public string DocHolderName;

        // <remarks/>
        [XmlElement("DocLimitations")]
        public string[] DocLimitations;

        // <remarks/>
        [XmlAttribute()]
        public DocumentShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public DocumentShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string DocIssueAuthority;

        // <remarks/>
        [XmlAttribute()]
        public string DocIssueLocation;

        // <remarks/>
        [XmlAttribute()]
        public string DocID;

        // <remarks/>
        [XmlAttribute()]
        public string DocType;

        // <remarks/>
        [XmlAttribute()]
        public DocumentGender Gender;

        // <remarks/>
        [XmlIgnore()]
        public bool GenderSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate;

        // <remarks/>
        [XmlIgnore()]
        public bool BirthDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        // <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified;
    }

    // <remarks/>
    public enum DocumentShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum DocumentShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum DocumentGender
    {

        // <remarks/>
        Male,

        // <remarks/>
        Female,

        // <remarks/>
        Unknown
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CustLoyalty
    {

        // <remarks/>
        [XmlAttribute()]
        public CustLoyaltyShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CustLoyaltyShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string ProgramID;

        // <remarks/>
        [XmlAttribute()]
        public string MembershipID;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string LoyalLevel;

        // <remarks/>
        [XmlAttribute()]
        public CustLoyaltySingleVendorInd SingleVendorInd;

        // <remarks/>
        [XmlIgnore()]
        public bool SingleVendorIndSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime SignupDate;

        // <remarks/>
        [XmlIgnore()]
        public bool SignupDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        // <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    public enum CustLoyaltyShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum CustLoyaltyShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum CustLoyaltySingleVendorInd
    {

        // <remarks/>
        SingleVndr,

        // <remarks/>
        Alliance
    }

    // <remarks/>
    public enum CustomerGender
    {

        // <remarks/>
        Male,

        // <remarks/>
        Female,

        // <remarks/>
        Unknown
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class EmployeeInfo
    {

        // <remarks/>
        [XmlAttribute()]
        public string EmployeeId;

        // <remarks/>
        [XmlAttribute()]
        public string EmployeeLevel;

        // <remarks/>
        [XmlAttribute()]
        public string EmployeeTitle;

        // <remarks/>
        [XmlAttribute()]
        public string EmployeeStatus;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Address
    {

        // <remarks/>
        public StreetNmbr StreetNmbr;

        // <remarks/>
        public string BldgRoom;

        // <remarks/>
        [XmlElement("AddressLine")]
        public string[] AddressLine;

        // <remarks/>
        public string CityName;

        // <remarks/>
        public string PostalCode;

        // <remarks/>
        public string County;

        // <remarks/>
        public StateProv StateProv;

        // <remarks/>
        public CountryName CountryName;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        // <remarks/>
        [XmlAttribute()]
        public AddressShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public AddressShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Type;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class StreetNmbr
    {

        // <remarks/>
        [XmlAttribute()]
        public string PO_Box;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class StateProv
    {

        // <remarks/>
        [XmlAttribute()]
        public string StateCode;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CountryName
    {

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum AddressShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AddressShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AddressInfo
    {

        // <remarks/>
        public StreetNmbr StreetNmbr;

        // <remarks/>
        public string BldgRoom;

        // <remarks/>
        [XmlElement("AddressLine")]
        public string[] AddressLine;

        // <remarks/>
        public string CityName;

        // <remarks/>
        public string PostalCode;

        // <remarks/>
        public string County;

        // <remarks/>
        public StateProv StateProv;

        // <remarks/>
        public CountryName CountryName;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        // <remarks/>
        [XmlAttribute()]
        public AddressInfoShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public AddressInfoShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        // <remarks/>
        [XmlAttribute()]
        public string UseType;
    }

    // <remarks/>
    public enum AddressInfoShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AddressInfoShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AddressPref
    {

        // <remarks/>
        public Address Address;

        // <remarks/>
        [XmlAttribute()]
        public AddressPrefShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public AddressPrefShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;
    }

    // <remarks/>
    public enum AddressPrefShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AddressPrefShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Organization
    {

        // <remarks/>
        public OrgMemberName OrgMemberName;

        // <remarks/>
        public OrgName OrgName;

        // <remarks/>
        [XmlElement("RelatedOrgName")]
        public RelatedOrgName[] RelatedOrgName;

        // <remarks/>
        [XmlElement("TravelArranger")]
        public TravelArranger[] TravelArranger;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        // <remarks/>
        [XmlAttribute()]
        public OrganizationShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public OrganizationShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        // <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified;

        // <remarks/>
        [XmlAttribute()]
        public OrganizationOfficeType OfficeType;

        // <remarks/>
        [XmlIgnore()]
        public bool OfficeTypeSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class OrgMemberName
    {

        // <remarks/>
        [XmlElement("NamePrefix")]
        public string[] NamePrefix;

        // <remarks/>
        [XmlElement("GivenName")]
        public string[] GivenName;

        // <remarks/>
        [XmlElement("MiddleName")]
        public string[] MiddleName;

        // <remarks/>
        public string SurnamePrefix;

        // <remarks/>
        public string Surname;

        // <remarks/>
        [XmlElement("NameSuffix")]
        public string[] NameSuffix;

        // <remarks/>
        [XmlElement("NameTitle")]
        public string[] NameTitle;

        // <remarks/>
        [XmlAttribute()]
        public OrgMemberNameShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public OrgMemberNameShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string NameType;

        // <remarks/>
        [XmlAttribute()]
        public string ID;

        // <remarks/>
        [XmlAttribute()]
        public string Level;

        // <remarks/>
        [XmlAttribute()]
        public string Title;
    }

    // <remarks/>
    public enum OrgMemberNameShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum OrgMemberNameShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class OrgName
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class RelatedOrgName
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TravelArranger
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string TravelArrangerType;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum TravelArrangerShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum TravelArrangerShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum OrganizationShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum OrganizationShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum OrganizationOfficeType
    {

        // <remarks/>
        Main,

        // <remarks/>
        Field,

        // <remarks/>
        Division,

        // <remarks/>
        Regional,

        // <remarks/>
        Remote
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Employer
    {

        // <remarks/>
        public Code.CompanyName CompanyName;

        // <remarks/>
        [XmlElement("RelatedEmployer")]
        public RelatedEmployer[] RelatedEmployer;

        // <remarks/>
        [XmlElement("EmployeeInfo")]
        public EmployeeInfo[] EmployeeInfo;

        // <remarks/>
        [XmlElement("InternalRefNmbr")]
        public InternalRefNmbr[] InternalRefNmbr;

        // <remarks/>
        [XmlElement("TravelArranger")]
        public TravelArranger[] TravelArranger;

        // <remarks/>
        [XmlElement("LoyaltyProgram")]
        public LoyaltyProgram[] LoyaltyProgram;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        // <remarks/>
        [XmlAttribute()]
        public EmployerOfficeType OfficeType;

        // <remarks/>
        [XmlIgnore()]
        public bool OfficeTypeSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        // <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class RelatedEmployer
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class InternalRefNmbr
    {

        // <remarks/>
        [XmlAttribute()]
        public string Language;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class LoyaltyProgram
    {

        // <remarks/>
        [XmlAttribute()]
        public string ProgramCode;

        // <remarks/>
        [XmlAttribute()]
        public LoyaltyProgramSingleVendorInd SingleVendorInd;

        // <remarks/>
        [XmlIgnore()]
        public bool SingleVendorIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string LoyaltyLevel;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum LoyaltyProgramSingleVendorInd
    {

        // <remarks/>
        SingleVndr,

        // <remarks/>
        Alliance
    }

    // <remarks/>
    public enum EmployerOfficeType
    {

        // <remarks/>
        Main,

        // <remarks/>
        Field,

        // <remarks/>
        Division,

        // <remarks/>
        Regional,

        // <remarks/>
        Remote
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TravelClub
    {

        // <remarks/>
        public TravelClubName TravelClubName;

        // <remarks/>
        public ClubMemberName ClubMemberName;

        // <remarks/>
        [XmlAttribute()]
        public TravelClubShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelClubShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        // <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TravelClubName
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class ClubMemberName
    {

        // <remarks/>
        [XmlElement("NamePrefix")]
        public string[] NamePrefix;

        // <remarks/>
        [XmlElement("GivenName")]
        public string[] GivenName;

        // <remarks/>
        [XmlElement("MiddleName")]
        public string[] MiddleName;

        // <remarks/>
        public string SurnamePrefix;

        // <remarks/>
        public string Surname;

        // <remarks/>
        [XmlElement("NameSuffix")]
        public string[] NameSuffix;

        // <remarks/>
        [XmlElement("NameTitle")]
        public string[] NameTitle;

        // <remarks/>
        [XmlAttribute()]
        public ClubMemberNameShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public ClubMemberNameShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string NameType;

        // <remarks/>
        [XmlAttribute()]
        public string ID;
    }

    // <remarks/>
    public enum ClubMemberNameShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum ClubMemberNameShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum TravelClubShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum TravelClubShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Insurance
    {

        // <remarks/>
        public InsuredName InsuredName;

        // <remarks/>
        public InsuranceCompany InsuranceCompany;

        // <remarks/>
        public Underwriter Underwriter;

        // <remarks/>
        [XmlAttribute()]
        public InsuranceShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public InsuranceShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string InsuranceType;

        // <remarks/>
        [XmlAttribute()]
        public string PolicyNumber;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        // <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class InsuredName
    {

        // <remarks/>
        [XmlElement("NamePrefix")]
        public string[] NamePrefix;

        // <remarks/>
        [XmlElement("GivenName")]
        public string[] GivenName;

        // <remarks/>
        [XmlElement("MiddleName")]
        public string[] MiddleName;

        // <remarks/>
        public string SurnamePrefix;

        // <remarks/>
        public string Surname;

        // <remarks/>
        [XmlElement("NameSuffix")]
        public string[] NameSuffix;

        // <remarks/>
        [XmlElement("NameTitle")]
        public string[] NameTitle;

        // <remarks/>
        [XmlAttribute()]
        public InsuredNameShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public InsuredNameShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string NameType;
    }

    // <remarks/>
    public enum InsuredNameShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum InsuredNameShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class InsuranceCompany
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Underwriter
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum InsuranceShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum InsuranceShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AffiliationsShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AffiliationsShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Certification
    {

        // <remarks/>
        [XmlAttribute()]
        public string Language;

        // <remarks/>
        [XmlAttribute()]
        public string ID;

        // <remarks/>
        [XmlAttribute()]
        public CertificationSingleVendorInd SingleVendorInd;

        // <remarks/>
        [XmlIgnore()]
        public bool SingleVendorIndSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        // <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum CertificationSingleVendorInd
    {

        // <remarks/>
        SingleVndr,

        // <remarks/>
        Alliance
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AllianceConsortium
    {

        // <remarks/>
        [XmlElement("AllianceMember")]
        public AllianceMember[] AllianceMember;

        // <remarks/>
        [XmlAttribute()]
        public string ID;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        // <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AllianceMember
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlAttribute()]
        public string MemberCode;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CommissionInfo
    {

        // <remarks/>
        [XmlAttribute()]
        public string Language;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string CommissionPlanCode;

        // <remarks/>
        [XmlAttribute()]
        public double Amount;

        // <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute()]
        public int DecimalPlaces;

        // <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum CommissionInfoShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum CommissionInfoShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AgreementsShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AgreementsShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class LoyaltyPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(LoyaltyPrefPreferLevel.Preferred)]
        public LoyaltyPrefPreferLevel PreferLevel = LoyaltyPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum LoyaltyPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class VendorPref
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(VendorPrefPreferLevel.Preferred)]
        public VendorPrefPreferLevel PreferLevel = VendorPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum VendorPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PaymentFormPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PaymentFormPrefPreferLevel.Preferred)]
        public PaymentFormPrefPreferLevel PreferLevel = PaymentFormPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum PaymentFormPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AirportOriginPref
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(AirportOriginPrefPreferLevel.Preferred)]
        public AirportOriginPrefPreferLevel PreferLevel = AirportOriginPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum AirportOriginPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AirportRoutePref
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(AirportRoutePrefPreferLevel.Preferred)]
        public AirportRoutePrefPreferLevel PreferLevel = AirportRoutePrefPreferLevel.Preferred;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum AirportRoutePrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class FareRestrictPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(FareRestrictPrefPreferLevel.Preferred)]
        public FareRestrictPrefPreferLevel PreferLevel = FareRestrictPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string FareRestriction;
    }

    // <remarks/>
    public enum FareRestrictPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class FlightTypePref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(FlightTypePrefPreferLevel.Preferred)]
        public FlightTypePrefPreferLevel PreferLevel = FlightTypePrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public FlightTypePrefFlightType FlightType;

        // <remarks/>
        [XmlIgnore()]
        public bool FlightTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public int MaxConnections;

        // <remarks/>
        [XmlIgnore()]
        public bool MaxConnectionsSpecified;
    }

    // <remarks/>
    public enum FlightTypePrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum FlightTypePrefFlightType
    {

        // <remarks/>
        Nonstop,

        // <remarks/>
        Direct,

        // <remarks/>
        Connection
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class EquipPref
    {

        // <remarks/>
        [XmlAttribute()]
        public string AirEquipType;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool ChangeofGauge = false;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(EquipPrefPreferLevel.Preferred)]
        public EquipPrefPreferLevel PreferLevel = EquipPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum EquipPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CabinPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(CabinPrefPreferLevel.Preferred)]
        public CabinPrefPreferLevel PreferLevel = CabinPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public CabinPrefCabin Cabin;

        // <remarks/>
        [XmlIgnore()]
        public bool CabinSpecified;
    }

    // <remarks/>
    public enum CabinPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum CabinPrefCabin
    {

        // <remarks/>
        First,

        // <remarks/>
        Business,

        // <remarks/>
        Economy
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class SeatPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SeatPrefPreferLevel.Preferred)]
        public SeatPrefPreferLevel PreferLevel = SeatPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string SeatNumber;

        // <remarks/>
        [XmlAttribute()]
        public string SeatPreference;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool SmokingAllowed = false;
    }

    // <remarks/>
    public enum SeatPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TicketDistribPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(TicketDistribPrefPreferLevel.Preferred)]
        public TicketDistribPrefPreferLevel PreferLevel = TicketDistribPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string DistribType;

        // <remarks/>
        [XmlAttribute()]
        public string TicketTime;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum TicketDistribPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class MealPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(MealPrefPreferLevel.Preferred)]
        public MealPrefPreferLevel PreferLevel = MealPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public MealPrefMealType MealType;

        // <remarks/>
        [XmlIgnore()]
        public bool MealTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string FavoriteFood;

        // <remarks/>
        [XmlAttribute()]
        public string Beverage;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum MealPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum MealPrefMealType
    {

        // <remarks/>
        AVML,

        // <remarks/>
        BBML,

        // <remarks/>
        BLML,

        // <remarks/>
        CHML,

        // <remarks/>
        DBML,

        // <remarks/>
        FPML,

        // <remarks/>
        GFML,

        // <remarks/>
        HFML,

        // <remarks/>
        HNML,

        // <remarks/>
        KSML,

        // <remarks/>
        LCML,

        // <remarks/>
        LFML,

        // <remarks/>
        LPML,

        // <remarks/>
        LSML,

        // <remarks/>
        MOML,

        // <remarks/>
        NLML,

        // <remarks/>
        ORML,

        // <remarks/>
        PRML,

        // <remarks/>
        RVML,

        // <remarks/>
        SFML,

        // <remarks/>
        SPML,

        // <remarks/>
        VGML,

        // <remarks/>
        VLML
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class SpecRequestPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SpecRequestPrefPreferLevel.Preferred)]
        public SpecRequestPrefPreferLevel PreferLevel = SpecRequestPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum SpecRequestPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class SSR_Pref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SSR_PrefPreferLevel.Preferred)]
        public SSR_PrefPreferLevel PreferLevel = SSR_PrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string SSR_Code;
    }

    // <remarks/>
    public enum SSR_PrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class MediaEntertainPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(MediaEntertainPrefPreferLevel.Preferred)]
        public MediaEntertainPrefPreferLevel PreferLevel = MediaEntertainPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum MediaEntertainPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PetInfoPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PetInfoPrefPreferLevel.Preferred)]
        public PetInfoPrefPreferLevel PreferLevel = PetInfoPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum PetInfoPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum AirlinePrefShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AirlinePrefShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AirlinePrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum AirlinePrefAirTicketType
    {

        // <remarks/>
        eTicket,

        // <remarks/>
        Paper
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class DateWindowRange
    {

        // <remarks/>
        [XmlAttribute()]
        public string WindowBefore;

        // <remarks/>
        [XmlAttribute()]
        public string WindowAfter;

        // <remarks/>
        [XmlAttribute()]
        public bool CrossDateAllowedIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool CrossDateAllowedIndicatorSpecified;

        // <remarks/>
        [XmlText(DataType = "date")]
        public DateTime Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class NamePref
    {

        // <remarks/>
        public UniqueID UniqueID;

        // <remarks/>
        public PersonName PersonName;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(NamePrefPreferLevel.Preferred)]
        public NamePrefPreferLevel PreferLevel = NamePrefPreferLevel.Preferred;
    }

    // <remarks/>
    public enum NamePrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PhonePref
    {

        // <remarks/>
        public Telephone Telephone;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class InterestPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(InterestPrefPreferLevel.Preferred)]
        public InterestPrefPreferLevel PreferLevel = InterestPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum InterestPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class InsurancePref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(InsurancePrefPreferLevel.Preferred)]
        public InsurancePrefPreferLevel PreferLevel = InsurancePrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum InsurancePrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class SeatingPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SeatingPrefPreferLevel.Preferred)]
        public SeatingPrefPreferLevel PreferLevel = SeatingPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string SeatDirection;

        // <remarks/>
        [XmlAttribute()]
        public string SeatLocation;

        // <remarks/>
        [XmlAttribute()]
        public string SeatPosition;

        // <remarks/>
        [XmlAttribute()]
        public string SeatRow;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum SeatingPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class RelatedTravelerPref
    {

        // <remarks/>
        public UniqueID UniqueID;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(RelatedTravelerPrefPreferLevel.Preferred)]
        public RelatedTravelerPrefPreferLevel PreferLevel = RelatedTravelerPrefPreferLevel.Preferred;
    }

    // <remarks/>
    public enum RelatedTravelerPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum CommonPrefShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum CommonPrefShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class VehicleRentalPref
    {

        // <remarks/>
        [XmlElement("LoyaltyPref")]
        public LoyaltyPref[] LoyaltyPref;

        // <remarks/>
        [XmlElement("VendorPref")]
        public VendorPref[] VendorPref;

        // <remarks/>
        [XmlElement("PaymentFormPref")]
        public PaymentFormPref[] PaymentFormPref;

        // <remarks/>
        [XmlElement("CoveragePref")]
        public CoveragePref[] CoveragePref;

        // <remarks/>
        [XmlElement("SpecialReqPref")]
        public SpecialReqPref[] SpecialReqPref;

        // <remarks/>
        [XmlElement("VehTypePref")]
        public VehTypePref[] VehTypePref;

        // <remarks/>
        [XmlElement("SpecialEquipPref")]
        public SpecialEquipPref[] SpecialEquipPref;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(VehicleRentalPrefPreferLevel.Preferred)]
        public VehicleRentalPrefPreferLevel PreferLevel = VehicleRentalPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public VehicleRentalPrefShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public VehicleRentalPrefShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool SmokingAllowed = false;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool GasPrePay = false;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class SpecialReqPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SpecialReqPrefPreferLevel.Preferred)]
        public SpecialReqPrefPreferLevel PreferLevel = SpecialReqPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum SpecialReqPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CoveragePref
    {

        // <remarks/>
        [XmlAttribute()]
        public string CoverageType;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(CoveragePrefPreferLevel.Preferred)]
        public CoveragePrefPreferLevel PreferLevel = CoveragePrefPreferLevel.Preferred;
    }

    // <remarks/>
    public enum CoveragePrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class VehTypePref
    {

        // <remarks/>
        public VehType VehType;

        // <remarks/>
        public VehClass VehClass;

        // <remarks/>
        [XmlAttribute()]
        public bool AirConditionInd;

        // <remarks/>
        [XmlIgnore()]
        public bool AirConditionIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public VehTypePrefTransmissionType TransmissionType;

        // <remarks/>
        [XmlIgnore()]
        public bool TransmissionTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public VehTypePrefTypePref TypePref;

        // <remarks/>
        [XmlIgnore()]
        public bool TypePrefSpecified;

        // <remarks/>
        [XmlAttribute()]
        public VehTypePrefClassPref ClassPref;

        // <remarks/>
        [XmlIgnore()]
        public bool ClassPrefSpecified;

        // <remarks/>
        [XmlAttribute()]
        public VehTypePrefAirConditionPref AirConditionPref;

        // <remarks/>
        [XmlIgnore()]
        public bool AirConditionPrefSpecified;

        // <remarks/>
        [XmlAttribute()]
        public VehTypePrefTransmissionPref TransmissionPref;

        // <remarks/>
        [XmlIgnore()]
        public bool TransmissionPrefSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class VehType
    {

        // <remarks/>
        [XmlAttribute()]
        public string VehicleCategory;

        // <remarks/>
        [XmlAttribute()]
        public int DoorCount;

        // <remarks/>
        [XmlIgnore()]
        public bool DoorCountSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class VehClass
    {

        // <remarks/>
        [XmlAttribute()]
        public string Size;
    }

    // <remarks/>
    public enum VehTypePrefTransmissionType
    {

        // <remarks/>
        Automatic,

        // <remarks/>
        Manual
    }

    // <remarks/>
    public enum VehTypePrefTypePref
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum VehTypePrefClassPref
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum VehTypePrefAirConditionPref
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum VehTypePrefTransmissionPref
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class SpecialEquipPref
    {

        // <remarks/>
        [XmlAttribute()]
        public string EquipType;

        // <remarks/>
        [XmlAttribute()]
        public int Quantity;

        // <remarks/>
        [XmlIgnore()]
        public bool QuantitySpecified;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SpecialEquipPrefPreferLevel.Preferred)]
        public SpecialEquipPrefPreferLevel PreferLevel = SpecialEquipPrefPreferLevel.Preferred;
    }

    // <remarks/>
    public enum SpecialEquipPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum VehicleRentalPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum VehicleRentalPrefShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum VehicleRentalPrefShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class HotelChainPref
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(HotelChainPrefPreferLevel.Preferred)]
        public HotelChainPrefPreferLevel PreferLevel = HotelChainPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum HotelChainPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PropertyNamePref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PropertyNamePrefPreferLevel.Preferred)]
        public PropertyNamePrefPreferLevel PreferLevel = PropertyNamePrefPreferLevel.Preferred;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum PropertyNamePrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PropertyLocationPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PropertyLocationPrefPreferLevel.Preferred)]
        public PropertyLocationPrefPreferLevel PreferLevel = PropertyLocationPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string PropertyLocationType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum PropertyLocationPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PropertyTypePref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PropertyTypePrefPreferLevel.Preferred)]
        public PropertyTypePrefPreferLevel PreferLevel = PropertyTypePrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string PropertyType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum PropertyTypePrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PropertyClassPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PropertyClassPrefPreferLevel.Preferred)]
        public PropertyClassPrefPreferLevel PreferLevel = PropertyClassPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string PropertyClassType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum PropertyClassPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PropertyAmenityPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PropertyAmenityPrefPreferLevel.Preferred)]
        public PropertyAmenityPrefPreferLevel PreferLevel = PropertyAmenityPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string PropertyAmenityType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum PropertyAmenityPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class RoomAmenityPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(RoomAmenityPrefPreferLevel.Preferred)]
        public RoomAmenityPrefPreferLevel PreferLevel = RoomAmenityPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string RoomAmenity;

        // <remarks/>
        [XmlAttribute()]
        public int Quantity;

        // <remarks/>
        [XmlIgnore()]
        public bool QuantitySpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum RoomAmenityPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class RoomLocationPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(RoomLocationPrefPreferLevel.Preferred)]
        public RoomLocationPrefPreferLevel PreferLevel = RoomLocationPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string RoomLocationType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum RoomLocationPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class BedTypePref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(BedTypePrefPreferLevel.Preferred)]
        public BedTypePrefPreferLevel PreferLevel = BedTypePrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string BedType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum BedTypePrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class FoodSrvcPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(FoodSrvcPrefPreferLevel.Preferred)]
        public FoodSrvcPrefPreferLevel PreferLevel = FoodSrvcPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string FoodSrvcType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum FoodSrvcPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class RecreationSrvcPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(RecreationSrvcPrefPreferLevel.Preferred)]
        public RecreationSrvcPrefPreferLevel PreferLevel = RecreationSrvcPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string RecreationSrvcType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum RecreationSrvcPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class BusinessSrvcPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(BusinessSrvcPrefPreferLevel.Preferred)]
        public BusinessSrvcPrefPreferLevel PreferLevel = BusinessSrvcPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string BusinessSrvcType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum BusinessSrvcPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class HotelPref
    {

        // <remarks/>
        [XmlElement("LoyaltyPref")]
        public LoyaltyPref[] LoyaltyPref;

        // <remarks/>
        [XmlElement("PaymentFormPref")]
        public PaymentFormPref[] PaymentFormPref;

        // <remarks/>
        [XmlElement("HotelChainPref")]
        public HotelChainPref[] HotelChainPref;

        // <remarks/>
        [XmlElement("PropertyNamePref")]
        public PropertyNamePref[] PropertyNamePref;

        // <remarks/>
        [XmlElement("PropertyLocationPref")]
        public PropertyLocationPref[] PropertyLocationPref;

        // <remarks/>
        [XmlElement("PropertyTypePref")]
        public PropertyTypePref[] PropertyTypePref;

        // <remarks/>
        [XmlElement("PropertyClassPref")]
        public PropertyClassPref[] PropertyClassPref;

        // <remarks/>
        [XmlElement("PropertyAmenityPref")]
        public PropertyAmenityPref[] PropertyAmenityPref;

        // <remarks/>
        [XmlElement("RoomAmenityPref")]
        public RoomAmenityPref[] RoomAmenityPref;

        // <remarks/>
        [XmlElement("RoomLocationPref")]
        public RoomLocationPref[] RoomLocationPref;

        // <remarks/>
        [XmlElement("BedTypePref")]
        public BedTypePref[] BedTypePref;

        // <remarks/>
        [XmlElement("FoodSrvcPref")]
        public FoodSrvcPref[] FoodSrvcPref;

        // <remarks/>
        [XmlElement("MediaEntertainPref")]
        public MediaEntertainPref[] MediaEntertainPref;

        // <remarks/>
        [XmlElement("PetInfoPref")]
        public PetInfoPref[] PetInfoPref;

        // <remarks/>
        [XmlElement("MealPref")]
        public MealPref[] MealPref;

        // <remarks/>
        [XmlElement("RecreationSrvcPref")]
        public RecreationSrvcPref[] RecreationSrvcPref;

        // <remarks/>
        [XmlElement("BusinessSrvcPref")]
        public BusinessSrvcPref[] BusinessSrvcPref;

        // <remarks/>
        [XmlElement("PersonalSrvcPref")]
        public PersonalSrvcPref[] PersonalSrvcPref;

        // <remarks/>
        [XmlElement("SecurityFeaturePref")]
        public SecurityFeaturePref[] SecurityFeaturePref;

        // <remarks/>
        [XmlElement("PhysChallFeaturePref")]
        public PhysChallFeaturePref[] PhysChallFeaturePref;

        // <remarks/>
        [XmlElement("SpecRequestPref")]
        public SpecRequestPref[] SpecRequestPref;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(HotelPrefPreferLevel.Preferred)]
        public HotelPrefPreferLevel PreferLevel = HotelPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public HotelPrefShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public HotelPrefShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool SmokingAllowed = false;

        // <remarks/>
        [XmlAttribute()]
        public string RatePlanCode;

        // <remarks/>
        [XmlAttribute()]
        public string HotelGuestType;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class OtherSrvcPref
    {

        // <remarks/>
        public string OtherSrvcName;

        // <remarks/>
        [XmlElement("VendorPref")]
        public VendorPref[] VendorPref;

        // <remarks/>
        [XmlElement("LoyaltyPref")]
        public LoyaltyPref[] LoyaltyPref;

        // <remarks/>
        [XmlElement("PaymentFormPref")]
        public PaymentFormPref[] PaymentFormPref;

        // <remarks/>
        [XmlElement("SpecRequestPref")]
        public SpecRequestPref[] SpecRequestPref;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OtherSrvcPrefPreferLevel.Preferred)]
        public OtherSrvcPrefPreferLevel PreferLevel = OtherSrvcPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public OtherSrvcPrefShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public OtherSrvcPrefShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string TravelPurpose;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PersonalSrvcPref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PersonalSrvcPrefPreferLevel.Preferred)]
        public PersonalSrvcPrefPreferLevel PreferLevel = PersonalSrvcPrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string PersonalSrvcType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum PersonalSrvcPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class SecurityFeaturePref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SecurityFeaturePrefPreferLevel.Preferred)]
        public SecurityFeaturePrefPreferLevel PreferLevel = SecurityFeaturePrefPreferLevel.Preferred;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum SecurityFeaturePrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PhysChallFeaturePref
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PhysChallFeaturePrefPreferLevel.Preferred)]
        public PhysChallFeaturePrefPreferLevel PreferLevel = PhysChallFeaturePrefPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string PhysChallFeatureType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum PhysChallFeaturePrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum HotelPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum HotelPrefShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum HotelPrefShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum OtherSrvcPrefPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum OtherSrvcPrefShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum OtherSrvcPrefShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum PrefCollectionShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum PrefCollectionShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum PrefCollectionsShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum PrefCollectionsShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PrefCollections
    {

        // <remarks/>
        [XmlElement("PrefCollection")]
        public PrefCollection[] PrefCollection;

        // <remarks/>
        [XmlAttribute()]
        public PrefCollectionsShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public PrefCollectionsShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PrefCollection
    {

        // <remarks/>
        [XmlElement("CommonPref")]
        public CommonPref[] CommonPref;

        // <remarks/>
        [XmlElement("VehicleRentalPref")]
        public VehicleRentalPref[] VehicleRentalPref;

        // <remarks/>
        [XmlElement("AirlinePref")]
        public AirlinePref[] AirlinePref;

        // <remarks/>
        [XmlElement("HotelPref")]
        public HotelPref[] HotelPref;

        // <remarks/>
        [XmlElement("OtherSrvcPref")]
        public OtherSrvcPref[] OtherSrvcPref;

        // <remarks/>
        [XmlAttribute()]
        public PrefCollectionShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public PrefCollectionShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string TravelPurpose;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Profiles
    {

        // <remarks/>
        [XmlElement("ProfileInfo")]
        public ProfileInfo[] ProfileInfo;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class ProfileInfo
    {

        // <remarks/>
        public UniqueID UniqueID;

        // <remarks/>
        public Profile Profile;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Profile
    {

        // <remarks/>
        public Accesses Accesses;

        // <remarks/>
        public Customer Customer;

        // <remarks/>
        public PrefCollections PrefCollections;

        // <remarks/>
        public CompanyInfo CompanyInfo;

        // <remarks/>
        public Affiliations Affiliations;

        // <remarks/>
        public Agreements Agreements;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(ProfileShareAllSynchInd.No)]
        public ProfileShareAllSynchInd ShareAllSynchInd = ProfileShareAllSynchInd.No;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(ProfileShareAllMarketInd.No)]
        public ProfileShareAllMarketInd ShareAllMarketInd = ProfileShareAllMarketInd.No;

        // <remarks/>
        [XmlAttribute()]
        public string ProfileType;

        // <remarks/>
        [XmlAttribute()]
        public DateTime CreateDateTime;

        // <remarks/>
        [XmlIgnore()]
        public bool CreateDateTimeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string CreatorID;

        // <remarks/>
        [XmlAttribute()]
        public DateTime LastModifyDateTime;

        // <remarks/>
        [XmlIgnore()]
        public bool LastModifyDateTimeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string LastModifierID;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CompanyInfo
    {

        // <remarks/>
        [XmlElement("CompanyNameFull")]
        public Code.CompanyName[] CompanyName;

        // <remarks/>
        [XmlElement("AddressInfo")]
        public AddressInfo[] AddressInfo;

        // <remarks/>
        [XmlElement("TelephoneInfo")]
        public TelephoneInfo[] TelephoneInfo;

        // <remarks/>
        [XmlElement("Email")]
        public Email[] Email;

        // <remarks/>
        [XmlElement("URL")]
        public URL[] URL;

        // <remarks/>
        [XmlElement("BusinessLocale")]
        public BusinessLocale[] BusinessLocale;

        // <remarks/>
        [XmlElement("PaymentForm")]
        public PaymentForm[] PaymentForm;

        // <remarks/>
        [XmlElement("ContactPerson")]
        public ContactPerson[] ContactPerson;

        // <remarks/>
        [XmlElement("TravelArranger")]
        public TravelArranger[] TravelArranger;

        // <remarks/>
        [XmlElement("LoyaltyProgram")]
        public LoyaltyProgram[] LoyaltyProgram;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TelephoneInfo
    {

        // <remarks/>
        [XmlAttribute()]
        public TelephoneInfoShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TelephoneInfoShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneLocationType;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneTechType;

        // <remarks/>
        [XmlAttribute()]
        public string CountryAccessCode;

        // <remarks/>
        [XmlAttribute()]
        public string AreaCityCode;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneNumber;

        // <remarks/>
        [XmlAttribute()]
        public string Extension;

        // <remarks/>
        [XmlAttribute()]
        public string PIN;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneUseType;
    }

    // <remarks/>
    public enum TelephoneInfoShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum TelephoneInfoShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class BusinessLocale
    {

        // <remarks/>
        public StreetNmbr StreetNmbr;

        // <remarks/>
        public string BldgRoom;

        // <remarks/>
        [XmlElement("AddressLine")]
        public string[] AddressLine;

        // <remarks/>
        public string CityName;

        // <remarks/>
        public string PostalCode;

        // <remarks/>
        public string County;

        // <remarks/>
        public StateProv StateProv;

        // <remarks/>
        public CountryName CountryName;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        // <remarks/>
        [XmlAttribute()]
        public BusinessLocaleShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public BusinessLocaleShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Type;
    }

    // <remarks/>
    public enum BusinessLocaleShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum BusinessLocaleShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum ProfileShareAllSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No
    }

    // <remarks/>
    public enum ProfileShareAllMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class GuestCounts
    {

        // <remarks/>
        [XmlElement("GuestCount")]
        public GuestCount[] GuestCount;

        // <remarks/>
        [XmlAttribute()]
        public bool IsPerRoom;

        // <remarks/>
        [XmlIgnore()]
        public bool IsPerRoomSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class GuestCount
    {

        // <remarks/>
        [XmlAttribute()]
        public string AgeQualifyingCode;

        // <remarks/>
        [XmlAttribute()]
        public int Age;

        // <remarks/>
        [XmlIgnore()]
        public bool AgeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public int Count;

        // <remarks/>
        [XmlIgnore()]
        public bool CountSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Criterion
    {

        // <remarks/>
        public wsTripXML.Code.Position Position;

        // <remarks/>
        public Address Address;

        // <remarks/>
        public Telephone Telephone;

        // <remarks/>
        public string RefPoint;

        // <remarks/>
        public CodeRef CodeRef;

        // <remarks/>
        [XmlElement("HotelRef")]
        public HotelRef[] HotelRef;

        // <remarks/>
        public Radius Radius;

        // <remarks/>
        [XmlElement("HotelAmenity")]
        public HotelAmenity[] HotelAmenity;

        // <remarks/>
        [XmlElement("Award")]
        public Award[] Award;

        // <remarks/>
        [XmlAttribute()]
        public bool ExactMatch;

        // <remarks/>
        [XmlIgnore()]
        public bool ExactMatchSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CriterionImportanceType ImportanceType;

        // <remarks/>
        [XmlIgnore()]
        public bool ImportanceTypeSpecified;
    }

    // local Position removed: identical to shared wsTripXML.Code.Position (XML type-name collision in one serializer scope)

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CodeRef
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class HotelRef
    {

        // <remarks/>
        [XmlAttribute()]
        public string ChainCode;

        // <remarks/>
        [XmlAttribute()]
        public string BrandCode;

        // <remarks/>
        [XmlAttribute()]
        public string HotelCode;

        // <remarks/>
        [XmlAttribute()]
        public string HotelCityCode;

        // <remarks/>
        [XmlAttribute()]
        public string HotelName;

        // <remarks/>
        [XmlAttribute()]
        public string HotelCodeContext;

        // <remarks/>
        [XmlAttribute()]
        public string ChainName;

        // <remarks/>
        [XmlAttribute()]
        public string BrandName;

        // <remarks/>
        [XmlAttribute()]
        public string AreaID;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Radius
    {

        // <remarks/>
        [XmlAttribute()]
        public string Distance;

        // <remarks/>
        [XmlAttribute()]
        public string DistanceMeasure;

        // <remarks/>
        [XmlAttribute()]
        public string Direction;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class HotelAmenity
    {

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Award
    {

        // <remarks/>
        [XmlAttribute()]
        public string Provider;

        // <remarks/>
        [XmlAttribute()]
        public string Rating;
    }

    // <remarks/>
    public enum CriterionImportanceType
    {

        // <remarks/>
        Mandatory,

        // <remarks/>
        High,

        // <remarks/>
        Medium,

        // <remarks/>
        Low
    }

}