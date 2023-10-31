Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmIssueMCOOut

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class TT_IssueMCORS
        '<remarks/>
        Public Success As wmIssueMCOModels.Success

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Warning", IsNullable:=False)>
        Public Warnings() As String

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Error", IsNullable:=False)>
        Public Errors() As String

        '<remarks/>
        Public ConversationID As String

        '<remarks/>
        Public MCOs() As wmIssueMCOModels.MCOMask

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(),
         System.ComponentModel.DefaultValueAttribute(wmIssueMCOModels.Target.Production)>
        Public Target As wmIssueMCOModels.Target = wmIssueMCOModels.Target.Production

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public TransactionStatusCode As wmIssueMCOModels.TransactionStatusCode

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public EchoToken As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Version As Double

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public VersionSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public PrimaryLangID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public AltLangID As String


    End Class




End Namespace
