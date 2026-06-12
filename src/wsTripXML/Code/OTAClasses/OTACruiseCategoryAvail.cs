
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCruiseCategoryAvail
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
    public class SelectedSailing
    {

        // <remarks/>
        [XmlAttribute()]
        public string VoyageID;

        // <remarks/>
        [XmlAttribute()]
        public string Start;

        // <remarks/>
        [XmlAttribute()]
        public string Duration;

        // <remarks/>
        [XmlAttribute()]
        public string End;

        // <remarks/>
        [XmlAttribute()]
        public string VendorCode;

        // <remarks/>
        [XmlAttribute()]
        public string VendorName;

        // <remarks/>
        [XmlAttribute()]
        public string ShipCode;

        // <remarks/>
        [XmlAttribute()]
        public string ShipName;

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string Status;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class SailingInfo
    {

        // <remarks/>
        public SelectedSailing SelectedSailing;

        // <remarks/>
        public InclusivePackageOption InclusivePackageOption;

        // <remarks/>
        public Currency Currency;
    }

}