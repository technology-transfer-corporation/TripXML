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
            string strResponse;

            // *******************************************************************************
            // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
            // ******************************************************************************* 

            try
            {
                SabreAdapter ttSA = SetAdapter();
                SetConversationID(ttSA);
                ConversationID = ConversationID.Replace("<", "&lt;").Replace(">", "&gt;");

                // Build Response.
                strResponse = "<SessionCreateRS Version='1.001'><Success/>";
                strResponse += $"<ConversationID>{ConversationID}</ConversationID></SessionCreateRS>";
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return strResponse;
        }

        public string CloseSession()
        {
            string strResponse;

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
                try
                {
                    ttSA.CloseSession(ConversationID);
                    ConversationID = string.Empty;

                    strResponse = "<SessionCloseRS Version='1.001'><Success/></SessionCloseRS>";
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CloseSession, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string ShowMileage()
        {
            string strResponse;

            try
            {
                
                // *****************************************************************
                // Transform OTA ShowMileage Request into Native Sabre Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Sabre_ShowMileageRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);
                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttSA.SendMessage(strRequest, "Mileage", "MileageLLSRQ", ConversationID);

                // *****************************************************************
                // Transform Native Sabre ShowMileage Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var strToReplace = "</ShowMileageRS>";

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_ShowMileageRS.xsl");

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{ strToReplace}");
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
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.ShowMileage, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string Cryptic()
        {
            string strResponse;
            try
            {
                string cdataToken = "";
               
                // *****************************************************************
                // Transform OTA Cryptic Request into Native Sabre Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Sabre_CrypticRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
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
                strResponse = ttSA.SendMessage(strRequest, "SabreCommand", "SabreCommandLLSRQ", ConversationID, cdataToken);

                // *****************************************************************
                // Transform Native Sabre Cryptic Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var strToReplace = "</SabreCommandLLSRS>";

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{ strToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_CrypticRS.xsl");
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
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Cryptic, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string SalesReport()
        {

            string strResponse;
            try
            {
                string cdataToken = string.Empty;

                string strRequest = SetRequest("Sabre_SalesReportRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttSA.SendMessage(strRequest, "SabreCommand", "DailySalesReportLLSRQ", ConversationID, cdataToken);

                // *****************************************************************
                // Transform Native Sabre Cryptic Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    if (inSession)
                        strResponse = strResponse.Replace("</DailySalesReportLLSRQ>", $"<ConversationID>{ConversationID}</ConversationID></DailySalesReportLLSRQ>");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_SalesReportRS.xsl");
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
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRReprice, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string CreditCardValid()
        {
            string strResponse;

            try
            {
                // *****************************************************************
                // Transform OTA CCValid Request into Native Sabre Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Sabre_CCValidRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttSA.SendMessage(strRequest, "Air", "SabreCommandLLSRQ", ConversationID);

                // *****************************************************************
                // Transform Native Sabre CCValid Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var strToReplace = "</CCValidRS>";

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_CCValidRS.xsl");

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{ strToReplace}");
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
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CCValid, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string CurrencyConvertion()
        {
            string strResponse;

            try
            {
                // *****************************************************************
                // Transform OTA CurConv Request into Native Sabre Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Sabre_CurConvRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                ttSA = new SabreAdapter(ProviderSystems);
                strResponse = ttSA.SendMessage(strRequest, "Air", "SabreCommandLLSRQ", ConversationID);

                // *****************************************************************
                // Transform Native Sabre CurConv Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var strToReplace = "</CurConvRS>";
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_CurConvRS.xsl");

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{ strToReplace}");
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
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CurConv, ex.Message, ProviderSystems);
            }


            return strResponse;
        }

        public string TimeDifference()
        {
            string strResponse;

            try
            {
                // *****************************************************************
                // Transform OTA TimeDiff Request into Native Sabre Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Sabre_TimeDiffRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttSA.SendMessage(strRequest, "Air", "SabreCommandLLSRQ", ConversationID);

                // *****************************************************************
                // Transform Native Sabre TimeDiff Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var strToReplace = "</TimeDiffRS>";
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_TimeDiffRS.xsl");

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{strToReplace}");
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
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.TimeDiff, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string Native()
        {
            string strResponse;

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
                
                string strRequest = oRoot.SelectSingleNode("Native")?.InnerXml;

                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);
                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttSA.SendNativeMessage(strRequest, ConversationID);

                // ********************
                // Build Response    *
                // ********************
                try
                {
                    var strToReplace = "</NativeRS>";

                    strResponse = $"<NativeRS><Success/><Response>{strResponse.Replace("<", "&lt;").Replace(">", "&gt;")}</Response></NativeRS>";

                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID><![CDATA[{ConversationID.Replace("<", "&lt;").Replace(">", "&gt;")}]]></ConversationID>{ strToReplace}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error in Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttSA.CloseSession(ConversationID);
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