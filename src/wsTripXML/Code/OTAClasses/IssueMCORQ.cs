using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmIssueMCOIn
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TT_IssueMCORQ
    {

        // <remarks/>
        public wmIssueMCOModels.POS POS;

        // <remarks/>
        public wmIssueMCOModels.UniqueID UniqueID;

        // <remarks/>
        public string ConversationID;

        // <remarks/>
        public wmIssueMCOModels.MCOMask[] MCOs;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(wmIssueMCOModels.Target.Production)]
        public wmIssueMCOModels.Target Target = wmIssueMCOModels.Target.Production;

        // <remarks/>
        [XmlAttribute()]
        public wmIssueMCOModels.TransactionStatusCode TransactionStatusCode;

        // <remarks/>
        [XmlAttribute()]
        public string EchoToken;

        // <remarks/>
        [XmlAttribute()]
        public DateTime TimeStamp;

        // <remarks/>
        [XmlIgnore()]
        public bool TimeStampSpecified;

        // <remarks/>
        [XmlAttribute()]
        public double Version;

        // <remarks/>
        [XmlIgnore()]
        public bool VersionSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string PrimaryLangID;

        // <remarks/>
        [XmlAttribute()]
        public string AltLangID;

        // <remarks/>
        [XmlAttribute()]
        public int MaxResponses;

        // <remarks/>
        [XmlIgnore()]
        public bool MaxResponsesSpecified;


    }
}