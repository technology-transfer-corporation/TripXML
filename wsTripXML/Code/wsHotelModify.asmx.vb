Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain
Imports System.Xml.Serialization
Imports TripXMLMain.modCore

Namespace wsTravelTalk


    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsHotelModify",
        Name:="wsHotelModify",
        Description:="A TripXML Web Service to Process Hotel Modify Messages Request.")>
    Public Class wsHotelModify
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

        Private Function DecodePNRRead(ByVal strResponse As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim ttAirports As DataView
            Dim ttAirlines As DataView
            Dim oNode As XmlNode = Nothing

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strResponse)
                oRoot = oDoc.DocumentElement

                ttAirports = CType(Application.Get("ttAirports"), DataView)
                ttAirlines = CType(Application.Get("ttAirlines"), DataView)

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
                        oNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                    End If
                    If Not oNode.SelectSingleNode("MarketingAirline") Is Nothing Then
                        oNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttAirlines, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                    End If
                Next

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", ex.Message, String.Empty)
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
            Dim StartTime As Date = Nothing
            Dim StartCounter As Date = Nothing
            Dim UUID As String = ""

            Try
                StartTime = Now

                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, StartTime, ttServiceID, Server.MachineName, UUID)
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                Select Case ttCredential.Providers(0).Name
                    'Case "OutriggerR"

                    '    strResponse = SendHotelRequestOutriggerR(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                StartCounter = Now

                strResponse = DecodePNRRead(strResponse, ttCredential.UserID)

                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.Append("Decoding = ").Append(CType(Now.Subtract(StartCounter).TotalMilliseconds, Integer)).ToString(), "", ttProviderSystems.LogUUID)
                sb.Remove(0, sb.Length())

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsHotelModify", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <WebMethod(Description:="Process Hotel Modify Messages Request.")>
        Public Function wmHotelModify(ByVal OTA_HotelResModifyRQ As wmHotelModifyIn.OTA_HotelResModifyRQ) As <XmlElementAttribute("OTA_TravelItineraryRS")> wmTravelItineraryOut.OTA_TravelItineraryRS
            Dim xmlMessage As String = ""
            Dim oTravelBuildRS As wmTravelItineraryOut.OTA_TravelItineraryRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmHotelModifyIn.OTA_HotelResModifyRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_HotelResModifyRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.HotelModify)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmTravelItineraryOut.OTA_TravelItineraryRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oTravelBuildRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut.OTA_TravelItineraryRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsHotelModify", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oTravelBuildRS

        End Function

        <WebMethod(Description:="Process Hotel Modify Xml Messages Request.")> _
        Public Function wmHotelModifyXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.HotelModify)
        End Function

#End Region

    End Class

End Namespace
