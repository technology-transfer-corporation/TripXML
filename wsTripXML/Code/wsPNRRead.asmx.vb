Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain
Imports System.Xml.Serialization
Imports TripXMLMain.modCore
Imports TripXMLTools.TripXMLLoad

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsPNRRead",
        Name:="wsPNRRead",
        Description:="A TripXML Web Service to Process PNR Read Request.")>
    Public Class wsPNRRead
        Inherits System.Web.Services.WebService
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

#Region " Decode Function "

        Private Function DecodePNRRead(ByVal strResponse As String, ByVal UserID As String, ByRef strUUID As String) As String
            Dim oDoc As XmlDocument
            Dim oRoot As XmlElement
            Dim ttAirports As DataView
            Dim ttAirlines As DataView
            Dim ttEquipments As DataView
            Dim oNode As XmlNode

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strResponse)
                oRoot = oDoc.DocumentElement

                ttAirports = CType(Application.Get("ttAirports"), DataView)
                ttAirlines = CType(Application.Get("ttAirlines"), DataView)
                ttEquipments = CType(Application.Get("ttEquipments"), DataView)

                Dim nsmgr As New XmlNamespaceManager(oDoc.NameTable)
                Dim testNode As XmlNode = oDoc.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/Item", nsmgr)

                If (testNode("Air") Is Nothing) Then
                    CoreLib.SendTrace(UserID, "wsPNRRead", "Error * No Air Segments in PNR", "", strUUID)
                Else

                    For Each oNode In oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air")
                        Try
                            If oNode IsNot Nothing Then
                                ' *******************
                                ' Decode Airports   *
                                ' *******************
                                If Not oNode.SelectSingleNode("DepartureAirport") Is Nothing Then
                                    oNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                                End If
                                If Not oNode.SelectSingleNode("ArrivalAirport") Is Nothing Then
                                    oNode.SelectSingleNode("ArrivalAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
                                End If

                                ' *******************
                                ' Decode Airlines   *
                                ' *******************
                                If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing And Not oNode.SelectSingleNode("OperatingAirline").Attributes("Code") Is Nothing Then
                                    If oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value <> "" Then
                                        oNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                                    End If
                                End If

                                If Not oNode.SelectSingleNode("MarketingAirline") Is Nothing Then
                                    oNode.SelectSingleNode("MarketingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                                End If

                                ' *******************
                                ' Decode Equipments   *
                                ' *******************
                                If Not oNode.SelectSingleNode("Equipment") Is Nothing Then
                                    If Not oNode.SelectSingleNode("Equipment").Attributes("AirEquipType") Is Nothing Then
                                        oNode.SelectSingleNode("Equipment").InnerText = DecodeValue(DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                                    End If
                                End If
                            End If
                        Catch e As Exception
                            CoreLib.SendTrace(UserID, "wsPNRRead", "Error ** Decoding AirAvail Response", e.Message, strUUID)
                        End Try

                    Next
                End If

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", ex.Message, strUUID)
            End Try
            Return strResponse
        End Function

#End Region

#Region " Process Service Request All GDS "
        Private sb As StringBuilder = New StringBuilder()

        Private Function ServiceRequest(ByVal strRequest As String, ByVal ttServiceID As Integer) As String
            Dim strResponse As String = ""
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim ValidateXSDOut As Boolean
            Dim StartTime As Date
            Dim UUID As String = ""

            Try
                StartTime = Now
                strRequest = strRequest.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://tripxml.downtowntravel.com/tripxml/wsQueue""", "")
                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, StartTime, ttServiceID, Server.MachineName, UUID)
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                Select Case ttCredential.Providers(0).Name
                    Case "Amadeus"
                        'Dim ttAA As AmadeusAPIAdapter

                        'ttAA = Application.Get(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                        'sb.Remove(0, sb.Length())
                        'If ttAA Is Nothing Then
                        '    Throw New Exception(sb.Append("Access denied to Amadeus - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString())
                        '    sb.Remove(0, sb.Length())
                        'End If

                        'If ttCredential.Providers(0).PCC.Trim.Length > 0 Then
                        '    ttAA.SourcePCC = ttCredential.Providers(0).PCC
                        'End If

                        'strResponse = SendPNRRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                        'Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                        'sb.Remove(0, sb.Length())

                    Case "AmadeusWS"

                        strResponse = SendPNRRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Apollo", "Galileo"

                        strResponse = SendPNRRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Sabre"

                        'ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                        'sb.Remove(0, sb.Length())
                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                            sb.Remove(0, sb.Length())
                            Exit Select
                        End If

                        ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC

                        strResponse = SendPNRRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Worldspan"

                        strResponse = SendPNRRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                End Select

                strResponse = DecodePNRRead(strResponse, ttCredential.UserID, UUID)

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsPNRRead", "============= OTA Response ============= ", strResponse, UUID)
            End Try

            Return strResponse
            sb = Nothing
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process PNR Read Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmPNRRead(ByVal OTA_ReadRQ As wmPNRReadIn.OTA_ReadRQ) As <XmlElementAttribute("OTA_TravelItineraryRS")> wmTravelItineraryOut.OTA_TravelItineraryRS
            Dim xmlMessage As String = ""
            Dim oPNRReadRS As wmTravelItineraryOut.OTA_TravelItineraryRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmPNRReadIn.OTA_ReadRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_ReadRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.PNRRead)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmTravelItineraryOut.OTA_TravelItineraryRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oPNRReadRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut.OTA_TravelItineraryRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsPNRRead", "Error Deserialing OTA Response", ex.InnerException.ToString(), String.Empty)
                xmlMessage = "<OTA_TravelItineraryRS Version=""2.000""><Errors><Error>" & ex.InnerException.ToString() & "</Error></Errors></OTA_TravelItineraryRS>"
                oReader = New System.IO.StringReader(xmlMessage.Replace("&", "&amp;"))
                oPNRReadRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut.OTA_TravelItineraryRS)
            End Try

            Return oPNRReadRS

        End Function

        <WebMethod(Description:="Process PNR Read Xml Messages Request.")> _
        Public Function wmPNRReadXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.PNRRead)
        End Function

#End Region

    End Class

End Namespace
