Option Strict Off
Option Explicit On

Imports System

Namespace wsTravelTalk.wmContractManager
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ContractManagerRQ
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         ComponentModel.DefaultValueAttribute("")> _
        Public RecordLocator As String = String.Empty

        <System.Xml.Serialization.XmlAttributeAttribute(), _
         ComponentModel.DefaultValueAttribute(True)> _
        Public IsLoaded As Boolean = True

        <System.Xml.Serialization.XmlAttributeAttribute(), _
         ComponentModel.DefaultValueAttribute(False)> _
        Public IsTracing As Boolean = False

        '<remarks/>
        Public POS As POS

        Public FQH As String
        Public MISS As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class POS

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Source")> _
        Public Source() As Source

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TPA_Extensions

        '<remarks/>
        Public Provider As Provider
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Source

        '<remarks/>
        Public RequestorID As RequestorID

        '<remarks/>
        Public Position As Position

        '<remarks/>
        Public BookingChannel As BookingChannel

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AgentSine As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PseudoCityCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ISOCountry As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ISOCurrency As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AgentDutyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirlineVendorID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirportCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FirstDepartPoint As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ERSP_UserID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class RequestorID

        '<remarks/>
        Public CompanyName As CompanyName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")> _
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
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Position

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Latitude As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Longitude As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Altitude As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class BookingChannel

        '<remarks/>
        Public CompanyName As CompanyName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Primary As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PrimarySpecified As Boolean
    End Class

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
    Public Class Provider

        '<remarks/>
        Public Name As String

        '<remarks/>
        Public System As String

        '<remarks/>
        Public Userid As String

        '<remarks/>
        Public Password As String
    End Class


End Namespace
