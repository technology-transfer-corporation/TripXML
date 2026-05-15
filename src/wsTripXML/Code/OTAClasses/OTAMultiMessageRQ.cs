using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmMultiMessageIn
{

    [XmlRoot(IsNullable = false)]
    public class MultiMessageRQ
    {

        public POSType POS;

        public string MultiMessage;
    }

    public class POSType : Code.IPOS
    {

        public TPA_Extensions TPA_Extensions;
    }

    public class SourceType
    {

        public RequestorIDType RequestorID;

        [XmlAttribute()]
        public string PseudoCityCode;
    }

    public class RequestorIDType
    {

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public string ID;
    }

    public class ProviderType
    {

        public string Name;

        public string System;

        public string Userid;

        public string Password;
    }

    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public ProviderType Provider;
    }
}