
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmDisplayFares
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AdvResTicketing
    {

        // <remarks/>
        public AdvReservation AdvReservation;

        // <remarks/>
        public AdvTicketing AdvTicketing;

        // <remarks/>
        [XmlAttribute()]
        public string AdvResInd;

        // <remarks/>
        [XmlAttribute()]
        public string AdvTicketingInd;

        // <remarks/>
        [XmlAttribute()]
        public string RequestedTicketingDate;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AdvReservation
    {

        // <remarks/>
        [XmlAttribute()]
        public string LatestTimeOfDay;

        // <remarks/>
        [XmlAttribute()]
        public string LatestPeriod;

        // <remarks/>
        [XmlAttribute()]
        public AdvReservationLatestUnit LatestUnit;

        // <remarks/>
        [XmlIgnore()]
        public bool LatestUnitSpecified;
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
    [XmlRoot(IsNullable = false)]
    public class AdvTicketing
    {

        // <remarks/>
        [XmlAttribute()]
        public string FromResTimeOfDay;

        // <remarks/>
        [XmlAttribute()]
        public string FromResPeriod;

        // <remarks/>
        [XmlAttribute()]
        public AdvTicketingFromResUnit FromResUnit;

        // <remarks/>
        [XmlIgnore()]
        public bool FromResUnitSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string FromDepartTimeOfDay;

        // <remarks/>
        [XmlAttribute()]
        public string FromDepartPeriod;

        // <remarks/>
        [XmlAttribute()]
        public AdvTicketingFromDepartUnit FromDepartUnit;

        // <remarks/>
        [XmlIgnore()]
        public bool FromDepartUnitSpecified;
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

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Airline
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
    public class MinimumStay
    {

        // <remarks/>
        [XmlAttribute()]
        public string ReturnTimeOfDay;

        // <remarks/>
        [XmlAttribute()]
        public string MinStay;

        // <remarks/>
        [XmlAttribute()]
        public MinimumStayStayUnit StayUnit;

        // <remarks/>
        [XmlIgnore()]
        public bool StayUnitSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string MinStayDate;
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
    [XmlRoot(IsNullable = false)]
    public class MaximumStay
    {

        // <remarks/>
        [XmlAttribute()]
        public MaximumStayReturnType ReturnType;

        // <remarks/>
        [XmlIgnore()]
        public bool ReturnTypeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string ReturnTimeOfDay;

        // <remarks/>
        [XmlAttribute()]
        public string MaxStay;

        // <remarks/>
        [XmlAttribute()]
        public MaximumStayStayUnit StayUnit;

        // <remarks/>
        [XmlIgnore()]
        public bool StayUnitSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string MaxStayDate;
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
    [XmlRoot(IsNullable = false)]
    public class VoluntaryChanges
    {

        // <remarks/>
        public Penalty Penalty;

        // <remarks/>
        [XmlAttribute()]
        public string VolChangeInd;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Penalty
    {

        // <remarks/>
        [XmlAttribute()]
        public string PenaltyType;

        // <remarks/>
        [XmlAttribute()]
        public string DepartureStatus;

        // <remarks/>
        [XmlAttribute()]
        public string Amount;

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute()]
        public string DecimalPlaces;

        // <remarks/>
        [XmlAttribute()]
        public string Percent;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class ExchangeRate
    {

        // <remarks/>
        [XmlAttribute()]
        public string FromCurrency;

        // <remarks/>
        [XmlAttribute()]
        public string ToCurrency;

        // <remarks/>
        [XmlAttribute()]
        public string Rate;

        // <remarks/>
        [XmlAttribute()]
        public string Date;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class ExchangeRates
    {

        // <remarks/>
        [XmlElement("ExchangeRate")]
        public ExchangeRate[] ExchangeRate;
    }


}