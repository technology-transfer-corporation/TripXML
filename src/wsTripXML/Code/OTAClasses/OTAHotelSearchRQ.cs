using System;
using System.Xml.Serialization;
using wsTripXML.wsTravelTalk.wmHotelSearch;

namespace wsTripXML.wsTravelTalk.wmHotelSearchIn
{

    [XmlRoot(IsNullable = false)]
    public class OTA_HotelSearchRQ
    {

        public POS POS;

        [XmlArrayItem(IsNullable = false)]
        public Criterion[] Criteria;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_HotelSearchRQTarget.Production)]
        public OTA_HotelSearchRQTarget Target = OTA_HotelSearchRQTarget.Production;

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
        public OTA_HotelSearchRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public int MaxResponses;

        [XmlIgnore()]
        public bool MaxResponsesSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_HotelSearchRQResponseType.PropertyList)]
        public OTA_HotelSearchRQResponseType ResponseType = OTA_HotelSearchRQResponseType.PropertyList;
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

        public Name Name;

        public string System;

        public string Userid;

        public string Password;
    }

    [XmlRoot(IsNullable = false)]
    public class Name
    {

        [XmlAttribute()]
        public string PseudoCityCode;

        [XmlText()]
        public string Value;
    }

    public enum OTA_HotelSearchRQTarget
    {

        Test,

        Production
    }

    public enum OTA_HotelSearchRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }

    public enum OTA_HotelSearchRQResponseType
    {

        PropertyList,

        AreaList
    }
}
