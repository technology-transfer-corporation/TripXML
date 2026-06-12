using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using TripXMLMain;

namespace Galileo
{
    public class GalileoAdapter : GalileoBase
    {
        protected GalileoSoapClient ows = null;
        private XmlDocument mxmlFilter = null;
        private string mstrProfile = "";
        private string mstrSystem = "";
        private string mstrUserID = "";
        private CredentialCache oCc;
        private int MaximumCount = 0;
        public int InitialBlockSize = 0;
        private int NextBlockSize = 0;
        private int SessionCount = 0;
        private int BlockIDNum = 1;
        private char IsInitialBlock = 'Y';
        private string BlockID = "";
        private int NewSessions = 0;
        // static int counts = 0;

        public string TracerID { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="provider">TripXML provider object</param>
        public GalileoAdapter(modCore.TripXMLProviderSystems provider, string version = "")
        {
            NetworkCredential oNc;
            try
            {
                provider.UserName = !provider.UserName.Contains("GWS/") ? $"GWS/{provider.UserName}" : provider.UserName;

                ProviderSystems = provider;

                // Prod and copy systems differ only in endpoint URL and SOAPAction prefix
                // (the legacy wsGalileoProd/wsGalileoCopy proxies); GalileoSoapClient keeps both.
                ows = new GalileoSoapClient(ProviderSystems.URL, ProviderSystems.System == "Production");

                mstrProfile = ProviderSystems.Profile.Text;
                mstrSystem = ProviderSystems.System;
                mstrUserID = ProviderSystems.UserID;
                oNc = new NetworkCredential(ProviderSystems.UserName, ProviderSystems.Password);
                oCc = new CredentialCache();

                oCc.Add(new Uri(ows.Url.ToString()), "Basic", oNc);
                ows.Timeout = 60000; // 1 Minute
                ows.Credentials = oCc;
                ows.PreAuthenticate = true;

                // Creating Filter
                mxmlFilter = new XmlDocument();
                mxmlFilter.LoadXml("<_/>");
            }
            catch (Exception ex)
            {
                throw new Exception("Error Add the Network Credentials to the Providers Web Services.", ex);
            }

            if (!string.IsNullOrEmpty(version))
            {
                try
                {
                    cDA oDa = null;
                    oDa = new cDA("ConnectionString");
                    //providerSystems = providerSystems;
                    ProviderSystems = oDa.SetPCCBlock(ProviderSystems);
                    // ProviderSystems = oDa.SetPCCBlock(ProviderSystems,"system")

                    MaximumCount = ProviderSystems.ProviderSession.MaximumCount;
                    InitialBlockSize = ProviderSystems.ProviderSession.InitialBlockSize;
                    NextBlockSize = ProviderSystems.ProviderSession.NextBlockSize;
                    SessionCount = ProviderSystems.ProviderSession.SessionsUsed;
                    oDa.Dispose();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error Extracting Session Pooling Info of Providers Web Services.", ex);
                }
            }
        }

        public string CreateSession()
        {
            try
            {
                if (ows == null)
                {
                    ows = new GalileoSoapClient(ProviderSystems.URL, ProviderSystems.System == "Production");
                }
                // CoreLib.SendTrace(sb.Append($"{mstrUserID}").ToString(), "ttGalileoAdapter", "Create Session", mstrProfile, String.Empty)
                CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoAdapter", "Create Session", $"Creating session for \"{mstrProfile}\" - profile", ProviderSystems.LogUUID);
                string token = ows.BeginSession(mstrProfile).ToString();

                if (!string.IsNullOrEmpty(ProviderSystems.AAAPCC) & (ProviderSystems.AAAPCC ?? "") != (ProviderSystems.PCC ?? ""))
                {
                    Emulate(token);
                }
                // Write a procedure that will write a Log file with information on just opened session for SPLUNK
                var trace = new JObject(new JProperty("Provider", ProviderSystems.Provider), new JProperty("ID", token), new JProperty("Type", "Open"), new JProperty("User", ProviderSystems.UserID), new JProperty("UUID", ProviderSystems.LogUUID), new JProperty("TimeStamp", DateTime.Now));
                string argmessage = "Session Manager";
                modCore.AddLog(modCore.LogType.Info, ref argmessage, ProviderSystems, trace);
                //CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Session Created", token, string.Empty);
                CoreLib.SendTrace(ProviderSystems.UserID, $"ttGalileoAdapter", "Session Created", token, ProviderSystems.LogUUID);

                return token;
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace(ProviderSystems.UserID, $"ttGalileoAdapter", "Session Created Error", $"Failed to create session for \"{mstrProfile}\" - profile", ProviderSystems.LogUUID);
                throw ex;
            }
        }

