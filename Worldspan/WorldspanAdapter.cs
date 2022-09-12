using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json.Linq;
using TripXMLMain;

namespace Worldspan
{
    public class WorldspanAdapter
    {
        private modCore.TripXMLProviderSystems ttProviderSystems;
        public string ConversationID = "";

        public WorldspanAdapter(modCore.TripXMLProviderSystems providerSystems)
        {
            ttProviderSystems = providerSystems;
            ConversationID = $"|{providerSystems.Profile}";
        }

        private string Send(string body, string token, string uuid)
        {
            int intStart = 0;
            int intLength = 0;
            try
            {
                ttHttpWebClient oHttpWebClient = new ttHttpWebClient();
                string strHeader;
                {
                    var withBlock = ttProviderSystems;
                    strHeader = "<t:Transaction xmlns:t=\"xxs\"><tc>" +
                                $"<iden u=\"{withBlock.UserName}\" p=\"{withBlock.Password}\"/>" +
                                $"<provider session=\"{withBlock.Profile}\" pcc=\"{withBlock.PCC}\">Worldspan</provider></tc></t:Transaction>";

                }
                
                var withBlock1 = oHttpWebClient;
                withBlock1.SoapAction = ttProviderSystems.SOAPAction;
                withBlock1.ServiceURL = ttProviderSystems.URL;
                withBlock1.HttpMethod = "POST";
                withBlock1.Body = body;
                withBlock1.Header = strHeader;
                var strResponse = withBlock1.SendHttpRequest(ttProviderSystems.UserID, token, uuid);
                

                // Get Response From Soap

                if (strResponse.Contains("GetProviderSessionResponse"))
                {
                    if (strResponse.Contains("CONTEXT"))
                    {
                        intStart = strResponse.IndexOf("CONTEXT", StringComparison.Ordinal) + 8;
                        intLength = strResponse.IndexOf("</CONTEXT>", StringComparison.Ordinal) - strResponse.IndexOf("CONTEXT", StringComparison.Ordinal) - 8;
                    }
                    
                }
                else
                {
                    var strNode = "RSP";
                    var xmlResponse = new System.Xml.XmlDocument();
                    xmlResponse.LoadXml(strResponse);

                    // Worldspan error
                    if (xmlResponse.GetElementsByTagName("faultcode").Count > 0)
                    {
                        var xmlResp = new System.Xml.XmlDocument();
                        xmlResp.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><XXW></XXW>");
                        System.Xml.XmlNode rootNode = xmlResp.DocumentElement;
                        var erNode = xmlResp.CreateNode("element", "ERROR", "");
                        if (rootNode != null) rootNode.AppendChild(erNode);
                        var errCode = xmlResp.CreateNode("element", "CODE", "");
                        errCode.InnerText = xmlResponse.GetElementsByTagName("faultcode").Item(0)?.InnerText ??
                                            string.Empty;
                        erNode.AppendChild(errCode);
                        var errText = xmlResp.CreateNode("element", "TEXT", "");
                        errText.InnerText = xmlResponse.GetElementsByTagName("faultstring").Item(0)?.InnerText ??
                                            string.Empty;
                        erNode.AppendChild(errText);
                        return xmlResp.OuterXml;
                    }

                    if (!strResponse.Contains($"<{strNode}>"))
                        return "";
                    
                    intStart = strResponse.IndexOf($"<{strNode}>", StringComparison.Ordinal) + strNode.Length + 2;
                    
                    intLength = strResponse.IndexOf($"</{strNode}>", StringComparison.Ordinal) - intStart;
                }

                return strResponse.Substring(intStart, intLength).Trim();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Request to Worldspan.\r\n{ex.Message}");
            }
        }

