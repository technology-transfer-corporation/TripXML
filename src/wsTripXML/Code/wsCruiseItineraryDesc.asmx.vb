Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml.Serialization

Namespace wsTravelTalk


    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsCruiseItineraryDesc",
        Name:="wsCruiseItineraryDesc",
        Description:="A TripXML Web Service to Process Cruise Itinerary Description Messages Request.")>
    Public Class wsCruiseItineraryDesc
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
        Private sb As StringBuilder = New StringBuilder()

        Private mstrVendorCode As String = ""
        Private mstrShipCode As String = ""
        Private mstrDepartureDate As String = ""
        Private mstrDuration As String = ""
        Private mstrCabinNo As String = ""

        Private Function DecodeCruiseItineraryDesc(ByVal strResponse As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim ttCruiseLines As DataView
            Dim ttCruiseShips As DataView
            Dim ttCruiseAdvisory As DataView
            Dim oNode As XmlNode = Nothing

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strResponse)
                oRoot = oDoc.DocumentElement

                ttCruiseAdvisory = CType(Application.Get("ttCruiseAdvisory"), DataView)

                If oRoot.SelectSingleNode("Errors") Is Nothing Then

                    ttCruiseLines = CType(Application.Get("ttCruiseLines"), DataView)
                    ttCruiseShips = CType(Application.Get("ttCruiseShips"), DataView)

                    oNode = oRoot.SelectSingleNode("SelectedSailing")

                    ' Departure Date and Duration
                    If oNode.Attributes("Start").Value = "" Then
                        oNode.Attributes("Start").Value = mstrDepartureDate
                    End If
                    If oNode.Attributes("Duration").Value = "" Then
                        oNode.Attributes("Duration").Value = mstrDuration
                    End If
                    ' *******************************
                    ' Decode CruiseLines & Ships    *
                    ' *******************************
                    If oNode.Attributes("VendorCode").Value = "" Then
                        oNode.Attributes("VendorCode").Value = mstrVendorCode
                    End If
                    If oNode.Attributes("ShipCode").Value = "" Then
                        oNode.Attributes("ShipCode").Value = mstrShipCode
                    End If
                    oNode.Attributes("VendorName").Value = GetDecodeValue(ttCruiseLines, oNode.Attributes("VendorCode").Value)
                    oNode.Attributes("ShipName").Value = GetCruiseFilterValue(ttCruiseShips, oNode.Attributes("VendorCode").Value, oNode.Attributes("ShipCode").Value)

                    ' *******************************
                    ' Decode Advisory Errors Codes  *
                    ' *******************************
                    For Each oNode In oRoot.SelectNodes("Warnings/Warning")
                        If oNode.InnerText.Length = 0 Then
                            oNode.InnerText = GetDecodeValue(ttCruiseAdvisory, oNode.Attributes("Code").Value)
                        End If
                    Next
                Else
                    ' *******************************
                    ' Decode Advisory Errors Codes  *
                    ' *******************************
                    For Each oNode In oRoot.SelectNodes("Errors/Error")
                        If oNode.InnerText.Length = 0 Then
                            oNode.InnerText = GetDecodeValue(ttCruiseAdvisory, oNode.Attributes("Code").Value)
                        End If
                    Next
                End If

                strResponse = oDoc.OuterXml

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsCruiseItineraryDesc", "Error *** Decoding CruiseItineraryDesc Response", ex.Message, String.Empty)
            End Try
            Return strResponse
        End Function

        Private Function FilterCruiseItineraryDesc(ByVal strRequest As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing
            Dim oNodeGt As XmlNode = Nothing
            Dim ttCruiseLines As DataView
            Dim ttCruiseMot As DataView
            Dim ttCruiseShips As DataView
            Dim ttCruiseProfiles As DataView
            Dim ttCruiseCities As DataView
            Dim ttCruiseCurrency As DataView
            Dim ttCruiseCabinFilter As DataView
            Dim intQuantity As Integer = 0
            Dim intMaxGuestPerCabin As Integer = 0

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strRequest)
                oRoot = oDoc.DocumentElement

                ttCruiseLines = CType(Application.Get("ttCruiseLines"), DataView)
                ttCruiseMot = CType(Application.Get("ttCruiseMot"), DataView)
                ttCruiseShips = CType(Application.Get("ttCruiseShips"), DataView)
                ttCruiseProfiles = CType(Application.Get("ttCruiseProfiles"), DataView)
                ttCruiseCities = CType(Application.Get("ttCruiseCities"), DataView)
                ttCruiseCurrency = CType(Application.Get("ttCruiseCurrency"), DataView)
                ttCruiseCabinFilter = CType(Application.Get("ttCruiseCabinFilter"), DataView)

                If oRoot.SelectNodes("SelectedSailing").Count > 1 Then
                    Throw New Exception("Multiple SelectedSailing not supported by Amadeus.")
                End If

                ' Check Cruise Line Code - Vendor Code
                oNode = oRoot.SelectSingleNode("SelectedSailing")

                If oNode.Attributes("VendorCode") Is Nothing Then
                    Throw New Exception("Invalid request. Cruise line vendor code is mandatory for this message.")
                End If

                mstrVendorCode = oNode.Attributes("VendorCode").Value
                If Not IsDecodeValue(ttCruiseLines, mstrVendorCode) Then
                    Throw New Exception(sb.Append("Invalid Cruise Line Code - ").Append(mstrVendorCode).ToString())
                    sb.Remove(0, sb.Length())
                End If

                ' Check ShipCode
                mstrShipCode = IsNothing(oNode.Attributes("ShipCode"), "")
                If mstrShipCode.Length > 0 Then
                    If Not IsCruiseFilterValue(ttCruiseShips, mstrVendorCode, mstrShipCode) Then
                        Throw New Exception(sb.Append("Invalid Ship code - ").Append(mstrShipCode).Append(" for cruise line ").Append(mstrVendorCode).ToString())
                        sb.Remove(0, sb.Length())
                    End If
                Else
                    Throw New Exception("Invalid request. Ship code is mandatory for this message.")
                End If

                ' Get Some Info from the Request to Echo them back on the Response
                mstrDepartureDate = oNode.Attributes("Start").Value
                mstrDuration = oNode.Attributes("Duration").Value
                mstrCabinNo = oNode.SelectSingleNode("SelectedCategory/SelectedCabin").Attributes("CabinNumber").Value

                ' Check Currency Code
                If CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "currencyRequiredFareAvailabilityRequest") = "true"), Boolean) Then
                    If oRoot.SelectSingleNode("Currency") Is Nothing Then
                        Throw New Exception("Currency Code is mandatory for this Cruise line.")
                    ElseIf IsNothing(oRoot.SelectSingleNode("Currency").Attributes("CurrencyCode"), "") = "" Then
                        Throw New Exception("Currency Code is mandatory for this Cruise line.")
                    ElseIf Not IsCruiseFilterValue(ttCruiseCurrency, mstrVendorCode, oRoot.SelectSingleNode("Currency").Attributes("CurrencyCode").Value) Then
                        Throw New Exception(sb.Append("Currency code - ").Append(oRoot.SelectSingleNode("Currency").Attributes("CurrencyCode").Value).Append(" not supported by this cruise line ").Append(mstrVendorCode).ToString())
                        sb.Remove(0, sb.Length())
                    End If
                End If

                ' Check Voyage Number
                Select Case mstrVendorCode
                    Case "RCC", "CEL", "ICL"
                        If String.Compare(IsNothing(oNode.Attributes("VoyageID"), ""), CVoyageID) <> 0 Then
                            Throw New Exception(sb.Append("Invalid VoyageID number, it must be ").Append(CVoyageID).Append(".").ToString())
                            sb.Remove(0, sb.Length())
                        End If
                End Select

                ' Get maxGuestPerCabin
                intMaxGuestPerCabin = CType(GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "maxGuestPerCabin"), Integer)
                ' Check for Number In Party
                intQuantity = 0
                For Each oNode In oRoot.SelectNodes("GuestCounts/GuestCount")
                    intQuantity += CType(IsNothing(oNode.Attributes("Quantity"), 0), Integer)
                Next
                If intQuantity > intMaxGuestPerCabin Then
                    Throw New Exception("Maximum number of guest per cabin exceeded for specified cruise line.")
                End If

                ' Check MOT
                For Each oNode In oRoot.SelectNodes("Guest")
                    For Each oNodeGt In oNode.SelectNodes("GuestTransportation")
                        If Not IsCruiseFilterValue(ttCruiseMot, mstrVendorCode, oNodeGt.Attributes("TransportationMode").Value) Then
                            Throw New Exception(sb.Append("Invalid Transportation Mode - ").Append(oNodeGt.Attributes("TransportationMode").Value).Append(" for cruise line ").Append(mstrVendorCode).ToString())
                            sb.Remove(0, sb.Length())
                        End If
                        If Not IsCruiseFilterValue(ttCruiseCities, mstrVendorCode, oNodeGt.SelectSingleNode("GatewayCity").Attributes("LocationCode").Value) Then
                            Throw New Exception(sb.Append("Gateway location - ").Append(oNodeGt.SelectSingleNode("GatewayCity").Attributes("LocationCode").Value).Append(" not supported by this cruise line ").Append(mstrVendorCode).ToString())
                            sb.Remove(0, sb.Length())
                        End If
                    Next
                Next

                ' Check Cabin Filters
                For Each oNode In oRoot.SelectNodes("SelectedCategory/CabinFilters/CabinFilter")
                    If Not oNode.Attributes("CabinFilterCode") Is Nothing Then
                        If Not IsCruiseFilterValue(ttCruiseCabinFilter, mstrVendorCode, oNode.Attributes("CabinFilterCode").Value) Then
                            Throw New Exception(sb.Append("Cabin filter - ").Append(oNode.Attributes("CabinFilterCode").Value).Append(" not supported by this cruise line ").Append(mstrVendorCode).ToString())
                            sb.Remove(0, sb.Length())
                        End If
                    End If
                Next

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsCruiseItineraryDesc", "Error *** Filtering CruiseItineraryDesc Request", ex.Message, String.Empty)
                Throw ex
            End Try
            sb = Nothing
            Return strRequest
        End Function

