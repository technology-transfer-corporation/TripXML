using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using TripXMLMain;
using System.Globalization;
using System.ComponentModel;
using static TripXMLMain.modCore;
using static TripXMLMain.modCore.enAmadeusWSSchema;

namespace AmadeusWS
{
    public abstract class AmadeusBase
    {
        public modCore.TripXMLProviderSystems ttProviderSystems;
        private string mstrVersion = "";
        private string mstrXslPath = "";
        public string _tracerID = "";

        public string ConversationID { get; set; }

        public string Request { get; set; }

        public string Version
        {
            get => mstrVersion;
            set
            {
                mstrVersion = value;
                if (!string.IsNullOrEmpty(mstrVersion)) mstrVersion = $"{mstrVersion.TrimEnd('_')}_";
            }
        }

        public string XslPath
        {
            get => mstrXslPath;
            set => mstrXslPath = $"{value}AmadeusWS\\";
        }

        public enum enMonth
        {
            [Description("January")]
            JAN = 1,
            [Description("Febuary")]
            FEB = 2,
            [Description("March")]
            MAR = 3,
            [Description("April")]
            APR = 4,
            [Description("May")]
            MAY = 5,
            [Description("June")]
            JUN = 6,
            [Description("July")]
            JUL = 7,
            [Description("August")]
            AUG = 8,
            [Description("September")]
            SEP = 9,
            [Description("October")]
            OCT = 10,
            [Description("November")]
            NOV = 11,
            [Description("December")]
            DEC = 12
        }

        #region Functional Methods

        protected bool AllDigits(string txt)
        {
            bool allDigits = txt.All(char.IsDigit);

            if (!allDigits)
            {
                foreach (char c in txt)
                {
                    if ((c < '0' || c > '9') && !c.ToString().Equals("."))
                        return false;
                }
                return true;
            }
            return allDigits;
        }

        protected static bool IsNumeric(object Expression)
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

        protected string SubSessionID(string SessionID)
        {
            int intSession;
            string[] sessionid = SessionID.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            if (ttProviderSystems.SOAP2)
            {
                intSession = Convert.ToInt32(sessionid[2]);
                intSession -= 1;
                SessionID = $"{sessionid[0]}|{sessionid[1]}|{intSession}";

            }
            if (ttProviderSystems.SOAP4)
            {
                var session4 = new Soap4Session(SessionID);
                session4.DecreaseSequenceNo();
                SessionID = session4.ToString();
            }
            else
            {
                intSession = Convert.ToInt32(sessionid[1]);
                intSession -= 1;
                SessionID = $"{sessionid[0]}|{intSession}";
            }

            return SessionID;
        }

        protected string UpdateSessionID(string sessionID)
        {
            if (string.IsNullOrEmpty(sessionID))
                return sessionID;

            int intSession;
            var sessionid = sessionID.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            if (ttProviderSystems.SOAP2)
            {
                intSession = Convert.ToInt32(sessionid[2]);
                intSession += 1;
                sessionID = $"{sessionid[0]}|{sessionid[1]}|{intSession}";
            }
            else if (ttProviderSystems.SOAP4)
            {
                var session4 = new Soap4Session(sessionID);
                session4.IncreaseSequenceNo();
                sessionID = session4.ToString();
            }
            else
            {
                intSession = Convert.ToInt32(sessionid[1]);
                intSession += 1;
                sessionID = $"{sessionid[0]}|{intSession}";
            }

            return sessionID;
        }

        protected static void addLog(string msg, string username)
        {
            try
            {
                string filePath = $"log\\{username}_{DateTime.Today:dd-MM-yyyy}";
                string dirPath = "C:\\TripXML\\log";
                filePath = $"C:\\TripXML\\{filePath}.txt";

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                if (!File.Exists(filePath))
                {
                    using (StreamWriter sw = File.CreateText(filePath))
                    {
                        sw.WriteLine("created On - {0}\r\n", DateTime.Now);
                        sw.Flush();
                        sw.Close();
                    }
                }
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    DateTimeFormatInfo myDtfi = new CultureInfo("en-US", true).DateTimeFormat;

                    sw.WriteLine(DateTime.UtcNow.ToString(myDtfi).Substring(11) + " GMT - " + msg + "\r\n");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(new StringBuilder("Error adding line to Log.").Append("\r\n").Append(ex.Message).ToString());
            }
        }

