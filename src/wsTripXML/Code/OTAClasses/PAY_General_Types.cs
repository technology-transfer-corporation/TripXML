using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.VirtualCreditCard
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/Types_v3")]
    public partial class ErrorType : FreeTextType
    {

        private string typeField;

        private string shortTextField;

        private string codeField;

        private string docURLField;

        private string statusField;

        private string tagField;

        private string recordIDField;

        private string nodeListField;

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ShortText
        {
            get
            {
                return shortTextField;
            }
            set
            {
                shortTextField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string DocURL
        {
            get
            {
                return docURLField;
            }
            set
            {
                docURLField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Status
        {
            get
            {
                return statusField;
            }
            set
            {
                statusField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Tag
        {
            get
            {
                return tagField;
            }
            set
            {
                tagField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RecordID
        {
            get
            {
                return recordIDField;
            }
            set
            {
                recordIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NodeList
        {
            get
            {
                return nodeListField;
            }
            set
            {
                nodeListField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class VirtualCardDetails
    {

        /// <remarks/>
        public CardType Card { get; set; }

        /// <remarks/>
        [XmlArrayItem("Reference", IsNullable = false)]
        public VirtualCardDetailsTypeReference[] References { get; set; }

        /// <remarks/>
        public string Provider { get; set; }

        /// <remarks/>
        [XmlArrayItem("Value", IsNullable = false)]
        public VirtualCardAmountType[] Values { get; set; }
        // Public Property Values As FundsTransferType()

        /// <remarks/>
        public VirtualCardAccountType Account { get; set; }

        /// <remarks/>
        public CardRestrictionsType Limitations { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public DateTime LastUpdatedTime { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool LastUpdatedTimeSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public DateTime CreationTime { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string CreationUser { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string CreationOffice { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string CardStatus { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class VirtualCardDetailsTypeReference
    {

        /// <remarks/>
        [XmlAttribute()]
        public string Type { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class VirtualCardAccountType
    {

        private string typeField;

        private string userIdField;

        private string agencyIdField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string UserId
        {
            get
            {
                return userIdField;
            }
            set
            {
                userIdField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string AgencyId
        {
            get
            {
                return agencyIdField;
            }
            set
            {
                agencyIdField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class VirtualCardAmountType
    {

        /// <remarks/>
        [XmlAttribute()]
        public string Type { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string Amount { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string DecimalPlaces { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode { get; set; }

        public string GetFormatedAmount()
        {
            try
            {
                if (string.IsNullOrEmpty(Amount))
                {
                    return string.Empty;
                }
                decimal amt = Convert.ToDecimal(Amount);
                decimal decPlaces = Convert.ToDecimal(DecimalPlaces);


                decimal amountRet = (decimal)Math.Round((double)amt / Math.Pow(10d, (double)decPlaces), 2);
                return amountRet.ToString("#0.00");
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CardRestrictionsType
    {

        /// <remarks/>
        public CardRestrictionsTypeAllowedTransactions AllowedTransactions { get; set; }

        /// <remarks/>
        [XmlArrayItem("CurrencyCode", IsNullable = false)]
        public string[] CurrencyList { get; set; }

        /// <remarks/>
        [XmlElement("Merchant")]
        public CardRestrictionsTypeMerchant[] Merchant { get; set; }

        /// <remarks/>
        public CardRestrictionsTypeValidityPeriod ValidityPeriod { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CardRestrictionsTypeAllowedTransactions
    {

        private string maximumField;

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string Maximum
        {
            get
            {
                return maximumField;
            }
            set
            {
                maximumField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CardRestrictionsTypeMerchant
    {

        private string typeField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CardRestrictionsTypeValidityPeriod
    {

        private DateTime startDateField;

        private bool startDateFieldSpecified;

        private DateTime endDateField;

        private bool endDateFieldSpecified;

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime StartDate
        {
            get
            {
                return startDateField;
            }
            set
            {
                startDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StartDateSpecified
        {
            get
            {
                return startDateFieldSpecified;
            }
            set
            {
                startDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EndDate
        {
            get
            {
                return endDateField;
            }
            set
            {
                endDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool EndDateSpecified
        {
            get
            {
                return endDateFieldSpecified;
            }
            set
            {
                endDateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlRoot("TPA_Extensions", Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable = false)]
    public partial class TPA_ExtensionsType
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [XmlAnyElement()]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return anyField;
            }
            set
            {
                anyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    [XmlRoot("VehReservation", Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable = false)]
    public partial class VehicleReservationType
    {

        private CustomerPrimaryAdditionalType customerField;

        private VehicleReservationTypeVehSegmentCore vehSegmentCoreField;

        private VehicleSegmentAdditionalInfoType vehSegmentInfoField;

        private DateTime createDateTimeField;

        private bool createDateTimeFieldSpecified;

        private string creatorIDField;

        private DateTime lastModifyDateTimeField;

        private bool lastModifyDateTimeFieldSpecified;

        private string lastModifierIDField;

        private DateTime purgeDateField;

        private bool purgeDateFieldSpecified;

        private string reservationStatusField;

        /// <remarks/>
        public CustomerPrimaryAdditionalType Customer
        {
            get
            {
                return customerField;
            }
            set
            {
                customerField = value;
            }
        }

        /// <remarks/>
        public VehicleReservationTypeVehSegmentCore VehSegmentCore
        {
            get
            {
                return vehSegmentCoreField;
            }
            set
            {
                vehSegmentCoreField = value;
            }
        }

        /// <remarks/>
        public VehicleSegmentAdditionalInfoType VehSegmentInfo
        {
            get
            {
                return vehSegmentInfoField;
            }
            set
            {
                vehSegmentInfoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DateTime CreateDateTime
        {
            get
            {
                return createDateTimeField;
            }
            set
            {
                createDateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool CreateDateTimeSpecified
        {
            get
            {
                return createDateTimeFieldSpecified;
            }
            set
            {
                createDateTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CreatorID
        {
            get
            {
                return creatorIDField;
            }
            set
            {
                creatorIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DateTime LastModifyDateTime
        {
            get
            {
                return lastModifyDateTimeField;
            }
            set
            {
                lastModifyDateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool LastModifyDateTimeSpecified
        {
            get
            {
                return lastModifyDateTimeFieldSpecified;
            }
            set
            {
                lastModifyDateTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string LastModifierID
        {
            get
            {
                return lastModifierIDField;
            }
            set
            {
                lastModifierIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime PurgeDate
        {
            get
            {
                return purgeDateField;
            }
            set
            {
                purgeDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PurgeDateSpecified
        {
            get
            {
                return purgeDateFieldSpecified;
            }
            set
            {
                purgeDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ReservationStatus
        {
            get
            {
                return reservationStatusField;
            }
            set
            {
                reservationStatusField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerPrimaryAdditionalType
    {

        private CustomerPrimaryAdditionalTypePrimary primaryField;

        private CustomerPrimaryAdditionalTypeAdditional[] additionalField;

        /// <remarks/>
        public CustomerPrimaryAdditionalTypePrimary Primary
        {
            get
            {
                return primaryField;
            }
            set
            {
                primaryField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Additional")]
        public CustomerPrimaryAdditionalTypeAdditional[] Additional
        {
            get
            {
                return additionalField;
            }
            set
            {
                additionalField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerPrimaryAdditionalTypePrimary : CustomerType
    {

        private UniqueID_Type customerIDField;

        /// <remarks/>
        public UniqueID_Type CustomerID
        {
            get
            {
                return customerIDField;
            }
            set
            {
                customerIDField = value;
            }
        }
    }

    /// <remarks/>
    [XmlInclude(typeof(ReservationID_Type))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class UniqueID_Type
    {

        private CompanyNameType companyNameField;

        private string uRLField;

        private string typeField;

        private string instanceField;

        private string idField;

        private string iD_ContextField;

        /// <remarks/>
        public CompanyNameType CompanyName
        {
            get
            {
                return companyNameField;
            }
            set
            {
                companyNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string URL
        {
            get
            {
                return uRLField;
            }
            set
            {
                uRLField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Instance
        {
            get
            {
                return instanceField;
            }
            set
            {
                instanceField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ID
        {
            get
            {
                return idField;
            }
            set
            {
                idField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ID_Context
        {
            get
            {
                return iD_ContextField;
            }
            set
            {
                iD_ContextField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum CustomerTypeTelephoneShareSynchInd
    {

        /// <remarks/>
        Yes,

        /// <remarks/>
        No,

        /// <remarks/>
        Inherit
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum CustomerTypeTelephoneShareMarketInd
    {

        /// <remarks/>
        Yes,

        /// <remarks/>
        No,

        /// <remarks/>
        Inherit
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum TransferActionType
    {

        /// <remarks/>
        Automatic,

        /// <remarks/>
        Mandatory,

        /// <remarks/>
        Selectable
    }

    /// <remarks/>
    [XmlInclude(typeof(TravelArrangerType))]
    [XmlInclude(typeof(CompanyNamePrefType))]
    [XmlInclude(typeof(OperatingAirlineType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CompanyNameType
    {

        private string companyShortNameField;

        private string travelSectorField;

        private string codeField;

        private string codeContextField;

        private string divisionField;

        private string departmentField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string CompanyShortName
        {
            get
            {
                return companyShortNameField;
            }
            set
            {
                companyShortNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TravelSector
        {
            get
            {
                return travelSectorField;
            }
            set
            {
                travelSectorField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CodeContext
        {
            get
            {
                return codeContextField;
            }
            set
            {
                codeContextField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Division
        {
            get
            {
                return divisionField;
            }
            set
            {
                divisionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Department
        {
            get
            {
                return departmentField;
            }
            set
            {
                departmentField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum PersonNameTypeShareSynchInd
    {

        /// <remarks/>
        Yes,

        /// <remarks/>
        No,

        /// <remarks/>
        Inherit
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum PersonNameTypeShareMarketInd
    {

        /// <remarks/>
        Yes,

        /// <remarks/>
        No,

        /// <remarks/>
        Inherit
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class TravelArrangerType : CompanyNameType
    {

        private bool defaultIndField;

        private bool defaultIndFieldSpecified;

        private PersonNameTypeShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private PersonNameTypeShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string travelArrangerType1Field;

        private string rPHField;

        private string remarkField;

        /// <remarks/>
        [XmlAttribute()]
        public bool DefaultInd
        {
            get
            {
                return defaultIndField;
            }
            set
            {
                defaultIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified
        {
            get
            {
                return defaultIndFieldSpecified;
            }
            set
            {
                defaultIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("TravelArrangerType")]
        public string TravelArrangerType1
        {
            get
            {
                return travelArrangerType1Field;
            }
            set
            {
                travelArrangerType1Field = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH
        {
            get
            {
                return rPHField;
            }
            set
            {
                rPHField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Remark
        {
            get
            {
                return remarkField;
            }
            set
            {
                remarkField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum PreferLevelType
    {

        /// <remarks/>
        Only,

        /// <remarks/>
        Unacceptable,

        /// <remarks/>
        Preferred,

        /// <remarks/>
        Required,

        /// <remarks/>
        NoPreference
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CompanyNamePrefType : CompanyNameType
    {

        private PreferLevelType preferLevelField;

        private bool preferLevelFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel
        {
            get
            {
                return preferLevelField;
            }
            set
            {
                preferLevelField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified
        {
            get
            {
                return preferLevelFieldSpecified;
            }
            set
            {
                preferLevelFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class OperatingAirlineType : CompanyNameType
    {

        private string flightNumberField;

        private string resBookDesigCodeField;

        /// <remarks/>
        [XmlAttribute()]
        public string FlightNumber
        {
            get
            {
                return flightNumberField;
            }
            set
            {
                flightNumberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ResBookDesigCode
        {
            get
            {
                return resBookDesigCodeField;
            }
            set
            {
                resBookDesigCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class ReservationID_Type : UniqueID_Type
    {

        private string statusCodeField;

        private DateTime lastModifyDateTimeField;

        private bool lastModifyDateTimeFieldSpecified;

        private string bookedDateField;

        private string offerDateField;

        private DateTime syncDateTimeField;

        private bool syncDateTimeFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public string StatusCode
        {
            get
            {
                return statusCodeField;
            }
            set
            {
                statusCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DateTime LastModifyDateTime
        {
            get
            {
                return lastModifyDateTimeField;
            }
            set
            {
                lastModifyDateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool LastModifyDateTimeSpecified
        {
            get
            {
                return lastModifyDateTimeFieldSpecified;
            }
            set
            {
                lastModifyDateTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string BookedDate
        {
            get
            {
                return bookedDateField;
            }
            set
            {
                bookedDateField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string OfferDate
        {
            get
            {
                return offerDateField;
            }
            set
            {
                offerDateField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DateTime SyncDateTime
        {
            get
            {
                return syncDateTimeField;
            }
            set
            {
                syncDateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool SyncDateTimeSpecified
        {
            get
            {
                return syncDateTimeFieldSpecified;
            }
            set
            {
                syncDateTimeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum CommissionInfoTypeShareSynchInd
    {

        /// <remarks/>
        Yes,

        /// <remarks/>
        No,

        /// <remarks/>
        Inherit
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum CommissionInfoTypeShareMarketInd
    {

        /// <remarks/>
        Yes,

        /// <remarks/>
        No,

        /// <remarks/>
        Inherit
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class EmailType
    {

        private CommissionInfoTypeShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private CommissionInfoTypeShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private bool defaultIndField;

        private bool defaultIndFieldSpecified;

        private string emailType1Field;

        private string rPHField;

        private string remarkField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public CommissionInfoTypeShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool DefaultInd
        {
            get
            {
                return defaultIndField;
            }
            set
            {
                defaultIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified
        {
            get
            {
                return defaultIndFieldSpecified;
            }
            set
            {
                defaultIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("EmailType")]
        public string EmailType1
        {
            get
            {
                return emailType1Field;
            }
            set
            {
                emailType1Field = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH
        {
            get
            {
                return rPHField;
            }
            set
            {
                rPHField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Remark
        {
            get
            {
                return remarkField;
            }
            set
            {
                remarkField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }




    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypeEmail : EmailType
    {

        private TransferActionType transferActionField;

        private bool transferActionFieldSpecified;

        private string parentCompanyRefField;

        /// <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction
        {
            get
            {
                return transferActionField;
            }
            set
            {
                transferActionField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified
        {
            get
            {
                return transferActionFieldSpecified;
            }
            set
            {
                transferActionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ParentCompanyRef
        {
            get
            {
                return parentCompanyRefField;
            }
            set
            {
                parentCompanyRefField = value;
            }
        }
    }

    /// <remarks/>
    [XmlInclude(typeof(InsuranceCustomerType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerType
    {

        private PersonNameType[] personNameField;

        private CustomerTypeTelephone[] telephoneField;

        private CustomerTypeEmail[] emailField;

        private CustomerTypeAddress[] addressField;

        private CustomerTypeURL[] uRLField;

        private CustomerTypeCitizenCountryName[] citizenCountryNameField;

        private CustomerTypePhysChallName[] physChallNameField;

        private string[] petInfoField;

        private CustomerTypePaymentForm[] paymentFormField;

        private RelatedTravelerType[] relatedTravelerField;

        private ContactPersonType[] contactPersonField;

        private DocumentType[] documentField;

        private CustomerTypeCustLoyalty[] custLoyaltyField;

        private EmployeeInfoType[] employeeInfoField;

        private CompanyNameType employerInfoField;

        private CustomerTypeAdditionalLanguage[] additionalLanguageField;

        private TPA_ExtensionsType tPA_ExtensionsField;

        private DocumentTypeGender genderField;

        private bool genderFieldSpecified;

        private bool deceasedField;

        private bool deceasedFieldSpecified;

        private string lockoutTypeField;

        private DateTime birthDateField;

        private bool birthDateFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        private bool vIP_IndicatorField;

        private bool vIP_IndicatorFieldSpecified;

        private string textField;

        private string languageField;

        private string customerValueField;

        private CustomerTypeMaritalStatus maritalStatusField;

        private bool maritalStatusFieldSpecified;

        private bool previouslyMarriedIndicatorField;

        private bool previouslyMarriedIndicatorFieldSpecified;

        private string childQuantityField;

        /// <remarks/>
        [XmlElement("PersonName")]
        public PersonNameType[] PersonName
        {
            get
            {
                return personNameField;
            }
            set
            {
                personNameField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Telephone")]
        public CustomerTypeTelephone[] Telephone
        {
            get
            {
                return telephoneField;
            }
            set
            {
                telephoneField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Email")]
        public CustomerTypeEmail[] Email
        {
            get
            {
                return emailField;
            }
            set
            {
                emailField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Address")]
        public CustomerTypeAddress[] Address
        {
            get
            {
                return addressField;
            }
            set
            {
                addressField = value;
            }
        }

        /// <remarks/>
        [XmlElement("URL")]
        public CustomerTypeURL[] URL
        {
            get
            {
                return uRLField;
            }
            set
            {
                uRLField = value;
            }
        }

        /// <remarks/>
        [XmlElement("CitizenCountryName")]
        public CustomerTypeCitizenCountryName[] CitizenCountryName
        {
            get
            {
                return citizenCountryNameField;
            }
            set
            {
                citizenCountryNameField = value;
            }
        }

        /// <remarks/>
        [XmlElement("PhysChallName")]
        public CustomerTypePhysChallName[] PhysChallName
        {
            get
            {
                return physChallNameField;
            }
            set
            {
                physChallNameField = value;
            }
        }

        /// <remarks/>
        [XmlElement("PetInfo")]
        public string[] PetInfo
        {
            get
            {
                return petInfoField;
            }
            set
            {
                petInfoField = value;
            }
        }

        /// <remarks/>
        [XmlElement("PaymentForm")]
        public CustomerTypePaymentForm[] PaymentForm
        {
            get
            {
                return paymentFormField;
            }
            set
            {
                paymentFormField = value;
            }
        }

        /// <remarks/>
        [XmlElement("RelatedTraveler")]
        public RelatedTravelerType[] RelatedTraveler
        {
            get
            {
                return relatedTravelerField;
            }
            set
            {
                relatedTravelerField = value;
            }
        }

        /// <remarks/>
        [XmlElement("ContactPerson")]
        public ContactPersonType[] ContactPerson
        {
            get
            {
                return contactPersonField;
            }
            set
            {
                contactPersonField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Document")]
        public DocumentType[] Document
        {
            get
            {
                return documentField;
            }
            set
            {
                documentField = value;
            }
        }

        /// <remarks/>
        [XmlElement("CustLoyalty")]
        public CustomerTypeCustLoyalty[] CustLoyalty
        {
            get
            {
                return custLoyaltyField;
            }
            set
            {
                custLoyaltyField = value;
            }
        }

        /// <remarks/>
        [XmlElement("EmployeeInfo")]
        public EmployeeInfoType[] EmployeeInfo
        {
            get
            {
                return employeeInfoField;
            }
            set
            {
                employeeInfoField = value;
            }
        }

        /// <remarks/>
        public CompanyNameType EmployerInfo
        {
            get
            {
                return employerInfoField;
            }
            set
            {
                employerInfoField = value;
            }
        }

        /// <remarks/>
        [XmlElement("AdditionalLanguage")]
        public CustomerTypeAdditionalLanguage[] AdditionalLanguage
        {
            get
            {
                return additionalLanguageField;
            }
            set
            {
                additionalLanguageField = value;
            }
        }

        /// <remarks/>
        public TPA_ExtensionsType TPA_Extensions
        {
            get
            {
                return tPA_ExtensionsField;
            }
            set
            {
                tPA_ExtensionsField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DocumentTypeGender Gender
        {
            get
            {
                return genderField;
            }
            set
            {
                genderField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool GenderSpecified
        {
            get
            {
                return genderFieldSpecified;
            }
            set
            {
                genderFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool Deceased
        {
            get
            {
                return deceasedField;
            }
            set
            {
                deceasedField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DeceasedSpecified
        {
            get
            {
                return deceasedFieldSpecified;
            }
            set
            {
                deceasedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string LockoutType
        {
            get
            {
                return lockoutTypeField;
            }
            set
            {
                lockoutTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate
        {
            get
            {
                return birthDateField;
            }
            set
            {
                birthDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool BirthDateSpecified
        {
            get
            {
                return birthDateFieldSpecified;
            }
            set
            {
                birthDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool VIP_Indicator
        {
            get
            {
                return vIP_IndicatorField;
            }
            set
            {
                vIP_IndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool VIP_IndicatorSpecified
        {
            get
            {
                return vIP_IndicatorFieldSpecified;
            }
            set
            {
                vIP_IndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "language")]
        public string Language
        {
            get
            {
                return languageField;
            }
            set
            {
                languageField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CustomerValue
        {
            get
            {
                return customerValueField;
            }
            set
            {
                customerValueField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeMaritalStatus MaritalStatus
        {
            get
            {
                return maritalStatusField;
            }
            set
            {
                maritalStatusField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MaritalStatusSpecified
        {
            get
            {
                return maritalStatusFieldSpecified;
            }
            set
            {
                maritalStatusFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool PreviouslyMarriedIndicator
        {
            get
            {
                return previouslyMarriedIndicatorField;
            }
            set
            {
                previouslyMarriedIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PreviouslyMarriedIndicatorSpecified
        {
            get
            {
                return previouslyMarriedIndicatorFieldSpecified;
            }
            set
            {
                previouslyMarriedIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string ChildQuantity
        {
            get
            {
                return childQuantityField;
            }
            set
            {
                childQuantityField = value;
            }
        }
    }

    /// <remarks/>
    [XmlInclude(typeof(PersonNameType1))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PersonNameType
    {

        private string[] namePrefixField;

        private string[] givenNameField;

        private string[] middleNameField;

        private string surnamePrefixField;

        private string surnameField;

        private string[] nameSuffixField;

        private string[] nameTitleField;

        private PersonNameTypeDocument documentField;

        private PersonNameTypeShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private PersonNameTypeShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string nameTypeField;

        /// <remarks/>
        [XmlElement("NamePrefix")]
        public string[] NamePrefix
        {
            get
            {
                return namePrefixField;
            }
            set
            {
                namePrefixField = value;
            }
        }

        /// <remarks/>
        [XmlElement("GivenName")]
        public string[] GivenName
        {
            get
            {
                return givenNameField;
            }
            set
            {
                givenNameField = value;
            }
        }

        /// <remarks/>
        [XmlElement("MiddleName")]
        public string[] MiddleName
        {
            get
            {
                return middleNameField;
            }
            set
            {
                middleNameField = value;
            }
        }

        /// <remarks/>
        public string SurnamePrefix
        {
            get
            {
                return surnamePrefixField;
            }
            set
            {
                surnamePrefixField = value;
            }
        }

        /// <remarks/>
        public string Surname
        {
            get
            {
                return surnameField;
            }
            set
            {
                surnameField = value;
            }
        }

        /// <remarks/>
        [XmlElement("NameSuffix")]
        public string[] NameSuffix
        {
            get
            {
                return nameSuffixField;
            }
            set
            {
                nameSuffixField = value;
            }
        }

        /// <remarks/>
        [XmlElement("NameTitle")]
        public string[] NameTitle
        {
            get
            {
                return nameTitleField;
            }
            set
            {
                nameTitleField = value;
            }
        }

        /// <remarks/>
        public PersonNameTypeDocument Document
        {
            get
            {
                return documentField;
            }
            set
            {
                documentField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NameType
        {
            get
            {
                return nameTypeField;
            }
            set
            {
                nameTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PersonNameTypeDocument
    {

        private string docIDField;

        private string docTypeField;

        /// <remarks/>
        [XmlAttribute()]
        public string DocID
        {
            get
            {
                return docIDField;
            }
            set
            {
                docIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DocType
        {
            get
            {
                return docTypeField;
            }
            set
            {
                docTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(TypeName = "PersonNameType", Namespace = "http://xml.amadeus.com/2010/06/Types_v3")]
    public partial class PersonNameType1 : PersonNameType
    {

        private string nameVarietyField;

        private string languageField;

        private bool displayedNameField;

        private bool displayedNameFieldSpecified;

        private string romanizationMethodField;

        /// <remarks/>
        [XmlAttribute()]
        public string NameVariety
        {
            get
            {
                return nameVarietyField;
            }
            set
            {
                nameVarietyField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "language")]
        public string Language
        {
            get
            {
                return languageField;
            }
            set
            {
                languageField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool DisplayedName
        {
            get
            {
                return displayedNameField;
            }
            set
            {
                displayedNameField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DisplayedNameSpecified
        {
            get
            {
                return displayedNameFieldSpecified;
            }
            set
            {
                displayedNameFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RomanizationMethod
        {
            get
            {
                return romanizationMethodField;
            }
            set
            {
                romanizationMethodField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypeTelephone
    {

        private DateTime effectiveDateField;

        private bool effectiveDateFieldSpecified;

        private DateTime expireDateField;

        private bool expireDateFieldSpecified;

        private bool expireDateExclusiveIndicatorField;

        private bool expireDateExclusiveIndicatorFieldSpecified;

        private CustomerTypeTelephoneShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private CustomerTypeTelephoneShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string phoneLocationTypeField;

        private string phoneTechTypeField;

        private string phoneUseTypeField;

        private string countryAccessCodeField;

        private string areaCityCodeField;

        private string phoneNumberField;

        private string extensionField;

        private string pINField;

        private string remarkField;

        private bool formattedIndField;

        private bool formattedIndFieldSpecified;

        private bool defaultIndField;

        private bool defaultIndFieldSpecified;

        private string rPHField;

        private TransferActionType transferActionField;

        private bool transferActionFieldSpecified;

        private string parentCompanyRefField;

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDateField;
            }
            set
            {
                effectiveDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified
        {
            get
            {
                return effectiveDateFieldSpecified;
            }
            set
            {
                effectiveDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate
        {
            get
            {
                return expireDateField;
            }
            set
            {
                expireDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified
        {
            get
            {
                return expireDateFieldSpecified;
            }
            set
            {
                expireDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator
        {
            get
            {
                return expireDateExclusiveIndicatorField;
            }
            set
            {
                expireDateExclusiveIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified
        {
            get
            {
                return expireDateExclusiveIndicatorFieldSpecified;
            }
            set
            {
                expireDateExclusiveIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeTelephoneShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeTelephoneShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneLocationType
        {
            get
            {
                return phoneLocationTypeField;
            }
            set
            {
                phoneLocationTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneTechType
        {
            get
            {
                return phoneTechTypeField;
            }
            set
            {
                phoneTechTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneUseType
        {
            get
            {
                return phoneUseTypeField;
            }
            set
            {
                phoneUseTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CountryAccessCode
        {
            get
            {
                return countryAccessCodeField;
            }
            set
            {
                countryAccessCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string AreaCityCode
        {
            get
            {
                return areaCityCodeField;
            }
            set
            {
                areaCityCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneNumber
        {
            get
            {
                return phoneNumberField;
            }
            set
            {
                phoneNumberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Extension
        {
            get
            {
                return extensionField;
            }
            set
            {
                extensionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PIN
        {
            get
            {
                return pINField;
            }
            set
            {
                pINField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Remark
        {
            get
            {
                return remarkField;
            }
            set
            {
                remarkField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool FormattedInd
        {
            get
            {
                return formattedIndField;
            }
            set
            {
                formattedIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool FormattedIndSpecified
        {
            get
            {
                return formattedIndFieldSpecified;
            }
            set
            {
                formattedIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool DefaultInd
        {
            get
            {
                return defaultIndField;
            }
            set
            {
                defaultIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified
        {
            get
            {
                return defaultIndFieldSpecified;
            }
            set
            {
                defaultIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH
        {
            get
            {
                return rPHField;
            }
            set
            {
                rPHField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction
        {
            get
            {
                return transferActionField;
            }
            set
            {
                transferActionField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified
        {
            get
            {
                return transferActionFieldSpecified;
            }
            set
            {
                transferActionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ParentCompanyRef
        {
            get
            {
                return parentCompanyRefField;
            }
            set
            {
                parentCompanyRefField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypeAddress : AddressInfoType
    {

        private CompanyNameType companyNameField;

        private PersonNameType addresseeNameField;

        private DateTime effectiveDateField;

        private bool effectiveDateFieldSpecified;

        private DateTime expireDateField;

        private bool expireDateFieldSpecified;

        private bool expireDateExclusiveIndicatorField;

        private bool expireDateExclusiveIndicatorFieldSpecified;

        private CustomerTypeAddressValidationStatus validationStatusField;

        private bool validationStatusFieldSpecified;

        private TransferActionType transferActionField;

        private bool transferActionFieldSpecified;

        private string parentCompanyRefField;

        /// <remarks/>
        public CompanyNameType CompanyName
        {
            get
            {
                return companyNameField;
            }
            set
            {
                companyNameField = value;
            }
        }

        /// <remarks/>
        public PersonNameType AddresseeName
        {
            get
            {
                return addresseeNameField;
            }
            set
            {
                addresseeNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDateField;
            }
            set
            {
                effectiveDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified
        {
            get
            {
                return effectiveDateFieldSpecified;
            }
            set
            {
                effectiveDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate
        {
            get
            {
                return expireDateField;
            }
            set
            {
                expireDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified
        {
            get
            {
                return expireDateFieldSpecified;
            }
            set
            {
                expireDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator
        {
            get
            {
                return expireDateExclusiveIndicatorField;
            }
            set
            {
                expireDateExclusiveIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified
        {
            get
            {
                return expireDateExclusiveIndicatorFieldSpecified;
            }
            set
            {
                expireDateExclusiveIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeAddressValidationStatus ValidationStatus
        {
            get
            {
                return validationStatusField;
            }
            set
            {
                validationStatusField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ValidationStatusSpecified
        {
            get
            {
                return validationStatusFieldSpecified;
            }
            set
            {
                validationStatusFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction
        {
            get
            {
                return transferActionField;
            }
            set
            {
                transferActionField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified
        {
            get
            {
                return transferActionFieldSpecified;
            }
            set
            {
                transferActionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ParentCompanyRef
        {
            get
            {
                return parentCompanyRefField;
            }
            set
            {
                parentCompanyRefField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum CustomerTypeAddressValidationStatus
    {

        /// <remarks/>
        SystemValidated,

        /// <remarks/>
        UserValidated,

        /// <remarks/>
        NotChecked
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class AddressInfoType : AddressType
    {

        private bool defaultIndField;

        private bool defaultIndFieldSpecified;

        private string useTypeField;

        private string rPHField;

        /// <remarks/>
        [XmlAttribute()]
        public bool DefaultInd
        {
            get
            {
                return defaultIndField;
            }
            set
            {
                defaultIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified
        {
            get
            {
                return defaultIndFieldSpecified;
            }
            set
            {
                defaultIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string UseType
        {
            get
            {
                return useTypeField;
            }
            set
            {
                useTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH
        {
            get
            {
                return rPHField;
            }
            set
            {
                rPHField = value;
            }
        }
    }

    /// <remarks/>
    [XmlInclude(typeof(AddressInfoType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class AddressType
    {

        private AddressTypeStreetNmbr streetNmbrField;

        private AddressTypeBldgRoom[] bldgRoomField;

        private string[] addressLineField;

        private string cityNameField;

        private string postalCodeField;

        private string countyField;

        private StateProvType stateProvField;

        private CountryNameType countryNameField;

        private bool formattedIndField;

        private bool formattedIndFieldSpecified;

        private PersonNameTypeShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private PersonNameTypeShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string typeField;

        private string remarkField;

        /// <remarks/>
        public AddressTypeStreetNmbr StreetNmbr
        {
            get
            {
                return streetNmbrField;
            }
            set
            {
                streetNmbrField = value;
            }
        }

        /// <remarks/>
        [XmlElement("BldgRoom")]
        public AddressTypeBldgRoom[] BldgRoom
        {
            get
            {
                return bldgRoomField;
            }
            set
            {
                bldgRoomField = value;
            }
        }

        /// <remarks/>
        [XmlElement("AddressLine")]
        public string[] AddressLine
        {
            get
            {
                return addressLineField;
            }
            set
            {
                addressLineField = value;
            }
        }

        /// <remarks/>
        public string CityName
        {
            get
            {
                return cityNameField;
            }
            set
            {
                cityNameField = value;
            }
        }

        /// <remarks/>
        public string PostalCode
        {
            get
            {
                return postalCodeField;
            }
            set
            {
                postalCodeField = value;
            }
        }

        /// <remarks/>
        public string County
        {
            get
            {
                return countyField;
            }
            set
            {
                countyField = value;
            }
        }

        /// <remarks/>
        public StateProvType StateProv
        {
            get
            {
                return stateProvField;
            }
            set
            {
                stateProvField = value;
            }
        }

        /// <remarks/>
        public CountryNameType CountryName
        {
            get
            {
                return countryNameField;
            }
            set
            {
                countryNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool FormattedInd
        {
            get
            {
                return formattedIndField;
            }
            set
            {
                formattedIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool FormattedIndSpecified
        {
            get
            {
                return formattedIndFieldSpecified;
            }
            set
            {
                formattedIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Remark
        {
            get
            {
                return remarkField;
            }
            set
            {
                remarkField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class AddressTypeStreetNmbr : StreetNmbrType
    {

        private string streetNmbrSuffixField;

        private string streetDirectionField;

        private string ruralRouteNmbrField;

        /// <remarks/>
        [XmlAttribute()]
        public string StreetNmbrSuffix
        {
            get
            {
                return streetNmbrSuffixField;
            }
            set
            {
                streetNmbrSuffixField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string StreetDirection
        {
            get
            {
                return streetDirectionField;
            }
            set
            {
                streetDirectionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RuralRouteNmbr
        {
            get
            {
                return ruralRouteNmbrField;
            }
            set
            {
                ruralRouteNmbrField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class StreetNmbrType
    {

        private string pO_BoxField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string PO_Box
        {
            get
            {
                return pO_BoxField;
            }
            set
            {
                pO_BoxField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class AddressTypeBldgRoom
    {

        private bool bldgNameIndicatorField;

        private bool bldgNameIndicatorFieldSpecified;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public bool BldgNameIndicator
        {
            get
            {
                return bldgNameIndicatorField;
            }
            set
            {
                bldgNameIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool BldgNameIndicatorSpecified
        {
            get
            {
                return bldgNameIndicatorFieldSpecified;
            }
            set
            {
                bldgNameIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class StateProvType
    {

        private string stateCodeField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string StateCode
        {
            get
            {
                return stateCodeField;
            }
            set
            {
                stateCodeField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CountryNameType
    {

        private string codeField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypeURL : URL_Type
    {

        private TransferActionType transferActionField;

        private bool transferActionFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction
        {
            get
            {
                return transferActionField;
            }
            set
            {
                transferActionField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified
        {
            get
            {
                return transferActionFieldSpecified;
            }
            set
            {
                transferActionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class URL_Type
    {

        private PersonNameTypeShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private PersonNameTypeShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string typeField;

        private bool defaultIndField;

        private bool defaultIndFieldSpecified;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool DefaultInd
        {
            get
            {
                return defaultIndField;
            }
            set
            {
                defaultIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified
        {
            get
            {
                return defaultIndFieldSpecified;
            }
            set
            {
                defaultIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlText(DataType = "anyURI")]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypeCitizenCountryName
    {

        private bool defaultIndField;

        private bool defaultIndFieldSpecified;

        private string codeField;

        /// <remarks/>
        [XmlAttribute()]
        public bool DefaultInd
        {
            get
            {
                return defaultIndField;
            }
            set
            {
                defaultIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified
        {
            get
            {
                return defaultIndFieldSpecified;
            }
            set
            {
                defaultIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypePhysChallName
    {

        private bool physChallIndField;

        private bool physChallIndFieldSpecified;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public bool PhysChallInd
        {
            get
            {
                return physChallIndField;
            }
            set
            {
                physChallIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PhysChallIndSpecified
        {
            get
            {
                return physChallIndFieldSpecified;
            }
            set
            {
                physChallIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypePaymentForm : PaymentFormType
    {

        private CustomerTypePaymentFormAssociatedSupplier associatedSupplierField;

        private TransferActionType transferActionField;

        private bool transferActionFieldSpecified;

        private bool defaultIndField;

        private bool defaultIndFieldSpecified;

        private string parentCompanyRefField;

        /// <remarks/>
        public CustomerTypePaymentFormAssociatedSupplier AssociatedSupplier
        {
            get
            {
                return associatedSupplierField;
            }
            set
            {
                associatedSupplierField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public TransferActionType TransferAction
        {
            get
            {
                return transferActionField;
            }
            set
            {
                transferActionField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TransferActionSpecified
        {
            get
            {
                return transferActionFieldSpecified;
            }
            set
            {
                transferActionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool DefaultInd
        {
            get
            {
                return defaultIndField;
            }
            set
            {
                defaultIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified
        {
            get
            {
                return defaultIndFieldSpecified;
            }
            set
            {
                defaultIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ParentCompanyRef
        {
            get
            {
                return parentCompanyRefField;
            }
            set
            {
                parentCompanyRefField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypePaymentFormAssociatedSupplier
    {

        private string companyShortNameField;

        private string travelSectorField;

        private string codeField;

        private string codeContextField;

        /// <remarks/>
        [XmlAttribute()]
        public string CompanyShortName
        {
            get
            {
                return companyShortNameField;
            }
            set
            {
                companyShortNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TravelSector
        {
            get
            {
                return travelSectorField;
            }
            set
            {
                travelSectorField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CodeContext
        {
            get
            {
                return codeContextField;
            }
            set
            {
                codeContextField = value;
            }
        }
    }

    /// <remarks/>
    [XmlInclude(typeof(PaymentResponseType))]
    [XmlInclude(typeof(HotelPaymentFormType))]
    [XmlInclude(typeof(PaymentDetailType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class PaymentFormType
    {


        /// <remarks/>
        [XmlElement("BankAcct", typeof(BankAcctType))]
        [XmlElement("Cash", typeof(PaymentFormTypeCash))]
        [XmlElement("DirectBill", typeof(DirectBillType))]
        [XmlElement("LoyaltyRedemption", typeof(PaymentFormTypeLoyaltyRedemption))]
        [XmlElement("MiscChargeOrder", typeof(PaymentFormTypeMiscChargeOrder))]
        [XmlElement("PaymentCard", typeof(PaymentCardType))]
        [XmlElement("Ticket", typeof(PaymentFormTypeTicket))]
        [XmlElement("Voucher", typeof(PaymentFormTypeVoucher))]
        public object Item { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareSynchInd ShareSynchInd { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareMarketInd ShareMarketInd { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string CostCenterID { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public PaymentFormTypePaymentTransactionTypeCode PaymentTransactionTypeCode { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool PaymentTransactionTypeCodeSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public bool GuaranteeIndicator { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool GuaranteeIndicatorSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string GuaranteeTypeCode { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string GuaranteeID { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Remark { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class BankAcctType
    {

        /// <remarks/>
        public string BankAcctName { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareSynchInd ShareSynchInd { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareMarketInd ShareMarketInd { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string BankID { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string AcctType { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string BankAcctNumber { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public bool ChecksAcceptedInd { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ChecksAcceptedIndSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string CheckNumber { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class PaymentFormTypeCash
    {

        /// <remarks/>
        [XmlAttribute()]
        public bool CashIndicator { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool CashIndicatorSpecified { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class DirectBillType
    {

        /// <remarks/>
        public DirectBillTypeCompanyName CompanyName { get; set; }

        /// <remarks/>
        public AddressInfoType Address { get; set; }

        /// <remarks/>
        public EmailType Email { get; set; }

        /// <remarks/>
        public DirectBillTypeTelephone Telephone { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareSynchInd ShareSynchInd { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareMarketInd ShareMarketInd { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string DirectBill_ID { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string BillingNumber { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class DirectBillTypeCompanyName : CompanyNameType
    {

        /// <remarks/>
        [XmlAttribute()]
        public string ContactName { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class DirectBillTypeTelephone
    {

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeTelephoneShareSynchInd ShareSynchInd { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeTelephoneShareMarketInd ShareMarketInd { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneLocationType { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneTechType { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneUseType { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string CountryAccessCode { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string AreaCityCode { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneNumber { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Extension { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string PIN { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Remark { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public bool FormattedInd { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool FormattedIndSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public bool DefaultInd { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class PaymentFormTypeLoyaltyRedemption
    {

        /// <remarks/>
        [XmlElement("LoyaltyCertificate")]
        public PaymentFormTypeLoyaltyRedemptionLoyaltyCertificate[] LoyaltyCertificate { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string CertificateNumber { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string MemberNumber { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string ProgramName { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string PromotionCode { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string[] PromotionVendorCode { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public long RedemptionQuantity { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool RedemptionQuantitySpecified { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class PaymentFormTypeLoyaltyRedemptionLoyaltyCertificate
    {

        /// <remarks/>
        [XmlAttribute()]
        public string ID { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string ID_Context { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string CertificateNumber { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string MemberNumber { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string ProgramName { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public long NmbrOfNights { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool NmbrOfNightsSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public PaymentFormTypeLoyaltyRedemptionLoyaltyCertificateFormat Format { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool FormatSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Status { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum PaymentFormTypeLoyaltyRedemptionLoyaltyCertificateFormat
    {

        /// <remarks/>
        Paper,

        /// <remarks/>
        Electronic
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class PaymentFormTypeMiscChargeOrder
    {

        /// <remarks/>
        [XmlAttribute()]
        public string TicketNumber { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string OriginalTicketNumber { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string OriginalIssuePlace { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime OriginalIssueDate { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool OriginalIssueDateSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string OriginalIssueIATA { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string OriginalPaymentForm { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public PaymentFormTypeMiscChargeOrderCheckInhibitorType CheckInhibitorType { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool CheckInhibitorTypeSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string[] CouponRPHs { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public bool PaperMCO_ExistInd { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool PaperMCO_ExistIndSpecified { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum PaymentFormTypeMiscChargeOrderCheckInhibitorType
    {

        /// <remarks/>
        CheckDigit,

        /// <remarks/>
        InterlineAgreement,

        /// <remarks/>
        Both
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class PaymentCardType
    {

        private string cardHolderNameField;

        private PaymentCardTypeCardIssuerName cardIssuerNameField;

        private AddressType addressField;

        private PaymentCardTypeTelephone[] telephoneField;

        private EmailType[] emailField;

        private PaymentCardTypeCustLoyalty[] custLoyaltyField;

        private PaymentCardTypeSignatureOnFile signatureOnFileField;

        private PaymentCardTypeMagneticStripe magneticStripeField;

        private PersonNameTypeShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private PersonNameTypeShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string cardTypeField;

        private string cardCodeField;

        private string cardNumberField;

        private string seriesCodeField;

        private string effectiveDateField;

        private string expireDateField;

        private string maskedCardNumberField;

        private string cardHolderRPHField;

        private bool extendPaymentIndicatorField;

        private bool extendPaymentIndicatorFieldSpecified;

        private string countryOfIssueField;

        private string extendedPaymentQuantityField;

        private bool signatureOnFileIndicatorField;

        private bool signatureOnFileIndicatorFieldSpecified;

        private string companyCardReferenceField;

        private string remarkField;

        private string encryptionKeyField;

        /// <remarks/>
        public string CardHolderName
        {
            get
            {
                return cardHolderNameField;
            }
            set
            {
                cardHolderNameField = value;
            }
        }

        /// <remarks/>
        public PaymentCardTypeCardIssuerName CardIssuerName
        {
            get
            {
                return cardIssuerNameField;
            }
            set
            {
                cardIssuerNameField = value;
            }
        }

        /// <remarks/>
        public AddressType Address
        {
            get
            {
                return addressField;
            }
            set
            {
                addressField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Telephone")]
        public PaymentCardTypeTelephone[] Telephone
        {
            get
            {
                return telephoneField;
            }
            set
            {
                telephoneField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Email")]
        public EmailType[] Email
        {
            get
            {
                return emailField;
            }
            set
            {
                emailField = value;
            }
        }

        /// <remarks/>
        [XmlElement("CustLoyalty")]
        public PaymentCardTypeCustLoyalty[] CustLoyalty
        {
            get
            {
                return custLoyaltyField;
            }
            set
            {
                custLoyaltyField = value;
            }
        }

        /// <remarks/>
        public PaymentCardTypeSignatureOnFile SignatureOnFile
        {
            get
            {
                return signatureOnFileField;
            }
            set
            {
                signatureOnFileField = value;
            }
        }

        /// <remarks/>
        public PaymentCardTypeMagneticStripe MagneticStripe
        {
            get
            {
                return magneticStripeField;
            }
            set
            {
                magneticStripeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CardType
        {
            get
            {
                return cardTypeField;
            }
            set
            {
                cardTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CardCode
        {
            get
            {
                return cardCodeField;
            }
            set
            {
                cardCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CardNumber
        {
            get
            {
                return cardNumberField;
            }
            set
            {
                cardNumberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string SeriesCode
        {
            get
            {
                return seriesCodeField;
            }
            set
            {
                seriesCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string EffectiveDate
        {
            get
            {
                return effectiveDateField;
            }
            set
            {
                effectiveDateField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ExpireDate
        {
            get
            {
                return expireDateField;
            }
            set
            {
                expireDateField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string MaskedCardNumber
        {
            get
            {
                return maskedCardNumberField;
            }
            set
            {
                maskedCardNumberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CardHolderRPH
        {
            get
            {
                return cardHolderRPHField;
            }
            set
            {
                cardHolderRPHField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ExtendPaymentIndicator
        {
            get
            {
                return extendPaymentIndicatorField;
            }
            set
            {
                extendPaymentIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExtendPaymentIndicatorSpecified
        {
            get
            {
                return extendPaymentIndicatorFieldSpecified;
            }
            set
            {
                extendPaymentIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CountryOfIssue
        {
            get
            {
                return countryOfIssueField;
            }
            set
            {
                countryOfIssueField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string ExtendedPaymentQuantity
        {
            get
            {
                return extendedPaymentQuantityField;
            }
            set
            {
                extendedPaymentQuantityField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool SignatureOnFileIndicator
        {
            get
            {
                return signatureOnFileIndicatorField;
            }
            set
            {
                signatureOnFileIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool SignatureOnFileIndicatorSpecified
        {
            get
            {
                return signatureOnFileIndicatorFieldSpecified;
            }
            set
            {
                signatureOnFileIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CompanyCardReference
        {
            get
            {
                return companyCardReferenceField;
            }
            set
            {
                companyCardReferenceField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Remark
        {
            get
            {
                return remarkField;
            }
            set
            {
                remarkField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string EncryptionKey
        {
            get
            {
                return encryptionKeyField;
            }
            set
            {
                encryptionKeyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PaymentCardTypeCardIssuerName
    {

        private string bankIDField;

        /// <remarks/>
        [XmlAttribute()]
        public string BankID
        {
            get
            {
                return bankIDField;
            }
            set
            {
                bankIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PaymentCardTypeTelephone
    {

        private CustomerTypeTelephoneShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private CustomerTypeTelephoneShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string phoneLocationTypeField;

        private string phoneTechTypeField;

        private string phoneUseTypeField;

        private string countryAccessCodeField;

        private string areaCityCodeField;

        private string phoneNumberField;

        private string extensionField;

        private string pINField;

        private string remarkField;

        private bool formattedIndField;

        private bool formattedIndFieldSpecified;

        private bool defaultIndField;

        private bool defaultIndFieldSpecified;

        private string rPHField;

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeTelephoneShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeTelephoneShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneLocationType
        {
            get
            {
                return phoneLocationTypeField;
            }
            set
            {
                phoneLocationTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneTechType
        {
            get
            {
                return phoneTechTypeField;
            }
            set
            {
                phoneTechTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneUseType
        {
            get
            {
                return phoneUseTypeField;
            }
            set
            {
                phoneUseTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CountryAccessCode
        {
            get
            {
                return countryAccessCodeField;
            }
            set
            {
                countryAccessCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string AreaCityCode
        {
            get
            {
                return areaCityCodeField;
            }
            set
            {
                areaCityCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneNumber
        {
            get
            {
                return phoneNumberField;
            }
            set
            {
                phoneNumberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Extension
        {
            get
            {
                return extensionField;
            }
            set
            {
                extensionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PIN
        {
            get
            {
                return pINField;
            }
            set
            {
                pINField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Remark
        {
            get
            {
                return remarkField;
            }
            set
            {
                remarkField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool FormattedInd
        {
            get
            {
                return formattedIndField;
            }
            set
            {
                formattedIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool FormattedIndSpecified
        {
            get
            {
                return formattedIndFieldSpecified;
            }
            set
            {
                formattedIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool DefaultInd
        {
            get
            {
                return defaultIndField;
            }
            set
            {
                defaultIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified
        {
            get
            {
                return defaultIndFieldSpecified;
            }
            set
            {
                defaultIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH
        {
            get
            {
                return rPHField;
            }
            set
            {
                rPHField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PaymentCardTypeCustLoyalty
    {

        private PaymentCardTypeCustLoyaltyShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private PaymentCardTypeCustLoyaltyShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string programIDField;

        private string membershipIDField;

        private string travelSectorField;

        private string loyalLevelField;

        private string loyalLevelCodeField;

        private PaymentCardTypeCustLoyaltySingleVendorInd singleVendorIndField;

        private bool singleVendorIndFieldSpecified;

        private DateTime signupDateField;

        private bool signupDateFieldSpecified;

        private DateTime effectiveDateField;

        private bool effectiveDateFieldSpecified;

        private DateTime expireDateField;

        private bool expireDateFieldSpecified;

        private bool expireDateExclusiveIndicatorField;

        private bool expireDateExclusiveIndicatorFieldSpecified;

        private string rPHField;

        private string[] vendorCodeField;

        private bool primaryLoyaltyIndicatorField;

        private bool primaryLoyaltyIndicatorFieldSpecified;

        private string allianceLoyaltyLevelNameField;

        private string customerTypeField;

        private string customerValueField;

        private string passwordField;

        /// <remarks/>
        [XmlAttribute()]
        public PaymentCardTypeCustLoyaltyShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PaymentCardTypeCustLoyaltyShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ProgramID
        {
            get
            {
                return programIDField;
            }
            set
            {
                programIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string MembershipID
        {
            get
            {
                return membershipIDField;
            }
            set
            {
                membershipIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TravelSector
        {
            get
            {
                return travelSectorField;
            }
            set
            {
                travelSectorField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string LoyalLevel
        {
            get
            {
                return loyalLevelField;
            }
            set
            {
                loyalLevelField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string LoyalLevelCode
        {
            get
            {
                return loyalLevelCodeField;
            }
            set
            {
                loyalLevelCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PaymentCardTypeCustLoyaltySingleVendorInd SingleVendorInd
        {
            get
            {
                return singleVendorIndField;
            }
            set
            {
                singleVendorIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool SingleVendorIndSpecified
        {
            get
            {
                return singleVendorIndFieldSpecified;
            }
            set
            {
                singleVendorIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime SignupDate
        {
            get
            {
                return signupDateField;
            }
            set
            {
                signupDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool SignupDateSpecified
        {
            get
            {
                return signupDateFieldSpecified;
            }
            set
            {
                signupDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDateField;
            }
            set
            {
                effectiveDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified
        {
            get
            {
                return effectiveDateFieldSpecified;
            }
            set
            {
                effectiveDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate
        {
            get
            {
                return expireDateField;
            }
            set
            {
                expireDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified
        {
            get
            {
                return expireDateFieldSpecified;
            }
            set
            {
                expireDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator
        {
            get
            {
                return expireDateExclusiveIndicatorField;
            }
            set
            {
                expireDateExclusiveIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified
        {
            get
            {
                return expireDateExclusiveIndicatorFieldSpecified;
            }
            set
            {
                expireDateExclusiveIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH
        {
            get
            {
                return rPHField;
            }
            set
            {
                rPHField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string[] VendorCode
        {
            get
            {
                return vendorCodeField;
            }
            set
            {
                vendorCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool PrimaryLoyaltyIndicator
        {
            get
            {
                return primaryLoyaltyIndicatorField;
            }
            set
            {
                primaryLoyaltyIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PrimaryLoyaltyIndicatorSpecified
        {
            get
            {
                return primaryLoyaltyIndicatorFieldSpecified;
            }
            set
            {
                primaryLoyaltyIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string AllianceLoyaltyLevelName
        {
            get
            {
                return allianceLoyaltyLevelNameField;
            }
            set
            {
                allianceLoyaltyLevelNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CustomerType
        {
            get
            {
                return customerTypeField;
            }
            set
            {
                customerTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CustomerValue
        {
            get
            {
                return customerValueField;
            }
            set
            {
                customerValueField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Password
        {
            get
            {
                return passwordField;
            }
            set
            {
                passwordField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum PaymentCardTypeCustLoyaltyShareSynchInd
    {

        /// <remarks/>
        Yes,

        /// <remarks/>
        No,

        /// <remarks/>
        Inherit
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum PaymentCardTypeCustLoyaltyShareMarketInd
    {

        /// <remarks/>
        Yes,

        /// <remarks/>
        No,

        /// <remarks/>
        Inherit
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum PaymentCardTypeCustLoyaltySingleVendorInd
    {

        /// <remarks/>
        SingleVndr,

        /// <remarks/>
        Alliance
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PaymentCardTypeSignatureOnFile
    {

        private bool signatureOnFileIndicatorField;

        private bool signatureOnFileIndicatorFieldSpecified;

        private DateTime effectiveDateField;

        private bool effectiveDateFieldSpecified;

        private DateTime expireDateField;

        private bool expireDateFieldSpecified;

        private bool expireDateExclusiveIndicatorField;

        private bool expireDateExclusiveIndicatorFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public bool SignatureOnFileIndicator
        {
            get
            {
                return signatureOnFileIndicatorField;
            }
            set
            {
                signatureOnFileIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool SignatureOnFileIndicatorSpecified
        {
            get
            {
                return signatureOnFileIndicatorFieldSpecified;
            }
            set
            {
                signatureOnFileIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDateField;
            }
            set
            {
                effectiveDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified
        {
            get
            {
                return effectiveDateFieldSpecified;
            }
            set
            {
                effectiveDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate
        {
            get
            {
                return expireDateField;
            }
            set
            {
                expireDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified
        {
            get
            {
                return expireDateFieldSpecified;
            }
            set
            {
                expireDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator
        {
            get
            {
                return expireDateExclusiveIndicatorField;
            }
            set
            {
                expireDateExclusiveIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified
        {
            get
            {
                return expireDateExclusiveIndicatorFieldSpecified;
            }
            set
            {
                expireDateExclusiveIndicatorFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PaymentCardTypeMagneticStripe
    {

        private byte[] track1Field;

        private byte[] track2Field;

        private byte[] track3Field;

        /// <remarks/>
        [XmlAttribute(DataType = "base64Binary")]
        public byte[] Track1
        {
            get
            {
                return track1Field;
            }
            set
            {
                track1Field = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "base64Binary")]
        public byte[] Track2
        {
            get
            {
                return track2Field;
            }
            set
            {
                track2Field = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "base64Binary")]
        public byte[] Track3
        {
            get
            {
                return track3Field;
            }
            set
            {
                track3Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PaymentFormTypeTicket
    {

        private PaymentFormTypeTicketConjunctionTicketNbr[] conjunctionTicketNbrField;

        private string ticketNumberField;

        private string originalTicketNumberField;

        private string originalIssuePlaceField;

        private DateTime originalIssueDateField;

        private bool originalIssueDateFieldSpecified;

        private string originalIssueIATAField;

        private string originalPaymentFormField;

        private PaymentFormTypeMiscChargeOrderCheckInhibitorType checkInhibitorTypeField;

        private bool checkInhibitorTypeFieldSpecified;

        private string[] couponRPHsField;

        private PaymentFormTypeTicketReroutingType reroutingTypeField;

        private bool reroutingTypeFieldSpecified;

        private string reasonForRerouteField;

        /// <remarks/>
        [XmlElement("ConjunctionTicketNbr")]
        public PaymentFormTypeTicketConjunctionTicketNbr[] ConjunctionTicketNbr
        {
            get
            {
                return conjunctionTicketNbrField;
            }
            set
            {
                conjunctionTicketNbrField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TicketNumber
        {
            get
            {
                return ticketNumberField;
            }
            set
            {
                ticketNumberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string OriginalTicketNumber
        {
            get
            {
                return originalTicketNumberField;
            }
            set
            {
                originalTicketNumberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string OriginalIssuePlace
        {
            get
            {
                return originalIssuePlaceField;
            }
            set
            {
                originalIssuePlaceField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime OriginalIssueDate
        {
            get
            {
                return originalIssueDateField;
            }
            set
            {
                originalIssueDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool OriginalIssueDateSpecified
        {
            get
            {
                return originalIssueDateFieldSpecified;
            }
            set
            {
                originalIssueDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string OriginalIssueIATA
        {
            get
            {
                return originalIssueIATAField;
            }
            set
            {
                originalIssueIATAField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string OriginalPaymentForm
        {
            get
            {
                return originalPaymentFormField;
            }
            set
            {
                originalPaymentFormField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PaymentFormTypeMiscChargeOrderCheckInhibitorType CheckInhibitorType
        {
            get
            {
                return checkInhibitorTypeField;
            }
            set
            {
                checkInhibitorTypeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool CheckInhibitorTypeSpecified
        {
            get
            {
                return checkInhibitorTypeFieldSpecified;
            }
            set
            {
                checkInhibitorTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string[] CouponRPHs
        {
            get
            {
                return couponRPHsField;
            }
            set
            {
                couponRPHsField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PaymentFormTypeTicketReroutingType ReroutingType
        {
            get
            {
                return reroutingTypeField;
            }
            set
            {
                reroutingTypeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ReroutingTypeSpecified
        {
            get
            {
                return reroutingTypeFieldSpecified;
            }
            set
            {
                reroutingTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ReasonForReroute
        {
            get
            {
                return reasonForRerouteField;
            }
            set
            {
                reasonForRerouteField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PaymentFormTypeTicketConjunctionTicketNbr
    {

        private string[] couponsField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string[] Coupons
        {
            get
            {
                return couponsField;
            }
            set
            {
                couponsField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum PaymentFormTypeTicketReroutingType
    {

        /// <remarks/>
        voluntary,

        /// <remarks/>
        involuntary
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PaymentFormTypeVoucher
    {

        private DateTime effectiveDateField;

        private bool effectiveDateFieldSpecified;

        private DateTime expireDateField;

        private bool expireDateFieldSpecified;

        private bool expireDateExclusiveIndicatorField;

        private bool expireDateExclusiveIndicatorFieldSpecified;

        private string seriesCodeField;

        private string billingNumberField;

        private string supplierIdentifierField;

        private string identifierField;

        private string valueTypeField;

        private bool electronicIndicatorField;

        private bool electronicIndicatorFieldSpecified;

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDateField;
            }
            set
            {
                effectiveDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified
        {
            get
            {
                return effectiveDateFieldSpecified;
            }
            set
            {
                effectiveDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate
        {
            get
            {
                return expireDateField;
            }
            set
            {
                expireDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified
        {
            get
            {
                return expireDateFieldSpecified;
            }
            set
            {
                expireDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator
        {
            get
            {
                return expireDateExclusiveIndicatorField;
            }
            set
            {
                expireDateExclusiveIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified
        {
            get
            {
                return expireDateExclusiveIndicatorFieldSpecified;
            }
            set
            {
                expireDateExclusiveIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string SeriesCode
        {
            get
            {
                return seriesCodeField;
            }
            set
            {
                seriesCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string BillingNumber
        {
            get
            {
                return billingNumberField;
            }
            set
            {
                billingNumberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string SupplierIdentifier
        {
            get
            {
                return supplierIdentifierField;
            }
            set
            {
                supplierIdentifierField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Identifier
        {
            get
            {
                return identifierField;
            }
            set
            {
                identifierField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ValueType
        {
            get
            {
                return valueTypeField;
            }
            set
            {
                valueTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ElectronicIndicator
        {
            get
            {
                return electronicIndicatorField;
            }
            set
            {
                electronicIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ElectronicIndicatorSpecified
        {
            get
            {
                return electronicIndicatorFieldSpecified;
            }
            set
            {
                electronicIndicatorFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum PaymentFormTypePaymentTransactionTypeCode
    {

        /// <remarks/>
        charge,

        /// <remarks/>
        reserve,

        /// <remarks/>
        refund
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PaymentResponseType : PaymentFormType
    {

        private PaymentResponseTypePaymentAmount paymentAmountField;

        private UniqueID_Type paymentReferenceIDField;

        private ErrorType1 errorField;

        /// <remarks/>
        public PaymentResponseTypePaymentAmount PaymentAmount
        {
            get
            {
                return paymentAmountField;
            }
            set
            {
                paymentAmountField = value;
            }
        }

        /// <remarks/>
        public UniqueID_Type PaymentReferenceID
        {
            get
            {
                return paymentReferenceIDField;
            }
            set
            {
                paymentReferenceIDField = value;
            }
        }

        /// <remarks/>
        public ErrorType1 Error
        {
            get
            {
                return errorField;
            }
            set
            {
                errorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PaymentResponseTypePaymentAmount
    {

        private decimal amountField;

        private bool amountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        private string approvalCodeField;

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified
        {
            get
            {
                return amountFieldSpecified;
            }
            set
            {
                amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ApprovalCode
        {
            get
            {
                return approvalCodeField;
            }
            set
            {
                approvalCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ErrorType1 : FreeTextType
    {

        /// <remarks/>
        [XmlAttribute()]
        public string Type { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string ShortText { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Code { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string DocURL { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Status { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Tag { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string RecordID { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string NodeList { get; set; }
    }

    /// <remarks/>
    [XmlInclude(typeof(CommissionInfoType))]
    [XmlInclude(typeof(CertificationType))]
    [XmlInclude(typeof(WarningType1))]
    [XmlInclude(typeof(ErrorType1))]
    [XmlInclude(typeof(ErrorType))]
    [XmlInclude(typeof(WarningType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class FreeTextType
    {

        /// <remarks/>
        [XmlAttribute(DataType = "language")]
        public string Language { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CommissionInfoType : FreeTextType
    {


        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareSynchInd ShareSynchInd { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareMarketInd ShareMarketIndAs { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string CommissionPlanCode { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CertificationType : FreeTextType
    {

        /// <remarks/>
        [XmlAttribute()]
        public string ID { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public CertificationTypeSingleVendorInd SingleVendorInd { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool SingleVendorIndSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum CertificationTypeSingleVendorInd
    {

        /// <remarks/>
        SingleVndr,

        /// <remarks/>
        Alliance
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class WarningType1 : FreeTextType
    {

        /// <remarks/>
        [XmlAttribute()]
        public string Type { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string ShortText { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Code { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string DocURL { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Status { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Tag { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string RecordID { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class WarningType : FreeTextType
    {

        /// <remarks/>
        [XmlAttribute()]
        public string Type { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string ShortText { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Code { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string DocURL { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Status { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Tag { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string RecordID { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class HotelPaymentFormType : PaymentFormType
    {

        private HotelPaymentFormTypeMasterAccountUsage masterAccountUsageField;

        /// <remarks/>
        public HotelPaymentFormTypeMasterAccountUsage MasterAccountUsage
        {
            get
            {
                return masterAccountUsageField;
            }
            set
            {
                masterAccountUsageField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class HotelPaymentFormTypeMasterAccountUsage
    {

        private HotelPaymentFormTypeMasterAccountUsageBillingType billingTypeField;

        private bool billingTypeFieldSpecified;

        private bool signFoodAndBevField;

        private bool signFoodAndBevFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public HotelPaymentFormTypeMasterAccountUsageBillingType BillingType
        {
            get
            {
                return billingTypeField;
            }
            set
            {
                billingTypeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool BillingTypeSpecified
        {
            get
            {
                return billingTypeFieldSpecified;
            }
            set
            {
                billingTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool SignFoodAndBev
        {
            get
            {
                return signFoodAndBevField;
            }
            set
            {
                signFoodAndBevField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool SignFoodAndBevSpecified
        {
            get
            {
                return signFoodAndBevFieldSpecified;
            }
            set
            {
                signFoodAndBevFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum HotelPaymentFormTypeMasterAccountUsageBillingType
    {

        /// <remarks/>
        EachPaysOwn,

        /// <remarks/>
        SignRoomAndTax,

        /// <remarks/>
        SignAllCharges,

        /// <remarks/>
        SignRoomOnly
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PaymentDetailType : PaymentFormType
    {

        private PaymentDetailTypePaymentAmount[] paymentAmountField;

        private CommissionType commissionField;

        private string paymentTypeField;

        private bool splitPaymentIndField;

        private bool splitPaymentIndFieldSpecified;

        private string authorizedDaysField;

        private bool primaryPaymentIndField;

        private bool primaryPaymentIndFieldSpecified;

        /// <remarks/>
        [XmlElement("PaymentAmount")]
        public PaymentDetailTypePaymentAmount[] PaymentAmount
        {
            get
            {
                return paymentAmountField;
            }
            set
            {
                paymentAmountField = value;
            }
        }

        /// <remarks/>
        public CommissionType Commission
        {
            get
            {
                return commissionField;
            }
            set
            {
                commissionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PaymentType
        {
            get
            {
                return paymentTypeField;
            }
            set
            {
                paymentTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool SplitPaymentInd
        {
            get
            {
                return splitPaymentIndField;
            }
            set
            {
                splitPaymentIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool SplitPaymentIndSpecified
        {
            get
            {
                return splitPaymentIndFieldSpecified;
            }
            set
            {
                splitPaymentIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string AuthorizedDays
        {
            get
            {
                return authorizedDaysField;
            }
            set
            {
                authorizedDaysField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool PrimaryPaymentInd
        {
            get
            {
                return primaryPaymentIndField;
            }
            set
            {
                primaryPaymentIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PrimaryPaymentIndSpecified
        {
            get
            {
                return primaryPaymentIndFieldSpecified;
            }
            set
            {
                primaryPaymentIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PaymentDetailTypePaymentAmount
    {

        private decimal amountField;

        private bool amountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        private string approvalCodeField;

        private PaymentDetailTypePaymentAmountRefundCalcMethod refundCalcMethodField;

        private bool refundCalcMethodFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified
        {
            get
            {
                return amountFieldSpecified;
            }
            set
            {
                amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ApprovalCode
        {
            get
            {
                return approvalCodeField;
            }
            set
            {
                approvalCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PaymentDetailTypePaymentAmountRefundCalcMethod RefundCalcMethod
        {
            get
            {
                return refundCalcMethodField;
            }
            set
            {
                refundCalcMethodField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool RefundCalcMethodSpecified
        {
            get
            {
                return refundCalcMethodFieldSpecified;
            }
            set
            {
                refundCalcMethodFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum PaymentDetailTypePaymentAmountRefundCalcMethod
    {

        /// <remarks/>
        System,

        /// <remarks/>
        Manual
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CommissionType
    {

        private UniqueID_Type uniqueIDField;

        private CommissionTypeCommissionableAmount commissionableAmountField;

        private CommissionTypePrepaidAmount prepaidAmountField;

        private CommissionTypeFlatCommission flatCommissionField;

        private CommissionTypeCommissionPayableAmount commissionPayableAmountField;

        private ParagraphType commentField;

        private CommissionTypeStatusType statusTypeField;

        private bool statusTypeFieldSpecified;

        private decimal percentField;

        private bool percentFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        private string reasonCodeField;

        private string billToIDField;

        private string frequencyField;

        private long maxCommissionUnitAppliesField;

        private bool maxCommissionUnitAppliesFieldSpecified;

        private decimal capAmountField;

        private bool capAmountFieldSpecified;

        /// <remarks/>
        public UniqueID_Type UniqueID
        {
            get
            {
                return uniqueIDField;
            }
            set
            {
                uniqueIDField = value;
            }
        }

        /// <remarks/>
        public CommissionTypeCommissionableAmount CommissionableAmount
        {
            get
            {
                return commissionableAmountField;
            }
            set
            {
                commissionableAmountField = value;
            }
        }

        /// <remarks/>
        public CommissionTypePrepaidAmount PrepaidAmount
        {
            get
            {
                return prepaidAmountField;
            }
            set
            {
                prepaidAmountField = value;
            }
        }

        /// <remarks/>
        public CommissionTypeFlatCommission FlatCommission
        {
            get
            {
                return flatCommissionField;
            }
            set
            {
                flatCommissionField = value;
            }
        }

        /// <remarks/>
        public CommissionTypeCommissionPayableAmount CommissionPayableAmount
        {
            get
            {
                return commissionPayableAmountField;
            }
            set
            {
                commissionPayableAmountField = value;
            }
        }

        /// <remarks/>
        public ParagraphType Comment
        {
            get
            {
                return commentField;
            }
            set
            {
                commentField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public CommissionTypeStatusType StatusType
        {
            get
            {
                return statusTypeField;
            }
            set
            {
                statusTypeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StatusTypeSpecified
        {
            get
            {
                return statusTypeFieldSpecified;
            }
            set
            {
                statusTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Percent
        {
            get
            {
                return percentField;
            }
            set
            {
                percentField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PercentSpecified
        {
            get
            {
                return percentFieldSpecified;
            }
            set
            {
                percentFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ReasonCode
        {
            get
            {
                return reasonCodeField;
            }
            set
            {
                reasonCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string BillToID
        {
            get
            {
                return billToIDField;
            }
            set
            {
                billToIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Frequency
        {
            get
            {
                return frequencyField;
            }
            set
            {
                frequencyField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long MaxCommissionUnitApplies
        {
            get
            {
                return maxCommissionUnitAppliesField;
            }
            set
            {
                maxCommissionUnitAppliesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MaxCommissionUnitAppliesSpecified
        {
            get
            {
                return maxCommissionUnitAppliesFieldSpecified;
            }
            set
            {
                maxCommissionUnitAppliesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal CapAmount
        {
            get
            {
                return capAmountField;
            }
            set
            {
                capAmountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool CapAmountSpecified
        {
            get
            {
                return capAmountFieldSpecified;
            }
            set
            {
                capAmountFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CommissionTypeCommissionableAmount
    {

        private decimal amountField;

        private bool amountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        private bool taxInclusiveIndicatorField;

        private bool taxInclusiveIndicatorFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified
        {
            get
            {
                return amountFieldSpecified;
            }
            set
            {
                amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool TaxInclusiveIndicator
        {
            get
            {
                return taxInclusiveIndicatorField;
            }
            set
            {
                taxInclusiveIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TaxInclusiveIndicatorSpecified
        {
            get
            {
                return taxInclusiveIndicatorFieldSpecified;
            }
            set
            {
                taxInclusiveIndicatorFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CommissionTypePrepaidAmount
    {

        private decimal amountField;

        private bool amountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified
        {
            get
            {
                return amountFieldSpecified;
            }
            set
            {
                amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CommissionTypeFlatCommission
    {

        private decimal amountField;

        private bool amountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified
        {
            get
            {
                return amountFieldSpecified;
            }
            set
            {
                amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CommissionTypeCommissionPayableAmount
    {

        private decimal amountField;

        private bool amountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified
        {
            get
            {
                return amountFieldSpecified;
            }
            set
            {
                amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [XmlInclude(typeof(DescriptionType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class ParagraphType
    {

        private object[] itemsField;

        private ItemsChoiceType[] itemsElementNameField;

        private string nameField;

        private long paragraphNumberField;

        private bool paragraphNumberFieldSpecified;

        private DateTime createDateTimeField;

        private bool createDateTimeFieldSpecified;

        private string creatorIDField;

        private DateTime lastModifyDateTimeField;

        private bool lastModifyDateTimeFieldSpecified;

        private string lastModifierIDField;

        private DateTime purgeDateField;

        private bool purgeDateFieldSpecified;

        private string languageField;

        /// <remarks/>
        [XmlElement("Image", typeof(string))]
        [XmlElement("ListItem", typeof(ParagraphTypeListItem))]
        [XmlElement("Text", typeof(FormattedTextTextType))]
        [XmlElement("URL", typeof(string), DataType = "anyURI")]
        [XmlChoiceIdentifier("ItemsElementName")]
        public object[] Items
        {
            get
            {
                return itemsField;
            }
            set
            {
                itemsField = value;
            }
        }

        /// <remarks/>
        [XmlElement("ItemsElementName")]
        [XmlIgnore()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return itemsElementNameField;
            }
            set
            {
                itemsElementNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long ParagraphNumber
        {
            get
            {
                return paragraphNumberField;
            }
            set
            {
                paragraphNumberField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ParagraphNumberSpecified
        {
            get
            {
                return paragraphNumberFieldSpecified;
            }
            set
            {
                paragraphNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DateTime CreateDateTime
        {
            get
            {
                return createDateTimeField;
            }
            set
            {
                createDateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool CreateDateTimeSpecified
        {
            get
            {
                return createDateTimeFieldSpecified;
            }
            set
            {
                createDateTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CreatorID
        {
            get
            {
                return creatorIDField;
            }
            set
            {
                creatorIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DateTime LastModifyDateTime
        {
            get
            {
                return lastModifyDateTimeField;
            }
            set
            {
                lastModifyDateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool LastModifyDateTimeSpecified
        {
            get
            {
                return lastModifyDateTimeFieldSpecified;
            }
            set
            {
                lastModifyDateTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string LastModifierID
        {
            get
            {
                return lastModifierIDField;
            }
            set
            {
                lastModifierIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime PurgeDate
        {
            get
            {
                return purgeDateField;
            }
            set
            {
                purgeDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PurgeDateSpecified
        {
            get
            {
                return purgeDateFieldSpecified;
            }
            set
            {
                purgeDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "language")]
        public string Language
        {
            get
            {
                return languageField;
            }
            set
            {
                languageField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class ParagraphTypeListItem : FormattedTextTextType
    {

        private long listItemField;

        private bool listItemFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public long ListItem
        {
            get
            {
                return listItemField;
            }
            set
            {
                listItemField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ListItemSpecified
        {
            get
            {
                return listItemFieldSpecified;
            }
            set
            {
                listItemFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [XmlInclude(typeof(PkgCautionType))]
    [XmlInclude(typeof(CoverageDetailsType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class FormattedTextTextType
    {

        private bool formattedField;

        private bool formattedFieldSpecified;

        private string languageField;

        private FormattedTextTextTypeTextFormat textFormatField;

        private bool textFormatFieldSpecified;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public bool Formatted
        {
            get
            {
                return formattedField;
            }
            set
            {
                formattedField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool FormattedSpecified
        {
            get
            {
                return formattedFieldSpecified;
            }
            set
            {
                formattedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "language")]
        public string Language
        {
            get
            {
                return languageField;
            }
            set
            {
                languageField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public FormattedTextTextTypeTextFormat TextFormat
        {
            get
            {
                return textFormatField;
            }
            set
            {
                textFormatField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TextFormatSpecified
        {
            get
            {
                return textFormatFieldSpecified;
            }
            set
            {
                textFormatFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum FormattedTextTextTypeTextFormat
    {

        /// <remarks/>
        PlainText,

        /// <remarks/>
        HTML
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PkgCautionType : FormattedTextTextType
    {

        private string startField;

        private string durationField;

        private string endField;

        private string typeField;

        private string idField;

        private string[] listOfItineraryItemRPHField;

        private string[] listOfExtraRPHField;

        /// <remarks/>
        [XmlAttribute()]
        public string Start
        {
            get
            {
                return startField;
            }
            set
            {
                startField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Duration
        {
            get
            {
                return durationField;
            }
            set
            {
                durationField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string End
        {
            get
            {
                return endField;
            }
            set
            {
                endField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ID
        {
            get
            {
                return idField;
            }
            set
            {
                idField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string[] ListOfItineraryItemRPH
        {
            get
            {
                return listOfItineraryItemRPHField;
            }
            set
            {
                listOfItineraryItemRPHField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string[] ListOfExtraRPH
        {
            get
            {
                return listOfExtraRPHField;
            }
            set
            {
                listOfExtraRPHField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CoverageDetailsType : FormattedTextTextType
    {

        private CoverageTextType coverageTextTypeField;

        /// <remarks/>
        [XmlAttribute()]
        public CoverageTextType CoverageTextType
        {
            get
            {
                return coverageTextTypeField;
            }
            set
            {
                coverageTextTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum CoverageTextType
    {

        /// <remarks/>
        Supplement,

        /// <remarks/>
        Description,

        /// <remarks/>
        Limits
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A", IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        /// <remarks/>
        Image,

        /// <remarks/>
        ListItem,

        /// <remarks/>
        Text,

        /// <remarks/>
        URL
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class DescriptionType : ParagraphType
    {

        private bool locationField;

        private bool locationFieldSpecified;

        private bool refDirectionToField;

        private bool refDirectionToFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public bool Location
        {
            get
            {
                return locationField;
            }
            set
            {
                locationField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool LocationSpecified
        {
            get
            {
                return locationFieldSpecified;
            }
            set
            {
                locationFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool RefDirectionTo
        {
            get
            {
                return refDirectionToField;
            }
            set
            {
                refDirectionToField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool RefDirectionToSpecified
        {
            get
            {
                return refDirectionToFieldSpecified;
            }
            set
            {
                refDirectionToFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum CommissionTypeStatusType
    {

        /// <remarks/>
        Full,

        /// <remarks/>
        Partial,

        /// <remarks/>
        [XmlEnum("Non-paying")]
        Nonpaying,

        /// <remarks/>
        [XmlEnum("No-show")]
        Noshow,

        /// <remarks/>
        Adjustment,

        /// <remarks/>
        Commissionable
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class RelatedTravelerType
    {

        private UniqueID_Type uniqueIDField;

        private PersonNameType personNameField;

        private PersonNameTypeShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private PersonNameTypeShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string relationField;

        private DateTime birthDateField;

        private bool birthDateFieldSpecified;

        /// <remarks/>
        public UniqueID_Type UniqueID
        {
            get
            {
                return uniqueIDField;
            }
            set
            {
                uniqueIDField = value;
            }
        }

        /// <remarks/>
        public PersonNameType PersonName
        {
            get
            {
                return personNameField;
            }
            set
            {
                personNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Relation
        {
            get
            {
                return relationField;
            }
            set
            {
                relationField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate
        {
            get
            {
                return birthDateField;
            }
            set
            {
                birthDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool BirthDateSpecified
        {
            get
            {
                return birthDateFieldSpecified;
            }
            set
            {
                birthDateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class ContactPersonType
    {

        private PersonNameType personNameField;

        private ContactPersonTypeTelephone[] telephoneField;

        private AddressInfoType[] addressField;

        private EmailType[] emailField;

        private URL_Type[] uRLField;

        private CompanyNameType[] companyNameField;

        private EmployeeInfoType[] employeeInfoField;

        private PersonNameTypeShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private PersonNameTypeShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private bool defaultIndField;

        private bool defaultIndFieldSpecified;

        private string contactTypeField;

        private string relationField;

        private bool emergencyFlagField;

        private bool emergencyFlagFieldSpecified;

        private string rPHField;

        private string communicationMethodCodeField;

        private string documentDistribMethodCodeField;

        /// <remarks/>
        public PersonNameType PersonName
        {
            get
            {
                return personNameField;
            }
            set
            {
                personNameField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Telephone")]
        public ContactPersonTypeTelephone[] Telephone
        {
            get
            {
                return telephoneField;
            }
            set
            {
                telephoneField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Address")]
        public AddressInfoType[] Address
        {
            get
            {
                return addressField;
            }
            set
            {
                addressField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Email")]
        public EmailType[] Email
        {
            get
            {
                return emailField;
            }
            set
            {
                emailField = value;
            }
        }

        /// <remarks/>
        [XmlElement("URL")]
        public URL_Type[] URL
        {
            get
            {
                return uRLField;
            }
            set
            {
                uRLField = value;
            }
        }

        /// <remarks/>
        [XmlElement("CompanyNameFull")]
        public CompanyNameType[] CompanyName
        {
            get
            {
                return companyNameField;
            }
            set
            {
                companyNameField = value;
            }
        }

        /// <remarks/>
        [XmlElement("EmployeeInfo")]
        public EmployeeInfoType[] EmployeeInfo
        {
            get
            {
                return employeeInfoField;
            }
            set
            {
                employeeInfoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool DefaultInd
        {
            get
            {
                return defaultIndField;
            }
            set
            {
                defaultIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified
        {
            get
            {
                return defaultIndFieldSpecified;
            }
            set
            {
                defaultIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ContactType
        {
            get
            {
                return contactTypeField;
            }
            set
            {
                contactTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Relation
        {
            get
            {
                return relationField;
            }
            set
            {
                relationField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool EmergencyFlag
        {
            get
            {
                return emergencyFlagField;
            }
            set
            {
                emergencyFlagField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool EmergencyFlagSpecified
        {
            get
            {
                return emergencyFlagFieldSpecified;
            }
            set
            {
                emergencyFlagFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH
        {
            get
            {
                return rPHField;
            }
            set
            {
                rPHField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CommunicationMethodCode
        {
            get
            {
                return communicationMethodCodeField;
            }
            set
            {
                communicationMethodCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DocumentDistribMethodCode
        {
            get
            {
                return documentDistribMethodCodeField;
            }
            set
            {
                documentDistribMethodCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class ContactPersonTypeTelephone
    {

        private CustomerTypeTelephoneShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private CustomerTypeTelephoneShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string phoneLocationTypeField;

        private string phoneTechTypeField;

        private string phoneUseTypeField;

        private string countryAccessCodeField;

        private string areaCityCodeField;

        private string phoneNumberField;

        private string extensionField;

        private string pINField;

        private string remarkField;

        private bool formattedIndField;

        private bool formattedIndFieldSpecified;

        private bool defaultIndField;

        private bool defaultIndFieldSpecified;

        private string rPHField;

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeTelephoneShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeTelephoneShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneLocationType
        {
            get
            {
                return phoneLocationTypeField;
            }
            set
            {
                phoneLocationTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneTechType
        {
            get
            {
                return phoneTechTypeField;
            }
            set
            {
                phoneTechTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneUseType
        {
            get
            {
                return phoneUseTypeField;
            }
            set
            {
                phoneUseTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CountryAccessCode
        {
            get
            {
                return countryAccessCodeField;
            }
            set
            {
                countryAccessCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string AreaCityCode
        {
            get
            {
                return areaCityCodeField;
            }
            set
            {
                areaCityCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneNumber
        {
            get
            {
                return phoneNumberField;
            }
            set
            {
                phoneNumberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Extension
        {
            get
            {
                return extensionField;
            }
            set
            {
                extensionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PIN
        {
            get
            {
                return pINField;
            }
            set
            {
                pINField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Remark
        {
            get
            {
                return remarkField;
            }
            set
            {
                remarkField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool FormattedInd
        {
            get
            {
                return formattedIndField;
            }
            set
            {
                formattedIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool FormattedIndSpecified
        {
            get
            {
                return formattedIndFieldSpecified;
            }
            set
            {
                formattedIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool DefaultInd
        {
            get
            {
                return defaultIndField;
            }
            set
            {
                defaultIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified
        {
            get
            {
                return defaultIndFieldSpecified;
            }
            set
            {
                defaultIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH
        {
            get
            {
                return rPHField;
            }
            set
            {
                rPHField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class EmployeeInfoType
    {

        private string employeeIdField;

        private string employeeLevelField;

        private string employeeTitleField;

        private string employeeStatusField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string EmployeeId
        {
            get
            {
                return employeeIdField;
            }
            set
            {
                employeeIdField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string EmployeeLevel
        {
            get
            {
                return employeeLevelField;
            }
            set
            {
                employeeLevelField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string EmployeeTitle
        {
            get
            {
                return employeeTitleField;
            }
            set
            {
                employeeTitleField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string EmployeeStatus
        {
            get
            {
                return employeeStatusField;
            }
            set
            {
                employeeStatusField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class DocumentType
    {

        private object itemField;

        private string[] docLimitationsField;

        private string[] additionalPersonNamesField;

        private PersonNameTypeShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private PersonNameTypeShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string docIssueAuthorityField;

        private string docIssueLocationField;

        private string docIDField;

        private string docTypeField;

        private DocumentTypeGender genderField;

        private bool genderFieldSpecified;

        private DateTime birthDateField;

        private bool birthDateFieldSpecified;

        private DateTime effectiveDateField;

        private bool effectiveDateFieldSpecified;

        private DateTime expireDateField;

        private bool expireDateFieldSpecified;

        private bool expireDateExclusiveIndicatorField;

        private bool expireDateExclusiveIndicatorFieldSpecified;

        private string docIssueStateProvField;

        private string docIssueCountryField;

        private string birthCountryField;

        private string birthPlaceField;

        private string docHolderNationalityField;

        private string contactNameField;

        private DocumentTypeHolderType holderTypeField;

        private bool holderTypeFieldSpecified;

        private string remarkField;

        private string postalCodeField;

        /// <remarks/>
        [XmlElement("DocHolderFormattedName", typeof(PersonNameType))]
        [XmlElement("DocHolderName", typeof(string))]
        public object Item
        {
            get
            {
                return itemField;
            }
            set
            {
                itemField = value;
            }
        }

        /// <remarks/>
        [XmlElement("DocLimitations")]
        public string[] DocLimitations
        {
            get
            {
                return docLimitationsField;
            }
            set
            {
                docLimitationsField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("AdditionalPersonName", IsNullable = false)]
        public string[] AdditionalPersonNames
        {
            get
            {
                return additionalPersonNamesField;
            }
            set
            {
                additionalPersonNamesField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PersonNameTypeShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DocIssueAuthority
        {
            get
            {
                return docIssueAuthorityField;
            }
            set
            {
                docIssueAuthorityField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DocIssueLocation
        {
            get
            {
                return docIssueLocationField;
            }
            set
            {
                docIssueLocationField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DocID
        {
            get
            {
                return docIDField;
            }
            set
            {
                docIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DocType
        {
            get
            {
                return docTypeField;
            }
            set
            {
                docTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DocumentTypeGender Gender
        {
            get
            {
                return genderField;
            }
            set
            {
                genderField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool GenderSpecified
        {
            get
            {
                return genderFieldSpecified;
            }
            set
            {
                genderFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate
        {
            get
            {
                return birthDateField;
            }
            set
            {
                birthDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool BirthDateSpecified
        {
            get
            {
                return birthDateFieldSpecified;
            }
            set
            {
                birthDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDateField;
            }
            set
            {
                effectiveDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified
        {
            get
            {
                return effectiveDateFieldSpecified;
            }
            set
            {
                effectiveDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate
        {
            get
            {
                return expireDateField;
            }
            set
            {
                expireDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified
        {
            get
            {
                return expireDateFieldSpecified;
            }
            set
            {
                expireDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator
        {
            get
            {
                return expireDateExclusiveIndicatorField;
            }
            set
            {
                expireDateExclusiveIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified
        {
            get
            {
                return expireDateExclusiveIndicatorFieldSpecified;
            }
            set
            {
                expireDateExclusiveIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DocIssueStateProv
        {
            get
            {
                return docIssueStateProvField;
            }
            set
            {
                docIssueStateProvField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DocIssueCountry
        {
            get
            {
                return docIssueCountryField;
            }
            set
            {
                docIssueCountryField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string BirthCountry
        {
            get
            {
                return birthCountryField;
            }
            set
            {
                birthCountryField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string BirthPlace
        {
            get
            {
                return birthPlaceField;
            }
            set
            {
                birthPlaceField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DocHolderNationality
        {
            get
            {
                return docHolderNationalityField;
            }
            set
            {
                docHolderNationalityField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ContactName
        {
            get
            {
                return contactNameField;
            }
            set
            {
                contactNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DocumentTypeHolderType HolderType
        {
            get
            {
                return holderTypeField;
            }
            set
            {
                holderTypeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool HolderTypeSpecified
        {
            get
            {
                return holderTypeFieldSpecified;
            }
            set
            {
                holderTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Remark
        {
            get
            {
                return remarkField;
            }
            set
            {
                remarkField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PostalCode
        {
            get
            {
                return postalCodeField;
            }
            set
            {
                postalCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum DocumentTypeGender
    {

        /// <remarks/>
        Male,

        /// <remarks/>
        Female,

        /// <remarks/>
        Unknown,

        /// <remarks/>
        Male_NoShare,

        /// <remarks/>
        Female_NoShare
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum DocumentTypeHolderType
    {

        /// <remarks/>
        Infant,

        /// <remarks/>
        HeadOfHousehold
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypeCustLoyalty
    {

        private CustomerTypeCustLoyaltyMemberPreferences memberPreferencesField;

        private CustomerTypeCustLoyaltySecurityInfo securityInfoField;

        private CustomerTypeCustLoyaltySubAccountBalance[] subAccountBalanceField;

        private PaymentCardTypeCustLoyaltyShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private PaymentCardTypeCustLoyaltyShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string programIDField;

        private string membershipIDField;

        private string travelSectorField;

        private string loyalLevelField;

        private string loyalLevelCodeField;

        private PaymentCardTypeCustLoyaltySingleVendorInd singleVendorIndField;

        private bool singleVendorIndFieldSpecified;

        private DateTime signupDateField;

        private bool signupDateFieldSpecified;

        private DateTime effectiveDateField;

        private bool effectiveDateFieldSpecified;

        private DateTime expireDateField;

        private bool expireDateFieldSpecified;

        private bool expireDateExclusiveIndicatorField;

        private bool expireDateExclusiveIndicatorFieldSpecified;

        private string rPHField;

        private string[] vendorCodeField;

        private bool primaryLoyaltyIndicatorField;

        private bool primaryLoyaltyIndicatorFieldSpecified;

        private string allianceLoyaltyLevelNameField;

        private string customerTypeField;

        private string customerValueField;

        private string passwordField;

        private string remarkField;

        /// <remarks/>
        public CustomerTypeCustLoyaltyMemberPreferences MemberPreferences
        {
            get
            {
                return memberPreferencesField;
            }
            set
            {
                memberPreferencesField = value;
            }
        }

        /// <remarks/>
        public CustomerTypeCustLoyaltySecurityInfo SecurityInfo
        {
            get
            {
                return securityInfoField;
            }
            set
            {
                securityInfoField = value;
            }
        }

        /// <remarks/>
        [XmlElement("SubAccountBalance")]
        public CustomerTypeCustLoyaltySubAccountBalance[] SubAccountBalance
        {
            get
            {
                return subAccountBalanceField;
            }
            set
            {
                subAccountBalanceField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PaymentCardTypeCustLoyaltyShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PaymentCardTypeCustLoyaltyShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ProgramID
        {
            get
            {
                return programIDField;
            }
            set
            {
                programIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string MembershipID
        {
            get
            {
                return membershipIDField;
            }
            set
            {
                membershipIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TravelSector
        {
            get
            {
                return travelSectorField;
            }
            set
            {
                travelSectorField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string LoyalLevel
        {
            get
            {
                return loyalLevelField;
            }
            set
            {
                loyalLevelField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string LoyalLevelCode
        {
            get
            {
                return loyalLevelCodeField;
            }
            set
            {
                loyalLevelCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PaymentCardTypeCustLoyaltySingleVendorInd SingleVendorInd
        {
            get
            {
                return singleVendorIndField;
            }
            set
            {
                singleVendorIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool SingleVendorIndSpecified
        {
            get
            {
                return singleVendorIndFieldSpecified;
            }
            set
            {
                singleVendorIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime SignupDate
        {
            get
            {
                return signupDateField;
            }
            set
            {
                signupDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool SignupDateSpecified
        {
            get
            {
                return signupDateFieldSpecified;
            }
            set
            {
                signupDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDateField;
            }
            set
            {
                effectiveDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified
        {
            get
            {
                return effectiveDateFieldSpecified;
            }
            set
            {
                effectiveDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate
        {
            get
            {
                return expireDateField;
            }
            set
            {
                expireDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified
        {
            get
            {
                return expireDateFieldSpecified;
            }
            set
            {
                expireDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator
        {
            get
            {
                return expireDateExclusiveIndicatorField;
            }
            set
            {
                expireDateExclusiveIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified
        {
            get
            {
                return expireDateExclusiveIndicatorFieldSpecified;
            }
            set
            {
                expireDateExclusiveIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH
        {
            get
            {
                return rPHField;
            }
            set
            {
                rPHField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string[] VendorCode
        {
            get
            {
                return vendorCodeField;
            }
            set
            {
                vendorCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool PrimaryLoyaltyIndicator
        {
            get
            {
                return primaryLoyaltyIndicatorField;
            }
            set
            {
                primaryLoyaltyIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PrimaryLoyaltyIndicatorSpecified
        {
            get
            {
                return primaryLoyaltyIndicatorFieldSpecified;
            }
            set
            {
                primaryLoyaltyIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string AllianceLoyaltyLevelName
        {
            get
            {
                return allianceLoyaltyLevelNameField;
            }
            set
            {
                allianceLoyaltyLevelNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CustomerType
        {
            get
            {
                return customerTypeField;
            }
            set
            {
                customerTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CustomerValue
        {
            get
            {
                return customerValueField;
            }
            set
            {
                customerValueField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Password
        {
            get
            {
                return passwordField;
            }
            set
            {
                passwordField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Remark
        {
            get
            {
                return remarkField;
            }
            set
            {
                remarkField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypeCustLoyaltyMemberPreferences
    {

        private CustomerTypeCustLoyaltyMemberPreferencesAdditionalReward[] additionalRewardField;

        private CustomerTypeCustLoyaltyMemberPreferencesOffer[] offerField;

        private string awarenessField;

        private string promotionCodeField;

        private string[] promotionVendorCodeField;

        private CustomerTypeCustLoyaltyMemberPreferencesAwardsPreference awardsPreferenceField;

        private bool awardsPreferenceFieldSpecified;

        /// <remarks/>
        [XmlElement("AdditionalReward")]
        public CustomerTypeCustLoyaltyMemberPreferencesAdditionalReward[] AdditionalReward
        {
            get
            {
                return additionalRewardField;
            }
            set
            {
                additionalRewardField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Offer")]
        public CustomerTypeCustLoyaltyMemberPreferencesOffer[] Offer
        {
            get
            {
                return offerField;
            }
            set
            {
                offerField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Awareness
        {
            get
            {
                return awarenessField;
            }
            set
            {
                awarenessField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PromotionCode
        {
            get
            {
                return promotionCodeField;
            }
            set
            {
                promotionCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string[] PromotionVendorCode
        {
            get
            {
                return promotionVendorCodeField;
            }
            set
            {
                promotionVendorCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeCustLoyaltyMemberPreferencesAwardsPreference AwardsPreference
        {
            get
            {
                return awardsPreferenceField;
            }
            set
            {
                awardsPreferenceField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AwardsPreferenceSpecified
        {
            get
            {
                return awardsPreferenceFieldSpecified;
            }
            set
            {
                awardsPreferenceFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypeCustLoyaltyMemberPreferencesAdditionalReward
    {

        private CompanyNameType companyNameField;

        private PersonNameType nameField;

        private string memberIDField;

        /// <remarks/>
        public CompanyNameType CompanyName
        {
            get
            {
                return companyNameField;
            }
            set
            {
                companyNameField = value;
            }
        }

        /// <remarks/>
        public PersonNameType Name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string MemberID
        {
            get
            {
                return memberIDField;
            }
            set
            {
                memberIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypeCustLoyaltyMemberPreferencesOffer
    {

        private CustomerTypeCustLoyaltyMemberPreferencesOfferCommunication[] communicationField;

        private CustomerTypeCustLoyaltyMemberPreferencesOfferType typeField;

        private bool typeFieldSpecified;

        /// <remarks/>
        [XmlElement("Communication")]
        public CustomerTypeCustLoyaltyMemberPreferencesOfferCommunication[] Communication
        {
            get
            {
                return communicationField;
            }
            set
            {
                communicationField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeCustLoyaltyMemberPreferencesOfferType Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TypeSpecified
        {
            get
            {
                return typeFieldSpecified;
            }
            set
            {
                typeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypeCustLoyaltyMemberPreferencesOfferCommunication
    {

        private string distribTypeField;

        /// <remarks/>
        [XmlAttribute()]
        public string DistribType
        {
            get
            {
                return distribTypeField;
            }
            set
            {
                distribTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum CustomerTypeCustLoyaltyMemberPreferencesOfferType
    {

        /// <remarks/>
        Partner,

        /// <remarks/>
        Loyalty
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum CustomerTypeCustLoyaltyMemberPreferencesAwardsPreference
    {

        /// <remarks/>
        Points,

        /// <remarks/>
        Miles
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypeCustLoyaltySecurityInfo
    {

        private CustomerTypeCustLoyaltySecurityInfoPasswordHint[] passwordHintField;

        private string usernameField;

        private string passwordField;

        /// <remarks/>
        [XmlElement("PasswordHint")]
        public CustomerTypeCustLoyaltySecurityInfoPasswordHint[] PasswordHint
        {
            get
            {
                return passwordHintField;
            }
            set
            {
                passwordHintField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Username
        {
            get
            {
                return usernameField;
            }
            set
            {
                usernameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Password
        {
            get
            {
                return passwordField;
            }
            set
            {
                passwordField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypeCustLoyaltySecurityInfoPasswordHint
    {

        private CustomerTypeCustLoyaltySecurityInfoPasswordHintHint hintField;

        private bool hintFieldSpecified;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeCustLoyaltySecurityInfoPasswordHintHint Hint
        {
            get
            {
                return hintField;
            }
            set
            {
                hintField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool HintSpecified
        {
            get
            {
                return hintFieldSpecified;
            }
            set
            {
                hintFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum CustomerTypeCustLoyaltySecurityInfoPasswordHintHint
    {

        /// <remarks/>
        Question,

        /// <remarks/>
        Answer
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypeCustLoyaltySubAccountBalance
    {

        private string typeField;

        private long balanceField;

        private bool balanceFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long Balance
        {
            get
            {
                return balanceField;
            }
            set
            {
                balanceField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool BalanceSpecified
        {
            get
            {
                return balanceFieldSpecified;
            }
            set
            {
                balanceFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerTypeAdditionalLanguage
    {

        private string codeField;

        /// <remarks/>
        [XmlAttribute(DataType = "language")]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum CustomerTypeMaritalStatus
    {

        /// <remarks/>
        Annulled,

        /// <remarks/>
        [XmlEnum("Co-habitating")]
        Cohabitating,

        /// <remarks/>
        Divorced,

        /// <remarks/>
        Engaged,

        /// <remarks/>
        Married,

        /// <remarks/>
        Separated,

        /// <remarks/>
        Single,

        /// <remarks/>
        Widowed,

        /// <remarks/>
        Unknown
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class InsuranceCustomerType : CustomerType
    {

        private string idField;

        /// <remarks/>
        [XmlAttribute()]
        public string ID
        {
            get
            {
                return idField;
            }
            set
            {
                idField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CustomerPrimaryAdditionalTypeAdditional : CustomerType
    {

        private string startField;

        private string durationField;

        private string endField;

        private string corpDiscountNameField;

        private string corpDiscountNmbrField;

        private CustomerPrimaryAdditionalTypeAdditionalQualificationMethod qualificationMethodField;

        private bool qualificationMethodFieldSpecified;

        private string ageField;

        private string codeField;

        private string codeContextField;

        private string uRIField;

        private long quantityField;

        private bool quantityFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public string Start
        {
            get
            {
                return startField;
            }
            set
            {
                startField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Duration
        {
            get
            {
                return durationField;
            }
            set
            {
                durationField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string End
        {
            get
            {
                return endField;
            }
            set
            {
                endField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CorpDiscountName
        {
            get
            {
                return corpDiscountNameField;
            }
            set
            {
                corpDiscountNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CorpDiscountNmbr
        {
            get
            {
                return corpDiscountNmbrField;
            }
            set
            {
                corpDiscountNmbrField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public CustomerPrimaryAdditionalTypeAdditionalQualificationMethod QualificationMethod
        {
            get
            {
                return qualificationMethodField;
            }
            set
            {
                qualificationMethodField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool QualificationMethodSpecified
        {
            get
            {
                return qualificationMethodFieldSpecified;
            }
            set
            {
                qualificationMethodFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string Age
        {
            get
            {
                return ageField;
            }
            set
            {
                ageField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CodeContext
        {
            get
            {
                return codeContextField;
            }
            set
            {
                codeContextField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string URI
        {
            get
            {
                return uRIField;
            }
            set
            {
                uRIField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long Quantity
        {
            get
            {
                return quantityField;
            }
            set
            {
                quantityField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool QuantitySpecified
        {
            get
            {
                return quantityFieldSpecified;
            }
            set
            {
                quantityFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum CustomerPrimaryAdditionalTypeAdditionalQualificationMethod
    {

        /// <remarks/>
        RT_AirlineTicket,

        /// <remarks/>
        CreditCard,

        /// <remarks/>
        PassportAndReturnTkt
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleReservationTypeVehSegmentCore : VehicleSegmentCoreType
    {

        private bool optionChangeAllowedIndicatorField;

        private bool optionChangeAllowedIndicatorFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public bool OptionChangeAllowedIndicator
        {
            get
            {
                return optionChangeAllowedIndicatorField;
            }
            set
            {
                optionChangeAllowedIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool OptionChangeAllowedIndicatorSpecified
        {
            get
            {
                return optionChangeAllowedIndicatorFieldSpecified;
            }
            set
            {
                optionChangeAllowedIndicatorFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleSegmentCoreType
    {

        private VehicleSegmentCoreTypeConfID[] confIDField;

        private CompanyNameType vendorField;

        private VehicleRentalCoreType vehRentalCoreField;

        private VehicleType vehicleField;

        private VehicleRentalRateType rentalRateField;

        private VehicleEquipmentPricedType[] pricedEquipsField;

        private VehicleChargePurposeType[] feesField;

        private VehicleSegmentCoreTypeTotalCharge totalChargeField;

        private TPA_ExtensionsType tPA_ExtensionsField;

        private string indexNumberField;

        /// <remarks/>
        [XmlElement("ConfID")]
        public VehicleSegmentCoreTypeConfID[] ConfID
        {
            get
            {
                return confIDField;
            }
            set
            {
                confIDField = value;
            }
        }

        /// <remarks/>
        public CompanyNameType Vendor
        {
            get
            {
                return vendorField;
            }
            set
            {
                vendorField = value;
            }
        }

        /// <remarks/>
        public VehicleRentalCoreType VehRentalCore
        {
            get
            {
                return vehRentalCoreField;
            }
            set
            {
                vehRentalCoreField = value;
            }
        }

        /// <remarks/>
        public VehicleType Vehicle
        {
            get
            {
                return vehicleField;
            }
            set
            {
                vehicleField = value;
            }
        }

        /// <remarks/>
        public VehicleRentalRateType RentalRate
        {
            get
            {
                return rentalRateField;
            }
            set
            {
                rentalRateField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("PricedEquip", IsNullable = false)]
        public VehicleEquipmentPricedType[] PricedEquips
        {
            get
            {
                return pricedEquipsField;
            }
            set
            {
                pricedEquipsField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("Fee", IsNullable = false)]
        public VehicleChargePurposeType[] Fees
        {
            get
            {
                return feesField;
            }
            set
            {
                feesField = value;
            }
        }

        /// <remarks/>
        public VehicleSegmentCoreTypeTotalCharge TotalCharge
        {
            get
            {
                return totalChargeField;
            }
            set
            {
                totalChargeField = value;
            }
        }

        /// <remarks/>
        public TPA_ExtensionsType TPA_Extensions
        {
            get
            {
                return tPA_ExtensionsField;
            }
            set
            {
                tPA_ExtensionsField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string IndexNumber
        {
            get
            {
                return indexNumberField;
            }
            set
            {
                indexNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleSegmentCoreTypeConfID : UniqueID_Type
    {

        private string statusField;

        /// <remarks/>
        [XmlAttribute()]
        public string Status
        {
            get
            {
                return statusField;
            }
            set
            {
                statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleRentalCoreType
    {

        private VehicleRentalCoreTypePickUpLocation[] pickUpLocationField;

        private VehicleRentalCoreTypeReturnLocation returnLocationField;

        private DateTime pickUpDateTimeField;

        private bool pickUpDateTimeFieldSpecified;

        private DateTime returnDateTimeField;

        private bool returnDateTimeFieldSpecified;

        private DateTime startChargesDateTimeField;

        private bool startChargesDateTimeFieldSpecified;

        private DateTime stopChargesDateTimeField;

        private bool stopChargesDateTimeFieldSpecified;

        private bool oneWayIndicatorField;

        private bool oneWayIndicatorFieldSpecified;

        private string multiIslandRentalDaysField;

        private long quantityField;

        private bool quantityFieldSpecified;

        private DistanceUnitNameType distUnitNameField;

        private bool distUnitNameFieldSpecified;

        /// <remarks/>
        [XmlElement("PickUpLocation")]
        public VehicleRentalCoreTypePickUpLocation[] PickUpLocation
        {
            get
            {
                return pickUpLocationField;
            }
            set
            {
                pickUpLocationField = value;
            }
        }

        /// <remarks/>
        public VehicleRentalCoreTypeReturnLocation ReturnLocation
        {
            get
            {
                return returnLocationField;
            }
            set
            {
                returnLocationField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DateTime PickUpDateTime
        {
            get
            {
                return pickUpDateTimeField;
            }
            set
            {
                pickUpDateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PickUpDateTimeSpecified
        {
            get
            {
                return pickUpDateTimeFieldSpecified;
            }
            set
            {
                pickUpDateTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DateTime ReturnDateTime
        {
            get
            {
                return returnDateTimeField;
            }
            set
            {
                returnDateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ReturnDateTimeSpecified
        {
            get
            {
                return returnDateTimeFieldSpecified;
            }
            set
            {
                returnDateTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DateTime StartChargesDateTime
        {
            get
            {
                return startChargesDateTimeField;
            }
            set
            {
                startChargesDateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StartChargesDateTimeSpecified
        {
            get
            {
                return startChargesDateTimeFieldSpecified;
            }
            set
            {
                startChargesDateTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DateTime StopChargesDateTime
        {
            get
            {
                return stopChargesDateTimeField;
            }
            set
            {
                stopChargesDateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StopChargesDateTimeSpecified
        {
            get
            {
                return stopChargesDateTimeFieldSpecified;
            }
            set
            {
                stopChargesDateTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool OneWayIndicator
        {
            get
            {
                return oneWayIndicatorField;
            }
            set
            {
                oneWayIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool OneWayIndicatorSpecified
        {
            get
            {
                return oneWayIndicatorFieldSpecified;
            }
            set
            {
                oneWayIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string MultiIslandRentalDays
        {
            get
            {
                return multiIslandRentalDaysField;
            }
            set
            {
                multiIslandRentalDaysField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long Quantity
        {
            get
            {
                return quantityField;
            }
            set
            {
                quantityField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool QuantitySpecified
        {
            get
            {
                return quantityFieldSpecified;
            }
            set
            {
                quantityFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DistanceUnitNameType DistUnitName
        {
            get
            {
                return distUnitNameField;
            }
            set
            {
                distUnitNameField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DistUnitNameSpecified
        {
            get
            {
                return distUnitNameFieldSpecified;
            }
            set
            {
                distUnitNameFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleRentalCoreTypePickUpLocation : LocationType
    {

        private string extendedLocationCodeField;

        private string counterLocationField;

        /// <remarks/>
        [XmlAttribute()]
        public string ExtendedLocationCode
        {
            get
            {
                return extendedLocationCodeField;
            }
            set
            {
                extendedLocationCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CounterLocation
        {
            get
            {
                return counterLocationField;
            }
            set
            {
                counterLocationField = value;
            }
        }
    }

    /// <remarks/>
    [XmlInclude(typeof(StationType))]
    [XmlInclude(typeof(AirportPrefType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class LocationType
    {

        private string locationCodeField;

        private string codeContextField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string LocationCode
        {
            get
            {
                return locationCodeField;
            }
            set
            {
                locationCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CodeContext
        {
            get
            {
                return codeContextField;
            }
            set
            {
                codeContextField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class StationType : LocationType
    {

        private bool isStaffedIndField;

        private bool isStaffedIndFieldSpecified;

        private bool ticketPrinterIndField;

        private bool ticketPrinterIndFieldSpecified;

        private bool sST_MachineIndField;

        private bool sST_MachineIndFieldSpecified;

        private string timeZoneOffsetField;

        /// <remarks/>
        [XmlAttribute()]
        public bool IsStaffedInd
        {
            get
            {
                return isStaffedIndField;
            }
            set
            {
                isStaffedIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool IsStaffedIndSpecified
        {
            get
            {
                return isStaffedIndFieldSpecified;
            }
            set
            {
                isStaffedIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool TicketPrinterInd
        {
            get
            {
                return ticketPrinterIndField;
            }
            set
            {
                ticketPrinterIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TicketPrinterIndSpecified
        {
            get
            {
                return ticketPrinterIndFieldSpecified;
            }
            set
            {
                ticketPrinterIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool SST_MachineInd
        {
            get
            {
                return sST_MachineIndField;
            }
            set
            {
                sST_MachineIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool SST_MachineIndSpecified
        {
            get
            {
                return sST_MachineIndFieldSpecified;
            }
            set
            {
                sST_MachineIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TimeZoneOffset
        {
            get
            {
                return timeZoneOffsetField;
            }
            set
            {
                timeZoneOffsetField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class AirportPrefType : LocationType
    {

        private PreferLevelType preferLevelField;

        private bool preferLevelFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel
        {
            get
            {
                return preferLevelField;
            }
            set
            {
                preferLevelField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified
        {
            get
            {
                return preferLevelFieldSpecified;
            }
            set
            {
                preferLevelFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleRentalCoreTypeReturnLocation : LocationType
    {

        private string extendedLocationCodeField;

        private string counterLocationField;

        /// <remarks/>
        [XmlAttribute()]
        public string ExtendedLocationCode
        {
            get
            {
                return extendedLocationCodeField;
            }
            set
            {
                extendedLocationCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CounterLocation
        {
            get
            {
                return counterLocationField;
            }
            set
            {
                counterLocationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum DistanceUnitNameType
    {

        /// <remarks/>
        Mile,

        /// <remarks/>
        Km,

        /// <remarks/>
        Block
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleType : VehicleCoreType
    {

        private VehicleTypeVehMakeModel vehMakeModelField;

        private string pictureURLField;

        private VehicleTypeVehIdentity vehIdentityField;

        private string passengerQuantityField;

        private long baggageQuantityField;

        private bool baggageQuantityFieldSpecified;

        private string vendorCarTypeField;

        private string codeField;

        private string codeContextField;

        private decimal unitOfMeasureQuantityField;

        private bool unitOfMeasureQuantityFieldSpecified;

        private string unitOfMeasureField;

        private string unitOfMeasureCodeField;

        private string startField;

        private string durationField;

        private string endField;

        private DistanceUnitNameType odometerUnitOfMeasureField;

        private bool odometerUnitOfMeasureFieldSpecified;

        private string descriptionField;

        /// <remarks/>
        public VehicleTypeVehMakeModel VehMakeModel
        {
            get
            {
                return vehMakeModelField;
            }
            set
            {
                vehMakeModelField = value;
            }
        }

        /// <remarks/>
        [XmlElement(DataType = "anyURI")]
        public string PictureURL
        {
            get
            {
                return pictureURLField;
            }
            set
            {
                pictureURLField = value;
            }
        }

        /// <remarks/>
        public VehicleTypeVehIdentity VehIdentity
        {
            get
            {
                return vehIdentityField;
            }
            set
            {
                vehIdentityField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PassengerQuantity
        {
            get
            {
                return passengerQuantityField;
            }
            set
            {
                passengerQuantityField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long BaggageQuantity
        {
            get
            {
                return baggageQuantityField;
            }
            set
            {
                baggageQuantityField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool BaggageQuantitySpecified
        {
            get
            {
                return baggageQuantityFieldSpecified;
            }
            set
            {
                baggageQuantityFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string VendorCarType
        {
            get
            {
                return vendorCarTypeField;
            }
            set
            {
                vendorCarTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CodeContext
        {
            get
            {
                return codeContextField;
            }
            set
            {
                codeContextField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal UnitOfMeasureQuantity
        {
            get
            {
                return unitOfMeasureQuantityField;
            }
            set
            {
                unitOfMeasureQuantityField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool UnitOfMeasureQuantitySpecified
        {
            get
            {
                return unitOfMeasureQuantityFieldSpecified;
            }
            set
            {
                unitOfMeasureQuantityFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string UnitOfMeasure
        {
            get
            {
                return unitOfMeasureField;
            }
            set
            {
                unitOfMeasureField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string UnitOfMeasureCode
        {
            get
            {
                return unitOfMeasureCodeField;
            }
            set
            {
                unitOfMeasureCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Start
        {
            get
            {
                return startField;
            }
            set
            {
                startField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Duration
        {
            get
            {
                return durationField;
            }
            set
            {
                durationField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string End
        {
            get
            {
                return endField;
            }
            set
            {
                endField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DistanceUnitNameType OdometerUnitOfMeasure
        {
            get
            {
                return odometerUnitOfMeasureField;
            }
            set
            {
                odometerUnitOfMeasureField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool OdometerUnitOfMeasureSpecified
        {
            get
            {
                return odometerUnitOfMeasureFieldSpecified;
            }
            set
            {
                odometerUnitOfMeasureFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleTypeVehMakeModel
    {

        private string nameField;

        private string codeField;

        private string modelYearField;

        /// <remarks/>
        [XmlAttribute()]
        public string Name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "gYear")]
        public string ModelYear
        {
            get
            {
                return modelYearField;
            }
            set
            {
                modelYearField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleTypeVehIdentity
    {

        private string vehicleAssetNumberField;

        private string licensePlateNumberField;

        private string stateProvCodeField;

        private string countryCodeField;

        private string vehicleID_NumberField;

        private string vehicleColorField;

        /// <remarks/>
        [XmlAttribute()]
        public string VehicleAssetNumber
        {
            get
            {
                return vehicleAssetNumberField;
            }
            set
            {
                vehicleAssetNumberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string LicensePlateNumber
        {
            get
            {
                return licensePlateNumberField;
            }
            set
            {
                licensePlateNumberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string StateProvCode
        {
            get
            {
                return stateProvCodeField;
            }
            set
            {
                stateProvCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CountryCode
        {
            get
            {
                return countryCodeField;
            }
            set
            {
                countryCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string VehicleID_Number
        {
            get
            {
                return vehicleID_NumberField;
            }
            set
            {
                vehicleID_NumberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string VehicleColor
        {
            get
            {
                return vehicleColorField;
            }
            set
            {
                vehicleColorField = value;
            }
        }
    }

    /// <remarks/>
    [XmlInclude(typeof(VehicleType))]
    [XmlInclude(typeof(VehiclePrefType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleCoreType
    {

        private VehicleCoreTypeVehType vehTypeField;

        private VehicleCoreTypeVehClass vehClassField;

        private bool airConditionIndField;

        private bool airConditionIndFieldSpecified;

        private VehicleTransmissionType transmissionTypeField;

        private bool transmissionTypeFieldSpecified;

        private VehicleCoreTypeFuelType fuelTypeField;

        private bool fuelTypeFieldSpecified;

        private VehicleCoreTypeDriveType driveTypeField;

        private bool driveTypeFieldSpecified;

        /// <remarks/>
        public VehicleCoreTypeVehType VehType
        {
            get
            {
                return vehTypeField;
            }
            set
            {
                vehTypeField = value;
            }
        }

        /// <remarks/>
        public VehicleCoreTypeVehClass VehClass
        {
            get
            {
                return vehClassField;
            }
            set
            {
                vehClassField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool AirConditionInd
        {
            get
            {
                return airConditionIndField;
            }
            set
            {
                airConditionIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AirConditionIndSpecified
        {
            get
            {
                return airConditionIndFieldSpecified;
            }
            set
            {
                airConditionIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public VehicleTransmissionType TransmissionType
        {
            get
            {
                return transmissionTypeField;
            }
            set
            {
                transmissionTypeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TransmissionTypeSpecified
        {
            get
            {
                return transmissionTypeFieldSpecified;
            }
            set
            {
                transmissionTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public VehicleCoreTypeFuelType FuelType
        {
            get
            {
                return fuelTypeField;
            }
            set
            {
                fuelTypeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool FuelTypeSpecified
        {
            get
            {
                return fuelTypeFieldSpecified;
            }
            set
            {
                fuelTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public VehicleCoreTypeDriveType DriveType
        {
            get
            {
                return driveTypeField;
            }
            set
            {
                driveTypeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DriveTypeSpecified
        {
            get
            {
                return driveTypeFieldSpecified;
            }
            set
            {
                driveTypeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleCoreTypeVehType
    {

        private string vehicleCategoryField;

        private string doorCountField;

        /// <remarks/>
        [XmlAttribute()]
        public string VehicleCategory
        {
            get
            {
                return vehicleCategoryField;
            }
            set
            {
                vehicleCategoryField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DoorCount
        {
            get
            {
                return doorCountField;
            }
            set
            {
                doorCountField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleCoreTypeVehClass
    {

        private string sizeField;

        /// <remarks/>
        [XmlAttribute()]
        public string Size
        {
            get
            {
                return sizeField;
            }
            set
            {
                sizeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum VehicleTransmissionType
    {

        /// <remarks/>
        Automatic,

        /// <remarks/>
        Manual
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum VehicleCoreTypeFuelType
    {

        /// <remarks/>
        Unspecified,

        /// <remarks/>
        Diesel,

        /// <remarks/>
        Hybrid,

        /// <remarks/>
        Electric,

        /// <remarks/>
        LPG_CompressedGas,

        /// <remarks/>
        Hydrogen,

        /// <remarks/>
        MultiFuel,

        /// <remarks/>
        Petrol,

        /// <remarks/>
        Ethanol
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum VehicleCoreTypeDriveType
    {

        /// <remarks/>
        AWD,

        /// <remarks/>
        [XmlEnum("4WD")]
        Item4WD,

        /// <remarks/>
        Unspecified
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehiclePrefType : VehicleCoreType
    {

        private VehiclePrefTypeVehMakeModel vehMakeModelField;

        private PreferLevelType typePrefField;

        private bool typePrefFieldSpecified;

        private PreferLevelType classPrefField;

        private bool classPrefFieldSpecified;

        private PreferLevelType airConditionPrefField;

        private bool airConditionPrefFieldSpecified;

        private PreferLevelType transmissionPrefField;

        private bool transmissionPrefFieldSpecified;

        private string vendorCarTypeField;

        private long vehicleQtyField;

        private bool vehicleQtyFieldSpecified;

        private string codeField;

        private string codeContextField;

        /// <remarks/>
        public VehiclePrefTypeVehMakeModel VehMakeModel
        {
            get
            {
                return vehMakeModelField;
            }
            set
            {
                vehMakeModelField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PreferLevelType TypePref
        {
            get
            {
                return typePrefField;
            }
            set
            {
                typePrefField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TypePrefSpecified
        {
            get
            {
                return typePrefFieldSpecified;
            }
            set
            {
                typePrefFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PreferLevelType ClassPref
        {
            get
            {
                return classPrefField;
            }
            set
            {
                classPrefField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ClassPrefSpecified
        {
            get
            {
                return classPrefFieldSpecified;
            }
            set
            {
                classPrefFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PreferLevelType AirConditionPref
        {
            get
            {
                return airConditionPrefField;
            }
            set
            {
                airConditionPrefField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AirConditionPrefSpecified
        {
            get
            {
                return airConditionPrefFieldSpecified;
            }
            set
            {
                airConditionPrefFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PreferLevelType TransmissionPref
        {
            get
            {
                return transmissionPrefField;
            }
            set
            {
                transmissionPrefField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TransmissionPrefSpecified
        {
            get
            {
                return transmissionPrefFieldSpecified;
            }
            set
            {
                transmissionPrefFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string VendorCarType
        {
            get
            {
                return vendorCarTypeField;
            }
            set
            {
                vendorCarTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long VehicleQty
        {
            get
            {
                return vehicleQtyField;
            }
            set
            {
                vehicleQtyField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool VehicleQtySpecified
        {
            get
            {
                return vehicleQtyFieldSpecified;
            }
            set
            {
                vehicleQtyFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CodeContext
        {
            get
            {
                return codeContextField;
            }
            set
            {
                codeContextField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehiclePrefTypeVehMakeModel
    {

        private string nameField;

        private string codeField;

        private string modelYearField;

        /// <remarks/>
        [XmlAttribute()]
        public string Name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "gYear")]
        public string ModelYear
        {
            get
            {
                return modelYearField;
            }
            set
            {
                modelYearField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleRentalRateType
    {

        private VehicleRentalRateTypeRateDistance[] rateDistanceField;

        private VehicleChargePurposeType[] vehicleChargesField;

        private VehicleRentalRateTypeRateQualifier rateQualifierField;

        private VehicleRentalRateTypeRateRestrictions rateRestrictionsField;

        private VehicleRentalRateTypeRateGuarantee rateGuaranteeField;

        private VehicleRentalRateTypePickupReturnRule[] pickupReturnRuleField;

        private NoShowFeeType noShowFeeInfoField;

        private string quoteIDField;

        /// <remarks/>
        [XmlElement("RateDistance")]
        public VehicleRentalRateTypeRateDistance[] RateDistance
        {
            get
            {
                return rateDistanceField;
            }
            set
            {
                rateDistanceField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("VehicleCharge", IsNullable = false)]
        public VehicleChargePurposeType[] VehicleCharges
        {
            get
            {
                return vehicleChargesField;
            }
            set
            {
                vehicleChargesField = value;
            }
        }

        /// <remarks/>
        public VehicleRentalRateTypeRateQualifier RateQualifier
        {
            get
            {
                return rateQualifierField;
            }
            set
            {
                rateQualifierField = value;
            }
        }

        /// <remarks/>
        public VehicleRentalRateTypeRateRestrictions RateRestrictions
        {
            get
            {
                return rateRestrictionsField;
            }
            set
            {
                rateRestrictionsField = value;
            }
        }

        /// <remarks/>
        public VehicleRentalRateTypeRateGuarantee RateGuarantee
        {
            get
            {
                return rateGuaranteeField;
            }
            set
            {
                rateGuaranteeField = value;
            }
        }

        /// <remarks/>
        [XmlElement("PickupReturnRule")]
        public VehicleRentalRateTypePickupReturnRule[] PickupReturnRule
        {
            get
            {
                return pickupReturnRuleField;
            }
            set
            {
                pickupReturnRuleField = value;
            }
        }

        /// <remarks/>
        public NoShowFeeType NoShowFeeInfo
        {
            get
            {
                return noShowFeeInfoField;
            }
            set
            {
                noShowFeeInfoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string QuoteID
        {
            get
            {
                return quoteIDField;
            }
            set
            {
                quoteIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleRentalRateTypeRateDistance
    {

        private bool unlimitedField;

        private long quantityField;

        private bool quantityFieldSpecified;

        private DistanceUnitNameType distUnitNameField;

        private bool distUnitNameFieldSpecified;

        private VehiclePeriodUnitNameType vehiclePeriodUnitNameField;

        private bool vehiclePeriodUnitNameFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public bool Unlimited
        {
            get
            {
                return unlimitedField;
            }
            set
            {
                unlimitedField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long Quantity
        {
            get
            {
                return quantityField;
            }
            set
            {
                quantityField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool QuantitySpecified
        {
            get
            {
                return quantityFieldSpecified;
            }
            set
            {
                quantityFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DistanceUnitNameType DistUnitName
        {
            get
            {
                return distUnitNameField;
            }
            set
            {
                distUnitNameField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DistUnitNameSpecified
        {
            get
            {
                return distUnitNameFieldSpecified;
            }
            set
            {
                distUnitNameFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public VehiclePeriodUnitNameType VehiclePeriodUnitName
        {
            get
            {
                return vehiclePeriodUnitNameField;
            }
            set
            {
                vehiclePeriodUnitNameField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool VehiclePeriodUnitNameSpecified
        {
            get
            {
                return vehiclePeriodUnitNameFieldSpecified;
            }
            set
            {
                vehiclePeriodUnitNameFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum VehiclePeriodUnitNameType
    {

        /// <remarks/>
        RentalPeriod,

        /// <remarks/>
        Year,

        /// <remarks/>
        Month,

        /// <remarks/>
        Week,

        /// <remarks/>
        Day,

        /// <remarks/>
        Hour,

        /// <remarks/>
        Weekend,

        /// <remarks/>
        ExtraMonth,

        /// <remarks/>
        Bundle,

        /// <remarks/>
        Package,

        /// <remarks/>
        ExtraDay,

        /// <remarks/>
        ExtraHour,

        /// <remarks/>
        ExtraWeek
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleChargePurposeType : VehicleChargeType
    {

        private string purposeField;

        private bool requiredIndField;

        private bool requiredIndFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public string Purpose
        {
            get
            {
                return purposeField;
            }
            set
            {
                purposeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool RequiredInd
        {
            get
            {
                return requiredIndField;
            }
            set
            {
                requiredIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool RequiredIndSpecified
        {
            get
            {
                return requiredIndFieldSpecified;
            }
            set
            {
                requiredIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [XmlInclude(typeof(VehicleChargePurposeType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleChargeType
    {

        private VehicleChargeTypeTaxAmount[] taxAmountsField;

        private VehicleChargeTypeMinMax minMaxField;

        private VehicleChargeTypeCalculation[] calculationField;

        private decimal amountField;

        private bool amountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        private bool taxInclusiveField;

        private bool taxInclusiveFieldSpecified;

        private string descriptionField;

        private bool guaranteedIndField;

        private bool guaranteedIndFieldSpecified;

        private bool includedInRateField;

        private bool includedInRateFieldSpecified;

        private bool includedInEstTotalIndField;

        private bool includedInEstTotalIndFieldSpecified;

        private bool rateConvertIndField;

        private bool rateConvertIndFieldSpecified;

        /// <remarks/>
        [XmlArrayItem("TaxAmount", IsNullable = false)]
        public VehicleChargeTypeTaxAmount[] TaxAmounts
        {
            get
            {
                return taxAmountsField;
            }
            set
            {
                taxAmountsField = value;
            }
        }

        /// <remarks/>
        public VehicleChargeTypeMinMax MinMax
        {
            get
            {
                return minMaxField;
            }
            set
            {
                minMaxField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Calculation")]
        public VehicleChargeTypeCalculation[] Calculation
        {
            get
            {
                return calculationField;
            }
            set
            {
                calculationField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified
        {
            get
            {
                return amountFieldSpecified;
            }
            set
            {
                amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool TaxInclusive
        {
            get
            {
                return taxInclusiveField;
            }
            set
            {
                taxInclusiveField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TaxInclusiveSpecified
        {
            get
            {
                return taxInclusiveFieldSpecified;
            }
            set
            {
                taxInclusiveFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool GuaranteedInd
        {
            get
            {
                return guaranteedIndField;
            }
            set
            {
                guaranteedIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool GuaranteedIndSpecified
        {
            get
            {
                return guaranteedIndFieldSpecified;
            }
            set
            {
                guaranteedIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool IncludedInRate
        {
            get
            {
                return includedInRateField;
            }
            set
            {
                includedInRateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool IncludedInRateSpecified
        {
            get
            {
                return includedInRateFieldSpecified;
            }
            set
            {
                includedInRateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool IncludedInEstTotalInd
        {
            get
            {
                return includedInEstTotalIndField;
            }
            set
            {
                includedInEstTotalIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool IncludedInEstTotalIndSpecified
        {
            get
            {
                return includedInEstTotalIndFieldSpecified;
            }
            set
            {
                includedInEstTotalIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool RateConvertInd
        {
            get
            {
                return rateConvertIndField;
            }
            set
            {
                rateConvertIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool RateConvertIndSpecified
        {
            get
            {
                return rateConvertIndFieldSpecified;
            }
            set
            {
                rateConvertIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleChargeTypeTaxAmount
    {

        private decimal totalField;

        private string currencyCodeField;

        private string taxCodeField;

        private decimal percentageField;

        private bool percentageFieldSpecified;

        private string descriptionField;

        /// <remarks/>
        [XmlAttribute()]
        public decimal Total
        {
            get
            {
                return totalField;
            }
            set
            {
                totalField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TaxCode
        {
            get
            {
                return taxCodeField;
            }
            set
            {
                taxCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Percentage
        {
            get
            {
                return percentageField;
            }
            set
            {
                percentageField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PercentageSpecified
        {
            get
            {
                return percentageFieldSpecified;
            }
            set
            {
                percentageFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleChargeTypeMinMax
    {

        private decimal maxChargeField;

        private bool maxChargeFieldSpecified;

        private decimal minChargeField;

        private bool minChargeFieldSpecified;

        private long maxChargeDaysField;

        private bool maxChargeDaysFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public decimal MaxCharge
        {
            get
            {
                return maxChargeField;
            }
            set
            {
                maxChargeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MaxChargeSpecified
        {
            get
            {
                return maxChargeFieldSpecified;
            }
            set
            {
                maxChargeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal MinCharge
        {
            get
            {
                return minChargeField;
            }
            set
            {
                minChargeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MinChargeSpecified
        {
            get
            {
                return minChargeFieldSpecified;
            }
            set
            {
                minChargeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long MaxChargeDays
        {
            get
            {
                return maxChargeDaysField;
            }
            set
            {
                maxChargeDaysField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MaxChargeDaysSpecified
        {
            get
            {
                return maxChargeDaysFieldSpecified;
            }
            set
            {
                maxChargeDaysFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleChargeTypeCalculation
    {

        private decimal unitChargeField;

        private bool unitChargeFieldSpecified;

        private string unitNameField;

        private string quantityField;

        private decimal percentageField;

        private bool percentageFieldSpecified;

        private VehicleChargeTypeCalculationApplicability applicabilityField;

        private bool applicabilityFieldSpecified;

        private string maxQuantityField;

        private decimal totalField;

        private bool totalFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public decimal UnitCharge
        {
            get
            {
                return unitChargeField;
            }
            set
            {
                unitChargeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool UnitChargeSpecified
        {
            get
            {
                return unitChargeFieldSpecified;
            }
            set
            {
                unitChargeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string UnitName
        {
            get
            {
                return unitNameField;
            }
            set
            {
                unitNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string Quantity
        {
            get
            {
                return quantityField;
            }
            set
            {
                quantityField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Percentage
        {
            get
            {
                return percentageField;
            }
            set
            {
                percentageField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PercentageSpecified
        {
            get
            {
                return percentageFieldSpecified;
            }
            set
            {
                percentageFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public VehicleChargeTypeCalculationApplicability Applicability
        {
            get
            {
                return applicabilityField;
            }
            set
            {
                applicabilityField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ApplicabilitySpecified
        {
            get
            {
                return applicabilityFieldSpecified;
            }
            set
            {
                applicabilityFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string MaxQuantity
        {
            get
            {
                return maxQuantityField;
            }
            set
            {
                maxQuantityField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Total
        {
            get
            {
                return totalField;
            }
            set
            {
                totalField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TotalSpecified
        {
            get
            {
                return totalFieldSpecified;
            }
            set
            {
                totalFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum VehicleChargeTypeCalculationApplicability
    {

        /// <remarks/>
        FromPickupLocation,

        /// <remarks/>
        FromDropoffLocation,

        /// <remarks/>
        BeforePickup,

        /// <remarks/>
        AfterDropoff
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleRentalRateTypeRateQualifier : RateQualifierType
    {

        private string tourInfoRPHField;

        private string[] custLoyaltyRPHField;

        private string quoteIDField;

        /// <remarks/>
        [XmlAttribute()]
        public string TourInfoRPH
        {
            get
            {
                return tourInfoRPHField;
            }
            set
            {
                tourInfoRPHField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string[] CustLoyaltyRPH
        {
            get
            {
                return custLoyaltyRPHField;
            }
            set
            {
                custLoyaltyRPHField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string QuoteID
        {
            get
            {
                return quoteIDField;
            }
            set
            {
                quoteIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class RateQualifierType
    {

        private string promoDescField;

        private RateQualifierTypeRateComment[] rateCommentsField;

        private string travelPurposeField;

        private string rateCategoryField;

        private string corpDiscountNmbrField;

        private string promotionCodeField;

        private string[] promotionVendorCodeField;

        private string rateQualifierField;

        private RateQualifierTypeRatePeriod ratePeriodField;

        private bool ratePeriodFieldSpecified;

        private bool guaranteedIndField;

        private bool guaranteedIndFieldSpecified;

        private bool arriveByFlightField;

        private bool arriveByFlightFieldSpecified;

        private string rateAuthorizationCodeField;

        private string vendorRateIDField;

        /// <remarks/>
        public string PromoDesc
        {
            get
            {
                return promoDescField;
            }
            set
            {
                promoDescField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("RateComment", IsNullable = false)]
        public RateQualifierTypeRateComment[] RateComments
        {
            get
            {
                return rateCommentsField;
            }
            set
            {
                rateCommentsField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TravelPurpose
        {
            get
            {
                return travelPurposeField;
            }
            set
            {
                travelPurposeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RateCategory
        {
            get
            {
                return rateCategoryField;
            }
            set
            {
                rateCategoryField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CorpDiscountNmbr
        {
            get
            {
                return corpDiscountNmbrField;
            }
            set
            {
                corpDiscountNmbrField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PromotionCode
        {
            get
            {
                return promotionCodeField;
            }
            set
            {
                promotionCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string[] PromotionVendorCode
        {
            get
            {
                return promotionVendorCodeField;
            }
            set
            {
                promotionVendorCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RateQualifier
        {
            get
            {
                return rateQualifierField;
            }
            set
            {
                rateQualifierField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public RateQualifierTypeRatePeriod RatePeriod
        {
            get
            {
                return ratePeriodField;
            }
            set
            {
                ratePeriodField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool RatePeriodSpecified
        {
            get
            {
                return ratePeriodFieldSpecified;
            }
            set
            {
                ratePeriodFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool GuaranteedInd
        {
            get
            {
                return guaranteedIndField;
            }
            set
            {
                guaranteedIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool GuaranteedIndSpecified
        {
            get
            {
                return guaranteedIndFieldSpecified;
            }
            set
            {
                guaranteedIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ArriveByFlight
        {
            get
            {
                return arriveByFlightField;
            }
            set
            {
                arriveByFlightField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ArriveByFlightSpecified
        {
            get
            {
                return arriveByFlightFieldSpecified;
            }
            set
            {
                arriveByFlightFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RateAuthorizationCode
        {
            get
            {
                return rateAuthorizationCodeField;
            }
            set
            {
                rateAuthorizationCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string VendorRateID
        {
            get
            {
                return vendorRateIDField;
            }
            set
            {
                vendorRateIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class RateQualifierTypeRateComment : FormattedTextTextType
    {

        private string nameField;

        /// <remarks/>
        [XmlAttribute()]
        public string Name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum RateQualifierTypeRatePeriod
    {

        /// <remarks/>
        Hourly,

        /// <remarks/>
        Daily,

        /// <remarks/>
        Weekly,

        /// <remarks/>
        Monthly,

        /// <remarks/>
        WeekendDay,

        /// <remarks/>
        Other,

        /// <remarks/>
        Package,

        /// <remarks/>
        Bundle,

        /// <remarks/>
        Total
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleRentalRateTypeRateRestrictions
    {

        private bool arriveByFlightField;

        private bool arriveByFlightFieldSpecified;

        private bool minimumDayIndField;

        private bool minimumDayIndFieldSpecified;

        private bool maximumDayIndField;

        private bool maximumDayIndFieldSpecified;

        private bool advancedBookingIndField;

        private bool advancedBookingIndFieldSpecified;

        private bool restrictedMileageIndField;

        private bool restrictedMileageIndFieldSpecified;

        private bool corporateRateIndField;

        private bool corporateRateIndFieldSpecified;

        private bool guaranteeReqIndField;

        private bool guaranteeReqIndFieldSpecified;

        private string maximumVehiclesAllowedField;

        private bool overnightIndField;

        private bool overnightIndFieldSpecified;

        private VehicleRentalRateTypeRateRestrictionsOneWayPolicy oneWayPolicyField;

        private bool oneWayPolicyFieldSpecified;

        private bool cancellationPenaltyIndField;

        private bool cancellationPenaltyIndFieldSpecified;

        private bool modificationPenaltyIndField;

        private bool modificationPenaltyIndFieldSpecified;

        private string minimumAgeField;

        private string maximumAgeField;

        private bool noShowFeeIndField;

        private bool noShowFeeIndFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public bool ArriveByFlight
        {
            get
            {
                return arriveByFlightField;
            }
            set
            {
                arriveByFlightField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ArriveByFlightSpecified
        {
            get
            {
                return arriveByFlightFieldSpecified;
            }
            set
            {
                arriveByFlightFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool MinimumDayInd
        {
            get
            {
                return minimumDayIndField;
            }
            set
            {
                minimumDayIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MinimumDayIndSpecified
        {
            get
            {
                return minimumDayIndFieldSpecified;
            }
            set
            {
                minimumDayIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool MaximumDayInd
        {
            get
            {
                return maximumDayIndField;
            }
            set
            {
                maximumDayIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MaximumDayIndSpecified
        {
            get
            {
                return maximumDayIndFieldSpecified;
            }
            set
            {
                maximumDayIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool AdvancedBookingInd
        {
            get
            {
                return advancedBookingIndField;
            }
            set
            {
                advancedBookingIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AdvancedBookingIndSpecified
        {
            get
            {
                return advancedBookingIndFieldSpecified;
            }
            set
            {
                advancedBookingIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool RestrictedMileageInd
        {
            get
            {
                return restrictedMileageIndField;
            }
            set
            {
                restrictedMileageIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool RestrictedMileageIndSpecified
        {
            get
            {
                return restrictedMileageIndFieldSpecified;
            }
            set
            {
                restrictedMileageIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool CorporateRateInd
        {
            get
            {
                return corporateRateIndField;
            }
            set
            {
                corporateRateIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool CorporateRateIndSpecified
        {
            get
            {
                return corporateRateIndFieldSpecified;
            }
            set
            {
                corporateRateIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool GuaranteeReqInd
        {
            get
            {
                return guaranteeReqIndField;
            }
            set
            {
                guaranteeReqIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool GuaranteeReqIndSpecified
        {
            get
            {
                return guaranteeReqIndFieldSpecified;
            }
            set
            {
                guaranteeReqIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string MaximumVehiclesAllowed
        {
            get
            {
                return maximumVehiclesAllowedField;
            }
            set
            {
                maximumVehiclesAllowedField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool OvernightInd
        {
            get
            {
                return overnightIndField;
            }
            set
            {
                overnightIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool OvernightIndSpecified
        {
            get
            {
                return overnightIndFieldSpecified;
            }
            set
            {
                overnightIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public VehicleRentalRateTypeRateRestrictionsOneWayPolicy OneWayPolicy
        {
            get
            {
                return oneWayPolicyField;
            }
            set
            {
                oneWayPolicyField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool OneWayPolicySpecified
        {
            get
            {
                return oneWayPolicyFieldSpecified;
            }
            set
            {
                oneWayPolicyFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool CancellationPenaltyInd
        {
            get
            {
                return cancellationPenaltyIndField;
            }
            set
            {
                cancellationPenaltyIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool CancellationPenaltyIndSpecified
        {
            get
            {
                return cancellationPenaltyIndFieldSpecified;
            }
            set
            {
                cancellationPenaltyIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ModificationPenaltyInd
        {
            get
            {
                return modificationPenaltyIndField;
            }
            set
            {
                modificationPenaltyIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ModificationPenaltyIndSpecified
        {
            get
            {
                return modificationPenaltyIndFieldSpecified;
            }
            set
            {
                modificationPenaltyIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string MinimumAge
        {
            get
            {
                return minimumAgeField;
            }
            set
            {
                minimumAgeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string MaximumAge
        {
            get
            {
                return maximumAgeField;
            }
            set
            {
                maximumAgeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool NoShowFeeInd
        {
            get
            {
                return noShowFeeIndField;
            }
            set
            {
                noShowFeeIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool NoShowFeeIndSpecified
        {
            get
            {
                return noShowFeeIndFieldSpecified;
            }
            set
            {
                noShowFeeIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum VehicleRentalRateTypeRateRestrictionsOneWayPolicy
    {

        /// <remarks/>
        OneWayAllowed,

        /// <remarks/>
        OneWayNotAllowed,

        /// <remarks/>
        RestrictedOneWay
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleRentalRateTypeRateGuarantee
    {

        private FormattedTextTextType descriptionField;

        private string absoluteDeadlineField;

        private TimeUnitType offsetTimeUnitField;

        private bool offsetTimeUnitFieldSpecified;

        private string offsetUnitMultiplierField;

        private VehicleRentalRateTypeRateGuaranteeOffsetDropTime offsetDropTimeField;

        private bool offsetDropTimeFieldSpecified;

        /// <remarks/>
        public FormattedTextTextType Description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string AbsoluteDeadline
        {
            get
            {
                return absoluteDeadlineField;
            }
            set
            {
                absoluteDeadlineField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public TimeUnitType OffsetTimeUnit
        {
            get
            {
                return offsetTimeUnitField;
            }
            set
            {
                offsetTimeUnitField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool OffsetTimeUnitSpecified
        {
            get
            {
                return offsetTimeUnitFieldSpecified;
            }
            set
            {
                offsetTimeUnitFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string OffsetUnitMultiplier
        {
            get
            {
                return offsetUnitMultiplierField;
            }
            set
            {
                offsetUnitMultiplierField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public VehicleRentalRateTypeRateGuaranteeOffsetDropTime OffsetDropTime
        {
            get
            {
                return offsetDropTimeField;
            }
            set
            {
                offsetDropTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool OffsetDropTimeSpecified
        {
            get
            {
                return offsetDropTimeFieldSpecified;
            }
            set
            {
                offsetDropTimeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum TimeUnitType
    {

        /// <remarks/>
        Year,

        /// <remarks/>
        Month,

        /// <remarks/>
        Week,

        /// <remarks/>
        Day,

        /// <remarks/>
        Hour,

        /// <remarks/>
        Second,

        /// <remarks/>
        FullDuration,

        /// <remarks/>
        Minute
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum VehicleRentalRateTypeRateGuaranteeOffsetDropTime
    {

        /// <remarks/>
        BeforeArrival,

        /// <remarks/>
        AfterBooking,

        /// <remarks/>
        AfterConfirmation,

        /// <remarks/>
        AfterArrival
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleRentalRateTypePickupReturnRule
    {

        private DayOfWeekType dayOfWeekField;

        private bool dayOfWeekFieldSpecified;

        private string timeField;

        private VehicleRentalRateTypePickupReturnRuleRuleType ruleTypeField;

        private bool ruleTypeFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public DayOfWeekType DayOfWeek
        {
            get
            {
                return dayOfWeekField;
            }
            set
            {
                dayOfWeekField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DayOfWeekSpecified
        {
            get
            {
                return dayOfWeekFieldSpecified;
            }
            set
            {
                dayOfWeekFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Time
        {
            get
            {
                return timeField;
            }
            set
            {
                timeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public VehicleRentalRateTypePickupReturnRuleRuleType RuleType
        {
            get
            {
                return ruleTypeField;
            }
            set
            {
                ruleTypeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool RuleTypeSpecified
        {
            get
            {
                return ruleTypeFieldSpecified;
            }
            set
            {
                ruleTypeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum DayOfWeekType
    {

        /// <remarks/>
        Mon,

        /// <remarks/>
        Tue,

        /// <remarks/>
        Wed,

        /// <remarks/>
        Thu,

        /// <remarks/>
        Fri,

        /// <remarks/>
        Sat,

        /// <remarks/>
        Sun
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum VehicleRentalRateTypePickupReturnRuleRuleType
    {

        /// <remarks/>
        EarliestPickup,

        /// <remarks/>
        LatestPickup,

        /// <remarks/>
        LatestReturn
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class NoShowFeeType
    {

        private NoShowFeeTypeDeadline deadlineField;

        private NoShowFeeTypeGracePeriod gracePeriodField;

        private NoShowFeeTypeFeeAmount feeAmountField;

        private FormattedTextTextType descriptionField;

        /// <remarks/>
        public NoShowFeeTypeDeadline Deadline
        {
            get
            {
                return deadlineField;
            }
            set
            {
                deadlineField = value;
            }
        }

        /// <remarks/>
        public NoShowFeeTypeGracePeriod GracePeriod
        {
            get
            {
                return gracePeriodField;
            }
            set
            {
                gracePeriodField = value;
            }
        }

        /// <remarks/>
        public NoShowFeeTypeFeeAmount FeeAmount
        {
            get
            {
                return feeAmountField;
            }
            set
            {
                feeAmountField = value;
            }
        }

        /// <remarks/>
        public FormattedTextTextType Description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class NoShowFeeTypeDeadline
    {

        private string absoluteDeadlineField;

        private TimeUnitType offsetTimeUnitField;

        private bool offsetTimeUnitFieldSpecified;

        private string offsetUnitMultiplierField;

        private VehicleRentalRateTypeRateGuaranteeOffsetDropTime offsetDropTimeField;

        private bool offsetDropTimeFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public string AbsoluteDeadline
        {
            get
            {
                return absoluteDeadlineField;
            }
            set
            {
                absoluteDeadlineField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public TimeUnitType OffsetTimeUnit
        {
            get
            {
                return offsetTimeUnitField;
            }
            set
            {
                offsetTimeUnitField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool OffsetTimeUnitSpecified
        {
            get
            {
                return offsetTimeUnitFieldSpecified;
            }
            set
            {
                offsetTimeUnitFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string OffsetUnitMultiplier
        {
            get
            {
                return offsetUnitMultiplierField;
            }
            set
            {
                offsetUnitMultiplierField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public VehicleRentalRateTypeRateGuaranteeOffsetDropTime OffsetDropTime
        {
            get
            {
                return offsetDropTimeField;
            }
            set
            {
                offsetDropTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool OffsetDropTimeSpecified
        {
            get
            {
                return offsetDropTimeFieldSpecified;
            }
            set
            {
                offsetDropTimeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class NoShowFeeTypeGracePeriod
    {

        private string absoluteDeadlineField;

        private TimeUnitType offsetTimeUnitField;

        private bool offsetTimeUnitFieldSpecified;

        private string offsetUnitMultiplierField;

        private VehicleRentalRateTypeRateGuaranteeOffsetDropTime offsetDropTimeField;

        private bool offsetDropTimeFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public string AbsoluteDeadline
        {
            get
            {
                return absoluteDeadlineField;
            }
            set
            {
                absoluteDeadlineField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public TimeUnitType OffsetTimeUnit
        {
            get
            {
                return offsetTimeUnitField;
            }
            set
            {
                offsetTimeUnitField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool OffsetTimeUnitSpecified
        {
            get
            {
                return offsetTimeUnitFieldSpecified;
            }
            set
            {
                offsetTimeUnitFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string OffsetUnitMultiplier
        {
            get
            {
                return offsetUnitMultiplierField;
            }
            set
            {
                offsetUnitMultiplierField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public VehicleRentalRateTypeRateGuaranteeOffsetDropTime OffsetDropTime
        {
            get
            {
                return offsetDropTimeField;
            }
            set
            {
                offsetDropTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool OffsetDropTimeSpecified
        {
            get
            {
                return offsetDropTimeFieldSpecified;
            }
            set
            {
                offsetDropTimeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class NoShowFeeTypeFeeAmount
    {

        private decimal amountField;

        private bool amountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        private bool rateConvertedIndField;

        private bool rateConvertedIndFieldSpecified;

        private bool guaranteeReqIndField;

        private bool guaranteeReqIndFieldSpecified;

        private bool emailRequiredIndField;

        private bool emailRequiredIndFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified
        {
            get
            {
                return amountFieldSpecified;
            }
            set
            {
                amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool RateConvertedInd
        {
            get
            {
                return rateConvertedIndField;
            }
            set
            {
                rateConvertedIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool RateConvertedIndSpecified
        {
            get
            {
                return rateConvertedIndFieldSpecified;
            }
            set
            {
                rateConvertedIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool GuaranteeReqInd
        {
            get
            {
                return guaranteeReqIndField;
            }
            set
            {
                guaranteeReqIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool GuaranteeReqIndSpecified
        {
            get
            {
                return guaranteeReqIndFieldSpecified;
            }
            set
            {
                guaranteeReqIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool EmailRequiredInd
        {
            get
            {
                return emailRequiredIndField;
            }
            set
            {
                emailRequiredIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool EmailRequiredIndSpecified
        {
            get
            {
                return emailRequiredIndFieldSpecified;
            }
            set
            {
                emailRequiredIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleEquipmentPricedType
    {

        private VehicleEquipmentType equipmentField;

        private VehicleChargeType chargeField;

        private bool requiredField;

        private bool requiredFieldSpecified;

        /// <remarks/>
        public VehicleEquipmentType Equipment
        {
            get
            {
                return equipmentField;
            }
            set
            {
                equipmentField = value;
            }
        }

        /// <remarks/>
        public VehicleChargeType Charge
        {
            get
            {
                return chargeField;
            }
            set
            {
                chargeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool Required
        {
            get
            {
                return requiredField;
            }
            set
            {
                requiredField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool RequiredSpecified
        {
            get
            {
                return requiredFieldSpecified;
            }
            set
            {
                requiredFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleEquipmentType
    {

        private string descriptionField;

        private string equipTypeField;

        private long quantityField;

        private bool quantityFieldSpecified;

        private EquipmentRestrictionType restrictionField;

        private bool restrictionFieldSpecified;

        /// <remarks/>
        public string Description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string EquipType
        {
            get
            {
                return equipTypeField;
            }
            set
            {
                equipTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long Quantity
        {
            get
            {
                return quantityField;
            }
            set
            {
                quantityField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool QuantitySpecified
        {
            get
            {
                return quantityFieldSpecified;
            }
            set
            {
                quantityFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public EquipmentRestrictionType Restriction
        {
            get
            {
                return restrictionField;
            }
            set
            {
                restrictionField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool RestrictionSpecified
        {
            get
            {
                return restrictionFieldSpecified;
            }
            set
            {
                restrictionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum EquipmentRestrictionType
    {

        /// <remarks/>
        OneWayOnly,

        /// <remarks/>
        RoundTripOnly,

        /// <remarks/>
        AnyReservation
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleSegmentCoreTypeTotalCharge
    {

        private decimal rateTotalAmountField;

        private bool rateTotalAmountFieldSpecified;

        private decimal estimatedTotalAmountField;

        private bool estimatedTotalAmountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public decimal RateTotalAmount
        {
            get
            {
                return rateTotalAmountField;
            }
            set
            {
                rateTotalAmountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool RateTotalAmountSpecified
        {
            get
            {
                return rateTotalAmountFieldSpecified;
            }
            set
            {
                rateTotalAmountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal EstimatedTotalAmount
        {
            get
            {
                return estimatedTotalAmountField;
            }
            set
            {
                estimatedTotalAmountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool EstimatedTotalAmountSpecified
        {
            get
            {
                return estimatedTotalAmountFieldSpecified;
            }
            set
            {
                estimatedTotalAmountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleSegmentAdditionalInfoType
    {

        private MonetaryRuleType[] paymentRulesField;

        private PaymentDetailType[] rentalPaymentAmountField;

        private CoveragePricedType[] pricedCoveragesField;

        private OffLocationServicePricedType[] pricedOffLocServiceField;

        private FormattedTextType[] vendorMessagesField;

        private VehicleLocationDetailsType[] locationDetailsField;

        private VehicleTourInfoType tourInfoField;

        private VehicleSpecialReqPrefType[] specialReqPrefField;

        private VehicleArrivalDetailsType arrivalDetailsField;

        private WrittenConfInstType writtenConfInstField;

        private ParagraphType[] remarkField;

        private TPA_ExtensionsType tPA_ExtensionsField;

        private bool writtenConfIndField;

        private bool writtenConfIndFieldSpecified;

        /// <remarks/>
        [XmlArrayItem("PaymentRule", IsNullable = false)]
        public MonetaryRuleType[] PaymentRules
        {
            get
            {
                return paymentRulesField;
            }
            set
            {
                paymentRulesField = value;
            }
        }

        /// <remarks/>
        [XmlElement("RentalPaymentAmount")]
        public PaymentDetailType[] RentalPaymentAmount
        {
            get
            {
                return rentalPaymentAmountField;
            }
            set
            {
                rentalPaymentAmountField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("PricedCoverage", IsNullable = false)]
        public CoveragePricedType[] PricedCoverages
        {
            get
            {
                return pricedCoveragesField;
            }
            set
            {
                pricedCoveragesField = value;
            }
        }

        /// <remarks/>
        [XmlElement("PricedOffLocService")]
        public OffLocationServicePricedType[] PricedOffLocService
        {
            get
            {
                return pricedOffLocServiceField;
            }
            set
            {
                pricedOffLocServiceField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("VendorMessage", IsNullable = false)]
        public FormattedTextType[] VendorMessages
        {
            get
            {
                return vendorMessagesField;
            }
            set
            {
                vendorMessagesField = value;
            }
        }

        /// <remarks/>
        [XmlElement("LocationDetails")]
        public VehicleLocationDetailsType[] LocationDetails
        {
            get
            {
                return locationDetailsField;
            }
            set
            {
                locationDetailsField = value;
            }
        }

        /// <remarks/>
        public VehicleTourInfoType TourInfo
        {
            get
            {
                return tourInfoField;
            }
            set
            {
                tourInfoField = value;
            }
        }

        /// <remarks/>
        [XmlElement("SpecialReqPref")]
        public VehicleSpecialReqPrefType[] SpecialReqPref
        {
            get
            {
                return specialReqPrefField;
            }
            set
            {
                specialReqPrefField = value;
            }
        }

        /// <remarks/>
        public VehicleArrivalDetailsType ArrivalDetails
        {
            get
            {
                return arrivalDetailsField;
            }
            set
            {
                arrivalDetailsField = value;
            }
        }

        /// <remarks/>
        public WrittenConfInstType WrittenConfInst
        {
            get
            {
                return writtenConfInstField;
            }
            set
            {
                writtenConfInstField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Remark")]
        public ParagraphType[] Remark
        {
            get
            {
                return remarkField;
            }
            set
            {
                remarkField = value;
            }
        }

        /// <remarks/>
        public TPA_ExtensionsType TPA_Extensions
        {
            get
            {
                return tPA_ExtensionsField;
            }
            set
            {
                tPA_ExtensionsField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool WrittenConfInd
        {
            get
            {
                return writtenConfIndField;
            }
            set
            {
                writtenConfIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool WrittenConfIndSpecified
        {
            get
            {
                return writtenConfIndFieldSpecified;
            }
            set
            {
                writtenConfIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class MonetaryRuleType
    {

        private decimal amountField;

        private bool amountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        private string ruleTypeField;

        private decimal percentField;

        private bool percentFieldSpecified;

        private DateTime dateTimeField;

        private bool dateTimeFieldSpecified;

        private string paymentTypeField;

        private bool rateConvertedIndField;

        private bool rateConvertedIndFieldSpecified;

        private string absoluteDeadlineField;

        private TimeUnitType offsetTimeUnitField;

        private bool offsetTimeUnitFieldSpecified;

        private string offsetUnitMultiplierField;

        private VehicleRentalRateTypeRateGuaranteeOffsetDropTime offsetDropTimeField;

        private bool offsetDropTimeFieldSpecified;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified
        {
            get
            {
                return amountFieldSpecified;
            }
            set
            {
                amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RuleType
        {
            get
            {
                return ruleTypeField;
            }
            set
            {
                ruleTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Percent
        {
            get
            {
                return percentField;
            }
            set
            {
                percentField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PercentSpecified
        {
            get
            {
                return percentFieldSpecified;
            }
            set
            {
                percentFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DateTime DateTime
        {
            get
            {
                return dateTimeField;
            }
            set
            {
                dateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DateTimeSpecified
        {
            get
            {
                return dateTimeFieldSpecified;
            }
            set
            {
                dateTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PaymentType
        {
            get
            {
                return paymentTypeField;
            }
            set
            {
                paymentTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool RateConvertedInd
        {
            get
            {
                return rateConvertedIndField;
            }
            set
            {
                rateConvertedIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool RateConvertedIndSpecified
        {
            get
            {
                return rateConvertedIndFieldSpecified;
            }
            set
            {
                rateConvertedIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string AbsoluteDeadline
        {
            get
            {
                return absoluteDeadlineField;
            }
            set
            {
                absoluteDeadlineField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public TimeUnitType OffsetTimeUnit
        {
            get
            {
                return offsetTimeUnitField;
            }
            set
            {
                offsetTimeUnitField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool OffsetTimeUnitSpecified
        {
            get
            {
                return offsetTimeUnitFieldSpecified;
            }
            set
            {
                offsetTimeUnitFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string OffsetUnitMultiplier
        {
            get
            {
                return offsetUnitMultiplierField;
            }
            set
            {
                offsetUnitMultiplierField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public VehicleRentalRateTypeRateGuaranteeOffsetDropTime OffsetDropTime
        {
            get
            {
                return offsetDropTimeField;
            }
            set
            {
                offsetDropTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool OffsetDropTimeSpecified
        {
            get
            {
                return offsetDropTimeFieldSpecified;
            }
            set
            {
                offsetDropTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CoveragePricedType
    {

        private CoverageType coverageField;

        private VehicleChargeType chargeField;

        private DeductibleType deductibleField;

        private bool requiredField;

        private bool requiredFieldSpecified;

        /// <remarks/>
        public CoverageType Coverage
        {
            get
            {
                return coverageField;
            }
            set
            {
                coverageField = value;
            }
        }

        /// <remarks/>
        public VehicleChargeType Charge
        {
            get
            {
                return chargeField;
            }
            set
            {
                chargeField = value;
            }
        }

        /// <remarks/>
        public DeductibleType Deductible
        {
            get
            {
                return deductibleField;
            }
            set
            {
                deductibleField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool Required
        {
            get
            {
                return requiredField;
            }
            set
            {
                requiredField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool RequiredSpecified
        {
            get
            {
                return requiredFieldSpecified;
            }
            set
            {
                requiredFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CoverageType
    {

        private CoverageDetailsType[] detailsField;

        private string coverageType1Field;

        private string codeField;

        /// <remarks/>
        [XmlElement("Details")]
        public CoverageDetailsType[] Details
        {
            get
            {
                return detailsField;
            }
            set
            {
                detailsField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("CoverageType")]
        public string CoverageType1
        {
            get
            {
                return coverageType1Field;
            }
            set
            {
                coverageType1Field = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class DeductibleType
    {

        private decimal amountField;

        private bool amountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        private decimal liabilityAmountField;

        private bool liabilityAmountFieldSpecified;

        private decimal excessAmountField;

        private bool excessAmountFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified
        {
            get
            {
                return amountFieldSpecified;
            }
            set
            {
                amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal LiabilityAmount
        {
            get
            {
                return liabilityAmountField;
            }
            set
            {
                liabilityAmountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool LiabilityAmountSpecified
        {
            get
            {
                return liabilityAmountFieldSpecified;
            }
            set
            {
                liabilityAmountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal ExcessAmount
        {
            get
            {
                return excessAmountField;
            }
            set
            {
                excessAmountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExcessAmountSpecified
        {
            get
            {
                return excessAmountFieldSpecified;
            }
            set
            {
                excessAmountFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class OffLocationServicePricedType
    {

        private OffLocationServiceType offLocServiceField;

        private VehicleChargeType chargeField;

        /// <remarks/>
        public OffLocationServiceType OffLocService
        {
            get
            {
                return offLocServiceField;
            }
            set
            {
                offLocServiceField = value;
            }
        }

        /// <remarks/>
        public VehicleChargeType Charge
        {
            get
            {
                return chargeField;
            }
            set
            {
                chargeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class OffLocationServiceType : OffLocationServiceCoreType
    {

        private PersonNameType personNameField;

        private OffLocationServiceTypeTelephone telephoneField;

        private UniqueID_Type trackingIDField;

        private string specInstructionsField;

        /// <remarks/>
        public PersonNameType PersonName
        {
            get
            {
                return personNameField;
            }
            set
            {
                personNameField = value;
            }
        }

        /// <remarks/>
        public OffLocationServiceTypeTelephone Telephone
        {
            get
            {
                return telephoneField;
            }
            set
            {
                telephoneField = value;
            }
        }

        /// <remarks/>
        public UniqueID_Type TrackingID
        {
            get
            {
                return trackingIDField;
            }
            set
            {
                trackingIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string SpecInstructions
        {
            get
            {
                return specInstructionsField;
            }
            set
            {
                specInstructionsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class OffLocationServiceTypeTelephone
    {

        private OffLocationServiceTypeTelephoneShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private OffLocationServiceTypeTelephoneShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string phoneLocationTypeField;

        private string phoneTechTypeField;

        private string phoneUseTypeField;

        private string countryAccessCodeField;

        private string areaCityCodeField;

        private string phoneNumberField;

        private string extensionField;

        private string pINField;

        private string remarkField;

        private bool formattedIndField;

        private bool formattedIndFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public OffLocationServiceTypeTelephoneShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public OffLocationServiceTypeTelephoneShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneLocationType
        {
            get
            {
                return phoneLocationTypeField;
            }
            set
            {
                phoneLocationTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneTechType
        {
            get
            {
                return phoneTechTypeField;
            }
            set
            {
                phoneTechTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneUseType
        {
            get
            {
                return phoneUseTypeField;
            }
            set
            {
                phoneUseTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CountryAccessCode
        {
            get
            {
                return countryAccessCodeField;
            }
            set
            {
                countryAccessCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string AreaCityCode
        {
            get
            {
                return areaCityCodeField;
            }
            set
            {
                areaCityCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneNumber
        {
            get
            {
                return phoneNumberField;
            }
            set
            {
                phoneNumberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Extension
        {
            get
            {
                return extensionField;
            }
            set
            {
                extensionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PIN
        {
            get
            {
                return pINField;
            }
            set
            {
                pINField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Remark
        {
            get
            {
                return remarkField;
            }
            set
            {
                remarkField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool FormattedInd
        {
            get
            {
                return formattedIndField;
            }
            set
            {
                formattedIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool FormattedIndSpecified
        {
            get
            {
                return formattedIndFieldSpecified;
            }
            set
            {
                formattedIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum OffLocationServiceTypeTelephoneShareSynchInd
    {

        /// <remarks/>
        Yes,

        /// <remarks/>
        No,

        /// <remarks/>
        Inherit
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum OffLocationServiceTypeTelephoneShareMarketInd
    {

        /// <remarks/>
        Yes,

        /// <remarks/>
        No,

        /// <remarks/>
        Inherit
    }

    /// <remarks/>
    [XmlInclude(typeof(OffLocationServiceType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class OffLocationServiceCoreType
    {

        private OffLocationServiceCoreTypeAddress addressField;

        private OffLocationServiceID_Type typeField;

        /// <remarks/>
        public OffLocationServiceCoreTypeAddress Address
        {
            get
            {
                return addressField;
            }
            set
            {
                addressField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public OffLocationServiceID_Type Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class OffLocationServiceCoreTypeAddress : AddressType
    {

        private string siteIDField;

        private string siteNameField;

        /// <remarks/>
        [XmlAttribute()]
        public string SiteID
        {
            get
            {
                return siteIDField;
            }
            set
            {
                siteIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string SiteName
        {
            get
            {
                return siteNameField;
            }
            set
            {
                siteNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum OffLocationServiceID_Type
    {

        /// <remarks/>
        CustPickUp,

        /// <remarks/>
        VehDelivery,

        /// <remarks/>
        CustDropOff,

        /// <remarks/>
        VehCollection,

        /// <remarks/>
        Exchange,

        /// <remarks/>
        RepairLocation
    }

    /// <remarks/>
    [XmlInclude(typeof(VehicleLocationInformationType))]
    [XmlInclude(typeof(VendorMessageType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class FormattedTextType
    {

        private FormattedTextSubSectionType[] subSectionField;

        private object dummyElementField;

        private string titleField;

        private string languageField;

        /// <remarks/>
        [XmlElement("SubSection")]
        public FormattedTextSubSectionType[] SubSection
        {
            get
            {
                return subSectionField;
            }
            set
            {
                subSectionField = value;
            }
        }

        /// <remarks/>
        public object DummyElement
        {
            get
            {
                return dummyElementField;
            }
            set
            {
                dummyElementField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Title
        {
            get
            {
                return titleField;
            }
            set
            {
                titleField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "language")]
        public string Language
        {
            get
            {
                return languageField;
            }
            set
            {
                languageField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class FormattedTextSubSectionType
    {

        private ParagraphType[] paragraphField;

        private object dummyElementField;

        private string subTitleField;

        private string subCodeField;

        private string subSectionNumberField;

        /// <remarks/>
        [XmlElement("Paragraph")]
        public ParagraphType[] Paragraph
        {
            get
            {
                return paragraphField;
            }
            set
            {
                paragraphField = value;
            }
        }

        /// <remarks/>
        public object DummyElement
        {
            get
            {
                return dummyElementField;
            }
            set
            {
                dummyElementField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string SubTitle
        {
            get
            {
                return subTitleField;
            }
            set
            {
                subTitleField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string SubCode
        {
            get
            {
                return subCodeField;
            }
            set
            {
                subCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string SubSectionNumber
        {
            get
            {
                return subSectionNumberField;
            }
            set
            {
                subSectionNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleLocationInformationType : FormattedTextType
    {

        private string typeField;

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class VendorMessageType : FormattedTextType
    {

        /// <remarks/>
        [XmlAttribute()]
        public string InfoType { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class VehicleLocationDetailsType
    {

        /// <remarks/>
        [XmlElement("Address")]
        public AddressInfoType[] Address { get; set; }

        /// <remarks/>
        [XmlElement("Telephone")]
        public VehicleLocationDetailsTypeTelephone[] Telephone { get; set; }

        /// <remarks/>
        public VehicleLocationAdditionalDetailsType AdditionalInfo { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public bool AtAirport { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool AtAirportSpecified { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Code { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string CodeContext { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string ExtendedLocationCode { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string[] AssocAirportLocList { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class VehicleLocationDetailsTypeTelephone
    {

        private CustomerTypeTelephoneShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private CustomerTypeTelephoneShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string phoneLocationTypeField;

        private string phoneTechTypeField;

        private string phoneUseTypeField;

        private string countryAccessCodeField;

        private string areaCityCodeField;

        private string phoneNumberField;

        private string extensionField;

        private string pINField;

        private string remarkField;

        private bool formattedIndField;

        private bool formattedIndFieldSpecified;

        private bool defaultIndField;

        private bool defaultIndFieldSpecified;

        private string rPHField;

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeTelephoneShareSynchInd ShareSynchInd
        {
            get
            {
                return shareSynchIndField;
            }
            set
            {
                shareSynchIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified
        {
            get
            {
                return shareSynchIndFieldSpecified;
            }
            set
            {
                shareSynchIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public CustomerTypeTelephoneShareMarketInd ShareMarketInd
        {
            get
            {
                return shareMarketIndField;
            }
            set
            {
                shareMarketIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified
        {
            get
            {
                return shareMarketIndFieldSpecified;
            }
            set
            {
                shareMarketIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneLocationType
        {
            get
            {
                return phoneLocationTypeField;
            }
            set
            {
                phoneLocationTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneTechType
        {
            get
            {
                return phoneTechTypeField;
            }
            set
            {
                phoneTechTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneUseType
        {
            get
            {
                return phoneUseTypeField;
            }
            set
            {
                phoneUseTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CountryAccessCode
        {
            get
            {
                return countryAccessCodeField;
            }
            set
            {
                countryAccessCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string AreaCityCode
        {
            get
            {
                return areaCityCodeField;
            }
            set
            {
                areaCityCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PhoneNumber
        {
            get
            {
                return phoneNumberField;
            }
            set
            {
                phoneNumberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Extension
        {
            get
            {
                return extensionField;
            }
            set
            {
                extensionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PIN
        {
            get
            {
                return pINField;
            }
            set
            {
                pINField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Remark
        {
            get
            {
                return remarkField;
            }
            set
            {
                remarkField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool FormattedInd
        {
            get
            {
                return formattedIndField;
            }
            set
            {
                formattedIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool FormattedIndSpecified
        {
            get
            {
                return formattedIndFieldSpecified;
            }
            set
            {
                formattedIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool DefaultInd
        {
            get
            {
                return defaultIndField;
            }
            set
            {
                defaultIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DefaultIndSpecified
        {
            get
            {
                return defaultIndFieldSpecified;
            }
            set
            {
                defaultIndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH
        {
            get
            {
                return rPHField;
            }
            set
            {
                rPHField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleLocationAdditionalDetailsType
    {

        private VehicleLocationInformationType[] vehRentLocInfosField;

        private VehicleWhereAtFacilityType parkLocationField;

        private VehicleWhereAtFacilityType counterLocationField;

        private OperationSchedulesType operationSchedulesField;

        private VehicleLocationAdditionalDetailsTypeShuttle shuttleField;

        private VehicleLocationAdditionalDetailsTypeOneWayDropLocation[] oneWayDropLocationsField;

        private TPA_ExtensionsType tPA_ExtensionsField;

        /// <remarks/>
        [XmlArrayItem("VehRentLocInfo", IsNullable = false)]
        public VehicleLocationInformationType[] VehRentLocInfos
        {
            get
            {
                return vehRentLocInfosField;
            }
            set
            {
                vehRentLocInfosField = value;
            }
        }

        /// <remarks/>
        public VehicleWhereAtFacilityType ParkLocation
        {
            get
            {
                return parkLocationField;
            }
            set
            {
                parkLocationField = value;
            }
        }

        /// <remarks/>
        public VehicleWhereAtFacilityType CounterLocation
        {
            get
            {
                return counterLocationField;
            }
            set
            {
                counterLocationField = value;
            }
        }

        /// <remarks/>
        public OperationSchedulesType OperationSchedules
        {
            get
            {
                return operationSchedulesField;
            }
            set
            {
                operationSchedulesField = value;
            }
        }

        /// <remarks/>
        public VehicleLocationAdditionalDetailsTypeShuttle Shuttle
        {
            get
            {
                return shuttleField;
            }
            set
            {
                shuttleField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("OneWayDropLocation", IsNullable = false)]
        public VehicleLocationAdditionalDetailsTypeOneWayDropLocation[] OneWayDropLocations
        {
            get
            {
                return oneWayDropLocationsField;
            }
            set
            {
                oneWayDropLocationsField = value;
            }
        }

        /// <remarks/>
        public TPA_ExtensionsType TPA_Extensions
        {
            get
            {
                return tPA_ExtensionsField;
            }
            set
            {
                tPA_ExtensionsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleWhereAtFacilityType
    {

        private string locationField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string Location
        {
            get
            {
                return locationField;
            }
            set
            {
                locationField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class OperationSchedulesType
    {

        private OperationScheduleType[] operationScheduleField;

        private string startField;

        private string durationField;

        private string endField;

        /// <remarks/>
        [XmlElement("OperationSchedule")]
        public OperationScheduleType[] OperationSchedule
        {
            get
            {
                return operationScheduleField;
            }
            set
            {
                operationScheduleField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Start
        {
            get
            {
                return startField;
            }
            set
            {
                startField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Duration
        {
            get
            {
                return durationField;
            }
            set
            {
                durationField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string End
        {
            get
            {
                return endField;
            }
            set
            {
                endField = value;
            }
        }
    }

    /// <remarks/>
    [XmlInclude(typeof(PeriodPriceType))]
    [XmlInclude(typeof(OperationSchedulePlusChargeType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class OperationScheduleType
    {

        private OperationScheduleTypeOperationTime[] operationTimesField;

        private string startField;

        private string durationField;

        private string endField;

        /// <remarks/>
        [XmlArrayItem("OperationTime", IsNullable = false)]
        public OperationScheduleTypeOperationTime[] OperationTimes
        {
            get
            {
                return operationTimesField;
            }
            set
            {
                operationTimesField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Start
        {
            get
            {
                return startField;
            }
            set
            {
                startField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Duration
        {
            get
            {
                return durationField;
            }
            set
            {
                durationField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string End
        {
            get
            {
                return endField;
            }
            set
            {
                endField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class OperationScheduleTypeOperationTime
    {

        private bool monField;

        private bool monFieldSpecified;

        private bool tueField;

        private bool tueFieldSpecified;

        private bool wedsField;

        private bool wedsFieldSpecified;

        private bool thurField;

        private bool thurFieldSpecified;

        private bool friField;

        private bool friFieldSpecified;

        private bool satField;

        private bool satFieldSpecified;

        private bool sunField;

        private bool sunFieldSpecified;

        private string startField;

        private string durationField;

        private string endField;

        private string additionalOperationInfoCodeField;

        private string frequencyField;

        private string textField;

        /// <remarks/>
        [XmlAttribute()]
        public bool Mon
        {
            get
            {
                return monField;
            }
            set
            {
                monField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MonSpecified
        {
            get
            {
                return monFieldSpecified;
            }
            set
            {
                monFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool Tue
        {
            get
            {
                return tueField;
            }
            set
            {
                tueField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TueSpecified
        {
            get
            {
                return tueFieldSpecified;
            }
            set
            {
                tueFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool Weds
        {
            get
            {
                return wedsField;
            }
            set
            {
                wedsField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool WedsSpecified
        {
            get
            {
                return wedsFieldSpecified;
            }
            set
            {
                wedsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool Thur
        {
            get
            {
                return thurField;
            }
            set
            {
                thurField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ThurSpecified
        {
            get
            {
                return thurFieldSpecified;
            }
            set
            {
                thurFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool Fri
        {
            get
            {
                return friField;
            }
            set
            {
                friField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool FriSpecified
        {
            get
            {
                return friFieldSpecified;
            }
            set
            {
                friFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool Sat
        {
            get
            {
                return satField;
            }
            set
            {
                satField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool SatSpecified
        {
            get
            {
                return satFieldSpecified;
            }
            set
            {
                satFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool Sun
        {
            get
            {
                return sunField;
            }
            set
            {
                sunField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool SunSpecified
        {
            get
            {
                return sunFieldSpecified;
            }
            set
            {
                sunFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Start
        {
            get
            {
                return startField;
            }
            set
            {
                startField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Duration
        {
            get
            {
                return durationField;
            }
            set
            {
                durationField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string End
        {
            get
            {
                return endField;
            }
            set
            {
                endField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string AdditionalOperationInfoCode
        {
            get
            {
                return additionalOperationInfoCodeField;
            }
            set
            {
                additionalOperationInfoCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Frequency
        {
            get
            {
                return frequencyField;
            }
            set
            {
                frequencyField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PeriodPriceType : OperationScheduleType
    {

        private PkgPriceType[] priceField;

        private string rPHField;

        private PeriodPriceTypeCategory categoryField;

        private bool categoryFieldSpecified;

        private PeriodPriceTypeType typeField;

        private bool typeFieldSpecified;

        private string durationPeriodField;

        private PricingType priceBasisField;

        private bool priceBasisFieldSpecified;

        private string[] basePeriodRPHsField;

        private bool guidePriceIndicatorField;

        private bool guidePriceIndicatorFieldSpecified;

        private string maximumPeriodField;

        /// <remarks/>
        [XmlElement("Price")]
        public PkgPriceType[] Price
        {
            get
            {
                return priceField;
            }
            set
            {
                priceField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH
        {
            get
            {
                return rPHField;
            }
            set
            {
                rPHField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PeriodPriceTypeCategory Category
        {
            get
            {
                return categoryField;
            }
            set
            {
                categoryField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool CategorySpecified
        {
            get
            {
                return categoryFieldSpecified;
            }
            set
            {
                categoryFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PeriodPriceTypeType Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TypeSpecified
        {
            get
            {
                return typeFieldSpecified;
            }
            set
            {
                typeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DurationPeriod
        {
            get
            {
                return durationPeriodField;
            }
            set
            {
                durationPeriodField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PricingType PriceBasis
        {
            get
            {
                return priceBasisField;
            }
            set
            {
                priceBasisField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PriceBasisSpecified
        {
            get
            {
                return priceBasisFieldSpecified;
            }
            set
            {
                priceBasisFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string[] BasePeriodRPHs
        {
            get
            {
                return basePeriodRPHsField;
            }
            set
            {
                basePeriodRPHsField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool GuidePriceIndicator
        {
            get
            {
                return guidePriceIndicatorField;
            }
            set
            {
                guidePriceIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool GuidePriceIndicatorSpecified
        {
            get
            {
                return guidePriceIndicatorFieldSpecified;
            }
            set
            {
                guidePriceIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string MaximumPeriod
        {
            get
            {
                return maximumPeriodField;
            }
            set
            {
                maximumPeriodField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class PkgPriceType
    {

        private string ageField;

        private string codeField;

        private string codeContextField;

        private string uRIField;

        private long quantityField;

        private bool quantityFieldSpecified;

        private decimal amountField;

        private bool amountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        private PricingType priceBasisField;

        private bool priceBasisFieldSpecified;

        /// <remarks/>
        [XmlAttribute(DataType = "integer")]
        public string Age
        {
            get
            {
                return ageField;
            }
            set
            {
                ageField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CodeContext
        {
            get
            {
                return codeContextField;
            }
            set
            {
                codeContextField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string URI
        {
            get
            {
                return uRIField;
            }
            set
            {
                uRIField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long Quantity
        {
            get
            {
                return quantityField;
            }
            set
            {
                quantityField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool QuantitySpecified
        {
            get
            {
                return quantityFieldSpecified;
            }
            set
            {
                quantityFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified
        {
            get
            {
                return amountFieldSpecified;
            }
            set
            {
                amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PricingType PriceBasis
        {
            get
            {
                return priceBasisField;
            }
            set
            {
                priceBasisField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PriceBasisSpecified
        {
            get
            {
                return priceBasisFieldSpecified;
            }
            set
            {
                priceBasisFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum PricingType
    {

        /// <remarks/>
        [XmlEnum("Per stay")]
        Perstay,

        /// <remarks/>
        [XmlEnum("Per person")]
        Perperson,

        /// <remarks/>
        [XmlEnum("Per night")]
        Pernight,

        /// <remarks/>
        [XmlEnum("Per person per night")]
        Perpersonpernight,

        /// <remarks/>
        [XmlEnum("Per use")]
        Peruse
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum PeriodPriceTypeCategory
    {

        /// <remarks/>
        Room,

        /// <remarks/>
        Booking,

        /// <remarks/>
        Person,

        /// <remarks/>
        Adult,

        /// <remarks/>
        Child,

        /// <remarks/>
        Car
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum PeriodPriceTypeType
    {

        /// <remarks/>
        Base,

        /// <remarks/>
        AddOn
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class OperationSchedulePlusChargeType : OperationScheduleType
    {

        private FeeType[] chargeField;

        /// <remarks/>
        [XmlElement("Charge")]
        public FeeType[] Charge
        {
            get
            {
                return chargeField;
            }
            set
            {
                chargeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class FeeType
    {

        private TaxesType taxesField;

        private ParagraphType[] descriptionField;

        private bool taxInclusiveField;

        private bool taxInclusiveFieldSpecified;

        private AmountDeterminationType typeField;

        private bool typeFieldSpecified;

        private string codeField;

        private decimal percentField;

        private bool percentFieldSpecified;

        private decimal amountField;

        private bool amountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        private DateTime effectiveDateField;

        private bool effectiveDateFieldSpecified;

        private DateTime expireDateField;

        private bool expireDateFieldSpecified;

        private bool expireDateExclusiveIndicatorField;

        private bool expireDateExclusiveIndicatorFieldSpecified;

        private bool mandatoryIndicatorField;

        private bool mandatoryIndicatorFieldSpecified;

        private string rPHField;

        private string chargeUnitField;

        private string chargeFrequencyField;

        private long chargeUnitExemptField;

        private bool chargeUnitExemptFieldSpecified;

        private long chargeFrequencyExemptField;

        private bool chargeFrequencyExemptFieldSpecified;

        private long maxChargeUnitAppliesField;

        private bool maxChargeUnitAppliesFieldSpecified;

        private long maxChargeFrequencyAppliesField;

        private bool maxChargeFrequencyAppliesFieldSpecified;

        private bool taxableIndicatorField;

        private bool taxableIndicatorFieldSpecified;

        /// <remarks/>
        public TaxesType Taxes
        {
            get
            {
                return taxesField;
            }
            set
            {
                taxesField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Description")]
        public ParagraphType[] Description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool TaxInclusive
        {
            get
            {
                return taxInclusiveField;
            }
            set
            {
                taxInclusiveField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TaxInclusiveSpecified
        {
            get
            {
                return taxInclusiveFieldSpecified;
            }
            set
            {
                taxInclusiveFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public AmountDeterminationType Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TypeSpecified
        {
            get
            {
                return typeFieldSpecified;
            }
            set
            {
                typeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Percent
        {
            get
            {
                return percentField;
            }
            set
            {
                percentField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PercentSpecified
        {
            get
            {
                return percentFieldSpecified;
            }
            set
            {
                percentFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified
        {
            get
            {
                return amountFieldSpecified;
            }
            set
            {
                amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDateField;
            }
            set
            {
                effectiveDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified
        {
            get
            {
                return effectiveDateFieldSpecified;
            }
            set
            {
                effectiveDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate
        {
            get
            {
                return expireDateField;
            }
            set
            {
                expireDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified
        {
            get
            {
                return expireDateFieldSpecified;
            }
            set
            {
                expireDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator
        {
            get
            {
                return expireDateExclusiveIndicatorField;
            }
            set
            {
                expireDateExclusiveIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified
        {
            get
            {
                return expireDateExclusiveIndicatorFieldSpecified;
            }
            set
            {
                expireDateExclusiveIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool MandatoryIndicator
        {
            get
            {
                return mandatoryIndicatorField;
            }
            set
            {
                mandatoryIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MandatoryIndicatorSpecified
        {
            get
            {
                return mandatoryIndicatorFieldSpecified;
            }
            set
            {
                mandatoryIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RPH
        {
            get
            {
                return rPHField;
            }
            set
            {
                rPHField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ChargeUnit
        {
            get
            {
                return chargeUnitField;
            }
            set
            {
                chargeUnitField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ChargeFrequency
        {
            get
            {
                return chargeFrequencyField;
            }
            set
            {
                chargeFrequencyField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long ChargeUnitExempt
        {
            get
            {
                return chargeUnitExemptField;
            }
            set
            {
                chargeUnitExemptField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ChargeUnitExemptSpecified
        {
            get
            {
                return chargeUnitExemptFieldSpecified;
            }
            set
            {
                chargeUnitExemptFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long ChargeFrequencyExempt
        {
            get
            {
                return chargeFrequencyExemptField;
            }
            set
            {
                chargeFrequencyExemptField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ChargeFrequencyExemptSpecified
        {
            get
            {
                return chargeFrequencyExemptFieldSpecified;
            }
            set
            {
                chargeFrequencyExemptFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long MaxChargeUnitApplies
        {
            get
            {
                return maxChargeUnitAppliesField;
            }
            set
            {
                maxChargeUnitAppliesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MaxChargeUnitAppliesSpecified
        {
            get
            {
                return maxChargeUnitAppliesFieldSpecified;
            }
            set
            {
                maxChargeUnitAppliesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long MaxChargeFrequencyApplies
        {
            get
            {
                return maxChargeFrequencyAppliesField;
            }
            set
            {
                maxChargeFrequencyAppliesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MaxChargeFrequencyAppliesSpecified
        {
            get
            {
                return maxChargeFrequencyAppliesFieldSpecified;
            }
            set
            {
                maxChargeFrequencyAppliesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool TaxableIndicator
        {
            get
            {
                return taxableIndicatorField;
            }
            set
            {
                taxableIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TaxableIndicatorSpecified
        {
            get
            {
                return taxableIndicatorFieldSpecified;
            }
            set
            {
                taxableIndicatorFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class TaxesType
    {

        private TaxType[] taxField;

        private decimal amountField;

        private bool amountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        /// <remarks/>
        [XmlElement("Tax")]
        public TaxType[] Tax
        {
            get
            {
                return taxField;
            }
            set
            {
                taxField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified
        {
            get
            {
                return amountFieldSpecified;
            }
            set
            {
                amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class TaxType
    {

        private ParagraphType[] taxDescriptionField;

        private object dummyElementField;

        private AmountDeterminationType typeField;

        private bool typeFieldSpecified;

        private string codeField;

        private decimal percentField;

        private bool percentFieldSpecified;

        private decimal amountField;

        private bool amountFieldSpecified;

        private string currencyCodeField;

        private long decimalPlacesField;

        private bool decimalPlacesFieldSpecified;

        private DateTime effectiveDateField;

        private bool effectiveDateFieldSpecified;

        private DateTime expireDateField;

        private bool expireDateFieldSpecified;

        private bool expireDateExclusiveIndicatorField;

        private bool expireDateExclusiveIndicatorFieldSpecified;

        private string chargeUnitField;

        private string chargeFrequencyField;

        private long chargeUnitExemptField;

        private bool chargeUnitExemptFieldSpecified;

        private long chargeFrequencyExemptField;

        private bool chargeFrequencyExemptFieldSpecified;

        private long maxChargeUnitAppliesField;

        private bool maxChargeUnitAppliesFieldSpecified;

        private long maxChargeFrequencyAppliesField;

        private bool maxChargeFrequencyAppliesFieldSpecified;

        /// <remarks/>
        [XmlElement("TaxDescription")]
        public ParagraphType[] TaxDescription
        {
            get
            {
                return taxDescriptionField;
            }
            set
            {
                taxDescriptionField = value;
            }
        }

        /// <remarks/>
        public object DummyElement
        {
            get
            {
                return dummyElementField;
            }
            set
            {
                dummyElementField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public AmountDeterminationType Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool TypeSpecified
        {
            get
            {
                return typeFieldSpecified;
            }
            set
            {
                typeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Percent
        {
            get
            {
                return percentField;
            }
            set
            {
                percentField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PercentSpecified
        {
            get
            {
                return percentFieldSpecified;
            }
            set
            {
                percentFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public decimal Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AmountSpecified
        {
            get
            {
                return amountFieldSpecified;
            }
            set
            {
                amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DecimalPlacesSpecified
        {
            get
            {
                return decimalPlacesFieldSpecified;
            }
            set
            {
                decimalPlacesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDateField;
            }
            set
            {
                effectiveDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified
        {
            get
            {
                return effectiveDateFieldSpecified;
            }
            set
            {
                effectiveDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate
        {
            get
            {
                return expireDateField;
            }
            set
            {
                expireDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified
        {
            get
            {
                return expireDateFieldSpecified;
            }
            set
            {
                expireDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ExpireDateExclusiveIndicator
        {
            get
            {
                return expireDateExclusiveIndicatorField;
            }
            set
            {
                expireDateExclusiveIndicatorField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ExpireDateExclusiveIndicatorSpecified
        {
            get
            {
                return expireDateExclusiveIndicatorFieldSpecified;
            }
            set
            {
                expireDateExclusiveIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ChargeUnit
        {
            get
            {
                return chargeUnitField;
            }
            set
            {
                chargeUnitField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ChargeFrequency
        {
            get
            {
                return chargeFrequencyField;
            }
            set
            {
                chargeFrequencyField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long ChargeUnitExempt
        {
            get
            {
                return chargeUnitExemptField;
            }
            set
            {
                chargeUnitExemptField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ChargeUnitExemptSpecified
        {
            get
            {
                return chargeUnitExemptFieldSpecified;
            }
            set
            {
                chargeUnitExemptFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long ChargeFrequencyExempt
        {
            get
            {
                return chargeFrequencyExemptField;
            }
            set
            {
                chargeFrequencyExemptField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ChargeFrequencyExemptSpecified
        {
            get
            {
                return chargeFrequencyExemptFieldSpecified;
            }
            set
            {
                chargeFrequencyExemptFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long MaxChargeUnitApplies
        {
            get
            {
                return maxChargeUnitAppliesField;
            }
            set
            {
                maxChargeUnitAppliesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MaxChargeUnitAppliesSpecified
        {
            get
            {
                return maxChargeUnitAppliesFieldSpecified;
            }
            set
            {
                maxChargeUnitAppliesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long MaxChargeFrequencyApplies
        {
            get
            {
                return maxChargeFrequencyAppliesField;
            }
            set
            {
                maxChargeFrequencyAppliesField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MaxChargeFrequencyAppliesSpecified
        {
            get
            {
                return maxChargeFrequencyAppliesFieldSpecified;
            }
            set
            {
                maxChargeFrequencyAppliesFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum AmountDeterminationType
    {

        /// <remarks/>
        Inclusive,

        /// <remarks/>
        Exclusive,

        /// <remarks/>
        Cumulative
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleLocationAdditionalDetailsTypeShuttle
    {

        private VehicleLocationAdditionalDetailsTypeShuttleShuttleInfo[] shuttleInfosField;

        private OperationSchedulesType operationSchedulesField;

        /// <remarks/>
        [XmlArrayItem("ShuttleInfo", IsNullable = false)]
        public VehicleLocationAdditionalDetailsTypeShuttleShuttleInfo[] ShuttleInfos
        {
            get
            {
                return shuttleInfosField;
            }
            set
            {
                shuttleInfosField = value;
            }
        }

        /// <remarks/>
        public OperationSchedulesType OperationSchedules
        {
            get
            {
                return operationSchedulesField;
            }
            set
            {
                operationSchedulesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleLocationAdditionalDetailsTypeShuttleShuttleInfo : FormattedTextType
    {

        private LocationDetailShuttleInfoType typeField;

        /// <remarks/>
        [XmlAttribute()]
        public LocationDetailShuttleInfoType Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum LocationDetailShuttleInfoType
    {

        /// <remarks/>
        Transportation,

        /// <remarks/>
        Frequency,

        /// <remarks/>
        PickupInfo,

        /// <remarks/>
        Distance,

        /// <remarks/>
        ElapsedTime,

        /// <remarks/>
        Fee,

        /// <remarks/>
        Miscellaneous,

        /// <remarks/>
        Hours
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleLocationAdditionalDetailsTypeOneWayDropLocation : LocationType
    {

        private string extendedLocationCodeField;

        /// <remarks/>
        [XmlAttribute()]
        public string ExtendedLocationCode
        {
            get
            {
                return extendedLocationCodeField;
            }
            set
            {
                extendedLocationCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleTourInfoType
    {

        private CompanyNameType tourOperatorField;

        private string tourNumberField;

        /// <remarks/>
        public CompanyNameType TourOperator
        {
            get
            {
                return tourOperatorField;
            }
            set
            {
                tourOperatorField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TourNumber
        {
            get
            {
                return tourNumberField;
            }
            set
            {
                tourNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleSpecialReqPrefType
    {

        private PreferLevelType preferLevelField;

        private bool preferLevelFieldSpecified;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel
        {
            get
            {
                return preferLevelField;
            }
            set
            {
                preferLevelField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified
        {
            get
            {
                return preferLevelFieldSpecified;
            }
            set
            {
                preferLevelFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class VehicleArrivalDetailsType
    {

        private LocationType arrivalLocationField;

        private CompanyNameType marketingCompanyField;

        private CompanyNameType operatingCompanyField;

        private string transportationCodeField;

        private string numberField;

        private DateTime arrivalDateTimeField;

        private bool arrivalDateTimeFieldSpecified;

        /// <remarks/>
        public LocationType ArrivalLocation
        {
            get
            {
                return arrivalLocationField;
            }
            set
            {
                arrivalLocationField = value;
            }
        }

        /// <remarks/>
        public CompanyNameType MarketingCompany
        {
            get
            {
                return marketingCompanyField;
            }
            set
            {
                marketingCompanyField = value;
            }
        }

        /// <remarks/>
        public CompanyNameType OperatingCompany
        {
            get
            {
                return operatingCompanyField;
            }
            set
            {
                operatingCompanyField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TransportationCode
        {
            get
            {
                return transportationCodeField;
            }
            set
            {
                transportationCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Number
        {
            get
            {
                return numberField;
            }
            set
            {
                numberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DateTime ArrivalDateTime
        {
            get
            {
                return arrivalDateTimeField;
            }
            set
            {
                arrivalDateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ArrivalDateTimeSpecified
        {
            get
            {
                return arrivalDateTimeFieldSpecified;
            }
            set
            {
                arrivalDateTimeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class WrittenConfInstType
    {

        private ParagraphType supplementalDataField;

        private EmailType emailField;

        private string languageIDField;

        private string addresseeNameField;

        private string addressField;

        private string telephoneField;

        private bool confirmIndField;

        private bool confirmIndFieldSpecified;

        /// <remarks/>
        public ParagraphType SupplementalData
        {
            get
            {
                return supplementalDataField;
            }
            set
            {
                supplementalDataField = value;
            }
        }

        /// <remarks/>
        public EmailType Email
        {
            get
            {
                return emailField;
            }
            set
            {
                emailField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string LanguageID
        {
            get
            {
                return languageIDField;
            }
            set
            {
                languageIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string AddresseeName
        {
            get
            {
                return addresseeNameField;
            }
            set
            {
                addresseeNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Address
        {
            get
            {
                return addressField;
            }
            set
            {
                addressField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Telephone
        {
            get
            {
                return telephoneField;
            }
            set
            {
                telephoneField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ConfirmInd
        {
            get
            {
                return confirmIndField;
            }
            set
            {
                confirmIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ConfirmIndSpecified
        {
            get
            {
                return confirmIndFieldSpecified;
            }
            set
            {
                confirmIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    [XmlRoot("AccommodationCategory", Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable = false)]
    public partial class AccommodationCategoryType
    {

        private AccommodationCategoryTypeAccommodation[] accommodationField;

        private AncillaryService[] ancillaryServiceField;

        /// <remarks/>
        [XmlElement("Accommodation")]
        public AccommodationCategoryTypeAccommodation[] Accommodation
        {
            get
            {
                return accommodationField;
            }
            set
            {
                accommodationField = value;
            }
        }

        /// <remarks/>
        [XmlElement("AncillaryService")]
        public AncillaryService[] AncillaryService
        {
            get
            {
                return ancillaryServiceField;
            }
            set
            {
                ancillaryServiceField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class AccommodationCategoryTypeAccommodation : AccommodationType
    {

        private long quantityField;

        private bool quantityFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public long Quantity
        {
            get
            {
                return quantityField;
            }
            set
            {
                quantityField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool QuantitySpecified
        {
            get
            {
                return quantityFieldSpecified;
            }
            set
            {
                quantityFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class AccommodationType
    {

        private object itemField;

        private AccommodationClass classField;

        private CompartmentType compartmentField;

        /// <remarks/>
        [XmlElement("Berth", typeof(BerthAccommodationType))]
        [XmlElement("Seat", typeof(SeatAccommodationType))]
        public object Item
        {
            get
            {
                return itemField;
            }
            set
            {
                itemField = value;
            }
        }

        /// <remarks/>
        public AccommodationClass Class
        {
            get
            {
                return classField;
            }
            set
            {
                classField = value;
            }
        }

        /// <remarks/>
        public CompartmentType Compartment
        {
            get
            {
                return compartmentField;
            }
            set
            {
                compartmentField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum BerthAccommodationType
    {

        /// <remarks/>
        NotSignificant,

        /// <remarks/>
        Berth,

        /// <remarks/>
        Couchette,

        /// <remarks/>
        Sleeper
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum SeatAccommodationType
    {

        /// <remarks/>
        NotSignificant,

        /// <remarks/>
        Seat,

        /// <remarks/>
        Sleeperette,

        /// <remarks/>
        NoSeat
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class AccommodationClass
    {

        private string extensionField;

        private AccommodationClassEnum valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string @extension
        {
            get
            {
                return extensionField;
            }
            set
            {
                extensionField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public AccommodationClassEnum Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum AccommodationClassEnum
    {

        /// <remarks/>
        FirstClass,

        /// <remarks/>
        SecondClass,

        /// <remarks/>
        Premium,

        /// <remarks/>
        Business,

        /// <remarks/>
        Leisure,

        /// <remarks/>
        Coach,

        /// <remarks/>
        Deluxe,

        /// <remarks/>
        GranClasse,

        /// <remarks/>
        SoftClass,

        /// <remarks/>
        HardClass,

        /// <remarks/>
        SpecialClass,

        /// <remarks/>
        HighGradeSoftClass,

        /// <remarks/>
        MixedHardClass,

        /// <remarks/>
        MixedSoftClass,

        /// <remarks/>
        SoftCompartmentClass,

        /// <remarks/>
        HardCompartmentClass,

        /// <remarks/>
        Other_
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class CompartmentType
    {

        private string extensionField;

        private CompartmentTypeEnum valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string @extension
        {
            get
            {
                return extensionField;
            }
            set
            {
                extensionField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public CompartmentTypeEnum Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum CompartmentTypeEnum
    {

        /// <remarks/>
        NotSignificant,

        /// <remarks/>
        Family,

        /// <remarks/>
        Quite,

        /// <remarks/>
        Conference,

        /// <remarks/>
        CompartmentWithoutAnimals,

        /// <remarks/>
        Complete,

        /// <remarks/>
        Video,

        /// <remarks/>
        Pram,

        /// <remarks/>
        WomanAndChild,

        /// <remarks/>
        EasyAccess,

        /// <remarks/>
        T2,

        /// <remarks/>
        T3,

        /// <remarks/>
        T4,

        /// <remarks/>
        T6,

        /// <remarks/>
        C2,

        /// <remarks/>
        C4,

        /// <remarks/>
        C5,

        /// <remarks/>
        C6,

        /// <remarks/>
        Single,

        /// <remarks/>
        Double,

        /// <remarks/>
        SingleSuite,

        /// <remarks/>
        DoubleSuite,

        /// <remarks/>
        Special,

        /// <remarks/>
        Other_
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    [XmlRoot(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable = false)]
    public partial class AncillaryService : AncillaryServiceType
    {

        private PreferLevelType preferLevelField;

        private bool preferLevelFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public PreferLevelType PreferLevel
        {
            get
            {
                return preferLevelField;
            }
            set
            {
                preferLevelField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PreferLevelSpecified
        {
            get
            {
                return preferLevelFieldSpecified;
            }
            set
            {
                preferLevelFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class AncillaryServiceType
    {

        private string codeField;

        private string codeContextField;

        private long quantityField;

        private bool quantityFieldSpecified;

        private string descriptionField;

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CodeContext
        {
            get
            {
                return codeContextField;
            }
            set
            {
                codeContextField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long Quantity
        {
            get
            {
                return quantityField;
            }
            set
            {
                quantityField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool QuantitySpecified
        {
            get
            {
                return quantityFieldSpecified;
            }
            set
            {
                quantityFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    [XmlRoot("AccommodationService", Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable = false)]
    public partial class AccommodationServiceType
    {

        private AccommodationServiceTypeAccommodationDetail accommodationDetailField;

        private AncillaryService[] ancillaryServiceField;

        /// <remarks/>
        public AccommodationServiceTypeAccommodationDetail AccommodationDetail
        {
            get
            {
                return accommodationDetailField;
            }
            set
            {
                accommodationDetailField = value;
            }
        }

        /// <remarks/>
        [XmlElement("AncillaryService")]
        public AncillaryService[] AncillaryService
        {
            get
            {
                return ancillaryServiceField;
            }
            set
            {
                ancillaryServiceField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class AccommodationServiceTypeAccommodationDetail : RailAccommDetailType
    {

        private string referenceTravelerRPHField;

        private bool referenceIndField;

        private bool referenceIndFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public string ReferenceTravelerRPH
        {
            get
            {
                return referenceTravelerRPHField;
            }
            set
            {
                referenceTravelerRPHField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public bool ReferenceInd
        {
            get
            {
                return referenceIndField;
            }
            set
            {
                referenceIndField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool ReferenceIndSpecified
        {
            get
            {
                return referenceIndFieldSpecified;
            }
            set
            {
                referenceIndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class RailAccommDetailType
    {

        private object itemField;

        private AccommodationClass classField;

        private RailAccommDetailTypeCompartment compartmentField;

        private RailAccommDetailTypeCar carField;

        private DeckType deckField;

        private bool deckFieldSpecified;

        /// <remarks/>
        [XmlElement("Berth", typeof(BerthDetailType))]
        [XmlElement("Seat", typeof(SeatDetailType))]
        public object Item
        {
            get
            {
                return itemField;
            }
            set
            {
                itemField = value;
            }
        }

        /// <remarks/>
        public AccommodationClass Class
        {
            get
            {
                return classField;
            }
            set
            {
                classField = value;
            }
        }

        /// <remarks/>
        public RailAccommDetailTypeCompartment Compartment
        {
            get
            {
                return compartmentField;
            }
            set
            {
                compartmentField = value;
            }
        }

        /// <remarks/>
        public RailAccommDetailTypeCar Car
        {
            get
            {
                return carField;
            }
            set
            {
                carField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public DeckType Deck
        {
            get
            {
                return deckField;
            }
            set
            {
                deckField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DeckSpecified
        {
            get
            {
                return deckFieldSpecified;
            }
            set
            {
                deckFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class BerthDetailType
    {

        private string numberField;

        private BerthPositionType positionField;

        private bool positionFieldSpecified;

        private BerthAccommodationType valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string Number
        {
            get
            {
                return numberField;
            }
            set
            {
                numberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public BerthPositionType Position
        {
            get
            {
                return positionField;
            }
            set
            {
                positionField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PositionSpecified
        {
            get
            {
                return positionFieldSpecified;
            }
            set
            {
                positionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public BerthAccommodationType Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum BerthPositionType
    {

        /// <remarks/>
        Upper,

        /// <remarks/>
        Middle,

        /// <remarks/>
        Lower
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class SeatDetailType
    {

        private string numberField;

        private SeatPositionType positionField;

        private bool positionFieldSpecified;

        private SeatDirectionType directionField;

        private bool directionFieldSpecified;

        private SeatAccommodationType valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string Number
        {
            get
            {
                return numberField;
            }
            set
            {
                numberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public SeatPositionType Position
        {
            get
            {
                return positionField;
            }
            set
            {
                positionField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PositionSpecified
        {
            get
            {
                return positionFieldSpecified;
            }
            set
            {
                positionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public SeatDirectionType Direction
        {
            get
            {
                return directionField;
            }
            set
            {
                directionField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DirectionSpecified
        {
            get
            {
                return directionFieldSpecified;
            }
            set
            {
                directionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public SeatAccommodationType Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum SeatPositionType
    {

        /// <remarks/>
        None,

        /// <remarks/>
        Together,

        /// <remarks/>
        Aisle,

        /// <remarks/>
        Center,

        /// <remarks/>
        Window,

        /// <remarks/>
        Specific,

        /// <remarks/>
        Exit,

        /// <remarks/>
        Table,

        /// <remarks/>
        AdjacentAisle,

        /// <remarks/>
        Individual,

        /// <remarks/>
        Middle
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum SeatDirectionType
    {

        /// <remarks/>
        Facing,

        /// <remarks/>
        Back,

        /// <remarks/>
        Airline,

        /// <remarks/>
        Lateral,

        /// <remarks/>
        Unknown
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class RailAccommDetailTypeCompartment : CompartmentType
    {

        private string numberField;

        private CompartmentPositionType positionField;

        private bool positionFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public string Number
        {
            get
            {
                return numberField;
            }
            set
            {
                numberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public CompartmentPositionType Position
        {
            get
            {
                return positionField;
            }
            set
            {
                positionField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool PositionSpecified
        {
            get
            {
                return positionFieldSpecified;
            }
            set
            {
                positionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum CompartmentPositionType
    {

        /// <remarks/>
        CloseToRestaurantCar,

        /// <remarks/>
        CloseToExit,

        /// <remarks/>
        CloseToToilet,

        /// <remarks/>
        MiddleOfCar
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public partial class RailAccommDetailTypeCar
    {

        private string numberField;

        /// <remarks/>
        [XmlAttribute()]
        public string Number
        {
            get
            {
                return numberField;
            }
            set
            {
                numberField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    public enum DeckType
    {

        /// <remarks/>
        [XmlEnum("Regular-OneLevelOnly")]
        RegularOneLevelOnly,

        /// <remarks/>
        LowerLevel,

        /// <remarks/>
        UpperLevel
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    [XmlRoot("Success", Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable = false)]
    public partial class SuccessType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(TypeName = "WarningsType", Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    [XmlRoot("Warnings", Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable = false)]
    public partial class WarningsType1
    {

        private WarningType1[] warningField;

        /// <remarks/>
        [XmlElement("Warning")]
        public WarningType1[] Warning
        {
            get
            {
                return warningField;
            }
            set
            {
                warningField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(TypeName = "ErrorsType", Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
    [XmlRoot("Errors", Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A", IsNullable = false)]
    public partial class ErrorsType1
    {

        private ErrorType1[] errorField;

        /// <remarks/>
        [XmlElement("Error")]
        public ErrorType1[] Error
        {
            get
            {
                return errorField;
            }
            set
            {
                errorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(TypeName = "WarningsType", Namespace = "http://www.iata.org/IATA/2007/00/IATA2010.1")]
    [XmlRoot("Warnings", Namespace = "http://www.iata.org/IATA/2007/00/IATA2010.1", IsNullable = false)]
    public partial class WarningsType2
    {

        private WarningType2[] warningField;

        /// <remarks/>
        [XmlElement("Warning")]
        public WarningType2[] Warning
        {
            get
            {
                return warningField;
            }
            set
            {
                warningField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(TypeName = "WarningType", Namespace = "http://www.iata.org/IATA/2007/00/IATA2010.1")]
    public partial class WarningType2 : FreeTextType1
    {

        private string typeField;

        private string shortTextField;

        private string codeField;

        private string docURLField;

        private string statusField;

        private string tagField;

        private string recordIDField;

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ShortText
        {
            get
            {
                return shortTextField;
            }
            set
            {
                shortTextField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string DocURL
        {
            get
            {
                return docURLField;
            }
            set
            {
                docURLField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Status
        {
            get
            {
                return statusField;
            }
            set
            {
                statusField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Tag
        {
            get
            {
                return tagField;
            }
            set
            {
                tagField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RecordID
        {
            get
            {
                return recordIDField;
            }
            set
            {
                recordIDField = value;
            }
        }
    }

    /// <remarks/>
    [XmlInclude(typeof(WarningType2))]
    [XmlInclude(typeof(ErrorType2))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(TypeName = "FreeTextType", Namespace = "http://www.iata.org/IATA/2007/00/IATA2010.1")]
    public partial class FreeTextType1
    {

        private string languageField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute(DataType = "language")]
        public string Language
        {
            get
            {
                return languageField;
            }
            set
            {
                languageField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(TypeName = "ErrorType", Namespace = "http://www.iata.org/IATA/2007/00/IATA2010.1")]
    public partial class ErrorType2 : FreeTextType1
    {

        private string typeField;

        private string shortTextField;

        private string codeField;

        private string docURLField;

        private string statusField;

        private string tagField;

        private string recordIDField;

        private string nodeListField;

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ShortText
        {
            get
            {
                return shortTextField;
            }
            set
            {
                shortTextField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string DocURL
        {
            get
            {
                return docURLField;
            }
            set
            {
                docURLField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Status
        {
            get
            {
                return statusField;
            }
            set
            {
                statusField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Tag
        {
            get
            {
                return tagField;
            }
            set
            {
                tagField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string RecordID
        {
            get
            {
                return recordIDField;
            }
            set
            {
                recordIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NodeList
        {
            get
            {
                return nodeListField;
            }
            set
            {
                nodeListField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(TypeName = "ErrorsType", Namespace = "http://www.iata.org/IATA/2007/00/IATA2010.1")]
    [XmlRoot("Errors", Namespace = "http://www.iata.org/IATA/2007/00/IATA2010.1", IsNullable = false)]
    public partial class ErrorsType2
    {

        private ErrorType2[] errorField;

        /// <remarks/>
        [XmlElement("Error")]
        public ErrorType2[] Error
        {
            get
            {
                return errorField;
            }
            set
            {
                errorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    [XmlRoot(Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2", IsNullable = false)]
    public partial class QtElement
    {

        private QuotationGenericElementTypePrice[] priceField;

        private QuotationGenericElementTypeTax[] taxField;

        private PointType[] pointField;

        private FullQuotationType_Type quotationTypeField;

        private FlagType[] flagField;

        private QuotationGenericElementTypePricingInfoState stateField;

        private bool stateFieldSpecified;

        /// <remarks/>
        [XmlElement("Price")]
        public QuotationGenericElementTypePrice[] Price
        {
            get
            {
                return priceField;
            }
            set
            {
                priceField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Tax")]
        public QuotationGenericElementTypeTax[] Tax
        {
            get
            {
                return taxField;
            }
            set
            {
                taxField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Point")]
        public PointType[] Point
        {
            get
            {
                return pointField;
            }
            set
            {
                pointField = value;
            }
        }

        /// <remarks/>
        public FullQuotationType_Type QuotationType
        {
            get
            {
                return quotationTypeField;
            }
            set
            {
                quotationTypeField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Flag")]
        public FlagType[] Flag
        {
            get
            {
                return flagField;
            }
            set
            {
                flagField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public QuotationGenericElementTypePricingInfoState State
        {
            get
            {
                return stateField;
            }
            set
            {
                stateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StateSpecified
        {
            get
            {
                return stateFieldSpecified;
            }
            set
            {
                stateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class QuotationGenericElementTypePrice
    {

        private string currencyCodeField;

        private string decimalPlacesField;

        private string amountField;

        private FareOrRateTypeMiscValue miscValueField;

        private bool miscValueFieldSpecified;

        private string typeField;

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public FareOrRateTypeMiscValue MiscValue
        {
            get
            {
                return miscValueField;
            }
            set
            {
                miscValueField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MiscValueSpecified
        {
            get
            {
                return miscValueFieldSpecified;
            }
            set
            {
                miscValueFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public enum FareOrRateTypeMiscValue
    {

        /// <remarks/>
        EXEMPTED,

        /// <remarks/>
        [XmlEnum("")]
        Item
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class QuotationGenericElementTypeTax
    {

        private string currencyCodeField;

        private string decimalPlacesField;

        private string amountField;

        private QuotationGenericElementTypeTaxMiscValue miscValueField;

        private bool miscValueFieldSpecified;

        private string typeField;

        private string isExemptedField;

        private string natureCodeField;

        private string indicatorField;

        private string isoCodeField;

        private string rateField;

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public QuotationGenericElementTypeTaxMiscValue MiscValue
        {
            get
            {
                return miscValueField;
            }
            set
            {
                miscValueField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool MiscValueSpecified
        {
            get
            {
                return miscValueFieldSpecified;
            }
            set
            {
                miscValueFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string IsExempted
        {
            get
            {
                return isExemptedField;
            }
            set
            {
                isExemptedField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NatureCode
        {
            get
            {
                return natureCodeField;
            }
            set
            {
                natureCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Indicator
        {
            get
            {
                return indicatorField;
            }
            set
            {
                indicatorField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string IsoCode
        {
            get
            {
                return isoCodeField;
            }
            set
            {
                isoCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Rate
        {
            get
            {
                return rateField;
            }
            set
            {
                rateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public enum QuotationGenericElementTypeTaxMiscValue
    {

        /// <remarks/>
        EXEMPTED,

        /// <remarks/>
        [XmlEnum("")]
        Item
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class PointType
    {

        private string typeField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlText(DataType = "integer")]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class FullQuotationType_Type
    {

        private FullQuotationType_TypeDescription descriptionField;

        private bool descriptionFieldSpecified;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public FullQuotationType_TypeDescription Description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool DescriptionSpecified
        {
            get
            {
                return descriptionFieldSpecified;
            }
            set
            {
                descriptionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlText(DataType = "nonNegativeInteger")]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public enum FullQuotationType_TypeDescription
    {

        /// <remarks/>
        QT_DEFAULT,

        /// <remarks/>
        QT_GEN,

        /// <remarks/>
        QT_PR,

        /// <remarks/>
        QT_DOC_PR,

        /// <remarks/>
        QT_ITI_PR,

        /// <remarks/>
        QT_ITI,

        /// <remarks/>
        QT_ATC_PR,

        /// <remarks/>
        QT_DOC,

        /// <remarks/>
        QT_OBF,

        /// <remarks/>
        QT_OBF_DET,

        /// <remarks/>
        QT_CPN,

        /// <remarks/>
        QT_PCK,

        /// <remarks/>
        QT_DOC_CPN,

        /// <remarks/>
        QT_ITI_CPN,

        /// <remarks/>
        QT_MRK,

        /// <remarks/>
        QT_REF,

        /// <remarks/>
        QT_EXT_PR,

        /// <remarks/>
        QT_ADN,

        /// <remarks/>
        QT_UNKNOWN,

        /// <remarks/>
        QT_FAR_COM,

        /// <remarks/>
        QT_USE,

        /// <remarks/>
        QT_ITI_USE,

        /// <remarks/>
        QT_OCF,

        /// <remarks/>
        QT_ITI_SER,

        /// <remarks/>
        QT_BND
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class FlagType
    {

        private string typeField;

        private string[] textField;

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string[] Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public enum QuotationGenericElementTypePricingInfoState
    {

        /// <remarks/>
        Original,

        /// <remarks/>
        Current,

        /// <remarks/>
        Disrupted
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    [XmlRoot("QtItineraryPricingRecord", Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2", IsNullable = false)]
    public partial class QtItineraryPricingRecordType
    {

        private QuotationGenericElementTypePrice[] priceField;

        private QuotationGenericElementTypeTax[] taxField;

        private PointType[] pointField;

        private FullQuotationType_Type quotationTypeField;

        private FlagType[] flagField;

        private QtItineraryPricingRecordTypePricingInfo[] pricingInfoField;

        private QuotationGenericElementTypePricingInfoState stateField;

        private bool stateFieldSpecified;

        /// <remarks/>
        [XmlElement("Price")]
        public QuotationGenericElementTypePrice[] Price
        {
            get
            {
                return priceField;
            }
            set
            {
                priceField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Tax")]
        public QuotationGenericElementTypeTax[] Tax
        {
            get
            {
                return taxField;
            }
            set
            {
                taxField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Point")]
        public PointType[] Point
        {
            get
            {
                return pointField;
            }
            set
            {
                pointField = value;
            }
        }

        /// <remarks/>
        public FullQuotationType_Type QuotationType
        {
            get
            {
                return quotationTypeField;
            }
            set
            {
                quotationTypeField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Flag")]
        public FlagType[] Flag
        {
            get
            {
                return flagField;
            }
            set
            {
                flagField = value;
            }
        }

        /// <remarks/>
        [XmlElement("PricingInfo")]
        public QtItineraryPricingRecordTypePricingInfo[] PricingInfo
        {
            get
            {
                return pricingInfoField;
            }
            set
            {
                pricingInfoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public QuotationGenericElementTypePricingInfoState State
        {
            get
            {
                return stateField;
            }
            set
            {
                stateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StateSpecified
        {
            get
            {
                return stateFieldSpecified;
            }
            set
            {
                stateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class QtItineraryPricingRecordTypePricingInfo
    {

        private long numberField;

        private bool numberFieldSpecified;

        private string[] indicatorField;

        private object[] itemsField;

        private PointOfSaleType1 originatorField;

        private PassengerTypeCodeType[] passengerTypeCodeField;

        private QtItineraryPricingRecordTypePricingInfoState stateField;

        private bool stateFieldSpecified;

        /// <remarks/>
        public long Number
        {
            get
            {
                return numberField;
            }
            set
            {
                numberField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool NumberSpecified
        {
            get
            {
                return numberFieldSpecified;
            }
            set
            {
                numberFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlElement("Indicator")]
        public string[] Indicator
        {
            get
            {
                return indicatorField;
            }
            set
            {
                indicatorField = value;
            }
        }

        /// <remarks/>
        [XmlElement("ProcessFlag", typeof(QtItineraryPricingRecordTypePricingInfoProcessFlag))]
        [XmlElement("UseCase", typeof(UseCaseType))]
        public object[] Items
        {
            get
            {
                return itemsField;
            }
            set
            {
                itemsField = value;
            }
        }

        /// <remarks/>
        public PointOfSaleType1 Originator
        {
            get
            {
                return originatorField;
            }
            set
            {
                originatorField = value;
            }
        }

        /// <remarks/>
        [XmlElement("PassengerTypeCode")]
        public PassengerTypeCodeType[] PassengerTypeCode
        {
            get
            {
                return passengerTypeCodeField;
            }
            set
            {
                passengerTypeCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public QtItineraryPricingRecordTypePricingInfoState State
        {
            get
            {
                return stateField;
            }
            set
            {
                stateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StateSpecified
        {
            get
            {
                return stateFieldSpecified;
            }
            set
            {
                stateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public enum QtItineraryPricingRecordTypePricingInfoProcessFlag
    {

        /// <remarks/>
        ATU,

        /// <remarks/>
        HTF,

        /// <remarks/>
        MultiTicket
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class UseCaseType
    {

        private UseCaseTypeSystem systemField;

        private string nameField;

        private string[] typesField;

        /// <remarks/>
        public UseCaseTypeSystem System
        {
            get
            {
                return systemField;
            }
            set
            {
                systemField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string[] Types
        {
            get
            {
                return typesField;
            }
            set
            {
                typesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class UseCaseTypeSystem
    {

        private string ownerField;

        private string[] modeField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string Owner
        {
            get
            {
                return ownerField;
            }
            set
            {
                ownerField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string[] Mode
        {
            get
            {
                return modeField;
            }
            set
            {
                modeField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(TypeName = "PointOfSaleType", Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class PointOfSaleType1
    {

        private PointOfSaleTypeOffice officeField;

        private PointOfSaleTypeLogin loginField;

        private PointOfSaleTypeActor actorField;

        /// <remarks/>
        public PointOfSaleTypeOffice Office
        {
            get
            {
                return officeField;
            }
            set
            {
                officeField = value;
            }
        }

        /// <remarks/>
        public PointOfSaleTypeLogin Login
        {
            get
            {
                return loginField;
            }
            set
            {
                loginField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public PointOfSaleTypeActor Actor
        {
            get
            {
                return actorField;
            }
            set
            {
                actorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class PointOfSaleTypeOffice
    {

        private string idField;

        private string numericIdField;

        private string cityField;

        private string systemCodeField;

        private string countryField;

        private string agentTypeField;

        /// <remarks/>
        [XmlAttribute()]
        public string ID
        {
            get
            {
                return idField;
            }
            set
            {
                idField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NumericId
        {
            get
            {
                return numericIdField;
            }
            set
            {
                numericIdField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string City
        {
            get
            {
                return cityField;
            }
            set
            {
                cityField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string SystemCode
        {
            get
            {
                return systemCodeField;
            }
            set
            {
                systemCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Country
        {
            get
            {
                return countryField;
            }
            set
            {
                countryField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string AgentType
        {
            get
            {
                return agentTypeField;
            }
            set
            {
                agentTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class PointOfSaleTypeLogin
    {

        private PointOfSaleTypeLoginChannel channelField;

        private string signField;

        private string dutyCodeField;

        /// <remarks/>
        public PointOfSaleTypeLoginChannel Channel
        {
            get
            {
                return channelField;
            }
            set
            {
                channelField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Sign
        {
            get
            {
                return signField;
            }
            set
            {
                signField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DutyCode
        {
            get
            {
                return dutyCodeField;
            }
            set
            {
                dutyCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class PointOfSaleTypeLoginChannel
    {

        private string accessTypeField;

        private string productField;

        private string subProductField;

        /// <remarks/>
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string AccessType
        {
            get
            {
                return accessTypeField;
            }
            set
            {
                accessTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string Product
        {
            get
            {
                return productField;
            }
            set
            {
                productField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string SubProduct
        {
            get
            {
                return subProductField;
            }
            set
            {
                subProductField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public enum PointOfSaleTypeActor
    {

        /// <remarks/>
        Creator,

        /// <remarks/>
        Owner,

        /// <remarks/>
        Updator
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class PassengerTypeCodeType
    {

        private string typeField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public enum QtItineraryPricingRecordTypePricingInfoState
    {

        /// <remarks/>
        Original,

        /// <remarks/>
        Current,

        /// <remarks/>
        Disrupted
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    [XmlRoot("QtRefund", Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2", IsNullable = false)]
    public partial class QtRefundType
    {

        private QuotationGenericElementTypePrice[] priceField;

        private QuotationGenericElementTypeTax[] taxField;

        private PointType[] pointField;

        private FullQuotationType_Type quotationTypeField;

        private FlagType[] flagField;

        private QtRefundTypePricingInfo[] pricingInfoField;

        private QuotationGenericElementTypePricingInfoState stateField;

        private bool stateFieldSpecified;

        /// <remarks/>
        [XmlElement("Price")]
        public QuotationGenericElementTypePrice[] Price
        {
            get
            {
                return priceField;
            }
            set
            {
                priceField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Tax")]
        public QuotationGenericElementTypeTax[] Tax
        {
            get
            {
                return taxField;
            }
            set
            {
                taxField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Point")]
        public PointType[] Point
        {
            get
            {
                return pointField;
            }
            set
            {
                pointField = value;
            }
        }

        /// <remarks/>
        public FullQuotationType_Type QuotationType
        {
            get
            {
                return quotationTypeField;
            }
            set
            {
                quotationTypeField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Flag")]
        public FlagType[] Flag
        {
            get
            {
                return flagField;
            }
            set
            {
                flagField = value;
            }
        }

        /// <remarks/>
        [XmlElement("PricingInfo")]
        public QtRefundTypePricingInfo[] PricingInfo
        {
            get
            {
                return pricingInfoField;
            }
            set
            {
                pricingInfoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public QuotationGenericElementTypePricingInfoState State
        {
            get
            {
                return stateField;
            }
            set
            {
                stateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StateSpecified
        {
            get
            {
                return stateFieldSpecified;
            }
            set
            {
                stateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class QtRefundTypePricingInfo
    {

        private long numberField;

        private bool numberFieldSpecified;

        private string refundedItineraryField;

        private string waiverCodeField;

        private string dataSourceIdentifierField;

        private QtRefundTypePricingInfoCancellationFeeCom cancellationFeeComField;

        private string pricingCodeField;

        private string[] settlementAuthorizationCodeField;

        private QuotationGenericElementTypePricingInfoState stateField;

        private bool stateFieldSpecified;

        /// <remarks/>
        public long Number
        {
            get
            {
                return numberField;
            }
            set
            {
                numberField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool NumberSpecified
        {
            get
            {
                return numberFieldSpecified;
            }
            set
            {
                numberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string RefundedItinerary
        {
            get
            {
                return refundedItineraryField;
            }
            set
            {
                refundedItineraryField = value;
            }
        }

        /// <remarks/>
        public string WaiverCode
        {
            get
            {
                return waiverCodeField;
            }
            set
            {
                waiverCodeField = value;
            }
        }

        /// <remarks/>
        public string DataSourceIdentifier
        {
            get
            {
                return dataSourceIdentifierField;
            }
            set
            {
                dataSourceIdentifierField = value;
            }
        }

        /// <remarks/>
        public QtRefundTypePricingInfoCancellationFeeCom CancellationFeeCom
        {
            get
            {
                return cancellationFeeComField;
            }
            set
            {
                cancellationFeeComField = value;
            }
        }

        /// <remarks/>
        public string PricingCode
        {
            get
            {
                return pricingCodeField;
            }
            set
            {
                pricingCodeField = value;
            }
        }

        /// <remarks/>
        [XmlElement("SettlementAuthorizationCode")]
        public string[] SettlementAuthorizationCode
        {
            get
            {
                return settlementAuthorizationCodeField;
            }
            set
            {
                settlementAuthorizationCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public QuotationGenericElementTypePricingInfoState State
        {
            get
            {
                return stateField;
            }
            set
            {
                stateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StateSpecified
        {
            get
            {
                return stateFieldSpecified;
            }
            set
            {
                stateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class QtRefundTypePricingInfoCancellationFeeCom
    {

        private string currencyCodeField;

        private string decimalPlacesField;

        private string amountField;

        /// <remarks/>
        [XmlAttribute()]
        public string CurrencyCode
        {
            get
            {
                return currencyCodeField;
            }
            set
            {
                currencyCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DecimalPlaces
        {
            get
            {
                return decimalPlacesField;
            }
            set
            {
                decimalPlacesField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Amount
        {
            get
            {
                return amountField;
            }
            set
            {
                amountField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    [XmlRoot("QtCouponItinerary", Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2", IsNullable = false)]
    public partial class QtCouponItineraryType
    {

        private QuotationGenericElementTypePrice[] priceField;

        private QuotationGenericElementTypeTax[] taxField;

        private PointType[] pointField;

        private FullQuotationType_Type quotationTypeField;

        private FlagType[] flagField;

        private QtCouponItineraryTypePricingInfo[] pricingInfoField;

        private QtCouponItineraryTypeProductInfo productInfoField;

        private QtCouponItineraryTypeTicketingInfo[] ticketingInfoField;

        private QuotationGenericElementTypePricingInfoState stateField;

        private bool stateFieldSpecified;

        /// <remarks/>
        [XmlElement("Price")]
        public QuotationGenericElementTypePrice[] Price
        {
            get
            {
                return priceField;
            }
            set
            {
                priceField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Tax")]
        public QuotationGenericElementTypeTax[] Tax
        {
            get
            {
                return taxField;
            }
            set
            {
                taxField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Point")]
        public PointType[] Point
        {
            get
            {
                return pointField;
            }
            set
            {
                pointField = value;
            }
        }

        /// <remarks/>
        public FullQuotationType_Type QuotationType
        {
            get
            {
                return quotationTypeField;
            }
            set
            {
                quotationTypeField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Flag")]
        public FlagType[] Flag
        {
            get
            {
                return flagField;
            }
            set
            {
                flagField = value;
            }
        }

        /// <remarks/>
        [XmlElement("PricingInfo")]
        public QtCouponItineraryTypePricingInfo[] PricingInfo
        {
            get
            {
                return pricingInfoField;
            }
            set
            {
                pricingInfoField = value;
            }
        }

        /// <remarks/>
        public QtCouponItineraryTypeProductInfo ProductInfo
        {
            get
            {
                return productInfoField;
            }
            set
            {
                productInfoField = value;
            }
        }

        /// <remarks/>
        [XmlElement("TicketingInfo")]
        public QtCouponItineraryTypeTicketingInfo[] TicketingInfo
        {
            get
            {
                return ticketingInfoField;
            }
            set
            {
                ticketingInfoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public QuotationGenericElementTypePricingInfoState State
        {
            get
            {
                return stateField;
            }
            set
            {
                stateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StateSpecified
        {
            get
            {
                return stateFieldSpecified;
            }
            set
            {
                stateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class QtCouponItineraryTypePricingInfo
    {

        private long numberField;

        private bool numberFieldSpecified;

        private string pricedPTCField;

        private FareBasisType fareBasisField;

        private string classOfServiceField;

        private BaggageAllowanceType baggageAllowanceField;

        private QuotationGenericElementTypePricingInfoState stateField;

        private bool stateFieldSpecified;

        /// <remarks/>
        public long Number
        {
            get
            {
                return numberField;
            }
            set
            {
                numberField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool NumberSpecified
        {
            get
            {
                return numberFieldSpecified;
            }
            set
            {
                numberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string PricedPTC
        {
            get
            {
                return pricedPTCField;
            }
            set
            {
                pricedPTCField = value;
            }
        }

        /// <remarks/>
        public FareBasisType FareBasis
        {
            get
            {
                return fareBasisField;
            }
            set
            {
                fareBasisField = value;
            }
        }

        /// <remarks/>
        public string ClassOfService
        {
            get
            {
                return classOfServiceField;
            }
            set
            {
                classOfServiceField = value;
            }
        }

        /// <remarks/>
        public BaggageAllowanceType BaggageAllowance
        {
            get
            {
                return baggageAllowanceField;
            }
            set
            {
                baggageAllowanceField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public QuotationGenericElementTypePricingInfoState State
        {
            get
            {
                return stateField;
            }
            set
            {
                stateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StateSpecified
        {
            get
            {
                return stateFieldSpecified;
            }
            set
            {
                stateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class FareBasisType
    {

        private string ticketDesignatorField;

        private string fareBasisCodeField;

        private string primaryCodeField;

        /// <remarks/>
        [XmlAttribute()]
        public string TicketDesignator
        {
            get
            {
                return ticketDesignatorField;
            }
            set
            {
                ticketDesignatorField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string FareBasisCode
        {
            get
            {
                return fareBasisCodeField;
            }
            set
            {
                fareBasisCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PrimaryCode
        {
            get
            {
                return primaryCodeField;
            }
            set
            {
                primaryCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class BaggageAllowanceType
    {

        private string allowanceTypeField;

        private string measureUnitField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string AllowanceType
        {
            get
            {
                return allowanceTypeField;
            }
            set
            {
                allowanceTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string MeasureUnit
        {
            get
            {
                return measureUnitField;
            }
            set
            {
                measureUnitField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class QtCouponItineraryTypeProductInfo
    {

        private QtCouponItineraryTypeProductInfoProvider providerField;

        private DateTimeAndLocationType1 startField;

        private DateTimeAndLocationType1 endField;

        private QuotationGenericElementTypePricingInfoState stateField;

        private bool stateFieldSpecified;

        /// <remarks/>
        public QtCouponItineraryTypeProductInfoProvider Provider
        {
            get
            {
                return providerField;
            }
            set
            {
                providerField = value;
            }
        }

        /// <remarks/>
        public DateTimeAndLocationType1 Start
        {
            get
            {
                return startField;
            }
            set
            {
                startField = value;
            }
        }

        /// <remarks/>
        public DateTimeAndLocationType1 End
        {
            get
            {
                return endField;
            }
            set
            {
                endField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public QuotationGenericElementTypePricingInfoState State
        {
            get
            {
                return stateField;
            }
            set
            {
                stateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StateSpecified
        {
            get
            {
                return stateFieldSpecified;
            }
            set
            {
                stateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class QtCouponItineraryTypeProductInfoProvider
    {

        private string codeField;

        private string contextField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Context
        {
            get
            {
                return contextField;
            }
            set
            {
                contextField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(TypeName = "DateTimeAndLocationType", Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class DateTimeAndLocationType1
    {

        private DateTimeAndLocationTypeLocation locationField;

        private string dateTimeField;

        /// <remarks/>
        public DateTimeAndLocationTypeLocation Location
        {
            get
            {
                return locationField;
            }
            set
            {
                locationField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DateTime
        {
            get
            {
                return dateTimeField;
            }
            set
            {
                dateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class DateTimeAndLocationTypeLocation
    {

        private string codeField;

        private string contextField;

        /// <remarks/>
        [XmlAttribute()]
        public string Code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Context
        {
            get
            {
                return contextField;
            }
            set
            {
                contextField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class QtCouponItineraryTypeTicketingInfo
    {

        private DisruptionFlightsType[] flightReferenceField;

        private QuotationGenericElementTypePricingInfoState stateField;

        private bool stateFieldSpecified;

        /// <remarks/>
        [XmlElement("FlightReference")]
        public DisruptionFlightsType[] FlightReference
        {
            get
            {
                return flightReferenceField;
            }
            set
            {
                flightReferenceField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public QuotationGenericElementTypePricingInfoState State
        {
            get
            {
                return stateField;
            }
            set
            {
                stateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StateSpecified
        {
            get
            {
                return stateFieldSpecified;
            }
            set
            {
                stateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class DisruptionFlightsType
    {

        private string orderField;

        private string ticketNumberField;

        private string couponNumberField;

        private string disruptionTagField;

        /// <remarks/>
        public string Order
        {
            get
            {
                return orderField;
            }
            set
            {
                orderField = value;
            }
        }

        /// <remarks/>
        public string TicketNumber
        {
            get
            {
                return ticketNumberField;
            }
            set
            {
                ticketNumberField = value;
            }
        }

        /// <remarks/>
        public string CouponNumber
        {
            get
            {
                return couponNumberField;
            }
            set
            {
                couponNumberField = value;
            }
        }

        /// <remarks/>
        public string DisruptionTag
        {
            get
            {
                return disruptionTagField;
            }
            set
            {
                disruptionTagField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    [XmlRoot("QtItineraryService", Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2", IsNullable = false)]
    public partial class QtItineraryServiceType
    {

        private QuotationGenericElementTypePrice[] priceField;

        private QuotationGenericElementTypeTax[] taxField;

        private PointType[] pointField;

        private FullQuotationType_Type quotationTypeField;

        private FlagType[] flagField;

        private QtItineraryServiceTypePricingInfo[] pricingInfoField;

        private QuotationGenericElementTypePricingInfoState stateField;

        private bool stateFieldSpecified;

        /// <remarks/>
        [XmlElement("Price")]
        public QuotationGenericElementTypePrice[] Price
        {
            get
            {
                return priceField;
            }
            set
            {
                priceField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Tax")]
        public QuotationGenericElementTypeTax[] Tax
        {
            get
            {
                return taxField;
            }
            set
            {
                taxField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Point")]
        public PointType[] Point
        {
            get
            {
                return pointField;
            }
            set
            {
                pointField = value;
            }
        }

        /// <remarks/>
        public FullQuotationType_Type QuotationType
        {
            get
            {
                return quotationTypeField;
            }
            set
            {
                quotationTypeField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Flag")]
        public FlagType[] Flag
        {
            get
            {
                return flagField;
            }
            set
            {
                flagField = value;
            }
        }

        /// <remarks/>
        [XmlElement("PricingInfo")]
        public QtItineraryServiceTypePricingInfo[] PricingInfo
        {
            get
            {
                return pricingInfoField;
            }
            set
            {
                pricingInfoField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public QuotationGenericElementTypePricingInfoState State
        {
            get
            {
                return stateField;
            }
            set
            {
                stateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StateSpecified
        {
            get
            {
                return stateFieldSpecified;
            }
            set
            {
                stateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/QuotationTypes_v2")]
    public partial class QtItineraryServiceTypePricingInfo
    {

        private long numberField;

        private bool numberFieldSpecified;

        private QuotationGenericElementTypePricingInfoState stateField;

        private bool stateFieldSpecified;

        /// <remarks/>
        public long Number
        {
            get
            {
                return numberField;
            }
            set
            {
                numberField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool NumberSpecified
        {
            get
            {
                return numberFieldSpecified;
            }
            set
            {
                numberFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public QuotationGenericElementTypePricingInfoState State
        {
            get
            {
                return stateField;
            }
            set
            {
                stateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool StateSpecified
        {
            get
            {
                return stateFieldSpecified;
            }
            set
            {
                stateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class GenericWarningsType
    {


        /// <remarks/>
        [XmlArrayItem("Warning", IsNullable = false)]
        public ErrorType[] Warnings { get; set; }

        /// <remarks/>
        [XmlArray("Warnings", Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
        [XmlArrayItem("Warning", IsNullable = false)]
        public WarningType1[] Warnings1 { get; set; }

        /// <remarks/>
        [XmlArray("Warnings", Namespace = "http://www.iata.org/IATA/2007/00/IATA2010.1")]
        [XmlArrayItem("Warning", IsNullable = false)]
        public WarningType2[] Warnings2 { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class GenericErrorsType
    {

        /// <remarks/>
        [XmlArrayItem("Error", IsNullable = false)]
        public ErrorType[] Errors { get; set; }

        /// <remarks/>
        [XmlArray("Errors", Namespace = "http://www.opentravel.org/OTA/2003/05/OTA2011A")]
        [XmlArrayItem("Error", IsNullable = false)]
        public ErrorType1[] Errors1 { get; set; }

        /// <remarks/>
        [XmlArray("Errors", Namespace = "http://www.iata.org/IATA/2007/00/IATA2010.1")]
        [XmlArrayItem("Error", IsNullable = false)]
        public ErrorType2[] Errors2 { get; set; }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class NotificationType
    {
        /// <remarks/>
        [XmlElement("To")]
        public NotificationTypeTO[] To { get; set; }

        /// <remarks/>
        [XmlElement("Comment")]
        public NotificationTypeComment[] Comment { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Type { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "language")]
        public string Language { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class NotificationTypeTO
    {

        /// <remarks/>
        [XmlAttribute()]
        public string Type { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class NotificationTypeComment
    {

        /// <remarks/>
        [XmlAttribute()]
        public string Type { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CardType
    {

        private AVS_Type addressVerificationSystemValueField;

        private CardTypeMagneticTrack[] magneticTrackField;

        private CardIssuanceInformationType issuanceField;

        private CardTypePrimaryAccountNumber primaryAccountNumberField;

        private CardTypeCVV cVVField;

        private CardTypeValidity validityField;

        private CardTypeVendor vendorField;

        private CardTypeProgram programField;

        private string holderNameField;

        private string subTypeField;

        private string processField;

        private CardTypeAccountType accountTypeField;

        private bool accountTypeFieldSpecified;

        private string tierLevelField;

        /// <remarks/>
        public AVS_Type AddressVerificationSystemValue
        {
            get
            {
                return addressVerificationSystemValueField;
            }
            set
            {
                addressVerificationSystemValueField = value;
            }
        }

        /// <remarks/>
        [XmlElement("MagneticTrack")]
        public CardTypeMagneticTrack[] MagneticTrack
        {
            get
            {
                return magneticTrackField;
            }
            set
            {
                magneticTrackField = value;
            }
        }

        /// <remarks/>
        public CardIssuanceInformationType Issuance
        {
            get
            {
                return issuanceField;
            }
            set
            {
                issuanceField = value;
            }
        }

        /// <remarks/>
        public CardTypePrimaryAccountNumber PrimaryAccountNumber
        {
            get
            {
                return primaryAccountNumberField;
            }
            set
            {
                primaryAccountNumberField = value;
            }
        }

        /// <remarks/>
        public CardTypeCVV CVV
        {
            get
            {
                return cVVField;
            }
            set
            {
                cVVField = value;
            }
        }

        /// <remarks/>
        public CardTypeValidity Validity
        {
            get
            {
                return validityField;
            }
            set
            {
                validityField = value;
            }
        }

        /// <remarks/>
        public CardTypeVendor Vendor
        {
            get
            {
                return vendorField;
            }
            set
            {
                vendorField = value;
            }
        }

        /// <remarks/>
        public CardTypeProgram Program
        {
            get
            {
                return programField;
            }
            set
            {
                programField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string HolderName
        {
            get
            {
                return holderNameField;
            }
            set
            {
                holderNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string SubType
        {
            get
            {
                return subTypeField;
            }
            set
            {
                subTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Process
        {
            get
            {
                return processField;
            }
            set
            {
                processField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public CardTypeAccountType AccountType
        {
            get
            {
                return accountTypeField;
            }
            set
            {
                accountTypeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool AccountTypeSpecified
        {
            get
            {
                return accountTypeFieldSpecified;
            }
            set
            {
                accountTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string TierLevel
        {
            get
            {
                return tierLevelField;
            }
            set
            {
                tierLevelField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class AVS_Type
    {

        private string[] lineField;

        private string cityNameField;

        private string postalCodeField;

        private string countryField;

        /// <remarks/>
        [XmlElement("Line")]
        public string[] Line
        {
            get
            {
                return lineField;
            }
            set
            {
                lineField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string CityName
        {
            get
            {
                return cityNameField;
            }
            set
            {
                cityNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PostalCode
        {
            get
            {
                return postalCodeField;
            }
            set
            {
                postalCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Country
        {
            get
            {
                return countryField;
            }
            set
            {
                countryField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/FOP_Types_v6")]
    public enum CardTypeMagneticTrackType
    {

        /// <remarks/>
        Raw,

        /// <remarks/>
        [XmlEnum("1")]
        Item1,

        /// <remarks/>
        [XmlEnum("2")]
        Item2,

        /// <remarks/>
        [XmlEnum("3")]
        Item3
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CardTypeMagneticTrack
    {

        /// <remarks/>
        [XmlAttribute()]
        public CardTypeMagneticTrackType Type { get; set; }

        /// <remarks/>
        [XmlIgnore()]
        public bool TypeSpecified { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CardIssuanceInformationTypeBank
    {
        /// <remarks/>
        [XmlAttribute()]
        public string Name { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string ID { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CardIssuanceInformationType
    {

        /// <remarks/>
        public CardIssuanceInformationTypeBank Bank { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Country { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string CountryCode { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CardTypePrimaryAccountNumber
    {

        /// <remarks/>
        [XmlAttribute()]
        public string KnoxID { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CardTypeCVV
    {

        /// <remarks/>
        [XmlAttribute()]
        public string KnoxID { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CardTypeValidity
    {

        /// <remarks/>
        [XmlAttribute()]
        public string StartDate { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string EndDate { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CardTypeVendor
    {

        /// <remarks/>
        [XmlAttribute()]
        public string Code { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CardTypeProgram
    {

        /// <remarks/>
        [XmlAttribute()]
        public string Code { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum CardTypeAccountType
    {

        /// <remarks/>
        Corporate,

        /// <remarks/>
        Personal
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class FundsTransferDetailsType : FundsTransferType
    {

        /// <remarks/>
        [XmlAttribute()]
        public string Status { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Reference { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string UserID { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string OfficeID { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string CreationDate { get; set; }

    }

    /// <remarks/>
    [XmlInclude(typeof(FundsTransferDetailsType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class FundsTransferType
    {

        /// <remarks/>
        [XmlElement("Value")]
        public VirtualCardAmountType[] Value { get; set; }

        /// <remarks/>
        public FundsTransferTypeScheduling Scheduling { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Action { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class FundsTransferTypeScheduling
    {

        /// <remarks/>
        [XmlAttribute()]
        public string Date { get; set; }

    }



    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class TTR_ActorType : PassengerNameType
    {

        /// <remarks/>
        [XmlElement("nationality")]
        public TTR_ActorTypeNationality[] nationality { get; set; }

        /// <remarks/>
        public TTR_ActorTypeContact contact { get; set; }

        /// <remarks/>
        [XmlElement("address")]
        public TTR_ActorTypeAddress[] address { get; set; }


        /// <remarks/>
        [XmlElement("event")]
        public TTR_ActorTypeEvent[] @event { get; set; }

        /// <remarks/>
        [XmlElement("loyalty")]
        public TTR_ActorTypeLoyalty[] loyalty { get; set; }

        /// <remarks/>
        [XmlElement("docRef")]
        public TTR_ActorTypeDocRef[] docRef { get; set; }


        /// <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string externalID { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "NMTOKENS")]
        public string roles { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class TTR_ActorTypeNationality
    {

        /// <remarks/>
        [XmlAttribute()]
        public string code { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class TTR_ActorTypeContact
    {

        /// <remarks/>
        [XmlElement("phone")]
        public TTR_ActorTypeContactPhone[] phone { get; set; }

        /// <remarks/>
        [XmlElement("email")]
        public TTR_ActorTypeContactEmail[] email { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class TTR_ActorTypeContactPhone
    {

        /// <remarks/>
        [XmlAttribute()]
        public string label { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string purpose { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string overseasCode { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string countryCode { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "normalizedString")]
        public string addresseeName { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class TTR_ActorTypeContactEmail
    {

        /// <remarks/>
        [XmlAttribute()]
        public string label { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string purpose { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "normalizedString")]
        public string addresseeName { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class TTR_ActorTypeAddress
    {

        /// <remarks/>
        [XmlAttribute()]
        public string label { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string line { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string complement { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string zip { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string countryCode { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string latitude { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string longitude { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string cityCode { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string cityName { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string countryName { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string stateCode { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string stateName { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string postalBox { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "normalizedString")]
        public string companyName { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "normalizedString")]
        public string addresseeName { get; set; }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class TTR_ActorTypeEvent
    {

        /// <remarks/>
        [XmlAttribute()]
        public string description { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string date { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class TTR_ActorTypeLoyalty
    {

        /// <remarks/>
        [XmlAttribute()]
        public string companyCode { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string cardNumber { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string tierLevel { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string alliance { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class TTR_ActorTypeDocRef
    {

        /// <remarks/>
        [XmlAttribute()]
        public string @type { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string description { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string issuer { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string issuanceDate { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string expirationDate { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ETR_TransferType
    {

        private RailProviderType serviceProviderField;

        private string identifierField;

        private ETR_TransferTypeStart startField;

        private ETR_TransferTypeEnd endField;

        private ETR_TransferTypeVehicle vehicleField;

        private MileageType mileageField;

        private ETR_TransferTypeBaggage baggageField;

        private ETR_TransferTypeCheckIn checkInField;

        private ETR_TransferTypeTicket ticketField;

        private CreationType creationField;

        private ModificationType modificationField;

        private ConfirmationType confirmationField;

        private ETR_TransferTypeDescriptions[] descriptionsField;

        private BookingChannelType bkgChannelField;

        private FullOriginSystemType creationChannelField;

        private ExternalSystemType externalSystemField;

        private SystemInformation creatorField;

        private string propertiesField;

        private string descriptionField;

        private string bkgClassField;

        private string statusField;

        private string nIPField;

        private string confirmNbrField;

        private string cancelNbrField;

        private string overrideStatusField;

        private string mBOProductCodeField;

        /// <remarks/>
        public RailProviderType serviceProvider
        {
            get
            {
                return serviceProviderField;
            }
            set
            {
                serviceProviderField = value;
            }
        }

        /// <remarks/>
        public string identifier
        {
            get
            {
                return identifierField;
            }
            set
            {
                identifierField = value;
            }
        }

        /// <remarks/>
        public ETR_TransferTypeStart start
        {
            get
            {
                return startField;
            }
            set
            {
                startField = value;
            }
        }

        /// <remarks/>
        public ETR_TransferTypeEnd end
        {
            get
            {
                return endField;
            }
            set
            {
                endField = value;
            }
        }

        /// <remarks/>
        public ETR_TransferTypeVehicle vehicle
        {
            get
            {
                return vehicleField;
            }
            set
            {
                vehicleField = value;
            }
        }

        /// <remarks/>
        public MileageType mileage
        {
            get
            {
                return mileageField;
            }
            set
            {
                mileageField = value;
            }
        }

        /// <remarks/>
        public ETR_TransferTypeBaggage baggage
        {
            get
            {
                return baggageField;
            }
            set
            {
                baggageField = value;
            }
        }

        /// <remarks/>
        public ETR_TransferTypeCheckIn checkIn
        {
            get
            {
                return checkInField;
            }
            set
            {
                checkInField = value;
            }
        }

        /// <remarks/>
        public ETR_TransferTypeTicket ticket
        {
            get
            {
                return ticketField;
            }
            set
            {
                ticketField = value;
            }
        }

        /// <remarks/>
        public CreationType creation
        {
            get
            {
                return creationField;
            }
            set
            {
                creationField = value;
            }
        }

        /// <remarks/>
        public ModificationType modification
        {
            get
            {
                return modificationField;
            }
            set
            {
                modificationField = value;
            }
        }

        /// <remarks/>
        public ConfirmationType confirmation
        {
            get
            {
                return confirmationField;
            }
            set
            {
                confirmationField = value;
            }
        }

        /// <remarks/>
        [XmlElement("descriptions")]
        public ETR_TransferTypeDescriptions[] descriptions
        {
            get
            {
                return descriptionsField;
            }
            set
            {
                descriptionsField = value;
            }
        }

        /// <remarks/>
        public BookingChannelType bkgChannel
        {
            get
            {
                return bkgChannelField;
            }
            set
            {
                bkgChannelField = value;
            }
        }

        /// <remarks/>
        public FullOriginSystemType creationChannel
        {
            get
            {
                return creationChannelField;
            }
            set
            {
                creationChannelField = value;
            }
        }

        /// <remarks/>
        public ExternalSystemType externalSystem
        {
            get
            {
                return externalSystemField;
            }
            set
            {
                externalSystemField = value;
            }
        }

        /// <remarks/>
        public SystemInformation creator
        {
            get
            {
                return creatorField;
            }
            set
            {
                creatorField = value;
            }
        }

        /// <remarks/>
        public string properties
        {
            get
            {
                return propertiesField;
            }
            set
            {
                propertiesField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string bkgClass
        {
            get
            {
                return bkgClassField;
            }
            set
            {
                bkgClassField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string status
        {
            get
            {
                return statusField;
            }
            set
            {
                statusField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NIP
        {
            get
            {
                return nIPField;
            }
            set
            {
                nIPField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string confirmNbr
        {
            get
            {
                return confirmNbrField;
            }
            set
            {
                confirmNbrField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string cancelNbr
        {
            get
            {
                return cancelNbrField;
            }
            set
            {
                cancelNbrField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string overrideStatus
        {
            get
            {
                return overrideStatusField;
            }
            set
            {
                overrideStatusField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string MBOProductCode
        {
            get
            {
                return mBOProductCodeField;
            }
            set
            {
                mBOProductCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ETR_TransferTypeStart
    {

        private string locationCodeField;

        private ETR_TransferTypeStartAddress addressField;

        private string dateTimeField;

        private string locationNameField;

        private string terminalField;

        /// <remarks/>
        public string locationCode
        {
            get
            {
                return locationCodeField;
            }
            set
            {
                locationCodeField = value;
            }
        }

        /// <remarks/>
        public ETR_TransferTypeStartAddress address
        {
            get
            {
                return addressField;
            }
            set
            {
                addressField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string dateTime
        {
            get
            {
                return dateTimeField;
            }
            set
            {
                dateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string locationName
        {
            get
            {
                return locationNameField;
            }
            set
            {
                locationNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string terminal
        {
            get
            {
                return terminalField;
            }
            set
            {
                terminalField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ETR_TransferTypeStartAddress
    {

        private string lineField;

        private string complementField;

        private string zipField;

        private string countryCodeField;

        private string latitudeField;

        private string longitudeField;

        private string cityCodeField;

        private string cityNameField;

        private string countryNameField;

        private string stateCodeField;

        private string stateNameField;

        private string postalBoxField;

        /// <remarks/>
        [XmlAttribute()]
        public string line
        {
            get
            {
                return lineField;
            }
            set
            {
                lineField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string complement
        {
            get
            {
                return complementField;
            }
            set
            {
                complementField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string zip
        {
            get
            {
                return zipField;
            }
            set
            {
                zipField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string countryCode
        {
            get
            {
                return countryCodeField;
            }
            set
            {
                countryCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string latitude
        {
            get
            {
                return latitudeField;
            }
            set
            {
                latitudeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string longitude
        {
            get
            {
                return longitudeField;
            }
            set
            {
                longitudeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string cityCode
        {
            get
            {
                return cityCodeField;
            }
            set
            {
                cityCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string cityName
        {
            get
            {
                return cityNameField;
            }
            set
            {
                cityNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string countryName
        {
            get
            {
                return countryNameField;
            }
            set
            {
                countryNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string stateCode
        {
            get
            {
                return stateCodeField;
            }
            set
            {
                stateCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string stateName
        {
            get
            {
                return stateNameField;
            }
            set
            {
                stateNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string postalBox
        {
            get
            {
                return postalBoxField;
            }
            set
            {
                postalBoxField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ETR_TransferTypeEnd
    {

        private string locationCodeField;

        private ETR_TransferTypeEndAddress addressField;

        private string dateTimeField;

        private string locationNameField;

        private string terminalField;

        /// <remarks/>
        public string locationCode
        {
            get
            {
                return locationCodeField;
            }
            set
            {
                locationCodeField = value;
            }
        }

        /// <remarks/>
        public ETR_TransferTypeEndAddress address
        {
            get
            {
                return addressField;
            }
            set
            {
                addressField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string dateTime
        {
            get
            {
                return dateTimeField;
            }
            set
            {
                dateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string locationName
        {
            get
            {
                return locationNameField;
            }
            set
            {
                locationNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string terminal
        {
            get
            {
                return terminalField;
            }
            set
            {
                terminalField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ETR_TransferTypeEndAddress
    {

        private string lineField;

        private string complementField;

        private string zipField;

        private string countryCodeField;

        private string latitudeField;

        private string longitudeField;

        private string cityCodeField;

        private string cityNameField;

        private string countryNameField;

        private string stateCodeField;

        private string stateNameField;

        private string postalBoxField;

        /// <remarks/>
        [XmlAttribute()]
        public string line
        {
            get
            {
                return lineField;
            }
            set
            {
                lineField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string complement
        {
            get
            {
                return complementField;
            }
            set
            {
                complementField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string zip
        {
            get
            {
                return zipField;
            }
            set
            {
                zipField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string countryCode
        {
            get
            {
                return countryCodeField;
            }
            set
            {
                countryCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string latitude
        {
            get
            {
                return latitudeField;
            }
            set
            {
                latitudeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string longitude
        {
            get
            {
                return longitudeField;
            }
            set
            {
                longitudeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string cityCode
        {
            get
            {
                return cityCodeField;
            }
            set
            {
                cityCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string cityName
        {
            get
            {
                return cityNameField;
            }
            set
            {
                cityNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string countryName
        {
            get
            {
                return countryNameField;
            }
            set
            {
                countryNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string stateCode
        {
            get
            {
                return stateCodeField;
            }
            set
            {
                stateCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string stateName
        {
            get
            {
                return stateNameField;
            }
            set
            {
                stateNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string postalBox
        {
            get
            {
                return postalBoxField;
            }
            set
            {
                postalBoxField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ETR_TransferTypeVehicle
    {

        private string codeField;

        private string descriptionField;

        /// <remarks/>
        [XmlAttribute()]
        public string code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ETR_TransferTypeBaggage
    {

        private string quantityField;

        /// <remarks/>
        [XmlAttribute()]
        public string quantity
        {
            get
            {
                return quantityField;
            }
            set
            {
                quantityField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ETR_TransferTypeCheckIn
    {

        private string endDateTimeField;

        /// <remarks/>
        [XmlAttribute()]
        public string endDateTime
        {
            get
            {
                return endDateTimeField;
            }
            set
            {
                endDateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ETR_TransferTypeTicket
    {

        private string numberField;

        /// <remarks/>
        [XmlAttribute()]
        public string number
        {
            get
            {
                return numberField;
            }
            set
            {
                numberField = value;
            }
        }
    }

    /// <remarks/>
    [XmlInclude(typeof(TTR_ActorType))]
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class PassengerNameType
    {

        private PassengerNameTypeName[] nameField;

        private string pTCField;

        private string staffTypeField;

        private string dateOfBirthField;

        private string ageField;

        private string specialSeatField;

        private string identificationCodeField;

        /// <remarks/>
        [XmlElement("Name")]
        public PassengerNameTypeName[] Name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string PTC
        {
            get
            {
                return pTCField;
            }
            set
            {
                pTCField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string StaffType
        {
            get
            {
                return staffTypeField;
            }
            set
            {
                staffTypeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string DateOfBirth
        {
            get
            {
                return dateOfBirthField;
            }
            set
            {
                dateOfBirthField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Age
        {
            get
            {
                return ageField;
            }
            set
            {
                ageField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string SpecialSeat
        {
            get
            {
                return specialSeatField;
            }
            set
            {
                specialSeatField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string IdentificationCode
        {
            get
            {
                return identificationCodeField;
            }
            set
            {
                identificationCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class PassengerNameTypeName
    {

        private string typeField;

        private string isRefField;

        private string lastNameField;

        private string firstNameField;

        private string titleField;

        /// <remarks/>
        [XmlAttribute()]
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string IsRef
        {
            get
            {
                return isRefField;
            }
            set
            {
                isRefField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string LastName
        {
            get
            {
                return lastNameField;
            }
            set
            {
                lastNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string FirstName
        {
            get
            {
                return firstNameField;
            }
            set
            {
                firstNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Title
        {
            get
            {
                return titleField;
            }
            set
            {
                titleField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ReservationTypeTransport : ETR_TransferType
    {

        private string typeField;

        /// <remarks/>
        [XmlAttribute()]
        public string @type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ReservationTypeAccommodation : ETR_SleepMiscellaneousType
    {

        private string identifierField;

        private string typeField;

        private string bkgClassField;

        /// <remarks/>
        public string identifier
        {
            get
            {
                return identifierField;
            }
            set
            {
                identifierField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string @type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string bkgClass
        {
            get
            {
                return bkgClassField;
            }
            set
            {
                bkgClassField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ReservationTypeActivity : ETR_ShowAndEventType
    {

        /// <remarks/>
        public string identifier { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string @type { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ReportingInfoTypeAdditionalInfo
    {

        /// <remarks/>
        [XmlAttribute()]
        public string CodeContext { get; set; }

        /// <remarks/>
        [XmlAttribute()]
        public string Code { get; set; }

        /// <remarks/>
        [XmlText()]
        public string Value { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ReportingInfoType
    {
        /// <remarks/>
        [XmlElement("AdditionalInfo")]
        public ReportingInfoTypeAdditionalInfo[] AdditionalInfo { get; set; }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class RailProviderType
    {

        private string refField;

        private string nameField;

        private string codeField;

        private string externalRefField;

        /// <remarks/>
        [XmlAttribute()]
        public string @ref
        {
            get
            {
                return refField;
            }
            set
            {
                refField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string externalRef
        {
            get
            {
                return externalRefField;
            }
            set
            {
                externalRefField = value;
            }
        }
    }



    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ReservationType
    {

        private TTR_ActorType[] travelersField;

        private ReservationTypeTransport[] transportField;

        private ReservationTypeAccommodation accommodationField;

        private ReservationTypeActivity activityField;

        private string idField;

        private string externalIDField;

        private DateTime creationDateField;

        private bool creationDateFieldSpecified;

        /// <remarks/>
        [XmlArrayItem("Traveler", IsNullable = false)]
        public TTR_ActorType[] Travelers
        {
            get
            {
                return travelersField;
            }
            set
            {
                travelersField = value;
            }
        }

        /// <remarks/>
        [XmlElement("Transport")]
        public ReservationTypeTransport[] Transport
        {
            get
            {
                return transportField;
            }
            set
            {
                transportField = value;
            }
        }

        /// <remarks/>
        public ReservationTypeAccommodation Accommodation
        {
            get
            {
                return accommodationField;
            }
            set
            {
                accommodationField = value;
            }
        }

        /// <remarks/>
        public ReservationTypeActivity Activity
        {
            get
            {
                return activityField;
            }
            set
            {
                activityField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ID
        {
            get
            {
                return idField;
            }
            set
            {
                idField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string ExternalID
        {
            get
            {
                return externalIDField;
            }
            set
            {
                externalIDField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime CreationDate
        {
            get
            {
                return creationDateField;
            }
            set
            {
                creationDateField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool CreationDateSpecified
        {
            get
            {
                return creationDateFieldSpecified;
            }
            set
            {
                creationDateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class MileageType
    {

        private string distanceField;

        private string unitField;

        /// <remarks/>
        [XmlAttribute()]
        public string distance
        {
            get
            {
                return distanceField;
            }
            set
            {
                distanceField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string unit
        {
            get
            {
                return unitField;
            }
            set
            {
                unitField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class CreationType
    {

        private string dateTimeField;

        /// <remarks/>
        [XmlAttribute()]
        public string dateTime
        {
            get
            {
                return dateTimeField;
            }
            set
            {
                dateTimeField = value;
            }
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ModificationType
    {

        private string dateTimeField;

        /// <remarks/>
        [XmlAttribute()]
        public string dateTime
        {
            get
            {
                return dateTimeField;
            }
            set
            {
                dateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ConfirmationType
    {

        private string deadlineField;

        /// <remarks/>
        [XmlAttribute()]
        public string deadline
        {
            get
            {
                return deadlineField;
            }
            set
            {
                deadlineField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ETR_TransferTypeDescriptions : ProductDescriptionType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ProductDescriptionType
    {

        private object[] itemsField;

        /// <remarks/>
        [XmlElement("iframe", typeof(ProductDescriptionTypeIframe))]
        [XmlElement("media", typeof(ProductDescriptionTypeMedia))]
        [XmlElement("text", typeof(ProductDescriptionTypeText))]
        public object[] Items
        {
            get
            {
                return itemsField;
            }
            set
            {
                itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ProductDescriptionTypeIframe
    {

        private string nameField;

        private string srcField;

        private long widthField;

        private bool widthFieldSpecified;

        private long heightField;

        private bool heightFieldSpecified;

        private long borderField;

        private bool borderFieldSpecified;

        private string srcdocField;

        /// <remarks/>
        [XmlAttribute()]
        public string name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string src
        {
            get
            {
                return srcField;
            }
            set
            {
                srcField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long width
        {
            get
            {
                return widthField;
            }
            set
            {
                widthField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool widthSpecified
        {
            get
            {
                return widthFieldSpecified;
            }
            set
            {
                widthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long height
        {
            get
            {
                return heightField;
            }
            set
            {
                heightField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool heightSpecified
        {
            get
            {
                return heightFieldSpecified;
            }
            set
            {
                heightFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long border
        {
            get
            {
                return borderField;
            }
            set
            {
                borderField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool borderSpecified
        {
            get
            {
                return borderFieldSpecified;
            }
            set
            {
                borderFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string srcdoc
        {
            get
            {
                return srcdocField;
            }
            set
            {
                srcdocField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ProductDescriptionTypeMedia
    {

        private string nameField;

        private string typeField;

        private ProductDescriptionTypeMediaEncoding encodingField;

        private bool encodingFieldSpecified;

        private long sizeField;

        private bool sizeFieldSpecified;

        private string srcField;

        private string idField;

        private long widthField;

        private bool widthFieldSpecified;

        private long heightField;

        private bool heightFieldSpecified;

        /// <remarks/>
        [XmlAttribute()]
        public string name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string @type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public ProductDescriptionTypeMediaEncoding encoding
        {
            get
            {
                return encodingField;
            }
            set
            {
                encodingField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool encodingSpecified
        {
            get
            {
                return encodingFieldSpecified;
            }
            set
            {
                encodingFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long size
        {
            get
            {
                return sizeField;
            }
            set
            {
                sizeField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool sizeSpecified
        {
            get
            {
                return sizeFieldSpecified;
            }
            set
            {
                sizeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string src
        {
            get
            {
                return srcField;
            }
            set
            {
                srcField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ID
        {
            get
            {
                return idField;
            }
            set
            {
                idField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long width
        {
            get
            {
                return widthField;
            }
            set
            {
                widthField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool widthSpecified
        {
            get
            {
                return widthFieldSpecified;
            }
            set
            {
                widthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public long height
        {
            get
            {
                return heightField;
            }
            set
            {
                heightField = value;
            }
        }

        /// <remarks/>
        [XmlIgnore()]
        public bool heightSpecified
        {
            get
            {
                return heightFieldSpecified;
            }
            set
            {
                heightFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum ProductDescriptionTypeMediaEncoding
    {

        /// <remarks/>
        TXT,

        /// <remarks/>
        DOC,

        /// <remarks/>
        GIF,

        /// <remarks/>
        PNG,

        /// <remarks/>
        PDF,

        /// <remarks/>
        JPG
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class ProductDescriptionTypeText
    {

        private string languageField;

        private string typeField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute(DataType = "language")]
        public string language
        {
            get
            {
                return languageField;
            }
            set
            {
                languageField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string @type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class BookingChannelType
    {

        private string codeField;

        private string descriptionField;

        /// <remarks/>
        [XmlAttribute()]
        public string code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/PNR_Types_v4")]
    public partial class FullOriginSystemType
    {

        private string typeField;

        private string classField;

        private string categoryField;

        private string ownerField;

        private string accessPointField;

        /// <remarks/>
        [XmlAttribute()]
        public string @type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string @class
        {
            get
            {
                return classField;
            }
            set
            {
                classField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string category
        {
            get
            {
                return categoryField;
            }
            set
            {
                categoryField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string owner
        {
            get
            {
                return ownerField;
            }
            set
            {
                ownerField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string accessPoint
        {
            get
            {
                return accessPointField;
            }
            set
            {
                accessPointField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ExternalSystemType
    {

        private ExternalSystemTypeCreation creationField;

        private ExternalSystemTypeBkgReference[] bkgReferenceField;

        /// <remarks/>
        public ExternalSystemTypeCreation creation
        {
            get
            {
                return creationField;
            }
            set
            {
                creationField = value;
            }
        }

        /// <remarks/>
        [XmlElement("bkgReference")]
        public ExternalSystemTypeBkgReference[] bkgReference
        {
            get
            {
                return bkgReferenceField;
            }
            set
            {
                bkgReferenceField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ExternalSystemTypeCreation
    {

        private string dateTimeField;

        /// <remarks/>
        [XmlAttribute()]
        public string dateTime
        {
            get
            {
                return dateTimeField;
            }
            set
            {
                dateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ExternalSystemTypeBkgReference
    {

        private string ownerField;

        private string numberField;

        private string additionalInformationField;

        /// <remarks/>
        [XmlAttribute()]
        public string Owner
        {
            get
            {
                return ownerField;
            }
            set
            {
                ownerField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string Number
        {
            get
            {
                return numberField;
            }
            set
            {
                numberField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string additionalInformation
        {
            get
            {
                return additionalInformationField;
            }
            set
            {
                additionalInformationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class SystemInformation
    {

        private string codeField;

        private string descriptionField;

        /// <remarks/>
        [XmlAttribute()]
        public string code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_ShowAndEventTypeTicket
    {

        private string numberField;

        /// <remarks/>
        [XmlAttribute()]
        public string number
        {
            get
            {
                return numberField;
            }
            set
            {
                numberField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_ShowAndEventTypeStart
    {

        private string locationCodeField;

        private ETR_ShowAndEventTypeStartAddress addressField;

        private ETR_ShowAndEventTypeStartContact contactField;

        private string dateTimeField;

        /// <remarks/>
        public string locationCode
        {
            get
            {
                return locationCodeField;
            }
            set
            {
                locationCodeField = value;
            }
        }

        /// <remarks/>
        public ETR_ShowAndEventTypeStartAddress address
        {
            get
            {
                return addressField;
            }
            set
            {
                addressField = value;
            }
        }

        /// <remarks/>
        public ETR_ShowAndEventTypeStartContact contact
        {
            get
            {
                return contactField;
            }
            set
            {
                contactField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string dateTime
        {
            get
            {
                return dateTimeField;
            }
            set
            {
                dateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_ShowAndEventTypeStartAddress
    {

        private string lineField;

        private string complementField;

        private string zipField;

        private string countryCodeField;

        private string latitudeField;

        private string longitudeField;

        private string cityCodeField;

        private string cityNameField;

        private string countryNameField;

        private string stateCodeField;

        private string stateNameField;

        private string postalBoxField;

        /// <remarks/>
        [XmlAttribute()]
        public string line
        {
            get
            {
                return lineField;
            }
            set
            {
                lineField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string complement
        {
            get
            {
                return complementField;
            }
            set
            {
                complementField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string zip
        {
            get
            {
                return zipField;
            }
            set
            {
                zipField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string countryCode
        {
            get
            {
                return countryCodeField;
            }
            set
            {
                countryCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string latitude
        {
            get
            {
                return latitudeField;
            }
            set
            {
                latitudeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string longitude
        {
            get
            {
                return longitudeField;
            }
            set
            {
                longitudeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string cityCode
        {
            get
            {
                return cityCodeField;
            }
            set
            {
                cityCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string cityName
        {
            get
            {
                return cityNameField;
            }
            set
            {
                cityNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string countryName
        {
            get
            {
                return countryNameField;
            }
            set
            {
                countryNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string stateCode
        {
            get
            {
                return stateCodeField;
            }
            set
            {
                stateCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string stateName
        {
            get
            {
                return stateNameField;
            }
            set
            {
                stateNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string postalBox
        {
            get
            {
                return postalBoxField;
            }
            set
            {
                postalBoxField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_ShowAndEventTypeStartContact
    {

        private string phoneField;

        private string faxField;

        private string emailField;

        /// <remarks/>
        [XmlAttribute()]
        public string phone
        {
            get
            {
                return phoneField;
            }
            set
            {
                phoneField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string fax
        {
            get
            {
                return faxField;
            }
            set
            {
                faxField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string email
        {
            get
            {
                return emailField;
            }
            set
            {
                emailField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_ShowAndEventTypeEnd
    {

        private string dateTimeField;

        /// <remarks/>
        [XmlAttribute()]
        public string dateTime
        {
            get
            {
                return dateTimeField;
            }
            set
            {
                dateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_ShowAndEventTypeValidity
    {

        private string startDateTimeField;

        private string endDateTimeField;

        /// <remarks/>
        [XmlAttribute()]
        public string startDateTime
        {
            get
            {
                return startDateTimeField;
            }
            set
            {
                startDateTimeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string endDateTime
        {
            get
            {
                return endDateTimeField;
            }
            set
            {
                endDateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_ShowAndEventTypeRate
    {

        private string descriptionField;

        private string codeField;

        private string inclusionsField;

        /// <remarks/>
        [XmlAttribute()]
        public string description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string inclusions
        {
            get
            {
                return inclusionsField;
            }
            set
            {
                inclusionsField = value;
            }
        }
    }



    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_SleepMiscellaneousType
    {

        private ProviderType serviceProviderField;

        private ETR_SleepMiscellaneousTypeStart startField;

        private ETR_SleepMiscellaneousTypeEnd endField;

        private ETR_SleepMiscellaneousTypeCheckIn checkInField;

        private ETR_SleepMiscellaneousTypeCheckOut checkOutField;

        private ETR_SleepMiscellaneousTypeCustomers customersField;

        private ETR_SleepMiscellaneousTypeGuarantee guaranteeField;

        private ETR_SleepMiscellaneousTypeDeposit depositField;

        private CreationType creationField;

        private ModificationType modificationField;

        private ConfirmationType confirmationField;

        private ETR_TransferTypeDescriptions[] descriptionsField;

        private BookingChannelType bkgChannelField;

        private FullOriginSystemType creationChannelField;

        private ExternalSystemType externalSystemField;

        private SystemInformation creatorField;

        private string propertiesField;

        private string descriptionField;

        private string nameField;

        private string statusField;

        private string nIPField;

        private string additionalServicesField;

        private string roomRateDescriptionField;

        private string cancelPoliciesField;

        private string inclusionsField;

        private string otherRulesField;

        private string confirmNbrField;

        private string cancelNbrField;

        private string overrideStatusField;

        private string mBOProductCodeField;

        /// <remarks/>
        public ProviderType serviceProvider
        {
            get
            {
                return serviceProviderField;
            }
            set
            {
                serviceProviderField = value;
            }
        }

        /// <remarks/>
        public ETR_SleepMiscellaneousTypeStart start
        {
            get
            {
                return startField;
            }
            set
            {
                startField = value;
            }
        }

        /// <remarks/>
        public ETR_SleepMiscellaneousTypeEnd end
        {
            get
            {
                return endField;
            }
            set
            {
                endField = value;
            }
        }

        /// <remarks/>
        public ETR_SleepMiscellaneousTypeCheckIn checkIn
        {
            get
            {
                return checkInField;
            }
            set
            {
                checkInField = value;
            }
        }

        /// <remarks/>
        public ETR_SleepMiscellaneousTypeCheckOut checkOut
        {
            get
            {
                return checkOutField;
            }
            set
            {
                checkOutField = value;
            }
        }

        /// <remarks/>
        public ETR_SleepMiscellaneousTypeCustomers customers
        {
            get
            {
                return customersField;
            }
            set
            {
                customersField = value;
            }
        }

        /// <remarks/>
        public ETR_SleepMiscellaneousTypeGuarantee guarantee
        {
            get
            {
                return guaranteeField;
            }
            set
            {
                guaranteeField = value;
            }
        }

        /// <remarks/>
        public ETR_SleepMiscellaneousTypeDeposit deposit
        {
            get
            {
                return depositField;
            }
            set
            {
                depositField = value;
            }
        }

        /// <remarks/>
        public CreationType creation
        {
            get
            {
                return creationField;
            }
            set
            {
                creationField = value;
            }
        }

        /// <remarks/>
        public ModificationType modification
        {
            get
            {
                return modificationField;
            }
            set
            {
                modificationField = value;
            }
        }

        /// <remarks/>
        public ConfirmationType confirmation
        {
            get
            {
                return confirmationField;
            }
            set
            {
                confirmationField = value;
            }
        }

        /// <remarks/>
        [XmlElement("descriptions")]
        public ETR_TransferTypeDescriptions[] descriptions
        {
            get
            {
                return descriptionsField;
            }
            set
            {
                descriptionsField = value;
            }
        }

        /// <remarks/>
        public BookingChannelType bkgChannel
        {
            get
            {
                return bkgChannelField;
            }
            set
            {
                bkgChannelField = value;
            }
        }

        /// <remarks/>
        public FullOriginSystemType creationChannel
        {
            get
            {
                return creationChannelField;
            }
            set
            {
                creationChannelField = value;
            }
        }

        /// <remarks/>
        public ExternalSystemType externalSystem
        {
            get
            {
                return externalSystemField;
            }
            set
            {
                externalSystemField = value;
            }
        }

        /// <remarks/>
        public SystemInformation creator
        {
            get
            {
                return creatorField;
            }
            set
            {
                creatorField = value;
            }
        }

        /// <remarks/>
        public string properties
        {
            get
            {
                return propertiesField;
            }
            set
            {
                propertiesField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string status
        {
            get
            {
                return statusField;
            }
            set
            {
                statusField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NIP
        {
            get
            {
                return nIPField;
            }
            set
            {
                nIPField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string additionalServices
        {
            get
            {
                return additionalServicesField;
            }
            set
            {
                additionalServicesField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string roomRateDescription
        {
            get
            {
                return roomRateDescriptionField;
            }
            set
            {
                roomRateDescriptionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string cancelPolicies
        {
            get
            {
                return cancelPoliciesField;
            }
            set
            {
                cancelPoliciesField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string inclusions
        {
            get
            {
                return inclusionsField;
            }
            set
            {
                inclusionsField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string otherRules
        {
            get
            {
                return otherRulesField;
            }
            set
            {
                otherRulesField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string confirmNbr
        {
            get
            {
                return confirmNbrField;
            }
            set
            {
                confirmNbrField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string cancelNbr
        {
            get
            {
                return cancelNbrField;
            }
            set
            {
                cancelNbrField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string overrideStatus
        {
            get
            {
                return overrideStatusField;
            }
            set
            {
                overrideStatusField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string MBOProductCode
        {
            get
            {
                return mBOProductCodeField;
            }
            set
            {
                mBOProductCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_SleepMiscellaneousTypeStart
    {

        private string locationCodeField;

        private ETR_SleepMiscellaneousTypeStartAddress addressField;

        private ETR_SleepMiscellaneousTypeStartContact contactField;

        private string dateTimeField;

        /// <remarks/>
        public string locationCode
        {
            get
            {
                return locationCodeField;
            }
            set
            {
                locationCodeField = value;
            }
        }

        /// <remarks/>
        public ETR_SleepMiscellaneousTypeStartAddress address
        {
            get
            {
                return addressField;
            }
            set
            {
                addressField = value;
            }
        }

        /// <remarks/>
        public ETR_SleepMiscellaneousTypeStartContact contact
        {
            get
            {
                return contactField;
            }
            set
            {
                contactField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string dateTime
        {
            get
            {
                return dateTimeField;
            }
            set
            {
                dateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_SleepMiscellaneousTypeStartAddress
    {

        private string lineField;

        private string complementField;

        private string zipField;

        private string countryCodeField;

        private string latitudeField;

        private string longitudeField;

        private string cityCodeField;

        private string cityNameField;

        private string countryNameField;

        private string stateCodeField;

        private string stateNameField;

        private string postalBoxField;

        /// <remarks/>
        [XmlAttribute()]
        public string line
        {
            get
            {
                return lineField;
            }
            set
            {
                lineField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string complement
        {
            get
            {
                return complementField;
            }
            set
            {
                complementField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string zip
        {
            get
            {
                return zipField;
            }
            set
            {
                zipField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string countryCode
        {
            get
            {
                return countryCodeField;
            }
            set
            {
                countryCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string latitude
        {
            get
            {
                return latitudeField;
            }
            set
            {
                latitudeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string longitude
        {
            get
            {
                return longitudeField;
            }
            set
            {
                longitudeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string cityCode
        {
            get
            {
                return cityCodeField;
            }
            set
            {
                cityCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string cityName
        {
            get
            {
                return cityNameField;
            }
            set
            {
                cityNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string countryName
        {
            get
            {
                return countryNameField;
            }
            set
            {
                countryNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string stateCode
        {
            get
            {
                return stateCodeField;
            }
            set
            {
                stateCodeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string stateName
        {
            get
            {
                return stateNameField;
            }
            set
            {
                stateNameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string postalBox
        {
            get
            {
                return postalBoxField;
            }
            set
            {
                postalBoxField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_SleepMiscellaneousTypeStartContact
    {

        private string phoneField;

        private string faxField;

        private string emailField;

        /// <remarks/>
        [XmlAttribute()]
        public string phone
        {
            get
            {
                return phoneField;
            }
            set
            {
                phoneField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string fax
        {
            get
            {
                return faxField;
            }
            set
            {
                faxField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string email
        {
            get
            {
                return emailField;
            }
            set
            {
                emailField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_SleepMiscellaneousTypeEnd
    {

        private string dateTimeField;

        /// <remarks/>
        [XmlAttribute()]
        public string dateTime
        {
            get
            {
                return dateTimeField;
            }
            set
            {
                dateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_SleepMiscellaneousTypeCheckIn
    {

        private string startDateTimeField;

        /// <remarks/>
        [XmlAttribute()]
        public string startDateTime
        {
            get
            {
                return startDateTimeField;
            }
            set
            {
                startDateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_SleepMiscellaneousTypeCheckOut
    {

        private string endDateTimeField;

        /// <remarks/>
        [XmlAttribute()]
        public string endDateTime
        {
            get
            {
                return endDateTimeField;
            }
            set
            {
                endDateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_SleepMiscellaneousTypeCustomers
    {

        private string adultsField;

        private ETR_SleepMiscellaneousTypeCustomersChildren[] childrenField;

        /// <remarks/>
        public string adults
        {
            get
            {
                return adultsField;
            }
            set
            {
                adultsField = value;
            }
        }

        /// <remarks/>
        [XmlElement("children")]
        public ETR_SleepMiscellaneousTypeCustomersChildren[] children
        {
            get
            {
                return childrenField;
            }
            set
            {
                childrenField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_SleepMiscellaneousTypeCustomersChildren
    {

        private string ageField;

        private string ageCodeField;

        private string valueField;

        /// <remarks/>
        [XmlAttribute()]
        public string age
        {
            get
            {
                return ageField;
            }
            set
            {
                ageField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string ageCode
        {
            get
            {
                return ageCodeField;
            }
            set
            {
                ageCodeField = value;
            }
        }

        /// <remarks/>
        [XmlText()]
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_SleepMiscellaneousTypeGuarantee
    {

        private string paymentFormField;

        private string paymentDetailsField;

        /// <remarks/>
        [XmlAttribute()]
        public string paymentForm
        {
            get
            {
                return paymentFormField;
            }
            set
            {
                paymentFormField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string paymentDetails
        {
            get
            {
                return paymentDetailsField;
            }
            set
            {
                paymentDetailsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_SleepMiscellaneousTypeDeposit
    {

        private string paymentFormField;

        private string paymentDetailsField;

        /// <remarks/>
        [XmlAttribute()]
        public string paymentForm
        {
            get
            {
                return paymentFormField;
            }
            set
            {
                paymentFormField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string paymentDetails
        {
            get
            {
                return paymentDetailsField;
            }
            set
            {
                paymentDetailsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ETR_ShowAndEventType
    {

        private ProviderType serviceProviderField;

        private string[] seatNbrField;

        private ETR_ShowAndEventTypeTicket ticketField;

        private ETR_ShowAndEventTypeStart startField;

        private ETR_ShowAndEventTypeEnd endField;

        private ETR_ShowAndEventTypeValidity validityField;

        private ETR_ShowAndEventTypeRate rateField;

        private CreationType creationField;

        private ModificationType modificationField;

        private ConfirmationType confirmationField;

        private ETR_TransferTypeDescriptions[] descriptionsField;

        private BookingChannelType bkgChannelField;

        private FullOriginSystemType creationChannelField;

        private ExternalSystemType externalSystemField;

        private SystemInformation creatorField;

        private string propertiesField;

        private string descriptionField;

        private string nameField;

        private string durationField;

        private string nIPField;

        private string statusField;

        private string confirmNbrField;

        private string cancelNbrField;

        private string overrideStatusField;

        private string mBOProductCodeField;

        /// <remarks/>
        public ProviderType serviceProvider
        {
            get
            {
                return serviceProviderField;
            }
            set
            {
                serviceProviderField = value;
            }
        }

        /// <remarks/>
        [XmlElement("seatNbr")]
        public string[] seatNbr
        {
            get
            {
                return seatNbrField;
            }
            set
            {
                seatNbrField = value;
            }
        }

        /// <remarks/>
        public ETR_ShowAndEventTypeTicket ticket
        {
            get
            {
                return ticketField;
            }
            set
            {
                ticketField = value;
            }
        }

        /// <remarks/>
        public ETR_ShowAndEventTypeStart start
        {
            get
            {
                return startField;
            }
            set
            {
                startField = value;
            }
        }

        /// <remarks/>
        public ETR_ShowAndEventTypeEnd end
        {
            get
            {
                return endField;
            }
            set
            {
                endField = value;
            }
        }

        /// <remarks/>
        public ETR_ShowAndEventTypeValidity validity
        {
            get
            {
                return validityField;
            }
            set
            {
                validityField = value;
            }
        }

        /// <remarks/>
        public ETR_ShowAndEventTypeRate rate
        {
            get
            {
                return rateField;
            }
            set
            {
                rateField = value;
            }
        }

        /// <remarks/>
        public CreationType creation
        {
            get
            {
                return creationField;
            }
            set
            {
                creationField = value;
            }
        }

        /// <remarks/>
        public ModificationType modification
        {
            get
            {
                return modificationField;
            }
            set
            {
                modificationField = value;
            }
        }

        /// <remarks/>
        public ConfirmationType confirmation
        {
            get
            {
                return confirmationField;
            }
            set
            {
                confirmationField = value;
            }
        }

        /// <remarks/>
        [XmlElement("descriptions")]
        public ETR_TransferTypeDescriptions[] descriptions
        {
            get
            {
                return descriptionsField;
            }
            set
            {
                descriptionsField = value;
            }
        }

        /// <remarks/>
        public BookingChannelType bkgChannel
        {
            get
            {
                return bkgChannelField;
            }
            set
            {
                bkgChannelField = value;
            }
        }

        /// <remarks/>
        public FullOriginSystemType creationChannel
        {
            get
            {
                return creationChannelField;
            }
            set
            {
                creationChannelField = value;
            }
        }

        /// <remarks/>
        public ExternalSystemType externalSystem
        {
            get
            {
                return externalSystemField;
            }
            set
            {
                externalSystemField = value;
            }
        }

        /// <remarks/>
        public SystemInformation creator
        {
            get
            {
                return creatorField;
            }
            set
            {
                creatorField = value;
            }
        }

        /// <remarks/>
        public string properties
        {
            get
            {
                return propertiesField;
            }
            set
            {
                propertiesField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string description
        {
            get
            {
                return descriptionField;
            }
            set
            {
                descriptionField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string duration
        {
            get
            {
                return durationField;
            }
            set
            {
                durationField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string NIP
        {
            get
            {
                return nIPField;
            }
            set
            {
                nIPField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string status
        {
            get
            {
                return statusField;
            }
            set
            {
                statusField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string confirmNbr
        {
            get
            {
                return confirmNbrField;
            }
            set
            {
                confirmNbrField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string cancelNbr
        {
            get
            {
                return cancelNbrField;
            }
            set
            {
                cancelNbrField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string overrideStatus
        {
            get
            {
                return overrideStatusField;
            }
            set
            {
                overrideStatusField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string MBOProductCode
        {
            get
            {
                return mBOProductCodeField;
            }
            set
            {
                mBOProductCodeField = value;
            }
        }
    }


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(Namespace = "http://xml.amadeus.com/2010/06/ETR_Types_v4")]
    public partial class ProviderType
    {

        private string refField;

        private string nameField;

        private string codeField;

        private string externalRefField;

        /// <remarks/>
        [XmlAttribute()]
        public string @ref
        {
            get
            {
                return refField;
            }
            set
            {
                refField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string code
        {
            get
            {
                return codeField;
            }
            set
            {
                codeField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute()]
        public string externalRef
        {
            get
            {
                return externalRefField;
            }
            set
            {
                externalRefField = value;
            }
        }
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        //[XmlElement("Source")]
        //public Source[] Source;
        public TPA_Extensions TPA_Extensions;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        // <remarks/>
        public Provider Provider;

        // <remarks/>
        public string MoreIndicator;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        // <remarks/>
        [XmlElement("Name")]
        public Name[] Name;

        // <remarks/>
        [XmlElement("System")]
        public string GDSSystem;

        // <remarks/>
        public string Userid;

        // <remarks/>
        public string Password;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Name
    {

        // <remarks/>
        [XmlAttribute()]
        public string PseudoCityCode;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Source
    {

        // <remarks/>
        public RequestorID RequestorID;

        // <remarks/>
        public Position Position;

        // <remarks/>
        public BookingChannel BookingChannel;

        // <remarks/>
        [XmlAttribute()]
        public string AgentSine;

        // <remarks/>
        [XmlAttribute()]
        public string PseudoCityCode;

        // <remarks/>
        [XmlAttribute()]
        public string ISOCountry;

        // <remarks/>
        [XmlAttribute()]
        public string ISOCurrency;

        // <remarks/>
        [XmlAttribute()]
        public string AgentDutyCode;

        // <remarks/>
        [XmlAttribute()]
        public string AirlineVendorID;

        // <remarks/>
        [XmlAttribute()]
        public string AirportCode;

        // <remarks/>
        [XmlAttribute()]
        public string FirstDepartPoint;

        // <remarks/>
        [XmlAttribute()]
        public string ERSP_UserID;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class RequestorID
    {

        // <remarks/>
        public Code.CompanyName CompanyName;

        // <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string URL;

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        public string Instance;

        // <remarks/>
        [XmlAttribute()]
        public string ID;

        // <remarks/>
        [XmlAttribute()]
        public string ID_Context;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Position
    {

        // <remarks/>
        [XmlAttribute()]
        public string Latitude;

        // <remarks/>
        [XmlAttribute()]
        public string Longitude;

        // <remarks/>
        [XmlAttribute()]
        public string Altitude;
    }

    // local CompanyName removed: identical to shared wsTripXML.Code.CompanyName (XML type-name collision in one serializer scope)

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class BookingChannel
    {

        // <remarks/>
        public Code.CompanyName CompanyName;

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        public bool Primary;

        // <remarks/>
        [XmlIgnore()]
        public bool PrimarySpecified;
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum Target
    {

        /// <remarks/>
        Test,

        /// <remarks/>
        Production
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    public enum TransactionStatusCode
    {

        /// <remarks/>
        Start,

        /// <remarks/>
        End,

        /// <remarks/>
        Rollback,

        /// <remarks/>
        InSeries,

        /// <remarks/>
        Continuation,

        /// <remarks/>
        Subsequent
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class Reservation
    {

        /// <remarks/>
        [XmlAttribute()]
        public string ID { get; set; }

        /// <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string ExternalID { get; set; }

    }

    #region Errors and Warnings
    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Success
    {
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Warning
    {

        // <remarks/>
        [XmlAttribute()]
        public string Language { get; set; }

        // <remarks/>
        [XmlAttribute()]
        public string Type { get; set; }

        // <remarks/>
        [XmlAttribute()]
        public string ShortText { get; set; }

        // <remarks/>
        [XmlAttribute()]
        public string Code { get; set; }

        // <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string DocURL { get; set; }

        // <remarks/>
        [XmlAttribute()]
        public string Status { get; set; }

        // <remarks/>
        [XmlAttribute()]
        public string Tag { get; set; }

        // <remarks/>
        [XmlAttribute()]
        public string RecordID { get; set; }

        // <remarks/>
        [XmlText()]
        public string Value { get; set; }
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Error
    {

        // <remarks/>
        [XmlAttribute()]
        public string Language { get; set; }

        // <remarks/>
        [XmlAttribute()]
        public string Type { get; set; }

        // <remarks/>
        [XmlAttribute()]
        public string ShortText { get; set; }

        // <remarks/>
        [XmlAttribute()]
        public string Code { get; set; }

        // <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string DocURL { get; set; }

        // <remarks/>
        [XmlAttribute()]
        public string Status { get; set; }

        // <remarks/>
        [XmlAttribute()]
        public string Tag { get; set; }

        // <remarks/>
        [XmlAttribute()]
        public string RecordID { get; set; }

        // <remarks/>
        [XmlAttribute()]
        public string NodeList { get; set; }

        // <remarks/>
        [XmlText()]
        public string Value { get; set; }
    }

    #endregion



}