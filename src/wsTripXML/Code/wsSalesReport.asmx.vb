Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain
Imports System.Xml.Serialization
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsSalesReport",
        Name:="wsSalesReport",
        Description:="A TripXML Web Service to Process SalesReport Messages Request.")>
    Public Class wsSalesReport
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

                        'strResponse = SendOtherRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                        'Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                        'sb.Remove(0, sb.Length())

                    Case "AmadeusWS"
                        strResponse = SendOtherRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")
                    Case "Sabre"
                        strResponse = SendOtherRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                        ' DecodeSalesReport(strResponse) Not Implemented.
                        strResponse = DecodeSalesReport(strResponse, ttCredential.UserID)
                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsSalesReport", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Decode Function "

        Private Function DecodeSalesReport(ByVal strResponse As String, ByVal UserID As String) As String
            Dim ttAirlines As DataView
            Try

                Dim oDoc As XmlDocument = New XmlDocument
                oDoc.LoadXml(strResponse)
                Dim oRoot As XmlElement = oDoc.DocumentElement

                ttAirlines = CType(Application.Get("ttAirlines"), DataView)

                Dim oNode As XmlNode
                For Each oNode In oRoot.SelectNodes("JournalEntries/JournalEntry")
                    Try

                        ' *******************
                        ' Decode Airlines   *
                        ' *******************
                        If Not oNode.SelectSingleNode("Airline") Is Nothing And Not oNode.SelectSingleNode("Airline").Attributes("Code") Is Nothing Then
                            If oNode.SelectSingleNode("Airline").Attributes("Code").Value <> "" Then
                                oNode.SelectSingleNode("Airline").InnerText = GetCodeValue(ttAirlines, oNode.SelectSingleNode("Airline").Attributes("Code").Value)
                            End If
                        End If

                    Catch e As Exception
                        CoreLib.SendTrace(UserID, "wsSalesReport", "Error *** Decoding Airline Response", e.Message, String.Empty)
                    End Try

                Next

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsSalesReport", "Error *** Decoding Airline Response", ex.Message, String.Empty)
            End Try
            Return strResponse
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process SalesReport Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmSalesReport(ByVal SalesReportRQ As wmSalesReportIn.SalesReportRQ) As <XmlElementAttribute("SalesReportRS")> wmSalesReportOut.SalesReportRS
            Dim xmlMessage As String = ""
            Dim oSalesReportRS As wmSalesReportOut.SalesReportRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmSalesReportIn.SalesReportRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, SalesReportRQ)
            xmlMessage = oWriter.ToString()
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.SalesReport)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmSalesReportOut.SalesReportRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oSalesReportRS = CType(oSerializer.Deserialize(oReader), wmSalesReportOut.SalesReportRS)

            Catch ex As Exception
                CoreLib.SendTrace("", "wsSalesReport", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oSalesReportRS

        End Function

        <WebMethod(Description:="Process SalesReport Xml Messages Request.")>
        Public Function wmSalesReportXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.SalesReport)
        End Function

#End Region

    End Class

End Namespace
