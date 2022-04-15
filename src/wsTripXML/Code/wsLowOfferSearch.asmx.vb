Imports System.Web.Services
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml
Imports System.Xml.Serialization

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsLowOfferSearch",
        Name:="wsLowOfferSearch",
        Description:="A TripXML Web Service to Process Low Offer Search Messages Request.")>
    Public Class wsLowOfferSearch
        Inherits System.Web.Services.WebService

        Private mstrResponse As String = ""
        Private mintProviders As Integer = 0

        Public Sub GotResponse(ByVal Response As String)
            mstrResponse &= Response
            mintProviders += 1
        End Sub

        Public tXML As TripXML

#Region " Web Services Designer Generated Code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Web Services Designer.
            InitializeComponent()

            'Add your own initialization code after the InitializeComponent() call

        End Sub

        'Required by the Web Services Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Web Services Designer
        'It can be modified using the Web Services Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            components = New System.ComponentModel.Container
        End Sub

        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            'CODEGEN: This procedure is required by the Web Services Designer
            'Do not modify it using the code editor.
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

#End Region

#Region " Decode Functions "

        Private Function DecodeLowOfferSearch(ByVal strResponse As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim ttAirports As DataView
            Dim ttAirlines As DataView
            'Dim ttHiddenAirlines As DataView
            Dim ttEquipments As DataView
            Dim oNode As XmlNode = Nothing
            Dim oFareNode As XmlNode = Nothing
            Dim oFlightNode As XmlNode = Nothing

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strResponse)
                oRoot = oDoc.DocumentElement

                ttAirports = CType(Application.Get("ttAirports"), DataView)
                ttAirlines = CType(Application.Get("ttAirlines"), DataView)
                'ttHiddenAirlines = CType(Application.Get("ttHiddenAirlines"), DataView)
                ttEquipments = CType(Application.Get("ttEquipments"), DataView)

                For Each oNode In oRoot.SelectNodes("PricedItineraries/PricedItinerary")
                    For Each oFlightNode In oNode.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment")
                        ' *******************
                        ' *******************
                        ' Decode Airports   *
                        ' *******************
                        oFlightNode.SelectSingleNode("DepartureAirport").InnerText = GetDecodeValue(ttAirports, oFlightNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                        oFlightNode.SelectSingleNode("ArrivalAirport").InnerText = GetDecodeValue(ttAirports, oFlightNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)

                        ' *******************
                        ' Decode Airlines   *
                        ' *******************
                        If Not oFlightNode.SelectSingleNode("OperatingAirline") Is Nothing And Not oFlightNode.SelectSingleNode("OperatingAirline/@Code") Is Nothing Then
                            'oFareNode = oNode.SelectSingleNode("AirItineraryPricingInfo").Attributes("PricingSource")

                            'If oFareNode.InnerText = "Private" Then
                            '    oFlightNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttHiddenAirlines, oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)

                            '    If oFlightNode.SelectSingleNode("OperatingAirline").InnerText = "" Then
                            '        oFlightNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                            '    End If
                            'Else
                            If oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value <> "" And oFlightNode.SelectSingleNode("OperatingAirline").InnerText = "" Then
                                oFlightNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                            End If
                            'End If

                        End If

                        If Not oFlightNode.SelectSingleNode("MarketingAirline") Is Nothing Then

                            'If oFareNode.InnerText = "Private" Then
                            '    oFlightNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttHiddenAirlines, oFlightNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)

                            '    If oFlightNode.SelectSingleNode("MarketingAirline").InnerText = "" Then
                            '        oFlightNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                            '    End If
                            'Else
                            If oFlightNode.SelectSingleNode("MarketingAirline").InnerText = "" Then
                                oFlightNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                            End If
                            'End If
                        End If

                        ' *******************
                        ' Decode Equipments *
                        ' *******************
                        If Not oFlightNode.SelectSingleNode("Equipment") Is Nothing Then
                            oFlightNode.SelectSingleNode("Equipment").InnerText = GetDecodeValue(ttEquipments, oFlightNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                        End If
                    Next
                Next

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding LowOfferSearch Response", ex.Message, String.Empty)
            End Try
            Return strResponse
        End Function

#End Region

#Region " Filter Flights "

        Private Structure FlightSegment
            Dim DepartureDate As String
            Dim ArrivalDate As String
            Dim FlightNo As String
            Dim DepartureAirport As String
            Dim ArrivalAirport As String
            Dim AirlineCode As String
        End Structure

        Private Function RemoveLeadingZeros(ByVal FlightNo As String) As String
            Dim i As Integer

            For i = 0 To FlightNo.Length - 1
                If String.Compare(FlightNo.Substring(i, 1), "0") <> 0 Then Exit For
            Next

            Return FlightNo.Substring(i)

        End Function

        Private Sub FilterFlights(ByRef strResponse As String, ByVal FeaturedProvider As String)
            Dim Provider As String = ""
            Dim TotalFare As Single
            Dim Fare As String = ""
            Dim offID As String = ""
            Dim FlightSegments() As FlightSegment = Nothing
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing
            Dim oNodeOnd As XmlNode = Nothing
            Dim oNodeFlight As XmlNode = Nothing
            Dim oNote As XmlNode = Nothing
            Dim i As Integer
            Dim j As Integer
            Dim k As Integer
            Dim ItineraryCount As Integer
            Dim SameFlight As Boolean

            Try
                strResponse = strResponse.Replace("Provider=", "Flag="""" Provider=")

                oDoc = New XmlDocument

                oDoc.LoadXml(strResponse)

                oRoot = oDoc.DocumentElement

                ItineraryCount = oRoot.SelectNodes("PricedItineraries/PricedItinerary").Count

                For i = 0 To ItineraryCount - 2 ' Don't get the lastone.
                    ' Get the Node for comparison
                    oNode = oRoot.SelectNodes("PricedItineraries/PricedItinerary").Item(i)
                    Provider = oNode.Attributes("Provider").Value
                    oNodeOnd = oNode.SelectSingleNode("AirItineraryPricingInfo/ItinTotalFare/TotalFare")
                    Fare = oNodeOnd.Attributes("Amount").Value
                    Fare = Fare.Insert(Fare.Length - oNodeOnd.Attributes("DecimalPlaces").Value, ".")
                    TotalFare = CType(Fare, Single)
                    If Not FlightSegments Is Nothing Then Erase FlightSegments
                    j = 0
                    For Each oNodeOnd In oNode.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption")
                        For Each oNodeFlight In oNodeOnd.SelectNodes("FlightSegment")
                            ReDim Preserve FlightSegments(j)
                            With FlightSegments(j)
                                .DepartureDate = oNodeFlight.Attributes("DepartureDateTime").Value
                                .ArrivalDate = oNodeFlight.Attributes("ArrivalDateTime").Value
                                .FlightNo = RemoveLeadingZeros(oNodeFlight.Attributes("FlightNumber").Value)
                                .DepartureAirport = oNodeFlight.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value
                                .ArrivalAirport = oNodeFlight.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value
                                .AirlineCode = oNodeFlight.SelectSingleNode("MarketingAirline").Attributes("Code").Value
                            End With
                            j += 1
                        Next    ' oNodeFlight
                    Next    ' oNodeOnd
                    ' Loop thru the Nodes below and compare
                    For k = i + 1 To ItineraryCount - 1
                        ' First check that it has the same Number of Flight
                        oNode = oRoot.SelectNodes("PricedItineraries/PricedItinerary").Item(k)
                        If String.Compare(Provider, oNode.Attributes("Provider").Value) <> 0 And oNode.Attributes("Flag").Value.Length = 0 Then
                            j = 0
                            For Each oNodeOnd In oNode.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption")
                                For Each oNodeFlight In oNodeOnd.SelectNodes("FlightSegment")
                                    j += 1
                                Next    ' oNodeFlight
                            Next    ' oNodeOnd
                            If j = FlightSegments.Length Then
                                ' Do comparissons
                                j = 0
                                SameFlight = True
                                For Each oNodeOnd In oNode.SelectNodes("AirItinerary/OriginDestinationOptions/OriginDestinationOption")
                                    For Each oNodeFlight In oNodeOnd.SelectNodes("FlightSegment")
                                        With FlightSegments(j)
                                            If Not (.DepartureDate = oNodeFlight.Attributes("DepartureDateTime").Value And
                                                .ArrivalDate = oNodeFlight.Attributes("ArrivalDateTime").Value And
                                                .FlightNo = RemoveLeadingZeros(oNodeFlight.Attributes("FlightNumber").Value) And
                                                .DepartureAirport = oNodeFlight.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value And
                                                .ArrivalAirport = oNodeFlight.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value And
                                                .AirlineCode = oNodeFlight.SelectSingleNode("MarketingAirline").Attributes("Code").Value) Then
                                                SameFlight = False
                                                Exit For
                                            End If
                                        End With
                                        j += 1
                                    Next    ' oNodeFlight
                                    If Not SameFlight Then Exit For
                                Next    ' oNodeOnd
                                If SameFlight Then
                                    ' Check Price
                                    oNodeOnd = oNode.SelectSingleNode("AirItineraryPricingInfo/ItinTotalFare/TotalFare")
                                    Fare = oNodeOnd.Attributes("Amount").Value
                                    Fare = Fare.Insert(Fare.Length - oNodeOnd.Attributes("DecimalPlaces").Value, ".")
                                    Select Case CType(Fare, Single)
                                        Case TotalFare
                                            ' Same Price Check Provider (OfficeID)
                                            If String.Compare(oNode.Attributes("Provider").Value, FeaturedProvider) <> 0 Then
                                                oNode.Attributes("Flag").Value = "Deleted"
                                                offID = oNode.Attributes("Provider").Value
                                                oNode = oRoot.SelectNodes("PricedItineraries/PricedItinerary").Item(i)
                                                oNote = oDoc.CreateNode(XmlNodeType.Element, "Notes", "")
                                                oNote.InnerText = offID
                                                oNode.AppendChild(oNote)
                                            Else
                                                offID = oRoot.SelectNodes("PricedItineraries/PricedItinerary").Item(i).Attributes("Provider").Value
                                                oNote = oDoc.CreateNode(XmlNodeType.Element, "Notes", "")
                                                oNote.InnerText = offID
                                                oNode.AppendChild(oNote)
                                                oNode = oRoot.SelectNodes("PricedItineraries/PricedItinerary").Item(i)
                                                oNode.Attributes("Flag").Value = "Deleted"
                                                Exit For
                                            End If
                                        Case Is > TotalFare
                                            ' Delete this one (B)
                                            oNode.Attributes("Flag").Value = "Deleted"
                                        Case Is < TotalFare
                                            ' Delete Node A
                                            oNode = oRoot.SelectNodes("PricedItineraries/PricedItinerary").Item(i)
                                            oNode.Attributes("Flag").Value = "Deleted"
                                            ' Skip Further Node A Comparison
                                            Exit For
                                    End Select
                                End If  ' Same Flight
                            End If  ' Same Number of Flights
                        End If  ' Not Deleted
                    Next    ' k = ItineraryCount Node B
                Next    ' i = ItineraryCount Node A

                ' Delete From Xml Documents all Flag as Deleted
                ' Leave the Deleted Flag for Testing.
                oNode = oRoot.SelectSingleNode("PricedItineraries")
                For Each oNodeOnd In oNode.SelectNodes("PricedItinerary")
                    If oNodeOnd.Attributes("Flag").Value = "Deleted" Then
                        oNode.RemoveChild(oNodeOnd)
                    End If
                Next

                oNode = oRoot.SelectSingleNode("PricedItineraries")
                j = oNode.SelectNodes("PricedItinerary").Count
                If j > 200 Then
                    For i = j - 1 To 200 Step -1
                        oNodeOnd = oNode.SelectNodes("PricedItinerary").Item(i)
                        oNode.RemoveChild(oNodeOnd)
                    Next
                End If

                ' Remove the empty Flags
                strResponse = oDoc.OuterXml.Replace("Flag="""" ", "")

            Catch ex As Exception
                ' Error Filtering Flights
            Finally
                oDoc = Nothing
            End Try

        End Sub

#End Region

#Region " Process Service Request All GDS "

        Private Function ServiceRequest(ByVal strRequest As String, ByVal ttServiceID As Integer) As String
            Dim strResponse As String = ""
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim ValidateXSDOut As Boolean
            Dim StartTime As Date
            Dim UUID As String = ""
            Dim i As Integer
            Dim StartCounter As Date
            Dim DoAmadeusWSSearches(99) As SearchAmadeusOfferSearchWS
            'Dim DoPortalSearches(99) As SearchPortalOfferSearch
            Dim sb As StringBuilder = Nothing
            Dim uTravelSum As String = ""
            Dim TravelSum As String = ""
            'Static MultipleCount As Integer = 0
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing
            Dim FaringPreference As String = ""
            Dim OriginDestination As String = ""
            Dim lfstrRequest(99) As String
            Dim j As Integer = 0

            sb = New StringBuilder()

            Try
                StartTime = Now

                'oDoc = New XmlDocument
                'oDoc.LoadXml(strRequest)
                'oRoot = oDoc.DocumentElement

                'sb.Append("<OTA_AirLowOfferSearchPlusRQ>").Append(oRoot.SelectSingleNode("POS").OuterXml)


                'For Each oNode In oRoot.SelectNodes("OriginDestinationInformation")
                '    sb.Append(oNode.OuterXml)
                'Next

                ''uTravelSum = sb.Append(oRoot.SelectSingleNode("TravelerInfoSummary").OuterXml).ToString()
                ''sb.Append("<FaringPreferences>").ToString()

                ''For Each oNode In oRoot.SelectNodes("FaringPreferences/FaringPreference")
                ''    sb.Append(oNode.OuterXml)
                ''    sb.Remove(0, sb.Length)
                ''    lfstrRequest(j) = sb.Append(uTravelSum).Append(oNode.OuterXml).Append("</OTA_AirLowOfferSearchPlusRQ>").ToString
                ''    j += 1

                ''Next

                ''sb.Remove(0, sb.Length)

                'TravelSum = sb.ToString()
                'uTravelSum = oRoot.SelectSingleNode("TravelerInfoSummary").InnerXml.ToString()

                'For Each oNode In oRoot.SelectNodes("FaringPreferences/FaringPreference")

                '    sb.Remove(0, sb.Length)
                '    sb.Append(TravelSum).Append(oNode.SelectSingleNode("TravelPreferences").OuterXml)
                '    sb.Append("<TravelerInfoSummary>").Append(uTravelSum).Append(oNode.SelectSingleNode("AirTravelerAvail").OuterXml)
                '    sb.Append(oNode.SelectSingleNode("PriceRequestInformation").OuterXml).Append("</TravelerInfoSummary></OTA_AirLowOfferSearchPlusRQ>")

                '    lfstrRequest(j) = sb.ToString()
                '    j += 1
                'Next

                'sb.Remove(0, sb.Length)

                PreServiceRequestPool(strRequest, Application, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                sb.Append("XSD").Append(ttCredential.UserID).Append("Out")
                ValidateXSDOut = Application.Get(sb.ToString())
                sb.Remove(0, sb.Length)

                'strRequest= "<?xml version="1.0" encoding="utf-16"?><OTA_AirLowOfferSearchPlusRQ><POS><Source PseudoCityCode="MIA1S21AV"><RequestorID Type="21" ID="Thomalex" /></Source><TPA_Extensions><Provider><Name>Amadeus</Name><System>Test</System><Userid>Thomalex</Userid><Password>thefalls</Password></Provider></TPA_Extensions></POS><OriginDestinationInformation><DepartureDateTime>2010-03-04T00:00:00</DepartureDateTime><OriginLocation LocationCode="ATL" /><DestinationLocation LocationCode="MIA" /></OriginDestinationInformation><TravelerInfoSummary><SeatsRequested>1</SeatsRequested><FaringPreferences><FaringPreference PseudoCityCode="ATL1S2157"><TravelPreferences><VendorPref Code="DL" PreferLevel="Preferred"/><VendorPref Code="UA" PreferLevel="Preferred"/><CabinPref PreferLevel="Preferred" Cabin="Economy"/></TravelPreferences><AirTravelerAvail><PassengerTypeQuantity Code="JCB" Quantity="1"/></AirTravelerAvail><PriceRequestInformation PricingSource="Private"/></FaringPreference><FaringPreference PseudoCityCode="ATL1S2157"><TravelPreferences><VendorPref Code="AA" PreferLevel </AirTravelerAvail><PriceRequestInformation PricingSource="Published"/></FaringPreference><FaringPreference PseudoCityCode="NYC1S218Z"><TravelPreferences><VendorPref Code="AA" PreferLevel="Preferred"/><CabinPref PreferLevel="Preferred" Cabin="Business"/></TravelPreferences><AirTravelerAvail><PassengerTypeQuantity Code="JCB" Quantity="1"/></AirTravelerAvail><PriceRequestInformation PricingSource="Private"/></FaringPreference></FaringPreferences></TravelerInfoSummary></OTA_AirLowOfferSearchPlusRQ>"

                With ttCredential
                    For i = 0 To .Providers.Length - 1
                        Select Case .Providers(i).Name.ToLower
                            Case "amadeus"
                                Try
                                    'Dim ttAA As AmadeusAPIAdapter

                                    sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                                    'ttAA = Application.Get(sb.ToString())
                                    sb.Remove(0, sb.Length)

                                    'If ttAA Is Nothing Then
                                    ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(i).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(i).PCC).ToString())
                                    sb.Remove(0, sb.Length())

                                    If ttProviderSystems.AmadeusWS = False Then
                                        sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.")
                                        GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), .Providers(i).Name))
                                        sb.Remove(0, sb.Length)
                                        Exit Select
                                    End If
                                    'End If

                                    If ttProviderSystems.AmadeusWS = True Then
                                        If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                            ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                        End If

                                        ttCredential.Providers(0).Name = "AmadeusWS"

                                        If ttCredential.System = "Test" Then
                                            ttProviderSystems.URL = "https://test.webservices.amadeus.com"
                                        ElseIf ttCredential.System = "Training" Then
                                            ttProviderSystems.URL = "https://production.webservices.amadeus.com"
                                        Else
                                            ttProviderSystems.URL = "https://production.webservices.amadeus.com"
                                        End If


                                        Dim oAmadeusWS As New cServiceAmadeusWS
                                        AddHandler oAmadeusWS.GotResponse, AddressOf GotResponse

                                        With oAmadeusWS
                                            .ServiceID = ttServiceID
                                            '.Request = strRequest
                                            .Request = strRequest
                                            .ttProviderSystems = ttProviderSystems
                                            '.ttProviderSystems.ProviderSession.MultipleCount = .ttProviderSystems.ProviderSession.MultipleCount + 1
                                            'If .ttProviderSystems.ProviderSession.MultipleCount <> 1 Then
                                            '    .ttProviderSystems.ProviderSession.MultipleAccess = False
                                            'Else
                                            '    .ttProviderSystems.ProviderSession.MultipleAccess = True
                                            'End If
                                            .Version = ""
                                        End With

                                        DoAmadeusWSSearches(i) = New SearchAmadeusOfferSearchWS(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oAmadeusWS)
                                        'DoAmadeusWSSearches(i).Request = strRequest
                                        DoAmadeusWSSearches(i).Request = strRequest
                                        DoAmadeusWSSearches(i).ServiceID = ttServiceID
                                        DoAmadeusWSSearches(i).BeginSearch()

                                        ttProviderSystems = Nothing
                                    Else
                                    End If
                                Catch e As Exception
                                    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))

                                End Try
                            Case "portal"
                                'Try
                                '    sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                                '    ttProviderSystems = Application.Get(sb.ToString())
                                '    sb.Remove(0, sb.Length)

                                '    If ttProviderSystems.System Is Nothing Then
                                '        sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.")
                                '        GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), .Providers(i).Name))
                                '        sb.Remove(0, sb.Length)
                                '        Exit Select
                                '    End If

                                '    If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                '        ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                '    End If

                                '    Dim oPortal As New cServicePortal
                                '    AddHandler oPortal.GotResponse, AddressOf GotResponse

                                '    With oPortal
                                '        .ServiceID = ttServiceID
                                '        .Request = strRequest
                                '        .ProviderSystems = ttProviderSystems
                                '        .Version = ""
                                '    End With

                                '    DoPortalSearches(i) = New SearchPortalOfferSearch(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oPortal)
                                '    DoPortalSearches(i).Request = strRequest
                                '    DoPortalSearches(i).ServiceID = ttServiceID
                                '    DoPortalSearches(i).BeginSearch()

                                'Catch e As Exception
                                '    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                'End Try

                            Case Else
                                sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.")
                                Throw New Exception(sb.ToString())
                                sb.Remove(0, sb.Length)
                        End Select
                    Next

                End With

                StartCounter = Now

                Do While mintProviders < ttCredential.Providers.Length
                    If CType(Now.Subtract(StartCounter).TotalSeconds, Integer) > CPrdTimeOut Then Exit Do
                    Threading.Thread.Sleep(10)
                Loop

                If ttCredential.Providers.Length > 1 Then
                    strResponse = String.Concat("<SuperRS>", mstrResponse, "</SuperRS>")
                    ' Aggregate
                    cAggregation.Aggregate(ttServiceID, XslPath, "", strResponse)

                    ' Filter Flights

                    If ttProviderSystems.AggFilter = True Then
                        sb.Append("ttFP").Append(ttCredential.UserID)
                        FilterFlights(strResponse, CType(Application.Get(sb.ToString()), String))
                        sb.Remove(0, sb.Length)
                    End If
                Else
                    strResponse = mstrResponse
                End If

                StartCounter = Now
                strResponse = DecodeLowOfferSearch(strResponse, ttCredential.UserID)
                sb.Append("Decoding = ").Append(CType(Now.Subtract(StartCounter).TotalMilliseconds, Integer))
                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.ToString(), "", UUID)

                If strResponse.IndexOf("<SearchPromotionsResponse>") <> -1 Then
                    cAggregation.ProcessMarkup(XslPath, "", strResponse)
                End If

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, "")
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsLowOfferSearch", "============= OTA Response ============= ", strResponse, UUID)
            End Try

            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Low Fare Search Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmLowOfferSearch(ByVal OTA_AirLowOfferSearchRQ As wmLowOfferSearchIn.OTA_AirLowOfferSearchRQ) As <XmlElementAttribute("OTA_AirLowOfferSearchRS")> wmLowOfferSearchOut.OTA_AirLowOfferSearchRS

            Dim xmlMessage As String = ""
            Dim oLowOfferSearchRS As wmLowOfferSearchOut.OTA_AirLowOfferSearchRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmLowOfferSearchIn.OTA_AirLowOfferSearchRQ))

            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_AirLowOfferSearchRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.LowOfferSearch)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmLowOfferSearchOut.OTA_AirLowOfferSearchRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oLowOfferSearchRS = CType(oSerializer.Deserialize(oReader), wmLowOfferSearchOut.OTA_AirLowOfferSearchRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsLowOfferSearch", "Error Deserialing OTA Response", ex.Message, String.Empty)
                xmlMessage = "<OTA_AirLowOfferSearchRS Version=""1.001""><Errors><Error>" & ex.InnerException.ToString() & "</Error></Errors></OTA_AirLowOfferSearchRS>"
                oReader = New System.IO.StringReader(xmlMessage)
                oLowOfferSearchRS = CType(oSerializer.Deserialize(oReader), wmLowOfferSearchOut.OTA_AirLowOfferSearchRS)
            End Try

            Return oLowOfferSearchRS

        End Function

        <WebMethod(Description:="Process Low Fare Search Xml Messages Request.")> _
        Public Function wmLowOfferSearchXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.LowOfferSearch)
        End Function

