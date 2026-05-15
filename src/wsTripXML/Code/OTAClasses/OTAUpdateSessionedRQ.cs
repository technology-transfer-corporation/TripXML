using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmUpdateSessionedIn
{

    [XmlType()]
    [XmlRoot(IsNullable = false)]
    public class OTA_UpdateSessionedRQ
    {

        public UniqueID_Type UniqueID;

        public POS_Type POS;

        [XmlElement("Position")]
        public UpdatePositionType[] Position;

        public string ConversationID;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_UpdateRQTarget.Production)]
        public OTA_UpdateRQTarget Target = OTA_UpdateRQTarget.Production;

        [XmlAttribute()]
        public string Version;

        [XmlAttribute()]
        public string TransactionIdentifier;

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string SequenceNmbr;

        [XmlAttribute()]
        public OTA_UpdateRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public bool RetransmissionIndicator;

        [XmlIgnore()]
        public bool RetransmissionIndicatorSpecified;

        [XmlAttribute()]
        public string ReqRespVersion;
    }

    [XmlType()]
    [XmlInclude(typeof(SourceTypeRequestorID))]
    public class UniqueID_Type
    {

        public CompanyNameType CompanyName;

        [XmlAttribute(DataType = "anyURI")]
        public string URL;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public string Instance;

        [XmlAttribute()]
        public string ID_Context;

        [XmlAttribute()]
        public string ID;
    }

    [XmlType()]
    public class CompanyNameType
    {

        [XmlAttribute()]
        public string CompanyShortName;

        [XmlAttribute()]
        public string TravelSector;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute()]
        public string Division;

        [XmlAttribute()]
        public string Department;

        [XmlText()]
        public string Value;
    }

    [XmlType()]
    [XmlRoot(IsNullable = false)]
    public class Root
    {

        [XmlAnyElement()]
        public System.Xml.XmlElement Any;

        [XmlAttribute()]
        public RootOperation Operation;
    }

    [XmlType()]
    public enum RootOperation
    {

        replace
    }

    [XmlType()]
    [XmlRoot(IsNullable = false)]
    public class Subtree
    {

        [XmlAnyElement()]
        public System.Xml.XmlElement Any;

        [XmlAttribute()]
        public SubtreeOperation Operation;

        [XmlAttribute()]
        public string Child;
    }

    [XmlType()]
    public enum SubtreeOperation
    {

        insert,

        delete,

        modify
    }

    [XmlType()]
    [XmlRoot(IsNullable = false)]
    public class Element
    {

        [XmlAnyElement()]
        public System.Xml.XmlElement[] Any;

        [XmlAttribute()]
        public ElementOperation Operation;

        [XmlAttribute()]
        public string Child;
    }

    [XmlType()]
    public enum ElementOperation
    {

        insert,

        modify,

        delete
    }

    [XmlType()]
    [XmlRoot(IsNullable = false)]
    public class Attribute
    {

        [XmlAttribute()]
        public AttributeOperation Operation;

        [XmlAttribute()]
        public string Name;

        [XmlAttribute()]
        public string Value;
    }

    [XmlType()]
    public enum AttributeOperation
    {

        insert,

        modify,

        delete
    }

    [XmlType()]
    public class UpdatePositionType
    {

        [XmlElement("Attribute", typeof(Attribute))]
        [XmlElement("Subtree", typeof(Subtree))]
        [XmlElement("Element", typeof(Element))]
        [XmlElement("Root", typeof(Root))]
        public object[] Items;

        [XmlAttribute()]
        public string XPath;
    }

    [XmlType()]
    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        public string Name;

        public string System;

        public string Userid;

        public string Password;
    }

    [XmlType()]
    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;
    }

    [XmlType()]
    public class SourceTypeBookingChannel
    {

        public CompanyNameType CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public bool Primary;

        [XmlIgnore()]
        public bool PrimarySpecified;
    }

    [XmlType()]
    public class SourceTypePosition
    {

        [XmlAttribute()]
        public string Latitude;

        [XmlAttribute()]
        public string Longitude;

        [XmlAttribute()]
        public string Altitude;

        [XmlAttribute()]
        public string AltitudeUnitOfMeasureCode;
    }

    [XmlType()]
    public class SourceType
    {

        public SourceTypeRequestorID RequestorID;

        public SourceTypePosition Position;

        public SourceTypeBookingChannel BookingChannel;

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
    }

    [XmlType()]
    public class SourceTypeRequestorID : UniqueID_Type
    {

        [XmlAttribute()]
        public string MessagePassword;
    }

    [XmlType()]
    public class POS_Type : Code.IPOS
    {
        //[XmlElement("Source")]
        //public SourceType[] Source;
        public TPA_Extensions TPA_Extensions;
    }

    [XmlType()]
    public enum OTA_UpdateRQTarget
    {

        Test,

        Production,

        WSP,

        GAL
    }

    [XmlType()]
    public enum OTA_UpdateRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries,

        Continuation,

        Subsequent
    }
}