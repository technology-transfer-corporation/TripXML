Imports System
Imports TripXMLMain
Imports System.Threading
Imports System.IO.File
Imports System.Text
Imports TripXMLMain.modCore

Namespace wsTravelTalk


    Public Class cLog
        Private mintLogType As Integer
        Private mstrServerName As String = ""
        Private mstrCustomer As String = ""
        Private mstrUUID As String = ""
        Private mstrUserName As String = ""
        Private mstrProvider As String = ""
        Private mintMessageID As Integer
        Private mstrMessage As String = ""
        Private mdtMessageDate As Date
        Private mintResponseTime As Integer
        Private sb As StringBuilder = New StringBuilder()

        Public Function LogRequest(ByVal ServerName As String, ByVal Customer As String, ByVal UserName As String, _
                                    ByVal Provider As String, ByVal MessageID As Integer, ByRef Message As String, _
                                    ByVal StartTime As Date) As String


            Try
                mstrUUID = CreateUUID()

                mintLogType = enLogType.Request
                mstrServerName = ServerName
                mstrCustomer = Customer
                mstrUserName = UserName
                mstrProvider = Provider
                mintMessageID = MessageID
                mstrMessage = Message
                mdtMessageDate = StartTime
                mintResponseTime = 0

                Dim oLofThread As New Thread(New ThreadStart(AddressOf LogMessage))

                oLofThread.Start()

            Catch ex As Exception

            End Try

            Return mstrUUID

        End Function

        Public Sub LogResponse(ByVal UUID As String, ByRef ServerName As String, ByVal Customer As String, ByVal UserName As String, _
                                ByVal Provider As String, ByVal MessageID As Integer, ByRef Message As String, ByVal StartTime As Date)

            mdtMessageDate = Now
            mintLogType = enLogType.Response
            mstrUUID = UUID
            mstrServerName = ServerName
            mstrCustomer = Customer
            mstrUserName = UserName
            mstrProvider = Provider
            mintMessageID = MessageID
            mstrMessage = Message
            mintResponseTime = CType(mdtMessageDate.Subtract(StartTime).TotalMilliseconds, Integer)

            Dim oLofThread As New Thread(New ThreadStart(AddressOf LogMessage))

            oLofThread.Start()

        End Sub

        Private Sub LogMessage()
            Dim oDA As cDA = Nothing
            Dim arAttributes As String()

            If mstrUUID Is Nothing Then mstrUUID = ""
            If mstrCustomer Is Nothing Then mstrCustomer = ""
            If mstrServerName Is Nothing Then mstrServerName = ""
            If mstrUserName Is Nothing Then mstrUserName = ""
            If mstrProvider Is Nothing Then mstrProvider = ""

            Select Case mintMessageID
                Case ttServices.TravelBuild, ttServices.CruiseCreateBooking
                    arAttributes = New String() {"CardNumber", "BankAcctNumber", "DocID"}
                    CoreLib.MaskPrivateData(mstrMessage, arAttributes)
            End Select

            Try
                If mstrUUID.Length = 0 Then
                    LogMessageToFile(mintLogType, mstrUUID, mstrServerName, mstrCustomer, mstrUserName, mstrProvider, mintMessageID, mstrMessage, mdtMessageDate, mintResponseTime, "UUID is Missing")
                    Exit Try
                ElseIf mstrCustomer.Length = 0 Then
                    LogMessageToFile(mintLogType, mstrUUID, mstrServerName, mstrCustomer, mstrUserName, mstrProvider, mintMessageID, mstrMessage, mdtMessageDate, mintResponseTime, "Customer is Missing")
                    Exit Try
                End If

                oDA = New cDA

                oDA.AddMessageLog(mintLogType, mstrUUID, mstrServerName, mstrCustomer, mstrUserName, mstrProvider, mintMessageID, mstrMessage, mdtMessageDate, mintResponseTime)

            Catch ex As Exception
                LogMessageToFile(mintLogType, mstrUUID, mstrServerName, mstrCustomer, mstrUserName, mstrProvider, mintMessageID, mstrMessage, mdtMessageDate, mintResponseTime, ex.Message)
            Finally
                If Not oDA Is Nothing Then
                    oDA.Dispose()
                    oDA = Nothing
                End If
            End Try

        End Sub

        Private Function GetNodeInnerText(ByRef xmlData As String, ByVal sNode As String) As String
            Dim intStart As Integer
            Dim intLength As Integer

            If xmlData.IndexOf(sb.Append("<").Append(sNode).Append(">").ToString()) = -1 Then
                sb.Remove(0, sb.Length())
                Return ""
            End If
            sb.Remove(0, sb.Length())
            intStart = xmlData.IndexOf(sb.Append("<").Append(sNode).Append(">").ToString()) + sNode.Length + 2
            sb.Remove(0, sb.Length())

            intLength = xmlData.IndexOf(sb.Append("</").Append(sNode).Append(">").ToString()) - intStart
            sb.Remove(0, sb.Length())

            Return xmlData.Substring(intStart, intLength).Replace(vbCr, "").Replace(vbLf, "").Trim
            sb = Nothing
        End Function

        Public Sub ImportLog()
            Dim fileNumber As Integer
            Dim strLine As String
            Dim oDA As cDA = Nothing
            Dim startCounter As Date
            Dim strLogType As String = ""

            Dim logType As Integer
            Dim webServer As String
            Dim customer As String
            Dim UUID As String
            Dim userName As String
            Dim provider As String
            Dim messageID As Integer
            Dim message As String
            Dim messageDate As Date
            Dim responseTime As Integer
            Dim recLoc As String
            Dim flightDate As String
            Dim intStart As Integer
            Dim intLength As Integer
            Dim sb2 As StringBuilder = New StringBuilder()
            Try
                If Exists(sb.Append(LogPath).Append(String.Format("{0}_Log.txt", DateTime.Now.ToShortDateString())).ToString()) Then
                    sb.Remove(0, sb.Length())
                    Move(sb.Append(LogPath).Append(String.Format("{0}_Log.txt", DateTime.Now.ToShortDateString())).ToString(), sb2.Append(LogPath).Append("LogImport.txt").ToString())
                    sb.Remove(0, sb.Length())
                    sb2.Remove(0, sb2.Length())
                Else
                    sb.Remove(0, sb.Length())
                    Throw New Exception(sb.Append("Log File ").Append(LogPath).Append("Log.txt Not found.").ToString())
                End If

                fileNumber = FreeFile()

                FileOpen(fileNumber, sb.Append(LogPath).Append("LogImport.txt").ToString(), OpenMode.Input, OpenAccess.Read, OpenShare.LockWrite)
                sb.Remove(0, sb.Length())

                oDA = New cDA

                Do While Not EOF(fileNumber)
                    strLine = LineInput(fileNumber)

                    logType = CType(GetNodeInnerText(strLine, "LogType"), Integer)
                    UUID = GetNodeInnerText(strLine, "UUID")
                    webServer = GetNodeInnerText(strLine, "WebServer")
                    customer = GetNodeInnerText(strLine, "Customer")
                    userName = GetNodeInnerText(strLine, "UserName")
                    provider = GetNodeInnerText(strLine, "Provider")
                    messageID = CType(GetNodeInnerText(strLine, "MessageID"), Integer)
                    message = GetNodeInnerText(strLine, "Message")
                    messageDate = CType(GetNodeInnerText(strLine, "MessageDate"), Date)
                    responseTime = CType(GetNodeInnerText(strLine, "ResponseTime"), Integer)

                    Select Case logType
                        Case enLogType.Request
                            strLogType = "Log Request = "
                        Case enLogType.Response
                            strLogType = "Log Response = "
                    End Select

                    Try
                        startCounter = Now

                        oDA.AddMessageLog(logType, UUID, webServer, customer, userName, provider, messageID, message, messageDate, responseTime)

                        CoreLib.SendTrace("", "cLog", sb.Append("Logging Entry from Log File ").Append(strLogType).Append(CType(Now.Subtract(startCounter).TotalMilliseconds, Integer).ToString).ToString(), "", String.Empty)
                        sb.Remove(0, sb.Length())

                        If logType = enLogType.Response And provider.IndexOf("Production") > 0 Then
                            Select Case messageID
                                Case ttServices.TravelBuild
                                    ' Import New Bookings
                                    If message.IndexOf("<Success />") > 0 Then
                                        intStart = message.IndexOf("ID=") + 4
                                        intLength = message.IndexOf(">", intStart) - (intStart + 1)
                                        recLoc = message.Substring(intStart, intLength)

                                        intStart = message.IndexOf("DepartureDateTime=") + 19
                                        flightDate = message.Substring(intStart, 10)

                                        oDA.ImportBooking(customer, userName, recLoc, CType(flightDate, Date))

                                    End If
                                Case ttServices.PNRCancel
                                    ' Cancel Bookings
                                    If message.IndexOf("<Success />") > 0 Then
                                        intStart = message.IndexOf("<UniqueID ID=") + 14
                                        intLength = message.IndexOf(">", intStart) - (intStart + 1)
                                        recLoc = message.Substring(intStart, intLength)

                                        oDA.UpdateBookingStatus(recLoc, "C")

                                    End If
                            End Select
                        End If

                    Catch exx As Exception
                        CoreLib.SendTrace("", "cLog", sb.Append("Error Logging Entry from Log File ").Append(strLogType).Append(CType(Now.Subtract(startCounter).TotalMilliseconds, Integer).ToString).ToString(), exx.Message, String.Empty)
                        sb.Remove(0, sb.Length())
                    End Try

                Loop

                CoreLib.SendTrace("", "cLog", sb.Append("Log File Imported. ").Append(Now.ToString).ToString(), "", String.Empty)
                sb.Remove(0, sb.Length())

            Catch ex As Exception
                CoreLib.SendTrace("", "cLog", "Error Importing Log File.", ex.Message, String.Empty)
            Finally
                If Not oDA Is Nothing Then
                    oDA.Dispose()
                End If
                FileClose(fileNumber)
                If Exists(sb.Append(LogPath).Append("LogImport.txt").ToString()) Then
                    sb.Remove(0, sb.Length())
                    Delete(sb.Append(LogPath).Append("LogImport.txt").ToString())
                    sb.Remove(0, sb.Length())
                End If
            End Try
            sb = Nothing
        End Sub

    End Class

End Namespace


