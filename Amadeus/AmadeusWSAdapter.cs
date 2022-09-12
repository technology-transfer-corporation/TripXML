using TripXMLMain;
using System.Xml;
using System.Text;
using System;
using System.Data;
using System.Security.Cryptography;
using Microsoft.Web.Services3.Security.Tokens;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using static TripXMLMain.modCore.enAmadeusWSSchema;

public class AmadeusWSAdapter
{
    private modCore.TripXMLProviderSystems ttProviderSystems;

    string newSessionID = "";
    int maximumCount = 0;
    int nextBlockSize = 0;

    int sessionCount = 0;
    char isInitialBlock = 'Y';
    int newSessions = 0;

    //static int counts = 0;
    public string TracerID { get; set; }

    public int InitialBlock { get; private set; }

    public bool isSOAP2 { get; set; }

    public bool isSOAP4 { get; set; }

    public modCore.enSOAPHeaderType SoapHeader { get; set; }

    public bool GetStoredFares { get; set; }

    public AmadeusWSAdapter(modCore.TripXMLProviderSystems providerSystems)
    {
        InitialBlock = 0;
        ttProviderSystems = providerSystems;
        if (providerSystems.SoapHeader != null)
        {
            SoapHeader = providerSystems.SoapHeader;
            switch (SoapHeader)
            {
                case modCore.enSOAPHeaderType.SOAP4:
                    isSOAP2 = false;
                    isSOAP4 = true;
                    break;
                default:
                    isSOAP2 = true;
                    isSOAP4 = false;
                    break;
            }
        }
        else
        {
            isSOAP2 = providerSystems.SOAP2;
            isSOAP4 = providerSystems.SOAP4;
        }
        ServicePointManager.ServerCertificateValidationCallback = delegate
        {
            return true;
        };
    }

    public AmadeusWSAdapter(modCore.TripXMLProviderSystems providerSystems, string version)
    {
        var oDa = new cDA("ConnectionString");

        ttProviderSystems = providerSystems;
        ttProviderSystems = oDa.SetPCCBlock(ttProviderSystems);
        //ttProviderSystems = oDa.SetPCCBlock(ttProviderSystems,"system");

        maximumCount = ttProviderSystems.ProviderSession.MaximumCount;
        InitialBlock = ttProviderSystems.ProviderSession.InitialBlockSize;
        nextBlockSize = ttProviderSystems.ProviderSession.NextBlockSize;
        sessionCount = ttProviderSystems.ProviderSession.SessionsUsed;

        //if (ttProviderSystems.PCC == "MIA1S21AV")
        //{ 
        //    counts++;
        //    if (counts == 1)
        //        ttProviderSystems.ProviderSession.MultipleAccess = false;
        //    else
        //        ttProviderSystems.ProviderSession.MultipleAccess = true;
        //}
        ServicePointManager.ServerCertificateValidationCallback = delegate
        {
            return true;
        };

        oDa.Dispose();
    }

    private string ComposeHeader(string serviceType, string amadeusWSService, string amadeusWSAction, string sessionID)
    {
        string strHeader;
        int intSession;
        string[] sessionid = sessionID.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
        intSession = isSOAP2 ? Convert.ToInt32(sessionid[2]) : Convert.ToInt32(sessionid[1]);
        intSession += 1;

        sessionID = isSOAP2 ? $"{sessionid[0]}|{sessionid[1]}|{intSession}" : $"{sessionid[0]}|{intSession}";
        strHeader = isSOAP2
            ? $"<Session><SessionId>{sessionid[1]}</SessionId><SequenceNumber>{intSession}</SequenceNumber><SecurityToken>{sessionid[0]}</SecurityToken></Session>"
            : $"<SessionId>{sessionID}</SessionId>";
        return strHeader;
    }

    private string ComposeSoap4Header(string amadeusWSAction, ref Soap4Session session)
    {
        var header = new StringBuilder();

        #region Session

        if (session.StatusCode == TransactionStatusCode.Start)
        {
            header.Append($"<ses:Session xmlns:ses=\"http://xml.amadeus.com/2010/06/Session_v3\" TransactionStatusCode=\"Start\"/>\n");
        }

        if (session.StatusCode == TransactionStatusCode.InSeries)
        {
            var sbsession = "<ses:Session xmlns:ses=\"http://xml.amadeus.com/2010/06/Session_v3\" TransactionStatusCode=\"InSeries\">" +
            $"<ses:SessionId>{session.SessionId}</ses:SessionId>" +
            $"<ses:SequenceNumber>{session.SequenceNumber}</ses:SequenceNumber>" +
            $"<ses:SecurityToken>{session.SecurityToken}</ses:SecurityToken></ses:Session>";
            header.Append(sbsession);
        }

        if (session.StatusCode == TransactionStatusCode.End)
        {
            var sbsession = "<ses:Session xmlns:ses=\"http://xml.amadeus.com/2010/06/Session_v3\" TransactionStatusCode=\"End\">" +
            $"<ses:SessionId>{session.SessionId}</ses:SessionId>" +
            $"<ses:SequenceNumber>{session.SequenceNumber}</ses:SequenceNumber>" +
            $"<ses:SecurityToken>{session.SecurityToken}</ses:SecurityToken></ses:Session>";
            header.Append(sbsession);
        }

        #endregion

        #region Addressing

        header.Append("\n").Append($"<wsa:MessageID>{Guid.NewGuid()}</wsa:MessageID>");
        header.Append("\n").Append($"<wsa:Action>http://webservices.amadeus.com/{amadeusWSAction.Substring(amadeusWSAction.LastIndexOf('/') + 1, amadeusWSAction.Length - (amadeusWSAction.LastIndexOf('/') + 1))}</wsa:Action>");
        header.Append("\n").Append($"<wsa:To>{ttProviderSystems.SOAP4URL}/{ttProviderSystems.Profile}</wsa:To>");
        header.Append("\n").Append($"<link:TransactionFlowLink xmlns:link=\"http://wsdl.amadeus.com/2010/06/ws/Link_v1\"/>");

        //var sbAddressing = new StringBuilder();
        //sbAddressing.Append("<wsa:ReplyTo soapenv:mustUnderstand=\"1\">").Append("<wsa:Address xmlns:wsa=\"http://www.w3.org/2005/08/addressing/anonymous\" />").Append("</wsa:ReplyTo>");
        //header.Append("\n").Append(sbAddressing.ToString());
        #endregion

        if (session.StatusCode == TransactionStatusCode.None || session.StatusCode == TransactionStatusCode.Start)
        {

            #region Security
            string trueDigest;
            string created;
            string nonce;

            AuthenticationCore(out nonce, out created, out trueDigest);

            var sbsecuritytoken = "<wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\">" +
            "<wsse:UsernameToken xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" wsse:Id=\"UsernameToken-1\">" +
            $"<wsse:Username>{ttProviderSystems.UserName}</wsse:Username>" +
            $"<wsse:Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\">{nonce}</wsse:Nonce>" +
            $"<wsse:Password Type=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest\">{trueDigest}</wsse:Password>" +
            $"<wsu:Created>{created}</wsu:Created></wsse:UsernameToken></wsse:Security>";

            header.Append("\n").Append(sbsecuritytoken);

            #endregion

            #region Security Hosted user
            var sbsecurityhostedUser = "<AMA_SecurityHostedUser xmlns=\"http://xml.amadeus.com/2010/06/Security_v1\">" +
            $"<UserID POS_Type=\"1\" PseudoCityCode=\"{ttProviderSystems.PCC}\" RequestorType=\"U\"  AgentDutyCode=\"SU\"/></AMA_SecurityHostedUser>";
            header.Append(sbsecurityhostedUser);
            #endregion

        }

        return header.ToString();
    }

