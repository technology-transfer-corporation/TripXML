Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml
Imports System.Xml.Serialization
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsPing",
        Name:="wsPing",
        Description:="A TripXML Web Service to Process Display Ticket Messages Request.")>
    Public Class wsPing
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

                Select Case ttCredential.Providers(0).Name.ToLower()

                    Case "amadeusws"

                        Dim oDoc As XmlDocument = Nothing
                        Dim oRoot As XmlElement = Nothing
                        Dim oNode As XmlNode = Nothing
                        Dim dTime As Double
                        Dim iWait As Integer = 10

                        oDoc = New XmlDocument
                        oDoc.LoadXml(strRequest)
                        oRoot = oDoc.DocumentElement

                        If Not oRoot.SelectSingleNode("WaitTime") Is Nothing Then

                            iWait = oRoot.SelectSingleNode("WaitTime").InnerText

                        End If

                        dTime = Microsoft.VisualBasic.Timer
                        Do Until Microsoft.VisualBasic.Timer - dTime > iWait
                        Loop

                        strResponse = "<TXML_PingRS><Success/></TXML_PingRS>"

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select


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
        <WebMethod(Description:="Process Void Ticket Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmPing(ByVal TXML_PingRQ As wmPingIn.TXML_PingRQ) As <XmlElementAttribute("TXML_PingRS")> wmPingOut.TXML_PingRS
            Dim xmlMessage As String = ""
            Dim oPingRS As wmPingOut.TXML_PingRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmPingIn.TXML_PingRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, TXML_PingRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.Ping)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmPingOut.TXML_PingRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oPingRS = CType(oSerializer.Deserialize(oReader), wmPingOut.TXML_PingRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsPing", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oPingRS

        End Function

        <WebMethod(Description:="Process Void Ticket Xml Messages Request.")> _
        Public Function wmPingXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.TicketDisplay)
        End Function

#End Region

        <WebMethod(Description:="Ping get")> _
        Public Function ping(Delay As Integer) As String
            Dim dTime As Double
            Dim iWait As Integer = 10

            If Delay <> 0 Then

                iWait = Delay

            End If

            dTime = Microsoft.VisualBasic.Timer
            Do Until Microsoft.VisualBasic.Timer - dTime > iWait
            Loop

            Return "Waited " & iWait.ToString() & " seconds."
        End Function

    End Class

    'Public Class _Default
    '    Inherits Page
    '    Protected Sub Page_Load(sender As Object, e As EventArgs)
    '        Dim v As String = Request.QueryString("Delay")
    '        If v IsNot Nothing Then
    '            Response.Write("param is ")
    '            Response.Write(v)
    '        End If
    '    End Sub
    'End Class


End Namespace
