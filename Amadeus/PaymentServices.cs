using System.Xml;
using TripXMLMain;
using System.Text;
using System;
using System.Globalization;
using static TripXMLMain.modCore.enAmadeusWSSchema;

namespace AmadeusWS
{
    public class PaymentServices : AmadeusBase
    {
        public PaymentServices()
        {
            Request = "";
            ConversationID = "";
        }
             
        public string CreateVirtualCard()
        {
            
            string strFinalResponse;
            string strRecLocator = ""; //Not sure if this is for future development...
            string strResponse;
            try
            {
                var sbNativeLog = new StringBuilder();                                
                DateTime requestTime = DateTime.Now;



                //*****************************************************************
                // Transform OTA PNRSplit Request into Several Navite Requests    *
                //***************************************************************** 

                string strRequest = SetRequest($"AmadeusWS_GenerateVirtualCardRQ.xsl");
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                /****** Additional Namespaces *****/
                //Put needed namspaces into the message.
                strRequest = strRequest.Replace("<AMA_PAY_GenerateVirtualCardRQ Version=\"2.0\">", "<AMA_PAY_GenerateVirtualCardRQ Version=\"2.0\" xmlns=\"http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2\" xmlns:pay1=\"http://xml.amadeus.com/2010/06/PAY_Types_v1\" xmlns:pay=\"http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2\"  xmlns:etr=\"http://xml.amadeus.com/2010/06/ETR_Types_v4\" xmlns:pnr=\"http://xml.amadeus.com/2010/06/PNR_Types_v4\">");
                strRequest = strRequest.Replace("AllowedTransactions", "pay1:AllowedTransactions");
                strRequest = strRequest.Replace("ValidityPeriod", "pay1:ValidityPeriod");
                strRequest = strRequest.Replace("AdditionalInfo", "pay1:AdditionalInfo");
                
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                sbNativeLog.Append($"{Environment.NewLine}{strRequest}");
                strResponse = SendGenerateVirtualCard(ttAA, strRequest);
                sbNativeLog.Append($"{Environment.NewLine}{strResponse}");


                //*****************************************************************
                // Transform Native Amadeus PNRSplit Response into OTA Response   *
                //***************************************************************** 
                try
                {
                    Version = "";

                    strResponse = strResponse.Replace(" xmlns=\"http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2\"", "");
                    strResponse = strResponse.Replace(" xmlns:iata=\"http://www.iata.org/IATA/2007/00/IATA2010.1\"", "");
                    strResponse = strResponse.Replace(" xmlns:ota=\"http://www.opentravel.org/OTA/2003/05/OTA2011A\"", "");
                    strResponse = strResponse.Replace(" xmlns:etr=\"http://xml.amadeus.com/2010/06/ETR_Types_v4\"", "");
                    strResponse = strResponse.Replace(" xmlns:fop=\"http://xml.amadeus.com/2010/06/FOP_Types_v6\"", "");
                    strResponse = strResponse.Replace(" xmlns:pay=\"http://xml.amadeus.com/2010/06/PAY_Types_v1\"", "");
                    strResponse = strResponse.Replace(" xmlns:pnr=\"http://xml.amadeus.com/2010/06/PNR_Types_v4\"", "");
                    strResponse = strResponse.Replace(" xmlns:tkt=\"http://xml.amadeus.com/2010/06/PayIssueTypes_v1\"", "");
                    strResponse = strResponse.Replace(" xmlns:qt=\"http://xml.amadeus.com/2010/06/QuotationTypes_v2\"", "");
                    strResponse = strResponse.Replace(" xmlns:ttr=\"http://xml.amadeus.com/2010/06/TTR_Types_v3\"", "");
                    strResponse = strResponse.Replace(" xmlns:ama_ct=\"http://xml.amadeus.com/2010/06/Types_v3\"", "");
                    strResponse = strResponse.Replace(" xmlns:ama=\"http://xml.amadeus.com/2010/06/Types_v3\"", "");

                    strResponse = strResponse.Replace("iata:", "");
                    strResponse = strResponse.Replace("ota:", "");
                    strResponse = strResponse.Replace("etr:", "");
                    strResponse = strResponse.Replace("fop:", "");
                    strResponse = strResponse.Replace("pay:", "");
                    strResponse = strResponse.Replace("pnr:", "");
                    strResponse = strResponse.Replace("tkt:", "");
                    strResponse = strResponse.Replace("qt:", "");
                    strResponse = strResponse.Replace("ttr:", "");
                    strResponse = strResponse.Replace("ama_ct:", "");
                    strResponse = strResponse.Replace("ama:", "");

                    if (inSession)
                        strResponse = strResponse.Replace("</AMA_PAY_GenerateVirtualCardRS>", $"<ConversationID>{ConversationID}</ConversationID></AMA_PAY_GenerateVirtualCardRS>");

                    CoreLib.SendTrace(ttProviderSystems.UserID, "PAY_VirtualCreditCard", "GenerateVirtualCard", strResponse, ttProviderSystems.LogUUID);
                    strFinalResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_GenerateVirtualCardRS.xsl");
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

                DateTime responseTime = DateTime.Now;
                string strMessage = sbNativeLog.ToString();
                sbNativeLog.Clear();

                if (ttProviderSystems.LogNative)
                {
                    TripXMLTools.TripXMLLog.LogMessage("CreateVirtualCard", ref strMessage, requestTime, responseTime, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                strFinalResponse = modCore.FormatErrorMessage(modCore.ttServices.GenerateVirtualCard, exx.Message, ttProviderSystems, strRecLocator);
            }

            return strFinalResponse;
        }

        [Obsolete("Not Implemented.")]
        public string CancelVirtualCardLoad()
        {
            string strResponse;
            string strRecLocator = "";
            try
            {
                var sbNativeLog = new StringBuilder();
                DateTime requestTime = DateTime.Now;
                //*****************************************************************
                // Transform OTA PNRCancel Request into Several Navite Requests    *
                //***************************************************************** 

                string strRequest = SetRequest($"AmadeusWS_CancelVirtualCardRQ.xsl");
                
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

                // *******************
                // Create Session    *
                // *******************
                AmadeusWSAdapter ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                sbNativeLog.Append($"{Environment.NewLine}{strRetrieve}");
                strResponse = SendRetrivePNRbyRL(ttAA, strRetrieve);
                sbNativeLog.Append($"{Environment.NewLine}{strResponse}");                

                // *******************
                // Check for Errors  *
                // *******************
                bool blnHotelSegs;
                try
                {
                    XmlNodeList oNodeList = null;
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;
                    if (oRoot != null)
                    {
                        if (oRoot.SelectSingleNode("Error") != null)
                        {
                            throw new Exception(oRoot.SelectSingleNode("Error")?.InnerText);
                        }

                        if (oRoot.SelectSingleNode("generalErrorInfo") != null && oRoot.SelectSingleNode("generalErrorInfo")?.SelectSingleNode("messageErrorInformation")?.SelectSingleNode("errorDetail")?.SelectSingleNode("qualifier")?.InnerText != "INF")
                        {
                            throw new Exception(oRoot.SelectSingleNode("generalErrorInfo")?.SelectSingleNode("messageErrorText")?.SelectSingleNode("text")?.InnerText);
                        }

                        if (oRoot.SelectSingleNode("CAPI_Messages") != null)
                        {
                            if (oRoot.SelectSingleNode("CAPI_Messages/ErrorCode") != null)
                            {
                                throw new Exception(oRoot.SelectSingleNode("CAPI_Messages/Text")?.InnerText);
                            }
                        }

                        # region business logic
                        if (ttProviderSystems.BLFile != "" && !strResponse.Contains("generalErrorInfo"))
                        {
                            XmlDocument oDocBL = new XmlDocument();
                            oDocBL.Load(ttProviderSystems.BLFile);

                            XmlElement oRootBL = oDocBL.DocumentElement;
                            XmlNode oNodeBL = oRootBL?.SelectSingleNode($"Security/ProviderBL[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\']");

                            if (oNodeBL != null)
                            {
                                // get accounting line from PNR
                                XmlDocument oDocRespBL = new XmlDocument();
                                oDocRespBL.LoadXml(strResponse);

                                XmlElement oRootRespBL = oDocRespBL.DocumentElement;
                                XmlNode oNodeRespBL = oRootRespBL?.SelectSingleNode("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']");

                                if (oNodeRespBL != null)
                                {
                                    if (oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']") != null)
                                    {
                                        string strAIAN = oNodeRespBL.SelectSingleNode("accounting/account/number")?.InnerXml;
                                        string strAinBL = oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']/@AuthorizeCode")?.InnerXml;

                                        //compare to the one in BL file 
                                        if (strAIAN != strAinBL)
                                        {
                                            throw new Exception("Secured PNR");
                                        }
                                    }
                                }
                            }
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
                    throw new Exception($"Error.\r\n{ ex.Message }");
                }


                //****************************************************************************
                // Send Cancel (11) Request or Send Cancel (0) and End Transaction Request   *
                //****************************************************************************

                try
                {
                    if (blnHotelSegs)
                    {
                        sbNativeLog.Append($"{Environment.NewLine}{strCancelHotel}");
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

                                foreach (XmlNode oSeg in oRoot?.SelectNodes("originDestinationDetails/itineraryInfo"))
                                {
                                    if (oSeg.SelectNodes("relatedProduct/status").Count == 1 && oSeg.SelectSingleNode("elementManagementItinerary/segmentName")?.InnerText != "RU")
                                    {
                                        strSegments += $"<element><identifier>ST</identifier><number>{oSeg.SelectSingleNode("elementManagementItinerary/reference/number")?.InnerText}</number></element>";
                                    }
                                }

                                if (!string.IsNullOrEmpty(strSegments))
                                {
                                    strCancel = $"<PNR_Cancel><pnrActions><optionCode>11</optionCode></pnrActions><cancelElements><entryType>E</entryType>{strSegments}</cancelElements></PNR_Cancel>";
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

                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Canceling PNR.\r\n{ex.Message}");
                }

                //*****************************************************************
                // Transform Native Amadeus PNRCancel Response into OTA Response   *
                //***************************************************************** 

                try
                {
                    strResponse = strResponse.Replace("<AMA_PAY_CancelVirtualCardRS xmlns=\"http://xml.amadeus.com/PNRACC_08_3_1A\">", "<AMA_PAY_CancelVirtualCardRS>");
                    if (inSession)
                        strResponse = strResponse.Replace("</AMA_PAY_CancelVirtualCardRS>", $"<ConversationID>{ConversationID}</ConversationID></AMA_PAY_CancelVirtualCardRS>");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CancelVirtualCardRS.xsl");
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
                sbNativeLog.Clear();

                if (ttProviderSystems.LogNative)
                {
                    TripXMLTools.TripXMLLog.LogMessage("VCC Cancel", ref strMessage, requestTime, responseTime, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CancelVirtualCardLoad, exx.Message, ttProviderSystems, strRecLocator);
            }

            return strResponse;
        }

        public string DeleteVirtualCard()
        {         
            string strResponseDelete;
            //*****************************************************************
            // Transform OTA Get Card Details Request into Native Amadeus Request     *
            //***************************************************************** 

            try
            {
                _tracerID = string.Empty;
                
                string strRequest = SetRequest($"AmadeusWS_DeleteVirtualCardRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //*******************************************************************************
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                //******************************************************************************* 
                AmadeusWSAdapter ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);
                var strResponse = SendDeleteVirtualCard(ttAA, strRequest);

                //*****************************************************************
                // Transform Native Amadeus PNRRead Response into OTA Response   *
                //***************************************************************** 
                try
                {
                    Version = "";

                    strResponse = strResponse.Replace(" xmlns=\"http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2\"", "");
                    strResponse = strResponse.Replace(" xmlns:iata=\"http://www.iata.org/IATA/2007/00/IATA2010.1\"", "");
                    strResponse = strResponse.Replace(" xmlns:ota=\"http://www.opentravel.org/OTA/2003/05/OTA2011A\"", "");
                    strResponse = strResponse.Replace(" xmlns:etr=\"http://xml.amadeus.com/2010/06/ETR_Types_v4\"", "");
                    strResponse = strResponse.Replace(" xmlns:fop=\"http://xml.amadeus.com/2010/06/FOP_Types_v6\"", "");
                    strResponse = strResponse.Replace(" xmlns:pay=\"http://xml.amadeus.com/2010/06/PAY_Types_v1\"", "");
                    strResponse = strResponse.Replace(" xmlns:pnr=\"http://xml.amadeus.com/2010/06/PNR_Types_v4\"", "");
                    strResponse = strResponse.Replace(" xmlns:tkt=\"http://xml.amadeus.com/2010/06/PayIssueTypes_v1\"", "");
                    strResponse = strResponse.Replace(" xmlns:qt=\"http://xml.amadeus.com/2010/06/QuotationTypes_v2\"", "");
                    strResponse = strResponse.Replace(" xmlns:ttr=\"http://xml.amadeus.com/2010/06/TTR_Types_v3\"", "");
                    strResponse = strResponse.Replace(" xmlns:ama_ct=\"http://xml.amadeus.com/2010/06/Types_v3\"", "");
                    strResponse = strResponse.Replace(" xmlns:ama=\"http://xml.amadeus.com/2010/06/Types_v3\"", "");


                    strResponse = strResponse.Replace("iata:", "");
                    strResponse = strResponse.Replace("ota:", "");
                    strResponse = strResponse.Replace("etr:", "");
                    strResponse = strResponse.Replace("fop:", "");
                    strResponse = strResponse.Replace("pay:", "");
                    strResponse = strResponse.Replace("pnr:", "");
                    strResponse = strResponse.Replace("tkt:", "");
                    strResponse = strResponse.Replace("qt:", "");
                    strResponse = strResponse.Replace("ttr:", "");
                    strResponse = strResponse.Replace("ama_ct:", "");
                    strResponse = strResponse.Replace("ama:", "");

                    if (inSession)
                        strResponse = strResponse.Replace("</AMA_PAY_DeleteVirtualCardRS>", $"<ConversationID>{ConversationID}</ConversationID></AMA_PAY_DeleteVirtualCardRS>");

                    CoreLib.SendTrace(ttProviderSystems.UserID, "PAY_VirtualCreditCard", "DeleteVirtualCard", strResponse, ttProviderSystems.LogUUID);
                    strResponseDelete = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_DeleteVirtualCardRS.xsl");
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
                strResponseDelete = modCore.FormatErrorMessage(modCore.ttServices.DeleteVirtualCard, exx.Message, ttProviderSystems);
            }

            return strResponseDelete;
        }

        public string ListVirtualCards()
        {
            string strResponse;
            string strRecLocator = "";
            
            try
            {
                var sbNativeLog = new StringBuilder();
                AmadeusWSAdapter ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);                
                DateTime requestTime = DateTime.Now;

                //*****************************************************************
                // Transform OTA PNRSplit Request into Several Navite Requests    *
                //***************************************************************** 

                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);

                string strRequest = SetRequest($"AmadeusWS_ListVirtualCardsRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");
                
                sbNativeLog.Append($"{Environment.NewLine}{strRequest}");
                strResponse = SendListVirtualCards(ttAA, strRequest);
                sbNativeLog.Append($"{Environment.NewLine}{strResponse}");               

                //*****************************************************************
                // Transform Native Amadeus PNRSplit Response into OTA Response   *
                //***************************************************************** 
                try
                {
                    Version = "";

                    strResponse = strResponse.Replace("xmlns=\"http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2\"","");
                    strResponse = strResponse.Replace("xmlns:iata=\"http://www.iata.org/IATA/2007/00/IATA2010.1\"", "");
                    strResponse = strResponse.Replace("xmlns:ota=\"http://www.opentravel.org/OTA/2003/05/OTA2011A\"", ""); 
                    strResponse = strResponse.Replace("xmlns:etr=\"http://xml.amadeus.com/2010/06/ETR_Types_v4\"", "");
                    strResponse = strResponse.Replace("xmlns:fop=\"http://xml.amadeus.com/2010/06/FOP_Types_v6\"", "");
                    strResponse = strResponse.Replace("xmlns:pay=\"http://xml.amadeus.com/2010/06/PAY_Types_v1\"", "");
                    strResponse = strResponse.Replace("xmlns:pnr=\"http://xml.amadeus.com/2010/06/PNR_Types_v4\"", "");
                    strResponse = strResponse.Replace("xmlns:tkt=\"http://xml.amadeus.com/2010/06/PayIssueTypes_v1\"", "");
                    strResponse = strResponse.Replace("xmlns:qt=\"http://xml.amadeus.com/2010/06/QuotationTypes_v2\"", "");
                    strResponse = strResponse.Replace("xmlns:ttr=\"http://xml.amadeus.com/2010/06/TTR_Types_v3\"", "");
                    strResponse = strResponse.Replace("xmlns:ama_ct=\"http://xml.amadeus.com/2010/06/Types_v3\"", "");

                    strResponse = strResponse.Replace("iata:", "");
                    strResponse = strResponse.Replace("ota:", "");
                    strResponse = strResponse.Replace("etr:", "");
                    strResponse = strResponse.Replace("fop:", "");
                    strResponse = strResponse.Replace("pay:", "");
                    strResponse = strResponse.Replace("pnr:", "");
                    strResponse = strResponse.Replace("tkt:", "");
                    strResponse = strResponse.Replace("qt:", "");
                    strResponse = strResponse.Replace("ttr:", "");
                    strResponse = strResponse.Replace("ama_ct:", "");

                    if (inSession)
                        strResponse = strResponse.Replace("</AMA_PAY_ListVirtualCardsRS>", $"<ConversationID>{ConversationID}</ConversationID></AMA_PAY_ListVirtualCardsRS>");

                    CoreLib.SendTrace(ttProviderSystems.UserID, "PAY_VirtualCreditCard", "ListVirtualCards", strResponse, ttProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_ListVirtualCardsRS.xsl");
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

                string strMessage = sbNativeLog.ToString();
                sbNativeLog.Clear();

                if (ttProviderSystems.LogNative)
                {
                    TripXMLTools.TripXMLLog.LogMessage("ListVirtualCards", ref strMessage, requestTime, DateTime.Now, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.ListVirtualCards, exx.Message, ttProviderSystems, strRecLocator);
            }
            
            return strResponse;
        }

        [Obsolete("Not Implemented")]
        public string ScheduleVirtualCardLoad()
        {
            string strResponse = "";           

            try
            {
                //*****************************************************************
                // Transform OTA Queue Request into Native Amadeus Request     *
                //***************************************************************** 
                
                AmadeusWSAdapter ttAA = SetAdapter();                
                bool inSession = SetConversationID(ttAA);

                string strRequest = SetRequest($"AmadeusWS_QueueRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                try
                {

                    #region  Send Transformed Request to the Amadeus Adapter and Getting Native Response  

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

                    #endregion

                    #region Format Response
                    if (strResponse.StartsWith("<Queue_ListReply>") & !strResponse.Contains("errorReturn"))
                    {
                        // get total number of pnr in response and in request
                        var oDocResp = new XmlDocument();
                        oDocResp.LoadXml(strResponse);
                        XmlElement oRootResp = oDocResp.DocumentElement;

                        var oDocReq = new XmlDocument();
                        oDocReq.LoadXml(strRequest);
                        XmlElement oRootReq = oDocReq.DocumentElement;

                        int iPNRsResp = Convert.ToInt32(oRootResp?.SelectSingleNode("queueView/pnrCount[1]/quantityDetails/numberOfUnit")?.InnerText);
                        int iPNRsReq = Convert.ToInt32(oRootReq?.SelectSingleNode("scanRange/rangeDetails/max")?.InnerText);

                        if (iPNRsResp > iPNRsReq)
                        {
                            strRequest = strRequest.Replace("<Queue_List>", "<Queue_List><scroll><numberOfItemsDetails><actionQualifier>54</actionQualifier></numberOfItemsDetails></scroll>");
                            oDocReq.LoadXml(strRequest);
                            oRootReq = oDocReq.DocumentElement;
                        }

                        while (iPNRsResp > iPNRsReq)
                        {
                            oRootReq.SelectSingleNode("scanRange/rangeDetails/min").InnerText = oRootReq.SelectSingleNode("scanRange/rangeDetails/min")?.InnerText + 200;
                            iPNRsReq = iPNRsReq + 200;
                            oRootReq.SelectSingleNode("scanRange/rangeDetails/max").InnerText = Convert.ToString(iPNRsReq);
                            strRequest = oRootReq.OuterXml;

                            string strResponseQL = SendListQueue(ttAA, strRequest);
                            oDocResp.LoadXml(strResponseQL);
                            oRootResp = oDocResp.DocumentElement;
                            iPNRsResp = Convert.ToInt32(oRootResp?.SelectSingleNode("queueView/pnrCount[1]/quantityDetails/numberOfUnit")?.InnerText);
                            strResponse = strResponse.Replace("</Queue_ListReply>", $"{oRootResp?.InnerXml}</Queue_ListReply>");
                        }
                    }

                    if (inSession)
                        strResponse = strResponse.Replace("</AMA_PAY_ListVirtualCardsRS>", $"<ConversationID>{ConversationID}</ConversationID></AMA_PAY_ListVirtualCardsRS>");
                    #endregion

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_QueueRS.xsl");
                    return strResponse;
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
            catch (Exception ex)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.ScheduleVirtualCardLoad, ex.Message, ttProviderSystems);
            }
            return strResponse;
        }

        [Obsolete("Not Implemented")]
        public string UpdateVirtualCard()
        {
            
            string strResponse = "";
            try
            {
                AmadeusWSAdapter ttAA = SetAdapter();
                ttAA.GetStoredFares = ttProviderSystems.GetStoredFares;
                bool inSession = SetConversationID(ttAA);

                //*****************************************************************
                // Transform OTA Queue Request into Native Amadeus Request     *
                //***************************************************************** 
                string strRequest = SetRequest($"AmadeusWS_QueueReadRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //*******************************************************************************
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                //******************************************************************************* 

                try
                {
                    string strMessage = Request.Contains("AccessQueue")
                        ? "AccessQueue"
                        : Request.Contains("ItemOnQueue")
                            ? Request.Contains("Redisplay") ? "Redisplay" : "ItemOnQueue"
                            : "ExitQueue";

                    if (strMessage != "Redisplay")
                    {
                        // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                        strResponse = strRequest.Contains("Command_Cryptic")
                                ? SendCommandCryptically(ttAA, strRequest)
                                : SendGDSMessage(ttAA, strRequest, ttProviderSystems.AmadeusWSSchema[QueueMode_ProcessQueue], ttProviderSystems.AmadeusWSSchema[QueueMode_ProcessQueueReply]);


                        if (strResponse.Contains("|Session|"))
                        {
                            CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Session Error", strResponse.Substring(0, strResponse.Length / 2), ttProviderSystems.LogUUID);
                            //Try update SessionID and reRun previos command.
                            strResponse = strRequest.Contains("Command_Cryptic")
                                ? SendCommandCryptically(ttAA, strRequest)
                                : SendGDSMessage(ttAA, strRequest, ttProviderSystems.AmadeusWSSchema[QueueMode_ProcessQueue], ttProviderSystems.AmadeusWSSchema[QueueMode_ProcessQueueReply]);
                        }
                    }

                    // Check PNR or Errors in Native Response
                    if (strResponse.StartsWith("<Error"))
                    {
                        if (strResponse.Contains("invalid character"))
                        {
                            strRequest = "<PNR_Retrieve><retrievalFacts><retrieve><type>1</type></retrieve></retrievalFacts></PNR_Retrieve>";
                            strRequest = SendRetrievePNR(ttAA, strRequest);
                            strResponse = $"{strResponse.Substring(20, strResponse.Length - 37)} in PNR {strRequest.Substring(92, 6)}";
                        }
                        strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, ttProviderSystems, "", "v03");

                        return strResponse;
                    }

                    if (strMessage == "Redisplay" || strMessage == "AccessQueue" && strResponse.Contains("RP/") || strMessage == "ItemOnQueue" && strResponse.Contains("RP/")
                        || strMessage == "AccessQueue" && strResponse.Contains("<reservation><controlNumber>") || strMessage == "ItemOnQueue" && strResponse.Contains("<reservation><controlNumber>"))
                    {
                        string strWarning = "";
                        if (strResponse.Contains("QUEUE CYCLE COMPLETE"))
                            strWarning = "<Warning Type=\"Amadeus\">QUEUE CYCLE COMPLETE</Warning>";

                        // Send PNR Redisplay
                        strRequest = "<PNR_Retrieve><retrievalFacts><retrieve><type>1</type></retrieve></retrievalFacts></PNR_Retrieve>";
                        strResponse = SendRetrievePNR(ttAA, strRequest);

                        if (strResponse.Contains("<longFreetext>--- TST ") && ttAA.GetStoredFares)
                        {
                            string strResponseTST = SendDisplayTST(ttAA);

                            if (Version == "v04_")
                            {
                                var oDocTST = new XmlDocument();
                                oDocTST.LoadXml(strResponseTST);
                                XmlElement oRootTST = oDocTST.DocumentElement;

                                foreach (XmlNode oNodeTST in oRootTST.SelectNodes("fareList"))
                                {
                                    string strRequestTST = oNodeTST.SelectSingleNode("fareReference/uniqueReference")?.InnerText;
                                    strRequest = "<Command_Cryptic><messageAction><messageFunctionDetails><messageFunction>M</messageFunction></messageFunctionDetails></messageAction><longTextString><textStringDetails>TQT/T" + strRequestTST + "</textStringDetails></longTextString></Command_Cryptic>";
                                    strResponseTST += SendCommandCryptically(ttAA, strRequest);
                                }
                            }

                            strResponse = strResponse.Replace("</PNR_Reply>", $"{strResponseTST}{strWarning}{Request}</PNR_Reply>");
                        }
                        else
                        {
                            strResponse = strResponse.Replace("</PNR_Reply>", $"{strWarning}{Request}</PNR_Reply>");
                        }

                        #region RTSVI Call

                        strRequest = "<Command_Cryptic><messageAction><messageFunctionDetails><messageFunction>M</messageFunction></messageFunctionDetails></messageAction><longTextString><textStringDetails>RTSVI</textStringDetails></longTextString></Command_Cryptic>";
                        string strRTSVI = SendCommandCryptically(ttAA, strRequest);

                        var oDocCryptic = new XmlDocument();
                        oDocCryptic.LoadXml(strRTSVI);
                        XmlElement oRootCryptic = oDocCryptic.DocumentElement;
                        string strScreen = oRootCryptic?.SelectSingleNode("longTextString/textStringDetails").InnerText;

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

                        var strToReplace = "</PNR_RetrieveByRecLocReply>";
                        if (!strResponse.Contains(strToReplace))
                            strToReplace = "</PNR_Reply>";

                        strResponse = strResponse.Replace(strToReplace, inSession
                            ? $"<ConversationID>{ConversationID}</ConversationID>{strToReplace}"
                            : $"{strToReplace}");

                        CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead",
                            $"Final response size for version {Version}", strResponse.Length.ToString(CultureInfo.InvariantCulture), ttProviderSystems.LogUUID);
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

                        // Add Conversation ID
                        //strResponse = strResponse.Replace("</OTA_TravelItineraryRS>", $"<ConversationID><![CDATA[{ConversationID}]]></ConversationID>\r\n</OTA_TravelItineraryRS>");

                        if (strResponse.Contains("<Errors>"))
                        {
                            inSession = false;
                        }
                    }
                    else
                    {
                        switch (strMessage)
                        {
                            case "AccessQueue":
                                inSession = false;
                                strResponse = CoreLib.GetNodeInnerText(strResponse, strResponse.Contains("Command_CrypticReply") ? "textStringDetails" : "freeText", false);
                                strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, ttProviderSystems);
                                break;
                            case "ExitQueue":
                                strResponse = CoreLib.GetNodeInnerText(strResponse, strResponse.Contains("Command_CrypticReply") ? "textStringDetails" : "description", false);

                                if (strResponse.Contains("Q/TTL"))
                                    inSession = false;

                                strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, ttProviderSystems);
                                break;
                            case "ItemOnQueue":
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
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.UpdateVirtualCard, ex.Message, ttProviderSystems, "");
            }
            return strResponse;
        }

        public string GetVirtualCardDetails()
        {
            string strResponse;            
            
            //*****************************************************************
            // Transform OTA Get Card Details Request into Native Amadeus Request     *
            //***************************************************************** 
            try
            {
                _tracerID = string.Empty;
                string strRequest = SetRequest($"AmadeusWS_GetVirtualCardDetailsRQ.xsl");
                AmadeusWSAdapter ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");


                //*******************************************************************************
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                //******************************************************************************* 
                strResponse = !inSession ? SendRetrivePNRbyRL(ttAA, strRequest) : SendVirtualCardDetails(ttAA, strRequest);       

                //*****************************************************************
                // Transform Native Amadeus PNRRead Response into OTA Response   *
                //***************************************************************** 
                try
                {
                    Version = "";

                    strResponse = strResponse.Replace("xmlns=\"http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2\"", "");
                    strResponse = strResponse.Replace("xmlns:iata=\"http://www.iata.org/IATA/2007/00/IATA2010.1\"", "");
                    strResponse = strResponse.Replace("xmlns:ota=\"http://www.opentravel.org/OTA/2003/05/OTA2011A\"", "");
                    strResponse = strResponse.Replace("xmlns:etr=\"http://xml.amadeus.com/2010/06/ETR_Types_v4\"", "");
                    strResponse = strResponse.Replace("xmlns:fop=\"http://xml.amadeus.com/2010/06/FOP_Types_v6\"", "");
                    strResponse = strResponse.Replace("xmlns:pay=\"http://xml.amadeus.com/2010/06/PAY_Types_v1\"", "");
                    strResponse = strResponse.Replace("xmlns:pnr=\"http://xml.amadeus.com/2010/06/PNR_Types_v4\"", "");
                    strResponse = strResponse.Replace("xmlns:tkt=\"http://xml.amadeus.com/2010/06/PayIssueTypes_v1\"", "");
                    strResponse = strResponse.Replace("xmlns:qt=\"http://xml.amadeus.com/2010/06/QuotationTypes_v2\"", "");
                    strResponse = strResponse.Replace("xmlns:ttr=\"http://xml.amadeus.com/2010/06/TTR_Types_v3\"", "");
                    strResponse = strResponse.Replace("xmlns:ama_ct=\"http://xml.amadeus.com/2010/06/Types_v3\"", "");

                    strResponse = strResponse.Replace("iata:", "");
                    strResponse = strResponse.Replace("ota:", "");
                    strResponse = strResponse.Replace("etr:", "");
                    strResponse = strResponse.Replace("fop:", "");
                    strResponse = strResponse.Replace("pay:", "");
                    strResponse = strResponse.Replace("pnr:", "");
                    strResponse = strResponse.Replace("tkt:", "");
                    strResponse = strResponse.Replace("qt:", "");
                    strResponse = strResponse.Replace("ttr:", "");
                    strResponse = strResponse.Replace("ama_ct:", "");

                    if (inSession)
                        strResponse = strResponse.Replace("</AMA_PAY_GetVirtualCardDetailsRS>", $"<ConversationID>{ConversationID}</ConversationID></AMA_PAY_GetVirtualCardDetailsRS>");

                    CoreLib.SendTrace(ttProviderSystems.UserID, "PAY_VirtualCreditCard", "GetCardDetails", strResponse, ttProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_GetVirtualCardDetailsRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.GetVirtualCardDetails, exx.Message, ttProviderSystems);
            }            
            return strResponse;
        }

        [Obsolete("This Method is not Implemented. This is only a skeleton")]
        public string ManageDBIData()
        {
            string strResponse;
            
            //*****************************************************************
            // Transform OTA Get Card Details Request into Native Amadeus Request     *
            //***************************************************************** 
            try
            {
                _tracerID = string.Empty;
                string strRequest = SetRequest($"AmadeusWS_GetVirtualCardDetailsRQ.xsl");
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //*******************************************************************************
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                //******************************************************************************* 
                strResponse = !inSession ? SendRetrivePNRbyRL(ttAA, strRequest) : SendVirtualCardDetails(ttAA, strRequest);

                //*****************************************************************
                // Transform Native Amadeus PNRRead Response into OTA Response   *
                //***************************************************************** 

                try
                {
                    Version = "";

                    strResponse = strResponse.Replace("xmlns=\"http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2\"", "");
                    strResponse = strResponse.Replace("xmlns:iata=\"http://www.iata.org/IATA/2007/00/IATA2010.1\"", "");
                    strResponse = strResponse.Replace("xmlns:ota=\"http://www.opentravel.org/OTA/2003/05/OTA2011A\"", "");
                    strResponse = strResponse.Replace("xmlns:etr=\"http://xml.amadeus.com/2010/06/ETR_Types_v4\"", "");
                    strResponse = strResponse.Replace("xmlns:fop=\"http://xml.amadeus.com/2010/06/FOP_Types_v6\"", "");
                    strResponse = strResponse.Replace("xmlns:pay=\"http://xml.amadeus.com/2010/06/PAY_Types_v1\"", "");
                    strResponse = strResponse.Replace("xmlns:pnr=\"http://xml.amadeus.com/2010/06/PNR_Types_v4\"", "");
                    strResponse = strResponse.Replace("xmlns:tkt=\"http://xml.amadeus.com/2010/06/PayIssueTypes_v1\"", "");
                    strResponse = strResponse.Replace("xmlns:qt=\"http://xml.amadeus.com/2010/06/QuotationTypes_v2\"", "");
                    strResponse = strResponse.Replace("xmlns:ttr=\"http://xml.amadeus.com/2010/06/TTR_Types_v3\"", "");
                    strResponse = strResponse.Replace("xmlns:ama_ct=\"http://xml.amadeus.com/2010/06/Types_v3\"", "");

                    strResponse = strResponse.Replace("iata:", "");
                    strResponse = strResponse.Replace("ota:", "");
                    strResponse = strResponse.Replace("etr:", "");
                    strResponse = strResponse.Replace("fop:", "");
                    strResponse = strResponse.Replace("pay:", "");
                    strResponse = strResponse.Replace("pnr:", "");
                    strResponse = strResponse.Replace("tkt:", "");
                    strResponse = strResponse.Replace("qt:", "");
                    strResponse = strResponse.Replace("ttr:", "");
                    strResponse = strResponse.Replace("ama_ct:", "");

                    if (inSession)
                        strResponse = strResponse.Replace("</AMA_PAY_VirtualCreditCardRS>", $"<ConversationID>{ConversationID}</ConversationID></AMA_PAY_VirtualCreditCardRS>");

                    CoreLib.SendTrace(ttProviderSystems.UserID, "PAY_VirtualCreditCard", "GetCardDetails", strResponse, ttProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_GetVirtualCardDetailsRS.xsl");
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

                strResponse = strResponse.Replace("</PAY_GetVirtualCardDetailsRS>", $"<ConversationID>{ConversationID}</ConversationID></PAY_GetVirtualCardDetailsRS>");
            }
            catch (Exception exx)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.GetVirtualCardDetails, exx.Message, ttProviderSystems);
            }
            return strResponse;
        }
    }
}
