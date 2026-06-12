
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCruiseCabinUnhold
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Budget
    {

        // <remarks/>
        [XmlAttribute()]
        public string PricingType;

        // <remarks/>
        [XmlAttribute()]
        public string MinPrice;

        // <remarks/>
        [XmlAttribute()]
        public string MaxPrice;

        // <remarks/>
        [XmlAttribute()]
        public string GuidelinePrice;

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute()]
        public string DecimalPlaces;
    }

}