
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmAirPrice
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class OriginDestinationOption
    {

        // <remarks/>
        [XmlElement("FlightSegment")]
        public FlightSegment[] FlightSegment;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class OriginDestinationOptions
    {

        // <remarks/>
        [XmlArrayItem(typeof(FlightSegment), IsNullable = false)]
        public FlightSegment[][] OriginDestinationOption;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AirItinerary
    {

        // <remarks/>
        [XmlArrayItem("OriginDestinationOption", IsNullable = false)]
        [XmlArrayItem(IsNullable = false, NestingLevel = 1)]
        public FlightSegment[][] OriginDestinationOptions;

        // <remarks/>
        [XmlAttribute()]
        public AirItineraryDirectionInd DirectionInd;

        // <remarks/>
        [XmlIgnore()]
        public bool DirectionIndSpecified;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class FlightSegment
    {

        // <remarks/>
        public DepartureAirport DepartureAirport;

        // <remarks/>
        public ArrivalAirport ArrivalAirport;

        // <remarks/>
        public OperatingAirline OperatingAirline;

        // <remarks/>
        [XmlElement("Equipment")]
        public Equipment[] Equipment;

        // <remarks/>
        public MarketingAirline MarketingAirline;

        // <remarks/>
        public string MarriageGrp;

        // <remarks/>
        [XmlAttribute()]
        public string DepartureDateTime;

        // <remarks/>
        [XmlAttribute()]
        public string ArrivalDateTime;

        // <remarks/>
        [XmlIgnore()]
        public bool ArrivalDateTimeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public int StopQuantity;

        // <remarks/>
        [XmlIgnore()]
        public bool StopQuantitySpecified;

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public string InfoSource;

        // <remarks/>
        [XmlAttribute()]
        public string FlightNumber;

        // <remarks/>
        [XmlAttribute()]
        public string TourOperatorFlightID;

        // <remarks/>
        [XmlAttribute()]
        public string ResBookDesigCode;

        // <remarks/>
        [XmlAttribute()]
        public FlightSegmentActionCode ActionCode;

        // <remarks/>
        [XmlIgnore()]
        public bool ActionCodeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public int NumberInParty;

        // <remarks/>
        [XmlIgnore()]
        public bool NumberInPartySpecified;

        // <remarks/>
        [XmlAttribute()]
        public string FlightContext;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class DepartureAirport
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class ArrivalAirport
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class OperatingAirline
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlAttribute()]
        public string FlightNumber;

        // <remarks/>
        [XmlText()]
        public string Value;


    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class MarketingAirline
    {

        // <remarks/>
        [XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Equipment
    {

        // <remarks/>
        [XmlAttribute()]
        public string AirEquipType;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool ChangeofGauge = false;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PassengerTypeQuantity
    {

        // <remarks/>
        [XmlAttribute()]
        public int Age;

        // <remarks/>
        [XmlIgnore()]
        public bool AgeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string URI;

        // <remarks/>
        [XmlAttribute()]
        public int Quantity;

        // <remarks/>
        [XmlIgnore()]
        public bool QuantitySpecified;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(true)]
        public bool Changeable = true;
    }

    // <remarks/>
    public enum FlightSegmentActionCode
    {

        // <remarks/>
        OK,

        // <remarks/>
        Waitlist,

        // <remarks/>
        Other
    }

    // <remarks/>
    public enum AirItineraryDirectionInd
    {

        // <remarks/>
        OneWay,

        // <remarks/>
        Return,

        // <remarks/>
        Circle,

        // <remarks/>
        OpenJaw,

        // <remarks/>
        Other
    }

}