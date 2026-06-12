
using System.Xml.Serialization;
using wsTripXML.wsTravelTalk.wmCruiseBooking;

namespace wsTripXML.wsTravelTalk.wmCruiseCreateBooking
{

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

        // <remarks/>
        public SelectedCategory SelectedCategory;

        // <remarks/>
        public DeparturePort DeparturePort;

        // <remarks/>
        public ArrivalPort ArrivalPort;

        // <remarks/>
        public FaxNotification FaxNotification;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class FaxNotification
    {

        // <remarks/>
        [XmlElement("NotificationType")]
        public string[] NotificationType;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneLocationType;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneTechType;

        // <remarks/>
        [XmlAttribute()]
        public string CountryAccessCode;

        // <remarks/>
        [XmlAttribute()]
        public string AreaCityCode;

        // <remarks/>
        [XmlAttribute()]
        public string PhoneNumber;

        // <remarks/>
        [XmlAttribute()]
        public string Extension;

        // <remarks/>
        [XmlAttribute()]
        public string PIN;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class DeparturePort
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class ArrivalPort
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class ReservationID
    {

        // <remarks/>
        public Code.CompanyName CompanyName;

        // <remarks/>
        [XmlAttribute()]
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

        // <remarks/>
        [XmlAttribute()]
        public string SyncDateTime;
    }

}