        public void Emulate(string sessionid)
        {
            /*************************** 
            * Emulate into another PCC *                                       
            ***************************/
            if (ProviderSystems.AAAPCC != ProviderSystems.PCC)
            {
                string strAAA = $"<SessionEmulation_1_0><SessionMods><EmulatePCC><PCC>{ProviderSystems.AAAPCC}</PCC><Duty>AG</Duty></EmulatePCC></SessionMods></SessionEmulation_1_0>";
                string strResponse = SendMessage(strAAA, sessionid);
            }
        }

        public void CreateSessionV2()
        {
            string Token = "";
            modCore.IsCreating = true;
            string length = "";
            string password = "";
            cDA oDa = null;
            var CreatedTime = DateTime.Now;
            var LastMessageTime = DateTime.Now;

            // Block Naming
            BlockID = $"B{BlockIDNum}";

            CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreAdapter", "Create Session", "", ProviderSystems.LogUUID);
            try
            {
                oDa = new cDA("ConnectionString");
                length = ProviderSystems.Password.Substring(0, 2);
                password = ProviderSystems.Password.Substring(2);
                CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Create Session", mstrProfile, ProviderSystems.LogUUID);

                try
                {
                    Token = ows.BeginSession(mstrProfile).ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Session Not Created. {ex.Message}");
                }

                // oDa.InsertNewSession(Token, 1, ProviderSystems.Provider, CreatedTime, LastMessageTime, ProviderSystems.UserID, "Active", "N", "N", ProviderSystems.URL, BlockID, IsInitialBlock, ProviderSystems.PCC, ProviderSystems.Profile, ProviderSystems.System, ProviderSystems.GPass)
                oDa.InsertNewSession(Token, 1, ProviderSystems.Provider, CreatedTime, LastMessageTime, ProviderSystems.UserName, ProviderSystems.UserID, "Active", 'N', 'N', ProviderSystems.URL, BlockID, IsInitialBlock, ProviderSystems.PCC, ProviderSystems.Profile.Text, ProviderSystems.System, ProviderSystems.GPass);
                modCore.IsCreating = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                modCore.IsCreating = false;
                oDa.Dispose();
            }
        }

