using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wsReIssueTicket
{

    public partial class TT_ReIssueTicketRQ
    {

        public POS POS;

        public UniqueID UniqueID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(TT_ReIssueTicketRQTarget.Production)]
        public TT_ReIssueTicketRQTarget Target = TT_ReIssueTicketRQTarget.Production;

        [XmlAttribute()]
        public double Version;

        public string ConversationID;

        public Reissue ReIssue;

    }

    [XmlRoot(IsNullable = false)]
    public class Reissue
    {

        public string OtherPrinter;

        public string StockRange;

        [XmlElement("Ticket")]
        public Ticket[] Ticket;

        public string OrderNumber;

        public string BookingPCC;

        public string TicketingPrinter;


    }


    public enum TicketingControlType
    {

        OK,

        TF
    }

    public enum TT_ReIssueTicketRQTarget
    {

        Test,

        Production
    }
    [XmlRoot(IsNullable = false)]
    public class UniqueID
    {

        [XmlAttribute()]
        public string ID;
    }


    public partial class CouponInfoFirst
    {

        public string CouponNumber;

        [XmlElement("OtherCouponDetails")]
        public string[] OtherCoupons;

    }

    public partial class TicketRange
    {
        public Ticket PaperticketDetailsfirst;
        public Ticket PaperticketDetailsLast;
    }

    public partial class TypeReprice
    {

        public StatusDetails StatusDetails;

    }

    public partial class StatusDetails
    {

        public string Indicator;

    }

    public partial class ReissuelPricingOptions
    {

        [XmlArrayItem("AttributeDetails", IsNullable = false)]
        public AttributeDetails[] OverrideInformation;

        [XmlArrayItem("CityDetail", IsNullable = false)]
        public CityDetail[] CityOverride;

        [XmlElement("DiscountInformation")]
        public DiscountInformation[] iscountInformation;

    }

    public partial class AttributeDetails
    {

        public string AttributeType;

        public string AttributeDescription;

    }

    public partial class CityDetail
    {

        public string CityCode;

        public string CityQualifier;

    }

    public partial class DiscountInformation
    {

        public PenDisInformation PenDisInformation;

        [XmlArrayItem("RefDetails", IsNullable = false)]
        public RefDetails[] ReferenceQualifier;

    }

    public partial class PenDisInformation
    {

        public string[] InfoQualifier;

        [XmlElement("penDisData")]
        public PenDisData[] PenDisData;

    }

    public partial class PenDisData
    {

        public string[] PenaltyType;

        public string[] PenaltyQualifier;

        public decimal[] PenaltyAmount;

        [XmlIgnore()]
        public bool[] PenaltyAmountSpecified;

        public string[] DiscountCode;

        public string[] PenaltyCurrency;

    }

    public partial class RefDetails
    {
        public string[] RefQualifier;

        public decimal[] RefNumber;

    }

}