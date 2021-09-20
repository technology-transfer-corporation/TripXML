Imports System.Reflection
Imports System.Text
Imports System.Web.Configuration
Imports System.Xml
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Module modCore

    Public gXslPath As String = ""
    Public XslPath As String = ""
    Public LogPath As String = ""
    Public SchemaPath As String = ""
    Public Trace As Boolean
    Public IsCreating As Boolean = False
    Public NonDirectFlights As Boolean = False
    Public LFSchRequestCount As Integer = 0

#Region " Structures "

    Public Structure ACLFile
        Dim ServerName As String
    End Structure

    Public Structure Provider
        Dim Name As String
        Dim PCC As String
    End Structure

    Public Structure TravelTalkCredential
        Public Shared RequestorID As String
        Dim Providers() As Provider
        Dim System As String
        Dim UserID As String
        Dim Password As String
    End Structure

    Public Structure TripXMLProviderSystems
        Dim UserID As String
        Dim System As String
        Dim URL As String

        Dim Port As String
        Dim PCC As String
        Dim AAAPCC As String
        Dim AmadeusTrace As Boolean
        Dim UserName As String
        Dim Password As String
        Dim Profile As String
        Dim ProfileXML As String
        Dim ProfileCryptic As String
        Dim ProfileTicketing As String
        Dim Origin As String

        Dim Trace As Boolean
        Dim OpenTypes() As OpenType
        Dim BLFile As String
        Dim AggFilter As Boolean
        Dim RebookNextFlight As Boolean
        Dim RebookPassive As Boolean
        Dim FareMessage As String
        Dim SaveInDB As String
        Dim LogNative As Boolean
        Dim AmadeusWS As Boolean
        Dim AmadeusWSSchema As AmadeusWSSchema
        Dim ProviderSession As ProviderSession
        Dim GPass As String
        Dim GReqID As String
        Dim SessionPool As Boolean
        Dim Provider As String
        '<Obsolete("SOAP2 Should be used through enum SoapHeader")>_
        Dim SOAP2 As Boolean
        '<Obsolete("SOAP4 Should be used through enum SoapHeader")>_
        Dim SOAP4 As Boolean
        Dim SOAP4URL As String
        Dim SOAPAction As String
        Dim SoapHeader As enSOAPHeaderType

        Dim GetStoredFares As Boolean
        Dim AddLog As Boolean
        Dim CheckBookedFare As Boolean
        Dim NoOfLowFareFlights As String
        'belo given variables are not in local code
        Dim HotelMedia As Boolean
        Dim SendEmailToAgency As Boolean
        Dim CreateInRHAdmin As Boolean
        Dim LFPLight As Boolean
        Dim CouponStatus As Boolean
        Dim AddLFPStat As Boolean
        Dim ProxyURL As String
        Dim HotelVersion As String
        Dim LogUUID As String
        Dim SourceMarket As String
        'Dim EmployeeProfileVersion As String
        Dim CarVersion As String
        Dim MiniRules As Boolean
        Dim AirPriceVersion As String
        Dim SeatMapVersion As String
        Dim PriceBookingVersion As String
        Dim ShowTestHotels As String
        Dim Language As String
        Dim SabreFareSearch As Boolean
        Dim AdVShop As String
    End Structure

    Public Structure ProviderSession
        Dim MaximumCount As Integer
        Dim InitialBlockSize As Integer
        Dim NextBlockSize As Integer
        Dim SessionsUsed As Integer
        Dim MultipleAccess As Boolean
        'Dim MultipleCount As Integer
    End Structure

    Public Structure OpenType
        Dim OfficeID As String
        Dim Agent As String
        Dim SignIn As String
        Dim CountryCode As String
        Dim CurrencyCode As String
        Dim LanguageCode As String
        Dim TravelAgentID As String
    End Structure

    Public Structure AmadeusWSSchema
        Dim Air_FlightInfo As String
        Dim Air_FlightInfoReply As String
        Dim Air_MultiAvailability As String
        Dim Air_MultiAvailabilityReply As String
        Dim Air_RebookAirSegment As String
        Dim Air_RebookAirSegmentReply As String
        Dim Air_RetrieveSeatMap As String
        Dim Air_RetrieveSeatMapReply As String
        Dim Air_SellFromRecommendation As String
        Dim Air_SellFromRecommendationReply As String
        Dim Car_InformationImage As String
        Dim Car_InformationImageReply As String
        Dim Car_LocationList As String
        Dim Car_LocationListReply As String
        Dim Car_MultiAvailability As String
        Dim Car_MultiAvailabilityReply As String
        Dim Car_Availability As String
        Dim Car_AvailabilityReply As String
        Dim Car_Policy As String
        Dim Car_PolicyReply As String
        Dim Car_RateInformationFromAvailability As String
        Dim Car_RateInformationFromAvailabilityReply As String
        Dim Car_RateInformationFromCarSegment As String
        Dim Car_RateInformationFromCarSegmentReply As String
        Dim Car_Sell As String
        Dim Car_SellReply As String
        Dim Car_SingleAvailability As String
        Dim Car_SingleAvailabilityReply As String
        Dim Command_Cryptic As String
        Dim Command_CrypticReply As String
        Dim Cruise_CancelBooking As String
        Dim Cruise_CancelBookingReply As String
        Dim Cruise_ClaimBooking As String
        Dim Cruise_ClaimBookingReply As String
        Dim Cruise_CreateBooking As String
        Dim Cruise_CreateBookingReply As String
        Dim Cruise_DisplayBusDescription As String
        Dim Cruise_DisplayBusDescriptionReply As String
        Dim Cruise_DisplayCabinDescription As String
        Dim Cruise_DisplayCabinDescriptionReply As String
        Dim Cruise_DisplayCategoryDescription As String
        Dim Cruise_DisplayCategoryDescriptionReply As String
        Dim Cruise_DisplayFareDescription As String
        Dim Cruise_DisplayFareDescriptionReply As String
        Dim Cruise_DisplayInclusivePackageDescription As String
        Dim Cruise_DisplayInclusivePackageDescriptionReply As String
        Dim Cruise_DisplayItineraryDescription As String
        Dim Cruise_DisplayItineraryDescriptionReply As String
        Dim Cruise_DisplayPrePostPackageDescription As String
        Dim Cruise_DisplayPrePostPackageDescriptionReply As String
        Dim Cruise_DisplayProductInformation As String
        Dim Cruise_DisplayProductInformationReply As String
        Dim Cruise_EnterPassengerInformation As String
        Dim Cruise_EnterPassengerInformationReply As String
        Dim Cruise_GetBookingDetails As String
        Dim Cruise_GetBookingDetailsReply As String
        Dim Cruise_HoldCabin As String
        Dim Cruise_HoldCabinReply As String
        Dim Cruise_ModifyBooking As String
        Dim Cruise_ModifyBookingReply As String
        Dim Cruise_PriceBooking As String
        Dim Cruise_PriceBookingReply As String
        Dim Cruise_PriceBookingCancellation As String
        Dim Cruise_PriceBookingCancellationReply As String
        Dim Cruise_RequestBusAvailability As String
        Dim Cruise_RequestBusAvailabilityReply As String
        Dim Cruise_RequestCabinAvailability As String
        Dim Cruise_RequestCabinAvailabilityReply As String
        Dim Cruise_RequestCategoryAvailability As String
        Dim Cruise_RequestCategoryAvailabilityReply As String
        Dim Cruise_RequestFareAvailability As String
        Dim Cruise_RequestFareAvailabilityReply As String
        Dim Cruise_RequestInclusivePackageAvailability As String
        Dim Cruise_RequestInclusivePackageAvailabilityReply As String
        Dim Cruise_RequestPrePostPackageAvailability As String
        Dim Cruise_RequestPrePostPackageAvailabilityReply As String
        Dim Cruise_RequestSailingAvailability As String
        Dim Cruise_RequestSailingAvailabilityReply As String
        Dim Cruise_RequestShoreExcursionAvailability As String
        Dim Cruise_RequestShoreExcursionAvailabilityReply As String
        Dim Cruise_RequestSpecialServicesAvailability As String
        Dim Cruise_RequestSpecialServicesAvailabilityReply As String
        Dim Cruise_RequestTransferAvailability As String
        Dim Cruise_RequestTransferAvailabilityReply As String
        Dim Cruise_SearchBooking As String
        Dim Cruise_SearchBookingReply As String
        Dim Cruise_UnholdCabin As String
        Dim Cruise_UnholdCabinReply As String
        Dim Doc_DisplayItinerary As String
        Dim Doc_DisplayItineraryReply As String
        Dim DocIssuance_IssueTicket As String
        Dim DocIssuance_IssueTicketReply As String
        Dim DocRefund_CalculateRefund As String
        Dim DocRefund_CalculateRefundReply As String
        Dim DocRefund_IgnoreRefund As String
        Dim DocRefund_IgnoreRefundReply As String
        Dim DocRefund_InitRefund As String
        Dim DocRefund_InitRefundReply As String
        Dim DocRefund_ProcessRefund As String
        Dim DocRefund_ProcessRefundReply As String
        Dim DocRefund_SearchRefundRule As String
        Dim DocRefund_SearchRefundRuleReply As String
        Dim DocRefund_UpdateRefund As String
        Dim DocRefund_UpdateRefundReply As String
        Dim Fare_CheckRules As String
        Dim Fare_CheckRulesReply As String
        Dim Fare_DisplayFaresForCityPair As String
        Dim Fare_DisplayFaresForCityPairReply As String
        Dim Fare_GetFareFamilyDescription As String
        Dim Fare_GetFareFamilyDescriptionReply As String
        Dim Fare_InformativePricingWithoutPNR As String
        Dim Fare_InformativePricingWithoutPNRReply As String
        Dim Fare_InformativeBestPricingWithoutPNR As String
        Dim Fare_InformativeBestPricingWithoutPNRReply As String
        Dim Fare_MasterPricerCalendar As String
        Dim Fare_MasterPricerCalendarReply As String
        Dim Fare_MasterPricerExpertSearch As String
        Dim Fare_MasterPricerExpertSearchReply As String
        Dim Fare_MasterPricerTravelBoardSearch As String
        Dim Fare_MasterPricerTravelBoardSearchReply As String
        Dim Fare_MetaPricerCalendar As String
        Dim Fare_MetaPricerCalendarReply As String
        Dim Fare_MetaPricerTravelBoardSearch As String
        Dim Fare_MetaPricerTravelBoardSearchReply As String
        Dim Fare_PricePNRWithBookingClass As String
        Dim Fare_PricePNRWithBookingClassReply As String
        Dim Fare_PricePNRWithLowerFares As String
        Dim Fare_PricePNRWithLowerFaresReply As String
        Dim Fare_QuoteItinerary As String
        Dim Fare_QuoteItineraryReply As String
        Dim Fare_SellByFareCalendar As String
        Dim Fare_SellByFareCalendarReply As String
        Dim Fare_SellByFareSearch As String
        Dim Fare_SellByFareSearchReply As String
        Dim Fare_FlexPricerUpsell As String
        Dim Fare_FlexPricerUpsellReply As String
        Dim Hotel_MultiSingleAvailability As String
        Dim Hotel_MultiSingleAvailabilityReply As String
        Dim Hotel_AvailabilityMultiProperties As String
        Dim Hotel_AvailabilityMultiPropertiesReply As String
        Dim Hotel_Features As String
        Dim Hotel_FeaturesReply As String
        Dim Hotel_DescriptiveInfo As String
        Dim Hotel_DescriptiveInfoReply As String
        Dim Hotel_List As String
        Dim Hotel_ListReply As String
        Dim Hotel_RateChange As String
        Dim Hotel_RateChangeReply As String
        Dim Hotel_Sell As String
        Dim Hotel_SellReply As String
        Dim Hotel_SingleAvailability As String
        Dim Hotel_SingleAvailabilityReply As String
        Dim Hotel_StructuredPricing As String
        Dim Hotel_StructuredPricingReply As String
        Dim Hotel_Terms As String
        Dim Hotel_TermsReply As String
        Dim Hotel_EnhancedSingleAvail As String
        Dim Hotel_EnhancedSingleAvailReply As String
        Dim Hotel_MultiAvailability As String
        Dim Hotel_MultiAvailabilityReply As String
        Dim Hotel_EnhancedPricing As String
        Dim Hotel_EnhancedPricingReply As String
        Dim Hotel_CalendarView As String
        Dim Hotel_CalendarViewReply As String
        Dim MiniRule_GetFromPricing As String
        Dim MiniRule_GetFromPricingReply As String
        Dim MiniRule_GetFromPricingRec As String
        Dim MiniRule_GetFromPricingRecReply As String
        Dim PNR_AddMultiElements As String
        Dim PNR_Cancel As String
        Dim PNR_Ignore As String
        Dim PNR_IgnoreReply As String
        Dim PNR_List As String
        Dim PNR_Reply As String
        Dim PNR_Reply1 As String
        Dim PNR_Retrieve As String
        Dim PNR_RetrieveByRecLoc As String
        Dim PNR_RetrieveByRecLocReply As String
        Dim PNR_TransferOwnership As String
        Dim PNR_TransferOwnershipReply As String
        Dim PNR_Split As String
        Dim PNR_SplitReply As String
        Dim Profile_CreateUpdateProfile As String
        Dim Profile_CreateUpdateProfileReply As String
        Dim Profile_CreateProfile As String
        Dim Profile_CreateProfileReply As String
        Dim Profile_UpdateProfile As String
        Dim Profile_UpdateProfileReply As String
        Dim Profile_DeleteProfile As String
        Dim Profile_DeleteProfileReply As String
        Dim Profile_DeactivateProfile As String
        Dim Profile_DeactivateProfileReply As String
        Dim Profile_RetrieveProfile As String
        Dim Profile_RetrieveProfileReply As String
        Dim Profile_ProfileReply As String
        Dim Profile_ReadProfile As String
        Dim Profile_ReadProfileReply As String
        Dim Queue_CountTotal As String
        Dim Queue_CountTotalReply As String
        Dim Queue_List As String
        Dim Queue_ListReply As String
        Dim Queue_MoveItem As String
        Dim Queue_MoveItemReply As String
        Dim Queue_PlacePNR As String
        Dim Queue_PlacePNRReply As String
        Dim Queue_RemoveItem As String
        Dim Queue_RemoveItemReply As String
        Dim QueueMode_ProcessQueue As String
        Dim QueueMode_ProcessQueueReply As String
        Dim Security_Authenticate As String
        Dim Security_AuthenticateReply As String
        Dim Security_SignOut As String
        Dim Security_SignOutReply As String
        Dim Ticket_ATCShopperMasterPricerTravelBoardSearch As String
        Dim Ticket_ATCShopperMasterPricerTravelBoardSearchReply As String
        Dim Ticket_CancelDocument As String
        Dim Ticket_CancelDocumentReply As String
        Dim Ticket_CheckEligibility As String
        Dim Ticket_CheckEligibilityReply As String
        Dim Ticket_CreateTSTFromPricing As String
        Dim Ticket_CreateTSTFromPricingReply As String
        Dim Ticket_CreditCardCheck As String
        Dim Ticket_CreditCardCheckReply As String
        Dim Ticket_DeleteTST As String
        Dim Ticket_DeleteTSTReply As String
        Dim Ticket_DisplayTST As String
        Dim Ticket_GetPricingOptions As String
        Dim Ticket_GetPricingOptionsReply As String
        Dim Ticket_DisplayTSTReply As String
        Dim Ticket_ProcessETicket As String
        Dim Ticket_ProcessETicketReply As String
        Dim Ticket_ProcessEDoc As String
        Dim Ticket_ProcessEDocReply As String
        Dim Ticket_RepricePNRWithBookingClass As String
        Dim Ticket_RepricePNRWithBookingClassReply As String
        Dim Ticket_UpdateTST As String
        Dim Ticket_UpdateTSTReply As String
        Dim Ticket_AutomaticUpdate As String
        Dim Ticket_AutomaticUpdateReply As String
        Dim PAY_GenerateVirtualCard As String
        Dim PAY_ListVirtualCards As String
        Dim PAY_VirtualCardDetails As String
        Dim PAY_DeleteVirtualCard As String
        Dim SalesReports_DisplayQueryReport As String
        Dim SalesReports_DisplayQueryReportReply As String

    End Structure

