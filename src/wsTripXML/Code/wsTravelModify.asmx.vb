Imports System
Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Data
Imports System.Text
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsTravelModify",
        Name:="wsTravelModify",
        Description:="A TripXML Web Service to Process Travel Build Request.")>
    Public Class wsTravelModify
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

        Private Function DecodeTravelModify(ByVal strResponse As String, ByVal UserID As String) As String
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

                For Each oNode In oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air")
                    ' *******************
                    ' Decode Airports   *
                    ' *******************
                    If Not oNode.SelectSingleNode("DepartureAirport") Is Nothing Then
                        oNode.SelectSingleNode("DepartureAirport").InnerText = GetDecodeValue(ttAirports, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                    End If
                    If Not oNode.SelectSingleNode("ArrivalAirport") Is Nothing Then
                        oNode.SelectSingleNode("ArrivalAirport").InnerText = GetDecodeValue(ttAirports, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
                    End If

                    ' *******************
                    ' Decode Airlines   *
                    ' *******************
                    If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                        If Not oNode.SelectSingleNode("OperatingAirline").Attributes("Code") Is Nothing Then
                            oNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                        End If
                    End If
                    If Not oNode.SelectSingleNode("MarketingAirline") Is Nothing Then
                        If Not oNode.SelectSingleNode("MarketingAirline").Attributes("Code") Is Nothing Then
                            oNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttAirlines, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                        End If
                    End If
                    ' *******************
                    ' Decode Equipments *
                    ' *******************
                    If Not oNode.SelectSingleNode("Equipment") Is Nothing Then
                        If Not oNode.SelectSingleNode("Equipment").Attributes("AirEquipType") Is Nothing Then
                            oNode.SelectSingleNode("Equipment").InnerText = GetDecodeValue(ttEquipments, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                        End If
                    End If
                Next

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsTravelServices", "Error *** Decoding TravelModify Response", ex.Message, String.Empty)
            End Try
            Return strResponse
        End Function

#End Region

#Region " Process Service Request All GDS "
        Private sb As StringBuilder = New StringBuilder()

        Private Function ServiceRequest(ByVal strRequest As String, ByVal ttServiceID As Integer) As String
            Dim strResponse As String = ""
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim validateXSDOut As Boolean
            Dim startTime As Date
            Dim uuid As String = ""

            Try
                startTime = Now

                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, startTime, ttServiceID, Server.MachineName, uuid)
                validateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

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

                        'strResponse = SendTravelRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                        'Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                        'sb.Remove(0, sb.Length())

                    Case "apollo", "galileo"

                        strResponse = SendTravelRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "sabre"

                        strResponse = SendTravelRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        'Case "outriggerr"

                        '    strResponse = SendTravelRequestOutriggerR(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        'Case "worldspan"

                        '    strResponse = SendTravelRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        'Case "agentware"

                        '    strResponse = SendTravelRequestAgentWare(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        'Case "sentient"

                        '    strResponse = SendTravelRequestSentient(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                strResponse = DecodeTravelModify(strResponse, ttCredential.UserID)

                PostServiceRequest(strResponse, validateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, startTime, ttServiceID, Server.MachineName, uuid)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsTravelModify", "============= OTA Response ============= ", strResponse, uuid)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Travel Modify Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmTravelModify(ByVal OTA_TravelModifyRQ As wmTravelModifyIn.OTA_TravelModifyRQ) As <XmlElementAttribute("OTA_TravelItineraryRS")> wmTravelItineraryOut.OTA_TravelItineraryRS
            Dim xmlMessage As String = ""
            Dim oTravelModifyRS As wmTravelItineraryOut.OTA_TravelItineraryRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmTravelModifyIn.OTA_TravelModifyRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_TravelModifyRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.TravelModify)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmTravelItineraryOut.OTA_TravelItineraryRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oTravelModifyRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut.OTA_TravelItineraryRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsTravelModify", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oTravelModifyRS

        End Function

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Travel Modify MCO Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmIssueMCO(ByVal OTA_TravelModifyRQ As wmTravelModifyIn.OTA_TravelModifyRQ) As <XmlElementAttribute("OTA_TravelItineraryRS")> wmTravelItineraryOut.OTA_TravelItineraryRS
            Dim xmlMessage As String = ""
            Dim oTravelModifyRS As wmTravelItineraryOut.OTA_TravelItineraryRS = Nothing
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            oSerializer = New XmlSerializer(GetType(wmTravelModifyIn.OTA_TravelModifyRQ))
            oWriter = New IO.StringWriter(New StringBuilder)
            oSerializer.Serialize(oWriter, OTA_TravelModifyRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.PNRSplit)

            Try
                oSerializer = New XmlSerializer(type:=GetType(wmTravelItineraryOut.OTA_TravelItineraryRS))
                oReader = New IO.StringReader(xmlMessage)
                oTravelModifyRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut.OTA_TravelItineraryRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wmIssueMCO", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oTravelModifyRS

        End Function

        <WebMethod(Description:="Process Travel Modify Xml Messages Request.")> _
        Public Function wmTravelModifyXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.TravelModify)
        End Function

#End Region

    End Class

End Namespace
