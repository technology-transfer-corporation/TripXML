using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmInventoryManagementIn
{
    [XmlRoot(IsNullable = false)]
    public class ArrivalAirport
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        public string CodeContext;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class TXML_InventoryManagementRQ
    {

        public POS POS;

        public FlightNumber FlightNumber;

        public DateTime InitialDate;

        public DateTime FinalDate;

        [XmlIgnore()]
        public bool FinalDateSpecified;

        [XmlElement("DayOfWeek")]
        public DayOfWeekRQ DayOfWeek;

        [XmlIgnore()]
        public bool DayOfWeekSpecified;

        public DepartureAirport DepartureAirport;

        public ArrivalAirport ArrivalAirport;

        [XmlElement("BookingClassPref")]
        public BookingClassPref[] BookingClassPref;

        [XmlAttribute()]
        public double Version;

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
    public class DepartureAirport
    {

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        public string CodeContext;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class BookingClassPref
    {

        [XmlAttribute()]
        public string ResBookDesigCode;

        [XmlAttribute()]
        public int NumberOfSeats;

        [XmlIgnore()]
        public bool NumberOfSeatsSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class FlightNumber
    {

        [XmlAttribute()]
        public InventoryAction InventoryAction;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public enum InventoryAction
    {

        AssignSeats,

        CloseClass
    }

    [XmlRoot(IsNullable = false)]
    public enum DayOfWeekRQ
    {

        Mon,

        Tue,

        Wed,

        Thu,

        Fri,

        Sat,

        Sun
    }
}