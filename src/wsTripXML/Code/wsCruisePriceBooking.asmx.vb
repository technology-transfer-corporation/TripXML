Imports System.Web.Services
Imports System.Xml
Imports TripXMLMain
Imports System.Xml.Serialization
Imports TripXMLMain.modCore

Namespace wsTravelTalk


    <System.Web.Services.WebService(Namespace:="http://tripxml.downtowntravel.com/tripxml/wsCruisePriceBooking",
        Name:="wsCruisePriceBooking",
        Description:="A TripXML Web Service to Process Cruise Price Booking Messages Request.")>
    Public Class wsCruisePriceBooking
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

        Private Function DecodeCruisePriceBooking(ByVal strResponse As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim ttCruiseLines As DataView
            Dim ttCruiseShips As DataView
            Dim ttCruiseAdvisory As DataView
            Dim ttCruisePricedItems As DataView
            Dim oNode As XmlNode = Nothing
            Dim oGpNode As XmlNode = Nothing

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

                    ' ***************************
                    ' Decode Price Type Code    *
                    ' ***************************
                    For Each oNode In oRoot.SelectNodes("BookingPayment/BookingPrices/BookingPrice")
                        If oNode.InnerText = "" Then
                            oNode.InnerText = GetDecodeValue(ttCruisePricedItems, oNode.Attributes("PriceTypeCode").Value)
                        End If
                    Next
                    For Each oGpNode In oRoot.SelectNodes("BookingPayment/GuestPrices/GuestPrice")
                        For Each oNode In oGpNode.SelectNodes("PriceInfos/PriceInfo")
                            If oNode.InnerText = "" Then
                                oNode.InnerText = GetDecodeValue(ttCruisePricedItems, oNode.Attributes("PriceTypeCode").Value)
                            End If
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
                CoreLib.SendTrace(UserID, "wsCruisePriceBooking", "Error *** Decoding CruisePriceBooking Response", ex.Message, String.Empty)
            End Try
            Return strResponse
        End Function

        Private Function FilterCruisePriceBooking(ByVal strRequest As String, ByVal UserID As String) As String
            Dim oDoc As XmlDocument = Nothing
            Dim oRoot As XmlElement = Nothing
            Dim oNode As XmlNode = Nothing
            Dim oNodeChild As XmlNode = Nothing
            Dim oNodeGrandChild As XmlNode = Nothing
            Dim oNodeGGrandChild As XmlNode = Nothing
            Dim ttCruiseLines As DataView
            Dim ttCruiseMot As DataView
            Dim ttCruiseShips As DataView
            Dim ttCruiseProfiles As DataView
            Dim ttCruiseCities As DataView
            Dim ttCruiseCurrency As DataView
            Dim ttCruiseBedConfiguration As DataView
            Dim ttCruisePaxTitle As DataView
            Dim ttwsCruiseInsurance As DataView
            Dim ttwsCreditCards As DataView
            Dim intQuantity As Integer = 0
            Dim intMaxGuestPerCabin As Integer = 0
            Dim CityFieldLength As Integer
            Dim AddressFieldLength As Integer
            Dim PostalCodeFieldLength As Integer
            Dim FirstNameFieldLength As Integer
            Dim LastNameFieldLength As Integer
            Dim TitleRequired As Boolean
            Dim strName As String = ""
            Dim GuestCitySupported As Boolean
            Dim FareCodeIndicator As Boolean
            Dim FareCode As String = Nothing
            Dim PastGuestIndicator As Boolean
            Dim Language As String = Nothing
            Dim DiningRoom As String = Nothing
            Dim TableSize As String = Nothing
            Dim AgeCode As String = Nothing
            Dim InsuranceCode As String = Nothing

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
                ttCruiseBedConfiguration = CType(Application.Get("ttCruiseBedConfiguration"), DataView)
                ttCruisePaxTitle = CType(Application.Get("ttCruisePaxTitle"), DataView)
                ttwsCruiseInsurance = CType(Application.Get("ttwsCruiseInsurance"), DataView)
                ttwsCreditCards = CType(Application.Get("ttwsCreditCards"), DataView)

                ' Check Cruise Line Code - Vendor Code
                oNode = oRoot.SelectSingleNode("SailingInfo/SelectedSailing")

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

                ' Check Voyage Number
                Select Case mstrVendorCode
                    Case "RCC", "CEL", "ICL"
                        If String.Compare(IsNothing(oNode.Attributes("VoyageID"), ""), CVoyageID) <> 0 Then
                            Throw New Exception(sb.Append("Invalid VoyageID number, it must be ").Append(CVoyageID).Append(".").ToString())
                            sb.Remove(0, sb.Length())
                        End If
                End Select

                ' Get Some Info from the Request to Echo them back on the Response
                mstrDepartureDate = oNode.Attributes("Start").Value
                mstrDuration = oNode.Attributes("Duration").Value

                ' Check Currency Code
                If CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "currencyRequiredFareAvailabilityRequest") = "true"), Boolean) Then
                    If oRoot.SelectSingleNode("SailingInfo/Currency") Is Nothing Then
                        Throw New Exception("Currency Code is mandatory for this Cruise line.")
                    ElseIf IsNothing(oRoot.SelectSingleNode("SailingInfo/Currency").Attributes("CurrencyCode"), "") = "" Then
                        Throw New Exception("Currency Code is mandatory for this Cruise line.")
                    ElseIf Not IsCruiseFilterValue(ttCruiseCurrency, mstrVendorCode, oRoot.SelectSingleNode("SailingInfo/Currency").Attributes("CurrencyCode").Value) Then
                        Throw New Exception(sb.Append("Currency code - ").Append(oRoot.SelectSingleNode("SailingInfo/Currency").Attributes("CurrencyCode").Value).Append(" not supported by this cruise line ").Append(mstrVendorCode).ToString())
                        sb.Remove(0, sb.Length())
                    End If
                End If

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

                FareCodeIndicator = CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "fareCodeAtPassangerLevel") = "false"), Boolean)
                PastGuestIndicator = CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "pastGuestIndicatorSupported") = "false"), Boolean)
                TitleRequired = CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "guestTitleRequired") = "true"), Boolean)
                FirstNameFieldLength = CType(GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "firstNameFieldLength"), Integer)
                LastNameFieldLength = CType(GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "lastNameFieldLength"), Integer)
                AddressFieldLength = CType(GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "streetAddressLength"), Integer)
                CityFieldLength = CType(GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "cityNameLength"), Integer)
                PostalCodeFieldLength = CType(GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "postalCodeLength"), Integer)
                GuestCitySupported = CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "guestCitySupported") = "true"), Boolean)

                ' Check Guest Rules
                For Each oNode In oRoot.SelectNodes("ReservationInfo/GuestDetails/GuestDetail")
                    For Each oNodeChild In oNode.SelectNodes("ContactInfo")
                        For Each oNodeGrandChild In oNodeChild.SelectNodes("GuestTransportation")
                            ' Check MOT
                            If Not IsCruiseFilterValue(ttCruiseMot, mstrVendorCode, oNodeGrandChild.Attributes("TransportationMode").Value) Then
                                Throw New Exception(sb.Append("Invalid Transportation Mode - ").Append(oNodeGrandChild.Attributes("TransportationMode").Value).Append(" for cruise line ").Append(mstrVendorCode).ToString())
                                sb.Remove(0, sb.Length())
                            End If
                            ' Check Gateway
                            If Not IsCruiseFilterValue(ttCruiseCities, mstrVendorCode, oNodeGrandChild.SelectSingleNode("GatewayCity").Attributes("LocationCode").Value) Then
                                Throw New Exception(sb.Append("Gateway location - ").Append(oNodeGrandChild.SelectSingleNode("GatewayCity").Attributes("LocationCode").Value).Append(" not supported by this cruise line ").Append(mstrVendorCode).ToString())
                                sb.Remove(0, sb.Length())
                            End If
                        Next    ' GuestTransportation

                        ' Check PersonName for ContactInfo
                        oNodeGrandChild = oNodeChild.SelectSingleNode("PersonName")
                        ' Check LastName Field Length
                        If oNodeGrandChild.SelectSingleNode("Surname").InnerText.Length > LastNameFieldLength Then
                            Throw New Exception(sb.Append("Last name too long. Maximum length allow by this cruise line is ").Append(LastNameFieldLength).ToString())
                            sb.Remove(0, sb.Length())
                        End If
                        ' Check FirstName Filed Length
                        For Each oNodeGGrandChild In oNodeGrandChild.SelectNodes("GivenName")
                            If oNodeGGrandChild.InnerText.Length > FirstNameFieldLength Then
                                Throw New Exception(sb.Append("First name too long. Maximum length allow by this cruise line is ").Append(FirstNameFieldLength).ToString())
                                sb.Remove(0, sb.Length())
                            End If
                        Next
                        ' Check Title
                        If TitleRequired And oNodeGrandChild.SelectSingleNode("NameTitle") Is Nothing Then
                            Throw New Exception("Passanger title is required by this cruise line.")
                        Else
                            For Each oNodeGGrandChild In oNodeGrandChild.SelectNodes("NameTitle")
                                If Not IsCruiseFilterValue(ttCruisePaxTitle, mstrVendorCode, oNodeGGrandChild.InnerText) Then
                                    Throw New Exception(sb.Append("Passanger title - ").Append(oNodeGGrandChild.InnerText).Append(" not supported by this cruise line ").Append(mstrVendorCode).ToString())
                                    sb.Remove(0, sb.Length())
                                End If
                            Next
                        End If

                        ' Check CityName for Contact Info
                        For Each oNodeGrandChild In oNodeChild.SelectNodes("Address")
                            If oNodeGrandChild.SelectSingleNode("CityName") Is Nothing Then
                                strName = ""
                            Else
                                strName = oNodeGrandChild.SelectSingleNode("CityName").InnerText.Trim
                            End If
                            Select Case CType(IsNothing(oNodeGrandChild.Attributes("UseType"), "0"), Integer)
                                Case 5
                                    If GuestCitySupported Then
                                        If strName.Length = 0 Then
                                            Throw New Exception("Guest city name is mandatory for this cruise line.")
                                        ElseIf Not IsCruiseFilterValue(ttCruiseCities, mstrVendorCode, strName) Then
                                            Throw New Exception(sb.Append("City location - ").Append(strName).Append(" not supported by this cruise line.").ToString())
                                            sb.Remove(0, sb.Length())
                                        End If
                                    End If
                                Case 6, 10
                                    If strName.Length > CityFieldLength Then
                                        Throw New Exception(sb.Append("City name too long. Maximum length allow by this cruise line is ").Append(CityFieldLength).ToString())
                                        sb.Remove(0, sb.Length())
                                    End If
                            End Select

                        Next    ' Address

                    Next ' ContactInfo

                    ' Check PersonName for LinkedTraveler
                    For Each oNodeChild In oNode.SelectNodes("LinkedTraveler")
                        ' Check PersonName
                        oNodeGrandChild = oNodeChild.SelectSingleNode("PersonName")
                        ' Check LastName Field Length
                        If oNodeGrandChild.SelectSingleNode("Surname").InnerText.Length > LastNameFieldLength Then
                            Throw New Exception(sb.Append("Last name too long. Maximum length allow by this cruise line is ").Append(LastNameFieldLength).ToString())
                            sb.Remove(0, sb.Length())
                        End If
                        ' Check FirstName Filed Length
                        For Each oNodeGGrandChild In oNodeGrandChild.SelectNodes("GivenName")
                            If oNodeGGrandChild.InnerText.Length > FirstNameFieldLength Then
                                Throw New Exception(sb.Append("First name too long. Maximum length allow by this cruise line is ").Append(FirstNameFieldLength).ToString())
                                sb.Remove(0, sb.Length())
                            End If
                        Next
                        ' Check Title
                        If TitleRequired And oNodeGrandChild.SelectSingleNode("NameTitle") Is Nothing Then
                            Throw New Exception("Passanger title is required by this cruise line.")
                        Else
                            For Each oNodeGGrandChild In oNodeGrandChild.SelectNodes("NameTitle")
                                If Not IsCruiseFilterValue(ttCruisePaxTitle, mstrVendorCode, oNodeGGrandChild.InnerText) Then
                                    Throw New Exception(sb.Append("Passanger title - ").Append(oNodeGGrandChild.InnerText).Append(" not supported by this cruise line ").Append(mstrVendorCode).ToString())
                                    sb.Remove(0, sb.Length())
                                End If
                            Next
                        End If
                    Next ' LinkedTraveler

                    ' Check FareCode
                    If FareCodeIndicator Then
                        For Each oNodeChild In oNode.SelectNodes("SelectedFareCode")
                            If FareCode Is Nothing Then
                                FareCode = oNodeChild.Attributes("FareCode").Value
                            ElseIf String.Compare(FareCode, oNodeChild.Attributes("FareCode").Value) <> 0 Then
                                Throw New Exception("Fare codes must be identical for all guests for this cruise line.")
                            End If
                        Next
                    End If
                    ' Check Past Passenger
                    If PastGuestIndicator Then
                        For Each oNodeChild In oNode.SelectNodes("LoyaltyInfo")
                            If Not oNodeChild.Attributes("MembershipID") Is Nothing Then
                                Throw New Exception("Past passanger number not supported for this cruise line.")
                            End If
                        Next
                    End If

                    ' Check Dining  -   ReservationInfo/GuestDetails/GuestDetail+/SelectedDining+
                    For Each oNodeChild In oNode.SelectNodes("SelectedDining")
                        ' Check Language
                        If CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "languageOptionSupported") = "false"), Boolean) Then
                            If IsNothing(oNodeChild.Attributes("Language"), "").ToString.Length <> 0 Then
                                Throw New Exception("Dining language selection not supported for this cruise line.")
                            End If
                        End If
                        ' Check DiningRoom
                        If CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "diningRoomOptionSupported") = "false"), Boolean) Then
                            If IsNothing(oNodeChild.Attributes("DiningRoom"), "").ToString.Length <> 0 Then
                                Throw New Exception("Dining room selection not supported for this cruise line.")
                            End If
                        End If
                        ' Check TableSize
                        If CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "diningTableSizeOptionSupported") = "false"), Boolean) Then
                            If IsNothing(oNodeChild.Attributes("TableSize"), "").ToString.Length <> 0 Then
                                Throw New Exception("Table size selection not supported for this cruise line.")
                            End If
                        End If
                        ' Check AgeCode
                        If CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "ageGroupPrefForDiningSupported") = "false"), Boolean) Then
                            If IsNothing(oNodeChild.Attributes("AgeCode"), "").ToString.Length <> 0 Then
                                Throw New Exception("Age group code selection not supported for this cruise line.")
                            End If
                        End If
                        ' Check Dining At Passenger Level
                        If CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "diningAtPassengerLevel") = "false"), Boolean) Then
                            If (Language Is Nothing) Then
                                If (Not oNodeChild.Attributes("Language") Is Nothing) Then
                                    Language = oNodeChild.Attributes("Language").Value
                                End If
                            ElseIf String.Compare(Language, IsNothing(oNodeChild.Attributes("Language"), "")) <> 0 Then
                                Throw New Exception("Dining options must be the same for all guests for this cruise line.")
                            End If
                            If (DiningRoom Is Nothing) Then
                                If (Not oNodeChild.Attributes("DiningRoom") Is Nothing) Then
                                    DiningRoom = oNodeChild.Attributes("DiningRoom").Value
                                End If
                            ElseIf String.Compare(DiningRoom, IsNothing(oNodeChild.Attributes("DiningRoom"), "")) <> 0 Then
                                Throw New Exception("Dining options must be the same for all guests for this cruise line.")
                            End If
                            If (TableSize Is Nothing) Then
                                If (Not oNodeChild.Attributes("TableSize") Is Nothing) Then
                                    TableSize = oNodeChild.Attributes("TableSize").Value
                                End If
                            ElseIf String.Compare(TableSize, IsNothing(oNodeChild.Attributes("TableSize"), "")) <> 0 Then
                                Throw New Exception("Dining options must be the same for all guests for this cruise line.")
                            End If
                            If (AgeCode Is Nothing) Then
                                If (Not oNodeChild.Attributes("AgeCode") Is Nothing) Then
                                    AgeCode = oNodeChild.Attributes("AgeCode").Value
                                End If
                            ElseIf String.Compare(AgeCode, IsNothing(oNodeChild.Attributes("AgeCode"), "")) <> 0 Then
                                Throw New Exception("Dining options must be the same for all guests for this cruise line.")
                            End If
                        End If

                    Next    ' SelectedDining (oNodeChild)

                    ' Check Insurance   -   ReservationInfo/GuestDetails/GuestDetail+/SelectedInsurance
                    If Not oNode.SelectSingleNode("SelectedInsurance") Is Nothing Then
                        oNodeChild = oNode.SelectSingleNode("SelectedInsurance")
                        ' Check Insurance Code
                        If Not oNodeChild.Attributes("InsuranceCode") Is Nothing Then
                            If Not IsDecodeValue(ttwsCruiseInsurance, oNodeChild.Attributes("InsuranceCode").Value) Then
                                Throw New Exception("Invalid insurance code.")
                            ElseIf CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "insuranceAtPassengerLevel") = "false"), Boolean) Then
                                ' Check Insurance Association
                                If InsuranceCode Is Nothing Then
                                    InsuranceCode = oNodeChild.Attributes("InsuranceCode").Value
                                ElseIf String.Compare(InsuranceCode, oNodeChild.Attributes("InsuranceCode").Value) <> 0 Then
                                    Throw New Exception("Insurance options must be the same for all guests for this cruise line.")
                                End If
                            End If
                        End If
                    End If

                Next    ' GuestDetail (oNode)

                ' Check Bed Configuration
                For Each oNode In oRoot.SelectNodes("SailingInfo/SelectedCategory/SelectedCabin")
                    If Not oNode.Attributes("BedConfigurationCode") Is Nothing Then
                        If Not IsDecodeValue(ttCruiseBedConfiguration, oNode.Attributes("BedConfigurationCode").Value) Then
                            Throw New Exception(sb.Append("Invalid Bed Configuration Code - ").Append(oNode.Attributes("BedConfigurationCode").Value).ToString())
                            sb.Remove(0, sb.Length())
                        End If
                    End If
                Next

                ' Check PersonName for LinkedBooking
                For Each oNode In oRoot.SelectNodes("ReservationInfo/LinkedBookings/LinkedBooking")
                    ' Check PersonName
                    oNodeChild = oNode.SelectSingleNode("PersonName")
                    ' Check LastName Field Length
                    If oNodeChild.SelectSingleNode("Surname").InnerText.Length > LastNameFieldLength Then
                        Throw New Exception(sb.Append("Last name too long. Maximum length allow by this cruise line is ").Append(LastNameFieldLength).ToString())
                        sb.Remove(0, sb.Length())
                    End If
                    ' Check FirstName Filed Length
                    For Each oNodeGrandChild In oNodeChild.SelectNodes("GivenName")
                        If oNodeGrandChild.InnerText.Length > FirstNameFieldLength Then
                            Throw New Exception(sb.Append("First name too long. Maximum length allow by this cruise line is ").Append(FirstNameFieldLength).ToString())
                            sb.Remove(0, sb.Length())
                        End If
                    Next
                    ' Check Title
                    If TitleRequired And oNodeChild.SelectSingleNode("NameTitle") Is Nothing Then
                        Throw New Exception("Passanger title is required by this cruise line.")
                    Else
                        For Each oNodeGrandChild In oNodeChild.SelectNodes("NameTitle")
                            If Not IsCruiseFilterValue(ttCruisePaxTitle, mstrVendorCode, oNodeGrandChild.InnerText) Then
                                Throw New Exception(sb.Append("Passanger title - ").Append(oNodeGrandChild.InnerText).Append(" not supported by this cruise line ").Append(mstrVendorCode).ToString())
                                sb.Remove(0, sb.Length())
                            End If
                        Next
                    End If
                Next    ' LinkedBooking (oNode)

                ' Check PaymentRequest Rules
                For Each oNode In oRoot.SelectNodes("ReservationInfo/PaymentRequests/PaymentRequest")
                    ' Check FOP
                    If oNode.SelectSingleNode("PaymentCard") Is Nothing And oNode.SelectSingleNode("DirectBill") Is Nothing Then
                        Throw New Exception("Invalid form of payment.")
                    ElseIf Not oNode.SelectSingleNode("PaymentCard") Is Nothing Then
                        oNodeChild = oNode.SelectSingleNode("PaymentCard")
                        ' Check Credit Card Effective Date
                        If CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "creditCardFromDateList") = "false"), Boolean) _
                            And CType((IsNothing(oNodeChild.Attributes("EffectiveDate"), "").ToString.Length > 0), Boolean) Then
                            Throw New Exception("Credit card effective date not supported by this cruise line.")
                        End If
                        ' Check Credit Card Code
                        If Not oNode.Attributes("CardCode") Is Nothing Then
                            If Not IsDecodeValue(ttwsCreditCards, oNodeChild.Attributes("CardCode").Value) Then
                                Throw New Exception("Credit card vendor code not supported.")
                            End If
                        End If
                        ' Check Address Line Mandatory
                        oNodeGrandChild = oNodeChild.SelectSingleNode("Address")
                        If CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "billAddMandOnCC") = "true"), Boolean) _
                            And oNodeGrandChild Is Nothing Then
                            Throw New Exception("Billing address is mandatory for this cruise line.")
                        End If
                        ' Check Payment Address Line Length
                        If Not oNodeGrandChild.SelectSingleNode("AddressLine") Is Nothing Then
                            If oNodeGrandChild.SelectSingleNode("AddressLine").InnerText.Length > AddressFieldLength Then
                                Throw New Exception(sb.Append("Address line too long. Maximum length allow by this cruise line is ").Append(AddressFieldLength).ToString())
                                sb.Remove(0, sb.Length())
                            End If
                        End If
                        ' Check Payment City Length
                        If Not oNodeGrandChild.SelectSingleNode("CityName") Is Nothing Then
                            If oNodeGrandChild.SelectSingleNode("CityName").InnerText.Length > CityFieldLength Then
                                Throw New Exception(sb.Append("City name too long. Maximum length allow by this cruise line is ").Append(CityFieldLength).ToString())
                                sb.Remove(0, sb.Length())
                            End If
                        End If
                        ' Check Payment Postal Code Length
                        If Not oNodeGrandChild.SelectSingleNode("PostalCode") Is Nothing Then
                            If oNodeGrandChild.SelectSingleNode("PostalCode").InnerText.Length > PostalCodeFieldLength Then
                                Throw New Exception(sb.Append("Postal code too long. Maximum length allow by this cruise line is ").Append(PostalCodeFieldLength).ToString())
                                sb.Remove(0, sb.Length())
                            End If
                        End If
                    ElseIf Not oNode.SelectSingleNode("DirectBill") Is Nothing Then
                        oNodeChild = oNode.SelectSingleNode("DirectBill")
                        If IsNothing(oNodeChild.Attributes("DirectBill_ID"), "").ToString.Length = 0 Then
                            Throw New Exception("Invalid form of payment.")
                        ElseIf IsNothing(oNode.Attributes("ReferenceNumber"), "").ToString.Length = 0 Then
                            ' Check Payment Reference
                            Throw New Exception("Payment reference number is mandatory with this form of payment.")
                        End If
                    End If

                    ' Check Extended Payment
                    If CType((GetCruiseFilterValue(ttCruiseProfiles, mstrVendorCode, "extendedPaymentSupported") = "false"), Boolean) _
                        And CType((IsNothing(oNode.Attributes("ExtendedIndicator"), "") = "true"), Boolean) Then
                        Throw New Exception("Extended payment not supported by this cruise line.")
                    End If

                    ' Check Payment Currency
                    If Not oNode.SelectSingleNode("PaymentAmount") Is Nothing Then
                        If Not oNode.SelectSingleNode("PaymentAmount").Attributes("CurrencyCode") Is Nothing Then
                            If Not IsCruiseFilterValue(ttCruiseCurrency, mstrVendorCode, oNode.SelectSingleNode("PaymentAmount").Attributes("CurrencyCode").Value) Then
                                Throw New Exception(sb.Append("Currency code - ").Append(oNode.SelectSingleNode("PaymentAmount").Attributes("CurrencyCode").Value).Append(" not supported by this cruise line.").ToString())
                                sb.Remove(0, sb.Length())
                            End If
                        End If
                    End If

                Next    ' PaymentRequest (oNode)

            Catch ex As Exception
                CoreLib.SendTrace(UserID, "wsCruisePriceBooking", "Error *** Filtering CruisePriceBooking Request", ex.Message, String.Empty)
                Throw ex
            End Try
            sb.Remove(0, sb.Length())
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

                ' Validate Rules for CruisePriceBooking
                strRequest = FilterCruisePriceBooking(strRequest, ttCredential.UserID)

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

                strResponse = DecodeCruisePriceBooking(strResponse, ttCredential.UserID)

                PostServiceRequest(strResponse, ValidateXSDOut, ttServiceID, ttCredential.UserID)

            Catch ex As Exception
                strResponse = FormatErrorMessage(ttServiceID, ex.Message, ttCredential.Providers(0).Name)
            Finally
                LogResponse(strResponse, ttCredential, StartTime, ttServiceID, Server.MachineName, UUID)
                If Trace Then CoreLib.SendTrace(ttCredential.UserID, "wsCruiseBooking", "============= OTA Response ============= ", strResponse, UUID)
            End Try
            sb = Nothing
            Return strResponse

        End Function

