Option Strict Off
Option Explicit On 

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmCarAvail

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
    Public Class CountryName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

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

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public DefaultInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhoneUseType As String
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
    Public Class VehRentalCore

        '<remarks/>
        Public PickUpLocation As PickUpLocation

        '<remarks/>
        Public ReturnLocation As ReturnLocation

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PickUpDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PickUpDateTimeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ReturnDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ReturnDateTimeSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PickUpLocation

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
    Public Class ReturnLocation

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
    Public Class OffLocService

        '<remarks/>
        Public Address As Address

        '<remarks/>
        Public PersonName As PersonName

        '<remarks/>
        Public Telephone As Telephone

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As OffLocServiceType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SpecInstructions As String
    End Class

    '<remarks/>
    Public Enum OffLocServiceType

        '<remarks/>
        CustPickUp

        '<remarks/>
        VehDelivery

        '<remarks/>
        CustDropOff

        '<remarks/>
        VehCollection
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class VehClass

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Size As String
    End Class

    '<remarks/>
    Public Enum RateQualifierRatePeriod

        '<remarks/>
        Hourly

        '<remarks/>
        Daily

        '<remarks/>
        Weekly

        '<remarks/>
        Monthly

        '<remarks/>
        WeekendDay

        '<remarks/>
        Other
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TourInfo

        '<remarks/>
        Public TourOperator As TourOperator

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TourNumber As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TourOperator

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

End Namespace
