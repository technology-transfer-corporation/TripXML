Option Strict Off
Option Explicit On 

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmHotelSearch

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
    Public Class Address

        '<remarks/>
        Public StreetNmbr As StreetNmbr

        '<remarks/>
        Public BldgRoom As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AddressLine")> _
        Public AddressLine() As String

        '<remarks/>
        Public CityName As String

        '<remarks/>
        Public PostalCode As String

        '<remarks/>
        Public County As String

        '<remarks/>
        Public StateProv As StateProv

        '<remarks/>
        Public CountryName As CountryName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public FormattedInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As AddressShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As AddressShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String
    End Class

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
    Public Class CountryName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

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
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Criteria

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Criterion")> _
        Public Criterion() As Criterion
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Criterion

        '<remarks/>
        Public Position As Position

        '<remarks/>
        Public Address As Address

        '<remarks/>
        Public Telephone As Telephone

        '<remarks/>
        Public RefPoint As String

        '<remarks/>
        Public CodeRef As CodeRef

        '<remarks/>
        Public HotelRef As HotelRef

        '<remarks/>
        Public Radius As Radius

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("HotelAmenity")> _
        Public HotelAmenity() As HotelAmenity

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Award")> _
        Public Award() As Award

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExactMatch As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ExactMatchSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ImportanceType As CriterionImportanceType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ImportanceTypeSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Telephone

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As TelephoneShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As TelephoneShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

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

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public FormattedInd As Boolean = False
    End Class

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
    Public Class HotelRef

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ChainCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BrandCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public HotelCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public HotelCityCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public HotelName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public HotelCodeContext As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ChainName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BrandName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AreaID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Radius

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Distance As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DistanceMeasure As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Direction As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class HotelAmenity

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String
    End Class

    '<remarks/>
    Public Enum CriterionImportanceType

        '<remarks/>
        Mandatory

        '<remarks/>
        High

        '<remarks/>
        Medium

        '<remarks/>
        Low
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Award

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Provider As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Rating As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CodeRef

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

End Namespace
