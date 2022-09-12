using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Xml;
using TripXMLMain;

namespace Sabre
{
    public class AirServices : SabreBase
    {
        public string AirAvail()
        {
            string strResponse;
            // *****************************************************************
            // Transform OTA AirAvail Request into Native Sabre Request     *
            // ***************************************************************** 
            try
            {
                var ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                string strRequest = SetRequest("Sabre_AirAvailRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttSA.SendMessage(strRequest, "Air", "OTA_AirAvailLLSRQ");

                // *****************************************************************
                // Transform Native Sabre AirAvail Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_AirAvailRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.AirAvail, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string AirFlifo()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA AirFlifo Request into Native Sabre Request     *
            // ***************************************************************** 
            try
            {
                var ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                var strRequest = SetRequest("Sabre_AirFlifoRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttSA.SendMessage(strRequest, "Air", "OTA_AirFlifoLLSRQ");

                // *****************************************************************
                // Transform Native Sabre AirFlifo Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_AirFlifoRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.{Environment.NewLine}{ex.Message}");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.AirFlifo, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string AirPrice()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA AirPrice Request into Native Sabre Request     *
            // ***************************************************************** 
            try
            {
                var ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                var strRequest = SetRequest("Sabre_AirPriceRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // ********************
                // Get All Requests  * 
                // ********************
                var oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                var oRoot = oDoc.DocumentElement;
                var strAir = oRoot.SelectSingleNode("AirBook").InnerXml.Replace(" xmlns=\"\"", "");
                var strRead = oRoot.SelectSingleNode("Read").InnerXml.Replace(" xmlns=\"\"", "");
                var strPricing = oRoot.SelectSingleNode("Pricing").InnerXml.Replace(" xmlns=\"\"", "");
                var strIgnore = oRoot.SelectSingleNode("Ignore").InnerXml.Replace(" xmlns=\"\"", "");

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttSA.SendMessage(strAir, "AirBook", "OTA_AirBookLLSRQ", ConversationID);

                // Log Errors
                if (!strResponse.Contains("<Error"))
                {
                    Thread.Sleep(2000);

                    strResponse = ttSA.SendMessage(strRead, "GetReservationRQ", "GetReservationRQ", ConversationID);

                    var oDocResp = new XmlDocument();
                    oDocResp.LoadXml(strResponse);
                    var oRootResp = oDocResp.DocumentElement;
                    bool readOK = true;
                    foreach (XmlNode oNodeResp in oRootResp.SelectNodes("Reservation/PassengerReservation/Segments/Segment/Air/ActionCode"))
                    {
                        if (oNodeResp.InnerText != "SS")
                        {
                            readOK = false;
                            break;
                        }
                    }

                    if (readOK)
                    {
                        if (strResponse.IndexOf("<Error") == -1)
                        {
                            try
                            {
                                strResponse = ttSA.SendMessage(strPricing, "Air Price", "OTA_AirPriceLLSRQ", ConversationID);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }

                    // here we ignore the flights booked to release the inventory back to the airline
                    strIgnore = ttSA.SendMessage(strIgnore, "IgnoreTransaction", "IgnoreTransactionLLSRQ", ConversationID);
                }

                // Close Session
                inSession = false;
                // *****************************************************************
                // Transform Native Sabre AirPrice Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = strResponse.Replace("</OTA_AirPriceRS>", $"{Request}</OTA_AirPriceRS>");
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_AirPriceRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.AirPrice, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string AirRules()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA AirRules Request into Native Sabre Request     *
            // ***************************************************************** 
            try
            {
                // *******************
                // Create Session    *
                // *******************
                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);
                string strRequest = Version == "v03_" ? SetRequest("Sabre_AirPriceRQ.xsl") : SetRequest("Sabre_AirRulesRQ.xsl");

                if (Version == "v03_")
                {                   

                    // ********************
                    // Get All Requests  * 
                    // ********************
                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(strRequest);
                    var oRoot = oDoc.DocumentElement;
                    string strAir = oRoot.SelectSingleNode("AirBook").InnerXml.Replace(" xmlns=\"\"", "");
                    string strPricing = oRoot.SelectSingleNode("Pricing").InnerXml.Replace(" xmlns=\"\"", "");

                    // *******************************************************************************
                    // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                    // ******************************************************************************* 
                    strResponse = ttSA.SendMessage(strAir, "AirBook", "OTA_AirBookLLSRQ", ConversationID);

                    // Log Errors
                    if (!strResponse.Contains("<Error"))
                    {
                        strResponse = ttSA.SendMessage(strPricing, "Air Price", "OTA_AirPriceLLSRQ", ConversationID);
                    }

                    strRequest = strResponse;
                    strRequest = CoreLib.TransformXML(strRequest, XslPath, $"{Version}Sabre_AirRulesRQ.xsl");

                    if (strRequest.Trim().Length == 0)
                    {
                        throw new Exception("Transformation produced empty xml.");
                    }

                    oDoc = new XmlDocument();
                    oDoc.LoadXml(strRequest);
                    oRoot = oDoc.DocumentElement;
                    strResponse = "";
                    string Uri = oRoot.FirstChild.Attributes["xmlns"].InnerText;
                    var nsmgr = new XmlNamespaceManager(oDoc.NameTable);
                    nsmgr.AddNamespace("dc", Uri);
                    foreach (XmlNode oNode in oRoot.SelectNodes("dc:RulesFromPriceRQ", nsmgr))
                    {

                        strResponse = strResponse + ttSA.SendMessage(oNode.OuterXml, "AirRules", "RulesFromPriceLLSRQ", ConversationID);
                    }

                    strResponse = $"<RulesRS>{strResponse}</RulesRS>";
                }
                else
                {
                    // *******************************************************************************
                    // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                    // *******************************************************************************                     
                    if (string.IsNullOrEmpty(strRequest))
                        throw new Exception("Transformation produced empty xml.");

                    strResponse = ttSA.SendMessage(strRequest, "AirRules", "OTA_AirRulesLLSRQ", ConversationID);
                }

                strResponse = strResponse.Replace("Â?", "|");

                // *****************************************************************
                // Transform Native Sabre AirRules Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_AirRulesRS.xsl");
                    if (strResponse.IndexOf("THE FOLLOWING CARRIERS ALSO PUBLISH FARES") != -1)
                    {
                        strRequest = "<OTA_AirRulesRQ xmlns=\"http://webservices.sabre.com/sabreXML/2003/07\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Version=\"2003A.TsabreXML1.3.1\">";
                        strRequest += "<POS><Source PseudoCityCode=\"B68B\"/></POS>";
                        string rph = "1";
                        if (strResponse.Contains("F¤"))
                        {
                            rph = strResponse.Substring(strResponse.IndexOf("F¤") - 2, 1);
                        }

                        strRequest += $"<RuleReqInfo RPH=\"{rph}'\'/>";
                        strRequest += "</OTA_AirRulesRQ>";
                        strResponse = ttSA.SendMessage(strRequest, "AirRules", "OTA_AirRulesLLSRQ", ConversationID);
                        strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_AirRulesRS.xsl");
                    }
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.AirRules, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string AirSeatMap()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA AirFareDisplay Request into Native Sabre Request     *
            // ***************************************************************** 

            try
            {
                string strRequest = SetRequest("Sabre_AirSeatMapRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);
                strResponse = ttSA.SendMessage(strRequest, "Seat Map", "EnhancedSeatMapRQ");
                
                // *****************************************************************
                // Transform Native Sabre AirFareDisplay Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = strResponse.Replace(" xmlns=\"http://stl.sabre.com/Merchandising/v5\"", "").Replace(" xmlns:ns2=\"http://opentravel.org/common/message/v02\"", "").Replace(" xmlns:ns3=\"http://services.sabre.com/STL_Payload/v02_00\"", "");
                    strResponse = strResponse.Replace(" xmlns:ns4=\"http://services.sabre.com/STL/v02\"", "").Replace(" xmlns:ns5=\"http://opentravel.org/common/v02\"", "").Replace(" xmlns:ns6=\"http://stl.sabre.com/Merchandising/diagnostics/v1\"", "").Replace("ns3:", "");
                    strResponse = strResponse.Replace("</EnhancedSeatMapRS>", Request + "</EnhancedSeatMapRS>");
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_AirSeatMapRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.AirSeatMap, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string FareDisplay()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA AirFareDisplay Request into Native Sabre Request     *
            // ***************************************************************** 

            try
            {
                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                var strRequest = SetRequest("Sabre_FareDisplayRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttSA.SendMessage(strRequest, "Fare", "FareLLSRQ");

                // *****************************************************************
                // Transform Native Sabre AirFareDisplay Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = strResponse.Replace("</FareRS>", $"{Request}</FareRS>");
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_FareDisplayRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.FareDisplay, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string LowFare(ref DataView ttCities)
        {
            string strResponse;

            // ************************************
            // Get ODs From LowFare OTA Request  *
            // ************************************

            try
            {
                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);
                List<string> arLocations = new List<string>();
                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                var oRoot = oDoc.DocumentElement;
                var oNodeTIS = oRoot.SelectSingleNode("TravelerInfoSummary");
                var intODs = oRoot.SelectNodes("OriginDestinationInformation").Count;
                if (intODs > 1)
                {
                    //arLocations = new string[intODs + 1];
                    foreach (XmlNode currentONode in oRoot.SelectNodes("OriginDestinationInformation"))
                    {         
                        arLocations.Add(currentONode.SelectSingleNode("DestinationLocation").Attributes["LocationCode"].Value);
                    }
                }

                // *****************************************************************
                // Transform OTA LowFare Request into Native Sabre Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Sabre_LowFareRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = ttSA.SendMessage(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "BargainFinderMaxRQ");

                // ************************************
                // Build Origin Destination Nodes    *
                // ************************************ 
                try
                {
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;
                    foreach (XmlNode oNodeResp in oRoot.SelectNodes("PricedItineraries/PricedItinerary"))
                    {
                        var i = 1;
                        var j = 0;
                        var blnNewOD = true;
                        var intDayDiff = oNodeResp.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment").Count;
                        foreach (XmlNode oNodeFlight in oNodeResp.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"))
                        {
                            XmlNode oNode = null;
                            j ++;
                            if (blnNewOD)
                            {
                                oNode = oDoc.CreateNode(XmlNodeType.Element, "", "OriginDestination", "");
                            }

                            if (intODs == 1)
                            {
                                blnNewOD = j == intDayDiff;
                            }
                            else
                            {
                                var CityAirport = $"{arLocations[i]}{oNodeFlight.SelectSingleNode("ArrivalAirport/@LocationCode").InnerText}";
                                blnNewOD = (CityAirport.Substring(0, 3) ?? "") == (CityAirport.Substring(3, 3) ?? "") ? true : IsSameOD(ref ttCities, CityAirport);
                            }

                            // Append Flight to OD
                            oNode.AppendChild(oNodeFlight);

                            // Add New OD Node
                            if (blnNewOD)
                            {
                                i++;
                                // Append OD to ALT_INF 
                                oNodeResp.SelectSingleNode("AirItinerary/OriginDestinationOptions").AppendChild(oNode);
                            }
                        }

                        var oNodeOD = oNodeResp.SelectSingleNode("AirItinerary/OriginDestinationOptions/OriginDestinationOption");
                        var oNodeODs = oNodeResp.SelectSingleNode("AirItinerary/OriginDestinationOptions");
                        oNodeODs.RemoveChild(oNodeOD);
                    }

                    strResponse = $"<OTA_AirLowFareSearchRS>{oRoot.InnerXml}{oNodeTIS.OuterXml}</OTA_AirLowFareSearchRS>";
                    CoreLib.SendTrace(ProviderSystems.UserID, "AirServices", "OD Response", strResponse, ProviderSystems.LogUUID);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Building Origin Destination Nodes.\r\n{ex.Message}");
                }

                // *****************************************************************
                // Transform Native Sabre LowFare Response into OTA Response   *
                // ***************************************************************** 

                strResponse = strResponse.Replace("</OTA_AirLowFareSearchRS>", $"{oNodeTIS.OuterXml}</OTA_AirLowFareSearchRS>");
                try
                {
                    CoreLib.SendTrace(ProviderSystems.UserID, "SabreAirServices", "Response", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_LowFareRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.LowFare, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string LowFarePlus(ref DataView ttCities)
        {
            string strResponse;

            // ************************************
            // Get ODs From LowFare OTA Request  *
            // ************************************
            try
            {
                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                var oRoot = oDoc.DocumentElement;
                string echoToken = "";

                SabreAdapter ttSA;
                bool inSession = false;

                string xmlversion = "";
                if (Request.StartsWith("<?xml version"))
                {
                    xmlversion = Request.Substring(0, Request.IndexOf("<OTA_"));
                    Request = Request.Replace(xmlversion, "");
                }
                List<string> arLocations = new List<string>();
                var oNodeTIS = oRoot.SelectSingleNode("TravelerInfoSummary");
                var intODs = oRoot.SelectNodes("OriginDestinationInformation").Count;
                if (intODs > 1)
                {
                    foreach (XmlNode currentONode in oRoot.SelectNodes("OriginDestinationInformation"))
                    {
                        arLocations.Add(currentONode.SelectSingleNode("DestinationLocation").Attributes["LocationCode"].Value);
                    }
                }

                // *****************************************************************
                // Transform OTA LowFarePlus Request into Native Sabre Request     *
                // ***************************************************************** 
                if (Version == "vJR_")
                    Version = "";

                string strRequest = "";

                if (ProviderSystems.SabreFareSearch)
                {
                    strRequest = SetRequest("Sabre_LowFarePlusRQ_FS.xsl");
                }
                else if (!string.IsNullOrEmpty(ProviderSystems.AdVShop))
                {
                    strRequest = SetRequest("Sabre_LowFarePlusRQ_Air.xsl");
                    strRequest = strRequest.Replace("XXAdVShop", ProviderSystems.AdVShop);
                }
                else
                {
                    strRequest = SetRequest("Sabre_LowFarePlusRQ.xsl");
                }

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");


                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 

                if (ProviderSystems.SessionPool)
                {
                    ttSA = new SabreAdapter(ProviderSystems, "V1");
                    inSession = SetConversationID(ttSA);

                    if (ProviderSystems.SabreFareSearch)
                    {
                        strResponse = ttSA.SendMessageV3(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "BargainFinderMaxRQ");
                    }
                    else if (Convert.ToBoolean(ProviderSystems.AdVShop))
                    {
                        strResponse = ttSA.SendMessageV3(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "SSSAdvShopRQ");
                    }
                    else
                    {
                        strResponse = ttSA.SendMessageV3(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "BargainFinderMaxRQ");
                    }
                }
                else
                {
                    ttSA = new SabreAdapter(ProviderSystems);
                    inSession = SetConversationID(ttSA);

                    if (ProviderSystems.SabreFareSearch)
                    {
                        strResponse = ttSA.SendMessage(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "BargainFinderMaxRQ");
                    }
                    else if (!string.IsNullOrEmpty(ProviderSystems.AdVShop))
                    {
                        strResponse = ttSA.SendMessage(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "SSSAdvShopRQ");
                    }
                    else
                    {
                        strResponse = ttSA.SendMessage(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "BargainFinderMaxRQ");
                    }
                }


                // ************************************
                // Build Origin Destination Nodes    *
                // ************************************ 
                try
                {
                    if (ProviderSystems.SabreFareSearch)
                    {
                        oDoc.LoadXml(strResponse);
                        oRoot = oDoc.DocumentElement;
                        foreach (XmlNode oNodeResp in oRoot.SelectNodes("PricedItineraries/PricedItinerary"))
                        {
                            var i = 1;
                            var j = 0;
                            var blnNewOD = true;
                            var intDayDiff = oNodeResp.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment").Count;
                            foreach (XmlNode oNodeFlight in oNodeResp.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"))
                            {
                                XmlNode oNode = null;
                                j += 1;
                                if (blnNewOD)
                                {
                                    oNode = oDoc.CreateNode(XmlNodeType.Element, "", "OriginDestination", "");
                                }

                                if (intODs == 1)
                                {
                                    blnNewOD = j == intDayDiff;
                                }
                                else
                                {
                                    var CityAirport = $"{arLocations[i].ToUpper()}{oNodeFlight.SelectSingleNode("ArrivalAirport/@LocationCode").InnerText}";
                                    blnNewOD = (CityAirport.Substring(0, 3) ?? "") == (CityAirport.Substring(3, 3) ?? "") ? true : IsSameOD(ref ttCities, CityAirport);
                                }

                                // Append Flight to OD
                                oNode.AppendChild(oNodeFlight);

                                // Add New OD Node
                                if (blnNewOD)
                                {
                                    i += 1;
                                    // Append OD to ALT_INF 
                                    oNodeResp.SelectSingleNode("AirItinerary/OriginDestinationOptions").AppendChild(oNode);
                                }
                            }

                            var oNodeOD = oNodeResp.SelectSingleNode("AirItinerary/OriginDestinationOptions/OriginDestinationOption");
                            var oNodeODs = oNodeResp.SelectSingleNode("AirItinerary/OriginDestinationOptions");
                            oNodeODs.RemoveChild(oNodeOD);
                        }

                        strResponse = $"<OTA_AirLowFareSearchRS>{oRoot.InnerXml}{oNodeTIS.OuterXml}</OTA_AirLowFareSearchRS>";
                    }
                    else
                    {
                        strResponse = strResponse.Replace("</OTA_AirLowFareSearchRS>", oNodeTIS.OuterXml + "</OTA_AirLowFareSearchRS>");
                        strResponse = strResponse.Replace("</OTA_AirLowFareSearchRS>", Request + "</OTA_AirLowFareSearchRS>");
                    }

                    if (!string.IsNullOrEmpty(echoToken))
                    {
                        strResponse = strResponse.Replace("<OTA_AirLowFareSearchPlusRS ", "<OTA_AirLowFareSearchPlusRS EchoToken=\"" + echoToken + "\"");
                    }

                    CoreLib.SendTrace(ProviderSystems.UserID, "AirServices", "OD Response", strResponse, ProviderSystems.LogUUID);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Building Origin Destination Nodes.\r\n{ex.Message}");
                }

                // *****************************************************************
                // Transform Native Sabre LowFarePlus Response into OTA Response   *
                // ***************************************************************** 
                try
                {                    
                    //strResponse = ProviderSystems.SabreFareSearch
                    //    ? CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_LowFarePlusRS.xsl")
                    //    : CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_LowFarePlusRS_FS.xsl");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_LowFarePlusRS_FS.xsl");
                    CoreLib.SendTrace(ProviderSystems.UserID, "SabreAirServices", "Response", strResponse, ProviderSystems.LogUUID);
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.LowFarePlus, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string LowFareMatrix(ref DataView ttCities)
        {
            string strResponse;

            // ************************************
            // Get ODs From LowFare OTA Request  *
            // ************************************
            try
            {
                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                string xmlversion = "";
                if (Request.StartsWith("<?xml version"))
                {
                    xmlversion = Request.Substring(0, Request.IndexOf("<OTA_"));
                    Request = Request.Replace(xmlversion, "");
                }

                List<string> arLocations = new List<string>();
                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                var oRoot = oDoc.DocumentElement;
                var oNodeTIS = oRoot.SelectSingleNode("TravelerInfoSummary");
                var intODs = oRoot.SelectNodes("OriginDestinationInformation").Count;

                if (intODs > 1)
                {
                    foreach (XmlNode oNode in oRoot.SelectNodes("OriginDestinationInformation"))
                    {
                        arLocations.Add(oNode.SelectSingleNode("DestinationLocation").Attributes["LocationCode"].Value);
                    }
                }

                // *****************************************************************
                // Transform OTA LowFarePlus Request into Native Sabre Request     *
                // ***************************************************************** 

                if (Version == "vJR_")
                    Version = "";

                string strRequest = SetRequest("Sabre_LowFarePlusRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 

                ttSA = ProviderSystems.SessionPool ? new SabreAdapter(ProviderSystems, "V1") : new SabreAdapter(ProviderSystems);
                strResponse = ProviderSystems.SessionPool
                    ? ttSA.SendMessageV3(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "BargainFinderMax_ADRQ")
                    : ttSA.SendMessage(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "BargainFinderMax_ADRQ");

                // ************************************
                // Build Origin Destination Nodes    *
                // ************************************ 
                strResponse = strResponse.Replace("</OTA_AirLowFareSearchRS>", $"{oNodeTIS.OuterXml}</OTA_AirLowFareSearchRS>");
                strResponse = strResponse.Replace("</OTA_AirLowFareSearchRS>", $"{Request}</OTA_AirLowFareSearchRS>");
                CoreLib.SendTrace(ProviderSystems.UserID, "AirServices", "OD Response", strResponse, ProviderSystems.LogUUID);

                // *****************************************************************
                // Transform Native Sabre LowFarePlus Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    CoreLib.SendTrace(ProviderSystems.UserID, "SabreAirServices", "Response", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_LowFarePlusRS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.LowFareMatrix, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string LowFareSchedule(ref DataView ttCities)
        {
            string strResponse;

            // ************************************
            // Get ODs From LowFare OTA Request  *
            // ************************************
            try
            {
                SabreAdapter ttSA = SetAdapter();
                bool inSession = SetConversationID(ttSA);

                string xmlversion = "";
                if (Request.StartsWith("<?xml version"))
                {
                    xmlversion = Request.Substring(0, Request.IndexOf("<OTA_"));
                    Request = Request.Replace(xmlversion, "");
                }

                List<string> arLocations = new List<string>();
                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                var oRoot = oDoc.DocumentElement;
                var oNodeTIS = oRoot.SelectSingleNode("TravelerInfoSummary");
                var intODs = oRoot.SelectNodes("OriginDestinationInformation").Count;

                if (intODs > 1)
                {
                    foreach (XmlNode currentONode in oRoot.SelectNodes("OriginDestinationInformation"))
                    {
                        arLocations.Add(currentONode.SelectSingleNode("DestinationLocation").Attributes["LocationCode"].Value);
                    }
                }

                // *****************************************************************
                // Transform OTA LowFareSchedule Request into Native Sabre Request     *
                // ***************************************************************** 

                string strRequest = "";
                if (!string.IsNullOrEmpty(ProviderSystems.AdVShop))
                {
                    strRequest = SetRequest("Sabre_LowFarePlusRQ_Air.xsl");
                    strRequest = strRequest.Replace("XXAdVShop", ProviderSystems.AdVShop);
                }
                else
                {
                    strRequest = SetRequest("Sabre_LowFareScheduleRQ.xsl");
                }

                // *******************************************************************************
                // Send Transformed Request to the Sabre Adapter and Getting Native Response  *
                // ******************************************************************************* 
                if (ProviderSystems.SessionPool)
                {
                    ttSA = new SabreAdapter(ProviderSystems, "V1");
                    
                    strResponse = Convert.ToBoolean(ProviderSystems.AdVShop)
                            ? ttSA.SendMessageV3(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "SSSAdvShopRQ")
                            : Convert.ToBoolean(ProviderSystems.SabreFareSearch) 
                                ? ttSA.SendMessageV3(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "OTA_AirLowFareSearchLLSRQ")
                                : ttSA.SendMessageV3(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "BargainFinderMaxRQ");
                }
                else
                {
                    ttSA = new SabreAdapter(ProviderSystems);
                    strResponse = !string.IsNullOrEmpty(ProviderSystems.AdVShop)
                            ? ttSA.SendMessage(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "SSSAdvShopRQ")
                            : Convert.ToBoolean(ProviderSystems.SabreFareSearch)  
                                ? ttSA.SendMessage(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "OTA_AirLowFareSearchLLSRQ")
                                : ttSA.SendMessage(strRequest.Replace(" xmlns=\"\"", ""), "Low Fare Search", "BargainFinderMaxRQ");
                }

                // ************************************
                // Build Origin Destination Nodes    *
                // ************************************ 
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;
                if (ProviderSystems.SabreFareSearch)
                {
                    foreach (XmlNode oNodeResp in oRoot.SelectNodes("PricedItineraries/PricedItinerary"))
                    {
                        var i = 1;
                        var j = 0;
                        var blnNewOD = true;
                        var intDayDiff = oNodeResp.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment").Count;
                        foreach (XmlNode oNodeFlight in oNodeResp.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"))
                        {
                            XmlNode oNode = null;
                            j += 1;
                            if (blnNewOD)
                            {
                                oNode = oDoc.CreateNode(XmlNodeType.Element, "", "OriginDestination", "");
                            }

                            if (intODs == 1)
                            {
                                blnNewOD = j == intDayDiff;
                            }
                            else
                            {
                                var CityAirport = $"{arLocations[i].ToUpper()}{oNodeFlight.SelectSingleNode("ArrivalAirport/@LocationCode").InnerText}";
                                blnNewOD = (CityAirport.Substring(0, 3) ?? "") == (CityAirport.Substring(3, 3) ?? "") ? true : IsSameOD(ref ttCities, CityAirport);
                            }

                            // Append Flight to OD
                            oNode.AppendChild(oNodeFlight);

                            // Add New OD Node
                            if (blnNewOD)
                            {
                                i += 1;
                                // Append OD to ALT_INF 
                                oNodeResp.SelectSingleNode("AirItinerary/OriginDestinationOptions").AppendChild(oNode);
                            }
                        }

                        var oNodeOD = oNodeResp.SelectSingleNode("AirItinerary/OriginDestinationOptions/OriginDestinationOption");
                        var oNodeODs = oNodeResp.SelectSingleNode("AirItinerary/OriginDestinationOptions");
                        oNodeODs.RemoveChild(oNodeOD);
                    }

                    strResponse = $"<OTA_AirLowFareSearchRS>{oRoot.InnerXml}{oNodeTIS.OuterXml}</OTA_AirLowFareSearchRS>";
                }
                else
                {
                    strResponse = strResponse.Replace("</OTA_AirLowFareSearchRS>", oNodeTIS.OuterXml + "</OTA_AirLowFareSearchRS>");
                    strResponse = strResponse.Replace("</OTA_AirLowFareSearchRS>", Request + "</OTA_AirLowFareSearchRS>");
                }

                CoreLib.SendTrace(ProviderSystems.UserID, "AirServices", "OD Response", strResponse, ProviderSystems.LogUUID);

                // *****************************************************************
                // Transform Native Sabre LowFareSchedule Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    CoreLib.SendTrace(ProviderSystems.UserID, "SabreAirServices", "Response", strResponse, ProviderSystems.LogUUID);

                    strResponse = !string.IsNullOrEmpty(ProviderSystems.AdVShop)
                        ? CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_LowFarePlusRS.xsl")
                        : CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_LowFareScheduleRS.xsl");

                    CoreLib.SendTrace(ProviderSystems.UserID, "SabreAirServices", "Response2", strResponse, ProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Sabre_LowFareSchedule2RS.xsl");
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
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.LowFareSchedule, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        private bool IsSameOD(ref DataView oDV, string CityAirport)
        {
            int i = oDV.Find(CityAirport);
            return i > -1;
        }
    }
}