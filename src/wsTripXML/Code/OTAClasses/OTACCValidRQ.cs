using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmCCValidIn
{

    [XmlRoot(IsNullable = false)]
    public class Authorization
    {

        public string Amount;

        public string CurrencyCode;

        public string CarrierCode;
    }

    [XmlRoot(IsNullable = false)]
    public class CreditCard
    {

        public string Code;

        public string Number;

        public Expiration Expiration;

        public Authorization Authorization;
    }

    [XmlRoot(IsNullable = false)]
    public class Expiration
    {

        public string Month;

        public string Year;
    }

    [XmlRoot(IsNullable = false)]
    public class OTA_CCValidRQ
    {

        public POS POS;

        [XmlElement("CreditCard")]
        public CreditCard[] CreditCard;
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
