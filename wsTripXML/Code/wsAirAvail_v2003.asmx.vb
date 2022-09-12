Imports System
Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Threading
Imports System.Data
Imports System.Text
Imports TripXMLMain.modCore
Imports TripXMLTools.TripXMLLoad

Namespace wsTravelTalk


    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsAirAvail",
        Name:="wsAirAvail_v2003",
        Description:="A TripXML Web Service to Process Air Availability Messages Request. OTA version 2.003.")>
    Public Class wsAirAvail_v2003
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

#Region " Decode Functions "

        Private Function DecodeAirAvail(ByVal strResponse As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strResponse)
                oRoot = oDoc.DocumentElement

                For Each oNode In oRoot.SelectNodes("OriginDestinationOptions/OriginDestinationOption/FlightSegment")
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
                CoreLib.SendTrace(UserID, "wsAirServices", "Error *** Decoding AirAvail Response", ex.Message, String.Empty)
            End Try
            Return strResponse
        End Function

#End Region

        Private mstrResponse As String = ""
        Private mintProviders As Integer = 0
        Private sb As StringBuilder = New StringBuilder()

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

            Try
                StartTime = Now

                PreServiceRequestPool(strRequest, Application, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID, "2005A")
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                With ttCredential
                    For i = 0 To .Providers.Length - 1
                        Select Case .Providers(i).Name.ToLower
                            Case "amadeus"
                                'Try
                                '    Dim ttAA As AmadeusAPIAdapter

                                '    ttAA = Application.Get(sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(i).PCC).ToString())
                                '    sb.Remove(0, sb.Length())
                                '    If ttAA Is Nothing Then
                                '        GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), .Providers(i).Name, "", False, "1.004"))
                                '        sb.Remove(0, sb.Length())
                                '        Exit Select
                                '    End If

                                '    If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                '        ttAA.SourcePCC = ttCredential.Providers(i).PCC
                                '    End If

                                '    Dim oAmadeus As New cServiceAmadeus
                                '    Dim oThreadAmadeus As New Thread(New ThreadStart(AddressOf oAmadeus.SendAirRequest))
                                '    AddHandler oAmadeus.GotResponse, AddressOf GotResponse

                                '    With oAmadeus
                                '        .ServiceID = ttServiceID
                                '        .Request = strRequest
                                '        .ttAA = ttAA
                                '        Version = "2005A"
                                '    End With
                                '    oThreadAmadeus.Start()
                                '    Application.Set(sb.Append("API").Append(.UserID).Append(.System).ToString(), ttAA)
                                '    sb.Remove(0, sb.Length())
                                'Catch e As Exception
                                '    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                'End Try
                            Case "apollo", "galileo"
                                Try
                                    ttProviderSystems = Application.Get("PS").Append(.Providers(i).Name).Append(.UserID).Append(.System).Append(.Providers(i).PCC)
                                    If ttProviderSystems.System Is Nothing Then
                                        GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(.Providers(i).Name).Append(" - ").Append(.System).Append(" system. Or invalid provider.").ToString(), .Providers(i).Name))
                                        sb.Remove(0, sb.Length())
                                        Exit Select
                                    End If

                                    If ttCredential.Providers(i).PCC.Trim.Length > 0 Then
                                        ttProviderSystems.PCC = ttCredential.Providers(i).PCC
                                    End If

                                    Dim oGalileo As New cServiceGalileo
                                    Dim oThreadGalileo As New Thread(New ThreadStart(AddressOf oGalileo.SendAirRequest))
                                    AddHandler oGalileo.GotResponse, AddressOf GotResponse

                                    With oGalileo
                                        .ServiceID = ttServiceID
                                        .Request = strRequest
                                        .ProviderSystems = ttProviderSystems
                                        .Version = "2005A"
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
                                    Dim oThreadSabre As New Thread(New ThreadStart(AddressOf oSabre.SendAirRequest))
                                    AddHandler oSabre.GotResponse, AddressOf GotResponse

                                    With oSabre
                                        .ServiceID = ttServiceID
                                        .Request = strRequest
                                        .ProviderSystems = ttProviderSystems
                                        .Version = "2005A"
                                    End With
                                    oThreadSabre.Start()
                                Catch e As Exception
                                    GotResponse(FormatErrorMessage(ttServiceID, e.Message, .Providers(i).Name))
                                End Try
                            Case Else
                                GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Provider ").Append(.Providers(i).Name).Append(" Not Currently Supported.").ToString(), .Providers(i).Name, "", False, "1.004"))
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
                strResponse = DecodeAirAvail(strResponse, ttCredential.UserID)
                CoreLib.SendTrace(ttCredential.UserID, "Performance", sb.Append("Decoding = ").Append(CType(Now.Subtract(StartCounter).TotalMilliseconds, Integer)).ToString(), "", UUID)
                sb.Remove(0, sb.Length())

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID, "2005A")

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, "1.004")
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsAirAvail_v2003", "============= OTA Response ============= ", strResponse, UUID)
            End Try

            Return strResponse
            sb = Nothing
        End Function

#End Region

#Region " Web Methods "

        <WebMethod(Description:="Process Air Availability Messages Request. OTA version 2.003.")>
        Public Function wmAirAvail(ByVal OTA_AirAvailRQ As wmAirAvail2005AIn.OTA_AirAvailRQ) As <XmlElementAttribute("OTA_AirAvailRS")> wmAirAvail2005AOut.OTA_AirAvailRS
            Dim xmlMessage As String = ""
            Dim oAirAvailRS As wmAirAvail2005AOut.OTA_AirAvailRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            If OTA_AirAvailRQ.Version = "2.003" Then

                oSerializer = New XmlSerializer(GetType(wmAirAvail2005AIn.OTA_AirAvailRQ))
                oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
                oSerializer.Serialize(oWriter, OTA_AirAvailRQ)
                xmlMessage = oWriter.ToString
                xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

                xmlMessage = ServiceRequest(xmlMessage, ttServices.AirAvail)

            Else
                xmlMessage = "<OTA_AirAvailRS Version=""1.004""><Errors>"
                xmlMessage &= "<Error Type=""E"">Invalid OTA version.</Error></Errors></OTA_AirAvailRS>"
            End If

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmAirAvail2005AOut.OTA_AirAvailRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oAirAvailRS = CType(oSerializer.Deserialize(oReader), wmAirAvail2005AOut.OTA_AirAvailRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsAirAvail_v2003", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oAirAvailRS

        End Function

        <WebMethod(Description:="Process Air Availability Xml Messages Request. OTA version 2.003.")> _
        Public Function wmAirAvailXml(ByVal xmlRequest As String) As String
            If xmlRequest.IndexOf("Version=""2.003""") > 0 Then
                Return ServiceRequest(xmlRequest, ttServices.AirAvail)
            Else
                xmlRequest = "<OTA_AirAvailRS Version='1.004'><Errors>"
                xmlRequest &= "<Error Type=""E"">Invalid OTA version.</Error></Errors></OTA_AirAvailRS>"
                Return xmlRequest
            End If
        End Function

#End Region

    End Class

End Namespace
