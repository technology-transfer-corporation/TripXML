using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCurConvIn
{

    [XmlRoot(IsNullable = false)]
    public class CurrencyRequest
    {

        public string Amount;

        public string FromCurrencyCode;

        public string ToCurrencyCode;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_CurConvRQ
    {

        public POS POS;

        [XmlElement("CurrencyRequest")]
        public CurrencyRequest[] CurrencyRequest;
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

        //public string ConversationID;
    }

    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        public string Name;

        public string System;

        public string Userid;

        public string Password;
    }
}
