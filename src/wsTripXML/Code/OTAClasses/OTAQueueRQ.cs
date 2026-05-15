using System.Xml.Serialization;
using wsTripXML.wsTravelTalk.wmQueue;

namespace wsTripXML.wsTravelTalk.wmQueueIn
{

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
    public class BounceQueue
    {

        [XmlAttribute(DataType = "NMTOKEN")]
        public string Action;

        [XmlAttribute()]
        public string ItemsForward;

        [XmlAttribute()]
        public string ItemsBackward;
    }

    [XmlRoot(IsNullable = false)]
    public class CleanQueue
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
    public class CountQueue
    {

        [XmlAttribute()]
        public string Number;

        [XmlAttribute()]
        public string Category;

        [XmlAttribute()]
        public string Name;

        [XmlAttribute()]
        public string PseudoCityCode;

        [XmlAttribute()]
        public string Summary;
    }

    [XmlRoot(IsNullable = false)]
    public class FromQueue
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
    public class ListQueue
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
    public class Move
    {

        public FromQueue FromQueue;

        public ToQueue ToQueue;

        [XmlAttribute()]
        public string ItemsQuantity;
    }

    [XmlRoot(IsNullable = false)]
    public class ToQueue
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
    public class OTA_QueueRQ
    {

        public POS POS;

        public BounceQueue BounceQueue;

        public CleanQueue CleanQueue;

        public CountQueue CountQueue;

        public ListQueue ListQueue;

        [XmlElement("PlaceQueue")]
        public PlaceQueue[] PlaceQueue;

        public RemoveQueue RemoveQueue;

        public Move Move;

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
    public class PlaceQueue
    {

        public UniqueID UniqueID;

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
    public class RemoveQueue
    {

        public UniqueID UniqueID;

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
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        [XmlElement("Provider")]
        public Provider[] Provider;
    }
}
