using System.Xml;
using TripXMLMain;
using System.Text;
using System;
using System.Globalization;
using System.Linq;

namespace AmadeusWS
{
    public class OtherServices : AmadeusBase
    {
        public OtherServices()
        {
            Request = string.Empty;
            Version = string.Empty;
        }

        public bool IsSOAP2 { get; set; }

        public bool IsSOAP4 { get; set; }

        public string CreateSession()
        {
            try
            {
                var ttAA = SetAdapter();
                SetConversationID(ttAA);

                //  Build Response.
                string strResponse = $"<SessionCreateRS Version=\'1.001\'><Success/><ConversationID>{ConversationID}</ConversationID></SessionCreateRS>";
                return strResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Session was not Created.\r\n{ex.Message}");
            }
        }

        public string CloseSession()
        {
            //  *******************
            //  Close Session    *
            //  *******************
            try
            {
                var ttAA = SetAdapter();
                SetConversationID(ttAA);

                if (string.IsNullOrEmpty(ConversationID))
                    throw new Exception("ConversationID is missing in the Request.");

                var resp = IsSOAP2 ? ttAA.CloseSession(ConversationID) : ttAA.CloseSoap4Session(ConversationID);

                //  Build Response.
                return String.Format("<SessionCloseRS Version=\'1.001\'><Success/></SessionCloseRS>", resp);
            }
            catch (Exception ex)
            {
                throw new Exception($"Session was not Closed.\r\n{ex.Message}");
            }
        }

