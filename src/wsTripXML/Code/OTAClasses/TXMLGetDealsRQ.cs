using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmGetDealsIn
{

    [XmlRoot(IsNullable = false)]
    public class DepartureDate
    {

        [XmlText()]
        public DateTime Value;
    }

    [XmlRoot(IsNullable = false)]
    public class ReturnDate
    {

        [XmlText()]
        public DateTime Value;
    }

    [XmlRoot(IsNullable = false)]
    public class DestinationLocation
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public enum FareType
    {

        Published,

        Private
    }

    [XmlRoot(IsNullable = false)]
    public enum TripType
    {

        All,

        OneWay,

        RoundTrip
    }

    [XmlRoot(IsNullable = false)]
    public class TXML_GetLeadsRQ
    {

        public POS POS;

        public OriginDestinationInformation OriginDestinationInformation;

        public FareType FareType;

        [XmlIgnore()]
        public bool FareTypeSpecified;

        public VendorPref VendorPref;

        [XmlAttribute()]
        public double Version;

        [XmlIgnore()]
        public bool VersionSpecified;
    }
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {

        public TPA_Extensions TPA_Extensions;
    }

    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions
    {

        public Provider Provider;
    }

    [XmlRoot(IsNullable = false)]
    public class OriginDestinationInformation
    {

        public DepartureDate DepartureDate;

        public ReturnDate ReturnDate;

        public OriginLocation OriginLocation;

        public DestinationLocation DestinationLocation;

        [XmlAttribute()]
        public TripType TripType;

    }

    [XmlRoot(IsNullable = false)]
    public class OriginLocation
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class VendorPref
    {

        [XmlAttribute()]
        public string Code;

        [XmlText()]
        public string Value;
    }
}