    private string CreateNonce(out string created)
    {
        try
        {
            var shaPwd1 = new SHA1Managed();
            byte[] pwd = shaPwd1.ComputeHash(Encoding.UTF8.GetBytes(ttProviderSystems.Password.Substring(2)));

            var unToken = new UsernameToken(ttProviderSystems.UserName, Convert.ToBase64String(pwd), PasswordOption.SendHashed);
            var document = new XmlDocument();
            XmlElement token = unToken.GetXml(document);
            XmlNodeList nonces = token.GetElementsByTagName("Nonce", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            if (nonces.Count == 0 || nonces.Count > 1)
                throw new Exception("Invalid UsernameToken");
            XmlAttribute encodingType = document.CreateAttribute("EncodingType");
            encodingType.Value = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary";
            nonces[0].Attributes.Append(encodingType);
            created = token.GetElementsByTagName("Created", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd").Item(0).InnerText;
            return nonces[0].InnerText;
        }
        catch (Exception ex)
        {
            created = "";
            addLog($"<M>{ex.Message}</M><Send/>", ttProviderSystems.UserID);
            return "";
        }
    }

    private void AuthenticationCore(out string nonce, out string created, out string digest)
    {
        try
        {
            /********************************
            nonce = CreateNonce(out created);            
            var shaPwd1 = new SHA1Managed();
            byte[] pwd = shaPwd1.ComputeHash(Encoding.UTF8.GetBytes(ttProviderSystems.Password));
            byte[] nonceBytes = Convert.FromBase64String(nonce);
            byte[] createdBytes = Encoding.UTF8.GetBytes(created);
            var operand = new byte[nonceBytes.Length + createdBytes.Length + pwd.Length];
            Array.Copy(nonceBytes, operand, nonceBytes.Length);
            Array.Copy(createdBytes, 0, operand, nonceBytes.Length, createdBytes.Length);
            Array.Copy(pwd, 0, operand, nonceBytes.Length + createdBytes.Length, pwd.Length);
            var sha1 = new SHA1Managed();
            digest = Convert.ToBase64String(sha1.ComputeHash(operand));
            *********************************/

            // Get the password and encode it in base64 :
            var base64EncodedBytes = Convert.FromBase64String(ttProviderSystems.Password.Substring(2));
            string passwordClear = Encoding.UTF8.GetString(base64EncodedBytes);
            int PasswordLength = passwordClear.Length;

            //Generate the date. UTC. Format yyyy-MM-ddTHH:mm:ss.fffZ
            DateTime dateTime = DateTime.UtcNow;
            created = dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            //Generate the nonce based on the string with multiple of 4 - lenght and encoded in base64:
            var nonceBytes = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8));
            nonce = Convert.ToBase64String(nonceBytes);

            // Generate the passwordDigest :
            digest = PasswordDigest(nonce, passwordClear, created);
        }
        catch (Exception ex)
        {
            addLog($"<M>{ex.Message}</M><Send/>", ttProviderSystems.UserID);
            digest = nonce = created = "";
        }
    }

    /// <summary>
    /// Generate the PasswordDigest based on the Nonce, the decoded Password in base 64 and the date in UTC format.
    /// </summary>
    /// <param name="nonce"></param>
    /// <param name="password"></param>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    private static string PasswordDigest(string nonce, string password, string timeStamp)
    {
        byte[] decodedNonce = Convert.FromBase64String(nonce);
        byte[] bytePasswd = System.Text.Encoding.UTF8.GetBytes(password);
        SHA1 sha1Passwd = SHA1Managed.Create();
        byte[] hashPasswd = sha1Passwd.ComputeHash(bytePasswd);
        SHA1 sha1Salted = SHA1Managed.Create();
        byte[] byteTimeStamp = System.Text.Encoding.UTF8.GetBytes(timeStamp);
        sha1Salted.TransformBlock(decodedNonce, 0, decodedNonce.Length, decodedNonce, 0);
        sha1Salted.TransformBlock(byteTimeStamp, 0, byteTimeStamp.Length, byteTimeStamp, 0);
        sha1Salted.TransformFinalBlock(hashPasswd, 0, hashPasswd.Length);
        byte[] saltedPasswd = sha1Salted.Hash;
        return Convert.ToBase64String(saltedPasswd);
    }

