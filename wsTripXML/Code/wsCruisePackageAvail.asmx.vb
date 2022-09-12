Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml.Serialization

Namespace wsTravelTalk


    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsCruisePackageAvail",
        Name:="wsCruisePackageAvail",
        Description:="A TripXML Web Service to Process Cruise Package Availibility Messages Request.")>
    Public Class wsCruisePackageAvail
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

        Private mstrVoyageID As String = ""
        Private mstrShipCode As String = ""
        Private mstrDepartureDate As String = ""
        Private mstrDuration As String = ""

        Private Function DecodeCruisePackageAvail(ByVal strResponse As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim ttCruiseLines As DataView
            Dim ttCruiseShips As DataView
            Dim ttCruiseAdvisory As DataView
            Dim ttCruisePricedItems As DataView
            Dim oNode As XmlNode = Nothing
            Dim oNodeChild As XmlNode = Nothing

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strResponse)
                oRoot = oDoc.DocumentElement

                ttCruiseAdvisory = CType(Application.Get("ttCruiseAdvisory"), DataView)

                If oRoot.SelectSingleNode("Errors") Is Nothing Then

                    ttCruiseLines = CType(Application.Get("ttCruiseLines"), DataView)
                    ttCruiseShips = CType(Application.Get("ttCruiseShips"), DataView)
                    ttCruisePricedItems = CType(Application.Get("ttCruisePricedItems"), DataView)

                    oNode = oRoot.SelectSingleNode("SailingInfo/SelectedSailing")
                    ' VoyageID, Departure Date and Duration
                    If oNode.Attributes("VoyageID").Value = "" Then
                        oNode.Attributes("VoyageID").Value = mstrVoyageID
                    End If
                    If oNode.Attributes("Start").Value = "" Then
                        oNode.Attributes("Start").Value = mstrDepartureDate
                    End If
                    If oNode.Attributes("Duration").Value = "" Then
                        oNode.Attributes("Duration").Value = mstrDuration
                    End If
                    ' *******************************
                    ' Decode CruiseLines & Ships    *
                    ' *******************************
                    oNode.Attributes("VendorName").Value = GetDecodeValue(ttCruiseLines, oNode.Attributes("VendorCode").Value)
                    If oNode.Attributes("ShipCode").Value = "" Then
                        oNode.Attributes("ShipCode").Value = mstrShipCode
                    End If
                    oNode.Attributes("ShipName").Value = GetCruiseFilterValue(ttCruiseShips, oNode.Attributes("VendorCode").Value, oNode.Attributes("ShipCode").Value)

                    ' *******************************
                    ' Decode Advisory Errors Codes  *
                    ' *******************************
                    For Each oNode In oRoot.SelectNodes("Warnings/Warning")
                        If oNode.InnerText.Length = 0 Then
                            oNode.InnerText = GetDecodeValue(ttCruiseAdvisory, oNode.Attributes("Code").Value)
                        End If
                    Next

                    ' ***************************
                    ' Decode Price Type Code    *
                    ' ***************************
                    For Each oNode In oRoot.SelectNodes("PackageOptions/PackageOption")
                        For Each oNodeChild In oNode.SelectNodes("PackagePrices/PackagePrice")
                            oNodeChild.Attributes("CodeDetail").Value = GetDecodeValue(ttCruisePricedItems, oNodeChild.Attributes("PriceTypeCode").Value)
                        Next
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
                CoreLib.SendTrace(UserID, "wsCruisePackageAvail", "Error *** Decoding CruisePackageAvail Response", ex.Message, String.Empty)
            End Try
            Return strResponse
        End Function

        Private Function FilterCruisePackageAvail(ByVal strRequest As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing
            Dim oNodeGt As XmlNode = Nothing

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strRequest)
                oRoot = oDoc.DocumentElement

                oNode = oRoot.SelectSingleNode("SailingInfo/SelectedSailing")

                ' Get Some Info from the Request to Echo them back on the Response
                mstrVoyageID = oNode.Attributes("VoyageID").Value
                mstrShipCode = IsNothing(oNode.Attributes("ShipCode"), "")
                mstrDepartureDate = oNode.Attributes("Start").Value
                mstrDuration = oNode.Attributes("Duration").Value

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsCruisePackageAvail", "Error *** Filtering CruisePackageAvail Request", ex.Message, String.Empty)
                Throw ex
            End Try
            Return strRequest
        End Function

#End Region

#Region " Process Service Request All GDS "
        Private sb As StringBuilder = New StringBuilder()

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

                ' Validate Rules for CruisePackageAvail
                strRequest = FilterCruisePackageAvail(strRequest, ttCredential.UserID)

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

                strResponse = DecodeCruisePackageAvail(strResponse, ttCredential.UserID)

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsPackageAvail", "============= OTA Response ============= ", strResponse, UUID)
            End Try

            Return strResponse
            sb = Nothing
        End Function

#End Region

#Region " Web Methods "

        <WebMethod(Description:="Process Cruise Package Availability Messages Request.")>
        Public Function wmCruisePackageAvail(ByVal OTA_CruisePackageAvailRQ As wmCruisePackageAvailIn.OTA_CruisePackageAvailRQ) As <XmlElementAttribute("OTA_CruisePackageAvailRS")> wmCruisePackageAvailOut.OTA_CruisePackageAvailRS
            Dim xmlMessage As String = ""
            Dim oCruisePackageAvailRS As wmCruisePackageAvailOut.OTA_CruisePackageAvailRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmCruisePackageAvailIn.OTA_CruisePackageAvailRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_CruisePackageAvailRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CruisePackageAvail)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmCruisePackageAvailOut.OTA_CruisePackageAvailRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oCruisePackageAvailRS = CType(oSerializer.Deserialize(oReader), wmCruisePackageAvailOut.OTA_CruisePackageAvailRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsPackageAvail", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oCruisePackageAvailRS

        End Function

        <WebMethod(Description:="Process Cruise Package Availibility Xml Messages Request.")> _
        Public Function wmCruisePackageAvailXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.CruisePackageAvail)
        End Function

#End Region

    End Class

End Namespace
