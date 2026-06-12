
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmInsuranceQuote
{

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Activities
    {

        // <remarks/>
        [XmlElement("Activity")]
        public string[] Activity;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class AdditionalPersonNames
    {

        // <remarks/>
        [XmlElement("AdditionalPersonName")]
        public string[] AdditionalPersonName;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class StreetNmbr
    {

        // <remarks/>
        [XmlAttribute()]
        public string PO_Box;

        // <remarks/>
        [XmlAttribute()]
        public string StreetNmbrSuffix;

        // <remarks/>
        [XmlAttribute()]
        public string StreetDirection;

        // <remarks/>
        [XmlAttribute()]
        public string RuralRouteNmbr;

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
    [XmlRoot(IsNullable = false)]
    public class CompanyName
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
    public class URL
    {

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        public string DefaultInd;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CitizenCountryName
    {

        // <remarks/>
        [XmlAttribute()]
        public string DefaultInd;

        // <remarks/>
        [XmlAttribute()]
        public string Code;
    }


    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CoverageRequirement
    {

        // <remarks/>
        public Deductible Deductible;

        // <remarks/>
        public PolicyLimit PolicyLimit;

        // <remarks/>
        public IndividualLimit IndividualLimit;

        // <remarks/>
        [XmlAttribute()]
        public string CoverageLevel;

        // <remarks/>
        [XmlAttribute()]
        public string CoverageType;

        // <remarks/>
        [XmlAttribute()]
        public string UnlimitedCoverage;

        // <remarks/>
        [XmlAttribute()]
        public string Covered;

        // <remarks/>
        [XmlAttribute()]
        public string EffectiveDate;

        // <remarks/>
        [XmlAttribute()]
        public string ExpireDate;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Deductible
    {

        // <remarks/>
        [XmlAttribute()]
        public string Amount;

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute()]
        public string DecimalPlaces;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PolicyLimit
    {

        // <remarks/>
        [XmlAttribute()]
        public string Amount;

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute()]
        public string DecimalPlaces;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class IndividualLimit
    {

        // <remarks/>
        [XmlAttribute()]
        public string Amount;

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute()]
        public string DecimalPlaces;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CoverageRequirements
    {

        // <remarks/>
        [XmlElement("CoverageRequirement")]
        public CoverageRequirement[] CoverageRequirement;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CoveredLuggage
    {

        // <remarks/>
        [XmlElement("LuggageItem")]
        public LuggageItem[] LuggageItem;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class LuggageItem
    {

        // <remarks/>
        public LuggageDescription LuggageDescription;

        // <remarks/>
        public ItemDeclaredValue ItemDeclaredValue;

        // <remarks/>
        public LuggagePremium LuggagePremium;

        // <remarks/>
        [XmlAttribute()]
        public string LuggageType;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class LuggageDescription
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
    public class ItemDeclaredValue
    {

        // <remarks/>
        [XmlAttribute()]
        public string Amount;

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute()]
        public string DecimalPlaces;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class LuggagePremium
    {

        // <remarks/>
        [XmlAttribute()]
        public string Amount;

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute()]
        public string DecimalPlaces;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class CoveredTrip
    {

        // <remarks/>
        [XmlArrayItem(IsNullable = false)]
        public Destination[] Destinations;

        // <remarks/>
        [XmlArrayItem("Activity", IsNullable = false)]
        public string[] Activities;

        // <remarks/>
        [XmlArrayItem(IsNullable = false)]
        public Operator[] Operators;

        // <remarks/>
        [XmlAttribute()]
        public string Start;

        // <remarks/>
        [XmlAttribute()]
        public string Duration;

        // <remarks/>
        [XmlAttribute()]
        public string End;

        // <remarks/>
        [XmlAttribute()]
        public string DepositDate;

        // <remarks/>
        [XmlAttribute()]
        public string FinalPayDate;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Destination
    {

        // <remarks/>
        public StreetNmbr StreetNmbr;

        // <remarks/>
        public string BldgRoom;

        // <remarks/>
        [XmlElement("AddressLine")]
        public string[] AddressLine;

        // <remarks/>
        public string CityName;

        // <remarks/>
        public string PostalCode;

        // <remarks/>
        public string County;

        // <remarks/>
        public StateProv StateProv;

        // <remarks/>
        public CountryName CountryName;

        // <remarks/>
        [XmlAttribute()]
        public string FormattedInd;

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        public string ArrivalDate;

        // <remarks/>
        [XmlAttribute()]
        public string DepartureDate;

        // <remarks/>
        [XmlAttribute()]
        public string AreaID;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Operator
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
    public class CoveredTrips
    {

        // <remarks/>
        [XmlElement("CoveredTrip")]
        public CoveredTrip[] CoveredTrip;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Destinations
    {

        // <remarks/>
        [XmlElement("Destination")]
        public Destination[] Destination;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class DeliveryPref
    {

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string PreferLevel;

        // <remarks/>
        [XmlAttribute()]
        public string DistribType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class DocHolderFormattedName
    {

        // <remarks/>
        [XmlElement("NamePrefix")]
        public string[] NamePrefix;

        // <remarks/>
        [XmlElement("GivenName")]
        public string[] GivenName;

        // <remarks/>
        [XmlElement("MiddleName")]
        public string[] MiddleName;

        // <remarks/>
        public string SurnamePrefix;

        // <remarks/>
        public string Surname;

        // <remarks/>
        [XmlElement("NameSuffix")]
        public string[] NameSuffix;

        // <remarks/>
        [XmlElement("NameTitle")]
        public string[] NameTitle;

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        // <remarks/>
        [XmlAttribute()]
        public string NameType;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Document
    {

        // <remarks/>
        public string DocHolderName;

        // <remarks/>
        public DocHolderFormattedName DocHolderFormattedName;

        // <remarks/>
        [XmlElement("DocLimitations")]
        public string[] DocLimitations;

        // <remarks/>
        [XmlArrayItem("AdditionalPersonName", IsNullable = false)]
        public string[] AdditionalPersonNames;

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        // <remarks/>
        [XmlAttribute()]
        public string DocIssueAuthority;

        // <remarks/>
        [XmlAttribute()]
        public string DocIssueLocation;

        // <remarks/>
        [XmlAttribute()]
        public string DocID;

        // <remarks/>
        [XmlAttribute()]
        public string DocType;

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string Gender;

        // <remarks/>
        [XmlAttribute()]
        public string BirthDate;

        // <remarks/>
        [XmlAttribute()]
        public string EffectiveDate;

        // <remarks/>
        [XmlAttribute()]
        public string ExpireDate;

        // <remarks/>
        [XmlAttribute()]
        public string DocIssueStateProv;

        // <remarks/>
        [XmlAttribute()]
        public string DocIssueCountry;
    }


    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Email
    {

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        // <remarks/>
        [XmlAttribute()]
        public string DefaultInd;

        // <remarks/>
        [XmlAttribute()]
        public string EmailType;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class EmployeeInfo
    {

        // <remarks/>
        [XmlAttribute()]
        public string EmployeeId;

        // <remarks/>
        [XmlAttribute()]
        public string EmployeeLevel;

        // <remarks/>
        [XmlAttribute()]
        public string EmployeeTitle;

        // <remarks/>
        [XmlAttribute()]
        public string EmployeeStatus;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class FlightAccidentAmount
    {

        // <remarks/>
        [XmlAttribute()]
        public string Amount;

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute()]
        public string DecimalPlaces;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PreexistingCondition
    {

        // <remarks/>
        [XmlAttribute()]
        public string Language;

        // <remarks/>
        [XmlAttribute()]
        public string DiagnosisDate;

        // <remarks/>
        [XmlAttribute()]
        public string LastTreatmentDate;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class IndCoverageReqs
    {

        // <remarks/>
        public IndTripCost IndTripCost;

        // <remarks/>
        public FlightAccidentAmount FlightAccidentAmount;

        // <remarks/>
        [XmlArrayItem(IsNullable = false)]
        public LuggageItem[] CoveredLuggage;

        // <remarks/>
        [XmlArrayItem(IsNullable = false)]
        public PreexistingCondition[] PreexistingConditions;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class IndTripCost
    {

        // <remarks/>
        [XmlAttribute()]
        public string Amount;

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute()]
        public string DecimalPlaces;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class InsCoverageDetail
    {

        // <remarks/>
        [XmlArrayItem(IsNullable = false)]
        public CoverageRequirement[] CoverageRequirements;

        // <remarks/>
        public TotalTripQuantity TotalTripQuantity;

        // <remarks/>
        public MaximumTripLength MaximumTripLength;

        // <remarks/>
        public TotalTripCost TotalTripCost;

        // <remarks/>
        [XmlArrayItem(IsNullable = false)]
        public CoveredTrip[] CoveredTrips;

        // <remarks/>
        [XmlElement("DeliveryPref")]
        public DeliveryPref[] DeliveryPref;

        // <remarks/>
        [XmlAttribute()]
        public string EffectiveDate;

        // <remarks/>
        [XmlAttribute()]
        public string ExpireDate;

        // <remarks/>
        [XmlAttribute()]
        public string Type;

        // <remarks/>
        [XmlAttribute()]
        public string AutoRenew;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TotalTripQuantity
    {

        // <remarks/>
        [XmlAttribute()]
        public string Quantity;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class MaximumTripLength
    {

        // <remarks/>
        [XmlAttribute()]
        public string Minimum;

        // <remarks/>
        [XmlAttribute()]
        public string Maximum;

        // <remarks/>
        [XmlText()]
        public string Value;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class TotalTripCost
    {

        // <remarks/>
        [XmlAttribute()]
        public string Amount;

        // <remarks/>
        [XmlAttribute()]
        public string CurrencyCode;

        // <remarks/>
        [XmlAttribute()]
        public string DecimalPlaces;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class Operators
    {

        // <remarks/>
        [XmlElement("Operator")]
        public Operator[] Operator;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PreexistingConditions
    {

        // <remarks/>
        [XmlElement("PreexistingCondition")]
        public PreexistingCondition[] PreexistingCondition;
    }

    // <remarks/>
    [XmlRoot(IsNullable = false)]
    public class PersonName
    {

        // <remarks/>
        [XmlElement("NamePrefix")]
        public string[] NamePrefix;

        // <remarks/>
        [XmlElement("GivenName")]
        public string[] GivenName;

        // <remarks/>
        [XmlElement("MiddleName")]
        public string[] MiddleName;

        // <remarks/>
        public string SurnamePrefix;

        // <remarks/>
        public string Surname;

        // <remarks/>
        [XmlElement("NameSuffix")]
        public string[] NameSuffix;

        // <remarks/>
        [XmlElement("NameTitle")]
        public string[] NameTitle;

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareSynchInd;

        // <remarks/>
        [XmlAttribute(DataType = "NMTOKEN")]
        public string ShareMarketInd;

        // <remarks/>
        [XmlAttribute()]
        public string NameType;
    }




}