        protected string formatAmadeus(string strDisp)
        {
            try
            {
                var sb = new StringBuilder();
                var lRet = strDisp.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (string line in lRet.FindAll(l => !String.IsNullOrEmpty(l.Trim())))
                {
                    //var index = lRet.IndexOf(line);
                    //if (index >= 1)
                    sb.AppendLine($"<Line>{line.Trim()}</Line>");
                }
                string strDisp1 = sb.ToString();
                sb.Clear();
                return strDisp1;
            }
            catch (Exception)
            {
                return "";
            }
        }

        protected AmadeusWSAdapter SetAdapter(string version = "")
        {
            var tt = string.IsNullOrEmpty(version) ? new AmadeusWSAdapter(ttProviderSystems) : new AmadeusWSAdapter(ttProviderSystems, version)
            {
                isSOAP2 = ttProviderSystems.SOAP2,
                isSOAP4 = ttProviderSystems.SOAP4,
                TracerID = _tracerID
            };

            return tt;
        }

        protected string SetRequest(string xslFile)
        {
            try
            {
                #region Get Tracer ID

                var otaDoc = new XmlDocument();
                otaDoc.LoadXml(Request);
                XmlElement otaElement = otaDoc.DocumentElement;
                if (otaElement != null && otaElement.HasAttribute("EchoToken") && otaElement.Attributes["EchoToken"].Value != null)
                {
                    _tracerID = otaElement.Attributes["EchoToken"].Value;
                }
                else
                {
                    _tracerID = "";
                }

                #endregion

                Request = Request.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Replace("<?xml version=\"1.0\"?>", "");

                if (string.IsNullOrEmpty(xslFile))
                    return Request;

                return CoreLib.TransformXML(Request, XslPath, $"{Version}{xslFile}"); //$"{Version}AmadeusWS_QueueReadRQ.xsl"
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming OTA Request. {ex.Message}");
            }
        }

        public static void addLogStat(string msg, string username, string PCC, DateTime myDTFI, DateTime myDTFIR, TimeSpan dur)
        {
            try
            {
                string FilePath = $"log\\{username}_LFP_{DateTime.Today.ToString("dd-MM-yyyy")}";
                string DirPath = "C:\\TripXML\\log";
                FilePath = $"C:\\TripXML\\{FilePath}.txt";

                if (!Directory.Exists(DirPath))
                {
                    Directory.CreateDirectory(DirPath);
                }
                if (!File.Exists(FilePath))
                {
                    using (StreamWriter sw = File.CreateText(FilePath))
                    {
                        sw.WriteLine($"created On - {DateTime.Now.ToString()}\r\n");
                        sw.Flush();
                        sw.Close();
                    }
                }
                using (StreamWriter sw = File.AppendText(FilePath))
                {
                    sw.WriteLine($"<S>{myDTFI.ToString("dd MMM yyyy HH:mm:ss")}</S><E>{myDTFIR.ToString("dd MMM yyyy HH:mm:ss")}</E>");
                    sw.WriteLine($"<D>{dur.TotalSeconds}</D><P>{PCC}</P>");
                    sw.WriteLine($"{msg}\r\n");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        protected string formatBreakPoint(string strScreen1)
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
                            strScreen = $"{strScreen}<Line>{oRootScreen.SelectNodes("Line").Item(i).InnerText}</Line>";
                        }
                    }
                }

