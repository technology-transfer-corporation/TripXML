using System;
using System.Runtime.Serialization;

namespace wsTripXML.wsTravelTalk.wmIssueMCOModels
{
    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    [Serializable]
    public class MCO
    {
        #region Request Fields
        public string ErrorMessage { get; set; }
        public string Amount { get; set; }
        public string PassengerNumber { get; set; }
        public string PaxNumber { get; set; }
        public string PassengerName { get; set; }
        public string PaxType { get; set; }
        public string TicketingAirlineCode { get; set; }
        public string To { get; set; }
        public string AT { get; set; }
        public string Tax { get; set; }
        public string TaxCode { get; set; }
        public string TypeOfService { get; set; }
        public int PQNumber { get; set; }
        public string TourNumber { get; set; }
        public string Endorsements { get; set; }
        public string Remarks { get; set; }
        public string TicketNumber { get; set; }
        public string CurrencyCode { get; set; }
        public string EquivelentAmount { get; set; }
        public string EquivCurrencyCode { get; set; }
        public string RateOfExchange { get; set; }
        public string CommissionPercentage { get; set; }
        public string CommissionAmount { get; set; }
        public bool InternationalItin { get; set; }

        // Form of payments
        public bool CASH { get; set; }
        public bool Check { get; set; }
        public bool Cheque { get; set; }
        public string CreditCard { get; set; }
        public string Expiration { get; set; }
        public string ApprovalCode { get; set; }
        public string Vendor { get; set; }
        #endregion
    }
    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    [KnownType(typeof(MCO))]
    [Serializable]
    public class MCOMask : MCO
    {
        public MCOMask()
        {
            ErrorMessage = string.Empty;
            AT = "NYC";
            StatementInformation = string.Empty;
            TourNumber = string.Empty;
            TicketNumber = string.Empty;
            Ignore = false;
            CurrencyCode = "USD";
            EquivelentAmount = string.Empty;
            EquivCurrencyCode = string.Empty;
            RateOfExchange = string.Empty;
            Total = string.Empty;
            CommissionPercentage = string.Empty;
            CommissionAmount = string.Empty;
            InternationalItin = false;
            CASH = false;
            Cheque = false;
            SelfSale = false;
            CreditCard = " ";
            Expiration = " ";
            ApprovalCode = " ";
            isAX = false;
            isSuppressed = false;
            SGR = " ";
            Other = " ";
            GTR = " ";
            IgnoreTJR = false;
            IgnoreCouponPrint = false;
            DisplayPrevMask = false;
            IssueDocuments = true;
            IsDone = false;
            Id = string.Empty;
        }

        #region MCO Creat Fields
        public string Total { get; set; }
        public string Id { get; set; }
        public string StatementInformation { get; set; }
        public bool Ignore { get; set; }
        public bool SelfSale { get; set; }
        public bool isAX { get; set; }
        public bool isSuppressed { get; set; }
        public string SGR { get; set; }
        public string Other { get; set; }
        public string GTR { get; set; }
        public bool IgnoreTJR { get; set; }
        public bool IgnoreCouponPrint { get; set; }
        public bool DisplayPrevMask { get; set; }
        public bool IssueDocuments { get; set; }
        public bool IsDone { get; set; }
        #endregion

        #region MCO Exchange Fields
        public string MCONumber { get; set; }
        public string ChangeFeeAmount { get; set; }
        public bool IsEMDFee { get; set; }
        public bool TaxComparison { get; set; }
        public string NewTktCommAmount { get; set; }
        public string AddCollCommAmount { get; set; }
        public string Waiver { get; set; }
        public bool IsModifyBAG { get; set; }
        public bool IsEndorsmentsUpdate { get; set; }
        public string ManualApproval { get; set; }
        public bool IsTicketing { get; set; }
        public bool IsRetain { get; set; }
        public bool IsPrevious { get; set; }
        public bool IsNext { get; set; }
        public bool IsQuit { get; set; }
        #endregion

    }

    // <remarks/>
    public enum TransactionStatusCode
    {

        // <remarks/>
        Start,

        // <remarks/>
        End,

        // <remarks/>
        Rollback,

        // <remarks/>
        InSeries
    }

    // <remarks/>
    public enum Target
    {
        // <remarks/>
        Test,
        // <remarks/>
        Production
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class POS
    {

        // <remarks/>
        public Source Source;

        // <remarks/>
        public TPA_Extensions TPA_Extensions;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class Source
    {

        // <remarks/>
        public RequestorID RequestorID;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string AgentSine;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string PseudoCityCode;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string ISOCountry;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string ISOCurrency;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string AgentDutyCode;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string AirlineVendorID;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string AirportCode;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string FirstDepartPoint;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string ERSP_UserID;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class RequestorID
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute(DataType = "anyURI")]
        public string URL;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Type;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string Instance;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string ID;

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string ID_Context;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class UniqueID
    {

        // <remarks/>
        [System.Xml.Serialization.XmlAttribute()]
        public string ID;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class Provider
    {

        // <remarks/>
        public string Name;

        // <remarks/>
        public string System;

        // <remarks/>
        public string Userid;

        // <remarks/>
        public string Password;
    }

    // <remarks/>
    [System.Xml.Serialization.XmlRoot(IsNullable = false)]
    public class Success
    {
    }
}