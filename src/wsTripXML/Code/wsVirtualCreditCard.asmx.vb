Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml.Serialization

Imports TripXMLMain.modCore
Imports TripXML.Core.Models
Imports TripXML.Core.Models.Base
Imports TripXML.Core.Utils
Imports TripXML.Core.Enums
Imports wsTripXML.wsTravelTalk.wmCancelVirtualCardLoad

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement),
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/wsVirtualCreditCard",
                                       Name:="wsVirtualCreditCard",
                                       Description:="A TripXML Web Service to Process Virtual Card Requests.")>
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

        Private Function ServiceRequest(Of T)(ByVal request As VirtualCardRQBase, ByVal ttServiceID As Integer) As T
            Dim strResponse As String = ""
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim validateXSDOut As Boolean
            Dim startTime As Date
            Dim uuid As String = ""
            Dim responseObj As T = Nothing

            Try
                startTime = Now

                Dim strRequest As String = SerializeRequest(request)
                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, startTime, ttServiceID, Server.MachineName, uuid)

                Select Case request.BankSource
                    Case VirtualCardSourceType.ConnexPay
                        responseObj = SendPaymentRequest(ttServiceID, ttCredential, ttProviderSystems, request)
                        strResponse = TripXMLSerializer.Serialize(responseObj)
                    Case VirtualCardSourceType.USBank
                        Select Case ttCredential.Providers(0).Name.ToLower
                            Case "amadeusws"
                                strResponse = SendPaymentRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                                responseObj = TripXMLSerializer.Deserialize(Of T)(strResponse)
                            Case Else
                                Dim ttDefProvider As TripXMLProviderSystems = Nothing
                                PreServiceRequest(strRequest, Application, ttCredential, ttDefProvider, startTime, ttServiceID, Server.MachineName, uuid, "", True)

                                strResponse = SendPaymentRequestAmadeusWS(ttServiceID, ttCredential, ttDefProvider, strRequest)
                                responseObj = TripXMLSerializer.Deserialize(Of T)(strResponse)
                        End Select
                End Select

                validateXSDOut = Application.Get($"XSD{ttCredential.UserID}Out")

                PostServiceRequest(strResponse, validateXSDOut, ttServiceID, ttCredential.UserID)
            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, startTime, ttServiceID, Server.MachineName, uuid)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsGenerateVirtualCard", "============= OTA Response ============= ", strResponse, uuid)
            End Try

            Return responseObj
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension>
        <WebMethod(Description:="Process Generate Virtual Card Messages Request.")>
        <Protocols.SoapHeader("tXML")>
        Public Function GenerateVirtualCard(ByVal PAY_GenerateVirtualCardRQ As PAY_GenerateVirtualCardRQ) As <XmlElementAttribute("PAY_GenerateVirtualCardRS")> PAY_GenerateVirtualCardRS
            Dim oVCCRS As PAY_GenerateVirtualCardRS = Nothing

            Try
                oVCCRS = ServiceRequest(Of PAY_GenerateVirtualCardRS)(PAY_GenerateVirtualCardRQ, ttServices.GenerateVirtualCard)
            Catch ex As Exception
                CoreLib.SendTrace("", "GenerateVirtualCard", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oVCCRS
        End Function

        <CompressionExtension.CompressionExtension>
        <WebMethod(Description:="Process Cancel Virtual Card Load Messages Request.")>
        <Protocols.SoapHeader("tXML")>
        Public Function CancelVirtualCardLoad(ByVal PAY_CancelVirtualCardLoadRQ As PAY_CancelVirtualCardLoadRQ) As <XmlElementAttribute("PAY_CancelVirtualCardLoadRS")> PAY_CancelVirtualCardLoadRS
            Dim oVCCRS As PAY_CancelVirtualCardLoadRS = Nothing

            Try
                oVCCRS = ServiceRequest(Of PAY_CancelVirtualCardLoadRS)(PAY_CancelVirtualCardLoadRQ, ttServices.CancelVirtualCardLoad)
            Catch ex As Exception
                CoreLib.SendTrace("", "CancelVirtualCardLoad", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oVCCRS

        End Function

        <CompressionExtension.CompressionExtension>
        <WebMethod(Description:="Process Delete Virtual Card Messages Request.")>
        <Protocols.SoapHeader("tXML")>
        Public Function DeleteVirtualCard(ByVal PAY_DeleteVirtualCardRQ As PAY_DeleteVirtualCardRQ) As <XmlElementAttribute("PAY_DeleteVirtualCardRS")> PAY_DeleteVirtualCardRS
            Dim oVCCRS As PAY_DeleteVirtualCardRS = Nothing

            Try
                oVCCRS = ServiceRequest(Of PAY_DeleteVirtualCardRS)(PAY_DeleteVirtualCardRQ, ttServices.DeleteVirtualCard)
            Catch ex As Exception
                CoreLib.SendTrace("", "DeleteVirtualCard", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oVCCRS

        End Function

        <CompressionExtension.CompressionExtension>
        <WebMethod(Description:="Process Detail Virtual Card Messages Request.")>
        <Protocols.SoapHeader("tXML")>
        Public Function GetVirtualCardDetails(ByVal PAY_GetVirtualCardDetailsRQ As PAY_GetVirtualCardDetailsRQ) As <XmlElementAttribute("PAY_GetVirtualCardDetailsRS")> PAY_GetVirtualCardDetailsRS
            Dim oVCCRS As PAY_GetVirtualCardDetailsRS = Nothing

            Try
                oVCCRS = ServiceRequest(Of PAY_GetVirtualCardDetailsRS)(PAY_GetVirtualCardDetailsRQ, ttServices.GetVirtualCardDetails)

            Catch ex As Exception
                CoreLib.SendTrace("", "GetVirtualCardDetails", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oVCCRS

        End Function

        <CompressionExtension.CompressionExtension>
        <WebMethod(Description:="Process List Virtual Credit Cards Request.")>
        <Protocols.SoapHeader("tXML")>
        Public Function ListVirtualCards(ByVal PAY_ListVirtualCardsRQ As PAY_ListVirtualCardsRQ) As <XmlElementAttribute("PAY_ListVirtualCardsRS")> PAY_ListVirtualCardsRS
            Dim oVCCRS As PAY_ListVirtualCardsRS = Nothing

            Try
                oVCCRS = ServiceRequest(Of PAY_ListVirtualCardsRS)(PAY_ListVirtualCardsRQ, ttServices.ListVirtualCards)
            Catch ex As Exception
                CoreLib.SendTrace("", "ListVirtualCards", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oVCCRS

        End Function

#End Region


#Region "Serializer / Desirializer"
        Public Function SerializeRequest(ByVal request As Object) As String
            Try
                Dim oSerializer As XmlSerializer = New XmlSerializer(request.GetType())
                Dim oWriter As IO.StringWriter = New IO.StringWriter(New StringBuilder)
                oSerializer.Serialize(oWriter, request)
                Dim xmlMessage As String = oWriter.ToString
                xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
                xmlMessage = xmlMessage.Replace(" xmlns=""http://www.opentravel.org/OTA/2003/05""", "")
                xmlMessage = xmlMessage.Replace(" xmlns=""http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2""", "")

                Return xmlMessage
            Catch ex As Exception
                Return String.Empty
            End Try

        End Function
#End Region

    End Class
End Namespace
