Imports System.Web.Services
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Threading

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsLowFareFlights",
        Name:="wsLowFareFlights",
        Description:="A TripXML Web Service to Process Low Fare Flights Messages Request.")>
    Public Class wsLowFareFlights
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

        Private Function DecodeLowFarePlus(ByVal strResponse As String, ByVal UserID As String) As String
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
                            If oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value <> "" Then
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
                            oFlightNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
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
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding LowFarePlus Response", ex.Message, String.Empty)
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
            'Dim DoAmadeusSearches(99) As SearchAmadeusFlights
            Dim DoAmadeusWSSearches(99) As SearchAmadeusWSFlights
            Dim DoGalileoSearches(99) As SearchGalileoFlights
            Dim DoSabreSearches(99) As SearchSabreFlights
            Dim DoWorldspanSearches(99) As SearchWorldspanFlights
            'Dim DoPortalSearches(99) As SearchPortalFlights
            'Dim DoPortalXMLSearches(99) As SearchPortalXML
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

                'sb.Append("<OTA_AirLowFareSearchPlusRQ>").Append(oRoot.SelectSingleNode("POS").OuterXml)


                'For Each oNode In oRoot.SelectNodes("OriginDestinationInformation")
                '    sb.Append(oNode.OuterXml)
                'Next

                ''uTravelSum = sb.Append(oRoot.SelectSingleNode("TravelerInfoSummary").OuterXml).ToString()
                ''sb.Append("<FaringPreferences>").ToString()

                ''For Each oNode In oRoot.SelectNodes("FaringPreferences/FaringPreference")
                ''    sb.Append(oNode.OuterXml)
                ''    sb.Remove(0, sb.Length)
                ''    lfstrRequest(j) = sb.Append(uTravelSum).Append(oNode.OuterXml).Append("</OTA_AirLowFareSearchPlusRQ>").ToString
                ''    j += 1

                ''Next

                ''sb.Remove(0, sb.Length)

                'TravelSum = sb.ToString()
                'uTravelSum = oRoot.SelectSingleNode("TravelerInfoSummary").InnerXml.ToString()

                'For Each oNode In oRoot.SelectNodes("FaringPreferences/FaringPreference")

                '    sb.Remove(0, sb.Length)
                '    sb.Append(TravelSum).Append(oNode.SelectSingleNode("TravelPreferences").OuterXml)
                '    sb.Append("<TravelerInfoSummary>").Append(uTravelSum).Append(oNode.SelectSingleNode("AirTravelerAvail").OuterXml)
                '    sb.Append(oNode.SelectSingleNode("PriceRequestInformation").OuterXml).Append("</TravelerInfoSummary></OTA_AirLowFareSearchPlusRQ>")

                '    lfstrRequest(j) = sb.ToString()
                '    j += 1
                'Next

                'sb.Remove(0, sb.Length)

                PreServiceRequestPool(strRequest, Application, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                sb.Append("XSD").Append(ttCredential.UserID).Append("Out")
                ValidateXSDOut = Application.Get(sb.ToString())
                sb.Remove(0, sb.Length)

                'strRequest= "<?xml version="1.0" encoding="utf-16"?><OTA_AirLowFareSearchPlusRQ><POS><Source PseudoCityCode="MIA1S21AV"><RequestorID Type="21" ID="Thomalex" /></Source><TPA_Extensions><Provider><Name>Amadeus</Name><System>Test</System><Userid>Thomalex</Userid><Password>thefalls</Password></Provider></TPA_Extensions></POS><OriginDestinationInformation><DepartureDateTime>2010-03-04T00:00:00</DepartureDateTime><OriginLocation LocationCode="ATL" /><DestinationLocation LocationCode="MIA" /></OriginDestinationInformation><TravelerInfoSummary><SeatsRequested>1</SeatsRequested><FaringPreferences><FaringPreference PseudoCityCode="ATL1S2157"><TravelPreferences><VendorPref Code="DL" PreferLevel="Preferred"/><VendorPref Code="UA" PreferLevel="Preferred"/><CabinPref PreferLevel="Preferred" Cabin="Economy"/></TravelPreferences><AirTravelerAvail><PassengerTypeQuantity Code="JCB" Quantity="1"/></AirTravelerAvail><PriceRequestInformation PricingSource="Private"/></FaringPreference><FaringPreference PseudoCityCode="ATL1S2157"><TravelPreferences><VendorPref Code="AA" PreferLevel </AirTravelerAvail><PriceRequestInformation PricingSource="Published"/></FaringPreference><FaringPreference PseudoCityCode="NYC1S218Z"><TravelPreferences><VendorPref Code="AA" PreferLevel="Preferred"/><CabinPref PreferLevel="Preferred" Cabin="Business"/></TravelPreferences><AirTravelerAvail><PassengerTypeQuantity Code="JCB" Quantity="1"/></AirTravelerAvail><PriceRequestInformation PricingSource="Private"/></FaringPreference></FaringPreferences></TravelerInfoSummary></OTA_AirLowFareSearchPlusRQ>"

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

                                        DoAmadeusWSSearches(i) = New SearchAmadeusWSFlights(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oAmadeusWS)
                                        'DoAmadeusWSSearches(i).Request = strRequest
                                        DoAmadeusWSSearches(i).Request = strRequest
                                        DoAmadeusWSSearches(i).ServiceID = ttServiceID
                                        DoAmadeusWSSearches(i).BeginSearch()

                                        ttProviderSystems = Nothing
                                        'Else
                                        '    ttProviderSystems = ttAA.ttProviderSystems

                                        '    If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                        '        ttAA.SourcePCC = ttCredential.Providers(i).PCC
                                        '    Else
                                        '        ttAA.SourcePCC = ttAA.ttProviderSystems.PCC
                                        '    End If

                                        '    Dim oAmadeus As New cServiceAmadeus
                                        '    AddHandler oAmadeus.GotResponse, AddressOf GotResponse

                                        '    With oAmadeus
                                        '        .ServiceID = ttServiceID
                                        '        '.Request = strRequest
                                        '        .Request = strRequest
                                        '        .ttAA = ttAA
                                        '        .Version = ""
                                        '    End With

                                        '    DoAmadeusSearches(i) = New SearchAmadeusFlights(.Providers(i).PCC, .UserID, .System, ttAA, oAmadeus)
                                        '    'DoAmadeusSearches(i).Request = strRequest
                                        '    DoAmadeusSearches(i).Request = strRequest
                                        '    DoAmadeusSearches(i).ServiceID = ttServiceID
                                        '    DoAmadeusSearches(i).BeginSearch()

                                        '    sb.Append("API").Append(.UserID).Append(.System)
                                        '    Application.Set(sb.ToString(), ttAA)
                                        '    sb.Remove(0, sb.Length)
                                    End If
                                Catch e As Exception
                                    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))

                                End Try

                            Case "apollo", "galileo"
                                Try
                                    sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                                    ttProviderSystems = Application.Get(sb.ToString())
                                    sb.Remove(0, sb.Length)

                                    If ttProviderSystems.System Is Nothing Then
                                        sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.")
                                        GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), .Providers(i).Name))
                                        sb.Remove(0, sb.Length)
                                        Exit Select
                                    End If

                                    If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                        ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                    End If

                                    Dim oGalileo As New cServiceGalileo
                                    AddHandler oGalileo.GotResponse, AddressOf GotResponse

                                    With oGalileo
                                        .ServiceID = ttServiceID
                                        '.Request = strRequest
                                        .Request = strRequest
                                        .ProviderSystems = ttProviderSystems
                                        .Version = ""
                                    End With

                                    DoGalileoSearches(i) = New SearchGalileoFlights(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oGalileo)
                                    DoGalileoSearches(i).Request = strRequest
                                    DoGalileoSearches(i).ServiceID = ttServiceID
                                    DoGalileoSearches(i).BeginSearch()

                                Catch e As Exception
                                    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                End Try

                            Case "sabre", "Sabre"
                                Try
                                    sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                                    ttProviderSystems = Application.Get(sb.ToString())
                                    sb.Remove(0, sb.Length)

                                    If ttProviderSystems.System Is Nothing Then
                                        sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.")
                                        GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), .Providers(i).Name))
                                        sb.Remove(0, sb.Length)
                                        Exit Select
                                    End If

                                    ttProviderSystems.AAAPCC = .Providers(i).PCC

                                    'If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                    '    ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                    'End If

                                    Dim oSabre As New cServiceSabre
                                    AddHandler oSabre.GotResponse, AddressOf GotResponse

                                    Dim ttCities As DataView
                                    ttCities = CType(Application.Get("ttCities"), DataView)

                                    With oSabre
                                        .ServiceID = ttServiceID
                                        .Request = strRequest
                                        .ProviderSystems = ttProviderSystems
                                        .Version = ""
                                        .ttCities = ttCities
                                    End With

                                    DoSabreSearches(i) = New SearchSabreFlights(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oSabre)
                                    DoSabreSearches(i).Request = strRequest
                                    DoSabreSearches(i).ServiceID = ttServiceID
                                    DoSabreSearches(i).BeginSearch()

                                Catch e As Exception
                                    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                End Try

                            Case "worldspan", "Worldspan"
                                Try
                                    sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                                    ttProviderSystems = Application.Get(sb.ToString())
                                    sb.Remove(0, sb.Length)

                                    If ttProviderSystems.System Is Nothing Then
                                        sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.")
                                        GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), .Providers(i).Name))
                                        sb.Remove(0, sb.Length)
                                        Exit Select
                                    End If

                                    If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                        ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                    End If

                                    Dim oWorldspan As New cServiceWorldspan
                                    AddHandler oWorldspan.GotResponse, AddressOf GotResponse

                                    Dim ttCities As DataView
                                    ttCities = CType(Application.Get("ttCities"), DataView)

                                    With oWorldspan
                                        .ServiceID = ttServiceID
                                        .Request = strRequest
                                        .ProviderSystems = ttProviderSystems
                                        .Version = ""
                                        .ttCities = ttCities
                                    End With

                                    DoWorldspanSearches(i) = New SearchWorldspanFlights(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oWorldspan)
                                    DoWorldspanSearches(i).Request = strRequest
                                    DoWorldspanSearches(i).ServiceID = ttServiceID
                                    DoWorldspanSearches(i).BeginSearch()

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

                                '    DoPortalSearches(i) = New SearchPortalFlights(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oPortal)
                                '    DoPortalSearches(i).Request = strRequest
                                '    DoPortalSearches(i).ServiceID = ttServiceID
                                '    DoPortalSearches(i).BeginSearch()

                                'Catch e As Exception
                                '    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                'End Try

                            Case "portalxml"
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

                                '    Dim oPortalXML As New cServicePortalXML
                                '    AddHandler oPortalXML.GotResponse, AddressOf GotResponse

                                '    With oPortalXML
                                '        .ServiceID = ttServiceID
                                '        .Request = strRequest
                                '        .ProviderSystems = ttProviderSystems
                                '        .Version = ""
                                '    End With

                                '    DoPortalXMLSearches(i) = New SearchPortalXML(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oPortalXML)
                                '    DoPortalXMLSearches(i).Request = strRequest
                                '    DoPortalXMLSearches(i).ServiceID = ttServiceID
                                '    DoPortalXMLSearches(i).BeginSearch()

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
                    If CType(Now.Subtract(StartCounter).TotalSeconds, Integer) > 300 Then Exit Do
                    Thread.Sleep(10)
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
                strResponse = DecodeLowFarePlus(strResponse, ttCredential.UserID)
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
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsLowFarePlus", "============= OTA Response ============= ", strResponse, UUID)
            End Try

            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Low Fare Flights Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmLowFareFlights(ByVal OTA_AirLowFareSearchFlightsRQ As wmLowFareFlightsIn.OTA_AirLowFareSearchFlightsRQ) As <XmlElementAttribute("OTA_AirLowFareSearchFlightsRS")> wmLowFareFlightsOut.OTA_AirLowFareSearchFlightsRS

            Dim xmlMessage As String = ""
            Dim oLowFareFlightsRS As wmLowFareFlightsOut.OTA_AirLowFareSearchFlightsRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmLowFareFlightsIn.OTA_AirLowFareSearchFlightsRQ))

            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_AirLowFareSearchFlightsRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.LowFareFlights)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmLowFareFlightsOut.OTA_AirLowFareSearchFlightsRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oLowFareFlightsRS = CType(oSerializer.Deserialize(oReader), wmLowFareFlightsOut.OTA_AirLowFareSearchFlightsRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsLowFareFlights", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oLowFareFlightsRS

        End Function

        <WebMethod(Description:="Process Low Fare Flights Xml Messages Request.")> _
        Public Function wmLowFareFlightsXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.LowFareFlights)
        End Function

