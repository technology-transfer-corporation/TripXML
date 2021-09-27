using System;
using System.Text;
using System.Xml;
using TripXMLMain;

namespace TravelPort
{
    public class OtherServices
    {
        public modCore.TripXMLProviderSystems ttProviderSystems;
        private readonly string mstrVersion = "";
        private readonly string mstrXslPath = "";

        public string Request { get; set; } = "";

        public string Native()
        {
            string ConversationID = "";
            string strRequest;
            string strResponse = "";
            try
            {
                strRequest = Request;
                XmlDocument oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(strRequest);
                XmlElement oRoot = oReqDoc.DocumentElement;
                if (oRoot.SelectSingleNode("Native") == null)
                {
                    throw new Exception("Native Message is missing in the Request.");
                }
                else
                {
                    strRequest = oRoot.SelectSingleNode("Native").InnerXml;
                }
                if (!(oRoot.SelectSingleNode("POS/TPA_Extensions/ConversationID") == null))
                {
                    ConversationID = oRoot.SelectSingleNode("POS/TPA_Extensions/ConversationID").InnerText;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Invalid Request.\r\n{ex.Message}");
            }
            // *******************************************************************************
            //  Send Native Request to the Amadeus Adapter and Getting Native Response  *
            // ******************************************************************************* 
            try
            {
                TravelPortWSAdapter ttTP = new TravelPortWSAdapter(ttProviderSystems);

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
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // ********************
            //  Build Response    *
            // ********************
            try
            {
                //  Insert ConversationID
                if (!string.IsNullOrEmpty(ConversationID))
                    ConversationID = $"<ConversationID>{ConversationID}</ConversationID>";

                strResponse = $"<NativeRS><Success/><Response>{strResponse.Replace("<", "&lt;").Replace(">", "&gt;")}</Response>{ConversationID}</NativeRS>";
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Native Response.\r\n{ex.Message}");
            }
            
        }

        public string Cryptic()
        {
            string strScreen = "";
            string strRequest;
            string ConversationID;
            string strBranch;
            string strHost = "1G";

            //*********************
            // Get ConversationID *
            //*********************

            try
            {
                XmlDocument oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(Request);
                XmlElement oRoot = oReqDoc.DocumentElement;

                ConversationID = string.IsNullOrEmpty(oRoot.SelectSingleNode("POS/TPA_Extensions/ConversationID").InnerText)
                    ? ""
                    : oRoot.SelectSingleNode("POS/TPA_Extensions/ConversationID").InnerText;

                strBranch = oRoot.SelectSingleNode("POS/Source/@PseudoCityCode").InnerText;

                if (oRoot.HasAttribute("Target"))
                {
                    switch (oRoot.Attributes["Target"].Value)
                    {
                        case "WSP":
                            strHost = "1P";
                            break;
                        case "GAL":
                            strHost = "1G";
                            break;
                        default:
                            strHost = "1V";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Loading Travel Port Native Request XML Document.\r\n{ex.Message}");
            }

            //*********************************************************
            // Transform Cryptic Request into Native TravelPort Request  *
            //*********************************************************
            string strResponse;
            string Token;

            try
            {
                strRequest = CoreLib.TransformXML(Request, mstrXslPath, $"{mstrVersion}Travelport_CrypticRQ.xsl");

                if (string.IsNullOrEmpty(strRequest.Trim()))
                    throw new Exception("Transformation produced empty xml.");

                //********************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response   *
                //******************************************************************************** 
                TravelPortWSAdapter ttTP = new TravelPortWSAdapter(ttProviderSystems);

                if (string.IsNullOrEmpty(ConversationID))
                    Token = ttTP.CreateTerminalSession(strBranch, strHost);
                else
                    Token = ConversationID;

                strResponse = ttTP.SubmitTerminalTransaction(strRequest, strBranch, strHost, Token);

                if (string.IsNullOrEmpty(ConversationID))
                    Token = ttTP.CloseTerminalSession(strBranch, strHost, Token);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming OTA Request.\r\n{ex.Message}");
            }

            //********************************************************************************
            // Build and Transform Native TravelPort Cryptic Response into XML Cryptic Response *
            //********************************************************************************
            try
            {
                var PreResponse = $"<CrypticRS><Response>{strResponse}</Response>{strScreen}<ConversationID>{Token}</ConversationID></CrypticRS>";
                strResponse = CoreLib.TransformXML(PreResponse, mstrXslPath, $"{mstrVersion}Travelport_CrypticRS.xsl");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
            }
           
            return strResponse;
        }

        public string CreateSession()
        {
            string branchID = "";
            string strHost = "1G"; //WSP
          
            try
            {
                XmlDocument oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(Request);
                XmlElement oRoot = oReqDoc.DocumentElement;
                
                if (!string.IsNullOrEmpty(oRoot.SelectSingleNode("POS/Source/@PseudoCityCode").InnerText))
                    branchID = oRoot.SelectSingleNode("POS/Source/@PseudoCityCode").InnerText;

                if (oRoot.HasAttribute("Target"))
                {
                    switch (oRoot.Attributes["Target"].Value)
                    {
                        case "WSP":
                            strHost = "1P";
                            break;
                        case "GAL":
                            strHost = "1G";
                            break;
                        default:
                            strHost = "1V";
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
                TravelPortWSAdapter ttGA = new TravelPortWSAdapter(ttProviderSystems);
                // Create Session and Get Sesson Token
                string Token = ttGA.CreateTerminalSession(branchID, strHost);
                
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
            string Host = "1G";
            string Token;
            //*********************
            // Get ConversationID *
            //*********************
            string branchID;

            try
            {
                XmlDocument oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(Request);
                XmlElement oRoot = oReqDoc.DocumentElement;

                Token = oRoot.SelectSingleNode("POS/TPA_Extensions/ConversationID").InnerText;
                branchID = oRoot.SelectSingleNode("POS/Source/@PseudoCityCode").InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception($"ConversationID is missing in the Request.\r\n{ex.Message}");
            }

            if (string.IsNullOrEmpty(Token))
                throw new Exception("ConversationID is missing in the Request.");

            //****************************
            // Close Session with Token  *
            //****************************
            try
            {
                TravelPortWSAdapter ttGA = new TravelPortWSAdapter(ttProviderSystems);
                string strResponse = ttGA.CloseTerminalSession(branchID, Host,Token);
                
                strResponse = string.IsNullOrEmpty(strResponse) 
                    ?  "<SessionCloseRS Version=\"1.001\"><Success/></SessionCloseRS>"
                    : $"<SessionCloseRS Version=\"1.001\">{strResponse}</SessionCloseRS>";

                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Session was not Closed.\r\n{ex.Message}");
            }
        }
    }
}
