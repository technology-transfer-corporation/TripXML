using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using TripXMLMain;
using TripXMLTools;

namespace Sabre
{
    public class SabreAdapter
    {
        private enum enRequestType
        {
            CreateSession = 1,
            CloseSession = 2,
            Message = 3
        }
        private const string CFromPartyID = "downtowntravel.com";
        private const string CToPartyID = "webservices.sabre.com";
        private const string ConvID = "itadmin@downtowntravel.com";

        // Private ttProviderSystems As TripXMLProviderSystems
        private modCore.TripXMLProviderSystems ttProviderSystems;
        private int MaximumCount = 0;
        public int InitialBlockSize = 0;
        private int NextBlockSize = 0;
        private int SessionCount = 0;
        private int BlockIDNum = 1;
        private char IsInitialBlock = 'Y';
        private string BlockID = "";
        private int NewSessions = 0;
        // static int counts = 0;
        private const char CRLOR = 'ç';
        private const char CHGKEY = '¥';
        private const char CURSIGN = '¤';
        private const char ENDITEM = '§';

        public SabreAdapter(modCore.TripXMLProviderSystems providerSystems)
        {
            ttProviderSystems = providerSystems;
        }

        public SabreAdapter(modCore.TripXMLProviderSystems providerSystems, string version)
        {
            cDA oDa = new cDA("ConnectionString");
            try
            {
                ttProviderSystems = oDa.SetPCCBlock(providerSystems);
                MaximumCount = ttProviderSystems.ProviderSession.MaximumCount;
                InitialBlockSize = ttProviderSystems.ProviderSession.InitialBlockSize;
                NextBlockSize = ttProviderSystems.ProviderSession.NextBlockSize;
                SessionCount = ttProviderSystems.ProviderSession.SessionsUsed;
            }
            catch (Exception)
            {
                ttProviderSystems = providerSystems;
            }
            finally
            {
                oDa.Dispose();
            }
        }

        private string ComposeHeader(string ServiceType, string SabreService, string SabreAction, string SecurityToken = "")
        {
            string strHeader;
            string messageID;
            string timeStamp;
            messageID = $"mid:{DateTime.Now.ToString("yyyyMMdd-hhmmss-ffff")}@tripxml.com";

            timeStamp = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ");
            {
                var withBlock = ttProviderSystems;
                strHeader = $"<MessageHeader xmlns='http://www.ebxml.org/namespaces/messageHeader'><From><PartyId>{CFromPartyID}</PartyId></From><To><PartyId>{ttProviderSystems.URL.Substring(7)}</PartyId></To>";

                if (!string.IsNullOrEmpty(withBlock.AAAPCC) & (withBlock.AAAPCC ?? "") != (withBlock.PCC ?? ""))
                {
                    strHeader += $"<CPAId>{withBlock.AAAPCC}</CPAId>";
                }
                else
                {
                    strHeader += $"<CPAId>{withBlock.PCC}</CPAId>";
                }

                strHeader += $"<ConversationID>{ConvID}</ConversationID><Service type='{ServiceType}'>{SabreService}</Service><Action>{SabreAction}</Action><MessageData><MessageId>{messageID}</MessageId><Timestamp>{timeStamp}</Timestamp></MessageData></MessageHeader>";

                if (SecurityToken.Length == 0)
                {
                    if (withBlock.PCC == "5K3B")
                    {
                        strHeader += $"<Security xmlns='http://schemas.xmlsoap.org/ws/2002/12/secext'><UsernameToken><Username>{withBlock.UserName}</Username><Password>{withBlock.Password}</Password><Organization xmlns=''>YV</Organization><Domain xmlns=''>YV</Domain></UsernameToken></Security>";

                    }
                    else if (withBlock.PCC == "JZR")
                    {
                        strHeader += $"<Security xmlns='http://schemas.xmlsoap.org/ws/2002/12/secext'><UsernameToken><Username>{withBlock.UserName}</Username><Password>{withBlock.Password}</Password><Organization xmlns=''>ET</Organization><Domain xmlns=''>ET</Domain></UsernameToken></Security>";
                    }
                    else if (withBlock.PCC == "WY")
                    {
                        strHeader += $"<Security xmlns='http://schemas.xmlsoap.org/ws/2002/12/secext'><UsernameToken><Username>{withBlock.UserName}</Username><Password>{withBlock.Password}</Password><Organization xmlns=''>WY</Organization><Domain xmlns=''>WY</Domain></UsernameToken></Security>";
                    }
                    else
                    {
                        strHeader += $"<Security xmlns='http://schemas.xmlsoap.org/ws/2002/12/secext'><UsernameToken><Username>{withBlock.UserName}</Username><Password>{withBlock.Password}</Password><Organization xmlns=''>{withBlock.PCC}</Organization><Domain xmlns=''>DEFAULT</Domain></UsernameToken></Security>";
                    }
                }
                else
                {
                    strHeader += SecurityToken.Replace("&lt;", "<").Replace("&gt;", ">");
                }
            }

            return strHeader;
        }