#End Region

    End Class

#Region "Search Amadeus"
    'Public Class SearchAmadeusFlights
    '    Private Delegate Sub StartSearch_Delegate()
    '    Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearch)
    '    Private pcc As String = ""
    '    Private userid As String = ""
    '    Private System As String = ""
    '    Private ttAA As AmadeusAPIAdapter
    '    Private _ServiceID As String = ""
    '    Private _Request As String = ""
    '    Private oAmadeus As cServiceAmadeus

    '    Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttAA As AmadeusAPIAdapter, ByRef _oAmadeus As cServiceAmadeus)
    '        Me.pcc = _pcc
    '        Me.userid = _userid
    '        Me.System = _System
    '        Me.ttAA = _ttAA
    '        Me.oAmadeus = _oAmadeus
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
    '    Private Sub DoAmadeusSearch()
    '        ttAA.SourcePCC = Me.pcc
    '        oAmadeus.SendAirRequest()
    '        oAmadeus = Nothing
    '    End Sub
    'End Class
#End Region

#Region "Search AmadeusWS"
    Public Class SearchAmadeusWSFlights
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

#Region "Search Galileo"
    Public Class SearchGalileoFlights
        Private Delegate Sub StartSearch_Delegate()
        Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoGalileoSearch)
        Private pcc As String = ""
        Private userid As String = ""
        Private System As String = ""
        Private ttProviderSystems As TripXMLProviderSystems
        Private _ServiceID As String = ""
        Private _Request As String = ""
        Private oGalileo As cServiceGalileo
        Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oGalileo As cServiceGalileo)
            Me.pcc = _pcc
            Me.userid = _userid
            Me.System = _System
            Me.ttProviderSystems = _ttProviderSystems
            Me.oGalileo = _oGalileo
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
        Private Sub DoGalileoSearch()
            ttProviderSystems.PCC = Me.pcc
            oGalileo.SendAirRequest()
            oGalileo = Nothing
        End Sub
    End Class
