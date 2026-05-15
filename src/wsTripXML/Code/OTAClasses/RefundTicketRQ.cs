using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmRefundTicketIn
{
    [XmlRoot(IsNullable = false)]
    public class TT_RefundTicketRQ
    {
        public POS POS;
        public string IATACode;
        public TT_RefundTicketRQTicket Ticket;
    }
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    [XmlRoot(IsNullable = false)]
    public class TT_RefundTicketRQTicket
    {

        [XmlElement("TicketNumber")]
        public TicketNumber[] TicketNumber;

        public TT_RefundTicketRQTicketRefund Refund;

        public TT_RefundTicketRQTicketPenalty Penalty;
    }

    [XmlRoot(IsNullable = false)]
    public class TicketNumber
    {

        [XmlAttribute(DataType = "date")]
        public DateTime DateOfIssue;

        [XmlAttribute(DataType = "date")]
        public DateTime DateOfRefund;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class TT_RefundTicketRQTicketRefund
    {

        [XmlAttribute()]
        public double Amount;

        [XmlAttribute()]
        public string Currency;

        [XmlAttribute()]
        public int DecimalPlaces;
    }

    [XmlRoot(IsNullable = false)]
    public class TT_RefundTicketRQTicketPenalty
    {

        [XmlAttribute()]
        public double Amount;

        [XmlAttribute()]
        public string Currency;

        [XmlAttribute()]
        public int DecimalPlaces;
    }

    public enum TT_RefundTicketRQTarget
    {

        Test,

        Production
    }

}