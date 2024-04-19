using System;
using System.Xml;
using TripXMLMain;

namespace Sabre
{
    public class OtherServices : SabreBase
    {
        public OtherServices()
        {
            ConversationID = "";
            Request = "";
        }

        public string CreateSession()
        {
            string response;

            // *******************************************************************************
            // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
            // ******************************************************************************* 

            try
            {
                SabreAdapter ttSA = SetAdapter();
                SetConversationID(ttSA);
                ConversationID = ConversationID.Replace("<", "&lt;").Replace(">", "&gt;");

                // Build Response.
                response = "<SessionCreateRS Version='1.001'><Success/>";
                response += $"<ConversationID>{ConversationID}</ConversationID></SessionCreateRS>";
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return response;
        }

        public string CloseSession()
        {
            string response;

            // *********************
            // Get ConversationID *
            // *********************
            try
            {
                SabreAdapter ttSA = SetAdapter();
                SetConversationID(ttSA);

                // ****************************
                // Close Session with Token  *
                // ****************************                
                ttSA.CloseSession(ConversationID);
                ConversationID = string.Empty;

                response = "<SessionCloseRS Version='1.001'><Success/></SessionCloseRS>";

            }
            catch (Exception ex)
            {
                response = modCore.FormatErrorMessage(modCore.ttServices.CloseSession, ex.Message, ProviderSystems);
            }

            return response;
        }

        public string ShowMileage()
        {
            string response;

            try
            {
                // *****************************************************************
                // Transform OTA ShowMileage Request into Native Sabre Request     *
                // ***************************************************************** 
                bool inSession = false;
                string request = SetRequest("Sabre_ShowMileageRQ.xsl");
                if (string.IsNullOrEmpty(request))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();

                if (request.Contains("HostCommand"))
                {                 
                    // *******************************************************************************
                    // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                    // ******************************************************************************* 
                    response = ttSA.SendMessage(request, "SabreCommand", "SabreCommandLLSRQ", ConversationID);
                    response = response.Replace("\r\n", "&#xA;").Replace("</SabreCommandLLSRS>", "<ConversationID /></SabreCommandLLSRS>");
                    inSession = true; //This needed in order not to try to close session when it's already closed
                }
                else
                {
                    inSession = SetConversationID(ttSA);
                    // *******************************************************************************
                    // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                    // ******************************************************************************* 
                    response = ttSA.SendMessage(request, "Mileage", "MileageLLSRQ", ConversationID);
                }
                // *****************************************************************
                // Transform Native Sabre ShowMileage Response into OTA Response   *
                // ***************************************************************** 

                response = FinalizeResponse(response, ttSA, inSession, "</OTA_ShowMileageRS>", $"{Version}Sabre_ShowMileageRS.xsl");
            }
            catch (Exception ex)
            {
                response = modCore.FormatErrorMessage(modCore.ttServices.ShowMileage, ex.Message, ProviderSystems);
            }

            return response;
        }
                
        public string Cryptic()
        {
            string response;
            try
            {
                string cdataToken = "";
               
                // *****************************************************************
                // Transform OTA Cryptic Request into Native Sabre Request     *
                // ***************************************************************** 
                string request = SetRequest("Sabre_CrypticRQ.xsl");
                if (string.IsNullOrEmpty(request))
                    throw new Exception("Transformation produced empty xml.");

                var oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(Request);
                var oRoot = oReqDoc.DocumentElement;

                if (oRoot.SelectSingleNode("Entry") != null)
                    cdataToken = oRoot.SelectSingleNode("Entry")?.InnerText;            

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                if (string.IsNullOrEmpty(ConversationID))
                    ConversationID = ConversationID.Replace("&lt;", "<").Replace("&gt;", ">");

                if (string.IsNullOrEmpty(cdataToken))
                    cdataToken = cdataToken?.Replace("&lt;", "<").Replace("&gt;", ">");

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                response = ttSA.SendMessage(request, "SabreCommand", "SabreCommandLLSRQ", ConversationID, cdataToken);
                
                // *****************************************************************
                // Transform Native Sabre Cryptic Response into OTA Response   *
                // ***************************************************************** 
                response = FinalizeResponse(response, ttSA, inSession, "</SabreCommandLLSRS>", $"{Version}Sabre_CrypticRS.xsl");
                
            }
            catch (Exception ex)
            {
                response = modCore.FormatErrorMessage(modCore.ttServices.Cryptic, ex.Message, ProviderSystems);
            }

            return response;
        }

        public string SalesReport()
        {

            string response;
            try
            {
                string cdataToken = string.Empty;

                string request = SetRequest("Sabre_SalesReportRQ.xsl");
                if (string.IsNullOrEmpty(request))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                response = ttSA.SendMessage(request, "SabreCommand", "DailySalesReportLLSRQ", ConversationID, cdataToken);

                // *****************************************************************
                // Transform Native Sabre Cryptic Response into OTA Response   *
                // ***************************************************************** 
                response = FinalizeResponse(response, ttSA, inSession, "</DailySalesReportLLSRQ>", $"{Version}Sabre_SalesReportRS.xsl");
            }
            catch (Exception ex)
            {
                response = modCore.FormatErrorMessage(modCore.ttServices.PNRReprice, ex.Message, ProviderSystems);
            }

            return response;
        }

        public string CreditCardValid()
        {
            string response;

            try
            {
                // *****************************************************************
                // Transform OTA CCValid Request into Native Sabre Request     *
                // ***************************************************************** 
                string request = SetRequest("Sabre_CCValidRQ.xsl");
                if (string.IsNullOrEmpty(request))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                response = ttSA.SendMessage(request, "Air", "SabreCommandLLSRQ", ConversationID);

                // *****************************************************************
                // Transform Native Sabre CCValid Response into OTA Response   *
                // ***************************************************************** 
                response = FinalizeResponse(response, ttSA, inSession, "</CCValidRS>", $"{Version}Sabre_CCValidRS.xsl");
            }
            catch (Exception ex)
            {
                response = modCore.FormatErrorMessage(modCore.ttServices.CCValid, ex.Message, ProviderSystems);
            }

            return response;
        }

        public string CurrencyConvertion()
        {
            string response;

            try
            {
                // *****************************************************************
                // Transform OTA CurConv Request into Native Sabre Request     *
                // ***************************************************************** 
                string request = SetRequest("Sabre_CurConvRQ.xsl");
                if (string.IsNullOrEmpty(request))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                ttSA = new SabreAdapter(ProviderSystems);
                response = ttSA.SendMessage(request, "Air", "SabreCommandLLSRQ", ConversationID);

                // *****************************************************************
                // Transform Native Sabre CurConv Response into OTA Response   *
                // ***************************************************************** 
                response = FinalizeResponse(response, ttSA, inSession, "</CurConvRS>", $"{Version}Sabre_CurConvRS.xsl");                
            }
            catch (Exception ex)
            {
                response = modCore.FormatErrorMessage(modCore.ttServices.CurConv, ex.Message, ProviderSystems);
            }


            return response;
        }

        public string TimeDifference()
        {
            string response;

            try
            {
                // *****************************************************************
                // Transform OTA TimeDiff Request into Native Sabre Request     *
                // ***************************************************************** 
                string request = SetRequest("Sabre_TimeDiffRQ.xsl");
                if (string.IsNullOrEmpty(request))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                response = ttSA.SendMessage(request, "Air", "SabreCommandLLSRQ", ConversationID);

                // *****************************************************************
                // Transform Native Sabre TimeDiff Response into OTA Response   *
                // ***************************************************************** 
                response = FinalizeResponse(response, ttSA, inSession, "</TimeDiffRS>", $"{Version}Sabre_TimeDiffRS.xsl");                
            }
            catch (Exception ex)
            {
                response = modCore.FormatErrorMessage(modCore.ttServices.TimeDiff, ex.Message, ProviderSystems);
            }

            return response;
        }

        public string Native()
        {
            string response;

            // *********************
            // Get ConversationID *
            // *********************

            try
            {
                var oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(Request);
                var oRoot = oReqDoc.DocumentElement;
                
                if (oRoot.SelectSingleNode("Native") == null)
                    throw new Exception("Native Message is missing in the Request.");
                
                string request = oRoot.SelectSingleNode("Native")?.InnerXml;

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);
                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                response = ttSA.SendNativeMessage(request, ConversationID);

                // ********************
                // Build Response    *
                // ********************
                response = $"<NativeRS><Success/><Response>{response.Replace("<", "&lt;").Replace(">", "&gt;")}</Response></NativeRS>";
                response = FinalizeResponse(response, ttSA, inSession, "</NativeRS>");                
            }
            catch (Exception ex)
            {
                response = modCore.FormatErrorMessage(modCore.ttServices.Native, ex.Message, ProviderSystems);
            }

            return response;
        }

        private string FinalizeResponse(string response, SabreAdapter ttSA, bool inSession, string tagToReplace, string stylesheet = "")
        {
            try
            {
                if (inSession)
                    response = response.Replace(tagToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{ tagToReplace}");

                if (!string.IsNullOrEmpty(stylesheet))
                    response = CoreLib.TransformXML(response, XslPath, stylesheet);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
            }
            finally
            {
                if (!inSession)
                {
                    ttSA.CloseSession(ConversationID);
                    ConversationID = string.Empty;
                }
            }

            return response;
        }
    }
}