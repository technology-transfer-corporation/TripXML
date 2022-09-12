Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml.Serialization
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsSessionCreate",
        Name:="wsSessionCreate",
        Description:="A TripXML Web Service to Process Session Create Messages Request.")>
    Public Class wsSessionCreate
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
            Dim strResponse As String
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim validateXSDOut As Boolean
            Dim startTime As Date
            Dim uuid As String

            Try
                startTime = Now

                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, startTime, ttServiceID, Server.MachineName, uuid)
                validateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())
                ttProviderSystems.LogUUID = uuid

                Select Case ttCredential.Providers(0).Name
                    Case "AmadeusWS"

                        strResponse = SendOtherRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Apollo", "Galileo"

                        strResponse = SendOtherRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Sabre"

                        sb.Remove(0, sb.Length())
                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                            sb.Remove(0, sb.Length())
                            Exit Select
                        End If

                        strResponse = SendOtherRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Worldspan"

                        strResponse = SendOtherRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Travelport"

                        strResponse = SendOtherRequestTravelport(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                End Select

                ' DecodeSessionCreate(strResponse) Not Implemented.

                PostServiceRequest(strResponse, validateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttProviderSystems)
            Finally
                LogResponse(strResponse, ttCredential, startTime, ttServiceID, Server.MachineName, uuid)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsSessionCreate", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Session Create Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmSessionCreate(ByVal SessionCreateRQ As wmSessionCreateIn.SessionCreateRQ) As <XmlElementAttribute("SessionCreateRS")> wmSessionCreateOut.SessionCreateRS
            Dim xmlMessage As String
            Dim oSessionCreateRS As wmSessionCreateOut.SessionCreateRS = Nothing
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            oSerializer = New XmlSerializer(GetType(wmSessionCreateIn.SessionCreateRQ))
            oWriter = New IO.StringWriter(New StringBuilder)
            oSerializer.Serialize(oWriter, SessionCreateRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CreateSession)

            Try
                oSerializer = New XmlSerializer(type:=GetType(wmSessionCreateOut.SessionCreateRS))
                oReader = New IO.StringReader(xmlMessage)
                oSessionCreateRS = CType(oSerializer.Deserialize(oReader), wmSessionCreateOut.SessionCreateRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsSessionCreate", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oSessionCreateRS

        End Function

        <WebMethod(Description:="Process Session Create Xml Messages Request.")> _
        Public Function wmSessionCreateXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.CreateSession)
        End Function

#End Region

    End Class

End Namespace
