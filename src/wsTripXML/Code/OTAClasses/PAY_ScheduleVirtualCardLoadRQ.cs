using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmScheduleVirtualCardLoad
{

    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2")]
    [XmlRoot(Namespace = "http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2", IsNullable = false)]
    public partial class PAY_ScheduleVirtualCardLoadRQ
    {

        public VirtualCreditCard.POS POS { get; set; }

        public string ConversationID { get; set; }

        public Target Target { get; set; }

        public VirtualCreditCard.FundsTransferType FundsTransfer { get; set; }

        public Reason Reason { get; set; }

        [XmlAttribute()]
        public string EchoToken { get; set; }

        [XmlAttribute()]
        public DateTime TimeStamp { get; set; }

        [XmlIgnore()]
        public bool TimeStampSpecified { get; set; }

        [XmlAttribute("Target")]
        public VirtualCreditCard.Target Target1 { get; set; }

        [XmlIgnore()]
        public bool Target1Specified { get; set; }

        [XmlAttribute()]
        public string TargetName { get; set; }

        [XmlAttribute()]
        public decimal Version { get; set; }

        [XmlAttribute()]
        public string TransactionIdentifier { get; set; }

        [XmlAttribute()]
        public long SequenceNmbr { get; set; }

        [XmlIgnore()]
        public bool SequenceNmbrSpecified { get; set; }

        [XmlAttribute()]
        public VirtualCreditCard.TransactionStatusCode TransactionStatusCode { get; set; }

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified { get; set; }

        [XmlAttribute(DataType = "language")]
        public string PrimaryLangID { get; set; }

        [XmlAttribute(DataType = "language")]
        public string AltLangID { get; set; }

        [XmlAttribute()]
        public bool RetransmissionIndicator { get; set; }

        [XmlIgnore()]
        public bool RetransmissionIndicatorSpecified { get; set; }

        [XmlAttribute()]
        public string CorrelationID { get; set; }

    }

    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2")]
    public partial class Target
    {

        private TargetReference[] referencesField;

        [XmlArrayItem("Reference", IsNullable = false)]
        public TargetReference[] References
        {
            get
            {
                return referencesField;
            }
            set
            {
                referencesField = value;
            }
        }
    }


    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2")]
    public partial class TargetReference
    {

        [XmlAttribute()]
        public string Type { get; set; }

        [XmlText()]
        public string Value { get; set; }

    }


    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2")]
    public partial class Reason
    {

        [XmlAttribute()]
        public string Label { get; set; }

        [XmlAttribute(DataType = "language")]
        public string Language { get; set; }

        [XmlText()]
        public string Value { get; set; }

    }


}