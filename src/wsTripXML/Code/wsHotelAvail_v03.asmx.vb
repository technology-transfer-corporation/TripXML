Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml.Serialization

Namespace wsTravelTalk


    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsHotelAvail",
        Name:="wsHotelAvail_v03",
        Description:="A TripXML Web Service to Process Hotel Availability Messages Request version 03.")>
    Public Class wsHotelAvail_v03
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

                If (Not oRoot.SelectNodes("RoomStays/RoomStay/RoomTypes/RoomType/Amenities/Amenity") Is Nothing) Or (Not oRoot.SelectNodes("Criteria/Criterion/HotelAmenity") Is Nothing) Then
                    ttHotelAmenities = CType(Application.Get("ttHotelAmenities"), DataView)

                    For Each oNode In oRoot.SelectNodes("RoomStays/RoomStay/RoomTypes/RoomType/Amenities/Amenity")
                        If Not oNode.Attributes("RoomAmenity") Is Nothing Then
                            oNode.InnerText = GetDecodeValue(ttHotelAmenities, oNode.Attributes("RoomAmenity").Value)
                        End If
                    Next

                    For Each oNode In oRoot.SelectNodes("Criteria/Criterion/HotelAmenity")
                        If Not oNode.Attributes("Code") Is Nothing Then
                            Dim hacode As String = ""
                            If oNode.Attributes("Code").Value.Contains(" ") Then
                                hacode = ":" & oNode.Attributes("Code").Value.Substring(oNode.Attributes("Code").Value.IndexOf(" ")).Trim()
                                oNode.Attributes("Code").Value = oNode.Attributes("Code").Value.Substring(0, oNode.Attributes("Code").Value.IndexOf(" ") - 1)
                            End If
                            oNode.InnerText = GetDecodeValue(ttHotelAmenities, oNode.Attributes("Code").Value) & hacode
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

                If Not oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo/VendorMessages/VendorMessages[@InfoType='Text']") Is Nothing Then
                    ttHotelSubTitles = CType(Application.Get("ttHotelSubTitles"), DataView)

                    For Each oNode In oRoot.SelectNodes("RoomStays/RoomStay/BasicPropertyInfo/VendorMessages/VendorMessages[@InfoType='Text']/SubSection")
                        If Not oNode.Attributes("SubCode") Is Nothing Then
                            oNode.Attributes("SubTitle").Value = GetDecodeValue(ttHotelSubTitles, oNode.Attributes("SubCode").Value)
                        End If
                    Next
                End If

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsHotelAvail_v03", "Error *** Decoding HotelAvail Response", ex.Message, String.Empty)
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