        public string CheckSessionV3()
        {
            string SecurityToken = "";
            string Header = "";
            string Body = "";
            string strAAA = "";
            string strResponse = "";
            cDA oDa = null;
            int SessionPoolSize = 0;
            int NumOfNextBlocks = 0;
            int RemainingNumOfSessions = 0;

            //while (modCore.IsCreating)
            //{
            //}

            try
            {
                oDa = new cDA("ConnectionString");

                // Check in tblSessionPool for available Sessions 

                if (oDa.CheckAvailableSessions(ProviderSystems.PCC, ProviderSystems.System, ProviderSystems.UserID))
                {
                    SecurityToken = oDa.SessionUpdate(ProviderSystems.PCC, ProviderSystems.System, ProviderSystems.UserID, ProviderSystems.Provider);
                }
                else
                {

                    // Check PCC has exceeded the Session Limit
                    if (SessionCount < MaximumCount)
                    {
                        NumOfNextBlocks = (int)Math.Round((MaximumCount - InitialBlockSize) / (double)NextBlockSize);
                        RemainingNumOfSessions = (MaximumCount - InitialBlockSize) % NextBlockSize;
                        if (SessionCount != 0)
                        {
                            IsInitialBlock = 'N';
                            BlockIDNum = (int)Math.Round((SessionCount - InitialBlockSize) / (double)NextBlockSize);

                            // Next Block 
                            if (BlockIDNum != NumOfNextBlocks)
                            {
                                SessionPoolSize = NextBlockSize;
                            }
                            // Remaining Block
                            else
                            {
                                SessionPoolSize = RemainingNumOfSessions;
                            }

                            BlockIDNum = BlockIDNum + 2;
                        }


                        // Create one Session in the main Thread
                        modCore.IsCreating = true;
                        CreateSessionV2();
                        SecurityToken = oDa.SessionUpdate(ProviderSystems.PCC, ProviderSystems.System, ProviderSystems.UserID, ProviderSystems.Provider);
                        NewSessions += 1;
                        int i = 0;
                        // Create other Sessions in Threads based on the Block Size
                        var loopTo = SessionPoolSize - 2;
                        for (i = 0; i <= loopTo; i++)
                        {
                            var CreateSessionThread = new Thread(new ThreadStart(CreateSessionV2));
                            CreateSessionThread.Start();
                            NewSessions += 1;
                        }

                        modCore.IsCreating = false;
                    }
                    // Save SessionCount in the DB
                    oDa.UpdatePCCSessions(ProviderSystems.PCC, NewSessions, ProviderSystems.UserID);
                    // oDa.UpdatePCCSessions(ProviderSystems.PCC, NewSessions, ProviderSystems.UserID, ProviderSystems.System);
                }

                return SecurityToken;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDa.Dispose();
            }
        }

        public void CloseSession(string SessionToken)
        {
            CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Close Session", SessionToken, ProviderSystems.LogUUID);
            ows.EndSession(SessionToken);
        }

