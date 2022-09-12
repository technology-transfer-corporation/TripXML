using System;
using System.Xml;
using TripXMLMain;

namespace Worldspan
{
    public class OtherServices : WorldspanBase
    {
        public OtherServices()
        {
            Request = string.Empty;
            Version = string.Empty;
        }

        public string CreateSession()
        {
            try
            {
                var tripXmlProviderSystems = ProviderSystems;
                tripXmlProviderSystems.Profile = ProviderSystems.ProfileCryptic;
                WorldspanAdapter ttWA = SetAdapter(tripXmlProviderSystems); //= new WorldspanAdapter(ProviderSystems);
                // Create Session and Get Sesson SessionID
                ttWA.CreateSession();
                // Build Response.
                var strResponse = "<SessionCreateRS Version='1.001'><Success/>" + 
                                  $"<ConversationID>{ttWA.ConversationID}</ConversationID></SessionCreateRS>";
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Communicating with the TravelTalk Adapter. Session was not Created.\r\n{ex.Message}");
            }
        }

        public string CloseSession()
        {
            string sessionID = string.Empty;

            // *********************
            // Get ConversationID *
            // *********************
            try
            {
                var oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(Request);
                var oRoot = oReqDoc.DocumentElement;

                if (oRoot != null) sessionID = oRoot.SelectSingleNode("POS/TPA_Extensions/ConversationID")?.InnerText;
            }
            catch (Exception ex)
            {
                throw new Exception($"ConversationID is missing in the Request.\r\n{ex.Message}");
            }

            if (string.IsNullOrEmpty(sessionID))
                throw new Exception("ConversationID is missing in the Request.");

            // ****************************
            // Close Session with SessionID  *
            // ****************************
            try
            {
                var tripXmlProviderSystems = ProviderSystems;
                tripXmlProviderSystems.Profile = ProviderSystems.ProfileCryptic;
                var ttWA = new WorldspanAdapter(tripXmlProviderSystems) { ConversationID = sessionID };
                var strResponse = ttWA.CloseSession();
                if (!strResponse.Contains("ERROR"))
                {
                    strResponse = "<SessionCloseRS Version=\"1.001\"><Success/></SessionCloseRS>";
                }
                else
                {
                    var xmlResp = new XmlDocument();
                    xmlResp.LoadXml(strResponse);
                    strResponse = $"<SessionCloseRS Version='1.001'>{xmlResp.SelectSingleNode("XXW/ERROR/ERROR/TEXT")?.InnerText}</SessionCloseRS>";
                }

                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Communicating with the TravelTalk Adapter. Session was not Closed.\r\n{ex.Message}");
            }
        }

        public string Cryptic()
        {
            string strResponse;
            
            // *********************
            // Get ConversationID *
            // *********************
            var tripXmlProviderSystems = ProviderSystems;
            try
            {
                string strRequest = SetRequest("Worldspan_CrypticRQ.xsl");
                
                // *********************************************************
                // Transform Cryptic Request into Native Worldspan Request  *
                // *********************************************************
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // ********************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response   *
                // ******************************************************************************** 

                
                tripXmlProviderSystems.Profile = ProviderSystems.ProfileCryptic;
                var ttWA = SetAdapter(tripXmlProviderSystems);
                bool inSession = SetConversationID(ttWA);

                strResponse = ttWA.SendMessage(strRequest);                

                // ********************************************************************************
                // parse the response and create screen with lines                               *
                // ******************************************************************************** 
                strResponse = strResponse.Substring(strResponse.IndexOf("<TextData>", StringComparison.Ordinal) + 10,
                    strResponse.IndexOf("</TextData>", StringComparison.Ordinal) - strResponse.IndexOf("<TextData>", StringComparison.Ordinal) - 10);
                string strScreen = strResponse.Replace("\r", "\r\n").Replace("\n", "\r\n");
                strScreen = FormatWorldspan(strScreen);

                // ********************************************************************************
                // Build and Transform Native Worldspan Cryptic Response into XML Cryptic Response *
                // ********************************************************************************
                try
                {
                    strResponse = $"<CrypticRS><Response>{strResponse}</Response>{strScreen}</CrypticRS>";
                    var strToReplace = "</CrypticRS>";
                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID>{ConversationID}</ConversationID>{strToReplace}");
                    
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_CrypticRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttWA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Cryptic, ex.Message, tripXmlProviderSystems);
            }
            return strResponse;
        }

        public string Native()
        {
            string strResponse;

            // ************************************************
            // Get ConversationID If Any and Native Request  *
            // ************************************************
            try
            {
                string strRequest = SetRequest("");
                // *******************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response  *
                // ******************************************************************************* 
                
                var ttWA = SetAdapter(ProviderSystems);
                bool inSession = SetConversationID(ttWA);
                strResponse = ttWA.SendMessage(strRequest);

                // *******************
                // Build Response   *
                // *******************
                try
                {
                    strResponse = $"<NativeRS><Success/><Response>{strResponse.Replace("<", "&lt;").Replace(">", "&gt;")}</Response></NativeRS>";
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error in Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttWA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Native, ex.Message, ProviderSystems);
            }

            return strResponse;
        }
    }
}