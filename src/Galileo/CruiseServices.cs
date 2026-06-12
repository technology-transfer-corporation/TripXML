using System;
using System.Xml;
using TripXMLMain;

namespace Galileo
{
    public class CruiseServices : GalileoBase
    {
        public string CruiseSailAvail()
        {

            string strResponse;
            try
            {
                // *****************************************************************
                // Transform OTA CruiseSailAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CruiseSailAvailRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 

                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);

                // *****************************************************************
                // Transform Native Galileo CruiseSailAvail Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = "</CruiseSailAvail>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CruiseSailAvailRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseSailAvail, ex.Message, ProviderSystems);
            }
            return strResponse;

        }

        public string CruiseFareAvail()
        {

            string strResponse;
            try
            {
                // *****************************************************************
                // Transform OTA CruiseSailAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CruiseFareAvailRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);


                // *****************************************************************
                // Transform Native Galileo CruiseFareAvail Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var tagToReplace = "</CruiseFareAvail>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CruiseFareAvailRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseFareAvail, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string CruiseCategoryAvail()
        {
            string strResponse;
            try
            {
                // *****************************************************************
                // Transform OTA CruiseSailAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CruiseCategoryAvailRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);


                // *****************************************************************
                // Transform Native Galileo CruiseFareAvail Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var tagToReplace = "</CruiseCategoryAvail>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CruiseCategoryAvailRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseCategoryAvail, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string CruiseCabinAvail()
        {
            string strResponse;
            try
            {
                // *****************************************************************
                // Transform OTA CruiseSailAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CruiseCabinAvailRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);


                // *****************************************************************
                // Transform Native Galileo CruiseFareAvail Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var tagToReplace = "</CruiseCabinAvail>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CruiseCabinAvailRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseCabinAvail, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string CruiseCabinHold()
        {
            string strResponse;
            try
            {
                // *****************************************************************
                // Transform OTA CruiseSailAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CruiseCabinHoldRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);


                // *****************************************************************
                // Transform Native Galileo CruiseFareAvail Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var tagToReplace = "</CruiseCabinHold>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CruiseCabinHoldRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseCabinHold, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string CruiseCabinUnhold()
        {
            string strResponse;
            try
            {
                // *****************************************************************
                // Transform OTA CruiseSailAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CruiseCabinUnholdRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);


                // *****************************************************************
                // Transform Native Galileo CruiseFareAvail Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var tagToReplace = "</CruiseCabinHold>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CruiseCabinUnholdRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseCabinUnhold, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string CruisePriceBooking()
        {
            string strResponse;
            try
            {
                // *****************************************************************
                // Transform OTA CruiseSailAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CruisePriceBookingRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);


                // *****************************************************************
                // Transform Native Galileo CruiseFareAvail Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var tagToReplace = "</CruiseCabinHold>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CruisePriceBookingRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruisePriceBooking, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string CruiseCreateBooking()
        {
            string strResponse;
            try
            {
                // *****************************************************************
                // Transform OTA CruiseSailAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CruiseCreateBookingRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);


                // *****************************************************************
                // Transform Native Galileo CruiseFareAvail Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var tagToReplace = "</CruiseCabinHold>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CruiseCreateBookingRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseCreateBooking, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string CruiseRead()
        {
            string strResponse;
            try
            {
                // *****************************************************************
                // Transform OTA CruiseSailAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CruiseReadRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);

                // Air Price Request
                XmlDocument oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                XmlElement oRoot = oDoc.DocumentElement;
                var oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo/elementManagementItinerary[segmentName='CRU']/reference/number");
                if (oNode != null)
                {
                    strRequest = $"<CruiseByPass_GetBookingDetails><agentEnvironment><agentTerminalId>09097451</agentTerminalId></agentEnvironment><bookingReference><referenceType>S</referenceType><uniqueReference>{oNode.InnerText}</uniqueReference></bookingReference></CruiseByPass_GetBookingDetails>";
                    strResponse = ttGA.SendMessage(strRequest, ConversationID);
                }

                // *****************************************************************
                // Transform Native Galileo CruiseFareAvail Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var tagToReplace = "</CruiseCabinHold>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CruiseReadRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseRead, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string CruiseCancelBooking()
        {

            string strResponse;
            try
            {
                // *****************************************************************
                // Transform OTA CruiseSailAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CruiseCancelRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);

                // ********************************************************
                // Get the Line Number and Send a Cruise Cancel to Galileo *
                // ********************************************************
                var oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                var oRoot = oDoc.DocumentElement;

                // Air Price Request
                var oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo/elementManagementItinerary[segmentName='CRU']/reference/number");
                if (oNode != null)
                {
                    strRequest = $"<CruiseByPass_CancelBooking><agentEnvironment><agentTerminalId>09097451</agentTerminalId></agentEnvironment><bookingReference><referenceType>S</referenceType><uniqueReference>{oNode.InnerText}</uniqueReference></bookingReference><bookingQualifier><partyQualifier>8</partyQualifier><componentDetails><componentQualifier>10</componentQualifier><componentDescription>TT</componentDescription></componentDetails></bookingQualifier><sailingGroup><sailingDescription><providerDetails><shipCode>";

                    oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']/typicalCruiseData/sailingShipInformation/shipDetails/code");
                    strRequest += $"{oNode.InnerText}</shipCode><cruiselineCode>";

                    oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']/typicalCruiseData/sailingShipInformation/shipDetails/cruiseLineCode");
                    strRequest += $"{oNode.InnerText}<cruiselineCode></providerDetails><sailingDateTime><sailingDepartureDate>";

                    oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']/typicalCruiseData/sailingDateInformation/beginDateTime/day");

                    strRequest += oNode.InnerText.Length == 1
                        ? $"0{oNode.InnerText}"
                        : $"{oNode.InnerText}";

                    oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']/typicalCruiseData/sailingDateInformation/beginDateTime/month");

                    strRequest += oNode.InnerText.Length == 1
                        ? $"0{oNode.InnerText}"
                        : $"{oNode.InnerText}";

                    oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']/typicalCruiseData/sailingDateInformation/beginDateTime/year");
                    strRequest += $"{oNode.InnerText}</sailingDepartureDate><sailingDuration></sailingDuration></sailingDateTime><sailingId><cruiseVoyageNbr></cruiseVoyageNbr></sailingId></sailingDescription><currencyInfo><currencyList><currencyQualifier>5</currencyQualifier><currencyIsoCode></currencyIsoCode></currencyList></currencyInfo></sailingGroup></CruiseByPass_CancelBooking>";

                    strResponse = ttGA.SendMessage(strRequest, ConversationID);
                }


                // *****************************************************************
                // Transform Native Galileo CruiseFareAvail Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = "</CruiseCancel>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CruiseCancelRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseCancelBooking, ex.Message, ProviderSystems);
            }

            return strResponse;

        }

        public string CruiseModifyBooking()
        {
            string strResponse;
            try
            {
                // *****************************************************************
                // Transform OTA CruiseSailAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CruiseModifyBookingRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);

                // ********************************************************
                // Get the Line Number and Send a Cruise Cancel to Galileo *
                // ********************************************************
                var oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                var oRoot = oDoc.DocumentElement;

                // Air Price Request
                var oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo/elementManagementItinerary[segmentName='CRU']/reference/number");

                if (oNode != null)
                {
                    strRequest = $"<CruiseByPass_CancelBooking><agentEnvironment><agentTerminalId>09097451</agentTerminalId></agentEnvironment><bookingReference><referenceType>S</referenceType><uniqueReference>{oNode.InnerText}</uniqueReference></bookingReference><bookingQualifier><partyQualifier>8</partyQualifier><componentDetails><componentQualifier>10</componentQualifier><componentDescription>TT</componentDescription></componentDetails></bookingQualifier><sailingGroup><sailingDescription><providerDetails><shipCode>";

                    oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']/typicalCruiseData/sailingShipInformation/shipDetails/code");
                    strRequest += $"{oNode.InnerText}</shipCode><cruiselineCode>";

                    oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']/typicalCruiseData/sailingShipInformation/shipDetails/cruiseLineCode");
                    strRequest += $"{oNode.InnerText}<cruiselineCode></providerDetails><sailingDateTime><sailingDepartureDate>";

                    oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']/typicalCruiseData/sailingDateInformation/beginDateTime/day");

                    strRequest += oNode.InnerText.Length == 1
                        ? $"0{oNode.InnerText}"
                        : $"{oNode.InnerText}";

                    oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']/typicalCruiseData/sailingDateInformation/beginDateTime/month");

                    strRequest += oNode.InnerText.Length == 1
                        ? $"0{oNode.InnerText}"
                        : $"{oNode.InnerText}";

                    oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']/typicalCruiseData/sailingDateInformation/beginDateTime/year");
                    strRequest += $"{oNode.InnerText}</sailingDepartureDate><sailingDuration></sailingDuration></sailingDateTime><sailingId><cruiseVoyageNbr></cruiseVoyageNbr></sailingId></sailingDescription><currencyInfo><currencyList><currencyQualifier>5</currencyQualifier><currencyIsoCode></currencyIsoCode></currencyList></currencyInfo></sailingGroup></CruiseByPass_CancelBooking>";

                    strResponse = ttGA.SendMessage(strRequest, ConversationID);
                }


                // *****************************************************************
                // Transform Native Galileo CruiseFareAvail Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = "</CruiseModifyBooking>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CruiseModifyBookingRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseModifyBooking, ex.Message, ProviderSystems);
            }

            return strResponse;



        }

        public string CruisePackageAvail()
        {
            string strResponse;
            try
            {
                // *****************************************************************
                // Transform OTA CruiseSailAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CruisePackageAvailRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);


                // *****************************************************************
                // Transform Native Galileo CruiseFareAvail Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = "</CruisePackageAvail>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CruisePackageAvailRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruisePackageAvail, ex.Message, ProviderSystems);
            }

            return strResponse;

        }

        public string CruisePackageDesc()
        {
            string strResponse;
            try
            {
                // *****************************************************************
                // Transform OTA CruiseSailAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CruisePackageDescRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);


                // *****************************************************************
                // Transform Native Galileo CruiseFareAvail Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var tagToReplace = "</CruisePackageAvail>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CruisePackageDescRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruisePackageDesc, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string CruiseTransferAvail()
        {
            string strResponse;
            try
            {
                // *****************************************************************
                // Transform OTA CruiseSailAvail Request into Native Galileo Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Galileo_CruiseTransferAvailRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);


                // *****************************************************************
                // Transform Native Galileo CruiseFareAvail Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    var tagToReplace = "</CruiseTransferAvail>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "ttGalileoService", "Response before last transform", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_CruiseTransferAvailRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruisePackageDesc, ex.Message, ProviderSystems);
            }

            return strResponse;
        }
    }
}