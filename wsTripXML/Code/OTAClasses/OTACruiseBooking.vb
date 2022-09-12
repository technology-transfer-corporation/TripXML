Option Strict Off
Option Explicit On 

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmCruiseBooking

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CompanyName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CompanyShortName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelSector As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CodeContext As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

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
        Public ReleaseDateTime As String

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
        Public DeclineIndicator As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public HeldIndicator As String
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
    Public Class CabinFilters

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CabinFilter")> _
        Public CabinFilter() As CabinFilter
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
    Public Class GuestCity

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LocationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CodeContext As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class GuestTransportation

        '<remarks/>
        Public GuestCity As GuestCity

        '<remarks/>
        Public GatewayCity As GatewayCity

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransportationMode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public TransportationStatus As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class GatewayCity

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LocationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CodeContext As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ListItem

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Formatted As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute("ListItem")> _
        Public ListItem1 As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class [Text]

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Formatted As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

End Namespace

