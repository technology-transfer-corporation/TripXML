using System;
using System.IO;
using System.IO.Compression;
using System.Net.Mail;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Xml;
using System.Threading;
using System.Collections.Concurrent;
using WebSocketSharp;
using WebSocket = WebSocketSharp.WebSocket;
using WebSocketState = WebSocketSharp.WebSocketState;
using System.Runtime.Remoting.Messaging;

namespace TripXMLMain
{
    public class CoreLib
    {
        private static BlockingCollection<string> _senderQueue = new BlockingCollection<string>();
        private static Thread _traceSend;

        #region  Transform XML with XSLs 

        public static string TransformXML(string inputXml, string xslPath, string xslName, bool fromFile = false)
        {
            System.Xml.Xsl.XslCompiledTransform xslt;
            XmlDocument oDoc;
            StringWriter oWriter = null;
            System.Xml.Xsl.XsltSettings settings = null;

            try
            {
                oDoc = new XmlDocument();
                if (fromFile)
                {
                    oDoc.Load(inputXml);
                }
                else
                {
                    oDoc.LoadXml(inputXml);
                }

                oWriter = new StringWriter();
                xslt = new System.Xml.Xsl.XslCompiledTransform();
                settings = new System.Xml.Xsl.XsltSettings(true, true);

                string xxslt = xslName.Replace(".xsl", "");
                xslt.Load(System.Reflection.Assembly.Load(xxslt).GetType(xxslt));

                /*
                #if xslTInline == true
                                string xxslt = xslName.Replace(".xsl", "");
                                xslt.Load(System.Reflection.Assembly.Load(xxslt).GetType(xxslt));

                #else
                                if (!xslPath.EndsWith("\\"))
                                    xslPath += "\\";                

                                xslt.Load($"{xslPath}{xslName}", settings, new XmlUrlResolver());
                #endif
                */

                xslt.Transform(oDoc.DocumentElement.ParentNode, null, oWriter);
                return oWriter.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (oWriter is object)
                {
                    oWriter.Close();
                }
            }
        }

        #endregion

        #region  Send Trace 

        public static void SendTrace(string userID, string strFile, string strText, string strItem, string strUUID)
        {
            if (_traceSend == null)
            {
                _traceSend = new Thread(TraceSender);
                _traceSend.Start();
            }

            var sb = new StringBuilder();
            try
            {
                strItem = strItem.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                strItem = strItem.Replace("<?xml version='1.0' encoding='utf-8'?>", "");
                strItem = strItem.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>", "");
                strItem = strItem.Replace("<?xml version=\"1.0\"  encoding=\"ISO-8859-1\" standalone=\"yes\" ?>", "");
                strItem = strItem.Replace("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"  standalone=\"yes\"?>", "");
                strItem = strItem.Replace("<?xml version=\"1.0\"   encoding=\"ISO-8859-1\"  standalone=\"yes\" ?>", "");
                strItem = strItem.Replace("xmlns = \"\"", "");

                byte[] sendBytes;
                if (userID is object)
                {
                }
                else
                {
                    userID = "";
                }

                sb.Append("<").Append(strFile).Append("><Text>").Append(strText).Append("</Text><UUID>").Append(strUUID).Append("</UUID><Item>").Append(strItem).Append("</Item><UserID>").Append(userID).Append("</UserID></").Append(strFile).Append(">");
                if (_senderQueue.Count < 50)
                    _senderQueue.Add(sb.ToString());
            }
            catch (Exception ex)
            {
            }
        }

        private static void TraceSender()
        {
            WebSocket wsClient;
            wsClient = new WebSocket($"ws://localhost:3070/Trace");
            while (true)
            {
                var reconnect = false;
                string msg = string.Empty;
                try
                {
                    msg = _senderQueue.Take();
                    if (wsClient.ReadyState != WebSocketState.Open) wsClient.Connect();
                    if (wsClient.ReadyState == WebSocketState.Connecting)
                        Thread.Sleep(1000);
                    wsClient.Send(msg);
                }
                catch
                {
                    reconnect = true;
                }
                if (reconnect)
                    try
                    {
                        wsClient.Close();
                        if (wsClient.ReadyState != WebSocketState.Open) wsClient.Connect();
                        if (wsClient.ReadyState == WebSocketState.Connecting)
                            Thread.Sleep(1000);
                        wsClient.Send(msg);
                    }
                    catch { }
            }
            wsClient.Close();
        }

