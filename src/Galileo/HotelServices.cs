using System;
using System.Linq;
using System.Xml;
using TripXMLMain;

namespace Galileo
{
    public class HotelServices : GalileoBase
    {
        public HotelServices()
        {
            Request = string.Empty;
            Version = string.Empty;
        }

        public string HotelAvail()
        {
            string strResponse = "";
            // *****************************************************************
            // Transform OTA HotelAvail Request into Native Galileo Request     *
            // ***************************************************************** 
            try
            {
                string strResponses = "";
                string strSummaryOnly = "true";
                int iMaxResponses = 14;

                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                var strRequest = SetRequest("Galileo_HotelAvailRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *************************************************************************************************************
                // Add StartDate and EndDate Information to Galileo Native Request StartDate = 1 Week from today, EndDate = 2 *
                // ************************************************************************************************************* 
                var oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(strRequest);
                var oMainRoot = oReqDoc.DocumentElement;
                foreach (XmlElement oRoot in oMainRoot.ChildNodes)
                {
                    string strNode = "";
                    string strNodeDepDt = "";
                    string strNodeArrDt = "";
                    string strNightNode = "";
                    // Get the Start Date
                    if (strRequest.Contains("<HotelCompleteAvailabilityMods>"))
                    {
                        strNode = "HotelInsideAvailability";
                        strNightNode = "HotelInsideAvailability";
                        strNodeArrDt = "/DepartureDt";
                        strNodeDepDt = "/ArrivalDt";
                    }
                    else
                    {
                        strNode = "HotelRequestHeader";
                        strNightNode = " HotelRequest";
                        strNodeDepDt = "/StartDt";
                        strNodeArrDt = "/EndDt";
                    }

                    var oNode = oRoot.SelectSingleNode(strNode);
                    string strDate = oNode.InnerText;
                    if (strDate.Length == 0)
                    {
                        throw new Exception("Start Date is missing from the Request.");
                    }

                    strDate = strDate.Insert(4, "-").Insert(7, "-");
                    DateTime dtStart = Convert.ToDateTime(strDate);

                    // Get the End Date
                    strDate = oRoot.SelectSingleNode($"{strNode}{strNodeArrDt}").InnerText;
                    if (strDate.Length > 0)
                    {
                        strDate = strDate.Insert(4, "-").Insert(7, "-");
                        DateTime dtEnd = Convert.ToDateTime(strDate);
                        // Calculate Number of Nights
                        oNode = oRoot.SelectSingleNode($"{strNightNode}NumNights");
                        oNode.InnerText = dtEnd.Subtract(dtStart).Days.ToString();
                    }
                    else
                    {
                        // End Date is Missing. Get Number of Nights
                        string strNights = oRoot.SelectSingleNode($"{strNightNode}/NumNights").InnerText;
                        if (strNights.All(char.IsNumber))
                        {
                            // Calculate End Date
                            oNode = oRoot.SelectSingleNode($"{strNode}{strNodeArrDt}");
                            oNode.InnerText = dtStart.AddDays(Convert.ToDouble(strNights)).ToString("yyyyMMdd");
                        }
                        else
                        {
                            // At this point End Date is Missing and Number of Night is not good.
                            throw new Exception("End Date and Number of Night are missing from the Request. At least one of them must be in the Request.");
                        }
                    }
                }

                // New Document
                strRequest = oReqDoc.OuterXml;

                // *************************************************************************************************************
                // Add StartDate and EndDate Information to Galileo Native Request StartDate = 1 Week from today, EndDate = 2 *
                // ************************************************************************************************************* 
                strRequest = Request;
                if (strRequest.IndexOf("<HotelAvail>") == -1)
                {
                    if (strRequest.IndexOf("SummaryOnly=") != -1)
                    {
                        strSummaryOnly = strRequest.Substring(strRequest.IndexOf("SummaryOnly=") + 13, 4);
                    }

                    if (strRequest.IndexOf("MaxResponses=") != -1)
                    {
                        iMaxResponses = Convert.ToInt32(strRequest.Substring(strRequest.IndexOf("MaxResponses=") + 14, 2));
                    }
                }

                strRequest = CoreLib.TransformXML(strRequest, XslPath, $"{Version}Galileo_HotelAvailRQ.xsl");

                var arg_ProviderSystems = ProviderSystems;
                ttGA = new GalileoAdapter(arg_ProviderSystems);
                ProviderSystems = arg_ProviderSystems;

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                if (iMaxResponses > 14)
                {
                    while (iMaxResponses > 0)
                    {
                        strResponse = ttGA.SendMessage(strRequest, ConversationID);
                        if (!strResponse.Contains("<MoreInd>Y</MoreInd>"))
                            break;
                        strRequest = strRequest.Replace("</AvailabilityRequestMods>", strResponse.Substring(strResponse.IndexOf("<HotelGetMore>"),
                            strResponse.IndexOf("</HotelAvailability>") - strResponse.IndexOf("<HotelGetMore>")));
                        strRequest += "</AvailabilityRequestMods>";
                        iMaxResponses = iMaxResponses - 14;
                    }

                    foreach (XmlElement oRoot in oMainRoot.ChildNodes)
                    {
                        strRequest = !strRequest.Contains("<HotelAvailability_11_0_2")
                            ? "<HotelAvailability_11_0_2>" + oRoot.OuterXml + "</HotelAvailability_11_0_2>"
                            : "<HotelCompleteAvailability_9_0_2>" + oRoot.OuterXml + "</HotelCompleteAvailability_9_0_2>";
                        strResponse = ttGA.SendMessage(strRequest, ConversationID);
                        strResponses += strResponse;
                    }
                }

                strResponse = !strResponses.Contains("<HotelAvailability_11_0_2")
                        ? $"<HotelAvailability_11_0_2>{strResponses.Replace("<HotelAvailability_11_0_2 xmlns=\"\">", "").Replace("</HotelAvailability_11_0_2>", "")}</HotelAvailability_11_0_2>"
                        : $"<HotelCompleteAvailability_9_0_2>{strResponses.Replace("<HotelCompleteAvailability_9_0_2 xmlns=\"\">", "").Replace("</HotelCompleteAvailability_9_0_2>", "")}</HotelCompleteAvailability_9_0_2>";

