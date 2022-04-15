Imports System
Imports TripXMLMain
Imports System.Threading
Imports System.IO.File
Imports System.Xml
Imports System.Text
Imports System.Web.Configuration


Namespace wsTravelTalk


    Public Class cLogData
        Private mstrRequest As String = ""
        Private mstrResponse As String = ""

        Private sb As StringBuilder = New StringBuilder()

        Public Sub LogDataDeals(ByVal strRequest As String, ByVal strResponse As String)

            mstrRequest = strRequest
            mstrResponse = strResponse

            If WebConfigurationManager.AppSettings("DataDatabase") <> Nothing Then
                Dim oLofThread As New Thread(New ThreadStart(AddressOf LogDeals))

                oLofThread.Start()
            End If
            
        End Sub

        Private Sub LogDeals()
            Dim oDA As cDA = Nothing

            Try
                oDA = New cDA("DataDatabase")
                oDA.AddDeals(mstrRequest, mstrResponse)

            Catch ex As Exception
            Finally
                If Not oDA Is Nothing Then
                    oDA.Dispose()
                End If
            End Try

        End Sub

        Public Function GetDataDeals(ByVal strRequest As String) As String
            Dim oDA As cDA = Nothing
            Dim strResponse As String = ""

            Try
                If WebConfigurationManager.AppSettings("DataDatabase") <> Nothing Then
                    oDA = New cDA("DataDatabase")
                    strResponse = oDA.GetDeals(strRequest)
                End If

                Return strResponse

            Catch ex As Exception
            Finally
                If Not oDA Is Nothing Then
                    oDA.Dispose()
                End If
            End Try
            Return String.Empty
        End Function

    End Class

End Namespace


