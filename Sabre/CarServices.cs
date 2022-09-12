using System;
using TripXMLMain;

namespace Sabre
{
    public class CarServices : SabreBase
    {
        public CarServices()
        {
            Request = "";
            ConversationID = "";
        }

        public string CarAvail()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA CarAvail Request into Native Sabre Request     *
            // ***************************************************************** 
            try
            {
                string strRequest = SetRequest("Sabre_CarAvailRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                var ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);
                strResponse = ttSA.SendMessage(strRequest, "Car", "OTA_VehAvailRateRQ");                

                // *****************************************************************
                // Transform Native Sabre CarAvail Response into OTA Response   *
                // *****************************************************************
                try
                {
                    var tagToReplace = "</OTA_VehAvailRateRS>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_CarAvailRS.xsl", false);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CarAvail, ex.Message, ProviderSystems, "");
            }

            return strResponse;
        }

        public string CarInfo()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA CarInfo Request into Native Sabre Request     *
            // ***************************************************************** 

            try
            {
                string strRequest = SetRequest("Sabre_CarInfoRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                var ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);
                strResponse = ttSA.SendMessage(strRequest, "Car", "OTA_VehLocDetailRQ");
                // *****************************************************************
                // Transform Native Sabre CarInfo Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var tagToReplace = "</OTA_VehLocDetailRS>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_CarInfoRS.xsl", false);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CarInfo, ex.Message, ProviderSystems, "");
            }

            return strResponse;

        }
    }
}