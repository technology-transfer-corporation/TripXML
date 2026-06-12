using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCrypticEntriesIn
{

    [XmlRoot(IsNullable = false)]
    public class CrypticEntriesRQ
    {

        public POS POS;

        [XmlElement("Entry")]
        public string[] Entry;
    }

    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        public string Name;

        public string System;

        public string Userid;

        public string Password;
    }

    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;

        //public string ConversationID;
    }
}