#End Region

#Region "Search Sabre"
    Public Class SearchSabreFlights
        Private Delegate Sub StartSearch_Delegate()
        Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearchWS)
        Private pcc As String = ""
        Private userid As String = ""
        Private System As String = ""
        Private ttProviderSystems As TripXMLProviderSystems
        Private _ServiceID As String = ""
        Private _Request As String = ""
        Private oSabre As cServiceSabre

        Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oSabre As cServiceSabre)
            Me.pcc = _pcc
            Me.userid = _userid
            Me.System = _System
            Me.ttProviderSystems = _ttProviderSystems
            Me.oSabre = _oSabre
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
            oSabre.SendAirRequest()
            oSabre = Nothing
        End Sub
    End Class
#End Region

#Region "Search Worldspan"
    Public Class SearchWorldspanFlights
        Private Delegate Sub StartSearch_Delegate()
        Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearchWS)
        Private pcc As String = ""
        Private userid As String = ""
        Private System As String = ""
        Private ttProviderSystems As TripXMLProviderSystems
        Private _ServiceID As String = ""
        Private _Request As String = ""
        Private oWorldspan As cServiceWorldspan

        Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oWorldspan As cServiceWorldspan)
            Me.pcc = _pcc
            Me.userid = _userid
            Me.System = _System
            Me.ttProviderSystems = _ttProviderSystems
            Me.oWorldspan = _oWorldspan
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
            oWorldspan.SendAirRequest()
            oWorldspan = Nothing
        End Sub
    End Class
