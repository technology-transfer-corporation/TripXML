using System;
using System.Xml;
using TripXMLMain;

namespace Galileo
{
    public class OtherServices : GalileoBase
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
                var ttAA = SetAdapter();
                SetConversationID(ttAA);

                //  Build Response.
                string strResponse = $"<SessionCreateRS Version=\'1.001\'><Success/><ConversationID>{ConversationID}</ConversationID></SessionCreateRS>";
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Session was not Created.\r\n{ex.Message}");
            }

        }

        public string CloseSession()
        {

            //  *******************
            //  Close Session    *
            //  *******************
            try
            {
                var ttAA = SetAdapter();
                SetConversationID(ttAA);

                if (string.IsNullOrEmpty(ConversationID))
                    throw new Exception("ConversationID is missing in the Request.");

                ttAA.CloseSession(ConversationID);

                //  Build Response.
                return "<SessionCloseRS Version=\'1.001\'><Success/></SessionCloseRS>";

            }
            catch (Exception ex)
            {
                throw new Exception($"Session was not Closed.\r\n{ex.Message}");
            }
        }

        public string ShowMileage()
        {
            string strResponse;
            try
            {
                string strRequest = SetRequest("Galileo_ShowMileageRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttGA.SendCrypticMessage(strRequest, ConversationID);


                // *****************************************************************
                //  Transform Native Amadeus ShowMileage Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = "</Command_CrypticReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_ShowMileageRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
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
                string strScreen = string.Empty;
                // ************************************************************
                //  Transform OTA Cryptic Request into Native Galileo Request *
                // ************************************************************ 
                string strRequest = SetRequest("Galileo_CrypticRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                var recordLocator = "";
                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 
                try
                {
                    strResponse = ttGA.SendCrypticMessage(strRequest, ConversationID);
                    CoreLib.SendTrace(ProviderSystems.UserID, "Cryptic", "Getting Native Response", strResponse, ProviderSystems.LogUUID);

                    if (strResponse.Contains("|Session|Inactive conversation"))
                        CoreLib.SendTrace(ProviderSystems.UserID, "Cryptic", "Getting Native Second Response", strResponse, ProviderSystems.LogUUID);

                    if (strResponse.Contains("NEED RECEIVED FROM"))
                        throw new Exception("NEED RECEIVED FROM");

                    // ********************************************************************************
                    // parse the response and create screen with lines                               *
                    // ******************************************************************************** 
                    strScreen = strResponse.Replace("\r", "\r\n");
                    strScreen = FormatGalileo(strScreen);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error Transforming Native Initial Response.", ex);
                }

                // ****************************************************************
                //  Transform Native Amadeus Cryptic Response into OTA Response   *
                // **************************************************************** 
                // ********************************************************************************
                // Build and Transform Native Galileo Cryptic Response into XML Cryptic Response *
                // ********************************************************************************
                try
                {
                    strResponse = $"<CrypticRS><Response>{strResponse.Replace(" & ", " and ")}</Response>{strScreen}</CrypticRS>";
                    if (inSession)
                        strResponse = strResponse.Replace("</CrypticRS>", $"<ConversationID>{ConversationID}</ConversationID></CrypticRS>");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CrypticRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error Transforming Native Response", ex);
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
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

        public string CreditCardValid()
        {
            string strResponse;
            try
            {
                string strRequest = SetRequest("Galileo_CCValidRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // *******************************************************************************                 
                strResponse = ttGA.SendMessage(strRequest);

                // ****************************************************************
                //  Transform Native Amadeus CCValid Response into OTA Response   *
                // **************************************************************** 
                try
                {
                    var tagToReplace = strResponse.Contains("</Command_CrypticReply>")
                        ? "</Command_CrypticReply>"
                        : "</CrypticRS>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CCValidRS.xsl", false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
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
                string strRequest = SetRequest("Galileo_CurConvRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttGA.SendMessage(strRequest);

                // ****************************************************************
                //  Transform Native Amadeus CurConv Response into OTA Response   *
                // **************************************************************** 
                try
                {
                    var tagToReplace = strResponse.Contains("</Command_CrypticReply>")
                        ? "</Command_CrypticReply>"
                        : "</CrypticRS>";

                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CurConvRS.xsl", false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
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

                // ****************************************************************
                //  Transform OTA TimeDiff Request into Native Amadeus Request    *
                // **************************************************************** 
                string strRequest = SetRequest("Galileo_TimeDiffRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttGA.SendMessage(strRequest);

                // *****************************************************************
                //  Transform Native Amadeus TimeDiff Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = strResponse.Contains("</Command_CrypticReply>")
                        ? "</Command_CrypticReply>"
                        : "</CrypticRS>";

                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_TimeDiffRS.xsl", false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
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
            string strResponse = "";

            try
            {
                XmlDocument oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(Request);
                XmlElement oRoot = oReqDoc.DocumentElement;
                if (oRoot.SelectSingleNode("Native") == null)
                {
                    throw new Exception("Native Message is missing in the Request.");
                }
                else
                {
                    Request = oRoot.SelectSingleNode("Native").InnerXml;
                }

                // ***************************************************************************
                // Send Native Request to the Galileo Adapter and Getting Native Response   *
                // ***************************************************************************
                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                try
                {
                    strResponse = ttGA.SendMessage(Request, ConversationID);

                    var strSession = "";
                    //  Insert ConversationID
                    if (inSession)
                        strSession = $"<ConversationID>{ConversationID}</ConversationID>";

                    strResponse = $"<NativeRS><Success/><Response>{strResponse.Replace("<", "&lt;").Replace(">", "&gt;")}</Response>{strSession}</NativeRS>";
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error in Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
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

        public string ETicketVerify()
        {
            string strResponse;
            try
            {

                // *****************************************************************
                // Transform OTA ETicketVerify Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_ETicketVerifyRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                XmlElement oRoot;
                try
                {
                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(strRequest);
                    oRoot = oDoc.DocumentElement;

                    // ETicketVerify PNRBFManagement_7_9
                    var oNode = oRoot.SelectSingleNode("PNRBFManagement_7_9[position()=1]");
                    strRequest = oNode.OuterXml;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Loading Transformed Request XML Document. {ex.Message}");
                }

                // Create Session
                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                // *************************************************************************************
                // Send Transformed Request ETicketVerify PNRBFManagement_7_9 to the Galileo Adapter  *
                // ************************************************************************************* 
                strResponse = ttGA.SendMessage(strRequest, ConversationID);


                if (strResponse.IndexOf("TransactionErrorCode") == -1)
                {
                    // ************************************************************************************
                    // Send Transformed Request DocProdETicketCheck_1_0 Tariff to the Galileo Adapter    *
                    // ************************************************************************************ 

                    // DocProdETicketCheck_1_0 Request
                    var oNode = oRoot.SelectSingleNode("DocProdETicketCheck_1_0");
                    strRequest = oNode.OuterXml;
                    strResponse = ttGA.SendMessage(strRequest, ConversationID);

                    // send ignore to release the seats
                    oNode = oRoot.SelectSingleNode("PNRBFManagement_7_9[position()=2]");
                    strRequest = oNode.OuterXml;
                    strRequest = ttGA.SendMessage(strRequest, ConversationID);

                }

                // *****************************************************************
                // Transform Native Galileo ETicketVerify Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var tagToReplace = strResponse.Contains("</DocProdETicketCheck_1_0>")
                        ? "</DocProdETicketCheck_1_0>"
                        : "</PNRBFManagement_7_9>";

                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_ETicketVerifyRS.xsl");

                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response. \r\n {ex.Message}");
                }

            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.ETicketVerify, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string MultiMessage()
        {
            string strResponse;

            try
            {
                GalileoAdapter ttGA = null;
                XmlElement oRoot;
                string strRequest = Request;
                var oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(strRequest);
                oRoot = oReqDoc.DocumentElement;

                if (oRoot.SelectSingleNode("MultiMessage") is null)
                {
                    throw new Exception("MultiMessage is missing in the Request.");
                }


                // ***************************************************************************
                // Transform each request inside the MultiMessage and aggregate native Galileo messages   *
                // ***************************************************************************

                try
                {
                    strRequest = "<Requests>";
                    foreach (XmlNode currentONode in oRoot.SelectSingleNode("MultiMessage").ChildNodes)
                    {
                        var oNode = currentONode;
                        strRequest += "<Request><Transaction>";

                        if (oNode.OuterXml.IndexOf("<OTA_AirAvailRQ") != -1)
                        {
                            strRequest += $"{CoreLib.TransformXML(oNode.OuterXml, XslPath, $"{Version}Galileo_AirAvailRQ.xsl")}";
                        }
                        else if (oNode.OuterXml.IndexOf("<OTA_AirLowFareSearchRQ") != -1)
                        {
                            strRequest += $"{CoreLib.TransformXML(oNode.OuterXml, XslPath, $"{Version}Galileo_LowFareRQ.xsl")}";
                        }

                        strRequest += "</Transaction></Request>";
                    }

                    strRequest += "</Requests>";
                }
                catch (Exception ex)
                {
                    throw new Exception($"Invalid Request.\r\n{ex.Message}");
                }

                // ***************************************************************************
                // Send MultiMessage Request to the Galileo Adapter and Getting MultiMessage Response   *
                // ***************************************************************************


                ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMultiMessage(strRequest);

                // ***************************************************************************
                // Transform each response from Galileo into OTA responses   *
                // ***************************************************************************
                try
                {
                    strResponse = strResponse.Replace(" xmlns=\"http://ns.galileo.com\"", "");
                    oReqDoc.LoadXml(strResponse);
                    oRoot = oReqDoc.DocumentElement;
                    strResponse = "";

                    foreach (XmlNode currentONode1 in oRoot.ChildNodes)
                    {
                        var oNode = currentONode1;
                        if (oNode.OuterXml.Contains("<AirAvailability_"))
                        {
                            strResponse += CoreLib.TransformXML(oNode.InnerXml, XslPath, $"{Version}Galileo_AirAvailRS.xsl");

                        }
                        else if (oNode.OuterXml.Contains("<FareQuoteSuperBB_"))
                        {
                            strResponse += CoreLib.TransformXML(oNode.InnerXml, XslPath, $"{Version}Galileo_LowFareRS.xsl");
                        }
                    }

                    strResponse = $"<MultiMessageRS><Success/><Response>{strResponse}</Response></MultiMessageRS>";

                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }


                // ****************************************************************
                // Build Native Galileo message Response into XML MultiMessage Response *
                // ****************************************************************
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.MultiMessage, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string ProfileRead()
        {

            string strRequest;
            string strResponse;

            // *************************************************************************
            // Transform OTA Currency Convertion Request into Native Galileo Request  *
            // *************************************************************************

            try
            {
                strRequest = SetRequest("Galileo_ProfileReadRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 

                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);

                // ***************************************************************************
                // Transform Native Galileo Currency Convertion Response into OTA Response  *
                // ***************************************************************************

                try
                {

                    var tagToReplace = "</ClientFile_2>";

                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_ProfileReadRS.xsl", false);

                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.ProfileRead, ex.Message, ProviderSystems);
            }
            return strResponse;
        }

        public string ProfileCreate()
        {
            string strResponse;

            // *************************************************************************
            // Transform OTA Currency Convertion Request into Native Galileo Request  *
            // *************************************************************************

            try
            {
                string strRequest = SetRequest("Galileo_ProfileCreateRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 

                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);

                // ***************************************************************************
                // Transform Native Galileo Currency Convertion Response into OTA Response  *
                // ***************************************************************************

                try
                {

                    var tagToReplace = "</ClientFile_2>";

                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_ProfileReadRS.xsl", false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }

            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.ProfileRead, ex.Message, ProviderSystems);
            }
            return strResponse;
        }

        public string SalesReport()
        {
            string strResponse;

            try
            {
                // ************************************************************
                //  Transform SalesReport Request into Native Amadeus Request *
                // ************************************************************ 
                string strRequest = SetRequest("Galileo_SalesReportRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 

                XmlDocument oDocA = new XmlDocument();
                oDocA.LoadXml(strRequest);
                XmlElement oRootA = oDocA.DocumentElement;

                strResponse = ttGA.SendMessage(strRequest);
                strResponse = strResponse.Replace("/$", "");

                // ****************************************************************
                //  Transform Native Amadeus SalesReport Response into Response   *
                // **************************************************************** 
                try
                {
                    var tagToReplace = strResponse.Contains("Errors") ? "</Errors>" : "</SalesReports_DisplayQueryReportReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "GalileoAdapter", "Response to transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_SalesReportRS.xsl", false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttGA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.SalesReport, ex.Message, ProviderSystems);
            }

            return strResponse;
        }
    }
}