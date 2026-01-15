
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmMultiMessageIn
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class MultiMessageRQ
    {

        // <remarks/>
        public POSType POS;

        // <remarks/>
        public string MultiMessage;
    }

    // <remarks/>
    public class POSType
    {

        // <remarks/>
        [XmlElement("Source")]
        public SourceType[] Source;

        // <remarks/>
        public TPA_ExtensionsType TPA_Extensions;
    }

    // <remarks/>
    public class SourceType
    {

        // <remarks/>
        public RequestorIDType RequestorID;

        // <remarks/>
        [XmlAttribute()]
        public string PseudoCityCode;
    }

    // <remarks/>
    public class RequestorIDType
    {

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        public string ID;
    }

    // <remarks/>
    public class ProviderType
    {

        // <remarks/>
        public string Name;

        // <remarks/>
        public string System;

        // <remarks/>
        public string Userid;

        // <remarks/>
        public string Password;
    }

    // <remarks/>
    public class TPA_ExtensionsType
    {

        // <remarks/>
        public ProviderType Provider;
    }
}