#End Region

#Region " Process Service Request All GDS "

        Private Function ServiceRequest(ByVal strRequest As String, ByVal ttServiceID As Integer) As String
            Dim strResponse As String = ""
            Dim ttCredential As TravelTalkCredential = Nothing
            Dim ttProviderSystems As TripXMLProviderSystems = Nothing
            Dim ValidateXSDOut As Boolean
            Dim StartTime As Date
            Dim UUID As String = ""

            Try
                StartTime = Now

                PreServiceRequest(strRequest, Application, ttCredential, ttProviderSystems, StartTime, ttServiceID, Server.MachineName, UUID)
                ValidateXSDOut = Application.Get(sb.Append("XSD").Append(ttCredential.UserID).Append("Out").ToString())
                sb.Remove(0, sb.Length())

                ' Validate Rules for CruiseItineraryDesc
                'strRequest = FilterCruiseItineraryDesc(strRequest, ttCredential.UserID)

                Select Case ttCredential.Providers(0).Name.ToLower
                    Case "amadeus"
                        'Dim ttAA As AmadeusAPIAdapter

                        'ttAA = Application.Get(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).Append(ttCredential.Providers(0).PCC).ToString())
                        'sb.Remove(0, sb.Length())
                        'If ttAA Is Nothing Then
                        '    Throw New Exception(sb.Append("Access denied to Amadeus - ").Append(ttCredential.System).Append(" system. Or invalid provider.").ToString())
                        '    sb.Remove(0, sb.Length())
                        'End If

                        'If ttCredential.Providers(0).PCC.Trim.Length > 0 Then
                        '    ttAA.SourcePCC = ttCredential.Providers(0).PCC
                        'End If

                        ''Send Reuest
                        'strResponse = SendCruiseRequestAmadeus(ttServiceID, ttCredential, ttAA, strRequest)
                        'Application.Set(sb.Append("API").Append(ttCredential.UserID).Append(ttCredential.System).ToString(), ttAA)
                        'sb.Remove(0, sb.Length())

                    Case "amadeusws"

                        strResponse = SendCruiseRequestAmadeusWS(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                        'Case "apollo", "galileo"

                        '    strResponse = SendCruiseRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                strResponse = DecodeCruiseItineraryDesc(strResponse, ttCredential.UserID)

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsCruiseItineraryDesc", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <WebMethod(Description:="Process Cruise Itinerary Description Messages Request.")>
        Public Function wmCruiseItineraryDesc(ByVal OTA_CruiseItineraryDescRQ As wmCruiseItineraryDescIn.OTA_CruiseItineraryDescRQ) As <XmlElementAttribute("OTA_CruiseItineraryDescRS")> wmCruiseItineraryDescOut.OTA_CruiseItineraryDescRS
            Dim xmlMessage As String = ""
            Dim oCruiseItineraryDescRS As wmCruiseItineraryDescOut.OTA_CruiseItineraryDescRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmCruiseItineraryDescIn.OTA_CruiseItineraryDescRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_CruiseItineraryDescRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CruiseItineraryDesc)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmCruiseItineraryDescOut.OTA_CruiseItineraryDescRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oCruiseItineraryDescRS = CType(oSerializer.Deserialize(oReader), wmCruiseItineraryDescOut.OTA_CruiseItineraryDescRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsCruiseItineraryDesc", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oCruiseItineraryDescRS

        End Function

        <WebMethod(Description:="Process Cruise Itinerary Description Xml Messages Request.")> _
        Public Function wmCruiseItineraryDescXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.CruiseItineraryDesc)
        End Function

#End Region

    End Class

End Namespace
