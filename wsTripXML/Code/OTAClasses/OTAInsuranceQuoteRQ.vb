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
Namespace wsTravelTalk.wmInsuranceQuoteIn

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
        Public CompanyName As CompanyName

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

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EffectiveDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDate As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class BankAcct

        '<remarks/>
        Public BankAcctName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

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
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Beneficiary

        '<remarks/>
        Public Name As String

        '<remarks/>
        Public Address As Address

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Relation As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BenefitPercent As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class BookingChannel

        '<remarks/>
        Public CompanyName As CompanyName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Primary As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CardIssuerName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BankID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Cash

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CashIndicator As String
    End Class

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
    Public Class Telephone

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

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EffectiveDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDate As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CoveredTraveler

        '<remarks/>
        Public CoveredPerson As CoveredPerson

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Email")> _
        Public Email() As Email

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Address")> _
        Public Address() As Address

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Telephone")> _
        Public Telephone() As Telephone

        '<remarks/>
        Public CitizenCountryName As CitizenCountryName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Document")> _
        Public Document() As Document

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("EmergencyContact")> _
        Public EmergencyContact() As EmergencyContact

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Beneficiary")> _
        Public Beneficiary() As Beneficiary

        '<remarks/>
        Public IndCoverageReqs As IndCoverageReqs

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class EmergencyContact

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
    Public Class CoveredTravelers

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CoveredTraveler")> _
        Public CoveredTraveler() As CoveredTraveler
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CustLoyalty

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

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
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public SingleVendorInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SignupDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EffectiveDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VendorCode As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class DirectBill

        '<remarks/>
        Public CompanyName As CompanyName

        '<remarks/>
        Public Address As Address

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DirectBill_ID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class EmployerInfo

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
    Public Class InsuranceCustomer

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
        Public EmployerInfo As EmployerInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public Gender As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Deceased As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LockoutType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BirthDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VIP_Indicator As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String
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
        Public Cash As Cash

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CostCenterID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public PaymentTransactionTypeCode As String
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
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

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

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MaskedCardNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CardHolderRPH As String
    End Class

    '<remarks/>
    Public Enum PaymentCardCardCode

        '<remarks/>
        AX

        '<remarks/>
        BC

        '<remarks/>
        BL

        '<remarks/>
        CB

        '<remarks/>
        DN

        '<remarks/>
        DS

        '<remarks/>
        EC

        '<remarks/>
        JC

        '<remarks/>
        MC

        '<remarks/>
        TP

        '<remarks/>
        VI
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Voucher

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EffectiveDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDate As String

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
        Public ProgramName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PromotionCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PromotionVendorCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RedemptionQuantity As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class MiscChargeOrder

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TicketNumber As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class RelatedTraveler

        '<remarks/>
        Public UniqueID As UniqueID

        '<remarks/>
        Public PersonName As PersonName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Relation As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class UniqueID

        '<remarks/>
        Public CompanyName As CompanyName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
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
    Public Class OTA_InsuranceQuoteRQ

        '<remarks/>
        Public POS As POS

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PlanForQuoteRQ")> _
        Public PlanForQuoteRQ() As PlanForQuoteRQ

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
    Public Class POS

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Source")> _
        Public Source() As Source

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public TPA_Extensions() As Provider
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Source

        '<remarks/>
        Public RequestorID As RequestorID

        '<remarks/>
        Public Position As Position

        '<remarks/>
        Public BookingChannel As BookingChannel

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AgentSine As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PseudoCityCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ISOCountry As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ISOCurrency As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AgentDutyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirlineVendorID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirportCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FirstDepartPoint As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ERSP_UserID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TerminalID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class RequestorID

        '<remarks/>
        Public CompanyName As CompanyName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
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

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AltitudeUnitOfMeasure As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
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
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PlanForQuoteRQ

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public CoveredTravelers() As CoveredTraveler

        '<remarks/>
        Public InsCoverageDetail As InsCoverageDetail

        '<remarks/>
        Public InsuranceCustomer As InsuranceCustomer

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
        Public CommissionPercent As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TPA_Extensions

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Provider")> _
        Public Provider() As Provider
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CoveredPerson

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
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NameType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Relation As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BirthDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Age As String
    End Class


End Namespace