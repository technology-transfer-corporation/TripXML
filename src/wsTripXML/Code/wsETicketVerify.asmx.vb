Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml.Serialization
Imports TripXMLMain.modCore

Namespace wsTravelTalk


    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsETicketVerify",
        Name:="wsETicketVerify",
        Description:="A TripXML Web Service to Process eTicket Verify Messages Request.")>
    Public Class wsETicketVerify
        Inherits System.Web.Services.WebService

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

        ' Not Apply

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
                    Case "Apollo", "Galileo"

                        strResponse = SendOtherRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                End Select

                ' Not Apply DecodeETicketVerify(strResponse, ttCredential.UserID)

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsETicketVerify", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <WebMethod(Description:="Process eTicket Verify Messages Request.")>
        Public Function wmETicketVerify(ByVal OTA_ETicketVerifyRQ As wmETicketVerifyIn.OTA_ETicketVerifyRQ) As <XmlElementAttribute("OTA_ETicketVerifyRS")> wmETicketVerifyOut.OTA_ETicketVerifyRS
            Dim xmlMessage As String = ""
            Dim oETicketVerifyRS As wmETicketVerifyOut.OTA_ETicketVerifyRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmETicketVerifyIn.OTA_ETicketVerifyRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_ETicketVerifyRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.ETicketVerify)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmETicketVerifyOut.OTA_ETicketVerifyRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oETicketVerifyRS = CType(oSerializer.Deserialize(oReader), wmETicketVerifyOut.OTA_ETicketVerifyRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsETicketVerify", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oETicketVerifyRS

        End Function

        <WebMethod(Description:="Process eTicket Verify Xml Messages Request.")> _
        Public Function wmETicketVerifyXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.ETicketVerify)
        End Function

#End Region

    End Class

End Namespace
