﻿'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'     Runtime Version: 1.1.4322.2032
'
'     Changes to this file may cause incorrect behavior and will be lost if 
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System.Xml.Serialization
Imports wsTripXML.wsTravelTalk.wmInsuranceQuote

'
'This source code was auto-generated by xsd, Version=1.1.4322.2032.
'
Namespace wsTravelTalk.wmInsuranceQuoteOut

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AddressRS

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
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FormattedInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public UseType As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class BasePremium

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class BookingDetailURL

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Charge

        '<remarks/>
        Public Taxes As Taxes

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Description")> _
        Public Description() As Description

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TaxInclusive As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As ChargeType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Percent As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ChargeUnit As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EffectiveDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDate As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Taxes

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Tax")> _
        Public Tax() As Tax

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Tax

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TaxDescription")> _
        Public TaxDescription() As TaxDescription

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As TaxType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Percent As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ChargeUnit As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EffectiveDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDate As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TaxDescription

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Text")> _
        Public [Text]() As [Text]

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Image")> _
        Public Image() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("URL")> _
        Public URL() As URL

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ListItem")> _
        Public ListItem() As ListItem

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Name As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ParagraphNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CreateDateTime As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CreatorID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LastModifyDateTime As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LastModifierID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class [Text]

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Formatted As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ListItem

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Formatted As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute("ListItem")> _
        Public ListItem1 As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum TaxType

        '<remarks/>
        Inclusive

        '<remarks/>
        Exclusive

        '<remarks/>
        Cumulative
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Description

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Text")> _
        Public [Text]() As [Text]

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Image")> _
        Public Image() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("URL")> _
        Public URL() As URL

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ListItem")> _
        Public ListItem() As ListItem

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Name As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ParagraphNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CreateDateTime As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CreatorID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LastModifyDateTime As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LastModifierID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String
    End Class

    '<remarks/>
    Public Enum ChargeType

        '<remarks/>
        Inclusive

        '<remarks/>
        Exclusive

        '<remarks/>
        Cumulative
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Charges

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Charge")> _
        Public Charge() As Charge
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Contact

        '<remarks/>
        Public PersonName As PersonName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Telephone")> _
        Public Telephone() As TelephoneRS

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Address")> _
        Public Address() As AddressRS

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Email")> _
        Public Email() As Email

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("URL")> _
        Public URL() As URL

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CompanyName")> _
        Public CompanyName() As CompanyName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("EmployeeInfo")> _
        Public EmployeeInfo() As EmployeeInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ContactType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Relation As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EmergencyFlag As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TelephoneRS

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

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
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FormattedInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhoneUseType As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class [Error]

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShortText As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocURL As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Status As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Tag As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RecordID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NodeList As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Errors

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Error")> _
        Public [Error]() As [Error]
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class OTA_InsuranceQuoteRS

        '<remarks/>
        Public Success As Success

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Warnings() As Warning

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PlanForQuoteRS")> _
        Public PlanForQuoteRS() As PlanForQuoteRS

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Errors() As [Error]

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EchoToken As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TimeStamp As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public Target As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Version As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransactionIdentifier As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SequenceNmbr As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public TransactionStatusCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PrimaryLangID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AltLangID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Success
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Warning

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShortText As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocURL As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Status As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Tag As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RecordID As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PlanForQuoteRS

        '<remarks/>
        Public QuoteDetail As QuoteDetail

        '<remarks/>
        Public InsCoverageDetail As InsCoverageDetail

        '<remarks/>
        Public PlanCost As PlanCost

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Contact")> _
        Public Contact() As Contact

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PlanID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Name As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TypeID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class QuoteDetail

        '<remarks/>
        Public ProviderCompany As ProviderCompany

        '<remarks/>
        Public QuoteDetailURL As QuoteDetailURL

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public PlanRestrictions() As PlanRestriction

        '<remarks/>
        Public BookingDetailURL As BookingDetailURL

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EffectiveDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDate As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ProviderCompany

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
    Public Class QuoteDetailURL

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PlanRestriction

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Name As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CodeContext As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PlanCost

        '<remarks/>
        Public BasePremium As BasePremium

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Charges() As Charge

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Warnings

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Warning")> _
        Public Warning() As Warning
    End Class

End Namespace
