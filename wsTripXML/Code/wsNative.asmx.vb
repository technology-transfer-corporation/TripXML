Imports System
Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Text
Imports CompressionExtension

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsNative",
        Name:="wsNative",
        Description:="A TripXML Web Service to Process Native Messages Request.")>
    Public Class wsNative
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

                'strRequest = strRequest.Replace("&amp;", "&")
                strRequest = strRequest.Replace("&lt;", "<").Replace("&gt;", ">")

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

                        'strResponse = SendOtherRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                        'Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                        'sb.Remove(0, sb.Length())

                    Case "amadeusws"
                        strResponse = SendOtherRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                    Case "apollo", "galileo"
                        strResponse = SendOtherRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                    Case "sabre"

                        'ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                        'sb.Remove(0, sb.Length())
                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                            sb.Remove(0, sb.Length())
                            Exit Select
                        End If

                        ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC

                        strResponse = SendOtherRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "worldspan"
                        strResponse = SendOtherRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                    Case "travelport"
                        strResponse = SendOtherRequestTravelport(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                    Case "sita"

                        'strResponse = SendOtherRequestSITA(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        'Case "airnz"

                        '    strResponse = SendOtherRequestAirNZ(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                End Select

                ' DecodeNative(strResponse) Not Implemented.

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsNative", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Native Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmNative(ByVal NativeRQ As wmNativeIn.NativeRQ) As <XmlElementAttribute("NativeRS")> wmNativeOut.NativeRS
            Dim xmlMessage As String = ""
            Dim oNativeRS As wmNativeOut.NativeRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            NativeRQ.Native = NativeRQ.Native.Replace("<", "&lt;").Replace(">", "&gt;")

            oSerializer = New XmlSerializer(GetType(wmNativeIn.NativeRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, NativeRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
            xmlMessage = xmlMessage.Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "")
            xmlMessage = xmlMessage.Replace("&amp;lt;", "&lt;").Replace("&amp;gt;", "&gt;")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.Native)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmNativeOut.NativeRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oNativeRS = CType(oSerializer.Deserialize(oReader), wmNativeOut.NativeRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsNative", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oNativeRS

        End Function

        <WebMethod(Description:="Process Native Xml Messages Request.")>
        Public Function wmNativeXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.Native)
        End Function

#End Region

    End Class

End Namespace
