Imports System.Web.Services
Imports System.Xml
Imports System.IO
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Globalization
Imports System.Linq
Imports TripXMLMain.modCore
Imports TripXMLTools.TripXMLLoad
Imports TripXMLTools

Namespace wsTravelTalk

    <Protocols.SoapDocumentService(RoutingStyle:=Protocols.SoapServiceRoutingStyle.RequestElement),
        WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsPNRRead",
        Name:="wsPNRRead",
        Description:="A TripXML Web Service to Process PNR Read Request - version 03.")>
    Public Class wsPNRRead_v03
        Inherits WebService
        Public tXML As TripXML

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
                                'oNode.SelectSingleNode("DepartureAirport").InnerText = DecodeValue(ttAirports, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                                oNode.SelectSingleNode("DepartureAirport").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                                'DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value)
                            End If
                            If Not oNode.SelectSingleNode("ArrivalAirport") Is Nothing Then
                                oNode.SelectSingleNode("ArrivalAirport").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
                                'DecodeValue(DecodingType.Airport, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
                            End If

                            ' *******************
                            ' Decode Airlines   *
                            ' *******************
                            If Not oNode.SelectSingleNode("OperatingAirline") Is Nothing Then
                                If Not oNode.SelectSingleNode("OperatingAirline").Attributes("Code") Is Nothing Then
                                    If oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value <> "" Then
                                        oNode.SelectSingleNode("OperatingAirline").InnerText = DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
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
                                        If Not oNode.SelectSingleNode("OperatingAirline").Attributes("Code") Is Nothing Then
                                            attCode.Value = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("OperatingAirline").Attributes("Code").Value)
                                            'GetEncodeValue(ttAirlines, oNode.SelectSingleNode("OperatingAirline").InnerText)

                                            If Not String.IsNullOrEmpty(attCode.Value) Then
                                                oNode.SelectSingleNode("OperatingAirline").Attributes.Append(attCode)
                                                oNode.SelectSingleNode("OperatingAirline").InnerText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(oNode.SelectSingleNode("OperatingAirline").InnerText.ToLower())
                                            End If
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

                If Not oNode.SelectSingleNode("MarketingAirline") Is Nothing Then
                    oNode.SelectSingleNode("MarketingAirline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                    'DecodeValue(DecodingType.Airline, oNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                End If

                ' *******************
                ' Decode Equipments   *
                ' *******************
                If Not oNode.SelectSingleNode("Equipment") Is Nothing Then
                    If Not oNode.SelectSingleNode("Equipment").Attributes("AirEquipType") Is Nothing Then
                        oNode.SelectSingleNode("Equipment").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
                        'DecodeValue(DecodingType.Equipment, oNode.SelectSingleNode("Equipment").Attributes("AirEquipType").Value)
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

                Select Case ttCredential.Providers(0).Name
                    Case "AmadeusWS"
                        strResponse = SendPNRRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")
                    Case "Apollo", "Galileo"
                        strResponse = SendPNRRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")
                    Case "Sabre"
                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                            sb.Remove(0, sb.Length())
                            Exit Select
                        End If

                        ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC
                        strResponse = SendPNRRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")

                    Case "Travelport"
                        strResponse = SendPNRRequestTravelPort(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")
                    Case "Worldspan"
                        Dim ttDefProvider As New TripXMLProviderSystems()
                        PreServiceRequest(strRequest, Application, ttCredential, ttDefProvider, startTime, ttServiceID, Server.MachineName, uuid, "", True)
                        strResponse = SendPNRRequestTravelPort(ttServiceID, ttCredential, ttDefProvider, strRequest, "v03")
                        'strResponse = SendPNRRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest, "v03")
                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                End Select

                strResponse = DecodePNRRead(strResponse, ttCredential.UserID, uuid)

                PostServiceRequest(strResponse, validateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, startTime, ttServiceID, Server.MachineName, uuid)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsPNRRead", "============= OTA Response ============= ", strResponse, uuid)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process PNR Read Messages Request.")>
        <Protocols.SoapHeader("tXML")>
        Public Function wmPNRRead(ByVal OTA_ReadRQ As wmPNRReadIn.OTA_ReadRQ) As <XmlElementAttribute("OTA_TravelItineraryRS")> wmTravelItineraryOut_v03.OTA_TravelItineraryRS

            Dim oPNRReadRS As wmTravelItineraryOut_v03.OTA_TravelItineraryRS
            Dim xmlMessage As String = String.Empty
            Try
                Dim oSerializer As New XmlSerializer(GetType(wmPNRReadIn.OTA_ReadRQ))
                Dim oWriter As New StringWriter(New StringBuilder)
                oSerializer.Serialize(oWriter, OTA_ReadRQ)

                xmlMessage = oWriter.ToString
                xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

                xmlMessage = ServiceRequest(xmlMessage, ttServices.PNRRead)

                oSerializer = New XmlSerializer(type:=GetType(wmTravelItineraryOut_v03.OTA_TravelItineraryRS))
                Dim oReader As New StringReader(xmlMessage)
                oPNRReadRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut_v03.OTA_TravelItineraryRS)
            Catch ex As Exception
                oPNRReadRS = GetErrorPNRObject(ex, xmlMessage)
            End Try

            Return oPNRReadRS

        End Function

        Private Function GetErrorPNRObject(ex As Exception, xmlMessage As String) As wmTravelItineraryOut_v03.OTA_TravelItineraryRS
            Dim oPNRReadRS As wmTravelItineraryOut_v03.OTA_TravelItineraryRS
            Dim errList As New List(Of Exception)

            Try
                errList.Add(ex)
                Dim oReader As StringReader
                Dim oSerializer As XmlSerializer
                Dim oDoc As XmlDocument
                Dim oRoot As XmlElement
                oDoc = New XmlDocument()
                oDoc.LoadXml(xmlMessage)
                oRoot = oDoc.DocumentElement
                Dim sessionID As String = ""

                If Not oRoot.SelectSingleNode("ConversationID") Is Nothing Then
                    sessionID = oRoot.SelectSingleNode("ConversationID").InnerText
                End If

                Dim itinRefXmlList As String
                Dim oItinRef As wmTravelItineraryOut_v03.ItineraryRef

                Dim oCustInfos As New wmTravelItineraryOut_v03.CustomerInfosRS
                Dim oTPA As wmTravelItineraryOut_v03.TPA_ExtensionsRS

                Try
                    itinRefXmlList = oRoot.SelectSingleNode("TravelItinerary/ItineraryRef")?.OuterXml
                    oSerializer = New XmlSerializer(type:=GetType(wmTravelItineraryOut_v03.ItineraryRef))
                    oReader = New StringReader(itinRefXmlList)
                    oItinRef = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut_v03.ItineraryRef)
                Catch eref As Exception
                    oItinRef = New wmTravelItineraryOut_v03.ItineraryRef
                    errList.Add(eref)
                End Try

                Dim custInfoXmlList As String
                Try
                    custInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/CustomerInfos")?.OuterXml
                    oSerializer = New XmlSerializer(type:=GetType(wmTravelItineraryOut_v03.CustomerInfosRS), New XmlRootAttribute("CustomerInfos"))
                    oReader = New StringReader(custInfoXmlList)
                    oCustInfos = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut_v03.CustomerInfosRS)

                Catch ecust As Exception
                    oCustInfos = New wmTravelItineraryOut_v03.CustomerInfosRS
                    errList.Add(ecust)
                End Try

                Dim tpaInfoXmlList As String
                Try
                    tpaInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/TPA_Extensions")?.OuterXml
                    oSerializer = New XmlSerializer(type:=GetType(wmTravelItineraryOut_v03.TPA_ExtensionsRS), New XmlRootAttribute("TPA_Extensions"))
                    oReader = New StringReader(tpaInfoXmlList)
                    oTPA = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut_v03.TPA_ExtensionsRS)
                Catch etpa As Exception
                    oTPA = New wmTravelItineraryOut_v03.TPA_ExtensionsRS
                    errList.Add(etpa)
                End Try

                Dim travelItin As New wmTravelItineraryOut_v03.TravelItinerary With {
                    .ItineraryRef = oItinRef,
                    .CustomerInfos = oCustInfos,
                    .TPA_Extensions = oTPA
                }

                oPNRReadRS = New wmTravelItineraryOut_v03.OTA_TravelItineraryRS With {
                    .Errors = GetErrorObject(errList),
                    .ConversationID = sessionID,
                    .TravelItinerary = travelItin,
                    .Success = Nothing
                }

            Catch exX As Exception
                errList.Add(exX)

                oPNRReadRS = New wmTravelItineraryOut_v03.OTA_TravelItineraryRS With {
                    .Errors = GetErrorObject(errList)
                }
            End Try

            Return oPNRReadRS
        End Function

        Private Function GetErrorObject(exs As List(Of Exception)) As wmTravelItineraryOut_v03.Error()
            Dim errMessage As New List(Of wmTravelItineraryOut_v03.Error)
            Try
                For Each ex As Exception In exs
                    errMessage.Add(New wmTravelItineraryOut_v03.Error With {.Value = ex.Message})
                    If Not ex.InnerException Is Nothing Then
                        errMessage.Add(New wmTravelItineraryOut_v03.Error With {.Value = ex.InnerException.Message})
                    End If
                Next
            Catch exp As Exception
                errMessage.Add(New wmTravelItineraryOut_v03.Error With {.Value = exp.Message})
                errMessage.Add(New wmTravelItineraryOut_v03.Error With {.Value = exs.FirstOrDefault().Message})
            End Try

            Return errMessage.ToArray
        End Function

        <WebMethod(Description:="Process PNR Read Xml Messages Request.")>
        Public Function wmPNRReadXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.PNRRead)
        End Function



#End Region

    End Class

End Namespace
