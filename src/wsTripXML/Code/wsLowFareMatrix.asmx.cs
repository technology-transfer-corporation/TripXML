using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using TripXMLTools;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsLowFareMatrix
    {

        private string mstrResponse = "";
        private int mintProviders = 0;

        public void GotResponse(string Response)
        {
            mstrResponse += Response;
            mintProviders += 1;
        }

        public TripXML tXML;

        private readonly modMain _modMain;

        public wsLowFareMatrix(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Functions 

        private string DecodeLowFareMatrix(string strResponse, string UserID)
        {
            try
            {

                var oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                var oRoot = oDoc.DocumentElement;
                foreach (XmlNode oNode in oRoot.SelectNodes("PricedItineraries/PricedItinerary"))
                {
                    foreach (XmlNode oFlightNode in oNode.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"))
                    {
                        // *******************
                        // *******************
                        // Decode Airports   *
                        // *******************
                        oFlightNode.SelectSingleNode("DepartureAirport").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oFlightNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value);
                        // GetDecodeValue(ttAirports, oFlightNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                        oFlightNode.SelectSingleNode("ArrivalAirport").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oFlightNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value);
                        // GetDecodeValue(ttAirports, oFlightNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)

                        // *******************
                        // Decode Airlines   *
                        // *******************
                        if (oFlightNode.SelectSingleNode("OperatingAirline") is not null & oFlightNode.SelectSingleNode("OperatingAirline/@Code") is not null)
                        {
                            // oFareNode = oNode.SelectSingleNode("AirItineraryPricingInfo").Attributes("PricingSource")

                            // If oFareNode.InnerText = "Private" Then
                            // oFlightNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttHiddenAirlines, oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)

                            // If oFlightNode.SelectSingleNode("OperatingAirline").InnerText = "" Then
                            // oFlightNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                            // End If
                            // Else
                            if (!string.IsNullOrEmpty(oFlightNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value) & string.IsNullOrEmpty(oFlightNode.SelectSingleNode("OperatingAirline").InnerText))
                            {
                                oFlightNode.SelectSingleNode("OperatingAirline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oFlightNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value);
                                // GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                            }
                            // End If

                        }

                        if (oFlightNode.SelectSingleNode("MarketingAirline") is not null)
                        {

                            // If oFareNode.InnerText = "Private" Then
                            // oFlightNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttHiddenAirlines, oFlightNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)

                            // If oFlightNode.SelectSingleNode("MarketingAirline").InnerText = "" Then
                            // oFlightNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                            // End If
                            // Else
                            if (string.IsNullOrEmpty(oFlightNode.SelectSingleNode("MarketingAirline").InnerText))
                            {
                                oFlightNode.SelectSingleNode("MarketingAirline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oFlightNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                                // GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                            }
                            // End If
                        }

                        // *******************
                        // Decode Equipments *
                        // *******************
                        if (oFlightNode.SelectSingleNode("Equipment") is not null)
                        {
                            oFlightNode.SelectSingleNode("Equipment").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Equipment, oFlightNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value);
                            // GetDecodeValue(ttEquipments, oFlightNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                        }
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding LowFareMatrix Response", ex.Message, string.Empty);
            }
            return strResponse;
        }

        #endregion

        #region  Filter Flights 

        private struct FlightSegment
        {
            public string DepartureDate;
            public string ArrivalDate;
            public string FlightNo;
            public string DepartureAirport;
            public string ArrivalAirport;
            public string AirlineCode;
        }

        private string RemoveLeadingZeros(string FlightNo)
        {
            int i;

            var loopTo = FlightNo.Length - 1;
            for (i = 0; i <= loopTo; i++)
            {
                if (string.Compare(FlightNo.Substring(i, 1), "0") != 0)
                    break;
            }

            return FlightNo.Substring(i);

        }

        private void FilterFlights(ref string strResponse, string FeaturedProvider)
        {
            string Provider = "";
            float TotalFare;
            string Fare = "";
            string offID = "";
            FlightSegment[] FlightSegments = null;
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlNode oNodeOnd = null;
            XmlNode oNodeFlight = null;
            XmlNode oNote = null;
            int i;
            int j;
            int k;
            int ItineraryCount;
            bool SameFlight;

            try
            {
                strResponse = strResponse.Replace("Provider=", "Flag=\"\" Provider=");

                oDoc = new XmlDocument();

                oDoc.LoadXml(strResponse);

                oRoot = oDoc.DocumentElement;

                ItineraryCount = oRoot.SelectNodes("PricedItineraries/PricedItinerary").Count;

                var loopTo = ItineraryCount - 2;
                for (i = 0; i <= loopTo; i++) // Don't get the lastone.
                {
                    // Get the Node for comparison
                    oNode = oRoot.SelectNodes("PricedItineraries/PricedItinerary").Item(i);
                    Provider = oNode.Attributes["Provider"].Value;
                    oNodeOnd = oNode.SelectSingleNode("AirItineraryPricingInfo/ItinTotalFare/TotalFare");
                    Fare = oNodeOnd.Attributes["Amount"].Value;
                    Fare = Fare.Insert(Fare.Length - Conversions.ToInteger(oNodeOnd.Attributes["DecimalPlaces"].Value), ".");
                    TotalFare = Conversions.ToSingle(Fare);
                    if (FlightSegments is not null)
                        FlightSegments = null;
                    j = 0;
                    foreach (XmlNode currentONodeOnd in oNode.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption"))
                    {
                        oNodeOnd = currentONodeOnd;
                        foreach (XmlNode currentONodeFlight in oNodeOnd.SelectNodes("FlightSegment"))
                        {
                            oNodeFlight = currentONodeFlight;
                            Array.Resize(ref FlightSegments, j + 1);
                            {
                                ref var withBlock = ref FlightSegments[j];
                                withBlock.DepartureDate = oNodeFlight.Attributes["DepartureDateTime"].Value;
                                withBlock.ArrivalDate = oNodeFlight.Attributes["ArrivalDateTime"].Value;
                                withBlock.FlightNo = RemoveLeadingZeros(oNodeFlight.Attributes["FlightNumber"].Value);
                                withBlock.DepartureAirport = oNodeFlight.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value;
                                withBlock.ArrivalAirport = oNodeFlight.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value;
                                withBlock.AirlineCode = oNodeFlight.SelectSingleNode("MarketingAirline").Attributes["Code"].Value;
                            }
                            j += 1;
                        }    // oNodeFlight
                    }    // oNodeOnd
                    // Loop thru the Nodes below and compare
                    var loopTo1 = ItineraryCount - 1;
                    for (k = i + 1; k <= loopTo1; k++)
                    {
                        // First check that it has the same Number of Flight
                        oNode = oRoot.SelectNodes("PricedItineraries/PricedItinerary").Item(k);
                        if (string.Compare(Provider, oNode.Attributes["Provider"].Value) != 0 & oNode.Attributes["Flag"].Value.Length == 0)
                        {
                            j = 0;
                            foreach (XmlNode currentONodeOnd1 in oNode.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption"))
                            {
                                oNodeOnd = currentONodeOnd1;
                                foreach (XmlNode currentONodeFlight1 in oNodeOnd.SelectNodes("FlightSegment"))
                                {
                                    oNodeFlight = currentONodeFlight1;
                                    j += 1;
                                }    // oNodeFlight
                            }    // oNodeOnd
                            if (j == FlightSegments.Length)
                            {
                                // Do comparissons
                                j = 0;
                                SameFlight = true;
                                foreach (XmlNode currentONodeOnd2 in oNode.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption"))
                                {
                                    oNodeOnd = currentONodeOnd2;
                                    foreach (XmlNode currentONodeFlight2 in oNodeOnd.SelectNodes("FlightSegment"))
                                    {
                                        oNodeFlight = currentONodeFlight2;
                                        {
                                            ref var withBlock1 = ref FlightSegments[j];
                                            if (!((withBlock1.DepartureDate ?? "") == (oNodeFlight.Attributes["DepartureDateTime"].Value ?? "") & (withBlock1.ArrivalDate ?? "") == (oNodeFlight.Attributes["ArrivalDateTime"].Value ?? "") & (withBlock1.FlightNo ?? "") == (RemoveLeadingZeros(oNodeFlight.Attributes["FlightNumber"].Value) ?? "") & (withBlock1.DepartureAirport ?? "") == (oNodeFlight.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value ?? "") & (withBlock1.ArrivalAirport ?? "") == (oNodeFlight.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value ?? "") & (withBlock1.AirlineCode ?? "") == (oNodeFlight.SelectSingleNode("MarketingAirline").Attributes["Code"].Value ?? "")))
                                            {
                                                SameFlight = false;
                                                break;
                                            }
                                        }
                                        j += 1;
                                    }    // oNodeFlight
                                    if (!SameFlight)
                                        break;
                                }    // oNodeOnd
                                if (SameFlight)
                                {
                                    // Check Price
                                    oNodeOnd = oNode.SelectSingleNode("AirItineraryPricingInfo/ItinTotalFare/TotalFare");
                                    Fare = oNodeOnd.Attributes["Amount"].Value;
                                    Fare = Fare.Insert(Fare.Length - Conversions.ToInteger(oNodeOnd.Attributes["DecimalPlaces"].Value), ".");
                                    bool exitFor = false;
                                    switch (Conversions.ToSingle(Fare))
                                    {
                                        case var @case when @case == TotalFare:
                                            {
                                                // Same Price Check Provider (OfficeID)
                                                if (string.Compare(oNode.Attributes["Provider"].Value, FeaturedProvider) != 0)
                                                {
                                                    oNode.Attributes["Flag"].Value = "Deleted";
                                                    offID = oNode.Attributes["Provider"].Value;
                                                    oNode = oRoot.SelectNodes("PricedItineraries/PricedItinerary").Item(i);
                                                    oNote = oDoc.CreateNode(XmlNodeType.Element, "Notes", "");
                                                    oNote.InnerText = offID;
                                                    oNode.AppendChild(oNote);
                                                }
                                                else
                                                {
                                                    offID = oRoot.SelectNodes("PricedItineraries/PricedItinerary").Item(i).Attributes["Provider"].Value;
                                                    oNote = oDoc.CreateNode(XmlNodeType.Element, "Notes", "");
                                                    oNote.InnerText = offID;
                                                    oNode.AppendChild(oNote);
                                                    oNode = oRoot.SelectNodes("PricedItineraries/PricedItinerary").Item(i);
                                                    oNode.Attributes["Flag"].Value = "Deleted";
                                                    exitFor = true;
                                                    break;
                                                }

                                                break;
                                            }
                                        case var case1 when case1 > TotalFare:
                                            {
                                                // Delete this one (B)
                                                oNode.Attributes["Flag"].Value = "Deleted";
                                                break;
                                            }
                                        case var case2 when case2 < TotalFare:
                                            {
                                                // Delete Node A
                                                oNode = oRoot.SelectNodes("PricedItineraries/PricedItinerary").Item(i);
                                                oNode.Attributes["Flag"].Value = "Deleted";
                                                // Skip Further Node A Comparison
                                                exitFor = true;
                                                break;
                                            }
                                    }

                                    if (exitFor)
                                    {
                                        break;
                                    }
                                }  // Same Flight
                            }  // Same Number of Flights
                        }  // Not Deleted
                    }    // k = ItineraryCount Node B
                }    // i = ItineraryCount Node A

                // Delete From Xml Documents all Flag as Deleted
                // Leave the Deleted Flag for Testing.
                oNode = oRoot.SelectSingleNode("PricedItineraries");
                foreach (XmlNode currentONodeOnd3 in oNode.SelectNodes("PricedItinerary"))
                {
                    oNodeOnd = currentONodeOnd3;
                    if (oNodeOnd.Attributes["Flag"].Value == "Deleted")
                    {
                        oNode.RemoveChild(oNodeOnd);
                    }
                }

                oNode = oRoot.SelectSingleNode("PricedItineraries");
                j = oNode.SelectNodes("PricedItinerary").Count;
                if (j > 200)
                {
                    for (i = j - 1; i >= 200; i -= 1)
                    {
                        oNodeOnd = oNode.SelectNodes("PricedItinerary").Item(i);
                        oNode.RemoveChild(oNodeOnd);
                    }
                }

                // Remove the empty Flags
                strResponse = oDoc.OuterXml.Replace("Flag=\"\" ", "");
            }

            // Error Filtering Flights
            catch (Exception ex)
            {
            }
            finally
            {
                oDoc = null;
            }

        }

        #endregion

        #region  Process Service Request All GDS 

        private string ServiceRequest(string strRequest, ttServices ttServiceID)
        {
            string strResponse = "";
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            bool ValidateXSDOut;
            var StartTime = default(DateTime);
            string UUID = "";
            int i;
            DateTime StartCounter;
            // Dim DoAmadeusSearches(99) As SearchAmadeusMatrix
            var DoAmadeusWSSearches = new SearchAmadeusMatrixWS[100];
            var DoGalileoSearches = new SearchGalileoMatrix[100];
            var DoSabreSearches = new SearchSabreMatrix[100];
            var DoWorldspanSearches = new SearchWorldspanMatrix[100];
            // Dim DoPortalSearches(99) As SearchPortalMatrix
            // Dim DoPortalXMLSearches(99) As SearchPortalMatrixXML
            StringBuilder sb = null;
            string uTravelSum = "";
            string TravelSum = "";
            // Static MultipleCount As Integer = 0
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            string FaringPreference = "";
            string OriginDestination = "";
            var lfstrRequest = new string[100];
            int j = 0;

            sb = new StringBuilder();

            try
            {
                StartTime = DateTime.Now;

                // oDoc = New XmlDocument
                // oDoc.LoadXml(strRequest)
                // oRoot = oDoc.DocumentElement

                // sb.Append("<OTA_AirLowFareSearchPlusRQ>").Append(oRoot.SelectSingleNode("POS").OuterXml)


                // For Each oNode In oRoot.SelectNodes("OriginDestinationInformation")
                // sb.Append(oNode.OuterXml)
                // Next

                // 'uTravelSum = sb.Append(oRoot.SelectSingleNode("TravelerInfoSummary").OuterXml).ToString()
                // 'sb.Append("<FaringPreferences>").ToString()

                // 'For Each oNode In oRoot.SelectNodes("FaringPreferences/FaringPreference")
                // '    sb.Append(oNode.OuterXml)
                // '    sb.Remove(0, sb.Length)
                // '    lfstrRequest(j) = sb.Append(uTravelSum).Append(oNode.OuterXml).Append("</OTA_AirLowFareSearchPlusRQ>").ToString
                // '    j += 1

                // 'Next

                // 'sb.Remove(0, sb.Length)

                // TravelSum = sb.ToString()
                // uTravelSum = oRoot.SelectSingleNode("TravelerInfoSummary").InnerXml.ToString()

                // For Each oNode In oRoot.SelectNodes("FaringPreferences/FaringPreference")

                // sb.Remove(0, sb.Length)
                // sb.Append(TravelSum).Append(oNode.SelectSingleNode("TravelPreferences").OuterXml)
                // sb.Append("<TravelerInfoSummary>").Append(uTravelSum).Append(oNode.SelectSingleNode("AirTravelerAvail").OuterXml)
                // sb.Append(oNode.SelectSingleNode("PriceRequestInformation").OuterXml).Append("</TravelerInfoSummary></OTA_AirLowFareSearchPlusRQ>")

                // lfstrRequest(j) = sb.ToString()
                // j += 1
                // Next

                // sb.Remove(0, sb.Length)
                _modMain.PreServiceRequestPool(ref strRequest, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                sb.Append("XSD").Append(ttCredential.UserID).Append("Out");
                ValidateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get(sb.ToString()));
                sb.Remove(0, sb.Length);

                // strRequest= "<?xml version="1.0" encoding="utf-16"?><OTA_AirLowFareSearchPlusRQ><POS><Source PseudoCityCode="MIA1S21AV"><RequestorID Type="21" ID="Thomalex" /></Source><TPA_Extensions><Provider><Name>Amadeus</Name><System>Test</System><Userid>Thomalex</Userid><Password>thefalls</Password></Provider></TPA_Extensions></POS><OriginDestinationInformation><DepartureDateTime>2010-03-04T00:00:00</DepartureDateTime><OriginLocation LocationCode="ATL" /><DestinationLocation LocationCode="MIA" /></OriginDestinationInformation><TravelerInfoSummary><SeatsRequested>1</SeatsRequested><FaringPreferences><FaringPreference PseudoCityCode="ATL1S2157"><TravelPreferences><VendorPref Code="DL" PreferLevel="Preferred"/><VendorPref Code="UA" PreferLevel="Preferred"/><CabinPref PreferLevel="Preferred" Cabin="Economy"/></TravelPreferences><AirTravelerAvail><PassengerTypeQuantity Code="JCB" Quantity="1"/></AirTravelerAvail><PriceRequestInformation PricingSource="Private"/></FaringPreference><FaringPreference PseudoCityCode="ATL1S2157"><TravelPreferences><VendorPref Code="AA" PreferLevel </AirTravelerAvail><PriceRequestInformation PricingSource="Published"/></FaringPreference><FaringPreference PseudoCityCode="NYC1S218Z"><TravelPreferences><VendorPref Code="AA" PreferLevel="Preferred"/><CabinPref PreferLevel="Preferred" Cabin="Business"/></TravelPreferences><AirTravelerAvail><PassengerTypeQuantity Code="JCB" Quantity="1"/></AirTravelerAvail><PriceRequestInformation PricingSource="Private"/></FaringPreference></FaringPreferences></TravelerInfoSummary></OTA_AirLowFareSearchPlusRQ>"

                {
                    ref var withBlock = ref ttCredential;
                    var loopTo = withBlock.Providers.Length - 1;
                    for (i = 0; i <= loopTo; i++)
                    {
                        switch (withBlock.Providers[i].Name.ToLower() ?? "")
                        {
                            case "amadeus":
                                {
                                    try
                                    {
                                        // Dim ttAA As AmadeusAPIAdapter

                                        sb.Append("API").Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC);
                                        // ttAA = TripXMLMain.AppState.Get(sb.ToString())
                                        sb.Remove(0, sb.Length);

                                        // If ttAA Is Nothing Then
                                        ttProviderSystems = (TripXMLProviderSystems)TripXMLMain.AppState.Get(sb.Append("PS").Append(ttCredential.Providers[i].Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers[i].PCC).ToString());
                                        sb.Remove(0, sb.Length);

                                        if (ttProviderSystems.AmadeusWS == false)
                                        {
                                            sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.");
                                            GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), withBlock.Providers[i].Name));
                                            sb.Remove(0, sb.Length);
                                            break;
                                        }
                                        // End If

                                        if (ttProviderSystems.AmadeusWS == true)
                                        {
                                            if (ttCredential.Providers[i].PCC.Trim().Length > 0)
                                            {
                                                ttProviderSystems.PCC = ttCredential.Providers[i].PCC;
                                            }

                                            ttCredential.Providers[0].Name = "AmadeusWS";

                                            if (ttCredential.System == "Test")
                                            {
                                                ttProviderSystems.URL = "https://test.webservices.amadeus.com";
                                            }
                                            else if (ttCredential.System == "Training")
                                            {
                                                ttProviderSystems.URL = "https://production.webservices.amadeus.com";
                                            }
                                            else
                                            {
                                                ttProviderSystems.URL = "https://production.webservices.amadeus.com";
                                            }




                                            var oAmadeusWS = new cServiceAmadeusWS();
                                            oAmadeusWS.GotResponse += GotResponse;

                                            {
                                                ref var withBlock1 = ref oAmadeusWS;
                                                withBlock1.ServiceID = (int)ttServiceID;
                                                // .Request = strRequest
                                                withBlock1.Request = strRequest;
                                                withBlock1.ttProviderSystems = ttProviderSystems;
                                                // .ttProviderSystems.ProviderSession.MultipleCount = .ttProviderSystems.ProviderSession.MultipleCount + 1
                                                // If .ttProviderSystems.ProviderSession.MultipleCount <> 1 Then
                                                // .ttProviderSystems.ProviderSession.MultipleAccess = False
                                                // Else
                                                // .ttProviderSystems.ProviderSession.MultipleAccess = True
                                                // End If
                                                withBlock1.Version = "";
                                            }

                                            DoAmadeusWSSearches[i] = new SearchAmadeusMatrixWS(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oAmadeusWS);
                                            // DoAmadeusWSSearches(i).Request = strRequest
                                            DoAmadeusWSSearches[i].Request = strRequest;
                                            DoAmadeusWSSearches[i].ServiceID = ((int)ttServiceID).ToString();
                                            DoAmadeusWSSearches[i].BeginSearch();

                                            ttProviderSystems = default;
                                            // Else
                                            // ttProviderSystems = ttAA.ttProviderSystems

                                            // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                            // ttAA.SourcePCC = ttCredential.Providers(i).PCC
                                            // Else
                                            // ttAA.SourcePCC = ttAA.ttProviderSystems.PCC
                                            // End If

                                            // Dim oAmadeus As New cServiceAmadeus
                                            // AddHandler oAmadeus.GotResponse, AddressOf GotResponse

                                            // With oAmadeus
                                            // .ServiceID = ttServiceID
                                            // '.Request = strRequest
                                            // .Request = strRequest
                                            // .ttAA = ttAA
                                            // .Version = ""
                                            // End With

                                            // DoAmadeusSearches(i) = New SearchAmadeusMatrix(.Providers(i).PCC, .UserID, .System, ttAA, oAmadeus)
                                            // 'DoAmadeusSearches(i).Request = strRequest
                                            // DoAmadeusSearches(i).Request = strRequest
                                            // DoAmadeusSearches(i).ServiceID =CInt(ttServiceID).ToString()
                                            // DoAmadeusSearches(i).BeginSearch()

                                            // sb.Append("API").Append(.UserID).Append(.System)
                                            // TripXMLMain.AppState.Set(sb.ToString(), ttAA)
                                            // sb.Remove(0, sb.Length)
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        GotResponse(FormatErrorMessage(ttServiceID, e.Message, withBlock.Providers[i].Name));

                                    }

                                    break;
                                }

                            case "apollo":
                            case "galileo":
                                {
                                    try
                                    {
                                        sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC);
                                        ttProviderSystems = (TripXMLProviderSystems)TripXMLMain.AppState.Get(sb.ToString());
                                        sb.Remove(0, sb.Length);

                                        if (ttProviderSystems.System is null)
                                        {
                                            sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.");
                                            GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), withBlock.Providers[i].Name));
                                            sb.Remove(0, sb.Length);
                                            break;
                                        }

                                        if (ttCredential.Providers[i].PCC.Trim().Length > 0)
                                        {
                                            ttProviderSystems.PCC = ttCredential.Providers[i].PCC;
                                        }

                                        var oGalileo = new cServiceGalileo();
                                        oGalileo.GotResponse += GotResponse;

                                        {
                                            ref var withBlock2 = ref oGalileo;
                                            withBlock2.ServiceID = (int)ttServiceID;
                                            // .Request = strRequest
                                            withBlock2.Request = strRequest;
                                            withBlock2.ProviderSystems = ttProviderSystems;
                                            withBlock2.Version = "";
                                        }

                                        DoGalileoSearches[i] = new SearchGalileoMatrix(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oGalileo);
                                        DoGalileoSearches[i].Request = strRequest;
                                        DoGalileoSearches[i].ServiceID = ((int)ttServiceID).ToString();
                                        DoGalileoSearches[i].BeginSearch();
                                    }

                                    catch (Exception e)
                                    {
                                        GotResponse(FormatErrorMessage(ttServiceID, e.Message, withBlock.Providers[i].Name));
                                    }

                                    break;
                                }

                            case "sabre":
                            case "Sabre":
                                {
                                    try
                                    {
                                        sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC);
                                        ttProviderSystems = (TripXMLProviderSystems)TripXMLMain.AppState.Get(sb.ToString());
                                        sb.Remove(0, sb.Length);

                                        if (ttProviderSystems.System is null)
                                        {
                                            sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.");
                                            GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), withBlock.Providers[i].Name));
                                            sb.Remove(0, sb.Length);
                                            break;
                                        }

                                        ttProviderSystems.AAAPCC = withBlock.Providers[i].PCC;

                                        // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                        // ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                        // End If

                                        var oSabre = new cServiceSabre();
                                        oSabre.GotResponse += GotResponse;

                                        DataView ttCities;
                                        ttCities = (DataView)TripXMLMain.AppState.Get("ttCities");

                                        {
                                            ref var withBlock3 = ref oSabre;
                                            withBlock3.ServiceID = ttServiceID;
                                            withBlock3.Request = strRequest;
                                            withBlock3.ProviderSystems = ttProviderSystems;
                                            withBlock3.Version = "";
                                            withBlock3.ttCities = ttCities;
                                        }

                                        DoSabreSearches[i] = new SearchSabreMatrix(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oSabre);
                                        DoSabreSearches[i].Request = strRequest;
                                        DoSabreSearches[i].ServiceID = ((int)ttServiceID).ToString();
                                        DoSabreSearches[i].BeginSearch();
                                    }

                                    catch (Exception e)
                                    {
                                        GotResponse(FormatErrorMessage(ttServiceID, e.Message, withBlock.Providers[i].Name));
                                    }

                                    break;
                                }

                            case "worldspan":
                            case "Worldspan":
                                {
                                    try
                                    {
                                        sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC);
                                        ttProviderSystems = (TripXMLProviderSystems)TripXMLMain.AppState.Get(sb.ToString());
                                        sb.Remove(0, sb.Length);

                                        if (ttProviderSystems.System is null)
                                        {
                                            sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.");
                                            GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), withBlock.Providers[i].Name));
                                            sb.Remove(0, sb.Length);
                                            break;
                                        }

                                        if (ttCredential.Providers[i].PCC.Trim().Length > 0)
                                        {
                                            ttProviderSystems.PCC = ttCredential.Providers[i].PCC;
                                        }

                                        var oWorldspan = new cServiceWorldspan();
                                        oWorldspan.GotResponse += GotResponse;

                                        DataView ttCities;
                                        ttCities = (DataView)TripXMLMain.AppState.Get("ttCities");

                                        {
                                            ref var withBlock4 = ref oWorldspan;
                                            withBlock4.ServiceID = ttServiceID;
                                            withBlock4.Request = strRequest;
                                            withBlock4.ProviderSystems = ttProviderSystems;
                                            withBlock4.Version = "";
                                            withBlock4.ttCities = ttCities;
                                        }

                                        DoWorldspanSearches[i] = new SearchWorldspanMatrix(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oWorldspan);
                                        DoWorldspanSearches[i].Request = strRequest;
                                        DoWorldspanSearches[i].ServiceID = ((int)ttServiceID).ToString();
                                        DoWorldspanSearches[i].BeginSearch();
                                    }

                                    catch (Exception e)
                                    {
                                        GotResponse(FormatErrorMessage(ttServiceID, e.Message, withBlock.Providers[i].Name));
                                    }

                                    break;
                                }

                            case "portal":
                                {
                                    break;
                                }
                            // Try
                            // sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                            // ttProviderSystems = TripXMLMain.AppState.Get(sb.ToString())
                            // sb.Remove(0, sb.Length)

                            // If ttProviderSystems.System Is Nothing Then
                            // sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.")
                            // GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), .Providers(i).Name))
                            // sb.Remove(0, sb.Length)
                            // Exit Select
                            // End If

                            // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                            // ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                            // End If

                            // Dim oPortal As New cServicePortal
                            // AddHandler oPortal.GotResponse, AddressOf GotResponse

                            // With oPortal
                            // .ServiceID = ttServiceID
                            // .Request = strRequest
                            // .ProviderSystems = ttProviderSystems
                            // .Version = ""
                            // End With

                            // DoPortalSearches(i) = New SearchPortalMatrix(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oPortal)
                            // DoPortalSearches(i).Request = strRequest
                            // DoPortalSearches(i).ServiceID =CInt(ttServiceID).ToString()
                            // DoPortalSearches(i).BeginSearch()

                            // Catch e As Exception
                            // GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                            // End Try

                            // Try
                            // sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                            // ttProviderSystems = TripXMLMain.AppState.Get(sb.ToString())
                            // sb.Remove(0, sb.Length)

                            // If ttProviderSystems.System Is Nothing Then
                            // sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.")
                            // GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), .Providers(i).Name))
                            // sb.Remove(0, sb.Length)
                            // Exit Select
                            // End If

                            // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                            // ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                            // End If

                            // Dim oPortalXML As New cServicePortalXML
                            // AddHandler oPortalXML.GotResponse, AddressOf GotResponse

                            // With oPortalXML
                            // .ServiceID = ttServiceID
                            // .Request = strRequest
                            // .ProviderSystems = ttProviderSystems
                            // .Version = ""
                            // End With

                            // DoPortalXMLSearches(i) = New SearchPortalMatrixXML(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oPortalXML)
                            // DoPortalXMLSearches(i).Request = strRequest
                            // DoPortalXMLSearches(i).ServiceID =CInt(ttServiceID).ToString()
                            // DoPortalXMLSearches(i).BeginSearch()

                            // Catch e As Exception
                            // GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                            // End Try
                            case "portalxml":
                                {
                                    break;
                                }

                            default:
                                {
                                    sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.");
                                    throw new Exception(sb.ToString());
                                    sb.Remove(0, sb.Length);
                                    break;
                                }
                        }
                    }

                }

                StartCounter = DateTime.Now;

                while (mintProviders < ttCredential.Providers.Length)
                {
                    if ((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalSeconds) > modMain.CPrdTimeOut)
                        break;
                    System.Threading.Thread.Sleep(10);
                }

                if (ttCredential.Providers.Length > 1)
                {
                    strResponse = string.Concat("<SuperRS>", mstrResponse, "</SuperRS>");
                    // Aggregate
                    cAggregation.Aggregate(ttServiceID, XslPath, "", ref strResponse);

                    // Filter Flights

                    if (ttProviderSystems.AggFilter == true)
                    {
                        sb.Append("ttFP").Append(ttCredential.UserID);
                        FilterFlights(ref strResponse, Conversions.ToString(TripXMLMain.AppState.Get(sb.ToString())));
                        sb.Remove(0, sb.Length);
                    }
                }
                else
                {
                    strResponse = mstrResponse;
                }

                StartCounter = DateTime.Now;
                strResponse = DecodeLowFareMatrix(strResponse, ttCredential.UserID);
                sb.Append("Decoding = ").Append((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalMilliseconds));
                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.ToString(), "", UUID);

                if (strResponse.IndexOf("<SearchPromotionsResponse>") != -1)
                {
                    cAggregation.ProcessMarkup(XslPath, "", ref strResponse);
                }

                modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, "");
            }
            finally
            {
                _modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsLowFareMatrix", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;

        }

        #endregion

        #region  Web Methods 
        public wmLowFareMatrixOut.OTA_AirLowFareSearchMatrixRS wmLowFareMatrix(wmLowFareMatrixIn.OTA_AirLowFareSearchMatrixRQ OTA_AirLowFareSearchMatrixRQ)
        {

            string xmlMessage = "";
            wmLowFareMatrixOut.OTA_AirLowFareSearchMatrixRS oLowFareMatrixRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmLowFareMatrixIn.OTA_AirLowFareSearchMatrixRQ));

            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_AirLowFareSearchMatrixRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.LowFareMatrix);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmLowFareMatrixOut.OTA_AirLowFareSearchMatrixRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oLowFareMatrixRS = (wmLowFareMatrixOut.OTA_AirLowFareSearchMatrixRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsLowFareMatrix", "Error Deserialing OTA Response", ex.Message, string.Empty);
                xmlMessage = "<OTA_AirLowFareSearchMatrixRS Version=\"1.001\"><Errors><Error>" + ex.InnerException.ToString() + "</Error></Errors></OTA_AirLowFareSearchMatrixRS>";
                oReader = new System.IO.StringReader(xmlMessage);
                oLowFareMatrixRS = (wmLowFareMatrixOut.OTA_AirLowFareSearchMatrixRS)oSerializer.Deserialize(oReader);
            }

            return oLowFareMatrixRS;

        }
        public string wmLowFareMatrixXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.LowFareMatrix);
        }

        #endregion

    }

    #region Search Amadeus
    // Public Class SearchAmadeusMatrix
    // Private Delegate Sub StartSearch_Delegate()
    // Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearch)
    // Private pcc As String = ""
    // Private userid As String = ""
    // Private System As String = ""
    // Private ttAA As AmadeusAPIAdapter
    // Private _ServiceID As String = ""
    // Private _Request As String = ""
    // Private oAmadeus As cServiceAmadeus

    // Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttAA As AmadeusAPIAdapter, ByRef _oAmadeus As cServiceAmadeus)
    // Me.pcc = _pcc
    // Me.userid = _userid
    // Me.System = _System
    // Me.ttAA = _ttAA
    // Me.oAmadeus = _oAmadeus
    // End Sub
    // Public Property ServiceID() As String
    // Get
    // Return _ServiceID
    // End Get
    // Set(ByVal value As String)
    // _ServiceID = value
    // End Set
    // End Property
    // Public Property Request() As String
    // Get
    // Return _Request
    // End Get
    // Set(ByVal value As String)
    // _Request = value
    // End Set
    // End Property
    // Public Sub BeginSearch()
    // Dim cbr As AsyncCallback = New AsyncCallback(AddressOf EndSearch)
    // Dim arr As IAsyncResult = StartSearch_Wrapper.BeginInvoke(cbr, Nothing)
    // End Sub
    // Private Sub EndSearch(ByVal asy As IAsyncResult)
    // StartSearch_Wrapper.EndInvoke(asy)
    // asy.AsyncWaitHandle.Close()
    // End Sub
    // Private Sub DoAmadeusSearch()
    // ttAA.SourcePCC = Me.pcc
    // oAmadeus.SendAirRequest()
    // oAmadeus = Nothing
    // End Sub
    // End Class
    #endregion

    #region Search AmadeusWS
    public class SearchAmadeusMatrixWS
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private string _ServiceID = "";
        private string _Request = "";
        private cServiceAmadeusWS oAmadeusWS;

        public SearchAmadeusMatrixWS(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref cServiceAmadeusWS _oAmadeusWS)
        {
            StartSearch_Wrapper = new StartSearch_Delegate(DoAmadeusSearchWS);
            pcc = _pcc;
            userid = _userid;
            System = _System;
            ttProviderSystems = _ttProviderSystems;
            oAmadeusWS = _oAmadeusWS;
        }
        public string ServiceID
        {
            get
            {
                return _ServiceID;
            }
            set
            {
                _ServiceID = value;
            }
        }
        public string Request
        {
            get
            {
                return _Request;
            }
            set
            {
                _Request = value;
            }
        }
        public void BeginSearch()
        {
            var cbr = new AsyncCallback(EndSearch);
            var arr = StartSearch_Wrapper.BeginInvoke(cbr, null);
        }
        private void EndSearch(IAsyncResult asy)
        {
            StartSearch_Wrapper.EndInvoke(asy);
            asy.AsyncWaitHandle.Close();
        }
        private void DoAmadeusSearchWS()
        {

            ttProviderSystems.PCC = pcc;
            // Static count As Integer = 0
            // count = count + 1
            // If count <> 1 Then
            // ttProviderSystems.ProviderSession.MultipleAccess = False
            // Else
            // ttProviderSystems.ProviderSession.MultipleAccess = True
            // End If

            oAmadeusWS.SendAirRequest();
            oAmadeusWS = null;
        }
    }
    #endregion

    #region Search Galileo
    public class SearchGalileoMatrix
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private string _ServiceID = "";
        private string _Request = "";
        private cServiceGalileo oGalileo;
        public SearchGalileoMatrix(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref cServiceGalileo _oGalileo)
        {
            StartSearch_Wrapper = new StartSearch_Delegate(DoGalileoSearch);
            pcc = _pcc;
            userid = _userid;
            System = _System;
            ttProviderSystems = _ttProviderSystems;
            oGalileo = _oGalileo;
        }
        public string ServiceID
        {
            get
            {
                return _ServiceID;
            }
            set
            {
                _ServiceID = value;
            }
        }
        public string Request
        {
            get
            {
                return _Request;
            }
            set
            {
                _Request = value;
            }
        }
        public void BeginSearch()
        {
            var cbr = new AsyncCallback(EndSearch);
            var arr = StartSearch_Wrapper.BeginInvoke(cbr, null);
        }
        private void EndSearch(IAsyncResult asy)
        {
            StartSearch_Wrapper.EndInvoke(asy);
            asy.AsyncWaitHandle.Close();
        }
        private void DoGalileoSearch()
        {
            ttProviderSystems.PCC = pcc;
            oGalileo.SendAirRequest();
            oGalileo = null;
        }
    }
    #endregion

    #region Search Sabre
    public class SearchSabreMatrix
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private string _ServiceID = "";
        private string _Request = "";
        private cServiceSabre oSabre;

        public SearchSabreMatrix(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref cServiceSabre _oSabre)
        {
            StartSearch_Wrapper = new StartSearch_Delegate(DoAmadeusSearchWS);
            pcc = _pcc;
            userid = _userid;
            System = _System;
            ttProviderSystems = _ttProviderSystems;
            oSabre = _oSabre;
        }
        public string ServiceID
        {
            get
            {
                return _ServiceID;
            }
            set
            {
                _ServiceID = value;
            }
        }
        public string Request
        {
            get
            {
                return _Request;
            }
            set
            {
                _Request = value;
            }
        }
        public void BeginSearch()
        {
            var cbr = new AsyncCallback(EndSearch);
            var arr = StartSearch_Wrapper.BeginInvoke(cbr, null);
        }
        private void EndSearch(IAsyncResult asy)
        {
            StartSearch_Wrapper.EndInvoke(asy);
            asy.AsyncWaitHandle.Close();
        }
        private void DoAmadeusSearchWS()
        {
            ttProviderSystems.PCC = pcc;
            oSabre.SendAirRequest();
            oSabre = null;
        }
    }
    #endregion

    #region Search Worldspan
    public class SearchWorldspanMatrix
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private string _ServiceID = "";
        private string _Request = "";
        private cServiceWorldspan oWorldspan;

        public SearchWorldspanMatrix(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref cServiceWorldspan _oWorldspan)
        {
            StartSearch_Wrapper = new StartSearch_Delegate(DoAmadeusSearchWS);
            pcc = _pcc;
            userid = _userid;
            System = _System;
            ttProviderSystems = _ttProviderSystems;
            oWorldspan = _oWorldspan;
        }
        public string ServiceID
        {
            get
            {
                return _ServiceID;
            }
            set
            {
                _ServiceID = value;
            }
        }
        public string Request
        {
            get
            {
                return _Request;
            }
            set
            {
                _Request = value;
            }
        }
        public void BeginSearch()
        {
            var cbr = new AsyncCallback(EndSearch);
            var arr = StartSearch_Wrapper.BeginInvoke(cbr, null);
        }
        private void EndSearch(IAsyncResult asy)
        {
            StartSearch_Wrapper.EndInvoke(asy);
            asy.AsyncWaitHandle.Close();
        }
        private void DoAmadeusSearchWS()
        {
            ttProviderSystems.PCC = pcc;
            oWorldspan.SendAirRequest();
            oWorldspan = null;
        }
    }
    #endregion

    #region Search Portal
    // Public Class SearchPortalMatrix
    // Private Delegate Sub StartSearch_Delegate()
    // Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoPortalSearch)
    // Private pcc As String = ""
    // Private userid As String = ""
    // Private System As String = ""
    // Private ttProviderSystems As TripXMLProviderSystems
    // Private _ServiceID As String = ""
    // Private _Request As String = ""
    // Private oPortal As cServicePortal
    // Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oPortal As cServicePortal)
    // Me.pcc = _pcc
    // Me.userid = _userid
    // Me.System = _System
    // Me.ttProviderSystems = _ttProviderSystems
    // Me.oPortal = _oPortal
    // End Sub
    // Public Property ServiceID() As String
    // Get
    // Return _ServiceID
    // End Get
    // Set(ByVal value As String)
    // _ServiceID = value
    // End Set
    // End Property
    // Public Property Request() As String
    // Get
    // Return _Request
    // End Get
    // Set(ByVal value As String)
    // _Request = value
    // End Set
    // End Property
    // Public Sub BeginSearch()
    // Dim cbr As AsyncCallback = New AsyncCallback(AddressOf EndSearch)
    // Dim arr As IAsyncResult = StartSearch_Wrapper.BeginInvoke(cbr, Nothing)
    // End Sub
    // Private Sub EndSearch(ByVal asy As IAsyncResult)
    // StartSearch_Wrapper.EndInvoke(asy)
    // asy.AsyncWaitHandle.Close()
    // End Sub
    // Private Sub DoPortalSearch()
    // ttProviderSystems.PCC = Me.pcc
    // oPortal.SendAirRequest()
    // oPortal = Nothing
    // End Sub
    // End Class
    #endregion

    #region Search PortalXML
    // Public Class SearchPortalMatrixXML
    // Private Delegate Sub StartSearch_Delegate()
    // Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoPortalXMLSearch)
    // Private pcc As String = ""
    // Private userid As String = ""
    // Private System As String = ""
    // Private ttProviderSystems As TripXMLProviderSystems
    // Private _ServiceID As String = ""
    // Private _Request As String = ""
    // Private oPortalXML As cServicePortalXML
    // Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oPortalXML As cServicePortalXML)
    // Me.pcc = _pcc
    // Me.userid = _userid
    // Me.System = _System
    // Me.ttProviderSystems = _ttProviderSystems
    // Me.oPortalXML = _oPortalXML
    // End Sub
    // Public Property ServiceID() As String
    // Get
    // Return _ServiceID
    // End Get
    // Set(ByVal value As String)
    // _ServiceID = value
    // End Set
    // End Property
    // Public Property Request() As String
    // Get
    // Return _Request
    // End Get
    // Set(ByVal value As String)
    // _Request = value
    // End Set
    // End Property
    // Public Sub BeginSearch()
    // Dim cbr As AsyncCallback = New AsyncCallback(AddressOf EndSearch)
    // Dim arr As IAsyncResult = StartSearch_Wrapper.BeginInvoke(cbr, Nothing)
    // End Sub
    // Private Sub EndSearch(ByVal asy As IAsyncResult)
    // StartSearch_Wrapper.EndInvoke(asy)
    // asy.AsyncWaitHandle.Close()
    // End Sub
    // Private Sub DoPortalXMLSearch()
    // ttProviderSystems.PCC = Me.pcc
    // oPortalXML.SendAirRequest()
    // oPortalXML = Nothing
    // End Sub
    // End Class
    #endregion

}