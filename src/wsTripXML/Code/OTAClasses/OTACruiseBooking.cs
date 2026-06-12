
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCruiseBooking
{

    // local CompanyName removed: identical to shared wsTripXML.Code.CompanyName (XML type-name collision in one serializer scope)

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
        public string ReleaseDateTime;

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
        public string DeclineIndicator;

        // <remarks/>
        [XmlAttribute()]
        public string HeldIndicator;
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
    public class CabinFilters
    {

        // <remarks/>
        [XmlElement("CabinFilter")]
        public CabinFilter[] CabinFilter;
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
    public class GuestCity
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class GuestTransportation
    {

        // <remarks/>
        public GuestCity GuestCity;

        // <remarks/>
        public GatewayCity GatewayCity;

        // <remarks/>
        [XmlAttribute()]
        public string TransportationMode;

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string TransportationStatus;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class GatewayCity
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class ListItem
    {

        // <remarks/>
        [XmlAttribute()]
        public string Formatted;

        // <remarks/>
        [XmlAttribute()]
        public string Language;

        // <remarks/>
        [XmlAttribute("ListItem")]
        public string ListItem1;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Text
    {

        // <remarks/>
        [XmlAttribute()]
        public string Formatted;

        // <remarks/>
        [XmlAttribute()]
        public string Language;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

}