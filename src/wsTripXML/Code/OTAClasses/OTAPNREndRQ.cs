using System;
using System.Xml.Serialization;
using wsTripXML.wsTravelTalk.wmTravelItinerary;

namespace wsTripXML.wsTravelTalk.wmPNREndIn
{

    [XmlRoot(IsNullable = false)]
    public class OTA_PNREndRQ
    {

        public POS POS;

        public EndRequest EndRequest;

        public string ConversationID;

        public UniqueID UniqueID;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_PNREndRQTarget.Production)]
        public OTA_PNREndRQTarget Target = OTA_PNREndRQTarget.Production;

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
        public OTA_PNREndRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public string ReqRespVersion;

        [XmlAttribute()]
        public string ReservationType;

        [XmlAttribute()]
        public bool ReturnListIndicator;

        [XmlIgnore()]
        public bool ReturnListIndicatorSpecified;

        [XmlAttribute()]
        public int MaxResponses;

        [XmlIgnore()]
        public bool MaxResponsesSpecified;
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
    public class EndRequest
    {

        [XmlAttribute()]
        public EndRequestType Type;
    }

    public enum EndRequestType
    {

        SaveOnly,

        RedisplayOnly,

        SaveAndRedisplay,

        IgnoreOnly,

        IgnoreAndRedisplay,

        SaveAndShowWarnings
    }

    public enum OTA_PNREndRQTarget
    {

        Test,

        Production
    }

    public enum OTA_PNREndRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }
}
