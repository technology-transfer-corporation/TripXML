using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmETicketVerifyIn
{

    [XmlRoot(IsNullable = false)]
    public class AirItinerary
    {

        [XmlArrayItem("OriginDestinationOption", IsNullable = false)]
        [XmlArrayItem(IsNullable = false, NestingLevel = 1)]
        public FlightSegment[][] OriginDestinationOptions;

        [XmlAttribute()]
        public AirItineraryDirectionInd DirectionInd;

        [XmlIgnore()]
        public bool DirectionIndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class FlightSegment
    {

        public DepartureAirport DepartureAirport;

        public ArrivalAirport ArrivalAirport;

        public OperatingAirline OperatingAirline;

        [XmlElement("Equipment")]
        public Equipment[] Equipment;

        public MarketingAirline MarketingAirline;

        public string MarriageGrp;

        [XmlAttribute()]
        public string DepartureDateTime;

        [XmlAttribute()]
        public string ArrivalDateTime;

        [XmlIgnore()]
        public bool ArrivalDateTimeSpecified;

        [XmlAttribute()]
        public int StopQuantity;

        [XmlIgnore()]
        public bool StopQuantitySpecified;

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public string InfoSource;

        [XmlAttribute()]
        public string FlightNumber;

        [XmlAttribute()]
        public string TourOperatorFlightID;

        [XmlAttribute()]
        public string ResBookDesigCode;

        [XmlAttribute()]
        public FlightSegmentActionCode ActionCode;

        [XmlIgnore()]
        public bool ActionCodeSpecified;

        [XmlAttribute()]
        public int NumberInParty;

        [XmlIgnore()]
        public bool NumberInPartySpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class DepartureAirport
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
    public class MarketingAirline
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

    [XmlRoot(IsNullable = false)]
    public class Equipment
    {

        [XmlAttribute()]
        public string AirEquipType;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool ChangeofGauge = false;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class OperatingAirline
    {

        [XmlAttribute()]
        public string CompanyShortName;

        [XmlAttribute()]
        public string TravelSector;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute()]
        public string FlightNumber;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class ArrivalAirport
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        [XmlText()]
        public string Value;
    }

    public enum FlightSegmentActionCode
    {

        OK,

        Waitlist,

        Other
    }

    public enum AirItineraryDirectionInd
    {

        OneWay,

        Return,

        Circle,

        OpenJaw,

        Other
    }

    [XmlRoot(IsNullable = false)]
    public class BookingChannel
    {

        public string CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public bool Primary;

        [XmlIgnore()]
        public bool PrimarySpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class FlightSegmentRPHs
    {

        [XmlElement("FlightSegmentRPH")]
        public string[] FlightSegmentRPH;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_ETicketVerifyRQ
    {

        public POS POS;

        public AirItinerary AirItinerary;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_ETicketVerifyRQTarget.Production)]
        public OTA_ETicketVerifyRQTarget Target = OTA_ETicketVerifyRQTarget.Production;

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
        public OTA_ETicketVerifyRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

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

    // local CompanyName removed: identical to shared wsTripXML.Code.CompanyName (XML type-name collision in one serializer scope)

[XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;
    }

    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        public object Name;

        public string System;

        public string Userid;

        public string Password;
    }

    public enum OTA_ETicketVerifyRQTarget
    {

        Test,

        Production
    }

    public enum OTA_ETicketVerifyRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }

    [XmlRoot(IsNullable = false)]
    public class OriginDestinationOption
    {

        [XmlElement("FlightSegment")]
        public FlightSegment[] FlightSegment;
    }

    [XmlRoot(IsNullable = false)]
    public class OriginDestinationOptions
    {

        [XmlArrayItem(typeof(FlightSegment), IsNullable = false)]
        public FlightSegment[][] OriginDestinationOption;
    }

    [XmlRoot(IsNullable = false)]
    public class UniqueID
    {

        public string CompanyName;

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
