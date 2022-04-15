Option Strict Off
Option Explicit On 

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmCruisePackageAvail

    '<remarks/>
    Public Enum PackageOptionPackageType

        '<remarks/>
        Pre

        '<remarks/>
        Post
    End Enum

    '<remarks/>
    Public Enum TransferOptionPackageType

        '<remarks/>
        Pre

        '<remarks/>
        Post

        '<remarks/>
        Round
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SelectedCategory

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public CabinFilters() As CabinFilter

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SelectedCabin")> _
        Public SelectedCabin() As SelectedCabin

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BerthedCategoryCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PricedCategoryCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DeckName As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SelectedCabin

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public CabinFilters() As CabinFilter

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Status As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ReleaseDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ReleaseDateTimeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CabinNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BedConfigurationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MaxOccupancy As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DeclineIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DeclineIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public HeldIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public HeldIndicatorSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CabinFilters

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CabinFilter")> _
        Public CabinFilter() As CabinFilter
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CabinFilter

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CabinFilterCode As String
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
    Public Class SelectedFare

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FareCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public GroupCode As String
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
