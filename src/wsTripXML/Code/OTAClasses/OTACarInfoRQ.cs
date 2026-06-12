using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCarInfoIn
{

    [XmlRoot(IsNullable = false)]
    public class BookingChannel
    {

        public Code.CompanyName CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public bool Primary;

        [XmlIgnore()]
        public bool PrimarySpecified;
    }

    // local CompanyName removed: identical to shared wsTripXML.Code.CompanyName (XML type-name collision in one serializer scope)

    [XmlRoot(IsNullable = false)]
    public class Location
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_VehLocDetailRQ
    {

        public POS POS;

        public Location Location;

        public wmCarInfo.Vendor Vendor;

        public ResponseFilter ResponseFilter;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_VehLocDetailRQTarget.Production)]
        public OTA_VehLocDetailRQTarget Target = OTA_VehLocDetailRQTarget.Production;

        [XmlAttribute()]
        public double Version;

        [XmlIgnore()]
        public bool VersionSpecified;

        [XmlAttribute()]
        public string TransactionIdentifier;

        [XmlAttribute()]
        public int SequenceNmbr;

        [XmlIgnore()]
        public bool SequenceNmbrSpecified;

        [XmlAttribute()]
        public OTA_VehLocDetailRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public string ReqRespVersion;
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
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;
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
    public class ResponseFilter
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool LocationAddressPhoneIndicator = false;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool LocationInfoIndicator = false;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool VehiclesIndicator = false;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool RequirementsIndicator = false;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool AdditionalFeesIndicator = false;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool LiabilitiesIndicator = false;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool ServicesOfferedIndicator = false;
    }

    public enum OTA_VehLocDetailRQTarget
    {

        Test,

        Production
    }

    public enum OTA_VehLocDetailRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }
}
