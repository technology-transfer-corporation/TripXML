Option Strict Off
Option Explicit On 

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmAirSeatMap

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AirTraveler

        '<remarks/>
        Public PersonName As PersonName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CustLoyalty")> _
        Public CustLoyalty() As CustLoyalty

        '<remarks/>
        Public PassengerTypeQuantity As PassengerTypeQuantity

        '<remarks/>
        Public TravelerRefNumber As TravelerRefNumber

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Gender As AirTravelerGender

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public GenderSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As AirTravelerShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As AirTravelerShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public BirthDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public BirthDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PassengerTypeCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AccompaniedByInfant As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AccompaniedByInfantSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PersonName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("NamePrefix")> _
        Public NamePrefix() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("GivenName")> _
        Public GivenName() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MiddleName")> _
        Public MiddleName() As String

        '<remarks/>
        Public SurnamePrefix As String

        '<remarks/>
        Public Surname As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("NameSuffix")> _
        Public NameSuffix() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("NameTitle")> _
        Public NameTitle() As String

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
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TravelerRefNumber

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
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
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CustLoyalty

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As CustLoyaltyShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As CustLoyaltyShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ProgramID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MembershipID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelSector As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LoyalLevel As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SingleVendorInd As CustLoyaltySingleVendorInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SingleVendorIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public SignupDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public SignupDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public EffectiveDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public EffectiveDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public ExpireDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExpireDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RPH As String
    End Class

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
    Public Enum AirTravelerGender

        '<remarks/>
        Male

        '<remarks/>
        Female

        '<remarks/>
        Unknown
    End Enum

    '<remarks/>
    Public Enum AirTravelerShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    Public Enum AirTravelerShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AirTravelers

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AirTraveler")> _
        Public AirTraveler() As AirTraveler
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
    Public Class FlightSegmentInfo

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
    Public Class BookingReferenceID

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
    Public Enum CabinClassCabinType

        '<remarks/>
        First

        '<remarks/>
        Business

        '<remarks/>
        Economy
    End Enum

End Namespace
