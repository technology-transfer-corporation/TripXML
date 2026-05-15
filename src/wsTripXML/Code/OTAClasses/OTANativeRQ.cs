using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmNativeIn
{

    [XmlRoot(IsNullable = false)]
    public class NativeRQ
    {

        public POS POS;

        public string Native;
    }
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    public class Provider
    {

        public string Name;

        public string System;

        public string Userid;

        public string Password;
    }

    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;

        //public string ConversationID;
    }
}
