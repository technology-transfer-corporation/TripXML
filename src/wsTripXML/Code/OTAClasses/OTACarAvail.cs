using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCarAvail
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PersonName
    {

        // <remarks/>
        [XmlElement("NamePrefix")]
        public string[] NamePrefix;

        // <remarks/>
        [XmlElement("GivenName")]
        public string[] GivenName;

        // <remarks/>
        [XmlElement("MiddleName")]
        public string[] MiddleName;

        // <remarks/>
        public string SurnamePrefix;

        // <remarks/>
        public string Surname;

        // <remarks/>
        [XmlElement("NameSuffix")]
        public string[] NameSuffix;

        // <remarks/>
        [XmlElement("NameTitle")]
        public string[] NameTitle;

        // <remarks/>
        [XmlAttribute()]
        public PersonNameShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public PersonNameShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string NameType;
    }

    // <remarks/>
    public enum PersonNameShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum PersonNameShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CustLoyalty
    {

        // <remarks/>
        [XmlAttribute()]
        public CustLoyaltyShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CustLoyaltyShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string ProgramID;

        // <remarks/>
        [XmlAttribute()]
        public string MembershipID;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string LoyalLevel;

        // <remarks/>
        [XmlAttribute()]
        public CustLoyaltySingleVendorInd SingleVendorInd;

        // <remarks/>
        [XmlIgnore()]
        public bool SingleVendorIndSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime SignupDate;

        // <remarks/>
        [XmlIgnore()]
        public bool SignupDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        // <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    public enum CustLoyaltyShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum CustLoyaltyShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum CustLoyaltySingleVendorInd
    {

        // <remarks/>
        SingleVndr,

        // <remarks/>
        Alliance
    }


    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Document
    {

        // <remarks/>
        public string DocHolderName;

        // <remarks/>
        [XmlElement("DocLimitations")]
        public string[] DocLimitations;

        // <remarks/>
        [XmlAttribute()]
        public DocumentShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public DocumentShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string DocIssueAuthority;

        // <remarks/>
        [XmlAttribute()]
        public string DocIssueLocation;

        // <remarks/>
        [XmlAttribute()]
        public string DocID;

        // <remarks/>
        [XmlAttribute()]
        public string DocType;

        // <remarks/>
        [XmlAttribute()]
        public DocumentGender Gender;

        // <remarks/>
        [XmlIgnore()]
        public bool GenderSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate;

        // <remarks/>
        [XmlIgnore()]
        public bool BirthDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime EffectiveDate;

        // <remarks/>
        [XmlIgnore()]
        public bool EffectiveDateSpecified;

        // <remarks/>
        [XmlAttribute(DataType = "date")]
        public DateTime ExpireDate;

        // <remarks/>
        [XmlIgnore()]
        public bool ExpireDateSpecified;
    }

    // <remarks/>
    public enum DocumentShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum DocumentShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum DocumentGender
    {

        // <remarks/>
        Male,

        // <remarks/>
        Female,

        // <remarks/>
        Unknown
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CitizenCountryName
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        // <remarks/>
        [XmlAttribute()]
        public string Code;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CountryName
    {

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class StateProv
    {

        // <remarks/>
        [XmlAttribute()]
        public string StateCode;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class StreetNmbr
    {

        // <remarks/>
        [XmlAttribute()]
        public string PO_Box;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Address
    {

        // <remarks/>
        public StreetNmbr StreetNmbr;

        // <remarks/>
        public string BldgRoom;

        // <remarks/>
        [XmlElement("AddressLine")]
        public string[] AddressLine;

        // <remarks/>
        public string CityName;

        // <remarks/>
        public string PostalCode;

        // <remarks/>
        public string County;

        // <remarks/>
        public StateProv StateProv;

        // <remarks/>
        public CountryName CountryName;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        // <remarks/>
        [XmlAttribute()]
        public AddressShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public AddressShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Type;
    }

    // <remarks/>
    public enum AddressShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AddressShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Email
    {

        // <remarks/>
        [XmlAttribute()]
        public EmailShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public EmailShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        // <remarks/>
        [XmlAttribute()]
        public string EmailType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum EmailShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum EmailShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Telephone
    {

        // <remarks/>
        [XmlAttribute()]
        public TelephoneShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public TelephoneShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneLocationType;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneTechType;

        // <remarks/>
        [XmlAttribute()]
        public string CountryAccessCode;

        // <remarks/>
        [XmlAttribute()]
        public string AreaCityCode;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneNumber;

        // <remarks/>
        [XmlAttribute()]
        public string Extension;

        // <remarks/>
        [XmlAttribute()]
        public string PIN;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool FormattedInd = false;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneUseType;
    }

    // <remarks/>
    public enum TelephoneShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum TelephoneShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CompanyName
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class VehRentalCore
    {

        // <remarks/>
        public PickUpLocation PickUpLocation;

        // <remarks/>
        public ReturnLocation ReturnLocation;

        // <remarks/>
        [XmlAttribute()]
        public DateTime PickUpDateTime;

        // <remarks/>
        [XmlIgnore()]
        public bool PickUpDateTimeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public DateTime ReturnDateTime;

        // <remarks/>
        [XmlIgnore()]
        public bool ReturnDateTimeSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PickUpLocation
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class ReturnLocation
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class OffLocService
    {

        // <remarks/>
        public Address Address;

        // <remarks/>
        public PersonName PersonName;

        // <remarks/>
        public Telephone Telephone;

        // <remarks/>
        [XmlAttribute()]
        public OffLocServiceType Type;

        // <remarks/>
        [XmlAttribute()]
        public string SpecInstructions;
    }

    // <remarks/>
    public enum OffLocServiceType
    {

        // <remarks/>
        CustPickUp,

        // <remarks/>
        VehDelivery,

        // <remarks/>
        CustDropOff,

        // <remarks/>
        VehCollection
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class VehClass
    {

        // <remarks/>
        [XmlAttribute()]
        public string Size;
    }

    // <remarks/>
    public enum RateQualifierRatePeriod
    {

        // <remarks/>
        Hourly,

        // <remarks/>
        Daily,

        // <remarks/>
        Weekly,

        // <remarks/>
        Monthly,

        // <remarks/>
        WeekendDay,

        // <remarks/>
        Other
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TourInfo
    {

        // <remarks/>
        public TourOperator TourOperator;

        // <remarks/>
        [XmlAttribute()]
        public string TourNumber;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TourOperator
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

}