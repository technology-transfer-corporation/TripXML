Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml.Serialization
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsSessionClose",
        Name:="wsSessionClose",
        Description:="A TripXML Web Service to Process Session Close Messages Request.")>
    Public Class wsSessionClose
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

        ' Not Implemented

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

                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, StartTime, ttServiceID, Server.MachineName, UUID)
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())
                ttProviderSystems.LogUUID = UUID

                Select Case ttCredential.Providers(0).Name

                    Case "AmadeusWS"

                        strResponse = SendOtherRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Apollo", "Galileo"

                        strResponse = SendOtherRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Sabre"

                        'ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                        'ttProviderSystems.LogUUID = UUID
                        sb.Remove(0, sb.Length())
                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                            sb.Remove(0, sb.Length())
                            Exit Select
                        End If

                        ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC

                        strResponse = SendOtherRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Worldspan"

                        If (strRequest.Contains("<ConversationID></ConversationID>") OrElse strRequest.Contains("<ConversationID>NONE</ConversationID>")) Then
                            CoreLib.SendTrace("", "wsSessionClose", "NO SessionID Passed", strRequest, String.Empty)
                            AddLog(LogType.Info, "NO SessionID Passed", New TripXMLProviderSystems(), strRequest)
                            Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append("No Session ID Provided.").ToString())
                        End If

                        strResponse = SendOtherRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                    Case "Travelport"

                        strResponse = SendOtherRequestTravelport(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                End Select

                ' DecodeSessionClose(strResponse) Not Implemented.

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttProviderSystems)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsSessionClose", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Session Close Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmSessionClose(ByVal SessionCloseRQ As wmSessionCloseIn.SessionCloseRQ) As <XmlElementAttribute("SessionCloseRS")> wmSessionCloseOut.SessionCloseRS
            Dim xmlMessage As String
            Dim oSessionCloseRS As wmSessionCloseOut.SessionCloseRS = Nothing
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            oSerializer = New XmlSerializer(GetType(wmSessionCloseIn.SessionCloseRQ))
            oWriter = New IO.StringWriter(New StringBuilder)
            oSerializer.Serialize(oWriter, SessionCloseRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CloseSession)

            Try
                oSerializer = New XmlSerializer(type:=GetType(wmSessionCloseOut.SessionCloseRS))
                oReader = New IO.StringReader(xmlMessage)
                oSessionCloseRS = CType(oSerializer.Deserialize(oReader), wmSessionCloseOut.SessionCloseRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsSessionClose", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oSessionCloseRS

        End Function

        <WebMethod(Description:="Process Session Close Xml Messages Request.")> _
        Public Function wmSessionCloseXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.CloseSession)
        End Function

#End Region

    End Class

End Namespace
