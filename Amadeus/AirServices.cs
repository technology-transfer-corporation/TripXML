using System.Xml;
using TripXMLMain;
using System.Text;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;
using static TripXMLMain.modCore.enAmadeusWSSchema;

namespace AmadeusWS
{
    public class AirServices : AmadeusBase
    {
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
        private string[] strReq;
        private bool[] bPriceEnd;
        private string strMessage = "";

        public string XslPortalPath
        {
            get
            {
                return XslPath.Replace("AmadeusWS", "Portal");
            }
            set
            {
                XslPath = $"{value}Portal\\";
            }
        }

        public AirServices()
        {
            Request = "";
            ConversationID = "";
        }

        public string AirAvail()
        {
            AmadeusWSAdapter ttAA = null;
            XmlElement oRoot;
            string response;
            string pages;
            string strRequest;
            string strIniAirReply;
            string strNxtAirReply;
            string strIniFlightAirReply;
            int count;
            string strResponse = "";
            string strNxtFlightAirReply = null;
            bool inSession = false;

            try
            {
                DateTime RequestTime = DateTime.Now;
                strRequest = SetRequest($"AmadeusWS_AirAvailRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                if (Version.Equals("v03_"))
                {
                    //*******************
                    //  Create Session  *
                    //*******************
                    inSession = SetConversationID(ttAA);

                    // *******************************************************************************
                    //  Get from Transformed Request and Send Fare message to the AmadeusWS Adapter  *
                    // *******************************************************************************

                    try
                    {
                        XmlDocument oDoc = new XmlDocument();
                        oDoc.LoadXml(Request);
                        oRoot = oDoc.DocumentElement;
                        string _tracerID = "";
                        if (oRoot != null && oRoot.HasAttribute("EchoToken") && !string.IsNullOrEmpty(oRoot.Attributes["EchoToken"].Value))
                        {
                            _tracerID = oRoot.Attributes["EchoToken"].Value;
                        }
                        ttAA.TracerID = _tracerID;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error Loading Transformed Request XML Document.\r\n{ex.Message}");
                    }

                    //********************************************************************************
                    // Send Transformed Request to the AmadeusWS Adapter and Getting Native Response *
                    //********************************************************************************
                    try
                    {
                        strMessage = strRequest;
                        strResponse = SendAirMultiAvailability(ttAA, strRequest);

                        strIniAirReply = strResponse;
                        strMessage += $"{Environment.NewLine}{strResponse}";

                        XmlDocument oDocResp = new XmlDocument();
                        oDocResp.LoadXml(strIniAirReply);
                        XmlElement oRootResp = oDocResp.DocumentElement;

                        XmlNodeList oNodeResp = oRootResp.SelectNodes("singleCityPairInfo/flightInfo");
                        var sb = new StringBuilder();
                        foreach (XmlNode oNode1 in oNodeResp)
                        {
                            sb.Append(oNode1.OuterXml);
                        }

                        strIniFlightAirReply = sb.ToString();
                    }

                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    var oNode = oRoot.SelectSingleNode("OriginDestinationInformation/DatePeriod");
                    pages = oNode.Attributes["Pages"].Value;
                    count = Convert.ToInt32(pages);

                    if (!string.IsNullOrEmpty(pages) && !strResponse.Contains("NO FLIGHT FOR THIS CITY PAIR"))
                    {

                        //********************************************************************************
                        // Send Transformed Request to the AmadeusWS Adapter and Getting Native Response *
                        //******************************************************************************** 
                        try
                        {
                            strRequest = CoreLib.TransformXML(Request, XslPath, $"AmadeusWS_AirAvailRQ1.xsl");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error Transforming OTA Request. \r\n{ex.Message}");
                        }

                        var sb = new StringBuilder();
                        for (int i = 0; i < count - 1; i++)
                        {

                            try
                            {
                                //sb.Append(strMessage).Append(Environment.NewLine).Append(strRequest);
                                strMessage += $"{Environment.NewLine}{strRequest}";

                                strResponse = SendAirMultiAvailability(ttAA, strRequest);

                                strMessage += $"{Environment.NewLine}{strResponse}";

                                strNxtAirReply = strResponse;

                                XmlDocument oDocRespN = new XmlDocument();
                                oDocRespN.LoadXml(strNxtAirReply);
                                XmlElement oRootRespN = oDocRespN.DocumentElement;
                                XmlNodeList oNodeRespN = oRootRespN.SelectNodes("singleCityPairInfo/flightInfo");

                                foreach (XmlNode oNode2 in oNodeRespN)
                                {
                                    sb.Append(oNode2.OuterXml);
                                }

                                if (strResponse.IndexOf("NO LATER FLTS") != -1)
                                    break;

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }

                        }
                        strNxtFlightAirReply = sb.ToString();
                        sb.Clear();

                    }
                }
                else
                {
                    // *******************************************************************************
                    //  Get from Transformed Request and Send Fare message to the AmadeusWS Adapter  *
                    // *******************************************************************************

                    try
                    {
                        XmlDocument oDoc = new XmlDocument();
                        oDoc.LoadXml(Request);
                        oRoot = oDoc.DocumentElement;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error Loading Transformed Request XML Document.\r\n{ex.Message}");
                    }

                    //*******************************************************************************
                    // Send Transformed Request to the AmadeusWS Adapter and Getting Native Response  *
                    //******************************************************************************* 

                    try
                    {
                        strMessage += $"{strMessage}{Environment.NewLine}{strRequest}";

                        ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
                        strResponse = SendAirMultiAvailability(ttAA, strRequest);

                        strIniAirReply = strResponse;

                        strMessage += $"{strMessage}{Environment.NewLine}{strRequest}";

                        XmlDocument oDocResp = new XmlDocument();
                        oDocResp.LoadXml(strIniAirReply);
                        XmlElement oRootResp = oDocResp.DocumentElement;

                        XmlNodeList oNodeResp = oRootResp.SelectNodes("singleCityPairInfo/flightInfo");

                        var sb = new StringBuilder();
                        foreach (XmlNode oNode1 in oNodeResp)
                        {
                            sb.Append(oNode1.OuterXml);
                        }
                        strIniFlightAirReply = sb.ToString();
                        sb.Clear();
                    }

                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                if (strIniAirReply.IndexOf("<flightInfo>") != -1)
                {
                    response = strIniAirReply.Substring(0, strIniAirReply.IndexOf("<flightInfo>"));
                    strResponse = $"{response}{strIniFlightAirReply}{strNxtFlightAirReply}</singleCityPairInfo></Air_MultiAvailabilityReply>";
                }

                //*****************************************************************
                // Transform Native AmadeusWS AirAvail Response into OTA Response   *
                //***************************************************************** 
                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_AirAvailRS.xsl", false);

                    if (Version.Equals("v03_") && !strResponse.Contains("Errors"))
                        strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_AirAvailRPHRS.xsl", false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }

                DateTime responseTime = DateTime.Now;
                if (ttProviderSystems.LogNative)
                    TripXMLTools.TripXMLLog.LogMessage("LowFarePlus", ref strMessage, RequestTime, responseTime, "Native", ttProviderSystems.Provider.ToString(), ttProviderSystems.System.ToString(), ttProviderSystems.UserName.ToString());
            }

            catch (Exception ex)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.AirAvail, ex.Message, ttProviderSystems);
            }

            finally
            {
                if (inSession && Version.Equals("v03_"))
                {
                    if (ttProviderSystems.SessionPool)
                    {
                        ConversationID = SubSessionID(ConversationID);
                        ttAA.CloseSessionFromPool(ConversationID);
                    }
                    else
                    {
                        ttAA.CloseSession(ConversationID);
                    }
                }
            }
            return strResponse;
        }

        public string AirFlifo()
        {
            string strResponse = "";

            var strRequest = SetRequest($"AmadeusWS_AirFlifoRQ.xsl");

            if (string.IsNullOrEmpty(strRequest))
                throw new Exception("Transformation produced empty xml.");

            AmadeusWSAdapter ttAA;
            // *******************************************************************************
            //  Send Transformed Request to the AmadeusWS Adapter and Getting Native Response  *
            // ******************************************************************************* 
            try
            {
                ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
                strResponse = SendFlightInfo(ttAA, strRequest);
            }
            catch (Exception ex)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                throw ex;
            }

            // *****************************************************************
            //  Transform Native AmadeusWS AirFlifo Response into OTA Response   *
            // ***************************************************************** 
            try
            {
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_AirFlifoRS.xsl", false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
            }
            return strResponse;
        }

