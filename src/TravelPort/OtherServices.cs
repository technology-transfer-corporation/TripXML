using System;
using System.Text;
using System.Xml;
using TripXMLMain;

namespace Travelport
{
    public class OtherServices : TravelportBase
    {
        public string Native()
        {
            string strResponse = "";
            try
            {
                string ConversationID = "";
                string strRequest = Request;
                XmlDocument oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(strRequest);
                XmlElement oRoot = oReqDoc.DocumentElement;

                if (oRoot.SelectSingleNode("Native") == null)
                    throw new Exception("Native Message is missing in the Request.");
                
                strRequest = oRoot.SelectSingleNode("Native").InnerXml;            

            // *******************************************************************************
            //  Send Native Request to the Amadeus Adapter and Getting Native Response  *
            // ******************************************************************************* 
            var ttProviderSystems = ProviderSystems;
            TravelPortWSAdapter ttTP = SetAdapter(ttProviderSystems);
            bool inSession = SetConversationID(ttTP);

                if (strRequest.Contains("AvailabilitySearchReq") || strRequest.Contains("AirPriceReq"))
                strResponse = ttTP.SendMessage(strRequest, TravelPortWSAdapter.enRequestType.AirService);
            else if (strRequest.Contains("CreateTerminalSessionReq") || strRequest.Contains("EndTerminalSessionReq") || strRequest.Contains("TerminalReq"))
                strResponse = ttTP.SendMessage(strRequest, TravelPortWSAdapter.enRequestType.TerminalService);
            else if (strRequest.Contains("GdsQueueCountReq") || strRequest.Contains("GdsEnterQueueReq"))
                strResponse = ttTP.SendMessage(strRequest, TravelPortWSAdapter.enRequestType.GdsQueueService);
            else if (strRequest.Contains("PingReq"))
                strResponse = ttTP.SendMessage(strRequest, TravelPortWSAdapter.enRequestType.SystemService);
            else if (strRequest.Contains("UniversalRecordRetrieveReq") || strRequest.Contains("UniversalRecordImportReq") || strRequest.Contains("UniversalRecordCancelReq") || strRequest.Contains("UniversalRecordModifyReq") || strRequest.Contains("UniversalRecordSearchReq"))
                strResponse = ttTP.SendMessage(strRequest, TravelPortWSAdapter.enRequestType.UniversalRecordService);

            // ********************
            //  Build Response    *
            // ********************
           
                //  Insert ConversationID
                if (!string.IsNullOrEmpty(ConversationID))
                    ConversationID = $"<ConversationID>{ConversationID}</ConversationID>";

                strResponse = $"<NativeRS><Success/><Response>{strResponse.Replace("<", "&lt;").Replace(">", "&gt;")}</Response>{ConversationID}</NativeRS>";
                return strResponse;
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Native, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string Cryptic()
        {
            string strResponse;

            try
            {
                string screen = string.Empty;
                string strRequest = Request;

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                XmlDocument oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(Request);
                XmlElement oRoot = oReqDoc.DocumentElement;

                if (oRoot.HasAttribute("Target"))
                {
                    switch (oRoot.Attributes["Target"].Value)
                    {
                        case "WSP":
                            host = "1P";
                            break;
                        case "GAL":
                            host = "1G";
                            break;
                        default:
                            host = "1V";
                            break;
                    }
                }

                string conversationID = string.IsNullOrEmpty(oRoot.SelectSingleNode("POS/TPA_Extensions/ConversationID").InnerText)
                    ? ""
                    : oRoot.SelectSingleNode("POS/TPA_Extensions/ConversationID").InnerText;

                branch = oRoot.SelectSingleNode("POS/Source/@PseudoCityCode").InnerText;

                if (oRoot.HasAttribute("Target"))
                {
                    switch (oRoot.Attributes["Target"].Value)
                    {
                        case "WSP":
                            host = "1P";
                            break;
                        case "GAL":
                            host = "1G";
                            break;
                        default:
                            host = "1V";
                            break;
                    }
                }


                //***********************************************************
                // Transform Cryptic Request into Native TravelPort Request *
                //***********************************************************
                strResponse = CoreLib.TransformXML(Request, XslPath, $"{Version}Travelport_CrypticRQ.xsl");

                if (string.IsNullOrEmpty(strRequest.Trim()))
                    throw new Exception("Transformation produced empty xml.");

                //********************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response   *
                //******************************************************************************** 
                TravelPortWSAdapter ttTP = SetAdapter(ProviderSystems);
                string Token = string.IsNullOrEmpty(conversationID) ? ttTP.CreateTerminalSession(branch, host) : conversationID;
                strResponse = ttTP.SubmitTerminalTransaction(strRequest, branch, host, Token);

                if (string.IsNullOrEmpty(conversationID))
                    Token = ttTP.CloseTerminalSession(branch, host, Token);

                //***********************************************************************************
                // Build and Transform Native TravelPort Cryptic Response into XML Cryptic Response *
                //***********************************************************************************
                var PreResponse = $"<CrypticRS><Response>{strResponse}</Response>{screen}<ConversationID>{Token}</ConversationID></CrypticRS>";
                strResponse = CoreLib.TransformXML(PreResponse, XslPath, $"{Version}Travelport_CrypticRS.xsl");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
            }
           
            return strResponse;
        }

        public string CreateSession()
        {          
            try
            {
                XmlDocument oReqDoc = new XmlDocument();
                 oReqDoc.LoadXml(Request);
                XmlElement oRoot = oReqDoc.DocumentElement;

                if (!string.IsNullOrEmpty(oRoot.SelectSingleNode("POS/Source/@PseudoCityCode").InnerText))
                { 
                    pcc = oRoot.SelectSingleNode("POS/Source/@PseudoCityCode").InnerText;
                    branch = ProviderSystems.Profile;
                }

                if (oRoot.HasAttribute("Target"))
                {
                    switch (oRoot.Attributes["Target"].Value)
                    {
                        case "APL":
                            host = "1V";
                            break;
                        case "GAL":
                            host = "1G";
                            break;
                        case "WSP":
                            host = "1P";
                            break;
                        default:
                            host = "1G"; //GAL
                            break;
                    }
                }                
            }
            catch (Exception ex)
            {
                throw new Exception($"Invalid Request.\r\n{ex.Message}");
            }

            try
            {                
                TravelPortWSAdapter ttGA = SetAdapter(ProviderSystems);
                // Create Session and Get Sesson Token
                string Token = ttGA.CreateTerminalSession(branch, host);
                
                // Build Response.
                string strResponse = Token.Length == 36 
                        ? $"<SessionCreateRS Version='1.001'><Success/><ConversationID>{Token}</ConversationID></SessionCreateRS>"
                        : Token;
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Session was not Created.\r\n{ex.Message}");
            }
        }

        public string CloseSession()
        {
            try
            {
                XmlDocument oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(Request);
                XmlElement oRoot = oReqDoc.DocumentElement;

                string Token = oRoot.SelectSingleNode("POS/TPA_Extensions/ConversationID").InnerText;
                string branchID = oRoot.SelectSingleNode("POS/Source/@PseudoCityCode").InnerText;


                if (string.IsNullOrEmpty(Token))
                    throw new Exception("ConversationID is missing in the Request.");

                //****************************
                // Close Session with Token  *
                //****************************
                if (oRoot.HasAttribute("Target"))
                {
                    switch (oRoot.Attributes["Target"].Value)
                    {
                        case "WSP":
                            host = "1P";
                            break;
                        case "GAL":
                            host = "1G";
                            break;
                        default:
                            host = "1V";
                            break;
                    }
                }

                TravelPortWSAdapter ttGA = SetAdapter(ProviderSystems); 
                string strResponse = ttGA.CloseTerminalSession(branchID, host, Token);

                strResponse = string.IsNullOrEmpty(strResponse)
                    ? "<SessionCloseRS Version=\"1.001\"><Success/></SessionCloseRS>"
                    : $"<SessionCloseRS Version=\"1.001\">{strResponse}</SessionCloseRS>";

                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Communicating with the TravelTalk Adapter. Session was not Closed.\r\n{ex.Message}");
            }
        }
    }
}
