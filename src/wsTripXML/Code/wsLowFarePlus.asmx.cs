using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;
using static TripXMLTools.TripXMLLoad;

namespace wsTripXML.wsTravelTalk
{

    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsLowFarePlus", Name = "wsLowFarePlus", Description = "A TripXML Web Service to Process Low Fare Plus Messages Request.")]
    public class wsLowFarePlus : WebService
    {

        private string mstrResponse = "";
        private int mintProviders = 0;

        public void GotResponse(string Response)
        {
            mstrResponse += Response;
            mintProviders += 1;
        }

        public TripXML tXML;


        #region  Web Services Designer Generated Code 

        public wsLowFarePlus() : base()
        {

            // This call is required by the Web Services Designer.
            InitializeComponent();

            // Add your own initialization code after the InitializeComponent() call

        }

        // Required by the Web Services Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Web Services Designer
        // It can be modified using the Web Services Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        protected override void Dispose(bool disposing)
        {
            // CODEGEN: This procedure is required by the Web Services Designer
            // Do not modify it using the code editor.
            if (disposing)
            {
                if (components is not null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region  Decode Functions 

        private string DecodeLowFarePlus(string strResponse, string UserID)
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
                        // Decode Airports   *
                        // *******************
                        oFlightNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue(DecodingType.Airport, oFlightNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value);
                        oFlightNode.SelectSingleNode("ArrivalAirport").InnerText = DecodeValue(DecodingType.Airport, oFlightNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value);
                        // *******************
                        // Decode Airlines   *
                        // *******************
                        if (oFlightNode.SelectSingleNode("OperatingAirline") is not null & oFlightNode.SelectSingleNode("OperatingAirline/@Code") is not null)
                        {
                            if (!string.IsNullOrEmpty(oFlightNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value) & string.IsNullOrEmpty(oFlightNode.SelectSingleNode("OperatingAirline").InnerText))
                            {
                                oFlightNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue(DecodingType.Airline, oFlightNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value);
                            }
                        }
                        if (oFlightNode.SelectSingleNode("MarketingAirline") is not null)
                        {
                            if (string.IsNullOrEmpty(oFlightNode.SelectSingleNode("MarketingAirline").InnerText))
                            {
                                oFlightNode.SelectSingleNode("MarketingAirline").InnerText = DecodeValue(DecodingType.Airline, oFlightNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value);
                            }
                        }
                        // *******************
                        // Decode Equipments *
                        // *******************
                        if (oFlightNode.SelectSingleNode("Equipment") is not null)
                        {
                            oFlightNode.SelectSingleNode("Equipment").InnerText = DecodeValue(DecodingType.Equipment, oFlightNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value);
                        }
                        // *******************
                        // Decode Stops *
                        // *******************
                        if (oFlightNode.SelectSingleNode("TPA_Extensions/StopInfo") is not null)
                        {
                            foreach (XmlNode stopNode in oFlightNode.SelectNodes("TPA_Extensions/StopInfo"))
                                stopNode.InnerText = DecodeValue(DecodingType.Airport, stopNode.Attributes["LocationCode"].Value);
                        }
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding LowFarePlus Response", ex.Message, string.Empty);
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

            var DoAmadeusWSSearches = new SearchAmadeusWS[100];
            var DoGalileoSearches = new SearchGalileo[100];
            var DoSabreSearches = new SearchSabre[100];
            var DoWorldspanSearches = new SearchWorldspan[100];

            string uTravelSum = "";
            string TravelSum = "";
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            string FaringPreference = "";
            string OriginDestination = "";
            var lfstrRequest = new string[100];
            int j = 0;

            try
            {
                StartTime = DateTime.Now;
                var argoApp = Application;
                modMain.PreServiceRequestPool(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(Application.Get($"XSD{ttCredential.UserID}Out"));
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
                                        // ttAA = Application.Get($"API{.UserID}{.System}{.Providers(i).PCC}")
                                        // 'If ttAA Is Nothing Then
                                        // Dim ekbpPCC As String = .Providers(i).PCC.Replace("*", "")
                                        // ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(i).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ekbpPCC).ToString())
                                        // sb.Remove(0, sb.Length())

                                        if (ttProviderSystems.AmadeusWS == false)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, $"Access denied to {withBlock.Providers[i].Name} - {ttCredential.System} system. Or invalid provider.", withBlock.Providers[i].Name));
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

                                            // If ttCredential.System = "Test" Then
                                            // ttProviderSystems.URL = "https://test.webservices.amadeus.com"
                                            // ElseIf ttCredential.System = "Training" Then
                                            // ttProviderSystems.URL = "https://production.webservices.amadeus.com"
                                            // Else
                                            // ttProviderSystems.URL = "https://production.webservices.amadeus.com"
                                            // End If

                                            var oAmadeusWS = new cServiceAmadeusWS();
                                            oAmadeusWS.GotResponse += GotResponse;

                                            {
                                                ref var withBlock1 = ref oAmadeusWS;
                                                withBlock1.ServiceID = (int)ttServiceID;
                                                withBlock1.Request = strRequest;
                                                withBlock1.ttProviderSystems = ttProviderSystems;
                                                withBlock1.Version = "";
                                            }

                                            DoAmadeusWSSearches[i] = new SearchAmadeusWS(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oAmadeusWS);
                                            DoAmadeusWSSearches[i].Request = strRequest;
                                            DoAmadeusWSSearches[i].ServiceID = ((int)ttServiceID).ToString();
                                            DoAmadeusWSSearches[i].BeginSearch();
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
                                        if (ttProviderSystems.System is null)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, $"Access denied to {withBlock.Providers[i].Name} - {ttCredential.System} system. Or invalid provider.", withBlock.Providers[i].Name));
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
                                            withBlock2.Request = strRequest;
                                            withBlock2.ProviderSystems = ttProviderSystems;
                                            withBlock2.Version = "";
                                        }

