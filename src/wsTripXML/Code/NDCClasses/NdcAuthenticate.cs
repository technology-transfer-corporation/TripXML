using System.Xml;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk
{
    public class ndcAuthenticate : System.Web.Services.Protocols.SoapHeader
    {

        public POS POS;
        public bool compressed = true;
    }

    public class POS : Code.IPOS
    {
//        public Source Source;
        public TPA_Extensions TPA_Extensions;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Source
    {

        // <remarks/>
        public RequestorID RequestorID;

        // <remarks/>
        public string Profile;

        // <remarks/>
        [XmlAttribute()]
        public string PseudoCityCode;

    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class RequestorID
    {

        // <remarks/>
        [XmlAttribute()]
        public string ID;

        // <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string URL;

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        public string Instance;

    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        // <remarks/>
        public Provider Provider;

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
    public class Ticket
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public string StatusIndicator;

        [XmlAttribute()]
        public bool Exchange = true;

        // <remarks/>
        [XmlAttribute()]
        public bool OmitInfant = false;

        // <remarks/>
        [XmlIgnore()]
        public bool OmitInfantSpecified;

        // <remarks/>
        [XmlElement("PassangerInfo")]
        public PassangerDetails[] PassangerInfo;

        // <remarks/>
        [XmlAttribute()]
        public TicketTypeType TicketType;

        // <remarks/>
        [XmlAttribute()]
        public string Number;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool BoardingPass = false;

        // <remarks/>
        [XmlAttribute()]
        public string FlightRefNumberRPHList;

        // <remarks/>
        [XmlAttribute()]
        public bool InfantOnly = false;

        // <remarks/>
        [XmlIgnore()]
        public bool InfantOnlySpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool IssueInvoice = false;

        // <remarks/>
        [XmlIgnore()]
        public bool IssueInvoiceSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool IssueItinerary = false;

        // <remarks/>
        [XmlIgnore()]
        public bool IssueItinerarySpecified;

        // <remarks/>
        [XmlIgnore()]
        public bool ExchangeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RemoteLocation;

        // <remarks/>
        [XmlAttribute()]
        public string SpecialInstruction;

        // <remarks/>
        [XmlAttribute()]
        public bool IssueJointInvoice = false;

        // <remarks/>
        [XmlIgnore()]
        public bool IssueJointInvoiceSpecified;
    }


    // <remarks/>
    public enum TicketTypeType
    {

        // <remarks/>
        Electronic,

        // <remarks/>
        Paper,

        // <remarks/>
        MCO,

        // <remarks/>
        Void
    }


    /// <remarks/>
    public partial class PassangerDetails
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

    }


}