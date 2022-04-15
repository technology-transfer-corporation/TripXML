Imports TripXMLMain
Imports System.Xml
Imports TripXMLMain.modCore

Namespace wsTravelTalk

    Public Class cServiceWorldspan

        Public Event GotResponse(ByVal Response As String)

#Region " Properties "
        Private sb As StringBuilder = New StringBuilder()
        Private ttProviderSystems As TripXMLProviderSystems

        Public Property ServiceID() As Integer

        Public Property Request() As String = ""

        Public Property Version() As String = ""

        Public Property ProviderSystems() As TripXMLProviderSystems
            Get
                Return ttProviderSystems
            End Get
            Set(ByVal Value As TripXMLProviderSystems)
                ttProviderSystems = Value
            End Set
        End Property

        Public Property ttCities() As DataView

#End Region

        Public Sub SendAirRequest()
            Dim strResponse As String = ""
            Dim ttService As Worldspan.AirServices = Nothing
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing
            Dim strMsg As String = ""

            Try
                ttService = New Worldspan.AirServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = Request

                    Select Case ServiceID
                        Case ttServices.LowFare
                            strResponse = .LowFare(ttCities)
                        Case ttServices.LowFarePlus
                            strResponse = .LowFarePlus(ttCities)
                        Case Else
                            Throw New Exception("Invalid request or message not supported by Worldspan air services.")
                    End Select
                    If ttProviderSystems.PCC.Length > 0 Then
                        strResponse = strResponse.Replace("TransactionIdentifier=""Worldspan", sb.Append("TransactionIdentifier=""Worldspan").Append("-").Append(ttProviderSystems.PCC).ToString())
                        sb.Remove(0, sb.Length())
                    End If

                    If .ProviderSystems.BLFile <> "" Then
                        oDoc = New XmlDocument
                        ' Load Access Control List into memory
                        Try
                            oDoc.Load(.ProviderSystems.BLFile)
                        Catch exr As Exception
                            CoreLib.SendTrace("", "cServiceWorldspan", "Error Loading business logic file", exr.Message, ttProviderSystems.LogUUID)
                            Throw exr
                        End Try

                        oRoot = oDoc.DocumentElement
                        oNode = oRoot.SelectSingleNode(sb.Append("Message[@Name='").Append(strMsg).Append("'][@Direction='Out']").ToString())
                        sb.Remove(0, sb.Length())

                        If Not oNode Is Nothing Then
                            oNode = oNode.SelectSingleNode(sb.Append("ProviderBL[@Name='Worldspan'][@System='").Append(.ProviderSystems.System).Append("'][@PCC='").Append(.ProviderSystems.PCC.ToUpper).Append("']").ToString())
                            sb.Remove(0, sb.Length())

                            If Not oNode Is Nothing Then
                                strResponse = BusinessLogic(strResponse, oNode.OuterXml, sb.Append(XslPath).Append("BL\").ToString(), strMsg)
                                sb.Remove(0, sb.Length())
                            End If
                        End If
                    End If
                End With

            Catch ex As Exception
                strResponse = FormatErrorMessage(ServiceID, ex.Message, "Worldspan", "", False, Version)
            Finally
                If Not ttService Is Nothing Then ttService = Nothing
                RaiseEvent GotResponse(strResponse)
            End Try
            sb = Nothing
        End Sub

        Public Sub SendPNRRequest()
            Dim strResponse As String = ""
            Dim ttService As Worldspan.PNRServices = Nothing

            Try
                ttService = New Worldspan.PNRServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = Request

                    Select Case ServiceID
                        Case ttServices.PNRRead
                            strResponse = .PNRRead()
                        Case ttServices.PNRCancel
                            strResponse = .PNRCancel()
                        Case Else
                            Throw New Exception("Invalid request or message not supported by Worldspan PNR services.")
                    End Select

                End With

            Catch ex As Exception
                strResponse = FormatErrorMessage(ServiceID, ex.Message, "Worldspan", "", False, Version)
            Finally
                If Not ttService Is Nothing Then ttService = Nothing
                RaiseEvent GotResponse(strResponse)
            End Try

        End Sub

        Public Sub SendTravelRequest()
            Dim strResponse As String = ""
            Dim ttService As Worldspan.TravelServices = Nothing

            Try
                ttService = New Worldspan.TravelServices

                With ttService
                    .Version = Version
                    .XslPath = XslPath
                    .ProviderSystems = ttProviderSystems
                    .Request = Request

                    Select Case ServiceID
                        Case ttServices.TravelBuild
                            strResponse = .TravelBuild()
                        Case Else
                            Throw New Exception("Invalid request or message not supported by Worldspan Travel services.")
                    End Select

                End With

            Catch ex As Exception
                strResponse = FormatErrorMessage(ServiceID, ex.Message, "Worldspan", "", False, Version)
            Finally
                If Not ttService Is Nothing Then ttService = Nothing
                RaiseEvent GotResponse(strResponse)
            End Try

        End Sub

        Public Function BusinessLogic(ByVal strResponse As String, ByVal strBusiness As String, ByVal xslPath As String, ByVal strMsg As String) As String

            If strResponse.IndexOf("<Success />") <> -1 Or strResponse.IndexOf("<Success></Success>") Then
                strResponse = strResponse.Replace("<Success />", sb.Append(strBusiness).Append("<Success />").ToString())
                sb.Remove(0, sb.Length())
                strResponse = strResponse.Replace("<Success></Success>", sb.Append(strBusiness).Append("<Success></Success>").ToString())
                sb.Remove(0, sb.Length())
                CoreLib.SendTrace("", "cServiceWorldspan", sb.Append("Before ").Append(strMsg).Append(" business logic").ToString(), strResponse, ttProviderSystems.LogUUID)
                sb.Remove(0, sb.Length())
                strResponse = CoreLib.TransformXML(strResponse, xslPath, sb.Append(Version).Append("BL_").Append(strMsg).Append("RS.xsl").ToString())
                sb.Remove(0, sb.Length())
            End If
            sb = Nothing
            Return strResponse
        End Function

    End Class

End Namespace
