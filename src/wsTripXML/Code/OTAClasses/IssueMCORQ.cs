using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmIssueMCOIn
{

    [XmlRoot(IsNullable = false)]
    public class TT_IssueMCORQ
    {

        public wmIssueMCOModels.POS POS;

        public wmIssueMCOModels.UniqueID UniqueID;

        public string ConversationID;

        public wmIssueMCOModels.MCOMask[] MCOs;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(wmIssueMCOModels.Target.Production)]
        public wmIssueMCOModels.Target Target = wmIssueMCOModels.Target.Production;

        [XmlAttribute()]
        public wmIssueMCOModels.TransactionStatusCode TransactionStatusCode;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        public double Version;

        [XmlIgnore()]
        public bool VersionSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public int MaxResponses;

        [XmlIgnore()]
        public bool MaxResponsesSpecified;


    }
}