#End Region

    End Class

#Region "Search AmadeusWS"
    Public Class SearchAmadeusOfferSearchWS
        Private Delegate Sub StartSearch_Delegate()
        Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearchWS)
        Private pcc As String = ""
        Private userid As String = ""
        Private System As String = ""
        Private ttProviderSystems As TripXMLProviderSystems
        Private _ServiceID As String = ""
        Private _Request As String = ""
        Private oAmadeusWS As cServiceAmadeusWS

        Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oAmadeusWS As cServiceAmadeusWS)
            Me.pcc = _pcc
            Me.userid = _userid
            Me.System = _System
            Me.ttProviderSystems = _ttProviderSystems
            Me.oAmadeusWS = _oAmadeusWS
        End Sub
        Public Property ServiceID() As String
            Get
                Return _ServiceID
            End Get
            Set(ByVal value As String)
                _ServiceID = value
            End Set
        End Property
        Public Property Request() As String
            Get
                Return _Request
            End Get
            Set(ByVal value As String)
                _Request = value
            End Set
        End Property
        Public Sub BeginSearch()
            Dim cbr As AsyncCallback = New AsyncCallback(AddressOf EndSearch)
            Dim arr As IAsyncResult = StartSearch_Wrapper.BeginInvoke(cbr, Nothing)
        End Sub
        Private Sub EndSearch(ByVal asy As IAsyncResult)
            StartSearch_Wrapper.EndInvoke(asy)
            asy.AsyncWaitHandle.Close()
        End Sub
        Private Sub DoAmadeusSearchWS()

            ttProviderSystems.PCC = Me.pcc
            'Static count As Integer = 0
            'count = count + 1
            'If count <> 1 Then
            '    ttProviderSystems.ProviderSession.MultipleAccess = False
            'Else
            '    ttProviderSystems.ProviderSession.MultipleAccess = True
            'End If

            oAmadeusWS.SendAirRequest()
            oAmadeusWS = Nothing
        End Sub
    End Class