#End Region

#Region " Web Methods "

        <WebMethod(Description:="Process Cruise Price Booking Messages Request.")>
        Public Function wmCruisePriceBooking(ByVal OTA_CruisePriceBookingRQ As wmCruisePriceBookingIn.OTA_CruisePriceBookingRQ) As <XmlElementAttribute("OTA_CruisePriceBookingRS")> wmCruisePriceBookingOut.OTA_CruisePriceBookingRS
            Dim xmlMessage As String = ""
            Dim oCruisePriceBookingRS As wmCruisePriceBookingOut.OTA_CruisePriceBookingRS = Nothing
            Dim oSerializer As XmlSerializer = Nothing
            Dim oWriter As System.IO.StringWriter = Nothing
            Dim oReader As System.IO.StringReader = Nothing

            oSerializer = New XmlSerializer(GetType(wmCruisePriceBookingIn.OTA_CruisePriceBookingRQ))
            oWriter = New System.IO.StringWriter(New System.Text.StringBuilder)
            oSerializer.Serialize(oWriter, OTA_CruisePriceBookingRQ)
            xmlMessage = oWriter.ToString
            xmlMessage = xmlMessage.Replace(" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""", "")

            xmlMessage = ServiceRequest(xmlMessage, ttServices.CruisePriceBooking)

            Try
                oSerializer = Nothing
                oSerializer = New XmlSerializer(type:=GetType(wmCruisePriceBookingOut.OTA_CruisePriceBookingRS))
                oReader = New System.IO.StringReader(xmlMessage)
                oCruisePriceBookingRS = CType(oSerializer.Deserialize(oReader), wmCruisePriceBookingOut.OTA_CruisePriceBookingRS)
            Catch ex As Exception
                CoreLib.SendTrace("", "wsCruiseBooking", "Error Deserialing OTA Response", ex.Message, String.Empty)
            End Try

            Return oCruisePriceBookingRS

        End Function

        <WebMethod(Description:="Process Cruise Price Booking Xml Messages Request.")> _
        Public Function wmCruisePriceBookingXml(ByVal xmlRequest As String) As String
            Return ServiceRequest(xmlRequest, ttServices.CruisePriceBooking)
        End Function

#End Region

    End Class

End Namespace
