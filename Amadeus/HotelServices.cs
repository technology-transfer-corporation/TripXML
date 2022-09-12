using TripXMLMain;
using System.Xml;
using System;
using AmadeusWS;
using static TripXMLMain.modCore.enAmadeusWSSchema;

public class HotelServices : AmadeusBase
{
    public HotelServices()
    {
        Request = "";
        ConversationID = "";
    }

    public string HotelAvail()
    {
        string strResponse = "";
        
        //***************************************************************** 
        // Transform OTA HotelAvail Request into Native Amadeus Request * 
        //***************************************************************** 
        try
        {
            string strFeaturesResponses = "";
            var test = "";
            string strSummaryOnly = "true";

            var ttAA = ttProviderSystems.SessionPool ? SetAdapter("V1") : SetAdapter();
            bool inSession = SetConversationID(ttAA);

            var strRequest = ttProviderSystems.HotelVersion.Equals("2")
                ? SetRequest("v02_AmadeusWS_HotelAvailRQ.xsl")
                : SetRequest("AmadeusWS_HotelAvailRQ.xsl");

            if (strRequest.Contains("SummaryOnly="))
            {
                strSummaryOnly = strRequest.Substring(strRequest.IndexOf("SummaryOnly=") + 13, 4);
            }

            if (strRequest.Length == 0)
            {
                throw new Exception("Transformation produced empty xml.");
            }

            //******************************************************************************* 
            // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
            //******************************************************************************* 

            XmlElement oRoot = null;
            try
            {
                string strResponses = "";
                XmlDocument oDoc = null;
                if (strRequest.Contains("<Hotel_StructuredPricing>"))
                {
                    oDoc = new XmlDocument();
                    oDoc.LoadXml(strRequest);
                    oRoot = oDoc.DocumentElement;

                    strRequest = oRoot?.FirstChild.OuterXml;
                    strResponse = SendHotelSingleAvailability(ttAA, strRequest);

                    //***************************************************************************
                    // Below given 'if else' condtion was not in teh local code. But content was there
                    //***************************************************************************

                    if (!strResponse.Contains("messageErrorList") && !strResponse.Contains("<Error>"))
                    {
                        //---------------------------------------------------------------------------
                        strRequest = oRoot.FirstChild.NextSibling.OuterXml;
                        strResponses = SendHotelStructuredPricing(ttAA, strRequest);
                    }
                    else
                    {
                        strResponses = strResponse;
                    }

                    //*************************************
                    //The below given line is in local code
                    //strResponses = strResponses.Replace(" xmlns=\"http://xml.amadeus.com/HPRSRR_04_1_1A\"", "");
                    //*************************************
                }
                else if (strRequest.Contains("<maxNumberItems>"))
                {
                    var iMaxResponses = Convert.ToInt32(strRequest.Substring(strRequest.IndexOf("<maxNumberItems>") + 16, strRequest.IndexOf("</maxNumberItems>") - (strRequest.IndexOf("<maxNumberItems>") + 16)));

                    if (iMaxResponses > 30)
                    {
                        strRequest += $"{strRequest.Substring(0, strRequest.IndexOf("<maxNumberItems>") + 16)}30{strRequest.Substring(strRequest.IndexOf("</maxNumberItems>"))}";

                        while (iMaxResponses > 0)
                        {
                            strResponse = strRequest.Contains("<Hotel_AvailabilityMultiProperties>")
                                ? SendHotelAvailabilityMultiProperties(ttAA, strRequest)
                                : SendHotelSingleAvailability(ttAA, strRequest);

                            strResponses += $"{strResponses}{strResponse}";
                            iMaxResponses = iMaxResponses - 30;

                            if (!strResponse.Contains("<displayResponse>") || !strResponse.Contains("<displayResponse>14") || iMaxResponses < 0)
                                break; // TODO: might not be correct. Was : Exit Do 

                            var iNextItem = Convert.ToInt32(strResponse.Substring(strResponse.IndexOf("<nextItemReference>") + 19,
                                strResponse.IndexOf("</nextItemReference>") - (strResponse.IndexOf("<nextItemReference>") + 19)));
                            strRequest += $"{strRequest.Substring(0, strRequest.IndexOf("<nextItemReference />"))}<nextItemReference>{iNextItem})</nextItemReference>{strRequest.Substring(strRequest.IndexOf("<nextItemReference />") + 21)}";
                            strRequest += $"{strRequest.Substring(0, strRequest.IndexOf("<displayRequest>") + 16)}030{strRequest.Substring(strRequest.IndexOf("</displayRequest>"))}";

                            if (iMaxResponses < 30)
                            {
                                strRequest = $"{strRequest.Substring(0, strRequest.IndexOf("<maxNumberItems>") + 16)}{iMaxResponses}{strRequest.Substring(strRequest.IndexOf("</maxNumberItems>"))}";
                            }
                        }
                        //**************
                        // The below given entire code block was not in local code
                        //**************

                        if (ttProviderSystems.HotelMedia && strResponses.Contains("messageErrorList") && !strResponses.Contains("<Error>"))
                        {
                            string strHotelMedia = "";
                            strRequest = CoreLib.TransformXML(strResponses, XslPath, $"AmadeusWS_HotelInfo1RQ.xsl", false);

                            strHotelMedia = SendHotelDescriptiveInfo(ttAA, strRequest);

                            strHotelMedia = strHotelMedia.Replace(" xmlns=\"http://www.opentravel.org/OTA/2003/05\"", "")
                                .Replace(" xmlns=\"http://www.opentravel.org/OTA/2003/05\"", "")
                                .Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "")
                                .Replace(" xsi:schemaLocation=\"http://www.opentravel.org/OTA/2003/05 OTA_HotelDescriptiveInfoRS.xsd\"", "")
                                .Replace("pdt.multimediarepository.testing", "multimediarepository");

                            strResponses = strResponses.Replace("</Hotel_SingleAvailabilityReply>", strHotelMedia + "</Hotel_SingleAvailabilityReply>")
                                .Replace("</Hotel_AvailabilityMultiPropertiesReply>", strHotelMedia + "</Hotel_AvailabilityMultiPropertiesReply>");
                        }
                        //-----------------------------------------------------------------------------------------------
                    }
                    else
                    {
                        strResponse = strRequest.Contains("<Hotel_AvailabilityMultiProperties>")
                            ? SendHotelAvailabilityMultiProperties(ttAA, strRequest)
                            : SendHotelSingleAvailability(ttAA, strRequest);
                        //*****************************************************
                        // Below given entire if block is not in the local code
                        //*****************************************************


                        if (ttProviderSystems.HotelMedia && (!strResponses.Contains("messageErrorList") || strResponses.Contains("propertyAvailabilityList")) && !strResponses.Contains("<Error>"))
                        {
                            strRequest = CoreLib.TransformXML(strResponses, XslPath, $"AmadeusWS_HotelInfo1RQ.xsl", false);

                            oDoc = new XmlDocument();
                            oDoc.LoadXml(strRequest);
                            oRoot = oDoc.DocumentElement;

                            var strHotelMedia = SendHotelDescriptiveInfo(ttAA, strRequest);

                            strHotelMedia = strHotelMedia.Replace(" xmlns=\"http://www.opentravel.org/OTA/2003/05\"", "")
                                .Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "")
                                .Replace(" xsi:schemaLocation=\"http://www.opentravel.org/OTA/2003/05 OTA_HotelDescriptiveInfoRS.xsd\"", "")
                                .Replace("pdt.multimediarepository.testing", "multimediarepository");

                            if (strResponses.Contains("Hotel_SingleAvailabilityReply"))
                            {
                                foreach (XmlNode oNodeM in oRoot.SelectNodes("Hotel_StructuredPricing"))
                                {
                                    strHotelMedia = SendHotelStructuredPricing(ttAA, strRequest);
                                }
                            }

                            strResponses = strResponses.Replace("</Hotel_SingleAvailabilityReply>", strHotelMedia + "</Hotel_SingleAvailabilityReply>")
                                .Replace("</Hotel_AvailabilityMultiPropertiesReply>", strHotelMedia + "</Hotel_AvailabilityMultiPropertiesReply>");
                        }

                        //--------------------------------------------------------------------------------------------------
                    }
                }
                else if (ttProviderSystems.HotelVersion == "2")
                {
                    strResponse = SendHotelMultiSingleAvailability(ttAA, strRequest);

                    strResponses = strResponses.Replace(" xsi:schemaLocation=\"http://www.opentravel.org/OTA/2003/05 OTA_HotelAvailRS.xsd\"", "")
                        .Replace(" xmlns=\"http://www.opentravel.org/OTA/2003/05\"", "")
                        .Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");

                    if (ttProviderSystems.HotelMedia && strResponses.Contains("HotelStay") && !strResponses.Contains("<Errors>"))
                    {
                        string strHotelMedia = "";
                        strRequest = CoreLib.TransformXML(strResponses, XslPath, $"{Version}v02_AmadeusWS_HotelInfo1RQ.xsl", false);

                        oDoc = new XmlDocument();
                        oDoc.LoadXml(strRequest);
                        oRoot = oDoc.DocumentElement;

                        strHotelMedia = SendHotelDescriptiveInfo(ttAA, strRequest);

                        strHotelMedia = strHotelMedia.Replace(" xmlns=\"http://www.opentravel.org/OTA/2003/05\"", "")
                            .Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "")
                            .Replace(" xsi:schemaLocation=\"http://www.opentravel.org/OTA/2003/05 OTA_HotelDescriptiveInfoRS.xsd\"", "")
                            .Replace("pdt.multimediarepository.testing", "multimediarepository");

                        if (Request.Contains("HotelCode"))
                        {
                            foreach (XmlNode oNodeM in oRoot.SelectNodes("OTA_HotelAvailRQ"))
                            {
                                strHotelMedia = SendHotelMultiSingleAvailability(ttAA, strRequest);

                                strHotelMedia = strHotelMedia.Replace(" xmlns=\"http://www.opentravel.org/OTA/2003/05\"", "")
                                    .Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "")
                                    .Replace(" xsi:schemaLocation=\"http://www.opentravel.org/OTA/2003/05 OTA_HotelAvailRS.xsd\"", "");
                            }
                        }

                        strResponses = strResponses.Replace("</OTA_HotelAvailRS>", strHotelMedia + "</OTA_HotelAvailRS>");
                    }
                }
                else
                {
                    throw new Exception("Maximum number of items mandatory in request.");
                }

                if (strResponses.Contains("<Hotel_AvailabilityMultiPropertiesReply>"))
                {
                    strResponse += $"<Hotel_AvailabilityMultiPropertiesReply>{strResponses.Replace("<Hotel_AvailabilityMultiPropertiesReply>", "").Replace("</Hotel_AvailabilityMultiPropertiesReply>", "")}</Hotel_AvailabilityMultiPropertiesReply>";
                }
                else if (strResponses.Contains("<Hotel_SingleAvailabilityReply>"))
                {
                    strResponse += $"<Hotel_SingleAvailabilityReply>{strResponses.Replace("<Hotel_SingleAvailabilityReply>", "").Replace("</Hotel_SingleAvailabilityReply>", "")}</Hotel_SingleAvailabilityReply>";
                }
                else
                {
                    strResponse = strResponses;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (strSummaryOnly == "fals")
            {

                //********************************************************************************** 
                // Transform Native Amadeus HotelAvail Response into Native Hotel Features Request * 
                //********************************************************************************** 

                string strFeaturesResponse;
                try
                {
                    strFeaturesResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_HotelAvail_FeaturesRS.xsl", false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Features Response. \r\n{ex.Message}");
                }

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 

                XmlDocument oReqDoc = null;
                try
                {
                    oReqDoc = new XmlDocument();
                    oReqDoc.LoadXml(strFeaturesResponse);
                    oRoot = oReqDoc.DocumentElement;
                    foreach (XmlNode oNodef in oRoot.SelectNodes("Hotel_Features"))
                    {
                        strFeaturesResponses = SendHotelFeatures(ttAA, oNodef.OuterXml);
                    }

                    strFeaturesResponses += $"<HotelsRS>{strFeaturesResponses}</HotelsRS>";
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                //*********************************************************************** 
                // Add Amadeus Features Response to Native Amadeus HotelAvail Response * 
                //*********************************************************************** 

                XmlNode oNode = null;
                try
                {
                    oReqDoc.LoadXml(strFeaturesResponses);
                    oRoot = oReqDoc.DocumentElement;
                    oNode = oRoot;
                    oReqDoc.LoadXml(strResponse);
                    oRoot = oReqDoc.DocumentElement;
                    oRoot?.AppendChild(oNode);

                    strResponse = oRoot.OuterXml;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Adding Amadeus Features Response to Native Amadeus HotelAvail Response.\r\n{ex.Message}");
                }
            }

            //**************************************************************************************** 
            // Transform Native Amadeus HotelAvail Response + Features Response into OTA Response * 
            //**************************************************************************************** 

            try
            {
                var tagToReplace = strRequest.Contains("Hotel_AvailabilityMultiPropertiesReply") ? "</Hotel_AvailabilityMultiPropertiesReply>" : "</Hotel_SingleAvailabilityReply>";
                if (inSession)
                    strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSService", "Final native hotel Avail response", strResponse, ttProviderSystems.LogUUID);

                strResponse = ttProviderSystems.HotelVersion == "2"
                    ? CoreLib.TransformXML(strResponse, XslPath, "v02_AmadeusWS_HotelAvailRS.xsl", false)
                    : CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_HotelAvailRS.xsl", false);
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
            strResponse = modCore.FormatErrorMessage(modCore.ttServices.HotelAvail, ex.Message, ttProviderSystems);
        }

        return strResponse;
    }

    public string HotelInfo()
    {
        string strResponse;

        //***************************************************************** 
        // Transform OTA HotelInfo Request into Native Amadeus Request * 
        //***************************************************************** 
        try
        {
            var ttAA = ttProviderSystems.SessionPool
                ? SetAdapter("V1")
                : SetAdapter();
            bool inSession = SetConversationID(ttAA);

            var strRequest = SetRequest("AmadeusWS_HotelInfoRQ.xsl");

            if (string.IsNullOrEmpty(strRequest))
                throw new Exception("Transformation produced empty xml.");

            //******************************************************************************* 
            // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
            //******************************************************************************* 
            strResponse = SendHotelDescriptiveInfo(ttAA, strRequest);
            strResponse = strResponse.Replace(" xsi:schemaLocation=\"http://www.opentravel.org/OTA/2003/05 OTA_HotelDescriptiveInfoRS.xsd\"", "")
                .Replace(" xmlns=\"http://www.opentravel.org/OTA/2003/05\"", "")
                .Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "")
                .Replace(" xmlns=\"http://xml.amadeus.com/" + ttProviderSystems.AmadeusWSSchema[Hotel_FeaturesReply] + "\"", "");

            //***************************************************************** 
            // Transform Native Amadeus HotelInfo Response into OTA Response * 
            //***************************************************************** 
            try
            {
                var tagToReplace = "</Hotel_FeaturesReply>";
                if (inSession)
                    strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_HotelInfoRS.xsl", false);
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
                    ConversationID = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            strResponse = modCore.FormatErrorMessage(modCore.ttServices.HotelInfo, ex.Message, ttProviderSystems);
        }

        return strResponse;
    }

    public string HotelSearch()
    {
        string strResponse;

        //***************************************************************** 
        // Transform OTA HotelSearch Request into Native Amadeus Request * 
        //***************************************************************** 
        try
        {
            var ttAA = ttProviderSystems.SessionPool
                ? SetAdapter("V1")
                : SetAdapter();

            bool inSession = SetConversationID(ttAA);

            var strRequest = SetRequest("AmadeusWS_HotelSearchRQ.xsl");
            if (string.IsNullOrEmpty(strRequest))
                throw new Exception("Transformation produced empty xml.");

            strResponse = SendHotelList(ttAA, strRequest);

            var oDocResp = new XmlDocument();
            oDocResp.LoadXml(strResponse);
            var oRootResp = oDocResp.DocumentElement;

            if (oRootResp?.SelectSingleNode("scrollingInformation/displayResponse")?.InnerText == "19")
            {
                var xmlDocResFinal = oDocResp;
                var oDocReq = new XmlDocument();

                oDocReq.LoadXml(strRequest);
                var oRootReq = oDocReq.DocumentElement;

                XmlNode nodeNew = oDocReq.SelectSingleNode("Hotel_List/scrollingInformation");
                nodeNew = oDocReq.CreateElement("nextItemReference");
                oDocReq.SelectSingleNode("Hotel_List/scrollingInformation").AppendChild(nodeNew);

                oDocReq.SelectSingleNode("Hotel_List/scrollingInformation/nextItemReference").InnerText = (oRootResp).FirstChild.ChildNodes[0].InnerText;
                oDocReq.SelectSingleNode("Hotel_List/scrollingInformation/displayRequest").InnerText = "030";

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendHotelList(ttAA, strRequest);

                oDocResp = new XmlDocument();
                oDocResp.LoadXml(strResponse);
                oRootResp = oDocResp.DocumentElement;

                XmlNodeList reslist = oRootResp.SelectNodes("propertyList");

                foreach (XmlNode node in reslist)
                {
                    XmlNode impNode = xmlDocResFinal.ImportNode(node, true);
                    xmlDocResFinal.DocumentElement.InsertAfter(impNode, xmlDocResFinal.DocumentElement.LastChild);
                }

                strResponse = xmlDocResFinal.InnerXml;
            }

            try
            {
                var tagToReplace = "</Hotel_ListReply>";
                if (inSession)
                    strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");
                
                //***************************************************************** 
                // Transform Native Amadeus HotelSearch Response into OTA Response * 
                //***************************************************************** 
                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_HotelSearchRS.xsl", false);
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
                    ConversationID = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            strResponse = modCore.FormatErrorMessage(modCore.ttServices.HotelSearch, ex.Message, ttProviderSystems);
        }
        
        return strResponse;
    }
}