#End Region

#Region "Search Portal"
    'Public Class SearchPortalOfferSearch
    '    Private Delegate Sub StartSearch_Delegate()
    '    Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoPortalSearch)
    '    Private pcc As String = ""
    '    Private userid As String = ""
    '    Private System As String = ""
    '    Private ttProviderSystems As TripXMLProviderSystems
    '    Private _ServiceID As String = ""
    '    Private _Request As String = ""
    '    Private oPortal As cServicePortal
    '    Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oPortal As cServicePortal)
    '        Me.pcc = _pcc
    '        Me.userid = _userid
    '        Me.System = _System
    '        Me.ttProviderSystems = _ttProviderSystems
    '        Me.oPortal = _oPortal
    '    End Sub
    '    Public Property ServiceID() As String
    '        Get
    '            Return _ServiceID
    '        End Get
    '        Set(ByVal value As String)
    '            _ServiceID = value
    '        End Set
    '    End Property
    '    Public Property Request() As String
    '        Get
    '            Return _Request
    '        End Get
    '        Set(ByVal value As String)
    '            _Request = value
    '        End Set
    '    End Property
    '    Public Sub BeginSearch()
    '        Dim cbr As AsyncCallback = New AsyncCallback(AddressOf EndSearch)
    '        Dim arr As IAsyncResult = StartSearch_Wrapper.BeginInvoke(cbr, Nothing)
    '    End Sub
    '    Private Sub EndSearch(ByVal asy As IAsyncResult)
    '        StartSearch_Wrapper.EndInvoke(asy)
    '        asy.AsyncWaitHandle.Close()
    '    End Sub
    '    Private Sub DoPortalSearch()
    '        ttProviderSystems.PCC = Me.pcc
    '        oPortal.SendAirRequest()
    '        oPortal = Nothing
    '    End Sub
    'End Class
#End Region

End Namespace