        private string Send(string Header, string Body)
        {
            ttHttpWebClient oHttpWebClient;
            string strResponse;
            try
            {
                oHttpWebClient = new ttHttpWebClient();
                oHttpWebClient.SoapAction = ttProviderSystems.SOAPAction;
                oHttpWebClient.ServiceURL = ttProviderSystems.URL;
                oHttpWebClient.HttpMethod = "POST";
                oHttpWebClient.Header = Header;
                oHttpWebClient.Body = Body;
                strResponse = oHttpWebClient.SendHttpRequest(ttProviderSystems.UserID, strUUID: ttProviderSystems.LogUUID);
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Request to Sabre.\r\n{ex.Message}");
            }
        }

        private string Send(string Message)
        {
            string strResponse;
            try
            {
                var oHttpWebClient = new ttHttpWebClient();
                {
                    var withBlock = oHttpWebClient;
                    withBlock.SoapAction = ttProviderSystems.SOAPAction;
                    withBlock.ServiceURL = ttProviderSystems.URL;
                    withBlock.HttpMethod = "POST";
                    withBlock.Header = "";
                    withBlock.Body = "";
                    strResponse = withBlock.SendHttpRequest(ttProviderSystems.UserID, Message, ttProviderSystems.LogUUID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return strResponse;
        }
                
        private string GetResponseFromSoap(string strResponse, string sabreService, enRequestType requestType)
        {
            XmlDocument oDoc;
            XmlElement oRoot;
            XmlNode oNode;
            try
            {
                if (requestType != enRequestType.CreateSession)
                {
                    XDocument xDoc = XDocument.Parse(strResponse);
                    xDoc.StripNamespace();
                    strResponse = xDoc.ToString();
                }

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;
                
                // Get The Body
                oNode = oRoot.LastChild;                
                // ********************************
                // Check for Errors in Response  *
                // ********************************
                if (oNode.FirstChild.Name == "soap-env:Fault")
                {
                    oNode = oNode.FirstChild;
                    oNode = oNode.SelectSingleNode("faultstring");
                    throw new Exception(oNode.InnerXml);
                }

                // *************************
                // Get Response From Soap  *
                // *************************
                if (requestType == enRequestType.CreateSession)
                {
                    // Get The Header
                    oNode = oRoot.FirstChild;
                    oNode = oNode.LastChild;
                    strResponse = oNode.OuterXml;
                }
                else
                {
                    // Get The Body
                    oNode = oRoot.LastChild;
                    strResponse = oNode.InnerXml;
                }

                /****************************************************
                 * This is no longer needed since we are using XDocument 
                 * object in order to strip all namespaces
                 * from incomming XML string.
                 ****************************************************
                strResponse = strResponse.Replace(" xmlns=\"http://www.opentravel.org/OTA/2002/08\"", "");
                strResponse = strResponse.Replace(" xmlns=\"http://www.opentravel.org/OTA/2002/11\"", "");
                strResponse = strResponse.Replace(" xmlns:fo=\"http://www.w3.org/1999/XSL/Format\"", "");
                strResponse = strResponse.Replace(" xmlns=\"http://webservices.sabre.com/sabreXML/2003/07\"", "");
                strResponse = strResponse.Replace(" xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\"", "");
                strResponse = strResponse.Replace(" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\"", "");
                strResponse = strResponse.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                strResponse = strResponse.Replace(" xmlns:stl=\"http://services.sabre.com/STL/v01\"", "").Replace("stl:", "");
                strResponse = strResponse.Replace(" xmlns:or=\"http://services.sabre.com/res/or/v1_4\"", "").Replace("or:", "").Replace("xsi:type=\"", "type=\"");
                strResponse = strResponse.Replace(" xmlns=\"http://services.sabre.com/res/tir/v3_6\"", "");
                strResponse = strResponse.Replace(" xmlns:or=\"http://services.sabre.com/res/or/v1_4\"", "");
                strResponse = strResponse.Replace(" xmlns:stl19=\"http://webservices.sabre.com/pnrbuilder/v1_19\"", "").Replace("stl19:", "");
                strResponse = strResponse.Replace(" xmlns:or114=\"http://services.sabre.com/res/or/v1_14\"", "").Replace("or114:", "");
                strResponse = strResponse.Replace(" xmlns:ns6=\"http://services.sabre.com/res/orr/v0\"", "").Replace("ns6:", "");
                strResponse = strResponse.Replace(" xmlns:raw=\"http://tds.sabre.com/itinerary\"", "").Replace("raw:", "");
                strResponse = strResponse.Replace(" xmlns:ns4=\"http://webservices.sabre.com/pnrconn/ReaccSearch\"", "").Replace("ns4:", "");
                strResponse = strResponse.Replace(" xmlns=\"http://www.sabre.com/ns/Ticketing/pqs/1.0\"", "");
                ********************************************************/

                return strResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CreateSession()
        {
            string securityToken;
            CoreLib.SendTrace(ttProviderSystems.UserID, "ttSabreAdapter", "Create Session", "", ttProviderSystems.LogUUID);
            try
            {
                string header = ComposeHeader("sabreXML", "Session", "SessionCreateRQ");
                string body = $"<SessionCreateRQ><POS><Source PseudoCityCode='{ttProviderSystems.PCC}'/></POS></SessionCreateRQ>";
                securityToken = Send(header, body);

                // ****************************************
                // Get SecurityToken From Sabre Response *
                // ****************************************
                securityToken = GetResponseFromSoap(securityToken, "SessionCreate", enRequestType.CreateSession);

                // check if change of AAA required
                if (!string.IsNullOrEmpty(ttProviderSystems.AAAPCC) & (ttProviderSystems.AAAPCC ?? "") != (ttProviderSystems.PCC ?? ""))
                {
                    /************************ 
                    * AAA into another PCC
                    *************************
                    string strAAA = "<ChangeAAARQ xmlns=\"http://webservices.sabre.com/sabreXML/2003/07\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Version=\"2003A.TsabreXML1.0.1\"><POS><Source PseudoCityCode=\"";
                    strAAA += $"{ttProviderSystems.PCC}\"/></POS><AAA PseudoCityCode=\"{ttProviderSystems.AAAPCC}\"/></ChangeAAARQ>";
                    string strResponse = SendMessage(strAAA, "ChangeAAA", "ChangeAAALLSRQ", securityToken);
                    *************************/

                    if (ttProviderSystems.AAAPCC != ttProviderSystems.PCC)
                    {
                        string strAAA = "<ContextChangeRQ  xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" " +
                            "xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" " +
                            "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"  " +
                            "ReturnHostCommand=\"false\" " +
                            "Version=\"2.0.3\"><ChangeAAA PseudoCityCode=\"" +
                            $"{ttProviderSystems.AAAPCC}\"/></ContextChangeRQ>";
                        string strResponse = SendMessage(strAAA, "ChangeAAA", "ContextChangeLLSRQ", securityToken);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return securityToken;
        }

        public void CreateSessionV2()
        {
            
            string strResponse;
            modCore.IsCreating = true;            
            var createdTime = DateTime.Now;
            
            // Block Naming
            BlockID = $"B{BlockIDNum}";
            
            CoreLib.SendTrace(ttProviderSystems.UserID, "ttSabreAdapter", "Create Session", "", ttProviderSystems.LogUUID);
            try
            {
                var oDa = new cDA("ConnectionString");
                var length = ttProviderSystems.Password.Substring(0, 2);
                var password = ttProviderSystems.Password.Substring(2);
                string header = ComposeHeader("sabreXML", "Session", "SessionCreateRQ");
                string body = $"<SessionCreateRQ><POS><Source PseudoCityCode='{ttProviderSystems.PCC}'/></POS></SessionCreateRQ>";
                
                string SecurityToken = Send(header, body);

                // ****************************************
                // Get SecurityToken From Sabre Response *
                // ****************************************

                SecurityToken = GetResponseFromSoap(SecurityToken, "SessionCreate", enRequestType.CreateSession);

                // check if change of AAA required
                if (!string.IsNullOrEmpty(ttProviderSystems.AAAPCC) & (ttProviderSystems.AAAPCC ?? "") != (ttProviderSystems.PCC ?? ""))
                {
                    // AAA into another PCC
                    //string strAAA = "<ChangeAAARQ xmlns=\"http://webservices.sabre.com/sabreXML/2003/07\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Version=\"2003A.TsabreXML1.0.1\"><POS><Source PseudoCityCode=\"";
                    //strAAA += $"{ttProviderSystems.PCC}\"/></POS><AAA PseudoCityCode=\"{ttProviderSystems.AAAPCC}\"/></ChangeAAARQ>";                    
                    //strResponse = SendMessage(strAAA, "ChangeAAA", "ChangeAAALLSRQ", SecurityToken);
                    
                    string strAAA = "<ContextChangeRQ  xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"  Version=\"2.0.3\"><ChangeAAA PseudoCityCode=\"";
                    strAAA += $"{ttProviderSystems.AAAPCC}\"/></ContextChangeRQ>";
                    strResponse = SendMessage(strAAA, "ChangeAAA", "ContextChangeLLSRQ", SecurityToken);
                    
                }

                oDa.InsertNewSession(SecurityToken, 1, ttProviderSystems.Provider, createdTime, DateTime.Now, ttProviderSystems.UserName, ttProviderSystems.UserID, "Active", 'N', 'N', ttProviderSystems.URL, BlockID, IsInitialBlock, ttProviderSystems.PCC, ttProviderSystems.Profile, ttProviderSystems.System, ttProviderSystems.GPass);
                modCore.IsCreating = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                modCore.IsCreating = false;
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
            while (modCore.IsCreating)
            {
            }

            try
            {
                oDa = new cDA("ConnectionString");

                // Check in tblSessionPool for available Sessions 

                if (oDa.CheckAvailableSessions(ttProviderSystems.PCC, ttProviderSystems.System, ttProviderSystems.UserID))
                {
                    SecurityToken = oDa.SessionUpdate(ttProviderSystems.PCC, ttProviderSystems.System, ttProviderSystems.UserID, ttProviderSystems.Provider);
                }
                // strResponse = SendMessageV2(Message, AmadeusWSService, AmadeusWSAction, SessionID);

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
                        SecurityToken = oDa.SessionUpdate(ttProviderSystems.PCC, ttProviderSystems.System, ttProviderSystems.UserID, ttProviderSystems.Provider);
                        // SessionCount++;
                        NewSessions += 1;
                        int i = 0;
                        // Create other Sessions in Threads based on the Block Size
                        var loopTo = SessionPoolSize - 2;
                        for (i = 0; i <= loopTo; i++)
                        {
                            var CreateSessionThread = new Thread(new ThreadStart(CreateSessionV2));
                            CreateSessionThread.Start();
                            // SessionCount++;
                            NewSessions += 1;
                        }

                        modCore.IsCreating = false;
                    }
                    // Save SessionCount in the DB
                    oDa.UpdatePCCSessions(ttProviderSystems.PCC, NewSessions, ttProviderSystems.UserID);
                    // oDa.UpdatePCCSessions(ttProviderSystems.PCC, NewSessions, ttProviderSystems.UserID, ttProviderSystems.System);
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

        public string CloseSession(string SecurityToken)
        {
            
            string strResponse;
            
            CoreLib.SendTrace(ttProviderSystems.UserID, "ttSabreAdapter", "Close Session", "", ttProviderSystems.LogUUID);
            try
            {
                string header = ComposeHeader("sabreXML", "Session", "SessionCloseRQ", SecurityToken);

                // check if change of AAA required
                if (!string.IsNullOrEmpty(ttProviderSystems.AAAPCC) & (ttProviderSystems.AAAPCC ?? "") != (ttProviderSystems.PCC ?? ""))
                {
                    // AAA into another PCC
                    //strAAA = "<ChangeAAARQ xmlns=\"http://webservices.sabre.com/sabreXML/2003/07\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Version=\"2003A.TsabreXML1.0.1\"><POS><Source PseudoCityCode=\"";
                    //strAAA += $"{ttProviderSystems.AAAPCC}\"/></POS><AAA PseudoCityCode=\"{ttProviderSystems.PCC}\"/></ChangeAAARQ>";
                    //strResponse = SendMessage(strAAA, "ChangeAAA", "ChangeAAALLSRQ", SecurityToken);

                    string strAAA = "<ContextChangeRQ  xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"  Version=\"2.0.3\"><ChangeAAA PseudoCityCode=\"";
                    strAAA += $"{ttProviderSystems.AAAPCC}\"/></ContextChangeRQ>";
                    strResponse = SendMessage(strAAA, "ChangeAAA", "ContextChangeLLSRQ", SecurityToken);

                    if (strResponse.Contains("PLEASE FINISH OR IGNORE THE CURRENT TRANSACTION"))
                    {
                        // Ignore current PNR
                        string strER = "<SabreCommandLLSRQ xmlns=\"http://webservices.sabre.com/sabreXML/2011/10\" Version=\"2.0.0\"><Request Output=\"SCREEN\" MDRSubset=\"AD01\" CDATA=\"true\"><HostCommand>I</HostCommand></Request></SabreCommandLLSRQ>";
                        SendMessage(strER, "I", "SabreCommandLLSRQ", SecurityToken);
                        strResponse = SendMessage(strAAA, "ChangeAAA", "ChangeAAALLSRQ", SecurityToken);
                    }
                }

                string body = $"<SessionCloseRQ><POS><Source PseudoCityCode='{ttProviderSystems.PCC}'/></POS></SessionCloseRQ>";
                strResponse = Send(header, body);

                // *************************
                // Get Response From Soap *
                // *************************

                strResponse = GetResponseFromSoap(strResponse, "SessionClose", enRequestType.CloseSession);
                return strResponse;
            }
            catch (Exception ex)
            {
                throw ex;
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
                    return CloseSession(SecurityToken);
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

        public string SendMessage(string Message, string SabreService, string SabreAction, string SecurityToken = "", string cdata = "")
        {
            string Header;
            string Body;
            string strResponse;
            var CloseThisSession = default(bool);
            
            try
            {
                if (SecurityToken.Length == 0)
                {
                    CloseThisSession = true;
                    SecurityToken = CreateSession();
                }
                else
                {
                    CloseThisSession = false;
                }

                Header = ComposeHeader("sabreXML", SabreService, SabreAction, SecurityToken);
                
                Body = Message.Contains("[CDATA[") 
                    ? Message.Replace("&lt;", "<").Replace("&gt;", ">")
                    : Message.Replace("'", CRLOR.ToString()).Replace("^", CHGKEY.ToString()).Replace(@"\", ENDITEM.ToString()).Replace("[", CURSIGN.ToString());
                

                strResponse = Send(Header, Uri.UnescapeDataString(Body));

                // *************************
                // Get Response From Soap *
                // *************************

                strResponse = GetResponseFromSoap(strResponse, SabreService, enRequestType.Message);
                if (CloseThisSession)
                {
                    CloseSession(SecurityToken);
                }

                return strResponse;
            }
            catch (Exception)
            {
                if (CloseThisSession)
                {
                    CloseSession(SecurityToken);
                }

                throw;
            }
        }

        public string SendMessageV3(string Message, string SabreService, string SabreAction, string SecurityToken = "")
        {
            string Header;
            string Body;
            string strResponse;
            string soapResponse;
            bool CloseThisSession = false;
            
            try
            {
                if (SecurityToken.Length == 0)
                {
                    CloseThisSession = true;
                    string soapAction = "";
                    soapAction = SabreAction;
                    // SessionID = CheckSessionV2()
                    SecurityToken = CheckSessionV3();
                    SabreAction = soapAction;
                }
                else
                {
                    CloseThisSession = false;
                }

                // Header = ComposeHeader("AmadeusWSXML", AmadeusWSService, AmadeusWSAction, SessionID);
                Header = ComposeHeader("sabreXML", SabreService, SabreAction, SecurityToken);
                Body = Message.Replace("'", CRLOR.ToString()).Replace("^", CHGKEY.ToString()).Replace(@"\", ENDITEM.ToString()).Replace("[", CURSIGN.ToString());
                strResponse = Send(Header, Body);


                // *************************
                // Get Response From Soap *
                // *************************
                strResponse = GetResponseFromSoap(strResponse, SabreService, enRequestType.Message);
                if (CloseThisSession)
                {

                    // SecurityToken = GetSessionFromSoap(soapResponse)
                    if (!string.IsNullOrEmpty(SecurityToken))
                    {
                        CloseSessionFromPool(SecurityToken);
                    }
                }

                return strResponse;
            }
            catch (Exception ex)
            {
                if (CloseThisSession)
                {
                    CloseSessionFromPool(SecurityToken);
                }

                throw ex;
            }
        }

        public string SendNativeMessage(string Message, string SecurityToken = "")
        {
            string strResponse;
            
            try
            {
                bool CloseThisSession = SecurityToken.Length == 0;

                if (CloseThisSession)
                {                    
                    SecurityToken = CreateSession();
                    Message = Message.Replace("</MessageHeader>", $"</MessageHeader>{SecurityToken}");
                }
                
                strResponse = Send(Message);
                
                if (CloseThisSession)
                {
                    CloseSession(SecurityToken);
                }

                return strResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void addLog(string msg, string username)
        {
            try
            {
                TripXMLLog.LogErrorMessage(msg, username, "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public static class XmlExtensions
    {
        public static void StripNamespace(this XDocument document)
        {
            if (document.Root == null) return;

            foreach (var element in document.Root.DescendantsAndSelf())
            {
                element.Name = element.Name.LocalName;
                element.ReplaceAttributes(GetAttributesWithoutNamespace(element));
            }
        }

        static IEnumerable GetAttributesWithoutNamespace(XElement xElement)
        {
            return xElement.Attributes()
                .Where(x => !x.IsNamespaceDeclaration)
                .Select(x => new XAttribute(x.Name.LocalName, x.Value));
        }
    }
}