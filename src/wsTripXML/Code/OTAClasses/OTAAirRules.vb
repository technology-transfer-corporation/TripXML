Namespace wsTravelTalk.wmAirRules

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AdvResTicketing

        '<remarks/>
        Public AdvReservation As AdvReservation

        '<remarks/>
        Public AdvTicketing As AdvTicketing

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AdvResInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AdvResIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AdvTicketingInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AdvTicketingIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AdvReservation

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LatestTimeOfDay As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public LatestTimeOfDaySpecified As Boolean

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
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AdvTicketing

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FromResTimeOfDay As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FromResTimeOfDaySpecified As Boolean

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
        Public FromDepartTimeOfDay As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FromDepartTimeOfDaySpecified As Boolean

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
    Public Class ChargesRules

        '<remarks/>
        Public VoluntaryChanges As VoluntaryChanges
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class VoluntaryChanges

        '<remarks/>
        Public Penalty As Penalty

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VolChangeInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public VolChangeIndSpecified As Boolean
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
        Public Amount As Double

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AmountSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DecimalPlacesSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Percent As Double

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PercentSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class FilingAirline

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
    Public Class LengthOfStayRules

        '<remarks/>
        Public MinimumStay As MinimumStay

        '<remarks/>
        Public MaximumStay As MaximumStay

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StayRestrictionsInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public StayRestrictionsIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class MinimumStay

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ReturnTimeOfDay As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ReturnTimeOfDaySpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MinStay As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public MinStaySpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StayUnit As MinimumStayStayUnit

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public StayUnitSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MinStayDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public MinStayDateSpecified As Boolean
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
        Public ReturnTimeOfDay As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ReturnTimeOfDaySpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MaxStay As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public MaxStaySpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StayUnit As MaximumStayStayUnit

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public StayUnitSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MaxStayDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public MaxStayDateSpecified As Boolean
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
    Public Class RuleInfo

        '<remarks/>
        Public ResTicketingRules As ResTicketingRules

        '<remarks/>
        Public LengthOfStayRules As LengthOfStayRules

        '<remarks/>
        Public ChargesRules As ChargesRules
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ResTicketingRules

        '<remarks/>
        Public AdvResTicketing As AdvResTicketing
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SubSection

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Paragraph")> _
        Public Paragraph() As Paragraph

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SubTitle As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SubCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SubSectionNumber As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SubSectionNumberSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Paragraph

        '<remarks/>
        Public [Text] As [Text]

        '<remarks/>
        Public Image As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="anyURI")> _
        Public URL As String

        '<remarks/>
        Public ListItem As ListItem

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Name As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ParagraphNumber As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ParagraphNumberSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CreateDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public CreateDateTimeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CreatorID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LastModifyDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public LastModifyDateTimeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LastModifierID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class [Text]

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Formatted As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FormattedSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ListItem

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Formatted As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FormattedSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute("ListItem")> _
        Public ListItem1 As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ListItem1Specified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
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

End Namespace
