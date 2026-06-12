using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCrypticIn
{

    [XmlRoot(IsNullable = false)]
    public class CrypticRQ
    {

        public POS POS;

        public UniqueID UniqueID;

        public string Entry;
    }

    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    public class UniqueID
    {

        [XmlAttribute()]
        public string ID;
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
