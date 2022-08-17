Imports System
Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Text
Imports TripXMLMain.modCore
Imports System.Globalization

Namespace wsTravelTalk

    <Protocols.SoapDocumentService(RoutingStyle:=Protocols.SoapServiceRoutingStyle.RequestElement), WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsUpdateSessioned", _
        Name:="wsUpdateSessioned", _
        Description:="A TripXML Web Service to Process UpdateSessioned Messages Request.")> _
    Public Class wsUpdateSessioned
        Inherits WebService

#Region " Web Services Designer Generated Code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Web Services Designer.
            InitializeComponent()

            'Add your own initialization code after the InitializeComponent() call

        End Sub

        'Required by the Web Services Designer
        Private components As ComponentModel.IContainer

        'NOTE: The following procedure is required by the Web Services Designer
        'It can be modified using the Web Services Designer.  
        'Do not modify it using the code editor.
        <DebuggerStepThrough()> Private Sub InitializeComponent()
            components = New ComponentModel.Container
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

#Region " Decode Function "

        Private Function DecodePNRRead(ByVal strResponse As String, ByVal UserID As String, ByVal UUID As String) As String
            Dim oDoc As XmlDocument
            Dim oRoot As XmlElement
            Dim ttAirports As DataView
            Dim ttAirlines As DataView
            Dim ttAirlinesNames As DataView
            Dim ttEquipments As DataView
            Dim oNode As XmlNode

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strResponse)
                oRoot = oDoc.DocumentElement

                ttAirports = CType(Application.Get("ttAirports"), DataView)
                ttAirlines = CType(Application.Get("ttAirlines"), DataView)
                'ttAirlines.Table.PrimaryKey = New DataColumn() { ttAirlines.Table.Columns("Code") } 

                ttEquipments = CType(Application.Get("ttEquipments"), DataView)
                ttAirlinesNames = CType(Application.Get("ttAirlinesNames"), DataView)
                'ttAirlinesNames.Table.PrimaryKey = New DataColumn() { ttAirlinesNames.Table.Columns("Code") } 

                Dim testNode As XmlNode = oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air")

                If (testNode Is Nothing) Then
                    CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** No Air Segments in PNR", "", UUID)
                Else

                    For Each oNode In oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air")
                        Try
                            Dim arnkElem As XmlNode = oNode.SelectSingleNode("TPA_Extensions/Arnk")
                            If Not arnkElem Is Nothing Then
                                Continue For
                            End If
                            ' *******************
                            ' Decode Airports   *
                            ' *******************
                            If Not oNode.SelectSingleNode("DepartureAirport") Is Nothing Then
                                oNode.SelectSingleNode("DepartureAirport").InnerText = GetDecodeValue(ttAirports, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                            End If
                            If Not oNode.SelectSingleNode("ArrivalAirport") Is Nothing Then
                                oNode.SelectSingleNode("ArrivalAirport").InnerText = GetDecodeValue(ttAirports, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
                            End If

                            ' *******************
                            ' Decode Airlines   *
                            ' *******************
                            If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                                If Not oNode.SelectSingleNode("OperatingAirline").Attributes("Code") Is Nothing Then
                                    If oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value <> "" Then
                                        oNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                                    ElseIf Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                                        Dim attCode As XmlAttribute
                                        attCode = oDoc.CreateAttribute("Code")
                                        attCode.Value = GetEncodeValue(ttAirlinesNames, oNode.SelectSingleNode("OperatingAirline").InnerText)
                                        oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)

                                        oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                                    End If
                                Else
                                    If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                                        Dim attCode As XmlAttribute
                                        attCode = oDoc.CreateAttribute("Code")
                                        attCode.Value = GetEncodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").InnerText)

                                        If Not String.IsNullOrEmpty(attCode.Value) Then
                                            oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)
                                            oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                                        End If
                                    End If
                                End If
                            ElseIf Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                                Dim attCode As XmlAttribute
                                attCode = oDoc.CreateAttribute("Code")
                                attCode.Value = GetEncodeValue(ttAirlinesNames, oNode.SelectSingleNode("OperatingAirline").InnerText)
                                oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)

                                oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                            End If

                            If Not oNode.SelectSingleNode("MarketingAirline") Is Nothing Then
                                oNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttAirlines, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                            End If

                            ' *******************
                            ' Decode Equipments   *
                            ' *******************
                            If Not oNode.SelectSingleNode("Equipment") Is Nothing Then
                                If Not oNode.SelectSingleNode("Equipment").Attributes("AirEquipType") Is Nothing Then
                                    oNode.SelectSingleNode("Equipment").InnerText = GetDecodeValue(ttEquipments, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                                End If
                            End If
                        Catch e As Exception
                            CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", e.Message, UUID)
                        End Try

                    Next
                End If

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", ex.Message, UUID)
            End Try
            Return strResponse
        End Function

#End Region

        Private sb As StringBuilder = New StringBuilder()

        Private mstrResponse As String = ""
        Private mintProviders As Integer = 0
        Private _recordLocator As String = ""
        'Private ttAPIAdapter As AmadeusAPIAdapter

        Private Sub GotResponse(ByVal Response As String)
            mstrResponse &= Response
            mintProviders += 1
        End Sub

        Public tXML As TripXML

#Region " Process Service Request All Suppliers "
        Private Function ServiceRequest(ByVal strRequest As String, ByVal ttServiceID As Integer) As String
            Dim strResponse As String = ""
            'Dim ttAA As AmadeusAdapter = Nothing
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim validateXSDOut As Boolean
            Dim startTime As Date
            Dim uuID As String = ""
            'Dim conversationID As String = ""

            Try
                startTime = Now

                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, startTime, ttServiceID, Server.MachineName, uuID)
                validateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                With ttCredential
                    Select Case .Providers(0).Name.ToLower
                        Case "amadeus"
                            'ttAPIAdapter = Application.Get(sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(0).PCC).ToString())
                            'sb.Remove(0, sb.Length())
                            'If ttAPIAdapter Is Nothing Then
                            '    Throw New Exception(sb.Append("Access denied to Amadeus - ").Append(.System).Append(" system. Or invalid provider.").ToString())
                            '    sb.Remove(0, sb.Length())
                            'End If

                            'If ttCredential.Providers(0).PCC.Trim.Length > 0 Then
                            '    ttAPIAdapter.SourcePCC = ttCredential.Providers(0).PCC
                            'End If

                            ''********************************
                            ''* Send  PNR Modify Request     * 
                            ''********************************
                            'strResponse = SendTravelRequestAmadeus(ttServiceID, ttCredential, ttAPIAdapter, strRequest)
                            'Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAPIAdapter)
                            'sb.Remove(0, sb.Length())

                        Case "amadeusws"

                            strResponse = SendTravelRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        Case "apollo", "galileo"
                            'ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                            sb.Remove(0, sb.Length())
                            If ttProviderSystems.System Is Nothing Then
                                FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                                sb.Remove(0, sb.Length())
                                Exit Select
                            End If

                            strResponse = SendTravelRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        Case "sabre"

                            'ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                            'sb.Remove(0, sb.Length())
                            If ttProviderSystems.System Is Nothing Then
                                FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                                sb.Remove(0, sb.Length())
                                Exit Select
                            End If

                            ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC

                            strResponse = SendTravelRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        Case "worldspan"
                            strResponse = SendTravelRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                            strResponse = DecodePNRRead(strResponse, ttCredential.UserID, uuID)
                        Case Else
                            GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Provider ").Append(.Providers(0).Name).Append(" Not Currently Supported.").ToString(), .Providers(0).Name))
                            sb.Remove(0, sb.Length())
                    End Select

                End With

                ' DecodeUpdateSessioned(strResponse) Not Implemented.

                PostServiceRequest(strResponse, validateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, startTime, ttServiceID, Server.MachineName, uuID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsUpdateSessioned", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()> _
        <WebMethod(Description:="Process UpdateSessioned Info Messages Request.")>
        <Protocols.SoapHeader("tXML")>
        Public Function wmUpdateSessioned(ByVal OTA_UpdateSessionedRQ As wmUpdateSessionedIn.OTA_UpdateSessionedRQ) As <XmlElementAttribute("OTA_TravelItineraryRS")> wmTravelItineraryOut_v03.OTA_TravelItineraryRS
            Dim xmlMessage As String = String.Empty
            Dim otaUpdateSessionedRS As wmTravelItineraryOut_v03.OTA_TravelItineraryRS
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            Try
                oSerializer = New XmlSerializer(GetType(wmUpdateSessionedIn.OTA_UpdateSessionedRQ))
                oWriter = New IO.StringWriter(New StringBuilder)

                '*************************************
                '* Get PNR Modify XML Request Msg    * 
                '*************************************
                oSerializer.Serialize(oWriter, OTA_UpdateSessionedRQ)
                xmlMessage = oWriter.ToString
                xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
                xmlMessage = xmlMessage.Replace(" xmlns=""http://tripxml.downtowntravel.com/tripxml/wsUpdateSessioned""", "")
                xmlMessage = xmlMessage.Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "")
                xmlMessage = xmlMessage.Replace(vbCrLf, "")
                xmlMessage = xmlMessage.Replace("""", "'")

                xmlMessage = ServiceRequest(xmlMessage, ttServices.UpdateSessioned)

                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmTravelItineraryOut_v03.OTA_TravelItineraryRS))
                oReader = New IO.StringReader(xmlMessage)
                otaUpdateSessionedRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut_v03.OTA_TravelItineraryRS)
            Catch ex As Exception

                Dim oDoc As XmlDocument
                Dim oRoot As XmlElement
                oDoc = New XmlDocument()
                oDoc.LoadXml(xmlMessage)
                oRoot = oDoc.DocumentElement
                Dim sessionID = ""
                If Not oRoot.SelectSingleNode("ConversationID") Is Nothing Then
                    sessionID = oRoot.SelectSingleNode("ConversationID").OuterXml.Replace("&amp;", "&")
                End If

                Dim itinRefXmlList As String = String.Empty
                Dim custInfoXmlList As String = String.Empty
                Dim tpaInfoXmlList As String = String.Empty
                If oRoot.SelectSingleNode("TravelItinerary") Is Nothing Then
                    itinRefXmlList = oRoot.SelectSingleNode("TravelItinerary/ItineraryRef").OuterXml
                    custInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/CustomerInfos").OuterXml
                    tpaInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/TPA_Extensions").OuterXml
                End If

                Dim errMessage = $"<Errors><Error>{ex.InnerException?.Message}</Error><Error>{ex.Message}</Error></Errors>"

                xmlMessage = $"<OTA_TravelItineraryRS Version=""v03"" xmlns:stl=""http://services.sabre.com/STL/v01"">{errMessage}<TravelItinerary>{itinRefXmlList}{custInfoXmlList}<ItineraryInfo></ItineraryInfo>{tpaInfoXmlList}</TravelItinerary>{sessionID}</OTA_TravelItineraryRS>"

                oReader = New IO.StringReader(xmlMessage)
                otaUpdateSessionedRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut_v03.OTA_TravelItineraryRS)

            End Try

            Return otaUpdateSessionedRS

        End Function

        <WebMethod(Description:="Process UpdateSessioned Info Xml Messages Request.")> _
        Public Function wmUpdateSessionedXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.UpdateSessioned)
        End Function

#End Region

    End Class

End Namespace


