Option Strict Off
Option Explicit On 

Imports System.Xml.Serialization
Imports wsTripXML.wsTravelTalk.wmCruiseBooking

Namespace wsTravelTalk.wmCruiseCreateBooking

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SailingInfo

        '<remarks/>
        Public SelectedSailing As SelectedSailing

        '<remarks/>
        Public InclusivePackageOption As InclusivePackageOption

        '<remarks/>
        Public Currency As Currency

        '<remarks/>
        Public SelectedCategory As SelectedCategory

        '<remarks/>
        Public DeparturePort As DeparturePort

        '<remarks/>
        Public ArrivalPort As ArrivalPort

        '<remarks/>
        Public FaxNotification As FaxNotification
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class FaxNotification

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("NotificationType")> _
        Public NotificationType() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhoneLocationType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhoneTechType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CountryAccessCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AreaCityCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PhoneNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Extension As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PIN As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class DeparturePort

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LocationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ArrivalPort

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LocationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ReservationID

        '<remarks/>
        Public CompanyName As CompanyName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public URL As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Instance As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ID_Context As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SyncDateTime As String
    End Class

End Namespace
