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
    Partial Public Class PAY_ScheduleVirtualCardLoadRS
    
        '<remarks/>
        Public Property ConversationID As String

        ''<remarks/>
        'Public Property Success As VirtualCreditCard.Success

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Warnings() As VirtualCreditCard.Warning

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Errors() As VirtualCreditCard.[Error]
    
       '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Failure", GetType(Failure)),  _
            System.Xml.Serialization.XmlElementAttribute("Success", GetType(Success))>  _
        Public Property Item() As Object
    
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
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Target As VirtualCreditCard.Target
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property TargetSpecified As Boolean
    
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
    Partial Public Class Failure
    
        '''<remarks/>
        Public Property Warnings As VirtualCreditCard.GenericWarningsType
    
        '''<remarks/>
        Public Property Errors As VirtualCreditCard.GenericErrorsType

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2")>  _
    Partial Public Class Success
    
        '''<remarks/>
        Public Property Target As SuccessTarget
    
        '''<remarks/>
        Public Property FundsTransfer As VirtualCreditCard.FundsTransferDetailsType
    
        '''<remarks/>
        Public Property Warnings As VirtualCreditCard.GenericWarningsType

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2")>  _
    Partial Public Class SuccessTarget
        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Reference", IsNullable:=false)>  _
        Public Property References As SuccessTargetReference()
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2")>  _
    Partial Public Class SuccessTargetReference

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Type As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String

    End Class

    
End Namespace
