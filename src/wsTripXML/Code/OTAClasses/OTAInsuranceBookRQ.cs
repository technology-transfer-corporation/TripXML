using System.Xml.Serialization;
using wsTripXML.wsTravelTalk.wmInsuranceBook;

namespace wsTripXML.wsTravelTalk.wmInsuranceBookIn
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

        public CompanyName CompanyName;

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

        [XmlAttribute()]
        public string EffectiveDate;

        [XmlAttribute()]
        public string ExpireDate;
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
    public class Beneficiary
    {

        public string Name;

        public Address Address;

        [XmlAttribute()]
        public string Relation;

        [XmlAttribute()]
        public string BenefitPercent;

        [XmlAttribute()]
        public string ID;
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
    public class ContactPerson
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

        [XmlElement("CompanyName")]
        public CompanyName[] CompanyName;

        [XmlElement("EmployeeInfo")]
        public EmployeeInfo[] EmployeeInfo;

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
        public string EffectiveDate;

        [XmlAttribute()]
        public string ExpireDate;
    }

    [XmlRoot(IsNullable = false)]
    public class CoveredTraveler
    {

        public CoveredPerson CoveredPerson;

        [XmlElement("Email")]
        public Email[] Email;

        [XmlElement("Address")]
        public Address[] Address;

        [XmlElement("Telephone")]
        public Telephone[] Telephone;

        public CitizenCountryName CitizenCountryName;

        [XmlElement("Document")]
        public Document[] Document;

        [XmlElement("EmergencyContact")]
        public EmergencyContact[] EmergencyContact;

        [XmlElement("Beneficiary")]
        public Beneficiary[] Beneficiary;

        public IndCoverageReqs IndCoverageReqs;

        [XmlAttribute()]
        public string RPH;
    }

    [XmlRoot(IsNullable = false)]
    public class EmergencyContact
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

        [XmlElement("CompanyName")]
        public CompanyName[] CompanyName;

        [XmlElement("EmployeeInfo")]
        public EmployeeInfo[] EmployeeInfo;

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
    }

    [XmlRoot(IsNullable = false)]
    public class CoveredTravelers
    {

        [XmlElement("CoveredTraveler")]
        public CoveredTraveler[] CoveredTraveler;
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
    }

    [XmlRoot(IsNullable = false)]
    public class DirectBill
    {

        public CompanyName CompanyName;

        public Address Address;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        [XmlAttribute()]
        public string DirectBill_ID;
    }

    [XmlRoot(IsNullable = false)]
    public class EmployerInfo
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
    public class InsuranceCustomer
    {

        public PersonName PersonName;

        [XmlElement("Telephone")]
        public Telephone[] Telephone;

        [XmlElement("Email")]
        public Email[] Email;

        [XmlElement("Address")]
        public Address[] Address;

        [XmlElement("URL")]
        public URL[] URL;

        [XmlElement("CitizenCountryName")]
        public CitizenCountryName[] CitizenCountryName;

        [XmlElement("PhysChallName")]
        public string[] PhysChallName;

        [XmlElement("PetInfo")]
        public string[] PetInfo;

        [XmlElement("PaymentForm")]
        public PaymentForm[] PaymentForm;

        [XmlElement("RelatedTraveler")]
        public RelatedTraveler[] RelatedTraveler;

        [XmlElement("ContactPerson")]
        public ContactPerson[] ContactPerson;

        [XmlElement("Document")]
        public Document[] Document;

        [XmlElement("CustLoyalty")]
        public CustLoyalty[] CustLoyalty;

        [XmlElement("EmployeeInfo")]
        public EmployeeInfo[] EmployeeInfo;

        public EmployerInfo EmployerInfo;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string Gender;

        [XmlAttribute()]
        public string Deceased;

        [XmlAttribute()]
        public string LockoutType;

        [XmlAttribute()]
        public string BirthDate;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public string DecimalPlaces;

        [XmlAttribute()]
        public string VIP_Indicator;

        [XmlAttribute()]
        public string ID;
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentForm
    {

        public PaymentCard PaymentCard;

        public BankAcct BankAcct;

        public DirectBill DirectBill;

        public Voucher Voucher;

        public LoyaltyRedemption LoyaltyRedemption;

        public MiscChargeOrder MiscChargeOrder;

        public Cash Cash;

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

        [XmlAttribute()]
        public string MaskedCardNumber;

        [XmlAttribute()]
        public string CardHolderRPH;
    }

    public enum PaymentCardCardCode
    {

        AX,

        BC,

        BL,

        CB,

        DN,

        DS,

        EC,

        JC,

        MC,

        TP,

        VI
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
        public string PromotionVendorCode;

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
    public class RelatedTraveler
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

        public CompanyName CompanyName;

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
    public class OTA_InsuranceBookRQ
    {

        public POS POS;

        [XmlElement("PlanForBookRQ")]
        public PlanForBookRQ[] PlanForBookRQ;

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

    [XmlRoot(IsNullable = false)]
    public class PlanForBookRQ
    {

        [XmlArrayItem(IsNullable = false)]
        public CoveredTraveler[] CoveredTravelers;

        public InsCoverageDetail InsCoverageDetail;

        public InsuranceCustomer InsuranceCustomer;

        public PlanCost PlanCost;

        public UniqueID UniqueID;

        [XmlAttribute()]
        public string PlanID;

        [XmlAttribute()]
        public string Name;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public string TypeID;
    }

    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        [XmlElement("Provider")]
        public Provider[] Provider;
    }
}
