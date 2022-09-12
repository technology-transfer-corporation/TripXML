using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using TripXMLMain;

namespace Worldspan
{
    public class AirServices : WorldspanBase
    {
        public AirServices()
        {
            Request = "";
        }
        
        public string AirPrice()
        {
            string strResponse;
            
            // *****************************************************************
            // Transform OTA AirPrice Request into Native Worldspan Request     *
            // ***************************************************************** 

            try
            {
                string strRequest = SetRequest("Worldspan_AirPriceRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response  *
                // ******************************************************************************* 
                var ttWA = SetAdapter(ProviderSystems); 
                strResponse = ttWA.SendMessage(strRequest);
                

                // **********************************************************************
                // Transform Native Worldspan AirPrice Response into OTA Response   *
                // ********************************************************************** 
                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_AirPriceRS.xsl");
                    
                    if (strResponse.Contains("<Success"))
                    {
                        var oDoc = new XmlDocument();
                        oDoc.LoadXml(strResponse);
                        var oRoot = oDoc.DocumentElement;

                        // calculate the year for the flights
                        var curDate = DateTime.Now;
                        foreach (XmlNode oNode in oRoot?.SelectNodes("PricedItineraries/PricedItinerary/AirItineraryPricingInfo/FareInfos/FareInfo/DepartureDate"))
                        {
                            string newDate;
                            int fMonth = Convert.ToInt32(oNode.InnerText.Substring(5, 2));
                            int fDay = Convert.ToInt32(oNode.InnerText.Substring(8, 2));
                            if (fMonth > curDate.Month)
                            {
                                // flight is this year
                                newDate = $"{curDate.Year}{oNode.InnerText.Substring(4)}";
                            }
                            else if (fMonth == curDate.Month)
                            {
                                // flight is the same month, check the day
                                newDate = fDay < curDate.Day ? $"{curDate.AddYears(1).Year}{oNode.InnerText.Substring(4)}" : $"{curDate.Year}{oNode.InnerText.Substring(4)}";
                            }
                            else
                            {
                                // flight is next year
                                newDate = $"{curDate.AddYears(1).Year}{oNode.InnerText.Substring(4)}";
                            }

                            oNode.InnerText = newDate;
                        }

                        strResponse = oRoot?.OuterXml;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }

            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.AirAvail, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string AirRules()
        {
            string strResponse;

            // *****************************************************************
            // Transform OTA AirRules Request into Native Worldspan Request   *
            // ***************************************************************** 

            try
            {
                string strRequest = Request;
                string strRuleReqInfo = GetNodeXml(ref strRequest, "RuleReqInfo", false, false);
                strRequest = SetRequest("Worldspan_AirRulesRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");
                
                // *******************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response*
                // ******************************************************************************* 
                var ttWA = SetAdapter(ProviderSystems);
                strResponse = ttWA.SendMessage(strRequest);

                // *****************************************************************
                // Transform Native Worldspan AirRules Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    if (strResponse.Contains("</FRW3>"))
                        strResponse = strResponse.Replace("</FRW3>",  $"{strRuleReqInfo}</FRW3>");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_AirRulesRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
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
            // Transform OTA AirSeatMap Request into Native Worldspan Request     *
            // ***************************************************************** 
            try
            {
                string strRequest = SetRequest("Worldspan_AirSeatMapRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response  *
                // ******************************************************************************* 
                WorldspanAdapter ttWA = SetAdapter(ProviderSystems);
                strResponse = ttWA.SendMessage(strRequest);

                // *****************************************************************
                // Transform Native Worldspan AirSeatMap Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_AirSeatMapRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.AirSeatMap, ex.Message, ProviderSystems);
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
                
                List<string> arLocations = new List<string>();
                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                var oRoot = oDoc.DocumentElement;
                int intODs = oRoot.SelectNodes("OriginDestinationInformation").Count;
                if (intODs > 1)
                {
                    foreach (XmlNode currentONode in oRoot.SelectNodes("OriginDestinationInformation"))
                    {
                        arLocations.Add(currentONode.SelectSingleNode("DestinationLocation").Attributes["LocationCode"].Value);
                    }
                }
                
                // *****************************************************************
                // Transform OTA LowFare Request into Native Worldspan Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Worldspan_LowFareRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response  *
                // ******************************************************************************* 
                var ttWA = SetAdapter(ProviderSystems);
                strResponse = ttWA.SendMessage(strRequest);
                

                // ************************************************************************
                // calculate year in all dates and arrival date based on change of day   *
                // ************************************************************************ 
                try
                {
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;

                    // If Not oRoot.SelectNodes("ALT_INF[BAS_FAR != '']/FLI_INF[LNK_IND != '']") Is Nothing Then
                    // For Each oNodeResp In oRoot.SelectNodes("ALT_INF[BAS_FAR != '']/FLI_INF[LNK_IND != '']")
                    if (oRoot.SelectNodes("ALT_INF[BAS_FAR != '']/FLI_INF") != null)
                    {
                        foreach (XmlNode currentONodeResp in oRoot.SelectNodes("ALT_INF[BAS_FAR != '']/FLI_INF"))
                        {
                            var oNodeResp = currentONodeResp;
                            var dtDepartureDate = Convert.ToDateTime($"{oNodeResp.SelectSingleNode("FLI_DAT")?.InnerText}{DateTime.Now.Year}");
                            
                            if (DateTime.Now.DayOfYear > dtDepartureDate.DayOfYear)
                            {
                                dtDepartureDate = dtDepartureDate.AddYears(1);
                            }

                            oNodeResp.SelectSingleNode("FLI_DAT").InnerText = dtDepartureDate.ToString("yyyy-MM-dd");
                            var oNode = oNodeResp.SelectSingleNode("DAY_CHG_IND");
                            if (oNode != null)
                            {
                                bool blnSameYear;
                                int intDayDiff = Convert.ToInt32(oNodeResp.SelectSingleNode("DEP_ARR_DAT_DIF").InnerText);
                                if (oNode.InnerText == "#")
                                {
                                    blnSameYear = DateTime.Now.Year == dtDepartureDate.AddDays(intDayDiff).Year;
                                    dtDepartureDate = dtDepartureDate.AddDays(intDayDiff);
                                }
                                else
                                {
                                    blnSameYear = DateTime.Now.Year == dtDepartureDate.AddDays(intDayDiff).Year;
                                    dtDepartureDate = dtDepartureDate.AddDays(-1 * intDayDiff);
                                }

                                if (DateTime.Now.DayOfYear > dtDepartureDate.DayOfYear & blnSameYear)
                                {
                                    dtDepartureDate = dtDepartureDate.AddYears(1);
                                }
                            }

                            oNode = oDoc.CreateNode(XmlNodeType.Element, "", "ARR_DAT", "");
                            oNode.InnerText = dtDepartureDate.ToString("yyyy-MM-dd");
                            oNodeResp.AppendChild(oNode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Calculating Dates.\r\n{ex.Message}");
                }

                // ************************************
                // Build Origin Destination Nodes    *
                // ************************************
                try
                {
                    foreach (XmlNode currentONodeResp1 in oRoot.SelectNodes("ALT_INF"))
                    {
                        var oNodeResp = currentONodeResp1;
                        int i = 1;
                        int j = 0;
                        bool blnNewOD = true;
                        int intDayDiff = oNodeResp.SelectNodes("FLI_INF").Count;
                        foreach (XmlNode oNodeFlight in oNodeResp.SelectNodes("FLI_INF"))
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
                                string cityAirport = $"{arLocations[i]}{oNodeFlight.SelectSingleNode("ARR_ARP")?.InnerText}";
                                blnNewOD = (cityAirport.Substring(0, 3) ?? "") == (cityAirport.Substring(3, 3) ?? "") || IsSameOD(ref ttCities, cityAirport);
                            }

                            if (oNode != null)
                            {
                                // Append Flight to OD
                                oNode.AppendChild(oNodeFlight);

                                // Add New OD Node
                                if (blnNewOD)
                                {
                                    i += 1;
                                    // Append OD to ALT_INF 
                                    oNodeResp.AppendChild(oNode);
                                }
                            }
                        }
                    }

                    strResponse = oRoot.OuterXml;
                    CoreLib.SendTrace(ProviderSystems.UserID, "AirServices", "OD Response", strResponse, ProviderSystems.LogUUID);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Building Origin Destination Nodes.\r\n{ex.Message}");
                }

                // *****************************************************************
                // Transform Native Worldspan LowFare Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_LowFareRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
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
            // Get ODs From LowFarePlus OTA Request  *
            // ************************************
            try
            {
                List<string> arLocations = new List<string>();
                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                var oRoot = oDoc.DocumentElement;
                int intODs = oRoot.SelectNodes("OriginDestinationInformation").Count;
                if (intODs > 1)
                {
                    foreach (XmlNode currentONode in oRoot.SelectNodes("OriginDestinationInformation"))
                    {
                        arLocations.Add(currentONode.SelectSingleNode("DestinationLocation")?.Attributes?["LocationCode"].Value);
                    }
                }

                // *****************************************************************
                // Transform OTA LowFarePlus Request into Native Worldspan Request     *
                // ***************************************************************** 
                string strRequest = SetRequest("Worldspan_LowFarePlusRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response  *
                // ******************************************************************************* 
                modCore.TripXMLProviderSystems ttProviderSystems = ProviderSystems;
                ttProviderSystems.Profile = ProviderSystems.ProfileXML;
                var ttWA = SetAdapter(ttProviderSystems);
                strResponse = ttWA.SendMessage(strRequest);

                // ************************************************************************
                // calculate year in all dates and arrival date based on change of day   *
                // ************************************************************************ 
                try
                {
                    oDoc.LoadXml(strResponse);
                    oRoot = oDoc.DocumentElement;

                    // If Not oRoot.SelectNodes("ALT_INF[BAS_FAR != '']/FLI_INF[LNK_IND != '']") Is Nothing Then
                    // For Each oNodeResp In oRoot.SelectNodes("ALT_INF[BAS_FAR != '']/FLI_INF[LNK_IND != '']")
                    if (oRoot.SelectNodes("ALT_INF[BAS_FAR != '']/FLI_INF") != null)
                    {
                        foreach (XmlNode currentONodeResp in oRoot.SelectNodes("ALT_INF[BAS_FAR != '']/FLI_INF"))
                        {
                            var oNodeResp = currentONodeResp;
                            var dtDepartureDate = Convert.ToDateTime($"{oNodeResp.SelectSingleNode("FLI_DAT").InnerText}{DateTime.Now.Year}");

                            if (DateTime.Now.DayOfYear > dtDepartureDate.DayOfYear)
                            {
                                dtDepartureDate = dtDepartureDate.AddYears(1);
                            }

                            oNodeResp.SelectSingleNode("FLI_DAT").InnerText = dtDepartureDate.ToString("yyyy-MM-dd");
                            var oNode = oNodeResp.SelectSingleNode("DAY_CHG_IND");
                            if (oNode != null)
                            {
                                bool blnSameYear;
                                int intDayDiff = Convert.ToInt32(oNodeResp.SelectSingleNode("DEP_ARR_DAT_DIF").InnerText);
                                if (oNode.InnerText == "#")
                                {
                                    blnSameYear = DateTime.Now.Year == dtDepartureDate.AddDays(intDayDiff).Year;
                                    dtDepartureDate = dtDepartureDate.AddDays(intDayDiff);
                                }
                                else
                                {
                                    blnSameYear = DateTime.Now.Year == dtDepartureDate.AddDays(intDayDiff).Year;
                                    dtDepartureDate = dtDepartureDate.AddDays(-1 * intDayDiff);
                                }

                                if (DateTime.Now.DayOfYear > dtDepartureDate.DayOfYear & blnSameYear)
                                {
                                    dtDepartureDate = dtDepartureDate.AddYears(1);
                                }
                            }

                            oNode = oDoc.CreateNode(XmlNodeType.Element, "", "ARR_DAT", "");
                            oNode.InnerText = dtDepartureDate.ToString("yyyy-MM-dd");
                            oNodeResp.AppendChild(oNode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Calculating Dates.\r\n{ex.Message}");
                }

                // ************************************
                // Build Origin Destination Nodes    *
                // ************************************ 

                try
                {
                    foreach (XmlNode currentONodeResp1 in oRoot.SelectNodes("ALT_INF"))
                    {
                        var oNodeResp = currentONodeResp1;
                        int i = 1;
                        int j = 0;
                        bool blnNewOD = true;
                        int intDayDiff = oNodeResp.SelectNodes("FLI_INF").Count;
                        foreach (XmlNode oNodeFlight in oNodeResp.SelectNodes("FLI_INF"))
                        {
                            XmlNode oNode = null;
                            j ++;
                            if (blnNewOD)
                                oNode = oDoc.CreateNode(XmlNodeType.Element, "", "OriginDestination", "");

                            if (intODs == 1)
                            {
                                blnNewOD = j == intDayDiff;
                            }
                            else
                            {
                                var cityAirport = $"{arLocations[i]}{oNodeFlight.SelectSingleNode("ARR_ARP").InnerText}";
                                blnNewOD = (cityAirport.Substring(0, 3) ?? "") == (cityAirport.Substring(3, 3) ?? "") || IsSameOD(ref ttCities, cityAirport);
                            }

                            if (oNode != null)
                            {
                                // Append Flight to OD
                                oNode.AppendChild(oNodeFlight);

                                // Add New OD Node
                                if (blnNewOD)
                                {
                                    i += 1;
                                    // Append OD to ALT_INF 
                                    oNodeResp.AppendChild(oNode);
                                }
                            }
                        }
                    }

                    strResponse = oRoot.OuterXml;
                    CoreLib.SendTrace(ProviderSystems.UserID, "AirServices", "OD Response", strResponse,
                        ProviderSystems.LogUUID);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Building Origin Destination Nodes.\r\n{ex.Message}");
                }

                // *****************************************************************
                // Transform Native Worldspan LowFarePlus Response into OTA Response   *
                // ***************************************************************** 

                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_LowFarePlusRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.LowFarePlus, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        public string FareDisplay()
        {
            string strResponse;
            
            // *****************************************************************
            // Transform OTA FareDisplay Request into Native Worldspan Request     *
            // ***************************************************************** 

            try
            {
                string strRequest = SetRequest("Worldspan_FareDisplayRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                // Send Transformed Request to the Worldspan Adapter and Getting Native Response  *
                // ******************************************************************************* 
                var ttWA = SetAdapter(ProviderSystems); 
                strResponse = ttWA.SendMessage(strRequest);
                
                // ****************************************************************
                // Add OriginDestinationInformation Request to Native Response   *
                // ****************************************************************
                try
                {
                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(strResponse);
                    var oRoot = oDoc.DocumentElement;
                    var oNode = oDoc.CreateNode(XmlNodeType.Element, "", "Request", "");
                    oNode.InnerXml = Request;
                    oRoot?.AppendChild(oNode);
                    strResponse = oDoc.OuterXml;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Loading OTA Request into Native Response.\r\n{ex.Message}");
                }

                // **********************************************************************
                // Transform Native Worldspan FareDisplay Response into OTA Response   *
                // ********************************************************************** 
                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Worldspan_FareDisplayRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.FareDisplay, ex.Message, ProviderSystems);
            }

            return strResponse;
        }

        private bool IsSameOD(ref DataView oDv, string cityAirport)
        {
            int i = oDv.Find(cityAirport);
            return i > -1;
        }

        private string GetNodeXml(ref string xmlData, string sNode, bool innerXml = false, bool retData = true)
        {
            int intStart;
            int intLength;
            if (!xmlData.Contains($"<{sNode}>") & !xmlData.Contains($"<{sNode} "))
            {
                return retData ? xmlData : "";
            }

            if (innerXml)
            {
                intStart = !xmlData.Contains($"<{sNode}>")
                    ? xmlData.IndexOf($"<{sNode} ", StringComparison.Ordinal) + sNode.Length + 27
                    : xmlData.IndexOf($"<{sNode}>", StringComparison.Ordinal) + sNode.Length + 2;

                intLength = xmlData.IndexOf($"</{sNode}>", StringComparison.Ordinal) - intStart;
            }
            else
            {
                intStart = xmlData.IndexOf(xmlData.IndexOf($"<{sNode}>", StringComparison.Ordinal) == -1 ? $"<{sNode} " : $"<{sNode}>", StringComparison.Ordinal);
                intLength = xmlData.IndexOf($"</{sNode}>", StringComparison.Ordinal) + sNode.Length + 3 - intStart;
            }

            return xmlData.Substring(intStart, intLength).Replace("\r", "").Replace("\n", "").Trim();
        }
    }
}