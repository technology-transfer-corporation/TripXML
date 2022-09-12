using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using TripXMLMain;
using System.Text.RegularExpressions;

namespace Worldspan
{
    public class PNRServices : WorldspanBase
    {
        public PNRServices()
        {
            ConversationID = "";
            Request = "";
        }

        public string Queue()
        {

            string strResponse = String.Empty;
            // *****************************************************************
            // Transform OTA Queue Request into Native Worldspan Request     *
            // ***************************************************************** 
            try
            {
                string strRequest = SetRequest("Worldspan_QueueRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                var ttProviderSystems = ProviderSystems;
                ttProviderSystems.Profile = ProviderSystems.ProfileCryptic;
                // *******************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response  *
                // ******************************************************************************* 
                var ttWA = SetAdapter(ttProviderSystems);
                bool inSession = SetConversationID(ttWA);
                var strToReplace = "";

                if (Request.Contains("ListQueue"))
                {

                    strResponse = ttWA.SendCryptic(strRequest) ?? string.Empty;

                    if (strResponse.Contains(")"))
                    {
                        int i = 0;
                        string strResponseQL = strResponse;
                        while (i < 10 && strResponse.Contains(")"))
                        {
                            strResponse = ttWA.SendCryptic("MD");
                            strResponseQL += strResponse;
                            i++;
                        }

                        strResponse = strResponseQL;
                    }

                    strResponse = $"<ListQueue>{strResponse}</ListQueue>";
                    strToReplace = "</ListQueue>";
                }
                else if (Request.Contains("PlaceQueue"))
                {
                    // send *pnrloc
                    strResponse = ttWA.SendCryptic(strRequest.Substring(0, 7));
                    // If (Not strResponse.Contains("INVALID") AndAlso Not strResponse.Contains("INVLD ADDRESS")) Then
                    if (strResponse.Contains(strRequest.Substring(1, 6)))
                    {
                        strResponse = ttWA.SendCryptic(strRequest.Substring(7));
                    }

                    strResponse = $"<PlaceQueue>{strResponse}</PlaceQueue>";
                    inSession = false;
                    strToReplace = "</PlaceQueue>";
                }
                else if (Request.Contains("RemoveQueue"))
                {
                    // send *pnrloc
                    strResponse = ttWA.SendCryptic(strRequest.Substring(0, 7));
                    // If (Not strResponse.Contains("INVALID") AndAlso Not strResponse.Contains("INVLD ADDRESS")) Then
                    if (strResponse.Contains(strRequest.Substring(1, 6)))
                    {
                        strResponse = ttWA.SendCryptic(strRequest.Substring(7));
                    }

                    strResponse = $"<RemoveQueue>{strResponse}</RemoveQueue>";
                    inSession = false;
                    strToReplace = "</RemoveQueue>";
                }


                // *******************************************************************************
                // check if message is queue lists and if need need to scroll the response      *
                // ******************************************************************************* 
                try
                {
                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace,
                            $"<ConversationID>{ConversationID}</ConversationID>{strToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_QueueRS.xsl");

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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Queue, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        private class flSegsData
        {
            public string segNum;
            public string depCity;
            public string arrCity;
            public bool processed = false;
        }

        public string PNRRead()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA PNRRead Request into Native Worldspan Request     *
            // ***************************************************************** 
            try
            {
                var ttProviderSystems = ProviderSystems;
                ttProviderSystems.Profile = ProviderSystems.ProfileXML;
                var ttWA = SetAdapter(ttProviderSystems);
                string strRequest = SetRequest("Worldspan_PNRReadRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");


                // *******************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response  *
                // *******************************************************************************                                 
                bool inSession = false; //SetConversationID(ttWA);
                strResponse = ttWA.SendMessage(strRequest);

                var agentCommResp = string.Empty;

                if (!string.IsNullOrEmpty(strResponse)
                    && !strResponse.Contains("no session configured with name ") && !strResponse.Contains("UTR PNR INVALID ADDRESS")
                    && !strResponse.Contains("NO BRIDGE BRANCH") & !strResponse.Contains("SECURED PNR"))
                {
                    // If strResponse.Contains("EQV_BAS_FAR_CUR_COD") Then
                    // *******************************************************************************
                    // Send 4* Command in order to retrive more detqailed TST information          *
                    // ******************************************************************************* 
                    try
                    {
                        var oDoc = new XmlDocument();
                        oDoc.LoadXml(Request);
                        var oRoot = oDoc.DocumentElement;
                        var pnrNum = oRoot.SelectSingleNode("UniqueID/@ID").InnerText;

                        var xResp = new XmlDocument();
                        xResp.LoadXml(strResponse);

                        try
                        {
                            var ticketNums = xResp.SelectNodes("//DPW8/ETR_INF/ETR_TIC_INF[CPN_INF/E_TIC_STA_COD = 'O']/TIC_NUM").Cast<XmlNode>()
                                .Select(x => x.InnerText).ToList();
                            //filter conjunction
                            var ticketSSR = ticketNums.SelectMany(tk => xResp.SelectNodes($"//SSR_INF/SSR_ITM[contains(SSR_TXT, {tk}) and contains(SSR_TXT, '/') and contains(SSR_TXT, '-')]/SSR_TXT").Cast<XmlNode>().Select(x => x.InnerText).ToList()).ToList();
                            var conjTks = ticketSSR.Select(s =>
                            {
                                var det = s.Split(".C/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                return new KeyValuePair<string, string>(det[0], det[2]);
                            }).GroupBy(x => x.Key).Select(y => y.First()).ToList();

                            ticketNums.RemoveAll(t => conjTks.Any(c => t.EndsWith(c.Value.Split('-').Last())));

                            foreach (var ticketNode in ticketNums)
                            {
                                var dhReq = $"<DPC8><REC_LOC>{pnrNum}</REC_LOC><DOC_HIS><REC_LOC>{pnrNum}</REC_LOC><DOC_NUM>{ticketNode}</DOC_NUM></DOC_HIS></DPC8>";
                                agentCommResp += ttWA.SendMessage(dhReq).Replace("</DOC_HIS></DPW8>", "").Replace("<DPW8><DOC_HIS>", "");
                            }
                            if (!string.IsNullOrEmpty(agentCommResp))
                            {
                                agentCommResp = "<PNR_DHT_INF>" + agentCommResp + "</PNR_DHT_INF>";
                            }
                        }
                        catch { }

                        // send to ticket
                        ttProviderSystems.Profile = ProviderSystems.ProfileCryptic;
                        ttWA = SetAdapter(ttProviderSystems);
                        inSession = SetConversationID(ttWA);
                        // CoreLib.SendTrace(ProviderSystems.UserID, "WorldspanCommand", "4*", "", ProviderSystems.LogUUID)
                        //ttWA.ConversationID = ConversationID;
                        string pRead = ttWA.SendCryptic($"*{pnrNum}", conversationID: ConversationID);

                        bool bEMD = pRead.Contains("EMDL");
                        string emdDisplay = bEMD ? ttWA.SendCryptic("EMDL(") : "";

                        if (!strResponse.Contains("SECURED PNR") && !pRead.Contains("Error"))
                        {

                            #region 4*

                            string str4Display = ttWA.SendCryptic("4*");
                            var lstLines = str4Display
                                .Split(new string[] { "<Screen>", "<Line>", "</Screen>", "</Line>" },
                                    StringSplitOptions.RemoveEmptyEntries).ToList();
                            // Conduct Move Down (MD)
                            if (lstLines.Last().Contains(")&gt;"))
                            {
                                string str4More = ttWA.SendCryptic("MD");
                                var lstMoreLines = str4More
                                    .Split(new string[] { "<Screen>", "<Line>", "</Screen>", "</Line>" },
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
                            var ptcPos = 0;
                            foreach (string line in lstLines)
                            {
                                var trNum = "";
                                var strLine = line.Trim().Replace(")&gt;", "").Replace("&gt;", "");
                                if (!string.IsNullOrEmpty(strLine))
                                {
                                    var index = lstLines.IndexOf(line);
                                    //TR-   1. 4P*FSR.SR                           8YQ/LV 23JUL
                                    //TR-   2. 4P*FSR.SR/-$P10.00/#TR           1P/0G3/RO 23JUL 1819Z
                                    //TR-   3. 4P*FSR.SR/-$P0.00/#TR            1P/0G3/RO 23JUL 1819Z
                                    //TR-   4. 4P*FSR.SR/-$P10.00/#TR           1P/0G3/RO 23JUL 1819Z
                                    if (strLine.StartsWith("TR-   ") && index > 1)
                                    {
                                        ptcPos++;
                                        strResponse = FilterPricePNRByTR(strResponse, line, ptcPos);
                                        trNum = line.Split(new[] { "TR-", " ", ". ", }, StringSplitOptions.RemoveEmptyEntries).ToList()[0];
                                    }
                                    sb4.Append(strLine.StartsWith("TR-   ") && ptcPos > 0 ? $"<Line ID=\"{ptcPos}\" TR=\"{trNum}\">{strLine}</Line>" : $"<Line>{strLine}</Line>");
                                }

                            }

                            sb4.Append("</PNR_4_INF>");

                            #endregion

                            #region EMDL
                            if (bEMD)
                            {
                                List<string> trElems = new List<string>();
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

                            #region *DH

                            string strDisplayDHA = ttWA.SendCryptic("*DH");
                            lstLines = strDisplayDHA.Split(new string[] { "<Screen>", "<Line>", "</Screen>", "</Line>" },
                                StringSplitOptions.RemoveEmptyEntries).ToList();

                            // Conduct Move Down (MD)
                            if (lstLines.Last().Contains(")&gt;"))
                            {
                                string strDHMore = ttWA.SendCryptic("MD");
                                var lstMoreLines = strDHMore
                                    .Split(new string[] { "<Screen>", "<Line>", "</Screen>", "</Line>" },
                                        StringSplitOptions.RemoveEmptyEntries).ToList();
                                foreach (string line in lstMoreLines)
                                {
                                    if (!lstLines.Contains(line))
                                    {
                                        lstLines.Add(line);
                                    }
                                }
                            }

                            if (lstLines.Exists(l => l.Contains("**DOCUMENT COMMANDS**")))
                            {
                                // lstTemp = lstLines.GetRange(1, 2) - Thi sis in Case if we would need name of the passanger
                                var lstTemp = lstLines.GetRange(0, 2);
                                lstLines = lstTemp;
                            }

                            var sbDH = new StringBuilder("<PNR_DH_INF>");
                            foreach (string line in lstLines.GetRange(1, lstLines.Count - 1))
                            {
                                var strLine = line.Trim().Replace(")&gt;", "").Replace("&gt;", "");
                                var lineElem = strLine.Split(new string[] { " ", "*" },
                                    StringSplitOptions.RemoveEmptyEntries).ToList();
                                // If Not String.IsNullOrEmpty(line) AndAlso (line.Length > 40 OrElse line.Trim.StartsWith("NO DOC HISTORY DATA FOUND")) Then
                                if (!string.IsNullOrEmpty(strLine) && (lineElem.Count > 3 || strLine.Trim().StartsWith("NO DOC HISTORY DATA FOUND")))
                                {
                                    sbDH.Append($"<Line TicketNumber='{lineElem[3]}'>{strLine}</Line>");
                                }
                            }

                            sbDH.Append("</PNR_DH_INF>");

                            #endregion

                            #region *DHV


                            string strDisplayDHV = ttWA.SendCryptic("*DHV");
                            lstLines = strDisplayDHV.Split(new string[] { "<Screen>", "<Line>", "</Screen>", "</Line>" },
                                StringSplitOptions.RemoveEmptyEntries).ToList();

                            // Conduct Move Down (MD)
                            if (lstLines.Last().Contains(")&gt;"))
                            {
                                string strDHMore = ttWA.SendCryptic("MD", conversationID: ConversationID);
                                var lstMoreLines = strDHMore
                                    .Split(new string[] { "<Screen>", "<Line>", "</Screen>", "</Line>" },
                                        StringSplitOptions.RemoveEmptyEntries).ToList();
                                foreach (string line in lstMoreLines)
                                {
                                    if (!lstLines.Contains(line))
                                    {
                                        lstLines.Add(line);
                                    }
                                }
                            }

                            if (lstLines.Exists(l => l.Contains("**DOCUMENT COMMANDS** ")))
                            {
                                // lstTemp = lstLines.GetRange(1, 2) - Thi sis in Case if we would need name of the passanger
                                var lstTemp = lstLines.GetRange(0, 2);
                                lstLines = lstTemp;
                            }

                            var sbDHV = new StringBuilder("<PNR_DHV_INF>");
                            foreach (string line in lstLines.GetRange(1, lstLines.Count - 1))
                            {
                                var strLine = line.Trim().Replace(")&gt;", "").Replace("&gt;", "");
                                if (!string.IsNullOrEmpty(strLine) && (strLine.Length > 34 || strLine.Trim().StartsWith("NO DOC HISTORY DATA FOUND")))
                                {
                                    var lineElem = strLine.Split(new string[] { " ", "*" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    if (lineElem.Count > 3)
                                    {
                                        sbDHV.Append($"<Line TicketNumber='{lineElem[3]}'>{strLine}</Line>");
                                    }
                                }
                            }

                            sbDHV.Append("</PNR_DHV_INF>");

                            #endregion

                            #region *HI


                            string strDisplayHI = ttWA.SendCryptic("*HI");
                            lstLines = strDisplayHI.Split(new string[] { "<Screen>", "<Line>", "</Screen>", "</Line>" },
                                StringSplitOptions.RemoveEmptyEntries).ToList();

                            // Conduct Move Bottom (MB)
                            if (lstLines.Last().Equals(")"))
                            {
                                string strDHMore = ttWA.SendCryptic("MB", conversationID: ConversationID);
                                var lstMoreLines = strDHMore
                                    .Split(new string[] { "<Screen>", "<Line>", "</Screen>", "</Line>" },
                                        StringSplitOptions.RemoveEmptyEntries).ToList();
                                foreach (string line in lstMoreLines)
                                {
                                    if (!lstLines.Contains(line))
                                    {
                                        lstLines.Add(line);
                                    }
                                }
                            }

                            if (lstLines.Exists(l => l.Contains("**DOCUMENT COMMANDS**")))
                            {
                                // lstTemp = lstLines.GetRange(1, 2) - Thi sis in Case if we would need name of the passanger
                                var lstTemp = lstLines.GetRange(0, 2);
                                lstLines = lstTemp;
                            }

                            // R-FLYUSRX-DIR -CR- 1QE/1P GS RS 05MAR20 0112Z 10C7F9 ***
                            // R-FLYUSRX-DIR -CR- 1QE/1P GS RS 05MAR20 0112Z 4A02D4 ***
                            // R-0571788MSG -CR- DTK@93D/1P GS XP 04MAR20 1920Z CBB07D ***

                            var sbH = new StringBuilder("<PNR_HI_INF>");
                            var bBookElem = false;
                            foreach (string line in lstLines.GetRange(1, lstLines.Count - 1))
                            {
                                var strLine = line.Trim().Replace(")&gt;", "").Replace("&gt;", "");
                                if (!string.IsNullOrEmpty(strLine) && strLine.Trim().StartsWith("R-") && strLine.Trim().Contains(" -CR- ") && bBookElem)
                                {
                                    var elems = strLine.Split(new string[] { "-", " ", "/", "*" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    string pcc = string.Empty;
                                    string agent = string.Empty;
                                    foreach (string elem in elems)
                                    {
                                        if (elem.Equals("CR"))
                                        {
                                            int index = elems.IndexOf(elem);
                                            pcc = elems[index + 1].Contains("@")
                                                ? elems[index + 1].Substring(elems[index + 1].Length - 3)
                                                : elems[index + 1];
                                            agent = elems[index + 4];
                                            break;
                                        }
                                    }

                                    sbH.Append($"<Line PCC='{pcc}' Agent='{agent}'>{strLine}</Line>");
                                }
                                else
                                {
                                    bBookElem = line.Trim().StartsWith("AS ");
                                }
                            }

                            sbH.Append("</PNR_HI_INF>");

                            #endregion

                            #region *4PR

                            var prcs = xResp.DocumentElement.SelectNodes("//DPW8/PRC_INF/PRC_QUO/PTC_FAR_DTL/FAR_SHE_ORI").Cast<XmlNode>().Select(x => x.InnerText).ToList();
                            var prc_opts = string.Empty;
                            if (prcs.Any(s => s.EndsWith(" SR")))
                            {
                                if (prcs.Any(s => s.Contains("JWZ")))
                                    prc_opts = "FSR";
                                else
                                    prc_opts = "FSR.SR";
                            }
                            string str4P = ttWA.SendCryptic($"4P{prc_opts}");
                            string str4PR = ttWA.SendCryptic("4PRC");
                            lstLines = str4PR.Split(new string[] { "<Screen>", "</Line><Line>", "</Screen>", "<Line>", "</Line>" }, StringSplitOptions.None).ToList();
                            // Conduct Move Down (MD)
                            if (lstLines.Last().Contains(")&gt;"))
                            {
                                string strDHMore = ttWA.SendCryptic("MD", conversationID: ConversationID);
                                var lstMoreLines = strDHMore
                                    .Split(new string[] { "<Screen>", "<Line>", "</Screen>", "</Line>" },
                                        StringSplitOptions.RemoveEmptyEntries).ToList();
                                foreach (string line in lstMoreLines)
                                {
                                    if (!lstLines.Contains(line))
                                    {
                                        lstLines.Add(line);
                                    }
                                }
                            }
                            var sb4PR = new StringBuilder("<PNR_4PR>");

                            var flSegs = new List<flSegsData>();
                            try
                            {
                                xResp.DocumentElement.SelectNodes("//AIR_SEG_INF/AIR_ITM[FLI_NUM!='ARNK']").Cast<XmlNode>().ToList().Select(x =>
                                    new//(string segNum, string depCity, string arrCity)
                                    {
                                        segNum = x.SelectSingleNode("SEG_NUM").InnerText,
                                        depCity = x.SelectSingleNode("DEP_ARP").InnerText,
                                        arrCity = x.SelectSingleNode("ARR_ARP").InnerText
                                    }).ToList().ForEach(x => flSegs.Add(new flSegsData { segNum = x.segNum, depCity = x.depCity, arrCity = x.arrCity }));
                            }
                            catch { }

                            if (flSegs.Any())
                            {

                                var stop = false;
                                foreach (string line in lstLines.GetRange(1, lstLines.Count - 1))
                                {
                                    var strLine = line.Trim().Replace(")&gt;", "").Replace("&gt;", "");
                                    if (!string.IsNullOrEmpty(strLine) && (strLine.Length > 34) && !strLine.StartsWith("PTC   FARE  FARE"))
                                    {
                                        var lineElem = strLine.Split(new string[] { " ", "*" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                        switch (lineElem.Count)
                                        {
                                            case 6:
                                                sb4PR.Append($"<Line PTC='{lineElem[1]}' CC='{lineElem.Last()}' Flights='{GetFlightRefs(lineElem[3], lineElem[4], ref flSegs)}'>{lineElem[3]}{lineElem[4]}</Line>");
                                                stop = true;
                                                break;
                                            case 7:
                                                sb4PR.Append($"<Line PTC='{lineElem[2]}' CC='{lineElem.Last()}' Flights='{GetFlightRefs(lineElem[4], lineElem[5], ref flSegs)}'>{lineElem[4]}{lineElem[5]}</Line>");
                                                stop = true;
                                                break;
                                        }
                                    }
                                    else if (string.IsNullOrEmpty(strLine) && stop)
                                        break;
                                }
                                sb4PR.Append("</PNR_4PR>");
                            }
                            else
                                sb4PR.Clear();
                            #endregion

                            #region Add Segments Node to PRC_INF/TIC_REC_PRC_QUO
                            var prcNode = xResp.SelectSingleNode("//PRC_INF/TIC_REC_PRC_QUO[contains(PRC_QUO_CMD,'*S')]/PRC_QUO_CMD");
                            if (prcNode != null)
                            {
                                var prc_command = prcNode.InnerText.Substring(prcNode.InnerText.IndexOf("*S") + 2);
                                if (prc_command.Contains("#"))
                                    prc_command = prc_command.Substring(0, prc_command.IndexOf("#"));
                                prc_command = Regex.Replace(prc_command, @"(BF\d+)", "");
                                prc_command = prc_command.Replace("*", "").Replace(":", "").Replace("/", " ");
                                var segsNode = xResp.CreateElement("SEGMENTS");
                                segsNode.InnerText = prc_command;
                                var prc = xResp.SelectSingleNode("//PRC_INF/TIC_REC_PRC_QUO[contains(PRC_QUO_CMD,'*S')]");
                                prc.InsertAfter(segsNode, prc.FirstChild);

                                strResponse = xResp.OuterXml;
                            }

                            #endregion

                            #region Add TPA_Extensions/AgencyCommission info


                            #endregion

                            strResponse = strResponse.Replace("</DPW8>", $"{sb4}{sbDH}{sbDHV}{sbH}{sb4PR}{agentCommResp}</DPW8>"); //
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                    }

                    // End If

                    // ************************************************
                    // calculate year in all dates and arrival date  *
                    // ************************************************
                    try
                    {
                        var oDoc = new XmlDocument();
                        oDoc.LoadXml(strResponse);
                        var oRoot = oDoc.DocumentElement;
                        if (oRoot.SelectNodes("AIR_SEGMENT_INFO") != null)
                        {
                            foreach (XmlNode oNode in oRoot.SelectNodes("AIR_SEGMENT_INFO/AIR_ITEM"))
                            {
                                var dtDepartureDate = Convert.ToDateTime($"{oNode.SelectSingleNode("DEP_DATE/DEP_DAY").InnerText}{oNode.SelectSingleNode("DEP_DATE/DEP_MONTH").InnerText}{DateTime.Now.Year}");
                                var dtArrivalDate = Convert.ToDateTime($"{oNode.SelectSingleNode("ARR_DATE/ARR_DAY").InnerText}{oNode.SelectSingleNode("ARR_DATE/ARR_MONTH").InnerText}{DateTime.Now.Year}");

                                if (DateTime.Now.DayOfYear > dtDepartureDate.DayOfYear)
                                {
                                    dtDepartureDate = dtDepartureDate.AddYears(1);
                                }

                                oNode.SelectSingleNode("DEP_DATE").InnerText = dtDepartureDate.ToString("yyyy-MM-dd");
                                if (DateTime.Now.DayOfYear > dtArrivalDate.DayOfYear)
                                {
                                    dtArrivalDate = dtArrivalDate.AddYears(1);
                                }

                                oNode.SelectSingleNode("ARR_DATE").InnerText = dtArrivalDate.ToString("yyyy-MM-dd");
                            }

                            strResponse = oRoot.OuterXml;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                    }
                }

                // *****************************************************************
                // Transform Native Worldspan PNRRead Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    if (strResponse.Length > 1500)
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response I",
                            strResponse.Substring(0, (int)Math.Round(strResponse.Length / 2d)),
                            ProviderSystems.LogUUID);
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response II",
                            strResponse.Substring((int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                    }
                    else
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response I", strResponse,
                            ProviderSystems.LogUUID);
                    }

                    var strToReplace = "</DPW8>";


                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID>{ConversationID}</ConversationID>{strToReplace}");

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
                        ttProviderSystems.Profile = ProviderSystems.ProfileXML;
                    }
                }

            }
            catch (Exception exx)
            {
                AddLog($"<M>{Request}<BL/>", ProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRRead, exx.Message, ProviderSystems);
            }

            return strResponse;
        }

        private string GetFlightRefs(string from, string to, ref List<flSegsData> flSegs)
        {
            var res = string.Empty;
            try
            {
                if (flSegs.Any(f => !f.processed && f.depCity == from) && flSegs.Any(f => !f.processed && f.arrCity == to))
                {
                    res += flSegs.First(f => !f.processed && f.depCity == from).segNum;
                    var found = false;
                    if (flSegs.First(f => !f.processed && f.depCity == from).segNum != flSegs.First(f => !f.processed && f.arrCity == to).segNum)
                    {
                        flSegs.First(f => !f.processed && f.depCity == from).processed = true;
                        foreach (var seg in flSegs.FindAll(x => !x.processed))
                        {
                            if (found && seg.arrCity != to)
                                break;
                            res += " " + seg.segNum;
                            seg.processed = true;
                            if (seg.arrCity.Equals(to))
                                found = true;
                        }
                    }
                }
            }
            catch { }
            return res;
        }

        public string PNRCancel()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA PNRCancel Request into Native Worldspan Request     *
            // ***************************************************************** 

            try
            {
                string strRequest = SetRequest("Worldspan_PNRCancelRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response  *
                // ******************************************************************************* 
                var ttWA = SetAdapter(ProviderSystems);
                bool inSession = SetConversationID(ttWA);

                strResponse = ttWA.SendMessage(strRequest);

                // *****************************************************************
                // Transform Native Worldspan PNRCancel Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var strToReplace = strResponse.Contains("XPW3") ? "</XPW3>" : "XXW";

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID>{ConversationID}</ConversationID>{strToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_PNRCancelRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRCancel, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string PNRReprice()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA PNRReprice Request into Native Worldspan Request     *
            // ***************************************************************** 
            var oDoc = new XmlDocument();
            try
            {
                string strRequest = SetRequest("Worldspan_PNRRepriceRQ.xsl");
                CoreLib.SendTrace(ProviderSystems.UserID, "PNRRePrice", "Request", strRequest, ProviderSystems.LogUUID);
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                oDoc.LoadXml(Request);
                bool bStoredFare = oDoc.DocumentElement.Attributes.GetNamedItem("StoreFare").InnerText.Equals("true");
                var recordLocator = oDoc.DocumentElement.SelectSingleNode("UniqueID/@ID") != null
                    ? oDoc.DocumentElement.SelectSingleNode("UniqueID/@ID").Value
                    : string.Empty;

                // *******************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response  *
                // ******************************************************************************* 

                // Dim oDoc As New XmlDocument()
                XmlElement oRoot;
                XmlNode oNode;
                modCore.TripXMLProviderSystems ttProviderSystems = ProviderSystems;
                WorldspanAdapter ttWA;
                bool inSession = false;
                List<string> trElems = new List<string>();
                if (Request.Contains("Markup"))
                {
                    #region Apply Markups
                    ttProviderSystems.Profile = ProviderSystems.ProfileCryptic;
                    ttWA = SetAdapter(ttProviderSystems);
                    inSession = SetConversationID(ttWA);
                    oDoc.LoadXml(strRequest);
                    oRoot = oDoc.DocumentElement;

                    foreach (XmlNode currentONode in oRoot)
                    {
                        oNode = currentONode;
                        ttWA.SendCryptic(oNode.InnerText);
                    }
                    ttWA.CloseSession();
                    inSession = false;
                    #endregion

                    #region ReRead PNR
                    ttProviderSystems.Profile = ProviderSystems.ProfileXML;
                    ttWA = SetAdapter(ttProviderSystems);
                    recordLocator = oRoot.SelectSingleNode("ScreenEntry[1]").InnerXml.Substring(1);
                    strResponse = ttWA.SendMessage($"<DPC8><MSG_VERSION>8</MSG_VERSION><REC_LOC>{recordLocator}</REC_LOC><ETR_INF>Y</ETR_INF><ALL_PNR_INF>Y</ALL_PNR_INF><PRC_INF>Y</PRC_INF></DPC8>");
                    #endregion

                    #region 4*
                    ttProviderSystems.Profile = ProviderSystems.ProfileCryptic;
                    ttWA = SetAdapter(ttProviderSystems);
                    inSession = SetConversationID(ttWA);
                    string pnr = ttWA.SendCryptic($"*{recordLocator}");
                    bool bEMD = pnr.Contains("**  ELECTRONIC MISC DOCUMENT LIST  **  >EMDL");

                    string emdDisplay = bEMD ? ttWA.SendCryptic("EMDL") : "";
                    string str4Display = ttWA.SendCryptic("4*");

                    var lstLines = str4Display.Split(new string[] { "<Screen>", "<Line>", "</Screen>", "</Line>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    // Conduct Move Down (MD)
                    if (lstLines.Last().Contains(")&gt;"))
                    {
                        string str4More = ttWA.SendCryptic("MD");
                        var lstMoreLines = str4More.Split(new string[] { "<Screen>", "<Line>", "</Screen>", "</Line>" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        foreach (string line in lstMoreLines)
                        {
                            if (!lstLines.Contains(line))
                            {
                                lstLines.Add(line);
                            }
                        }
                    }
                    ttWA.CloseSession();
                    inSession = false;
                    var sb4 = new StringBuilder("<PNR_4_INF>");
                    var ptcPos = 0;
                    foreach (string line in lstLines)
                    {
                        var trNum = "";
                        var strLine = line.Trim().Replace(")&gt;", "").Replace("&gt;", "");
                        if (!string.IsNullOrEmpty(strLine))
                        {
                            var index = lstLines.IndexOf(line);
                            //TR-   1. 4P*FSR.SR                           8YQ/LV 23JUL
                            //TR-   2. 4P*FSR.SR/-$P10.00/#TR           1P/0G3/RO 23JUL 1819Z
                            //TR-   3. 4P*FSR.SR/-$P0.00/#TR            1P/0G3/RO 23JUL 1819Z
                            //TR-   4. 4P*FSR.SR/-$P10.00/#TR           1P/0G3/RO 23JUL 1819Z
                            if (strLine.StartsWith("TR-   ") && index > 1)
                            {
                                ptcPos++;
                                strResponse = FilterPricePNRByTR(strResponse, line, ptcPos);
                                trNum = line.Split(new[] { "TR-", " ", ". ", }, StringSplitOptions.RemoveEmptyEntries).ToList()[0];
                                trElems.Add(trNum);
                            }
                            sb4.Append(strLine.StartsWith("TR-   ") && ptcPos > 0 ? $"<Line ID=\"{ptcPos}\" TR=\"{trNum}\">{strLine}</Line>" : $"<Line>{strLine}</Line>");

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
                else
                {
                    ttProviderSystems.Profile = ProviderSystems.ProfileXML;
                    ttWA = SetAdapter(ttProviderSystems);
                    inSession = false; //SetConversationID(ttWA);
                    strResponse = ttWA.SendMessage(strRequest);
                    strResponse = strResponse.Replace("xmlns=\"http://www.opentravel.org/OTA_RS/2003/05\" ", "");

                    // ---------------------------------------------
                    // Identifying Fare Type (Private or Published)
                    // ---------------------------------------------
                    oDoc.LoadXml(Request);
                    oRoot = oDoc.DocumentElement;
                    string pnrType = oRoot.SelectSingleNode("StoredFare/@FareType").InnerXml;
                    strResponse = strResponse.Replace("<AirItineraryPricingInfo>",
                        $"<AirItineraryPricingInfo PricingSource=\"{pnrType}\">");

                    // ---------------------------------------------
                }

                // *****************************************************************
                // Transform Native Worldspan PNRReprice Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var strToReplace = "</OTA_AirPriceRS>";
                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID>{ConversationID}</ConversationID>{strToReplace}");

                    if (strResponse.Length > 1500)
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRReprice", "Final response I",
                            strResponse.Substring(0, (int)Math.Round(strResponse.Length / 2d)),
                            ProviderSystems.LogUUID);
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRReprice", "Final response II",
                            strResponse.Substring((int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                    }
                    else
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRReprice", "Final response I", strResponse,
                            ProviderSystems.LogUUID);
                    }

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_PNRRepriceRS.xsl");

                    #region W/o Storing for Price Comparecing

                    if (!Request.Contains("Markup"))
                    {

                        if (!string.IsNullOrEmpty(recordLocator))
                        {
                            string strPNRResp = ttWA.SendMessage($"<DPC8><MSG_VERSION>8</MSG_VERSION><REC_LOC>{recordLocator}</REC_LOC><ETR_INF>Y</ETR_INF><ALL_PNR_INF>Y</ALL_PNR_INF><PRC_INF>Y</PRC_INF></DPC8>");

                            Version = string.IsNullOrEmpty(Version) ? "v03" : Version;

                            strToReplace = "</DPC8>";
                            if (inSession)
                                strResponse = strResponse.Replace(strToReplace, $"<ConversationID>{ConversationID}</ConversationID>{strToReplace}");

                            strPNRResp = CoreLib.TransformXML(strPNRResp, XslPath, $"{Version}Worldspan_PNRReadRS.xsl");

                            oDoc.LoadXml(strPNRResp);
                            oRoot = oDoc.DocumentElement;
                            foreach (XmlNode currentONode1 in oRoot.ChildNodes)
                            {
                                oNode = currentONode1;
                                if (oNode.Name.Equals("TravelItinerary"))
                                {
                                    foreach (XmlNode tiNode in oNode.ChildNodes)
                                    {
                                        if (tiNode.Name.Equals("ItineraryInfo"))
                                        {
                                            foreach (XmlNode iiNode in tiNode.ChildNodes)
                                            {
                                                if (iiNode.Name.Equals("ReservationItems"))
                                                {
                                                    foreach (XmlNode riNode in iiNode.ChildNodes)
                                                    {
                                                        if (riNode.Name.Equals("ItemPricing"))
                                                        {
                                                            strResponse =
                                                                strResponse.Replace(
                                                                    $"<AirItineraryPricingInfo PricingSource=\"\" />",
                                                                    $"{riNode.InnerXml.Replace("AirFareInfo", "AirItineraryPricingInfo")}");
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        // ---------------------------------------------
                    }

                    #endregion

                    // Bug 999 - T-robot Worldspan - 4PQC after each call of repricing with change of stored fare
                    if (bStoredFare)
                    {
                        ttProviderSystems.Profile = ProviderSystems.ProfileCryptic;
                        ttWA = SetAdapter(ttProviderSystems);
                        SetConversationID(ttWA);
                        ttWA.SendCryptic($"*{recordLocator}");

                        ttWA.SendCryptic(trElems.Count() > 0 ? $"4PQCTR{string.Join("/", trElems).TrimEnd('/')}" : "4PQC");

                        ttWA.SendCryptic("ER");
                        inSession = false;
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
                        ttWA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRReprice, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        private string FilterPricePNRByTR(string response, string request, int index)
        {
            if (string.IsNullOrEmpty(request))
                return response;

            //TR-   1. 4P*FSR.SR                           8YQ/LV 23JUL
            //TR-   2. 4P*FSR.SR/-$P10.00/#TR           1P/0G3/RO 23JUL 1819Z
            //TR-   3. 4P*FSR.SR/-$P0.00/#TR            1P/0G3/RO 23JUL 1819Z
            //TR-   4. 4P*FSR.SR/-$P10.00/#TR           1P/0G3/RO 23JUL 1819Z

            var rqElem = request.Split(new[] { "TR-", " ", ". ", }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //TR-   2. 4P*FSR.SR/-$P10.00/#TR           1P/0G3/RO 23JUL 1819Z
            //---------------------------------------------------------------
            //0:2
            //1:4P*FSR.SR/-$P10.00/#TR
            //2:1P/0G3/RO
            //3:23JUL
            //4:1819Z

            XmlDocument docRS = new XmlDocument();
            docRS.LoadXml(response);
            XmlElement rootRS = docRS.DocumentElement;

            var nodeList = docRS.SelectNodes($"//TIC_REC_PRC_QUO[TIC_REC_NUM={rqElem[0]}]/PTC_FAR_DTL");
            for (int i = 0; i < nodeList.Count; i++)
            {
                if (i + 1 != index)
                {
                    nodeList[i].ParentNode?.RemoveChild(nodeList[i]);
                }
            }

            return docRS.InnerXml;
        }

    }
}