                                        DoGalileoSearches[i] = new SearchGalileo(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oGalileo);
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
                                        if (ttProviderSystems.System is null)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, $"Access denied to {withBlock.Providers[i].Name} - {ttCredential.System} system. Or invalid provider.", withBlock.Providers[i].Name));
                                            break;
                                        }
                                        ttProviderSystems.AAAPCC = withBlock.Providers[i].PCC;
                                        var oSabre = new cServiceSabre();
                                        oSabre.GotResponse += GotResponse;
                                        DataView ttCities;
                                        ttCities = (DataView)Application.Get("ttCities");

                                        {
                                            ref var withBlock3 = ref oSabre;
                                            withBlock3.ServiceID = ttServiceID;
                                            withBlock3.Request = strRequest;
                                            withBlock3.ProviderSystems = ttProviderSystems;
                                            withBlock3.Version = "";
                                            withBlock3.ttCities = ttCities;
                                        }

                                        DoSabreSearches[i] = new SearchSabre(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oSabre);
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
                                        if (ttProviderSystems.System is null)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, $"Access denied to {withBlock.Providers[i].Name} - {ttCredential.System} system. Or invalid provider.", withBlock.Providers[i].Name));
                                            break;
                                        }

                                        if (ttCredential.Providers[i].PCC.Trim().Length > 0)
                                        {
                                            ttProviderSystems.PCC = ttCredential.Providers[i].PCC;
                                        }

                                        var oWorldspan = new cServiceWorldspan();
                                        oWorldspan.GotResponse += GotResponse;

                                        DataView ttCities;
                                        ttCities = (DataView)Application.Get("ttCities");

                                        {
                                            ref var withBlock4 = ref oWorldspan;
                                            withBlock4.ServiceID = ttServiceID;
                                            withBlock4.Request = strRequest;
                                            withBlock4.ProviderSystems = ttProviderSystems;
                                            withBlock4.Version = "";
                                            withBlock4.ttCities = ttCities;
                                        }

                                        DoWorldspanSearches[i] = new SearchWorldspan(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oWorldspan);
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

                            case "travelport":
                                {
                                    try
                                    {
                                        if (ttProviderSystems.System is null)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, $"Access denied to {withBlock.Providers[i].Name} - {ttCredential.System} system. Or invalid provider.", withBlock.Providers[i].Name));
                                            break;
                                        }

                                        if (ttCredential.Providers[i].PCC.Trim().Length > 0)
                                        {
                                            ttProviderSystems.PCC = ttCredential.Providers[i].PCC;
                                        }

                                        var oTravelport = new cServiceTravelport();
                                        oTravelport.GotResponse += GotResponse;

                                        oTravelport.ServiceID = ttServiceID;
                                        oTravelport.Request = strRequest;
                                        oTravelport.ProviderSystems = ttProviderSystems;
                                        oTravelport.Version = "";
                                    }
                                    catch (Exception e)
                                    {
                                        GotResponse(FormatErrorMessage(ttServiceID, e.Message, withBlock.Providers[i].Name));
                                    }

                                    break;
                                }

                            default:
                                {
                                    throw new Exception($"Provider {ttCredential.Providers[0].Name} Not Currently Supported.");
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
                        FilterFlights(ref strResponse, Conversions.ToString(Application.Get($"ttFP{ttCredential.UserID}")));
                    }
                }
                else
                {
                    strResponse = mstrResponse;
                }

                if (!ttProviderSystems.LFPLight)
                {

                    StartCounter = DateTime.Now;
                    strResponse = DecodeLowFarePlus(strResponse, ttCredential.UserID);
                    CoreLib.SendTrace(ttCredential.UserID, "Performance", $"Decoding = {(int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalMilliseconds)}", "", ttProviderSystems.LogUUID);

                    if (!string.IsNullOrEmpty(ttProviderSystems.BLFile))
                    {
                        oDoc = new XmlDocument();
                        oDoc.Load(ttProviderSystems.BLFile);

                        oRoot = oDoc.DocumentElement;
                        oNode = oRoot.SelectSingleNode("Message[@Name='LowFare'][@Direction='Out']");

                        if (oNode is not null)
                        {
                            // check if flights from or to country to eliminate
                            var oBLNode = oNode.SelectSingleNode($"NoCountry[@Name='Amadeus'][@System='{ttProviderSystems.System}'][@PCC='{ttProviderSystems.PCC}']");
                            if (oBLNode is not null)
                            {
                                string strBusiness = oBLNode.OuterXml;
                                if (strResponse.IndexOf("<Success/>") != -1 || strResponse.IndexOf("<Success></Success>") != -1)
                                {
                                    strResponse = strResponse.Replace("<Success/>", $"{strBusiness}<Success/>");
                                    strResponse = strResponse.Replace("<Success></Success>", $"{strBusiness}<Success></Success>");
                                    strResponse = CoreLib.TransformXML(strResponse, $@"{XslPath}BL\", "BL_LowFareNoCountryRS.xsl", false);
                                }
                            }
                        }
                    }

                    if (strResponse.IndexOf("<SearchPromotionsResponse") != -1)
                    {
                        cAggregation.ProcessMarkup(XslPath, "", ref strResponse);
                    }
                }

                modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, "");
            }
            finally
            {
                modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                modMain.LogDeals(ref strRequest, ref strResponse);
                if (modCore.Trace)
                {
                    if (strResponse.Length > 2999)
                    {
                        CoreLib.SendTrace(ttCredential.UserID, "wsLowFarePlus", "============= OTA Response ============= ", strResponse.Substring(0, 3000), UUID);
                    }
                    else
                    {
                        CoreLib.SendTrace(ttCredential.UserID, "wsLowFarePlus", "============= OTA Response ============= ", strResponse, UUID);
                    }
                }
            }

            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [CompressionExtension.CompressionExtension()]
        [WebMethod(Description = "Process Low Fare Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS wmLowFarePlus(wmLowFarePlusIn.OTA_AirLowFareSearchPlusRQ OTA_AirLowFareSearchPlusRQ)
        {
            var oSerializer = new XmlSerializer(typeof(wmLowFarePlusIn.OTA_AirLowFareSearchPlusRQ));
            var oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_AirLowFareSearchPlusRQ);
            string xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            xmlMessage = ServiceRequest(xmlMessage, ttServices.LowFarePlus);
            wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS oLowFarePlusRS;

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS));
                var oReader = new System.IO.StringReader(xmlMessage);
                oLowFarePlusRS = (wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsLowFarePlus", "Error Deserialing OTA Response", ex.Message, string.Empty);
                xmlMessage = "<OTA_AirLowFareSearchPlusRS Version=\" 1.001\"><Errors><Error>" + ex.InnerException.ToString() + "</Error></Errors></OTA_AirLowFareSearchPlusRS>";
                var oReader = new System.IO.StringReader(xmlMessage);
                oLowFarePlusRS = (wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS)oSerializer.Deserialize(oReader);
            }

            return oLowFarePlusRS;

        }

        [WebMethod(Description = "Process Low Fare Plus Xml Messages Request.")]
        public string wmLowFarePlusXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.LowFarePlus);
        }

        #endregion

    }

    #region Search Amadeus
    // Public Class SearchAmadeus
    // Private Delegate Sub StartSearch_Delegate()
    // Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearch)
    // Private pcc As String = ""
    // Private userid As String = ""
    // Private System As String = ""
    // Private ttAA As AmadeusAPIAdapter
    // Private oAmadeus As cServiceAmadeus

    // Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttAA As AmadeusAPIAdapter, ByRef _oAmadeus As cServiceAmadeus)
    // Me.pcc = _pcc
    // Me.userid = _userid
    // Me.System = _System
    // Me.ttAA = _ttAA
    // Me.oAmadeus = _oAmadeus
    // End Sub
    // Public Property ServiceID() As String = ""
    // Public Property Request() As String = ""
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
    public class SearchAmadeusWS
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private readonly string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private cServiceAmadeusWS oAmadeusWS;

        public SearchAmadeusWS(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref cServiceAmadeusWS _oAmadeusWS)
        {
            StartSearch_Wrapper = new StartSearch_Delegate(DoAmadeusSearchWS);
            pcc = _pcc;
            userid = _userid;
            System = _System;
            ttProviderSystems = _ttProviderSystems;
            oAmadeusWS = _oAmadeusWS;
        }
        public string ServiceID { get; set; } = "";
        public string Request { get; set; } = "";
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
    public class SearchGalileo
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private cServiceGalileo oGalileo;

        public SearchGalileo(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref cServiceGalileo _oGalileo)
        {
            StartSearch_Wrapper = new StartSearch_Delegate(DoGalileoSearch);
            pcc = _pcc;
            userid = _userid;
            System = _System;
            ttProviderSystems = _ttProviderSystems;
            oGalileo = _oGalileo;
        }

        public string ServiceID { get; set; } = "";
        public string Request { get; set; } = "";

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
    public class SearchSabre
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private cServiceSabre oSabre;

        public SearchSabre(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref cServiceSabre _oSabre)
        {
            StartSearch_Wrapper = new StartSearch_Delegate(DoSabreSearchWS);
            pcc = _pcc;
            userid = _userid;
            System = _System;
            ttProviderSystems = _ttProviderSystems;
            oSabre = _oSabre;
        }
        public string ServiceID { get; set; } = "";
        public string Request { get; set; } = "";
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
        private void DoSabreSearchWS()
        {
            ttProviderSystems.PCC = pcc;
            oSabre.SendAirRequest();
            oSabre = null;
        }
    }
    #endregion

    #region Search Worldspan
    public class SearchWorldspan
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private cServiceWorldspan oWorldspan;

        public SearchWorldspan(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref cServiceWorldspan _oWorldspan)
        {
            StartSearch_Wrapper = new StartSearch_Delegate(DoWorldspanSearch);
            pcc = _pcc;
            userid = _userid;
            System = _System;
            ttProviderSystems = _ttProviderSystems;
            oWorldspan = _oWorldspan;
        }
        public string ServiceID { get; set; } = "";
        public string Request { get; set; } = "";
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
        private void DoWorldspanSearch()
        {
            ttProviderSystems.PCC = pcc;
            oWorldspan.SendAirRequest();
            oWorldspan = null;
        }
    }
    #endregion

    #region Search Portal
    // Public Class SearchPortal
    // Private Delegate Sub StartSearch_Delegate()
    // Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoPortalSearch)
    // Private pcc As String = ""
    // Private userid As String = ""
    // Private System As String = ""
    // Private ttProviderSystems As TripXMLProviderSystems
    // Private _ServiceID As String = ""
    // Private _Request As String = ""
    // 'Private oPortal As cServicePortal
    // Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oPortal As cServicePortal)
    // Me.pcc = _pcc
    // Me.userid = _userid
    // Me.System = _System
    // Me.ttProviderSystems = _ttProviderSystems
    // 'Me.oPortal = _oPortal
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
    // Public Class SearchPortalXML
    // Private Delegate Sub StartSearch_Delegate()
    // Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoPortalXMLSearch)
    // Private pcc As String = ""
    // Private userid As String = ""
    // Private System As String = ""
    // Private ttProviderSystems As TripXMLProviderSystems
    // Private _ServiceID As String = ""
    // Private _Request As String = ""
    // 'Private oPortalXML As cServicePortalXML
    // Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oPortalXML As cServicePortalXML)
    // Me.pcc = _pcc
    // Me.userid = _userid
    // Me.System = _System
    // Me.ttProviderSystems = _ttProviderSystems
    // 'Me.oPortalXML = _oPortalXML
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

    #region Search TravelFusion
    // Public Class SearchTravelFusion
    // Private Delegate Sub StartSearch_Delegate()
    // Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoTravelFusionSearch)
    // Private pcc As String = ""
    // Private userid As String = ""
    // Private System As String = ""
    // Private ttProviderSystems As TripXMLProviderSystems
    // Private _ServiceID As String = ""
    // Private _Request As String = ""
    // Private oTravelFusion As cServiceTravelFusion

    // Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oTravelFusion As cServiceTravelFusion)
    // Me.pcc = _pcc
    // Me.userid = _userid
    // Me.System = _System
    // Me.ttProviderSystems = _ttProviderSystems
    // Me.oTravelFusion = _oTravelFusion
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
    // Private Sub DoTravelFusionSearch()
    // ttProviderSystems.PCC = Me.pcc
    // oTravelFusion.SendAirRequest()
    // oTravelFusion = Nothing
    // End Sub
    // End Class
    #endregion

}