#Region " Process Service Request All GDS "

        Private Function ServiceRequest(ByVal strRequest As String, ByVal ttServiceID As Integer) As String
            Dim strResponse As String = ""
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim ValidateXSDOut As Boolean
            Dim StartTime As Date
            Dim UUID As String = ""
            Dim i As Integer
            'Dim DoAmadeusSearches(99) As SearchHotelAmadeus
            Dim DoAmadeusWSSearches(99) As SearchHotelAmadeusWS
            Dim DoGalileoSearches(99) As SearchHotelGalileo
            Dim DoSabreSearches(99) As SearchHotelSabre
            'Dim DoWorldspanSearches(99) As SearchHotelWorldspan
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing
            Dim oNodeNewCriterion As XmlNode = Nothing
            Dim oDocCriterion As XmlDocument = Nothing
            Dim oRootCriterion As XmlElement = Nothing
            Dim oNodeCriterion As XmlNode = Nothing
            Dim strCriterion As String
            Dim j As Integer = 0

            Try
                StartTime = Now

                PreServiceRequestPool(strRequest, Application, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                oDoc = New XmlDocument
                oDoc.LoadXml(strRequest)
                oRoot = oDoc.DocumentElement
                oNodeNewCriterion = oRoot.SelectSingleNode("AvailRequestSegments/AvailRequestSegment/HotelSearchCriteria/Criterion")
                strCriterion = oNodeNewCriterion.OuterXml
                oDocCriterion = New XmlDocument
                oDocCriterion.LoadXml(strCriterion)
                oRootCriterion = oDocCriterion.DocumentElement
                oNodeNewCriterion.RemoveAll()

                With ttCredential
                    For i = 0 To .Providers.Length - 1
                        For Each oNodeCriterion In oRootCriterion.SelectNodes("HotelRef")
                            oNodeNewCriterion.InnerXml = oNodeCriterion.OuterXml
                            strRequest = oRoot.OuterXml
                            oNodeNewCriterion.RemoveAll()

                            Select Case .Providers(i).Name.ToLower
                                Case "amadeus", "amadeusws"
                                    Try
                                        'Dim ttAA As AmadeusAPIAdapter
                                        .Providers(i).Name = .Providers(i).Name.Replace("AmadeusWS", "Amadeus")
                                        'ttAA = Application.Get(sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(i).PCC).ToString())
                                        'sb.Remove(0, sb.Length())
                                        'If ttAA Is Nothing Then
                                        '    ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(i).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(i).PCC).ToString())
                                        '    sb.Remove(0, sb.Length())

                                        '    If ttProviderSystems.AmadeusWS = False Then
                                        '        GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), .Providers(i).Name))
                                        '        sb.Remove(0, sb.Length())
                                        '        Exit Select
                                        '    End If
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
                                            'Dim oThreadAmadeusWS As New Thread(New ThreadStart(AddressOf oAmadeusWS.SendHotelRequest))
                                            AddHandler oAmadeusWS.GotResponse, AddressOf GotResponse

                                            With oAmadeusWS
                                                .ServiceID = ttServiceID
                                                .Request = strRequest
                                                .ttProviderSystems = ttProviderSystems
                                                .Version = ""
                                            End With
                                            'oThreadAmadeusWS.Start()
                                            DoAmadeusWSSearches(j) = New SearchHotelAmadeusWS(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oAmadeusWS)
                                            DoAmadeusWSSearches(j).Request = strRequest
                                            DoAmadeusWSSearches(j).ServiceID = ttServiceID
                                            DoAmadeusWSSearches(j).BeginSearch()
                                            ttProviderSystems = Nothing
                                            'Else
                                            '    ttProviderSystems = ttAA.ttProviderSystems

                                            '    If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                            '        ttAA.SourcePCC = ttCredential.Providers(i).PCC
                                            '    Else
                                            '        ttAA.SourcePCC = ttAA.ttProviderSystems.PCC
                                            '    End If

                                            '    Dim oAmadeus As New cServiceAmadeus
                                            '    'Dim oThreadAmadeus As New Thread(New ThreadStart(AddressOf oAmadeus.SendHotelRequest))
                                            '    AddHandler oAmadeus.GotResponse, AddressOf GotResponse

                                            '    With oAmadeus
                                            '        .ServiceID = ttServiceID
                                            '        .Request = strRequest
                                            '        .ttAA = ttAA
                                            '        .Version = ""
                                            '    End With

                                            '    'oThreadAmadeus.Start()

                                            '    DoAmadeusSearches(j) = New SearchHotelAmadeus(.Providers(i).PCC, .UserID, .System, ttAA, oAmadeus)
                                            '    DoAmadeusSearches(j).Request = strRequest
                                            '    DoAmadeusSearches(j).ServiceID = ttServiceID
                                            '    DoAmadeusSearches(j).BeginSearch()

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
                                        'Dim oThreadGalileo As New Thread(New ThreadStart(AddressOf oGalileo.SendHotelRequest))
                                        AddHandler oGalileo.GotResponse, AddressOf GotResponse

                                        With oGalileo
                                            .ServiceID = ttServiceID
                                            .Request = strRequest
                                            .ProviderSystems = ttProviderSystems
                                            .Version = ""
                                        End With

                                        'oThreadGalileo.Start()
                                        DoGalileoSearches(j) = New SearchHotelGalileo(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oGalileo)
                                        DoGalileoSearches(j).Request = strRequest
                                        DoGalileoSearches(j).ServiceID = ttServiceID
                                        DoGalileoSearches(j).BeginSearch()

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
                                        'Dim oThreadSabre As New Thread(New ThreadStart(AddressOf oSabre.SendHotelRequest))
                                        AddHandler oSabre.GotResponse, AddressOf GotResponse

                                        With oSabre
                                            .ServiceID = ttServiceID
                                            .Request = strRequest
                                            .ProviderSystems = ttProviderSystems
                                            .Version = ""
                                        End With

                                        'oThreadSabre.Start()
                                        DoSabreSearches(j) = New SearchHotelSabre(.Providers(i).PCC, .UserID, .System, ttProviderSystems, oSabre)
                                        DoSabreSearches(j).Request = strRequest
                                        DoSabreSearches(j).ServiceID = ttServiceID
                                        DoSabreSearches(j).BeginSearch()
                                    Catch e As Exception
                                        GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                    End Try

                                Case Else
                                    GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Provider ").Append(.Providers(i).Name).Append(" Not Currently Supported.").ToString(), .Providers(i).Name))
                                    sb.Remove(0, sb.Length())
                            End Select
                            j += 1
                        Next
                    Next
                End With

                Dim StartCounter As Date = Now

                'Do While mintProviders < ttCredential.Providers.Length
                Do While mintProviders < j
                    If CType(Now.Subtract(StartCounter).TotalSeconds, Integer) > CPrdTimeOut Then Exit Do
                    Threading.Thread.Sleep(10)
                Loop

                'If ttCredential.Providers.Length > 1 Then
                If j > 1 Then
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
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsHotelAvail_v03", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <WebMethod(Description:="Process Hotel Availability Messages Request version v03.")>
        Public Function wmHotelAvail(ByVal OTA_HotelAvailRQ As wmHotelAvailIn_v03.OTA_HotelAvailRQ) As <XmlElementAttribute("OTA_HotelAvailRS")> wmHotelAvailOut_v03.OTA_HotelAvailRS
            Dim xmlMessage As String = ""
            Dim oHotelAvailRS As wmHotelAvailOut_v03.OTA_HotelAvailRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing
            oSerializer = New XmlSerializer(GetType(wmHotelAvailIn_v03.OTA_HotelAvailRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_HotelAvailRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.HotelAvail)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmHotelAvailOut_v03.OTA_HotelAvailRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oHotelAvailRS = CType(oSerializer.Deserialize(oReader), wmHotelAvailOut_v03.OTA_HotelAvailRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsHotelAvail_v03", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oHotelAvailRS

        End Function

        <WebMethod(Description:="Process Hotel Availability Xml Messages Request.")> _
        Public Function wmHotelAvailXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.HotelAvail)
        End Function

#End Region

    End Class

#Region "Search Amadeus"
    'Public Class SearchHotelAmadeus
    '    Private Delegate Sub StartSearch_Delegate()
    '    Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearch)
    '    Private pcc As String = ""
    '    Private userid As String = ""
    '    Private System As String = ""
    '    Private ttAA As AmadeusAPIAdapter
    '    Private _ServiceID As String = ""
    '    Private _Request As String = ""
    '    Private oAmadeus As cServiceAmadeus

    '    Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttAA As AmadeusAPIAdapter, ByRef _oAmadeus As cServiceAmadeus)
    '        Me.pcc = _pcc
    '        Me.userid = _userid
    '        Me.System = _System
    '        Me.ttAA = _ttAA
    '        Me.oAmadeus = _oAmadeus
    '    End Sub
    '    Public Property ServiceID() As String
    '        Get
    '            Return _ServiceID
    '        End Get
    '        Set(ByVal value As String)
    '            _ServiceID = value
    '        End Set
    '    End Property
    '    Public Property Request() As String
    '        Get
    '            Return _Request
    '        End Get
    '        Set(ByVal value As String)
    '            _Request = value
    '        End Set
    '    End Property
    '    Public Sub BeginSearch()
    '        Dim cbr As AsyncCallback = New AsyncCallback(AddressOf EndSearch)
    '        Dim arr As IAsyncResult = StartSearch_Wrapper.BeginInvoke(cbr, Nothing)
    '    End Sub
    '    Private Sub EndSearch(ByVal asy As IAsyncResult)
    '        StartSearch_Wrapper.EndInvoke(asy)
    '        asy.AsyncWaitHandle.Close()
    '    End Sub
    '    Private Sub DoAmadeusSearch()
    '        ttAA.SourcePCC = Me.pcc
    '        oAmadeus.SendHotelRequest()
    '        oAmadeus = Nothing
    '    End Sub
    'End Class
#End Region

#Region "Search AmadeusWS"
    Public Class SearchHotelAmadeusWS
        Private Delegate Sub StartSearch_Delegate()
        Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearchWS)
        Private pcc As String = ""
        Private userid As String = ""
        Private System As String = ""
        Private ttProviderSystems As TripXMLProviderSystems
        Private _ServiceID As String = ""
        Private _Request As String = ""
        Private oAmadeusWS As cServiceAmadeusWS

        Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oAmadeusWS As cServiceAmadeusWS)
            Me.pcc = _pcc
            Me.userid = _userid
            Me.System = _System
            Me.ttProviderSystems = _ttProviderSystems
            Me.oAmadeusWS = _oAmadeusWS
        End Sub
        Public Property ServiceID() As String
            Get
                Return _ServiceID
            End Get
            Set(ByVal value As String)
                _ServiceID = value
            End Set
        End Property
        Public Property Request() As String
            Get
                Return _Request
            End Get
            Set(ByVal value As String)
                _Request = value
            End Set
        End Property
        Public Sub BeginSearch()
            Dim cbr As AsyncCallback = New AsyncCallback(AddressOf EndSearch)
            Dim arr As IAsyncResult = StartSearch_Wrapper.BeginInvoke(cbr, Nothing)
        End Sub
        Private Sub EndSearch(ByVal asy As IAsyncResult)
            StartSearch_Wrapper.EndInvoke(asy)
            asy.AsyncWaitHandle.Close()
        End Sub
        Private Sub DoAmadeusSearchWS()

            ttProviderSystems.PCC = Me.pcc
            oAmadeusWS.SendHotelRequest()
            oAmadeusWS = Nothing
        End Sub
    End Class
