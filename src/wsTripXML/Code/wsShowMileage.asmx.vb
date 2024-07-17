Imports System.Web.Services
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml.Serialization
Imports wsTripXML.wsTravelTalk.wmShowMileageOut
Imports System.Linq
Imports TripXMLTools

Namespace wsTravelTalk

    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsShowMileage",
        Name:="wsShowMileage",
        Description:="A TripXML Web Service to Process Show Mileage Messages Request.")>
    Public Class wsShowMileage
        Inherits System.Web.Services.WebService

#Region " Web Services Designer Generated Code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Web Services Designer.
            InitializeComponent()

            'Add your own initialization code after the InitializeComponent() call

        End Sub

        'Required by the Web Services Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Web Services Designer
        'It can be modified using the Web Services Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            components = New System.ComponentModel.Container
        End Sub

        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            'CODEGEN: This procedure is required by the Web Services Designer
            'Do not modify it using the code editor.
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

#End Region

#Region " Decode Function "
        Enum enMile
            FromCity = 0
            ToCity = 1
            DepTime = 2
            ArrTime = 3
            Meals = 4
            EQP = 5
            ELPD = 6
            ACCUM = 7
            MILES = 8
            SM = 9
        End Enum

        ''' <summary>
        ''' Get Code of Airline based on OPERATED BY line
        ''' </summary>
        ''' <param name="encodedLine">Line that containes OPERATED BY</param>
        ''' <param name="UserID"></param>
        ''' <param name="UUID"></param>
        ''' <returns></returns>
        Private Function DecodePNRRead(ByVal encodedLine As String) As String
            Dim responseCode As String
            Try
                If encodedLine Is Nothing Then
                    responseCode = encodedLine
                Else
                    responseCode = TripXMLLoad.EncodeValue(TripXMLLoad.DecodingType.Airline, encodedLine)
                End If
            Catch ex As Exception
                'CoreLib.SendTrace(UserID, "wsPNRRead", "Error *** Decoding AirAvail Response", ex.Message, UUID)
                responseCode = encodedLine
            End Try
            Return responseCode
        End Function
        ''' <summary>
        ''' Clean up string fopr Decoding. Removing unneeded information from string in order to maximize probobility of the correct result.
        ''' </summary>
        ''' <param name="encodedLine"></param>
        ''' <returns>Airline Code. EX: TK</returns>
        Private Function CleanUpString(remark As String) As String
            '*DTW-CMH OPERATED BY DL/REPUBLIC AIRWAYS DELTA CONNECTION FOR DELTA AIR LINES.
            '*YUL-MCO OPERATED BY /AIR CANADA ROUGE.
            '*TSA-KNH OPERATED BY /EVA AIRWAYS FOR UNI AIRWAYS.
            '*ATL-SLC OPERATED BY DELTA AIR LINES FOR LATAM AIRLINES GROUP.
            '*FLL-ATL OPERATED BY DELTA AIR LINES INC.
            '*MCO-FRA OPERATED BY EW DISCOVER GMBH.
            Try
                Dim index As Integer = remark.IndexOf("OPERATED BY")
                Dim endIndex As Integer = remark.IndexOf("/")
                Dim _remark = remark.Substring(index).Replace(".", "")

                If remark.Contains("/") Then
                    Dim _reg As New Regex("\s([A-Z]){2}/")
                    If _reg.IsMatch(_remark) Then
                        '*DTW-CMH OPERATED BY DL/REPUBLIC AIRWAYS DELTA CONNECTION FOR DELTA AIR LINES.
                        'In this case correct answer is DL
                        _remark = remark.Substring(index, endIndex - index)
                    Else
                        '*YUL-MCO OPERATED BY /AIR CANADA ROUGE.
                        'In this correct answer is AIR CANADA ROUGE
                        _remark = remark.Substring(endIndex + 1)
                    End If

                Else
                    '*MCO-FRA OPERATED BY EW DISCOVER GMBH.
                    'In this case correct answer is EW
                    Dim _code As String = remark.Substring(index + 12).Trim
                    Dim _reg As New Regex("^([A-Z]){2}\s")

                    If _reg.IsMatch(_remark.Replace("OPERATED BY ", "")) Then
                        _remark = _code.Substring(0, 2)
                        Return _remark
                    End If

                    If _remark.Contains(" AS ") Then
                        Dim elem As List(Of String) = _remark.Replace("OPERATED BY ", "").Split({" AS "}, StringSplitOptions.None).ToList()
                        _remark = elem(0).Trim
                    End If

                End If

                Return _remark
            Catch ex As Exception
                Return remark
            End Try

        End Function

#End Region