                if (oRootScreen.SelectNodes("Line").Item(i).InnerText.Contains("TOTAL FARE CALCULATION"))
                {
                    strScreen = $"<Screen>{strScreen}</Screen>";
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
                            strBreakPoint = $"{strBreakPoint}N";
                        }
                        else
                        {
                            strBreakPoint = $"{strBreakPoint}Y";
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
                                strBreakPoint = $"{strBreakPoint}N";
                            }
                            else
                            {
                                strBreakPoint = $"{strBreakPoint}Y";
                            }
                        }
                    }
                }
            }

            strBreakPoint = $"{strBreakPoint}</BreakPoint>";

            return strBreakPoint;
        }

        public void LogMessageToFile(string MsgType, string Message, DateTime RequestTime, DateTime ResponseTime)
        {
            try
            {
                TimeSpan dur;
                dur = ResponseTime - RequestTime;
                string strLine = $"<Message Type=\'{MsgType}\' RequestTime=\'{RequestTime.ToString("dd MMM yyyy HH:mm:ss")}\' ResponseTime=\'{ResponseTime.ToString("dd MMM yyyy HH:mm:ss")}\' Duration=\'{dur.TotalSeconds}\'><AmadeusMessage>{Message}</AmadeusMessage></Message>";
                addLog(strLine, ttProviderSystems.UserID);
            }
            catch (Exception)
            {
                //  
            }
        }

        protected string BuildOTAResponse(string strResponse)
        {
            string strEchoToken = "";
            try
            {
                if (Request.Contains(" EchoToken"))
                {
                    var oDocReq = new XmlDocument();
                    oDocReq.LoadXml(Request);
                    XmlElement oRootReq = oDocReq.DocumentElement;
                    strEchoToken = $"<EchoToken>{oRootReq.Attributes.GetNamedItem("EchoToken").Value}</EchoToken>";
                }

                if (Request.Contains("SecurityToken"))
                {
                    var oDocReq = new XmlDocument();
                    oDocReq.LoadXml(Request);
                    XmlElement oRootReq = oDocReq.DocumentElement;
                    strEchoToken = $"<ConversationID>{oRootReq.SelectSingleNode("SecurityToken").InnerText}|{oRootReq.SelectSingleNode("SessionId").InnerText}|{oRootReq.SelectSingleNode("SequenceNumber").InnerText}</ConversationID>";
                }               

                strResponse = $"<PNR_Reply>{strResponse}{strEchoToken}</PNR_Reply>";
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_PNRReadRS.xsl");
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Building OTA Response.\r\n{ex.Message}");
            }
        }

        /// <summary>
        /// This method will either get SessionID from Request or will set new SessionID.
        /// </summary>
        /// <param name="ttAA">GDS Adapter</param>
        /// <returns>returns flag wether conversation in session or not.</returns>
        protected bool SetConversationID(AmadeusWSAdapter ttAA)
        {
            try
            {
                var oDocReq = new XmlDocument();
                oDocReq.LoadXml(Request);
                var oRootReq = oDocReq.DocumentElement;

                XmlNode oNodeSPL = ttAA.isSOAP4
                    ? oRootReq?.SelectSingleNode("POS/TPA_Extensions/ConversationID")
                    : oRootReq?.SelectSingleNode("ConversationID");

                if (oNodeSPL == null)
                {
                    var oElem = oRootReq?.GetElementsByTagName("ConversationID");
                    oNodeSPL = oElem.Count > 0 ? oElem[0] : null;
                }

                if (oNodeSPL != null)
                {
                    if (oNodeSPL.InnerText.Contains("|"))
                        ConversationID = oNodeSPL.InnerText;

                    if (!String.IsNullOrEmpty(ConversationID))
                        return true;
                }

                if (String.IsNullOrEmpty(ConversationID))
                    ConversationID = ttProviderSystems.SessionPool ? ttAA.CheckSessionV2() : ttAA.CreateSession();

                if (!string.IsNullOrEmpty(ConversationID) && ConversationID.Contains("Error"))
                {
                    string conv = ConversationID.Substring(15, ConversationID.Length - 32);
                    ConversationID = "";
                    throw new Exception(conv);
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Creating Session.\r\n{ex.Message}");
            }
        }
        #endregion

        #region Send Methods
        /// <summary>
        /// General Method that Sends out message to GDS
        /// </summary>
        /// <param name="ttAA">GDS Adapter</param>
        /// <param name="request">Request XML</param>
        /// <param name="methodRQ">Request Object Name</param>
        /// <param name="methodRS">Response Object Name</param>
        /// <param name="nameSpace">Namespace for which this message belong. Example: "Air". By default it will be empty.</param>
        /// <returns></returns>
        protected string SendGDSMessage(AmadeusWSAdapter ttAA, string request, string methodRQ, string methodRS, string nameSpace = "")
        {
            try
            {
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSService", methodRQ, "", ttProviderSystems.LogUUID);

                var response = ttProviderSystems.SessionPool
                    ? ttAA.SendMessageV3(request, nameSpace, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/{methodRQ}", ConversationID).Replace($" xmlns=\"http://xml.amadeus.com/{methodRS}\"", "")
                    : ttAA.SendMessage(request, nameSpace, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/{methodRQ}", ConversationID).Replace($" xmlns=\"http://xml.amadeus.com/{methodRS}\"", "");
                ConversationID = UpdateSessionID(ConversationID);

                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSService", methodRS, response, ttProviderSystems.LogUUID);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending GDS Message.\r\n{ex.Message}");
            }
        }

        protected string SendRetrivePNRbyRL(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[PNR_RetrieveByRecLoc], ttProviderSystems.AmadeusWSSchema[PNR_RetrieveByRecLocReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Retrive PNR By RL.\r\n{ex.Message}");
            }
        }

        protected string SendRetrievePNR(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                //var response = ttAA.SendMessage(request, "", $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/{ttProviderSystems.AmadeusWSSchema.PNR_Retrieve}", ConversationID)
                //    .Replace($" xmlns=\"http://xml.amadeus.com/{ttProviderSystems.AmadeusWSSchema[PNR_Reply]}\"", "")
                //    .Replace($" xmlns=\"http://xml.amadeus.com/{ttProviderSystems.AmadeusWSSchema[PNR_Reply1]}\"", "");
                //ConversationID = UpdateSessionID(ConversationID);
                var response = SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[PNR_Retrieve], ttProviderSystems.AmadeusWSSchema[PNR_Reply]);
                response = response.Replace($" xmlns=\"http://xml.amadeus.com/{ttProviderSystems.AmadeusWSSchema[enAmadeusWSSchema.PNR_Reply]}\"", "");
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Retrieve PNR.\r\n{ex.Message}");
            }
        }

        protected string SendCancelPNR(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                var response = ttAA.SendMessage(request, "", $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/{ttProviderSystems.AmadeusWSSchema[PNR_Cancel]}", ConversationID)
                    .Replace($" xmlns=\"http://xml.amadeus.com/{ttProviderSystems.AmadeusWSSchema[PNR_Reply]}\"", "")
                    .Replace($" xmlns=\"http://xml.amadeus.com/{ttProviderSystems.AmadeusWSSchema[PNR_Reply1]}\"", "");
                ConversationID = UpdateSessionID(ConversationID);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Cancel PNR Message.\r\n{ex.Message}");
            }
        }

        protected string SendCommandCryptically(AmadeusWSAdapter ttAA, string command)
        {
            try
            {
                var request = $"<Command_Cryptic><messageAction><messageFunctionDetails><messageFunction>M</messageFunction></messageFunctionDetails></messageAction><longTextString><textStringDetails>{command}</textStringDetails></longTextString></Command_Cryptic>";
                return SendRequestCryptically(ttAA, request);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Command Cryptically Message.\r\n{ex.Message}");
            }
        }

        protected string SendRequestCryptically(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                var response = SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Command_Cryptic], ttProviderSystems.AmadeusWSSchema[Command_CrypticReply]);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Request Cryptically.\r\n{ex.Message}");
            }
        }

        protected string SendAddMultiElements(AmadeusWSAdapter ttAA, string request, string nameSpace = "")
        {
            try
            {
                var response = ttAA.SendMessage(request, nameSpace, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/{ttProviderSystems.AmadeusWSSchema[PNR_AddMultiElements]}", ConversationID)
                    .Replace($" xmlns=\"http://xml.amadeus.com/{ttProviderSystems.AmadeusWSSchema[PNR_Reply]}\"", "")
                    .Replace($" xmlns=\"http://xml.amadeus.com/{ttProviderSystems.AmadeusWSSchema[PNR_Reply1]}\"", "");
                ConversationID = UpdateSessionID(ConversationID);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Add Multi Elements Message.\r\n{ex.Message}");
            }
        }

        protected string SendDeleteVirtualCard(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[PAY_DeleteVirtualCard], ttProviderSystems.AmadeusWSSchema[PAY_DeleteVirtualCard]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Delete Virtual Card Message.\r\n{ex.Message}");
            }
        }

        protected string SendGenerateVirtualCard(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[PAY_GenerateVirtualCard], ttProviderSystems.AmadeusWSSchema[PAY_GenerateVirtualCard]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Generate Virtual Card Message.\r\n{ex.Message}");
            }
        }

        protected string SendListVirtualCards(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[PAY_ListVirtualCards], ttProviderSystems.AmadeusWSSchema[PAY_ListVirtualCards]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending List Virtual Cards Message.\r\n{ex.Message}");
            }
        }

        protected string SendVirtualCardDetails(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[PAY_VirtualCardDetails], ttProviderSystems.AmadeusWSSchema[PAY_VirtualCardDetails]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Virtual Cards Details Message.\r\n{ex.Message}");
            }
        }

        protected string SendListQueue(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, "QDQLRQ_03_1_1A", "QDQLRQ_03_1_1A");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCountQueue(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, "QCSDRQ_03_1_1A", "QCSDRR_03_1_1A");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendRemoveQueue(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, "QUQMDQ_03_1_1A", "QUQMDR_03_1_1A");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendBounceQueue(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, "HSFREQ_07_3_1A", "HSFRES_07_3_1A");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendQueueMove(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, "QUQMUQ_03_1_1A", "QUQMUR_03_1_1A");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendPlaceQueue(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, "QUQPCQ_03_1_1A", "QUQPCR_03_1_1A");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendDisplayTST(AmadeusWSAdapter ttAA)
        {
            try
            {
                var request = "<Ticket_DisplayTST><displayMode><attributeDetails><attributeType>ALL</attributeType></attributeDetails></displayMode></Ticket_DisplayTST>";
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Ticket_DisplayTST], ttProviderSystems.AmadeusWSSchema[Ticket_DisplayTSTReply]); ;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendDeleteTST(AmadeusWSAdapter ttAA)
        {
            try
            {
                var request = "<Ticket_DeleteTST><deleteMode><attributeDetails><attributeType>ALL</attributeType></attributeDetails></deleteMode></Ticket_DeleteTST>";
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Ticket_DeleteTST], ttProviderSystems.AmadeusWSSchema[Ticket_DeleteTSTReply]); ;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendAirMultiAvailability(AmadeusWSAdapter ttAA, string strRequest)
        {
            try
            {
                return SendGDSMessage(ttAA, strRequest, ttProviderSystems.AmadeusWSSchema[Air_MultiAvailability], ttProviderSystems.AmadeusWSSchema[Air_MultiAvailabilityReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendInformativePricingWithoutPNR(AmadeusWSAdapter ttAA, string strRequest)
        {
            try
            {
                return SendGDSMessage(ttAA, strRequest, ttProviderSystems.AmadeusWSSchema[Fare_InformativePricingWithoutPNR], ttProviderSystems.AmadeusWSSchema[Fare_InformativePricingWithoutPNRReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendFareCheckRules(AmadeusWSAdapter ttAA, string strRequest)
        {
            try
            {
                return SendGDSMessage(ttAA, strRequest, ttProviderSystems.AmadeusWSSchema[Fare_CheckRules], ttProviderSystems.AmadeusWSSchema[Fare_CheckRulesReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendFareDisplayFaresForCityPair(AmadeusWSAdapter ttAA, string strRequest)
        {
            try
            {
                return SendGDSMessage(ttAA, strRequest, ttProviderSystems.AmadeusWSSchema[Fare_DisplayFaresForCityPair], ttProviderSystems.AmadeusWSSchema[Fare_DisplayFaresForCityPairReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendFareInformativeBestPricingWithoutPNR(AmadeusWSAdapter ttAA, string strRequest)
        {
            try
            {
                return SendGDSMessage(ttAA, strRequest, ttProviderSystems.AmadeusWSSchema[Fare_InformativeBestPricingWithoutPNR], ttProviderSystems.AmadeusWSSchema[Fare_InformativeBestPricingWithoutPNRReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendFlightInfo(AmadeusWSAdapter ttAA, string strRequest)
        {
            try
            {
                return SendGDSMessage(ttAA, strRequest, ttProviderSystems.AmadeusWSSchema[Air_FlightInfo], ttProviderSystems.AmadeusWSSchema[Air_FlightInfoReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendRetrievePNR(AmadeusWSAdapter ttAA)
        {
            try
            {
                var request = $"<PNR_Retrieve><settings><options><optionCode>51</optionCode></options></settings><retrievalFacts><retrieve><type>1</type></retrieve></retrievalFacts></PNR_Retrieve>";
                return SendRetrievePNR(ttAA, request);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Retrieve PNR Message.\r\n{ex.Message}");
            }
        }

        protected string SendSplitPNR(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                var response = ttAA.SendMessage(request, "", $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/{ttProviderSystems.AmadeusWSSchema[PNR_Split]}", ConversationID);
                response = response.Replace($" xmlns=\"http://xml.amadeus.com/{ttProviderSystems.AmadeusWSSchema[PNR_Reply]}\"", "");
                response = response.Replace($" xmlns=\"http://xml.amadeus.com/{ttProviderSystems.AmadeusWSSchema[PNR_Reply1]}\"", "");
                ConversationID = UpdateSessionID(ConversationID);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Split PNR.\r\n{ex.Message}");
            }
        }

        protected string SendGetPricingOptions(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Ticket_GetPricingOptions], ttProviderSystems.AmadeusWSSchema[Ticket_GetPricingOptionsReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendETicketProcess(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Ticket_ProcessETicket], ttProviderSystems.AmadeusWSSchema[Ticket_ProcessETicketReply]);
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected string SendPricePNRWithBookingClass(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Fare_PricePNRWithBookingClass], ttProviderSystems.AmadeusWSSchema[Fare_PricePNRWithBookingClassReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Price PNR With Booking Class.\r\n{ex.Message}");
            }

        }

        protected string SendPricePNRWithLowerFares(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Fare_PricePNRWithLowerFares], ttProviderSystems.AmadeusWSSchema[Fare_PricePNRWithLowerFaresReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Price PNR With Booking Class.\r\n{ex.Message}");
            }

        }

        protected string SendCreateTSTFromPricing(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Ticket_CreateTSTFromPricing], ttProviderSystems.AmadeusWSSchema[Ticket_CreateTSTFromPricingReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendUpdateTST(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Ticket_UpdateTST], ttProviderSystems.AmadeusWSSchema[Ticket_UpdateTSTReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendQueueRequest(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[QueueMode_ProcessQueue], ttProviderSystems.AmadeusWSSchema[QueueMode_ProcessQueueReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Queue Request.\r\n{ex.Message}");
            }
        }

        protected string SendAirSellFromRecommendation(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Air_SellFromRecommendation], ttProviderSystems.AmadeusWSSchema[Air_SellFromRecommendationReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Queue Request.\r\n{ex.Message}");
            }
        }

        protected string SendGetFareFamilyDescription(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Fare_GetFareFamilyDescription], ttProviderSystems.AmadeusWSSchema[Fare_GetFareFamilyDescriptionReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendRetrieveSeatMap(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Air_RetrieveSeatMap], ttProviderSystems.AmadeusWSSchema[Air_RetrieveSeatMap]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendMasterPricerExpertSearch(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Fare_MasterPricerExpertSearch], ttProviderSystems.AmadeusWSSchema[Fare_MasterPricerExpertSearch]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendMasterPricerTravelBoardSearch(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Fare_MasterPricerTravelBoardSearch], ttProviderSystems.AmadeusWSSchema[Fare_MasterPricerTravelBoardSearchReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendMasterPricerCalendar(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Fare_MasterPricerCalendar], ttProviderSystems.AmadeusWSSchema[Fare_MasterPricerCalendarReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendSellByFareCalendar(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Fare_SellByFareCalendar], ttProviderSystems.AmadeusWSSchema[Fare_SellByFareCalendarReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendSellByFareSearch(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Fare_SellByFareSearch], ttProviderSystems.AmadeusWSSchema[Fare_SellByFareSearchReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCarPolicy(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Car_Policy], ttProviderSystems.AmadeusWSSchema[Car_PolicyReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCarSingleAvailability(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Car_SingleAvailability], ttProviderSystems.AmadeusWSSchema[Car_SingleAvailabilityReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCarMultiAvailability(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Car_MultiAvailability], ttProviderSystems.AmadeusWSSchema[Car_MultiAvailabilityReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCarRateInformationFromAvailability(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Car_RateInformationFromAvailability], ttProviderSystems.AmadeusWSSchema[Car_RateInformationFromAvailabilityReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCarLocationList(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Car_LocationList], ttProviderSystems.AmadeusWSSchema[Car_LocationListReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCarInformationImage(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Car_InformationImage], ttProviderSystems.AmadeusWSSchema[Car_InformationImageReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected string SendCarSell(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Car_Sell], ttProviderSystems.AmadeusWSSchema[Car_SellReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCarAvailability(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Car_Availability], ttProviderSystems.AmadeusWSSchema[Car_AvailabilityReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendHotelSingleAvailability(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Hotel_SingleAvailability], ttProviderSystems.AmadeusWSSchema[Hotel_SingleAvailabilityReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendHotelStructuredPricing(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Hotel_StructuredPricing], ttProviderSystems.AmadeusWSSchema[Hotel_StructuredPricingReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendHotelAvailabilityMultiProperties(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Hotel_AvailabilityMultiProperties], ttProviderSystems.AmadeusWSSchema[Hotel_AvailabilityMultiPropertiesReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendHotelMultiSingleAvailability(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Hotel_MultiSingleAvailability], ttProviderSystems.AmadeusWSSchema[Hotel_MultiSingleAvailabilityReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendHotelDescriptiveInfo(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Hotel_DescriptiveInfo], ttProviderSystems.AmadeusWSSchema[Hotel_DescriptiveInfoReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendHotelFeatures(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Hotel_Features], ttProviderSystems.AmadeusWSSchema[Hotel_FeaturesReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendHotelList(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Hotel_List], ttProviderSystems.AmadeusWSSchema[Hotel_ListReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendHotelEnhancedPricing(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Hotel_EnhancedPricing], ttProviderSystems.AmadeusWSSchema[Hotel_EnhancedPricingReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendHotelRateChange(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Hotel_RateChange], ttProviderSystems.AmadeusWSSchema[Hotel_RateChangeReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendHotelTerms(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Hotel_Terms], ttProviderSystems.AmadeusWSSchema[Hotel_TermsReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendHotelSell(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Hotel_Sell], ttProviderSystems.AmadeusWSSchema[Hotel_SellReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendIssueTicket(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[DocIssuance_IssueTicket], ttProviderSystems.AmadeusWSSchema[DocIssuance_IssueTicketReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Issue Ticket.\r\n{ex.Message}");
            }
        }

        protected string SendCancelDocument(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Ticket_CancelDocument], ttProviderSystems.AmadeusWSSchema[Ticket_CancelDocumentReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Cancel Document.\r\n{ex.Message}");
            }
        }

        protected string SendCheckEligibility(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Ticket_CheckEligibility], ttProviderSystems.AmadeusWSSchema[Ticket_CheckEligibilityReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Cancel Document.\r\n{ex.Message}");
            }
        }

        protected string SendProcessEDoc(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Ticket_ProcessEDoc], ttProviderSystems.AmadeusWSSchema[Ticket_ProcessEDocReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Cancel Document.\r\n{ex.Message}");
            }
        }

        protected string SendRepricePNRWithBookingClass(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Ticket_RepricePNRWithBookingClass], ttProviderSystems.AmadeusWSSchema[Ticket_RepricePNRWithBookingClassReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Cancel Document.\r\n{ex.Message}");
            }
        }

        protected string SendAutomaticUpdate(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Ticket_AutomaticUpdate], ttProviderSystems.AmadeusWSSchema[Ticket_AutomaticUpdateReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendDisplayQueryReport(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[SalesReports_DisplayQueryReport], ttProviderSystems.AmadeusWSSchema[SalesReports_DisplayQueryReportReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendTicketCreditCardCheck(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Ticket_CreditCardCheck], ttProviderSystems.AmadeusWSSchema[Ticket_CreditCardCheckReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendQueuePlacePNR(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Queue_PlacePNR], ttProviderSystems.AmadeusWSSchema[Queue_PlacePNRReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendQueueMoveItem(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Queue_MoveItem], ttProviderSystems.AmadeusWSSchema[Queue_MoveItemReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendQueueRemoveItem(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Queue_RemoveItem], ttProviderSystems.AmadeusWSSchema[Queue_RemoveItemReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendCalculateRefund(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[DocRefund_CalculateRefund], ttProviderSystems.AmadeusWSSchema[DocRefund_CalculateRefundReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendIgnoreRefund(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[DocRefund_IgnoreRefund], ttProviderSystems.AmadeusWSSchema[DocRefund_IgnoreRefundReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendInitRefund(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[DocRefund_InitRefund], ttProviderSystems.AmadeusWSSchema[DocRefund_InitRefundReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendProcessRefund(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[DocRefund_ProcessRefund], ttProviderSystems.AmadeusWSSchema[DocRefund_ProcessRefundReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendSearchRefund(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[DocRefund_SearchRefundRule], ttProviderSystems.AmadeusWSSchema[DocRefund_SearchRefundRuleReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendUpdateRefund(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[DocRefund_UpdateRefund], ttProviderSystems.AmadeusWSSchema[DocRefund_UpdateRefundReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendSecurityAuthenticate(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Security_Authenticate], ttProviderSystems.AmadeusWSSchema[Security_AuthenticateReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendSecuritySignOut(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Security_SignOut], ttProviderSystems.AmadeusWSSchema[Security_SignOutReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendGetFromPricing(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[MiniRule_GetFromPricing], ttProviderSystems.AmadeusWSSchema[MiniRule_GetFromPricingReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendGetFromPricingRecLoc(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[MiniRule_GetFromPricingRec], ttProviderSystems.AmadeusWSSchema[MiniRule_GetFromPricingRecReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendFareFlexPriceUpsell(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Fare_FlexPricerUpsell], ttProviderSystems.AmadeusWSSchema[Fare_FlexPricerUpsellReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendShopperMasterPricerTravelBoardSearch(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Ticket_ATCShopperMasterPricerTravelBoardSearch], ttProviderSystems.AmadeusWSSchema[Ticket_ATCShopperMasterPricerTravelBoardSearchReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendPNRTransferOwnership(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[PNR_TransferOwnership], ttProviderSystems.AmadeusWSSchema[PNR_TransferOwnershipReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendFareQuoteItinerary(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Fare_QuoteItinerary], ttProviderSystems.AmadeusWSSchema[Fare_QuoteItineraryReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendFareMetaPricerTravelboardSearch(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Fare_MetaPricerTravelBoardSearch], ttProviderSystems.AmadeusWSSchema[Fare_MetaPricerTravelBoardSearchReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendFareMetaPricerCalendar(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Fare_MetaPricerCalendar], ttProviderSystems.AmadeusWSSchema[Fare_MetaPricerCalendarReply]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Automatic Update.\r\n{ex.Message}");
            }
        }

        protected string SendCruiseRequestSailingAvailability(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_RequestSailingAvailability], ttProviderSystems.AmadeusWSSchema[Cruise_RequestSailingAvailabilityReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCruiseRequestFareAvailability(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_RequestFareAvailability], ttProviderSystems.AmadeusWSSchema[Cruise_RequestFareAvailabilityReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCruiseRequestCategoryAvailability(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_RequestCategoryAvailability], ttProviderSystems.AmadeusWSSchema[Cruise_RequestCategoryAvailabilityReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCruiseRequestCabinAvailability(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_RequestCabinAvailability], ttProviderSystems.AmadeusWSSchema[Cruise_RequestCabinAvailabilityReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCruiseHoldCabin(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_HoldCabin], ttProviderSystems.AmadeusWSSchema[Cruise_HoldCabinReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCruiseUnHoldCabin(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_UnholdCabin], ttProviderSystems.AmadeusWSSchema[Cruise_UnholdCabinReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCruisePriceBooking(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_PriceBooking], ttProviderSystems.AmadeusWSSchema[Cruise_PriceBookingReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCruiseCreateBooking(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_CreateBooking], ttProviderSystems.AmadeusWSSchema[Cruise_CreateBookingReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCruiseGetBookingDetails(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_GetBookingDetails], ttProviderSystems.AmadeusWSSchema[Cruise_GetBookingDetailsReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCruiseCancelBooking(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_CancelBooking], ttProviderSystems.AmadeusWSSchema[Cruise_CancelBookingReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCruiseModifyBooking(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_ModifyBooking], ttProviderSystems.AmadeusWSSchema[Cruise_ModifyBookingReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCruiseRequestPrePostPackageAvailability(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_RequestPrePostPackageAvailability], ttProviderSystems.AmadeusWSSchema[Cruise_RequestPrePostPackageAvailabilityReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCruiseRequestDisplayPrePostPackageDescription(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_DisplayPrePostPackageDescription], ttProviderSystems.AmadeusWSSchema[Cruise_DisplayPrePostPackageDescriptionReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCruiseRequestTransferAvailability(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_RequestTransferAvailability], ttProviderSystems.AmadeusWSSchema[Cruise_RequestTransferAvailabilityReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendCruiseDisplayItineraryDescription(AmadeusWSAdapter ttAA, string request)
        {
            try
            {
                return SendGDSMessage(ttAA, request, ttProviderSystems.AmadeusWSSchema[Cruise_DisplayItineraryDescription], ttProviderSystems.AmadeusWSSchema[Cruise_DisplayItineraryDescriptionReply]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