        public string ShowMileage()
        {
            string strResponse;
            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                string strRequest = SetRequest("AmadeusWS_ShowMileageRQ.xsl");               

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = SendGDSMessage(ttAA, strRequest, "HSFREQ_07_3_1A", "HSFRES_07_3_1A");
                
                // *****************************************************************
                //  Transform Native Amadeus ShowMileage Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = "</Command_CrypticReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_ShowMileageRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.ShowMileage, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }

        public string Cryptic()
        {             
            string strResponse;

            try
            {
                string strAinBL = "";
                #region business logic
                if (!string.IsNullOrEmpty(ttProviderSystems.BLFile))
                {
                    var oDocBL = new XmlDocument();
                    oDocBL.Load(ttProviderSystems.BLFile);

                    XmlElement oRootBL = oDocBL.DocumentElement;
                    XmlNode oNodeBL = oRootBL.SelectSingleNode($"Security/ProviderBL[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\']");

                    if (oNodeBL != null)
                    {
                        if (oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']") != null)
                        {
                            bool bAllowCryptic = Convert.ToBoolean(oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']/@Cryptic").InnerXml);
                            strAinBL = oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']/@AuthorizeCode").InnerXml;

                            //compare to the one in BL file 
                            if (!bAllowCryptic)
                                throw new Exception("Secured transaction");
                        }
                    }
                }
                #endregion

                // ************************************************************
                //  Transform OTA Cryptic Request into Native Amadeus Request *
                // ************************************************************ 
                string strRequest = SetRequest("AmadeusWS_CrypticRQ.xsl");
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");
                              
                try
                {
                    // *******************************************************************************
                    //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                    // ******************************************************************************* 

                    strResponse = SendRequestCryptically(ttAA, strRequest);
                    CoreLib.SendTrace(ttProviderSystems.UserID, "Cryptic", "Getting Native Response", strResponse, ttProviderSystems.LogUUID);

                    if (strResponse.Contains("|Session|Inactive conversation"))
                        CoreLib.SendTrace(ttProviderSystems.UserID, "Cryptic", "Getting Native Second Response", strResponse, ttProviderSystems.LogUUID);

                    if (strResponse.Contains("NEED RECEIVED FROM"))
                        strResponse = strResponse.Replace("/", "").Replace("\r\n", "");

                    if (inSession)
                        strResponse = strResponse.Replace("</Command_CrypticReply>", $"<ConversationID>{ConversationID}</ConversationID></Command_CrypticReply>");
                    
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CrypticRS.xsl");

                    // ****************************************************************
                    //  Transform Native Amadeus Cryptic Response into OTA Response   *
                    // **************************************************************** 
                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(strResponse);
                    var oRoot = oDoc.DocumentElement;
                    if (!(oRoot.SelectSingleNode("Response") == null))
                    {
                        string strScreen = formatAmadeus(oRoot.SelectSingleNode("Response").InnerText);
                        if (!string.IsNullOrEmpty(strScreen))
                        {
                            strResponse = strResponse.Insert(strResponse.IndexOf("</Response>") + "</Response>".Length, $"<Screen>{strScreen}</Screen>");
                            strResponse = strResponse.Replace("&", "&amp;");
                        }

                        if (!string.IsNullOrEmpty(strAinBL))
                        {
                            var oDocBL = new XmlDocument();
                            oDocBL.LoadXml(strResponse);
                            XmlElement oRootBL = oDocBL.DocumentElement;
                            XmlNode oNodeBL = oRootBL.SelectSingleNode("Screen/Line[position()=2]");

                            if (oNodeBL != null)
                            {
                                if (oNodeBL.InnerXml.StartsWith("RP/"))
                                {
                                    string recloc = oNodeBL.InnerXml.Substring(57, 6);
                                    strRequest = $"<PNR_RetrieveByRecLoc><sbrRecLoc><reservation><controlNumber>{recloc}</controlNumber></reservation></sbrRecLoc></PNR_RetrieveByRecLoc>";
                                    string strResp = SendRequestCryptically(ttAA, strRequest);

                                    var oDocRespBL = new XmlDocument();
                                    oDocRespBL.LoadXml(strResp);

                                    XmlElement oRootRespBL = oDocRespBL.DocumentElement;
                                    XmlNode oNodeRespBL = oRootRespBL.SelectSingleNode("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']");

                                    if (oNodeRespBL != null)
                                    {
                                        string strAIAN = oNodeRespBL.SelectSingleNode("accounting/account/number").InnerXml;

                                        //compare to the one in BL file 
                                        if (strAIAN != strAinBL)
                                            throw new Exception("Secured PNR");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error Transforming Native Response", ex);
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Cryptic, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }

        public string SalesReport()
        {
            string strResponse;           

            try
            {
                // ************************************************************
                //  Transform SalesReport Request into Native Amadeus Request *
                // ************************************************************ 
                string strRequest = SetRequest("AmadeusWS_SalesReportRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);
                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 

                XmlDocument oDocA = new XmlDocument();
                oDocA.LoadXml(strRequest);
                XmlElement oRootA = oDocA.DocumentElement;

                strResponse = SendDisplayQueryReport(ttAA, strRequest);
                strResponse = strResponse.Replace("/$", "");

                // ****************************************************************
                //  Transform Native Amadeus SalesReport Response into Response   *
                // **************************************************************** 
                try
                {
                    var tagToReplace = strResponse.Contains("Errors") ? "</Errors>" : "</SalesReports_DisplayQueryReportReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSAdapter", "Response to transform", strResponse, ttProviderSystems.LogUUID);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_SalesReportRS.xsl", false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.SalesReport, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }

        public string CreditCardValid()
        {   
            string strResponse;
            try
            {
                string strRequest = SetRequest("AmadeusWS_CCValidRQ.xsl");
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // *******************************************************************************                 
                strResponse = SendGDSMessage(ttAA, strRequest, $"HSFREQ_07_3_1A", "HSFRES_07_3_1A");                

                // ****************************************************************
                //  Transform Native Amadeus CCValid Response into OTA Response   *
                // **************************************************************** 
                try
                {
                    var tagToReplace = "</Command_CrypticReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CCValidRS.xsl", false);                    
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CCValid, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }

        public string CurrencyConvertion()
        {
            string strResponse;            
            try
            {
                string strRequest = SetRequest("AmadeusWS_CurConvRQ.xsl");
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");
                
                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = SendGDSMessage(ttAA, strRequest, "HSFREQ_07_3_1A", "HSFRES_07_3_1A");
                
                // ****************************************************************
                //  Transform Native Amadeus CurConv Response into OTA Response   *
                // **************************************************************** 
                try
                {
                    var tagToReplace = "</Command_CrypticReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_CurConvRS.xsl", false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.CurConv, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }

        public string TimeDifference()
        {
            string strResponse;
            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);
                // ****************************************************************
                //  Transform OTA TimeDiff Request into Native Amadeus Request    *
                // **************************************************************** 
                string strRequest = SetRequest("AmadeusWS_TimeDiffRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // *******************************************************************************
                //  Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 
                strResponse = SendGDSMessage(ttAA, strRequest, "HSFREQ_07_3_1A", "HSFRES_07_3_1A");
                
                // *****************************************************************
                //  Transform Native Amadeus TimeDiff Response into OTA Response   *
                // ***************************************************************** 
                try
                {
                    var tagToReplace = "</Command_CrypticReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_TimeDiffRS.xsl", false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.TimeDiff, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }

        public string Native()
        {
            string strResponse = "";
            
            try
            {
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                XmlDocument oReqDoc = new XmlDocument();
                oReqDoc.LoadXml(Request);
                XmlElement oRoot = oReqDoc.DocumentElement;
                if (oRoot.SelectSingleNode("Native") == null)
                {
                    throw new Exception("Native Message is missing in the Request.");
                }
                else
                {
                    Request = oRoot.SelectSingleNode("Native").InnerXml;
                }


                // *******************************************************************************
                //  Send Native Request to the Amadeus Adapter and Getting Native Response  *
                // ******************************************************************************* 
                try
                {
                    if (Request.StartsWith("<Command_Cryptic>"))
                        strResponse = SendRequestCryptically(ttAA, Request);
                    else if (Request.StartsWith("<Air_FlightInfo>"))
                        strResponse = SendFlightInfo(ttAA, Request);
                    else if (Request.StartsWith("<PNR_AddMultiElements>"))
                        strResponse = SendAddMultiElements(ttAA, Request);
                    else if (Request.StartsWith("<Fare_PricePNRWithLowerFares>"))
                        strResponse = SendPricePNRWithLowerFares(ttAA, Request);
                    else if (Request.StartsWith("<Fare_PricePNRWithBookingClass>"))
                        strResponse = SendPricePNRWithBookingClass(ttAA, Request);
                    else if (Request.StartsWith("<Air_SellFromRecommendation>"))
                        strResponse = SendAirSellFromRecommendation(ttAA, Request);
                    else if (Request.StartsWith("<Air_RetrieveSeatMap>"))
                        strResponse = SendRetrieveSeatMap(ttAA, Request);
                    else if (Request.StartsWith("<Fare_DisplayFaresForCityPair>"))
                        strResponse = SendFareDisplayFaresForCityPair(ttAA, Request);
                    else if (Request.StartsWith("<Fare_GetFareFamilyDescription>"))
                        strResponse = SendGetFareFamilyDescription(ttAA, Request);
                    else if (Request.StartsWith("<Fare_CheckRules>"))
                        strResponse = SendFareCheckRules(ttAA, Request);
                    else if (Request.StartsWith("<Air_MultiAvailability>"))
                        strResponse = SendAirMultiAvailability(ttAA, Request);
                    else if (Request.StartsWith("<Fare_MasterPricerExpertSearch>"))
                        strResponse = SendMasterPricerExpertSearch(ttAA, Request);
                    else if (Request.StartsWith("<Fare_MasterPricerTravelBoardSearch>"))
                        strResponse = SendMasterPricerTravelBoardSearch(ttAA, Request);
                    else if (Request.StartsWith("<Fare_MasterPricerCalendar>"))
                        strResponse = SendMasterPricerCalendar(ttAA, Request);
                    else if (Request.StartsWith("<Fare_InformativePricingWithoutPNR>"))
                        strResponse = SendInformativePricingWithoutPNR(ttAA, Request);
                    else if (Request.StartsWith("<Fare_InformativeBestPricingWithoutPNR>"))
                        strResponse = SendFareInformativeBestPricingWithoutPNR(ttAA, Request);
                    else if (Request.StartsWith("<Fare_SellByFareCalendar>"))
                        strResponse = SendSellByFareCalendar(ttAA, Request);
                    else if (Request.StartsWith("<Fare_SellByFareSearch>"))
                        strResponse = SendSellByFareSearch(ttAA, Request);
                    else if (Request.StartsWith("<OTA_HotelAvailRQ EchoToken=\"Pricing\""))
                        strResponse = SendHotelEnhancedPricing(ttAA, Request);
                    else if (Request.StartsWith("<OTA_HotelAvailRQ"))
                        strResponse = SendHotelMultiSingleAvailability(ttAA, Request);
                    else if (Request.StartsWith("<Hotel_AvailabilityMultiProperties>"))
                        strResponse = SendHotelAvailabilityMultiProperties(ttAA, Request);
                    else if (Request.StartsWith("<Hotel_SingleAvailability>"))
                        strResponse = SendHotelSingleAvailability(ttAA, Request);
                    else if (Request.StartsWith("<Hotel_RateChange>"))
                        strResponse = SendHotelRateChange(ttAA, Request);
                    else if (Request.StartsWith("<Hotel_Features>"))
                        strResponse = SendHotelFeatures(ttAA, Request);
                    else if (Request.StartsWith("<Hotel_Terms>"))
                        strResponse = SendHotelTerms(ttAA, Request);
                    else if (Request.StartsWith("<Hotel_List>"))
                        strResponse = SendHotelList(ttAA, Request);
                    else if (Request.StartsWith("<Hotel_StructuredPricing>"))
                        strResponse = SendHotelStructuredPricing(ttAA, Request);
                    else if (Request.StartsWith("<Hotel_Sell>"))
                        strResponse = SendHotelSell(ttAA, Request);
                    else if (Request.StartsWith("<Car_SingleAvailability>"))
                        strResponse = SendCarSingleAvailability(ttAA, Request);
                    else if (Request.StartsWith("<Car_MultiAvailability>"))
                        strResponse = SendCarMultiAvailability(ttAA, Request);
                    else if (Request.StartsWith("<Car_Policy>"))
                        strResponse = SendCarPolicy(ttAA, Request);
                    else if (Request.StartsWith("<Car_RateInformationFromAvailability>"))
                        strResponse = SendCarRateInformationFromAvailability(ttAA, Request);
                    else if (Request.StartsWith("<Car_InformationImage>"))
                        strResponse = SendCarInformationImage(ttAA, Request);
                    else if (Request.StartsWith("<Car_Sell>"))
                        strResponse = SendCarSell(ttAA, Request);
                    else if (Request.StartsWith("<Car_LocationList>"))
                        strResponse = SendCarLocationList(ttAA, Request);
                    else if (Request.StartsWith("<Car_Availability>"))
                        strResponse = SendCarAvailability(ttAA, Request);
                    else if (Request.StartsWith("<Ticket_CreditCardCheck>"))
                        strResponse = SendTicketCreditCardCheck(ttAA, Request);
                    else if (Request.StartsWith("<PNR_Retrieve>"))
                        strResponse = SendRetrievePNR(ttAA, Request);
                    else if (Request.StartsWith("<PNR_RetrieveByRecLoc>"))
                        strResponse = SendRetrivePNRbyRL(ttAA, Request);
                    else if (Request.StartsWith("<Ticket_DisplayTST>"))
                        strResponse = SendDisplayTST(ttAA);
                    else if (Request.StartsWith("<Ticket_DeleteTST>"))
                        strResponse = SendDeleteTST(ttAA);
                    else if (Request.StartsWith("<Ticket_CreateTSTFromPricing>"))
                        strResponse = SendCreateTSTFromPricing(ttAA, Request);
                    else if (Request.StartsWith("<Ticket_UpdateTST>"))
                        strResponse = SendUpdateTST(ttAA, Request);
                    else if (Request.StartsWith("<Ticket_ProcessETicket>"))
                        strResponse = SendETicketProcess(ttAA, Request);
                    else if (Request.StartsWith("<Ticket_AutomaticUpdate>"))
                        strResponse = SendAutomaticUpdate(ttAA, Request);
                    else if (Request.StartsWith("<Queue_PlacePNR>"))
                        strResponse = SendQueuePlacePNR(ttAA, Request);
                    else if (Request.StartsWith("<Queue_MoveItem>"))
                        strResponse = SendQueueMoveItem(ttAA, Request);
                    else if (Request.StartsWith("<Queue_RemoveItem>"))
                        strResponse = SendQueueRemoveItem(ttAA, Request);
                    else if (Request.StartsWith("<DocIssuance_IssueTicket>"))
                        strResponse = SendIssueTicket(ttAA, Request);
                    else if (Request.StartsWith("<PNR_Cancel>"))
                        strResponse = SendCancelPNR(ttAA, Request);
                    else if (Request.StartsWith("<DocRefund_CalculateRefund>"))
                        strResponse = SendCalculateRefund(ttAA, Request);
                    else if (Request.StartsWith("<DocRefund_IgnoreRefund>"))
                        strResponse = SendIgnoreRefund(ttAA, Request);
                    else if (Request.StartsWith("<DocRefund_InitRefund>"))
                        strResponse = SendInitRefund(ttAA, Request);
                    else if (Request.StartsWith("<DocRefund_ProcessRefund>"))
                        strResponse = SendProcessRefund(ttAA, Request);
                    else if (Request.StartsWith("<DocRefund_SearchRefundRule>"))
                        strResponse = SendSearchRefund(ttAA, Request);
                    else if (Request.StartsWith("<DocRefund_UpdateRefund>"))
                        strResponse = SendUpdateRefund(ttAA, Request);
                    else if (Request.StartsWith("<Security_Authenticate>"))
                        strResponse = SendSecurityAuthenticate(ttAA, Request);
                    else if (Request.StartsWith("<Security_SignOut>"))
                        strResponse = SendSecuritySignOut(ttAA, Request);
                    else if (Request.StartsWith("<OTA_HotelDescriptiveInfoRQ"))
                        strResponse = SendHotelDescriptiveInfo(ttAA, Request);
                    else if (Request.StartsWith("<MiniRule_GetFromPricing"))
                        strResponse = SendGetFromPricing(ttAA, Request);
                    else if (Request.StartsWith("<MiniRule_GetFromPricingRec"))
                        strResponse = SendGetFromPricingRecLoc(ttAA, Request);
                    else if (Request.StartsWith("<Fare_FlexPricerUpsell"))
                        strResponse = SendFareFlexPriceUpsell(ttAA, Request);
                    else if (Request.StartsWith("<Ticket_ATCShopperMasterPricerTravelBoardSearch"))
                        strResponse = SendShopperMasterPricerTravelBoardSearch(ttAA, Request);
                    else if (Request.StartsWith("<Ticket_CancelDocument"))
                        strResponse = SendCancelDocument(ttAA, Request);
                    else if (Request.StartsWith("<Ticket_CheckEligibility"))
                        strResponse = SendCheckEligibility(ttAA, Request);
                    else if (Request.StartsWith("<Ticket_ProcessEDoc"))
                        strResponse = SendProcessEDoc(ttAA, Request);
                    else if (Request.StartsWith("<Ticket_RepricePNRWithBookingClass"))
                        strResponse = SendRepricePNRWithBookingClass(ttAA, Request);
                    else if (Request.StartsWith("<PNR_TransferOwnership"))
                        strResponse = SendPNRTransferOwnership(ttAA, Request);
                    else if (Request.StartsWith("<PNR_Split"))
                        strResponse = SendSplitPNR(ttAA, Request);
                    else if (Request.StartsWith("<Fare_QuoteItinerary"))
                        strResponse = SendFareQuoteItinerary(ttAA, Request);
                    else if (Request.StartsWith("<Fare_MetaPricerTravelboardSearch>"))
                        strResponse = SendFareMetaPricerTravelboardSearch(ttAA, Request);
                    else if (Request.StartsWith("<Fare_MetaPricerCalendar>"))
                        strResponse = SendFareMetaPricerCalendar(ttAA, Request);

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                // ********************
                //  Build Response    *
                // ********************
                try
                {
                    var strSession = "";
                    //  Insert ConversationID
                    if (inSession)
                        strSession = $"<ConversationID>{ConversationID}</ConversationID>";

                    strResponse = $"<NativeRS><Success/><Response>{strResponse.Replace("<", "&lt;").Replace(">", "&gt;")}</Response>{strSession}</NativeRS>";
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error in Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.Native, ex.Message, ttProviderSystems);
            }

            return strResponse;
        }
    }
}