        private string Send(string body, string uuid)
        {
            int intStart = 0;
            int intLength = 0;
            try
            {
                var oHttpWebClient = new ttHttpWebClient();
                var withBlock = ttProviderSystems;
                var strHeader = "<t:Transaction xmlns:t=\"xxs\"><tc>" +
                                $"<iden u=\"{withBlock.UserName}\" p=\"{withBlock.Password}\"/>" +
                                $"<provider session=\"{withBlock.Profile}\" pcc=\"{withBlock.PCC}\">Worldspan</provider></tc></t:Transaction>";
            
                var token = ConversationID.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                
                var withBlock1 = oHttpWebClient;
                withBlock1.SoapAction = ttProviderSystems.SOAPAction;
                withBlock1.ServiceURL = ttProviderSystems.URL;
                withBlock1.HttpMethod = "POST";
                withBlock1.Body = body;
                withBlock1.Header = strHeader;
                var strResponse = withBlock1.SendHttpRequest(ttProviderSystems.UserID, token[0], uuid);
                

                // Get Response From Soap

                if (strResponse.Contains("GetProviderSessionResponse"))
                {
                    if (strResponse.Contains("CONTEXT"))
                    {
                        intStart = strResponse.IndexOf("CONTEXT", StringComparison.Ordinal) + 8;
                        intLength = strResponse.IndexOf("</CONTEXT>", StringComparison.Ordinal) - strResponse.IndexOf("CONTEXT", StringComparison.Ordinal) - 8;
                    }
                }
                else
                {
                    var strNode = "RSP";
                    var xmlResponse = new System.Xml.XmlDocument();
                    xmlResponse.LoadXml(strResponse);

                    // Worldspan error
                    if (xmlResponse.GetElementsByTagName("faultcode").Count > 0)
                    {
                        var xmlResp = new System.Xml.XmlDocument();
                        xmlResp.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><XXW></XXW>");
                        System.Xml.XmlNode rootNode = xmlResp.DocumentElement;
                        var erNode = xmlResp.CreateNode("element", "ERROR", "");
                        if (rootNode != null) rootNode.AppendChild(erNode);
                        var errCode = xmlResp.CreateNode("element", "CODE", "");
                        errCode.InnerText = xmlResponse.GetElementsByTagName("faultcode").Item(0)?.InnerText ??
                                            string.Empty;
                        erNode.AppendChild(errCode);
                        var errText = xmlResp.CreateNode("element", "TEXT", "");
                        errText.InnerText = xmlResponse.GetElementsByTagName("faultstring").Item(0)?.InnerText ??
                                            string.Empty;
                        erNode.AppendChild(errText);
                        return xmlResp.OuterXml;
                    }

                    if (!strResponse.Contains($"<{strNode}>"))
                    {
                        return "";
                    }

                    
                    intStart = strResponse.IndexOf($"<{strNode}>", StringComparison.Ordinal) + strNode.Length + 2;
                    intLength = strResponse.IndexOf($"<{strNode}>", StringComparison.Ordinal) - intStart;
                }

                return strResponse.Substring(intStart, intLength).Trim();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Request to Worldspan.\r\n{ex.Message}");
            }
            
        }

