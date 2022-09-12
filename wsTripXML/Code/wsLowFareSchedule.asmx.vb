Imports System
Imports System.Web.Services
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Threading
Imports System.Data
Imports System.Text
Imports CompressionExtension

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement), _
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsLowFareSchedule", _
        Name:="wsLowFareSchedule", _
        Description:="A TripXML Web Service to Process Low Fare Schedule Messages Request.")> _
    Public Class wsLowFareSchedule
        Inherits System.Web.Services.WebService

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

        Private Function DecodeLowFareSchedule(ByVal strResponse As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim ttAirports As DataView
            Dim ttAirlines As DataView
            Dim ttEquipments As DataView
            Dim oNode As XmlNode = Nothing

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strResponse)
                oRoot = oDoc.DocumentElement

                ttAirports = CType(Application.Get("ttAirports"), DataView)
                ttAirlines = CType(Application.Get("ttAirlines"), DataView)
                ttEquipments = CType(Application.Get("ttEquipments"), DataView)

                For Each oNode In oRoot.SelectNodes("PricedItineraries/PricedItinerary/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment")
                    ' *******************
                    ' Decode Airports   *
                    ' *******************
                    oNode.SelectSingleNode("DepartureAirport").InnerText = GetDecodeValue(ttAirports, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                    oNode.SelectSingleNode("ArrivalAirport").InnerText = GetDecodeValue(ttAirports, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)

                    ' *******************
                    ' Decode Airlines   *
                    ' *******************
                    If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing And Not oNode.SelectSingleNode("OperatingAirline/@Code") Is Nothing Then
                        If oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value <> "" And oNode.SelectSingleNode("OperatingAirline").InnerText = "" Then
                            oNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                        End If
                    End If
                    If Not oNode.SelectSingleNode("MarketingAirline") Is Nothing Then
                        If oNode.SelectSingleNode("MarketingAirline").InnerText = "" Then
                            oNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttAirlines, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                        End If
                    End If

                    ' *******************
                    ' Decode Equipments *
                    ' *******************
                    If Not oNode.SelectSingleNode("Equipment") Is Nothing Then
                        oNode.SelectSingleNode("Equipment").InnerText = GetDecodeValue(ttEquipments, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                    End If
                Next

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding LowFare Response", ex.Message, String.Empty)
            End Try
            Return strResponse
        End Function

#End Region
        Private sb As StringBuilder = New StringBuilder()

        Private mstrResponse As String = ""
        Private mintProviders As Integer = 0

        Private Sub GotResponse(ByVal Response As String)
            mstrResponse &= Response
            mintProviders += 1
        End Sub

        Public tXML As TripXML

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
            Dim FlightSegments() As FlightSegment = Nothing
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing
            Dim oNodeOnd As XmlNode = Nothing
            Dim oNodeFlight As XmlNode = Nothing
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
                                            If Not (.DepartureDate = oNodeFlight.Attributes("DepartureDateTime").Value And _
                                                .ArrivalDate = oNodeFlight.Attributes("ArrivalDateTime").Value And _
                                                .FlightNo = RemoveLeadingZeros(oNodeFlight.Attributes("FlightNumber").Value) And _
                                                .DepartureAirport = oNodeFlight.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value And _
                                                .ArrivalAirport = oNodeFlight.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value And _
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
                                            Else
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

            Try
                StartTime = Now

                PreServiceRequestPool(strRequest, Application, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                With ttCredential
                    For i = 0 To .Providers.Length - 1
                        Select Case .Providers(i).Name.ToLower
                            Case "amadeus"
                                Try
                                    'Dim ttAA As AmadeusAPIAdapter

                                    'ttAA = Application.Get(sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(i).PCC).ToString())
                                    sb.Remove(0, sb.Length())

                                    'If ttAA Is Nothing Then
                                    ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(i).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(i).PCC).ToString())
                                    sb.Remove(0, sb.Length())

                                    If ttProviderSystems.AmadeusWS = False Then
                                        GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), .Providers(i).Name))
                                        sb.Remove(0, sb.Length())
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
                                        Dim oThreadAmadeusWS As New Thread(New ThreadStart(AddressOf oAmadeusWS.SendAirRequest))
                                        AddHandler oAmadeusWS.GotResponse, AddressOf GotResponse

                                        With oAmadeusWS
                                            .ServiceID = ttServiceID
                                            .Request = strRequest
                                            .ttProviderSystems = ttProviderSystems
                                            .Version = ""
                                        End With
                                        oThreadAmadeusWS.Start()
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

                                        '    Dim oThreadAmadeus As New Thread(New ThreadStart(AddressOf oAmadeus.SendAirRequest))

                                        '    With oAmadeus
                                        '        .ServiceID = ttServiceID
                                        '        .Request = strRequest
                                        '        .ttAA = ttAA
                                        '        .Version = ""
                                        '    End With

                                        '    oThreadAmadeus.Start()

                                        '    Application.Set(sb.Append("API").Append(.UserID).Append(.System).ToString(), ttAA)
                                        '    sb.Remove(0, sb.Length())

                                        '    oAmadeus = Nothing
                                    End If
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

                                    Dim oSabre As New cServiceSabre
                                    Dim oThreadSabre As New Thread(New ThreadStart(AddressOf oSabre.SendAirRequest))
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

                                    oThreadSabre.Start()

                                    oSabre = Nothing
                                Catch e As Exception
                                    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                End Try

                            Case "apollo", "galileo"
                                Try
                                    ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(i).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(i).PCC).ToString())

                                    'sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                                    'ttProviderSystems = Application.Get(sb.ToString())
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
                                    Dim oThreadGalileo As New Thread(New ThreadStart(AddressOf oGalileo.SendAirRequest))
                                    AddHandler oGalileo.GotResponse, AddressOf GotResponse

                                    With oGalileo
                                        .ServiceID = ttServiceID
                                        .Request = strRequest
                                        .ProviderSystems = ttProviderSystems
                                        .Version = ""
                                    End With

                                    oThreadGalileo.Start()

                                    oGalileo = Nothing
                                Catch e As Exception
                                    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                End Try

                                'Case "sentient"
                                '    Try
                                '        ttProviderSystems = Application.Get("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                                '        If ttProviderSystems.System Is Nothing Then
                                '            GotResponse(FormatErrorMessage(ttServiceID, "Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(.System).Append(" system. Or invalid provider.", .Providers(i).Name))
                                '            Exit Select
                                '        End If

                                '        If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                '            ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                '        End If

                                '        Dim oSentient As New cServiceSentient
                                '        Dim oThreadSentient As New Thread(New ThreadStart(AddressOf oSentient.SendAirRequest))
                                '        AddHandler oSentient.GotResponse, AddressOf GotResponse

                                '        With oSentient
                                '            .ServiceID = ttServiceID
                                '            .Request = strRequest
                                '            .ProviderSystems = ttProviderSystems
                                '            .Version = ""
                                '        End With

                                '        oThreadSentient.Start()

                                '        oSentient = Nothing
                                '    Catch e As Exception
                                '        GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                '    End Try

                            Case Else
                                Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                                sb.Remove(0, sb.Length())
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

                    'If ttProviderSystems.AggFilter = True Then
                    '    FilterFlights(strResponse, CType(Application.Get("ttFP").Append(ttCredential.UserID), String))
                    'End If
                Else
                    strResponse = mstrResponse
                End If

                StartCounter = Now
                strResponse = DecodeLowFareSchedule(strResponse, ttCredential.UserID)
                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.Append("Decoding = ").Append(CType(Now.Subtract(StartCounter).TotalMilliseconds, Integer)).ToString(), "", UUID)
                sb.Remove(0, sb.Length())

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, "")
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsLowFareSchedule", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()> _
        <WebMethod(Description:="Process Low Fare Plus Messages Request.")> _
        <System.Web.Services.Protocols.SoapHeader("tXML")> _
        Public Function wmLowFareSchedule(ByVal OTA_AirLowFareSearchScheduleRQ As wmLowFareScheduleIn.OTA_AirLowFareSearchScheduleRQ) As <XmlElementAttribute("OTA_AirLowFareSearchPlusRS")> wmLowFareScheduleOut.OTA_AirLowFareSearchScheduleRS
            Dim xmlMessage As String = ""
            Dim oLowFareScheduleRS As wmLowFareScheduleOut.OTA_AirLowFareSearchScheduleRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmLowFareScheduleIn.OTA_AirLowFareSearchScheduleRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_AirLowFareSearchScheduleRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.LowFareSchedule)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(Type:=GetType(wmLowFareScheduleOut.OTA_AirLowFareSearchScheduleRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oLowFareScheduleRS = CType(oSerializer.Deserialize(oReader), wmLowFareScheduleOut.OTA_AirLowFareSearchScheduleRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsLowFareSchedule", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oLowFareScheduleRS

        End Function

        <WebMethod(Description:="Process Low Fare Plus Xml Messages Request.")> _
        Public Function wmLowFareScheduleXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.LowFareSchedule)
        End Function

#End Region

    End Class

End Namespace
