Imports System.Data.SqlClient
Imports System.Web.Configuration
Imports System.Xml
Imports System.Text
Imports System.Data

Public Class cDA
    Private moConn As SqlConnection

    Public Sub New(Optional ByVal CnxString As String = "ConnectionString")
        OpenConnection(CnxString)
    End Sub

    Public Sub Dispose()
        If Not moConn Is Nothing Then
            If moConn.State = ConnectionState.Open Then moConn.Close()
            moConn.Dispose()
            moConn = Nothing
        End If
    End Sub

    Private Function OpenConnection(ByVal CnxString As String) As Boolean
        Dim server As String
        Dim database As String
        Dim user As String
        Dim password As String
        Dim cnnString As String
        Dim sb As StringBuilder

        Try

            If CnxString = "BEConnectionString" Then
                cnnString = WebConfigurationManager.AppSettings("BEConnectionString").Trim
            ElseIf CnxString = "DataDatabase" Then
                cnnString = WebConfigurationManager.AppSettings("DataDatabase").Trim
            Else
                cnnString = WebConfigurationManager.AppSettings("ConnectionString").Trim

                If cnnString.Length = 0 Then
                    server = WebConfigurationManager.AppSettings("Server")
                    database = WebConfigurationManager.AppSettings("Database")
                    user = WebConfigurationManager.AppSettings("User")
                    password = WebConfigurationManager.AppSettings("Password")

                    sb = New StringBuilder()
                    sb.Append("data source=").Append(server).Append(";initial Catalog=").Append(database).Append(";User ID=").Append(user).Append(";Password=").Append(password)

                    cnnString = sb.ToString()
                End If
            End If

            moConn = New SqlConnection
            moConn.ConnectionString = cnnString
            moConn.Open()

        Catch ex As Exception
            Throw
        End Try

        Return True

    End Function

    Public Sub AddMessageLog(ByVal LogTypeID As Integer, ByRef UUID As String, ByRef WebServer As String, _
                                  ByRef Customer As String, ByRef UserName As String, ByRef Provider As String, _
                                  ByVal MessageID As Integer, ByRef Message As String, _
                                  ByVal MessageDate As Date, ByVal ResponseTime As Integer)

        Dim oCommand As SqlCommand = Nothing
        Dim logMessage As String = Message

        Try

            oCommand = New SqlCommand("uspAddMessageLog", moConn)

            With oCommand
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = 60
                With oCommand.Parameters
                    .Add(New SqlParameter("@LogTypeID", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, LogTypeID))
                    .Add(New SqlParameter("@UUID", SqlDbType.Char, 35, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, UUID))
                    .Add(New SqlParameter("@WebServer", SqlDbType.VarChar, 50, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, WebServer))
                    .Add(New SqlParameter("@Customer", SqlDbType.VarChar, 50, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, Customer))
                    .Add(New SqlParameter("@UserName", SqlDbType.VarChar, 50, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, UserName))
                    .Add(New SqlParameter("@Provider", SqlDbType.VarChar, 50, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, Provider))
                    .Add(New SqlParameter("@MessageID", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, MessageID))
                    .Add(New SqlParameter("@MessageDate", SqlDbType.DateTime, 8, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, MessageDate))
                    .Add(New SqlParameter("@ResponseTime", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, ResponseTime))
                    .Add(New SqlParameter("@Message", logMessage))
                End With

                .ExecuteNonQuery()

            End With

        Catch ex As Exception
            Throw
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
            End If
        End Try

    End Sub

    Public Sub AddDeals(ByVal strRequest As String, ByVal strResponse As String)

        Dim oCommand As SqlCommand = Nothing
        'Dim strSQL As String = ""
        Dim logMessage As String
        Dim oDoc As XmlDocument
        Dim oRoot As XmlElement
        Dim oNode As XmlNode = Nothing
        Dim oNodes As XmlNodeList
        Dim departureCity As String
        Dim arrivalCity As String
        Dim departureDate As String
        Dim returnDate As String = ""
        Dim fareAmount As Decimal
        Dim markup As Decimal = 0
        Dim fareType As String
        Dim airline As String = ""
        Dim officeID As String
        Dim insertDate As Date
        Dim tripType As String

        Try
            logMessage = "<LogMessage>" & strRequest.Replace("OTA_AirLowFareSearchPlusRQ", "OTA_AirLowFareSearchRQ").Replace("OTA_AirLowFareSearchScheduleRQ", "OTA_AirLowFareSearchRQ").Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "") & strResponse.Replace("OTA_AirLowFareSearchPlusRS", "OTA_AirLowFareSearchRS").Replace("OTA_AirLowFareSearchScheduleRS", "OTA_AirLowFareSearchRS") & "</LogMessage>"

            oDoc = New XmlDocument
            oDoc.LoadXml(logMessage)
            oRoot = oDoc.DocumentElement

            If Not oRoot.SelectSingleNode("OTA_AirLowFareSearchRS/Success") Is Nothing Then

                oNodes = oRoot.SelectNodes("OTA_AirLowFareSearchRQ/OriginDestinationInformation")

                If oNodes.Count < 3 Then

                    If oNodes.Count = 1 Then
                        tripType = "O"
                    ElseIf oNodes.Count = 2 Then
                        tripType = "R"
                    End If

                    officeID = oRoot.SelectSingleNode("OTA_AirLowFareSearchRQ/POS/Source/@PseudoCityCode").InnerText
                    departureCity = oRoot.SelectSingleNode("OTA_AirLowFareSearchRQ/OriginDestinationInformation[1]/OriginLocation/@LocationCode").InnerText
                    arrivalCity = oRoot.SelectSingleNode("OTA_AirLowFareSearchRQ/OriginDestinationInformation[1]/DestinationLocation/@LocationCode").InnerText

                    departureDate = oRoot.SelectSingleNode("OTA_AirLowFareSearchRQ/OriginDestinationInformation[1]/DepartureDateTime").InnerText.Substring(0, 10)

                    If oNodes.Count = 2 Then
                        returnDate = oRoot.SelectSingleNode("OTA_AirLowFareSearchRQ/OriginDestinationInformation[position()=2]/DepartureDateTime").InnerText.Substring(0, 10)
                    End If

                    fareAmount = oRoot.SelectSingleNode("OTA_AirLowFareSearchRS/PricedItineraries/PricedItinerary[1]/AirItineraryPricingInfo/PTC_FareBreakdowns/PTC_FareBreakdown[1]/PassengerFare/TotalFare/@Amount").InnerText
                    Dim nip As Integer = oRoot.SelectSingleNode("OTA_AirLowFareSearchRS/PricedItineraries/PricedItinerary[1]/AirItineraryPricingInfo/PTC_FareBreakdowns/PTC_FareBreakdown[1]/PassengerTypeQuantity/@Quantity").InnerText
                    fareAmount = (fareAmount / nip) / 100

                    Dim oFeeNode As XmlNode = oRoot.SelectSingleNode("OTA_AirLowFareSearchRS/PricedItineraries/PricedItinerary[1]/AirItineraryPricingInfo/PTC_FareBreakdowns/PTC_FareBreakdown[1]/PassengerFare/Fees/Fee/@Amount")

                    If Not oFeeNode Is Nothing Then
                        markup = oFeeNode.InnerText
                        markup = (markup / nip) / 100
                    End If

                    fareType = oRoot.SelectSingleNode("OTA_AirLowFareSearchRS/PricedItineraries/PricedItinerary[1]/AirItineraryPricingInfo/@PricingSource").InnerText

                    If fareType = "Published" Then
                        fareType = "P"
                    Else
                        fareType = "U"
                    End If

                    Dim oAirNodes As XmlNodeList = oRoot.SelectNodes("OTA_AirLowFareSearchRS/PricedItineraries/PricedItinerary[1]/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment")

                    For Each oAirNode As XmlNode In oAirNodes
                        airline = airline & oAirNode.SelectSingleNode("OperatingAirline/@Code").InnerText & "/"
                    Next

                    Dim airtemp As String = airline.Replace(airline.Substring(0, 3), "")

                    If airtemp = "" Then
                        airline = airline.Substring(0, 2)
                    Else
                        airline = ""
                    End If

                    insertDate = Now

                    oNode = Nothing

                    oCommand = New SqlCommand("uspAddDeals", moConn)

                    With oCommand
                        .CommandType = CommandType.StoredProcedure
                        .CommandTimeout = 60
                        With oCommand.Parameters
                            .Add(New SqlParameter("@DepartureCity", SqlDbType.Char, 3, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, departureCity))
                            .Add(New SqlParameter("@ArrivalCity", SqlDbType.Char, 3, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, arrivalCity))
                            .Add(New SqlParameter("@DepartureDate", SqlDbType.Char, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, departureDate))
                            .Add(New SqlParameter("@ReturnDate", SqlDbType.Char, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, returnDate))
                            .Add(New SqlParameter("@TripType", SqlDbType.Char, 1, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, tripType))
                            .Add(New SqlParameter("@FareAmount", SqlDbType.Decimal, 18, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, fareAmount))
                            .Add(New SqlParameter("@Markup", SqlDbType.Decimal, 18, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, markup))
                            .Add(New SqlParameter("@FareType", SqlDbType.Char, 1, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, fareType))
                            .Add(New SqlParameter("@Airline", SqlDbType.Char, 2, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, airline))
                            .Add(New SqlParameter("@OfficeID", SqlDbType.VarChar, 9, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, officeID))
                            .Add(New SqlParameter("@InsertDate", SqlDbType.DateTime, 8, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, insertDate))
                        End With

                        .ExecuteNonQuery()

                    End With
                End If
            End If

        Catch ex As Exception
            Throw
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
            End If
        End Try

    End Sub

    Public Function GetDeals(ByVal strRequest As String) As String

        Dim oCommand As SqlCommand = Nothing
        'Dim strSQL As String = ""
        Dim oDoc As XmlDocument
        Dim oRoot As XmlElement
        Dim oNode As XmlNode = Nothing
        'Dim oNodes As XmlNodeList = Nothing
        Dim strResponse As String
        Dim departureCity As String
        Dim arrivalCity As String = Nothing
        Dim departureDate As String
        Dim returnDate As String = Nothing
        Dim fareType As String = Nothing
        Dim airline As String = Nothing
        Dim officeID As String
        Dim tripType As String = Nothing
        Dim dr As SqlDataReader
        Dim sb As StringBuilder
        Dim iRPH As Integer = 0

        Try
            oDoc = New XmlDocument
            oDoc.LoadXml(strRequest)
            oRoot = oDoc.DocumentElement

            strResponse = "<TXML_GetDealsRS Version=""1.0"">"

            If Not oRoot.SelectSingleNode("OriginDestinationInformation/DepartureDate") Is Nothing And Not oRoot.SelectSingleNode("OriginDestinationInformation/OriginLocation/@LocationCode") Is Nothing And Not oRoot.SelectSingleNode("OriginDestinationInformation/@TripType") Is Nothing Then

                If oRoot.SelectSingleNode("OriginDestinationInformation/@TripType").InnerText = "OneWay" Then
                    tripType = "O"
                ElseIf oRoot.SelectSingleNode("OriginDestinationInformation/@TripType").InnerText = "RoundTrip" Then
                    tripType = "R"
                End If

                officeID = oRoot.SelectSingleNode("POS/Source/@PseudoCityCode").InnerText
                departureCity = oRoot.SelectSingleNode("OriginDestinationInformation/OriginLocation/@LocationCode").InnerText

                If Not oRoot.SelectSingleNode("OriginDestinationInformation/DestinationLocation/@LocationCode") Is Nothing Then
                    arrivalCity = oRoot.SelectSingleNode("OriginDestinationInformation/DestinationLocation/@LocationCode").InnerText
                End If

                departureDate = oRoot.SelectSingleNode("OriginDestinationInformation/DepartureDate").InnerText.Substring(0, 10)

                If Not oRoot.SelectSingleNode("OriginDestinationInformation/ReturnDate") Is Nothing Then
                    returnDate = oRoot.SelectSingleNode("OriginDestinationInformation/ReturnDate").InnerText.Substring(0, 10)
                End If

                If Not oRoot.SelectSingleNode("FareType") Is Nothing Then
                    fareType = oRoot.SelectSingleNode("FareType").InnerText

                    If fareType = "Published" Then
                        fareType = "P"
                    Else
                        fareType = "U"
                    End If
                End If

                If Not oRoot.SelectSingleNode("VendorPref/@Code") Is Nothing Then
                    airline = oRoot.SelectSingleNode("VendorPref/@Code").InnerText
                End If

                oNode = Nothing

                oCommand = New SqlCommand("uspGetDeals", moConn)

                With oCommand
                    .CommandType = CommandType.StoredProcedure
                    .CommandTimeout = 60
                    With oCommand.Parameters
                        .Add(New SqlParameter("@DepartureCity", SqlDbType.Char, 3, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, departureCity))
                        .Add(New SqlParameter("@ArrivalCity", SqlDbType.Char, 3, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, arrivalCity))
                        .Add(New SqlParameter("@DepartureDate", SqlDbType.Char, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, departureDate))
                        .Add(New SqlParameter("@ReturnDate", SqlDbType.Char, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, returnDate))
                        .Add(New SqlParameter("@TripType", SqlDbType.Char, 1, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, tripType))
                        .Add(New SqlParameter("@FareType", SqlDbType.Char, 1, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, fareType))
                        .Add(New SqlParameter("@Airline", SqlDbType.Char, 2, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, airline))
                        .Add(New SqlParameter("@OfficeID", SqlDbType.VarChar, 9, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, officeID))
                    End With

                    dr = .ExecuteReader()

                End With
            Else
                Throw New Exception("Manadatory elements missing")
            End If

            sb = New StringBuilder()
            sb.Append("<Success/><Deals>")

            Do
                While (dr.Read())
                    iRPH = iRPH + 1
                    sb.Append("<Deal RPH=""").Append(iRPH.ToString())
                    sb.Append("""><OriginDestinationInformation>")
                    sb.Append("<DepartureDate>")
                    sb.Append(dr.GetValue(3).ToString())
                    sb.Append("</DepartureDate>")

                    If dr.GetValue(5).ToString() = "R" Then
                        sb.Append("<ReturnDate>")
                        sb.Append(dr.GetValue(4).ToString())
                        sb.Append("</ReturnDate>")
                    End If

                    sb.Append("<OriginLocation LocationCode=""")
                    sb.Append(dr.GetValue(1).ToString())
                    sb.Append("""/>")
                    sb.Append("<DestinationLocation LocationCode=""")
                    sb.Append(dr.GetValue(2).ToString())
                    sb.Append("""/>")

                    If dr.GetValue(9).ToString() <> "  " Then
                        sb.Append("<MarketingAirline Code=""")
                        sb.Append(dr.GetValue(9).ToString())
                        sb.Append("""/>")
                    End If

                    sb.Append("</OriginDestinationInformation>")

                    sb.Append("<FareInfo FareType=""")

                    If dr.GetValue(8).ToString() = "P" Then
                        sb.Append("Published")
                    Else
                        sb.Append("Private")
                    End If

                    sb.Append(""">")
                    sb.Append("<TotalAmount>")
                    sb.Append(dr.GetValue(6).ToString())
                    sb.Append("</TotalAmount>")
                    sb.Append("<IncludedMarkup>")
                    sb.Append(dr.GetValue(7).ToString())
                    sb.Append("</IncludedMarkup></FareInfo>")

                    sb.Append("</Deal>")
                End While

                If dr.NextResult() = Nothing Then
                    Exit Do
                End If
            Loop

            sb.Append("</Deals>")

            If sb.ToString().Contains("<Deal RPH") Then
                strResponse = strResponse & sb.ToString()
            Else
                strResponse = strResponse & "<Errors><Error>No deals available</Error></Errors>"
            End If

            dr.Close()

            strResponse = strResponse & "</TXML_GetDealsRS>"

            strResponse = CoreLib.TransformXML(strResponse, "C:\\TRIPXML\\XSL\\TXML\\", "TXML_GetDealsRS.xsl")

            Return strResponse

        Catch ex As Exception
            strResponse = "<TXML_GetDealsRS><Errors><Error>" & ex.Message & "</Error></Errors></TXML_GetDealsRS>"
            Return strResponse
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
            End If
        End Try

    End Function

    Public Function CheckSession(ByVal SessionID As String, ByVal isSOAP2 As Boolean) As Boolean ' Check conditions when closing - by Shashin - 23-02-2010
        Dim oCommand As SqlCommand = New SqlCommand()
        Dim strSQL As String
        Dim oAdapter As SqlDataAdapter = Nothing
        Dim dtTest1 As DataTable = New DataTable()
        Dim dtTest2 As DataTable = New DataTable()
        'Dim Server As String = ""
        'Dim Database As String = ""
        'Dim User As String = ""
        'Dim Password As String = ""
        'Dim CnnString As String = ""
        'Dim sb As StringBuilder = Nothing
        Dim cnt As Integer = 0

        'Dim oCon As SqlConnection = New SqlConnection()


        Try

            'Server = WebConfigurationManager.AppSettings("Server")
            'Database = WebConfigurationManager.AppSettings("Database")
            'User = WebConfigurationManager.AppSettings("User")
            'Password = WebConfigurationManager.AppSettings("Password")

            'sb = New StringBuilder()
            'sb.Append("data source=").Append(Server).Append(";initial Catalog=").Append(Database).Append(";User ID=").Append(User).Append(";Password=").Append(Password)

            'CnnString = sb.ToString()
            'oCon.ConnectionString = CnnString

            Dim tSession As String = ""
            Try
                If isSOAP2 Then
                    tSession = SessionID.Substring(0, SessionID.LastIndexOf("|")) 'ignore the sequence number
                Else
                    tSession = SessionID.Substring(0, SessionID.IndexOf("|")) 'ignore the sequence number
                End If

            Catch ex As Exception
                tSession = SessionID
            End Try

            strSQL = "select sessionid from tblSessionPool where sessionid like '" & tSession & "%'"
            oCommand.CommandText = strSQL
            oCommand.Connection = moConn
            oAdapter = New SqlDataAdapter(oCommand)
            oAdapter.Fill(dtTest1)
            If dtTest1.Rows.Count = 0 Then
                Return False
            End If
            strSQL = "select sessionID from tblSessionPool where sessionid like '" & tSession & "%' and ToBeDeleted='Y'"
            oCommand.CommandText = strSQL
            oAdapter.Fill(dtTest2)

            If dtTest2.Rows.Count > 0 Then
                If Not moConn.State = ConnectionState.Open Then
                    moConn.Open()
                End If
                strSQL = "delete from tblSessionPool where sessionid like '" & tSession & "%'"
                oCommand.CommandText = strSQL
                cnt = oCommand.ExecuteNonQuery()

                strSQL = "update tblPCCBlocks set sessionsUsed=sessionsUsed-1 where PCC=(SELECT top 1  [PCC]() FROM tblSessionPool where sessionid like '" & tSession & "%')"
                oCommand.CommandText = strSQL
                cnt = oCommand.ExecuteNonQuery()
                Return False
            End If

            If Not moConn.State = ConnectionState.Open Then
                moConn.Open()
            End If


            'strSQL = "update tblSessionPool set LastMessageTime=getdate(),IsUse='N',sessionid='" + tSession + "|" + seq.ToString() + "',SequenceNo=" + seq.ToString() + " where sessionid like '" & tSession & "%'"


            'Dim seq As Integer = 0
            'Try
            '    seq = Integer.Parse(SessionID.Substring(SessionID.IndexOf("|") + 1, SessionID.Length - SessionID.IndexOf("|") - 1))
            '    'seq = seq + 1
            'Catch ex As Exception

            'End Try
            'strSQL = "update tblSessionPool set LastMessageTime=getdate(),IsUse='N',sessionid='" + tSession + "|" + seq.ToString() + "',SequenceNo=" + seq.ToString() + " where sessionid like '" & tSession & "%'"

            strSQL = "update tblSessionPool set LastMessageTime=getdate(),IsUse='N' where sessionid like '" & tSession & "%'"


            oCommand.CommandText = strSQL

            cnt = oCommand.ExecuteNonQuery()
            Return True

        Catch ex As Exception
            Throw
        Finally
            dtTest1.Dispose()
            dtTest2.Dispose()
            oAdapter.Dispose()
            oCommand.Dispose()
        End Try


    End Function

    Public Function ToBeDeleted(ByVal SessionID As String) As DataTable
        Dim oCommand As SqlCommand = New SqlCommand()
        Dim strSQL As String
        Dim oAdapter As SqlDataAdapter = Nothing
        Dim dtTest1 As DataTable = New DataTable()
        Dim dtTest2 As DataTable = New DataTable()
        'Dim Server As String = ""
        'Dim Database As String = ""
        'Dim User As String = ""
        'Dim Password As String = ""
        'Dim CnnString As String = ""
        'Dim sb As StringBuilder = Nothing
        Dim cnt As Integer = 0
        Dim strRet As DataTable

        'Dim oCon As SqlConnection = New SqlConnection()


        Try

            'Server = WebConfigurationManager.AppSettings("Server")
            'Database = WebConfigurationManager.AppSettings("Database")
            'User = WebConfigurationManager.AppSettings("User")
            'Password = WebConfigurationManager.AppSettings("Password")

            'sb = New StringBuilder()
            'sb.Append("data source=").Append(Server).Append(";initial Catalog=").Append(Database).Append(";User ID=").Append(User).Append(";Password=").Append(Password)

            'CnnString = sb.ToString()
            'oCon.ConnectionString = CnnString
            Dim tSession As String = ""
            Try
                tSession = SessionID.Substring(0, SessionID.IndexOf("|")) 'ignore the sequence number
            Catch ex As Exception
                tSession = SessionID
            End Try

            strSQL = "select sessionid, BlockID, IsInitialBlock from tblSessionPool where sessionid like '%" & tSession & "%'"
            oCommand.CommandText = strSQL
            oCommand.Connection = moConn
            oAdapter = New SqlDataAdapter(oCommand)
            oAdapter.Fill(dtTest1)
            If dtTest1.Rows.Count = 0 Then
                Return dtTest1
            Else
                strRet = dtTest1
            End If

            If Not moConn.State = ConnectionState.Open Then
                moConn.Open()
            End If

            strSQL = "delete from tblSessionPool where SessionID like '%" & tSession & "%'"
            oCommand.CommandText = strSQL
            cnt = oCommand.ExecuteNonQuery()

            Return strRet

        Catch ex As Exception
            Throw
        Finally
            dtTest1.Dispose()
            dtTest2.Dispose()
            oAdapter.Dispose()
            oCommand.Dispose()
        End Try


    End Function

    Public Function CheckSessionWithOutSequence(ByVal SessionID As String) As Boolean ' Check conditions when closing - by Shashin - 23-02-2010
        Dim oCommand As SqlCommand = New SqlCommand()
        Dim strSQL As String = ""
        Dim oAdapter As SqlDataAdapter = Nothing
        Dim dtTest1 As DataTable = New DataTable()
        Dim dtTest2 As DataTable = New DataTable()
        Dim Server As String = ""
        Dim Database As String = ""
        Dim User As String = ""
        Dim Password As String = ""
        Dim CnnString As String = ""
        Dim sb As StringBuilder = Nothing
        Dim cnt As Integer = 0

        Dim oCon As SqlConnection = New SqlConnection()


        Try

            Server = WebConfigurationManager.AppSettings("Server")
            Database = WebConfigurationManager.AppSettings("Database")
            User = WebConfigurationManager.AppSettings("User")
            Password = WebConfigurationManager.AppSettings("Password")

            sb = New StringBuilder()
            sb.Append("data source=").Append(Server).Append(";initial Catalog=").Append(Database).Append(";User ID=").Append(User).Append(";Password=").Append(Password)

            CnnString = sb.ToString()
            oCon.ConnectionString = CnnString
            Dim tSession As String = ""

            tSession = SessionID

            strSQL = "select sessionid from tblSessionPool where sessionid = '" & tSession & "'"
            oCommand.CommandText = strSQL
            oCommand.Connection = oCon
            oAdapter = New SqlDataAdapter(oCommand)
            oAdapter.Fill(dtTest1)
            If dtTest1.Rows.Count = 0 Then
                Return False
            End If
            strSQL = "select sessionID from tblSessionPool where sessionid = '" & tSession & "' and ToBeDeleted='Y'"
            oCommand.CommandText = strSQL
            oAdapter.Fill(dtTest2)

            If dtTest2.Rows.Count > 0 Then
                If Not oCon.State = ConnectionState.Open Then
                    oCon.Open()
                End If
                strSQL = "delete from tblSessionPool where sessionid = '" & tSession & "'"
                oCommand.CommandText = strSQL
                cnt = oCommand.ExecuteNonQuery()

                strSQL = "update tblPCCBlocks set sessionsUsed=sessionsUsed-1 where PCC=(SELECT top 1  [PCC]() FROM tblSessionPool where sessionid = '" & tSession & "')"
                oCommand.CommandText = strSQL
                cnt = oCommand.ExecuteNonQuery()
                oCon.Close()
                Return False
            End If

            If Not oCon.State = ConnectionState.Open Then
                oCon.Open()
            End If



            strSQL = "update tblSessionPool set LastMessageTime=getdate(),IsUse='N',sessionid='" + tSession + "' where sessionid like '" & tSession & "%'"

            ' strSQL = "update tblSessionPool set LastMessageTime=getdate(),IsUse='N' where sessionid like '" & tSession & "%'"


            oCommand.CommandText = strSQL

            cnt = oCommand.ExecuteNonQuery()
            oCon.Close()
            Return True

        Catch ex As Exception
            Throw ex
        Finally
            dtTest1.Dispose()
            dtTest2.Dispose()
            oAdapter.Dispose()
            oCommand.Dispose()
            oCon.Close()
            oCon.Dispose()
        End Try


    End Function

    Public Sub AddSoapException(ByRef SoapException As String, ByRef SoapEnvelope As String)
        Dim oCommand As SqlCommand = Nothing
        Dim strSQL As String = ""

        Try

            oCommand = New SqlCommand("uspAddSoapException", moConn)

            With oCommand
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = 60
                With oCommand.Parameters
                    .Add(New SqlParameter("@SoapException", SqlDbType.VarChar, 2000, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, SoapException))
                    .Add(New SqlParameter("@SoapEnvelope", SoapEnvelope))
                End With

                .ExecuteNonQuery()

            End With

        Catch ex As Exception
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try

    End Sub

    Public Sub ImportBooking(ByVal Customer As String, ByVal UserName As String, ByVal RecLoc As String, ByVal FlightDate As Date)

        Dim oCommand As SqlCommand = Nothing
        Dim strSQL As String = ""

        Try

            oCommand = New SqlCommand("uspImportBookings", moConn)

            With oCommand
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = 60
                With oCommand.Parameters
                    .Add(New SqlParameter("@Customer", SqlDbType.VarChar, 50, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, Customer))
                    .Add(New SqlParameter("@UserName", SqlDbType.VarChar, 50, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, UserName))
                    .Add(New SqlParameter("@RecLoc", SqlDbType.Char, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, RecLoc))
                    .Add(New SqlParameter("@FlightDate", SqlDbType.SmallDateTime, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, FlightDate))
                End With

                .ExecuteNonQuery()

            End With

        Catch ex As Exception
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try

    End Sub

    Public Sub UpdateBookingStatus(ByVal RecLoc As String, ByVal Status As Char)

        Dim oCommand As SqlCommand = Nothing
        Dim strSQL As String = ""

        Try

            oCommand = New SqlCommand("uspUpdateBookingStatus", moConn)

            With oCommand
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = 60
                With oCommand.Parameters
                    .Add(New SqlParameter("@RecLoc", SqlDbType.Char, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, RecLoc))
                    .Add(New SqlParameter("@Status", SqlDbType.Char, 1, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, Status))
                End With

                .ExecuteNonQuery()

            End With

        Catch ex As Exception
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try

    End Sub

    Public Function GetMarkups(ByVal siteItemID As Integer, ByVal databaseName As String, ByVal languageId As Integer, _
                                  ByVal currencyId As String, ByVal supplierId As String, ByVal externalId As String, _
                                  ByVal destinationId As String, ByVal brandId As String, ByVal departureCity As String, _
                                  ByVal departureStartDate As DateTime, ByVal departureEndDate As DateTime, ByVal countryId As String, _
                                  ByVal stateId As String, ByVal promotionTypeId As Integer, ByVal classTypeId As Integer, ByVal seniorCount As Integer, _
                                  ByVal adultCount As Integer, ByVal childCount As Integer, ByVal BookingDate As DateTime, ByVal PackageID As Integer, _
                                  ByVal FareCodeSiteItemID As Integer, ByVal ProductTypeId As Integer, ByVal fromCity As String, ByVal toCity As String, _
                                  ByVal fromCountry As String, ByVal toCountry As String, ByVal FareType As String, _
                                  ByVal Affiliate As Integer, ByVal JourneyType As Integer, ByVal OfficeID As String, ByVal ApplicationTypeId As String, _
                                  ByVal RoutingCity As String, ByVal BookingClass As String, ByVal FlightNumber As String) As String

        Dim oCommand As SqlCommand = Nothing
        Dim strSQL As String = ""
        Dim dr As SqlDataReader
        Dim sb As StringBuilder = New StringBuilder()

        Try
            oCommand = New SqlCommand("SitePromotionsSearchNew", moConn)

            With oCommand
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = 60
                With oCommand.Parameters
                    .Add(New SqlParameter("@siteItemID", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, siteItemID))
                    .Add(New SqlParameter("@databaseName", SqlDbType.Char, 100, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, databaseName))
                    .Add(New SqlParameter("@languageId", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, languageId))
                    .Add(New SqlParameter("@currencyId", SqlDbType.Char, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, currencyId))
                    .Add(New SqlParameter("@supplierId", SqlDbType.Char, 100, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, supplierId))
                    .Add(New SqlParameter("@externalId", SqlDbType.Char, 3, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, externalId))
                    .Add(New SqlParameter("@destinationId", SqlDbType.Char, 100, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, destinationId))
                    .Add(New SqlParameter("@brandId", SqlDbType.Char, 100, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, brandId))
                    .Add(New SqlParameter("@departureCity", SqlDbType.Char, 200, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, departureCity))
                    .Add(New SqlParameter("@departureStartDate", SqlDbType.DateTime, 8, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, departureStartDate))
                    .Add(New SqlParameter("@departureEndDate", SqlDbType.DateTime, 8, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, departureEndDate))
                    .Add(New SqlParameter("@countryId", SqlDbType.Char, 100, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, countryId))
                    .Add(New SqlParameter("@stateId", SqlDbType.Char, 100, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, stateId))
                    .Add(New SqlParameter("@promotionTypeId", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, promotionTypeId))
                    .Add(New SqlParameter("@classTypeId", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, classTypeId))
                    .Add(New SqlParameter("@seniorCount", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, seniorCount))
                    .Add(New SqlParameter("@adultCount", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, adultCount))
                    .Add(New SqlParameter("@childCount", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, childCount))
                    .Add(New SqlParameter("@BookingDate", SqlDbType.DateTime, 8, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, BookingDate))
                    .Add(New SqlParameter("@PackageID", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, PackageID))
                    .Add(New SqlParameter("@FareCodeSiteItemID", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, FareCodeSiteItemID))
                    .Add(New SqlParameter("@ProductTypeId", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, ProductTypeId))
                    .Add(New SqlParameter("@fromCity", SqlDbType.Char, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, fromCity))
                    .Add(New SqlParameter("@toCity", SqlDbType.Char, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, toCity))
                    .Add(New SqlParameter("@fromCountry", SqlDbType.Char, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, fromCountry))
                    .Add(New SqlParameter("@toCountry", SqlDbType.Char, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, toCountry))
                    .Add(New SqlParameter("@FareType", SqlDbType.Char, 50, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, FareType))
                    .Add(New SqlParameter("@Affiliate", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, Affiliate))
                    .Add(New SqlParameter("@JourneyType", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, JourneyType))
                    .Add(New SqlParameter("@OfficeID", SqlDbType.Char, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, OfficeID))
                    .Add(New SqlParameter("@ApplicationTypeId", SqlDbType.Char, 3, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, ApplicationTypeId))
                    .Add(New SqlParameter("@RoutingCity", SqlDbType.Char, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, RoutingCity))
                    .Add(New SqlParameter("@BookingClass", SqlDbType.Char, 3, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, BookingClass))
                    .Add(New SqlParameter("@FlightNumber", SqlDbType.Char, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, FlightNumber))
                End With

                '.ExecuteXmlReader()
            End With

            'markupsXML = oCommand.ExecuteXmlReader
            dr = oCommand.ExecuteReader()
            Dim i As Integer = 0
            sb.Append("<SearchPromotionsResponse>")
            Do
                While dr.Read()
                    i = i + 1
                    sb.Append("<Promotion>")

                    For j As Integer = 0 To dr.FieldCount - 1
                        sb.Append("<")
                        sb.Append(dr.GetName(j))
                        sb.Append(">")
                        sb.Append(dr.Item(j))
                        sb.Append("</")
                        sb.Append(dr.GetName(j))
                        sb.Append(">")
                    Next

                    sb.Append("</Promotion>")
                End While
            Loop While dr.NextResult()

            sb.Append("</SearchPromotionsResponse>")

            Return sb.ToString()

        Catch ex As Exception
            Throw ex
        Finally
            dr.Close()
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try

    End Function
    'Check AvailableSessions in the pool
    Public Function CheckAvailableSessions(ByVal PCC As String, ByVal System As String, ByVal UserID As String) As Boolean

        Dim oCommand As SqlCommand = New SqlCommand()
        Dim strSQL As String = ""
        Dim AvailableSessions As Boolean
        'Dim reader As SqlDataReader = Nothing
        Dim reader As SqlDataReader = Nothing

        Try

            strSQL = "Select * from dbo.tblSessionPool where PCC like '" & PCC & "' AND system like '" & System & "' AND UserID like '" & UserID & "' AND ToBeDeleted='N' AND IsUse='N';"

            'oCommand = New SqlCommand("uspCheckAvailableSessions", moConn)
            'oCommand = New SqlCommand(strSQL, moConn)
            'strSQL = "select sessionid from tblSessionPool where sessionid like '" & tSession & "%'"

            oCommand.CommandText = strSQL
            oCommand.Connection = moConn

            reader = oCommand.ExecuteReader()


            If (reader.HasRows) Then
                AvailableSessions = True
            Else
                AvailableSessions = False
            End If

            Return AvailableSessions

        Catch ex As Exception
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                reader.Close()
                oCommand.Dispose()
                oCommand = Nothing

            End If
        End Try

    End Function

    'Update an available session
    Public Function SessionUpdate1() As String

        Dim oCommand As SqlCommand = Nothing
        Dim strSQL As String = ""
        Dim LastMessageTime As DateTime = DateTime.Now
        'Dim dr As SqlDataReader
        Dim SessionID As String = ""
        Dim SessionIDParam1 As SqlParameterCollection = Nothing

        Try

            oCommand = New SqlCommand("uspSessionPoolUpdate", moConn)

            With oCommand
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = 60
                With oCommand.Parameters
                    .Add(New SqlParameter("@LastMessageTime", SqlDbType.DateTime, 8, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, LastMessageTime))
                    Dim SessionIDParam As SqlParameter = New SqlParameter("@SessionID", SqlDbType.NVarChar, 15, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, SessionID)
                    SessionIDParam.Direction = ParameterDirection.Output
                    .Add(SessionIDParam)
                End With

                .ExecuteNonQuery()

            End With

            'dr = oCommand.ExecuteReader(CommandBehavior.CloseConnection)

            'dr.Read()
            'Dim SessionID As String = dr.GetString(dr.GetOrdinal("SessionID"))
            'dr.Close()

            'SessionIDParam1 = oCommand.Parameters["@SessionID"]

            Return SessionID

        Catch ex As Exception
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try
    End Function

    Public Function SessionUpdate(ByVal PCC As String, ByVal System As String, ByVal UserId As String, ByVal isSOAP2 As Boolean) As String

        Dim oCommand As SqlCommand = New SqlCommand()
        Dim strSQL As String = ""
        'Dim uLastMessageTime As DateTime = DateTime.Now
        'Dim dr As SqlDataReader
        Dim uSessionID As String = ""
        'Dim uSequenceNo As Integer
        Dim tSession As String = ""
        Dim NewSessionID As String = ""
        Dim SessionIDParam1 As SqlParameterCollection = Nothing
        Dim sb As StringBuilder = Nothing
        Dim reader As SqlDataReader = Nothing
        Dim oTran As SqlTransaction = Nothing


        Try


            oCommand = New SqlCommand("SessionUpdate", moConn)

            'strSQL = "SELECT TOP 1 SessionID,SequenceNo FROM tblSessionPool WITH (XLOCK, PAGLOCK) where PCC like '" & PCC & "' AND system like '" & System & "' AND UserID like '" & UserId & "' AND ToBeDeleted='N' AND IsUse='N' ORDER by LastMessageTime;"

            Dim SessionIDParam As SqlParameter = New SqlParameter("@Session", SqlDbType.NVarChar, 450, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, uSessionID)

            With oCommand
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = 60
                With oCommand.Parameters
                    .Add(New SqlParameter("@PCC", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, PCC))
                    .Add(New SqlParameter("@System", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, System))
                    .Add(New SqlParameter("@UserID", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, UserId))

                    SessionIDParam.Direction = ParameterDirection.Output
                    .Add(SessionIDParam)
                End With

                .ExecuteNonQuery()
                uSessionID = SessionIDParam.Value.ToString()

                Dim iseq As Integer = 0
                Dim seq As String = ""

                If isSOAP2 Then
                    seq = uSessionID.Substring(uSessionID.IndexOf("|") + 1)
                    seq = seq.Substring(seq.IndexOf("|") + 1)
                Else
                    seq = uSessionID.Substring(uSessionID.IndexOf("|") + 1)
                End If

                iseq = Convert.ToInt16(seq) - 1
                uSessionID = uSessionID.Substring(0, uSessionID.Length - seq.Length) & iseq.ToString()

                Return uSessionID

            End With

        Catch ex As Exception
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try


        '    'oCommand = New SqlCommand("uspCheckAvailableSessions", moConn)
        '    'oCommand = New SqlCommand(strSQL, moConn)
        '    'strSQL = "select sessionid from tblSessionPool where sessionid like '" & tSession & "%'"
        '    Dim waitTime As Double
        '    Dim rand As Random = New Random()
        '    waitTime = rand.NextDouble() * 1000 'waits random millisecond (max 1 second), so we can delay requests
        '    Threading.Thread.Sleep(waitTime)


        '    oTran = moConn.BeginTransaction()

        '    oCommand.CommandText = strSQL
        '    oCommand.Connection = moConn
        '    oCommand.Transaction = oTran
        '    reader = oCommand.ExecuteReader()


        '        .ExecuteNonQuery()

        '    Do While reader.Read()
        '        uSessionID = reader("SessionID").ToString
        '        uSequenceNo = reader("SequenceNo").ToString
        '    Loop

        '    reader.Close()

        '    uSequenceNo = uSequenceNo + 1
        '    tSession = uSessionID.Substring(0, uSessionID.IndexOf("|"))
        '    sb = New StringBuilder()
        '    NewSessionID = sb.Append(tSession).Append("|").Append(uSequenceNo).ToString



        '        Return uSessionID

        '    End With

        '    strSQL = "UPDATE tblSessionPool SET SessionID='" & NewSessionID & "',SequenceNo='" & uSequenceNo & "', LastMessageTime=getdate(), IsUse='Y' WHERE  UserID like '" & UserId & "'AND SessionID like '" & uSessionID & "%'"
        '    oCommand.CommandText = strSQL
        '    oCommand.Connection = moConn
        '    oCommand.ExecuteNonQuery()

        '    oTran.Commit()

        '    Return NewSessionID


        'Catch ex As Exception
        '    Throw ex
        'Finally
        '    If Not oCommand Is Nothing Then
        '        oCommand.Dispose()
        '        oCommand = Nothing
        '    End If
        'End Try



        'Try

        '    strSQL = "SELECT TOP 1 SessionID,SequenceNo FROM tblSessionPool where PCC like '" & PCC & "' AND system like '" & System & "' AND UserID like '" & UserId & "' AND ToBeDeleted='N' AND IsUse='N' ORDER by LastMessageTime;"

        '    'oCommand = New SqlCommand("uspCheckAvailableSessions", moConn)
        '    'oCommand = New SqlCommand(strSQL, moConn)
        '    'strSQL = "select sessionid from tblSessionPool where sessionid like '" & tSession & "%'"
        '    oTran = moConn.BeginTransaction()
        '    oCommand.CommandText = strSQL
        '    oCommand.Connection = moConn
        '    oCommand.Transaction = oTran
        '    reader = oCommand.ExecuteReader()

        '    Do While reader.Read()
        '        uSessionID = reader("SessionID").ToString
        '        uSequenceNo = reader("SequenceNo").ToString
        '    Loop
        '    reader.Close()

        '    uSequenceNo = uSequenceNo + 1
        '    tSession = uSessionID.Substring(0, uSessionID.IndexOf("|"))
        '    sb = New StringBuilder()
        '    NewSessionID = sb.Append(tSession).Append("|").Append(uSequenceNo).ToString



        '    strSQL = "UPDATE tblSessionPool SET SessionID='" & NewSessionID & "',SequenceNo='" & uSequenceNo & "', LastMessageTime=getdate(), IsUse='Y' WHERE  UserID like '" & UserId & "'AND SessionID like '" & uSessionID & "%'"
        '    oCommand.CommandText = strSQL
        '    oCommand.Connection = moConn
        '    oCommand.ExecuteNonQuery()

        '    oTran.Commit()

        '    Return NewSessionID

        'Catch ex As Exception
        '    oTran.Rollback()
        '    Throw ex
        'Finally
        '    If Not oCommand Is Nothing Then
        '        reader.Close()
        '        oCommand.Dispose()
        '        oCommand = Nothing
        '    End If
        'End Try
    End Function

    Public Function SessionUpdate(ByVal PCC As String, ByVal System As String, ByVal UserId As String, ByVal GDS As String) As String

        Dim oCommand As SqlCommand = New SqlCommand()
        Dim strSQL As String = ""
        'Dim uLastMessageTime As DateTime = DateTime.Now
        'Dim dr As SqlDataReader
        Dim uSessionID As String = ""
        'Dim uSequenceNo As Integer
        Dim tSession As String = ""
        Dim NewSessionID As String = ""
        Dim SessionIDParam1 As SqlParameterCollection = Nothing
        Dim sb As StringBuilder = Nothing
        Dim reader As SqlDataReader = Nothing
        Dim oTran As SqlTransaction = Nothing

        Try


            oCommand = New SqlCommand("SessionUpdate", moConn)

            'strSQL = "SELECT TOP 1 SessionID,SequenceNo FROM tblSessionPool WITH (XLOCK, PAGLOCK) where PCC like '" & PCC & "' AND system like '" & System & "' AND UserID like '" & UserId & "' AND ToBeDeleted='N' AND IsUse='N' ORDER by LastMessageTime;"

            Dim SessionIDParam As SqlParameter = New SqlParameter("@Session", SqlDbType.NVarChar, 450, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, uSessionID)

            With oCommand
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = 60
                With oCommand.Parameters
                    .Add(New SqlParameter("@PCC", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, PCC))
                    .Add(New SqlParameter("@System", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, System))
                    .Add(New SqlParameter("@UserID", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, UserId))

                    SessionIDParam.Direction = ParameterDirection.Output
                    .Add(SessionIDParam)
                End With

                .ExecuteNonQuery()
                uSessionID = SessionIDParam.Value.ToString()
                Return uSessionID

            End With

        Catch ex As Exception
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try

        'Try

        '    strSQL = "SELECT TOP 1 SessionID,SequenceNo FROM tblSessionPool where PCC like '" & PCC & "' AND system like '" & System & "' AND UserID like '" & UserId & "' AND ToBeDeleted='N' AND IsUse='N' ORDER by LastMessageTime;"

        '    oTran = moConn.BeginTransaction()
        '    oCommand.CommandText = strSQL
        '    oCommand.Connection = moConn
        '    oCommand.Transaction = oTran
        '    reader = oCommand.ExecuteReader()

        '    Do While reader.Read()
        '        uSessionID = reader("SessionID").ToString
        '        uSequenceNo = reader("SequenceNo").ToString
        '    Loop
        '    reader.Close()

        '    uSequenceNo = uSequenceNo + 1


        '    strSQL = "UPDATE tblSessionPool SET SequenceNo='" & uSequenceNo & "', LastMessageTime=getdate(), IsUse='Y' WHERE UserID like '" & UserId & "' AND SessionID like '" & uSessionID & "%'"
        '    oCommand.CommandText = strSQL
        '    oCommand.Connection = moConn
        '    oCommand.ExecuteNonQuery()
        '    oTran.Commit()
        '    Return uSessionID

        'Catch ex As Exception
        '    oTran.Rollback()
        '    Throw ex
        'Finally
        '    If Not oCommand Is Nothing Then
        '        reader.Close()
        '        oCommand.Dispose()
        '        oCommand = Nothing
        '    End If
        'End Try
    End Function

    Public Function SetPCCBlock(ByVal Provider As TripXMLProviderSystems) As TripXMLProviderSystems

        Dim oCommand As SqlCommand = New SqlCommand()
        Dim strSQL As String = ""
        'Dim uLastMessageTime As DateTime = DateTime.Now
        'Dim dr As SqlDataReader
        Dim PCC As String = ""
        Dim reader As SqlDataReader = Nothing

        PCC = Provider.PCC

        Try

            strSQL = "SELECT * FROM tblPCCBlocks where PCC like '" & PCC & "%' and UserID='" & Provider.UserID & "'"

            oCommand.CommandText = strSQL
            oCommand.Connection = moConn

            reader = oCommand.ExecuteReader()

            Do While reader.Read()
                Provider.ProviderSession.InitialBlockSize = reader("InitialBlock")
                Provider.ProviderSession.MaximumCount = reader("MaxSessions")
                Provider.ProviderSession.NextBlockSize = reader("NextBlock")
                Provider.ProviderSession.SessionsUsed = reader("SessionsUsed")
            Loop
            reader.Close()


            Return Provider

        Catch ex As Exception
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                reader.Close()
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try
    End Function

    Public Function SetPCCBlock(ByVal Provider As TripXMLProviderSystems, ByVal version As String) As TripXMLProviderSystems

        Dim oCommand As SqlCommand = New SqlCommand()
        Dim strSQL As String
        'Dim uLastMessageTime As DateTime = DateTime.Now
        'Dim dr As SqlDataReader
        Dim pcc As String
        Dim reader As SqlDataReader = Nothing

        pcc = Provider.PCC

        Try

            strSQL = "SELECT * FROM tblPCCBlocks where PCC like '" & pcc & "%' and UserID='" & Provider.UserID & "' and system='" & Provider.System & "'"

            oCommand.CommandText = strSQL
            oCommand.Connection = moConn

            reader = oCommand.ExecuteReader()

            Do While reader.Read()
                Provider.ProviderSession.InitialBlockSize = reader("InitialBlock")
                Provider.ProviderSession.MaximumCount = reader("MaxSessions")
                Provider.ProviderSession.NextBlockSize = reader("NextBlock")
                Provider.ProviderSession.SessionsUsed = reader("SessionsUsed")
            Loop
            reader.Close()


            Return Provider

        Catch ex As Exception
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                reader.Close()
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try
    End Function

    Public Function CheckInitialPool(ByVal PCC As String, ByVal UserID As String) As Boolean
        'Public Function CheckInitialPool(ByVal PCC As String, ByVal UserID As String, ByVal system As String) As Boolean
        Dim oCommand As SqlCommand = New SqlCommand()
        Dim strSQL As String = ""
        Dim reader As SqlDataReader = Nothing
        Dim SessionsUsed As Integer = 1
        Dim Creating As Integer = 1
        Dim tran As SqlTransaction = moConn.BeginTransaction()


        Try

            strSQL = "SELECT SessionsUsed,CreatingInit FROM tblPCCBlocks where PCC like '" & PCC & "%' AND UserID='" & UserID & "'"


            oCommand.CommandText = strSQL
            oCommand.Connection = moConn
            oCommand.Transaction = tran
            reader = oCommand.ExecuteReader()

            Do While reader.Read()
                SessionsUsed = reader("SessionsUsed").ToString
                Creating = reader("CreatingInit").ToString
            Loop

            reader.Close()

            strSQL = "UPDATE tblPCCBlocks SET CreatingInit=1 where PCC like '" & PCC & "%' AND UserID='" & UserID & "' AND InitialBlock !=0"


            oCommand.CommandText = strSQL
            oCommand.Connection = moConn
            oCommand.ExecuteNonQuery()

            tran.Commit()

            If SessionsUsed = 0 And Creating = 0 Then
                Return True
            Else
                Return False


            End If

        Catch ex As Exception
            tran.Rollback()
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try
    End Function

    Public Function CheckInitialPool(ByVal PCC As String, ByVal UserID As String, ByVal system As String) As Boolean
        Dim oCommand As SqlCommand = New SqlCommand()
        Dim strSQL As String = ""
        Dim reader As SqlDataReader = Nothing
        Dim SessionsUsed As Integer = 1
        Dim Creating As Integer = 1
        Dim tran As SqlTransaction = moConn.BeginTransaction()


        Try

            'strSQL = "SELECT SessionsUsed,CreatingInit FROM tblPCCBlocks where PCC like '" & PCC & "%' AND UserID='" & UserID & "'"

            strSQL = "SELECT SessionsUsed,CreatingInit FROM tblPCCBlocks where PCC like '" & PCC & "%' AND UserID='" & UserID & "' AND system='" & system & "'"


            oCommand.CommandText = strSQL
            oCommand.Connection = moConn
            oCommand.Transaction = tran
            reader = oCommand.ExecuteReader()

            Do While reader.Read()
                SessionsUsed = reader("SessionsUsed").ToString
                Creating = reader("CreatingInit").ToString
            Loop

            reader.Close()

            'strSQL = "UPDATE tblPCCBlocks SET CreatingInit=1 where PCC like '" & PCC & "%' AND UserID='" & UserID & "' AND InitialBlock !=0"

            strSQL = "UPDATE tblPCCBlocks SET CreatingInit=1 where PCC like '" & PCC & "%' AND UserID='" & UserID & "' AND system='" & system & "'AND InitialBlock !=0"


            oCommand.CommandText = strSQL
            oCommand.Connection = moConn
            oCommand.ExecuteNonQuery()

            tran.Commit()

            If SessionsUsed = 0 And Creating = 0 Then
                Return True
            Else
                Return False


            End If

        Catch ex As Exception
            tran.Rollback()
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try
    End Function

    'Public Sub UpdatePCCSessions(ByVal PCC As String, ByVal NewSessions As Integer)
    Public Sub UpdatePCCSessions(ByVal PCC As String, ByVal NewSessions As Integer)
        Dim oCommand As SqlCommand = New SqlCommand()
        Dim strSQL As String = ""
        Dim reader As SqlDataReader = Nothing
        Dim SessionsUsed As Integer


        Try
            'strSQL = "UPDATE tblPCCBlocks SET SessionsUsed='" & SessionCount & "' WHERE  PCC like '" & PCC & "%'"

            'strSQL = "SELECT SessionsUsed FROM tblPCCBlocks where PCC like '" & PCC & "%'"
            strSQL = "SELECT SessionsUsed FROM tblPCCBlocks where PCC like '" & PCC & "%'"

            oCommand.CommandText = strSQL
            oCommand.Connection = moConn

            reader = oCommand.ExecuteReader()

            Do While reader.Read()
                SessionsUsed = reader("SessionsUsed").ToString
            Loop
            reader.Close()


            NewSessions = NewSessions + SessionsUsed

            strSQL = "UPDATE tblPCCBlocks SET SessionsUsed='" & NewSessions & "' WHERE  PCC like '" & PCC & "%'"
            oCommand.CommandText = strSQL
            oCommand.Connection = moConn
            oCommand.ExecuteNonQuery()


        Catch ex As Exception
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try
    End Sub
    Public Sub UpdatePCCSessions(ByVal PCC As String, ByVal NewSessions As Integer, ByVal userID As String)

        Dim oCommand As SqlCommand = New SqlCommand()
        Dim strSQL As String = ""
        Dim reader As SqlDataReader = Nothing
        'Dim SessionsUsed As Integer


        Try
            'strSQL = "UPDATE tblPCCBlocks SET SessionsUsed='" & SessionCount & "' WHERE  PCC like '" & PCC & "%'"
            strSQL = "SELECT SessionsUsed FROM tblPCCBlocks where PCC like '" & PCC & "%' and UserID='" & userID & "'"

            oCommand.CommandText = strSQL
            oCommand.Connection = moConn

            'reader = oCommand.ExecuteReader()

            'Do While reader.Read()
            'SessionsUsed = reader("SessionsUsed").ToString
            'Loop
            'reader.Close()


            'NewSessions = NewSessions + SessionsUsed

            strSQL = "UPDATE tblPCCBlocks SET SessionsUsed=SessionsUsed+" & NewSessions & " WHERE  PCC like '" & PCC & "%' and UserID='" & userID & "'"
            oCommand.CommandText = strSQL
            oCommand.Connection = moConn
            oCommand.ExecuteNonQuery()


        Catch ex As Exception
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try
    End Sub

    Public Sub UpdatePCCSessions(ByVal PCC As String, ByVal NewSessions As Integer, ByVal userID As String, ByVal system As String)
        Dim oCommand As SqlCommand = New SqlCommand()
        Dim strSQL As String = ""
        Dim reader As SqlDataReader = Nothing
        'Dim SessionsUsed As Integer


        Try
            'strSQL = "UPDATE tblPCCBlocks SET SessionsUsed='" & SessionCount & "' WHERE  PCC like '" & PCC & "%'"

            'strSQL = "SELECT SessionsUsed FROM tblPCCBlocks where PCC like '" & PCC & "%'"
            strSQL = "SELECT SessionsUsed FROM tblPCCBlocks where PCC like '" & PCC & "%'AND UserID='" & userID & "' AND system='" & system & "'"

            oCommand.CommandText = strSQL
            oCommand.Connection = moConn

            'reader = oCommand.ExecuteReader()

            'Do While reader.Read()
            '    SessionsUsed = reader("SessionsUsed").ToString
            'Loop
            'reader.Close()


            'NewSessions = NewSessions + SessionsUsed

            strSQL = "UPDATE tblPCCBlocks SET SessionsUsed=SessionsUsed+" & NewSessions & " WHERE  PCC like '" & PCC & "%' and UserID='" & userID & "'AND system='" & system & "'"
            oCommand.CommandText = strSQL
            oCommand.Connection = moConn
            oCommand.ExecuteNonQuery()


        Catch ex As Exception
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try
    End Sub


    'Insert a Session to the Pool
    Public Sub InsertNewSession(ByVal SessionID As String, ByVal SequenceNo As Integer, ByVal GDS As String, ByVal CreatedTime As DateTime, ByVal LastMessageTime As DateTime, ByVal UserName As String, ByVal UserID As String, ByVal Status As String, ByVal IsUse As Char, ByVal TobeDeleted As Char, ByVal URL As String, ByVal BlockId As String, ByVal IsInitialBlock As Char, ByVal PCC As String, ByVal Profile As String, ByVal System As String, ByVal Password As String)

        Dim oCommand As SqlCommand = Nothing
        Dim strSQL As String = ""

        Try

            oCommand = New SqlCommand("uspInsertNewSession", moConn)

            With oCommand
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = 60
                With oCommand.Parameters

                    .Add(New SqlParameter("@SessionID", SqlDbType.NVarChar, 450, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, SessionID))
                    .Add(New SqlParameter("@SequenceNo", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, SequenceNo))
                    .Add(New SqlParameter("@GDS", SqlDbType.NVarChar, 15, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, GDS))
                    .Add(New SqlParameter("@CreatedTime", SqlDbType.DateTime, 8, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, CreatedTime))
                    .Add(New SqlParameter("@LastMessageTime", SqlDbType.DateTime, 8, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, LastMessageTime))
                    .Add(New SqlParameter("@UserName", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, UserName))
                    .Add(New SqlParameter("@UserID", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, UserID))
                    .Add(New SqlParameter("@Status", SqlDbType.NVarChar, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, Status))
                    .Add(New SqlParameter("@ToBeDeleted", SqlDbType.Char, 1, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, TobeDeleted))
                    .Add(New SqlParameter("@IsUse", SqlDbType.Char, 1, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, IsUse))
                    .Add(New SqlParameter("@URL", SqlDbType.NVarChar, 50, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, URL))
                    .Add(New SqlParameter("@BlockId", SqlDbType.NVarChar, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, BlockId))
                    .Add(New SqlParameter("@IsInitialBlock", SqlDbType.Char, 1, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, IsInitialBlock))
                    .Add(New SqlParameter("@PCC", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, PCC))
                    .Add(New SqlParameter("@Profile", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, Profile))
                    .Add(New SqlParameter("@System", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, System))
                    .Add(New SqlParameter("@Password", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, Password))


                End With

                Dim numberOfRows As Integer = .ExecuteNonQuery()

                Dim a As Integer = numberOfRows

            End With

        Catch ex As Exception
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try

    End Sub

    Public Sub InsertNewSession(ByVal SessionID As String, ByVal SequenceNo As Integer, ByVal GDS As String, ByVal CreatedTime As DateTime, ByVal LastMessageTime As DateTime, ByVal UserName As String, ByVal Status As String, ByVal IsUse As Char, ByVal TobeDeleted As Char, ByVal URL As String, ByVal BlockId As String, ByVal IsInitialBlock As Char, ByVal PCC As String, ByVal Profile As String, ByVal System As String, ByVal Password As String)

        Dim oCommand As SqlCommand = Nothing
        Dim strSQL As String = ""

        Try

            oCommand = New SqlCommand("uspInsertNewSession", moConn)

            With oCommand
                .CommandType = CommandType.StoredProcedure
                .CommandTimeout = 60
                With oCommand.Parameters

                    .Add(New SqlParameter("@SessionID", SqlDbType.NVarChar, 4000, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, SessionID))
                    .Add(New SqlParameter("@SequenceNo", SqlDbType.Int, 4, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, SequenceNo))
                    .Add(New SqlParameter("@GDS", SqlDbType.NVarChar, 15, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, GDS))
                    .Add(New SqlParameter("@CreatedTime", SqlDbType.DateTime, 8, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, CreatedTime))
                    .Add(New SqlParameter("@LastMessageTime", SqlDbType.DateTime, 8, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, LastMessageTime))
                    .Add(New SqlParameter("@UserName", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, UserName))
                    .Add(New SqlParameter("@Status", SqlDbType.NVarChar, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, Status))
                    .Add(New SqlParameter("@ToBeDeleted", SqlDbType.Char, 1, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, TobeDeleted))
                    .Add(New SqlParameter("@IsUse", SqlDbType.Char, 1, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, IsUse))
                    .Add(New SqlParameter("@URL", SqlDbType.NVarChar, 50, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, URL))
                    .Add(New SqlParameter("@BlockId", SqlDbType.NVarChar, 10, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, BlockId))
                    .Add(New SqlParameter("@IsInitialBlock", SqlDbType.Char, 1, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, IsInitialBlock))
                    .Add(New SqlParameter("@PCC", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, PCC))
                    .Add(New SqlParameter("@Profile", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, Profile))
                    .Add(New SqlParameter("@System", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, System))
                    .Add(New SqlParameter("@Password", SqlDbType.NVarChar, 20, ParameterDirection.Input, False, 0, 0, "", DataRowVersion.Current, Password))


                End With

                .ExecuteNonQuery()

            End With

        Catch ex As Exception
            Throw ex
        Finally
            If Not oCommand Is Nothing Then
                oCommand.Dispose()
                oCommand = Nothing
            End If
        End Try

    End Sub

End Class



Public Class UpdateTACount

    Private Delegate Sub TAIncrementDelegate()
    Private TAIncrementWrapper As New TAIncrementDelegate(AddressOf TACount_Method)
    Private _TAIncrement As Integer = 0
    Private _TASupplierID As String = ""
    Private _TAOwnerID As String = ""

    Public Sub New()
    End Sub

    Public Sub BeginTACount()
        Dim cbr As New AsyncCallback(AddressOf EndTACount)
        Dim arr As IAsyncResult = TAIncrementWrapper.BeginInvoke(cbr, Nothing)
        'TACount_Method()
    End Sub

    Private Sub EndTACount(ByVal aBr As IAsyncResult)
        TAIncrementWrapper.EndInvoke(aBr)
        aBr.AsyncWaitHandle.Close()
    End Sub

    Private Sub TACount_Method()
        Dim Server As String = ""
        Dim Database As String = ""
        Dim User As String = ""
        Dim Password As String = ""
        Dim CnnString As String = ""
        Dim sb As StringBuilder = Nothing

        CnnString = WebConfigurationManager.AppSettings("ConnectionString").Trim

        If CnnString.Length = 0 Then
            Server = WebConfigurationManager.AppSettings("Server")
            Database = WebConfigurationManager.AppSettings("Database")
            User = WebConfigurationManager.AppSettings("User")
            Password = WebConfigurationManager.AppSettings("Password")

            sb = New StringBuilder()
            sb.Append("data source=").Append(Server).Append(";initial Catalog=").Append(Database).Append(";User ID=").Append(User).Append(";Password=").Append(Password)

            CnnString = sb.ToString()
            sb = Nothing
            'CnnString = "data source=***REMOVED***\DEDICATED8-VM6,1433;initial Catalog=Traveltalk;User ID=***REMOVED***;Password=***REMOVED***"
        End If

        Dim cmd As New SqlCommand() : Dim conn As New SqlConnection(CnnString)
        Try
            conn.Open()
            Dim cmdBuild As New StringBuilder("<MessageContent>")
            cmdBuild.Append("<TA_Add>" & TAIncrement & "</TA_Add>")
            cmdBuild.Append("<TAUser>" & TAOwnerID & "</TAUser>")
            cmdBuild.Append("<TAOwner>" & TASupplierID & "</TAOwner>")
            cmdBuild.Append("</MessageContent>")

            cmd.Connection = conn
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_BeginTACount"
            cmd.Parameters.Add("@Message", SqlDbType.VarBinary)
            cmd.Parameters("@Message").Value = Encoding.Unicode.GetBytes(cmdBuild.ToString)
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            If Not conn.State = ConnectionState.Closed Then conn.Close()
        Catch ex As Exception
            If Not conn.State = ConnectionState.Closed Then conn.Close()
            'Log or handle Error here
        End Try
    End Sub

    Public Property TASupplierID() As String
        Get
            Return _TASupplierID
        End Get
        Set(ByVal value As String)
            _TASupplierID = value
        End Set
    End Property
    Public Property TAOwnerID() As String
        Get
            Return _TAOwnerID
        End Get
        Set(ByVal value As String)
            _TAOwnerID = value
        End Set
    End Property


    Public Property TAIncrement() As Integer
        Get
            Return _TAIncrement
        End Get
        Set(ByVal value As Integer)
            _TAIncrement = value
        End Set
    End Property
End Class

