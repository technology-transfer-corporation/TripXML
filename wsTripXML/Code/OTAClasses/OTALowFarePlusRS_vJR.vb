﻿
Option Strict Off
Option Explicit On

Imports System.Xml.Serialization


Namespace wsTravelTalk.wmLowFarePlusOut_vJR

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AdvResTicketingRS

        '<remarks/>
        Public AdvReservation As AdvReservationRS

        '<remarks/>
        Public AdvTicketing As AdvTicketingRS

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
    Public Class AdvReservationRS

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
    Public Class AdvTicketingRS

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

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
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
    Public Class AirItinerary

        ''<remarks/>
        '<System.Xml.Serialization.XmlArrayItemAttribute("OriginDestinationOption", IsNullable:=False), _
        ' System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False, NestingLevel:=1)> _
        'Public OriginDestinationOptions()() As FlightSegment

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public OriginDestinationOptions() As OriginDestinationOption

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
        Public TPA_Extensions As TPA_ExtensionsRS

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
        Public E_TicketEligibility As E_TicketEligibilityType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public E_TicketEligibilitySpecified As Boolean
    End Class

    '<remarks/>
    Public Enum E_TicketEligibilityType

        '<remarks/>
        Eligible

        '<remarks/>
        NotEligible

        '<remarks/>
        Required
    End Enum

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

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EquipTypeText As String
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
    Public Class TPA_ExtensionsRS

        '<remarks/>
        Public CabinType As CabinType

        '<remarks/>
        Public JourneyTotalDuration As String

        '<remarks/>
        Public JourneyDuration As String

        '<remarks/>
        Public PricedCode As String

        '<remarks/>
        Public Text() As String

        '<remarks/>
        Public FlightContext As String
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

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AirItineraryPricingInfo

        '<remarks/>
        Public ItinTotalFare As ItinTotalFare

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public PTC_FareBreakdowns() As PTC_FareBreakdown

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public FareInfos() As FareInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PricingSource As AirItineraryPricingInfoPricingSource

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PricingSourceSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ValidatingAirlineCode As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ItinTotalFare

        '<remarks/>
        Public BaseFare As BaseFare

        '<remarks/>
        Public EquivFare As EquivFare

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Taxes() As Tax

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Fees() As Fee

        '<remarks/>
        Public TotalFare As TotalFare

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public NegotiatedFare As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NegotiatedFareCode As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class BaseFare

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
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class EquivFare

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
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Tax

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TaxCode As String

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
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Fee

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FeeCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FeeType As String

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
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TotalFare

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
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PTC_FareBreakdown

        '<remarks/>
        Public PassengerTypeQuantity As PassengerTypeQuantityRS

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("FareBasisCode", IsNullable:=False)> _
        Public FareBasisCodes() As String

        '<remarks/>
        Public PassengerFare As PassengerFare

        '<remarks/>
        Public TPA_Extensions As TPA_ExtensionsRS

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PricingSource As PTC_FareBreakdownPricingSource

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PricingSourceSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PassengerTypeQuantityRS

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
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PassengerFare

        '<remarks/>
        Public BaseFare As BaseFare

        '<remarks/>
        Public EquivFare As EquivFare

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Taxes() As Tax

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Fees() As Fee

        '<remarks/>
        Public TotalFare As TotalFare

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public NegotiatedFare As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NegotiatedFareCode As String
    End Class

    '<remarks/>
    Public Enum PTC_FareBreakdownPricingSource

        '<remarks/>
        Published

        '<remarks/>
        [Private]

        '<remarks/>
        Both
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class FareInfo

        '<remarks/>
        Public DepartureDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DepartureDateSpecified As Boolean

        '<remarks/>
        Public FareReference As String

        '<remarks/>
        Public RuleInfo As RuleInfo

        '<remarks/>
        Public FilingAirline As FilingAirline

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MarketingAirline")> _
        Public MarketingAirline() As MarketingAirline

        '<remarks/>
        Public DepartureAirport As DepartureAirport

        '<remarks/>
        Public ArrivalAirport As ArrivalAirport

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public NegotiatedFare As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NegotiatedFareCode As String
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
        Public AdvResTicketing As AdvResTicketingRS
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class LengthOfStayRules

        '<remarks/>
        Public MinimumStay As MinimumStayRS

        '<remarks/>
        Public MaximumStay As MaximumStayRS

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StayRestrictionsInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public StayRestrictionsIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class MinimumStayRS

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ReturnTimeOfDay As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ReturnTimeOfDaySpecified As Boolean

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
    Public Class MaximumStayRS

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ReturnType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ReturnTimeOfDay As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ReturnTimeOfDaySpecified As Boolean

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
        Public MaxStayDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public MaxStayDateSpecified As Boolean
    End Class

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
    Public Class CabinType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Cabin As CabinTypeCabin
    End Class

    '<remarks/>
    Public Enum CabinTypeCabin

        '<remarks/>
        First

        '<remarks/>
        Business

        '<remarks/>
        Economy

        Premium
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ChargesRules

        '<remarks/>
        Public VoluntaryChanges As VoluntaryChangesRS
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class VoluntaryChangesRS

        '<remarks/>
        Public Penalty As PenaltyRS

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VolChangeInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public VolChangeIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PenaltyRS

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
    Public Enum AirItineraryPricingInfoPricingSource

        '<remarks/>
        Published

        '<remarks/>
        [Private]

        '<remarks/>
        Both
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class [Error]

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShortText As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")> _
        Public DocURL As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Status As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Tag As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RecordID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NodeList As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Errors

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Error")> _
        Public [Error]() As [Error]
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class FareBasisCodes

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FareBasisCode")> _
        Public FareBasisCode() As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class FareInfos

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FareInfo")> _
        Public FareInfo() As FareInfo
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Fees

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Fee")> _
        Public Fee() As Fee
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Notes

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class OTA_AirLowFareSearchPlusRS

        '<remarks/>
        Public Success As Success

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Warnings() As Warning

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public PricedItineraries() As PricedItinerary

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Errors() As [Error]

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EchoToken As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TimeStamp As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TimeStampSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(OTA_AirLowFareSearchPlusRSTarget.Production)> _
        Public Target As OTA_AirLowFareSearchPlusRSTarget = OTA_AirLowFareSearchPlusRSTarget.Production

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Version As Double

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransactionIdentifier As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SequenceNmbr As String

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SequenceNmbrSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransactionStatusCode As OTA_AirLowFareSearchPlusRSTransactionStatusCode

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransactionStatusCodeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PrimaryLangID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AltLangID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Success
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Warning

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShortText As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")> _
        Public DocURL As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Status As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Tag As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RecordID As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PricedItinerary

        '<remarks/>
        Public AirItinerary As AirItinerary

        '<remarks/>
        Public AirItineraryPricingInfo As AirItineraryPricingInfo

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Notes")> _
        Public Notes() As Notes

        '<remarks/>
        Public TicketingInfo As TicketingInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SequenceNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Provider As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TicketingInfo

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TicketAdvisory")> _
        Public TicketAdvisory() As TicketAdvisory

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TicketTimeLimit As String

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TicketTimeLimitSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TicketType As TicketingInfoTicketType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TicketTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public eTicketNumber As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TicketAdvisory

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum TicketingInfoTicketType

        '<remarks/>
        eTicket

        '<remarks/>
        Paper
    End Enum

    '<remarks/>
    Public Enum OTA_AirLowFareSearchPlusRSTarget

        '<remarks/>
        Test

        '<remarks/>
        Production
    End Enum

    '<remarks/>
    Public Enum OTA_AirLowFareSearchPlusRSTransactionStatusCode

        '<remarks/>
        Start

        '<remarks/>
        [End]

        '<remarks/>
        Rollback

        '<remarks/>
        InSeries
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class OriginDestinationOption

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FlightSegment")> _
        Public FlightSegment() As FlightSegment

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SectorSequence As String
    End Class

    '<remarks/>
    '<System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    'Public Class OriginDestinationOptions

    '    '<remarks/>
    '    <System.Xml.Serialization.XmlArrayItemAttribute(GetType(FlightSegment), IsNullable:=False)> _
    '    Public OriginDestinationOption()() As FlightSegment
    'End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PTC_FareBreakdowns

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PTC_FareBreakdown")> _
        Public PTC_FareBreakdown() As PTC_FareBreakdown
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PricedItineraries

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PricedItinerary")> _
        Public PricedItinerary() As PricedItinerary
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Taxes

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Tax")> _
        Public Tax() As Tax
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Warnings

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Warning")> _
        Public Warning() As Warning
    End Class
End Namespace

