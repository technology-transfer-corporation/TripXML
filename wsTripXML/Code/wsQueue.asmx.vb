Imports System.Web.Services
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Web.Services.Protocols
Imports System.Text
Imports CompressionExtension

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement), _
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsQueue", _
        Name:="wsQueue", _
        Description:="A TripXML Web Service to Process Queue Messages Request.")> _
    Public Class wsQueue
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



#End Region

#Region " Process Service Request All GDS "
        Private sb As StringBuilder = New StringBuilder()

        Private Function ServiceRequest(ByVal strRequest As String, ByVal ttServiceID As Integer) As String
            Dim strResponse As String = ""
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim validateXSDOut As Boolean
            Dim startTime As Date
            Dim UUID As String = ""

            Try
                startTime = Now
                strRequest = strRequest.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
                strRequest = strRequest.Replace(" xmlns=""http://tripxml.downtowntravel.com/tripxml/wsQueue""", "")
                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, startTime, ttServiceID, Server.MachineName, UUID)
                validateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
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
                        'strResponse = SendPNRRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                        'Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                        'sb.Remove(0, sb.Length())

                    Case "amadeusws"

                        strResponse = SendPNRRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "apollo", "galileo"

                        strResponse = SendPNRRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "travelport"

                        strResponse = SendPNRRequestTravelPort(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "sabre"

                        'ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                        'sb.Remove(0, sb.Length())
                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                            sb.Remove(0, sb.Length())
                            Exit Select
                        End If

                        ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC

                        strResponse = SendPNRRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "worldspan"

                        strResponse = SendPNRRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                ' Not Implemented DecodeQueue

                PostServiceRequest(strResponse, validateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, startTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsQueue", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()> _
        <WebMethod(Description:="Process Queue Messages Request.")> _
        <System.Web.Services.Protocols.SoapHeader("tXML")> _
        Public Function wmQueue(ByVal OTA_QueueRQ As wmQueueIn.OTA_QueueRQ) As <XmlElementAttribute("OTA_QueueRS")> wmQueueOut.OTA_QueueRS
            Dim xmlMessage As String
            Dim oQueueRS As wmQueueOut.OTA_QueueRS = Nothing
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            Try
                oSerializer = New XmlSerializer(GetType(wmQueueIn.OTA_QueueRQ))
                oWriter = New IO.StringWriter(New StringBuilder)
                oSerializer.Serialize(oWriter, OTA_QueueRQ)
                xmlMessage = oWriter.ToString
                xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

                xmlMessage = ServiceRequest(xmlMessage, ttServices.Queue)

                If xmlMessage.IndexOf("Object reference not set to an instance of an object") <> -1 Then
                    'CoreLib.SendEmail("Queue message", "Object reference.")
                End If

                oSerializer = New XmlSerializer(Type:=GetType(wmQueueOut.OTA_QueueRS))
                oReader = New IO.StringReader(xmlMessage)
                oQueueRS = CType(oSerializer.Deserialize(oReader), wmQueueOut.OTA_QueueRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsQueue", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oQueueRS

        End Function

        <WebMethod(Description:="Process Queue well format Xml Messages Request.")> _
        Public Function wmQueueXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.Queue)
        End Function

#End Region

    End Class

End Namespace
