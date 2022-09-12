Imports TripXMLMain
Imports System.Xml
Imports Portal
Imports System.Text

Public Class PortalAirServices
    Private sb As StringBuilder = New StringBuilder()

    Private mstrRequest As String = ""
    Private mstrVersion As String = ""
    Private mstrXslPath As String = ""
    Private ttProviderSystems As TripXMLProviderSystems

    Public Property Request() As String
        Get
            Return mstrRequest
        End Get
        Set(ByVal Value As String)
            mstrRequest = Value
        End Set
    End Property

    Public Property Version() As String
        Get
            Return mstrVersion
        End Get
        Set(ByVal Value As String)
            mstrVersion = Value
            If mstrVersion.Length > 0 Then mstrVersion &= "_"
        End Set
    End Property

    Public Property XslPath() As String
        Get
            Return mstrXslPath
        End Get
        Set(ByVal Value As String)
            mstrXslPath = sb.Append(Value).Append("Portal\").ToString()
            sb.Remove(0, sb.Length())
        End Set
    End Property

    Public Property ProviderSystems() As TripXMLProviderSystems
        Get
            Return ttProviderSystems
        End Get
        Set(ByVal Value As TripXMLProviderSystems)
            ttProviderSystems = Value
        End Set
    End Property

    Public Function AirAvail() As String
        Dim ttPA As PortalAdapter = Nothing
        Dim strRequest As String = ""
        Dim strResponse As String = ""

        '*****************************************************************
        ' Transform OTA AirAvail Request into Native Portal Request     *
        '***************************************************************** 

        Try
            strRequest = mstrRequest

            strRequest = CoreLib.TransformXML(strRequest, mstrXslPath, sb.Append(mstrVersion).Append("Portal_AirAvailRQ.xsl").ToString())
            sb.Remove(0, sb.Length())
        Catch ex As Exception
            Throw New Exception(sb.Append("Error Transforming OTA Request.").Append(vbNewLine).Append(ex.Message).ToString())
            sb.Remove(0, sb.Length())
        End Try

        If strRequest.Trim.Length = 0 Then
            Throw New Exception("Transformation produced empty xml.")
        ElseIf strRequest.Substring(0, 4) = "ERR:" Then
            If strRequest = "ERR:" Then
                Throw New Exception("Transformation produced empty xml.")
            Else
                Throw New Exception(strRequest.Substring(4))
            End If
        End If

        '*******************************************************************************
        ' Send Transformed Request to the Portal Adapter and Getting Native Response  *
        '******************************************************************************* 

        Try
            ttPA = New PortalAdapter(ttProviderSystems)
            strResponse = ttPA.SendMessage(strRequest, "")
        Catch ex As Exception
            Throw ex
        Finally
            ttPA = Nothing
        End Try

        '*****************************************************************
        ' Transform Native Portal AirAvail Response into OTA Response   *
        '***************************************************************** 

        Try
            strResponse = CoreLib.TransformXML(strResponse, mstrXslPath, sb.Append(mstrVersion).Append("Portal_AirAvailRS.xsl").ToString())
            sb.Remove(0, sb.Length())

        Catch ex As Exception
            Throw New Exception(sb.Append("Error Transforming Native Response.").Append(vbNewLine).Append(ex.Message).ToString())
            sb.Remove(0, sb.Length())
        End Try

        Return strResponse
        sb = Nothing
    End Function

    Public Function AirFlifo() As String
        Dim ttPA As PortalAdapter = Nothing
        Dim strRequest As String = ""
        Dim strResponse As String = ""

        '*****************************************************************
        ' Transform OTA AirFlifo Request into Native Portal Request     *
        '***************************************************************** 

        Try
            strRequest = mstrRequest

            strRequest = CoreLib.TransformXML(strRequest, mstrXslPath, sb.Append(mstrVersion).Append("Portal_AirFlifoRQ.xsl").ToString())
            sb.Remove(0, sb.Length())
        Catch ex As Exception
            Throw New Exception(sb.Append("Error Transforming OTA Request.").Append(vbNewLine).Append(ex.Message).ToString())
            sb.Remove(0, sb.Length())
        End Try

        If strRequest.Trim.Length = 0 Then
            Throw New Exception("Transformation produced empty xml.")
        ElseIf strRequest.Substring(0, 4) = "ERR:" Then
            If strRequest = "ERR:" Then
                Throw New Exception("Transformation produced empty xml.")
            Else
                Throw New Exception(strRequest.Substring(4))
            End If
        End If

        '*******************************************************************************
        ' Send Transformed Request to the Portal Adapter and Getting Native Response  *
        '******************************************************************************* 

        Try
            ttPA = New PortalAdapter(ttProviderSystems)
            strResponse = ttPA.SendMessage(strRequest, "")
        Catch ex As Exception
            Throw ex
        Finally
            ttPA = Nothing
        End Try

        '*****************************************************************
        ' Transform Native Portal AirFlifo Response into OTA Response   *
        '***************************************************************** 

        Try
            strResponse = CoreLib.TransformXML(strResponse, mstrXslPath, sb.Append(mstrVersion).Append("Portal_AirFlifoRS.xsl").ToString())
            sb.Remove(0, sb.Length())

        Catch ex As Exception
            Throw New Exception(sb.Append("Error Transforming Native Response.").Append(vbNewLine).Append(ex.Message).ToString())
            sb.Remove(0, sb.Length())
        End Try
        sb = Nothing
        Return strResponse

    End Function

    Public Function LowFare() As String
        Dim ttPA As PortalAdapter = Nothing
        Dim strRequest As String = ""
        Dim strResponse As String = ""
        Dim Token As String = ""

        '*****************************************************************
        ' Transform OTA LowFare Request into Native Portal Request     *
        '***************************************************************** 
        Try
            Try
                strRequest = mstrRequest
                strRequest = CoreLib.TransformXML(strRequest, mstrXslPath, sb.Append(mstrVersion).Append("Portal_LowFareRQ.xsl").ToString())
                sb.Remove(0, sb.Length())
            Catch ex As Exception
                Throw New Exception(sb.Append("Error Transforming OTA Request.").Append(vbNewLine).Append(ex.Message).ToString())
                sb.Remove(0, sb.Length())
            End Try

            If strRequest.Trim.Length = 0 Then
                Throw New Exception("Transformation produced empty xml.")
            Else
                strRequest = strRequest.Replace(" xmlns=""""", "")
            End If

            ' Create Session
            Try
                ttPA = New PortalAdapter(ttProviderSystems)
                Token = ttPA.CreateSession()
            Catch ex As Exception
                Throw New Exception(sb.Append("Unable to Create Session.").Append(vbNewLine).Append(ex.Message).ToString())
                sb.Remove(0, sb.Length())
            End Try

            ' add session token to request

            'strRequest = "<SearchForPackages xmlns=""http://odyssey.com/webservices/""><requestObj xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><MaxSearchResults>0</MaxSearchResults><InternationalFlight>false</InternationalFlight><InternationalPublishedMarkup>0</InternationalPublishedMarkup><InternationalPrivateMarkup>0</InternationalPrivateMarkup><DomesticPublishedMarkup>0</DomesticPublishedMarkup><DomesticPrivateMarkup>0</DomesticPrivateMarkup><InternationalPublishedMarkupPercent>0</InternationalPublishedMarkupPercent><InternationalPrivateMarkupPercent>0</InternationalPrivateMarkupPercent><DomesticPublishedMarkupPercent>0</DomesticPublishedMarkupPercent><DomesticPrivateMarkupPercent>0</DomesticPrivateMarkupPercent><DirectFlight>false</DirectFlight><AirSearchObj><AirSearchObject><DepartureDateTime>2007-12-09T00:00:00</DepartureDateTime><SearchFromDepDateTime>0001-01-01T00:00:00</SearchFromDepDateTime><SearchAfterDepDateTime>0001-01-01T00:00:00</SearchAfterDepDateTime><ArrivalDateTime>0001-01-01T00:00:00</ArrivalDateTime><DepartureCity>MIA</DepartureCity><ArrivalCity>ATL</ArrivalCity></AirSearchObject><AirSearchObject><DepartureDateTime>2007-12-14T00:00:00</DepartureDateTime><SearchFromDepDateTime>0001-01-01T00:00:00</SearchFromDepDateTime><SearchAfterDepDateTime>0001-01-01T00:00:00</SearchAfterDepDateTime><ArrivalDateTime>0001-01-01T00:00:00</ArrivalDateTime><DepartureCity>ATL</DepartureCity><ArrivalCity>MIA</ArrivalCity></AirSearchObject></AirSearchObj><AirPackage><internationalFlight>false</internationalFlight><DiscountApplied>0</DiscountApplied><AirJourneyType>	<ID>0</ID><SupplierID>0</SupplierID><LanguageID>0</LanguageID><PseudoCode>Circle</PseudoCode><Active>false</Active></AirJourneyType><PaperTicketRequested>false</PaperTicketRequested><SequenceNumber>0</SequenceNumber><Flights /><ClassTypeChanged>false</ClassTypeChanged><CommissionPercent>0</CommissionPercent><TicketIssueDate>0001-01-01T00:00:00</TicketIssueDate><PaymentSchedule /><DepartureDateTime>0001-01-01T00:00:00.0000000-05:00</DepartureDateTime><ArrivalDateTime>0001-01-01T00:00:00</ArrivalDateTime><Duration>0</Duration></AirPackage><ClassType>Economy</ClassType><RefundableTicket>false</RefundableTicket><UserID>1</UserID><ApplicationID>0</ApplicationID><AdultCount>1</AdultCount><ChildCount>0</ChildCount><YouthCount>0</YouthCount><SeniorCount>0</SeniorCount><InfantCount>0</InfantCount><SeatedInfantCount>0</SeatedInfantCount><CurrencyID>USD</CurrencyID><SiteItemID>0</SiteItemID><SiteItemTypeID>0</SiteItemTypeID><ParentSiteItemID>0</ParentSiteItemID><LanguageID>1</LanguageID></requestObj></SearchForPackages>"

            strRequest = strRequest.Replace("</SearchForPackages>", sb.Append("<sessionID>").Append(Token).Append("</sessionID></SearchForPackages>").ToString())
            sb.Remove(0, sb.Length())

            '*******************************************************************************
            ' Send Transformed Request to the Portal Adapter and Getting Native Response  *
            '******************************************************************************* 

            Try
                strResponse = ttPA.SendMessage(strRequest, """http://BBTrip.com/webservices/SearchForPackages""")
            Catch ex As Exception
                Throw ex
            End Try

            '*****************************************************************
            ' Transform Native Portal LowFare Response into OTA Response   *
            '***************************************************************** 

            Try
                strResponse = strResponse.Replace("</SearchForPackagesResponse>", sb.Append("<SessionID>").Append(Token).Append("</SessionID></SearchForPackagesResponse>").ToString())
                sb.Remove(0, sb.Length())
                strResponse = CoreLib.TransformXML(strResponse, mstrXslPath, sb.Append(mstrVersion).Append("Portal_LowFareRS.xsl").ToString())
                sb.Remove(0, sb.Length())
            Catch ex As Exception
                Throw New Exception(sb.Append("Error Transforming Native Response.").Append(vbNewLine).Append(ex.Message).ToString())
                sb.Remove(0, sb.Length())
            End Try

            If strResponse.Trim.Length = 0 Then
                Throw New Exception("Transformation produced empty xml.")
            End If

            ' Close Session
            Try
                ttPA.CloseSession(Token)
            Catch ex As Exception
                Throw New Exception(sb.Append("Unable to Close Session.").Append(vbNewLine).Append(ex.Message).ToString())
                sb.Remove(0, sb.Length())
            Finally
                ttPA = Nothing
            End Try

            If Not ttPA Is Nothing Then
                ttPA = Nothing
            End If

            Return strResponse

        Catch exx As Exception
            Throw exx
        Finally

            If Token.Trim.Length > 0 Then
                If Not ttPA Is Nothing Then ttPA.CloseSession(Token)
            End If

            ttPA = Nothing

        End Try
        sb = Nothing
    End Function

    Public Function AirSeatMap() As String
        Dim ttPA As PortalAdapter = Nothing
        Dim strRequest As String = ""
        Dim strResponse As String = ""

        '*****************************************************************
        ' Transform OTA AirSeatMap Request into Native Portal Request     *
        '***************************************************************** 

        Try
            strRequest = mstrRequest

            strRequest = CoreLib.TransformXML(strRequest, mstrXslPath, sb.Append(mstrVersion).Append("Portal_AirSeatMapRQ.xsl").ToString())
            sb.Remove(0, sb.Length())
        Catch ex As Exception
            Throw New Exception(sb.Append("Error Transforming OTA Request.").Append(vbNewLine).Append(ex.Message).ToString())
            sb.Remove(0, sb.Length())
        End Try

        If strRequest.Trim.Length = 0 Then
            Throw New Exception("Transformation produced empty xml.")
        ElseIf strRequest.Substring(0, 4) = "ERR:" Then
            If strRequest = "ERR:" Then
                Throw New Exception("Transformation produced empty xml.")
            Else
                Throw New Exception(strRequest.Substring(4))
            End If
        End If

        '*******************************************************************************
        ' Send Transformed Request to the Portal Adapter and Getting Native Response  *
        '******************************************************************************* 

        Try
            ttPA = New PortalAdapter(ttProviderSystems)
            strResponse = ttPA.SendMessage(strRequest, "")
        Catch ex As Exception
            Throw ex
        Finally
            ttPA = Nothing
        End Try

        '*****************************************************************
        ' Transform Native Portal AirSeatMap Response into OTA Response   *
        '***************************************************************** 

        Try
            strResponse = CoreLib.TransformXML(strResponse, mstrXslPath, sb.Append(mstrVersion).Append("Portal_AirSeatMapRS.xsl").ToString())
            sb.Remove(0, sb.Length())

        Catch ex As Exception
            Throw New Exception(sb.Append("Error Transforming Native Response.").Append(vbNewLine).Append(ex.Message).ToString())
            sb.Remove(0, sb.Length())
        End Try

        Return strResponse
        sb = Nothing
    End Function

    Public Function AirPrice() As String
        Dim ttPA As PortalAdapter = Nothing
        Dim strRequest As String = ""
        Dim strResponse As String = ""

        '*****************************************************************
        ' Transform OTA AirPrice Request into Native Portal Request     *
        '***************************************************************** 

        Try
            strRequest = mstrRequest

            strRequest = CoreLib.TransformXML(strRequest, mstrXslPath, sb.Append(mstrVersion).Append("Portal_AirPriceRQ.xsl").ToString())
            sb.Remove(0, sb.Length())
        Catch ex As Exception
            Throw New Exception(sb.Append("Error Transforming OTA Request.").Append(vbNewLine).Append(ex.Message).ToString())
            sb.Remove(0, sb.Length())
        End Try

        If strRequest.Trim.Length = 0 Then
            Throw New Exception("Transformation produced empty xml.")
        ElseIf strRequest.Substring(0, 4) = "ERR:" Then
            If strRequest = "ERR:" Then
                Throw New Exception("Transformation produced empty xml.")
            Else
                Throw New Exception(strRequest.Substring(4))
            End If
        End If

        '*******************************************************************************
        ' Send Transformed Request to the Portal Adapter and Getting Native Response  *
        '******************************************************************************* 

        Try
            ttPA = New PortalAdapter(ttProviderSystems)
            strResponse = ttPA.SendMessage(strRequest, "")
        Catch ex As Exception
            Throw ex
        Finally
            ttPA = Nothing
        End Try

        '*****************************************************************
        ' Transform Native Portal AirPrice Response into OTA Response   *
        '***************************************************************** 

        Try
            strResponse = CoreLib.TransformXML(strResponse, mstrXslPath, sb.Append(mstrVersion).Append("Portal_AirPriceRS.xsl").ToString())
            sb.Remove(0, sb.Length())

        Catch ex As Exception
            Throw New Exception(sb.Append("Error Transforming Native Response.").Append(vbNewLine).Append(ex.Message).ToString())
            sb.Remove(0, sb.Length())
        End Try

        Return strResponse
        sb = Nothing
    End Function

    Public Function AirRules() As String
        Dim ttPA As PortalAdapter = Nothing
        Dim oReqDoc As XmlDocument = Nothing
        Dim oRoot As XmlElement = Nothing
        Dim oNode As XmlNode = Nothing
        Dim strRequest As String = ""
        Dim strResponse As String = ""
        Dim Token As String = ""

        Try

            '*****************************************************************
            ' Transform OTA AirRules Request into Native Portal Request     *
            '***************************************************************** 

            Try
                strRequest = mstrRequest

                strRequest = CoreLib.TransformXML(strRequest, mstrXslPath, sb.Append(mstrVersion).Append("Portal_AirRulesRQ.xsl").ToString())
                sb.Remove(0, sb.Length())
            Catch ex As Exception
                Throw New Exception(sb.Append("Error Transforming OTA Request.").Append(vbNewLine).Append(ex.Message).ToString())
                sb.Remove(0, sb.Length())
            End Try

            If strRequest.Trim.Length = 0 Then
                Throw New Exception("Transformation produced empty xml.")
            End If

            Try
                oReqDoc = New XmlDocument
                oReqDoc.LoadXml(strRequest)
                oRoot = oReqDoc.DocumentElement

                ' AirRules Tariff Request
                oNode = oRoot.SelectSingleNode("FareQuoteTariffDisplay_8_0")
                strRequest = oNode.OuterXml
            Catch ex As Exception
                Throw New Exception(sb.Append("Error Loading Transformed Request XML Document. ").Append(ex.Message).ToString())
                sb.Remove(0, sb.Length())
            End Try

            ' Create Session
            Try
                ttPA = New PortalAdapter(ttProviderSystems)
                Token = ttPA.CreateSession()
            Catch ex As Exception
                Throw New Exception(sb.Append("Unable to Create Session.").Append(vbNewLine).Append(ex.Message).ToString())
                sb.Remove(0, sb.Length())
            End Try

            '*******************************************************************
            ' Send Transformed Request AirRules Tariff to the Portal Adapter  *
            '******************************************************************* 

            Try
                strResponse = ttPA.SendMessage(strRequest, "")
            Catch ex As Exception
                Throw ex
            End Try

            If strResponse.IndexOf("NO FARES FOUND FOR INPUT REQUEST") = -1 Then

                '*******************************************************************
                ' Send Transformed Request AirRules Tariff to the Portal Adapter  *
                '******************************************************************* 
                Try
                    ' AirRules Request
                    oNode = oRoot.SelectSingleNode("FareQuoteRulesDisplay_8_0")
                    strRequest = oNode.OuterXml
                    strResponse = ttPA.SendMessage(strRequest, "")
                Catch ex As Exception
                    Throw ex
                End Try

            End If

            '*****************************************************************
            ' Transform Native Portal AirRules Response into OTA Response   *
            '***************************************************************** 

            Try
                strResponse = CoreLib.TransformXML(strResponse, mstrXslPath, sb.Append(mstrVersion).Append("Portal_AirRulesRS.xsl").ToString())
                sb.Remove(0, sb.Length())

            Catch ex As Exception
                Throw New Exception(sb.Append("Error Transforming Native Response. ").Append(ex.Message).ToString())
                sb.Remove(0, sb.Length())
            End Try

            Return strResponse

        Catch exx As Exception
            Throw exx
        Finally
            oNode = Nothing
            oRoot = Nothing
            oReqDoc = Nothing

            'If Token.Trim.Length > 0 Then
            '    If Not ttPA Is Nothing Then ttPA.CloseSession(Token)
            'End If

            ttPA = Nothing

        End Try
        sb = Nothing
    End Function

    Public Function LowFarePlus() As String
        'Dim ttPA As PortalAdapter = Nothing
        'Dim strRequest As String = ""
        Dim strResponse As String = "<SearchPromotionsResponse>" + mstrRequest.Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "").Replace("<?xml version=""1.0""?>", "") + "</SearchPromotionsResponse>"
        'Dim Token As String = ""
        'Dim oDoc As XmlDocument = Nothing
        'Dim oRoot As XmlElement = Nothing
        'Dim oNode As XmlNode = Nothing
        'Dim oDA As cDA = Nothing
        'Dim markupXML As String

        Try
            'Try
            '    strRequest = mstrRequest
            '    strResponse = CoreLib.TransformXML(strRequest, mstrXslPath, mstrVersion & "Portal_SearchPromotionsRQ.xsl")
            '    CoreLib.SendTrace(ttProviderSystems.UserID, "PortalService", "PromoRequest", strResponse)

            '    'LogMessageToFile("Markups", "<MarkupTransformed>" & strRequest & strResponse & "</MarkupTransformed>", reqTime, reqTime)

            'Catch ex As Exception
            '    'LogMessageToFile("Markups", "<MarkupEx1>" & strRequest & ex.Message & "</MarkupEx1>", reqTime, reqTime)
            '    Throw New Exception("Error Transforming Request." & vbNewLine & ex.Message)
            'End Try

            'If strResponse.Trim.Length = 0 Then
            '    Throw New Exception("Transformation produced empty xml.")
            'End If

            'oDoc = New XmlDocument
            'oDoc.LoadXml(strResponse)
            'oRoot = oDoc.DocumentElement
            'oNode = oRoot.SelectSingleNode("promoSearch")

            'Try
            '    oDA = New cDA("BEConnectionString")

            '    markupXML = oDA.GetMarkups(oNode.SelectSingleNode("Affiliate").InnerText, oRoot.SelectSingleNode("catalog").InnerText, oRoot.SelectSingleNode("languageId").InnerText, _
            '                oRoot.SelectSingleNode("currencyId").InnerText, "0", "0", "0", "0", Nothing, Convert.ToDateTime(oNode.SelectSingleNode("DepartureStartDate").InnerText), _
            '                oNode.SelectSingleNode("DepartureEndDate").InnerText, Nothing, Nothing, 0, 1, oNode.SelectSingleNode("SeniorCount").InnerText, oNode.SelectSingleNode("AdultCount").InnerText, _
            '                oNode.SelectSingleNode("ChildCount").InnerText, oNode.SelectSingleNode("BookingDate").InnerText, 0, 0, oNode.SelectSingleNode("ProductTypeID").InnerText, _
            '                Nothing, Nothing, Nothing, Nothing, Nothing, oNode.SelectSingleNode("Affiliate").InnerText, 0, oNode.SelectSingleNode("OfficeID/string").InnerText, Nothing, Nothing, Nothing, Nothing)

            'Catch ex As Exception
            '    Throw New Exception("Cannot get markups. " & ex.Message)
            'Finally
            '    If Not oDA Is Nothing Then
            '        oDA.Dispose()
            '        oDA = Nothing
            '    End If
            'End Try

            'markupXML = markupXML.Replace("</SearchPromotionsResponse>", mstrRequest.Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "") & "</SearchPromotionsResponse>")
            'strResponse = markupXML
            'strResponse = strResponse.Replace(",", ".")

            'CoreLib.SendTrace(ttProviderSystems.UserID, "PortalService", "PromoResponse", strResponse)

            ''If strResponse.Length < 10000 Then
            ''    LogMessageToFile("Markups", "<MarkupSO><respMarkup>" & strResponse & "</respMarkup></MarkupSO>", reqTime, reqTime)
            ''Else
            ''    LogMessageToFile("Markups", "<MarkupSO><respLen>" & strResponse.Length.ToString() & "</respLen></MarkupSO>", reqTime, reqTime)
            ''End If

            Return strResponse

        Catch exx As Exception
            Throw exx

        End Try

    End Function

    Public Sub LogMessageToFile(ByVal MsgType As String, ByRef Message As String, ByVal RequestTime As Date, ByVal ResponseTime As Date)

        Dim FileNumber As Integer
        Dim strLine As String = ""

        Try
            strLine = "<Message"
            strLine &= " Type='" & MsgType & "'"
            strLine &= " RequestTime='" & RequestTime.ToString("dd MMM yyyy HH:mm:ss") & "'"
            strLine &= " ResponseTime='" & ResponseTime.ToString("dd MMM yyyy HH:mm:ss") & "'"
            Dim dur As TimeSpan
            dur = ResponseTime - RequestTime
            strLine &= " Duration='" & dur.TotalSeconds.ToString() & "'>"
            strLine &= "<PortalMessage>" & Message & "</PortalMessage>"
            strLine &= "</Message>"

            FileNumber = FreeFile()

            FileOpen(FileNumber, mstrXslPath + "\Log\Log.txt", OpenMode.Append)
            PrintLine(FileNumber, strLine)
            FileClose(FileNumber)
        Catch ex As Exception
            ' 
        End Try

    End Sub

End Class
