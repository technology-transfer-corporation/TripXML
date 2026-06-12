using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCarAvailIn
{

    [XmlRoot(IsNullable = false)]
    public class Additional
    {

        public wmCarAvail.PersonName PersonName;

        [XmlElement("Telephone")]
        public wmCarAvail.Telephone[] Telephone;

        [XmlElement("Email")]
        public wmCarAvail.Email[] Email;

        [XmlElement("Address")]
        public wmCarAvail.Address[] Address;

        [XmlElement("CitizenCountryName")]
        public wmCarAvail.CitizenCountryName[] CitizenCountryName;

        [XmlElement("Document")]
        public wmCarAvail.Document[] Document;

        [XmlElement("CustLoyalty")]
        public wmCarAvail.CustLoyalty[] CustLoyalty;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate;

        [XmlIgnore()]
        public bool BirthDateSpecified;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public int DecimalPlaces;

        [XmlIgnore()]
        public bool DecimalPlacesSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class VehOptions
    {

        [XmlAttribute()]
        public string LowRateRange;

        [XmlAttribute()]
        public string HighRateRange;

        [XmlAttribute()]
        public string LocationCategory;

        [XmlAttribute()]
        public string LocationCode;

        [XmlAttribute()]
        public bool GuaranteedRateInd;

        [XmlIgnore()]
        public bool GuaranteedRateIndSpecified;

        [XmlAttribute()]
        public bool UnlimitedMilesInd;

        [XmlIgnore()]
        public bool UnlimitedMilesIndSpecified;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        [XmlElement("Name")]
        public Name[] Name;

        [XmlElement("System")]
        public string GDSSystem;

        public string Userid;

        public string Password;
    }

    [XmlRoot(IsNullable = false)]
    public class Name
    {

        [XmlAttribute()]
        public string PseudoCityCode;

        [XmlText()]
        public string Value;
    }

    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;

        [XmlElement("VehOptions")]
        public VehOptions[] VehOptions;

        [XmlText()]
        public string[] Text;
    }

    [XmlRoot(IsNullable = false)]
    public class ArrivalDetails
    {

        public ArrivalLocation ArrivalLocation;

        public MarketingCompany MarketingCompany;

        public OperatingCompany OperatingCompany;

        [XmlAttribute()]
        public string TransportationCode;

        [XmlAttribute()]
        public string Number;

        [XmlAttribute()]
        public DateTime ArrivalDateTime;

        [XmlIgnore()]
        public bool ArrivalDateTimeSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class ArrivalLocation
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
    public class MarketingCompany
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
    public class OperatingCompany
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
    public class CoveragePref
    {

        [XmlAttribute()]
        public string CoverageType;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(CoveragePrefPreferLevel.Preferred)]
        public CoveragePrefPreferLevel PreferLevel = CoveragePrefPreferLevel.Preferred;
    }

    public enum CoveragePrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class CoveragePrefs
    {

        [XmlElement("CoveragePref")]
        public CoveragePref[] CoveragePref;
    }

    [XmlRoot(IsNullable = false)]
    public class Customer
    {

        public Primary Primary;

        [XmlElement("Additional")]
        public Additional[] Additional;
    }

    [XmlRoot(IsNullable = false)]
    public class Primary
    {

        public wmCarAvail.PersonName PersonName;

        [XmlElement("Telephone")]
        public wmCarAvail.Telephone[] Telephone;

        [XmlElement("Email")]
        public wmCarAvail.Email[] Email;

        [XmlElement("Address")]
        public wmCarAvail.Address[] Address;

        [XmlElement("CitizenCountryName")]
        public wmCarAvail.CitizenCountryName[] CitizenCountryName;

        [XmlElement("Document")]
        public wmCarAvail.Document[] Document;

        [XmlElement("CustLoyalty")]
        public wmCarAvail.CustLoyalty[] CustLoyalty;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute(DataType = "date")]
        public DateTime BirthDate;

        [XmlIgnore()]
        public bool BirthDateSpecified;

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public int DecimalPlaces;

        [XmlIgnore()]
        public bool DecimalPlacesSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class DriverType
    {

        [XmlAttribute()]
        public int Age;

        [XmlIgnore()]
        public bool AgeSpecified;

        [XmlAttribute()]
        public string Code;

        [XmlAttribute()]
        public string CodeContext;

        [XmlAttribute(DataType = "anyURI")]
        public string URI;

        [XmlAttribute()]
        public int Quantity;

        [XmlIgnore()]
        public bool QuantitySpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_VehAvailRateRQ
    {

        public POS POS;

        public VehAvailRQCore VehAvailRQCore;

        public VehAvailRQInfo VehAvailRQInfo;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_VehAvailRateRQTarget.Production)]
        public OTA_VehAvailRateRQTarget Target = OTA_VehAvailRateRQTarget.Production;

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
        public OTA_VehAvailRateRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public string ReqRespVersion;

        [XmlAttribute()]
        public int MaxResponses;

        [XmlIgnore()]
        public bool MaxResponsesSpecified;
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
    public class VehAvailRQCore
    {

        public wmCarAvail.VehRentalCore VehRentalCore;

        [XmlArrayItem(IsNullable = false)]
        public VendorPref[] VendorPrefs;

        [XmlArrayItem(IsNullable = false)]
        public VehPref[] VehPrefs;

        [XmlElement("DriverType")]
        public DriverType[] DriverType;

        public RateQualifier RateQualifier;

        [XmlArrayItem(IsNullable = false)]
        public SpecialEquipPref[] SpecialEquipPrefs;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public VehAvailRQCoreStatus Status;
    }

    [XmlRoot(IsNullable = false)]
    public class VendorPref
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
        [System.ComponentModel.DefaultValue(VendorPrefPreferLevel.Preferred)]
        public VendorPrefPreferLevel PreferLevel = VendorPrefPreferLevel.Preferred;

        [XmlText()]
        public string Value;
    }

    public enum VendorPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class VehPref
    {

        public VehType VehType;

        public wmCarAvail.VehClass VehClass;

        [XmlAttribute()]
        public bool AirConditionInd;

        [XmlIgnore()]
        public bool AirConditionIndSpecified;

        [XmlAttribute()]
        public VehPrefTransmissionType TransmissionType;

        [XmlIgnore()]
        public bool TransmissionTypeSpecified;

        [XmlAttribute()]
        public VehPrefTypePref TypePref;

        [XmlIgnore()]
        public bool TypePrefSpecified;

        [XmlAttribute()]
        public VehPrefClassPref ClassPref;

        [XmlIgnore()]
        public bool ClassPrefSpecified;

        [XmlAttribute()]
        public VehPrefAirConditionPref AirConditionPref;

        [XmlIgnore()]
        public bool AirConditionPrefSpecified;

        [XmlAttribute()]
        public VehPrefTransmissionPref TransmissionPref;

        [XmlIgnore()]
        public bool TransmissionPrefSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class VehType
    {

        [XmlAttribute()]
        public string VehicleCategory;

        [XmlAttribute()]
        public int DoorCount;

        [XmlIgnore()]
        public bool DoorCountSpecified;
    }

    public enum VehPrefTransmissionType
    {

        Automatic,

        Manual
    }

    public enum VehPrefTypePref
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum VehPrefClassPref
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum VehPrefAirConditionPref
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum VehPrefTransmissionPref
    {

        Only,

        Unacceptable,

        Preferred
    }

    [XmlRoot(IsNullable = false)]
    public class RateQualifier
    {

        [XmlAttribute()]
        public string TravelPurpose;

        [XmlAttribute()]
        public string RateCategory;

        [XmlAttribute()]
        public string CorpDiscountNmbr;

        [XmlAttribute()]
        public string PromotionCode;

        [XmlAttribute("RateQualifier")]
        public string RateQualifier1;

        [XmlAttribute()]
        public wmCarAvail.RateQualifierRatePeriod RatePeriod;

        [XmlIgnore()]
        public bool RatePeriodSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class SpecialEquipPref
    {

        [XmlAttribute()]
        public string EquipType;

        [XmlAttribute()]
        public int Quantity;

        [XmlIgnore()]
        public bool QuantitySpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SpecialEquipPrefPreferLevel.Preferred)]
        public SpecialEquipPrefPreferLevel PreferLevel = SpecialEquipPrefPreferLevel.Preferred;
    }

    public enum SpecialEquipPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum VehAvailRQCoreStatus
    {

        Available,

        Unavailable,

        OnRequest,

        Confirmed,

        All
    }

    [XmlRoot(IsNullable = false)]
    public class VehAvailRQInfo
    {

        public Customer Customer;

        [XmlElement("SpecialReqPref")]
        public SpecialReqPref[] SpecialReqPref;

        [XmlArrayItem(IsNullable = false)]
        public CoveragePref[] CoveragePrefs;

        [XmlElement("OffLocService")]
        public wmCarAvail.OffLocService[] OffLocService;

        public ArrivalDetails ArrivalDetails;

        public wmCarAvail.TourInfo TourInfo;

        public TPA_Extensions TPA_Extensions;

        [XmlAttribute()]
        public int LuggageQty;

        [XmlIgnore()]
        public bool LuggageQtySpecified;

        [XmlAttribute()]
        public int PassengerQty;

        [XmlIgnore()]
        public bool PassengerQtySpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool SmokingAllowed = false;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool GasPrePay = false;

        [XmlAttribute()]
        public bool SingleQuote;

        [XmlIgnore()]
        public bool SingleQuoteSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class SpecialReqPref
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(SpecialReqPrefPreferLevel.Preferred)]
        public SpecialReqPrefPreferLevel PreferLevel = SpecialReqPrefPreferLevel.Preferred;

        [XmlText()]
        public string Value;
    }

    public enum SpecialReqPrefPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum OTA_VehAvailRateRQTarget
    {

        Test,

        Production
    }

    public enum OTA_VehAvailRateRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }

    [XmlRoot(IsNullable = false)]
    public class SpecialEquipPrefs
    {

        [XmlElement("SpecialEquipPref")]
        public SpecialEquipPref[] SpecialEquipPref;
    }

    [XmlRoot(IsNullable = false)]
    public class VehPrefs
    {

        [XmlElement("VehPref")]
        public VehPref[] VehPref;
    }

    [XmlRoot(IsNullable = false)]
    public class VendorPrefs
    {

        [XmlElement("VendorPref")]
        public VendorPref[] VendorPref;
    }
}
