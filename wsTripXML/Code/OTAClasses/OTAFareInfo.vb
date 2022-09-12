Option Strict Off
Option Explicit On 

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmInfoFares

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AdvResTicketing

        '<remarks/>
        Public AdvReservation As AdvReservation

        '<remarks/>
        Public AdvTicketing As AdvTicketing

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AdvResInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AdvTicketingInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RequestedTicketingDate As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AdvReservation

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LatestTimeOfDay As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LatestPeriod As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LatestUnit As AdvReservationLatestUnit

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public LatestUnitSpecified As Boolean
    End Class

    '<remarks/>
    Public Enum AdvReservationLatestUnit

        '<remarks/>
        Minutes

        '<remarks/>
        Hours

        '<remarks/>
        Days

        '<remarks/>
        Months

        '<remarks/>
        MON

        '<remarks/>
        TUES

        '<remarks/>
        WED

        '<remarks/>
        THU

        '<remarks/>
        FRI

        '<remarks/>
        SAT

        '<remarks/>
        SUN
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AdvTicketing

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FromResTimeOfDay As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FromResPeriod As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FromResUnit As AdvTicketingFromResUnit

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FromResUnitSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FromDepartTimeOfDay As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FromDepartPeriod As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FromDepartUnit As AdvTicketingFromDepartUnit

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FromDepartUnitSpecified As Boolean
    End Class

    '<remarks/>
    Public Enum AdvTicketingFromResUnit

        '<remarks/>
        Minutes

        '<remarks/>
        Hours

        '<remarks/>
        Days

        '<remarks/>
        Months

        '<remarks/>
        MON

        '<remarks/>
        TUES

        '<remarks/>
        WED

        '<remarks/>
        THU

        '<remarks/>
        FRI

        '<remarks/>
        SAT

        '<remarks/>
        SUN
    End Enum

    '<remarks/>
    Public Enum AdvTicketingFromDepartUnit

        '<remarks/>
        Minutes

        '<remarks/>
        Hours

        '<remarks/>
        Days

        '<remarks/>
        Months

        '<remarks/>
        MON

        '<remarks/>
        TUES

        '<remarks/>
        WED

        '<remarks/>
        THU

        '<remarks/>
        FRI

        '<remarks/>
        SAT

        '<remarks/>
        SUN
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Airline

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
    Public Class MinimumStay

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ReturnTimeOfDay As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MinStay As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StayUnit As MinimumStayStayUnit

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public StayUnitSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MinStayDate As String
    End Class

    '<remarks/>
    Public Enum MinimumStayStayUnit

        '<remarks/>
        Minutes

        '<remarks/>
        Hours

        '<remarks/>
        Days

        '<remarks/>
        Months

        '<remarks/>
        MON

        '<remarks/>
        TUES

        '<remarks/>
        WED

        '<remarks/>
        THU

        '<remarks/>
        FRI

        '<remarks/>
        SAT

        '<remarks/>
        SUN
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class MaximumStay

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ReturnType As MaximumStayReturnType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ReturnTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ReturnTimeOfDay As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MaxStay As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StayUnit As MaximumStayStayUnit

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public StayUnitSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MaxStayDate As String
    End Class

    '<remarks/>
    Public Enum MaximumStayReturnType

        '<remarks/>
        C

        '<remarks/>
        S
    End Enum

    '<remarks/>
    Public Enum MaximumStayStayUnit

        '<remarks/>
        Minutes

        '<remarks/>
        Hours

        '<remarks/>
        Days

        '<remarks/>
        Months

        '<remarks/>
        MON

        '<remarks/>
        TUES

        '<remarks/>
        WED

        '<remarks/>
        THU

        '<remarks/>
        FRI

        '<remarks/>
        SAT

        '<remarks/>
        SUN
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class VoluntaryChanges

        '<remarks/>
        Public Penalty As Penalty

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VolChangeInd As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Penalty

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PenaltyType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DepartureStatus As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Percent As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ExchangeRate

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FromCurrency As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ToCurrency As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Rate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public [Date] As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ExchangeRates

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ExchangeRate")> _
        Public ExchangeRate() As ExchangeRate
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ValidatingAirline

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

End Namespace
