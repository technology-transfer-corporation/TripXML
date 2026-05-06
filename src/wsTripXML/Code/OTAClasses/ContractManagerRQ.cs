

namespace wsTripXML.wsTravelTalk.wmContractManager
{
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class ContractManagerRQ
    {
        [System.Xml.Serialization.XmlAttribute()]
        [System.ComponentModel.DefaultValue("")]
        public string RecordLocator = string.Empty;

        [System.Xml.Serialization.XmlAttribute()]
        [System.ComponentModel.DefaultValue(true)]
        public bool IsLoaded = true;

        [System.Xml.Serialization.XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool IsTracing = false;

        // <remarks/>
        public POS POS;

        public string FQH;
        public string MISS;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {

        // <remarks/>
        [System.Xml.Serialization.XmlElement("Source")]
        public Source[] Source;

        // <remarks/>
        public TPA_Extensions TPA_Extensions;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class TPA_Extensions
    {

        // <remarks/>
        public Provider Provider;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class Source
    {

        // <remarks/>
        public RequestorID RequestorID;

        // <remarks/>
        public Position Position;

        // <remarks/>
        public BookingChannel BookingChannel;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string AgentSine;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string PseudoCityCode;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string ISOCountry;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string ISOCurrency;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string AgentDutyCode;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string AirlineVendorID;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string AirportCode;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string FirstDepartPoint;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string ERSP_UserID;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class RequestorID
    {

        // <remarks/>
        public CompanyName CompanyName;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute(DataType = "anyURI")]
        public string URL;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Type;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Instance;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string ID;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string ID_Context;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class Position
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Latitude;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Longitude;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Altitude;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class BookingChannel
    {

        // <remarks/>
        public CompanyName CompanyName;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Type;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public bool Primary;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool PrimarySpecified;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class CompanyName
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Code;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [System.Xml.Serialization.XmlText()]
        public string Value;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class Provider
    {

        // <remarks/>
        public string Name;

        // <remarks/>
        public string System;

        // <remarks/>
        public string Userid;

        // <remarks/>
        public string Password;
    }


}