#End Region

#Region " Enumerators "

    Public Enum ttServices
        AirAvail = 1
        AirFlifo = 2
        AirPrice = 3
        AirRules = 4
        AirSeatMap = 5
        LowFare = 6
        LowFarePlus = 7
        CarAvail = 8
        CarInfo = 9
        HotelAvail = 10
        HotelInfo = 11
        HotelSearch = 12
        PNRRead = 13
        PNRCancel = 14
        TravelBuild = 15
        CreateSession = 16
        CloseSession = 17
        CurConv = 18
        CCValid = 19
        TimeDiff = 20
        ShowMileage = 21
        Cryptic = 22
        CruiseSailAvail = 23
        CruiseFareAvail = 24
        CruiseCategoryAvail = 25
        CruiseCabinAvail = 26
        CruiseCabinHold = 27
        CruiseCreateBooking = 28
        CruisePriceBooking = 29
        Native = 30
        HotelModify = 31
        IssueTicket = 32
        GeoList = 33
        FareDisplay = 34
        Queue = 35
        QueueRead = 36
        CruiseCabinUnhold = 37
        CruiseRead = 38
        ETicketVerify = 39
        InsuranceBook = 40
        InsuranceQuote = 41
        CruiseCancelBooking = 42
        CruiseModifyBooking = 43
        CruisePackageAvail = 44
        CruiseTransferAvail = 45
        CruisePackageDesc = 46
        EncodeDecode = 47
        CruiseItineraryDesc = 48
        AddonAvail = 49
        MultiMessage = 50
        TravelModify = 51
        ProductCategories = 52
        ProductList = 53
        ProductInfo = 54
        FormFields = 55
        SendOrder = 56
        HotelRatePlanNotif = 57
        PkgAvail = 58
        HotelRes = 59
        PkgBook = 60
        'Cancel = 61
        Read = 62
        BookingsRead = 63
        TourcoGetCities = 64
        TourcoGetPackages = 65
        TourcoGetVendors = 66
        TourcoGetCustom = 67
        LowFareSchedule = 68
        Ping = 69
        Update = 70
        TransferOwnership = 71
        StoredFareBuild = 72
        StoredFareUpdate = 73
        FareInfo = 74
        CarRules = 75
        CrypticEntries = 76
        AddPNRToAdmin = 77
        PNRReprice = 78
        AirSchedule = 79
        ProfileRead = 80
        GetDeals = 81
        ProfileCreate = 82
        UpdateSessioned = 83
        PNREnd = 84
        LowFareMatrix = 85
        LowFareFlights = 86
        IssueTicketSessioned = 87
        AddRecLocToAdmin = 88
        TicketVoid = 89
        AddRecLocToNewAdminOnly = 90
        InventoryManagement = 91
        TicketDisplay = 92
        LowOfferMatrix = 93
        LowOfferSearch = 94
        SalesReport = 95
        CarList = 96
        MiniRule = 97
        RefundTicket = 98
        PNRSplit = 99
        SearchName = 100
        AirShopping = 101
        Authorization = 102
        GenerateVirtualCard = 103
        CancelVirtualCardLoad = 104
        DeleteVirtualCard = 105
        GetVirtualCardDetails = 106
        ListVirtualCards = 107
        ManageDBIData = 108
        ScheduleVirtualCardLoad = 109
        UpdateVirtualCard = 110
        ReissueTicket = 111
        DisplayQueryReport = 112
        '--------------------------------------
    End Enum

    Public Enum enSchemaType
        Request = 1
        Response = 2
    End Enum

    Public Enum enSOAPHeaderType
        SOAP1 = 0
        SOAP2 = 1
        SOAP4 = 2
    End Enum


