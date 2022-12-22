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

namespace wsTripXML.wsTravelTalk
{

    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsLowFare", Name = "wsLowFare_v03", Description = "A TripXML Web Service to Process Low Fare Plus Messages Request.")]
    public class wsLowFare_v03 : WebService
    {

        private string mstrResponse = "";
        private int mintProviders = 0;

        public void GotResponse(string Response)
        {
            mstrResponse += Response;
            mintProviders += 1;
        }

        public wsTravelTalk.TripXML tXML;

        #region  Web Services Designer Generated Code 

        public wsLowFare_v03() : base()
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

        private string DecodeLowFare(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            DataView ttAirports;
            DataView ttAirlines;
            // Dim ttHiddenAirlines As DataView
            DataView ttEquipments;
            XmlNode oFareNode = null;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                ttAirports = (DataView)Application.Get("ttAirports");
                ttAirlines = (DataView)Application.Get("ttAirlines");
                // ttHiddenAirlines = CType(Application.Get("ttHiddenAirlines"), DataView)
                ttEquipments = (DataView)Application.Get("ttEquipments");

                foreach (XmlNode oNode in oRoot.SelectNodes("PricedItineraries/PricedItinerary"))
                {
                    foreach (XmlNode oFlightNode in oNode.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"))
                    {
                        // *******************
                        // *******************
                        // Decode Airports   *
                        // *******************
                        string argstrCode = oFlightNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value;
                        oFlightNode.SelectSingleNode("DepartureAirport").InnerText = wsTravelTalk.modMain.GetDecodeValue(ref ttAirports, ref argstrCode);
                        oFlightNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value = argstrCode;
                        string argstrCode1 = oFlightNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value;
                        oFlightNode.SelectSingleNode("ArrivalAirport").InnerText = wsTravelTalk.modMain.GetDecodeValue(ref ttAirports, ref argstrCode1);
                        oFlightNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value = argstrCode1;

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
                            if (!string.IsNullOrEmpty(oFlightNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value))
                            {
                                string argstrCode2 = oFlightNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value;
                                oFlightNode.SelectSingleNode("OperatingAirline").InnerText = wsTravelTalk.modMain.GetDecodeValue(ref ttAirlines, ref argstrCode2);
                                oFlightNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value = argstrCode2;
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
                            string argstrCode3 = oFlightNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value;
                            oFlightNode.SelectSingleNode("MarketingAirline").InnerText = wsTravelTalk.modMain.GetDecodeValue(ref ttAirlines, ref argstrCode3);
                            oFlightNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value = argstrCode3;
                            // End If
                        }

                        // *******************
                        // Decode Equipments *
                        // *******************
                        if (oFlightNode.SelectSingleNode("Equipment") is not null)
                        {
                            string argstrCode4 = oFlightNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value;
                            oFlightNode.SelectSingleNode("Equipment").InnerText = wsTravelTalk.modMain.GetDecodeValue(ref ttEquipments, ref argstrCode4);
                            oFlightNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value = argstrCode4;
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
                    Fare = Fare.Insert((int)Math.Round(Fare.Length - Conversions.ToDouble(oNodeOnd.Attributes["DecimalPlaces"].Value)), ".");
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
                                    Fare = Fare.Insert((int)Math.Round(Fare.Length - Conversions.ToDouble(oNodeOnd.Attributes["DecimalPlaces"].Value)), ".");
                                    bool exitFor = false;
                                    bool exitFor1 = false;
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
                                                exitFor1 = true;
                                                break;
                                            }
                                    }

                                    if (exitFor)
                                    {
                                        break;
                                    }

                                    if (exitFor1)
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

            catch (Exception ex)
            {
            }
            // Error Filtering Flights
            finally
            {
                oDoc = null;
            }

        }

        #endregion

        #region  Process Service Request All GDS 

        private string ServiceRequest(string strRequest, int ttServiceID)
        {
            string strResponse = "";
            TravelTalkCredential ttCredential = default;
            TripXMLProviderSystems ttProviderSystems = default;
            bool ValidateXSDOut;
            var StartTime = default(DateTime);
            string UUID = "";
            int i;
            DateTime StartCounter;
            // Dim DoAmadeusSearches(99) As SearchAmadeus_v03
            var DoAmadeusWSSearches = new wsTravelTalk.SearchAmadeusWS_v03[100];
            var DoGalileoSearches = new wsTravelTalk.SearchGalileo_v03[100];
            var DoSabreSearches = new wsTravelTalk.SearchSabre_v03[100];
            var DoWorldspanSearches = new wsTravelTalk.SearchWorldspan_v03[100];
            // Dim DoPortalSearches(99) As SearchPortal_v03
            // Dim DoPortalXMLSearches(99) As SearchPortalXML_v03
            StringBuilder sb = null;
            StringBuilder sb1 = null;
            string uTravelSum = "";
            string TravelSum = "";
            string tempTravelSum = "";
            // Static MultipleCount As Integer = 0
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            // Dim FaringPreference As String = ""
            // Dim OriginDestination As String = ""
            var lfstrRequest = new string[100];
            int j = 0;
            string repTravelSumString = "";
            int intChars = 0;


            sb = new StringBuilder();
            sb1 = new StringBuilder();

            try
            {
                StartTime = DateTime.Now;


                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;

                sb.Append("<OTA_AirLowFareSearchRQ>").Append(oRoot.SelectSingleNode("POS").OuterXml);


                foreach (XmlNode currentONode in oRoot.SelectNodes("OriginDestinationInformation"))
                {
                    oNode = currentONode;
                    sb.Append(oNode.OuterXml);
                }

                // sb.Append(oRoot.SelectSingleNode("TravelerInfoSummary").OuterXml)
                // sb.Append("<FaringPreferences>").ToString()

                TravelSum = sb.ToString();
                uTravelSum = oRoot.SelectSingleNode("TravelerInfoSummary").InnerXml.ToString();

                // For Each oNode In oRoot.SelectNodes("FaringPreferences/FaringPreference")
                // 'For Each oNode In oRoot.SelectNodes("FaringPreference")
                // 'FaringPreference = oNode.InnerXml
                // sb.Append(oNode.OuterXml)
                // lfstrRequest(j) = sb.Append(oNode.OuterXml).Append("</FaringPreferences></OTA_AirLowFareSearchRQ>").ToString
                // j += 1
                // Next

                sb.Remove(0, sb.Length);

                var argoApp = Application;
                wsTravelTalk.modMain.PreServiceRequestPool(ref strRequest, ref argoApp, ref ttCredential, StartTime, ttServiceID, Server.MachineName, ref UUID);
                sb.Append("XSD").Append(ttCredential.UserID).Append("Out");
                ValidateXSDOut = Conversions.ToBoolean(Application.Get(sb.ToString()));
                sb.Remove(0, sb.Length);


                {
                    ref var withBlock = ref ttCredential;
                    var loopTo = withBlock.Providers.Length - 1;
                    for (i = 0; i <= loopTo; i++)
                    {
                        foreach (XmlNode currentONode1 in oRoot.SelectNodes("FaringPreferences/FaringPreference"))
                        {
                            oNode = currentONode1;
                            if ((ttCredential.Providers[i].PCC ?? "") == (oNode.Attributes["PseudoCityCode"].Value ?? ""))
                            {
                                sb.Remove(0, sb.Length);

                                // sb.Append(TravelSum).Append(oNode.SelectSingleNode("TravelPreferences").OuterXml)
                                // sb.Append("<TravelerInfoSummary>").Append(uTravelSum).Append(oNode.SelectSingleNode("AirTravelerAvail").OuterXml)
                                // sb.Append(oNode.SelectSingleNode("PriceRequestInformation").OuterXml).Append("</TravelerInfoSummary></OTA_AirLowFareSearchRQ>")

                                tempTravelSum = TravelSum;

                                if (oNode.Attributes["MaxResponses"] is not null)
                                {
                                    tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchRQ>", "<OTA_AirLowFareSearchRQ MaxResponses=\"" + oNode.Attributes["MaxResponses"].Value + "\">");
                                }

                                sb.Append(tempTravelSum).Append(oNode.InnerXml);
                                intChars = sb.ToString().IndexOf("<TravelPreferences>");

                                // If TravelPreferences tag is not there
                                if (intChars == -1)
                                {
                                    repTravelSumString = sb1.Append("<TravelerInfoSummary>").Append(uTravelSum).Append("<AirTravelerAvail>").ToString();
                                    sb1.Remove(0, sb1.Length);
                                    sb.Replace("<AirTravelerAvail>", repTravelSumString).Append("</TravelerInfoSummary></OTA_AirLowFareSearchRQ>");
                                }
                                else
                                {
                                    repTravelSumString = sb1.Append("</TravelPreferences><TravelerInfoSummary>").Append(uTravelSum).ToString();
                                    sb1.Remove(0, sb1.Length);
                                    sb.Replace("</TravelPreferences>", repTravelSumString).Append("</TravelerInfoSummary></OTA_AirLowFareSearchRQ>");

                                }

                                lfstrRequest[j] = sb.ToString();

                                sb.Remove(0, sb.Length);

                                switch (withBlock.Providers[i].Name.ToLower() ?? "")
                                {
                                    case "amadeus":
                                        {
                                            try
                                            {
                                                // Dim ttAA As AmadeusAPIAdapter

                                                sb.Append("API").Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC);
                                                // ttAA = Application.Get(sb.ToString())
                                                sb.Remove(0, sb.Length);

                                                // If ttAA Is Nothing Then
                                                ttProviderSystems = (TripXMLProviderSystems)Application.Get(sb.Append("PS").Append(ttCredential.Providers[i].Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers[i].PCC).ToString());
                                                sb.Remove(0, sb.Length);

                                                if (ttProviderSystems.AmadeusWS == false)
                                                {
                                                    sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.");
                                                    GotResponse(FormatErrorMessage((ttServices)ttServiceID, sb.ToString(), withBlock.Providers[i].Name));
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

                                                    // ttCredential.Providers(0).Name = "AmadeusWS"

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

                                                    var oAmadeusWS = new wsTravelTalk.cServiceAmadeusWS();
                                                    oAmadeusWS.GotResponse += this.GotResponse;

                                                    {
                                                        ref var withBlock1 = ref oAmadeusWS;
                                                        withBlock1.ServiceID = ttServiceID;
                                                        withBlock1.Request = lfstrRequest[j];
                                                        withBlock1.ttProviderSystems = ttProviderSystems;
                                                        // .Version = "v03"
                                                    }

                                                    DoAmadeusWSSearches[j] = new wsTravelTalk.SearchAmadeusWS_v03(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oAmadeusWS);
                                                    DoAmadeusWSSearches[j].Request = lfstrRequest[j];
                                                    DoAmadeusWSSearches[j].ServiceID = ttServiceID.ToString();
                                                    DoAmadeusWSSearches[j].BeginSearch();

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
                                                    // .Request = lfstrRequest(j)
                                                    // .ttAA = ttAA
                                                    // '.Version = "v03"
                                                    // End With

                                                    // DoAmadeusSearches(j) = New SearchAmadeus_v03(.Providers(i).PCC, .UserID, .System, ttAA, oAmadeus)
                                                    // DoAmadeusSearches(j).Request = lfstrRequest(j)
                                                    // DoAmadeusSearches(j).ServiceID = ttServiceID
                                                    // DoAmadeusSearches(j).BeginSearch()

                                                    // sb.Append("API").Append(.UserID).Append(.System)
                                                    // Application.Set(sb.ToString(), ttAA)
                                                    // sb.Remove(0, sb.Length)
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                GotResponse(FormatErrorMessage((ttServices)ttServiceID, e.Message, withBlock.Providers[i].Name));

                                            }

                                            break;
                                        }

                                    case "apollo":
                                    case "galileo":
                                        {
                                            try
                                            {
                                                sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC);
                                                ttProviderSystems = (TripXMLProviderSystems)Application.Get(sb.ToString());
                                                sb.Remove(0, sb.Length);

                                                if (ttProviderSystems.System is null)
                                                {
                                                    sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.");
                                                    GotResponse(FormatErrorMessage((ttServices)ttServiceID, sb.ToString(), withBlock.Providers[i].Name));
                                                    sb.Remove(0, sb.Length);
                                                    break;
                                                }

                                                if (ttCredential.Providers[i].PCC.Trim().Length > 0)
                                                {
                                                    ttProviderSystems.PCC = ttCredential.Providers[i].PCC;
                                                }

                                                var oGalileo = new wsTravelTalk.cServiceGalileo();
                                                oGalileo.GotResponse += this.GotResponse;

                                                {
                                                    ref var withBlock2 = ref oGalileo;
                                                    withBlock2.ServiceID = ttServiceID;
                                                    withBlock2.Request = lfstrRequest[j];
                                                    withBlock2.ProviderSystems = ttProviderSystems;
                                                    // .Version = "v03"
                                                }

                                                DoGalileoSearches[j] = new wsTravelTalk.SearchGalileo_v03(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oGalileo);
                                                DoGalileoSearches[j].Request = lfstrRequest[j];
                                                DoGalileoSearches[j].ServiceID = ttServiceID.ToString();
                                                DoGalileoSearches[j].BeginSearch();
                                            }

                                            catch (Exception e)
                                            {
                                                GotResponse(FormatErrorMessage((ttServices)ttServiceID, e.Message, withBlock.Providers[i].Name));
                                            }

                                            break;
                                        }

                                    case "sabre":
                                    case "Sabre":
                                        {
                                            try
                                            {
                                                sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC);
                                                ttProviderSystems = (TripXMLProviderSystems)Application.Get(sb.ToString());
                                                sb.Remove(0, sb.Length);

                                                if (ttProviderSystems.System is null)
                                                {
                                                    sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.");
                                                    GotResponse(FormatErrorMessage((ttServices)ttServiceID, sb.ToString(), withBlock.Providers[i].Name));
                                                    sb.Remove(0, sb.Length);
                                                    break;
                                                }

                                                ttProviderSystems.AAAPCC = withBlock.Providers[i].PCC;

                                                // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                                // ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                                // End If

                                                var oSabre = new wsTravelTalk.cServiceSabre();
                                                oSabre.GotResponse += this.GotResponse;

                                                DataView ttCities;
                                                ttCities = (DataView)Application.Get("ttCities");

                                                {
                                                    ref var withBlock3 = ref oSabre;
                                                    withBlock3.ServiceID = ttServiceID;
                                                    withBlock3.Request = lfstrRequest[j];
                                                    withBlock3.ProviderSystems = ttProviderSystems;
                                                    // .Version = "v03"
                                                    withBlock3.ttCities = ttCities;
                                                }

                                                DoSabreSearches[j] = new wsTravelTalk.SearchSabre_v03(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oSabre);
                                                DoSabreSearches[j].Request = lfstrRequest[j];
                                                DoSabreSearches[j].ServiceID = ttServiceID.ToString();
                                                DoSabreSearches[j].BeginSearch();
                                            }

                                            catch (Exception e)
                                            {
                                                GotResponse(FormatErrorMessage((ttServices)ttServiceID, e.Message, withBlock.Providers[i].Name));
                                            }

                                            break;
                                        }

                                    case "worldspan":
                                    case "Worldspan":
                                        {
                                            try
                                            {
                                                sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC);
                                                ttProviderSystems = (TripXMLProviderSystems)Application.Get(sb.ToString());
                                                sb.Remove(0, sb.Length);

                                                if (ttProviderSystems.System is null)
                                                {
                                                    sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.");
                                                    GotResponse(FormatErrorMessage((ttServices)ttServiceID, sb.ToString(), withBlock.Providers[i].Name));
                                                    sb.Remove(0, sb.Length);
                                                    break;
                                                }

                                                if (ttCredential.Providers[i].PCC.Trim().Length > 0)
                                                {
                                                    ttProviderSystems.PCC = ttCredential.Providers[i].PCC;
                                                }

                                                var oWorldspan = new wsTravelTalk.cServiceWorldspan();
                                                oWorldspan.GotResponse += this.GotResponse;

                                                DataView ttCities;
                                                ttCities = (DataView)Application.Get("ttCities");

                                                {
                                                    ref var withBlock4 = ref oWorldspan;
                                                    withBlock4.ServiceID = ttServiceID;
                                                    withBlock4.Request = lfstrRequest[j];
                                                    withBlock4.ProviderSystems = ttProviderSystems;
                                                    // .Version = "v03"
                                                    withBlock4.ttCities = ttCities;
                                                }

                                                DoWorldspanSearches[j] = new wsTravelTalk.SearchWorldspan_v03(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oWorldspan);
                                                DoWorldspanSearches[j].Request = lfstrRequest[j];
                                                DoWorldspanSearches[j].ServiceID = ttServiceID.ToString();
                                                DoWorldspanSearches[j].BeginSearch();
                                            }

                                            catch (Exception e)
                                            {
                                                GotResponse(FormatErrorMessage((ttServices)ttServiceID, e.Message, withBlock.Providers[i].Name));
                                            }

                                            break;
                                        }

                                    case "portal":
                                        {
                                            break;
                                        }
                                    // Try
                                    // sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                                    // ttProviderSystems = Application.Get(sb.ToString())
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
                                    // .Request = lfstrRequest(j)
                                    // .ProviderSystems = ttProviderSystems
                                    // '.Version = "v03"
                                    // End With

                                    // DoPortalSearches(j) = New SearchPortal_v03(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oPortal)
                                    // DoPortalSearches(j).Request = lfstrRequest(j)
                                    // DoPortalSearches(j).ServiceID = ttServiceID
                                    // DoPortalSearches(j).BeginSearch()

                                    // Catch e As Exception
                                    // GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                    // End Try

                                    // Try
                                    // sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                                    // ttProviderSystems = Application.Get(sb.ToString())
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
                                    // .Request = lfstrRequest(j)
                                    // .ProviderSystems = ttProviderSystems
                                    // '.Version = "v03"
                                    // End With

                                    // DoPortalXMLSearches(j) = New SearchPortalXML_v03(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oPortalXML)
                                    // DoPortalXMLSearches(j).Request = lfstrRequest(j)
                                    // DoPortalXMLSearches(j).ServiceID = ttServiceID
                                    // DoPortalXMLSearches(j).BeginSearch()

                                    // Catch e As Exception
                                    // GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                    // End Try
                                    case "portalxml":
                                        {
                                            break;
                                        }

                                    default:
                                        {
                                            sb.Append("Provider ").Append(ttCredential.Providers[i].Name).Append(" Not Currently Supported.");
                                            throw new Exception(sb.ToString());
                                            sb.Remove(0, sb.Length);
                                            break;
                                        }
                                }
                                j += 1;
                            }
                        }
                    }

                }

