Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmIssueMCOIn

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class TT_IssueMCORQ

        '<remarks/>
        Public POS As wmIssueMCOModels.POS

        '<remarks/>
        Public UniqueID As wmIssueMCOModels.UniqueID

        '<remarks/>
        Public ConversationID As String

        '<remarks/>
        Public MCOs() As wmIssueMCOModels.MCO

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
        Public TimeStamp As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public TimeStampSpecified As Boolean

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

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public MaxResponses As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public MaxResponsesSpecified As Boolean


    End Class
End Namespace
