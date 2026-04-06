using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath; // for XPathSelectElement / XPathSelectElements extensions
using TripXMLMain;
using WebSocketSharp;
using static TripXMLMain.modCore.enAmadeusWSSchema;

namespace AmadeusWS
{
    public class PNRServices : AmadeusBase
    {
        public PNRServices()
        {
            Request = "";
            ConversationID = "";
        }

        public string PNRRead()
        {
            string strResponse;

            //*****************************************************************
            // Transform OTA PNRRead Request into Native Amadeus Request     *
            //***************************************************************** 

            try
            {
                DateTime requestTime = DateTime.Now;

                // *******************
                // Create Session    *
                // *******************
                var ttAA = SetAdapter();
                ttAA.GetStoredFares = ttProviderSystems.GetStoredFares;
                bool inSession = SetConversationID(ttAA);

                var sbNativeLog = new StringBuilder();
                string strRequest = SetRequest($"AmadeusWS_PNRReadRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                bool isReturnFares = Request.ToUpper().Contains("RETURNFARES")
                    ? Request.ToUpper().Contains("RETURNFARES=\"TRUE\"")
                    : true;

                bool isReturnFlightInfo = Request.ToUpper().Contains("RETURNFARES")
                    ? Request.ToUpper().Contains("RETURNFARES=\"TRUE\"")
                    : true;

                #region Retrive PNR

                sbNativeLog.Append(strRequest);
                strResponse = strRequest.Contains("PNR_RetrieveByRecLoc")
                        ? SendRetrivePNRbyRL(ttAA, strRequest)
                        : SendRetrievePNR(ttAA, strRequest);
                sbNativeLog.Append($"{Environment.NewLine}{strResponse}");

                #endregion

                #region business logic
                if (ttProviderSystems.BLFile != "" && !strResponse.Contains("generalErrorInfo"))
                {
                    var oDocBL = new XmlDocument();
                    oDocBL.Load(ttProviderSystems.BLFile);

                    XmlElement oRootBL = oDocBL.DocumentElement;
                    XmlNode oNodeBL = oRootBL?.SelectSingleNode($"Security/ProviderBL[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\']");

                    if (oNodeBL != null)
                    {
                        // get accounting line from PNR
                        var oDocRespBL = new XmlDocument();
                        oDocRespBL.LoadXml(strResponse);

                        XmlElement oRootRespBL = oDocRespBL.DocumentElement;
                        XmlNode oNodeRespBL = oRootRespBL?.SelectSingleNode("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']");

                        if (oNodeRespBL != null)
                        {

                            if (oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']") != null)
                            {
                                string strAIAN = oNodeRespBL.SelectSingleNode("accounting/account/number").InnerXml;
                                string strAIN_BL = oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']/@AuthorizeCode").InnerXml;

                                //compare to the one in BL file 

                                if (strAIAN != strAIN_BL)
                                {
                                    throw new Exception("Secured PNR");
                                }
                            }
                        }
                    }
                }
                #endregion

                #region TST

                string strResponseTST = "";
                if (strResponse.Contains("<longFreetext>--- TST ") && isReturnFares && ttAA.GetStoredFares)
                {
                    sbNativeLog.Append($"{Environment.NewLine}{strRequest}");
                    strResponseTST = SendDisplayTST(ttAA);
                    sbNativeLog.Append($"{Environment.NewLine}{strRequest}");

                    if (Version.Equals("v04_"))
                    {
                        var oDocTST = new XmlDocument();
                        oDocTST.LoadXml(strResponseTST);
                        XmlElement oRootTST = oDocTST.DocumentElement;

                        var xmlNodeList = oRootTST?.SelectNodes("fareList");
                        if (xmlNodeList != null)
                        {
                            foreach (XmlNode oNodeTST in xmlNodeList)
                            {
                                string strRequestTST = oNodeTST.SelectSingleNode("fareReference/uniqueReference").InnerText;

                                sbNativeLog.Append($"{Environment.NewLine}{strRequest}");
                                strResponseTST += SendCommandCryptically(ttAA, $"TQT/T{strRequestTST}");
                                sbNativeLog.Append($"{Environment.NewLine}{strRequest}");
                            }
                        }
                    }

                }

                #endregion

                #region GetPricingOptions

                var GetPricingOptionsTST = string.Empty;

                if (strResponse.Contains("<longFreetext>--- TST ") && isReturnFares && ttAA.GetStoredFares && strResponseTST.Contains("<referenceType>TST</referenceType>"))
                {
                    //var tstNum = strResponseTST.Substring(strResponseTST.IndexOf("<referenceType>TST</referenceType>", StringComparison.Ordinal));
                    //tstNum = tstNum.Substring(tstNum.IndexOf("<uniqueReference>") + "<uniqueReference>".Length, tstNum.IndexOf("</uniqueReference>") - tstNum.IndexOf("<uniqueReference>") - "<uniqueReference>".Length);

                    var oDocTST = new XmlDocument();
                    oDocTST.LoadXml(strResponseTST);
                    XmlElement oRootTST = oDocTST.DocumentElement;
                    var xmlNodeList = oRootTST?.SelectNodes("fareList");

                    if (xmlNodeList != null)
                        foreach (XmlNode oNodeTST in xmlNodeList)
                        {
                            string tstPricingOption = string.Empty;
                            string tstNum = oNodeTST.SelectSingleNode("fareReference/uniqueReference").InnerText;
                            strRequest = $"<Ticket_GetPricingOptions xmlns=\"http://xml.amadeus.com/TPORRQ_14_1_1A\"><documentSelection><referenceType>TST</referenceType><uniqueReference>{tstNum}</uniqueReference></documentSelection></Ticket_GetPricingOptions>";

                            sbNativeLog.Append($"{Environment.NewLine}{strRequest}");
                            tstPricingOption = SendGetPricingOptions(ttAA, strRequest);
                            sbNativeLog.Append($"{Environment.NewLine}{strRequest}");

                            //AK: I commented it in order to return PNR even if TST is not active.
                            //if (tstPricingOption.Contains("<Error>"))
                            //    throw new Exception(tstPricingOption);

                            GetPricingOptionsTST += tstPricingOption;

                        }
                }
                #endregion

                #region SUBSIDIARY/FRANCHISE
                string strRTSVC = string.Empty;
                if (strResponse.Contains("SUBSIDIARY/FRANCHISE"))
                {
                    sbNativeLog.Append($"{Environment.NewLine}{strRequest}");
                    strRTSVC = SendCommandCryptically(ttAA, "RTSVC");
                    sbNativeLog.Append($"{Environment.NewLine}{strRequest}");
                }

                #endregion

                #region RTSVI
                string strRTSVI = string.Empty;

                if (!strResponse.Contains("<Errors><Error>"))
                {
                    strRTSVI = SendCommandCryptically(ttAA, "RTSVI");

                    var oDocCryptic = new XmlDocument();
                    oDocCryptic.LoadXml(strRTSVI);
                    XmlElement oRootCryptic = oDocCryptic.DocumentElement;

                    string strScreen = oRootCryptic.SelectSingleNode("longTextString/textStringDetails") != null
                        ? oRootCryptic.SelectSingleNode("longTextString/textStringDetails").InnerText
                        : oRootCryptic.SelectSingleNode("Error").InnerText;

                    strRTSVI = formatAmadeus(strScreen);
                    if (!string.IsNullOrEmpty(strRTSVI))
                    {
                        strRTSVI = $"<RTSVI>{strRTSVI.Replace("&", "&amp;")}</RTSVI>";
                    }
                }
                #endregion

                #region Split PNR Check
                string strSplitPNR = string.Empty;
                if (strResponse.Contains("<segmentName>SP</segmentName>"))
                {
                    var oDocSP = new XmlDocument();
                    oDocSP.LoadXml(strResponse);
                    XmlElement oRootSP = oDocSP.DocumentElement;

                    XmlNode oNodeSP = oRootSP.SelectSingleNode("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='SP']/referencedRecord/referencedReservationInfo/reservation/controlNumber");

                    if (oNodeSP != null)
                    {
                        string strSP = oNodeSP.InnerText;
                        string strRTV = $"<PNR_RetrieveByRecLoc><sbrRecLoc><reservation><controlNumber>{strSP}</controlNumber></reservation></sbrRecLoc></PNR_RetrieveByRecLoc>";
                        sbNativeLog.Append($"{Environment.NewLine}{strRTV}");
                        string strResponseSP = SendRetrivePNRbyRL(ttAA, strRTV);
                        sbNativeLog.Append($"{Environment.NewLine}{strResponseSP}");
                        //------------------------------------------

                        oDocSP = new XmlDocument();
                        oDocSP.LoadXml(strResponseSP);
                        oRootSP = oDocSP.DocumentElement;

                        XmlNodeList oNodeSPL = oRootSP.SelectNodes("travellerInfo");

                        if (oNodeSPL != null && oNodeSPL.Count > 0)
                        {
                            strSplitPNR = $"<SplitPNR>AXR:{strSP}:{oNodeSPL.Count}</SplitPNR>";
                        }
                    }
                }
                #endregion

                #region Process Coupon
                string strCouponStatus = string.Empty;
                if (ttProviderSystems.CouponStatus)
                {
                    if (strResponse.Contains("<segmentName>FA</segmentName>"))
                    {
                        var oDocCS = new XmlDocument();
                        oDocCS.LoadXml(strResponse);
                        XmlElement oRootCS = oDocCS.DocumentElement;

                        string tktnum = oRootCS.SelectSingleNode("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA']/otherDataFreetext/longFreetext").InnerText;
                        tktnum = tktnum.Substring(4, 14).Replace("-", "");

                        strRequest = "<Ticket_ProcessETicket><msgActionDetails><messageFunctionDetails><messageFunction>131</messageFunction></messageFunctionDetails></msgActionDetails><ticketInfoGroup><ticket><documentDetails><number>" + tktnum + "</number></documentDetails></ticket></ticketInfoGroup></Ticket_ProcessETicket>";
                        sbNativeLog.Append($"{Environment.NewLine}{strRequest}");
                        strCouponStatus = SendETicketProcess(ttAA, strRequest);
                        sbNativeLog.Append($"{Environment.NewLine}{strCouponStatus}");

                    }
                }
                #endregion

                #region Process Electronic Manual or Automated Ticket                        

                if (strResponse.Contains("<segmentName>FHE</segmentName>") || strResponse.Contains("<segmentName>FHA</segmentName>") || strResponse.Contains("<segmentName>FHM</segmentName>"))
                {
                    string strManualTicket = "<ManualTickets>";
                    var oDocCS = new XmlDocument();
                    oDocCS.LoadXml(strResponse);
                    XmlElement oRootMT = oDocCS.DocumentElement;
                    if (strResponse.Contains("<segmentName>FHE</segmentName>"))
                        strManualTicket += ProcessFHTickets(ttAA, oRootMT.SelectNodes("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FHE']/otherDataFreetext"));

                    if (strResponse.Contains("<segmentName>FHA</segmentName>"))
                        strManualTicket += ProcessFHTickets(ttAA, oRootMT.SelectNodes("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FHA']/otherDataFreetext"));

                    if (strResponse.Contains("<segmentName>FHM</segmentName>"))
                        strManualTicket += ProcessFHTickets(ttAA, oRootMT.SelectNodes("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FHM']/otherDataFreetext"));

                    strManualTicket += "</ManualTickets>";
                    strResponse = strResponse.Replace("</PNR_Reply>", $"{strManualTicket}</PNR_Reply>");
                }

                #endregion

                //*****************************************************************
                // Transform Native Amadeus PNRRead Response into OTA Response   *
                //***************************************************************** 

                var strToReplace = "</PNR_RetrieveByRecLocReply>";
                if (!strResponse.Contains(strToReplace))
                    strToReplace = "</PNR_Reply>";
                strResponse = strResponse.Replace(strToReplace, inSession
                    ? $"{strResponseTST}{strRTSVC}{strRTSVI}{strSplitPNR}{strCouponStatus}<ConversationID>{ConversationID}</ConversationID>{strToReplace}"
                    : $"{strResponseTST}{strRTSVC}{strRTSVI}{strSplitPNR}{strCouponStatus}{strToReplace}");

                if (!string.IsNullOrEmpty(GetPricingOptionsTST))
                    strResponse = strResponse.Replace(strToReplace, $"{GetPricingOptionsTST}{strToReplace}");

                //CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response size", strResponse.Length.ToString(CultureInfo.InvariantCulture), ttProviderSystems.LogUUID);
                CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response", strResponse, ttProviderSystems.LogUUID);

                // check if fare rule requested to be returned
                if (Request.Contains("ReturnRules=\"true\""))
                {
                    string strRulesRQ = CoreLib.TransformXML(strResponse, XslPath, "AmadeusWS_PNRReadRS_Rules.xsl");
                    var ttAir = new AirServices
                    {
                        Request = strRulesRQ,
                        Version = "v03",
                        ttProviderSystems = ttProviderSystems
                    };

                    string strRulesRS = ttAir.AirRules();
                    strResponse = strResponse.Replace(strToReplace, $"{strRulesRS}{strToReplace}");
                }

                // check if flight info requested to be returned
                if (isReturnFlightInfo && !strResponse.Contains("<Errors><Error>"))
                {
                    string strFlifoRQ = CoreLib.TransformXML(strResponse, XslPath, "AmadeusWS_PNRReadRS_FlightInfo.xsl");
                    string strFlifoRS = "";

                    var oDocFlifo = new XmlDocument();
                    oDocFlifo.LoadXml(strFlifoRQ);
                    XmlElement oRootFlifo = oDocFlifo.DocumentElement;

                    foreach (XmlNode oNode in oRootFlifo)
                    {
                        sbNativeLog.Append($"{Environment.NewLine}{oNode.OuterXml}");
                        strFlifoRS = SendFlightInfo(ttAA, oNode.OuterXml);
                        sbNativeLog.Append($"{Environment.NewLine}{strFlifoRS}");
                    }

                    strResponse = strResponse.Replace("</PNR_RetrieveByRecLocReply>", $"{strFlifoRS}</PNR_RetrieveByRecLocReply>");
                }


                strResponse = ConvertToTripXMLMessage(ttAA, Request, inSession, strResponse);
                try
                {


                    DateTime responseTime = DateTime.Now;
                    String strMeessege = sbNativeLog.ToString();
                    sbNativeLog.Remove(0, sbNativeLog.Length);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
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
                //-------------------------------------------
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRRead, exx.Message, ttProviderSystems);
            }
            return strResponse;
        }

        public string PNRCancel()
        {
            XmlNodeList oNodeList = null;
            string strResponse;
            string strRecLocator = "";
            var sbNativeLog = new StringBuilder();
            try
            {
                // *******************
                // Create Session    *
                // *******************
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                DateTime requestTime = DateTime.Now;
                //*****************************************************************
                // Transform OTA PNRCancel Request into Several Navite Requests    *
                //***************************************************************** 
                string strRequest = SetRequest($"AmadeusWS_PNRCancelRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //********************
                // Get All Requests  * 
                //********************

                string strRetrieve = string.Empty;
                string strCancelHotel = string.Empty;
                string strCancel = string.Empty;
                string strEndTransaction = string.Empty;
                XmlDocument oDoc;
                XmlElement oRoot;
                try
                {
                    oDoc = new XmlDocument();
                    oDoc.LoadXml(strRequest);
                    oRoot = oDoc.DocumentElement;

                    if (oRoot != null)
                    {
                        var nodeRetrieve = oRoot.SelectSingleNode("Retrieve");
                        if (nodeRetrieve != null)
                            strRetrieve = nodeRetrieve.InnerXml;
                        var nodeControlNumber = oRoot.SelectSingleNode("Retrieve/PNR_RetrieveByRecLoc/sbrRecLoc/reservation/controlNumber");
                        if (nodeControlNumber != null)
                            strRecLocator = nodeControlNumber.InnerText;
                        var nodeCancel = oRoot.SelectSingleNode("Cancel");
                        if (nodeCancel != null)
                            strCancel = nodeCancel.InnerXml;
                        var nodeCancelHotel = oRoot.SelectSingleNode("CancelHotel");
                        if (nodeCancelHotel != null)
                            strCancelHotel = nodeCancelHotel.InnerXml;
                        var nodeET = oRoot.SelectSingleNode("ET");
                        if (nodeET != null)
                            strEndTransaction = nodeET.InnerXml;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Loading Transformed Request XML Document.\r\n{ex.Message}");
                }

                sbNativeLog.Append($"{Environment.NewLine}{strRetrieve}");
                strResponse = SendRetrivePNRbyRL(ttAA, strRetrieve);
                sbNativeLog.Append($"{Environment.NewLine}{strResponse}");


                // *******************
                // Check for Errors  *
                // *******************

                bool blnHotelSegs;
                try
                {
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;
                    if (oRoot != null)
                    {
                        if (oRoot.SelectSingleNode("Error") != null)
                        {
                            throw new Exception(oRoot.SelectSingleNode("Error").InnerText);
                        }

                        if (oRoot.SelectSingleNode("generalErrorInfo") != null && oRoot.SelectSingleNode("generalErrorInfo").SelectSingleNode("messageErrorInformation").SelectSingleNode("errorDetail").SelectSingleNode("qualifier").InnerText != "INF")
                        {
                            throw new Exception(oRoot.SelectSingleNode("generalErrorInfo").SelectSingleNode("messageErrorText").SelectSingleNode("text").InnerText);
                        }

                        if (oRoot.SelectSingleNode("CAPI_Messages") != null)
                        {
                            if (oRoot.SelectSingleNode("CAPI_Messages/ErrorCode") != null)
                            {
                                throw new Exception(oRoot.SelectSingleNode("CAPI_Messages/Text").InnerText);
                            }
                        }

                        # region business logic
                        if (ttProviderSystems.BLFile != "" && strResponse.IndexOf("generalErrorInfo") == -1)
                        {
                            XmlDocument oDocBL = new XmlDocument();
                            oDocBL.Load(ttProviderSystems.BLFile);

                            XmlElement oRootBL = oDocBL.DocumentElement;
                            XmlNode oNodeBL = oRootBL.SelectSingleNode("Security/ProviderBL[@Name=\'Amadeus\'][@System=\'" + ttProviderSystems.System + "\']");

                            if (oNodeBL != null)
                            {
                                // get accounting line from PNR
                                XmlDocument oDocRespBL = new XmlDocument();
                                oDocRespBL.LoadXml(strResponse);

                                XmlElement oRootRespBL = oDocRespBL.DocumentElement;
                                XmlNode oNodeRespBL = oRootRespBL.SelectSingleNode("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']");

                                if (oNodeRespBL != null)
                                {

                                    if (!(oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']") == null))
                                    {
                                        string strAIAN = oNodeRespBL.SelectSingleNode("accounting/account/number").InnerXml;
                                        string strAIN_BL = oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']/@AuthorizeCode").InnerXml;

                                        //compare to the one in BL file 
                                        if (strAIAN != strAIN_BL)
                                        {
                                            oDocBL = null;
                                            oDocRespBL = null;
                                            throw new Exception("Secured PNR");
                                        }
                                    }
                                }

                                oDocRespBL = null;
                            }

                            oDocBL = null;

                        }
                        # endregion

                        // *******************************************
                        // Find out if the PNR has Hotel Segments    *
                        // *******************************************
                        oNodeList = oRoot.SelectNodes("originDestinationDetails/itineraryInfo/elementManagementItinerary[segmentName = 'HHL']");
                    }

                    blnHotelSegs = oNodeList == null ? false : oNodeList.Count > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error.\r\n{ex.Message}");
                }


                //****************************************************************************
                // Send Cancel (11) Request or Send Cancel (0) and End Transaction Request   *
                //****************************************************************************
                if (blnHotelSegs)
                {
                    sbNativeLog.Append(Environment.NewLine + strCancelHotel);
                    strResponse = SendCancelPNR(ttAA, strCancelHotel);
                    sbNativeLog.Append($"{Environment.NewLine}{strResponse}");


                    sbNativeLog.Append($"{Environment.NewLine}{strEndTransaction}");
                    strEndTransaction = SendAddMultiElements(ttAA, strEndTransaction);
                    sbNativeLog.Append($"{Environment.NewLine}{strEndTransaction}");
                }
                else
                {
                    if (strCancel.Contains("Command_Cryptic"))
                    {
                        sbNativeLog.Append($"{Environment.NewLine}{strCancel}");
                        strResponse = SendCommandCryptically(ttAA, strCancel);
                        sbNativeLog.Append($"{Environment.NewLine}{strResponse}");

                        sbNativeLog.Append($"{Environment.NewLine}{strEndTransaction}");
                        strResponse = SendAddMultiElements(ttAA, strEndTransaction);
                        sbNativeLog.Append($"{Environment.NewLine}{strResponse}");
                    }
                    else
                    {
                        sbNativeLog.Append($"{Environment.NewLine}{strCancel}");
                        strResponse = SendCancelPNR(ttAA, strCancel);
                        sbNativeLog.Append($"{Environment.NewLine}{strResponse}");

                        if (strResponse.Contains("RESTRICTED / USE XE"))
                        {
                            string strSegments = "";

                            foreach (XmlNode oSeg in oRoot.SelectNodes("originDestinationDetails/itineraryInfo"))
                            {
                                if (oSeg.SelectNodes("relatedProduct/status").Count == 1 && oSeg.SelectSingleNode("elementManagementItinerary/segmentName").InnerText != "RU")
                                {
                                    strSegments += $"<element><identifier>ST</identifier><number>{oSeg.SelectSingleNode("elementManagementItinerary/reference/number").InnerText}</number></element>";
                                }
                            }

                            if (strSegments != "")
                            {
                                strCancel = "<PNR_Cancel><pnrActions><optionCode>11</optionCode></pnrActions><cancelElements><entryType>E</entryType>" + strSegments + "</cancelElements></PNR_Cancel>";
                                strResponse = SendCancelPNR(ttAA, strCancel);
                                sbNativeLog.Append($"{Environment.NewLine}{strResponse}");
                            }
                            else
                            {
                                strResponse = "<Error>No active segment to cancel</Error>";
                            }
                        }
                    }
                    //-------------------------------------------------------
                }


                //******************************************************************
                // Transform Native Amadeus PNRCancel Response into OTA Response   *
                //******************************************************************
                try
                {
                    if (inSession)
                    {
                        strResponse = strResponse.Replace("</PNR_Reply>", $"<ConversationID>{ConversationID}</ConversationID></PNR_Reply>");
                    }
                    strResponse = strResponse.Replace("<PNR_Reply xmlns=\"http://xml.amadeus.com/PNRACC_08_3_1A\">", "<PNR_Reply>");
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_PNRCancelRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }

                if (strResponse.Contains("<UniqueID ID=\"\""))
                    strResponse = strResponse.Replace("<UniqueID ID=\"\"", $"<UniqueID ID=\"{strRecLocator}\"");

                DateTime responseTime = DateTime.Now;
                string strMessage = sbNativeLog.ToString();
                sbNativeLog.Remove(0, sbNativeLog.Length);
            }

            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRCancel, exx.Message, ttProviderSystems, strRecLocator);
            }

            return strResponse;
        }

        public string PNRSplit()
        {

            string strResponse;
            string strRecLocator = "";
            var sbNativeLog = new StringBuilder();
            try
            {
                // *******************
                // Create Session    *
                // *******************
                AmadeusWSAdapter ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);
                DateTime requestTime = DateTime.Now;

                //*****************************************************************
                // Transform OTA PNRSplit Request into Several Navite Requests    *
                //***************************************************************** 
                string strRequest = SetRequest($"AmadeusWS_PNRSplitRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //********************
                // Get All Requests  * 
                //********************

                string strRetrieve;
                string strNewPNR;
                string strSplit;
                string strSplitRequest;
                string strEndTransaction;
                XmlDocument oDoc;
                XmlElement oRoot;
                try
                {
                    oDoc = new XmlDocument();
                    oDoc.LoadXml(strRequest);
                    oRoot = oDoc.DocumentElement;

                    strRetrieve = oRoot.SelectSingleNode("Retrieve").InnerXml;
                    strSplitRequest = oRoot.SelectSingleNode("SplitRequest").InnerXml;
                    strSplit = oRoot.SelectSingleNode("Split").InnerXml;
                    strEndTransaction = oRoot.SelectSingleNode("ET").InnerXml;
                    strNewPNR = oRoot.SelectSingleNode("GetNewPNR").InnerXml;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Loading Transformed Request XML Document.\r\n{ex.Message}");
                }

                sbNativeLog.Append($"{Environment.NewLine}{strRetrieve}");
                strResponse = SendRetrivePNRbyRL(ttAA, strRetrieve);
                sbNativeLog.Append($"{Environment.NewLine}{strResponse}");

                // *******************
                // Check for Errors  *
                // *******************
                try
                {
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;

                    if (oRoot.SelectSingleNode("Error") != null)
                    {
                        throw new Exception(oRoot.SelectSingleNode("Error").InnerText);
                    }
                    else if (oRoot.SelectSingleNode("generalErrorInfo") != null && oRoot.SelectSingleNode("generalErrorInfo").SelectSingleNode("messageErrorInformation").SelectSingleNode("errorDetail").SelectSingleNode("qualifier").InnerText != "INF")
                    {
                        throw new Exception(oRoot.SelectSingleNode("generalErrorInfo").SelectSingleNode("messageErrorText").SelectSingleNode("text").InnerText);
                    }

                    # region business logic
                    if (ttProviderSystems.BLFile != "" && !strResponse.Contains("generalErrorInfo"))
                    {
                        var oDocBL = new XmlDocument();
                        oDocBL.Load(ttProviderSystems.BLFile);

                        XmlElement oRootBL = oDocBL.DocumentElement;
                        XmlNode oNodeBL = oRootBL.SelectSingleNode("Security/ProviderBL[@Name=\'Amadeus\'][@System=\'" + ttProviderSystems.System + "\']");

                        if (oNodeBL != null)
                        {
                            // get accounting line from PNR
                            var oDocRespBL = new XmlDocument();
                            oDocRespBL.LoadXml(strResponse);

                            XmlElement oRootRespBL = oDocRespBL.DocumentElement;
                            XmlNode oNodeRespBL = oRootRespBL.SelectSingleNode("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']");

                            if (oNodeRespBL != null)
                            {
                                if (!(oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']") == null))
                                {
                                    string strAIAN = oNodeRespBL.SelectSingleNode("accounting/account/number").InnerXml;
                                    string strAIN_BL = oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']/@AuthorizeCode").InnerXml;

                                    //compare to the one in BL file 
                                    if (strAIAN != strAIN_BL)
                                    {
                                        throw new Exception("Secured PNR");
                                    }
                                }
                            }
                        }
                    }
                    # endregion

                    var oDocResp = new XmlDocument();
                    oDocResp.LoadXml(strSplitRequest);
                    XmlElement oRootResp = oDocResp.DocumentElement;

                    foreach (XmlNode paxnode in oRootResp.SelectNodes("splitDetails/passenger"))
                    {
                        string strLineNo = paxnode.SelectSingleNode("tattoo").InnerText;
                        string strTatoo = oRoot.SelectSingleNode($"travellerInfo[elementManagementPassenger/lineNumber ='{strLineNo}']/elementManagementPassenger/reference/number").InnerText;
                        paxnode.SelectSingleNode("tattoo").InnerText = strTatoo;
                    }

                    foreach (XmlNode paxnode in oRootResp.SelectNodes("splitDetails/otherElement"))
                    {
                        string strLineNo = paxnode.SelectSingleNode("tattoo").InnerText;
                        string strTatoo = oRoot.SelectSingleNode($"travellerInfo[elementManagementPassenger/lineNumber ='{strLineNo}']/elementManagementPassenger/reference/number").InnerText;
                        paxnode.SelectSingleNode("tattoo").InnerText = strTatoo;
                    }

                    strSplitRequest = oRootResp.OuterXml;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error.\r\n{ex.Message}");
                }

                sbNativeLog.Append($"{Environment.NewLine}{strSplitRequest}");
                strResponse = SendSplitPNR(ttAA, strSplitRequest);
                sbNativeLog.Append($"{Environment.NewLine}{strResponse}");

                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                if (oRoot.SelectSingleNode("Error") != null)
                {
                    throw new Exception(oRoot.SelectSingleNode("Error").InnerText);
                }

                if (oRoot.SelectSingleNode("generalErrorInfo") != null && oRoot.SelectSingleNode("generalErrorInfo").SelectSingleNode("messageErrorInformation").SelectSingleNode("errorDetail").SelectSingleNode("qualifier").InnerText != "INF")
                {
                    throw new Exception(oRoot.SelectSingleNode("generalErrorInfo").SelectSingleNode("messageErrorText").SelectSingleNode("text").InnerText);
                }

                sbNativeLog.Append($"{Environment.NewLine}{strSplit}");
                strResponse = SendAddMultiElements(ttAA, strSplit);
                sbNativeLog.Append($"{Environment.NewLine}{strResponse}");

                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                if (oRoot.SelectSingleNode("Error") != null)
                {
                    throw new Exception(oRoot.SelectSingleNode("Error").InnerText);
                }
                else if (oRoot.SelectSingleNode("generalErrorInfo") != null && oRoot.SelectSingleNode("generalErrorInfo").SelectSingleNode("messageErrorInformation").SelectSingleNode("errorDetail").SelectSingleNode("qualifier").InnerText != "INF")
                {
                    throw new Exception(oRoot.SelectSingleNode("generalErrorInfo").SelectSingleNode("messageErrorText").SelectSingleNode("text").InnerText);
                }

                sbNativeLog.Append($"{Environment.NewLine}{strEndTransaction}");
                strResponse = SendAddMultiElements(ttAA, strEndTransaction);
                sbNativeLog.Append($"{Environment.NewLine}{strResponse}");

                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                if (oRoot.SelectSingleNode("Error") != null)
                {
                    throw new Exception(oRoot.SelectSingleNode("Error").InnerText);
                }

                if (oRoot.SelectSingleNode("generalErrorInfo") != null && oRoot.SelectSingleNode("generalErrorInfo").SelectSingleNode("messageErrorInformation").SelectSingleNode("errorDetail").SelectSingleNode("qualifier").InnerText != "INF")
                {
                    throw new Exception(oRoot.SelectSingleNode("generalErrorInfo").SelectSingleNode("messageErrorText").SelectSingleNode("text").InnerText);
                }

                strRecLocator = oRoot.SelectSingleNode("pnrHeader/reservationInfo/reservation/controlNumber").InnerText;

                strNewPNR = strNewPNR.Replace("XXXXXX", strRecLocator);

                sbNativeLog.Append(Environment.NewLine + strNewPNR);
                strResponse = SendRetrivePNRbyRL(ttAA, strNewPNR);

                //*****************************************************************
                // Transform Native Amadeus PNRSplit Response into OTA Response   *
                //***************************************************************** 
                try
                {
                    Version = "v03_";
                    var strToReplace = "</PNR_RetrieveByRecLocReply>";
                    if (!strResponse.Contains(strToReplace))
                        strToReplace = "</PNR_Reply>";
                    if (inSession)
                    {
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID>{ConversationID}</ConversationID>{strToReplace}");
                    }
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_PNRReadRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = "";
                    }
                }

                DateTime responseTime = DateTime.Now;
                string strMessage = sbNativeLog.ToString();
                sbNativeLog.Remove(0, sbNativeLog.Length);
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRSplit, exx.Message, ttProviderSystems, strRecLocator);
            }

            return strResponse;
        }

        public string Queue()
        {
            string strResponse = "";
            try
            {
                //*****************************************************************
                // Transform OTA Queue Request into Native Amadeus Request     *
                //***************************************************************** 
                string strRequest = SetRequest($"AmadeusWS_QueueRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //*******************************************************************************
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                //******************************************************************************* 
                AmadeusWSAdapter ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                if (Request.Contains("ListQueue"))
                {
                    strResponse = SendListQueue(ttAA, strRequest);
                }
                else if (Request.Contains("CountQueue"))
                {
                    strResponse = SendCountQueue(ttAA, strRequest);
                }
                else if (Request.Contains("RemoveQueue"))
                {
                    strResponse = SendRemoveQueue(ttAA, strRequest);
                }
                else if (Request.Contains("BounceQueue"))
                {
                    strResponse = SendBounceQueue(ttAA, strRequest);
                }
                else if (Request.Contains("Move"))
                {
                    strResponse = SendQueueMove(ttAA, strRequest);
                }
                else if (Request.Contains("PlaceQueue"))
                {
                    strResponse = SendPlaceQueue(ttAA, strRequest);
                }


                //*******************************************************************************
                // check if message is queue lists and if need need to scroll the response      *
                //******************************************************************************* 
                try
                {

                    if (strResponse.StartsWith("<Queue_ListReply>") & !strResponse.Contains("errorReturn"))
                    {
                        // get total number of pnr in response and in request
                        var oDocResp = new XmlDocument();
                        oDocResp.LoadXml(strResponse);
                        XmlElement oRootResp = oDocResp.DocumentElement;

                        var oDocReq = new XmlDocument();
                        oDocReq.LoadXml(strRequest);
                        XmlElement oRootReq = oDocReq.DocumentElement;

                        int iPNRsResp = Convert.ToInt32(oRootResp.SelectSingleNode("queueView/pnrCount[1]/quantityDetails/numberOfUnit").InnerText);
                        int iPNRsReq = Convert.ToInt32(oRootReq.SelectSingleNode("scanRange/rangeDetails/max").InnerText);

                        if (iPNRsResp > iPNRsReq)
                        {
                            strRequest = strRequest.Replace("<Queue_List>", "<Queue_List><scroll><numberOfItemsDetails><actionQualifier>54</actionQualifier></numberOfItemsDetails></scroll>");
                            oDocReq.LoadXml(strRequest);
                            oRootReq = oDocReq.DocumentElement;
                        }

                        while (iPNRsResp > iPNRsReq)
                        {
                            oRootReq.SelectSingleNode("scanRange/rangeDetails/min").InnerText = oRootReq.SelectSingleNode("scanRange/rangeDetails/min").InnerText + 200;
                            iPNRsReq = iPNRsReq + 200;
                            oRootReq.SelectSingleNode("scanRange/rangeDetails/max").InnerText = Convert.ToString(iPNRsReq);
                            strRequest = oRootReq.OuterXml;

                            string strResponseQL = SendListQueue(ttAA, strRequest);

                            oDocResp.LoadXml(strResponseQL);
                            oRootResp = oDocResp.DocumentElement;
                            iPNRsResp = Convert.ToInt32(oRootResp.SelectSingleNode("queueView/pnrCount[1]/quantityDetails/numberOfUnit").InnerText);
                            strResponse = strResponse.Replace("</Queue_ListReply>", $"{oRootResp.InnerXml}</Queue_ListReply>");
                        }
                    }

                    try
                    {
                        var tagToReplace = "</Queue_PlacePNRReply>";
                        if (Request.Contains("ListQueue"))
                        {
                            tagToReplace = "</Queue_ListReply>";
                        }
                        else if (Request.Contains("CountQueue"))
                        {
                            tagToReplace = "</Queue_CountTotalReply>";
                        }
                        else if (Request.Contains("RemoveQueue"))
                        {
                            tagToReplace = "</Queue_RemoveItemReply>";
                        }
                        else if (Request.Contains("BounceQueue"))
                        {
                            tagToReplace = "</MessagesOnly_Reply>";
                        }
                        else if (Request.Contains("Move"))
                        {
                            tagToReplace = "</Queue_MoveItemReply>";
                        }
                        else if (Request.Contains("PlaceQueue"))
                        {
                            tagToReplace = "</Queue_PlacePNRReply>";
                        }

                        if (inSession)
                            strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                        strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_QueueRS.xsl");
                        return strResponse;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                    }

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = "";
                    }
                    ttAA = null;
                }

            }
            catch (Exception ex)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Queue, ex.Message, ttProviderSystems);
            }
            return strResponse;
        }

        public string QueueRead()
        {
            string strResponse = "";

            try
            {
                AmadeusWSAdapter ttAA = SetAdapter();
                ttAA.GetStoredFares = ttProviderSystems.GetStoredFares;
                bool inSession = SetConversationID(ttAA);

                string strRequest = SetRequest($"AmadeusWS_QueueReadRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //*******************************************************************************
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                //******************************************************************************* 

                try
                {

                    string strMessage;
                    if (Request.Contains("AccessQueue"))
                    {
                        strMessage = "AccessQueue";
                        inSession = true;
                    }
                    else
                    {
                        if (Request.Contains("ItemOnQueue"))
                        {
                            strMessage = Request.Contains("Redisplay") ? "Redisplay" : "ItemOnQueue";
                            //inSession = true;
                        }
                        else
                            strMessage = "ExitQueue";
                    }

                    if (strMessage != "Redisplay")
                    {
                        // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                        strResponse = strRequest.Contains("Command_Cryptic")
                                    ? SendRequestCryptically(ttAA, strRequest)
                                    : SendQueueRequest(ttAA, strRequest);

                        if (strResponse.Contains("|Session|"))
                        {
                            CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Session Error", strResponse.Substring(0, strResponse.Length / 2), ttProviderSystems.LogUUID);
                            //Try update SessionID and reRun previos command.                            
                            strResponse = strRequest.Contains("Command_Cryptic")
                                    ? SendRequestCryptically(ttAA, strRequest)
                                    : SendQueueRequest(ttAA, strRequest);

                        }
                    }

                    // Check PNR or Errors in Native Response
                    if (strResponse.StartsWith("<Error"))
                    {
                        if (strResponse.Contains("invalid character"))
                        {
                            strRequest = SendRetrievePNR(ttAA);
                            strResponse = $"{strResponse.Substring(20, strResponse.Length - 37)} in PNR {strRequest.Substring(92, 6)}";
                        }
                        strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, ttProviderSystems, "", "v03");
                        /**** old code ******
                        ttAA.CloseSession(ConversationID);
                        ConversationID = null;
                        ************************/

                        inSession = false;
                        return strResponse;
                    }

                    if ((strMessage == "Redisplay") || (strMessage == "AccessQueue" && strResponse.Contains("RP/")) || (strMessage == "ItemOnQueue" && strResponse.Contains("RP/"))
                        || (strMessage == "AccessQueue" && strResponse.Contains("<reservation><controlNumber>")) || (strMessage == "ItemOnQueue" && strResponse.Contains("<reservation><controlNumber>")))
                    {
                        string strWarning = "";
                        if (strResponse.Contains("QUEUE CYCLE COMPLETE"))
                            strWarning = "<Warning Type=\"Amadeus\">QUEUE CYCLE COMPLETE</Warning>";

                        // Send PNR Redisplay                    
                        strResponse = SendRetrievePNR(ttAA);

                        var GetPricingOptionsTST = string.Empty;

                        if (strResponse.Contains("<longFreetext>--- TST ") && ttAA.GetStoredFares)
                        {
                            var strResponseTST = SendDisplayTST(ttAA);

                            #region GetPricingOptions

                            if (strResponseTST.Contains("<referenceType>TST</referenceType>"))
                            {
                                var oDocTST = new XmlDocument();
                                oDocTST.LoadXml(strResponseTST);
                                XmlElement oRootTST = oDocTST.DocumentElement;
                                var xmlNodeList = oRootTST?.SelectNodes("fareList");

                                if (xmlNodeList != null)
                                    foreach (XmlNode oNodeTST in xmlNodeList)
                                    {
                                        string tstPricingOption = string.Empty;
                                        string tstNum = oNodeTST.SelectSingleNode("fareReference/uniqueReference").InnerText;
                                        strRequest = $"<Ticket_GetPricingOptions xmlns=\"http://xml.amadeus.com/TPORRQ_14_1_1A\"><documentSelection><referenceType>TST</referenceType><uniqueReference>{tstNum}</uniqueReference></documentSelection></Ticket_GetPricingOptions>";

                                        //sbNativeLog.Append($"{Environment.NewLine}{strRequest}");
                                        tstPricingOption = SendGetPricingOptions(ttAA, strRequest);
                                        //sbNativeLog.Append($"{Environment.NewLine}{strRequest}");

                                        //AK: I commented it in order to return PNR even if TST is not active.
                                        //if (tstPricingOption.Contains("<Error>"))
                                        //    throw new Exception(tstPricingOption);

                                        GetPricingOptionsTST += tstPricingOption;

                                    }
                            }
                            else
                            {
                                GetPricingOptionsTST = string.Empty;
                            }

                            #endregion

                            if (Version == "v04_")
                            {
                                var oDocTST = new XmlDocument();
                                oDocTST.LoadXml(strResponseTST);
                                XmlElement oRootTST = oDocTST.DocumentElement;

                                foreach (XmlNode oNodeTST in oRootTST.SelectNodes("fareList"))
                                {
                                    string strRequestTST = oNodeTST.SelectSingleNode("fareReference/uniqueReference").InnerText;
                                    strResponseTST += SendCommandCryptically(ttAA, $"TQT/T{strRequestTST}");
                                }
                            }

                            strResponse = strResponse.Replace("</PNR_Reply>", $"{strResponseTST}{strWarning}{Request}</PNR_Reply>");
                        }
                        else
                        {
                            strResponse = strResponse.Replace("</PNR_Reply>", $"{strWarning}{Request}</PNR_Reply>");
                        }

                        #region RTSVI Call

                        string strRTSVI = HandleCrypticComands(ttAA, "RTSVI");
                        if (strRTSVI.Length > 0)
                        {
                            strRTSVI = $"<RTSVI>{strRTSVI.Replace("&", "&amp;")}</RTSVI>";
                            strResponse = strResponse.Replace("</PNR_Reply>", $"{strRTSVI}</PNR_Reply>");
                        }

                        #endregion

                        #region Process Electronic Manual or Automated Ticket                        

                        if (strResponse.Contains("<segmentName>FHE</segmentName>") || strResponse.Contains("<segmentName>FHA</segmentName>") || strResponse.Contains("<segmentName>FHM</segmentName>"))
                        {
                            string strManualTicket = "<ManualTickets>";
                            var oDocCS = new XmlDocument();
                            oDocCS.LoadXml(strResponse);
                            XmlElement oRootMT = oDocCS.DocumentElement;
                            if (strResponse.Contains("<segmentName>FHE</segmentName>"))
                                strManualTicket += ProcessFHTickets(ttAA, oRootMT.SelectNodes("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FHE']/otherDataFreetext"));

                            if (strResponse.Contains("<segmentName>FHA</segmentName>"))
                                strManualTicket += ProcessFHTickets(ttAA, oRootMT.SelectNodes("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FHA']/otherDataFreetext"));

                            if (strResponse.Contains("<segmentName>FHM</segmentName>"))
                                strManualTicket += ProcessFHTickets(ttAA, oRootMT.SelectNodes("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FHM']/otherDataFreetext"));

                            strManualTicket += "</ManualTickets>";
                            strResponse = strResponse.Replace("</PNR_Reply>", $"{strManualTicket}</PNR_Reply>");
                        }

                        #endregion

                        // Transform PNR Read
                        if (Version != "v04_")
                            Version = "v03_";

                        CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead",
                            $"Final response size for version {Version}", strResponse.Length.ToString(CultureInfo.InvariantCulture), ttProviderSystems.LogUUID);

                        var strToReplace = "</PNR_RetrieveByRecLocReply>";
                        if (!strResponse.Contains(strToReplace))
                            strToReplace = "</PNR_Reply>";
                        strResponse = strResponse.Replace(strToReplace, $"{Request}{strToReplace}");

                        if (!string.IsNullOrEmpty(GetPricingOptionsTST))
                            strResponse = strResponse.Replace(strToReplace, $"{GetPricingOptionsTST}{strToReplace}");

                        if (inSession)
                            strResponse = strResponse.Replace(strToReplace, $"<ConversationID>{ConversationID}</ConversationID>{strToReplace}");

                        //if (strResponse.Length > 5500)
                        //{
                        //    CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response", strResponse.Substring(0, strResponse.Length / 2), ttProviderSystems.LogUUID);
                        //    CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response II", strResponse.Substring(strResponse.Length / 2), ttProviderSystems.LogUUID);
                        //}
                        //else
                        //{
                        CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response", strResponse, ttProviderSystems.LogUUID);
                        //}

                        strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_PNRReadRS.xsl");

                        //This is questionable code...
                        //Why would we reset Session to not in session when we alredy formed our response?
                        //This code will not have any impact. 
                        if (strResponse.Contains("<Errors>"))
                            inSession = false;
                    }
                    else
                    {
                        switch (strMessage)
                        {
                            case "AccessQueue":
                                //ttAA.CloseSession(ConversationID);
                                //ConversationID = null;
                                inSession = false;

                                strResponse = CoreLib.GetNodeInnerText(strResponse, strResponse.Contains("Command_CrypticReply") ? "textStringDetails" : "freeText", false);
                                strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, ttProviderSystems);
                                break;
                            case "ExitQueue":
                                strResponse = CoreLib.GetNodeInnerText(strResponse, strResponse.Contains("Command_CrypticReply") ? "textStringDetails" : "description", false);

                                if (strResponse.Contains("Q/TTL"))
                                {
                                    //ttAA.CloseSession(ConversationID);
                                    //ConversationID = null;
                                    inSession = false;
                                }

                                strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, ttProviderSystems);
                                if (!String.IsNullOrEmpty(ConversationID))
                                {
                                    //ttAA.CloseSession(ConversationID);
                                    //ConversationID = null;
                                    inSession = false;
                                }
                                break;
                            case "ItemOnQueue":
                                //ttAA.CloseSession(ConversationID);
                                //ConversationID = null;
                                inSession = false;
                                strResponse = CoreLib.GetNodeInnerText(strResponse, strResponse.Contains("Command_CrypticReply") ? "textStringDetails" : "description", false);

                                strResponse = strResponse.Contains("IGNORED - OFF QUEUE") || strResponse.Contains("OFF QUEUE")
                                    ? $"<OTA_TravelItineraryRS Version=\"v03\"><Success/><Warnings><Warning Type=\"Queue\">{strResponse}</Warning></Warnings></OTA_TravelItineraryRS>"
                                    : modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, ttProviderSystems);
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    addLog($"<M>{Request}<BL/>", ttProviderSystems);
                    Console.WriteLine(ex.Message);
                    throw;
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = "";
                    }
                }
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, exx.Message, ttProviderSystems, "");
            }
            finally
            {
                GC.Collect();
            }

