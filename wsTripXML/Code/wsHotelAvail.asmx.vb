Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Threading
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
    System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsHotelAvail",
        Name:="wsHotelAvail",
        Description:="A TripXML Web Service to Process Hotel Availability Messages Request.")>
    Public Class wsHotelAvail
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

        Private Function DecodeHotelAvail(ByVal strResponse As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim ttHotels As DataView
            Dim ttHotelAmenities As DataView
            Dim ttHotelAreas As DataView
            Dim ttHotelSubTitles As DataView
            Dim oNode As XmlNode = Nothing

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strResponse)
                oRoot = oDoc.DocumentElement

                ttHotels = CType(Application.Get("ttHotels"), DataView)

                For Each oNode In oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo")
                    ' *******************
                    ' Decode Hotels   *
                    ' *******************
                    If Not (oNode.Attributes("ChainCode") Is Nothing Or oNode.Attributes("ChainName") Is Nothing) Then
                        oNode.Attributes("ChainName").Value = GetDecodeValue(ttHotels, oNode.Attributes("ChainCode").Value)
                    End If
                    ' *******************************
                    ' Decode Hotels for OutriggerR  *
                    ' *******************************
                    If Not (oNode.Attributes("HotelCode") Is Nothing Or oNode.Attributes("HotelName") Is Nothing) Then
                        If oNode.Attributes("HotelName").Value.Length = 0 Then
                            oNode.Attributes("HotelName").Value = GetDecodeValue(ttHotels, oNode.Attributes("HotelCode").Value)
                        End If
                    End If
                Next

                If (Not oRoot.SelectNodes("RoomStays/RoomStay/RoomTypes/RoomType/Amenities/Amenity") Is Nothing) Or (Not oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo/HotelAmenity") Is Nothing) Then
                    ttHotelAmenities = CType(Application.Get("ttHotelAmenities"), DataView)

                    For Each oNode In oRoot.SelectNodes("RoomStays/RoomStay/RoomTypes/RoomType/Amenities/Amenity")
                        If Not oNode.Attributes("RoomAmenity") Is Nothing Then
                            oNode.InnerText = GetDecodeValue(ttHotelAmenities, oNode.Attributes("RoomAmenity").Value)
                        End If
                    Next

                    For Each oNode In oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo/HotelAmenity")
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
        Private sb As StringBuilder = New StringBuilder()

        Private mstrResponse As String = ""
        Private mintProviders As Integer = 0

        Private Sub GotResponse(ByVal Response As String)
            mstrResponse &= Response
            mintProviders += 1
        End Sub

        Public tXML As TripXML

#Region " Process Service Request All GDS "

        Private Function ServiceRequest(ByVal strRequest As String, ByVal ttServiceID As Integer) As String
            Dim strResponse As String = ""
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim ValidateXSDOut As Boolean
            Dim StartTime As Date
            Dim UUID As String = ""
            Dim i As Integer

            Try
                StartTime = Now

                PreServiceRequestPool(strRequest, Application, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                With ttCredential
                    For i = 0 To .Providers.Length - 1
                        Select Case .Providers(i).Name.ToLower
                            Case "amadeus"
                                Try
                                    'Dim ttAA As AmadeusAPIAdapter

                                    'ttAA = Application.Get(sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(i).PCC).ToString())
                                    sb.Remove(0, sb.Length())
                                    'If ttAA Is Nothing Then
                                    ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(i).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(i).PCC).ToString())
                                    sb.Remove(0, sb.Length())

                                    If ttProviderSystems.AmadeusWS = False Then
                                        GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), .Providers(i).Name))
                                        sb.Remove(0, sb.Length())
                                        Exit Select
                                    End If
                                    'End If

                                    If ttProviderSystems.AmadeusWS = True Then
                                        If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                            ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                        End If

                                        ttCredential.Providers(0).Name = "AmadeusWS"

                                        If ttCredential.System = "Test" Then
                                            ttProviderSystems.URL = "https://test.webservices.amadeus.com"
                                        ElseIf ttCredential.System = "Training" Then
                                            ttProviderSystems.URL = "https://production.webservices.amadeus.com"
                                        Else
                                            ttProviderSystems.URL = "https://production.webservices.amadeus.com"
                                        End If

                                        Dim oAmadeusWS As New cServiceAmadeusWS
                                        Dim oThreadAmadeusWS As New Thread(New ThreadStart(AddressOf oAmadeusWS.SendHotelRequest))
                                        AddHandler oAmadeusWS.GotResponse, AddressOf GotResponse

                                        With oAmadeusWS
                                            .ServiceID = ttServiceID
                                            .Request = strRequest
                                            .ttProviderSystems = ttProviderSystems
                                            .Version = ""
                                        End With
                                        oThreadAmadeusWS.Start()
                                        ttProviderSystems = Nothing
                                        'Else
                                        '    ttProviderSystems = ttAA.ttProviderSystems

                                        '    If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                        '        ttAA.SourcePCC = ttCredential.Providers(i).PCC
                                        '    Else
                                        '        ttAA.SourcePCC = ttAA.ttProviderSystems.PCC
                                        '    End If

                                        '    Dim oAmadeus As New cServiceAmadeus
                                        '    Dim oThreadAmadeus As New Thread(New ThreadStart(AddressOf oAmadeus.SendHotelRequest))
                                        '    AddHandler oAmadeus.GotResponse, AddressOf GotResponse

                                        '    With oAmadeus
                                        '        .ServiceID = ttServiceID
                                        '        .Request = strRequest
                                        '        .ttAA = ttAA
                                        '        .Version = ""
                                        '    End With

                                        '    oThreadAmadeus.Start()
                                        '    Application.Set(sb.Append("API").Append(.UserID).Append(.System).ToString(), ttAA)
                                        '    sb.Remove(0, sb.Length())
                                    End If
                                Catch e As Exception
                                    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                End Try
                            Case "apollo", "galileo"
                                Try
                                    ttProviderSystems = Application.Get(sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC).ToString())
                                    sb.Remove(0, sb.Length())
                                    If ttProviderSystems.System Is Nothing Then
                                        GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(.System).Append(" system. Or invalid provider.").ToString(), .Providers(i).Name))
                                        sb.Remove(0, sb.Length())
                                        Exit Select
                                    End If

                                    If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                        ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                    End If

                                    Dim oGalileo As New cServiceGalileo
                                    Dim oThreadGalileo As New Thread(New ThreadStart(AddressOf oGalileo.SendHotelRequest))
                                    AddHandler oGalileo.GotResponse, AddressOf GotResponse

                                    With oGalileo
                                        .ServiceID = ttServiceID
                                        .Request = strRequest
                                        .ProviderSystems = ttProviderSystems
                                        .Version = ""
                                    End With

                                    oThreadGalileo.Start()

                                Catch e As Exception
                                    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                End Try
                            Case "sabre"
                                Try
                                    ttProviderSystems = Application.Get(sb.Append("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC).ToString())
                                    sb.Remove(0, sb.Length())
                                    If ttProviderSystems.System Is Nothing Then
                                        GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(.System).Append(" system. Or invalid provider.").ToString(), .Providers(i).Name))
                                        sb.Remove(0, sb.Length())
                                        Exit Select
                                    End If

                                    ttProviderSystems.AAAPCC = .Providers(i).PCC

                                    'If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                    '    ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                    'End If

                                    Dim oSabre As New cServiceSabre
                                    Dim oThreadSabre As New Thread(New ThreadStart(AddressOf oSabre.SendHotelRequest))
                                    AddHandler oSabre.GotResponse, AddressOf GotResponse

                                    With oSabre
                                        .ServiceID = ttServiceID
                                        .Request = strRequest
                                        .ProviderSystems = ttProviderSystems
                                        .Version = ""
                                    End With

                                    oThreadSabre.Start()
                                Catch e As Exception
                                    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                End Try

                            Case Else
                                GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Provider ").Append(.Providers(i).Name).Append(" Not Currently Supported.").ToString(), .Providers(i).Name))
                                sb.Remove(0, sb.Length())
                        End Select
                    Next
                End With

                Dim StartCounter As Date = Now

                Do While mintProviders < ttCredential.Providers.Length
                    If CType(Now.Subtract(StartCounter).TotalSeconds, Integer) > CPrdTimeOut Then Exit Do
                    Threading.Thread.Sleep(10)
                Loop

                If ttCredential.Providers.Length > 1 Then
                    strResponse = String.Concat("<SuperRS>", mstrResponse, "</SuperRS>")
                    ' Aggregate
                    cAggregation.Aggregate(ttServiceID, XslPath, "", strResponse)
                Else
                    strResponse = mstrResponse
                End If

                StartCounter = Now
                strResponse = DecodeHotelAvail(strResponse, ttCredential.UserID)
                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.Append("Decoding = ").Append(CType(Now.Subtract(StartCounter).TotalMilliseconds, Integer)).ToString(), "", UUID)
                sb.Remove(0, sb.Length())

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, "")
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsHotelAvail", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "
        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Hotel Availability Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmHotelAvail(ByVal OTA_HotelAvailRQ As wmHotelAvailIn.OTA_HotelAvailRQ) As <XmlElementAttribute("OTA_HotelAvailRS")> wmHotelAvailOut.OTA_HotelAvailRS
            Dim xmlMessage As String = ""
            Dim oHotelAvailRS As wmHotelAvailOut.OTA_HotelAvailRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing
            oSerializer = New XmlSerializer(GetType(wmHotelAvailIn.OTA_HotelAvailRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_HotelAvailRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.HotelAvail)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmHotelAvailOut.OTA_HotelAvailRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oHotelAvailRS = CType(oSerializer.Deserialize(oReader), wmHotelAvailOut.OTA_HotelAvailRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsHotelAvail", "Error Deserialing OTA Response", ex.Message, String.Empty)
                xmlMessage = "<OTA_HotelAvailRS Version=""1.001""><Errors><Error>" & ex.InnerException.ToString() & "</Error></Errors></OTA_HotelAvailRS>"
                oReader = New System.IO.StringReader(xmlMessage)
                oHotelAvailRS = CType(oSerializer.Deserialize(oReader), wmHotelAvailOut.OTA_HotelAvailRS)
            End Try

            Return oHotelAvailRS

        End Function

        <WebMethod(Description:="Process Hotel Availability Xml Messages Request.")> _
        Public Function wmHotelAvailXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.HotelAvail)
        End Function

#End Region

    End Class

End Namespace

