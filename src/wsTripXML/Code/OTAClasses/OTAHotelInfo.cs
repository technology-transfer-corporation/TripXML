
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmHotelInfo
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

}