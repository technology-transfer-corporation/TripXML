using System;
using System.Xml.Serialization;


namespace wsTripXML.wsTravelTalk.wmHotelModifyIn
{

    [XmlRoot(IsNullable = false)]
    public class AcceptedPayment
    {

        public PaymentCardRQ PaymentCard;

        public BankAcctRQ BankAcct;

        public DirectBill DirectBill;

        public Voucher Voucher;

        public LoyaltyRedemption LoyaltyRedemption;

        public MiscChargeOrder MiscChargeOrder;

        public Cash Cash;

        [XmlAttribute()]
        public AcceptedPaymentShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AcceptedPaymentShareMarketIndRQ ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CostCenterID;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public AcceptedPaymentPaymentTransactionTypeCode PaymentTransactionTypeCode;

        [XmlIgnore()]
        public bool PaymentTransactionTypeCodeSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentCardRQ
    {

        public string CardHolderName;

        public CardIssuerName CardIssuerName;

        public Address Address;

        [XmlAttribute()]
        public PaymentCardShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public PaymentCardShareMarketIndRQ ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CardType;

        [XmlAttribute()]
        public PaymentCardCardCodeRQ CardCode;

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
    public class Cash
    {

        [XmlAttribute()]
        public bool CashIndicator;

        [XmlIgnore()]
        public bool CashIndicatorSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class MiscChargeOrder
    {

        [XmlAttribute()]
        public string TicketNumber;
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
    public class DirectBill
    {

        public CompanyName CompanyName;

        public Address Address;

        [XmlAttribute()]
        public DirectBillShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public DirectBillShareMarketIndRQ ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string DirectBill_ID;
    }

    [XmlRoot(IsNullable = false)]
    public class Address
    {

        public StreetNmbrRQ StreetNmbr;

        public string BldgRoom;

        [XmlElement("AddressLine")]
        public string[] AddressLine;

        public string CityName;

        public string PostalCode;

        public string County;

        public StateProvRQ StateProv;

        public CountryNameRQ CountryName;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        [XmlAttribute()]
        public AddressShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressShareMarketIndRQ ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string Type;
    }

    [XmlRoot(IsNullable = false)]
    public class StreetNmbrRQ
    {

        [XmlAttribute()]
        public string PO_Box;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class StateProvRQ
    {

        [XmlAttribute()]
        public string StateCode;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class CountryNameRQ
    {

        [XmlAttribute()]
        public string Code;

        [XmlText()]
        public string Value;
    }

    public enum AddressShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum AddressShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum DirectBillShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum DirectBillShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class BankAcctRQ
    {

        public string BankAcctName;

        [XmlAttribute()]
        public BankAcctShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public BankAcctShareMarketIndRQ ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string BankID;

        [XmlAttribute()]
        public string AcctType;

        [XmlAttribute()]
        public string BankAcctNumber;
    }

    public enum BankAcctShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum BankAcctShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum PaymentCardShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum PaymentCardShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum PaymentCardCardCodeRQ
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

    public enum AcceptedPaymentShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum AcceptedPaymentShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum AcceptedPaymentPaymentTransactionTypeCode
    {

        charge,

        reserve
    }

    [XmlRoot(IsNullable = false)]
    public class AcceptedPayments
    {

        [XmlElement("AcceptedPayment")]
        public AcceptedPayment[] AcceptedPayment;
    }

    [XmlRoot(IsNullable = false)]
    public class AccessRQ
    {

        public AccessPersonRQ AccessPerson;

        public AccessCommentRQ AccessComment;

        [XmlAttribute()]
        public AccessActionTypeRQ ActionType;

        [XmlIgnore()]
        public bool ActionTypeSpecified;

        [XmlAttribute()]
        public DateTime ActionDateTime;

        [XmlIgnore()]
        public bool ActionDateTimeSpecified;

        [XmlAttribute()]
        public string ID;
    }

    [XmlRoot(IsNullable = false)]
    public class AccessPersonRQ
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
        public AccessPersonShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AccessPersonShareMarketIndRQ ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string NameType;
    }

    public enum AccessPersonShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum AccessPersonShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class AccessCommentRQ
    {

        [XmlAttribute()]
        public string Language;

        [XmlText()]
        public string Value;
    }

    public enum AccessActionTypeRQ
    {

        Create,

        Read,

        Update,

        Delete
    }

    [XmlRoot(IsNullable = false)]
    public class Accesses
    {

        [XmlElement("Access")]
        public AccessRQ[] Access;

        [XmlAttribute()]
        public AccessesShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AccessesShareMarketIndRQ ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public DateTime CreateDateTime;

        [XmlIgnore()]
        public bool CreateDateTimeSpecified;
    }

    public enum AccessesShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum AccessesShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
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

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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
    public class Text
    {

        [XmlAttribute()]
        public bool Formatted;

        [XmlIgnore()]
        public bool FormattedSpecified;

        [XmlAttribute()]
        public string Language;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class ListItemRQ
    {

        [XmlAttribute()]
        public bool Formatted;

        [XmlIgnore()]
        public bool FormattedSpecified;

        [XmlAttribute()]
        public string Language;

        [XmlAttribute("ListItem")]
        public int ListItem1;

        [XmlIgnore()]
        public bool ListItem1Specified;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class AdditionalDetails
    {

        [XmlElement("AdditionalDetail")]
        public AdditionalDetail[] AdditionalDetail;
    }

    [XmlRoot(IsNullable = false)]
    public class AdditionalGuestAmount
    {

        public Amount Amount;

        [XmlElement("AddlGuestAmtDescription")]
        public AddlGuestAmtDescription[] AddlGuestAmtDescription;

        [XmlAttribute()]
        public int MaxAdditionalGuests;

        [XmlIgnore()]
        public bool MaxAdditionalGuestsSpecified;

        [XmlAttribute()]
        public string AgeQualifyingCode;

        [XmlAttribute()]
        public int MinAge;

        [XmlIgnore()]
        public bool MinAgeSpecified;

        [XmlAttribute()]
        public int MaxAge;

        [XmlIgnore()]
        public bool MaxAgeSpecified;

        [XmlAttribute()]
        public AdditionalGuestAmountAgeTimeUnit AgeTimeUnit;

        [XmlIgnore()]
        public bool AgeTimeUnitSpecified;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public double Percent;

        [XmlIgnore()]
        public bool PercentSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class Amount
    {

        public Taxes Taxes;

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
    public class Taxes
    {

        [XmlElement("Tax")]
        public Tax[] Tax;

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
    public class Tax
    {

        [XmlElement("TaxDescription")]
        public TaxDescriptionRQ[] TaxDescription;

        [XmlAttribute()]
        public TaxTypeRQ Type;

        [XmlIgnore()]
        public bool TypeSpecified;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public double Percent;

        [XmlIgnore()]
        public bool PercentSpecified;

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
    public class TaxDescriptionRQ
    {

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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

    public enum TaxTypeRQ
    {

        Inclusive,

        Exclusive,

        Cumulative
    }

    [XmlRoot(IsNullable = false)]
    public class AddlGuestAmtDescription
    {

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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

    public enum AdditionalGuestAmountAgeTimeUnit
    {

        Year,

        Month,

        Week,

        Day,

        Hour,

        Second,

        FullDuration
    }

    [XmlRoot(IsNullable = false)]
    public class AdditionalGuestAmounts
    {

        [XmlElement("AdditionalGuestAmount")]
        public AdditionalGuestAmount[] AdditionalGuestAmount;
    }

    [XmlRoot(IsNullable = false)]
    public class AddressInfo
    {

        public StreetNmbrRQ StreetNmbr;

        public string BldgRoom;

        [XmlElement("AddressLine")]
        public string[] AddressLine;

        public string CityName;

        public string PostalCode;

        public string County;

        public StateProvRQ StateProv;

        public CountryNameRQ CountryName;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        [XmlAttribute()]
        public AddressInfoShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressInfoShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public string UseType;
    }

    public enum AddressInfoShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum AddressInfoShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class AddressPref
    {

        public Address Address;

        [XmlAttribute()]
        public AddressPrefShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressPrefShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;
    }

    public enum AddressPrefShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum AddressPrefShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class Affiliations
    {

        [XmlElement("Organization")]
        public Organization[] Organization;

        [XmlElement("Employer")]
        public Employer[] Employer;

        [XmlElement("TravelArranger")]
        public TravelArranger[] TravelArranger;

        [XmlElement("TravelClub")]
        public TravelClub[] TravelClub;

        [XmlElement("Insurance")]
        public Insurance[] Insurance;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public AffiliationsShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AffiliationsShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class Organization
    {

        public OrgMemberName OrgMemberName;

        public OrgName OrgName;

        [XmlElement("RelatedOrgName")]
        public RelatedOrgName[] RelatedOrgName;

        [XmlElement("TravelArranger")]
        public TravelArranger[] TravelArranger;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public OrganizationShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public OrganizationShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;

        [XmlAttribute()]
        public OrganizationOfficeType OfficeType;

        [XmlIgnore()]
        public bool OfficeTypeSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class OrgMemberName
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
        public OrgMemberNameShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public OrgMemberNameShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string NameType;

        [XmlAttribute()]
        public string ID;

        [XmlAttribute()]
        public string Level;

        [XmlAttribute()]
        public string Title;
    }

    public enum OrgMemberNameShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum OrgMemberNameShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class OrgName
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
    public class RelatedOrgName
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
    public class TravelArranger
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
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public TravelArrangerShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public TravelArrangerShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string TravelArrangerType;

        [XmlAttribute()]
        public string RPH;

        [XmlText()]
        public string Value;
    }

    public enum TravelArrangerShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum TravelArrangerShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum OrganizationShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum OrganizationShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum OrganizationOfficeType
    {

        Main,

        Field,

        Division,

        Regional,

        Remote
    }

    [XmlRoot(IsNullable = false)]
    public class Employer
    {

        public CompanyName CompanyName;

        [XmlElement("RelatedEmployer")]
        public RelatedEmployer[] RelatedEmployer;

        [XmlElement("EmployeeInfo")]
        public EmployeeInfoRQ[] EmployeeInfo;

        [XmlElement("InternalRefNmbr")]
        public InternalRefNmbr[] InternalRefNmbr;

        [XmlElement("TravelArranger")]
        public TravelArranger[] TravelArranger;

        [XmlElement("LoyaltyProgram")]
        public LoyaltyProgram[] LoyaltyProgram;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public EmployerOfficeType OfficeType;

        [XmlIgnore()]
        public bool OfficeTypeSpecified;

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
    public class RelatedEmployer
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
    public class EmployeeInfoRQ
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
    public class InternalRefNmbr
    {

        [XmlAttribute()]
        public string Language;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class LoyaltyProgram
    {

        [XmlAttribute()]
        public string ProgramCode;

        [XmlAttribute()]
        public LoyaltyProgramSingleVendorInd SingleVendorInd;

        [XmlIgnore()]
        public bool SingleVendorIndSpecified;

        [XmlAttribute()]
        public string LoyaltyLevel;

        [XmlAttribute()]
        public string RPH;

        [XmlText()]
        public string Value;
    }

    public enum LoyaltyProgramSingleVendorInd
    {

        SingleVndr,

        Alliance
    }

    public enum EmployerOfficeType
    {

        Main,

        Field,

        Division,

        Regional,

        Remote
    }

    [XmlRoot(IsNullable = false)]
    public class TravelClub
    {

        public TravelClubName TravelClubName;

        public ClubMemberName ClubMemberName;

        [XmlAttribute()]
        public TravelClubShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public TravelClubShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

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
    public class TravelClubName
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
    public class ClubMemberName
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
        public ClubMemberNameShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public ClubMemberNameShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string NameType;

        [XmlAttribute()]
        public string ID;
    }

    public enum ClubMemberNameShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum ClubMemberNameShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum TravelClubShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum TravelClubShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class Insurance
    {

        public InsuredName InsuredName;

        public InsuranceCompany InsuranceCompany;

        public Underwriter Underwriter;

        [XmlAttribute()]
        public InsuranceShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public InsuranceShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string InsuranceType;

        [XmlAttribute()]
        public string PolicyNumber;

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
    public class InsuredName
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
        public InsuredNameShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public InsuredNameShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string NameType;
    }

    public enum InsuredNameShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum InsuredNameShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class InsuranceCompany
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
    public class Underwriter
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

    public enum InsuranceShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum InsuranceShareMarketInd
    {

        Yes,

        No,

        Inherit
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

    public enum AffiliationsShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum AffiliationsShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class Agreements
    {

        [XmlElement("Certification")]
        public Certification[] Certification;

        [XmlElement("AllianceConsortium")]
        public AllianceConsortium[] AllianceConsortium;

        [XmlElement("CommissionInfo")]
        public CommissionInfo[] CommissionInfo;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public AgreementsShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AgreementsShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class Certification
    {

        [XmlAttribute()]
        public string Language;

        [XmlAttribute()]
        public string ID;

        [XmlAttribute()]
        public CertificationSingleVendorInd SingleVendorInd;

        [XmlIgnore()]
        public bool SingleVendorIndSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;

        [XmlText()]
        public string Value;
    }

    public enum CertificationSingleVendorInd
    {

        SingleVndr,

        Alliance
    }

    [XmlRoot(IsNullable = false)]
    public class AllianceConsortium
    {

        [XmlElement("AllianceMember")]
        public AllianceMember[] AllianceMember;

        [XmlAttribute()]
        public string ID;

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
    public class AllianceMember
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
        public string MemberCode;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class CommissionInfo
    {

        [XmlAttribute()]
        public string Language;

        [XmlAttribute()]
        public CommissionInfoShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public CommissionInfoShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CommissionPlanCode;

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

    public enum CommissionInfoShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum CommissionInfoShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum AgreementsShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum AgreementsShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class AirlinePref
    {

        [XmlElement("LoyaltyPref")]
        public LoyaltyPref[] LoyaltyPref;

        [XmlElement("VendorPref")]
        public VendorPref[] VendorPref;

        [XmlElement("PaymentFormPref")]
        public PaymentFormPref[] PaymentFormPref;

        [XmlElement("AirportOriginPref")]
        public AirportOriginPref[] AirportOriginPref;

        [XmlElement("AirportRoutePref")]
        public AirportRoutePref[] AirportRoutePref;

        [XmlElement("FareRestrictPref")]
        public FareRestrictPref[] FareRestrictPref;

        [XmlElement("FlightTypePref")]
        public FlightTypePref[] FlightTypePref;

        [XmlElement("EquipPref")]
        public EquipPref[] EquipPref;

        [XmlElement("CabinPref")]
        public CabinPref[] CabinPref;

        [XmlElement("SeatPref")]
        public SeatPref[] SeatPref;

        [XmlElement("TicketDistribPref")]
        public TicketDistribPref[] TicketDistribPref;

        [XmlElement("MealPref")]
        public MealPref[] MealPref;

        [XmlElement("SpecRequestPref")]
        public SpecRequestPref[] SpecRequestPref;

        [XmlElement("SSR_Pref")]
        public SSR_Pref[] SSR_Pref;

        public TPA_Extensions TPA_Extensions;

        [XmlElement("MediaEntertainPref")]
        public MediaEntertainPref[] MediaEntertainPref;

        [XmlElement("PetInfoPref")]
        public PetInfoPref[] PetInfoPref;

        [XmlAttribute()]
        public AirlinePrefShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AirlinePrefShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(AirlinePrefPreferLevel.Preferred)]
        public AirlinePrefPreferLevel PreferLevel = AirlinePrefPreferLevel.Preferred;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool SmokingAllowed = false;

        [XmlAttribute()]
        public string PassengerTypeCode;

        [XmlAttribute()]
        public AirlinePrefAirTicketType AirTicketType;

        [XmlIgnore()]
        public bool AirTicketTypeSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class LoyaltyPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(LoyaltyPrefPreferLevel.Preferred)]
        public LoyaltyPrefPreferLevel PreferLevel = LoyaltyPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string RPH;

        [XmlText()]
        public string Value;
    }

    public enum LoyaltyPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
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
    public class PaymentFormPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PaymentFormPrefPreferLevel.Preferred)]
        public PaymentFormPrefPreferLevel PreferLevel = PaymentFormPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string RPH;

        [XmlText()]
        public string Value;
    }

    public enum PaymentFormPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class AirportOriginPref
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(AirportOriginPrefPreferLevel.Preferred)]
        public AirportOriginPrefPreferLevel PreferLevel = AirportOriginPrefPreferLevel.Preferred;

        [XmlText()]
        public string Value;
    }

    public enum AirportOriginPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class AirportRoutePref
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(AirportRoutePrefPreferLevel.Preferred)]
        public AirportRoutePrefPreferLevel PreferLevel = AirportRoutePrefPreferLevel.Preferred;

        [XmlText()]
        public string Value;
    }

    public enum AirportRoutePrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class FareRestrictPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(FareRestrictPrefPreferLevel.Preferred)]
        public FareRestrictPrefPreferLevel PreferLevel = FareRestrictPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string FareRestriction;
    }

    public enum FareRestrictPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
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

        Economy
    }

    [XmlRoot(IsNullable = false)]
    public class SeatPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SeatPrefPreferLevel.Preferred)]
        public SeatPrefPreferLevel PreferLevel = SeatPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string SeatNumber;

        [XmlAttribute()]
        public string SeatPreference;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool SmokingAllowed = false;
    }

    public enum SeatPrefPreferLevel
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
    public class MealPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(MealPrefPreferLevel.Preferred)]
        public MealPrefPreferLevel PreferLevel = MealPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public MealPrefMealType MealType;

        [XmlIgnore()]
        public bool MealTypeSpecified;

        [XmlAttribute()]
        public string FavoriteFood;

        [XmlAttribute()]
        public string Beverage;

        [XmlText()]
        public string Value;
    }

    public enum MealPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum MealPrefMealType
    {

        AVML,

        BBML,

        BLML,

        CHML,

        DBML,

        FPML,

        GFML,

        HFML,

        HNML,

        KSML,

        LCML,

        LFML,

        LPML,

        LSML,

        MOML,

        NLML,

        ORML,

        PRML,

        RVML,

        SFML,

        SPML,

        VGML,

        VLML
    }

    [XmlRoot(IsNullable = false)]
    public class SpecRequestPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SpecRequestPrefPreferLevel.Preferred)]
        public SpecRequestPrefPreferLevel PreferLevel = SpecRequestPrefPreferLevel.Preferred;

