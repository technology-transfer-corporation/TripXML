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
Namespace wsTravelTalk.wmCurConvOut
    
    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class OTA_CurConvRS

        '<remarks/>
        Public Success As Success

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Error", IsNullable:=False)> _
        Public Errors() As ErrorType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Conversion")> _
        Public Conversion() As ConversionType

        '<remarks/>
        Public ConversationID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Version As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Success
    End Class

    '<remarks/>
    Public Class ToType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String
    End Class

    '<remarks/>
    Public Class FromType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String
    End Class

    '<remarks/>
    Public Class ConversionType

        '<remarks/>
        Public From As FromType

        '<remarks/>
        Public [To] As ToType

        '<remarks/>
        Public ConversionRate As String

        '<remarks/>
        Public Rounding As String

        '<remarks/>
        Public Remark As String
    End Class

    '<remarks/>
    Public Class ErrorType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

End Namespace
