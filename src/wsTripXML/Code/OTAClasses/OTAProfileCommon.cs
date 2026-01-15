using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmProfileCommon
{
    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(CommissionInfoType))]
    [XmlInclude(typeof(CertificationType))]
    public class FreeTextType
    {

        // <remarks/>
        [XmlAttribute(DataType = "language")]
        public string Language;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class CommissionInfoType : FreeTextType
    {

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string CommissionPlanCode;

        // <remarks/>
        [XmlAttribute()]
        public decimal Amount;

        // <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class CertificationType : FreeTextType
    {

        // <remarks/>
        [XmlAttribute()]
        public string ID;

        // <remarks/>
        [XmlAttribute()]
        public CertificationTypeSingleVendorInd SingleVendorInd;

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
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;
    }

    // <remarks/>
    [XmlType()]
    public enum TravelArrangerTypeShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlType()]
    public enum TravelArrangerTypeShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlType()]
    public enum CertificationTypeSingleVendorInd
    {

        // <remarks/>
        SingleVndr,

        // <remarks/>
        Alliance
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(ProfileTypeUserID))]
    [XmlInclude(typeof(SourceTypeRequestorID))]
    public class UniqueID_Type
    {

        // <remarks/>
        public CompanyNameType CompanyName;

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

    // <remarks/>
    [XmlType()]
    public class ProfileTypeUserID : UniqueID_Type
    {

        // <remarks/>
        [XmlAttribute()]
        public string PinNumber;
    }

    // <remarks/>
    [XmlType()]
    public class SourceTypeRequestorID : UniqueID_Type
    {

        // <remarks/>
        [XmlAttribute()]
        public string MessagePassword;
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(AllianceConsortiumTypeAllianceMember))]
    [XmlInclude(typeof(DirectBillTypeCompanyName))]
    [XmlInclude(typeof(CompanyNamePrefType))]
    [XmlInclude(typeof(AirlinePrefTypeVendorPref))]
    [XmlInclude(typeof(TravelArrangerType))]
    public class CompanyNameType
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
        public string Division;

        // <remarks/>
        [XmlAttribute()]
        public string Department;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class AllianceConsortiumTypeAllianceMember : CompanyNameType
    {

        // <remarks/>
        [XmlAttribute()]
        public string MemberCode;
    }

    // <remarks/>
    [XmlType()]
    public class DirectBillTypeCompanyName : CompanyNameType
    {

        // <remarks/>
        [XmlAttribute()]
        public string ContactName;
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(AirlinePrefTypeVendorPref))]
    public class CompanyNamePrefType : CompanyNameType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;
    }

    // <remarks/>
    [XmlType()]
    public enum PreferLevelType
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred,

        // <remarks/>
        Required,

        // <remarks/>
        NoPreference
    }

    // <remarks/>
    [XmlType()]
    public class AirlinePrefTypeVendorPref : CompanyNamePrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    [XmlType()]
    public class TravelArrangerType : CompanyNameType
    {

        // <remarks/>
        [XmlAttribute()]
        public bool DefaultInd;

        // <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute("TravelArrangerType")]
        public string TravelArrangerType1;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public string Remark;
    }

    // <remarks/>
    [XmlType()]
    public class ProfileType
    {

        // <remarks/>
        public AccessesType Accesses;

        // <remarks/>
        public CustomerType Customer;

        // <remarks/>
        [XmlElement("UserID")]
        public ProfileTypeUserID[] UserID;

        // <remarks/>
        public PreferencesType PrefCollections;

        // <remarks/>
        public CompanyInfoType CompanyInfo;

        // <remarks/>
        public AffiliationsType Affiliations;

        // <remarks/>
        public AgreementsType Agreements;

        // <remarks/>
        public Remarks Remarks;

        // <remarks/>
        [XmlArrayItem("Comment", IsNullable = false)]
        public ProfileTypeComment[] Comments;

        // <remarks/>
        public TPA_Extensions TPA_Extensions;

        // <remarks/>
        [XmlAttribute()]
        public YesNoType ShareAllSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareAllSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public YesNoType ShareAllMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareAllMarketIndSpecified;

        // <remarks/>
        [XmlAttribute("ProfileType")]
        public string ProfileType1;

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

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime PurgeDate;

        // <remarks/>
        [XmlIgnore()]
        public bool PurgeDateSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public string[] StatusCode;
    }

    // <remarks/>
    [XmlType()]
    public class AccessesType
    {

        // <remarks/>
        [XmlElement("Access")]
        public AccessesTypeAccess[] Access;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareMarketInd ShareMarketInd;

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
    [XmlType()]
    public class CustomerType
    {

        // <remarks/>
        [XmlElement("PersonName")]
        public PersonNameType[] PersonName;

        // <remarks/>
        [XmlElement("Telephone")]
        public CustomerTypeTelephone[] Telephone;

        // <remarks/>
        [XmlElement("Email")]
        public CustomerTypeEmail[] Email;

        // <remarks/>
        [XmlElement("Address")]
        public CustomerTypeAddress[] Address;

        // <remarks/>
        [XmlElement("URL")]
        public CustomerTypeURL[] URL;

        // <remarks/>
        [XmlElement("CitizenCountryName")]
        public CustomerTypeCitizenCountryName[] CitizenCountryName;

        // <remarks/>
        [XmlElement("PhysChallName")]
        public CustomerTypePhysChallName[] PhysChallName;

        // <remarks/>
        [XmlElement("PetInfo")]
        public string[] PetInfo;

        // <remarks/>
        [XmlElement("PaymentForm")]
        public CustomerTypePaymentForm[] PaymentForm;

        // <remarks/>
        [XmlElement("RelatedTraveler")]
        public RelatedTravelerType[] RelatedTraveler;

        // <remarks/>
        [XmlElement("ContactPerson")]
        public ContactPersonType[] ContactPerson;

        // <remarks/>
        [XmlElement("Document")]
        public DocumentType[] Document;

        // <remarks/>
        [XmlElement("CustLoyalty")]
        public CustomerTypeCustLoyalty[] CustLoyalty;

        // <remarks/>
        [XmlElement("EmployeeInfo")]
        public EmployeeInfoType[] EmployeeInfo;

        // <remarks/>
        public CompanyNameType EmployerInfo;

        // <remarks/>
        [XmlElement("AdditionalLanguage")]
        public CustomerTypeAdditionalLanguage[] AdditionalLanguage;

        // <remarks/>
        public TPA_Extensions TPA_Extensions;

        // <remarks/>
        [XmlAttribute()]
        public DocumentTypeGender Gender;

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
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string DecimalPlaces;

        // <remarks/>
        [XmlAttribute()]
        public bool VIP_Indicator;

        // <remarks/>
        [XmlIgnore()]
        public bool VIP_IndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Text;

        // <remarks/>
        [XmlAttribute(DataType = "language")]
        public string Language;

        // <remarks/>
        [XmlAttribute()]
        public string CustomerValue;

        // <remarks/>
        [XmlAttribute()]
        public CustomerTypeMaritalStatus MaritalStatus;

        // <remarks/>
        [XmlIgnore()]
        public bool MaritalStatusSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool PreviouslyMarriedIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool PreviouslyMarriedIndicatorSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string ChildQuantity;
    }

    // <remarks/>
    [XmlType()]
    public enum CustomerTypeMaritalStatus
    {

        // <remarks/>
        Annulled,

        // <remarks/>
        [XmlEnum("Co-habitating")]
        Cohabitating,

        // <remarks/>
        Divorced,

        // <remarks/>
        Engaged,

        // <remarks/>
        Married,

        // <remarks/>
        Separated,

        // <remarks/>
        Single,

        // <remarks/>
        Widowed,

        // <remarks/>
        Unknown
    }

    // <remarks/>
    [XmlType()]
    public class PreferencesType
    {

        // <remarks/>
        [XmlElement("PrefCollection")]
        public PreferencesTypePrefCollection[] PrefCollection;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class CompanyInfoType
    {

        // <remarks/>
        [XmlElement("CompanyName")]
        public CompanyNameType[] CompanyName;

        // <remarks/>
        [XmlElement("AddressInfo")]
        public CompanyInfoTypeAddressInfo[] AddressInfo;

        // <remarks/>
        [XmlElement("TelephoneInfo")]
        public CompanyInfoTypeTelephoneInfo[] TelephoneInfo;

        // <remarks/>
        [XmlElement("Email")]
        public CompanyInfoTypeEmail[] Email;

        // <remarks/>
        [XmlElement("URL")]
        public URL_Type[] URL;

        // <remarks/>
        [XmlElement("BusinessLocale")]
        public AddressType[] BusinessLocale;

        // <remarks/>
        [XmlElement("PaymentForm")]
        public CompanyInfoTypePaymentForm[] PaymentForm;

        // <remarks/>
        [XmlElement("ContactPerson")]
        public ContactPersonType[] ContactPerson;

        // <remarks/>
        [XmlElement("TravelArranger")]
        public TravelArrangerType[] TravelArranger;

        // <remarks/>
        [XmlElement("LoyaltyProgram")]
        public LoyaltyProgramType[] LoyaltyProgram;

        // <remarks/>
        [XmlElement("TripPurpose")]
        public CompanyInfoTypeTripPurpose[] TripPurpose;

        // <remarks/>
        [XmlElement("OtherServiceInformation")]
        public OtherServiceInformationType[] OtherServiceInformation;

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string DecimalPlaces;
    }

    // <remarks/>
    [XmlType()]
    public class Comments
    {
        // <remarks/>
        [XmlElement("Comment", IsNullable = false)]
        public ProfileTypeComment[] Comment;
    }

    // <remarks/>
    [XmlType()]
    public class AffiliationsType
    {

        // <remarks/>
        [XmlElement("Organization")]
        public OrganizationType[] Organization;

        // <remarks/>
        [XmlElement("Employer")]
        public EmployerType[] Employer;

        // <remarks/>
        [XmlElement("TravelArranger")]
        public TravelArrangerType[] TravelArranger;

        // <remarks/>
        [XmlElement("TravelClub")]
        public TravelClubType[] TravelClub;

        // <remarks/>
        [XmlElement("Insurance")]
        public InsuranceType[] Insurance;

        // <remarks/>
        public TPA_Extensions TPA_Extensions;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class AgreementsType
    {

        // <remarks/>
        [XmlElement("Certification")]
        public CertificationType[] Certification;

        // <remarks/>
        [XmlElement("AllianceConsortium")]
        public AllianceConsortiumType[] AllianceConsortium;

        // <remarks/>
        [XmlElement("CommissionInfo")]
        public CommissionInfoType[] CommissionInfo;

        // <remarks/>
        [XmlElement("ProfileSecurity")]
        public AgreementsTypeProfileSecurity[] ProfileSecurity;

        // <remarks/>
        public ParagraphType ContractInformation;

        // <remarks/>
        public TPA_Extensions TPA_Extensions;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class ProfileTypeComment : ParagraphType
    {

        // <remarks/>
        [XmlElement("AuthorizedViewer")]
        public ProfileTypeCommentAuthorizedViewer[] AuthorizedViewer;

        // <remarks/>
        [XmlAttribute()]
        public string CommentOriginatorCode;

        // <remarks/>
        [XmlAttribute()]
        public bool GuestViewable;

        // <remarks/>
        [XmlIgnore()]
        public bool GuestViewableSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Category;

        // <remarks/>
        [XmlAttribute()]
        public string AirlineVendorPrefRPH;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ActionDate;

        // <remarks/>
        [XmlIgnore()]
        public bool ActionDateSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

    }

    // <remarks/>
    [XmlType()]
    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : TPA_ExtensionsType
    {

    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(TPA_Extensions))]
    public class TPA_ExtensionsType
    {

        // <remarks/>
        public Provider Provider;

        public CustomCheckRule CustomCheckRule;
    }

    // <remarks/>
    [XmlType()]
    public class CustomCheckRule
    {

        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;

    }

    // <remarks/>
    [XmlType()]
    public enum YesNoType
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No
    }

    // <remarks/>
    [XmlType()]
    public class AccessesTypeAccess
    {

        // <remarks/>
        public PersonNameType AccessPerson;

        // <remarks/>
        public FreeTextType AccessComment;

        // <remarks/>
        [XmlAttribute()]
        public AccessesTypeAccessActionType ActionType;

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
    [XmlType()]
    public enum AccessesTypeAccessActionType
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
    [XmlType()]
    public class PreferencesTypePrefCollection
    {

        // <remarks/>
        [XmlElement("CommonPref")]
        public CommonPrefType[] CommonPref;

        // <remarks/>
        [XmlElement("VehicleRentalPref")]
        public VehicleProfileRentalPrefType[] VehicleRentalPref;

        // <remarks/>
        [XmlElement("AirlinePref")]
        public AirlinePrefType[] AirlinePref;

        // <remarks/>
        [XmlElement("HotelPref")]
        public HotelPrefType[] HotelPref;

        // <remarks/>
        [XmlElement("OtherSrvcPref")]
        public OtherSrvcPrefType[] OtherSrvcPref;

        // <remarks/>
        public TPA_Extensions TPA_Extensions;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string TravelPurpose;
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(TravelClubTypeClubMemberName))]
    [XmlInclude(typeof(OrganizationTypeOrgMemberName))]
    public class PersonNameType
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
        public TravelArrangerTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string NameType;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class TravelClubTypeClubMemberName : PersonNameType
    {

        // <remarks/>
        [XmlAttribute()]
        public string ID;
    }

    // <remarks/>
    [XmlType()]
    public class OrganizationTypeOrgMemberName : PersonNameType
    {

        // <remarks/>
        [XmlAttribute()]
        public string Level;

        // <remarks/>
        [XmlAttribute()]
        public string Title;
    }

    // <remarks/>
    [XmlType()]
    public class OtherSrvcPrefType
    {

        // <remarks/>
        public string OtherSrvcName;

        // <remarks/>
        [XmlElement("VendorPref")]
        public CompanyNamePrefType[] VendorPref;

        // <remarks/>
        [XmlElement("LoyaltyPref")]
        public LoyaltyPrefType[] LoyaltyPref;

        // <remarks/>
        [XmlElement("PaymentFormPref")]
        public PaymentFormPrefType[] PaymentFormPref;

        // <remarks/>
        [XmlElement("SpecRequestPref")]
        public SpecRequestPrefType[] SpecRequestPref;

        // <remarks/>
        public TPA_Extensions TPA_Extensions;

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string TravelPurpose;
    }

    // <remarks/>
    [XmlType()]
    public class LoyaltyPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class PaymentFormPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class SpecRequestPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class CommonPrefType
    {

        // <remarks/>
        [XmlElement("NamePref")]
        public NamePrefType[] NamePref;

        // <remarks/>
        [XmlElement("PhonePref")]
        public PhonePrefType[] PhonePref;

        // <remarks/>
        [XmlElement("AddressPref")]
        public AddressPrefType[] AddressPref;

        // <remarks/>
        [XmlElement("PaymentFormPref")]
        public PaymentFormPrefType[] PaymentFormPref;

        // <remarks/>
        [XmlElement("InterestPref")]
        public InterestPrefType[] InterestPref;

        // <remarks/>
        [XmlElement("InsurancePref")]
        public InsurancePrefType[] InsurancePref;

        // <remarks/>
        [XmlElement("SeatingPref")]
        public SeatingPrefType[] SeatingPref;

        // <remarks/>
        [XmlElement("TicketDistribPref")]
        public TicketDistribPrefType[] TicketDistribPref;

        // <remarks/>
        [XmlElement("MediaEntertainPref")]
        public MediaEntertainPrefType[] MediaEntertainPref;

        // <remarks/>
        [XmlElement("PetInfoPref")]
        public PetInfoPrefType[] PetInfoPref;

        // <remarks/>
        [XmlElement("MealPref")]
        public MealPrefType[] MealPref;

        // <remarks/>
        [XmlElement("LoyaltyPref")]
        public LoyaltyPrefType[] LoyaltyPref;

        // <remarks/>
        [XmlElement("SpecRequestPref")]
        public SpecRequestPrefType[] SpecRequestPref;

        // <remarks/>
        [XmlElement("RelatedTravelerPref")]
        public RelatedTravelerPrefType[] RelatedTravelerPref;

        // <remarks/>
        [XmlElement("ContactPref")]
        public CommonPrefTypeContactPref[] ContactPref;

        // <remarks/>
        public EmployeeInfoType EmployeeLevelInfo;

        // <remarks/>
        public TPA_Extensions TPA_Extensions;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool SmokingAllowed;

        // <remarks/>
        [XmlIgnore()]
        public bool SmokingAllowedSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "language")]
        public string PrimaryLangID;

        // <remarks/>
        [XmlAttribute(DataType = "language")]
        public string AltLangID;
    }

    // <remarks/>
    [XmlType()]
    public class NamePrefType
    {

        // <remarks/>
        public UniqueID_Type UniqueID;

        // <remarks/>
        public PersonNameType PersonName;

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class PhonePrefType
    {

        // <remarks/>
        public PhonePrefTypeTelephone Telephone;
    }

    // <remarks/>
    [XmlType()]
    public class PhonePrefTypeTelephone
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    [XmlType()]
    public class AddressPrefType
    {

        // <remarks/>
        public AddressInfoType Address;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(CompanyInfoTypeAddressInfo))]
    [XmlInclude(typeof(CustomerTypeAddress))]
    public class AddressInfoType : AddressType
    {

        // <remarks/>
        [XmlAttribute()]
        public bool DefaultInd;

        // <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string UseType;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(AddressInfoType))]
    [XmlInclude(typeof(CompanyInfoTypeAddressInfo))]
    [XmlInclude(typeof(CustomerTypeAddress))]
    public class AddressType
    {

        // <remarks/>
        public AddressTypeStreetNmbr StreetNmbr;

        // <remarks/>
        [XmlElement("BldgRoom")]
        public AddressTypeBldgRoom[] BldgRoom;

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
        public StateProvType StateProv;

        // <remarks/>
        public CountryNameType CountryName;

        // <remarks/>
        [XmlAttribute()]
        public bool FormattedInd;

        // <remarks/>
        [XmlIgnore()]
        public bool FormattedIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TravelArrangerTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        public string Remark;
    }

    // <remarks/>
    [XmlType()]
    public class CountryNameType
    {

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class StateProvType
    {

        // <remarks/>
        [XmlAttribute()]
        public string StateCode;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class AddressTypeStreetNmbr : StreetNmbrType
    {

        // <remarks/>
        [XmlAttribute()]
        public string StreetNmbrSuffix;

        // <remarks/>
        [XmlAttribute()]
        public string StreetDirection;

        // <remarks/>
        [XmlAttribute()]
        public string RuralRouteNmbr;
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(AddressTypeStreetNmbr))]
    public class StreetNmbrType
    {

        // <remarks/>
        [XmlAttribute()]
        public string PO_Box;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class AddressTypeBldgRoom
    {

        // <remarks/>
        [XmlAttribute()]
        public bool BldgNameIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool BldgNameIndicatorSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class CompanyInfoTypeAddressInfo : AddressInfoType
    {

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class CustomerTypeAddress : AddressInfoType
    {

        // <remarks/>
        public CompanyNameType CompanyName;

        // <remarks/>
        public PersonNameType AddresseeName;

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
        public bool ExpireDateExclusiveIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CustomerTypeAddressValidationStatus ValidationStatus;

        // <remarks/>
        [XmlIgnore()]
        public bool ValidationStatusSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string ParentCompanyRef;
    }

    // <remarks/>
    [XmlType()]
    public enum CustomerTypeAddressValidationStatus
    {

        // <remarks/>
        SystemValidated,

        // <remarks/>
        UserValidated,

        // <remarks/>
        NotChecked
    }

    // <remarks/>
    [XmlType()]
    public enum TransferActionType
    {

        // <remarks/>
        Automatic,

        // <remarks/>
        Mandatory,

        // <remarks/>
        Selectable,

        // <remarks/>
        Never
    }

    // <remarks/>
    [XmlType()]
    public class EmployeeInfoType
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
    [XmlType()]
    public class InterestPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class SeatingPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

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
    [XmlType()]
    public class InsurancePrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class TicketDistribPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string DistribType;

        // <remarks/>
        [XmlAttribute(DataType = "duration")]
        public string TicketTime;

        // <remarks/>
        [XmlAttribute()]
        public string Remark;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class MediaEntertainPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class PetInfoPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class MealPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public MealType MealType;

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
    [XmlType()]
    public enum MealType
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
        VLML,

        // <remarks/>
        RGML
    }

    // <remarks/>
    [XmlType()]
    public class CommonPrefTypeContactPref
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string ContactMethodCode;
    }

    // <remarks/>
    [XmlType()]
    public class RelatedTravelerPrefType
    {

        // <remarks/>
        public UniqueID_Type UniqueID;

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class CustomerTypePhysChallName
    {

        // <remarks/>
        [XmlAttribute()]
        public bool PhysChallInd;

        // <remarks/>
        [XmlIgnore()]
        public bool PhysChallIndSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class CustomerTypeCitizenCountryName
    {

        // <remarks/>
        [XmlAttribute()]
        public string Code;
    }

    // <remarks/>
    [XmlType()]
    public class CustomerTypeTelephone
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
        public bool ExpireDateExclusiveIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string AreaCityCode;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneNumber;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string ParentCompanyRef;
    }

    // <remarks/>
    [XmlType()]
    public class CustomerTypeEmail : EmailType
    {

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string ParentCompanyRef;
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(CompanyInfoTypeEmail))]
    [XmlInclude(typeof(CustomerTypeEmail))]
    public class EmailType
    {

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool DefaultInd;

        // <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified;

        // <remarks/>
        [XmlAttribute("EmailType")]
        public string EmailType1;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public string Remark;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class CompanyInfoTypeEmail : EmailType
    {

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;
    }

    // <remarks/>
    [XmlType()]
    public enum CommissionInfoTypeShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlType()]
    public enum CommissionInfoTypeShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlType()]
    public class CustomerTypeURL : URL_Type
    {

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(CustomerTypeURL))]
    public class URL_Type
    {

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        public bool DefaultInd;

        // <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified;

        // <remarks/>
        [XmlText(DataType = "anyURI")]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class CustomerTypePaymentForm : PaymentFormType
    {

        // <remarks/>
        public CustomerTypePaymentFormAssociatedSupplier AssociatedSupplier;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool DefaultInd;

        // <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string ParentCompanyRef;
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(CompanyInfoTypePaymentForm))]
    [XmlInclude(typeof(CustomerTypePaymentForm))]
    public class PaymentFormType
    {

        // <remarks/>
        [XmlElement("Ticket", typeof(PaymentFormTypeTicket))]
        [XmlElement("Cash", typeof(PaymentFormTypeCash))]
        [XmlElement("PaymentCard", typeof(PaymentCardType))]
        [XmlElement("LoyaltyRedemption", typeof(PaymentFormTypeLoyaltyRedemption))]
        [XmlElement("MiscChargeOrder", typeof(PaymentFormTypeMiscChargeOrder))]
        [XmlElement("DirectBill", typeof(DirectBillType))]
        [XmlElement("BankAcct", typeof(BankAcctType))]
        [XmlElement("Voucher", typeof(PaymentFormTypeVoucher))]
        public object Item;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string CostCenterID;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public PaymentFormTypePaymentTransactionTypeCode PaymentTransactionTypeCode;

        // <remarks/>
        [XmlIgnore()]
        public bool PaymentTransactionTypeCodeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool GuaranteeIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool GuaranteeIndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string GuaranteeTypeCode;

        // <remarks/>
        [XmlAttribute()]
        public string GuaranteeID;

        // <remarks/>
        [XmlAttribute()]
        public string Remark;
    }

    // <remarks/>
    [XmlType()]
    public enum PaymentFormTypePaymentTransactionTypeCode
    {

        // <remarks/>
        charge,

        // <remarks/>
        reserve,

        // <remarks/>
        refund
    }

    // <remarks/>
    [XmlType()]
    public class CompanyInfoTypePaymentForm : PaymentFormType
    {

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class PaymentFormTypeTicket
    {

        // <remarks/>
        [XmlElement("ConjunctionTicketNbr")]
        public PaymentFormTypeTicketConjunctionTicketNbr[] ConjunctionTicketNbr;

        // <remarks/>
        [XmlAttribute()]
        public string TicketNumber;

        // <remarks/>
        [XmlAttribute()]
        public string OriginalTicketNumber;

        // <remarks/>
        [XmlAttribute()]
        public string OriginalIssuePlace;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime OriginalIssueDate;

        // <remarks/>
        [XmlIgnore()]
        public bool OriginalIssueDateSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string OriginalIssueIATA;

        // <remarks/>
        [XmlAttribute()]
        public string OriginalPaymentForm;

        // <remarks/>
        [XmlAttribute()]
        public PaymentFormTypeMiscChargeOrderCheckInhibitorType CheckInhibitorType;

        // <remarks/>
        [XmlIgnore()]
        public bool CheckInhibitorTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string[] CouponRPHs;

        // <remarks/>
        [XmlAttribute()]
        public PaymentFormTypeTicketReroutingType ReroutingType;

        // <remarks/>
        [XmlIgnore()]
        public bool ReroutingTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string ReasonForReroute;
    }

    // <remarks/>
    [XmlType()]
    public enum PaymentFormTypeTicketReroutingType
    {

        // <remarks/>
        voluntary,

        // <remarks/>
        involuntary
    }

    // <remarks/>
    [XmlType()]
    public enum PaymentFormTypeMiscChargeOrderCheckInhibitorType
    {

        // <remarks/>
        CheckDigit,

        // <remarks/>
        InterlineAgreement,

        // <remarks/>
        Both
    }

    // <remarks/>
    [XmlType()]
    public class PaymentFormTypeTicketConjunctionTicketNbr
    {

        // <remarks/>
        [XmlAttribute()]
        public string[] Coupons;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class PaymentFormTypeCash
    {

        // <remarks/>
        [XmlAttribute()]
        public bool CashIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool CashIndicatorSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class PaymentCardType
    {

        // <remarks/>
        public string CardHolderName;

        // <remarks/>
        public PaymentCardTypeCardIssuerName CardIssuerName;

        // <remarks/>
        public AddressType Address;

        // <remarks/>
        [XmlElement("Telephone")]
        public PaymentCardTypeTelephone[] Telephone;

        // <remarks/>
        [XmlElement("Email")]
        public EmailType[] Email;

        // <remarks/>
        [XmlElement("CustLoyalty")]
        public PaymentCardTypeCustLoyalty[] CustLoyalty;

        // <remarks/>
        public PaymentCardTypeSignatureOnFile SignatureOnFile;

        // <remarks/>
        public PaymentCardTypeMagneticStripe MagneticStripe;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string CardType;

        // <remarks/>
        [XmlAttribute()]
        public string CardCode;

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

        // <remarks/>
        [XmlAttribute()]
        public string MaskedCardNumber;

        // <remarks/>
        [XmlAttribute()]
        public string CardHolderRPH;

        // <remarks/>
        [XmlAttribute()]
        public bool ExtendPaymentIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool ExtendPaymentIndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string CountryOfIssue;

        // <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string ExtendedPaymentQuantity;

        // <remarks/>
        [XmlAttribute()]
        public bool SignatureOnFileIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool SignatureOnFileIndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string CompanyCardReference;

        // <remarks/>
        [XmlAttribute()]
        public string Remark;

        // <remarks/>
        [XmlAttribute()]
        public string EncryptionKey;
    }

    // <remarks/>
    [XmlType()]
    public class PaymentCardTypeCustLoyalty
    {

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
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public string[] VendorCode;

        // <remarks/>
        [XmlAttribute()]
        public bool PrimaryLoyaltyIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool PrimaryLoyaltyIndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string AllianceLoyaltyLevelName;

        // <remarks/>
        [XmlAttribute()]
        public string CustomerType;

        // <remarks/>
        [XmlAttribute()]
        public string CustomerValue;
    }

    // <remarks/>
    [XmlType()]
    public class PaymentCardTypeTelephone
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    [XmlType()]
    public class PaymentCardTypeSignatureOnFile
    {

        // <remarks/>
        [XmlAttribute()]
        public bool SignatureOnFileIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool SignatureOnFileIndicatorSpecified;

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
        public bool ExpireDateExclusiveIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class PaymentCardTypeMagneticStripe
    {

        // <remarks/>
        [XmlAttribute(DataType = "base64Binary")]
        public byte[] Track1;

        // <remarks/>
        [XmlAttribute(DataType = "base64Binary")]
        public byte[] Track2;

        // <remarks/>
        [XmlAttribute(DataType = "base64Binary")]
        public byte[] Track3;
    }

    // <remarks/>
    [XmlType()]
    public class RelatedTravelerType
    {

        // <remarks/>
        public UniqueID_Type UniqueID;

        // <remarks/>
        public PersonNameType PersonName;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Relation;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate;

        // <remarks/>
        [XmlIgnore()]
        public bool BirthDateSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class ContactPersonType
    {

        // <remarks/>
        public PersonNameType PersonName;

        // <remarks/>
        [XmlElement("Telephone")]
        public ContactPersonTypeTelephone[] Telephone;

        // <remarks/>
        [XmlElement("Address")]
        public AddressInfoType[] Address;

        // <remarks/>
        [XmlElement("Email")]
        public EmailType[] Email;

        // <remarks/>
        [XmlElement("URL")]
        public URL_Type[] URL;

        // <remarks/>
        [XmlElement("CompanyName")]
        public CompanyNameType[] CompanyName;

        // <remarks/>
        [XmlElement("EmployeeInfo")]
        public EmployeeInfoType[] EmployeeInfo;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool DefaultInd;

        // <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string ContactType;

        // <remarks/>
        [XmlAttribute()]
        public string Relation;

        // <remarks/>
        [XmlAttribute()]
        public bool EmergencyFlag;

        // <remarks/>
        [XmlIgnore()]
        public bool EmergencyFlagSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public string CommunicationMethodCode;

        // <remarks/>
        [XmlAttribute()]
        public string DocumentDistribMethodCode;
    }

    // <remarks/>
    [XmlType()]
    public class DocumentType
    {

        // <remarks/>
        [XmlElement("DocHolderName", typeof(string))]
        [XmlElement("DocHolderFormattedName", typeof(PersonNameType))]
        public object Item;

        // <remarks/>
        [XmlElement("DocLimitations")]
        public string[] DocLimitations;

        // <remarks/>
        [XmlArrayItem("AdditionalPersonName", IsNullable = false)]
        public string[] AdditionalPersonNames;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

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
        public DocumentTypeGender Gender;

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

        // <remarks/>
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string DocIssueStateProv;

        // <remarks/>
        [XmlAttribute()]
        public string DocIssueCountry;

        // <remarks/>
        [XmlAttribute()]
        public string BirthCountry;

        // <remarks/>
        [XmlAttribute()]
        public string BirthPlace;

        // <remarks/>
        [XmlAttribute()]
        public string DocHolderNationality;

        // <remarks/>
        [XmlAttribute()]
        public string ContactName;

        // <remarks/>
        [XmlAttribute()]
        public DocumentTypeHolderType HolderType;

        // <remarks/>
        [XmlIgnore()]
        public bool HolderTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Remark;

        // <remarks/>
        [XmlAttribute()]
        public string PostalCode;
    }

    // <remarks/>
    [XmlType()]
    public class CustomerTypeCustLoyalty
    {

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
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public string[] VendorCode;

        // <remarks/>
        [XmlAttribute()]
        public bool PrimaryLoyaltyIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool PrimaryLoyaltyIndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string AllianceLoyaltyLevelName;

        // <remarks/>
        [XmlAttribute()]
        public string CustomerType;

        // <remarks/>
        [XmlAttribute()]
        public string CustomerValue;

        // <remarks/>
        [XmlAttribute()]
        public string Remark;
    }

    // <remarks/>
    [XmlType()]
    public class CustomerTypeAdditionalLanguage
    {

        // <remarks/>
        [XmlAttribute(DataType = "language")]
        public string Code;
    }

    // <remarks/>
    [XmlType()]
    public enum DocumentTypeGender
    {

        // <remarks/>
        Male,

        // <remarks/>
        Female,

        // <remarks/>
        Unknown
    }

    // <remarks/>
    [XmlType()]
    public class CompanyInfoTypeTelephoneInfo
    {

        // <remarks/>
        [XmlAttribute()]
        public string PhoneNumber;

        // <remarks/>
        [XmlAttribute()]
        public string AreaCityCode;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class LoyaltyProgramType
    {

        // <remarks/>
        [XmlAttribute()]
        public string ProgramCode;

        // <remarks/>
        [XmlAttribute()]
        public CertificationTypeSingleVendorInd SingleVendorInd;

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
        [XmlAttribute()]
        public bool PrimaryLoyaltyIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool PrimaryLoyaltyIndicatorSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class OrganizationType
    {

        // <remarks/>
        public OrganizationTypeOrgMemberName OrgMemberName;

        // <remarks/>
        public CompanyNameType OrgName;

        // <remarks/>
        [XmlElement("RelatedOrgName")]
        public CompanyNameType[] RelatedOrgName;

        // <remarks/>
        [XmlElement("TravelArranger")]
        public TravelArrangerType[] TravelArranger;

        // <remarks/>
        [XmlAttribute()]
        public bool DefaultInd;

        // <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

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
        public bool ExpireDateExclusiveIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public OfficeLocationType OfficeType;

        // <remarks/>
        [XmlIgnore()]
        public bool OfficeTypeSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class CompanyInfoTypeTripPurpose
    {

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string Description;
    }

    // <remarks/>
    [XmlType()]
    public class EmployerType
    {

        // <remarks/>
        public CompanyNameType CompanyName;

        // <remarks/>
        [XmlElement("RelatedEmployer")]
        public CompanyNameType[] RelatedEmployer;

        // <remarks/>
        [XmlElement("EmployeeInfo")]
        public EmployeeInfoType[] EmployeeInfo;

        // <remarks/>
        [XmlElement("InternalRefNmbr")]
        public FreeTextType[] InternalRefNmbr;

        // <remarks/>
        [XmlElement("TravelArranger")]
        public TravelArrangerType[] TravelArranger;

        // <remarks/>
        [XmlElement("LoyaltyProgram")]
        public LoyaltyProgramType[] LoyaltyProgram;

        // <remarks/>
        [XmlAttribute()]
        public bool DefaultInd;

        // <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public OfficeLocationType OfficeType;

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

        // <remarks/>
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class InsuranceType
    {

        // <remarks/>
        public PersonNameType InsuredName;

        // <remarks/>
        public CompanyNameType InsuranceCompany;

        // <remarks/>
        public CompanyNameType Underwriter;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute("InsuranceType")]
        public string InsuranceType1;

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
        public bool ExpireDateExclusiveIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    [XmlType()]
    public class TravelClubType
    {

        // <remarks/>
        public CompanyNameType TravelClubName;

        // <remarks/>
        public TravelClubTypeClubMemberName ClubMemberName;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

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
        public bool ExpireDateExclusiveIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class AllianceConsortiumType
    {

        // <remarks/>
        [XmlElement("AllianceMember")]
        public AllianceConsortiumTypeAllianceMember[] AllianceMember;

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

        // <remarks/>
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class AgreementsTypeProfileSecurity
    {

        // <remarks/>
        [XmlAttribute()]
        public AgreementsTypeProfileSecurityAccessingOrganizationType AccessingOrganizationType;

        // <remarks/>
        [XmlIgnore()]
        public bool AccessingOrganizationTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string AccessingOrganizationID;

        // <remarks/>
        [XmlAttribute()]
        public AgreementsTypeProfileSecurityAccessType AccessType;

        // <remarks/>
        [XmlIgnore()]
        public bool AccessTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(ProfileTypeComment))]
    public class ParagraphType
    {

        // <remarks/>
        [XmlElement("Text")]
        public FormattedTextTextType[] Text;

        // <remarks/>
        [XmlElement("Image")]
        public string[] Image;

        // <remarks/>
        [XmlElement("URL", DataType = "anyURI")]
        public string[] URL;

        // <remarks/>
        [XmlElement("ListItem")]
        public ParagraphTypeListItem[] ListItem;

        // <remarks/>
        [XmlAttribute()]
        public string Name;

        // <remarks/>
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string ParagraphNumber;

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

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime PurgeDate;

        // <remarks/>
        [XmlIgnore()]
        public bool PurgeDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "language")]
        public string Language;
    }

    // <remarks/>
    [XmlType()]
    public class ProfileTypeCommentAuthorizedViewer
    {

        // <remarks/>
        [XmlAttribute()]
        public string ViewerCode;
    }

    // <remarks/>
    [XmlType()]
    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        // <remarks/>
        public string Name;

        // <remarks/>
        public string System;

        // <remarks/>
        public string Userid;

        // <remarks/>
        public string Password;
    }

    // <remarks/>
    [XmlType()]
    public class VehicleProfileRentalPrefType
    {

        // <remarks/>
        [XmlElement("LoyaltyPref")]
        public VehicleProfileRentalPrefTypeLoyaltyPref[] LoyaltyPref;

        // <remarks/>
        [XmlElement("VendorPref")]
        public CompanyNamePrefType[] VendorPref;

        // <remarks/>
        [XmlElement("PaymentFormPref")]
        public VehicleProfileRentalPrefTypePaymentFormPref[] PaymentFormPref;

        // <remarks/>
        [XmlElement("CoveragePref")]
        public VehicleProfileRentalPrefTypeCoveragePref[] CoveragePref;

        // <remarks/>
        [XmlElement("SpecialReqPref")]
        public VehicleSpecialReqPrefType[] SpecialReqPref;

        // <remarks/>
        [XmlElement("VehTypePref")]
        public VehiclePrefType[] VehTypePref;

        // <remarks/>
        [XmlElement("SpecialEquipPref")]
        public VehicleProfileRentalPrefTypeSpecialEquipPref[] SpecialEquipPref;

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool SmokingAllowed;

        // <remarks/>
        [XmlIgnore()]
        public bool SmokingAllowedSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool GasPrePay;

        // <remarks/>
        [XmlIgnore()]
        public bool GasPrePaySpecified;
    }

    // <remarks/>
    [XmlType()]
    public class AirlinePrefType
    {

        // <remarks/>
        [XmlElement("LoyaltyPref")]
        public LoyaltyPrefType[] LoyaltyPref;

        // <remarks/>
        [XmlElement("VendorPref")]
        public AirlinePrefTypeVendorPref[] VendorPref;

        // <remarks/>
        [XmlElement("PaymentFormPref")]
        public PaymentFormPrefType[] PaymentFormPref;

        // <remarks/>
        [XmlElement("AirportOriginPref")]
        public AirportPrefType[] AirportOriginPref;

        // <remarks/>
        public AirportPrefType AirportDestinationPref;

        // <remarks/>
        [XmlElement("AirportRoutePref")]
        public AirportPrefType[] AirportRoutePref;

        // <remarks/>
        [XmlElement("FareRestrictPref")]
        public AirlinePrefTypeFareRestrictPref[] FareRestrictPref;

        // <remarks/>
        [XmlElement("FarePref")]
        public AirlinePrefTypeFarePref[] FarePref;

        // <remarks/>
        [XmlElement("TourCodePref")]
        public AirlinePrefTypeTourCodePref[] TourCodePref;

        // <remarks/>
        [XmlElement("FlightTypePref")]
        public AirlinePrefTypeFlightTypePref[] FlightTypePref;

        // <remarks/>
        [XmlElement("EquipPref")]
        public EquipmentTypePref[] EquipPref;

        // <remarks/>
        [XmlElement("CabinPref")]
        public AirlinePrefTypeCabinPref[] CabinPref;

        // <remarks/>
        [XmlElement("SeatPref")]
        public AirlinePrefTypeSeatPref[] SeatPref;

        // <remarks/>
        [XmlElement("TicketDistribPref")]
        public TicketDistribPrefType[] TicketDistribPref;

        // <remarks/>
        [XmlElement("MealPref")]
        public MealPrefType[] MealPref;

        // <remarks/>
        [XmlElement("SpecRequestPref")]
        public SpecRequestPrefType[] SpecRequestPref;

        // <remarks/>
        [XmlElement("SSR_Pref")]
        public AirlinePrefTypeSSR_Pref[] SSR_Pref;

        // <remarks/>
        public TPA_ExtensionsType TPA_Extensions;

        // <remarks/>
        [XmlElement("MediaEntertainPref")]
        public MediaEntertainPrefType[] MediaEntertainPref;

        // <remarks/>
        [XmlElement("PetInfoPref")]
        public PetInfoPrefType[] PetInfoPref;

        // <remarks/>
        public AirlinePrefTypeAccountInformation AccountInformation;

        // <remarks/>
        [XmlElement("OSI_Pref")]
        public OtherServiceInfoType[] OSI_Pref;

        // <remarks/>
        [XmlElement("KeywordPref")]
        public AirlinePrefTypeKeywordPref[] KeywordPref;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool SmokingAllowed;

        // <remarks/>
        [XmlIgnore()]
        public bool SmokingAllowedSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string PassengerTypeCode;

        // <remarks/>
        [XmlAttribute()]
        public TicketType AirTicketType;

        // <remarks/>
        [XmlIgnore()]
        public bool AirTicketTypeSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class HotelPrefType
    {

        // <remarks/>
        [XmlElement("LoyaltyPref")]
        public LoyaltyPrefType[] LoyaltyPref;

        // <remarks/>
        [XmlElement("PaymentFormPref")]
        public PaymentFormPrefType[] PaymentFormPref;

        // <remarks/>
        [XmlElement("HotelChainPref")]
        public CompanyNamePrefType[] HotelChainPref;

        // <remarks/>
        [XmlElement("PropertyNamePref")]
        public PropertyNamePrefType[] PropertyNamePref;

        // <remarks/>
        [XmlElement("PropertyLocationPref")]
        public PropertyLocationPrefType[] PropertyLocationPref;

        // <remarks/>
        [XmlElement("PropertyTypePref")]
        public PropertyTypePrefType[] PropertyTypePref;

        // <remarks/>
        [XmlElement("PropertyClassPref")]
        public PropertyClassPrefType[] PropertyClassPref;

        // <remarks/>
        [XmlElement("PropertyAmenityPref")]
        public PropertyAmenityPrefType[] PropertyAmenityPref;

        // <remarks/>
        [XmlElement("RoomAmenityPref")]
        public RoomAmenityPrefType[] RoomAmenityPref;

        // <remarks/>
        [XmlElement("RoomLocationPref")]
        public RoomLocationPrefType[] RoomLocationPref;

        // <remarks/>
        [XmlElement("BedTypePref")]
        public BedTypePrefType[] BedTypePref;

        // <remarks/>
        [XmlElement("FoodSrvcPref")]
        public FoodSrvcPrefType[] FoodSrvcPref;

        // <remarks/>
        [XmlElement("MediaEntertainPref")]
        public MediaEntertainPrefType[] MediaEntertainPref;

        // <remarks/>
        [XmlElement("PetInfoPref")]
        public PetInfoPrefType[] PetInfoPref;

        // <remarks/>
        [XmlElement("MealPref")]
        public MealPrefType[] MealPref;

        // <remarks/>
        [XmlElement("RecreationSrvcPref")]
        public RecreationSrvcPrefType[] RecreationSrvcPref;

        // <remarks/>
        [XmlElement("BusinessSrvcPref")]
        public BusinessSrvcPrefType[] BusinessSrvcPref;

        // <remarks/>
        [XmlElement("PersonalSrvcPref")]
        public PersonalSrvcPrefType[] PersonalSrvcPref;

        // <remarks/>
        [XmlElement("SecurityFeaturePref")]
        public SecurityFeaturePrefType[] SecurityFeaturePref;

        // <remarks/>
        [XmlElement("PhysChallFeaturePref")]
        public PhysChallFeaturePrefType[] PhysChallFeaturePref;

        // <remarks/>
        [XmlElement("SpecRequestPref")]
        public SpecRequestPrefType[] SpecRequestPref;

        // <remarks/>
        public TPA_ExtensionsType TPA_Extensions;

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool SmokingAllowed;

        // <remarks/>
        [XmlIgnore()]
        public bool SmokingAllowedSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RatePlanCode;

        // <remarks/>
        [XmlAttribute()]
        public string HotelGuestType;
    }

    // <remarks/>
    [XmlType()]
    public class CustomerTypePaymentFormAssociatedSupplier
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
    }

    // <remarks/>
    [XmlType()]
    public class PaymentFormTypeLoyaltyRedemption
    {

        // <remarks/>
        [XmlElement("LoyaltyCertificate")]
        public PaymentFormTypeLoyaltyRedemptionLoyaltyCertificate[] LoyaltyCertificate;

        // <remarks/>
        [XmlAttribute()]
        public string CertificateNumber;

        // <remarks/>
        [XmlAttribute()]
        public string MemberNumber;

        // <remarks/>
        [XmlAttribute()]
        public string ProgramName;

        // <remarks/>
        [XmlAttribute()]
        public string PromotionCode;

        // <remarks/>
        [XmlAttribute()]
        public string[] PromotionVendorCode;

        // <remarks/>
        [XmlAttribute(DataType = "positiveInteger")]
        public string RedemptionQuantity;
    }

    // <remarks/>
    [XmlType()]
    public class PaymentFormTypeMiscChargeOrder
    {

        // <remarks/>
        [XmlAttribute()]
        public string TicketNumber;

        // <remarks/>
        [XmlAttribute()]
        public string OriginalTicketNumber;

        // <remarks/>
        [XmlAttribute()]
        public string OriginalIssuePlace;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime OriginalIssueDate;

        // <remarks/>
        [XmlIgnore()]
        public bool OriginalIssueDateSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string OriginalIssueIATA;

        // <remarks/>
        [XmlAttribute()]
        public string OriginalPaymentForm;

        // <remarks/>
        [XmlAttribute()]
        public PaymentFormTypeMiscChargeOrderCheckInhibitorType CheckInhibitorType;

        // <remarks/>
        [XmlIgnore()]
        public bool CheckInhibitorTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string[] CouponRPHs;

        // <remarks/>
        [XmlAttribute()]
        public bool PaperMCO_ExistInd;

        // <remarks/>
        [XmlIgnore()]
        public bool PaperMCO_ExistIndSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class DirectBillType
    {

        // <remarks/>
        public DirectBillTypeCompanyName CompanyName;

        // <remarks/>
        public AddressInfoType Address;

        // <remarks/>
        public EmailType Email;

        // <remarks/>
        public DirectBillTypeTelephone Telephone;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string DirectBill_ID;

        // <remarks/>
        [XmlAttribute()]
        public string BillingNumber;
    }

    // <remarks/>
    [XmlType()]
    public class BankAcctType
    {

        // <remarks/>
        public string BankAcctName;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd;

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

        // <remarks/>
        [XmlAttribute()]
        public bool ChecksAcceptedInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ChecksAcceptedIndSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class PaymentFormTypeVoucher
    {

        // <remarks/>
        [XmlAttribute()]
        public string SeriesCode;

        // <remarks/>
        [XmlAttribute()]
        public string BillingNumber;

        // <remarks/>
        [XmlAttribute()]
        public string SupplierIdentifier;

        // <remarks/>
        [XmlAttribute()]
        public string Identifier;

        // <remarks/>
        [XmlAttribute()]
        public string ValueType;

        // <remarks/>
        [XmlAttribute()]
        public bool ElectronicIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool ElectronicIndicatorSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class PaymentCardTypeCardIssuerName
    {

        // <remarks/>
        [XmlAttribute()]
        public string BankID;
    }

    // <remarks/>
    [XmlType()]
    public class ContactPersonTypeTelephone
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    [XmlType()]
    public enum DocumentTypeHolderType
    {

        // <remarks/>
        Infant,

        // <remarks/>
        HeadOfHousehold
    }

    // <remarks/>
    [XmlType()]
    public enum OfficeLocationType
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
    [XmlType()]
    public enum AgreementsTypeProfileSecurityAccessingOrganizationType
    {

        // <remarks/>
        ProfileOwner,

        // <remarks/>
        IATA,

        // <remarks/>
        Other
    }

    // <remarks/>
    [XmlType()]
    public enum AgreementsTypeProfileSecurityAccessType
    {

        // <remarks/>
        ReadOnly,

        // <remarks/>
        ReadWrite,

        // <remarks/>
        NoAccess
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(ParagraphTypeListItem))]
    public class FormattedTextTextType
    {

        // <remarks/>
        [XmlAttribute()]
        public bool Formatted;

        // <remarks/>
        [XmlIgnore()]
        public bool FormattedSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "language")]
        public string Language;

        // <remarks/>
        [XmlAttribute()]
        public FormattedTextTextTypeTextFormat TextFormat;

        // <remarks/>
        [XmlIgnore()]
        public bool TextFormatSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class ParagraphTypeListItem : FormattedTextTextType
    {

        // <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string ListItem;
    }

    // <remarks/>
    [XmlType()]
    public class VehiclePrefType : VehicleCoreType
    {

        // <remarks/>
        public VehiclePrefTypeVehMakeModel VehMakeModel;

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType TypePref;

        // <remarks/>
        [XmlIgnore()]
        public bool TypePrefSpecified;

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType ClassPref;

        // <remarks/>
        [XmlIgnore()]
        public bool ClassPrefSpecified;

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType AirConditionPref;

        // <remarks/>
        [XmlIgnore()]
        public bool AirConditionPrefSpecified;

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType TransmissionPref;

        // <remarks/>
        [XmlIgnore()]
        public bool TransmissionPrefSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string VendorCarType;

        // <remarks/>
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string VehicleQty;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;
    }

    // <remarks/>
    [XmlType()]
    public class VehicleSpecialReqPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class VehicleProfileRentalPrefTypeCoveragePref
    {
    }

    // <remarks/>
    [XmlType()]
    public class VehicleProfileRentalPrefTypePaymentFormPref
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    [XmlType()]
    public class VehicleProfileRentalPrefTypeLoyaltyPref
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    [XmlType()]
    public class VehicleProfileRentalPrefTypeSpecialEquipPref
    {

        // <remarks/>
        [XmlAttribute()]
        public ActionType Action;

        // <remarks/>
        [XmlIgnore()]
        public bool ActionSpecified;
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(AirportPrefType))]
    public class LocationType
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class AirportPrefType : LocationType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class AirlinePrefTypeTourCodePref
    {

        // <remarks/>
        [XmlElement("TourCodeInfo", typeof(AirlinePrefTypeTourCodePrefTourCodeInfo))]
        [XmlElement("StaffTourCodeInfo", typeof(AirlinePrefTypeTourCodePrefStaffTourCodeInfo))]
        public object Item;

        // <remarks/>
        [XmlAttribute()]
        public string PassengerTypeCode;

        // <remarks/>
        [XmlAttribute()]
        public string[] AirlineVendorPrefRPH;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class AirlinePrefTypeFarePref
    {

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string Description;

        // <remarks/>
        [XmlAttribute()]
        public string[] AirlineVendorPrefRPH;

        // <remarks/>
        [XmlAttribute()]
        public string RateCategoryCode;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class AirlinePrefTypeFareRestrictPref
    {

        // <remarks/>
        [XmlAttribute()]
        public string FareRestriction;

        // <remarks/>
        [XmlAttribute()]
        public string Date;
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(EquipmentTypePref))]
    public class EquipmentType
    {

        // <remarks/>
        [XmlAttribute()]
        public string AirEquipType;

        // <remarks/>
        [XmlAttribute()]
        public bool ChangeofGauge;

        // <remarks/>
        [XmlIgnore()]
        public bool ChangeofGaugeSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class EquipmentTypePref : EquipmentType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool WideBody;

        // <remarks/>
        [XmlIgnore()]
        public bool WideBodySpecified;
    }

    // <remarks/>
    [XmlType()]
    public class AirlinePrefTypeFlightTypePref
    {

        // <remarks/>
        [XmlAttribute()]
        public FlightTypeType FlightType;

        // <remarks/>
        [XmlIgnore()]
        public bool FlightTypeSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string MaxConnections;

        // <remarks/>
        [XmlAttribute()]
        public AirlinePrefTypeFlightTypePrefNonScheduledFltInfo NonScheduledFltInfo;

        // <remarks/>
        [XmlIgnore()]
        public bool NonScheduledFltInfoSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool BackhaulIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool BackhaulIndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool GroundTransportIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool GroundTransportIndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool DirectAndNonStopOnlyInd;

        // <remarks/>
        [XmlIgnore()]
        public bool DirectAndNonStopOnlyIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool NonStopsOnlyInd;

        // <remarks/>
        [XmlIgnore()]
        public bool NonStopsOnlyIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool OnlineConnectionsOnlyInd;

        // <remarks/>
        [XmlIgnore()]
        public bool OnlineConnectionsOnlyIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public AirlinePrefTypeFlightTypePrefRoutingType RoutingType;

        // <remarks/>
        [XmlIgnore()]
        public bool RoutingTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool ExcludeTrainInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ExcludeTrainIndSpecified;
    }

    // <remarks/>
    [XmlType()]
    public enum FlightTypeType
    {

        // <remarks/>
        Nonstop,

        // <remarks/>
        Direct,

        // <remarks/>
        Connection,

        // <remarks/>
        SingleConnection,

        // <remarks/>
        DoubleConnection,

        // <remarks/>
        OneStopOnly
    }

    // <remarks/>
    [XmlType()]
    public class AirlinePrefTypeCabinPref
    {

        // <remarks/>
        [XmlAttribute()]
        public CabinType Cabin;

        // <remarks/>
        [XmlIgnore()]
        public bool CabinSpecified;
    }

    // <remarks/>
    [XmlType()]
    public enum CabinType
    {

        // <remarks/>
        First,

        // <remarks/>
        Business,

        // <remarks/>
        Economy
    }

    // <remarks/>
    [XmlType()]
    public class AirlinePrefTypeAccountInformation
    {

        // <remarks/>
        public AirlinePrefTypeAccountInformationTaxRegistrationDetails TaxRegistrationDetails;

        // <remarks/>
        [XmlAttribute()]
        public string Number;

        // <remarks/>
        [XmlAttribute()]
        public string CostCenter;

        // <remarks/>
        [XmlAttribute()]
        public string CompanyNumber;

        // <remarks/>
        [XmlAttribute()]
        public string ClientReference;
    }

    // <remarks/>
    [XmlType()]
    public class AirlinePrefTypeSSR_Pref
    {

        // <remarks/>
        [XmlAttribute()]
        public string SSR_Code;

        // <remarks/>
        [XmlAttribute()]
        public string VendorCode;

        // <remarks/>
        [XmlAttribute(DataType = "positiveInteger")]
        public string NumberInParty;

        // <remarks/>
        [XmlAttribute()]
        public string DefaultStatusCode;

        // <remarks/>
        [XmlAttribute()]
        public string Remark;

        // <remarks/>
        [XmlAttribute()]
        public string LookupKey;

        // <remarks/>
        [XmlAttribute()]
        public string[] AirlineVendorPrefRPH;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferActionType;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionTypeSpecified;
    }

    // <remarks/>
    [XmlType()]
    public class AirlinePrefTypeSeatPref
    {

        // <remarks/>
        [XmlAttribute()]
        public AirlinePrefTypeSeatPrefFlightDistanceQualifier FlightDistanceQualifier;

        // <remarks/>
        [XmlIgnore()]
        public bool FlightDistanceQualifierSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool InternationalIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool InternationalIndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string[] AirlineVendorPrefRPH;

        // <remarks/>
        [XmlAttribute()]
        public string PassengerTypeCode;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;
    }

    // <remarks/>
    [XmlType()]
    public enum AirlinePrefTypeSeatPrefFlightDistanceQualifier
    {

        // <remarks/>
        LongHaul,

        // <remarks/>
        ShortHaul
    }

    // <remarks/>
    [XmlType()]
    public class OtherServiceInfoType
    {

        // <remarks/>
        [XmlElement("TravelerRefNumber")]
        public OtherServiceInfoTypeTravelerRefNumber[] TravelerRefNumber;

        // <remarks/>
        public CompanyNameType Airline;

        // <remarks/>
        public string Text;

        // <remarks/>
        [XmlAttribute()]
        public string Code;
    }

    // <remarks/>
    [XmlType()]
    public class AirlinePrefTypeKeywordPref
    {

        // <remarks/>
        [XmlAttribute()]
        public string VendorCode;

        // <remarks/>
        [XmlAttribute()]
        public string Description;

        // <remarks/>
        [XmlAttribute()]
        public string Keyword;

        // <remarks/>
        [XmlAttribute()]
        public string StatusCode;

        // <remarks/>
        [XmlAttribute(DataType = "positiveInteger")]
        public string NumberInParty;

        // <remarks/>
        [XmlAttribute()]
        public string[] AirlineVendorRPH;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified;
    }

    // <remarks/>
    [XmlType()]
    public enum TicketType
    {

        // <remarks/>
        eTicket,

        // <remarks/>
        Paper,

        // <remarks/>
        MCO
    }

    // <remarks/>
    [XmlType()]
    public class PropertyAmenityPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string PropertyAmenityType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class PropertyClassPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string PropertyClassType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class PropertyTypePrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string PropertyType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class PropertyLocationPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string PropertyLocationType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class PropertyNamePrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string HotelCode;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class RecreationSrvcPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RecreationSrvcType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class FoodSrvcPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string FoodSrvcType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class BedTypePrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string BedType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class RoomLocationPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RoomLocationType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class RoomAmenityPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RoomAmenity;

        // <remarks/>
        [XmlAttribute()]
        public string ExistsCode;

        // <remarks/>
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string Quantity;

        // <remarks/>
        [XmlAttribute()]
        public string QualityLevel;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class PhysChallFeaturePrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string PhysChallFeatureType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class SecurityFeaturePrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class PersonalSrvcPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class BusinessSrvcPrefType
    {

        // <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel;

        // <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string BusinessSrvcType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlType()]
    public class PaymentFormTypeLoyaltyRedemptionLoyaltyCertificate
    {

        // <remarks/>
        [XmlAttribute()]
        public string ID_Context;

        // <remarks/>
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string NmbrOfNights;

        // <remarks/>
        [XmlAttribute()]
        public PaymentFormTypeLoyaltyRedemptionLoyaltyCertificateFormat Format;

        // <remarks/>
        [XmlIgnore()]
        public bool FormatSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Status;
    }

    // <remarks/>
    [XmlType()]
    public enum PaymentFormTypeLoyaltyRedemptionLoyaltyCertificateFormat
    {

        // <remarks/>
        Paper,

        // <remarks/>
        Electronic
    }

    // <remarks/>
    [XmlType()]
    public class DirectBillTypeTelephone
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    [XmlType()]
    public enum FormattedTextTextTypeTextFormat
    {

        // <remarks/>
        PlainText,

        // <remarks/>
        HTML
    }

    // <remarks/>
    [XmlType()]
    [XmlInclude(typeof(VehiclePrefType))]
    public class VehicleCoreType
    {

        // <remarks/>
        public VehicleCoreTypeVehType VehType;

        // <remarks/>
        public VehicleCoreTypeVehClass VehClass;

        // <remarks/>
        [XmlAttribute()]
        public bool AirConditionInd;

        // <remarks/>
        [XmlIgnore()]
        public bool AirConditionIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public VehicleTransmissionType TransmissionType;

        // <remarks/>
        [XmlIgnore()]
        public bool TransmissionTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public VehicleCoreTypeFuelType FuelType;

        // <remarks/>
        [XmlIgnore()]
        public bool FuelTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public VehicleCoreTypeDriveType DriveType;

        // <remarks/>
        [XmlIgnore()]
        public bool DriveTypeSpecified;
    }

    // <remarks/>
    [XmlType()]
    public enum VehicleTransmissionType
    {

        // <remarks/>
        Automatic,

        // <remarks/>
        Manual
    }

    // <remarks/>
    [XmlType()]
    public enum VehicleCoreTypeFuelType
    {

        // <remarks/>
        Unspecified,

        // <remarks/>
        Diesel,

        // <remarks/>
        Hybrid,

        // <remarks/>
        Electric,

        // <remarks/>
        LPG_CompressedGas,

        // <remarks/>
        Hydrogen,

        // <remarks/>
        MultiFuel,

        // <remarks/>
        Petrol,

        // <remarks/>
        Ethanol
    }

    // <remarks/>
    [XmlType()]
    public enum VehicleCoreTypeDriveType
    {

        // <remarks/>
        AWD,

        // <remarks/>
        [XmlEnum("4WD")]
        Item4WD,

        // <remarks/>
        Unspecified
    }

    // <remarks/>
    [XmlType()]
    public class VehiclePrefTypeVehMakeModel
    {

        // <remarks/>
        [XmlAttribute(DataType = "gYear")]
        public string ModelYear;
    }

    // <remarks/>
    [XmlType()]
    public enum ActionType
    {

        // <remarks/>
        [XmlEnum("Add-Update")]
        AddUpdate,

        // <remarks/>
        Cancel,

        // <remarks/>
        Delete,

        // <remarks/>
        Add,

        // <remarks/>
        Replace
    }

    // <remarks/>
    [XmlType()]
    public class AirlinePrefTypeTourCodePrefTourCodeInfo
    {

        // <remarks/>
        [XmlAttribute()]
        public string TourTypeCode;

        // <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string YearNum;

        // <remarks/>
        [XmlAttribute()]
        public string PromotionCode;

        // <remarks/>
        [XmlAttribute()]
        public string[] PromotionVendorCode;

        // <remarks/>
        [XmlAttribute()]
        public string PartyID;
    }

    // <remarks/>
    [XmlType()]
    public class OtherServiceInfoTypeTravelerRefNumber
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public string SurnameRefNumber;
    }

    // <remarks/>
    [XmlType()]
    public class AirlinePrefTypeAccountInformationTaxRegistrationDetails
    {

        // <remarks/>
        [XmlAttribute()]
        public string TaxID;

        // <remarks/>
        [XmlAttribute()]
        public string RecipientName;

        // <remarks/>
        [XmlAttribute()]
        public string RecipientAddress;
    }

    // <remarks/>
    [XmlType()]
    public enum AirlinePrefTypeFlightTypePrefNonScheduledFltInfo
    {

        // <remarks/>
        ChartersOnly,

        // <remarks/>
        ExcludeCharters,

        // <remarks/>
        All
    }

    // <remarks/>
    [XmlType()]
    public enum AirlinePrefTypeFlightTypePrefRoutingType
    {

        // <remarks/>
        Normal,

        // <remarks/>
        Mirror
    }

    // <remarks/>
    [XmlType()]
    public class AirlinePrefTypeTourCodePrefStaffTourCodeInfo
    {

        // <remarks/>
        [XmlAttribute()]
        public AirlinePrefTypeTourCodePrefStaffTourCodeInfoStaffType StaffType;

        // <remarks/>
        [XmlIgnore()]
        public bool StaffTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string EmployeeID;

        // <remarks/>
        [XmlAttribute()]
        public string VendorCode;

        // <remarks/>
        [XmlAttribute()]
        public string Description;
    }

    // <remarks/>
    [XmlType()]
    public class VehicleCoreTypeVehClass
    {

        // <remarks/>
        [XmlAttribute()]
        public string Size;
    }

    // <remarks/>
    [XmlType()]
    public class VehicleCoreTypeVehType
    {

        // <remarks/>
        [XmlAttribute()]
        public string VehicleCategory;

        // <remarks/>
        [XmlAttribute()]
        public string DoorCount;
    }

    // <remarks/>
    [XmlType()]
    public enum AirlinePrefTypeTourCodePrefStaffTourCodeInfoStaffType
    {

        // <remarks/>
        Current,

        // <remarks/>
        Duty,

        // <remarks/>
        CabinCrew,

        // <remarks/>
        Retired,

        // <remarks/>
        TechCrew,

        // <remarks/>
        UnaccompaniedFamilyMember,

        // <remarks/>
        OtherAirlinePersonnel
    }

    // <remarks/>
    [XmlRoot()]
    public class OtherServiceInformationType
    {

        // <remarks/>
        public Airline Airline;

        // <remarks/>
        public string Text;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Airline
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
    public class Remarks
    {

        // <remarks/>
        [XmlElement("Remark")]
        public Remark[] Remark;

        // <remarks/>
        public TPA_Extensions TPA_Extensions;

        // <remarks/>
        [XmlAttribute()]
        public RemarksRemarkType RemarkType;

        // <remarks/>
        [XmlIgnore()]
        public bool RemarkTypeSpecified;
    }

    // <remarks/>
    public enum RemarksRemarkType
    {

        // <remarks/>
        General,

        // <remarks/>
        TravelPolicy

    }

    // <remarks/>
    [XmlRoot()]
    public class Remark
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction;

        // <remarks/>
        [XmlText()]
        public string Value;
    }
}