using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmListVirtualCards
{

    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class PAY_ListVirtualCardsRQ
    {

        public VirtualCreditCard.POS POS { get; set; }

        public string ConversationID { get; set; }

        public string SubType { get; set; }

        public string VendorCode { get; set; }

        public AmountRange AmountRange { get; set; }

        public string CurrencyCode { get; set; }

        public Period Period { get; set; }

        public string CardStatus { get; set; }

        public VirtualCreditCard.Reservation Reservation { get; set; }

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
    public partial class AmountRange
    {

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string Max { get; set; }

        [XmlAttribute(DataType = "nonNegativeInteger")]
        public string Min { get; set; }

    }


    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class TimeInstantType
    {

        [XmlAttribute(DataType = "duration")]
        public string WindowBefore { get; set; }

        [XmlAttribute(DataType = "duration")]
        public string WindowAfter { get; set; }

        [XmlAttribute()]
        public bool CrossDateAllowedIndicator { get; set; }

        [XmlIgnore()]
        public bool CrossDateAllowedIndicatorSpecified { get; set; }

        [XmlText()]
        public string Value { get; set; }

    }


    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class DateTimeSpanType
    {

        private object[] itemsField;

        [XmlElement("DateWindowRange", typeof(TimeInstantType))]
        [XmlElement("EndDateWindow", typeof(DateTimeSpanTypeEndDateWindow))]
        [XmlElement("StartDateWindow", typeof(DateTimeSpanTypeStartDateWindow))]
        public object[] Items
        {
            get
            {
                return itemsField;
            }
            set
            {
                itemsField = value;
            }
        }

        [XmlAttribute()]
        public string Start { get; set; }

        [XmlAttribute()]
        public string Duration { get; set; }

        [XmlAttribute()]
        public string End { get; set; }
    }


    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class DateTimeSpanTypeEndDateWindow
    {

        [XmlAttribute()]
        public string EarliestDate { get; set; }

        [XmlAttribute()]
        public string LatestDate { get; set; }

        [XmlAttribute()]
        public VirtualCreditCard.DayOfWeekType DOW { get; set; }

        [XmlIgnore()]
        public bool DOWSpecified { get; set; }

    }


    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class DateTimeSpanTypeStartDateWindow
    {

        [XmlAttribute()]
        public string EarliestDate { get; set; }

        [XmlAttribute()]
        public string LatestDate { get; set; }

        [XmlAttribute()]
        public VirtualCreditCard.DayOfWeekType DOW { get; set; }

        [XmlIgnore()]
        public bool DOWSpecified { get; set; }

    }


    [Serializable()]
    [DebuggerStepThrough()]
    [System.ComponentModel.DesignerCategory("code")]
    public partial class Period : DateTimeSpanType
    {

        [XmlAttribute()]
        public string EventType { get; set; }

    }


}