﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.9148
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

'
'This source code was auto-generated by xsd, Version=2.0.50727.3038.
'

Namespace wsTravelTalk.wsReIssueTicket

    Partial Public Class TT_ReIssueTicketRQ

        '<remarks/>
        Public POS As POS

        '<remarks/>
        Public UniqueID As UniqueID

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public AltLangID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public EchoToken As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public TimeStamp As Date

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(),
         System.ComponentModel.DefaultValueAttribute(TT_ReIssueTicketRQTarget.Production)>
        Public Target As TT_ReIssueTicketRQTarget = TT_ReIssueTicketRQTarget.Production

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Version As Double

        '<remarks/>
        Public ConversationID As String

        '<remarks/>
        Public ReIssue As Reissue

    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class Reissue

        '<remarks/>
        Public OtherPrinter As String

        '<remarks/>
        Public StockRange As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Ticket")>
        Public Ticket() As Ticket

        '<remarks/>
        Public OrderNumber As String

        '<remarks/>
        Public BookingPCC As String

        '<remarks/>
        Public TicketingPrinter As String


    End Class


    '<remarks/>
    Public Enum TicketingControlType

        '<remarks/>
        OK

        '<remarks/>
        TF
    End Enum

    '<remarks/>
    Public Enum TT_ReIssueTicketRQTarget

        '<remarks/>
        Test

        '<remarks/>
        Production
    End Enum
    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class UniqueID

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public ID As String
    End Class

    ''''<remarks/>
    'Partial Public Class TicketInfo
    '    '''<remarks/>
    '    Public TicketDetails As DocumentDetails
    '    '''<remarks/>
    '    Public CouponInfoFirst As CouponInfoFirst
    '    '''<remarks/>
    '    Public PaperInformation As PaperInformation
    'End Class

    '''<remarks/>
    'Partial Public Class DocumentDetails
    '    '<remarks/>
    '    <System.Xml.Serialization.XmlAttributeAttribute()>
    '    Public Number As String
    '    '<remarks/>
    '    <System.Xml.Serialization.XmlAttributeAttribute()>
    '    Public TicketType As String
    'End Class

    '''<remarks/>
    Partial Public Class CouponInfoFirst

        '''<remarks/>
        Public CouponNumber As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("OtherCouponDetails")>
        Public OtherCoupons As String()

    End Class

    ''''<remarks/>
    'Partial Public Class PaperInformation
    '    '''<remarks/>
    '    Public DocumentDetails As DocumentDetails
    '    '''<remarks/>
    '    Public CouponNumber As String
    '    '''<remarks/>
    '    <System.Xml.Serialization.XmlElementAttribute("otherCouponDetails")>
    '    Public OtherCoupons As String()
    '    '''<remarks/>
    '    Public TicketRange As TicketRange
    'End Class

    '''<remarks/>
    Partial Public Class TicketRange
        '''<remarks/>
        Public PaperticketDetailsfirst As Ticket
        '''<remarks/>
        Public PaperticketDetailsLast As Ticket
    End Class


    '''<remarks/>
    Partial Public Class TypeReprice

        '''<remarks/>
        Public StatusDetails As StatusDetails

    End Class

    Partial Public Class StatusDetails

        '''<remarks/>
        Public Indicator As String

    End Class

    Partial Public Class ReissuelPricingOptions

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("AttributeDetails", IsNullable:=False)>
        Public OverrideInformation As AttributeDetails()

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("CityDetail", IsNullable:=False)>
        Public CityOverride As CityDetail()

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("DiscountInformation")>
        Public iscountInformation As DiscountInformation()

    End Class

    '''<remarks/>
    Partial Public Class AttributeDetails

        '''<remarks/>
        Public AttributeType As String

        '''<remarks/>
        Public AttributeDescription As String

    End Class

    '''<remarks/>
    Partial Public Class CityDetail

        '''<remarks/>
        Public CityCode As String

        '''<remarks/>
        Public CityQualifier As String

    End Class

    '''<remarks/>
    Partial Public Class DiscountInformation

        '''<remarks/>
        Public PenDisInformation As PenDisInformation

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("RefDetails", IsNullable:=False)>
        Public ReferenceQualifier As RefDetails()

    End Class

    '''<remarks/>
    Partial Public Class PenDisInformation

        '''<remarks/>
        Public InfoQualifier() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("penDisData")>
        Public PenDisData As PenDisData()

    End Class

    '''<remarks/>
    Partial Public Class PenDisData

        '''<remarks/>
        Public PenaltyType() As String

        '''<remarks/>
        Public PenaltyQualifier() As String

        '''<remarks/>
        Public PenaltyAmount() As Decimal

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public PenaltyAmountSpecified() As Boolean

        '''<remarks/>
        Public DiscountCode() As String

        '''<remarks/>
        Public PenaltyCurrency() As String

    End Class

    '''<remarks/>
    Partial Public Class RefDetails
        '''<remarks/>
        Public RefQualifier() As String

        '''<remarks/>
        Public RefNumber() As Decimal

    End Class

End Namespace