        public string CloseSessionFromPool(string SecurityToken)
        {
            var oCDa = new cDA("ConnectionString");
            bool blTest = oCDa.CheckSessionWithOutSequence(SecurityToken);
            if (!blTest)
            {
                try
                {
                    CloseSession(SecurityToken);
                    return "";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return "";
            }
        }

        public string SendMessage(string Message, string SessionToken = "")
        {
            XmlDocument oReqDoc;
            XmlElement xmlResponse;
            var startTime = default(DateTime);
            try
            {
                oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(Message);
            }
            catch (Exception ex)
            {
                throw new Exception("ttAdapter: Error Loading Message Request.");
            }

            try
            {
                CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Send to Galileo", oReqDoc.DocumentElement.OuterXml, ProviderSystems.LogUUID);
                startTime = DateTime.Now;
                var requesttime = DateTime.Now;


                xmlResponse = string.IsNullOrEmpty(SessionToken)
                   ? ows.SubmitXml(mstrProfile, oReqDoc.DocumentElement, mxmlFilter.DocumentElement)
                   : ows.SubmitXmlOnSession(SessionToken, oReqDoc.DocumentElement, mxmlFilter.DocumentElement);

                var responsetime = DateTime.Now;

                CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Receive from Galileo", xmlResponse.OuterXml, ProviderSystems.LogUUID);
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                int ind;
                ind = errText.IndexOf("HRESULT:");
                if (ind != -1)
                {
                    errText = $"Data invalid in Galileo request. Exception: {errText.Substring(ind + 9, 10)}";
                }
                else if (errText.IndexOf("Data too long") != -1)
                {
                    errText = "Data too long in Galileo request.";
                }
                else if (errText.IndexOf("--->") != -1)
                {
                    errText = errText.Substring(0, errText.IndexOf("--->") - 1);
                }
                else if (errText.IndexOf("-->") != -1)
                {
                    errText = errText.Substring(0, errText.IndexOf("-->") - 1);
                }

                CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Galileo exception error", errText, ProviderSystems.LogUUID);
                throw new Exception(errText, ex);
            }
            finally
            {
                var sb2 = new StringBuilder();
                CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", sb2.Append("Galileo Response Time = ").Append(Convert.ToInt32(DateTime.Now.Subtract(startTime).TotalSeconds)).Append(" seconds.").ToString(), "", ProviderSystems.LogUUID);
                sb2.Remove(0, sb2.Length);
            }

            return xmlResponse.OuterXml;
        }

        public string SendCrypticMessage(string Message, string sessionToken)
        {
            string strScreen;
            string strResponse;
            try
            {
                CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Send to Galileo", Message, ProviderSystems.LogUUID);

                strResponse = ows.SubmitTerminalTransaction(sessionToken, Message, "").ToString();

                strResponse = strResponse.Replace("<", "&lt;").Replace(">", "&gt;");
                CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Receive from Galileo", strResponse, ProviderSystems.LogUUID);
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                int ind;
                ind = errText.IndexOf("HRESULT:");

                if (ind != -1)
                {
                    errText = $"Data invalid in Galileo request. Exception: {errText.Substring(ind + 9, 10)}";

                }
                else if (errText.IndexOf("Data too long") != -1)
                {
                    errText = "Data too long in Galileo request.";
                }
                else if (errText.IndexOf("--->") != -1)
                {
                    errText = errText.Substring(0, errText.IndexOf("--->") - 1);
                }
                else if (errText.IndexOf("-->") != -1)
                {
                    errText = errText.Substring(0, errText.IndexOf("-->") - 1);
                }

                CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Galileo exception error", errText, ProviderSystems.LogUUID);
                throw new Exception(errText, ex);
            }

            return strResponse;
        }

        public string SendImageViewer(string Message)
        {

            XmlDocument oReqDoc = null;
            XmlElement xmlResponse = null;
            try
            {
                oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(Message);
                Message = $"<ImageSetRequest xmlns='http://www.galileo.com/GI_GDS'><HostAccessProfile>{mstrProfile}</HostAccessProfile>{oReqDoc.DocumentElement.InnerXml}</ImageSetRequest>";
                oReqDoc.LoadXml(Message);
            }
            catch (Exception ex)
            {
                throw new Exception("ttAdapter: Error Loading Message Request.");
            }

            try
            {
                CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Send to Galileo", oReqDoc.DocumentElement.OuterXml, ProviderSystems.LogUUID);

                // The legacy wsGalileoProdIV/wsGalileoCopyIV proxies were identical (same default
                // URL and SOAPAction); a fresh client per call keeps the proxy-default 100s timeout.
                var owsIV = new GalileoSoapClient(GalileoSoapClient.DefaultImageViewerUrl, mstrSystem == "Production");
                owsIV.Credentials = oCc;
                xmlResponse = owsIV.RetrievePhotoInformation(oReqDoc.DocumentElement);

                CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Receive from Galileo", xmlResponse.OuterXml, ProviderSystems.LogUUID);

            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Request to Galileo.\r\n{ex.Message}");

            }

            return xmlResponse.OuterXml;
        }

        public string SendMultiMessage(string Message)
        {
            string strResponse;

            try
            {
                XmlDocument oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(Message);

                CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Send to Galileo", oReqDoc.DocumentElement.OuterXml, ProviderSystems.LogUUID);

                XmlElement xmlResponse = ows.MultiSubmitXml(mstrProfile, oReqDoc.DocumentElement);
                strResponse = xmlResponse.OuterXml;
                CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Receive from Galileo", strResponse, ProviderSystems.LogUUID);

            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Galileo exception error", errText, ProviderSystems.LogUUID);

                throw new Exception(errText, ex);
            }

            return strResponse;
        }

        public string formatGalileo(string strDisp, string id = "")
        {
            strDisp = strDisp.Replace("<", "&lt;").Replace(">", "&gt;");
            return strDisp;
        }

    }
}