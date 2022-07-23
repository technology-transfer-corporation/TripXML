Imports System
Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Data
Imports System.Text
Imports TripXMLMain.modCore
Imports TripXMLTools.TripXMLLoad

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsAirRules",
        Name:="wsAirRules_v03",
        Description:="A TripXML Web Service to Process Air Rules Messages Request - version 03.")>
    Public Class wsAirRules_v03
        Inherits System.Web.Services.WebService
        Private sb As StringBuilder = New StringBuilder()
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

        Private Function DecodeAirRules(ByVal strResponse As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim ttAirports As DataView
            Dim oNode As XmlNode = Nothing

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strResponse)
                oRoot = oDoc.DocumentElement

                ttAirports = CType(Application.Get("ttAirports"), DataView)

                For Each oNode In oRoot.SelectNodes("FareRuleResponseInfo/FareRuleInfo")
                    ' *******************
                    ' Decode Airports   *
                    ' *******************
                    If Not oNode.SelectSingleNode("DepartureAirport") Is Nothing Then
                        oNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                    End If
                    If Not oNode.SelectSingleNode("ArrivalAirport") Is Nothing Then
                        oNode.SelectSingleNode("ArrivalAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
                    End If
                Next

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding AirRules Response", ex.Message, String.Empty)
            End Try
            Return strResponse
        End Function

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
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                Select Case ttCredential.Providers(0).Name

                    Case "AmadeusWS"
                        strResponse = SendAirRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")
                    Case "Travelport"
                        strResponse = SendAirRequestTravelport(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")
                    Case "Worldspan"
                        strResponse = SendAirRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")
                    Case "Galileo"
                        strResponse = SendAirRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")
                    Case "Sabre"
                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                            sb.Remove(0, sb.Length())
                            Exit Select
                        End If

                        ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC

                        strResponse = SendAirRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")
                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                strResponse = DecodeAirRules(strResponse, ttCredential.UserID)

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsAirRules", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID)
            End Try

            Return strResponse
            sb = Nothing
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Air Rules Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmAirRules(ByVal OTA_AirRulesRQ As wmAirRulesIn_v03.OTA_AirRulesRQ) As <XmlElementAttribute("OTA_AirRulesRS")> wmAirRulesOut_v03.OTA_AirRulesRS
            Dim xmlMessage As String = ""
            Dim oAirRulesRS As wmAirRulesOut_v03.OTA_AirRulesRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmAirRulesIn_v03.OTA_AirRulesRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_AirRulesRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.AirRules)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmAirRulesOut_v03.OTA_AirRulesRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oAirRulesRS = CType(oSerializer.Deserialize(oReader), wmAirRulesOut_v03.OTA_AirRulesRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsAirRules", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oAirRulesRS

        End Function

        <WebMethod(Description:="Process Air Rules Xml Messages Request.")> _
        Public Function wmAirRulesXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.AirRules)
        End Function

#End Region

    End Class

End Namespace
