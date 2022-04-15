using System;
using System.Linq;
using System.Text;
using System.Xml;
using TripXMLMain;

namespace Galileo
{
    public class PNRServices : GalileoBase
    {

        public string PNRRead()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA PNRRead Request into Native Galileo Request     *
            // ***************************************************************** 
            try
            {
                var requestTime = DateTime.Now;

                #region Get Tracer ID

                string strRequest = SetRequest("Galileo_PNRReadRQ.xsl");

                if (string.IsNullOrEmpty(Version))
                    Version = "v03_";

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                #endregion

                #region Send Transformed Request to the Galileo Adapter and Getting Native Response


                // CoreLib.SendTrace(ProviderSystems.UserID, "Galileo", "PNR Read Request", strRequest, ProviderSystems.LogUUID)
                strResponse = ttGA.SendMessage(strRequest, ConversationID);
                var strMessage = $"{strRequest}\r\n{strResponse}";


                #endregion

                #region Read History of PNR throught *HI
                string strDisplayHI = GetHistory(ConversationID, ttGA);
                strResponse = strResponse.Replace("</PNRBFManagement_53>", $"{strDisplayHI}<ConversationID>{ConversationID}</ConversationID></PNRBFManagement_53>");
                #endregion

                #region Transform Native Worldspan PNRRead Response into OTA Response

                try
                {
                    if (strResponse.Length > 1500)
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response I", strResponse.Substring(0, (int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response II", strResponse.Substring((int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                    }
                    else
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response I", strResponse, ProviderSystems.LogUUID);
                    }

                    var tagToReplace = strResponse.Contains("</PNRBFManagement_17>")
                        ? "</PNRBFManagement_17>"
                        : "</PNRBFManagement_53>";

                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_PNRReadRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }

                    if (ProviderSystems.LogNative)
                    {
                        TripXMLTools.TripXMLLog.LogMessage("PNRRead", ref strMessage, requestTime, DateTime.Now, "Native", ProviderSystems.Provider, ProviderSystems.System, ProviderSystems.UserName);
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRRead, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string PNRCancel()
        {
            string strResponse;

            try
            {
                var requestTime = DateTime.Now;

                // *****************************************************************
                // Transform OTA PNRCancel Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_PNRCancelRQ.xsl");

                var oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                var oRoot = oDoc.DocumentElement;
                var strRecLocator = oRoot.SelectSingleNode("PNRBFRetrieveMods/PNRAddr/RecLoc").InnerText;

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // Create Session
                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                // ************************************************************
                // Send Transformed Request PNR Read to the Galileo Adapter  *
                // ************************************************************                 
                strResponse = ttGA.SendMessage(strRequest, ConversationID);
                var strMessage = $"{strRequest}\r\n{strResponse}";

                // ***************************************
                // Check for End Transaction Warnings    
                // ***************************************

                if (strResponse.Contains("<ErrSeverityInd>W</ErrSeverityInd>"))
                {
                    // *******************************************************************
                    // Send Transformed Request End Transaction to the Galileo Adapter  *
                    // ******************************************************************* 
                    strRequest = $"<PNRBFManagement_17>{oRoot.SelectSingleNode("EndTransactionMods")?.OuterXml}</PNRBFManagement_17>";
                    // End Transaction Request
                    ttGA.SendCrypticMessage("ER", ConversationID);
                    strResponse = ttGA.SendMessage(strRequest, ConversationID);
                }

                // *****************************************************************
                // Transform Native Galileo PNRCancel Response into OTA Response   *
                // ***************************************************************** 
                try
                {

                    var tagToReplace = strResponse.Contains("</PNRBFManagement_17>")
                        ? "</PNRBFManagement_17>"
                        : "</PNRBFManagement_53>";

                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_PNRCancelRS.xsl");

                    if (strResponse.Contains("<UniqueID ID=\"\""))
                    {
                        strResponse = strResponse.Replace("<UniqueID ID=\"\"", $"<UniqueID ID=\"{strRecLocator}\"");
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
                        ttGA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }

                    if (ProviderSystems.LogNative)
                    {
                        TripXMLTools.TripXMLLog.LogMessage("PNRCancel", ref strMessage, requestTime, DateTime.Now, "Native", ProviderSystems.Provider, ProviderSystems.System, ProviderSystems.UserName);
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRCancel, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string PNRReprice()
        {
            string strResponse;
            try
            {
                bool bStoreFare = false;

                #region Transform OTA PNRRead Request into Native Sabre Request

                string strRequest = SetRequest("Galileo_PNRRePriceRQ.xsl");

                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                var oRoot = oDoc.DocumentElement;

                string strRead = ""; // oRoot.SelectSingleNode("PNRRead").InnerXml;
                string strPriceCombined = ""; //oRoot.SelectSingleNode("PriceCombined").InnerXml;
                string strRedisplay = strRequest; // oRootT.SelectSingleNode("PNRRedisplay").InnerXml

                if (oRoot.SelectSingleNode("@StoreFare") != null)
                    bStoreFare = Convert.ToBoolean(oRoot.SelectSingleNode("@StoreFare")?.InnerText);

                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                #endregion

                #region Send Transformed Request to the Sabre Adapter and Getting Native Response

                try
                {
                    string strReadResp = ttGA.SendMessage(strRequest, ConversationID);
                    strReadResp = strReadResp.Replace(" xmlns=\"http://www.opentravel.org/OTA_RS/2003/05\"", "").Replace(" Version=\"2.0.0\"", "");

                    if (!bStoreFare)
                    {

                        string strRepriceReq = CoreLib.TransformXML(strReadResp.Replace("</PNRBFManagement_53>", $"{Request}</PNRBFManagement_53>"), XslPath, $"{Version}Galileo_PNRRePriceRQ.xsl");

                        string strRepriceResp;
                        if (strRepriceReq.Contains("Error Type=\"Galileo\""))
                        {
                            strRepriceResp = strRepriceReq;
                        }
                        else
                        {
                            strRepriceResp = ttGA.SendMessage(strRepriceReq, ConversationID);
                            if (strRepriceResp.Contains("NO COMBINABLE FARES FOR CLASS USED"))
                            {
                                strPriceCombined = strPriceCombined.Replace("<NameSelect>NS</NameSelect>", ""); //strPaxCombined
                                strRepriceResp = ttGA.SendMessage(strPriceCombined, ConversationID);
                                strRepriceResp = strRepriceResp.Replace("<OTA_AirPriceRS Version=\"2.4.0\">", "").Replace("</OTA_AirPriceRS>", "");
                            }
                        }



                        strResponse = strReadResp.Replace("</PNRBFManagement_53>", $"<OTA_AirPriceRS>{strRepriceResp}</OTA_AirPriceRS><ConversationID>{ConversationID}</ConversationID></PNRBFManagement_53>");
                        CoreLib.SendTrace(ProviderSystems.UserID, "wsPNRReprice", "RePrice", strResponse, ProviderSystems.LogUUID);
                    }
                    else
                    {
                        string strRePriceRQ = strReadResp.Replace("</PNRBFManagement_53>", $"{Request}</PNRBFManagement_53>");
                        string strRepriceStoreReq = CoreLib.TransformXML(strRePriceRQ, XslPath, $"{Version}Galileo_PNRRePriceRQ.xsl");

                        string strRepriceResp = ttGA.SendMessage(strRepriceStoreReq, ConversationID);
                        CoreLib.SendTrace(ProviderSystems.UserID, "wsPNRReprice", "RePrice", strRepriceResp, ProviderSystems.LogUUID);

                        #region Save PNR

                        string strER = strRedisplay.Replace("<PNRBFRetrieveMods><CurrentPNR /></PNRBFManagement_53>", "<EndTransactionMods><EndTransactRequest><ETInd>R</ETInd><RcvdFrom>TRIPXML</RcvdFrom></EndTransactRequest></EndTransactionMods>");
                        strRepriceResp = ttGA.SendMessage(strER, ConversationID);
                        if (strRepriceResp.Contains("<Text>CHECK CONTINUITY SEGMENT"))
                        {
                            strER = strRedisplay.Replace("</PNRBFRetrieveMods>", "</PNRBFRetrieveMods><EndTransactionMods><EndTransactRequest><ETInd>R</ETInd><RcvdFrom>TRIPXML</RcvdFrom></EndTransactRequest></EndTransactionMods>");
                            strRepriceResp = ttGA.SendMessage(strER, ConversationID);
                        }

                        #endregion

                        strResponse = strReadResp.Replace("</PNRBFManagement_53>", $"<OTA_AirPriceRS>{strRepriceResp}</OTA_AirPriceRS><ConversationID>{ConversationID}</ConversationID></PNRBFManagement_53>");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("PNRReprice", ex);
                }
                #endregion

                #region Transform Native Sabre PNRRead Response into OTA Response
                try
                {
                    if (strResponse.Length > 5500)
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "wsPNRReprice", "Final response I", strResponse.Substring(0, (int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                        CoreLib.SendTrace(ProviderSystems.UserID, "wsPNRReprice", "Final response II", strResponse.Substring((int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                    }
                    else
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "wsPNRReprice", "Final response I", strResponse, ProviderSystems.LogUUID);
                    }

                    var tagToReplace = strResponse.Contains("</PNRBFManagement_17>")
                        ? "</PNRBFManagement_17>"
                        : "</PNRBFManagement_53>";

                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_PNRRepriceRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRReprice, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string Queue()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA Queue Request into Native Galileo Request     *
            // ***************************************************************** 

            try
            {


                string strRequest = SetRequest("Galileo_QueueRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");


                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                // check if action is queue count or anything else
                if (strRequest.Contains("<Action>QCT</Action>"))
                    inSession = false;

                strResponse = ttGA.SendMessage(strRequest, ConversationID);


                // *****************************************************************
                // Transform Native Galileo Queue Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = strResponse.Contains("</PNRBFManagement_17>") ? "</PNRBFManagement_17>" : "</PNRBFManagement_53>";

                    if (strResponse.Contains("QueueProcessing_16"))
                    {
                        tagToReplace = "</QueueProcessing_16>";
                    }
                    else if (strResponse.Contains("PoweredQueue_CountTotalReply"))
                    {
                        tagToReplace = "</PoweredQueue_CountTotalReply>";
                    }
                    else if (strResponse.Contains("PoweredQueue_ListReply"))
                    {
                        tagToReplace = "</PoweredQueue_ListReply>";
                    }
                    else if (strResponse.Contains("PoweredQueue_MoveItemReply"))
                    {
                        tagToReplace = "</PoweredQueue_MoveItemReply>";
                    }
                    else if (strResponse.Contains("PoweredQueue_RemoveItemReply"))
                    {
                        tagToReplace = "</PoweredQueue_RemoveItemReply>";
                    }

                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_QueueRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Queue, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string QueueRead()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA Queue Request into Native Galileo Request     *
            // ***************************************************************** 
            try
            {
                bool queueAccess = false;
                bool queueRemove = false;
                bool queueKeep = false;
                bool queueExit = false;

                //if (string.IsNullOrEmpty(Version))
                Version = "";

                string strRequest = SetRequest("Galileo_QueueReadRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                var ttGA = SetAdapter();
                bool inSession = true; /* This service will always return Session. Will either create one or return existing */
                SetConversationID(ttGA);

                try
                {
                    if (strRequest.Contains("<Action>Q</Action>"))
                    {
                        queueAccess = true;
                    }
                    else
                    {
                        if (strRequest.Contains("<Action>QR</Action>"))
                        {
                            queueRemove = true;
                        }
                        else if (strRequest.Contains("<Cryptic>I</Cryptic>"))
                        {
                            queueKeep = true;
                        }

                    }

                    // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                    strResponse = queueKeep
                        ? ttGA.SendCrypticMessage("I", ConversationID)
                        : ttGA.SendMessage(strRequest, ConversationID);

                    // Check PNR or Errors in Native Response
                    if (queueAccess & strResponse.Contains("<PNRBFRetrieve>") | queueKeep | queueRemove & strResponse.Contains("<PNRBFRetrieve>"))
                    {

                        // Send PNR Redisplay
                        strRequest = "<PNRBFManagement_53><PNRBFRetrieveMods><CurrentPNR/></PNRBFRetrieveMods><FareRedisplayMods><DisplayAction><Action>D</Action></DisplayAction><FareNumInfo><FareNumAry><FareNum>1</FareNum></FareNumAry></FareNumInfo></FareRedisplayMods></PNRBFManagement_53>";
                        strResponse = ttGA.SendMessage(strRequest, ConversationID);

                        #region Read History of PNR throught *HI
                        string strDisplayHI = GetHistory(ConversationID, ttGA);
                        strResponse = strResponse.Replace("</PNRBFManagement_53>", $"{strDisplayHI}</PNRBFManagement_53>");
                        #endregion

                        // Transform PNR Read
                        var tagToReplace = strResponse.Contains("</PNRBFManagement_53>")
                        ? strResponse.Contains("</PNRBFManagement_17>")
                            ? "</PNRBFManagement_17>"
                            : "</PNRBFManagement_53>"
                        : "</QueueProcessing_16>";

                        if (inSession)
                            strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                        CoreLib.SendTrace(ProviderSystems.UserID, "QRead", "Final response", strResponse, ProviderSystems.LogUUID);

                        if (string.IsNullOrEmpty(Version))
                            Version = "v03_";

                        strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_PNRReadRS.xsl");
                    }
                    else if (queueAccess)
                    {
                        inSession = false;
                        strResponse = CoreLib.GetNodeInnerText(strResponse, "Txt", false);
                        if (strResponse == "QUEUE EMPTY")
                        {
                            strResponse = "QUEUE CATEGORY EMPTY";
                        }

                        strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, ProviderSystems);
                    }
                    else if (queueRemove)
                    {
                        inSession = false;
                        strResponse = CoreLib.GetNodeInnerText(strResponse, "Txt", false);
                        if (strResponse == "OFF QUEUE")
                            strResponse = "Queue Empty";

                        strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, ProviderSystems);
                    }
                    else
                    {
                        inSession = false;
                        strResponse = CoreLib.GetNodeInnerText(strResponse, "Txt", false);
                        strResponse = strResponse.Contains("IGNORED") ? "<OTA_TravelItineraryRS Version=\"v03\"><Success/><Warnings><Warning Type=\"Queue\">IGNORED - OFF QUEUE</Warning></Warnings></OTA_TravelItineraryRS>" : modCore.FormatErrorMessage(modCore.ttServices.QueueRead, strResponse, ProviderSystems);
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
                        ttGA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.QueueRead, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        private string GetHistory(string conversationID, GalileoAdapter ttGA)
        {
            var sbH = new StringBuilder("<PNR_HI_INF>");
            try
            {
                string strDisplayHI = ttGA.SendCrypticMessage("*HI", conversationID);
                string strScreen = strDisplayHI.Replace("\r", "\r\n");
                strScreen = strScreen.Replace("\r", "\r\n");
                strDisplayHI = FormatGalileo(strScreen);
                var lstLines = strDisplayHI.Split(new[] { "<Screen>", "<Line>", "</Screen>", "</Line>" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                // Conduct Move Bottom (MB)
                if (lstLines.Last().Contains(")&gt;"))
                {
                    string strDHMore = ttGA.SendCrypticMessage("MB", conversationID);
                    var lstMoreLines = strDHMore.Split(new[] { "<Screen>", "<Line>", "</Screen>", "</Line>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (string line in lstMoreLines)
                    {
                        if (!lstLines.Contains(line))
                        {
                            lstLines.Add(line);
                        }
                    }
                }

                // CRDT- ATS/533W/1G AG WS       0448Z/04MAR
                // CRDT- ATS/533W/1G AG WS       1801Z/26FEB
                // CRDT- ATS/533W/1G AG WS       0513Z/27FEB
                // CRDT- HDQ/    /1G RM UA       0500Z/02MAR
                // CRDT- QSB/3LM3/1G AG 96       1741Z/05MAR

                foreach (string line in lstLines.GetRange(1, lstLines.Count - 1))
                {
                    var strline = line.Trim().Replace(")&gt;", "").Replace("&gt;", "");
                    if (!string.IsNullOrEmpty(strline) && line.Trim().StartsWith("CRDT- "))
                    {
                        var elems = strline.Split(new[] { "-", " ", "/" }, StringSplitOptions.None).ToList();
                        string pcc = elems[3].Trim();
                        sbH.Append($"<Line PCC='{pcc}'>{strline}</Line>");
                    }
                }
            }
            catch (Exception ex)
            {
                sbH.Append($"<Line Error=true>{ex.Message}</Line>");
            }
            finally
            {
                sbH.Append("</PNR_HI_INF>");
            }

            return sbH.ToString();
        }
    }
}