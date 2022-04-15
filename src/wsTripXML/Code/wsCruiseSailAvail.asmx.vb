Imports System
Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain.modCore
Imports TripXMLMain
Imports System.Xml.Serialization
Imports System.Data
Imports System.Text

Namespace wsTravelTalk


    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsCruiseSailAvail", _
        Name:="wsCruiseSailAvail", _
        Description:="A TripXML Web Service to Process Cruise Sail Availibility Messages Request.")> _
    Public Class wsCruiseSailAvail
        Inherits System.Web.Services.WebService
        Private sb As StringBuilder = New StringBuilder()

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

        Private Function DecodeCruiseSailAvail(ByVal strResponse As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim ttwsCruiseCities As DataView
            Dim ttCruiseRegions As DataView
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

                    ttwsCruiseCities = CType(Application.Get("ttwsCruiseCities"), DataView)
                    ttCruiseRegions = CType(Application.Get("ttCruiseRegions"), DataView)
                    ttCruiseLines = CType(Application.Get("ttCruiseLines"), DataView)
                    ttCruiseShips = CType(Application.Get("ttCruiseShips"), DataView)

                    For Each oNode In oRoot.SelectNodes("SailingOptions/SailingOption")
                        ' *******************************
                        ' Decode CruiseLines & Ships    *
                        ' *******************************
                        oNode.SelectSingleNode("CruiseLine").Attributes("VendorName").Value = GetDecodeValue(ttCruiseLines, oNode.SelectSingleNode("CruiseLine").Attributes("VendorCode").Value)
                        oNode.SelectSingleNode("CruiseLine").Attributes("ShipName").Value = GetCruiseFilterValue(ttCruiseShips, oNode.SelectSingleNode("CruiseLine").Attributes("VendorCode").Value, oNode.SelectSingleNode("CruiseLine").Attributes("ShipCode").Value)

                        ' *******************
                        ' Decode Regions    *
                        ' *******************
                        If Not oNode.SelectSingleNode("Region") Is Nothing Then
                            oNode.SelectSingleNode("Region").Attributes("RegionName").Value = GetDecodeValue(ttCruiseRegions, oNode.SelectSingleNode("Region").Attributes("RegionCode").Value)
                        End If

                        ' ***********************
                        ' Decode CruiseCities   *
                        ' ***********************
                        If Not oNode.SelectSingleNode("DeparturePort") Is Nothing Then
                            oNode.SelectSingleNode("DeparturePort").InnerText = GetDecodeValue(ttwsCruiseCities, oNode.SelectSingleNode("DeparturePort").Attributes("LocationCode").Value)
                        End If
                        If Not oNode.SelectSingleNode("ArrivalPort") Is Nothing Then
                            oNode.SelectSingleNode("ArrivalPort").InnerText = GetDecodeValue(ttwsCruiseCities, oNode.SelectSingleNode("ArrivalPort").Attributes("LocationCode").Value)
                        End If

                    Next

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
                CoreLib.SendTrace(UserID, "wsCruiseSailAvail", "Error *** Decoding CruiseSailAvail Response", ex.Message, String.Empty)
            End Try
            Return strResponse
        End Function

        Private Function FilterCruiseSailAvail(ByVal strRequest As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing
            Dim ttCruiseLines As DataView
            Dim ttCruiseRegions As DataView
            Dim ttCruiseShips As DataView
            Dim ttCruiseProfiles As DataView
            Dim intCruiseLines As Integer
            Dim VendorCode As String = ""
            Dim ShipCode As String = ""
            Dim intQuantity As Integer = 0
            Dim intMaxGuestPerCabin As Integer = 0
            Dim intdefaultSailingDuration As Integer
            Dim AddDuration As Boolean
            Dim oAttr As XmlAttribute
            Dim GroupIndicator As String = ""
            Dim InclusiveFilter As String = ""

            Try

                oDoc = New XmlDocument
                oDoc.LoadXml(strRequest)
                oRoot = oDoc.DocumentElement

                ttCruiseLines = CType(Application.Get("ttCruiseLines"), DataView)
                ttCruiseRegions = CType(Application.Get("ttCruiseRegions"), DataView)
                ttCruiseShips = CType(Application.Get("ttCruiseShips"), DataView)
                ttCruiseProfiles = CType(Application.Get("ttCruiseProfiles"), DataView)

                If oRoot.SelectSingleNode("CruiseLinePrefs") Is Nothing Then
                    Throw New Exception("Invalid request. Cruise Line Preferences is mandatory.")
                End If

                intCruiseLines = oRoot.SelectNodes("CruiseLinePrefs/CruiseLinePref").Count

                If intCruiseLines > 5 Then
                    Throw New Exception("Invalid request. Maximum of 5 preferred cruise lines allowed.")
                End If

                ' Check SailingDuration
                If oRoot.SelectSingleNode("SailingDateRange").Attributes("Duration") Is Nothing Then
                    intdefaultSailingDuration = 0
                    oAttr = oDoc.CreateAttribute("Duration")
                    oAttr.Value = intdefaultSailingDuration
                    oRoot.SelectSingleNode("SailingDateRange").Attributes.Append(oAttr)
                Else
                    intdefaultSailingDuration = oRoot.SelectSingleNode("SailingDateRange").Attributes("Duration").Value
                End If
                AddDuration = (intdefaultSailingDuration = 0)

                ' Get GroupIndicator
                If oRoot.SelectSingleNode("GuestCounts") Is Nothing Then
                    GroupIndicator = ""
                Else
                    GroupIndicator = IsNothing(oRoot.SelectSingleNode("GuestCounts").Attributes("GroupIndicator"), "").ToString
                End If

                If intCruiseLines = 1 Then
                    ' Check Cruise Line Code - Vendor Code
                    oNode = oRoot.SelectSingleNode("CruiseLinePrefs/CruiseLinePref")

                    If oNode.Attributes("VendorCode") Is Nothing Then
                        Throw New Exception("Invalid request. Cruise line vendor code is mandatory.")
                    End If
                    VendorCode = oNode.Attributes("VendorCode").Value
                    If Not IsDecodeValue(ttCruiseLines, VendorCode) Then
                        Throw New Exception(sb.Append("Invalid Cruise Line Code - ").Append(VendorCode).ToString())
                        sb.Remove(0, sb.Length())
                    Else
                        ' Check ShipCode for Single Cruise Line
                        ShipCode = IsNothing(oNode.Attributes("ShipCode"), "")
                        ' Get inclusiveFilteringSupportedForSailAvl
                        If oNode.SelectSingleNode("InclusivePackageOption") Is Nothing Then
                            InclusiveFilter = ""
                        Else
                            InclusiveFilter = IsNothing(oNode.SelectSingleNode("InclusivePackageOption").Attributes("InclusiveIndicator"), "")
                        End If
                        If ShipCode.Length > 0 Then
                            If Not IsCruiseFilterValue(ttCruiseShips, VendorCode, ShipCode) Then
                                Throw New Exception(sb.Append("Invalid Ship Code - ").Append(ShipCode).Append(" for cruise line ").Append(VendorCode).ToString())
                                sb.Remove(0, sb.Length())
                            End If
                        Else
                            'Check Region
                            If oRoot.SelectNodes("RegionPref").Count = 0 Then
                                Throw New Exception("Invalid request. Region code is mandatory when ship code not specified.")
                            End If
                            For Each oNode In oRoot.SelectNodes("RegionPref")
                                If Not IsDecodeValue(ttCruiseRegions, oNode.Attributes("RegionCode").Value) Then
                                    Throw New Exception(sb.Append("Invalid Region Code - ").Append(oNode.Attributes("RegionCode").Value).ToString())
                                    sb.Remove(0, sb.Length())
                                End If
                            Next
                        End If
                        ' Get defaultSailingDuration
                        If AddDuration Then
                            intdefaultSailingDuration = CType(GetCruiseFilterValue(ttCruiseProfiles, VendorCode, "defaultSailingDuration"), Integer)
                        End If
                        ' Get maxGuestPerCabin
                        intMaxGuestPerCabin = CType(GetCruiseFilterValue(ttCruiseProfiles, VendorCode, "maxGuestPerCabin"), Integer)
                        ' Check GroupIndicator
                        If GroupIndicator.Length > 0 And GetCruiseFilterValue(ttCruiseProfiles, VendorCode, "groupIndicatorOnSailAvlSupported").ToLower = "false" Then
                            Throw New Exception("Group indicator on sailing availability not supported for this cruise line.")
                        End If
                        ' Check inclusiveFilteringSupportedForSailAvl
                        If InclusiveFilter.Length > 0 And GetCruiseFilterValue(ttCruiseProfiles, VendorCode, "inclusiveFilteringSupportedForSailAvl").ToLower = "false" Then
                            Throw New Exception("Inclusive package indicator on sailing availability not supported for this cruise line.")
                        End If
                    End If
                Else
                    ' Check GroupIndicator
                    If GroupIndicator.Length > 0 Then
                        Throw New Exception("GroupIndicator not allowed if multiple cruise lines selected in the request.")
                    End If
                    For Each oNode In oRoot.SelectNodes("CruiseLinePrefs/CruiseLinePref")
                        ' Check InclusiveFilter
                        If oNode.SelectSingleNode("InclusivePackageOption") Is Nothing Then
                            InclusiveFilter = ""
                        Else
                            InclusiveFilter = IsNothing(oNode.SelectSingleNode("InclusivePackageOption").Attributes("InclusiveIndicator"), "")
                        End If
                        If InclusiveFilter.ToString.Length > 0 Then
                            Throw New Exception("GroupIndicator not allowed if multiple cruise lines selected in the request.")
                        End If
                        ' Check Cruise Line Code
                        If oNode.Attributes("VendorCode") Is Nothing Then
                            Throw New Exception("Invalid request. Cruise line vendor code is mandatory.")
                        End If
                        VendorCode = IsNothing(oNode.Attributes("VendorCode"), "")
                        If Not IsDecodeValue(ttCruiseLines, VendorCode) Then
                            Throw New Exception(sb.Append("Invalid Cruise Line Code - ").Append(VendorCode).ToString())
                            sb.Remove(0, sb.Length())
                        Else
                            ' Check ShipCode
                            ShipCode = IsNothing(oNode.Attributes("ShipCode"), "")
                            If ShipCode.Length > 0 Then
                                Throw New Exception("Ship code not allow with more than one preferred cruise line selection.")
                            End If
                            ' Get defaultSailingDuration. The Max of all Cruise
                            If AddDuration Then
                                intQuantity = CType(GetCruiseFilterValue(ttCruiseProfiles, VendorCode, "defaultSailingDuration"), Integer)
                                If intQuantity > intdefaultSailingDuration Then
                                    intdefaultSailingDuration = intQuantity
                                End If
                            End If
                            ' Get maxGuestPerCabin. The Max of all Cruise
                            intQuantity = CType(GetCruiseFilterValue(ttCruiseProfiles, VendorCode, "maxGuestPerCabin"), Integer)
                            If intQuantity > intMaxGuestPerCabin Then
                                intMaxGuestPerCabin = intQuantity
                            End If
                        End If
                    Next
                    'Check Region
                    If oRoot.SelectNodes("RegionPref").Count = 0 Then
                        Throw New Exception("Invalid request. Region code is mandatory when ship code not specified.")
                    End If
                    For Each oNode In oRoot.SelectSingleNode("RegionPref")
                        If Not IsDecodeValue(ttCruiseRegions, oNode.Attributes("RegionCode").Value) Then
                            Throw New Exception(sb.Append("Invalid Region Code - ").Append(oNode.Attributes("RegionCode").Value).ToString())
                            sb.Remove(0, sb.Length())
                        End If
                    Next
                End If

                ' Check for Number In Party
                intQuantity = 0
                For Each oNode In oRoot.SelectNodes("GuestCounts/GuestCount")
                    intQuantity += CType(IsNothing(oNode.Attributes("Quantity"), 0), Integer)
                Next
                If intQuantity > intMaxGuestPerCabin Then
                    Throw New Exception("Maximum number of guest per cabin exceeded for specified cruise line.")
                End If

                If AddDuration Then
                    oRoot.SelectSingleNode("SailingDateRange").Attributes("Duration").Value = intdefaultSailingDuration
                    strRequest = oDoc.OuterXml
                End If

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsCruiseSailAvail", "Error *** Filtering CruiseSailAvail Request", ex.Message, String.Empty)
                Throw ex
            End Try
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

                ' Validate Rules for CruiseSailAvail
                strRequest = FilterCruiseSailAvail(strRequest, ttCredential.UserID)

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

                strResponse = DecodeCruiseSailAvail(strResponse, ttCredential.UserID)

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsCruiseSailAvail", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <WebMethod(Description:="Process Cruise Sail Availability Messages Request.")> _
        Public Function wmCruiseSailAvail(ByVal OTA_CruiseSailAvailRQ As wmCruiseSailAvailIn.OTA_CruiseSailAvailRQ) As <XmlElementAttribute("OTA_CruiseSailAvailRS")> wmCruiseSailAvailOut.OTA_CruiseSailAvailRS
            Dim xmlMessage As String = ""
            Dim oCruiseSailAvailRS As wmCruiseSailAvailOut.OTA_CruiseSailAvailRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmCruiseSailAvailIn.OTA_CruiseSailAvailRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_CruiseSailAvailRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")
            xmlMessage = xmlMessage.Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CruiseSailAvail)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(Type:=GetType(wmCruiseSailAvailOut.OTA_CruiseSailAvailRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oCruiseSailAvailRS = CType(oSerializer.Deserialize(oReader), wmCruiseSailAvailOut.OTA_CruiseSailAvailRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsCruiseSailServices", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oCruiseSailAvailRS

        End Function

        <WebMethod(Description:="Process Cruise Sail Availibility Xml Messages Request.")> _
        Public Function wmCruiseSailAvailXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.CruiseSailAvail)
        End Function

#End Region

    End Class

End Namespace
