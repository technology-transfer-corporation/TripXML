using System;
using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmHotelInfoIn
{

    [XmlRoot(IsNullable = false)]
    public class AffiliationInfo
    {

        [XmlAttribute()]
        public bool SendDistribSystems;

        [XmlIgnore()]
        public bool SendDistribSystemsSpecified;

        [XmlAttribute()]
        public bool SendBrands;

        [XmlIgnore()]
        public bool SendBrandsSpecified;

        [XmlAttribute()]
        public bool SendLoyalPrograms;

        [XmlIgnore()]
        public bool SendLoyalProgramsSpecified;

        [XmlAttribute()]
        public bool SendAwards;

        [XmlIgnore()]
        public bool SendAwardsSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class AreaInfo
    {

        [XmlAttribute()]
        public bool SendRefPoints;

        [XmlIgnore()]
        public bool SendRefPointsSpecified;

        [XmlAttribute()]
        public bool SendAttractions;

        [XmlIgnore()]
        public bool SendAttractionsSpecified;

        [XmlAttribute()]
        public bool SendRecreations;

        [XmlIgnore()]
        public bool SendRecreationsSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class ContactInfo
    {

        [XmlAttribute()]
        public bool SendData;

        [XmlIgnore()]
        public bool SendDataSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class FacilityInfo
    {

        [XmlAttribute()]
        public bool SendMeetingRooms;

        [XmlIgnore()]
        public bool SendMeetingRoomsSpecified;

        [XmlAttribute()]
        public bool SendGuestRooms;

        [XmlIgnore()]
        public bool SendGuestRoomsSpecified;

        [XmlAttribute()]
        public bool SendRestaurants;

        [XmlIgnore()]
        public bool SendRestaurantsSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class HotelDescriptiveInfo
    {

        public HotelInfo HotelInfo;

        public FacilityInfo FacilityInfo;

        public Policies Policies;

        public AreaInfo AreaInfo;

        public AffiliationInfo AffiliationInfo;

        public ContactInfo ContactInfo;

        public MultimediaObjects MultimediaObjects;

        public ContentInfos ContentInfos;

        [XmlAttribute()]
        public string ChainCode;

        [XmlAttribute()]
        public string BrandCode;

        [XmlAttribute()]
        public string HotelCode;

        [XmlAttribute()]
        public string HotelCityCode;

        [XmlAttribute()]
        public string HotelName;

        [XmlAttribute()]
        public string HotelCodeContext;

        [XmlAttribute()]
        public string ChainName;

        [XmlAttribute()]
        public string BrandName;

        [XmlAttribute()]
        public string AreaID;

        [XmlAttribute()]
        public string StateCodeList;

        [XmlAttribute()]
        public string CountryCodeList;

        [XmlAttribute()]
        public string BrandCodeList;
    }

    [XmlRoot(IsNullable = false)]
    public class ContentInfos
    {

        [XmlElement("ContentInfo")]
        public ContentInfo[] ContentInfo;

    }

    [XmlRoot(IsNullable = false)]
    public class ContentInfo
    {

        [XmlAttribute()]
        public NameRQ Name;

    }

    public enum NameRQ
    {

        SecureMultimediaURLs

    }

    [XmlRoot(IsNullable = false)]
    public class HotelInfo
    {

        [XmlAttribute()]
        public bool SendData;

        [XmlIgnore()]
        public bool SendDataSpecified;

        [XmlAttribute()]
        public string HotelStatus;

        [XmlAttribute()]
        public string HotelStatusCode;
    }

    [XmlRoot(IsNullable = false)]
    public class Policies
    {

        [XmlAttribute()]
        public bool SendPolicies;

        [XmlIgnore()]
        public bool SendPoliciesSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class MultimediaObjects
    {

        [XmlAttribute()]
        public bool SendData;

        [XmlIgnore()]
        public bool SendDataSpecified;
    }

    [XmlRoot(IsNullable = false)]
    public class HotelDescriptiveInfos
    {

        [XmlElement("HotelDescriptiveInfo")]
        public HotelDescriptiveInfo[] HotelDescriptiveInfo;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_HotelDescriptiveInfoRQ
    {

        public POS POS;

        [XmlArrayItem(IsNullable = false)]
        public HotelDescriptiveInfo[] HotelDescriptiveInfos;

        [XmlAttribute()]
        public string EchoToken;

        [XmlAttribute()]
        public DateTime TimeStamp;

        [XmlIgnore()]
        public bool TimeStampSpecified;

        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(OTA_HotelDescriptiveInfoRQTarget.Production)]
        public OTA_HotelDescriptiveInfoRQTarget Target = OTA_HotelDescriptiveInfoRQTarget.Production;

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
        public OTA_HotelDescriptiveInfoRQTransactionStatusCode TransactionStatusCode;

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

    public enum OTA_HotelDescriptiveInfoRQTarget
    {

        Test,

        Production
    }

    public enum OTA_HotelDescriptiveInfoRQTransactionStatusCode
    {

        Start,

        End,

        Rollback,

        InSeries
    }
}
