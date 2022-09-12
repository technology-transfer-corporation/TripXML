
Option Strict Off
Option Explicit On 

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmAirPrice

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class OriginDestinationOption

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FlightSegment")> _
        Public FlightSegment() As FlightSegment
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class OriginDestinationOptions

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(GetType(FlightSegment), IsNullable:=False)> _
        Public OriginDestinationOption()() As FlightSegment
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AirItinerary

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("OriginDestinationOption", IsNullable:=False), _
         System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False, NestingLevel:=1)> _
        Public OriginDestinationOptions()() As FlightSegment

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DirectionInd As AirItineraryDirectionInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DirectionIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class FlightSegment

        '<remarks/>
        Public DepartureAirport As DepartureAirport

        '<remarks/>
        Public ArrivalAirport As ArrivalAirport

        '<remarks/>
        Public OperatingAirline As OperatingAirline

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Equipment")> _
        Public Equipment() As Equipment

        '<remarks/>
        Public MarketingAirline As MarketingAirline

        '<remarks/>
        Public MarriageGrp As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DepartureDateTime As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ArrivalDateTime As String

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ArrivalDateTimeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StopQuantity As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public StopQuantitySpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public InfoSource As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FlightNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TourOperatorFlightID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ResBookDesigCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ActionCode As FlightSegmentActionCode

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ActionCodeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NumberInParty As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public NumberInPartySpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FlightContext As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class DepartureAirport

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LocationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute("IATA")> _
        Public CodeContext As String = "IATA"

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ArrivalAirport

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LocationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute("IATA")> _
        Public CodeContext As String = "IATA"

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class OperatingAirline

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
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FlightNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String


    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class MarketingAirline

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
    Public Class Equipment

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirEquipType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public ChangeofGauge As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PassengerTypeQuantity

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Age As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AgeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CodeContext As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")> _
        Public URI As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Quantity As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public QuantitySpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(True)> _
        Public Changeable As Boolean = True
    End Class

    '<remarks/>
    Public Enum FlightSegmentActionCode

        '<remarks/>
        OK

        '<remarks/>
        Waitlist

        '<remarks/>
        Other
    End Enum

    '<remarks/>
    Public Enum AirItineraryDirectionInd

        '<remarks/>
        OneWay

        '<remarks/>
        [Return]

        '<remarks/>
        Circle

        '<remarks/>
        OpenJaw

        '<remarks/>
        Other
    End Enum

End Namespace
