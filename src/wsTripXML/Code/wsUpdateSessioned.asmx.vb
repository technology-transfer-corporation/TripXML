Imports System
Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Text
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    <Protocols.SoapDocumentService(RoutingStyle:=Protocols.SoapServiceRoutingStyle.RequestElement), WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsUpdateSessioned", _
        Name:="wsUpdateSessioned", _
        Description:="A TripXML Web Service to Process UpdateSessioned Messages Request.")> _
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

        ' Not Implemented

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
                        Case "amadeus"
                            'ttAPIAdapter = Application.Get(sb.Append("API").Append(.UserID).Append(.System).Append(.Providers(0).PCC).ToString())
                            'sb.Remove(0, sb.Length())
                            'If ttAPIAdapter Is Nothing Then
                            '    Throw New Exception(sb.Append("Access denied to Amadeus - ").Append(.System).Append(" system. Or invalid provider.").ToString())
                            '    sb.Remove(0, sb.Length())
                            'End If

                            'If ttCredential.Providers(0).PCC.Trim.Length > 0 Then
                            '    ttAPIAdapter.SourcePCC = ttCredential.Providers(0).PCC
                            'End If

                            ''********************************
                            ''* Send  PNR Modify Request     * 
                            ''********************************
                            'strResponse = SendTravelRequestAmadeus(ttServiceID, ttCredential, ttAPIAdapter, strRequest)
                            'Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAPIAdapter)
                            'sb.Remove(0, sb.Length())

                        Case "amadeusws"

                            strResponse = SendTravelRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        Case "apollo", "galileo"
                            'ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                            sb.Remove(0, sb.Length())
                            If ttProviderSystems.System Is Nothing Then
                                FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                                sb.Remove(0, sb.Length())
                                Exit Select
                            End If

                            strResponse = SendTravelRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        Case "sabre"

                            'ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                            'sb.Remove(0, sb.Length())
                            If ttProviderSystems.System Is Nothing Then
                                FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                                sb.Remove(0, sb.Length())
                                Exit Select
                            End If

                            ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC

                            strResponse = SendTravelRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        Case "worldspan"

                            strResponse = SendTravelRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)
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

        <CompressionExtension.CompressionExtension()> _
        <WebMethod(Description:="Process UpdateSessioned Info Messages Request.")>
        <Protocols.SoapHeader("tXML")>
        Public Function wmUpdateSessioned(ByVal OTA_UpdateSessionedRQ As wmUpdateSessionedIn.OTA_UpdateSessionedRQ) As <XmlElementAttribute("OTA_TravelItineraryRS")> wmTravelItineraryOut_v03.OTA_TravelItineraryRS
            Dim xmlMessage As String
            Dim otaUpdateSessionedRS As wmTravelItineraryOut_v03.OTA_TravelItineraryRS
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            oSerializer = New XmlSerializer(GetType(wmUpdateSessionedIn.OTA_UpdateSessionedRQ))
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

            Try
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

                Dim itinRefXmlList As String
                Dim custInfoXmlList As String
                Dim tpaInfoXmlList As String
                If oRoot.SelectSingleNode("TravelItinerary") Is Nothing Then
                    itinRefXmlList = oRoot.SelectSingleNode("TravelItinerary/ItineraryRef").OuterXml
                    custInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/CustomerInfos").OuterXml
                    tpaInfoXmlList = oRoot.SelectSingleNode("TravelItinerary/TPA_Extensions").OuterXml
                End If

                Dim errMessage = String.Format("<Errors><Error>{0}</Error><Error>{1}</Error></Errors>", ex.InnerException.Message.ToString(), ex.Message.ToString())

                xmlMessage = String.Format("<OTA_TravelItineraryRS Version=""v03"" xmlns:stl=""http://services.sabre.com/STL/v01"">{0}<TravelItinerary>{1}{2}{3}{4}</TravelItinerary>{5}</OTA_TravelItineraryRS>", errMessage, itinRefXmlList, custInfoXmlList, "<ItineraryInfo></ItineraryInfo>", tpaInfoXmlList, sessionID)

                oReader = New IO.StringReader(xmlMessage)
                otaUpdateSessionedRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut_v03.OTA_TravelItineraryRS)

            End Try

            Return otaUpdateSessionedRS

        End Function

        <WebMethod(Description:="Process UpdateSessioned Info Xml Messages Request.")> _
        Public Function wmUpdateSessionedXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.UpdateSessioned)
        End Function

#End Region

    End Class

End Namespace


