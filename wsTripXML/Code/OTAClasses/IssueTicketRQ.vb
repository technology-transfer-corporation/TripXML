﻿'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'     Runtime Version: 1.1.4322.573
'
'     Changes to this file may cause incorrect behavior and will be lost if 
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

'
'This source code was auto-generated by xsd, Version=1.1.4322.573.
'
Namespace wsTravelTalk.wmIssueTicketIn

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TT_IssueTicketRQ

        '<remarks/>
        Public POS As POS

        '<remarks/>
        Public UniqueID As UniqueID

        '<remarks/>
        Public Fulfillment As Fulfillment

        '<remarks/>
        Public Ticketing As Ticketing

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EchoToken As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TimeStamp As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TimeStampSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(TT_IssueTicketRQTarget.Production)> _
        Public Target As TT_IssueTicketRQTarget = TT_IssueTicketRQTarget.Production

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Version As Double

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public VersionSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransactionIdentifier As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SequenceNmbr As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SequenceNmbrSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransactionStatusCode As TT_IssueTicketRQTransactionStatusCode

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransactionStatusCodeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PrimaryLangID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AltLangID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MaxResponses As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public MaxResponsesSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public DirectFlightsOnly As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NumberStops As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public NumberStopsSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class POS

        '<remarks/>
        Public Source As Source

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Source

        '<remarks/>
        Public RequestorID As RequestorID

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
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class RequestorID

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
    Public Class Ticketing

        '<remarks/>
        Public OtherPrinter As String

        '<remarks/>
        Public StockRange As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FareNumber")> _
        Public FareNumber() As Integer

        '<remarks/>
        Public Notification As Notification

        '<remarks/>
        Public OrderNumber As String

        '<remarks/>
        Public BookingPCC As String

        '<remarks/>
        Public TicketingPrinter As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public BoardingPass As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelerRefNumberRPHList As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FlightRefNumberRPHList As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public InfantOnly As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public InfantOnlySpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TicketType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public IssueMCO As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public IssueMCOSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public IssueInvoice As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public IssueInvoiceSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public OmitInfant As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public OmitInfantSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public IssueItinerary As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public IssueItinerarySpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Exchange As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExchangeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RemoteLocation As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SpecialInstruction As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public IssueJointInvoice As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public IssueJointInvoiceSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=True)> _
    Public Enum TicketingTicketType

        '<remarks/>
        eTicket

        '<remarks/>
        Paper

        '<remarks/>
        None
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Notification

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ByEmail As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ByFax As Boolean = False

    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class UniqueID

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String
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
    Public Class TPA_Extensions

        '<remarks/>
        Public Provider As Provider
    End Class

    '<remarks/>
    Public Enum TT_IssueTicketRQTarget

        '<remarks/>
        Test

        '<remarks/>
        Production
    End Enum

    '<remarks/>
    Public Enum TT_IssueTicketRQTransactionStatusCode

        '<remarks/>
        Start

        '<remarks/>
        [End]

        '<remarks/>
        Rollback

        '<remarks/>
        InSeries
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Fulfillment

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public PaymentDetails() As PaymentDetail
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PaymentDetail

        '<remarks/>
        Public PaymentCard As PaymentCard

        '<remarks/>
        Public DirectBill As DirectBill

        '<remarks/>
        Public MiscChargeOrder As MiscChargeOrder

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As PaymentDetailShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As PaymentDetailShareMarketInd

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
    Public Enum PaymentDetailShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum PaymentDetailShareMarketInd

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
    Public Class CardHolderName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BankID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PaymentCard

        '<remarks/>
        Public CardHolderName As CardHolderName

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

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ConfirmationNumber As String
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
    Public Class MiscChargeOrder

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TicketNumber As String
    End Class

End Namespace
