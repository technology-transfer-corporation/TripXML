Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Data
Imports System.Text
Imports CompressionExtension

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement), _
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsPNRSplit", _
        Name:="wsPNRSplit", _
        Description:="A TripXML Web Service to Process PNR Split Request.")> _
    Public Class wsPNRSplit
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

        Private Function DecodePNRSplit(ByVal strResponse As String, ByVal UserID As String) As String
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
                    Try
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
                        If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing And Not oNode.SelectSingleNode("OperatingAirline").Attributes("Code") Is Nothing Then
                            If oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value <> "" Then
                                oNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                            End If
                        End If

                        If Not oNode.SelectSingleNode("MarketingAirline") Is Nothing Then
                            oNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttAirlines, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                        End If

                        ' *******************
                        ' Decode Equipments   *
                        ' *******************
                        If Not oNode.SelectSingleNode("Equipment") Is Nothing Then
                            If Not oNode.SelectSingleNode("Equipment").Attributes("AirEquipType") Is Nothing Then
                                oNode.SelectSingleNode("Equipment").InnerText = GetDecodeValue(ttEquipments, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                            End If
                        End If
                    Catch e As Exception
                        CoreLib.SendTrace(UserID, "wsPNRSplit", "Error *** Decoding PNRSplit Response", e.Message, String.Empty)
                    End Try

                Next

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsPNRSplit", "Error *** Decoding PNRSplit Response", ex.Message, String.Empty)
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
            Dim ValidateXSDOut As Boolean
            Dim StartTime As Date
            Dim UUID As String = ""

            Try
                StartTime = Now
                strRequest = strRequest.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://tripxml.downtowntravel.com/tripxml/wsQueue""", "")
                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, StartTime, ttServiceID, Server.MachineName, UUID)
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                Select Case ttCredential.Providers(0).Name

                    Case "AmadeusWS"

                        strResponse = SendPNRRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                strResponse = DecodePNRSplit(strResponse, ttCredential.UserID)

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsPNRSplit", "============= OTA Response ============= ", strResponse, UUID)
            End Try

            Return strResponse
            sb = Nothing
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()> _
        <WebMethod(Description:="Process PNR Split Messages Request.")> _
        <System.Web.Services.Protocols.SoapHeader("tXML")> _
        Public Function wmPNRSplit(ByVal OTA_PNRSplitRQ As wmPNRSplitIn.OTA_PNRSplitRQ) As <XmlElementAttribute("OTA_TravelItineraryRS")> wmTravelItineraryOut_v03.OTA_TravelItineraryRS
            Dim xmlMessage As String = ""
            Dim oPNRSplitRS As wmTravelItineraryOut_v03.OTA_TravelItineraryRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmPNRSplitIn.OTA_PNRSplitRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_PNRSplitRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.PNRSplit)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(Type:=GetType(wmTravelItineraryOut_v03.OTA_TravelItineraryRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oPNRSplitRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut_v03.OTA_TravelItineraryRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsPNRSplit", "Error Deserialing OTA Response", ex.InnerException.ToString(), String.Empty)
                xmlMessage = "<OTA_TravelItineraryRS Version=""v03""><Errors><Error>" & ex.InnerException.ToString() & "</Error></Errors></OTA_TravelItineraryRS>"
                oReader = New System.IO.StringReader(xmlMessage.Replace("&", "&amp;"))
                oPNRSplitRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut_v03.OTA_TravelItineraryRS)
            End Try

            Return oPNRSplitRS

        End Function

        <WebMethod(Description:="Process PNR Read Xml Messages Request.")> _
        Public Function wmPNRSplitXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.PNRSplit)
        End Function

#End Region

    End Class

End Namespace
