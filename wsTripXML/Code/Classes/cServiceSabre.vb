Imports System
Imports TripXMLMain
Imports System.Xml
Imports System.Data
Imports System.Text
Imports TripXMLMain.modCore

Namespace wsTravelTalk


    Public Class cServiceSabre

        Public Event GotResponse(ByVal Response As String)

#Region " Properties "
        Private sb As StringBuilder = New StringBuilder()

        Private mintServiceID As Integer
        Private mstrRequest As String = ""
        Private mstrVersion As String = ""
        Private ttProviderSystems As TripXMLProviderSystems
        Private mttCities As DataView

        Public Property ServiceID() As Integer
            Get
                Return mintServiceID
            End Get
            Set(ByVal Value As Integer)
                mintServiceID = Value
            End Set
        End Property

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

        Public Property ttCities() As DataView
            Get
                Return mttCities
            End Get
            Set(ByVal Value As DataView)
                mttCities = Value
            End Set
        End Property

#End Region

        Public Sub SendAirRequest()
            Dim strResponse As String = ""
            Dim ttService As Sabre.AirServices = Nothing
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing
            Dim strMsg As String = ""
            Dim oBLNode As XmlNode = Nothing
            Dim strPCC As String = ""

            Dim oDocReq As XmlDocument = Nothing
            Dim oRootReq As XmlElement = Nothing
            Dim oDocReq1 As XmlDocument = Nothing
            Dim oRootReq1 As XmlElement = Nothing
            Dim oNode1 As XmlNode = Nothing
            Dim aClassNode As XmlNode = Nothing

            Dim oAttr As XmlAttribute
            Dim dt As DataTable = Nothing

            Try
                ttService = New Sabre.AirServices

                With ttService
                    .Version = mstrVersion
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = mstrRequest

                    oDocReq = New XmlDocument
                    oDocReq.LoadXml(mstrRequest)
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
                    For Each oNode1 In oRootReq.SelectNodes("TravelPreferences/VendorPref")
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


                    Select Case mintServiceID
                        Case ttServices.AirAvail
                            strResponse = .AirAvail()
                            strMsg = "AirAvail"
                        Case ttServices.LowFare
                            strResponse = .LowFare(mttCities)
                            strMsg = "LowFare"
                        Case ttServices.LowFarePlus
                            strResponse = .LowFarePlus(mttCities)
                            strMsg = "LowFare"
                        Case ttServices.LowFareSchedule
                            strResponse = .LowFareSchedule(mttCities)
                            strMsg = "LowFare"
                        Case Else
                            Throw New Exception("Invalid request or message not supported by Sabre air services.")
                    End Select

                    ''''''''''''''''''Air line class filtering -  shashin''''''''''''''''''

                    If dt.Rows.Count > 0 Then
                        Select Case mintServiceID
                            Case 6, 7
                                strResponse = FilterAirLineClasses(strResponse, dt)
                            Case 68
                                'strResponse = FilterAirLineClasses_LFS(strResponse, dt)
                        End Select
                    End If

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    If ttProviderSystems.AAAPCC.Length > 0 Then
                        strResponse = strResponse.Replace("TransactionIdentifier=""Sabre", sb.Append("TransactionIdentifier=""Sabre").Append("-").Append(ttProviderSystems.AAAPCC).ToString())
                        sb.Remove(0, sb.Length())
                    ElseIf ttProviderSystems.PCC.Length > 0 Then
                        strResponse = strResponse.Replace("TransactionIdentifier=""Sabre", sb.Append("TransactionIdentifier=""Sabre").Append("-").Append(ttProviderSystems.PCC).ToString())
                        sb.Remove(0, sb.Length())
                    End If

                    If .ProviderSystems.BLFile <> "" Then
                        oDoc = New XmlDocument
                        ' Load Access Control List into memory
                        Try
                            oDoc.Load(.ProviderSystems.BLFile)
                        Catch exr As Exception
                            CoreLib.SendTrace("", "cServiceSabre", "Error Loading business logic file", exr.Message, ttProviderSystems.LogUUID)
                            Throw exr
                        End Try

                        oRoot = oDoc.DocumentElement
                        oNode = oRoot.SelectSingleNode(sb.Append("Message[@Name='").Append(strMsg).Append("'][@Direction='Out']").ToString())
                        sb.Remove(0, sb.Length())

                        If Not oNode Is Nothing Then
                            If ttProviderSystems.AAAPCC <> "" Then
                                strPCC = ttProviderSystems.AAAPCC
                            Else
                                strPCC = ttProviderSystems.PCC
                            End If
                            ' check if non ticketable flights/fares to eliminate
                            sb.Append("NoTktAirline[@Name='Sabre'][@System='").Append(ttProviderSystems.System).Append("'][@PCC='").Append(strPCC.ToUpper()).Append("']")
                            oBLNode = oNode.SelectSingleNode(sb.ToString())
                            sb.Remove(0, sb.Length)

                            If Not oBLNode Is Nothing Then
                                sb.Append(XslPath).Append("BL\")
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, sb.ToString(), strMsg, "NoTkt")
                                sb.Remove(0, sb.Length)
                            End If

                            ' check if no mix airline to eliminate
                            sb.Append("NoMixAirline[@Name='Sabre'][@System='").Append(ttProviderSystems.System).Append("'][@PCC='").Append(strPCC.ToUpper()).Append("']")
                            oBLNode = oNode.SelectSingleNode(sb.ToString())
                            sb.Remove(0, sb.Length)

                            If Not oBLNode Is Nothing Then
                                sb.Append(XslPath).Append("BL\")
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, sb.ToString(), strMsg, "NoMix")
                                sb.Remove(0, sb.Length)
                            End If

                            ' check if No Fare Type to eliminate
                            sb.Append("NoFareType[@Name='Sabre'][@System='").Append(ttProviderSystems.System).Append("'][@PCC='").Append(strPCC.ToUpper()).Append("']")
                            oBLNode = oNode.SelectSingleNode(sb.ToString())
                            sb.Remove(0, sb.Length)

                            If Not oBLNode Is Nothing Then
                                sb.Append(XslPath).Append("BL\")
                                strResponse = BusinessLogic(strResponse, oBLNode.OuterXml, sb.ToString(), strMsg, "NoFareType")
                                sb.Remove(0, sb.Length)
                            End If

                            '' add fare markup if needed
                            oNode = oNode.SelectSingleNode(sb.Append("ProviderBL[@Name='Sabre'][@System='").Append(.ProviderSystems.System).Append("'][@PCC='").Append(strPCC.ToUpper).Append("']").ToString())
                            sb.Remove(0, sb.Length())

                            If Not oNode Is Nothing Then
                                strResponse = BusinessLogic(strResponse, oNode.OuterXml, sb.Append(XslPath).Append("BL\").ToString(), strMsg, "")
                                sb.Remove(0, sb.Length())
                            End If
                        End If
                    End If
                End With

            Catch ex As Exception
                strResponse = FormatErrorMessage(mintServiceID, ex.Message, "Sabre", "", False, mstrVersion)
            Finally
                If Not ttService Is Nothing Then ttService = Nothing
                RaiseEvent GotResponse(strResponse)
            End Try
            sb = Nothing
        End Sub

        Public Sub SendCarRequest()
            Dim strResponse As String = ""
            Dim ttService As Sabre.CarServices = Nothing

            Try
                ttService = New Sabre.CarServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = mstrRequest

                    Select Case mintServiceID
                        Case ttServices.CarAvail
                            strResponse = .CarAvail()
                        Case Else
                            Throw New Exception("Invalid request or message not supported by Sabre car services.")
                    End Select

                End With

            Catch ex As Exception
                strResponse = FormatErrorMessage(mintServiceID, ex.Message, "Sabre", "", False, mstrVersion)
            Finally
                If Not ttService Is Nothing Then ttService = Nothing
                RaiseEvent GotResponse(strResponse)
            End Try

        End Sub

        Public Sub SendHotelRequest()
            Dim strResponse As String = ""
            Dim ttService As Sabre.HotelServices = Nothing

            Try
                ttService = New Sabre.HotelServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = mstrRequest

                    Select Case mintServiceID
                        Case ttServices.HotelAvail
                            strResponse = .HotelAvail
                        Case ttServices.HotelSearch
                            strResponse = .HotelSearch
                        Case Else
                            Throw New Exception("Invalid request or message not supported by Sabre hotel services.")
                    End Select

                End With

            Catch ex As Exception
                strResponse = FormatErrorMessage(mintServiceID, ex.Message, "Sabre", "", False, mstrVersion)
            Finally
                If Not ttService Is Nothing Then ttService = Nothing
                RaiseEvent GotResponse(strResponse)
            End Try

        End Sub


        Private Function FilterAirLineClasses_remove(ByVal strResponse As String, ByVal dtClasses As DataTable) As String
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
                    Throw New Exception("NO ITINERARY FOUND FOR REQUESTED SEGMENT 1")
                End If

                sb = sb.Append(strFooter)
                strResult = sb.ToString
                sb = sb.Remove(0, sb.Length)
                Return strResult
            Else
                Return strResponse
            End If
        End Function


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

                                If dtClasses.Select("Airline='" + strAL + "' and Class='" + strClass + "'").Length > 0 Then
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
                    Throw New Exception("NO ITINERARY FOUND FOR REQUESTED SEGMENT 1")
                End If

                sb = sb.Append(strFooter)
                strResult = sb.ToString
                sb = sb.Remove(0, sb.Length)
                Return strResult
            Else
                Return strResponse
            End If
        End Function


        Public Function BusinessLogic(ByVal strResponse As String, ByVal strBusiness As String, ByVal xslPath As String, ByVal strMsg As String, ByVal strExt As String) As String
            Dim sb1 As StringBuilder = New StringBuilder()
            If strResponse.IndexOf("<Success />") <> -1 Or strResponse.IndexOf("<Success></Success>") Then
                strResponse = strResponse.Replace("<Success />", sb1.Append(strBusiness).Append("<Success />").ToString())
                sb1.Remove(0, sb1.Length())
                strResponse = strResponse.Replace("<Success></Success>", sb1.Append(strBusiness).Append("<Success></Success>").ToString())
                sb1.Remove(0, sb1.Length())
                CoreLib.SendTrace("", "cServiceSabre", sb1.Append("Before ").Append(strMsg).Append(" business logic").ToString(), strResponse, ttProviderSystems.LogUUID)
                sb1.Remove(0, sb1.Length())
                strResponse = CoreLib.TransformXML(strResponse, xslPath, sb1.Append(mstrVersion).Append("BL_").Append(strMsg).Append(strExt).Append("RS.xsl").ToString())
                sb1.Remove(0, sb1.Length())
            End If
            sb1 = Nothing
            Return strResponse
        End Function

    End Class
End Namespace
