using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TripXML.Library
{
    public static class modCore
    {
        public static string gXslPath = "";
        public static string XslPath = "";
        public static string LogPath = "";
        public static string SchemaPath = "";
        public static bool Trace;
        public static bool IsCreating = false;
        public static bool NonDirectFlights = false;
        public static int LFSchRequestCount = 0;
        public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) as Configuration;

        #region  Structures 

        public struct ACLFile
        {
            public string ServerName;
        }

        public struct Provider
        {
            public string Name;
            public string PCC;
        }

        public struct TravelTalkCredential
        {
            public static string RequestorID;
            public Provider[] Providers;
            public string System;
            public string UserID;
            public string Password;
        }

        public struct TripXMLProviderSystems
        {
            public string UserID;
            public string System;
            public string URL;
            public string Port;
            public string PCC;
            public string AAAPCC;
            public bool AmadeusTrace;
            public string UserName;
            public string Password;
            public string Profile;
            public string ProfileXML;
            public string ProfileCryptic;
            public string ProfileTicketing;
            public string Origin;
            public bool Trace;
            public OpenType[] OpenTypes;
            public string BLFile;
            public bool AggFilter;
            public bool RebookNextFlight;
            public bool RebookPassive;
            public string FareMessage;
            public string SaveInDB;
            public bool LogNative;
            public bool AmadeusWS;
            public AmadeusWSSchema AmadeusWSSchema;
            public ProviderSession ProviderSession;
            public string GPass;
            public string GReqID;
            public bool SessionPool;
            public string Provider;
            // <Obsolete("SOAP2 Should be used through enum SoapHeader")>_
            public bool SOAP2;
            // <Obsolete("SOAP4 Should be used through enum SoapHeader")>_
            public bool SOAP4;
            public string SOAP4URL;
            public string SOAPAction;
            public enSOAPHeaderType SoapHeader;
            public bool GetStoredFares;
            public bool AddLog;
            public bool CheckBookedFare;
            public string NoOfLowFareFlights;
            // belo given variables are not in local code
            public bool HotelMedia;
            public bool SendEmailToAgency;
            public bool CreateInRHAdmin;
            public bool LFPLight;
            public bool CouponStatus;
            public bool AddLFPStat;
            public string ProxyURL;
            public string HotelVersion;
            public string LogUUID;
            public string SourceMarket;
            // Dim EmployeeProfileVersion As String
            public string CarVersion;
            public bool MiniRules;
            public string AirPriceVersion;
            public string SeatMapVersion;
            public string PriceBookingVersion;
            public string ShowTestHotels;
            public string Language;
            public bool SabreFareSearch;
            public string AdVShop;
        }

        public struct ProviderSession
        {
            public int MaximumCount;
            public int InitialBlockSize;
            public int NextBlockSize;
            public int SessionsUsed;
            public bool MultipleAccess;
            // Dim MultipleCount As Integer
        }

        public struct OpenType
        {
            public string OfficeID;
            public string Agent;
            public string SignIn;
            public string CountryCode;
            public string CurrencyCode;
            public string LanguageCode;
            public string TravelAgentID;
        }

        public struct AmadeusWSSchema
        {
            public string Air_FlightInfo;
            public string Air_FlightInfoReply;
            public string Air_MultiAvailability;
            public string Air_MultiAvailabilityReply;
            public string Air_RebookAirSegment;
            public string Air_RebookAirSegmentReply;
            public string Air_RetrieveSeatMap;
            public string Air_RetrieveSeatMapReply;
            public string Air_SellFromRecommendation;
            public string Air_SellFromRecommendationReply;
            public string Car_InformationImage;
            public string Car_InformationImageReply;
            public string Car_LocationList;
            public string Car_LocationListReply;
            public string Car_MultiAvailability;
            public string Car_MultiAvailabilityReply;
            public string Car_Availability;
            public string Car_AvailabilityReply;
            public string Car_Policy;
            public string Car_PolicyReply;
            public string Car_RateInformationFromAvailability;
            public string Car_RateInformationFromAvailabilityReply;
            public string Car_RateInformationFromCarSegment;
            public string Car_RateInformationFromCarSegmentReply;
            public string Car_Sell;
            public string Car_SellReply;
            public string Car_SingleAvailability;
            public string Car_SingleAvailabilityReply;
            public string Command_Cryptic;
            public string Command_CrypticReply;
            public string Cruise_CancelBooking;
            public string Cruise_CancelBookingReply;
            public string Cruise_ClaimBooking;
            public string Cruise_ClaimBookingReply;
            public string Cruise_CreateBooking;
            public string Cruise_CreateBookingReply;
            public string Cruise_DisplayBusDescription;
            public string Cruise_DisplayBusDescriptionReply;
            public string Cruise_DisplayCabinDescription;
            public string Cruise_DisplayCabinDescriptionReply;
            public string Cruise_DisplayCategoryDescription;
            public string Cruise_DisplayCategoryDescriptionReply;
            public string Cruise_DisplayFareDescription;
            public string Cruise_DisplayFareDescriptionReply;
            public string Cruise_DisplayInclusivePackageDescription;
            public string Cruise_DisplayInclusivePackageDescriptionReply;
            public string Cruise_DisplayItineraryDescription;
            public string Cruise_DisplayItineraryDescriptionReply;
            public string Cruise_DisplayPrePostPackageDescription;
            public string Cruise_DisplayPrePostPackageDescriptionReply;
            public string Cruise_DisplayProductInformation;
            public string Cruise_DisplayProductInformationReply;
            public string Cruise_EnterPassengerInformation;
            public string Cruise_EnterPassengerInformationReply;
            public string Cruise_GetBookingDetails;
            public string Cruise_GetBookingDetailsReply;
            public string Cruise_HoldCabin;
            public string Cruise_HoldCabinReply;
            public string Cruise_ModifyBooking;
            public string Cruise_ModifyBookingReply;
            public string Cruise_PriceBooking;
            public string Cruise_PriceBookingReply;
            public string Cruise_PriceBookingCancellation;
            public string Cruise_PriceBookingCancellationReply;
            public string Cruise_RequestBusAvailability;
            public string Cruise_RequestBusAvailabilityReply;
            public string Cruise_RequestCabinAvailability;
            public string Cruise_RequestCabinAvailabilityReply;
            public string Cruise_RequestCategoryAvailability;
            public string Cruise_RequestCategoryAvailabilityReply;
            public string Cruise_RequestFareAvailability;
            public string Cruise_RequestFareAvailabilityReply;
            public string Cruise_RequestInclusivePackageAvailability;
            public string Cruise_RequestInclusivePackageAvailabilityReply;
            public string Cruise_RequestPrePostPackageAvailability;
            public string Cruise_RequestPrePostPackageAvailabilityReply;
            public string Cruise_RequestSailingAvailability;
            public string Cruise_RequestSailingAvailabilityReply;
            public string Cruise_RequestShoreExcursionAvailability;
            public string Cruise_RequestShoreExcursionAvailabilityReply;
            public string Cruise_RequestSpecialServicesAvailability;
            public string Cruise_RequestSpecialServicesAvailabilityReply;
            public string Cruise_RequestTransferAvailability;
            public string Cruise_RequestTransferAvailabilityReply;
            public string Cruise_SearchBooking;
            public string Cruise_SearchBookingReply;
            public string Cruise_UnholdCabin;
            public string Cruise_UnholdCabinReply;
            public string Doc_DisplayItinerary;
            public string Doc_DisplayItineraryReply;
            public string DocIssuance_IssueTicket;
            public string DocIssuance_IssueTicketReply;
            public string DocRefund_CalculateRefund;
            public string DocRefund_CalculateRefundReply;
            public string DocRefund_IgnoreRefund;
            public string DocRefund_IgnoreRefundReply;
            public string DocRefund_InitRefund;
            public string DocRefund_InitRefundReply;
            public string DocRefund_ProcessRefund;
            public string DocRefund_ProcessRefundReply;
            public string DocRefund_SearchRefundRule;
            public string DocRefund_SearchRefundRuleReply;
            public string DocRefund_UpdateRefund;
            public string DocRefund_UpdateRefundReply;
            public string Fare_CheckRules;
            public string Fare_CheckRulesReply;
            public string Fare_DisplayFaresForCityPair;
            public string Fare_DisplayFaresForCityPairReply;
            public string Fare_GetFareFamilyDescription;
            public string Fare_GetFareFamilyDescriptionReply;
            public string Fare_InformativePricingWithoutPNR;
            public string Fare_InformativePricingWithoutPNRReply;
            public string Fare_InformativeBestPricingWithoutPNR;
            public string Fare_InformativeBestPricingWithoutPNRReply;
            public string Fare_MasterPricerCalendar;
            public string Fare_MasterPricerCalendarReply;
            public string Fare_MasterPricerExpertSearch;
            public string Fare_MasterPricerExpertSearchReply;
            public string Fare_MasterPricerTravelBoardSearch;
            public string Fare_MasterPricerTravelBoardSearchReply;
            public string Fare_MetaPricerCalendar;
            public string Fare_MetaPricerCalendarReply;
            public string Fare_MetaPricerTravelBoardSearch;
            public string Fare_MetaPricerTravelBoardSearchReply;
            public string Fare_PricePNRWithBookingClass;
            public string Fare_PricePNRWithBookingClassReply;
            public string Fare_PricePNRWithLowerFares;
            public string Fare_PricePNRWithLowerFaresReply;
            public string Fare_QuoteItinerary;
            public string Fare_QuoteItineraryReply;
            public string Fare_SellByFareCalendar;
            public string Fare_SellByFareCalendarReply;
            public string Fare_SellByFareSearch;
            public string Fare_SellByFareSearchReply;
            public string Fare_FlexPricerUpsell;
            public string Fare_FlexPricerUpsellReply;
            public string Hotel_MultiSingleAvailability;
            public string Hotel_MultiSingleAvailabilityReply;
            public string Hotel_AvailabilityMultiProperties;
            public string Hotel_AvailabilityMultiPropertiesReply;
            public string Hotel_Features;
            public string Hotel_FeaturesReply;
            public string Hotel_DescriptiveInfo;
            public string Hotel_DescriptiveInfoReply;
            public string Hotel_List;
            public string Hotel_ListReply;
            public string Hotel_RateChange;
            public string Hotel_RateChangeReply;
            public string Hotel_Sell;
            public string Hotel_SellReply;
            public string Hotel_SingleAvailability;
            public string Hotel_SingleAvailabilityReply;
            public string Hotel_StructuredPricing;
            public string Hotel_StructuredPricingReply;
            public string Hotel_Terms;
            public string Hotel_TermsReply;
            public string Hotel_EnhancedSingleAvail;
            public string Hotel_EnhancedSingleAvailReply;
            public string Hotel_MultiAvailability;
            public string Hotel_MultiAvailabilityReply;
            public string Hotel_EnhancedPricing;
            public string Hotel_EnhancedPricingReply;
            public string Hotel_CalendarView;
            public string Hotel_CalendarViewReply;
            public string MiniRule_GetFromPricing;
            public string MiniRule_GetFromPricingReply;
            public string MiniRule_GetFromPricingRec;
            public string MiniRule_GetFromPricingRecReply;
            public string PNR_AddMultiElements;
            public string PNR_Cancel;
            public string PNR_Ignore;
            public string PNR_IgnoreReply;
            public string PNR_List;
            public string PNR_Reply;
            public string PNR_Reply1;
            public string PNR_Retrieve;
            public string PNR_RetrieveByRecLoc;
            public string PNR_RetrieveByRecLocReply;
            public string PNR_TransferOwnership;
            public string PNR_TransferOwnershipReply;
            public string PNR_Split;
            public string PNR_SplitReply;
            public string Profile_CreateUpdateProfile;
            public string Profile_CreateUpdateProfileReply;
            public string Profile_CreateProfile;
            public string Profile_CreateProfileReply;
            public string Profile_UpdateProfile;
            public string Profile_UpdateProfileReply;
            public string Profile_DeleteProfile;
            public string Profile_DeleteProfileReply;
            public string Profile_DeactivateProfile;
            public string Profile_DeactivateProfileReply;
            public string Profile_RetrieveProfile;
            public string Profile_RetrieveProfileReply;
            public string Profile_ProfileReply;
            public string Profile_ReadProfile;
            public string Profile_ReadProfileReply;
            public string Queue_CountTotal;
            public string Queue_CountTotalReply;
            public string Queue_List;
            public string Queue_ListReply;
            public string Queue_MoveItem;
            public string Queue_MoveItemReply;
            public string Queue_PlacePNR;
            public string Queue_PlacePNRReply;
            public string Queue_RemoveItem;
            public string Queue_RemoveItemReply;
            public string QueueMode_ProcessQueue;
            public string QueueMode_ProcessQueueReply;
            public string Security_Authenticate;
            public string Security_AuthenticateReply;
            public string Security_SignOut;
            public string Security_SignOutReply;
            public string Ticket_ATCShopperMasterPricerTravelBoardSearch;
            public string Ticket_ATCShopperMasterPricerTravelBoardSearchReply;
            public string Ticket_CancelDocument;
            public string Ticket_CancelDocumentReply;
            public string Ticket_CheckEligibility;
            public string Ticket_CheckEligibilityReply;
            public string Ticket_CreateTSTFromPricing;
            public string Ticket_CreateTSTFromPricingReply;
            public string Ticket_CreditCardCheck;
            public string Ticket_CreditCardCheckReply;
            public string Ticket_DeleteTST;
            public string Ticket_DeleteTSTReply;
            public string Ticket_DisplayTST;
            public string Ticket_GetPricingOptions;
            public string Ticket_GetPricingOptionsReply;
            public string Ticket_DisplayTSTReply;
            public string Ticket_ProcessETicket;
            public string Ticket_ProcessETicketReply;
            public string Ticket_ProcessEDoc;
            public string Ticket_ProcessEDocReply;
            public string Ticket_RepricePNRWithBookingClass;
            public string Ticket_RepricePNRWithBookingClassReply;
            public string Ticket_UpdateTST;
            public string Ticket_UpdateTSTReply;
            public string Ticket_AutomaticUpdate;
            public string Ticket_AutomaticUpdateReply;
            public string PAY_GenerateVirtualCard;
            public string PAY_ListVirtualCards;
            public string PAY_VirtualCardDetails;
            public string PAY_DeleteVirtualCard;
            public string SalesReports_DisplayQueryReport;
            public string SalesReports_DisplayQueryReportReply;
        }

        #endregion

        #region  Enumerators 

        public enum ttServices
        {
            AirAvail = 1,
            AirFlifo = 2,
            AirPrice = 3,
            AirRules = 4,
            AirSeatMap = 5,
            LowFare = 6,
            LowFarePlus = 7,
            CarAvail = 8,
            CarInfo = 9,
            HotelAvail = 10,
            HotelInfo = 11,
            HotelSearch = 12,
            PNRRead = 13,
            PNRCancel = 14,
            TravelBuild = 15,
            CreateSession = 16,
            CloseSession = 17,
            CurConv = 18,
            CCValid = 19,
            TimeDiff = 20,
            ShowMileage = 21,
            Cryptic = 22,
            CruiseSailAvail = 23,
            CruiseFareAvail = 24,
            CruiseCategoryAvail = 25,
            CruiseCabinAvail = 26,
            CruiseCabinHold = 27,
            CruiseCreateBooking = 28,
            CruisePriceBooking = 29,
            Native = 30,
            HotelModify = 31,
            IssueTicket = 32,
            GeoList = 33,
            FareDisplay = 34,
            Queue = 35,
            QueueRead = 36,
            CruiseCabinUnhold = 37,
            CruiseRead = 38,
            ETicketVerify = 39,
            InsuranceBook = 40,
            InsuranceQuote = 41,
            CruiseCancelBooking = 42,
            CruiseModifyBooking = 43,
            CruisePackageAvail = 44,
            CruiseTransferAvail = 45,
            CruisePackageDesc = 46,
            EncodeDecode = 47,
            CruiseItineraryDesc = 48,
            AddonAvail = 49,
            MultiMessage = 50,
            TravelModify = 51,
            ProductCategories = 52,
            ProductList = 53,
            ProductInfo = 54,
            FormFields = 55,
            SendOrder = 56,
            HotelRatePlanNotif = 57,
            PkgAvail = 58,
            HotelRes = 59,
            PkgBook = 60,
            // Cancel = 61
            Read = 62,
            BookingsRead = 63,
            TourcoGetCities = 64,
            TourcoGetPackages = 65,
            TourcoGetVendors = 66,
            TourcoGetCustom = 67,
            LowFareSchedule = 68,
            Ping = 69,
            Update = 70,
            TransferOwnership = 71,
            StoredFareBuild = 72,
            StoredFareUpdate = 73,
            FareInfo = 74,
            CarRules = 75,
            CrypticEntries = 76,
            AddPNRToAdmin = 77,
            PNRReprice = 78,
            AirSchedule = 79,
            ProfileRead = 80,
            GetDeals = 81,
            ProfileCreate = 82,
            UpdateSessioned = 83,
            PNREnd = 84,
            LowFareMatrix = 85,
            LowFareFlights = 86,
            IssueTicketSessioned = 87,
            AddRecLocToAdmin = 88,
            TicketVoid = 89,
            AddRecLocToNewAdminOnly = 90,
            InventoryManagement = 91,
            TicketDisplay = 92,
            LowOfferMatrix = 93,
            LowOfferSearch = 94,
            SalesReport = 95,
            CarList = 96,
            MiniRule = 97,
            RefundTicket = 98,
            PNRSplit = 99,
            SearchName = 100,
            AirShopping = 101,
            Authorization = 102,
            GenerateVirtualCard = 103,
            CancelVirtualCardLoad = 104,
            DeleteVirtualCard = 105,
            GetVirtualCardDetails = 106,
            ListVirtualCards = 107,
            ManageDBIData = 108,
            ScheduleVirtualCardLoad = 109,
            UpdateVirtualCard = 110,
            ReissueTicket = 111,
            DisplayQueryReport = 112
            // --------------------------------------
        }

        public enum enSchemaType
        {
            Request = 1,
            Response = 2
        }

        public enum enSOAPHeaderType
        {
            SOAP1 = 0,
            SOAP2 = 1,
            SOAP4 = 2
        }


        #endregion

        #region  Format Error Message 

        public static string FormatErrorMessage(ttServices service, string message, TripXMLProviderSystems providerSystems, string recordLocator = "", string OTA_Version = "")
        {
            string strResponse;
            string version;
            string tag;
            switch (service)
            {
                case ttServices.AirAvail:
                    {
                        tag = "OTA_AirAvailRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.AirFlifo:
                    {
                        tag = "OTA_AirFlifoRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.AirPrice:
                    {
                        tag = "OTA_AirPriceRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.AirRules:
                    {
                        tag = "OTA_AirRulesRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.AirSeatMap:
                    {
                        tag = "OTA_AirSeatMapRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.LowFare:
                    {
                        tag = "OTA_AirLowFareSearchRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.LowFarePlus:
                    {
                        tag = "OTA_AirLowFareSearchPlusRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.LowFareMatrix:
                    {
                        tag = "OTA_AirLowFareSearchMatrixRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.LowFareFlights:
                    {
                        tag = "OTA_AirLowFareSearchFlightsRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.LowFareSchedule:
                    {
                        tag = "OTA_AirLowFareSearchScheduleRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.CarAvail:
                    {
                        tag = "OTA_VehAvailRateRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.CarInfo:
                    {
                        tag = "OTA_VehLocDetailRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.HotelAvail:
                    {
                        tag = "OTA_HotelAvailRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.HotelInfo:
                    {
                        tag = "OTA_HotelDescriptiveInfoRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.HotelSearch:
                    {
                        tag = "OTA_HotelSearchRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.PNRRead:
                    {
                        tag = "OTA_TravelItineraryRS";
                        version = "v03";
                        break;
                    }

                case ttServices.PNRCancel:
                    {
                        tag = "OTA_CancelRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.TravelBuild:
                    {
                        tag = "OTA_TravelItineraryRS";
                        version = "v03";
                        break;
                    }

                case ttServices.ShowMileage:
                    {
                        tag = "OTA_ShowMileageRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.CreateSession:
                    {
                        tag = "SessionCreateRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.CloseSession:
                    {
                        tag = "SessionCloseRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.Cryptic:
                    {
                        tag = "CrypticRS";
                        version = "";
                        break;
                    }

                case ttServices.CCValid:
                    {
                        tag = "OTA_CCValidRS";
                        version = "";
                        break;
                    }

                case ttServices.CurConv:
                    {
                        tag = "OTA_CurConvRS";
                        version = "";
                        break;
                    }

                case ttServices.TimeDiff:
                    {
                        tag = "OTA_TimeDiffRS";
                        version = "";
                        break;
                    }

                case ttServices.CruiseCabinAvail:
                    {
                        tag = "OTA_CruiseCabinAvailRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseCategoryAvail:
                    {
                        tag = "OTA_CruiseCategoryAvailRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseFareAvail:
                    {
                        tag = "OTA_CruiseFareAvailRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseSailAvail:
                    {
                        tag = "OTA_CruiseSailAvailRS";
                        version = "1.000";
                        break;
                    }

                case var @case when @case == ttServices.CruiseCabinAvail:
                    {
                        tag = "OTA_CruiseCabinAvailRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseCabinHold:
                    {
                        tag = "OTA_CruiseCabinHoldRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseCabinUnhold:
                    {
                        tag = "OTA_CruiseCabinUnholdRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruisePriceBooking:
                    {
                        tag = "OTA_CruisePriceBookingRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseCreateBooking:
                    {
                        tag = "OTA_CruiseCreateBookingRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseRead:
                    {
                        tag = "OTA_CruiseReadRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseCancelBooking:
                    {
                        tag = "OTA_CruiseCancelRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseModifyBooking:
                    {
                        tag = "OTA_CruiseCreateBookingRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruisePackageAvail:
                    {
                        tag = "OTA_CruisePackageAvailRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.Native:
                    {
                        tag = "NativeRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.HotelModify:
                    {
                        tag = "OTA_TravelItineraryRS";
                        version = "v03";
                        break;
                    }

                case ttServices.IssueTicket:
                    {
                        tag = "TT_IssueTicketRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.GeoList:
                    {
                        tag = "TT_GeoListRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.FareDisplay:
                    {
                        tag = "OTA_AirFareDisplayRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.Queue:
                    {
                        tag = "OTA_QueueRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.QueueRead:
                    {
                        tag = "OTA_TravelItineraryRS";
                        if (!string.IsNullOrEmpty(OTA_Version))
                        {
                            version = OTA_Version;
                        }
                        else
                        {
                            version = "v03";
                        }

                        break;
                    }

                case ttServices.ETicketVerify:
                    {
                        tag = "OTA_ETicketVerifyRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.InsuranceBook:
                    {
                        tag = "OTA_InsuranceBookRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.InsuranceQuote:
                    {
                        tag = "OTA_InsuranceQuoteRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.CruiseItineraryDesc:
                    {
                        tag = "OTA_CruiseItineraryAvailRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.AddonAvail:
                    {
                        tag = "OTA_AddonAvailRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.TravelModify:
                    {
                        tag = "OTA_TravelItineraryRS";
                        version = "v03";
                        break;
                    }

                case ttServices.Update:
                    {
                        tag = "OTA_TravelItineraryRS";
                        version = "v03";
                        break;
                    }

                case ttServices.UpdateSessioned:
                    {
                        tag = "OTA_TravelItineraryRS";
                        version = "v03";
                        break;
                    }

                case ttServices.PNRSplit:
                    {
                        tag = "OTA_TravelItineraryRS";
                        version = "v03";
                        break;
                    }

                case ttServices.IssueTicketSessioned:
                    {
                        tag = "TT_IssueTicketRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.PNRReprice:
                    {
                        tag = "OTA_PNRRepriceRS";
                        version = "v03";
                        break;
                    }

                default:
                    {
                        tag = "";
                        version = "1.001";
                        break;
                    }
            }

            if (!string.IsNullOrEmpty(OTA_Version))
                version = OTA_Version;
            var lstError = new List<string>();
            if (!message.Contains(Environment.NewLine))
            {
                lstError.Add(message);
            }
            else
            {
                lstError.AddRange(message.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList());
            }

            var trace = new JObject(new JProperty("@Status", tag.Contains("OTA_CancelRS") ? "" : "Unsuccessful"), new JProperty("@Version", version), new JProperty("@TransactionIdentifier", providerSystems.Provider), new JProperty("@UniqueID", !string.IsNullOrEmpty(recordLocator) ? recordLocator : ""), new JProperty("@TimeStamp", DateTime.Now), new JProperty("Errors", GetListToJSON(lstError)));
            AddLog(LogType.Error, ref tag, providerSystems, trace);
            string jsonTrace = JsonConvert.SerializeObject(trace);
            var doc = JsonConvert.DeserializeXmlNode(jsonTrace, tag); // strResponse
            strResponse = doc.InnerXml;
            return strResponse;
        }

        public static string FormatErrorMessage(ttServices Service, string Message, string Provider, string RecordLocator = "", bool MessageIsNode = false, string OTA_Version = "")
        {
            string strResponse = "";
            string version = "";
            string tag = "";
            switch (Service)
            {
                case ttServices.AirAvail:
                    {
                        tag = "OTA_AirAvailRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.AirFlifo:
                    {
                        tag = "OTA_AirFlifoRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.AirPrice:
                    {
                        tag = "OTA_AirPriceRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.AirRules:
                    {
                        tag = "OTA_AirRulesRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.AirSeatMap:
                    {
                        tag = "OTA_AirSeatMapRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.LowFare:
                    {
                        tag = "OTA_AirLowFareSearchRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.LowFarePlus:
                    {
                        tag = "OTA_AirLowFareSearchPlusRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.LowFareMatrix:
                    {
                        tag = "OTA_AirLowFareSearchMatrixRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.LowFareFlights:
                    {
                        tag = "OTA_AirLowFareSearchFlightsRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.LowFareSchedule:
                    {
                        tag = "OTA_AirLowFareSearchScheduleRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.CarAvail:
                    {
                        tag = "OTA_VehAvailRateRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.CarInfo:
                    {
                        tag = "OTA_VehLocDetailRS";
                        version = "2.000";
                        break;
                    }

                case ttServices.HotelAvail:
                    {
                        tag = "OTA_HotelAvailRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.HotelInfo:
                    {
                        tag = "OTA_HotelDescriptiveInfoRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.HotelSearch:
                    {
                        tag = "OTA_HotelSearchRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.PNRRead:
                    {
                        tag = "OTA_TravelItineraryRS";
                        version = "v03";
                        break;
                    }

                case ttServices.PNRReprice:
                    {
                        tag = "OTA_PNRRepriceRS";
                        version = "v03";
                        break;
                    }

                case ttServices.PNRCancel:
                    {
                        tag = "OTA_CancelRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.TravelBuild:
                    {
                        tag = "OTA_TravelItineraryRS";
                        version = "v03";
                        break;
                    }

                case ttServices.ShowMileage:
                    {
                        tag = "OTA_ShowMileageRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.CreateSession:
                    {
                        tag = "SessionCreateRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.CloseSession:
                    {
                        tag = "SessionCloseRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.Cryptic:
                    {
                        tag = "CrypticRS";
                        version = "";
                        break;
                    }

                case ttServices.CCValid:
                    {
                        tag = "OTA_CCValidRS";
                        version = "";
                        break;
                    }

                case ttServices.CurConv:
                    {
                        tag = "OTA_CurConvRS";
                        version = "";
                        break;
                    }

                case ttServices.TimeDiff:
                    {
                        tag = "OTA_TimeDiffRS";
                        version = "";
                        break;
                    }

                case ttServices.CruiseCabinAvail:
                    {
                        tag = "OTA_CruiseCabinAvailRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseCategoryAvail:
                    {
                        tag = "OTA_CruiseCategoryAvailRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseFareAvail:
                    {
                        tag = "OTA_CruiseFareAvailRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseSailAvail:
                    {
                        tag = "OTA_CruiseSailAvailRS";
                        version = "1.000";
                        break;
                    }

                case var @case when @case == ttServices.CruiseCabinAvail:
                    {
                        tag = "OTA_CruiseCabinAvailRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseCabinHold:
                    {
                        tag = "OTA_CruiseCabinHoldRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseCabinUnhold:
                    {
                        tag = "OTA_CruiseCabinUnholdRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruisePriceBooking:
                    {
                        tag = "OTA_CruisePriceBookingRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseCreateBooking:
                    {
                        tag = "OTA_CruiseCreateBookingRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseRead:
                    {
                        tag = "OTA_CruiseReadRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseCancelBooking:
                    {
                        tag = "OTA_CruiseCancelRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruiseModifyBooking:
                    {
                        tag = "OTA_CruiseCreateBookingRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.CruisePackageAvail:
                    {
                        tag = "OTA_CruisePackageAvailRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.Native:
                    {
                        tag = "NativeRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.HotelModify:
                    {
                        tag = "OTA_TravelItineraryRS";
                        version = "v03";
                        break;
                    }

                case ttServices.IssueTicket:
                    {
                        tag = "TT_IssueTicketRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.GeoList:
                    {
                        tag = "TT_GeoListRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.FareDisplay:
                    {
                        tag = "OTA_AirFareDisplayRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.Queue:
                    {
                        tag = "OTA_QueueRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.QueueRead:
                    {
                        tag = "OTA_TravelItineraryRS";
                        if (!string.IsNullOrEmpty(OTA_Version))
                        {
                            version = OTA_Version;
                        }
                        else
                        {
                            version = "v03";
                        }

                        break;
                    }

                case ttServices.ETicketVerify:
                    {
                        tag = "OTA_ETicketVerifyRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.InsuranceBook:
                    {
                        tag = "OTA_InsuranceBookRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.InsuranceQuote:
                    {
                        tag = "OTA_InsuranceQuoteRS";
                        version = "1.001";
                        break;
                    }

                case ttServices.CruiseItineraryDesc:
                    {
                        tag = "OTA_CruiseItineraryAvailRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.AddonAvail:
                    {
                        tag = "OTA_AddonAvailRS";
                        version = "1.000";
                        break;
                    }

                case ttServices.TravelModify:
                    {
                        tag = "OTA_TravelItineraryRS";
                        version = "v03";
                        break;
                    }

                case ttServices.Update:
                    {
                        tag = "OTA_TravelItineraryRS";
                        version = "v03";
                        break;
                    }

                case ttServices.UpdateSessioned:
                    {
                        tag = "OTA_TravelItineraryRS";
                        version = "v03";
                        break;
                    }

                case ttServices.PNRSplit:
                    {
                        tag = "OTA_TravelItineraryRS";
                        version = "v03";
                        break;
                    }

                case ttServices.IssueTicketSessioned:
                    {
                        tag = "TT_IssueTicketRS";
                        version = "1.001";
                        break;
                    }
            }

            if (!string.IsNullOrEmpty(OTA_Version))
                version = OTA_Version;
            var lstError = new List<string>();
            if (MessageIsNode)
            {
                lstError.Add(Message);
            }
            else
            {
                lstError.AddRange(Message.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList());
            }

            var ps = new TripXMLProviderSystems();
            ps.UserName = "";
            ps.UserID = "";
            ps.Provider = Provider;
            var trace = new JObject(new JProperty("Status", tag.Contains("OTA_CancelRS") ? "" : "Unsuccessful"), new JProperty("@Version", version), new JProperty("@AltLangID", !string.IsNullOrEmpty(Provider) ? Provider : string.Empty), new JProperty("@UniqueID", !string.IsNullOrEmpty(RecordLocator) ? RecordLocator : ""), new JProperty("@TimeStamp", DateTime.Now), new JProperty("Errors", GetListToJSON(lstError)));
            AddLog(LogType.Error, ref tag, ps, trace);
            string jsonTrace = JsonConvert.SerializeObject(trace);
            var doc = JsonConvert.DeserializeXmlNode(jsonTrace, tag); // strResponse
            strResponse = doc.InnerXml;
            return strResponse;
        }

        private static JArray GetListToJSON(List<string> list)
        {
            // For i = 0 To arError.GetLength(0) - 1
            // sb.Append("<Error Type=""E"">").Append(arError(i)).Append("</Error>")
            // Next

            // <OTA_TravelItineraryRS Version="v03" TransactionIdentifier="Amadeus"><Errors><Error Type="E">/QUEUE CATEGORY EMPTY</Error></Errors></OTA_TravelItineraryRS>
            try
            {
                var elements = new JArray(from cont in list
                                          select new JObject(new JProperty("Error", new JObject(new JProperty("@Type", "E"), new JProperty("#text", cont)))));
                return elements;
            }
            catch (Exception ex)
            {
                return (JArray)JsonConvert.DeserializeObject(string.Format("<Error Type='E'>{0}</Error>", string.Join(",", list)));
            }
        }

        #endregion

        #region Logging

        public static void AddLog(LogType logType, ref string message, TripXMLProviderSystems providerSystems)
        {
            var lg = new Log();
            lg.Message = message;
            lg.Type = logType.ToString();
            lg.UserName = providerSystems.UserName;
            lg.UserID = providerSystems.UserID;
            lg.Provider = providerSystems.Provider;
            AddLog(lg);
        }

        private static void AddLog(Log log)
        {
            int fileNumber;
            // Dim DirPath As String = "C:\\TripXML\\log"
            try
            {
                string filePath = string.Format(@"{0}\\{1}_{2}.log", config.GetSection("TripXMLLogFolder"), log.UserName, DateTime.Today.ToString("dd-MM-yyyy"));
                //fileNumber = FileSystem.FreeFile();
                //FileSystem.FileOpen(fileNumber, filePath, OpenMode.Append);
                //FileSystem.PrintLine(fileNumber, log.ToString());
                //FileSystem.FileClose(fileNumber);

                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(log.ToString());
                }
            }
            catch (Exception ex)
            {
                // 
            }
        }

        public static void AddLog(ref string message, string username)
        {
            int fileNumber;
            string strPath = config.GetSection("TripXMLLogFolder").ToString();
            try
            {
                string filePath = $@"\\{username}_{DateTime.Today.ToString("dd - MM - yyyy")}";
                filePath = $"{strPath}{filePath}.log";

                //fileNumber = FileSystem.FreeFile();
                //FileSystem.FileOpen(fileNumber, filePath, OpenMode.Append);
                //FileSystem.PrintLine(fileNumber, message);
                //FileSystem.FileClose(fileNumber);
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(message);
                }
            }
            catch (Exception ex)
            {
                // 
            }
        }

        public static void AddLog_old(ref string message, string Username)
        {
            int fileNumber;
            try
            {
                string filePath = $@"log\\{Username}_{DateTime.Today.ToString("dd-MM-yyyy")}";
                filePath = $@"C:\\TripXML\\{filePath}.txt";

                //fileNumber = FileSystem.FreeFile();
                //FileSystem.FileOpen(fileNumber, filePath, OpenMode.Append);
                //FileSystem.PrintLine(fileNumber, message);
                //FileSystem.FileClose(fileNumber);
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(message);
                }
            }
            catch (Exception ex)
            {
                // 
            }
        }

        public static void AddLog(LogType logType, ref string message, TripXMLProviderSystems providerSystems, object items)
        {
            var lg = new Log();
            lg.Message = message;
            lg.Type = logType.ToString();
            lg.UserName = providerSystems.UserName;
            lg.UserID = providerSystems.UserID;
            lg.Provider = providerSystems.Provider;
            lg.Items = new List<object>() { items };
            lg.Resourse = Assembly.GetExecutingAssembly().GetName().Name;
            AddLog(lg);
        }

        public enum LogType
        {
            Info = 1,
            Error = 2,
            Success = 3
        }

        public class Log
        {
            /// <summary>
            /// TransactionIdentifier
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks>Amadeus, Sabre</remarks>
            public string Provider { get; set; }
            public string RecordLocator { get; set; }
            public string UserName { get; set; }
            public string UserID { get; set; }
            public string Version { get; set; }
            public string Resourse { get; set; }
            public string Type { get; set; }
            public string Message { get; set; }
            public IEnumerable<object> Items { get; set; }

            public new string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
        #endregion

        #region  Get Schema File Name 

        public static string GetSchemaFile(ttServices Service, enSchemaType SchemaType, string Version)
        {
            var sb = new StringBuilder();
            if (Version.Length > 0)
                sb.Append(Version).Append("_");
            switch (Service)
            {
                case ttServices.AirAvail:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_AirAvailRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_AirAvailRS.xsd").ToString();
                        break;
                    }

                case ttServices.AirFlifo:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_AirFlifoRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_AirFlifoRS.xsd").ToString();
                        break;
                    }

                case ttServices.AirPrice:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_AirPriceRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_AirPriceRS.xsd").ToString();
                        break;
                    }

                case ttServices.AirRules:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_AirRulesRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_AirRulesRS.xsd").ToString();
                        break;
                    }

                case ttServices.AirSeatMap:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_AirSeatMapRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_AirSeatMapRS.xsd").ToString();
                        break;
                    }

                case ttServices.LowFare:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_LowFareRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_LowFareRS.xsd").ToString();
                        break;
                    }

                case ttServices.LowFarePlus:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_LowFarePlusRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_LowFarePlusRS.xsd").ToString();
                        break;
                    }

                case ttServices.LowFareMatrix:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_LowFareMatrixRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_LowFareMatrixsRS.xsd").ToString();
                        break;
                    }

                case ttServices.LowFareFlights:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_LowFareFlightsRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_LowFareFlightsRS.xsd").ToString();
                        break;
                    }

                case ttServices.CarAvail:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CarAvailRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CarAvailRS.xsd").ToString();
                        break;
                    }

                case ttServices.CarInfo:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CarInfoRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CarInfoRS.xsd").ToString();
                        break;
                    }

                case ttServices.HotelAvail:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_HotelAvailRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_HotelAvailRS.xsd").ToString();
                        break;
                    }

                case ttServices.HotelInfo:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_HotelInfoRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_HotelInfoRS.xsd").ToString();
                        break;
                    }

                case ttServices.HotelSearch:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_HotelSearchRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_HotelSearchRS.xsd").ToString();
                        break;
                    }

                case ttServices.HotelModify:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_HotelModifyRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_HotelModifyRS.xsd").ToString();
                        break;
                    }

                case ttServices.PNRRead:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_PNRReadRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_TravelItineraryRS.xsd").ToString();
                        break;
                    }

                case ttServices.PNRCancel:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_PNRCancelRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_PNRCancelRS.xsd").ToString();
                        break;
                    }

                case ttServices.TravelBuild:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_TravelItineraryRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_TravelItineraryRS.xsd").ToString();
                        break;
                    }

                case ttServices.ShowMileage:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_ShowMileageRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_ShowMileageRS.xsd").ToString();
                        break;
                    }

                case ttServices.CreateSession:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_SessionCreateRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_SessionCreateRS.xsd").ToString();
                        break;
                    }

                case ttServices.CloseSession:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_SessionCloseRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_SessionCloseRS.xsd").ToString();
                        break;
                    }

                case ttServices.Cryptic:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CrypticRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CrypticRS.xsd").ToString();
                        break;
                    }

                case ttServices.CCValid:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CCValidRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CCValidRS.xsd").ToString();
                        break;
                    }

                case ttServices.CurConv:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CurConvRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CurConvRS.xsd").ToString();
                        break;
                    }

                case ttServices.TimeDiff:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_TimeDiffRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_TimeDiffRS.xsd").ToString();
                        break;
                    }

                case ttServices.Native:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_NativeRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_NativeRS.xsd").ToString();
                        break;
                    }

                case ttServices.IssueTicket:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_IssueTicketRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_IssueTicketRS.xsd").ToString();
                        break;
                    }

                case ttServices.FareDisplay:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_AirFareDisplayRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_AirFareDisplayRS.xsd").ToString();
                        break;
                    }

                case ttServices.Queue:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_QueueRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_QueueRS.xsd").ToString();
                        break;
                    }

                case ttServices.QueueRead:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_QueueReadRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_TravelItineraryRS.xsd").ToString();
                        break;
                    }

                case ttServices.CruiseCabinAvail:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinAvailRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinAvailRS.xsd").ToString();
                        break;
                    }

                case ttServices.CruiseCategoryAvail:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseCategoryAvailRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseCategoryAvailRS.xsd").ToString();
                        break;
                    }

                case ttServices.CruiseFareAvail:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseFareAvailRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseFareAvailRS.xsd").ToString();
                        break;
                    }

                case ttServices.CruiseSailAvail:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseSailAvailRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseSailAvailRS.xsd").ToString();
                        break;
                    }

                case ttServices.CruiseCabinHold:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinHoldRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinHoldRS.xsd").ToString();
                        break;
                    }

                case ttServices.CruiseCabinUnhold:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinUnholdRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinUnholdRS.xsd").ToString();
                        break;
                    }

                case ttServices.CruiseCreateBooking:
                case ttServices.CruiseModifyBooking:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseCreateBookingRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseCreateBookingRS.xsd").ToString();
                        break;
                    }

                case ttServices.CruisePriceBooking:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruisePriceBookingRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruisePriceBookingRS.xsd").ToString();
                        break;
                    }

                case ttServices.CruiseRead:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseReadRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseReadRS.xsd").ToString();
                        break;
                    }

                case ttServices.CruiseCancelBooking:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseCancelRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseCancelRS.xsd").ToString();
                        break;
                    }

                case ttServices.CruisePackageAvail:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageAvailRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageAvailRS.xsd").ToString();
                        break;
                    }

                case ttServices.ETicketVerify:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_ETicketVerifyRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_ETicketVerifyRS.xsd").ToString();
                        break;
                    }

                case ttServices.InsuranceBook:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_InsuranceBookRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_InsuranceBookRS.xsd").ToString();
                        break;
                    }

                case ttServices.InsuranceQuote:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_InsuranceQuoteRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_InsuranceQuoteRS.xsd").ToString();
                        break;
                    }

                case var @case when @case == ttServices.CruiseCancelBooking:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseCancelBookingRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseCancelBookingRS.xsd").ToString();
                        break;
                    }

                case var case1 when case1 == ttServices.CruiseModifyBooking:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseModifyBookingRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseModifyBookingRS.xsd").ToString();
                        break;
                    }

                case var case2 when case2 == ttServices.CruisePackageAvail:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageAvailRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageAvailRS.xsd").ToString();
                        break;
                    }

                case ttServices.CruiseTransferAvail:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseTransferAvailRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseTransferAvailRS.xsd").ToString();
                        break;
                    }

                case ttServices.CruisePackageDesc:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageDescRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageDescRS.xsd").ToString();
                        break;
                    }

                case ttServices.EncodeDecode:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_EncodeDecodeRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_EncodeDecodeRS.xsd").ToString();
                        break;
                    }

                case ttServices.CruiseItineraryDesc:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseItineraryDescRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_CruiseItineraryDescRS.xsd").ToString();
                        break;
                    }

                case ttServices.AddonAvail:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_AddonAvailRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_AddonAvailRS.xsd").ToString();
                        break;
                    }

                case ttServices.MultiMessage:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_MultiMessageRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_MultiMessageRS.xsd").ToString();
                        break;
                    }

                case ttServices.TravelModify:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_TravelModifyRQ.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_TravelItineraryRS.xsd").ToString();
                        break;
                    }

                case ttServices.TicketVoid:
                    {
                        if (SchemaType == enSchemaType.Request)
                            return sb.Append(Version).Append("Traveltalk_OTA_VoidTicket.xsd").ToString();
                        else
                            return sb.Append(Version).Append("Traveltalk_OTA_TravelItineraryRS.xsd").ToString();
                        break;
                    }

                default:
                    {
                        return "";
                    }
            }
        }

        public static string GetSchemaFile(string otaMessage, string Version)
        {
            var sb = new StringBuilder();
            if (Version.Length > 0)
                sb.Append("_");
            switch (otaMessage ?? "")
            {
                case "OTA_AirAvailRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_AirAvailRQ.xsd").ToString();
                    }

                case "OTA_AirFlifoRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_AirFlifoRQ.xsd").ToString();
                    }

                case "OTA_AirPriceRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_AirPriceRQ.xsd").ToString();
                    }

                case "OTA_AirRulesRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_AirRulesRQ.xsd").ToString();
                    }

                case "OTA_AirSeatMapRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_AirSeatMapRQ.xsd").ToString();
                    }

                case "OTA_AirLowFareSearchRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_LowFareRQ.xsd").ToString();
                    }

                case "OTA_AirLowFareSearchPlusRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_LowFarePlusRQ.xsd").ToString();
                    }

                case "OTA_AirLowFareMatrixRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_LowFareMatrixRQ.xsd").ToString();
                    }

                case "OTA_AirLowFareFlightsRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_LowFareFlightsRQ.xsd").ToString();
                    }

                case "OTA_VehAvailRateRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CarAvailRQ.xsd").ToString();
                    }

                case "OTA_VehLocDetailRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CarInfoRQ.xsd").ToString();
                    }

                case "OTA_HotelAvailRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_HotelAvailRQ.xsd").ToString();
                    }

                case "OTA_HotelDescriptiveInfoRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_HotelInfoRQ.xsd").ToString();
                    }

                case "OTA_HotelSearchRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_HotelSearchRQ.xsd").ToString();
                    }

                case "OTA_HotelResModifyRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_HotelModifyRQ.xsd").ToString();
                    }

                case "OTA_ReadRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_PNRReadRQ.xsd").ToString();
                    }

                case "OTA_CancelRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_PNRCancelRQ.xsd").ToString();
                    }

                case "OTA_TravelItineraryRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_TravelItineraryRQ.xsd").ToString();
                    }

                case "OTA_ShowMileageRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_ShowMileageRQ.xsd").ToString();
                    }

                case "SessionCreateRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_SessionCreateRQ.xsd").ToString();
                    }

                case "SessionCloseRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_SessionCloseRQ.xsd").ToString();
                    }

                case "CrypticRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CrypticRQ.xsd").ToString();
                    }

                case "OTA_CCValidRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CCValidRQ.xsd").ToString();
                    }

                case "OTA_CurConvRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CurConvRQ.xsd").ToString();
                    }

                case "OTA_TimeDiffRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_TimeDiffRQ.xsd").ToString();
                    }

                case "NativeRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_NativeRQ.xsd").ToString();
                    }

                case "TT_IssueTicketRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_IssueTicketRQ.xsd").ToString();
                    }

                case "34":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_AirFareDisplayRQ.xsd").ToString();
                    }

                case "35":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_QueueRQ.xsd").ToString();
                    }

                case "36":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_QueueReadRQ.xsd").ToString();
                    }

                case "OTA_CruiseCabinAvailRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinAvailRQ.xsd").ToString();
                    }

                case "OTA_CruiseCategoryAvailRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruiseCtgAvailRQ.xsd").ToString();
                    }

                case "OTA_CruiseFareAvailRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruiseFareAvailRQ.xsd").ToString();
                    }

                case "OTA_CruiseSailAvailRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruiseSailAvailRQ.xsd").ToString();
                    }

                case "OTA_CruiseCabinHoldRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinHoldRQ.xsd").ToString();
                    }

                case "OTA_CruiseCabinUnholdRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruiseCabinUnholdRQ.xsd").ToString();
                    }

                case "OTA_CruiseCreateBookingRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruiseCreateBookingRQ.xsd").ToString();
                    }

                case "OTA_CruisePriceBookingRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruisePriceBookingRQ.xsd").ToString();
                    }

                case "OTA_CruiseReadRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruiseReadRQ.xsd").ToString();
                    }

                case var @case when @case == "OTA_CruiseReadRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_ETicketVerifyRQ.xsd").ToString();
                    }

                case "OTA_CruiseCancelRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruiseCancelRQ.xsd").ToString();
                    }

                case "OTA_CruisePackageAvailRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruiseReadRQ.xsd").ToString();
                    }

                case "OTA_InsuranceQuoteRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_InsuranceQuoteRQ.xsd").ToString();
                    }

                case "OTA_InsuranceBookRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_InsuranceBookRQ.xsd").ToString();
                    }

                case "OTA_CruiseCancelBookingRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruiseCancelBookingRQ.xsd").ToString();
                    }

                case "OTA_CruiseModifyBookingRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruiseModifyBookingRQ.xsd").ToString();
                    }

                case var case1 when case1 == "OTA_CruisePackageAvailRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageAvailRQ.xsd").ToString();
                    }

                case "OTA_CruiseTransferAvailRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruiseTransferAvailRQ.xsd").ToString();
                    }

                case "OTA_CruisePackageDescRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruisePackageDescRQ.xsd").ToString();
                    }

                case "OTA_EncodeDecodeRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_EncodeDecodeRQ.xsd").ToString();
                    }

                case "OTA_CruiseItineraryDescRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_CruiseItineraryDescRQ.xsd").ToString();
                    }

                case "OTA_AddonAvailRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_AddonAvailRQ.xsd").ToString();
                    }

                case "OTA_MultiMessageRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_MultiMessageRQ.xsd").ToString();
                    }

                case "OTA_TravelModifyRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_TravelModifyRQ.xsd").ToString();
                    }

                case "TT_VoidTicketRQ":
                    {
                        return sb.Append(Version).Append("Traveltalk_OTA_VoidTicketRQ.xsd").ToString();
                    }

                default:
                    {
                        return "";
                    }
            }
        }

        #endregion

    }
}
