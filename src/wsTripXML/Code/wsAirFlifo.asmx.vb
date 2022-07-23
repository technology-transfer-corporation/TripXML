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
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsAirFlifo",
        Name:="wsAirFlifo",
        Description:="A TripXML Web Service to Process Air Flight Info Messages Request.")>
    Public Class wsAirFlifo
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

        Private Function DecodeAirFlifo(ByVal strResponse As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim ttAirports As DataView
            Dim ttAirlines As DataView
            Dim ttEquipments As DataView
            Dim oNode As XmlNode = Nothing

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strResponse)
                oRoot = oDoc.DocumentElement

                ttAirports = CType(Application.Get("ttAirports"), DataView)
                ttAirlines = CType(Application.Get("ttAirlines"), DataView)
                ttEquipments = CType(Application.Get("ttEquipments"), DataView)

                For Each oNode In oRoot.SelectNodes("FlightInfoDetails/FlightLegInfo")
                    ' *******************
                    ' Decode Airports   *
                    ' *******************
                    oNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                    oNode.SelectSingleNode("ArrivalAirport").InnerText = DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)

                    ' *******************
                    ' Decode Airlines   *
                    ' *******************
                    If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                        oNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                    End If
                    If Not oNode.SelectSingleNode("MarketingAirline") Is Nothing Then
                        oNode.SelectSingleNode("MarketingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                    End If

                    ' *******************
                    ' Decode Equipments *
                    ' *******************
                    If Not oNode.SelectSingleNode("Equipment") Is Nothing Then
                        oNode.SelectSingleNode("Equipment").InnerText = DecodeValue(DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                    End If
                Next

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding AirFlifo Response", ex.Message, String.Empty)
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

                        'strResponse = SendAirRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                        'Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                        'sb.Remove(0, sb.Length())

                    Case "AmadeusWS"

                        strResponse = SendAirRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Apollo", "Galileo"

                        strResponse = SendAirRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Travelport"
                        strResponse = SendAirRequestTravelport(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Sabre"

                        'ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                        'sb.Remove(0, sb.Length())
                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                            sb.Remove(0, sb.Length())
                            Exit Select
                        End If

                        ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC

                        strResponse = SendAirRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                Dim StartCounter As Date
                StartCounter = Now

                strResponse = DecodeAirFlifo(strResponse, ttCredential.UserID)

                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.Append("Decoding = ").Append(CType(Now.Subtract(StartCounter).TotalMilliseconds, Integer)).ToString(), "", ttProviderSystems.LogUUID)
                sb.Remove(0, sb.Length())

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsAirFlifo", "============= OTA Response ============= ", strResponse, UUID)
            End Try

            Return strResponse
            sb = Nothing
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Air Flight Info Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmAirFlifo(ByVal OTA_AirFlifoRQ As wmAirFlifoIn.OTA_AirFlifoRQ) As <XmlElementAttribute("OTA_AirFlifoRS")> wmAirFlifoOut.OTA_AirFlifoRS
            Dim xmlMessage As String = ""
            Dim oAirFlifoRS As wmAirFlifoOut.OTA_AirFlifoRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmAirFlifoIn.OTA_AirFlifoRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_AirFlifoRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.AirFlifo)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmAirFlifoOut.OTA_AirFlifoRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oAirFlifoRS = CType(oSerializer.Deserialize(oReader), wmAirFlifoOut.OTA_AirFlifoRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsAirFlifo", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oAirFlifoRS

        End Function

        <WebMethod(Description:="Process Air Flight Info Xml Messages Request.")> _
        Public Function wmAirFlifoXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.AirFlifo)
        End Function

#End Region

    End Class

End Namespace
