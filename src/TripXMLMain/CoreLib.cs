using System;
using System.IO;
using System.IO.Compression;
using System.Net.Mail;
using System.Text;
using System.Xml;
using System.Threading;
using System.Collections.Concurrent;
using WebSocket = WebSocketSharp.WebSocket;
using WebSocketState = WebSocketSharp.WebSocketState;
using System.Collections.Generic;

namespace TripXMLMain
{
    /// <summary>
    /// Tracing and othe communicational methods
    /// </summary>
    public static class CoreLib
    {
        private static BlockingCollection<string> _senderQueue = new BlockingCollection<string>();
        private static Thread _traceSend;

        #region  Transform XML with XSLs 

        private static readonly ConcurrentDictionary<string, System.Xml.Xsl.XslCompiledTransform> _xslCache =
            new ConcurrentDictionary<string, System.Xml.Xsl.XslCompiledTransform>(StringComparer.OrdinalIgnoreCase);

        public static string TransformXML(string inputXml, string xslPath, string xslName, bool fromFile = false)
        {
            XmlDocument oDoc;
            StringWriter oWriter = null;

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

                string xslFile = Path.Combine(xslPath ?? "", xslName);
                if (!xslFile.EndsWith(".xsl", StringComparison.OrdinalIgnoreCase))
                {
                    xslFile += ".xsl";
                }

                // Stylesheets are loaded from source and compiled once per process; the cache
                // replicates the performance profile of the retired xsltc-precompiled assemblies.
                var xslt = _xslCache.GetOrAdd(xslFile, static path =>
                {
                    var t = new System.Xml.Xsl.XslCompiledTransform();
                    var settings = new System.Xml.Xsl.XsltSettings(enableDocumentFunction: true, enableScript: false);
                    t.Load(path, settings, new XmlUrlResolver());
                    return t;
                });

                var args = new System.Xml.Xsl.XsltArgumentList();
                args.AddExtensionObject(TtVbXsltFunctions.Namespace, TtVbXsltFunctions.Instance);

                oWriter = new StringWriter();
                xslt.Transform(oDoc.DocumentElement.ParentNode, args, oWriter);
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

        /// <summary>Drops all compiled stylesheets so edited .xsl files are picked up (wsRefreshMem hook).</summary>
        public static void ClearXslCache()
        {
            _xslCache.Clear();
        }

        #endregion

        #region  Send Trace 

        public static void SendTrace(string userID, string file, string text, string item, string UUID)
        {
            if (_traceSend == null)
            {
                _traceSend = new Thread(TraceSender);
                _traceSend.Start();
            }

            try
            {
                item = item.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                item = item.Replace("<?xml version='1.0' encoding='utf-8'?>", "");
                item = item.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>", "");
                item = item.Replace("<?xml version=\"1.0\" encoding=\"ISO-8859-1\" standalone=\"yes\" ?>", "");
                item = item.Replace("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"  standalone=\"yes\"?>", "");
                item = item.Replace("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"  standalone=\"yes\" ?>", "");
                item = item.Replace("xmlns = \"\"", "");

                userID = userID ?? "";                

                var msg = $"<{file}><Server>{modCore.config["ServerGuid"]}</Server><Text>{text}</Text><UUID>{UUID}</UUID><Item>{item}</Item><UserID>{userID}</UserID></{file}>";
                if (_senderQueue.Count < 50)
                    _senderQueue.Add(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void TraceSender()
        {
            try
            {
              TraceSender(new string[] { "ws://localhost:3070/Trace", modCore.config["TraceServerUrl"] });
              //TraceSender(new string[] { "ws://localhost:3070/Trace", "ws://localhost:8111/Trace" });
              //TraceSender("ws://localhost:3070/Trace");
            }
            catch (Exception)
            {
                throw;
            }            
        }

        private static void TraceSender(string[] paths)
        {
            List<WebSocket> webSockets = new List<WebSocket>();

            foreach (string path in paths)
            {
                // TraceServerUrl may be unset; an empty url throws in the WebSocket ctor and a
                // background-thread exception would take the whole host down on modern .NET.
                if (string.IsNullOrWhiteSpace(path)) continue;
                WebSocket ws = new WebSocket(path);
                webSockets.Add(ws);
            }

            while (true)
            {
                var reconnect = false;
                string message = _senderQueue.Take();

                foreach (WebSocket wsClient in webSockets)
                {
                    try
                    {
                        if (wsClient.ReadyState != WebSocketState.Open) wsClient.Connect();
                        if (wsClient.ReadyState == WebSocketState.Connecting)
                            Thread.Sleep(1000);
                        wsClient.Send(message);
                    }
                    catch
                    {
                        reconnect = true;
                    }

                    if (reconnect)
                    {
                        try
                        {
                            if (wsClient.ReadyState != WebSocketState.Open) wsClient.Connect();
                            if (wsClient.ReadyState == WebSocketState.Connecting)
                                Thread.Sleep(1000);
                            wsClient.Send(message);
                        }
                        catch { }
                    }
                }
            }
        }

        private static void TraceSender(string pathTrace)
        {
            var wsClient = new WebSocket(pathTrace);

            while (true)
            {
                var reconnect = false;
                
                string message = _senderQueue.Take();
                try
                {
                     
                    if (wsClient.ReadyState != WebSocketState.Open) wsClient.Connect();
                    if (wsClient.ReadyState == WebSocketState.Connecting)
                        Thread.Sleep(1000);
                    wsClient.Send(message);
                }
                catch
                {
                    reconnect = true;
                }

                if (reconnect)
                    try
                    {
                        if (wsClient.ReadyState != WebSocketState.Open) wsClient.Connect();
                        if (wsClient.ReadyState == WebSocketState.Connecting)
                            Thread.Sleep(1000);
                        wsClient.Send(message);
                    }
                    catch {}
            }
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


        public static IEnumerable<string> SplitBy(this string str, int chunkLength)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentException();
            if (chunkLength < 1) throw new ArgumentException();

            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
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
    /// <summary>
    /// Sending Email asynch
    /// </summary>
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

    public static class XmlExtensions
    {
        public static XmlElement AddElement(this XmlNode parent, string name, string namespaceUri = null)
        {
            var doc = parent.OwnerDocument ?? (XmlDocument)parent;
            var element = string.IsNullOrEmpty(namespaceUri)
                ? doc.CreateElement(name)
                : doc.CreateElement(name, namespaceUri);
            parent.AppendChild(element);
            return element;
        }

        public static XmlElement WithAttribute(this XmlElement element, string name, string value)
        {
            element.SetAttribute(name, value);
            return element;
        }

        public static XmlElement WithText(this XmlElement element, string text)
        {
            element.InnerText = text;
            return element;
        }

        public static XmlElement AddElements<T>(
            this XmlElement parent,
            IEnumerable<T> items,
            string elementName,
            string namespaceUri,
            Action<XmlElement, T> configure)
        {
            foreach (var item in items)
            {
                var element = parent.AddElement(elementName, namespaceUri);
                configure(element, item);
            }
            return parent;
        }
    }
}