                StartCounter = DateTime.Now;

                // Do While mintProviders < ttCredential.Providers.Length
                while (mintProviders < j)
                {
                    if ((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalSeconds) > wsTravelTalk.modMain.CPrdTimeOut)
                        break;
                    System.Threading.Thread.Sleep(10);
                }

                // If ttCredential.Providers.Length > 1 Then
                if (j > 1)
                {
                    strResponse = string.Concat("<SuperRS>", mstrResponse, "</SuperRS>");
                    // Aggregate
                    wsTravelTalk.cAggregation.Aggregate((ttServices)ttServiceID, XslPath, "", ref strResponse);

                    // Filter Flights

                    if (ttProviderSystems.AggFilter == true)
                    {
                        sb.Append("ttFP").Append(ttCredential.UserID);
                        FilterFlights(ref strResponse, Conversions.ToString(Application.Get(sb.ToString())));
                        sb.Remove(0, sb.Length);
                    }
                }
                else
                {
                    strResponse = mstrResponse;
                }

                StartCounter = DateTime.Now;
                strResponse = DecodeLowFare(strResponse, ttCredential.UserID);
                sb.Append("Decoding = ").Append((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalMilliseconds));
                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.ToString(), "", UUID);

                if (strResponse.IndexOf("<SearchPromotionsResponse>") != -1)
                {
                    wsTravelTalk.cAggregation.ProcessMarkup(XslPath, "", ref strResponse);
                }

