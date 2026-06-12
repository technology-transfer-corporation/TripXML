using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using CompressionExtension;
using Microsoft.VisualBasic.CompilerServices;
using TripXMLMain;
using static TripXMLMain.modCore;

namespace wsTripXML.wsTravelTalk
{

    [System.Web.Services.Protocols.SoapDocumentService(RoutingStyle = System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement)]
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsLowFareSchedule", Name = "wsLowFareSchedule", Description = "A TripXML Web Service to Process Low Fare Schedule Messages Request.")]


    public class wsLowFareSchedule : WebService
    {

        #region  Web Services Designer Generated Code 

        public wsLowFareSchedule() : base()
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

        private string DecodeLowFareSchedule(string strResponse, string UserID)
        {
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            DataView ttAirports;
            DataView ttAirlines;
            DataView ttEquipments;

            try
            {

                oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                oRoot = oDoc.DocumentElement;

                ttAirports = (DataView)Application.Get("ttAirports");
                ttAirlines = (DataView)Application.Get("ttAirlines");
                ttEquipments = (DataView)Application.Get("ttEquipments");

                foreach (XmlNode oNode in oRoot.SelectNodes("PricedItineraries/PricedItinerary/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"))
                {
                    // *******************
                    // Decode Airports   *
                    // *******************
                    string argstrCode = oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value;
                    oNode.SelectSingleNode("DepartureAirport").InnerText = modMain.GetDecodeValue(ref ttAirports, ref argstrCode);
                    oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value = argstrCode;
                    string argstrCode1 = oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value;
                    oNode.SelectSingleNode("ArrivalAirport").InnerText = modMain.GetDecodeValue(ref ttAirports, ref argstrCode1);
                    oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value = argstrCode1;

                    // *******************
                    // Decode Airlines   *
                    // *******************
                    if (oNode.SelectSingleNode("OperatingAirline") is not null & oNode.SelectSingleNode("OperatingAirline/@Code") is not null)
                    {
                        if (!string.IsNullOrEmpty(oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value) & string.IsNullOrEmpty(oNode.SelectSingleNode("OperatingAirline").InnerText))
                        {
                            string argstrCode2 = oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value;
                            oNode.SelectSingleNode("OperatingAirline").InnerText = modMain.GetDecodeValue(ref ttAirlines, ref argstrCode2);
                            oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value = argstrCode2;
                        }
                    }
                    if (oNode.SelectSingleNode("MarketingAirline") is not null)
                    {
                        if (string.IsNullOrEmpty(oNode.SelectSingleNode("MarketingAirline").InnerText))
                        {
                            string argstrCode3 = oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value;
                            oNode.SelectSingleNode("MarketingAirline").InnerText = modMain.GetDecodeValue(ref ttAirlines, ref argstrCode3);
                            oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value = argstrCode3;
                        }
                    }

                    // *******************
                    // Decode Equipments *
                    // *******************
                    if (oNode.SelectSingleNode("Equipment") is not null)
                    {
                        string argstrCode4 = oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value;
                        oNode.SelectSingleNode("Equipment").InnerText = modMain.GetDecodeValue(ref ttEquipments, ref argstrCode4);
                        oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value = argstrCode4;
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
        private StringBuilder sb = new StringBuilder();

        private string mstrResponse = "";
        private int mintProviders = 0;

        private void GotResponse(string Response)
        {
            mstrResponse += Response;
            mintProviders += 1;
        }

        public TripXML tXML;

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
            FlightSegment[] FlightSegments = null;
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            XmlNode oNode = null;
            XmlNode oNodeOnd = null;
            XmlNode oNodeFlight = null;
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
                                                }
                                                else
                                                {
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

            try
            {
                StartTime = DateTime.Now;

                var argoApp = Application;
                modMain.PreServiceRequestPool(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                ValidateXSDOut = Conversions.ToBoolean(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()));
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

                                        // ttAA = Application.Get(sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(i).PCC).ToString())
                                        sb.Remove(0, sb.Length);

                                        // If ttAA Is Nothing Then
                                        ttProviderSystems = (TripXMLProviderSystems)Application.Get(sb.Append("PS").Append(ttCredential.Providers[i].Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers[i].PCC).ToString());
                                        sb.Remove(0, sb.Length);

                                        if (ttProviderSystems.AmadeusWS == false)
                                        {
                                            GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), withBlock.Providers[i].Name));
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
                                            oThreadAmadeusWS.Start();
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

                                            // Dim oThreadAmadeus As New Thread(New ThreadStart(AddressOf oAmadeus.SendAirRequest))

                                            // With oAmadeus
                                            // .ServiceID = ttServiceID
                                            // .Request = strRequest
                                            // .ttAA = ttAA
                                            // .Version = ""
                                            // End With

                                            // oThreadAmadeus.Start()

                                            // Application.Set(sb.Append("API").Append(.UserID).Append(.System).ToString(), ttAA)
                                            // sb.Remove(0, sb.Length())

                                            // oAmadeus = Nothing
                                        }
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
                                        ttProviderSystems = (TripXMLProviderSystems)Application.Get(sb.ToString());
                                        sb.Remove(0, sb.Length);

                                        if (ttProviderSystems.System is null)
                                        {
                                            sb.Append("Access denied to ").Append(withBlock.Providers[i].Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.");
                                            GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), withBlock.Providers[i].Name));
                                            sb.Remove(0, sb.Length);
                                            break;
                                        }

                                        ttProviderSystems.AAAPCC = withBlock.Providers[i].PCC;

                                        var oSabre = new cServiceSabre();
                                        var oThreadSabre = new Thread(new ThreadStart(oSabre.SendAirRequest));
                                        oSabre.GotResponse += GotResponse;

