using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using TripXMLMain;

namespace Travelport
{
    public class TravelPortWSAdapter
    {
        //private StringBuilder sb = new StringBuilder();

        private modCore.TripXMLProviderSystems ttProviderSystems;

        public string TracerID { get; set; }

        public TravelPortWSAdapter(modCore.TripXMLProviderSystems providerSystems)
        {
            ttProviderSystems = providerSystems;
        }

        public TravelPortWSAdapter(modCore.TripXMLProviderSystems providerSystems, string version)
        {
            var oDa = new cDA("ConnectionString");

            ttProviderSystems = providerSystems;
            ttProviderSystems = oDa.SetPCCBlock(ttProviderSystems);
            
            oDa.Dispose();
        }

        private string ComposeHeader()
        {
            string strHeader = "";
            return strHeader;
        }

        private string Send(string message, enRequestType strEndPointService)
        {
            try
            {
                var oHttpWebClient = new ttHttpWebClient
                {
                    TracerID = TracerID,
                    ServiceName = strEndPointService.ToString(),
                    HttpMethod = "POST",
                    Header = "",
                    Body = message
                };

                CoreLib.SendTrace(ttProviderSystems.UserID, "TravelportWSAdapter", "Request", message, ttProviderSystems.LogUUID);
                
                DateTime requesttime = DateTime.Now;
                var strResponse = oHttpWebClient.SendHttpRequest(message,ttProviderSystems) ?? "";
                DateTime responsetime = DateTime.Now;

                CoreLib.SendTrace(ttProviderSystems.UserID, "TravelportWSAdapter", "Response", strResponse, ttProviderSystems.LogUUID);
                
                //if (ttProviderSystems.AddLog)
                //    addSoapLog(log + Environment.NewLine + strResponse, requesttime, responsetime, ttProviderSystems.PCC, ttProviderSystems.UserID);

                return strResponse;
            }
            catch (Exception ex)
            {
                //addLog("<EXOR><M>" + Message + "</M><SendSoap2/>", ttProviderSystems.PCC);
                throw new Exception(ex.Message);
            }
        }

