using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCruisePackageAvail
{

    // <remarks/>
    public enum PackageOptionPackageType
    {

        // <remarks/>
        Pre,

        // <remarks/>
        Post
    }

    // <remarks/>
    public enum TransferOptionPackageType
    {

        // <remarks/>
        Pre,

        // <remarks/>
        Post,

        // <remarks/>
        Round
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class SelectedCategory
    {

        // <remarks/>
        [XmlArrayItem(IsNullable = false)]
        public CabinFilter[] CabinFilters;

        // <remarks/>
        [XmlElement("SelectedCabin")]
        public SelectedCabin[] SelectedCabin;

        // <remarks/>
        [XmlAttribute()]
        public string BerthedCategoryCode;

        // <remarks/>
        [XmlAttribute()]
        public string PricedCategoryCode;

        // <remarks/>
        [XmlAttribute()]
        public string DeckName;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class SelectedCabin
    {

        // <remarks/>
        [XmlArrayItem(IsNullable = false)]
        public CabinFilter[] CabinFilters;

        // <remarks/>
        [XmlAttribute()]
        public string Status;

        // <remarks/>
        [XmlAttribute()]
        public DateTime ReleaseDateTime;

        // <remarks/>
        [XmlIgnore()]
        public bool ReleaseDateTimeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string CabinNumber;

        // <remarks/>
        [XmlAttribute()]
        public string BedConfigurationCode;

        // <remarks/>
        [XmlAttribute()]
        public string MaxOccupancy;

        // <remarks/>
        [XmlAttribute()]
        public bool DeclineIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool DeclineIndicatorSpecified;

        // <remarks/>
        [XmlAttribute()]
        public bool HeldIndicator;

        // <remarks/>
        [XmlIgnore()]
        public bool HeldIndicatorSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CabinFilters
    {

        // <remarks/>
        [XmlElement("CabinFilter")]
        public CabinFilter[] CabinFilter;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CabinFilter
    {

        // <remarks/>
        [XmlAttribute()]
        public string CabinFilterCode;
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
    public class SelectedFare
    {

        // <remarks/>
        [XmlAttribute()]
        public string FareCode;

        // <remarks/>
        [XmlAttribute()]
        public string GroupCode;
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