    private string Send(string header, string body, string AmadeusWSAction)
    {
        try
        {
            ttHttpWebClient oHttpWebClient = new ttHttpWebClient();
            oHttpWebClient.TracerID = TracerID;
            oHttpWebClient.SoapAction = AmadeusWSAction;

            if (!string.IsNullOrEmpty(ttProviderSystems.ProxyURL))
            {
                oHttpWebClient.ServiceURL = ttProviderSystems.ProxyURL;
                ServicePointManager.ServerCertificateValidationCallback = TrustAllCertificatesCallback;
            }
            else
            {
                oHttpWebClient.ServiceURL = ttProviderSystems.URL;
            }

            oHttpWebClient.HttpMethod = "POST";
            oHttpWebClient.Header = header;
            oHttpWebClient.Body = body;

            string log = oHttpWebClient.ComposeMessage();
            DateTime requesttime = DateTime.Now;
            string strResponse = oHttpWebClient.SendHttpRequest(ttProviderSystems);
            DateTime responsetime = DateTime.Now;

            if (ttProviderSystems.AddLog)
                addSoapLog(log + Environment.NewLine + strResponse, requesttime, responsetime, ttProviderSystems.PCC, ttProviderSystems.UserID);

            return strResponse;
        }
        catch (Exception ex)
        {
            string errroText = $"Error Sending Request to AmadeusWS.\r\n{ex.Message}";
            throw new Exception(errroText);
        }
    }

    private string Send(string message, string amadeusWSAction)
    {

        string strResponse = "";
        try
        {
            ttHttpWebClient oHttpWebClient = new ttHttpWebClient();
            oHttpWebClient.TracerID = TracerID;
            oHttpWebClient.SoapAction = amadeusWSAction;

            if (ttProviderSystems.ProxyURL != "")
            {
                oHttpWebClient.ServiceURL = ttProviderSystems.ProxyURL;
                ServicePointManager.ServerCertificateValidationCallback = TrustAllCertificatesCallback;
            }
            else
            {
                oHttpWebClient.ServiceURL = ttProviderSystems.URL;
            }

            oHttpWebClient.HttpMethod = "POST";
            oHttpWebClient.Header = "";
            oHttpWebClient.Body = message;

            string log = oHttpWebClient.ComposeMessage();

            DateTime requesttime = DateTime.Now;
            strResponse = oHttpWebClient.SendHttpRequest(ttProviderSystems);
            DateTime responsetime = DateTime.Now;

            if (ttProviderSystems.AddLog)
                addSoapLog($"{log}{Environment.NewLine}{strResponse}", requesttime, responsetime, ttProviderSystems.PCC, ttProviderSystems.UserID);

            return strResponse;
        }
        catch (Exception ex)
        {
            addLog($"<M>{message}</M><Send/>", ttProviderSystems.PCC);
            throw new Exception(ex.Message);
        }
    }

    private string SendSOAP2(string Message, string AmadeusWSAction)
    {
        string strResponse = "";
        try
        {
            ttHttpWebClient oHttpWebClient = new ttHttpWebClient();
            oHttpWebClient.TracerID = this.TracerID;
            oHttpWebClient.SoapAction = AmadeusWSAction;

            if (ttProviderSystems.ProxyURL != "")
            {
                oHttpWebClient.ServiceURL = ttProviderSystems.ProxyURL;
                ServicePointManager.ServerCertificateValidationCallback = TrustAllCertificatesCallback;
            }
            else
            {
                oHttpWebClient.ServiceURL = ttProviderSystems.URL;
            }

            oHttpWebClient.HttpMethod = "POST";
            oHttpWebClient.Header = "<Session><SessionId></SessionId><SequenceNumber></SequenceNumber><SecurityToken></SecurityToken></Session>";
            oHttpWebClient.Body = Message;

            string log = oHttpWebClient.ComposeMessage();

            DateTime requesttime = DateTime.Now;
            strResponse = oHttpWebClient.SendHttpRequest(ttProviderSystems);
            DateTime responsetime = DateTime.Now;
            if (ttProviderSystems.AddLog)
                addSoapLog($"{log}{Environment.NewLine}{strResponse}", requesttime, responsetime, ttProviderSystems.PCC, ttProviderSystems.UserID);

            return strResponse;
        }
        catch (Exception ex)
        {
            addLog($"<M>{ex.Message}</M><SendSoap2/>", ttProviderSystems.PCC);
            throw new Exception(ex.Message);
        }
    }

    private string SendSoap4(string header, string body, string AmadeusWSAction)
    {
        try
        {
            ttHttpWebClient oHttpWebClient = new ttHttpWebClient();
            string action = AmadeusWSAction.Substring(AmadeusWSAction.LastIndexOf('/') + 1, AmadeusWSAction.Length - (AmadeusWSAction.LastIndexOf('/') + 1));
            oHttpWebClient.SoapAction = $"http://webservices.amadeus.com/{action}";
            oHttpWebClient.ServiceURL = $"{ttProviderSystems.SOAP4URL}/{ttProviderSystems.Profile}";
            oHttpWebClient.HttpMethod = "POST";
            oHttpWebClient.Header = header;
            oHttpWebClient.Body = body;

            string log = oHttpWebClient.ComposeMessageSOAP4(AmadeusWSAction.Substring(AmadeusWSAction.LastIndexOf('/') + 1, (AmadeusWSAction.Length - (AmadeusWSAction.LastIndexOf('/') + 1))));
            DateTime requesttime = DateTime.Now;
            string strResponse = oHttpWebClient.SendHttpRequestSoap4(ttProviderSystems, log);
            DateTime responsetime = DateTime.Now;

            if (ttProviderSystems.AddLog)
                addSoapLog($"{log}{Environment.NewLine}{strResponse}", requesttime, responsetime, ttProviderSystems.PCC, ttProviderSystems.UserID);

            return strResponse;
        }
        catch (Exception ex)
        {
            string errroText = $"Error Sending Request to AmadeusWS.\r\n{ex.Message}";
            throw new Exception(errroText);
        }
    }

    public static bool TrustAllCertificatesCallback(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors)
    {
        return true;
    }

