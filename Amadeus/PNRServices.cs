using System.Collections.Generic;
using System.Xml;
using TripXMLMain;
using System.Text;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
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

                CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response size", strResponse.Length.ToString(CultureInfo.InvariantCulture), ttProviderSystems.LogUUID);
                if (strResponse.Length > 5500)
                {
                    CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response I", strResponse.Substring(0, strResponse.Length / 2), ttProviderSystems.LogUUID);
                    CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response II", strResponse.Substring(strResponse.Length / 2), ttProviderSystems.LogUUID);
                }
                else
                {
                    CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response I", strResponse, ttProviderSystems.LogUUID);
                }


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

                try
                {
                    strResponse = ConvertToTripXMLMessage(Request, inSession, strResponse);

                    DateTime responseTime = DateTime.Now;
                    String strMeessege = sbNativeLog.ToString();
                    sbNativeLog.Remove(0, sbNativeLog.Length);

                    if (ttProviderSystems.LogNative && strMeessege.Trim() != string.Empty)
                    {
                        TripXMLTools.TripXMLLog.LogMessage("PNRRead", ref strMeessege, requestTime, responseTime, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                    }
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
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
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

                if (ttProviderSystems.LogNative)
                    TripXMLTools.TripXMLLog.LogMessage("PNRCancel", ref strMessage, requestTime, responseTime, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
            }

            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
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

                if (ttProviderSystems.LogNative)
                {
                    TripXMLTools.TripXMLLog.LogMessage("PNRSplit", ref strMessage, requestTime, responseTime, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
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
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
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

                        string strRTSVI = SendCommandCryptically(ttAA, "RTSVI");

                        var oDocCryptic = new XmlDocument();
                        oDocCryptic.LoadXml(strRTSVI);
                        XmlElement oRootCryptic = oDocCryptic.DocumentElement;
                        string strScreen = oRootCryptic.SelectSingleNode("longTextString/textStringDetails").InnerText;

                        strRTSVI = formatAmadeus(strScreen);
                        if (strRTSVI.Length > 0)
                        {
                            strRTSVI = $"<RTSVI>{strRTSVI.Replace("&", "&amp;")}</RTSVI>";
                            strResponse = strResponse.Replace("</PNR_Reply>", $"{strRTSVI}</PNR_Reply>");
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

                        if (strResponse.Length > 5500)
                        {
                            CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response I", strResponse.Substring(0, strResponse.Length / 2), ttProviderSystems.LogUUID);
                            CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response II", strResponse.Substring(strResponse.Length / 2), ttProviderSystems.LogUUID);
                        }
                        else
                        {
                            CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response I", strResponse, ttProviderSystems.LogUUID);
                        }

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
                    addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
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
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, exx.Message, ttProviderSystems, "");
            }
            finally
            {
                GC.Collect();
            }

            return strResponse;
        }

        public string PNRReprice()
        {
            string strResponse;
            //*****************************************************************
            // Transform OTA PNRRead Request into Native Amadeus Request     *
            //***************************************************************** 
            try
            {
                string strResponseReprice = "";
                bool bStoreFare = true;
                _tracerID = string.Empty;

                string strRequest = SetRequest($"AmadeusWS_PNRRepriceRQ.xsl");

                bool bMarkup = Request.Contains("Markup");
                bool bPrivate = Request.Contains("Private");
                bool bPublished = Request.Contains("Published");
                bool bExchange = Request.Contains("FareQualifier=\"EXC\"") || Request.Contains("FareQualifier=\"EX\"") || Request.Contains("FareQualifier=\"EXL\"");

                var otaDoc = new XmlDocument();
                otaDoc.LoadXml(Request);
                XmlElement oRootReq = otaDoc.DocumentElement;
                if (oRootReq != null && oRootReq.HasAttribute("StoreFare") && oRootReq.SelectSingleNode("@StoreFare")?.InnerText == "false")
                    bStoreFare = false;

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************
                // Create Session    *
                // *******************
                AmadeusWSAdapter ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //*******************************************************************************
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                //******************************************************************************* 

                string strPNRReplay;

                strPNRReplay = inSession
                    ? SendRetrievePNR(ttAA)
                    : SendRetrivePNRbyRL(ttAA, strRequest);

                # region business logic
                if (ttProviderSystems.BLFile != "" && !strPNRReplay.Contains("generalErrorInfo"))
                {
                    var oDocBL = new XmlDocument();
                    oDocBL.Load(ttProviderSystems.BLFile);

                    XmlElement oRootBL = oDocBL.DocumentElement;
                    XmlNode oNodeBL = oRootBL.SelectSingleNode("Security/ProviderBL[@Name=\'Amadeus\'][@System=\'" + ttProviderSystems.System + "\']");

                    if (oNodeBL != null)
                    {
                        // get accounting line from PNR
                        var oDocRespBL = new XmlDocument();
                        oDocRespBL.LoadXml(strPNRReplay);

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
                                    throw new Exception("Secured PNR");
                            }
                        }
                    }
                }
                # endregion

                string strResponseTST = string.Empty;
                if (strPNRReplay.Contains("<longFreetext>--- TST ") && strPNRReplay.Contains("originDestinationDetails"))
                {
                    strResponseTST = SendDisplayTST(ttAA);

                    #region GetPricingOptions

                    var GetPricingOptionsTST = string.Empty;

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
                                var request = $"<Ticket_GetPricingOptions xmlns=\"http://xml.amadeus.com/TPORRQ_14_1_1A\"><documentSelection><referenceType>TST</referenceType><uniqueReference>{tstNum}</uniqueReference></documentSelection></Ticket_GetPricingOptions>";

                                tstPricingOption = SendGetPricingOptions(ttAA, request);
                                GetPricingOptionsTST += tstPricingOption;
                            }
                    }
                    #endregion

                    XmlDocument oDocStored;
                    XmlElement oRootStored;
                    if (!strResponseTST.Contains("NO TST RECORD EXISTS"))
                    {
                        var paxAssoc = new Dictionary<Tuple<string, int>, int>();
                        string strFareType;
                        if (Request.Contains("StoreHistoricalFare") && bStoreFare)
                        {
                            #region Store Fare

                            // Collect TST and passenger RPH references
                            var oTSTDoc = new XmlDocument();
                            oTSTDoc.LoadXml(strResponseTST);
                            var oTSTnodes = oTSTDoc.DocumentElement?.SelectNodes("fareList");
                            if (oTSTnodes != null)
                                foreach (XmlNode tst in oTSTnodes)
                                {
                                    var paxRPH = int.Parse(tst.SelectSingleNode("paxSegReference/refDetails/refNumber")?.InnerText);
                                    var paxTST = int.Parse(tst.SelectSingleNode("fareReference/uniqueReference")?.InnerText);
                                    var pn = oRootReq.SelectSingleNode($"StoredFare[@RPH='{paxTST}']");
                                    paxAssoc.Add(new Tuple<string, int>(pn.SelectSingleNode("PassengerType/@Code")?.InnerText, paxTST), paxRPH);
                                }

                            oRootStored = oTSTDoc.DocumentElement;

                            if (oRootStored != null)
                            {
                                strFareType = "";

                                if (oRootReq.SelectSingleNode("StoredFare").SelectSingleNode("@FareType") != null)
                                {
                                    if (oRootReq.SelectSingleNode("StoredFare").SelectSingleNode("@FareType").InnerXml == "Private")
                                        strFareType = "/R,U";
                                    else
                                        strFareType = "/R";
                                }

                                List<Tuple<string, string>> ffList;
                                // Blocked check
                                //if (!GetPricingOptionsTST.Contains(">PFF<"))
                                if (!Request.Contains("BrandedFares") || Request.Replace(" ", "").Contains("<BrandedFares/>"))
                                    ffList = GetPricingOptionsFXX(Request, GetPricingOptionsTST, strPNRReplay, oRootStored);
                                else
                                    ffList = GetFareFamilyFXX(Request, strPNRReplay, oRootStored);

                                string strHistFareRS;
                                string strEndTransaction;
                                foreach (var ff in ffList)
                                {
                                    var fxOpt = System.Text.RegularExpressions.Regex.Replace(ff.Item2, @"\/ZO-0\*[A-Z0-9.,]*", "");
                                    strHistFareRS = SendCommandCryptically(ttAA, $"FXX{strFareType}{fxOpt}");
                                    if (!string.IsNullOrEmpty(ff.Item1))
                                    {
                                        var fxxResp = strHistFareRS.Split(new[] { '\r', '\n' }).ToList();
                                        var sIdx = fxxResp.FindIndex(x => x.StartsWith("LAST"));//one pax
                                        if (sIdx > 0)
                                        {
                                            fxxResp = fxxResp.GetRange(sIdx, fxxResp.Count - sIdx);
                                            sIdx = fxxResp.FindIndex(x => string.IsNullOrEmpty(x));
                                            fxxResp = fxxResp.GetRange(sIdx, fxxResp.Count - sIdx);
                                            sIdx = fxxResp.FindIndex(x => x.Trim().StartsWith("USD"));
                                            var sAmount = fxxResp[sIdx].Split(new[] { ' ' },
                                                StringSplitOptions.RemoveEmptyEntries)[1];
                                            var tAmount = oRootStored.SelectSingleNode($"fareList[fareReference/uniqueReference = '{ff.Item1}']/fareDataInformation/fareDataSupInformation[(fareDataQualifier='E' and fareCurrency='USD') or (fareDataQualifier='B' and fareCurrency='USD')]/fareAmount") == null
                                                ? "0.00"
                                                : oRootStored.SelectSingleNode($"fareList[fareReference/uniqueReference = '{ff.Item1}']/fareDataInformation/fareDataSupInformation[(fareDataQualifier='E' and fareCurrency='USD') or (fareDataQualifier='B' and fareCurrency='USD')]/fareAmount").InnerText;
                                            if (sAmount != tAmount)
                                                throw new Exception("ADULTS AND KIDS - DIFFERENT FARE FAMILIES. ISSUE MANUALLY");
                                        }
                                        else if (fxxResp.FindIndex(x => x.Trim().StartsWith("PASSENGER")) > 0)//several pax
                                        {
                                            sIdx = fxxResp.FindIndex(x => x.Trim().StartsWith("PASSENGER"));
                                            fxxResp = fxxResp.GetRange(sIdx + 1, fxxResp.Count - (sIdx + 1));
                                            sIdx = fxxResp.FindIndex(string.IsNullOrEmpty);
                                            fxxResp = fxxResp.GetRange(0, sIdx);
                                            var bfList = fxxResp.Select(s => s.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Reverse().Skip(2).First()).Select(x => new { FXXAmount = x, IsPresent = false }).ToList();
                                            foreach (var iTst in ff.Item1.Split(','))
                                            {
                                                var tAmount = oRootStored.SelectSingleNode($"fareList[fareReference/uniqueReference = '{iTst}']/fareDataInformation/fareDataSupInformation[(fareDataQualifier='E' and fareCurrency='USD') or (fareDataQualifier='B' and fareCurrency='USD')]/fareAmount") == null
                                                    ? "0.00"
                                                    : oRootStored.SelectSingleNode($"fareList[fareReference/uniqueReference = '{iTst}']/fareDataInformation/fareDataSupInformation[(fareDataQualifier='E' and fareCurrency='USD') or (fareDataQualifier='B' and fareCurrency='USD')]/fareAmount").InnerText;
                                                if (!bfList.Select(x => x.FXXAmount).Contains(tAmount))
                                                    throw new Exception("ADULTS AND KIDS - DIFFERENT FARE FAMILIES. ISSUE MANUALLY");
                                                else
                                                {
                                                    bfList = bfList.Select(x => new { x.FXXAmount, IsPresent = x.IsPresent || x.FXXAmount.Equals(tAmount) }).ToList();
                                                }
                                            }
                                            if (bfList.Any(x => !x.IsPresent))
                                                throw new Exception("ADULTS AND KIDS - DIFFERENT FARE FAMILIES. ISSUE MANUALLY");
                                        }
                                    }
                                    strHistFareRS = SendCommandCryptically(ttAA, "FR");
                                    strEndTransaction = "<PNR_AddMultiElements><pnrActions><optionCode>11</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";
                                    SendAddMultiElements(ttAA, strEndTransaction);
                                }

                                var tktDesTooLong = false;
                                foreach (var ff in ffList)
                                {
                                    strHistFareRS = SendCommandCryptically(ttAA, $"FXP{strFareType}{ff.Item2}");
                                    if (strHistFareRS.Contains("TICKET DESIGNATOR TOO LONG TO PROCESS"))
                                    {
                                        var fxOpt = System.Text.RegularExpressions.Regex.Replace(ff.Item2, @"\/ZO-0\*[A-Z0-9.,]*", "");
                                        strHistFareRS = SendCommandCryptically(ttAA, $"FXP{strFareType}{fxOpt}");
                                        tktDesTooLong = true;
                                    }
                                }
                                strEndTransaction = "<PNR_AddMultiElements><pnrActions><optionCode>11</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";
                                SendAddMultiElements(ttAA, strEndTransaction);

                                if (tktDesTooLong)
                                {
                                    strResponseTST = SendDisplayTST(ttAA);
                                    var nTSTDoc = new XmlDocument();
                                    nTSTDoc.LoadXml(strResponseTST);
                                    var nRootStored = nTSTDoc.DocumentElement;

                                    XmlDocument pnr = new XmlDocument();
                                    pnr.LoadXml(strPNRReplay);
                                    XmlNodeList flightSegs = pnr.DocumentElement.SelectNodes("//originDestinationDetails/itineraryInfo");
                                    Dictionary<string, string> flSegMap = new Dictionary<string, string>();
                                    foreach (XmlNode fnode in flightSegs)
                                    {
                                        flSegMap.Add(fnode.SelectSingleNode("elementManagementItinerary/reference/number").InnerText,
                                            fnode.SelectSingleNode("elementManagementItinerary/lineNumber").InnerText);
                                    }

                                    foreach (XmlNode reqNode in oRootReq.SelectNodes("//StoredFare"))
                                    {
                                        var tstRPH = reqNode.Attributes["RPH"].Value;
                                        var paxType = oRootStored?.SelectSingleNode(
                                            $"fareList[fareReference/uniqueReference = '{tstRPH}']/paxSegReference/refDetails/refQualifier").InnerText;
                                        var paxNum = oRootStored?.SelectSingleNode(
                                            $"fareList[fareReference/uniqueReference = '{tstRPH}']/paxSegReference/refDetails/refNumber").InnerText;

                                        var nTstRPH = nRootStored?.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxNum}' and paxSegReference/refDetails/refQualifier = '{paxType}']/fareReference/uniqueReference").InnerText;
                                        var oSegmentNodes = nRootStored?.SelectNodes($"fareList[fareReference/uniqueReference = '{nTstRPH}']/segmentInformation");
                                        var tktNode = oSegmentNodes[0].SelectSingleNode("fareQualifier/fareBasisDetails/ticketDesignator");
                                        var ticketDes = reqNode.SelectSingleNode($"FareSegments/AirSegments").Attributes["TicketDesignator"] != null ?
                                            reqNode.SelectSingleNode($"FareSegments/AirSegments").Attributes["TicketDesignator"].Value : string.Empty;
                                        var farebasis = $"{oSegmentNodes[0].SelectSingleNode("fareQualifier/fareBasisDetails/primaryCode").InnerText}{oSegmentNodes[0].SelectSingleNode("fareQualifier/fareBasisDetails/fareBasisCode").InnerText}";
                                        var fbSegList = string.Empty;
                                        string updRequest;
                                        string strUpdTicketDes;
                                        foreach (XmlNode segNode in oSegmentNodes)
                                        {
                                            var segNum = segNode.SelectSingleNode("segmentReference/refDetails[refQualifier='S']/refNumber").InnerText;
                                            var segTD = string.Empty;
                                            if (reqNode.SelectSingleNode($"FareSegments/AirSegments[@RPH='{flSegMap[segNum]}']").Attributes["TicketDesignator"] != null)
                                            {
                                                segTD = reqNode.SelectSingleNode($"FareSegments/AirSegments[@RPH='{flSegMap[segNum]}']").Attributes["TicketDesignator"].Value;
                                            }
                                            else
                                                continue;
                                            //if (tktNode != null && tktNode.InnerText.Equals(ticketDes))
                                            //    continue;

                                            if (segNode.SelectSingleNode("fareQualifier") == null)
                                                continue;
                                            var segFB = $"{segNode.SelectSingleNode("fareQualifier/fareBasisDetails/primaryCode").InnerText}{segNode.SelectSingleNode("fareQualifier/fareBasisDetails/fareBasisCode").InnerText}";
                                            if (farebasis.Equals(segFB) && ticketDes.Equals(segTD))
                                                fbSegList += $",{segNode.SelectSingleNode("sequenceInformation/sequenceSection/sequenceNumber").InnerText}";
                                            else
                                            {
                                                updRequest = $"TTI/T{nTstRPH}/L{fbSegList.TrimStart(',')}{(fbSegList.TrimStart(',').Contains(",") ? "x" : string.Empty)}/B{farebasis} {ticketDes}";
                                                strUpdTicketDes = SendCommandCryptically(ttAA, updRequest);
                                                farebasis = segFB;
                                                ticketDes = segTD;
                                                fbSegList = $",{segNode.SelectSingleNode("sequenceInformation/sequenceSection/sequenceNumber").InnerText}";
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(fbSegList))
                                        {
                                            updRequest = $"TTI/T{nTstRPH}/L{fbSegList.TrimStart(',')}{(fbSegList.TrimStart(',').Contains(",") ? "x" : string.Empty)}/B{farebasis} {ticketDes}";
                                            strUpdTicketDes = SendCommandCryptically(ttAA, updRequest);
                                        }
                                    }
                                    strEndTransaction = "<PNR_AddMultiElements><pnrActions><optionCode>11</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";
                                    SendAddMultiElements(ttAA, strEndTransaction);
                                }

                                strResponseTST = SendDisplayTST(ttAA);
                                strResponseReprice = $"<strResponseReprice>{strResponseTST}</strResponseReprice>";
                            }
                        }
                        #endregion

                        else if (Request.Contains("StoreHistoricalFare") && !bStoreFare)
                        {
                            // Collect TST and passenger RPH references
                            var oTSTDoc = new XmlDocument();
                            oTSTDoc.LoadXml(strResponseTST);
                            var oTSTnodes = oTSTDoc.DocumentElement?.SelectNodes("fareList");
                            if (oTSTnodes != null)
                                foreach (XmlNode tst in oTSTnodes)
                                {
                                    var paxRPH = int.Parse(tst.SelectSingleNode("paxSegReference/refDetails/refNumber")?.InnerText);
                                    var paxTST = int.Parse(tst.SelectSingleNode("fareReference/uniqueReference")?.InnerText);
                                    var pn = oRootReq.SelectSingleNode($"StoredFare[@RPH='{paxTST}']");
                                    paxAssoc.Add(new Tuple<string, int>(pn.SelectSingleNode("PassengerType/@Code")?.InnerText, paxTST), paxRPH);
                                }

                            oRootStored = oTSTDoc.DocumentElement;
                            //TODO: fix this
                            var oNode1 = oRootReq.SelectNodes("StoredFare").Item(0);

                            if (oRootStored != null)
                            {
                                var sXmlFareFamily = GetPricingOptionsFF_FB(Request, strPNRReplay, oRootStored);

                                if (sXmlFareFamily.All(x => string.IsNullOrEmpty(x)))
                                {
                                    sXmlFareFamily = GetPricingOptions(Request, GetPricingOptionsTST, strPNRReplay, oRootStored);
                                }

                                string strDiscount = "";
                                string strTktDes = "";
                                strFareType = "RP";
                                string discQualif = "707"; // amont discount
                                string strZap = string.Empty;

                                if (oNode1.SelectSingleNode("Discount/@Amount") != null)
                                {
                                    strDiscount = oNode1.SelectSingleNode("Discount/@Amount").InnerXml;
                                }
                                else if (oNode1.SelectSingleNode("Discount/@Percent") != null)
                                {
                                    strDiscount = oNode1.SelectSingleNode("Discount/@Percent").InnerXml;

                                    if (strDiscount.Contains("."))
                                    {
                                        strDiscount = strDiscount.Substring(0, strDiscount.IndexOf("."));
                                    }

                                    discQualif = "708"; // percent discount
                                }

                                //if (oNode1.SelectSingleNode("TicketDesignator") != null)
                                //{
                                //    strTktDes = $"<rate>{oNode1.SelectSingleNode("TicketDesignator").InnerXml}</rate>";
                                //}

                                if (oNode1.Attributes["FareType"] != null)
                                {
                                    if (oNode1.Attributes["FareType"].Value == "Private")
                                        strFareType = "RU";
                                }

                                if (strDiscount != "" || strTktDes != "")
                                {
                                    strZap = $"<pricingOptionGroup><pricingOptionKey><pricingOptionKey>ZAP</pricingOptionKey></pricingOptionKey><penDisInformation><discountPenaltyQualifier>ZAP</discountPenaltyQualifier><discountPenaltyDetails><function>700</function><amountType>{discQualif}</amountType><amount>{strDiscount}</amount>{strTktDes}</discountPenaltyDetails></penDisInformation></pricingOptionGroup>";
                                }

                                foreach (var xmlFareFamily in sXmlFareFamily)
                                {
                                    string strRepriceRQ = $"<Fare_PricePNRWithBookingClass>{xmlFareFamily}<pricingOptionGroup><pricingOptionKey><pricingOptionKey>RLO</pricingOptionKey></pricingOptionKey></pricingOptionGroup>{(xmlFareFamily.Contains($">{strFareType}<") ? "" : $"<pricingOptionGroup><pricingOptionKey><pricingOptionKey>{strFareType}</pricingOptionKey></pricingOptionKey></pricingOptionGroup>")}{strZap}</Fare_PricePNRWithBookingClass>";
                                    strResponseReprice += FilterPricePNRWithBookingClassResponseByPax(SendPricePNRWithBookingClass(ttAA, strRepriceRQ), xmlFareFamily);
                                }
                                //string strAddress =$"http://webservices.amadeus.com/{ttProviderSystems.Profile}/{ttProviderSystems.AmadeusWSSchema[Fare_PricePNRWithBookingClass]}";
                                //strResponseReprice = strResponse =ttAA.SendMessage(strRepriceRQ, "", strAddress, conversationID);
                                strResponseReprice = strResponseReprice.Replace(@"</Fare_PricePNRWithBookingClassReply><Fare_PricePNRWithBookingClassReply>", "");
                                strResponseReprice = strResponseReprice.Replace($" xmlns=\"http://xml.amadeus.com/{ttProviderSystems.AmadeusWSSchema[Fare_PricePNRWithLowerFaresReply]}\"", "");
                                strResponseReprice = strResponseReprice.Replace($" xmlns=\"http://xml.amadeus.com/{ttProviderSystems.AmadeusWSSchema[Fare_PricePNRWithBookingClassReply]}\"", "");
                                //conversationID = UpdateSessionID(conversationID);
                            }
                        }

                        if (bMarkup)
                        {
                            #region MarkUp
                            // for each markup element 
                            //      get TST number and markup amount
                            //      get current base fare
                            //      calculate new base fare = current base fare + markup
                            //      build the TTK entry: TTK/Tn/Unnn.nn whewre n is TST number and nnn.nn is new base fare 
                            //      send TTK cryptic entry
                            // retrieve all TSTs again to collect repriced values
                            // if bStoreFare
                            //      end transact PNR

                            oDocStored = new XmlDocument();
                            oDocStored.LoadXml(strResponseTST);
                            oRootStored = oDocStored.DocumentElement;

                            foreach (XmlNode oNode in oRootReq.SelectNodes("StoredFare[Markup]"))
                            {
                                string updRequest = string.Empty;
                                string tstRPH = oNode.SelectSingleNode("@RPH").InnerText;
                                int iTSTRPH = Convert.ToInt16(tstRPH);
                                string strBf = string.Empty;
                                string strTf = string.Empty;
                                string fareCurrency = string.Empty;
                                if (paxAssoc.ContainsKey(new Tuple<string, int>(oNode.SelectSingleNode("PassengerType/@Code")?.InnerText, iTSTRPH)))
                                {
                                    var paxRPH = paxAssoc[new Tuple<string, int>(oNode.SelectSingleNode("PassengerType/@Code")?.InnerText, iTSTRPH)];
                                    if (oRootStored != null)
                                    {
                                        if (oNode.SelectSingleNode("PassengerType/@Code").InnerText.Equals("INF"))
                                        {
                                            strBf = oRootStored.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and statusInformation/firstStatusDetails/tstFlag = 'INF']/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount")?.InnerText;
                                            strTf = oRootStored.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and statusInformation/firstStatusDetails/tstFlag = 'INF']/fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount")?.InnerText;
                                            fareCurrency = oRootStored.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and statusInformation/firstStatusDetails/tstFlag = 'INF']/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency")?.InnerText;
                                            tstRPH = oRootStored.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and statusInformation/firstStatusDetails/tstFlag = 'INF']/fareReference/uniqueReference")?.InnerText;
                                        }
                                        else
                                        {
                                            strBf = oRootStored.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and statusInformation/firstStatusDetails/tstFlag != 'INF']/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount")?.InnerText;
                                            strTf = oRootStored.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and statusInformation/firstStatusDetails/tstFlag != 'INF']/fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount")?.InnerText;
                                            fareCurrency = oRootStored.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and statusInformation/firstStatusDetails/tstFlag != 'INF']/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency")?.InnerText;
                                            tstRPH = oRootStored.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and statusInformation/firstStatusDetails/tstFlag != 'INF']/fareReference/uniqueReference")?.InnerText;
                                        }
                                    }
                                }
                                else
                                {
                                    //iTSTRPH += iTSTCount;
                                    tstRPH = iTSTRPH.ToString(CultureInfo.InvariantCulture);
                                    if (oRootStored != null)
                                    {
                                        strBf = oRootStored.SelectSingleNode($"fareList[fareReference/uniqueReference = '{tstRPH}']/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount")?.InnerText;
                                        strTf = oRootStored.SelectSingleNode($"fareList[fareReference/uniqueReference = '{tstRPH}']/fareDataInformation/fareDataSupInformation[fareDataQualifier = '712']/fareAmount")?.InnerText;
                                        fareCurrency = oRootStored.SelectSingleNode($"fareList[fareReference/uniqueReference = '{tstRPH}']/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency")?.InnerText;
                                    }
                                }
                                var culture = new CultureInfo("en-US");
                                decimal decMarkup = Convert.ToDecimal(oNode.SelectSingleNode("Markup/@Amount").InnerText, culture);
                                decimal oldBF = Convert.ToDecimal(strBf, culture);
                                decimal oldTF = Convert.ToDecimal(strTf, culture);
                                string newBF = Convert.ToString(oldBF + decMarkup);
                                string newTF = Convert.ToString(oldTF + decMarkup);

                                if (fareCurrency != "USD")
                                {
                                    int decPoints;
                                    if (strBf.Contains("."))
                                    {
                                        decPoints = strBf.Trim().Length - (strBf.IndexOf(".", StringComparison.Ordinal) + 1);
                                    }
                                    else
                                    {
                                        decPoints = 0;
                                    }

                                    var tstNode = oRootStored.SelectSingleNode("fareList/otherPricingInfo/attributeDetails[attributeType='FCA']/attributeDescription");
                                    var fxxData = tstNode.InnerText.Split(new[] { '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                    var strROE = fxxData.Exists(s => s.StartsWith("ROE")) ? fxxData.Find(s => s.StartsWith("ROE")).Substring(3) : "1.0";
                                    decimal roe = decimal.Parse(strROE);
                                    decimal oldBFusd = Convert.ToDecimal(oRootStored.SelectSingleNode($"fareList[fareReference/uniqueReference = '{tstRPH}']/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount").InnerText, culture);

                                    // Bug 624 - T-Robot - foreign Currency - no mark added
                                    //AS: Added TST assoctiatin to TTK command
                                    updRequest = $"TTK/T{tstRPH}/I{fareCurrency}{Math.Round(oldBF + decMarkup * roe, decPoints)}/EUSD{oldBFusd + decMarkup:0.00}";
                                    //updRequest = String.Format("TTK/I{0}/EUSD{1}", fareCurrency + Convert.ToString(Math.Round(oldBF + decMarkup, decPoints)), Convert.ToString(oldBFusd + Math.Round((decMarkup / roe), 2)));

                                    /*************************
                                    var BSR = Convert.ToDecimal(oRootStored.SelectSingleNode("fareList/bankerRates/firstRateDetail/amount").InnerText, culture);
                                    var newBFusd = Convert.ToString(Math.Round(((oldBF + decMarkup) * BSR), 2));
                                    updRequest = String.Format("TTK/I{0}/EUSD{1}", fareCurrency + newBF, newBFusd);
                                     *************************/
                                }
                                else
                                { updRequest = $"TTK/T{tstRPH}/U{newBF}"; }

                                SendCommandCryptically(ttAA, updRequest);
                                if (bStoreFare)
                                {
                                    var saveCMDresp = SendCommandCryptically(ttAA, "RFTRIPXML;ER");
                                    if (saveCMDresp.Contains("textStringDetails") && saveCMDresp.Contains("WARNING"))
                                        saveCMDresp = SendCommandCryptically(ttAA, "ER");
                                }
                            }

                            if (bStoreFare)
                            {
                                string strEndTransaction = "<PNR_AddMultiElements><pnrActions><optionCode>11</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";
                                SendAddMultiElements(ttAA, strEndTransaction);
                            }
                            strResponseReprice = $"<strResponseReprice>{SendDisplayTST(ttAA)}</strResponseReprice>";
                            #endregion
                            #region oldCode
                            //    int iTSTCount = 0;

                            //    var paxAssoc = new Dictionary<Tuple<string, int>, int>();
                            //    if (Request.Contains("StoreHistoricalFare=\"true\""))
                            //    {
                            //        var oDocReq = new XmlDocument();
                            //        oDocReq.LoadXml(Request);
                            //        XmlElement oRootReq = oDocReq.DocumentElement;

                            //        // Collect TST and passenger RPH references
                            //        var oTSTDoc = new XmlDocument();
                            //        oTSTDoc.LoadXml(strResponseTST);
                            //        var oTSTnodes = oTSTDoc.DocumentElement?.SelectNodes("fareList");
                            //        if (oTSTnodes != null)
                            //            foreach (XmlNode tst in oTSTnodes)
                            //            {
                            //                var paxRPH = int.Parse(tst.SelectSingleNode("paxSegReference/refDetails/refNumber")?.InnerText);
                            //                var paxTST = int.Parse(tst.SelectSingleNode("fareReference/uniqueReference")?.InnerText);
                            //                var pn = oRootReq.SelectSingleNode($"StoredFare[@RPH='{paxTST}']");
                            //                paxAssoc.Add(new Tuple<string, int>(pn.SelectSingleNode("PassengerType/@Code")?.InnerText, paxTST), paxRPH);
                            //            }

                            //        string strFareType = "";

                            //        if (oRootReq.SelectSingleNode("StoredFare").SelectSingleNode("@FareType") != null)
                            //        {
                            //            if (oRootReq.SelectSingleNode("StoredFare").SelectSingleNode("@FareType").InnerXml == "Private")
                            //                strFareType = "/R,U";
                            //        }

                            //        iTSTCount = oRootReq.SelectNodes("StoredFare").Count;

                            //        string strHistFareRS = SendCommandCryptically(ttAA, $"FXX{strFareType}");
                            //        strHistFareRS = SendCommandCryptically(ttAA, "FR");
                            //        strHistFareRS = SendCommandCryptically(ttAA, "RFTRIPXML;ER");
                            //        strHistFareRS = SendCommandCryptically(ttAA, $"FXP{strFareType}");
                            //        strHistFareRS = SendCommandCryptically(ttAA, "RFTRIPXML;ER");
                            //        strResponseTST = SendDisplayTST(ttAA);
                            //    }

                            //    if (bMarkup)
                            //    {
                            //        oDocStored = new XmlDocument();
                            //        oDocStored.LoadXml(strResponseTST);
                            //        oRootStored = oDocStored.DocumentElement;

                            //        XmlElement oRoot;
                            //        var oDoc = new XmlDocument();
                            //        oDoc.LoadXml(Request);
                            //        oRoot = oDoc.DocumentElement;

                            //        foreach (XmlNode oNode in oRoot.SelectNodes("StoredFare[Markup]"))
                            //        {
                            //            string updRequest = string.Empty;
                            //            string tstRPH = oNode.SelectSingleNode("@RPH").InnerText;
                            //            int iTSTRPH = Convert.ToInt16(tstRPH);
                            //            string strBf = string.Empty;
                            //            string fareCurrency = string.Empty;
                            //            if (paxAssoc.ContainsKey(new Tuple<string, int>(oNode.SelectSingleNode("PassengerType/@Code")?.InnerText, iTSTRPH)))
                            //            {
                            //                var paxRPH = paxAssoc[new Tuple<string, int>(oNode.SelectSingleNode("PassengerType/@Code")?.InnerText, iTSTRPH)];
                            //                if (oRootStored != null)
                            //                {
                            //                    if (oNode.SelectSingleNode("PassengerType/@Code").InnerText.Equals("INF"))
                            //                    {
                            //                        strBf = oRootStored.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and statusInformation/firstStatusDetails/tstFlag = 'INF']/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount")?.InnerText;
                            //                        fareCurrency = oRootStored.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and statusInformation/firstStatusDetails/tstFlag = 'INF']/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency")?.InnerText;
                            //                        tstRPH = oRootStored.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and statusInformation/firstStatusDetails/tstFlag = 'INF']/fareReference/uniqueReference")?.InnerText;
                            //                    }
                            //                    else
                            //                    {
                            //                        strBf = oRootStored.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and statusInformation/firstStatusDetails/tstFlag != 'INF']/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount")?.InnerText;
                            //                        fareCurrency = oRootStored.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and statusInformation/firstStatusDetails/tstFlag != 'INF']/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency")?.InnerText;
                            //                        tstRPH = oRootStored.SelectSingleNode($"fareList[paxSegReference/refDetails/refNumber = '{paxRPH}'  and statusInformation/firstStatusDetails/tstFlag != 'INF']/fareReference/uniqueReference")?.InnerText;
                            //                    }
                            //                }
                            //            }
                            //            else
                            //            {
                            //                iTSTRPH += iTSTCount;
                            //                tstRPH = iTSTRPH.ToString(CultureInfo.InvariantCulture);
                            //                if (oRootStored != null)
                            //                {
                            //                    strBf = oRootStored.SelectSingleNode($"fareList[fareReference/uniqueReference = '{tstRPH}']/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareAmount")?.InnerText;
                            //                    fareCurrency = oRootStored.SelectSingleNode($"fareList[fareReference/uniqueReference = '{tstRPH}']/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'B']/fareCurrency")?.InnerText;
                            //                }
                            //            }
                            //            var culture = new CultureInfo("en-US");
                            //            decimal decMarkup = Convert.ToDecimal(oNode.SelectSingleNode("Markup/@Amount").InnerText, culture);
                            //            decimal oldBF = Convert.ToDecimal(strBf, culture);
                            //            string newBF = Convert.ToString(oldBF + decMarkup);


                            //            if (fareCurrency != "USD")
                            //            {
                            //                int decPoints;
                            //                if (strBf.Contains("."))
                            //                {
                            //                    decPoints = strBf.Trim().Length - (strBf.IndexOf(".", StringComparison.Ordinal) + 1);
                            //                }
                            //                else
                            //                {
                            //                    decPoints = 0;
                            //                }

                            //                var tstNode = oRootStored.SelectSingleNode("fareList/otherPricingInfo/attributeDetails[attributeType='FCA']/attributeDescription");
                            //                var fxxData = tstNode.InnerText.Split(new[] { '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                            //                var strROE = fxxData.Exists(s => s.StartsWith("ROE")) ? fxxData.Find(s => s.StartsWith("ROE")).Substring(3) : "1.0";
                            //                decimal roe = decimal.Parse(strROE);
                            //                decimal oldBFusd = Convert.ToDecimal(oRootStored.SelectSingleNode($"fareList[fareReference/uniqueReference = '{tstRPH}']/fareDataInformation/fareDataSupInformation[fareDataQualifier = 'E']/fareAmount").InnerText, culture);

                            //                // Bug 624 - T-Robot - foreign Currency - no mark added
                            //                //AS: Added TST assoctiatin to TTK command
                            //                updRequest = $"TTK/T{tstRPH}/I{fareCurrency}{Math.Round(oldBF + decMarkup * roe, decPoints)}/EUSD{oldBFusd + decMarkup:0.00}";
                            //                //updRequest = String.Format("TTK/I{0}/EUSD{1}", fareCurrency + Convert.ToString(Math.Round(oldBF + decMarkup, decPoints)), Convert.ToString(oldBFusd + Math.Round((decMarkup / roe), 2)));

                            //                /*************************
                            //                var BSR = Convert.ToDecimal(oRootStored.SelectSingleNode("fareList/bankerRates/firstRateDetail/amount").InnerText, culture);
                            //                var newBFusd = Convert.ToString(Math.Round(((oldBF + decMarkup) * BSR), 2));
                            //                updRequest = String.Format("TTK/I{0}/EUSD{1}", fareCurrency + newBF, newBFusd);
                            //                 *************************/
                            //            }
                            //            else
                            //            { updRequest = $"TTK/T{tstRPH}/U{newBF}"; }

                            //            SendCommandCryptically(ttAA, updRequest);
                            //        }

                            //        strResponseReprice = $"<strResponseReprice>{SendDisplayTST(ttAA)}</strResponseReprice>";
                            //        if (bStoreFare)
                            //        {
                            //            string strEndTransaction = "<PNR_AddMultiElements><pnrActions><optionCode>11</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";
                            //            SendAddMultiElements(ttAA, strEndTransaction);
                            //        }
                            //    }
                            //    else if (Request.Contains("StoredFare"))
                            //    {
                            //        var oDocReq = new XmlDocument();
                            //        oDocReq.LoadXml(Request);
                            //        XmlElement oRootReq = oDocReq.DocumentElement;

                            //        oDocStored = new XmlDocument();
                            //        oDocStored.LoadXml(strResponseTST);
                            //        oRootStored = oDocStored.DocumentElement;

                            //        if ((oRootReq.SelectSingleNode("StoredFare/Discount") == null) && (oRootReq.SelectSingleNode("StoredFare/TicketDesignator") == null))
                            //        {
                            //            string strFXX = (bPrivate)
                            //                ? "<Fare_PricePNRWithBookingClass><overrideInformation><attributeDetails><attributeType>RLO</attributeType></attributeDetails><attributeDetails><attributeType>RU</attributeType></attributeDetails></overrideInformation></Fare_PricePNRWithBookingClass>"
                            //                : "<Fare_PricePNRWithBookingClass><overrideInformation><attributeDetails><attributeType>RLO</attributeType></attributeDetails><attributeDetails><attributeType>RP</attributeType></attributeDetails></overrideInformation></Fare_PricePNRWithBookingClass>";

                            //            strResponseReprice = SendPricePNRWithBookingClass(ttAA, strFXX);
                            //        }
                            //        else
                            //        {

                            //            foreach (XmlNode oNode in oRootReq.SelectNodes("StoredFare"))
                            //            {
                            //                string tstRPH = oNode.SelectSingleNode("@RPH").InnerText;

                            //                int iTSTRPH = Convert.ToInt16(tstRPH);
                            //                iTSTRPH += iTSTCount;

                            //                tstRPH = iTSTRPH.ToString(CultureInfo.InvariantCulture);

                            //                string strDiscount = "";
                            //                string strTktDes = "";
                            //                string strPax = "";
                            //                string strFareType = "RP";
                            //                string discQualif = "707"; // amont discount
                            //                string strZap = string.Empty;

                            //                if (oNode.SelectSingleNode("Discount/@Amount") != null)
                            //                {
                            //                    strDiscount = oNode.SelectSingleNode("Discount/@Amount").InnerXml;
                            //                }
                            //                else if (oNode.SelectSingleNode("Discount/@Percent") != null)
                            //                {
                            //                    strDiscount = oNode.SelectSingleNode("Discount/@Percent").InnerXml;

                            //                    if (strDiscount.Contains("."))
                            //                    {
                            //                        strDiscount = strDiscount.Substring(0, strDiscount.IndexOf("."));
                            //                    }

                            //                    discQualif = "708"; // percent discount
                            //                }

                            //                if (oNode.SelectSingleNode("TicketDesignator") != null)
                            //                {
                            //                    strTktDes = $"<discountCode>{oNode.SelectSingleNode("TicketDesignator").InnerXml}</discountCode>";
                            //                }

                            //                if (oNode.SelectSingleNode("@FareType") != null)
                            //                {
                            //                    if (oNode.SelectSingleNode("@FareType").InnerXml == "Private")
                            //                        strFareType = "RU";
                            //                }

                            //                if (strDiscount != "" || strTktDes != "")
                            //                {
                            //                    strZap = "<discountInformation><penDisInformation><infoQualifier>ZAP</infoQualifier><penDisData><penaltyType>700</penaltyType><penaltyQualifier>{0}</penaltyQualifier><penaltyAmount>{1}</penaltyAmount>{2}</penDisData></penDisInformation></discountInformation>";
                            //                    strZap = string.Format(strZap, discQualif, strDiscount, strTktDes);

                            //                }

                            //                strPax = oRootStored.SelectSingleNode("fareList[fareReference/uniqueReference = '" + tstRPH + "']/paxSegReference").OuterXml;
                            //                XmlNode oTSTpax = oRootStored.SelectSingleNode("fareList[fareReference/uniqueReference = '" + tstRPH + "']/statusInformation/firstStatusDetails/tstFlag");
                            //                string strTSTpax = "";

                            //                if (oTSTpax != null)
                            //                    strTSTpax = oTSTpax.InnerXml;

                            //                if (strPax.Contains("<refQualifier>PT</refQualifier>") && strTSTpax == "INF")
                            //                {
                            //                    strPax = strPax.Replace("<refQualifier>PT</refQualifier>", "<refQualifier>PI</refQualifier>");
                            //                }
                            //                else
                            //                {
                            //                    strPax = strPax.Replace("<refQualifier>PT</refQualifier>", "<refQualifier>PA</refQualifier>");
                            //                }

                            //                string strRepriceRQ = $"<Fare_PricePNRWithBookingClass>{strPax}<overrideInformation><attributeDetails><attributeType>RLO</attributeType></attributeDetails><attributeDetails><attributeType>{strFareType}</attributeType></attributeDetails></overrideInformation>{strZap}</Fare_PricePNRWithBookingClass>";
                            //                strResponse = SendPricePNRWithBookingClass(ttAA, strRepriceRQ);

                            //                bool ticketDesignatorTooLong = false;
                            //                if (strResponse.Contains("TICKET DESIGNATOR TOO LONG TO PROCESS"))
                            //                {
                            //                    ticketDesignatorTooLong = true;

                            //                    strRepriceRQ = $"<Fare_PricePNRWithBookingClass>{strPax}<overrideInformation><attributeDetails><attributeType>RLO</attributeType></attributeDetails><attributeDetails><attributeType>{strFareType}</attributeType></attributeDetails></overrideInformation></Fare_PricePNRWithBookingClass>";
                            //                    strResponse = SendPricePNRWithBookingClass(ttAA, strRepriceRQ);
                            //                }

                            //                strResponseReprice += strResponse.Replace(" xmlns=\"http://xml.amadeus.com/" + ttProviderSystems.AmadeusWSSchema[Fare_PricePNRWithBookingClassReply] + "\"", "");

                            //                XmlElement oRootTst = null;
                            //                if (bStoreFare)
                            //                {
                            //                    string strStorePrice = "<Ticket_CreateTSTFromPricing><psaList><itemReference><referenceType>TST</referenceType><uniqueReference>1</uniqueReference></itemReference></psaList></Ticket_CreateTSTFromPricing>";

                            //                    string strStorePriceRS = SendCreateTSTFromPricing(ttAA, strStorePrice);

                            //                    if (ticketDesignatorTooLong)
                            //                    {
                            //                        var oDocNewTst = new XmlDocument();
                            //                        oDocNewTst.LoadXml(strStorePriceRS);
                            //                        oRootTst = oDocNewTst.DocumentElement;
                            //                    }
                            //                }

                            //                if (ticketDesignatorTooLong)
                            //                {
                            //                    var oSegmentNodes = oRootStored?.SelectNodes($"fareList[fareReference/uniqueReference = '{tstRPH}']/segmentInformation");
                            //                    var farebasis = $"{oSegmentNodes[0].SelectSingleNode("fareQualifier/fareBasisDetails/primaryCode").InnerText}{oSegmentNodes[0].SelectSingleNode("fareQualifier/fareBasisDetails/fareBasisCode").InnerText}";
                            //                    var fbSegList = string.Empty;
                            //                    var ticketDes = oNode.SelectSingleNode("TicketDesignator").InnerXml;
                            //                    string updRequest;
                            //                    string strUpdTicketDes;
                            //                    if (bStoreFare)
                            //                        tstRPH = oRootTst.SelectSingleNode("tstList/tstReference/uniqueReference").InnerText;
                            //                    foreach (XmlNode segNode in oSegmentNodes)
                            //                    {
                            //                        if (segNode.SelectSingleNode("fareQualifier") == null)
                            //                            continue;
                            //                        var segFB = $"{segNode.SelectSingleNode("fareQualifier/fareBasisDetails/primaryCode").InnerText}{segNode.SelectSingleNode("fareQualifier/fareBasisDetails/fareBasisCode").InnerText}";
                            //                        if (farebasis.Equals(segFB))
                            //                            fbSegList += $",{segNode.SelectSingleNode("sequenceInformation/sequenceSection/sequenceNumber").InnerText}";
                            //                        else
                            //                        {
                            //                            updRequest = $"TTI/T{tstRPH}/L{fbSegList.Substring(1)}{(fbSegList.Substring(1).Contains(",") ? "x" : string.Empty)}/B{farebasis} {ticketDes}";
                            //                            strUpdTicketDes = SendCommandCryptically(ttAA, updRequest);
                            //                            farebasis = segFB;
                            //                            fbSegList = $",{segNode.SelectSingleNode("sequenceInformation/sequenceSection/sequenceNumber").InnerText}";
                            //                        }
                            //                    }

                            //                    updRequest = $"TTI/T{tstRPH}/L{fbSegList.Substring(1)}{(fbSegList.Substring(1).Contains(",") ? "x" : string.Empty)}/B{farebasis} {ticketDes}";
                            //                    strUpdTicketDes = SendCommandCryptically(ttAA, updRequest);
                            //                    var strTicketDesRS = SendCommandCryptically(ttAA, "RFTRIPXML;ER");

                            //                    if (strTicketDesRS.Contains("textStringDetails") && strTicketDesRS.Contains("WARNING"))
                            //                    {
                            //                        strTicketDesRS = SendCommandCryptically(ttAA, "RFTRIPXML;ER");
                            //                    }
                            //                }
                            //            }
                            //            strResponseReprice = strResponseReprice.Replace("<Fare_PricePNRWithBookingClassReply>", "").Replace("</Fare_PricePNRWithBookingClassReply>", "");
                            //            strResponseReprice = $"<Fare_PricePNRWithBookingClassReply>{strResponseReprice}</Fare_PricePNRWithBookingClassReply>";
                            //        }

                            //        if (bStoreFare)
                            //        {
                            //            strResponseReprice = $"<strResponseReprice>{SendDisplayTST(ttAA)}</strResponseReprice>";

                            //            string strEndTransaction = "<PNR_AddMultiElements><pnrActions><optionCode>11</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";
                            //            SendAddMultiElements(ttAA, strEndTransaction);
                            //        }
                            #endregion
                        }
                        #region Old Code
                        //else
                        //{
                        //    #region No Stored Fares
                        //    if (bStoreFare)
                        //    {
                        //        oDocStored = new XmlDocument();
                        //        oDocStored.LoadXml(strResponseTST);
                        //        oRootStored = oDocStored.DocumentElement;
                        //        string strRPRU = ">RP<";
                        //        string strVC = "";
                        //        if (oRootStored.SelectSingleNode("fareList[1]/pricingInformation/tstInformation/tstIndicator").InnerText == "B")
                        //            strRPRU = ">RU<";
                        //        else if (oRootStored.SelectSingleNode("fareList[1]/pricingInformation/tstInformation/tstIndicator").InnerText == "F")
                        //            strRPRU = ">RU<";
                        //        else if (oRootStored.SelectSingleNode("fareList[1]/pricingInformation/tstInformation/tstIndicator").InnerText == "G")
                        //            strRPRU = ">RU<";
                        //        else if (oRootStored.SelectSingleNode("fareList[1]/pricingInformation/fcmi").InnerText == "F")
                        //            strRPRU = ">RU<";
                        //        else if (oRootStored.SelectSingleNode("fareList[1]/pricingInformation/fcmi").InnerText == "I")
                        //            strRPRU = ">RU<";
                        //        else if (oRootStored.SelectSingleNode("fareList[1]/pricingInformation/fcmi").InnerText == "M")
                        //            strRPRU = ">RU<";
                        //        else if (oRootStored.SelectSingleNode("fareList[1]/pricingInformation/fcmi").InnerText == "N")
                        //            strRPRU = ">RU<";
                        //        else if (oRootStored.SelectSingleNode("fareList[1]/pricingInformation/fcmi").InnerText == "R")
                        //            strRPRU = ">RU<";
                        //        else if (oRootStored.SelectSingleNode("fareList[1]/pricingInformation/fcmi").InnerText == "G")
                        //            strRPRU = ">RU<";
                        //        if (oRootStored.SelectSingleNode("fareList[1]/validatingCarrier/carrierInformation/carrierCode") != null)
                        //        {
                        //            strVC = "<validatingCarrier><carrierInformation><carrierCode>";
                        //            strVC += oRootStored.SelectSingleNode("fareList[1]/validatingCarrier/carrierInformation/carrierCode").InnerText;
                        //            strVC += "</carrierCode></carrierInformation></validatingCarrier>";
                        //        }
                        //        string strPT = "";
                        //        if (Request.Contains("<FareNumber>"))
                        //        {
                        //            string strFN = oRootReq.SelectSingleNode("FareNumber").InnerText;
                        //            strPT = oRootStored.SelectSingleNode(
                        //                $"fareList[fareReference/uniqueReference = {strFN}]/paxSegReference[1]/refDetails[1]/refQualifier").InnerText;
                        //            strByFare = $"<paxSegReference><refDetails><refQualifier>{strPT}</refQualifier></refDetails></paxSegReference>";
                        //        }
                        //        if (Request.Contains("<Userid>Downtown</Userid>"))
                        //        {
                        //            SendDeleteTST(ttAA);
                        //        }
                        //        else
                        //        {
                        //            SendCommandCryptically(ttAA, "TTE/ALL");
                        //        }
                        //        string strRepriceRQ = $"<Fare_PricePNRWithBookingClass>{strByFare}<overrideInformation><attributeDetails><attributeType>RLO</attributeType></attributeDetails><attributeDetails><attributeType>RP</attributeType></attributeDetails></overrideInformation>{strVC}</Fare_PricePNRWithBookingClass>";
                        //        strRepriceRQ = strRepriceRQ.Replace(">RP<", strRPRU);
                        //        strResponseReprice = SendGDSMessage(ttAA, strRepriceRQ, ttProviderSystems.AmadeusWSSchema[Fare_PricePNRWithBookingClass], ttProviderSystems.AmadeusWSSchema[Fare_PricePNRWithBookingClassReply]);
                        //        // now we need to compare stored fare to repriced fare
                        //        // if they are different we need to store the new fare in PNR
                        //        XmlDocument oDocRepriced = new XmlDocument();
                        //        oDocRepriced.LoadXml(strResponseReprice);
                        //        XmlElement oRootRepriced = oDocRepriced.DocumentElement;
                        //        bool bPriceHasChanged = false;
                        //        int iPos = 1;
                        //        foreach (XmlNode oNodeRepriced in oRootRepriced.SelectNodes("fareList"))
                        //        {
                        //            if (oRootStored.SelectSingleNode($"fareList[position()={iPos}]") != null)
                        //            {
                        //                if (oNodeRepriced.SelectSingleNode("fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareAmount").InnerText !=
                        //                    oRootStored.SelectSingleNode($"fareList[position()={iPos}]/fareDataInformation/fareDataSupInformation[fareDataQualifier='712']/fareAmount").InnerText)
                        //                    bPriceHasChanged = true;
                        //                iPos += 1;
                        //            }
                        //            else
                        //            {
                        //                bPriceHasChanged = true;
                        //                break;
                        //            }
                        //        }
                        //        //if (bPriceHasChanged || !bPriceHasChanged)
                        //        //{
                        //        string strStorePrice = "<Ticket_CreateTSTFromPricing>";
                        //        int iFareList = 1;
                        //        foreach (XmlNode oNodeResps in oRootRepriced.SelectNodes("fareList"))
                        //        {
                        //            strStorePrice += $"<psaList><itemReference><referenceType>TST</referenceType><uniqueReference>{iFareList}</uniqueReference></itemReference></psaList>";
                        //            iFareList = iFareList + 1;
                        //        }
                        //        strStorePrice = $"{strStorePrice}</Ticket_CreateTSTFromPricing>";
                        //        string strStorePriceRS = SendCreateTSTFromPricing(ttAA, strStorePrice);
                        //        strResponseReprice = $"<strResponseReprice>{SendDisplayTST(ttAA)}</strResponseReprice>";
                        //        if (Request.Contains("StoreHistoricalFare=\"true\""))
                        //        {
                        //            string strPaxT = "";
                        //            if (strPT == "PI")
                        //                strPaxT = "/INF";
                        //            else if (strPT == "PA")
                        //                strPaxT = "/PAX";
                        //            string strHistFareRS = SendCommandCryptically(ttAA, $"FXX{strPaxT}");
                        //            strHistFareRS = SendCommandCryptically(ttAA, $"FR{strPaxT}");
                        //        }
                        //        //send end transact
                        //        string strEndTransaction;
                        //        strEndTransaction = !inSession ? "<PNR_AddMultiElements><pnrActions><optionCode>10</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>" : "<PNR_AddMultiElements><pnrActions><optionCode>11</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";
                        //        string strResponseET = SendAddMultiElements(ttAA, strEndTransaction);
                        //    }
                        //    else
                        //    {
                        //        //string strFXX = (bPrivate || bPublished)
                        //        //    ? "<Fare_PricePNRWithBookingClass><overrideInformation><attributeDetails><attributeType>RLO</attributeType></attributeDetails><attributeDetails><attributeType>RU</attributeType></attributeDetails><attributeDetails><attributeType>RP</attributeType></attributeDetails></overrideInformation></Fare_PricePNRWithBookingClass>"
                        //        //    : "<Fare_PricePNRWithLowerFares><overrideInformation><attributeDetails><attributeType>NOP</attributeType></attributeDetails></overrideInformation></Fare_PricePNRWithLowerFares>";
                        //        //strResponseReprice = (bPrivate || bPublished)
                        //        //    ? SendPricePNRWithBookingClass(ttAA, strFXX)
                        //        //    : SendPricePNRWithLowerFares(ttAA, strFXX);
                        //    }
                        //    #endregion
                        //}
                        #endregion

                    }
                    else
                    {
                        string strReprice = "<Fare_PricePNRWithBookingClass><overrideInformation><attributeDetails><attributeType>RLO</attributeType></attributeDetails><attributeDetails><attributeType>RU</attributeType></attributeDetails><attributeDetails><attributeType>RP</attributeType></attributeDetails></overrideInformation></Fare_PricePNRWithBookingClass>";
                        strResponseReprice = SendPricePNRWithBookingClass(ttAA, strReprice);

                        oDocStored = new XmlDocument();
                        oDocStored.LoadXml(strResponseReprice);
                        oRootStored = oDocStored.DocumentElement;
                        strResponse = CoreLib.TransformXML(strResponseReprice, XslPath, $"{Version}AmadeusWS_TB_Errors.xsl");

                        // Fatal Error 
                        if (!strResponse.Contains("<Error"))
                        {
                            string strStorePrice = "<Ticket_CreateTSTFromPricing>";
                            int iFareList = 1;

                            foreach (XmlNode oNodeResps in oRootStored.SelectNodes("fareList"))
                            {
                                strStorePrice += $"<psaList><itemReference><referenceType>TST</referenceType><uniqueReference>{iFareList}</uniqueReference></itemReference></psaList>";
                                iFareList = iFareList + 1;
                            }

                            strStorePrice += "</Ticket_CreateTSTFromPricing>";
                            strResponse = SendCreateTSTFromPricing(ttAA, strStorePrice);

                            strResponseReprice = $"<strResponseReprice>{SendDisplayTST(ttAA)}</strResponseReprice>";
                            string strEndTransaction = !inSession ? "<PNR_AddMultiElements><pnrActions><optionCode>10</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>" : "<PNR_AddMultiElements><pnrActions><optionCode>11</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";
                            strResponse = SendAddMultiElements(ttAA, strEndTransaction);
                        }
                    }
                }

                //*****************************************************************
                // Transform Native Amadeus PNRRead Response into OTA Response   *
                //***************************************************************** 
                try
                {
                    strPNRReplay = strPNRReplay.Replace("PNR_Reply", "PNR_RetrieveByRecLocReply");

                    if (inSession)
                        strPNRReplay = strPNRReplay.Replace("</PNR_RetrieveByRecLocReply>", $"{strResponseTST}{strResponseReprice}{Request}<ConversationID>{ConversationID}</ConversationID></PNR_RetrieveByRecLocReply>");

                    if (strPNRReplay.Length > 5500)
                    {
                        CoreLib.SendTrace(ttProviderSystems.UserID, "PNRReprice", "Final response I", strPNRReplay.Substring(0, (int)Math.Round(strPNRReplay.Length / 2d)), ttProviderSystems.LogUUID);
                        CoreLib.SendTrace(ttProviderSystems.UserID, "PNRReprice", "Final response II", strPNRReplay.Substring((int)Math.Round(strPNRReplay.Length / 2d)), ttProviderSystems.LogUUID);
                    }
                    else
                    {
                        CoreLib.SendTrace(ttProviderSystems.UserID, "PNRReprice", "Final response I", strPNRReplay, ttProviderSystems.LogUUID);
                    }

                    //CoreLib.SendTrace(ttProviderSystems.UserID, "PNRReprice", "Final response", strPNRReplay, ttProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strPNRReplay, XslPath, $"{Version}AmadeusWS_PNRRepriceRS.xsl");
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
            }
            catch (Exception exx)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRReprice, exx.Message, ttProviderSystems);
            }

            return strResponse;
        }

        private string FilterPricePNRWithBookingClassResponseByPax(string response, string request)
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

            var okTst = new List<string>();
            if (okPax.Count.Equals(0))// TST ref
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

        private List<Tuple<string, string>> GetFareFamilyFXX(string request, string pnrRead, XmlElement oRootStored)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(request);
            XmlElement root = doc.DocumentElement;

            var res = new List<Tuple<string, string>>();

            XmlNodeList nodes = root.SelectNodes("//StoredFare");

            XmlDocument pnr = new XmlDocument();
            pnr.LoadXml(pnrRead);
            XmlNodeList paxLines = pnr.DocumentElement.SelectNodes("//travellerInfo/elementManagementPassenger");
            Dictionary<string, string> paxMap = new Dictionary<string, string>();
            foreach (XmlNode fnode in paxLines)
            {
                paxMap.Add(fnode.SelectSingleNode("reference/number").InnerText,
                    fnode.SelectSingleNode("lineNumber").InnerText);
            }

            var segCount = pnr.DocumentElement
                .SelectNodes("//originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR']")
                .Count;

            List<Tuple<string, List<Tuple<string, string, string>>>> paxFareSegs =
                new List<Tuple<string, List<Tuple<string, string, string>>>>();
            foreach (XmlNode fnode in nodes)
            {
                var tst = fnode.Attributes["RPH"].Value;
                if (!paxFareSegs.Any(fs => fs.Item1 == tst))
                    paxFareSegs.Add(new Tuple<string, List<Tuple<string, string, string>>>(tst,
                            new List<Tuple<string, string, string>>()));
                foreach (XmlNode ffnode in fnode.SelectNodes("BrandedFares/FareFamily"))
                {
                    if (!paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Any(i =>
                        i.Item1 == ffnode.InnerText && i.Item2 == "S" && i.Item3 == ffnode.Attributes["RPH"].Value))
                        paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Add(
                            new Tuple<string, string, string>(ffnode.InnerText, "S", ffnode.Attributes["RPH"].Value));
                }

                foreach (XmlNode ffnode in fnode.SelectNodes("FareSegments/AirSegments"))
                {
                    if (ffnode.Attributes["TicketDesignator"] != null && !string.IsNullOrEmpty(ffnode.Attributes["TicketDesignator"].Value))
                    {
                        paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Add(
                            new Tuple<string, string, string>(ffnode.Attributes["TicketDesignator"].Value, "TD", ffnode.Attributes["RPH"].Value));
                    }
                }

                if (!paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Any(i => i.Item2.StartsWith("P")))
                {
                    var elemPax = oRootStored.SelectSingleNode(
                        "fareList[fareReference/uniqueReference = '" + fnode.Attributes["RPH"].Value +
                        "']/paxSegReference");
                    var strTSTpax = fnode.SelectSingleNode("PassengerType").Attributes["Code"].Value;
                    foreach (XmlNode pax in elemPax.SelectNodes("refDetails/refNumber"))
                    {
                        paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Add(new Tuple<string, string, string>("",
                            strTSTpax == "INF" ? "PI" : ((strTSTpax == "ADT" || strTSTpax == "JCB") ? "PA" : "P"),
                            paxMap[pax.InnerText]));
                    }
                }
            }

            paxFareSegs.ForEach(pfs => pfs.Item2.RemoveAll(p => p.Item2 == "T"));
            var paxFareSegsGrouped = new List<Tuple<string, List<Tuple<string, string, string>>>>();
            paxFareSegsGrouped = paxFareSegs;

            if (paxFareSegsGrouped.SelectMany(x => x.Item2).ToList().FindAll(x => x.Item2 == "S").TrueForAll(s => s.Item1 == paxFareSegsGrouped.First().Item2.First(x => x.Item2 == "S").Item1) &&
                paxFareSegsGrouped.SelectMany(x => x.Item2).ToList().FindAll(x => x.Item2 == "S").Distinct().Count().Equals(segCount) &&
                paxFareSegsGrouped.SelectMany(x => x.Item2).ToList().FindAll(x => x.Item2 == "TD").Distinct().Count().Equals(segCount) &&
                paxFareSegsGrouped.TrueForAll(p => p.Item2.FindAll(x => x.Item2 == "TD").OrderBy(x => x.Item3).SequenceEqual(
                          paxFareSegsGrouped.First().Item2.FindAll(x => x.Item2 == "TD").OrderBy(x => x.Item3)))
                )
            {
                string tdes = "";
                var grpTd = paxFareSegsGrouped.First().Item2.FindAll(x => x.Item2 == "TD").GroupBy(x => x.Item1);
                if (grpTd.Count().Equals(1))
                    tdes = grpTd.Select(x => $"/ZO-0*{x.Key}").First();
                else
                {
                    foreach (var item in grpTd)
                        tdes += $"/ZO-0*{item.Key}.{string.Join(",", paxFareSegsGrouped.First().Item2.FindAll(x => x.Item2 == "TD" && x.Item1 == item.Key).Select(x => x.Item3))}";
                }
                res.Add(new Tuple<string, string>("", $"/FF-{paxFareSegsGrouped.First().Item2.First(x => x.Item2 == "S").Item1}{tdes}"));
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
                    var segs = psg.First().Item2.FindAll(p => p.Item2.StartsWith("S")).TrueForAll(s => s.Item1.Equals(psg.First().Item2.First().Item1)) && psg.First().Item2.FindAll(p => p.Item2.StartsWith("S")).Count.Equals(segCount) ?
                        $"/FF-{psg.First().Item2.First(x => x.Item2 == "S").Item1}" :
                        $"{string.Join("", psg.First().Item2.FindAll(p => p.Item2.StartsWith("S")).SelectMany(s => $"/FF{s.Item3}-{s.Item1}"))}";

                    string tdes = "";
                    var grpTd = psg.First().Item2.FindAll(x => x.Item2 == "TD").GroupBy(x => x.Item1);
                    if (grpTd.Count().Equals(1))
                        tdes = grpTd.Select(x => $"/ZO-0*{x.Key}").First();
                    else
                    {
                        foreach (var item in grpTd)
                            tdes += $"/ZO-0*{item.Key}.{string.Join(",", paxFareSegsGrouped.First().Item2.FindAll(x => x.Item2 == "TD" && x.Item1 == item.Key).Select(x => x.Item3))}";
                    }

                    List<string> tRes = new List<string>();
                    {
                        var isInf = psg.SelectMany(x => x.Item2).Any(p => p.Item2.Equals("PI"));
                        tRes.Add($"/P{string.Join(",", psg.SelectMany(x => x.Item2).Where(p => p.Item2.StartsWith("P")).SelectMany(s => $"{s.Item3}"))}{(isInf ? "/INF" : "")}" +
                            $"{(psg.SelectMany(x => x.Item2).Any(p => p.Item2.Equals("PA")) ? "/PAX" : "")}{(tRes.Count.Equals(0) ? segs + tdes : "")}");
                    }
                    res.Add(new Tuple<string, string>(string.Join(",", psg.Select(x => x.Item1)), string.Join("/", tRes)));
                }
            }

            return res;
        }

        private List<string> GetPricingOptions(string request, string tstResp, string pnrRead, XmlElement oRootStored)
        {
            XmlDocument tstdoc = new XmlDocument();
            tstdoc.LoadXml("<Ticket_GetPricingOptions>" + tstResp + "</Ticket_GetPricingOptions>");
            XmlElement tst = tstdoc.DocumentElement;
            if (!tstResp.Contains("pricingOptionsGroup"))
            {
                return new List<string> { "" };
            }

            var res = new List<string>();

            XmlNodeList opts = tst.SelectNodes("//Ticket_GetPricingOptionsReply");
            foreach (XmlNode opt in opts)
            {
                XmlDocument poks = new XmlDocument();
                poks.LoadXml(opt.SelectSingleNode("documentInformation").OuterXml);

                var memberNames = poks.SelectNodes("//pricingOptionsGroup").Cast<XmlNode>()
                               .Select(node => node).Where(n => n.InnerText != "RP" && n.InnerText != "NOP" && n.InnerText != "RLO" && n.InnerText != "RU")
                               .ToList();

                var xml = new XElement("option", from po in memberNames
                                                 select
                    XElement.Parse(po.OuterXml.Replace("pricingOptionsGroup", "pricingOptionGroup")),
                     new XElement("pricingOptionGroup",
                      new XElement("pricingOptionKey",
                       new XElement("pricingOptionKey", "SEL")),
                       new XElement("paxSegTstReference",
                        new XElement("referenceDetails",
                        new XElement("type", "T"), new XElement("value", poks.SelectSingleNode("//documentSelection/uniqueReference").InnerText))
                )));
                res.Add(xml.Nodes().Aggregate("", (b, node) => b += node.ToString()));
            }
            return res;
        }

        private List<Tuple<string, string>> GetPricingOptionsFXX(string request, string tstResp, string pnrRead, XmlElement oRootStored)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(request);
            XmlElement root = doc.DocumentElement;

            XmlDocument tstdoc = new XmlDocument();
            tstdoc.LoadXml("<Ticket_GetPricingOptions>" + tstResp + "</Ticket_GetPricingOptions>");
            XmlElement tstRsp = tstdoc.DocumentElement;

            // Blocked check
            //if (tstResp.Contains(">PFF<"))
            //    return new List<Tuple<string, string>> { new Tuple<string, string>("", "") };

            var res = new List<Tuple<string, string>>();

            XmlNodeList nodes = root.SelectNodes("//StoredFare");

            XmlDocument pnr = new XmlDocument();
            pnr.LoadXml(pnrRead);
            XmlNodeList paxLines = pnr.DocumentElement.SelectNodes("//travellerInfo/elementManagementPassenger");
            Dictionary<string, string> paxMap = new Dictionary<string, string>();
            foreach (XmlNode fnode in paxLines)
            {
                paxMap.Add(fnode.SelectSingleNode("reference/number").InnerText,
                    fnode.SelectSingleNode("lineNumber").InnerText);
            }

            var segCount = pnr.DocumentElement
                .SelectNodes("//originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR']")
                .Count;

            List<Tuple<string, List<Tuple<string, string, string>>>> paxFareSegs =
                new List<Tuple<string, List<Tuple<string, string, string>>>>();
            foreach (XmlNode fnode in nodes)
            {
                var tst = fnode.Attributes["RPH"].Value;
                if (!paxFareSegs.Any(fs => fs.Item1 == tst))
                    paxFareSegs.Add(new Tuple<string, List<Tuple<string, string, string>>>(tst,
                            new List<Tuple<string, string, string>>()));

                if (!paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Any(i => i.Item2.StartsWith("P")))
                {
                    var elemPax = oRootStored.SelectSingleNode(
                        "fareList[fareReference/uniqueReference = '" + fnode.Attributes["RPH"].Value +
                        "']/paxSegReference");
                    var strTSTpax = fnode.SelectSingleNode("PassengerType").Attributes["Code"].Value;
                    foreach (XmlNode pax in elemPax.SelectNodes("refDetails/refNumber"))
                    {
                        paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Add(new Tuple<string, string, string>("",
                            strTSTpax == "INF" ? "PI" : ((strTSTpax == "ADT" || strTSTpax == "JCB") ? "PA" : "P"),
                            paxMap[pax.InnerText]));
                    }
                }
                if (paxFareSegs.Any(fs => fs.Item1 == tst))
                {
                    foreach (XmlNode optDet in tstRsp.SelectNodes($"//documentInformation[documentSelection/referenceType='TST' and documentSelection/uniqueReference='{tst}']/pricingOptionsGroup[pricingOptionKey/pricingOptionKey='PRM']/optionDetail"))
                    {
                        XmlDocument poks = new XmlDocument();
                        poks.LoadXml(optDet.OuterXml);
                        paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Add(new Tuple<string, string, string>("",
                           $"*{poks.SelectSingleNode("//criteriaDetails/attributeType").InnerText}",
                           ""));
                    }
                }
            }
            var paxFareSegsGrouped = new List<Tuple<string, List<Tuple<string, string, string>>>>();
            paxFareSegsGrouped = paxFareSegs;
            //foreach (var psg in paxFareSegsGrouped.GroupBy(x => new
            //{
            //    segs = string.Join(",", x.Item2.FindAll(p => p.Item2.StartsWith("*"))),
            //    isInf = x.Item2.Any(p => p.Item2.Equals("PI"))
            //}))
            {
                List<string> tRes = new List<string>();
                //foreach (var ps in psg)
                foreach (var ps in paxFareSegs)
                {
                    var isInf = ps.Item2.Any(p => p.Item2.Equals("PI"));
                    var opt = $"{string.Join(",", ps.Item2.FindAll(p => p.Item2.StartsWith("*")).Select(s => $",{s.Item2}"))}";
                    tRes.Add($"{opt}{(isInf ? "/INF" : "")}" +
                        $"{(ps.Item2.Any(p => p.Item2.Equals("PA")) ? "/PAX" : "")}");
                    var fbCodes = new Dictionary<string, string>();
                    var tktDes = new Dictionary<string, string>();
                    foreach (XmlNode item in root.SelectSingleNode($"//StoredFare[@RPH='{ps.Item1}']").SelectNodes("FareSegments/AirSegments"))
                    {
                        fbCodes[item.InnerText.TrimEnd('/')] = fbCodes.ContainsKey(item.InnerText.TrimEnd('/'))
                            ? fbCodes[item.InnerText.TrimEnd('/')] + "," + item.Attributes["RPH"].Value
                            : item.Attributes["RPH"].Value;
                        if (item.Attributes["TicketDesignator"] != null)
                            tktDes[item.Attributes["TicketDesignator"].Value] = tktDes.ContainsKey(item.Attributes["TicketDesignator"].Value)
                                ? tktDes[item.Attributes["TicketDesignator"].Value] + "," + item.Attributes["RPH"].Value
                                : item.Attributes["RPH"].Value;
                    }
                    tRes.Add(string.Join("/", fbCodes.Select(x => $"A{x.Value}-{x.Key}")));
                    if (tktDes.Any())
                    {
                        tRes.Add(string.Join("/", tktDes.Select(x => $"ZO-0*{x.Key}.{x.Value}")));
                    }
                    tRes.Add($"P{string.Join(",", ps.Item2.FindAll(p => p.Item2.StartsWith("P")).SelectMany(s => $"{s.Item3}"))}");
                    res.Add(new Tuple<string, string>(ps.Item1, string.Join("/", tRes).Replace("/,", ",").TrimEnd('/')));
                    tRes = new List<string>();
                }
            }
            if (res.TrueForAll(r => System.Text.RegularExpressions.Regex.Replace(r.Item2, @"\/(P\d+|PAX|PI|INF)", "")
                            .Equals(System.Text.RegularExpressions.Regex.Replace(res.First().Item2, @"\/(P\d+|PAX|PI|INF)", ""))))
            {
                var combRes = System.Text.RegularExpressions.Regex.Replace(res.First().Item2, @"\/(P\d+|PAX|PI|INF)", "");
                res.Clear();
                res.Add(new Tuple<string, string>("", combRes));
            }
            return res;
        }

        private List<string> GetPricingOptionsFF_FB(string request, string pnrRead, XmlElement oRootStored)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(request);
            XmlElement root = doc.DocumentElement;
            var reqElems = "BrandedFares/FareFamily";
            var isFF = true;

            if (!request.Contains("BrandedFares") ||
                root.SelectNodes("//StoredFare/BrandedFares").Cast<XmlNode>().ToList().TrueForAll(x => x.InnerText.Equals("")))
            {
                isFF = false;
                reqElems = "FareSegments/AirSegments[text() != 'VOID']";
            }

            var res = new List<string>();

            XmlNodeList nodes = root.SelectNodes("//StoredFare");

            XmlDocument pnr = new XmlDocument();
            pnr.LoadXml(pnrRead);
            XmlNodeList flightSegs = pnr.DocumentElement.SelectNodes("//originDestinationDetails/itineraryInfo");
            Dictionary<string, string> flSegMap = new Dictionary<string, string>();
            foreach (XmlNode fnode in flightSegs)
            {
                flSegMap.Add(fnode.SelectSingleNode("elementManagementItinerary/lineNumber").InnerText, fnode.SelectSingleNode("elementManagementItinerary/reference/number").InnerText);
            }

            List<Tuple<string, List<Tuple<string, string, string>>>> paxFareSegs = new List<Tuple<string, List<Tuple<string, string, string>>>>();
            foreach (XmlNode fnode in nodes)
            {
                var tst = fnode.Attributes["RPH"].Value;
                if (!paxFareSegs.Any(fs => fs.Item1 == tst))
                    paxFareSegs.Add(new Tuple<string, List<Tuple<string, string, string>>>(tst, new List<Tuple<string, string, string>>()));
                foreach (XmlNode ffnode in fnode.SelectNodes(reqElems))
                {
                    if (!paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Any(i => i.Item1 == ffnode.InnerText && i.Item2 == "S" && i.Item3 == ffnode.Attributes["RPH"].Value))
                        paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Add(new Tuple<string, string, string>(ffnode.InnerText, "S", ffnode.Attributes["RPH"].Value));

                    if (!paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Any(i => i.Item2.StartsWith("P")))
                    {
                        var elemPax = oRootStored.SelectSingleNode("fareList[fareReference/uniqueReference = '" + fnode.Attributes["RPH"].Value + "']/paxSegReference");
                        XmlNode oTSTpax = oRootStored.SelectSingleNode("fareList[fareReference/uniqueReference = '" + fnode.Attributes["RPH"].Value + "']/statusInformation/firstStatusDetails/tstFlag");
                        string strTSTpax = "";

                        if (oTSTpax != null)
                            strTSTpax = oTSTpax.InnerXml;

                        foreach (XmlNode pax in elemPax.SelectNodes("refDetails/refNumber"))
                        {
                            paxFareSegs.Find(fs => fs.Item1 == tst).Item2.Add(new Tuple<string, string, string>("", strTSTpax == "INF" ? "PI" : (strTSTpax == "CHD" ? "P" : "PA"), pax.InnerText));
                        }
                    }
                }
            }

            paxFareSegs.ForEach(pfs => pfs.Item2.RemoveAll(p => p.Item1 == "T"));
            var paxFareSegsGrouped = new List<Tuple<string, List<Tuple<string, string, string>>>>();
            foreach (var pfs in paxFareSegs)
            {
                if (paxFareSegsGrouped.Any(x => x.Item2.Any(pg => pfs.Item2.FindAll(p => p.Item2.StartsWith("P")).Any(pf => pf.Item2 == pg.Item2 && pf.Item3 == pg.Item3))))
                    continue;
                paxFareSegsGrouped.Add(new Tuple<string, List<Tuple<string, string, string>>>(pfs.Item1, new List<Tuple<string, string, string>>()));
                var pgs = paxFareSegsGrouped.Find(x => x.Item1 == pfs.Item1);
                foreach (var pfg in paxFareSegs.FindAll(x => x.Item2.FindAll(i => i.Item2 == "S").OrderBy(i => i.Item3).Except(pfs.Item2.FindAll(pi => pi.Item2 == "S").OrderBy(pi => pi.Item3)).Count().Equals(0)).SelectMany(x => x.Item2))
                {
                    if (!pgs.Item2.Any(x => x.Item1 == pfg.Item1 && x.Item2 == pfg.Item2 && x.Item3 == pfg.Item3))
                        pgs.Item2.Add(pfg);
                }
            }
            Console.WriteLine();

            var xml = new XElement("options",
                from fgs in paxFareSegsGrouped
                select
                    new XElement("option",
                    from fs in fgs.Item2.FindAll(x => !string.IsNullOrEmpty(x.Item1)).Select(x => x.Item1).Distinct()
                    select
                    new XElement("pricingOptionGroup",
                    new XElement("pricingOptionKey",
                      new XElement("pricingOptionKey", isFF ? "PFF" : "FBA")),
                    new XElement("optionDetail",
                    isFF ?
                      new XElement("criteriaDetails",
                          new XElement("attributeType", "FF"),
                          new XElement("attributeDescription", fs)) :
                      new XElement("criteriaDetails",
                          new XElement("attributeType", fs.TrimEnd('/'))))
                  , new XElement("paxSegTstReference",
                            fgs.Item2.FindAll(f => f.Item1 == fs || string.IsNullOrEmpty(f.Item1)).OrderByDescending(fse => fse.Item2).Select(fseg =>
                                new XElement("referenceDetails",
                                    new XElement("type", fseg.Item2),
                                    new XElement("value", fseg.Item2 == "S" ? flSegMap[fseg.Item3] : fseg.Item3))))
                  )));

            Console.WriteLine(xml);
            //Amadeus  IR 21319264 [DTT] Fare_PricePNRWithBookingClass not returning requested FareFamily/pax
            //res = string.Join("", xml.Elements().Select(x => x.ToString()).ToList());
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

                    if (ttProviderSystems.LogNative)
                    {
                        TripXMLTools.TripXMLLog.LogMessage("TransferOwnership", ref strMessage, requestTime, responseTime, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                    }
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
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
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

                try
                {
                    response = ConvertToTripXMLMessage(strRequest, inSession, response, strResponseTST);
                    if (!string.IsNullOrEmpty(warning))
                        response = response.Replace("<Success />", $"<Success />{warning}");
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
            }
            catch (Exception exx)
            {
                response = modCore.FormatErrorMessage(modCore.ttServices.PNREnd, exx.Message, ttProviderSystems);
            }
            return response;
        }

        private string ConvertToTripXMLMessage(string request, bool inSession, string response, string responseTST = "")
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
                response = CoreLib.TransformXML(response, XslPath, $"{Version}AmadeusWS_PNRReadRS.xsl");
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

    }
}
