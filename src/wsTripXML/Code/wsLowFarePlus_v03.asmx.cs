using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
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
    [WebService(Namespace = "http://tripxml.downtowntravel.com/tripxml/wsLowFarePlus", Name = "wsLowFarePlus_v03", Description = "A TripXML Web Service to Process Low Fare Plus Messages Request.")]
    public class wsLowFarePlus_v03 : WebService
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

        public wsLowFarePlus_v03() : base()
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
            XmlDocument oDoc = null;
            XmlElement oRoot = null;
            DataView ttAirports;
            DataView ttAirlines;
            DataView ttAirlinesNames;
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
                ttAirlinesNames = (DataView)Application.Get("ttAirlinesNames");
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

                        if (oFlightNode.SelectSingleNode("OperatingAirline") is not null)
                        {
                            if (oFlightNode.SelectSingleNode("OperatingAirline").Attributes["Code"] is not null)
                            {
                                if (!string.IsNullOrEmpty(oFlightNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value))
                                {
                                    if (string.IsNullOrEmpty(oFlightNode.SelectSingleNode("OperatingAirline").InnerText))
                                    {
                                        string argstrCode2 = oFlightNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value;
                                        oFlightNode.SelectSingleNode("OperatingAirline").InnerText = wsTravelTalk.modMain.GetDecodeValue(ref ttAirlines, ref argstrCode2);
                                        oFlightNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value = argstrCode2;
                                    }
                                }
                            }
                            else
                            {
                                string argstrCode3 = oFlightNode.SelectSingleNode("OperatingAirline").InnerText.ToLower();
                                string a = wsTravelTalk.modMain.GetDecodeValue(ref ttAirlinesNames, ref argstrCode3);
                                if (!string.IsNullOrEmpty(a))
                                {
                                    XmlAttribute attCode;
                                    attCode = oDoc.CreateAttribute("Code");
                                    attCode.Value = a;
                                    oFlightNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode);
                                    oFlightNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oFlightNode.SelectSingleNode("OperatingAirline").InnerText.ToLower());
                                }
                            }
                        }

                        // If Not oFlightNode.SelectSingleNode("OperatingAirline") Is Nothing And Not oFlightNode.SelectSingleNode("OperatingAirline/@Code") Is Nothing Then
                        // 'oFareNode = oNode.SelectSingleNode("AirItineraryPricingInfo").Attributes("PricingSource")

                        // 'If oFareNode.InnerText = "Private" Then
                        // '    oFlightNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttHiddenAirlines, oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)

                        // '    If oFlightNode.SelectSingleNode("OperatingAirline").InnerText = "" Then
                        // '        oFlightNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                        // '    End If
                        // 'Else
                        // If oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value <> "" Then
                        // oFlightNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                        // End If
                        // 'End If

                        // End If

                        if (oFlightNode.SelectSingleNode("MarketingAirline") is not null)
                        {

                            // If oFareNode.InnerText = "Private" Then
                            // oFlightNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttHiddenAirlines, oFlightNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)

                            // If oFlightNode.SelectSingleNode("MarketingAirline").InnerText = "" Then
                            // oFlightNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                            // End If
                            // Else
                            string argstrCode4 = oFlightNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value;
                            oFlightNode.SelectSingleNode("MarketingAirline").InnerText = wsTravelTalk.modMain.GetDecodeValue(ref ttAirlines, ref argstrCode4);
                            oFlightNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value = argstrCode4;
                            // End If
                        }

                        // *******************
                        // Decode Equipments *
                        // *******************
                        if (oFlightNode.SelectSingleNode("Equipment") is not null)
                        {
                            string argstrCode5 = oFlightNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value;
                            oFlightNode.SelectSingleNode("Equipment").InnerText = wsTravelTalk.modMain.GetDecodeValue(ref ttEquipments, ref argstrCode5);
                            oFlightNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value = argstrCode5;
                        }

                        // *******************
                        // Decode Stops *
                        // *******************
                        if (oFlightNode.SelectSingleNode("TPA_Extensions/StopInfo") is not null)
                        {
                            foreach (XmlNode stopNode in oFlightNode.SelectNodes("TPA_Extensions/StopInfo"))
                            {
                                string argstrCode6 = stopNode.Attributes["LocationCode"].Value;
                                stopNode.InnerText = wsTravelTalk.modMain.GetDecodeValue(ref ttAirports, ref argstrCode6);
                                stopNode.Attributes["LocationCode"].Value = argstrCode6;

                                if (stopNode.Attributes["AirEquipType"] is not null)
                                {
                                    string equip = stopNode.Attributes["AirEquipType"].Value;
                                    string localGetDecodeValue() { string argstrCode = stopNode.Attributes["AirEquipType"].Value; var ret = wsTravelTalk.modMain.GetDecodeValue(ref ttEquipments, ref argstrCode); stopNode.Attributes["AirEquipType"].Value = argstrCode; return ret; }

                                    equip = equip + "-" + localGetDecodeValue();
                                    stopNode.Attributes["AirEquipType"].Value = equip;
                                }
                            }
                        }
                    }
                }

                strResponse = oDoc.OuterXml;
            }

            catch (Exception ex)
            {
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding LowFarePlus_v03 Response", ex.Message, string.Empty);
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
            var DoAmadeusWSSearches = new SearchAmadeusWS_v03[100];
            var DoGalileoSearches = new SearchGalileo_v03[100];
            var DoSabreSearches = new SearchSabre_v03[100];
            var DoWorldspanSearches = new SearchWorldspan_v03[100];
            // Dim DoTravelFusionSearches(99) As SearchTravelFusion_v03
            // Dim DoAirCanadaSearches(99) As SearchAirCanada_v03
            // Dim DoVukaSearches(99) As SearchVuka_v03
            // Dim DoPytonSearches(99) As SearchPyton_v03
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
            bool SearchODByOneWay = false;
            int officeIDCounter = 0;

            sb = new StringBuilder();
            sb1 = new StringBuilder();

            try
            {
                StartTime = DateTime.Now;


                oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                oRoot = oDoc.DocumentElement;

                // temporary quick fix for Tomtours to be removed as soon as it is fixed on ResVoyage
                if (oRoot.SelectSingleNode("POS/Source/RequestorID/@ID").InnerXml == "TomTours")
                {
                    string farPrefs = "";
                    foreach (XmlNode currentONode in oRoot.SelectNodes("FaringPreferences/FaringPreference"))
                    {
                        oNode = currentONode;
                        if (oNode.Attributes["MaxResponses"] is not null)
                        {
                            oNode.Attributes["MaxResponses"].Value = 125.ToString();
                        }

                        oNode.InnerXml = oNode.InnerXml.Replace("Both", "Published");
                        string farPref = oNode.OuterXml;
                        farPref = farPref.Replace("Published", "Private");
                        farPrefs += farPref;
                    }

                    strRequest = oRoot.OuterXml;
                    strRequest = strRequest.Replace("</FaringPreferences>", farPrefs + "</FaringPreferences>");
                    oDoc.LoadXml(strRequest);
                    oRoot = oDoc.DocumentElement;
                }
                // end of temporary fix for Tomtours

                if (oRoot.SelectSingleNode("@SearchODByOneWay") is not null)
                {
                    if (oRoot.SelectSingleNode("@SearchODByOneWay").InnerText == "true")
                    {
                        SearchODByOneWay = true;
                    }
                }

                sb.Append("<OTA_AirLowFareSearchPlusRQ>").Append("<POS>").Append(oRoot.SelectSingleNode("POS/Source").OuterXml).Append("<TPA_Extensions>").Append("<Provider>").Append("providerNameToReplace").Append(oRoot.SelectSingleNode("POS/TPA_Extensions/Provider/System").OuterXml).Append(oRoot.SelectSingleNode("POS/TPA_Extensions/Provider/Userid").OuterXml).Append(oRoot.SelectSingleNode("POS/TPA_Extensions/Provider/Password").OuterXml).Append("</Provider>").Append("</TPA_Extensions>").Append("</POS>");

                // sb.Append(oRoot.SelectSingleNode("TravelerInfoSummary").OuterXml)
                // sb.Append("<FaringPreferences>").ToString()

                TravelSum = sb.ToString();
                uTravelSum = oRoot.SelectSingleNode("TravelerInfoSummary").InnerXml.ToString();

                // For Each oNode In oRoot.SelectNodes("FaringPreferences/FaringPreference")
                // 'For Each oNode In oRoot.SelectNodes("FaringPreference")
                // 'FaringPreference = oNode.InnerXml
                // sb.Append(oNode.OuterXml)
                // lfstrRequest(j) = sb.Append(oNode.OuterXml).Append("</FaringPreferences></OTA_AirLowFareSearchPlusRQ>").ToString
                // j += 1
                // Next

                sb.Remove(0, sb.Length);

                var argoApp = Application;
                wsTravelTalk.modMain.PreServiceRequestPool(ref strRequest, ref argoApp, ref ttCredential, ref ttProviderSystems, StartTime, ttServiceID, Server.MachineName, ref UUID);
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

                            int iOD = 1;
                            bool skipOD = true;

                            foreach (XmlNode oNodeOD in oRoot.SelectNodes("OriginDestinationInformation"))
                            {

                                if (skipOD)
                                {

                                    if ((ttCredential.Providers[i].PCC.ToUpper() ?? "") == (oNode.Attributes["PseudoCityCode"].Value.ToUpper() ?? "") | (ttCredential.Providers[i].PCC.ToUpper().Substring(1) ?? "") == (oNode.Attributes["PseudoCityCode"].Value.ToUpper() ?? ""))
                                    {
                                        sb.Remove(0, sb.Length);

                                        // sb.Append(TravelSum).Append(oNode.SelectSingleNode("TravelPreferences").OuterXml)
                                        // sb.Append("<TravelerInfoSummary>").Append(uTravelSum).Append(oNode.SelectSingleNode("AirTravelerAvail").OuterXml)
                                        // sb.Append(oNode.SelectSingleNode("PriceRequestInformation").OuterXml).Append("</TravelerInfoSummary></OTA_AirLowFareSearchPlusRQ>")


                                        tempTravelSum = TravelSum.Replace("providerNameToReplace", "<Name PseudoCityCode=\"" + ttCredential.Providers[i].PCC + "\">" + ttCredential.Providers[i].Name + "</Name>");

                                        if (SearchODByOneWay)
                                        {
                                            tempTravelSum = tempTravelSum + oNodeOD.OuterXml;
                                        }
                                        else
                                        {
                                            skipOD = false;
                                            foreach (XmlNode oNodeODS in oRoot.SelectNodes("OriginDestinationInformation"))
                                                tempTravelSum = tempTravelSum + oNodeODS.OuterXml;
                                        }

                                        if (oNode.Attributes["MaxResponses"] is not null)
                                        {
                                            tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchPlusRQ>", "<OTA_AirLowFareSearchPlusRQ MaxResponses=\"" + oNode.Attributes["MaxResponses"].Value + "\">");
                                        }

                                        if (oNode.Attributes["TwoOneWays"] is not null)
                                        {
                                            if (oNode.Attributes["MaxResponses"] is not null)
                                            {
                                                tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchPlusRQ ", "<OTA_AirLowFareSearchPlusRQ TwoOneWays=\"" + oNode.Attributes["TwoOneWays"].Value + "\" ");
                                            }
                                            else
                                            {
                                                tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchPlusRQ>", "<OTA_AirLowFareSearchPlusRQ TwoOneWays=\"" + oNode.Attributes["TwoOneWays"].Value + "\">");
                                            }
                                        }

                                        if (oNode.Attributes["ExcludeLightTicketing"] is not null)
                                        {
                                            if (oNode.Attributes["MaxResponses"] is not null || oNode.Attributes["TwoOneWays"] is not null)
                                            {
                                                tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchPlusRQ ", "<OTA_AirLowFareSearchPlusRQ ExcludeLightTicketing=\"" + oNode.Attributes["ExcludeLightTicketing"].Value + "\" ");
                                            }
                                            else
                                            {
                                                tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchPlusRQ>", "<OTA_AirLowFareSearchPlusRQ ExcludeLightTicketing=\"" + oNode.Attributes["ExcludeLightTicketing"].Value + "\">");
                                            }
                                        }

                                        if (SearchODByOneWay)
                                        {
                                            if (oNode.Attributes["MaxResponses"] is not null || oNode.Attributes["TwoOneWays"] is not null || oNode.Attributes["ExcludeLightTicketing"] is not null)
                                            {
                                                tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchPlusRQ ", "<OTA_AirLowFareSearchPlusRQ EchoToken=\"" + iOD.ToString() + "\" SearchODByOneWay=\"true\" ");
                                            }
                                            else
                                            {
                                                tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchPlusRQ>", "<OTA_AirLowFareSearchPlusRQ EchoToken=\"" + iOD.ToString() + "\" SearchODByOneWay=\"true\">");
                                            }
                                        }

                                        sb.Append(tempTravelSum).Append(oNode.InnerXml);
                                        intChars = sb.ToString().IndexOf("<TravelPreferences>");

                                        // If TravelPreferences tag is not there
                                        if (intChars == -1)
                                        {
                                            repTravelSumString = sb1.Append("<TravelerInfoSummary>").Append(uTravelSum).Append("<AirTravelerAvail>").ToString();
                                            sb1.Remove(0, sb1.Length);
                                            sb.Replace("<AirTravelerAvail>", repTravelSumString).Append("</TravelerInfoSummary></OTA_AirLowFareSearchPlusRQ>");
                                        }
                                        else
                                        {
                                            repTravelSumString = sb1.Append("</TravelPreferences><TravelerInfoSummary>").Append(uTravelSum).ToString();
                                            sb1.Remove(0, sb1.Length);
                                            sb.Replace("</TravelPreferences>", repTravelSumString).Append("</TravelerInfoSummary></OTA_AirLowFareSearchPlusRQ>");

                                        }

                                        lfstrRequest[j] = sb.ToString();

                                        sb.Remove(0, sb.Length);

                                        CoreLib.SendTrace(ttCredential.UserID, "wsLowFarePlus_v03", "======= FaringPreference request ============= ", lfstrRequest[j], ttProviderSystems.LogUUID);

                                        switch (withBlock.Providers[i].Name.ToLower() ?? "")
                                        {
                                            case "amadeus":
                                                {
                                                    try
                                                    {

                                                        // sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(i).PCC.ToUpper())
                                                        // ttProviderSystems = Application.Get(sb.ToString())
                                                        string tPCC;

                                                        tPCC = ttCredential.Providers[i].PCC.Replace("*", "");

                                                        ttProviderSystems = (TripXMLProviderSystems)Application.Get(sb.Append("PS").Append(ttCredential.Providers[i].Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(tPCC).ToString());
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

                                                        DoAmadeusWSSearches[j] = new SearchAmadeusWS_v03(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oAmadeusWS);
                                                        DoAmadeusWSSearches[j].Request = lfstrRequest[j];
                                                        DoAmadeusWSSearches[j].ServiceID = ttServiceID.ToString();
                                                        DoAmadeusWSSearches[j].BeginSearch();

                                                        ttProviderSystems = default;
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
                                                        sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC.ToUpper());
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

                                                        DoGalileoSearches[j] = new SearchGalileo_v03(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oGalileo);
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

                                                    XmlDocument RQdocument;
                                                    RQdocument = new XmlDocument();
                                                    RQdocument.LoadXml(lfstrRequest[j]);

                                                    XmlElement officeIDElement;
                                                    officeIDElement = RQdocument.DocumentElement;

                                                    string pseudoCityCode = null;
                                                    XmlNode nodeOffice = null;

                                                    // If officeIDElement.FirstChild().LastChild().FirstChild.ChildNodes(officeIDCounter).Attributes().Count > 0 And officeIDElement.FirstChild().LastChild().FirstChild.ChildNodes(officeIDCounter).Attributes() IsNot Nothing Then

                                                    // For Each nodeOffice In officeIDElement.FirstChild().LastChild().FirstChild.ChildNodes(officeIDCounter).Attributes

                                                    // If nodeOffice IsNot Nothing Then

                                                    // If nodeOffice.LocalName = "PseudoCityCode" Then

                                                    // pseudoCityCode = nodeOffice.InnerText
                                                    // officeIDCounter += 1
                                                    // Else
                                                    // pseudoCityCode = pseudoCityCode = officeIDElement.FirstChild().LastChild().FirstChild.ChildNodes(0).Attributes(0).InnerText
                                                    // End If

                                                    // End If

                                                    // Next
                                                    // Else
                                                    // pseudoCityCode = officeIDElement.FirstChild().LastChild().FirstChild.ChildNodes(0).Attributes(0).InnerText
                                                    // End If

                                                    // RQdocument.FirstChild().FirstChild().FirstChild().Attributes(0).InnerText = pseudoCityCode

                                                    // lfstrRequest(j) = RQdocument.OuterXml

                                                    // ttProviderSystems.AAAPCC = pseudoCityCode
                                                    // ttProviderSystems.PCC = pseudoCityCode

                                                    try
                                                    {
                                                        sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC.ToUpper());
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

                                                        DoSabreSearches[j] = new SearchSabre_v03(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oSabre);
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
                                                        sb.Append("PS").Append(withBlock.Providers[i].Name).Append(withBlock.UserID).Append(withBlock.System).Append(withBlock.Providers[i].PCC.ToUpper());
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

                                                        DoWorldspanSearches[j] = new SearchWorldspan_v03(withBlock.Providers[i].PCC, withBlock.UserID, withBlock.System, ref ttProviderSystems, ref oWorldspan);
                                                        DoWorldspanSearches[j].Request = lfstrRequest[j];
                                                        DoWorldspanSearches[j].ServiceID = ttServiceID.ToString();
                                                        DoWorldspanSearches[j].BeginSearch();
                                                    }

                                                    catch (Exception e)
                                                    {
                                                        GotResponse(FormatErrorMessage((ttServices)ttServiceID, e.Message, withBlock.Providers[i].Name));

                                                        #region Future Integrations
                                                        // Case "travelfusion"
                                                        // Try
                                                        // oDoc = New XmlDocument
                                                        // oDoc.LoadXml(strRequest)
                                                        // oRoot = oDoc.DocumentElement
                                                        // If oRoot.SelectNodes("OriginDestinationInformation").Count > 2 Then
                                                        // Throw New Exception("Travel Fusion does not support multi destination searches")
                                                        // End If
                                                        // sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC.ToUpper())
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
                                                        // Dim oTravelFusion As New cServiceTravelFusion
                                                        // AddHandler oTravelFusion.GotResponse, AddressOf GotResponse
                                                        // With oTravelFusion
                                                        // .ServiceID = ttServiceID
                                                        // .Request = lfstrRequest(j)
                                                        // .ttProviderSystems = ttProviderSystems
                                                        // .Version = ""
                                                        // End With
                                                        // DoTravelFusionSearches(j) = New SearchTravelFusion_v03(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oTravelFusion)
                                                        // DoTravelFusionSearches(j).Request = lfstrRequest(j)
                                                        // DoTravelFusionSearches(j).ServiceID = ttServiceID
                                                        // DoTravelFusionSearches(j).BeginSearch()
                                                        // Catch e As Exception
                                                        // GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                                        // End Try
                                                        // Case "aircanada"
                                                        // Try
                                                        // 'oDoc = New XmlDocument
                                                        // 'oDoc.LoadXml(strRequest)
                                                        // 'oRoot = oDoc.DocumentElement
                                                        // sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC.ToUpper())
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

                                                        // Dim oAirCanada As New cServiceAirCanada
                                                        // AddHandler oAirCanada.GotResponse, AddressOf GotResponse

                                                        // With oAirCanada
                                                        // .ServiceID = ttServiceID
                                                        // .Request = lfstrRequest(j)
                                                        // .ttProviderSystems = ttProviderSystems
                                                        // .Version = ""
                                                        // End With

                                                        // DoAirCanadaSearches(j) = New SearchAirCanada_v03(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oAirCanada)
                                                        // DoAirCanadaSearches(j).Request = lfstrRequest(j)
                                                        // DoAirCanadaSearches(j).ServiceID = ttServiceID
                                                        // DoAirCanadaSearches(j).BeginSearch()

                                                        // Catch e As Exception
                                                        // GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                                        // End Try
                                                        // Case "vuka"
                                                        // Try
                                                        // 'oDoc = New XmlDocument
                                                        // 'oDoc.LoadXml(strRequest)
                                                        // 'oRoot = oDoc.DocumentElement

                                                        // sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC.ToUpper())
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

                                                        // Dim oVuka As New cServiceVuka
                                                        // AddHandler oVuka.GotResponse, AddressOf GotResponse

                                                        // With oVuka
                                                        // .ServiceID = ttServiceID
                                                        // .Request = lfstrRequest(j)
                                                        // .ttProviderSystems = ttProviderSystems
                                                        // .Version = ""
                                                        // End With

                                                        // DoVukaSearches(j) = New SearchVuka_v03(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oVuka)
                                                        // DoVukaSearches(j).Request = lfstrRequest(j)
                                                        // DoVukaSearches(j).ServiceID = ttServiceID
                                                        // DoVukaSearches(j).BeginSearch()

                                                        // Catch e As Exception
                                                        // GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                                        // End Try
                                                        // Case "pyton"
                                                        // Try
                                                        // 'oDoc = New XmlDocument
                                                        // 'oDoc.LoadXml(strRequest)
                                                        // 'oRoot = oDoc.DocumentElement

                                                        // sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC.ToUpper())
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

                                                        // Dim oPyton As New cServicePyton
                                                        // AddHandler oPyton.GotResponse, AddressOf GotResponse

                                                        // With oPyton
                                                        // .ServiceID = ttServiceID
                                                        // .Request = lfstrRequest(j)
                                                        // .ttProviderSystems = ttProviderSystems
                                                        // .Version = ""
                                                        // End With

                                                        // DoPytonSearches(j) = New SearchPyton_v03(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oPyton)
                                                        // DoPytonSearches(j).Request = lfstrRequest(j)
                                                        // DoPytonSearches(j).ServiceID = ttServiceID
                                                        // DoPytonSearches(j).BeginSearch()

                                                        // Catch e As Exception
                                                        // GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                                        // End Try
                                                        #endregion
                                                    }

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
                                iOD += 1;
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
                    strResponse = string.Concat("<SuperRS>", mstrResponse + strRequest, "</SuperRS>");
                    // Aggregate
                    wsTravelTalk.cAggregation.Aggregate((ttServices)ttServiceID, gXslPath, "", ref strResponse);

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
                strResponse = DecodeLowFarePlus(strResponse, ttCredential.UserID);
                sb.Append("Decoding = ").Append((int)Math.Round(DateTime.Now.Subtract(StartCounter).TotalMilliseconds));
                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.ToString(), "", ttProviderSystems.LogUUID);

                if (strResponse.IndexOf("<SearchPromotionsResponse>") != -1)
                {
                    wsTravelTalk.cAggregation.ProcessMarkup(gXslPath, "", ref strResponse);
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
                    CoreLib.SendTrace(ttCredential.UserID, "wsLowFarePlus_v03", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID);
            }

            return strResponse;

        }

        #endregion

        #region  Web Methods 

        [CompressionExtension.CompressionExtension()]
        [WebMethod(Description = "Process Low Fare Messages Request.")]
        [System.Web.Services.Protocols.SoapHeader("tXML")]
        public wsTravelTalk.wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS wmLowFarePlus(wsTravelTalk.wmLowFarePlusIn_v03.OTA_AirLowFareSearchPlusRQ OTA_AirLowFareSearchPlusRQ)
        {
            var oSerializer = new XmlSerializer(typeof(wsTravelTalk.wmLowFarePlusIn_v03.OTA_AirLowFareSearchPlusRQ));

            var oWriter = new System.IO.StringWriter(new StringBuilder());
            oSerializer.Serialize(oWriter, OTA_AirLowFareSearchPlusRQ);
            string xmlMessage = oWriter.ToString();
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            xmlMessage = xmlMessage.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

            xmlMessage = ServiceRequest(xmlMessage, (int)ttServices.LowFarePlus);

            wsTravelTalk.wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS oLowFarePlusRS;
            System.IO.StringReader oReader;

            try
            {
                oSerializer = null;
                oSerializer = new XmlSerializer(type: typeof(wsTravelTalk.wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS));
                oReader = new System.IO.StringReader(xmlMessage);
                oLowFarePlusRS = (wsTravelTalk.wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS)oSerializer.Deserialize(oReader);
            }
            catch (Exception ex)
            {
                xmlMessage = "<OTA_AirLowFareSearchPlusRS Version=\"v03\"><Errors><Error>" + ex.InnerException.ToString() + "</Error></Errors></OTA_AirLowFareSearchPlusRS>";
                oReader = new System.IO.StringReader(xmlMessage.Replace("&", "&amp;"));
                oLowFarePlusRS = (wsTravelTalk.wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS)oSerializer.Deserialize(oReader);
            }

            return oLowFarePlusRS;

        }

        [WebMethod(Description = "Process Low Fare Plus Xml Messages Request.")]
        public string wmLowFarePlusXml(string xmlRequest)
        {
            return ServiceRequest(xmlRequest, (int)ttServices.LowFarePlus);
        }

        #endregion

    }

    #region Search AmadeusWS
    public class SearchAmadeusWS_v03
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private string _ServiceID = "";
        private string _Request = "";
        private wsTravelTalk.cServiceAmadeusWS oAmadeusWS;

        public SearchAmadeusWS_v03(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref wsTravelTalk.cServiceAmadeusWS _oAmadeusWS)
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
            oAmadeusWS.SendAirRequest();
            oAmadeusWS = null;
        }
    }
    #endregion

    #region Search Galileo
    public class SearchGalileo_v03
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private string _ServiceID = "";
        private string _Request = "";
        private wsTravelTalk.cServiceGalileo oGalileo;
        public SearchGalileo_v03(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref wsTravelTalk.cServiceGalileo _oGalileo)
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
    public class SearchSabre_v03
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private wsTravelTalk.cServiceSabre oSabre;

        public SearchSabre_v03(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref wsTravelTalk.cServiceSabre _oSabre)
        {
            StartSearch_Wrapper = new StartSearch_Delegate(DoAmadeusSearchWS);
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
        private void DoAmadeusSearchWS()
        {
            ttProviderSystems.PCC = pcc;
            oSabre.SendAirRequest();
            oSabre = null;
        }
    }
    #endregion

    #region Search Worldspan
    public class SearchWorldspan_v03
    {
        private delegate void StartSearch_Delegate();
        private StartSearch_Delegate StartSearch_Wrapper;
        private string pcc = "";
        private string userid = "";
        private string System = "";
        private TripXMLProviderSystems ttProviderSystems;
        private string _ServiceID = "";
        private string _Request = "";
        private wsTravelTalk.cServiceWorldspan oWorldspan;

        public SearchWorldspan_v03(string _pcc, string _userid, string _System, ref TripXMLProviderSystems _ttProviderSystems, ref wsTravelTalk.cServiceWorldspan _oWorldspan)
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

    #region Search Pyton
    // Public Class SearchPyton_v03
    // Private Delegate Sub StartSearch_Delegate()
    // Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoPytonSearch)
    // Private pcc As String = ""
    // Private userid As String = ""
    // Private System As String = ""
    // Private ttProviderSystems As TripXMLProviderSystems
    // Private _ServiceID As String = ""
    // Private _Request As String = ""
    // Private oPyton As cServicePyton

    // Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oPyton As cServicePyton)
    // Me.pcc = _pcc
    // Me.userid = _userid
    // Me.System = _System
    // Me.ttProviderSystems = _ttProviderSystems
    // Me.oPyton = _oPyton
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
    // Private Sub DoPytonSearch()
    // ttProviderSystems.PCC = Me.pcc
    // oPyton.SendAirRequest()
    // oPyton = Nothing
    // End Sub
    // End Class
    #endregion

}