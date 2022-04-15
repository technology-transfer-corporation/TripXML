Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain
Imports System.Xml.Serialization
Imports TripXMLMain.modCore

Namespace wsTravelTalk


    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsCruiseCabinUnhold",
        Name:="wsCruiseCabinUnhold",
        Description:="A TripXML Web Service to Process Cruise Cabin Unhold Messages Request.")>
    Public Class wsCruiseCabinUnUnhold
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

        Private Function DecodeCruiseCabinUnhold(ByVal strResponse As String, ByVal UserID As String) As String
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

                    If oNode.SelectSingleNode("SelectedCabin").Attributes("CabinNumber").Value = "" Then
                        oNode.SelectSingleNode("SelectedCabin").Attributes("CabinNumber").Value = mstrCabinNo
                    End If

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
                CoreLib.SendTrace(UserID, "wsCruiseCabinUnhold", "Error *** Decoding CruiseCabinUnhold Response", ex.Message, String.Empty)
            End Try
            Return strResponse
        End Function

        Private Function FilterCruiseCabinUnhold(ByVal strRequest As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing
            Dim oNodeGt As XmlNode = Nothing
            Dim ttCruiseLines As DataView
            Dim ttCruiseShips As DataView

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strRequest)
                oRoot = oDoc.DocumentElement

                ttCruiseLines = CType(Application.Get("ttCruiseLines"), DataView)
                ttCruiseShips = CType(Application.Get("ttCruiseShips"), DataView)

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
                mstrCabinNo = oNode.SelectSingleNode("SelectedCabin").Attributes("CabinNumber").Value

                ' Check Voyage Number
                Select Case mstrVendorCode
                    Case "RCC", "CEL", "ICL"
                        If String.Compare(IsNothing(oNode.Attributes("VoyageID"), ""), CVoyageID) <> 0 Then
                            Throw New Exception(sb.Append("Invalid VoyageID number, it must be ").Append(CVoyageID).Append(".").ToString())
                            sb.Remove(0, sb.Length())
                        End If
                End Select

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsCruiseCabinUnhold", "Error *** Filtering CruiseCabinUnhold Request", ex.Message, String.Empty)
                Throw ex
            End Try
            Return strRequest
            sb = Nothing
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

                ' Validate Rules for CruiseCabinUnhold
                strRequest = FilterCruiseCabinUnhold(strRequest, ttCredential.UserID)

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

                    Case "apollo", "galileo"

                        strResponse = SendCruiseRequestGalileo(ttServiceID, ttCredential, ttProviderSystems, strRequest)

                    Case Else
                        Throw New Exception(sb.Append("Provider ").Append(ttCredential.Providers(0).Name).Append(" Not Currently Supported.").ToString())
                        sb.Remove(0, sb.Length())
                End Select

                strResponse = DecodeCruiseCabinUnhold(strResponse, ttCredential.UserID)

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsCruiseCabinUnhold", "============= OTA Response ============= ", strResponse, UUID)
            End Try

            Return strResponse
            sb = Nothing
        End Function

#End Region

#Region " Web Methods "

        <WebMethod(Description:="Process Cruise Cabin Unhold Messages Request.")>
        Public Function wmCruiseCabinUnhold(ByVal OTA_CruiseCabinUnholdRQ As wmCruiseCabinUnholdIn.OTA_CruiseCabinUnholdRQ) As <XmlElementAttribute("OTA_CruiseCabinUnholdRS")> wmCruiseCabinUnholdOut.OTA_CruiseCabinUnholdRS
            Dim xmlMessage As String = ""
            Dim oCruiseCabinUnholdRS As wmCruiseCabinUnholdOut.OTA_CruiseCabinUnholdRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmCruiseCabinUnholdIn.OTA_CruiseCabinUnholdRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_CruiseCabinUnholdRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CruiseCabinUnhold)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmCruiseCabinUnholdOut.OTA_CruiseCabinUnholdRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oCruiseCabinUnholdRS = CType(oSerializer.Deserialize(oReader), wmCruiseCabinUnholdOut.OTA_CruiseCabinUnholdRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsCruiseCabinUnhold", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oCruiseCabinUnholdRS

        End Function

        <WebMethod(Description:="Process Cruise Cabin Unhold Xml Messages Request.")> _
        Public Function wmCruiseCabinUnholdXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.CruiseCabinUnhold)
        End Function

#End Region

    End Class

End Namespace