#Region " Process Service Request All GDS "
        Private sb As StringBuilder = New StringBuilder()

        Private Function ServiceRequest(ByVal request As String, ByVal ttServiceID As Integer) As String
            Dim response As String = ""
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim ValidateXSDOut As Boolean
            Dim StartTime As Date
            Dim UUID As String = ""

            Try
                StartTime = Now

                PreServiceRequest(request, Application, ttCredential, ttProviderSystems, StartTime, ttServiceID, Server.MachineName, UUID)
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())
                If request.Contains("OTA_ShowMilesRQ") Then
                    'All comunicatioon for this purpose will be gone through Sabe
                    Dim ttDefProvider As New TripXMLProviderSystems()
                    Dim aaapcc As String = ttDefProvider.AAAPCC
                    Dim _pcc As String = ttCredential.Providers(0).PCC

                    ttDefProvider.AAAPCC = ttCredential.Providers(0).PCC
                    ttCredential.Providers(0).PCC = "A5C6"

                    PreServiceRequest(request, Application, ttCredential, ttDefProvider, StartTime, ttServiceID, Server.MachineName, UUID, "", True)
                    response = SendOtherRequestSabre(ttServiceID, ttCredential, ttDefProvider, request)
                    ttCredential.Providers(0).PCC = _pcc
                Else
                    Select Case ttCredential.Providers(0).Name
                        Case "AmadeusWS"
                            response = SendOtherRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, request)
                        Case "Apollo", "Galileo"
                            response = SendOtherRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, request)
                        Case "Sabre"
                            If ttProviderSystems.System Is Nothing Then
                                FormatErrorMessage(ttServiceID, sb.Append("Access denied to ").Append(ttCredential.Providers(0).Name).Append(" - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString(), ttCredential.Providers(0).Name)
                                sb.Remove(0, sb.Length())
                                Exit Select
                            End If
                            ttProviderSystems.AAAPCC = ttCredential.Providers(0).PCC
                            response = SendOtherRequestSabre(ttServiceID, ttCredential, ttProviderSystems, request)
                        Case Else
                            Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                            sb.Remove(0, sb.Length())
                    End Select
                End If


                ' DecodeShowMileage(strResponse) Not Implemented.

                PostServiceRequest(response, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                response = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(response, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsShowMileage", "============= OTA Response ============= ", response, UUID)
            End Try
            sb = Nothing
            Return response

        End Function

#End Region

#Region " Web Methods "

        <WebMethod(Description:="Process Show Mileage Messages Request.")>
        Public Function wmShowMileage(ByVal OTA_ShowMileageRQ As wmShowMileageIn.OTA_ShowMileageRQ) As <XmlElementAttribute("OTA_ShowMileageRS")> wmShowMileageOut.OTA_ShowMileageRS
            Dim oShowMileageRS As wmShowMileageOut.OTA_ShowMileageRS = Nothing

            Dim oSerializer As XmlSerializer = New XmlSerializer(GetType(wmShowMileageIn.OTA_ShowMileageRQ))
            Dim oWriter As IO.StringWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_ShowMileageRQ)
            Dim xmlMessage As String = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.ShowMileage)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmShowMileageOut.OTA_ShowMileageRS))
                Dim oReader As IO.StringReader = New System.IO.StringReader(xmlMessage)
                oShowMileageRS = CType(oSerializer.Deserialize(oReader), wmShowMileageOut.OTA_ShowMileageRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsShowMileage", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oShowMileageRS

        End Function

        <WebMethod(Description:="Process Show Universal Mileage Messages Request  regarding GDS.")>
        Public Function wmShowMiles(ByVal OTA_ShowMilesRQ As wmShowMileageIn.OTA_ShowMilesRQ) As <XmlElementAttribute("OTA_ShowMileageRS")> wmShowMileageOut.OTA_ShowMileageRS
            Dim oShowMileageRS As wmShowMileageOut.OTA_ShowMileageRS = Nothing

            Dim oSerializer As XmlSerializer = New XmlSerializer(GetType(wmShowMileageIn.OTA_ShowMilesRQ))
            Dim oWriter As IO.StringWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_ShowMilesRQ)
            Dim xmlMessage As String = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.ShowMileage)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmShowMileageOut.OTA_ShowMileageRS))
                Dim oReader As IO.StringReader = New System.IO.StringReader(xmlMessage)
                oShowMileageRS = CType(oSerializer.Deserialize(oReader), wmShowMileageOut.OTA_ShowMileageRS)
                FillMilesInformation(oShowMileageRS, OTA_ShowMilesRQ)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsShowMileage", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oShowMileageRS

        End Function

        Private Sub FillMilesInformation(ByRef response As OTA_ShowMileageRS, request As wmShowMileageIn.OTA_ShowMilesRQ)
            Try
                'DPTR ARVL               MEALS EQP ELPD ACCUM MILES SM
                'JFK IST 100P 650A\u00871 M    333 9.50  9.50  5000 N
                'Error: ChrW(135) & "INVALID FLT"

                If response.Errors?.ToList().Exists(Function(e) e.Value?.Contains("INVALID FLT")) Then
                    response.OperatingCarrier = "INVALID FLT"
                    Return
                End If

                Dim elems As List(Of String) = response.Remarks.Remark(1).Split(" ").ToList
                response.FromCity = elems(enMile.FromCity)
                response.ToCity = New ToCity With {.Value = elems(enMile.ToCity), .Mileage = elems(enMile.MILES), .AccumulativeMileage = elems(enMile.ACCUM)}
                response.TotalMileage = elems(enMile.MILES)

                If response.Remarks.Remark.ToList.Exists(Function(l) l.Contains(" OPERATED BY ")) Then
                    Dim remark As String = response.Remarks.Remark.ToList.Find(Function(l) l.Contains(" OPERATED BY ")) '.Split("OPERATED BY", StringSplitOptions.RemoveEmptyEntries).ToList
                    If Not String.IsNullOrEmpty(remark) Then
                        response.OperatingCarrier = GetOperatingCarrier(remark)
                    Else
                        response.OperatingCarrier = request.CarrierCode
                    End If
                Else
                    response.OperatingCarrier = request.CarrierCode
                End If

            Catch ex As Exception
                response.Errors = {New [Error] With {.Value = ex.Message}}
            End Try
        End Sub

        Private Function GetOperatingCarrier(remark As String) As String
            remark = CleanUpString(remark)
            Dim _code As String = DecodePNRRead(remark)
            If _code.Length.Equals(2) Then
                Return _code
            End If
            Return String.Empty
        End Function

        <WebMethod(Description:="Process Show Mileage Xml Messages Request.")>
        Public Function wmShowMileageXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.ShowMileage)
        End Function

        <WebMethod(Description:="Process Universal Mileage Xml Messages Request.")>
        Public Function wmShowMilesXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.ShowMileage)
        End Function

#End Region


    End Class

End Namespace