    private string GetResponseFromSoap(string strResponse, string AmadeusWSService, enRequestType RequestType)
    {
        try
        {
            var oDoc = new XmlDocument();
            oDoc.LoadXml(strResponse);
            XmlElement oRoot = oDoc.DocumentElement;
            //  Get The Body
            XmlNode oNode = oRoot.LastChild;
            // ********************************
            //  Check for Errors in Response  *
            // ********************************
            if ((oNode.FirstChild.Name == "soap-env:Fault") || (oNode.FirstChild.Name == "soap:Fault"))
            {
                oNode = oNode.FirstChild;
                oNode = oNode.SelectSingleNode("faultstring");
                throw new Exception(oNode.InnerXml.Replace(" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"", "").Replace("soap:", ""));
            }
            else if (strResponse == "")
            {
                throw new Exception("No response from Amadeus");
            }
            // *************************
            //  Get Response From Soap *
            // *************************
            if (RequestType == enRequestType.CreateSession)
            {
                //  Get The Header

                if (strResponse.IndexOf("<statusCode>P</statusCode>") == -1)
                {
                    string exError = "Cannot open session with Amadeus\n\rError opening session";

                    if (strResponse.Contains("<freeText>"))
                        exError += $"\n\r{strResponse.Substring(strResponse.IndexOf("<freeText>") + 10, strResponse.IndexOf("</freeText>") - strResponse.IndexOf("<freeText>") - 10)}";

                    throw new Exception(exError);
                }

                oNode = oRoot.FirstChild;
                if (isSOAP2)
                {
                    strResponse = $"{oNode.FirstChild.ChildNodes[2].InnerText}|{oNode.FirstChild.ChildNodes[0].InnerText}|{oNode.FirstChild.ChildNodes[1].InnerText}";
                }
                else
                {
                    oNode = oNode.LastChild;
                    strResponse = oNode.InnerText;
                }
            }
            else
            {
                //  Get The Body
                oNode = oRoot.LastChild;
                strResponse = oNode.InnerXml;
                strResponse = strResponse.Replace(" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"", "").Replace("soap:", "");

                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Soap body response", strResponse, ttProviderSystems.LogUUID);
            }

            return strResponse;
        }
        catch (Exception ex)
        {
            addLog($"<M>{strResponse}</M><GetResponseFromSoap/>", ttProviderSystems.PCC);
            return $"<Errors><Error>{ex.Message}</Error></Errors>";
        }
    }

    private string GetResponseFromSoap2(string strResponse, string AmadeusWSService, enRequestType RequestType)
    {
        try
        {
            var oDoc = new XmlDocument();
            oDoc.LoadXml(strResponse);
            XmlElement oRoot = oDoc.DocumentElement;
            //  Get The Body
            XmlNode oNode = oRoot.LastChild;
            // ********************************
            //  Check for Errors in Response  *
            // ********************************
            if ((oNode.FirstChild.Name == "soap-env:Fault") || (oNode.FirstChild.Name == "soap:Fault"))
            {
                oNode = oNode.FirstChild;
                oNode = oNode.SelectSingleNode("faultstring");
                throw new Exception(oNode.InnerXml.Replace(" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"", "").Replace("soap:", ""));
            }
            // *************************
            //  Get Response From Soap *
            // *************************
            if (RequestType == enRequestType.CreateSession)
            {
                //  Get The Header

                oNode = oRoot.FirstChild;
                if (isSOAP2)
                {
                    strResponse = oNode.SelectSingleNode("SecurityToken").InnerText + "|" + oNode.SelectSingleNode("SessionId").InnerText + "|" + oNode.SelectSingleNode("SequenceNumber").InnerText;
                }
                else
                {
                    oNode = oNode.LastChild;
                    strResponse = oNode.InnerText;
                }
            }
            else
            {
                //  Get The Body
                oNode = oRoot.LastChild;
                strResponse = oNode.InnerXml;
                strResponse = strResponse.Replace(" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"", "").Replace("soap:", "");

                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Soap body response", strResponse, ttProviderSystems.LogUUID);
            }

            return strResponse;
        }
        catch (Exception ex)
        {
            return $"<Errors><Error>{ex.Message}</Error></Errors>";
        }
    }

    private string GetSessionFromSoap(string strResponse)
    {
        try
        {
            var oDoc = new XmlDocument();
            oDoc.LoadXml(strResponse);
            XmlElement oRoot = oDoc.DocumentElement;
            //  Get The Body
            XmlNode oNode = oRoot.FirstChild;
            // ********************************
            //  Check for Errors in Response  *
            // ********************************
            string strsession = string.Empty;
            if (oNode.Name == "soap:Header")
            {
                strsession = isSOAP2
                    ? $"{oNode.FirstChild.ChildNodes[2].InnerText}|{oNode.FirstChild.ChildNodes[0].InnerText}|{oNode.FirstChild.ChildNodes[1].InnerText}"
                    : oNode.FirstChild.InnerText;
            }

            return strsession;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void GetSessionFromSoap4(string strResponse, ref Soap4Session session)
    {
        try
        {
            var oDoc = new XmlDocument();
            oDoc.LoadXml(strResponse);
            XmlElement oRoot = oDoc.DocumentElement;

            //Get The Body
            XmlNode oNode = oRoot.FirstChild;

            // ********************************
            //  Check for Errors in Response  *
            // ********************************
            if (oNode.Name == "soapenv:Header")
            {
                XmlNode oSession = oNode.LastChild;
                if (oSession.Name == "awsse:Session")
                {
                    session.SessionId = oSession.ChildNodes[0].InnerText;
                    session.SequenceNumber = oSession.ChildNodes[1].InnerText;
                    session.SecurityToken = oSession.ChildNodes[2].InnerText;
                    session.StatusCode = (TransactionStatusCode)Enum.Parse(typeof(TransactionStatusCode), oSession.Attributes["TransactionStatusCode"].Value, true);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //New Session Pooling Method
    public string CheckSessionV3()
    {
        try
        {
            string sessionID = "";
            var oDa = new cDA("ConnectionString");

            //Check in tblSessionPool for available Sessions 
            if (oDa.CheckAvailableSessions(ttProviderSystems.PCC, ttProviderSystems.System, ttProviderSystems.UserID))
            {
                sessionID = oDa.SessionUpdate(ttProviderSystems.PCC, ttProviderSystems.System, ttProviderSystems.UserID, isSOAP2);
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Updated Session", sessionID, ttProviderSystems.LogUUID);
            }

            // Rastko March 5, 2011 rewriting the else routine to make more simple
            // the idea is to not have additional blocks any more
            // we now have the initial block and then only one session created at a time above the block (when needed)
            else
            {
                //Check PCC has exceeded the Session Limit
                if (sessionCount < maximumCount)
                {
                    isInitialBlock = 'N';
                    //blockIDNum = 2;


                    //Create one Session in the main Thread
                    modCore.IsCreating = true;
                    sessionID = CreateSessionV3();

                    if (sessionID.IndexOf("<Error") != -1 || sessionID == "")
                    {
                        modCore.IsCreating = false;
                        return sessionID;
                    }

                    newSessions++;
                    modCore.IsCreating = false;
                }

                oDa.UpdatePCCSessions(ttProviderSystems.PCC, newSessions, ttProviderSystems.UserID);
            }
            return sessionID;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string CheckSessionV2()
    {
        try
        {
            string sessionID = "";
            var oDa = new cDA("ConnectionString");

            //Check in tblSessionPool for available Sessions 
            if (oDa.CheckAvailableSessions(ttProviderSystems.PCC, ttProviderSystems.System, ttProviderSystems.UserID))
            {
                sessionID = oDa.SessionUpdate(ttProviderSystems.PCC, ttProviderSystems.System, ttProviderSystems.UserID, isSOAP2);
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Updated Session", sessionID, ttProviderSystems.LogUUID);
            }
            else
            {
                //Check PCC has exceeded the Session Limit
                if (sessionCount < maximumCount)
                {
                    isInitialBlock = 'N';
                    //blockIDNum = 2;

                    //Create one Session in the main Thread
                    modCore.IsCreating = true;
                    sessionID = CreateSessionV3();
                    sessionID = oDa.SessionUpdate(ttProviderSystems.PCC, ttProviderSystems.System, ttProviderSystems.UserID, isSOAP2);

                    modCore.IsCreating = false;
                }
            }
            return sessionID;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string CreateSession()
    {

        string sessionID = "";
        string body = "";
        CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Create Session", "", ttProviderSystems.LogUUID);
        try
        {
            if (isSOAP4)
            {
                /*
                 * return string.Empty;
                 */
                body = "<Command_Cryptic><messageAction><messageFunctionDetails><messageFunction>M</messageFunction></messageFunctionDetails></messageAction><longTextString><textStringDetails>DDNYC</textStringDetails></longTextString></Command_Cryptic>";
                Soap4Session session = new Soap4Session(TransactionStatusCode.Start);
                var resp = SendMessagesoap4(body, "", $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/{ttProviderSystems.AmadeusWSSchema[Command_Cryptic]}", ref session);
                session.StatusCode = TransactionStatusCode.InSeries;
                return string.IsNullOrEmpty(session.SecurityToken) ? session.SecurityToken : session.ToString();
            }
            else
            {
                var length = Convert.ToInt32(ttProviderSystems.Password.Substring(0, 2)).ToString("#0");
                var password = ttProviderSystems.Password.Substring(2);

                body = $"<Security_Authenticate><userIdentifier><originIdentification><sourceOffice>{ttProviderSystems.PCC}</sourceOffice></originIdentification>" +
                $"<originatorTypeCode>U</originatorTypeCode><originator>{ttProviderSystems.UserName}</originator></userIdentifier><dutyCode><dutyCodeDetails><referenceQualifier>DUT</referenceQualifier><referenceIdentifier>SU</referenceIdentifier></dutyCodeDetails></dutyCode>" +
                $"<systemDetails><organizationDetails><organizationId>{ttProviderSystems.Origin}</organizationId></organizationDetails></systemDetails>" +
                $"<passwordInfo><dataLength>{length}</dataLength><dataType>E</dataType><binaryData>{password}</binaryData></passwordInfo></Security_Authenticate>";

                DateTime requesttime = DateTime.Now;

                sessionID = isSOAP2
                    ? SendSOAP2(body, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/VLSSLQ_06_1_1A")
                    : Send(body, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/VLSSLQ_06_1_1A");

                DateTime responsetime = DateTime.Now;

                // ****************************************
                //  Get SecurityToken From AmadeusWS Response *
                // ****************************************            
                sessionID = GetResponseFromSoap(sessionID, "SessionCreate", enRequestType.CreateSession);
            }

            return sessionID;
        }
        catch (Exception ex)
        {
            addLog($"<M>{body}</M><M>{ex.Message}</M><CreateSession/>", ttProviderSystems.PCC);

            if (ttProviderSystems.AddLog)
                addLog($"<EXCOS/>{sessionID} {body}", ttProviderSystems.PCC);

            sessionID = isSOAP2
                    ? SendSOAP2(body, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/VLSSLQ_06_1_1A")
                    : Send(body, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/VLSSLQ_06_1_1A");

            if (ttProviderSystems.AddLog)
                addLog($"<EXOK/>{sessionID}", ttProviderSystems.PCC);

            sessionID = GetResponseFromSoap(sessionID, "SessionCreate", enRequestType.CreateSession);

            return sessionID;
        }
    }

    public string CreateSessionSOAP2()
    {
        CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Create Session SOAP2", "", ttProviderSystems.LogUUID);
        try
        {
            string length = ttProviderSystems.Password.Substring(0, 2);
            string password = ttProviderSystems.Password.Substring(2);

            string body = $"<Security_Authenticate><userIdentifier><originIdentification><sourceOffice>{ttProviderSystems.PCC}</sourceOffice></originIdentification>" +
            $"<originatorTypeCode>U</originatorTypeCode><originator>{ ttProviderSystems.UserName}</originator></userIdentifier><dutyCode><dutyCodeDetails><referenceQualifier>DUT</referenceQualifier><referenceIdentifier>SU</referenceIdentifier></dutyCodeDetails></dutyCode>" +
            $"<systemDetails><organizationDetails><organizationId>{ ttProviderSystems.Origin}</organizationId></organizationDetails></systemDetails>" +
            $"<passwordInfo><dataLength>{length}</dataLength><dataType>E</dataType><binaryData>{password}</binaryData></passwordInfo></Security_Authenticate>";

            DateTime requesttime = DateTime.Now;
            string sessionID = Send(body, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/VLSSLQ_06_1_1A");
            DateTime responsetime = DateTime.Now;

            // ****************************************
            //  Get SecurityToken From AmadeusWS Response *
            // ****************************************
            if (ttProviderSystems.AddLog)
                addSoapLog(sessionID, requesttime, responsetime, ttProviderSystems.PCC, ttProviderSystems.UserID);

            sessionID = GetResponseFromSoap(sessionID, "SessionCreate", enRequestType.CreateSession);

            return sessionID;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void CreateSessionV2()
    {
        modCore.IsCreating = true;        
        CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Create Session", "", ttProviderSystems.LogUUID);
        try
        {
            DateTime CreatedTime = DateTime.Now;
            string length = ttProviderSystems.Password.Substring(0, 2);
            string password = ttProviderSystems.Password.Substring(2);
            string body = $"<Security_Authenticate><userIdentifier><originIdentification><sourceOffice>{ttProviderSystems.PCC}</sourceOffice></originIdentification>" +
            $"<originatorTypeCode>U</originatorTypeCode><originator>{ttProviderSystems.UserName}</originator></userIdentifier><dutyCode><dutyCodeDetails><referenceQualifier>DUT</referenceQualifier><referenceIdentifier>SU</referenceIdentifier></dutyCodeDetails></dutyCode>" +
            $"<systemDetails><organizationDetails><organizationId>{ttProviderSystems.Origin}</organizationId></organizationDetails></systemDetails>" +
            $"<passwordInfo><dataLength>{length}</dataLength><dataType>E</dataType><binaryData>{password}</binaryData></passwordInfo></Security_Authenticate>";

            DateTime requesttime = DateTime.Now;

            newSessionID = isSOAP2
                ? SendSOAP2(body, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/VLSSLQ_06_1_1A")
                : Send(body, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/VLSSLQ_06_1_1A");


            DateTime responsetime = DateTime.Now;
            // ****************************************
            //  Get SecurityToken From AmadeusWS Response *
            // ****************************************
            if (ttProviderSystems.AddLog)
                addSoapLog(newSessionID, requesttime, responsetime, ttProviderSystems.PCC, ttProviderSystems.UserID);

            newSessionID = GetResponseFromSoap(newSessionID, "SessionCreate", enRequestType.CreateSession);

            if (!newSessionID.Contains("<Error"))
            {
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", newSessionID, "", ttProviderSystems.LogUUID);
                var oDa = new cDA("ConnectionString");
                oDa.InsertNewSession(newSessionID, 1, ttProviderSystems.Provider, CreatedTime, DateTime.Now, ttProviderSystems.GReqID, ttProviderSystems.UserID, "Active", 'N', 'N', ttProviderSystems.URL, "B2", isInitialBlock, ttProviderSystems.PCC, ttProviderSystems.Profile, ttProviderSystems.System, ttProviderSystems.GPass);
                oDa.Dispose();
            }
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

    public string CreateSessionV3()
    {
        modCore.IsCreating = true;
        CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Create Session", "", ttProviderSystems.LogUUID);
        try
        {
            DateTime CreatedTime = DateTime.Now;            
            string length = ttProviderSystems.Password.Substring(0, 2);
            string password = ttProviderSystems.Password.Substring(2);
            string body = $"<Security_Authenticate><userIdentifier><originIdentification><sourceOffice>{ttProviderSystems.PCC}</sourceOffice></originIdentification>" + 
            $"<originatorTypeCode>U</originatorTypeCode><originator>{ttProviderSystems.UserName}</originator></userIdentifier><dutyCode><dutyCodeDetails><referenceQualifier>DUT</referenceQualifier><referenceIdentifier>SU</referenceIdentifier></dutyCodeDetails></dutyCode>" +
            $"<systemDetails><organizationDetails><organizationId>{ ttProviderSystems.Origin}</organizationId></organizationDetails></systemDetails>" +
            $"<passwordInfo><dataLength>{length}</dataLength><dataType>E</dataType><binaryData>{password}</binaryData></passwordInfo></Security_Authenticate>";

            DateTime reqesttime = DateTime.Now;

            newSessionID = isSOAP2 
                ? SendSOAP2(body, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/VLSSLQ_06_1_1A")
                : Send(body, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/VLSSLQ_06_1_1A");
            
            DateTime responsetime = DateTime.Now;

            // ****************************************
            //  Get SecurityToken From AmadeusWS Response *
            // ****************************************
            if (ttProviderSystems.AddLog)
                addSoapLog(newSessionID, reqesttime, responsetime, ttProviderSystems.PCC, ttProviderSystems.UserID);

            newSessionID = GetResponseFromSoap(newSessionID, "SessionCreate", enRequestType.CreateSession);

            if (newSessionID.StartsWith("<Error"))
            {
                modCore.IsCreating = false;
                return newSessionID;
            }

            CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", newSessionID, "", ttProviderSystems.LogUUID);
            var oDa = new cDA("ConnectionString");
            oDa.InsertNewSession(newSessionID.Substring(0, newSessionID.Length - 1) + "2", 2, ttProviderSystems.Provider, CreatedTime, DateTime.Now, ttProviderSystems.GReqID, ttProviderSystems.UserID, "Active", 'N', 'N', ttProviderSystems.URL, "B2", isInitialBlock, ttProviderSystems.PCC, ttProviderSystems.Profile, ttProviderSystems.System, ttProviderSystems.GPass);
            oDa.Dispose();
            modCore.IsCreating = false;
            string SessionID = newSessionID;
            return SessionID;
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

    public string CloseSession(string sessionID)
    {
        string response;
        CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Close Session", "", ttProviderSystems.LogUUID);

        if (sessionID.Contains("Error"))
            return sessionID;

        try
        {
            var sessionElems = sessionID.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (sessionElems.Count.Equals(4))
            {
                response = CloseSoap4Session(sessionID);
            }
            else
            {
                string header = ComposeHeader("AmadeusWSXML", "Session", "SessionCloseRQ", sessionID);
                string body = "<Security_SignOut/>";
                response = Send(header, body, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/VLSSOQ_04_1_1A");
            }
            return response;
        }
        catch (Exception ex)
        {
            addLog($"<M>{sessionID}</M><CloseSession/>", ttProviderSystems.PCC);

            if (ttProviderSystems.AddLog)
                addLog($"<EXCCS/>{sessionID}", ttProviderSystems.PCC);

            throw ex;
        }
    }

    public string CloseSoap4Session(ref Soap4Session session)
    {
        CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Close Session", "", ttProviderSystems.LogUUID);

        try
        {
            string header = ComposeSoap4Header("VLSSOQ_04_1_1A", ref session);
            string body = "<Security_SignOut/>";
            string strResponse = SendSoap4(header, body, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/VLSSOQ_04_1_1A");
            return strResponse;
        }
        catch (Exception ex)
        {
            addLog($"<M>{session.SessionId}</M><CloseSession/>", ttProviderSystems.UserID);

            if (ttProviderSystems.AddLog)
                addLog($"<EXCCS/>{session.SessionId}", ttProviderSystems.UserID);

            throw ex;
        }
    }

    public string CloseSoap4Session(string sessionString)
    {
        var session4 = new Soap4Session(sessionString, TransactionStatusCode.End);
        return CloseSoap4Session(ref session4);
    }

    /// <summary>
    /// New close methods for session pooling - Shashin - 23-2-2010
    /// </summary>
    /// <param name="SessionID">Session ID assigned by GDS</param>
    /// <returns></returns>
    public string CloseSessionFromPool(string SessionID)
    {
        try
        {
            cDA oDB = new cDA("ConnectionString");
            bool blTest = oDB.CheckSession(SessionID, isSOAP2);
            oDB.Dispose();

            if (!blTest)
            {
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Close Session", "", ttProviderSystems.LogUUID);
                try
                {
                    var header = ComposeHeader("AmadeusWSXML", "Session", "SessionCloseRQ", SessionID);
                    var body = "<Security_SignOut/>";
                    var response = Send(header, body, $"http://webservices.amadeus.com/{ttProviderSystems.Profile}/VLSSOQ_04_1_1A");
                    return response;
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
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string SendMessage(string message, string AmadeusWSService, string AmadeusWSAction, string sessionID)
    {
        string response = "";
        string soapResponse = "";
        bool CloseThisSession = false;

        Soap4Session session = string.IsNullOrEmpty(sessionID)
            ? new Soap4Session(TransactionStatusCode.None)
            : new Soap4Session(sessionID, TransactionStatusCode.InSeries);
        try
        {
            CloseThisSession = string.IsNullOrEmpty(sessionID) && !isSOAP4;
            if (string.IsNullOrEmpty(sessionID) && !isSOAP4)
            {
                string soapAction = "";
                soapAction = AmadeusWSAction;
                sessionID = CreateSession();
                AmadeusWSAction = soapAction;
            }

            if (sessionID.StartsWith("<Error"))
            {
                return sessionID;
            }

            string Header = isSOAP4 ? ComposeSoap4Header(AmadeusWSAction, ref session) : ComposeHeader("AmadeusWSXML", AmadeusWSService, AmadeusWSAction, sessionID);
            string Body = message;

            soapResponse = isSOAP4 ? SendSoap4(Header, Body, AmadeusWSAction) : Send(Header, Body, AmadeusWSAction);

            // *************************
            //  Get Response From Soap *
            // *************************

            if (soapResponse.StartsWith("<Error"))
            {
                response = soapResponse;
            }
            else
            {
                soapResponse = soapResponse.Replace("", "");
                response = GetResponseFromSoap(soapResponse, "SendMessage", enRequestType.Message);
            }

            //if (ttProviderSystems.AddLog)
            //    addLog(strResponse, ttProviderSystems.UserID);

            if (CloseThisSession)
            {
                if (!response.StartsWith("<Error"))
                    sessionID = GetSessionFromSoap(soapResponse);

                if (sessionID != null && sessionID != "")
                    CloseSession(sessionID);//CloseSessionFromPool(SessionID);
            }

            if (isSOAP4)
            {
                this.GetSessionFromSoap4(soapResponse, ref session);
                session.IncreaseSequenceNo();
            }

            return response;
        }
        catch (Exception ex)
        {
            addLog($"<M>{message}</M><SendMessage/>", ttProviderSystems.PCC);
            if (ttProviderSystems.AddLog)
                addLog($"<EXCSM/>{response} {soapResponse}", ttProviderSystems.PCC);

            if (CloseThisSession)
            {
                if (soapResponse != "")
                    sessionID = GetSessionFromSoap(soapResponse);
                else
                {
                    string[] sessionid = sessionID.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    int intSession = ttProviderSystems.SOAP2
                        ? Convert.ToInt32(sessionid[2])
                        : Convert.ToInt32(sessionid[1]);

                    intSession -= 1;
                    sessionID = ttProviderSystems.SOAP2
                        ? $"{sessionid[0]}|{sessionid[1]}|{intSession}"
                        : $"{sessionid[0]}|{intSession}";                    
                }

                if (sessionID != null)
                    CloseSession(sessionID);//CloseSessionFromPool(SessionID);
            }
            throw ex;
        }
    }

    public string SendMessagesoap4(string message, string AmadeusWSService, string AmadeusWSAction, ref Soap4Session session)
    {
        string response = string.Empty;
        string soapResponse = string.Empty;
        try
        {
            string header = ComposeSoap4Header(AmadeusWSAction, ref session);
            soapResponse = SendSoap4(header, message, AmadeusWSAction);

            // *************************
            //  Get Response From Soap *
            // *************************
            response = soapResponse.StartsWith("<Error") ? soapResponse : GetResponseFromSoap(soapResponse, "SendMessage", enRequestType.Message);

            if (!soapResponse.StartsWith("<Error"))
                soapResponse = soapResponse.Replace("", "");

            GetSessionFromSoap4(soapResponse, ref session);

            return response;
        }
        catch (Exception ex)
        {
            addLog($"<M>{message}</M><SendMessage/>", ttProviderSystems.UserID);
            if (ttProviderSystems.AddLog)
                addLog($"<EXCSM/>{response} {soapResponse}", ttProviderSystems.UserID);
            throw ex;
        }
        finally
        {
            session.IncreaseSequenceNo();
        }
    }

    public string SendMessageV3(string message, string AmadeusWSService, string AmadeusWSAction, string sessionID)
    {
        string soapResponse = "";
        bool CloseThisSession = false;

        try
        {
            
            if (sessionID.Length == 0)
            {
                CloseThisSession = true;
                string soapAction = "";
                soapAction = AmadeusWSAction;
                sessionID = CheckSessionV3();
                AmadeusWSAction = soapAction;
            }
            else
            {
                CloseThisSession = false;
            }

            if (sessionID == "")
            {
                return "<Errors><Error>Cannot create session with Amadeus</Error></Errors>";
            }

            var header = ComposeHeader("AmadeusWSXML", AmadeusWSService, AmadeusWSAction, sessionID);
            soapResponse = Send(header, message, AmadeusWSAction);
            // *************************
            //  Get Response From Soap *
            // *************************
            string response = GetResponseFromSoap(soapResponse, "SendMessage", enRequestType.Message);

            if (response.IndexOf("Inactive conversation") != -1 || response.IndexOf("Bad SecurityToken") != -1)
            {
                var oDB = new cDA("ConnectionString");

                DataTable dtSession;
                dtSession = oDB.ToBeDeleted(sessionID);
                newSessions--;
                oDB.UpdatePCCSessions(ttProviderSystems.PCC, newSessions, ttProviderSystems.UserID);

                isInitialBlock = System.Convert.ToChar("N");
                //blockIDNum = 2;

                modCore.IsCreating = true;
                sessionID = CreateSessionV3();
                newSessions++;
                modCore.IsCreating = false;

                oDB.UpdatePCCSessions(ttProviderSystems.PCC, newSessions, ttProviderSystems.UserID);
                oDB.Dispose();

                header = ComposeHeader("AmadeusWSXML", AmadeusWSService, AmadeusWSAction, sessionID);
                soapResponse = Send(header, message, AmadeusWSAction);
                response = GetResponseFromSoap(soapResponse, "SendMessage", enRequestType.Message);
            }

            if (CloseThisSession)
            {
                sessionID = GetSessionFromSoap(soapResponse);

                if (sessionID != null && sessionID != "")
                    CloseSessionFromPool(sessionID);// CloseSession(SessionID);
            }

            return response;
        }
        catch (Exception ex)
        {
            addLog($"<M>{message}</M><SendMessage3/>", ttProviderSystems.PCC);

            if (CloseThisSession)
            {
                if (soapResponse != "")
                    sessionID = GetSessionFromSoap(soapResponse);
                else
                {
                    string[] sessionid = sessionID.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    int intSession = ttProviderSystems.SOAP2
                        ? Convert.ToInt32(sessionid[2])
                        : Convert.ToInt32(sessionid[1]);

                    intSession -= 1;
                    sessionID = ttProviderSystems.SOAP2
                        ? $"{sessionid[0]}|{sessionid[1]}|{intSession}"
                        : $"{sessionid[0]}|{intSession}";
                }

                if (sessionID != null)
                    CloseSessionFromPool(sessionID);//CloseSession(SessionID);
            }
            throw ex;
        }
    }

    private void addSoapLog(string msg, DateTime starttime, DateTime endtime, string username, string userid)
    {
        try
        {
            TripXMLTools.TripXMLLog.LogSoapMessage(msg, starttime, endtime, username, TracerID);
        }
        catch (Exception)
        {
        }
    }

    private void addLog(string msg, string username)
    {
        try
        {
            TripXMLTools.TripXMLLog.LogErrorMessage(msg, username, TracerID);
        }
        catch (Exception)
        {
        }
    }

    private enum enRequestType
    {
        CreateSession = 1,
        CloseSession = 2,
        Message = 3,
    }
}

public enum TransactionStatusCode { None, Start, InSeries, End };

public class Soap4Session
{
    public string SessionId
    {
        get;
        set;
    }

    public string SequenceNumber
    {
        get;
        set;
    }

    public string SecurityToken
    {
        get;
        set;
    }

    public TransactionStatusCode StatusCode
    {
        get;
        set;
    }

    public Soap4Session()
    {
        SessionId = string.Empty;
        SequenceNumber = string.Empty;
        SecurityToken = string.Empty;
    }

    public Soap4Session(string sessionId, string sequenceNumber, string securityToken)
    {
        SessionId = sessionId;
        SequenceNumber = sequenceNumber;
        SecurityToken = securityToken;
    }

    public Soap4Session(string sessionId, string sequenceNumber, string securityToken, TransactionStatusCode statuscode)
    {
        SessionId = sessionId;
        SequenceNumber = sequenceNumber;
        SecurityToken = securityToken;
        StatusCode = statuscode;
    }

    public Soap4Session(TransactionStatusCode statuscode)
    {
        StatusCode = statuscode;
        SessionId = string.Empty;
        SequenceNumber = string.Empty;
        SecurityToken = string.Empty;
    }

    public Soap4Session(string sessionString)
    {
        SessionId = string.Empty;
        SequenceNumber = string.Empty;
        SecurityToken = string.Empty;

        var elems = sessionString.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();

        if (elems.Count.Equals(4))
        {
            TransactionStatusCode status = (TransactionStatusCode)Enum.Parse(typeof(TransactionStatusCode), elems[0], true);
            SessionId = elems[2];
            SequenceNumber = elems[3];
            SecurityToken = elems[1];
            StatusCode = status;
        }
    }

    public Soap4Session(string sessionString, TransactionStatusCode statuscode)
    {
        SessionId = string.Empty;
        SequenceNumber = string.Empty;
        SecurityToken = string.Empty;

        var elems = sessionString.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();

        if (elems.Count.Equals(4))
        {
            SessionId = elems[2];
            SequenceNumber = elems[3];
            SecurityToken = elems[1];
            StatusCode = statuscode;
        }
    }

    public void IncreaseSequenceNo()
    {
        int sequence;
        int.TryParse(SequenceNumber, out sequence);
        sequence++;
        SequenceNumber = sequence.ToString();
    }

    public void DecreaseSequenceNo()
    {
        int sequence;
        int.TryParse(SequenceNumber, out sequence);
        sequence--;
        SequenceNumber = sequence.ToString();
    }

    public string ToString(bool incroment = false)
    {
        if (incroment)
            IncreaseSequenceNo();

        return $"{StatusCode}|{SecurityToken}|{SessionId}|{SequenceNumber}";
    }
}