#End Region

#Region " Format Error Message "

    Public Function FormatErrorMessage(ByVal service As ttServices, ByVal message As String, ByVal providerSystems As TripXMLProviderSystems, Optional ByVal recordLocator As String = "", Optional ByVal OTA_Version As String = "") As String
        Dim strResponse As String
        Dim version As String
        Dim tag As String

        Select Case service
            Case ttServices.AirAvail
                tag = "OTA_AirAvailRS"
                version = "1.001"
            Case ttServices.AirFlifo
                tag = "OTA_AirFlifoRS"
                version = "1.001"
            Case ttServices.AirPrice
                tag = "OTA_AirPriceRS"
                version = "2.000"
            Case ttServices.AirRules
                tag = "OTA_AirRulesRS"
                version = "2.000"
            Case ttServices.AirSeatMap
                tag = "OTA_AirSeatMapRS"
                version = "1.000"
            Case ttServices.LowFare
                tag = "OTA_AirLowFareSearchRS"
                version = "2.000"
            Case ttServices.LowFarePlus
                tag = "OTA_AirLowFareSearchPlusRS"
                version = "2.000"
            Case ttServices.LowFareMatrix
                tag = "OTA_AirLowFareSearchMatrixRS"
                version = "2.000"
            Case ttServices.LowFareFlights
                tag = "OTA_AirLowFareSearchFlightsRS"
                version = "2.000"
            Case ttServices.LowFareSchedule
                tag = "OTA_AirLowFareSearchScheduleRS"
                version = "2.000"
            Case ttServices.CarAvail
                tag = "OTA_VehAvailRateRS"
                version = "2.000"
            Case ttServices.CarInfo
                tag = "OTA_VehLocDetailRS"
                version = "2.000"
            Case ttServices.HotelAvail
                tag = "OTA_HotelAvailRS"
                version = "1.001"
            Case ttServices.HotelInfo
                tag = "OTA_HotelDescriptiveInfoRS"
                version = "1.001"
            Case ttServices.HotelSearch
                tag = "OTA_HotelSearchRS"
                version = "1.001"
            Case ttServices.PNRRead
                tag = "OTA_TravelItineraryRS"
                version = "v03"
            Case ttServices.PNRCancel
                tag = "OTA_CancelRS"
                version = "1.001"
            Case ttServices.TravelBuild
                tag = "OTA_TravelItineraryRS"
                version = "v03"
            Case ttServices.ShowMileage
                tag = "OTA_ShowMileageRS"
                version = "1.001"
            Case ttServices.CreateSession
                tag = "SessionCreateRS"
                version = "1.001"
            Case ttServices.CloseSession
                tag = "SessionCloseRS"
                version = "1.001"
            Case ttServices.Cryptic
                tag = "CrypticRS"
                version = ""
            Case ttServices.CCValid
                tag = "OTA_CCValidRS"
                version = ""
            Case ttServices.CurConv
                tag = "OTA_CurConvRS"
                version = ""
            Case ttServices.TimeDiff
                tag = "OTA_TimeDiffRS"
                version = ""
            Case ttServices.CruiseCabinAvail
                tag = "OTA_CruiseCabinAvailRS"
                version = "1.000"
            Case ttServices.CruiseCategoryAvail
                tag = "OTA_CruiseCategoryAvailRS"
                version = "1.000"
            Case ttServices.CruiseFareAvail
                tag = "OTA_CruiseFareAvailRS"
                version = "1.000"
            Case ttServices.CruiseSailAvail
                tag = "OTA_CruiseSailAvailRS"
                version = "1.000"
            Case ttServices.CruiseCabinAvail
                tag = "OTA_CruiseCabinAvailRS"
                version = "1.000"
            Case ttServices.CruiseCabinHold
                tag = "OTA_CruiseCabinHoldRS"
                version = "1.000"
            Case ttServices.CruiseCabinUnhold
                tag = "OTA_CruiseCabinUnholdRS"
                version = "1.000"
            Case ttServices.CruisePriceBooking
                tag = "OTA_CruisePriceBookingRS"
                version = "1.000"
            Case ttServices.CruiseCreateBooking
                tag = "OTA_CruiseCreateBookingRS"
                version = "1.000"
            Case ttServices.CruiseRead
                tag = "OTA_CruiseReadRS"
                version = "1.000"
            Case ttServices.CruiseCancelBooking
                tag = "OTA_CruiseCancelRS"
                version = "1.000"
            Case ttServices.CruiseModifyBooking
                tag = "OTA_CruiseCreateBookingRS"
                version = "1.000"
            Case ttServices.CruisePackageAvail
                tag = "OTA_CruisePackageAvailRS"
                version = "1.000"
            Case ttServices.Native
                tag = "NativeRS"
                version = "1.000"
            Case ttServices.HotelModify
                tag = "OTA_TravelItineraryRS"
                version = "v03"
            Case ttServices.IssueTicket
                tag = "TT_IssueTicketRS"
                version = "1.000"
            Case ttServices.GeoList
                tag = "TT_GeoListRS"
                version = "1.000"
            Case ttServices.FareDisplay
                tag = "OTA_AirFareDisplayRS"
                version = "1.000"
            Case ttServices.Queue
                tag = "OTA_QueueRS"
                version = "1.000"
            Case ttServices.QueueRead
                tag = "OTA_TravelItineraryRS"
                If Not String.IsNullOrEmpty(OTA_Version) Then
                    version = OTA_Version
                Else
                    version = "v03"
                End If
            Case ttServices.ETicketVerify
                tag = "OTA_ETicketVerifyRS"
                version = "1.000"
            Case ttServices.InsuranceBook
                tag = "OTA_InsuranceBookRS"
                version = "1.001"
            Case ttServices.InsuranceQuote
                tag = "OTA_InsuranceQuoteRS"
                version = "1.001"
            Case ttServices.CruiseItineraryDesc
                tag = "OTA_CruiseItineraryAvailRS"
                version = "1.000"
            Case ttServices.AddonAvail
                tag = "OTA_AddonAvailRS"
                version = "1.000"
            Case ttServices.TravelModify
                tag = "OTA_TravelItineraryRS"
                version = "v03"
            Case ttServices.Update
                tag = "OTA_TravelItineraryRS"
                version = "v03"
            Case ttServices.UpdateSessioned
                tag = "OTA_TravelItineraryRS"
                version = "v03"
            Case ttServices.PNRSplit
                tag = "OTA_TravelItineraryRS"
                version = "v03"
            Case ttServices.IssueTicketSessioned
                tag = "TT_IssueTicketRS"
                version = "1.001"
            Case ttServices.PNRReprice
                tag="OTA_PNRRepriceRS"
                version = "v03"
            Case Else
                tag = ""
                version = "1.001"
        End Select

        If Not String.IsNullOrEmpty(OTA_Version) Then version = OTA_Version

        Dim lstError As New List(Of String)
        If Not message.Contains(vbNewLine) Then
            lstError.Add(message)
        Else
            lstError.AddRange(message.Split(vbNewLine).ToList())
        End If

        Dim trace As New JObject(
            New JProperty("@Status", IIf(StrComp(tag, "OTA_CancelRS") = 0, "Unsuccessful", "")),
            New JProperty("@Version", version),
            New JProperty("@TransactionIdentifier", providerSystems.Provider),
            New JProperty("@UniqueID", IIf(Not String.IsNullOrEmpty(recordLocator), recordLocator, "")),
            New JProperty("@TimeStamp", Now),
            New JProperty("Errors", GetListToJSON(lstError)))


        AddLog(LogType.Error, tag, providerSystems, trace)

        Dim jsonTrace As String = JsonConvert.SerializeObject(trace)
        Dim doc As XmlDocument = JsonConvert.DeserializeXmlNode(jsonTrace, tag) 'strResponse
        strResponse = doc.InnerXml

        Return strResponse

    End Function

    Public Function FormatErrorMessage(ByVal Service As ttServices, ByVal Message As String, ByVal Provider As String, Optional ByVal RecordLocator As String = "", Optional ByVal MessageIsNode As Boolean = False, Optional ByVal OTA_Version As String = "") As String
        Dim strResponse As String = ""
        Dim version As String = ""
        Dim tag As String = ""

        Select Case Service
            Case ttServices.AirAvail
                tag = "OTA_AirAvailRS"
                version = "1.001"
            Case ttServices.AirFlifo
                tag = "OTA_AirFlifoRS"
                version = "1.001"
            Case ttServices.AirPrice
                tag = "OTA_AirPriceRS"
                version = "2.000"
            Case ttServices.AirRules
                tag = "OTA_AirRulesRS"
                version = "2.000"
            Case ttServices.AirSeatMap
                tag = "OTA_AirSeatMapRS"
                version = "1.000"
            Case ttServices.LowFare
                tag = "OTA_AirLowFareSearchRS"
                version = "2.000"
            Case ttServices.LowFarePlus
                tag = "OTA_AirLowFareSearchPlusRS"
                version = "2.000"
            Case ttServices.LowFareMatrix
                tag = "OTA_AirLowFareSearchMatrixRS"
                version = "2.000"
            Case ttServices.LowFareFlights
                tag = "OTA_AirLowFareSearchFlightsRS"
                version = "2.000"
            Case ttServices.LowFareSchedule
                tag = "OTA_AirLowFareSearchScheduleRS"
                version = "2.000"
            Case ttServices.CarAvail
                tag = "OTA_VehAvailRateRS"
                version = "2.000"
            Case ttServices.CarInfo
                tag = "OTA_VehLocDetailRS"
                version = "2.000"
            Case ttServices.HotelAvail
                tag = "OTA_HotelAvailRS"
                version = "1.001"
            Case ttServices.HotelInfo
                tag = "OTA_HotelDescriptiveInfoRS"
                version = "1.001"
            Case ttServices.HotelSearch
                tag = "OTA_HotelSearchRS"
                version = "1.001"
            Case ttServices.PNRRead
                tag = "OTA_TravelItineraryRS"
                version = "v03"
            Case ttServices.PNRReprice
                tag = "OTA_PNRRepriceRS"
                version = "v03"
            Case ttServices.PNRCancel
                tag = "OTA_CancelRS"
                version = "1.001"
            Case ttServices.TravelBuild
                tag = "OTA_TravelItineraryRS"
                version = "v03"
            Case ttServices.ShowMileage
                tag = "OTA_ShowMileageRS"
                version = "1.001"
            Case ttServices.CreateSession
                tag = "SessionCreateRS"
                version = "1.001"
            Case ttServices.CloseSession
                tag = "SessionCloseRS"
                version = "1.001"
            Case ttServices.Cryptic
                tag = "CrypticRS"
                version = ""
            Case ttServices.CCValid
                tag = "OTA_CCValidRS"
                version = ""
            Case ttServices.CurConv
                tag = "OTA_CurConvRS"
                version = ""
            Case ttServices.TimeDiff
                tag = "OTA_TimeDiffRS"
                version = ""
            Case ttServices.CruiseCabinAvail
                tag = "OTA_CruiseCabinAvailRS"
                version = "1.000"
            Case ttServices.CruiseCategoryAvail
                tag = "OTA_CruiseCategoryAvailRS"
                version = "1.000"
            Case ttServices.CruiseFareAvail
                tag = "OTA_CruiseFareAvailRS"
                version = "1.000"
            Case ttServices.CruiseSailAvail
                tag = "OTA_CruiseSailAvailRS"
                version = "1.000"
            Case ttServices.CruiseCabinAvail
                tag = "OTA_CruiseCabinAvailRS"
                version = "1.000"
            Case ttServices.CruiseCabinHold
                tag = "OTA_CruiseCabinHoldRS"
                version = "1.000"
            Case ttServices.CruiseCabinUnhold
                tag = "OTA_CruiseCabinUnholdRS"
                version = "1.000"
            Case ttServices.CruisePriceBooking
                tag = "OTA_CruisePriceBookingRS"
                version = "1.000"
            Case ttServices.CruiseCreateBooking
                tag = "OTA_CruiseCreateBookingRS"
                version = "1.000"
            Case ttServices.CruiseRead
                tag = "OTA_CruiseReadRS"
                version = "1.000"
            Case ttServices.CruiseCancelBooking
                tag = "OTA_CruiseCancelRS"
                version = "1.000"
            Case ttServices.CruiseModifyBooking
                tag = "OTA_CruiseCreateBookingRS"
                version = "1.000"
            Case ttServices.CruisePackageAvail
                tag = "OTA_CruisePackageAvailRS"
                version = "1.000"
            Case ttServices.Native
                tag = "NativeRS"
                version = "1.000"
            Case ttServices.HotelModify
                tag = "OTA_TravelItineraryRS"
                version = "v03"
            Case ttServices.IssueTicket
                tag = "TT_IssueTicketRS"
                version = "1.000"
            Case ttServices.GeoList
                tag = "TT_GeoListRS"
                version = "1.000"
            Case ttServices.FareDisplay
                tag = "OTA_AirFareDisplayRS"
                version = "1.000"
            Case ttServices.Queue
                tag = "OTA_QueueRS"
                version = "1.000"
            Case ttServices.QueueRead
                tag = "OTA_TravelItineraryRS"
                If Not String.IsNullOrEmpty(OTA_Version) Then
                    version = OTA_Version
                Else
                    version = "v03"
                End If
            Case ttServices.ETicketVerify
                tag = "OTA_ETicketVerifyRS"
                version = "1.000"
            Case ttServices.InsuranceBook
                tag = "OTA_InsuranceBookRS"
                version = "1.001"
            Case ttServices.InsuranceQuote
                tag = "OTA_InsuranceQuoteRS"
                version = "1.001"
            Case ttServices.CruiseItineraryDesc
                tag = "OTA_CruiseItineraryAvailRS"
                version = "1.000"
            Case ttServices.AddonAvail
                tag = "OTA_AddonAvailRS"
                version = "1.000"
            Case ttServices.TravelModify
                tag = "OTA_TravelItineraryRS"
                version = "v03"
            Case ttServices.Update
                tag = "OTA_TravelItineraryRS"
                version = "v03"
            Case ttServices.UpdateSessioned
                tag = "OTA_TravelItineraryRS"
                version = "v03"
            Case ttServices.PNRSplit
                tag = "OTA_TravelItineraryRS"
                version = "v03"
            Case ttServices.IssueTicketSessioned
                tag = "TT_IssueTicketRS"
                version = "1.001"
        End Select

        If Not String.IsNullOrEmpty(OTA_Version) Then version = OTA_Version

        Dim lstError As New List(Of String)
        If MessageIsNode Then
            lstError.Add(Message)
        Else
            lstError.AddRange(Message.Split(vbNewLine).ToList())
        End If

        Dim ps As New TripXMLProviderSystems

        ps.UserName = ""
        ps.UserID = ""
        ps.Provider = Provider

        Dim trace As New JObject(
            New JProperty("Status", IIf(StrComp(tag, "OTA_CancelRS") = 0, "Unsuccessful", "")),
                New JProperty("@Version", version),
                New JProperty("@AltLangID", IIf(Not String.IsNullOrEmpty(Provider), Provider, String.Empty)),
                New JProperty("@UniqueID", IIf(Not String.IsNullOrEmpty(RecordLocator), RecordLocator, "")),
                New JProperty("@TimeStamp", Now),
                New JProperty("Errors", GetListToJSON(lstError)))

        AddLog(LogType.Error, tag, ps, trace)

        Dim jsonTrace As String = JsonConvert.SerializeObject(trace)
        Dim doc As XmlDocument = JsonConvert.DeserializeXmlNode(jsonTrace, tag) 'strResponse
        strResponse = doc.InnerXml

        Return strResponse

    End Function

    Private Function GetListToJSON(list As List(Of String)) As JArray
        '    For i = 0 To arError.GetLength(0) - 1
        '        sb.Append("<Error Type=""E"">").Append(arError(i)).Append("</Error>")
        '    Next

        '<OTA_TravelItineraryRS Version="v03" TransactionIdentifier="Amadeus"><Errors><Error Type="E">/QUEUE CATEGORY EMPTY</Error></Errors></OTA_TravelItineraryRS>
        Try
            Dim elements As New JArray(
                From cont In list Select New JObject(
                                      New JProperty("Error", New JObject(New JProperty("@Type", "E"),
                                                                         New JProperty("#text", cont)))
                ))
            Return elements
        Catch ex As Exception
            Return JsonConvert.DeserializeObject(String.Format("<Error Type='E'>{0}</Error>", String.Join(",", list)))
        End Try

    End Function

    'Public Function FormatErrorMessage(ByVal service As ttServices, ByVal message As String, ByVal providerSystems As TripXMLProviderSystems, Optional ByVal recordLocator As String = "", Optional ByVal OTA_Version As String = "") As String
    '    Dim strResponse As String
    '    Dim version As String = ""
    '    Dim tag As String = ""
    '    Dim status As String
    '    Dim arError() As String
    '    Dim i As Integer
    '    Dim sb As StringBuilder = New StringBuilder()

    '    Select Case service
    '        Case ttServices.AirAvail
    '            tag = "OTA_AirAvailRS"
    '            version = "1.001"
    '        Case ttServices.AirFlifo
    '            tag = "OTA_AirFlifoRS"
    '            version = "1.001"
    '        Case ttServices.AirPrice
    '            tag = "OTA_AirPriceRS"
    '            version = "2.000"
    '        Case ttServices.AirRules
    '            tag = "OTA_AirRulesRS"
    '            version = "2.000"
    '        Case ttServices.AirSeatMap
    '            tag = "OTA_AirSeatMapRS"
    '            version = "1.000"
    '        Case ttServices.LowFare
    '            tag = "OTA_AirLowFareSearchRS"
    '            version = "2.000"
    '        Case ttServices.LowFarePlus
    '            tag = "OTA_AirLowFareSearchPlusRS"
    '            version = "2.000"
    '        Case ttServices.LowFareMatrix
    '            tag = "OTA_AirLowFareSearchMatrixRS"
    '            version = "2.000"
    '        Case ttServices.LowFareFlights
    '            tag = "OTA_AirLowFareSearchFlightsRS"
    '            version = "2.000"
    '        Case ttServices.LowFareSchedule
    '            tag = "OTA_AirLowFareSearchScheduleRS"
    '            version = "2.000"
    '        Case ttServices.CarAvail
    '            tag = "OTA_VehAvailRateRS"
    '            version = "2.000"
    '        Case ttServices.CarInfo
    '            tag = "OTA_VehLocDetailRS"
    '            version = "2.000"
    '        Case ttServices.HotelAvail
    '            tag = "OTA_HotelAvailRS"
    '            version = "1.001"
    '        Case ttServices.HotelInfo
    '            tag = "OTA_HotelDescriptiveInfoRS"
    '            version = "1.001"
    '        Case ttServices.HotelSearch
    '            tag = "OTA_HotelSearchRS"
    '            version = "1.001"
    '        Case ttServices.PNRRead
    '            tag = "OTA_TravelItineraryRS"
    '            version = "v03"
    '        Case ttServices.PNRCancel
    '            tag = "OTA_CancelRS"
    '            version = "1.001"
    '        Case ttServices.TravelBuild
    '            tag = "OTA_TravelItineraryRS"
    '            version = "v03"
    '        Case ttServices.ShowMileage
    '            tag = "OTA_ShowMileageRS"
    '            version = "1.001"
    '        Case ttServices.CreateSession
    '            tag = "SessionCreateRS"
    '            version = "1.001"
    '        Case ttServices.CloseSession
    '            tag = "SessionCloseRS"
    '            version = "1.001"
    '        Case ttServices.Cryptic
    '            tag = "CrypticRS"
    '            version = ""
    '        Case ttServices.CCValid
    '            tag = "OTA_CCValidRS"
    '            version = ""
    '        Case ttServices.CurConv
    '            tag = "OTA_CurConvRS"
    '            version = ""
    '        Case ttServices.TimeDiff
    '            tag = "OTA_TimeDiffRS"
    '            version = ""
    '        Case ttServices.CruiseCabinAvail
    '            tag = "OTA_CruiseCabinAvailRS"
    '            version = "1.000"
    '        Case ttServices.CruiseCategoryAvail
    '            tag = "OTA_CruiseCategoryAvailRS"
    '            version = "1.000"
    '        Case ttServices.CruiseFareAvail
    '            tag = "OTA_CruiseFareAvailRS"
    '            version = "1.000"
    '        Case ttServices.CruiseSailAvail
    '            tag = "OTA_CruiseSailAvailRS"
    '            version = "1.000"
    '        Case ttServices.CruiseCabinAvail
    '            tag = "OTA_CruiseCabinAvailRS"
    '            version = "1.000"
    '        Case ttServices.CruiseCabinHold
    '            tag = "OTA_CruiseCabinHoldRS"
    '            version = "1.000"
    '        Case ttServices.CruiseCabinUnhold
    '            tag = "OTA_CruiseCabinUnholdRS"
    '            version = "1.000"
    '        Case ttServices.CruisePriceBooking
    '            tag = "OTA_CruisePriceBookingRS"
    '            version = "1.000"
    '        Case ttServices.CruiseCreateBooking
    '            tag = "OTA_CruiseCreateBookingRS"
    '            version = "1.000"
    '        Case ttServices.CruiseRead
    '            tag = "OTA_CruiseReadRS"
    '            version = "1.000"
    '        Case ttServices.CruiseCancelBooking
    '            tag = "OTA_CruiseCancelRS"
    '            version = "1.000"
    '        Case ttServices.CruiseModifyBooking
    '            tag = "OTA_CruiseCreateBookingRS"
    '            version = "1.000"
    '        Case ttServices.CruisePackageAvail
    '            tag = "OTA_CruisePackageAvailRS"
    '            version = "1.000"
    '        Case ttServices.Native
    '            tag = "NativeRS"
    '            version = "1.000"
    '        Case ttServices.HotelModify
    '            tag = "OTA_TravelItineraryRS"
    '            version = "v03"
    '        Case ttServices.IssueTicket
    '            tag = "TT_IssueTicketRS"
    '            version = "1.000"
    '        Case ttServices.GeoList
    '            tag = "TT_GeoListRS"
    '            version = "1.000"
    '        Case ttServices.FareDisplay
    '            tag = "OTA_AirFareDisplayRS"
    '            version = "1.000"
    '        Case ttServices.Queue
    '            tag = "OTA_QueueRS"
    '            version = "1.000"
    '        Case ttServices.QueueRead
    '            tag = "OTA_TravelItineraryRS"
    '            If Not String.IsNullOrEmpty(OTA_Version) Then
    '                version = OTA_Version
    '            Else
    '                version = "v03"
    '            End If
    '        Case ttServices.ETicketVerify
    '            tag = "OTA_ETicketVerifyRS"
    '            version = "1.000"
    '        Case ttServices.InsuranceBook
    '            tag = "OTA_InsuranceBookRS"
    '            version = "1.001"
    '        Case ttServices.InsuranceQuote
    '            tag = "OTA_InsuranceQuoteRS"
    '            version = "1.001"
    '        Case ttServices.CruiseItineraryDesc
    '            tag = "OTA_CruiseItineraryAvailRS"
    '            version = "1.000"
    '        Case ttServices.AddonAvail
    '            tag = "OTA_AddonAvailRS"
    '            version = "1.000"
    '        Case ttServices.TravelModify
    '            tag = "OTA_TravelItineraryRS"
    '            version = "v03"
    '        Case ttServices.Update
    '            tag = "OTA_TravelItineraryRS"
    '            version = "v03"
    '        Case ttServices.UpdateSessioned
    '            tag = "OTA_TravelItineraryRS"
    '            version = "v03"
    '        Case ttServices.PNRSplit
    '            tag = "OTA_TravelItineraryRS"
    '            version = "v03"
    '        Case ttServices.IssueTicketSessioned
    '            tag = "TT_IssueTicketRS"
    '            version = "1.001"
    '    End Select
    '    If Not String.IsNullOrEmpty(OTA_Version) Then version = OTA_Version
    '    If StrComp(tag, "OTA_CancelRS") = 0 Then
    '        status = " Status = ""Unsuccessful"""
    '    Else
    '        status = ""
    '    End If
    '    sb.Append(" Version=""").Append(version).Append("""")
    '    version = sb.ToString()
    '    sb.Remove(0, sb.Length())
    '    If Not String.IsNullOrEmpty(providerSystems.Provider) Then
    '        sb.Append(" TransactionIdentifier=""").Append(providerSystems.Provider).Append("""")
    '        providerSystems.Provider = sb.ToString()
    '        sb.Remove(0, sb.Length())
    '    End If
    '    sb.Append("<").Append(tag).Append(version).Append(providerSystems.Provider).Append(status).Append(">")
    '    If Not String.IsNullOrEmpty(recordLocator) Then
    '        sb.Append("<UniqueID ID='").Append(recordLocator).Append("'></UniqueID>")
    '    End If
    '    sb.Append("<Errors>")
    '    arError = message.Split(vbNewLine)
    '    For i = 0 To arError.GetLength(0) - 1
    '        sb.Append("<Error Type=""E"">").Append(arError(i)).Append("</Error>")
    '    Next
    '    '=============================================================================
    '    'Try
    '    '    Dim strPath As String = WebConfigurationManager.AppSettings("TripXMLFolder")
    '    '    strPath &= "\Tables\Users\"
    '    '    Dim oDoc As New XmlDocument
    '    '    ' Load Access Control List into memory'Try
    '    '    oDoc.Load(sb1.Append(strPath).Append("tt_acl.xml").ToString())
    '    '    sb1.Remove(0, sb1.Length())
    '    '    Dim oRoot As XmlElement = oDoc.DocumentElement
    '    '    If Not oRoot.SelectSingleNode("@Name") Is Nothing Then
    '    '        ThisMachine = oRoot.SelectSingleNode("@Name").InnerText
    '    '    End If
    '    '    sb.Append("<Error>").Append(ThisMachine).Append("</Error>")
    '    '    Dim myDTFI As DateTimeFormatInfo = New CultureInfo("en-US", True).DateTimeFormat
    '    '    sb.Append("<Error>").Append(DateTime.UtcNow.ToString(myDTFI).Substring(11) + " GMT").Append("</Error>")
    '    'Catch exr As Exception
    '    '    CoreLib.SendTrace("", "modeCore", "FormatErrorMessage: Error Loading tt_acl.xml", exr.Message)
    '    '    Throw exr
    '    'End Try
    '    '===========================================================================
    '    sb.Append("</Errors></").Append(tag).Append(">")
    '    strResponse = sb.ToString()
    '    'sb1 = Nothing
    '    AddLog(String.Format("<EXOR><M>{0}</M><EXOR/>", strResponse), providerSystems.UserName)
    '    'ttProviderSystems.UserName
    '    Return strResponse
    'End Function

