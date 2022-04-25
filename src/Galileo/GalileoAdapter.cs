using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using TripXMLMain;

namespace Galileo
{
    public class GalileoAdapter : GalileoBase
    {
        protected object ows = null;
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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            try
            {
                provider.UserName = !provider.UserName.Contains("GWS/") ? $"GWS/{provider.UserName}" : provider.UserName;
                ProviderSystems = provider;

                ows = ProviderSystems.System == "Production"
                    ? new wsGalileoProd.XmlSelect { Url = ProviderSystems.URL }
                    : new wsGalileoCopy.XmlSelect { Url = ProviderSystems.URL };

                mstrProfile = ProviderSystems.Profile;
                mstrSystem = ProviderSystems.System;
                mstrUserID = ProviderSystems.UserID;
                oNc = new NetworkCredential(ProviderSystems.UserName, ProviderSystems.Password);
                oCc = new CredentialCache();


                if (ProviderSystems.System == "Production")
                {
                    oCc.Add(new Uri((ows as wsGalileoProd.XmlSelect).Url.ToString()), "Basic", oNc);
                    ((wsGalileoProd.XmlSelect)ows).Timeout = 60000; // 1 Minute
                    ((wsGalileoProd.XmlSelect)ows).Credentials = oCc;
                    ((wsGalileoProd.XmlSelect)ows).PreAuthenticate = true;
                }
                else
                {
                    oCc.Add(new Uri(((wsGalileoCopy.XmlSelect)ows).Url.ToString()), "Basic", oNc);
                    ((wsGalileoCopy.XmlSelect)ows).Timeout = 60000; // 1 Minute
                    ((wsGalileoCopy.XmlSelect)ows).Credentials = oCc;
                    ((wsGalileoCopy.XmlSelect)ows).PreAuthenticate = true;
                }


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
                    ows = ProviderSystems.System == "Production"
                            ? new wsGalileoProd.XmlSelect { Url = ProviderSystems.URL }
                            : new wsGalileoCopy.XmlSelect { Url = ProviderSystems.URL };
                }
                // CoreLib.SendTrace(sb.Append($"{mstrUserID}").ToString(), "ttGalileoAdapter", "Create Session", mstrProfile, String.Empty)
                CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoAdapter", "Create Session", "", ProviderSystems.LogUUID);
                string token = ProviderSystems.System == "Production"
                    ? (ows as wsGalileoProd.XmlSelect).BeginSession(mstrProfile).ToString()
                    : ((wsGalileoCopy.XmlSelect)ows).BeginSession(mstrProfile).ToString();

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
                throw ex;
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
                    switch (ProviderSystems.System)
                    {
                        case "Production":
                            Token = (ows as wsGalileoProd.XmlSelect).BeginSession(mstrProfile).ToString();
                            break;
                        default:
                            Token = ((wsGalileoCopy.XmlSelect)ows).BeginSession(mstrProfile).ToString();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Session Not Created. {ex.Message}");
                }

                // oDa.InsertNewSession(Token, 1, ProviderSystems.Provider, CreatedTime, LastMessageTime, ProviderSystems.UserID, "Active", "N", "N", ProviderSystems.URL, BlockID, IsInitialBlock, ProviderSystems.PCC, ProviderSystems.Profile, ProviderSystems.System, ProviderSystems.GPass)
                oDa.InsertNewSession(Token, 1, ProviderSystems.Provider, CreatedTime, LastMessageTime, ProviderSystems.UserName, ProviderSystems.UserID, "Active", 'N', 'N', ProviderSystems.URL, BlockID, IsInitialBlock, ProviderSystems.PCC, ProviderSystems.Profile, ProviderSystems.System, ProviderSystems.GPass);
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
            switch (ProviderSystems.System)
            {
                case "Production":
                    (ows as wsGalileoProd.XmlSelect).EndSession(SessionToken);
                    break;
                default:
                    ((wsGalileoCopy.XmlSelect)ows).EndSession(SessionToken);
                    break;
            }

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


