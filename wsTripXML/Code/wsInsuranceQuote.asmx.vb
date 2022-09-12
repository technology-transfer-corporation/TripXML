Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml.Serialization
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsInsuranceQuote",
        Name:="wsInsuranceQuote",
        Description:="A TravelTalk Web Service to Process Insurance Booking Messages Request.")>
    Public Class wsInsuranceQuote
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

        ' Not Implemented

#End Region

#Region " Process Service Request All GDS "

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
                ValidateXSDOut = Application.Get("XSD" & ttCredential.UserID & "Out")

                Select Case ttCredential.Providers(0).Name
                    Case "iTravelInsured", "iTI"

                        'strResponse = SendOtherRequestiTravelInsured(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception("Provider " & ttCredential.Providers(0).Name & " Not Currently Supported.")
                End Select

                Dim StartCounter As Date
                StartCounter = Now

                ' Not Implemented DecodeInsuranceQuote(strResponse, ttCredential.UserID)

                CoreLib.SendTrace(ttCredential.UserID, "Performance", "Decoding = " & CType(Now.Subtract(StartCounter).TotalMilliseconds, Integer), "", ttProviderSystems.LogUUID)

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsInsuranceQuote", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID)
            End Try

            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Insurance Booking Xml Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmInsuranceQuoteXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.InsuranceQuote)
        End Function

        <WebMethod(Description:="Process Insurance Booking Messages Request.")>
        Public Function wmInsuranceQuote(ByVal OTA_InsuranceQuoteRQ As wmInsuranceQuoteIn.OTA_InsuranceQuoteRQ) As <XmlElementAttribute("OTA_InsuranceQuoteRS")> wmInsuranceQuoteOut.OTA_InsuranceQuoteRS
            Dim xmlMessage As String = ""
            Dim oInsuranceQuoteRS As wmInsuranceQuoteOut.OTA_InsuranceQuoteRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmInsuranceQuoteIn.OTA_InsuranceQuoteRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_InsuranceQuoteRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.InsuranceQuote)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmInsuranceQuoteOut.OTA_InsuranceQuoteRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oInsuranceQuoteRS = CType(oSerializer.Deserialize(oReader), wmInsuranceQuoteOut.OTA_InsuranceQuoteRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsInsuranceQuote", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oInsuranceQuoteRS

        End Function

#End Region

    End Class

End Namespace
