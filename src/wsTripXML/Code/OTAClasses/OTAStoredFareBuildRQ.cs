using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmStoredFareBuildIn
{

    [XmlRoot(IsNullable = false)]
    public class OTA_StoredFareBuildRQ
    {

        public POS POS;

        public UniqueID UniqueID;

        [XmlElement("Traveler")]
        public OTA_StoredFareBuildRQTraveler[] Traveler;

        [XmlElement("FlightSegment")]
        public OTA_StoredFareBuildRQFlightSegment[] FlightSegment;

        [XmlElement("Options")]
        public OTA_StoredFareBuildRQOptions[] Options;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_StoredFareBuildRQTarget.Production)]
        public OTA_StoredFareBuildRQTarget Target = OTA_StoredFareBuildRQTarget.Production;

        [XmlAttribute()]
        public string Version;

        [XmlIgnore()]
        public bool VersionSpecified;

        [XmlAttribute()]
        public string TransactionIdentifier;

        [XmlAttribute()]
        public int SequenceNmbr;

        [XmlIgnore()]
        public bool SequenceNmbrSpecified;

        [XmlAttribute()]
        public OTA_StoredFareBuildRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;
    }
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    public class OTA_StoredFareBuildRQOptions
    {

        [XmlAttribute()]
        public OTA_StoredFareBuildRQOptionsType Type;
    }

    public enum OTA_StoredFareBuildRQOptionsType
    {

        Open,

        Sata
    }

    public class OTA_StoredFareBuildRQFlightSegment
    {

        [XmlAttribute()]
        public string RPH;
    }

    [XmlRoot(IsNullable = false)]
    public class UniqueID
    {

        [XmlAttribute()]
        public string ID;
    }

    public class OTA_StoredFareBuildRQTraveler
    {

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public bool Infant;
    }

    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        public object Name;

        public string System;

        public string Userid;

        public string Password;
    }

    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;
    }

    public enum OTA_StoredFareBuildRQTarget
    {

        Test,

        Production
    }

    public enum OTA_StoredFareBuildRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }
}
