Imports System
Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Text
Imports System.Xml
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsUpdate",
        Name:="wsUpdate",
        Description:="A TripXML Web Service to Process Update Messages Request.")>
    Public Class wsUpdate
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

        ' Not Implemented

#End Region
        Private sb As StringBuilder = New StringBuilder()

        Private mstrResponse As String = ""
        Private mintProviders As Integer = 0
        Private RecordLocator As String = ""
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
            Dim ValidateXSDOut As Boolean
            Dim StartTime As Date
            Dim UUID As String = ""
            Dim ConversationID As String = ""

            Try
                StartTime = Now

                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, StartTime, ttServiceID, Server.MachineName, UUID)
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
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
                            ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
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

                ' DecodeUpdate(strResponse) Not Implemented.

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsUpdate", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Update Info Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmUpdate(ByVal OTA_UpdateRQ As wmUpdateIn.OTA_UpdateRQ) As <XmlElementAttribute("OTA_TravelItineraryRS")> wmTravelItineraryOut_v03.OTA_TravelItineraryRS
            Dim xmlMessage As String = ""
            Dim OTA_UpdateRS As wmTravelItineraryOut_v03.OTA_TravelItineraryRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmUpdateIn.OTA_UpdateRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)

            '*************************************
            '* Get PNR Modify XML Request Msg    * 
            '*************************************
            oSerializer.Serialize(oWriter, OTA_UpdateRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
            xmlMessage = xmlMessage.Replace(" xmlns=""http://tripxml.downtowntravel.com/tripxml/wsUpdate""", "")
            xmlMessage = xmlMessage.Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.Update)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmTravelItineraryOut_v03.OTA_TravelItineraryRS))
                oReader = New System.IO.StringReader(xmlMessage)
                OTA_UpdateRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut_v03.OTA_TravelItineraryRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsUpdate", "Error Deserialing OTA Response", ex.Message, String.Empty)
                Dim oDoc As XmlDocument
                Dim oRoot As XmlElement
                oDoc = New XmlDocument()
                oDoc.LoadXml(xmlMessage)
                oRoot = oDoc.DocumentElement
                Dim sessionID = ""
                If Not oRoot.SelectSingleNode("ConversationID") Is Nothing Then
                    sessionID = oRoot.SelectSingleNode("ConversationID").OuterXml.Replace("&amp;", "&")
                End If

                Dim itinRefXmlList As String = oRoot.SelectSingleNode("TravelItinerary/ItineraryRef").OuterXml
                Dim custInfoXmlList As String = oRoot.SelectSingleNode("TravelItinerary/CustomerInfos").OuterXml
                Dim tpaInfoXmlList As String = oRoot.SelectSingleNode("TravelItinerary/TPA_Extensions").OuterXml
                Dim errMessage = String.Format("<Errors><Error>{0}</Error><Error>{1}</Error></Errors>", ex.InnerException.Message.ToString(), ex.Message.ToString())

                xmlMessage = String.Format("<OTA_TravelItineraryRS Version=""v03"" xmlns:stl=""http://services.sabre.com/STL/v01"">{0}<TravelItinerary>{1}{2}{3}{4}</TravelItinerary>{5}</OTA_TravelItineraryRS>", errMessage, itinRefXmlList, custInfoXmlList, "<ItineraryInfo></ItineraryInfo>", tpaInfoXmlList, sessionID)

                oReader = New IO.StringReader(xmlMessage)
                OTA_UpdateRS = CType(oSerializer.Deserialize(oReader), wmTravelItineraryOut_v03.OTA_TravelItineraryRS)

            End Try

            Return OTA_UpdateRS

        End Function

        <WebMethod(Description:="Process Update Info Xml Messages Request.")> _
        Public Function wmUpdateXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.Update)
        End Function

#End Region

    End Class

End Namespace


