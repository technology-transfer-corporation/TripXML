Imports System.Web.Services
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Text
Imports CompressionExtension

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement), _
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsQueueRead", _
        Name:="wsQueueRead_v04", _
        Description:="A TripXML Web Service to Process QueueRead Messages Request version v04.")> _
    Public Class wsQueueRead_v04
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

#Region " Decode Functions "

        Private Function DecodeQueueRead(ByVal strResponse As String, ByVal UserID As String) As String
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

                For Each oNode In oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air")
                    Try
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
                                oNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                            End If
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
                        CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", e.Message, String.Empty)
                    End Try

                Next

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", ex.Message, String.Empty)
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

                strRequest = strRequest.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
                strRequest = strRequest.Replace(" xmlns=""http://tripxml.downtowntravel.com/tripxml/wsQueueRead""", "")
                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, StartTime, ttServiceID, Server.MachineName, UUID)
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                Select Case ttCredential.Providers(0).Name.ToLower
                    Case "amadeus"
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
                        'strResponse = SendPNRRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest, "v04")
                        'Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                        'sb.Remove(0, sb.Length())

                    Case "amadeusws"

                        strResponse = SendPNRRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v04")

                    Case "apollo", "galileo"

                        strResponse = SendPNRRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v04")

                    Case "sabre"

                        'ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                        'sb.Remove(0, sb.Length())
                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                            sb.Remove(0, sb.Length())
                            Exit Select
                        End If

                        ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC

                        strResponse = SendPNRRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v04")

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                strResponse = DecodeQueueRead(strResponse, ttCredential.UserID)

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsQueueRead", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()> _
        <WebMethod(Description:="Process QueueRead Messages Request.")> _
        <System.Web.Services.Protocols.SoapHeader("tXML")> _
        Public Function wmQueueRead(ByVal OTA_QueueReadRQ As wmQueueReadIn.OTA_QueueReadRQ) As <XmlElementAttribute("OTA_TravelItineraryRS")> wmTravelItineraryOut_v04.OTA_TravelItineraryRS
            Dim xmlMessage As String = ""
            Dim oQueueReadRS As wmTravelItineraryOut_v04.OTA_TravelItineraryRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmQueueReadIn.OTA_QueueReadRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_QueueReadRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.QueueRead)

            If xmlMessage.IndexOf("Object reference not set to an instance of an object") <> -1 Then
                'CoreLib.SendEmail("QueueRead message", "Object reference.")
            End If

            Try
                oSerializer = New XmlSerializer(Type:=GetType(wmTravelItineraryOut_v04.OTA_TravelItineraryRS))
                oReader = New IO.StringReader(xmlMessage)
                oQueueReadRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut_v04.OTA_TravelItineraryRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsQueueRead", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oQueueReadRS

        End Function

        <WebMethod(Description:="Process QueueRead well format Xml Messages Request.")> _
        Public Function wmQueueReadXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.QueueRead)
        End Function

#End Region

    End Class
End Namespace
