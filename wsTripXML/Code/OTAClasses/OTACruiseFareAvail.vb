Option Strict Off
Option Explicit On 

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmCruiseFareAvail

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class InclusivePackageOption

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CruisePackageCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public InclusiveIndicator As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StartDate As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Currency

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Budget

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PricingType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MinPrice As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MaxPrice As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public GuidelinePrice As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String
    End Class

End Namespace