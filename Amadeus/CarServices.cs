using TripXMLMain;
using System;

// System.Xml namespace is not added in the local code
using System.Xml;
using AmadeusWS;

public class CarServices : AmadeusBase
{
    public CarServices()
    {
        Request = "";
        ConversationID = "";
    }

    public string CarAvail()
    {
        string strResponse;

        //***************************************************************** 
        // Transform OTA CarAvail Request into Native Amadeus Request * 
        //***************************************************************** 

        try
        {
            AmadeusWSAdapter ttAA = ttProviderSystems.SessionPool
                ? SetAdapter("V1")
                : SetAdapter();
            bool inSession = SetConversationID(ttAA);

            string strRequest = SetRequest("AmadeusWS_CarAvailRQ.xsl");

            //******************************************************************************* 
            // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
            //******************************************************************************* 
            strResponse = SendCarSingleAvailability(ttAA, strRequest);

            //***************************************************************** 
            // Transform Native Amadeus CarAvail Response into OTA Response * 
            //***************************************************************** 
            try
            {
                var tagToReplace = "</Car_SingleAvailability>";
                if (inSession)
                    strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CarAvailRS.xsl", false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
            }
            finally
            {
                if (!inSession)
                {
                    ttAA.CloseSession(ConversationID);
                    ConversationID = "";
                }
            }
        }
        catch (Exception ex)
        {
            addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
            strResponse = modCore.FormatErrorMessage(modCore.ttServices.CarAvail, ex.Message, ttProviderSystems, "");
        }
        return strResponse;
    }

    public string CarInfo()
    {
        string strRequest;
        string strResponse;
        //***************************************************************** 
        // Transform OTA CarInfo Request into Native Amadeus Request * 
        //***************************************************************** 

        try
        {
            var ttAA = SetAdapter();
            bool inSession = SetConversationID(ttAA);

            strRequest = SetRequest("AmadeusWS_CarInfoRQ.xsl");
            if (string.IsNullOrEmpty(strRequest))
                throw new Exception("Transformation produced empty xml.");

            //******************************************************************************* 
            // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
            //******************************************************************************* 
            strResponse = SendCarPolicy(ttAA, strRequest);

            //***************************************************************** 
            // Transform Native Amadeus CarInfo Response into OTA Response * 
            //***************************************************************** 

            try
            {
                var tagToReplace = "</Car_Policy>";
                if (inSession)
                    strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CarInfoRS.xsl", false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming Native Response.\r\n {ex.Message}");
            }
            finally
            {
                if (!inSession)
                {
                    ttAA.CloseSession(ConversationID);
                    ConversationID = "";
                }
            }
        }
        catch (Exception ex)
        {
            addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
            strResponse = modCore.FormatErrorMessage(modCore.ttServices.SalesReport, ex.Message, ttProviderSystems);
        }

        return strResponse;
    }

    public string CarRules()
    {
        string strRequest;
        string strResponse;

        //Below given variables are not in the local code

        //------------------------------------------------

        //***************************************************************** 
        // Transform OTA CarRules Request into Native Amadeus Request * 
        //***************************************************************** 

        try
        {
            var oDoc = new XmlDocument();
            var ttAA = SetAdapter();
            bool inSession = SetConversationID(ttAA);

            strRequest = SetRequest("AmadeusWS_CarRulesRQ.xsl");
            if (string.IsNullOrEmpty(strRequest))
                throw new Exception("Transformation produced empty xml.");

            oDoc.LoadXml(strRequest);
            var oRoot = oDoc.DocumentElement;

            //******************************************************************************* 
            // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
            //******************************************************************************* 
            strRequest = oRoot?.SelectSingleNode("Car_SingleAvailability")?.OuterXml;
            strResponse = SendCarSingleAvailability(ttAA, strRequest);

            strRequest = oRoot?.SelectSingleNode("Car_RateInformationFromAvailability")?.OuterXml;
            strResponse = SendCarRateInformationFromAvailability(ttAA, strRequest);

            //***************************************************************** 
            // Transform Native Amadeus CarRules Response into OTA Response * 
            //***************************************************************** 

            try
            {
                var tagToReplace = "</Car_RateInformationFromAvailability>";
                if (inSession)
                    strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CarRulesRS.xsl", false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
            }
            finally
            {
                if (!inSession)
                {
                    ttAA.CloseSession(ConversationID);
                    ConversationID = "";
                }
            }
        }
        catch (Exception ex)
        {
            addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
            strResponse = modCore.FormatErrorMessage(modCore.ttServices.CarRules, ex.Message, ttProviderSystems, "");
        }

        return strResponse;
    }

    public string CarList()
    {
        string strRequest;
        string strResponse = "";
        try
        {
            //***************************************************************** 
            // Transform OTA CarList Request into Native Amadeus Request * 
            //***************************************************************** 
            AmadeusWSAdapter ttAA = SetAdapter("V1");
            bool inSession = SetConversationID(ttAA);

            strRequest = SetRequest("AmadeusWS_CarListRQ.xsl");
            if (string.IsNullOrEmpty(strRequest))
                throw new Exception("Transformation produced empty xml.");

            //******************************************************************************* 
            // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
            //******************************************************************************* 
            strResponse = SendCarLocationList(ttAA, strRequest);

            //***************************************************************** 
            // Transform Native Amadeus CarList Response into OTA Response * 
            //***************************************************************** 
            try
            {
                var tagToReplace = "</Car_LocationListReply>";
                if (inSession)
                    strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CarListRS.xsl", false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error Transforming OTA Request. {ex.Message}");
            }
            finally
            {
                if (!inSession)
                {
                    ttAA.CloseSession(ConversationID);
                    ConversationID = "";
                }
            }
        }
        catch (Exception exx)
        {
            addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
            strResponse = modCore.FormatErrorMessage(modCore.ttServices.CarList, exx.Message, ttProviderSystems, "");
        }
        return strResponse;

    }
}
