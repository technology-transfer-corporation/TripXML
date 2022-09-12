using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Xml;
using TripXMLMain;

namespace Sabre
{
    public abstract class SabreBase
    {
        private string mstrVersion = "";
        private string mstrXslPath = "";
        private modCore.TripXMLProviderSystems providerSystems;

        protected string ConversationID { get; set; }

        public string Request { get; set; }

        public modCore.TripXMLProviderSystems ProviderSystems
        {
            get => providerSystems;
            set => providerSystems = value;
        }

        public string Version
        {
            get { return mstrVersion; }
            set
            {
                mstrVersion = value;
                if (!string.IsNullOrEmpty(mstrVersion)) mstrVersion += "_";
            }
        }

        public string XslPath
        {
            get => mstrXslPath;
            set => mstrXslPath = $"{value}Sabre\\";
        }

        protected void LogMessageToFile(string msgType, ref string message, DateTime requestTime, DateTime responseTime)
        {
            try
            {
                TimeSpan dur;
                dur = responseTime - requestTime;
                string strLine = $"<Message Type=\'{msgType}\' RequestTime=\'{requestTime.ToString("dd MMM yyyy HH:mm:ss")}\' ResponseTime=\'{responseTime.ToString("dd MMM yyyy HH:mm:ss")}\' Duration=\'{dur.TotalSeconds}\'><GalileoMessage>{message}</GalileoMessage></Message>";
                AddLog(strLine, ProviderSystems.UserID);
            }
            catch (Exception)
            {
                //  
            }
        }

        protected static void AddLog(string msg, string username)
        {
            try
            {
                string filePath = $"log\\{username}_{DateTime.Today:dd-MM-yyyy}";
                string dirPath = ConfigurationManager.AppSettings["TripXMLLogFolder"]; //"C:\\TripXML\\log"
                filePath = $"{dirPath}\\{filePath}.txt";

                FileInfo ffInfo = new FileInfo(filePath);

                if (ffInfo.Directory is { Exists: false })
                {
                    ffInfo.Directory.Create();
                }

                if (!ffInfo.Exists)
                {
                    using StreamWriter sw = ffInfo.CreateText();
                    sw.WriteLine("created On - {0}\r\n", DateTime.Now);
                    sw.Flush();
                    sw.Close();
                }

                using (StreamWriter sw = ffInfo.AppendText())
                {
                    DateTimeFormatInfo myDtfi = new CultureInfo("en-US", true).DateTimeFormat;

                    sw.WriteLine($"{DateTime.UtcNow.ToString(myDtfi).Substring(11)} GMT - {msg}\r\n");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding line to Log.\r\n{ex.Message}");
            }
        }

        protected SabreAdapter SetAdapter(string version = "")
        {
            var argProviderSystems = ProviderSystems;
            var tt = string.IsNullOrEmpty(version) ? new SabreAdapter(argProviderSystems) : new SabreAdapter(argProviderSystems, version);

            ProviderSystems = argProviderSystems;
            return tt;
        }

        protected string SetRequest(string xslFile)
        {
            try
            {
                #region Get Tracer ID
                var otaDoc = new XmlDocument();
                XmlElement otaElement;
                otaDoc.LoadXml(Request);
                otaElement = otaDoc.DocumentElement;

                if (otaElement != null && otaElement?.SelectSingleNode("//ConversationID") != null && otaElement?.SelectSingleNode("//ConversationID").InnerText != null)
                {
                    ConversationID = otaElement?.SelectSingleNode("//ConversationID").InnerText;
                }
                else
                {
                    ConversationID = "";
                }
                #endregion

                Request = Request.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Replace("<?xml version=\"1.0\"?>", "").Replace("xmlns=\"\"", "");

                if (string.IsNullOrEmpty(xslFile))
                    return Request;

                return CoreLib.TransformXML(Request, mstrXslPath, $"{Version}{xslFile}").Replace("xmlns=\"\"", "");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming OTA Request. {ex.Message}");
            }
        }

        protected bool SetConversationID(SabreAdapter ttAA)
        {
            try
            {
                if (!string.IsNullOrEmpty(ConversationID))
                {
                    return true;
                }
                var oDocReq = new XmlDocument();
                oDocReq.LoadXml(Request);
                var oRootReq = oDocReq.DocumentElement;

                XmlNode oNodeSPL = oRootReq?.SelectSingleNode("ConversationID");

                if (oNodeSPL == null)
                {
                    var oElem = oRootReq?.GetElementsByTagName("ConversationID");
                    oNodeSPL = oElem is { Count: > 0 } ? oElem[0] : null;
                }

                if (oNodeSPL != null)
                {
                    ConversationID = oNodeSPL.InnerText;
                }

                if (!string.IsNullOrEmpty(ConversationID))
                {
                    return true;
                }
                else
                {
                    ConversationID = ttAA.CreateSession();
                    CoreLib.SendTrace(ProviderSystems.UserID, "ttSabreService", "Start with clean working area", "", ProviderSystems.LogUUID);
                }

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

    }
}
