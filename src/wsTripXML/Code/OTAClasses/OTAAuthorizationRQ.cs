using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmAuthorizationIn
{

    [XmlRoot(IsNullable = false)]
    public class OTA_AuthorizationRQ
    {

        public OTA_AuthorizationRQ() : base()
        {
            Target = OTA_AuthorizationRQTarget.Production;
        }

        public POS POS;

        [XmlElement()]
        public AuthorizationType AuthorizationDetail;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_AuthorizationRQTarget.Production)]
        public OTA_AuthorizationRQTarget Target = OTA_AuthorizationRQTarget.Production;

        [XmlAttribute()]
        public double Version;

        [XmlIgnore()]
        public bool VersionSpecified;

        [XmlAttribute()]
        public string TransactionIdentifier;

        [XmlAttribute()]
        public int SequenceNmbr;

        [XmlIgnore()]
        public bool SequenceNmbrSpecified;

        [XmlAttribute()]
        public OTA_AuthorizationRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public bool RetransmissionIndicator;

        [XmlIgnore()]
        public bool RetransmissionIndicatorSpecified;

    }


    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType()]
    [XmlRoot("TPA_Extensions", IsNullable = false)]
    public partial class TPA_ExtensionsType
    {

        private System.Xml.XmlElement[] anyField;

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

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType()]
    public partial class PersonNameType
    {

        private string[] namePrefixField;

        private string[] givenNameField;

        private string[] middleNameField;

        private string surnamePrefixField;

        private string surnameField;

        private string[] nameSuffixField;

        private string[] nameTitleField;

        private BankAcctTypeShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private BankAcctTypeShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string nameTypeField;

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

        [XmlAttribute()]
        public BankAcctTypeShareSynchInd ShareSynchInd
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

        [XmlAttribute()]
        public BankAcctTypeShareMarketInd ShareMarketInd
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

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public enum BankAcctTypeShareSynchInd
    {
        Yes,
        No,
        Inherit
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public enum BankAcctTypeShareMarketInd
    {
        Yes,
        No,
        Inherit
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType()]
    public partial class DocumentType
    {

        private object itemField;

        private string[] docLimitationsField;

        private string[] additionalPersonNamesField;

        private BankAcctTypeShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private BankAcctTypeShareMarketInd shareMarketIndField;

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

        private string docHolderNationalityField;

        private string contactNameField;

        private DocumentTypeHolderType holderTypeField;

        private bool holderTypeFieldSpecified;

        private string remarkField;

        private string postalCodeField;

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

        [XmlAttribute()]
        public BankAcctTypeShareSynchInd ShareSynchInd
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

        [XmlAttribute()]
        public BankAcctTypeShareMarketInd ShareMarketInd
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

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public enum DocumentTypeGender
    {
        Male,
        Female,
        Unknown
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public enum DocumentTypeHolderType
    {
        Infant,
        HeadOfHousehold
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType()]
    public partial class CountryNameType
    {
        private string codeField;

        private string valueField;

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

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType()]
    public partial class StateProvType
    {

        private string stateCodeField;

        private string valueField;

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

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType()]
    public partial class StreetNmbrType
    {

        private string pO_BoxField;

        private string valueField;

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

    [XmlInclude(typeof(AddressInfoType))]
    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType()]
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

        private BankAcctTypeShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private BankAcctTypeShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string typeField;

        private string remarkField;

        public AddressType() : base()
        {
            formattedIndField = false;
        }

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

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
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

        [XmlAttribute()]
        public BankAcctTypeShareSynchInd ShareSynchInd
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

        [XmlAttribute()]
        public BankAcctTypeShareMarketInd ShareMarketInd
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

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType(AnonymousType = true)]
    public partial class AddressTypeStreetNmbr : StreetNmbrType
    {

        private string streetNmbrSuffixField;

        private string streetDirectionField;

        private string ruralRouteNmbrField;

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

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType(AnonymousType = true)]
    public partial class AddressTypeBldgRoom
    {

        private bool bldgNameIndicatorField;

        private bool bldgNameIndicatorFieldSpecified;

        private string valueField;

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

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType()]
    public partial class AddressInfoType : AddressType
    {

        private bool defaultIndField;

        private string useTypeField;

        private string rPHField;

        public AddressInfoType() : base()
        {
            defaultIndField = false;
        }

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
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

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType()]
    public partial class PaymentCardType
    {

        private string cardHolderNameField;

        private PaymentCardTypeCardIssuerName cardIssuerNameField;

        private AddressType addressField;

        private PaymentCardTypeTelephone[] telephoneField;

        private PaymentCardTypeSignatureOnFile signatureOnFileField;

        private BankAcctTypeShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private BankAcctTypeShareMarketInd shareMarketIndField;

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

        [XmlAttribute()]
        public BankAcctTypeShareSynchInd ShareSynchInd
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

        [XmlAttribute()]
        public BankAcctTypeShareMarketInd ShareMarketInd
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
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType(AnonymousType = true)]
    public partial class PaymentCardTypeCardIssuerName
    {

        private string bankIDField;

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

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType(AnonymousType = true)]
    public partial class PaymentCardTypeTelephone
    {

        private string rPHField;

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

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType(AnonymousType = true)]
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

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType()]
    public partial class BankAcctType
    {

        private string bankAcctNameField;

        private BankAcctTypeShareSynchInd shareSynchIndField;

        private bool shareSynchIndFieldSpecified;

        private BankAcctTypeShareMarketInd shareMarketIndField;

        private bool shareMarketIndFieldSpecified;

        private string bankIDField;

        private string acctTypeField;

        private string bankAcctNumberField;

        private bool checksAcceptedIndField;

        private bool checksAcceptedIndFieldSpecified;

        public string BankAcctName
        {
            get
            {
                return bankAcctNameField;
            }
            set
            {
                bankAcctNameField = value;
            }
        }

        [XmlAttribute()]
        public BankAcctTypeShareSynchInd ShareSynchInd
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

        [XmlAttribute()]
        public BankAcctTypeShareMarketInd ShareMarketInd
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

        [XmlAttribute()]
        public string AcctType
        {
            get
            {
                return acctTypeField;
            }
            set
            {
                acctTypeField = value;
            }
        }

        [XmlAttribute()]
        public string BankAcctNumber
        {
            get
            {
                return bankAcctNumberField;
            }
            set
            {
                bankAcctNumberField = value;
            }
        }

        [XmlAttribute()]
        public bool ChecksAcceptedInd
        {
            get
            {
                return checksAcceptedIndField;
            }
            set
            {
                checksAcceptedIndField = value;
            }
        }

        [XmlIgnore()]
        public bool ChecksAcceptedIndSpecified
        {
            get
            {
                return checksAcceptedIndFieldSpecified;
            }
            set
            {
                checksAcceptedIndFieldSpecified = value;
            }
        }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType()]
    public partial class AuthorizationType
    {

        private object itemField;

        private DocumentType driversLicenseAuthorizationField;

        private AuthorizationTypeBookingReferenceID bookingReferenceIDField;

        private string principalCompanyCodeField;

        private string refNumberField;

        [XmlElement("CheckAuthorization", typeof(AuthorizationTypeCheckAuthorization))]
        [XmlElement("CreditCardAuthorization", typeof(AuthorizationTypeCreditCardAuthorization))]
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

        public DocumentType DriversLicenseAuthorization
        {
            get
            {
                return driversLicenseAuthorizationField;
            }
            set
            {
                driversLicenseAuthorizationField = value;
            }
        }

        public AuthorizationTypeBookingReferenceID BookingReferenceID
        {
            get
            {
                return bookingReferenceIDField;
            }
            set
            {
                bookingReferenceIDField = value;
            }
        }

        [XmlAttribute()]
        public string PrincipalCompanyCode
        {
            get
            {
                return principalCompanyCodeField;
            }
            set
            {
                principalCompanyCodeField = value;
            }
        }

        [XmlAttribute()]
        public string RefNumber
        {
            get
            {
                return refNumberField;
            }
            set
            {
                refNumberField = value;
            }
        }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType(AnonymousType = true)]
    public partial class AuthorizationTypeCheckAuthorization
    {

        private BankAcctType bankAcctField;

        private decimal amountField;

        private bool amountFieldSpecified;

        public BankAcctType BankAcct
        {
            get
            {
                return bankAcctField;
            }
            set
            {
                bankAcctField = value;
            }
        }

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
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType(AnonymousType = true)]
    public partial class AuthorizationTypeCreditCardAuthorization
    {

        private PaymentCardType creditCardField;

        private UniqueID_Type[] idField;

        private decimal amountField;

        private bool amountFieldSpecified;

        private AuthorizationTypeCreditCardAuthorizationSourceType sourceTypeField;

        private bool sourceTypeFieldSpecified;

        private bool extendedPaymentIndField;

        private bool extendedPaymentIndFieldSpecified;

        private string extendedPaymentQuantityField;

        private TimeUnitType extendedPaymentFrequencyField;

        private bool extendedPaymentFrequencyFieldSpecified;

        private string authorizationCodeField;

        private bool reversalIndicatorField;

        private bool reversalIndicatorFieldSpecified;

        private bool cardPresentIndField;

        private bool cardPresentIndFieldSpecified;

        private string e_CommerceCodeField;

        private string authTransactionIDField;

        private string authVerificationValueField;

        public PaymentCardType CreditCard
        {
            get
            {
                return creditCardField;
            }
            set
            {
                creditCardField = value;
            }
        }

        [XmlElement("ID")]
        public UniqueID_Type[] ID
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

        [XmlAttribute()]
        public AuthorizationTypeCreditCardAuthorizationSourceType SourceType
        {
            get
            {
                return sourceTypeField;
            }
            set
            {
                sourceTypeField = value;
            }
        }

        [XmlIgnore()]
        public bool SourceTypeSpecified
        {
            get
            {
                return sourceTypeFieldSpecified;
            }
            set
            {
                sourceTypeFieldSpecified = value;
            }
        }

        [XmlAttribute()]
        public bool ExtendedPaymentInd
        {
            get
            {
                return extendedPaymentIndField;
            }
            set
            {
                extendedPaymentIndField = value;
            }
        }

        [XmlIgnore()]
        public bool ExtendedPaymentIndSpecified
        {
            get
            {
                return extendedPaymentIndFieldSpecified;
            }
            set
            {
                extendedPaymentIndFieldSpecified = value;
            }
        }

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

        [XmlAttribute()]
        public TimeUnitType ExtendedPaymentFrequency
        {
            get
            {
                return extendedPaymentFrequencyField;
            }
            set
            {
                extendedPaymentFrequencyField = value;
            }
        }

        [XmlIgnore()]
        public bool ExtendedPaymentFrequencySpecified
        {
            get
            {
                return extendedPaymentFrequencyFieldSpecified;
            }
            set
            {
                extendedPaymentFrequencyFieldSpecified = value;
            }
        }

        [XmlAttribute()]
        public string AuthorizationCode
        {
            get
            {
                return authorizationCodeField;
            }
            set
            {
                authorizationCodeField = value;
            }
        }

        [XmlAttribute()]
        public bool ReversalIndicator
        {
            get
            {
                return reversalIndicatorField;
            }
            set
            {
                reversalIndicatorField = value;
            }
        }

        [XmlIgnore()]
        public bool ReversalIndicatorSpecified
        {
            get
            {
                return reversalIndicatorFieldSpecified;
            }
            set
            {
                reversalIndicatorFieldSpecified = value;
            }
        }

        [XmlAttribute()]
        public bool CardPresentInd
        {
            get
            {
                return cardPresentIndField;
            }
            set
            {
                cardPresentIndField = value;
            }
        }

        [XmlIgnore()]
        public bool CardPresentIndSpecified
        {
            get
            {
                return cardPresentIndFieldSpecified;
            }
            set
            {
                cardPresentIndFieldSpecified = value;
            }
        }

        [XmlAttribute()]
        public string E_CommerceCode
        {
            get
            {
                return e_CommerceCodeField;
            }
            set
            {
                e_CommerceCodeField = value;
            }
        }

        [XmlAttribute()]
        public string AuthTransactionID
        {
            get
            {
                return authTransactionIDField;
            }
            set
            {
                authTransactionIDField = value;
            }
        }

        [XmlAttribute()]
        public string AuthVerificationValue
        {
            get
            {
                return authVerificationValueField;
            }
            set
            {
                authVerificationValueField = value;
            }
        }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType()]
    public partial class UniqueID_Type
    {

        private CompanyName companyNameField;

        private string uRLField;

        private string typeField;

        private string instanceField;

        private string iD_ContextField;

        public CompanyName CompanyName
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

        [XmlAttribute()]
        public string ID
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

    [XmlInclude(typeof(OperatingAirlineType))]
    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType()]
    public partial class CompanyName
    {

        private string companyShortNameField;

        private string travelSectorField;

        private string codeField;

        private string codeContextField;

        private string divisionField;

        private string departmentField;

        private string valueField;

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

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType()]
    public partial class OperatingAirlineType : CompanyName
    {

        private string flightNumberField;

        private string resBookDesigCodeField;

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

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public enum AuthorizationTypeCreditCardAuthorizationSourceType
    {
        NormalTransaction,
        MailOrPhoneOrder,
        UnattendedTerminal,
        MerchantIsSuspicious,
        eCommerceSecuredTransaction,
        eCommerceUnsecuredTransaction,
        InFlightAirPhone,
        CID_NotLegible,
        CID_NotOnCard
    }

    [Serializable()]
    [XmlType()]
    public enum TimeUnitType
    {
        Year,
        Month,
        Week,
        Day,
        Hour,
        Second,
        FullDuration
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType(AnonymousType = true)]
    public partial class AuthorizationTypeBookingReferenceID : UniqueID_Type
    {

        private bool ignoreReservationIndField;

        private bool ignoreReservationIndFieldSpecified;

        [XmlAttribute()]
        public bool IgnoreReservationInd
        {
            get
            {
                return ignoreReservationIndField;
            }
            set
            {
                ignoreReservationIndField = value;
            }
        }

        [XmlIgnore()]
        public bool IgnoreReservationIndSpecified
        {
            get
            {
                return ignoreReservationIndFieldSpecified;
            }
            set
            {
                ignoreReservationIndFieldSpecified = value;
            }
        }
    }


    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;
    }

    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        public string Name;

        public string System;

        public string Userid;

        public string Password;
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class SourceTypeRequestorID : UniqueID_Type
    {

        private string messagePasswordField;

        [XmlAttribute()]
        public string MessagePassword
        {
            get
            {
                return messagePasswordField;
            }
            set
            {
                messagePasswordField = value;
            }
        }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType(AnonymousType = true)]
    public partial class SourceTypePosition
    {

        private string latitudeField;

        private string longitudeField;

        private string altitudeField;

        private string altitudeUnitOfMeasureCodeField;

        [XmlAttribute()]
        public string Latitude
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

        [XmlAttribute()]
        public string Longitude
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

        [XmlAttribute()]
        public string Altitude
        {
            get
            {
                return altitudeField;
            }
            set
            {
                altitudeField = value;
            }
        }

        [XmlAttribute()]
        public string AltitudeUnitOfMeasureCode
        {
            get
            {
                return altitudeUnitOfMeasureCodeField;
            }
            set
            {
                altitudeUnitOfMeasureCodeField = value;
            }
        }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [XmlType(AnonymousType = true)]
    public partial class SourceTypeBookingChannel
    {

        private CompanyName companyNameField;

        private string typeField;

        private bool primaryField;

        private bool primaryFieldSpecified;

        public CompanyName CompanyName
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

        [XmlAttribute()]
        public bool Primary
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

        [XmlIgnore()]
        public bool PrimarySpecified
        {
            get
            {
                return primaryFieldSpecified;
            }
            set
            {
                primaryFieldSpecified = value;
            }
        }
    }
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }


    [Serializable()]
    [XmlType(AnonymousType = true)]
    public enum OTA_AuthorizationRQTarget
    {
        Test,
        Production
    }

    [Serializable()]
    [XmlType(AnonymousType = true)]
    public enum OTA_AuthorizationRQTransactionStatusCode
    {
        Start,
        End,
        Rollback,
        InSeries,
        Continuation,
        Subsequent
    }
}
