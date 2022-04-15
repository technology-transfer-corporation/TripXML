Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmProfileCommon
    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(CommissionInfoType)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(CertificationType))> _
    Public Class FreeTextType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CommissionInfoType
        Inherits FreeTextType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TravelArrangerTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TravelArrangerTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CommissionPlanCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As Decimal

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AmountSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CertificationType
        Inherits FreeTextType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SingleVendorInd As CertificationTypeSingleVendorInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SingleVendorIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public EffectiveDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public EffectiveDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public ExpireDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDateExclusiveIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateExclusiveIndicatorSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum TravelArrangerTypeShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum TravelArrangerTypeShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum CertificationTypeSingleVendorInd

        '<remarks/>
        SingleVndr

        '<remarks/>
        Alliance
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(ProfileTypeUserID)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(SourceTypeRequestorID))> _
    Public Class UniqueID_Type

        '<remarks/>
        Public CompanyName As CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")> _
        Public URL As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Instance As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID_Context As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class ProfileTypeUserID
        Inherits UniqueID_Type

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PinNumber As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class SourceTypeRequestorID
        Inherits UniqueID_Type

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MessagePassword As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(AllianceConsortiumTypeAllianceMember)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(DirectBillTypeCompanyName)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(CompanyNamePrefType)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(AirlinePrefTypeVendorPref)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(TravelArrangerType))> _
    Public Class CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CompanyShortName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelSector As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CodeContext As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Division As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Department As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AllianceConsortiumTypeAllianceMember
        Inherits CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MemberCode As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class DirectBillTypeCompanyName
        Inherits CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ContactName As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(AirlinePrefTypeVendorPref))> _
    Public Class CompanyNamePrefType
        Inherits CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum PreferLevelType

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred

        '<remarks/>
        Required

        '<remarks/>
        NoPreference
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirlinePrefTypeVendorPref
        Inherits CompanyNamePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class TravelArrangerType
        Inherits CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DefaultIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TravelArrangerTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TravelArrangerTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute("TravelArrangerType")> _
        Public TravelArrangerType1 As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Remark As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class ProfileType

        '<remarks/>
        Public Accesses As AccessesType

        '<remarks/>
        Public Customer As CustomerType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("UserID")> _
        Public UserID() As ProfileTypeUserID

        '<remarks/>
        Public PrefCollections As PreferencesType

        '<remarks/>
        Public CompanyInfo As CompanyInfoType

        '<remarks/>
        Public Affiliations As AffiliationsType

        '<remarks/>
        Public Agreements As AgreementsType

        '<remarks/>
        Public Remarks As Remarks

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Comment", IsNullable:=False)> _
        Public Comments() As ProfileTypeComment

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareAllSynchInd As YesNoType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareAllSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareAllMarketInd As YesNoType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareAllMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute("ProfileType")> _
        Public ProfileType1 As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CreateDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public CreateDateTimeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CreatorID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LastModifyDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public LastModifyDateTimeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LastModifierID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public PurgeDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PurgeDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StatusCode() As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AccessesType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Access")> _
        Public Access() As AccessesTypeAccess

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TravelArrangerTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TravelArrangerTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CreateDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public CreateDateTimeSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CustomerType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PersonName")> _
        Public PersonName() As PersonNameType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Telephone")> _
        Public Telephone() As CustomerTypeTelephone

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Email")> _
        Public Email() As CustomerTypeEmail

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Address")> _
        Public Address() As CustomerTypeAddress

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("URL")> _
        Public URL() As CustomerTypeURL

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CitizenCountryName")> _
        Public CitizenCountryName() As CustomerTypeCitizenCountryName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PhysChallName")> _
        Public PhysChallName() As CustomerTypePhysChallName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PetInfo")> _
        Public PetInfo() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentForm")> _
        Public PaymentForm() As CustomerTypePaymentForm

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RelatedTraveler")> _
        Public RelatedTraveler() As RelatedTravelerType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ContactPerson")> _
        Public ContactPerson() As ContactPersonType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Document")> _
        Public Document() As DocumentType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CustLoyalty")> _
        Public CustLoyalty() As CustomerTypeCustLoyalty

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("EmployeeInfo")> _
        Public EmployeeInfo() As EmployeeInfoType

        '<remarks/>
        Public EmployerInfo As CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AdditionalLanguage")> _
        Public AdditionalLanguage() As CustomerTypeAdditionalLanguage

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Gender As DocumentTypeGender

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public GenderSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Deceased As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DeceasedSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LockoutType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public BirthDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public BirthDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")> _
        Public DecimalPlaces As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VIP_Indicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public VIP_IndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public [Text] As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CustomerValue As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MaritalStatus As CustomerTypeMaritalStatus

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public MaritalStatusSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreviouslyMarriedIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreviouslyMarriedIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")> _
        Public ChildQuantity As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum CustomerTypeMaritalStatus

        '<remarks/>
        Annulled

        '<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("Co-habitating")> _
        Cohabitating

        '<remarks/>
        Divorced

        '<remarks/>
        Engaged

        '<remarks/>
        Married

        '<remarks/>
        Separated

        '<remarks/>
        [Single]

        '<remarks/>
        Widowed

        '<remarks/>
        Unknown
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PreferencesType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PrefCollection")> _
        Public PrefCollection() As PreferencesTypePrefCollection

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TravelArrangerTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TravelArrangerTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CompanyInfoType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CompanyName")> _
        Public CompanyName() As CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AddressInfo")> _
        Public AddressInfo() As CompanyInfoTypeAddressInfo

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TelephoneInfo")> _
        Public TelephoneInfo() As CompanyInfoTypeTelephoneInfo

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Email")> _
        Public Email() As CompanyInfoTypeEmail

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("URL")> _
        Public URL() As URL_Type

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("BusinessLocale")> _
        Public BusinessLocale() As AddressType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentForm")> _
        Public PaymentForm() As CompanyInfoTypePaymentForm

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ContactPerson")> _
        Public ContactPerson() As ContactPersonType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TravelArranger")> _
        Public TravelArranger() As TravelArrangerType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyProgram")> _
        Public LoyaltyProgram() As LoyaltyProgramType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TripPurpose")> _
        Public TripPurpose() As CompanyInfoTypeTripPurpose

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("OtherServiceInformation")> _
        Public OtherServiceInformation() As OtherServiceInformationType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")> _
        Public DecimalPlaces As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class Comments
        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Comment", IsNullable:=False)> _
        Public Comment() As ProfileTypeComment
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AffiliationsType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Organization")> _
        Public Organization() As OrganizationType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Employer")> _
        Public Employer() As EmployerType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TravelArranger")> _
        Public TravelArranger() As TravelArrangerType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TravelClub")> _
        Public TravelClub() As TravelClubType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Insurance")> _
        Public Insurance() As InsuranceType

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TravelArrangerTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TravelArrangerTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AgreementsType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Certification")> _
        Public Certification() As CertificationType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AllianceConsortium")> _
        Public AllianceConsortium() As AllianceConsortiumType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CommissionInfo")> _
        Public CommissionInfo() As CommissionInfoType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ProfileSecurity")> _
        Public ProfileSecurity() As AgreementsTypeProfileSecurity

        '<remarks/>
        Public ContractInformation As ParagraphType

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TravelArrangerTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TravelArrangerTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class ProfileTypeComment
        Inherits ParagraphType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AuthorizedViewer")> _
        Public AuthorizedViewer() As ProfileTypeCommentAuthorizedViewer

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CommentOriginatorCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public GuestViewable As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public GuestViewableSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Category As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirlineVendorPrefRPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public ActionDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ActionDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TPA_Extensions
        Inherits TPA_ExtensionsType

    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(TPA_Extensions))> _
    Public Class TPA_ExtensionsType

        '<remarks/>
        Public Provider As Provider

        Public CustomCheckRule As CustomCheckRule
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CustomCheckRule

        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String

    End Class
    
    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum YesNoType

        '<remarks/>
        Yes

        '<remarks/>
        No
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AccessesTypeAccess

        '<remarks/>
        Public AccessPerson As PersonNameType

        '<remarks/>
        Public AccessComment As FreeTextType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ActionType As AccessesTypeAccessActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ActionTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ActionDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ActionDateTimeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum AccessesTypeAccessActionType

        '<remarks/>
        Create

        '<remarks/>
        Read

        '<remarks/>
        Update

        '<remarks/>
        Delete
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PreferencesTypePrefCollection

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CommonPref")> _
        Public CommonPref() As CommonPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("VehicleRentalPref")> _
        Public VehicleRentalPref() As VehicleProfileRentalPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AirlinePref")> _
        Public AirlinePref() As AirlinePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("HotelPref")> _
        Public HotelPref() As HotelPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("OtherSrvcPref")> _
        Public OtherSrvcPref() As OtherSrvcPrefType

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TravelArrangerTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TravelArrangerTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelPurpose As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(TravelClubTypeClubMemberName)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(OrganizationTypeOrgMemberName))> _
    Public Class PersonNameType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("NamePrefix")> _
        Public NamePrefix() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("GivenName")> _
        Public GivenName() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MiddleName")> _
        Public MiddleName() As String

        '<remarks/>
        Public SurnamePrefix As String

        '<remarks/>
        Public Surname As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("NameSuffix")> _
        Public NameSuffix() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("NameTitle")> _
        Public NameTitle() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TravelArrangerTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TravelArrangerTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NameType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class TravelClubTypeClubMemberName
        Inherits PersonNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class OrganizationTypeOrgMemberName
        Inherits PersonNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Level As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Title As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class OtherSrvcPrefType

        '<remarks/>
        Public OtherSrvcName As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("VendorPref")> _
        Public VendorPref() As CompanyNamePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyPref")> _
        Public LoyaltyPref() As LoyaltyPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentFormPref")> _
        Public PaymentFormPref() As PaymentFormPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SpecRequestPref")> _
        Public SpecRequestPref() As SpecRequestPrefType

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TravelArrangerTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TravelArrangerTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelPurpose As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class LoyaltyPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PaymentFormPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class SpecRequestPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CommonPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("NamePref")> _
        Public NamePref() As NamePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PhonePref")> _
        Public PhonePref() As PhonePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AddressPref")> _
        Public AddressPref() As AddressPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentFormPref")> _
        Public PaymentFormPref() As PaymentFormPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("InterestPref")> _
        Public InterestPref() As InterestPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("InsurancePref")> _
        Public InsurancePref() As InsurancePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SeatingPref")> _
        Public SeatingPref() As SeatingPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TicketDistribPref")> _
        Public TicketDistribPref() As TicketDistribPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MediaEntertainPref")> _
        Public MediaEntertainPref() As MediaEntertainPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PetInfoPref")> _
        Public PetInfoPref() As PetInfoPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MealPref")> _
        Public MealPref() As MealPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyPref")> _
        Public LoyaltyPref() As LoyaltyPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SpecRequestPref")> _
        Public SpecRequestPref() As SpecRequestPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RelatedTravelerPref")> _
        Public RelatedTravelerPref() As RelatedTravelerPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ContactPref")> _
        Public ContactPref() As CommonPrefTypeContactPref

        '<remarks/>
        Public EmployeeLevelInfo As EmployeeInfoType

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TravelArrangerTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TravelArrangerTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SmokingAllowed As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SmokingAllowedSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")> _
        Public PrimaryLangID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")> _
        Public AltLangID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class NamePrefType

        '<remarks/>
        Public UniqueID As UniqueID_Type

        '<remarks/>
        Public PersonName As PersonNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PhonePrefType

        '<remarks/>
        Public Telephone As PhonePrefTypeTelephone
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PhonePrefTypeTelephone

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AddressPrefType

        '<remarks/>
        Public Address As AddressInfoType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TravelArrangerTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TravelArrangerTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(CompanyInfoTypeAddressInfo)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(CustomerTypeAddress))> _
    Public Class AddressInfoType
        Inherits AddressType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DefaultIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public UseType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(AddressInfoType)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(CompanyInfoTypeAddressInfo)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(CustomerTypeAddress))> _
    Public Class AddressType

        '<remarks/>
        Public StreetNmbr As AddressTypeStreetNmbr

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("BldgRoom")> _
        Public BldgRoom() As AddressTypeBldgRoom

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AddressLine")> _
        Public AddressLine() As String

        '<remarks/>
        Public CityName As String

        '<remarks/>
        Public PostalCode As String

        '<remarks/>
        Public County As String

        '<remarks/>
        Public StateProv As StateProvType

        '<remarks/>
        Public CountryName As CountryNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FormattedInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FormattedIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TravelArrangerTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TravelArrangerTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Remark As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CountryNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class StateProvType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StateCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AddressTypeStreetNmbr
        Inherits StreetNmbrType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StreetNmbrSuffix As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StreetDirection As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RuralRouteNmbr As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(AddressTypeStreetNmbr))> _
    Public Class StreetNmbrType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PO_Box As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AddressTypeBldgRoom

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BldgNameIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public BldgNameIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CompanyInfoTypeAddressInfo
        Inherits AddressInfoType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CustomerTypeAddress
        Inherits AddressInfoType

        '<remarks/>
        Public CompanyName As CompanyNameType

        '<remarks/>
        Public AddresseeName As PersonNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public EffectiveDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public EffectiveDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public ExpireDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDateExclusiveIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateExclusiveIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ValidationStatus As CustomerTypeAddressValidationStatus

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ValidationStatusSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ParentCompanyRef As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum CustomerTypeAddressValidationStatus

        '<remarks/>
        SystemValidated

        '<remarks/>
        UserValidated

        '<remarks/>
        NotChecked
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum TransferActionType

        '<remarks/>
        Automatic

        '<remarks/>
        Mandatory

        '<remarks/>
        Selectable

        '<remarks/>
        Never
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class EmployeeInfoType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EmployeeId As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EmployeeLevel As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EmployeeTitle As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EmployeeStatus As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class InterestPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class SeatingPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SeatDirection As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SeatLocation As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SeatPosition As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SeatRow As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class InsurancePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class TicketDistribPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DistribType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="duration")> _
        Public TicketTime As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Remark As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class MediaEntertainPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PetInfoPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class MealPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MealType As MealType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public MealTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FavoriteFood As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Beverage As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum MealType

        '<remarks/>
        AVML

        '<remarks/>
        BBML

        '<remarks/>
        BLML

        '<remarks/>
        CHML

        '<remarks/>
        DBML

        '<remarks/>
        FPML

        '<remarks/>
        GFML

        '<remarks/>
        HFML

        '<remarks/>
        HNML

        '<remarks/>
        KSML

        '<remarks/>
        LCML

        '<remarks/>
        LFML

        '<remarks/>
        LPML

        '<remarks/>
        LSML

        '<remarks/>
        MOML

        '<remarks/>
        NLML

        '<remarks/>
        ORML

        '<remarks/>
        PRML

        '<remarks/>
        RVML

        '<remarks/>
        SFML

        '<remarks/>
        SPML

        '<remarks/>
        VGML

        '<remarks/>
        VLML

        '<remarks/>
        RGML
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CommonPrefTypeContactPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ContactMethodCode As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class RelatedTravelerPrefType

        '<remarks/>
        Public UniqueID As UniqueID_Type

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CustomerTypePhysChallName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhysChallInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PhysChallIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CustomerTypeCitizenCountryName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CustomerTypeTelephone

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public EffectiveDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public EffectiveDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public ExpireDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDateExclusiveIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateExclusiveIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AreaCityCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhoneNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ParentCompanyRef As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CustomerTypeEmail
        Inherits EmailType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ParentCompanyRef As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(CompanyInfoTypeEmail)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(CustomerTypeEmail))> _
    Public Class EmailType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DefaultIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute("EmailType")> _
        Public EmailType1 As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Remark As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CompanyInfoTypeEmail
        Inherits EmailType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum CommissionInfoTypeShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum CommissionInfoTypeShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CustomerTypeURL
        Inherits URL_Type

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(CustomerTypeURL))> _
    Public Class URL_Type

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DefaultIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute(DataType:="anyURI")> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CustomerTypePaymentForm
        Inherits PaymentFormType

        '<remarks/>
        Public AssociatedSupplier As CustomerTypePaymentFormAssociatedSupplier

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DefaultIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ParentCompanyRef As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(CompanyInfoTypePaymentForm)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(CustomerTypePaymentForm))> _
    Public Class PaymentFormType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Ticket", GetType(PaymentFormTypeTicket)), _
         System.Xml.Serialization.XmlElementAttribute("Cash", GetType(PaymentFormTypeCash)), _
         System.Xml.Serialization.XmlElementAttribute("PaymentCard", GetType(PaymentCardType)), _
         System.Xml.Serialization.XmlElementAttribute("LoyaltyRedemption", GetType(PaymentFormTypeLoyaltyRedemption)), _
         System.Xml.Serialization.XmlElementAttribute("MiscChargeOrder", GetType(PaymentFormTypeMiscChargeOrder)), _
         System.Xml.Serialization.XmlElementAttribute("DirectBill", GetType(DirectBillType)), _
         System.Xml.Serialization.XmlElementAttribute("BankAcct", GetType(BankAcctType)), _
         System.Xml.Serialization.XmlElementAttribute("Voucher", GetType(PaymentFormTypeVoucher))> _
        Public Item As Object

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CostCenterID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PaymentTransactionTypeCode As PaymentFormTypePaymentTransactionTypeCode

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PaymentTransactionTypeCodeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public GuaranteeIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public GuaranteeIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public GuaranteeTypeCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public GuaranteeID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Remark As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum PaymentFormTypePaymentTransactionTypeCode

        '<remarks/>
        charge

        '<remarks/>
        reserve

        '<remarks/>
        refund
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CompanyInfoTypePaymentForm
        Inherits PaymentFormType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PaymentFormTypeTicket

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ConjunctionTicketNbr")> _
        Public ConjunctionTicketNbr() As PaymentFormTypeTicketConjunctionTicketNbr

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TicketNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public OriginalTicketNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public OriginalIssuePlace As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public OriginalIssueDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public OriginalIssueDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public OriginalIssueIATA As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public OriginalPaymentForm As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CheckInhibitorType As PaymentFormTypeMiscChargeOrderCheckInhibitorType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public CheckInhibitorTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CouponRPHs() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ReroutingType As PaymentFormTypeTicketReroutingType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ReroutingTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ReasonForReroute As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum PaymentFormTypeTicketReroutingType

        '<remarks/>
        voluntary

        '<remarks/>
        involuntary
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum PaymentFormTypeMiscChargeOrderCheckInhibitorType

        '<remarks/>
        CheckDigit

        '<remarks/>
        InterlineAgreement

        '<remarks/>
        Both
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PaymentFormTypeTicketConjunctionTicketNbr

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Coupons() As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PaymentFormTypeCash

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CashIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public CashIndicatorSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PaymentCardType

        '<remarks/>
        Public CardHolderName As String

        '<remarks/>
        Public CardIssuerName As PaymentCardTypeCardIssuerName

        '<remarks/>
        Public Address As AddressType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Telephone")> _
        Public Telephone() As PaymentCardTypeTelephone

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Email")> _
        Public Email() As EmailType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CustLoyalty")> _
        Public CustLoyalty() As PaymentCardTypeCustLoyalty

        '<remarks/>
        Public SignatureOnFile As PaymentCardTypeSignatureOnFile

        '<remarks/>
        Public MagneticStripe As PaymentCardTypeMagneticStripe

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CardType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CardCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CardNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SeriesCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EffectiveDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MaskedCardNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CardHolderRPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExtendPaymentIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExtendPaymentIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CountryOfIssue As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")> _
        Public ExtendedPaymentQuantity As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SignatureOnFileIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SignatureOnFileIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CompanyCardReference As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Remark As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EncryptionKey As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PaymentCardTypeCustLoyalty

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ProgramID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MembershipID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelSector As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VendorCode() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PrimaryLoyaltyIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PrimaryLoyaltyIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AllianceLoyaltyLevelName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CustomerType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CustomerValue As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PaymentCardTypeTelephone

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PaymentCardTypeSignatureOnFile

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SignatureOnFileIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SignatureOnFileIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public EffectiveDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public EffectiveDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public ExpireDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDateExclusiveIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateExclusiveIndicatorSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PaymentCardTypeMagneticStripe

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="base64Binary")> _
        Public Track1() As Byte

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="base64Binary")> _
        Public Track2() As Byte

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="base64Binary")> _
        Public Track3() As Byte
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class RelatedTravelerType

        '<remarks/>
        Public UniqueID As UniqueID_Type

        '<remarks/>
        Public PersonName As PersonNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Relation As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public BirthDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public BirthDateSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class ContactPersonType

        '<remarks/>
        Public PersonName As PersonNameType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Telephone")> _
        Public Telephone() As ContactPersonTypeTelephone

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Address")> _
        Public Address() As AddressInfoType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Email")> _
        Public Email() As EmailType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("URL")> _
        Public URL() As URL_Type

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CompanyName")> _
        Public CompanyName() As CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("EmployeeInfo")> _
        Public EmployeeInfo() As EmployeeInfoType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DefaultIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ContactType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Relation As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EmergencyFlag As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public EmergencyFlagSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CommunicationMethodCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocumentDistribMethodCode As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class DocumentType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("DocHolderName", GetType(System.String)), _
         System.Xml.Serialization.XmlElementAttribute("DocHolderFormattedName", GetType(PersonNameType))> _
        Public Item As Object

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("DocLimitations")> _
        Public DocLimitations() As String

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("AdditionalPersonName", IsNullable:=False)> _
        Public AdditionalPersonNames() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocIssueAuthority As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocIssueLocation As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Gender As DocumentTypeGender

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public GenderSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public BirthDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public BirthDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public EffectiveDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public EffectiveDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public ExpireDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDateExclusiveIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateExclusiveIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocIssueStateProv As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocIssueCountry As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BirthCountry As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BirthPlace As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocHolderNationality As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ContactName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public HolderType As DocumentTypeHolderType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public HolderTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Remark As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PostalCode As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CustomerTypeCustLoyalty

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ProgramID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MembershipID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelSector As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VendorCode() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PrimaryLoyaltyIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PrimaryLoyaltyIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AllianceLoyaltyLevelName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CustomerType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CustomerValue As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Remark As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CustomerTypeAdditionalLanguage

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")> _
        Public Code As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum DocumentTypeGender

        '<remarks/>
        Male

        '<remarks/>
        Female

        '<remarks/>
        Unknown
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CompanyInfoTypeTelephoneInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhoneNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AreaCityCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class LoyaltyProgramType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ProgramCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SingleVendorInd As CertificationTypeSingleVendorInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SingleVendorIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LoyaltyLevel As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PrimaryLoyaltyIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PrimaryLoyaltyIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class OrganizationType

        '<remarks/>
        Public OrgMemberName As OrganizationTypeOrgMemberName

        '<remarks/>
        Public OrgName As CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RelatedOrgName")> _
        Public RelatedOrgName() As CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TravelArranger")> _
        Public TravelArranger() As TravelArrangerType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DefaultIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public EffectiveDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public EffectiveDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public ExpireDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDateExclusiveIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateExclusiveIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public OfficeType As OfficeLocationType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public OfficeTypeSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CompanyInfoTypeTripPurpose

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Description As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class EmployerType

        '<remarks/>
        Public CompanyName As CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RelatedEmployer")> _
        Public RelatedEmployer() As CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("EmployeeInfo")> _
        Public EmployeeInfo() As EmployeeInfoType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("InternalRefNmbr")> _
        Public InternalRefNmbr() As FreeTextType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TravelArranger")> _
        Public TravelArranger() As TravelArrangerType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyProgram")> _
        Public LoyaltyProgram() As LoyaltyProgramType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DefaultIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public OfficeType As OfficeLocationType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public OfficeTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public EffectiveDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public EffectiveDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public ExpireDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDateExclusiveIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateExclusiveIndicatorSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class InsuranceType

        '<remarks/>
        Public InsuredName As PersonNameType

        '<remarks/>
        Public InsuranceCompany As CompanyNameType

        '<remarks/>
        Public Underwriter As CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute("InsuranceType")> _
        Public InsuranceType1 As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PolicyNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public EffectiveDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public EffectiveDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public ExpireDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDateExclusiveIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateExclusiveIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class TravelClubType

        '<remarks/>
        Public TravelClubName As CompanyNameType

        '<remarks/>
        Public ClubMemberName As TravelClubTypeClubMemberName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public EffectiveDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public EffectiveDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public ExpireDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDateExclusiveIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateExclusiveIndicatorSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AllianceConsortiumType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AllianceMember")> _
        Public AllianceMember() As AllianceConsortiumTypeAllianceMember

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public EffectiveDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public EffectiveDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public ExpireDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDateExclusiveIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateExclusiveIndicatorSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AgreementsTypeProfileSecurity

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AccessingOrganizationType As AgreementsTypeProfileSecurityAccessingOrganizationType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AccessingOrganizationTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AccessingOrganizationID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AccessType As AgreementsTypeProfileSecurityAccessType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AccessTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(ProfileTypeComment))> _
    Public Class ParagraphType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Text")> _
        Public [Text]() As FormattedTextTextType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Image")> _
        Public Image() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("URL", DataType:="anyURI")> _
        Public URL() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ListItem")> _
        Public ListItem() As ParagraphTypeListItem

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Name As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")> _
        Public ParagraphNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CreateDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public CreateDateTimeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CreatorID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LastModifyDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public LastModifyDateTimeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LastModifierID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public PurgeDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PurgeDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")> _
        Public Language As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class ProfileTypeCommentAuthorizedViewer

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ViewerCode As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Provider

        '<remarks/>
        Public Name As String

        '<remarks/>
        Public System As String

        '<remarks/>
        Public Userid As String

        '<remarks/>
        Public Password As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleProfileRentalPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyPref")> _
        Public LoyaltyPref() As VehicleProfileRentalPrefTypeLoyaltyPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("VendorPref")> _
        Public VendorPref() As CompanyNamePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentFormPref")> _
        Public PaymentFormPref() As VehicleProfileRentalPrefTypePaymentFormPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CoveragePref")> _
        Public CoveragePref() As VehicleProfileRentalPrefTypeCoveragePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SpecialReqPref")> _
        Public SpecialReqPref() As VehicleSpecialReqPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("VehTypePref")> _
        Public VehTypePref() As VehiclePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SpecialEquipPref")> _
        Public SpecialEquipPref() As VehicleProfileRentalPrefTypeSpecialEquipPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SmokingAllowed As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SmokingAllowedSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public GasPrePay As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public GasPrePaySpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirlinePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyPref")> _
        Public LoyaltyPref() As LoyaltyPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("VendorPref")> _
        Public VendorPref() As AirlinePrefTypeVendorPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentFormPref")> _
        Public PaymentFormPref() As PaymentFormPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AirportOriginPref")> _
        Public AirportOriginPref() As AirportPrefType

        '<remarks/>
        Public AirportDestinationPref As AirportPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AirportRoutePref")> _
        Public AirportRoutePref() As AirportPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FareRestrictPref")> _
        Public FareRestrictPref() As AirlinePrefTypeFareRestrictPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FarePref")> _
        Public FarePref() As AirlinePrefTypeFarePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TourCodePref")> _
        Public TourCodePref() As AirlinePrefTypeTourCodePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FlightTypePref")> _
        Public FlightTypePref() As AirlinePrefTypeFlightTypePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("EquipPref")> _
        Public EquipPref() As EquipmentTypePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CabinPref")> _
        Public CabinPref() As AirlinePrefTypeCabinPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SeatPref")> _
        Public SeatPref() As AirlinePrefTypeSeatPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TicketDistribPref")> _
        Public TicketDistribPref() As TicketDistribPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MealPref")> _
        Public MealPref() As MealPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SpecRequestPref")> _
        Public SpecRequestPref() As SpecRequestPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SSR_Pref")> _
        Public SSR_Pref() As AirlinePrefTypeSSR_Pref

        '<remarks/>
        Public TPA_Extensions As TPA_ExtensionsType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MediaEntertainPref")> _
        Public MediaEntertainPref() As MediaEntertainPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PetInfoPref")> _
        Public PetInfoPref() As PetInfoPrefType

        '<remarks/>
        Public AccountInformation As AirlinePrefTypeAccountInformation

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("OSI_Pref")> _
        Public OSI_Pref() As OtherServiceInfoType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("KeywordPref")> _
        Public KeywordPref() As AirlinePrefTypeKeywordPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SmokingAllowed As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SmokingAllowedSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PassengerTypeCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirTicketType As TicketType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AirTicketTypeSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class HotelPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyPref")> _
        Public LoyaltyPref() As LoyaltyPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentFormPref")> _
        Public PaymentFormPref() As PaymentFormPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("HotelChainPref")> _
        Public HotelChainPref() As CompanyNamePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PropertyNamePref")> _
        Public PropertyNamePref() As PropertyNamePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PropertyLocationPref")> _
        Public PropertyLocationPref() As PropertyLocationPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PropertyTypePref")> _
        Public PropertyTypePref() As PropertyTypePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PropertyClassPref")> _
        Public PropertyClassPref() As PropertyClassPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PropertyAmenityPref")> _
        Public PropertyAmenityPref() As PropertyAmenityPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RoomAmenityPref")> _
        Public RoomAmenityPref() As RoomAmenityPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RoomLocationPref")> _
        Public RoomLocationPref() As RoomLocationPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("BedTypePref")> _
        Public BedTypePref() As BedTypePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FoodSrvcPref")> _
        Public FoodSrvcPref() As FoodSrvcPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MediaEntertainPref")> _
        Public MediaEntertainPref() As MediaEntertainPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PetInfoPref")> _
        Public PetInfoPref() As PetInfoPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MealPref")> _
        Public MealPref() As MealPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RecreationSrvcPref")> _
        Public RecreationSrvcPref() As RecreationSrvcPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("BusinessSrvcPref")> _
        Public BusinessSrvcPref() As BusinessSrvcPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PersonalSrvcPref")> _
        Public PersonalSrvcPref() As PersonalSrvcPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SecurityFeaturePref")> _
        Public SecurityFeaturePref() As SecurityFeaturePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PhysChallFeaturePref")> _
        Public PhysChallFeaturePref() As PhysChallFeaturePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SpecRequestPref")> _
        Public SpecRequestPref() As SpecRequestPrefType

        '<remarks/>
        Public TPA_Extensions As TPA_ExtensionsType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SmokingAllowed As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SmokingAllowedSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RatePlanCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public HotelGuestType As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CustomerTypePaymentFormAssociatedSupplier

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CompanyShortName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelSector As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CodeContext As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PaymentFormTypeLoyaltyRedemption

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyCertificate")> _
        Public LoyaltyCertificate() As PaymentFormTypeLoyaltyRedemptionLoyaltyCertificate

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CertificateNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MemberNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ProgramName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PromotionCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PromotionVendorCode() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="positiveInteger")> _
        Public RedemptionQuantity As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PaymentFormTypeMiscChargeOrder

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TicketNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public OriginalTicketNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public OriginalIssuePlace As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public OriginalIssueDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public OriginalIssueDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public OriginalIssueIATA As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public OriginalPaymentForm As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CheckInhibitorType As PaymentFormTypeMiscChargeOrderCheckInhibitorType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public CheckInhibitorTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CouponRPHs() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PaperMCO_ExistInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PaperMCO_ExistIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class DirectBillType

        '<remarks/>
        Public CompanyName As DirectBillTypeCompanyName

        '<remarks/>
        Public Address As AddressInfoType

        '<remarks/>
        Public Email As EmailType

        '<remarks/>
        Public Telephone As DirectBillTypeTelephone

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DirectBill_ID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BillingNumber As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class BankAcctType

        '<remarks/>
        Public BankAcctName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BankID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AcctType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BankAcctNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ChecksAcceptedInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ChecksAcceptedIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PaymentFormTypeVoucher

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SeriesCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BillingNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SupplierIdentifier As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Identifier As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ValueType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ElectronicIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ElectronicIndicatorSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PaymentCardTypeCardIssuerName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BankID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class ContactPersonTypeTelephone

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum DocumentTypeHolderType

        '<remarks/>
        Infant

        '<remarks/>
        HeadOfHousehold
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum OfficeLocationType

        '<remarks/>
        Main

        '<remarks/>
        Field

        '<remarks/>
        Division

        '<remarks/>
        Regional

        '<remarks/>
        Remote
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum AgreementsTypeProfileSecurityAccessingOrganizationType

        '<remarks/>
        ProfileOwner

        '<remarks/>
        IATA

        '<remarks/>
        Other
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum AgreementsTypeProfileSecurityAccessType

        '<remarks/>
        [ReadOnly]

        '<remarks/>
        ReadWrite

        '<remarks/>
        NoAccess
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(ParagraphTypeListItem))> _
    Public Class FormattedTextTextType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Formatted As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FormattedSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TextFormat As FormattedTextTextTypeTextFormat

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TextFormatSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class ParagraphTypeListItem
        Inherits FormattedTextTextType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")> _
        Public ListItem As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehiclePrefType
        Inherits VehicleCoreType

        '<remarks/>
        Public VehMakeModel As VehiclePrefTypeVehMakeModel

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TypePref As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TypePrefSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ClassPref As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ClassPrefSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirConditionPref As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AirConditionPrefSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransmissionPref As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransmissionPrefSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VendorCarType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")> _
        Public VehicleQty As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CodeContext As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleSpecialReqPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleProfileRentalPrefTypeCoveragePref
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleProfileRentalPrefTypePaymentFormPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleProfileRentalPrefTypeLoyaltyPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleProfileRentalPrefTypeSpecialEquipPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Action As ActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ActionSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(AirportPrefType))> _
    Public Class LocationType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LocationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CodeContext As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirportPrefType
        Inherits LocationType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirlinePrefTypeTourCodePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TourCodeInfo", GetType(AirlinePrefTypeTourCodePrefTourCodeInfo)), _
         System.Xml.Serialization.XmlElementAttribute("StaffTourCodeInfo", GetType(AirlinePrefTypeTourCodePrefStaffTourCodeInfo))> _
        Public Item As Object

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PassengerTypeCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirlineVendorPrefRPH() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirlinePrefTypeFarePref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Description As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirlineVendorPrefRPH() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RateCategoryCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirlinePrefTypeFareRestrictPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FareRestriction As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public [Date] As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(EquipmentTypePref))> _
    Public Class EquipmentType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirEquipType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ChangeofGauge As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ChangeofGaugeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class EquipmentTypePref
        Inherits EquipmentType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public WideBody As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public WideBodySpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirlinePrefTypeFlightTypePref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FlightType As FlightTypeType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FlightTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")> _
        Public MaxConnections As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NonScheduledFltInfo As AirlinePrefTypeFlightTypePrefNonScheduledFltInfo

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public NonScheduledFltInfoSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BackhaulIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public BackhaulIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public GroundTransportIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public GroundTransportIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DirectAndNonStopOnlyInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DirectAndNonStopOnlyIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NonStopsOnlyInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public NonStopsOnlyIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public OnlineConnectionsOnlyInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public OnlineConnectionsOnlyIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RoutingType As AirlinePrefTypeFlightTypePrefRoutingType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public RoutingTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExcludeTrainInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExcludeTrainIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum FlightTypeType

        '<remarks/>
        Nonstop

        '<remarks/>
        Direct

        '<remarks/>
        Connection

        '<remarks/>
        SingleConnection

        '<remarks/>
        DoubleConnection

        '<remarks/>
        OneStopOnly
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirlinePrefTypeCabinPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Cabin As CabinType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public CabinSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum CabinType

        '<remarks/>
        First

        '<remarks/>
        Business

        '<remarks/>
        Economy
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirlinePrefTypeAccountInformation

        '<remarks/>
        Public TaxRegistrationDetails As AirlinePrefTypeAccountInformationTaxRegistrationDetails

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Number As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CostCenter As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CompanyNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ClientReference As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirlinePrefTypeSSR_Pref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SSR_Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VendorCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="positiveInteger")> _
        Public NumberInParty As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultStatusCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Remark As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LookupKey As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirlineVendorPrefRPH() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferActionType As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionTypeSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirlinePrefTypeSeatPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FlightDistanceQualifier As AirlinePrefTypeSeatPrefFlightDistanceQualifier

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FlightDistanceQualifierSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public InternationalIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public InternationalIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirlineVendorPrefRPH() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PassengerTypeCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum AirlinePrefTypeSeatPrefFlightDistanceQualifier

        '<remarks/>
        LongHaul

        '<remarks/>
        ShortHaul
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class OtherServiceInfoType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TravelerRefNumber")> _
        Public TravelerRefNumber() As OtherServiceInfoTypeTravelerRefNumber

        '<remarks/>
        Public Airline As CompanyNameType

        '<remarks/>
        Public [Text] As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirlinePrefTypeKeywordPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VendorCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Description As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Keyword As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StatusCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="positiveInteger")> _
        Public NumberInParty As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirlineVendorRPH() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransferActionSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum TicketType

        '<remarks/>
        eTicket

        '<remarks/>
        Paper

        '<remarks/>
        MCO
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PropertyAmenityPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PropertyAmenityType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PropertyClassPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PropertyClassType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PropertyTypePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PropertyType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PropertyLocationPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PropertyLocationType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PropertyNamePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public HotelCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class RecreationSrvcPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RecreationSrvcType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class FoodSrvcPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FoodSrvcType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class BedTypePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BedType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class RoomLocationPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RoomLocationType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class RoomAmenityPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RoomAmenity As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExistsCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")> _
        Public Quantity As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public QualityLevel As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PhysChallFeaturePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhysChallFeatureType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class SecurityFeaturePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PersonalSrvcPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class BusinessSrvcPrefType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PreferLevel As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PreferLevelSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BusinessSrvcType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PaymentFormTypeLoyaltyRedemptionLoyaltyCertificate

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID_Context As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")> _
        Public NmbrOfNights As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Format As PaymentFormTypeLoyaltyRedemptionLoyaltyCertificateFormat

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FormatSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Status As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum PaymentFormTypeLoyaltyRedemptionLoyaltyCertificateFormat

        '<remarks/>
        Paper

        '<remarks/>
        Electronic
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class DirectBillTypeTelephone

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum FormattedTextTextTypeTextFormat

        '<remarks/>
        PlainText

        '<remarks/>
        HTML
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(VehiclePrefType))> _
    Public Class VehicleCoreType

        '<remarks/>
        Public VehType As VehicleCoreTypeVehType

        '<remarks/>
        Public VehClass As VehicleCoreTypeVehClass

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirConditionInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AirConditionIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransmissionType As VehicleTransmissionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransmissionTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FuelType As VehicleCoreTypeFuelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FuelTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DriveType As VehicleCoreTypeDriveType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DriveTypeSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum VehicleTransmissionType

        '<remarks/>
        Automatic

        '<remarks/>
        Manual
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum VehicleCoreTypeFuelType

        '<remarks/>
        Unspecified

        '<remarks/>
        Diesel

        '<remarks/>
        Hybrid

        '<remarks/>
        Electric

        '<remarks/>
        LPG_CompressedGas

        '<remarks/>
        Hydrogen

        '<remarks/>
        MultiFuel

        '<remarks/>
        Petrol

        '<remarks/>
        Ethanol
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum VehicleCoreTypeDriveType

        '<remarks/>
        AWD

        '<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("4WD")> _
        Item4WD

        '<remarks/>
        Unspecified
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehiclePrefTypeVehMakeModel

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="gYear")> _
        Public ModelYear As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum ActionType

        '<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("Add-Update")> _
        AddUpdate

        '<remarks/>
        Cancel

        '<remarks/>
        Delete

        '<remarks/>
        Add

        '<remarks/>
        Replace
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirlinePrefTypeTourCodePrefTourCodeInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TourTypeCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")> _
        Public YearNum As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PromotionCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PromotionVendorCode() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PartyID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class OtherServiceInfoTypeTravelerRefNumber

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SurnameRefNumber As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirlinePrefTypeAccountInformationTaxRegistrationDetails

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TaxID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RecipientName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RecipientAddress As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum AirlinePrefTypeFlightTypePrefNonScheduledFltInfo

        '<remarks/>
        ChartersOnly

        '<remarks/>
        ExcludeCharters

        '<remarks/>
        All
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum AirlinePrefTypeFlightTypePrefRoutingType

        '<remarks/>
        Normal

        '<remarks/>
        Mirror
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AirlinePrefTypeTourCodePrefStaffTourCodeInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StaffType As AirlinePrefTypeTourCodePrefStaffTourCodeInfoStaffType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public StaffTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EmployeeID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VendorCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Description As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleCoreTypeVehClass

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Size As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleCoreTypeVehType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VehicleCategory As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DoorCount As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum AirlinePrefTypeTourCodePrefStaffTourCodeInfoStaffType

        '<remarks/>
        Current

        '<remarks/>
        Duty

        '<remarks/>
        CabinCrew

        '<remarks/>
        Retired

        '<remarks/>
        TechCrew

        '<remarks/>
        UnaccompaniedFamilyMember

        '<remarks/>
        OtherAirlinePersonnel
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute()> _
    Public Class OtherServiceInformationType

        '<remarks/>
        Public Airline As Airline

        '<remarks/>
        Public [Text] As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Airline

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CompanyShortName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelSector As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CodeContext As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Remarks

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Remark")> _
        Public Remark() As Remark

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RemarkType As RemarksRemarkType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public RemarkTypeSpecified As Boolean
    End Class

    '<remarks/>
    Public Enum RemarksRemarkType

        '<remarks/>
        General

        '<remarks/>
        TravelPolicy

    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute()> _
    Public Class Remark

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransferAction As TransferActionType

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class
End Namespace