        [XmlText()]
        public string Value;
    }

    public enum SpecRequestPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class SSR_Pref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SSR_PrefPreferLevel.Preferred)]
        public SSR_PrefPreferLevel PreferLevel = SSR_PrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string SSR_Code;
    }

    public enum SSR_PrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class MediaEntertainPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(MediaEntertainPrefPreferLevel.Preferred)]
        public MediaEntertainPrefPreferLevel PreferLevel = MediaEntertainPrefPreferLevel.Preferred;

        [XmlText()]
        public string Value;
    }

    public enum MediaEntertainPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class PetInfoPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PetInfoPrefPreferLevel.Preferred)]
        public PetInfoPrefPreferLevel PreferLevel = PetInfoPrefPreferLevel.Preferred;

        [XmlText()]
        public string Value;
    }

    public enum PetInfoPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum AirlinePrefShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum AirlinePrefShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum AirlinePrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum AirlinePrefAirTicketType
    {

        eTicket,

        Paper
    }

    [XmlRoot(IsNullable = false)]
    public class Amenities
    {

        [XmlElement("Amenity")]
        public AmenityRQ[] Amenity;
    }

    [XmlRoot(IsNullable = false)]
    public class AmenityRQ
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(AmenityPreferLevelRQ.Preferred)]
        public AmenityPreferLevelRQ PreferLevel = AmenityPreferLevelRQ.Preferred;

        [XmlAttribute()]
        public string RoomAmenity;

        [XmlAttribute()]
        public int Quantity;

        [XmlIgnore()]
        public bool QuantitySpecified;

        [XmlText()]
        public string Value;
    }

    public enum AmenityPreferLevelRQ
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class AmountPercentRQ
    {

        public Taxes Taxes;

        [XmlAttribute()]
        public bool TaxInclusive;

        [XmlIgnore()]
        public bool TaxInclusiveSpecified;

        [XmlAttribute()]
        public bool FeesInclusive;

        [XmlIgnore()]
        public bool FeesInclusiveSpecified;

        [XmlAttribute()]
        public int NmbrOfNights;

        [XmlIgnore()]
        public bool NmbrOfNightsSpecified;

        [XmlAttribute()]
        public AmountPercentBasisTypeRQ BasisType;

        [XmlIgnore()]
        public bool BasisTypeSpecified;

        [XmlAttribute()]
        public double Percent;

        [XmlIgnore()]
        public bool PercentSpecified;

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

    public enum AmountPercentBasisTypeRQ
    {

        FullStay,

        Nights,

        FirstLast
    }

    [XmlRoot(IsNullable = false)]
    public class ArrivalTransport
    {

        public TransportInfo TransportInfo;
    }

    [XmlRoot(IsNullable = false)]
    public class TransportInfo
    {

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public string ID;

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        public DateTime Time;

        [XmlIgnore()]
        public bool TimeSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class AssociatedQuantity
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
    }

    [XmlRoot(IsNullable = false)]
    public class Award
    {

        [XmlAttribute()]
        public string Provider;

        [XmlAttribute()]
        public string Rating;
    }

    [XmlRoot(IsNullable = false)]
    public class BaseRQ
    {

        public Taxes Taxes;

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
    public class BasicPropertyInfo
    {

        [XmlArrayItem(typeof(VendorMessage), IsNullable = false)]
        public VendorMessage[] VendorMessages;

        public Position Position;

        public Address Address;

        [XmlArrayItem(IsNullable = false)]
        public ContactNumber[] ContactNumbers;

        [XmlElement("Award")]
        public Award[] Award;

        public RelativePosition RelativePosition;

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
        public string AreaID;
    }

    [XmlRoot(IsNullable = false)]
    public class VendorMessage
    {

        [XmlElement("SubSection")]
        public SubSection[] SubSection;

        [XmlAttribute()]
        public string Title;

        [XmlAttribute()]
        public string Language;

        [XmlAttribute()]
        public string InfoType;
    }

    [XmlRoot(IsNullable = false)]
    public class SubSection
    {

        [XmlElement("Paragraph")]
        public Paragraph[] Paragraph;

        [XmlAttribute()]
        public string SubTitle;

        [XmlAttribute()]
        public string SubCode;

        [XmlAttribute()]
        public int SubSectionNumber;

        [XmlIgnore()]
        public bool SubSectionNumberSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class Paragraph
    {

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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
    public class ContactNumber
    {

        [XmlAttribute()]
        public ContactNumberShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public ContactNumberShareMarketInd ShareMarketInd;

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

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public string PhoneUseType;
    }

    public enum ContactNumberShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum ContactNumberShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class RelativePosition
    {

        [XmlArrayItem(IsNullable = false)]
        public Transportation[] Transportations;

        [XmlAttribute()]
        public string Direction;

        [XmlAttribute()]
        public string Distance;

        [XmlAttribute()]
        public RelativePositionDistanceUnitName DistanceUnitName;

        [XmlIgnore()]
        public bool DistanceUnitNameSpecified;

        [XmlAttribute()]
        public bool Nearest;

        [XmlIgnore()]
        public bool NearestSpecified;

        [XmlAttribute()]
        public string IndexPointCode;

        [XmlAttribute()]
        public string Name;
    }

    [XmlRoot(IsNullable = false)]
    public class Transportation
    {

        [XmlArrayItem(typeof(DescriptionRQ), IsNullable = false)]
        public DescriptionRQ[] Descriptions;

        [XmlAttribute()]
        public string NotificationRequired;

        [XmlAttribute()]
        public string TransportationCode;

        [XmlAttribute()]
        public string CodeDetail;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool Removal = false;

        [XmlAttribute()]
        public string Description;

        [XmlAttribute()]
        public string TypicalTravelTime;

        [XmlAttribute()]
        public double Amount;

        [XmlIgnore()]
        public bool AmountSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class DescriptionRQ
    {

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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

    public enum RelativePositionDistanceUnitName
    {

        Mile,

        Km
    }

    [XmlRoot(IsNullable = false)]
    public class BedTypePref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(BedTypePrefPreferLevel.Preferred)]
        public BedTypePrefPreferLevel PreferLevel = BedTypePrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string BedType;

        [XmlText()]
        public string Value;
    }

    public enum BedTypePrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
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
    public class BusinessLocale
    {

        public StreetNmbrRQ StreetNmbr;

        public string BldgRoom;

        [XmlElement("AddressLine")]
        public string[] AddressLine;

        public string CityName;

        public string PostalCode;

        public string County;

        public StateProvRQ StateProv;

        public CountryNameRQ CountryName;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        [XmlAttribute()]
        public BusinessLocaleShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public BusinessLocaleShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string Type;
    }

    public enum BusinessLocaleShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum BusinessLocaleShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class BusinessSrvcPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(BusinessSrvcPrefPreferLevel.Preferred)]
        public BusinessSrvcPrefPreferLevel PreferLevel = BusinessSrvcPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string BusinessSrvcType;

        [XmlText()]
        public string Value;
    }

    public enum BusinessSrvcPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class CancelPenalties
    {

        [XmlElement("CancelPenalty")]
        public CancelPenalty[] CancelPenalty;
    }

    [XmlRoot(IsNullable = false)]
    public class CancelPenalty
    {

        public DeadlineRQ Deadline;

        public AmountPercentRQ AmountPercent;

        [XmlElement("PenaltyDescription")]
        public PenaltyDescription[] PenaltyDescription;

        [XmlAttribute()]
        public string ConfirmClassCode;

        [XmlAttribute()]
        public string PolicyCode;

        [XmlAttribute()]
        public bool NonRefundable;

        [XmlIgnore()]
        public bool NonRefundableSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class DeadlineRQ
    {

        [XmlAttribute()]
        public DateTime AbsoluteDeadline;

        [XmlIgnore()]
        public bool AbsoluteDeadlineSpecified;

        [XmlAttribute()]
        public DeadlineOffsetTimeUnitRQ OffsetTimeUnit;

        [XmlIgnore()]
        public bool OffsetTimeUnitSpecified;

        [XmlAttribute()]
        public int OffsetUnitMultiplier;

        [XmlIgnore()]
        public bool OffsetUnitMultiplierSpecified;

        [XmlAttribute()]
        public DeadlineOffsetDropTimeRQ OffsetDropTime;

        [XmlIgnore()]
        public bool OffsetDropTimeSpecified;
    }

    public enum DeadlineOffsetTimeUnitRQ
    {

        Year,

        Month,

        Week,

        Day,

        Hour,

        Second,

        FullDuration
    }

    public enum DeadlineOffsetDropTimeRQ
    {

        BeforeArrival,

        AfterBooking
    }

    [XmlRoot(IsNullable = false)]
    public class PenaltyDescription
    {

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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
    public class CancelPolicies
    {

        [XmlElement("CancelPenalty")]
        public CancelPenalty[] CancelPenalty;
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
    public class Comment
    {

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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
        public string CommentOriginatorCode;

        [XmlAttribute()]
        public bool GuestViewable;

        [XmlIgnore()]
        public bool GuestViewableSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class Comments
    {

        [XmlElement("Comment")]
        public Comment[] Comment;
    }

    [XmlRoot(IsNullable = false)]
    public class Commission
    {

        public UniqueIDRQ UniqueID;

        public CommissionableAmount CommissionableAmount;

        public PrepaidAmount PrepaidAmount;

        public FlatCommission FlatCommission;

        public CommissionPayableAmount CommissionPayableAmount;

        public Comment Comment;

        [XmlAttribute()]
        public CommissionStatusType StatusType;

        [XmlIgnore()]
        public bool StatusTypeSpecified;

        [XmlAttribute()]
        public double Percent;

        [XmlIgnore()]
        public bool PercentSpecified;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public int DecimalPlaces;

        [XmlIgnore()]
        public bool DecimalPlacesSpecified;

        [XmlAttribute()]
        public string ReasonCode;

        [XmlAttribute()]
        public string BillToID;
    }

    [XmlRoot(IsNullable = false)]
    public class UniqueIDRQ
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
    public class CommissionableAmount
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
    public class PrepaidAmount
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
    public class FlatCommission
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
    public class CommissionPayableAmount
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

    public enum CommissionStatusType
    {

        Full,

        Partial,

        [XmlEnum("Non-paying")]
        Nonpaying,

        [XmlEnum("No-show")]
        Noshow,

        Adjustment
    }

    [XmlRoot(IsNullable = false)]
    public class CommonPref
    {

        [XmlElement("NamePref")]
        public NamePref[] NamePref;

        [XmlElement("PhonePref")]
        public PhonePref[] PhonePref;

        [XmlElement("AddressPref")]
        public AddressPref[] AddressPref;

        [XmlElement("PaymentFormPref")]
        public PaymentFormPref[] PaymentFormPref;

        [XmlElement("InterestPref")]
        public InterestPref[] InterestPref;

        [XmlElement("InsurancePref")]
        public InsurancePref[] InsurancePref;

        [XmlElement("SeatingPref")]
        public SeatingPref[] SeatingPref;

        [XmlElement("TicketDistribPref")]
        public TicketDistribPref[] TicketDistribPref;

        [XmlElement("MediaEntertainPref")]
        public MediaEntertainPref[] MediaEntertainPref;

        [XmlElement("PetInfoPref")]
        public PetInfoPref[] PetInfoPref;

        [XmlElement("MealPref")]
        public MealPref[] MealPref;

        [XmlElement("LoyaltyPref")]
        public LoyaltyPref[] LoyaltyPref;

        [XmlElement("SpecRequestPref")]
        public SpecRequestPref[] SpecRequestPref;

        [XmlElement("RelatedTravelerPref")]
        public RelatedTravelerPref[] RelatedTravelerPref;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public CommonPrefShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public CommonPrefShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool SmokingAllowed = false;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;
    }

    [XmlRoot(IsNullable = false)]
    public class NamePref
    {

        public UniqueIDRQ UniqueID;

        public PersonNameRQ PersonName;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(NamePrefPreferLevel.Preferred)]
        public NamePrefPreferLevel PreferLevel = NamePrefPreferLevel.Preferred;
    }

    [XmlRoot(IsNullable = false)]
    public class PersonNameRQ
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
        public PersonNameShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public PersonNameShareMarketIndRQ ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string NameType;

        [XmlAttribute()]
        public bool PartialName;

        [XmlIgnore()]
        public bool PartialNameSpecified;
    }

    public enum PersonNameShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum PersonNameShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum NamePrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class PhonePref
    {

        public Telephone Telephone;
    }

    [XmlRoot(IsNullable = false)]
    public class Telephone
    {

        [XmlAttribute()]
        public TelephoneShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public TelephoneShareMarketIndRQ ShareMarketInd;

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

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public string PhoneUseType;
    }

    public enum TelephoneShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum TelephoneShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class InterestPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(InterestPrefPreferLevel.Preferred)]
        public InterestPrefPreferLevel PreferLevel = InterestPrefPreferLevel.Preferred;

        [XmlText()]
        public string Value;
    }

    public enum InterestPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class InsurancePref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(InsurancePrefPreferLevel.Preferred)]
        public InsurancePrefPreferLevel PreferLevel = InsurancePrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string RPH;

        [XmlText()]
        public string Value;
    }

    public enum InsurancePrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class SeatingPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SeatingPrefPreferLevel.Preferred)]
        public SeatingPrefPreferLevel PreferLevel = SeatingPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string SeatDirection;

        [XmlAttribute()]
        public string SeatLocation;

        [XmlAttribute()]
        public string SeatPosition;

        [XmlAttribute()]
        public string SeatRow;

        [XmlText()]
        public string Value;
    }

    public enum SeatingPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class RelatedTravelerPref
    {

        public UniqueIDRQ UniqueID;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(RelatedTravelerPrefPreferLevel.Preferred)]
        public RelatedTravelerPrefPreferLevel PreferLevel = RelatedTravelerPrefPreferLevel.Preferred;
    }

    public enum RelatedTravelerPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum CommonPrefShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum CommonPrefShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class CompanyInfo
    {

        [XmlElement("CompanyName")]
        public CompanyName[] CompanyName;

        [XmlElement("AddressInfo")]
        public AddressInfo[] AddressInfo;

        [XmlElement("TelephoneInfo")]
        public TelephoneInfo[] TelephoneInfo;

        [XmlElement("Email")]
        public EmailRQ[] Email;

        [XmlElement("URL", DataType = "anyURI")]
        public string[] URL;

        [XmlElement("BusinessLocale")]
        public BusinessLocale[] BusinessLocale;

        [XmlElement("PaymentForm")]
        public PaymentFormRQ[] PaymentForm;

        [XmlElement("ContactPerson")]
        public ContactPersonRQ[] ContactPerson;

        [XmlElement("TravelArranger")]
        public TravelArranger[] TravelArranger;

        [XmlElement("LoyaltyProgram")]
        public LoyaltyProgram[] LoyaltyProgram;
    }

    [XmlRoot(IsNullable = false)]
    public class TelephoneInfo
    {

        [XmlAttribute()]
        public TelephoneInfoShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public TelephoneInfoShareMarketInd ShareMarketInd;

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

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public string PhoneUseType;
    }

    public enum TelephoneInfoShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum TelephoneInfoShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class EmailRQ
    {

        [XmlAttribute()]
        public EmailShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public EmailShareMarketIndRQ ShareMarketInd;

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

    public enum EmailShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum EmailShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentFormRQ
    {

        public PaymentCardRQ PaymentCard;

        public BankAcctRQ BankAcct;

        public DirectBill DirectBill;

        public Voucher Voucher;

        public LoyaltyRedemption LoyaltyRedemption;

        public MiscChargeOrder MiscChargeOrder;

        public Cash Cash;

        [XmlAttribute()]
        public PaymentFormShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public PaymentFormShareMarketIndRQ ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CostCenterID;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public PaymentFormPaymentTransactionTypeCodeRQ PaymentTransactionTypeCode;

        [XmlIgnore()]
        public bool PaymentTransactionTypeCodeSpecified;
    }

    public enum PaymentFormShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum PaymentFormShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum PaymentFormPaymentTransactionTypeCodeRQ
    {

        charge,

        reserve
    }

    [XmlRoot(IsNullable = false)]
    public class ContactPersonRQ
    {

        public PersonNameRQ PersonName;

        [XmlElement("Telephone")]
        public Telephone[] Telephone;

        [XmlElement("Address")]
        public Address[] Address;

        [XmlElement("Email")]
        public EmailRQ[] Email;

        [XmlElement("URL", DataType = "anyURI")]
        public string[] URL;

        [XmlElement("CompanyName")]
        public CompanyName[] CompanyName;

        [XmlElement("EmployeeInfo")]
        public EmployeeInfoRQ[] EmployeeInfo;

        [XmlAttribute()]
        public ContactPersonShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public ContactPersonShareMarketIndRQ ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public string ContactType;

        [XmlAttribute()]
        public string Relation;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool EmergencyFlag = false;

        [XmlAttribute()]
        public string RPH;
    }

    public enum ContactPersonShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum ContactPersonShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class ContactNumbers
    {

        [XmlElement("ContactNumber")]
        public ContactNumber[] ContactNumber;
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
    public class CustLoyalty
    {

        [XmlAttribute()]
        public CustLoyaltyShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public CustLoyaltyShareMarketIndRQ ShareMarketInd;

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
        public CustLoyaltySingleVendorIndRQ SingleVendorInd;

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

    public enum CustLoyaltyShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum CustLoyaltyShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum CustLoyaltySingleVendorIndRQ
    {

        SingleVndr,

        Alliance
    }

    [XmlRoot(IsNullable = false)]
    public class Customer
    {

        public PersonNameRQ PersonName;

        [XmlElement("Telephone")]
        public Telephone[] Telephone;

        [XmlElement("Email")]
        public EmailRQ[] Email;

        [XmlElement("Address")]
        public Address[] Address;

        [XmlElement("URL", DataType = "anyURI")]
        public string[] URL;

        [XmlElement("CitizenCountryName")]
        public CitizenCountryName[] CitizenCountryName;

        [XmlElement("PhysChallName")]
        public string[] PhysChallName;

        [XmlElement("PetInfo")]
        public string[] PetInfo;

        [XmlElement("PaymentForm")]
        public PaymentFormRQ[] PaymentForm;

        [XmlElement("RelatedTraveler")]
        public RelatedTravelerRQ[] RelatedTraveler;

        [XmlElement("ContactPerson")]
        public ContactPersonRQ[] ContactPerson;

        [XmlElement("Document")]
        public Document[] Document;

        [XmlElement("CustLoyalty")]
        public CustLoyalty[] CustLoyalty;

        [XmlElement("EmployeeInfo")]
        public EmployeeInfoRQ[] EmployeeInfo;

        [XmlAttribute()]
        public CustomerGenderRQ Gender;

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

        [XmlAttribute()]
        public int DecimalPlaces;

        [XmlIgnore()]
        public bool DecimalPlacesSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class RelatedTravelerRQ
    {

        public UniqueIDRQ UniqueID;

        public PersonNameRQ PersonName;

        [XmlAttribute()]
        public RelatedTravelerShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public RelatedTravelerShareMarketIndRQ ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string Relation;
    }

    public enum RelatedTravelerShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum RelatedTravelerShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class Document
    {

        public string DocHolderName;

        [XmlElement("DocLimitations")]
        public string[] DocLimitations;

        [XmlAttribute()]
        public DocumentShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public DocumentShareMarketIndRQ ShareMarketInd;

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
        public DocumentGenderRQ Gender;

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

    public enum DocumentShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum DocumentShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum DocumentGenderRQ
    {

        Male,

        Female,

        Unknown
    }

    public enum CustomerGenderRQ
    {

        Male,

        Female,

        Unknown
    }

    [XmlRoot(IsNullable = false)]
    public class DateWindowRange
    {

        [XmlAttribute()]
        public string WindowBefore;

        [XmlAttribute()]
        public string WindowAfter;

        [XmlAttribute()]
        public bool CrossDateAllowedIndicator;

        [XmlIgnore()]
        public bool CrossDateAllowedIndicatorSpecified;

        [XmlText(DataType = "date")]
        public DateTime Value;
    }

    [XmlRoot(IsNullable = false)]
    public class DepartureTransport
    {

        public TransportInfo TransportInfo;
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

        public AmountPercentRQ AmountPercent;

        public DeadlineRQ Deadline;

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

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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
    public class Descriptions
    {

        [XmlElement("Description")]
        public DescriptionRQ[] Description;
    }

    [XmlRoot(IsNullable = false)]
    public class Discount
    {

        public Taxes Taxes;

        public DiscountReason DiscountReason;

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

        [XmlAttribute()]
        public bool TaxInclusive;

        [XmlIgnore()]
        public bool TaxInclusiveSpecified;

        [XmlAttribute()]
        public double Percent;

        [XmlIgnore()]
        public bool PercentSpecified;

        [XmlAttribute()]
        public string DiscountCode;
    }

    [XmlRoot(IsNullable = false)]
    public class DiscountReason
    {

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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
    public class EndLocation
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlAttribute()]
        public DateTime AssociatedDateTime;

        [XmlIgnore()]
        public bool AssociatedDateTimeSpecified;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class Feature
    {

        [XmlElement("Description")]
        public DescriptionRQ[] Description;

        [XmlAttribute()]
        public string RoomAmenity;

        [XmlAttribute()]
        public int Quantity;

        [XmlIgnore()]
        public bool QuantitySpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class Features
    {

        [XmlElement("Feature")]
        public Feature[] Feature;
    }

    [XmlRoot(IsNullable = false)]
    public class Fee
    {

        public Taxes Taxes;

        [XmlElement("Description")]
        public DescriptionRQ[] Description;

        [XmlAttribute()]
        public bool TaxInclusive;

        [XmlIgnore()]
        public bool TaxInclusiveSpecified;

        [XmlAttribute()]
        public FeeType Type;

        [XmlIgnore()]
        public bool TypeSpecified;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public double Percent;

        [XmlIgnore()]
        public bool PercentSpecified;

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

    public enum FeeType
    {

        Inclusive,

        Exclusive,

        Cumulative
    }

    [XmlRoot(IsNullable = false)]
    public class Fees
    {

        [XmlElement("Fee")]
        public Fee[] Fee;
    }

    [XmlRoot(IsNullable = false)]
    public class FoodSrvcPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(FoodSrvcPrefPreferLevel.Preferred)]
        public FoodSrvcPrefPreferLevel PreferLevel = FoodSrvcPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string FoodSrvcType;

        [XmlText()]
        public string Value;
    }

    public enum FoodSrvcPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class Guarantee
    {

        [XmlArrayItem(IsNullable = false)]
        public GuaranteeAccepted[] GuaranteesAccepted;

        public DeadlineRQ Deadline;

        [XmlArrayItem(IsNullable = false)]
        public Comment[] Comments;

        [XmlElement("GuaranteeDescription")]
        public GuaranteeDescription[] GuaranteeDescription;

        [XmlAttribute()]
        public GuaranteeRetributionTypeRQ RetributionType;

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

        public PaymentCardRQ PaymentCard;

        public BankAcctRQ BankAcct;

        public DirectBill DirectBill;

        public Voucher Voucher;

        public LoyaltyRedemption LoyaltyRedemption;

        public MiscChargeOrder MiscChargeOrder;

        public Cash Cash;

        [XmlAttribute()]
        public GuaranteeAcceptedShareSynchIndRQ ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public GuaranteeAcceptedShareMarketIndRQ ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CostCenterID;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public GuaranteeAcceptedPaymentTransactionTypeCode PaymentTransactionTypeCode;

        [XmlIgnore()]
        public bool PaymentTransactionTypeCodeSpecified;

        [XmlAttribute()]
        public bool Default;

        [XmlIgnore()]
        public bool DefaultSpecified;
    }

    public enum GuaranteeAcceptedShareSynchIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum GuaranteeAcceptedShareMarketIndRQ
    {

        Yes,

        No,

        Inherit
    }

    public enum GuaranteeAcceptedPaymentTransactionTypeCode
    {

        charge,

        reserve
    }

    [XmlRoot(IsNullable = false)]
    public class GuaranteeDescription
    {

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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

    public enum GuaranteeRetributionTypeRQ
    {

        ResAutoCancelled,

        ResNotGuaranteed
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

        [XmlIgnore()]
        public bool CountSpecified;
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
    public class HotelChainPref
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
        [System.ComponentModel.DefaultValue(HotelChainPrefPreferLevel.Preferred)]
        public HotelChainPrefPreferLevel PreferLevel = HotelChainPrefPreferLevel.Preferred;

        [XmlText()]
        public string Value;
    }

    public enum HotelChainPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class HotelPref
    {

        [XmlElement("LoyaltyPref")]
        public LoyaltyPref[] LoyaltyPref;

        [XmlElement("PaymentFormPref")]
        public PaymentFormPref[] PaymentFormPref;

        [XmlElement("HotelChainPref")]
        public HotelChainPref[] HotelChainPref;

        [XmlElement("PropertyNamePref")]
        public PropertyNamePref[] PropertyNamePref;

        [XmlElement("PropertyLocationPref")]
        public PropertyLocationPref[] PropertyLocationPref;

        [XmlElement("PropertyTypePref")]
        public PropertyTypePref[] PropertyTypePref;

        [XmlElement("PropertyClassPref")]
        public PropertyClassPref[] PropertyClassPref;

        [XmlElement("PropertyAmenityPref")]
        public PropertyAmenityPref[] PropertyAmenityPref;

        [XmlElement("RoomAmenityPref")]
        public RoomAmenityPref[] RoomAmenityPref;

        [XmlElement("RoomLocationPref")]
        public RoomLocationPref[] RoomLocationPref;

        [XmlElement("BedTypePref")]
        public BedTypePref[] BedTypePref;

        [XmlElement("FoodSrvcPref")]
        public FoodSrvcPref[] FoodSrvcPref;

        [XmlElement("MediaEntertainPref")]
        public MediaEntertainPref[] MediaEntertainPref;

        [XmlElement("PetInfoPref")]
        public PetInfoPref[] PetInfoPref;

        [XmlElement("MealPref")]
        public MealPref[] MealPref;

        [XmlElement("RecreationSrvcPref")]
        public RecreationSrvcPref[] RecreationSrvcPref;

        [XmlElement("BusinessSrvcPref")]
        public BusinessSrvcPref[] BusinessSrvcPref;

        [XmlElement("PersonalSrvcPref")]
        public PersonalSrvcPref[] PersonalSrvcPref;

        [XmlElement("SecurityFeaturePref")]
        public SecurityFeaturePref[] SecurityFeaturePref;

        [XmlElement("PhysChallFeaturePref")]
        public PhysChallFeaturePref[] PhysChallFeaturePref;

        [XmlElement("SpecRequestPref")]
        public SpecRequestPref[] SpecRequestPref;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(HotelPrefPreferLevel.Preferred)]
        public HotelPrefPreferLevel PreferLevel = HotelPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public HotelPrefShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public HotelPrefShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool SmokingAllowed = false;

        [XmlAttribute()]
        public string RatePlanCode;

        [XmlAttribute()]
        public string HotelGuestType;
    }

    [XmlRoot(IsNullable = false)]
    public class PropertyNamePref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PropertyNamePrefPreferLevel.Preferred)]
        public PropertyNamePrefPreferLevel PreferLevel = PropertyNamePrefPreferLevel.Preferred;

        [XmlText()]
        public string Value;
    }

    public enum PropertyNamePrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class PropertyLocationPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PropertyLocationPrefPreferLevel.Preferred)]
        public PropertyLocationPrefPreferLevel PreferLevel = PropertyLocationPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string PropertyLocationType;

        [XmlText()]
        public string Value;
    }

    public enum PropertyLocationPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class PropertyTypePref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PropertyTypePrefPreferLevel.Preferred)]
        public PropertyTypePrefPreferLevel PreferLevel = PropertyTypePrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string PropertyType;

        [XmlText()]
        public string Value;
    }

    public enum PropertyTypePrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class PropertyClassPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PropertyClassPrefPreferLevel.Preferred)]
        public PropertyClassPrefPreferLevel PreferLevel = PropertyClassPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string PropertyClassType;

        [XmlText()]
        public string Value;
    }

    public enum PropertyClassPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class PropertyAmenityPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PropertyAmenityPrefPreferLevel.Preferred)]
        public PropertyAmenityPrefPreferLevel PreferLevel = PropertyAmenityPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string PropertyAmenityType;

        [XmlText()]
        public string Value;
    }

    public enum PropertyAmenityPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class RoomAmenityPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(RoomAmenityPrefPreferLevel.Preferred)]
        public RoomAmenityPrefPreferLevel PreferLevel = RoomAmenityPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string RoomAmenity;

        [XmlAttribute()]
        public int Quantity;

        [XmlIgnore()]
        public bool QuantitySpecified;

        [XmlText()]
        public string Value;
    }

    public enum RoomAmenityPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class RoomLocationPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(RoomLocationPrefPreferLevel.Preferred)]
        public RoomLocationPrefPreferLevel PreferLevel = RoomLocationPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string RoomLocationType;

        [XmlText()]
        public string Value;
    }

    public enum RoomLocationPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class RecreationSrvcPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(RecreationSrvcPrefPreferLevel.Preferred)]
        public RecreationSrvcPrefPreferLevel PreferLevel = RecreationSrvcPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string RecreationSrvcType;

        [XmlText()]
        public string Value;
    }

    public enum RecreationSrvcPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class PersonalSrvcPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PersonalSrvcPrefPreferLevel.Preferred)]
        public PersonalSrvcPrefPreferLevel PreferLevel = PersonalSrvcPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string PersonalSrvcType;

        [XmlText()]
        public string Value;
    }

    public enum PersonalSrvcPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class SecurityFeaturePref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SecurityFeaturePrefPreferLevel.Preferred)]
        public SecurityFeaturePrefPreferLevel PreferLevel = SecurityFeaturePrefPreferLevel.Preferred;

        [XmlText()]
        public string Value;
    }

    public enum SecurityFeaturePrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class PhysChallFeaturePref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(PhysChallFeaturePrefPreferLevel.Preferred)]
        public PhysChallFeaturePrefPreferLevel PreferLevel = PhysChallFeaturePrefPreferLevel.Preferred;

        [XmlAttribute()]
        public string PhysChallFeatureType;

        [XmlText()]
        public string Value;
    }

    public enum PhysChallFeaturePrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum HotelPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum HotelPrefShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum HotelPrefShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class HotelResModifies
    {

        [XmlElement("HotelResModify")]
        public HotelResModify[] HotelResModify;

        [XmlArrayItem(IsNullable = false)]
        public RoutingHop[] RoutingHops;

        public WrittenConfInst WrittenConfInst;
    }

    [XmlRoot(IsNullable = false)]
    public class HotelResModify
    {

        public UniqueIDRQ UniqueID;

        [XmlArrayItem(IsNullable = false)]
        public RoomStay[] RoomStays;

        [XmlArrayItem(IsNullable = false)]
        public Service[] Services;

        [XmlArrayItem(IsNullable = false)]
        public ResGuest[] ResGuests;

        public ResGlobalInfo ResGlobalInfo;

        public WrittenConfInst WrittenConfInst;

        public TPA_Extensions TPA_Extensions;

        public Verification Verification;

        [XmlAttribute()]
        public bool RoomStayReservation;

        [XmlIgnore()]
        public bool RoomStayReservationSpecified;

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
        public RatePlanRQ[] RatePlans;

        [XmlArrayItem(IsNullable = false)]
        public RoomRate[] RoomRates;

        public GuestCounts GuestCounts;

        public TimeSpan TimeSpan;

        [XmlElement("Guarantee")]
        public Guarantee[] Guarantee;

        [XmlArrayItem(IsNullable = false)]
        public RequiredPayment[] DepositPayments;

        [XmlArrayItem(IsNullable = false)]
        public CancelPenalty[] CancelPenalties;

        public Discount Discount;

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

        [XmlArrayItem(IsNullable = false)]
        public AmenityRQ[] Amenities;

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

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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
    public class RatePlanRQ
    {

        [XmlElement("Guarantee")]
        public Guarantee[] Guarantee;

        [XmlArrayItem(IsNullable = false)]
        public CancelPenalty[] CancelPenalties;

        public RatePlanDescriptionRQ RatePlanDescription;

        public RatePlanInclusions RatePlanInclusions;

        public Commission Commission;

        public MealsIncluded MealsIncluded;

        public RestrictionStatus RestrictionStatus;

        [XmlArrayItem(IsNullable = false)]
        public AdditionalDetail[] AdditionalDetails;

        [XmlAttribute()]
        public string BookingCode;

        [XmlAttribute()]
        public string RatePlanCode;

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;

        [XmlAttribute()]
        public RatePlanRateIndicatorRQ RateIndicator;

        [XmlIgnore()]
        public bool RateIndicatorSpecified;

        [XmlAttribute()]
        public string RatePlanType;

        [XmlAttribute()]
        public string RatePlanID;
    }

    [XmlRoot(IsNullable = false)]
    public class RatePlanDescriptionRQ
    {

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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
    public class RatePlanInclusions
    {

        public RatePlanInclusionDesciption RatePlanInclusionDesciption;

        [XmlAttribute()]
        public bool TaxInclusive;

        [XmlIgnore()]
        public bool TaxInclusiveSpecified;

        [XmlAttribute()]
        public bool ServiceFeeInclusive;

        [XmlIgnore()]
        public bool ServiceFeeInclusiveSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class RatePlanInclusionDesciption
    {

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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
    public class MealsIncluded
    {

        [XmlAttribute()]
        public bool Breakfast;

        [XmlIgnore()]
        public bool BreakfastSpecified;

        [XmlAttribute()]
        public bool Lunch;

        [XmlIgnore()]
        public bool LunchSpecified;

        [XmlAttribute()]
        public bool Dinner;

        [XmlIgnore()]
        public bool DinnerSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class RestrictionStatus
    {

        [XmlAttribute()]
        public RestrictionStatusRestriction Restriction;

        [XmlIgnore()]
        public bool RestrictionSpecified;

        [XmlAttribute()]
        public RestrictionStatusStatus Status;

        [XmlIgnore()]
        public bool StatusSpecified;
    }

    public enum RestrictionStatusRestriction
    {

        Master,

        Arrival,

        Departure,

        NonGuarantee,

        TravelAgent
    }

    public enum RestrictionStatusStatus
    {

        Open,

        Close,

        ClosedOnArrival,

        ClosedOnArrivalOnRequest,

        OnRequest
    }

    public enum RatePlanRateIndicatorRQ
    {

        ChangeDuringStay,

        MultipleNights,

        Exclusive,

        OnRequest,

        LimitedAvailability,

        AvailableForSale,

        ClosedOut,

        OtherAvailable
    }

    [XmlRoot(IsNullable = false)]
    public class RoomRate
    {

        [XmlArrayItem(IsNullable = false)]
        public RateRQ[] Rates;

        public RoomRateDescription RoomRateDescription;

        [XmlArrayItem(IsNullable = false)]
        public Feature[] Features;

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

        [XmlAttribute()]
        public string RatePlanCategory;
    }

    [XmlRoot(IsNullable = false)]
    public class RateRQ
    {

        public BaseRQ Base;

        [XmlArrayItem(IsNullable = false)]
        public AdditionalGuestAmount[] AdditionalGuestAmounts;

        [XmlArrayItem(IsNullable = false)]
        public Fee[] Fees;

        [XmlArrayItem(IsNullable = false)]
        public CancelPenalty[] CancelPolicies;

        [XmlArrayItem(IsNullable = false)]
        public RequiredPayment[] PaymentPolicies;

        public Discount Discount;

        public Total Total;

        public RateDescription RateDescription;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;

        [XmlAttribute()]
        public string AgeQualifyingCode;

        [XmlAttribute()]
        public int MinAge;

        [XmlIgnore()]
        public bool MinAgeSpecified;

        [XmlAttribute()]
        public int MaxAge;

        [XmlIgnore()]
        public bool MaxAgeSpecified;

        [XmlAttribute()]
        public RateAgeTimeUnitRQ AgeTimeUnit;

        [XmlIgnore()]
        public bool AgeTimeUnitSpecified;

        [XmlAttribute()]
        public bool GuaranteedInd;

        [XmlIgnore()]
        public bool GuaranteedIndSpecified;

        [XmlAttribute()]
        public int NumberOfUnits;

        [XmlIgnore()]
        public bool NumberOfUnitsSpecified;

        [XmlAttribute()]
        public RateRateTimeUnit RateTimeUnit;

        [XmlIgnore()]
        public bool RateTimeUnitSpecified;

        [XmlAttribute()]
        public int UnitMultiplier;

        [XmlIgnore()]
        public bool UnitMultiplierSpecified;

        [XmlAttribute()]
        public int MinGuestApplicable;

        [XmlIgnore()]
        public bool MinGuestApplicableSpecified;

        [XmlAttribute()]
        public int MaxGuestApplicable;

        [XmlIgnore()]
        public bool MaxGuestApplicableSpecified;

        [XmlAttribute()]
        public int MinLOS;

        [XmlIgnore()]
        public bool MinLOSSpecified;

        [XmlAttribute()]
        public int MaxLOS;

        [XmlIgnore()]
        public bool MaxLOSSpecified;

        [XmlAttribute()]
        public RateStayOverDate StayOverDate;

        [XmlIgnore()]
        public bool StayOverDateSpecified;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute()]
        public string RateMode;
    }

    [XmlRoot(IsNullable = false)]
    public class Total
    {

        public Taxes Taxes;

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
    public class RateDescription
    {

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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

    public enum RateAgeTimeUnitRQ
    {

        Year,

        Month,

        Week,

        Day,

        Hour,

        Second,

        FullDuration
    }

    public enum RateRateTimeUnit
    {

        Year,

        Month,

        Week,

        Day,

        Hour,

        Second,

        FullDuration
    }

    public enum RateStayOverDate
    {

        Mon,

        Tue,

        Wed,

        Thu,

        Fri,

        Sat,

        Sun
    }

    [XmlRoot(IsNullable = false)]
    public class RoomRateDescription
    {

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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
    public class TimeSpan
    {

        public DateWindowRange DateWindowRange;

        [XmlAttribute(DataType = "date")]
        public DateTime Start;

        [XmlIgnore()]
        public bool StartSpecified;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute(DataType = "date")]
        public DateTime End;

        [XmlIgnore()]
        public bool EndSpecified;
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

        [XmlAttribute()]
        public string TravelSector;
    }

    [XmlRoot(IsNullable = false)]
    public class SpecialRequest
    {

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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

        [XmlAttribute()]
        public string CodeContext;
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
    public class Service
    {

        public Price Price;

        public ServiceDetails ServiceDetails;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public string ServicePricingType;

        [XmlAttribute()]
        public string ReservationStatusType;

        [XmlAttribute()]
        public string ServiceRPH;

        [XmlAttribute()]
        public string ServiceInventoryCode;

        [XmlAttribute()]
        public string RatePlanCode;

        [XmlAttribute()]
        public string InventoryBlockCode;

        [XmlAttribute()]
        public bool PriceGuaranteed;

        [XmlIgnore()]
        public bool PriceGuaranteedSpecified;

        [XmlAttribute()]
        public bool Inclusive;

        [XmlIgnore()]
        public bool InclusiveSpecified;

        [XmlAttribute()]
        public int Quantity;

        [XmlIgnore()]
        public bool QuantitySpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class Price
    {

        public BaseRQ Base;

        [XmlArrayItem(IsNullable = false)]
        public AdditionalGuestAmount[] AdditionalGuestAmounts;

        [XmlArrayItem(IsNullable = false)]
        public Fee[] Fees;

        [XmlArrayItem(IsNullable = false)]
        public CancelPenalty[] CancelPolicies;

        [XmlArrayItem(IsNullable = false)]
        public RequiredPayment[] PaymentPolicies;

        public Discount Discount;

        public Total Total;

        public RateDescription RateDescription;

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;

        [XmlAttribute()]
        public string AgeQualifyingCode;

        [XmlAttribute()]
        public int MinAge;

        [XmlIgnore()]
        public bool MinAgeSpecified;

        [XmlAttribute()]
        public int MaxAge;

        [XmlIgnore()]
        public bool MaxAgeSpecified;

        [XmlAttribute()]
        public PriceAgeTimeUnit AgeTimeUnit;

        [XmlIgnore()]
        public bool AgeTimeUnitSpecified;

        [XmlAttribute()]
        public bool GuaranteedInd;

        [XmlIgnore()]
        public bool GuaranteedIndSpecified;

        [XmlAttribute()]
        public int NumberOfUnits;

        [XmlIgnore()]
        public bool NumberOfUnitsSpecified;

        [XmlAttribute()]
        public PriceRateTimeUnit RateTimeUnit;

        [XmlIgnore()]
        public bool RateTimeUnitSpecified;

        [XmlAttribute()]
        public int UnitMultiplier;

        [XmlIgnore()]
        public bool UnitMultiplierSpecified;

        [XmlAttribute()]
        public int MinGuestApplicable;

        [XmlIgnore()]
        public bool MinGuestApplicableSpecified;

        [XmlAttribute()]
        public int MaxGuestApplicable;

        [XmlIgnore()]
        public bool MaxGuestApplicableSpecified;

        [XmlAttribute()]
        public int MinLOS;

        [XmlIgnore()]
        public bool MinLOSSpecified;

        [XmlAttribute()]
        public int MaxLOS;

        [XmlIgnore()]
        public bool MaxLOSSpecified;

        [XmlAttribute()]
        public PriceStayOverDate StayOverDate;

        [XmlIgnore()]
        public bool StayOverDateSpecified;
    }

    public enum PriceAgeTimeUnit
    {

        Year,

        Month,

        Week,

        Day,

        Hour,

        Second,

        FullDuration
    }

    public enum PriceRateTimeUnit
    {

        Year,

        Month,

        Week,

        Day,

        Hour,

        Second,

        FullDuration
    }

    public enum PriceStayOverDate
    {

        Mon,

        Tue,

        Wed,

        Thu,

        Fri,

        Sat,

        Sun
    }

    [XmlRoot(IsNullable = false)]
    public class ServiceDetails
    {

        public GuestCounts GuestCounts;

        public TimeSpan TimeSpan;

        [XmlArrayItem(IsNullable = false)]
        public ResGuestRPH[] ResGuestRPHs;

        [XmlArrayItem(IsNullable = false)]
        public Membership[] Memberships;

        [XmlArrayItem(IsNullable = false)]
        public Comment[] Comments;

        [XmlArrayItem(IsNullable = false)]
        public SpecialRequest[] SpecialRequests;

        public Guarantee Guarantee;

        [XmlArrayItem(IsNullable = false)]
        public RequiredPayment[] DepositPayments;

        [XmlArrayItem(IsNullable = false)]
        public CancelPenalty[] CancelPenalties;

        public Total Total;
    }

    [XmlRoot(IsNullable = false)]
    public class ResGuest
    {

        [XmlArrayItem(IsNullable = false)]
        public ProfileInfo[] Profiles;

        [XmlArrayItem(IsNullable = false)]
        public SpecialRequest[] SpecialRequests;

        [XmlArrayItem(IsNullable = false)]
        public Comment[] Comments;

        [XmlArrayItem(IsNullable = false)]
        public ServiceRPH[] ServiceRPHs;

        public ArrivalTransport ArrivalTransport;

        public DepartureTransport DepartureTransport;

        public GuestCounts GuestCounts;

        public InHouseTimeSpan InHouseTimeSpan;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public string ResGuestRPH;

        [XmlAttribute()]
        public string AgeQualifyingCode;

        [XmlAttribute(DataType = "time")]
        public DateTime ArrivalTime;

        [XmlIgnore()]
        public bool ArrivalTimeSpecified;

        [XmlAttribute(DataType = "time")]
        public DateTime DepartureTime;

        [XmlIgnore()]
        public bool DepartureTimeSpecified;

        [XmlAttribute()]
        public string GroupEventCode;

        [XmlAttribute()]
        public bool VIP;

        [XmlIgnore()]
        public bool VIPSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class ProfileInfo
    {

        public UniqueIDRQ UniqueID;

        public Profile Profile;
    }

    [XmlRoot(IsNullable = false)]
    public class Profile
    {

        public Accesses Accesses;

        public Customer Customer;

        public PrefCollections PrefCollections;

        public CompanyInfo CompanyInfo;

        public Affiliations Affiliations;

        public Agreements Agreements;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(ProfileShareAllSynchInd.No)]
        public ProfileShareAllSynchInd ShareAllSynchInd = ProfileShareAllSynchInd.No;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(ProfileShareAllMarketInd.No)]
        public ProfileShareAllMarketInd ShareAllMarketInd = ProfileShareAllMarketInd.No;

        [XmlAttribute()]
        public string ProfileType;

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
    public class PrefCollections
    {

        [XmlElement("PrefCollection")]
        public PrefCollection[] PrefCollection;

        [XmlAttribute()]
        public PrefCollectionsShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public PrefCollectionsShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class PrefCollection
    {

        [XmlElement("CommonPref")]
        public CommonPref[] CommonPref;

        [XmlElement("VehicleRentalPref")]
        public VehicleRentalPref[] VehicleRentalPref;

        [XmlElement("AirlinePref")]
        public AirlinePref[] AirlinePref;

        [XmlElement("HotelPref")]
        public HotelPref[] HotelPref;

        [XmlElement("OtherSrvcPref")]
        public OtherSrvcPref[] OtherSrvcPref;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public PrefCollectionShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public PrefCollectionShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string TravelPurpose;
    }

    [XmlRoot(IsNullable = false)]
    public class VehicleRentalPref
    {

        [XmlElement("LoyaltyPref")]
        public LoyaltyPref[] LoyaltyPref;

        [XmlElement("VendorPref")]
        public VendorPref[] VendorPref;

        [XmlElement("PaymentFormPref")]
        public PaymentFormPref[] PaymentFormPref;

        [XmlElement("CoveragePref")]
        public CoveragePref[] CoveragePref;

        [XmlElement("SpecialReqPref")]
        public SpecialReqPref[] SpecialReqPref;

        [XmlElement("VehTypePref")]
        public VehTypePref[] VehTypePref;

        [XmlElement("SpecialEquipPref")]
        public SpecialEquipPref[] SpecialEquipPref;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(VehicleRentalPrefPreferLevel.Preferred)]
        public VehicleRentalPrefPreferLevel PreferLevel = VehicleRentalPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public VehicleRentalPrefShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public VehicleRentalPrefShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

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
    public class VehTypePref
    {

        public VehType VehType;

        public VehClass VehClass;

        [XmlAttribute()]
        public bool AirConditionInd;

        [XmlIgnore()]
        public bool AirConditionIndSpecified;

        [XmlAttribute()]
        public VehTypePrefTransmissionType TransmissionType;

        [XmlIgnore()]
        public bool TransmissionTypeSpecified;

        [XmlAttribute()]
        public VehTypePrefTypePref TypePref;

        [XmlIgnore()]
        public bool TypePrefSpecified;

        [XmlAttribute()]
        public VehTypePrefClassPref ClassPref;

        [XmlIgnore()]
        public bool ClassPrefSpecified;

        [XmlAttribute()]
        public VehTypePrefAirConditionPref AirConditionPref;

        [XmlIgnore()]
        public bool AirConditionPrefSpecified;

        [XmlAttribute()]
        public VehTypePrefTransmissionPref TransmissionPref;

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

    public enum VehTypePrefTransmissionType
    {

        Automatic,

        Manual
    }

    public enum VehTypePrefTypePref
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum VehTypePrefClassPref
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum VehTypePrefAirConditionPref
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum VehTypePrefTransmissionPref
    {

        Only,

        Unacceptable,

        Preferred
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

        [XmlAttribute()]
        public SpecialEquipPrefAction Action;

        [XmlIgnore()]
        public bool ActionSpecified;
    }

    public enum SpecialEquipPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum SpecialEquipPrefAction
    {

        [XmlEnum("Add-Update")]
        AddUpdate,

        Cancel,

        Delete,

        Add,

        Replace
    }

    public enum VehicleRentalPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum VehicleRentalPrefShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum VehicleRentalPrefShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class OtherSrvcPref
    {

        public string OtherSrvcName;

        [XmlElement("VendorPref")]
        public VendorPref[] VendorPref;

        [XmlElement("LoyaltyPref")]
        public LoyaltyPref[] LoyaltyPref;

        [XmlElement("PaymentFormPref")]
        public PaymentFormPref[] PaymentFormPref;

        [XmlElement("SpecRequestPref")]
        public SpecRequestPref[] SpecRequestPref;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OtherSrvcPrefPreferLevel.Preferred)]
        public OtherSrvcPrefPreferLevel PreferLevel = OtherSrvcPrefPreferLevel.Preferred;

        [XmlAttribute()]
        public OtherSrvcPrefShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public OtherSrvcPrefShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string TravelPurpose;
    }

    public enum OtherSrvcPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum OtherSrvcPrefShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum OtherSrvcPrefShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum PrefCollectionShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum PrefCollectionShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum PrefCollectionsShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum PrefCollectionsShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum ProfileShareAllSynchInd
    {

        Yes,

        No
    }

    public enum ProfileShareAllMarketInd
    {

        Yes,

        No
    }

    [XmlRoot(IsNullable = false)]
    public class InHouseTimeSpan
    {

        public DateWindowRange DateWindowRange;

        [XmlAttribute(DataType = "date")]
        public DateTime Start;

        [XmlIgnore()]
        public bool StartSpecified;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute(DataType = "date")]
        public DateTime End;

        [XmlIgnore()]
        public bool EndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class ResGlobalInfo
    {

        public GuestCounts GuestCounts;

        public TimeSpan TimeSpan;

        [XmlArrayItem(IsNullable = false)]
        public ResGuestRPH[] ResGuestRPHs;

        [XmlArrayItem(IsNullable = false)]
        public Membership[] Memberships;

        [XmlArrayItem(IsNullable = false)]
        public Comment[] Comments;

        [XmlArrayItem(IsNullable = false)]
        public SpecialRequest[] SpecialRequests;

        public Guarantee Guarantee;

        [XmlArrayItem(IsNullable = false)]
        public RequiredPayment[] DepositPayments;

        [XmlArrayItem(IsNullable = false)]
        public CancelPenalty[] CancelPenalties;

        public Total Total;

        [XmlArrayItem(IsNullable = false)]
        public HotelReservationID[] HotelReservationIDs;

        [XmlArrayItem(IsNullable = false)]
        public RoutingHop[] RoutingHops;

        [XmlArrayItem(IsNullable = false)]
        public ProfileInfo[] Profiles;
    }

    [XmlRoot(IsNullable = false)]
    public class HotelReservationID
    {

        [XmlAttribute()]
        public string ResID_Type;

        [XmlAttribute()]
        public string ResID_Value;

        [XmlAttribute()]
        public string ResID_Source;

        [XmlAttribute()]
        public string ResID_SourceContext;

        [XmlAttribute()]
        public DateTime ResID_Date;

        [XmlIgnore()]
        public bool ResID_DateSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool ForGuest = false;

        [XmlAttribute()]
        public string ResGuestRPH;

        [XmlAttribute()]
        public string CancelOriginatorCode;

        [XmlAttribute()]
        public DateTime CancellationDate;

        [XmlIgnore()]
        public bool CancellationDateSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class RoutingHop
    {

        [XmlAttribute()]
        public string SystemCode;

        [XmlAttribute()]
        public string LocalRefID;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        public string Comment;

        [XmlAttribute()]
        public int SequenceNmbr;

        [XmlIgnore()]
        public bool SequenceNmbrSpecified;

        [XmlAttribute()]
        public string Data;
    }

    [XmlRoot(IsNullable = false)]
    public class WrittenConfInst
    {

        public SupplementalData SupplementalData;

        public EmailRQ Email;

        [XmlAttribute()]
        public string LanguageID;

        [XmlAttribute()]
        public string AddresseeName;

        [XmlAttribute()]
        public string Address;

        [XmlAttribute()]
        public string Telephone;
    }

    [XmlRoot(IsNullable = false)]
    public class SupplementalData
    {

        public Text Text;

        public string Image;

        [XmlElement(DataType = "anyURI")]
        public string URL;

        public ListItemRQ ListItem;

        [XmlAttribute()]
        public string Name;

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
    public class Verification
    {

        public PersonNameRQ PersonName;

        public EmailRQ Email;

        public TelephoneInfo TelephoneInfo;

        public PaymentCardRQ PaymentCard;

        public AddressInfo AddressInfo;

        [XmlElement("CustLoyalty")]
        public CustLoyalty[] CustLoyalty;

        [XmlElement("Vendor")]
        public VendorRQ[] Vendor;

        public ReservationTimeSpan ReservationTimeSpan;

        [XmlElement("AssociatedQuantity")]
        public AssociatedQuantity[] AssociatedQuantity;

        public StartLocation StartLocation;

        public EndLocation EndLocation;

        public TPA_Extensions TPA_Extensions;
    }

    [XmlRoot(IsNullable = false)]
    public class VendorRQ
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
    public class ReservationTimeSpan
    {

        [XmlAttribute(DataType = "date")]
        public DateTime Start;

        [XmlIgnore()]
        public bool StartSpecified;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute(DataType = "date")]
        public DateTime End;

        [XmlIgnore()]
        public bool EndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class StartLocation
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlAttribute()]
        public DateTime AssociatedDateTime;

        [XmlIgnore()]
        public bool AssociatedDateTimeSpecified;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class HotelReservationIDs
    {

        [XmlElement("HotelReservationID")]
        public HotelReservationID[] HotelReservationID;
    }

    [XmlRoot(IsNullable = false)]
    public class Memberships
    {

        [XmlElement("Membership")]
        public Membership[] Membership;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_HotelResModifyRQ
    {

        public POS POS;

        [XmlElement("UniqueID")]
        public UniqueIDRQ[] UniqueID;

        public HotelResModifies HotelResModifies;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_HotelResModifyRQTarget.Production)]
        public OTA_HotelResModifyRQTarget Target = OTA_HotelResModifyRQTarget.Production;

        [XmlAttribute()]
        public double Version;

        [XmlAttribute()]
        public string TransactionIdentifier;

        [XmlAttribute()]
        public int SequenceNmbr;

        [XmlIgnore()]
        public bool SequenceNmbrSpecified;

        [XmlAttribute()]
        public OTA_HotelResModifyRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public OTA_HotelResModifyRQResStatus ResStatus;

        [XmlIgnore()]
        public bool ResStatusSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    public enum OTA_HotelResModifyRQTarget
    {

        Test,

        Production
    }

    public enum OTA_HotelResModifyRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }

    public enum OTA_HotelResModifyRQResStatus
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
    public class PaymentPolicies
    {

        [XmlElement("RequiredPayment")]
        public RequiredPayment[] RequiredPayment;
    }

    [XmlRoot(IsNullable = false)]
    public class Profiles
    {

        [XmlElement("ProfileInfo")]
        public ProfileInfo[] ProfileInfo;
    }

    [XmlRoot(IsNullable = false)]
    public class RatePlans
    {

        [XmlElement("RatePlan")]
        public RatePlanRQ[] RatePlan;
    }

    [XmlRoot(IsNullable = false)]
    public class Rates
    {

        [XmlElement("Rate")]
        public RateRQ[] Rate;
    }

    [XmlRoot(IsNullable = false)]
    public class ResGuestRPHs
    {

        [XmlElement("ResGuestRPH")]
        public ResGuestRPH[] ResGuestRPH;
    }

    [XmlRoot(IsNullable = false)]
    public class ResGuests
    {

        [XmlElement("ResGuest")]
        public ResGuest[] ResGuest;
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
    public class RoutingHops
    {

        [XmlElement("RoutingHop")]
        public RoutingHop[] RoutingHop;
    }

    [XmlRoot(IsNullable = false)]
    public class ServiceRPHs
    {

        [XmlElement("ServiceRPH")]
        public ServiceRPH[] ServiceRPH;
    }

    [XmlRoot(IsNullable = false)]
    public class Services
    {

        [XmlElement("Service")]
        public Service[] Service;
    }

    [XmlRoot(IsNullable = false)]
    public class SpecialRequests
    {

        [XmlElement("SpecialRequest")]
        public SpecialRequest[] SpecialRequest;
    }

    [XmlRoot(IsNullable = false)]
    public class Transportations
    {

        [XmlElement("Transportation")]
        public Transportation[] Transportation;
    }

    [XmlRoot(IsNullable = false)]
    public class VendorMessages
    {

        [XmlElement("VendorMessage")]
        public VendorMessage[] VendorMessage;
    }

}
