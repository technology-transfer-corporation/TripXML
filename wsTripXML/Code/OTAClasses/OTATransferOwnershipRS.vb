﻿'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'     Runtime Version: 1.1.4322.2407
'
'     Changes to this file may cause incorrect behavior and will be lost if 
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

'
'This source code was auto-generated by xsd, Version=1.1.4322.2407.
'
Namespace wsTravelTalk.wmTransferOwnershipOut

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)> _
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
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)> _
    Public Class Errors

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Error")> _
        Public [Error]() As [Error]
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)> _
    Public Class OTA_TransferOwnershipRS

        '<remarks/>
        Public Success As Success

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Warnings() As Warning

        '<remarks/>
        Public TPA_Extensions As TPA_ExtensionsRS

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Errors() As [Error]

        '<remarks/>
        Public UniqueID As UniqueIDRS

        '<remarks/>
        Public NewOwner As OTA_TransferOwnershipRSNewOwner

        '<remarks/>
        Public OtherOwner As OTA_TransferOwnershipRSOtherOwner

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
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)> _
    Public Class Success
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)> _
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
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)> _
    Public Class TPA_ExtensionsRS
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)> _
    Public Class UniqueIDRS

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String
    End Class

    '<remarks/>
    Public Class OTA_TransferOwnershipRSNewOwner

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PseudoCityCode As String
    End Class

    '<remarks/>
    Public Class OTA_TransferOwnershipRSOtherOwner

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)> _
    Public Class Warnings

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Warning")> _
        Public Warning() As Warning
    End Class
End Namespace
