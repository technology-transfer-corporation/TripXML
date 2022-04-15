Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Text
Imports CompressionExtension

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement), _
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsCryptic", _
        Name:="wsCryptic", _
        Description:="A TripXML Web Service to Process Cryptic Messages Request.")> _
    Public Class wsCryptic
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
                    Case "Travelport"
                        strResponse = SendOtherRequestTravelport(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                    Case "Worldspan"
                        strResponse = SendOtherRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                    Case "Sabre"
                        'ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                        'sb.Remove(0, sb.Length())
                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                            sb.Remove(0, sb.Length())
                            Exit Select
                        End If

                        ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC

                        strResponse = SendOtherRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                End Select

                ' DecodeCryptic(strResponse) Not Implemented.

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsCryptic", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()> _
        <WebMethod(Description:="Process Cryptic Messages Request.")> _
        <System.Web.Services.Protocols.SoapHeader("tXML")> _
        Public Function wmCryptic(ByVal CrypticRQ As wmCrypticIn.CrypticRQ) As <XmlElementAttribute("CrypticRS")> wmCrypticOut.CrypticRS
            Dim xmlMessage As String = ""
            Dim oCrypticRS As wmCrypticOut.CrypticRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing
            Dim oLine As wmCrypticOut.Line

            oSerializer = New XmlSerializer(GetType(wmCrypticIn.CrypticRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, CrypticRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.Cryptic)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(Type:=GetType(wmCrypticOut.CrypticRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oCrypticRS = CType(oSerializer.Deserialize(oReader), wmCrypticOut.CrypticRS)
                ' Adding Back the CR removed by the Serializer
                If Not oCrypticRS.Response Is Nothing Then
                    oCrypticRS.Response = oCrypticRS.Response.Replace(vbLf, vbNewLine).Replace("<", "&lt;").Replace(">", "&gt;")

                    If Not oCrypticRS.Screen Is Nothing Then
                        For Each oLine In oCrypticRS.Screen
                            If Not oLine.Value Is Nothing Then
                                oLine.Value = oLine.Value.Replace(vbLf, vbNewLine).Replace("<", "&lt;").Replace(">", "&gt;")
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                CoreLib.SendTrace("", "wsCryptic", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oCrypticRS

        End Function

        <WebMethod(Description:="Process Cryptic Xml Messages Request.")> _
        Public Function wmCrypticXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.Cryptic)
        End Function

#End Region

    End Class

End Namespace
