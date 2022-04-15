Imports System
Imports System.Web.Services
Imports System.Text


Namespace wsTravelTalk


    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsRefreshMem", _
        Name:="wsRefreshMem", _
        Description:="A TripXML Web Service to Refresh Application Variables.")> _
    Public Class wsRefreshMem
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
        Private sb As StringBuilder = New StringBuilder()

        <WebMethod(Description:="Refresh Application Variables.")> _
        Public Function wmRefreshMem() As String
            Try
                TripXMLStartUp(Application)
                Return "Succes. Application Variables Reloaded."
            Catch ex As Exception
                Return sb.Append("Error Reloading Application Variables. ").Append(ex.Message).ToString()
            End Try
        End Function

    End Class

End Namespace
