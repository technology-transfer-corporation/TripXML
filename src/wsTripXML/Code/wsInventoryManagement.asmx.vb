Imports System
Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Data
Imports System.Text
Imports CompressionExtension

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement), _
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsInventoryManagement", _
        Name:="wsInventoryManagement", _
        Description:="A TripXML Web Service to work with inventory management.")> _
    Public Class wsInventoryManagement
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

        Private Function DecodeTXMLInventoryManagement(ByVal strResponse As String, ByVal UserID As String) As String
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

                For Each oNode In oRoot.SelectNodes("Deals/Deal")
                    For Each oFlightNode In oNode.SelectNodes("OriginDestinationOption")
                        ' *******************
                        ' *******************
                        ' Decode Airports   *
                        ' *******************
                        oFlightNode.SelectSingleNode("OriginLocation").InnerText = GetDecodeValue(ttAirports, oFlightNode.SelectSingleNode("OriginLocation").Attributes("LocationCode").Value)
                        oFlightNode.SelectSingleNode("DestinationLocation").InnerText = GetDecodeValue(ttAirports, oFlightNode.SelectSingleNode("DestinationLocation").Attributes("LocationCode").Value)

                        ' *******************
                        ' Decode Airlines   *
                        ' *******************
                        If Not oFlightNode.SelectSingleNode("MarketingAirline") Is Nothing Then
                            oFlightNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ttAirlines, oFlightNode.SelectSingleNode("MarketingAirline").Attributes("Code").Value)
                        End If
                    Next
                Next

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsTXMLInventoryManagement", "Error *** Decoding InventoryManagement Response", ex.Message, String.Empty)
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
                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, StartTime, ttServiceID, Server.MachineName, UUID)
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                Select Case ttCredential.Providers(0).Name

                    Case "SITA"

                        ''strResponse = SendOtherRequestSITA(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsTXMLInventoryManagement", "============= TXML Response ============= ", strResponse, UUID)
            End Try

            Return strResponse
            sb = Nothing
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()> _
        <WebMethod(Description:="Process Inventory Management Messages Request.")> _
        <System.Web.Services.Protocols.SoapHeader("tXML")> _
        Public Function wmInventoryManagement(ByVal TXML_InventoryManagementRQ As wmInventoryManagementIn.TXML_InventoryManagementRQ) As <XmlElementAttribute("TXML_InventoryManagementRS")> wmInventoryManagementOut.TXML_InventoryManagementRS
            Dim xmlMessage As String = ""
            Dim oInventoryManagementRS As wmInventoryManagementOut.TXML_InventoryManagementRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmInventoryManagementIn.TXML_InventoryManagementRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, TXML_InventoryManagementRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.InventoryManagement)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(Type:=GetType(wmInventoryManagementOut.TXML_InventoryManagementRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oInventoryManagementRS = CType(oSerializer.Deserialize(oReader), wmInventoryManagementOut.TXML_InventoryManagementRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsInventoryManagement", "Error Deserialing TXML Response", ex.Message, String.Empty)
            End Try

            Return oInventoryManagementRS

        End Function

        <WebMethod(Description:="Process Inventory Management Xml Messages Request.")> _
        Public Function wmTXMLInventoryManagementXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.InventoryManagement)
        End Function

#End Region

    End Class

End Namespace
