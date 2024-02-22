Imports System
Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Text
Imports TripXMLMain.modCore
Imports System.Globalization
Imports TripXMLTools
Imports TripXMLTools.TripXMLLoad

Namespace wsTravelTalk

    <Protocols.SoapDocumentService(RoutingStyle:=Protocols.SoapServiceRoutingStyle.RequestElement), WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsUpdateSessioned",
        Name:="wsUpdateSessioned",
        Description:="A TripXML Web Service to Process UpdateSessioned Messages Request.")>
    Public Class wsUpdateSessioned
        Inherits WebService

#Region " Web Services Designer Generated Code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Web Services Designer.
            InitializeComponent()

            'Add your own initialization code after the InitializeComponent() call

        End Sub

        'Required by the Web Services Designer
        Private components As ComponentModel.IContainer

        'NOTE: The following procedure is required by the Web Services Designer
        'It can be modified using the Web Services Designer.  
        'Do not modify it using the code editor.
        <DebuggerStepThrough()> Private Sub InitializeComponent()
            components = New ComponentModel.Container
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

        Private Function DecodePNRRead(ByVal strResponse As String, ByVal UserID As String, ByVal UUID As String) As String
            Dim oDoc As XmlDocument
            Dim oRoot As XmlElement
            Dim oNode As XmlNode

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strResponse)
                oRoot = oDoc.DocumentElement

                Dim testNode As XmlNode = oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air")

                If (testNode Is Nothing) Then
                    CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** No Air Segments in PNR", "", UUID)
                Else

                    For Each oNode In oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air")
                        Try
                            Dim arnkElem As XmlNode = oNode.SelectSingleNode("TPA_Extensions/Arnk")
                            If Not arnkElem Is Nothing Then
                                Continue For
                            End If
                            ' *******************
                            ' Decode Airports   *
                            ' *******************
                            If Not oNode.SelectSingleNode("DepartureAirport") Is Nothing Then
                                oNode.SelectSingleNode("DepartureAirport").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                                'GetDecodeValue(ttAirports, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                            End If
                            If Not oNode.SelectSingleNode("ArrivalAirport") Is Nothing Then
                                oNode.SelectSingleNode("ArrivalAirport").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
                                'GetDecodeValue(ttAirports, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
                            End If

                            ' *******************
                            ' Decode Airlines   *
                            ' *******************
                            If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                                If Not oNode.SelectSingleNode("OperatingAirline").Attributes("Code") Is Nothing Then
                                    If oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value <> "" Then
                                        oNode.SelectSingleNode("OperatingAirline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                                        'GetDecodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                                    ElseIf Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                                        Dim attCode As XmlAttribute
                                        attCode = oDoc.CreateAttribute("Code")
                                        attCode.Value = TripXMLLoad.EncodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText)
                                        'GetEncodeValue(ttAirlinesNames, oNode.SelectSingleNode("OperatingAirline").InnerText)
                                        oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)

                                        oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                                    End If
                                Else
                                    If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                                        Dim attCode As XmlAttribute
                                        attCode = oDoc.CreateAttribute("Code")
                                        attCode.Value = TripXMLLoad.EncodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText)
                                        'GetEncodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").InnerText)

                                        If Not String.IsNullOrEmpty(attCode.Value) Then
                                            oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)
                                            oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                                        End If
                                    End If
                                End If
                            ElseIf Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                                Dim attCode As XmlAttribute
                                attCode = oDoc.CreateAttribute("Code")
                                attCode.Value = TripXMLLoad.EncodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText)
                                'GetEncodeValue(ttAirlinesNames, oNode.SelectSingleNode("OperatingAirline").InnerText)
                                oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)

                                oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                            End If
                            'If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                            '    If Not oNode.SelectSingleNode("OperatingAirline").Attributes("Code") Is Nothing Then
                            '        If oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value <> "" Then
                            '            oNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                            '        ElseIf Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                            '            Dim attCode As XmlAttribute
                            '            attCode = oDoc.CreateAttribute("Code")
                            '            attCode.Value = TripXMLLoad.EncodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText)
                            '            'GetEncodeValue(ttAirlinesNames, oNode.SelectSingleNode("OperatingAirline").InnerText)
                            '            oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)

                            '            oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                            '        End If
                            '        'oNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                            '    Else
                            '        If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                            '            Dim attCode As XmlAttribute
                            '            attCode = oDoc.CreateAttribute("Code")
                            '            If Not oNode.SelectSingleNode("OperatingAirline").Attributes("Code") Is Nothing Then
                            '                attCode.Value = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                            '                'GetEncodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").InnerText)

                            '                If Not String.IsNullOrEmpty(attCode.Value) Then
                            '                    oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)
                            '                    oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                            '                End If
                            '            Else
                            '                attCode.Value = TripXMLLoad.EncodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText)

                            '                If Not String.IsNullOrEmpty(attCode.Value) Then
                            '                    oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)
                            '                    oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                            '                End If
                            '            End If
                            '        End If
                            '    End If
                            'ElseIf Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                            '    Dim attCode As XmlAttribute
                            '    attCode = oDoc.CreateAttribute("Code")
                            '    attCode.Value = TripXMLLoad.EncodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").InnerText)
                            '    'GetEncodeValue(ttAirlinesNames, oNode.SelectSingleNode("OperatingAirline").InnerText)
                            '    oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)

                            '    oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                            'End If

                            If Not oNode.SelectSingleNode("MarketingAirline") Is Nothing Then
                                oNode.SelectSingleNode("MarketingAirline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                                'GetDecodeValue(ttAirlines, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                            End If

                            ' *******************
                            ' Decode Equipments   *
                            ' *******************
                            If Not oNode.SelectSingleNode("Equipment") Is Nothing Then
                                If Not oNode.SelectSingleNode("Equipment").Attributes("AirEquipType") Is Nothing Then
                                    oNode.SelectSingleNode("Equipment").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                                    'GetDecodeValue(ttEquipments, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                                End If
                            End If
                        Catch e As Exception
                            CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", e.Message, UUID)
                        End Try

                    Next
                End If

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", ex.Message, UUID)
            End Try
            Return strResponse
        End Function

#End Region

        Private sb As StringBuilder = New StringBuilder()

        Private mstrResponse As String = ""
        Private mintProviders As Integer = 0
        Private _recordLocator As String = ""
        'Private ttAPIAdapter As AmadeusAPIAdapter

        Private Sub GotResponse(ByVal Response As String)
            mstrResponse &= Response
            mintProviders += 1
        End Sub

        Public tXML As TripXML

#Region " Process Service Request All Suppliers "
        Private Function ServiceRequest(ByVal strRequest As String, ByVal ttServiceID As Integer) As String
            Dim strResponse As String = ""
            'Dim ttAA As AmadeusAdapter = Nothing
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim validateXSDOut As Boolean
            Dim startTime As Date
            Dim uuID As String = ""
            'Dim conversationID As String = ""

            Try
                startTime = Now

                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, startTime, ttServiceID, Server.MachineName, uuID)
                validateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                With ttCredential
                    Select Case .Providers(0).Name.ToLower
                        Case "amadeusws"
                            strResponse = SendTravelRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                            strResponse = DecodePNRRead(strResponse, ttCredential.UserID, uuID)
                        Case "apollo", "galileo"
                            sb.Remove(0, sb.Length())
                            If ttProviderSystems.System Is Nothing Then
                                FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                                sb.Remove(0, sb.Length())
                                Exit Select
                            End If

                            strResponse = SendTravelRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        Case "sabre"
                            If ttProviderSystems.System Is Nothing Then
                                FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                                sb.Remove(0, sb.Length())
                                Exit Select
                            End If

                            ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC
                            strResponse = SendTravelRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        Case "worldspan"
                            strResponse = SendTravelRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                            'Dim ttDefProvider As New TripXMLProviderSystems()
                            'Dim sTPRequest As String = CreatePNRRead(strRequest)
                            'PreServiceRequest(sTPRequest, Application, ttCredential, ttDefProvider, startTime, ttServiceID, Server.MachineName, uuID, "", True)
                            'strResponse = SendPNRRequestTravelPort(ttServiceID, ttCredential, ttDefProvider, sTPRequest, "v03")

                            strResponse = DecodePNRRead(strResponse, ttCredential.UserID, uuID)
                        Case Else
                            GotResponse(FormatErrorMessage(ttServiceID, sb.Append("Provider ").Append(.Providers(0).Name).Append(" Not Currently Supported.").ToString(), .Providers(0).Name))
                            sb.Remove(0, sb.Length())
                    End Select

                End With

                ' DecodeUpdateSessioned(strResponse) Not Implemented.

                PostServiceRequest(strResponse, validateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, startTime, ttServiceID, Server.MachineName, uuID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsUpdateSessioned", "============= OTA Response ============= ", strResponse, ttProviderSystems.LogUUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process UpdateSessioned Info Messages Request.")>
        <Protocols.SoapHeader("tXML")>
        Public Function wmUpdateSessioned(ByVal OTA_UpdateSessionedRQ As wmUpdateSessionedIn.OTA_UpdateSessionedRQ) As <XmlElementAttribute("OTA_TravelItineraryRS")> wmTravelItineraryOut_v03.OTA_TravelItineraryRS
            Dim xmlMessage As String = String.Empty
            Dim otaUpdateSessionedRS As wmTravelItineraryOut_v03.OTA_TravelItineraryRS
            Dim oSerializer As New XmlSerializer(GetType(wmUpdateSessionedIn.OTA_UpdateSessionedRQ))
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            Try
                oWriter = New IO.StringWriter(New StringBuilder)

                '*************************************
                '* Get PNR Modify XML Request Msg    * 
                '*************************************
                oSerializer.Serialize(oWriter, OTA_UpdateSessionedRQ)
                xmlMessage = oWriter.ToString
                xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
                xmlMessage = xmlMessage.Replace(" xmlns=""http://tripxml.downtowntravel.com/tripxml/wsUpdateSessioned""", "")
                xmlMessage = xmlMessage.Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "")
                xmlMessage = xmlMessage.Replace(vbCrLf, "")
                xmlMessage = xmlMessage.Replace("""", "'")

                xmlMessage = ServiceRequest(xmlMessage, ttServices.UpdateSessioned)

                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmTravelItineraryOut_v03.OTA_TravelItineraryRS))
                oReader = New IO.StringReader(xmlMessage)
                otaUpdateSessionedRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut_v03.OTA_TravelItineraryRS)
            Catch ex As Exception

                Dim oDoc As XmlDocument
                Dim oRoot As XmlElement
                oDoc = New XmlDocument()
                oDoc.LoadXml(xmlMessage)
                oRoot = oDoc.DocumentElement
                Dim sessionID = ""
                If Not oRoot.SelectSingleNode("ConversationID") Is Nothing Then
                    sessionID = oRoot.SelectSingleNode("ConversationID").OuterXml.Replace("&amp;", "&")
                End If

                Dim itinRefXmlList As String = String.Empty
                Dim custInfoXmlList As String = String.Empty
                Dim tpaInfoXmlList As String = String.Empty
                If oRoot.SelectSingleNode("TravelItinerary") Is Nothing Then
                    itinRefXmlList = oRoot.SelectSingleNode("TravelItinerary/ItineraryRef").OuterXml
                    custInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/CustomerInfos").OuterXml
                    tpaInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/TPA_Extensions").OuterXml
                End If

                Dim errMessage = $"<Errors><Error>{ex.InnerException?.Message}</Error><Error>{ex.Message}</Error></Errors>"

                xmlMessage = $"<OTA_TravelItineraryRS Version=""v03"" xmlns:stl=""http://services.sabre.com/STL/v01"">{errMessage}<TravelItinerary>{itinRefXmlList}{custInfoXmlList}<ItineraryInfo></ItineraryInfo>{tpaInfoXmlList}</TravelItinerary>{sessionID}</OTA_TravelItineraryRS>"

                oReader = New IO.StringReader(xmlMessage)
                otaUpdateSessionedRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut_v03.OTA_TravelItineraryRS)

            End Try

            Return otaUpdateSessionedRS

        End Function

        <WebMethod(Description:="Process UpdateSessioned Info Xml Messages Request.")>
        Public Function wmUpdateSessionedXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.UpdateSessioned)
        End Function

#End Region

    End Class

End Namespace


