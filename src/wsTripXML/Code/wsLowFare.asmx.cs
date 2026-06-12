using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using TripXMLTools;

namespace wsTripXML.wsTravelTalk
{
    public partial class wsLowFare
    {
        private StringBuilder sb = new StringBuilder();

        private string mstrResponse = "";
        private int mintProviders = 0;

        private void GotResponse(string Response)
        {
            mstrResponse += Response;
            mintProviders += 1;
        }

        public TripXML tXML;

        private readonly modMain _modMain;

        public wsLowFare(modMain modMain)
        {
            _modMain = modMain;
        }

        #region  Decode Functions 

        private string DecodeLowFare(string strResponse, string UserID)
        {
            // Dim ttAirports As DataView
            // Dim ttAirlines As DataView
            // Dim ttHiddenAirlines As DataView
            // Dim ttEquipments As DataView

            try
            {

                var oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);

                // ttAirports = CType(TripXMLMain.AppState.Get("ttAirports"), DataView)
                // ttAirlines = CType(TripXMLMain.AppState.Get("ttAirlines"), DataView)
                // ttHiddenAirlines = CType(TripXMLMain.AppState.Get("ttHiddenAirlines"), DataView)
                // ttEquipments = CType(TripXMLMain.AppState.Get("ttEquipments"), DataView)

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
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding LowFare Response", ex.Message, string.Empty);
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
            var DoAmadeusWSSearches = new SearchAmadeusWS[100];
            // Dim DoAmadeusSearches(99) As SearchAmadeus
            bool ValidateXSDOut;
            var StartTime = default(DateTime);
            string UUID = "";
            int i;
            DateTime StartCounter;

            try
            {
                StartTime = DateTime.Now;
                _modMain.PreServiceRequestPool(ref strRequest, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Environment.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(TripXMLMain.AppState.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
                sb.Remove(0, sb.Length);

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
                                        string ekbpPCC = withBlock.Providers[i].PCC.Replace("*", "");
                                        ttProviderSystems = (TripXMLProviderSystems)TripXMLMain.AppState.Get(sb.Append("PS").Append(ttCredential.Providers[i].Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ekbpPCC).ToString());
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
                                            var oThreadAmadeusWS = new Thread(new ThreadStart(oAmadeusWS.SendAirRequest));
                                            oAmadeusWS.GotResponse += GotResponse;

                                            oAmadeusWS.ServiceID = (int)ttServiceID;
                                            oAmadeusWS.Request = strRequest;
                                            oAmadeusWS.ttProviderSystems = ttProviderSystems;
                                            oAmadeusWS.Version = "";
                                            // 19-4-2013
                                            oThreadAmadeusWS.Start();
                                            // ttProviderSystems = Nothing

                                            // Else
                                            // ttProviderSystems = ttAA.ttProviderSystems
                                            // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                            // ttAA.SourcePCC = ttCredential.Providers(i).PCC
                                            // Else
                                            // ttAA.SourcePCC = ttAA.ttProviderSystems.PCC
                                            // End If
                                            // Dim oAmadeus As New cServiceAmadeus
                                            // AddHandler oAmadeus.GotResponse, AddressOf GotResponse
                                            // Dim oThreadAmadeus As New Thread(New ThreadStart(AddressOf oAmadeus.SendAirRequest))
                                            // With oAmadeus
                                            // .ServiceID = ttServiceID
                                            // .Request = strRequest
                                            // .ttAA = ttAA
                                            // .Version = ""
                                            // End With
                                            // oThreadAmadeus.Start()
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
                                        ttProviderSystems = (TripXMLProviderSystems)TripXMLMain.AppState.Get(sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC).ToString());
                                        sb.Remove(0, sb.Length);
                                        if (ttProviderSystems.System is null)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(withBlock.System).Append(" system. Or invalid provider.").ToString(), withBlock.Providers[i].Name));
                                            sb.Remove(0, sb.Length);
                                            break;
                                        }

                                        if (ttCredential.Providers[i].PCC.Trim().Length > 0)
                                        {
                                            ttProviderSystems.PCC = ttCredential.Providers[i].PCC;
                                        }

                                        var oGalileo = new cServiceGalileo();
                                        var oThreadGalileo = new Thread(new ThreadStart(oGalileo.SendAirRequest));
                                        oGalileo.GotResponse += GotResponse;

