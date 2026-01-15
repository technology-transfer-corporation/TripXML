
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmIssueMCOOut
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TT_IssueMCORS
    {
        // <remarks/>
        public wmIssueMCOModels.Success Success;

        // <remarks/>
        [XmlArrayItem("Warning", IsNullable = false)]
        public string[] Warnings;

        // <remarks/>
        [XmlArrayItem("Error", IsNullable = false)]
        public string[] Errors;

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


    }




}