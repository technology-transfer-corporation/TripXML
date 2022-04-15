Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Text
Imports wsTripXML.wsTravelTalk.wmAuthorizationIn
Imports wsTripXML.wsTravelTalk.wmAuthorizationOut
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsAuthorization",
                                       Name:="wsAuthorization",
                                       Description:="A TripXML Web Service to Process Authorization command.")>
    Public Class wsAuthorization
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
                        strResponse = SendTravelRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "worldspan"
                        strResponse = SendTravelRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                End Select

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsAuthorization", "============= OTA Response ============= ", strResponse, UUID)
            End Try

            Return strResponse
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension>
        <WebMethod(Description:="Process Authorization Messages Request.")>
        <Protocols.SoapHeader("tXML")>
        Public Function wmAuthorization(ByVal OTA_AuthorizationRQ As OTA_AuthorizationRQ) As <XmlElementAttribute("OTA_AuthorizationRS")> OTA_AuthorizationRS
            Dim xmlMessage
            Dim oAuthorizationRS As OTA_AuthorizationRS = Nothing
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            oSerializer = New XmlSerializer(GetType(OTA_AuthorizationRQ))
            oWriter = New IO.StringWriter(New StringBuilder)
            oSerializer.Serialize(oWriter, OTA_AuthorizationRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
            xmlMessage = xmlMessage.Replace(" xmlns=""http://www.opentravel.org/OTA/2003/05""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.Authorization)

            Try
                oSerializer = New XmlSerializer(type:=GetType(OTA_AuthorizationRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oAuthorizationRS = CType(oSerializer.Deserialize(oReader), OTA_AuthorizationRS)
                oAuthorizationRS.Authorization.AuthorizationDetail.BookingReferenceID = New BookingReferenceID
                oAuthorizationRS.Authorization.AuthorizationDetail.BookingReferenceID.ID = OTA_AuthorizationRQ.AuthorizationDetail.BookingReferenceID.ID
                oAuthorizationRS.Authorization.AuthorizationDetail.BookingReferenceID.Type = OTA_AuthorizationRQ.AuthorizationDetail.BookingReferenceID.Type

                If Not String.IsNullOrEmpty(oAuthorizationRS.Authorization.AuthorizationResult.AuthorizationCode) Then
                    oAuthorizationRS.Authorization.AuthorizationResult.Result = Detail.Approved
                End If

            Catch ex As Exception
                CoreLib.SendTrace("", "wsAuthorization", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oAuthorizationRS

        End Function

        <WebMethod(Description:="Process Authorization Xml Messages Request.")> _
        Public Function wmAuthorizationXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.Authorization)
        End Function

#End Region

    End Class
End Namespace
