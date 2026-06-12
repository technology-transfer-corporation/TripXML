using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmContractManager
{
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class ContractManagerRQ
    {
        [System.Xml.Serialization.XmlAttribute()]
        [System.ComponentModel.DefaultValue("")]
        public string RecordLocator = string.Empty;

        [System.Xml.Serialization.XmlAttribute()]
        [System.ComponentModel.DefaultValue(true)]
        public bool IsLoaded = true;

        [System.Xml.Serialization.XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool IsTracing = false;

        public POS POS;

        public string FQH;
        public string MISS;
    }

    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;
    }

[System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class Position
    {

        [System.Xml.Serialization.XmlAttribute()]
        public string Latitude;

        [System.Xml.Serialization.XmlAttribute()]
        public string Longitude;

        [System.Xml.Serialization.XmlAttribute()]
        public string Altitude;
    }

    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class BookingChannel
    {

        public CompanyName CompanyName;

        [System.Xml.Serialization.XmlAttribute()]
        public string Type;

        [System.Xml.Serialization.XmlAttribute()]
        public bool Primary;

        [System.Xml.Serialization.XmlIgnore()]
        public bool PrimarySpecified;
    }

    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class CompanyName
    {

        [System.Xml.Serialization.XmlAttribute()]
        public string CompanyShortName;

        [System.Xml.Serialization.XmlAttribute()]
        public string TravelSector;

        [System.Xml.Serialization.XmlAttribute()]
        public string Code;

        [System.Xml.Serialization.XmlAttribute()]
        public string CodeContext;

        [System.Xml.Serialization.XmlText()]
        public string Value;
    }

    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class Provider
    {

        public string Name;

        public string System;

        public string Userid;

        public string Password;
    }


}
