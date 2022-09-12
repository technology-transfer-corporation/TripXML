Imports System.Web.Services
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Data
Imports System.Text


Namespace wsTravelTalk


    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsStoredFareBuild", _
        Name:="wsStoredFareBuild", _
        Description:="A TripXML Web Service to Process Stored Fare Build Messages Request.")> _
    Public Class wsStoredFareBuild
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

                        'strResponse = SendTravelRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                        'Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                        'sb.Remove(0, sb.Length())

                    Case "AmadeusWS"

                        strResponse = SendTravelRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                Dim StartCounter As Date
                StartCounter = Now

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsStoredFareBuild", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <WebMethod(Description:="Process Air Flight Info Messages Request.")> _
        Public Function wmStoredFareBuild(ByVal OTA_StoredFareBuildRQ As wmStoredFareBuildIn.OTA_StoredFareBuildRQ) As <XmlElementAttribute("OTA_StoredFareBuildRS")> wmStoredFareBuildOut.OTA_StoredFareBuildRS
            Dim xmlMessage As String = ""
            Dim oStoredFareBuildRS As wmStoredFareBuildOut.OTA_StoredFareBuildRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmStoredFareBuildIn.OTA_StoredFareBuildRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_StoredFareBuildRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.StoredFareBuild)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(Type:=GetType(wmStoredFareBuildOut.OTA_StoredFareBuildRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oStoredFareBuildRS = CType(oSerializer.Deserialize(oReader), wmStoredFareBuildOut.OTA_StoredFareBuildRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsStoredFareBuild", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oStoredFareBuildRS

        End Function

        <WebMethod(Description:="Process Air Flight Info Xml Messages Request.")> _
        Public Function wmStoredFareBuildXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.StoredFareBuild)
        End Function

#End Region

    End Class

End Namespace
