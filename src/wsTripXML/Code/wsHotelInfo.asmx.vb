Imports System
Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Text
Imports CompressionExtension


Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement), _
     System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsHotelInfo", _
        Name:="wsHotelInfo", _
        Description:="A TripXML Web Service to Process Hotel Info Messages Request.")> _
    Public Class wsHotelInfo
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

        Private Function DecodeHotelInfo(ByVal strResponse As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim ttHotelAmenities As DataView
            Dim ttHotelAreas As DataView
            Dim ttHotelSubTitles As DataView
            Dim oNode As XmlNode = Nothing

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strResponse)
                oRoot = oDoc.DocumentElement

                If (Not oRoot.SelectNodes("HotelDescriptiveContents/HotelDescriptiveContent/FacilityInfo/GuestRooms/GuestRoom/Amenities/Amenity") Is Nothing) Or (Not oRoot.SelectNodes("Criteria/Criterion/HotelAmenity") Is Nothing) Then
                    ttHotelAmenities = CType(Application.Get("ttHotelAmenities"), DataView)

                    For Each oNode In oRoot.SelectNodes("HotelDescriptiveContents/HotelDescriptiveContent/FacilityInfo/GuestRooms/GuestRoom/Amenities/Amenity")
                        If Not oNode.Attributes("RoomAmenityCode") Is Nothing Then
                            oNode.InnerText = GetDecodeValue(ttHotelAmenities, oNode.Attributes("RoomAmenityCode").Value)
                        End If
                    Next

                    For Each oNode In oRoot.SelectNodes("Criteria/Criterion/HotelAmenity")
                        If Not oNode.Attributes("Code") Is Nothing Then
                            oNode.InnerText = GetDecodeValue(ttHotelAmenities, oNode.Attributes("Code").Value)
                        End If
                    Next
                End If

                If Not oRoot.SelectNodes("Areas/Area") Is Nothing Then
                    ttHotelAreas = CType(Application.Get("ttHotelAreas"), DataView)

                    For Each oNode In oRoot.SelectNodes("Areas/Area")
                        If Not oNode.Attributes("AreaID") Is Nothing Then
                            oNode.SelectSingleNode("AreaDescription/Text").InnerText = GetDecodeValue(ttHotelAreas, oNode.Attributes("AreaID").Value)
                        End If
                    Next
                End If

                If Not oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo/VendorMessages/VendorMessage[@InfoType='Text']") Is Nothing Then
                    ttHotelSubTitles = CType(Application.Get("ttHotelSubTitles"), DataView)

                    For Each oNode In oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo/VendorMessages/VendorMessage[@InfoType='Text']/SubSection")
                        If Not oNode.Attributes("SubCode") Is Nothing Then
                            oNode.Attributes("SubTitle").Value = GetDecodeValue(ttHotelSubTitles, oNode.Attributes("SubCode").Value)
                        End If
                    Next
                End If

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsHotelAvail", "Error *** Decoding HotelAvail Response", ex.Message, String.Empty)
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

                strRequest = strRequest.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
                strRequest = strRequest.Replace(" xmlns=""http://tripxml.downtowntravel.com/tripxml/wsHotelInfo""", "")
                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, StartTime, ttServiceID, Server.MachineName, UUID)
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
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

                        'strResponse = SendHotelRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                        'Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                        'sb.Remove(0, sb.Length())

                    Case "amadeusws"

                        strResponse = SendHotelRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "apollo", "galileo"

                        strResponse = SendHotelRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "sabre"

                        'ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                        'sb.Remove(0, sb.Length())
                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                            sb.Remove(0, sb.Length())
                            Exit Select
                        End If

                        ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC

                        strResponse = SendHotelRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                strResponse = DecodeHotelInfo(strResponse, ttCredential.UserID)

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsHotelInfo", "============= OTA Response ============= ", strResponse, UUID)
            End Try

            Return strResponse
            sb = Nothing
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()> _
        <WebMethod(Description:="Process Hotel Info Messages Request.")> _
        <System.Web.Services.Protocols.SoapHeader("tXML")> _
        Public Function wmHotelInfo(ByVal OTA_HotelDescriptiveInfoRQ As wmHotelInfoIn.OTA_HotelDescriptiveInfoRQ) As <XmlElementAttribute("OTA_HotelDescriptiveInfoRS")> wmHotelInfoOut.OTA_HotelDescriptiveInfoRS
            Dim xmlMessage As String = ""
            Dim oHotelInfoRS As wmHotelInfoOut.OTA_HotelDescriptiveInfoRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmHotelInfoIn.OTA_HotelDescriptiveInfoRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_HotelDescriptiveInfoRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.HotelInfo)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(Type:=GetType(wmHotelInfoOut.OTA_HotelDescriptiveInfoRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oHotelInfoRS = CType(oSerializer.Deserialize(oReader), wmHotelInfoOut.OTA_HotelDescriptiveInfoRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsHotelInfo", "Error Deserialing OTA Response", ex.InnerException.ToString(), String.Empty)
            End Try

            Return oHotelInfoRS

        End Function

        <WebMethod(Description:="Process Hotel Info Xml Messages Request.")> _
        Public Function wmHotelInfoXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.HotelInfo)
        End Function

#End Region

    End Class

End Namespace


