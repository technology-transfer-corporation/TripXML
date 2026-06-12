
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCryptic
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class RequestorID
    {

        // <remarks/>
        public Code.CompanyName CompanyName;

        // <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string URL;

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        public string Instance;

        // <remarks/>
        [XmlAttribute()]
        public string ID;

        // <remarks/>
        [XmlAttribute()]
        public string ID_Context;
    }

    // local CompanyName removed: identical to shared wsTripXML.Code.CompanyName (XML type-name collision in one serializer scope)

}