#End Region

#Region "Search Galileo"
    Public Class SearchHotelGalileo
        Private Delegate Sub StartSearch_Delegate()
        Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoGalileoSearch)
        Private pcc As String = ""
        Private userid As String = ""
        Private System As String = ""
        Private ttProviderSystems As TripXMLProviderSystems
        Private _ServiceID As String = ""
        Private _Request As String = ""
        Private oGalileo As cServiceGalileo
        Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oGalileo As cServiceGalileo)
            Me.pcc = _pcc
            Me.userid = _userid
            Me.System = _System
            Me.ttProviderSystems = _ttProviderSystems
            Me.oGalileo = _oGalileo
        End Sub
        Public Property ServiceID() As String
            Get
                Return _ServiceID
            End Get
            Set(ByVal value As String)
                _ServiceID = value
            End Set
        End Property
        Public Property Request() As String
            Get
                Return _Request
            End Get
            Set(ByVal value As String)
                _Request = value
            End Set
        End Property
        Public Sub BeginSearch()
            Dim cbr As AsyncCallback = New AsyncCallback(AddressOf EndSearch)
            Dim arr As IAsyncResult = StartSearch_Wrapper.BeginInvoke(cbr, Nothing)
        End Sub
        Private Sub EndSearch(ByVal asy As IAsyncResult)
            StartSearch_Wrapper.EndInvoke(asy)
            asy.AsyncWaitHandle.Close()
        End Sub
        Private Sub DoGalileoSearch()
            ttProviderSystems.PCC = Me.pcc
            oGalileo.SendHotelRequest()
            oGalileo = Nothing
        End Sub
    End Class