        public string AirPrice()
        {
            XmlElement oRoot;
            string strRequest;
            string strResponse = "";
            string strPNRReply;
            DateTime RequestTime;
            DateTime ResponseTime;
            string strMessage = "";
            bool returnBreakPoint = false;
            bool inSession = false;
            XmlDocument oDocResp;
            XmlElement oRootResp;
            XmlNode oNodeResp;
            AmadeusWSAdapter ttAA = null;

            try
            {
                // *****************************************************************
                //  Transform OTA AirPrice Request into Several Navite Requests    *
                // ***************************************************************** 
                CoreLib.SendTrace(ttProviderSystems.UserID, "AirServices", "AirPrice", Request, ttProviderSystems.LogUUID);
                RequestTime = DateTime.Now;
                strRequest = SetRequest($"AmadeusWS_AirPriceRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                if (Request.Contains("ReturnBreakPoint=\"true\""))
                    returnBreakPoint = true;

                if (Version.Equals("v03_"))
                {
                    try
                    {
                        ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
                        inSession = SetConversationID(ttAA);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error Creating Session.\r\n{ex.Message}");
                    }

                    // *******************************************************************************
                    //  Get from Transformed Request and Send Fare message to the AmadeusWS Adapter  *
                    // *******************************************************************************

                    try
                    {
                        XmlDocument oDoc = new XmlDocument();
                        oDoc.LoadXml(strRequest);
                        oRoot = oDoc.DocumentElement;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error Loading Transformed Request XML Document.\r\n{ex.Message}");
                    }

                    string strAddMultiElements = "";
                    string strSellFlights = "";
                    string strFarePlus = "";
                    string strIgnore = "";

                    XmlNode oNode = oRoot.SelectSingleNode("AddMultiElements");
                    strAddMultiElements = oNode.InnerXml;

                    oNode = oRoot.SelectSingleNode("SellFlights");
                    strSellFlights = oNode.InnerXml;

                    oNode = oRoot.SelectSingleNode("FarePlus");
                    strFarePlus = oNode.InnerXml;

                    oNode = oRoot.SelectSingleNode("Ignore");
                    strIgnore = oNode.InnerXml;

                    // this is the logic:
                    // - sell flights first so if there is an error, send an ignore and exit with error
                    // - add names and then price
                    // - ignore everything to release the seats booked
                    try
                    {
                        strResponse = SendAirSellFromRecommendation(ttAA, strRequest);
                        strMessage = $"{strMessage}{strResponse}";
                        strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_TB_Errors.xsl", false);
                        CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSService", "strResponse", strResponse, ttProviderSystems.LogUUID);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    //  Log Errors
                    if (!strResponse.Contains("<Error"))
                    {
                        try
                        {
                            //  Send Request
                            strMessage = strRequest;
                            strResponse = SendAddMultiElements(ttAA, strRequest);

                            strPNRReply = strResponse;
                            strMessage = $"{strMessage}{strResponse}";
                            strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_TB_Errors.xsl", false);
                            CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSService", "strResponse", strResponse, ttProviderSystems.LogUUID);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        if (!strResponse.Contains("<Error"))
                        {
                            // ************************************************************************************
                            //  Get from Transformed Request and Send FarePlus_DisplayLowestApplicableFare_Query  *
                            // ************************************************************************************
                            try
                            {
                                //  Send Request
                                strMessage = $"{strMessage}{strFarePlus}";

                                strResponse = strFarePlus.StartsWith("<Fare_PricePNRWithLowerFares>")
                                    ? SendPricePNRWithLowerFares(ttAA, strFarePlus)
                                    : SendPricePNRWithBookingClass(ttAA, strFarePlus);

                                strMessage = $"{strMessage}{strResponse}";

                                // ignore everything
                                strIgnore = SendRequestCryptically(ttAA, strIgnore);

                                //  Close Session
                                inSession = false;
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }

                            //  *******************************************************************
                            //  Add AirTravelerAvail/PassengerTypeQuantity to AmadeusWS Response    *
                            //  *******************************************************************
                            try
                            {
                                strRequest = oRoot.SelectSingleNode("AirTravelerAvail").InnerXml;
                                oDocResp = new XmlDocument();
                                oDocResp.LoadXml(strResponse);
                                oRootResp = oDocResp.DocumentElement;
                                oNodeResp = oDocResp.CreateNode(XmlNodeType.Element, "", "AirTravelerAvail", "");
                                oNodeResp.InnerXml = strRequest;
                                oRootResp.AppendChild(oNodeResp);
                                strRequest = oRoot.SelectSingleNode("FlightSegments").InnerXml;
                                oNodeResp = oDocResp.CreateNode(XmlNodeType.Element, "", "FlightSegments", "");
                                oNodeResp.InnerXml = strRequest;
                                oRootResp.AppendChild(oNodeResp);
                                strResponse = oDocResp.OuterXml;
                                oDocResp = null;

                                if (strResponse.Contains("</Fare_PricePNRWithLowerFaresReply>"))
                                {
                                    strPNRReply += "</Fare_PricePNRWithLowerFaresReply>";
                                    strResponse = strResponse.Replace("</Fare_PricePNRWithLowerFaresReply>", strPNRReply);
                                }
                                else
                                {
                                    strPNRReply += "</Fare_PricePNRWithBookingClassReply>";
                                    strResponse = strResponse.Replace("</Fare_PricePNRWithBookingClassReply>", strPNRReply);
                                }

                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Error Loading AirTravelerAvail into Air Price Response. \r\n{ex.Message}");
                            }
                        }
                        else
                        {
                            strResponse = $"<PNR_Reply>{strResponse}</PNR_Reply>";
                        }
                    }
                    else
                    {
                        // sell segments has failed
                        // ignore flights to release seats and close session
                        // ignore everything

                        strIgnore = SendRequestCryptically(ttAA, strIgnore);

                        //  Close Session
                        strResponse = $"<PNR_Reply>{strResponse}</PNR_Reply>";
                    }
                }
                else
                {
                    // *******************************************************************************
                    //  Get from Transformed Request and Send Fare message to the AmadeusWS Adapter  *
                    // *******************************************************************************
                    try
                    {
                        ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
                        ConversationID = ttProviderSystems.SessionPool ? ttAA.CheckSessionV2() : ttAA.CreateSession();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error Creating Session.\r\n{ex.Message} ");
                    }

                    try
                    {
                        XmlDocument oDoc = new XmlDocument();
                        oDoc.LoadXml(strRequest);
                        oRoot = oDoc.DocumentElement;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error Loading Transformed Request XML Document.\r\n{ex.Message}");
                    }

                    XmlNode oNode = oRoot.SelectSingleNode("Fare_InformativePricingWithoutPNR");

                    if (oNode == null)
                        oNode = oRoot.SelectSingleNode("Fare_InformativeBestPricingWithoutPNR");

                    strRequest = oNode.OuterXml;

                    try
                    {
                        ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();

                        if (strRequest.StartsWith("<Fare_InformativePricingWithoutPNR"))
                        {
                            strResponse = SendInformativePricingWithoutPNR(ttAA, strRequest);
                            strResponse = CoreLib.TransformXML($"<FIP>{strRequest}{strResponse}</FIP>", XslPath, $"AmadeusWS_AirPrice1RS.xsl", false);
                        }

                        else
                        {
                            strResponse = SendFareInformativeBestPricingWithoutPNR(ttAA, strRequest);
                            strResponse = CoreLib.TransformXML($"<FIP>{strRequest}{strResponse}</FIP>", XslPath, $"AmadeusWS_LowFareFlights1RS.xsl", false);
                        }

                        string strFareFamiliyDescResponse = CoreLib.TransformXML($"<FIP>{strRequest}{ strResponse}</FIP>", XslPath, $"AmadeusWS_LowFareFlights1RS.xsl", false);
                        strResponse = SendGetFareFamilyDescription(ttAA, strFareFamiliyDescResponse);
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    if (strRequest.StartsWith("<Fare_InformativeBestPricingWithoutPNR") == true && strResponse.IndexOf("<Error>") == -1 && returnBreakPoint)
                    {
                        var strCrypticResp = SendCommandCryptically(ttAA, "FQH1");

                        XmlDocument oDocCryptic = new XmlDocument();
                        oDocCryptic.LoadXml(strCrypticResp);
                        XmlElement oRootCryptic = oDocCryptic.DocumentElement;
                        string strScreen = oRootCryptic.SelectSingleNode("longTextString/textStringDetails").InnerText;

                        strScreen = formatAmadeus(strScreen);
                        if (!string.IsNullOrEmpty(strScreen))
                        {
                            strScreen = formatBreakPoint($"<Screen>{strScreen}</Screen>");
                            strResponse = strResponse.Replace("</Fare_InformativeBestPricingWithoutPNRReply>", $"{strScreen}</Fare_InformativeBestPricingWithoutPNRReply>");
                        }
                    }

                    var strcSession = ttProviderSystems.SessionPool ? ttAA.CloseSessionFromPool(ConversationID) : ttAA.CloseSession(ConversationID);

                    ttAA = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!inSession)
                {
                    ttAA.CloseSession(ConversationID);
                    ConversationID = string.Empty;
                }
                ttAA = null;
            }

            //  *******************************************************************
            //  Add AirTravelerAvail/PassengerTypeQuantity to AmadeusWS Response    *
            //  *******************************************************************
            try
            {
                strRequest = oRoot.SelectSingleNode("AirTravelerAvail").InnerXml;
                oDocResp = new XmlDocument();
                oDocResp.LoadXml(strResponse);
                oRootResp = oDocResp.DocumentElement;
                oNodeResp = oDocResp.CreateNode(XmlNodeType.Element, "", "AirTravelerAvail", "");
                oNodeResp.InnerXml = strRequest;
                oRootResp.AppendChild(oNodeResp);
                strRequest = oRoot.SelectSingleNode("FlightSegments").InnerXml;
                oNodeResp = oDocResp.CreateNode(XmlNodeType.Element, "", "FlightSegments", "");
                oNodeResp.InnerXml = strRequest;
                oRootResp.AppendChild(oNodeResp);
                strResponse = oDocResp.OuterXml;
                oDocResp = null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Loading AirTravelerAvail into Air Price Response.\r\n{ex.Message}");
            }
            // *****************************************************************
            //  Transform Native AmadeusWS AirPrice Response into OTA Response   *
            // ***************************************************************** 
            try
            {
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Response to transform", strResponse, ttProviderSystems.LogUUID);
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_AirPriceRS.xsl", false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
            }

            ResponseTime = DateTime.Now;
            try
            {
                if (ttProviderSystems.LogNative)
                {
                    TripXMLTools.TripXMLLog.LogMessage("AirPrice", ref strMessage, RequestTime, ResponseTime, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }
                return strResponse;
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                throw exx;
            }

            finally
            {
                if (inSession && ttAA != null && Version.Equals("v03_"))
                {
                    if (ttProviderSystems.SessionPool)
                    {
                        ConversationID = SubSessionID(ConversationID);
                        ttAA.CloseSessionFromPool(ConversationID);
                    }
                    else
                    {
                        ttAA.CloseSession(ConversationID);
                    }
                }
            }
        }

        public string AirRules()
        {
            AmadeusWSAdapter ttAA = null;
            XmlElement oRoot = null;
            string categories = "";
            DateTime requestTime;
            DateTime responseTime;
            string strMessage = "";
            bool inSession = false;

            try
            {
                // *****************************************************************
                //  Transform OTA AirRules Request into Several Navite Requests    *
                // ***************************************************************** 
                ttAA = SetAdapter();
                inSession = SetConversationID(ttAA);
                bool skipFareRequest = !string.IsNullOrEmpty(ConversationID);
                var strRequest = SetRequest($"AmadeusWS_AirRulesRQ.xsl");

                requestTime = DateTime.Now;

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                ttAA = SetAdapter();

                string strResponse = "";

                if (Version.Equals("v03_"))
                {
                    XmlNode oNode;

                    if (!skipFareRequest)
                    {
                        try
                        {
                            var oDoc = new XmlDocument();
                            oDoc.LoadXml(strRequest);
                            oRoot = oDoc.DocumentElement;
                            oNode = oRoot.SelectSingleNode("Fare_InformativePricingWithoutPNR");
                            strRequest = oNode.OuterXml;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error Loading Transformed Request XML Document.\r\n{ex.Message}");
                        }

                        strResponse = SendInformativePricingWithoutPNR(ttAA, strRequest);

                        // now we need to find how many different fare basis codes we have in the response
                        // we will then send one check rules message per different fare basis code in response
                        // finally we will collect all fare rules replies and aggregate into one response back

                        XmlDocument oDocResp = new XmlDocument();
                        oDocResp.LoadXml(strResponse);
                        XmlElement oRootResp = oDocResp.DocumentElement;

                        XmlNodeList oNodeRespList = oRootResp.SelectNodes("mainGroup/pricingGroupLevelGroup");
                        XmlNodeList oNodePaxList = oRoot.SelectNodes("Fare_InformativePricingWithoutPNR/passengersGroup");
                        int noofSegments = 0;

                        if (oNodeRespList.Count > 0)
                        {
                            string FBCodes = "";
                            string FBCode = "";
                            string[] arrFBCode = new string[20];
                            string[] arrDepDate = new string[20];
                            string[] arrDepCity = new string[20];
                            string[] arrArrCity = new string[20];
                            string[] arrFCInd = new string[20];
                            string[] arrPaxInd = new string[20];
                            string[] arrPaxType = new string[20];
                            string[] arrFBExist = new string[20];

                            int i = 0;
                            int Paxnum = 1;
                            string sPaxType = "";

                            foreach (XmlNode oNodeResp in oNodeRespList)
                            {
                                XmlNodeList oNodeSegList = oNodeResp.SelectNodes("fareInfoGroup/segmentLevelGroup");

                                int segnum = 1;
                                int fbcnum = 0;
                                FBCodes = "";

                                strRequest =
                                    $"<Fare_CheckRules><msgType><messageFunctionDetails><messageFunction>712</messageFunction></messageFunctionDetails></msgType><itemNumber><itemNumberDetails><number>{Paxnum}"
                                    +
                                    "</number></itemNumberDetails></itemNumber><fareRule><tarifFareRule><ruleSectionId>50</ruleSectionId><ruleSectionId>15</ruleSectionId><ruleSectionId>16</ruleSectionId><ruleSectionId>14</ruleSectionId></tarifFareRule></fareRule></Fare_CheckRules>";

                                strResponse = SendFareCheckRules(ttAA, strRequest);

                                XmlDocument oDocFCR = new XmlDocument();
                                oDocFCR.LoadXml(strResponse);
                                XmlElement oRootFCR = oDocFCR.DocumentElement;
                                XmlNodeList oNodeFCRList = oRootFCR.SelectNodes("flightDetails");
                                string[] arrFCRbasis = new string[20];
                                int iFCR = 0;

                                foreach (XmlNode oNodeFCR in oNodeFCRList)
                                {
                                    arrFCRbasis.SetValue(oNodeFCR.SelectSingleNode("qualificationFareDetails/additionalFareDetails/rateClass").InnerText, iFCR);
                                    iFCR++;
                                }

                                foreach (XmlNode oNodeSeg in oNodeSegList)
                                {
                                    FBCode = oNodeSeg.SelectSingleNode("fareBasis/additionalFareDetails/rateClass").InnerText;

                                    if (FBCodes.IndexOf(FBCode) == -1)
                                    {
                                        arrFBExist.SetValue("N", i);
                                        noofSegments++;
                                        for (int iSearchFB = 0; iSearchFB < arrFCRbasis.Length; iSearchFB++)
                                        {
                                            if (arrFCRbasis[iSearchFB] == FBCode
                                                || (FBCode.Contains("CH")
                                                    && ((FBCode.Length > 4 &&
                                                         arrFCRbasis[iSearchFB] == FBCode.Substring(0, FBCode.Length - 4) &&
                                                         FBCode.Substring(0, FBCode.Length - 2)
                                                             .Substring(FBCode.Substring(0, FBCode.Length - 2).Length - 2) ==
                                                         "CH")
                                                        || (arrFCRbasis[iSearchFB] == FBCode.Substring(0, FBCode.Length - 2) &&
                                                            FBCode.Substring(FBCode.Length - 2) == "CH"))))
                                            {
                                                fbcnum = iSearchFB + 1;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        arrFBExist.SetValue("Y", i);

                                    FBCodes = FBCodes + FBCode + "-";
                                    arrFBCode.SetValue(FBCode, i);
                                    arrFCInd.SetValue(fbcnum.ToString(), i);
                                    arrPaxInd.SetValue(Paxnum.ToString(), i);

                                    sPaxType = oNodePaxList[Paxnum - 1].SelectSingleNode("ptcGroup/discountPtc/valueQualifier") != null
                                        ? oNodePaxList[Paxnum - 1].SelectSingleNode("ptcGroup/discountPtc/valueQualifier").InnerXml
                                        : oNodePaxList[Paxnum - 1].SelectSingleNode("discountPtc/valueQualifier").InnerXml;

                                    if (sPaxType == "CNN")
                                        sPaxType = "CHD";

                                    arrPaxType.SetValue(sPaxType, i);

                                    if (oNodeResp.SelectSingleNode("tripsGroup") != null)
                                    {
                                        arrDepDate.SetValue(oNode.SelectSingleNode($"tripsGroup/segmentGroup[position()={segnum}]/segmentInformation/flightDate/departureDate").InnerText, i);
                                        arrDepCity.SetValue(oNode.SelectSingleNode($"tripsGroup/segmentGroup[position()={segnum}]/segmentInformation/boardPointDetails/trueLocationId").InnerText, i);
                                        arrArrCity.SetValue(oNode.SelectSingleNode($"tripsGroup/segmentGroup[position()={segnum}]/segmentInformation/offpointDetails/trueLocationId").InnerText, i);
                                    }
                                    else
                                    {
                                        arrDepDate.SetValue(oNode.SelectSingleNode($"segmentGroup[position()={segnum}]/segmentInformation/flightDate/departureDate").InnerText, i);
                                        arrDepCity.SetValue(oNode.SelectSingleNode($"segmentGroup[position()={segnum}]/segmentInformation/boardPointDetails/trueLocationId").InnerText, i);
                                        arrArrCity.SetValue(oNode.SelectSingleNode($"segmentGroup[position()={segnum}]/segmentInformation/offpointDetails/trueLocationId").InnerText, i);
                                    }

                                    i++;
                                    segnum++;
                                }
                                Paxnum++;
                            }
                        }
                    }

                    if (oRoot != null)
                    {
                        oNode = oRoot.SelectSingleNode("Fare_CheckRules");
                        strRequest = oNode.OuterXml;
                    }
                    else
                    {
                        try
                        {
                            var oDoc = new XmlDocument();
                            oDoc.LoadXml(Request);
                            oRoot = oDoc.DocumentElement;

                            oNode = oRoot.SelectSingleNode("TravelerInfoSummary");
                            strRequest = oNode.OuterXml;
                            oRoot = null;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error Loading Transformed Request XML Document.\r\n{ex.Message}");
                        }

                        strRequest =
                            @"<Fare_CheckRules><msgType><messageFunctionDetails><messageFunction>712</messageFunction></messageFunctionDetails></msgType><itemNumber><itemNumberDetails><number>0x</number></itemNumberDetails></itemNumber><fareRule><tarifFareRule><ruleSectionId>TF</ruleSectionId></tarifFareRule></fareRule></Fare_CheckRules>";
                    }

                    string strReq = strRequest;
                    string strDepDates = "<DepartureDates>";
                    string strCityPairs = "<CityPairs>";
                    string strPaxTypes = "<PaxTypes>";
                    string strResponses = "<Fare_CheckRulesReplies>";

                    string[] arrFRResp = new string[20];

                    for (int j = 0; j < oNode.ChildNodes.Count; j++)
                    {
                        strRequest = strReq;
                        strRequest = strRequest.Replace("<number>0x</number>", $"<number>{j + 1}</number>"  /*arrPaxInd[j]*/ );

                        strResponse = SendFareCheckRules(ttAA, strRequest);

                        if (oRoot != null)
                        {
                            strDepDates = $"{strDepDates}</DepartureDates>";
                            strCityPairs = $"{strCityPairs}</CityPairs>";
                            strPaxTypes = $"{strPaxTypes}</PaxTypes>";
                            strResponses = $"{strResponses}{strDepDates}{strCityPairs}{strPaxTypes}</Fare_CheckRulesReplies>";
                            strResponse = strResponses;
                        }
                        else
                        {
                            strResponses += strResponse;
                        }
                    }
                    strResponse = $"{strResponses}</Fare_CheckRulesReplies>";
                }
                else
                {
                    // ****************************************************************************
                    //  Get RuleReqInfo, FareBasisCode and Categories from Transformed Request    * 
                    //  and Send Fare_DisplayFaresForCityPairPlus_Query to the AmadeusWS Adapter    *
                    // ****************************************************************************
                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(strRequest);
                    oRoot = oDoc.DocumentElement;
                    //  Get the RuleReqInfo Node
                    string strRuleReqInfo = oRoot.SelectSingleNode("RuleReqInfo").OuterXml;
                    //  Get Categories. If any
                    if (oRoot.SelectSingleNode("fareRule") != null)
                        categories = oRoot.SelectSingleNode("fareRule").OuterXml;
                    //  Get FareBasisCode (FareReference)
                    string FareBasisCode = oRoot.SelectSingleNode("RuleReqInfo/FareReference").InnerText;
                    //  Display Fares Request (Fare_DisplayFaresForCityPairPlus_Query)
                    XmlNode oNode = oRoot.SelectSingleNode("Fare_DisplayFaresForCityPair");
                    strRequest = oNode.OuterXml;

                    if (string.IsNullOrEmpty(FareBasisCode))
                        throw new Exception("Fare Reference Code is Missing in the Request.");

                    //  Send Request
                    strResponse = SendFareDisplayFaresForCityPair(ttAA, strRequest);
                    strMessage = $"{strRequest}{strResponse}";

                    // *************************************
                    //  Find Fare Basis Code in Response   *
                    // ************************************* 
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;
                    oNode = oRoot.SelectSingleNode($"flightDetails/itemGrp[fareQualifItem/additionalFareDetails/rateClass=\'{FareBasisCode}\']");

                    if (oNode != null)
                    {
                        strRequest = $"<Fare_CheckRules><msgType><messageFunctionDetails><messageFunction>712</messageFunction></messageFunctionDetails></msgType><itemNumber><itemNumberDetails><number>{oNode.SelectSingleNode("itemNb/itemNumberDetails/number").InnerXml}</number></itemNumberDetails></itemNumber>{categories}</Fare_CheckRules>";
                        strResponse = SendFareCheckRules(ttAA, strRequest);
                        strMessage = $"{strRequest}{strResponse}";
                        strResponse = strResponse.Replace("</Fare_CheckRulesReply>", $"{strRuleReqInfo}</Fare_CheckRulesReply>");
                    }
                }

                // *******************************************************************
                //  Transform Native AmadeusWS AirRules Response into OTA Response   *
                // ******************************************************************* 
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_AirRulesRS.xsl", false);
                responseTime = DateTime.Now;
                if (ttProviderSystems.LogNative)
                {
                    TripXMLTools.TripXMLLog.LogMessage("AirRules", ref strMessage, requestTime, responseTime, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }
                return strResponse;
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}</M><BL/>", ttProviderSystems.UserID);
                throw exx;
            }
            finally
            {
                if (inSession && ttAA != null)
                {
                    if (!string.IsNullOrEmpty(ConversationID))
                    {
                        ttAA.CloseSession(ConversationID);
                    }
                }
            }
        }

        public string AirSeatMap()
        {
            string strRequest;

            strRequest = SetRequest($"AmadeusWS_AirSeatMapRQ.xsl");

            if (string.IsNullOrEmpty(strRequest))
                throw new Exception("Transformation produced empty xml.");

            string strResponse;
            // *******************************************************************************
            //  Send Transformed Request to the AmadeusWS Adapter and Getting Native Response  *
            // ******************************************************************************* 
            try
            {
                AmadeusWSAdapter ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
                ttAA.TracerID = _tracerID;

                strResponse = SendRetrieveSeatMap(ttAA, strRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // *****************************************************************
            //  Transform Native AmadeusWS AirSeatMap Response into OTA Response   *
            // ***************************************************************** 
            try
            {
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_AirSeatMapRS.xsl", false);
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
            }
        }

        public string FareDisplay()
        {
            // *****************************************************************
            //  Transform OTA AirFareDisplay Request into Native AmadeusWS Request     *
            // ***************************************************************** 
            try
            {
                string strRequest = SetRequest($"AmadeusWS_FareDisplayRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                var ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
                ttAA.TracerID = _tracerID;

                var strResponse = SendFareDisplayFaresForCityPair(ttAA, strRequest);

                // *****************************************************************
                //  Transform Native AmadeusWS AirFareDisplay Response into OTA Response   *
                // ***************************************************************** 
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSService", "Aggregated fare display response", strResponse, ttProviderSystems.LogUUID);
                return strResponse;
            }
            catch (Exception exx)
            {
                throw exx;
            }
        }

        public string LowFare()
        {
            string strResponse = "";
            XmlElement oRoot = null;
            DateTime RequestTime;
            DateTime ResponseTime;
            string strMessage = "";
            try
            {
                RequestTime = DateTime.Now;
                string strRequest = SetRequest($"AmadeusWS_LowFareRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                //  Send Transformed Request to the AmadeusWS Adapter and Getting Native Response  *
                // ******************************************************************************* 

                AmadeusWSAdapter ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
                ttAA.TracerID = _tracerID;

                strResponse = SendMasterPricerExpertSearch(ttAA, strRequest);
                strMessage = $"{strRequest}{strResponse}";

                // ****************************************************
                //  Add AirTravelerAvail Request to Native Response   *
                // ****************************************************
                try
                {
                    strRequest = CoreLib.GetNodeInnerText(Request, "AirTravelerAvail", false);
                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;
                    XmlNode oNode = oDoc.CreateNode(XmlNodeType.Element, "", "AirTravelerAvail", "");
                    oNode.InnerXml = strRequest;
                    oRoot.AppendChild(oNode);
                    strResponse = oDoc.OuterXml;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Loading AirTravelerAvail into Native Response.\r\n{ex.Message}");
                }

                try
                {
                    // ********************************************************************
                    //  Transform Native AmadeusWS LowFarePlus Response into OTA Response   *
                    //  This transformation better organizes the AmadeusWS response to make *
                    //  easier to create and filter the final response                    *
                    // ******************************************************************** 
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_LowFare1RS.xsl", false);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_LowFare2RS.xsl", false);

                    // ***********************************************
                    //  process output business logic if necessary   *
                    // ***********************************************                 
                    strResponse = strResponse.Replace("TransactionIdentifier=\"Amadeus", $"TransactionIdentifier=\"Amadeus-{ttProviderSystems.PCC}");

                    if (!string.IsNullOrEmpty(ttProviderSystems.BLFile))
                    {
                        var oDoc = new XmlDocument();
                        //  Load Access Control List into memory
                        try
                        {
                            oDoc.Load(ttProviderSystems.BLFile);
                        }
                        catch (Exception exr)
                        {
                            CoreLib.SendTrace("", "AmadeusWS", "Error Loading LowFare business logic file", exr.Message, ttProviderSystems.LogUUID);
                            throw exr;
                        }

                        oRoot = oDoc.DocumentElement;
                        XmlNode oNode = oRoot.SelectSingleNode("Message[@Name=\'LowFare\'][@Direction=\'Out\']");
                        if (oNode != null)
                        {
                            //  check if non ticketable flights/fares to eliminate
                            var oBLNode = oNode.SelectSingleNode($"NoTktAirline[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");
                            if (oBLNode != null)
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareNoTktRS.xsl");

                            //  check if no mix airline to eliminate                        
                            oBLNode = oNode.SelectSingleNode($"NoMixAirline[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");
                            if (oBLNode != null)
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareNoMixRS.xsl");

                            //  add fare markup if needed
                            oBLNode = oNode.SelectSingleNode($"ProviderBL[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");
                            if (oBLNode != null)
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareRS.xsl");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }

                ResponseTime = DateTime.Now;

                if (ttProviderSystems.LogNative)
                {
                    TripXMLTools.TripXMLLog.LogMessage("LowFare", ref strMessage, RequestTime, ResponseTime, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                    //LogMessageToFile("LowFare", strMessage, RequestTime, ResponseTime);
                }
                return strResponse;
            }
            catch (Exception ex)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                throw ex;
            }
        }

        public string LowFarePlus()
        {
            DateTime RequestTime;
            DateTime ResponseTime;

            // ************************************************************
            //  Get the Filtering Elements from OTA LowFarePlus Request   *
            // ************************************************************
            try
            {
                RequestTime = DateTime.Now;
                string strRequest = Request;

                ttProviderSystems.PCC = ttProviderSystems.PCC.Replace("*", "");

                XmlDocument oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                XmlElement oRoot = oDoc.DocumentElement;
                XmlNode oNode = oRoot.SelectSingleNode("POS/Source/@PseudoCityCode");

                if (oNode != null)
                {
                    oNode.InnerText = ttProviderSystems.PCC;
                    strRequest = oRoot.OuterXml;
                }

                // *****************************************************************
                //  Transform OTA LowFarePlus Request into Native Amadeus Request     *
                // ***************************************************************** 
                strRequest = SetRequest($"AmadeusWS_LowFarePlusRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                AmadeusWSAdapter ttAA;
                string strResponse = "";
                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 
                ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();

                strResponse = SendMasterPricerTravelBoardSearch(ttAA, strRequest);

                if (ttProviderSystems.SessionPool)
                {
                    DateTime myDTFI = default(DateTime);
                    myDTFI = DateTime.Now;
                    if (ttProviderSystems.AddLFPStat)
                    {
                        DateTime myDTFIR = default(DateTime);
                        myDTFIR = DateTime.Now;

                        TimeSpan dur = default(TimeSpan);
                        dur = myDTFIR - myDTFI;

                        addLogStat(strRequest, ttProviderSystems.UserID, ttProviderSystems.PCC, myDTFI, myDTFIR, dur);
                    }
                }

                strMessage = $"{strRequest}{strResponse}";

                if (ttProviderSystems.LFPLight)
                {
                    strResponse = strResponse.Replace("</Fare_MasterPricerTravelBoardSearchReply>", "<LFPLight>Y</LFPLight></Fare_MasterPricerTravelBoardSearchReply>");
                }
                // ********************************************************************
                //  Transform Native Amadeus LowFarePlus Response into OTA Response   *
                //  This transformation better organizes the Amadeus response to make *
                //  easier to create and filter the final response                    *
                // ******************************************************************** 
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_LowFarePlus1RS.xsl", false);
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSService", "After 1st transform", $"Response {strResponse.Length} char. long", ttProviderSystems.LogUUID);
                // ********************************************************************
                //  Transform Native Amadeus LowFarePlus Response into OTA Response   *
                // ******************************************************************** 
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_LowFarePlus2RS.xsl", false);
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSService", "After 2nd transform", $"Response {strResponse.Length} char. long", ttProviderSystems.LogUUID);
                // ***********************************************
                //  process output business logic if necessary   *
                // *********************************************** 
                strResponse = strResponse.Replace("TransactionIdentifier=\"Amadeus", $"TransactionIdentifier=\"Amadeus-{ttProviderSystems.PCC}");

                if (!ttProviderSystems.LFPLight)
                {
                    #region business logic
                    if (!string.IsNullOrEmpty(ttProviderSystems.BLFile) && !strResponse.Contains("<Error"))
                    {
                        oDoc = new XmlDocument();
                        //  Load Access Control List into memory
                        oDoc.Load(ttProviderSystems.BLFile);

                        oRoot = oDoc.DocumentElement;
                        oNode = oRoot.SelectSingleNode("Message[@Name=\'LowFare\'][@Direction=\'Out\']");

                        if (oNode != null)
                        {
                            //  check if non ticketable flights/fares to eliminate
                            XmlNode oBLNode = oNode.SelectSingleNode($"NoTktAirline[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");
                            if (oBLNode != null)
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareNoTktRS.xsl");

                            //  check if no mix airline to eliminate                        
                            oBLNode = oNode.SelectSingleNode($"NoMixAirline[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");

                            if (oBLNode != null)
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareNoMixRS.xsl");

                            //  check if no fare type to eliminate
                            oBLNode = oNode.SelectSingleNode($"NoFareType[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");

                            if (oBLNode != null)
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareNoFareTypeRS.xsl");

                            //  add fare markup if needed                        
                            oBLNode = oNode.SelectSingleNode($"ProviderBL[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");

                            if (oBLNode != null)
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareRS.xsl");
                        }
                    }

                    #endregion
                }

                ResponseTime = DateTime.Now;

                if (ttProviderSystems.LogNative)
                    TripXMLTools.TripXMLLog.LogMessage("LowFarePlus", ref strMessage, RequestTime, ResponseTime, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);

                return strResponse;
            }
            catch (Exception ex)
            {
                addLog($"<M>{Request}</M><BL/>", ttProviderSystems.UserID);
                throw new Exception($"Exception Error.\r\n{ex.Message}\r\n{ex.InnerException}");
            }
        }

        public string LowFareMatrix()
        {
            DateTime RequestTime;
            DateTime ResponseTime;
            XmlNode oBLNode;
            // ************************************************************
            //  Get the Filtering Elements from OTA LowFareMatrix Request   *
            // ************************************************************
            try
            {
                RequestTime = DateTime.Now;

                XmlDocument oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                XmlElement oRoot = oDoc.DocumentElement;
                XmlNode oNode = oRoot.SelectSingleNode("POS/Source/@PseudoCityCode");
                string strRequest = "";

                if (oNode != null)
                {
                    oNode.InnerText = ttProviderSystems.PCC;
                    strRequest = oRoot.OuterXml;
                }

                // *****************************************************************
                //  Transform OTA LowFareMatrix Request into Native Amadeus Request     *
                // ***************************************************************** 
                strRequest = SetRequest($"AmadeusWS_LowFareMatrixRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                string strResponse;

                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 

                AmadeusWSAdapter ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
                ttAA.TracerID = _tracerID;

                strResponse = SendMasterPricerCalendar(ttAA, strRequest);
                strMessage = $"{strRequest}{strResponse}";

                // ********************************************************************
                //  Transform Native Amadeus LowFareMatrix Response into OTA Response   *
                //  This transformation better organizes the Amadeus response to make *
                //  easier to create and filter the final response                    *
                // ******************************************************************** 
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_LowFareMatrix1RS.xsl", false);

                // ********************************************************************
                //  Transform Native Amadeus LowFareMatrix Response into OTA Response   *
                // ******************************************************************** 
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_LowFareMatrix2RS.xsl", false);

                // ***********************************************
                //  process output business logic if necessary   *
                // ***********************************************             
                strResponse = strResponse.Replace("TransactionIdentifier=\"Amadeus", $"TransactionIdentifier=\"Amadeus-{ttProviderSystems.PCC}");

                if (!string.IsNullOrEmpty(ttProviderSystems.BLFile))
                {
                    oDoc = new XmlDocument();
                    //  Load Access Control List into memory
                    oDoc.Load(ttProviderSystems.BLFile);
                    oRoot = oDoc.DocumentElement;
                    oNode = oRoot.SelectSingleNode("Message[@Name=\'LowFare\'][@Direction=\'Out\']");

                    if (oNode != null)
                    {
                        //  check if non ticketable flights/fares to eliminate
                        oBLNode = oNode.SelectSingleNode($"NoTktAirline[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");

                        if (oBLNode != null)
                            strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareNoTktRS.xsl");

                        //  check if no mix airline to eliminate
                        oBLNode = oNode.SelectSingleNode($"NoMixAirline[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");


                        if (oBLNode != null)
                            strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareNoMixRS.xsl");

                        //  add fare markup if needed
                        oBLNode = oNode.SelectSingleNode($"ProviderBL[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");


                        if (oBLNode != null)
                            strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareRS.xsl");
                    }
                }

                ResponseTime = DateTime.Now;

                if (ttProviderSystems.LogNative)
                {
                    TripXMLTools.TripXMLLog.LogMessage("LowFareMatrix", ref strMessage, RequestTime, ResponseTime, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }

                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception Error.\r\n{ex.Message}");
            }
        }

        public string LowOfferMatrix()
        {
            string strResponse;
            string strRequest;
            XmlNode oBLNode;
            DateTime RequestTime;
            DateTime ResponseTime;
            // ************************************************************
            //  Get the Filtering Elements from OTA LowOfferMatrix Request   *
            // ************************************************************
            try
            {
                RequestTime = DateTime.Now;
                XmlDocument oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                XmlElement oRoot = oDoc.DocumentElement;
                XmlNode oNode = oRoot.SelectSingleNode("POS/Source/@PseudoCityCode");

                if (oNode != null)
                    oNode.InnerText = ttProviderSystems.PCC;

                strRequest = oNode != null ? oRoot.OuterXml : Request;

                // *****************************************************************
                //  Transform OTA LowOfferMatrix Request into Native Amadeus Request     *
                // ***************************************************************** 
                strRequest = SetRequest($"AmadeusWS_LowOfferMatrixRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                AmadeusWSAdapter ttAA;
                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 

                ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
                strResponse = SendSellByFareCalendar(ttAA, strRequest);
                strMessage = $"{strRequest}{strResponse}";

                // ********************************************************************
                //  Transform Native Amadeus LowOfferMatrix Response into OTA Response   *
                //  This transformation better organizes the Amadeus response to make *
                //  easier to create and filter the final response                    *
                // ******************************************************************** 
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_LowOfferMatrix1RS.xsl", false);

                // ********************************************************************
                //  Transform Native Amadeus LowOfferMatrix Response into OTA Response   *
                // ******************************************************************** 
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_LowOfferMatrix2RS.xsl", false);

                // ***********************************************
                //  process output business logic if necessary   *
                // *********************************************** 
                strResponse = strResponse.Replace("TransactionIdentifier=\"Amadeus", $"TransactionIdentifier=\"Amadeus-{ ttProviderSystems.PCC}");

                if (!string.IsNullOrEmpty(ttProviderSystems.BLFile))
                {
                    oDoc = new XmlDocument();
                    //  Load Access Control List into memory
                    oDoc.Load(ttProviderSystems.BLFile);

                    oRoot = oDoc.DocumentElement;
                    oNode = oRoot.SelectSingleNode("Message[@Name=\'LowFare\'][@Direction=\'Out\']");

                    if (oNode != null)
                    {
                        //  check if non ticketable flights/fares to eliminate
                        oBLNode = oNode.SelectSingleNode($"NoTktAirline[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");

                        if (oBLNode != null)
                            strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareNoTktRS.xsl");

                        //  check if no mix airline to eliminate
                        oBLNode = oNode.SelectSingleNode($"NoMixAirline[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");

                        if (oBLNode != null)
                            strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareNoMixRS.xsl");

                        //  add fare markup if needed
                        oBLNode = oNode.SelectSingleNode($"ProviderBL[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");


                        if (oBLNode != null)
                            strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareRS.xsl");
                    }
                }

                ResponseTime = DateTime.Now;

                if (ttProviderSystems.LogNative)
                {
                    TripXMLTools.TripXMLLog.LogMessage("LowOfferMatrix", ref strMessage, RequestTime, ResponseTime, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }

                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception Error.\r\n{ex.Message}");
            }
        }

        public string LowOfferSearch()
        {
            // ************************************************************
            //  Get the Filtering Elements from OTA LowOfferSearch Request   *
            // ************************************************************
            try
            {
                DateTime RequestTime = DateTime.Now;
                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                XmlElement oRoot = oDoc.DocumentElement;
                XmlNode oNode = oRoot.SelectSingleNode("POS/Source/@PseudoCityCode");

                if (oNode != null)
                    oNode.InnerText = ttProviderSystems.PCC;

                string strRequest = oNode != null ? oRoot.OuterXml : Request;

                // *****************************************************************
                //  Transform OTA LowOfferSearch Request into Native Amadeus Request     *
                // ***************************************************************** 
                strRequest = SetRequest($"AmadeusWS_LowOfferSearchRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 

                var ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
                ttAA.TracerID = _tracerID;

                string strResponse = SendSellByFareSearch(ttAA, strRequest);
                strMessage = $"{strRequest}{strResponse}";

                // ********************************************************************
                //  Transform Native Amadeus LowOfferSearch Response into OTA Response   *
                //  This transformation better organizes the Amadeus response to make *
                //  easier to create and filter the final response                    *
                // ******************************************************************** 
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_LowOfferSearch1RS.xsl", false);

                // ********************************************************************
                //  Transform Native Amadeus LowOfferSearch Response into OTA Response   *
                // ******************************************************************** 
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_LowOfferSearch2RS.xsl", false);

                // ***********************************************
                //  process output business logic if necessary   *
                // *********************************************** 
                strResponse = strResponse.Replace("TransactionIdentifier=\"Amadeus", $"TransactionIdentifier=\"Amadeus-{ttProviderSystems.PCC}");

                if (!string.IsNullOrEmpty(ttProviderSystems.BLFile))
                {
                    oDoc = new XmlDocument();
                    //  Load Access Control List into memory
                    oDoc.Load(ttProviderSystems.BLFile);
                    oRoot = oDoc.DocumentElement;
                    oNode = oRoot.SelectSingleNode("Message[@Name=\'LowFare\'][@Direction=\'Out\']");

                    if (oNode != null)
                    {
                        //  check if non ticketable flights/fares to eliminate
                        var oBLNode = oNode.SelectSingleNode($"NoTktAirline[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");

                        if (oBLNode != null)
                            strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareNoTktRS.xsl");

                        //  check if no mix airline to eliminate
                        oBLNode = oNode.SelectSingleNode($"NoMixAirline[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");


                        if (oBLNode != null)
                            strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareNoMixRS.xsl");

                        //  add fare markup if needed
                        oBLNode = oNode.SelectSingleNode($"ProviderBL[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");


                        if (oBLNode != null)
                            strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.XslPath}BL\\", "BL_LowFareRS.xsl");
                    }
                }

                if (ttProviderSystems.LogNative)
                    TripXMLTools.TripXMLLog.LogMessage("LowOfferSearch", ref strMessage, RequestTime, DateTime.Now, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);

                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception Error.\r\n{ ex.Message }");
            }
        }

        public string LowFareFlights()
        {

            string strSecondRequest = "";
            string strNewResponse = "";
            string strFirstResponse = "";
            string finalResp = "";

            XmlElement oRoot = null;

            XmlDocument oDocResp = null;
            XmlElement oRootResp = null;
            XmlElement oRootNewResp = null;
            XmlElement oRootFinal = null;
            XmlNode oBLNode = null;


            string strAvailResponses = "";
            string strFirstFlight = "";
            string strFinalResponse = "";
            
            int NCount = 0;
            
            int RNCount = 0;
            List<string> AClasses = new List<string>();
            List<string> FiltClasses = new List<string>();
            List<string> IndexFiltClass = new List<string>();
            string PricedItinerary = "";
            int iNIP = 0;
            
            string FlexToken = "";
            string EchoToken = "";
            string avTempResponse = "";
            string avFinalResponse = "";
            
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
            
            string cabinpref = "Economy";
            bool isReturn = true;
            int tempNoOfFlights = 0;
            bool inSession = false;
            // ************************************************************
            //  Get the Filtering Elements from OTA LowFareFlights Request   *
            // ************************************************************
            try
            {


                if (string.IsNullOrEmpty(ttProviderSystems.NoOfLowFareFlights) && (Int32.TryParse(ttProviderSystems.NoOfLowFareFlights.ToString(), out tempNoOfFlights)))
                {
                    outboundcnt = tempNoOfFlights;
                    inboundcnt = tempNoOfFlights;
                }

                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                oRoot = oDoc.DocumentElement;

                iNIP = Convert.ToInt32(oRoot.SelectSingleNode("TravelerInfoSummary/SeatsRequested").InnerText);

                strSecondRequest = Request;
                string strRequest = Request;

                foreach (XmlNode oNodeResp in oRoot.SelectNodes("SpecificFlightInfo/BookingClassPref[@ResBookDesigCode!='']"))
                {
                    AClasses.Add(oNodeResp.SelectSingleNode("@ResBookDesigCode").InnerText);
                }

                try
                {
                    cabinpref = oRoot.SelectSingleNode("TravelPreferences").SelectSingleNode("CabinPref").Attributes["Cabin"].Value;
                }
                catch (Exception)
                {
                    cabinpref = "Economy";
                }

                isReturn = oRoot.SelectNodes("OriginDestinationInformation").Count > 1;

                // *****************************************************************
                //  Transform OTA LowFareFlights Request into Native Amadeus Request     *
                // ***************************************************************** 
                string strResponse = SetRequest($"AmadeusWS_LowFareFlightsRQ.xsl");

                strMessage = $"{strRequest}{strResponse}";

                if (string.IsNullOrEmpty(strResponse))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 

                oDocResp = new XmlDocument();
                oDocResp.LoadXml(strResponse);
                oRootResp = oDocResp.DocumentElement;


                //  *******************
                //  Create Session    *
                //  *******************

                AmadeusWSAdapter ttAA = SetAdapter();
                ttAA.TracerID = _tracerID;
                inSession = SetConversationID(ttAA);

                foreach (XmlNode oNodeResp in oRootResp)
                {
                    string s1 = oNodeResp.OuterXml;
                    strResponse = SendAirMultiAvailability(ttAA,oNodeResp.OuterXml);
                    XmlDocument oDocRespN1 = new XmlDocument();
                    oDocRespN1.LoadXml(strResponse);
                    XmlElement oRootRespN1 = oDocRespN1.DocumentElement;
                    strAvailResponses += strResponse;                    
                }

                strAvailResponses = $"<AirAvail>{strAvailResponses}</AirAvail>";

                #region Filtering Request Classes

                if (oRoot.SelectSingleNode("SpecificFlightInfo/BookingClassPref") != null)
                {
                    oDocResp.LoadXml(strAvailResponses);
                    oRootResp = oDocResp.DocumentElement;
                    oRootNewResp = oDocResp.DocumentElement;

                    foreach (XmlNode oNodeResp in oRootResp)
                    {
                        NCount++;
                        int FCount = oNodeResp.SelectNodes("singleCityPairInfo/flightInfo").Count;

                        for (int k = 1; k <= FCount; k++)
                        {

                            string tst1 = oNodeResp.OuterXml;
                            int FClassCount = oNodeResp.SelectNodes("singleCityPairInfo/flightInfo[position()=" + k.ToString() + "]/infoOnClasses").Count;


                            if (FClassCount > 0)
                            {
                                for (int i = 1; i <= FClassCount; i++)
                                {
                                    string test7 = oNodeResp.SelectSingleNode("singleCityPairInfo/flightInfo[position()=" + k.ToString() + "]/infoOnClasses[position()=" + i.ToString() + "]/productClassDetail/serviceClass").InnerText;
                                    if (!AClasses.Contains(oNodeResp.SelectSingleNode("singleCityPairInfo/flightInfo[position()=" + k.ToString() + "]/infoOnClasses[position()=" + i.ToString() + "]/productClassDetail/serviceClass").InnerText))
                                    {
                                        oRootNewResp.SelectSingleNode("Air_MultiAvailabilityReply[position()=" + NCount.ToString() + "]/singleCityPairInfo/flightInfo[position()=" + k.ToString() + "]/infoOnClasses[position()=" + i.ToString() + "]").RemoveAll();
                                        RNCount++;
                                        strAvailResponses = oRootNewResp.OuterXml;
                                    }
                                    else
                                    {
                                        int iAS = 0;
                                        iAS = Convert.ToInt32(oNodeResp.SelectSingleNode("singleCityPairInfo/flightInfo[position()=" + k.ToString() + "]/infoOnClasses[position()=" + i.ToString() + "]/productClassDetail/availabilityStatus").InnerText);

                                        if (iNIP > iAS)
                                        {
                                            oRootNewResp.SelectSingleNode("Air_MultiAvailabilityReply[position()=" + NCount.ToString() + "]/singleCityPairInfo/flightInfo[position()=" + k.ToString() + "]/infoOnClasses[position()=" + i.ToString() + "]").RemoveAll();
                                            RNCount++;
                                            strAvailResponses = oRootNewResp.OuterXml;
                                        }
                                    }
                                }

                                if (FClassCount == RNCount)
                                {
                                    oRootNewResp.SelectSingleNode("Air_MultiAvailabilityReply[position()=" + NCount.ToString() + "]/singleCityPairInfo/flightInfo[position()=" + k.ToString() + "]").RemoveAll();
                                }

                                RNCount = 0;
                            }
                            else
                                break;

                        }
                    }

                    strAvailResponses = oRootNewResp.OuterXml;
                }
                #endregion
                strAvailResponses = strAvailResponses.Replace("<flightInfo></flightInfo>", "");
                oDocResp.LoadXml(strAvailResponses);
                oRootResp = oDocResp.DocumentElement;
                
                if (oRootResp.SelectSingleNode("Air_MultiAvailabilityReply/singleCityPairInfo/flightInfo/basicFlightInfo/flightDetails") != null
                    && strAvailResponses.IndexOf("NO NEGOTIATED SPACE IS AVAILABLE") == -1 && strAvailResponses.IndexOf("NO AVAILABILITY FOR SELECTED PREFERENCE") == -1
                    && (strAvailResponses.IndexOf("<Air_MultiAvailabilityReply/>") == -1 && strAvailResponses.IndexOf("<Air_MultiAvailabilityReply></Air_MultiAvailabilityReply>") == -1))
                {
                    #region "Avianca change"

                    if (oRoot.SelectSingleNode("TravelPreferences/VendorPref") != null && oRoot.SelectSingleNode("TravelPreferences/VendorPref").Attributes["Code"] != null && ((oRoot.SelectSingleNode("TravelPreferences/VendorPref").Attributes["Code"].Value.ToUpper() == "AV") || (oRoot.SelectSingleNode("TravelPreferences/VendorPref").Attributes["Code"].Value.ToUpper() == "TA") || (oRoot.SelectSingleNode("TravelPreferences/VendorPref").Attributes["Code"].Value.ToUpper() == "UL") || (oRoot.SelectSingleNode("TravelPreferences/VendorPref").Attributes["Code"].Value.ToUpper() == "Z8")))
                    {

                        if (oRootResp.SelectSingleNode("Air_MultiAvailabilityReply/singleCityPairInfo/flightInfo").InnerXml != "")
                        {
                            string tt = oRootResp.SelectSingleNode("Air_MultiAvailabilityReply/singleCityPairInfo/flightInfo").InnerXml;

                            int outBoundCnt = 0;
                            int inBoundCnt = 0;

                            outBoundCnt = oRootResp.SelectNodes("Air_MultiAvailabilityReply[1]/singleCityPairInfo/flightInfo[basicFlightInfo/productTypeDetail/productIndicators='D' or basicFlightInfo/productTypeDetail/productIndicators='S']").Count;
                            if (isReturn)
                                inBoundCnt = oRootResp.SelectNodes("Air_MultiAvailabilityReply[2]/singleCityPairInfo/flightInfo[basicFlightInfo/productTypeDetail/productIndicators='D' or basicFlightInfo/productTypeDetail/productIndicators='S']").Count;

                            if (outBoundCnt > outboundcnt)
                            {
                                outBoundCntTemp = outboundcnt;
                            }
                            else
                            {
                                outBoundCntTemp = outBoundCnt;
                            }
                            if (isReturn)
                            {


                                if (inBoundCnt > inboundcnt)
                                {
                                    inBoundCntTemp = inboundcnt;
                                }
                                else
                                {
                                    inBoundCntTemp = inBoundCnt;
                                }
                            }
                            else
                            {
                                inBoundCnt = 1;
                                inBoundCntTemp = inBoundCnt;
                            }


                            priceTags = new string[outboundcnt, inboundcnt];
                            PricingSource = new string[outboundcnt, inboundcnt];
                            NegoCode = new string[outboundcnt, inboundcnt];
                            TicketTimeLimit = new string[outboundcnt, inboundcnt];
                            OutClass = new string[outboundcnt, inboundcnt];
                            InClass = new string[outboundcnt, inboundcnt];
                            flSegments = new string[outboundcnt, inboundcnt];

                            strReq = new string[26];
                            bPriceEnd = new bool[26];
                            OTAPriceTotal = new string[25];
                            OTAReqTotal = new string[25];
                            avTempResponseTotal = new string[25];


                            XmlNode[,] BaseFare = new XmlNode[outboundcnt, inboundcnt];
                            XmlNode[,] TotalTax = new XmlNode[outboundcnt, inboundcnt];
                            XmlNode[,] TotalFare = new XmlNode[outboundcnt, inboundcnt];
                            XmlNode[,] PriceBreakDown = new XmlNode[outboundcnt, inboundcnt];

                            string query = "<OTA_AirLowFareSearchFlightsRS><PricedItineraries><PricedItinerary SequenceNumber=\"0\"><AirItinerary DirectionInd=\"Circle\"><OriginDestinationOptions><OriginDestinationOption SectorSequence=\"1\"><FlightSegment DepartureDateTime=\"2000-01-07T10:15:00\" ArrivalDateTime=\"2000-01-07T13:50:00\" StopQuantity=\"0\" RPH=\"D\" FlightNumber=\"38\" ResBookDesigCode=\"111111\" NumberInParty=\"1\" E_TicketEligibility=\"Eligible\">" +
                                             "<DepartureAirport LocationCode=\"CLO\"/><ArrivalAirport LocationCode=\"MIA\"/><OperatingAirline Code=\"AV\"/><Equipment AirEquipType=\"000\"/><MarketingAirline Code=\"AV\"/><TPA_Extensions><CabinType Cabin=\"Economy\"/><JourneyTotalDuration>03:35</JourneyTotalDuration>" +
                                             "</TPA_Extensions></FlightSegment></OriginDestinationOption><OriginDestinationOption SectorSequence=\"2\"><FlightSegment DepartureDateTime=\"2000-01-07T10:15:00\" ArrivalDateTime=\"2000-01-07T13:50:00\" StopQuantity=\"0\" RPH=\"D\" FlightNumber=\"38\" ResBookDesigCode=\"Q\" NumberInParty=\"1\" E_TicketEligibility=\"Eligible\">" +
                                             "<DepartureAirport LocationCode=\"CLO\"/><ArrivalAirport LocationCode=\"MIA\"/><OperatingAirline Code=\"AV\"/><Equipment AirEquipType=\"000\"/><MarketingAirline Code=\"AV\"/><TPA_Extensions><CabinType Cabin=\"Economy\"/><JourneyTotalDuration>03:35</JourneyTotalDuration>" +
                                             "</TPA_Extensions></FlightSegment></OriginDestinationOption></OriginDestinationOptions></AirItinerary><AirItineraryPricingInfo PricingSource=\"Published\" ValidatingAirlineCode=\"AV\"><ItinTotalFare><BaseFare Amount=\"00000\" CurrencyCode=\"COP\" DecimalPlaces=\"0\"/><Taxes><Tax TaxCode=\"TotalTax\" Amount=\"00000\" CurrencyCode=\"COP\" DecimalPlaces=\"0\"/>" +
                                             "</Taxes><TotalFare Amount=\"00000\" CurrencyCode=\"COP\" DecimalPlaces=\"0\"/></ItinTotalFare><FareInfos><FareInfo><DepartureDate/><FareReference>Q</FareReference><FilingAirline/><DepartureAirport LocationCode=\"\"/><ArrivalAirport LocationCode=\"\"/></FareInfo><FareInfo><DepartureDate/><FareReference>V</FareReference><FilingAirline/><DepartureAirport LocationCode=\"\"/><ArrivalAirport LocationCode=\"\"/></FareInfo></FareInfos></AirItineraryPricingInfo><TicketingInfo TicketTimeLimit=\"2000-12-11T23:59:00\"/></PricedItinerary></PricedItineraries></OTA_AirLowFareSearchFlightsRS>";

                            if (isReturn)
                            {
                                Thread[] oDbThread = new Thread[25];

                                for (int i = 1; i <= outBoundCntTemp; i++)
                                {
                                    for (int j = 1; j <= inBoundCntTemp; j++)
                                    {
                                        strFirstFlight = "";
                                        int segmentCount = 1;
                                        //// outbound ////////
                                        strFirstFlight += "<OD>";
                                        XmlNode oNodeResp = oRootResp.SelectSingleNode("Air_MultiAvailabilityReply[1]");

                                        var oNode = oNodeResp.SelectSingleNode("singleCityPairInfo/flightInfo[basicFlightInfo/productTypeDetail/productIndicators='D' or basicFlightInfo/productTypeDetail/productIndicators='S'][" + i.ToString() + "]");
                                        strFirstFlight += oNode.OuterXml;

                                        if (oNode.SelectSingleNode("basicFlightInfo/productTypeDetail/productIndicators").InnerText == "S")
                                        {
                                            XmlNodeList oFlightInfoList = oNodeResp.SelectNodes("singleCityPairInfo/flightInfo");
                                            int fiIndex = 1;

                                            foreach (XmlNode oFlightInfo in oFlightInfoList)
                                            {
                                                if (oFlightInfo.SelectSingleNode("basicFlightInfo/productTypeDetail/productIndicators").InnerText == "S")
                                                {
                                                    if (oNode.InnerXml == oFlightInfo.InnerXml)
                                                    {
                                                        for (int m = fiIndex; m <= oFlightInfoList.Count; m++)
                                                        {
                                                            strFirstFlight += oFlightInfoList[m].OuterXml;
                                                            segmentCount += 1;

                                                            if (oFlightInfoList[m].SelectSingleNode("basicFlightInfo/productTypeDetail/productIndicators").InnerText == "E")
                                                            {
                                                                break;
                                                            }
                                                        }

                                                        break;
                                                    }
                                                }

                                                fiIndex += 1;
                                            }
                                        }

                                        flSegments[i - 1, j - 1] = segmentCount.ToString();
                                        segmentCount = 1;

                                        strFirstFlight += "</OD>";
                                        /////////////

                                        //// inbound ////////
                                        strFirstFlight += "<OD>";
                                        oNodeResp = oRootResp.SelectSingleNode("Air_MultiAvailabilityReply[2]");
                                        oNode = oNodeResp.SelectSingleNode("singleCityPairInfo/flightInfo[basicFlightInfo/productTypeDetail/productIndicators='D' or basicFlightInfo/productTypeDetail/productIndicators='S'][" + j.ToString() + "]");
                                        strFirstFlight += oNode.OuterXml;

                                        if (oNode.SelectSingleNode("basicFlightInfo/productTypeDetail/productIndicators").InnerText == "S")
                                        {
                                            XmlNodeList oFlightInfoList = oNodeResp.SelectNodes("singleCityPairInfo/flightInfo");
                                            int fiIndex = 1;

                                            foreach (XmlNode oFlightInfo in oFlightInfoList)
                                            {
                                                if (oFlightInfo.SelectSingleNode("basicFlightInfo/productTypeDetail/productIndicators").InnerText == "S")
                                                {
                                                    if (oNode.InnerXml == oFlightInfo.InnerXml)
                                                    {
                                                        for (int m = fiIndex; m <= oFlightInfoList.Count; m++)
                                                        {
                                                            strFirstFlight += oFlightInfoList[m].OuterXml;
                                                            segmentCount += 1;

                                                            if (oFlightInfoList[m].SelectSingleNode("basicFlightInfo/productTypeDetail/productIndicators").InnerText == "E")
                                                            {
                                                                break;
                                                            }
                                                        }

                                                        break;
                                                    }
                                                }

                                                fiIndex += 1;
                                            }
                                        }

                                        flSegments[i - 1, j - 1] += segmentCount.ToString();
                                        strFirstFlight += "</OD>";


                                        /////////////
                                        // ********************************************************************
                                        //  Transform Native Amadeus response into OTA air price request      *
                                        // ******************************************************************** 
                                        strRequest = Request.Replace("</OTA_AirLowFareSearchFlightsRQ>", $"{strFirstFlight}</OTA_AirLowFareSearchFlightsRQ>");

                                        strRequest = CoreLib.TransformXML(strRequest, XslPath, $"{Version}AmadeusWS_LowFareFlights1RQ.xsl", false);
                                        
                                        strRequestTotal = strRequest;
                                        oDbThread[iFareSearches] = new Thread(new ThreadStart(SearchFares));
                                        oDbThread[iFareSearches].Start();

                                        while (!oDbThread[iFareSearches].IsAlive) ;
                                        Thread.Sleep(10);

                                        iFareSearches += 1;
                                    }
                                }
                                iFareSearches = iFareSearches - 1;
                                DateTime StartCounter = DateTime.Now;

                                while (iFinishedPrices < (outBoundCntTemp * inBoundCntTemp))
                                {
                                    if (Convert.ToInt32(DateTime.Now.Subtract(StartCounter).TotalSeconds) > 60)
                                        break;
                                    Thread.Sleep(1000);
                                }

                                string ORT = "";
                                string OPT = "";
                                string aTR = "";
                                string OTOT = "";
                                for (int k = 0; k < (outBoundCntTemp * inBoundCntTemp); k++)
                                {
                                    ORT += OTAReqTotal[k];
                                    OPT += OTAPriceTotal[k];
                                    aTR += avTempResponseTotal[k];
                                }

                                OTOT = "<a>" + ORT + aTR + OPT + "</a>";

                                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSService", "strResponseTotal", OTOT, ttProviderSystems.LogUUID);
                                firstPrice = avTempResponseTotal[0];
                                iFareSearches = 0;


                                for (int i = 1; i <= outBoundCntTemp; i++)
                                {
                                    for (int j = 1; j <= inBoundCntTemp; j++)
                                    {
                                        if (OTAPriceTotal[iFareSearches] == null || OTAPriceTotal[iFareSearches].Length == 0)
                                        {
                                            OTAPriceTotal[iFareSearches] = "<OTA_AirPriceRS Version=\"2003.2\"><Errors><Error Type=\"Amadeus\" Code=\"911\">NO FARE FOR BOOKING CODE-TRY OTHER PRICING OPTIONS</Error></Errors></OTA_AirPriceRS>";
                                        }
                                        oPDoc = new XmlDocument();
                                        oPDoc.LoadXml(OTAPriceTotal[iFareSearches]);
                                        oPRoot = oPDoc.DocumentElement;
                                        string strTemp = null;
                                        if (oPRoot != null && oPRoot.FirstChild.LocalName != "Errors")
                                        {
                                            string pRes = CoreLib.TransformXML($"<FIP>{strRequest}{avTempResponseTotal[iFareSearches]}</FIP>", XslPath, $"{Version}AmadeusWS_LowFareFlights1RS.xsl", false);
                                            pRes = pRes.Replace("</Fare_InformativeBestPricingWithoutPNRReply>", oRoot.SelectSingleNode("TravelerInfoSummary/AirTravelerAvail").OuterXml + strFirstFlight + "</Fare_InformativeBestPricingWithoutPNRReply>");

                                            pRes = $"<FS>{strAvailResponses}{pRes}</FS>";
                                            string presF = CoreLib.TransformXML(pRes, XslPath, $"{Version}AmadeusWS_LowFareFlightsRS.xsl", false);                                            

                                            oPriceDoc = new XmlDocument();
                                            oPriceDoc.LoadXml(presF);
                                            oPriceRoot = oPriceDoc.DocumentElement;

                                            strTemp = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").InnerXml;
                                            XmlNodeList fareInfos = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("FareInfos").SelectNodes("FareInfo");
                                            int iOut = int.Parse(flSegments[i - 1, j - 1].Substring(0, 1));

                                            if (oPriceRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo/PTC_FareBreakdowns") != null)
                                            {
                                                fareBasisCodes.Add(oPriceRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo/PTC_FareBreakdowns"));

                                            }

                                            for (int v = 0; v < iOut; v++)
                                            {
                                                OutClass[i - 1, j - 1] += fareInfos[v].SelectSingleNode("FareReference").InnerText;
                                            }

                                            int iIn = int.Parse(flSegments[i - 1, j - 1].Substring(1)) + iOut;

                                            for (int v = iOut; v < iIn; v++)
                                            {
                                                InClass[i - 1, j - 1] += fareInfos[v].SelectSingleNode("FareReference").InnerText;
                                            }

                                        }
                                        else
                                        {
                                            fareBasisCodes.Add(oPRoot.FirstChild);
                                            XmlDocument tempDoc = new XmlDocument();
                                            tempDoc.LoadXml(query);
                                            oPRoot = tempDoc.DocumentElement;

                                            strTemp = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").InnerXml;
                                            
                                            OutClass[i - 1, j - 1] = "00000000";
                                            InClass[i - 1, j - 1] = "111111111111111";


                                        }
                                        priceTags[i - 1, j - 1] = strTemp;
                                        PricingSource[i - 1, j - 1] = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").Attributes["PricingSource"].Value;
                                        if (oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").Attributes["NegotiatedFareCode"] != null)
                                        {
                                            NegoCode[i - 1, j - 1] = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").Attributes["NegotiatedFareCode"].Value;
                                        }
                                        else
                                        {
                                            NegoCode[i - 1, j - 1] = "";
                                        }
                                        TicketTimeLimit[i - 1, j - 1] = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("TicketingInfo").Attributes["TicketTimeLimit"].Value;

                                        BaseFare[i - 1, j - 1] = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare");
                                        TotalTax[i - 1, j - 1] = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").SelectSingleNode("Tax");
                                        TotalTax[i - 1, j - 1].Attributes.RemoveNamedItem("TaxCode");
                                        TotalFare[i - 1, j - 1] = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("TotalFare");

                                        iFareSearches += 1;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 1; i <= outBoundCntTemp; i++)
                                {
                                    strFirstFlight = "";


                                    //// outbound ////////
                                    strFirstFlight += "<OD>";

                                    XmlNode oNodeResp = oRootResp.SelectSingleNode("Air_MultiAvailabilityReply[1]");

                                    var oNode = oNodeResp.SelectSingleNode("singleCityPairInfo/flightInfo[basicFlightInfo/productTypeDetail/productIndicators='D' or basicFlightInfo/productTypeDetail/productIndicators='S'][" + i.ToString() + "]");
                                    strFirstFlight += oNode.OuterXml;

                                    if (oNode.SelectSingleNode("basicFlightInfo/productTypeDetail/productIndicators").InnerText == "S")
                                    {
                                        XmlNodeList oFlightInfoList = oNodeResp.SelectNodes("singleCityPairInfo/flightInfo");
                                        int fiIndex = 1;

                                        foreach (XmlNode oFlightInfo in oFlightInfoList)
                                        {
                                            if (oFlightInfo.SelectSingleNode("basicFlightInfo/productTypeDetail/productIndicators").InnerText == "S")
                                            {
                                                if (oNode.InnerXml == oFlightInfo.InnerXml)
                                                {
                                                    for (int m = fiIndex; m <= oFlightInfoList.Count; m++)
                                                    {
                                                        strFirstFlight += oFlightInfoList[m].OuterXml;
                                                        //segmentCount += 1;

                                                        if (oFlightInfoList[m].SelectSingleNode("basicFlightInfo/productTypeDetail/productIndicators").InnerText == "E")
                                                        {
                                                            break;
                                                        }
                                                    }

                                                    break;
                                                }
                                            }

                                            fiIndex += 1;
                                        }
                                    }

                                    strFirstFlight += "</OD>";
                                    /////////////
                                    // ********************************************************************
                                    //  Transform Native Amadeus response into OTA air price request      *
                                    // ******************************************************************** 
                                    strRequest = Request.Replace("</OTA_AirLowFareSearchFlightsRQ>", $"{strFirstFlight}</OTA_AirLowFareSearchFlightsRQ>");


                                    strRequest = CoreLib.TransformXML(strRequest, XslPath, $"{Version}AmadeusWS_LowFareFlights1RQ.xsl", false);                                    

                                    avTempResponse = SendInformativePricingWithoutPNR(ttAA,strRequest);
                                    
                                    avTempResponse = CoreLib.TransformXML($"<FIP>{strRequest}{avTempResponse}</FIP>", XslPath, $"{strFirstFlight}</AmadeusWS_LowFareFlights1RS>", false);

                                    OTAPrice = CoreLib.TransformXML(avTempResponse, XslPath, "AmadeusWS_AirPriceRS.xsl", false);

                                    avFinalResponse = avFinalResponse + avTempResponse;

                                    oPDoc = new XmlDocument();
                                    oPDoc.LoadXml(OTAPrice);
                                    oPRoot = oPDoc.DocumentElement;

                                    if (oPRoot != null && oPRoot.FirstChild.LocalName != "Errors")
                                    {
                                        string pRes = CoreLib.TransformXML($"<FIP>{strRequest}{avTempResponse}</FIP>", XslPath, $"{Version}AmadeusWS_LowFareFlights1RS.xsl", false);
                                        

                                        pRes = pRes.Replace("</Fare_InformativeBestPricingWithoutPNRReply>", oRoot.SelectSingleNode("TravelerInfoSummary/AirTravelerAvail").OuterXml + strFirstFlight + "</Fare_InformativeBestPricingWithoutPNRReply>");

                                        pRes = $"<FS>{strAvailResponses}{pRes}</FS>";
                                        string presF = CoreLib.TransformXML(pRes, XslPath, $"{Version}AmadeusWS_LowFareFlightsRS.xsl", false);                                        

                                        oPriceDoc = new XmlDocument();
                                        oPriceDoc.LoadXml(presF);
                                        oPriceRoot = oPriceDoc.DocumentElement;

                                        XmlNodeList fareInfos = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("FareInfos").SelectNodes("FareInfo");
                                        for (int v = 0; v < fareInfos.Count; v++)
                                        {
                                            OutClass[i - 1, 0] += fareInfos[v].InnerText;

                                        }

                                        if (oPriceRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo/PTC_FareBreakdowns") != null)
                                        {
                                            fareBasisCodes.Add(oPriceRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo/PTC_FareBreakdowns"));

                                        }
                                    }
                                    else
                                    {
                                        oPDoc = new XmlDocument();
                                        oPDoc.LoadXml(query);
                                        oPRoot = oPDoc.DocumentElement;

                                        OutClass[i - 1, 0] = "0000000000";


                                    }
                                    string strTemp = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").InnerXml;
                                    //strTemp = oPNode.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").InnerXml;

                                    priceTags[i - 1, 0] = strTemp;
                                    PricingSource[i - 1, 0] = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").Attributes["PricingSource"].Value;
                                    if (oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").Attributes["NegotiatedFareCode"] != null)
                                    {
                                        NegoCode[i - 1, 0] = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").Attributes["NegotiatedFareCode"].Value;
                                    }
                                    else
                                    {
                                        NegoCode[i - 1, 0] = "";
                                    }
                                    TicketTimeLimit[i - 1, 0] = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("TicketingInfo").Attributes["TicketTimeLimit"].Value;




                                    BaseFare[i - 1, 0] = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare");
                                    TotalTax[i - 1, 0] = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").SelectSingleNode("Tax");
                                    TotalTax[i - 1, 0].Attributes.RemoveNamedItem("TaxCode");
                                    TotalFare[i - 1, 0] = oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Item(0).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("TotalFare");

                                    if (i == 1)
                                    {
                                        firstPrice = avTempResponse;
                                    }

                                }
                            }

                            int loopcnt2 = 0;
                            if (firstPrice != null && firstPrice != "")
                            {


                                if (firstPrice.ToUpper().IndexOf("NO FARE FOR BOOKING CODE-TRY OTHER PRICING") > -1 || firstPrice.ToUpper().IndexOf("REQUESTED CABIN NOT AVAILABLE") > -1)
                                {
                                    for (int i = 1; i < avTempResponseTotal.Length - 1; i++)
                                    {
                                        loopcnt2++;
                                        firstPrice = avTempResponseTotal[i];
                                        if (firstPrice != null && firstPrice.Length > 0)
                                        {
                                            if (firstPrice.ToUpper().IndexOf("NO FARE FOR BOOKING CODE-TRY OTHER PRICING") < 0 && firstPrice.ToUpper().IndexOf("REQUESTED CABIN NOT AVAILABLE") < 0)
                                            {
                                                loopcnt2--;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            firstPrice = avTempResponseTotal[0];
                                        }
                                        if (i == avTempResponseTotal.Length - 1)
                                        {
                                            loopcnt2 = 0;
                                            firstPrice = avTempResponseTotal[0];
                                        }
                                    }
                                }
                            }


                            firstPrice = CoreLib.TransformXML($"<FIP>{strRequest}{firstPrice}</FIP>", XslPath, $"{Version}AmadeusWS_LowFareFlights1RS.xsl", false);
                            

                            firstPrice = firstPrice.Replace("</Fare_InformativeBestPricingWithoutPNRReply>", oRoot.SelectSingleNode("TravelerInfoSummary/AirTravelerAvail").OuterXml + strFirstFlight + "</Fare_InformativeBestPricingWithoutPNRReply>");
                            strFinalResponse = $"<FIP>{strAvailResponses}{firstPrice}</FS>";
                            strResponse = CoreLib.TransformXML(strFinalResponse, XslPath, $"{Version}AmadeusWS_LowFareFlightsRS.xsl", false);
                            
                            oPDoc = new XmlDocument();
                            oPDoc.LoadXml(strResponse);
                            strResponse = strResponse.Replace("<OTA_AirLowFareSearchFlightsRS", "<OTA_AirLowFareSearchFlightsRS EchoToken=\"FLTS\"");
                            EchoToken = "FLTS";

                            oPRoot = oPDoc.DocumentElement;
                            List<XmlNode> outboundNodes = new List<XmlNode>();
                            List<XmlNode> inboundNodes = new List<XmlNode>();

                            if (oPRoot.FirstChild.LocalName != "Errors")
                            {
                                foreach (XmlNode nd in oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary"))
                                {
                                    //including first flight segment(from 1st version)
                                    if (nd.Attributes["SequenceNumber"].Value == "1")
                                    {

                                        if (oPRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Count == 1)
                                        {
                                            foreach (XmlNode chNonde in nd.SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption").Item(0).SelectNodes("FlightSegment"))
                                            {

                                                outboundNodes.Add(chNonde);


                                            }
                                            if (isReturn)
                                            {
                                                foreach (XmlNode chNonde in nd.SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption").Item(1).SelectNodes("FlightSegment"))
                                                {

                                                    inboundNodes.Add(chNonde);
                                                }
                                            }
                                        }
                                        //fistcount = true;

                                    }
                                    else
                                    {
                                        foreach (XmlNode chNonde in nd.SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption").Item(0).SelectNodes("FlightSegment"))
                                        {
                                            //XmlNode outNode = chNonde;
                                            // chNonde.Attributes["RPH"].Value = "1";
                                            outboundNodes.Add(chNonde);

                                        }
                                        if (isReturn)
                                        {
                                            foreach (XmlNode chNonde in nd.SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption").Item(1).SelectNodes("FlightSegment"))
                                            {
                                                inboundNodes.Add(chNonde);
                                            }
                                        }
                                    }
                                }


                                oPRoot.RemoveChild(oPRoot.SelectSingleNode("PricedItineraries"));

                                string test = "<PricedItineraries>";

                                string prepareChild = "";

                                //int outTraverseCount = 0;
                                string depCity = outboundNodes[0].SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value;
                                int prevPriceIt = 0;

                                List<string> lstOutNodes = new List<string>();
                                List<string> lstOriginClasses = new List<string>();
                                List<string> lstFareBasisCodes = new List<string>();

                                string[,] strOriginClasses2 = new string[outboundcnt, inboundcnt];

                                string strOC = $"<OriginClass Index=\"$\" Cabin=\"{cabinpref}\">-</OriginClass>";

                                string depstring = "";
                                depCity = outboundNodes[0].SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value;
                                int fIndex = 1;
                                int ocIndex = 1;
                                int ocIndex2 = 1;
                                string strOriginClass = "";
                                string strOriginClass2 = "";

                                for (int i = 1; i <= outboundNodes.Count; i++)
                                {
                                    if (i == 1 || depCity != outboundNodes[i - 1].SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value)
                                    {
                                        if (fIndex <= outBoundCntTemp)
                                        {

                                            if (isReturn)
                                                outboundNodes[i - 1].Attributes["ResBookDesigCode"].Value = OutClass[fIndex - 1, 0].Substring(ocIndex2 - 1, 1);
                                            else
                                                outboundNodes[i - 1].Attributes["ResBookDesigCode"].Value = OutClass[fIndex - 1, 0].Substring(0, 1);



                                            ocIndex2 += 1;
                                        }
                                        else
                                            outboundNodes[i - 1].Attributes["ResBookDesigCode"].Value = "";

                                        prepareChild = outboundNodes[i - 1].OuterXml;

                                        if (fIndex <= outBoundCntTemp)
                                        {
                                            if (isReturn)
                                                strOriginClass += strOC.Replace("$", ocIndex.ToString()).Replace("-", OutClass[fIndex - 1, 0].Substring(ocIndex - 1, 1));
                                            else
                                                strOriginClass += strOC.Replace("$", ocIndex.ToString()).Replace("-", OutClass[fIndex - 1, 0].Substring(0, 1));

                                            if (isReturn)
                                            {
                                                for (int j = 0; j < inBoundCntTemp; j++)
                                                {
                                                    for (int k = 1; k <= OutClass[fIndex - 1, j].Length; k++)
                                                    {
                                                        strOriginClass2 += strOC.Replace("$", k.ToString()).Replace("-", OutClass[fIndex - 1, j].Substring(k - 1, 1));
                                                    }
                                                    //strOriginClass2 = strOC.Replace("$", ocIndex.ToString()).Replace("-", OutClass[fIndex - 1, j].Substring(ocIndex - 1, 1));
                                                    strOriginClasses2[fIndex - 1, j] = strOriginClass2;
                                                    strOriginClass2 = "";
                                                }
                                            }
                                            ocIndex += 1;

                                            string fareBasis = "";
                                            //fareBasis = fareBasisCodes[i - 1].OuterXml;
                                            for (int k = 0; k < fareBasisCodes[outBoundCntTemp - 1].SelectNodes("PTC_FareBreakdown").Count; k++)
                                            {
                                                fareBasis += "<FareBasisCodes PassengerType=\"" + fareBasisCodes[outBoundCntTemp - 1].SelectNodes("PTC_FareBreakdown")[k].SelectSingleNode("PassengerTypeQuantity").Attributes["Code"].Value + "\">";
                                                for (int y = 0; y < fareBasisCodes[outBoundCntTemp - 1].SelectNodes("PTC_FareBreakdown")[k].SelectSingleNode("FareBasisCodes").SelectNodes("FareBasisCode").Count; y++)
                                                {
                                                    fareBasis += "<FareBasisCode>" + fareBasisCodes[outBoundCntTemp - 1].SelectNodes("PTC_FareBreakdown")[k].SelectSingleNode("FareBasisCodes").SelectNodes("FareBasisCode")[y].InnerText + "</FareBasisCode>";
                                                }
                                                fareBasis += "</FareBasisCodes>";
                                            }

                                            string strFares = BaseFare[fIndex - 1, 0].OuterXml.Replace("BaseFare", "FromTotalBaseFare") + TotalTax[fIndex - 1, 0].OuterXml.Replace("Tax", "FromTotalTax") + TotalFare[fIndex - 1, 0].OuterXml.Replace("TotalFare", "FromTotalFare");
                                            prepareChild = prepareChild.Replace("</TPA_Extensions>", strFares + fareBasis + "</TPA_Extensions>");
                                        }

                                        depstring += prepareChild;
                                        if (depCity == "")
                                        {
                                            depCity = outboundNodes[i - 1].SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value;
                                        }
                                    }
                                    else
                                    {
                                        lstOutNodes.Add(depstring);
                                        depCity = "";
                                        depstring = "";
                                        fIndex += 1;

                                        if (fIndex <= outBoundCntTemp)
                                        {

                                            ocIndex2 = 1;
                                            outboundNodes[i - 1].Attributes["ResBookDesigCode"].Value = OutClass[fIndex - 1, 0].Substring(ocIndex2 - 1, 1);
                                            ocIndex2 += 1;
                                        }
                                        else
                                            outboundNodes[i - 1].Attributes["ResBookDesigCode"].Value = "";

                                        prepareChild = outboundNodes[i - 1].OuterXml;

                                        if (fIndex <= outBoundCntTemp)
                                        {
                                            lstOriginClasses.Add(strOriginClass);
                                            ocIndex = 1;
                                            strOriginClass = strOC.Replace("$", ocIndex.ToString()).Replace("-", OutClass[fIndex - 1, 0].Substring(ocIndex - 1, 1));

                                            if (isReturn)
                                            {
                                                for (int j = 0; j < inBoundCntTemp; j++)
                                                {
                                                    for (int k = 1; k <= OutClass[fIndex - 1, j].Length; k++)
                                                    {
                                                        strOriginClass2 += strOC.Replace("$", k.ToString()).Replace("-", OutClass[fIndex - 1, j].Substring(k - 1, 1));
                                                    }
                                                    //strOriginClass2 = strOC.Replace("$", ocIndex.ToString()).Replace("-", OutClass[fIndex - 1, j].Substring(ocIndex - 1, 1));
                                                    strOriginClasses2[fIndex - 1, j] = strOriginClass2;
                                                    strOriginClass2 = "";
                                                }
                                            }
                                            ocIndex += 1;

                                            string fareBasis = null;
                                            //string fareBasis = fareBasisCodes[i - 1].OuterXml;
                                            for (int k = 0; k < fareBasisCodes[outBoundCntTemp - 1].SelectNodes("PTC_FareBreakdown").Count; k++)
                                            {
                                                fareBasis += "<FareBasisCodes PassengerType=\"" + fareBasisCodes[outBoundCntTemp - 1].SelectNodes("PTC_FareBreakdown")[k].SelectSingleNode("PassengerTypeQuantity").Attributes["Code"].Value + "\">";
                                                for (int y = 0; y < fareBasisCodes[outBoundCntTemp - 1].SelectNodes("PTC_FareBreakdown")[k].SelectSingleNode("FareBasisCodes").SelectNodes("FareBasisCode").Count; y++)
                                                {
                                                    fareBasis += "<FareBasisCode>" + fareBasisCodes[outBoundCntTemp - 1].SelectNodes("PTC_FareBreakdown")[k].SelectSingleNode("FareBasisCodes").SelectNodes("FareBasisCode")[y].InnerText + "</FareBasisCode>";
                                                }
                                                fareBasis += "</FareBasisCodes>";
                                            }

                                            string strFares = BaseFare[fIndex - 1, 0].OuterXml.Replace("BaseFare", "FromTotalBaseFare") + TotalTax[fIndex - 1, 0].OuterXml.Replace("Tax", "FromTotalTax") + TotalFare[fIndex - 1, 0].OuterXml.Replace("TotalFare", "FromTotalFare");
                                            prepareChild = prepareChild.Replace("</TPA_Extensions>", strFares + fareBasis + "</TPA_Extensions>");
                                        }


                                        depstring += prepareChild;
                                        if (depCity == "")
                                        {
                                            depCity = outboundNodes[i - 1].SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value;
                                        }
                                    }
                                }
                                lstOutNodes.Add(depstring);
                                lstOriginClasses.Add(strOriginClass);
                                prepareChild = "";
                                //for (int i = 1; i <= strOutNodes.Count; i++)
                                foreach (string dest in lstOutNodes)
                                {
                                    int i = lstOutNodes.IndexOf(dest);
                                    if (i == 0)
                                        continue;

                                    test += $"<PricedItinerary SequenceNumber=\"{i}\">";
                                    test += "<AirItinerary>";
                                    test += "<OriginDestinationOptions>";
                                    test += "<OriginDestinationOption>";
                                    //outboundNodes[i-1].Attributes["RPH"].Value = i.ToString();
                                    prepareChild = lstOutNodes[i - 1];
                                    if (!isReturn && i <= outboundcnt)
                                    {
                                        if (NegoCode[i - 1, 0] != null && NegoCode[i - 1, 0] != "")
                                        {
                                            prepareChild = prepareChild.Replace("<TPA_Extensions>", "<TPA_Extensions PricingSource=\"" + PricingSource[i - 1, 0] + "\" NegotiatedFareCode=\"" + NegoCode[i - 1, 0] + "\">");
                                        }
                                        else
                                        {
                                            prepareChild = prepareChild.Replace("<TPA_Extensions>", "<TPA_Extensions PricingSource=\"" + PricingSource[i - 1, 0] + "\">");
                                        }

                                    }
                                    test = test + prepareChild;
                                    test += "</OriginDestinationOption>";
                                    if (isReturn)
                                    {
                                        test += "<OriginDestinationOption>";

                                        int iClass = 0;
                                        string strRPH = "0";

                                        int loopcnt = inboundNodes.Count; 

                                        for (int j = 1; j <= loopcnt; j++)
                                        {
                                            if (Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) <= inBoundCntTemp && i <= outBoundCntTemp)
                                            {
                                                if (strRPH != inboundNodes[j - 1].Attributes["RPH"].Value)
                                                {
                                                    iClass = 0;
                                                }
                                                else
                                                {
                                                    iClass += 1;
                                                }

                                                strRPH = inboundNodes[j - 1].Attributes["RPH"].Value;

                                                inboundNodes[j - 1].Attributes["ResBookDesigCode"].Value = InClass[i - 1, Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1].Substring(iClass, 1);

                                                prepareChild = inboundNodes[j - 1].OuterXml;
                                                if (NegoCode[i - 1, Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1] != null && NegoCode[i - 1, Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1] != "")
                                                {
                                                    prepareChild = prepareChild.Replace("<TPA_Extensions>", "<TPA_Extensions PricingSource=\"" + PricingSource[i - 1, Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1] + "\" NegotiatedFareCode=\"" + NegoCode[i - 1, Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1] + "\">");
                                                }
                                                else
                                                {
                                                    prepareChild = prepareChild.Replace("<TPA_Extensions>", "<TPA_Extensions PricingSource=\"" + PricingSource[i - 1, Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1] + "\">");
                                                }
                                                //prepareChild = prepareChild.Replace("</TPA_Extensions>", "<OriginClass Index=\"1\" Cabin=\"" + cabinpref + "\">" + OutClass[i - 1, Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1] + "</OriginClass>" + "</TPA_Extensions>");
                                                prepareChild = prepareChild.Replace("</TPA_Extensions>", strOriginClasses2[i - 1, Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1] + "</TPA_Extensions>");

                                                string fareBasis = "";
                                                //fareBasis = fareBasisCodes[i - 1].OuterXml;
                                                for (int k = 0; k < fareBasisCodes[Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1].SelectNodes("PTC_FareBreakdown").Count; k++)
                                                {

                                                    if (fareBasisCodes[Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1].SelectNodes("PTC_FareBreakdown").Count > 0)
                                                    {
                                                        fareBasis += "<FareBasisCodes PassengerType=\"" + fareBasisCodes[Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1].SelectNodes("PTC_FareBreakdown")[k].SelectSingleNode("PassengerTypeQuantity").Attributes["Code"].Value + "\">";

                                                        for (int y = 0; y < fareBasisCodes[Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1].SelectNodes("PTC_FareBreakdown")[k].SelectSingleNode("FareBasisCodes").SelectNodes("FareBasisCode").Count; y++)
                                                        {
                                                            fareBasis += "<FareBasisCode>" + fareBasisCodes[Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1].SelectNodes("PTC_FareBreakdown")[k].SelectSingleNode("FareBasisCodes").SelectNodes("FareBasisCode")[y].InnerText + "</FareBasisCode>";
                                                        }
                                                        fareBasis += "</FareBasisCodes>";
                                                    }

                                                }

                                                string strFares = BaseFare[i - 1, Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1].OuterXml.Replace("BaseFare", "FromTotalBaseFare") + TotalTax[i - 1, Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1].OuterXml.Replace("Tax", "FromTotalTax") + TotalFare[i - 1, Int32.Parse(inboundNodes[j - 1].Attributes["RPH"].Value) - 1].OuterXml.Replace("TotalFare", "FromTotalFare");
                                                prepareChild = prepareChild.Replace("</TPA_Extensions>", strFares + fareBasis + "</TPA_Extensions>");
                                                test = test + prepareChild;
                                            }
                                            else
                                            {
                                                inboundNodes[j - 1].Attributes["ResBookDesigCode"].Value = "";
                                                test = test + inboundNodes[j - 1].OuterXml;
                                            }
                                        }
                                        test += "</OriginDestinationOption>";
                                    }
                                    test += "</OriginDestinationOptions>";
                                    test += "</AirItinerary>";
                                    if (i <= outboundcnt) //&& isReturn)|| !isReturn)
                                    {
                                        if (TicketTimeLimit[i - 1, 0] == "2000-12-11T23:59:00")
                                        {
                                            for (int m = 1; m < outboundcnt; m++)
                                            {
                                                if (TicketTimeLimit[i - 1, m] != "2000-12-11T23:59:00")
                                                {
                                                    test += "<TicketingInfo TicketTimeLimit=\"" + TicketTimeLimit[i - 1, m] + "\"/>";
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                            test += "<TicketingInfo TicketTimeLimit=\"" + TicketTimeLimit[i - 1, 0] + "\"/>";
                                    }
                                    test += "</PricedItinerary>";
                                    prevPriceIt = i;
                                }

                                test += "</PricedItineraries>";
                                prepareChild = oPRoot.OuterXml;
                                prepareChild = prepareChild.Replace("</OTA_AirLowFareSearchFlightsRS>", $"{test}</OTA_AirLowFareSearchFlightsRS>");

                                strResponse = prepareChild;
                            }

                        }
                    }
                    #endregion

                    #region "old code without Avianca"
                    else
                    {
                        if (oRootResp.SelectSingleNode("Air_MultiAvailabilityReply/singleCityPairInfo/flightInfo").InnerXml != "")
                        {
                            string tt = oRootResp.SelectSingleNode("Air_MultiAvailabilityReply/singleCityPairInfo/flightInfo").InnerXml;
                            foreach (XmlNode oNodeResp in oRootResp)
                            {
                                strFirstFlight += "<OD>";
                                var oNode = oNodeResp.SelectSingleNode("singleCityPairInfo/flightInfo[1]");
                                strFirstFlight += oNode != null ? oNode.OuterXml : string.Empty;

                                if (oNode != null && oNode.SelectSingleNode("basicFlightInfo") != null)
                                {



                                    if (oNode.SelectSingleNode("basicFlightInfo/productTypeDetail/productIndicators").InnerText == "S")
                                    {
                                        strFirstFlight += oNodeResp.SelectSingleNode("singleCityPairInfo/flightInfo[position()=2]").OuterXml;

                                        if (oNodeResp.SelectSingleNode("singleCityPairInfo/flightInfo[position()=2]/basicFlightInfo/productTypeDetail/productIndicators").InnerText == "S"
                                            || oNodeResp.SelectSingleNode("singleCityPairInfo/flightInfo[position()=2]/basicFlightInfo/productTypeDetail/productIndicators").InnerText == "C")
                                        {
                                            strFirstFlight += oNodeResp.SelectSingleNode("singleCityPairInfo/flightInfo[position()=3]").OuterXml;

                                            if (oNodeResp.SelectSingleNode("singleCityPairInfo/flightInfo[position()=3]/basicFlightInfo/productTypeDetail/productIndicators").InnerText == "S"
                                                || oNodeResp.SelectSingleNode("singleCityPairInfo/flightInfo[position()=3]/basicFlightInfo/productTypeDetail/productIndicators").InnerText == "C")
                                            {
                                                strFirstFlight += oNodeResp.SelectSingleNode("singleCityPairInfo/flightInfo[position()=4]").OuterXml;
                                            }
                                        }
                                    }
                                }
                                strFirstFlight += "</OD>";
                            }

                            // ********************************************************************
                            //  Transform Native Amadeus response into OTA air price request      *
                            // ******************************************************************** 
                            strRequest = Request.Replace("</OTA_AirLowFareSearchFlightsRQ>", strFirstFlight + "</OTA_AirLowFareSearchFlightsRQ>");

                            if (oRoot.SelectSingleNode("SpecificFlightInfo/BookingClassPref") == null)
                            {
                                strRequest = CoreLib.TransformXML(strRequest, XslPath, $"{Version}AmadeusWS_LowFareFlights1RQ.xsl", false);
                                strResponse = SendInformativePricingWithoutPNR(ttAA,strRequest);
                                strResponse = CoreLib.TransformXML($"<FIP>{strRequest}{strResponse}</FIP>", XslPath, $"{Version}AmadeusWS_LowFareFlights1RS.xsl", false);
                                
                                strResponse = strResponse.Replace("</Fare_InformativeBestPricingWithoutPNRReply>", oRoot.SelectSingleNode("TravelerInfoSummary/AirTravelerAvail").OuterXml + strFirstFlight + "</Fare_InformativeBestPricingWithoutPNRReply>");
                                strFinalResponse = $"<FS>{strAvailResponses}{strResponse}</FS>";
                                strResponse = CoreLib.TransformXML(strFinalResponse, XslPath, $"{Version}AmadeusWS_LowFareFlightsRS.xsl", false);
                                strResponse = strResponse.Replace("<OTA_AirLowFareSearchFlightsRS", "<OTA_AirLowFareSearchFlightsRS EchoToken=\"FLTS\"");
                                EchoToken = "FLTS";
                            }
                            else
                            {

                                strFirstFlight = $"<AirAvail>{strFirstFlight}</AirAvail>";
                                oDocResp.LoadXml(strFirstFlight);
                                oRootResp = oDocResp.DocumentElement;
                                int countD = 1;

                                foreach (XmlNode oNodeResp in oRootResp.SelectNodes("OD[1]/flightInfo[1]/infoOnClasses"))
                                {
                                    if (oNodeResp.SelectSingleNode("productClassDetail/serviceClass") != null)
                                        FiltClasses.Add(oNodeResp.SelectSingleNode("productClassDetail/serviceClass").InnerText);
                                }

                                for (int j = 0; j < FiltClasses.Count; j++)
                                {
                                    oDocResp.LoadXml(strFirstFlight);
                                    oRootResp = oDocResp.DocumentElement;
                                    oRootNewResp = oDocResp.DocumentElement;
                                    strRequest = oRootNewResp.OuterXml;
                                    strRequest = strRequest.Substring((strRequest.IndexOf("<AirAvail>") + 10), strRequest.IndexOf("</AirAvail>") - 10);
                                    strRequest = Request.Replace("</OTA_AirLowFareSearchFlightsRQ>", strRequest + "</OTA_AirLowFareSearchFlightsRQ>");
                                    strRequest = CoreLib.TransformXML(strRequest, XslPath, $"{Version}AmadeusWS_LowFareFlights2RQ.xsl", false);
                                    strResponse = SendInformativePricingWithoutPNR(ttAA,strRequest);
                                    strResponse = strResponse.Replace("</Fare_InformativePricingWithoutPNRReply>", oRoot.SelectSingleNode("TravelerInfoSummary/AirTravelerAvail").OuterXml + strFirstFlight + "</Fare_InformativePricingWithoutPNRReply>");
                                    strFinalResponse = "<FS>" + strAvailResponses + strResponse + "</FS>";
                                    strResponse = CoreLib.TransformXML(strFinalResponse, XslPath, $"{Version}AmadeusWS_LowFareFlights3RS.xsl", false);
                                    



                                    if (j == 0)
                                    {
                                        strFirstResponse = strResponse.Replace("<OTA_AirLowFareSearchFlightsRS", "<OTA_AirLowFareSearchFlightsRS EchoToken=\"NEGO\"");
                                        strFirstResponse = RemoveDuplicate(strFirstResponse);
                                        oDocResp.LoadXml(strFirstResponse);
                                        oRootFinal = oDocResp.DocumentElement;

                                        EchoToken = "NEGO";
                                    }
                                    else
                                    {
                                        strNewResponse = strResponse.Replace("<OTA_AirLowFareSearchFlightsRS", "<OTA_AirLowFareSearchFlightsRS EchoToken=\"NEGO\"");
                                        strNewResponse = RemoveDuplicate(strNewResponse);
                                        oDocResp.LoadXml(strNewResponse);
                                        oRootResp = oDocResp.DocumentElement;
                                        EchoToken = "NEGO";

                                        if (oRootFinal.SelectSingleNode("Errors") != null)
                                        {
                                            oDocResp.LoadXml(strNewResponse);
                                            oRootFinal = oDocResp.DocumentElement;
                                        }

                                        

                                        XmlElement oRootSeq = null;

                                        foreach (XmlNode oNodeResp in oRootResp.SelectNodes("PricedItineraries/PricedItinerary[@SequenceNumber!='']"))
                                        {
                                            countD++;
                                            PricedItinerary = oNodeResp.InnerXml;
                                            PricedItinerary = $"<PricedItinerary SequenceNumber='{countD}'>{PricedItinerary}</PricedItinerary>";
                                            oDocResp.LoadXml(PricedItinerary);
                                            oRootSeq = oDocResp.DocumentElement;
                                            string testroot = oRootSeq.OuterXml;
                                            oRootFinal.SelectSingleNode("PricedItineraries").AppendChild(oRootSeq);
                                            string tr = oRootFinal.OuterXml;
                                        }
                                    }
                                    countD++;

                                }

                                if (countD == 1)
                                    strResponse = strFirstResponse;
                                else
                                    strResponse = oRootFinal.OuterXml;
                            }
                        }
                    }
                    #endregion
                }
                #region "else parts"
                else if ((oRoot.SelectSingleNode("TravelPreferences/VendorPref/@Code") == null || (oRoot.SelectSingleNode("TravelPreferences/VendorPref/@Code") != null && (oRoot.SelectSingleNode("TravelPreferences/VendorPref/@Code").InnerText != "AV" || oRoot.SelectSingleNode("TravelPreferences/VendorPref/@Code").InnerText != "TA" || oRoot.SelectSingleNode("TravelPreferences/VendorPref/@Code").InnerText != "UL")))
                    && (oRoot.SelectSingleNode("TravelPreferences/@AvailableFlightsOnly") == null || (oRoot.SelectSingleNode("TravelPreferences/@AvailableFlightsOnly") != null && oRoot.SelectSingleNode("TravelPreferences/@AvailableFlightsOnly").InnerText == "false"))
                    && ((oRoot.SelectSingleNode("TravelPreferences/VendorPref/@Code") != null) && (oRoot.SelectSingleNode("TravelPreferences/VendorPref/@Code").InnerText != "AV" && oRoot.SelectSingleNode("TravelPreferences/VendorPref/@Code").InnerText != "TA" && oRoot.SelectSingleNode("TravelPreferences/VendorPref/@Code").InnerText != "UL")))
                {
                    try
                    {

                        strSecondRequest = strSecondRequest.Replace("OTA_AirLowFareSearchFlightsRQ", "OTA_AirLowFareSearchScheduleRQ");

                        oDoc.LoadXml(strSecondRequest);
                        oRoot = oDoc.DocumentElement;
                        bool testb = false;
                        string FTTage = "";
                        if (oRoot.Attributes["PriorityDirectFlights"] != null)
                            testb = bool.Parse(oRoot.Attributes["PriorityDirectFlights"].Value);

                        if (oRoot.SelectSingleNode("TravelPreferences/FlightTypePref") != null
                            && oRoot.SelectSingleNode("TravelPreferences/FlightTypePref").Attributes["FlightType"] != null)
                            FTTage = oRoot.SelectSingleNode("TravelPreferences/FlightTypePref").Attributes["FlightType"].Value;

                        int ReqValue = 1;
                        strResponse = "";
                        XmlElement oRootFT = null;
                        string finalStrResponse = "";
                        int SuccessCount = 0;
                        int SeqCount, k = 0;

                        if (testb && FTTage.ToLower() != "nonstop")
                        {
                            ReqValue = 2;
                        }

                        if (oRoot.Attributes["FlexToken"] != null)
                            FlexToken = oRoot.Attributes["FlexToken"].InnerText.ToString();


                        for (k = 0; k < ReqValue; k++)
                        {


                            if (testb)
                            {
                                if (oRoot.SelectSingleNode("TravelPreferences") != null)
                                {
                                    string tt1 = oRoot.OuterXml;
                                    if (k == 0)
                                        oDoc.LoadXml("<FT><FlightTypePref FlightType='Direct'/></FT>");
                                    else if (k == 1)
                                        oDoc.LoadXml("<FT><FlightTypePref FlightType=''/></FT>");
                                    oRootFT = oDoc.DocumentElement;
                                    tt1 = oRootFT.SelectSingleNode("FlightTypePref").OuterXml;
                                    oRoot.SelectSingleNode("TravelPreferences").AppendChild(oRootFT.SelectSingleNode("FlightTypePref"));
                                    string tt = oRoot.OuterXml;
                                    strSecondRequest = oRoot.OuterXml;
                                    oRoot.SelectSingleNode("TravelPreferences/FlightTypePref").RemoveAll();
                                    tt = oRoot.OuterXml;
                                }
                                else
                                {
                                    if (k == 0)
                                        oDoc.LoadXml("<FT><TravelPreferences><FlightTypePref FlightType='Direct'/></TravelPreferences></FT>");
                                    else if (k == 1)
                                        oDoc.LoadXml("<FT><TravelPreferences><FlightTypePref FlightType=''/></TravelPreferences></FT>");

                                    oRootFT = oDoc.DocumentElement;
                                    oRoot.AppendChild(oRootFT.SelectSingleNode("TravelPreferences"));
                                    //strSndRequest.Add(oRoot.InnerXml);
                                    strSecondRequest = oRoot.OuterXml;
                                    oRoot.SelectSingleNode("TravelPreferences/FlightTypePref").RemoveAll();
                                }
                            }

                            if (FlexToken != "MPC")
                            {
                                
                                strRequest = CoreLib.TransformXML(strSecondRequest, XslPath, $"{Version}AmadeusWS_LowFareScheduleRQ.xsl", false);
                                strMessage = strRequest;
                                strResponse = SendMasterPricerTravelBoardSearch(ttAA,strRequest);
                                strMessage += strResponse;                                
                                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_LowFareScheduleRS.xsl", false);                                
                            }
                            else
                            {
                                strSecondRequest = strSecondRequest.Replace("<OTA_AirLowFareSearchScheduleRQ", "<OTA_AirLowFareSearchMatrixRQ");
                                strSecondRequest = strSecondRequest.Replace("</OTA_AirLowFareSearchScheduleRQ", "</OTA_AirLowFareSearchMatrixRQ");
                                strSecondRequest = strSecondRequest.Replace("Private", "Both");

                                // *****************************************************************
                                //  Transform OTA LowFareMatrix Request into Native Amadeus Request     *
                                // ***************************************************************** 
                                strRequest = CoreLib.TransformXML(strSecondRequest, XslPath, $"{Version}AmadeusWS_LowFareMatrixRQ.xsl", false);
                                
                                strMessage = strRequest;
                                strResponse = SendMasterPricerCalendar(ttAA,strRequest);
                                strMessage += strResponse;
                                


                                // ********************************************************************
                                //  Transform Native Amadeus LowFareMatrix Response into OTA Response   *
                                //  This transformation better organizes the Amadeus response to make *
                                //  easier to create and filter the final response                    *
                                // ******************************************************************** 
                                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_LowFareMatrix1RS.xsl", false);
                                

                                // ********************************************************************
                                //  Transform Native Amadeus LowFareMatrix Response into OTA Response   *
                                // ******************************************************************** 
                                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_LowFareMatrix2RS.xsl", false);
                                

                                // ***********************************************
                                //  process output business logic if necessary   *
                                // ***********************************************                                 
                                strResponse = strResponse.Replace("TransactionIdentifier=\"Amadeus", $"TransactionIdentifier=\"Amadeus-{ttProviderSystems.PCC}");
                                strResponse = strResponse.Replace("<OTA_AirLowFareSearchMatrixRS", "<OTA_AirLowFareSearchScheduleRS");
                                strResponse = strResponse.Replace("</OTA_AirLowFareSearchMatrixRS", "</OTA_AirLowFareSearchScheduleRS");

                            }

                            if (strResponse.IndexOf("<Error") == -1)
                            {
                                SuccessCount++;
                                if (SuccessCount == 1)
                                    finalStrResponse = strResponse;
                                else if (SuccessCount == 2)
                                {
                                    oDocResp.LoadXml(finalStrResponse);
                                    oRootFinal = oDocResp.DocumentElement;

                                    XmlElement oRootSeq = null;

                                    oDocResp.LoadXml(strResponse);
                                    oRootResp = oDocResp.DocumentElement;

                                    SeqCount = oRootFinal.SelectNodes("PricedItineraries/PricedItinerary[@SequenceNumber!='']").Count;
                                    foreach (XmlNode oNodeResp in oRootResp.SelectNodes("PricedItineraries/PricedItinerary[@SequenceNumber!='']"))
                                    {
                                        SeqCount++;
                                        PricedItinerary = oNodeResp.InnerXml;
                                        PricedItinerary = $"<PricedItinerary SequenceNumber='{SeqCount}'>{PricedItinerary}</PricedItinerary>";
                                        oDocResp.LoadXml(PricedItinerary);
                                        oRootSeq = oDocResp.DocumentElement;
                                        string testroot = oRootSeq.OuterXml;
                                        string tstrt = oRootFinal.OuterXml;
                                        oRootFinal.SelectSingleNode("PricedItineraries").AppendChild(oRootSeq);
                                        string tr = oRootFinal.OuterXml;
                                    }
                                    finalStrResponse = oRootFinal.OuterXml;
                                }
                            }

                            if (finalStrResponse == "" && k == 1)
                                finalStrResponse = strResponse;
                        }

                        if (k == 2)
                            strResponse = finalStrResponse;

                        if (FlexToken != "MPC")
                        {
                            strResponse = strResponse.Replace("<OTA_AirLowFareSearchScheduleRS", "<OTA_AirLowFareSearchFlightsRS EchoToken=\"LFSC\"");
                            EchoToken = "LFSC";
                        }
                        else
                        {
                            strResponse = strResponse.Replace("<OTA_AirLowFareSearchScheduleRS", "<OTA_AirLowFareSearchFlightsRS EchoToken=\"LFSCC\"");
                            EchoToken = "LFSCC";
                        }
                        strResponse = strResponse.Replace("</OTA_AirLowFareSearchScheduleRS>", "</OTA_AirLowFareSearchFlightsRS>");

                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error in MasterPricerTravelBoardSearch.\r\n{ex.Message}");                        
                    }
                }
                else
                {
                    if (((oRoot.SelectSingleNode("TravelPreferences/VendorPref/@Code") != null) && (oRoot.SelectSingleNode("TravelPreferences/VendorPref/@Code").InnerText == "AV" || oRoot.SelectSingleNode("TravelPreferences/VendorPref/@Code").InnerText == "TA" || oRoot.SelectSingleNode("TravelPreferences/VendorPref/@Code").InnerText == "UL")))
                    {
                        throw new Exception("NO AVAILABILITY FOR SELECTED PREFERENCE");
                    }

                    strFinalResponse = $"<FS>{strAvailResponses}</FS>";
                    strResponse = CoreLib.TransformXML(strFinalResponse, XslPath, $"{Version}AmadeusWS_LowFareFlightsRS.xsl", false);
                    

                    strResponse = strResponse.Replace("<OTA_AirLowFareSearchFlightsRS", "<OTA_AirLowFareSearchFlightsRS EchoToken=\"FLTS\"");
                    // strResponse = RemoveDuplicate(strResponse);
                    EchoToken = "FLTS";
                }
                #endregion

                if (EchoToken != "LFSC" && EchoToken != "LFSCC" && EchoToken != "FLTS")
                    strResponse = RemoveDuplicate(strResponse);
                
                if (!ttAA.isSOAP4)
                {
                    ttAA.CloseSession(ConversationID);
                }
                //}
                ttAA = null;

                // ***********************************************
                //  process output business logic if necessary   *
                // *********************************************** 
                
                //string t = $"TransactionIdentifier=\"Amadeus-{ttProviderSystems.PCC}";
                strResponse = strResponse.Replace("TransactionIdentifier=\"Amadeus", $"TransactionIdentifier=\"Amadeus-{ttProviderSystems.PCC}");
                

                #region "BL section"
                if (ttProviderSystems.BLFile != "")
                {
                    oDoc = new XmlDocument();
                    //  Load Access Control List into memory
                    oDoc.Load(ttProviderSystems.BLFile);

                    oRoot = oDoc.DocumentElement;
                    var oNode = oRoot.SelectSingleNode("Message[@Name=\'LowFare\'][@Direction=\'Out\']");

                    if (oNode != null)
                    {
                        //  check if non ticketable flights/fares to eliminate
                        oBLNode = oNode.SelectSingleNode($"NoTktAirline[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");
                        

                        if (oBLNode != null)
                            strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.gXslPath}BL\\", "BL_LowFareNoTktRS.xsl");                            

                        //  check if no mix airline to eliminate
                        oBLNode = oNode.SelectSingleNode($"NoMixAirline[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");

                        if (oBLNode != null)
                            strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.gXslPath}BL\\", "BL_LowFareNoMixRS.xsl");
                            
                        
                        //  add fare markup if needed
                        oBLNode = oNode.SelectSingleNode($"ProviderBL[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\'][@PCC=\'{ttProviderSystems.PCC}\']");
                        
                        if (oBLNode != null)
                            strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, $"{modCore.gXslPath}BL\\", "BL_LowFareRS.xsl");                            
                    }
                }
                #endregion
                strResponse = strResponse.Replace(" xmlns:msxsl=\"urn:schemas-microsoft-com:xslt\"", "");

                oPDoc = new XmlDocument();
                oPDoc.LoadXml(strResponse);
                oPRoot = oPDoc.DocumentElement;

                if (!strResponse.Contains("Errors"))
                {
                    for (int i = 0; i < oPDoc.GetElementsByTagName("PricedItinerary").Count; i++)
                    {
                        int k = 1;
                        if (!isReturn)
                            k = 0;
                        
                        for (int j = oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[k].SelectNodes("FlightSegment").Count - 1; j >= 0; j--)
                        {
                            if (oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[k].SelectNodes("FlightSegment").Item(j).Attributes["ResBookDesigCode"].Value == "1" || oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[k].SelectNodes("FlightSegment").Item(j).Attributes["ResBookDesigCode"].Value == "0")
                                oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[k].SelectNodes("FlightSegment").Item(j).ParentNode.RemoveChild(oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[k].SelectNodes("FlightSegment")[j]);
                        }
                    }

                    string asd = oPDoc.OuterXml;
                    for (int i = 0; i < oPDoc.GetElementsByTagName("PricedItinerary").Count; i++)
                    {
                        if (isReturn)
                        {
                            for (int j = 0; j < oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment").Count; j++)
                            {
                                if (oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalBaseFare") != null && oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalBaseFare").Attributes["Amount"].Value == "00000")
                                {
                                    oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalBaseFare").ParentNode.RemoveChild(oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalBaseFare"));
                                    oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalTax").ParentNode.RemoveChild(oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalTax"));
                                    oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalFare").ParentNode.RemoveChild(oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalFare"));

                                    try
                                    {
                                        oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FareBasisCodes").ParentNode.RemoveChild(oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FareBasisCodes"));
                                    }
                                    catch (Exception) { }
                                    oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].Attributes["ResBookDesigCode"].Value = "";
                                }
                            }
                            if (!oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].HasChildNodes)
                            {
                                XmlNode testnode = oPDoc.GetElementsByTagName("PricedItinerary").Item(i);
                                testnode.ParentNode.RemoveChild(testnode);
                            }
                        }
                        else
                        {
                            if (!oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].HasChildNodes)
                            {
                                oPDoc.GetElementsByTagName("PricedItinerary").Item(i).ParentNode.RemoveChild(oPDoc.GetElementsByTagName("PricedItinerary").Item(i));
                            }
                        }

                    }
                    Console.Write(oPDoc.OuterXml);
                    for (int i = 0; i < oPDoc.GetElementsByTagName("PricedItinerary").Count; i++)
                    {
                        if (isReturn)
                        {
                            for (int j = 0; j < oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment").Count; j++)
                            {
                                if (oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalBaseFare") != null && oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalBaseFare").Attributes["Amount"].Value == "00000")
                                {
                                    oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalBaseFare").ParentNode.RemoveChild(oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalBaseFare"));
                                    oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalTax").ParentNode.RemoveChild(oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalTax"));
                                    oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalFare").ParentNode.RemoveChild(oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FromTotalFare"));
                                    try
                                    {
                                        oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FareBasisCodes").ParentNode.RemoveChild(oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].SelectSingleNode("TPA_Extensions").SelectSingleNode("FareBasisCodes"));
                                    }
                                    catch (Exception) { }
                                    oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].Attributes["ResBookDesigCode"].Value = "";
                                }
                            }
                            if (!oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].HasChildNodes)
                            {
                                XmlNode testnode = oPDoc.GetElementsByTagName("PricedItinerary").Item(i);
                                testnode.ParentNode.RemoveChild(testnode);
                            }

                        }
                        else
                        {
                            if (!oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].HasChildNodes)
                            {
                                oPDoc.GetElementsByTagName("PricedItinerary").Item(i).ParentNode.RemoveChild(oPDoc.GetElementsByTagName("PricedItinerary").Item(i));
                            }
                        }

                    }


                    for (int i = 0; i < oPDoc.GetElementsByTagName("PricedItinerary").Count; i++)
                    {
                        if (isReturn)
                        {
                            for (int j = 0; j < oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment").Count; j++)
                            {
                                oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[j].Attributes["ResBookDesigCode"].Value = oPDoc.GetElementsByTagName("PricedItinerary").Item(i).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[0].SelectSingleNode("TPA_Extensions").SelectNodes("OriginClass")[j].InnerText;
                            }
                        }
                    }




                }

                finalResp = oPDoc.OuterXml;
                if (EchoToken == "NEGO")
                {
                    string modifiedStr = finalResp;
                    XmlDocument oReqDoc = new XmlDocument();
                    oReqDoc.LoadXml(modifiedStr);
                    XmlElement oNegoRoot = oReqDoc.DocumentElement;

                    if (oNegoRoot.SelectSingleNode("Success") != null)
                    {
                        if (oNegoRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary").Count > 1)
                        {
                            foreach (XmlNode nd in oNegoRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment"))
                            {
                                nd.Attributes["RPH"].Value = (Int32.Parse(nd.Attributes["RPH"].Value) + 1).ToString();
                                oNegoRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[0].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].AppendChild(nd.Clone());
                            }
                            XmlNode tmpND = oNegoRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[0];
                            foreach (XmlNode nd in oNegoRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[0].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment"))
                            {
                                if (Int32.Parse(nd.Attributes["RPH"].Value) == 1)
                                {
                                    oNegoRoot.SelectSingleNode("PricedItineraries").SelectNodes("PricedItinerary")[1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].InsertBefore(nd.Clone(), tmpND);
                                }


                            }

                        }
                    }
                    string test = oNegoRoot.OuterXml;
                    finalResp = test;

                }
            }
            catch (Exception ex)
            {
                finalResp = modCore.FormatErrorMessage(modCore.ttServices.LowFareFlights, ex.Message, ttProviderSystems);
            }
            finally
            {
                oBLNode = null;
            }

            return finalResp;
        }

        public string LowFareSchedule()
        {
            string strRequest = "";
            string strResponse = "";
            XmlElement oRoot = null;
            DateTime RequestTime;

            // ************************************************************
            //  Get the Filtering Elements from OTA LowFareSchedule Request   *
            // ************************************************************
            try
            {
                AmadeusWSAdapter ttAA = null;

                RequestTime = DateTime.Now;
                strRequest = Request;
                XmlDocument oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;
                XmlNode oNode = oRoot.SelectSingleNode("POS/Source/@PseudoCityCode");

                strRequest = oNode != null ? oRoot.OuterXml : Request;

                if (oNode != null)
                    oNode.InnerText = ttProviderSystems.PCC;

                // *****************************************************************
                //  Transform OTA LowFarePlus Request into Native Amadeus Request     *
                // *****************************************************************             
                strRequest = SetRequest($"AmadeusWS_LowFareScheduleRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 

                if (ttProviderSystems.SessionPool)
                {
                    int waitTime = 0;
                    Random r = new Random();
                    waitTime = r.Next(1, 1000);
                    Thread.Sleep(waitTime);
                }
                ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
                ttAA.TracerID = _tracerID;

                strResponse = SendMasterPricerTravelBoardSearch(ttAA, strRequest);
                strMessage = $"{Request}{strResponse}";

                // ********************************************************************
                //  Transform Native Amadeus LowFarePlus Response into OTA Response   *
                //  This transformation better organizes the Amadeus response to make *
                //  easier to create and filter the final response                    *
                // ********************************************************************             
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_LowFareScheduleRS.xsl", false);

                // ***********************************************
                //  process output business logic if necessary   *
                // *********************************************** 
                strResponse = strResponse.Replace("TransactionIdentifier=\"Amadeus", $"TransactionIdentifier=\"Amadeus-{ttProviderSystems.PCC}");

                if (ttProviderSystems.LogNative)
                {
                    TripXMLTools.TripXMLLog.LogMessage("LowFareSchedule", ref strMessage, RequestTime, DateTime.Now, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }

                if (modCore.NonDirectFlights)
                {
                    if (strResponse.Contains("<Error"))
                    {
                        ErrorReq = strResponse;
                        strResponse = "";
                        RequestCount++;
                    }

                    if (modCore.LFSchRequestCount == RequestCount)
                    {
                        strResponse = ErrorReq;
                    }
                }

                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception Error.\r\n{ex.Message}");
            }
        }

        public string FareInfo()
        {
            string strRequest = "";
            string strResponse = "";
            string strRespNative = "";
            string[] AirFareInfoRS = null;
            XmlElement oRoot = null;
            XmlDocument oDocResp = null;
            XmlElement oRootResp = null;
            XmlElement oRootNative = null;
            // XmlNode oNodeNative = null;
            string currency = "";
            string DepartureDate = "";
            string ArrivalDate = "";
            string DepartureLocation = "";
            string ArrivalLocation = "";
            string AirlineCode = "";
            string TicketDate = "";
            int i = 0;
            bool inSession = false;

            try
            {
                try
                {
                    strRespNative = CoreLib.TransformXML(Request, XslPath, $"AmadeusWS_FareInfoRQ.xsl", false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming OTA Request. \r\n {ex.Message}");
                }

                if (string.IsNullOrEmpty(strRespNative))
                    throw new Exception("Transformation produced empty xml.");

                //  *******************
                //  Create Session    *
                //  *******************
                AmadeusWSAdapter ttAA;
                try
                {
                    ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
                    ttAA.TracerID = _tracerID;
                    inSession = inSession = SetConversationID(ttAA);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Creating Session.\r\n{ex.Message}");
                }

                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 
                var oDocNative = new XmlDocument();
                oDocNative.LoadXml(strRespNative);
                oRootNative = oDocNative.DocumentElement;
                i = 0;

                AirFareInfoRS = new string[oRootNative.ChildNodes.Count];

                foreach (XmlNode oNodeNative in oRootNative.ChildNodes)
                {
                    try
                    {
                        AirFareInfoRS[i] = SendRequestCryptically(ttAA, oNodeNative.OuterXml);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    if (AirFareInfoRS[i].Contains("MessagesOnly_Reply"))
                    {
                        strRespNative = CoreLib.TransformXML(AirFareInfoRS[i], XslPath, $"AmadeusWS_FareInfoRS.xsl", false);

                        return strRespNative;
                    }

                    if (AirFareInfoRS[i].Contains("NO VALID FARE"))
                    {
                        throw new Exception("NO VALID FARE/RULE COMBINATIONS FOR PRICING");
                    }

                    if (AirFareInfoRS[i].Substring((AirFareInfoRS[i].IndexOf(" PAGE ") + 7), 1) != AirFareInfoRS[i].Substring((AirFareInfoRS[i].IndexOf(" PAGE ") + 10), 1))
                    {
                        strRequest = "<Command_Cryptic><messageAction><messageFunctionDetails><messageFunction>M</messageFunction></messageFunctionDetails></messageAction><longTextString><textStringDetails>MD</textStringDetails></longTextString></Command_Cryptic>";
                        try
                        {
                            strResponse = SendRequestCryptically(ttAA, strRequest).Replace("<textStringDetails>/$", "<textStringDetails>");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        if (strResponse.Contains("MessagesOnly_Reply"))
                        {
                            strRespNative = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_FareInfoRS.xsl", false);

                            return strRespNative;
                        }
                        strResponse = strResponse.Replace("<Command_CrypticReply><longTextString><textStringDetails>", "");
                        strResponse = strResponse.Replace("</textStringDetails></longTextString></Command_CrypticReply>", "");
                        AirFareInfoRS[i] = AirFareInfoRS[i].Replace("</textStringDetails></longTextString></Command_CrypticReply>", $"{strResponse}</textStringDetails></longTextString></Command_CrypticReply>");

                    }
                    i++;
                }
                //  Close Session
                if (!string.IsNullOrEmpty(ConversationID))
                {
                    if (ttProviderSystems.SessionPool)
                    {
                        ConversationID = SubSessionID(ConversationID);
                        ttAA.CloseSessionFromPool(ConversationID);
                    }
                    else
                    {
                        ttAA.CloseSession(ConversationID);
                    }
                }

                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                oRoot = oDoc.DocumentElement;
                var oNode = oRoot.SelectSingleNode("TravelPreferences/FareRestrictPref/@FareDisplayCurrency");
                currency = oNode != null ? oNode.InnerText : "USD";

                oNode = oRoot.SelectSingleNode("OriginDestinationInformation[1]/FlightSegment[1]/@DepartureDateTime");
                if (oNode != null)
                {
                    DepartureDate = oNode.InnerText;
                }

                oNode = oRoot.SelectSingleNode("OriginDestinationInformation[1]/FlightSegment[1]/DepartureAirport/@LocationCode");
                if (oNode != null)
                {
                    DepartureLocation = oNode.InnerText;
                }

                oNode = oRoot.SelectSingleNode("OriginDestinationInformation[position()=last()]/FlightSegment[position()=last()]/@DepartureDateTime");
                if (oNode != null)
                {
                    ArrivalDate = oNode.InnerText;
                }

                oNode = oRoot.SelectSingleNode("OriginDestinationInformation[position()=last()]/FlightSegment[position()=last()]/ArrivalAirport/@LocationCode");
                if (oNode != null)
                {
                    ArrivalLocation = oNode.InnerText;
                }

                AirlineCode = AirFareInfoRS[0].Substring(AirFareInfoRS[0].IndexOf("VALIDATING CARRIER") + 19, 2);
                TicketDate = AirFareInfoRS[0].Substring(AirFareInfoRS[0].IndexOf("LAST TKT DTE") + 13, 7);
                DateTime ddate;
                ddate = Convert.ToDateTime(TicketDate);
                TicketDate = ddate.ToString("yyyy-MM-dd");
                strResponse = "";

                for (i = 0; i <= (AirFareInfoRS.Length - 1); i++)
                {
                    try
                    {
                        oDocResp = new XmlDocument();
                        oDocResp.LoadXml(AirFareInfoRS[i]);
                        oRootResp = oDocResp.DocumentElement;
                        if (oRootResp == null)
                        {
                            throw new Exception("Unable to load AirFareInfo response xml");
                        }
                        // *******************************
                        // * Execute parsing funcion     *
                        // *******************************

                        AirFareInfoRS[i] = ParseAirFareInfoRS(currency, DepartureDate, ArrivalDate, DepartureLocation, ArrivalLocation, AirlineCode, TicketDate, oRootResp.SelectSingleNode("longTextString/textStringDetails").InnerText, i, oRoot.SelectSingleNode($"TravelerInfoSummary/PassengerTypeQuantity[position()={i + 1}]/@Code").InnerText);

                        if (AirFareInfoRS[i].Contains("MessagesOnly_Reply"))
                        {
                            strRespNative = CoreLib.TransformXML(AirFareInfoRS[i], XslPath, $"AmadeusWS_FareInfoRS.xsl", false);
                            return strRespNative;
                        }

                        strResponse += AirFareInfoRS[i];

                    }
                    catch (Exception ex)
                    {
                        if (!string.IsNullOrEmpty(ConversationID))
                        {
                            if (ttProviderSystems.SessionPool)
                            {
                                ConversationID = SubSessionID(ConversationID);
                                ttAA.CloseSessionFromPool(ConversationID);
                            }
                            else
                            {
                                ttAA.CloseSession(ConversationID);
                            }
                        }
                        strRespNative = CoreLib.TransformXML($"<MessagesOnly_Reply><CAPI_Messages><ErrorCode>9999</ErrorCode><Text>{ex.Message}</Text></CAPI_Messages></MessagesOnly_Reply>", XslPath, $"AmadeusWS_FareInfoRS.xsl", false);
                        return strRespNative;
                    }
                }

                strResponse = $"<OTA_AirFareInfoRS><Success/><FareDisplayInfos>{strResponse}</FareDisplayInfos></OTA_AirFareInfoRS>";
                return strResponse;
            }
            catch (Exception ex)
            {
                string strError = $"Error *** Parsing AirFareInfoRespose:{ex.Message}";
                strRespNative = CoreLib.TransformXML($"<MessagesOnly_Reply><CAPI_Messages><ErrorCode>9999</ErrorCode><Text>{ex.Message}</Text></CAPI_Messages></MessagesOnly_Reply>", XslPath, $"AmadeusWS_FareInfoRS.xsl", false);
                return strRespNative;
            }
        }

        public string AirSchedule()
        {

            string strRequest;
            string strResponse;
            try
            {
                strRequest = SetRequest($"AmadeusWS_AirScheduleRQ.xsl");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming OTA Request. {ex.Message}");
            }
            if (string.IsNullOrEmpty(strRequest))
                throw new Exception("Transformation produced empty xml.");

            //*******************************************************************************
            // Send Transformed Request to the AmadeusWS Adapter and Getting Native Response  *
            //******************************************************************************* 
            try
            {
                AmadeusWSAdapter ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
                strResponse = SendAirMultiAvailability(ttAA, strRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //*****************************************************************
            // Transform Native AmadeusWS AirSchedule Response into OTA Response   *
            //***************************************************************** 
            try
            {
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_AirScheduleRS.xsl", false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
            }

            return strResponse;
        }

        public string BusinessLogic(string strResponse, string strBusiness, string xslPath, string xslFile)
        {

            if (strResponse.Contains("<Success />") || strResponse.Contains("<Success></Success>"))
            {
                strResponse = strResponse.Replace("<Success />", $"{strBusiness}<Success />");
                strResponse = strResponse.Replace("<Success></Success>", $"{strBusiness}<Success></Success>");
                strResponse = CoreLib.TransformXML(strResponse, xslPath, $"{xslFile}", false);
            }
            return strResponse;
        }

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

            string itin = "";
            try
            {
                DashCount = pResponseXML.IndexOf("---------");
                // Find where end of header starts
                intChars = (pResponseXML.Length - DashCount);
                pResponseXML = pResponseXML.Substring(DashCount, intChars);
                // Get rid of header lines
                intChars = pResponseXML.IndexOf("NVA   BG");
                // Find out beginning of itin description 
                if ((intChars == -1))
                {
                    // If not found  
                    throw new Exception("Invalid AirFareInfo response msg");
                    // Send Error Reponse
                }
                intCount = pResponseXML.IndexOf(pCurrency);
                // Find out first currency instance 
                if ((intCount == -1))
                {
                    // If not found  
                    throw new Exception("Invalid AirFareInfo response msg");
                    // Send Error Reponse
                }
                itin = pResponseXML.Substring(intChars + 9, intCount - (intChars + 9));
                // get itin info only
                itin = parseItin(itin, Request);
                //  extract itinerary data
                intChars = (pResponseXML.Length - intCount);
                // Calculate relevant fare info character count
                pResponseXML = pResponseXML.Substring(intCount, intChars);
                // Extract relevant fare info 
                intCount = pResponseXML.IndexOf(">");
                // Removes the end line of 1st screen
                if (intCount > -1)
                {
                    // If is found  
                    intChars = pResponseXML.IndexOf(">", (intCount + 1));
                    // find the 2nd >
                    if (intChars > -1)
                    {
                        pResponseXML = $"{pResponseXML.Substring(0, intCount - 1)}\r{pResponseXML.Substring(intChars + 1)}";
                    }
                }
                string tempResp = "";
                string newTax = "";
                ArrayList taxTable = new ArrayList();
                tempResp = pResponseXML.Substring(3);
                //  skip first currency code
                tempResp = tempResp.Substring(tempResp.IndexOf(pCurrency));
                //  point to next currency code
                while (tempResp.Length > 0)
                {
                    if (tempResp.Substring(0, 3) == pCurrency)
                    {
                        if ((tempResp.Substring(12, 1) != " ")
                                    && (tempResp.Substring(12, 1) != "\r")
                                    && ((tempResp.Substring(14, 1) == " ")
                                    || (tempResp.Substring(14, 1) == "\r"))
                                    && (tempResp.Substring(12, 2) != "XT"))
                        {
                            // tax code found
                            newTax = $"{newTax}{tempResp.Substring(0, 14)}";
                        }
                    }
                    else
                    {
                        break;
                    }
                    tempResp = tempResp.Substring((tempResp.IndexOf("\r") + 1));
                }
                tempResp = pResponseXML.Substring(3);
                //  skip first currency code
                tempResp = tempResp.Substring(tempResp.IndexOf(pCurrency));
                //  point to next currency code
                while (tempResp.Length > 0)
                {
                    if (tempResp.Substring(0, 3) == pCurrency)
                    {
                        if (tempResp.IndexOf("\r") > 17)
                        {
                            if ((tempResp.IndexOf("XT ") > 17) && (tempResp.IndexOf("XT ") < tempResp.IndexOf("\r")))
                            {
                                newTax = $"{newTax}{tempResp.Substring(tempResp.IndexOf("XT ") + 3, tempResp.IndexOf("\r") - (tempResp.IndexOf("XT ") - 3))}";
                            }
                            else if (tempResp.Substring(16, 2) == "  ")
                            {
                                newTax = $"{newTax}{ tempResp.Substring(17, tempResp.IndexOf("\r") - 17)}";
                            }
                        }
                    }
                    else if (tempResp.Substring(0, 3) == "   ")
                    {
                        newTax = $"{newTax}{ tempResp.Substring(17, tempResp.IndexOf("\r") - 17)}";
                    }
                    else
                    {
                        break;
                    }
                    tempResp = tempResp.Substring((tempResp.IndexOf("\r") + 1));
                }
                AirFareInfo = Regex.Split(pResponseXML, pCurrency);
                // split fare info using currency as delimeter
                // ********************************
                // * Extract the base fare amount *
                // ********************************
                for (i = 0; (i <= (AirFareInfo.Length - 1)); i++)
                {
                    if (AirFareInfo[i] != "")
                    {
                        strTemp = AirFareInfo[i].Replace("\r", "").Replace("\n", "").Trim();
                        x = strTemp.IndexOf(" ");
                        // Find where the end base amount is
                        if (x == -1)
                        {
                            BaseFare = strTemp;
                            // Extract Fare Base amount
                            // Throw New Exception("invalid base fare amount")
                        }
                        else
                        {
                            BaseFare = strTemp.Substring(0, x);
                            // Extract Fare Base amount
                        }
                        break;
                    }
                }
                // ********************************
                // * Build an xml from the taxes  *
                // ********************************
                string strTempResp = "";
                string strXFResp = "";
                string[] strFareInfo = null;
                string[] strNewFareInfo = null;
                string[] strXFInfo = null;
                string[] strNewXFInfo = null;
                int j = 0;
                int k = 0;
                pResponseXML = pResponseXML.Substring(3);
                //  skip first currency code
                strTempResp = pResponseXML.Substring(pResponseXML.IndexOf(pCurrency));
                strTempResp = newTax;
                strFareInfo = Regex.Split(strTempResp, pCurrency);
                // split fare info using currency as delimeter

                strNewFareInfo = new string[strFareInfo.Length];

                for (i = 0; (i <= (strFareInfo.Length - 1)); i++)
                {
                    strTemp = strFareInfo[i].Replace("\r", "").Replace("\n", "");
                    strTemp = strTemp.Trim();

                    if (strTemp.IndexOf("PAGE ") != -1)
                    {
                        strTemp = strTemp.Substring(0, strTemp.IndexOf("PAGE ")) + strTemp.Substring(strTemp.IndexOf("PAGE ") + 10);
                        strTemp = strTemp.Trim();
                    }

                    if (!string.IsNullOrEmpty(strTemp) && !AllDigits(strTemp))
                    {
                        if (strTemp.IndexOf(" ") != -1)
                        {
                            if (IsNumeric(strTemp.Substring(0, strTemp.IndexOf(" ")))
                                        && (strTemp.IndexOf("XF ") == -1))
                            {
                                strTemp = strTemp.Substring(0, (strTemp.IndexOf(" ") + 3));
                                strTemp = strTemp.Replace(" ", "");
                            }
                            else
                            {
                                if (strTemp.IndexOf("XF ") != -1)
                                {
                                    strXFResp = strTemp.Substring((strTemp.IndexOf("XF ") + 3));
                                    strXFResp = strXFResp.Replace(" ", "");
                                    strXFInfo = strXFResp.Split(char.Parse("."));
                                    strNewXFInfo = new string[strXFInfo.Length - 1];

                                    for (k = 0; (k <= (strNewXFInfo.Length - 1)); k++)
                                    {
                                        strNewXFInfo[k] = strXFResp.Substring(0, 3);
                                        strXFResp = strXFResp.Substring(3);
                                        strNewXFInfo[k] = $"{strNewXFInfo[k]}{ strXFResp.Substring(0, strXFResp.IndexOf(".") + 3)}";
                                        strXFResp = strXFResp.Substring(strXFResp.IndexOf(".") + 3);
                                    }
                                }
                                if (strTemp.IndexOf(" XF") != -1)
                                {
                                    strTemp = strTemp.Replace(" XF", "XF");
                                }
                                strTemp = strTemp.Substring(0, strTemp.IndexOf(" "));
                            }
                        }
                        strNewFareInfo[j] = strTemp.Trim();
                        j++;
                    }
                }
                for (i = 0; i <= (strNewFareInfo.Length - 1); i++)
                {
                    if (!(strNewFareInfo[i] == null))
                    {
                        Amounts.Add(strNewFareInfo[i]);
                    }
                }

                // **********************************
                // * Calculate decimal places       *
                // **********************************
                intChars = BaseFare.IndexOf(".");

                if (intChars == -1)
                    NumDecimals = 0;
                else
                    NumDecimals = BaseFare.Length - (intChars + 1);

                BaseFare = BaseFare.Replace(".", "");
                // **********************************
                // * Build the xml response         *
                // ***********r**********************
                XmlDocument xmlDoc = new XmlDocument();
                // Instantiate an xml document    
                XmlNode Root = null;
                XmlNode node = null;
                XmlNode nItem = null;
                XmlAttribute Attribute = null;
                xmlDoc.LoadXml($"<FareDisplayInfo><TravelDates/><ValidatingAirline/><DepartureLocation/><ArrivalLocation/><LastTicketing/>{itin}<PricingInfo><BaseFare></BaseFare><Taxes></Taxes><TotalFare></TotalFare></PricingInfo></FareDisplayInfo>");
                Root = xmlDoc.DocumentElement;
                if (Root == null)
                {
                    throw new Exception("Unable to load CBC Response");
                }
                // *******************************************
                // *  Process FareDisplayInfo node           *
                // *******************************************
                Attribute = xmlDoc.CreateAttribute("FareRPH");
                Attribute.Value = (fareRPH + 1).ToString();
                Root.Attributes.Append(Attribute);
                // Attribute = xmlDoc.CreateAttribute("ResBookDesigCode")
                // 'Attribute.Value = "1"
                // node.Attributes.Append(Attribute)
                // *******************************************
                // *  Process TravelDates node               *
                // *******************************************
                node = Root.SelectSingleNode("TravelDates");
                Attribute = xmlDoc.CreateAttribute("DepartureDate");
                Attribute.Value = pDepartureDate;
                node.Attributes.Append(Attribute);
                Attribute = xmlDoc.CreateAttribute("ArrivalDate");
                Attribute.Value = pArrivalDate;
                node.Attributes.Append(Attribute);
                // *******************************************
                // *  Process ValidatingAirline node          *
                // *******************************************
                node = Root.SelectSingleNode("ValidatingAirline");
                Attribute = xmlDoc.CreateAttribute("Code");
                Attribute.Value = pAirline;
                node.Attributes.Append(Attribute);
                // *******************************************
                // *  Process DepartureLocation node         *
                // *******************************************
                node = Root.SelectSingleNode("DepartureLocation");
                Attribute = xmlDoc.CreateAttribute("LocationCode");
                Attribute.Value = pDepLocation;
                node.Attributes.Append(Attribute);
                // *******************************************
                // *  Process ArrivalLocation node           *    
                // *******************************************
                node = Root.SelectSingleNode("ArrivalLocation");
                Attribute = xmlDoc.CreateAttribute("LocationCode");
                Attribute.Value = pArrLocation;
                node.Attributes.Append(Attribute);
                // *******************************************
                // *  Process LastTicketingDate node          *
                // *******************************************
                node = Root.SelectSingleNode("LastTicketing");
                Attribute = xmlDoc.CreateAttribute("Date");
                Attribute.Value = pTicketDate;
                node.Attributes.Append(Attribute);
                // *******************************************
                // *  Add passenger type info               *
                // *******************************************
                node = Root.SelectSingleNode("PricingInfo");
                Attribute = xmlDoc.CreateAttribute("PassengerTypeCode");
                Attribute.Value = paxType;
                node.Attributes.Append(Attribute);
                // *******************************************
                // *  Process PricingInfo/BaseFare node      *
                // *******************************************
                node = Root.SelectSingleNode("PricingInfo/BaseFare");
                Attribute = xmlDoc.CreateAttribute("Amount");
                Attribute.Value = BaseFare;
                node.Attributes.Append(Attribute);
                Attribute = xmlDoc.CreateAttribute("CurrencyCode");
                Attribute.Value = pCurrency;
                node.Attributes.Append(Attribute);
                Attribute = xmlDoc.CreateAttribute("DecimalPlaces");
                Attribute.Value = NumDecimals.ToString();
                node.Attributes.Append(Attribute);
                // *******************************************
                // *  Process PricingInfo/Taxes node         *
                // *******************************************
                node = Root.SelectSingleNode("PricingInfo/Taxes");
                for (i = 0; (i <= (Amounts.Count - 1)); i++)
                {
                    TaxAmount = Amounts[i].ToString();
                    // *******************************************
                    // *  Process PricingInfo/Taxes/Tax node     *
                    // *******************************************
                    nItem = xmlDoc.CreateElement("Tax");
                    Attribute = xmlDoc.CreateAttribute("TaxCode");
                    Attribute.Value = TaxAmount.Substring((TaxAmount.Length - 2));
                    nItem.Attributes.Append(Attribute);
                    Attribute = xmlDoc.CreateAttribute("Amount");
                    Attribute.Value = TaxAmount.Substring(0, (TaxAmount.Length - 2)).Replace(".", "");
                    nItem.Attributes.Append(Attribute);
                    Attribute = xmlDoc.CreateAttribute("CurrencyCode");
                    Attribute.Value = pCurrency;
                    nItem.Attributes.Append(Attribute);
                    Attribute = xmlDoc.CreateAttribute("DecimalPlaces");
                    Attribute.Value = NumDecimals.ToString();
                    nItem.Attributes.Append(Attribute);
                    if (TaxAmount.Substring((TaxAmount.Length - 2)) == "XF")
                    {
                        XmlNode nXFItem = null;
                        XmlAttribute XFAttribute = null;
                        for (k = 0; (k <= (strNewXFInfo.Length - 1)); k++)
                        {
                            nXFItem = xmlDoc.CreateElement("SegTax");
                            XFAttribute = xmlDoc.CreateAttribute("CityCode");
                            XFAttribute.Value = strNewXFInfo[k].Substring(0, 3);
                            nXFItem.Attributes.Append(XFAttribute);
                            XFAttribute = xmlDoc.CreateAttribute("Amount");
                            XFAttribute.Value = strNewXFInfo[k].Substring(3).Replace(".", "");
                            nXFItem.Attributes.Append(XFAttribute);
                            nItem.AppendChild(nXFItem);
                        }
                    }
                    node.AppendChild(nItem);
                    // *******************************************
                    // * Calculate total tax amount              *
                    // *******************************************
                    TotalTax = (TotalTax + Convert.ToInt32(TaxAmount.Substring(0, (TaxAmount.Length - 2)).Replace(".", "")));
                }
                // *******************************************
                // *  Process PricingInfo/TotalFare node     *
                // *******************************************  
                TotalFare = ((Convert.ToInt32(BaseFare) + TotalTax)).ToString();
                node = Root.SelectSingleNode("PricingInfo/TotalFare");
                Attribute = xmlDoc.CreateAttribute("Amount");
                Attribute.Value = TotalFare;
                node.Attributes.Append(Attribute);
                Attribute = xmlDoc.CreateAttribute("CurrencyCode");
                Attribute.Value = pCurrency;
                node.Attributes.Append(Attribute);
                Attribute = xmlDoc.CreateAttribute("DecimalPlaces");
                Attribute.Value = NumDecimals.ToString();
                node.Attributes.Append(Attribute);
                OTAResponseXML = Root.OuterXml;
            }
            catch (Exception ex)
            {
                OTAResponseXML = $"<MessagesOnly_Reply><CAPI_Messages><ErrorCode>9999</ErrorCode><Text>{ex.Message}</Text></CAPI_Messages></MessagesOnly_Reply>";
            }
            return OTAResponseXML;
        }

        private string parseItin(string itin, string request)
        {
            XmlDocument oDoc = new XmlDocument();
            oDoc.LoadXml(request);
            XmlElement oRoot = oDoc.DocumentElement;
            itin = itin.Substring(5);
            XmlNodeList oNodeSegList = oRoot.SelectNodes("OriginDestinationInformation/FlightSegment");

            foreach (XmlNode oNode in oNodeSegList)
            {
                string strTemp = oNode.SelectSingleNode("ArrivalAirport/@LocationCode").InnerText;
                if (itin.IndexOf("S U R F A C E") == 10)
                    itin = itin.Substring(24);

                //  class of service
                string strValue = itin.Substring(14, 1);
                XmlNode node = oNode.SelectSingleNode("MarketingAirline");
                XmlAttribute attribute = oDoc.CreateAttribute("ClassOfService");
                attribute.Value = strValue;
                node.Attributes.Append(attribute);
                //  fare basis code
                strValue = itin.Substring(31, itin.Substring(31).IndexOf(" "));
                XmlNode oNodeNew = oDoc.CreateElement("FareBasis");
                attribute = oDoc.CreateAttribute("FareBasisCode");
                attribute.Value = strValue;
                oNodeNew.Attributes.Append(attribute);
                oNode.AppendChild(oNodeNew);
                //  validity before
                strValue = itin.Substring(47, 5);
                if (strValue != "     ")
                {
                    oNodeNew = oDoc.CreateElement("FareValidity");
                    attribute = oDoc.CreateAttribute("ValidityReason");
                    attribute.Value = "Before";
                    oNodeNew.Attributes.Append(attribute);
                    attribute = oDoc.CreateAttribute("ValidityDate");
                    attribute.Value = oNode.SelectSingleNode("@DepartureDateTime").InnerText;
                    oNodeNew.Attributes.Append(attribute);
                    oNode.AppendChild(oNodeNew);
                }
                //  validity after
                strValue = itin.Substring(52, 5);
                if (strValue != "     ")
                {
                    oNodeNew = oDoc.CreateElement("FareValidity");
                    attribute = oDoc.CreateAttribute("ValidityReason");
                    attribute.Value = "After";
                    oNodeNew.Attributes.Append(attribute);
                    attribute = oDoc.CreateAttribute("ValidityDate");
                    attribute.Value = oNode.SelectSingleNode("@DepartureDateTime").InnerText;
                    oNodeNew.Attributes.Append(attribute);
                    oNode.AppendChild(oNodeNew);
                }
                //  bag allowance
                strValue = itin.Substring(58, 2);
                oNodeNew = oDoc.CreateElement("BagAllowance");
                if (strValue == "PC")
                {
                    attribute = oDoc.CreateAttribute("Quantity");
                    attribute.Value = "1";
                    oNodeNew.Attributes.Append(attribute);
                    attribute = oDoc.CreateAttribute("Type");
                    attribute.Value = "Piece";
                    oNodeNew.Attributes.Append(attribute);
                    oNode.AppendChild(oNodeNew);
                }
                else if (strValue == "1P")
                {
                    attribute = oDoc.CreateAttribute("Quantity");
                    attribute.Value = "1";
                    oNodeNew.Attributes.Append(attribute);
                    attribute = oDoc.CreateAttribute("Type");
                    attribute.Value = "Piece";
                    oNodeNew.Attributes.Append(attribute);
                    oNode.AppendChild(oNodeNew);
                }
                else if (strValue == "2P")
                {
                    attribute = oDoc.CreateAttribute("Quantity");
                    attribute.Value = "2";
                    oNodeNew.Attributes.Append(attribute);
                    attribute = oDoc.CreateAttribute("Type");
                    attribute.Value = "Piece";
                    oNodeNew.Attributes.Append(attribute);
                    oNode.AppendChild(oNodeNew);
                }
                else
                {
                    attribute = oDoc.CreateAttribute("Weight");
                    attribute.Value = strValue;
                    oNodeNew.Attributes.Append(attribute);
                    attribute = oDoc.CreateAttribute("Type");
                    attribute.Value = "Weight";
                    oNodeNew.Attributes.Append(attribute);
                    oNode.AppendChild(oNodeNew);
                }
                itin = itin.Substring(61);
            }
            request = oDoc.OuterXml;
            request = request.Substring(request.IndexOf("<OriginDestinationInformation RPH=\"1\">"), (request.IndexOf("<TravelPreferences>") - request.IndexOf("<OriginDestinationInformation RPH=\"1\">")));
            return request;
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

            try
            {
                AmadeusWSAdapter ttAAPrice = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
                var avTempResponse = SendFareInformativeBestPricingWithoutPNR(ttAAPrice, req);

                if (req != null)
                {
                    if (fareindex < OTAPriceTotal.Length && fareindex < OTAReqTotal.Length && fareindex < avTempResponseTotal.Length)
                    {
                        OTAPriceTotal[fareindex] = "<OTA_AirPriceRS Version=\"2003.2\"><Errors><Error Type=\"Amadeus\" Code=\"911\">NO FARE FOR BOOKING CODE-TRY OTHER PRICING OPTIONS</Error></Errors></OTA_AirPriceRS>";
                        OTAReqTotal[fareindex] = req;
                        avTempResponse = avTempResponse.Replace($" xmlns=\"http://xml.amadeus.com/{ttProviderSystems.AmadeusWSSchema[Fare_InformativeBestPricingWithoutPNRReply]}\"", "");

                        avTempResponseTotal[fareindex] = avTempResponse;

                        avTempResponse = CoreLib.TransformXML($"<FIP>{req}{avTempResponse}</FIP>", XslPath, $"AmadeusWS_LowFareFlights1RS.xsl", false);

                        if (!string.IsNullOrEmpty(avTempResponse))
                        {
                            OTAPriceTotal[fareindex] = CoreLib.TransformXML(avTempResponse, XslPath, "AmadeusWS_AirPriceRS.xsl", false);
                        }
                        else
                        {
                            OTAPriceTotal[fareindex] = "<OTA_AirPriceRS Version=\"2003.2\"><Errors><Error Type=\"Amadeus\" Code=\"911\">NO FARE FOR BOOKING CODE-TRY OTHER PRICING OPTIONS</Error></Errors></OTA_AirPriceRS>";
                        }

                        if (OTAPriceTotal[fareindex] == null || OTAPriceTotal[fareindex].Length == 0)
                        {
                            OTAPriceTotal[fareindex] = "<OTA_AirPriceRS Version=\"2003.2\"><Errors><Error Type=\"Amadeus\" Code=\"911\">NO FARE FOR BOOKING CODE-TRY OTHER PRICING OPTIONS</Error></Errors></OTA_AirPriceRS>";
                        }

                        iFinishedPrices += 1;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private class RemovableFlights
        {
            public string SectorSequence { get; set; }
            public string DepartureDateTime { get; set; }
            public string FlightNumber { get; set; }
            public string ResBookDesigCode { get; set; }
        }

    }

}