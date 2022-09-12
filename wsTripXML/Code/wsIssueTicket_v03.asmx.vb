Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml.Serialization
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsIssueTicket",
        Name:="wsIssueTicket_v03",
        Description:="A TripXML Web Service to Process Issue Ticket Messages Request - version 03.")>
    Public Class wsIssueTicket_v03
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
            Dim ValidateXSDOut As Boolean
            Dim StartTime As Date
            Dim UUID As String = ""

            Try
                StartTime = Now

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

                        'strResponse = SendTravelRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest, "v03")
                        'Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                        'sb.Remove(0, sb.Length())

                    Case "Apollo", "Galileo"

                        strResponse = SendTravelRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                'Dim StartCounter As Date
                'StartCounter = Now
                ' Not Implemented DecodeIssueTicket(strResponse, ttCredential.UserID)
                'CoreLib.SendTrace(ttCredential.UserID, "Performance", "Decoding = ").Append(CType(Now.Subtract(StartCounter).TotalMilliseconds, Integer), "")

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsIssueTicket", "============= OTA Response ============= ", strResponse, UUID)
            End Try

            Return strResponse
            sb = Nothing
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Issue Ticket Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmIssueTicket(ByVal TT_IssueTicketRQ As wmIssueTicketIn_v03.TT_IssueTicketRQ) As <XmlElementAttribute("TT_IssueTicketRS")> wmIssueTicketOut.TT_IssueTicketRS
            Dim xmlMessage As String = ""
            Dim oIssueTicketRS As wmIssueTicketOut.TT_IssueTicketRS = Nothing
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            oSerializer = New XmlSerializer(GetType(wmIssueTicketIn_v03.TT_IssueTicketRQ))
            oWriter = New IO.StringWriter(New StringBuilder)
            oSerializer.Serialize(oWriter, TT_IssueTicketRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.IssueTicket)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmIssueTicketOut.TT_IssueTicketRS))
                oReader = New IO.StringReader(xmlMessage)
                oIssueTicketRS = CType(oSerializer.Deserialize(oReader), wmIssueTicketOut.TT_IssueTicketRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsIssueTicket_v03", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oIssueTicketRS

        End Function

        <WebMethod(Description:="Process Issue Ticket Xml Messages Request.")> _
        Public Function wmIssueTicketXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.IssueTicket)
        End Function

#End Region

    End Class

End Namespace
