Imports System.Web.Services
Imports System.IO.File
Imports TripXMLMain
Imports System.Xml
Imports System.Threading


Namespace wsTravelTalk


    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsImportLog", _
        Name:="wsImportLog", _
        Description:="A TripXML Web Service to Import Log from Log File.")> _
    Public Class wsImportLog
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

        Private Sub ImportLog()
            Dim oLog As cLog = Nothing

            Try

                oLog = New cLog

                oLog.ImportLog()

            Catch ex As Exception
                CoreLib.SendTrace("", "wsImportLog", "Error Importing Log.", ex.Message, String.Empty)
            Finally

            End Try
        End Sub

        Private Function ViewErrorLog() As String
            Dim xmlLog As String = ""
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing

            Try

                xmlLog = ReadLogFile(False)

                oDoc = New XmlDocument
                oDoc.LoadXml(xmlLog)
                oRoot = oDoc.DocumentElement

                xmlLog = "<ErrorLog>"

                For Each oNode In oRoot.SelectNodes("Line[LogType='1']")
                    xmlLog &= oNode.SelectSingleNode("ExError").OuterXml
                    xmlLog &= oNode.SelectSingleNode("MessageID").OuterXml
                    xmlLog &= oNode.SelectSingleNode("Message").OuterXml
                    xmlLog &= oNode.SelectSingleNode("MessageDate").OuterXml
                Next

                Return sb.Append(xmlLog).Append("</ErrorLog>").ToString()

            Catch ex As Exception
                Throw ex
            Finally

            End Try
            sb = Nothing
        End Function

        Private Function ReadLogFile(ByVal DeleteLog As Boolean) As String
            Dim FileNumber As Integer
            Dim strLine As String = ""
            Dim i As Integer
            Dim sb2 As StringBuilder = New StringBuilder()
            Try
                CoreLib.SendTrace("", "wsImportLog", sb.Append("Reading Log File ").Append(modCore.LogPath).Append(String.Format("{0}_Log.txt", DateTime.Now.ToShortDateString())).ToString(), "", String.Empty)
                sb.Remove(0, sb.Length())

                If Exists(sb.Append(modCore.LogPath).Append(String.Format("{0}_Log.txt", DateTime.Now.ToShortDateString())).ToString()) Then
                    sb.Remove(0, sb.Length())
                    If DeleteLog Then
                        Move(sb.Append(modCore.LogPath).Append(String.Format("{0}_Log.txt", DateTime.Now.ToShortDateString())).ToString(), sb2.Append(modCore.LogPath).Append("LogImport.txt").ToString())
                        sb.Remove(0, sb.Length())
                        sb2.Remove(0, sb2.Length())
                    Else
                        Copy(sb.Append(modCore.LogPath).Append(String.Format("{0}_Log.txt", DateTime.Now.ToShortDateString())).ToString(), sb2.Append(modCore.LogPath).Append("LogImport.txt").ToString(), True)
                        sb.Remove(0, sb.Length())
                        sb2.Remove(0, sb2.Length())
                    End If
                Else
                    Throw New Exception(sb.Append("Log File ").Append(modCore.LogPath).Append("Log.txt Not found.").ToString())
                End If

                FileNumber = FreeFile()

                FileOpen(FileNumber, sb.Append(modCore.LogPath).Append("LogImport.txt").ToString(), OpenMode.Input, OpenAccess.Read, OpenShare.LockWrite)
                sb.Remove(0, sb.Length())

                Do While Not EOF(FileNumber)
                    strLine &= LineInput(FileNumber)
                    i += 1
                    If i > 400 Then Exit Do
                Loop

                Return sb.Append("<LogFile>").Append(strLine).Append("</LogFile>").ToString()

            Catch ex As Exception
                Throw ex
            Finally
                FileClose(FileNumber)
                sb.Remove(0, sb.Length())
                If Exists(sb.Append(modCore.LogPath).Append("LogImport.txt").ToString()) Then
                    sb.Remove(0, sb.Length())
                    Delete(sb.Append(modCore.LogPath).Append("LogImport.txt").ToString())
                    sb.Remove(0, sb.Length())
                End If
            End Try
            sb = Nothing
        End Function

        <WebMethod(Description:="Import Log from Log File. Progress is sent to the Trace.")> _
        Public Function wmImportLog() As String

            Try
                Dim oLofThread As New Thread(New ThreadStart(AddressOf ImportLog))

                oLofThread.Start()

                Return "Import Log Process was started. Monitor the Trace Tool to view the progress."

            Catch ex As Exception
                Return sb.Append("Error Importing Log File. ").Append(ex.Message).ToString()
                sb.Remove(0, sb.Length())
            End Try
            sb = Nothing
        End Function

        <WebMethod(Description:="View the first 200 errors Log from Log File.")> _
        Public Function wmViewErrorLog() As String

            Try
                Return ViewErrorLog()
            Catch ex As Exception
                Return sb.Append("Error Viewing Log File. ").Append(ex.Message).ToString()
                sb.Remove(0, sb.Length())
            End Try
            sb = Nothing
        End Function

    End Class

End Namespace
