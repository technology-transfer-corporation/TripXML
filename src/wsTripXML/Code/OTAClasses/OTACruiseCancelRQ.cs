using System;
using System.Xml.Serialization;
using wsTripXML.wsTravelTalk.wmCruiseCancel;

namespace wsTripXML.wsTravelTalk.wmCruiseCancelIn
{

    [XmlRoot(IsNullable = false)]
    public class Address
    {

        public StreetNmbr StreetNmbr;

        public string BldgRoom;

        [XmlElement("AddressLine")]
        public string[] AddressLine;

        public string CityName;

        public string PostalCode;

        public string County;

        public StateProv StateProv;

        public CountryName CountryName;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        [XmlAttribute()]
        public AddressShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string Type;
    }

    [XmlRoot(IsNullable = false)]
    public class StreetNmbr
    {

        [XmlAttribute()]
        public string PO_Box;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class CountryName
    {

        [XmlAttribute()]
        public string Code;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class StateProv
    {

        [XmlAttribute()]
        public string StateCode;

        [XmlText()]
        public string Value;
    }

    public enum AddressShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum AddressShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class AddressInfo
    {

        public StreetNmbr StreetNmbr;

        public string BldgRoom;

        [XmlElement("AddressLine")]
        public string[] AddressLine;

        public string CityName;

        public string PostalCode;

        public string County;

        public StateProv StateProv;

        public CountryName CountryName;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        [XmlAttribute()]
        public AddressInfoShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public AddressInfoShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public string UseType;
    }

    public enum AddressInfoShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum AddressInfoShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class AssociatedQuantity
    {

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute(DataType = "anyURI")]
        public string URI;

        [XmlAttribute()]
        public int Quantity;

        [XmlIgnore()]
        public bool QuantitySpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class BookingChannel
    {

        public Code.CompanyName CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public bool Primary;

        [XmlIgnore()]
        public bool PrimarySpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class CardIssuerName
    {

        [XmlAttribute()]
        public string BankID;
    }

    [XmlRoot(IsNullable = false)]
    public class CustLoyalty
    {

        [XmlAttribute()]
        public CustLoyaltyShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public CustLoyaltyShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string ProgramID;

        [XmlAttribute()]
        public string MembershipID;

        [XmlAttribute()]
        public string TravelSector;

        [XmlAttribute()]
        public string LoyalLevel;

        [XmlAttribute()]
        public CustLoyaltySingleVendorInd SingleVendorInd;

        [XmlIgnore()]
        public bool SingleVendorIndSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime SignupDate;

        [XmlIgnore()]
        public bool SignupDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        [XmlIgnore()]
        public bool ExpireDateSpecified;

        [XmlAttribute()]
        public string RPH;
    }

    public enum CustLoyaltyShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum CustLoyaltyShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum CustLoyaltySingleVendorInd
    {

        SingleVndr,

        Alliance
    }

    [XmlRoot(IsNullable = false)]
    public class Email
    {

        [XmlAttribute()]
        public EmailShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public EmailShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public string EmailType;

        [XmlText()]
        public string Value;
    }

    public enum EmailShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum EmailShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class EndLocation
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlAttribute()]
        public DateTime AssociatedDateTime;

        [XmlIgnore()]
        public bool AssociatedDateTimeSpecified;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_CruiseCancelRQ
    {

        public POS POS;

        [XmlElement("UniqueID")]
        public UniqueID[] UniqueID;

        public Verification Verification;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_CancelRQTarget.Production)]
        public OTA_CancelRQTarget Target = OTA_CancelRQTarget.Production;

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
        public OTA_CancelRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public string ReqRespVersion;

        [XmlAttribute()]
        public OTA_CancelRQCancelType CancelType;

        [XmlIgnore()]
        public bool CancelTypeSpecified;
    }
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
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

    [XmlRoot(IsNullable = false)]
    public class Verification
    {

        public PersonName PersonName;

        public Email Email;

        public TelephoneInfo TelephoneInfo;

        public PaymentCard PaymentCard;

        public AddressInfo AddressInfo;

        [XmlElement("CustLoyalty")]
        public CustLoyalty[] CustLoyalty;

        [XmlElement("Vendor")]
        public Vendor[] Vendor;

        public ReservationTimeSpan ReservationTimeSpan;

        [XmlElement("AssociatedQuantity")]
        public AssociatedQuantity[] AssociatedQuantity;

        public StartLocation StartLocation;

        public EndLocation EndLocation;

        public TPA_Extensions TPA_Extensions;
    }

    [XmlRoot(IsNullable = false)]
    public class PersonName
    {

        [XmlElement("NamePrefix")]
        public string[] NamePrefix;

        [XmlElement("GivenName")]
        public string[] GivenName;

        [XmlElement("MiddleName")]
        public string[] MiddleName;

        public string SurnamePrefix;

        public string Surname;

        [XmlElement("NameSuffix")]
        public string[] NameSuffix;

        [XmlElement("NameTitle")]
        public string[] NameTitle;

        [XmlAttribute()]
        public PersonNameShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public PersonNameShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string NameType;

        [XmlAttribute()]
        public bool PartialName;

        [XmlIgnore()]
        public bool PartialNameSpecified;
    }

    public enum PersonNameShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum PersonNameShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class TelephoneInfo
    {

        [XmlAttribute()]
        public TelephoneInfoShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public TelephoneInfoShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string PhoneLocationType;

        [XmlAttribute()]
        public string PhoneTechType;

        [XmlAttribute()]
        public string CountryAccessCode;

        [XmlAttribute()]
        public string AreaCityCode;

        [XmlAttribute()]
        public string PhoneNumber;

        [XmlAttribute()]
        public string Extension;

        [XmlAttribute()]
        public string PIN;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        [XmlAttribute()]
        public string PhoneUseType;
    }

    public enum TelephoneInfoShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum TelephoneInfoShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentCard
    {

        public string CardHolderName;

        public CardIssuerName CardIssuerName;

        public Address Address;

        [XmlAttribute()]
        public PaymentCardShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public PaymentCardShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CardType;

        [XmlAttribute()]
        public PaymentCardCardCode CardCode;

        [XmlIgnore()]
        public bool CardCodeSpecified;

        [XmlAttribute()]
        public string CardNumber;

        [XmlAttribute()]
        public string SeriesCode;

        [XmlAttribute()]
        public string EffectiveDate;

        [XmlAttribute()]
        public string ExpireDate;
    }

    public enum PaymentCardShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum PaymentCardShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum PaymentCardCardCode
    {

        AX,

        BC,

        BL,

        CB,

        DN,

        DS,

        EC,

        JC,

        MC,

        TP,

        VI
    }

    [XmlRoot(IsNullable = false)]
    public class Vendor
    {

        [XmlAttribute()]
        public string CompanyShortName;

        [XmlAttribute()]
        public string TravelSector;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class ReservationTimeSpan
    {

        [XmlAttribute(DataType = "date")]
        public DateTime Start;

        [XmlIgnore()]
        public bool StartSpecified;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute(DataType = "date")]
        public DateTime End;

        [XmlIgnore()]
        public bool EndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class StartLocation
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlAttribute()]
        public DateTime AssociatedDateTime;

        [XmlIgnore()]
        public bool AssociatedDateTimeSpecified;

        [XmlText()]
        public string Value;
    }

    public enum OTA_CancelRQTarget
    {

        Test,

        Production
    }

    public enum OTA_CancelRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }

    public enum OTA_CancelRQCancelType
    {

        Book,

        Quote,

        Hold,

        Initiate,

        Ignore,

        Modify,

        Commit,

        Cancel
    }
}
