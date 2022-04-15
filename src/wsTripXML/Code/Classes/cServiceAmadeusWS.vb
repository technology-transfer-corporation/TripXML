Imports TripXMLMain
Imports System.Xml
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    Public Class cServiceAmadeusWS

        Public Event GotResponse(ByVal Response As String)

#Region " Properties "
        Private sb As StringBuilder = New StringBuilder()

        Public ttProviderSystems As TripXMLProviderSystems

        Public Property ServiceID() As Integer

        Public Property Request() As String = ""

        Public Property Version() As String = ""

#End Region

        Public Sub SendAirRequest()
            Dim ttService As AmadeusWS.AirServices = Nothing
            Dim strResponse As String = ""
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oDocReq As XmlDocument = Nothing
            Dim oRootReq As XmlElement = Nothing
            Dim oDocReq1 As XmlDocument = Nothing
            Dim oRootReq1 As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing
            Dim oNode1 As XmlNode = Nothing
            Dim aClassNode As XmlNode = Nothing
            Dim oBLNode As XmlNode = Nothing
            Dim oAttr As XmlAttribute
            Dim strMsg As String = ""
            Dim strPCC As String = ""
            Dim strBLPCC As String = ""
            Dim i As Integer
            Dim dt As DataTable = Nothing

            Try
                ttService = New AmadeusWS.AirServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ttProviderSystems = ttProviderSystems

                    oDocReq = New XmlDocument
                    oDocReq.LoadXml(Request)
                    oRootReq = oDocReq.DocumentElement
                    oNode = oRootReq.SelectSingleNode("POS/Source")
                    'oNode1 = oRootReq.SelectSingleNode("POS/Source")


                    If oNode.Attributes.ItemOf("PseudoCityCode") Is Nothing Then
                        oAttr = oDocReq.CreateAttribute("PseudoCityCode")
                        oNode.Attributes.Append(oAttr)
                    End If


                    'If mintServiceID = ttService.LowFarePlus Then
                    dt = New DataTable
                    dt.Columns.Add("Airline")
                    dt.Columns.Add("Class")
                    dt.Columns.Add("PreferLevel")

                    'Filter Classes
                    Dim str2 As String = ""
                    For Each oNode1 In oRootReq.SelectNodes("TravelPreferences/VendorPref[@Code!='']")
                        Dim str1 As String = oNode1.Attributes.ItemOf("Code").InnerText

                        For Each aClassNode In oNode1.SelectNodes("AClasses/AClass")
                            If oNode1.SelectSingleNode("AClasses").Attributes.ItemOf("PreferLevel") Is Nothing Then
                                str2 = ""
                            Else
                                str2 = oNode1.ChildNodes(0).Attributes.ItemOf("PreferLevel").InnerText.ToLower
                            End If
                            Dim dr As DataRow = dt.NewRow()
                            dr("Airline") = str1
                            dr("Class") = aClassNode.InnerXml.ToString
                            If str2 <> "unacceptable" Then
                                dr("PreferLevel") = "1"
                            Else
                                dr("PreferLevel") = "0"
                            End If
                            dt.Rows.Add(dr)
                        Next
                    Next
                    'End If


                    oNode.Attributes.ItemOf("PseudoCityCode").InnerText = .ttProviderSystems.PCC
                    Request = oRootReq.OuterXml

                    Select Case ServiceID
                        Case ttServices.AirAvail
                            strMsg = "AirAvail"
                        Case ttServices.LowFare
                            strMsg = "LowFare"
                        Case ttServices.LowFarePlus
                            strMsg = "LowFare"
                        Case ttServices.LowFareSchedule
                            strMsg = "LowFare"
                        Case ttServices.AirSchedule
                            strMsg = "AirSchedule"
                        Case ttServices.LowFareMatrix
                            strMsg = "LowFare"
                        Case ttServices.LowFareFlights
                            strMsg = "LowFareFlights"
                        Case ttServices.LowOfferMatrix
                            strMsg = "LowFare"
                        Case ttServices.LowOfferSearch
                            strMsg = "LowFare"
                        Case Else
                            Throw New Exception("Invalid request or message not supported by Amadeus air services.")
                    End Select

                    Dim EKBP As Boolean = False

                    If ttProviderSystems.BLFile <> "" Then
                        oDoc = New XmlDocument
                        ' Load Access Control List into memory
                        Try
                            oDoc.Load(ttProviderSystems.BLFile)
                        Catch exr As Exception
                            CoreLib.SendTrace("", "cServiceAmadeusWS", "Error Loading business logic file", exr.Message, String.Empty)
                            Throw exr
                        End Try

                        oRoot = oDoc.DocumentElement
                        ' check "In" business logic
                        sb.Append("Message[@Name='").Append(strMsg).Append("'][@Direction='In']")
                        oNode = oRoot.SelectSingleNode(sb.ToString())
                        sb.Remove(0, sb.Length)

                        If Not oNode Is Nothing Then

                            If ttProviderSystems.PCC.StartsWith("*") Then
                                ttProviderSystems.PCC = ttProviderSystems.PCC.Substring(1)
                                EKBP = True
                            End If

                            sb.Append("ProviderBL[@Name='Amadeus'][@System='").Append(ttProviderSystems.System).Append("'][@PCC='").Append(ttProviderSystems.PCC.ToUpper).Append("']")
                            oNode = oNode.SelectSingleNode(sb.ToString())
                            sb.Remove(0, sb.Length)

                            If Not oNode Is Nothing Then
                                sb.Append(XslPath).Append("BL\")
                                Request = BusinessLogicIn(Request, oNode.OuterXml, sb.ToString(), strMsg)
                                sb.Remove(0, sb.Length)
                            Else
                                sb.Append("Message[@Name='").Append(strMsg).Append("'][@Direction='In']")
                                oNode = oRoot.SelectSingleNode(sb.ToString())
                                sb.Remove(0, sb.Length)
                                sb.Append("ProviderBL[@Name='Amadeus'][@System='").Append(ttProviderSystems.System).Append("'][contains(@PCC,'*')]")
                                oNode = oNode.SelectSingleNode(sb.ToString())
                                sb.Remove(0, sb.Length)

                                If Not oNode Is Nothing Then
                                    strBLPCC = oNode.SelectSingleNode("@PCC").InnerText
                                    strPCC = ttProviderSystems.PCC.ToUpper

                                    sb.Append("Preferences/Except[PCC='").Append(strPCC).Append("']")
                                    If oNode.SelectSingleNode(sb.ToString()) Is Nothing Then
                                        If strBLPCC.Length = strPCC.Length Then
                                            For i = 0 To strBLPCC.Length - 1
                                                If strBLPCC.Substring(i, 1) = "*" Then
                                                    strPCC = strPCC.Remove(i, 1)
                                                    strPCC = strPCC.Insert(i, "*")
                                                End If
                                            Next

                                            If strBLPCC = strPCC Then

                                                If EKBP Then
                                                    Request = Request.Replace("PseudoCityCode=""*", "PseudoCityCode=""")
                                                    Request = Request.Replace("OTA_AirLowFareSearchPlusRQ", "OTA_AirLowFareSearchRQ")
                                                    'mintServiceID = ttServices.LowFare
                                                End If

                                                sb.Remove(0, sb.Length)
                                                sb.Append(XslPath).Append("BL\")
                                                Request = BusinessLogicIn(Request, oNode.OuterXml, sb.ToString(), strMsg)
                                            End If
                                        End If
                                    End If
                                    sb.Remove(0, sb.Length)
                                End If
                            End If
                        End If
                        oDoc = Nothing
                        oRoot = Nothing
                        oNode = Nothing
                    End If

                    .Request = Request.Trim()

                    Select Case ServiceID
                        Case ttServices.AirAvail
                            strResponse = .AirAvail()
                        Case ttServices.LowFare

                            If EKBP And ttProviderSystems.UserID = "FlightSite" And (.Request.Contains("<VendorPref Code=""EK""") Or .Request.Contains("<VendorPref Code=""BP""")) Then
                                Console.Write(.Request)
                                .Request = .Request.Replace("<VendorPref Code=""G3"" PreferLevel=""Unacceptable"" />", String.Empty)
                                Console.Write(.Request)
                                .Request = .Request.Replace("<VendorPref Code=""FL"" PreferLevel=""Unacceptable"" />", String.Empty)
                                Console.Write(.Request)
                                .Request = .Request.Replace("<VendorPref Code=""U2"" PreferLevel=""Unacceptable"" />", String.Empty)
                                Console.Write(.Request)
                            ElseIf Not EKBP And ttProviderSystems.UserID = "FlightSite" And (.Request.Contains("<VendorPref Code=""EK""") Or .Request.Contains("<VendorPref Code=""BP""")) Then
                                .Request = .Request.Replace("<VendorPref Code=""BP"" PreferLevel=""Only"" />", "<VendorPref Code=""BP"" PreferLevel=""Unacceptable"" />")
                                Console.Write(.Request)
                                .Request = .Request.Replace("<VendorPref Code=""EK"" PreferLevel=""Only"" />", "<VendorPref Code=""EK"" PreferLevel=""Unacceptable"" />")
                                Console.Write(.Request)
                            End If

                            strResponse = .LowFare()

                            'If EKBP Then
                            'strResponse = strResponse.Replace("OTA_AirLowFareSearchRS", "OTA_AirLowFareSearchPlusRS")
                            'End If
                        Case ttServices.LowFarePlus
                            'oDocReq1 = New XmlDocument
                            'oDocReq1.LoadXml(mstrRequest)
                            'oRootReq1 = oDocReq1.DocumentElement

                            If ttProviderSystems.UserID = "FlightSite" And (.Request.Contains("<VendorPref Code=""EK"" PreferLevel=""Only""") Or .Request.Contains("<VendorPref Code=""BP"" PreferLevel=""Only""")) Then
                                .Request = Request.Replace("OTA_AirLowFareSearchPlusRQ", "OTA_AirLowFareSearchRQ")
                                strResponse = .LowFare()
                                strResponse = strResponse.Replace("OTA_AirLowFareSearchRS", "OTA_AirLowFareSearchPlusRS")
                            Else
                                strResponse = .LowFarePlus()
                            End If

                            'If oRootReq1.SelectNodes("OriginDestinationInformation").Count < 4 Then
                            '    strResponse = .LowFarePlus()
                            'Else
                            '    .Request = mstrRequest.Replace("OTA_AirLowFareSearchPlusRQ", "OTA_AirLowFareSearchRQ")
                            '    strResponse = .LowFare()
                            '    strResponse = strResponse.Replace("OTA_AirLowFareSearchRS", "OTA_AirLowFareSearchPlusRS")
                            'End If
                            'oDocReq1 = Nothing
                            'oRootReq1 = Nothing
                        Case ttServices.LowFareSchedule
                            strResponse = .LowFareSchedule()
                        Case ttServices.AirSchedule
                            strResponse = .AirSchedule()
                        Case ttServices.LowFareMatrix
                            strResponse = .LowFareMatrix()
                        Case ttServices.LowOfferMatrix
                            strResponse = .LowOfferMatrix()
                        Case ttServices.LowOfferSearch
                            strResponse = .LowOfferSearch()
                        Case ttServices.LowFareFlights
                            strResponse = .LowFareFlights()
                        Case Else
                            Throw New Exception("Invalid request or message not supported by Amadeus air services.")
                    End Select


                    ''''''''''''''''''Air line class filtering -  shashin''''''''''''''''''

                    'If dt.Rows.Count > 0 Then
                    '    Select Case mintServiceID
                    '        Case 6, 7
                    '            strResponse = FilterAirLineClasses(strResponse, dt)
                    '        Case 68
                    '            strResponse = FilterAirLineClasses_LFS(strResponse, dt)
                    '    End Select
                    'End If

                    If dt.Select("PreferLevel='1'").Length > 0 Then
                        Select Case ServiceID
                            Case 6, 7
                                strResponse = FilterAirLineClasses(strResponse, dt)
                            Case 68
                                strResponse = FilterAirLineClasses_LFS(strResponse, dt)
                        End Select
                    End If

                    If dt.Select("PreferLevel='0'").Length > 0 Then
                        Select Case ServiceID
                            Case 6, 7
                                strResponse = FilterAirLineClasses_remove(strResponse, dt)
                            Case 68
                                strResponse = FilterAirLineClasses_LFS_remove(strResponse, dt)
                        End Select
                    End If

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    'ttAA = .ttAPIAdapter

                    If ttProviderSystems.BLFile <> "" And ServiceID <> ttServices.LowFareSchedule Then
                        oDoc = New XmlDocument
                        ' Load Access Control List into memory
                        Try
                            oDoc.Load(ttProviderSystems.BLFile)
                        Catch exr As Exception
                            CoreLib.SendTrace("", "cServiceAmadeus", "Error Loading business logic file", exr.Message, String.Empty)
                            Throw exr
                        End Try

                        oRoot = oDoc.DocumentElement
                        ' check "Out" business logic
                        sb.Append("Message[@Name='").Append(strMsg).Append("'][@Direction='Out']")
                        oNode = oRoot.SelectSingleNode(sb.ToString())
                        sb.Remove(0, sb.Length)

                        If Not oNode Is Nothing Then
                            ' check if non ticketable flights/fares to eliminate
                            sb.Append("NoTktAirline[@Name='Amadeus'][@System='").Append(ttProviderSystems.System).Append("'][@PCC='").Append(ttProviderSystems.PCC.ToUpper()).Append("']")
                            oBLNode = oNode.SelectSingleNode(sb.ToString())
                            sb.Remove(0, sb.Length)

                            If Not oBLNode Is Nothing Then
                                sb.Append(XslPath).Append("BL\")
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, sb.ToString(), strMsg, "NoTkt")
                                sb.Remove(0, sb.Length)
                            End If

                            ' check if no mix airline to eliminate
                            sb.Append("NoMixAirline[@Name='Amadeus'][@System='").Append(ttProviderSystems.System).Append("'][@PCC='").Append(ttProviderSystems.PCC.ToUpper()).Append("']")
                            oBLNode = oNode.SelectSingleNode(sb.ToString())
                            sb.Remove(0, sb.Length)

                            If Not oBLNode Is Nothing Then
                                sb.Append(XslPath).Append("BL\")
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, sb.ToString(), strMsg, "NoMix")
                                sb.Remove(0, sb.Length)
                            End If

                            ' check if No Fare Type to eliminate
                            sb.Append("NoFareType[@Name='Amadeus'][@System='").Append(ttProviderSystems.System).Append("'][@PCC='").Append(ttProviderSystems.PCC.ToUpper()).Append("']")
                            oBLNode = oNode.SelectSingleNode(sb.ToString())
                            sb.Remove(0, sb.Length)

                            If Not oBLNode Is Nothing Then
                                sb.Append(XslPath).Append("BL\")
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, sb.ToString(), strMsg, "NoFareType")
                                sb.Remove(0, sb.Length)
                            End If

                            '' add fare markup if needed
                            sb.Append("ProviderBL[@Name='Amadeus'][@System='").Append(ttProviderSystems.System).Append("'][@PCC='").Append(ttProviderSystems.PCC).Append("']")
                            oBLNode = oNode.SelectSingleNode(sb.ToString())
                            sb.Remove(0, sb.Length)

                            If Not oBLNode Is Nothing Then
                                sb.Append(XslPath).Append("BL\")
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, sb.ToString(), strMsg, "")
                                sb.Remove(0, sb.Length)
                            End If
                        End If
                    End If
                End With

            Catch ex As Exception
                sb.Append("AmadeusWS").Append("-").Append(ttProviderSystems.PCC)
                strResponse = FormatErrorMessage(ServiceID, ex.Message, sb.ToString(), "", False, Version)
                sb.Remove(0, sb.Length)
            Finally
                If Not ttService Is Nothing Then ttService = Nothing
                oDocReq = Nothing
                oRootReq = Nothing
                RaiseEvent GotResponse(strResponse)
            End Try

        End Sub

        Public Sub SendCarRequest()
            Dim ttService As CarServices = Nothing
            Dim strResponse As String = ""

            Try
                ttService = New CarServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .Request = Request
                    .ttProviderSystems = ttProviderSystems

                    Select Case ServiceID
                        Case ttServices.CarAvail
                            strResponse = .CarAvail()
                        Case Else
                            Throw New Exception("Invalid request or message not supported by Amadeus car services.")
                    End Select

                    'ttAA = .ttAPIAdapter
                End With

            Catch ex As Exception
                strResponse = FormatErrorMessage(ServiceID, ex.Message, "Amadeus", "", False, Version)
            Finally
                If Not ttService Is Nothing Then ttService = Nothing
                RaiseEvent GotResponse(strResponse)
            End Try

        End Sub

        Public Sub SendHotelRequest()
            Dim ttService As HotelServices = Nothing
            Dim strResponse As String = ""

            Try
                ttService = New HotelServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .Request = Request
                    .ttProviderSystems = ttProviderSystems

                    Select Case ServiceID
                        Case ttServices.HotelAvail
                            strResponse = .HotelAvail()
                        Case ttServices.HotelSearch
                            strResponse = .HotelSearch
                        Case Else
                            Throw New Exception("Invalid request or message not supported by Amadeus hotel services.")
                    End Select

                    'ttAA = .ttAPIAdapter
                End With

            Catch ex As Exception
                strResponse = FormatErrorMessage(ServiceID, ex.Message, "Amadeus", "", False, Version)
            Finally
                If Not ttService Is Nothing Then ttService = Nothing
                RaiseEvent GotResponse(strResponse)
            End Try

        End Sub

        ''' <param name="strResponse">Response after converting to OTA format</param>
        ''' <param name="dtClasses">DataTable of airlines and classes</param>
        ''' <returns>Filtered response for given classes</returns>
        ''' <remarks>By Shashin - 31/03/2010</remarks>
        Private Function FilterAirLineClasses_remove(ByVal strResponse As String, ByVal dtClasses As DataTable) As String
            If strResponse.IndexOf("Success") <> -1 Then
                Dim strResult As String
                Dim strXML As String
                'Dim inStart As Integer = strResponse.IndexOf("<soap:Body>") + 11
                'Dim iLength As Integer = strResponse.IndexOf("</soap:Body>") - (strResponse.IndexOf("<soap:Body>") + 11)
                '                strXML = "<wmLowFarePlusResponse xmlns=""http://tripxml.downtowntravel.com/wsLowFarePlus"">" + strResponse '.Substring(inStart, iLength)
                'strXML = "<wmLowFarePlusResponse xmlns=""tripxml.dowtowntravel.com/tripxml/wsLowFarePlus"">" + strResponse + "</wmLowFarePlusResponse>"
                strXML = strResponse

                Dim oDoc As XmlDocument = New XmlDocument()
                oDoc.LoadXml(strXML)
                Dim oRoot As XmlElement
                oRoot = oDoc.DocumentElement
                Dim oNode As XmlNode
                Dim oInnerNode As XmlNode
                Dim cnt As Integer
                Dim seqCounter As Integer
                cnt = oRoot.ChildNodes(1).ChildNodes.Count
                Dim sb As StringBuilder = New StringBuilder()
                Dim strHeader As String = strXML.Substring(0, strXML.IndexOf("<PricedItineraries>") + 19)
                'sb = sb.Append("<?xml version=""1.0""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><soap:Body><wmLowFarePlusResponse xmlns=""tripxml.dowtowntravel.com/tripxml/wsLowFarePlus""><OTA_AirLowFareSearchPlusRS Version=""1.001"" TransactionIdentifier=""Amadeus-MIA1S21AV""><Success/><PricedItineraries>")

                sb = sb.Append(strHeader)
                Dim strFooter As String = strXML.Substring(strXML.IndexOf("</PricedItineraries>"), strXML.Length - strXML.IndexOf("</PricedItineraries>"))

                For i As Integer = 0 To cnt
                    oNode = oRoot.ChildNodes(1).ChildNodes(i)
                    Dim flag As Boolean = False

                    If Not oNode Is Nothing Then
                        Dim icnt As Integer = oNode.ChildNodes(0).ChildNodes(0).ChildNodes.Count

                        For j As Integer = 0 To icnt
                            oInnerNode = oNode.ChildNodes(0).ChildNodes(0).ChildNodes(j)

                            If Not oInnerNode Is Nothing Then
                                Dim strClass As String = oInnerNode.ChildNodes(0).Attributes("ResBookDesigCode").Value
                                Dim strAL As String = oInnerNode.ChildNodes(0).ChildNodes(4).Attributes("Code").Value

                                If dtClasses.Select("Airline='" + strAL + "' and Class='" + strClass + "' and PreferLevel='0'").Length > 0 Then
                                    flag = False
                                    j = icnt + 1
                                Else
                                    flag = True

                                End If
                            End If
                        Next
                        If flag Then
                            seqCounter = seqCounter + 1
                            oNode.Attributes("SequenceNumber").Value = seqCounter.ToString
                            sb = sb.Append(oNode.OuterXml)
                        End If
                    End If
                Next
                'sb = sb.Append("</PricedItineraries></OTA_AirLowFareSearchPlusRS>") '</wmLowFarePlusResponse></soap:Body></soap:Envelope>")

                If seqCounter = 0 Then
                    Throw New Exception("NO ITINERARY FOUND FOR REQUESTED CLASSES OF SERVICE")
                End If

                sb = sb.Append(strFooter)
                strResult = sb.ToString
                sb = sb.Remove(0, sb.Length)
                Return strResult
            Else
                Return strResponse
            End If
        End Function

        ''' <param name="strResponse">Response after converting to OTA format</param>
        ''' <param name="dtClasses">DataTable of airlines and classes</param>
        ''' <returns>Filtered response for given classes</returns>
        ''' <remarks>By Shashin - 31/03/2010</remarks>
        Private Function FilterAirLineClasses(ByVal strResponse As String, ByVal dtClasses As DataTable) As String
            If strResponse.IndexOf("Success") <> -1 Then
                Dim strResult As String
                Dim strXML As String
                'Dim inStart As Integer = strResponse.IndexOf("<soap:Body>") + 11
                'Dim iLength As Integer = strResponse.IndexOf("</soap:Body>") - (strResponse.IndexOf("<soap:Body>") + 11)
                '                strXML = "<wmLowFarePlusResponse xmlns=""tripxml.dowtowntravel.com/tripxml/wsLowFarePlus"">" + strResponse '.Substring(inStart, iLength)
                'strXML = "<wmLowFarePlusResponse xmlns=""tripxml.dowtowntravel.com/tripxml/wsLowFarePlus"">" + strResponse + "</wmLowFarePlusResponse>"
                strXML = strResponse

                Dim oDoc As XmlDocument = New XmlDocument()
                oDoc.LoadXml(strXML)
                Dim oRoot As XmlElement
                oRoot = oDoc.DocumentElement
                Dim oNode As XmlNode
                Dim oInnerNode As XmlNode
                Dim cnt As Integer
                Dim seqCounter As Integer
                cnt = oRoot.ChildNodes(1).ChildNodes.Count
                Dim sb As StringBuilder = New StringBuilder()
                Dim strHeader As String = strXML.Substring(0, strXML.IndexOf("<PricedItineraries>") + 19)
                'sb = sb.Append("<?xml version=""1.0""?><soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><soap:Body><wmLowFarePlusResponse xmlns=""tripxml.dowtowntravel.com/tripxml/wsLowFarePlus""><OTA_AirLowFareSearchPlusRS Version=""1.001"" TransactionIdentifier=""Amadeus-MIA1S21AV""><Success/><PricedItineraries>")

                sb = sb.Append(strHeader)
                Dim strFooter As String = strXML.Substring(strXML.IndexOf("</PricedItineraries>"), strXML.Length - strXML.IndexOf("</PricedItineraries>"))

                For i As Integer = 0 To cnt
                    oNode = oRoot.ChildNodes(1).ChildNodes(i)
                    Dim flag As Boolean = False

                    If Not oNode Is Nothing Then
                        Dim icnt As Integer = oNode.ChildNodes(0).ChildNodes(0).ChildNodes.Count

                        For j As Integer = 0 To icnt
                            oInnerNode = oNode.ChildNodes(0).ChildNodes(0).ChildNodes(j)

                            If Not oInnerNode Is Nothing Then
                                Dim strClass As String = oInnerNode.ChildNodes(0).Attributes("ResBookDesigCode").Value
                                Dim strAL As String = oInnerNode.ChildNodes(0).ChildNodes(4).Attributes("Code").Value

                                If dtClasses.Select("Airline='" + strAL + "' and Class='" + strClass + "' and PreferLevel='1'").Length > 0 Then
                                    flag = True
                                Else
                                    flag = False
                                    j = icnt + 1
                                End If
                            End If
                        Next
                        If flag Then
                            seqCounter = seqCounter + 1
                            oNode.Attributes("SequenceNumber").Value = seqCounter.ToString
                            sb = sb.Append(oNode.OuterXml)
                        End If
                    End If
                Next
                'sb = sb.Append("</PricedItineraries></OTA_AirLowFareSearchPlusRS>") '</wmLowFarePlusResponse></soap:Body></soap:Envelope>")

                If seqCounter = 0 Then
                    Throw New Exception("NO ITINERARY FOUND WITH REQUESTED CLASSES OF SERVICE")
                End If

                sb = sb.Append(strFooter)
                strResult = sb.ToString
                sb = sb.Remove(0, sb.Length)
                Return strResult
            Else
                Return strResponse
            End If
        End Function

        ''' <summary> 
        ''' Method to filter response for given classes for LowFareSchedule
        ''' </summary>
        ''' <param name="strResponse">Response after converting to OTA format</param>
        ''' <param name="dtClasses">DataTable of airlines and classes</param>
        ''' <returns>Filtered response for given classes</returns>
        ''' <remarks>By Shashin - 05/04/2010</remarks>
        Private Function FilterAirLineClasses_LFS_remove(ByVal strResponse As String, ByVal dtClasses As DataTable) As String
            Dim strResult As String
            If strResponse.IndexOf("Success") <> -1 Then
                Dim oDoc As XmlDocument = New XmlDocument()
                oDoc.LoadXml(strResponse)
                Dim oRoot As XmlElement
                oRoot = oDoc.DocumentElement
                Dim oNode As XmlNode
                Dim oInnerNode As XmlNode
                Dim cnt As Integer = oRoot.ChildNodes(1).ChildNodes.Count
                Dim seqCounter As Integer = 1
                Dim sbResult As StringBuilder = New StringBuilder()
                Dim sbFinal As StringBuilder = New StringBuilder()
                Dim dtReturns As DataTable = New DataTable("Results")
                dtReturns.Columns.Add("Index", Type.GetType("System.Int32"))
                dtReturns.Columns.Add("RPH", Type.GetType("System.Int32"))
                dtReturns.Columns.Add("Flag")
                Dim strAL As String = ""
                Dim strClass As String = ""
                Dim dtFinal As DataTable = New DataTable()
                dtFinal.Columns.Add("Index")
                dtFinal.Columns.Add("RPH")
                Dim dc(2) As DataColumn
                dc(0) = dtFinal.Columns("Index")
                dc(1) = dtFinal.Columns("RPH")
                dtFinal.PrimaryKey = dc
                Dim dtIndex As DataTable = New DataTable()
                dtIndex.Columns.Add("Index")
                Dim dc1(1) As DataColumn
                dc1(0) = dtIndex.Columns("Index")
                dtIndex.PrimaryKey = dc1
                Dim strHeader As String = strResponse.Substring(0, strResponse.IndexOf("<PricedItineraries>") + 19)
                'sbFinal = sbFinal.Append("<OTA_AirLowFareSearchPlusRS Version=""1.001"" TransactionIdentifier=""Amadeus-MIA1S21AV\""><Success/><PricedItineraries>")
                sbFinal = sbFinal.Append(strHeader)
                Dim strFooter As String = strResponse.Substring(strResponse.IndexOf("</PricedItineraries>"), strResponse.Length - strResponse.IndexOf("</PricedItineraries>"))
                For i As Integer = 0 To cnt - 1
                    oNode = oRoot.ChildNodes(1).ChildNodes(i)
                    Dim innnercnt1 As Integer = oNode.ChildNodes(0).ChildNodes(0).ChildNodes(0).ChildNodes.Count
                    Dim innnercnt2 As Integer = oNode.ChildNodes(0).ChildNodes(0).ChildNodes(1).ChildNodes.Count

                    For j As Integer = 0 To innnercnt2
                        oInnerNode = oNode.ChildNodes(0).ChildNodes(0).ChildNodes(1).ChildNodes(j)

                        If Not oInnerNode Is Nothing Then
                            strAL = oInnerNode.ChildNodes(4).Attributes("Code").Value
                            strClass = oInnerNode.Attributes("ResBookDesigCode").Value

                            For k As Integer = 0 To innnercnt1 - 1
                                Dim dr As DataRow = dtReturns.NewRow()
                                'dr("Index") = Integer.Parse(oInnerNode.ChildNodes(5).ChildNodes(4 + k).Attributes("Index").Value)

                                dr("Index") = Integer.Parse(oInnerNode.ChildNodes(5).SelectSingleNode("OriginClass").Attributes("Index").Value)
                                dr("RPH") = Integer.Parse(oInnerNode.Attributes("RPH").Value)

                                If dtClasses.Select("Airline='" + strAL + "' and Class='" + strClass + "' and PreferLevel='0'").Length > 0 Then
                                    'strClass = oInnerNode.ChildNodes(5).ChildNodes(4 + k).InnerText
                                    dr("Flag") = "F"
                                Else

                                    strClass = oInnerNode.ChildNodes(5).SelectSingleNode("OriginClass").InnerText
                                    strAL = oNode.ChildNodes(0).ChildNodes(0).ChildNodes(0).ChildNodes(k).ChildNodes(2).Attributes("Code").Value
                                    If dtClasses.Select("Airline='" + strAL + "' and Class='" + strClass + "' and PreferLevel='0'").Length > 0 Then
                                        dr("Flag") = "F"
                                    Else
                                        dr("Flag") = "T"
                                    End If
                                End If
                                dtReturns.Rows.Add(dr)
                            Next
                        End If

                    Next
                    Dim drArray() As DataRow = dtReturns.Select("Flag='T'")

                    If drArray.Length > 0 Then
                        Dim dr As DataRow
                        For Each dr In drArray
                            Dim strIn As String = dr("Index").ToString()
                            Dim strRPH As String = dr("RPH").ToString()

                            If dtReturns.Select("RPH=" + strRPH + " and Flag='F'").Length = 0 Then
                                Dim drTemp As DataRow = dtFinal.NewRow()
                                drTemp("RPH") = strRPH
                                drTemp("Index") = strIn
                                Try
                                    dtFinal.Rows.Add(drTemp)
                                Catch ex As Exception

                                End Try
                                Dim drTemp2 As DataRow = dtIndex.NewRow()
                                drTemp2("Index") = strIn
                                Try
                                    dtIndex.Rows.Add(drTemp2)
                                Catch ex As Exception

                                End Try
                            End If

                        Next dr

                        If dtIndex.Rows.Count > 0 Then
                            sbResult = sbResult.Append("<PricedItinerary SequenceNumber=""" + seqCounter.ToString() + """><AirItinerary><OriginDestinationOptions><OriginDestinationOption>")
                            Dim dr1 As DataRow
                            For Each dr1 In dtIndex.Rows
                                sbResult = sbResult.Append(oNode.ChildNodes(0).ChildNodes(0).ChildNodes(0).ChildNodes(Integer.Parse(dr1("Index").ToString()) - 1).OuterXml)
                            Next dr1
                            sbResult = sbResult.Append("</OriginDestinationOption><OriginDestinationOption>")

                            For j As Integer = 0 To innnercnt2
                                oInnerNode = oNode.ChildNodes(0).ChildNodes(0).ChildNodes(1).ChildNodes(j)

                                If Not oInnerNode Is Nothing Then
                                    strAL = oInnerNode.ChildNodes(2).Attributes("Code").Value
                                    strClass = oInnerNode.Attributes("ResBookDesigCode").Value
                                    Dim strIN As String = oInnerNode.ChildNodes(5).SelectSingleNode("OriginClass").Attributes("Index").Value
                                    Dim strRPH As String = oInnerNode.Attributes("RPH").Value

                                    If dtFinal.Select("Index='" + strIN + "' and RPH='" + strRPH + "'").Length > 0 Then
                                        sbResult = sbResult.Append(oInnerNode.OuterXml)
                                    End If
                                End If
                            Next
                            sbResult = sbResult.Append("</OriginDestinationOption></OriginDestinationOptions></AirItinerary>")
                            sbResult = sbResult.Append(oNode.ChildNodes(1).OuterXml)
                            sbResult = sbResult.Append("</PricedItinerary>")
                            seqCounter = seqCounter + 1
                        End If
                    End If
                    dtReturns.Rows.Clear()
                    dtFinal.Rows.Clear()
                    dtIndex.Clear()
                    sbFinal = sbFinal.Append(sbResult.ToString())
                    sbResult = sbResult.Remove(0, sbResult.Length)

                Next

                'sbFinal = sbFinal.Append("</PricedItineraries></OTA_AirLowFareSearchPlusRS>")

                If seqCounter = 1 Then
                    Throw New Exception("NO ITINERARY FOUND FOR REQUESTED CLASSES OF SERVICE")
                End If

                sbFinal = sbFinal.Append(strFooter)
                strResult = sbFinal.ToString()
                sbFinal.Remove(0, sbFinal.Length)
                Return strResult
            Else
                Return strResponse
            End If
        End Function

        ''' <summary> 
        ''' Method to filter response for given classes for LowFareSchedule
        ''' </summary>
        ''' <param name="strResponse">Response after converting to OTA format</param>
        ''' <param name="dtClasses">DataTable of airlines and classes</param>
        ''' <returns>Filtered response for given classes</returns>
        ''' <remarks>By Shashin - 05/04/2010</remarks>
        Private Function FilterAirLineClasses_LFS(ByVal strResponse As String, ByVal dtClasses As DataTable) As String
            Dim strResult As String
            If strResponse.IndexOf("Success") <> -1 Then
                Dim oDoc As XmlDocument = New XmlDocument()
                oDoc.LoadXml(strResponse)
                Dim oRoot As XmlElement
                oRoot = oDoc.DocumentElement
                Dim oNode As XmlNode
                Dim oInnerNode As XmlNode
                Dim cnt As Integer = oRoot.ChildNodes(1).ChildNodes.Count
                Dim seqCounter As Integer = 1
                Dim sbResult As StringBuilder = New StringBuilder()
                Dim sbFinal As StringBuilder = New StringBuilder()
                Dim dtReturns As DataTable = New DataTable("Results")
                dtReturns.Columns.Add("Index", Type.GetType("System.Int32"))
                dtReturns.Columns.Add("RPH", Type.GetType("System.Int32"))
                dtReturns.Columns.Add("Flag")
                Dim strAL As String = ""
                Dim strClass As String = ""
                Dim dtFinal As DataTable = New DataTable()
                dtFinal.Columns.Add("Index")
                dtFinal.Columns.Add("RPH")
                Dim dc(2) As DataColumn
                dc(0) = dtFinal.Columns("Index")
                dc(1) = dtFinal.Columns("RPH")
                dtFinal.PrimaryKey = dc
                Dim dtIndex As DataTable = New DataTable()
                dtIndex.Columns.Add("Index")
                Dim dc1(1) As DataColumn
                dc1(0) = dtIndex.Columns("Index")
                dtIndex.PrimaryKey = dc1
                Dim strHeader As String = strResponse.Substring(0, strResponse.IndexOf("<PricedItineraries>") + 19)
                'sbFinal = sbFinal.Append("<OTA_AirLowFareSearchPlusRS Version=""1.001"" TransactionIdentifier=""Amadeus-MIA1S21AV\""><Success/><PricedItineraries>")
                sbFinal = sbFinal.Append(strHeader)
                Dim strFooter As String = strResponse.Substring(strResponse.IndexOf("</PricedItineraries>"), strResponse.Length - strResponse.IndexOf("</PricedItineraries>"))
                For i As Integer = 0 To cnt - 1
                    oNode = oRoot.ChildNodes(1).ChildNodes(i)
                    Dim innnercnt1 As Integer = oNode.ChildNodes(0).ChildNodes(0).ChildNodes(0).ChildNodes.Count
                    Dim innnercnt2 As Integer = oNode.ChildNodes(0).ChildNodes(0).ChildNodes(1).ChildNodes.Count

                    For j As Integer = 0 To innnercnt2
                        oInnerNode = oNode.ChildNodes(0).ChildNodes(0).ChildNodes(1).ChildNodes(j)

                        If Not oInnerNode Is Nothing Then
                            strAL = oInnerNode.ChildNodes(4).Attributes("Code").Value
                            strClass = oInnerNode.Attributes("ResBookDesigCode").Value

                            For k As Integer = 0 To innnercnt1 - 1
                                Dim dr As DataRow = dtReturns.NewRow()
                                'dr("Index") = Integer.Parse(oInnerNode.ChildNodes(5).ChildNodes(4 + k).Attributes("Index").Value)

                                dr("Index") = Integer.Parse(oInnerNode.ChildNodes(5).SelectSingleNode("OriginClass").Attributes("Index").Value)
                                dr("RPH") = Integer.Parse(oInnerNode.Attributes("RPH").Value)

                                If dtClasses.Select("Airline='" + strAL + "' and Class='" + strClass + "' and PreferLevel='1'").Length > 0 Then
                                    'strClass = oInnerNode.ChildNodes(5).ChildNodes(4 + k).InnerText
                                    strClass = oInnerNode.ChildNodes(5).SelectSingleNode("OriginClass").InnerText
                                    strAL = oNode.ChildNodes(0).ChildNodes(0).ChildNodes(0).ChildNodes(k).ChildNodes(2).Attributes("Code").Value
                                    If dtClasses.Select("Airline='" + strAL + "' and Class='" + strClass + "' and PreferLevel='1'").Length > 0 Then
                                        dr("Flag") = "T"
                                    Else
                                        dr("Flag") = "F"
                                    End If
                                Else
                                    dr("Flag") = "F"
                                End If
                                dtReturns.Rows.Add(dr)
                            Next
                        End If

                    Next
                    Dim drArray() As DataRow = dtReturns.Select("Flag='T'")

                    If drArray.Length > 0 Then
                        Dim dr As DataRow
                        For Each dr In drArray
                            Dim strIn As String = dr("Index").ToString()
                            Dim strRPH As String = dr("RPH").ToString()

                            If dtReturns.Select("RPH=" + strRPH + " and Flag='F'").Length = 0 Then
                                Dim drTemp As DataRow = dtFinal.NewRow()
                                drTemp("RPH") = strRPH
                                drTemp("Index") = strIn
                                Try
                                    dtFinal.Rows.Add(drTemp)
                                Catch ex As Exception

                                End Try
                                Dim drTemp2 As DataRow = dtIndex.NewRow()
                                drTemp2("Index") = strIn
                                Try
                                    dtIndex.Rows.Add(drTemp2)
                                Catch ex As Exception

                                End Try
                            End If

                        Next dr

                        If dtIndex.Rows.Count > 0 Then
                            sbResult = sbResult.Append("<PricedItinerary SequenceNumber=""" + seqCounter.ToString() + """><AirItinerary><OriginDestinationOptions><OriginDestinationOption>")
                            Dim dr1 As DataRow
                            For Each dr1 In dtIndex.Rows
                                sbResult = sbResult.Append(oNode.ChildNodes(0).ChildNodes(0).ChildNodes(0).ChildNodes(Integer.Parse(dr1("Index").ToString()) - 1).OuterXml)
                            Next dr1
                            sbResult = sbResult.Append("</OriginDestinationOption><OriginDestinationOption>")

                            For j As Integer = 0 To innnercnt2
                                oInnerNode = oNode.ChildNodes(0).ChildNodes(0).ChildNodes(1).ChildNodes(j)

                                If Not oInnerNode Is Nothing Then
                                    strAL = oInnerNode.ChildNodes(2).Attributes("Code").Value
                                    strClass = oInnerNode.Attributes("ResBookDesigCode").Value
                                    Dim strIN As String = oInnerNode.ChildNodes(5).SelectSingleNode("OriginClass").Attributes("Index").Value
                                    Dim strRPH As String = oInnerNode.Attributes("RPH").Value

                                    If dtFinal.Select("Index='" + strIN + "' and RPH='" + strRPH + "'").Length > 0 Then
                                        sbResult = sbResult.Append(oInnerNode.OuterXml)
                                    End If
                                End If
                            Next
                            sbResult = sbResult.Append("</OriginDestinationOption></OriginDestinationOptions></AirItinerary>")
                            sbResult = sbResult.Append(oNode.ChildNodes(1).OuterXml)
                            sbResult = sbResult.Append("</PricedItinerary>")
                            seqCounter = seqCounter + 1
                        End If
                    End If
                    dtReturns.Rows.Clear()
                    dtFinal.Rows.Clear()
                    dtIndex.Clear()
                    sbFinal = sbFinal.Append(sbResult.ToString())
                    sbResult = sbResult.Remove(0, sbResult.Length)

                Next

                'sbFinal = sbFinal.Append("</PricedItineraries></OTA_AirLowFareSearchPlusRS>")

                If seqCounter = 1 Then
                    Throw New Exception("NO ITINERARY FOUND FOR REQUESTED CLASSES OF SERVICE")
                End If

                sbFinal = sbFinal.Append(strFooter)
                strResult = sbFinal.ToString()
                sbFinal.Remove(0, sbFinal.Length)
                Return strResult
            Else
                Return strResponse
            End If
        End Function

        Public Function BusinessLogic(ByVal strResponse As String, ByVal strBusiness As String, ByVal xslPath As String, ByVal strMsg As String, ByVal strExt As String) As String

            If strResponse.IndexOf("<Success />") <> -1 Or strResponse.IndexOf("<Success></Success>") Then
                Dim sb As StringBuilder = New StringBuilder()
                sb.Append(strBusiness).Append("<Success />")
                strResponse = strResponse.Replace("<Success />", sb.ToString())
                sb.Remove(0, sb.Length)

                sb.Append(strBusiness).Append("<Success></Success>")
                strResponse = strResponse.Replace("<Success></Success>", sb.ToString())
                sb.Remove(0, sb.Length)

                sb.Append("Before ").Append(strMsg).Append(" business logic")
                CoreLib.SendTrace("", "cServiceAmadeus", sb.ToString(), strResponse, String.Empty)
                sb.Remove(0, sb.Length)

                sb.Append(Version).Append("BL_").Append(strMsg).Append(strExt).Append("RS.xsl")
                strResponse = CoreLib.TransformXML(strResponse, xslPath, sb.ToString())
                sb = Nothing
            End If

            Return strResponse
        End Function

        Public Function BusinessLogicIn(ByVal strRequest As String, ByVal strBusiness As String, ByVal xslPath As String, ByVal strMsg As String) As String
            Dim sb As StringBuilder = New StringBuilder()
            sb.Append(strBusiness).Append("</OTA_AirLowFareSearchRQ>")
            strRequest = strRequest.Replace("</OTA_AirLowFareSearchRQ>", sb.ToString())
            sb.Remove(0, sb.Length)

            sb.Append(strBusiness).Append("</OTA_AirLowFareSearchPlusRQ>")
            strRequest = strRequest.Replace("</OTA_AirLowFareSearchPlusRQ>", sb.ToString())
            sb.Remove(0, sb.Length)

            sb.Append(strBusiness).Append("</OTA_AirLowFareSearchMatrixRQ>")
            strRequest = strRequest.Replace("</OTA_AirLowFareSearchMatrixRQ>", sb.ToString())
            sb.Remove(0, sb.Length)

            sb.Append(strBusiness).Append("</OTA_AirAvailRQ>")
            strRequest = strRequest.Replace("</OTA_AirAvailRQ>", sb.ToString())
            sb.Remove(0, sb.Length)

            sb.Append("Before ").Append(strMsg).Append(" business logic")
            CoreLib.SendTrace("", "cServiceAmadeus", sb.ToString(), strRequest, String.Empty)
            sb.Remove(0, sb.Length)

            sb.Append(Version).Append("BL_").Append(strMsg).Append("RQ.xsl")
            strRequest = CoreLib.TransformXML(strRequest, xslPath, sb.ToString())
            sb = Nothing

            Return strRequest
        End Function

    End Class
End Namespace
