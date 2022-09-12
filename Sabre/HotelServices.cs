using System;
using System.Xml;
using TripXMLMain;

namespace Sabre
{
    public class HotelServices : SabreBase
    {
        public HotelServices()
        {
            Request = "";
            ConversationID = "";
        }

        public string HotelAvail()
        {
            string strResponse;

            try
            {
                string SabreAction = "";
                
                try
                {
                    var oReqDoc = new XmlDocument();
                    oReqDoc.LoadXml(Request);
                    var oRoot = oReqDoc.DocumentElement;
                    if (oRoot.SelectSingleNode("AvailRequestSegments/AvailRequestSegment/HotelSearchCriteria/Criterion/HotelRef").Attributes["HotelCode"] is null)
                    {
                        SabreAction = "OTA_HotelAvailLLSRQ";
                    }
                    else
                    {
                        SabreAction = "OTA_HotelAvailLLSRQ";
                    }
                }
                catch (Exception ex)
                {
                    SabreAction = "OTA_HotelAvailLLSRQ";
                }

                // *****************************************************************
                // Transform OTA HotelAvail Request into Native Sabre Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Sabre_HotelAvailRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                var ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);
                strResponse = ttSA.SendMessage(strRequest.Replace(" xmlns=\"\"", ""), "Hotel Availability", SabreAction);                

                // *****************************************************************
                // Transform Native Sabre HotelAvail Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var tagToReplace = "</OTA_HotelAvailRS>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_HotelAvailRS.xsl", false);
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
                        ConversationID = "";
                    }
                }

            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.HotelAvail, ex.Message, ProviderSystems, "");
            }

            return strResponse;
        }

        public string HotelInfo()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA HotelInfo Request into Native Sabre Request     *
            // ***************************************************************** 

            try
            {
                string strRequest = SetRequest("Sabre_HotelInfoRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                var ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);
                strResponse = ttSA.SendMessage(strRequest, "Hotel", "OTA_HotelDescriptionRQ");
                

                // *****************************************************************
                // Transform Native Sabre HotelInfo Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = "</OTA_HotelAvailRS>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_HotelInfoRS.xsl", false);
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
                        ConversationID = "";
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.HotelInfo, ex.Message, ProviderSystems, "");
            }

            return strResponse;
            
        }

        public string HotelSearch()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA HotelSearch Request into Native Sabre Request     *
            // ***************************************************************** 
            try
            {
                string strRequest = SetRequest("Sabre_HotelSearchRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                var ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);
                strResponse = ttSA.SendMessage(strRequest, "Hotel", "OTA_HotelAvailRQ");
                

                // *****************************************************************
                // Transform Native Sabre HotelSearch Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = "</OTA_HotelAvailRS>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_HotelSearchRS.xsl", false);
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
                        ConversationID = "";
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.HotelSearch, ex.Message, ProviderSystems, "");
            }

            return strResponse;
            
        }
    }
}