#End Region

#Region "Logging"

    Public Sub AddLog(ByVal logType As LogType, ByRef message As String, ByVal providerSystems As TripXMLProviderSystems)
        Dim lg As New Log
        lg.Message = message
        lg.Type = logType.ToString()
        lg.UserName = providerSystems.UserName
        lg.UserID = providerSystems.UserID
        lg.Provider = providerSystems.Provider

        AddLog(lg)
    End Sub

    Private Sub AddLog(ByVal log As Log)
        Dim fileNumber As Integer
        'Dim DirPath As String = "C:\\TripXML\\log"
        Try
            Dim filePath As String = String.Format("{0}\\{1}_{2}.log", WebConfigurationManager.AppSettings("TripXMLLogFolder"), log.UserName, DateTime.Today.ToString("dd-MM-yyyy"))
            fileNumber = FreeFile()
            FileOpen(fileNumber, filePath, OpenMode.Append)
            PrintLine(fileNumber, log.ToString())
            FileClose(fileNumber)
        Catch ex As Exception
            ' 
        End Try
    End Sub

    Public Sub AddLog(ByRef message As String, ByVal username As String)
        Dim fileNumber As Integer
        Dim strPath As String = WebConfigurationManager.AppSettings("TripXMLLogFolder")

        Try
            Dim filePath As String = String.Format("\\{0}_{1}", username, DateTime.Today.ToString("dd-MM-yyyy"))
            'Dim DirPath As String = "C:\\TripXML\\log"
            filePath = strPath + filePath + ".log"
            fileNumber = FreeFile()

            FileOpen(fileNumber, filePath, OpenMode.Append)
            PrintLine(fileNumber, message)
            FileClose(fileNumber)
        Catch ex As Exception
            ' 
        End Try

    End Sub

    Public Sub AddLog_old(ByRef Message As String, ByVal Username As String)

        Dim fileNumber As Integer
        Try
            Dim filePath As String = "log\\" + Username + "_" + DateTime.Today.ToString("dd-MM-yyyy")
            'Dim DirPath As String = "C:\\TripXML\\log"
            filePath = "C:\\TripXML\\" + filePath + ".txt"
            fileNumber = FreeFile()

            FileOpen(fileNumber, filePath, OpenMode.Append)
            PrintLine(fileNumber, Message)
            FileClose(fileNumber)
        Catch ex As Exception
            ' 
        End Try

    End Sub

    Public Sub AddLog(ByVal logType As LogType, ByRef message As String, ByVal providerSystems As TripXMLProviderSystems, ByVal items As Object)
        Dim lg As New Log
        lg.Message = message
        lg.Type = logType.ToString()
        lg.UserName = providerSystems.UserName
        lg.UserID = providerSystems.UserID
        lg.Provider = providerSystems.Provider
        lg.Items = New List(Of Object) From {items}
        lg.Resourse = Assembly.GetExecutingAssembly().GetName().Name

        AddLog(lg)
    End Sub

    Public Enum LogType
        Info = 1
        [Error] = 2
        Success = 3
    End Enum

    Public Class Log
        ''' <summary>
        ''' TransactionIdentifier
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Amadeus, Sabre</remarks>
        Public Property Provider As String
        Public Property RecordLocator As String
        Public Property UserName As String
        Public Property UserID As String
        Public Property Version As String
        Public Property Resourse As String
        Public Property Type As String
        Public Property Message As String
        Public Property Items As IEnumerable(Of Object)

        Public Shadows Function ToString() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
    End Class
