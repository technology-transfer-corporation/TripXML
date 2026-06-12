using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmPingIn
{

    [XmlRoot(IsNullable = false)]
    public class TXML_PingRQ
    {
        public POS POS;
        public int WaitTime;
        [XmlIgnore()]
        public bool WaitTimeSpecified;
    }
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    [XmlRoot(IsNullable = false)]
    public class UniqueID
    {

        [XmlAttribute()]
        public string ID;
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

}