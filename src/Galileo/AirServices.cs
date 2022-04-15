using System;
using System.Text;
using System.Xml;
using TripXMLMain;

namespace Galileo
{
    public class AirServices : GalileoBase
    {
        private string[,] combinations = new string[600, 16];
        private string[,] inbound_comb = new string[600, 16];
        private int count = 0;
        private bool dup_check = false;
        private bool dup_verifier = false;
        private int turn = 0;
        private int unique_outbounds = 0;

        public AirServices()
        {
            Request = "";
            ConversationID = "";
        }

        public string AirAvail()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA AirAvail Request into Native Galileo Request     *
            // ***************************************************************** 

            try
            {

                string strRequest = SetRequest("Galileo_AirAvailRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 

                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);

                // *****************************************************************
                // Transform Native Galileo AirAvail Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    if (strResponse.Length > 1500)
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response I", strResponse.Substring(0, (int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response II", strResponse.Substring((int)Math.Round(strResponse.Length / 2d)), ProviderSystems.LogUUID);
                    }
                    else
                    {
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response I", strResponse, ProviderSystems.LogUUID);
                    }

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_AirAvailRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.AirAvail, ex.Message, ProviderSystems);
            }

            return strResponse;

        }

        public string AirFlifo()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA AirFlifo Request into Native Galileo Request     *
            // ***************************************************************** 

            try
            {

                string strRequest = SetRequest("Galileo_AirFlifoRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 

                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);


                // *****************************************************************
                // Transform Native Galileo AirFlifo Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_AirFlifoRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.AirFlifo, ex.Message, ProviderSystems);

            }
            return strResponse;

        }

        public string LowFare()
        {

            string strResponse;
            // *****************************************************************
            // Transform OTA LowFare Request into Native Galileo Request     *
            // ***************************************************************** 

            try
            {
                string strRequest = SetRequest("Galileo_LowFareRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 

                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);

                // *****************************************************************
                // Transform Native Galileo LowFare Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = strResponse.Replace("</FareQuoteSuperBB_9>", $"{Request}</FareQuoteSuperBB_9>");
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_LowFareRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.LowFare, ex.Message, ProviderSystems);

            }
            return strResponse;
        }

        public string AirSeatMap()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA AirSeatMap Request into Native Galileo Request     *
            // ***************************************************************** 
            try
            {
                string strRequest = SetRequest("Galileo_AirSeatMapRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);


                // *****************************************************************
                // Transform Native Galileo AirSeatMap Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_AirSeatMapRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.AirSeatMap, ex.Message, ProviderSystems);

            }
            return strResponse;
        }

        public string AirPrice()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA AirSeatMap Request into Native Galileo Request     *
            // ***************************************************************** 
            try
            {
                string strRequest = SetRequest("Galileo_AirPriceRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);

                // *****************************************************************
                // Transform Native Galileo AirPrice Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_AirPriceRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.AirPrice, ex.Message, ProviderSystems);
            }
            return strResponse;
        }

        public string AirRules()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA AirSeatMap Request into Native Galileo Request     *
            // ***************************************************************** 
            try
            {
                string strRequest = SetRequest("Galileo_AirRulesRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                var oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                var oRoot = oDoc.DocumentElement;

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter();
                bool inSession = SetConversationID(ttGA);

                // *******************************************************************
                // Send Transformed Request AirRules Tariff to the Galileo Adapter  *
                // ******************************************************************* 
                strResponse = ttGA.SendMessage(strRequest);

                if (strResponse.IndexOf("NO FARES FOUND FOR INPUT REQUEST") == -1)
                {
                    // *******************************************************************
                    // Send Transformed Request AirRules Tariff to the Galileo Adapter  *
                    // ******************************************************************* 
                    // AirRules Request
                    var oNode = oRoot.SelectSingleNode("FareQuoteRulesDisplay_8_0");
                    strRequest = oNode.OuterXml;
                    strResponse = ttGA.SendMessage(strRequest);
                }

                // *****************************************************************
                // Transform Native Galileo AirRules Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_AirRulesRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.AirRules, ex.Message, ProviderSystems);
            }
            return strResponse;
        }

        public string LowFarePlus()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA LowFarePlus Request into Native Galileo Request     *
            // ***************************************************************** 
            try
            {
                string strRequest = SetRequest("Galileo_LowFarePlusRQ1.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter(ProviderSystems.SessionPool ? "V1" : "");
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);

                // *****************************************************************
                // Transform Native Galileo LowFarePlus Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    strResponse = strResponse.Replace("</FareQuoteSuperBB_9>", $"{Request}</FareQuoteSuperBB_9>").Replace(" xmlns=\"\"", "");
                    strResponse = strResponse.Replace("</FareQuoteSuperBB_25>", $"{Request}</FareQuoteSuperBB_25>").Replace(" xmlns=\"\"", "");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_LowFarePlusRS1.xsl");
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_LowFarePlusRS2.xsl");
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_LowFarePlusRS3.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.LowFarePlus, ex.Message, ProviderSystems);
            }
            return strResponse;
        }

        public string LowFareSchedule()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA LowFarePlus Request into Native Galileo Request     *
            // ***************************************************************** 
            try
            {
                string strRequest = SetRequest("Galileo_LowFareScheduleRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter(ProviderSystems.SessionPool ? "V1" : "");
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);

                // *****************************************************************
                // Transform Native Galileo LowFarePlus Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    strResponse = strResponse.Replace("</FareQuoteSuperBB_9>", $"{Request}</FareQuoteSuperBB_9>").Replace(" xmlns=\"\"", "");
                    strResponse = strResponse.Replace("</FareQuoteSuperBB_25>", $"{Request}</FareQuoteSuperBB_25>").Replace(" xmlns=\"\"", "");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_LowFarePlusRS1.xsl");
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_LowFarePlusRS2.xsl");
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_LowFarePlusRS3.xsl");

                    var doc = new XmlDocument();
                    doc.LoadXml(strResponse);
                    workOnPricedItinerary(doc);
                    strResponse = createXml(doc);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.LowFareSchedule, ex.Message, ProviderSystems);
            }
            return strResponse;
        }

        private void workOnPricedItinerary(XmlDocument doc)
        {
            var sb_rezBookDesigCode_1 = new StringBuilder();
            var sb_rezBookDesigCode_2 = new StringBuilder();
            var sb_rezBookDesigCode_3 = new StringBuilder();
            var sb_AL_CodeMak_1 = new StringBuilder();
            var sb_AL_CodeMak_2 = new StringBuilder();
            var sb_AL_CodeMak_3 = new StringBuilder();
            var sb_DepDateTime_1 = new StringBuilder();
            var sb_DepDateTime_2 = new StringBuilder();
            var sb_DepDateTime_3 = new StringBuilder();
            var xmlNL = doc.GetElementsByTagName("PricedItinerary");
            count = xmlNL.Count;

            int index = 0;
            int c = 0;
            int filghtNo_1 = 0;
            int filghtNo_2 = 0;
            int filghtNo_3 = 0;
            int d = 0;
            var loopTo = count - 1;
            for (int x = 0; x <= loopTo; x++)
            {
                c = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS ").SelectSingleNode("PricedItineraries ").SelectNodes("PricedItinerary ")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment").Count;
                int inBoundFlightCount = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS ").SelectSingleNode("PricedItineraries ").SelectNodes("PricedItinerary ")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment").Count;

                // 2012/11/21 modification starts

                if (c == 1)
                {
                    filghtNo_1 = Convert.ToInt32(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].Attributes["FlightNumber"].Value);
                    sb_rezBookDesigCode_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].Attributes["ResBookDesigCode"].Value);
                    sb_AL_CodeMak_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                    sb_DepDateTime_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].Attributes["DepartureDateTime"].Value);
                    dup_verifier = duplicantChecker_1(filghtNo_1, sb_rezBookDesigCode_1.ToString(), sb_AL_CodeMak_1.ToString(), sb_DepDateTime_1.ToString());
                    if (dup_verifier == false)
                    {
                        combinations[x, 0] = filghtNo_1.ToString();
                        combinations[x, 4] = sb_rezBookDesigCode_1.ToString();
                        combinations[x, 8] = sb_AL_CodeMak_1.ToString();
                        combinations[x, 12] = sb_DepDateTime_1.ToString();
                    }
                }

                // 2012/11/21 modification ends...

                // should call the method which decides whether the new values are almost there in the array
                else if (c == 2)
                {
                    filghtNo_1 = Convert.ToInt32(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].Attributes["FlightNumber"].Value);
                    filghtNo_2 = Convert.ToInt32(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[1].Attributes["FlightNumber"].Value);
                    sb_rezBookDesigCode_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].Attributes["ResBookDesigCode"].Value);
                    sb_rezBookDesigCode_2.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[1].Attributes["ResBookDesigCode"].Value);
                    sb_AL_CodeMak_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                    sb_AL_CodeMak_2.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[1].SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                    sb_DepDateTime_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].Attributes["DepartureDateTime"].Value);
                    sb_DepDateTime_2.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[1].Attributes["DepartureDateTime"].Value);
                    dup_verifier = duplicantChecker(filghtNo_1, filghtNo_2, sb_rezBookDesigCode_1.ToString(), sb_rezBookDesigCode_2.ToString(), sb_AL_CodeMak_1.ToString(), sb_AL_CodeMak_2.ToString(), sb_DepDateTime_1.ToString(), sb_DepDateTime_2.ToString());
                    if (dup_verifier == false)
                    {
                        combinations[x, 0] = filghtNo_1.ToString();
                        combinations[x, 1] = filghtNo_2.ToString();
                        combinations[x, 4] = sb_rezBookDesigCode_1.ToString();
                        combinations[x, 5] = sb_rezBookDesigCode_2.ToString();
                        combinations[x, 8] = sb_AL_CodeMak_1.ToString();
                        combinations[x, 9] = sb_AL_CodeMak_2.ToString();
                        combinations[x, 12] = sb_DepDateTime_1.ToString();
                        combinations[x, 13] = sb_DepDateTime_2.ToString();
                    }
                }
                else if (c == 3)
                {
                    filghtNo_1 = Convert.ToInt32(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].Attributes["FlightNumber"].Value);
                    filghtNo_2 = Convert.ToInt32(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[1].Attributes["FlightNumber"].Value);
                    filghtNo_3 = Convert.ToInt32(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[2].Attributes["FlightNumber"].Value);
                    sb_rezBookDesigCode_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].Attributes["ResBookDesigCode"].Value);
                    sb_rezBookDesigCode_2.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[1].Attributes["ResBookDesigCode"].Value);
                    sb_rezBookDesigCode_3.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[2].Attributes["ResBookDesigCode"].Value);
                    sb_AL_CodeMak_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].SelectSingleNode("MarketingAirline").Attributes["Code"].Value.ToString());
                    sb_AL_CodeMak_2.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[1].SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                    sb_AL_CodeMak_3.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[2].SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                    sb_DepDateTime_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].Attributes["DepartureDateTime"].Value);
                    sb_DepDateTime_2.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[1].Attributes["DepartureDateTime"].Value);
                    sb_DepDateTime_3.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[2].Attributes["DepartureDateTime"].Value);
                    var dup_verifier = duplicantChecker_3(filghtNo_1, filghtNo_2, filghtNo_3, sb_rezBookDesigCode_1.ToString(), sb_rezBookDesigCode_2.ToString(), sb_rezBookDesigCode_3.ToString(), sb_AL_CodeMak_1.ToString(), sb_AL_CodeMak_2.ToString(), sb_AL_CodeMak_3.ToString(), sb_DepDateTime_1.ToString(), sb_DepDateTime_2.ToString(), sb_DepDateTime_3.ToString());
                    if (dup_verifier == false)
                    {
                        combinations[x, 0] = filghtNo_1.ToString();
                        combinations[x, 1] = filghtNo_2.ToString();
                        combinations[x, 2] = filghtNo_3.ToString();
                        combinations[x, 4] = sb_rezBookDesigCode_1.ToString();
                        combinations[x, 5] = sb_rezBookDesigCode_2.ToString();
                        combinations[x, 6] = sb_rezBookDesigCode_3.ToString();
                        combinations[x, 8] = sb_AL_CodeMak_1.ToString();
                        combinations[x, 9] = sb_AL_CodeMak_2.ToString();
                        combinations[x, 10] = sb_AL_CodeMak_3.ToString();
                        combinations[x, 12] = sb_DepDateTime_1.ToString();
                        combinations[x, 13] = sb_DepDateTime_2.ToString();
                        combinations[x, 14] = sb_DepDateTime_3.ToString();
                    }
                }

                sb_rezBookDesigCode_1.Remove(0, sb_rezBookDesigCode_1.Length);
                sb_rezBookDesigCode_2.Remove(0, sb_rezBookDesigCode_2.Length);
                sb_rezBookDesigCode_3.Remove(0, sb_rezBookDesigCode_3.Length);
                sb_AL_CodeMak_1.Remove(0, sb_AL_CodeMak_1.Length);
                sb_AL_CodeMak_2.Remove(0, sb_AL_CodeMak_2.Length);
                sb_AL_CodeMak_3.Remove(0, sb_AL_CodeMak_3.Length);
                sb_DepDateTime_1.Remove(0, sb_DepDateTime_1.Length);
                sb_DepDateTime_2.Remove(0, sb_DepDateTime_2.Length);
                sb_DepDateTime_3.Remove(0, sb_DepDateTime_3.Length);
                if (inBoundFlightCount == 1)
                {
                    filghtNo_1 = Convert.ToInt32(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[0].Attributes["FlightNumber"].Value);
                    sb_rezBookDesigCode_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[0].Attributes["ResBookDesigCode"].Value);
                    sb_AL_CodeMak_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[0].SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                    sb_DepDateTime_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[0].Attributes["DepartureDateTime"].Value);


                    // fillinf in to inbound_comb array

                    inbound_comb[x, 0] = filghtNo_1.ToString();
                    inbound_comb[x, 4] = sb_rezBookDesigCode_1.ToString();
                    inbound_comb[x, 8] = sb_AL_CodeMak_1.ToString();
                    inbound_comb[x, 12] = sb_DepDateTime_1.ToString();
                }
                else if (inBoundFlightCount == 2)
                {
                    filghtNo_1 = Convert.ToInt32(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[0].Attributes["FlightNumber"].Value);
                    filghtNo_2 = Convert.ToInt32(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[1].Attributes["FlightNumber"].Value);
                    sb_rezBookDesigCode_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[0].Attributes["ResBookDesigCode"].Value);
                    sb_rezBookDesigCode_2.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[1].Attributes["ResBookDesigCode"].Value);
                    sb_AL_CodeMak_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[0].SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                    sb_AL_CodeMak_2.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[1].SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                    sb_DepDateTime_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[0].Attributes["DepartureDateTime"].Value);
                    sb_DepDateTime_2.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[1].Attributes["DepartureDateTime"].Value);

                    // fillinf in to inbound_comb array
                    inbound_comb[x, 0] = filghtNo_1.ToString();
                    inbound_comb[x, 1] = filghtNo_2.ToString();
                    inbound_comb[x, 4] = sb_rezBookDesigCode_1.ToString();
                    inbound_comb[x, 5] = sb_rezBookDesigCode_2.ToString();
                    inbound_comb[x, 8] = sb_AL_CodeMak_1.ToString();
                    inbound_comb[x, 9] = sb_AL_CodeMak_2.ToString();
                    inbound_comb[x, 12] = sb_DepDateTime_1.ToString();
                    inbound_comb[x, 13] = sb_DepDateTime_2.ToString();
                }
                else if (inBoundFlightCount == 3)
                {
                    filghtNo_1 = Convert.ToInt32(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[0].Attributes["FlightNumber"].Value);
                    filghtNo_2 = Convert.ToInt32(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[1].Attributes["FlightNumber"].Value);
                    filghtNo_3 = Convert.ToInt32(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[2].Attributes["FlightNumber"].Value);
                    sb_rezBookDesigCode_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[0].Attributes["ResBookDesigCode"].Value);
                    sb_rezBookDesigCode_2.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[1].Attributes["ResBookDesigCode"].Value);
                    sb_rezBookDesigCode_3.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[2].Attributes["ResBookDesigCode"].Value);
                    sb_AL_CodeMak_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[0].SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                    sb_AL_CodeMak_2.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[1].SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                    sb_AL_CodeMak_3.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[2].SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                    sb_DepDateTime_1.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[0].Attributes["DepartureDateTime"].Value);
                    sb_DepDateTime_2.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[1].Attributes["DepartureDateTime"].Value);
                    sb_DepDateTime_3.Append(doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[x].SelectSingleNode("AirItinerary ").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[2].Attributes["DepartureDateTime"].Value);

                    // fillinf in to inbound_comb array

                    inbound_comb[x, 0] = filghtNo_1.ToString();
                    inbound_comb[x, 1] = filghtNo_2.ToString();
                    inbound_comb[x, 2] = filghtNo_3.ToString();
                    inbound_comb[x, 4] = sb_rezBookDesigCode_1.ToString();
                    inbound_comb[x, 5] = sb_rezBookDesigCode_2.ToString();
                    inbound_comb[x, 6] = sb_rezBookDesigCode_3.ToString();
                    inbound_comb[x, 8] = sb_AL_CodeMak_1.ToString();
                    inbound_comb[x, 9] = sb_AL_CodeMak_2.ToString();
                    inbound_comb[x, 10] = sb_AL_CodeMak_3.ToString();
                    inbound_comb[x, 12] = sb_DepDateTime_1.ToString();
                    inbound_comb[x, 13] = sb_DepDateTime_2.ToString();
                    inbound_comb[x, 14] = sb_DepDateTime_3.ToString();
                }

                // string builder object removals

                sb_rezBookDesigCode_1.Remove(0, sb_rezBookDesigCode_1.Length);
                sb_rezBookDesigCode_2.Remove(0, sb_rezBookDesigCode_2.Length);
                sb_rezBookDesigCode_3.Remove(0, sb_rezBookDesigCode_3.Length);
                sb_AL_CodeMak_1.Remove(0, sb_AL_CodeMak_1.Length);
                sb_AL_CodeMak_2.Remove(0, sb_AL_CodeMak_2.Length);
                sb_AL_CodeMak_3.Remove(0, sb_AL_CodeMak_3.Length);
                sb_DepDateTime_1.Remove(0, sb_DepDateTime_1.Length);
                sb_DepDateTime_2.Remove(0, sb_DepDateTime_2.Length);
                sb_DepDateTime_3.Remove(0, sb_DepDateTime_3.Length);
                dup_verifier = false;
                dup_check = false;
            }
        }

        private bool duplicantChecker(int filghtNo_1, int filghtNo_2, string rezBookDesigCode_1, string rezBookDesigCode_2, string AL_CodeMak_1, string AL_CodeMak_2, string DepDateTime_1, string DepDateTime_2)
        {
            int ind = 0;
            while (ind <= count - 1 && dup_check != true)
            {
                if (filghtNo_1.ToString().Equals(combinations[ind, 0]) && filghtNo_2.ToString().Equals(combinations[ind, 1]) && rezBookDesigCode_1.Equals(combinations[ind, 4]) && rezBookDesigCode_2.Equals(combinations[ind, 5]) && AL_CodeMak_1.Equals(combinations[ind, 8]) && AL_CodeMak_2.Equals(combinations[ind, 9]) && DepDateTime_1.Equals(combinations[ind, 12]) && DepDateTime_2.Equals(combinations[ind, 13]))
                {
                    dup_check = true;
                }

                ind++;
            }

            return dup_check;
        }

        private bool duplicantChecker_1(int flightNo_1, string rezBookDesigCode_1, string AL_CodeMak_1, string DepDateTime_1)
        {
            int ind = 0;
            while (ind <= count - 1 && dup_check != true)
            {
                if (flightNo_1.ToString().Equals(combinations[ind, 0]) && rezBookDesigCode_1.Equals(combinations[ind, 4]) && AL_CodeMak_1.Equals(combinations[ind, 8]) && DepDateTime_1.Equals(combinations[ind, 12]))
                {
                    dup_check = true;
                }

                ind++;
            }

            return dup_check;
        }

        private bool duplicantChecker_3(int filghtNo_1, int filghtNo_2, int filghtNo_3, string rezBookDesigCode_1, string rezBookDesigCode_2, string rezBookDesigCode_3, string AL_CodeMak_1, string AL_CodeMak_2, string AL_CodeMak_3, string DepDateTime_1, string DepDateTime_2, string DepDateTime_3)
        {
            int z = 0;
            while (z <= count - 1 && dup_check != true)
            {
                if (filghtNo_1.ToString().Equals(combinations[z, 0]) && filghtNo_2.ToString().Equals(combinations[z, 1]) && filghtNo_3.ToString().Equals(combinations[z, 2]) && rezBookDesigCode_1.Equals(combinations[z, 4]) && rezBookDesigCode_2.Equals(combinations[z, 5]) && rezBookDesigCode_3.Equals(combinations[z, 6]) && AL_CodeMak_1.Equals(combinations[z, 8]) && AL_CodeMak_2.Equals(combinations[z, 9]) && AL_CodeMak_3.Equals(combinations[z, 10]) && DepDateTime_1.Equals(combinations[z, 12]) && DepDateTime_2.Equals(combinations[z, 13]) && DepDateTime_3.Equals(combinations[z, 14]))
                {
                    dup_check = true;
                }
                z++;
            }

            return dup_check;
        }

        private string createXml(XmlDocument doc)
        {
            string strResponse;
            try
            {
                int inbound_flg_count = 0;
                var doc_response = new XmlDocument();
                XmlNode Root = null;
                XmlNode node = null;
                XmlNode priceItinarary = null;
                XmlNode nItem = null;
                XmlAttribute Attribute = null;
                var sb2 = new StringBuilder();
                object flight_val_in = 0;
                int pct_fairdowncount = 0;
                int flightSeg_Counter = 0;
                doc_response.LoadXml("<OTA_AirLowFareSearchScheduleRS><Success/><PricedItineraries></PricedItineraries></OTA_AirLowFareSearchScheduleRS>");
                Console.WriteLine("****************** Came in side the <OTA_AirLowFareSearchPlusRS> ***********************");
                Root = doc_response.DocumentElement;
                Attribute = doc_response.CreateAttribute("Version");
                Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS").Attributes["Version"].Value;
                Root.Attributes.Append(Attribute);
                Attribute = doc_response.CreateAttribute("TransactionIdentifier");
                Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS").Attributes["TransactionIdentifier"].Value;
                Root.Attributes.Append(Attribute);


                // ******************** Working on Priced Itineraries ***********************

                var priceItinararies = doc_response.SelectSingleNode("OTA_AirLowFareSearchScheduleRS/PricedItineraries");
                int seqNo = 0;
                for (int y = 1, loopTo = count; y <= loopTo; y++)
                {
                    if (combinations[y - 1, 0] is object)
                    {
                        seqNo += 1;
                        XmlNode pi = doc_response.CreateElement("PricedItinerary");
                        priceItinararies.AppendChild(pi);
                        Attribute = doc_response.CreateAttribute("SequenceNumber");
                        Attribute.Value = seqNo.ToString();
                        pi.Attributes.Append(Attribute);
                        XmlNode airItinerary = doc_response.CreateElement("AirItinerary");
                        pi.AppendChild(airItinerary);
                        XmlNode Odo_node = doc_response.CreateElement("OriginDestinationOptions");
                        airItinerary.AppendChild(Odo_node);
                        XmlNode nw_nItem = doc_response.CreateElement("OriginDestinationOption");
                        Odo_node.AppendChild(nw_nItem);

                        // **********************************************
                        // *    Printing the out bound tags             *
                        // **********************************************
                        // considerring if the out bound consists only two filght items
                        if (combinations[y - 1, 1] is null)
                        {
                            turn = 1;
                        }
                        else if (combinations[y - 1, 2] is null)
                        {
                            turn = 2;
                        }
                        else
                        {
                            turn = 3;
                        }
                        // #Region "Loop for printing OutBound Flight Segments"
                        // ***********************
                        // For loop for priniting Flight Segments (OUT_Bounds)
                        // ***********************
                        int fl_No_OutBound = 0;
                        flightSeg_Counter = 0;
                        for (int k = 1, loopTo1 = turn; k <= loopTo1; k++)
                        {
                            flightSeg_Counter += 1;
                            node = doc_response.CreateElement("FlightSegment");
                            nw_nItem.AppendChild(node);
                            Attribute = doc_response.CreateAttribute("DepartureDateTime");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[k - 1].Attributes["DepartureDateTime"].Value;
                            node.Attributes.Append(Attribute);
                            Attribute = doc_response.CreateAttribute("ArrivalDateTime");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[k - 1].Attributes["ArrivalDateTime"].Value;
                            node.Attributes.Append(Attribute);
                            Attribute = doc_response.CreateAttribute("RPH");
                            Attribute.Value = "1";
                            node.Attributes.Append(Attribute);
                            Attribute = doc_response.CreateAttribute("FlightNumber");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[k - 1].Attributes["FlightNumber"].Value;
                            fl_No_OutBound = int.Parse(Attribute.Value);
                            node.Attributes.Append(Attribute);
                            Attribute = doc_response.CreateAttribute("ResBookDesigCode");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[k - 1].Attributes["ResBookDesigCode"].Value;
                            node.Attributes.Append(Attribute);
                            Attribute = doc_response.CreateAttribute("NumberInParty");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[k - 1].Attributes["NumberInParty"].Value;
                            node.Attributes.Append(Attribute);
                            Attribute = doc_response.CreateAttribute("E_TicketEligibility");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[k - 1].Attributes["E_TicketEligibility"].Value;
                            node.Attributes.Append(Attribute);
                            nItem = doc_response.CreateElement("DepartureAirport");
                            node.AppendChild(nItem);
                            Attribute = doc_response.CreateAttribute("LocationCode");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[k - 1].SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value;
                            nItem.Attributes.Append(Attribute);
                            nItem = doc_response.CreateElement("ArrivalAirport");
                            node.AppendChild(nItem);
                            Attribute = doc_response.CreateAttribute("LocationCode");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[k - 1].SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value;
                            nItem.Attributes.Append(Attribute);
                            nItem = doc_response.CreateElement("OperatingAirline");
                            Attribute = doc_response.CreateAttribute("Code");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[k - 1].SelectSingleNode("OperatingAirline").Attributes["Code"].Value;
                            nItem.Attributes.Append(Attribute);
                            node.AppendChild(nItem);
                            nItem = doc_response.CreateElement("Equipment");
                            Attribute = doc_response.CreateAttribute("AirEquipType");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[k - 1].SelectSingleNode("Equipment").Attributes["AirEquipType"].Value;
                            nItem.Attributes.Append(Attribute);
                            node.AppendChild(nItem);
                            nItem = doc_response.CreateElement("MarketingAirline");
                            Attribute = doc_response.CreateAttribute("Code");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[k - 1].SelectSingleNode("MarketingAirline").Attributes["Code"].Value;
                            nItem.Attributes.Append(Attribute);
                            node.AppendChild(nItem);
                            nItem = doc_response.CreateElement("TPA_Extensions");
                            Attribute = doc_response.CreateAttribute("PricingSource");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[k - 1].SelectSingleNode("TPA_Extensions").Attributes["PricingSource"].Value;
                            nItem.Attributes.Append(Attribute);
                            node.AppendChild(nItem);
                            node = doc_response.CreateElement("JourneyTotalDuration");
                            node.InnerText = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[k - 1].SelectSingleNode("TPA_Extensions").SelectSingleNode("JourneyTotalDuration").InnerText;
                            nItem.AppendChild(node);
                            var node_2 = doc_response.CreateElement("FromTotalBaseFare");
                            nItem.AppendChild(node_2);
                            Attribute = doc_response.CreateAttribute("Amount");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["Amount"].Value;
                            // Attribute.Value = "1000"
                            node_2.Attributes.Append(Attribute);
                            Attribute = doc_response.CreateAttribute("CurrencyCode");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["CurrencyCode"].Value;
                            // Attribute.Value = "USD"
                            node_2.Attributes.Append(Attribute);
                            Attribute = doc_response.CreateAttribute("DecimalPlaces");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["DecimalPlaces"].Value;
                            // Attribute.Value = "2"
                            node_2.Attributes.Append(Attribute);
                            var node_3 = doc_response.CreateElement("FromTotalTax");
                            nItem.AppendChild(node_3);
                            Attribute = doc_response.CreateAttribute("Amount");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").SelectSingleNode("Tax").Attributes["Amount"].Value;
                            // Attribute.Value = "1000"
                            node_3.Attributes.Append(Attribute);
                            Attribute = doc_response.CreateAttribute("CurrencyCode");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").SelectSingleNode("Tax").Attributes["CurrencyCode"].Value;
                            // Attribute.Value = "USD"
                            node_3.Attributes.Append(Attribute);
                            Attribute = doc_response.CreateAttribute("DecimalPlaces");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").SelectSingleNode("Tax").Attributes["DecimalPlaces"].Value;
                            // Attribute.Value = "2"
                            node_3.Attributes.Append(Attribute);
                            var node_4 = doc_response.CreateElement("FromTotalFare");
                            nItem.AppendChild(node_4);
                            Attribute = doc_response.CreateAttribute("PricingSource");
                            Attribute.Value = "Private";
                            node_4.Attributes.Append(Attribute);
                            Attribute = doc_response.CreateAttribute("Amount");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("TotalFare").Attributes["Amount"].Value;
                            // Attribute.Value = "1000"
                            node_4.Attributes.Append(Attribute);
                            Attribute = doc_response.CreateAttribute("CurrencyCode");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("TotalFare").Attributes["CurrencyCode"].Value;
                            // Attribute.Value = "USD"
                            node_4.Attributes.Append(Attribute);
                            Attribute = doc_response.CreateAttribute("DecimalPlaces");
                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("TotalFare").Attributes["DecimalPlaces"].Value;
                            // Attribute.Value = "2"
                            node_4.Attributes.Append(Attribute);
                            // nItem.AppendChild(node)

                        }

                        XmlNode tickInfo = doc_response.CreateElement("TicketingInfo");
                        pi.AppendChild(tickInfo);
                        Attribute = doc_response.CreateAttribute("TicketTimeLimit");
                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("TicketingInfo").Attributes["TicketTimeLimit"].Value;
                        tickInfo.Attributes.Append(Attribute);
                        XmlNode inbound = doc_response.CreateElement("OriginDestinationOption");
                        Odo_node.AppendChild(inbound);
                        for (int m = 1, loopTo2 = count; m <= loopTo2; m++)
                        {

                            // matching outbounds with one flight items againsat the in bounds
                            if (combinations[y - 1, 1] is null && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment").Count == 1)
                            {
                                string xml_flightNo1 = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].Attributes["FlightNumber"].Value.ToString();
                                string comb_flightNo1 = combinations[y - 1, 0];
                                xml_flightNo1 = digit_checker(xml_flightNo1, comb_flightNo1);
                                comb_flightNo1 = digit_checker(comb_flightNo1, xml_flightNo1);
                                int new_flNo = 0;
                                if (xml_flightNo1.Equals(comb_flightNo1) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].Attributes["ResBookDesigCode"].Value.ToString().Equals(combinations[y - 1, 4]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[0].SelectSingleNode("MarketingAirline").Attributes["Code"].Value.ToString().Equals(combinations[y - 1, 8]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[0].Attributes["DepartureDateTime"].Value.ToString().Equals(combinations[y - 1, 12]))
                                {
                                    int r = 0;
                                    inbound_flg_count += 1;
                                    if (inbound_comb[m - 1, 1] is null)
                                    {
                                        r = 1;
                                    }
                                    else if (inbound_comb[m - 1, 2] is null && inbound_comb[m - 1, 3] is null)
                                    {
                                        r = 2;
                                    }
                                    else if (inbound_comb[m - 1, 3] is null)
                                    {
                                        r = 3;
                                    }

                                    for (int e = 1, loopTo3 = r; e <= loopTo3; e++)
                                    {
                                        node = doc_response.CreateElement("FlightSegment");
                                        inbound.AppendChild(node);
                                        Attribute = doc_response.CreateAttribute("DepartureDateTime");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["DepartureDateTime"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("ArrivalDateTime");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["ArrivalDateTime"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("RPH");
                                        Attribute.Value = inbound_flg_count.ToString();
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("FlightNumber");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["FlightNumber"].Value;
                                        new_flNo = int.Parse(Attribute.Value);
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("ResBookDesigCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["ResBookDesigCode"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("NumberInParty");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["NumberInParty"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("E_TicketEligibility");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["E_TicketEligibility"].Value;
                                        node.Attributes.Append(Attribute);

                                        // ************************************************
                                        // ' tags in side the filgtItem tag of inbounds
                                        // ************************************************

                                        nItem = doc_response.CreateElement("DepartureAirport");
                                        Attribute = doc_response.CreateAttribute("LocationCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("ArrivalAirport");
                                        Attribute = doc_response.CreateAttribute("LocationCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("OperatingAirline");
                                        Attribute = doc_response.CreateAttribute("Code");

                                        // erroe line
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("OperatingAirline").Attributes["Code"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("Equipment");
                                        Attribute = doc_response.CreateAttribute("AirEquipType");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("Equipment").Attributes["AirEquipType"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("MarketingAirline");
                                        Attribute = doc_response.CreateAttribute("Code");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("MarketingAirline").Attributes["Code"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("TPA_Extensions");
                                        Attribute = doc_response.CreateAttribute("PricingSource");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("TPA_Extensions").Attributes["PricingSource"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        node = doc_response.CreateElement("CabinType");
                                        Attribute = doc_response.CreateAttribute("Cabin");
                                        Attribute.Value = "Economy";
                                        node.Attributes.Append(Attribute);
                                        nItem.AppendChild(node);
                                        node = doc_response.CreateElement("JourneyTotalDuration");
                                        node = doc_response.CreateElement("JourneyTotalDuration");
                                        node.InnerText = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment ")[e - 1].SelectSingleNode("TPA_Extensions").SelectSingleNode("JourneyTotalDuration").InnerText;
                                        nItem.AppendChild(node);
                                        node = doc_response.CreateElement("TotalBaseFare");
                                        Attribute = doc_response.CreateAttribute("Amount");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["Amount"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("CurrencyCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["CurrencyCode"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("DecimalPlaces");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["DecimalPlaces"].Value;
                                        node.Attributes.Append(Attribute);
                                        nItem.AppendChild(node);
                                        node = doc_response.CreateElement("TotalTax");
                                        Attribute = doc_response.CreateAttribute("Amount");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").SelectSingleNode("Tax").Attributes["Amount"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("CurrencyCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").SelectSingleNode("Tax").Attributes["CurrencyCode"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("DecimalPlaces");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").SelectSingleNode("Tax").Attributes["DecimalPlaces"].Value;
                                        node.Attributes.Append(Attribute);
                                        nItem.AppendChild(node);
                                        node = doc_response.CreateElement("TotalFare");
                                        Attribute = doc_response.CreateAttribute("Amount");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("TotalFare").Attributes["Amount"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("CurrencyCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("TotalFare").Attributes["CurrencyCode"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("DecimalPlaces");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("TotalFare").Attributes["DecimalPlaces"].Value;
                                        node.Attributes.Append(Attribute);
                                        nItem.AppendChild(node);

                                        // ******************************************************
                                        // Values are not given to the 'Origin Class' attributes
                                        // ******************************************************

                                        // node = doc_response.CreateElement("OriginClass");
                                        // Attribute = doc_response.CreateAttribute("Index");

                                        // in_flg_counter = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")(m - 1).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")(1).SelectNodes("FlightSegment").Count

                                        var in_flg_counter = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment").Count;
                                        for (int index_2 = 1, loopTo4 = in_flg_counter; index_2 <= loopTo4; index_2++)
                                        {
                                            node = doc_response.CreateElement("OriginClass");
                                            Attribute = doc_response.CreateAttribute("Index");
                                            Attribute.Value = index_2.ToString();
                                            node.Attributes.Append(Attribute);
                                            Attribute = doc_response.CreateAttribute("Cabin");
                                            Attribute.Value = "Economy";
                                            node.Attributes.Append(Attribute);
                                            node.InnerText = "U";
                                            nItem.AppendChild(node);
                                        }


                                        // node = doc_response.CreateElement("FareBasisCodes")
                                        // Attribute = doc_response.CreateAttribute("PassengerType")
                                        // node.Attributes.Append(Attribute)
                                        // nItem.AppendChild(node)

                                        // flight_counter = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")(m - 1).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("FareInfos").SelectNodes("FareInfo").Count

                                        // For p As Integer = 0 To flight_counter - 1
                                        // nItem = doc_response.CreateElement("FareBasisCode")
                                        // Dim ss As String = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")(m - 1).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("FareInfos").SelectNodes("FareInfo")(p).SelectSingleNode("FareReference").InnerText
                                        // nItem.InnerText = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")(m - 1).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("FareInfos").SelectNodes("FareInfo")(p).SelectSingleNode("FareReference").InnerText
                                        // node.AppendChild(nItem)
                                        // Next

                                        pct_fairdowncount = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("PTC_FareBreakdowns").SelectNodes("PTC_FareBreakdown").Count;
                                        for (int fdc = 0, loopTo5 = pct_fairdowncount - 1; fdc <= loopTo5; fdc++)
                                        {
                                            node = doc_response.CreateElement("FareBasisCodes");
                                            Attribute = doc_response.CreateAttribute("PassengerType");
                                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("PTC_FareBreakdowns").SelectNodes("PTC_FareBreakdown")[fdc].SelectSingleNode("PassengerTypeQuantity").Attributes["Code"].Value;
                                            node.Attributes.Append(Attribute);
                                            var flight_counter = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("PTC_FareBreakdowns").SelectNodes("PTC_FareBreakdown")[fdc].SelectSingleNode("FareBasisCodes").SelectNodes("FareBasisCode").Count;
                                            for (int p = 0, loopTo6 = flight_counter - 1; p <= loopTo6; p++)
                                            {
                                                XmlNode fbItem = doc_response.CreateElement("FareBasisCode");
                                                fbItem.InnerText = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("PTC_FareBreakdowns").SelectNodes("PTC_FareBreakdown")[fdc].SelectSingleNode("FareBasisCodes").SelectNodes("FareBasisCode")[p].InnerText;
                                                node.AppendChild(fbItem);
                                            }

                                            nItem.AppendChild(node);
                                        }
                                    }
                                }

                            }
                            else if (combinations[y - 1, 2] is null && combinations[y - 1, 1] is object && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment").Count == 2)
                            {
                                // checking whether outbound consists of only two flightItems 

                                string xml_flightNo1 = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[0].Attributes["FlightNumber"].Value.ToString();
                                string comb_flightNo1 = combinations[y - 1, 0];
                                string xml_flightNo2 = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[1].Attributes["FlightNumber"].Value.ToString();
                                string comb_flightNo2 = combinations[y - 1, 1];
                                xml_flightNo1 = digit_checker(xml_flightNo1, comb_flightNo1);
                                comb_flightNo1 = digit_checker(comb_flightNo1, xml_flightNo1);
                                xml_flightNo2 = digit_checker(xml_flightNo2, comb_flightNo2);
                                comb_flightNo2 = digit_checker(comb_flightNo2, xml_flightNo2);
                                int new_flNo = 0;
                                int r = 0;
                                if (xml_flightNo1.Equals(comb_flightNo1) && xml_flightNo2.Equals(comb_flightNo2) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[0].Attributes["ResBookDesigCode"].Value.ToString().Equals(combinations[y - 1, 4]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[1].Attributes["ResBookDesigCode"].Value.ToString().Equals(combinations[y - 1, 5]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[0].SelectSingleNode("MarketingAirline").Attributes["Code"].Value.ToString().Equals(combinations[y - 1, 8]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[1].SelectSingleNode("MarketingAirline").Attributes["Code"].Value.ToString().Equals(combinations[y - 1, 9]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[0].Attributes["DepartureDateTime"].Value.ToString().Equals(combinations[y - 1, 12]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[1].Attributes["DepartureDateTime"].Value.ToString().Equals(combinations[y - 1, 13]))
                                {
                                    inbound_flg_count += 1;
                                    if (inbound_comb[m - 1, 1] is null && inbound_comb[m - 1, 0] is object)
                                    {
                                        r = 1;
                                    }
                                    else if (inbound_comb[m - 1, 2] is null && inbound_comb[m - 1, 3] is null && inbound_comb[m - 1, 1] is object)
                                    {
                                        r = 2;
                                    }
                                    else if (inbound_comb[m - 1, 3] is null && inbound_comb[m - 1, 2] is object)
                                    {
                                        r = 3;
                                    }

                                    // #Region "For loop forFlight Segments (IN_Bounds) using 2 or 3 for 'r'"
                                    // ***********************
                                    // For loop for priniting Flight Segments (IN_Bounds)
                                    // ***********************

                                    for (int e = 1, loopTo7 = r; e <= loopTo7; e++)
                                    {
                                        node = doc_response.CreateElement("FlightSegment");
                                        inbound.AppendChild(node);
                                        Attribute = doc_response.CreateAttribute("DepartureDateTime");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["DepartureDateTime"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("ArrivalDateTime");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["ArrivalDateTime"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("RPH");
                                        Attribute.Value = inbound_flg_count.ToString();
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("FlightNumber");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["FlightNumber"].Value;
                                        new_flNo = int.Parse(Attribute.Value);
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("ResBookDesigCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["ResBookDesigCode"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("NumberInParty");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["NumberInParty"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("E_TicketEligibility");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["E_TicketEligibility"].Value;
                                        node.Attributes.Append(Attribute);

                                        // ************************************************
                                        // '/ tags in side the filgtItem tag of inbounds
                                        // ************************************************

                                        nItem = doc_response.CreateElement("DepartureAirport");
                                        Attribute = doc_response.CreateAttribute("LocationCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("ArrivalAirport");
                                        Attribute = doc_response.CreateAttribute("LocationCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("OperatingAirline");
                                        Attribute = doc_response.CreateAttribute("Code");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("OperatingAirline").Attributes["Code"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("Equipment");
                                        Attribute = doc_response.CreateAttribute("AirEquipType");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("Equipment").Attributes["AirEquipType"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("MarketingAirline");
                                        Attribute = doc_response.CreateAttribute("Code");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("MarketingAirline").Attributes["Code"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("TPA_Extensions");
                                        Attribute = doc_response.CreateAttribute("PricingSource");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("TPA_Extensions").Attributes["PricingSource"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        node = doc_response.CreateElement("CabinType");
                                        Attribute = doc_response.CreateAttribute("Cabin");
                                        Attribute.Value = "Economy";
                                        node.Attributes.Append(Attribute);
                                        nItem.AppendChild(node);
                                        node = doc_response.CreateElement("JourneyTotalDuration");
                                        node = doc_response.CreateElement("JourneyTotalDuration");
                                        node.InnerText = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment ")[e - 1].SelectSingleNode("TPA_Extensions").SelectSingleNode("JourneyTotalDuration").InnerText;
                                        nItem.AppendChild(node);
                                        node = doc_response.CreateElement("TotalBaseFare");
                                        Attribute = doc_response.CreateAttribute("Amount");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["Amount"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("CurrencyCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["CurrencyCode"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("DecimalPlaces");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["DecimalPlaces"].Value;
                                        node.Attributes.Append(Attribute);
                                        nItem.AppendChild(node);
                                        node = doc_response.CreateElement("TotalTax");
                                        Attribute = doc_response.CreateAttribute("Amount");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").SelectSingleNode("Tax").Attributes["Amount"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("CurrencyCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").SelectSingleNode("Tax").Attributes["CurrencyCode"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("DecimalPlaces");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").SelectSingleNode("Tax").Attributes["DecimalPlaces"].Value;
                                        node.Attributes.Append(Attribute);
                                        nItem.AppendChild(node);
                                        node = doc_response.CreateElement("TotalFare");
                                        Attribute = doc_response.CreateAttribute("Amount");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("TotalFare").Attributes["Amount"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("CurrencyCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("TotalFare").Attributes["CurrencyCode"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("DecimalPlaces");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("TotalFare").Attributes["DecimalPlaces"].Value;
                                        node.Attributes.Append(Attribute);
                                        nItem.AppendChild(node);

                                        // ******************************************************
                                        // Values are not given to the 'Origin Class' attributes
                                        // ******************************************************
                                        // node = doc_response.CreateElement("OriginClass");
                                        // Attribute = doc_response.CreateAttribute("Index");

                                        var in_flg_counter = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment").Count;

                                        // in_flg_counter = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")(m - 1).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")(1).SelectNodes("FlightSegment").Count

                                        for (int index_2 = 1, loopTo8 = in_flg_counter; index_2 <= loopTo8; index_2++)
                                        {
                                            node = doc_response.CreateElement("OriginClass");
                                            Attribute = doc_response.CreateAttribute("Index");
                                            Attribute.Value = index_2.ToString();
                                            node.Attributes.Append(Attribute);
                                            Attribute = doc_response.CreateAttribute("Cabin");
                                            Attribute.Value = "Economy";
                                            node.Attributes.Append(Attribute);
                                            node.InnerText = "U";
                                            nItem.AppendChild(node);
                                        }

                                        pct_fairdowncount = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("PTC_FareBreakdowns").SelectNodes("PTC_FareBreakdown").Count;
                                        for (int fdc = 0, loopTo9 = pct_fairdowncount - 1; fdc <= loopTo9; fdc++)
                                        {
                                            node = doc_response.CreateElement("FareBasisCodes");
                                            Attribute = doc_response.CreateAttribute("PassengerType");
                                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("PTC_FareBreakdowns").SelectNodes("PTC_FareBreakdown")[fdc].SelectSingleNode("PassengerTypeQuantity").Attributes["Code"].Value;
                                            node.Attributes.Append(Attribute);
                                            var flight_counter = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("PTC_FareBreakdowns").SelectNodes("PTC_FareBreakdown")[fdc].SelectSingleNode("FareBasisCodes").SelectNodes("FareBasisCode").Count;
                                            for (int p = 0, loopTo10 = flight_counter - 1; p <= loopTo10; p++)
                                            {
                                                XmlNode fbItem = doc_response.CreateElement("FareBasisCode");
                                                fbItem.InnerText = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("PTC_FareBreakdowns").SelectNodes("PTC_FareBreakdown")[fdc].SelectSingleNode("FareBasisCodes").SelectNodes("FareBasisCode")[p].InnerText;
                                                node.AppendChild(fbItem);
                                            }

                                            nItem.AppendChild(node);
                                        }

                                        // node = doc_response.CreateElement("FareBasisCodes")
                                        // Attribute = doc_response.CreateAttribute("PassengerType")
                                        // node.Attributes.Append(Attribute)
                                        // nItem.AppendChild(node)


                                        // flight_counter = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")(m - 1).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("FareInfos").SelectNodes("FareInfo").Count

                                        // For p As Integer = 0 To flight_counter - 1
                                        // nItem = doc_response.CreateElement("FareBasisCode")
                                        // Dim ss As String = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")(m - 1).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("FareInfos").SelectNodes("FareInfo")(p).SelectSingleNode("FareReference").InnerText
                                        // nItem.InnerText = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")(m - 1).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("FareInfos").SelectNodes("FareInfo")(p).SelectSingleNode("FareReference").InnerText
                                        // node.AppendChild(nItem)

                                        // Next
                                        // #End Region
                                    }
                                }

                            }
                            else if (combinations[y - 1, 2] is object && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment").Count == 3)
                            {
                                string aa = combinations[y - 1, 2].ToString();
                                string xml_flightNo1 = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[0].Attributes["FlightNumber"].Value.ToString();
                                string comb_flightNo1 = combinations[y - 1, 0];
                                string comb_flightNo2 = combinations[y - 1, 1];
                                string xml_flightNo2 = null;
                                try
                                {
                                    xml_flightNo2 = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[1].Attributes["FlightNumber"].Value.ToString();
                                }
                                catch
                                {
                                }

                                string xml_flightNo3 = null;
                                string comb_flightNo3 = combinations[y - 1, 2];
                                try
                                {
                                    xml_flightNo3 = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[2].Attributes["FlightNumber"].Value.ToString();
                                }
                                catch
                                {
                                }

                                xml_flightNo1 = digit_checker(xml_flightNo1, comb_flightNo1);
                                comb_flightNo1 = digit_checker(comb_flightNo1, xml_flightNo1);
                                xml_flightNo2 = digit_checker(xml_flightNo2, comb_flightNo2);
                                comb_flightNo2 = digit_checker(comb_flightNo2, xml_flightNo2);
                                xml_flightNo3 = digit_checker(xml_flightNo3, comb_flightNo3);
                                comb_flightNo3 = digit_checker(comb_flightNo3, xml_flightNo3);
                                if (xml_flightNo1.Equals(comb_flightNo1) && xml_flightNo2.Equals(comb_flightNo2) && xml_flightNo3.Equals(comb_flightNo3) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[0].Attributes["ResBookDesigCode"].Value.ToString().Equals(combinations[y - 1, 4]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[1].Attributes["ResBookDesigCode"].Value.ToString().Equals(combinations[y - 1, 5]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[2].Attributes["ResBookDesigCode"].Value.ToString().Equals(combinations[y - 1, 6]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[0].SelectSingleNode("MarketingAirline").Attributes["Code"].Value.ToString().Equals(combinations[y - 1, 8]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[1].SelectSingleNode("MarketingAirline").Attributes["Code"].Value.ToString().Equals(combinations[y - 1, 9]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[2].SelectSingleNode("MarketingAirline").Attributes["Code"].Value.ToString().Equals(combinations[y - 1, 10]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[0].Attributes["DepartureDateTime"].Value.ToString().Equals(combinations[y - 1, 12]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment ")[1].Attributes["DepartureDateTime"].Value.ToString().Equals(combinations[y - 1, 13]) && doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment")[2].Attributes["DepartureDateTime"].Value.ToString().Equals(combinations[y - 1, 14]))
                                {
                                    int r = 0;
                                    inbound_flg_count += 1;
                                    if (inbound_comb[m - 1, 1] is null)
                                    {
                                        r = 1;
                                    }
                                    else if (inbound_comb[m - 1, 2] is null && inbound_comb[m - 1, 3] is null)
                                    {
                                        r = 2;
                                    }
                                    else if (inbound_comb[m - 1, 3] is null)
                                    {
                                        r = 3;
                                    }

                                    // ***********************
                                    // For loop for priniting Flight Segments (IN_Bounds)
                                    // ***********************
                                    for (int e = 1, loopTo11 = r; e <= loopTo11; e++)
                                    {
                                        node = doc_response.CreateElement("FlightSegment");
                                        inbound.AppendChild(node);
                                        Attribute = doc_response.CreateAttribute("DepartureDateTime");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["DepartureDateTime"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("ArrivalDateTime");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["ArrivalDateTime"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("RPH");
                                        Attribute.Value = inbound_flg_count.ToString();
                                        node.Attributes.Append(Attribute);

                                        // ' ************************** ERROR ***************************************************

                                        Attribute = doc_response.CreateAttribute("FlightNumber");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["FlightNumber"].Value;

                                        // 'Attribute.Value = Convert.ToString(flight_val_in)
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("ResBookDesigCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["ResBookDesigCode"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("NumberInParty");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["NumberInParty"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("E_TicketEligibility");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].Attributes["E_TicketEligibility"].Value;
                                        node.Attributes.Append(Attribute);

                                        // ************************************************
                                        // ' tags in side the filgtItem tag of inbounds
                                        // ************************************************

                                        nItem = doc_response.CreateElement("DepartureAirport");
                                        Attribute = doc_response.CreateAttribute("LocationCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("ArrivalAirport");
                                        Attribute = doc_response.CreateAttribute("LocationCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("OperatingAirline");
                                        Attribute = doc_response.CreateAttribute("Code");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("OperatingAirline").Attributes["Code"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("Equipment");
                                        Attribute = doc_response.CreateAttribute("AirEquipType");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("Equipment").Attributes["AirEquipType"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("MarketingAirline");
                                        Attribute = doc_response.CreateAttribute("Code");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("MarketingAirline").Attributes["Code"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        nItem = doc_response.CreateElement("TPA_Extensions");
                                        Attribute = doc_response.CreateAttribute("PricingSource");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment")[e - 1].SelectSingleNode("TPA_Extensions").Attributes["PricingSource"].Value;
                                        nItem.Attributes.Append(Attribute);
                                        node.AppendChild(nItem);
                                        node = doc_response.CreateElement("CabinType");
                                        Attribute = doc_response.CreateAttribute("Cabin");
                                        Attribute.Value = "Economy";
                                        node.Attributes.Append(Attribute);
                                        nItem.AppendChild(node);
                                        node = doc_response.CreateElement("JourneyTotalDuration");
                                        node = doc_response.CreateElement("JourneyTotalDuration");
                                        node.InnerText = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[1].SelectNodes("FlightSegment ")[e - 1].SelectSingleNode("TPA_Extensions").SelectSingleNode("JourneyTotalDuration").InnerText;
                                        nItem.AppendChild(node);
                                        node = doc_response.CreateElement("TotalBaseFare");
                                        Attribute = doc_response.CreateAttribute("Amount");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["Amount"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("CurrencyCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["CurrencyCode"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("DecimalPlaces");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["DecimalPlaces"].Value;
                                        node.Attributes.Append(Attribute);
                                        nItem.AppendChild(node);
                                        node = doc_response.CreateElement("TotalTax");
                                        Attribute = doc_response.CreateAttribute("Amount");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").SelectSingleNode("Tax").Attributes["Amount"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("CurrencyCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").SelectSingleNode("Tax").Attributes["CurrencyCode"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("DecimalPlaces");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").SelectSingleNode("Tax").Attributes["DecimalPlaces"].Value;
                                        node.Attributes.Append(Attribute);
                                        nItem.AppendChild(node);
                                        node = doc_response.CreateElement("TotalFare");
                                        Attribute = doc_response.CreateAttribute("Amount");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("TotalFare").Attributes["Amount"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("CurrencyCode");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("TotalFare").Attributes["CurrencyCode"].Value;
                                        node.Attributes.Append(Attribute);
                                        Attribute = doc_response.CreateAttribute("DecimalPlaces");
                                        Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("TotalFare").Attributes["DecimalPlaces"].Value;
                                        node.Attributes.Append(Attribute);
                                        nItem.AppendChild(node);
                                        var in_flg_counter = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[y - 1].SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")[0].SelectNodes("FlightSegment").Count;

                                        // in_flg_counter = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")(m - 1).SelectSingleNode("AirItinerary").SelectSingleNode("OriginDestinationOptions").SelectNodes("OriginDestinationOption")(1).SelectNodes("FlightSegment").Count

                                        for (int index_2 = 1, loopTo12 = in_flg_counter; index_2 <= loopTo12; index_2++)
                                        {
                                            node = doc_response.CreateElement("OriginClass");
                                            Attribute = doc_response.CreateAttribute("Index");
                                            Attribute.Value = index_2.ToString();
                                            node.Attributes.Append(Attribute);
                                            Attribute = doc_response.CreateAttribute("Cabin");
                                            Attribute.Value = "Economy";
                                            node.Attributes.Append(Attribute);
                                            node.InnerText = "U";
                                            nItem.AppendChild(node);
                                        }

                                        pct_fairdowncount = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("PTC_FareBreakdowns").SelectNodes("PTC_FareBreakdown").Count;
                                        for (int fdc = 0, loopTo13 = pct_fairdowncount - 1; fdc <= loopTo13; fdc++)
                                        {
                                            node = doc_response.CreateElement("FareBasisCodes");
                                            Attribute = doc_response.CreateAttribute("PassengerType");
                                            Attribute.Value = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("PTC_FareBreakdowns").SelectNodes("PTC_FareBreakdown")[fdc].SelectSingleNode("PassengerTypeQuantity").Attributes["Code"].Value;
                                            node.Attributes.Append(Attribute);
                                            var flight_counter = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("PTC_FareBreakdowns").SelectNodes("PTC_FareBreakdown")[fdc].SelectSingleNode("FareBasisCodes").SelectNodes("FareBasisCode").Count;
                                            for (int p = 0, loopTo14 = flight_counter - 1; p <= loopTo14; p++)
                                            {
                                                XmlNode fbItem = doc_response.CreateElement("FareBasisCode");
                                                fbItem.InnerText = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")[m - 1].SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("PTC_FareBreakdowns").SelectNodes("PTC_FareBreakdown")[fdc].SelectSingleNode("FareBasisCodes").SelectNodes("FareBasisCode")[p].InnerText;
                                                node.AppendChild(fbItem);
                                            }

                                            nItem.AppendChild(node);
                                        }

                                        // node = doc_response.CreateElement("FareBasisCodes")
                                        // Attribute = doc_response.CreateAttribute("PassengerType")
                                        // node.Attributes.Append(Attribute)
                                        // nItem.AppendChild(node)


                                        // flight_counter = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")(m - 1).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("FareInfos").SelectNodes("FareInfo").Count

                                        // For p As Integer = 0 To flight_counter - 1
                                        // nItem = doc_response.CreateElement("FareBasisCode")
                                        // Dim ss As String = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")(m - 1).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("FareInfos").SelectNodes("FareInfo")(p).SelectSingleNode("FareReference").InnerText
                                        // nItem.InnerText = doc.SelectSingleNode("OTA_AirLowFareSearchPlusRS/PricedItineraries").SelectNodes("PricedItinerary")(m - 1).SelectSingleNode("AirItineraryPricingInfo").SelectSingleNode("FareInfos").SelectNodes("FareInfo")(p).SelectSingleNode("FareReference").InnerText
                                        // node.AppendChild(nItem)
                                        // Next
                                    }
                                }

                                // #End Region
                            }
                        }

                        inbound_flg_count = 0;
                        unique_outbounds += 1;
                    }
                }

                doc_response.AppendChild(Root);
                strResponse = Root.OuterXml;
            }
            catch (Exception ex)
            {
                strResponse = ex.Message;
            }
            return strResponse;
        }

        private string digit_checker(string a, string b)
        {
            int c = 0;
            int d = 0;
            try
            {
                c = a.Length;
                d = b.Length;
            }
            catch
            {
            }

            if (c < d)
            {
                int dif = d - c;
                for (int g = 1, loopTo = dif; g <= loopTo; g++)
                    a = "0" + a;
                return a;
            }
            else if (a is null)
            {
                return null;
            }
            else
            {
                return a;
            }
        }

        public string FareDisplay()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA LowFarePlus Request into Native Galileo Request     *
            // ***************************************************************** 
            try
            {
                string strRequest = SetRequest("Galileo_FareDisplayRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Galileo Adapter and Getting Native Response  *
                // ******************************************************************************* 
                GalileoAdapter ttGA = SetAdapter(ProviderSystems.SessionPool ? "V1" : "");
                bool inSession = SetConversationID(ttGA);
                strResponse = ttGA.SendMessage(strRequest);

                // ****************************************************************
                // Add OriginDestinationInformation Request to Native Response   *
                // ****************************************************************
                var oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                var oRoot = oDoc.DocumentElement;
                var oNode = oDoc.CreateNode(XmlNodeType.Element, "", "Request", "");
                oNode.InnerXml = Request;
                oRoot.AppendChild(oNode);
                strResponse = oDoc.OuterXml;

                // *****************************************************************
                // Transform Native Galileo FareDisplay Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Galileo_FareDisplayRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.FareDisplay, ex.Message, ProviderSystems);
            }
            return strResponse;
        }

    }
}