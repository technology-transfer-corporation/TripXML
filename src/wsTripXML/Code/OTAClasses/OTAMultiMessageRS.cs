
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmMultiMessageOut
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class MultiMessageRS
    {

        // <remarks/>
        public Success Success;

        // <remarks/>
        [XmlArrayItem(IsNullable = false)]
        public Error[] Errors;

        // <remarks/>
        public string Response;

        // <remarks/>
        [XmlAttribute()]
        public string Version;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Success
    {
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Error
    {

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Errors
    {

        // <remarks/>
        [XmlElement("Error")]
        public Error[] Error;
    }
}