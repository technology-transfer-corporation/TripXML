Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Threading
Imports System.Data
Imports CompressionExtension
Imports Microsoft.VisualBasic
Imports System.Text

Namespace wsTravelTalk


    Public Class ndcAuthenticate
        Inherits System.Web.Services.Protocols.SoapHeader

        Public POS As POS
        Public compressed As Boolean = True
    End Class

    Public Class POS
        Public Source As Source

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class Source

        '<remarks/>
        Public RequestorID As RequestorID

        '<remarks/>
        Public Profile As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public PseudoCityCode As String

    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class RequestorID

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public ID As String

    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class TPA_Extensions

        '<remarks/>
        Public Provider As Provider

    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class Provider

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Name")>
        Public Name() As Name

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("System")>
        Public GDSSystem As String

        '<remarks/>
        Public Userid As String

        '<remarks/>
        Public Password As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class Name

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public PseudoCityCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class Ticket

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public StatusIndicator As String

        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Exchange As Boolean = True

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public OmitInfant As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public OmitInfantSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PassangerInfo")>
        Public PassangerInfo() As PassangerDetails

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public TicketType As TicketTypeType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Number As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(),
         System.ComponentModel.DefaultValueAttribute(False)>
        Public BoardingPass As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public FlightRefNumberRPHList As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public InfantOnly As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public InfantOnlySpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public IssueInvoice As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public IssueInvoiceSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public IssueItinerary As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public IssueItinerarySpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public ExchangeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public RemoteLocation As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public SpecialInstruction As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public IssueJointInvoice As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public IssueJointInvoiceSpecified As Boolean
    End Class


    '<remarks/>
    Public Enum TicketTypeType

        '<remarks/>
        Electronic

        '<remarks/>
        Paper

        '<remarks/>
        MCO

        '<remarks/>
        Void
    End Enum


    '''<remarks/>
    Partial Public Class PassangerDetails

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Code As String

    End Class


End Namespace