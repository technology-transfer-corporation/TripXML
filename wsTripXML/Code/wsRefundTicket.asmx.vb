﻿Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Web.Services.Protocols
Imports TripXMLMain.modCore
Imports System.Text
Imports CompressionExtension

Namespace wsTravelTalk

    <System.Web.Services.Protocols.SoapDocumentService(RoutingStyle:=System.Web.Services.Protocols.SoapServiceRoutingStyle.RequestElement), _
        System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsRefundTicket", _
        Name:="wsRefundTicket", _
        Description:="A TripXML Web Service to Process Refund Ticket Messages Request.")> _
    Public Class wsRefundTicket
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

#Region " Decode Functions "

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

                Select Case ttCredential.Providers(0).Name.ToLower()

                    Case "amadeusws"

                        strResponse = SendTravelRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                'Dim StartCounter As Date
                'StartCounter = Now
                ' Not Implemented DecodeIssueTicket(strResponse, ttCredential.UserID)
                'CoreLib.SendTrace(ttCredential.UserID, "Performance", "Decoding = ").Append(CType(Now.Subtract(StartCounter).TotalMilliseconds, Integer), "")

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsRefundTicket", "============= OTA Response ============= ", strResponse, UUID)
            End Try

            Return strResponse
            sb = Nothing
        End Function

#End Region

#Region " Web Methods "

        <CompressionExtension.CompressionExtension()> _
        <WebMethod(Description:="Process Refund Ticket Messages Request.")> _
        <System.Web.Services.Protocols.SoapHeader("tXML")> _
        Public Function wmRefundTicket(ByVal TT_RefundTicketRQ As wmRefundTicketIn.TT_RefundTicketRQ) As <XmlElementAttribute("TT_RefundTicketRS")> wmRefundTicketOut.TT_RefundTicketRS
            Dim xmlMessage As String = ""
            Dim oRefundTicketRS As wmRefundTicketOut.TT_RefundTicketRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmRefundTicketIn.TT_RefundTicketRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, TT_RefundTicketRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.RefundTicket)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(Type:=GetType(wmRefundTicketOut.TT_RefundTicketRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oRefundTicketRS = CType(oSerializer.Deserialize(oReader), wmRefundTicketOut.TT_RefundTicketRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsRefundTicket", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oRefundTicketRS

        End Function

        <WebMethod(Description:="Process Refund Ticket Xml Messages Request.")> _
        Public Function wmRefundTicketXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.RefundTicket)
        End Function

#End Region

    End Class

End Namespace
