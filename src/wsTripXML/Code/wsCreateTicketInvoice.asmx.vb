Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports TripXMLMain
Imports AdminTools
Imports System.Xml
Imports System.IO
Imports System.Configuration

Namespace wsTravelTalk

    ' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    ' <System.Web.Script.Services.ScriptService()> _
    <System.Web.Services.WebService(Namespace:="http://tripxml.com/wsUpdateMarkups")> _
    <System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <ToolboxItem(False)> _
    Public Class wsCreateTicketInvoice
        Inherits System.Web.Services.WebService
        Private sb As StringBuilder = New StringBuilder()

        Private Function ServiceRequest(ByVal strRequest As String) As String
            Dim strResponse As String = ""

            'strResponse = AdminStatusManager.Program.CreateTicket(strRequest)

            Return strResponse
            sb = Nothing
        End Function

        <WebMethod(Description:="Update Markups.")> _
        Public Function CreateTicketInvoice(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest)
        End Function

    End Class
End Namespace