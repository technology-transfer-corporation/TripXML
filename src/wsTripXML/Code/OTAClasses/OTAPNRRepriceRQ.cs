using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmPNRRepriceIn
{

    [XmlRoot(IsNullable = false)]
    public class OTA_PNRRepriceRQ
    {

        public POS POS;

        public UniqueIDRQ UniqueID;

        public string FareNumber;

        [XmlElement("StoredFare")]
        public StoredFare[] StoredFare;

        [XmlElement("FlightReference")]
        public FlightReference[] FlightReference;

        public string ConversationID;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(Target.Production)]
        public Target Target = Target.Production;

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
        public TransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public string ReqRespVersion;

        [XmlAttribute()]
        public string ReservationType;

        [XmlAttribute()]
        public bool ReturnListIndicator;

        [XmlIgnore()]
        public bool ReturnListIndicatorSpecified;

        [XmlAttribute()]
        public int MaxResponses;

        [XmlIgnore()]
        public bool MaxResponsesSpecified;

        [XmlAttribute()]
        public bool StoreHistoricalFare;

        [XmlIgnore()]
        public bool StoreHistoricalFareSpecified;

        [XmlAttribute()]
        public bool StoreFare = true;

        [XmlIgnore()]
        public bool StoreFareSpecified;
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
    public class BookingChannel
    {

        public CompanyName CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public bool Primary;

        [XmlIgnore()]
        public bool PrimarySpecified;
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
    public class UserID
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

        [XmlAttribute()]
        public string PinNumber;
    }

    public class StoredFare
    {
        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public string FarePriceGroup;

        [XmlAttribute()]
        public FareTypes FareType;

        [XmlAttribute()]
        public FareQualifier FareQualifier;

        [XmlElement("PassengerType")]
        public PassengerType[] PassengerType;

        public Markup Markup;

        public Discount Discount;

        [XmlArrayItem("FareFamily", IsNullable = false)]
        public FareFamily[] BrandedFares;

        [XmlArrayItem("AirSegments", IsNullable = false)]
        public FareSegment[] FareSegments;

        public string TourCode;

        public string Endorsement;

        public string CorporateId;

    }

    public class FlightReference
    {

        [XmlAttribute()]
        public int RPH;

    }

    public enum FareTypes
    {

        Published,

        Private,

        Web

    }


    public enum FareQualifier
    {

        IT,
        EXC,
        EX,
        EXL,
        NL,
        NLL,
        NLX,
        NXL,
        CHL,
        CH,
        CT,
        FP,
        GVC,
        GV,
        GVX,
        GP,
        MLC,
        MLX,
        ML,
        MSC,
        MSX,
        PRC,
        PR,
        RW,
        SR,
        SRC
    }


    public class PassengerType
    {

        [XmlAttribute()]
        public int Quantity;

        [XmlAttribute()]
        public string Code;
    }

    public class Discount
    {
        [XmlAttribute()]
        public string Percent;

        [XmlAttribute()]
        public string Amount;

    }

    public class FareFamily
    {

        [XmlAttribute()]
        public string RPH;

        [XmlAttribute()]
        public string Code;

        [XmlText()]
        public string Value;

    }

    public class FareSegment
    {
        [XmlAttribute()]
        public int RPH;

        [XmlAttribute()]
        public string TicketDesignator;

        [XmlText()]
        public string FareBasis;

    }

    public class Markup
    {

        [XmlAttribute()]
        public string Amount;

    }

    [XmlRoot(IsNullable = false)]
    public class UniqueIDRQ
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

    public enum Target
    {

        Test,

        Production,

        WSP,

        GAL,

        APL
    }

    public enum TransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }
}