                if (strSummaryOnly == "false")
                {

                    // ********************************************************************************
                    // Transform Native Galileo HotelAvail Response into Native Image Viewer Request *
                    string strImgResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_HotelAvail_ImagesRS.xsl");

                    // *******************************************************************************
                    // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                    // ******************************************************************************* 
                    strImgResponse = ttGA.SendImageViewer(strImgResponse);

                    // ********************************************************************
                    // Add Galileo Image Response to Native Galileo HotelAvail Response  *
                    // ********************************************************************
                    strImgResponse = strImgResponse.Replace(" xmlns=\"http://www.galileo.com/GI_GDS\"", "");
                    oReqDoc.LoadXml(strImgResponse);
                    oMainRoot = oReqDoc.DocumentElement;
                    var oNode = oMainRoot;
                    oReqDoc.LoadXml(strResponse);
                    oMainRoot = oReqDoc.DocumentElement;
                    oMainRoot.AppendChild(oNode);
                    strResponse = oMainRoot.OuterXml;
                }

                // ********************************************************************************************
                // Transform Native Galileo HotelAvail Response + Image Viewer Response into OTA Response    *
                // ******************************************************************************************** 
                try
                {
                    var tagToReplace = "</Hotel_AvailReply>";

                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_HotelAvailRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.HotelAvail, ex.Message, ProviderSystems);
            }
            return strResponse;
        }

        public string HotelInfo()
        {
            string strResponse = string.Empty;

            // *****************************************************************
            // Transform OTA HotelInfo Request into Native Galileo Request    *
            // ***************************************************************** 
            try
            {
                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                var strRequest = SetRequest("Galileo_HotelInfoRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *****************************************************
                // Add StartDate Information to Galileo Native Request 
                // *****************************************************
                var oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(strRequest);
                var oRoot = oReqDoc.DocumentElement;

                // Start Date 
                var oNode = oRoot.SelectSingleNode("HotelDescMods/StartDt");
                oNode.InnerText = DateTime.Now.ToString("yyyyMMdd");

                // New Document
                strRequest = oReqDoc.OuterXml;

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttGA.SendMessage(strRequest);

                // *****************************************************************
                // Transform Native Galileo HotelInfo Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = "</Hotel_InfoReply>";

                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_HotelInfoRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.HotelInfo, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string HotelSearch()
        {
            string strResponse = "";

            // *****************************************************************
            // Transform OTA HotelSearch Request into Native Galileo Request     *
            // ***************************************************************** 

            try
            {
                var ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                var strRequest = SetRequest("Galileo_HotelSearchRQ.xsl");

                // *************************************************************************************************************
                // Add StartDate and EndDate Information to Galileo Native Request StartDate = 1 Week from today, EndDate = 2 *
                // ************************************************************************************************************* 
                var oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(strRequest);
                var oRoot = oReqDoc.DocumentElement;

                // Start Date 
                var oNode = oRoot.SelectSingleNode("HtlIndexMods/StartDt");
                oNode.InnerText = DateTime.Now.AddDays(7).ToString("yyyyMMdd");
                // End Date
                oNode = oRoot.SelectSingleNode("HtlIndexMods/EndDt");
                oNode.InnerText = DateTime.Now.AddDays(14).ToString("yyyyMMdd");

                // New Document
                strRequest = oReqDoc.OuterXml;

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttGA.SendMessage(strRequest);

                // *****************************************************************
                // Transform Native Galileo HotelSearch Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = "</Hotel_SearchReply>";

                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_HotelSearchRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.HotelSearch, ex.Message, ProviderSystems);
            }

            return strResponse;
        }
    }
}