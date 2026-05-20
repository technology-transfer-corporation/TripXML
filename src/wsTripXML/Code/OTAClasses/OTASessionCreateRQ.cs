using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmSessionCreateIn
{
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }

    [XmlRoot(IsNullable = false)]
    public class Provider
    {

        public string Name;

        public string @System;

        public string Userid;

        public string Password;
    }

    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions
    {

        public Provider Provider;
    }

    [XmlRoot(IsNullable = false)]
    public class SessionCreateRQ
    {
        [XmlAttribute()]
        [System.ComponentModel.DefaultValue(Target.GAL)]
        public Target Target = Target.GAL;

        public POS POS;
    }

    public enum Target
    {

        GAL,
        WSP,
        APL
    }
}
