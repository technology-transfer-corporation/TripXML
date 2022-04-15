Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

Namespace wsTravelTalk.VirtualCreditCard
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/Types_v3")>
    Partial Public Class ErrorType
        Inherits FreeTextType

        Private typeField As String

        Private shortTextField As String

        Private codeField As String

        Private docURLField As String

        Private statusField As String

        Private tagField As String

        Private recordIDField As String

        Private nodeListField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShortText() As String
            Get
                Return Me.shortTextField
            End Get
            Set
                Me.shortTextField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")>
        Public Property DocURL() As String
            Get
                Return Me.docURLField
            End Get
            Set
                Me.docURLField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Status() As String
            Get
                Return Me.statusField
            End Get
            Set
                Me.statusField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Tag() As String
            Get
                Return Me.tagField
            End Get
            Set
                Me.tagField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RecordID() As String
            Get
                Return Me.recordIDField
            End Get
            Set
                Me.recordIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property NodeList() As String
            Get
                Return Me.nodeListField
            End Get
            Set
                Me.nodeListField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class VirtualCardDetails
    
        '''<remarks/>
        Public Property Card As CardType
            
        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Reference", IsNullable:=false)>  _
        Public Property References As VirtualCardDetailsTypeReference()
    
        '''<remarks/>
        Public Property Provider As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Value", IsNullable:=false)>  _
        Public Property Values As VirtualCreditCard.VirtualCardAmountType()
        'Public Property Values As FundsTransferType()
            
        '''<remarks/>
        Public Property Account As VirtualCardAccountType
            
        '''<remarks/>
        Public Property Limitations As CardRestrictionsType
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property LastUpdatedTime As Date
            
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property LastUpdatedTimeSpecified As Boolean
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property CreationTime As Date
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property CreationUser As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property CreationOffice As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property CardStatus As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class VirtualCardDetailsTypeReference
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Type() As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value() As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class VirtualCardAccountType

        Private typeField As String

        Private userIdField As String

        Private agencyIdField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property UserId() As String
            Get
                Return Me.userIdField
            End Get
            Set
                Me.userIdField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AgencyId() As String
            Get
                Return Me.agencyIdField
            End Get
            Set
                Me.agencyIdField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class VirtualCardAmountType
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")>
        Public Property Amount As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")>
        Public Property DecimalPlaces As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode As String

        Public Function GetFormatedAmount() AS String
            Try
                If String.IsNullOrEmpty(Amount)
                    Return string.Empty
                End If
                dim amt as Decimal = Convert.ToDecimal(Amount)
                dim decPlaces  as Decimal = Convert.ToDecimal(DecimalPlaces)
            

                Dim amountRet As Decimal = Math.Round(amt / Math.Pow(10, decPlaces), 2)
                return amountRet.ToString("#0.00")
            Catch ex As Exception
                return string.Empty
            End Try
        End Function 

            
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class CardRestrictionsType

        '''<remarks/>
        Public Property AllowedTransactions As CardRestrictionsTypeAllowedTransactions

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("CurrencyCode", IsNullable:=False)>
        Public Property CurrencyList As String()

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Merchant")>
        Public Property Merchant As CardRestrictionsTypeMerchant()

        '''<remarks/>
        Public Property ValidityPeriod As CardRestrictionsTypeValidityPeriod
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class CardRestrictionsTypeAllowedTransactions

        Private maximumField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property Maximum() As String
            Get
                Return Me.maximumField
            End Get
            Set
                Me.maximumField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class CardRestrictionsTypeMerchant

        Private typeField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class CardRestrictionsTypeValidityPeriod

        Private startDateField As Date

        Private startDateFieldSpecified As Boolean

        Private endDateField As Date

        Private endDateFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property StartDate() As Date
            Get
                Return Me.startDateField
            End Get
            Set
                Me.startDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StartDateSpecified() As Boolean
            Get
                Return Me.startDateFieldSpecified
            End Get
            Set
                Me.startDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property EndDate() As Date
            Get
                Return Me.endDateField
            End Get
            Set
                Me.endDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EndDateSpecified() As Boolean
            Get
                Return Me.endDateFieldSpecified
            End Get
            Set
                Me.endDateFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlRootAttribute("TPA_Extensions", [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable:=False)>
    Partial Public Class TPA_ExtensionsType

        Private anyField() As System.Xml.XmlElement

        '''<remarks/>
        <System.Xml.Serialization.XmlAnyElementAttribute()>
        Public Property Any() As System.Xml.XmlElement()
            Get
                Return Me.anyField
            End Get
            Set
                Me.anyField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A"),
        System.Xml.Serialization.XmlRootAttribute("VehReservation", [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable:=False)>
    Partial Public Class VehicleReservationType

        Private customerField As CustomerPrimaryAdditionalType

        Private vehSegmentCoreField As VehicleReservationTypeVehSegmentCore

        Private vehSegmentInfoField As VehicleSegmentAdditionalInfoType

        Private createDateTimeField As Date

        Private createDateTimeFieldSpecified As Boolean

        Private creatorIDField As String

        Private lastModifyDateTimeField As Date

        Private lastModifyDateTimeFieldSpecified As Boolean

        Private lastModifierIDField As String

        Private purgeDateField As Date

        Private purgeDateFieldSpecified As Boolean

        Private reservationStatusField As String

        '''<remarks/>
        Public Property Customer() As CustomerPrimaryAdditionalType
            Get
                Return Me.customerField
            End Get
            Set
                Me.customerField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property VehSegmentCore() As VehicleReservationTypeVehSegmentCore
            Get
                Return Me.vehSegmentCoreField
            End Get
            Set
                Me.vehSegmentCoreField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property VehSegmentInfo() As VehicleSegmentAdditionalInfoType
            Get
                Return Me.vehSegmentInfoField
            End Get
            Set
                Me.vehSegmentInfoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CreateDateTime() As Date
            Get
                Return Me.createDateTimeField
            End Get
            Set
                Me.createDateTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property CreateDateTimeSpecified() As Boolean
            Get
                Return Me.createDateTimeFieldSpecified
            End Get
            Set
                Me.createDateTimeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CreatorID() As String
            Get
                Return Me.creatorIDField
            End Get
            Set
                Me.creatorIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property LastModifyDateTime() As Date
            Get
                Return Me.lastModifyDateTimeField
            End Get
            Set
                Me.lastModifyDateTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property LastModifyDateTimeSpecified() As Boolean
            Get
                Return Me.lastModifyDateTimeFieldSpecified
            End Get
            Set
                Me.lastModifyDateTimeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property LastModifierID() As String
            Get
                Return Me.lastModifierIDField
            End Get
            Set
                Me.lastModifierIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property PurgeDate() As Date
            Get
                Return Me.purgeDateField
            End Get
            Set
                Me.purgeDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PurgeDateSpecified() As Boolean
            Get
                Return Me.purgeDateFieldSpecified
            End Get
            Set
                Me.purgeDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ReservationStatus() As String
            Get
                Return Me.reservationStatusField
            End Get
            Set
                Me.reservationStatusField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerPrimaryAdditionalType

        Private primaryField As CustomerPrimaryAdditionalTypePrimary

        Private additionalField() As CustomerPrimaryAdditionalTypeAdditional

        '''<remarks/>
        Public Property Primary() As CustomerPrimaryAdditionalTypePrimary
            Get
                Return Me.primaryField
            End Get
            Set
                Me.primaryField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Additional")>
        Public Property Additional() As CustomerPrimaryAdditionalTypeAdditional()
            Get
                Return Me.additionalField
            End Get
            Set
                Me.additionalField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerPrimaryAdditionalTypePrimary
        Inherits CustomerType

        Private customerIDField As UniqueID_Type

        '''<remarks/>
        Public Property CustomerID() As UniqueID_Type
            Get
                Return Me.customerIDField
            End Get
            Set
                Me.customerIDField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(ReservationID_Type)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class UniqueID_Type

        Private companyNameField As CompanyNameType

        Private uRLField As String

        Private typeField As String

        Private instanceField As String

        Private idField As String

        Private iD_ContextField As String

        '''<remarks/>
        Public Property CompanyName() As CompanyNameType
            Get
                Return Me.companyNameField
            End Get
            Set
                Me.companyNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")>
        Public Property URL() As String
            Get
                Return Me.uRLField
            End Get
            Set
                Me.uRLField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Instance() As String
            Get
                Return Me.instanceField
            End Get
            Set
                Me.instanceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ID() As String
            Get
                Return Me.idField
            End Get
            Set
                Me.idField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ID_Context() As String
            Get
                Return Me.iD_ContextField
            End Get
            Set
                Me.iD_ContextField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>  _
    Public Enum CustomerTypeTelephoneShareSynchInd
    
        '''<remarks/>
        Yes
    
        '''<remarks/>
        No
    
        '''<remarks/>
        Inherit
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>  _
    Public Enum CustomerTypeTelephoneShareMarketInd
    
        '''<remarks/>
        Yes
    
        '''<remarks/>
        No
    
        '''<remarks/>
        Inherit
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>  _
    Public Enum TransferActionType
    
        '''<remarks/>
        Automatic
    
        '''<remarks/>
        Mandatory
    
        '''<remarks/>
        Selectable
    End Enum

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(TravelArrangerType)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(CompanyNamePrefType)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(OperatingAirlineType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CompanyNameType

        Private companyShortNameField As String

        Private travelSectorField As String

        Private codeField As String

        Private codeContextField As String

        Private divisionField As String

        Private departmentField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CompanyShortName() As String
            Get
                Return Me.companyShortNameField
            End Get
            Set
                Me.companyShortNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TravelSector() As String
            Get
                Return Me.travelSectorField
            End Get
            Set
                Me.travelSectorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CodeContext() As String
            Get
                Return Me.codeContextField
            End Get
            Set
                Me.codeContextField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Division() As String
            Get
                Return Me.divisionField
            End Get
            Set
                Me.divisionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Department() As String
            Get
                Return Me.departmentField
            End Get
            Set
                Me.departmentField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>  _
    Public Enum PersonNameTypeShareSynchInd
    
        '''<remarks/>
        Yes
    
        '''<remarks/>
        No
    
        '''<remarks/>
        Inherit
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>  _
    Public Enum PersonNameTypeShareMarketInd
    
        '''<remarks/>
        Yes
    
        '''<remarks/>
        No
    
        '''<remarks/>
        Inherit
    End Enum
   

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class TravelArrangerType
        Inherits CompanyNameType

        Private defaultIndField As Boolean

        Private defaultIndFieldSpecified As Boolean

        Private shareSynchIndField As PersonNameTypeShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As PersonNameTypeShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private travelArrangerType1Field As String

        Private rPHField As String

        Private remarkField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DefaultInd() As Boolean
            Get
                Return Me.defaultIndField
            End Get
            Set
                Me.defaultIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DefaultIndSpecified() As Boolean
            Get
                Return Me.defaultIndFieldSpecified
            End Get
            Set
                Me.defaultIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As PersonNameTypeShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As PersonNameTypeShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute("TravelArrangerType")>
        Public Property TravelArrangerType1() As String
            Get
                Return Me.travelArrangerType1Field
            End Get
            Set
                Me.travelArrangerType1Field = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RPH() As String
            Get
                Return Me.rPHField
            End Get
            Set
                Me.rPHField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Remark() As String
            Get
                Return Me.remarkField
            End Get
            Set
                Me.remarkField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>  _
    Public Enum PreferLevelType
    
        '''<remarks/>
        Only
    
        '''<remarks/>
        Unacceptable
    
        '''<remarks/>
        Preferred
    
        '''<remarks/>
        Required
    
        '''<remarks/>
        NoPreference
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CompanyNamePrefType
        Inherits CompanyNameType

        Private preferLevelField As PreferLevelType

        Private preferLevelFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PreferLevel() As PreferLevelType
            Get
                Return Me.preferLevelField
            End Get
            Set
                Me.preferLevelField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PreferLevelSpecified() As Boolean
            Get
                Return Me.preferLevelFieldSpecified
            End Get
            Set
                Me.preferLevelFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class OperatingAirlineType
        Inherits CompanyNameType

        Private flightNumberField As String

        Private resBookDesigCodeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property FlightNumber() As String
            Get
                Return Me.flightNumberField
            End Get
            Set
                Me.flightNumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ResBookDesigCode() As String
            Get
                Return Me.resBookDesigCodeField
            End Get
            Set
                Me.resBookDesigCodeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class ReservationID_Type
        Inherits UniqueID_Type

        Private statusCodeField As String

        Private lastModifyDateTimeField As Date

        Private lastModifyDateTimeFieldSpecified As Boolean

        Private bookedDateField As String

        Private offerDateField As String

        Private syncDateTimeField As Date

        Private syncDateTimeFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property StatusCode() As String
            Get
                Return Me.statusCodeField
            End Get
            Set
                Me.statusCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property LastModifyDateTime() As Date
            Get
                Return Me.lastModifyDateTimeField
            End Get
            Set
                Me.lastModifyDateTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property LastModifyDateTimeSpecified() As Boolean
            Get
                Return Me.lastModifyDateTimeFieldSpecified
            End Get
            Set
                Me.lastModifyDateTimeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property BookedDate() As String
            Get
                Return Me.bookedDateField
            End Get
            Set
                Me.bookedDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OfferDate() As String
            Get
                Return Me.offerDateField
            End Get
            Set
                Me.offerDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SyncDateTime() As Date
            Get
                Return Me.syncDateTimeField
            End Get
            Set
                Me.syncDateTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SyncDateTimeSpecified() As Boolean
            Get
                Return Me.syncDateTimeFieldSpecified
            End Get
            Set
                Me.syncDateTimeFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>  _
    Public Enum CommissionInfoTypeShareSynchInd
    
        '''<remarks/>
        Yes
    
        '''<remarks/>
        No
    
        '''<remarks/>
        Inherit
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>  _
    Public Enum CommissionInfoTypeShareMarketInd
    
        '''<remarks/>
        Yes
    
        '''<remarks/>
        No
    
        '''<remarks/>
        Inherit
    End Enum

    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>  _
    Partial Public Class EmailType
    
        Private shareSynchIndField As CommissionInfoTypeShareSynchInd
    
        Private shareSynchIndFieldSpecified As Boolean
    
        Private shareMarketIndField As CommissionInfoTypeShareMarketInd
    
        Private shareMarketIndFieldSpecified As Boolean
    
        Private defaultIndField As Boolean
    
        Private defaultIndFieldSpecified As Boolean
    
        Private emailType1Field As String
    
        Private rPHField As String
    
        Private remarkField As String
    
        Private valueField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property ShareSynchInd() As CommissionInfoTypeShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property ShareMarketInd() As CommissionInfoTypeShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property DefaultInd() As Boolean
            Get
                Return Me.defaultIndField
            End Get
            Set
                Me.defaultIndField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property DefaultIndSpecified() As Boolean
            Get
                Return Me.defaultIndFieldSpecified
            End Get
            Set
                Me.defaultIndFieldSpecified = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute("EmailType")>  _
        Public Property EmailType1() As String
            Get
                Return Me.emailType1Field
            End Get
            Set
                Me.emailType1Field = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property RPH() As String
            Get
                Return Me.rPHField
            End Get
            Set
                Me.rPHField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Remark() As String
            Get
                Return Me.remarkField
            End Get
            Set
                Me.remarkField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = value
            End Set
        End Property
    End Class



    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>  _
    Partial Public Class CustomerTypeEmail
        Inherits EmailType
    
        Private transferActionField As TransferActionType
    
        Private transferActionFieldSpecified As Boolean
    
        Private parentCompanyRefField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property TransferAction() As TransferActionType
            Get
                Return Me.transferActionField
            End Get
            Set
                Me.transferActionField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property TransferActionSpecified() As Boolean
            Get
                Return Me.transferActionFieldSpecified
            End Get
            Set
                Me.transferActionFieldSpecified = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property ParentCompanyRef() As String
            Get
                Return Me.parentCompanyRefField
            End Get
            Set
                Me.parentCompanyRefField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(InsuranceCustomerType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerType

        Private personNameField() As PersonNameType

        Private telephoneField() As CustomerTypeTelephone

        Private emailField() As CustomerTypeEmail

        Private addressField() As CustomerTypeAddress

        Private uRLField() As CustomerTypeURL

        Private citizenCountryNameField() As CustomerTypeCitizenCountryName

        Private physChallNameField() As CustomerTypePhysChallName

        Private petInfoField() As String

        Private paymentFormField() As CustomerTypePaymentForm

        Private relatedTravelerField() As RelatedTravelerType

        Private contactPersonField() As ContactPersonType

        Private documentField() As DocumentType

        Private custLoyaltyField() As CustomerTypeCustLoyalty

        Private employeeInfoField() As EmployeeInfoType

        Private employerInfoField As CompanyNameType

        Private additionalLanguageField() As CustomerTypeAdditionalLanguage

        Private tPA_ExtensionsField As TPA_ExtensionsType

        Private genderField As DocumentTypeGender

        Private genderFieldSpecified As Boolean

        Private deceasedField As Boolean

        Private deceasedFieldSpecified As Boolean

        Private lockoutTypeField As String

        Private birthDateField As Date

        Private birthDateFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        Private vIP_IndicatorField As Boolean

        Private vIP_IndicatorFieldSpecified As Boolean

        Private textField As String

        Private languageField As String

        Private customerValueField As String

        Private maritalStatusField As CustomerTypeMaritalStatus

        Private maritalStatusFieldSpecified As Boolean

        Private previouslyMarriedIndicatorField As Boolean

        Private previouslyMarriedIndicatorFieldSpecified As Boolean

        Private childQuantityField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PersonName")>
        Public Property PersonName() As PersonNameType()
            Get
                Return Me.personNameField
            End Get
            Set
                Me.personNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Telephone")>
        Public Property Telephone() As CustomerTypeTelephone()
            Get
                Return Me.telephoneField
            End Get
            Set
                Me.telephoneField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Email")>
        Public Property Email() As CustomerTypeEmail()
            Get
                Return Me.emailField
            End Get
            Set
                Me.emailField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Address")>
        Public Property Address() As CustomerTypeAddress()
            Get
                Return Me.addressField
            End Get
            Set
                Me.addressField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("URL")>
        Public Property URL() As CustomerTypeURL()
            Get
                Return Me.uRLField
            End Get
            Set
                Me.uRLField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CitizenCountryName")>
        Public Property CitizenCountryName() As CustomerTypeCitizenCountryName()
            Get
                Return Me.citizenCountryNameField
            End Get
            Set
                Me.citizenCountryNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PhysChallName")>
        Public Property PhysChallName() As CustomerTypePhysChallName()
            Get
                Return Me.physChallNameField
            End Get
            Set
                Me.physChallNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PetInfo")>
        Public Property PetInfo() As String()
            Get
                Return Me.petInfoField
            End Get
            Set
                Me.petInfoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentForm")>
        Public Property PaymentForm() As CustomerTypePaymentForm()
            Get
                Return Me.paymentFormField
            End Get
            Set
                Me.paymentFormField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RelatedTraveler")>
        Public Property RelatedTraveler() As RelatedTravelerType()
            Get
                Return Me.relatedTravelerField
            End Get
            Set
                Me.relatedTravelerField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ContactPerson")>
        Public Property ContactPerson() As ContactPersonType()
            Get
                Return Me.contactPersonField
            End Get
            Set
                Me.contactPersonField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Document")>
        Public Property Document() As DocumentType()
            Get
                Return Me.documentField
            End Get
            Set
                Me.documentField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CustLoyalty")>
        Public Property CustLoyalty() As CustomerTypeCustLoyalty()
            Get
                Return Me.custLoyaltyField
            End Get
            Set
                Me.custLoyaltyField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("EmployeeInfo")>
        Public Property EmployeeInfo() As EmployeeInfoType()
            Get
                Return Me.employeeInfoField
            End Get
            Set
                Me.employeeInfoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property EmployerInfo() As CompanyNameType
            Get
                Return Me.employerInfoField
            End Get
            Set
                Me.employerInfoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AdditionalLanguage")>
        Public Property AdditionalLanguage() As CustomerTypeAdditionalLanguage()
            Get
                Return Me.additionalLanguageField
            End Get
            Set
                Me.additionalLanguageField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property TPA_Extensions() As TPA_ExtensionsType
            Get
                Return Me.tPA_ExtensionsField
            End Get
            Set
                Me.tPA_ExtensionsField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Gender() As DocumentTypeGender
            Get
                Return Me.genderField
            End Get
            Set
                Me.genderField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property GenderSpecified() As Boolean
            Get
                Return Me.genderFieldSpecified
            End Get
            Set
                Me.genderFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Deceased() As Boolean
            Get
                Return Me.deceasedField
            End Get
            Set
                Me.deceasedField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DeceasedSpecified() As Boolean
            Get
                Return Me.deceasedFieldSpecified
            End Get
            Set
                Me.deceasedFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property LockoutType() As String
            Get
                Return Me.lockoutTypeField
            End Get
            Set
                Me.lockoutTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property BirthDate() As Date
            Get
                Return Me.birthDateField
            End Get
            Set
                Me.birthDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property BirthDateSpecified() As Boolean
            Get
                Return Me.birthDateFieldSpecified
            End Get
            Set
                Me.birthDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property VIP_Indicator() As Boolean
            Get
                Return Me.vIP_IndicatorField
            End Get
            Set
                Me.vIP_IndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property VIP_IndicatorSpecified() As Boolean
            Get
                Return Me.vIP_IndicatorFieldSpecified
            End Get
            Set
                Me.vIP_IndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Text() As String
            Get
                Return Me.textField
            End Get
            Set
                Me.textField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")>
        Public Property Language() As String
            Get
                Return Me.languageField
            End Get
            Set
                Me.languageField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CustomerValue() As String
            Get
                Return Me.customerValueField
            End Get
            Set
                Me.customerValueField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MaritalStatus() As CustomerTypeMaritalStatus
            Get
                Return Me.maritalStatusField
            End Get
            Set
                Me.maritalStatusField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MaritalStatusSpecified() As Boolean
            Get
                Return Me.maritalStatusFieldSpecified
            End Get
            Set
                Me.maritalStatusFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PreviouslyMarriedIndicator() As Boolean
            Get
                Return Me.previouslyMarriedIndicatorField
            End Get
            Set
                Me.previouslyMarriedIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PreviouslyMarriedIndicatorSpecified() As Boolean
            Get
                Return Me.previouslyMarriedIndicatorFieldSpecified
            End Get
            Set
                Me.previouslyMarriedIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property ChildQuantity() As String
            Get
                Return Me.childQuantityField
            End Get
            Set
                Me.childQuantityField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(PersonNameType1)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PersonNameType

        Private namePrefixField() As String

        Private givenNameField() As String

        Private middleNameField() As String

        Private surnamePrefixField As String

        Private surnameField As String

        Private nameSuffixField() As String

        Private nameTitleField() As String

        Private documentField As PersonNameTypeDocument

        Private shareSynchIndField As PersonNameTypeShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As PersonNameTypeShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private nameTypeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("NamePrefix")>
        Public Property NamePrefix() As String()
            Get
                Return Me.namePrefixField
            End Get
            Set
                Me.namePrefixField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("GivenName")>
        Public Property GivenName() As String()
            Get
                Return Me.givenNameField
            End Get
            Set
                Me.givenNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MiddleName")>
        Public Property MiddleName() As String()
            Get
                Return Me.middleNameField
            End Get
            Set
                Me.middleNameField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property SurnamePrefix() As String
            Get
                Return Me.surnamePrefixField
            End Get
            Set
                Me.surnamePrefixField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Surname() As String
            Get
                Return Me.surnameField
            End Get
            Set
                Me.surnameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("NameSuffix")>
        Public Property NameSuffix() As String()
            Get
                Return Me.nameSuffixField
            End Get
            Set
                Me.nameSuffixField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("NameTitle")>
        Public Property NameTitle() As String()
            Get
                Return Me.nameTitleField
            End Get
            Set
                Me.nameTitleField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Document() As PersonNameTypeDocument
            Get
                Return Me.documentField
            End Get
            Set
                Me.documentField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As PersonNameTypeShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As PersonNameTypeShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property NameType() As String
            Get
                Return Me.nameTypeField
            End Get
            Set
                Me.nameTypeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PersonNameTypeDocument

        Private docIDField As String

        Private docTypeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DocID() As String
            Get
                Return Me.docIDField
            End Get
            Set
                Me.docIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DocType() As String
            Get
                Return Me.docTypeField
            End Get
            Set
                Me.docTypeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(TypeName:="PersonNameType", [Namespace]:="http://xml.amadeus.com/2010/06/Types_v3")>
    Partial Public Class PersonNameType1
        Inherits PersonNameType

        Private nameVarietyField As String

        Private languageField As String

        Private displayedNameField As Boolean

        Private displayedNameFieldSpecified As Boolean

        Private romanizationMethodField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property NameVariety() As String
            Get
                Return Me.nameVarietyField
            End Get
            Set
                Me.nameVarietyField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")>
        Public Property Language() As String
            Get
                Return Me.languageField
            End Get
            Set
                Me.languageField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DisplayedName() As Boolean
            Get
                Return Me.displayedNameField
            End Get
            Set
                Me.displayedNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DisplayedNameSpecified() As Boolean
            Get
                Return Me.displayedNameFieldSpecified
            End Get
            Set
                Me.displayedNameFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RomanizationMethod() As String
            Get
                Return Me.romanizationMethodField
            End Get
            Set
                Me.romanizationMethodField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypeTelephone

        Private effectiveDateField As Date

        Private effectiveDateFieldSpecified As Boolean

        Private expireDateField As Date

        Private expireDateFieldSpecified As Boolean

        Private expireDateExclusiveIndicatorField As Boolean

        Private expireDateExclusiveIndicatorFieldSpecified As Boolean

        Private shareSynchIndField As CustomerTypeTelephoneShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As CustomerTypeTelephoneShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private phoneLocationTypeField As String

        Private phoneTechTypeField As String

        Private phoneUseTypeField As String

        Private countryAccessCodeField As String

        Private areaCityCodeField As String

        Private phoneNumberField As String

        Private extensionField As String

        Private pINField As String

        Private remarkField As String

        Private formattedIndField As Boolean

        Private formattedIndFieldSpecified As Boolean

        Private defaultIndField As Boolean

        Private defaultIndFieldSpecified As Boolean

        Private rPHField As String

        Private transferActionField As TransferActionType

        Private transferActionFieldSpecified As Boolean

        Private parentCompanyRefField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property EffectiveDate() As Date
            Get
                Return Me.effectiveDateField
            End Get
            Set
                Me.effectiveDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EffectiveDateSpecified() As Boolean
            Get
                Return Me.effectiveDateFieldSpecified
            End Get
            Set
                Me.effectiveDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property ExpireDate() As Date
            Get
                Return Me.expireDateField
            End Get
            Set
                Me.expireDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateSpecified() As Boolean
            Get
                Return Me.expireDateFieldSpecified
            End Get
            Set
                Me.expireDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExpireDateExclusiveIndicator() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorField
            End Get
            Set
                Me.expireDateExclusiveIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateExclusiveIndicatorSpecified() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorFieldSpecified
            End Get
            Set
                Me.expireDateExclusiveIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As CustomerTypeTelephoneShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As CustomerTypeTelephoneShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneLocationType() As String
            Get
                Return Me.phoneLocationTypeField
            End Get
            Set
                Me.phoneLocationTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneTechType() As String
            Get
                Return Me.phoneTechTypeField
            End Get
            Set
                Me.phoneTechTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneUseType() As String
            Get
                Return Me.phoneUseTypeField
            End Get
            Set
                Me.phoneUseTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CountryAccessCode() As String
            Get
                Return Me.countryAccessCodeField
            End Get
            Set
                Me.countryAccessCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AreaCityCode() As String
            Get
                Return Me.areaCityCodeField
            End Get
            Set
                Me.areaCityCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneNumber() As String
            Get
                Return Me.phoneNumberField
            End Get
            Set
                Me.phoneNumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Extension() As String
            Get
                Return Me.extensionField
            End Get
            Set
                Me.extensionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PIN() As String
            Get
                Return Me.pINField
            End Get
            Set
                Me.pINField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Remark() As String
            Get
                Return Me.remarkField
            End Get
            Set
                Me.remarkField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property FormattedInd() As Boolean
            Get
                Return Me.formattedIndField
            End Get
            Set
                Me.formattedIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property FormattedIndSpecified() As Boolean
            Get
                Return Me.formattedIndFieldSpecified
            End Get
            Set
                Me.formattedIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DefaultInd() As Boolean
            Get
                Return Me.defaultIndField
            End Get
            Set
                Me.defaultIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DefaultIndSpecified() As Boolean
            Get
                Return Me.defaultIndFieldSpecified
            End Get
            Set
                Me.defaultIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RPH() As String
            Get
                Return Me.rPHField
            End Get
            Set
                Me.rPHField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TransferAction() As TransferActionType
            Get
                Return Me.transferActionField
            End Get
            Set
                Me.transferActionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TransferActionSpecified() As Boolean
            Get
                Return Me.transferActionFieldSpecified
            End Get
            Set
                Me.transferActionFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ParentCompanyRef() As String
            Get
                Return Me.parentCompanyRefField
            End Get
            Set
                Me.parentCompanyRefField = Value
            End Set
        End Property
    End Class
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypeAddress
        Inherits AddressInfoType

        Private companyNameField As CompanyNameType

        Private addresseeNameField As PersonNameType

        Private effectiveDateField As Date

        Private effectiveDateFieldSpecified As Boolean

        Private expireDateField As Date

        Private expireDateFieldSpecified As Boolean

        Private expireDateExclusiveIndicatorField As Boolean

        Private expireDateExclusiveIndicatorFieldSpecified As Boolean

        Private validationStatusField As CustomerTypeAddressValidationStatus

        Private validationStatusFieldSpecified As Boolean

        Private transferActionField As TransferActionType

        Private transferActionFieldSpecified As Boolean

        Private parentCompanyRefField As String

        '''<remarks/>
        Public Property CompanyName() As CompanyNameType
            Get
                Return Me.companyNameField
            End Get
            Set
                Me.companyNameField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property AddresseeName() As PersonNameType
            Get
                Return Me.addresseeNameField
            End Get
            Set
                Me.addresseeNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property EffectiveDate() As Date
            Get
                Return Me.effectiveDateField
            End Get
            Set
                Me.effectiveDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EffectiveDateSpecified() As Boolean
            Get
                Return Me.effectiveDateFieldSpecified
            End Get
            Set
                Me.effectiveDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property ExpireDate() As Date
            Get
                Return Me.expireDateField
            End Get
            Set
                Me.expireDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateSpecified() As Boolean
            Get
                Return Me.expireDateFieldSpecified
            End Get
            Set
                Me.expireDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExpireDateExclusiveIndicator() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorField
            End Get
            Set
                Me.expireDateExclusiveIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateExclusiveIndicatorSpecified() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorFieldSpecified
            End Get
            Set
                Me.expireDateExclusiveIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ValidationStatus() As CustomerTypeAddressValidationStatus
            Get
                Return Me.validationStatusField
            End Get
            Set
                Me.validationStatusField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ValidationStatusSpecified() As Boolean
            Get
                Return Me.validationStatusFieldSpecified
            End Get
            Set
                Me.validationStatusFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TransferAction() As TransferActionType
            Get
                Return Me.transferActionField
            End Get
            Set
                Me.transferActionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TransferActionSpecified() As Boolean
            Get
                Return Me.transferActionFieldSpecified
            End Get
            Set
                Me.transferActionFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ParentCompanyRef() As String
            Get
                Return Me.parentCompanyRefField
            End Get
            Set
                Me.parentCompanyRefField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum CustomerTypeAddressValidationStatus

        '''<remarks/>
        SystemValidated

        '''<remarks/>
        UserValidated

        '''<remarks/>
        NotChecked
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class AddressInfoType
        Inherits AddressType

        Private defaultIndField As Boolean

        Private defaultIndFieldSpecified As Boolean

        Private useTypeField As String

        Private rPHField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DefaultInd() As Boolean
            Get
                Return Me.defaultIndField
            End Get
            Set
                Me.defaultIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DefaultIndSpecified() As Boolean
            Get
                Return Me.defaultIndFieldSpecified
            End Get
            Set
                Me.defaultIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property UseType() As String
            Get
                Return Me.useTypeField
            End Get
            Set
                Me.useTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RPH() As String
            Get
                Return Me.rPHField
            End Get
            Set
                Me.rPHField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(AddressInfoType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class AddressType

        Private streetNmbrField As AddressTypeStreetNmbr

        Private bldgRoomField() As AddressTypeBldgRoom

        Private addressLineField() As String

        Private cityNameField As String

        Private postalCodeField As String

        Private countyField As String

        Private stateProvField As StateProvType

        Private countryNameField As CountryNameType

        Private formattedIndField As Boolean

        Private formattedIndFieldSpecified As Boolean

        Private shareSynchIndField As PersonNameTypeShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As PersonNameTypeShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private typeField As String

        Private remarkField As String

        '''<remarks/>
        Public Property StreetNmbr() As AddressTypeStreetNmbr
            Get
                Return Me.streetNmbrField
            End Get
            Set
                Me.streetNmbrField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("BldgRoom")>
        Public Property BldgRoom() As AddressTypeBldgRoom()
            Get
                Return Me.bldgRoomField
            End Get
            Set
                Me.bldgRoomField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AddressLine")>
        Public Property AddressLine() As String()
            Get
                Return Me.addressLineField
            End Get
            Set
                Me.addressLineField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property CityName() As String
            Get
                Return Me.cityNameField
            End Get
            Set
                Me.cityNameField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property PostalCode() As String
            Get
                Return Me.postalCodeField
            End Get
            Set
                Me.postalCodeField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property County() As String
            Get
                Return Me.countyField
            End Get
            Set
                Me.countyField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property StateProv() As StateProvType
            Get
                Return Me.stateProvField
            End Get
            Set
                Me.stateProvField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property CountryName() As CountryNameType
            Get
                Return Me.countryNameField
            End Get
            Set
                Me.countryNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property FormattedInd() As Boolean
            Get
                Return Me.formattedIndField
            End Get
            Set
                Me.formattedIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property FormattedIndSpecified() As Boolean
            Get
                Return Me.formattedIndFieldSpecified
            End Get
            Set
                Me.formattedIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As PersonNameTypeShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As PersonNameTypeShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Remark() As String
            Get
                Return Me.remarkField
            End Get
            Set
                Me.remarkField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class AddressTypeStreetNmbr
        Inherits StreetNmbrType

        Private streetNmbrSuffixField As String

        Private streetDirectionField As String

        Private ruralRouteNmbrField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property StreetNmbrSuffix() As String
            Get
                Return Me.streetNmbrSuffixField
            End Get
            Set
                Me.streetNmbrSuffixField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property StreetDirection() As String
            Get
                Return Me.streetDirectionField
            End Get
            Set
                Me.streetDirectionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RuralRouteNmbr() As String
            Get
                Return Me.ruralRouteNmbrField
            End Get
            Set
                Me.ruralRouteNmbrField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class StreetNmbrType

        Private pO_BoxField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PO_Box() As String
            Get
                Return Me.pO_BoxField
            End Get
            Set
                Me.pO_BoxField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class AddressTypeBldgRoom

        Private bldgNameIndicatorField As Boolean

        Private bldgNameIndicatorFieldSpecified As Boolean

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property BldgNameIndicator() As Boolean
            Get
                Return Me.bldgNameIndicatorField
            End Get
            Set
                Me.bldgNameIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property BldgNameIndicatorSpecified() As Boolean
            Get
                Return Me.bldgNameIndicatorFieldSpecified
            End Get
            Set
                Me.bldgNameIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class StateProvType

        Private stateCodeField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property StateCode() As String
            Get
                Return Me.stateCodeField
            End Get
            Set
                Me.stateCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CountryNameType

        Private codeField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypeURL
        Inherits URL_Type

        Private transferActionField As TransferActionType

        Private transferActionFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TransferAction() As TransferActionType
            Get
                Return Me.transferActionField
            End Get
            Set
                Me.transferActionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TransferActionSpecified() As Boolean
            Get
                Return Me.transferActionFieldSpecified
            End Get
            Set
                Me.transferActionFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class URL_Type

        Private shareSynchIndField As PersonNameTypeShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As PersonNameTypeShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private typeField As String

        Private defaultIndField As Boolean

        Private defaultIndFieldSpecified As Boolean

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As PersonNameTypeShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As PersonNameTypeShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DefaultInd() As Boolean
            Get
                Return Me.defaultIndField
            End Get
            Set
                Me.defaultIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DefaultIndSpecified() As Boolean
            Get
                Return Me.defaultIndFieldSpecified
            End Get
            Set
                Me.defaultIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute(DataType:="anyURI")>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypeCitizenCountryName

        Private defaultIndField As Boolean

        Private defaultIndFieldSpecified As Boolean

        Private codeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DefaultInd() As Boolean
            Get
                Return Me.defaultIndField
            End Get
            Set
                Me.defaultIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DefaultIndSpecified() As Boolean
            Get
                Return Me.defaultIndFieldSpecified
            End Get
            Set
                Me.defaultIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypePhysChallName

        Private physChallIndField As Boolean

        Private physChallIndFieldSpecified As Boolean

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhysChallInd() As Boolean
            Get
                Return Me.physChallIndField
            End Get
            Set
                Me.physChallIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PhysChallIndSpecified() As Boolean
            Get
                Return Me.physChallIndFieldSpecified
            End Get
            Set
                Me.physChallIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypePaymentForm
        Inherits PaymentFormType

        Private associatedSupplierField As CustomerTypePaymentFormAssociatedSupplier

        Private transferActionField As TransferActionType

        Private transferActionFieldSpecified As Boolean

        Private defaultIndField As Boolean

        Private defaultIndFieldSpecified As Boolean

        Private parentCompanyRefField As String

        '''<remarks/>
        Public Property AssociatedSupplier() As CustomerTypePaymentFormAssociatedSupplier
            Get
                Return Me.associatedSupplierField
            End Get
            Set
                Me.associatedSupplierField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TransferAction() As TransferActionType
            Get
                Return Me.transferActionField
            End Get
            Set
                Me.transferActionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TransferActionSpecified() As Boolean
            Get
                Return Me.transferActionFieldSpecified
            End Get
            Set
                Me.transferActionFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DefaultInd() As Boolean
            Get
                Return Me.defaultIndField
            End Get
            Set
                Me.defaultIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DefaultIndSpecified() As Boolean
            Get
                Return Me.defaultIndFieldSpecified
            End Get
            Set
                Me.defaultIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ParentCompanyRef() As String
            Get
                Return Me.parentCompanyRefField
            End Get
            Set
                Me.parentCompanyRefField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypePaymentFormAssociatedSupplier

        Private companyShortNameField As String

        Private travelSectorField As String

        Private codeField As String

        Private codeContextField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CompanyShortName() As String
            Get
                Return Me.companyShortNameField
            End Get
            Set
                Me.companyShortNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TravelSector() As String
            Get
                Return Me.travelSectorField
            End Get
            Set
                Me.travelSectorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CodeContext() As String
            Get
                Return Me.codeContextField
            End Get
            Set
                Me.codeContextField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(PaymentResponseType)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(HotelPaymentFormType)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(PaymentDetailType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class PaymentFormType


        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("BankAcct", GetType(BankAcctType)),
            System.Xml.Serialization.XmlElementAttribute("Cash", GetType(PaymentFormTypeCash)),
            System.Xml.Serialization.XmlElementAttribute("DirectBill", GetType(DirectBillType)),
            System.Xml.Serialization.XmlElementAttribute("LoyaltyRedemption", GetType(PaymentFormTypeLoyaltyRedemption)),
            System.Xml.Serialization.XmlElementAttribute("MiscChargeOrder", GetType(PaymentFormTypeMiscChargeOrder)),
            System.Xml.Serialization.XmlElementAttribute("PaymentCard", GetType(PaymentCardType)),
            System.Xml.Serialization.XmlElementAttribute("Ticket", GetType(PaymentFormTypeTicket)),
            System.Xml.Serialization.XmlElementAttribute("Voucher", GetType(PaymentFormTypeVoucher))>
        Public Property Item() As Object

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As PersonNameTypeShareSynchInd

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As PersonNameTypeShareMarketInd

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CostCenterID() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RPH() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PaymentTransactionTypeCode() As PaymentFormTypePaymentTransactionTypeCode

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PaymentTransactionTypeCodeSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property GuaranteeIndicator() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property GuaranteeIndicatorSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property GuaranteeTypeCode() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property GuaranteeID() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Remark() As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class BankAcctType

        '''<remarks/>
        Public Property BankAcctName() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As PersonNameTypeShareSynchInd

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As PersonNameTypeShareMarketInd

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property BankID() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AcctType() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property BankAcctNumber() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ChecksAcceptedInd() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ChecksAcceptedIndSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CheckNumber() As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class PaymentFormTypeCash

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CashIndicator As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property CashIndicatorSpecified As Boolean
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class DirectBillType

        '''<remarks/>
        Public Property CompanyName() As DirectBillTypeCompanyName

        '''<remarks/>
        Public Property Address() As AddressInfoType

        '''<remarks/>
        Public Property Email() As EmailType

        '''<remarks/>
        Public Property Telephone() As DirectBillTypeTelephone

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As PersonNameTypeShareSynchInd

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As PersonNameTypeShareMarketInd

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DirectBill_ID() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property BillingNumber() As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class DirectBillTypeCompanyName
        Inherits CompanyNameType

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ContactName As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class DirectBillTypeTelephone

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As CustomerTypeTelephoneShareSynchInd

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As CustomerTypeTelephoneShareMarketInd

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneLocationType() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneTechType() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneUseType() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CountryAccessCode() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AreaCityCode() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneNumber() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Extension() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PIN() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Remark() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property FormattedInd() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property FormattedIndSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DefaultInd() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DefaultIndSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RPH() As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class PaymentFormTypeLoyaltyRedemption

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyCertificate")>
        Public Property LoyaltyCertificate() As PaymentFormTypeLoyaltyRedemptionLoyaltyCertificate()

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CertificateNumber() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MemberNumber() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ProgramName() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PromotionCode() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PromotionVendorCode() As String()

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RedemptionQuantity() As Long

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RedemptionQuantitySpecified() As Boolean
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class PaymentFormTypeLoyaltyRedemptionLoyaltyCertificate

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ID() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ID_Context() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CertificateNumber() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MemberNumber() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ProgramName() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property EffectiveDate() As Date

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EffectiveDateSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property ExpireDate() As Date

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExpireDateExclusiveIndicator() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateExclusiveIndicatorSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property NmbrOfNights() As Long

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property NmbrOfNightsSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Format() As PaymentFormTypeLoyaltyRedemptionLoyaltyCertificateFormat

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property FormatSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Status() As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute()>
    Public Enum PaymentFormTypeLoyaltyRedemptionLoyaltyCertificateFormat

        '''<remarks/>
        Paper

        '''<remarks/>
        Electronic
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class PaymentFormTypeMiscChargeOrder

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TicketNumber() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OriginalTicketNumber() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OriginalIssuePlace() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property OriginalIssueDate() As Date

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OriginalIssueDateSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OriginalIssueIATA() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OriginalPaymentForm() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CheckInhibitorType() As PaymentFormTypeMiscChargeOrderCheckInhibitorType

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property CheckInhibitorTypeSpecified() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CouponRPHs() As String()

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PaperMCO_ExistInd() As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PaperMCO_ExistIndSpecified() As Boolean
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute()>
    Public Enum PaymentFormTypeMiscChargeOrderCheckInhibitorType

        '''<remarks/>
        CheckDigit

        '''<remarks/>
        InterlineAgreement

        '''<remarks/>
        Both
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class PaymentCardType

        Private cardHolderNameField As String

        Private cardIssuerNameField As PaymentCardTypeCardIssuerName

        Private addressField As AddressType

        Private telephoneField() As PaymentCardTypeTelephone

        Private emailField() As EmailType

        Private custLoyaltyField() As PaymentCardTypeCustLoyalty

        Private signatureOnFileField As PaymentCardTypeSignatureOnFile

        Private magneticStripeField As PaymentCardTypeMagneticStripe

        Private shareSynchIndField As PersonNameTypeShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As PersonNameTypeShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private cardTypeField As String

        Private cardCodeField As String

        Private cardNumberField As String

        Private seriesCodeField As String

        Private effectiveDateField As String

        Private expireDateField As String

        Private maskedCardNumberField As String

        Private cardHolderRPHField As String

        Private extendPaymentIndicatorField As Boolean

        Private extendPaymentIndicatorFieldSpecified As Boolean

        Private countryOfIssueField As String

        Private extendedPaymentQuantityField As String

        Private signatureOnFileIndicatorField As Boolean

        Private signatureOnFileIndicatorFieldSpecified As Boolean

        Private companyCardReferenceField As String

        Private remarkField As String

        Private encryptionKeyField As String

        '''<remarks/>
        Public Property CardHolderName() As String
            Get
                Return Me.cardHolderNameField
            End Get
            Set
                Me.cardHolderNameField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property CardIssuerName() As PaymentCardTypeCardIssuerName
            Get
                Return Me.cardIssuerNameField
            End Get
            Set
                Me.cardIssuerNameField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Address() As AddressType
            Get
                Return Me.addressField
            End Get
            Set
                Me.addressField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Telephone")>
        Public Property Telephone() As PaymentCardTypeTelephone()
            Get
                Return Me.telephoneField
            End Get
            Set
                Me.telephoneField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Email")>
        Public Property Email() As EmailType()
            Get
                Return Me.emailField
            End Get
            Set
                Me.emailField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CustLoyalty")>
        Public Property CustLoyalty() As PaymentCardTypeCustLoyalty()
            Get
                Return Me.custLoyaltyField
            End Get
            Set
                Me.custLoyaltyField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property SignatureOnFile() As PaymentCardTypeSignatureOnFile
            Get
                Return Me.signatureOnFileField
            End Get
            Set
                Me.signatureOnFileField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property MagneticStripe() As PaymentCardTypeMagneticStripe
            Get
                Return Me.magneticStripeField
            End Get
            Set
                Me.magneticStripeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As PersonNameTypeShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As PersonNameTypeShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CardType() As String
            Get
                Return Me.cardTypeField
            End Get
            Set
                Me.cardTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CardCode() As String
            Get
                Return Me.cardCodeField
            End Get
            Set
                Me.cardCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CardNumber() As String
            Get
                Return Me.cardNumberField
            End Get
            Set
                Me.cardNumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SeriesCode() As String
            Get
                Return Me.seriesCodeField
            End Get
            Set
                Me.seriesCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property EffectiveDate() As String
            Get
                Return Me.effectiveDateField
            End Get
            Set
                Me.effectiveDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExpireDate() As String
            Get
                Return Me.expireDateField
            End Get
            Set
                Me.expireDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MaskedCardNumber() As String
            Get
                Return Me.maskedCardNumberField
            End Get
            Set
                Me.maskedCardNumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CardHolderRPH() As String
            Get
                Return Me.cardHolderRPHField
            End Get
            Set
                Me.cardHolderRPHField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExtendPaymentIndicator() As Boolean
            Get
                Return Me.extendPaymentIndicatorField
            End Get
            Set
                Me.extendPaymentIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExtendPaymentIndicatorSpecified() As Boolean
            Get
                Return Me.extendPaymentIndicatorFieldSpecified
            End Get
            Set
                Me.extendPaymentIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CountryOfIssue() As String
            Get
                Return Me.countryOfIssueField
            End Get
            Set
                Me.countryOfIssueField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property ExtendedPaymentQuantity() As String
            Get
                Return Me.extendedPaymentQuantityField
            End Get
            Set
                Me.extendedPaymentQuantityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SignatureOnFileIndicator() As Boolean
            Get
                Return Me.signatureOnFileIndicatorField
            End Get
            Set
                Me.signatureOnFileIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SignatureOnFileIndicatorSpecified() As Boolean
            Get
                Return Me.signatureOnFileIndicatorFieldSpecified
            End Get
            Set
                Me.signatureOnFileIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CompanyCardReference() As String
            Get
                Return Me.companyCardReferenceField
            End Get
            Set
                Me.companyCardReferenceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Remark() As String
            Get
                Return Me.remarkField
            End Get
            Set
                Me.remarkField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property EncryptionKey() As String
            Get
                Return Me.encryptionKeyField
            End Get
            Set
                Me.encryptionKeyField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PaymentCardTypeCardIssuerName

        Private bankIDField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property BankID() As String
            Get
                Return Me.bankIDField
            End Get
            Set
                Me.bankIDField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PaymentCardTypeTelephone

        Private shareSynchIndField As CustomerTypeTelephoneShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As CustomerTypeTelephoneShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private phoneLocationTypeField As String

        Private phoneTechTypeField As String

        Private phoneUseTypeField As String

        Private countryAccessCodeField As String

        Private areaCityCodeField As String

        Private phoneNumberField As String

        Private extensionField As String

        Private pINField As String

        Private remarkField As String

        Private formattedIndField As Boolean

        Private formattedIndFieldSpecified As Boolean

        Private defaultIndField As Boolean

        Private defaultIndFieldSpecified As Boolean

        Private rPHField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As CustomerTypeTelephoneShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As CustomerTypeTelephoneShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneLocationType() As String
            Get
                Return Me.phoneLocationTypeField
            End Get
            Set
                Me.phoneLocationTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneTechType() As String
            Get
                Return Me.phoneTechTypeField
            End Get
            Set
                Me.phoneTechTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneUseType() As String
            Get
                Return Me.phoneUseTypeField
            End Get
            Set
                Me.phoneUseTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CountryAccessCode() As String
            Get
                Return Me.countryAccessCodeField
            End Get
            Set
                Me.countryAccessCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AreaCityCode() As String
            Get
                Return Me.areaCityCodeField
            End Get
            Set
                Me.areaCityCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneNumber() As String
            Get
                Return Me.phoneNumberField
            End Get
            Set
                Me.phoneNumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Extension() As String
            Get
                Return Me.extensionField
            End Get
            Set
                Me.extensionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PIN() As String
            Get
                Return Me.pINField
            End Get
            Set
                Me.pINField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Remark() As String
            Get
                Return Me.remarkField
            End Get
            Set
                Me.remarkField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property FormattedInd() As Boolean
            Get
                Return Me.formattedIndField
            End Get
            Set
                Me.formattedIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property FormattedIndSpecified() As Boolean
            Get
                Return Me.formattedIndFieldSpecified
            End Get
            Set
                Me.formattedIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DefaultInd() As Boolean
            Get
                Return Me.defaultIndField
            End Get
            Set
                Me.defaultIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DefaultIndSpecified() As Boolean
            Get
                Return Me.defaultIndFieldSpecified
            End Get
            Set
                Me.defaultIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RPH() As String
            Get
                Return Me.rPHField
            End Get
            Set
                Me.rPHField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PaymentCardTypeCustLoyalty

        Private shareSynchIndField As PaymentCardTypeCustLoyaltyShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As PaymentCardTypeCustLoyaltyShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private programIDField As String

        Private membershipIDField As String

        Private travelSectorField As String

        Private loyalLevelField As String

        Private loyalLevelCodeField As String

        Private singleVendorIndField As PaymentCardTypeCustLoyaltySingleVendorInd

        Private singleVendorIndFieldSpecified As Boolean

        Private signupDateField As Date

        Private signupDateFieldSpecified As Boolean

        Private effectiveDateField As Date

        Private effectiveDateFieldSpecified As Boolean

        Private expireDateField As Date

        Private expireDateFieldSpecified As Boolean

        Private expireDateExclusiveIndicatorField As Boolean

        Private expireDateExclusiveIndicatorFieldSpecified As Boolean

        Private rPHField As String

        Private vendorCodeField() As String

        Private primaryLoyaltyIndicatorField As Boolean

        Private primaryLoyaltyIndicatorFieldSpecified As Boolean

        Private allianceLoyaltyLevelNameField As String

        Private customerTypeField As String

        Private customerValueField As String

        Private passwordField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As PaymentCardTypeCustLoyaltyShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As PaymentCardTypeCustLoyaltyShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ProgramID() As String
            Get
                Return Me.programIDField
            End Get
            Set
                Me.programIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MembershipID() As String
            Get
                Return Me.membershipIDField
            End Get
            Set
                Me.membershipIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TravelSector() As String
            Get
                Return Me.travelSectorField
            End Get
            Set
                Me.travelSectorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property LoyalLevel() As String
            Get
                Return Me.loyalLevelField
            End Get
            Set
                Me.loyalLevelField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property LoyalLevelCode() As String
            Get
                Return Me.loyalLevelCodeField
            End Get
            Set
                Me.loyalLevelCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SingleVendorInd() As PaymentCardTypeCustLoyaltySingleVendorInd
            Get
                Return Me.singleVendorIndField
            End Get
            Set
                Me.singleVendorIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SingleVendorIndSpecified() As Boolean
            Get
                Return Me.singleVendorIndFieldSpecified
            End Get
            Set
                Me.singleVendorIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property SignupDate() As Date
            Get
                Return Me.signupDateField
            End Get
            Set
                Me.signupDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SignupDateSpecified() As Boolean
            Get
                Return Me.signupDateFieldSpecified
            End Get
            Set
                Me.signupDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property EffectiveDate() As Date
            Get
                Return Me.effectiveDateField
            End Get
            Set
                Me.effectiveDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EffectiveDateSpecified() As Boolean
            Get
                Return Me.effectiveDateFieldSpecified
            End Get
            Set
                Me.effectiveDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property ExpireDate() As Date
            Get
                Return Me.expireDateField
            End Get
            Set
                Me.expireDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateSpecified() As Boolean
            Get
                Return Me.expireDateFieldSpecified
            End Get
            Set
                Me.expireDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExpireDateExclusiveIndicator() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorField
            End Get
            Set
                Me.expireDateExclusiveIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateExclusiveIndicatorSpecified() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorFieldSpecified
            End Get
            Set
                Me.expireDateExclusiveIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RPH() As String
            Get
                Return Me.rPHField
            End Get
            Set
                Me.rPHField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property VendorCode() As String()
            Get
                Return Me.vendorCodeField
            End Get
            Set
                Me.vendorCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PrimaryLoyaltyIndicator() As Boolean
            Get
                Return Me.primaryLoyaltyIndicatorField
            End Get
            Set
                Me.primaryLoyaltyIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PrimaryLoyaltyIndicatorSpecified() As Boolean
            Get
                Return Me.primaryLoyaltyIndicatorFieldSpecified
            End Get
            Set
                Me.primaryLoyaltyIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AllianceLoyaltyLevelName() As String
            Get
                Return Me.allianceLoyaltyLevelNameField
            End Get
            Set
                Me.allianceLoyaltyLevelNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CustomerType() As String
            Get
                Return Me.customerTypeField
            End Get
            Set
                Me.customerTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CustomerValue() As String
            Get
                Return Me.customerValueField
            End Get
            Set
                Me.customerValueField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Password() As String
            Get
                Return Me.passwordField
            End Get
            Set
                Me.passwordField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum PaymentCardTypeCustLoyaltyShareSynchInd

        '''<remarks/>
        Yes

        '''<remarks/>
        No

        '''<remarks/>
        Inherit
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum PaymentCardTypeCustLoyaltyShareMarketInd

        '''<remarks/>
        Yes

        '''<remarks/>
        No

        '''<remarks/>
        Inherit
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum PaymentCardTypeCustLoyaltySingleVendorInd

        '''<remarks/>
        SingleVndr

        '''<remarks/>
        Alliance
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PaymentCardTypeSignatureOnFile

        Private signatureOnFileIndicatorField As Boolean

        Private signatureOnFileIndicatorFieldSpecified As Boolean

        Private effectiveDateField As Date

        Private effectiveDateFieldSpecified As Boolean

        Private expireDateField As Date

        Private expireDateFieldSpecified As Boolean

        Private expireDateExclusiveIndicatorField As Boolean

        Private expireDateExclusiveIndicatorFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SignatureOnFileIndicator() As Boolean
            Get
                Return Me.signatureOnFileIndicatorField
            End Get
            Set
                Me.signatureOnFileIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SignatureOnFileIndicatorSpecified() As Boolean
            Get
                Return Me.signatureOnFileIndicatorFieldSpecified
            End Get
            Set
                Me.signatureOnFileIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property EffectiveDate() As Date
            Get
                Return Me.effectiveDateField
            End Get
            Set
                Me.effectiveDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EffectiveDateSpecified() As Boolean
            Get
                Return Me.effectiveDateFieldSpecified
            End Get
            Set
                Me.effectiveDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property ExpireDate() As Date
            Get
                Return Me.expireDateField
            End Get
            Set
                Me.expireDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateSpecified() As Boolean
            Get
                Return Me.expireDateFieldSpecified
            End Get
            Set
                Me.expireDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExpireDateExclusiveIndicator() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorField
            End Get
            Set
                Me.expireDateExclusiveIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateExclusiveIndicatorSpecified() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorFieldSpecified
            End Get
            Set
                Me.expireDateExclusiveIndicatorFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PaymentCardTypeMagneticStripe

        Private track1Field() As Byte

        Private track2Field() As Byte

        Private track3Field() As Byte

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="base64Binary")>
        Public Property Track1() As Byte()
            Get
                Return Me.track1Field
            End Get
            Set
                Me.track1Field = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="base64Binary")>
        Public Property Track2() As Byte()
            Get
                Return Me.track2Field
            End Get
            Set
                Me.track2Field = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="base64Binary")>
        Public Property Track3() As Byte()
            Get
                Return Me.track3Field
            End Get
            Set
                Me.track3Field = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PaymentFormTypeTicket

        Private conjunctionTicketNbrField() As PaymentFormTypeTicketConjunctionTicketNbr

        Private ticketNumberField As String

        Private originalTicketNumberField As String

        Private originalIssuePlaceField As String

        Private originalIssueDateField As Date

        Private originalIssueDateFieldSpecified As Boolean

        Private originalIssueIATAField As String

        Private originalPaymentFormField As String

        Private checkInhibitorTypeField As PaymentFormTypeMiscChargeOrderCheckInhibitorType

        Private checkInhibitorTypeFieldSpecified As Boolean

        Private couponRPHsField() As String

        Private reroutingTypeField As PaymentFormTypeTicketReroutingType

        Private reroutingTypeFieldSpecified As Boolean

        Private reasonForRerouteField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ConjunctionTicketNbr")>
        Public Property ConjunctionTicketNbr() As PaymentFormTypeTicketConjunctionTicketNbr()
            Get
                Return Me.conjunctionTicketNbrField
            End Get
            Set
                Me.conjunctionTicketNbrField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TicketNumber() As String
            Get
                Return Me.ticketNumberField
            End Get
            Set
                Me.ticketNumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OriginalTicketNumber() As String
            Get
                Return Me.originalTicketNumberField
            End Get
            Set
                Me.originalTicketNumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OriginalIssuePlace() As String
            Get
                Return Me.originalIssuePlaceField
            End Get
            Set
                Me.originalIssuePlaceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property OriginalIssueDate() As Date
            Get
                Return Me.originalIssueDateField
            End Get
            Set
                Me.originalIssueDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OriginalIssueDateSpecified() As Boolean
            Get
                Return Me.originalIssueDateFieldSpecified
            End Get
            Set
                Me.originalIssueDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OriginalIssueIATA() As String
            Get
                Return Me.originalIssueIATAField
            End Get
            Set
                Me.originalIssueIATAField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OriginalPaymentForm() As String
            Get
                Return Me.originalPaymentFormField
            End Get
            Set
                Me.originalPaymentFormField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CheckInhibitorType() As PaymentFormTypeMiscChargeOrderCheckInhibitorType
            Get
                Return Me.checkInhibitorTypeField
            End Get
            Set
                Me.checkInhibitorTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property CheckInhibitorTypeSpecified() As Boolean
            Get
                Return Me.checkInhibitorTypeFieldSpecified
            End Get
            Set
                Me.checkInhibitorTypeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CouponRPHs() As String()
            Get
                Return Me.couponRPHsField
            End Get
            Set
                Me.couponRPHsField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ReroutingType() As PaymentFormTypeTicketReroutingType
            Get
                Return Me.reroutingTypeField
            End Get
            Set
                Me.reroutingTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ReroutingTypeSpecified() As Boolean
            Get
                Return Me.reroutingTypeFieldSpecified
            End Get
            Set
                Me.reroutingTypeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ReasonForReroute() As String
            Get
                Return Me.reasonForRerouteField
            End Get
            Set
                Me.reasonForRerouteField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PaymentFormTypeTicketConjunctionTicketNbr

        Private couponsField() As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Coupons() As String()
            Get
                Return Me.couponsField
            End Get
            Set
                Me.couponsField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum PaymentFormTypeTicketReroutingType

        '''<remarks/>
        voluntary

        '''<remarks/>
        involuntary
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PaymentFormTypeVoucher

        Private effectiveDateField As Date

        Private effectiveDateFieldSpecified As Boolean

        Private expireDateField As Date

        Private expireDateFieldSpecified As Boolean

        Private expireDateExclusiveIndicatorField As Boolean

        Private expireDateExclusiveIndicatorFieldSpecified As Boolean

        Private seriesCodeField As String

        Private billingNumberField As String

        Private supplierIdentifierField As String

        Private identifierField As String

        Private valueTypeField As String

        Private electronicIndicatorField As Boolean

        Private electronicIndicatorFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property EffectiveDate() As Date
            Get
                Return Me.effectiveDateField
            End Get
            Set
                Me.effectiveDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EffectiveDateSpecified() As Boolean
            Get
                Return Me.effectiveDateFieldSpecified
            End Get
            Set
                Me.effectiveDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property ExpireDate() As Date
            Get
                Return Me.expireDateField
            End Get
            Set
                Me.expireDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateSpecified() As Boolean
            Get
                Return Me.expireDateFieldSpecified
            End Get
            Set
                Me.expireDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExpireDateExclusiveIndicator() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorField
            End Get
            Set
                Me.expireDateExclusiveIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateExclusiveIndicatorSpecified() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorFieldSpecified
            End Get
            Set
                Me.expireDateExclusiveIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SeriesCode() As String
            Get
                Return Me.seriesCodeField
            End Get
            Set
                Me.seriesCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property BillingNumber() As String
            Get
                Return Me.billingNumberField
            End Get
            Set
                Me.billingNumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SupplierIdentifier() As String
            Get
                Return Me.supplierIdentifierField
            End Get
            Set
                Me.supplierIdentifierField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Identifier() As String
            Get
                Return Me.identifierField
            End Get
            Set
                Me.identifierField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ValueType() As String
            Get
                Return Me.valueTypeField
            End Get
            Set
                Me.valueTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ElectronicIndicator() As Boolean
            Get
                Return Me.electronicIndicatorField
            End Get
            Set
                Me.electronicIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ElectronicIndicatorSpecified() As Boolean
            Get
                Return Me.electronicIndicatorFieldSpecified
            End Get
            Set
                Me.electronicIndicatorFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum PaymentFormTypePaymentTransactionTypeCode

        '''<remarks/>
        charge

        '''<remarks/>
        reserve

        '''<remarks/>
        refund
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PaymentResponseType
        Inherits PaymentFormType

        Private paymentAmountField As PaymentResponseTypePaymentAmount

        Private paymentReferenceIDField As UniqueID_Type

        Private errorField As ErrorType1

        '''<remarks/>
        Public Property PaymentAmount() As PaymentResponseTypePaymentAmount
            Get
                Return Me.paymentAmountField
            End Get
            Set
                Me.paymentAmountField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentReferenceID() As UniqueID_Type
            Get
                Return Me.paymentReferenceIDField
            End Get
            Set
                Me.paymentReferenceIDField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property [Error]() As ErrorType1
            Get
                Return Me.errorField
            End Get
            Set
                Me.errorField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PaymentResponseTypePaymentAmount

        Private amountField As Decimal

        Private amountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        Private approvalCodeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ApprovalCode() As String
            Get
                Return Me.approvalCodeField
            End Get
            Set
                Me.approvalCodeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class ErrorType1
        Inherits FreeTextType
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type As String
 
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShortText As String
 
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code As String
 
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")>
        Public Property DocURL As String
 
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Status As String
 
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Tag As String
 
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RecordID As String
 
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property NodeList As String
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(CommissionInfoType)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(CertificationType)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(WarningType1)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(ErrorType1)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(ErrorType)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(WarningType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class FreeTextType
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")>
        Public Property Language As String

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class CommissionInfoType
        Inherits FreeTextType


        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd As PersonNameTypeShareSynchInd

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketIndAs As PersonNameTypeShareMarketInd

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CommissionPlanCode As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount As Decimal

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces As Long

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified As Boolean
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class CertificationType
        Inherits FreeTextType

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ID As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SingleVendorInd As CertificationTypeSingleVendorInd

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SingleVendorIndSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property EffectiveDate As Date

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EffectiveDateSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property ExpireDate As Date

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExpireDateExclusiveIndicator As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateExclusiveIndicatorSpecified As Boolean
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute()>
    Public Enum CertificationTypeSingleVendorInd

        '''<remarks/>
        SingleVndr

        '''<remarks/>
        Alliance
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class WarningType1
        Inherits FreeTextType

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShortText As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")>
        Public Property DocURL As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Status As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Tag As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RecordID As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RPH As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class WarningType
        Inherits FreeTextType

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShortText() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")>
        Public Property DocURL() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Status() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Tag() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RecordID() As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class HotelPaymentFormType
        Inherits PaymentFormType

        Private masterAccountUsageField As HotelPaymentFormTypeMasterAccountUsage

        '''<remarks/>
        Public Property MasterAccountUsage() As HotelPaymentFormTypeMasterAccountUsage
            Get
                Return Me.masterAccountUsageField
            End Get
            Set
                Me.masterAccountUsageField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class HotelPaymentFormTypeMasterAccountUsage

        Private billingTypeField As HotelPaymentFormTypeMasterAccountUsageBillingType

        Private billingTypeFieldSpecified As Boolean

        Private signFoodAndBevField As Boolean

        Private signFoodAndBevFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property BillingType() As HotelPaymentFormTypeMasterAccountUsageBillingType
            Get
                Return Me.billingTypeField
            End Get
            Set
                Me.billingTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property BillingTypeSpecified() As Boolean
            Get
                Return Me.billingTypeFieldSpecified
            End Get
            Set
                Me.billingTypeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SignFoodAndBev() As Boolean
            Get
                Return Me.signFoodAndBevField
            End Get
            Set
                Me.signFoodAndBevField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SignFoodAndBevSpecified() As Boolean
            Get
                Return Me.signFoodAndBevFieldSpecified
            End Get
            Set
                Me.signFoodAndBevFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum HotelPaymentFormTypeMasterAccountUsageBillingType

        '''<remarks/>
        EachPaysOwn

        '''<remarks/>
        SignRoomAndTax

        '''<remarks/>
        SignAllCharges

        '''<remarks/>
        SignRoomOnly
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PaymentDetailType
        Inherits PaymentFormType

        Private paymentAmountField() As PaymentDetailTypePaymentAmount

        Private commissionField As CommissionType

        Private paymentTypeField As String

        Private splitPaymentIndField As Boolean

        Private splitPaymentIndFieldSpecified As Boolean

        Private authorizedDaysField As String

        Private primaryPaymentIndField As Boolean

        Private primaryPaymentIndFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PaymentAmount")>
        Public Property PaymentAmount() As PaymentDetailTypePaymentAmount()
            Get
                Return Me.paymentAmountField
            End Get
            Set
                Me.paymentAmountField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Commission() As CommissionType
            Get
                Return Me.commissionField
            End Get
            Set
                Me.commissionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PaymentType() As String
            Get
                Return Me.paymentTypeField
            End Get
            Set
                Me.paymentTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SplitPaymentInd() As Boolean
            Get
                Return Me.splitPaymentIndField
            End Get
            Set
                Me.splitPaymentIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SplitPaymentIndSpecified() As Boolean
            Get
                Return Me.splitPaymentIndFieldSpecified
            End Get
            Set
                Me.splitPaymentIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property AuthorizedDays() As String
            Get
                Return Me.authorizedDaysField
            End Get
            Set
                Me.authorizedDaysField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PrimaryPaymentInd() As Boolean
            Get
                Return Me.primaryPaymentIndField
            End Get
            Set
                Me.primaryPaymentIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PrimaryPaymentIndSpecified() As Boolean
            Get
                Return Me.primaryPaymentIndFieldSpecified
            End Get
            Set
                Me.primaryPaymentIndFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PaymentDetailTypePaymentAmount

        Private amountField As Decimal

        Private amountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        Private approvalCodeField As String

        Private refundCalcMethodField As PaymentDetailTypePaymentAmountRefundCalcMethod

        Private refundCalcMethodFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ApprovalCode() As String
            Get
                Return Me.approvalCodeField
            End Get
            Set
                Me.approvalCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RefundCalcMethod() As PaymentDetailTypePaymentAmountRefundCalcMethod
            Get
                Return Me.refundCalcMethodField
            End Get
            Set
                Me.refundCalcMethodField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RefundCalcMethodSpecified() As Boolean
            Get
                Return Me.refundCalcMethodFieldSpecified
            End Get
            Set
                Me.refundCalcMethodFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum PaymentDetailTypePaymentAmountRefundCalcMethod

        '''<remarks/>
        System

        '''<remarks/>
        Manual
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CommissionType

        Private uniqueIDField As UniqueID_Type

        Private commissionableAmountField As CommissionTypeCommissionableAmount

        Private prepaidAmountField As CommissionTypePrepaidAmount

        Private flatCommissionField As CommissionTypeFlatCommission

        Private commissionPayableAmountField As CommissionTypeCommissionPayableAmount

        Private commentField As ParagraphType

        Private statusTypeField As CommissionTypeStatusType

        Private statusTypeFieldSpecified As Boolean

        Private percentField As Decimal

        Private percentFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        Private reasonCodeField As String

        Private billToIDField As String

        Private frequencyField As String

        Private maxCommissionUnitAppliesField As Long

        Private maxCommissionUnitAppliesFieldSpecified As Boolean

        Private capAmountField As Decimal

        Private capAmountFieldSpecified As Boolean

        '''<remarks/>
        Public Property UniqueID() As UniqueID_Type
            Get
                Return Me.uniqueIDField
            End Get
            Set
                Me.uniqueIDField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property CommissionableAmount() As CommissionTypeCommissionableAmount
            Get
                Return Me.commissionableAmountField
            End Get
            Set
                Me.commissionableAmountField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property PrepaidAmount() As CommissionTypePrepaidAmount
            Get
                Return Me.prepaidAmountField
            End Get
            Set
                Me.prepaidAmountField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property FlatCommission() As CommissionTypeFlatCommission
            Get
                Return Me.flatCommissionField
            End Get
            Set
                Me.flatCommissionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property CommissionPayableAmount() As CommissionTypeCommissionPayableAmount
            Get
                Return Me.commissionPayableAmountField
            End Get
            Set
                Me.commissionPayableAmountField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Comment() As ParagraphType
            Get
                Return Me.commentField
            End Get
            Set
                Me.commentField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property StatusType() As CommissionTypeStatusType
            Get
                Return Me.statusTypeField
            End Get
            Set
                Me.statusTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StatusTypeSpecified() As Boolean
            Get
                Return Me.statusTypeFieldSpecified
            End Get
            Set
                Me.statusTypeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Percent() As Decimal
            Get
                Return Me.percentField
            End Get
            Set
                Me.percentField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PercentSpecified() As Boolean
            Get
                Return Me.percentFieldSpecified
            End Get
            Set
                Me.percentFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ReasonCode() As String
            Get
                Return Me.reasonCodeField
            End Get
            Set
                Me.reasonCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property BillToID() As String
            Get
                Return Me.billToIDField
            End Get
            Set
                Me.billToIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Frequency() As String
            Get
                Return Me.frequencyField
            End Get
            Set
                Me.frequencyField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MaxCommissionUnitApplies() As Long
            Get
                Return Me.maxCommissionUnitAppliesField
            End Get
            Set
                Me.maxCommissionUnitAppliesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MaxCommissionUnitAppliesSpecified() As Boolean
            Get
                Return Me.maxCommissionUnitAppliesFieldSpecified
            End Get
            Set
                Me.maxCommissionUnitAppliesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CapAmount() As Decimal
            Get
                Return Me.capAmountField
            End Get
            Set
                Me.capAmountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property CapAmountSpecified() As Boolean
            Get
                Return Me.capAmountFieldSpecified
            End Get
            Set
                Me.capAmountFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CommissionTypeCommissionableAmount

        Private amountField As Decimal

        Private amountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        Private taxInclusiveIndicatorField As Boolean

        Private taxInclusiveIndicatorFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TaxInclusiveIndicator() As Boolean
            Get
                Return Me.taxInclusiveIndicatorField
            End Get
            Set
                Me.taxInclusiveIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TaxInclusiveIndicatorSpecified() As Boolean
            Get
                Return Me.taxInclusiveIndicatorFieldSpecified
            End Get
            Set
                Me.taxInclusiveIndicatorFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CommissionTypePrepaidAmount

        Private amountField As Decimal

        Private amountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CommissionTypeFlatCommission

        Private amountField As Decimal

        Private amountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CommissionTypeCommissionPayableAmount

        Private amountField As Decimal

        Private amountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(DescriptionType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class ParagraphType

        Private itemsField() As Object

        Private itemsElementNameField() As ItemsChoiceType

        Private nameField As String

        Private paragraphNumberField As Long

        Private paragraphNumberFieldSpecified As Boolean

        Private createDateTimeField As Date

        Private createDateTimeFieldSpecified As Boolean

        Private creatorIDField As String

        Private lastModifyDateTimeField As Date

        Private lastModifyDateTimeFieldSpecified As Boolean

        Private lastModifierIDField As String

        Private purgeDateField As Date

        Private purgeDateFieldSpecified As Boolean

        Private languageField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Image", GetType(String)),
            System.Xml.Serialization.XmlElementAttribute("ListItem", GetType(ParagraphTypeListItem)),
            System.Xml.Serialization.XmlElementAttribute("Text", GetType(FormattedTextTextType)),
            System.Xml.Serialization.XmlElementAttribute("URL", GetType(String), DataType:="anyURI"),
            System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")>
        Public Property Items() As Object()
            Get
                Return Me.itemsField
            End Get
            Set
                Me.itemsField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ItemsElementName"),
            System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ItemsElementName() As ItemsChoiceType()
            Get
                Return Me.itemsElementNameField
            End Get
            Set
                Me.itemsElementNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Name() As String
            Get
                Return Me.nameField
            End Get
            Set
                Me.nameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ParagraphNumber() As Long
            Get
                Return Me.paragraphNumberField
            End Get
            Set
                Me.paragraphNumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ParagraphNumberSpecified() As Boolean
            Get
                Return Me.paragraphNumberFieldSpecified
            End Get
            Set
                Me.paragraphNumberFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CreateDateTime() As Date
            Get
                Return Me.createDateTimeField
            End Get
            Set
                Me.createDateTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property CreateDateTimeSpecified() As Boolean
            Get
                Return Me.createDateTimeFieldSpecified
            End Get
            Set
                Me.createDateTimeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CreatorID() As String
            Get
                Return Me.creatorIDField
            End Get
            Set
                Me.creatorIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property LastModifyDateTime() As Date
            Get
                Return Me.lastModifyDateTimeField
            End Get
            Set
                Me.lastModifyDateTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property LastModifyDateTimeSpecified() As Boolean
            Get
                Return Me.lastModifyDateTimeFieldSpecified
            End Get
            Set
                Me.lastModifyDateTimeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property LastModifierID() As String
            Get
                Return Me.lastModifierIDField
            End Get
            Set
                Me.lastModifierIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property PurgeDate() As Date
            Get
                Return Me.purgeDateField
            End Get
            Set
                Me.purgeDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PurgeDateSpecified() As Boolean
            Get
                Return Me.purgeDateFieldSpecified
            End Get
            Set
                Me.purgeDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")>
        Public Property Language() As String
            Get
                Return Me.languageField
            End Get
            Set
                Me.languageField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class ParagraphTypeListItem
        Inherits FormattedTextTextType

        Private listItemField As Long

        Private listItemFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ListItem() As Long
            Get
                Return Me.listItemField
            End Get
            Set
                Me.listItemField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ListItemSpecified() As Boolean
            Get
                Return Me.listItemFieldSpecified
            End Get
            Set
                Me.listItemFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(PkgCautionType)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(CoverageDetailsType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class FormattedTextTextType

        Private formattedField As Boolean

        Private formattedFieldSpecified As Boolean

        Private languageField As String

        Private textFormatField As FormattedTextTextTypeTextFormat

        Private textFormatFieldSpecified As Boolean

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Formatted() As Boolean
            Get
                Return Me.formattedField
            End Get
            Set
                Me.formattedField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property FormattedSpecified() As Boolean
            Get
                Return Me.formattedFieldSpecified
            End Get
            Set
                Me.formattedFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")>
        Public Property Language() As String
            Get
                Return Me.languageField
            End Get
            Set
                Me.languageField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TextFormat() As FormattedTextTextTypeTextFormat
            Get
                Return Me.textFormatField
            End Get
            Set
                Me.textFormatField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TextFormatSpecified() As Boolean
            Get
                Return Me.textFormatFieldSpecified
            End Get
            Set
                Me.textFormatFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum FormattedTextTextTypeTextFormat

        '''<remarks/>
        PlainText

        '''<remarks/>
        HTML
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PkgCautionType
        Inherits FormattedTextTextType

        Private startField As String

        Private durationField As String

        Private endField As String

        Private typeField As String

        Private idField As String

        Private listOfItineraryItemRPHField() As String

        Private listOfExtraRPHField() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Start() As String
            Get
                Return Me.startField
            End Get
            Set
                Me.startField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Duration() As String
            Get
                Return Me.durationField
            End Get
            Set
                Me.durationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property [End]() As String
            Get
                Return Me.endField
            End Get
            Set
                Me.endField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ID() As String
            Get
                Return Me.idField
            End Get
            Set
                Me.idField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ListOfItineraryItemRPH() As String()
            Get
                Return Me.listOfItineraryItemRPHField
            End Get
            Set
                Me.listOfItineraryItemRPHField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ListOfExtraRPH() As String()
            Get
                Return Me.listOfExtraRPHField
            End Get
            Set
                Me.listOfExtraRPHField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CoverageDetailsType
        Inherits FormattedTextTextType

        Private coverageTextTypeField As CoverageTextType

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CoverageTextType() As CoverageTextType
            Get
                Return Me.coverageTextTypeField
            End Get
            Set
                Me.coverageTextTypeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum CoverageTextType

        '''<remarks/>
        Supplement

        '''<remarks/>
        Description

        '''<remarks/>
        Limits
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A", IncludeInSchema:=False)>
    Public Enum ItemsChoiceType

        '''<remarks/>
        Image

        '''<remarks/>
        ListItem

        '''<remarks/>
        Text

        '''<remarks/>
        URL
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class DescriptionType
        Inherits ParagraphType

        Private locationField As Boolean

        Private locationFieldSpecified As Boolean

        Private refDirectionToField As Boolean

        Private refDirectionToFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Location() As Boolean
            Get
                Return Me.locationField
            End Get
            Set
                Me.locationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property LocationSpecified() As Boolean
            Get
                Return Me.locationFieldSpecified
            End Get
            Set
                Me.locationFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RefDirectionTo() As Boolean
            Get
                Return Me.refDirectionToField
            End Get
            Set
                Me.refDirectionToField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RefDirectionToSpecified() As Boolean
            Get
                Return Me.refDirectionToFieldSpecified
            End Get
            Set
                Me.refDirectionToFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum CommissionTypeStatusType

        '''<remarks/>
        Full

        '''<remarks/>
        [Partial]

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("Non-paying")>
        Nonpaying

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("No-show")>
        Noshow

        '''<remarks/>
        Adjustment

        '''<remarks/>
        Commissionable
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class RelatedTravelerType

        Private uniqueIDField As UniqueID_Type

        Private personNameField As PersonNameType

        Private shareSynchIndField As PersonNameTypeShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As PersonNameTypeShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private relationField As String

        Private birthDateField As Date

        Private birthDateFieldSpecified As Boolean

        '''<remarks/>
        Public Property UniqueID() As UniqueID_Type
            Get
                Return Me.uniqueIDField
            End Get
            Set
                Me.uniqueIDField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property PersonName() As PersonNameType
            Get
                Return Me.personNameField
            End Get
            Set
                Me.personNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As PersonNameTypeShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As PersonNameTypeShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Relation() As String
            Get
                Return Me.relationField
            End Get
            Set
                Me.relationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property BirthDate() As Date
            Get
                Return Me.birthDateField
            End Get
            Set
                Me.birthDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property BirthDateSpecified() As Boolean
            Get
                Return Me.birthDateFieldSpecified
            End Get
            Set
                Me.birthDateFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class ContactPersonType

        Private personNameField As PersonNameType

        Private telephoneField() As ContactPersonTypeTelephone

        Private addressField() As AddressInfoType

        Private emailField() As EmailType

        Private uRLField() As URL_Type

        Private companyNameField() As CompanyNameType

        Private employeeInfoField() As EmployeeInfoType

        Private shareSynchIndField As PersonNameTypeShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As PersonNameTypeShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private defaultIndField As Boolean

        Private defaultIndFieldSpecified As Boolean

        Private contactTypeField As String

        Private relationField As String

        Private emergencyFlagField As Boolean

        Private emergencyFlagFieldSpecified As Boolean

        Private rPHField As String

        Private communicationMethodCodeField As String

        Private documentDistribMethodCodeField As String

        '''<remarks/>
        Public Property PersonName() As PersonNameType
            Get
                Return Me.personNameField
            End Get
            Set
                Me.personNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Telephone")>
        Public Property Telephone() As ContactPersonTypeTelephone()
            Get
                Return Me.telephoneField
            End Get
            Set
                Me.telephoneField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Address")>
        Public Property Address() As AddressInfoType()
            Get
                Return Me.addressField
            End Get
            Set
                Me.addressField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Email")>
        Public Property Email() As EmailType()
            Get
                Return Me.emailField
            End Get
            Set
                Me.emailField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("URL")>
        Public Property URL() As URL_Type()
            Get
                Return Me.uRLField
            End Get
            Set
                Me.uRLField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CompanyName")>
        Public Property CompanyName() As CompanyNameType()
            Get
                Return Me.companyNameField
            End Get
            Set
                Me.companyNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("EmployeeInfo")>
        Public Property EmployeeInfo() As EmployeeInfoType()
            Get
                Return Me.employeeInfoField
            End Get
            Set
                Me.employeeInfoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As PersonNameTypeShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As PersonNameTypeShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DefaultInd() As Boolean
            Get
                Return Me.defaultIndField
            End Get
            Set
                Me.defaultIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DefaultIndSpecified() As Boolean
            Get
                Return Me.defaultIndFieldSpecified
            End Get
            Set
                Me.defaultIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ContactType() As String
            Get
                Return Me.contactTypeField
            End Get
            Set
                Me.contactTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Relation() As String
            Get
                Return Me.relationField
            End Get
            Set
                Me.relationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property EmergencyFlag() As Boolean
            Get
                Return Me.emergencyFlagField
            End Get
            Set
                Me.emergencyFlagField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EmergencyFlagSpecified() As Boolean
            Get
                Return Me.emergencyFlagFieldSpecified
            End Get
            Set
                Me.emergencyFlagFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RPH() As String
            Get
                Return Me.rPHField
            End Get
            Set
                Me.rPHField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CommunicationMethodCode() As String
            Get
                Return Me.communicationMethodCodeField
            End Get
            Set
                Me.communicationMethodCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DocumentDistribMethodCode() As String
            Get
                Return Me.documentDistribMethodCodeField
            End Get
            Set
                Me.documentDistribMethodCodeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class ContactPersonTypeTelephone

        Private shareSynchIndField As CustomerTypeTelephoneShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As CustomerTypeTelephoneShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private phoneLocationTypeField As String

        Private phoneTechTypeField As String

        Private phoneUseTypeField As String

        Private countryAccessCodeField As String

        Private areaCityCodeField As String

        Private phoneNumberField As String

        Private extensionField As String

        Private pINField As String

        Private remarkField As String

        Private formattedIndField As Boolean

        Private formattedIndFieldSpecified As Boolean

        Private defaultIndField As Boolean

        Private defaultIndFieldSpecified As Boolean

        Private rPHField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As CustomerTypeTelephoneShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As CustomerTypeTelephoneShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneLocationType() As String
            Get
                Return Me.phoneLocationTypeField
            End Get
            Set
                Me.phoneLocationTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneTechType() As String
            Get
                Return Me.phoneTechTypeField
            End Get
            Set
                Me.phoneTechTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneUseType() As String
            Get
                Return Me.phoneUseTypeField
            End Get
            Set
                Me.phoneUseTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CountryAccessCode() As String
            Get
                Return Me.countryAccessCodeField
            End Get
            Set
                Me.countryAccessCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AreaCityCode() As String
            Get
                Return Me.areaCityCodeField
            End Get
            Set
                Me.areaCityCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneNumber() As String
            Get
                Return Me.phoneNumberField
            End Get
            Set
                Me.phoneNumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Extension() As String
            Get
                Return Me.extensionField
            End Get
            Set
                Me.extensionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PIN() As String
            Get
                Return Me.pINField
            End Get
            Set
                Me.pINField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Remark() As String
            Get
                Return Me.remarkField
            End Get
            Set
                Me.remarkField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property FormattedInd() As Boolean
            Get
                Return Me.formattedIndField
            End Get
            Set
                Me.formattedIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property FormattedIndSpecified() As Boolean
            Get
                Return Me.formattedIndFieldSpecified
            End Get
            Set
                Me.formattedIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DefaultInd() As Boolean
            Get
                Return Me.defaultIndField
            End Get
            Set
                Me.defaultIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DefaultIndSpecified() As Boolean
            Get
                Return Me.defaultIndFieldSpecified
            End Get
            Set
                Me.defaultIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RPH() As String
            Get
                Return Me.rPHField
            End Get
            Set
                Me.rPHField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class EmployeeInfoType

        Private employeeIdField As String

        Private employeeLevelField As String

        Private employeeTitleField As String

        Private employeeStatusField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property EmployeeId() As String
            Get
                Return Me.employeeIdField
            End Get
            Set
                Me.employeeIdField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property EmployeeLevel() As String
            Get
                Return Me.employeeLevelField
            End Get
            Set
                Me.employeeLevelField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property EmployeeTitle() As String
            Get
                Return Me.employeeTitleField
            End Get
            Set
                Me.employeeTitleField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property EmployeeStatus() As String
            Get
                Return Me.employeeStatusField
            End Get
            Set
                Me.employeeStatusField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class DocumentType

        Private itemField As Object

        Private docLimitationsField() As String

        Private additionalPersonNamesField() As String

        Private shareSynchIndField As PersonNameTypeShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As PersonNameTypeShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private docIssueAuthorityField As String

        Private docIssueLocationField As String

        Private docIDField As String

        Private docTypeField As String

        Private genderField As DocumentTypeGender

        Private genderFieldSpecified As Boolean

        Private birthDateField As Date

        Private birthDateFieldSpecified As Boolean

        Private effectiveDateField As Date

        Private effectiveDateFieldSpecified As Boolean

        Private expireDateField As Date

        Private expireDateFieldSpecified As Boolean

        Private expireDateExclusiveIndicatorField As Boolean

        Private expireDateExclusiveIndicatorFieldSpecified As Boolean

        Private docIssueStateProvField As String

        Private docIssueCountryField As String

        Private birthCountryField As String

        Private birthPlaceField As String

        Private docHolderNationalityField As String

        Private contactNameField As String

        Private holderTypeField As DocumentTypeHolderType

        Private holderTypeFieldSpecified As Boolean

        Private remarkField As String

        Private postalCodeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("DocHolderFormattedName", GetType(PersonNameType)),
            System.Xml.Serialization.XmlElementAttribute("DocHolderName", GetType(String))>
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set
                Me.itemField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("DocLimitations")>
        Public Property DocLimitations() As String()
            Get
                Return Me.docLimitationsField
            End Get
            Set
                Me.docLimitationsField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("AdditionalPersonName", IsNullable:=False)>
        Public Property AdditionalPersonNames() As String()
            Get
                Return Me.additionalPersonNamesField
            End Get
            Set
                Me.additionalPersonNamesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As PersonNameTypeShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As PersonNameTypeShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DocIssueAuthority() As String
            Get
                Return Me.docIssueAuthorityField
            End Get
            Set
                Me.docIssueAuthorityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DocIssueLocation() As String
            Get
                Return Me.docIssueLocationField
            End Get
            Set
                Me.docIssueLocationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DocID() As String
            Get
                Return Me.docIDField
            End Get
            Set
                Me.docIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DocType() As String
            Get
                Return Me.docTypeField
            End Get
            Set
                Me.docTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Gender() As DocumentTypeGender
            Get
                Return Me.genderField
            End Get
            Set
                Me.genderField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property GenderSpecified() As Boolean
            Get
                Return Me.genderFieldSpecified
            End Get
            Set
                Me.genderFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property BirthDate() As Date
            Get
                Return Me.birthDateField
            End Get
            Set
                Me.birthDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property BirthDateSpecified() As Boolean
            Get
                Return Me.birthDateFieldSpecified
            End Get
            Set
                Me.birthDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property EffectiveDate() As Date
            Get
                Return Me.effectiveDateField
            End Get
            Set
                Me.effectiveDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EffectiveDateSpecified() As Boolean
            Get
                Return Me.effectiveDateFieldSpecified
            End Get
            Set
                Me.effectiveDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property ExpireDate() As Date
            Get
                Return Me.expireDateField
            End Get
            Set
                Me.expireDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateSpecified() As Boolean
            Get
                Return Me.expireDateFieldSpecified
            End Get
            Set
                Me.expireDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExpireDateExclusiveIndicator() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorField
            End Get
            Set
                Me.expireDateExclusiveIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateExclusiveIndicatorSpecified() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorFieldSpecified
            End Get
            Set
                Me.expireDateExclusiveIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DocIssueStateProv() As String
            Get
                Return Me.docIssueStateProvField
            End Get
            Set
                Me.docIssueStateProvField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DocIssueCountry() As String
            Get
                Return Me.docIssueCountryField
            End Get
            Set
                Me.docIssueCountryField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property BirthCountry() As String
            Get
                Return Me.birthCountryField
            End Get
            Set
                Me.birthCountryField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property BirthPlace() As String
            Get
                Return Me.birthPlaceField
            End Get
            Set
                Me.birthPlaceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DocHolderNationality() As String
            Get
                Return Me.docHolderNationalityField
            End Get
            Set
                Me.docHolderNationalityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ContactName() As String
            Get
                Return Me.contactNameField
            End Get
            Set
                Me.contactNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property HolderType() As DocumentTypeHolderType
            Get
                Return Me.holderTypeField
            End Get
            Set
                Me.holderTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property HolderTypeSpecified() As Boolean
            Get
                Return Me.holderTypeFieldSpecified
            End Get
            Set
                Me.holderTypeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Remark() As String
            Get
                Return Me.remarkField
            End Get
            Set
                Me.remarkField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PostalCode() As String
            Get
                Return Me.postalCodeField
            End Get
            Set
                Me.postalCodeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum DocumentTypeGender

        '''<remarks/>
        Male

        '''<remarks/>
        Female

        '''<remarks/>
        Unknown

        '''<remarks/>
        Male_NoShare

        '''<remarks/>
        Female_NoShare
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum DocumentTypeHolderType

        '''<remarks/>
        Infant

        '''<remarks/>
        HeadOfHousehold
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypeCustLoyalty

        Private memberPreferencesField As CustomerTypeCustLoyaltyMemberPreferences

        Private securityInfoField As CustomerTypeCustLoyaltySecurityInfo

        Private subAccountBalanceField() As CustomerTypeCustLoyaltySubAccountBalance

        Private shareSynchIndField As PaymentCardTypeCustLoyaltyShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As PaymentCardTypeCustLoyaltyShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private programIDField As String

        Private membershipIDField As String

        Private travelSectorField As String

        Private loyalLevelField As String

        Private loyalLevelCodeField As String

        Private singleVendorIndField As PaymentCardTypeCustLoyaltySingleVendorInd

        Private singleVendorIndFieldSpecified As Boolean

        Private signupDateField As Date

        Private signupDateFieldSpecified As Boolean

        Private effectiveDateField As Date

        Private effectiveDateFieldSpecified As Boolean

        Private expireDateField As Date

        Private expireDateFieldSpecified As Boolean

        Private expireDateExclusiveIndicatorField As Boolean

        Private expireDateExclusiveIndicatorFieldSpecified As Boolean

        Private rPHField As String

        Private vendorCodeField() As String

        Private primaryLoyaltyIndicatorField As Boolean

        Private primaryLoyaltyIndicatorFieldSpecified As Boolean

        Private allianceLoyaltyLevelNameField As String

        Private customerTypeField As String

        Private customerValueField As String

        Private passwordField As String

        Private remarkField As String

        '''<remarks/>
        Public Property MemberPreferences() As CustomerTypeCustLoyaltyMemberPreferences
            Get
                Return Me.memberPreferencesField
            End Get
            Set
                Me.memberPreferencesField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property SecurityInfo() As CustomerTypeCustLoyaltySecurityInfo
            Get
                Return Me.securityInfoField
            End Get
            Set
                Me.securityInfoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SubAccountBalance")>
        Public Property SubAccountBalance() As CustomerTypeCustLoyaltySubAccountBalance()
            Get
                Return Me.subAccountBalanceField
            End Get
            Set
                Me.subAccountBalanceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As PaymentCardTypeCustLoyaltyShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As PaymentCardTypeCustLoyaltyShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ProgramID() As String
            Get
                Return Me.programIDField
            End Get
            Set
                Me.programIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MembershipID() As String
            Get
                Return Me.membershipIDField
            End Get
            Set
                Me.membershipIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TravelSector() As String
            Get
                Return Me.travelSectorField
            End Get
            Set
                Me.travelSectorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property LoyalLevel() As String
            Get
                Return Me.loyalLevelField
            End Get
            Set
                Me.loyalLevelField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property LoyalLevelCode() As String
            Get
                Return Me.loyalLevelCodeField
            End Get
            Set
                Me.loyalLevelCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SingleVendorInd() As PaymentCardTypeCustLoyaltySingleVendorInd
            Get
                Return Me.singleVendorIndField
            End Get
            Set
                Me.singleVendorIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SingleVendorIndSpecified() As Boolean
            Get
                Return Me.singleVendorIndFieldSpecified
            End Get
            Set
                Me.singleVendorIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property SignupDate() As Date
            Get
                Return Me.signupDateField
            End Get
            Set
                Me.signupDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SignupDateSpecified() As Boolean
            Get
                Return Me.signupDateFieldSpecified
            End Get
            Set
                Me.signupDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property EffectiveDate() As Date
            Get
                Return Me.effectiveDateField
            End Get
            Set
                Me.effectiveDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EffectiveDateSpecified() As Boolean
            Get
                Return Me.effectiveDateFieldSpecified
            End Get
            Set
                Me.effectiveDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property ExpireDate() As Date
            Get
                Return Me.expireDateField
            End Get
            Set
                Me.expireDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateSpecified() As Boolean
            Get
                Return Me.expireDateFieldSpecified
            End Get
            Set
                Me.expireDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExpireDateExclusiveIndicator() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorField
            End Get
            Set
                Me.expireDateExclusiveIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateExclusiveIndicatorSpecified() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorFieldSpecified
            End Get
            Set
                Me.expireDateExclusiveIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RPH() As String
            Get
                Return Me.rPHField
            End Get
            Set
                Me.rPHField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property VendorCode() As String()
            Get
                Return Me.vendorCodeField
            End Get
            Set
                Me.vendorCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PrimaryLoyaltyIndicator() As Boolean
            Get
                Return Me.primaryLoyaltyIndicatorField
            End Get
            Set
                Me.primaryLoyaltyIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PrimaryLoyaltyIndicatorSpecified() As Boolean
            Get
                Return Me.primaryLoyaltyIndicatorFieldSpecified
            End Get
            Set
                Me.primaryLoyaltyIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AllianceLoyaltyLevelName() As String
            Get
                Return Me.allianceLoyaltyLevelNameField
            End Get
            Set
                Me.allianceLoyaltyLevelNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CustomerType() As String
            Get
                Return Me.customerTypeField
            End Get
            Set
                Me.customerTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CustomerValue() As String
            Get
                Return Me.customerValueField
            End Get
            Set
                Me.customerValueField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Password() As String
            Get
                Return Me.passwordField
            End Get
            Set
                Me.passwordField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Remark() As String
            Get
                Return Me.remarkField
            End Get
            Set
                Me.remarkField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypeCustLoyaltyMemberPreferences

        Private additionalRewardField() As CustomerTypeCustLoyaltyMemberPreferencesAdditionalReward

        Private offerField() As CustomerTypeCustLoyaltyMemberPreferencesOffer

        Private awarenessField As String

        Private promotionCodeField As String

        Private promotionVendorCodeField() As String

        Private awardsPreferenceField As CustomerTypeCustLoyaltyMemberPreferencesAwardsPreference

        Private awardsPreferenceFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AdditionalReward")>
        Public Property AdditionalReward() As CustomerTypeCustLoyaltyMemberPreferencesAdditionalReward()
            Get
                Return Me.additionalRewardField
            End Get
            Set
                Me.additionalRewardField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Offer")>
        Public Property Offer() As CustomerTypeCustLoyaltyMemberPreferencesOffer()
            Get
                Return Me.offerField
            End Get
            Set
                Me.offerField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Awareness() As String
            Get
                Return Me.awarenessField
            End Get
            Set
                Me.awarenessField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PromotionCode() As String
            Get
                Return Me.promotionCodeField
            End Get
            Set
                Me.promotionCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PromotionVendorCode() As String()
            Get
                Return Me.promotionVendorCodeField
            End Get
            Set
                Me.promotionVendorCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AwardsPreference() As CustomerTypeCustLoyaltyMemberPreferencesAwardsPreference
            Get
                Return Me.awardsPreferenceField
            End Get
            Set
                Me.awardsPreferenceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AwardsPreferenceSpecified() As Boolean
            Get
                Return Me.awardsPreferenceFieldSpecified
            End Get
            Set
                Me.awardsPreferenceFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypeCustLoyaltyMemberPreferencesAdditionalReward

        Private companyNameField As CompanyNameType

        Private nameField As PersonNameType

        Private memberIDField As String

        '''<remarks/>
        Public Property CompanyName() As CompanyNameType
            Get
                Return Me.companyNameField
            End Get
            Set
                Me.companyNameField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Name() As PersonNameType
            Get
                Return Me.nameField
            End Get
            Set
                Me.nameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MemberID() As String
            Get
                Return Me.memberIDField
            End Get
            Set
                Me.memberIDField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypeCustLoyaltyMemberPreferencesOffer

        Private communicationField() As CustomerTypeCustLoyaltyMemberPreferencesOfferCommunication

        Private typeField As CustomerTypeCustLoyaltyMemberPreferencesOfferType

        Private typeFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Communication")>
        Public Property Communication() As CustomerTypeCustLoyaltyMemberPreferencesOfferCommunication()
            Get
                Return Me.communicationField
            End Get
            Set
                Me.communicationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As CustomerTypeCustLoyaltyMemberPreferencesOfferType
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TypeSpecified() As Boolean
            Get
                Return Me.typeFieldSpecified
            End Get
            Set
                Me.typeFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypeCustLoyaltyMemberPreferencesOfferCommunication

        Private distribTypeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DistribType() As String
            Get
                Return Me.distribTypeField
            End Get
            Set
                Me.distribTypeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum CustomerTypeCustLoyaltyMemberPreferencesOfferType

        '''<remarks/>
        Partner

        '''<remarks/>
        Loyalty
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum CustomerTypeCustLoyaltyMemberPreferencesAwardsPreference

        '''<remarks/>
        Points

        '''<remarks/>
        Miles
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypeCustLoyaltySecurityInfo

        Private passwordHintField() As CustomerTypeCustLoyaltySecurityInfoPasswordHint

        Private usernameField As String

        Private passwordField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PasswordHint")>
        Public Property PasswordHint() As CustomerTypeCustLoyaltySecurityInfoPasswordHint()
            Get
                Return Me.passwordHintField
            End Get
            Set
                Me.passwordHintField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Username() As String
            Get
                Return Me.usernameField
            End Get
            Set
                Me.usernameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Password() As String
            Get
                Return Me.passwordField
            End Get
            Set
                Me.passwordField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypeCustLoyaltySecurityInfoPasswordHint

        Private hintField As CustomerTypeCustLoyaltySecurityInfoPasswordHintHint

        Private hintFieldSpecified As Boolean

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Hint() As CustomerTypeCustLoyaltySecurityInfoPasswordHintHint
            Get
                Return Me.hintField
            End Get
            Set
                Me.hintField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property HintSpecified() As Boolean
            Get
                Return Me.hintFieldSpecified
            End Get
            Set
                Me.hintFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum CustomerTypeCustLoyaltySecurityInfoPasswordHintHint

        '''<remarks/>
        Question

        '''<remarks/>
        Answer
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypeCustLoyaltySubAccountBalance

        Private typeField As String

        Private balanceField As Long

        Private balanceFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Balance() As Long
            Get
                Return Me.balanceField
            End Get
            Set
                Me.balanceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property BalanceSpecified() As Boolean
            Get
                Return Me.balanceFieldSpecified
            End Get
            Set
                Me.balanceFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerTypeAdditionalLanguage

        Private codeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum CustomerTypeMaritalStatus

        '''<remarks/>
        Annulled

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("Co-habitating")>
        Cohabitating

        '''<remarks/>
        Divorced

        '''<remarks/>
        Engaged

        '''<remarks/>
        Married

        '''<remarks/>
        Separated

        '''<remarks/>
        [Single]

        '''<remarks/>
        Widowed

        '''<remarks/>
        Unknown
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class InsuranceCustomerType
        Inherits CustomerType

        Private idField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ID() As String
            Get
                Return Me.idField
            End Get
            Set
                Me.idField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CustomerPrimaryAdditionalTypeAdditional
        Inherits CustomerType

        Private startField As String

        Private durationField As String

        Private endField As String

        Private corpDiscountNameField As String

        Private corpDiscountNmbrField As String

        Private qualificationMethodField As CustomerPrimaryAdditionalTypeAdditionalQualificationMethod

        Private qualificationMethodFieldSpecified As Boolean

        Private ageField As String

        Private codeField As String

        Private codeContextField As String

        Private uRIField As String

        Private quantityField As Long

        Private quantityFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Start() As String
            Get
                Return Me.startField
            End Get
            Set
                Me.startField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Duration() As String
            Get
                Return Me.durationField
            End Get
            Set
                Me.durationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property [End]() As String
            Get
                Return Me.endField
            End Get
            Set
                Me.endField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CorpDiscountName() As String
            Get
                Return Me.corpDiscountNameField
            End Get
            Set
                Me.corpDiscountNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CorpDiscountNmbr() As String
            Get
                Return Me.corpDiscountNmbrField
            End Get
            Set
                Me.corpDiscountNmbrField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property QualificationMethod() As CustomerPrimaryAdditionalTypeAdditionalQualificationMethod
            Get
                Return Me.qualificationMethodField
            End Get
            Set
                Me.qualificationMethodField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property QualificationMethodSpecified() As Boolean
            Get
                Return Me.qualificationMethodFieldSpecified
            End Get
            Set
                Me.qualificationMethodFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property Age() As String
            Get
                Return Me.ageField
            End Get
            Set
                Me.ageField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CodeContext() As String
            Get
                Return Me.codeContextField
            End Get
            Set
                Me.codeContextField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")>
        Public Property URI() As String
            Get
                Return Me.uRIField
            End Get
            Set
                Me.uRIField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Quantity() As Long
            Get
                Return Me.quantityField
            End Get
            Set
                Me.quantityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property QuantitySpecified() As Boolean
            Get
                Return Me.quantityFieldSpecified
            End Get
            Set
                Me.quantityFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum CustomerPrimaryAdditionalTypeAdditionalQualificationMethod

        '''<remarks/>
        RT_AirlineTicket

        '''<remarks/>
        CreditCard

        '''<remarks/>
        PassportAndReturnTkt
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleReservationTypeVehSegmentCore
        Inherits VehicleSegmentCoreType

        Private optionChangeAllowedIndicatorField As Boolean

        Private optionChangeAllowedIndicatorFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OptionChangeAllowedIndicator() As Boolean
            Get
                Return Me.optionChangeAllowedIndicatorField
            End Get
            Set
                Me.optionChangeAllowedIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OptionChangeAllowedIndicatorSpecified() As Boolean
            Get
                Return Me.optionChangeAllowedIndicatorFieldSpecified
            End Get
            Set
                Me.optionChangeAllowedIndicatorFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleSegmentCoreType

        Private confIDField() As VehicleSegmentCoreTypeConfID

        Private vendorField As CompanyNameType

        Private vehRentalCoreField As VehicleRentalCoreType

        Private vehicleField As VehicleType

        Private rentalRateField As VehicleRentalRateType

        Private pricedEquipsField() As VehicleEquipmentPricedType

        Private feesField() As VehicleChargePurposeType

        Private totalChargeField As VehicleSegmentCoreTypeTotalCharge

        Private tPA_ExtensionsField As TPA_ExtensionsType

        Private indexNumberField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ConfID")>
        Public Property ConfID() As VehicleSegmentCoreTypeConfID()
            Get
                Return Me.confIDField
            End Get
            Set
                Me.confIDField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Vendor() As CompanyNameType
            Get
                Return Me.vendorField
            End Get
            Set
                Me.vendorField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property VehRentalCore() As VehicleRentalCoreType
            Get
                Return Me.vehRentalCoreField
            End Get
            Set
                Me.vehRentalCoreField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Vehicle() As VehicleType
            Get
                Return Me.vehicleField
            End Get
            Set
                Me.vehicleField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property RentalRate() As VehicleRentalRateType
            Get
                Return Me.rentalRateField
            End Get
            Set
                Me.rentalRateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("PricedEquip", IsNullable:=False)>
        Public Property PricedEquips() As VehicleEquipmentPricedType()
            Get
                Return Me.pricedEquipsField
            End Get
            Set
                Me.pricedEquipsField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Fee", IsNullable:=False)>
        Public Property Fees() As VehicleChargePurposeType()
            Get
                Return Me.feesField
            End Get
            Set
                Me.feesField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property TotalCharge() As VehicleSegmentCoreTypeTotalCharge
            Get
                Return Me.totalChargeField
            End Get
            Set
                Me.totalChargeField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property TPA_Extensions() As TPA_ExtensionsType
            Get
                Return Me.tPA_ExtensionsField
            End Get
            Set
                Me.tPA_ExtensionsField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property IndexNumber() As String
            Get
                Return Me.indexNumberField
            End Get
            Set
                Me.indexNumberField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleSegmentCoreTypeConfID
        Inherits UniqueID_Type

        Private statusField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Status() As String
            Get
                Return Me.statusField
            End Get
            Set
                Me.statusField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleRentalCoreType

        Private pickUpLocationField() As VehicleRentalCoreTypePickUpLocation

        Private returnLocationField As VehicleRentalCoreTypeReturnLocation

        Private pickUpDateTimeField As Date

        Private pickUpDateTimeFieldSpecified As Boolean

        Private returnDateTimeField As Date

        Private returnDateTimeFieldSpecified As Boolean

        Private startChargesDateTimeField As Date

        Private startChargesDateTimeFieldSpecified As Boolean

        Private stopChargesDateTimeField As Date

        Private stopChargesDateTimeFieldSpecified As Boolean

        Private oneWayIndicatorField As Boolean

        Private oneWayIndicatorFieldSpecified As Boolean

        Private multiIslandRentalDaysField As String

        Private quantityField As Long

        Private quantityFieldSpecified As Boolean

        Private distUnitNameField As DistanceUnitNameType

        Private distUnitNameFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PickUpLocation")>
        Public Property PickUpLocation() As VehicleRentalCoreTypePickUpLocation()
            Get
                Return Me.pickUpLocationField
            End Get
            Set
                Me.pickUpLocationField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property ReturnLocation() As VehicleRentalCoreTypeReturnLocation
            Get
                Return Me.returnLocationField
            End Get
            Set
                Me.returnLocationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PickUpDateTime() As Date
            Get
                Return Me.pickUpDateTimeField
            End Get
            Set
                Me.pickUpDateTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PickUpDateTimeSpecified() As Boolean
            Get
                Return Me.pickUpDateTimeFieldSpecified
            End Get
            Set
                Me.pickUpDateTimeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ReturnDateTime() As Date
            Get
                Return Me.returnDateTimeField
            End Get
            Set
                Me.returnDateTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ReturnDateTimeSpecified() As Boolean
            Get
                Return Me.returnDateTimeFieldSpecified
            End Get
            Set
                Me.returnDateTimeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property StartChargesDateTime() As Date
            Get
                Return Me.startChargesDateTimeField
            End Get
            Set
                Me.startChargesDateTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StartChargesDateTimeSpecified() As Boolean
            Get
                Return Me.startChargesDateTimeFieldSpecified
            End Get
            Set
                Me.startChargesDateTimeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property StopChargesDateTime() As Date
            Get
                Return Me.stopChargesDateTimeField
            End Get
            Set
                Me.stopChargesDateTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StopChargesDateTimeSpecified() As Boolean
            Get
                Return Me.stopChargesDateTimeFieldSpecified
            End Get
            Set
                Me.stopChargesDateTimeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OneWayIndicator() As Boolean
            Get
                Return Me.oneWayIndicatorField
            End Get
            Set
                Me.oneWayIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OneWayIndicatorSpecified() As Boolean
            Get
                Return Me.oneWayIndicatorFieldSpecified
            End Get
            Set
                Me.oneWayIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property MultiIslandRentalDays() As String
            Get
                Return Me.multiIslandRentalDaysField
            End Get
            Set
                Me.multiIslandRentalDaysField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Quantity() As Long
            Get
                Return Me.quantityField
            End Get
            Set
                Me.quantityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property QuantitySpecified() As Boolean
            Get
                Return Me.quantityFieldSpecified
            End Get
            Set
                Me.quantityFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DistUnitName() As DistanceUnitNameType
            Get
                Return Me.distUnitNameField
            End Get
            Set
                Me.distUnitNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DistUnitNameSpecified() As Boolean
            Get
                Return Me.distUnitNameFieldSpecified
            End Get
            Set
                Me.distUnitNameFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleRentalCoreTypePickUpLocation
        Inherits LocationType

        Private extendedLocationCodeField As String

        Private counterLocationField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExtendedLocationCode() As String
            Get
                Return Me.extendedLocationCodeField
            End Get
            Set
                Me.extendedLocationCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CounterLocation() As String
            Get
                Return Me.counterLocationField
            End Get
            Set
                Me.counterLocationField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(StationType)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(AirportPrefType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class LocationType

        Private locationCodeField As String

        Private codeContextField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property LocationCode() As String
            Get
                Return Me.locationCodeField
            End Get
            Set
                Me.locationCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CodeContext() As String
            Get
                Return Me.codeContextField
            End Get
            Set
                Me.codeContextField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class StationType
        Inherits LocationType

        Private isStaffedIndField As Boolean

        Private isStaffedIndFieldSpecified As Boolean

        Private ticketPrinterIndField As Boolean

        Private ticketPrinterIndFieldSpecified As Boolean

        Private sST_MachineIndField As Boolean

        Private sST_MachineIndFieldSpecified As Boolean

        Private timeZoneOffsetField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property IsStaffedInd() As Boolean
            Get
                Return Me.isStaffedIndField
            End Get
            Set
                Me.isStaffedIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property IsStaffedIndSpecified() As Boolean
            Get
                Return Me.isStaffedIndFieldSpecified
            End Get
            Set
                Me.isStaffedIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TicketPrinterInd() As Boolean
            Get
                Return Me.ticketPrinterIndField
            End Get
            Set
                Me.ticketPrinterIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TicketPrinterIndSpecified() As Boolean
            Get
                Return Me.ticketPrinterIndFieldSpecified
            End Get
            Set
                Me.ticketPrinterIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SST_MachineInd() As Boolean
            Get
                Return Me.sST_MachineIndField
            End Get
            Set
                Me.sST_MachineIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SST_MachineIndSpecified() As Boolean
            Get
                Return Me.sST_MachineIndFieldSpecified
            End Get
            Set
                Me.sST_MachineIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TimeZoneOffset() As String
            Get
                Return Me.timeZoneOffsetField
            End Get
            Set
                Me.timeZoneOffsetField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class AirportPrefType
        Inherits LocationType

        Private preferLevelField As PreferLevelType

        Private preferLevelFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PreferLevel() As PreferLevelType
            Get
                Return Me.preferLevelField
            End Get
            Set
                Me.preferLevelField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PreferLevelSpecified() As Boolean
            Get
                Return Me.preferLevelFieldSpecified
            End Get
            Set
                Me.preferLevelFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleRentalCoreTypeReturnLocation
        Inherits LocationType

        Private extendedLocationCodeField As String

        Private counterLocationField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExtendedLocationCode() As String
            Get
                Return Me.extendedLocationCodeField
            End Get
            Set
                Me.extendedLocationCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CounterLocation() As String
            Get
                Return Me.counterLocationField
            End Get
            Set
                Me.counterLocationField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum DistanceUnitNameType

        '''<remarks/>
        Mile

        '''<remarks/>
        Km

        '''<remarks/>
        Block
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleType
        Inherits VehicleCoreType

        Private vehMakeModelField As VehicleTypeVehMakeModel

        Private pictureURLField As String

        Private vehIdentityField As VehicleTypeVehIdentity

        Private passengerQuantityField As String

        Private baggageQuantityField As Long

        Private baggageQuantityFieldSpecified As Boolean

        Private vendorCarTypeField As String

        Private codeField As String

        Private codeContextField As String

        Private unitOfMeasureQuantityField As Decimal

        Private unitOfMeasureQuantityFieldSpecified As Boolean

        Private unitOfMeasureField As String

        Private unitOfMeasureCodeField As String

        Private startField As String

        Private durationField As String

        Private endField As String

        Private odometerUnitOfMeasureField As DistanceUnitNameType

        Private odometerUnitOfMeasureFieldSpecified As Boolean

        Private descriptionField As String

        '''<remarks/>
        Public Property VehMakeModel() As VehicleTypeVehMakeModel
            Get
                Return Me.vehMakeModelField
            End Get
            Set
                Me.vehMakeModelField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="anyURI")>
        Public Property PictureURL() As String
            Get
                Return Me.pictureURLField
            End Get
            Set
                Me.pictureURLField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property VehIdentity() As VehicleTypeVehIdentity
            Get
                Return Me.vehIdentityField
            End Get
            Set
                Me.vehIdentityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PassengerQuantity() As String
            Get
                Return Me.passengerQuantityField
            End Get
            Set
                Me.passengerQuantityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property BaggageQuantity() As Long
            Get
                Return Me.baggageQuantityField
            End Get
            Set
                Me.baggageQuantityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property BaggageQuantitySpecified() As Boolean
            Get
                Return Me.baggageQuantityFieldSpecified
            End Get
            Set
                Me.baggageQuantityFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property VendorCarType() As String
            Get
                Return Me.vendorCarTypeField
            End Get
            Set
                Me.vendorCarTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CodeContext() As String
            Get
                Return Me.codeContextField
            End Get
            Set
                Me.codeContextField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property UnitOfMeasureQuantity() As Decimal
            Get
                Return Me.unitOfMeasureQuantityField
            End Get
            Set
                Me.unitOfMeasureQuantityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property UnitOfMeasureQuantitySpecified() As Boolean
            Get
                Return Me.unitOfMeasureQuantityFieldSpecified
            End Get
            Set
                Me.unitOfMeasureQuantityFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property UnitOfMeasure() As String
            Get
                Return Me.unitOfMeasureField
            End Get
            Set
                Me.unitOfMeasureField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property UnitOfMeasureCode() As String
            Get
                Return Me.unitOfMeasureCodeField
            End Get
            Set
                Me.unitOfMeasureCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Start() As String
            Get
                Return Me.startField
            End Get
            Set
                Me.startField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Duration() As String
            Get
                Return Me.durationField
            End Get
            Set
                Me.durationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property [End]() As String
            Get
                Return Me.endField
            End Get
            Set
                Me.endField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OdometerUnitOfMeasure() As DistanceUnitNameType
            Get
                Return Me.odometerUnitOfMeasureField
            End Get
            Set
                Me.odometerUnitOfMeasureField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OdometerUnitOfMeasureSpecified() As Boolean
            Get
                Return Me.odometerUnitOfMeasureFieldSpecified
            End Get
            Set
                Me.odometerUnitOfMeasureFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleTypeVehMakeModel

        Private nameField As String

        Private codeField As String

        Private modelYearField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Name() As String
            Get
                Return Me.nameField
            End Get
            Set
                Me.nameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="gYear")>
        Public Property ModelYear() As String
            Get
                Return Me.modelYearField
            End Get
            Set
                Me.modelYearField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleTypeVehIdentity

        Private vehicleAssetNumberField As String

        Private licensePlateNumberField As String

        Private stateProvCodeField As String

        Private countryCodeField As String

        Private vehicleID_NumberField As String

        Private vehicleColorField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property VehicleAssetNumber() As String
            Get
                Return Me.vehicleAssetNumberField
            End Get
            Set
                Me.vehicleAssetNumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property LicensePlateNumber() As String
            Get
                Return Me.licensePlateNumberField
            End Get
            Set
                Me.licensePlateNumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property StateProvCode() As String
            Get
                Return Me.stateProvCodeField
            End Get
            Set
                Me.stateProvCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CountryCode() As String
            Get
                Return Me.countryCodeField
            End Get
            Set
                Me.countryCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property VehicleID_Number() As String
            Get
                Return Me.vehicleID_NumberField
            End Get
            Set
                Me.vehicleID_NumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property VehicleColor() As String
            Get
                Return Me.vehicleColorField
            End Get
            Set
                Me.vehicleColorField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(VehicleType)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(VehiclePrefType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleCoreType

        Private vehTypeField As VehicleCoreTypeVehType

        Private vehClassField As VehicleCoreTypeVehClass

        Private airConditionIndField As Boolean

        Private airConditionIndFieldSpecified As Boolean

        Private transmissionTypeField As VehicleTransmissionType

        Private transmissionTypeFieldSpecified As Boolean

        Private fuelTypeField As VehicleCoreTypeFuelType

        Private fuelTypeFieldSpecified As Boolean

        Private driveTypeField As VehicleCoreTypeDriveType

        Private driveTypeFieldSpecified As Boolean

        '''<remarks/>
        Public Property VehType() As VehicleCoreTypeVehType
            Get
                Return Me.vehTypeField
            End Get
            Set
                Me.vehTypeField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property VehClass() As VehicleCoreTypeVehClass
            Get
                Return Me.vehClassField
            End Get
            Set
                Me.vehClassField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AirConditionInd() As Boolean
            Get
                Return Me.airConditionIndField
            End Get
            Set
                Me.airConditionIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AirConditionIndSpecified() As Boolean
            Get
                Return Me.airConditionIndFieldSpecified
            End Get
            Set
                Me.airConditionIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TransmissionType() As VehicleTransmissionType
            Get
                Return Me.transmissionTypeField
            End Get
            Set
                Me.transmissionTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TransmissionTypeSpecified() As Boolean
            Get
                Return Me.transmissionTypeFieldSpecified
            End Get
            Set
                Me.transmissionTypeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property FuelType() As VehicleCoreTypeFuelType
            Get
                Return Me.fuelTypeField
            End Get
            Set
                Me.fuelTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property FuelTypeSpecified() As Boolean
            Get
                Return Me.fuelTypeFieldSpecified
            End Get
            Set
                Me.fuelTypeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DriveType() As VehicleCoreTypeDriveType
            Get
                Return Me.driveTypeField
            End Get
            Set
                Me.driveTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DriveTypeSpecified() As Boolean
            Get
                Return Me.driveTypeFieldSpecified
            End Get
            Set
                Me.driveTypeFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleCoreTypeVehType

        Private vehicleCategoryField As String

        Private doorCountField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property VehicleCategory() As String
            Get
                Return Me.vehicleCategoryField
            End Get
            Set
                Me.vehicleCategoryField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DoorCount() As String
            Get
                Return Me.doorCountField
            End Get
            Set
                Me.doorCountField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleCoreTypeVehClass

        Private sizeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Size() As String
            Get
                Return Me.sizeField
            End Get
            Set
                Me.sizeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum VehicleTransmissionType

        '''<remarks/>
        Automatic

        '''<remarks/>
        Manual
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum VehicleCoreTypeFuelType

        '''<remarks/>
        Unspecified

        '''<remarks/>
        Diesel

        '''<remarks/>
        Hybrid

        '''<remarks/>
        Electric

        '''<remarks/>
        LPG_CompressedGas

        '''<remarks/>
        Hydrogen

        '''<remarks/>
        MultiFuel

        '''<remarks/>
        Petrol

        '''<remarks/>
        Ethanol
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum VehicleCoreTypeDriveType

        '''<remarks/>
        AWD

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("4WD")>
        Item4WD

        '''<remarks/>
        Unspecified
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehiclePrefType
        Inherits VehicleCoreType

        Private vehMakeModelField As VehiclePrefTypeVehMakeModel

        Private typePrefField As PreferLevelType

        Private typePrefFieldSpecified As Boolean

        Private classPrefField As PreferLevelType

        Private classPrefFieldSpecified As Boolean

        Private airConditionPrefField As PreferLevelType

        Private airConditionPrefFieldSpecified As Boolean

        Private transmissionPrefField As PreferLevelType

        Private transmissionPrefFieldSpecified As Boolean

        Private vendorCarTypeField As String

        Private vehicleQtyField As Long

        Private vehicleQtyFieldSpecified As Boolean

        Private codeField As String

        Private codeContextField As String

        '''<remarks/>
        Public Property VehMakeModel() As VehiclePrefTypeVehMakeModel
            Get
                Return Me.vehMakeModelField
            End Get
            Set
                Me.vehMakeModelField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TypePref() As PreferLevelType
            Get
                Return Me.typePrefField
            End Get
            Set
                Me.typePrefField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TypePrefSpecified() As Boolean
            Get
                Return Me.typePrefFieldSpecified
            End Get
            Set
                Me.typePrefFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ClassPref() As PreferLevelType
            Get
                Return Me.classPrefField
            End Get
            Set
                Me.classPrefField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ClassPrefSpecified() As Boolean
            Get
                Return Me.classPrefFieldSpecified
            End Get
            Set
                Me.classPrefFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AirConditionPref() As PreferLevelType
            Get
                Return Me.airConditionPrefField
            End Get
            Set
                Me.airConditionPrefField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AirConditionPrefSpecified() As Boolean
            Get
                Return Me.airConditionPrefFieldSpecified
            End Get
            Set
                Me.airConditionPrefFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TransmissionPref() As PreferLevelType
            Get
                Return Me.transmissionPrefField
            End Get
            Set
                Me.transmissionPrefField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TransmissionPrefSpecified() As Boolean
            Get
                Return Me.transmissionPrefFieldSpecified
            End Get
            Set
                Me.transmissionPrefFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property VendorCarType() As String
            Get
                Return Me.vendorCarTypeField
            End Get
            Set
                Me.vendorCarTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property VehicleQty() As Long
            Get
                Return Me.vehicleQtyField
            End Get
            Set
                Me.vehicleQtyField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property VehicleQtySpecified() As Boolean
            Get
                Return Me.vehicleQtyFieldSpecified
            End Get
            Set
                Me.vehicleQtyFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CodeContext() As String
            Get
                Return Me.codeContextField
            End Get
            Set
                Me.codeContextField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehiclePrefTypeVehMakeModel

        Private nameField As String

        Private codeField As String

        Private modelYearField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Name() As String
            Get
                Return Me.nameField
            End Get
            Set
                Me.nameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="gYear")>
        Public Property ModelYear() As String
            Get
                Return Me.modelYearField
            End Get
            Set
                Me.modelYearField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleRentalRateType

        Private rateDistanceField() As VehicleRentalRateTypeRateDistance

        Private vehicleChargesField() As VehicleChargePurposeType

        Private rateQualifierField As VehicleRentalRateTypeRateQualifier

        Private rateRestrictionsField As VehicleRentalRateTypeRateRestrictions

        Private rateGuaranteeField As VehicleRentalRateTypeRateGuarantee

        Private pickupReturnRuleField() As VehicleRentalRateTypePickupReturnRule

        Private noShowFeeInfoField As NoShowFeeType

        Private quoteIDField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RateDistance")>
        Public Property RateDistance() As VehicleRentalRateTypeRateDistance()
            Get
                Return Me.rateDistanceField
            End Get
            Set
                Me.rateDistanceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("VehicleCharge", IsNullable:=False)>
        Public Property VehicleCharges() As VehicleChargePurposeType()
            Get
                Return Me.vehicleChargesField
            End Get
            Set
                Me.vehicleChargesField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property RateQualifier() As VehicleRentalRateTypeRateQualifier
            Get
                Return Me.rateQualifierField
            End Get
            Set
                Me.rateQualifierField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property RateRestrictions() As VehicleRentalRateTypeRateRestrictions
            Get
                Return Me.rateRestrictionsField
            End Get
            Set
                Me.rateRestrictionsField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property RateGuarantee() As VehicleRentalRateTypeRateGuarantee
            Get
                Return Me.rateGuaranteeField
            End Get
            Set
                Me.rateGuaranteeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PickupReturnRule")>
        Public Property PickupReturnRule() As VehicleRentalRateTypePickupReturnRule()
            Get
                Return Me.pickupReturnRuleField
            End Get
            Set
                Me.pickupReturnRuleField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property NoShowFeeInfo() As NoShowFeeType
            Get
                Return Me.noShowFeeInfoField
            End Get
            Set
                Me.noShowFeeInfoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property QuoteID() As String
            Get
                Return Me.quoteIDField
            End Get
            Set
                Me.quoteIDField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleRentalRateTypeRateDistance

        Private unlimitedField As Boolean

        Private quantityField As Long

        Private quantityFieldSpecified As Boolean

        Private distUnitNameField As DistanceUnitNameType

        Private distUnitNameFieldSpecified As Boolean

        Private vehiclePeriodUnitNameField As VehiclePeriodUnitNameType

        Private vehiclePeriodUnitNameFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Unlimited() As Boolean
            Get
                Return Me.unlimitedField
            End Get
            Set
                Me.unlimitedField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Quantity() As Long
            Get
                Return Me.quantityField
            End Get
            Set
                Me.quantityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property QuantitySpecified() As Boolean
            Get
                Return Me.quantityFieldSpecified
            End Get
            Set
                Me.quantityFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DistUnitName() As DistanceUnitNameType
            Get
                Return Me.distUnitNameField
            End Get
            Set
                Me.distUnitNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DistUnitNameSpecified() As Boolean
            Get
                Return Me.distUnitNameFieldSpecified
            End Get
            Set
                Me.distUnitNameFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property VehiclePeriodUnitName() As VehiclePeriodUnitNameType
            Get
                Return Me.vehiclePeriodUnitNameField
            End Get
            Set
                Me.vehiclePeriodUnitNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property VehiclePeriodUnitNameSpecified() As Boolean
            Get
                Return Me.vehiclePeriodUnitNameFieldSpecified
            End Get
            Set
                Me.vehiclePeriodUnitNameFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum VehiclePeriodUnitNameType

        '''<remarks/>
        RentalPeriod

        '''<remarks/>
        Year

        '''<remarks/>
        Month

        '''<remarks/>
        Week

        '''<remarks/>
        Day

        '''<remarks/>
        Hour

        '''<remarks/>
        Weekend

        '''<remarks/>
        ExtraMonth

        '''<remarks/>
        Bundle

        '''<remarks/>
        Package

        '''<remarks/>
        ExtraDay

        '''<remarks/>
        ExtraHour

        '''<remarks/>
        ExtraWeek
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleChargePurposeType
        Inherits VehicleChargeType

        Private purposeField As String

        Private requiredIndField As Boolean

        Private requiredIndFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Purpose() As String
            Get
                Return Me.purposeField
            End Get
            Set
                Me.purposeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RequiredInd() As Boolean
            Get
                Return Me.requiredIndField
            End Get
            Set
                Me.requiredIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RequiredIndSpecified() As Boolean
            Get
                Return Me.requiredIndFieldSpecified
            End Get
            Set
                Me.requiredIndFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(VehicleChargePurposeType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleChargeType

        Private taxAmountsField() As VehicleChargeTypeTaxAmount

        Private minMaxField As VehicleChargeTypeMinMax

        Private calculationField() As VehicleChargeTypeCalculation

        Private amountField As Decimal

        Private amountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        Private taxInclusiveField As Boolean

        Private taxInclusiveFieldSpecified As Boolean

        Private descriptionField As String

        Private guaranteedIndField As Boolean

        Private guaranteedIndFieldSpecified As Boolean

        Private includedInRateField As Boolean

        Private includedInRateFieldSpecified As Boolean

        Private includedInEstTotalIndField As Boolean

        Private includedInEstTotalIndFieldSpecified As Boolean

        Private rateConvertIndField As Boolean

        Private rateConvertIndFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("TaxAmount", IsNullable:=False)>
        Public Property TaxAmounts() As VehicleChargeTypeTaxAmount()
            Get
                Return Me.taxAmountsField
            End Get
            Set
                Me.taxAmountsField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property MinMax() As VehicleChargeTypeMinMax
            Get
                Return Me.minMaxField
            End Get
            Set
                Me.minMaxField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Calculation")>
        Public Property Calculation() As VehicleChargeTypeCalculation()
            Get
                Return Me.calculationField
            End Get
            Set
                Me.calculationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TaxInclusive() As Boolean
            Get
                Return Me.taxInclusiveField
            End Get
            Set
                Me.taxInclusiveField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TaxInclusiveSpecified() As Boolean
            Get
                Return Me.taxInclusiveFieldSpecified
            End Get
            Set
                Me.taxInclusiveFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property GuaranteedInd() As Boolean
            Get
                Return Me.guaranteedIndField
            End Get
            Set
                Me.guaranteedIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property GuaranteedIndSpecified() As Boolean
            Get
                Return Me.guaranteedIndFieldSpecified
            End Get
            Set
                Me.guaranteedIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property IncludedInRate() As Boolean
            Get
                Return Me.includedInRateField
            End Get
            Set
                Me.includedInRateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property IncludedInRateSpecified() As Boolean
            Get
                Return Me.includedInRateFieldSpecified
            End Get
            Set
                Me.includedInRateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property IncludedInEstTotalInd() As Boolean
            Get
                Return Me.includedInEstTotalIndField
            End Get
            Set
                Me.includedInEstTotalIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property IncludedInEstTotalIndSpecified() As Boolean
            Get
                Return Me.includedInEstTotalIndFieldSpecified
            End Get
            Set
                Me.includedInEstTotalIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RateConvertInd() As Boolean
            Get
                Return Me.rateConvertIndField
            End Get
            Set
                Me.rateConvertIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RateConvertIndSpecified() As Boolean
            Get
                Return Me.rateConvertIndFieldSpecified
            End Get
            Set
                Me.rateConvertIndFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleChargeTypeTaxAmount

        Private totalField As Decimal

        Private currencyCodeField As String

        Private taxCodeField As String

        Private percentageField As Decimal

        Private percentageFieldSpecified As Boolean

        Private descriptionField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Total() As Decimal
            Get
                Return Me.totalField
            End Get
            Set
                Me.totalField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TaxCode() As String
            Get
                Return Me.taxCodeField
            End Get
            Set
                Me.taxCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Percentage() As Decimal
            Get
                Return Me.percentageField
            End Get
            Set
                Me.percentageField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PercentageSpecified() As Boolean
            Get
                Return Me.percentageFieldSpecified
            End Get
            Set
                Me.percentageFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleChargeTypeMinMax

        Private maxChargeField As Decimal

        Private maxChargeFieldSpecified As Boolean

        Private minChargeField As Decimal

        Private minChargeFieldSpecified As Boolean

        Private maxChargeDaysField As Long

        Private maxChargeDaysFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MaxCharge() As Decimal
            Get
                Return Me.maxChargeField
            End Get
            Set
                Me.maxChargeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MaxChargeSpecified() As Boolean
            Get
                Return Me.maxChargeFieldSpecified
            End Get
            Set
                Me.maxChargeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MinCharge() As Decimal
            Get
                Return Me.minChargeField
            End Get
            Set
                Me.minChargeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MinChargeSpecified() As Boolean
            Get
                Return Me.minChargeFieldSpecified
            End Get
            Set
                Me.minChargeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MaxChargeDays() As Long
            Get
                Return Me.maxChargeDaysField
            End Get
            Set
                Me.maxChargeDaysField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MaxChargeDaysSpecified() As Boolean
            Get
                Return Me.maxChargeDaysFieldSpecified
            End Get
            Set
                Me.maxChargeDaysFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleChargeTypeCalculation

        Private unitChargeField As Decimal

        Private unitChargeFieldSpecified As Boolean

        Private unitNameField As String

        Private quantityField As String

        Private percentageField As Decimal

        Private percentageFieldSpecified As Boolean

        Private applicabilityField As VehicleChargeTypeCalculationApplicability

        Private applicabilityFieldSpecified As Boolean

        Private maxQuantityField As String

        Private totalField As Decimal

        Private totalFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property UnitCharge() As Decimal
            Get
                Return Me.unitChargeField
            End Get
            Set
                Me.unitChargeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property UnitChargeSpecified() As Boolean
            Get
                Return Me.unitChargeFieldSpecified
            End Get
            Set
                Me.unitChargeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property UnitName() As String
            Get
                Return Me.unitNameField
            End Get
            Set
                Me.unitNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property Quantity() As String
            Get
                Return Me.quantityField
            End Get
            Set
                Me.quantityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Percentage() As Decimal
            Get
                Return Me.percentageField
            End Get
            Set
                Me.percentageField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PercentageSpecified() As Boolean
            Get
                Return Me.percentageFieldSpecified
            End Get
            Set
                Me.percentageFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Applicability() As VehicleChargeTypeCalculationApplicability
            Get
                Return Me.applicabilityField
            End Get
            Set
                Me.applicabilityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ApplicabilitySpecified() As Boolean
            Get
                Return Me.applicabilityFieldSpecified
            End Get
            Set
                Me.applicabilityFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property MaxQuantity() As String
            Get
                Return Me.maxQuantityField
            End Get
            Set
                Me.maxQuantityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Total() As Decimal
            Get
                Return Me.totalField
            End Get
            Set
                Me.totalField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TotalSpecified() As Boolean
            Get
                Return Me.totalFieldSpecified
            End Get
            Set
                Me.totalFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum VehicleChargeTypeCalculationApplicability

        '''<remarks/>
        FromPickupLocation

        '''<remarks/>
        FromDropoffLocation

        '''<remarks/>
        BeforePickup

        '''<remarks/>
        AfterDropoff
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleRentalRateTypeRateQualifier
        Inherits RateQualifierType

        Private tourInfoRPHField As String

        Private custLoyaltyRPHField() As String

        Private quoteIDField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TourInfoRPH() As String
            Get
                Return Me.tourInfoRPHField
            End Get
            Set
                Me.tourInfoRPHField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CustLoyaltyRPH() As String()
            Get
                Return Me.custLoyaltyRPHField
            End Get
            Set
                Me.custLoyaltyRPHField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property QuoteID() As String
            Get
                Return Me.quoteIDField
            End Get
            Set
                Me.quoteIDField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class RateQualifierType

        Private promoDescField As String

        Private rateCommentsField() As RateQualifierTypeRateComment

        Private travelPurposeField As String

        Private rateCategoryField As String

        Private corpDiscountNmbrField As String

        Private promotionCodeField As String

        Private promotionVendorCodeField() As String

        Private rateQualifierField As String

        Private ratePeriodField As RateQualifierTypeRatePeriod

        Private ratePeriodFieldSpecified As Boolean

        Private guaranteedIndField As Boolean

        Private guaranteedIndFieldSpecified As Boolean

        Private arriveByFlightField As Boolean

        Private arriveByFlightFieldSpecified As Boolean

        Private rateAuthorizationCodeField As String

        Private vendorRateIDField As String

        '''<remarks/>
        Public Property PromoDesc() As String
            Get
                Return Me.promoDescField
            End Get
            Set
                Me.promoDescField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("RateComment", IsNullable:=False)>
        Public Property RateComments() As RateQualifierTypeRateComment()
            Get
                Return Me.rateCommentsField
            End Get
            Set
                Me.rateCommentsField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TravelPurpose() As String
            Get
                Return Me.travelPurposeField
            End Get
            Set
                Me.travelPurposeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RateCategory() As String
            Get
                Return Me.rateCategoryField
            End Get
            Set
                Me.rateCategoryField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CorpDiscountNmbr() As String
            Get
                Return Me.corpDiscountNmbrField
            End Get
            Set
                Me.corpDiscountNmbrField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PromotionCode() As String
            Get
                Return Me.promotionCodeField
            End Get
            Set
                Me.promotionCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PromotionVendorCode() As String()
            Get
                Return Me.promotionVendorCodeField
            End Get
            Set
                Me.promotionVendorCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RateQualifier() As String
            Get
                Return Me.rateQualifierField
            End Get
            Set
                Me.rateQualifierField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RatePeriod() As RateQualifierTypeRatePeriod
            Get
                Return Me.ratePeriodField
            End Get
            Set
                Me.ratePeriodField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RatePeriodSpecified() As Boolean
            Get
                Return Me.ratePeriodFieldSpecified
            End Get
            Set
                Me.ratePeriodFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property GuaranteedInd() As Boolean
            Get
                Return Me.guaranteedIndField
            End Get
            Set
                Me.guaranteedIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property GuaranteedIndSpecified() As Boolean
            Get
                Return Me.guaranteedIndFieldSpecified
            End Get
            Set
                Me.guaranteedIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ArriveByFlight() As Boolean
            Get
                Return Me.arriveByFlightField
            End Get
            Set
                Me.arriveByFlightField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ArriveByFlightSpecified() As Boolean
            Get
                Return Me.arriveByFlightFieldSpecified
            End Get
            Set
                Me.arriveByFlightFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RateAuthorizationCode() As String
            Get
                Return Me.rateAuthorizationCodeField
            End Get
            Set
                Me.rateAuthorizationCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property VendorRateID() As String
            Get
                Return Me.vendorRateIDField
            End Get
            Set
                Me.vendorRateIDField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class RateQualifierTypeRateComment
        Inherits FormattedTextTextType

        Private nameField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Name() As String
            Get
                Return Me.nameField
            End Get
            Set
                Me.nameField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum RateQualifierTypeRatePeriod

        '''<remarks/>
        Hourly

        '''<remarks/>
        Daily

        '''<remarks/>
        Weekly

        '''<remarks/>
        Monthly

        '''<remarks/>
        WeekendDay

        '''<remarks/>
        Other

        '''<remarks/>
        Package

        '''<remarks/>
        Bundle

        '''<remarks/>
        Total
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleRentalRateTypeRateRestrictions

        Private arriveByFlightField As Boolean

        Private arriveByFlightFieldSpecified As Boolean

        Private minimumDayIndField As Boolean

        Private minimumDayIndFieldSpecified As Boolean

        Private maximumDayIndField As Boolean

        Private maximumDayIndFieldSpecified As Boolean

        Private advancedBookingIndField As Boolean

        Private advancedBookingIndFieldSpecified As Boolean

        Private restrictedMileageIndField As Boolean

        Private restrictedMileageIndFieldSpecified As Boolean

        Private corporateRateIndField As Boolean

        Private corporateRateIndFieldSpecified As Boolean

        Private guaranteeReqIndField As Boolean

        Private guaranteeReqIndFieldSpecified As Boolean

        Private maximumVehiclesAllowedField As String

        Private overnightIndField As Boolean

        Private overnightIndFieldSpecified As Boolean

        Private oneWayPolicyField As VehicleRentalRateTypeRateRestrictionsOneWayPolicy

        Private oneWayPolicyFieldSpecified As Boolean

        Private cancellationPenaltyIndField As Boolean

        Private cancellationPenaltyIndFieldSpecified As Boolean

        Private modificationPenaltyIndField As Boolean

        Private modificationPenaltyIndFieldSpecified As Boolean

        Private minimumAgeField As String

        Private maximumAgeField As String

        Private noShowFeeIndField As Boolean

        Private noShowFeeIndFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ArriveByFlight() As Boolean
            Get
                Return Me.arriveByFlightField
            End Get
            Set
                Me.arriveByFlightField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ArriveByFlightSpecified() As Boolean
            Get
                Return Me.arriveByFlightFieldSpecified
            End Get
            Set
                Me.arriveByFlightFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MinimumDayInd() As Boolean
            Get
                Return Me.minimumDayIndField
            End Get
            Set
                Me.minimumDayIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MinimumDayIndSpecified() As Boolean
            Get
                Return Me.minimumDayIndFieldSpecified
            End Get
            Set
                Me.minimumDayIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MaximumDayInd() As Boolean
            Get
                Return Me.maximumDayIndField
            End Get
            Set
                Me.maximumDayIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MaximumDayIndSpecified() As Boolean
            Get
                Return Me.maximumDayIndFieldSpecified
            End Get
            Set
                Me.maximumDayIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AdvancedBookingInd() As Boolean
            Get
                Return Me.advancedBookingIndField
            End Get
            Set
                Me.advancedBookingIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AdvancedBookingIndSpecified() As Boolean
            Get
                Return Me.advancedBookingIndFieldSpecified
            End Get
            Set
                Me.advancedBookingIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RestrictedMileageInd() As Boolean
            Get
                Return Me.restrictedMileageIndField
            End Get
            Set
                Me.restrictedMileageIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RestrictedMileageIndSpecified() As Boolean
            Get
                Return Me.restrictedMileageIndFieldSpecified
            End Get
            Set
                Me.restrictedMileageIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CorporateRateInd() As Boolean
            Get
                Return Me.corporateRateIndField
            End Get
            Set
                Me.corporateRateIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property CorporateRateIndSpecified() As Boolean
            Get
                Return Me.corporateRateIndFieldSpecified
            End Get
            Set
                Me.corporateRateIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property GuaranteeReqInd() As Boolean
            Get
                Return Me.guaranteeReqIndField
            End Get
            Set
                Me.guaranteeReqIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property GuaranteeReqIndSpecified() As Boolean
            Get
                Return Me.guaranteeReqIndFieldSpecified
            End Get
            Set
                Me.guaranteeReqIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property MaximumVehiclesAllowed() As String
            Get
                Return Me.maximumVehiclesAllowedField
            End Get
            Set
                Me.maximumVehiclesAllowedField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OvernightInd() As Boolean
            Get
                Return Me.overnightIndField
            End Get
            Set
                Me.overnightIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OvernightIndSpecified() As Boolean
            Get
                Return Me.overnightIndFieldSpecified
            End Get
            Set
                Me.overnightIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OneWayPolicy() As VehicleRentalRateTypeRateRestrictionsOneWayPolicy
            Get
                Return Me.oneWayPolicyField
            End Get
            Set
                Me.oneWayPolicyField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OneWayPolicySpecified() As Boolean
            Get
                Return Me.oneWayPolicyFieldSpecified
            End Get
            Set
                Me.oneWayPolicyFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CancellationPenaltyInd() As Boolean
            Get
                Return Me.cancellationPenaltyIndField
            End Get
            Set
                Me.cancellationPenaltyIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property CancellationPenaltyIndSpecified() As Boolean
            Get
                Return Me.cancellationPenaltyIndFieldSpecified
            End Get
            Set
                Me.cancellationPenaltyIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ModificationPenaltyInd() As Boolean
            Get
                Return Me.modificationPenaltyIndField
            End Get
            Set
                Me.modificationPenaltyIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ModificationPenaltyIndSpecified() As Boolean
            Get
                Return Me.modificationPenaltyIndFieldSpecified
            End Get
            Set
                Me.modificationPenaltyIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property MinimumAge() As String
            Get
                Return Me.minimumAgeField
            End Get
            Set
                Me.minimumAgeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property MaximumAge() As String
            Get
                Return Me.maximumAgeField
            End Get
            Set
                Me.maximumAgeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property NoShowFeeInd() As Boolean
            Get
                Return Me.noShowFeeIndField
            End Get
            Set
                Me.noShowFeeIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property NoShowFeeIndSpecified() As Boolean
            Get
                Return Me.noShowFeeIndFieldSpecified
            End Get
            Set
                Me.noShowFeeIndFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum VehicleRentalRateTypeRateRestrictionsOneWayPolicy

        '''<remarks/>
        OneWayAllowed

        '''<remarks/>
        OneWayNotAllowed

        '''<remarks/>
        RestrictedOneWay
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleRentalRateTypeRateGuarantee

        Private descriptionField As FormattedTextTextType

        Private absoluteDeadlineField As String

        Private offsetTimeUnitField As TimeUnitType

        Private offsetTimeUnitFieldSpecified As Boolean

        Private offsetUnitMultiplierField As String

        Private offsetDropTimeField As VehicleRentalRateTypeRateGuaranteeOffsetDropTime

        Private offsetDropTimeFieldSpecified As Boolean

        '''<remarks/>
        Public Property Description() As FormattedTextTextType
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AbsoluteDeadline() As String
            Get
                Return Me.absoluteDeadlineField
            End Get
            Set
                Me.absoluteDeadlineField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OffsetTimeUnit() As TimeUnitType
            Get
                Return Me.offsetTimeUnitField
            End Get
            Set
                Me.offsetTimeUnitField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OffsetTimeUnitSpecified() As Boolean
            Get
                Return Me.offsetTimeUnitFieldSpecified
            End Get
            Set
                Me.offsetTimeUnitFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property OffsetUnitMultiplier() As String
            Get
                Return Me.offsetUnitMultiplierField
            End Get
            Set
                Me.offsetUnitMultiplierField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OffsetDropTime() As VehicleRentalRateTypeRateGuaranteeOffsetDropTime
            Get
                Return Me.offsetDropTimeField
            End Get
            Set
                Me.offsetDropTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OffsetDropTimeSpecified() As Boolean
            Get
                Return Me.offsetDropTimeFieldSpecified
            End Get
            Set
                Me.offsetDropTimeFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum TimeUnitType

        '''<remarks/>
        Year

        '''<remarks/>
        Month

        '''<remarks/>
        Week

        '''<remarks/>
        Day

        '''<remarks/>
        Hour

        '''<remarks/>
        Second

        '''<remarks/>
        FullDuration

        '''<remarks/>
        Minute
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum VehicleRentalRateTypeRateGuaranteeOffsetDropTime

        '''<remarks/>
        BeforeArrival

        '''<remarks/>
        AfterBooking

        '''<remarks/>
        AfterConfirmation

        '''<remarks/>
        AfterArrival
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleRentalRateTypePickupReturnRule

        Private dayOfWeekField As DayOfWeekType

        Private dayOfWeekFieldSpecified As Boolean

        Private timeField As String

        Private ruleTypeField As VehicleRentalRateTypePickupReturnRuleRuleType

        Private ruleTypeFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DayOfWeek() As DayOfWeekType
            Get
                Return Me.dayOfWeekField
            End Get
            Set
                Me.dayOfWeekField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DayOfWeekSpecified() As Boolean
            Get
                Return Me.dayOfWeekFieldSpecified
            End Get
            Set
                Me.dayOfWeekFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Time() As String
            Get
                Return Me.timeField
            End Get
            Set
                Me.timeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RuleType() As VehicleRentalRateTypePickupReturnRuleRuleType
            Get
                Return Me.ruleTypeField
            End Get
            Set
                Me.ruleTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RuleTypeSpecified() As Boolean
            Get
                Return Me.ruleTypeFieldSpecified
            End Get
            Set
                Me.ruleTypeFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum DayOfWeekType

        '''<remarks/>
        Mon

        '''<remarks/>
        Tue

        '''<remarks/>
        Wed

        '''<remarks/>
        Thu

        '''<remarks/>
        Fri

        '''<remarks/>
        Sat

        '''<remarks/>
        Sun
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum VehicleRentalRateTypePickupReturnRuleRuleType

        '''<remarks/>
        EarliestPickup

        '''<remarks/>
        LatestPickup

        '''<remarks/>
        LatestReturn
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class NoShowFeeType

        Private deadlineField As NoShowFeeTypeDeadline

        Private gracePeriodField As NoShowFeeTypeGracePeriod

        Private feeAmountField As NoShowFeeTypeFeeAmount

        Private descriptionField As FormattedTextTextType

        '''<remarks/>
        Public Property Deadline() As NoShowFeeTypeDeadline
            Get
                Return Me.deadlineField
            End Get
            Set
                Me.deadlineField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property GracePeriod() As NoShowFeeTypeGracePeriod
            Get
                Return Me.gracePeriodField
            End Get
            Set
                Me.gracePeriodField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property FeeAmount() As NoShowFeeTypeFeeAmount
            Get
                Return Me.feeAmountField
            End Get
            Set
                Me.feeAmountField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As FormattedTextTextType
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class NoShowFeeTypeDeadline

        Private absoluteDeadlineField As String

        Private offsetTimeUnitField As TimeUnitType

        Private offsetTimeUnitFieldSpecified As Boolean

        Private offsetUnitMultiplierField As String

        Private offsetDropTimeField As VehicleRentalRateTypeRateGuaranteeOffsetDropTime

        Private offsetDropTimeFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AbsoluteDeadline() As String
            Get
                Return Me.absoluteDeadlineField
            End Get
            Set
                Me.absoluteDeadlineField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OffsetTimeUnit() As TimeUnitType
            Get
                Return Me.offsetTimeUnitField
            End Get
            Set
                Me.offsetTimeUnitField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OffsetTimeUnitSpecified() As Boolean
            Get
                Return Me.offsetTimeUnitFieldSpecified
            End Get
            Set
                Me.offsetTimeUnitFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property OffsetUnitMultiplier() As String
            Get
                Return Me.offsetUnitMultiplierField
            End Get
            Set
                Me.offsetUnitMultiplierField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OffsetDropTime() As VehicleRentalRateTypeRateGuaranteeOffsetDropTime
            Get
                Return Me.offsetDropTimeField
            End Get
            Set
                Me.offsetDropTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OffsetDropTimeSpecified() As Boolean
            Get
                Return Me.offsetDropTimeFieldSpecified
            End Get
            Set
                Me.offsetDropTimeFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class NoShowFeeTypeGracePeriod

        Private absoluteDeadlineField As String

        Private offsetTimeUnitField As TimeUnitType

        Private offsetTimeUnitFieldSpecified As Boolean

        Private offsetUnitMultiplierField As String

        Private offsetDropTimeField As VehicleRentalRateTypeRateGuaranteeOffsetDropTime

        Private offsetDropTimeFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AbsoluteDeadline() As String
            Get
                Return Me.absoluteDeadlineField
            End Get
            Set
                Me.absoluteDeadlineField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OffsetTimeUnit() As TimeUnitType
            Get
                Return Me.offsetTimeUnitField
            End Get
            Set
                Me.offsetTimeUnitField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OffsetTimeUnitSpecified() As Boolean
            Get
                Return Me.offsetTimeUnitFieldSpecified
            End Get
            Set
                Me.offsetTimeUnitFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property OffsetUnitMultiplier() As String
            Get
                Return Me.offsetUnitMultiplierField
            End Get
            Set
                Me.offsetUnitMultiplierField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OffsetDropTime() As VehicleRentalRateTypeRateGuaranteeOffsetDropTime
            Get
                Return Me.offsetDropTimeField
            End Get
            Set
                Me.offsetDropTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OffsetDropTimeSpecified() As Boolean
            Get
                Return Me.offsetDropTimeFieldSpecified
            End Get
            Set
                Me.offsetDropTimeFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class NoShowFeeTypeFeeAmount

        Private amountField As Decimal

        Private amountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        Private rateConvertedIndField As Boolean

        Private rateConvertedIndFieldSpecified As Boolean

        Private guaranteeReqIndField As Boolean

        Private guaranteeReqIndFieldSpecified As Boolean

        Private emailRequiredIndField As Boolean

        Private emailRequiredIndFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RateConvertedInd() As Boolean
            Get
                Return Me.rateConvertedIndField
            End Get
            Set
                Me.rateConvertedIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RateConvertedIndSpecified() As Boolean
            Get
                Return Me.rateConvertedIndFieldSpecified
            End Get
            Set
                Me.rateConvertedIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property GuaranteeReqInd() As Boolean
            Get
                Return Me.guaranteeReqIndField
            End Get
            Set
                Me.guaranteeReqIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property GuaranteeReqIndSpecified() As Boolean
            Get
                Return Me.guaranteeReqIndFieldSpecified
            End Get
            Set
                Me.guaranteeReqIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property EmailRequiredInd() As Boolean
            Get
                Return Me.emailRequiredIndField
            End Get
            Set
                Me.emailRequiredIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EmailRequiredIndSpecified() As Boolean
            Get
                Return Me.emailRequiredIndFieldSpecified
            End Get
            Set
                Me.emailRequiredIndFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleEquipmentPricedType

        Private equipmentField As VehicleEquipmentType

        Private chargeField As VehicleChargeType

        Private requiredField As Boolean

        Private requiredFieldSpecified As Boolean

        '''<remarks/>
        Public Property Equipment() As VehicleEquipmentType
            Get
                Return Me.equipmentField
            End Get
            Set
                Me.equipmentField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Charge() As VehicleChargeType
            Get
                Return Me.chargeField
            End Get
            Set
                Me.chargeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Required() As Boolean
            Get
                Return Me.requiredField
            End Get
            Set
                Me.requiredField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RequiredSpecified() As Boolean
            Get
                Return Me.requiredFieldSpecified
            End Get
            Set
                Me.requiredFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleEquipmentType

        Private descriptionField As String

        Private equipTypeField As String

        Private quantityField As Long

        Private quantityFieldSpecified As Boolean

        Private restrictionField As EquipmentRestrictionType

        Private restrictionFieldSpecified As Boolean

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property EquipType() As String
            Get
                Return Me.equipTypeField
            End Get
            Set
                Me.equipTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Quantity() As Long
            Get
                Return Me.quantityField
            End Get
            Set
                Me.quantityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property QuantitySpecified() As Boolean
            Get
                Return Me.quantityFieldSpecified
            End Get
            Set
                Me.quantityFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Restriction() As EquipmentRestrictionType
            Get
                Return Me.restrictionField
            End Get
            Set
                Me.restrictionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RestrictionSpecified() As Boolean
            Get
                Return Me.restrictionFieldSpecified
            End Get
            Set
                Me.restrictionFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum EquipmentRestrictionType

        '''<remarks/>
        OneWayOnly

        '''<remarks/>
        RoundTripOnly

        '''<remarks/>
        AnyReservation
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleSegmentCoreTypeTotalCharge

        Private rateTotalAmountField As Decimal

        Private rateTotalAmountFieldSpecified As Boolean

        Private estimatedTotalAmountField As Decimal

        Private estimatedTotalAmountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RateTotalAmount() As Decimal
            Get
                Return Me.rateTotalAmountField
            End Get
            Set
                Me.rateTotalAmountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RateTotalAmountSpecified() As Boolean
            Get
                Return Me.rateTotalAmountFieldSpecified
            End Get
            Set
                Me.rateTotalAmountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property EstimatedTotalAmount() As Decimal
            Get
                Return Me.estimatedTotalAmountField
            End Get
            Set
                Me.estimatedTotalAmountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EstimatedTotalAmountSpecified() As Boolean
            Get
                Return Me.estimatedTotalAmountFieldSpecified
            End Get
            Set
                Me.estimatedTotalAmountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleSegmentAdditionalInfoType

        Private paymentRulesField() As MonetaryRuleType

        Private rentalPaymentAmountField() As PaymentDetailType

        Private pricedCoveragesField() As CoveragePricedType

        Private pricedOffLocServiceField() As OffLocationServicePricedType

        Private vendorMessagesField() As FormattedTextType

        Private locationDetailsField() As VehicleLocationDetailsType

        Private tourInfoField As VehicleTourInfoType

        Private specialReqPrefField() As VehicleSpecialReqPrefType

        Private arrivalDetailsField As VehicleArrivalDetailsType

        Private writtenConfInstField As WrittenConfInstType

        Private remarkField() As ParagraphType

        Private tPA_ExtensionsField As TPA_ExtensionsType

        Private writtenConfIndField As Boolean

        Private writtenConfIndFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("PaymentRule", IsNullable:=False)>
        Public Property PaymentRules() As MonetaryRuleType()
            Get
                Return Me.paymentRulesField
            End Get
            Set
                Me.paymentRulesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RentalPaymentAmount")>
        Public Property RentalPaymentAmount() As PaymentDetailType()
            Get
                Return Me.rentalPaymentAmountField
            End Get
            Set
                Me.rentalPaymentAmountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("PricedCoverage", IsNullable:=False)>
        Public Property PricedCoverages() As CoveragePricedType()
            Get
                Return Me.pricedCoveragesField
            End Get
            Set
                Me.pricedCoveragesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PricedOffLocService")>
        Public Property PricedOffLocService() As OffLocationServicePricedType()
            Get
                Return Me.pricedOffLocServiceField
            End Get
            Set
                Me.pricedOffLocServiceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("VendorMessage", IsNullable:=False)>
        Public Property VendorMessages() As FormattedTextType()
            Get
                Return Me.vendorMessagesField
            End Get
            Set
                Me.vendorMessagesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LocationDetails")>
        Public Property LocationDetails() As VehicleLocationDetailsType()
            Get
                Return Me.locationDetailsField
            End Get
            Set
                Me.locationDetailsField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property TourInfo() As VehicleTourInfoType
            Get
                Return Me.tourInfoField
            End Get
            Set
                Me.tourInfoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SpecialReqPref")>
        Public Property SpecialReqPref() As VehicleSpecialReqPrefType()
            Get
                Return Me.specialReqPrefField
            End Get
            Set
                Me.specialReqPrefField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property ArrivalDetails() As VehicleArrivalDetailsType
            Get
                Return Me.arrivalDetailsField
            End Get
            Set
                Me.arrivalDetailsField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property WrittenConfInst() As WrittenConfInstType
            Get
                Return Me.writtenConfInstField
            End Get
            Set
                Me.writtenConfInstField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Remark")>
        Public Property Remark() As ParagraphType()
            Get
                Return Me.remarkField
            End Get
            Set
                Me.remarkField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property TPA_Extensions() As TPA_ExtensionsType
            Get
                Return Me.tPA_ExtensionsField
            End Get
            Set
                Me.tPA_ExtensionsField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property WrittenConfInd() As Boolean
            Get
                Return Me.writtenConfIndField
            End Get
            Set
                Me.writtenConfIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property WrittenConfIndSpecified() As Boolean
            Get
                Return Me.writtenConfIndFieldSpecified
            End Get
            Set
                Me.writtenConfIndFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class MonetaryRuleType

        Private amountField As Decimal

        Private amountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        Private ruleTypeField As String

        Private percentField As Decimal

        Private percentFieldSpecified As Boolean

        Private dateTimeField As Date

        Private dateTimeFieldSpecified As Boolean

        Private paymentTypeField As String

        Private rateConvertedIndField As Boolean

        Private rateConvertedIndFieldSpecified As Boolean

        Private absoluteDeadlineField As String

        Private offsetTimeUnitField As TimeUnitType

        Private offsetTimeUnitFieldSpecified As Boolean

        Private offsetUnitMultiplierField As String

        Private offsetDropTimeField As VehicleRentalRateTypeRateGuaranteeOffsetDropTime

        Private offsetDropTimeFieldSpecified As Boolean

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RuleType() As String
            Get
                Return Me.ruleTypeField
            End Get
            Set
                Me.ruleTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Percent() As Decimal
            Get
                Return Me.percentField
            End Get
            Set
                Me.percentField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PercentSpecified() As Boolean
            Get
                Return Me.percentFieldSpecified
            End Get
            Set
                Me.percentFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DateTime() As Date
            Get
                Return Me.dateTimeField
            End Get
            Set
                Me.dateTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DateTimeSpecified() As Boolean
            Get
                Return Me.dateTimeFieldSpecified
            End Get
            Set
                Me.dateTimeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PaymentType() As String
            Get
                Return Me.paymentTypeField
            End Get
            Set
                Me.paymentTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RateConvertedInd() As Boolean
            Get
                Return Me.rateConvertedIndField
            End Get
            Set
                Me.rateConvertedIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RateConvertedIndSpecified() As Boolean
            Get
                Return Me.rateConvertedIndFieldSpecified
            End Get
            Set
                Me.rateConvertedIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AbsoluteDeadline() As String
            Get
                Return Me.absoluteDeadlineField
            End Get
            Set
                Me.absoluteDeadlineField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OffsetTimeUnit() As TimeUnitType
            Get
                Return Me.offsetTimeUnitField
            End Get
            Set
                Me.offsetTimeUnitField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OffsetTimeUnitSpecified() As Boolean
            Get
                Return Me.offsetTimeUnitFieldSpecified
            End Get
            Set
                Me.offsetTimeUnitFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property OffsetUnitMultiplier() As String
            Get
                Return Me.offsetUnitMultiplierField
            End Get
            Set
                Me.offsetUnitMultiplierField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property OffsetDropTime() As VehicleRentalRateTypeRateGuaranteeOffsetDropTime
            Get
                Return Me.offsetDropTimeField
            End Get
            Set
                Me.offsetDropTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property OffsetDropTimeSpecified() As Boolean
            Get
                Return Me.offsetDropTimeFieldSpecified
            End Get
            Set
                Me.offsetDropTimeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CoveragePricedType

        Private coverageField As CoverageType

        Private chargeField As VehicleChargeType

        Private deductibleField As DeductibleType

        Private requiredField As Boolean

        Private requiredFieldSpecified As Boolean

        '''<remarks/>
        Public Property Coverage() As CoverageType
            Get
                Return Me.coverageField
            End Get
            Set
                Me.coverageField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Charge() As VehicleChargeType
            Get
                Return Me.chargeField
            End Get
            Set
                Me.chargeField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Deductible() As DeductibleType
            Get
                Return Me.deductibleField
            End Get
            Set
                Me.deductibleField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Required() As Boolean
            Get
                Return Me.requiredField
            End Get
            Set
                Me.requiredField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RequiredSpecified() As Boolean
            Get
                Return Me.requiredFieldSpecified
            End Get
            Set
                Me.requiredFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CoverageType

        Private detailsField() As CoverageDetailsType

        Private coverageType1Field As String

        Private codeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Details")>
        Public Property Details() As CoverageDetailsType()
            Get
                Return Me.detailsField
            End Get
            Set
                Me.detailsField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute("CoverageType")>
        Public Property CoverageType1() As String
            Get
                Return Me.coverageType1Field
            End Get
            Set
                Me.coverageType1Field = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class DeductibleType

        Private amountField As Decimal

        Private amountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        Private liabilityAmountField As Decimal

        Private liabilityAmountFieldSpecified As Boolean

        Private excessAmountField As Decimal

        Private excessAmountFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property LiabilityAmount() As Decimal
            Get
                Return Me.liabilityAmountField
            End Get
            Set
                Me.liabilityAmountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property LiabilityAmountSpecified() As Boolean
            Get
                Return Me.liabilityAmountFieldSpecified
            End Get
            Set
                Me.liabilityAmountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExcessAmount() As Decimal
            Get
                Return Me.excessAmountField
            End Get
            Set
                Me.excessAmountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExcessAmountSpecified() As Boolean
            Get
                Return Me.excessAmountFieldSpecified
            End Get
            Set
                Me.excessAmountFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class OffLocationServicePricedType

        Private offLocServiceField As OffLocationServiceType

        Private chargeField As VehicleChargeType

        '''<remarks/>
        Public Property OffLocService() As OffLocationServiceType
            Get
                Return Me.offLocServiceField
            End Get
            Set
                Me.offLocServiceField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Charge() As VehicleChargeType
            Get
                Return Me.chargeField
            End Get
            Set
                Me.chargeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class OffLocationServiceType
        Inherits OffLocationServiceCoreType

        Private personNameField As PersonNameType

        Private telephoneField As OffLocationServiceTypeTelephone

        Private trackingIDField As UniqueID_Type

        Private specInstructionsField As String

        '''<remarks/>
        Public Property PersonName() As PersonNameType
            Get
                Return Me.personNameField
            End Get
            Set
                Me.personNameField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Telephone() As OffLocationServiceTypeTelephone
            Get
                Return Me.telephoneField
            End Get
            Set
                Me.telephoneField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property TrackingID() As UniqueID_Type
            Get
                Return Me.trackingIDField
            End Get
            Set
                Me.trackingIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SpecInstructions() As String
            Get
                Return Me.specInstructionsField
            End Get
            Set
                Me.specInstructionsField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class OffLocationServiceTypeTelephone

        Private shareSynchIndField As OffLocationServiceTypeTelephoneShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As OffLocationServiceTypeTelephoneShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private phoneLocationTypeField As String

        Private phoneTechTypeField As String

        Private phoneUseTypeField As String

        Private countryAccessCodeField As String

        Private areaCityCodeField As String

        Private phoneNumberField As String

        Private extensionField As String

        Private pINField As String

        Private remarkField As String

        Private formattedIndField As Boolean

        Private formattedIndFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As OffLocationServiceTypeTelephoneShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As OffLocationServiceTypeTelephoneShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneLocationType() As String
            Get
                Return Me.phoneLocationTypeField
            End Get
            Set
                Me.phoneLocationTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneTechType() As String
            Get
                Return Me.phoneTechTypeField
            End Get
            Set
                Me.phoneTechTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneUseType() As String
            Get
                Return Me.phoneUseTypeField
            End Get
            Set
                Me.phoneUseTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CountryAccessCode() As String
            Get
                Return Me.countryAccessCodeField
            End Get
            Set
                Me.countryAccessCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AreaCityCode() As String
            Get
                Return Me.areaCityCodeField
            End Get
            Set
                Me.areaCityCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneNumber() As String
            Get
                Return Me.phoneNumberField
            End Get
            Set
                Me.phoneNumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Extension() As String
            Get
                Return Me.extensionField
            End Get
            Set
                Me.extensionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PIN() As String
            Get
                Return Me.pINField
            End Get
            Set
                Me.pINField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Remark() As String
            Get
                Return Me.remarkField
            End Get
            Set
                Me.remarkField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property FormattedInd() As Boolean
            Get
                Return Me.formattedIndField
            End Get
            Set
                Me.formattedIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property FormattedIndSpecified() As Boolean
            Get
                Return Me.formattedIndFieldSpecified
            End Get
            Set
                Me.formattedIndFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum OffLocationServiceTypeTelephoneShareSynchInd

        '''<remarks/>
        Yes

        '''<remarks/>
        No

        '''<remarks/>
        Inherit
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum OffLocationServiceTypeTelephoneShareMarketInd

        '''<remarks/>
        Yes

        '''<remarks/>
        No

        '''<remarks/>
        Inherit
    End Enum

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(OffLocationServiceType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class OffLocationServiceCoreType

        Private addressField As OffLocationServiceCoreTypeAddress

        Private typeField As OffLocationServiceID_Type

        '''<remarks/>
        Public Property Address() As OffLocationServiceCoreTypeAddress
            Get
                Return Me.addressField
            End Get
            Set
                Me.addressField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As OffLocationServiceID_Type
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class OffLocationServiceCoreTypeAddress
        Inherits AddressType

        Private siteIDField As String

        Private siteNameField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SiteID() As String
            Get
                Return Me.siteIDField
            End Get
            Set
                Me.siteIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SiteName() As String
            Get
                Return Me.siteNameField
            End Get
            Set
                Me.siteNameField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum OffLocationServiceID_Type

        '''<remarks/>
        CustPickUp

        '''<remarks/>
        VehDelivery

        '''<remarks/>
        CustDropOff

        '''<remarks/>
        VehCollection

        '''<remarks/>
        Exchange

        '''<remarks/>
        RepairLocation
    End Enum

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(VehicleLocationInformationType)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(VendorMessageType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class FormattedTextType

        Private subSectionField() As FormattedTextSubSectionType

        Private dummyElementField As Object

        Private titleField As String

        Private languageField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SubSection")>
        Public Property SubSection() As FormattedTextSubSectionType()
            Get
                Return Me.subSectionField
            End Get
            Set
                Me.subSectionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property DummyElement() As Object
            Get
                Return Me.dummyElementField
            End Get
            Set
                Me.dummyElementField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Title() As String
            Get
                Return Me.titleField
            End Get
            Set
                Me.titleField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")>
        Public Property Language() As String
            Get
                Return Me.languageField
            End Get
            Set
                Me.languageField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class FormattedTextSubSectionType

        Private paragraphField() As ParagraphType

        Private dummyElementField As Object

        Private subTitleField As String

        Private subCodeField As String

        Private subSectionNumberField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Paragraph")>
        Public Property Paragraph() As ParagraphType()
            Get
                Return Me.paragraphField
            End Get
            Set
                Me.paragraphField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property DummyElement() As Object
            Get
                Return Me.dummyElementField
            End Get
            Set
                Me.dummyElementField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SubTitle() As String
            Get
                Return Me.subTitleField
            End Get
            Set
                Me.subTitleField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SubCode() As String
            Get
                Return Me.subCodeField
            End Get
            Set
                Me.subCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property SubSectionNumber() As String
            Get
                Return Me.subSectionNumberField
            End Get
            Set
                Me.subSectionNumberField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleLocationInformationType
        Inherits FormattedTextType

        Private typeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class VendorMessageType
        Inherits FormattedTextType
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property InfoType As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class VehicleLocationDetailsType

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Address")>
        Public Property Address As AddressInfoType()

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Telephone")>
        Public Property Telephone As VehicleLocationDetailsTypeTelephone()

        '''<remarks/>
        Public Property AdditionalInfo As VehicleLocationAdditionalDetailsType

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AtAirport As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AtAirportSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Name As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CodeContext As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExtendedLocationCode As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AssocAirportLocList As String()
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code")>
    Partial Public Class VehicleLocationDetailsTypeTelephone

        Private shareSynchIndField As CustomerTypeTelephoneShareSynchInd

        Private shareSynchIndFieldSpecified As Boolean

        Private shareMarketIndField As CustomerTypeTelephoneShareMarketInd

        Private shareMarketIndFieldSpecified As Boolean

        Private phoneLocationTypeField As String

        Private phoneTechTypeField As String

        Private phoneUseTypeField As String

        Private countryAccessCodeField As String

        Private areaCityCodeField As String

        Private phoneNumberField As String

        Private extensionField As String

        Private pINField As String

        Private remarkField As String

        Private formattedIndField As Boolean

        Private formattedIndFieldSpecified As Boolean

        Private defaultIndField As Boolean

        Private defaultIndFieldSpecified As Boolean

        Private rPHField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareSynchInd() As CustomerTypeTelephoneShareSynchInd
            Get
                Return Me.shareSynchIndField
            End Get
            Set
                Me.shareSynchIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareSynchIndSpecified() As Boolean
            Get
                Return Me.shareSynchIndFieldSpecified
            End Get
            Set
                Me.shareSynchIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShareMarketInd() As CustomerTypeTelephoneShareMarketInd
            Get
                Return Me.shareMarketIndField
            End Get
            Set
                Me.shareMarketIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ShareMarketIndSpecified() As Boolean
            Get
                Return Me.shareMarketIndFieldSpecified
            End Get
            Set
                Me.shareMarketIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneLocationType() As String
            Get
                Return Me.phoneLocationTypeField
            End Get
            Set
                Me.phoneLocationTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneTechType() As String
            Get
                Return Me.phoneTechTypeField
            End Get
            Set
                Me.phoneTechTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneUseType() As String
            Get
                Return Me.phoneUseTypeField
            End Get
            Set
                Me.phoneUseTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CountryAccessCode() As String
            Get
                Return Me.countryAccessCodeField
            End Get
            Set
                Me.countryAccessCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AreaCityCode() As String
            Get
                Return Me.areaCityCodeField
            End Get
            Set
                Me.areaCityCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PhoneNumber() As String
            Get
                Return Me.phoneNumberField
            End Get
            Set
                Me.phoneNumberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Extension() As String
            Get
                Return Me.extensionField
            End Get
            Set
                Me.extensionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PIN() As String
            Get
                Return Me.pINField
            End Get
            Set
                Me.pINField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Remark() As String
            Get
                Return Me.remarkField
            End Get
            Set
                Me.remarkField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property FormattedInd() As Boolean
            Get
                Return Me.formattedIndField
            End Get
            Set
                Me.formattedIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property FormattedIndSpecified() As Boolean
            Get
                Return Me.formattedIndFieldSpecified
            End Get
            Set
                Me.formattedIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DefaultInd() As Boolean
            Get
                Return Me.defaultIndField
            End Get
            Set
                Me.defaultIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DefaultIndSpecified() As Boolean
            Get
                Return Me.defaultIndFieldSpecified
            End Get
            Set
                Me.defaultIndFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RPH() As String
            Get
                Return Me.rPHField
            End Get
            Set
                Me.rPHField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleLocationAdditionalDetailsType

        Private vehRentLocInfosField() As VehicleLocationInformationType

        Private parkLocationField As VehicleWhereAtFacilityType

        Private counterLocationField As VehicleWhereAtFacilityType

        Private operationSchedulesField As OperationSchedulesType

        Private shuttleField As VehicleLocationAdditionalDetailsTypeShuttle

        Private oneWayDropLocationsField() As VehicleLocationAdditionalDetailsTypeOneWayDropLocation

        Private tPA_ExtensionsField As TPA_ExtensionsType

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("VehRentLocInfo", IsNullable:=False)>
        Public Property VehRentLocInfos() As VehicleLocationInformationType()
            Get
                Return Me.vehRentLocInfosField
            End Get
            Set
                Me.vehRentLocInfosField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property ParkLocation() As VehicleWhereAtFacilityType
            Get
                Return Me.parkLocationField
            End Get
            Set
                Me.parkLocationField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property CounterLocation() As VehicleWhereAtFacilityType
            Get
                Return Me.counterLocationField
            End Get
            Set
                Me.counterLocationField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property OperationSchedules() As OperationSchedulesType
            Get
                Return Me.operationSchedulesField
            End Get
            Set
                Me.operationSchedulesField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Shuttle() As VehicleLocationAdditionalDetailsTypeShuttle
            Get
                Return Me.shuttleField
            End Get
            Set
                Me.shuttleField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("OneWayDropLocation", IsNullable:=False)>
        Public Property OneWayDropLocations() As VehicleLocationAdditionalDetailsTypeOneWayDropLocation()
            Get
                Return Me.oneWayDropLocationsField
            End Get
            Set
                Me.oneWayDropLocationsField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property TPA_Extensions() As TPA_ExtensionsType
            Get
                Return Me.tPA_ExtensionsField
            End Get
            Set
                Me.tPA_ExtensionsField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleWhereAtFacilityType

        Private locationField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Location() As String
            Get
                Return Me.locationField
            End Get
            Set
                Me.locationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class OperationSchedulesType

        Private operationScheduleField() As OperationScheduleType

        Private startField As String

        Private durationField As String

        Private endField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("OperationSchedule")>
        Public Property OperationSchedule() As OperationScheduleType()
            Get
                Return Me.operationScheduleField
            End Get
            Set
                Me.operationScheduleField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Start() As String
            Get
                Return Me.startField
            End Get
            Set
                Me.startField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Duration() As String
            Get
                Return Me.durationField
            End Get
            Set
                Me.durationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property [End]() As String
            Get
                Return Me.endField
            End Get
            Set
                Me.endField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(PeriodPriceType)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(OperationSchedulePlusChargeType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class OperationScheduleType

        Private operationTimesField() As OperationScheduleTypeOperationTime

        Private startField As String

        Private durationField As String

        Private endField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("OperationTime", IsNullable:=False)>
        Public Property OperationTimes() As OperationScheduleTypeOperationTime()
            Get
                Return Me.operationTimesField
            End Get
            Set
                Me.operationTimesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Start() As String
            Get
                Return Me.startField
            End Get
            Set
                Me.startField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Duration() As String
            Get
                Return Me.durationField
            End Get
            Set
                Me.durationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property [End]() As String
            Get
                Return Me.endField
            End Get
            Set
                Me.endField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class OperationScheduleTypeOperationTime

        Private monField As Boolean

        Private monFieldSpecified As Boolean

        Private tueField As Boolean

        Private tueFieldSpecified As Boolean

        Private wedsField As Boolean

        Private wedsFieldSpecified As Boolean

        Private thurField As Boolean

        Private thurFieldSpecified As Boolean

        Private friField As Boolean

        Private friFieldSpecified As Boolean

        Private satField As Boolean

        Private satFieldSpecified As Boolean

        Private sunField As Boolean

        Private sunFieldSpecified As Boolean

        Private startField As String

        Private durationField As String

        Private endField As String

        Private additionalOperationInfoCodeField As String

        Private frequencyField As String

        Private textField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Mon() As Boolean
            Get
                Return Me.monField
            End Get
            Set
                Me.monField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MonSpecified() As Boolean
            Get
                Return Me.monFieldSpecified
            End Get
            Set
                Me.monFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Tue() As Boolean
            Get
                Return Me.tueField
            End Get
            Set
                Me.tueField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TueSpecified() As Boolean
            Get
                Return Me.tueFieldSpecified
            End Get
            Set
                Me.tueFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Weds() As Boolean
            Get
                Return Me.wedsField
            End Get
            Set
                Me.wedsField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property WedsSpecified() As Boolean
            Get
                Return Me.wedsFieldSpecified
            End Get
            Set
                Me.wedsFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Thur() As Boolean
            Get
                Return Me.thurField
            End Get
            Set
                Me.thurField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ThurSpecified() As Boolean
            Get
                Return Me.thurFieldSpecified
            End Get
            Set
                Me.thurFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Fri() As Boolean
            Get
                Return Me.friField
            End Get
            Set
                Me.friField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property FriSpecified() As Boolean
            Get
                Return Me.friFieldSpecified
            End Get
            Set
                Me.friFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Sat() As Boolean
            Get
                Return Me.satField
            End Get
            Set
                Me.satField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SatSpecified() As Boolean
            Get
                Return Me.satFieldSpecified
            End Get
            Set
                Me.satFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Sun() As Boolean
            Get
                Return Me.sunField
            End Get
            Set
                Me.sunField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SunSpecified() As Boolean
            Get
                Return Me.sunFieldSpecified
            End Get
            Set
                Me.sunFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Start() As String
            Get
                Return Me.startField
            End Get
            Set
                Me.startField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Duration() As String
            Get
                Return Me.durationField
            End Get
            Set
                Me.durationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property [End]() As String
            Get
                Return Me.endField
            End Get
            Set
                Me.endField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AdditionalOperationInfoCode() As String
            Get
                Return Me.additionalOperationInfoCodeField
            End Get
            Set
                Me.additionalOperationInfoCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Frequency() As String
            Get
                Return Me.frequencyField
            End Get
            Set
                Me.frequencyField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Text() As String
            Get
                Return Me.textField
            End Get
            Set
                Me.textField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PeriodPriceType
        Inherits OperationScheduleType

        Private priceField() As PkgPriceType

        Private rPHField As String

        Private categoryField As PeriodPriceTypeCategory

        Private categoryFieldSpecified As Boolean

        Private typeField As PeriodPriceTypeType

        Private typeFieldSpecified As Boolean

        Private durationPeriodField As String

        Private priceBasisField As PricingType

        Private priceBasisFieldSpecified As Boolean

        Private basePeriodRPHsField() As String

        Private guidePriceIndicatorField As Boolean

        Private guidePriceIndicatorFieldSpecified As Boolean

        Private maximumPeriodField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Price")>
        Public Property Price() As PkgPriceType()
            Get
                Return Me.priceField
            End Get
            Set
                Me.priceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RPH() As String
            Get
                Return Me.rPHField
            End Get
            Set
                Me.rPHField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Category() As PeriodPriceTypeCategory
            Get
                Return Me.categoryField
            End Get
            Set
                Me.categoryField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property CategorySpecified() As Boolean
            Get
                Return Me.categoryFieldSpecified
            End Get
            Set
                Me.categoryFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As PeriodPriceTypeType
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TypeSpecified() As Boolean
            Get
                Return Me.typeFieldSpecified
            End Get
            Set
                Me.typeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DurationPeriod() As String
            Get
                Return Me.durationPeriodField
            End Get
            Set
                Me.durationPeriodField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PriceBasis() As PricingType
            Get
                Return Me.priceBasisField
            End Get
            Set
                Me.priceBasisField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PriceBasisSpecified() As Boolean
            Get
                Return Me.priceBasisFieldSpecified
            End Get
            Set
                Me.priceBasisFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property BasePeriodRPHs() As String()
            Get
                Return Me.basePeriodRPHsField
            End Get
            Set
                Me.basePeriodRPHsField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property GuidePriceIndicator() As Boolean
            Get
                Return Me.guidePriceIndicatorField
            End Get
            Set
                Me.guidePriceIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property GuidePriceIndicatorSpecified() As Boolean
            Get
                Return Me.guidePriceIndicatorFieldSpecified
            End Get
            Set
                Me.guidePriceIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MaximumPeriod() As String
            Get
                Return Me.maximumPeriodField
            End Get
            Set
                Me.maximumPeriodField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class PkgPriceType

        Private ageField As String

        Private codeField As String

        Private codeContextField As String

        Private uRIField As String

        Private quantityField As Long

        Private quantityFieldSpecified As Boolean

        Private amountField As Decimal

        Private amountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        Private priceBasisField As PricingType

        Private priceBasisFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="integer")>
        Public Property Age() As String
            Get
                Return Me.ageField
            End Get
            Set
                Me.ageField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CodeContext() As String
            Get
                Return Me.codeContextField
            End Get
            Set
                Me.codeContextField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")>
        Public Property URI() As String
            Get
                Return Me.uRIField
            End Get
            Set
                Me.uRIField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Quantity() As Long
            Get
                Return Me.quantityField
            End Get
            Set
                Me.quantityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property QuantitySpecified() As Boolean
            Get
                Return Me.quantityFieldSpecified
            End Get
            Set
                Me.quantityFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PriceBasis() As PricingType
            Get
                Return Me.priceBasisField
            End Get
            Set
                Me.priceBasisField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PriceBasisSpecified() As Boolean
            Get
                Return Me.priceBasisFieldSpecified
            End Get
            Set
                Me.priceBasisFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum PricingType

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("Per stay")>
        Perstay

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("Per person")>
        Perperson

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("Per night")>
        Pernight

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("Per person per night")>
        Perpersonpernight

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("Per use")>
        Peruse
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum PeriodPriceTypeCategory

        '''<remarks/>
        Room

        '''<remarks/>
        Booking

        '''<remarks/>
        Person

        '''<remarks/>
        Adult

        '''<remarks/>
        Child

        '''<remarks/>
        Car
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum PeriodPriceTypeType

        '''<remarks/>
        Base

        '''<remarks/>
        AddOn
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class OperationSchedulePlusChargeType
        Inherits OperationScheduleType

        Private chargeField() As FeeType

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Charge")>
        Public Property Charge() As FeeType()
            Get
                Return Me.chargeField
            End Get
            Set
                Me.chargeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class FeeType

        Private taxesField As TaxesType

        Private descriptionField() As ParagraphType

        Private taxInclusiveField As Boolean

        Private taxInclusiveFieldSpecified As Boolean

        Private typeField As AmountDeterminationType

        Private typeFieldSpecified As Boolean

        Private codeField As String

        Private percentField As Decimal

        Private percentFieldSpecified As Boolean

        Private amountField As Decimal

        Private amountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        Private effectiveDateField As Date

        Private effectiveDateFieldSpecified As Boolean

        Private expireDateField As Date

        Private expireDateFieldSpecified As Boolean

        Private expireDateExclusiveIndicatorField As Boolean

        Private expireDateExclusiveIndicatorFieldSpecified As Boolean

        Private mandatoryIndicatorField As Boolean

        Private mandatoryIndicatorFieldSpecified As Boolean

        Private rPHField As String

        Private chargeUnitField As String

        Private chargeFrequencyField As String

        Private chargeUnitExemptField As Long

        Private chargeUnitExemptFieldSpecified As Boolean

        Private chargeFrequencyExemptField As Long

        Private chargeFrequencyExemptFieldSpecified As Boolean

        Private maxChargeUnitAppliesField As Long

        Private maxChargeUnitAppliesFieldSpecified As Boolean

        Private maxChargeFrequencyAppliesField As Long

        Private maxChargeFrequencyAppliesFieldSpecified As Boolean

        Private taxableIndicatorField As Boolean

        Private taxableIndicatorFieldSpecified As Boolean

        '''<remarks/>
        Public Property Taxes() As TaxesType
            Get
                Return Me.taxesField
            End Get
            Set
                Me.taxesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Description")>
        Public Property Description() As ParagraphType()
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TaxInclusive() As Boolean
            Get
                Return Me.taxInclusiveField
            End Get
            Set
                Me.taxInclusiveField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TaxInclusiveSpecified() As Boolean
            Get
                Return Me.taxInclusiveFieldSpecified
            End Get
            Set
                Me.taxInclusiveFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As AmountDeterminationType
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TypeSpecified() As Boolean
            Get
                Return Me.typeFieldSpecified
            End Get
            Set
                Me.typeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Percent() As Decimal
            Get
                Return Me.percentField
            End Get
            Set
                Me.percentField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PercentSpecified() As Boolean
            Get
                Return Me.percentFieldSpecified
            End Get
            Set
                Me.percentFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property EffectiveDate() As Date
            Get
                Return Me.effectiveDateField
            End Get
            Set
                Me.effectiveDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EffectiveDateSpecified() As Boolean
            Get
                Return Me.effectiveDateFieldSpecified
            End Get
            Set
                Me.effectiveDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property ExpireDate() As Date
            Get
                Return Me.expireDateField
            End Get
            Set
                Me.expireDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateSpecified() As Boolean
            Get
                Return Me.expireDateFieldSpecified
            End Get
            Set
                Me.expireDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExpireDateExclusiveIndicator() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorField
            End Get
            Set
                Me.expireDateExclusiveIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateExclusiveIndicatorSpecified() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorFieldSpecified
            End Get
            Set
                Me.expireDateExclusiveIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MandatoryIndicator() As Boolean
            Get
                Return Me.mandatoryIndicatorField
            End Get
            Set
                Me.mandatoryIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MandatoryIndicatorSpecified() As Boolean
            Get
                Return Me.mandatoryIndicatorFieldSpecified
            End Get
            Set
                Me.mandatoryIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RPH() As String
            Get
                Return Me.rPHField
            End Get
            Set
                Me.rPHField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ChargeUnit() As String
            Get
                Return Me.chargeUnitField
            End Get
            Set
                Me.chargeUnitField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ChargeFrequency() As String
            Get
                Return Me.chargeFrequencyField
            End Get
            Set
                Me.chargeFrequencyField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ChargeUnitExempt() As Long
            Get
                Return Me.chargeUnitExemptField
            End Get
            Set
                Me.chargeUnitExemptField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ChargeUnitExemptSpecified() As Boolean
            Get
                Return Me.chargeUnitExemptFieldSpecified
            End Get
            Set
                Me.chargeUnitExemptFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ChargeFrequencyExempt() As Long
            Get
                Return Me.chargeFrequencyExemptField
            End Get
            Set
                Me.chargeFrequencyExemptField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ChargeFrequencyExemptSpecified() As Boolean
            Get
                Return Me.chargeFrequencyExemptFieldSpecified
            End Get
            Set
                Me.chargeFrequencyExemptFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MaxChargeUnitApplies() As Long
            Get
                Return Me.maxChargeUnitAppliesField
            End Get
            Set
                Me.maxChargeUnitAppliesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MaxChargeUnitAppliesSpecified() As Boolean
            Get
                Return Me.maxChargeUnitAppliesFieldSpecified
            End Get
            Set
                Me.maxChargeUnitAppliesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MaxChargeFrequencyApplies() As Long
            Get
                Return Me.maxChargeFrequencyAppliesField
            End Get
            Set
                Me.maxChargeFrequencyAppliesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MaxChargeFrequencyAppliesSpecified() As Boolean
            Get
                Return Me.maxChargeFrequencyAppliesFieldSpecified
            End Get
            Set
                Me.maxChargeFrequencyAppliesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TaxableIndicator() As Boolean
            Get
                Return Me.taxableIndicatorField
            End Get
            Set
                Me.taxableIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TaxableIndicatorSpecified() As Boolean
            Get
                Return Me.taxableIndicatorFieldSpecified
            End Get
            Set
                Me.taxableIndicatorFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class TaxesType

        Private taxField() As TaxType

        Private amountField As Decimal

        Private amountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Tax")>
        Public Property Tax() As TaxType()
            Get
                Return Me.taxField
            End Get
            Set
                Me.taxField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class TaxType

        Private taxDescriptionField() As ParagraphType

        Private dummyElementField As Object

        Private typeField As AmountDeterminationType

        Private typeFieldSpecified As Boolean

        Private codeField As String

        Private percentField As Decimal

        Private percentFieldSpecified As Boolean

        Private amountField As Decimal

        Private amountFieldSpecified As Boolean

        Private currencyCodeField As String

        Private decimalPlacesField As Long

        Private decimalPlacesFieldSpecified As Boolean

        Private effectiveDateField As Date

        Private effectiveDateFieldSpecified As Boolean

        Private expireDateField As Date

        Private expireDateFieldSpecified As Boolean

        Private expireDateExclusiveIndicatorField As Boolean

        Private expireDateExclusiveIndicatorFieldSpecified As Boolean

        Private chargeUnitField As String

        Private chargeFrequencyField As String

        Private chargeUnitExemptField As Long

        Private chargeUnitExemptFieldSpecified As Boolean

        Private chargeFrequencyExemptField As Long

        Private chargeFrequencyExemptFieldSpecified As Boolean

        Private maxChargeUnitAppliesField As Long

        Private maxChargeUnitAppliesFieldSpecified As Boolean

        Private maxChargeFrequencyAppliesField As Long

        Private maxChargeFrequencyAppliesFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TaxDescription")>
        Public Property TaxDescription() As ParagraphType()
            Get
                Return Me.taxDescriptionField
            End Get
            Set
                Me.taxDescriptionField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property DummyElement() As Object
            Get
                Return Me.dummyElementField
            End Get
            Set
                Me.dummyElementField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As AmountDeterminationType
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TypeSpecified() As Boolean
            Get
                Return Me.typeFieldSpecified
            End Get
            Set
                Me.typeFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Percent() As Decimal
            Get
                Return Me.percentField
            End Get
            Set
                Me.percentField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PercentSpecified() As Boolean
            Get
                Return Me.percentFieldSpecified
            End Get
            Set
                Me.percentFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AmountSpecified() As Boolean
            Get
                Return Me.amountFieldSpecified
            End Get
            Set
                Me.amountFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As Long
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DecimalPlacesSpecified() As Boolean
            Get
                Return Me.decimalPlacesFieldSpecified
            End Get
            Set
                Me.decimalPlacesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property EffectiveDate() As Date
            Get
                Return Me.effectiveDateField
            End Get
            Set
                Me.effectiveDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EffectiveDateSpecified() As Boolean
            Get
                Return Me.effectiveDateFieldSpecified
            End Get
            Set
                Me.effectiveDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>
        Public Property ExpireDate() As Date
            Get
                Return Me.expireDateField
            End Get
            Set
                Me.expireDateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateSpecified() As Boolean
            Get
                Return Me.expireDateFieldSpecified
            End Get
            Set
                Me.expireDateFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExpireDateExclusiveIndicator() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorField
            End Get
            Set
                Me.expireDateExclusiveIndicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ExpireDateExclusiveIndicatorSpecified() As Boolean
            Get
                Return Me.expireDateExclusiveIndicatorFieldSpecified
            End Get
            Set
                Me.expireDateExclusiveIndicatorFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ChargeUnit() As String
            Get
                Return Me.chargeUnitField
            End Get
            Set
                Me.chargeUnitField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ChargeFrequency() As String
            Get
                Return Me.chargeFrequencyField
            End Get
            Set
                Me.chargeFrequencyField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ChargeUnitExempt() As Long
            Get
                Return Me.chargeUnitExemptField
            End Get
            Set
                Me.chargeUnitExemptField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ChargeUnitExemptSpecified() As Boolean
            Get
                Return Me.chargeUnitExemptFieldSpecified
            End Get
            Set
                Me.chargeUnitExemptFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ChargeFrequencyExempt() As Long
            Get
                Return Me.chargeFrequencyExemptField
            End Get
            Set
                Me.chargeFrequencyExemptField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ChargeFrequencyExemptSpecified() As Boolean
            Get
                Return Me.chargeFrequencyExemptFieldSpecified
            End Get
            Set
                Me.chargeFrequencyExemptFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MaxChargeUnitApplies() As Long
            Get
                Return Me.maxChargeUnitAppliesField
            End Get
            Set
                Me.maxChargeUnitAppliesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MaxChargeUnitAppliesSpecified() As Boolean
            Get
                Return Me.maxChargeUnitAppliesFieldSpecified
            End Get
            Set
                Me.maxChargeUnitAppliesFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MaxChargeFrequencyApplies() As Long
            Get
                Return Me.maxChargeFrequencyAppliesField
            End Get
            Set
                Me.maxChargeFrequencyAppliesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MaxChargeFrequencyAppliesSpecified() As Boolean
            Get
                Return Me.maxChargeFrequencyAppliesFieldSpecified
            End Get
            Set
                Me.maxChargeFrequencyAppliesFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum AmountDeterminationType

        '''<remarks/>
        Inclusive

        '''<remarks/>
        Exclusive

        '''<remarks/>
        Cumulative
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleLocationAdditionalDetailsTypeShuttle

        Private shuttleInfosField() As VehicleLocationAdditionalDetailsTypeShuttleShuttleInfo

        Private operationSchedulesField As OperationSchedulesType

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("ShuttleInfo", IsNullable:=False)>
        Public Property ShuttleInfos() As VehicleLocationAdditionalDetailsTypeShuttleShuttleInfo()
            Get
                Return Me.shuttleInfosField
            End Get
            Set
                Me.shuttleInfosField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property OperationSchedules() As OperationSchedulesType
            Get
                Return Me.operationSchedulesField
            End Get
            Set
                Me.operationSchedulesField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleLocationAdditionalDetailsTypeShuttleShuttleInfo
        Inherits FormattedTextType

        Private typeField As LocationDetailShuttleInfoType

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As LocationDetailShuttleInfoType
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum LocationDetailShuttleInfoType

        '''<remarks/>
        Transportation

        '''<remarks/>
        Frequency

        '''<remarks/>
        PickupInfo

        '''<remarks/>
        Distance

        '''<remarks/>
        ElapsedTime

        '''<remarks/>
        Fee

        '''<remarks/>
        Miscellaneous

        '''<remarks/>
        Hours
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleLocationAdditionalDetailsTypeOneWayDropLocation
        Inherits LocationType

        Private extendedLocationCodeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ExtendedLocationCode() As String
            Get
                Return Me.extendedLocationCodeField
            End Get
            Set
                Me.extendedLocationCodeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleTourInfoType

        Private tourOperatorField As CompanyNameType

        Private tourNumberField As String

        '''<remarks/>
        Public Property TourOperator() As CompanyNameType
            Get
                Return Me.tourOperatorField
            End Get
            Set
                Me.tourOperatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TourNumber() As String
            Get
                Return Me.tourNumberField
            End Get
            Set
                Me.tourNumberField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleSpecialReqPrefType

        Private preferLevelField As PreferLevelType

        Private preferLevelFieldSpecified As Boolean

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PreferLevel() As PreferLevelType
            Get
                Return Me.preferLevelField
            End Get
            Set
                Me.preferLevelField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PreferLevelSpecified() As Boolean
            Get
                Return Me.preferLevelFieldSpecified
            End Get
            Set
                Me.preferLevelFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class VehicleArrivalDetailsType

        Private arrivalLocationField As LocationType

        Private marketingCompanyField As CompanyNameType

        Private operatingCompanyField As CompanyNameType

        Private transportationCodeField As String

        Private numberField As String

        Private arrivalDateTimeField As Date

        Private arrivalDateTimeFieldSpecified As Boolean

        '''<remarks/>
        Public Property ArrivalLocation() As LocationType
            Get
                Return Me.arrivalLocationField
            End Get
            Set
                Me.arrivalLocationField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property MarketingCompany() As CompanyNameType
            Get
                Return Me.marketingCompanyField
            End Get
            Set
                Me.marketingCompanyField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property OperatingCompany() As CompanyNameType
            Get
                Return Me.operatingCompanyField
            End Get
            Set
                Me.operatingCompanyField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TransportationCode() As String
            Get
                Return Me.transportationCodeField
            End Get
            Set
                Me.transportationCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Number() As String
            Get
                Return Me.numberField
            End Get
            Set
                Me.numberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ArrivalDateTime() As Date
            Get
                Return Me.arrivalDateTimeField
            End Get
            Set
                Me.arrivalDateTimeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ArrivalDateTimeSpecified() As Boolean
            Get
                Return Me.arrivalDateTimeFieldSpecified
            End Get
            Set
                Me.arrivalDateTimeFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class WrittenConfInstType

        Private supplementalDataField As ParagraphType

        Private emailField As EmailType

        Private languageIDField As String

        Private addresseeNameField As String

        Private addressField As String

        Private telephoneField As String

        Private confirmIndField As Boolean

        Private confirmIndFieldSpecified As Boolean

        '''<remarks/>
        Public Property SupplementalData() As ParagraphType
            Get
                Return Me.supplementalDataField
            End Get
            Set
                Me.supplementalDataField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Email() As EmailType
            Get
                Return Me.emailField
            End Get
            Set
                Me.emailField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property LanguageID() As String
            Get
                Return Me.languageIDField
            End Get
            Set
                Me.languageIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AddresseeName() As String
            Get
                Return Me.addresseeNameField
            End Get
            Set
                Me.addresseeNameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Address() As String
            Get
                Return Me.addressField
            End Get
            Set
                Me.addressField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Telephone() As String
            Get
                Return Me.telephoneField
            End Get
            Set
                Me.telephoneField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ConfirmInd() As Boolean
            Get
                Return Me.confirmIndField
            End Get
            Set
                Me.confirmIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ConfirmIndSpecified() As Boolean
            Get
                Return Me.confirmIndFieldSpecified
            End Get
            Set
                Me.confirmIndFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A"),
        System.Xml.Serialization.XmlRootAttribute("AccommodationCategory", [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable:=False)>
    Partial Public Class AccommodationCategoryType

        Private accommodationField() As AccommodationCategoryTypeAccommodation

        Private ancillaryServiceField() As AncillaryService

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Accommodation")>
        Public Property Accommodation() As AccommodationCategoryTypeAccommodation()
            Get
                Return Me.accommodationField
            End Get
            Set
                Me.accommodationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AncillaryService")>
        Public Property AncillaryService() As AncillaryService()
            Get
                Return Me.ancillaryServiceField
            End Get
            Set
                Me.ancillaryServiceField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class AccommodationCategoryTypeAccommodation
        Inherits AccommodationType

        Private quantityField As Long

        Private quantityFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Quantity() As Long
            Get
                Return Me.quantityField
            End Get
            Set
                Me.quantityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property QuantitySpecified() As Boolean
            Get
                Return Me.quantityFieldSpecified
            End Get
            Set
                Me.quantityFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class AccommodationType

        Private itemField As Object

        Private classField As AccommodationClass

        Private compartmentField As CompartmentType

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Berth", GetType(BerthAccommodationType)),
            System.Xml.Serialization.XmlElementAttribute("Seat", GetType(SeatAccommodationType))>
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set
                Me.itemField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property [Class]() As AccommodationClass
            Get
                Return Me.classField
            End Get
            Set
                Me.classField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Compartment() As CompartmentType
            Get
                Return Me.compartmentField
            End Get
            Set
                Me.compartmentField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum BerthAccommodationType

        '''<remarks/>
        NotSignificant

        '''<remarks/>
        Berth

        '''<remarks/>
        Couchette

        '''<remarks/>
        Sleeper
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum SeatAccommodationType

        '''<remarks/>
        NotSignificant

        '''<remarks/>
        Seat

        '''<remarks/>
        Sleeperette

        '''<remarks/>
        NoSeat
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class AccommodationClass

        Private extensionField As String

        Private valueField As AccommodationClassEnum

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property extension() As String
            Get
                Return Me.extensionField
            End Get
            Set
                Me.extensionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As AccommodationClassEnum
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum AccommodationClassEnum

        '''<remarks/>
        FirstClass

        '''<remarks/>
        SecondClass

        '''<remarks/>
        Premium

        '''<remarks/>
        Business

        '''<remarks/>
        Leisure

        '''<remarks/>
        Coach

        '''<remarks/>
        Deluxe

        '''<remarks/>
        GranClasse

        '''<remarks/>
        SoftClass

        '''<remarks/>
        HardClass

        '''<remarks/>
        SpecialClass

        '''<remarks/>
        HighGradeSoftClass

        '''<remarks/>
        MixedHardClass

        '''<remarks/>
        MixedSoftClass

        '''<remarks/>
        SoftCompartmentClass

        '''<remarks/>
        HardCompartmentClass

        '''<remarks/>
        Other_
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class CompartmentType

        Private extensionField As String

        Private valueField As CompartmentTypeEnum

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property extension() As String
            Get
                Return Me.extensionField
            End Get
            Set
                Me.extensionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As CompartmentTypeEnum
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum CompartmentTypeEnum

        '''<remarks/>
        NotSignificant

        '''<remarks/>
        Family

        '''<remarks/>
        Quite

        '''<remarks/>
        Conference

        '''<remarks/>
        CompartmentWithoutAnimals

        '''<remarks/>
        Complete

        '''<remarks/>
        Video

        '''<remarks/>
        Pram

        '''<remarks/>
        WomanAndChild

        '''<remarks/>
        EasyAccess

        '''<remarks/>
        T2

        '''<remarks/>
        T3

        '''<remarks/>
        T4

        '''<remarks/>
        T6

        '''<remarks/>
        C2

        '''<remarks/>
        C4

        '''<remarks/>
        C5

        '''<remarks/>
        C6

        '''<remarks/>
        [Single]

        '''<remarks/>
        [Double]

        '''<remarks/>
        SingleSuite

        '''<remarks/>
        DoubleSuite

        '''<remarks/>
        Special

        '''<remarks/>
        Other_
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A"),
        System.Xml.Serialization.XmlRootAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable:=False)>
    Partial Public Class AncillaryService
        Inherits AncillaryServiceType

        Private preferLevelField As PreferLevelType

        Private preferLevelFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PreferLevel() As PreferLevelType
            Get
                Return Me.preferLevelField
            End Get
            Set
                Me.preferLevelField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PreferLevelSpecified() As Boolean
            Get
                Return Me.preferLevelFieldSpecified
            End Get
            Set
                Me.preferLevelFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class AncillaryServiceType

        Private codeField As String

        Private codeContextField As String

        Private quantityField As Long

        Private quantityFieldSpecified As Boolean

        Private descriptionField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CodeContext() As String
            Get
                Return Me.codeContextField
            End Get
            Set
                Me.codeContextField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Quantity() As Long
            Get
                Return Me.quantityField
            End Get
            Set
                Me.quantityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property QuantitySpecified() As Boolean
            Get
                Return Me.quantityFieldSpecified
            End Get
            Set
                Me.quantityFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A"),
        System.Xml.Serialization.XmlRootAttribute("AccommodationService", [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable:=False)>
    Partial Public Class AccommodationServiceType

        Private accommodationDetailField As AccommodationServiceTypeAccommodationDetail

        Private ancillaryServiceField() As AncillaryService

        '''<remarks/>
        Public Property AccommodationDetail() As AccommodationServiceTypeAccommodationDetail
            Get
                Return Me.accommodationDetailField
            End Get
            Set
                Me.accommodationDetailField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AncillaryService")>
        Public Property AncillaryService() As AncillaryService()
            Get
                Return Me.ancillaryServiceField
            End Get
            Set
                Me.ancillaryServiceField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class AccommodationServiceTypeAccommodationDetail
        Inherits RailAccommDetailType

        Private referenceTravelerRPHField As String

        Private referenceIndField As Boolean

        Private referenceIndFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ReferenceTravelerRPH() As String
            Get
                Return Me.referenceTravelerRPHField
            End Get
            Set
                Me.referenceTravelerRPHField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ReferenceInd() As Boolean
            Get
                Return Me.referenceIndField
            End Get
            Set
                Me.referenceIndField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ReferenceIndSpecified() As Boolean
            Get
                Return Me.referenceIndFieldSpecified
            End Get
            Set
                Me.referenceIndFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class RailAccommDetailType

        Private itemField As Object

        Private classField As AccommodationClass

        Private compartmentField As RailAccommDetailTypeCompartment

        Private carField As RailAccommDetailTypeCar

        Private deckField As DeckType

        Private deckFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Berth", GetType(BerthDetailType)),
            System.Xml.Serialization.XmlElementAttribute("Seat", GetType(SeatDetailType))>
        Public Property Item() As Object
            Get
                Return Me.itemField
            End Get
            Set
                Me.itemField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property [Class]() As AccommodationClass
            Get
                Return Me.classField
            End Get
            Set
                Me.classField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Compartment() As RailAccommDetailTypeCompartment
            Get
                Return Me.compartmentField
            End Get
            Set
                Me.compartmentField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Car() As RailAccommDetailTypeCar
            Get
                Return Me.carField
            End Get
            Set
                Me.carField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Deck() As DeckType
            Get
                Return Me.deckField
            End Get
            Set
                Me.deckField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DeckSpecified() As Boolean
            Get
                Return Me.deckFieldSpecified
            End Get
            Set
                Me.deckFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class BerthDetailType

        Private numberField As String

        Private positionField As BerthPositionType

        Private positionFieldSpecified As Boolean

        Private valueField As BerthAccommodationType

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Number() As String
            Get
                Return Me.numberField
            End Get
            Set
                Me.numberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Position() As BerthPositionType
            Get
                Return Me.positionField
            End Get
            Set
                Me.positionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PositionSpecified() As Boolean
            Get
                Return Me.positionFieldSpecified
            End Get
            Set
                Me.positionFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As BerthAccommodationType
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum BerthPositionType

        '''<remarks/>
        Upper

        '''<remarks/>
        Middle

        '''<remarks/>
        Lower
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class SeatDetailType

        Private numberField As String

        Private positionField As SeatPositionType

        Private positionFieldSpecified As Boolean

        Private directionField As SeatDirectionType

        Private directionFieldSpecified As Boolean

        Private valueField As SeatAccommodationType

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Number() As String
            Get
                Return Me.numberField
            End Get
            Set
                Me.numberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Position() As SeatPositionType
            Get
                Return Me.positionField
            End Get
            Set
                Me.positionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PositionSpecified() As Boolean
            Get
                Return Me.positionFieldSpecified
            End Get
            Set
                Me.positionFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Direction() As SeatDirectionType
            Get
                Return Me.directionField
            End Get
            Set
                Me.directionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DirectionSpecified() As Boolean
            Get
                Return Me.directionFieldSpecified
            End Get
            Set
                Me.directionFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As SeatAccommodationType
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum SeatPositionType

        '''<remarks/>
        None

        '''<remarks/>
        Together

        '''<remarks/>
        Aisle

        '''<remarks/>
        Center

        '''<remarks/>
        Window

        '''<remarks/>
        Specific

        '''<remarks/>
        [Exit]

        '''<remarks/>
        Table

        '''<remarks/>
        AdjacentAisle

        '''<remarks/>
        Individual

        '''<remarks/>
        Middle
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum SeatDirectionType

        '''<remarks/>
        Facing

        '''<remarks/>
        Back

        '''<remarks/>
        Airline

        '''<remarks/>
        Lateral

        '''<remarks/>
        Unknown
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class RailAccommDetailTypeCompartment
        Inherits CompartmentType

        Private numberField As String

        Private positionField As CompartmentPositionType

        Private positionFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Number() As String
            Get
                Return Me.numberField
            End Get
            Set
                Me.numberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Position() As CompartmentPositionType
            Get
                Return Me.positionField
            End Get
            Set
                Me.positionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PositionSpecified() As Boolean
            Get
                Return Me.positionFieldSpecified
            End Get
            Set
                Me.positionFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum CompartmentPositionType

        '''<remarks/>
        CloseToRestaurantCar

        '''<remarks/>
        CloseToExit

        '''<remarks/>
        CloseToToilet

        '''<remarks/>
        MiddleOfCar
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Partial Public Class RailAccommDetailTypeCar

        Private numberField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Number() As String
            Get
                Return Me.numberField
            End Get
            Set
                Me.numberField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A")>
    Public Enum DeckType

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("Regular-OneLevelOnly")>
        RegularOneLevelOnly

        '''<remarks/>
        LowerLevel

        '''<remarks/>
        UpperLevel
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A"),
        System.Xml.Serialization.XmlRootAttribute("Success", [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable:=False)>
    Partial Public Class SuccessType
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(TypeName:="WarningsType", [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A"),
        System.Xml.Serialization.XmlRootAttribute("Warnings", [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable:=False)>
    Partial Public Class WarningsType1

        Private warningField() As WarningType1

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Warning")>
        Public Property Warning() As WarningType1()
            Get
                Return Me.warningField
            End Get
            Set
                Me.warningField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(TypeName:="ErrorsType", [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A"),
        System.Xml.Serialization.XmlRootAttribute("Errors", [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable:=False)>
    Partial Public Class ErrorsType1

        Private errorField() As ErrorType1

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Error")>
        Public Property [Error]() As ErrorType1()
            Get
                Return Me.errorField
            End Get
            Set
                Me.errorField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(TypeName:="WarningsType", [Namespace]:="http://www.iata.org/IATA/2007/00/IATA2010.1"),
        System.Xml.Serialization.XmlRootAttribute("Warnings", [Namespace]:="http://www.iata.org/IATA/2007/00/IATA2010.1", IsNullable:=False)>
    Partial Public Class WarningsType2

        Private warningField() As WarningType2

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Warning")>
        Public Property Warning() As WarningType2()
            Get
                Return Me.warningField
            End Get
            Set
                Me.warningField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(TypeName:="WarningType", [Namespace]:="http://www.iata.org/IATA/2007/00/IATA2010.1")>
    Partial Public Class WarningType2
        Inherits FreeTextType1

        Private typeField As String

        Private shortTextField As String

        Private codeField As String

        Private docURLField As String

        Private statusField As String

        Private tagField As String

        Private recordIDField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShortText() As String
            Get
                Return Me.shortTextField
            End Get
            Set
                Me.shortTextField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")>
        Public Property DocURL() As String
            Get
                Return Me.docURLField
            End Get
            Set
                Me.docURLField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Status() As String
            Get
                Return Me.statusField
            End Get
            Set
                Me.statusField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Tag() As String
            Get
                Return Me.tagField
            End Get
            Set
                Me.tagField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RecordID() As String
            Get
                Return Me.recordIDField
            End Get
            Set
                Me.recordIDField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(WarningType2)),
        System.Xml.Serialization.XmlIncludeAttribute(GetType(ErrorType2)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(TypeName:="FreeTextType", [Namespace]:="http://www.iata.org/IATA/2007/00/IATA2010.1")>
    Partial Public Class FreeTextType1

        Private languageField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")>
        Public Property Language() As String
            Get
                Return Me.languageField
            End Get
            Set
                Me.languageField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(TypeName:="ErrorType", [Namespace]:="http://www.iata.org/IATA/2007/00/IATA2010.1")>
    Partial Public Class ErrorType2
        Inherits FreeTextType1

        Private typeField As String

        Private shortTextField As String

        Private codeField As String

        Private docURLField As String

        Private statusField As String

        Private tagField As String

        Private recordIDField As String

        Private nodeListField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ShortText() As String
            Get
                Return Me.shortTextField
            End Get
            Set
                Me.shortTextField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")>
        Public Property DocURL() As String
            Get
                Return Me.docURLField
            End Get
            Set
                Me.docURLField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Status() As String
            Get
                Return Me.statusField
            End Get
            Set
                Me.statusField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Tag() As String
            Get
                Return Me.tagField
            End Get
            Set
                Me.tagField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property RecordID() As String
            Get
                Return Me.recordIDField
            End Get
            Set
                Me.recordIDField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property NodeList() As String
            Get
                Return Me.nodeListField
            End Get
            Set
                Me.nodeListField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(TypeName:="ErrorsType", [Namespace]:="http://www.iata.org/IATA/2007/00/IATA2010.1"),
        System.Xml.Serialization.XmlRootAttribute("Errors", [Namespace]:="http://www.iata.org/IATA/2007/00/IATA2010.1", IsNullable:=False)>
    Partial Public Class ErrorsType2

        Private errorField() As ErrorType2

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Error")>
        Public Property [Error]() As ErrorType2()
            Get
                Return Me.errorField
            End Get
            Set
                Me.errorField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2"),
        System.Xml.Serialization.XmlRootAttribute([Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2", IsNullable:=False)>
    Partial Public Class QtElement

        Private priceField() As QuotationGenericElementTypePrice

        Private taxField() As QuotationGenericElementTypeTax

        Private pointField() As PointType

        Private quotationTypeField As FullQuotationType_Type

        Private flagField() As FlagType

        Private stateField As QuotationGenericElementTypePricingInfoState

        Private stateFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Price")>
        Public Property Price() As QuotationGenericElementTypePrice()
            Get
                Return Me.priceField
            End Get
            Set
                Me.priceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Tax")>
        Public Property Tax() As QuotationGenericElementTypeTax()
            Get
                Return Me.taxField
            End Get
            Set
                Me.taxField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Point")>
        Public Property Point() As PointType()
            Get
                Return Me.pointField
            End Get
            Set
                Me.pointField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property QuotationType() As FullQuotationType_Type
            Get
                Return Me.quotationTypeField
            End Get
            Set
                Me.quotationTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Flag")>
        Public Property Flag() As FlagType()
            Get
                Return Me.flagField
            End Get
            Set
                Me.flagField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property State() As QuotationGenericElementTypePricingInfoState
            Get
                Return Me.stateField
            End Get
            Set
                Me.stateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StateSpecified() As Boolean
            Get
                Return Me.stateFieldSpecified
            End Get
            Set
                Me.stateFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class QuotationGenericElementTypePrice

        Private currencyCodeField As String

        Private decimalPlacesField As String

        Private amountField As String

        Private miscValueField As FareOrRateTypeMiscValue

        Private miscValueFieldSpecified As Boolean

        Private typeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As String
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As String
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MiscValue() As FareOrRateTypeMiscValue
            Get
                Return Me.miscValueField
            End Get
            Set
                Me.miscValueField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MiscValueSpecified() As Boolean
            Get
                Return Me.miscValueFieldSpecified
            End Get
            Set
                Me.miscValueFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Public Enum FareOrRateTypeMiscValue

        '''<remarks/>
        EXEMPTED

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("")>
        Item
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class QuotationGenericElementTypeTax

        Private currencyCodeField As String

        Private decimalPlacesField As String

        Private amountField As String

        Private miscValueField As QuotationGenericElementTypeTaxMiscValue

        Private miscValueFieldSpecified As Boolean

        Private typeField As String

        Private isExemptedField As String

        Private natureCodeField As String

        Private indicatorField As String

        Private isoCodeField As String

        Private rateField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As String
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As String
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MiscValue() As QuotationGenericElementTypeTaxMiscValue
            Get
                Return Me.miscValueField
            End Get
            Set
                Me.miscValueField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MiscValueSpecified() As Boolean
            Get
                Return Me.miscValueFieldSpecified
            End Get
            Set
                Me.miscValueFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property IsExempted() As String
            Get
                Return Me.isExemptedField
            End Get
            Set
                Me.isExemptedField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property NatureCode() As String
            Get
                Return Me.natureCodeField
            End Get
            Set
                Me.natureCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Indicator() As String
            Get
                Return Me.indicatorField
            End Get
            Set
                Me.indicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property IsoCode() As String
            Get
                Return Me.isoCodeField
            End Get
            Set
                Me.isoCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Rate() As String
            Get
                Return Me.rateField
            End Get
            Set
                Me.rateField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Public Enum QuotationGenericElementTypeTaxMiscValue

        '''<remarks/>
        EXEMPTED

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("")>
        Item
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class PointType

        Private typeField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute(DataType:="integer")>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class FullQuotationType_Type

        Private descriptionField As FullQuotationType_TypeDescription

        Private descriptionFieldSpecified As Boolean

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Description() As FullQuotationType_TypeDescription
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DescriptionSpecified() As Boolean
            Get
                Return Me.descriptionFieldSpecified
            End Get
            Set
                Me.descriptionFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute(DataType:="nonNegativeInteger")>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Public Enum FullQuotationType_TypeDescription

        '''<remarks/>
        QT_DEFAULT

        '''<remarks/>
        QT_GEN

        '''<remarks/>
        QT_PR

        '''<remarks/>
        QT_DOC_PR

        '''<remarks/>
        QT_ITI_PR

        '''<remarks/>
        QT_ITI

        '''<remarks/>
        QT_ATC_PR

        '''<remarks/>
        QT_DOC

        '''<remarks/>
        QT_OBF

        '''<remarks/>
        QT_OBF_DET

        '''<remarks/>
        QT_CPN

        '''<remarks/>
        QT_PCK

        '''<remarks/>
        QT_DOC_CPN

        '''<remarks/>
        QT_ITI_CPN

        '''<remarks/>
        QT_MRK

        '''<remarks/>
        QT_REF

        '''<remarks/>
        QT_EXT_PR

        '''<remarks/>
        QT_ADN

        '''<remarks/>
        QT_UNKNOWN

        '''<remarks/>
        QT_FAR_COM

        '''<remarks/>
        QT_USE

        '''<remarks/>
        QT_ITI_USE

        '''<remarks/>
        QT_OCF

        '''<remarks/>
        QT_ITI_SER

        '''<remarks/>
        QT_BND
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class FlagType

        Private typeField As String

        Private textField() As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Text() As String()
            Get
                Return Me.textField
            End Get
            Set
                Me.textField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Public Enum QuotationGenericElementTypePricingInfoState

        '''<remarks/>
        Original

        '''<remarks/>
        Current

        '''<remarks/>
        Disrupted
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2"),
        System.Xml.Serialization.XmlRootAttribute("QtItineraryPricingRecord", [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2", IsNullable:=False)>
    Partial Public Class QtItineraryPricingRecordType

        Private priceField() As QuotationGenericElementTypePrice

        Private taxField() As QuotationGenericElementTypeTax

        Private pointField() As PointType

        Private quotationTypeField As FullQuotationType_Type

        Private flagField() As FlagType

        Private pricingInfoField() As QtItineraryPricingRecordTypePricingInfo

        Private stateField As QuotationGenericElementTypePricingInfoState

        Private stateFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Price")>
        Public Property Price() As QuotationGenericElementTypePrice()
            Get
                Return Me.priceField
            End Get
            Set
                Me.priceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Tax")>
        Public Property Tax() As QuotationGenericElementTypeTax()
            Get
                Return Me.taxField
            End Get
            Set
                Me.taxField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Point")>
        Public Property Point() As PointType()
            Get
                Return Me.pointField
            End Get
            Set
                Me.pointField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property QuotationType() As FullQuotationType_Type
            Get
                Return Me.quotationTypeField
            End Get
            Set
                Me.quotationTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Flag")>
        Public Property Flag() As FlagType()
            Get
                Return Me.flagField
            End Get
            Set
                Me.flagField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PricingInfo")>
        Public Property PricingInfo() As QtItineraryPricingRecordTypePricingInfo()
            Get
                Return Me.pricingInfoField
            End Get
            Set
                Me.pricingInfoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property State() As QuotationGenericElementTypePricingInfoState
            Get
                Return Me.stateField
            End Get
            Set
                Me.stateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StateSpecified() As Boolean
            Get
                Return Me.stateFieldSpecified
            End Get
            Set
                Me.stateFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class QtItineraryPricingRecordTypePricingInfo

        Private numberField As Long

        Private numberFieldSpecified As Boolean

        Private indicatorField() As String

        Private itemsField() As Object

        Private originatorField As PointOfSaleType1

        Private passengerTypeCodeField() As PassengerTypeCodeType

        Private stateField As QtItineraryPricingRecordTypePricingInfoState

        Private stateFieldSpecified As Boolean

        '''<remarks/>
        Public Property Number() As Long
            Get
                Return Me.numberField
            End Get
            Set
                Me.numberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property NumberSpecified() As Boolean
            Get
                Return Me.numberFieldSpecified
            End Get
            Set
                Me.numberFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Indicator")>
        Public Property Indicator() As String()
            Get
                Return Me.indicatorField
            End Get
            Set
                Me.indicatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ProcessFlag", GetType(QtItineraryPricingRecordTypePricingInfoProcessFlag)),
            System.Xml.Serialization.XmlElementAttribute("UseCase", GetType(UseCaseType))>
        Public Property Items() As Object()
            Get
                Return Me.itemsField
            End Get
            Set
                Me.itemsField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Originator() As PointOfSaleType1
            Get
                Return Me.originatorField
            End Get
            Set
                Me.originatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PassengerTypeCode")>
        Public Property PassengerTypeCode() As PassengerTypeCodeType()
            Get
                Return Me.passengerTypeCodeField
            End Get
            Set
                Me.passengerTypeCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property State() As QtItineraryPricingRecordTypePricingInfoState
            Get
                Return Me.stateField
            End Get
            Set
                Me.stateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StateSpecified() As Boolean
            Get
                Return Me.stateFieldSpecified
            End Get
            Set
                Me.stateFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Public Enum QtItineraryPricingRecordTypePricingInfoProcessFlag

        '''<remarks/>
        ATU

        '''<remarks/>
        HTF

        '''<remarks/>
        MultiTicket
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class UseCaseType

        Private systemField As UseCaseTypeSystem

        Private nameField As String

        Private typesField() As String

        '''<remarks/>
        Public Property System() As UseCaseTypeSystem
            Get
                Return Me.systemField
            End Get
            Set
                Me.systemField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Name() As String
            Get
                Return Me.nameField
            End Get
            Set
                Me.nameField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Types() As String()
            Get
                Return Me.typesField
            End Get
            Set
                Me.typesField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class UseCaseTypeSystem

        Private ownerField As String

        Private modeField() As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Owner() As String
            Get
                Return Me.ownerField
            End Get
            Set
                Me.ownerField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Mode() As String()
            Get
                Return Me.modeField
            End Get
            Set
                Me.modeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(TypeName:="PointOfSaleType", [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class PointOfSaleType1

        Private officeField As PointOfSaleTypeOffice

        Private loginField As PointOfSaleTypeLogin

        Private actorField As PointOfSaleTypeActor

        '''<remarks/>
        Public Property Office() As PointOfSaleTypeOffice
            Get
                Return Me.officeField
            End Get
            Set
                Me.officeField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Login() As PointOfSaleTypeLogin
            Get
                Return Me.loginField
            End Get
            Set
                Me.loginField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Actor() As PointOfSaleTypeActor
            Get
                Return Me.actorField
            End Get
            Set
                Me.actorField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class PointOfSaleTypeOffice

        Private idField As String

        Private numericIdField As String

        Private cityField As String

        Private systemCodeField As String

        Private countryField As String

        Private agentTypeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property ID() As String
            Get
                Return Me.idField
            End Get
            Set
                Me.idField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property NumericId() As String
            Get
                Return Me.numericIdField
            End Get
            Set
                Me.numericIdField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property City() As String
            Get
                Return Me.cityField
            End Get
            Set
                Me.cityField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property SystemCode() As String
            Get
                Return Me.systemCodeField
            End Get
            Set
                Me.systemCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Country() As String
            Get
                Return Me.countryField
            End Get
            Set
                Me.countryField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AgentType() As String
            Get
                Return Me.agentTypeField
            End Get
            Set
                Me.agentTypeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class PointOfSaleTypeLogin

        Private channelField As PointOfSaleTypeLoginChannel

        Private signField As String

        Private dutyCodeField As String

        '''<remarks/>
        Public Property Channel() As PointOfSaleTypeLoginChannel
            Get
                Return Me.channelField
            End Get
            Set
                Me.channelField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Sign() As String
            Get
                Return Me.signField
            End Get
            Set
                Me.signField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DutyCode() As String
            Get
                Return Me.dutyCodeField
            End Get
            Set
                Me.dutyCodeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class PointOfSaleTypeLoginChannel

        Private accessTypeField As String

        Private productField As String

        Private subProductField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")>
        Public Property AccessType() As String
            Get
                Return Me.accessTypeField
            End Get
            Set
                Me.accessTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")>
        Public Property Product() As String
            Get
                Return Me.productField
            End Get
            Set
                Me.productField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="nonNegativeInteger")>
        Public Property SubProduct() As String
            Get
                Return Me.subProductField
            End Get
            Set
                Me.subProductField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Public Enum PointOfSaleTypeActor

        '''<remarks/>
        Creator

        '''<remarks/>
        Owner

        '''<remarks/>
        Updator
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class PassengerTypeCodeType

        Private typeField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Public Enum QtItineraryPricingRecordTypePricingInfoState

        '''<remarks/>
        Original

        '''<remarks/>
        Current

        '''<remarks/>
        Disrupted
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2"),
        System.Xml.Serialization.XmlRootAttribute("QtRefund", [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2", IsNullable:=False)>
    Partial Public Class QtRefundType

        Private priceField() As QuotationGenericElementTypePrice

        Private taxField() As QuotationGenericElementTypeTax

        Private pointField() As PointType

        Private quotationTypeField As FullQuotationType_Type

        Private flagField() As FlagType

        Private pricingInfoField() As QtRefundTypePricingInfo

        Private stateField As QuotationGenericElementTypePricingInfoState

        Private stateFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Price")>
        Public Property Price() As QuotationGenericElementTypePrice()
            Get
                Return Me.priceField
            End Get
            Set
                Me.priceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Tax")>
        Public Property Tax() As QuotationGenericElementTypeTax()
            Get
                Return Me.taxField
            End Get
            Set
                Me.taxField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Point")>
        Public Property Point() As PointType()
            Get
                Return Me.pointField
            End Get
            Set
                Me.pointField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property QuotationType() As FullQuotationType_Type
            Get
                Return Me.quotationTypeField
            End Get
            Set
                Me.quotationTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Flag")>
        Public Property Flag() As FlagType()
            Get
                Return Me.flagField
            End Get
            Set
                Me.flagField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PricingInfo")>
        Public Property PricingInfo() As QtRefundTypePricingInfo()
            Get
                Return Me.pricingInfoField
            End Get
            Set
                Me.pricingInfoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property State() As QuotationGenericElementTypePricingInfoState
            Get
                Return Me.stateField
            End Get
            Set
                Me.stateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StateSpecified() As Boolean
            Get
                Return Me.stateFieldSpecified
            End Get
            Set
                Me.stateFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class QtRefundTypePricingInfo

        Private numberField As Long

        Private numberFieldSpecified As Boolean

        Private refundedItineraryField As String

        Private waiverCodeField As String

        Private dataSourceIdentifierField As String

        Private cancellationFeeComField As QtRefundTypePricingInfoCancellationFeeCom

        Private pricingCodeField As String

        Private settlementAuthorizationCodeField() As String

        Private stateField As QuotationGenericElementTypePricingInfoState

        Private stateFieldSpecified As Boolean

        '''<remarks/>
        Public Property Number() As Long
            Get
                Return Me.numberField
            End Get
            Set
                Me.numberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property NumberSpecified() As Boolean
            Get
                Return Me.numberFieldSpecified
            End Get
            Set
                Me.numberFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        Public Property RefundedItinerary() As String
            Get
                Return Me.refundedItineraryField
            End Get
            Set
                Me.refundedItineraryField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property WaiverCode() As String
            Get
                Return Me.waiverCodeField
            End Get
            Set
                Me.waiverCodeField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property DataSourceIdentifier() As String
            Get
                Return Me.dataSourceIdentifierField
            End Get
            Set
                Me.dataSourceIdentifierField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property CancellationFeeCom() As QtRefundTypePricingInfoCancellationFeeCom
            Get
                Return Me.cancellationFeeComField
            End Get
            Set
                Me.cancellationFeeComField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property PricingCode() As String
            Get
                Return Me.pricingCodeField
            End Get
            Set
                Me.pricingCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SettlementAuthorizationCode")>
        Public Property SettlementAuthorizationCode() As String()
            Get
                Return Me.settlementAuthorizationCodeField
            End Get
            Set
                Me.settlementAuthorizationCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property State() As QuotationGenericElementTypePricingInfoState
            Get
                Return Me.stateField
            End Get
            Set
                Me.stateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StateSpecified() As Boolean
            Get
                Return Me.stateFieldSpecified
            End Get
            Set
                Me.stateFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class QtRefundTypePricingInfoCancellationFeeCom

        Private currencyCodeField As String

        Private decimalPlacesField As String

        Private amountField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set
                Me.currencyCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DecimalPlaces() As String
            Get
                Return Me.decimalPlacesField
            End Get
            Set
                Me.decimalPlacesField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Amount() As String
            Get
                Return Me.amountField
            End Get
            Set
                Me.amountField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2"),
        System.Xml.Serialization.XmlRootAttribute("QtCouponItinerary", [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2", IsNullable:=False)>
    Partial Public Class QtCouponItineraryType

        Private priceField() As QuotationGenericElementTypePrice

        Private taxField() As QuotationGenericElementTypeTax

        Private pointField() As PointType

        Private quotationTypeField As FullQuotationType_Type

        Private flagField() As FlagType

        Private pricingInfoField() As QtCouponItineraryTypePricingInfo

        Private productInfoField As QtCouponItineraryTypeProductInfo

        Private ticketingInfoField() As QtCouponItineraryTypeTicketingInfo

        Private stateField As QuotationGenericElementTypePricingInfoState

        Private stateFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Price")>
        Public Property Price() As QuotationGenericElementTypePrice()
            Get
                Return Me.priceField
            End Get
            Set
                Me.priceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Tax")>
        Public Property Tax() As QuotationGenericElementTypeTax()
            Get
                Return Me.taxField
            End Get
            Set
                Me.taxField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Point")>
        Public Property Point() As PointType()
            Get
                Return Me.pointField
            End Get
            Set
                Me.pointField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property QuotationType() As FullQuotationType_Type
            Get
                Return Me.quotationTypeField
            End Get
            Set
                Me.quotationTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Flag")>
        Public Property Flag() As FlagType()
            Get
                Return Me.flagField
            End Get
            Set
                Me.flagField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PricingInfo")>
        Public Property PricingInfo() As QtCouponItineraryTypePricingInfo()
            Get
                Return Me.pricingInfoField
            End Get
            Set
                Me.pricingInfoField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property ProductInfo() As QtCouponItineraryTypeProductInfo
            Get
                Return Me.productInfoField
            End Get
            Set
                Me.productInfoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("TicketingInfo")>
        Public Property TicketingInfo() As QtCouponItineraryTypeTicketingInfo()
            Get
                Return Me.ticketingInfoField
            End Get
            Set
                Me.ticketingInfoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property State() As QuotationGenericElementTypePricingInfoState
            Get
                Return Me.stateField
            End Get
            Set
                Me.stateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StateSpecified() As Boolean
            Get
                Return Me.stateFieldSpecified
            End Get
            Set
                Me.stateFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class QtCouponItineraryTypePricingInfo

        Private numberField As Long

        Private numberFieldSpecified As Boolean

        Private pricedPTCField As String

        Private fareBasisField As FareBasisType

        Private classOfServiceField As String

        Private baggageAllowanceField As BaggageAllowanceType

        Private stateField As QuotationGenericElementTypePricingInfoState

        Private stateFieldSpecified As Boolean

        '''<remarks/>
        Public Property Number() As Long
            Get
                Return Me.numberField
            End Get
            Set
                Me.numberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property NumberSpecified() As Boolean
            Get
                Return Me.numberFieldSpecified
            End Get
            Set
                Me.numberFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        Public Property PricedPTC() As String
            Get
                Return Me.pricedPTCField
            End Get
            Set
                Me.pricedPTCField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property FareBasis() As FareBasisType
            Get
                Return Me.fareBasisField
            End Get
            Set
                Me.fareBasisField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property ClassOfService() As String
            Get
                Return Me.classOfServiceField
            End Get
            Set
                Me.classOfServiceField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property BaggageAllowance() As BaggageAllowanceType
            Get
                Return Me.baggageAllowanceField
            End Get
            Set
                Me.baggageAllowanceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property State() As QuotationGenericElementTypePricingInfoState
            Get
                Return Me.stateField
            End Get
            Set
                Me.stateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StateSpecified() As Boolean
            Get
                Return Me.stateFieldSpecified
            End Get
            Set
                Me.stateFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class FareBasisType

        Private ticketDesignatorField As String

        Private fareBasisCodeField As String

        Private primaryCodeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property TicketDesignator() As String
            Get
                Return Me.ticketDesignatorField
            End Get
            Set
                Me.ticketDesignatorField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property FareBasisCode() As String
            Get
                Return Me.fareBasisCodeField
            End Get
            Set
                Me.fareBasisCodeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property PrimaryCode() As String
            Get
                Return Me.primaryCodeField
            End Get
            Set
                Me.primaryCodeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class BaggageAllowanceType

        Private allowanceTypeField As String

        Private measureUnitField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property AllowanceType() As String
            Get
                Return Me.allowanceTypeField
            End Get
            Set
                Me.allowanceTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property MeasureUnit() As String
            Get
                Return Me.measureUnitField
            End Get
            Set
                Me.measureUnitField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class QtCouponItineraryTypeProductInfo

        Private providerField As QtCouponItineraryTypeProductInfoProvider

        Private startField As DateTimeAndLocationType1

        Private endField As DateTimeAndLocationType1

        Private stateField As QuotationGenericElementTypePricingInfoState

        Private stateFieldSpecified As Boolean

        '''<remarks/>
        Public Property Provider() As QtCouponItineraryTypeProductInfoProvider
            Get
                Return Me.providerField
            End Get
            Set
                Me.providerField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property Start() As DateTimeAndLocationType1
            Get
                Return Me.startField
            End Get
            Set
                Me.startField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property [End]() As DateTimeAndLocationType1
            Get
                Return Me.endField
            End Get
            Set
                Me.endField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property State() As QuotationGenericElementTypePricingInfoState
            Get
                Return Me.stateField
            End Get
            Set
                Me.stateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StateSpecified() As Boolean
            Get
                Return Me.stateFieldSpecified
            End Get
            Set
                Me.stateFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class QtCouponItineraryTypeProductInfoProvider

        Private codeField As String

        Private contextField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Context() As String
            Get
                Return Me.contextField
            End Get
            Set
                Me.contextField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(TypeName:="DateTimeAndLocationType", [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class DateTimeAndLocationType1

        Private locationField As DateTimeAndLocationTypeLocation

        Private dateTimeField As String

        '''<remarks/>
        Public Property Location() As DateTimeAndLocationTypeLocation
            Get
                Return Me.locationField
            End Get
            Set
                Me.locationField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property DateTime() As String
            Get
                Return Me.dateTimeField
            End Get
            Set
                Me.dateTimeField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class DateTimeAndLocationTypeLocation

        Private codeField As String

        Private contextField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property Context() As String
            Get
                Return Me.contextField
            End Get
            Set
                Me.contextField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class QtCouponItineraryTypeTicketingInfo

        Private flightReferenceField() As DisruptionFlightsType

        Private stateField As QuotationGenericElementTypePricingInfoState

        Private stateFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("FlightReference")>
        Public Property FlightReference() As DisruptionFlightsType()
            Get
                Return Me.flightReferenceField
            End Get
            Set
                Me.flightReferenceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property State() As QuotationGenericElementTypePricingInfoState
            Get
                Return Me.stateField
            End Get
            Set
                Me.stateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StateSpecified() As Boolean
            Get
                Return Me.stateFieldSpecified
            End Get
            Set
                Me.stateFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class DisruptionFlightsType

        Private orderField As String

        Private ticketNumberField As String

        Private couponNumberField As String

        Private disruptionTagField As String

        '''<remarks/>
        Public Property Order() As String
            Get
                Return Me.orderField
            End Get
            Set
                Me.orderField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property TicketNumber() As String
            Get
                Return Me.ticketNumberField
            End Get
            Set
                Me.ticketNumberField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property CouponNumber() As String
            Get
                Return Me.couponNumberField
            End Get
            Set
                Me.couponNumberField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property DisruptionTag() As String
            Get
                Return Me.disruptionTagField
            End Get
            Set
                Me.disruptionTagField = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2"),
        System.Xml.Serialization.XmlRootAttribute("QtItineraryService", [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2", IsNullable:=False)>
    Partial Public Class QtItineraryServiceType

        Private priceField() As QuotationGenericElementTypePrice

        Private taxField() As QuotationGenericElementTypeTax

        Private pointField() As PointType

        Private quotationTypeField As FullQuotationType_Type

        Private flagField() As FlagType

        Private pricingInfoField() As QtItineraryServiceTypePricingInfo

        Private stateField As QuotationGenericElementTypePricingInfoState

        Private stateFieldSpecified As Boolean

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Price")>
        Public Property Price() As QuotationGenericElementTypePrice()
            Get
                Return Me.priceField
            End Get
            Set
                Me.priceField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Tax")>
        Public Property Tax() As QuotationGenericElementTypeTax()
            Get
                Return Me.taxField
            End Get
            Set
                Me.taxField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Point")>
        Public Property Point() As PointType()
            Get
                Return Me.pointField
            End Get
            Set
                Me.pointField = Value
            End Set
        End Property

        '''<remarks/>
        Public Property QuotationType() As FullQuotationType_Type
            Get
                Return Me.quotationTypeField
            End Get
            Set
                Me.quotationTypeField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Flag")>
        Public Property Flag() As FlagType()
            Get
                Return Me.flagField
            End Get
            Set
                Me.flagField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("PricingInfo")>
        Public Property PricingInfo() As QtItineraryServiceTypePricingInfo()
            Get
                Return Me.pricingInfoField
            End Get
            Set
                Me.pricingInfoField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property State() As QuotationGenericElementTypePricingInfoState
            Get
                Return Me.stateField
            End Get
            Set
                Me.stateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StateSpecified() As Boolean
            Get
                Return Me.stateFieldSpecified
            End Get
            Set
                Me.stateFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://xml.amadeus.com/2010/06/QuotationTypes_v2")>
    Partial Public Class QtItineraryServiceTypePricingInfo

        Private numberField As Long

        Private numberFieldSpecified As Boolean

        Private stateField As QuotationGenericElementTypePricingInfoState

        Private stateFieldSpecified As Boolean

        '''<remarks/>
        Public Property Number() As Long
            Get
                Return Me.numberField
            End Get
            Set
                Me.numberField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property NumberSpecified() As Boolean
            Get
                Return Me.numberFieldSpecified
            End Get
            Set
                Me.numberFieldSpecified = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>
        Public Property State() As QuotationGenericElementTypePricingInfoState
            Get
                Return Me.stateField
            End Get
            Set
                Me.stateField = Value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property StateSpecified() As Boolean
            Get
                Return Me.stateFieldSpecified
            End Get
            Set
                Me.stateFieldSpecified = Value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class GenericWarningsType
    
    
        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Warning", IsNullable:=false)>  _
        Public Property Warnings As ErrorType()
    
        '''<remarks/>
        <System.Xml.Serialization.XmlArrayAttribute("Warnings", [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A"),  _
            System.Xml.Serialization.XmlArrayItemAttribute("Warning", IsNullable:=false)>  _
        Public Property Warnings1 As WarningType1()
    
        '''<remarks/>
        <System.Xml.Serialization.XmlArrayAttribute("Warnings", [Namespace]:="http://www.iata.org/IATA/2007/00/IATA2010.1"),  _
            System.Xml.Serialization.XmlArrayItemAttribute("Warning", IsNullable:=false)>  _
        Public Property Warnings2 As WarningType2()
            
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class GenericErrorsType

        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Error", IsNullable:=false)>  _
        Public Property Errors As ErrorType()
    
        '''<remarks/>
        <System.Xml.Serialization.XmlArrayAttribute("Errors", [Namespace]:="http://www.opentravel.org/OTA/2003/05/OTA2011A"),  _
            System.Xml.Serialization.XmlArrayItemAttribute("Error", IsNullable:=false)>  _
        Public Property Errors1 As ErrorType1()
    
        '''<remarks/>
        <System.Xml.Serialization.XmlArrayAttribute("Errors", [Namespace]:="http://www.iata.org/IATA/2007/00/IATA2010.1"),  _
            System.Xml.Serialization.XmlArrayItemAttribute("Error", IsNullable:=false)>  _
        Public Property Errors2 As ErrorType2()
    End Class


    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class NotificationType
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("To")>  _
        Public Property [To] As NotificationTypeTO()
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Comment")>  _
        Public Property Comment As NotificationTypeComment()
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Type As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")>  _
        Public Property Language As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class NotificationTypeTO
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Type As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class NotificationTypeComment

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Type As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class CardType
    
        Private addressVerificationSystemValueField As AVS_Type
    
        Private magneticTrackField() As CardTypeMagneticTrack
    
        Private issuanceField As CardIssuanceInformationType
    
        Private primaryAccountNumberField As CardTypePrimaryAccountNumber
    
        Private cVVField As CardTypeCVV
    
        Private validityField As CardTypeValidity
    
        Private vendorField As CardTypeVendor
    
        Private programField As CardTypeProgram
    
        Private holderNameField As String
    
        Private subTypeField As String
    
        Private processField As String
    
        Private accountTypeField As CardTypeAccountType
    
        Private accountTypeFieldSpecified As Boolean
    
        Private tierLevelField As String
    
        '''<remarks/>
        Public Property AddressVerificationSystemValue() As AVS_Type
            Get
                Return Me.addressVerificationSystemValueField
            End Get
            Set
                Me.addressVerificationSystemValueField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("MagneticTrack")>  _
        Public Property MagneticTrack() As CardTypeMagneticTrack()
            Get
                Return Me.magneticTrackField
            End Get
            Set
                Me.magneticTrackField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property Issuance() As CardIssuanceInformationType
            Get
                Return Me.issuanceField
            End Get
            Set
                Me.issuanceField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property PrimaryAccountNumber() As CardTypePrimaryAccountNumber
            Get
                Return Me.primaryAccountNumberField
            End Get
            Set
                Me.primaryAccountNumberField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property CVV() As CardTypeCVV
            Get
                Return Me.cVVField
            End Get
            Set
                Me.cVVField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property Validity() As CardTypeValidity
            Get
                Return Me.validityField
            End Get
            Set
                Me.validityField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property Vendor() As CardTypeVendor
            Get
                Return Me.vendorField
            End Get
            Set
                Me.vendorField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property Program() As CardTypeProgram
            Get
                Return Me.programField
            End Get
            Set
                Me.programField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property HolderName() As String
            Get
                Return Me.holderNameField
            End Get
            Set
                Me.holderNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property SubType() As String
            Get
                Return Me.subTypeField
            End Get
            Set
                Me.subTypeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Process() As String
            Get
                Return Me.processField
            End Get
            Set
                Me.processField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property AccountType() As CardTypeAccountType
            Get
                Return Me.accountTypeField
            End Get
            Set
                Me.accountTypeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property AccountTypeSpecified() As Boolean
            Get
                Return Me.accountTypeFieldSpecified
            End Get
            Set
                Me.accountTypeFieldSpecified = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property TierLevel() As String
            Get
                Return Me.tierLevelField
            End Get
            Set
                Me.tierLevelField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class AVS_Type
    
        Private lineField() As String
    
        Private cityNameField As String
    
        Private postalCodeField As String
    
        Private countryField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Line")>  _
        Public Property Line() As String()
            Get
                Return Me.lineField
            End Get
            Set
                Me.lineField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property CityName() As String
            Get
                Return Me.cityNameField
            End Get
            Set
                Me.cityNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property PostalCode() As String
            Get
                Return Me.postalCodeField
            End Get
            Set
                Me.postalCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Country() As String
            Get
                Return Me.countryField
            End Get
            Set
                Me.countryField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/FOP_Types_v6")>  _
    Public Enum CardTypeMagneticTrackType
    
        '''<remarks/>
        Raw
    
        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("1")>  _
        Item1
    
        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("2")>  _
        Item2
    
        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("3")>  _
        Item3
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class CardTypeMagneticTrack
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Type As CardTypeMagneticTrackType
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property TypeSpecified As Boolean
            
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String
            
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class CardIssuanceInformationTypeBank
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Name As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property ID As String
            
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class CardIssuanceInformationType
    
        '''<remarks/>
        Public Property Bank As CardIssuanceInformationTypeBank
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Country As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property CountryCode As String
            
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class CardTypePrimaryAccountNumber
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property KnoxID As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String
            
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class CardTypeCVV
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property KnoxID As String

        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String
            
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class CardTypeValidity
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property StartDate As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property EndDate As String
            
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class CardTypeVendor
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Code As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class CardTypeProgram
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Code As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String
            
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute()>  _
    Public Enum CardTypeAccountType
    
        '''<remarks/>
        Corporate
    
        '''<remarks/>
        Personal
    End Enum


    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class FundsTransferDetailsType
        Inherits FundsTransferType
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Status As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Reference As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property UserID As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property OfficeID As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property CreationDate As String

    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(FundsTransferDetailsType)),  _
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class FundsTransferType
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Value")>  _
        Public Property Value As VirtualCreditCard.VirtualCardAmountType()
            
        '''<remarks/>
        Public Property Scheduling As FundsTransferTypeScheduling
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Action As String
            
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class FundsTransferTypeScheduling
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property [Date] As String
            
    End Class


    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class TTR_ActorType
        Inherits PassengerNameType
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("nationality")>  _
        Public Property nationality As TTR_ActorTypeNationality()
            
        '''<remarks/>
        Public Property contact As TTR_ActorTypeContact
            
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("address")>  _
        Public Property address As TTR_ActorTypeAddress()
            
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("event")>  _
        Public Property [event] As TTR_ActorTypeEvent()
            
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("loyalty")>  _
        Public Property loyalty As TTR_ActorTypeLoyalty()
            
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("docRef")>  _
        Public Property docRef As TTR_ActorTypeDocRef()
            
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")>  _
        Public Property externalID As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKENS")>  _
        Public Property roles As String
            
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class TTR_ActorTypeNationality
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property code As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String
            
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class TTR_ActorTypeContact
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("phone")>  _
        Public Property phone As TTR_ActorTypeContactPhone()
            
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("email")>  _
        Public Property email As TTR_ActorTypeContactEmail()

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class TTR_ActorTypeContactPhone
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property label As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property purpose As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property overseasCode As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property countryCode As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="normalizedString")>  _
        Public Property addresseeName As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class TTR_ActorTypeContactEmail
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property label As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property purpose As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="normalizedString")>  _
        Public Property addresseeName As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String

    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class TTR_ActorTypeAddress
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property label As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property line As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property complement As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property zip As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property countryCode As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property latitude As String
        
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property longitude As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cityCode As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cityName As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property countryName As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property stateCode As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property stateName As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property postalBox As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="normalizedString")>  _
        Public Property companyName As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="normalizedString")>  _
        Public Property addresseeName As String
            
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class TTR_ActorTypeEvent
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property description As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property [date] As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class TTR_ActorTypeLoyalty

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property companyCode As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cardNumber As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property tierLevel As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property alliance As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class TTR_ActorTypeDocRef
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property type As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property description As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property issuer As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property issuanceDate As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property expirationDate As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String
    End Class


    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ETR_TransferType
    
        Private serviceProviderField As RailProviderType
    
        Private identifierField As String
    
        Private startField As ETR_TransferTypeStart
    
        Private endField As ETR_TransferTypeEnd
    
        Private vehicleField As ETR_TransferTypeVehicle
    
        Private mileageField As MileageType
    
        Private baggageField As ETR_TransferTypeBaggage
    
        Private checkInField As ETR_TransferTypeCheckIn
    
        Private ticketField As ETR_TransferTypeTicket
    
        Private creationField As CreationType
    
        Private modificationField As ModificationType
    
        Private confirmationField As ConfirmationType
    
        Private descriptionsField() As ETR_TransferTypeDescriptions
    
        Private bkgChannelField As BookingChannelType
    
        Private creationChannelField As FullOriginSystemType
    
        Private externalSystemField As ExternalSystemType
    
        Private creatorField As SystemInformation
    
        Private propertiesField As String
    
        Private descriptionField As String
    
        Private bkgClassField As String
    
        Private statusField As String
    
        Private nIPField As String
    
        Private confirmNbrField As String
    
        Private cancelNbrField As String
    
        Private overrideStatusField As String
    
        Private mBOProductCodeField As String
    
        '''<remarks/>
        Public Property serviceProvider() As RailProviderType
            Get
                Return Me.serviceProviderField
            End Get
            Set
                Me.serviceProviderField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property identifier() As String
            Get
                Return Me.identifierField
            End Get
            Set
                Me.identifierField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property start() As ETR_TransferTypeStart
            Get
                Return Me.startField
            End Get
            Set
                Me.startField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property [end]() As ETR_TransferTypeEnd
            Get
                Return Me.endField
            End Get
            Set
                Me.endField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property vehicle() As ETR_TransferTypeVehicle
            Get
                Return Me.vehicleField
            End Get
            Set
                Me.vehicleField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property mileage() As MileageType
            Get
                Return Me.mileageField
            End Get
            Set
                Me.mileageField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property baggage() As ETR_TransferTypeBaggage
            Get
                Return Me.baggageField
            End Get
            Set
                Me.baggageField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property checkIn() As ETR_TransferTypeCheckIn
            Get
                Return Me.checkInField
            End Get
            Set
                Me.checkInField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property ticket() As ETR_TransferTypeTicket
            Get
                Return Me.ticketField
            End Get
            Set
                Me.ticketField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property creation() As CreationType
            Get
                Return Me.creationField
            End Get
            Set
                Me.creationField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property modification() As ModificationType
            Get
                Return Me.modificationField
            End Get
            Set
                Me.modificationField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property confirmation() As ConfirmationType
            Get
                Return Me.confirmationField
            End Get
            Set
                Me.confirmationField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("descriptions")>  _
        Public Property descriptions() As ETR_TransferTypeDescriptions()
            Get
                Return Me.descriptionsField
            End Get
            Set
                Me.descriptionsField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property bkgChannel() As BookingChannelType
            Get
                Return Me.bkgChannelField
            End Get
            Set
                Me.bkgChannelField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property creationChannel() As FullOriginSystemType
            Get
                Return Me.creationChannelField
            End Get
            Set
                Me.creationChannelField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property externalSystem() As ExternalSystemType
            Get
                Return Me.externalSystemField
            End Get
            Set
                Me.externalSystemField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property creator() As SystemInformation
            Get
                Return Me.creatorField
            End Get
            Set
                Me.creatorField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property properties() As String
            Get
                Return Me.propertiesField
            End Get
            Set
                Me.propertiesField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property bkgClass() As String
            Get
                Return Me.bkgClassField
            End Get
            Set
                Me.bkgClassField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property status() As String
            Get
                Return Me.statusField
            End Get
            Set
                Me.statusField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property NIP() As String
            Get
                Return Me.nIPField
            End Get
            Set
                Me.nIPField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property confirmNbr() As String
            Get
                Return Me.confirmNbrField
            End Get
            Set
                Me.confirmNbrField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cancelNbr() As String
            Get
                Return Me.cancelNbrField
            End Get
            Set
                Me.cancelNbrField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property overrideStatus() As String
            Get
                Return Me.overrideStatusField
            End Get
            Set
                Me.overrideStatusField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property MBOProductCode() As String
            Get
                Return Me.mBOProductCodeField
            End Get
            Set
                Me.mBOProductCodeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ETR_TransferTypeStart
    
        Private locationCodeField As String
    
        Private addressField As ETR_TransferTypeStartAddress
    
        Private dateTimeField As String
    
        Private locationNameField As String
    
        Private terminalField As String
    
        '''<remarks/>
        Public Property locationCode() As String
            Get
                Return Me.locationCodeField
            End Get
            Set
                Me.locationCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property address() As ETR_TransferTypeStartAddress
            Get
                Return Me.addressField
            End Get
            Set
                Me.addressField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property dateTime() As String
            Get
                Return Me.dateTimeField
            End Get
            Set
                Me.dateTimeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property locationName() As String
            Get
                Return Me.locationNameField
            End Get
            Set
                Me.locationNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property terminal() As String
            Get
                Return Me.terminalField
            End Get
            Set
                Me.terminalField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ETR_TransferTypeStartAddress
    
        Private lineField As String
    
        Private complementField As String
    
        Private zipField As String
    
        Private countryCodeField As String
    
        Private latitudeField As String
    
        Private longitudeField As String
    
        Private cityCodeField As String
    
        Private cityNameField As String
    
        Private countryNameField As String
    
        Private stateCodeField As String
    
        Private stateNameField As String
    
        Private postalBoxField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property line() As String
            Get
                Return Me.lineField
            End Get
            Set
                Me.lineField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property complement() As String
            Get
                Return Me.complementField
            End Get
            Set
                Me.complementField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property zip() As String
            Get
                Return Me.zipField
            End Get
            Set
                Me.zipField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property countryCode() As String
            Get
                Return Me.countryCodeField
            End Get
            Set
                Me.countryCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property latitude() As String
            Get
                Return Me.latitudeField
            End Get
            Set
                Me.latitudeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property longitude() As String
            Get
                Return Me.longitudeField
            End Get
            Set
                Me.longitudeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cityCode() As String
            Get
                Return Me.cityCodeField
            End Get
            Set
                Me.cityCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cityName() As String
            Get
                Return Me.cityNameField
            End Get
            Set
                Me.cityNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property countryName() As String
            Get
                Return Me.countryNameField
            End Get
            Set
                Me.countryNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property stateCode() As String
            Get
                Return Me.stateCodeField
            End Get
            Set
                Me.stateCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property stateName() As String
            Get
                Return Me.stateNameField
            End Get
            Set
                Me.stateNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property postalBox() As String
            Get
                Return Me.postalBoxField
            End Get
            Set
                Me.postalBoxField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ETR_TransferTypeEnd
    
        Private locationCodeField As String
    
        Private addressField As ETR_TransferTypeEndAddress
    
        Private dateTimeField As String
    
        Private locationNameField As String
    
        Private terminalField As String
    
        '''<remarks/>
        Public Property locationCode() As String
            Get
                Return Me.locationCodeField
            End Get
            Set
                Me.locationCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property address() As ETR_TransferTypeEndAddress
            Get
                Return Me.addressField
            End Get
            Set
                Me.addressField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property dateTime() As String
            Get
                Return Me.dateTimeField
            End Get
            Set
                Me.dateTimeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property locationName() As String
            Get
                Return Me.locationNameField
            End Get
            Set
                Me.locationNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property terminal() As String
            Get
                Return Me.terminalField
            End Get
            Set
                Me.terminalField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ETR_TransferTypeEndAddress
    
        Private lineField As String
    
        Private complementField As String
    
        Private zipField As String
    
        Private countryCodeField As String
    
        Private latitudeField As String
    
        Private longitudeField As String
    
        Private cityCodeField As String
    
        Private cityNameField As String
    
        Private countryNameField As String
    
        Private stateCodeField As String
    
        Private stateNameField As String
    
        Private postalBoxField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property line() As String
            Get
                Return Me.lineField
            End Get
            Set
                Me.lineField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property complement() As String
            Get
                Return Me.complementField
            End Get
            Set
                Me.complementField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property zip() As String
            Get
                Return Me.zipField
            End Get
            Set
                Me.zipField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property countryCode() As String
            Get
                Return Me.countryCodeField
            End Get
            Set
                Me.countryCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property latitude() As String
            Get
                Return Me.latitudeField
            End Get
            Set
                Me.latitudeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property longitude() As String
            Get
                Return Me.longitudeField
            End Get
            Set
                Me.longitudeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cityCode() As String
            Get
                Return Me.cityCodeField
            End Get
            Set
                Me.cityCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cityName() As String
            Get
                Return Me.cityNameField
            End Get
            Set
                Me.cityNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property countryName() As String
            Get
                Return Me.countryNameField
            End Get
            Set
                Me.countryNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property stateCode() As String
            Get
                Return Me.stateCodeField
            End Get
            Set
                Me.stateCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property stateName() As String
            Get
                Return Me.stateNameField
            End Get
            Set
                Me.stateNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property postalBox() As String
            Get
                Return Me.postalBoxField
            End Get
            Set
                Me.postalBoxField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ETR_TransferTypeVehicle
    
        Private codeField As String
    
        Private descriptionField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ETR_TransferTypeBaggage
    
        Private quantityField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property quantity() As String
            Get
                Return Me.quantityField
            End Get
            Set
                Me.quantityField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ETR_TransferTypeCheckIn
    
        Private endDateTimeField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property endDateTime() As String
            Get
                Return Me.endDateTimeField
            End Get
            Set
                Me.endDateTimeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ETR_TransferTypeTicket
    
        Private numberField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property number() As String
            Get
                Return Me.numberField
            End Get
            Set
                Me.numberField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(TTR_ActorType)),  _
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class PassengerNameType
    
        Private nameField() As PassengerNameTypeName
    
        Private pTCField As String
    
        Private staffTypeField As String
    
        Private dateOfBirthField As String
    
        Private ageField As String
    
        Private specialSeatField As String
    
        Private identificationCodeField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Name")>  _
        Public Property Name() As PassengerNameTypeName()
            Get
                Return Me.nameField
            End Get
            Set
                Me.nameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property PTC() As String
            Get
                Return Me.pTCField
            End Get
            Set
                Me.pTCField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property StaffType() As String
            Get
                Return Me.staffTypeField
            End Get
            Set
                Me.staffTypeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property DateOfBirth() As String
            Get
                Return Me.dateOfBirthField
            End Get
            Set
                Me.dateOfBirthField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Age() As String
            Get
                Return Me.ageField
            End Get
            Set
                Me.ageField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property SpecialSeat() As String
            Get
                Return Me.specialSeatField
            End Get
            Set
                Me.specialSeatField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property IdentificationCode() As String
            Get
                Return Me.identificationCodeField
            End Get
            Set
                Me.identificationCodeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class PassengerNameTypeName
    
        Private typeField As String
    
        Private isRefField As String
    
        Private lastNameField As String
    
        Private firstNameField As String
    
        Private titleField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property IsRef() As String
            Get
                Return Me.isRefField
            End Get
            Set
                Me.isRefField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property LastName() As String
            Get
                Return Me.lastNameField
            End Get
            Set
                Me.lastNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property FirstName() As String
            Get
                Return Me.firstNameField
            End Get
            Set
                Me.firstNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Title() As String
            Get
                Return Me.titleField
            End Get
            Set
                Me.titleField = value
            End Set
        End Property
    End Class
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ReservationTypeTransport
        Inherits ETR_TransferType
    
        Private typeField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ReservationTypeAccommodation
        Inherits ETR_SleepMiscellaneousType
    
        Private identifierField As String
    
        Private typeField As String
    
        Private bkgClassField As String
    
        '''<remarks/>
        Public Property identifier() As String
            Get
                Return Me.identifierField
            End Get
            Set
                Me.identifierField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property bkgClass() As String
            Get
                Return Me.bkgClassField
            End Get
            Set
                Me.bkgClassField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ReservationTypeActivity
        Inherits ETR_ShowAndEventType
    
        '''<remarks/>
        Public Property identifier As String
            
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property type As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ReportingInfoTypeAdditionalInfo
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property CodeContext As String
   
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Code As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value As String
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ReportingInfoType
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("AdditionalInfo")>  _
        Public Property AdditionalInfo As ReportingInfoTypeAdditionalInfo()
    End Class

    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class RailProviderType
    
        Private refField As String
    
        Private nameField As String
    
        Private codeField As String
    
        Private externalRefField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property ref() As String
            Get
                Return Me.refField
            End Get
            Set
                Me.refField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property name() As String
            Get
                Return Me.nameField
            End Get
            Set
                Me.nameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property externalRef() As String
            Get
                Return Me.externalRefField
            End Get
            Set
                Me.externalRefField = value
            End Set
        End Property
    End Class



    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ReservationType
    
        Private travelersField() As TTR_ActorType
    
        Private transportField() As ReservationTypeTransport
    
        Private accommodationField As ReservationTypeAccommodation
    
        Private activityField As ReservationTypeActivity
    
        Private idField As String
    
        Private externalIDField As String
    
        Private creationDateField As Date
    
        Private creationDateFieldSpecified As Boolean
    
        '''<remarks/>
        <System.Xml.Serialization.XmlArrayItemAttribute("Traveler", IsNullable:=false)>  _
        Public Property Travelers() As TTR_ActorType()
            Get
                Return Me.travelersField
            End Get
            Set
                Me.travelersField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Transport")>  _
        Public Property Transport() As ReservationTypeTransport()
            Get
                Return Me.transportField
            End Get
            Set
                Me.transportField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property Accommodation() As ReservationTypeAccommodation
            Get
                Return Me.accommodationField
            End Get
            Set
                Me.accommodationField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property Activity() As ReservationTypeActivity
            Get
                Return Me.activityField
            End Get
            Set
                Me.activityField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property ID() As String
            Get
                Return Me.idField
            End Get
            Set
                Me.idField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")>  _
        Public Property ExternalID() As String
            Get
                Return Me.externalIDField
            End Get
            Set
                Me.externalIDField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="date")>  _
        Public Property CreationDate() As Date
            Get
                Return Me.creationDateField
            End Get
            Set
                Me.creationDateField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property CreationDateSpecified() As Boolean
            Get
                Return Me.creationDateFieldSpecified
            End Get
            Set
                Me.creationDateFieldSpecified = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class MileageType
    
        Private distanceField As String
    
        Private unitField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property distance() As String
            Get
                Return Me.distanceField
            End Get
            Set
                Me.distanceField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property unit() As String
            Get
                Return Me.unitField
            End Get
            Set
                Me.unitField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class CreationType
    
        Private dateTimeField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property dateTime() As String
            Get
                Return Me.dateTimeField
            End Get
            Set
                Me.dateTimeField = value
            End Set
        End Property
    End Class

    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ModificationType
    
        Private dateTimeField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property dateTime() As String
            Get
                Return Me.dateTimeField
            End Get
            Set
                Me.dateTimeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ConfirmationType
    
        Private deadlineField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property deadline() As String
            Get
                Return Me.deadlineField
            End Get
            Set
                Me.deadlineField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ETR_TransferTypeDescriptions
        Inherits ProductDescriptionType
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ProductDescriptionType
    
        Private itemsField() As Object
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("iframe", GetType(ProductDescriptionTypeIframe)),  _
            System.Xml.Serialization.XmlElementAttribute("media", GetType(ProductDescriptionTypeMedia)),  _
            System.Xml.Serialization.XmlElementAttribute("text", GetType(ProductDescriptionTypeText))>  _
        Public Property Items() As Object()
            Get
                Return Me.itemsField
            End Get
            Set
                Me.itemsField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ProductDescriptionTypeIframe
    
        Private nameField As String
    
        Private srcField As String
    
        Private widthField As Long
    
        Private widthFieldSpecified As Boolean
    
        Private heightField As Long
    
        Private heightFieldSpecified As Boolean
    
        Private borderField As Long
    
        Private borderFieldSpecified As Boolean
    
        Private srcdocField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property name() As String
            Get
                Return Me.nameField
            End Get
            Set
                Me.nameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")>  _
        Public Property src() As String
            Get
                Return Me.srcField
            End Get
            Set
                Me.srcField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property width() As Long
            Get
                Return Me.widthField
            End Get
            Set
                Me.widthField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property widthSpecified() As Boolean
            Get
                Return Me.widthFieldSpecified
            End Get
            Set
                Me.widthFieldSpecified = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property height() As Long
            Get
                Return Me.heightField
            End Get
            Set
                Me.heightField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property heightSpecified() As Boolean
            Get
                Return Me.heightFieldSpecified
            End Get
            Set
                Me.heightFieldSpecified = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property border() As Long
            Get
                Return Me.borderField
            End Get
            Set
                Me.borderField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property borderSpecified() As Boolean
            Get
                Return Me.borderFieldSpecified
            End Get
            Set
                Me.borderFieldSpecified = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property srcdoc() As String
            Get
                Return Me.srcdocField
            End Get
            Set
                Me.srcdocField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ProductDescriptionTypeMedia
    
        Private nameField As String
    
        Private typeField As String
    
        Private encodingField As ProductDescriptionTypeMediaEncoding
    
        Private encodingFieldSpecified As Boolean
    
        Private sizeField As Long
    
        Private sizeFieldSpecified As Boolean
    
        Private srcField As String
    
        Private idField As String
    
        Private widthField As Long
    
        Private widthFieldSpecified As Boolean
    
        Private heightField As Long
    
        Private heightFieldSpecified As Boolean
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property name() As String
            Get
                Return Me.nameField
            End Get
            Set
                Me.nameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property encoding() As ProductDescriptionTypeMediaEncoding
            Get
                Return Me.encodingField
            End Get
            Set
                Me.encodingField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property encodingSpecified() As Boolean
            Get
                Return Me.encodingFieldSpecified
            End Get
            Set
                Me.encodingFieldSpecified = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property size() As Long
            Get
                Return Me.sizeField
            End Get
            Set
                Me.sizeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property sizeSpecified() As Boolean
            Get
                Return Me.sizeFieldSpecified
            End Get
            Set
                Me.sizeFieldSpecified = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")>  _
        Public Property src() As String
            Get
                Return Me.srcField
            End Get
            Set
                Me.srcField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property ID() As String
            Get
                Return Me.idField
            End Get
            Set
                Me.idField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property width() As Long
            Get
                Return Me.widthField
            End Get
            Set
                Me.widthField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property widthSpecified() As Boolean
            Get
                Return Me.widthFieldSpecified
            End Get
            Set
                Me.widthFieldSpecified = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property height() As Long
            Get
                Return Me.heightField
            End Get
            Set
                Me.heightField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property heightSpecified() As Boolean
            Get
                Return Me.heightFieldSpecified
            End Get
            Set
                Me.heightFieldSpecified = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute()>  _
    Public Enum ProductDescriptionTypeMediaEncoding
    
        '''<remarks/>
        TXT
    
        '''<remarks/>
        DOC
    
        '''<remarks/>
        GIF
    
        '''<remarks/>
        PNG
    
        '''<remarks/>
        PDF
    
        '''<remarks/>
        JPG
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class ProductDescriptionTypeText
    
        Private languageField As String
    
        Private typeField As String
    
        Private valueField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="language")>  _
        Public Property language() As String
            Get
                Return Me.languageField
            End Get
            Set
                Me.languageField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class BookingChannelType
    
        Private codeField As String
    
        Private descriptionField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/PNR_Types_v4")>  _
    Partial Public Class FullOriginSystemType
    
        Private typeField As String
    
        Private classField As String
    
        Private categoryField As String
    
        Private ownerField As String
    
        Private accessPointField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property type() As String
            Get
                Return Me.typeField
            End Get
            Set
                Me.typeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property [class]() As String
            Get
                Return Me.classField
            End Get
            Set
                Me.classField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property category() As String
            Get
                Return Me.categoryField
            End Get
            Set
                Me.categoryField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property owner() As String
            Get
                Return Me.ownerField
            End Get
            Set
                Me.ownerField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property accessPoint() As String
            Get
                Return Me.accessPointField
            End Get
            Set
                Me.accessPointField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ExternalSystemType
    
        Private creationField As ExternalSystemTypeCreation
    
        Private bkgReferenceField() As ExternalSystemTypeBkgReference
    
        '''<remarks/>
        Public Property creation() As ExternalSystemTypeCreation
            Get
                Return Me.creationField
            End Get
            Set
                Me.creationField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("bkgReference")>  _
        Public Property bkgReference() As ExternalSystemTypeBkgReference()
            Get
                Return Me.bkgReferenceField
            End Get
            Set
                Me.bkgReferenceField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ExternalSystemTypeCreation
    
        Private dateTimeField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property dateTime() As String
            Get
                Return Me.dateTimeField
            End Get
            Set
                Me.dateTimeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ExternalSystemTypeBkgReference
    
        Private ownerField As String
    
        Private numberField As String
    
        Private additionalInformationField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Owner() As String
            Get
                Return Me.ownerField
            End Get
            Set
                Me.ownerField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property Number() As String
            Get
                Return Me.numberField
            End Get
            Set
                Me.numberField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property additionalInformation() As String
            Get
                Return Me.additionalInformationField
            End Get
            Set
                Me.additionalInformationField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class SystemInformation
    
        Private codeField As String
    
        Private descriptionField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_ShowAndEventTypeTicket
    
        Private numberField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property number() As String
            Get
                Return Me.numberField
            End Get
            Set
                Me.numberField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_ShowAndEventTypeStart
    
        Private locationCodeField As String
    
        Private addressField As ETR_ShowAndEventTypeStartAddress
    
        Private contactField As ETR_ShowAndEventTypeStartContact
    
        Private dateTimeField As String
    
        '''<remarks/>
        Public Property locationCode() As String
            Get
                Return Me.locationCodeField
            End Get
            Set
                Me.locationCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property address() As ETR_ShowAndEventTypeStartAddress
            Get
                Return Me.addressField
            End Get
            Set
                Me.addressField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property contact() As ETR_ShowAndEventTypeStartContact
            Get
                Return Me.contactField
            End Get
            Set
                Me.contactField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property dateTime() As String
            Get
                Return Me.dateTimeField
            End Get
            Set
                Me.dateTimeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_ShowAndEventTypeStartAddress
    
        Private lineField As String
    
        Private complementField As String
    
        Private zipField As String
    
        Private countryCodeField As String
    
        Private latitudeField As String
    
        Private longitudeField As String
    
        Private cityCodeField As String
    
        Private cityNameField As String
    
        Private countryNameField As String
    
        Private stateCodeField As String
    
        Private stateNameField As String
    
        Private postalBoxField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property line() As String
            Get
                Return Me.lineField
            End Get
            Set
                Me.lineField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property complement() As String
            Get
                Return Me.complementField
            End Get
            Set
                Me.complementField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property zip() As String
            Get
                Return Me.zipField
            End Get
            Set
                Me.zipField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property countryCode() As String
            Get
                Return Me.countryCodeField
            End Get
            Set
                Me.countryCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property latitude() As String
            Get
                Return Me.latitudeField
            End Get
            Set
                Me.latitudeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property longitude() As String
            Get
                Return Me.longitudeField
            End Get
            Set
                Me.longitudeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cityCode() As String
            Get
                Return Me.cityCodeField
            End Get
            Set
                Me.cityCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cityName() As String
            Get
                Return Me.cityNameField
            End Get
            Set
                Me.cityNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property countryName() As String
            Get
                Return Me.countryNameField
            End Get
            Set
                Me.countryNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property stateCode() As String
            Get
                Return Me.stateCodeField
            End Get
            Set
                Me.stateCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property stateName() As String
            Get
                Return Me.stateNameField
            End Get
            Set
                Me.stateNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property postalBox() As String
            Get
                Return Me.postalBoxField
            End Get
            Set
                Me.postalBoxField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_ShowAndEventTypeStartContact
    
        Private phoneField As String
    
        Private faxField As String
    
        Private emailField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property phone() As String
            Get
                Return Me.phoneField
            End Get
            Set
                Me.phoneField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property fax() As String
            Get
                Return Me.faxField
            End Get
            Set
                Me.faxField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property email() As String
            Get
                Return Me.emailField
            End Get
            Set
                Me.emailField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_ShowAndEventTypeEnd
    
        Private dateTimeField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property dateTime() As String
            Get
                Return Me.dateTimeField
            End Get
            Set
                Me.dateTimeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_ShowAndEventTypeValidity
    
        Private startDateTimeField As String
    
        Private endDateTimeField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property startDateTime() As String
            Get
                Return Me.startDateTimeField
            End Get
            Set
                Me.startDateTimeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property endDateTime() As String
            Get
                Return Me.endDateTimeField
            End Get
            Set
                Me.endDateTimeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_ShowAndEventTypeRate
    
        Private descriptionField As String
    
        Private codeField As String
    
        Private inclusionsField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property inclusions() As String
            Get
                Return Me.inclusionsField
            End Get
            Set
                Me.inclusionsField = value
            End Set
        End Property
    End Class



    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_SleepMiscellaneousType
    
        Private serviceProviderField As ProviderType
    
        Private startField As ETR_SleepMiscellaneousTypeStart
    
        Private endField As ETR_SleepMiscellaneousTypeEnd
    
        Private checkInField As ETR_SleepMiscellaneousTypeCheckIn
    
        Private checkOutField As ETR_SleepMiscellaneousTypeCheckOut
    
        Private customersField As ETR_SleepMiscellaneousTypeCustomers
    
        Private guaranteeField As ETR_SleepMiscellaneousTypeGuarantee
    
        Private depositField As ETR_SleepMiscellaneousTypeDeposit
    
        Private creationField As CreationType
    
        Private modificationField As ModificationType
    
        Private confirmationField As ConfirmationType
    
        Private descriptionsField() As ETR_TransferTypeDescriptions
    
        Private bkgChannelField As BookingChannelType
    
        Private creationChannelField As FullOriginSystemType
    
        Private externalSystemField As ExternalSystemType
    
        Private creatorField As SystemInformation
    
        Private propertiesField As String
    
        Private descriptionField As String
    
        Private nameField As String
    
        Private statusField As String
    
        Private nIPField As String
    
        Private additionalServicesField As String
    
        Private roomRateDescriptionField As String
    
        Private cancelPoliciesField As String
    
        Private inclusionsField As String
    
        Private otherRulesField As String
    
        Private confirmNbrField As String
    
        Private cancelNbrField As String
    
        Private overrideStatusField As String
    
        Private mBOProductCodeField As String
    
        '''<remarks/>
        Public Property serviceProvider() As ProviderType
            Get
                Return Me.serviceProviderField
            End Get
            Set
                Me.serviceProviderField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property start() As ETR_SleepMiscellaneousTypeStart
            Get
                Return Me.startField
            End Get
            Set
                Me.startField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property [end]() As ETR_SleepMiscellaneousTypeEnd
            Get
                Return Me.endField
            End Get
            Set
                Me.endField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property checkIn() As ETR_SleepMiscellaneousTypeCheckIn
            Get
                Return Me.checkInField
            End Get
            Set
                Me.checkInField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property checkOut() As ETR_SleepMiscellaneousTypeCheckOut
            Get
                Return Me.checkOutField
            End Get
            Set
                Me.checkOutField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property customers() As ETR_SleepMiscellaneousTypeCustomers
            Get
                Return Me.customersField
            End Get
            Set
                Me.customersField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property guarantee() As ETR_SleepMiscellaneousTypeGuarantee
            Get
                Return Me.guaranteeField
            End Get
            Set
                Me.guaranteeField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property deposit() As ETR_SleepMiscellaneousTypeDeposit
            Get
                Return Me.depositField
            End Get
            Set
                Me.depositField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property creation() As CreationType
            Get
                Return Me.creationField
            End Get
            Set
                Me.creationField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property modification() As ModificationType
            Get
                Return Me.modificationField
            End Get
            Set
                Me.modificationField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property confirmation() As ConfirmationType
            Get
                Return Me.confirmationField
            End Get
            Set
                Me.confirmationField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("descriptions")>  _
        Public Property descriptions() As ETR_TransferTypeDescriptions()
            Get
                Return Me.descriptionsField
            End Get
            Set
                Me.descriptionsField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property bkgChannel() As BookingChannelType
            Get
                Return Me.bkgChannelField
            End Get
            Set
                Me.bkgChannelField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property creationChannel() As FullOriginSystemType
            Get
                Return Me.creationChannelField
            End Get
            Set
                Me.creationChannelField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property externalSystem() As ExternalSystemType
            Get
                Return Me.externalSystemField
            End Get
            Set
                Me.externalSystemField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property creator() As SystemInformation
            Get
                Return Me.creatorField
            End Get
            Set
                Me.creatorField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property properties() As String
            Get
                Return Me.propertiesField
            End Get
            Set
                Me.propertiesField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property name() As String
            Get
                Return Me.nameField
            End Get
            Set
                Me.nameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property status() As String
            Get
                Return Me.statusField
            End Get
            Set
                Me.statusField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property NIP() As String
            Get
                Return Me.nIPField
            End Get
            Set
                Me.nIPField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property additionalServices() As String
            Get
                Return Me.additionalServicesField
            End Get
            Set
                Me.additionalServicesField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property roomRateDescription() As String
            Get
                Return Me.roomRateDescriptionField
            End Get
            Set
                Me.roomRateDescriptionField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cancelPolicies() As String
            Get
                Return Me.cancelPoliciesField
            End Get
            Set
                Me.cancelPoliciesField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property inclusions() As String
            Get
                Return Me.inclusionsField
            End Get
            Set
                Me.inclusionsField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property otherRules() As String
            Get
                Return Me.otherRulesField
            End Get
            Set
                Me.otherRulesField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property confirmNbr() As String
            Get
                Return Me.confirmNbrField
            End Get
            Set
                Me.confirmNbrField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cancelNbr() As String
            Get
                Return Me.cancelNbrField
            End Get
            Set
                Me.cancelNbrField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property overrideStatus() As String
            Get
                Return Me.overrideStatusField
            End Get
            Set
                Me.overrideStatusField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property MBOProductCode() As String
            Get
                Return Me.mBOProductCodeField
            End Get
            Set
                Me.mBOProductCodeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_SleepMiscellaneousTypeStart
    
        Private locationCodeField As String
    
        Private addressField As ETR_SleepMiscellaneousTypeStartAddress
    
        Private contactField As ETR_SleepMiscellaneousTypeStartContact
    
        Private dateTimeField As String
    
        '''<remarks/>
        Public Property locationCode() As String
            Get
                Return Me.locationCodeField
            End Get
            Set
                Me.locationCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property address() As ETR_SleepMiscellaneousTypeStartAddress
            Get
                Return Me.addressField
            End Get
            Set
                Me.addressField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property contact() As ETR_SleepMiscellaneousTypeStartContact
            Get
                Return Me.contactField
            End Get
            Set
                Me.contactField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property dateTime() As String
            Get
                Return Me.dateTimeField
            End Get
            Set
                Me.dateTimeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_SleepMiscellaneousTypeStartAddress
    
        Private lineField As String
    
        Private complementField As String
    
        Private zipField As String
    
        Private countryCodeField As String
    
        Private latitudeField As String
    
        Private longitudeField As String
    
        Private cityCodeField As String
    
        Private cityNameField As String
    
        Private countryNameField As String
    
        Private stateCodeField As String
    
        Private stateNameField As String
    
        Private postalBoxField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property line() As String
            Get
                Return Me.lineField
            End Get
            Set
                Me.lineField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property complement() As String
            Get
                Return Me.complementField
            End Get
            Set
                Me.complementField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property zip() As String
            Get
                Return Me.zipField
            End Get
            Set
                Me.zipField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property countryCode() As String
            Get
                Return Me.countryCodeField
            End Get
            Set
                Me.countryCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property latitude() As String
            Get
                Return Me.latitudeField
            End Get
            Set
                Me.latitudeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property longitude() As String
            Get
                Return Me.longitudeField
            End Get
            Set
                Me.longitudeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cityCode() As String
            Get
                Return Me.cityCodeField
            End Get
            Set
                Me.cityCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cityName() As String
            Get
                Return Me.cityNameField
            End Get
            Set
                Me.cityNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property countryName() As String
            Get
                Return Me.countryNameField
            End Get
            Set
                Me.countryNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property stateCode() As String
            Get
                Return Me.stateCodeField
            End Get
            Set
                Me.stateCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property stateName() As String
            Get
                Return Me.stateNameField
            End Get
            Set
                Me.stateNameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property postalBox() As String
            Get
                Return Me.postalBoxField
            End Get
            Set
                Me.postalBoxField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_SleepMiscellaneousTypeStartContact
    
        Private phoneField As String
    
        Private faxField As String
    
        Private emailField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property phone() As String
            Get
                Return Me.phoneField
            End Get
            Set
                Me.phoneField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property fax() As String
            Get
                Return Me.faxField
            End Get
            Set
                Me.faxField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property email() As String
            Get
                Return Me.emailField
            End Get
            Set
                Me.emailField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_SleepMiscellaneousTypeEnd
    
        Private dateTimeField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property dateTime() As String
            Get
                Return Me.dateTimeField
            End Get
            Set
                Me.dateTimeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_SleepMiscellaneousTypeCheckIn
    
        Private startDateTimeField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property startDateTime() As String
            Get
                Return Me.startDateTimeField
            End Get
            Set
                Me.startDateTimeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_SleepMiscellaneousTypeCheckOut
    
        Private endDateTimeField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property endDateTime() As String
            Get
                Return Me.endDateTimeField
            End Get
            Set
                Me.endDateTimeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_SleepMiscellaneousTypeCustomers
    
        Private adultsField As String
    
        Private childrenField() As ETR_SleepMiscellaneousTypeCustomersChildren
    
        '''<remarks/>
        Public Property adults() As String
            Get
                Return Me.adultsField
            End Get
            Set
                Me.adultsField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("children")>  _
        Public Property children() As ETR_SleepMiscellaneousTypeCustomersChildren()
            Get
                Return Me.childrenField
            End Get
            Set
                Me.childrenField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_SleepMiscellaneousTypeCustomersChildren
    
        Private ageField As String
    
        Private ageCodeField As String
    
        Private valueField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property age() As String
            Get
                Return Me.ageField
            End Get
            Set
                Me.ageField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property ageCode() As String
            Get
                Return Me.ageCodeField
            End Get
            Set
                Me.ageCodeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()>  _
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set
                Me.valueField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_SleepMiscellaneousTypeGuarantee
    
        Private paymentFormField As String
    
        Private paymentDetailsField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property paymentForm() As String
            Get
                Return Me.paymentFormField
            End Get
            Set
                Me.paymentFormField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property paymentDetails() As String
            Get
                Return Me.paymentDetailsField
            End Get
            Set
                Me.paymentDetailsField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=true, [Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_SleepMiscellaneousTypeDeposit
    
        Private paymentFormField As String
    
        Private paymentDetailsField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property paymentForm() As String
            Get
                Return Me.paymentFormField
            End Get
            Set
                Me.paymentFormField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property paymentDetails() As String
            Get
                Return Me.paymentDetailsField
            End Get
            Set
                Me.paymentDetailsField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ETR_ShowAndEventType
    
        Private serviceProviderField As ProviderType
    
        Private seatNbrField() As String
    
        Private ticketField As ETR_ShowAndEventTypeTicket
    
        Private startField As ETR_ShowAndEventTypeStart
    
        Private endField As ETR_ShowAndEventTypeEnd
    
        Private validityField As ETR_ShowAndEventTypeValidity
    
        Private rateField As ETR_ShowAndEventTypeRate
    
        Private creationField As CreationType
    
        Private modificationField As ModificationType
    
        Private confirmationField As ConfirmationType
    
        Private descriptionsField() As ETR_TransferTypeDescriptions
    
        Private bkgChannelField As BookingChannelType
    
        Private creationChannelField As FullOriginSystemType
    
        Private externalSystemField As ExternalSystemType
    
        Private creatorField As SystemInformation
    
        Private propertiesField As String
    
        Private descriptionField As String
    
        Private nameField As String
    
        Private durationField As String
    
        Private nIPField As String
    
        Private statusField As String
    
        Private confirmNbrField As String
    
        Private cancelNbrField As String
    
        Private overrideStatusField As String
    
        Private mBOProductCodeField As String
    
        '''<remarks/>
        Public Property serviceProvider() As ProviderType
            Get
                Return Me.serviceProviderField
            End Get
            Set
                Me.serviceProviderField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("seatNbr")>  _
        Public Property seatNbr() As String()
            Get
                Return Me.seatNbrField
            End Get
            Set
                Me.seatNbrField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property ticket() As ETR_ShowAndEventTypeTicket
            Get
                Return Me.ticketField
            End Get
            Set
                Me.ticketField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property start() As ETR_ShowAndEventTypeStart
            Get
                Return Me.startField
            End Get
            Set
                Me.startField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property [end]() As ETR_ShowAndEventTypeEnd
            Get
                Return Me.endField
            End Get
            Set
                Me.endField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property validity() As ETR_ShowAndEventTypeValidity
            Get
                Return Me.validityField
            End Get
            Set
                Me.validityField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property rate() As ETR_ShowAndEventTypeRate
            Get
                Return Me.rateField
            End Get
            Set
                Me.rateField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property creation() As CreationType
            Get
                Return Me.creationField
            End Get
            Set
                Me.creationField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property modification() As ModificationType
            Get
                Return Me.modificationField
            End Get
            Set
                Me.modificationField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property confirmation() As ConfirmationType
            Get
                Return Me.confirmationField
            End Get
            Set
                Me.confirmationField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("descriptions")>  _
        Public Property descriptions() As ETR_TransferTypeDescriptions()
            Get
                Return Me.descriptionsField
            End Get
            Set
                Me.descriptionsField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property bkgChannel() As BookingChannelType
            Get
                Return Me.bkgChannelField
            End Get
            Set
                Me.bkgChannelField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property creationChannel() As FullOriginSystemType
            Get
                Return Me.creationChannelField
            End Get
            Set
                Me.creationChannelField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property externalSystem() As ExternalSystemType
            Get
                Return Me.externalSystemField
            End Get
            Set
                Me.externalSystemField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property creator() As SystemInformation
            Get
                Return Me.creatorField
            End Get
            Set
                Me.creatorField = value
            End Set
        End Property
    
        '''<remarks/>
        Public Property properties() As String
            Get
                Return Me.propertiesField
            End Get
            Set
                Me.propertiesField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property description() As String
            Get
                Return Me.descriptionField
            End Get
            Set
                Me.descriptionField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property name() As String
            Get
                Return Me.nameField
            End Get
            Set
                Me.nameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property duration() As String
            Get
                Return Me.durationField
            End Get
            Set
                Me.durationField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property NIP() As String
            Get
                Return Me.nIPField
            End Get
            Set
                Me.nIPField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property status() As String
            Get
                Return Me.statusField
            End Get
            Set
                Me.statusField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property confirmNbr() As String
            Get
                Return Me.confirmNbrField
            End Get
            Set
                Me.confirmNbrField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property cancelNbr() As String
            Get
                Return Me.cancelNbrField
            End Get
            Set
                Me.cancelNbrField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property overrideStatus() As String
            Get
                Return Me.overrideStatusField
            End Get
            Set
                Me.overrideStatusField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property MBOProductCode() As String
            Get
                Return Me.mBOProductCodeField
            End Get
            Set
                Me.mBOProductCodeField = value
            End Set
        End Property
    End Class


    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code"),  _
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://xml.amadeus.com/2010/06/ETR_Types_v4")>  _
    Partial Public Class ProviderType
    
        Private refField As String
    
        Private nameField As String
    
        Private codeField As String
    
        Private externalRefField As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property ref() As String
            Get
                Return Me.refField
            End Get
            Set
                Me.refField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property name() As String
            Get
                Return Me.nameField
            End Get
            Set
                Me.nameField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property code() As String
            Get
                Return Me.codeField
            End Get
            Set
                Me.codeField = value
            End Set
        End Property
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property externalRef() As String
            Get
                Return Me.externalRefField
            End Get
            Set
                Me.externalRefField = value
            End Set
        End Property
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class POS

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Source")> _
        Public Source() As Source

        '<remarks/>
        Public TPA_Extensions As TPA_Extensions
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class TPA_Extensions

        '<remarks/>
        Public Provider As Provider

        '<remarks/>
        Public MoreIndicator As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Provider

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Name")> _
        Public Name() As Name

        '<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("System")> _
        Public GDSSystem As String

        '<remarks/>
        Public Userid As String

        '<remarks/>
        Public Password As String
    End Class
    
    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Name

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public PseudoCityCode As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Value As String
    End Class

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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute()>  _
    Public Enum Target
    
        '''<remarks/>
        Test
    
        '''<remarks/>
        Production
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute()>  _
    Public Enum TransactionStatusCode
    
        '''<remarks/>
        Start
    
        '''<remarks/>
        [End]
    
        '''<remarks/>
        Rollback
    
        '''<remarks/>
        InSeries
    
        '''<remarks/>
        Continuation
    
        '''<remarks/>
        Subsequent
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0"),  _
        System.SerializableAttribute(),  _
        System.Diagnostics.DebuggerStepThroughAttribute(),  _
        System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class Reservation
   
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()>  _
        Public Property ID As String
    
        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="NMTOKEN")>  _
        Public Property ExternalID As String

    End Class

#Region "Errors and Warnings"
    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Success
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class Warning

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property ShortText As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")> _
        Public Property DocURL As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Status As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Tag As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property RecordID As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Property Value As String
    End Class

    '<remarks/>
    <System.Xml.Serialization.XmlRootAttribute(IsNullable:=False)> _
    Public Class [Error]

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Language As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Type As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property ShortText As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Code As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute(DataType:="anyURI")> _
        Public Property DocURL As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Status As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Tag As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property RecordID As String

        '<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property NodeList As String

        '<remarks/>
        <System.Xml.Serialization.XmlTextAttribute()> _
        Public Property Value As String
    End Class

#End Region



End Namespace