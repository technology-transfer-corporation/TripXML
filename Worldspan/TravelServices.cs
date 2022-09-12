using System;
using System.Linq;
using System.Text;
using System.Xml;
using TripXMLMain;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Worldspan
{
    public class TravelServices : WorldspanBase
    {
        public string TravelBuild()
        {
            string strResponse;
            
            // *******************************************************************
            // Transform OTA Travel Build Request into Native Worldspan Request *
            // ******************************************************************* 

            try
            {
                XmlNode oNode;
                Version = "";
                var strRequest = SetRequest("Worldspan_TravelBuildRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                CoreLib.SendTrace(ProviderSystems.UserID, "ttWorldspanService", "OTA Transformed Request", strRequest,
                    ProviderSystems.LogUUID);
                

                // *************************
                // Get Multiple Requests  *
                // *************************
                var oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                var oRoot = oDoc.DocumentElement;
                var strBPC = oRoot.SelectSingleNode("TTBPC").InnerXml;
                var strRMC = oRoot.SelectSingleNode("TTRMC") != null ? oRoot.SelectSingleNode("TTRMC").InnerXml : "";
                var strUPC = oRoot.SelectSingleNode("TTUPC") != null ? oRoot.SelectSingleNode("TTUPC").InnerXml : "";
                    
                
                // *******************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response*
                // ******************************************************************************* 
                var ttWA = SetAdapter(ProviderSystems);
                strResponse = ttWA.SendMessage(strBPC);
                var strNative = $"{strBPC}{strResponse}";

                // **************************************************
                // Get record locator                              *
                // ************************************************** 
                var oDocResp = new XmlDocument();
                oDocResp.LoadXml(strResponse);
                var oRootResp = oDocResp.DocumentElement;
                var oNodeResp = oRootResp.SelectSingleNode("PNR_RLOC");
                if (oNodeResp != null)
                {
                    // **************************************
                    // check if any SSR and send if yes *
                    // **************************************
                    if (!string.IsNullOrEmpty(strUPC))
                    {
                        oNode = oRoot.SelectSingleNode("TTUPC/UPC7/PNR_RLOC");
                        oNode.InnerText = oNodeResp.InnerText;
                        strRequest = oRoot.SelectSingleNode("TTUPC").InnerXml;

                        strResponse = ttWA.SendMessage(strRequest);
                        strNative += $"{strRequest}{strResponse}";
                    }

                    // **************************************
                    // check if any Remark and send if yes *
                    // **************************************
                    if (!string.IsNullOrEmpty(strRMC))
                    {
                        oNode = oRoot.SelectSingleNode("TTRMC/RMC2/PNR_RLOC");
                        oNode.InnerText = oNodeResp.InnerText;
                        strRequest = oRoot.SelectSingleNode("TTRMC").InnerXml;

                        strResponse = ttWA.SendMessage(strRequest);
                        strNative += $"{strRequest}{strResponse}";
                    }

                    // ********************
                    // Send retrieve PNR *
                    // ******************** 
                    oNode = oRoot.SelectSingleNode("TTDPC/DPC8/REC_LOC");
                    oNode.InnerText = oNodeResp.InnerText;
                    strRequest = oRoot.SelectSingleNode("TTDPC").InnerXml;

                    strResponse = ttWA.SendMessage(strRequest);
                    strNative += $"{strRequest}{strResponse}";
                }

                // ************************************************
                // calculate year in all dates and arrival date  *
                // ************************************************

                try
                {
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;
                    if (oRoot.SelectNodes("AIR_SEGMENT_INFO") != null)
                    {
                        foreach (XmlNode currentONode in oRoot.SelectNodes("AIR_SEGMENT_INFO/AIR_ITEM"))
                        {
                            oNode = currentONode;
                            DateTime dtDepartureDate =
                                Convert.ToDateTime(
                                    $"{oNode.SelectSingleNode("DEP_DATE/DEP_DAY").InnerText}{oNode.SelectSingleNode("DEP_DATE/DEP_MONTH").InnerText}{DateTime.Now.Year}");

                            DateTime dtArrivalDate =
                                Convert.ToDateTime(
                                    $"{oNode.SelectSingleNode("ARR_DATE/ARR_DAY").InnerText}{oNode.SelectSingleNode("ARR_DATE/ARR_MONTH").InnerText}{DateTime.Now.Year}");

                            if (DateTime.Now.DayOfYear > dtDepartureDate.DayOfYear)
                            {
                                dtDepartureDate = dtDepartureDate.AddYears(1);
                            }

                            oNode.SelectSingleNode("DEP_DATE").InnerText =
                                dtDepartureDate.ToString("yyyy-MM-dd");
                            if (DateTime.Now.DayOfYear > dtArrivalDate.DayOfYear)
                            {
                                dtArrivalDate = dtArrivalDate.AddYears(1);
                            }

                            oNode.SelectSingleNode("ARR_DATE").InnerText = dtArrivalDate.ToString("yyyy-MM-dd");
                        }

                        strResponse = oRoot.OuterXml;
                    }

                    // *****************************************************************
                    // Transform Native Worldspan PNRRead Response into OTA Response  *
                    // ***************************************************************** 
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_PNRReadRS.xsl");

                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }

            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.TravelBuild, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string Update()
        {
            string strResponse = "";
            try
            {
                XmlElement oRootTemp = null;
                var ttWA = SetAdapter(ProviderSystems);

                // *******************************
                // Load OTA Modify XML document  *
                // ******************************* 
                var oDoc = new XmlDocument();
                var oDocTemp = new XmlDocument();
                oDoc.LoadXml(Request);
                var oRoot = oDoc.DocumentElement;

                // *******************************
                // Modify PNR - Insert elements *
                // ******************************* 
                //string strErrEvent = "Modify PNR - Insert elements Error.";
                if (oRoot.SelectSingleNode("Position/Element[@Operation='insert']") != null)
                {
                    // *******************************************************************
                    // * Transform OTA Modify Request into Worldspan Native Insert Request *
                    // *******************************************************************
                    
                    var strRequest = $"<UpdateInsert>{Request}</UpdateInsert>";
                    strRequest = CoreLib.TransformXML(strRequest, XslPath, $"{Version}Worldspan_UpdateInsertRQ.xsl");

                    // ********************
                    // Get All Requests  * 
                    // ********************
                    oDocTemp.LoadXml(strRequest);
                    oRootTemp = oDocTemp.DocumentElement;
                    var strRMC = oRootTemp.SelectSingleNode("TTRMC").InnerXml;
                    strResponse = ttWA.SendMessage(strRMC);
                }
                
                // ********************
                // Send retrieve PNR *
                // ******************** 
                if (!strResponse.Contains("<XXW "))
                {
                    var strDPC = oRootTemp.SelectSingleNode("TTDPC").InnerXml;
                    strResponse = ttWA.SendMessage(strDPC);
                }

                // *****************************************************************
                // Transform Native Worldspan Update Response into OTA Response   *
                // ***************************************************************** 
                //strErrEvent = "Worldspan_PNRReadRS.xsl Error.";
                // mstrVersion = "v03_"

                CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response size", strResponse.Length.ToString(), ProviderSystems.LogUUID);
                CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response", strResponse, ProviderSystems.LogUUID);
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_PNRReadRS.xsl");
                
            }
            catch (Exception exx)
            {
                AddLog($"<M>{Request}<BL/>", ProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Update, exx.Message, ProviderSystems);
            }
            return strResponse;
        }

        public string IssueTicketSessioned()
        {
            string strResponse;

            try
            {
                string strRequest = SetRequest("Worldspan_IssueTicketSessionedRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                modCore.TripXMLProviderSystems ttProviderSystems = ProviderSystems;
                ttProviderSystems.Profile = ProviderSystems.ProfileTicketing;
                
                var ttWA = SetAdapter(ttProviderSystems);
                bool inSession = false; //SetConversationID(ttWA);

                strResponse = ttWA.SendMessage(strRequest);
                strResponse = strResponse.Replace("xmlns=\"http://www.opentravel.org/OTA_RS/2003/05\"", "");

                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                var oRoot = oDoc.DocumentElement;
                var pnrNum = oRoot.SelectSingleNode("UniqueID/@ID").InnerText;
                var diNumber = oRoot.SelectSingleNode("Ticketing/FutureTicket/Number").InnerText;
                var addNC = strResponse.Contains("5000 CURRENT FARE DOES NOT MATCH QUOTED FARE - ENTER 4PQ OR 4PQC");
                try
                {
                    if (strResponse.Contains("COMMISSION EXISTS-ADD -OK OPTION TO OVERRIDE") || strResponse.Contains("PROC-COMMISSION DATA") || addNC)
                    {
                        // get pnr from request xml
                        // get future ticket number from request xml
                        // retrieve PNR
                        var strDPC = $"<DPC8><MSG_VERSION>8</MSG_VERSION><REC_LOC>{pnrNum}</REC_LOC><ETR_INF>Y</ETR_INF><ALL_PNR_INF>Y</ALL_PNR_INF><PRC_INF>Y</PRC_INF></DPC8>";

                        ttProviderSystems.Profile = ProviderSystems.ProfileXML;
                        ttWA = SetAdapter(ttProviderSystems);
                        strResponse = ttWA.SendMessage(strDPC, "");
                        strResponse = strResponse.Replace("xmlns=\"http://www.opentravel.org/OTA_RS/2003/05\"", "");
                        strResponse = CoreLib.TransformXML(strResponse, XslPath, "v03_Worldspan_PNRReadRS.xsl");


                        // get DI line in pnr that matches future ticket number
                        var oDocResp = new XmlDocument();
                        oDocResp.LoadXml(strResponse);
                        var oRootResp = oDocResp.DocumentElement;

                        // Due to fact that in Transfered XML RPH starts at index 1.
                        var oNode = oRootResp.SelectSingleNode($"TravelItinerary/TPA_Extensions/FuturePriceInfo[@RPH='{Convert.ToDouble(diNumber) + 1d}']");

                        // get commission from DI line
                        var strComm = oNode.InnerText.Substring(oNode.InnerText.IndexOf("#K", StringComparison.Ordinal));
                        if (strComm.Substring(1).Contains("#"))
                        {
                            strComm = strComm.Substring(0, strComm.Substring(1).IndexOf("#", StringComparison.Ordinal) + 1);
                        }

                        if (strComm.Contains("$"))
                        {
                            if (strComm.Equals("#K$0"))
                            {
                                strComm = strComm.Replace("$", "");
                            }
                        }

                        // create tkt entry with commission and -OK
                        var strTkt = !addNC
                            ? $"EZEI#DI{diNumber}{strComm}-OK"
                            : $"EZEI#DI{diNumber}{strComm}#NC";

                        // send to ticket
                        ttProviderSystems.Profile = ProviderSystems.ProfileCryptic;
                        ttWA = SetAdapter(ttProviderSystems);
                        inSession = SetConversationID(ttWA);
                        ttWA.SendCryptic($"*{pnrNum}");
                        strResponse = ttWA.SendCryptic(strTkt);

                        // If (strResponse.Contains("LOG OR MANUALLY SUPPLY INVOICE NUMBER OR ENTER *IX TO SUPPRESS")) Then
                        // strResponse = ttWA.SendCryptic(strTkt, conversationID)
                        // End If

                    }

                    if (strResponse.Contains("ITEM") & strResponse.Contains("GROSS") & strResponse.Contains("PSGR NAME") & strResponse.Contains("INVOICE"))
                    {
                        if (!(strResponse.Contains("Warning") || strResponse.Contains("Error") || strResponse.Contains("PROC-COMMISSION DATA")))
                        {
                            // *DI
                            // 2GXIYP::HOST Rep ------------------------------------------ 
                            // 2:          GXIYP() : HOST Rep
                            // DI-    *. #RL#DR#WL#TC#V#FT#*P#*I#SP
                            // DI-TK  1. #N1.1/2.1#K5 -1KROT/ALISA -1KROT/GLEB
                            // T/06APR0818  1P/0G3/RO*E5557979075799-800 I635731 *I N          1.1/2.1
                            // DI-TK  2. #N3.1/4.1#K5 -1KROT/OLEG -1KROT/DARIA
                            // T/06APR0758  1P/0G3/RO*E5557979075792-793 I635730 *I N          3.1/4.1
                            // &gt;
                            // 2GXIYP::HOST Rep ------------------------------------------ 
                            // -- Retrive all ticketing information ----------------------
                            if (ttProviderSystems.Profile != ProviderSystems.ProfileCryptic)
                            {
                                ttProviderSystems.Profile = ProviderSystems.ProfileCryptic;
                                ttWA = SetAdapter(ttProviderSystems);
                                inSession = SetConversationID(ttWA);
                            }    

                            ttWA.SendCryptic($"*{pnrNum}");
                            strResponse = ttWA.SendCryptic($"*DI{diNumber}");
                            List<string> strDIRes = strResponse.Split(new[] { "<Screen>","<Line>", "</Screen>","</Line>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (string cmd in strDIRes)
                            {
                                ttWA.SendCryptic(cmd, pnrNum, "");
                            }                            
                            ttWA.SendCryptic("ER", pnrNum, "");
                            strResponse = strResponse.Replace("<Screen>", $"<Screen><Line>{pnrNum}</Line>");
                            // -----------------------------------------------------------
                        }
                    }

                    var strToReplace = "</OTA_AirDemandTicketRS>";
                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID>{ConversationID}</ConversationID>{strToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_IssueTicketSessionedRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttWA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.IssueTicketSessioned, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string UpdateSessioned()
        {
            string strResponse = "";
         
            try
            {
                string strRequest = Request;
                // ProviderSystems.Profile = ProviderSystems.ProfileCryptic
                var ttWA = SetAdapter(ProviderSystems);

                // *******************************
                // Load OTA Modify XML document  *
                // ******************************* 
                var oDoc = new XmlDocument();
                var oDocTemp = new XmlDocument();
                oDoc.LoadXml(Request);
                var oRoot = oDoc.DocumentElement;
                
                // *******************************
                // Modify PNR - Insert elements *
                // ******************************* 
                string strErrEvent = "Modify PNR - Insert elements Error.";
                XmlElement oRootTemp = null;
                if (oRoot.SelectSingleNode("Position/Element[@Operation='insert']") != null)
                {
                    // *******************************************************************
                    // * Transform OTA Modify Request into Worldspan Native Insert Request *
                    // *******************************************************************
                    strRequest = $"<UpdateInsert>{Request}</UpdateInsert>";
                    CoreLib.SendTrace(ProviderSystems.UserID, "UpdateInsert", "Request", strRequest,
                        ProviderSystems.LogUUID);
                    strRequest = CoreLib.TransformXML(strRequest, XslPath, $"{Version}Worldspan_UpdateInsertRQ.xsl");


                    // ********************
                    // Get All Requests  * 
                    // ********************
                    oDocTemp.LoadXml(strRequest);
                    oRootTemp = oDocTemp.DocumentElement;
                    if (oRootTemp.SelectSingleNode("TTRMC") != null)
                    {
                        var strRMC = oRootTemp.SelectSingleNode("TTRMC").InnerXml;
                        strResponse = ttWA.SendMessage(strRMC);
                    }

                    if (oRootTemp.SelectSingleNode("TTUPC") != null)
                    {
                        foreach (XmlNode oNode in oRootTemp.SelectSingleNode("TTUPC"))
                            strResponse = ttWA.SendMessage(oNode.OuterXml);
                    }

                }

                // *******************************
                // Modify PNR - Delete elements *
                // ******************************* 
                strErrEvent = "Modify PNR - Delete elements Error.";
                if (oRoot.SelectSingleNode("Position/Element[@Operation='delete']") != null)
                {
                    // ********************************
                    // * Build PNR Retrieve xml msg   * 
                    // ********************************
                    
                    strRequest = $"<UpdateDelete>{Request}</UpdateDelete>";
                    strRequest = CoreLib.TransformXML(strRequest, XslPath, $"{Version}Worldspan_UpdateDeleteRQ.xsl");
                    
                    // **************************************
                    // * Send Worldspan Native Delete Request *
                    // **************************************
                    oDocTemp.LoadXml(strRequest);
                    oRootTemp = oDocTemp.DocumentElement;
                    var strUPC = oRootTemp.SelectSingleNode("TTUPC").InnerXml;
                    strResponse = ttWA.SendMessage(strUPC);
                }
                
                // ********************
                // Send retrieve PNR *
                // ******************** 
                if (!strResponse.Contains("<XXW "))
                {
                    //Check for correct object usage
                    var strDPC = oRootTemp.SelectSingleNode("TTDPC").InnerXml;
                    
                    strResponse = ttWA.SendMessage(strDPC);
                    
                    
                }

                // *******************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response  *
                // ******************************************************************************* 
                bool inSession = false;
                try
                {
                    var tripXmlProviderSystems = ProviderSystems;
                    tripXmlProviderSystems.Profile = ProviderSystems.ProfileCryptic;
                    ttWA = SetAdapter(tripXmlProviderSystems);
                    inSession = SetConversationID(ttWA);
                    
                    if (!string.IsNullOrEmpty(strResponse) & !strResponse.Contains("no session configured with name ") &
                        !strResponse.Contains("NO BRIDGE BRANCH") & !strResponse.Contains("SECURED PNR"))
                    {
                        // If strResponse.Contains("EQV_BAS_FAR_CUR_COD") Then
                        // *******************************************************************************
                        // Send 4* Command in order to retrive more detqailed TST information          *
                        // ******************************************************************************* 
                        try
                        {
                            oDoc = new XmlDocument();
                            oDoc.LoadXml(Request);
                            oRoot = oDoc.DocumentElement;
                            var pnrNum = oRoot.SelectSingleNode("UniqueID/@ID").InnerText;
                            
                            // CoreLib.SendTrace(ttProviderSystems.UserID, "WorldspanCommand", "4*", "", ttProviderSystems.LogUUID)
                            string pRead = ttWA.SendCryptic($"*{pnrNum}");

                            bool bEMD = pRead.Contains("EMDL");
                            string emdDisplay = bEMD ? ttWA.SendCryptic("EMDL(") : "";

                            if (!strResponse.Contains("SECURED PNR"))
                            {
                                #region 4*

                                string str4Display = ttWA.SendCryptic("4*");
                                var lstLines = str4Display
                                    .Split(new string[] {"<Screen>", "<Line>", "</Screen>", "</Line>"},
                                        StringSplitOptions.RemoveEmptyEntries).ToList();
                                // Conduct Move Down (MD)
                                if (lstLines.Last().Contains(")&gt;"))
                                {
                                    string str4More = ttWA.SendCryptic("MD");
                                    var lstMoreLines = str4More
                                        .Split(new string[] {"<Screen>", "<Line>", "</Screen>", "</Line>"},
                                            StringSplitOptions.RemoveEmptyEntries).ToList();
                                    foreach (string line in lstMoreLines)
                                    {
                                        if (!lstLines.Contains(line))
                                        {
                                            lstLines.Add(line);
                                        }
                                    }
                                }

                                var sb4 = new StringBuilder("<PNR_4_INF>");
                                foreach (string line in lstLines)
                                {
                                    var strLine = line.Trim().Replace(")&gt;", "").Replace("&gt;", "");
                                    if (!string.IsNullOrEmpty(line))
                                    {
                                        sb4.Append($"<Line>{strLine}</Line>");
                                    }
                                }

                                sb4.Append("</PNR_4_INF>");
                                strResponse = strResponse.Replace("</DPW8>", $"{sb4}</DPW8>");
                                #endregion

                                #region EMDL
                                if (bEMD)
                                {
                                    var emdLines = emdDisplay.Split(new string[] { "<Screen>", "<Line>", "</Screen>", "</Line>" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                                    if (emdLines.Last().Contains(")&gt;"))
                                    {
                                        string str4More = ttWA.SendCryptic("MD");
                                        var lstMoreLines = str4More.Split(new string[] { "<Screen>", "<Line>", "</Screen>", "</Line>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                        foreach (string line in lstMoreLines)
                                        {
                                            if (!emdLines.Contains(line))
                                            {
                                                emdLines.Add(line);
                                            }
                                        }
                                    }
                                    var sbEMD = new StringBuilder("<PNR_EMD_INF>");

                                    //EMDL - ELECTRONIC MISCELLANEOUS DOCUMENT LIST
                                    //  1.LA 0458302220183
                                    //MARKOVA / ALLA
                                    //          I 29APR22  181139 Z
                                    //***** END OF LIST *****                    

                                    foreach (string line in emdLines)
                                    {
                                        if (!string.IsNullOrEmpty(line))
                                        {
                                            if (line.Contains("END OF LIST"))
                                                break;

                                            if (line.Contains("EMDL - ELECTRONIC MISCELLANEOUS DOCUMENT LIST"))
                                                continue;

                                            var bStart = Regex.IsMatch(line, "\\d+\\.\\s*[A-Z]{2}\\s(\\d+)");

                                            if (bStart)
                                            {
                                                var index = emdLines.IndexOf(line);
                                                var trElem = line.Trim().Replace(")&gt;", "").Replace("&gt;", "").Split(new[] { ".", " " }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                                var trNum = trElem[2];
                                                var strLine = $"{emdLines[index + 1].Trim()} {emdLines[index + 2].Trim()}";
                                                sbEMD.Append($"<Line ID=\"{trElem[0]}\" EMD=\"{trNum}\">{strLine}</Line>");
                                            }
                                        }
                                    }

                                    sbEMD.Append("</PNR_EMD_INF>");
                                    strResponse = strResponse.Replace("</DPW8>", $"{sbEMD}</DPW8>");
                                }
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error Transforming Native Response", ex);
                        }
                        finally
                        {
                            if (!inSession)
                            {
                                ttWA.CloseSession();
                                ConversationID = "";
                            }

                            tripXmlProviderSystems.Profile = ProviderSystems.ProfileXML;
                            ProviderSystems = tripXmlProviderSystems;
                        }
                        // End If
                    }

                    // *****************************************************************
                    // Transform Native Worldspan Update Response into OTA Response   *
                    // ***************************************************************** 
                    strErrEvent = "Worldspan_PNRReadRS.xsl Error.";
                    Version = "v03";
                    CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response size", strResponse.Length.ToString(), ProviderSystems.LogUUID);
                    if (strResponse.Length > 3500)
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response I",
                            strResponse.Substring(0, (int) Math.Round(strResponse.Length / 2d)),
                            ProviderSystems.LogUUID);
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response II",
                            strResponse.Substring((int) Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                    }
                    else
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response I", strResponse,
                            ProviderSystems.LogUUID);
                    }
                    
                    if (inSession)
                        strResponse = strResponse.Replace("<ConversationID>NONE</ConversationID>", $"<ConversationID>{ConversationID}</ConversationID>");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_PNRReadRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttWA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.UpdateSessioned, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        /// <summary>
    /// Credit Card Authorization Process
    /// </summary>
    /// <returns></returns>
        public string Authorization()
        {
            string strResponse;
            try
            {

                string strRequest = SetRequest( "Worldspan_AuthorizationRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                modCore.TripXMLProviderSystems ttProviderSystems = ProviderSystems;
                ttProviderSystems.Profile = ProviderSystems.ProfileXML;
                WorldspanAdapter ttWA = SetAdapter(ttProviderSystems);

                strResponse = ttWA.SendMessage(strRequest);
                strResponse = strResponse.Replace("xmlns=\"http://www.opentravel.org/OTA_RS/2003/05\"", "");
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_AuthorizationRS.xsl");
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Authorization, ex.Message, ProviderSystems);
            }

            return strResponse;
        }
    }
}