#End Region

#Region "Search Sabre"
    Public Class SearchHotelSabre
        Private Delegate Sub StartSearch_Delegate()
        Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoSabreSearchWS)
        Private pcc As String = ""
        Private userid As String = ""
        Private System As String = ""
        Private ttProviderSystems As TripXMLProviderSystems
        Private _ServiceID As String = ""
        Private _Request As String = ""
        Private oSabre As cServiceSabre

        Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oSabre As cServiceSabre)
            Me.pcc = _pcc
            Me.userid = _userid
            Me.System = _System
            Me.ttProviderSystems = _ttProviderSystems
            Me.oSabre = _oSabre
        End Sub
        Public Property ServiceID() As String
            Get
                Return _ServiceID
            End Get
            Set(ByVal value As String)
                _ServiceID = value
            End Set
        End Property
        Public Property Request() As String
            Get
                Return _Request
            End Get
            Set(ByVal value As String)
                _Request = value
            End Set
        End Property
        Public Sub BeginSearch()
            Dim cbr As AsyncCallback = New AsyncCallback(AddressOf EndSearch)
            Dim arr As IAsyncResult = StartSearch_Wrapper.BeginInvoke(cbr, Nothing)
        End Sub
        Private Sub EndSearch(ByVal asy As IAsyncResult)
            StartSearch_Wrapper.EndInvoke(asy)
            asy.AsyncWaitHandle.Close()
        End Sub
        Private Sub DoSabreSearchWS()
            ttProviderSystems.PCC = Me.pcc
            oSabre.SendHotelRequest()
            oSabre = Nothing
        End Sub
    End Class