#End Region

#Region "Search Portal"
    'Public Class SearchPortalFlights
    '    Private Delegate Sub StartSearch_Delegate()
    '    Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoPortalSearch)
    '    Private pcc As String = ""
    '    Private userid As String = ""
    '    Private System As String = ""
    '    Private ttProviderSystems As TripXMLProviderSystems
    '    Private _ServiceID As String = ""
    '    Private _Request As String = ""
    '    'Private oPortal As cServicePortal
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

#Region "Search PortalXML"
    'Public Class SearchPortalFlightsXML
    '    Private Delegate Sub StartSearch_Delegate()
    '    Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoPortalXMLSearch)
    '    Private pcc As String = ""
    '    Private userid As String = ""
    '    Private System As String = ""
    '    Private ttProviderSystems As TripXMLProviderSystems
    '    Private _ServiceID As String = ""
    '    Private _Request As String = ""
    '    Private oPortalXML As cServicePortalXML
    '    Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oPortalXML As cServicePortalXML)
    '        Me.pcc = _pcc
    '        Me.userid = _userid
    '        Me.System = _System
    '        Me.ttProviderSystems = _ttProviderSystems
    '        Me.oPortalXML = _oPortalXML
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
    '    Private Sub DoPortalXMLSearch()
    '        ttProviderSystems.PCC = Me.pcc
    '        oPortalXML.SendAirRequest()
    '        oPortalXML = Nothing
    '    End Sub
    'End Class
#End Region

End Namespace

