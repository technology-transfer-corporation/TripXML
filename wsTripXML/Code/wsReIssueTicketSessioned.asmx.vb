Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml.Serialization
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
            System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsReIssueTicketSessioned",
            Name:="wsReIssueTicketSessioned",
            Description:="A TripXML Web Service to Process ReIssue ticket with open session")>
    Public Class wsReIssueTicketSessioned
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

#Region " Process Service Request All GDS "
        Private sb As StringBuilder = New StringBuilder()

        Private Function ServiceRequest(ByVal strRequest As String, ByVal ttServiceID As Integer) As String
            Dim strResponse As String = ""
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim validateXSDOut As Boolean
            Dim startTime As Date
            Dim uuid As String = ""

            Try
                startTime = Now

                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, startTime, ttServiceID, Server.MachineName, uuid)
                validateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                Select Case ttCredential.Providers(0).Name.ToLower()
                    Case "amadeus"
                        strResponse = SendTravelRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "amadeusws"
                        strResponse = SendTravelRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

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

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                End Select

                PostServiceRequest(strResponse, validateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, startTime, ttServiceID, Server.MachineName, uuid)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsIssueTicket", "============= OTA Response ============= ", strResponse, uuid)
            End Try

            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Issue Ticket Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmReIssueTicketSessioned(ByVal TT_AutomaticUpdateRQ As wsReIssueTicket.TT_ReIssueTicketRQ) As <XmlElementAttribute("TT_ReIssueTicketRS")> wsReIssueTicketOut.TT_ReIssueTicketRS
            Dim oReIssueTicketRS As wsReIssueTicketOut.TT_ReIssueTicketRS = Nothing
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            oSerializer = New XmlSerializer(GetType(wsReIssueTicket.TT_ReIssueTicketRQ))
            oWriter = New IO.StringWriter(New StringBuilder)
            oSerializer.Serialize(oWriter, TT_AutomaticUpdateRQ)
            Dim xmlMessage As String = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""", "")
            xmlMessage = xmlMessage.Replace(" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.ReissueTicket)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wsReIssueTicketOut.TT_ReIssueTicketRS))
                oReader = New IO.StringReader(xmlMessage)
                oReIssueTicketRS = CType(oSerializer.Deserialize(oReader), wsReIssueTicketOut.TT_ReIssueTicketRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsIssueTicketSessioned", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oReIssueTicketRS

        End Function

        <WebMethod(Description:="Process Issue Ticket Xml Messages Request.")>
        Public Function wmReIssueTicketSessionedXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.ReissueTicket)
        End Function

#End Region

    End Class

End Namespace