using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmAirRulesIn
{

    [XmlRoot(IsNullable = false)]
    public class BookingChannel
    {

        public CompanyName CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public bool Primary;

        [XmlIgnore()]
        public bool PrimarySpecified;
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
    public class OTA_AirRulesRQ
    {

        public POS POS;

        public RuleReqInfo RuleReqInfo;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_AirRulesRQTarget.Production)]
        public OTA_AirRulesRQTarget Target = OTA_AirRulesRQTarget.Production;

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
        public OTA_AirRulesRQTransactionStatusCode TransactionStatusCode;

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
    public class TPA_Extensions : Code.ITPA_Extensions
    {
        public Provider Provider;
        //public string ConversationID;
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
    public class RuleReqInfo
    {

        public DateTime DepartureDate;

        [XmlIgnore()]
        public bool DepartureDateSpecified;

        public string FareReference;

        public wmAirRules.RuleInfo RuleInfo;

        public wmAirRules.FilingAirline FilingAirline;

        [XmlElement("MarketingAirline")]
        public wmAirRules.MarketingAirline[] MarketingAirline;

        public wmAirRules.DepartureAirport DepartureAirport;

        public wmAirRules.ArrivalAirport ArrivalAirport;

        [XmlElement("SubSection")]
        public wmAirRules.SubSection[] SubSection;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool NegotiatedFare = false;

        [XmlAttribute()]
        public string NegotiatedFareCode;
    }

    public enum OTA_AirRulesRQTarget
    {

        Test,

        Production
    }

    public enum OTA_AirRulesRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }
}
