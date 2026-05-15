using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmVoidTicketIn
{

    [XmlRoot(IsNullable = false)]
    public class TT_VoidTicketRQ
    {
        public POS POS;

        [XmlArrayItem("TicketNumber", IsNullable = false)]
        public string[] Tickets;

        public UniqueIDRQ UniqueID;
    }

    [XmlRoot(IsNullable = false)]
    public class UniqueIDRQ
    {

        [XmlAttribute()]
        public string ID;

    }
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {

        public TPA_Extensions TPA_Extensions;
    }


    [XmlRoot(IsNullable = false)]
    public class Ticketing
    {

        public string OtherPrinter;

        public string StockRange;

        [XmlElement("FareNumber")]
        public int[] FareNumber;

        public Notification Notification;

        public string OrderNumber;

        public string BookingPCC;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool BoardingPass = false;

        [XmlAttribute()]
        public string TravelerRefNumberRPHList;

        [XmlAttribute()]
        public string FlightRefNumberRPHList;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool InfantOnly = false;

        [XmlAttribute()]
        public string TicketType;

        [XmlAttribute()]
        public bool IssueMCO = false;

        [XmlAttribute()]
        public bool IssueInvoice = false;

        [XmlAttribute()]
        public bool OmitInfant = false;
    }

    [XmlRoot(IsNullable = true)]
    public enum TicketingTicketType
    {

        eTicket,

        Paper,

        None
    }

    [XmlRoot(IsNullable = false)]
    public class Notification
    {

        [XmlAttribute()]
        public bool ByEmail = false;

        [XmlAttribute()]
        public bool ByFax = false;

    }

    [XmlRoot(IsNullable = false)]
    public class UniqueID
    {

        [XmlAttribute()]
        public string ID;
    }
    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions
    {
        public Provider Provider;
    }

    public enum TT_IssueTicketRQTarget
    {

        Test,

        Production
    }

    public enum TT_IssueTicketRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }

    [XmlRoot(IsNullable = false)]
    public class Fulfillment
    {

        [XmlArrayItem(IsNullable = false)]
        public PaymentDetail[] PaymentDetails;
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentDetail
    {

        public PaymentCard PaymentCard;

        public DirectBill DirectBill;

        public MiscChargeOrder MiscChargeOrder;

        [XmlAttribute()]
        public PaymentDetailShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public PaymentDetailShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CostCenterID;

        [XmlAttribute()]
        public string RPH;
    }

    public enum PaymentDetailShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum PaymentDetailShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class DirectBill
    {

        [XmlAttribute()]
        public DirectBillShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public DirectBillShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string DirectBill_ID;
    }

    public enum DirectBillShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum DirectBillShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    [XmlRoot(IsNullable = false)]
    public class CardHolderName
    {

        [XmlAttribute()]
        public string BankID;
    }

    [XmlRoot(IsNullable = false)]
    public class PaymentCard
    {

        public CardHolderName CardHolderName;

        [XmlAttribute()]
        public PaymentCardShareSynchInd ShareSynchInd;

        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        [XmlAttribute()]
        public PaymentCardShareMarketInd ShareMarketInd;

        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        [XmlAttribute()]
        public string CardType;

        [XmlAttribute()]
        public PaymentCardCardCode CardCode;

        [XmlIgnore()]
        public bool CardCodeSpecified;

        [XmlAttribute()]
        public string CardNumber;

        [XmlAttribute()]
        public string SeriesCode;

        [XmlAttribute()]
        public string EffectiveDate;

        [XmlAttribute()]
        public string ExpireDate;

        [XmlAttribute()]
        public string ConfirmationNumber;
    }

    public enum PaymentCardShareSynchInd
    {

        Yes,

        No,

        Inherit
    }

    public enum PaymentCardShareMarketInd
    {

        Yes,

        No,

        Inherit
    }

    public enum PaymentCardCardCode
    {

        AX,

        BC,

        BL,

        CB,

        DN,

        DS,

        EC,

        JC,

        MC,

        TP,

        VI
    }

    [XmlRoot(IsNullable = false)]
    public class MiscChargeOrder
    {

        [XmlAttribute()]
        public string TicketNumber;
    }

}