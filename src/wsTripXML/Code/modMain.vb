Imports System.Xml
Imports System.Linq
Imports TripXMLMain
Imports System.Web.Configuration
Imports System.Threading
Imports System.Net
Imports TripXMLMain.modCore
Imports PaymentServices
Imports System.Text
Imports System.Web
Imports System.Data

Namespace wsTravelTalk

    Public Module modMain

        Private Structure EncodingTables
            Dim TableName As String
            Dim FileName As String
            Dim PlainText As Boolean
        End Structure

        Public Enum enLogType
            Request = 1
            Response = 2
        End Enum

        Public Const CVoyageID As String = "00000"
        Public Const CPrdTimeOut As Integer = 80

#Region " Message Request "

        Public Sub LogResponse(ByRef strResponse As String, ByRef ttCredential As TravelTalkCredential,
                               ByVal StartTime As Date, ByVal ttServiceID As Integer, ByVal ServerName As String, ByRef UUID As String)

            Dim strResp As String

            'If ttServiceID = 6 And strResponse.IndexOf("<Success") <> -1 Then
            '    strResp = "<OTA_AirLowFareSearchRS><Success/></OTA_AirLowFareSearchRS>"
            'ElseIf ttServiceID = 7 And strResponse.IndexOf("<Success") <> -1 Then
            '    strResp = "<OTA_AirLowFareSearchPlusRS><Success/></OTA_AirLowFareSearchPlusRS>"
            'ElseIf ttServiceID = 68 And strResponse.IndexOf("<Success") <> -1 Then
            '    strResp = "<OTA_AirLowFareSearchScheduleRS><Success/></OTA_AirLowFareSearchScheduleRS>"
            'Else
            '    strResp = strResponse
            'End If

            strResp = strResponse

            Dim oLog As cLog
            'Dim startCounter As Date
            Dim sb As StringBuilder = New StringBuilder()

            Try
                If ttServiceID <> 2 And ttServiceID <> 6 And ttServiceID <> 7 And ttServiceID <> 24 And ttServiceID <> 25 And ttServiceID <> 81 And ttServiceID <> 85 Then
                    'startCounter = Now
                    oLog = New cLog
                    With ttCredential
                        oLog.LogResponse(UUID, ServerName, .RequestorID, .UserID, sb.Append(.Providers(0).Name).Append(" ").Append(.System).ToString(), ttServiceID, strResp, StartTime)
                        sb.Remove(0, sb.Length())
                    End With
                End If

            Catch ex As Exception
                ' Just Ignore Log Class will Log Error to Log File
            Finally
                GC.Collect()
            End Try

        End Sub

        Public Sub PreServiceRequest(ByRef strRequest As String, ByRef oApp As HttpApplicationState, ByRef ttCredential As TravelTalkCredential,
                                    ByRef ttProviderSystems As TripXMLProviderSystems, ByVal StartTime As Date,
                                    ByVal ttServiceID As Integer, ByVal ServerName As String, ByRef UUID As String, Optional ByVal Version As String = "",
                                    Optional ByVal isDefault As Boolean = False)
            Dim oDoc As XmlDocument
            Dim validateXSDIn As Boolean
            Dim oLog As cLog
            Dim logged As Boolean = False

            Try
                If isDefault Then
                    ttCredential = GetTravelTalkDefaultCredential(strRequest, ttServiceID)
                Else
                    ttCredential = GetTravelTalkCredential(strRequest, ttServiceID)
                End If

                oDoc = oApp.Get("ttACL")
                'validateXSDIn = oApp.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("In").ToString())
                validateXSDIn = oApp.Get($"XSD{ttCredential.UserID}In")

                ' SQL Message Log
                Try
                    If ttServiceID <> 81 Then
                        oLog = New cLog
                        With ttCredential
                            'sb.Append(.Providers(0).Name).Append(" ").Append(.System).ToString()
                            UUID = oLog.LogRequest(ServerName, .RequestorID, .UserID, $"{ .Providers(0).Name } { .System }", ttServiceID, strRequest, StartTime)
                        End With
                    End If
                    logged = True

                Catch e As Exception
                    ' Just Ignore Log Class will Log Error to Log File
                Finally
                    GC.Collect()
                End Try

                If Trace Then CoreLib.SendTrace(ttCredential.UserID, $"ttMain {ttServiceID}", "============= OTA Request ============= ", strRequest, UUID)

                Try
                    If validateXSDIn Then
                        CoreLib.ValidateXML(strRequest, ttServiceID, enSchemaType.Request, ttCredential.UserID, Version)
                    End If
                Catch exx As Exception
                    Throw New Exception($"Invalid Request. Schema Validation Failed.{vbNewLine}{exx.Message}")
                End Try

                AuthenticateUser(oDoc, ttCredential)
                'ttProviderSystems = oApp.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())

                ttProviderSystems = oApp.Get($"PS{ttCredential.Providers(0).Name}{ttCredential.UserID}{ttCredential.System}{ttCredential.Providers.First().PCC}")

                ttProviderSystems.LogUUID = UUID

                If ttProviderSystems.AmadeusWS = True Then
                    ttCredential.Providers(0).Name = "AmadeusWS"

                    If ttCredential.System = "Test" Then
                        ttProviderSystems.URL = "https://test.webservices.amadeus.com"
                    ElseIf ttCredential.System = "Training" Then
                        ttProviderSystems.URL = "https://production.webservices.amadeus.com"
                    Else
                        ttProviderSystems.URL = "https://production.webservices.amadeus.com"
                    End If
                End If

                If ttCredential.Providers(0).Name <> "Amadeus" Then
                    'ttProviderSystems = oApp.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                    'sb.Remove(0, sb.Length())
                    If ttProviderSystems.System Is Nothing Then
                        '$"Access denied to {ttCredential.Providers(0).Name} - {ttCredential.System} system. Or invalid provider or PCC ({ttCredential.Providers(0).PCC})."
                        Throw New Exception($"Access denied to {ttCredential.Providers(0).Name} - {ttCredential.System} system. Or invalid provider or PCC ({ttCredential.Providers(0).PCC}).")
                    End If
                    If (ttCredential.Providers(0).PCC.Trim.Length > 0 And ttCredential.Providers(0).Name <> "Sabre") Then
                        ttProviderSystems.PCC = ttCredential.Providers(0).PCC
                    End If
                End If

            Catch ex As Exception
                If Not logged Then
                    If Trace Then CoreLib.SendTrace(ttCredential.UserID, $"ttMain {ttServiceID}", "============= OTA Request ============= ", strRequest, ttProviderSystems.LogUUID)

                    If Not UUID Is Nothing Then
                        With ttCredential
                            LogMessageToFile(enLogType.Request, UUID, ServerName, .RequestorID, .UserID, $"{ .Providers(0).Name} { .System}", ttServiceID, strRequest, StartTime, 0, ex.Message)
                        End With
                    End If
                End If
                Throw
            Finally
                GC.Collect()
            End Try
        End Sub

        Public Sub PreServiceRequestPool(ByRef strRequest As String, ByRef oApp As HttpApplicationState, ByRef ttCredential As TravelTalkCredential,
                                    ByVal StartTime As Date, ByVal ttServiceID As Integer, ByVal ServerName As String, ByRef UUID As String, Optional ByVal Version As String = "")
            Dim oDoc As XmlDocument
            Dim validateXSDIn As Boolean
            Dim oLog As cLog
            Dim logged As Boolean = False
            Dim sb As StringBuilder = New StringBuilder()

            Try
                ttCredential = GetTravelTalkCredential(strRequest, ttServiceID)

                oDoc = oApp.Get("ttACL")
                If oDoc Is Nothing Then
                    Throw New Exception("Failed to find ttACL")
                End If

                validateXSDIn = oApp.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("In").ToString())
                sb.Remove(0, sb.Length())

                ' SQL Message Log
                Try

                    oLog = New cLog
                    With ttCredential
                        If ttServiceID <> 2 And ttServiceID <> 6 And ttServiceID <> 7 And ttServiceID <> 24 And ttServiceID <> 25 And ttServiceID <> 81 And ttServiceID <> 85 Then
                            UUID = oLog.LogRequest(ServerName, .RequestorID, .UserID, sb.Append(.Providers(0).Name).Append(" ").Append(.System).ToString(), ttServiceID, strRequest, StartTime)
                            sb.Remove(0, sb.Length())
                        Else
                            ' request set to empty to to send data over network for nothing 
                            UUID = oLog.LogRequest(ServerName, .RequestorID, .UserID, sb.Append(.Providers(0).Name).Append(" ").Append(.System).ToString(), ttServiceID, "", StartTime)
                            sb.Remove(0, sb.Length())
                        End If
                    End With


                    logged = True

                Catch e As Exception
                    ' Just Ignore Log Class will Log Error to Log File
                Finally
                    GC.Collect()
                End Try

                If Trace Then CoreLib.SendTrace(ttCredential.UserID, sb.Append("ttMain ").Append(ttServiceID).ToString(), "============= OTA Request ============= ", strRequest, UUID)
                sb.Remove(0, sb.Length())

                Try
                    If validateXSDIn Then
                        CoreLib.ValidateXML(strRequest, ttServiceID, enSchemaType.Request, ttCredential.UserID, Version)
                    End If
                Catch exx As Exception
                    Throw New Exception(sb.Append("Invalid Request. Schema Validation Failed.").Append(vbNewLine).Append(exx.Message).ToString())
                End Try

                AuthenticateUser(oDoc, ttCredential)

            Catch ex As Exception
                If Not logged Then
                    If Trace Then CoreLib.SendTrace(ttCredential.UserID, sb.Append("ttMain ").Append(ttServiceID).ToString(), "============= OTA Request ============= ", strRequest, UUID)
                    sb.Remove(0, sb.Length())
                    If Not UUID Is Nothing Then
                        With ttCredential
                            LogMessageToFile(enLogType.Request, UUID, ServerName, .RequestorID, .UserID, sb.Append(.Providers(0).Name).Append(" ").Append(.System).ToString(), ttServiceID, strRequest, StartTime, 0, ex.Message)
                            sb.Remove(0, sb.Length())
                        End With
                    End If
                End If
                Throw
            Finally
                GC.Collect()
            End Try
        End Sub

        Public Sub PostServiceRequest(ByRef strResponse As String, ByVal ValidateXSDOut As Boolean, ByVal ttServiceID As Integer, ByVal UserID As String, Optional ByVal Version As String = "")
            Dim sb As StringBuilder = New StringBuilder()

            Try
                If ValidateXSDOut Then
                    CoreLib.ValidateXML(strResponse, ttServiceID, enSchemaType.Response, UserID, Version)
                End If
            Catch ex As Exception
                Throw New Exception(sb.Append("Invalid Response. Schema Validation Failed.").Append(vbNewLine).Append(ex.Message).ToString())
            Finally
                GC.Collect()
            End Try

        End Sub

        Public Sub ndcPreServiceRequest(ByRef strRequest As String, ByRef oApp As HttpApplicationState, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems,
                                    ByVal StartTime As Date, ByVal ttServiceID As Integer, ByVal ServerName As String, ByRef UUID As String, ByVal POS As POS, Optional ByVal Version As String = "")
            Dim oDoc As XmlDocument = Nothing
            Dim ValidateXSDIn As Boolean
            Dim oLog As cLog = Nothing
            Dim Logged As Boolean = False
            Dim sb As StringBuilder = New StringBuilder()

            Try
                ttCredential = ndcGetTravelTalkCredential(strRequest, ttServiceID, POS)

                oDoc = oApp.Get("ttACL")
                ValidateXSDIn = oApp.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("In").ToString())
                sb.Remove(0, sb.Length())

                If Trace Then CoreLib.SendTrace(ttCredential.UserID, sb.Append("ttMain ").Append(ttServiceID).ToString(), "============= OTA Request ============= ", strRequest, ttProviderSystems.LogUUID)
                sb.Remove(0, sb.Length())

                ' SQL Message Log
                Try

                    oLog = New cLog
                    With ttCredential
                        If ttServiceID <> 2 And ttServiceID <> 6 And ttServiceID <> 7 And ttServiceID <> 24 And ttServiceID <> 25 And ttServiceID <> 81 And ttServiceID <> 85 Then
                            UUID = oLog.LogRequest(ServerName, .RequestorID, .UserID, sb.Append(.Providers(0).Name).Append(" ").Append(.System).ToString(), ttServiceID, strRequest, StartTime)
                            sb.Remove(0, sb.Length())
                        Else
                            ' request set to empty to to send data over network for nothing 
                            UUID = oLog.LogRequest(ServerName, .RequestorID, .UserID, sb.Append(.Providers(0).Name).Append(" ").Append(.System).ToString(), ttServiceID, "", StartTime)
                            sb.Remove(0, sb.Length())
                        End If
                    End With


                    Logged = True

                Catch e As Exception
                    ' Just Ignore Log Class will Log Error to Log File
                Finally
                    If Not oLog Is Nothing Then
                        oLog = Nothing
                    End If
                End Try

                Try
                    If ValidateXSDIn Then
                        CoreLib.ValidateXML(strRequest, ttServiceID, modCore.enSchemaType.Request, ttCredential.UserID, Version)
                    End If
                Catch exx As Exception
                    Throw New Exception(sb.Append("Invalid Request. Schema Validation Failed.").Append(vbNewLine).Append(exx.Message).ToString())
                    sb.Remove(0, sb.Length())
                End Try

                ndcAuthenticateUser(oDoc, ttCredential)

            Catch ex As Exception
                If Not Logged Then
                    If Trace Then CoreLib.SendTrace(ttCredential.UserID, sb.Append("ttMain ").Append(ttServiceID).ToString(), "============= OTA Request ============= ", strRequest, ttProviderSystems.LogUUID)
                    sb.Remove(0, sb.Length())
                    If Not UUID Is Nothing Then
                        With ttCredential
                            LogMessageToFile(enLogType.Request, UUID, ServerName, .RequestorID, .UserID, sb.Append(.Providers(0).Name).Append(" ").Append(.System).ToString(), ttServiceID, strRequest, StartTime, 0, ex.Message)
                            sb.Remove(0, sb.Length())
                        End With
                    End If
                End If
                Throw ex
            Finally
                GC.Collect()
            End Try
            sb = Nothing
        End Sub

        Public Sub LogDeals(ByRef strRequest As String, ByRef strResponse As String)

            Dim oLogData As cLogData

            Try
                oLogData = New cLogData
                oLogData.LogDataDeals(strRequest, strResponse)

            Catch ex As Exception
                ' Just Ignore Log Class will Log Error to Log File
            Finally
                GC.Collect()
            End Try

        End Sub

        Public Function GetDeals(ByRef strRequest As String) As String

            Dim oLogData As cLogData
            Dim strResponse As String = ""

            Try
                oLogData = New cLogData
                strResponse = oLogData.GetDataDeals(strRequest)
            Catch ex As Exception
                ' Just Ignore Log Class will Log Error to Log File
            Finally
                GC.Collect()
            End Try

            Return strResponse
        End Function

#End Region

