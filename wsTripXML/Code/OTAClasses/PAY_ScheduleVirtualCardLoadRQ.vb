﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version: 4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if 
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System.Xml.Serialization
'
'This source code was auto-generated by xsd, Version=4.6.1055.0.
'

Namespace wsTravelTalk.wmScheduleVirtualCardLoad

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2"),  _
        System.Xml.Serialization.XmlRootAttribute([Namespace]:="http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2", IsNullable:=false)>  _
    Partial Public Class PAY_ScheduleVirtualCardLoadRQ
    
        '<remarks/>
        Public Property POS As VirtualCreditCard.POS

        '<remarks/>
        Public Property ConversationID As String

        '''<remarks/>
        Public Property Target As Target
    
        '''<remarks/>
        Public Property FundsTransfer As VirtualCreditCard.FundsTransferType
    
        '''<remarks/>
        Public Property Reason As Reason
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property EchoToken As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property TimeStamp As Date
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property TimeStampSpecified As Boolean
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute("Target")>  _
        Public Property Target1 As VirtualCreditCard.Target
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property Target1Specified As Boolean
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property TargetName As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Version As Decimal
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property TransactionIdentifier As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property SequenceNmbr As Long
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property SequenceNmbrSpecified As Boolean
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property TransactionStatusCode As VirtualCreditCard.TransactionStatusCode
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property TransactionStatusCodeSpecified As Boolean
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")>  _
        Public Property PrimaryLangID As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")>  _
        Public Property AltLangID As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property RetransmissionIndicator As Boolean
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property RetransmissionIndicatorSpecified As Boolean
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property CorrelationID As String

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2")>  _
    Partial Public Class Target
    
        Private referencesField() As TargetReference
    
        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Reference", IsNullable:=false)>  _
        Public Property References() As TargetReference()
            Get
                Return Me.referencesField
            End Get
            Set
                Me.referencesField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2")>  _
    Partial Public Class TargetReference
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Type As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String

    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2")>  _
    Partial Public Class Reason

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Label As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")>  _
        Public Property Language As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String

    End Class

    
End Namespace