Imports System.Web.Services.Protocols
Imports System.Threading
Imports TripXMLMain


Namespace wsTravelTalk


    Public Class cSoapRQ
        Inherits SoapExtension

        Private mstrSoapEnvelope As String = ""
        Private mstrSoapException As String = ""
        Private sr As System.IO.StreamReader

        Public Overloads Overrides Function GetInitializer(ByVal serviceType As System.Type) As Object
            Return Nothing
        End Function

        Public Overloads Overrides Function GetInitializer(ByVal methodInfo As System.Web.Services.Protocols.LogicalMethodInfo, ByVal attribute As System.Web.Services.Protocols.SoapExtensionAttribute) As Object
            Return Nothing
        End Function

        Public Overrides Sub Initialize(ByVal initializer As Object)

        End Sub

        Public Overrides Sub ProcessMessage(ByVal message As System.Web.Services.Protocols.SoapMessage)
            Try
                Select Case message.Stage
                    Case SoapMessageStage.BeforeDeserialize
                        GetSoapEnvelope(message)
                    Case SoapMessageStage.AfterSerialize
                        If Not sr Is Nothing Then
                            sr.Close()
                            sr = Nothing
                        End If
                        GC.Collect()
                End Select

                If Not (message.Exception Is Nothing) Then

                    mstrSoapException = message.Exception.Message

                    Dim oLofThread As New Thread(New ThreadStart(AddressOf LogSoapException))

                    oLofThread.Start()

                    Throw message.Exception

                End If
            Catch ex As SoapException
                Throw ex
            End Try

        End Sub

        Public Sub GetSoapEnvelope(ByRef myMessage As SoapMessage)

            Try
                sr = New System.IO.StreamReader(myMessage.Stream)

                If sr.BaseStream.CanSeek Then
                    If sr.BaseStream.CanRead Then mstrSoapEnvelope = sr.ReadToEnd
                    myMessage.Stream.Position = 0
                End If

            Finally

            End Try

        End Sub

        Private Sub LogSoapException()
            Dim oDA As cDA = Nothing

            Try
                oDA = New cDA

                oDA.AddSoapException(mstrSoapException, mstrSoapEnvelope)

            Catch ex As Exception
                LogSoapExceptionToFile(mstrSoapException, mstrSoapEnvelope, ex.Message)
            Finally
                If Not oDA Is Nothing Then
                    oDA.Dispose()
                    oDA = Nothing
                End If
            End Try

        End Sub

    End Class

End Namespace