                                        DataView ttCities;
                                        ttCities = (DataView)Application.Get("ttCities");

                                        {
                                            ref var withBlock1 = ref oSabre;
                                            withBlock1.ServiceID = ttServiceID;
                                            withBlock1.Request = strRequest;
                                            withBlock1.ProviderSystems = ttProviderSystems;
                                            withBlock1.Version = "";
                                            withBlock1.ttCities = ttCities;
                                        }

                                        oThreadSabre.Start();

                                        oSabre = null;
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
                                        ttProviderSystems = (TripXMLProviderSystems)Application.Get(sb.Append("PS").Append(ttCredential.Providers[i].Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers[i].PCC).ToString());

                                        // sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                                        // ttProviderSystems = Application.Get(sb.ToString())
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
                                        var oThreadGalileo = new Thread(new ThreadStart(oGalileo.SendAirRequest));
                                        oGalileo.GotResponse += GotResponse;

                                        {
                                            ref var withBlock2 = ref oGalileo;
                                            withBlock2.ServiceID = (int)ttServiceID;
                                            withBlock2.Request = strRequest;
                                            withBlock2.ProviderSystems = ttProviderSystems;
                                            withBlock2.Version = "";
                                        }

                                        oThreadGalileo.Start();

                                        oGalileo = null;
                                    }
                                    catch (Exception e)
                                    {
                                        GotResponse(FormatErrorMessage(ttServiceID, e.Message, withBlock.Providers[i].Name));

                                        // Case "sentient"
                                        // Try
                                        // ttProviderSystems = Application.Get("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                                        // If ttProviderSystems.System Is Nothing Then
                                        // GotResponse(FormatErrorMessage(ttServiceID, "Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(.System).Append(" system. Or invalid provider.", .Providers(i).Name))
                                        // Exit Select
                                        // End If

                                        // If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                        // ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                        // End If

                                        // Dim oSentient As New cServiceSentient
                                        // Dim oThreadSentient As New Thread(New ThreadStart(AddressOf oSentient.SendAirRequest))
                                        // AddHandler oSentient.GotResponse, AddressOf GotResponse

                                        // With oSentient
                                        // .ServiceID = ttServiceID
                                        // .Request = strRequest
                                        // .ProviderSystems = ttProviderSystems
                                        // .Version = ""
                                        // End With

                                        // oThreadSentient.Start()

                                        // oSentient = Nothing
                                        // Catch e As Exception
                                        // GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                        // End Try

                                    }

                                    break;
                                }

                            default:
                                {
                                    throw new Exception(sb.Append("Provider ").Append(ttCredential.Providers[0].Name).Append(" Not Currently Supported.").ToString());
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
                    Thread.Sleep(10);
                }

                if (ttCredential.Providers.Length > 1)
                {
                    strResponse = string.Concat("<SuperRS>", mstrResponse, "</SuperRS>");
                    // Aggregate
                    cAggregation.Aggregate(ttServiceID, XslPath, "", ref strResponse);
                }

                // Filter Flights

                // If ttProviderSystems.AggFilter = True Then
                // FilterFlights(strResponse, CType(Application.Get("ttFP").Append(ttCredential.UserID), String))
                // End If
                else
                {
                    strResponse = mstrResponse;
                }

                StartCounter = DateTime.Now;
                strResponse = DecodeLowFareSchedule(strResponse, ttCredential.UserID);
                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.Append("Decoding = ").Append((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalMilliseconds)).ToString(), "", UUID);
                sb.Remove(0, sb.Length);

                modMain.PostServiceRequest(ref strResponse, ValidateXSDOut, (int)ttServiceID, ttCredential.UserID);
            }

            catch (Exception ex)
            {
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, "");
            }
            finally
            {
                modMain.LogResponse(ref strResponse, ref ttCredential, StartTime, (int)ttServiceID, Server.MachineName, ref UUID);
                if (modCore.Trace)
                    CoreLib.SendTrace(ttCredential.UserID, "wsLowFareSchedule", "============= OTA Response ============= ", strResponse, UUID);
            }
            sb = null;
            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [CompressionExtension()]
        [WebMethod(Description = "Process Low Fare Plus Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public wmLowFareScheduleOut.OTA_AirLowFareSearchScheduleRS wmLowFareSchedule(wmLowFareScheduleIn.OTA_AirLowFareSearchScheduleRQ OTA_AirLowFareSearchScheduleRQ)
        {
            string xmlMessage = "";
            wmLowFareScheduleOut.OTA_AirLowFareSearchScheduleRS oLowFareScheduleRS = null;
            XmlSerializer oSerializer = null;
            System.IO.StringWriter oWriter = null;
            System.IO.StringReader oReader = null;

            oSerializer = new XmlSerializer(typeof(wmLowFareScheduleIn.OTA_AirLowFareSearchScheduleRQ));
            oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_AirLowFareSearchScheduleRQ);
            xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");

            xmlMessage = ServiceRequest(xmlMessage, ttServices.LowFareSchedule);

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(@type: typeof(wmLowFareScheduleOut.OTA_AirLowFareSearchScheduleRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oLowFareScheduleRS = (wmLowFareScheduleOut.OTA_AirLowFareSearchScheduleRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                CoreLib.SendTrace("", "wsLowFareSchedule", "Error Deserialing OTA Response", ex.Message, string.Empty);
            }

            return oLowFareScheduleRS;

        }

        [WebMethod(Description = "Process Low Fare Plus Xml Messages Request.")]
        public string wmLowFareScheduleXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, ttServices.LowFareSchedule);
        }

        #endregion

    }

}