        public string SendCryptic(string entry, string conversationID, string rocordLocator = "", string uuid = "")
        {
            try
            {
                var oHttpWebClient = new ttHttpWebClient();
                string strHeader;
                {
                    var withBlock = ttProviderSystems;
                    strHeader = "<t:Transaction xmlns:t=\"xxs\"><tc>" +
                    $"<iden u=\"{withBlock.UserName}\" p=\"{withBlock.Password}\"/>" + 
                    $"<provider session=\"{withBlock.Profile}\" pcc=\"{withBlock.PCC}\">Worldspan</provider></tc></t:Transaction>";
                }

                var body = $"<OTA_ScreenTextRQ Version=\"1\" xmlns=\"http://www.opentravel.org/OTA/2003/05\"><ScreenEntry>{entry}</ScreenEntry></OTA_ScreenTextRQ>";
                
                if (string.IsNullOrEmpty(uuid) & !string.IsNullOrEmpty(ttProviderSystems.LogUUID))
                {
                    uuid = ttProviderSystems.LogUUID;
                }

                var token = ConversationID.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                oHttpWebClient.SoapAction = ttProviderSystems.SOAPAction;
                oHttpWebClient.ServiceURL = ttProviderSystems.URL;
                oHttpWebClient.HttpMethod = "POST";
                oHttpWebClient.Body = body;
                oHttpWebClient.Header = strHeader;
                var strResponse = oHttpWebClient.SendHttpRequest(ttProviderSystems.UserID, token[0], uuid);

                // ********************************************************************************
                // parse the response and create screen with lines                               *
                // ******************************************************************************** 
                // Dim xmlDocResp As XElement
                if (strResponse.Contains("<Errors>") | strResponse.Contains("faultstring"))
                {
                    if (strResponse.Contains("<RSP>"))
                        strResponse = strResponse.Substring(strResponse.IndexOf("<RSP>", StringComparison.Ordinal), strResponse.IndexOf("</RSP>", StringComparison.Ordinal) - strResponse.IndexOf("<RSP>", StringComparison.Ordinal) + 6);
                    if (strResponse.Contains("<Errors>"))
                        strResponse = strResponse.Substring(strResponse.IndexOf("<Errors>", StringComparison.Ordinal), strResponse.IndexOf("</Errors>", StringComparison.Ordinal) - strResponse.IndexOf("<Errors>", StringComparison.Ordinal) + 9);
                    if (strResponse.Contains("<Error>"))
                        strResponse = strResponse.Substring(strResponse.IndexOf("<Error>", StringComparison.Ordinal), strResponse.IndexOf("</Error>", StringComparison.Ordinal) - strResponse.IndexOf("<Error>", StringComparison.Ordinal) + 8);
                    if (strResponse.Contains("<faultstring>"))
                    {
                        strResponse = strResponse.Substring(strResponse.IndexOf("<faultstring>", StringComparison.Ordinal), strResponse.IndexOf("</faultstring>", StringComparison.Ordinal) - strResponse.IndexOf("<faultstring>", StringComparison.Ordinal) + 14);
                    }
                    else
                    {
                        strResponse = strResponse.Split(new[] { " ", "<", ">", "/", "Error" }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("ShortText=", "").Replace("\"", "");
                    }
                }
                else
                {
                    strResponse = strResponse.Substring(strResponse.IndexOf("<TextData>", StringComparison.Ordinal) + 10, strResponse.IndexOf("</TextData>", StringComparison.Ordinal) - strResponse.IndexOf("<TextData>", StringComparison.Ordinal) - 10);
                    var strScreen = strResponse.Replace("\r", "\r\n").Replace("\n", "\r\n");
                    strResponse = formatWorldspan(strScreen, rocordLocator);
                }

                CoreLib.SendTrace(ttProviderSystems.UserID, "Cryptic", entry, strResponse, ttProviderSystems.LogUUID);
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Request to Worldspan.\r\n{ex.Message}");
            }
        }

        public string SendCryptic(string entry, string rocordLocator = "", string uuid = "")
        {
            try
            {
                //var oHttpWebClient = new ttHttpWebClient();
                //string strHeader;
                //{
                //    var withBlock = ttProviderSystems;
                //    strHeader = "<t:Transaction xmlns:t='xxs'><tc>" + $"<iden u='{withBlock.UserName}' p='{withBlock.Password}'/>" + $"<provider session='{withBlock.Profile}' pcc='{withBlock.PCC}'>Worldspan</provider></tc></t:Transaction>";
                //}

                string body = entry.Contains("OTA_ScreenTextRQ") 
                    ? entry 
                    : $"<OTA_ScreenTextRQ Version=\"1\" xmlns=\"http://www.opentravel.org/OTA/2003/05\"><ScreenEntry>{entry}</ScreenEntry></OTA_ScreenTextRQ>";

                if (string.IsNullOrEmpty(uuid) & !string.IsNullOrEmpty(ttProviderSystems.LogUUID))
                {
                    uuid = ttProviderSystems.LogUUID;
                }

                var token = ConversationID.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                //oHttpWebClient.SoapAction = ttProviderSystems.SOAPAction;
                //oHttpWebClient.ServiceURL = ttProviderSystems.URL;
                //oHttpWebClient.HttpMethod = "POST";
                //oHttpWebClient.Body = body;
                //oHttpWebClient.Header = strHeader;
                //var strResponse = oHttpWebClient.SendHttpRequest(ttProviderSystems.UserID, token[0], uuid);
                var strResponse = Send(body, token[0], uuid);

                // ********************************************************************************
                // parse the response and create screen with lines                                *
                // ******************************************************************************** 
                // Dim xmlDocResp As XElement
                if (strResponse.Contains("<Errors>") | strResponse.Contains("faultstring"))
                {
                    if (strResponse.Contains("<RSP>"))
                        strResponse = strResponse.Substring(strResponse.IndexOf("<RSP>", StringComparison.Ordinal), strResponse.IndexOf("</RSP>", StringComparison.Ordinal) - strResponse.IndexOf("<RSP>", StringComparison.Ordinal) + 6);
                    if (strResponse.Contains("<Errors>"))
                        strResponse = strResponse.Substring(strResponse.IndexOf("<Errors>", StringComparison.Ordinal), strResponse.IndexOf("</Errors>", StringComparison.Ordinal) - strResponse.IndexOf("<Errors>", StringComparison.Ordinal) + 9);
                    if (strResponse.Contains("<Error>"))
                        strResponse = strResponse.Substring(strResponse.IndexOf("<Error>", StringComparison.Ordinal), strResponse.IndexOf("</Error>", StringComparison.Ordinal) - strResponse.IndexOf("<Error>", StringComparison.Ordinal) + 8);
                    if (strResponse.Contains("<faultstring>"))
                    {
                        strResponse = strResponse.Substring(strResponse.IndexOf("<faultstring>", StringComparison.Ordinal), strResponse.IndexOf("</faultstring>", StringComparison.Ordinal) - strResponse.IndexOf("<faultstring>", StringComparison.Ordinal) + 14);
                    }
                    else
                    {
                        //strResponse = strResponse.Split(new[] { " ", "<", ">", "/", "Error" }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("ShortText=", "").Replace("\"", "");
                        var oDoc = new XmlDocument();
                        oDoc.LoadXml(strResponse);
                        var oRoot = oDoc.DocumentElement;
                        strResponse = formatWorldspan($"{oRoot.GetElementsByTagName("Error")[0].Attributes["ShortText"].InnerText.Trim()}") ;
                    }
                }
                else
                {
                    strResponse = strResponse.Substring(strResponse.IndexOf("<TextData>", StringComparison.Ordinal) + 10, strResponse.IndexOf("</TextData>", StringComparison.Ordinal) - strResponse.IndexOf("<TextData>", StringComparison.Ordinal) - 10);
                    string strScreen = strResponse.Replace("&gt;", "").Replace("\r", "\r\n").Replace("\n", "\r\n");
                    strResponse = formatWorldspan(strScreen, rocordLocator);
                    
                }

                CoreLib.SendTrace(ttProviderSystems.UserID, "Cryptic", entry, strResponse, ttProviderSystems.LogUUID);
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Request to Worldspan.\r\n{ex.Message}");
            }
        }

        public string SendMessage(string message, string conversationID = "")
        {
            try
            {
                var token = string.IsNullOrEmpty(conversationID)
                    ? ConversationID.Split(new[] { '|' }, StringSplitOptions.None)
                    : conversationID.Split(new[] { '|' }, StringSplitOptions.None);

                var strResponse = Send(message, token[0], ttProviderSystems.LogUUID);
                if (strResponse.Contains($"no session pool configured with name {ttProviderSystems.ProfileXML}") || strResponse.Contains($"no session pool configured with name {ttProviderSystems.ProfileTicketing}"))
                {
                    strResponse = Send(message, token[0], ttProviderSystems.LogUUID);
                }
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Sending Message to Worldspan.\r\n{ex.Message}");
            }
        }

        public string CreateSession()
        {
            // Dim token As String
            string body;
            CoreLib.SendTrace(ttProviderSystems.UserID, "ttWorldspanAdapter", "Create Session", "", ttProviderSystems.LogUUID);
            try
            {
                body = "<ns1:GetProviderSession xmlns:ns1=\"xxs\"><OPTIONS/></ns1:GetProviderSession>";
                string token = Send(body, "", ttProviderSystems.LogUUID);
                ConversationID = $"{token}|{ttProviderSystems.Profile}";

                // Write a procedure that will write a Log file with information on just opened session for SPLUNK
                var trace = new JObject(new JProperty("Provider", ttProviderSystems.Provider), new JProperty("ID", ConversationID), new JProperty("Type", "Open"), new JProperty("User", ttProviderSystems.UserID), new JProperty("UUID", ttProviderSystems.LogUUID), new JProperty("TimeStamp", DateTime.Now));
                string argMessage = "Session Manager";
                modCore.AddLog(modCore.LogType.Info, ref argMessage, ttProviderSystems, trace);
                return ConversationID;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Creating Session in Worldspan.\r\n{ex.Message}");
            }
        }

        public string CloseSession(string conversationID)
        {
            CoreLib.SendTrace(ttProviderSystems.UserID, "ttWorldspanAdapter", "Close Session", "", ttProviderSystems.LogUUID);
            try
            {
                var body = $"<ns1:ReleaseProviderSession  xmlns:ns1=\"xxs\"><CONTEXT>{conversationID}</CONTEXT></ns1:ReleaseProviderSession>";
                var token = ConversationID.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                var strResponse = Send(body, token[0], ttProviderSystems.LogUUID);

                // Write a procedure that will write a Log file with information on just opened session for SPLUNK
                var trace = new JObject(new JProperty("Provider", ttProviderSystems.Provider), new JProperty("ID", conversationID), new JProperty("Type", "Close"), new JProperty("User", ttProviderSystems.UserID), new JProperty("UUID", ttProviderSystems.LogUUID), new JProperty("TimeStamp", DateTime.Now));
                string argMessage = "Session Manager";
                modCore.AddLog(modCore.LogType.Info, ref argMessage, ttProviderSystems, trace);
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Closing Session Request to Worldspan.\r\n{ex.Message}");
            }
            
        }

        public string CloseSession()
        {
            CoreLib.SendTrace(ttProviderSystems.UserID, "ttWorldspanAdapter", "Close Session", "", ttProviderSystems.LogUUID);
            try
            {
                var body = $"<ns1:ReleaseProviderSession  xmlns:ns1=\"xxs\"><CONTEXT>{ConversationID}</CONTEXT></ns1:ReleaseProviderSession>";
                var token = ConversationID.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                var strResponse = Send(body, token[0], ttProviderSystems.LogUUID);

                // Write a procedure that will write a Log file with information on just opened session for SPLUNK
                var trace = new JObject(new JProperty("Provider", ttProviderSystems.Provider), new JProperty("ID", ConversationID), new JProperty("Type", "Close"), new JProperty("User", ttProviderSystems.UserID), new JProperty("UUID", ttProviderSystems.LogUUID), new JProperty("TimeStamp", DateTime.Now));
                string argMessage = "Session Manager";
                modCore.AddLog(modCore.LogType.Info, ref argMessage, ttProviderSystems, trace);
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Closing Session Request to Worldspan.\r\n{ex.Message}");
            }
            
        }
                
        private string formatWorldspan(string strDisplay, string id = "")
        {
            string display = "";
            strDisplay = strDisplay.Replace(" & ", " and ");

            var lstLines = strDisplay.Contains(Environment.NewLine)
                ? strDisplay.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList()
                : Regex.Split(strDisplay, "(?<=^(.{64})+)").ToList();

            lstLines.ForEach(l => display += $"<Line>{l.Replace("<", "&lt;").Replace(">", "&gt;").Trim()}</Line>");

            return string.IsNullOrEmpty(id) ? $"<Screen>{display}</Screen>" : $"<Screen><Line>{id}</Line>{display}</Screen>";
        }
    }
}