Option Strict Off
Option Explicit On 

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmCruiseCategoryAvail

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
    Public Class SelectedSailing

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VoyageID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Start As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Duration As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public [End] As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VendorCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VendorName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShipCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShipName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public Status As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SailingInfo

        '<remarks/>
        Public SelectedSailing As SelectedSailing

        '<remarks/>
        Public InclusivePackageOption As InclusivePackageOption

        '<remarks/>
        Public Currency As Currency
    End Class

End Namespace
