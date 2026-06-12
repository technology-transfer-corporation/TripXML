
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmHotelSearch
{

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
        public CompanyName CompanyName;

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

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class BookingChannel
    {

        // <remarks/>
        public CompanyName CompanyName;

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
    public class Criteria
    {

        // <remarks/>
        [XmlElement("Criterion")]
        public Criterion[] Criterion;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Criterion
    {

        // <remarks/>
        public Position Position;

        // <remarks/>
        public Address Address;

        // <remarks/>
        public Telephone Telephone;

        // <remarks/>
        public string RefPoint;

        // <remarks/>
        public CodeRef CodeRef;

        // <remarks/>
        public HotelRef HotelRef;

        // <remarks/>
        public Radius Radius;

        // <remarks/>
        [XmlElement("HotelAmenity")]
        public HotelAmenity[] HotelAmenity;

        // <remarks/>
        [XmlElement("Award")]
        public Award[] Award;

        // <remarks/>
        [XmlAttribute()]
        public bool ExactMatch;

        // <remarks/>
        [XmlIgnore()]
        public bool ExactMatchSpecified;

        // <remarks/>
        [XmlAttribute()]
        public CriterionImportanceType ImportanceType;

        // <remarks/>
        [XmlIgnore()]
        public bool ImportanceTypeSpecified;
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
    public class HotelRef
    {

        // <remarks/>
        [XmlAttribute()]
        public string ChainCode;

        // <remarks/>
        [XmlAttribute()]
        public string BrandCode;

        // <remarks/>
        [XmlAttribute()]
        public string HotelCode;

        // <remarks/>
        [XmlAttribute()]
        public string HotelCityCode;

        // <remarks/>
        [XmlAttribute()]
        public string HotelName;

        // <remarks/>
        [XmlAttribute()]
        public string HotelCodeContext;

        // <remarks/>
        [XmlAttribute()]
        public string ChainName;

        // <remarks/>
        [XmlAttribute()]
        public string BrandName;

        // <remarks/>
        [XmlAttribute()]
        public string AreaID;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Radius
    {

        // <remarks/>
        [XmlAttribute()]
        public string Distance;

        // <remarks/>
        [XmlAttribute()]
        public string DistanceMeasure;

        // <remarks/>
        [XmlAttribute()]
        public string Direction;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class HotelAmenity
    {

        // <remarks/>
        [XmlAttribute()]
        public string Code;
    }

    // <remarks/>
    public enum CriterionImportanceType
    {

        // <remarks/>
        Mandatory,

        // <remarks/>
        High,

        // <remarks/>
        Medium,

        // <remarks/>
        Low
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Award
    {

        // <remarks/>
        [XmlAttribute()]
        public string Provider;

        // <remarks/>
        [XmlAttribute()]
        public string Rating;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CodeRef
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

}