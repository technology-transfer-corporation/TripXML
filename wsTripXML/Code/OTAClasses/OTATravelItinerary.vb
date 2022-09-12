Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmTravelItinerary

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class StreetNmbr

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PO_Box As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class StateProv

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StateCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CountryName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum AddressShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum AddressShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum DirectBillShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum DirectBillShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class BankAcct

        '<remarks/>
        Public BankAcctName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As BankAcctShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As BankAcctShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BankID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AcctType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BankAcctNumber As String
    End Class

    '<remarks/>
    Public Enum BankAcctShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum BankAcctShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum PaymentCardShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum PaymentCardShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum PaymentCardCardCode

        '<remarks/> Carte Aurore
        AU

        '<remarks/> Amex
        AX

        '<remarks/>
        BC

        '<remarks/>
        BL

        '<remarks/> Carte Blanche
        CB

        '<remarks/> Cofinoga
        CG

        '<remarks/> Connect
        CN

        '<remarks/> Choice
        CX

        '<remarks/> Diners (DC)
        DN

        '<remarks/>
        DK

        '<remarks/> Discover
        DS

        '<remarks/>
        EC

        '<remarks/> Lufthansa GK Card
        GK

        '<remarks/> JCB
        JC

        '<remarks/> Mastercard (CA)
        MC

        '<remarks/> Mastercard Debit
        MD

        '<remarks/> Mastercard Maestro
        MO

        '<remarks/> Mastercard Prepaid
        MP

        '<remarks/> Solo
        SO

        '<remarks/> Switch
        SW

        '<remarks/> Torch Club
        TC

        '<remarks/> UATP
        TP

        '<remarks/> Visa
        VI

        '<remarks/> Visa Debit
        VD

        '<remarks/> Visa Electron
        VE

        '<remarks/> Visa Delta
        VT

        '<remarks/> Access
        XS


    End Enum

    '<remarks/>
    Public Enum AcceptedPaymentShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum AcceptedPaymentShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PersonName

        '<remarks/>
        Public NamePrefix As String

        '<remarks/>
        Public GivenName As String

        '<remarks/>
        Public MiddleName As String

        '<remarks/>
        Public SurnamePrefix As String

        '<remarks/>
        Public Surname As String

        '<remarks/>
        Public NameSuffix As String

        '<remarks/>
        Public NameTitle As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As PersonNameShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As PersonNameShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NameType As String
    End Class

    '<remarks/>
    Public Enum PersonNameShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum PersonNameShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum TelephoneShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum TelephoneShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Email

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As EmailShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As EmailShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public DefaultInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EmailType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum EmailShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum EmailShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum DocumentShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum DocumentShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum DocumentGender

        '<remarks/>
        Male

        '<remarks/>
        Female

        '<remarks/>
        Unknown
    End Enum

    '<remarks/>
    Public Enum CustLoyaltyShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum CustLoyaltyShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum CustLoyaltySingleVendorInd

        '<remarks/>
        SingleVndr

        '<remarks/>
        Alliance
    End Enum

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

    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class StopInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LocationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute("IATA")> _
        Public CodeContext As String = "IATA"

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ArrivalDateTime As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DepartureDateTime As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
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
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Terminal As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Gate As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)>
    Public Class MarriageGrp

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
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
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Terminal As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Gate As String

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
        Public FlightNumber As OperatingAirlineFlightNumber

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FlightNumberSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum OperatingAirlineFlightNumber

        '<remarks/>
        OPEN

        '<remarks/>
        ARNK
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
    Public Class Amenity

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(AmenityPreferLevel.Preferred)> _
        Public PreferLevel As AmenityPreferLevel = AmenityPreferLevel.Preferred

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RoomAmenity As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Quantity As Integer

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public QuantitySpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum AmenityPreferLevel

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred
    End Enum

    '<remarks/>
    Public Enum StartDateWindowDOW

        '<remarks/>
        Mon

        '<remarks/>
        Tue

        '<remarks/>
        Wed

        '<remarks/>
        Thu

        '<remarks/>
        Fri

        '<remarks/>
        Sat

        '<remarks/>
        Sun
    End Enum

    '<remarks/>
    Public Enum EndDateWindowDOW

        '<remarks/>
        Mon

        '<remarks/>
        Tue

        '<remarks/>
        Wed

        '<remarks/>
        Thu

        '<remarks/>
        Fri

        '<remarks/>
        Sat

        '<remarks/>
        Sun
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class FareBasisCodes

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FareBasisCode")> _
        Public FareBasisCode() As String
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
    Public Class FlightRefNumber

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    Public Enum GuaranteeAcceptedShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum GuaranteeAcceptedShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum GuaranteeRetributionType

        '<remarks/>
        ResAutoCancelled

        '<remarks/>
        ResNotGuaranteed
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class UniqueID

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
    Public Class SeatRequest

        '<remarks/>
        Public DepartureAirport As DepartureAirport

        '<remarks/>
        Public ArrivalAirport As ArrivalAirport

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SeatNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SeatPreference As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SmokingAllowed As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Status As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelerRefNumberRPHList As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FlightRefNumberRPHList As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Chargeable As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SeatRequests

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SeatRequest")> _
        Public SeatRequest() As SeatRequest
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SpecialServiceRequests

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SpecialServiceRequest")> _
        Public SpecialServiceRequest() As SpecialServiceRequest
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SpecialServiceRequest

        '<remarks/>
        Public Airline As Airline

        '<remarks/>
        Public [Text] As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SSRCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelerRefNumberRPHList As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FlightRefNumberRPHList As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class UniqueRemark

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RemarkType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class UniqueRemarks

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("UniqueRemark")> _
        Public UniqueRemark() As UniqueRemark
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PickUpLocation

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
    Public Class ReturnLocation

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
    Public Class VehRentalCore

        '<remarks/>
        Public PickUpLocation As PickUpLocation

        '<remarks/>
        Public ReturnLocation As ReturnLocation

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PickUpDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PickUpDateTimeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ReturnDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ReturnDateTimeSpecified As Boolean
    End Class

    '<remarks/>
    Public Enum RateQualifierRatePeriod

        '<remarks/>
        Hourly

        '<remarks/>
        Daily

        '<remarks/>
        Weekly

        '<remarks/>
        Monthly

        '<remarks/>
        WeekendDay

        '<remarks/>
        Other
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TravelerRefNumber

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class OtherServiceInformation

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TravelerRefNumber")> _
        Public TravelerRefNumber() As TravelerRefNumber

        '<remarks/>
        Public Airline As Airline

        '<remarks/>
        Public [Text] As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class OtherServiceInformations

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("OtherServiceInformation")> _
        Public OtherServiceInformation() As OtherServiceInformation
    End Class

End Namespace

