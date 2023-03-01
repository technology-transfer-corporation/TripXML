Imports System.Web.Services
Imports TripXMLMain
Imports System.Xml
Imports System.Threading
Imports System.IO
Imports System.Xml.Serialization

Namespace wsTravelTalk


    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsImportLog",
        Name:="wsAppSettings",
        Description:="A TripXML Web Service to read App Settings from Web.config File.")>
    Public Class wsAppSettings
        Inherits System.Web.Services.WebService
        Property AppSettings As New AppSettings


#Region " Web Services Designer Generated Code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Web Services Designer.
            InitializeComponent()

            'Add your own initialization code after the InitializeComponent() call
            ReadConfigFile()
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

        Private Sub ReadConfigFile()
            Try
                CoreLib.SendTrace("", "wsAppSettings", $"Reading Log File {modCore.LogPath}{DateTime.Now.ToShortDateString()}_Log.txt", "", String.Empty)
                Dim keys As String() = ConfigurationManager.AppSettings.AllKeys
                Dim oXML As String = "<AppSettings>"
                For Each key In keys
                    oXML += $"<{key}>{ConfigurationManager.AppSettings(key)}</{key}>"
                Next
                oXML += "</AppSettings>"

                AppSettings = DeserializeXMLFileToObject(Of AppSettings)(oXML)
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        <WebMethod(Description:="Import Log from Log File. Progress is sent to the Trace.")>
        Public Function wsAppSettings() As String

            Try
                Dim oLofThread As New Thread(New ThreadStart(AddressOf ReadConfigFile))
                oLofThread.Start()
                Return "Read Config Process was started. Monitor the Trace Tool to view the progress."
            Catch ex As Exception
                Return $"Error Reading App Settings. {ex.Message}"
            End Try
        End Function


        Public Shared Function DeserializeXMLFileToObject(Of T)(ByVal XmlFilename As String) As T
            Dim returnObject As T = Nothing
            If String.IsNullOrEmpty(XmlFilename) Then Return Nothing

            Try
                Dim doc As New XmlDocument()
                doc.LoadXml(XmlFilename)

                Dim serializer As XmlSerializer = New XmlSerializer(GetType(T))
                Using rdr As New StringReader(doc.InnerXml)
                    returnObject = CType(serializer.Deserialize(rdr), T)
                End Using
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            Return returnObject
        End Function
    End Class

End Namespace
