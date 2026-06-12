using System;
using System.Xml.Serialization;
using wsTripXML.wsTravelTalk.wmProfileCommon;

namespace wsTripXML.wsTravelTalk.wmProfileCreateIn
{

    [XmlType()]
    [XmlRoot(IsNullable = false)]
    public class OTA_ProfileCreateRQ
    {

        public OTA_ProfileCreateRQPOS POS;

        [XmlElement("UniqueID")]
        public UniqueID_Type[] UniqueID;

        public ProfileType Profile;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        public OTA_ProfileCreateRQTarget Target;

        [XmlIgnore()]
        public bool TargetSpecified;

        [XmlAttribute()]
        public decimal Version;

        [XmlAttribute()]
        public string TransactionIdentifier;

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string SequenceNmbr;

        [XmlAttribute()]
        public OTA_ProfileCreateRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public bool RetransmissionIndicator;

        [XmlIgnore()]
        public bool RetransmissionIndicatorSpecified;

        [XmlAttribute()]
        public string CorrelationID;
    }

    [XmlType()]
    public class OTA_ProfileCreateRQPOS : Code.IPOS
    {
        // re-pointed from the parent-namespace wsTravelTalk.TPA_Extensions (ndcAuthenticate header type) to
        // wmProfileCommon.TPA_Extensions: both map to XML type 'TPA_Extensions' in one serializer scope
        public wmProfileCommon.TPA_Extensions TPA_Extensions;
    }

    [XmlType(IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        ListItem,

        Text,

        Image,

        URL
    }

    //[XmlType()]
    //[XmlInclude(typeof(OTA_ProfileCreateRQPOS))]
    //public class POS_Type : Code.IPOS
    //{

    //    [XmlElement("Source")]
    //    public SourceType[] Source;
    //}

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
    public enum OTA_ProfileCreateRQTarget
    {

        Test,

        Production
    }

    [XmlType()]
    public enum OTA_ProfileCreateRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries,

        Continuation,

        Subsequent
    }
}