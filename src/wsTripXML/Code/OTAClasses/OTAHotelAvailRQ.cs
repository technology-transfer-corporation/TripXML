using System;
using System.Xml.Serialization;
using wsTripXML.wsTravelTalk.wmHotelAvail;

namespace wsTripXML.wsTravelTalk.wmHotelAvailIn
{

    [XmlRoot(IsNullable = false)]
    public class AvailRequestSegment
    {

        public StayDateRange StayDateRange;

        public RateRange RateRange;

        [XmlArrayItem(IsNullable = false)]
        public RatePlanCandidate[] RatePlanCandidates;

        [XmlArrayItem(IsNullable = false)]
        public wmHotelAvail.ProfileInfo[] Profiles;

        [XmlArrayItem(IsNullable = false)]
        public RoomStayCandidate[] RoomStayCandidates;

        [XmlArrayItem(IsNullable = false)]
        public Criterion[] HotelSearchCriteria;

        [XmlAttribute()]
        public AvailRequestSegmentAvailReqType AvailReqType;

        [XmlIgnore()]
        public bool AvailReqTypeSpecified;

        [XmlAttribute()]
        public string MoreDataEchoToken;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(AvailRequestSegmentResponseType.PropertyList)]
        public AvailRequestSegmentResponseType ResponseType = AvailRequestSegmentResponseType.PropertyList;
    }

    [XmlRoot(IsNullable = false)]
    public class StayDateRange
    {

        public DateWindowRange DateWindowRange;

        [XmlAttribute(DataType = "date")]
        public DateTime Start;

        [XmlIgnore()]
        public bool StartSpecified;

        [XmlAttribute()]
        public string Duration;

        [XmlAttribute(DataType = "date")]
        public DateTime End;

        [XmlIgnore()]
        public bool EndSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class RateRange
    {

        [XmlAttribute()]
        public string CurrencyCode;

        [XmlAttribute()]
        public double MinRate;

        [XmlIgnore()]
        public bool MinRateSpecified;

        [XmlAttribute()]
        public double MaxRate;

        [XmlIgnore()]
        public bool MaxRateSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class RatePlanCandidate
    {

        [XmlAttribute()]
        public string RatePlanType;

        [XmlAttribute()]
        public string RatePlanCode;

        [XmlAttribute()]
        public string RatePlanID;

        [XmlAttribute()]
        public bool RatePlanQualifier;

        [XmlIgnore()]
        public bool RatePlanQualifierSpecified;

        [XmlAttribute()]
        public string PromotionCode;
    }

    [XmlRoot(IsNullable = false)]
    public class RoomStayCandidate
    {

        public GuestCounts GuestCounts;

        [XmlElement("RoomAmenity")]
        public RoomAmenity[] RoomAmenity;

        [XmlAttribute()]
        public string RoomType;

        [XmlAttribute()]
        public string RoomTypeCode;

        [XmlAttribute()]
        public string RoomCategory;

        [XmlAttribute()]
        public string RoomID;

        [XmlAttribute()]
        public int Floor;

        [XmlIgnore()]
        public bool FloorSpecified;

        [XmlAttribute()]
        public string InvBlockCode;

        [XmlAttribute()]
        public string PromotionCode;

        [XmlAttribute()]
        public string RoomLocationCode;

        [XmlAttribute()]
        public string RoomViewCode;

        [XmlAttribute()]
        public string BedTypeCode;

        [XmlAttribute()]
        public bool NonSmoking;

        [XmlIgnore()]
        public bool NonSmokingSpecified;

        [XmlAttribute()]
        public string Configuration;

        [XmlAttribute()]
        public string SizeMeasurement;

        [XmlAttribute()]
        public int Quantity;

        [XmlIgnore()]
        public bool QuantitySpecified;

        [XmlAttribute()]
        public bool Composite;

        [XmlIgnore()]
        public bool CompositeSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class RoomAmenity
    {

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(RoomAmenityPreferLevel.Preferred)]
        public RoomAmenityPreferLevel PreferLevel = RoomAmenityPreferLevel.Preferred;

        [XmlAttribute("RoomAmenity")]
        public string RoomAmenity1;

        [XmlAttribute()]
        public int Quantity;

        [XmlIgnore()]
        public bool QuantitySpecified;

        [XmlText()]
        public string Value;
    }

    public enum RoomAmenityPreferLevel
    {

        Only,

        Unacceptable,

        Preferred
    }

    public enum AvailRequestSegmentAvailReqType
    {

        Room,

        NonRoom,

        Both,

        PricingDetails
    }

    public enum AvailRequestSegmentResponseType
    {

        PropertyList,

        AreaList
    }

    [XmlRoot(IsNullable = false)]
    public class AvailRequestSegments
    {

        [XmlElement("AvailRequestSegment")]
        public AvailRequestSegment[] AvailRequestSegment;
    }

    [XmlRoot(IsNullable = false)]
    public class BookingChannel
    {

        public Code.CompanyName CompanyName;

        [XmlAttribute()]
        public string Type;

        [XmlAttribute()]
        public bool Primary;

        [XmlIgnore()]
        public bool PrimarySpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class HotelReservationID
    {

        [XmlAttribute()]
        public string ResID_Type;

        [XmlAttribute()]
        public string ResID_Value;

        [XmlAttribute()]
        public string ResID_Source;

        [XmlAttribute()]
        public string ResID_SourceContext;

        [XmlAttribute()]
        public DateTime ResID_Date;

        [XmlIgnore()]
        public bool ResID_DateSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool ForGuest = false;

        [XmlAttribute()]
        public string ResGuestRPH;

        [XmlAttribute()]
        public string CancelOriginatorCode;

        [XmlAttribute()]
        public DateTime CancellationDate;

        [XmlIgnore()]
        public bool CancellationDateSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class HotelReservationIDs
    {

        [XmlElement("HotelReservationID")]
        public HotelReservationID[] HotelReservationID;
    }

    [XmlRoot(IsNullable = false)]
    public class HotelSearchCriteria
    {

        [XmlElement("Criterion")]
        public Criterion[] Criterion;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_HotelAvailRQ
    {

        public POS POS;

        [XmlArrayItem(IsNullable = false)]
        public AvailRequestSegment[] AvailRequestSegments;

        [XmlArrayItem(IsNullable = false)]
        public HotelReservationID[] HotelReservationIDs;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_HotelAvailRQTarget.Production)]
        public OTA_HotelAvailRQTarget Target = OTA_HotelAvailRQTarget.Production;

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
        public OTA_HotelAvailRQTransactionStatusCode TransactionStatusCode;

        [XmlIgnore()]
        public bool TransactionStatusCodeSpecified;

        [XmlAttribute()]
        public string PrimaryLangID;

        [XmlAttribute()]
        public string AltLangID;

        [XmlAttribute()]
        public bool SummaryOnly;

        [XmlIgnore()]
        public bool SummaryOnlySpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_HotelAvailRQSortOrder.A)]
        public OTA_HotelAvailRQSortOrder SortOrder = OTA_HotelAvailRQSortOrder.A;

        [XmlAttribute()]
        public bool AvailRatesOnly;

        [XmlIgnore()]
        public bool AvailRatesOnlySpecified;

        [XmlAttribute()]
        public bool BestOnly;

        [XmlIgnore()]
        public bool BestOnlySpecified;

        [XmlAttribute()]
        public bool RateRangeOnly;

        [XmlIgnore()]
        public bool RateRangeOnlySpecified;

        [XmlAttribute()]
        public bool ExactMatchOnly;

        [XmlIgnore()]
        public bool ExactMatchOnlySpecified;

        [XmlAttribute()]
        public bool AllowPartialAvail;

        [XmlIgnore()]
        public bool AllowPartialAvailSpecified;

        [XmlAttribute()]
        public string RequestedCurrency;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(false)]
        public bool IsModify = false;

        [XmlAttribute()]
        public int MaxResponses;

        [XmlIgnore()]
        public bool MaxResponsesSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public wmHotelAvail.TPA_Extensions TPA_Extensions;
    }

    public enum OTA_HotelAvailRQTarget
    {

        Test,

        Production
    }

    public enum OTA_HotelAvailRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }

    public enum OTA_HotelAvailRQSortOrder
    {

        A,

        D,

        N
    }

    [XmlRoot(IsNullable = false)]
    public class RatePlanCandidates
    {

        [XmlElement("RatePlanCandidate")]
        public RatePlanCandidate[] RatePlanCandidate;
    }

    [XmlRoot(IsNullable = false)]
    public class RoomStayCandidates
    {

        [XmlElement("RoomStayCandidate")]
        public RoomStayCandidate[] RoomStayCandidate;
    }

}
