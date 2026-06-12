using System.Xml.Serialization;

namespace wsTripXML.wsTravelTalk.wmSalesReportIn
{
    [XmlRoot(IsNullable = false)]
    public class SalesReportRQ
    {
        public POS POS;

        public string ReportType;

        public string ReportDate;

        public ReportDateRange ReportDateRange;

        public string PCC;
    }

    [XmlRoot(IsNullable = false)]
    public class ReportDateRange
    {
        [XmlAttribute()]
        public string Start;

        [XmlAttribute()]
        public string End;
    }
    [XmlRoot(IsNullable = false)]
    public class POS : Code.IPOS
    {
        public TPA_Extensions TPA_Extensions;
    }
    [XmlRoot(IsNullable = false)]
    public class TPA_Extensions : Code.ITPA_Extensions

    {
        public Code.Provider Provider;

    }
}