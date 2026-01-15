Imports System
Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Data
Imports System.Text
Imports CompressionExtension
Imports TripXMLTools

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsGetDeals",
        Name:="wsGetDeals",
        Description:="A TripXML Web Service to get fare deals.")>
    Public Class wsGetDeals
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

        Private Function DecodeTXMLGetDeals(ByVal strResponse As String, ByVal UserID As String) As String
            Try

                Dim oDoc As XmlDocument = New XmlDocument
                oDoc.LoadXml(strResponse)
                Dim oRoot As XmlElement = oDoc.DocumentElement

                'Dim ttAirports As DataView
                'Dim ttAirlines As DataView
                Dim oNode As XmlNode
                'ttAirports = CType(Application.Get("ttAirports"), DataView)
                'ttAirlines = CType(Application.Get("ttAirlines"), DataView)

                For Each oNode In oRoot.SelectNodes("Deals/Deal")
                    For Each oFlightNode As XmlNode In oNode.SelectNodes("OriginDestinationOption")
                        ' *******************
                        ' *******************
                        ' Decode Airports   *
                        ' *******************
                        oFlightNode.SelectSingleNode("OriginLocation").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oFlightNode.SelectSingleNode("OriginLocation").Attributes("LocationCode").Value)
                        'GetDecodeValue(ttAirports, oFlightNode.SelectSingleNode("OriginLocation").Attributes("LocationCode").Value)
                        oFlightNode.SelectSingleNode("DestinationLocation").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airport, oFlightNode.SelectSingleNode("DestinationLocation").Attributes("LocationCode").Value)
                        'GetDecodeValue(ttAirports, oFlightNode.SelectSingleNode("DestinationLocation").Attributes("LocationCode").Value)

                        ' *******************
                        ' Decode Airlines   *
                        ' *******************
                        If Not oFlightNode.SelectSingleNode("MarketingAirline") Is Nothing Then
                            oFlightNode.SelectSingleNode("MarketingAirline").InnerText = TripXMLLoad.DecodeValue(TripXMLLoad.DecodingType.Airline, oFlightNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                            'GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                        End If
                    Next
                Next

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsTXMLGetDeals", "Error *** Decoding GetDeals Response", ex.Message, String.Empty)
            End Try
            Return strResponse
        End Function

#End Region

#Region " Process Service Request All GDS "
        Private sb As StringBuilder = New StringBuilder()

        Private Function ServiceRequest(ByVal strRequest As String, ByVal ttServiceID As ttServices) As String
            Dim strResponse As String = ""
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim ValidateXSDOut As Boolean
            Dim StartTime As Date
            Dim UUID As String = ""

            Try
                StartTime = Now
                strRequest = strRequest.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://tripxml.downtowntravel.com/tripxml/wsQueue""", "")
                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, StartTime, ttServiceID, Server.MachineName, UUID)
                ValidateXSDOut = CBool(Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString()))
                sb.Remove(0, sb.Length())

                strResponse = GetDeals(strRequest)

                strResponse = DecodeTXMLGetDeals(strResponse, ttCredential.UserID)

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsTXMLGetDeals", "============= TXML Response ============= ", strResponse, String.Empty)
            End Try

            Return strResponse
            sb = Nothing
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()>
        <WebMethod(Description:="Process Get Deals Messages Request.")>
        <System.Web.Services.Protocols.SoapHeader("tXML")>
        Public Function wmGetDeals(ByVal TXML_GetLeadsRQ As wmGetDealsIn.TXML_GetLeadsRQ) As <XmlElementAttribute("TXML_GetDealsRS")> wmGetDealsOut.TXML_GetDealsRS
            Dim xmlMessage As String = ""
            Dim oGetDealsRS As wmGetDealsOut.TXML_GetDealsRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmGetDealsIn.TXML_GetLeadsRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, TXML_GetLeadsRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.GetDeals)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmGetDealsOut.TXML_GetDealsRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oGetDealsRS = CType(oSerializer.Deserialize(oReader), wmGetDealsOut.TXML_GetDealsRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsGetDeals", "Error Deserialing TXML Response", ex.Message, String.Empty)
            End Try

            Return oGetDealsRS

        End Function

        <WebMethod(Description:="Process PNR Read Xml Messages Request.")>
        Public Function wmTXMLGetDealsXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.GetDeals)
        End Function

#End Region

    End Class

End Namespace
