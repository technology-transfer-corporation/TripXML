using System.Xml;
using TripXMLMain;
using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace Travelport
{
    public class AirServices
    {

        private StringBuilder sb = new StringBuilder();
        public modCore.TripXMLProviderSystems ProviderSystems;
        private string mstrVersion = "";
        private string mstrXslPath = "";
        public static int RequestCount;
        public static string ErrorReq = "";
        private int iFinishedPrices;
        private string[,] priceTags;
        private string[,] PricingSource;
        private string[,] NegoCode;
        private string[,] TicketTimeLimit;
        private string[,] OutClass;
        private string[,] InClass;
        private string[,] flSegments;
        private int iFareSearches;
        private string[] OTAPriceTotal;
        private string[] OTAReqTotal;
        private string[] avTempResponseTotal;
        private string strRequestTotal = "";
        string[] strReq;
        bool[] bPriceEnd;
        private string strMessage = "";
        string _tracerID = "";

        public string Request { get; set; } = "";

        public string Version
        {
            get
            {
                return mstrVersion;
            }
            set
            {
                mstrVersion = value;
                if ((mstrVersion.Length > 0))
                {
                    mstrVersion = mstrVersion + "_";
                }
            }
        }

        public string XslPath
        {
            get
            {
                return mstrXslPath;
            }
            set
            {
                var sbx = new StringBuilder();
                sbx.Append(value).Append("AmadeusWS\\");
                mstrXslPath = sbx.ToString();
            }
        }

        public string XslPortalPath
        {
            get
            {
                return mstrXslPath.Replace("AmadeusWS", "Portal");
            }
            set
            {
                StringBuilder sbx = new StringBuilder();
                sbx.Append(value).Append("Portal\\");
                mstrXslPath = sbx.ToString();
                sbx = null;
            }
        }

        public string AirAvail()
        {
            var sb1 = new StringBuilder();
            TravelPortWSAdapter ttAA = null;

            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlDocument oDocResp = null;
            XmlElement oRootResp = null;
            XmlNodeList oNodeResp = null;
            XmlDocument oDocRespN = null;
            XmlElement oRootRespN = null;
            XmlNodeList oNodeRespN = null;

            string response = null;
            string Pages = null;
            string ConversationID = "";
            string strRequest = "";
            string strResponse = "";
            string strIniAirReply = "";
            string strNxtAirReply = "";
            string strNxtFlightAirReply = null;
            string strIniFlightAirReply = null;
            int count = 0;




            return strResponse;
        }

        public string AirFlifo()
        {
            string strResponse = "";
            string strRequest;
            
            return strResponse;
        }

        public string AirPrice()
        {
            TravelPortWSAdapter ttAA = null;
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            string ConversationID = "";
            string strRequest = "";
            string strResponse = "";
            string strPNRReply = "";
            DateTime RequestTime;
            DateTime ResponseTime;
            string strMessage = "";
            bool returnBreakPoint = false;
           
            return strResponse;
        }

        public string AirRules()
        {
            TravelPortWSAdapter ttAA = null;
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlDocument oDocResp = null;
            XmlElement oRootResp = null;
            XmlNodeList oNodeRespList = null;
            XmlNodeList oNodePaxList = null;
            string ConversationID = "";
            string strRequest = "";
            string strRuleReqInfo = "";
            string strResponse = "";
            string Categories = "";
            string FareBasisCode = "";
            DateTime RequestTime;
            DateTime ResponseTime;
            string strMessage = "";
            string strDepDates = "";
            return strResponse;
        }

        public string AirSeatMap()
        {
            TravelPortWSAdapter ttAA = null;
            string strRequest = "";
            string strResponse = "";
            return strResponse;
        }

        public string FareDisplay()
        {
            TravelPortWSAdapter ttAA = null;
            string strRequest = "";
            string strResponse = "";
            return strResponse;
        }

        public string LowFare()
        {
            TravelPortWSAdapter ttAA = null;
            string strRequest = "";
            string strResponse = "";
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlNode oBLNode = null;
            DateTime RequestTime;
            DateTime ResponseTime;
            string strMessage = "";
            return strResponse;
        }

        public string LowFarePlus()
        {
            TravelPortWSAdapter ttAA = null;
            string strRequest = "";
            string strResponse = "";
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlNode oBLNode = null;
            DateTime RequestTime;
            DateTime ResponseTime;
            string strMessage = "";
            StringBuilder sb = new StringBuilder();
            return strResponse;
        }

        public string LowFareMatrix()
        {
            TravelPortWSAdapter ttAA = null;
            string strRequest = "";
            string strResponse = "";
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlNode oBLNode = null;
            DateTime RequestTime;
            DateTime ResponseTime;
            string strMessage = "";
            StringBuilder sb = new StringBuilder();
            return strResponse;
        }

        public string LowOfferMatrix()
        {
            TravelPortWSAdapter ttAA = null;
            string strRequest;
            string strResponse = "";
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlNode oBLNode;
            DateTime RequestTime;
            DateTime ResponseTime;
            string strMessage = "";
            var sb = new StringBuilder();
            return strResponse;
        }

        public string LowOfferSearch()
        {
            TravelPortWSAdapter ttAA = null;
            string strRequest = "";
            string strResponse = "";
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlNode oBLNode = null;
            DateTime RequestTime;
            DateTime ResponseTime;
            string strMessage = "";
            StringBuilder sb = new StringBuilder();
            // ************************************************************
            //  Get the Filtering Elements from OTA LowOfferSearch Request   *
            // ************************************************************
            return strResponse;
        }

        public string LowFareFlights()
        {
            TravelPortWSAdapter ttAA = null;
            string strRequest = "";
            string strSecondRequest = "";
            string strResponse = "";
            string strNewResponse = "";
            string strFirstResponse = "";
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlDocument oDocResp = null;
            XmlElement oRootResp = null;
            XmlElement oRootNewResp = null;
            XmlElement oRootFinal = null;
            XmlNode oBLNode = null;
            string strMessage = "";
            StringBuilder sb = new StringBuilder();
            string strAvailResponses = "";
            string ConversationID = "";
            string strFirstFlight = "";
            string strFinalResponse = "";
            XmlDocument oDocIP = null;
            XmlElement oRootIP = null;
            XmlNode oNodeIPMG = null;
            int count = 0;
            int NCount = 0;
            int FilterCount = 0;
            int RNCount = 0;
            List<string> AClasses = new List<string>();
            List<string> FiltClasses = new List<string>();
            List<string> IndexFiltClass = new List<string>();
            string PricedItinerary = "";
            int iNIP = 0;
            bool isNotEnough = false;
            string FlexToken = "";
            string EchoToken = "";
            string avTempResponse = "";
            string avFinalResponse = "";
            string avFinalRequest = "";
            int outboundcnt = 5;
            int inboundcnt = 5;
            string OTAPrice = "";
            string firstPrice = "";
            int outBoundCntTemp, inBoundCntTemp;
            List<XmlNode> fareBasisCodes = new List<XmlNode>();

            XmlDocument oPDoc = null;
            XmlElement oPRoot = null;
            XmlDocument oPriceDoc = null;
            XmlElement oPriceRoot = null;
            XmlNode oPNode = null;
            string cabinpref = "Economy";
            bool isReturn = true;
            int tempNoOfFlights = 0;
            // ************************************************************
            //  Get the Filtering Elements from OTA LowFareFlights Request   *
            // ************************************************************
            return strResponse;
        }

        public string LowFareSchedule()
        {
            TravelPortWSAdapter ttAA = null;
            string strRequest = "";
            string strResponse = "";
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            DateTime RequestTime;
            DateTime ResponseTime;
            string strMessage = "";
            StringBuilder sb = new StringBuilder();


            // ************************************************************
            //  Get the Filtering Elements from OTA LowFareSchedule Request   *
            // ************************************************************
            return strResponse;
        }

        public string FareInfo()
        {
            TravelPortWSAdapter ttAA = null;
            string strRequest = "";
            string strResponse = "";
            string strRespNative = "";
            string[] AirFareInfoRS = null;
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlDocument oDocResp = null;
            XmlElement oRootResp = null;
            XmlDocument oDocNative = null;
            XmlElement oRootNative = null;
            // XmlNode oNodeNative = null;
            string ConversationID = "";
            string currency = "";
            string DepartureDate = "";
            string ArrivalDate = "";
            string DepartureLocation = "";
            string ArrivalLocation = "";
            string AirlineCode = "";
            string TicketDate = "";
            int i = 0;
            StringBuilder sb2 = null;

            return strResponse;
        }

        public string AirSchedule()
        {
            TravelPortWSAdapter ttAA = null;
            string strRequest = "";
            string strResponse = "";
            
            return strResponse;
        }

        public string BusinessLogic(string strResponse, string strBusiness, string xslPath, string xslFile)
        {
            StringBuilder sb1 = new StringBuilder();
            if (strResponse.IndexOf("<Success />") != -1 || strResponse.IndexOf("<Success></Success>") != -1)
            {
                sb1.Append(strBusiness).Append("<Success />");
                strResponse = strResponse.Replace("<Success />", sb1.ToString());
                strResponse = strResponse.Replace("<Success/>", sb1.ToString());
                sb1.Remove(0, sb.Length);
                sb1.Append(strBusiness).Append("<Success></Success>");
                strResponse = strResponse.Replace("<Success></Success>", sb1.ToString());
                sb1.Remove(0, sb.Length);
                strResponse = CoreLib.TransformXML(strResponse, xslPath, (mstrVersion + xslFile), false);
            }
            return strResponse;
        }

        //public string GetDecodeValue(ref DataView oDV, ref string strCode)
        // {
        //     int i;
        //     i = oDV.Find(strCode);
        //     if (i > -1)
        //     {
        //         return oDV.Item[i].Item["Name"].ToString;
        //     }
        //     else
        //     {
        //         return "";
        //     }
        // }

        private string ParseAirFareInfoRS(string pCurrency, string pDepartureDate, string pArrivalDate, string pDepLocation, string pArrLocation, string pAirline, string pTicketDate, string pResponseXML, int fareRPH, string paxType)
        {
            // **************************
            // * Variable definition    *
            // **************************
            string OTAResponseXML = "";
            string strError = "";
            string[] AirFareInfo = null;
            int intCount = 0;
            int intChars = 0;
            string BaseFare = "";
            string TaxAmount = "";
            string TotalFare = "";
            ArrayList Amounts = new ArrayList();
            int DashCount = 0;
            int NumDecimals = 0;
            int TotalTax = 0;
            int i = 0;
            int x = 0;
            int z = 0;
            int y = 0;
            string strTemp = "";

            sb.Remove(0, sb.Length);
            
            return OTAResponseXML;
        }

        private bool AllDigits(string txt)
        {
            string ch;
            int i;
            bool allDigits = true;

            for (i = 1; (i <= txt.Length); i++)
            {
                //  See if the next character is a non-digit.
                ch = txt.Substring((i - 1), 1);
                if ((string.Compare(ch, "0") < 0 || string.Compare(ch, "9") > 0) && (ch != "."))
                {
                    //  This is not a digit.
                    allDigits = false;
                    break;
                }
            }

            return allDigits;
        }

        private string parseItin(string itin, string request)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNodeList oNodeSegList = null;
            XmlNode oNodeNew = null;
            XmlNode node = null;
            string strTemp = "";
            string strValue = "";
            XmlAttribute Attribute = null;

            oDoc = new XmlDocument();
            oDoc.LoadXml(request);
            oRoot = oDoc.DocumentElement;
            itin = itin.Substring(5);
            oNodeSegList = oRoot.SelectNodes("OriginDestinationInformation/FlightSegment");

            foreach (XmlNode oNode in oNodeSegList)
            {
                strTemp = oNode.SelectSingleNode("ArrivalAirport/@LocationCode").InnerText;
                // i = itin.IndexOf(strTemp & " ")
                // If i <> -1 Then
                //     itin = itin.Substring(i - 1)
                // End If
                if ((itin.IndexOf("S U R F A C E") == 10))
                {
                    itin = itin.Substring(24);
                    // Continue For
                }
                //  class of service
                strValue = itin.Substring(14, 1);
                node = oNode.SelectSingleNode("MarketingAirline");
                Attribute = oDoc.CreateAttribute("ClassOfService");
                Attribute.Value = strValue;
                node.Attributes.Append(Attribute);
                //  fare basis code
                strValue = itin.Substring(31, itin.Substring(31).IndexOf(" "));
                oNodeNew = oDoc.CreateElement("FareBasis");
                Attribute = oDoc.CreateAttribute("FareBasisCode");
                Attribute.Value = strValue;
                oNodeNew.Attributes.Append(Attribute);
                oNode.AppendChild(oNodeNew);
                //  validity before
                strValue = itin.Substring(47, 5);
                if ((strValue != "     "))
                {
                    oNodeNew = oDoc.CreateElement("FareValidity");
                    Attribute = oDoc.CreateAttribute("ValidityReason");
                    Attribute.Value = "Before";
                    oNodeNew.Attributes.Append(Attribute);
                    Attribute = oDoc.CreateAttribute("ValidityDate");
                    Attribute.Value = oNode.SelectSingleNode("@DepartureDateTime").InnerText;
                    oNodeNew.Attributes.Append(Attribute);
                    oNode.AppendChild(oNodeNew);
                }
                //  validity after
                strValue = itin.Substring(52, 5);
                if ((strValue != "     "))
                {
                    oNodeNew = oDoc.CreateElement("FareValidity");
                    Attribute = oDoc.CreateAttribute("ValidityReason");
                    Attribute.Value = "After";
                    oNodeNew.Attributes.Append(Attribute);
                    Attribute = oDoc.CreateAttribute("ValidityDate");
                    Attribute.Value = oNode.SelectSingleNode("@DepartureDateTime").InnerText;
                    oNodeNew.Attributes.Append(Attribute);
                    oNode.AppendChild(oNodeNew);
                }
                //  bag allowance
                strValue = itin.Substring(58, 2);
                oNodeNew = oDoc.CreateElement("BagAllowance");
                if (strValue == "PC")
                {
                    Attribute = oDoc.CreateAttribute("Quantity");
                    Attribute.Value = "1";
                    oNodeNew.Attributes.Append(Attribute);
                    Attribute = oDoc.CreateAttribute("Type");
                    Attribute.Value = "Piece";
                    oNodeNew.Attributes.Append(Attribute);
                    oNode.AppendChild(oNodeNew);
                }
                else if (strValue == "1P")
                {
                    Attribute = oDoc.CreateAttribute("Quantity");
                    Attribute.Value = "1";
                    oNodeNew.Attributes.Append(Attribute);
                    Attribute = oDoc.CreateAttribute("Type");
                    Attribute.Value = "Piece";
                    oNodeNew.Attributes.Append(Attribute);
                    oNode.AppendChild(oNodeNew);
                }
                else if (strValue == "2P")
                {
                    Attribute = oDoc.CreateAttribute("Quantity");
                    Attribute.Value = "2";
                    oNodeNew.Attributes.Append(Attribute);
                    Attribute = oDoc.CreateAttribute("Type");
                    Attribute.Value = "Piece";
                    oNodeNew.Attributes.Append(Attribute);
                    oNode.AppendChild(oNodeNew);
                }
                else
                {
                    Attribute = oDoc.CreateAttribute("Weight");
                    Attribute.Value = strValue;
                    oNodeNew.Attributes.Append(Attribute);
                    Attribute = oDoc.CreateAttribute("Type");
                    Attribute.Value = "Weight";
                    oNodeNew.Attributes.Append(Attribute);
                    oNode.AppendChild(oNodeNew);
                }
                itin = itin.Substring(61);
            }
            request = oDoc.OuterXml;
            request = request.Substring(request.IndexOf("<OriginDestinationInformation RPH=\"1\">"), (request.IndexOf("<TravelPreferences>") - request.IndexOf("<OriginDestinationInformation RPH=\"1\">")));
            return request;
        }

        public void LogMessageToFile(string MsgType, string Message, DateTime RequestTime, DateTime ResponseTime)
        {
            string strLine = "";
            try
            {
                sb.Append("<Message").Append(" Type=\'").Append(MsgType).Append("\'").Append(" RequestTime=\'");
                sb.Append(RequestTime.ToString("dd MMM yyyy HH:mm:ss")).Append("\'").Append(" ResponseTime=\'");
                sb.Append(ResponseTime.ToString("dd MMM yyyy HH:mm:ss")).Append("\'");
                TimeSpan dur;
                dur = (ResponseTime - RequestTime);
                sb.Append(" Duration=\'").Append(dur.TotalSeconds.ToString()).Append("\'>");
                sb.Append("<AmadeusMessage>").Append(Message).Append("</AmadeusMessage>");
                sb.Append("</Message>");
                strLine = sb.ToString();
                sb.Remove(0, sb.Length);
                sb = null;

                addLog(strLine, ProviderSystems.UserID);
            }
            catch (Exception ex)
            {
                //  
            }
        }

        static bool IsNumeric(object Expression)
        {
            // Variable to collect the Return value of the TryParse method.
            bool isNum;

            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            double retNum;

            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        private string UpdateSessionID(string SessionID)
        {
            string[] sessionid;
            int intSession = 0;


            sb.Remove(0, sb.Length);
            sessionid = SessionID.Split(char.Parse("|"));

            if (ProviderSystems.SOAP2)
            {
                intSession = Convert.ToInt32(sessionid[2]);
                intSession += 1;
                SessionID = sb.Append(sessionid[0]).Append("|").Append(sessionid[1]).Append("|").Append(intSession.ToString()).ToString();
                sb.Remove(0, sb.Length);
            }
            else
            {

                intSession = Convert.ToInt32(sessionid[1]);
                intSession += 1;
                SessionID = sb.Append(sessionid[0]).Append("|").Append(intSession.ToString()).ToString();
                sb.Remove(0, sb.Length);
            }

            return SessionID;
        }

        private string SubSessionID(string SessionID)
        {
            string[] sessionid;
            int intSession = 0;

            sb.Remove(0, sb.Length);
            sessionid = SessionID.Split(char.Parse("|"));



            if (ProviderSystems.SOAP2)
            {
                intSession = Convert.ToInt32(sessionid[2]);
                intSession -= 1;
                SessionID = sb.Append(sessionid[0]).Append("|").Append(sessionid[1]).Append("|").Append(intSession.ToString()).ToString();
                sb.Remove(0, sb.Length);
            }
            else
            {
                intSession = Convert.ToInt32(sessionid[1]);
                intSession -= 1;
                SessionID = sb.Append(sessionid[0]).Append("|").Append(intSession.ToString()).ToString();

            }
            sb.Remove(0, sb.Length);
            return SessionID;
        }

        private string RemoveDuplicate(string xml)
        {
            //StringReader reader = new StringReader(xml);
            XmlDocument TravelBuildResult = new XmlDocument();
            TravelBuildResult.LoadXml(xml);

            List<RemovableFlights> lsRMFlights = new List<RemovableFlights>();

            try
            {
                XmlNodeList node = TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS");
                node = node[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary");
                foreach (XmlNode nd in node)
                {
                    if (nd.Attributes["SequenceNumber"].Value == "1")
                    {
                        XmlNodeList node2 = nd.SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption");
                        foreach (XmlNode nd1 in node2)
                        {
                            XmlNodeList node3 = nd1.SelectNodes("FlightSegment");
                            foreach (XmlNode nd2 in node3)
                            {
                                RemovableFlights rf = new RemovableFlights();
                                rf.DepartureDateTime = nd2.Attributes["DepartureDateTime"].Value;
                                rf.FlightNumber = nd2.Attributes["FlightNumber"].Value;
                                rf.ResBookDesigCode = nd2.Attributes["ResBookDesigCode"].Value;
                                lsRMFlights.Add(rf);
                            }
                        }
                    }

                }
                for (int i = 0; i < TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Count; i++)
                {
                    if (TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[i].Attributes["SequenceNumber"].Value != "1")
                    {
                        for (int j = 0; j < TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[i].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption").Count; j++)
                        {
                            //int cnt = 1;
                            int RPH = 0;
                            for (int k = 0; k < TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[i].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[j].SelectNodes("FlightSegment").Count; k++)
                            {

                                XmlNode xmNode = TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[i].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[j].SelectNodes("FlightSegment")[k];
                                //if (lsRMFlights.FindAll(o => o.DepartureDateTime == xmNode.Attributes["DepartureDateTime"].Value && o.FlightNumber == xmNode.Attributes["FlightNumber"].Value && o.ResBookDesigCode == xmNode.Attributes["ResBookDesigCode"].Value).Count > 0)
                                if (lsRMFlights.FindAll(o => o.DepartureDateTime == xmNode.Attributes["DepartureDateTime"].Value && o.FlightNumber == xmNode.Attributes["FlightNumber"].Value).Count > 0)
                                {
                                    RPH = Int32.Parse(TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[i].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[j].SelectNodes("FlightSegment")[k].Attributes["RPH"].Value);
                                    TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[i].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[j].RemoveChild(TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[i].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[j].SelectNodes("FlightSegment")[k]);
                                }

                            }
                            for (int k = 0; k < TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[i].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[j].SelectNodes("FlightSegment").Count; k++)
                            {

                                XmlNode xmNode = TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[i].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[j].SelectNodes("FlightSegment")[k];
                                //if (lsRMFlights.FindAll(o => o.DepartureDateTime == xmNode.Attributes["DepartureDateTime"].Value && o.FlightNumber == xmNode.Attributes["FlightNumber"].Value && o.ResBookDesigCode == xmNode.Attributes["ResBookDesigCode"].Value).Count > 0)
                                if (lsRMFlights.FindAll(o => o.DepartureDateTime == xmNode.Attributes["DepartureDateTime"].Value && o.FlightNumber == xmNode.Attributes["FlightNumber"].Value).Count > 0)
                                {
                                    RPH = Int32.Parse(TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[i].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[j].SelectNodes("FlightSegment")[k].Attributes["RPH"].Value);
                                    TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[i].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[j].RemoveChild(TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[i].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[j].SelectNodes("FlightSegment")[k]);
                                }
                                else
                                {
                                    int temp = Int32.Parse(TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[i].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[j].SelectNodes("FlightSegment")[k].Attributes["RPH"].Value);
                                    temp = temp - RPH;
                                    if (temp > 0)
                                    {
                                        TravelBuildResult.GetElementsByTagName("OTA_AirLowFareSearchFlightsRS")[0].SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[i].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[j].SelectNodes("FlightSegment")[k].Attributes["RPH"].Value = temp.ToString();
                                    }

                                    //cnt++;
                                }
                            }



                        }
                    }
                }
                return TravelBuildResult.InnerXml;

            }
            catch (Exception ex)
            {
                return xml;
            }

        }

        private void SearchFares()
        {
            string req = strRequestTotal;
            int fareindex = iFareSearches;
            var ttAAPrice = new TravelPortWSAdapter(ProviderSystems);
            //string ConversationID = ttAAPrice.CreateSession();
            string avTempResponse = "";

        }

        private class RemovableFlights
        {
            public string SectorSequence { get; set; }
            public string DepartureDateTime { get; set; }
            public string FlightNumber { get; set; }
            public string ResBookDesigCode { get; set; }

        }

        public static void addLogStat(string msg, string username, string PCC, DateTime myDTFI, DateTime myDTFIR, TimeSpan dur)
        {
            try
            {
                string FilePath = "log\\" + username + "_LFP_" + DateTime.Today.ToString("dd-MM-yyyy");
                string DirPath = "C:\\TripXML\\log";
                FilePath = "C:\\TripXML\\" + FilePath + ".txt";

                if (!Directory.Exists(DirPath))
                {
                    Directory.CreateDirectory(DirPath);
                }
                if (!File.Exists(FilePath))
                {
                    using (StreamWriter sw = File.CreateText(FilePath))
                    {
                        sw.WriteLine("created On - " + DateTime.Now.ToString() + "\r\n");
                        sw.Flush();
                        sw.Close();
                    }
                }
                using (StreamWriter sw = File.AppendText(FilePath))
                {
                    sw.WriteLine("<S>" + myDTFI.ToString("dd MMM yyyy HH:mm:ss") + "</S><E>" + myDTFIR.ToString("dd MMM yyyy HH:mm:ss") + "</E>");
                    sw.WriteLine("<D>" + dur.TotalSeconds + "</D><P>" + PCC + "</P>");
                    sw.WriteLine(msg + "\r\n");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        public static void addLog(string msg, string username)
        {
            try
            {
                string FilePath = "log\\" + username + "_" + DateTime.Today.ToString("dd-MM-yyyy") + "_" + DateTime.Now.Hour;
                string DirPath = "C:\\TripXML\\log";
                FilePath = "C:\\TripXML\\" + FilePath + ".txt";

                if (!Directory.Exists(DirPath))
                {
                    Directory.CreateDirectory(DirPath);
                }
                if (!File.Exists(FilePath))
                {
                    using (StreamWriter sw = File.CreateText(FilePath))
                    {
                        sw.WriteLine("created On - " + DateTime.Now + "\r\n");
                        sw.Flush();
                        sw.Close();
                    }
                }
                using (StreamWriter sw = File.AppendText(FilePath))
                {
                    DateTimeFormatInfo myDTFI = new CultureInfo("en-US", true).DateTimeFormat;
                    sw.WriteLine(DateTime.UtcNow.ToString(myDTFI).Substring(11) + " GMT - " + msg + "\r\n");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        private string formatAmadeus(string strDisp)
        {
            int i;
            int a;
            int b;
            string strDisp1 = "";
            string scr = "";
            sb.Remove(0, sb.Length);
            try
            {
                strDisp = strDisp.Replace("\r\n", "\r");
                for (i = 1; i < strDisp.Length; i++)
                {
                    b = strDisp.Substring(i - 1).Length;

                    if (b < 81)
                        scr = strDisp.Substring(i - 1);
                    else
                        scr = strDisp.Substring((i - 1), 81);

                    a = (scr.IndexOf("\r", 0) + 1);
                    if ((a == 0))
                    {
                        i = (i + 80);
                        sb.Append("<Line>").Append(scr.Replace("&", "&").Replace("<", "<").Replace(">", ">")).Append("</Line>");
                    }
                    else
                    {
                        sb.Append("<Line>").Append(scr.Substring(0, (a - 1)).Replace("&", "&").Replace("<", "<").Replace(">", ">")).Append("</Line>");
                        i = (i + (a - 1));
                    }
                }

                sb.Append("<Line>&gt;</Line>");
                strDisp1 = sb.ToString();
                sb.Remove(0, sb.Length);
                return strDisp1;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private string formatBreakPoint(string strScreen1)
        {
            XmlDocument oDocScreen = new XmlDocument();
            oDocScreen.LoadXml(strScreen1);
            XmlElement oRootScreen = oDocScreen.DocumentElement;
            string strScreen = "";
            string strBreakPoint = "<BreakPoint>";

            for (int i = 5; i < oRootScreen.SelectNodes("Line").Count; i++)
            {
                if (oRootScreen.SelectNodes("Line").Item(i).InnerText != "")
                {
                    if (oRootScreen.SelectNodes("Line").Item(i).InnerText.Substring(0, 5) != "     ")
                    {
                        if (oRootScreen.SelectNodes("Line").Item(i).InnerText.Substring(0, 4) != " Q: "
                            && oRootScreen.SelectNodes("Line").Item(i).InnerText.Substring(0, 4) != " S: ")
                        {
                            strScreen = strScreen + "<Line>" + oRootScreen.SelectNodes("Line").Item(i).InnerText + "</Line>";
                        }
                    }
                }

                if (oRootScreen.SelectNodes("Line").Item(i).InnerText.Contains("TOTAL FARE CALCULATION"))
                {
                    strScreen = "<Screen>" + strScreen + "</Screen>";
                    oDocScreen.LoadXml(strScreen);
                    oRootScreen = oDocScreen.DocumentElement;
                    break;
                }
            }

            for (int i = 0; i < oRootScreen.SelectNodes("Line").Count; i++)
            {
                XmlNode oNodeScreen = oRootScreen.SelectNodes("Line").Item(i);

                if (oNodeScreen.InnerText.IndexOf(" FARE BASIS") == -1)
                {
                    if (oRootScreen.SelectNodes("Line").Item(i + 1).InnerText.IndexOf(" FARE BASIS") == -1)
                    {
                        if (oRootScreen.SelectNodes("Line").Item(i + 2).InnerText.IndexOf(" FARE BASIS") == -1)
                        {
                            strBreakPoint = strBreakPoint + "N";
                        }
                        else
                        {
                            strBreakPoint = strBreakPoint + "Y";
                        }
                    }
                    else if (oRootScreen.SelectNodes("Line").Item(i + 2).InnerText.Contains("TOTAL FARE CALCULATION"))
                    {
                        break;
                    }
                    else
                    {
                        if (oRootScreen.SelectNodes("Line").Item(i + 2).InnerText.IndexOf(" FARE BASIS") == -1)
                        {
                            if (oRootScreen.SelectNodes("Line").Item(i + 3).InnerText.IndexOf(" FARE BASIS") == -1)
                            {
                                strBreakPoint = strBreakPoint + "N";
                            }
                            else
                            {
                                strBreakPoint = strBreakPoint + "Y";
                            }
                        }
                    }
                }
            }

            strBreakPoint = strBreakPoint + "</BreakPoint>";

            return strBreakPoint;

            //        <Screen>
            //    <Line>FQH1</Line>
            //    <Line/>
            //    <Line/>
            //    <Line/>
            //    <Line>  FCP  AL  BK TPM   MPM   EMA  EMS R GI CC  NVB  NVA    BG</Line>
            //    <Line>  WAS</Line>
            //    <Line>  DKR  SA  W                       R AT SA 10APR10APR   2P</Line>
            //    <Line> FARE BASIS:WKPXZA3         AMOUNT IN NUC:         539.50</Line>
            //    <Line>  ABJ  DN  W  1132  1358           M EH DN              2P</Line>
            //    <Line> FARE BASIS:W               AMOUNT IN NUC:         377.61</Line>
            //    <Line>X DKR  KQ  M                                            2P</Line>
            //    <Line>  WAS  SA  K  5097  6116           M AT YY 10MAY10MAY   2P</Line>
            //    <Line> FARE BASIS:YIF             AMOUNT IN NUC:        2628.00</Line>
            //    <Line/>
            //    <Line> TOTAL FARE CALCULATION:                          3545.11</Line>
            //    <Line> ROE: 1.000000                     FARE USD:      3545.00</Line>
            //    <Line> TAX: YRVA          435.00   YQAC           31.57</Line>
            //    <Line>      YCAE            5.50   USAP           17.20</Line>
            //    <Line>      USAS           17.20   XACO            5.00</Line>
            //    <Line>      XYCR            7.00   AYSE            2.50</Line>
            //    <Line>      HPDE          138.82   KQEA            5.92</Line>
            //    <Line>                                                  PAGE  1/ 2</Line>
            //    <Line>&gt;</Line>
            //</Screen>
        }
    }
}
