Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml.Serialization

Imports wsTripXML.wsTravelTalk.wmCancelVirtualCardLoad
Imports wsTripXML.wsTravelTalk.wmDeleteVirtualCard
Imports wsTripXML.wsTravelTalk.wmGenerateVirtualCard
Imports wsTripXML.wsTravelTalk.wmGetVirtualCardDetails
Imports wsTripXML.wsTravelTalk.wmListVirtualCards

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement), _
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/wsVirtualCreditCard", _
                                       Name:="wsVirtualCreditCard", _
                                       Description:="A TripXML Web Service to Process Virtual Card Requests.")> _
    Public Class wsVirtualCreditCard
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
                validateXSDOut = Application.Get($"XSD{ttCredential.UserID}Out")

                Select Case ttCredential.Providers(0).Name.ToLower()

                    Case "amadeusws"
                        strResponse = SendPaymentRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                    Case Else
                        Dim ttDefProvider As TripXMLProviderSystems = Nothing
                        PreServiceRequest(strRequest, Application, ttCredential, ttDefProvider, startTime, ttServiceID, Server.MachineName, uuid, "", True)
                        strResponse = SendPaymentRequestAmadeusWS(ttServiceID, ttCredential, ttDefProvider, strRequest)
                        'Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                End Select

                PostServiceRequest(strResponse, validateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, startTime, ttServiceID, Server.MachineName, uuid)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsGenerateVirtualCard", "============= OTA Response ============= ", strResponse, uuid)
            End Try

            Return strResponse
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension> _
        <WebMethod(Description:="Process Generate Virtual Card Messages Request.")> _
        <Protocols.SoapHeader("tXML")> _
        Public Function GenerateVirtualCard(ByVal PAY_GenerateVirtualCardRQ As PAY_GenerateVirtualCardRQ) As <XmlElementAttribute("PAY_GenerateVirtualCardRS")> PAY_GenerateVirtualCardRS
            Dim xmlMessage
            Dim oVCCRS As PAY_GenerateVirtualCardRS = Nothing
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            oSerializer = New XmlSerializer(GetType(PAY_GenerateVirtualCardRQ))
            oWriter = New IO.StringWriter(New StringBuilder)
            oSerializer.Serialize(oWriter, PAY_GenerateVirtualCardRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
            xmlMessage = xmlMessage.Replace(" xmlns=""http://www.opentravel.org/OTA/2003/05""", "")
            xmlMessage = xmlMessage.Replace(" xmlns=""http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.GenerateVirtualCard)

            Try
                oSerializer = New XmlSerializer(Type:=GetType(PAY_GenerateVirtualCardRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oVCCRS = CType(oSerializer.Deserialize(oReader), PAY_GenerateVirtualCardRS)
                
            Catch ex As Exception
                CoreLib.SendTrace("", "GenerateVirtualCard", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oVCCRS

        End Function

        <WebMethod(Description:="Process Generate Virtual Card Xml Messages Request.")> _
        Public Function GenerateVirtualCardXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.GenerateVirtualCard)
        End Function

        <CompressionExtension.CompressionExtension> _
        <WebMethod(Description:="Process Cancel Virtual Card Load Messages Request.")> _
        <Protocols.SoapHeader("tXML")> _
        Public Function CancelVirtualCardLoad(ByVal PAY_CancelVirtualCardLoadRQ As PAY_CancelVirtualCardLoadRQ) As <XmlElementAttribute("PAY_CancelVirtualCardLoadRS")> PAY_CancelVirtualCardLoadRS
            Dim xmlMessage
            Dim oVCCRS As PAY_CancelVirtualCardLoadRS = Nothing
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            oSerializer = New XmlSerializer(GetType(PAY_CancelVirtualCardLoadRQ))
            oWriter = New IO.StringWriter(New StringBuilder)
            oSerializer.Serialize(oWriter, PAY_CancelVirtualCardLoadRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
            xmlMessage = xmlMessage.Replace(" xmlns=""http://www.opentravel.org/OTA/2003/05""", "")
            xmlMessage = xmlMessage.Replace(" xmlns=""http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CancelVirtualCardLoad)

            Try
                oSerializer = New XmlSerializer(Type:=GetType(PAY_CancelVirtualCardLoadRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oVCCRS = CType(oSerializer.Deserialize(oReader), PAY_CancelVirtualCardLoadRS)
                
            Catch ex As Exception
                CoreLib.SendTrace("", "CancelVirtualCardLoad", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oVCCRS

        End Function

        <WebMethod(Description:="Process Cancel Virtual Card Load Xml Messages Request.")> _
        Public Function CancelVirtualCardLoadXML(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.CancelVirtualCardLoad)
        End Function

        <CompressionExtension.CompressionExtension> _
        <WebMethod(Description:="Process Delete Virtual Card Messages Request.")> _
        <Protocols.SoapHeader("tXML")> _
        Public Function DeleteVirtualCard(ByVal PAY_DeleteVirtualCardRQ As PAY_DeleteVirtualCardRQ) As <XmlElementAttribute("PAY_DeleteVirtualCardRS")> PAY_DeleteVirtualCardRS
            Dim xmlMessage
            Dim oVCCRS As PAY_DeleteVirtualCardRS = Nothing
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            oSerializer = New XmlSerializer(GetType(PAY_DeleteVirtualCardRQ))
            oWriter = New IO.StringWriter(New StringBuilder)
            oSerializer.Serialize(oWriter, PAY_DeleteVirtualCardRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
            xmlMessage = xmlMessage.Replace(" xmlns=""http://www.opentravel.org/OTA/2003/05""", "")
            xmlMessage = xmlMessage.Replace(" xmlns=""http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.DeleteVirtualCard)

            Try
                oSerializer = New XmlSerializer(Type:=GetType(PAY_DeleteVirtualCardRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oVCCRS = CType(oSerializer.Deserialize(oReader), PAY_DeleteVirtualCardRS)
                
            Catch ex As Exception
                CoreLib.SendTrace("", "DeleteVirtualCard", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oVCCRS

        End Function

        <WebMethod(Description:="Process Delete Virtual Card Xml Messages Request.")> _
        Public Function DeleteVirtualCardXML(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.DeleteVirtualCard)
        End Function

        <CompressionExtension.CompressionExtension> _
        <WebMethod(Description:="Process Delete Virtual Card Messages Request.")> _
        <Protocols.SoapHeader("tXML")> _
        Public Function GetVirtualCardDetails(ByVal PAY_GetVirtualCardDetailsRQ As PAY_GetVirtualCardDetailsRQ) As <XmlElementAttribute("PAY_GetVirtualCardDetailsRS")> PAY_GetVirtualCardDetailsRS
            Dim xmlMessage
            Dim oVCCRS As PAY_GetVirtualCardDetailsRS = Nothing
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            oSerializer = New XmlSerializer(GetType(PAY_GetVirtualCardDetailsRQ))
            oWriter = New IO.StringWriter(New StringBuilder)
            oSerializer.Serialize(oWriter, PAY_GetVirtualCardDetailsRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
            xmlMessage = xmlMessage.Replace(" xmlns=""http://www.opentravel.org/OTA/2003/05""", "")
            xmlMessage = xmlMessage.Replace(" xmlns=""http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.GetVirtualCardDetails)

            Try
                oSerializer = New XmlSerializer(Type:=GetType(PAY_GetVirtualCardDetailsRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oVCCRS = CType(oSerializer.Deserialize(oReader), PAY_GetVirtualCardDetailsRS)
                
            Catch ex As Exception
                CoreLib.SendTrace("", "GetVirtualCardDetails", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oVCCRS

        End Function

        <WebMethod(Description:="Process Delete Virtual Card Xml Messages Request.")> _
        Public Function GetVirtualCardDetailsXML(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.GetVirtualCardDetails)
        End Function

        <CompressionExtension.CompressionExtension> _
        <WebMethod(Description:="Process List Virtual Credit Cards Request.")> _
        <Protocols.SoapHeader("tXML")> _
        Public Function ListVirtualCards(ByVal PAY_ListVirtualCardsRQ As PAY_ListVirtualCardsRQ) As <XmlElementAttribute("PAY_ListVirtualCardsRS")> PAY_ListVirtualCardsRS
            Dim xmlMessage
            Dim oVCCRS As PAY_ListVirtualCardsRS = Nothing
            Dim oSerializer As XmlSerializer
            Dim oWriter As IO.StringWriter
            Dim oReader As IO.StringReader

            oSerializer = New XmlSerializer(GetType(PAY_ListVirtualCardsRQ))
            oWriter = New IO.StringWriter(New StringBuilder)
            oSerializer.Serialize(oWriter, PAY_ListVirtualCardsRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
            xmlMessage = xmlMessage.Replace(" xmlns=""http://www.opentravel.org/OTA/2003/05""", "")
            xmlMessage = xmlMessage.Replace(" xmlns=""http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.ListVirtualCards)

            Try
                oSerializer = New XmlSerializer(Type:=GetType(PAY_ListVirtualCardsRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oVCCRS = CType(oSerializer.Deserialize(oReader), PAY_ListVirtualCardsRS)
                
            Catch ex As Exception
                CoreLib.SendTrace("", "ListVirtualCards", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oVCCRS

        End Function

        <WebMethod(Description:="Process List Virtual Credit Cards Xml Request.")> _
        Public Function ListVirtualCardsXML(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.ListVirtualCards)
        End Function

#End Region

    End Class
End Namespace
