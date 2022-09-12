using TripXMLMain;
using System.Xml;
using System.Text;
using System;
using AmadeusWS;

namespace AmadeusWS
{
    public class CruiseServices : AmadeusBase
    {
        public CruiseServices()
        {
            Request = "";
            ConversationID = "";
        }

        public string CruiseSailAvail()
        {
            string strRequest = "";
            string strResponse = "";

            //***************************************************************** 
            // Transform OTA CruiseSailAvail Request into Native Amadeus Request * 
            //***************************************************************** 

            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                strRequest = SetRequest("AmadeusWS_CruiseSailAvailRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendCruiseRequestSailingAvailability(ttAA, strRequest);

                //***************************************************************** 
                // Transform Native Amadeus CruiseSailAvail Response into OTA Response * 
                //***************************************************************** 
                try
                {
                    var tagToReplace = "</Cruise_RequestSailingAvailabilityReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CruiseSailAvailRS.xsl", false);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseSailAvail, ex.Message, ttProviderSystems, "");
            }

            return strResponse;
        }

        public string CruiseFareAvail()
        {
            string strRequest = "";
            string strResponse = "";

            //***************************************************************** 
            // Transform OTA CruiseFareAvail Request into Native Amadeus Request * 
            //***************************************************************** 

            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                strRequest = SetRequest("AmadeusWS_CruiseFareAvailRQ.xsl");

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendCruiseRequestFareAvailability(ttAA, strRequest);

                //***************************************************************** 
                // Transform Native Amadeus CruiseFareAvail Response into OTA Response * 
                //***************************************************************** 
                try
                {
                    var tagToReplace = "</Car_SingleAvailability>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CruiseFareAvailRS.xsl", false);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseFareAvail, ex.Message, ttProviderSystems, "");
            }

            return strResponse;
        }

        public string CruiseCategoryAvail()
        {
            string strRequest = "";
            string strResponse = "";

            //***************************************************************** 
            // Transform OTA CruiseCategoryAvail Request into Native Amadeus Request * 
            //***************************************************************** 

            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                strRequest = SetRequest("AmadeusWS_CruiseCategoryAvailRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendCruiseRequestCategoryAvailability(ttAA, strRequest);

                //***************************************************************** 
                // Transform Native Amadeus CruiseCategoryAvail Response into OTA Response * 
                //***************************************************************** 

                try
                {
                    var tagToReplace = "</Cruise_RequestCategoryAvailabilityReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CruiseCategoryAvailRS.xsl", false);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseCategoryAvail, ex.Message, ttProviderSystems, "");
            }

            return strResponse;
        }

        public string CruiseCabinAvail()
        {
            string strRequest = "";
            string strResponse = "";

            //***************************************************************** 
            // Transform OTA CruiseCabinAvail Request into Native Amadeus Request * 
            //***************************************************************** 

            try
            {
                var test = "";
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                strRequest = SetRequest("AmadeusWS_CruiseCabinAvailRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendCruiseRequestCabinAvailability(ttAA, strRequest);

                //***************************************************************** 
                // Transform Native Amadeus CruiseCabinAvail Response into OTA Response * 
                //***************************************************************** 
                try
                {
                    var tagToReplace = "</Cruise_RequestCabinAvailabilityReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CruiseCabinAvailRS.xsl", false);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseCabinAvail, ex.Message, ttProviderSystems);
            }
            return strResponse;
        }

        public string CruiseCabinHold()
        {
            string strRequest = "";
            string strResponse = "";
            //***************************************************************** 
            // Transform OTA CruiseCabinHold Request into Native Amadeus Request * 
            //***************************************************************** 

            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                strRequest = SetRequest("AmadeusWS_CruiseCabinHoldRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendCruiseHoldCabin(ttAA, strRequest);

                //***************************************************************** 
                // Transform Native Amadeus CruiseCabinHold Response into OTA Response * 
                //***************************************************************** 
                try
                {
                    var tagToReplace = "</Cruise_HoldCabinReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CruiseCabinHoldRS.xsl", false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseCabinHold, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }

        public string CruiseCabinUnhold()
        {
            string strRequest = "";
            string strResponse = "";

            //***************************************************************** 
            // Transform OTA CruiseCabinUnhold Request into Native Amadeus Request * 
            //***************************************************************** 

            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                strRequest = SetRequest("AmadeusWS_CruiseCabinHoldRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                strRequest = SetRequest("AmadeusWS_CruiseCabinUnholdRQ.xsl");

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendCruiseUnHoldCabin(ttAA, strRequest);

                //***************************************************************** 
                // Transform Native Amadeus CruiseCabinUnhold Response into OTA Response * 
                //***************************************************************** 
                try
                {
                    var tagToReplace = "</Cruise_UnholdCabinReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CruiseCabinUnholdRS.xsl", false);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseCabinUnhold, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }

        public string CruisePriceBooking()
        {
            string strRequest = "";
            string strResponse = "";
            string strNative = "";
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;

            //***************************************************************** 
            // Transform OTA CruisePriceBooking Request into Native Amadeus Request * 
            //***************************************************************** 

            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                strRequest = SetRequest("AmadeusWS_CruiseCabinHoldRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;
                oNode = oRoot.SelectSingleNode("SailingInfo/SelectedCategory");
                strRequest = CoreLib.TransformXML(strRequest, XslPath, $"{Version}AmadeusWS_CruisePriceBookingRQ.xsl", false);

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendCruisePriceBooking(ttAA, strRequest);

                //***************************************************************** 
                // Transform Native Amadeus CruisePriceBooking Response into OTA Response * 
                //***************************************************************** 
                try
                {
                    var tagToReplace = "</Cruise_PriceBookingReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CruisePriceBookingRS.xsl", false);

                    if (Request.Contains("<System>Production</System>") && strResponse.Contains("<Errors>"))
                    {
                        strRequest = $"<PriceBooking><TripXML>{strRequest}{strResponse}</TripXML><Native>";
                        //CoreLib.SendEmail("Amadeus Cruise Price Error", sb.ToString(), ttProviderSystems.UserID);
                    }
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruisePriceBooking, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }

        public string CruiseCreateBooking()
        {
            string strRequest = "";
            string strResponse = "";
            string strNative = "";

            //***************************************************************** 
            // Transform OTA CruiseCreateBooking Request into Native Amadeus Request * 
            //***************************************************************** 

            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                strRequest = SetRequest("AmadeusWS_CruiseCreateBookingRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendCruiseCreateBooking(ttAA, strRequest);

                //***************************************************************** 
                // Transform Native Amadeus CruiseCreateBooking Response into OTA Response * 
                //***************************************************************** 
                try
                {
                    var tagToReplace = "</Cruise_CreateBookingReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CruiseCreateBookingRS.xsl", false);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruisePriceBooking, ex.Message, ttProviderSystems);
            }
            return strResponse;
        }

        public string CruiseRead()
        {
            string strRequest = "";
            string strResponse = "";
            string ConversationID = "";
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;

            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //***************************************************************** 
                // Transform OTA CruiseRead Request into Native Amadeus Request * 
                //***************************************************************** 
                strRequest = SetRequest("AmadeusWS_CruiseReadRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendRetrievePNR(ttAA);

                //******************************************************** 
                // Get the Line Number and Send a Cruise Read to Amadeus * 
                //******************************************************** 

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                // Air Price Request 
                oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo/elementManagementItinerary[segmentName='CRU']/reference/number");

                if (oNode != null)
                {
                    strRequest = ("<Cruise_GetBookingDetails><agentEnvironment><agentTerminalId>09097451</agentTerminalId></agentEnvironment>");
                    strRequest += "<bookingReference><referenceType>S</referenceType><uniqueReference>";
                    strRequest += oNode.SelectSingleNode("</uniqueReference></bookingReference></Cruise_GetBookingDetails>").InnerText;

                    strResponse = SendCruiseGetBookingDetails(ttAA, strRequest);
                }

                //***************************************************************** 
                // Transform Native Amadeus CruiseRead Response into OTA Response * 
                //***************************************************************** 

                try
                {
                    var tagToReplace = "</Cruise_GetBookingDetailsReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"AmadeusWS_CruiseReadRS.xsl", false);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseRead, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }

        public string CruiseCancelBooking()
        {
            string strRequest = "";
            string strResponse = "";
            string ConversationID = "";
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;

            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //***************************************************************** 
                // Transform OTA CruiseCancel Request into Native Amadeus Request * 
                //***************************************************************** 
                strRequest = SetRequest("AmadeusWS_CruiseCancelRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendRetrievePNR(ttAA, strRequest);

                //******************************************************** 
                // Get the Line Number and Send a Cruise Cancel to Amadeus * 
                //******************************************************** 

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                // Air Price Request 
                oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo/elementManagementItinerary[segmentName='CRU']/reference/number");

                if (oNode != null)
                {
                    strRequest = "<Cruise_CancelBooking><agentEnvironment><agentTerminalId>09097451</agentTerminalId></agentEnvironment>";
                    strRequest += "<bookingReference><referenceType>S</referenceType><uniqueReference>";
                    strRequest += "</uniqueReference></bookingReference>";
                    strRequest += "<bookingQualifier><partyQualifier>8</partyQualifier><componentDetails><componentQualifier>10</componentQualifier>";
                    strRequest += "<componentDescription>TT</componentDescription></componentDetails></bookingQualifier><sailingGroup><sailingDescription><providerDetails><shipCode>";

                    oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']/typicalCruiseData/sailingShipInformation/shipDetails/code");

                    strRequest += oNode.SelectSingleNode("</shipCode><cruiselineCode>").InnerText;

                    oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']/typicalCruiseData/sailingShipInformation/shipDetails/cruiseLineCode");

                    strRequest += oNode.SelectSingleNode("</cruiselineCode></providerDetails><sailingDateTime><sailingDepartureDate>");

                    oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']/typicalCruiseData/sailingDateInformation/beginDateTime/day");

                    oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='CRU']/typicalCruiseData/sailingDateInformation/beginDateTime/year");

                    strRequest += oNode.SelectSingleNode("</sailingDepartureDate><sailingDuration></sailingDuration></sailingDateTime>");
                    strRequest += "<sailingId><cruiseVoyageNbr></cruiseVoyageNbr></sailingId></sailingDescription><currencyInfo><currencyList><currencyQualifier>5</currencyQualifier><currencyIsoCode>";
                    strRequest += "</currencyIsoCode></currencyList></currencyInfo></sailingGroup></Cruise_CancelBooking>";

                    strResponse = SendCruiseCancelBooking(ttAA, strRequest);
                }

                //***************************************************************** 
                // Transform Native Amadeus CruiseCancel Response into OTA Response * 
                //***************************************************************** 

                try
                {
                    var tagToReplace = "</Cruise_CancelBookingReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CruiseCancelRS.xsl", false);
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
            catch (Exception exx)
            {
                throw exx;
            }

            return strResponse;
        }

        public string CruiseModifyBooking()
        {
            string strRequest = "";
            string strResponse = "";
            string ConversationID = "";
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;

            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //***************************************************************** 
                // Transform OTA CruiseModifyBooking Request into Native Amadeus Request * 
                //***************************************************************** 
                strRequest = SetRequest("AmadeusWS_CruiseModifyBookingRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendRetrievePNR(ttAA);

                //******************************************************** 
                // Get the Line Number and Send a Cruise Cancel to Amadeus * 
                //******************************************************** 

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                oNode = oRoot.SelectSingleNode("originDestinationDetails/itineraryInfo/elementManagementItinerary[segmentName='CRU']/reference/number");

                if (oNode != null)
                {
                    strRequest = "<Cruise_GetBookingDetails><agentEnvironment><agentTerminalId>12345678</agentTerminalId></agentEnvironment>";
                    strRequest += "<bookingReference><referenceType>S</referenceType><uniqueReference>";
                    strRequest += oNode.InnerText;
                    strRequest += "</uniqueReference></bookingReference></Cruise_GetBookingDetails>";

                    strResponse = SendCruiseGetBookingDetails(ttAA, strRequest);

                    strRequest = strRequest.Replace("Cruise_CreateBooking", "Cruise_ModifyBooking");
                    strRequest += "</agentEnvironment><bookingReference><referenceType>S</referenceType><uniqueReference>";
                    strRequest += oNode.InnerText;
                    strRequest += "</uniqueReference></bookingReference>";
                    //strRequest = strRequest.Replace("</agentEnvironment>", sb.ToString());

                    strResponse = SendCruiseModifyBooking(ttAA, strRequest);
                }

                //***************************************************************** 
                // Transform Native Amadeus CruiseModifyBooking Response into OTA Response * 
                //***************************************************************** 

                try
                {
                    var tagToReplace = "</Cruise_ModifyBookingReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CruiseCreateBookingRS.xsl", false);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseModifyBooking, ex.Message, ttProviderSystems);
            }
            return strResponse;
        }

        public string CruisePackageAvail()
        {
            string strRequest = "";
            string strResponse = "";

            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //***************************************************************** 
                // Transform OTA CruisePackageAvail Request into Native Amadeus Request * 
                //***************************************************************** 
                strRequest = SetRequest("AmadeusWS_CruiseModifyBookingRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendCruiseRequestPrePostPackageAvailability(ttAA, strRequest);

                //***************************************************************** 
                // Transform Native Amadeus CruisePackageAvail Response into OTA Response * 
                //***************************************************************** 

                try
                {
                    var tagToReplace = "</Cruise_RequestPrePostPackageAvailabilityReply";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CruisePackageAvailRS.xsl", false);


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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseModifyBooking, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }

        public string CruisePackageDesc()
        {
            string strRequest = "";
            string strResponse = "";

            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //***************************************************************** 
                // Transform OTA CruisePackageDesc Request into Native Amadeus Request * 
                //***************************************************************** 
                strRequest = SetRequest("AmadeusWS_CruisePackageDescRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendCruiseRequestDisplayPrePostPackageDescription(ttAA, strRequest);

                //***************************************************************** 
                // Transform Native Amadeus CruisePackageDesc Response into OTA Response * 
                //***************************************************************** 
                try
                {
                    var tagToReplace = "</Cruise_DisplayPrePostPackageDescriptionReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CruisePackageDescRS.xsl", false);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruisePackageDesc, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }

        public string CruiseTransferAvail()
        {
            string strRequest = "";
            string strResponse = "";

            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //***************************************************************** 
                // Transform OTA CruiseTransferAvail Request into Native Amadeus Request * 
                //***************************************************************** 
                strRequest = SetRequest("AmadeusWS_CruiseTransferAvailRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendCruiseRequestTransferAvailability(ttAA, strRequest);

                //***************************************************************** 
                // Transform Native Amadeus CruiseTransferAvail Response into OTA Response * 
                //***************************************************************** 
                try
                {
                    var tagToReplace = "</Cruise_RequestTransferAvailabilityReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CruiseTransferAvailRS.xsl", false);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseTransferAvail, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }

        public string CruiseItineraryDesc()
        {
            string strRequest = "";
            string strResponse = "";

            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //***************************************************************** 
                // Transform OTA CruiseItineraryDesc Request into Native Amadeus Request * 
                //***************************************************************** 
                strRequest = SetRequest("AmadeusWS_CruiseItineraryDescRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //******************************************************************************* 
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response * 
                //******************************************************************************* 
                strResponse = SendCruiseDisplayItineraryDescription(ttAA, strRequest);

                //***************************************************************** 
                // Transform Native Amadeus CruiseItineraryDesc Response into OTA Response * 
                //***************************************************************** 

                try
                {
                    var tagToReplace = "</Cruise_DisplayItineraryDescriptionReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CruiseItineraryDescRS.xsl", false);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CruiseItineraryDesc, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }
    }
}