#End Region

#Region "Search Worldspan"
    'Public Class SearchHotelWorldspan
    '    Private Delegate Sub StartSearch_Delegate()
    '    Private StartSearch_Wrapper As New StartSearch_Delegate(AddressOf DoAmadeusSearchWS)
    '    Private pcc As String = ""
    '    Private userid As String = ""
    '    Private System As String = ""
    '    Private ttProviderSystems As TripXMLProviderSystems
    '    Private _ServiceID As String = ""
    '    Private _Request As String = ""
    '    Private oWorldspan As cServiceWorldspan

    '    Public Sub New(ByVal _pcc As String, ByVal _userid As String, ByVal _System As String, ByRef _ttProviderSystems As TripXMLProviderSystems, ByRef _oWorldspan As cServiceWorldspan)
    '        Me.pcc = _pcc
    '        Me.userid = _userid
    '        Me.System = _System
    '        Me.ttProviderSystems = _ttProviderSystems
    '        Me.oWorldspan = _oWorldspan
    '    End Sub
    '    Public Property ServiceID() As String
    '        Get
    '            Return _ServiceID
    '        End Get
    '        Set(ByVal value As String)
    '            _ServiceID = value
    '        End Set
    '    End Property
    '    Public Property Request() As String
    '        Get
    '            Return _Request
    '        End Get
    '        Set(ByVal value As String)
    '            _Request = value
    '        End Set
    '    End Property
    '    Public Sub BeginSearch()
    '        Dim cbr As AsyncCallback = New AsyncCallback(AddressOf EndSearch)
    '        Dim arr As IAsyncResult = StartSearch_Wrapper.BeginInvoke(cbr, Nothing)
    '    End Sub
    '    Private Sub EndSearch(ByVal asy As IAsyncResult)
    '        StartSearch_Wrapper.EndInvoke(asy)
    '        asy.AsyncWaitHandle.Close()
    '    End Sub
    '    Private Sub DoAmadeusSearchWS()
    '        ttProviderSystems.PCC = Me.pcc
    '        oWorldspan.SendHotelRequest()
    '        oWorldspan = Nothing
    '    End Sub
    'End Class
#End Region


End Namespace

