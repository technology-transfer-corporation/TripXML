using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        
        public static string FormatGalileo(string strDisplay)
        {
            string display = "";
            int bucket = 64;
            var count = (int)Math.Ceiling((double)strDisplay.Length / bucket);

            //strDisplay = strDisplay.Replace("&gt;&lt;", "\r").Replace(" & ", " and ");
            strDisplay = strDisplay.Replace(" & ", " and ");

            var Lines = strDisplay.Contains("\n")
                ? strDisplay.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList()
                : Enumerable.Range(0, count)
                                    .Select(_ => _ * bucket)
                                    .Select(_ => strDisplay.Substring(_, Math.Min(bucket, strDisplay.Length - _)))
                                    .ToList();
            var _lines = new List<string>();

            Lines.ForEach(l=> _lines.Add(l.Replace(";", "").Replace("&gt", "").Replace("&lt", "")));
            _lines = _lines.FindAll(l => !string.IsNullOrEmpty(l));
            if (_lines.Any(l => l.Length > 64))
                _lines = DeepFormating(_lines);

            //Regex.Split(strDisplay, "(?<=^(.{64})+)").ToList();

            //_lines.ForEach(l => display += $"<Line>{l.Replace("<", "&lt;").Replace(">", "&gt;")}</Line>");
            //return $"<Screen>{display}</Screen>";

            display = string.Join("\r\n", _lines);            
            return display;
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
            var tt = string.IsNullOrEmpty(version) 
                ? new GalileoAdapter(argProviderSystems) 
                : new GalileoAdapter(argProviderSystems, version);

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

                if(string.IsNullOrEmpty(ConversationID))
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

                if (!string.IsNullOrEmpty(ConversationID))
                    ttAA.Emulate(ConversationID);

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
                AddLog(strLine, ProviderSystems);
            }
            catch (Exception ex)
            {
                //throw new Exception($"Error Transforming OTA Request. {ex.Message}");
            }
        }

        protected static void AddLog(string msg, modCore.TripXMLProviderSystems provider)
        {
            try
            {
                modCore.AddLog(modCore.LogType.Info, msg, provider);
                //string filePath = $"log\\{username}_{DateTime.Today:dd-MM-yyyy}";
                //string dirPath = ConfigurationManager.AppSettings["TripXMLLogFolder"]; //"C:\\TripXML\\log"

                //FileInfo ffInfo = new FileInfo(filePath);
                //if (ffInfo.Directory is { Exists: false })
                //    ffInfo.Directory.Create();

                //if (!ffInfo.Exists)
                //{
                //    WriteLogEntry($"{dirPath}\\{filePath}.txt", $"created On - {DateTime.Now}\r\n");
                //}
                //else
                //{
                //    DateTimeFormatInfo myDtfi = new CultureInfo("en-US", true).DateTimeFormat;
                //    WriteLogEntry($"{dirPath}\\{filePath}.txt", $"{DateTime.UtcNow.ToString(myDtfi).Substring(11)} GMT - {msg}\r\n");
                //}
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding line to Log.", ex);
            }
        }
        private static void WriteLogEntry(string filePath, string log, bool isAppend = true)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, !File.Exists(filePath) || !isAppend ? FileMode.Create : FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(stream))
                    {
                        streamWriter.WriteLine(log);
                        streamWriter.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #region Native TripXML Messages
        protected string SendLowFareSearchPlus(string strRequest)
        {
            try
            {
                AirServices _req = new AirServices { Request = strRequest, ProviderSystems = ProviderSystems, XslPath = XslPath, ConversationID = ConversationID, Version = Version };
                return _req.LowFarePlus();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendAirAvail(string strRequest)
        {
            try
            {
                AirServices _req = new AirServices { Request = strRequest, ProviderSystems = ProviderSystems, XslPath = XslPath, ConversationID = ConversationID, Version = Version };
                return _req.AirAvail();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string SendAirFlifo(string strRequest)
        {
            try
            {
                AirServices _req = new AirServices { Request = strRequest, ProviderSystems = ProviderSystems, XslPath = XslPath, ConversationID = ConversationID, Version = Version };
                return _req.AirFlifo();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}