                switch (ProviderSystems.System)
                {
                    case "Production":
                        xmlResponse = string.IsNullOrEmpty(SessionToken)
                           ? (ows as wsGalileoProd.XmlSelect).SubmitXml(mstrProfile, oReqDoc.DocumentElement, mxmlFilter.DocumentElement)
                           : (ows as wsGalileoProd.XmlSelect).SubmitXmlOnSession(SessionToken, oReqDoc.DocumentElement, mxmlFilter.DocumentElement);
                        break;
                    default:
                        xmlResponse = string.IsNullOrEmpty(SessionToken)
                           ? (ows as wsGalileoCopy.XmlSelect).SubmitXml(mstrProfile, oReqDoc.DocumentElement, mxmlFilter.DocumentElement)
                           : (ows as wsGalileoCopy.XmlSelect).SubmitXmlOnSession(SessionToken, oReqDoc.DocumentElement, mxmlFilter.DocumentElement);
                        break;
                }

                var responsetime = DateTime.Now;
                if (ProviderSystems.AddLog)
                {
                    addSoapLog(Message + Environment.NewLine + xmlResponse.OuterXml, requesttime, responsetime, ProviderSystems.PCC, ProviderSystems.UserID);
                    // addSoapLog(xmlResponse.OuterXml, requesttime, responsetime, ProviderSystems.PCC, ProviderSystems.UserID)
                }

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

                switch (ProviderSystems.System)
                {
                    case "Production":
                        strResponse = (ows as wsGalileoProd.XmlSelect).SubmitTerminalTransaction(sessionToken, Message, "").ToString();
                        break;
                    default:
                        strResponse = (ows as wsGalileoCopy.XmlSelect).SubmitTerminalTransaction(sessionToken, Message, "").ToString();
                        break;
                }

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
                wsGalileoProdIV.ImageViewer owsPrdIV = null;
                wsGalileoCopyIV.ImageViewer owsCopyIV = null;

                CoreLib.SendTrace($"{mstrUserID}", "ttGalileoAdapter", "Send to Galileo", oReqDoc.DocumentElement.OuterXml, ProviderSystems.LogUUID);

                if (mstrSystem == "Production")
                {
                    owsPrdIV = new wsGalileoProdIV.ImageViewer();
                    owsPrdIV.Credentials = oCc;
                    xmlResponse = owsPrdIV.RetrievePhotoInformation(oReqDoc.DocumentElement);
                }
                else
                {
                    owsCopyIV = new wsGalileoCopyIV.ImageViewer();
                    owsCopyIV.Credentials = oCc;
                    xmlResponse = owsCopyIV.RetrievePhotoInformation(oReqDoc.DocumentElement);
                }

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

                XmlElement xmlResponse;
                switch (ProviderSystems.System)
                {
                    case "Production":
                        xmlResponse = (ows as wsGalileoProd.XmlSelect).MultiSubmitXml(mstrProfile, oReqDoc.DocumentElement);
                        break;
                    default:
                        xmlResponse = ((wsGalileoCopy.XmlSelect)ows).MultiSubmitXml(mstrProfile, oReqDoc.DocumentElement);
                        break;
                }
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

        private void addSoapLog(string msg, DateTime starttime, DateTime endtime, string username, string userid)
        {
            try
            {
                TripXMLTools.TripXMLLog.LogSoapMessage(msg, starttime, endtime, username, TracerID);
            }
            catch (Exception generatedExceptionName)
            {
            }
        }

        private void addLog(string msg, string username)
        {
            TripXMLTools.TripXMLLog.LogErrorMessage(msg, username, TracerID);
        }

        public string formatGalileo(string strDisp, string id = "")
        {
            strDisp = strDisp.Replace("<", "&lt;").Replace(">", "&gt;");
            return strDisp;
        }
    }
}