#End Region

#Region " Get Schema File Name "

    Public Function GetSchemaFile(ByVal Service As ttServices, ByVal SchemaType As enSchemaType, ByVal Version As String) As String
        Dim sb As StringBuilder = New StringBuilder()

        If Version.Length > 0 Then sb.Append(Version).Append("_")

        Select Case Service
            Case ttServices.AirAvail
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_AirAvailRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_AirAvailRS.xsd").ToString()
            Case ttServices.AirFlifo
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_AirFlifoRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_AirFlifoRS.xsd").ToString()
            Case ttServices.AirPrice
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_AirPriceRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_AirPriceRS.xsd").ToString()
            Case ttServices.AirRules
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_AirRulesRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_AirRulesRS.xsd").ToString()
            Case ttServices.AirSeatMap
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_AirSeatMapRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_AirSeatMapRS.xsd").ToString()
            Case ttServices.LowFare
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_LowFareRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_LowFareRS.xsd").ToString()
            Case ttServices.LowFarePlus
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_LowFarePlusRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_LowFarePlusRS.xsd").ToString()
            Case ttServices.LowFareMatrix
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_LowFareMatrixRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_LowFareMatrixsRS.xsd").ToString()
            Case ttServices.LowFareFlights
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_LowFareFlightsRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_LowFareFlightsRS.xsd").ToString()
            Case ttServices.CarAvail
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CarAvailRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CarAvailRS.xsd").ToString()
            Case ttServices.CarInfo
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CarInfoRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CarInfoRS.xsd").ToString()
            Case ttServices.HotelAvail
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_HotelAvailRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_HotelAvailRS.xsd").ToString()
            Case ttServices.HotelInfo
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_HotelInfoRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_HotelInfoRS.xsd").ToString()
            Case ttServices.HotelSearch
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_HotelSearchRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_HotelSearchRS.xsd").ToString()
            Case ttServices.HotelModify
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_HotelModifyRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_HotelModifyRS.xsd").ToString()
            Case ttServices.PNRRead
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_PNRReadRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_TravelItineraryRS.xsd").ToString()
            Case ttServices.PNRCancel
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_PNRCancelRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_PNRCancelRS.xsd").ToString()
            Case ttServices.TravelBuild
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_TravelItineraryRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_TravelItineraryRS.xsd").ToString()
            Case ttServices.ShowMileage
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_ShowMileageRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_ShowMileageRS.xsd").ToString()
            Case ttServices.CreateSession
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_SessionCreateRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_SessionCreateRS.xsd").ToString()
            Case ttServices.CloseSession
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_SessionCloseRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_SessionCloseRS.xsd").ToString()
            Case ttServices.Cryptic
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CrypticRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CrypticRS.xsd").ToString()
            Case ttServices.CCValid
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CCValidRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CCValidRS.xsd").ToString()
            Case ttServices.CurConv
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CurConvRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CurConvRS.xsd").ToString()
            Case ttServices.TimeDiff
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_TimeDiffRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_TimeDiffRS.xsd").ToString()
            Case ttServices.Native
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_NativeRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_NativeRS.xsd").ToString()
            Case ttServices.IssueTicket
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_IssueTicketRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_IssueTicketRS.xsd").ToString()
            Case ttServices.FareDisplay
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_AirFareDisplayRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_AirFareDisplayRS.xsd").ToString()
            Case ttServices.Queue
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_QueueRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_QueueRS.xsd").ToString()
            Case ttServices.QueueRead
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_QueueReadRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_TravelItineraryRS.xsd").ToString()
            Case ttServices.CruiseCabinAvail
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinAvailRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinAvailRS.xsd").ToString()
            Case ttServices.CruiseCategoryAvail
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCategoryAvailRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCategoryAvailRS.xsd").ToString()
            Case ttServices.CruiseFareAvail
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruiseFareAvailRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruiseFareAvailRS.xsd").ToString()
            Case ttServices.CruiseSailAvail
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruiseSailAvailRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruiseSailAvailRS.xsd").ToString()
            Case ttServices.CruiseCabinHold
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinHoldRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinHoldRS.xsd").ToString()
            Case ttServices.CruiseCabinUnhold
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinUnholdRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinUnholdRS.xsd").ToString()
            Case ttServices.CruiseCreateBooking, ttServices.CruiseModifyBooking
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCreateBookingRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCreateBookingRS.xsd").ToString()
            Case ttServices.CruisePriceBooking
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruisePriceBookingRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruisePriceBookingRS.xsd").ToString()
            Case ttServices.CruiseRead
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruiseReadRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruiseReadRS.xsd").ToString()
            Case ttServices.CruiseCancelBooking
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCancelRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCancelRS.xsd").ToString()
            Case ttServices.CruisePackageAvail
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageAvailRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageAvailRS.xsd").ToString()
            Case ttServices.ETicketVerify
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_ETicketVerifyRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_ETicketVerifyRS.xsd").ToString()
            Case ttServices.InsuranceBook
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_InsuranceBookRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_InsuranceBookRS.xsd").ToString()
            Case ttServices.InsuranceQuote
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_InsuranceQuoteRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_InsuranceQuoteRS.xsd").ToString()
            Case ttServices.CruiseCancelBooking
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCancelBookingRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCancelBookingRS.xsd").ToString()
            Case ttServices.CruiseModifyBooking
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruiseModifyBookingRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruiseModifyBookingRS.xsd").ToString()
            Case ttServices.CruisePackageAvail
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageAvailRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageAvailRS.xsd").ToString()
            Case ttServices.CruiseTransferAvail
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruiseTransferAvailRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruiseTransferAvailRS.xsd").ToString()
            Case ttServices.CruisePackageDesc
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageDescRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageDescRS.xsd").ToString()
            Case ttServices.EncodeDecode
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_EncodeDecodeRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_EncodeDecodeRS.xsd").ToString()
            Case ttServices.CruiseItineraryDesc
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_CruiseItineraryDescRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_CruiseItineraryDescRS.xsd").ToString()
            Case ttServices.AddonAvail
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_AddonAvailRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_AddonAvailRS.xsd").ToString()
            Case ttServices.MultiMessage
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_MultiMessageRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_MultiMessageRS.xsd").ToString()
            Case ttServices.TravelModify
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_TravelModifyRQ.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_TravelItineraryRS.xsd").ToString()
            Case ttServices.TicketVoid
                If SchemaType = enSchemaType.Request Then Return sb.Append(Version).Append("Traveltalk_OTA_VoidTicket.xsd").ToString() Else Return sb.Append(Version).Append("Traveltalk_OTA_TravelItineraryRS.xsd").ToString()
            Case Else
                Return ""
        End Select

    End Function

    Public Function GetSchemaFile(ByVal otaMessage As String, ByVal Version As String) As String
        Dim sb As StringBuilder = New StringBuilder()

        If Version.Length > 0 Then sb.Append("_")

        Select Case otaMessage
            Case "OTA_AirAvailRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_AirAvailRQ.xsd").ToString()
            Case "OTA_AirFlifoRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_AirFlifoRQ.xsd").ToString()
            Case "OTA_AirPriceRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_AirPriceRQ.xsd").ToString()
            Case "OTA_AirRulesRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_AirRulesRQ.xsd").ToString()
            Case "OTA_AirSeatMapRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_AirSeatMapRQ.xsd").ToString()
            Case "OTA_AirLowFareSearchRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_LowFareRQ.xsd").ToString()
            Case "OTA_AirLowFareSearchPlusRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_LowFarePlusRQ.xsd").ToString()
            Case "OTA_AirLowFareMatrixRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_LowFareMatrixRQ.xsd").ToString()
            Case "OTA_AirLowFareFlightsRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_LowFareFlightsRQ.xsd").ToString()
            Case "OTA_VehAvailRateRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CarAvailRQ.xsd").ToString()
            Case "OTA_VehLocDetailRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CarInfoRQ.xsd").ToString()
            Case "OTA_HotelAvailRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_HotelAvailRQ.xsd").ToString()
            Case "OTA_HotelDescriptiveInfoRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_HotelInfoRQ.xsd").ToString()
            Case "OTA_HotelSearchRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_HotelSearchRQ.xsd").ToString()
            Case "OTA_HotelResModifyRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_HotelModifyRQ.xsd").ToString()
            Case "OTA_ReadRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_PNRReadRQ.xsd").ToString()
            Case "OTA_CancelRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_PNRCancelRQ.xsd").ToString()
            Case "OTA_TravelItineraryRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_TravelItineraryRQ.xsd").ToString()
            Case "OTA_ShowMileageRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_ShowMileageRQ.xsd").ToString()
            Case "SessionCreateRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_SessionCreateRQ.xsd").ToString()
            Case "SessionCloseRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_SessionCloseRQ.xsd").ToString()
            Case "CrypticRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CrypticRQ.xsd").ToString()
            Case "OTA_CCValidRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CCValidRQ.xsd").ToString()
            Case "OTA_CurConvRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CurConvRQ.xsd").ToString()
            Case "OTA_TimeDiffRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_TimeDiffRQ.xsd").ToString()
            Case "NativeRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_NativeRQ.xsd").ToString()
            Case "TT_IssueTicketRQ"
                Return sb.Append(Version).Append("Traveltalk_IssueTicketRQ.xsd").ToString()
            Case ttServices.FareDisplay
                Return sb.Append(Version).Append("Traveltalk_OTA_AirFareDisplayRQ.xsd").ToString()
            Case ttServices.Queue
                Return sb.Append(Version).Append("Traveltalk_OTA_QueueRQ.xsd").ToString()
            Case ttServices.QueueRead
                Return sb.Append(Version).Append("Traveltalk_OTA_QueueReadRQ.xsd").ToString()
            Case "OTA_CruiseCabinAvailRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinAvailRQ.xsd").ToString()
            Case "OTA_CruiseCategoryAvailRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCtgAvailRQ.xsd").ToString()
            Case "OTA_CruiseFareAvailRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruiseFareAvailRQ.xsd").ToString()
            Case "OTA_CruiseSailAvailRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruiseSailAvailRQ.xsd").ToString()
            Case "OTA_CruiseCabinHoldRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinHoldRQ.xsd").ToString()
            Case "OTA_CruiseCabinUnholdRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinUnholdRQ.xsd").ToString()
            Case "OTA_CruiseCreateBookingRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCreateBookingRQ.xsd").ToString()
            Case "OTA_CruisePriceBookingRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruisePriceBookingRQ.xsd").ToString()
            Case "OTA_CruiseReadRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruiseReadRQ.xsd").ToString()
            Case "OTA_CruiseReadRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_ETicketVerifyRQ.xsd").ToString()
            Case "OTA_CruiseCancelRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCancelRQ.xsd").ToString()
            Case "OTA_CruisePackageAvailRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruiseReadRQ.xsd").ToString()
            Case "OTA_InsuranceQuoteRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_InsuranceQuoteRQ.xsd").ToString()
            Case "OTA_InsuranceBookRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_InsuranceBookRQ.xsd").ToString()
            Case "OTA_CruiseCancelBookingRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruiseCancelBookingRQ.xsd").ToString()
            Case "OTA_CruiseModifyBookingRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruiseModifyBookingRQ.xsd").ToString()
            Case "OTA_CruisePackageAvailRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageAvailRQ.xsd").ToString()
            Case "OTA_CruiseTransferAvailRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruiseTransferAvailRQ.xsd").ToString()
            Case "OTA_CruisePackageDescRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageDescRQ.xsd").ToString()
            Case "OTA_EncodeDecodeRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_EncodeDecodeRQ.xsd").ToString()
            Case "OTA_CruiseItineraryDescRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_CruiseItineraryDescRQ.xsd").ToString()
            Case "OTA_AddonAvailRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_AddonAvailRQ.xsd").ToString()
            Case "OTA_MultiMessageRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_MultiMessageRQ.xsd").ToString()
            Case "OTA_TravelModifyRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_TravelModifyRQ.xsd").ToString()
            Case "TT_VoidTicketRQ"
                Return sb.Append(Version).Append("Traveltalk_OTA_VoidTicketRQ.xsd").ToString()
            Case Else
                Return ""
        End Select

    End Function

#End Region

End Module
