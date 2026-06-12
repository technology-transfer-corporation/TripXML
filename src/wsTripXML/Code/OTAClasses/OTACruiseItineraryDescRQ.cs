using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCruiseItineraryDescIn
{

    [XmlRoot(IsNullable = false)]
    public class BookingChannel
    {

        public Code.CompanyName CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public string Primary;
    }

    // local CompanyName removed: identical to shared wsTripXML.Code.CompanyName (XML type-name collision in one serializer scope)

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

        public Code.CompanyName CompanyName;

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