                                        oGalileo.ServiceID = (int)ttServiceID;
                                        oGalileo.Request = strRequest;
                                        oGalileo.ProviderSystems = ttProviderSystems;
                                        oGalileo.Version = "";

                                        oThreadGalileo.Start();
                                    }
                                    catch (Exception e)
                                    {
                                        GotResponse(FormatErrorMessage(ttServiceID, e.Message, withBlock.Providers[i].Name));
                                    }

                                    break;
                                }
                            case "sabre":
                                {
                                    try
                                    {
                                        ttProviderSystems = (TripXMLProviderSystems)TripXMLMain.AppState.Get(sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC).ToString());
                                        sb.Remove(0, sb.Length);
                                        if (ttProviderSystems.System is null)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(withBlock.System).Append(" system. Or invalid provider.").ToString(), withBlock.Providers[i].Name));
                                            sb.Remove(0, sb.Length);
                                            break;
                                        }

                                        ttProviderSystems.AAAPCC = withBlock.Providers[i].PCC;

                                        // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                        // ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                        // End If

                                        var oSabre = new cServiceSabre();
                                        var oThreadSabre = new Thread(new ThreadStart(oSabre.SendAirRequest));
                                        DataView ttCities;
                                        oSabre.GotResponse += GotResponse;

                                        ttCities = (DataView)TripXMLMain.AppState.Get("ttCities");

                                        oSabre.ServiceID = ttServiceID;
                                        oSabre.Request = strRequest;
                                        oSabre.ProviderSystems = ttProviderSystems;
                                        oSabre.Version = "";
                                        oSabre.ttCities = ttCities;

                                        oThreadSabre.Start();
                                    }

                                    catch (Exception e)
                                    {
                                        GotResponse(FormatErrorMessage(ttServiceID, e.Message, withBlock.Providers[i].Name));
                                    }

                                    break;
                                }
                            case "worldspan":
                                {
                                    try
                                    {
                                        ttProviderSystems = (TripXMLProviderSystems)TripXMLMain.AppState.Get(sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC).ToString());
                                        sb.Remove(0, sb.Length);
                                        if (ttProviderSystems.System is null)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(withBlock.System).Append(" system. Or invalid provider.").ToString(), withBlock.Providers[i].Name));
                                            sb.Remove(0, sb.Length);
                                            break;
                                        }

                                        if (ttCredential.Providers[i].PCC.Trim().Length > 0)
                                        {
                                            ttProviderSystems.PCC = ttCredential.Providers[i].PCC;
                                        }

                                        var oWorldspan = new cServiceWorldspan();
                                        var oThreadWorldspan = new Thread(new ThreadStart(oWorldspan.SendAirRequest));
                                        DataView ttCities;
                                        oWorldspan.GotResponse += GotResponse;

                                        ttCities = (DataView)TripXMLMain.AppState.Get("ttCities");

                                        oWorldspan.ServiceID = ttServiceID;
                                        oWorldspan.Request = strRequest;
                                        oWorldspan.ProviderSystems = ttProviderSystems;
                                        oWorldspan.Version = "";
                                        oWorldspan.ttCities = ttCities;

                                        oThreadWorldspan.Start();
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
                            // ttProviderSystems = TripXMLMain.AppState.Get(sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC).ToString())
                            // sb.Remove(0, sb.Length())
                            // If ttProviderSystems.System Is Nothing Then
                            // GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(.System).Append(" system. Or invalid provider.").ToString(), .Providers(i).Name))
                            // sb.Remove(0, sb.Length())
                            // Exit Select
                            // End If

                            // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                            // ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                            // End If

                            // Dim oPortal As New cServicePortal
                            // Dim oThreadPortal As New Thread(New ThreadStart(AddressOf oPortal.SendAirRequest))
                            // AddHandler oPortal.GotResponse, AddressOf GotResponse

                            // With oPortal
                            // .ServiceID = ttServiceID
                            // .Request = strRequest
                            // .ProviderSystems = ttProviderSystems
                            // .Version = ""
                            // End With

                            // oThreadPortal.Start()

                            // oPortal = Nothing
                            // Catch e As Exception
                            // GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                            // End Try

                            case "portalxml":
                                {
                                    break;
                                }
                            // Try
                            // ttProviderSystems = TripXMLMain.AppState.Get(sb.Append("PS").Append(ttCredential.Providers(i).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(i).PCC).ToString())
                            // sb.Remove(0, sb.Length())
                            // If ttProviderSystems.System Is Nothing Then
                            // GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(.System).Append(" system. Or invalid provider.").ToString(), .Providers(i).Name))
                            // sb.Remove(0, sb.Length())
                            // Exit Select
                            // End If

                            // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                            // ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                            // End If

                            // Dim oPortalXML As New cServicePortalXML
                            // Dim oThreadPortal As New Thread(New ThreadStart(AddressOf oPortalXML.SendAirRequest))
                            // AddHandler oPortalXML.GotResponse, AddressOf GotResponse

                            // With oPortalXML
                            // .ServiceID = ttServiceID
                            // .Request = strRequest
                            // .ProviderSystems = ttProviderSystems
                            // .Version = ""
                            // End With

                            // oThreadPortal.Start()

                            // oPortalXML = Nothing
                            // Catch e As Exception
                            // GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                            // End Try
                            // Try
                            // ttProviderSystems = TripXMLMain.AppState.Get(sb.Append("PS").Append(ttCredential.Providers(i).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(i).PCC).ToString())

                            // sb.Remove(0, sb.Length())
                            // If ttProviderSystems.System Is Nothing Then
                            // GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(.System).Append(" system. Or invalid provider.").ToString(), .Providers(i).Name))
                            // sb.Remove(0, sb.Length())
                            // Exit Select
                            // End If


                            // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                            // ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                            // End If

                            // Dim oTravelFusion As New cServiceTravelFusion
                            // Dim oThreadTravelFusion As New Thread(New ThreadStart(AddressOf oTravelFusion.SendAirRequest))
                            // Dim ttCities As DataView
                            // AddHandler oTravelFusion.GotResponse, AddressOf GotResponse

                            // ttCities = CType(TripXMLMain.AppState.Get("ttCities"), DataView)

                            // With oTravelFusion
                            // .ServiceID = ttServiceID
                            // .Request = strRequest
                            // .ttProviderSystems = ttProviderSystems
                            // .Version = ""

                            // End With

                            // oThreadTravelFusion.Start()

                            // Catch e As Exception
                            // GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                            // End Try
                            case "travelfusion":
                                {
                                    break;
                                }

                            default:
                                {
                                    GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Provider ").Append(withBlock.Providers[i].Name).Append(" Not Currently Supported.").ToString(), withBlock.Providers[i].Name));
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
                    Thread.Sleep(200);
                }

                if (ttCredential.Providers.Length > 1)
                {
                    strResponse = string.Concat("<SuperRS>", mstrResponse, "</SuperRS>");
                    // Aggregate
                    cAggregation.Aggregate(ttServiceID, XslPath, "", ref strResponse);

                    // Filter Flights
                    if (ttProviderSystems.AggFilter == true)
                    {
                        FilterFlights(ref strResponse, Conversions.ToString(TripXMLMain.AppState.Get(sb.Append("ttFP").Append(ttCredential.UserID).ToString())));
                        sb.Remove(0, sb.Length);
                    }
                }

                else
                {
                    strResponse = mstrResponse;
                }

                StartCounter = DateTime.Now;
                strResponse = DecodeLowFare(strResponse, ttCredential.UserID);
                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.Append("Decoding = ").Append((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalMilliseconds)).ToString(), "", ttProviderSystems.LogUUID);
                sb.Remove(0, sb.Length);

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
                _modMain.LogDeals(ref strRequest, ref strResponse);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsLowFare", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;
            sb = null;
        }

        #endregion

        #region  Web Methods 
        public wmLowFareOut.OTA_AirLowFareSearchRS wmLowFare(wmLowFareIn.OTA_AirLowFareSearchRQ OTA_AirLowFareSearchRQ)
        {
            string xmlMessage = "";
            wmLowFareOut.OTA_AirLowFareSearchRS oLowFareRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmLowFareIn.OTA_AirLowFareSearchRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_AirLowFareSearchRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.LowFare);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmLowFareOut.OTA_AirLowFareSearchRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oLowFareRS = (wmLowFareOut.OTA_AirLowFareSearchRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsLowFare", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oLowFareRS;

        }
        public string wmLowFareXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.LowFare);
        }

        #endregion

    }

    // duplicate TripXML header class removed — the canonical one lives in modMain.cs

}