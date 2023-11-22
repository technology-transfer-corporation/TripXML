Imports System.Web.Services
Imports System.IO
Imports TripXMLMain
Imports System.Xml.Serialization
Imports TripXMLMain.modCore
Imports System.Web.Configuration

Namespace wsTravelTalk

    <Protocols.SoapDocumentService(RoutingStyle:=Protocols.SoapServiceRoutingStyle.RequestElement),
        WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsPNRReprice",
        Name:="wsPNRReprice",
        Description:="A TripXML Web Service to Process PNR Reprice Request.")>
    Public Class wsPNRReprice
        Inherits WebService
        Public TXML As TripXML

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

#Region " Process Service Request All GDS "
        Private ReadOnly sb As StringBuilder = New StringBuilder()

        Private Function StoredFareServiceRequest(ByVal request As String, ByVal ttServiceID As Integer, Optional ByVal sessionID As String = "") As String
            Dim response As String = ""
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim validateXSDOut As Boolean
            Dim startTime As Date
            Dim UUID As String = ""

            Try
                startTime = Now

                PreServiceRequest(request, Application, ttCredential, ttProviderSystems, startTime, ttServiceID, Server.MachineName, UUID)
                validateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                Select Case ttCredential.Providers(0).Name
                    Case "AmadeusWS"
                        response = SendPNRRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, request)
                    Case "Sabre"
                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                            sb.Remove(0, sb.Length())
                            Exit Select
                        End If

                        ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC
                        response = SendPNRRequestSabre(ttServiceID, ttCredential, ttProviderSystems, request)
                    Case "Worldspan"
                        If CBool(WebConfigurationManager.AppSettings("IsTravelportWorldspan")) Then
                            Dim ttDefProvider As New TripXMLProviderSystems()
                            If Not String.IsNullOrEmpty(sessionID) Then
                                request = request.Replace(sessionID, "")
                            End If
                            PreServiceRequest(request, Application, ttCredential, ttDefProvider, startTime, ttServiceID, Server.MachineName, UUID, "", True)
                            response = SendPNRRequestTravelPort(ttServiceID, ttCredential, ttDefProvider, request)
                            response = response.Replace("</OTA_PNRRepriceRS>", $"<ConversationID>{sessionID}</ConversationID></OTA_PNRRepriceRS>")
                        Else
                            response = SendPNRRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, request)
                        End If
                    Case "Galileo"
                        If CBool(WebConfigurationManager.AppSettings("IsTravelportReprice")) Then
                            Dim ttDefProvider As New TripXMLProviderSystems()
                            If Not String.IsNullOrEmpty(sessionID) Then
                                request = request.Replace(sessionID, "")
                            End If

                            Dim aaapcc As String = ttDefProvider.AAAPCC
                            Dim _pcc As String = ttCredential.Providers(0).PCC

                            ttDefProvider.AAAPCC = ttCredential.Providers(0).PCC
                            ttCredential.Providers(0).PCC = "3M2Y"
                            PreServiceRequest(request, Application, ttCredential, ttDefProvider, startTime, ttServiceID, Server.MachineName, UUID, "", True)
                            response = SendPNRRequestTravelPort(ttServiceID, ttCredential, ttDefProvider, request)
                            response = response.Replace("</OTA_PNRRepriceRS>", $"<ConversationID>{sessionID}</ConversationID></OTA_PNRRepriceRS>")

                            'ttDefProvider.AAAPCC = aaapcc
                            ttCredential.Providers(0).PCC = _pcc
                        Else
                            response = SendPNRRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, request)
                        End If
                    Case "Travelport"
                        response = SendPNRRequestTravelPort(ttServiceID, ttCredential, ttProviderSystems, request)
                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                End Select

                PostServiceRequest(response, validateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                response = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(response, ttCredential, startTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsPNRReprice", "============= OTA Response ============= ", response, UUID)
            End Try

            Return response
        End Function

        Private Function ServiceRequest(ByVal request As String, ByVal ttServiceID As Integer) As String
            Dim response As String = ""
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim validateXSDOut As Boolean
            Dim startTime As Date
            Dim UUID As String = ""

            Try
                startTime = Now

                PreServiceRequest(request, Application, ttCredential, ttProviderSystems, startTime, ttServiceID, Server.MachineName, UUID)
                validateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                Select Case ttCredential.Providers(0).Name
                    Case "AmadeusWS"
                        response = SendPNRRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, request)
                    Case "Sabre"

                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                            sb.Remove(0, sb.Length())
                            Exit Select
                        End If

                        ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC
                        response = SendPNRRequestSabre(ttServiceID, ttCredential, ttProviderSystems, request)
                    Case "Worldspan"
                        response = SendPNRRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, request)
                    Case "Galileo"
                        response = SendPNRRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, request)
                    Case "Travelport"
                        response = SendPNRRequestTravelPort(ttServiceID, ttCredential, ttProviderSystems, request)
                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                End Select

                PostServiceRequest(response, validateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                response = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(response, ttCredential, startTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsPNRReprice", "============= OTA Response ============= ", response, UUID)
            End Try

            Return response
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process PNR Reprice Messages Request.")>
        <Protocols.SoapHeader("TXML")>
        Public Function wmPNRReprice(ByVal OTA_PNRRepriceRQ As wmPNRRepriceIn.OTA_PNRRepriceRQ) As <XmlElementAttribute("OTA_PNRRepriceRS")> wmPNRRepriceOut.OTA_PNRRepriceRS
            Dim oPNRRepriceRS As wmPNRRepriceOut.OTA_PNRRepriceRS = Nothing

            Dim oSerializer As XmlSerializer = New XmlSerializer(GetType(wmPNRRepriceIn.OTA_PNRRepriceRQ))
            Dim oWriter As StringWriter = New StringWriter(New StringBuilder)
            oSerializer.Serialize(oWriter, OTA_PNRRepriceRQ)
            Dim xmlMessage As String = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
            Dim sid As String = OTA_PNRRepriceRQ.ConversationID

            'If OTA_PNRRepriceRQ.StoreFare Then
            xmlMessage = StoredFareServiceRequest(xmlMessage, ttServices.PNRReprice, sid)
            'Else
            '    xmlMessage = ServiceRequest(xmlMessage, ttServices.PNRReprice)
            'End If

            Try
                oSerializer = New XmlSerializer(type:=GetType(wmPNRRepriceOut.OTA_PNRRepriceRS))
                Dim oReader As StringReader = New StringReader(xmlMessage)
                oPNRRepriceRS = CType(oSerializer.Deserialize(oReader), wmPNRRepriceOut.OTA_PNRRepriceRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsPNRReprice", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oPNRRepriceRS

        End Function

        <WebMethod(Description:="Process PNR Reprice Xml Messages Request.")>
        Public Function wmPNRRepriceXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.PNRReprice)
        End Function

#End Region

    End Class

End Namespace