                wsTravelTalk.modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage((ttServices)ttServiceID, ex.Message, "");
            }
            finally
            {
                wsTravelTalk.modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, ttServiceID, Server.MachineName, ref UUID);
                wsTravelTalk.modMain.LogDeals(ref strRequest, ref strResponse);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsLowFare_v03", "============= OTA Response ============= ", strResponse, UUID);
            }

            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [CompressionExtension.CompressionExtension()]
        [WebMethod(Description = "Process Low Fare Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public wsTravelTalk.wmLowFareOut.OTA_AirLowFareSearchRS wmLowFare(wsTravelTalk.wmLowFareIn_v03.OTA_AirLowFareSearchRQ OTA_AirLowFareSearchRQ)
        {

            string xmlMessage = "";
            wsTravelTalk.wmLowFareOut.OTA_AirLowFareSearchRS oLowFareRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wsTravelTalk.wmLowFareIn_v03.OTA_AirLowFareSearchRQ));

            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_AirLowFareSearchRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, (int)ttServices.LowFare);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(type: typeof(wsTravelTalk.wmLowFareOut.OTA_AirLowFareSearchRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oLowFareRS = (wsTravelTalk.wmLowFareOut.OTA_AirLowFareSearchRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsLowFare_v03", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oLowFareRS;

        }

        [WebMethod(Description = "Process Low Fare Xml Messages Request.")]
        public string wmLowFareXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, (int)ttServices.LowFare);
        }

        #endregion

    }

    // #Region "Search AmadeusWS"
    // Public Class SearchAmadeusWS_v03
    // Private Delegate Sub StartSearch_Delegate()
    // Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearchWS)
    // Private pcc As String = ""
    // Private userid As String = ""
    // Private System As String = ""
    // Private ttProviderSystems As TripXMLProviderSystems
    // Private _ServiceID As String = ""
    // Private _Request As String = ""
    // Private oAmadeusWS As cServiceAmadeusWS

    // Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oAmadeusWS As cServiceAmadeusWS)
    // Me.pcc = _pcc
    // Me.userid = _userid
    // Me.System = _System
    // Me.ttProviderSystems = _ttProviderSystems
    // Me.oAmadeusWS = _oAmadeusWS
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
    // Private Sub DoAmadeusSearchWS()

    // ttProviderSystems.PCC = Me.pcc
    // oAmadeusWS.SendAirRequest()
    // oAmadeusWS = Nothing
    // End Sub
    // End Class
    // #End Region

    // #Region "Search Galileo"
    // Public Class SearchGalileo_v03
    // Private Delegate Sub StartSearch_Delegate()
    // Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoGalileoSearch)
    // Private pcc As String = ""
    // Private userid As String = ""
    // Private System As String = ""
    // Private ttProviderSystems As TripXMLProviderSystems
    // Private _ServiceID As String = ""
    // Private _Request As String = ""
    // Private oGalileo As cServiceGalileo
    // Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oGalileo As cServiceGalileo)
    // Me.pcc = _pcc
    // Me.userid = _userid
    // Me.System = _System
    // Me.ttProviderSystems = _ttProviderSystems
    // Me.oGalileo = _oGalileo
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
    // Private Sub DoGalileoSearch()
    // ttProviderSystems.PCC = Me.pcc
    // oGalileo.SendAirRequest()
    // oGalileo = Nothing
    // End Sub
    // End Class
    // #End Region

    // #Region "Search Sabre"
    // Public Class SearchSabre_v03
    // Private Delegate Sub StartSearch_Delegate()
    // Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearchWS)
    // Private pcc As String = ""
    // Private userid As String = ""
    // Private System As String = ""
    // Private ttProviderSystems As TripXMLProviderSystems
    // Private _ServiceID As String = ""
    // Private _Request As String = ""
    // Private oSabre As cServiceSabre

    // Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oSabre As cServiceSabre)
    // Me.pcc = _pcc
    // Me.userid = _userid
    // Me.System = _System
    // Me.ttProviderSystems = _ttProviderSystems
    // Me.oSabre = _oSabre
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
    // Private Sub DoAmadeusSearchWS()
    // ttProviderSystems.PCC = Me.pcc
    // oSabre.SendAirRequest()
    // oSabre = Nothing
    // End Sub
    // End Class
    // #End Region

    // #Region "Search Worldspan"
    // Public Class SearchWorldspan_v03
    // Private Delegate Sub StartSearch_Delegate()
    // Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearchWS)
    // Private pcc As String = ""
    // Private userid As String = ""
    // Private System As String = ""
    // Private ttProviderSystems As TripXMLProviderSystems
    // Private _ServiceID As String = ""
    // Private _Request As String = ""
    // Private oWorldspan As cServiceWorldspan

    // Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oWorldspan As cServiceWorldspan)
    // Me.pcc = _pcc
    // Me.userid = _userid
    // Me.System = _System
    // Me.ttProviderSystems = _ttProviderSystems
    // Me.oWorldspan = _oWorldspan
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
    // Private Sub DoAmadeusSearchWS()
    // ttProviderSystems.PCC = Me.pcc
    // oWorldspan.SendAirRequest()
    // oWorldspan = Nothing
    // End Sub
    // End Class
    // #End Region

}