        static byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                return compressedStream.ToArray();
            }
        }

        #endregion

        #region  Validate XML against the Schema 

        public static bool ValidateXML(string xmlData, int Service, int SchemaType, string UserID, string Version)
        {
            string schemaFile;

            schemaFile = modCore.GetSchemaFile((modCore.ttServices)Service, (modCore.enSchemaType)SchemaType, Version);
            return ValidateXML(xmlData, schemaFile, UserID);
        }

        public static bool ValidateXML(string xmlData, string otaMessage, string SchemaFolder, string UserID, string version)
        {
            try
            {
                var sb = new StringBuilder();
                if (!SchemaFolder.EndsWith(@"\"))
                {
                    sb.Append(SchemaFolder).Append(@"\");
                }

                sb.Append(SchemaFolder).Append(modCore.GetSchemaFile(otaMessage, version));
                return ValidateXML(xmlData, sb.ToString(), UserID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool ValidateXML(string xmlData, string SchemaFile, string UserID)
        {
            XmlReaderSettings settings;
            XmlReader vr;
            StringReader stream;
            try
            {
                if (SchemaFile.IndexOf(@"\", StringComparison.Ordinal) == -1)
                    SchemaFile = string.Concat(modCore.SchemaPath, SchemaFile);
                settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, SchemaFile);
                stream = new StringReader(xmlData);
                vr = XmlReader.Create(stream, settings);
            }
            catch (Exception ex)
            {
                // Send to Trace Validation Schema not found.
                SendTrace(UserID, "wsTravelTalk", "XML Validation Error", ex.Message, string.Empty);
                return true;
            }

            try
            {
                while (vr.Read())
                {
                    // Just Read the XML Document
                }
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        #endregion

        #region  Fantome 

        public static string SentToFantome(string UserID, string Provider, ref string Request)
        {
            string requestTag;
            int delay;
            int minDelay;
            int maxDelay;
            string response;
            XmlDocument oDoc;
            XmlElement oRoot;
            XmlNode oNode;
            var sb = new StringBuilder();
            try
            {
                sb.Append("Received at Fantome Adapter").Append(Provider);
                SendTrace(UserID, "CoreLib", sb.ToString(), Request, string.Empty);
                sb.Remove(0, sb.Length);
                oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                oRoot = oDoc.DocumentElement;
                requestTag = oRoot.Name;
                oDoc.Load(@"C:\TravelTalk\Tables\ACTRS.xml");
                oRoot = oDoc.DocumentElement;
                sb.Append("Provider[@Name='").Append(Provider).Append("']");
                oNode = oRoot.SelectSingleNode(sb.ToString());
                sb.Remove(0, sb.Length);
                if (oNode is null)
                {
                    sb.Append("Provider : ").Append(Provider).Append(" not supported by Fantome");
                    throw new Exception(sb.ToString());
                }

                minDelay = Convert.ToInt32(oNode.Attributes["MinDelay"].Value);
                maxDelay = Convert.ToInt32(oNode.Attributes["MaxDelay"].Value);
                response = oNode.SelectSingleNode(requestTag).InnerXml;
                delay = (int)Math.Round(Convert.ToDecimal((maxDelay - minDelay + 1) * new Random().Next() + minDelay));
                sb.Append("Delaying ").Append(delay).Append(" miliseconds");
                SendTrace(UserID, "CoreLib", sb.ToString(), response, string.Empty);
                sb.Remove(0, sb.Length);
                System.Threading.Thread.Sleep(delay);
                sb.Append("Return by Fantome Adapter ").Append(Provider);
                SendTrace(UserID, "CoreLib", sb.ToString(), response, string.Empty);
                sb.Remove(0, sb.Length);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region  GetNodeInnerText 

        public static string GetNodeInnerText(string xmlData, string sNode, bool RetData = true)
        {
            int intStart;
            int intLength;
            var sb = new StringBuilder();
            sb.Append("<").Append(sNode).Append(">");
            if (xmlData.IndexOf(sb.ToString(), StringComparison.Ordinal) == -1)
            {
                if (RetData)
                {
                    return xmlData;
                }
                else
                {
                    return "";
                }
            }

            intStart = xmlData.IndexOf(sb.ToString(), StringComparison.Ordinal) + sNode.Length + 2;
            sb.Remove(0, sb.Length);
            sb.Append("</").Append(sNode).Append(">");
            intLength = xmlData.IndexOf(sb.ToString(), StringComparison.Ordinal) - intStart;
            return xmlData.Substring(intStart, intLength).Replace("\r", "").Replace("\n", "").Trim();
        }

        #endregion

        #region  Mask Credit Card, Passport Number 
        public static void MaskPrivateData(ref string Message, string[] Attributes)
        {
            int index;
            int length;
            int i;
            var sb = new StringBuilder();
            var loopTo = Attributes.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                index = Message.IndexOf(Attributes[i], StringComparison.Ordinal);
                while (index > -1)
                {
                    index = index + Attributes[i].Length + 2;
                    length = Message.IndexOf('"', index + 1);
                    if (string.IsNullOrEmpty(Message) | index < 1 | length < 0)
                    {
                        index = -1;
                    }
                    else
                    {
                        sb.Append(Message.Substring(0, index)).Append("****************").Append(Message.Substring(length));
                        Message = sb.ToString();
                        sb.Remove(0, sb.Length);
                        index = Message.IndexOf(Attributes[i].ToString(), length, StringComparison.Ordinal);
                    }
                }
            }
        }
        #endregion

        #region  Send emails out 
        public static void SendEmail(string Subject, string Body, string User = "")
        {
            MailMessage mail;
            SmtpClient smtp;
            XmlDocument oDoc;
            XmlElement oRoot;
            XmlNode oNode;
            string sendTo;
            string strPath;
            int i = 0;
            SendEmailAsynch doSendEmail;
            var sb = new StringBuilder();
            sb.Append(modCore.config["TripXMLFolder"]).Append(@"\Tables\Users\");
            strPath = sb.ToString();
            sb.Remove(0, sb.Length);
            oDoc = new XmlDocument();
            try
            {
                sb.Append(strPath).Append("tt_acl.xml");
                oDoc.Load(sb.ToString());
                sb.Remove(0, sb.Length);
            }
            catch (Exception exr)
            {
                SendTrace("", "CoreLib", "TripXMLSendMail: Error Loading tt_acl.xml", exr.Message, string.Empty);
                throw;
            }

            oRoot = oDoc.DocumentElement;
            if (string.IsNullOrEmpty(User))
            {
                foreach (XmlNode currentONode in oRoot.SelectNodes("SendMail/To"))
                {
                    oNode = currentONode;
                    sb.Append(oNode.InnerText);
                    i++;

                    if (i < oRoot.SelectNodes("SendMail/To").Count)
                        sb.Append(";");
                }

                sendTo = sb.ToString();
                sb.Remove(0, sb.Length);
            }
            else
            {
                sb.Append("Customer/User[Username='").Append(User).Append("']/@ErrEmail");
                oNode = oRoot.SelectSingleNode(sb.ToString());
                sb.Remove(0, sb.Length);
                if (oNode is object)
                {
                    sendTo = oNode.InnerText;
                }
                else
                {
                    foreach (XmlNode currentONode1 in oRoot.SelectNodes("SendMail/To"))
                    {
                        oNode = currentONode1;
                        sb.Append(oNode.InnerText);
                        i++;
                        if (i < oRoot.SelectNodes("SendMail/To").Count)
                            sb.Append(";");
                    }

                    sendTo = sb.ToString();
                    sb.Remove(0, sb.Length);
                }
            }

            mail = new MailMessage(oRoot.SelectSingleNode("SendMail/From").InnerText, sendTo, Subject, Body);
            mail.IsBodyHtml = oRoot.SelectSingleNode("SendMail/@Format").InnerText == "Text";

            smtp = new SmtpClient(oRoot.SelectSingleNode("SendMail/SmtpServer").InnerText, 25);
            smtp.UseDefaultCredentials = false;
            var smtpUserInfo = new System.Net.NetworkCredential(oRoot.SelectSingleNode("SendMail/Username").InnerText, oRoot.SelectSingleNode("SendMail/Password").InnerText);
            smtp.Credentials = smtpUserInfo;
            doSendEmail = new SendEmailAsynch(mail, ref smtp);
            doSendEmail.BeginSearch();
        }
        #endregion

    }

    public class SendEmailAsynch
    {
        private delegate void StartSearch_Delegate();

        private StartSearch_Delegate StartSearch_Wrapper;
        private MailMessage mail;
        private SmtpClient smtp;

        public SendEmailAsynch(MailMessage _mail, ref SmtpClient _smtp)
        {
            StartSearch_Wrapper = new StartSearch_Delegate(DoSendEmail);
            mail = _mail;
            smtp = _smtp;
        }

        public void BeginSearch()
        {
            var cbr = new AsyncCallback(EndSearch);
            var arr = StartSearch_Wrapper.BeginInvoke(cbr, null);
        }

        private void EndSearch(IAsyncResult asy)
        {
            StartSearch_Wrapper.EndInvoke(asy);
            asy.AsyncWaitHandle.Close();
        }

        private void DoSendEmail()
        {
            try
            {
                smtp.Send(mail);
                mail.Dispose();
            }
            catch (Exception exr)
            {
                CoreLib.SendTrace("", "CoreLib", "TripXMLSendMail: Error sending email", exr.Message, string.Empty);
                throw;
            }
            finally
            {
                smtp = null;
            }
        }
    }
}