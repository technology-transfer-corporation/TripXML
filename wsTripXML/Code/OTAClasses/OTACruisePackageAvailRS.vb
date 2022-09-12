﻿'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'     Runtime Version: 1.1.4322.573
'
'     Changes to this file may cause incorrect behavior and will be lost if 
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System.Xml.Serialization
Imports wsTripXML.wsTravelTalk.wmCruisePackageAvail

'
'This source code was auto-generated by xsd, Version=1.1.4322.573.
'
Namespace wsTravelTalk.wmCruisePackageAvailOut

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CabinConfiguration

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BedConfigurationCode As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CabinOption

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CabinConfiguration")> _
        Public CabinConfiguraton() As CabinConfiguration

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MeasurementInfo")> _
        Public MeasurementInfo() As MeasurementInfo

        '<remarks/>
        Public Remark As Remark

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CabinFilter")> _
        Public CabinFilter() As CabinFilter

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Status As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public CategoryLocation As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public ShipSide As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public PositionInShip As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MaxOccupancy As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DeckName As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CabinNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BedType As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class MeasurementInfo

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BalconyArea As Decimal

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public BalconyAreaSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CabinArea As Decimal

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public CabinAreaSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public UnitOfMeasure As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Remark

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class CabinOptions

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CabinOption")> _
        Public CabinOption() As CabinOption
    End Class

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
        <System.Xml.Serialization.XmlAttributeAttribute()> _
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
    Public Class Information

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Text")> _
        Public [Text]() As [Text]

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Image")> _
        Public Image() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("URL")> _
        Public URL() As String

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ListItem")> _
        Public ListItem() As ListItem

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Name As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ParagraphNumber As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CreateDateTime As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CreatorID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public LastModifyDateTime As String

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
        Public Formatted As String

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
        Public Formatted As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute("ListItem")> _
        Public ListItem1 As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class SailingInfoRS

        '<remarks/>
        Public SelectedSailing As SelectedSailing

        '<remarks/>
        Public InclusivePackageOption As InclusivePackageOption

        '<remarks/>
        Public Currency As Currency

        '<remarks/>
        Public SelectedCategory As SelectedCategory
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class OTA_CruisePackageAvailRS

        '<remarks/>
        Public Success As Success

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Warnings() As Warning

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public Errors() As [Error]

        '<remarks/>
        Public SailingInfo As SailingInfoRS

        '<remarks/>
        Public SelectedFare As SelectedFare

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public CabinOptions() As CabinOption

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Information")> _
        Public Information() As Information

        '<remarks/>
        Public PackageOptions As PackageOptions

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public EchoToken As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TimeStamp As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public Target As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Version As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransactionIdentifier As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SequenceNmbr As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public TransactionStatusCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PrimaryLangID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AltLangID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PackageOptions

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PackageOption")> _
        Public PackageOption() As PackageOptionRS
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class PackageOptionRS

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute(IsNullable:=False)> _
        Public PackagePrices() As PackagePrice

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PackageType As PackageOptionPackageType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public PackageTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PackageCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Description As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")> _
        Public StartDate As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public StartDateSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Duration As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ListOfPackageQualifier As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute()> _
    Public Class PackagePrice

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PriceTypeCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Amount As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CodeDetail As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AgeQualifyingCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BreakdownType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Status As String
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
        <System.Xml.Serialization.XmlAttributeAttribute()> _
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
    Public Class Warnings

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Warning")> _
        Public Warning() As Warning
    End Class

End Namespace