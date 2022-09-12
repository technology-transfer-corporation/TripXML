Option Strict Off
Option Explicit On 

Imports System.Xml.Serialization

Namespace wsTravelTalk.wmInsuranceQuote

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Activities

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Activity")> _
        Public Activity() As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class AdditionalPersonNames

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AdditionalPersonName")> _
        Public AdditionalPersonName() As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class StreetNmbr

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PO_Box As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StreetNmbrSuffix As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StreetDirection As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RuralRouteNmbr As String

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
    Public Class URL

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CitizenCountryName

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String
    End Class


    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CoverageRequirement

        '<remarks/>
        Public Deductible As Deductible

        '<remarks/>
        Public PolicyLimit As PolicyLimit

        '<remarks/>
        Public IndividualLimit As IndividualLimit

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CoverageLevel As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CoverageType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public UnlimitedCoverage As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Covered As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EffectiveDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDate As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Deductible

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PolicyLimit

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class IndividualLimit

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CoverageRequirements

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CoverageRequirement")> _
        Public CoverageRequirement() As CoverageRequirement
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CoveredLuggage

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LuggageItem")> _
        Public LuggageItem() As LuggageItem
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class LuggageItem

        '<remarks/>
        Public LuggageDescription As LuggageDescription

        '<remarks/>
        Public ItemDeclaredValue As ItemDeclaredValue

        '<remarks/>
        Public LuggagePremium As LuggagePremium

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LuggageType As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class LuggageDescription

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class ItemDeclaredValue

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class LuggagePremium

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CoveredTrip

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Destinations() As Destination

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Activity", IsNullable:=False)> _
        Public Activities() As String

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Operators() As [Operator]

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Start As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Duration As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public [End] As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DepositDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FinalPayDate As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Destination

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
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FormattedInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ArrivalDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DepartureDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AreaID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class [Operator]

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
    Public Class CoveredTrips

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CoveredTrip")> _
        Public CoveredTrip() As CoveredTrip
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Destinations

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Destination")> _
        Public Destination() As Destination
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class DeliveryPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public PreferLevel As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DistribType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class DocHolderFormattedName

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
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NameType As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Document

        '<remarks/>
        Public DocHolderName As String

        '<remarks/>
        Public DocHolderFormattedName As DocHolderFormattedName

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("DocLimitations")> _
        Public DocLimitations() As String

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("AdditionalPersonName", IsNullable:=False)> _
        Public AdditionalPersonNames() As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocIssueAuthority As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocIssueLocation As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public Gender As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BirthDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EffectiveDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocIssueStateProv As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DocIssueCountry As String
    End Class


    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Email

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DefaultInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EmailType As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class EmployeeInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EmployeeId As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EmployeeLevel As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EmployeeTitle As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EmployeeStatus As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class FlightAccidentAmount

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PreexistingCondition

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DiagnosisDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LastTreatmentDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class IndCoverageReqs

        '<remarks/>
        Public IndTripCost As IndTripCost

        '<remarks/>
        Public FlightAccidentAmount As FlightAccidentAmount

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public CoveredLuggage() As LuggageItem

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public PreexistingConditions() As PreexistingCondition
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class IndTripCost

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class InsCoverageDetail

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public CoverageRequirements() As CoverageRequirement

        '<remarks/>
        Public TotalTripQuantity As TotalTripQuantity

        '<remarks/>
        Public MaximumTripLength As MaximumTripLength

        '<remarks/>
        Public TotalTripCost As TotalTripCost

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public CoveredTrips() As CoveredTrip

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("DeliveryPref")> _
        Public DeliveryPref() As DeliveryPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EffectiveDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExpireDate As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AutoRenew As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TotalTripQuantity

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Quantity As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class MaximumTripLength

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Minimum As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Maximum As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TotalTripCost

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CurrencyCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DecimalPlaces As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Operators

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Operator")> _
        Public [Operator]() As [Operator]
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PreexistingConditions

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PreexistingCondition")> _
        Public PreexistingCondition() As PreexistingCondition
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
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareSynchInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShareMarketInd As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NameType As String
    End Class




End Namespace
