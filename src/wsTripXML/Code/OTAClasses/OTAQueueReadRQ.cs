using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmQueueReadIn
{

    [XmlRoot(IsNullable = false)]
    public class AccessQueue
    {

        [XmlAttribute()]
        public string Number;

        [XmlAttribute()]
        public string Category;

        [XmlAttribute()]
        public string Name;

        [XmlAttribute()]
        public string PseudoCityCode;
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
    public class ItemOnQueue
    {

        [XmlAttribute()]
        public ItemOnQueueAction Action;
    }

    public enum ItemOnQueueAction
    {

        NextRemove,

        NextKeep,

        Redisplay
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_QueueReadRQ
    {

        public POS POS;

        public AccessQueue AccessQueue;

        public ItemOnQueue ItemOnQueue;

        public string ExitQueue;

        public VerifyTickets VerifyTickets;

        public string ConversationID;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public string TimeStamp;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string Target;

        [XmlAttribute()]
        public string Version;

        [XmlAttribute()]
        public string TransactionIdentifier;

        [XmlAttribute()]
        public string SequenceNmbr;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string TransactionStatusCode;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public bool CheckIssuedTicket;

        [XmlIgnore()]
        public bool CheckIssuedTicketSpecified;

    }
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
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
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        [XmlElement("Provider")]
        public Provider[] Provider;
    }

    [XmlRoot(IsNullable = false)]
    public class VerifyTickets
    {

        [XmlElement("TicketDate")]
        public DateTime[] TicketDate;

        [XmlElement("PseudoCityCode")]
        public string[] PseudoCityCode;
    }

}
