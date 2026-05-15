using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCruiseItineraryDescIn
{

    [XmlRoot(IsNullable = false)]
    public class BookingChannel
    {

        public CompanyName CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public string Primary;
    }

    [XmlRoot(IsNullable = false)]
    public class CompanyName
    {

        [XmlAttribute()]
        public string CompanyShortName;

        [XmlAttribute()]
        public string TravelSector;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;

        [XmlText()]
        public string Value;
    }

    [XmlRoot()]
    public class PackageOption
    {

        [XmlAttribute()]
        public string CruisePackageCode;

        [XmlAttribute()]
        public bool InclusiveIndicator;

        [XmlAttribute()]
        public string Start;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute()]
        public string End;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_CruiseItineraryDescRQ
    {

        public POS POS;

        public ReservationID ReservationID;

        public SelectedSailing SelectedSailing;

        public PackageOption PackageOption;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public string TimeStamp;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string Target;

        [XmlAttribute()]
        public string Version;

        [XmlAttribute()]
        public string TransactionIdentifier;

        [XmlAttribute()]
        public string SequenceNmbr;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string TransactionStatusCode;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public bool RetransmissionIndicator;
    }

    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    [XmlRoot(IsNullable = false)]
    public class Position
    {

        [XmlAttribute()]
        public string Latitude;

        [XmlAttribute()]
        public string Longitude;

        [XmlAttribute()]
        public string Altitude;
    }

    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        public string Name;

        public string System;

        public string Userid;

        public string Password;
    }

    [XmlRoot(IsNullable = false)]
    public class SelectedSailing
    {

        [XmlAttribute()]
        public string VoyageID;

        [XmlAttribute()]
        public string Start;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute()]
        public string End;

        [XmlAttribute()]
        public string VendorCode;

        [XmlAttribute()]
        public string VendorName;

        [XmlAttribute()]
        public string ShipCode;

        [XmlAttribute()]
        public string ShipName;

        [XmlAttribute()]
        public string VendorCodeContext;

        [XmlAttribute(DataType = "NMTOKEN")]
        public string Status;
    }

    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        [XmlElement("Provider")]
        public Provider[] Provider;
    }

    [XmlRoot()]
    public class ReservationID
    {

        public CompanyName CompanyName;

        [XmlAttribute(DataType = "anyURI")]
        public string URL;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public string Instance;

        [XmlAttribute()]
        public string ID;

        [XmlAttribute()]
        public string ID_Context;
    }
}
