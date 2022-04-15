Option Strict Off
Option Explicit On 

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmHotelAvail

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TPA_Extensions

        '<remarks/>
        Public Provider As Provider

        '<remarks/>
        Public MultiRate As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Provider

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Name")> _
        Public Name() As Name

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("System")> _
        Public GDSSystem As String

        '<remarks/>
        Public Userid As String

        '<remarks/>
        Public Password As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Name

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PseudoCityCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Access

        '<remarks/>
        Public AccessPerson As AccessPerson

        '<remarks/>
        Public AccessComment As AccessComment

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ActionType As AccessActionType

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
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AccessPerson

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
        Public ShareSynchInd As AccessPersonShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As AccessPersonShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NameType As String
    End Class

    '<remarks/>
    Public Enum AccessPersonShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum AccessPersonShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AccessComment

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum AccessActionType

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
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Accesses

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Access")> _
        Public Access() As Access

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As AccessesShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As AccessesShareMarketInd

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
    Public Enum AccessesShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum AccessesShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Affiliations

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Organization")> _
        Public Organization() As Organization

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Employer")> _
        Public Employer() As Employer

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TravelArranger")> _
        Public TravelArranger() As TravelArranger

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TravelClub")> _
        Public TravelClub() As TravelClub

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Insurance")> _
        Public Insurance() As Insurance

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As AffiliationsShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As AffiliationsShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Agreements

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Certification")> _
        Public Certification() As Certification

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AllianceConsortium")> _
        Public AllianceConsortium() As AllianceConsortium

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CommissionInfo")> _
        Public CommissionInfo() As CommissionInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As AgreementsShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As AgreementsShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AirlinePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyPref")> _
        Public LoyaltyPref() As LoyaltyPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("VendorPref")> _
        Public VendorPref() As VendorPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentFormPref")> _
        Public PaymentFormPref() As PaymentFormPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AirportOriginPref")> _
        Public AirportOriginPref() As AirportOriginPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AirportRoutePref")> _
        Public AirportRoutePref() As AirportRoutePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FareRestrictPref")> _
        Public FareRestrictPref() As FareRestrictPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FlightTypePref")> _
        Public FlightTypePref() As FlightTypePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("EquipPref")> _
        Public EquipPref() As EquipPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CabinPref")> _
        Public CabinPref() As CabinPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SeatPref")> _
        Public SeatPref() As SeatPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TicketDistribPref")> _
        Public TicketDistribPref() As TicketDistribPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MealPref")> _
        Public MealPref() As MealPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SpecRequestPref")> _
        Public SpecRequestPref() As SpecRequestPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SSR_Pref")> _
        Public SSR_Pref() As SSR_Pref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MediaEntertainPref")> _
        Public MediaEntertainPref() As MediaEntertainPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PetInfoPref")> _
        Public PetInfoPref() As PetInfoPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As AirlinePrefShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As AirlinePrefShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(AirlinePrefPreferLevel.Preferred)> _
        Public PreferLevel As AirlinePrefPreferLevel = AirlinePrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public SmokingAllowed As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PassengerTypeCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirTicketType As AirlinePrefAirTicketType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AirTicketTypeSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CommonPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("NamePref")> _
        Public NamePref() As NamePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PhonePref")> _
        Public PhonePref() As PhonePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AddressPref")> _
        Public AddressPref() As AddressPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentFormPref")> _
        Public PaymentFormPref() As PaymentFormPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("InterestPref")> _
        Public InterestPref() As InterestPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("InsurancePref")> _
        Public InsurancePref() As InsurancePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SeatingPref")> _
        Public SeatingPref() As SeatingPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TicketDistribPref")> _
        Public TicketDistribPref() As TicketDistribPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MediaEntertainPref")> _
        Public MediaEntertainPref() As MediaEntertainPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PetInfoPref")> _
        Public PetInfoPref() As PetInfoPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MealPref")> _
        Public MealPref() As MealPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyPref")> _
        Public LoyaltyPref() As LoyaltyPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SpecRequestPref")> _
        Public SpecRequestPref() As SpecRequestPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RelatedTravelerPref")> _
        Public RelatedTravelerPref() As RelatedTravelerPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommonPrefShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommonPrefShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public SmokingAllowed As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PrimaryLangID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AltLangID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Customer

        '<remarks/>
        Public PersonName As PersonName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Telephone")> _
        Public Telephone() As Telephone

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Email")> _
        Public Email() As Email

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Address")> _
        Public Address() As Address

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("URL")> _
        Public URL() As URL

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CitizenCountryName")> _
        Public CitizenCountryName() As CitizenCountryName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PhysChallName")> _
        Public PhysChallName() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PetInfo")> _
        Public PetInfo() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentForm")> _
        Public PaymentForm() As PaymentForm

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RelatedTraveler")> _
        Public RelatedTraveler() As RelatedTraveler

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ContactPerson")> _
        Public ContactPerson() As ContactPerson

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Document")> _
        Public Document() As Document

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CustLoyalty")> _
        Public CustLoyalty() As CustLoyalty

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("EmployeeInfo")> _
        Public EmployeeInfo() As EmployeeInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Gender As CustomerGender

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
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DecimalPlacesSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PersonName

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
        Public ShareSynchInd As PersonNameShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As PersonNameShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NameType As String
    End Class

    '<remarks/>
    Public Enum PersonNameShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum PersonNameShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Telephone

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TelephoneShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TelephoneShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhoneLocationType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhoneTechType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CountryAccessCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AreaCityCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhoneNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Extension As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PIN As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public FormattedInd As Boolean = False
    End Class

    '<remarks/>
    Public Enum TelephoneShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum TelephoneShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Email

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As EmailShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As EmailShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public DefaultInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EmailType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum EmailShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum EmailShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class URL

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As URLShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As URLShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public DefaultInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute(DataType:="anyURI")> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum URLShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum URLShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class UniqueID

        '<remarks/>
        Public CompanyName As CompanyName

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
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CompanyName

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
    Public Class CitizenCountryName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public DefaultInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PaymentForm

        '<remarks/>
        Public PaymentCard As PaymentCard

        '<remarks/>
        Public BankAcct As BankAcct

        '<remarks/>
        Public DirectBill As DirectBill

        '<remarks/>
        Public Voucher As Voucher

        '<remarks/>
        Public LoyaltyRedemption As LoyaltyRedemption

        '<remarks/>
        Public MiscChargeOrder As MiscChargeOrder

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As PaymentFormShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As PaymentFormShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CostCenterID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PaymentCard

        '<remarks/>
        Public CardHolderName As String

        '<remarks/>
        Public CardIssuerName As CardIssuerName

        '<remarks/>
        Public Address As Address

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As PaymentCardShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As PaymentCardShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CardType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CardCode As PaymentCardCardCode

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public CardCodeSpecified As Boolean

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
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CardIssuerName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BankID As String
    End Class

    '<remarks/>
    Public Enum PaymentCardShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum PaymentCardShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum PaymentCardCardCode

        '<remarks/> Carte Aurore
        AU

        '<remarks/> Amex
        AX

        '<remarks/>
        BC

        '<remarks/>
        BL

        CA
        '<remarks/> Carte Blanche
        CB

        '<remarks/> Cofinoga
        CG

        '<remarks/> Connect
        CN

        '<remarks/> Choice
        CX

        '<remarks/> Diners (DC)
        DN

        '<remarks/>
        DK

        DI

        '<remarks/> Discover
        DS

        '<remarks/>
        EC

        '<remarks/> Lufthansa GK Card
        GK

        '<remarks/> 
        IK

        '<remarks/> JCB
        JC

        '<remarks/> Mastercard (CA)
        MC

        '<remarks/> Mastercard Debit
        MD

        '<remarks/> Mastercard Maestro
        MO

        '<remarks/> Mastercard Prepaid
        MP

        '<remarks/> 
        MS

        '<remarks/> Solo
        SO

        '<remarks/> Switch
        SW

        '<remarks/> Torch Club
        TC

        '<remarks/> UATP
        TP

        '<remarks/> Visa
        VI

        '<remarks/> Visa Debit
        VD

        '<remarks/> Visa Electron
        VE

        '<remarks/> ??
        VS

        '<remarks/> Visa Delta
        VT

        '<remarks/> ??
        WB

        '<remarks/> Access
        XS
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class BankAcct

        '<remarks/>
        Public BankAcctName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As BankAcctShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As BankAcctShareMarketInd

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
    End Class

    '<remarks/>
    Public Enum BankAcctShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum BankAcctShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class DirectBill

        '<remarks/>
        Public CompanyName As wmHotelAvail.CompanyName

        '<remarks/>
        Public Address As Address

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As DirectBillShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As DirectBillShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DirectBill_ID As String
    End Class

    '<remarks/>
    Public Enum DirectBillShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum DirectBillShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Voucher

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
        Public SeriesCode As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class LoyaltyRedemption

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CertificateNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MemberNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PromotionCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RedemptionQuantity As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public RedemptionQuantitySpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class MiscChargeOrder

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TicketNumber As String
    End Class

    '<remarks/>
    Public Enum PaymentFormShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum PaymentFormShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class RelatedTraveler

        '<remarks/>
        Public UniqueID As wmHotelAvail.UniqueID

        '<remarks/>
        Public PersonName As PersonName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As RelatedTravelerShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As RelatedTravelerShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Relation As String
    End Class

    '<remarks/>
    Public Enum RelatedTravelerShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum RelatedTravelerShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ContactPerson

        '<remarks/>
        Public PersonName As PersonName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Telephone")> _
        Public Telephone() As Telephone

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Address")> _
        Public Address() As Address

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Email")> _
        Public Email() As Email

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("URL")> _
        Public URL() As URL

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CompanyName")> _
        Public CompanyName() As wmHotelAvail.CompanyName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("EmployeeInfo")> _
        Public EmployeeInfo() As EmployeeInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As ContactPersonShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As ContactPersonShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public DefaultInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ContactType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Relation As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public EmergencyFlag As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    Public Enum ContactPersonShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum ContactPersonShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Document

        '<remarks/>
        Public DocHolderName As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("DocLimitations")> _
        Public DocLimitations() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As DocumentShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As DocumentShareMarketInd

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
        Public Gender As DocumentGender

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
    End Class

    '<remarks/>
    Public Enum DocumentShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum DocumentShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum DocumentGender

        '<remarks/>
        Male

        '<remarks/>
        Female

        '<remarks/>
        Unknown
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CustLoyalty

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CustLoyaltyShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CustLoyaltyShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

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
        Public LoyalLevel As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SingleVendorInd As CustLoyaltySingleVendorInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SingleVendorIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public SignupDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SignupDateSpecified As Boolean

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
        Public RPH As String
    End Class

    '<remarks/>
    Public Enum CustLoyaltyShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum CustLoyaltyShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum CustLoyaltySingleVendorInd

        '<remarks/>
        SingleVndr

        '<remarks/>
        Alliance
    End Enum

    '<remarks/>
    Public Enum CustomerGender

        '<remarks/>
        Male

        '<remarks/>
        Female

        '<remarks/>
        Unknown
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class EmployeeInfo

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
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Address

        '<remarks/>
        Public StreetNmbr As StreetNmbr

        '<remarks/>
        Public BldgRoom As String

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
        Public StateProv As StateProv

        '<remarks/>
        Public CountryName As CountryName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public FormattedInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As AddressShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As AddressShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class StreetNmbr

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PO_Box As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class StateProv

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StateCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CountryName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum AddressShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum AddressShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AddressInfo

        '<remarks/>
        Public StreetNmbr As StreetNmbr

        '<remarks/>
        Public BldgRoom As String

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
        Public StateProv As StateProv

        '<remarks/>
        Public CountryName As CountryName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public FormattedInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As AddressInfoShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As AddressInfoShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public DefaultInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public UseType As String
    End Class

    '<remarks/>
    Public Enum AddressInfoShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum AddressInfoShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AddressPref

        '<remarks/>
        Public Address As Address

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As AddressPrefShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As AddressPrefShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean
    End Class

    '<remarks/>
    Public Enum AddressPrefShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum AddressPrefShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Organization

        '<remarks/>
        Public OrgMemberName As OrgMemberName

        '<remarks/>
        Public OrgName As OrgName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RelatedOrgName")> _
        Public RelatedOrgName() As RelatedOrgName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TravelArranger")> _
        Public TravelArranger() As TravelArranger

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public DefaultInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As OrganizationShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As OrganizationShareMarketInd

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
        Public OfficeType As OrganizationOfficeType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public OfficeTypeSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class OrgMemberName

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
        Public ShareSynchInd As OrgMemberNameShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As OrgMemberNameShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NameType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Level As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Title As String
    End Class

    '<remarks/>
    Public Enum OrgMemberNameShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum OrgMemberNameShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class OrgName

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
    Public Class RelatedOrgName

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
    Public Class TravelArranger

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
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public DefaultInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TravelArrangerShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TravelArrangerShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelArrangerType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum TravelArrangerShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum TravelArrangerShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum OrganizationShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum OrganizationShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum OrganizationOfficeType

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
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Employer

        '<remarks/>
        Public CompanyName As wmHotelAvail.CompanyName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RelatedEmployer")> _
        Public RelatedEmployer() As RelatedEmployer

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("EmployeeInfo")> _
        Public EmployeeInfo() As EmployeeInfo

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("InternalRefNmbr")> _
        Public InternalRefNmbr() As InternalRefNmbr

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TravelArranger")> _
        Public TravelArranger() As TravelArranger

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyProgram")> _
        Public LoyaltyProgram() As LoyaltyProgram

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public DefaultInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public OfficeType As EmployerOfficeType

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
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class RelatedEmployer

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
    Public Class InternalRefNmbr

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class LoyaltyProgram

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ProgramCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SingleVendorInd As LoyaltyProgramSingleVendorInd

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
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum LoyaltyProgramSingleVendorInd

        '<remarks/>
        SingleVndr

        '<remarks/>
        Alliance
    End Enum

    '<remarks/>
    Public Enum EmployerOfficeType

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
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TravelClub

        '<remarks/>
        Public TravelClubName As TravelClubName

        '<remarks/>
        Public ClubMemberName As ClubMemberName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TravelClubShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TravelClubShareMarketInd

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
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TravelClubName

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
    Public Class ClubMemberName

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
        Public ShareSynchInd As ClubMemberNameShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As ClubMemberNameShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NameType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String
    End Class

    '<remarks/>
    Public Enum ClubMemberNameShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum ClubMemberNameShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum TravelClubShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum TravelClubShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Insurance

        '<remarks/>
        Public InsuredName As InsuredName

        '<remarks/>
        Public InsuranceCompany As InsuranceCompany

        '<remarks/>
        Public Underwriter As Underwriter

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As InsuranceShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As InsuranceShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public InsuranceType As String

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
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class InsuredName

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
        Public ShareSynchInd As InsuredNameShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As InsuredNameShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NameType As String
    End Class

    '<remarks/>
    Public Enum InsuredNameShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum InsuredNameShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class InsuranceCompany

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
    Public Class Underwriter

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
    Public Enum InsuranceShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum InsuranceShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum AffiliationsShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum AffiliationsShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Certification

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SingleVendorInd As CertificationSingleVendorInd

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
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum CertificationSingleVendorInd

        '<remarks/>
        SingleVndr

        '<remarks/>
        Alliance
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AllianceConsortium

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AllianceMember")> _
        Public AllianceMember() As AllianceMember

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
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AllianceMember

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
        Public MemberCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CommissionInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CommissionInfoShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CommissionInfoShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CommissionPlanCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As Double

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AmountSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DecimalPlacesSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum CommissionInfoShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum CommissionInfoShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum AgreementsShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum AgreementsShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class LoyaltyPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(LoyaltyPrefPreferLevel.Preferred)> _
        Public PreferLevel As LoyaltyPrefPreferLevel = LoyaltyPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum LoyaltyPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class VendorPref

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
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(VendorPrefPreferLevel.Preferred)> _
        Public PreferLevel As VendorPrefPreferLevel = VendorPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum VendorPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PaymentFormPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(PaymentFormPrefPreferLevel.Preferred)> _
        Public PreferLevel As PaymentFormPrefPreferLevel = PaymentFormPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum PaymentFormPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AirportOriginPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LocationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute("IATA")> _
        Public CodeContext As String = "IATA"

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(AirportOriginPrefPreferLevel.Preferred)> _
        Public PreferLevel As AirportOriginPrefPreferLevel = AirportOriginPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum AirportOriginPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AirportRoutePref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LocationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute("IATA")> _
        Public CodeContext As String = "IATA"

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(AirportRoutePrefPreferLevel.Preferred)> _
        Public PreferLevel As AirportRoutePrefPreferLevel = AirportRoutePrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum AirportRoutePrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class FareRestrictPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(FareRestrictPrefPreferLevel.Preferred)> _
        Public PreferLevel As FareRestrictPrefPreferLevel = FareRestrictPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FareRestriction As String
    End Class

    '<remarks/>
    Public Enum FareRestrictPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class FlightTypePref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(FlightTypePrefPreferLevel.Preferred)> _
        Public PreferLevel As FlightTypePrefPreferLevel = FlightTypePrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FlightType As FlightTypePrefFlightType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FlightTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MaxConnections As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public MaxConnectionsSpecified As Boolean
    End Class

    '<remarks/>
    Public Enum FlightTypePrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum FlightTypePrefFlightType

        '<remarks/>
        Nonstop

        '<remarks/>
        Direct

        '<remarks/>
        Connection
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class EquipPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirEquipType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public ChangeofGauge As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(EquipPrefPreferLevel.Preferred)> _
        Public PreferLevel As EquipPrefPreferLevel = EquipPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum EquipPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CabinPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(CabinPrefPreferLevel.Preferred)> _
        Public PreferLevel As CabinPrefPreferLevel = CabinPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Cabin As CabinPrefCabin

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public CabinSpecified As Boolean
    End Class

    '<remarks/>
    Public Enum CabinPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum CabinPrefCabin

        '<remarks/>
        First

        '<remarks/>
        Business

        '<remarks/>
        Economy
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SeatPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(SeatPrefPreferLevel.Preferred)> _
        Public PreferLevel As SeatPrefPreferLevel = SeatPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SeatNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SeatPreference As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public SmokingAllowed As Boolean = False
    End Class

    '<remarks/>
    Public Enum SeatPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TicketDistribPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(TicketDistribPrefPreferLevel.Preferred)> _
        Public PreferLevel As TicketDistribPrefPreferLevel = TicketDistribPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DistribType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TicketTime As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum TicketDistribPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class MealPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(MealPrefPreferLevel.Preferred)> _
        Public PreferLevel As MealPrefPreferLevel = MealPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MealType As MealPrefMealType

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
    Public Enum MealPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum MealPrefMealType

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
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SpecRequestPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(SpecRequestPrefPreferLevel.Preferred)> _
        Public PreferLevel As SpecRequestPrefPreferLevel = SpecRequestPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum SpecRequestPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SSR_Pref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(SSR_PrefPreferLevel.Preferred)> _
        Public PreferLevel As SSR_PrefPreferLevel = SSR_PrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SSR_Code As String
    End Class

    '<remarks/>
    Public Enum SSR_PrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class MediaEntertainPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(MediaEntertainPrefPreferLevel.Preferred)> _
        Public PreferLevel As MediaEntertainPrefPreferLevel = MediaEntertainPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum MediaEntertainPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PetInfoPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(PetInfoPrefPreferLevel.Preferred)> _
        Public PreferLevel As PetInfoPrefPreferLevel = PetInfoPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum PetInfoPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum AirlinePrefShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum AirlinePrefShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum AirlinePrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum AirlinePrefAirTicketType

        '<remarks/>
        eTicket

        '<remarks/>
        Paper
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class DateWindowRange

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public WindowBefore As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public WindowAfter As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CrossDateAllowedIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public CrossDateAllowedIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute(DataType:="date")> _
        Public Value As Date
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class NamePref

        '<remarks/>
        Public UniqueID As wmHotelAvail.UniqueID

        '<remarks/>
        Public PersonName As PersonName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(NamePrefPreferLevel.Preferred)> _
        Public PreferLevel As NamePrefPreferLevel = NamePrefPreferLevel.Preferred
    End Class

    '<remarks/>
    Public Enum NamePrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PhonePref

        '<remarks/>
        Public Telephone As Telephone
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class InterestPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(InterestPrefPreferLevel.Preferred)> _
        Public PreferLevel As InterestPrefPreferLevel = InterestPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum InterestPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class InsurancePref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(InsurancePrefPreferLevel.Preferred)> _
        Public PreferLevel As InsurancePrefPreferLevel = InsurancePrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum InsurancePrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SeatingPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(SeatingPrefPreferLevel.Preferred)> _
        Public PreferLevel As SeatingPrefPreferLevel = SeatingPrefPreferLevel.Preferred

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
    Public Enum SeatingPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class RelatedTravelerPref

        '<remarks/>
        Public UniqueID As wmHotelAvail.UniqueID

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(RelatedTravelerPrefPreferLevel.Preferred)> _
        Public PreferLevel As RelatedTravelerPrefPreferLevel = RelatedTravelerPrefPreferLevel.Preferred
    End Class

    '<remarks/>
    Public Enum RelatedTravelerPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum CommonPrefShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum CommonPrefShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class VehicleRentalPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyPref")> _
        Public LoyaltyPref() As LoyaltyPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("VendorPref")> _
        Public VendorPref() As VendorPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentFormPref")> _
        Public PaymentFormPref() As PaymentFormPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CoveragePref")> _
        Public CoveragePref() As CoveragePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SpecialReqPref")> _
        Public SpecialReqPref() As SpecialReqPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("VehTypePref")> _
        Public VehTypePref() As VehTypePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SpecialEquipPref")> _
        Public SpecialEquipPref() As SpecialEquipPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(VehicleRentalPrefPreferLevel.Preferred)> _
        Public PreferLevel As VehicleRentalPrefPreferLevel = VehicleRentalPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As VehicleRentalPrefShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As VehicleRentalPrefShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public SmokingAllowed As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public GasPrePay As Boolean = False
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SpecialReqPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(SpecialReqPrefPreferLevel.Preferred)> _
        Public PreferLevel As SpecialReqPrefPreferLevel = SpecialReqPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum SpecialReqPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CoveragePref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CoverageType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(CoveragePrefPreferLevel.Preferred)> _
        Public PreferLevel As CoveragePrefPreferLevel = CoveragePrefPreferLevel.Preferred
    End Class

    '<remarks/>
    Public Enum CoveragePrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class VehTypePref

        '<remarks/>
        Public VehType As VehType

        '<remarks/>
        Public VehClass As VehClass

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirConditionInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AirConditionIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransmissionType As VehTypePrefTransmissionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransmissionTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TypePref As VehTypePrefTypePref

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TypePrefSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ClassPref As VehTypePrefClassPref

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ClassPrefSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirConditionPref As VehTypePrefAirConditionPref

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AirConditionPrefSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransmissionPref As VehTypePrefTransmissionPref

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransmissionPrefSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class VehType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VehicleCategory As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DoorCount As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DoorCountSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class VehClass

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Size As String
    End Class

    '<remarks/>
    Public Enum VehTypePrefTransmissionType

        '<remarks/>
        Automatic

        '<remarks/>
        Manual
    End Enum

    '<remarks/>
    Public Enum VehTypePrefTypePref

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum VehTypePrefClassPref

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum VehTypePrefAirConditionPref

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum VehTypePrefTransmissionPref

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SpecialEquipPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EquipType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Quantity As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public QuantitySpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(SpecialEquipPrefPreferLevel.Preferred)> _
        Public PreferLevel As SpecialEquipPrefPreferLevel = SpecialEquipPrefPreferLevel.Preferred
    End Class

    '<remarks/>
    Public Enum SpecialEquipPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum VehicleRentalPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum VehicleRentalPrefShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum VehicleRentalPrefShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class HotelChainPref

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
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(HotelChainPrefPreferLevel.Preferred)> _
        Public PreferLevel As HotelChainPrefPreferLevel = HotelChainPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum HotelChainPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PropertyNamePref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(PropertyNamePrefPreferLevel.Preferred)> _
        Public PreferLevel As PropertyNamePrefPreferLevel = PropertyNamePrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum PropertyNamePrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PropertyLocationPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(PropertyLocationPrefPreferLevel.Preferred)> _
        Public PreferLevel As PropertyLocationPrefPreferLevel = PropertyLocationPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PropertyLocationType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum PropertyLocationPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PropertyTypePref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(PropertyTypePrefPreferLevel.Preferred)> _
        Public PreferLevel As PropertyTypePrefPreferLevel = PropertyTypePrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PropertyType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum PropertyTypePrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PropertyClassPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(PropertyClassPrefPreferLevel.Preferred)> _
        Public PreferLevel As PropertyClassPrefPreferLevel = PropertyClassPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PropertyClassType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum PropertyClassPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PropertyAmenityPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(PropertyAmenityPrefPreferLevel.Preferred)> _
        Public PreferLevel As PropertyAmenityPrefPreferLevel = PropertyAmenityPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PropertyAmenityType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum PropertyAmenityPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class RoomAmenityPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(RoomAmenityPrefPreferLevel.Preferred)> _
        Public PreferLevel As RoomAmenityPrefPreferLevel = RoomAmenityPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RoomAmenity As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Quantity As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public QuantitySpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum RoomAmenityPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class RoomLocationPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(RoomLocationPrefPreferLevel.Preferred)> _
        Public PreferLevel As RoomLocationPrefPreferLevel = RoomLocationPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RoomLocationType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum RoomLocationPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class BedTypePref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(BedTypePrefPreferLevel.Preferred)> _
        Public PreferLevel As BedTypePrefPreferLevel = BedTypePrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BedType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum BedTypePrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class FoodSrvcPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(FoodSrvcPrefPreferLevel.Preferred)> _
        Public PreferLevel As FoodSrvcPrefPreferLevel = FoodSrvcPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FoodSrvcType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum FoodSrvcPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class RecreationSrvcPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(RecreationSrvcPrefPreferLevel.Preferred)> _
        Public PreferLevel As RecreationSrvcPrefPreferLevel = RecreationSrvcPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RecreationSrvcType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum RecreationSrvcPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class BusinessSrvcPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(BusinessSrvcPrefPreferLevel.Preferred)> _
        Public PreferLevel As BusinessSrvcPrefPreferLevel = BusinessSrvcPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BusinessSrvcType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum BusinessSrvcPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class HotelPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyPref")> _
        Public LoyaltyPref() As LoyaltyPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentFormPref")> _
        Public PaymentFormPref() As PaymentFormPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("HotelChainPref")> _
        Public HotelChainPref() As HotelChainPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PropertyNamePref")> _
        Public PropertyNamePref() As PropertyNamePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PropertyLocationPref")> _
        Public PropertyLocationPref() As PropertyLocationPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PropertyTypePref")> _
        Public PropertyTypePref() As PropertyTypePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PropertyClassPref")> _
        Public PropertyClassPref() As PropertyClassPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PropertyAmenityPref")> _
        Public PropertyAmenityPref() As PropertyAmenityPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RoomAmenityPref")> _
        Public RoomAmenityPref() As RoomAmenityPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RoomLocationPref")> _
        Public RoomLocationPref() As RoomLocationPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("BedTypePref")> _
        Public BedTypePref() As BedTypePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FoodSrvcPref")> _
        Public FoodSrvcPref() As FoodSrvcPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MediaEntertainPref")> _
        Public MediaEntertainPref() As MediaEntertainPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PetInfoPref")> _
        Public PetInfoPref() As PetInfoPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MealPref")> _
        Public MealPref() As MealPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RecreationSrvcPref")> _
        Public RecreationSrvcPref() As RecreationSrvcPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("BusinessSrvcPref")> _
        Public BusinessSrvcPref() As BusinessSrvcPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PersonalSrvcPref")> _
        Public PersonalSrvcPref() As PersonalSrvcPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SecurityFeaturePref")> _
        Public SecurityFeaturePref() As SecurityFeaturePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PhysChallFeaturePref")> _
        Public PhysChallFeaturePref() As PhysChallFeaturePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SpecRequestPref")> _
        Public SpecRequestPref() As SpecRequestPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(HotelPrefPreferLevel.Preferred)> _
        Public PreferLevel As HotelPrefPreferLevel = HotelPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As HotelPrefShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As HotelPrefShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public SmokingAllowed As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RatePlanCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public HotelGuestType As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class OtherSrvcPref

        '<remarks/>
        Public OtherSrvcName As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("VendorPref")> _
        Public VendorPref() As VendorPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyPref")> _
        Public LoyaltyPref() As LoyaltyPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentFormPref")> _
        Public PaymentFormPref() As PaymentFormPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SpecRequestPref")> _
        Public SpecRequestPref() As SpecRequestPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(OtherSrvcPrefPreferLevel.Preferred)> _
        Public PreferLevel As OtherSrvcPrefPreferLevel = OtherSrvcPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As OtherSrvcPrefShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As OtherSrvcPrefShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelPurpose As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PersonalSrvcPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(PersonalSrvcPrefPreferLevel.Preferred)> _
        Public PreferLevel As PersonalSrvcPrefPreferLevel = PersonalSrvcPrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PersonalSrvcType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum PersonalSrvcPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SecurityFeaturePref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(SecurityFeaturePrefPreferLevel.Preferred)> _
        Public PreferLevel As SecurityFeaturePrefPreferLevel = SecurityFeaturePrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum SecurityFeaturePrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PhysChallFeaturePref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(PhysChallFeaturePrefPreferLevel.Preferred)> _
        Public PreferLevel As PhysChallFeaturePrefPreferLevel = PhysChallFeaturePrefPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhysChallFeatureType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum PhysChallFeaturePrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum HotelPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum HotelPrefShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum HotelPrefShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum OtherSrvcPrefPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum OtherSrvcPrefShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum OtherSrvcPrefShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum PrefCollectionShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum PrefCollectionShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum PrefCollectionsShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum PrefCollectionsShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PrefCollections

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PrefCollection")> _
        Public PrefCollection() As PrefCollection

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As PrefCollectionsShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As PrefCollectionsShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PrefCollection

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CommonPref")> _
        Public CommonPref() As CommonPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("VehicleRentalPref")> _
        Public VehicleRentalPref() As VehicleRentalPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AirlinePref")> _
        Public AirlinePref() As AirlinePref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("HotelPref")> _
        Public HotelPref() As HotelPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("OtherSrvcPref")> _
        Public OtherSrvcPref() As OtherSrvcPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As PrefCollectionShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As PrefCollectionShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelPurpose As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Profiles

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ProfileInfo")> _
        Public ProfileInfo() As ProfileInfo
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ProfileInfo

        '<remarks/>
        Public UniqueID As wmHotelAvail.UniqueID

        '<remarks/>
        Public Profile As Profile
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Profile

        '<remarks/>
        Public Accesses As wmHotelAvail.Accesses

        '<remarks/>
        Public Customer As Customer

        '<remarks/>
        Public PrefCollections As PrefCollections

        '<remarks/>
        Public CompanyInfo As CompanyInfo

        '<remarks/>
        Public Affiliations As Affiliations

        '<remarks/>
        Public Agreements As Agreements

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(ProfileShareAllSynchInd.No)> _
        Public ShareAllSynchInd As ProfileShareAllSynchInd = ProfileShareAllSynchInd.No

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(ProfileShareAllMarketInd.No)> _
        Public ShareAllMarketInd As ProfileShareAllMarketInd = ProfileShareAllMarketInd.No

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ProfileType As String

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
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CompanyInfo

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CompanyName")> _
        Public CompanyName() As wmHotelAvail.CompanyName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AddressInfo")> _
        Public AddressInfo() As AddressInfo

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TelephoneInfo")> _
        Public TelephoneInfo() As TelephoneInfo

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Email")> _
        Public Email() As Email

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("URL")> _
        Public URL() As URL

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("BusinessLocale")> _
        Public BusinessLocale() As BusinessLocale

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentForm")> _
        Public PaymentForm() As PaymentForm

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ContactPerson")> _
        Public ContactPerson() As ContactPerson

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TravelArranger")> _
        Public TravelArranger() As TravelArranger

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyProgram")> _
        Public LoyaltyProgram() As LoyaltyProgram
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TelephoneInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TelephoneInfoShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TelephoneInfoShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhoneLocationType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhoneTechType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CountryAccessCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AreaCityCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhoneNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Extension As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PIN As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public FormattedInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public DefaultInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhoneUseType As String
    End Class

    '<remarks/>
    Public Enum TelephoneInfoShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum TelephoneInfoShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class BusinessLocale

        '<remarks/>
        Public StreetNmbr As StreetNmbr

        '<remarks/>
        Public BldgRoom As String

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
        Public StateProv As StateProv

        '<remarks/>
        Public CountryName As CountryName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public FormattedInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As BusinessLocaleShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As BusinessLocaleShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String
    End Class

    '<remarks/>
    Public Enum BusinessLocaleShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum BusinessLocaleShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum ProfileShareAllSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No
    End Enum

    '<remarks/>
    Public Enum ProfileShareAllMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class GuestCounts

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("GuestCount")> _
        Public GuestCount() As GuestCount

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public IsPerRoom As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public IsPerRoomSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class GuestCount

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AgeQualifyingCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Age As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AgeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Count As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public CountSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Criterion

        '<remarks/>
        Public Position As Position

        '<remarks/>
        Public Address As Address

        '<remarks/>
        Public Telephone As Telephone

        '<remarks/>
        Public RefPoint As String

        '<remarks/>
        Public CodeRef As CodeRef

        '<remarks/>
        Public HotelRef As HotelRef

        '<remarks/>
        Public Radius As Radius

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("HotelAmenity")> _
        Public HotelAmenity() As HotelAmenity

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Award")> _
        Public Award() As Award

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExactMatch As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExactMatchSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ImportanceType As CriterionImportanceType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ImportanceTypeSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Position

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Latitude As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Longitude As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Altitude As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CodeRef

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LocationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute("IATA")> _
        Public CodeContext As String = "IATA"

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class HotelRef

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ChainCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BrandCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public HotelCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public HotelCityCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public HotelName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public HotelCodeContext As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ChainName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BrandName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AreaID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Radius

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Distance As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DistanceMeasure As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Direction As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class HotelAmenity

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Award

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Provider As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Rating As String
    End Class

    '<remarks/>
    Public Enum CriterionImportanceType

        '<remarks/>
        Mandatory

        '<remarks/>
        High

        '<remarks/>
        Medium

        '<remarks/>
        Low
    End Enum

End Namespace
