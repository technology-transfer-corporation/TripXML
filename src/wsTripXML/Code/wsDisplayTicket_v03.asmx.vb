Imports System
Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Web.Services.Protocols
Imports TripXMLMain.modCore
Imports System.Text
Imports CompressionExtension

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement), _
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsDisplayTicket", _
        Name:="wsDisplayTicket_v03", _
        Description:="A TripXML Web Service to Process Display Ticket Messages Request.")> _
    Public Class wsDisplayTicket_v03
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

                        strResponse = SendTravelRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")


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
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsIssueTicket", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID)
            End Try

            Return strResponse
            sb = Nothing
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()> _
        <WebMethod(Description:="Process Void Ticket Messages Request.")> _
        <System.Web.Services.Protocols.SoapHeader("tXML")> _
        Public Function wmDisplayTicket(ByVal TT_DisplayTicketRQ As wmDisplayTicketIn.TT_DisplayTicketRQ) As <XmlElementAttribute("TT_DisplayTicketRS")> wmDisplayTicketOut_v03.TT_DisplayTicketRS
            Dim xmlMessage As String = ""
            Dim oDisplayTicketRS As wmDisplayTicketOut_v03.TT_DisplayTicketRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmDisplayTicketIn.TT_DisplayTicketRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, TT_DisplayTicketRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.TicketDisplay)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(Type:=GetType(wmDisplayTicketOut_v03.TT_DisplayTicketRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oDisplayTicketRS = CType(oSerializer.Deserialize(oReader), wmDisplayTicketOut_v03.TT_DisplayTicketRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsDisplayTicket", "Error Deserialing OTA Response", ex.Message, String.Empty)
                xmlMessage = "<TT_DisplayTicketRS Version=""v03""><Errors><Error>" & ex.InnerException.ToString() & "</Error></Errors></TT_DisplayTicketRS>"
                oReader = New System.IO.StringReader(xmlMessage)
                oDisplayTicketRS = CType(oSerializer.Deserialize(oReader), wmDisplayTicketOut_v03.TT_DisplayTicketRS)
            End Try

            Return oDisplayTicketRS

        End Function

        <WebMethod(Description:="Process Void Ticket Xml Messages Request.")> _
        Public Function wmDisplayTicketXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.TicketDisplay)
        End Function

#End Region

    End Class

End Namespace
