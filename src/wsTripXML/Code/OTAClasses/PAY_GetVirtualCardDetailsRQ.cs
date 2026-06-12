using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmGetVirtualCardDetails
{

    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class PAY_GetVirtualCardDetailsRQ
    {

        private Reference[] referencesField;

        public VirtualCreditCard.POS POS { get; set; }

        public string ConversationID { get; set; }

        [XmlArrayItem("Reference", IsNullable = false)]
        public Reference[] References
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

        public string DisplayFilter { get; set; }

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


    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class Reference
    {

        [XmlAttribute()]
        public string Type { get; set; }

        [XmlText()]
        public string Value { get; set; }

    }

}