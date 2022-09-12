Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Globalization
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsLowFarePlus",
        Name:="wsLowFarePlus_v03",
        Description:="A TripXML Web Service to Process Low Fare Plus Messages Request.")>
    Public Class wsLowFarePlus_v03
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
            Dim ttAirlinesNames As DataView
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
                ttAirlinesNames = CType(Application.Get("ttAirlinesNames"), DataView)
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

                        If Not oFlightNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                            If Not oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code") Is Nothing Then
                                If oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value <> "" Then
                                    If oFlightNode.SelectSingleNode("OperatingAirline").InnerText = "" Then
                                        oFlightNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                                    End If
                                End If
                            Else
                                Dim a As String = GetDecodeValue(ttAirlinesNames, oFlightNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                                If a <> "" Then
                                    Dim attCode As XmlAttribute
                                    attCode = oDoc.CreateAttribute("Code")
                                    attCode.Value = a
                                    oFlightNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)
                                    oFlightNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oFlightNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                                End If
                            End If
                        End If

                        'If Not oFlightNode.SelectSingleNode("OperatingAirline") Is Nothing And Not oFlightNode.SelectSingleNode("OperatingAirline/@Code") Is Nothing Then
                        '    'oFareNode = oNode.SelectSingleNode("AirItineraryPricingInfo").Attributes("PricingSource")

                        '    'If oFareNode.InnerText = "Private" Then
                        '    '    oFlightNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttHiddenAirlines, oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)

                        '    '    If oFlightNode.SelectSingleNode("OperatingAirline").InnerText = "" Then
                        '    '        oFlightNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                        '    '    End If
                        '    'Else
                        '    If oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value <> "" Then
                        '        oFlightNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                        '    End If
                        '    'End If

                        'End If

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

                        ' *******************
                        ' Decode Stops *
                        ' *******************
                        If Not oFlightNode.SelectSingleNode("TPA_Extensions/StopInfo") Is Nothing Then
                            Dim stopNode As XmlNode = Nothing
                            For Each stopNode In oFlightNode.SelectNodes("TPA_Extensions/StopInfo")
                                stopNode.InnerText = GetDecodeValue(ttAirports, stopNode.Attributes("LocationCode").Value)

                                If Not stopNode.Attributes("AirEquipType") Is Nothing Then
                                    Dim equip As String = stopNode.Attributes("AirEquipType").Value
                                    equip = equip + "-" + GetDecodeValue(ttEquipments, stopNode.Attributes("AirEquipType").Value)
                                    stopNode.Attributes("AirEquipType").Value = equip
                                End If
                            Next
                        End If
                    Next
                Next

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding LowFarePlus_v03 Response", ex.Message, String.Empty)
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
            Dim DoAmadeusWSSearches(99) As SearchAmadeusWS_v03
            Dim DoGalileoSearches(99) As SearchGalileo_v03
            Dim DoSabreSearches(99) As SearchSabre_v03
            Dim DoWorldspanSearches(99) As SearchWorldspan_v03
            'Dim DoTravelFusionSearches(99) As SearchTravelFusion_v03
            'Dim DoAirCanadaSearches(99) As SearchAirCanada_v03
            'Dim DoVukaSearches(99) As SearchVuka_v03
            'Dim DoPytonSearches(99) As SearchPyton_v03
            Dim sb As StringBuilder = Nothing
            Dim sb1 As StringBuilder = Nothing
            Dim uTravelSum As String = ""
            Dim TravelSum As String = ""
            Dim tempTravelSum As String = ""
            'Static MultipleCount As Integer = 0
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing
            Dim oNodeOD As XmlNode = Nothing
            'Dim FaringPreference As String = ""
            'Dim OriginDestination As String = ""
            Dim lfstrRequest(99) As String
            Dim j As Integer = 0
            Dim repTravelSumString As String = ""
            Dim intChars As Integer = 0
            Dim SearchODByOneWay As Boolean = False
            Dim officeIDCounter As Int32 = 0

            sb = New StringBuilder()
            sb1 = New StringBuilder()

            Try
                StartTime = Now


                oDoc = New XmlDocument
                oDoc.LoadXml(strRequest)
                oRoot = oDoc.DocumentElement

                ' temporary quick fix for Tomtours to be removed as soon as it is fixed on ResVoyage
                If oRoot.SelectSingleNode("POS/Source/RequestorID/@ID").InnerXml = "TomTours" Then
                    Dim farPrefs As String = ""
                    For Each oNode In oRoot.SelectNodes("FaringPreferences/FaringPreference")
                        If Not oNode.Attributes("MaxResponses") Is Nothing Then
                            oNode.Attributes("MaxResponses").Value = 125
                        End If

                        oNode.InnerXml = oNode.InnerXml.Replace("Both", "Published")
                        Dim farPref As String = oNode.OuterXml
                        farPref = farPref.Replace("Published", "Private")
                        farPrefs += farPref
                    Next

                    strRequest = oRoot.OuterXml
                    strRequest = strRequest.Replace("</FaringPreferences>", farPrefs + "</FaringPreferences>")
                    oDoc.LoadXml(strRequest)
                    oRoot = oDoc.DocumentElement
                End If
                ' end of temporary fix for Tomtours

                If Not oRoot.SelectSingleNode("@SearchODByOneWay") Is Nothing Then
                    If oRoot.SelectSingleNode("@SearchODByOneWay").InnerText = "true" Then
                        SearchODByOneWay = True
                    End If
                End If

                sb.Append("<OTA_AirLowFareSearchPlusRQ>").Append("<POS>").Append(oRoot.SelectSingleNode("POS/Source").OuterXml).Append("<TPA_Extensions>").Append("<Provider>").Append("providerNameToReplace").Append(oRoot.SelectSingleNode("POS/TPA_Extensions/Provider/System").OuterXml).Append(oRoot.SelectSingleNode("POS/TPA_Extensions/Provider/Userid").OuterXml).Append(oRoot.SelectSingleNode("POS/TPA_Extensions/Provider/Password").OuterXml).Append("</Provider>").Append("</TPA_Extensions>").Append("</POS>")

                'sb.Append(oRoot.SelectSingleNode("TravelerInfoSummary").OuterXml)
                'sb.Append("<FaringPreferences>").ToString()

                TravelSum = sb.ToString()
                uTravelSum = oRoot.SelectSingleNode("TravelerInfoSummary").InnerXml.ToString()

                'For Each oNode In oRoot.SelectNodes("FaringPreferences/FaringPreference")
                '    'For Each oNode In oRoot.SelectNodes("FaringPreference")
                '    'FaringPreference = oNode.InnerXml
                '    sb.Append(oNode.OuterXml)
                '    lfstrRequest(j) = sb.Append(oNode.OuterXml).Append("</FaringPreferences></OTA_AirLowFareSearchPlusRQ>").ToString
                '    j += 1
                'Next

                sb.Remove(0, sb.Length)

                PreServiceRequestPool(strRequest, Application, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                sb.Append("XSD").Append(ttCredential.UserID).Append("Out")
                ValidateXSDOut = Application.Get(sb.ToString())
                sb.Remove(0, sb.Length)

                With ttCredential
                    For i = 0 To .Providers.Length - 1
                        For Each oNode In oRoot.SelectNodes("FaringPreferences/FaringPreference")

                            Dim iOD As Integer = 1
                            Dim skipOD As Boolean = True

                            For Each oNodeOD In oRoot.SelectNodes("OriginDestinationInformation")

                                If skipOD Then

                                    If (ttCredential.Providers(i).PCC.ToUpper() = oNode.Attributes("PseudoCityCode").Value.ToUpper() Or
                                    ttCredential.Providers(i).PCC.ToUpper().Substring(1) = oNode.Attributes("PseudoCityCode").Value.ToUpper()) Then
                                        sb.Remove(0, sb.Length)

                                        'sb.Append(TravelSum).Append(oNode.SelectSingleNode("TravelPreferences").OuterXml)
                                        'sb.Append("<TravelerInfoSummary>").Append(uTravelSum).Append(oNode.SelectSingleNode("AirTravelerAvail").OuterXml)
                                        'sb.Append(oNode.SelectSingleNode("PriceRequestInformation").OuterXml).Append("</TravelerInfoSummary></OTA_AirLowFareSearchPlusRQ>")


                                        tempTravelSum = TravelSum.Replace("providerNameToReplace", "<Name PseudoCityCode=""" + ttCredential.Providers(i).PCC + """>" + ttCredential.Providers(i).Name + "</Name>")

                                        If SearchODByOneWay Then
                                            tempTravelSum = tempTravelSum & oNodeOD.OuterXml
                                        Else
                                            skipOD = False
                                            Dim oNodeODS As XmlNode
                                            For Each oNodeODS In oRoot.SelectNodes("OriginDestinationInformation")
                                                tempTravelSum = tempTravelSum & oNodeODS.OuterXml
                                            Next
                                        End If

                                        If Not oNode.Attributes("MaxResponses") Is Nothing Then
                                            tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchPlusRQ>", "<OTA_AirLowFareSearchPlusRQ MaxResponses=""" & oNode.Attributes("MaxResponses").Value & """>")
                                        End If

                                        If Not oNode.Attributes("TwoOneWays") Is Nothing Then
                                            If Not oNode.Attributes("MaxResponses") Is Nothing Then
                                                tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchPlusRQ ", "<OTA_AirLowFareSearchPlusRQ TwoOneWays=""" & oNode.Attributes("TwoOneWays").Value & """ ")
                                            Else
                                                tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchPlusRQ>", "<OTA_AirLowFareSearchPlusRQ TwoOneWays=""" & oNode.Attributes("TwoOneWays").Value & """>")
                                            End If
                                        End If


                                        If Not oNode.Attributes("ExcludeLightTicketing") Is Nothing Then
                                            If Not oNode.Attributes("MaxResponses") Is Nothing OrElse Not oNode.Attributes("TwoOneWays") Is Nothing Then
                                                tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchPlusRQ ", "<OTA_AirLowFareSearchPlusRQ ExcludeLightTicketing=""" & oNode.Attributes("ExcludeLightTicketing").Value & """ ")
                                            Else
                                                tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchPlusRQ>", "<OTA_AirLowFareSearchPlusRQ ExcludeLightTicketing=""" & oNode.Attributes("ExcludeLightTicketing").Value & """>")
                                            End If
                                        End If

                                        If SearchODByOneWay Then
                                            If Not oNode.Attributes("MaxResponses") Is Nothing OrElse Not oNode.Attributes("TwoOneWays") Is Nothing OrElse Not oNode.Attributes("ExcludeLightTicketing") Is Nothing Then
                                                tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchPlusRQ ", "<OTA_AirLowFareSearchPlusRQ EchoToken=""" & iOD.ToString() & """ SearchODByOneWay=""true"" ")
                                            Else
                                                tempTravelSum = tempTravelSum.Replace("<OTA_AirLowFareSearchPlusRQ>", "<OTA_AirLowFareSearchPlusRQ EchoToken=""" & iOD.ToString() & """ SearchODByOneWay=""true"">")
                                            End If
                                        End If

                                        sb.Append(tempTravelSum).Append(oNode.InnerXml)
                                        intChars = sb.ToString().IndexOf("<TravelPreferences>")

                                        'If TravelPreferences tag is not there
                                        If intChars = -1 Then
                                            repTravelSumString = sb1.Append("<TravelerInfoSummary>").Append(uTravelSum).Append("<AirTravelerAvail>").ToString()
                                            sb1.Remove(0, sb1.Length)
                                            sb.Replace("<AirTravelerAvail>", repTravelSumString).Append("</TravelerInfoSummary></OTA_AirLowFareSearchPlusRQ>")
                                        Else
                                            repTravelSumString = sb1.Append("</TravelPreferences><TravelerInfoSummary>").Append(uTravelSum).ToString()
                                            sb1.Remove(0, sb1.Length)
                                            sb.Replace("</TravelPreferences>", repTravelSumString).Append("</TravelerInfoSummary></OTA_AirLowFareSearchPlusRQ>")

                                        End If

                                        lfstrRequest(j) = sb.ToString()

                                        sb.Remove(0, sb.Length)

                                        CoreLib.SendTrace(ttCredential.UserID, "wsLowFarePlus_v03", "======= FaringPreference request ============= ", lfstrRequest(j), ttProviderSystems.LogUUID)

                                        Select Case .Providers(i).Name.ToLower
                                            Case "amadeus"
                                                Try

                                                    'sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(i).PCC.ToUpper())
                                                    'ttProviderSystems = Application.Get(sb.ToString())
                                                    Dim tPCC As String

                                                    tPCC = ttCredential.Providers(i).PCC.Replace("*", "")

                                                    ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(i).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(tPCC).ToString())
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

                                                    'ttCredential.Providers(0).Name = "AmadeusWS"

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
                                                        .Request = lfstrRequest(j)
                                                        .ttProviderSystems = ttProviderSystems
                                                        '.Version = "v03"
                                                    End With

                                                    DoAmadeusWSSearches(j) = New SearchAmadeusWS_v03(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oAmadeusWS)
                                                    DoAmadeusWSSearches(j).Request = lfstrRequest(j)
                                                    DoAmadeusWSSearches(j).ServiceID = ttServiceID
                                                    DoAmadeusWSSearches(j).BeginSearch()

                                                    ttProviderSystems = Nothing

                                                Catch e As Exception
                                                    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))

                                                End Try

                                            Case "apollo", "galileo"
                                                Try
                                                    sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC.ToUpper())
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
                                                        .Request = lfstrRequest(j)
                                                        .ProviderSystems = ttProviderSystems
                                                        '.Version = "v03"
                                                    End With

                                                    DoGalileoSearches(j) = New SearchGalileo_v03(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oGalileo)
                                                    DoGalileoSearches(j).Request = lfstrRequest(j)
                                                    DoGalileoSearches(j).ServiceID = ttServiceID
                                                    DoGalileoSearches(j).BeginSearch()

                                                Catch e As Exception
                                                    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                                End Try

                                            Case "sabre", "Sabre"

                                                Dim RQdocument As XmlDocument
                                                RQdocument = New XmlDocument
                                                RQdocument.LoadXml(lfstrRequest(j))

                                                Dim officeIDElement As XmlElement
                                                officeIDElement = RQdocument.DocumentElement

                                                Dim pseudoCityCode As String = Nothing

                                                Dim nodeOffice As XmlNode

                                                'If officeIDElement.FirstChild().LastChild().FirstChild.ChildNodes(officeIDCounter).Attributes().Count > 0 And officeIDElement.FirstChild().LastChild().FirstChild.ChildNodes(officeIDCounter).Attributes() IsNot Nothing Then

                                                '    For Each nodeOffice In officeIDElement.FirstChild().LastChild().FirstChild.ChildNodes(officeIDCounter).Attributes

                                                '        If nodeOffice IsNot Nothing Then

                                                '            If nodeOffice.LocalName = "PseudoCityCode" Then

                                                '                pseudoCityCode = nodeOffice.InnerText
                                                '                officeIDCounter += 1
                                                '            Else
                                                '                pseudoCityCode = pseudoCityCode = officeIDElement.FirstChild().LastChild().FirstChild.ChildNodes(0).Attributes(0).InnerText
                                                '            End If

                                                '        End If

                                                '    Next
                                                'Else
                                                '    pseudoCityCode = officeIDElement.FirstChild().LastChild().FirstChild.ChildNodes(0).Attributes(0).InnerText
                                                'End If

                                                'RQdocument.FirstChild().FirstChild().FirstChild().Attributes(0).InnerText = pseudoCityCode

                                                'lfstrRequest(j) = RQdocument.OuterXml

                                                'ttProviderSystems.AAAPCC = pseudoCityCode
                                                'ttProviderSystems.PCC = pseudoCityCode

                                                Try
                                                    sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC.ToUpper())
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
                                                        .Request = lfstrRequest(j)
                                                        .ProviderSystems = ttProviderSystems
                                                        '.Version = "v03"
                                                        .ttCities = ttCities
                                                    End With



                                                    DoSabreSearches(j) = New SearchSabre_v03(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oSabre)
                                                    DoSabreSearches(j).Request = lfstrRequest(j)
                                                    DoSabreSearches(j).ServiceID = ttServiceID
                                                    DoSabreSearches(j).BeginSearch()



                                                Catch e As Exception
                                                    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                                End Try

                                            Case "worldspan", "Worldspan"
                                                Try
                                                    sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC.ToUpper())
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
                                                        .Request = lfstrRequest(j)
                                                        .ProviderSystems = ttProviderSystems
                                                        '.Version = "v03"
                                                        .ttCities = ttCities
                                                    End With

                                                    DoWorldspanSearches(j) = New SearchWorldspan_v03(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oWorldspan)
                                                    DoWorldspanSearches(j).Request = lfstrRequest(j)
                                                    DoWorldspanSearches(j).ServiceID = ttServiceID
                                                    DoWorldspanSearches(j).BeginSearch()

                                                Catch e As Exception
                                                    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                                End Try

#Region "Future Integrations"
                                                'Case "travelfusion"
                                                '    Try
                                                '        oDoc = New XmlDocument
                                                '        oDoc.LoadXml(strRequest)
                                                '        oRoot = oDoc.DocumentElement
                                                '        If oRoot.SelectNodes("OriginDestinationInformation").Count > 2 Then
                                                '            Throw New Exception("Travel Fusion does not support multi destination searches")
                                                '        End If
                                                '        sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC.ToUpper())
                                                '        ttProviderSystems = Application.Get(sb.ToString())
                                                '        sb.Remove(0, sb.Length)
                                                '        If ttProviderSystems.System Is Nothing Then
                                                '            sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.")
                                                '            GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), .Providers(i).Name))
                                                '            sb.Remove(0, sb.Length)
                                                '            Exit Select
                                                '        End If
                                                '        If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                                '            ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                                '        End If
                                                '        Dim oTravelFusion As New cServiceTravelFusion
                                                '        AddHandler oTravelFusion.GotResponse, AddressOf GotResponse
                                                '        With oTravelFusion
                                                '            .ServiceID = ttServiceID
                                                '            .Request = lfstrRequest(j)
                                                '            .ttProviderSystems = ttProviderSystems
                                                '            .Version = ""
                                                '        End With
                                                '        DoTravelFusionSearches(j) = New SearchTravelFusion_v03(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oTravelFusion)
                                                '        DoTravelFusionSearches(j).Request = lfstrRequest(j)
                                                '        DoTravelFusionSearches(j).ServiceID = ttServiceID
                                                '        DoTravelFusionSearches(j).BeginSearch()
                                                '    Catch e As Exception
                                                '        GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                                '    End Try
                                                'Case "aircanada"
                                                '    Try
                                                '        'oDoc = New XmlDocument
                                                '        'oDoc.LoadXml(strRequest)
                                                '        'oRoot = oDoc.DocumentElement
                                                '        sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC.ToUpper())
                                                '        ttProviderSystems = Application.Get(sb.ToString())
                                                '        sb.Remove(0, sb.Length)
                                                '        If ttProviderSystems.System Is Nothing Then
                                                '            sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.")
                                                '            GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), .Providers(i).Name))
                                                '            sb.Remove(0, sb.Length)
                                                '            Exit Select
                                                '        End If

                                                '        If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                                '            ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                                '        End If

                                                '        Dim oAirCanada As New cServiceAirCanada
                                                '        AddHandler oAirCanada.GotResponse, AddressOf GotResponse

                                                '        With oAirCanada
                                                '            .ServiceID = ttServiceID
                                                '            .Request = lfstrRequest(j)
                                                '            .ttProviderSystems = ttProviderSystems
                                                '            .Version = ""
                                                '        End With

                                                '        DoAirCanadaSearches(j) = New SearchAirCanada_v03(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oAirCanada)
                                                '        DoAirCanadaSearches(j).Request = lfstrRequest(j)
                                                '        DoAirCanadaSearches(j).ServiceID = ttServiceID
                                                '        DoAirCanadaSearches(j).BeginSearch()

                                                '    Catch e As Exception
                                                '        GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                                '    End Try
                                                'Case "vuka"
                                                '    Try
                                                '        'oDoc = New XmlDocument
                                                '        'oDoc.LoadXml(strRequest)
                                                '        'oRoot = oDoc.DocumentElement

                                                '        sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC.ToUpper())
                                                '        ttProviderSystems = Application.Get(sb.ToString())
                                                '        sb.Remove(0, sb.Length)

                                                '        If ttProviderSystems.System Is Nothing Then
                                                '            sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.")
                                                '            GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), .Providers(i).Name))
                                                '            sb.Remove(0, sb.Length)
                                                '            Exit Select
                                                '        End If

                                                '        If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                                '            ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                                '        End If

                                                '        Dim oVuka As New cServiceVuka
                                                '        AddHandler oVuka.GotResponse, AddressOf GotResponse

                                                '        With oVuka
                                                '            .ServiceID = ttServiceID
                                                '            .Request = lfstrRequest(j)
                                                '            .ttProviderSystems = ttProviderSystems
                                                '            .Version = ""
                                                '        End With

                                                '        DoVukaSearches(j) = New SearchVuka_v03(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oVuka)
                                                '        DoVukaSearches(j).Request = lfstrRequest(j)
                                                '        DoVukaSearches(j).ServiceID = ttServiceID
                                                '        DoVukaSearches(j).BeginSearch()

                                                '    Catch e As Exception
                                                '        GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                                '    End Try
                                                'Case "pyton"
                                                '    Try
                                                '        'oDoc = New XmlDocument
                                                '        'oDoc.LoadXml(strRequest)
                                                '        'oRoot = oDoc.DocumentElement

                                                '        sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC.ToUpper())
                                                '        ttProviderSystems = Application.Get(sb.ToString())
                                                '        sb.Remove(0, sb.Length)

                                                '        If ttProviderSystems.System Is Nothing Then
                                                '            sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.")
                                                '            GotResponse(FormatErrorMessage(ttServiceID, sb.ToString(), .Providers(i).Name))
                                                '            sb.Remove(0, sb.Length)
                                                '            Exit Select
                                                '        End If

                                                '        If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                                '            ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                                '        End If

                                                '        Dim oPyton As New cServicePyton
                                                '        AddHandler oPyton.GotResponse, AddressOf GotResponse

                                                '        With oPyton
                                                '            .ServiceID = ttServiceID
                                                '            .Request = lfstrRequest(j)
                                                '            .ttProviderSystems = ttProviderSystems
                                                '            .Version = ""
                                                '        End With

                                                '        DoPytonSearches(j) = New SearchPyton_v03(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oPyton)
                                                '        DoPytonSearches(j).Request = lfstrRequest(j)
                                                '        DoPytonSearches(j).ServiceID = ttServiceID
                                                '        DoPytonSearches(j).BeginSearch()

                                                '    Catch e As Exception
                                                '        GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                                '    End Try
#End Region
                                            Case Else
                                                sb.Append("Provider ").Append(ttCredential.Providers(i).Name).Append(" Not Currently Supported.")
                                                Throw New Exception(sb.ToString())
                                                sb.Remove(0, sb.Length)
                                        End Select
                                        j += 1
                                    End If
                                End If
                                iOD += 1
                            Next
                        Next
                    Next

                End With

                StartCounter = Now

                'Do While mintProviders < ttCredential.Providers.Length
                Do While mintProviders < j
                    If CType(Now.Subtract(StartCounter).TotalSeconds, Integer) > CPrdTimeOut Then Exit Do
                    Threading.Thread.Sleep(10)
                Loop

                'If ttCredential.Providers.Length > 1 Then
                If j > 1 Then
                    strResponse = String.Concat("<SuperRS>", mstrResponse + strRequest, "</SuperRS>")
                    ' Aggregate
                    cAggregation.Aggregate(ttServiceID, gXslPath, "", strResponse)

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
                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.ToString(), "", ttProviderSystems.LogUUID)

                If strResponse.IndexOf("<SearchPromotionsResponse>") <> -1 Then
                    cAggregation.ProcessMarkup(gXslPath, "", strResponse)
                End If

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, "")
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                LogDeals(strRequest, strResponse)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsLowFarePlus_v03", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID)
            End Try

            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Low Fare Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmLowFarePlus(ByVal OTA_AirLowFareSearchPlusRQ As wmLowFarePlusIn_v03.OTA_AirLowFareSearchPlusRQ) As <XmlElementAttribute("OTA_AirLowFareSearchPlusRS")> wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS
            Dim oSerializer As XmlSerializer = New XmlSerializer(GetType(wmLowFarePlusIn_v03.OTA_AirLowFareSearchPlusRQ))

            Dim oWriter As IO.StringWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_AirLowFareSearchPlusRQ)
            Dim xmlMessage As String = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
            xmlMessage = xmlMessage.Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.LowFarePlus)

            Dim oLowFarePlusRS As wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS
            Dim oReader As IO.StringReader

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oLowFarePlusRS = CType(oSerializer.Deserialize(oReader), wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS)
            Catch ex As Exception
                xmlMessage = "<OTA_AirLowFareSearchPlusRS Version=""v03""><Errors><Error>" & ex.InnerException.ToString() & "</Error></Errors></OTA_AirLowFareSearchPlusRS>"
                oReader = New System.IO.StringReader(xmlMessage.Replace("&", "&amp;"))
                oLowFarePlusRS = CType(oSerializer.Deserialize(oReader), wmLowFarePlusOut.OTA_AirLowFareSearchPlusRS)
            End Try

            Return oLowFarePlusRS

        End Function

        <WebMethod(Description:="Process Low Fare Plus Xml Messages Request.")>
        Public Function wmLowFarePlusXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.LowFarePlus)
        End Function

#End Region

    End Class

#Region "Search AmadeusWS"
    Public Class SearchAmadeusWS_v03
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
            oAmadeusWS.SendAirRequest()
            oAmadeusWS = Nothing
        End Sub
    End Class
#End Region

#Region "Search Galileo"
    Public Class SearchGalileo_v03
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
    Public Class SearchSabre_v03
        Private Delegate Sub StartSearch_Delegate()
        Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearchWS)
        Private pcc As String = ""
        Private userid As String = ""
        Private System As String = ""
        Private ttProviderSystems As TripXMLProviderSystems
        Private oSabre As cServiceSabre

        Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oSabre As cServiceSabre)
            Me.pcc = _pcc
            Me.userid = _userid
            Me.System = _System
            Me.ttProviderSystems = _ttProviderSystems
            Me.oSabre = _oSabre
        End Sub
        Public Property ServiceID() As String = ""
        Public Property Request() As String = ""
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
    Public Class SearchWorldspan_v03
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

#Region "Search Pyton"
    'Public Class SearchPyton_v03
    '    Private Delegate Sub StartSearch_Delegate()
    '    Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoPytonSearch)
    '    Private pcc As String = ""
    '    Private userid As String = ""
    '    Private System As String = ""
    '    Private ttProviderSystems As TripXMLProviderSystems
    '    Private _ServiceID As String = ""
    '    Private _Request As String = ""
    '    Private oPyton As cServicePyton

    '    Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oPyton As cServicePyton)
    '        Me.pcc = _pcc
    '        Me.userid = _userid
    '        Me.System = _System
    '        Me.ttProviderSystems = _ttProviderSystems
    '        Me.oPyton = _oPyton
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
    '    Private Sub DoPytonSearch()
    '        ttProviderSystems.PCC = Me.pcc
    '        oPyton.SendAirRequest()
    '        oPyton = Nothing
    '    End Sub
    'End Class
#End Region

End Namespace

