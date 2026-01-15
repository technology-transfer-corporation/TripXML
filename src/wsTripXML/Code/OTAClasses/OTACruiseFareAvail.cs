
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCruiseFareAvail
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class InclusivePackageOption
    {

        // <remarks/>
        [XmlAttribute()]
        public string CruisePackageCode;

        // <remarks/>
        [XmlAttribute()]
        public string InclusiveIndicator;

        // <remarks/>
        [XmlAttribute()]
        public string StartDate;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Currency
    {

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute()]
        public string DecimalPlaces;
    }

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