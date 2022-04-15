Imports System
Imports System.Web.Services
Imports System.ComponentModel
Imports TripXMLMain
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    ' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    ' <System.Web.Script.Services.ScriptService()> _
    <System.Web.Services.WebService(Namespace:="http://tripxml.com/wsAdmin")> _
    <System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <ToolboxItem(False)> _
    Public Class wsAdmin
        Inherits System.Web.Services.WebService
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
                    Case "Amadeus"
                        'Dim ttAA As AmadeusAPIAdapter

                        'ttAA = Application.Get(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                        'sb.Remove(0, sb.Length())
                        'If ttAA Is Nothing Then
                        '    Throw New Exception(sb.Append("Access denied to Amadeus - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString())
                        '    sb.Remove(0, sb.Length())
                        'End If

                        'If ttCredential.Providers(0).PCC.Trim.Length > 0 Then
                        '    ttAA.SourcePCC = ttCredential.Providers(0).PCC
                        'End If

                        'strResponse = SendTravelRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                        'Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                        'sb.Remove(0, sb.Length())

                    Case "AmadeusWS"

                        If ttServiceID = ttServices.AddPNRToAdmin Then
                            strResponse = SendTravelRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                        Else
                            strResponse = SendPNRRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                        End If
                    Case "Apollo", "Galileo"

                        strResponse = SendTravelRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Sabre"

                        'ttProviderSystems = Application.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                        'sb.Remove(0, sb.Length())
                        If ttProviderSystems.System Is Nothing Then
                            FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                            sb.Remove(0, sb.Length())
                            Exit Select
                        End If

                        ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC

                        strResponse = SendTravelRequestSabre(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case "Worldspan"

                        strResponse = SendTravelRequestWorldspan(ttServiceID, ttCredential, ttProviderSystems, strRequest)
                    Case "Travelport"

                        strResponse = SendTravelRequestTravelport(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsAddPNRToAdmin", "============= Response ============= ", strResponse, ttProviderSystems.LogUUID)
            End Try

            Return strResponse
            sb = Nothing
        End Function

        <WebMethod(Description:="Add a PNR to the Admin by TravelBuild response XML.")> _
        Public Function AddPNRToAdmin(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.AddPNRToAdmin)
        End Function

        <WebMethod(Description:="Add a PNR to the Admin by record locator.")> _
        Public Function AddRecLocToAdmin(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.AddRecLocToAdmin)
        End Function

        <WebMethod(Description:="Add a PNR to the Admin by record locator.")> _
        Public Function AddRecLocToNewAdminOnly(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.AddRecLocToNewAdminOnly)
        End Function

        <WebMethod(Description:="Update Markups.")> _
        Public Function UpdateMarkups(ByVal xmlRequest As String) As String
            Dim markUp As wsUpdateMarkups = New wsUpdateMarkups()
            Return markUp.UpdateMarkups(xmlRequest)
        End Function

        <WebMethod(Description:="Admin status management.")> _
        Public Function CreateTicketInvoice(ByVal xmlRequest As String) As String
            Dim tktInvoice As wsCreateTicketInvoice = New wsCreateTicketInvoice()
            Return tktInvoice.CreateTicketInvoice(xmlRequest)
        End Function

    End Class
End Namespace