        public string SendMessage(string message, enRequestType еndPointService)
        {
            try
            {
                //:TODO If no header used at all need to remove later.
                var Header = ComposeHeader();
                var body = $"{Header}{message}";

                var soapResponse = Send(message, еndPointService);

                //}
                // *************************
                //  Get Response From Soap *
                // *************************
                string strResponse = soapResponse.StartsWith("<Error") 
                        ? soapResponse 
                        : GetResponseFromSoap(soapResponse.Replace("", ""), еndPointService);
                
                
                return strResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetResponseFromSoap(string strResponse, enRequestType requestType)
        {   
            try
            {
                if (string.IsNullOrEmpty(strResponse))
                    throw new Exception("No response from Travelport");

                // ********************************
                //  Check for Errors in Response  *
                // ********************************
                if (strResponse.Contains("<SOAP-ENV:Fault") || strResponse.Contains("<SOAP:Fault"))
                {
                    var xmlReader = XmlReader.Create(new StringReader(strResponse));
                    XDocument doc = XDocument.Load(xmlReader);                    
                    var xRoot = RemoveAllNamespaces(doc.Root);
                    var err = xRoot.XPathSelectElement("//detail/ErrorInfo/Description");
                    if(err == null)
                        err = xRoot.XPathSelectElement("//faultstring");
                    throw new Exception(err.Value);
                }

                // *************************
                //  Get Response From Soap *
                // *************************
                XmlDocument oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                XmlElement oRoot = oDoc.DocumentElement;
                //  Get The Body
                XmlNode oNode = oRoot.LastChild;

                if (requestType == enRequestType.CreateSession)
                {
                    //  Get The Header
                    if (!strResponse.Contains("<statusCode>P</statusCode>"))
                        throw new Exception("Cannot open session with Travelport");

                    oNode = oRoot.FirstChild;
                    oNode = oNode.LastChild;
                    strResponse = oNode.InnerText;
                }
                //else
                //{
                //    //Get The Body
                //    oNode = oRoot.LastChild;
                //    strResponse = oNode.InnerXml;
                //    strResponse = strResponse.Replace(" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"", "").Replace("soap:", "");
                //    CoreLib.SendTrace(ttProviderSystems.UserID, "TravelportWSAdapter", "Soap body response", strResponse, ttProviderSystems.LogUUID);
                //}

                return strResponse;
            }
            catch (Exception ex)
            {
                strResponse = $"<Errors><Error>{ex.Message}</Error></Errors>";
                return strResponse;
            }
        }

        //public string SendCrypticMessage(string Message, string SessionToken)
        //{
        //    string strResponse = "";
        //    bool CloseThisSession = false;

        //    try
        //    {
        //        if (SessionToken.Length == 0)
        //        {
        //            CloseThisSession = true;
        //            SessionToken = CreateCrypticSession();
        //        }
        //        else
        //        {
        //            CloseThisSession = false;
        //        }

        //        CoreLib.SendTrace(sb.Append(mstrUserID).Append(" - ").Append(mstrProfile).ToString(), "ttTravelPortAdapter", "Send to TravelPort", Message);
        //        sb.Remove(0, sb.Length);

        //        strResponse = this.SubmitTerminalTransaction(SessionToken, Message);

        //        CoreLib.SendTrace(sb.Append(mstrUserID).Append(" - ").Append(mstrProfile).ToString(), "ttTravelPortAdapter", "Receive from Travel Port", strResponse);
        //        sb.Remove(0, sb.Length);

        //        if (CloseThisSession)
        //        {
        //            CloseCrypticSession(SessionToken);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string errText = ex.Message;

        //        CoreLib.SendTrace(sb.Append(mstrUserID).Append(" - ").Append(mstrProfile).ToString(), "ttTravelPortAdapter", "TravelPort exception error", errText);
        //        sb.Remove(0, sb.Length);
        //        //TODO: We need to handle the error responses here. 
        //        throw new Exception(errText, ex);
        //    }

        //    return strResponse;
        //}

        #region Terminal Session
        public string CreateTerminalSession(string branch, string host)
        {
            try
            {
                var ttTP = new TravelPortWSAdapter(ttProviderSystems);
                var strResponse = ttTP.SendMessage($"<ter:CreateTerminalSessionReq xmlns:ter='http://www.travelport.com/schema/terminal_v33_0' xmlns:com='http://www.travelport.com/schema/common_v33_0' TargetBranch=\"{branch}\" Host=\"{host}\"><com:BillingPointOfSaleInfo OriginApplication=\"UAPI\"/></ter:CreateTerminalSessionReq>", TravelPortWSAdapter.enRequestType.TerminalService);

                if (strResponse.Length > 36)
                {
                    //Questionable code!!!!!
                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(strResponse);
                    if (strResponse.Contains("HostToken"))                    
                    {
                        var oRoot = oDoc.DocumentElement;
                        var oNode = oRoot.FirstChild;
                        strResponse = oNode.InnerText;
                    }
                }

                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Session was not Created.\r\n{ex.Message}");
            }
            
        }

        public string CloseTerminalSession(string branch, string host, string strTokenid)
        {
            try
            {
                var request = $"<ter:EndTerminalSessionReq xmlns:ter=\"http://www.travelport.com/schema/terminal_v33_0\" xmlns:com=\"http://www.travelport.com/schema/common_v33_0\" TargetBranch=\"{branch}\">" +
                                 "<com:BillingPointOfSaleInfo OriginApplication=\"UAPI\"/>" + 
                                $"<com:HostToken Host=\"{host}\">{strTokenid}</com:HostToken>" +
                              "</ter:EndTerminalSessionReq>";
                                
                var ttTP = new TravelPortWSAdapter(ttProviderSystems);
                var response = ttTP.SendMessage(request, TravelPortWSAdapter.enRequestType.TerminalService);

                response = response.Contains("Terminal End Session Successful") 
                    ? "" : response; //|| response.Contains("Could not locate Session Token") 

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Session was not Closed.\r\n{ex.Message}");
            }            
        }

        public string SubmitTerminalTransaction(string message, string branch, string host, string sessionTokenID)
        {
            try
            {
                //TODO: Some values are hard coded for now. later we have to configure and take it from files or use constants.
                var request = $"<ter:TerminalReq  xmlns:ter='http://www.travelport.com/schema/terminal_v33_0' xmlns:com='http://www.travelport.com/schema/common_v33_0' TargetBranch=\"{branch}\"><com:HostToken Host=\"{host}\">{sessionTokenID}</com:HostToken><ter:TerminalCommand>{message}</ter:TerminalCommand></ter:TerminalReq>";
                var ttTP = new TravelPortWSAdapter(ttProviderSystems);
                var strResponse = ttTP.SendMessage(request, enRequestType.TerminalService);

                //TODO: We have to extract the reply and send it back.
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Session was not Created.\r\n{ex.Message}");
            }
        }

        public static XElement RemoveAllNamespaces(XElement element)
        {
            /*
             var nsmngr = new XmlNamespaceManager(doc.CreateReader().NameTable);

                    foreach (var attribute in doc.Descendants().Attributes().Where(a => a.IsNamespaceDeclaration))
                    {
                        nsmngr.AddNamespace(attribute.Name.LocalName, attribute.Value);
                    }
             */

            return new XElement(element.Name.LocalName,
                                element.HasAttributes ? element.Attributes().Select(a => new XAttribute(a.Name.LocalName, a.Value)) : null,
                                element.HasElements ? element.Elements().Select(e => RemoveAllNamespaces(e)) : null,
                                element.Value);
        }
        #endregion

        public enum enRequestType
        {
            CreateSession = 1,
            CloseSession = 2,
            Message = 3,
            AirService = 4,
            TerminalService = 5,
            GdsQueueService = 6,
            SystemService = 7,
            UniversalRecordService = 8
        }
    }
}