            return strResponse;
        }

        // ──────────────────────────────────────────────────────────────────────────────
        // Required usings (add to the top of the containing file if not already present)
        // ──────────────────────────────────────────────────────────────────────────────
        // using System.Xml.Linq;
        // using System.Xml.XPath;   // XPathSelectElement / XPathSelectElements extensions
        //
        // Key conversion rules applied throughout this method:
        //   new XmlDocument() + .LoadXml(s)  →  XDocument.Parse(s)
        //   new XmlDocument() + .Load(f)     →  XDocument.Load(f)
        //   doc.DocumentElement              →  doc.Root
        //   node.SelectSingleNode("path")    →  node.XPathSelectElement("path")
        //   node.SelectNodes("path")         →  node.XPathSelectElements("path")
        //   node.InnerText / .InnerXml       →  node.Value   (text-only nodes)
        //   node.Attributes["x"].Value       →  (string)node.Attribute("x")
        //   node.SelectSingleNode("@Attr")   →  node.Attribute("Attr")  (returns XAttribute)
        //   node.SelectSingleNode("A/@B")    →  node.XPathSelectElement("A")?.Attribute("B")
        //   node.HasAttribute("x")           →  node.Attribute("x") != null
        //   xmlNodeList.Count                →  .Count()
        //   xmlNodeList.Item(0)              →  .FirstOrDefault()
        //   foreach (XmlNode n in list)      →  foreach (XElement n in list)
        //   node.OuterXml                    →  node.ToString()
        //
        // NOTE: Helper methods that previously accepted XmlElement (GetPricingOptionsFXX,
        //       GetFareFamilyFXX, GetPricingOptionsFF_FB, GetPricingOptions,
        //       FilterPricePNRWithBookingClassResponseByPax) must have their signatures
        //       updated to accept XElement instead.
        // ──────────────────────────────────────────────────────────────────────────────

        public string PNRReprice()
        {
            string _response;
            //*****************************************************************
            // Transform OTA PNRRead Request into Native Amadeus Request     *
            //*****************************************************************
            try
            {
                string strResponseReprice = "";
                bool bStoreFare = true;
                _tracerID = string.Empty;
                string strRequest = SetRequest($"AmadeusWS_PNRRepriceRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // Parse the OTA request
                var otaDoc = XDocument.Parse(Request);
                XElement oRootReq = otaDoc.Root;

                bool bMarkup = oRootReq
                  .XPathSelectElements("StoredFare/Markup")
                  .Any(x => x.Attribute("Amount") != null);

                bool bPrivate = oRootReq
                  .XPathSelectElements("StoredFare[@FareType='Private']")
                  .Any();

                bool bPublished = oRootReq
                  .XPathSelectElements("StoredFare[@FareType='Published']")
                  .Any();

                bool bExchange = oRootReq
                    .XPathSelectElements("StoredFare[@FareQualifier='EXC' or @FareQualifier='EX' or @FareQualifier='EXL']")
                    .Any();

                if (oRootReq?.Attribute("StoreFare") != null
                    && (string)oRootReq.Attribute("StoreFare") == "false")
                    bStoreFare = false;

                // *******************
                // Create Session    *
                // *******************
                AmadeusWSAdapter ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //*******************************************************************************
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                //*******************************************************************************
                string strPNRReplay = inSession
                    ? SendRetrievePNR(ttAA)
                    : SendRetrivePNRbyRL(ttAA, strRequest);

                #region business logic
                if (ttProviderSystems.BLFile != "" && !strPNRReplay.Contains("generalErrorInfo"))
                {
                    // Was: new XmlDocument(); oDocBL.Load(...)
                    var oDocBL = XDocument.Load(ttProviderSystems.BLFile);
                    XElement oRootBL = oDocBL.Root;

                    // Was: oRootBL.SelectSingleNode("Security/ProviderBL[@Name=\'Amadeus\'][@System=\'...\']")
                    XElement oNodeBL = oRootBL.XPathSelectElement(
                        $"Security/ProviderBL[@Name='Amadeus'][@System='{ttProviderSystems.System}']");

                    if (oNodeBL != null)
                    {
                        // Was: new XmlDocument(); oDocRespBL.LoadXml(strPNRReplay)
                        var oDocRespBL = XDocument.Parse(strPNRReplay);
                        XElement oRootRespBL = oDocRespBL.Root;

                        XElement oNodeRespBL = oRootRespBL.XPathSelectElement(
                            "dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']");

                        if (oNodeRespBL != null)
                        {
                            // Was: !(oNodeBL.SelectSingleNode($"PCC[@Code=\'{...\']") == null)
                            if (oNodeBL.XPathSelectElement($"PCC[@Code='{ttProviderSystems.PCC}']") != null)
                            {
                                // Was: .SelectSingleNode("accounting/account/number").InnerXml
                                string strAIAN = oNodeRespBL
                                    .XPathSelectElement("accounting/account/number")?.Value;

                                // Was: .SelectSingleNode($"PCC[@Code=...]/@AuthorizeCode").InnerXml
                                string strAIN_BL = (string)oNodeBL
                                    .XPathSelectElement($"PCC[@Code='{ttProviderSystems.PCC}']")
                                    ?.Attribute("AuthorizeCode");

                                if (strAIAN != strAIN_BL)
                                    throw new Exception("Secured PNR");
                            }
                        }
                    }
                }
                #endregion

                string strResponseTST = string.Empty;
                if (strPNRReplay.Contains("<longFreetext>--- TST ")
                    && strPNRReplay.Contains("originDestinationDetails"))
                {
                    strResponseTST = SendDisplayTST(ttAA);

                    #region GetPricingOptions
                    var GetPricingOptionsTST = string.Empty;
                    if (strResponseTST.Contains("<referenceType>TST</referenceType>"))
                    {
                        var oDocTST = XDocument.Parse(strResponseTST);
                        XElement oRootTST = oDocTST.Root;

                        // Was: oRootTST?.SelectNodes("fareList")
                        var xmlNodeList = oRootTST?.XPathSelectElements("fareList");
                        if (xmlNodeList != null)
                        {
                            // Was: foreach (XmlNode oNodeTST in xmlNodeList)
                            foreach (XElement oNodeTST in xmlNodeList)
                            {
                                // Was: .SelectSingleNode("fareReference/uniqueReference").InnerText
                                string tstNum = oNodeTST
                                    .XPathSelectElement("fareReference/uniqueReference")?.Value;

                                var request =
                                    $"<Ticket_GetPricingOptions xmlns=\"http://xml.amadeus.com/TPORRQ_14_1_1A\">" +
                                    $"<documentSelection><referenceType>TST</referenceType>" +
                                    $"<uniqueReference>{tstNum}</uniqueReference></documentSelection>" +
                                    $"</Ticket_GetPricingOptions>";

                                GetPricingOptionsTST += SendGetPricingOptions(ttAA, request);
                            }
                        }
                    }
                    var isCorporateFare = IsCorporateFare(GetPricingOptionsTST, out var pricingOptionsGroup, out var corporateId);
                    #endregion

                    // Was: XmlDocument oDocStored; XmlElement oRootStored;
                    XElement oRootStored = null;

                    if (!strResponseTST.Contains("NO TST RECORD EXISTS"))
                    {
                        var paxAssoc = new Dictionary<Tuple<string, int>, Tuple<int, decimal, decimal, decimal>>();
                        string strFareType;

                        if (Request.Contains("StoreHistoricalFare") && bStoreFare)
                        {
                            #region Store Fare
                            var oTSTDoc = XDocument.Parse(strResponseTST);

                            var oTSTnodes = oTSTDoc.Root?.XPathSelectElements("fareList");

                            if (oTSTnodes != null)
                            {
                                // Was: foreach (XmlNode tst in oTSTnodes)
                                foreach (XElement tst in oTSTnodes)
                                {
                                    var paxRPH = int.Parse(tst.XPathSelectElement("paxSegReference/refDetails/refNumber")?.Value);
                                    var paxTST = int.Parse(tst.XPathSelectElement("fareReference/uniqueReference")?.Value);
                                    //var paxBase = decimal.Parse(tst.XPathSelectElement("fareDataInformation/fareDataSupInformation[fareDataQualifier='B' or fareDataQualifier='700']/fareAmount")?.Value ?? "0.0");
                                    var paxBase = decimal.Parse(tst.XPathSelectElement("fareDataInformation/fareDataSupInformation[(fareDataQualifier='E' and fareCurrency='USD') or (fareDataQualifier='B' and fareCurrency='USD')]/fareAmount")?.Value ?? "0.0");
                                    var paxTotal = decimal.Parse(tst.XPathSelectElement("fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareAmount")?.Value ?? "0.0");
                                    var paxEqv = decimal.Parse(tst.XPathSelectElement("fareDataInformation/fareDataSupInformation[(fareDataQualifier='E' and fareCurrency!='USD') or (fareDataQualifier='B' and fareCurrency!='USD')]/fareAmount")?.Value ?? "0.0");

                                    var pn = oRootReq.XPathSelectElement($"StoredFare[@RPH='{paxTST}']");

                                    paxAssoc.Add(
                                        new Tuple<string, int>(
                                            (string)pn?.XPathSelectElement("PassengerType")?.Attribute("Code"),
                                            paxTST),
                                        new Tuple<int, decimal, decimal, decimal>(paxRPH, paxBase, paxTotal, paxEqv));
                                }
                            }

                            oRootStored = oTSTDoc.Root;

                            if (oRootStored != null)
                            {
                                strFareType = "";
                                if (isCorporateFare)
                                {
                                    strFareType = $"/R,U*{corporateId}";
                                }
                                else
                                {
                                    if (oRootReq.XPathSelectElement("StoredFare")?.Attribute("FareType") != null)
                                    {
                                        strFareType = bPrivate
                                                ? "/R,U"
                                                : "/R";
                                    }
                                }

                                List<Tuple<string, string, string>> ffList;
                                if (!Request.Contains("BrandedFares") || Request.Replace(" ", "").Contains("<BrandedFares/>"))
                                    ffList = GetPricingOptionsFXX(Request, GetPricingOptionsTST, strPNRReplay, oRootStored);
                                else
                                    ffList = GetFareFamilyFXX(Request, strPNRReplay, oRootStored);

                                var saveResp = string.Empty;
                                var retry_count = 0;
                                string strHistFareRS;
                                string strEndTransaction;
                                var omitOptions = false;

                                if (ffList.TrueForAll(x => string.IsNullOrEmpty(x.Item2))
                                    && ffList.TrueForAll(r =>
                                        Regex.Replace(r.Item2, @"\/(P\d+(,\d+)*|PAX|PI|INF)", "")
                                            .Equals(Regex.Replace(ffList.First().Item2, @"\/(P\d+(,\d+)*|PAX|PI|INF)", ""))))
                                {
                                    ffList.Clear();
                                    ffList.Add(new Tuple<string, string, string>("", " ", ""));
                                }

                                if (ffList.TrueForAll(x => !string.IsNullOrEmpty(x.Item2))
                                    && ffList.Any(x => Regex.IsMatch(x.Item2, @"FF(\d)*-"))
                                    && ffList.TrueForAll(r =>
                                        Regex.Replace(r.Item2, @"\/(P\d+(,\d+)*|PAX|PI|INF)", "")
                                            .Equals(Regex.Replace(ffList.First().Item2, @"\/(P\d+(,\d+)*|PAX|PI|INF)", ""))))
                                {
                                    if (ffList.TrueForAll(x =>
                                        ffList.TrueForAll(r =>
                                            Regex.Replace(r.Item2, @"\/(P\d+(,\d+)*|PAX|PI|INF)", "")
                                                .Equals(Regex.Replace(ffList.First().Item2, @"\/(P\d+(,\d+)*|PAX|PI|INF)", "")))))
                                    {
                                        var ffp = ffList.First();
                                        var tsts = string.Join(",", ffList.Select(x => x.Item1));
                                        ffList.Clear();
                                        ffList.Add(new Tuple<string, string, string>(
                                            tsts,
                                            Regex.Replace(ffp.Item2, @"\/(P\d+(,\d+)*|PAX|PI|INF)", ""),
                                            Regex.Replace(ffp.Item3, @"\/(P\d+(,\d+)*|PAX|PI|INF)", "")));
                                    }
                                    else if (ffList.TrueForAll(x =>
                                        ffList.TrueForAll(r =>
                                            Regex.Replace(r.Item2, @"\/(P\d+(,\d+)*|PAX|PI|INF)|\/ZO-0\*[A-Z0-9.,]*", "")
                                                .Equals(Regex.Replace(ffList.First().Item2, @"\/(P\d+(,\d+)*|PAX|PI|INF)|\/ZO-0\*[A-Z0-9.,]*", "")))))
                                    {
                                    }
                                }

                                foreach (var ff in ffList.FindAll(x => !string.IsNullOrEmpty(x.Item2)).OrderBy(x => x.Item2))
                                {
                                    retry_count = 3;
                                    do
                                    {
                                        var fxOpt = Regex.Replace(ff.Item2, @"\/ZO-0\*[A-Z0-9.,]*", "").Trim();
                                        strHistFareRS = SendCommandCryptically(ttAA, $"FXX{strFareType}{fxOpt}");

                                        var fxxRespX = XDocument.Parse(strHistFareRS);
                                        var fxxParsed = new FareQuoteParser().Parse(fxxRespX.XPathSelectElement("Command_CrypticReply/longTextString/textStringDetails")?.Value);

                                        if (fxxParsed.Format == FareQuoteFormat.Detail)
                                        {
                                            if (!(Math.Abs(paxAssoc.First().Value.Item2 - fxxParsed.Detail.BaseFare.Amount) <= 1 ||
                                                Math.Abs(paxAssoc.First().Value.Item4 - fxxParsed.Detail.BaseFare.Amount) <= 1))
                                            {
                                                throw new Exception("THE FARE HAS CHANGED. PLEASE RESTORE THE FARE MANUALLY AND SEND BACK TO ISSUE.");
                                            }
                                        }
                                        else if (fxxParsed.Format == FareQuoteFormat.Summary)
                                        {
                                            foreach (var pax in paxAssoc)
                                            {
                                                if (fxxParsed.Summary.Passengers.Any(p => p.SequenceNumber == pax.Key.Item2) &&
                                                    Math.Abs(pax.Value.Item2 - fxxParsed.Summary.Passengers.First(p => p.SequenceNumber == pax.Key.Item2).BaseFare) > 1)
                                                {
                                                    throw new Exception("THE FARE HAS CHANGED. PLEASE RESTORE THE FARE MANUALLY AND SEND BACK TO ISSUE.");
                                                }
                                            }
                                        }

                                        // https://github.com/Downtown-Travel-TT/TicketingRobot/issues/341
                                        if (strHistFareRS.Contains("NO FARE FOR BOOKING CODE-TRY OTHER PRICING OPTIONS"))
                                        {
                                            // strHistFareRS = SendCommandCryptically(ttAA, $"FXA{strFareType}{fxOpt}");
                                        }

                                        if (!string.IsNullOrEmpty(ff.Item1)
                                            && ffList.FindAll(x => !string.IsNullOrEmpty(x.Item2)).Count == ffList.Count)
                                        {
                                            var fxxResp = strHistFareRS.Split(new[] { '\r', '\n' }).ToList();
                                            var sIdx = fxxResp.FindIndex(x => x.StartsWith("LAST")); // one pax
                                            if (sIdx > 0)
                                            {
                                                fxxResp = fxxResp.GetRange(sIdx, fxxResp.Count - sIdx);
                                                sIdx = fxxResp.FindIndex(x => string.IsNullOrEmpty(x));
                                                fxxResp = fxxResp.GetRange(sIdx, fxxResp.Count - sIdx);
                                                sIdx = fxxResp.FindIndex(x => x.Trim().StartsWith("USD"));
                                                var sAmount = fxxResp[sIdx]
                                                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];

                                                // Was: .SelectSingleNode("fareList[...]/fareAmount") == null ? "0.00" : .InnerText
                                                var tAmount = oRootStored.XPathSelectElement(
                                                    $"fareList[fareReference/uniqueReference = '{ff.Item1}']/fareDataInformation" +
                                                    $"/fareDataSupInformation[(fareDataQualifier='E' and fareCurrency='USD') or " +
                                                    $"(fareDataQualifier='B' and fareCurrency='USD')]/fareAmount")?.Value ?? "0.00";

                                                if (sAmount != tAmount)
                                                    throw new Exception("ADULTS AND KIDS - DIFFERENT FARE FAMILIES. ISSUE MANUALLY");
                                            }
                                            else if (fxxResp.FindIndex(x => x.Trim().StartsWith("PASSENGER")) > 0) // several pax
                                            {
                                                sIdx = fxxResp.FindIndex(x => x.Trim().StartsWith("PASSENGER"));
                                                fxxResp = fxxResp.GetRange(sIdx + 1, fxxResp.Count - (sIdx + 1));
                                                sIdx = fxxResp.FindIndex(string.IsNullOrEmpty);
                                                fxxResp = fxxResp.GetRange(0, sIdx);

                                                var bfList = fxxResp
                                                    .Select(s => s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                                  .Reverse().Skip(2).First())
                                                    .Select(x => new { FXXAmount = x, IsPresent = false })
                                                    .ToList();

                                                foreach (var iTst in ff.Item1.Split(','))
                                                {
                                                    var tAmount = oRootStored.XPathSelectElement(
                                                        $"fareList[fareReference/uniqueReference = '{iTst}']/fareDataInformation" +
                                                        $"/fareDataSupInformation[(fareDataQualifier='E' and fareCurrency='USD') or " +
                                                        $"(fareDataQualifier='B' and fareCurrency='USD')]/fareAmount")?.Value ?? "0.00";

                                                    if (!bfList.Select(x => x.FXXAmount).Contains(tAmount))
                                                        throw new Exception("ADULTS AND KIDS - DIFFERENT FARE FAMILIES. ISSUE MANUALLY");
                                                    else
                                                        bfList = bfList
                                                            .Select(x => new { x.FXXAmount, IsPresent = x.IsPresent || x.FXXAmount.Equals(tAmount) })
                                                            .ToList();
                                                }

                                                if (bfList.Any(x => !x.IsPresent))
                                                    throw new Exception("ADULTS AND KIDS - DIFFERENT FARE FAMILIES. ISSUE MANUALLY");
                                            }
                                        }

                                        strHistFareRS = SendCommandCryptically(ttAA, "FR");
                                        strEndTransaction = "<PNR_AddMultiElements><pnrActions><optionCode>11</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";
                                        saveResp = SendAddMultiElements(ttAA, strEndTransaction);
                                        if (saveResp.Contains("MISSING SSR CTCM MOBILE OR SSR CTCE EMAIL OR SSR CTCR NON-CONSENT"))
                                            saveResp = SendAddMultiElements(ttAA, strEndTransaction);
                                        if (!saveResp.Contains("<freeText>SIMULTANEOUS CHANGES TO PNR - USE WRA/RT TO PRINT OR IGNORE"))
                                            break;
                                    } while (retry_count-- > 0);
                                }

                                var tktDesTooLong = false;
                                retry_count = 3;
                                do
                                {
                                    var isTktDesInTst = false;

                                    var tsts = oRootReq.XPathSelectElements("//StoredFare").Select(x => (string)x.Attribute("RPH"));

                                    foreach (var item in tsts)
                                    {
                                        var tkdes = oRootStored.XPathSelectElements(
                                            $"fareList[fareReference/uniqueReference = '{item}']/segmentInformation/fareQualifier/fareBasisDetails/ticketDesignator");
                                        if (tkdes.Any())
                                        {
                                            isTktDesInTst = true;
                                            break;
                                        }
                                    }

                                    if (!bPrivate && isTktDesInTst && ffList.All(f => !string.IsNullOrEmpty(f.Item3)))
                                    {
                                        foreach (var ff in ffList)
                                        {
                                            strHistFareRS = SendCommandCryptically(ttAA, $"FXP{strFareType}{ff.Item3}");
                                            if (strHistFareRS.Contains("TICKET DESIGNATOR TOO LONG TO PROCESS"))
                                            {
                                                var fxOpt = Regex.Replace(ff.Item3, @"\/ZO-0\*[A-Z0-9.,]*", "");
                                                strHistFareRS = SendCommandCryptically(ttAA, $"FXP{strFareType}{fxOpt}");
                                                tktDesTooLong = true;
                                            }

                                            var fxxRespX = XDocument.Parse(strHistFareRS);
                                            var fxxParsed = new FareQuoteParser().Parse(fxxRespX.XPathSelectElement("Command_CrypticReply/longTextString/textStringDetails")?.Value);

                                            if (fxxParsed.Format == FareQuoteFormat.Detail)
                                            {
                                                //if (Math.Abs(paxAssoc.First().Value.Item2 - fxxParsed.Detail.BaseFare.Amount) > 1)
                                                if (!(Math.Abs(paxAssoc.First().Value.Item2 - fxxParsed.Detail.BaseFare.Amount) <= 1 ||
                                                      Math.Abs(paxAssoc.First().Value.Item4 - fxxParsed.Detail.BaseFare.Amount) <= 1))
                                                {
                                                    throw new Exception("THE FARE HAS CHANGED. PLEASE RESTORE THE FARE MANUALLY AND SEND BACK TO ISSUE.");
                                                }
                                            }
                                            else if (fxxParsed.Format == FareQuoteFormat.Summary)
                                            {
                                                foreach (var pax in paxAssoc)
                                                {
                                                    //if (fxxParsed.Summary.Passengers.Any(p => p.SequenceNumber == pax.Value.Item1) && Math.Abs(pax.Value.Item2 - fxxParsed.Summary.Passengers.First(p => p.SequenceNumber == pax.Value.Item1).BaseFare) > 1)
                                                    if (fxxParsed.Summary.Passengers.Any(p => p.SequenceNumber == pax.Key.Item2) &&
                                                        Math.Abs(pax.Value.Item2 - fxxParsed.Summary.Passengers.First(p => p.SequenceNumber == pax.Key.Item2).BaseFare) > 1)
                                                    {
                                                        throw new Exception("THE FARE HAS CHANGED. PLEASE RESTORE THE FARE MANUALLY AND SEND BACK TO ISSUE.");
                                                    }
                                                }
                                            }

                                        }
                                    }
                                    else
                                    {
                                        foreach (var ff in ffList.FindAll(x => !string.IsNullOrEmpty(x.Item2)))
                                        {
                                            strHistFareRS = SendCommandCryptically(ttAA, $"FXP{strFareType}{ff.Item2}");
                                            if (strHistFareRS.Contains("TICKET DESIGNATOR TOO LONG TO PROCESS"))
                                            {
                                                var fxOpt = Regex.Replace(ff.Item2, @"\/ZO-0\*[A-Z0-9.,]*", "");
                                                strHistFareRS = SendCommandCryptically(ttAA, $"FXP{strFareType}{fxOpt}");
                                                tktDesTooLong = true;
                                            }

                                            var fxxRespX = XDocument.Parse(strHistFareRS);
                                            var fxxParsed = new FareQuoteParser().Parse(fxxRespX.XPathSelectElement("Command_CrypticReply/longTextString/textStringDetails")?.Value);

                                            if (fxxParsed.Format == FareQuoteFormat.Detail)
                                            {
                                                if (!(Math.Abs(paxAssoc.First().Value.Item2 - fxxParsed.Detail.BaseFare.Amount) <= 1 ||
                                                      Math.Abs(paxAssoc.First().Value.Item4 - fxxParsed.Detail.BaseFare.Amount) <= 1))
                                                {
                                                    throw new Exception("THE FARE HAS CHANGED. PLEASE RESTORE THE FARE MANUALLY AND SEND BACK TO ISSUE.");
                                                }
                                            }
                                            else if (fxxParsed.Format == FareQuoteFormat.Summary)
                                            {
                                                foreach (var pax in paxAssoc)
                                                {
                                                    //if (fxxParsed.Summary.Passengers.Any(p => p.SequenceNumber == pax.Value.Item1) && Math.Abs(pax.Value.Item2 - fxxParsed.Summary.Passengers.First(p => p.SequenceNumber == pax.Value.Item1).BaseFare) > 1)
                                                    if (fxxParsed.Summary.Passengers.Any(p => p.SequenceNumber == pax.Key.Item2) &&
                                                        Math.Abs(pax.Value.Item2 - fxxParsed.Summary.Passengers.First(p => p.SequenceNumber == pax.Key.Item2).BaseFare) > 1)
                                                    {
                                                        throw new Exception("THE FARE HAS CHANGED. PLEASE RESTORE THE FARE MANUALLY AND SEND BACK TO ISSUE.");
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //TODO: ADD PRICE CHECK
                                    strEndTransaction = "<PNR_AddMultiElements><pnrActions><optionCode>11</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";
                                    saveResp = SendAddMultiElements(ttAA, strEndTransaction);
                                    if (saveResp.Contains("MISSING SSR CTCM MOBILE OR SSR CTCE EMAIL OR SSR CTCR NON-CONSENT"))
                                        saveResp = SendAddMultiElements(ttAA, strEndTransaction);
                                    if (!saveResp.Contains("<freeText>SIMULTANEOUS CHANGES TO PNR - USE WRA/RT TO PRINT OR IGNORE"))
                                        break;
                                } while (retry_count-- > 0);

                                if (tktDesTooLong)
                                {
                                    strResponseTST = SendDisplayTST(ttAA);

                                    var nTSTDoc = XDocument.Parse(strResponseTST);
                                    var nRootStored = nTSTDoc.Root;

                                    var pnr = XDocument.Parse(strPNRReplay);

                                    var flightSegs = pnr.Root.XPathSelectElements(
                                        "//originDestinationDetails/itineraryInfo");

                                    var flSegMap = new Dictionary<string, string>();

                                    foreach (XElement fnode in flightSegs)
                                    {
                                        flSegMap.Add(
                                            fnode.XPathSelectElement("elementManagementItinerary/reference/number")?.Value,
                                            fnode.XPathSelectElement("elementManagementItinerary/lineNumber")?.Value);
                                    }

                                    retry_count = 3;
                                    do
                                    {
                                        foreach (XElement reqNode in oRootReq.XPathSelectElements("//StoredFare"))
                                        {
                                            var tstRPH = (string)reqNode.Attribute("RPH");

                                            var paxType = oRootStored?.XPathSelectElement(
                                                $"fareList[fareReference/uniqueReference = '{tstRPH}']/paxSegReference/refDetails/refQualifier")?.Value;
                                            var paxNum = oRootStored?.XPathSelectElement(
                                                $"fareList[fareReference/uniqueReference = '{tstRPH}']/paxSegReference/refDetails/refNumber")?.Value;
                                            var nTstRPH = nRootStored?.XPathSelectElement(
                                                $"fareList[paxSegReference/refDetails/refNumber = '{paxNum}' and " +
                                                $"paxSegReference/refDetails/refQualifier = '{paxType}']/fareReference/uniqueReference")?.Value;

                                            // Was: nRootStored?.SelectNodes($"fareList[...]/segmentInformation")
                                            var oSegmentNodes = nRootStored?
                                                .XPathSelectElements($"fareList[fareReference/uniqueReference = '{nTstRPH}']/segmentInformation")
                                                .ToList();

                                            var tktNode = oSegmentNodes?[0].XPathSelectElement("fareQualifier/fareBasisDetails/ticketDesignator");

                                            // Was: reqNode.SelectSingleNode("FareSegments/AirSegments").Attributes["TicketDesignator"] != null
                                            //       ? reqNode.SelectSingleNode(...).Attributes["TicketDesignator"].Value : string.Empty
                                            var ticketDes =
                                                (string)reqNode.XPathSelectElement("FareSegments/AirSegments")?.Attribute("TicketDesignator")
                                                ?? string.Empty;

                                            var farebasis =
                                                (oSegmentNodes?[0].XPathSelectElement("fareQualifier/fareBasisDetails/primaryCode")?.Value ?? "") +
                                                (oSegmentNodes?[0].XPathSelectElement("fareQualifier/fareBasisDetails/fareBasisCode")?.Value ?? "");

                                            var fbSegList = string.Empty;
                                            string updRequest;
                                            string strUpdTicketDes;

                                            foreach (XElement segNode in oSegmentNodes ?? Enumerable.Empty<XElement>())
                                            {
                                                if (segNode.XPathSelectElement("connexInformation/connecDetails/routingInformation") != null &&
                                                    segNode.XPathSelectElement("connexInformation/connecDetails/routingInformation")
                                                           .Value.Equals("ARNK"))
                                                    continue;

                                                var segNum = segNode
                                                    .XPathSelectElement("segmentReference/refDetails[refQualifier='S']/refNumber")?.Value;

                                                var segTD = string.Empty;

                                                if (reqNode.XPathSelectElement(
                                                        $"FareSegments/AirSegments[@RPH='{flSegMap[segNum]}']")
                                                    ?.Attribute("TicketDesignator") != null)
                                                {
                                                    segTD = (string)reqNode
                                                        .XPathSelectElement($"FareSegments/AirSegments[@RPH='{flSegMap[segNum]}']")
                                                        .Attribute("TicketDesignator");

                                                    var nSegTD = segNode
                                                        .XPathSelectElement("fareQualifier/fareBasisDetails/ticketDesignator")?.Value;
                                                    if (nSegTD != null && segTD.Equals(nSegTD))
                                                        continue;
                                                }
                                                else
                                                    continue;

                                                if (segNode.XPathSelectElement("fareQualifier") == null)
                                                    continue;

                                                var segFB =
                                                    (segNode.XPathSelectElement("fareQualifier/fareBasisDetails/primaryCode")?.Value ?? "") +
                                                    (segNode.XPathSelectElement("fareQualifier/fareBasisDetails/fareBasisCode")?.Value ?? "");

                                                if (farebasis.Equals(segFB) && ticketDes.Equals(segTD))
                                                {
                                                    fbSegList += $",{segNode.XPathSelectElement("sequenceInformation/sequenceSection/sequenceNumber")?.Value}";
                                                }
                                                else
                                                {
                                                    updRequest =
                                                        $"TTI/T{nTstRPH}/L{fbSegList.TrimStart(',')}" +
                                                        $"{(fbSegList.TrimStart(',').Contains(",") ? "x" : string.Empty)}" +
                                                        $"/B{farebasis} {ticketDes}";
                                                    strUpdTicketDes = SendCommandCryptically(ttAA, updRequest);
                                                    farebasis = segFB;
                                                    ticketDes = segTD;
                                                    fbSegList = $",{segNode.XPathSelectElement("sequenceInformation/sequenceSection/sequenceNumber")?.Value}";
                                                }
                                            }

                                            if (!string.IsNullOrEmpty(fbSegList))
                                            {
                                                updRequest =
                                                    $"TTI/T{nTstRPH}/L{fbSegList.TrimStart(',')}" +
                                                    $"{(fbSegList.TrimStart(',').Contains(",") ? "x" : string.Empty)}" +
                                                    $"/B{farebasis} {ticketDes}";
                                                strUpdTicketDes = SendCommandCryptically(ttAA, updRequest);
                                            }
                                        }

                                        strEndTransaction = "<PNR_AddMultiElements><pnrActions><optionCode>11</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";
                                        saveResp = SendAddMultiElements(ttAA, strEndTransaction);
                                        if (saveResp.Contains("MISSING SSR CTCM MOBILE OR SSR CTCE EMAIL OR SSR CTCR NON-CONSENT"))
                                            saveResp = SendAddMultiElements(ttAA, strEndTransaction);
                                        if (!saveResp.Contains("<freeText>SIMULTANEOUS CHANGES TO PNR - USE WRA/RT TO PRINT OR IGNORE"))
                                            break;
                                    } while (retry_count-- > 0);
                                }

                                strResponseTST = SendDisplayTST(ttAA);
                                strResponseReprice = $"<strResponseReprice>{strResponseTST}</strResponseReprice>";
                            }
                            #endregion
                        }
                        else if (Request.Contains("StoreHistoricalFare") && !bStoreFare)
                        {
                            var oTSTDoc = XDocument.Parse(strResponseTST);
                            var oTSTnodes = oTSTDoc.Root?.XPathSelectElements("fareList");

                            if (oTSTnodes != null)
                            {
                                foreach (XElement tst in oTSTnodes)
                                {
                                    var paxRPH = int.Parse(tst.XPathSelectElement("paxSegReference/refDetails/refNumber")?.Value);
                                    var paxTST = int.Parse(tst.XPathSelectElement("fareReference/uniqueReference")?.Value);
                                    var paxBase = decimal.Parse(tst.XPathSelectElement("fareDataInformation/fareDataSupInformation[(fareDataQualifier='E' and fareCurrency='USD') or (fareDataQualifier='B' and fareCurrency='USD')]/fareAmount")?.Value ?? "0.0");
                                    var paxTotal = decimal.Parse(tst.XPathSelectElement("fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareAmount")?.Value ?? "0.0");
                                    var paxEqv = decimal.Parse(tst.XPathSelectElement("fareDataInformation/fareDataSupInformation[(fareDataQualifier='E' and fareCurrency!='USD') or (fareDataQualifier='B' and fareCurrency!='USD')]/fareAmount")?.Value ?? "0.0");

                                    var pn = oRootReq.XPathSelectElement($"StoredFare[@RPH='{paxTST}']");
                                    paxAssoc.Add(
                                        new Tuple<string, int>(
                                            (string)pn?.XPathSelectElement("PassengerType")?.Attribute("Code"),
                                            paxTST),
                                        new Tuple<int, decimal, decimal, decimal>(paxRPH, paxBase, paxTotal, paxEqv));
                                }
                            }

                            oRootStored = oTSTDoc.Root;

                            //TODO: fix this
                            var oNode1 = oRootReq.XPathSelectElements("StoredFare").FirstOrDefault();

                            if (oRootStored != null)
                            {
                                // NOTE: update GetPricingOptionsFF_FB / GetPricingOptions signatures to accept XElement
                                var sXmlFareFamily = GetPricingOptionsFF_FB(Request, strPNRReplay, oRootStored);
                                if (sXmlFareFamily.All(x => string.IsNullOrEmpty(x)))
                                    sXmlFareFamily = GetPricingOptions(Request, GetPricingOptionsTST, strPNRReplay, oRootStored);

                                string strDiscount = "";
                                string strTktDes = "";
                                strFareType = "RP";
                                string discQualif = "707"; // amount discount
                                string strZap = string.Empty;

                                // Was: oNode1.SelectSingleNode("Discount/@Amount") != null
                                if (oNode1?.XPathSelectElement("Discount")?.Attribute("Amount") != null)
                                {
                                    // Was: oNode1.SelectSingleNode("Discount/@Amount").InnerXml
                                    strDiscount = (string)oNode1.XPathSelectElement("Discount").Attribute("Amount");
                                }
                                else if (oNode1?.XPathSelectElement("Discount")?.Attribute("Percent") != null)
                                {
                                    strDiscount = (string)oNode1.XPathSelectElement("Discount").Attribute("Percent");
                                    if (strDiscount.Contains("."))
                                        strDiscount = strDiscount.Substring(0, strDiscount.IndexOf("."));
                                    discQualif = "708"; // percent discount
                                }

                                // Was: oNode1.Attributes["FareType"] != null
                                if (oNode1?.Attribute("FareType") != null)
                                {
                                    // Was: oNode1.Attributes["FareType"].Value == "Private"
                                    if ((string)oNode1.Attribute("FareType") == "Private")
                                        strFareType = "RU";
                                }

                                if (strDiscount != "" || strTktDes != "")
                                {
                                    strZap =
                                        $"<pricingOptionGroup><pricingOptionKey><pricingOptionKey>ZAP</pricingOptionKey></pricingOptionKey>" +
                                        $"<penDisInformation><discountPenaltyQualifier>ZAP</discountPenaltyQualifier>" +
                                        $"<discountPenaltyDetails><function>700</function><amountType>{discQualif}</amountType>" +
                                        $"<amount>{strDiscount}</amount>{strTktDes}</discountPenaltyDetails></penDisInformation></pricingOptionGroup>";
                                }

                                var strPriceRq = string.Empty;
                                var excludeFFopts = false;

                                foreach (var xmlFareFamily in sXmlFareFamily)
                                {
                                    string strRepriceRQ = string.Empty;
                                    var respReprice = string.Empty;

                                    if (isCorporateFare)
                                    {
                                        strRepriceRQ =
                                            $"<Fare_PricePNRWithBookingClass>" +
                                            $"{(excludeFFopts ? "" : xmlFareFamily)}" +
                                            $"<pricingOptionGroup><pricingOptionKey><pricingOptionKey>RLO</pricingOptionKey></pricingOptionKey></pricingOptionGroup>" +
                                            $"{pricingOptionsGroup}{strZap}" +
                                            $"</Fare_PricePNRWithBookingClass>";
                                    }
                                    else
                                    {
                                        strRepriceRQ =
                                            $"<Fare_PricePNRWithBookingClass>{(excludeFFopts ? "" : xmlFareFamily)}" +
                                            $"<pricingOptionGroup><pricingOptionKey><pricingOptionKey>RLO</pricingOptionKey></pricingOptionKey></pricingOptionGroup>" +
                                            $"{(xmlFareFamily.Contains($">{strFareType}<") ? "" : $"<pricingOptionGroup><pricingOptionKey><pricingOptionKey>{strFareType}</pricingOptionKey></pricingOptionKey></pricingOptionGroup>")}" +
                                            $"{strZap}</Fare_PricePNRWithBookingClass>";
                                    }

                                    if (!strPriceRq.Equals(strRepriceRQ))
                                    {
                                        strPriceRq = strRepriceRQ;
                                        respReprice = SendPricePNRWithBookingClass(ttAA, strRepriceRQ);

                                        if (respReprice != null &&
                                            (respReprice.Contains("NO VALID FARE/RULE COMBINATIONS FOR PRICING") ||
                                             respReprice.Contains("NO FARE FOR BOOKING CODE-TRY OTHER PRICING OPTIONS")))
                                        {
                                            excludeFFopts = true;
                                            strRepriceRQ =
                                                $"<Fare_PricePNRWithBookingClass>{(excludeFFopts ? "" : xmlFareFamily)}" +
                                                $"<pricingOptionGroup><pricingOptionKey><pricingOptionKey>RLO</pricingOptionKey></pricingOptionKey></pricingOptionGroup>" +
                                                $"{(xmlFareFamily.Contains($">{strFareType}<") ? "" : $"<pricingOptionGroup><pricingOptionKey><pricingOptionKey>{strFareType}</pricingOptionKey></pricingOptionKey></pricingOptionGroup>")}" +
                                                $"{strZap}</Fare_PricePNRWithBookingClass>";
                                            strPriceRq = strRepriceRQ;
                                            respReprice = SendPricePNRWithBookingClass(ttAA, strRepriceRQ);
                                        }

                                        // NOTE: update FilterPricePNRWithBookingClassResponseByPax if it uses XmlElement internally
                                        strResponseReprice += FilterPricePNRWithBookingClassResponseByPax(respReprice, xmlFareFamily, excludeFFopts);
                                    }
                                }

                                strResponseReprice = strResponseReprice.Replace(
                                    @"</Fare_PricePNRWithBookingClassReply><Fare_PricePNRWithBookingClassReply>", "");
                                strResponseReprice = strResponseReprice.Replace(
                                    $" xmlns=\"http://xml.amadeus.com/{ttProviderSystems.AmadeusWSSchema[Fare_PricePNRWithLowerFaresReply]}\"", "");
                                strResponseReprice = strResponseReprice.Replace(
                                    $" xmlns=\"http://xml.amadeus.com/{ttProviderSystems.AmadeusWSSchema[Fare_PricePNRWithBookingClassReply]}\"", "");
                            }
                        }

                        if (bMarkup)
                        {
                            #region MarkUp
                            // Was: oDocStored = new XmlDocument(); oDocStored.LoadXml(strResponseTST); oRootStored = oDocStored.DocumentElement
                            oRootStored = XDocument.Parse(strResponseTST).Root;

                            // Was: foreach (XmlNode oNode in oRootReq.SelectNodes("StoredFare[Markup]"))
                            foreach (XElement oNode in oRootReq.XPathSelectElements("StoredFare[Markup]"))
                            {
                                var retry_count = 3;
                                do
                                {
                                    string updRequest = string.Empty;

                                    // Was: oNode.SelectSingleNode("@RPH").InnerText
                                    string tstRPH = (string)oNode.Attribute("RPH");
                                    int iTSTRPH = Convert.ToInt16(tstRPH);
                                    string strBf = string.Empty;
                                    string strTf = string.Empty;
                                    string fareCurrency = string.Empty;

                                    // Was: oNode.SelectSingleNode("PassengerType/@Code")?.InnerText
                                    var passengerTypeCode = (string)oNode.XPathSelectElement("PassengerType")?.Attribute("Code");

                                    if (paxAssoc.ContainsKey(new Tuple<string, int>(passengerTypeCode, iTSTRPH)))
                                    {
                                        var paxRPH = paxAssoc[new Tuple<string, int>(passengerTypeCode, iTSTRPH)].Item1;

                                        if (oRootStored != null)
                                        {
                                            if (passengerTypeCode == "INF")
                                            {
                                                strBf = oRootStored.XPathSelectElement(
                                                    $"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and " +
                                                    $"statusInformation/firstStatusDetails/tstFlag = 'INF']" +
                                                    $"/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount")?.Value;
                                                strTf = oRootStored.XPathSelectElement(
                                                    $"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and " +
                                                    $"statusInformation/firstStatusDetails/tstFlag = 'INF']" +
                                                    $"/fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount")?.Value;
                                                fareCurrency = oRootStored.XPathSelectElement(
                                                    $"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and " +
                                                    $"statusInformation/firstStatusDetails/tstFlag = 'INF']" +
                                                    $"/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency")?.Value;
                                                tstRPH = oRootStored.XPathSelectElement(
                                                    $"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and " +
                                                    $"statusInformation/firstStatusDetails/tstFlag = 'INF']/fareReference/uniqueReference")?.Value;
                                            }
                                            else
                                            {
                                                strBf = oRootStored.XPathSelectElement(
                                                    $"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and " +
                                                    $"statusInformation/firstStatusDetails/tstFlag != 'INF']" +
                                                    $"/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount")?.Value;
                                                strTf = oRootStored.XPathSelectElement(
                                                    $"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and " +
                                                    $"statusInformation/firstStatusDetails/tstFlag != 'INF']" +
                                                    $"/fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount")?.Value;
                                                fareCurrency = oRootStored.XPathSelectElement(
                                                    $"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and " +
                                                    $"statusInformation/firstStatusDetails/tstFlag != 'INF']" +
                                                    $"/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency")?.Value;
                                                tstRPH = oRootStored.XPathSelectElement(
                                                    $"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and " +
                                                    $"statusInformation/firstStatusDetails/tstFlag != 'INF']/fareReference/uniqueReference")?.Value;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        tstRPH = iTSTRPH.ToString(CultureInfo.InvariantCulture);
                                        if (oRootStored != null)
                                        {
                                            strBf = oRootStored.XPathSelectElement(
                                                $"fareList[fareReference/uniqueReference = '{tstRPH}']/fareDataInformation" +
                                                $"/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount")?.Value;
                                            strTf = oRootStored.XPathSelectElement(
                                                $"fareList[fareReference/uniqueReference = '{tstRPH}']/fareDataInformation" +
                                                $"/fareDataSupInformation[fareDataQualifier = '712']/fareAmount")?.Value;
                                            fareCurrency = oRootStored.XPathSelectElement(
                                                $"fareList[fareReference/uniqueReference = '{tstRPH}']/fareDataInformation" +
                                                $"/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency")?.Value;
                                        }
                                    }

                                    var culture = new CultureInfo("en-US");

                                    // Was: oNode.SelectSingleNode("Markup/@Amount") == null ? 0 : Convert.ToDecimal(oNode.SelectSingleNode("Markup/@Amount").InnerText, culture)
                                    decimal decMarkup = oNode.XPathSelectElement("Markup")?.Attribute("Amount") == null
                                        ? 0
                                        : Convert.ToDecimal((string)oNode.XPathSelectElement("Markup").Attribute("Amount"), culture);

                                    decimal oldBF = Convert.ToDecimal(strBf, culture);
                                    decimal oldTF = Convert.ToDecimal(strTf, culture);
                                    string newBF = Convert.ToString(oldBF + decMarkup);
                                    string newTF = Convert.ToString(oldTF + decMarkup);

                                    if (fareCurrency != "USD")
                                    {
                                        int decPoints;
                                        if (strBf.Contains("."))
                                            decPoints = strBf.Trim().Length - (strBf.IndexOf(".", StringComparison.Ordinal) + 1);
                                        else
                                            decPoints = 0;

                                        var tstNode = oRootStored.XPathSelectElement(
                                            "fareList/otherPricingInfo/attributeDetails[attributeType='FCA']/attributeDescription");
                                        var fxxData = tstNode.Value
                                            .Split(new[] { '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                        var strROE = fxxData.Exists(s => s.StartsWith("ROE"))
                                            ? fxxData.Find(s => s.StartsWith("ROE")).Substring(3)
                                            : "1.0";
                                        decimal roe = decimal.Parse(strROE);
                                        decimal oldBFusd = Convert.ToDecimal(
                                            oRootStored.XPathSelectElement(
                                                $"fareList[fareReference/uniqueReference = '{tstRPH}']/fareDataInformation" +
                                                $"/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount")?.Value,
                                            culture);

                                        // Bug 624 - T-Robot - foreign Currency - no markup added
                                        updRequest = $"TTK/T{tstRPH}/I{fareCurrency}{Math.Round(oldBF + decMarkup * roe, decPoints)}/EUSD{oldBFusd + decMarkup:0.00}";
                                    }
                                    else
                                    {
                                        updRequest = $"TTK/T{tstRPH}/U{newBF}";
                                    }

                                    SendCommandCryptically(ttAA, updRequest);

                                    if (bStoreFare)
                                    {
                                        var saveCMDresp = SendCommandCryptically(ttAA, "RFTRIPXML;ER");
                                        if (saveCMDresp.Contains("textStringDetails") && saveCMDresp.Contains("WARNING"))
                                            saveCMDresp = SendCommandCryptically(ttAA, "ER");
                                        if (!saveCMDresp.Contains("SIMULTANEOUS CHANGES"))
                                            break;
                                    }
                                    else
                                        break;
                                } while (retry_count-- > 0);
                            }

                            if (bStoreFare)
                            {
                                string strEndTransaction =
                                    "<PNR_AddMultiElements><pnrActions><optionCode>11</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";
                                SendAddMultiElements(ttAA, strEndTransaction);
                            }

                            strResponseReprice = $"<strResponseReprice>{SendDisplayTST(ttAA)}</strResponseReprice>";
                            #endregion
                            #region oldCode
                            // [legacy commented-out code unchanged — omitted here for brevity]
                            #endregion
                        }

                        #region Old Code
                        // [legacy commented-out code unchanged — omitted here for brevity]
                        #endregion
                    }
                    else
                    {
                        string strReprice =
                            "<Fare_PricePNRWithBookingClass><overrideInformation>" +
                            "<attributeDetails><attributeType>RLO</attributeType></attributeDetails>" +
                            "<attributeDetails><attributeType>RU</attributeType></attributeDetails>" +
                            "<attributeDetails><attributeType>RP</attributeType></attributeDetails>" +
                            "</overrideInformation></Fare_PricePNRWithBookingClass>";

                        strResponseReprice = SendPricePNRWithBookingClass(ttAA, strReprice);

                        // Was: oDocStored = new XmlDocument(); oDocStored.LoadXml(strResponseReprice); oRootStored = oDocStored.DocumentElement
                        oRootStored = XDocument.Parse(strResponseReprice).Root;

                        _response = CoreLib.TransformXML(strResponseReprice, XslPath, $"{Version}AmadeusWS_TB_Errors.xsl");

                        // Fatal Error
                        if (!_response.Contains("<Error"))
                        {
                            string strStorePrice = "<Ticket_CreateTSTFromPricing>";
                            int iFareList = 1;

                            // Was: foreach (XmlNode oNodeResps in oRootStored.SelectNodes("fareList"))
                            foreach (XElement oNodeResps in oRootStored.XPathSelectElements("fareList"))
                            {
                                strStorePrice +=
                                    $"<psaList><itemReference><referenceType>TST</referenceType>" +
                                    $"<uniqueReference>{iFareList}</uniqueReference></itemReference></psaList>";
                                iFareList++;
                            }
                            strStorePrice += "</Ticket_CreateTSTFromPricing>";

                            _response = SendCreateTSTFromPricing(ttAA, strStorePrice);
                            strResponseReprice = $"<strResponseReprice>{SendDisplayTST(ttAA)}</strResponseReprice>";

                            string strEndTransaction = !inSession
                                ? "<PNR_AddMultiElements><pnrActions><optionCode>10</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>"
                                : "<PNR_AddMultiElements><pnrActions><optionCode>11</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";

                            _response = SendAddMultiElements(ttAA, strEndTransaction);
                        }
                    }
                }

                //*****************************************************************
                // Transform Native Amadeus PNRRead Response into OTA Response   *
                //*****************************************************************
                strPNRReplay = strPNRReplay.Replace("PNR_Reply", "PNR_RetrieveByRecLocReply");
                if (inSession)
                    strPNRReplay = strPNRReplay.Replace("</PNR_RetrieveByRecLocReply>",
                        $"{strResponseTST}{strResponseReprice}{Request}<ConversationID>{ConversationID}</ConversationID></PNR_RetrieveByRecLocReply>");

                _response = GetFinalResponse(ttAA, inSession, ref strPNRReplay, "PNRReprice");
            }
            catch (Exception exx)
            {
                _response = modCore.FormatErrorMessage(modCore.ttServices.PNRReprice, exx.Message, ttProviderSystems);
                _response = _response.Replace("</OTA_PNRRepriceRS>", $"<ConversationID>{ConversationID}</ConversationID></OTA_PNRRepriceRS>");
            }
            return _response;
        }

        private string FilterPricePNRWithBookingClassResponseByPax(string response, string request, bool skipFFopts)
        {
            if (string.IsNullOrEmpty(request))
                return response;
            XmlDocument docRQ = new XmlDocument();
            if (request.LastIndexOf("<pricingOptionGroup>") != 0)
                request = $"<opt>{request}</opt>";
            docRQ.LoadXml(request);
            XmlElement rootRQ = docRQ.DocumentElement;
            XmlDocument docRS = new XmlDocument();
            docRS.LoadXml(response);
            XmlElement rootRS = docRS.DocumentElement;

            var okPax = rootRQ // PAX ref
                .SelectNodes("//paxSegTstReference/referenceDetails[type='PA' or type='PI' or type='P']")
                .Cast<XmlNode>().ToList().Select(x => (value: x.SelectSingleNode("value").InnerText, type: x.SelectSingleNode("type").InnerText)).ToList();

            if (skipFFopts)
                okPax.Clear();

            var okTst = new List<string>();
            if (!skipFFopts && okPax.Count.Equals(0))// TST ref
            {
                okTst = rootRQ
                    .SelectNodes("//paxSegTstReference/referenceDetails[type='T']")
                    .Cast<XmlNode>().ToList().Select(x => x.SelectSingleNode("value").InnerText).ToList();
            }
            var nodeList = docRS.SelectNodes("//fareList");
            if (!okPax.Count.Equals(0))
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (!okPax.Any(x => x.type == nodeList[i].SelectSingleNode("paxSegReference/refDetails/refQualifier")?.InnerText &&
                                        x.value == nodeList[i].SelectSingleNode("paxSegReference/refDetails/refNumber")?.InnerText))
                    {
                        nodeList[i].ParentNode?.RemoveChild(nodeList[i]);
                    }
                }
            }
            if (!okTst.Count.Equals(0))
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    if (!okTst.Any(x => nodeList[i].SelectSingleNode("fareReference/referenceType")?.InnerText == "TST" &&
                                        nodeList[i].SelectSingleNode("fareReference/uniqueReference")?.InnerText == x))
                    {
                        if (nodeList.Count.Equals(okTst.Count))
                        {//Replace Ref
                            docRS.SelectSingleNode("//fareReference[referenceType='TST']/uniqueReference").InnerText = okTst.First();
                        }
                        else
                            nodeList[i].ParentNode?.RemoveChild(nodeList[i]);
                    }
                }
            }

            return docRS.InnerXml;
        }

        private List<Tuple<string, string, string>> GetFareFamilyFXX(string request, string pnrRead, XElement oRootStored)
        {
            // Was: new XmlDocument(); doc.LoadXml(request); root = doc.DocumentElement
            XElement root = XDocument.Parse(request).Root;

            var temRes = new List<Tuple<string, string, string>>();

            // Was: root.SelectNodes("//StoredFare")
            var nodes = root.XPathSelectElements("//StoredFare").ToList();

            // Was: new XmlDocument(); pnr.LoadXml(pnrRead); pnr.DocumentElement
            XElement pnrRoot = XDocument.Parse(pnrRead).Root;

            // Was: pnr.DocumentElement.SelectNodes("//travellerInfo/elementManagementPassenger")
            var paxLines = pnrRoot.XPathSelectElements("//travellerInfo/elementManagementPassenger");

            var paxMap = new Dictionary<string, string>();
            // Was: foreach (XmlNode fnode in paxLines)
            foreach (XElement fnode in paxLines)
            {
                // Was: fnode.SelectSingleNode("reference/number").InnerText
                //      fnode.SelectSingleNode("lineNumber").InnerText
                paxMap.Add(
                    fnode.XPathSelectElement("reference/number").Value,
                    fnode.XPathSelectElement("lineNumber").Value);
            }

            // Was: pnr.DocumentElement.SelectNodes("//originDestinationDetails/itineraryInfo[...]").Count
            var segCount = pnrRoot
                .XPathSelectElements("//originDestinationDetails/itineraryInfo[travelProduct/productDetails/identification != 'ARNK']")
                .Count();

            var paxFareSegs = new List<Tuple<string, List<Tuple<string, string, string>>>>();

            // Was: foreach (XmlNode fnode in nodes)
            foreach (XElement fnode in nodes)
            {
                // Was: fnode.Attributes["RPH"].Value
                var tst = (string)fnode.Attribute("RPH");

                if (!paxFareSegs.Any(fs => fs.Item1 == tst))
                    paxFareSegs.Add(new Tuple<string, List<Tuple<string, string, string>>>(
                        tst, new List<Tuple<string, string, string>>()));

                // Was: fnode.SelectNodes("BrandedFares/FareFamily")
                foreach (XElement ffnode in fnode.XPathSelectElements("BrandedFares/FareFamily"))
                {
                    // Was: ffnode.InnerText, ffnode.Attributes["RPH"].Value
                    var ffText = ffnode.Value;
                    var ffRph = (string)ffnode.Attribute("RPH");

                    if (!paxFareSegs.Find(fs => fs.Item1 == tst).Item2
                            .Any(i => i.Item1 == ffText && i.Item2 == "S" && i.Item3 == ffRph))
                        paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Add(
                            new Tuple<string, string, string>(ffText, "S", ffRph));
                }

                // Was: fnode.SelectNodes("FareSegments/AirSegments")
                foreach (XElement ffnode in fnode.XPathSelectElements("FareSegments/AirSegments"))
                {
                    // Was: ffnode.Attributes["TicketDesignator"] != null && !string.IsNullOrEmpty(ffnode.Attributes["TicketDesignator"].Value)
                    var tdAttr = (string)ffnode.Attribute("TicketDesignator");
                    if (!string.IsNullOrEmpty(tdAttr))
                    {
                        // Was: ffnode.Attributes["TicketDesignator"].Value, ffnode.Attributes["RPH"].Value
                        paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Add(
                            new Tuple<string, string, string>(tdAttr, "TD", (string)ffnode.Attribute("RPH")));
                    }
                }

                if (!paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Any(i => i.Item2.StartsWith("P")))
                {
                    // Was: oRootStored.SelectSingleNode("fareList[fareReference/uniqueReference = '...']/paxSegReference")
                    var elemPax = oRootStored.XPathSelectElement(
                        $"fareList[fareReference/uniqueReference = '{(string)fnode.Attribute("RPH")}']/paxSegReference");

                    // Was: fnode.SelectSingleNode("PassengerType").Attributes["Code"].Value
                    var strTSTpax = (string)fnode.XPathSelectElement("PassengerType")?.Attribute("Code");

                    // Was: foreach (XmlNode pax in elemPax.SelectNodes("refDetails/refNumber"))
                    foreach (XElement pax in elemPax.XPathSelectElements("refDetails/refNumber"))
                    {
                        // Was: pax.InnerText
                        paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Add(
                            new Tuple<string, string, string>(
                                "",
                                strTSTpax == "INF" ? "PI" : ((strTSTpax == "ADT" || strTSTpax == "JCB") ? "PA" : "P"),
                                paxMap[pax.Value]));
                    }
                }
            }

            // ── the rest is pure LINQ / list logic — no XML API, unchanged ──────────

            var combRes = new List<Tuple<string, string, string>>();
            paxFareSegs.ForEach(pfs => pfs.Item2.RemoveAll(p => p.Item2 == "T"));
            var paxFareSegsGrouped = paxFareSegs;

            if (paxFareSegsGrouped.SelectMany(x => x.Item2).ToList().FindAll(x => x.Item2 == "S")
                    .TrueForAll(s => s.Item1 == paxFareSegsGrouped.First().Item2.First(x => x.Item2 == "S").Item1)
                && paxFareSegsGrouped.SelectMany(x => x.Item2).ToList().FindAll(x => x.Item2 == "S")
                    .Distinct().Count().Equals(segCount)
                && (!paxFareSegsGrouped.SelectMany(x => x.Item2).ToList().Any(x => x.Item2 == "TD")
                    || (paxFareSegsGrouped.SelectMany(x => x.Item2).ToList().FindAll(x => x.Item2 == "TD")
                            .Distinct().Count().Equals(segCount)
                        && paxFareSegsGrouped.TrueForAll(p =>
                            p.Item2.FindAll(x => x.Item2 == "TD").OrderBy(x => x.Item3)
                                .SequenceEqual(paxFareSegsGrouped.First().Item2.FindAll(x => x.Item2 == "TD")
                                    .OrderBy(x => x.Item3))))))
            {
                string tdes = "";
                var grpTd = paxFareSegsGrouped.First().Item2.FindAll(x => x.Item2 == "TD").GroupBy(x => x.Item1);
                if (grpTd.Count().Equals(1))
                    tdes = grpTd.Select(x => $"/ZO-0*{x.Key}").First();
                else
                    foreach (var item in grpTd)
                        tdes += $"/ZO-0*{item.Key}.{string.Join(",", paxFareSegsGrouped.First().Item2.FindAll(x => x.Item2 == "TD" && x.Item1 == item.Key).Select(x => x.Item3))}";

                combRes.Add(new Tuple<string, string, string>(
                    "", $"/FF-{paxFareSegsGrouped.First().Item2.First(x => x.Item2 == "S").Item1}{tdes}", ""));
            }
            else
            {
                foreach (var psg in paxFareSegsGrouped.GroupBy(x => new
                {
                    segs = string.Join(",", x.Item2.FindAll(p => p.Item2.StartsWith("S")).OrderBy(o => o.Item3).Select(s => $"{s.Item3}-{s.Item1}")),
                    tktdes = string.Join("", x.Item2.FindAll(p => p.Item2.StartsWith("TD")).OrderBy(o => o.Item3).Select(s => s.Item1)),
                    isInf = x.Item2.Any(p => p.Item2.Equals("PI"))
                }))
                {
                    var segs = psg.First().Item2.FindAll(p => p.Item2.StartsWith("S"))
                                   .TrueForAll(s => s.Item1.Equals(psg.First().Item2.First().Item1))
                               && psg.First().Item2.FindAll(p => p.Item2.StartsWith("S")).Count.Equals(segCount)
                        ? $"/FF-{psg.First().Item2.First(x => x.Item2 == "S").Item1}"
                        : $"{string.Join("", psg.First().Item2.FindAll(p => p.Item2.StartsWith("S")).SelectMany(s => $"/FF{s.Item3}-{s.Item1}"))}";

                    string tdes = "";
                    var grpTd = psg.First().Item2.FindAll(x => x.Item2 == "TD").GroupBy(x => x.Item1);
                    if (grpTd.Count().Equals(1) && paxFareSegsGrouped.First().Item2.FindAll(x => x.Item2 == "TD").Count.Equals(segCount))
                        tdes = grpTd.Select(x => $"/ZO-0*{x.Key}").First();
                    else
                        foreach (var item in grpTd)
                            tdes += $"/ZO-0*{item.Key}.{string.Join(",", paxFareSegsGrouped.First().Item2.FindAll(x => x.Item2 == "TD" && x.Item1 == item.Key).Select(x => x.Item3))}";

                    var tRes = new List<string>();
                    var isInf = psg.SelectMany(x => x.Item2).Any(p => p.Item2.Equals("PI"));
                    tRes.Add(
                        $"/P{string.Join(",", psg.SelectMany(x => x.Item2).Where(p => p.Item2.StartsWith("P")).SelectMany(s => $"{s.Item3}"))}" +
                        $"{(isInf ? "/INF" : "")}" +
                        $"{(psg.SelectMany(x => x.Item2).Any(p => p.Item2.Equals("PA")) ? "/PAX" : "")}" +
                        $"{(tRes.Count.Equals(0) ? segs + tdes : "")}");

                    combRes.Add(new Tuple<string, string, string>(
                        string.Join(",", psg.Select(x => x.Item1)), string.Join("/", tRes), ""));
                }
            }

            foreach (var psg in paxFareSegsGrouped)
            {
                var segs = psg.Item2.FindAll(p => p.Item2.StartsWith("S"))
                               .TrueForAll(s => s.Item1.Equals(psg.Item2.First().Item1))
                           && psg.Item2.FindAll(p => p.Item2.StartsWith("S")).Count.Equals(segCount)
                    ? $"/FF-{psg.Item2.First(x => x.Item2 == "S").Item1}"
                    : string.Join("", psg.Item2.FindAll(p => p.Item2.StartsWith("S")).SelectMany(s => $"/FF{s.Item3}-{s.Item1}"));

                string tdes = "";
                var grpTd = psg.Item2.FindAll(x => x.Item2 == "TD").GroupBy(x => x.Item1);
                if (grpTd.Count().Equals(1))
                    tdes = grpTd.Select(x => $"/ZO-0*{x.Key}").First();
                else
                    foreach (var item in grpTd)
                        tdes += $"/ZO-0*{item.Key}.{string.Join(",", psg.Item2.FindAll(x => x.Item2 == "TD" && x.Item1 == item.Key).Select(x => x.Item3))}";

                var tRes = new List<string>();
                var isInf = psg.Item2.Any(x => x.Item2.Equals("PI"));
                temRes.Add(new Tuple<string, string, string>(
                    "", "",
                    $"/P{string.Join(",", psg.Item2.Where(p => p.Item2.StartsWith("P")).SelectMany(s => $"{s.Item3}"))}" +
                    $"{(isInf ? "/INF" : "")}" +
                    $"{(psg.Item2.Any(p => p.Item2.Equals("PA")) ? "/PAX" : "")}" +
                    $"{(tRes.Count.Equals(0) ? segs + tdes : "")}"));
            }

            var res = new List<Tuple<string, string, string>>();
            for (int i = 0; i < combRes.Count; i++)
            {
                res.Add(new Tuple<string, string, string>(
                    combRes[i].Item1,
                    combRes[i].Item2,
                    temRes.Count.Equals(combRes.Count) ? temRes[i].Item3 : combRes[i].Item2));
            }
            return res;
        }

        private List<string> GetPricingOptions(string request, string tstResp, string pnrRead, XElement oRootStored)
        {
            // Was: new XmlDocument(); tstdoc.LoadXml("<Ticket_GetPricingOptions>" + tstResp + "..."); tstdoc.DocumentElement
            XElement tstRoot = XDocument.Parse("<Ticket_GetPricingOptions>" + tstResp + "</Ticket_GetPricingOptions>").Root;

            if (!tstResp.Contains("pricingOptionsGroup"))
                return new List<string> { "" };

            var res = new List<string>();

            // Was: tst.SelectNodes("//Ticket_GetPricingOptionsReply")
            var opts = tstRoot.XPathSelectElements("//Ticket_GetPricingOptionsReply").ToList();

            foreach (XElement opt in opts)
            {
                // Was: new XmlDocument(); poks.LoadXml(opt.SelectSingleNode("documentInformation").OuterXml)
                // XElement is already parsed — query directly, no re-parse needed
                XElement docInfo = opt.XPathSelectElement("documentInformation");

                // Was: poks.SelectNodes("//pricingOptionsGroup").Cast<XmlNode>()
                //          .Select(node => node).Where(n => n.InnerText != "RP" && ...)
                var memberNames = docInfo
                    .XPathSelectElements("//pricingOptionsGroup")
                    .Where(n => n.Value != "RP" && n.Value != "NOP" && n.Value != "RLO" && n.Value != "RU")
                    .ToList();

                // Was: poks.SelectSingleNode("//documentSelection/uniqueReference").InnerText
                var uniqueRef = docInfo.XPathSelectElement("//documentSelection/uniqueReference")?.Value;

                var xml = new XElement("option",
                    // Was: XElement.Parse(po.OuterXml.Replace("pricingOptionsGroup", "pricingOptionGroup"))
                    from po in memberNames
                    select XElement.Parse(po.ToString().Replace("pricingOptionsGroup", "pricingOptionGroup")),
                    new XElement("pricingOptionGroup",
                        new XElement("pricingOptionKey",
                            new XElement("pricingOptionKey", "SEL")),
                        new XElement("paxSegTstReference",
                            new XElement("referenceDetails",
                                new XElement("type", "T"),
                                new XElement("value", uniqueRef)))));

                res.Add(xml.Nodes().Aggregate("", (b, node) => b += node.ToString()));
            }

            return res;
        }
        private List<Tuple<string, string, string>> GetPricingOptionsFXX(string request, string tstResp, string pnrRead, XElement oRootStored)
        {
            XElement root = XDocument.Parse(request).Root;

            XElement tstRsp = XDocument.Parse("<Ticket_GetPricingOptions>" + tstResp + "</Ticket_GetPricingOptions>").Root;

            var res = new List<Tuple<string, string, string>>();

            var storedFaresNodes = root.XPathSelectElements("//StoredFare").ToList();

            XElement pnrRoot = XDocument.Parse(pnrRead).Root;

            var paxLines = pnrRoot.XPathSelectElements("//travellerInfo/elementManagementPassenger");

            var paxMap = new Dictionary<string, string>();
            foreach (XElement fnode in paxLines)
            {
                paxMap.Add(
                    fnode.XPathSelectElement("reference/number").Value,
                    fnode.XPathSelectElement("lineNumber").Value);
            }

            var segCount = pnrRoot
                .XPathSelectElements("//originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR']")
                .Count();

            var paxFareSegs = new List<Tuple<string, List<Tuple<string, string, string>>>>();

            foreach (XElement fnode in storedFaresNodes)
            {
                var tst = (string)fnode.Attribute("RPH");

                if (!paxFareSegs.Any(fs => fs.Item1 == tst))
                    paxFareSegs.Add(new Tuple<string, List<Tuple<string, string, string>>>(
                        tst, new List<Tuple<string, string, string>>()));

                if (!paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Any(i => i.Item2.StartsWith("P")))
                {
                    var elemPax = oRootStored.XPathSelectElement(
                        $"fareList[fareReference/uniqueReference = '{(string)fnode.Attribute("RPH")}']/paxSegReference");

                    var strTSTpax = (string)fnode.XPathSelectElement("PassengerType")?.Attribute("Code");

                    foreach (XElement pax in elemPax.XPathSelectElements("refDetails/refNumber"))
                    {
                        paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Add(new Tuple<string, string, string>(
                            "",
                            strTSTpax == "INF" ? "PI" : ((strTSTpax == "ADT" || strTSTpax == "JCB") ? "PA" : "P"),
                            paxMap[pax.Value]));
                    }
                }

                if (paxFareSegs.Any(fs => fs.Item1 == tst))
                {
                    foreach (XElement optDet in tstRsp.XPathSelectElements(
                        $"//documentInformation[documentSelection/referenceType='TST' and " +
                        $"documentSelection/uniqueReference='{tst}']/pricingOptionsGroup" +
                        $"[pricingOptionKey/pricingOptionKey='PRM']/optionDetail"))
                    {
                        var attrType = optDet.XPathSelectElement("criteriaDetails/attributeType")?.Value;

                        paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Add(
                            new Tuple<string, string, string>("", $"*{attrType}", ""));
                    }
                }
            }

            var paxFareSegsGrouped = paxFareSegs;

            {
                var tRes = new List<string>();
                foreach (var ps in paxFareSegs)
                {
                    var isInf = ps.Item2.Any(p => p.Item2.Equals("PI"));
                    var opt = $"{string.Join(",", ps.Item2.FindAll(p => p.Item2.StartsWith("*")).Select(s => $",{s.Item2}"))}";
                    tRes.Add($"{opt}{(isInf ? "/INF" : "")}" +
                             $"{(ps.Item2.Any(p => p.Item2.Equals("PA")) ? "/PAX" : "")}");

                    var fbCodes = new Dictionary<string, string>();
                    var tktDes = new Dictionary<string, string>();

                    var storedFareNode = root.XPathSelectElement($"//StoredFare[@RPH='{ps.Item1}']");
                    foreach (XElement item in storedFareNode.XPathSelectElements("FareSegments/AirSegments"))
                    {
                        if ((string)storedFareNode.Attribute("FareType") != "Private")
                        {
                            var key = item.Value.TrimEnd('/');
                            fbCodes[key] = fbCodes.ContainsKey(key)
                                ? fbCodes[key] + "," + (string)item.Attribute("RPH")
                                : (string)item.Attribute("RPH");
                        }

                        var tdVal = (string)item.Attribute("TicketDesignator");
                        if (!string.IsNullOrEmpty(tdVal))
                            tktDes[tdVal] = tktDes.ContainsKey(tdVal)
                                ? tktDes[tdVal] + "," + (string)item.Attribute("RPH")
                                : (string)item.Attribute("RPH");
                    }

                    var fbOpt = string.Join("/", fbCodes.Where(x => !string.IsNullOrEmpty(x.Key)).Select(x => $"A{x.Value}-{x.Key}"));
                    if (!string.IsNullOrEmpty(fbOpt.Trim(' ')))
                        tRes.Add(fbOpt);
                    if (tktDes.Any())
                        tRes.Add(string.Join("/", tktDes.Select(x => $"ZO-0*{x.Key}.{x.Value}")));

                    tRes.Add($"P{string.Join(",", ps.Item2.FindAll(p => p.Item2.StartsWith("P")).SelectMany(s => $"{s.Item3}"))}");
                    res.Add(new Tuple<string, string, string>(
                        ps.Item1, string.Join("/", tRes).Replace("/,", ",").TrimEnd('/'), ""));
                    tRes = new List<string>();
                }
            }

            if (res.TrueForAll(r =>
                    Regex.Replace(r.Item2, @"\/(P\d+(,\d+)*|PAX|PI|INF)", "")
                        .Equals(Regex.Replace(res.First().Item2, @"\/(P\d+(,\d+)*|PAX|PI|INF)", ""))))
            {
                var combRes = Regex.Replace(res.First().Item2, @"\/(P\d+(,\d+)*|PAX|PI|INF)", "");
                res = res.Select(x => new Tuple<string, string, string>(x.Item1, "", x.Item2)).ToList();
                res[0] = new Tuple<string, string, string>(res.First().Item1, combRes, res.First().Item3);
            }

            return res;
        }

        private List<string> GetPricingOptionsFF_FB(string request, string pnrRead, XElement oRootStored)
        {
            // Was: new XmlDocument(); doc.LoadXml(request); doc.DocumentElement
            XElement root = XDocument.Parse(request).Root;

            var reqElems = "BrandedFares/FareFamily";
            var isFF = true;

            // Was: root.SelectNodes("//StoredFare/BrandedFares").Cast<XmlNode>().ToList().TrueForAll(x => x.InnerText.Equals(""))
            if (!request.Contains("BrandedFares") ||
                root.XPathSelectElements("//StoredFare/BrandedFares").ToList().TrueForAll(x => x.Value.Equals("")))
            {
                isFF = false;
                reqElems = "FareSegments/AirSegments[text() != 'VOID']";
            }

            var res = new List<string>();

            // Was: root.SelectNodes("//StoredFare")
            var nodes = root.XPathSelectElements("//StoredFare").ToList();

            // Was: new XmlDocument(); pnr.LoadXml(pnrRead); pnr.DocumentElement
            XElement pnrRoot = XDocument.Parse(pnrRead).Root;

            // Was: pnr.DocumentElement.SelectNodes("//originDestinationDetails/itineraryInfo")
            var flightSegs = pnrRoot.XPathSelectElements("//originDestinationDetails/itineraryInfo");

            var flSegMap = new Dictionary<string, string>();
            foreach (XElement fnode in flightSegs)
            {
                // Was: fnode.SelectSingleNode("elementManagementItinerary/lineNumber").InnerText
                //      fnode.SelectSingleNode("elementManagementItinerary/reference/number").InnerText
                flSegMap.Add(
                    fnode.XPathSelectElement("elementManagementItinerary/lineNumber").Value,
                    fnode.XPathSelectElement("elementManagementItinerary/reference/number").Value);
            }

            var paxFareSegs = new List<Tuple<string, List<Tuple<string, string, string>>>>();

            foreach (XElement fnode in nodes)
            {
                // Was: fnode.Attributes["RPH"].Value
                var tst = (string)fnode.Attribute("RPH");

                if (!paxFareSegs.Any(fs => fs.Item1 == tst))
                    paxFareSegs.Add(new Tuple<string, List<Tuple<string, string, string>>>(
                        tst, new List<Tuple<string, string, string>>()));

                // Was: fnode.SelectNodes(reqElems)
                foreach (XElement ffnode in fnode.XPathSelectElements(reqElems))
                {
                    // Was: ffnode.InnerText, ffnode.Attributes["RPH"].Value
                    var ffText = ffnode.Value;
                    var ffRph = (string)ffnode.Attribute("RPH");

                    if (!paxFareSegs.Find(fs => fs.Item1 == tst).Item2
                            .Any(i => i.Item1 == ffText && i.Item2 == "S" && i.Item3 == ffRph))
                        paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Add(
                            new Tuple<string, string, string>(ffText, "S", ffRph));

                    if (!paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Any(i => i.Item2.StartsWith("P")))
                    {
                        // Was: oRootStored.SelectSingleNode("fareList[fareReference/uniqueReference = '...']/paxSegReference")
                        var elemPax = oRootStored.XPathSelectElement(
                            $"fareList[fareReference/uniqueReference = '{(string)fnode.Attribute("RPH")}']/paxSegReference");

                        // Was: oRootStored.SelectSingleNode("fareList[...]/statusInformation/firstStatusDetails/tstFlag")
                        var oTSTpax = oRootStored.XPathSelectElement(
                            $"fareList[fareReference/uniqueReference = '{(string)fnode.Attribute("RPH")}']/statusInformation/firstStatusDetails/tstFlag");

                        // Was: oTSTpax != null ? oTSTpax.InnerXml : ""
                        string strTSTpax = oTSTpax?.Value ?? "";

                        // Was: foreach (XmlNode pax in elemPax.SelectNodes("refDetails/refNumber"))
                        foreach (XElement pax in elemPax.XPathSelectElements("refDetails/refNumber"))
                        {
                            // Was: pax.InnerText
                            paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Add(
                                new Tuple<string, string, string>(
                                    "",
                                    strTSTpax == "INF" ? "PI" : (strTSTpax == "CHD" ? "P" : "PA"),
                                    pax.Value));
                        }
                    }
                }
            }

            paxFareSegs.ForEach(pfs => pfs.Item2.RemoveAll(p => p.Item1 == "T"));

            var paxFareSegsGrouped = new List<Tuple<string, List<Tuple<string, string, string>>>>();
            foreach (var pfs in paxFareSegs)
            {
                if (paxFareSegsGrouped.Any(x => x.Item2.Any(pg =>
                        pfs.Item2.FindAll(p => p.Item2.StartsWith("P"))
                            .Any(pf => pf.Item2 == pg.Item2 && pf.Item3 == pg.Item3))))
                    continue;

                paxFareSegsGrouped.Add(new Tuple<string, List<Tuple<string, string, string>>>(
                    pfs.Item1, new List<Tuple<string, string, string>>()));

                var pgs = paxFareSegsGrouped.Find(x => x.Item1 == pfs.Item1);
                foreach (var pfg in paxFareSegs
                    .FindAll(x => x.Item2.FindAll(i => i.Item2 == "S").OrderBy(i => i.Item3)
                        .Except(pfs.Item2.FindAll(pi => pi.Item2 == "S").OrderBy(pi => pi.Item3))
                        .Count().Equals(0))
                    .SelectMany(x => x.Item2))
                {
                    if (!pgs.Item2.Any(x => x.Item1 == pfg.Item1 && x.Item2 == pfg.Item2 && x.Item3 == pfg.Item3))
                        pgs.Item2.Add(pfg);
                }
            }

            // XElement construction is already using LINQ to XML — no change needed here
            var xml = new XElement("options",
                from fgs in paxFareSegsGrouped
                select new XElement("option",
                    from fs in fgs.Item2.FindAll(x => !string.IsNullOrEmpty(x.Item1)).Select(x => x.Item1).Distinct()
                    select new XElement("pricingOptionGroup",
                        new XElement("pricingOptionKey",
                            new XElement("pricingOptionKey", isFF ? "PFF" : "FBA")),
                        new XElement("optionDetail",
                            isFF
                                ? new XElement("criteriaDetails",
                                    new XElement("attributeType", "FF"),
                                    new XElement("attributeDescription", fs))
                                : new XElement("criteriaDetails",
                                    new XElement("attributeType", fs.TrimEnd('/')))),
                        new XElement("paxSegTstReference",
                            fgs.Item2.FindAll(f => f.Item1 == fs || string.IsNullOrEmpty(f.Item1))
                                .OrderByDescending(fse => fse.Item2)
                                .Select(fseg => new XElement("referenceDetails",
                                    new XElement("type", fseg.Item2),
                                    new XElement("value", fseg.Item2 == "S" ? flSegMap[fseg.Item3] : fseg.Item3)))))));

            res = xml.Elements().Select(x =>
            {
                var innerXml = new StringBuilder();
                x.Nodes().ToList().ForEach(node => innerXml.Append(node.ToString()));
                return innerXml.ToString();
            }).ToList();

            return res;
        }

        private string GetCreateTSTrq(string response)
        {
            var res = string.Empty;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);
            XmlElement root = doc.DocumentElement;

            XmlNodeList nodes = root.SelectNodes("//fareList");

            List<string> tsts = new List<string>();

            foreach (XmlNode fnode in nodes)
            {
                foreach (XmlNode ffnode in fnode.SelectNodes("fareReference/uniqueReference"))
                {
                    tsts.Add(ffnode.InnerText);
                }
            }

            var xml = new XElement("options",
                from fs in tsts
                select new XElement("psaList",
                    new XElement("itemReference",
                        new XElement("referenceType", "TST"),
                        new XElement("uniqueReference", fs)))
            );
            Console.WriteLine(xml);

            res = string.Join("", xml.Elements().Select(x => x.ToString()).ToList());
            return res;
        }

        public string TransferOwnership()
        {
            string strResponse;
            try
            {
                DateTime requestTime = DateTime.Now;
                var sbNativeLogMessge = new StringBuilder();

                //*****************************************************************
                // Transform OTA TransferOwnership Request into Native Amadeus Request     *
                //***************************************************************** 
                string strRequest = SetRequest($"AmadeusWS_PNRReadRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************
                // Create Session    *
                // *******************             
                AmadeusWSAdapter ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //****************************************
                // Send Retreive PNR request to Amadeus  *
                //****************************************

                try
                {
                    sbNativeLogMessge.Append(strRequest);
                    strResponse = SendRetrivePNRbyRL(ttAA, strRequest);
                    sbNativeLogMessge.Append($"{Environment.NewLine}{strResponse}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Request. {ex.Message}");
                }

                #region business logic
                if (ttProviderSystems.BLFile != "" && strResponse.IndexOf("generalErrorInfo") == -1)
                {
                    var oDocBL = new XmlDocument();
                    oDocBL.Load(ttProviderSystems.BLFile);

                    XmlElement oRootBL = oDocBL.DocumentElement;
                    XmlNode oNodeBL = oRootBL.SelectSingleNode($"Security/ProviderBL[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\']");

                    if (oNodeBL != null)
                    {
                        // get accounting line from PNR
                        var oDocRespBL = new XmlDocument();
                        oDocRespBL.LoadXml(strResponse);

                        XmlElement oRootRespBL = oDocRespBL.DocumentElement;
                        XmlNode oNodeRespBL = oRootRespBL.SelectSingleNode("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']");

                        if (oNodeRespBL != null)
                        {
                            if (!(oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']") == null))
                            {
                                string strAIAN = oNodeRespBL.SelectSingleNode("accounting/account/number").InnerXml;
                                string strAIN_BL = oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']/@AuthorizeCode").InnerXml;

                                //compare to the one in BL file 

                                if (strAIAN != strAIN_BL)
                                {
                                    throw new Exception("Secured PNR");
                                }
                            }
                        }
                    }
                }
                #endregion

                strRequest = SetRequest($"AmadeusWS_TransferOwnershipRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //**********************************************
                // Send transfer ownership request to Amadeus  *
                //**********************************************
                if (!strRequest.Contains("PoweredPNR_TransferOwnership"))
                {
                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(strRequest);
                    XmlElement oRoot = oDoc.DocumentElement;
                    if (oRoot != null)
                    {
                        foreach (XmlNode oNode in oRoot.ChildNodes)
                        {
                            sbNativeLogMessge.Append($"{Environment.NewLine}{oNode.OuterXml}");
                            strResponse = SendRequestCryptically(ttAA, oNode.OuterXml);
                            sbNativeLogMessge.Append($"{Environment.NewLine}{strResponse}");

                            if (strResponse.Contains("RESTRICTED"))
                                break;

                        }
                    }

                    if (!strResponse.Contains("RESTRICTED"))
                    {
                        strRequest = "<PNR_AddMultiElements><pnrActions><optionCode>10</optionCode></pnrActions></PNR_AddMultiElements>";

                        sbNativeLogMessge.Append(strRequest);
                        strResponse = SendAddMultiElements(ttAA, strRequest);
                        sbNativeLogMessge.Append($"{Environment.NewLine}{strResponse}");

                        if (strResponse.Contains("/\rWARNING"))
                        {
                            sbNativeLogMessge.Append(strRequest);
                            strResponse = SendAddMultiElements(ttAA, strRequest);
                            sbNativeLogMessge.Append($"{Environment.NewLine}{strResponse}");
                        }

                        strResponse = strResponse.Replace("</PNR_Reply>", $"{Request}</PNR_Reply>");
                    }
                }

                //**************************************************************************
                // Transform Native Amadeus TransferOwnership Response into OTA Response   *
                //************************************************************************** 
                try
                {
                    var tagToReplace = strResponse.Contains("PNR_Reply")
                        ? "</PNR_Reply>"
                        : strResponse.Contains("PoweredPNR_TransferOwnershipReply")
                            ? "</PoweredPNR_TransferOwnershipReply>"
                            : " </Command_CrypticReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_TransferOwnershipRS.xsl");
                    DateTime responseTime = DateTime.Now;
                    string strMessage = sbNativeLogMessge.ToString();
                    sbNativeLogMessge.Clear();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response. \r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = "";
                    }
                }
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.TransferOwnership, exx.Message, ttProviderSystems, "");
            }


            return strResponse;
        }

        public string PNREnd()
        {
            string response;

            try
            {
                AmadeusWSAdapter ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //*****************************************************************
                // Transform OTA PNREnd Request into Native Amadeus Request     *
                //***************************************************************** 
                string strRequest = SetRequest($"AmadeusWS_PNREndRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //*******************************************************************************
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                //******************************************************************************* 
                response = SendAddMultiElements(ttAA, strRequest);
                var warning = string.Empty;

                if (response.Contains("SIMULTANEOUS CHANGES TO PNR"))
                {
                    SendCommandCryptically(ttAA, "IR");
                    response = SendAddMultiElements(ttAA, strRequest);
                    warning = $"<Warnings><Warning Type=\"TRPXML\">PNR IGNORED AND REDISPLAYED DUE TO SIMULTANEOUS CHANGE</Warning></Warnings>";
                }

                string strResponseTST = string.Empty;
                if (response.Contains("<longFreetext>--- TST "))
                    strResponseTST = SendDisplayTST(ttAA);


                #region GetPricingOptions

                var GetPricingOptionsTST = string.Empty;

                if (strResponseTST.Contains("<referenceType>TST</referenceType>"))
                {
                    //var tstNum = strResponseTST.Substring(strResponseTST.IndexOf("<referenceType>TST</referenceType>", StringComparison.Ordinal));
                    //tstNum = tstNum.Substring(tstNum.IndexOf("<uniqueReference>") + "<uniqueReference>".Length, tstNum.IndexOf("</uniqueReference>") - tstNum.IndexOf("<uniqueReference>") - "<uniqueReference>".Length);

                    var oDocTST = new XmlDocument();
                    oDocTST.LoadXml(strResponseTST);
                    XmlElement oRootTST = oDocTST.DocumentElement;
                    var xmlNodeList = oRootTST?.SelectNodes("fareList");

                    if (xmlNodeList != null)
                        foreach (XmlNode oNodeTST in xmlNodeList)
                        {
                            string tstPricingOption = string.Empty;
                            string tstNum = oNodeTST.SelectSingleNode("fareReference/uniqueReference").InnerText;
                            var request = $"<Ticket_GetPricingOptions xmlns=\"http://xml.amadeus.com/TPORRQ_14_1_1A\"><documentSelection><referenceType>TST</referenceType><uniqueReference>{tstNum}</uniqueReference></documentSelection></Ticket_GetPricingOptions>";

                            tstPricingOption = SendGetPricingOptions(ttAA, request);
                            GetPricingOptionsTST += tstPricingOption;
                        }
                }
                #endregion

                var tagToReplace = response.Contains("PNR_RetrieveByRecLocReply") ? "</PNR_RetrieveByRecLocReply>" : "</PNR_Reply>";

                if (!string.IsNullOrEmpty(GetPricingOptionsTST))
                    response = response.Replace(tagToReplace, $"{GetPricingOptionsTST}{tagToReplace}");


                response = ConvertToTripXMLMessage(ttAA, strRequest, inSession, response, strResponseTST);
                if (!string.IsNullOrEmpty(warning))
                    response = response.Replace("<Success />", $"<Success />{warning}");

            }
            catch (Exception exx)
            {
                response = modCore.FormatErrorMessage(modCore.ttServices.PNREnd, exx.Message, ttProviderSystems);
            }
            return response;
        }

        private string ConvertToTripXMLMessage(AmadeusWSAdapter ttAA, string request, bool inSession, string response, string responseTST = "")
        {
            //*****************************************************************
            // Transform Native Amadeus PNRRead Response into OTA Response   *
            //***************************************************************** 
            try
            {
                var tagToReplace = response.Contains("PNR_RetrieveByRecLoc") ? "</PNR_RetrieveByRecLoc>" : "</PNR_Reply>";
                response = response.Replace(tagToReplace, $"{responseTST}{request}{tagToReplace}");
                if (inSession)
                    response = response.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                Version = "v03_";
                response = GetFinalResponse(ttAA, inSession, ref response, "PNRRead");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
            }
            return response;
        }

        //private string NewMethod(string strResponse, bool inSession)
        //{
        //    string strToReplace = "</PNR_RetrieveByRecLocReply>";
        //    if (!strResponse.Contains(strToReplace))
        //        strToReplace = "</PNR_Reply>";
        //    strResponse = strResponse.Replace(strToReplace, $"{Request}{strToReplace}");

        //    if (inSession)
        //        strResponse = strResponse.Replace(strToReplace, $"<ConversationID>{ConversationID}</ConversationID>{strToReplace}");

        //    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_PNRReadRS.xsl");
        //    return strResponse;
        //}

        private string HandleCrypticComands(AmadeusWSAdapter ttAA, string command)
        {
            try
            {
                string crypticRS = SendCommandCryptically(ttAA, command);
                var oDocCryptic = new XmlDocument();
                oDocCryptic.LoadXml(crypticRS);
                XmlElement oRootCryptic = oDocCryptic.DocumentElement;
                string strScreen = oRootCryptic.SelectSingleNode("longTextString/textStringDetails").InnerText;

                crypticRS = formatAmadeus(strScreen);
                return crypticRS;
            }
            catch (Exception ex)
            {
                return modCore.FormatErrorMessage(modCore.ttServices.SearchName, ex.Message, ttProviderSystems);
            }
        }

        private string GetTicketStatus(string ticketImage)
        {
            try
            {
                var myRegex = new Regex(@"\d\s[A-Z]{3,4} ");
                List<string> lines = ticketImage.Split(new[] { "<Line>", "</Line>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var flightLines = lines.Where(f => myRegex.IsMatch(f)).ToList().FindAll(f => f.Contains(" V "));
                return flightLines.Count.Equals(0) ? "E" : "V";
            }
            catch (Exception ex)
            {
                return modCore.FormatErrorMessage(modCore.ttServices.SearchName, ex.Message, ttProviderSystems);
            }
        }

        private string ProcessFHTickets(AmadeusWSAdapter ttAA, XmlNodeList xmlNodeList)
        {
            var FHRS = string.Empty;

            foreach (XmlNode oNodeFH in xmlNodeList)
            {
                string tktnum = oNodeFH.SelectSingleNode("longFreetext").InnerText;
                if (string.IsNullOrEmpty(tktnum))
                    break;

                if (tktnum.Length != 14)
                    tktnum = tktnum.Substring(4, 14).Trim();
                var mTicket = HandleCrypticComands(ttAA, $"TWD/TKT{tktnum}");
                if (mTicket.Length > 0)
                    FHRS += $"<Ticket Number=\"{tktnum}\">{GetTicketStatus(mTicket.Replace("&", "&amp;"))}</Ticket>";
            }

            return FHRS;
        }

        public string SearchName()
        {
            string strResponse;
            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //*****************************************************************
                // Transform Search Name Request into Native Amadeus Request     *
                //***************************************************************** 
                string strRequest = SetRequest($"AmadeusWS_SearchNameRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //*******************************************************************************
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                //******************************************************************************* 

                strResponse = SendRetrievePNR(ttAA, strRequest);

                //*****************************************************************
                // Transform Native Amadeus PNRRead Response into OTA Response   *
                //***************************************************************** 
                try
                {
                    var tagToReplace = strRequest.Contains("PNR_List") ? "</PNR_List>" : "</PNR_Reply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_SearchNameRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming OTA Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = "";
                    }
                }
            }
            catch (Exception exx)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.SearchName, exx.Message, ttProviderSystems);
            }

            return strResponse;
        }

        private bool IsCorporateFare(string pricingOptions, out string pricingOptionsGroup, out string corporateId)
        {

            try
            {
                var doc = XDocument.Parse($"<root>{pricingOptions}</root>");
                corporateId = string.Empty;

                var rwGroup = doc
                    .Descendants("pricingOptionsGroup")
                    .FirstOrDefault(g =>
                        (string)g.Element("pricingOptionKey")
                                ?.Element("pricingOptionKey") == "RW");

                if (rwGroup != null)
                {
                    corporateId = rwGroup.Element("optionDetail")
                       ?.Element("criteriaDetails")
                       ?.Element("attributeType")
                       ?.Value;

                    pricingOptionsGroup = rwGroup.ToString().Replace("pricingOptionsGroup", "pricingOptionGroup");

                    return true;
                }

                pricingOptionsGroup = string.Empty;
                return false;
            }
            catch
            {
                pricingOptionsGroup = string.Empty;
                corporateId = string.Empty;
                return false;

            }
        }
    }
}
