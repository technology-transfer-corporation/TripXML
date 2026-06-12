using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmTravelItinerary
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class StreetNmbr
    {

        // <remarks/>
        [XmlAttribute()]
        public string PO_Box;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class StateProv
    {

        // <remarks/>
        [XmlAttribute()]
        public string StateCode;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CountryName
    {

        // <remarks/>
        [XmlAttribute()]
        public string Code;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum AddressShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AddressShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum DirectBillShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum DirectBillShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class BankAcct
    {

        // <remarks/>
        public string BankAcctName;

        // <remarks/>
        [XmlAttribute()]
        public BankAcctShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public BankAcctShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string BankID;

        // <remarks/>
        [XmlAttribute()]
        public string AcctType;

        // <remarks/>
        [XmlAttribute()]
        public string BankAcctNumber;
    }

    // <remarks/>
    public enum BankAcctShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum BankAcctShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum PaymentCardShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum PaymentCardShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum PaymentCardCardCode
    {

        // <remarks/> Carte Aurore
        AU,

        // <remarks/> Amex
        AX,

        // <remarks/>
        BC,

        // <remarks/>
        BL,

        // <remarks/> Carte Blanche
        CB,

        // <remarks/> Cofinoga
        CG,

        // <remarks/> Connect
        CN,

        // <remarks/> Choice
        CX,

        // <remarks/> Diners (DC)
        DN,

        // <remarks/>
        DK,

        // <remarks/> Discover
        DS,

        // <remarks/>
        EC,

        // <remarks/> Lufthansa GK Card
        GK,

        // <remarks/> JCB
        JC,

        // <remarks/> Mastercard (CA)
        MC,

        // <remarks/> Mastercard Debit
        MD,

        // <remarks/> Mastercard Maestro
        MO,

        // <remarks/> Mastercard Prepaid
        MP,

        // <remarks/> Solo
        SO,

        // <remarks/> Switch
        SW,

        // <remarks/> Torch Club
        TC,

        // <remarks/> UATP
        TP,

        // <remarks/> Visa
        VI,

        // <remarks/> Visa Debit
        VD,

        // <remarks/> Visa Electron
        VE,

        // <remarks/> Visa Delta
        VT,

        // <remarks/> Access
        XS


    }

    // <remarks/>
    public enum AcceptedPaymentShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum AcceptedPaymentShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PersonName
    {

        // <remarks/>
        public string NamePrefix;

        // <remarks/>
        public string GivenName;

        // <remarks/>
        public string MiddleName;

        // <remarks/>
        public string SurnamePrefix;

        // <remarks/>
        public string Surname;

        // <remarks/>
        public string NameSuffix;

        // <remarks/>
        public string NameTitle;

        // <remarks/>
        [XmlAttribute()]
        public PersonNameShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public PersonNameShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public string NameType;
    }

    // <remarks/>
    public enum PersonNameShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum PersonNameShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum TelephoneShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum TelephoneShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Email
    {

        // <remarks/>
        [XmlAttribute()]
        public EmailShareSynchInd ShareSynchInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareSynchIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        public EmailShareMarketInd ShareMarketInd;

        // <remarks/>
        [XmlIgnore()]
        public bool ShareMarketIndSpecified;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool DefaultInd = false;

        // <remarks/>
        [XmlAttribute()]
        public string EmailType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum EmailShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum EmailShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum DocumentShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum DocumentShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum DocumentGender
    {

        // <remarks/>
        Male,

        // <remarks/>
        Female,

        // <remarks/>
        Unknown
    }

    // <remarks/>
    public enum CustLoyaltyShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum CustLoyaltyShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum CustLoyaltySingleVendorInd
    {

        // <remarks/>
        SingleVndr,

        // <remarks/>
        Alliance
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TicketAdvisory
    {

        // <remarks/>
        [XmlAttribute()]
        public string Language;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AdvReservation
    {

        // <remarks/>
        [XmlAttribute()]
        public DateTime LatestTimeOfDay;

        // <remarks/>
        [XmlIgnore()]
        public bool LatestTimeOfDaySpecified;

        // <remarks/>
        [XmlAttribute()]
        public string LatestPeriod;

        // <remarks/>
        [XmlAttribute()]
        public AdvReservationLatestUnit LatestUnit;

        // <remarks/>
        [XmlIgnore()]
        public bool LatestUnitSpecified;

        // <remarks/>
        [XmlText()]
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

    [XmlRoot(IsNullable = false)]
    public class StopInfo
    {

        // <remarks/>
        [XmlAttribute()]
        public string LocationCode;

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue("IATA")]
        public string CodeContext = "IATA";

        // <remarks/>
        [XmlAttribute()]
        public string ArrivalDateTime;

        // <remarks/>
        [XmlAttribute()]
        public string DepartureDateTime;

        // <remarks/>
        [XmlText()]
        public string Value;
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
        [XmlAttribute()]
        public string Terminal;

        // <remarks/>
        [XmlAttribute()]
        public string Gate;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class MarriageGrp
    {

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
        [XmlAttribute()]
        public string Terminal;

        // <remarks/>
        [XmlAttribute()]
        public string Gate;

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
        public OperatingAirlineFlightNumber FlightNumber;

        // <remarks/>
        [XmlIgnore()]
        public bool FlightNumberSpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum OperatingAirlineFlightNumber
    {

        // <remarks/>
        OPEN,

        // <remarks/>
        ARNK
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
    //[XmlRoot(IsNullable = false)]
    //public class CompanyNameFull
    //{

    //    // <remarks/>
    //    [XmlAttribute()]
    //    public string CompanyShortName;

    //    // <remarks/>
    //    [XmlAttribute()]
    //    public string TravelSector;

    //    // <remarks/>
    //    [XmlAttribute()]
    //    public string Code;

    //    // <remarks/>
    //    [XmlAttribute()]
    //    public string CodeContext;

    //    // <remarks/>
    //    [XmlText()]
    //    public string Value;
    //}

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
    public class Amenity
    {

        // <remarks/>
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(AmenityPreferLevel.Preferred)]
        public AmenityPreferLevel PreferLevel = AmenityPreferLevel.Preferred;

        // <remarks/>
        [XmlAttribute()]
        public string RoomAmenity;

        // <remarks/>
        [XmlAttribute()]
        public int Quantity;

        // <remarks/>
        [XmlIgnore()]
        public bool QuantitySpecified;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum AmenityPreferLevel
    {

        // <remarks/>
        Only,

        // <remarks/>
        Unacceptable,

        // <remarks/>
        Preferred
    }

    // <remarks/>
    public enum StartDateWindowDOW
    {

        // <remarks/>
        Mon,

        // <remarks/>
        Tue,

        // <remarks/>
        Wed,

        // <remarks/>
        Thu,

        // <remarks/>
        Fri,

        // <remarks/>
        Sat,

        // <remarks/>
        Sun
    }

    // <remarks/>
    public enum EndDateWindowDOW
    {

        // <remarks/>
        Mon,

        // <remarks/>
        Tue,

        // <remarks/>
        Wed,

        // <remarks/>
        Thu,

        // <remarks/>
        Fri,

        // <remarks/>
        Sat,

        // <remarks/>
        Sun
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class FareBasisCodes
    {

        // <remarks/>
        [XmlElement("FareBasisCode")]
        public string[] FareBasisCode;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class FilingAirline
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
    public class FlightRefNumber
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    public enum GuaranteeAcceptedShareSynchInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum GuaranteeAcceptedShareMarketInd
    {

        // <remarks/>
        Yes,

        // <remarks/>
        No,

        // <remarks/>
        Inherit
    }

    // <remarks/>
    public enum GuaranteeRetributionType
    {

        // <remarks/>
        ResAutoCancelled,

        // <remarks/>
        ResNotGuaranteed
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class UniqueID
    {

        // <remarks/>
        public Code.CompanyName CompanyName;

        // <remarks/>
        [XmlAttribute(DataType = "anyURI")]
        public string URL;

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        public string Instance;

        // <remarks/>
        [XmlAttribute()]
        public string ID;

        // <remarks/>
        [XmlAttribute()]
        public string ID_Context;
    }

    // <remarks/>
    public enum PTC_FareBreakdownPricingSource
    {

        // <remarks/>
        Published,

        // <remarks/>
        Private,

        // <remarks/>
        Both
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class SeatRequest
    {

        // <remarks/>
        public DepartureAirport DepartureAirport;

        // <remarks/>
        public ArrivalAirport ArrivalAirport;

        // <remarks/>
        [XmlAttribute()]
        public string SeatNumber;

        // <remarks/>
        [XmlAttribute()]
        public string SeatPreference;

        // <remarks/>
        [XmlAttribute()]
        public bool SmokingAllowed;

        // <remarks/>
        [XmlAttribute()]
        public string Status;

        // <remarks/>
        [XmlAttribute()]
        public string TravelerRefNumberRPHList;

        // <remarks/>
        [XmlAttribute()]
        public string FlightRefNumberRPHList;

        // <remarks/>
        [XmlAttribute()]
        public bool Chargeable;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class SeatRequests
    {

        // <remarks/>
        [XmlElement("SeatRequest")]
        public SeatRequest[] SeatRequest;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class SpecialServiceRequests
    {

        // <remarks/>
        [XmlElement("SpecialServiceRequest")]
        public SpecialServiceRequest[] SpecialServiceRequest;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class SpecialServiceRequest
    {

        // <remarks/>
        public Airline Airline;

        // <remarks/>
        public string Text;

        // <remarks/>
        [XmlAttribute()]
        public string SSRCode;

        // <remarks/>
        [XmlAttribute()]
        public string TravelerRefNumberRPHList;

        // <remarks/>
        [XmlAttribute()]
        public string FlightRefNumberRPHList;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class UniqueRemark
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;

        // <remarks/>
        [XmlAttribute()]
        public string RemarkType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class UniqueRemarks
    {

        // <remarks/>
        [XmlElement("UniqueRemark")]
        public UniqueRemark[] UniqueRemark;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PickUpLocation
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
    public class ReturnLocation
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
    public class VehRentalCore
    {

        // <remarks/>
        public PickUpLocation PickUpLocation;

        // <remarks/>
        public ReturnLocation ReturnLocation;

        // <remarks/>
        [XmlAttribute()]
        public DateTime PickUpDateTime;

        // <remarks/>
        [XmlIgnore()]
        public bool PickUpDateTimeSpecified;

        // <remarks/>
        [XmlAttribute()]
        public DateTime ReturnDateTime;

        // <remarks/>
        [XmlIgnore()]
        public bool ReturnDateTimeSpecified;
    }

    // <remarks/>
    public enum RateQualifierRatePeriod
    {

        // <remarks/>
        Hourly,

        // <remarks/>
        Daily,

        // <remarks/>
        Weekly,

        // <remarks/>
        Monthly,

        // <remarks/>
        WeekendDay,

        // <remarks/>
        Other
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TravelerRefNumber
    {

        // <remarks/>
        [XmlAttribute()]
        public string RPH;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class OtherServiceInformation
    {

        // <remarks/>
        [XmlElement("TravelerRefNumber")]
        public TravelerRefNumber[] TravelerRefNumber;

        // <remarks/>
        public Airline Airline;

        // <remarks/>
        public string Text;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class OtherServiceInformations
    {

        // <remarks/>
        [XmlElement("OtherServiceInformation")]
        public OtherServiceInformation[] OtherServiceInformation;
    }

}