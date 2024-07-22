using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Galileo.Classes
{
    public class MCOFields
    {
        #region Declaration
        private string _request;
        private string _initrequest;
        public string ReadRQ { get; set; }
        public string CurrentRead { get; set; }
        public string EndTrans { get; set; }
        public string GetTickets { get; set; }
        public string Exchange { get; set; }
        public string MCODisplay { get; set; }
        public string IgnorePNR { get; set; }
        public XmlNodeList MCOs { get; set; }
        public MCOs initMCOs { get; set; }
        public string Error { get; set; }
        public string RL { get; set; }
        public string IATA { get; set; }

        private void Initialize()
        {
            ReadRQ = string.Empty;
            CurrentRead = string.Empty;
            EndTrans = string.Empty;
            GetTickets = string.Empty;
            Exchange = string.Empty;
            MCODisplay = string.Empty;
            IgnorePNR = string.Empty;
            Error = string.Empty;
            MCOs = null;
            initMCOs = null;
            RL = string.Empty;
            IATA = string.Empty;
        }
        #endregion

        public MCOFields(string request, string initrequest)
        {
            this._request = request;
            this._initrequest = initrequest;
            Initialize();
            GetInitialValues(request, initrequest);
        }

        private void GetInitialValues(string request, string initRequest)
        {
            try
            {
                var oDoc = new XmlDocument();
                oDoc.LoadXml(request);
                var oRoot = oDoc.DocumentElement;
                ReadRQ = oRoot.SelectSingleNode("PNRRead").InnerXml;
                CurrentRead = oRoot.SelectSingleNode("PNRCurrentRead").InnerXml;
                EndTrans = oRoot.SelectSingleNode("ET") != null ? oRoot.SelectSingleNode("ET").InnerXml : "";
                MCOs = oRoot.SelectSingleNode("MCOS/MiscellaneousChargeOrder_1_0") != null ? oRoot.SelectNodes("MCOS/MiscellaneousChargeOrder_1_0") : null;                
                MCODisplay = "<MiscellaneousChargeOrder_1_0><MCODisplayMods></MCODisplayMods></MiscellaneousChargeOrder_1_0>";
                GetTickets = oRoot.SelectSingleNode("GetTickets") != null ? oRoot.SelectSingleNode("GetTickets").InnerXml : "";
                Exchange = oRoot.SelectSingleNode("ExchangeMCO") != null ? oRoot.SelectSingleNode("ExchangeMCO").InnerXml : "";
                IgnorePNR = "<PNRBFManagement_53><IgnoreAndRedisplayMods></IgnoreAndRedisplayMods></PNRBFManagement_53>";
                RL = oRoot.SelectSingleNode("Config/RecordLocator") != null ? oRoot.SelectSingleNode("Config/RecordLocator").InnerText : "";
                IATA = oRoot.SelectSingleNode("Config/IATA") != null ? oRoot.SelectSingleNode("Config/IATA").InnerText : "";

                oDoc = new XmlDocument();
                oDoc.LoadXml(initRequest);
                oRoot = oDoc.DocumentElement;
                if(oRoot.SelectSingleNode("MCOs/MCOMask") != null )
                    using (StringReader reader = new StringReader(oRoot.SelectSingleNode("MCOs").OuterXml))
                    {
                        var oSerializer = new XmlSerializer(typeof(MCOs));
                        initMCOs = (MCOs)oSerializer.Deserialize(reader);
                    };
                
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
    }

    [XmlRoot(ElementName = "MCONumber")]
    public class MCONumber
    {

        [XmlElement(ElementName = "Num")]
        public int Num { get; set; }
    }

    [XmlRoot(ElementName = "MCOTicketData")]
    public class MCOTicketData
    {

        [XmlElement(ElementName = "TktIssueInd")]
        public string TktIssueInd { get; set; }
    }

    [XmlRoot(ElementName = "MCOIssue")]
    public class MCOIssue
    {

        [XmlElement(ElementName = "IssueInd")]
        public string IssueInd { get; set; }
    }

    [XmlRoot(ElementName = "MCOReasonCode")]
    public class MCOReasonCode
    {

        [XmlElement(ElementName = "ReasonCode")]
        public string ReasonCode { get; set; }
    }

    [XmlRoot(ElementName = "MCOMainData")]
    public class MCOMainData
    {

        [XmlElement(ElementName = "PsgrName")]
        public string PsgrName { get; set; }

        [XmlElement(ElementName = "TourOperator")]
        public string TourOperator { get; set; }

        [XmlElement(ElementName = "Location")]
        public string Location { get; set; }

        [XmlElement(ElementName = "ValidFor")]
        public string ValidFor { get; set; }

        [XmlElement(ElementName = "RelatedTktNum")]
        public object RelatedTktNum { get; set; }

        [XmlElement(ElementName = "Commission")]
        public double Commission { get; set; }

        [XmlElement(ElementName = "MCOTax")]
        public object MCOTax { get; set; }

        [XmlElement(ElementName = "TaxCode")]
        public object TaxCode { get; set; }

        [XmlElement(ElementName = "MCOAmt")]
        public double MCOAmt { get; set; }

        [XmlElement(ElementName = "Currency")]
        public string Currency { get; set; }

        [XmlElement(ElementName = "EquivFarePaid")]
        public object EquivFarePaid { get; set; }

        [XmlElement(ElementName = "EquivCurrency")]
        public object EquivCurrency { get; set; }

        [XmlElement(ElementName = "BankSellRate")]
        public object BankSellRate { get; set; }

        [XmlElement(ElementName = "PlatingCarrier")]
        public string PlatingCarrier { get; set; }
    }

    [XmlRoot(ElementName = "MCOIssueData")]
    public class MCOIssueData
    {

        [XmlElement(ElementName = "DocNum")]
        public double DocNum { get; set; }

        [XmlElement(ElementName = "TktIssueDt")]
        public String TktIssueDt { get; set; }
    }

    [XmlRoot(ElementName = "CreditCardFOP")]
    public class CreditCardFOP
    {
        [XmlElement(ElementName = "ID")]
        public int ID { get; set; }

        [XmlElement(ElementName = "Type")]
        public int Type { get; set; }

        [XmlElement(ElementName = "Currency")]
        public string Currency { get; set; }

        [XmlElement(ElementName = "Amt")]
        public int Amt { get; set; }

        [XmlElement(ElementName = "ExpDt")]
        public int ExpDt { get; set; }

        [XmlElement(ElementName = "TransType")]
        public string TransType { get; set; }

        [XmlElement(ElementName = "ApprovalInd")]
        public string ApprovalInd { get; set; }

        [XmlElement(ElementName = "AcceptOverride")]
        public string AcceptOverride { get; set; }

        [XmlElement(ElementName = "ValidationBypassReq")]
        public string ValidationBypassReq { get; set; }

        [XmlElement(ElementName = "Vnd")]
        public string Vnd { get; set; }

        [XmlElement(ElementName = "Acct")]
        public double Acct { get; set; }

        [XmlElement(ElementName = "AdditionalInfoAry")]
        public string AdditionalInfoAry { get; set; }
    }

    [XmlRoot(ElementName = "MCODisplay")]
    public class MCODisplay
    {
        [XmlElement(ElementName = "MCONumber")]
        public List<MCONumber> MCONumber { get; set; }

        [XmlElement(ElementName = "MCOTicketData")]
        public List<MCOTicketData> MCOTicketData { get; set; }

        [XmlElement(ElementName = "MCOIssue")]
        public List<MCOIssue> MCOIssue { get; set; }

        [XmlElement(ElementName = "MCOReasonCode")]
        public List<MCOReasonCode> MCOReasonCode { get; set; }

        [XmlElement(ElementName = "CreditCardFOP")]
        public List<CreditCardFOP> CreditCardFOP { get; set; }

        [XmlElement(ElementName = "MCOMainData")]
        public List<MCOMainData> MCOMainData { get; set; }

        [XmlElement(ElementName = "MCOIssueData")]
        public List<MCOIssueData> MCOIssueData { get; set; }

        public string Error { get; set; }

        private string strMCO { get; set; }

        public override string ToString() => strMCO;
        public MCODisplay()
        {
            strMCO = string.Empty;

            MCONumber = new List<MCONumber>();
            MCOTicketData = new List<MCOTicketData>();
            MCOIssue = new List<MCOIssue>();
            MCOReasonCode = new List<MCOReasonCode>();
            MCOMainData = new List<MCOMainData>();

        }
        public MCODisplay(string xml/*, bool isReload = false*/)
        {
            strMCO = xml.Replace("<MiscellaneousChargeOrder_1_0 xmlns=\"\">", "").Replace("</MiscellaneousChargeOrder_1_0>", "").Replace("<MiscellaneousChargeOrder_1_0 xmlns=''>", "");
            
            try
            {
                //if (isReload) {
                    using (StringReader reader = new StringReader(strMCO))
                    {
                        var oSerializer = new XmlSerializer(typeof(MCODisplay));
                        var _mco = (MCODisplay)oSerializer.Deserialize(reader);
                        MCONumber = _mco.MCONumber;
                        MCOTicketData = _mco.MCOTicketData;
                        MCOIssue = _mco.MCOIssue;
                        MCOReasonCode = _mco.MCOReasonCode;
                        MCOMainData = _mco.MCOMainData;
                        MCOIssueData = _mco.MCOIssueData;
                        CreditCardFOP = _mco.CreditCardFOP;
                    }
                    return;
                //}

                //var _lMCO = TryGetMCOs(strMCO);

                //if (_lMCO.Count > 0)
                //{
                //    MCONumber = _lMCO.Last().MCONumber;
                //    MCOTicketData = _lMCO.Last().MCOTicketData;
                //    MCOIssue = _lMCO.Last().MCOIssue;
                //    MCOReasonCode = _lMCO.Last().MCOReasonCode;
                //    MCOMainData = _lMCO.Last().MCOMainData;
                //    MCOIssueData = _lMCO.Last().MCOIssueData;
                //    CreditCardFOP = _lMCO.Last().CreditCardFOP;
                //}
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }


        //private List<MCODisplay> TryGetMCOs(string mcoDisplayRS)
        //{
        //    List<MCODisplay> _mcoDispList = new List<MCODisplay>();
        //    try
        //    {
                
        //        XmlDocument xDoc = new XmlDocument();
        //        xDoc.LoadXml(mcoDisplayRS);
        //        // получим корневой элемент
        //        XmlElement? xRoot = xDoc.DocumentElement;
        //        if (xRoot != null)
        //        {
        //            var _mcoSub = string.Empty;
        //            var i = 0;
        //            // обход всех узлов в корневом элементе
        //            foreach (XmlElement xnode in xRoot)
        //            {
        //                if (xnode.Name == "MCONumber")
        //                {
        //                    if (!string.IsNullOrEmpty(_mcoSub))
        //                    {
        //                        _mcoSub += $"</MCODisplay>";
        //                        _mcoDispList.Add(new MCODisplay(_mcoSub, true));                                
        //                    }
        //                    _mcoSub = $"<MCODisplay>";
        //                }
        //                _mcoSub += xnode.OuterXml;
                        
        //            }
        //            //Last time to add
        //            _mcoSub += $"</MCODisplay>";
        //            _mcoDispList.Add(new MCODisplay(_mcoSub, true));
        //            return _mcoDispList;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return _mcoDispList;
        //}

    }

    [XmlRoot(ElementName = "MCODisplayItem")]
    public class MCODisplayItem
    {
        [XmlElement(ElementName = "MCONumber")]
        public MCONumber MCONumber { get; set; }

        [XmlElement(ElementName = "MCOTicketData")]
        public MCOTicketData MCOTicketData { get; set; }

        [XmlElement(ElementName = "MCOIssue")]
        public MCOIssue MCOIssue { get; set; }

        [XmlElement(ElementName = "MCOReasonCode")]
        public MCOReasonCode MCOReasonCode { get; set; }

        [XmlElement(ElementName = "CreditCardFOP")]
        public CreditCardFOP CreditCardFOP { get; set; }

        [XmlElement(ElementName = "MCOMainData")]
        public MCOMainData MCOMainData { get; set; }

        [XmlElement(ElementName = "MCOIssueData")]
        public MCOIssueData MCOIssueData { get; set; }

        [XmlElement(ElementName = "IsVoided")]
        public bool IsVoided { get; set; }

        [XmlElement(ElementName = "Carrier")]
        public string Carrier { get; set; }

        public string Error { get; set; }

        public MCODisplayItem()
        {
            MCONumber = new MCONumber();
            MCOTicketData = new MCOTicketData();
            MCOIssue = new MCOIssue();
            MCOReasonCode = new MCOReasonCode();
            MCOMainData = new MCOMainData();
            MCOIssueData = new MCOIssueData();
        }
    }

    [XmlRoot(ElementName = "MCODisplayList")]
    public class MCODisplayList
    {
        [XmlElement(ElementName = "MCOs")]
        public List<MCODisplayItem> MCOs { get; set; }

        public MCODisplayList()
        {
            MCOs = new List<MCODisplayItem>();
        }

        public MCODisplayList(MCODisplay display)
        {
            try
            {
                MCOs = new List<MCODisplayItem>();

                foreach (var mcoN in display.MCONumber)
                {
                    var index = display.MCONumber.IndexOf(mcoN);

                    var _mco = new MCODisplayItem
                    {
                        MCONumber = mcoN,
                        MCOTicketData = display.MCOTicketData?[index] != null ? display.MCOTicketData[index] : new MCOTicketData(),
                        MCOIssue = display.MCOIssue?[index] != null ? display.MCOIssue[index] : new MCOIssue(),
                        MCOReasonCode = display.MCOReasonCode?[index] != null ? display.MCOReasonCode[index] : new MCOReasonCode(),
                        MCOMainData = display.MCOMainData?[index] != null ? display.MCOMainData[index] : new MCOMainData(),
                        CreditCardFOP = display.CreditCardFOP?[index] != null ? display.CreditCardFOP[index] : new CreditCardFOP(),

                        MCOIssueData = display.MCOIssueData != null
                            ? display.MCOIssueData.Count <= index ? new MCOIssueData() : display.MCOIssueData[index]
                            : new MCOIssueData()
                    };

                    MCOs.Add(_mco);
                }
            }
            catch (Exception ex)
            {
                MCOs.Add(new MCODisplayItem { Error = ex.Message });
            }
        }

        public void UpdateVoidedMCO(string pnr)
        {
            try
            {
                var oDoc = new XmlDocument();
                oDoc.LoadXml(pnr);
                var oRoot = oDoc.DocumentElement;
                var exchTicket = oRoot.SelectNodes("TravelItinerary/ItineraryInfo/TPA_Extensions/IssuedTickets/ExchangeDocument");
                var _carrirer = oRoot.SelectSingleNode("//ValidatingAirlineCode[1]").InnerText;
                var exMCO = MCOs.FindAll(mco => !string.IsNullOrEmpty(mco.MCOIssueData.TktIssueDt));

                foreach (XmlNode exch in exchTicket)
                {
                    exMCO.ForEach(mco => {
                        mco.Carrier = _carrirer;
                        if (exch.InnerText.Contains(mco.MCOIssueData.DocNum.ToString().Substring(0, 13)))
                            mco.IsVoided = exch.InnerText.Contains(mco.MCOIssueData.DocNum.ToString().Substring(0, 13)) && exch.InnerText.Contains("VOID");
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    [XmlRoot(ElementName = "MCOMask")]
    public class MCOMask
    {

        [XmlElement(ElementName = "ErrorMessage")]
        public string ErrorMessage { get; set; }

        [XmlElement(ElementName = "Amount")]
        public double Amount { get; set; }

        [XmlElement(ElementName = "PassengerNumber")]
        public string PassengerNumber { get; set; }

        [XmlElement(ElementName = "PaxNumber")]
        public string PaxNumber { get; set; }

        [XmlElement(ElementName = "PassengerName")]
        public string PassengerName { get; set; }

        [XmlElement(ElementName = "PaxType")]
        public string PaxType { get; set; }

        [XmlElement(ElementName = "TicketingAirlineCode")]
        public string TicketingAirlineCode { get; set; }

        [XmlElement(ElementName = "To")]
        public string To { get; set; }

        [XmlElement(ElementName = "AT")]
        public string AT { get; set; }

        [XmlElement(ElementName = "Tax")]
        public double Tax { get; set; }

        [XmlElement(ElementName = "TaxCode")]
        public string TaxCode { get; set; }

        [XmlElement(ElementName = "PQNumber")]
        public int PQNumber { get; set; }

        [XmlElement(ElementName = "TourNumber")]
        public string TourNumber { get; set; }

        [XmlElement(ElementName = "TicketNumber")]
        public string TicketNumber { get; set; }

        [XmlElement(ElementName = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [XmlElement(ElementName = "EquivelentAmount")]
        public double EquivelentAmount { get; set; }

        [XmlElement(ElementName = "EquivCurrencyCode")]
        public string EquivCurrencyCode { get; set; }

        [XmlElement(ElementName = "RateOfExchange")]
        public string RateOfExchange { get; set; }

        [XmlElement(ElementName = "CommissionPercentage")]
        public double CommissionPercentage { get; set; }

        [XmlElement(ElementName = "CommissionAmount")]
        public double CommissionAmount { get; set; }

        [XmlElement(ElementName = "InternationalItin")]
        public bool InternationalItin { get; set; }

        [XmlElement(ElementName = "CASH")]
        public bool CASH { get; set; }

        [XmlElement(ElementName = "Check")]
        public bool Check { get; set; }

        [XmlElement(ElementName = "Cheque")]
        public bool Cheque { get; set; }

        [XmlElement(ElementName = "CreditCard")]
        public double CreditCard { get; set; }

        [XmlElement(ElementName = "Expiration")]
        public int Expiration { get; set; }

        [XmlElement(ElementName = "ApprovalCode")]
        public string ApprovalCode { get; set; }

        [XmlElement(ElementName = "Vendor")]
        public string Vendor { get; set; }

        [XmlElement(ElementName = "Total")]
        public double Total { get; set; }

        [XmlElement(ElementName = "Id")]
        public int Id { get; set; }

        [XmlElement(ElementName = "StatementInformation")]
        public string StatementInformation { get; set; }

        [XmlElement(ElementName = "Ignore")]
        public bool Ignore { get; set; }

        [XmlElement(ElementName = "SelfSale")]
        public bool SelfSale { get; set; }

        [XmlElement(ElementName = "isAX")]
        public bool IsAX { get; set; }

        [XmlElement(ElementName = "isSuppressed")]
        public bool IsSuppressed { get; set; }

        [XmlElement(ElementName = "SGR")]
        public string SGR { get; set; }

        [XmlElement(ElementName = "Other")]
        public string Other { get; set; }

        [XmlElement(ElementName = "GTR")]
        public string GTR { get; set; }

        [XmlElement(ElementName = "IgnoreTJR")]
        public bool IgnoreTJR { get; set; }

        [XmlElement(ElementName = "IgnoreCouponPrint")]
        public bool IgnoreCouponPrint { get; set; }

        [XmlElement(ElementName = "DisplayPrevMask")]
        public bool DisplayPrevMask { get; set; }

        [XmlElement(ElementName = "IssueDocuments")]
        public bool IssueDocuments { get; set; }

        [XmlElement(ElementName = "IsDone")]
        public bool IsDone { get; set; }

        [XmlElement(ElementName = "IsEMDFee")]
        public bool IsEMDFee { get; set; }

        [XmlElement(ElementName = "TaxComparison")]
        public bool TaxComparison { get; set; }

        [XmlElement(ElementName = "AddCollCommAmount")]
        public double AddCollCommAmount { get; set; }

        [XmlElement(ElementName = "IsModifyBAG")]
        public bool IsModifyBAG { get; set; }

        [XmlElement(ElementName = "IsEndorsmentsUpdate")]
        public bool IsEndorsmentsUpdate { get; set; }

        [XmlElement(ElementName = "IsTicketing")]
        public bool IsTicketing { get; set; }

        [XmlElement(ElementName = "IsRetain")]
        public bool IsRetain { get; set; }

        [XmlElement(ElementName = "IsPrevious")]
        public bool IsPrevious { get; set; }

        [XmlElement(ElementName = "IsNext")]
        public bool IsNext { get; set; }

        [XmlElement(ElementName = "IsQuit")]
        public bool IsQuit { get; set; }
    }

    [XmlRoot(ElementName = "MCOs")]
    public class MCOs
    {

        [XmlElement(ElementName = "MCOMask")]
        public List<MCOMask> MCOMask { get; set; }
    }


}
