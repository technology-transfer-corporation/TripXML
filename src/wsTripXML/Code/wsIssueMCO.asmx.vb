Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Web.Services.Protocols
Imports CompressionExtension
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsIssueMCO",
        Name:="wsIssueMCO",
        Description:="A TripXML Web Service to Process Issue MCO Messages Request.")>
    Public Class wsIssueMCO
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

        Private Function ServiceRequest(ByVal strRequest As String, ByVal ttServiceID As Integer) As String
            Dim strResponse As String = ""
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim ValidateXSDOut As Boolean
            Dim StartTime As Date = Now
            Dim UUID As String = ""

            Try
                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, StartTime, ttServiceID, Server.MachineName, UUID)

                Select Case ttCredential.Providers(0).Name.ToLower()
                    Case "apollo", "galileo"
                        strResponse = SendTravelRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                    Case "sabre"
                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, $"Access denied to {ttCredential.Providers(0).Name} - {ttCredential.System} system. Or invalid provider.", ttCredential.Providers(0).Name)
                            Exit Select
                        End If
                        ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC
                        strResponse = SendTravelRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                    Case Else
                        Throw New Exception($"Provider {ttCredential.Providers(0).Name} Not Currently Supported.")
                End Select

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsIssueTicket", "============= OTA Response ============= ", strResponse, UUID)
            End Try

            Return strResponse
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension()>
        <WebMethod(Description:="Process Issue MCO Messages Request.")>
        <SoapHeader("tXML")>
        Public Function wmIssueMCO(ByVal TT_IssueMCORQ As wmIssueMCOIn.TT_IssueMCORQ) As <XmlElementAttribute("TT_IssueMCORS")> wmIssueMCOOut.TT_IssueMCORS
            Dim xmlMessage As String
            Dim oIssueMCORS As wmIssueMCOOut.TT_IssueMCORS = Nothing
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            oSerializer = New XmlSerializer(GetType(wmIssueMCOIn.TT_IssueMCORQ))
            oWriter = New IO.StringWriter(New StringBuilder)
            oSerializer.Serialize(oWriter, TT_IssueMCORQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.IssueMCO)

            Try
                oSerializer = New XmlSerializer(type:=GetType(wmIssueMCOOut.TT_IssueMCORS))
                oReader = New IO.StringReader(xmlMessage)
                oIssueMCORS = CType(oSerializer.Deserialize(oReader), wmIssueMCOOut.TT_IssueMCORS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsIssueMCO", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oIssueMCORS

        End Function

        <WebMethod(Description:="Process Issue Ticket Xml Messages Request.")>
        Public Function wmIssueMCOXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.IssueTicket)
        End Function

#End Region

    End Class

End Namespace
