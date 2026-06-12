using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCruiseCabinUnholdIn
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

    [XmlRoot(IsNullable = false)]
    public class OTA_CruiseCabinUnholdRQ
    {

        public POS POS;

        [XmlElement("SelectedSailing")]
        public SelectedSailing[] SelectedSailing;

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
    }

    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
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

        [XmlElement("SelectedCabin")]
        public SelectedCabin[] SelectedCabin;

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

        [XmlAttribute(DataType = "NMTOKEN")]
        public string Status;
    }

    [XmlRoot(IsNullable = false)]
    public class SelectedCabin
    {

        [XmlAttribute()]
        public string CabinNumber;

        [XmlAttribute()]
        public string BedConfigurationCode;

        [XmlAttribute()]
        public string DeclineIndicator;

        [XmlAttribute()]
        public string MaxOccupancy;

        [XmlAttribute()]
        public string HeldIndicator;
    }

    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        [XmlElement("Provider")]
        public Provider[] Provider;
    }
}