#Region " Authentication "

        Public Function GetTravelTalkDefaultCredential(ByRef strRequest As String, ByVal ttServiceID As Integer) As TravelTalkCredential
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim oReqDoc As XmlDocument
            Dim oRoot As XmlElement
            Dim oNodePOS As XmlNode
            Dim i As Integer
            Dim count As Integer
            Dim customErr As Boolean = False
            Dim strError As String
            Dim sb As StringBuilder = New StringBuilder()

            strRequest = strRequest.Replace("URL=""""", sb.Append("URL=""").Append(HttpContext.Current.Request.UserHostName).Append("""").ToString())
            sb.Remove(0, sb.Length())

            Try
                oReqDoc = New XmlDocument
                oReqDoc.LoadXml(strRequest)
                oRoot = oReqDoc.DocumentElement
            Catch ex As Exception
                Throw New Exception(sb.Append("Error Loading Request XML Document.").Append(ex.Message).ToString())
            End Try

            Try
                If Not oRoot.HasChildNodes Then
                    customErr = True
                    Throw New Exception(sb.Append("Invalid or empty request.").Append(vbNewLine).Append(strRequest).ToString())
                End If

                oNodePOS = oRoot.SelectSingleNode("POS")

                If oNodePOS Is Nothing Then
                    customErr = True
                    Throw New Exception("POS node element is missing or not valid.")
                End If

                If oNodePOS.SelectSingleNode("Source/RequestorID/@ID") Is Nothing Then
                    customErr = True
                    Throw New Exception("RequestorID is missing or not valid.")
                End If

                With ttCredential
                    .RequestorID = oNodePOS.SelectSingleNode("Source/RequestorID").Attributes("ID").Value
                    .System = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/System").InnerText
                    .UserID = "Amadeus"
                    .Password = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/Password").InnerText

                    If .UserID = "FlightSite" And Not (strRequest.Contains("<VendorPref Code=")) And (ttServiceID = 6 Or ttServiceID = 7) Then
                        strRequest = strRequest.Replace("<System>", "<Name>Amadeus</Name><System>")
                        oReqDoc.LoadXml(strRequest)
                        oRoot = oReqDoc.DocumentElement
                        oNodePOS = oRoot.SelectSingleNode("POS")
                    End If

                    count = oNodePOS.SelectNodes("TPA_Extensions/Provider/Name").Count - 1

                    ReDim .Providers(count)

                    For i = 0 To count
                        .Providers(i).PCC = ""
                    Next

                    'If oNodePOS.SelectSingleNode("Source").Attributes("PseudoCityCode") Is Nothing Then

                    .Providers(0).PCC = "NYC1S211F"
                    .Providers(0).Name = "Amadeus"

                    oNodePOS = oNodePOS.SelectSingleNode("TPA_Extensions/Provider")

                    'For i = 0 To count
                    '    .Providers(i).Name = "Amadeus"
                    '    If Not oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode") Is Nothing Then
                    '        If oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode").Value.Trim.Length > 0 Then
                    '            .Providers(i).PCC = oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode").Value.Trim
                    '        End If
                    '    ElseIf i > 0 Then
                    '        .Providers(i).PCC = "*" + .Providers(i - 1).PCC
                    '    End If
                    'Next
                End With

            Catch ex As Exception
                If customErr Then
                    strError = ex.Message
                Else
                    strError = "Error Loading User Credentials. POS node is missing or incomplete."
                End If
                Throw New Exception(strError)
            End Try

            Return ttCredential

        End Function

        Public Function GetTravelTalkCredential(ByRef strRequest As String, ByVal ttServiceID As Integer) As TravelTalkCredential
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim oReqDoc As XmlDocument
            Dim oRoot As XmlElement
            Dim oNodePOS As XmlNode
            Dim i As Integer
            Dim count As Integer
            Dim customErr As Boolean = False
            Dim strError As String

            'strRequest = strRequest.Insert(strRequest.IndexOf("<RequestorID") + 13, " URL=""").Append(HttpContext.Current.Request.UserHostName).Append(""" ")
            strRequest = strRequest.Replace("URL=""""", $"URL=""{HttpContext.Current.Request.UserHostName}""")

            Try
                oReqDoc = New XmlDocument
                oReqDoc.LoadXml(strRequest)
                oRoot = oReqDoc.DocumentElement
            Catch ex As Exception
                Throw New Exception($"Error Loading Request XML Document.{ex.Message}")
            End Try

            Try
                If Not oRoot.HasChildNodes Then
                    customErr = True
                    Throw New Exception($"Invalid or empty request.{Environment.NewLine}{strRequest}")
                End If

                oNodePOS = oRoot.SelectSingleNode("POS")

                If oNodePOS Is Nothing Then
                    customErr = True
                    Throw New Exception("POS node element is missing or not valid.")
                End If

                If oNodePOS.SelectSingleNode("Source/RequestorID/@ID") Is Nothing Then
                    customErr = True
                    Throw New Exception("RequestorID is missing or not valid.")
                End If

                With ttCredential
                    .RequestorID = oNodePOS.SelectSingleNode("Source/RequestorID").Attributes("ID").Value
                    .System = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/System").InnerText
                    .UserID = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/Userid").InnerText
                    .Password = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/Password").InnerText

                    If .UserID = "FlightSite" And Not (strRequest.Contains("<VendorPref Code=")) And (ttServiceID = 6 Or ttServiceID = 7) Then
                        strRequest = strRequest.Replace("<System>", "<Name>Amadeus</Name><System>")
                        oReqDoc.LoadXml(strRequest)
                        oRoot = oReqDoc.DocumentElement
                        oNodePOS = oRoot.SelectSingleNode("POS")
                    End If

                    count = oNodePOS.SelectNodes("TPA_Extensions/Provider/Name").Count - 1

                    ReDim .Providers(count)

                    For i = 0 To count
                        .Providers(i).PCC = ""
                    Next

                    If oNodePOS.SelectSingleNode("Source").Attributes("PseudoCityCode") Is Nothing Then
                        .Providers(0).PCC = ""
                    Else
                        .Providers(0).PCC = oNodePOS.SelectSingleNode("Source").Attributes("PseudoCityCode").Value
                    End If

                    oNodePOS = oNodePOS.SelectSingleNode("TPA_Extensions/Provider")

                    For i = 0 To count
                        .Providers(i).Name = oNodePOS.SelectNodes("Name").Item(i).InnerText
                        If Not oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode") Is Nothing Then
                            If oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode").Value.Trim.Length > 0 Then
                                .Providers(i).PCC = oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode").Value.Trim
                            End If
                        ElseIf i > 0 Then
                            .Providers(i).PCC = "*" + .Providers(i - 1).PCC
                        End If
                    Next
                End With

            Catch ex As Exception
                If customErr Then
                    strError = ex.Message
                Else
                    strError = "Error Loading User Credentials. POS node is missing or incomplete."
                End If
                Throw New Exception(strError)
            End Try

            Return ttCredential

        End Function

        Public Function ndcGetTravelTalkCredential(ByRef strRequest As String, ByVal ttServiceID As Integer, ByVal POS As POS) As TravelTalkCredential
            Dim ttCredential As TravelTalkCredential = Nothing
            'Dim oReqDoc As XmlDocument = Nothing
            'Dim oRoot As XmlElement = Nothing
            'Dim oNodePOS As XmlNode = Nothing
            Dim i As Integer
            Dim Count As Integer
            Dim CustomErr As Boolean = False
            Dim strError As String = ""
            Dim sb As StringBuilder = New StringBuilder()

            'strRequest = strRequest.Insert(strRequest.IndexOf("<RequestorID") + 13, " URL=""").Append(HttpContext.Current.Request.UserHostName).Append(""" ")
            strRequest = strRequest.Replace("URL=""""", sb.Append("URL=""").Append(HttpContext.Current.Request.UserHostName).Append("""").ToString())
            sb.Remove(0, sb.Length())

            'Try
            '    oReqDoc = New XmlDocument
            '    oReqDoc.LoadXml(strRequest)
            '    oRoot = oReqDoc.DocumentElement
            'Catch ex As Exception
            '    CustomErr = True
            '    Throw New Exception(sb.Append("Error Loading Request XML Document.").Append(ex.Message).ToString())
            '    sb.Remove(0, sb.Length())
            'End Try

            Try
                'If Not oRoot.HasChildNodes Then
                '    CustomErr = True
                '    Throw New Exception(sb.Append("Invalid or empty request.").Append(vbNewLine).Append(strRequest).ToString())
                '    sb.Remove(0, sb.Length())
                'End If

                'oNodePOS = oRoot.SelectSingleNode("POS")

                'If oNodePOS Is Nothing Then
                '    CustomErr = True
                '    Throw New Exception("POS node element is missing or not valid.")
                'End If

                'If oNodePOS.SelectSingleNode("Source/RequestorID/@ID") Is Nothing Then
                '    CustomErr = True
                '    Throw New Exception("RequestorID is missing or not valid.")
                'End If

                With ttCredential

                    If POS.Source.RequestorID.ID IsNot Nothing Then
                        .RequestorID = POS.Source.RequestorID.ID.ToString()
                    End If

                    If POS.TPA_Extensions.Provider.GDSSystem IsNot Nothing Then
                        .System = POS.TPA_Extensions.Provider.GDSSystem.ToString()
                    End If

                    If POS.TPA_Extensions.Provider.Userid IsNot Nothing Then
                        .UserID = POS.TPA_Extensions.Provider.Userid.ToString()
                    End If

                    If POS.TPA_Extensions.Provider.Password IsNot Nothing Then
                        .Password = POS.TPA_Extensions.Provider.Password.ToString()
                    End If

                    '.RequestorID = oNodePOS.SelectSingleNode("Source/RequestorID").Attributes("ID").Value
                    '.System = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/System").InnerText
                    '.UserID = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/Userid").InnerText
                    '.Password = oNodePOS.SelectSingleNode("TPA_Extensions/Provider/Password").InnerText

                    Count = POS.TPA_Extensions.Provider.Name.Length

                    ReDim .Providers(Count - 1)

                    'For i = 0 To Count
                    '    .Providers(i).PCC = ""
                    'Next

                    'If POS.Source.PseudoCityCode Is Nothing Then
                    '    .Providers(0).PCC = ""
                    'Else
                    .Providers(0).PCC = POS.Source.PseudoCityCode.ToString()
                    'End If

                    'oNodePOS = oNodePOS.SelectSingleNode("TPA_Extensions/Provider")

                    For i = 0 To Count - 1
                        .Providers(i).Name = POS.TPA_Extensions.Provider.Name(i).Value
                        If Not POS.TPA_Extensions.Provider.Name(i).PseudoCityCode Is Nothing Then
                            If POS.TPA_Extensions.Provider.Name(i).PseudoCityCode.ToString() <> "" Then
                                .Providers(i).PCC = POS.TPA_Extensions.Provider.Name(i).PseudoCityCode.ToString()
                            End If
                            'ElseIf i > 0 Then
                            '    .Providers(i).PCC = "*" + .Providers(i - 1).PCC
                        End If
                    Next

                    'For i = 0 To Count
                    '    .Providers(i).Name = oNodePOS.SelectNodes("Name").Item(i).InnerText
                    '    If Not oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode") Is Nothing Then
                    '        If oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode").Value.Trim.Length > 0 Then
                    '            .Providers(i).PCC = oNodePOS.SelectNodes("Name").Item(i).Attributes("PseudoCityCode").Value.Trim
                    '        End If
                    '    ElseIf i > 0 Then
                    '        .Providers(i).PCC = "*" + .Providers(i - 1).PCC
                    '    End If
                    'Next
                End With

            Catch ex As Exception
                If CustomErr Then
                    strError = ex.Message
                Else
                    strError = "Error Loading User Credentials. POS node Is missing Or incomplete."
                End If
                Throw New Exception(strError)
            End Try
            sb = Nothing
            Return ttCredential

        End Function

        Public Sub AuthenticateUser(ByRef oDoc As XmlDocument, ByVal ttCredential As TravelTalkCredential)
            Dim oRoot As XmlElement
            Dim oNode As XmlNode
            Dim sb As StringBuilder = New StringBuilder()

            oRoot = oDoc.DocumentElement

            oNode = oRoot.SelectSingleNode(sb.Append("Customer[@RequestorID='").Append(ttCredential.RequestorID).Append("']").ToString())
            sb.Remove(0, sb.Length())

            If oNode Is Nothing Then
                Throw New Exception(sb.Append("Customer ").Append(ttCredential.RequestorID).Append(" is not valid.").ToString())
            Else
                oNode = oNode.SelectSingleNode(sb.Append("User[Username='").Append(ttCredential.UserID).Append("']").ToString())
                sb.Remove(0, sb.Length())
                If oNode Is Nothing Then
                    Throw New Exception(sb.Append("User  ").Append(ttCredential.UserID).Append(" is not valid for Customer ").Append(ttCredential.RequestorID).ToString())
                End If
            End If

            With ttCredential
                If StrComp(.Password, oNode("Password").InnerText) <> 0 Then
                    Throw New Exception(sb.Append("Password ").Append(.Password).Append(" is not valid for User ").Append(.UserID).ToString())
                ElseIf DateDiff(DateInterval.Day, CType(oNode.SelectSingleNode("Services/Start").InnerText, Date), Today) < 0 Then
                    Throw New Exception(sb.Append("Access Denied. Services will start on ").Append(oNode.SelectSingleNode("Services/Start").InnerText).ToString())
                ElseIf DateDiff(DateInterval.Day, CType(oNode.SelectSingleNode("Services/End").InnerText, Date), Today) > 0 Then
                    Throw New Exception(sb.Append("Access Denied. Services expired on ").Append(oNode.SelectSingleNode("Services/End").InnerText).ToString())
                End If
            End With
        End Sub

        Public Sub ndcAuthenticateUser(ByRef oDoc As XmlDocument, ByVal ttCredential As TravelTalkCredential)
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing
            Dim sb As StringBuilder = New StringBuilder()

            oRoot = oDoc.DocumentElement

            oNode = oRoot.SelectSingleNode(sb.Append("Customer[@RequestorID='").Append(ttCredential.RequestorID).Append("']").ToString())
            sb.Remove(0, sb.Length())

            If oNode Is Nothing Then
                Throw New Exception(sb.Append("Customer ").Append(ttCredential.RequestorID).Append(" is not valid.").ToString())
                sb.Remove(0, sb.Length())
            Else
                oNode = oNode.SelectSingleNode(sb.Append("User[Username='").Append(ttCredential.UserID).Append("']").ToString())
                sb.Remove(0, sb.Length())
                If oNode Is Nothing Then
                    Throw New Exception(sb.Append("User  ").Append(ttCredential.UserID).Append(" is not valid for Customer ").Append(ttCredential.RequestorID).ToString())
                    sb.Remove(0, sb.Length())
                End If
            End If

            With ttCredential
                If StrComp(.Password, oNode("Password").InnerText) <> 0 Then
                    Throw New Exception(sb.Append("Password ").Append(.Password).Append(" is not valid for User ").Append(.UserID).ToString())
                    sb.Remove(0, sb.Length())
                ElseIf DateDiff(DateInterval.Day, CType(oNode.SelectSingleNode("Services/Start").InnerText, Date), Today) < 0 Then
                    Throw New Exception(sb.Append("Access Denied. Services will start on ").Append(oNode.SelectSingleNode("Services/Start").InnerText).ToString())
                    sb.Remove(0, sb.Length())
                ElseIf DateDiff(DateInterval.Day, CType(oNode.SelectSingleNode("Services/End").InnerText, Date), Today) > 0 Then
                    Throw New Exception(sb.Append("Access Denied. Services expired on ").Append(oNode.SelectSingleNode("Services/End").InnerText).ToString())
                    sb.Remove(0, sb.Length())
                End If
            End With
        End Sub

#End Region

#Region " Get Decode Values "

        Public Function GetDecodeValue(ByRef oDV As DataView, ByRef strCode As String) As String
            Try
                For Each row As DataRow In oDV.Table.Rows
                    If row("Code").ToString().Trim().ToUpper().Equals(strCode.Trim().ToUpper()) Then
                        Return row("Name").ToString()
                    End If
                    If row("Code").ToString().Trim().ToUpper().Contains(strCode.Trim().ToUpper()) Then
                        Return row("Name").ToString()
                    End If
                Next

                'AK: This has to be perform if nothing were found. 
                Dim elems As List(Of String) = strCode.Split(" ").ToList()

                For Each word As String In elems
                    For Each row As DataRow In oDV.Table.Rows
                        If row("Code").ToString().Trim().ToUpper().Contains(strCode.Trim().ToUpper()) Then
                            Return row("Name").ToString()
                        End If
                    Next
                Next

                Return String.Empty

                'Dim i As Integer
                'i = oDV.Find(strCode)
                'If i > -1 Then
                '    Return oDV.Item(i).Item("Name").ToString
                'Else
                '    Return ""
                'End If

            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

        Public Function GetCodeValue(ByRef oDV As DataView, ByRef strID As String) As String
            Try
                For Each row As DataRow In oDV.Table.Rows
                    If row("ID").ToString().Trim().ToUpper().Equals(strID.Trim().ToUpper()) Then
                        Return row("Code").ToString()
                    End If
                    If row("ID").ToString().Trim().ToUpper().Contains(strID.Trim().ToUpper()) Then
                        Return row("Code").ToString()
                    End If
                Next

                Return String.Empty

            Catch ex As Exception
                Return String.Empty
            End Try
        End Function

        Public Function GetEncodeValue(ByRef oDV As DataView, ByRef strName As String) As String
            Try
                If String.IsNullOrEmpty(strName) Then
                    Return String.Empty
                End If

                If strName.Contains("OPERATED BY") Then
                    strName = strName.Replace("OPERATED BY ", "")
                End If

                'Search all for exact name
                For Each row As DataRow In oDV.Table.Rows
                    If row("Name").ToString().Trim().ToUpper().Equals(strName.Trim().ToUpper()) Then
                        Return row("Code").ToString()
                    End If
                Next
                'Search all for similar name
                For Each row As DataRow In oDV.Table.Rows
                    If row("Name").ToString().Trim().ToUpper().Contains(strName.Trim().ToUpper()) Then
                        Return row("Code").ToString()
                    End If
                Next

                '******************************
                'Try to cut Airline Name
                'Example: Trans American Airlines F Ta
                'But we need to look only at: Trans American Airlines
                '******************************
                strName = strName.ToUpper().Replace("AIR LINES", "AIRLINES")

                Dim lstAirName = strName.Split(CType(" ", Char()), StringSplitOptions.RemoveEmptyEntries)
                Dim shortName As String = String.Empty

                'In case if Code already in the RTSVI line.
                '0123456789012345678901234567890123456789
                '0         1         2         3  
                'Azul Linhas Aereas Brasil Ad 2924
                '- AD is an airline code
                'AEROLITORAL DBA AEROMEXIC AM
                '- AM is an airline code
                'SKYWEST AS AMERICAN EAGLE
                'SKYWEST should be used

                If strName.ToUpper().Contains(" AS ") Then

                    For Each word As String In lstAirName
                        If word.Equals("AS") Then
                            Exit For
                        End If

                        For Each row As DataRow In oDV.Table.Rows
                            If row("Name").ToString().Trim().ToUpper().Equals(word.Trim().ToUpper()) Then
                                Return row("Code").ToString()
                            End If

                            If row("Name").ToString().Trim().ToUpper().Contains(word.Trim().ToUpper()) Then
                                Return row("Code").ToString()
                            End If
                        Next
                    Next
                Else
                    Dim lastIndex = lstAirName.Count() - 1

                    If IsNumeric(lstAirName.Last()) AndAlso lstAirName(lastIndex - 1).Length.Equals(2) Then
                        Return lstAirName(lastIndex - 1)
                    End If

                    If Not IsNumeric(lstAirName(lastIndex)) AndAlso lstAirName(lastIndex).Length.Equals(2) Then
                        Return lstAirName.Last()
                    End If

                End If



                For Each word As String In lstAirName
                    If IsNumeric(word) Then
                        Continue For
                    End If
                    If word.Length > 2 Then
                        shortName += String.Format(" {0}", word)
                    End If
                    If word.ToUpper().Equals("AIRLINES") Then
                        Exit For
                    End If
                Next

                For Each row As DataRow In oDV.Table.Rows
                    If row("Name").ToString().Trim().ToUpper().Equals(shortName.Trim().ToUpper()) Then
                        Return row("Code").ToString()
                    End If

                    If row("Name").ToString().Trim().ToUpper().Contains(shortName.Trim().ToUpper()) Then
                        Return row("Code").ToString()
                    End If
                Next

                Return String.Empty
                'i = oDV.Find(strName)
                'If i > -1 Then
                '    Return oDV.Item(i).Item("Code").ToString
                'Else
                '    Return ""
                'End If
            Catch ex As Exception
                Return ""
            End Try
        End Function

        Public Function IsDecodeValue(ByRef oDV As DataView, ByRef strCode As String) As Boolean

            Return CBool(oDV.Find(strCode) > -1)

        End Function

        Public Function IsCruiseFilterValue(ByRef oDV As DataView, ByVal strCruise As String, ByVal strCode As String) As Boolean
            Dim oVals(1) As Object

            oVals(0) = strCruise
            oVals(1) = strCode

            Return CBool(oDV.Find(oVals) > -1)

        End Function

        Public Function GetCruiseFilterValue(ByRef oDV As DataView, ByVal strCruise As String, ByVal strCode As String) As String
            Dim i As Integer
            Dim oVals(1) As Object

            oVals(0) = strCruise
            oVals(1) = strCode

            i = oDV.Find(oVals)

            If i > -1 Then
                Return oDV.Item(i).Item("value").ToString
            Else
                Return ""
            End If

        End Function

        Public Function IsNothing(ByVal Item As Object, ByVal Replace As Object) As Object
            If Item Is Nothing Then
                Return Replace
            Else
                Return Item.Value
            End If
        End Function

#End Region

#Region " Send Request to Providers "

#Region " Send Air Services Request to GDS "

        Public Function SendAirRequestGalileo(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Galileo.AirServices

            Try
                ttService = New Galileo.AirServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.AirFlifo
                            strResponse = .AirFlifo()
                        Case ttServices.AirPrice
                            strResponse = .AirPrice()
                        Case ttServices.AirRules
                            strResponse = .AirRules()
                        Case ttServices.AirSeatMap
                            strResponse = .AirSeatMap()
                        Case ttServices.LowFarePlus
                            strResponse = .LowFarePlus()
                        Case ttServices.FareDisplay
                            strResponse = .FareDisplay()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Galileo.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendAirRequestAmadeusWS(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim ttService As AmadeusWS.AirServices
            Dim strResponse As String = ""

            Try
                ttService = New AmadeusWS.AirServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ttProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.AirFlifo
                            If (Not ttProviderSystems.AmadeusWS) Or (ttProviderSystems.AmadeusWS And ttProviderSystems.AmadeusWSSchema.Air_FlightInfo <> "") Then
                                strResponse = .AirFlifo()
                            Else
                                Throw New Exception("Air_FlightInfo not authorized")
                            End If
                        Case ttServices.AirPrice
                            strResponse = .AirPrice()
                        Case ttServices.AirRules
                            strResponse = .AirRules()
                        Case ttServices.AirSeatMap
                            strResponse = .AirSeatMap()
                            'Case ttServices.LowFarePlus
                            '    strResponse = .LowFarePlus()
                        Case ttServices.FareInfo
                            strResponse = .FareInfo()
                        Case ttServices.FareDisplay
                            strResponse = .FareDisplay()
                        Case ttServices.AirSchedule
                            strResponse = .AirSchedule()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Amadeus.", Service.ToString()))
                    End Select

                    'ttAA = .ttAPIAdapter
                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendAirRequestSabre(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Sabre.AirServices

            Try
                ttService = New Sabre.AirServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.AirFlifo
                            strResponse = .AirFlifo()
                        Case ttServices.AirPrice
                            strResponse = .AirPrice()
                        Case ttServices.AirRules
                            strResponse = .AirRules()
                        Case ttServices.AirSeatMap
                            strResponse = .AirSeatMap()
                        Case ttServices.FareDisplay
                            strResponse = .FareDisplay()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Sabre.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendAirRequestWorldspan(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Worldspan.AirServices

            Try
                ttService = New Worldspan.AirServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.AirPrice
                            strResponse = .AirPrice()
                        Case ttServices.AirRules
                            strResponse = .AirRules()
                        Case ttServices.AirSeatMap
                            strResponse = .AirSeatMap()
                        Case ttServices.FareDisplay
                            strResponse = .FareDisplay()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Worldspan.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        'Public Function SendAirRequestAirCanada(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
        '    Dim ttService As AirCanada.AirServices = Nothing
        '    Dim strResponse As String = ""
        '    Try
        '        ttService = New AirCanada.AirServices
        '        With ttService
        '            .Version = Version
        '            .XslPath = gXslPath
        '            .ttProviderSystems = ttProviderSystems
        '            .Request = strRequest
        '            Select Case Service
        '                Case ttServices.AirPrice
        '                    strResponse = .AirPrice()
        '                    'Case ttServices.AirRules
        '                    '    strResponse = .AirRules()
        '                Case ttServices.AirSeatMap
        '                    strResponse = .AirSeatMap()
        '                    'Case ttServices.FareInfo
        '                    '    strResponse = .FareInfo()
        '                    'Case ttServices.FareDisplay
        '                    '    strResponse = .FareDisplay()
        '                    'Case ttServices.AirSchedule
        '                    '    strResponse = .AirSchedule()
        '            End Select
        '        End With
        '        Return strResponse
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If Not ttService Is Nothing Then ttService = Nothing
        '    End Try
        'End Function

        'Public Function SendAirRequestPyton(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
        '    Dim ttService As Pyton.AirServices = Nothing
        '    Dim strResponse As String = ""
        '    Try
        '        ttService = New Pyton.AirServices
        '        With ttService
        '            .Version = Version
        '            .XslPath = gXslPath
        '            .ttProviderSystems = ttProviderSystems
        '            .Request = strRequest
        '            Select Case Service
        '                Case ttServices.AirPrice
        '                    strResponse = .AirPrice()
        '                    'Case ttServices.AirRules
        '                    '    strResponse = .AirRules()
        '                    'Case ttServices.AirSeatMap
        '                    '   strResponse = .AirSeatMap()
        '                    'Case ttServices.FareInfo
        '                    '    strResponse = .FareInfo()
        '                    'Case ttServices.FareDisplay
        '                    '    strResponse = .FareDisplay()
        '                    'Case ttServices.AirSchedule
        '                    '    strResponse = .AirSchedule()
        '            End Select
        '        End With
        '        Return strResponse
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If Not ttService Is Nothing Then ttService = Nothing
        '    End Try
        'End Function

        Public Function SendAirRequestTravelport(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Travelport.AirServices

            Try
                ttService = New Travelport.AirServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.AirFlifo
                            strResponse = .AirFlifo()
                        Case ttServices.AirPrice
                            strResponse = .AirPrice()
                        Case ttServices.AirRules
                            strResponse = .AirRules()
                        Case ttServices.AirSeatMap
                            strResponse = .AirSeatMap()
                        Case ttServices.LowFarePlus
                            strResponse = .LowFarePlus()
                        Case ttServices.FareDisplay
                            strResponse = .FareDisplay()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Galileo.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function
#End Region

#Region " Send Car Services Request to GDS "

        Public Function SendCarRequestGalileo(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Galileo.CarServices

            Try
                ttService = New Galileo.CarServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.CarInfo
                            strResponse = .CarInfo()
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendCarRequestAmadeusWS(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim ttService As CarServices
            Dim strResponse As String = ""

            Try
                ttService = New CarServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ttProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.CarInfo
                            strResponse = .CarInfo()
                        Case ttServices.CarRules
                            strResponse = .CarRules()
                        Case ttServices.CarList
                            strResponse = .CarList()
                    End Select

                    'ttAA = .ttAPIAdapter
                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendCarRequestSabre(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Sabre.CarServices

            Try
                ttService = New Sabre.CarServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.CarInfo
                            strResponse = .CarInfo()
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

#End Region

#Region " Send Hotel Services Request to GDS "

        Public Function SendHotelRequestGalileo(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Galileo.HotelServices

            Try
                ttService = New Galileo.HotelServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.HotelInfo
                            strResponse = .HotelInfo()
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendHotelRequestAmadeusWS(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim ttService As HotelServices
            Dim strResponse As String = ""

            Try
                ttService = New HotelServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ttProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.HotelInfo
                            strResponse = .HotelInfo()
                    End Select

                    'ttAA = .ttAPIAdapter
                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendHotelRequestSabre(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Sabre.HotelServices

            Try
                ttService = New Sabre.HotelServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.HotelInfo
                            strResponse = .HotelInfo
                    End Select
                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function


#End Region

#Region " Send PNR Services Request to GDS "

        Public Function SendPNRRequestGalileo(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Galileo.PNRServices

            Try
                ttService = New Galileo.PNRServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.PNRRead
                            strResponse = .PNRRead()
                        Case ttServices.PNRCancel
                            strResponse = .PNRCancel()
                        Case ttServices.Queue
                            strResponse = .Queue
                        Case ttServices.QueueRead
                            strResponse = .QueueRead
                        Case ttServices.PNRReprice
                            strResponse = .PNRReprice
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Galileo.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendPNRRequestAmadeusWS(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim ttService As AmadeusWS.PNRServices
            Dim strResponse As String = ""

            Try
                ttService = New AmadeusWS.PNRServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .Request = strRequest
                    .ttProviderSystems = ttProviderSystems

                    Select Case Service
                        Case ttServices.PNRRead
                            strResponse = .PNRRead()
                        Case ttServices.PNREnd
                            strResponse = .PNREnd()
                        Case ttServices.PNRCancel
                            strResponse = .PNRCancel
                        Case ttServices.Queue
                            strResponse = .Queue()
                        Case ttServices.QueueRead
                            strResponse = .QueueRead()
                        Case ttServices.PNRReprice
                            strResponse = .PNRReprice()
                        Case ttServices.PNRSplit
                            strResponse = .PNRSplit()
                        Case ttServices.SearchName
                            strResponse = .SearchName()
                        Case ttServices.TransferOwnership
                            strResponse = .TransferOwnership()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Amadeus.", Service.ToString()))
                    End Select

                    'ttAA = .ttAPIAdapter
                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendPNRRequestSabre(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Sabre.PNRServices

            Try
                ttService = New Sabre.PNRServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.PNRRead
                            strResponse = .PNRRead()
                        Case ttServices.PNRCancel
                            strResponse = .PNRCancel()
                        Case ttServices.PNRReprice
                            strResponse = .PNRReprice()
                        Case ttServices.Queue
                            strResponse = .Queue()
                        Case ttServices.QueueRead
                            strResponse = .QueueRead()
                            'Case ttServices.PNREnd
                            '    strResponse = .PNREnd

                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Sabre.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendPNRRequestWorldspan(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Worldspan.PNRServices

            Try
                ttService = New Worldspan.PNRServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.PNRRead
                            strResponse = .PNRRead()
                        Case ttServices.PNRCancel
                            strResponse = .PNRCancel()
                        Case ttServices.Queue
                            strResponse = .Queue()
                        Case ttServices.PNRReprice
                            strResponse = .PNRReprice()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Worldspan.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendPNRRequestTravelPort(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Travelport.PNRServices

            Try
                ttService = New Travelport.PNRServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest
                    'TODO: PNR Read and other commendted ones are yet to implement
                    Select Case Service
                        Case ttServices.PNRRead
                            strResponse = .PNRRead()
                        Case ttServices.PNRReprice
                            strResponse = .PNRReprice()
                            'Case ttServices.PNRCancel
                            '    'strResponse = .PNRCancel()
                        Case ttServices.Queue
                            strResponse = .Queue
                            'Case ttServices.QueueRead
                            '    'strResponse = .QueueRead
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function
#End Region

#Region " Send Travel Services Request to GDS "

        Public Function SendTravelRequestGalileo(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Galileo.TravelServices

            Try
                ttService = New Galileo.TravelServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.TravelBuild
                            strResponse = .TravelBuild()
                        Case ttServices.TravelModify
                            strResponse = .TravelModify()
                        Case ttServices.IssueTicket
                            strResponse = .IssueTicket
                        Case ttServices.IssueTicketSessioned
                            strResponse = .IssueTicketSessioned
                        Case ttServices.Update
                            strResponse = .Update
                        Case ttServices.UpdateSessioned
                            strResponse = .UpdateSessioned()
                        Case ttServices.TicketVoid
                            strResponse = .VoidTicket
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Galileo.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendTravelRequestAmadeusWS(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim ttService As AmadeusWS.TravelServices
            Dim strResponse As String = ""

            Try
                ttService = New AmadeusWS.TravelServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .Request = strRequest.Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "")
                    .ttProviderSystems = ttProviderSystems

                    Select Case Service
                        Case ttServices.TravelBuild
                            strResponse = .TravelBuild()
                            'Case ttServices.TravelModify
                            '    strResponse = .TravelModify()
                        Case ttServices.IssueTicket
                            strResponse = .IssueTicket()
                        Case ttServices.IssueTicketSessioned
                            strResponse = .IssueTicketSessioned()
                        Case ttServices.StoredFareBuild
                            strResponse = .StoredFareBuild()
                        Case ttServices.StoredFareUpdate
                            strResponse = .StoredFareUpdate()
                        Case ttServices.Update
                            strResponse = .Update()
                        Case ttServices.UpdateSessioned
                            strResponse = .UpdateSessioned()
                        Case ttServices.TicketVoid
                            strResponse = .VoidTicket()
                        Case ttServices.TicketDisplay
                            strResponse = .DisplayTicket()
                        Case ttServices.RefundTicket
                            strResponse = .RefundTicket()
                        Case ttServices.ReissueTicket
                            strResponse = .ReissueTicket()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Amadeus.", Service.ToString()))
                    End Select

                    'ttAA = .ttAPIAdapter
                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendTravelRequestSabre(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Sabre.TravelServices

            Try
                ttService = New Sabre.TravelServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.TravelBuild
                            If Version = "v04" Then
                                strResponse = .TravelBuild_V4
                            Else
                                strResponse = .TravelBuild 'for testing only
                            End If
                        Case ttServices.IssueTicket
                            strResponse = .IssueTicket()
                        Case ttServices.IssueTicketSessioned
                            strResponse = .IssueTicketSessioned()
                        Case ttServices.Update
                            strResponse = .Update()
                        Case ttServices.UpdateSessioned
                            strResponse = .UpdateSessioned()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Sabre.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendTravelRequestWorldspan(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Worldspan.TravelServices

            Try
                ttService = New Worldspan.TravelServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.TravelBuild
                            strResponse = .TravelBuild()
                        Case ttServices.Update
                            strResponse = .Update()
                        Case ttServices.UpdateSessioned
                            strResponse = .UpdateSessioned()
                        Case ttServices.IssueTicketSessioned
                            strResponse = .IssueTicketSessioned()
                        Case ttServices.Authorization
                            strResponse = .Authorization()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Worldspan.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendTravelRequestTravelport(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Travelport.TravelServices

            Try
                ttService = New Travelport.TravelServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        'Case ttServices.TravelBuild
                        '    If Version = "v04" Then
                        '        strResponse = .TravelBuild_V4
                        '    Else
                        '        strResponse = .TravelBuild 'for testing only
                        '    End If
                        'Case ttServices.IssueTicket
                        '    strResponse = .IssueTicket()
                        'Case ttServices.IssueTicketSessioned
                        '    strResponse = .IssueTicketSessioned()
                        'Case ttServices.Update
                        '    strResponse = .Update()
                        Case ttServices.UpdateSessioned
                            strResponse = .UpdateSessioned()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Travelport.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

#End Region

#Region " Send Other Services Request to GDS "

        Public Function SendOtherRequestGalileo(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Galileo.OtherServices

            Try
                ttService = New Galileo.OtherServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.CreateSession
                            strResponse = .CreateSession()
                        Case ttServices.CloseSession
                            strResponse = .CloseSession()
                        Case ttServices.ShowMileage
                            strResponse = .ShowMileage()
                        Case ttServices.CCValid
                            strResponse = .CreditCardValid()
                        Case ttServices.CurConv
                            strResponse = .CurrencyConvertion
                        Case ttServices.TimeDiff
                            strResponse = .TimeDifference()
                        Case ttServices.Cryptic
                            strResponse = .Cryptic()
                        Case ttServices.Native
                            strResponse = .Native()
                        Case ttServices.ETicketVerify
                            strResponse = .ETicketVerify()
                        Case ttServices.MultiMessage
                            strResponse = .MultiMessage()
                        Case ttServices.ProfileRead
                            strResponse = .ProfileRead()
                        Case ttServices.ProfileCreate
                            strResponse = .ProfileCreate()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Galileo.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendOtherRequestAmadeusWS(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim ttService As AmadeusWS.OtherServices
            Dim strResponse As String = ""

            Try
                ttService = New AmadeusWS.OtherServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ttProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.CreateSession
                            strResponse = .CreateSession()
                        Case ttServices.CloseSession
                            strResponse = .CloseSession()
                        Case ttServices.ShowMileage
                            strResponse = .ShowMileage()
                        Case ttServices.CCValid
                            strResponse = .CreditCardValid()
                        Case ttServices.CurConv
                            strResponse = .CurrencyConvertion
                        Case ttServices.TimeDiff
                            strResponse = .TimeDifference()
                        Case ttServices.Cryptic
                            strResponse = .Cryptic()
                        Case ttServices.SalesReport
                            strResponse = .SalesReport()
                        Case ttServices.Native
                            strResponse = .Native()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Amadeus.", Service.ToString()))
                    End Select

                    'ttAA = .ttAPIAdapter
                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendOtherRequestSabre(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Sabre.OtherServices

            Try
                ttService = New Sabre.OtherServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.CreateSession
                            strResponse = .CreateSession()
                        Case ttServices.CloseSession
                            strResponse = .CloseSession()
                        Case ttServices.ShowMileage
                            strResponse = .ShowMileage()
                        Case ttServices.CCValid
                            strResponse = .CreditCardValid()
                        Case ttServices.CurConv
                            strResponse = .CurrencyConvertion
                        Case ttServices.TimeDiff
                            strResponse = .TimeDifference()
                        Case ttServices.Cryptic
                            strResponse = .Cryptic()
                        Case ttServices.SalesReport
                            strResponse = .SalesReport()
                        Case ttServices.Native
                            strResponse = .Native()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Sabre.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendOtherRequestWorldspan(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Worldspan.OtherServices

            Try
                ttService = New Worldspan.OtherServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.CreateSession
                            strResponse = .CreateSession()
                        Case ttServices.CloseSession
                            strResponse = .CloseSession()
                        Case ttServices.Native
                            strResponse = .Native()
                        Case ttServices.Cryptic
                            strResponse = .Cryptic()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Worldspan.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendOtherRequestTravelport(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Travelport.OtherServices

            Try
                ttService = New Travelport.OtherServices

                With ttService
                    '.Version = Version
                    '.XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.CreateSession
                            strResponse = .CreateSession()
                        Case ttServices.CloseSession
                            strResponse = .CloseSession()
                        Case ttServices.Cryptic
                            strResponse = .Cryptic()
                        Case ttServices.Native
                            strResponse = .Native()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Travelport.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendOtherRequestiTravelInsured(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            'Dim ttService As ttiTravelInsuredService.OtherServices = Nothing

            Try
                'ttService = New ttiTravelInsuredService.OtherServices

                'With ttService
                '    .Version = Version
                '    .XslPath = XslPath
                '    .ProviderSystems = ttProviderSystems
                '    .Request = strRequest

                '    Select Case Service
                '        Case ttServices.InsuranceBook
                '            strResponse = .InsuranceBook()
                '        Case ttServices.InsuranceQuote
                '            strResponse = .InsuranceQuote()
                '        Case ttServices.Native
                '            strResponse = .Native()
                '    End Select

                'End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function


#End Region

#Region " Send Cruise Services Request to GDS "

        Public Function SendCruiseRequestAmadeusWS(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim ttService As AmadeusWS.CruiseServices
            Dim strResponse As String = ""

            Try
                ttService = New AmadeusWS.CruiseServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .Request = strRequest
                    .ttProviderSystems = ttProviderSystems

                    Select Case Service
                        Case ttServices.CruiseSailAvail
                            strResponse = .CruiseSailAvail
                        Case ttServices.CruiseFareAvail
                            strResponse = .CruiseFareAvail
                        Case ttServices.CruiseCategoryAvail
                            strResponse = .CruiseCategoryAvail
                        Case ttServices.CruiseCabinAvail
                            strResponse = .CruiseCabinAvail
                        Case ttServices.CruiseCabinHold
                            strResponse = .CruiseCabinHold
                        Case ttServices.CruiseCabinUnhold
                            strResponse = .CruiseCabinUnhold
                        Case ttServices.CruisePriceBooking
                            strResponse = .CruisePriceBooking
                        Case ttServices.CruiseCreateBooking
                            strResponse = .CruiseCreateBooking
                        Case ttServices.CruiseRead
                            strResponse = .CruiseRead
                        Case ttServices.CruiseCancelBooking
                            strResponse = .CruiseCancelBooking
                        Case ttServices.CruiseModifyBooking
                            strResponse = .CruiseModifyBooking
                        Case ttServices.CruisePackageAvail
                            strResponse = .CruisePackageAvail
                        Case ttServices.CruisePackageDesc
                            strResponse = .CruisePackageDesc
                        Case ttServices.CruiseTransferAvail
                            strResponse = .CruiseTransferAvail
                        Case ttServices.CruiseItineraryDesc
                            strResponse = .CruiseItineraryDesc
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Amadeus.", Service.ToString()))
                    End Select
                    'ttAA = .ttAPIAdapter
                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendCruiseRequestGalileo(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim ttService As Galileo.CruiseServices
            Dim strResponse As String = ""


            Try
                ttService = New Galileo.CruiseServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.CruiseSailAvail
                            strResponse = .CruiseSailAvail
                        Case ttServices.CruiseFareAvail
                            strResponse = .CruiseFareAvail
                        Case ttServices.CruiseCategoryAvail
                            strResponse = .CruiseCategoryAvail
                        Case ttServices.CruiseCabinAvail
                            strResponse = .CruiseCabinAvail
                        Case ttServices.CruiseCabinHold
                            strResponse = .CruiseCabinHold
                        Case ttServices.CruiseCabinUnhold
                            strResponse = .CruiseCabinUnhold
                        Case ttServices.CruisePriceBooking
                            strResponse = .CruisePriceBooking
                        Case ttServices.CruiseCreateBooking
                            strResponse = .CruiseCreateBooking
                        Case ttServices.CruiseRead
                            strResponse = .CruiseRead
                        Case ttServices.CruiseCancelBooking
                            strResponse = .CruiseCancelBooking
                        Case ttServices.CruiseModifyBooking
                            strResponse = .CruiseModifyBooking
                        Case ttServices.CruisePackageAvail
                            strResponse = .CruisePackageAvail
                        Case ttServices.CruisePackageDesc
                            strResponse = .CruisePackageDesc
                        Case ttServices.CruiseTransferAvail
                            strResponse = .CruiseTransferAvail
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Galileo.", Service.ToString()))
                    End Select
                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

#End Region

#Region " Send Virtual Card Payment Services Request to GDS "

        Public Function SendPaymentRequestGalileo(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Galileo.OtherServices

            Try
                ttService = New Galileo.OtherServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.CreateSession
                            strResponse = .CreateSession()
                        Case ttServices.CloseSession
                            strResponse = .CloseSession()
                        Case ttServices.ShowMileage
                            strResponse = .ShowMileage()
                        Case ttServices.CCValid
                            strResponse = .CreditCardValid()
                        Case ttServices.CurConv
                            strResponse = .CurrencyConvertion
                        Case ttServices.TimeDiff
                            strResponse = .TimeDifference()
                        Case ttServices.Cryptic
                            strResponse = .Cryptic()
                        Case ttServices.Native
                            strResponse = .Native()
                        Case ttServices.ETicketVerify
                            strResponse = .ETicketVerify()
                        Case ttServices.MultiMessage
                            strResponse = .MultiMessage()
                        Case ttServices.ProfileRead
                            strResponse = .ProfileRead()
                        Case ttServices.ProfileCreate
                            strResponse = .ProfileCreate()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Galileo.", Service.ToString()))
                    End Select

                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendPaymentRequestAmadeusWS(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim ttService As AmadeusWS.PaymentServices
            Dim strResponse As String = ""

            Try
                ttService = New AmadeusWS.PaymentServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ttProviderSystems = ttProviderSystems
                    .Request = strRequest

                    Select Case Service
                        Case ttServices.GenerateVirtualCard
                            strResponse = .CreateVirtualCard()
                            'Case ttServices.CancelVirtualCardLoad
                            '    strResponse = .CancelVirtualCardLoad()
                        Case ttServices.DeleteVirtualCard
                            strResponse = .DeleteVirtualCard()
                        Case ttServices.GetVirtualCardDetails
                            strResponse = .GetVirtualCardDetails()
                        Case ttServices.ListVirtualCards
                            strResponse = .ListVirtualCards()
                            'Case ttServices.ManageDBIData
                            '    strResponse = .ManageDBIData()
                            'Case ttServices.ScheduleVirtualCardLoad
                            '    strResponse = .ScheduleVirtualCardLoad()
                            'Case ttServices.UpdateVirtualCard
                            '    strResponse = .UpdateVirtualCard()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Amadeus.", Service.ToString()))
                    End Select

                    'ttAA = .ttAPIAdapter
                End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendPaymentRequest(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef request As Object) As Object
            Dim strResponse As String = ""
            Dim responseObj As Object = Nothing

            Try
                Dim paymentServices = New VirtualCardPaymentService

                With paymentServices
                    .Provider = ttProviderSystems.Provider
                    .UUID = ttProviderSystems.LogUUID
                    .Request = request

                    Select Case Service
                        Case ttServices.GenerateVirtualCard
                            responseObj = .CreateVirtualCard()
                            'Case ttServices.CancelVirtualCardLoad
                            '    strResponse = .CancelVirtualCardLoad()
                        Case ttServices.DeleteVirtualCard
                            responseObj = .DeleteVirtualCard()
                        Case ttServices.GetVirtualCardDetails
                            responseObj = .GetVirtualCardDetails()
                        Case ttServices.ListVirtualCards
                            responseObj = .ListVirtualCards()
                            'Case ttServices.ManageDBIData
                            '    strResponse = .ManageDBIData()
                            'Case ttServices.ScheduleVirtualCardLoad
                            '    strResponse = .ScheduleVirtualCardLoad()
                            'Case ttServices.UpdateVirtualCard
                            '    strResponse = .UpdateVirtualCard()
                        Case Else
                            Throw New Exception(String.Format("{0} Message is not supported by Amadeus.", Service.ToString()))
                    End Select

                    'ttAA = .ttAPIAdapter
                End With

                Return responseObj
            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendPaymentRequestSabre(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Try
                'Dim ttService As New Sabre.PaymentServices
                'With ttService
                '    .Version = Version
                '    .XslPath = XslPath
                '    .ttProviderSystems = ttProviderSystems
                '    .Request = strRequest
                '    Select Case Service
                '        Case ttServices.GenerateVirtualCard
                '            strResponse = .CreateVirtualCard()
                '            'Case ttServices.CancelVirtualCardLoad
                '            '    strResponse = .CancelVirtualCardLoad()
                '        Case ttServices.DeleteVirtualCard
                '            strResponse = .DeleteVirtualCard()
                '        Case ttServices.GetVirtualCardDetails
                '            strResponse = .GetVirtualCardDetails()
                '        Case ttServices.ListVirtualCards
                '            strResponse = .ListVirtualCards()
                '            'Case ttServices.ManageDBIData
                '            '    strResponse = .ManageDBIData()
                '            'Case ttServices.ScheduleVirtualCardLoad
                '            '    strResponse = .ScheduleVirtualCardLoad()
                '            'Case ttServices.UpdateVirtualCard
                '            '    strResponse = .UpdateVirtualCard()
                '        Case Else
                '            Throw New Exception(String.Format("{0} Message is not supported by Amadeus.", Service.ToString()))
                '    End Select
                '    'ttAA = .ttAPIAdapter
                'End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendPaymentRequestWorldspan(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Worldspan.OtherServices

            Try
                ttService = New Worldspan.OtherServices

                'With ttService
                '    .Version = Version
                '    .XslPath = XslPath
                '    .ProviderSystems = ttProviderSystems
                '    .Request = strRequest

                '    Select Case Service
                '        Case ttServices.CreateSession
                '            strResponse = .CreateSession()
                '        Case ttServices.CloseSession
                '            strResponse = .CloseSession()
                '        Case ttServices.Native
                '            strResponse = .Native()
                '        Case ttServices.Cryptic
                '            strResponse = .Cryptic()
                '        Case Else
                '            Throw New Exception(String.Format("{0} Message is not supported by Worldspan.", Service.ToString()))
                '    End Select

                'End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendPaymentRequestTravelport(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            Dim ttService As Travelport.OtherServices

            Try
                ttService = New Travelport.OtherServices

                'With ttService
                '    '.Version = Version
                '    '.XslPath = XslPath
                '    .ProviderSystems = ttProviderSystems
                '    .Request = strRequest

                '    Select Case Service
                '        Case ttServices.CreateSession
                '            strResponse = .CreateSession()
                '        Case ttServices.CloseSession
                '            strResponse = .CloseSession()
                '        Case ttServices.Cryptic
                '            strResponse = .Cryptic()
                '        Case ttServices.Native
                '            strResponse = .Native()
                '        Case Else
                '            Throw New Exception(String.Format("{0} Message is not supported by Travelport.", Service.ToString()))
                '    End Select

                'End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function

        Public Function SendPaymentRequestiTravelInsured(ByVal Service As ttServices, ByRef ttCredential As TravelTalkCredential, ByRef ttProviderSystems As TripXMLProviderSystems, ByRef strRequest As String, Optional ByVal Version As String = "") As String
            Dim strResponse As String = ""
            'Dim ttService As ttiTravelInsuredService.OtherServices = Nothing

            Try
                'ttService = New ttiTravelInsuredService.OtherServices

                'With ttService
                '    .Version = Version
                '    .XslPath = XslPath
                '    .ProviderSystems = ttProviderSystems
                '    .Request = strRequest

                '    Select Case Service
                '        Case ttServices.InsuranceBook
                '            strResponse = .InsuranceBook()
                '        Case ttServices.InsuranceQuote
                '            strResponse = .InsuranceQuote()
                '        Case ttServices.Native
                '            strResponse = .Native()
                '    End Select

                'End With

                Return strResponse

            Catch ex As Exception
                Throw
            Finally
                GC.Collect()
            End Try

        End Function


#End Region

#End Region

#Region " CreateUUID "

        Public Function CreateUUID() As String
            Dim strGUID As String

            strGUID = UCase(Guid.NewGuid().ToString).Replace("-", "")

            Do While strGUID.LastIndexOf("-", StringComparison.Ordinal) < 20
                strGUID = strGUID.Insert(strGUID.LastIndexOf("-", StringComparison.Ordinal) + 9, "-")
            Loop

            Return strGUID

        End Function

#End Region

#Region " Log to File "

        Public Sub LogMessageToFile(ByVal LogType As Integer, ByRef UUID As String, ByRef WebServer As String,
                           ByRef Customer As String, ByRef UserName As String, ByRef Provider As String,
                           ByVal MessageID As Integer, ByRef Message As String,
                           ByVal MessageDate As Date, ByVal ResponseTime As Integer, ByVal ExError As String)

            Dim fileNumber As Integer
            Dim strLine As String
            Dim sb As StringBuilder = New StringBuilder()

            Try
                sb.Append("<Line>")
                sb.Append("<LogType>").Append(LogType).Append("</LogType>")
                sb.Append("<UUID>").Append(UUID).Append("</UUID>")
                sb.Append("<WebServer>").Append(WebServer).Append("</WebServer>")
                sb.Append("<Customer>").Append(Customer).Append("</Customer>")
                sb.Append("<UserName>").Append(UserName).Append("</UserName>")
                sb.Append("<Provider>").Append(Provider).Append("</Provider>")
                sb.Append("<MessageID>").Append(MessageID).Append("</MessageID>")
                sb.Append("<Message>").Append(Message.Replace(vbCr, "").Replace(vbLf, "").Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "")).Append("</Message>")
                sb.Append("<MessageDate>").Append(MessageDate).Append("</MessageDate>")
                sb.Append("<ResponseTime>").Append(ResponseTime).Append("</ResponseTime>")
                sb.Append("<ExError>").Append(ExError).Append("</ExError>")
                sb.Append("</Line>")
                strLine = sb.ToString()
                sb.Remove(0, sb.Length())

                fileNumber = FreeFile()

                FileOpen(fileNumber, sb.Append(LogPath).Append(String.Format("{0}_Log.txt", DateTime.Now.ToShortDateString())).ToString(), OpenMode.Append)
                sb.Remove(0, sb.Length())
                PrintLine(fileNumber, strLine)
                FileClose(fileNumber)
            Catch ex As Exception
                ' 
            End Try

        End Sub

        Public Sub LogSoapExceptionToFile(ByRef SoapException As String, ByRef SoapEnvelope As String, ByVal ExError As String)

            Dim fileNumber As Integer
            Dim strLine As String
            Dim sb As StringBuilder = New StringBuilder()

            Try
                sb.Append("<Line>")
                sb.Append("<LogType>").Append(1).Append("</LogType>")
                sb.Append("<UUID></UUID>")
                sb.Append("<WebServer></WebServer>")
                sb.Append("<Customer></Customer>")
                sb.Append("<UserName></UserName>")
                sb.Append("<Provider></Provider>")
                sb.Append("<MessageID>SoapException</MessageID>")
                sb.Append("<Message>").Append(SoapEnvelope).Append("</Message>")
                sb.Append("<MessageDate>").Append(Now).Append("</MessageDate>")
                sb.Append("<ResponseTime></ResponseTime>")
                sb.Append("<ExError>").Append(SoapException).Append(" ").Append(ExError).Append("</ExError>")
                sb.Append("</Line>")
                strLine = sb.ToString()
                sb.Remove(0, sb.Length())

                fileNumber = FreeFile()

                FileOpen(fileNumber, sb.Append(LogPath).Append("SoapException.txt").ToString(), OpenMode.Append)
                sb.Remove(0, sb.Length())
                PrintLine(fileNumber, strLine)
                FileClose(fileNumber)
            Catch ex As Exception
                ' 
            End Try
        End Sub

#End Region

#Region " Load Variables and Tables in Memory. Create Amadeus Factories. "

        Public Sub LoadEncodingTables(ByRef oApplication As HttpApplicationState, ByVal strPath As String)
            Dim arEncodingTables(21) As EncodingTables
            Dim oDS As DataSet
            Dim i As Integer
            Dim sb As StringBuilder = New StringBuilder()

            arEncodingTables(0).FileName = "ttwsairlines.xml"
            arEncodingTables(0).TableName = "ttAirlines"
            arEncodingTables(1).FileName = "ttwsairports.xml"
            arEncodingTables(1).TableName = "ttAirports"
            arEncodingTables(2).FileName = "ttwscars.xml"
            arEncodingTables(2).TableName = "ttCars"
            arEncodingTables(3).FileName = "ttwscartypes.xml"
            arEncodingTables(3).TableName = "ttCarTypes"
            arEncodingTables(4).FileName = "ttwscities.xml"
            arEncodingTables(4).TableName = "ttCities"
            arEncodingTables(5).FileName = "ttwsequipments.xml"
            arEncodingTables(5).TableName = "ttEquipments"
            arEncodingTables(6).FileName = "ttwshotels.xml"
            arEncodingTables(6).TableName = "ttHotels"
            arEncodingTables(7).FileName = "ttwshotelrooms.xml"
            arEncodingTables(7).TableName = "ttHotelRooms"
            arEncodingTables(8).FileName = "ttwsCruiseLines.xml"
            arEncodingTables(8).TableName = "ttCruiseLines"
            arEncodingTables(9).FileName = "ttwsCruiseAdvisory.xml"
            arEncodingTables(9).TableName = "ttCruiseAdvisory"
            arEncodingTables(10).FileName = "ttwsCruiseCities.xml"
            arEncodingTables(10).TableName = "ttwsCruiseCities"
            arEncodingTables(11).FileName = "ttwsCruiseRegions.xml"
            arEncodingTables(11).TableName = "ttCruiseRegions"
            arEncodingTables(12).FileName = "ttwsCruiseBedConfiguration.xml"
            arEncodingTables(12).TableName = "ttCruiseBedConfiguration"
            arEncodingTables(13).FileName = "ttwsCruiseInsurance.xml"
            arEncodingTables(13).TableName = "ttCruiseInsurance"
            arEncodingTables(14).FileName = "ttwsCruisePaxTitle.xml"
            arEncodingTables(14).TableName = "ttwsCruisePaxTitle"
            arEncodingTables(15).FileName = "ttwsCreditCards.xml"
            arEncodingTables(15).TableName = "ttwsCreditCards"
            arEncodingTables(16).FileName = "ttwsCruiseInsurance.xml"
            arEncodingTables(16).TableName = "ttwsCruiseInsurance"
            arEncodingTables(17).FileName = "ttwsCruisePricedItems.xml"
            arEncodingTables(17).TableName = "ttCruisePricedItems"
            arEncodingTables(18).FileName = "ttwshotelamenities.xml"
            arEncodingTables(18).TableName = "tthotelamenities"
            arEncodingTables(19).FileName = "ttwshotelareas.xml"
            arEncodingTables(19).TableName = "tthotelareas"
            arEncodingTables(20).FileName = "ttwshotelsubtitles.xml"
            arEncodingTables(20).TableName = "tthotelsubtitles"
            arEncodingTables(21).FileName = "ttwsairlines.xml"
            arEncodingTables(21).TableName = "ttAirlinesNames"
            'arEncodingTables(22).FileName = "ttwscarsnames.xml"
            'arEncodingTables(22).TableName = "ttCarsNames"
            'arEncodingTables(23).FileName = "ttDistantisCities.xml"
            'arEncodingTables(23).TableName = "ttDistantisCities"
            'arEncodingTables(23).PlainText = True
            'arEncodingTables(24).FileName = "ttDistantisCountries.xml"
            'arEncodingTables(24).TableName = "ttDistantisCountries"
            'arEncodingTables(24).PlainText = True
            'arEncodingTables(25).FileName = "ttArpcoCodes.xml"
            'arEncodingTables(25).TableName = "ttArpcoCodes"
            'arEncodingTables(25).PlainText = True

            oApplication.Lock()

            strPath &= "Decoding\"

            For i = 0 To 21
                CoreLib.SendTrace("", "wsTripXML", sb.Append("Loading Table ").Append(arEncodingTables(i).TableName).Append(" in Memory").ToString(), "", String.Empty)
                sb.Remove(0, sb.Length())

                oDS = New DataSet

                Try

                    oDS.ReadXml(sb.Append(strPath).Append(arEncodingTables(i).FileName).ToString())
                    sb.Remove(0, sb.Length())

                    If arEncodingTables(i).TableName = "ttCities" Then
                        oDS.Tables(0).DefaultView.Sort = "CityAirport_Text"
                    ElseIf arEncodingTables(i).TableName = "ttAirlinesNames" Then
                        oDS.Tables(0).DefaultView.Sort = "Name"
                    Else
                        oDS.Tables(0).DefaultView.Sort = "Code"
                    End If

                    oApplication.Add(arEncodingTables(i).TableName, oDS.Tables(0).DefaultView)

                Catch ex As Exception
                    CoreLib.SendTrace("", "wsTripXML", sb.Append("===== Error ===== Loading Table ").Append(arEncodingTables(i).TableName).Append(" in Memory").ToString(), ex.Message, String.Empty)
                    sb.Remove(0, sb.Length())
                End Try

            Next

            oApplication.UnLock()
        End Sub

        Public Sub LoadCruiseTables(ByRef oApplication As HttpApplicationState, ByVal strPath As String)
            Dim arEncodingTables(7) As EncodingTables
            Dim oDS As DataSet
            Dim i As Integer
            Dim sb As StringBuilder = New StringBuilder()

            arEncodingTables(0).FileName = "ttCruiseCities.xml"
            arEncodingTables(0).TableName = "ttCruiseCities"
            arEncodingTables(1).FileName = "ttCruiseCurrency.xml"
            arEncodingTables(1).TableName = "ttCruiseCurrency"
            arEncodingTables(2).FileName = "ttCruiseMot.xml"
            arEncodingTables(2).TableName = "ttCruiseMot"
            arEncodingTables(3).FileName = "ttCruiseShips.xml"
            arEncodingTables(3).TableName = "ttCruiseShips"
            arEncodingTables(4).FileName = "ttCruiseProfiles.xml"
            arEncodingTables(4).TableName = "ttCruiseProfiles"
            arEncodingTables(5).FileName = "ttCruiseCabinFilter.xml"
            arEncodingTables(5).TableName = "ttCruiseCabinFilter"
            arEncodingTables(6).FileName = "ttCruisePaxTitle.xml"
            arEncodingTables(6).TableName = "ttCruisePaxTitle"
            arEncodingTables(7).FileName = "ttCruiseOccupation.xml"
            arEncodingTables(7).TableName = "ttCruiseOccupation"

            oApplication.Lock()

            strPath &= "Decoding\"

            For i = 0 To 7
                CoreLib.SendTrace("", "wsTripXML", sb.Append("Loading Table ").Append(arEncodingTables(i).TableName).Append(" in Memory").ToString(), "", String.Empty)
                sb.Remove(0, sb.Length())

                oDS = New DataSet

                Try

                    oDS.ReadXml(sb.Append(strPath).Append(arEncodingTables(i).FileName).ToString())
                    sb.Remove(0, sb.Length())

                    oDS.Tables(0).DefaultView.Sort = "Cruise, Code"

                    oApplication.Add(arEncodingTables(i).TableName, oDS.Tables(0).DefaultView)

                Catch ex As Exception
                    CoreLib.SendTrace("", "wsTripXML", sb.Append("===== Error ===== Loading Table ").Append(arEncodingTables(i).TableName).Append(" in Memory").ToString(), ex.Message, String.Empty)
                    sb.Remove(0, sb.Length())
                End Try

            Next

            oApplication.UnLock()
        End Sub
        'Initial Block creation for all PCCs
        Public Sub CreatInitialSessionPool(ByRef oApplication As HttpApplicationState, ByRef ttProviderSystems As TripXMLProviderSystems, ByVal Provider As String)

            'Dim i As Integer
            Dim iBlockThread As Thread
            Dim ttAA As AmadeusWSAdapter
            Dim ttSA As Sabre.SabreAdapter
            Dim ttGA As Galileo.GalileoAdapter
            Dim sessionCount As Integer = 0
            Dim oDA As cDA

            Try

                oDA = New cDA("ConnectionString")


                If oDA.CheckInitialPool(ttProviderSystems.PCC, ttProviderSystems.UserID) Then
                    'If oDA.CheckInitialPool(ttProviderSystems.PCC, ttProviderSystems.UserID, ttProviderSystems.System) Then

                    If (Provider.ToLower = "amadeusws") Then

                        If ttProviderSystems.System = "Test" Then
                            ttProviderSystems.URL = "https://test.webservices.amadeus.com"
                        ElseIf ttProviderSystems.System = "Training" Then
                            ttProviderSystems.URL = "https://production.webservices.amadeus.com"
                        Else
                            ttProviderSystems.URL = "https://production.webservices.amadeus.com"
                        End If

                        ttAA = New AmadeusWSAdapter(ttProviderSystems, "V1")

                        ttAA.isSOAP2 = ttProviderSystems.SOAP2
                        ttAA.isSOAP4 = ttProviderSystems.SOAP4
                        ttAA.GetStoredFares = ttProviderSystems.GetStoredFares

                        For i = 0 To ttAA.InitialBlock - 1
                            ttAA.CreateSessionV2()
                            'iBlockThread = New Thread(New ThreadStart(AddressOf ttAA.CreateSessionV2))
                            'iBlockThread.Start()
                            sessionCount += 1
                        Next
                    ElseIf (Provider.ToLower = "sabre") Then

                        ttSA = New Sabre.SabreAdapter(ttProviderSystems, "V1")

                        For i = 0 To ttSA.InitialBlockSize - 1
                            iBlockThread = New Thread(New ThreadStart(AddressOf ttSA.CreateSessionV2))
                            iBlockThread.Start()
                            sessionCount += 1
                        Next
                    ElseIf (Provider.ToLower = "galileo" Or Provider.ToLower = "apollo") Then

                        ttGA = New Galileo.GalileoAdapter(ttProviderSystems, "V1")

                        For i = 0 To ttGA.InitialBlockSize - 1
                            iBlockThread = New Thread(New ThreadStart(AddressOf ttGA.CreateSessionV2))
                            iBlockThread.Start()
                            sessionCount += 1
                        Next


                    End If
                    oDA.UpdatePCCSessions(ttProviderSystems.PCC, sessionCount, ttProviderSystems.UserID)
                    'oDA.UpdatePCCSessions(ttProviderSystems.PCC, SessionCount, ttProviderSystems.UserID, ttProviderSystems.System)
                End If

            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Sub TripXMLStartUp(ByRef oApplication As HttpApplicationState)
            'Dim ttSA As Sabre.SabreAdapter = Nothing
            Dim strPath As String
            Dim strBLPath As String
            Dim oDoc As XmlDocument
            Dim oRoot As XmlElement
            Dim oNode As XmlNode
            Dim oNodeOT As XmlNode
            Dim oNodePCC As XmlNode
            Dim oDocPrv As XmlDocument
            Dim oRootPrv As XmlElement
            Dim oNodePrv As XmlNode
            Dim provider As String = ""
            Dim reqID() As String = Nothing
            Dim arUsers() As String = Nothing
            Dim arFiles() As String = Nothing
            Dim blFiles() As String = Nothing
            Dim arPass() As String = Nothing
            Dim i As Integer
            Dim j As Integer
            Dim key As String
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim validateXSDIn As Boolean
            Dim validateXSDOut As Boolean
            Dim bAggFilter() As Boolean = Nothing
            Dim sb As StringBuilder = New StringBuilder()

            Try
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

                oApplication.Clear()

                GC.Collect()

                strPath = WebConfigurationManager.AppSettings("TripXMLFolder")

                XslPath = sb.Append(strPath).Append("\xsl\").ToString()
                sb.Remove(0, sb.Length())

                LogPath = WebConfigurationManager.AppSettings("TripXMLLogFolder")
                'LogPath = sb.Append(strPath).Append("\Log\").ToString()
                sb.Remove(0, sb.Length())

                SchemaPath = sb.Append(strPath).Append("\Schemas\").ToString()
                sb.Remove(0, sb.Length())

                strPath &= "\Tables\"
                strBLPath = sb.Append(strPath).Append("BL\").ToString()
                sb.Remove(0, sb.Length())

                If Not IO.Directory.Exists(LogPath) Then
                    IO.Directory.CreateDirectory(LogPath)
                End If

                LoadEncodingTables(oApplication, strPath)
                LoadCruiseTables(oApplication, strPath)

                strPath &= "Users\"

                oDoc = New XmlDocument
                ' Load Access Control List into memory
                Try
                    oDoc.Load(sb.Append(strPath).Append("tt_acl.xml").ToString())
                    sb.Remove(0, sb.Length())
                Catch exr As Exception
                    CoreLib.SendTrace("", "modMain", "TripXMLStartUp: Error Loading tt_acl.xml", exr.Message, String.Empty)
                    Throw
                End Try

                ' Add it to Application Object Collection
                oApplication.Lock()
                oApplication.Add("ttACL", oDoc)
                oApplication.UnLock()

                oRoot = oDoc.DocumentElement

                i = 0
                For Each oNode In oRoot.SelectNodes("Customer/User")
                    If DateDiff(DateInterval.Day, CType(oNode.SelectSingleNode("Services/Start").InnerText, Date), Today) >= 0 _
                    And DateDiff(DateInterval.Day, CType(oNode.SelectSingleNode("Services/End").InnerText, Date), Today) < 0 Then
                        i += 1
                        ReDim Preserve reqID(i)
                        ReDim Preserve arUsers(i)
                        ReDim Preserve arFiles(i)
                        ReDim Preserve blFiles(i)
                        ReDim Preserve bAggFilter(i)
                        ReDim Preserve arPass(i)
                        reqID(i) = oNode.SelectSingleNode("../@RequestorID").InnerText
                        arUsers(i) = oNode.SelectSingleNode("Username").InnerText
                        arPass(i) = oNode.SelectSingleNode("Password").InnerText
                        arFiles(i) = sb.Append(strPath).Append(oNode.SelectSingleNode("Config").InnerText).ToString()
                        sb.Remove(0, sb.Length())

                        If oNode.SelectSingleNode("BL") Is Nothing Then
                            blFiles(i) = ""
                        ElseIf oNode.SelectSingleNode("BL").InnerText <> "" Then
                            blFiles(i) = sb.Append(strBLPath).Append(oNode.SelectSingleNode("BL").InnerText).ToString()
                            sb.Remove(0, sb.Length())
                        Else
                            blFiles(i) = ""
                        End If

                        If oNode.SelectSingleNode("BL/@AggFilter") Is Nothing Then
                            bAggFilter(i) = True
                        Else
                            If oNode.SelectSingleNode("BL/@AggFilter").InnerText.ToLower = "false" Then
                                bAggFilter(i) = False
                            Else
                                bAggFilter(i) = True
                            End If
                        End If
                    End If
                Next

                ' Load Provider Config File

                oDocPrv = New XmlDocument
                oDocPrv.Load(sb.Append(strPath).Append("ttProviders.xml").ToString())
                sb.Remove(0, sb.Length())
                oRootPrv = oDocPrv.DocumentElement

                oApplication.Lock()

                oDoc = New XmlDocument

                For i = 1 To arUsers.GetUpperBound(0)
                    Try
                        oDoc.Load(arFiles(i))
                        oRoot = oDoc.DocumentElement

                        ' Get Schema Validation Settings

                        If oRoot.SelectSingleNode("Validate") Is Nothing Then
                            validateXSDIn = False
                            validateXSDOut = False
                        Else
                            validateXSDIn = CBool(oRoot.SelectSingleNode("Validate").Attributes("In").Value = "Y")
                            validateXSDOut = CBool(oRoot.SelectSingleNode("Validate").Attributes("Out").Value = "Y")
                        End If

                        key = sb.Append("XSD").Append(arUsers(i)).Append("In").ToString()
                        sb.Remove(0, sb.Length())
                        oApplication.Add(key, validateXSDIn)

                        key = sb.Append("XSD").Append(arUsers(i)).Append("Out").ToString()
                        sb.Remove(0, sb.Length())
                        oApplication.Add(key, validateXSDOut)

                        ' Get FeaturedProvider
                        key = sb.Append("ttFP").Append(arUsers(i)).ToString()
                        sb.Remove(0, sb.Length())
                        If oRoot.SelectSingleNode("FeaturedProvider") Is Nothing Then
                            oApplication.Add(key, "")
                        Else
                            oApplication.Add(key, oRoot.SelectSingleNode("FeaturedProvider").InnerText)
                        End If

                        ' Get Provider Systems Settings

                        For Each oNode In oRoot.SelectNodes("Provider/System")
                            For Each oNodePCC In oNode.SelectNodes("PCC")
                                Try
                                    provider = oNode.ParentNode.Attributes("Name").InnerText

                                    ' Load the Provider Credentials for all users into memory.
                                    With ttProviderSystems
                                        .UserID = arUsers(i)
                                        .System = oNode.Attributes("Name").Value
                                        .PCC = oNodePCC.Attributes("Code").Value
                                        .UserName = oNodePCC.SelectSingleNode("Username").InnerText
                                        .Password = oNodePCC.SelectSingleNode("Password").InnerText
                                        .Profile = oNodePCC.SelectSingleNode("Profile").InnerText
                                        .BLFile = blFiles(i)
                                        .AggFilter = bAggFilter(i)
                                        .GPass = arPass(i)
                                        .GReqID = reqID(i)
                                        .Provider = provider

                                        If Not oNodePCC.SelectSingleNode("Profile").Attributes("Origin") Is Nothing Then
                                            .Origin = oNodePCC.SelectSingleNode("Profile").Attributes("Origin").Value.ToUpper
                                        Else
                                            .Origin = "NMC-US"
                                        End If

                                        If Not oNodePCC.SelectSingleNode("Profile").Attributes("XML") Is Nothing Then
                                            .ProfileXML = oNodePCC.SelectSingleNode("Profile").Attributes("XML").Value.ToUpper
                                        Else
                                            .ProfileXML = ""
                                        End If

                                        If Not oNodePCC.SelectSingleNode("Profile").Attributes("Cryptic") Is Nothing Then
                                            .ProfileCryptic = oNodePCC.SelectSingleNode("Profile").Attributes("Cryptic").Value.ToUpper
                                        Else
                                            .ProfileCryptic = ""
                                        End If

                                        If Not oNodePCC.SelectSingleNode("Profile").Attributes("Tkt") Is Nothing Then
                                            .ProfileTicketing = oNodePCC.SelectSingleNode("Profile").Attributes("Tkt").Value.ToUpper
                                        Else
                                            .ProfileTicketing = ""
                                        End If


                                        If Not oNodePCC.Attributes("CheckBookedFare") Is Nothing Then
                                            If oNodePCC.Attributes("CheckBookedFare").Value.ToLower = "false" Then
                                                .CheckBookedFare = False
                                            Else
                                                .CheckBookedFare = True
                                            End If
                                        Else
                                            .CheckBookedFare = False
                                        End If

                                        If Not oNodePCC.Attributes("AmadeusTrace") Is Nothing Then
                                            If oNodePCC.Attributes("AmadeusTrace").Value.ToLower = "false" Then
                                                .AmadeusTrace = False
                                            Else
                                                .AmadeusTrace = True
                                            End If
                                        Else
                                            .AmadeusTrace = False
                                        End If

                                        If Not oNodePCC.Attributes("RebookNextFlight") Is Nothing Then
                                            If oNodePCC.Attributes("RebookNextFlight").Value.ToLower = "false" Then
                                                .RebookNextFlight = False
                                            Else
                                                .RebookNextFlight = True
                                            End If
                                        Else
                                            .RebookNextFlight = False
                                        End If


                                        If Not oNodePCC.Attributes("FareMessage") Is Nothing Then
                                            .FareMessage = oNodePCC.Attributes("FareMessage").Value.ToUpper
                                        Else
                                            .FareMessage = "VP"
                                        End If

                                        If Not oNodePCC.Attributes("SaveInDB") Is Nothing Then
                                            .SaveInDB = oNodePCC.Attributes("SaveInDB").Value
                                        Else
                                            .SaveInDB = ""
                                        End If

                                        If Not oNodePCC.Attributes("LogNative") Is Nothing Then
                                            If oNodePCC.Attributes("LogNative").Value.ToLower = "false" Then
                                                .LogNative = False
                                            Else
                                                .LogNative = True
                                            End If
                                        Else
                                            .LogNative = False
                                        End If

                                        If Not oNodePCC.Attributes("NoOfLowFareFlights") Is Nothing Then
                                            .NoOfLowFareFlights = oNodePCC.Attributes("NoOfLowFareFlights").Value
                                        Else
                                            .NoOfLowFareFlights = ""
                                        End If

                                        If Not oNodePCC.Attributes("SessionPool") Is Nothing Then
                                            If oNodePCC.Attributes("SessionPool").Value.ToLower = "false" Then
                                                .SessionPool = False
                                            Else
                                                .SessionPool = True
                                            End If
                                        Else
                                            .SessionPool = False
                                        End If

                                        If Not oNodePCC.Attributes("SOAPHeader") Is Nothing Then
                                            Dim sValue As String = oNodePCC.Attributes("SOAPHeader").Value
                                            Dim eValue As enSOAPHeaderType = [Enum].Parse(GetType(enSOAPHeaderType), sValue)
                                            Select Case eValue
                                                Case enSOAPHeaderType.SOAP4
                                                    .SOAP2 = False
                                                    .SOAP4 = True
                                                Case Else
                                                    .SOAP2 = True
                                                    .SOAP4 = False
                                                    eValue = enSOAPHeaderType.SOAP2
                                            End Select
                                            .SoapHeader = eValue
                                        Else
                                            If Not oNodePCC.Attributes("SOAP2") Is Nothing Then
                                                If oNodePCC.Attributes("SOAP2").Value.ToLower = "false" Then
                                                    .SOAP2 = False
                                                Else
                                                    .SOAP2 = True
                                                    .SoapHeader = enSOAPHeaderType.SOAP2
                                                End If
                                            Else
                                                .SOAP2 = False
                                            End If

                                            If Not oNodePCC.Attributes("SOAP4") Is Nothing Then
                                                If oNodePCC.Attributes("SOAP4").Value.ToLower = "false" Then
                                                    .SOAP4 = False
                                                Else
                                                    .SOAP4 = True
                                                    .SoapHeader = enSOAPHeaderType.SOAP4
                                                End If
                                            Else
                                                .SOAP4 = False
                                            End If
                                        End If


                                        If Not oNodePCC.Attributes("GetStoredFares") Is Nothing Then
                                            If oNodePCC.Attributes("GetStoredFares").Value.ToLower = "false" Then
                                                .GetStoredFares = False
                                            Else
                                                .GetStoredFares = True
                                            End If
                                        Else
                                            .GetStoredFares = True
                                        End If

                                        If Not oNodePCC.Attributes("AddLog") Is Nothing Then
                                            If oNodePCC.Attributes("AddLog").Value.ToLower = "false" Then
                                                .AddLog = False
                                            Else
                                                .AddLog = True
                                            End If
                                        Else
                                            .AddLog = False
                                        End If

                                        If Not oNodePCC.Attributes("HotelMedia") Is Nothing Then
                                            If oNodePCC.Attributes("HotelMedia").Value.ToLower = "false" Then
                                                .HotelMedia = False
                                            Else
                                                .HotelMedia = True
                                            End If
                                        Else
                                            .HotelMedia = False
                                        End If

                                        If Not oNodePCC.Attributes("SendEmailToAgency") Is Nothing Then
                                            If oNodePCC.Attributes("SendEmailToAgency").Value.ToLower = "false" Then
                                                .SendEmailToAgency = False
                                            Else
                                                .SendEmailToAgency = True
                                            End If
                                        Else
                                            .SendEmailToAgency = False
                                        End If

                                        If Not oNodePCC.Attributes("CreateInRHAdmin") Is Nothing Then
                                            If oNodePCC.Attributes("CreateInRHAdmin").Value.ToLower = "false" Then
                                                .CreateInRHAdmin = False
                                            Else
                                                .CreateInRHAdmin = True
                                            End If
                                        Else
                                            .CreateInRHAdmin = False
                                        End If

                                        If Not oNodePCC.Attributes("LFPLight") Is Nothing Then
                                            If oNodePCC.Attributes("LFPLight").Value.ToLower = "false" Then
                                                .LFPLight = False
                                            Else
                                                .LFPLight = True
                                            End If
                                        Else
                                            .LFPLight = False
                                        End If

                                        If Not oNodePCC.Attributes("CouponStatus") Is Nothing Then
                                            If oNodePCC.Attributes("CouponStatus").Value.ToLower = "false" Then
                                                .CouponStatus = False
                                            Else
                                                .CouponStatus = True
                                            End If
                                        Else
                                            .CouponStatus = False
                                        End If

                                        If Not oNodePCC.Attributes("AddLFPStat") Is Nothing Then
                                            If oNodePCC.Attributes("AddLFPStat").Value.ToLower = "false" Then
                                                .AddLFPStat = False
                                            Else
                                                .AddLFPStat = True
                                            End If
                                        Else
                                            .AddLFPStat = False
                                        End If

                                        If Not oNodePCC.Attributes("ProxyURL") Is Nothing Then
                                            .ProxyURL = oNodePCC.Attributes("ProxyURL").Value
                                        Else
                                            .ProxyURL = ""
                                        End If

                                        If Not oNodePCC.Attributes("HotelVersion") Is Nothing Then
                                            .HotelVersion = oNodePCC.Attributes("HotelVersion").Value
                                        Else
                                            .HotelVersion = ""
                                        End If

                                        If Not oNodePCC.Attributes("AmadeusWS") Is Nothing Then
                                            If oNodePCC.Attributes("AmadeusWS").Value.ToLower = "false" Then
                                                .AmadeusWS = False
                                            Else
                                                .AmadeusWS = True

                                                Dim wsNode As XmlNode

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Air_FlightInfo']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Air_FlightInfo = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Air_FlightInfo = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Air_FlightInfoReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Air_FlightInfoReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Air_FlightInfoReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Air_MultiAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Air_MultiAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Air_MultiAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Air_MultiAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Air_MultiAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Air_MultiAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Air_RebookAirSegment']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Air_RebookAirSegment = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Air_RebookAirSegment = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Air_RebookAirSegmentReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Air_RebookAirSegmentReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Air_RebookAirSegmentReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Air_RetrieveSeatMap']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Air_RetrieveSeatMap = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Air_RetrieveSeatMap = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Air_RetrieveSeatMapReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Air_RetrieveSeatMapReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Air_RetrieveSeatMapReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Air_SellFromRecommendation']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Air_SellFromRecommendation = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Air_SellFromRecommendation = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Air_SellFromRecommendationReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Air_SellFromRecommendationReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Air_SellFromRecommendationReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_LocationList']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_LocationList = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_LocationList = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_LocationListReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_LocationListReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_LocationListReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_Availability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_Availability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_Availability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_AvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_AvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_AvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_MultiAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_MultiAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_MultiAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_MultiAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_MultiAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_MultiAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_Policy']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_Policy = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_Policy = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_PolicyReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_PolicyReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_PolicyReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_RateInformationFromAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_RateInformationFromAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_RateInformationFromAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_RateInformationFromAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_RateInformationFromAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_RateInformationFromAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_RateInformationFromCarSegment']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_RateInformationFromCarSegment = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_RateInformationFromCarSegment = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_RateInformationFromCarSegmentReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_RateInformationFromCarSegmentReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_RateInformationFromCarSegmentReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_Sell']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_Sell = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_Sell = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_SellReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_SellReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_SellReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_SingleAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_SingleAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_SingleAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Car_SingleAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Car_SingleAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Car_SingleAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Command_Cryptic']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Command_Cryptic = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Command_Cryptic = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Command_CrypticReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Command_CrypticReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Command_CrypticReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_CancelBooking']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_CancelBooking = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_CancelBooking = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_CancelBookingReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_CancelBookingReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_CancelBookingReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_ClaimBooking']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_ClaimBooking = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_ClaimBooking = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_ClaimBookingReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_ClaimBookingReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_ClaimBookingReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_CreateBooking']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_CreateBooking = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_CreateBooking = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_CreateBookingReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_CreateBookingReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_CreateBookingReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayBusDescription']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayBusDescription = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayBusDescription = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayBusDescriptionReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayBusDescriptionReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayBusDescriptionReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayCabinDescription']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayCabinDescription = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayCabinDescription = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayCabinDescriptionReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayCabinDescriptionReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayCabinDescriptionReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayCategoryDescription']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayCategoryDescription = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayCategoryDescription = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayCategoryDescriptionReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayCategoryDescriptionReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayCategoryDescriptionReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayFareDescription']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayFareDescription = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayFareDescription = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayFareDescriptionReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayFareDescriptionReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayFareDescriptionReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayInclusivePackageDescription']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayInclusivePackageDescription = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayInclusivePackageDescription = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayInclusivePackageDescriptionReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayInclusivePackageDescriptionReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayInclusivePackageDescriptionReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayItineraryDescription']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayItineraryDescription = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayItineraryDescription = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayItineraryDescriptionReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayItineraryDescriptionReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayItineraryDescriptionReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayPrePostPackageDescription']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayPrePostPackageDescription = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayPrePostPackageDescription = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayPrePostPackageDescriptionReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayPrePostPackageDescriptionReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayPrePostPackageDescriptionReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayProductInformation']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayProductInformation = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayProductInformation = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_DisplayProductInformationReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_DisplayProductInformationReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_DisplayProductInformationReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_EnterPassengerInformation']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_EnterPassengerInformation = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_EnterPassengerInformation = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_EnterPassengerInformationReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_EnterPassengerInformationReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_EnterPassengerInformationReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_GetBookingDetails']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_GetBookingDetails = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_GetBookingDetails = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_GetBookingDetailsReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_GetBookingDetailsReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_GetBookingDetailsReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_HoldCabin']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_HoldCabin = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_HoldCabin = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_HoldCabinReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_HoldCabinReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_HoldCabinReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_ModifyBooking']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_ModifyBooking = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_ModifyBooking = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_ModifyBookingReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_ModifyBookingReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_ModifyBookingReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_PriceBooking']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_PriceBooking = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_PriceBooking = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_PriceBookingReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_PriceBookingReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_PriceBookingReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_PriceBookingCancellation']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_PriceBookingCancellation = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_PriceBookingCancellation = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_PriceBookingCancellationReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_PriceBookingCancellationReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_PriceBookingCancellationReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestBusAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestBusAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestBusAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestBusAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestBusAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestBusAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestCabinAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestCabinAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestCabinAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestCabinAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestCabinAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestCabinAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestCategoryAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestCategoryAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestCategoryAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestCategoryAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestCategoryAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestCategoryAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestFareAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestFareAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestFareAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestFareAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestFareAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestFareAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestInclusivePackageAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestInclusivePackageAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestInclusivePackageAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestInclusivePackageAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestInclusivePackageAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestInclusivePackageAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestPrePostPackageAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestPrePostPackageAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestPrePostPackageAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestPrePostPackageAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestPrePostPackageAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestPrePostPackageAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestSailingAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestSailingAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestSailingAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestSailingAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestSailingAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestSailingAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestShoreExcursionAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestShoreExcursionAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestShoreExcursionAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestShoreExcursionAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestShoreExcursionAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestShoreExcursionAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestSpecialServicesAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestSpecialServicesAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestSpecialServicesAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestSpecialServicesAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestSpecialServicesAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestSpecialServicesAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestTransferAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestTransferAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestTransferAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_RequestTransferAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_RequestTransferAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_RequestTransferAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_SearchBooking']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_SearchBooking = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_SearchBooking = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_SearchBookingReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_SearchBookingReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_SearchBookingReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_UnholdCabin']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_UnholdCabin = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_UnholdCabin = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Cruise_UnholdCabinReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Cruise_UnholdCabinReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Cruise_UnholdCabinReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Doc_DisplayItinerary']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Doc_DisplayItinerary = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Doc_DisplayItinerary = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Doc_DisplayItineraryReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Doc_DisplayItineraryReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Doc_DisplayItineraryReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'DocIssuance_IssueTicket']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.DocIssuance_IssueTicket = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.DocIssuance_IssueTicket = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'DocIssuance_IssueTicketReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.DocIssuance_IssueTicketReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.DocIssuance_IssueTicketReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_CheckRules']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_CheckRules = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_CheckRules = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_CheckRulesReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_CheckRulesReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_CheckRulesReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_DisplayFaresForCityPair']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_DisplayFaresForCityPair = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_DisplayFaresForCityPair = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_DisplayFaresForCityPairReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_DisplayFaresForCityPairReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_DisplayFaresForCityPairReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_GetFareFamilyDescription']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_GetFareFamilyDescription = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_GetFareFamilyDescription = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_GetFareFamilyDescriptionReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_GetFareFamilyDescriptionReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_GetFareFamilyDescriptionReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_InformativePricingWithoutPNR']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_InformativePricingWithoutPNR = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_InformativePricingWithoutPNR = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_InformativePricingWithoutPNRReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_InformativePricingWithoutPNRReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_InformativePricingWithoutPNRReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_InformativeBestPricingWithoutPNR']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_InformativeBestPricingWithoutPNR = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_InformativeBestPricingWithoutPNR = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_InformativeBestPricingWithoutPNRReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_InformativeBestPricingWithoutPNRReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_InformativeBestPricingWithoutPNRReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_MasterPricerCalendar']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_MasterPricerCalendar = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_MasterPricerCalendar = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_MasterPricerCalendarReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_MasterPricerCalendarReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_MasterPricerCalendarReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_MetaPricerCalendar']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_MetaPricerCalendar = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_MetaPricerCalendar = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_MetaPricerCalendarReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_MetaPricerCalendarReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_MetaPricerCalendarReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_MasterPricerExpertSearch']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_MasterPricerExpertSearch = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_MasterPricerExpertSearch = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_MasterPricerExpertSearchReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_MasterPricerExpertSearchReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_MasterPricerExpertSearchReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_MasterPricerTravelBoardSearch']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_MasterPricerTravelBoardSearch = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_MasterPricerTravelBoardSearch = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_MasterPricerTravelBoardSearchReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_MasterPricerTravelBoardSearchReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_MasterPricerTravelBoardSearchReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_MetaPricerTravelBoardSearch']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_MetaPricerTravelBoardSearch = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_MetaPricerTravelBoardSearch = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_MetaPricerTravelBoardSearchReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_MetaPricerTravelBoardSearchReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_MetaPricerTravelBoardSearchReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_PricePNRWithBookingClass']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_PricePNRWithBookingClass = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_PricePNRWithBookingClass = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_PricePNRWithBookingClassReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_PricePNRWithBookingClassReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_PricePNRWithBookingClassReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_PricePNRWithLowerFares']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_PricePNRWithLowerFares = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_PricePNRWithLowerFares = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_PricePNRWithLowerFaresReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_PricePNRWithLowerFaresReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_PricePNRWithLowerFaresReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_QuoteItinerary']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_QuoteItinerary = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_QuoteItinerary = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_QuoteItineraryReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_QuoteItineraryReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_QuoteItineraryReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_SellByFareCalendar']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_SellByFareCalendar = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_SellByFareCalendar = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_SellByFareCalendarReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_SellByFareCalendarReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_SellByFareCalendarReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_SellByFareSearch']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_SellByFareSearch = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_SellByFareSearch = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_SellByFareSearchReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_SellByFareSearchReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_SellByFareSearchReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_FlexPricerUpsell']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_FlexPricerUpsell = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_FlexPricerUpsell = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Fare_FlexPricerUpsellReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Fare_FlexPricerUpsellReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Fare_FlexPricerUpsellReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_MultiSingleAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_MultiSingleAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_MultiSingleAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_MultiSingleAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_MultiSingleAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_MultiSingleAvailabilityReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_AvailabilityMultiProperties']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_AvailabilityMultiProperties = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_AvailabilityMultiProperties = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_AvailabilityMultiPropertiesReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_AvailabilityMultiPropertiesReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_AvailabilityMultiPropertiesReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_Features']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_Features = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_Features = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_FeaturesReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_FeaturesReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_FeaturesReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_DescriptiveInfo']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_DescriptiveInfo = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_DescriptiveInfo = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_DescriptiveInfoReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_DescriptiveInfoReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_DescriptiveInfoReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_List']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_List = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_List = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_ListReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_ListReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_ListReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_RateChange']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_RateChange = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_RateChange = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_RateChangeReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_RateChangeReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_RateChangeReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_Sell']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_Sell = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_Sell = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_SellReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_SellReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_SellReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_SingleAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_SingleAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_SingleAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_SingleAvailabilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_SingleAvailabilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_SingleAvailabilityReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_StructuredPricing']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_StructuredPricing = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_StructuredPricing = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_StructuredPricingReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_StructuredPricingReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_StructuredPricingReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_Terms']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_Terms = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_Terms = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_TermsReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_TermsReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_TermsReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_EnhancedSingleAvail']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_EnhancedSingleAvail = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_EnhancedSingleAvail = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_EnhancedSingleAvail']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_EnhancedSingleAvail = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_EnhancedSingleAvail = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_MultiAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_MultiAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_MultiAvailability = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_MultiAvailability']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_MultiAvailability = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_MultiAvailability = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_EnhancedPricing']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_EnhancedPricing = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_EnhancedPricing = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_EnhancedPricing']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_EnhancedPricing = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_EnhancedPricing = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_CalendarView']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_CalendarView = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_CalendarView = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Hotel_CalendarView']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Hotel_CalendarView = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Hotel_CalendarView = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PNR_AddMultiElements']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PNR_AddMultiElements = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PNR_AddMultiElements = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PNR_Cancel']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PNR_Cancel = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PNR_Cancel = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PNR_Ignore']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PNR_Ignore = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PNR_Ignore = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PNR_IgnoreReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PNR_IgnoreReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PNR_IgnoreReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PNR_List']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PNR_List = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PNR_List = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PNR_Reply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PNR_Reply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PNR_Reply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PNR_Reply1']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PNR_Reply1 = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PNR_Reply1 = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PNR_Retrieve']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PNR_Retrieve = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PNR_Retrieve = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PNR_RetrieveByRecLoc']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PNR_RetrieveByRecLoc = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PNR_RetrieveByRecLoc = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PNR_RetrieveByRecLocReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PNR_RetrieveByRecLocReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PNR_RetrieveByRecLocReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PNR_TransferOwnership']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PNR_TransferOwnership = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PNR_TransferOwnership = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PNR_TransferOwnershipReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PNR_TransferOwnershipReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PNR_TransferOwnershipReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PNR_Split']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PNR_Split = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PNR_Split = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PNR_SplitReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PNR_SplitReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PNR_SplitReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_CreateUpdateProfile']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_CreateUpdateProfile = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_CreateUpdateProfile = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_CreateUpdateProfileReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_CreateUpdateProfileReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_CreateUpdateProfileReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_CreateProfile']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_CreateProfile = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_CreateProfile = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_CreateProfileReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_CreateProfileReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_CreateProfileReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_UpdateProfile']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_UpdateProfile = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_UpdateProfile = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_UpdateProfileReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_UpdateProfileReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_UpdateProfileReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_DeleteProfile']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_DeleteProfile = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_DeleteProfile = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_DeleteProfileReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_DeleteProfileReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_DeleteProfileReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_DeactivateProfile']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_DeactivateProfile = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_DeactivateProfile = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_DeactivateProfileReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_DeactivateProfileReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_DeactivateProfileReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_RetrieveProfile']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_RetrieveProfile = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_RetrieveProfile = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_RetrieveProfileReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_RetrieveProfileReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_RetrieveProfileReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_ProfileReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_ProfileReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_ProfileReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_CreateProfile']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_CreateProfile = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_CreateProfile = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_CreateProfileReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_CreateProfileReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_CreateProfileReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_ReadProfile']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_ReadProfile = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_ReadProfile = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_ReadProfileReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_ReadProfileReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_ReadProfileReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_UpdateProfile']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_UpdateProfile = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_UpdateProfile = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_UpdateProfileReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_UpdateProfileReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_UpdateProfileReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_DeleteProfile']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_DeleteProfile = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_DeleteProfile = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Profile_DeleteProfileReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Profile_DeleteProfileReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Profile_DeleteProfileReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Queue_CountTotal']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Queue_CountTotal = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Queue_CountTotal = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Queue_CountTotalReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Queue_CountTotalReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Queue_CountTotalReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Queue_List']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Queue_List = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Queue_List = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Queue_ListReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Queue_ListReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Queue_ListReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Queue_MoveItem']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Queue_MoveItem = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Queue_MoveItem = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Queue_MoveItemReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Queue_MoveItemReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Queue_MoveItemReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Queue_PlacePNR']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Queue_PlacePNR = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Queue_PlacePNR = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Queue_PlacePNRReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Queue_PlacePNRReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Queue_PlacePNRReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Queue_RemoveItem']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Queue_RemoveItem = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Queue_RemoveItem = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Queue_RemoveItemReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Queue_RemoveItemReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Queue_RemoveItemReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Security_Authenticate']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Security_Authenticate = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Security_Authenticate = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Security_AuthenticateReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Security_AuthenticateReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Security_AuthenticateReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Security_SignOut']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Security_SignOut = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Security_SignOut = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Security_SignOutReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Security_SignOutReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Security_SignOutReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_ATCShopperMasterPricerTravelBoardSearch']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_ATCShopperMasterPricerTravelBoardSearch = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_ATCShopperMasterPricerTravelBoardSearch = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_ATCShopperMasterPricerTravelBoardSearchReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_ATCShopperMasterPricerTravelBoardSearchReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_ATCShopperMasterPricerTravelBoardSearchReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_CancelDocument']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_CancelDocument = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_CancelDocument = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_CancelDocumentReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_CancelDocumentReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_CancelDocumentReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_CheckEligibility']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_CheckEligibility = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_CheckEligibility = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_CheckEligibilityReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_CheckEligibilityReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_CheckEligibilityReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_CreateTSTFromPricing']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_CreateTSTFromPricing = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_CreateTSTFromPricing = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_CreateTSTFromPricingReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_CreateTSTFromPricingReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_CreateTSTFromPricingReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_CreditCardCheck']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_CreditCardCheck = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_CreditCardCheck = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_CreditCardCheckReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_CreditCardCheckReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_CreditCardCheckReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_DeleteTST']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_DeleteTST = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_DeleteTST = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_DeleteTSTReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_DeleteTSTReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_DeleteTSTReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_DisplayTST']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_DisplayTST = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_DisplayTST = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_DisplayTSTReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_DisplayTSTReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_DisplayTSTReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_GetPricingOptions']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_GetPricingOptions = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_GetPricingOptions = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_GetPricingOptionsReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_GetPricingOptionsReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_GetPricingOptionsReply = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_ProcessEDoc']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_ProcessEDoc = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_ProcessEDoc = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_ProcessEDocReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_ProcessEDocReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_ProcessEDocReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_ProcessETicket']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_ProcessETicket = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_ProcessETicket = ""
                                                End If


                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_ProcessETicketReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_ProcessETicketReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_ProcessETicketReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_UpdateTST']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_UpdateTST = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_UpdateTST = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_UpdateTSTReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_UpdateTSTReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_UpdateTSTReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_RepricePNRWithBookingClass']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_RepricePNRWithBookingClass = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_RepricePNRWithBookingClass = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_RepricePNRWithBookingClassReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_RepricePNRWithBookingClassReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_RepricePNRWithBookingClassReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_AutomaticUpdate']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_AutomaticUpdate = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_AutomaticUpdate = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'Ticket_AutomaticUpdateReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.Ticket_AutomaticUpdateReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.Ticket_AutomaticUpdateReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'SalesReports_DisplayQueryReport']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.SalesReports_DisplayQueryReport = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.SalesReports_DisplayQueryReport = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'SalesReports_DisplayQueryReportReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.SalesReports_DisplayQueryReportReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.SalesReports_DisplayQueryReportReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'DocRefund_CalculateRefund']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.DocRefund_CalculateRefund = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.DocRefund_CalculateRefund = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'DocRefund_CalculateRefundReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.DocRefund_CalculateRefundReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.DocRefund_CalculateRefundReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'DocRefund_IgnoreRefund']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.DocRefund_IgnoreRefund = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.DocRefund_IgnoreRefund = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'DocRefund_IgnoreRefundReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.DocRefund_IgnoreRefundReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.DocRefund_IgnoreRefundReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'DocRefund_InitRefund']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.DocRefund_InitRefund = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.DocRefund_InitRefund = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'DocRefund_InitRefundReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.DocRefund_InitRefundReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.DocRefund_InitRefundReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'DocRefund_ProcessRefund']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.DocRefund_ProcessRefund = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.DocRefund_ProcessRefund = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'DocRefund_ProcessRefundReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.DocRefund_ProcessRefundReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.DocRefund_ProcessRefundReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'DocRefund_SearchRefundRule']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.DocRefund_SearchRefundRule = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.DocRefund_SearchRefundRule = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'DocRefund_SearchRefundRuleReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.DocRefund_SearchRefundRuleReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.DocRefund_SearchRefundRuleReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'DocRefund_UpdateRefund']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.DocRefund_UpdateRefund = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.DocRefund_UpdateRefund = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'DocRefund_UpdateRefundReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.DocRefund_UpdateRefundReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.DocRefund_UpdateRefundReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'MiniRule_GetFromPricing']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.MiniRule_GetFromPricing = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.MiniRule_GetFromPricing = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'MiniRule_GetFromPricingReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.MiniRule_GetFromPricingReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.MiniRule_GetFromPricingReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'MiniRule_GetFromPricingRec']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.MiniRule_GetFromPricingRec = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.MiniRule_GetFromPricingRec = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'MiniRule_GetFromPricingRecReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.MiniRule_GetFromPricingRecReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.MiniRule_GetFromPricingRecReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'QueueMode_ProcessQueue']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.QueueMode_ProcessQueue = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.QueueMode_ProcessQueue = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'QueueMode_ProcessQueueReply']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.QueueMode_ProcessQueueReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.QueueMode_ProcessQueueReply = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PAY_GenerateVirtualCard']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PAY_GenerateVirtualCard = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PAY_GenerateVirtualCard = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PAY_ListVirtualCards']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PAY_ListVirtualCards = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PAY_ListVirtualCards = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PAY_VirtualCardDetails']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PAY_VirtualCardDetails = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PAY_VirtualCardDetails = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'PAY_DeleteVirtualCard']")

                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.PAY_DeleteVirtualCard = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.PAY_DeleteVirtualCard = ""
                                                End If

                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'SalesReports_DisplayQueryReport']")
                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.SalesReports_DisplayQueryReport = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.SalesReports_DisplayQueryReport = ""
                                                End If
                                                wsNode = oNodePCC.SelectSingleNode("WSDLSchema/Message[@Name = 'SalesReports_DisplayQueryReportReply']")
                                                If Not wsNode Is Nothing Then
                                                    .AmadeusWSSchema.SalesReports_DisplayQueryReportReply = wsNode.Attributes("Version").Value
                                                Else
                                                    .AmadeusWSSchema.SalesReports_DisplayQueryReportReply = ""
                                                End If

                                            End If
                                        Else
                                            .AmadeusWS = False
                                        End If

                                        'PS).Append(Provider).Append(UserName).Append(System. Sample: PSGalileoElleipsisTest
                                        key = sb.Append("PS").Append(provider).Append(arUsers(i)).Append(.System).Append(.PCC).ToString()
                                        sb.Remove(0, sb.Length())

                                        ' Get Provider Settings
                                        'TODO: This has to be changed attribute AmadeusWS no longer applicable and needs to be removed complitely from and XML files
                                        'It should be always worked with provider Amadeus and not AmadeusWS
                                        If Not oNodePCC.Attributes("AmadeusWS") Is Nothing Then
                                            .AmadeusWS = Convert.ToBoolean(oNodePCC.Attributes("AmadeusWS").Value.ToLower)
                                            If .AmadeusWS = False Then
                                                oNodePrv = oRootPrv.SelectSingleNode($"Provider[@Name='{provider}']/System[@Name='{ .System}']")
                                            Else
                                                oNodePrv = oRootPrv.SelectSingleNode($"Provider[@Name='{provider}WS']/System[@Name='{ .System}']")
                                            End If
                                        Else
                                            oNodePrv = oRootPrv.SelectSingleNode($"Provider[@Name='{provider}']/System[@Name='{ .System}']")
                                        End If

                                        'oNodePrv = oRootPrv.SelectSingleNode(sb.Append("Provider[@Name='").Append(provider).Append("']/System[@Name='").Append(.System).Append("']").ToString())

                                        sb.Remove(0, sb.Length())
                                        If Not oNodePrv Is Nothing Then
                                            .URL = oNodePrv.SelectSingleNode("URL").InnerText
                                            .Port = oNodePrv.SelectSingleNode("Port").InnerText
                                            .SOAPAction = oNodePrv.ParentNode.SelectSingleNode("SOAPAction").InnerText
                                            If Not oNodePrv.SelectSingleNode("SOAP4URL") Is Nothing Then
                                                .SOAP4URL = oNodePrv.SelectSingleNode("SOAP4URL").InnerText
                                            End If
                                        Else
                                            CoreLib.SendTrace("", "modMain", String.Format("Failed to load: {3} - {2} on {1} for {0}", provider, .System, .UserName, .PCC), sb.Append("TripXMLStartUp: Error Loading File ").Append(arFiles(i)).ToString(), String.Empty)
                                        End If


                                        If oRoot.SelectSingleNode("Trace") Is Nothing Then
                                            .Trace = False
                                        Else
                                            .Trace = CBool(oRoot.SelectSingleNode("Trace").InnerText = "Y")
                                        End If

                                        j = 0
                                        For Each oNodeOT In oNodePCC.SelectNodes("OpenType")
                                            ReDim Preserve .OpenTypes(j)
                                            .OpenTypes(j).OfficeID = oNodeOT.Attributes("OfficeID").Value
                                            .OpenTypes(j).Agent = oNodeOT.SelectSingleNode("Agent").InnerText
                                            .OpenTypes(j).SignIn = oNodeOT.SelectSingleNode("SignIn").InnerText
                                            .OpenTypes(j).CountryCode = oNodeOT.SelectSingleNode("CountryCode").InnerText
                                            .OpenTypes(j).CurrencyCode = oNodeOT.SelectSingleNode("CurrencyCode").InnerText
                                            .OpenTypes(j).LanguageCode = oNodeOT.SelectSingleNode("LanguageCode").InnerText
                                            .OpenTypes(j).TravelAgentID = oNodeOT.SelectSingleNode("TravelAgentID").InnerText
                                            j += 1
                                        Next

                                    End With

                                    If provider = "Sabre" Then
                                        ' If Sabre
                                        'CoreLib.SendTrace("", "modMain", sb.Append("Loading User PCC").Append(provider).Append("-").Append(arUsers(i)).Append("-").Append(ttProviderSystems.System).Append("-").Append(ttProviderSystems.PCC).ToString(), "", String.Empty)
                                        sb.Remove(0, sb.Length())
                                        oApplication.Add(key, ttProviderSystems)

                                        For Each oNodeOT In oNodePCC.SelectNodes("OpenType")
                                            'ttSA = New ttSabreAdapter.SabreAdapter(ttProviderSystems)

                                            ' API).Append(UserName).Append(System. Sample: APIElleipsisTest
                                            'Key = "API").Append(arUsers(i)).Append(ttProviderSystems.System).Append(oNodeOT.Attributes("OfficeID").Value
                                            'oApplication.Add(Key, ttSA)

                                            'ttSA = Nothing
                                            key = sb.Append("PS").Append(provider).Append(arUsers(i)).Append(ttProviderSystems.System).Append(oNodeOT.Attributes("OfficeID").Value).ToString()
                                            sb.Remove(0, sb.Length())
                                            oApplication.Add(key, ttProviderSystems)
                                        Next
                                    Else
                                        'CoreLib.SendTrace("", "modMain", sb.Append("Loading User PCC").Append(provider).Append("-").Append(arUsers(i)).Append("-").Append(ttProviderSystems.System).Append("-").Append(ttProviderSystems.PCC).ToString(), "", String.Empty)
                                        sb.Remove(0, sb.Length())
                                        oApplication.Add(key, ttProviderSystems)
                                    End If

                                    'Create the Initial Session Block for each PCC
                                    'If ttProviderSystems.AmadeusWS.ToString.ToLower = "true" And ttProviderSystems.SessionPool = False Then
                                    'ttProviderSystems.SessionPool = True

                                    'If ttProviderSystems.AmadeusWS.ToString.ToLower = "true" And ttProviderSystems.SessionPool Then
                                    '    CreatInitialSessionPool(oApplication, ttProviderSystems, "AmadeusWS")
                                    'End If

                                    'If Provider.ToLower = "sabre" And ttProviderSystems.SessionPool Then
                                    '    CreatInitialSessionPool(oApplication, ttProviderSystems, "Sabre")
                                    'End If

                                    'If Provider.ToLower = "galileo" Or Provider.ToLower = "apollo" And ttProviderSystems.SessionPool Then
                                    '    CreatInitialSessionPool(oApplication, ttProviderSystems, "galileo")
                                    'End If

                                Catch e As Exception
                                    CoreLib.SendTrace("", "modMain", sb.Append("TripXMLStartUp: Error Loading Provider ").Append(key).ToString(), e.Message, String.Empty)
                                    sb.Remove(0, sb.Length())
                                    'CoreLib.SendEmail("Amadeus factory failure", sb.Append("TripXMLStartUp: Error Loading Provider ").Append(Key).ToString())
                                    sb.Remove(0, sb.Length())

                                    'If Not ttAA Is Nothing Then ttAA = Nothing
                                End Try
                            Next    ' oNodePCC in oNode.SelectNodes("PCC")
                        Next    ' oNode In oRoot.SelectNodes("Provider/System")
                        CoreLib.SendTrace("", "modMain", "All Users PCC loaded successfully.", $"Key = {key}", String.Empty)
                    Catch exx As Exception
                        ' Config File Not Valid.
                        CoreLib.SendTrace("", "modMain", sb.Append("TripXMLStartUp: Error Loading File ").Append(arFiles(i)).ToString(), exx.Message, String.Empty)
                        sb.Remove(0, sb.Length())
                    End Try

                Next    ' i = 1 To arUsers.GetUpperBound(0)

                oApplication.UnLock()
                Trace = True   ' TODO Remove this line once TravelTalkUserSettings is working.

            Catch ex As Exception
                Throw New Exception(sb.Append("Error Starting up Application.").Append(ex.Message).ToString())
            Finally
                GC.Collect()
            End Try

        End Sub

#End Region

    End Module
End Namespace