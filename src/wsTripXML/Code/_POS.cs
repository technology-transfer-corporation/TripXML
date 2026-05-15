using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace wsTripXML.Code
{
    public class IPOS
    {
        [XmlElement("Source")]
        public Source[] Source;

        //public ITPA_Extensions TPA_Extensions;
    }
    [XmlRoot(IsNullable = false)]
    public class Source
    {
        public RequestorID RequestorID;
        public string Profile;
        public Position Position;
        public BookingChannel BookingChannel;
        [XmlAttribute()]
        public string AgentSine;
        [XmlAttribute()]
        public string PseudoCityCode;
        [XmlAttribute()]
        public string ISOCountry;
        [XmlAttribute()]
        public string ISOCurrency;
        [XmlAttribute()]
        public string AgentDutyCode;
        [XmlAttribute()]
        public string AirlineVendorID;
        [XmlAttribute()]
        public string AirportCode;
        [XmlAttribute()]
        public string FirstDepartPoint;
        [XmlAttribute()]
        public string ERSP_UserID;
        [XmlAttribute()]
        public string TerminalID;
        [XmlAttribute()]
        public string ISOSelectedCurrency;

    }
    [XmlRoot(IsNullable = false)]
    public class Position
    {

        [XmlAttribute()]
        public string Latitude;

        [XmlAttribute()]
        public string Longitude;

        [XmlAttribute()]
        public string Altitude;
    }
    [XmlRoot(IsNullable = false)]
    public class CompanyName
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
    public class BookingChannel
    {

        public CompanyName CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public string Primary;
    }

    [XmlRoot(IsNullable = false)]
    public class RequestorID
    {
        public CompanyName CompanyName;
        [XmlAttribute()]
        public string ID;
        [XmlAttribute(DataType = "anyURI")]
        public string URL;
        [XmlAttribute()]
        public string Type;
        [XmlAttribute()]
        public string Instance;
        [XmlAttribute()]
        public string ID_Context;
        [XmlAttribute()]
        public string MessagePassword;
    }
    [XmlRoot(IsNullable = false)]
    public class ITPA_Extensions
    {
        //public Provider Provider;
        public string ConversationID;
    }
    public class Provider
    {
        public string Name;
        public string System;
        public string Userid;
        public string Password;
    }

}