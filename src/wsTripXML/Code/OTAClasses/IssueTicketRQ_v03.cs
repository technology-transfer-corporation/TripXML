using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmIssueTicketIn_v03
{

    [XmlRoot(IsNullable = false)]
    public class TT_IssueTicketRQ
    {

        public POS POS;

        public UniqueID UniqueID;

        public Fulfillment Fulfillment;

        public Ticketing Ticketing;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(TT_IssueTicketRQTarget.Production)]
        public TT_IssueTicketRQTarget Target = TT_IssueTicketRQTarget.Production;

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
        public TT_IssueTicketRQTransactionStatusCode TransactionStatusCode;

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
        [System.ComponentModel.DefaultValue(false)]
        public bool DirectFlightsOnly = false;

        [XmlAttribute()]
        public int NumberStops;

        [XmlIgnore()]
        public bool NumberStopsSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }
    [XmlRoot(IsNullable = false)]
    public class Ticketing
    {

        public TicketDesignator TicketDesignators;

        public string OtherPrinter;

        public string StockRange;

        [XmlElement("FareNumber")]
        public int[] FareNumber;

        public Notification Notification;

        public string OrderNumber;

        public string BookingPCC;

        public Commission Commission;

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
        public bool IssueItinerary = false;
    }

    [XmlRoot(IsNullable = true)]
    public enum TicketingTicketType
    {

        eTicket,

        Paper,

        None
    }

    [XmlRoot(IsNullable = false)]
    public class Commission
    {

        [XmlAttribute()]
        public decimal Amount;

        [XmlAttribute()]
        public decimal Percent;
    }

    [XmlRoot(IsNullable = false)]
    public class Notification
    {

        [XmlAttribute()]
        public bool ByEmail = false;

        [XmlAttribute()]
        public bool ByFax = false;

    }

    public partial class TicketDesignator
    {

[XmlAttribute()]
        public string RPH;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class UniqueID
    {

        [XmlAttribute()]
        public string ID;
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

}
