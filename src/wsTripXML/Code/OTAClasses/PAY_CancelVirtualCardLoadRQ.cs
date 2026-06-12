using System;
using System.Diagnostics;
using System.Xml.Serialization;
using TripXML.Core.Models.Base;

namespace wsTripXML.wsTravelTalk.wmCancelVirtualCardLoad
{
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2")]
    [XmlRoot(Namespace = "http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2", IsNullable = false)]
    public partial class PAY_CancelVirtualCardLoadRQ : VirtualCardRQBase
    {

        public VirtualCreditCard.POS POS { get; set; }

        public string ConversationID { get; set; }

        public FundsTransfer FundsTransfer { get; set; }

        [XmlAttribute()]
        public string EchoToken { get; set; }

        [XmlAttribute()]
        public DateTime TimeStamp { get; set; }

        [XmlIgnore()]
        public bool TimeStampSpecified { get; set; }

        [XmlAttribute()]
        public VirtualCreditCard.Target Target { get; set; }

        [XmlIgnore()]
        public bool TargetSpecified { get; set; }

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

    [System.CodeDom.Compiler.GeneratedCode("xsd", "4.6.1055.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2")]
    public partial class FundsTransfer
    {
        [XmlElement("Reason")]
        public FundsTransferReason[] Reason { get; set; }

        [XmlAttribute()]
        public string Reference { get; set; }

    }


    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2")]
    public partial class FundsTransferReason
    {

        [XmlAttribute()]
        public string Label { get; set; }

        [XmlAttribute(DataType = "language")]
        public string Language { get; set; }

        [XmlText()]
        public string Value { get; set; }

    }


}