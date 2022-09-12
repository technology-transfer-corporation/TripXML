﻿'------------------------------------------------------------------------------
' <autogenerated>
'     This code was generated by a tool.
'     Runtime Version: 1.1.4322.2407
'
'     Changes to this file may cause incorrect behavior and will be lost if 
'     the code is regenerated.
' </autogenerated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

'
'This source code was auto-generated by xsd, Version=1.1.4322.2407.
'
Namespace wsTravelTalk.wmCarRulesIn
    
    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class OTA_VehRateRuleRQ

        '<remarks/>
        Public POS As POS_Type

        '<remarks/>
        Public Reference As UniqueID_Type

        '<remarks/>
        Public RentalInfo As OTA_VehRateRuleRQRentalInfo

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
         System.ComponentModel.DefaultValueAttribute(OTA_VehRateRuleRQTarget.Production)> _
        Public Target As OTA_VehRateRuleRQTarget = OTA_VehRateRuleRQTarget.Production

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Version As Decimal

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransactionIdentifier As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")> _
        Public SequenceNmbr As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransactionStatusCode As OTA_VehRateRuleRQTransactionStatusCode

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransactionStatusCodeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RetransmissionIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public RetransmissionIndicatorSpecified As Boolean

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
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class POS_Type

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Source")> _
        Public Source() As SourceType

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class SourceType

        '<remarks/>
        Public RequestorID As SourceTypeRequestorID

        '<remarks/>
        Public Position As SourceTypePosition

        '<remarks/>
        Public BookingChannel As SourceTypeBookingChannel

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

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TerminalID As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class SourceTypeRequestorID
        Inherits UniqueID_Type

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public MessagePassword As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(SourceTypeRequestorID))> _
    Public Class UniqueID_Type

        '<remarks/>
        Public CompanyName As CompanyNameType

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
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CompanyNameType

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
        Public Division As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Department As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleTourInfoType

        '<remarks/>
        Public TourOperator As CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TourNumber As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleArrivalDetailsType

        '<remarks/>
        Public ArrivalLocation As LocationType

        '<remarks/>
        Public MarketingCompany As CompanyNameType

        '<remarks/>
        Public OperatingCompany As CompanyNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransportationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Number As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ArrivalDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ArrivalDateTimeSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(VehicleRentalCoreTypeReturnLocation)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(VehicleRentalCoreTypePickUpLocation))> _
    Public Class LocationType

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
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleRentalCoreTypeReturnLocation
        Inherits LocationType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExtendedLocationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CounterLocation As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleRentalCoreTypePickUpLocation
        Inherits LocationType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ExtendedLocationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CounterLocation As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class OffLocationServiceTypeTelephone
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class PersonNameType

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
        Public ShareSynchInd As AddressTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As AddressTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public NameType As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum AddressTypeShareSynchInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum AddressTypeShareMarketInd

        '<remarks/>
        Yes

        '<remarks/>
        No

        '<remarks/>
        Inherit
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class CountryNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class StateProvType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StateCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AddressTypeBldgRoom

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public BldgNameIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public BldgNameIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(AddressTypeStreetNmbr))> _
    Public Class StreetNmbrType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PO_Box As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class AddressTypeStreetNmbr
        Inherits StreetNmbrType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StreetNmbrSuffix As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StreetDirection As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RuralRouteNmbr As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(OffLocationServiceCoreTypeAddress))> _
    Public Class AddressType

        '<remarks/>
        Public StreetNmbr As AddressTypeStreetNmbr

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("BldgRoom")> _
        Public BldgRoom() As AddressTypeBldgRoom

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
        Public StateProv As StateProvType

        '<remarks/>
        Public CountryName As CountryNameType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(), _
         System.ComponentModel.DefaultValueAttribute(False)> _
        Public FormattedInd As Boolean = False

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareSynchInd As AddressTypeShareSynchInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareSynchIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ShareMarketInd As AddressTypeShareMarketInd

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ShareMarketIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class OffLocationServiceCoreTypeAddress
        Inherits AddressType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SiteID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SiteName As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(OffLocationServiceType))> _
    Public Class OffLocationServiceCoreType

        '<remarks/>
        Public Address As OffLocationServiceCoreTypeAddress

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Type As OffLocationServiceID_Type
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum OffLocationServiceID_Type

        '<remarks/>
        CustPickUp

        '<remarks/>
        VehDelivery

        '<remarks/>
        CustDropOff

        '<remarks/>
        VehCollection

        '<remarks/>
        Exchange
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class OffLocationServiceType
        Inherits OffLocationServiceCoreType

        '<remarks/>
        Public PersonName As PersonNameType

        '<remarks/>
        Public Telephone As OffLocationServiceTypeTelephone

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public SpecInstructions As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class OTA_VehRateRuleRQRentalInfoRateQualifier

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TravelPurpose As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RateCategory As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CorpDiscountNmbr As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RateQualifier As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")> _
        Public RatePeriod As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public GuaranteedInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public GuaranteedIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RateAuthorizationCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VendorRateID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public RateModifiedInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public RateModifiedIndSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class OTA_VehRateRuleRQRentalInfoCustLoyalty

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
        Public RPH As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VendorCode() As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class OTA_VehRateRuleRQRentalInfoSpecialEquipPref

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Action As ActionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ActionSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum ActionType

        '<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("Add-Update")> _
        AddUpdate

        '<remarks/>
        Cancel

        '<remarks/>
        Delete

        '<remarks/>
        Add

        '<remarks/>
        Replace
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleCoreTypeVehClass

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Size As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleCoreTypeVehType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VehicleCategory As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DoorCount As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(VehiclePrefType))> _
    Public Class VehicleCoreType

        '<remarks/>
        Public VehType As VehicleCoreTypeVehType

        '<remarks/>
        Public VehClass As VehicleCoreTypeVehClass

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirConditionInd As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AirConditionIndSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransmissionType As VehicleTransmissionType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransmissionTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public FuelType As VehicleCoreTypeFuelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public FuelTypeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DriveType As VehicleCoreTypeDriveType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DriveTypeSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum VehicleTransmissionType

        '<remarks/>
        Automatic

        '<remarks/>
        Manual
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum VehicleCoreTypeFuelType

        '<remarks/>
        Unspecified

        '<remarks/>
        Diesel

        '<remarks/>
        Hybrid

        '<remarks/>
        Electric

        '<remarks/>
        LPG_CompressedGas

        '<remarks/>
        Hydrogen

        '<remarks/>
        MultiFuel

        '<remarks/>
        Petrol

        '<remarks/>
        Ethanol
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum VehicleCoreTypeDriveType

        '<remarks/>
        AWD

        '<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("4WD")> _
        Item4WD

        '<remarks/>
        Unspecified
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehiclePrefType
        Inherits VehicleCoreType

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TypePref As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TypePrefSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public ClassPref As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public ClassPrefSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AirConditionPref As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public AirConditionPrefSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public TransmissionPref As PreferLevelType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public TransmissionPrefSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public VendorCarType As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")> _
        Public VehicleQty As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public CodeContext As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum PreferLevelType

        '<remarks/>
        Only

        '<remarks/>
        Unacceptable

        '<remarks/>
        Preferred

        '<remarks/>
        Required

        '<remarks/>
        NoPreference
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class VehicleRentalCoreType

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PickUpLocation")> _
        Public PickUpLocation() As VehicleRentalCoreTypePickUpLocation

        '<remarks/>
        Public ReturnLocation As VehicleRentalCoreTypeReturnLocation

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

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StartChargesDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public StartChargesDateTimeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public StopChargesDateTime As Date

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public StopChargesDateTimeSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public OneWayIndicator As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public OneWayIndicatorSpecified As Boolean

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")> _
        Public MultiIslandRentalDays As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")> _
        Public Quantity As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public DistUnitName As DistanceUnitNameType

        '<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public DistUnitNameSpecified As Boolean
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum DistanceUnitNameType

        '<remarks/>
        Mile

        '<remarks/>
        Km

        '<remarks/>
        Block
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class OTA_VehRateRuleRQRentalInfo

        '<remarks/>
        Public VehRentalCore As VehicleRentalCoreType

        '<remarks/>
        Public VehicleInfo As VehiclePrefType

        '<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("SpecialEquipPref", IsNullable:=False)> _
        Public SpecialEquipPrefs() As OTA_VehRateRuleRQRentalInfoSpecialEquipPref

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CustLoyalty")> _
        Public CustLoyalty() As OTA_VehRateRuleRQRentalInfoCustLoyalty

        '<remarks/>
        Public RateQualifier As OTA_VehRateRuleRQRentalInfoRateQualifier

        '<remarks/>
        Public OffLocService As OffLocationServiceType

        '<remarks/>
        Public ArrivalDetails As VehicleArrivalDetailsType

        '<remarks/>
        Public TourInfo As VehicleTourInfoType

        '<remarks/>
        Public CustomerID As UniqueID_Type

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TPA_Extensions

        '<remarks/>
        Public Provider As Provider
    End Class
    
    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Provider

        '<remarks/>
        Public Name As Name

        '<remarks/>
        Public System As String

        '<remarks/>
        Public Userid As String

        '<remarks/>
        Public Password As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute(), _
     System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Name

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PseudoCityCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class
    
    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class SourceTypeBookingChannel

        '<remarks/>
        Public CompanyName As CompanyNameType

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
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Class SourceTypePosition

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Latitude As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Longitude As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Altitude As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public AltitudeUnitOfMeasureCode As String
    End Class
    
    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum OTA_VehRateRuleRQTarget

        '<remarks/>
        Test

        '<remarks/>
        Production
    End Enum

    '<remarks/>
    <System.Xml.Serialization.XmlTypeAttribute()> _
    Public Enum OTA_VehRateRuleRQTransactionStatusCode

        '<remarks/>
        Start

        '<remarks/>
        [End]

        '<remarks/>
        Rollback

        '<remarks/>
        InSeries

        '<remarks/>
        Continuation

        '<remarks/>
        Subsequent
    End Enum
End Namespace
