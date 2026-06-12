
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCarInfo
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Vendor
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

}