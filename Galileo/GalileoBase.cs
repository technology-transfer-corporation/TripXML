using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using TripXMLMain;


namespace Galileo
{
    public abstract class GalileoBase
    {

        private string mstrVersion = "";
        private string mstrXslPath = "";

        public string ConversationID { get; set; }

        public string Request { get; set; }

        public modCore.TripXMLProviderSystems ProviderSystems { get; set; }

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
            set => mstrXslPath = $"{value}Galileo\\";
        }

        public static string GetRecordLocatorFromNative(string nativeResponse, string userID)
        {
            string recordLocator = "";
            try
            {
                var index = nativeResponse.IndexOf("<RecLocator>", StringComparison.Ordinal) + 12;
                if (index > 0)
                {
                    var length = nativeResponse.IndexOf("</RecLocator>", index, StringComparison.Ordinal) - index;
                    recordLocator = nativeResponse.Substring(index, length);
                }
            }
            catch (Exception ex)
            {
                recordLocator = "";
            }

            CoreLib.SendTrace(userID, "ttGalileoService", "Record Locator", recordLocator, string.Empty);
            return recordLocator;
        }
        
        protected static string FormatGalileo(string strDisplay)
        {
            string display = "";
            int bucket = 64;
            var count = (int)Math.Ceiling((double)strDisplay.Length / bucket);

            strDisplay = strDisplay.Replace(" & ", " and ");

            var Lines = strDisplay.Contains("\n")
                ? strDisplay.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList()
                : Enumerable.Range(0, count)
                                    .Select(_ => _ * bucket)
                                    .Select(_ => strDisplay.Substring(_, Math.Min(bucket, strDisplay.Length - _)))
                                    .ToList();
            if (Lines.Any(l => l.Length > 64))
                Lines = DeepFormating(Lines);
            //Regex.Split(strDisplay, "(?<=^(.{64})+)").ToList();

            Lines.ForEach(l => display += $"<Line>{l.Replace("<", "&lt;").Replace(">", "&gt;")}</Line>");

            return $"<Screen>{display}</Screen>";
        }

        private static List<string> DeepFormating(List<string> display)
        {
            var Lines = new List<string>();
            foreach (var line in display)
            {
                if (line.Contains("\n"))
                {
                    var subLines = line.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    foreach (var sl in subLines)
                    {
                        if (sl.Contains("\n"))
                        {
                            Lines.AddRange(sl.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList());
                        }
                        else
                        {
                            if (sl.Trim().Length < 64)
                            {
                                Lines.Add(sl);
                            }
                            else
                            {
                                int bucket = 64;
                                var count = (int)Math.Ceiling((double)sl.Length / bucket);

                                Lines.AddRange(Enumerable.Range(0, count)
                                    .Select(_ => _ * bucket)
                                    .Select(_ => sl.Substring(_, Math.Min(bucket, sl.Length - _)))
                                    .ToList());
                            }
                        }
                    }
                }
                else
                {
                    int bucket = 64;
                    var count = (int)Math.Ceiling((double)line.Length / bucket);
                    Lines.AddRange(Enumerable.Range(0, count)
                                    .Select(_ => _ * bucket)
                                    .Select(_ => line.Substring(_, Math.Min(bucket, line.Length - _)))
                                    .ToList());
                }
            }
            return Lines;
        }

        protected GalileoAdapter SetAdapter(string version = "")
        {
            var argProviderSystems = ProviderSystems;
            var tt = string.IsNullOrEmpty(version) ? new GalileoAdapter(argProviderSystems) : new GalileoAdapter(argProviderSystems, version);

            ProviderSystems = argProviderSystems;
            return tt;
        }

        protected string SetRequest(string xslFile)
        {
            try
            {
                #region Get Tracer ID
                var otaDoc = new XmlDocument();
                otaDoc.LoadXml(Request);
                var otaElement = otaDoc.DocumentElement;
                ConversationID =
                    otaElement != null && otaElement.HasAttribute("EchoToken") && otaElement.Attributes["EchoToken"].Value != null
                        ? otaElement.Attributes["EchoToken"].Value
                        : "";

                //if (String.IsNullOrEmpty(ConversationID))
                //    ConversationID = ProviderSystems.SessionPool ? ttGA.CheckSessionV2() : ttGA.CreateSession();

                //if (!string.IsNullOrEmpty(ConversationID) && ConversationID.Contains("Error"))
                //{
                //    string conv = ConversationID.Substring(15, ConversationID.Length - 32);
                //    ConversationID = "";
                //    throw new Exception(conv);
                //}


                #endregion

                Request = Request.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Replace("<?xml version=\"1.0\"?>", "");

                if (string.IsNullOrEmpty(xslFile))
                    return Request;

                return CoreLib.TransformXML(Request, mstrXslPath, $"{Version}{xslFile}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming OTA Request. {ex.Message}");
            }
        }

        protected bool SetConversationID(GalileoAdapter ttAA)
        {
            try
            {
                var oDocReq = new XmlDocument();
                oDocReq.LoadXml(Request);
                var oRootReq = oDocReq.DocumentElement;

                XmlNode oNodeSPL = oRootReq?.SelectSingleNode("ConversationID");

                if (oNodeSPL == null)
                {
                    var oElem = oRootReq?.GetElementsByTagName("ConversationID");
                    oNodeSPL = oElem is { Count: > 0 } ? oElem[0] : null;
                }

                if (oNodeSPL != null && !string.IsNullOrEmpty(oNodeSPL.InnerText))
                {
                    ConversationID = oNodeSPL.InnerText.Trim();
                    if (!string.IsNullOrEmpty(ConversationID))
                        return true;
                }

                if (string.IsNullOrEmpty(ConversationID))
                    ConversationID = ttAA.CreateSession();

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

        protected string UpdateSessionID(string sessionID)
        {
            int intSession;
            var session = sessionID.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            if (ProviderSystems.SOAP2)
            {
                intSession = Convert.ToInt32(session[2]);
                intSession += 1;
                sessionID = $"{session[0]}|{session[1]}|{intSession}";
            }
            else
            {
                intSession = Convert.ToInt32(session[1]);
                intSession += 1;
                sessionID = $"{session[0]}|{intSession}";
            }

            return sessionID;
        }

        protected void LogMessageToFile(string msgType, ref string message, DateTime requestTime, DateTime responseTime)
        {
            try
            {
                var dur = responseTime - requestTime;
                string strLine = $"<Message Type=\'{msgType}\' RequestTime=\'{requestTime:dd MMM yyyy HH:mm:ss}\' ResponseTime=\'{responseTime:dd MMM yyyy HH:mm:ss}\' Duration=\'{dur.TotalSeconds}\'><GalileoMessage>{message}</GalileoMessage></Message>";
                AddLog(strLine, ProviderSystems.UserID);
            }
            catch (Exception ex)
            {
                //throw new Exception($"Error Transforming OTA Request. {ex.Message}");
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
    }
}