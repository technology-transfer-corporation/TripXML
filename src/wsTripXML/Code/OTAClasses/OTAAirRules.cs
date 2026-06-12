using System;

namespace wsTripXML.wsTravelTalk.wmAirRules
{

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class AdvResTicketing
    {

        // <remarks/>
        public AdvReservation AdvReservation;

        // <remarks/>
        public AdvTicketing AdvTicketing;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public bool AdvResInd;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool AdvResIndSpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public bool AdvTicketingInd;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool AdvTicketingIndSpecified;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class AdvReservation
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public DateTime LatestTimeOfDay;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool LatestTimeOfDaySpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string LatestPeriod;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public AdvReservationLatestUnit LatestUnit;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool LatestUnitSpecified;
    }


    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class AdvTicketing
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public DateTime FromResTimeOfDay;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool FromResTimeOfDaySpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string FromResPeriod;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public AdvTicketingFromResUnit FromResUnit;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool FromResUnitSpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public DateTime FromDepartTimeOfDay;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool FromDepartTimeOfDaySpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string FromDepartPeriod;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public AdvTicketingFromDepartUnit FromDepartUnit;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool FromDepartUnitSpecified;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class DepartureAirport
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        // <remarks/>
        [System.Xml.Serialization.XmlText()]
        public string Value;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class ArrivalAirport
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        // <remarks/>
        [System.Xml.Serialization.XmlText()]
        public string Value;
    }


    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class ChargesRules
    {

        // <remarks/>
        public VoluntaryChanges VoluntaryChanges;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class VoluntaryChanges
    {

        // <remarks/>
        public Penalty Penalty;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public bool VolChangeInd;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool VolChangeIndSpecified;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class Penalty
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string PenaltyType;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string DepartureStatus;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public double Amount;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool AmountSpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public int DecimalPlaces;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool DecimalPlacesSpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public double Percent;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool PercentSpecified;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class FilingAirline
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Code;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [System.Xml.Serialization.XmlText()]
        public string Value;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class LengthOfStayRules
    {

        // <remarks/>
        public MinimumStay MinimumStay;

        // <remarks/>
        public MaximumStay MaximumStay;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public bool StayRestrictionsInd;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool StayRestrictionsIndSpecified;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class MinimumStay
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public DateTime ReturnTimeOfDay;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool ReturnTimeOfDaySpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public int MinStay;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool MinStaySpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public MinimumStayStayUnit StayUnit;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool StayUnitSpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public DateTime MinStayDate;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool MinStayDateSpecified;
    }

    // <remarks/>
    public enum MinimumStayStayUnit
    {

        // <remarks/>
        Minutes,

        // <remarks/>
        Hours,

        // <remarks/>
        Days,

        // <remarks/>
        Months,

        // <remarks/>
        MON,

        // <remarks/>
        TUES,

        // <remarks/>
        WED,

        // <remarks/>
        THU,

        // <remarks/>
        FRI,

        // <remarks/>
        SAT,

        // <remarks/>
        SUN
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class MaximumStay
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public MaximumStayReturnType ReturnType;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool ReturnTypeSpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public DateTime ReturnTimeOfDay;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool ReturnTimeOfDaySpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public int MaxStay;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool MaxStaySpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public MaximumStayStayUnit StayUnit;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool StayUnitSpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public DateTime MaxStayDate;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool MaxStayDateSpecified;
    }

    // <remarks/>
    public enum MaximumStayReturnType
    {

        // <remarks/>
        C,

        // <remarks/>
        S
    }

    // <remarks/>
    public enum MaximumStayStayUnit
    {

        // <remarks/>
        Minutes,

        // <remarks/>
        Hours,

        // <remarks/>
        Days,

        // <remarks/>
        Months,

        // <remarks/>
        MON,

        // <remarks/>
        TUES,

        // <remarks/>
        WED,

        // <remarks/>
        THU,

        // <remarks/>
        FRI,

        // <remarks/>
        SAT,

        // <remarks/>
        SUN
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class MarketingAirline
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string CompanyShortName;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string TravelSector;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Code;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string CodeContext;

        // <remarks/>
        [System.Xml.Serialization.XmlText()]
        public string Value;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class RuleInfo
    {

        // <remarks/>
        public ResTicketingRules ResTicketingRules;

        // <remarks/>
        public LengthOfStayRules LengthOfStayRules;

        // <remarks/>
        public ChargesRules ChargesRules;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class ResTicketingRules
    {

        // <remarks/>
        public AdvResTicketing AdvResTicketing;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class SubSection
    {

        // <remarks/>
        [System.Xml.Serialization.XmlElement("Paragraph")]
        public Paragraph[] Paragraph;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string SubTitle;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string SubCode;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public int SubSectionNumber;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool SubSectionNumberSpecified;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class Paragraph
    {

        // <remarks/>
        public Text Text;

        // <remarks/>
        public string Image;

        // <remarks/>
        [System.Xml.Serialization.XmlElement(DataType = "anyURI")]
        public string URL;

        // <remarks/>
        public ListItem ListItem;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Name;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public int ParagraphNumber;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool ParagraphNumberSpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public DateTime CreateDateTime;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool CreateDateTimeSpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string CreatorID;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public DateTime LastModifyDateTime;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool LastModifyDateTimeSpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string LastModifierID;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Language;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class Text
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public bool Formatted;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool FormattedSpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Language;

        // <remarks/>
        [System.Xml.Serialization.XmlText()]
        public string Value;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class ListItem
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public bool Formatted;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool FormattedSpecified;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Language;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute("ListItem")]
        public int ListItem1;

        // <remarks/>
        [System.Xml.Serialization.XmlIgnore()]
        public bool ListItem1Specified;

        // <remarks/>
        [System.Xml.Serialization.XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum AdvReservationLatestUnit
    {

        // <remarks/>
        Minutes,

        // <remarks/>
        Hours,

        // <remarks/>
        Days,

        // <remarks/>
        Months,

        // <remarks/>
        MON,

        // <remarks/>
        TUES,

        // <remarks/>
        WED,

        // <remarks/>
        THU,

        // <remarks/>
        FRI,

        // <remarks/>
        SAT,

        // <remarks/>
        SUN
    }

    // <remarks/>
    public enum AdvTicketingFromResUnit
    {

        // <remarks/>
        Minutes,

        // <remarks/>
        Hours,

        // <remarks/>
        Days,

        // <remarks/>
        Months,

        // <remarks/>
        MON,

        // <remarks/>
        TUES,

        // <remarks/>
        WED,

        // <remarks/>
        THU,

        // <remarks/>
        FRI,

        // <remarks/>
        SAT,

        // <remarks/>
        SUN
    }

    // <remarks/>
    public enum AdvTicketingFromDepartUnit
    {

        // <remarks/>
        Minutes,

        // <remarks/>
        Hours,

        // <remarks/>
        Days,

        // <remarks/>
        Months,

        // <remarks/>
        MON,

        // <remarks/>
        TUES,

        // <remarks/>
        WED,

        // <remarks/>
        THU,

        // <remarks/>
        FRI,

        // <remarks/>
        SAT,

        // <remarks/>
        SUN
    }

}