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
                ttCredential = TripXMLTools.TripXMLLoad.GetTravelTalkCredential(strRequest, ttServiceID)

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

                TripXMLTools.TripXMLLoad.GetProviderSystem(ttProviderSystems, ttCredential)

                Try
                    If validateXSDIn Then
                        CoreLib.ValidateXML(strRequest, ttServiceID, enSchemaType.Request, ttCredential.UserID, Version)
                    End If
                Catch exx As Exception
                    Throw New Exception($"Invalid Request. Schema Validation Failed.{vbNewLine}{exx.Message}")
                End Try

                'AuthenticateUser(oDoc, ttCredential)
                'ttProviderSystems = oApp.Get(sb.Append("PS").Append(ttCredential.Providers(0).Name).Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())

                'ttProviderSystems = oApp.Get($"PS{ttCredential.Providers(0).Name}{ttCredential.UserID}{ttCredential.System}{ttCredential.Providers.First().PCC}")

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
                            If (Not ttProviderSystems.AmadeusWS) Or (ttProviderSystems.AmadeusWS And ttProviderSystems.AmadeusWSSchema("Air_FlightInfo") <> "") Then
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

        Public Sub TripXMLStartUp(ByRef oApplication As HttpApplicationState)
            Dim sb As StringBuilder = New StringBuilder()

            Try
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

                Dim start = DateTime.Now

                TripXMLTools.TripXMLLoad.TripXMLLoadObject()
                TripXMLTools.TripXMLLoad.GetDecodingTables()

                Dim loadTime = DateTime.Now - start

                CoreLib.SendTrace("", "TripXMLLoad", $"TripXML was loaded in {String.Format("{0:0.##}", loadTime.TotalSeconds)} seconds", "", "")

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