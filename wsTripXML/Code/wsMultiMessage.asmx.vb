Imports System
Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Data
Imports System.Text

Namespace wsTravelTalk


    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsMultiMessage", _
        Name:="wsMultiMessage", _
        Description:="A TripXML Web Service to Process MultiMessage Messages Request.")> _
    Public Class wsMultiMessage
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

#Region " Decode Function "
        Private sb As StringBuilder = New StringBuilder()

        Private Function DecodeMultiMessage(ByVal strResponse As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim ttAirports As DataView
            Dim ttAirlines As DataView
            Dim ttEquipments As DataView
            Dim oNode As XmlNode = Nothing
            Dim oNodeLF As XmlNode = Nothing
            Dim strResp As String = ""

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strResponse)
                oRoot = oDoc.DocumentElement

                ttAirports = CType(Application.Get("ttAirports"), DataView)
                ttAirlines = CType(Application.Get("ttAirlines"), DataView)
                ttEquipments = CType(Application.Get("ttEquipments"), DataView)

                For Each oNodeLF In oRoot.SelectSingleNode("Response").ChildNodes
                    For Each oNode In oNodeLF.SelectNodes("PricedItineraries/PricedItinerary/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment")
                        ' *******************
                        ' Decode Airports   *
                        ' *******************
                        oNode.SelectSingleNode("DepartureAirport").InnerText = GetDecodeValue(ttAirports, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                        oNode.SelectSingleNode("ArrivalAirport").InnerText = GetDecodeValue(ttAirports, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)

                        ' *******************
                        ' Decode Airlines   *
                        ' *******************
                        If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                            oNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                        End If
                        If Not oNode.SelectSingleNode("MarketingAirline") Is Nothing Then
                            oNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttAirlines, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                        End If

                        ' *******************
                        ' Decode Equipments *
                        ' *******************
                        If Not oNode.SelectSingleNode("Equipment") Is Nothing Then
                            oNode.SelectSingleNode("Equipment").InnerText = GetDecodeValue(ttEquipments, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                        End If
                    Next
                    strResp = sb.Append(strResp).Append(oNodeLF.OuterXml).ToString()
                    sb.Remove(0, sb.Length())
                Next

                'strResponse = oDoc.OuterXml

                strResponse = sb.Append("<MultiMessageRS><Success/><Response>").Append(strResp.Replace("<", "&lt;").Replace(">", "&gt;")).Append("</Response></MultiMessageRS>").ToString()
                sb.Remove(0, sb.Length())

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding MultiMessage Response", ex.Message, String.Empty)
            End Try
            Return strResponse
            sb = Nothing
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

                strRequest = strRequest.Replace("&amp;", "&")
                strRequest = strRequest.Replace("&lt;", "<").Replace("&gt;", ">")

                Select Case ttCredential.Providers(0).Name.ToLower
                    'Case "amadeus"
                    '    Dim ttAA As AmadeusAPIAdapter

                    '    ttAA = Application.Get("API").Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC)
                    '    If ttAA Is Nothing Then
                    '        Throw New Exception("Access denied to Amadeus - ").Append(ttCredential.System).Append(" system. Or invalid provider.")
                    '    End If

                    '    If ttCredential.Providers(0).PCC.Trim.Length > 0 Then
                    '        ttAA.SourcePCC = ttCredential.Providers(0).PCC
                    '    End If

                    '    strResponse = SendOtherRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                    '    Application.Set("API").Append(ttCredential.UserID).Append(ttCredential.System, ttAA)

                    Case "apollo", "galileo"

                        strResponse = SendOtherRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        'Case "sabre"

                        '    strResponse = SendOtherRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        'Case "worldspan"

                        '    strResponse = SendOtherRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                strResponse = DecodeMultiMessage(strResponse, ttCredential.UserID)
                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsMultiMessage", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <WebMethod(Description:="Process MultiMessage Messages Request.")> _
        Public Function wmMultiMessage(ByVal MultiMessageRQ As wmMultiMessageIn.MultiMessageRQ) As <XmlElementAttribute("MultiMessageRS")> wmMultiMessageOut.MultiMessageRS
            Dim xmlMessage As String = ""
            Dim oMultiMessageRS As wmMultiMessageOut.MultiMessageRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            MultiMessageRQ.MultiMessage = MultiMessageRQ.MultiMessage.Replace("<", "&lt;").Replace(">", "&gt;")

            oSerializer = New XmlSerializer(GetType(wmMultiMessageIn.MultiMessageRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, MultiMessageRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
            xmlMessage = xmlMessage.Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "")
            xmlMessage = xmlMessage.Replace("&amp;lt;", "&lt;").Replace("&amp;gt;", "&gt;")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.MultiMessage)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(Type:=GetType(wmMultiMessageOut.MultiMessageRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oMultiMessageRS = CType(oSerializer.Deserialize(oReader), wmMultiMessageOut.MultiMessageRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsMultiMessage", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oMultiMessageRS

        End Function

        <WebMethod(Description:="Process MultiMessage Xml Messages Request.")> _
        Public Function wmMultiMessageXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.MultiMessage)
        End Function

#End Region

    End Class

End Namespace

