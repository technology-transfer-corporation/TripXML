using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmShowMileageIn
{

    [XmlRoot(IsNullable = false)]
    public class OTA_ShowMileageRQ
    {

        public POS POS;

        public string FromCity;

        [XmlElement("ToCity")]
        public string[] ToCity;

    }

    [XmlRoot(IsNullable = false)]
    public class OTA_ShowMilesRQ
    {

        public POS POS;

        [XmlElement("CarrierCode")]
        public string CarrierCode;

        [XmlElement("FlightNummber")]
        public string FlightNummber;

        [XmlElement("TravelDate")]
        public string TravelDate;

    }
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
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
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;

        //public string ConversationID;
    }
}
