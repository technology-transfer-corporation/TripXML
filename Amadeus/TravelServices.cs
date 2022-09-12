using System.Xml;
using TripXMLMain;
using System.Text;
using System.Threading;
using System.IO;
using System;
using System.Data;
using System.Net;
using System.Globalization;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using static TripXMLMain.modCore.enAmadeusWSSchema;

namespace AmadeusWS
{
    public class TravelServices : AmadeusBase
    {
        public saveInDBData saveDbData;
        private string native = "";
        private string nativeResp = "";
        public string Warnings { get; set; }

        public string Errors { get; set; }

        public string Message { get; set; }

        public TravelServices()
        {
            Request = string.Empty;
            ConversationID = string.Empty;
            Warnings = string.Empty;
            Errors = string.Empty;
        }

        public struct saveInDBData
        {
            public string TravelBuildRQ;
            public string TravelBuildRS;
            public string portalSession;
        }

        private string SendRequestSegment(AmadeusWSAdapter ttAA, string request, string segment, string soapAction, string nameSpace)
        {
            string strResponse = "";

            if (!string.IsNullOrEmpty(request))
            {

                Message += request;
                strResponse = SendGDSMessage(ttAA, request, soapAction, nameSpace, segment);
                Message += strResponse;

                /*********************
                // Check for Errors  
                ********************/
                strResponse = strResponse.Replace($" xmlns=\"{nameSpace}\"", "");
                nativeResp = strResponse;
                native = Message.Replace($" xmlns=\"{nameSpace}\"", "");

                strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_TB_Errors.xsl", false);
                CoreLib.SendTrace(ttProviderSystems.UserID, "AmadeusWSService", "strResponse", strResponse, ttProviderSystems.LogUUID);

                // Log Errors 
                if (strResponse.Contains("<Error"))
                {
                    Errors += strResponse;
                }
                else if (strResponse.Contains("<Warning"))
                {
                    Warnings += strResponse;
                }
            }

            return strResponse;
        }

        public string TravelBuild()
        {
            XmlNodeList oNodesHotels = null;
            XmlNodeList oNodesPrices = null;
            XmlNode oNodeExchange = null;
            XmlNode oNodeMSCC = null;
            string strResponse = "";
            string strAir = "";
            string strCars = "";
            string strCarsAvail = "";
            string strMCO = "";
            string strFQTV = "";
            string strEchoToken = "";
            string strRTSVC = "";
            int iFareList = 0;
            int i = 0;
            string vRPH;
            string uri = "";
            //session = new Soap4Session(TransactionStatusCode.Start);
            //*******************************************************
            // These below given variables were not in shahsin's code
            //*******************************************************

            string ProfileCompanyName = "";
            bool bReferenceOnly = false; ;

            try
            {
                //**************************************************************** 
                // Transform OTA TravelBuild Request into Several Navite Request * 
                //**************************************************************** 

                string strRequest = SetRequest("AmadeusWS_TravelBuildRQ.xsl");
                DateTime RequestTime = default(DateTime);
                RequestTime = DateTime.Now;

                XmlDocument oDocRequest = new XmlDocument();
                oDocRequest.LoadXml(Request);

                XmlDocument oDocRequest_2 = new XmlDocument();
                oDocRequest_2.LoadXml(Request);

                XmlElement oRootRequest = oDocRequest.DocumentElement;
                XmlElement oRootRequest_2 = oDocRequest_2.DocumentElement;

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                bool Air;
                bool Car;
                bool Hotel;
                bool price;
                bool MCO;
                bool FQTV;
                bool EXCHANGE;
                XmlElement oRoot = null;
                string strMandatory = "";
                string strEpay = "";
                string strEndTransaction = "";
                bool PBN = false;
                //******************** 
                // Get All Requests * 
                //******************** 
                try
                {
                    XmlDocument oDoc = new XmlDocument();
                    oDoc.LoadXml(strRequest);
                    oRoot = oDoc.DocumentElement;

                    if (oRoot.SelectSingleNode("MultiElements") == null)
                    {
                        throw new Exception("Request is missing mandatory elements.");
                    }
                    else
                    {
                        strMandatory = oRoot.SelectSingleNode("MultiElements").InnerXml;

                        if (strMandatory.Contains("<depDate>HOLDDATE</depDate>"))
                        {
                            DateTime curDate = DateTime.Now.AddDays(359);
                            strMandatory = strMandatory.Replace("<depDate>HOLDDATE</depDate>", "<depDate>" + curDate.ToString("ddMMyy") + "</depDate>");
                        }

                        //****************************************************
                        //This 'if' block had been deleted from shashin's code
                        //****************************************************

                        if (oRootRequest.SelectSingleNode("POS/TPA_Extensions/Provider/Userid").InnerText == "euroaviamobile"
                            || oRootRequest.SelectSingleNode("POS/TPA_Extensions/Provider/Userid").InnerText == "euroavia")
                        {
                            string depDate = oRootRequest.SelectSingleNode("OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment/@DepartureDateTime").InnerText;
                            DateTime flightDate = Convert.ToDateTime(depDate);
                            TimeSpan tkDate = flightDate.Date - DateTime.Now.Date;

                            XmlDocument oDocTkt = null;
                            XmlElement oRootTkt = null;
                            XmlNode oNodeTkt = null;

                            oDocTkt = new XmlDocument();
                            oDocTkt.LoadXml(strMandatory);
                            oRootTkt = oDocTkt.DocumentElement;
                            oNodeTkt = oRootTkt.SelectSingleNode("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='TK']/ticketElement/ticket");

                            if (tkDate.Days < 8)
                            {
                                oNodeTkt.SelectSingleNode("date").InnerText = DateTime.Now.ToString("ddMMyy");
                            }
                            else
                            {
                                oNodeTkt.SelectSingleNode("date").InnerText = DateTime.Now.AddDays(2).ToString("ddMMyy");
                            }

                            oNodeTkt.SelectSingleNode("indicator").InnerText = "XL";
                            oNodeTkt.SelectSingleNode("time").InnerText = "2359";
                            strMandatory = oRootTkt.OuterXml;
                        }

                    }

                    if (oRoot.SelectSingleNode("MasterPricer/Air_SellFromRecommendation") == null)
                    {
                        Air = false;
                    }
                    else
                    {
                        strAir = oRoot.SelectSingleNode("MasterPricer").InnerXml;
                        Air = strAir.Length > 0;
                    }

                    if (oRoot.SelectSingleNode("FQTV") == null)
                    {
                        FQTV = false;
                    }
                    else
                    {
                        strFQTV = oRoot.SelectSingleNode("FQTV").OuterXml;
                        FQTV = strFQTV.Length > 0;
                    }

                    if (oRoot.SelectSingleNode("MCO") == null)
                    {
                        MCO = false;
                    }
                    else
                    {
                        strMCO = oRoot.SelectSingleNode("MCO").InnerXml;
                        MCO = strMCO.Length > 0;
                    }

                    if (oRoot.SelectSingleNode("Cars") == null)
                    {
                        Car = false;
                    }
                    else
                    {
                        if (oRoot.SelectSingleNode("Cars/CarAvail") != null)
                        {
                            strCarsAvail = oRoot.SelectSingleNode("Cars/CarAvail").InnerXml;
                        }
                        strCars = oRoot.SelectSingleNode("Cars/CarSell").InnerXml;
                        Car = strCars.Length > 0;
                    }

                    if (oRoot.SelectSingleNode("Hotels") == null)
                    {
                        Hotel = false;
                    }
                    else
                    {
                        oNodesHotels = oRoot.SelectNodes("Hotels");
                        Hotel = true;
                    }

                    if (oRoot.SelectSingleNode("Pricing/Command_Cryptic") == null && oRoot.SelectSingleNode("Pricing/Fare_PricePNRWithBookingClass") == null)
                    {
                        price = false;
                    }
                    else
                    {
                        oNodesPrices = oRoot.SelectNodes("Pricing");
                        price = true;
                    }

                    EXCHANGE = oRoot.SelectSingleNode("Exchange") == null;

                    if (!EXCHANGE)
                    {
                        oNodeExchange = oRoot.SelectSingleNode("Exchange").SelectSingleNode("EXCH");
                        oNodeMSCC = oRoot.SelectSingleNode("Exchange").SelectSingleNode("MSCC");
                    }

                    //*************************************************
                    // This 'if' block was deleted from shashin's code
                    //*************************************************

                    if (oRoot.SelectSingleNode("PBN") == null)
                    {
                        PBN = false;
                    }
                    else
                    {
                        ProfileCompanyName = oRoot.SelectSingleNode("PBN").InnerXml;
                        PBN = ProfileCompanyName.Length > 0;
                    }

                    if (oRootRequest.SelectSingleNode("@ReferenceOnly") != null && oRootRequest.SelectSingleNode("@ReferenceOnly").InnerText == "true")
                    {
                        bReferenceOnly = true;
                    }

                    //---------------------------------------------------

                    strEndTransaction = oRoot.SelectSingleNode("ET").InnerXml;
                    strEpay = oRoot.SelectSingleNode("EPAY").InnerXml;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Loading Transformed Request XML Document.\r\n{ex.Message}");
                }

                // ******************* 
                // Create Session * 
                // ******************* 
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);
                var strResponseTST = string.Empty;
                // ******************* 
                // Send the Requests * 
                // ******************* 

                try
                {
                    // Send Air elements 
                    if (Air)
                    {
                        strResponse = SendRequestSegment(ttAA, strAir, "Air", ttProviderSystems.AmadeusWSSchema[Air_SellFromRecommendation], ttProviderSystems.AmadeusWSSchema[Air_SellFromRecommendationReply]);

                        // Fatal Error 
                        if (!string.IsNullOrEmpty(strResponse))
                        {
                            strResponse = BuildOTAResponse(strResponse);
                            return strResponse;
                        }
                    }
                    //*************************************************
                    //    This 'if' was deleted from shashin's code
                    //*************************************************
                    if (PBN)
                    {
                        strResponse = SendCommandCryptically(ttAA, ProfileCompanyName);
                        Message += strResponse;
                    }

                    //--------------------------------------------------

                    // Send Mandatory elements 
                    strResponse = SendRequestSegment(ttAA, strMandatory, "MultiElements", ttProviderSystems.AmadeusWSSchema[PNR_AddMultiElements], ttProviderSystems.AmadeusWSSchema[PNR_Reply]);

                    // Get the native response multi elements 
                    XmlDocument oDocResp = new XmlDocument();
                    oDocResp.LoadXml(nativeResp);
                    XmlElement oRootResp = oDocResp.DocumentElement;
                    uri = oDocResp.DocumentElement.GetAttribute("xmlns");
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(oDocResp.NameTable);
                    nsmgr.AddNamespace("bk", uri);

                    // Fatal Error 
                    if (strResponse.Length > 0)
                    {

                        if (strResponse.IndexOf("Flight") != -1)
                        {

                            var strBlackListResp = strResponse;

                            // try to book next recommendation 
                            if (ttProviderSystems.RebookNextFlight == true)
                            {
                                // first cancel booked itinerary if any 
                                int iPax = 0;
                                int iSegs = 0;
                                int iErr = 0;
                                XmlDocument oDocErr = null;
                                XmlElement oRootErr = null;
                                string strSegs = "";

                                iErr = 0;
                                iPax = oRoot.SelectNodes("MultiElements/PNR_AddMultiElements/travellerInfo").Count;
                                iSegs = oRoot.SelectNodes("MultiElements/PNR_AddMultiElements/originDestinationDetails/itineraryInfo").Count;

                                oDocErr = new XmlDocument();
                                oDocErr.LoadXml($"<Err>{strResponse}</Err>");
                                oRootErr = oDocErr.DocumentElement;

                                foreach (XmlNode oNodeErr in oRootErr.ChildNodes)
                                {
                                    if (oNodeErr.InnerText.IndexOf("WAIT LIST") == -1)
                                    {
                                        iErr++;
                                    }
                                }

                                if (iErr < iSegs)
                                {
                                    if (iSegs - iErr > 1)
                                    {
                                        strSegs = $"-{iPax + iSegs - iErr}";
                                    }
                                    strRequest = $"<Command_Cryptic><messageAction><messageFunctionDetails><messageFunction>M</messageFunction></messageFunctionDetails></messageAction><longTextString><textStringDetails>XE{iPax + 1}{strSegs}</textStringDetails></longTextString></Command_Cryptic>";
                                    Message += strRequest;
                                    strResponse = SendCommandCryptically(ttAA, strRequest);
                                    Message += strResponse;
                                }

                                string strValuePricer = "";
                                // send value pricer request 
                                if (oRoot.SelectSingleNode("ValuePricer") != null)
                                {
                                    strValuePricer = oRoot.SelectSingleNode("ValuePricer").InnerXml;
                                    strResponse = SendRequestSegment(ttAA, strValuePricer, "ValuePricer", ttProviderSystems.AmadeusWSSchema[Command_Cryptic], ttProviderSystems.AmadeusWSSchema[Command_CrypticReply]);

                                    // Fatal Error 
                                    if (strResponse.Length > 0)
                                    {
                                        strResponse = BuildOTAResponse(strResponse);
                                        return strResponse;
                                    }
                                }

                                // book first recommendation that does not contain the failed flights 
                                strResponse = "";
                                i = 1;
                                while (!strResponse.Contains("ITINERARY REBOOKED") & !strResponse.Contains("INVALID RECOMMENDATION NUMBER") & i < 21)
                                {
                                    strValuePricer = $"<Command_Cryptic><messageAction><messageFunctionDetails><messageFunction>M</messageFunction></messageFunctionDetails></messageAction><longTextString><textStringDetails>FXZ{i}</textStringDetails></longTextString></Command_Cryptic>";
                                    Message += strValuePricer;
                                    strResponse = SendCommandCryptically(ttAA, strValuePricer);
                                    Message += strResponse;
                                    i++;
                                }

                                if (strResponse.Contains("INVALID RECOMMENDATION NUMBER"))
                                {
                                    strResponse = BuildOTAResponse("NO FLIGHTS AVAILABLE ON ITINERARY");
                                    return strResponse;
                                }
                                else if ((oRoot.SelectSingleNode("SSRs") != null))
                                {
                                    string strSSRs = oRoot.SelectSingleNode("SSRs").InnerXml;
                                    strResponse = SendRequestSegment(ttAA, strSSRs, "SSRs", ttProviderSystems.AmadeusWSSchema[PNR_AddMultiElements], ttProviderSystems.AmadeusWSSchema[PNR_Reply]);

                                    //Potencially we can do this.
                                    //strResponse = SendAddMultiElements(ttAA, strSSRs, "SSRs");

                                    // Fatal Error 
                                    if (strResponse.Length > 0)
                                    {
                                        strResponse = BuildOTAResponse(strResponse);
                                        return strResponse;
                                    }
                                }
                            }
                            else
                            {
                                strResponse = BuildOTAResponse(strResponse);
                                return strResponse;
                            }
                        }
                        else
                        {
                            strResponse = BuildOTAResponse(strResponse);
                            return strResponse;
                        }
                    }

                    if (FQTV)
                    {
                        XmlDocument oDocFQTV = null;
                        XmlElement oRootFQTV = null;

                        oDocFQTV = new XmlDocument();
                        oDocFQTV.LoadXml(strFQTV);
                        oRootFQTV = oDocFQTV.DocumentElement;
                        int ipos = 1;

                        foreach (XmlNode oNodeFQTV in oRootFQTV.ChildNodes)
                        {
                            string strFqtv = oNodeFQTV.SelectSingleNode("longTextString/textStringDetails").InnerText;
                            string paxref = strFqtv.Substring(strFqtv.LastIndexOf("/P")).Substring(2);
                            XmlNode oPax = oRootRequest.SelectSingleNode($"TPA_Extensions/PNRData/Traveler[TravelerRefNumber/@RPH='{paxref}']");
                            string lName = oPax.SelectSingleNode("PersonName/Surname").InnerText.ToUpper();
                            string fName = oPax.SelectSingleNode("PersonName/GivenName").InnerText.ToUpper();

                            if (oPax.SelectSingleNode("PersonName/NamePrefix") != null)
                            {
                                if (oPax.SelectSingleNode("PersonName/NamePrefix").InnerText != "")
                                    fName += $" {oPax.SelectSingleNode("PersonName/NamePrefix").InnerText.ToUpper()}";
                            }
                            string a = $"travellerInfo[passengerData/travellerInformation/traveller/surname = '{lName}' and passengerData/travellerInformation/passenger/firstName = '{fName}']/elementManagementPassenger/lineNumber";
                            oPax = oRootResp.SelectSingleNode(a);

                            if (oPax != null)
                            {
                                strFqtv = oNodeFQTV.OuterXml.Substring(0, oNodeFQTV.OuterXml.IndexOf("/P"));
                                strFqtv += $"/P{oPax.InnerText}{oNodeFQTV.OuterXml.Substring(oNodeFQTV.OuterXml.IndexOf("/P") + 3)}";
                                strResponse = SendRequestSegment(ttAA, strFqtv, "FQTV", ttProviderSystems.AmadeusWSSchema[Command_Cryptic], ttProviderSystems.AmadeusWSSchema[Command_CrypticReply]);

                                // Fatal Error 
                                if (strResponse.Length > 0)
                                {
                                    strResponse = BuildOTAResponse(strResponse);
                                    return strResponse;
                                }
                            }
                            ipos += 1;
                        }
                        oDocFQTV = null;
                        oRootFQTV = null;
                    }

                    if (MCO)
                    {
                        XmlDocument oDocMCO = null;
                        XmlElement oRootMCO = null;
                        XmlNode oNodeMCOPax = null;
                        string strMCOPax = "";
                        string strMCOTattoo = "";
                        strMCO = $"<MCO>{strMCO}</MCO>";

                        oDocMCO = new XmlDocument();
                        oDocMCO.LoadXml(strMCO);
                        oRootMCO = oDocMCO.DocumentElement;

                        foreach (XmlNode oNodeMCO in oRootMCO.ChildNodes)
                        {
                            oNodeMCOPax = oNodeMCO.SelectSingleNode("mcoData/paxTattoo/otherPaxDetails/uniqueCustomerIdentifier");

                            if (oNodeMCOPax != null)
                                strMCOPax = oNodeMCOPax.InnerXml;

                            strMCOTattoo = oRootResp.SelectSingleNode($"travellerInfo/elementManagementPassenger[lineNumber = '{strMCOPax}']/reference/number").InnerXml;
                            oNodeMCOPax.InnerXml = strMCOTattoo;
                            strResponse = SendRequestSegment(ttAA, oNodeMCO.OuterXml, "MCO", "", "");

                            // Fatal Error 
                            if (strResponse.Length > 0)
                            {
                                strResponse = BuildOTAResponse(strResponse);
                                oDocMCO = null;
                                oRootMCO = null;
                                oNodeMCOPax = null;
                                return strResponse;
                            }
                        }

                        oDocMCO = null;
                        oRootMCO = null;
                        oNodeMCOPax = null;
                    }

                    XmlNode oNodeResp = null;
                    //-----------------------------------------------------

                    // Send Cars Request 
                    if (Car)
                    {
                        string query = "bk:travellerInfo[bk:passengerData/bk:travellerInformation/bk:passenger/bk:type != 'INF' and bk:passengerData/bk:travellerInformation/bk:passenger/bk:type != 'CHD']/bk:elementManagementPassenger/bk:reference/bk:number";
                        oNodeResp = oRootResp.SelectSingleNode(query, nsmgr);
                        if (oNodeResp == null)
                        {
                            throw new Exception("Car cannot be associated to child or infant");
                        }

                        strCars = strCars.Replace("<value>X</value>", $"<value>{oNodeResp.InnerText}</value>");

                        if (!string.IsNullOrEmpty(strCarsAvail))
                            strResponse = SendRequestSegment(ttAA, strCarsAvail, "CarAvail", ttProviderSystems.AmadeusWSSchema[Car_SingleAvailability], ttProviderSystems.AmadeusWSSchema[Car_SingleAvailabilityReply]);

                        if (strResponse.Length == 0)
                        {
                            XmlDocument oDocCar = null;
                            XmlElement oRootCar = null;
                            XmlNode oNodeCar = null;

                            oDocCar = new XmlDocument();
                            oDocCar.LoadXml(nativeResp);
                            oRootCar = oDocCar.DocumentElement;

                            oNodeCar = oRootCar.SelectSingleNode("companyLocationInfo/availabilityLine[1]/sellFromAvailabilityGroup");

                            if (oNodeCar != null)
                            {
                                strCars = strCars.Replace("<sellFromAvailabilitylGroup />", oNodeCar.OuterXml.Replace("sellFromAvailabilityGroup", "sellFromAvailabilitylGroup")).Replace("<unit>MIN</unit>", "");

                                oNodeCar = oRootCar.SelectSingleNode("companyLocationInfo/availabilityLine[1]/rateDetailsInfo/tariffInfo/rateIdentifier");

                                if (oNodeCar != null)
                                    strCars = strCars.Replace("<rateType />", $"<rateType>{oNodeCar.InnerText}</rateType>");
                            }

                            strResponse = SendRequestSegment(ttAA, strCars, "CarSell", ttProviderSystems.AmadeusWSSchema[Car_Sell], ttProviderSystems.AmadeusWSSchema[Car_SellReply]);
                        }

                        if (strResponse.Length > 0)
                        {
                            strResponse = BuildOTAResponse(strResponse);
                            return strResponse;
                        }
                    }

                    // Send Hotels Request 
                    if (Hotel)
                    {
                        // here we loop on hotels we might have in request
                        // so you can ceate an index that corresponds to the hotel booking we are processing
                        //int iHotel = 0;
                        foreach (XmlNode oNodeHotel in oNodesHotels)
                        {
                            string strHotelAvail = "";
                            string strHotels = "";

                            if (ttProviderSystems.HotelVersion == "2")
                            {
                                if (oNodeHotel.SelectSingleNode("HotelAvail2") != null)
                                {
                                    strHotelAvail = oNodeHotel.SelectSingleNode("HotelAvail2").InnerXml;
                                }

                                if (oNodeHotel.SelectSingleNode("HotelSell2") != null)
                                {
                                    strHotels = oNodeHotel.SelectSingleNode("HotelSell2").InnerXml;

                                    XmlNode oHotel = oRootRequest_2.SelectSingleNode("OTA_HotelResRQ/HotelReservations").SelectNodes("HotelReservation")[i];
                                    // now we point to same hotel in input xml
                                    // then you need to test if RPH is provided in input for that hotel

                                    if (oHotel.SelectSingleNode("RoomStays").SelectSingleNode("RoomStay").SelectSingleNode("ResGuestRPHs") != null)
                                    {
                                        vRPH = oHotel.SelectSingleNode("RoomStays/RoomStay/ResGuestRPHs/ResGuestRPH/@RPH").InnerText;
                                        oNodeResp = oRootResp.SelectSingleNode("travellerInfo[passengerData/travellerInformation/passenger/type != 'INF' and passengerData/travellerInformation/passenger/type != 'CHD'][elementManagementPassenger/lineNumber='" + vRPH + "']/elementManagementPassenger/reference/number");
                                    }
                                    else
                                    {
                                        // here you have to pick the amadeus ref of first passenger as there is no RPH oin input
                                        oNodeResp = oRootResp.SelectSingleNode("travellerInfo[passengerData/travellerInformation/passenger/type != 'INF' and passengerData/travellerInformation/passenger/type != 'CHD'][elementManagementPassenger/lineNumber='1']/elementManagementPassenger/reference/number");
                                    }

                                    if (oNodeResp == null)
                                    {
                                        throw new Exception("Hotel cannot be associated to child or infant");
                                    }

                                    if (strHotels.IndexOf("<value>X</value>") != -1)
                                    {
                                        // here you will get the RPH number from input XML, from ResGuestRPHs/ResGuestRPH RPH="1"
                                        // and then match it to the line number of the passenger in Amadeus reply
                                        strHotels = strHotels.Replace("<value>X</value>", $"<value>{oNodeResp.InnerText}</value>");
                                    }
                                }

                                if (!string.IsNullOrEmpty(strHotelAvail))
                                    strResponse = SendRequestSegment(ttAA, strHotelAvail, "HotelAvail2", ttProviderSystems.AmadeusWSSchema[Hotel_MultiSingleAvailability], ttProviderSystems.AmadeusWSSchema[Hotel_MultiSingleAvailability]);

                                if (!string.IsNullOrEmpty(strHotelAvail))
                                    strResponse = SendRequestSegment(ttAA, strHotels, "HotelSell2", ttProviderSystems.AmadeusWSSchema[Hotel_Sell], ttProviderSystems.AmadeusWSSchema[Hotel_SellReply]);

                                if (strResponse.Length > 0)
                                {
                                    strResponse = BuildOTAResponse(strResponse);
                                    return strResponse;
                                }
                            }
                            else
                            {
                                if (oNodeHotel.SelectSingleNode("HotelAvail") != null)
                                {
                                    strHotelAvail = oNodeHotel.SelectSingleNode("HotelAvail").InnerXml;
                                }

                                if (oNodeHotel.SelectSingleNode("HotelSell") != null)
                                {
                                    strHotels = oNodeHotel.SelectSingleNode("HotelSell").InnerXml;

                                    // this node points to passenger names already, the only thing you need to do is to make it point to passenger with RPH number from input file
                                    // if varibale vRPH contains that number from input, you would just add this to it:
                                    // VRPH woudl have the RPH number from input and now the node oNodeResp would point to its Amadeus internal number

                                    XmlNode oHotel = oRootRequest_2.SelectSingleNode("OTA_HotelResRQ/HotelReservations").SelectNodes("HotelReservation")[i];
                                    // now we point to same hotel in input xml
                                    // then you need to test if RPH is provided in input for that hotel

                                    if (oHotel.SelectSingleNode("RoomStays").SelectSingleNode("RoomStay").SelectSingleNode("ResGuestRPHs") != null)
                                    {
                                        vRPH = oHotel.SelectSingleNode("RoomStays/RoomStay/ResGuestRPHs/ResGuestRPH/@RPH").InnerText;
                                        oNodeResp = oRootResp.SelectSingleNode("travellerInfo[passengerData/travellerInformation/passenger/type != 'INF' and passengerData/travellerInformation/passenger/type != 'CHD'][elementManagementPassenger/lineNumber='" + vRPH + "']/elementManagementPassenger/reference/number");
                                    }
                                    else
                                    {
                                        // here you have to pick the amadeus ref of first passenger as there is no RPH oin input
                                        oNodeResp = oRootResp.SelectSingleNode("travellerInfo[passengerData/travellerInformation/passenger/type != 'INF' and passengerData/travellerInformation/passenger/type != 'CHD'][elementManagementPassenger/lineNumber='1']/elementManagementPassenger/reference/number");
                                    }

                                    if (oNodeResp == null)
                                    {
                                        throw new Exception("Hotel cannot be associated to child or infant");
                                    }

                                    if (strHotels.IndexOf("<value>X</value>") != -1)
                                    {
                                        // here you will get the RPH number from input XML, from ResGuestRPHs/ResGuestRPH RPH="1"
                                        // and then match it to the line number of the passenger in Amadeus reply
                                        strHotels = strHotels.Replace("<value>X</value>", $"<value>{oNodeResp.InnerText}</value>");
                                    }
                                }

                                if (!string.IsNullOrEmpty(strHotelAvail))
                                {
                                    strResponse = SendRequestSegment(ttAA, strHotelAvail, "HotelAvail", ttProviderSystems.AmadeusWSSchema[Hotel_SingleAvailability], ttProviderSystems.AmadeusWSSchema[Hotel_SingleAvailabilityReply]);
                                }

                                if (!string.IsNullOrEmpty(strHotelAvail))
                                {
                                    strResponse = SendRequestSegment(ttAA, strHotels, "HotelSell", ttProviderSystems.AmadeusWSSchema[Hotel_Sell], ttProviderSystems.AmadeusWSSchema[Hotel_SellReply]);
                                }

                                if (strResponse.Length > 0)
                                {
                                    strResponse = BuildOTAResponse(strResponse);
                                    return strResponse;
                                }
                            }

                        }
                    }

                    //******************************************************
                    // In shashin code this if contains (Price) as condition
                    //******************************************************

                    if (price && Air)
                    {
                        decimal totalFare = 0;
                        foreach (XmlNode oNodePrice in oNodesPrices)
                        {
                            string strPricing = oNodePrice.InnerXml;

                            strResponse = strPricing.Contains("<Command_Cryptic>")
                                ? SendRequestSegment(ttAA, strPricing, "Price", ttProviderSystems.AmadeusWSSchema[Command_Cryptic], ttProviderSystems.AmadeusWSSchema[Command_CrypticReply])
                                : SendRequestSegment(ttAA, strPricing, "Price", ttProviderSystems.AmadeusWSSchema[Fare_PricePNRWithBookingClass], ttProviderSystems.AmadeusWSSchema[Fare_PricePNRWithBookingClassReply]);

                            oDocResp.LoadXml(nativeResp);
                            oRootResp = oDocResp.DocumentElement;

                            // Fatal Error 
                            if (strResponse.Length > 0)
                            {
                                strResponse = BuildOTAResponse(strResponse);
                                return strResponse;
                            }

                            if (!strPricing.Contains("<Cryptic_GetScreen_Query>"))
                            {
                                string strStorePrice = "<Ticket_CreateTSTFromPricing>";
                                iFareList = 1;

                                foreach (XmlNode oNodeResps in oRootResp.SelectNodes("fareList"))
                                {
                                    strStorePrice += $"<psaList><itemReference><referenceType>TST</referenceType><uniqueReference>{iFareList}</uniqueReference></itemReference></psaList>";
                                    if (oNodeResps.SelectSingleNode("fareDataInformation/fareDataSupInformation[2]") != null)
                                    {
                                        totalFare += decimal.Parse(oNodeResps.SelectSingleNode("fareDataInformation/fareDataSupInformation[2]/fareAmount").InnerText);
                                    }
                                    iFareList++;
                                }
                                // this code is for Avianca to test price differences
                                if (ttProviderSystems.CheckBookedFare)
                                {
                                    if (oRootRequest.SelectSingleNode("OTA_AirBookRQ/TravelerInfo") != null)
                                    {
                                        if (oRootRequest.SelectSingleNode("OTA_AirBookRQ/TravelerInfo/SpecialReqDetails") != null)
                                        {
                                            if (oRootRequest.SelectSingleNode("OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks") != null)
                                            {
                                                if (oRootRequest.SelectSingleNode("OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark") != null)
                                                {
                                                    if (oRootRequest.SelectSingleNode("OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark").HasChildNodes && oRootRequest.SelectSingleNode("OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark").Attributes["RemarkType"] != null)
                                                    {
                                                        if (oRootRequest.SelectSingleNode("OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark").Attributes["RemarkType"].InnerText == "T")
                                                        {
                                                            string totFare = oRootRequest.SelectSingleNode("OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark/Text").InnerXml;
                                                            decimal reqTotalFare = decimal.Parse(totFare.Substring(3, totFare.Length - 3));
                                                            if (totalFare > reqTotalFare)
                                                            {
                                                                string strIgnore = "<Command_Cryptic><messageAction><messageFunctionDetails><messageFunction>M</messageFunction></messageFunctionDetails></messageAction><longTextString><textStringDetails>IG</textStringDetails></longTextString></Command_Cryptic>";
                                                                strResponse = SendCommandCryptically(ttAA, strIgnore);
                                                                throw new Exception("Price is different from previous price.New price will be " + totalFare.ToString());
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }

                                strStorePrice += "</Ticket_CreateTSTFromPricing>";
                                strResponse = SendRequestSegment(ttAA, strStorePrice, "StorePrice", ttProviderSystems.AmadeusWSSchema[Ticket_CreateTSTFromPricing], ttProviderSystems.AmadeusWSSchema[Ticket_CreateTSTFromPricingReply]);
                            }
                        }
                    }
                    if (!EXCHANGE)
                    {
                        int PAXTypeCount = 0;

                        PAXTypeCount = oRootRequest.SelectSingleNode("TPA_Extensions").SelectSingleNode("PriceData").SelectNodes("PassengerTypeQuantity").Count;
                        PAXTypeCount++;
                        if (PAXTypeCount < iFareList)
                        {
                            PAXTypeCount = iFareList;
                        }

                        for (int exch = 1; exch < PAXTypeCount; exch++)
                        {
                            string strEXC = oNodeExchange.InnerXml;
                            strEXC = strEXC.Replace("/Tnn", "/T" + exch.ToString());

                            strResponse = SendRequestSegment(ttAA, strEXC, "Exchange", ttProviderSystems.AmadeusWSSchema[Command_Cryptic], ttProviderSystems.AmadeusWSSchema[Command_CrypticReply]);
                        }
                        string strMSCC = oNodeMSCC.InnerXml;
                        strResponse = SendRequestSegment(ttAA, strMSCC, "Exchange", ttProviderSystems.AmadeusWSSchema[Command_Cryptic], ttProviderSystems.AmadeusWSSchema[Command_Cryptic]);
                    }

                    // Send End Transaction Request 
                    Message += strEndTransaction;
                    strResponse = SendAddMultiElements(ttAA, strEndTransaction);

                    Message += strResponse;
                    native += $"{strEndTransaction}{strResponse}";

                    oDocResp.LoadXml(strResponse);
                    oRootResp = oDocResp.DocumentElement;

                    //******************************************************
                    //Below given code line was different in shahsin's codes
                    //oNodeResp = oRootResp.SelectSingleNode("pnrHeader/reservationInfo/reservation/controlNumber");
                    //******************************************************
                    oNodeResp = oRootResp.SelectSingleNode("pnrHeader[not(reservationInfo/reservation/controlType)]/reservationInfo/reservation/controlNumber");
                    //------------------------------------------------------

                    if (oNodeResp == null && strResponse.Contains("<text>WARNING"))
                    {
                        strResponse = SendAddMultiElements(ttAA, strEndTransaction);
                    }

                    // Check for Errors 
                    strEndTransaction = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_TB_Errors.xsl", false);

                    if (strEndTransaction.Length > 0)
                    {
                        if (strEndTransaction.IndexOf("PAYMENT INFORMATION MANDATORY - ENTER SSR EPAY") != -1)
                        {
                            strEpay = strEpay.Replace("XX", strEndTransaction.Substring(strEndTransaction.IndexOf("ENTER SSR EPAY") + 17, 2));
                            // Send EPAY Request 
                            Message += strEpay;
                            strResponse = SendAddMultiElements(ttAA, strEpay);
                            Message += strResponse;

                            native += $"{strEpay}{strResponse}";
                            strEndTransaction = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_TB_Errors.xsl", false);
                        }

                        if (strEndTransaction.Contains("<Error"))
                        {
                            // Fatal Error 
                            return BuildOTAResponse(strEndTransaction);
                        }
                        else if (strEndTransaction.IndexOf("<Warning>") >= 0)
                        {
                            Warnings += strEndTransaction;
                        }
                    }

                    oDocResp.LoadXml(strResponse);
                    oRootResp = oDocResp.DocumentElement;
                    //********************************************************
                    //The below given code line was diffrent in shashin's code
                    //oNodeResp = oRootResp.SelectSingleNode("pnrHeader/reservationInfo/reservation/controlNumber");
                    //********************************************************
                    oNodeResp = oRootResp.SelectSingleNode("pnrHeader[not(reservationInfo/reservation/controlType)]/reservationInfo/reservation/controlNumber");

                    //--------------------------------------------------------

                    if ((oNodeResp != null) && bReferenceOnly == false)
                    {
                        string RecordLocator = oNodeResp.InnerText;

                        if (!string.IsNullOrEmpty(RecordLocator))
                        {
                            // Send Retreive Request 
                            string strRTV = $"<PNR_RetrieveByRecLoc><sbrRecLoc><reservation><controlNumber>{RecordLocator}</controlNumber></reservation></sbrRecLoc></PNR_RetrieveByRecLoc>";
                            double dTime = 0;
                            dTime = DateTime.Now.TimeOfDay.TotalSeconds;

                            //// wait 1 seconds and retrive PNR again
                            //while (!(DateTime.Now.TimeOfDay.TotalSeconds - dTime > 1))
                            //{
                            //}
                            Thread.Sleep(1000);

                            Message += strRTV;
                            strResponse = SendRetrivePNRbyRL(ttAA, strRTV);
                            Message += strResponse;
                            native += $"{strRTV}{strResponse}";

                            oDocResp.LoadXml(strResponse);
                            oRootResp = oDocResp.DocumentElement;
                            oNodeResp = oRootResp.SelectSingleNode("originDestinationDetails/itineraryInfo[itineraryReservationInfo/reservation/controlNumber='']");

                            // check if we have a case where airline record locator is not in PNR yet
                            if (Air && (null == oRootResp.SelectSingleNode("originDestinationDetails/itineraryInfo/itineraryReservationInfo") || null != oNodeResp))
                            {
                                // wait 2 seconds and retrive PNR again
                                dTime = DateTime.Now.TimeOfDay.TotalSeconds;
                                //while (!(DateTime.Now.TimeOfDay.TotalSeconds - dTime > 2))
                                //{
                                //}
                                Thread.Sleep(2000);
                                strResponse = SendRetrivePNRbyRL(ttAA, strRTV);

                                oDocResp.LoadXml(strResponse);
                                oRootResp = oDocResp.DocumentElement;
                                oNodeResp = oRootResp.SelectSingleNode("originDestinationDetails/itineraryInfo[itineraryReservationInfo/reservation/controlNumber='']");

                                // check if airline record locator is still not in PNR 
                                if (Air && (oRootResp.SelectSingleNode("originDestinationDetails/itineraryInfo/itineraryReservationInfo") == null || oNodeResp != null))
                                {
                                    // wait 3 seconds and retrive PNR again
                                    dTime = DateTime.Now.TimeOfDay.TotalSeconds;
                                    //while (!(DateTime.Now.TimeOfDay.TotalSeconds - dTime > 3))
                                    //{
                                    //}
                                    Thread.Sleep(2000);
                                    strResponse = SendRetrivePNRbyRL(ttAA, strRTV);

                                    oDocResp.LoadXml(strResponse);
                                    oRootResp = oDocResp.DocumentElement;
                                    oNodeResp = oRootResp.SelectSingleNode("originDestinationDetails/itineraryInfo[itineraryReservationInfo/reservation/controlNumber='']");

                                    if (oNodeResp != null)
                                        Warnings += "<Warning>AIRLINE RECORD LOCATOR NOT IN PNR</Warning>";
                                }
                            }
                        }
                    }

                    //***********************************************************
                    // Below given if condition was diffrent in shashin's code
                    // if (strResponse.IndexOf("<longFreetext>--- TST ") != -1 )
                    //***********************************************************

                    if (strResponse.Contains("<longFreetext>--- TST ") && bReferenceOnly == false)
                    {
                        strRequest = "<Ticket_DisplayTST><displayMode><attributeDetails><attributeType>ALL</attributeType></attributeDetails></displayMode></Ticket_DisplayTST>";

                        Message += strRequest;
                        strResponse = SendDisplayTST(ttAA);
                        Message += strResponseTST;
                        native += $"{strRequest}{strResponseTST}";
                    }

                    if (strResponse.IndexOf("SUBSIDIARY/FRANCHISE") != -1)
                    {
                        strRequest = "<Command_Cryptic><messageAction><messageFunctionDetails><messageFunction>M</messageFunction></messageFunctionDetails></messageAction><longTextString><textStringDetails>RTSVC</textStringDetails></longTextString></Command_Cryptic>";
                        strRTSVC = SendCommandCryptically(ttAA, strRequest);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                

                //**************************************************************************** 
                // Add Previous Errors and Warnings To Amadeus Native End Transact Response * 
                //**************************************************************************** 

                // get EchoToken if any
                if (Request.Contains(" EchoToken"))
                {
                    XmlDocument oDocReq = new XmlDocument();
                    oDocReq.LoadXml(Request);
                    XmlElement oRootReq = oDocReq.DocumentElement;

                    strEchoToken = $"<EchoToken>{oRootReq.Attributes.GetNamedItem("EchoToken").Value}</EchoToken>";
                }

                strResponse = strResponse.Replace("</PNR_RetrieveByRecLocReply>", $"{Errors}{ Warnings}{strResponseTST}{strEchoToken}{strRTSVC}{Request}</PNR_RetrieveByRecLocReply>");

                //***************************************************************** 
                // Transform Native Amadeus TravelBuild Response into OTA Response * 
                //***************************************************************** 

                try
                {
                    strResponse = strResponse.Replace($"xmlns=\"{uri}\"", "");
                    strResponse = inSession
                        ? strResponse.Replace("</PNR_RetrieveByRecLocReply>", $"<ConversationID>{ConversationID}</ConversationID></PNR_RetrieveByRecLocReply>")
                        : strResponse;


                    CoreLib.SendTrace(ttProviderSystems.UserID, "TravelBuild", "Final response", strResponse, ttProviderSystems.LogUUID);

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_PNRReadRS.xsl", false);
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
                        ConversationID = null;
                        ttAA = null;
                    }
                }

                DateTime ResponseTime = default(DateTime);
                ResponseTime = DateTime.Now;

                if (ttProviderSystems.LogNative)
                {
                    var msg = Message;
                    TripXMLTools.TripXMLLog.LogMessage("TravelBuild", ref msg, RequestTime, ResponseTime, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }

                
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.TravelBuild, exx.Message, ttProviderSystems, "");
            }
            finally
            {
                GC.Collect();
            }

            return strResponse;
        }

        public string IssueTicket()
        {
            try
            {
                DateTime RequestTime = DateTime.Now;
                string strRequest = SetRequest("AmadeusWS_IssueTicketRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                string strRead = "";
                //************************* 
                // Get Multiple Requests * 
                //************************* 
                string strCheckPrt = "";
                string strFOP = "";
                string strTicket = "";

                string strResponse = "";
                string strET = "";
                string strPlatingAirV = "";
                string strTktTypeInPNR = "";
                string strPNR = "";
                string strFareNumber = "";
                string strTktPayment = "";
                string strChangeTkt = "";

                try
                {
                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(strRequest);
                    XmlElement oRoot = oDoc.DocumentElement;

                    if (oRoot.SelectSingleNode("CheckPRT") != null)
                        strCheckPrt = oRoot.SelectSingleNode("CheckPRT").InnerXml;

                    strRead = oRoot.SelectSingleNode("PNRRead").InnerXml;

                    strTicket = oRoot.SelectSingleNode("Ticket") != null
                            ? oRoot.SelectSingleNode("Ticket").InnerXml
                            : oRoot.SelectSingleNode("TicketCryptic").InnerXml;


                    if (oRoot.SelectSingleNode("ChangeFOP") != null)
                        strFOP = oRoot.SelectSingleNode("ChangeFOP").InnerXml;

                    if (oRoot.SelectSingleNode("ChangeTkt") != null)
                    {
                        strChangeTkt = oRoot.SelectSingleNode("ChangeTkt").InnerXml;
                        strET = oRoot.SelectSingleNode("ET").InnerXml;
                    }

                    if (oRoot.SelectSingleNode("Ticketing/FareNumber") != null)
                        strFareNumber = oRoot.SelectSingleNode("Ticketing/FareNumber").InnerText;

                    if (oRoot.SelectSingleNode("TktPayment") != null)
                        strTktPayment = oRoot.SelectSingleNode("TktPayment").InnerXml;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Loading Transformed Request XML Document. {ex.Message}");
                }
                finally
                {
                    GC.Collect();
                }

                //********************** 
                // Create Session * 
                //********************** 
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //***************************************** 
                // Check if printer is up and running * 
                //***************************************** 
                if (!string.IsNullOrEmpty(strCheckPrt))
                {
                    try
                    {
                        Message += strCheckPrt;
                        //strResponse = ttAA.SendMessage(strCheckPrt, "", "", ConversationID);
                        strResponse = SendRequestCryptically(ttAA, strCheckPrt);
                        Message += strResponse;

                        // Check for Printer Status "D" Down. 
                        var oDoc = new XmlDocument();
                        oDoc.LoadXml(strResponse);
                        XmlElement oRoot = oDoc.DocumentElement;

                        XmlNode oNode = oRoot.SelectSingleNode("LinkageDisplay/PrinterParameters[Type='T']");

                        if (oNode == null)
                        {
                            throw new Exception("No ticket printer linked");
                        }
                        else
                        {
                            switch (oNode.SelectSingleNode("Status").InnerText)
                            {
                                case "D":
                                    throw new Exception($"Printer {oNode.SelectSingleNode("LNIATA").InnerText} is Down");
                                case "B":
                                    throw new Exception($"Printer {oNode.SelectSingleNode("LNIATA").InnerText} is Busy");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                //******************** 
                // Retrieve the PNR * 
                //******************** 
                Message += strRead;
                strResponse = SendRetrivePNRbyRL(ttAA, strRead);
                Message += strResponse;
                strPNR = strResponse;

                # region business logic
                if (!string.IsNullOrEmpty(ttProviderSystems.BLFile) && !strResponse.Contains("generalErrorInfo"))
                {
                    XmlDocument oDocBL = new XmlDocument();
                    oDocBL.Load(ttProviderSystems.BLFile);

                    XmlElement oRootBL = oDocBL.DocumentElement;
                    XmlNode oNodeBL = oRootBL.SelectSingleNode($"Security/ProviderBL[@Name=\'Amadeus\'][@System=\'{ttProviderSystems.System}\']");

                    if (oNodeBL != null)
                    {
                        // get accounting line from PNR
                        XmlDocument oDocRespBL = new XmlDocument();
                        oDocRespBL.LoadXml(strResponse);

                        XmlElement oRootRespBL = oDocRespBL.DocumentElement;
                        XmlNode oNodeRespBL = oRootRespBL.SelectSingleNode("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='AI']");

                        if (oNodeRespBL != null)
                        {
                            if (!(oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']") == null))
                            {
                                string strAIAN = oNodeRespBL.SelectSingleNode("accounting/account/number").InnerXml;
                                string strAIN_BL = oNodeBL.SelectSingleNode($"PCC[@Code=\'{ttProviderSystems.PCC}\']/@AuthorizeCode").InnerXml;

                                //compare to the one in BL file 
                                if (strAIAN != strAIN_BL)
                                {
                                    throw new Exception("Secured PNR");
                                }
                            }
                        }
                    }
                }
                # endregion

                // Check for Errors 
                if (strResponse.Contains("<MessagesOnly_Reply>") | !strResponse.Contains("<controlNumber>"))
                    throw new Exception("Cannot retrieve PNR to ticket");

                // Check if stored fares exist 
                if (!strResponse.Contains("<longFreetext>--- TST "))
                    throw new Exception("Cannot issue ticket - no stored fares in PNR");

                try
                {
                    //******************************** 
                    // check if change FOP required * 
                    //******************************** 
                    if (!string.IsNullOrEmpty(strFOP))
                    {
                        Message += strFOP;
                        strResponse = SendRequestCryptically(ttAA, strFOP);
                        //strResponse = ttAA.SendMessage(strFOP, "", "", ConversationID);                            

                        if (strResponse.Contains("<RecID>EROR</RecID>"))
                            throw new Exception("Change of Form Of Payment failed");
                    }

                    //********************************************** 
                    // Check if change of ticketing type required * 
                    //********************************************** 
                    if (!string.IsNullOrEmpty(strChangeTkt))
                    {
                        string strTktTypeToTkt = string.Empty;

                        // get ticket type to issue 
                        if (strChangeTkt.Contains("<ETInd>"))
                            strTktTypeToTkt = strChangeTkt.Substring(strChangeTkt.IndexOf("<ETInd>"), 16);


                        // check if we need to change ticket type 
                        if (strTktTypeInPNR != strTktTypeToTkt)
                        {
                            string strCommission = "";
                            // we need to change ticket type 
                            // insert plating airline in change ticket type request 
                            strChangeTkt = strChangeTkt.Replace("</FareNumInfo>", $"</FareNumInfo>{strPlatingAirV}{strCommission}");

                            Message += strChangeTkt;
                            strResponse = SendRequestCryptically(ttAA, strChangeTkt);
                            //strResponse = ttAA.SendMessage(strChangeTkt, "", "", ConversationID);
                            Message += strResponse;

                            if (!strResponse.Contains("<DPOK></DPOK>"))
                                throw new Exception("Change of ticketing type failed");

                            //*********************************************************** 
                            // end transact after successfull change of ticketing type * 
                            //*********************************************************** 
                            Message += strET;
                            strResponse = SendRequestCryptically(ttAA, strET);
                            //strResponse = ttAA.SendMessage(strET, "", "", ConversationID);

                            Message += strResponse;

                            if (strResponse.Contains("<RecID>EROR</RecID>"))
                                throw new Exception("Change of ticketing type failed");
                        }

                    }

                    if (!string.IsNullOrEmpty(strTktPayment))
                    {
                        // we have a misc payment to send before we ticket
                        Message += strTktPayment;
                        strResponse = SendRequestCryptically(ttAA, strTktPayment);
                        Message += strResponse;

                        if (strResponse.Contains("\rWARNING"))
                        {
                            var strCMD = "<Command_Cryptic><messageAction><messageFunctionDetails><messageFunction>M</messageFunction></messageFunctionDetails></messageAction><longTextString><textStringDetails>ER</textStringDetails></longTextString></Command_Cryptic>";
                            strResponse = SendRequestCryptically(ttAA, strCMD);
                        }
                    }

                    XmlDocument oDocPNR = new XmlDocument();
                    oDocPNR.LoadXml(strPNR);
                    XmlElement oRootPNR = oDocPNR.DocumentElement;

                    XmlNode oNodePNR = oRootPNR.SelectSingleNode("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FV']");

                    if (oNodePNR != null)
                    {
                        // yes there a validating airline
                        XmlNode fvNode = null;
                        fvNode = oNodePNR.SelectSingleNode("otherDataFreetext/longFreetext");
                        if (fvNode != null)
                        {
                            strPlatingAirV = fvNode.InnerText.Substring(fvNode.InnerText.Length - 2);
                        }
                    }

                    // override validating carrier in ticketing entry if needed
                    strTicket = strTicket.Replace("/VC", strPlatingAirV);
                    // send cryptic issue ticket command and format response screen 
                    Message += strTicket;

                    // code added for TravelLinck to be able to ticket per segment when we have different VC's in PNR

                    //***************************************************************
                    // Below given commented block was not commented in shahin's code
                    //***************************************************************

                    if (!string.IsNullOrEmpty(strFareNumber))
                        strTicket = strTicket.Replace("</textStringDetails>", $"/T{ strFareNumber }</textStringDetails>");

                    strResponse = strTicket.StartsWith("<DocIssuance_IssueTicket>")
                        ? SendIssueTicket(ttAA, strTicket)
                        : SendRequestCryptically(ttAA, strTicket);

                    Message += strResponse;

                    if (Message.Contains("COMMISSION NOT VALIDATED - RE ENTER COMMISSION "))
                    {
                        oDocPNR = new XmlDocument();
                        oDocPNR.LoadXml(strPNR);
                        oRootPNR = oDocPNR.DocumentElement;
                        oNodePNR = oRootPNR.SelectSingleNode("dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM']");

                        if (oNodePNR != null)
                        {
                            strRequest = "<Command_Cryptic><messageAction><messageFunctionDetails><messageFunction>M</messageFunction></messageFunctionDetails></messageAction><longTextString><textStringDetails>";
                            strRequest += $"{oNodePNR.SelectSingleNode("elementManagementData/lineNumber").InnerText}/";
                            strRequest += oNodePNR.SelectSingleNode("otherDataFreetext/longFreetext").InnerText;
                            strRequest += "</textStringDetails></longTextString></Command_Cryptic>";

                            SendRequestCryptically(ttAA, strRequest);
                            Message += strResponse;

                            strRequest = "<Command_Cryptic><messageAction><messageFunctionDetails><messageFunction>M</messageFunction></messageFunctionDetails></messageAction><longTextString><textStringDetails>";
                            strRequest += "RFTRIPXML</textStringDetails></longTextString></Command_Cryptic>";

                            strResponse = SendRequestCryptically(ttAA, strRequest);
                            Message += strResponse;

                            strResponse = strTicket.StartsWith("<DocIssuance_IssueTicket>")
                                    ? SendIssueTicket(ttAA, strTicket)
                                    : SendRequestCryptically(ttAA, strTicket);
                            Message += strResponse;
                        }
                    }

                    if (strResponse.Contains("OK "))
                    {
                        strPNR = SendRetrivePNRbyRL(ttAA, strRead);
                        strResponse = strResponse.Replace("</Command_CrypticReply>", $"{strPNR}</Command_CrypticReply>");
                        strResponse = strResponse.Replace("</DocIssuance_IssueTicketReply>", $"{strPNR}</DocIssuance_IssueTicketReply>");
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = null;
                        ttAA = null;
                    }
                }

                //***************************************************************** 
                // Transform Native Amadeus IssueTicket Response into OTA Response * 
                //***************************************************************** 
                try
                {
                    var tagToReplace = "</PNR_RetrieveByRecLoc>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_IssueTicketRS.xsl", false);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }

                if (ttProviderSystems.LogNative)
                {
                    var strMSG = Message;
                    TripXMLTools.TripXMLLog.LogMessage("IssueTicket", ref strMSG, RequestTime, DateTime.Now, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }

                return strResponse;
            }
            catch (Exception exx)
            {
                addLog($"<EXOR><M>{Request}<BL/>", ttProviderSystems.UserID);
                throw exx;
            }
            finally
            {
                GC.Collect();
            }
        }

        public string IssueTicketSessioned()
        {
            try
            {
                DateTime RequestTime = DateTime.Now;
                string strRead;
                string strTicket;
                string strResponse;
                string strPlatingAirV = "";
                string strPNR = "";

                string strRequest = SetRequest("AmadeusWS_IssueTicketSessionedRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                var ttAA = SetAdapter();                
                bool inSession = SetConversationID(ttAA);

                var oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                XmlElement oRoot = oDoc.DocumentElement;

                strTicket = oRoot.SelectSingleNode("TicketCryptic") != null ? oRoot.SelectSingleNode("TicketCryptic").InnerXml : oRoot.SelectSingleNode("Ticket").InnerXml;
                strRead = oRoot.SelectSingleNode("PNRRead").InnerXml;

                #region override validating carrier in ticketing entry if needed
                strTicket = strTicket.Replace("/VC", strPlatingAirV);

                strResponse = strTicket.StartsWith("<DocIssuance_IssueTicket>")
                    ? SendIssueTicket(ttAA, strTicket)
                    : SendRequestCryptically(ttAA, strTicket);

                Message += strResponse;

                if (strResponse.Contains("OK "))
                {
                    strPNR = strTicket.Contains("TTP/ET/INV/ITR-EMLA/STFLL/RT")
                            ? SendCommandCryptically(ttAA, "IR")
                            : SendRetrievePNR(ttAA);

                    if (strTicket.Contains("DocIssuance_IssueTicket"))
                    {
                        strPNR = SendRetrievePNR(ttAA);

                        if (strPNR.Contains("INVALID") || strPNR.Contains("Error"))
                        {
                            strPNR = SendRetrivePNRbyRL(ttAA, strRead);
                        }
                    }

                    strResponse = strResponse.Contains("OK TICKETED") || strResponse.Contains("OK ETICKET")
                        ? strResponse.Replace("</DocIssuance_IssueTicketReply>", String.Format("{0}</DocIssuance_IssueTicketReply>", strPNR))
                        : strResponse.Replace("</Command_CrypticReply>", String.Format("{0}</Command_CrypticReply>", strPNR));
                }
                #endregion

                //***************************************************************** 
                // Transform Native Amadeus IssueTicket Response into OTA Response* 
                //***************************************************************** 

                try
                {
                    var tagToReplace = strRequest.Contains("DocIssuance_IssueTicket") ? "</DocIssuance_IssueTicketReply>" : "</Command_CrypticReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_IssueTicketSessionedRS.xsl");
                    strResponse = strResponse.Replace("999999999999", ConversationID);
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
                        ConversationID = null;
                        ttAA = null;
                    }
                }

                if (ttProviderSystems.LogNative)
                {
                    var strMSG = Message;
                    TripXMLTools.TripXMLLog.LogMessage("IssueTicket", ref strMSG, RequestTime, DateTime.Now, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }

                return strResponse;
            }
            catch (Exception exx)
            {
                throw exx;
            }
        }

        public string VoidTicket()
        {
            XmlElement oRootPNR = null;
            XmlElement oRoot = null;
            try
            {
                XmlElement oRootTicket = null;
                string strPNR = "";
                string strFinalResp = "";
                string strVoidTktReply = "";
                DateTime RequestTime = DateTime.Now;

                string strRequest = SetRequest("AmadeusWS_VoidTicketRQ.xsl");

                XmlDocument oDocMain = new XmlDocument();
                oDocMain.LoadXml(Request);
                XmlElement oRootMain = oDocMain.DocumentElement;

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                try
                {
                    var oDoc = new XmlDocument();
                    oDoc.LoadXml(strRequest);
                    oRoot = oDoc.DocumentElement;

                    if (oRoot.SelectSingleNode("PNR") != null)
                    {
                        strPNR = oRoot.SelectSingleNode("PNR").InnerXml;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Loading Transformed Request XML Document. {ex.Message}");
                }
                finally
                {
                    GC.Collect();
                }

                //********************** 
                // Create Session * 
                //********************** 
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                int i = 1;

                if (!string.IsNullOrEmpty(strPNR))
                {
                    string strPNRResp = SendRetrivePNRbyRL(ttAA, strPNR);
                    XmlDocument oDocPNR = new XmlDocument();
                    oDocPNR.LoadXml(strPNRResp);
                    oRootPNR = oDocPNR.DocumentElement;
                }

                string strResponse = "";
                foreach (XmlNode nd in oRoot.SelectNodes("Ticket"))
                {
                    string strCMD = "";
                    string strRTT = "";
                    string strVDT = "";
                    string strTicketNo = "";

                    if (nd.SelectSingleNode("CMD") != null)
                    {
                        strCMD = nd.SelectSingleNode("CMD").InnerXml;
                        strTicketNo = oRootMain.SelectSingleNode($"Tickets/TicketNumber[position()={i}]").InnerText;

                        if (strTicketNo.Length == 13)
                        {
                            strTicketNo = $"{strTicketNo.Substring(0, 3)}-{strTicketNo.Substring(3)}";
                        }
                    }
                    else
                    {
                        strRTT = nd.SelectSingleNode("RTT").InnerXml;
                        strVDT = nd.SelectSingleNode("VDT").InnerXml;
                        strTicketNo = nd.SelectSingleNode("RTT").SelectSingleNode("Ticket_ProcessETicket").SelectSingleNode("ticketInfoGroup").SelectSingleNode("ticket").SelectSingleNode("documentDetails").SelectSingleNode("number").InnerText;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(strRTT))
                        {
                            strResponse = SendETicketProcess(ttAA, strRTT);
                            Message += strResponse;

                            var oDocTicket = new XmlDocument();
                            oDocTicket.LoadXml(strResponse);
                            oRootTicket = oDocTicket.DocumentElement;

                            if (oRootTicket.SelectSingleNode("msgActionDetails").SelectSingleNode("responseType").InnerText == "3")
                            {
                                strResponse = SendCancelDocument(ttAA, strVDT);

                                oDocTicket = new XmlDocument();
                                oDocTicket.LoadXml(strResponse);
                                oRootTicket = oDocTicket.DocumentElement;

                                if (oRootTicket.SelectSingleNode("transactionResults").SelectSingleNode("responseDetails").SelectSingleNode("statusCode").InnerText == "O")
                                {
                                    strFinalResp = $"{strFinalResp}<Ticket Number=\"{strTicketNo}\" Status=\"Void\" AuthorizationCode=\"1111111111111\"/>";
                                }
                                else
                                {
                                    strFinalResp = $"{strFinalResp}<Ticket Number=\"{strTicketNo}\" Status=\"NotVoid\"/>";
                                    if (oRootTicket.SelectSingleNode("transactionResults/errorGroup/errorWarningDescription/freeText") != null)
                                    {
                                        strVoidTktReply = $"<Errors><Error>{oRootTicket.SelectSingleNode("transactionResults/errorGroup/errorWarningDescription/freeText").InnerText}</Error></Errors>";
                                    }
                                }
                            }
                            else
                            {
                                strFinalResp = $"{strFinalResp}<Ticket Number=\"{strTicketNo}\" Status=\"NotVoid\"/>";
                            }
                        }
                        else
                        {
                            i++;

                            string strTktLine = "";

                            if (oRootPNR.SelectSingleNode($"dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA'][contains(otherDataFreetext/longFreetext,'{strTicketNo}')]") != null)
                            {
                                strTktLine = oRootPNR.SelectSingleNode($"dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA'][contains(otherDataFreetext/longFreetext,'{strTicketNo}')]/elementManagementData/lineNumber").InnerText;
                                strCMD = strCMD.Replace("LTKT", $"L{strTktLine}");

                                strResponse = SendCommandCryptically(ttAA, strCMD);
                                //strResponse = ttAA.SendMessage(strCMD, "", ttProviderSystems.AmadeusWSSchema[Command_Cryptic], ConversationID);
                                Message += strResponse;

                                var oDocTicket = new XmlDocument();
                                oDocTicket.LoadXml(strResponse);
                                oRootTicket = oDocTicket.DocumentElement;
                            }

                            strFinalResp = oRootTicket.SelectSingleNode("longTextString").SelectSingleNode("textStringDetails").InnerText.Contains("OK")
                                ? $"{strFinalResp}<Ticket Number=\"{strTicketNo}\" Status=\"Void\" AuthorizationCode=\"1111111111111\"/>"
                                : $"{strFinalResp}<Ticket Number=\"{strTicketNo}\" Status=\"NotVoid\"/>";

                        }

                    }
                    catch (Exception)
                    {
                        strFinalResp += $"<Ticket Number=\"{strTicketNo}\" Status=\"NotVoid\"/>";
                    }
                }

                //********************** 
                // Close Session 
                //********************** 
                if (!inSession)
                {
                    ttAA.CloseSession(ConversationID);
                    ConversationID = null;
                    ttAA = null;
                }

                //***************************************************************** 
                // Transform Native Amadeus IssueTicket Response into OTA Response * 
                //***************************************************************** 

                try
                {
                    //***********************************************
                    // First if block was not there in local code
                    //***********************************************

                    if (string.IsNullOrEmpty(strFinalResp))
                        strResponse = $"<TT_VoidTicketRS Version=\"1.0\">{strVoidTktReply}<Warnings><Warning>Ticket not void</Warning></Warnings></TT_VoidTicketRS>";
                    else if (strFinalResp.Contains("NotVoid"))
                        strResponse = $"<TT_VoidTicketRS Version=\"1.0\">{strVoidTktReply}<Warnings><Warning>Ticket not void</Warning></Warnings>{strFinalResp}</TT_VoidTicketRS>";
                    else
                        strResponse = $"<TT_VoidTicketRS Version=\"1.0\"><Success/>{strFinalResp}</TT_VoidTicketRS>";
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }

                if (ttProviderSystems.LogNative)
                {
                    var strMSG = Message;
                    TripXMLTools.TripXMLLog.LogMessage("IssueTicket", ref strMSG, RequestTime, DateTime.Now, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }

                return strResponse;
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                throw exx;
            }
            finally
            {
                GC.Collect();
            }
        }

        public string RefundTicket()
        {
            try
            {
                string strRequest = SetRequest("AmadeusWS_RefundTicketRQ.xsl");
                DateTime RequestTime = DateTime.Now;

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                XmlDocument oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                XmlElement oRoot = oDoc.DocumentElement;

                //********************** 
                // Create Session * 
                //********************** 
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                strRequest = oRoot.SelectSingleNode("Cryptic").InnerXml;
                string strResponse = SendRequestCryptically(ttAA, strRequest);
                Message += strResponse;

                //****************************************************************** 
                // Transform Native Amadeus IssueTicket Response into OTA Response * 
                //****************************************************************** 
                try
                {
                    var tagToReplace = "</Command_CrypticReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_RefundTicketRS.xsl", false);
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
                        ConversationID = null;
                        ttAA = null;
                    }
                }

                if (ttProviderSystems.LogNative)
                {
                    var strMSG = Message;
                    TripXMLTools.TripXMLLog.LogMessage("IssueTicket", ref strMSG, RequestTime, DateTime.Now, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }

                return strResponse;
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                throw exx;
            }
            finally
            {
                GC.Collect();
            }
        }

        public string ReissueTicket()
        {
            try
            {

                DateTime RequestTime = DateTime.Now;
                string strAddElements = "";
                string strRead = "";
                string strPNR = "";
                string strTickets = "";

                string strRequest = SetRequest("AmadeusWS_ReissueTicketRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                XmlDocument oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                XmlElement oRoot = oDoc.DocumentElement;

                //Get Presetted Elements;
                strAddElements = oRoot.SelectSingleNode("PNR_AddMultiElements").InnerXml;
                strRead = oRoot.SelectSingleNode("PNRRead").InnerXml;
                strTickets = oRoot.SelectSingleNode("Ticket").InnerXml;

                //********************** 
                // Create Session * 
                //********************** 
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                string strResponse;
                //******************** 
                // Retrieve the PNR * 
                //********************                 
                Message += strRead;
                strResponse = SendRetrivePNRbyRL(ttAA, strRead);
                Message += strResponse;
                strPNR = strResponse;

                try
                {
                    strResponse = SendAutomaticUpdate(ttAA, strRequest);
                    Message += strResponse;

                    //The PNR requires to be filed before the ticket can be exchanged.
                    strResponse = SendAddMultiElements(ttAA, strAddElements);

                    oDoc = new XmlDocument();
                    oDoc.LoadXml(strTickets);
                    oRoot = oDoc.DocumentElement;

                    foreach (XmlNode oNode in oRoot.SelectSingleNode("Ticket"))
                    {
                        var strTicket = oNode.InnerXml;

                        strResponse = strTicket.StartsWith("<DocIssuance_IssueTicket>")
                            ? SendIssueTicket(ttAA, strTicket)
                            : SendRequestCryptically(ttAA, strTickets);

                        if (!strResponse.Contains("OK TICKETED") || !strResponse.Contains("OK ETICKET"))
                            throw new Exception("Failed to ReIssue ticket.");
                    }

                    Message += strResponse;

                    if (strResponse.Contains("OK "))
                    {
                        if (strTickets.Contains("DocIssuance_IssueTicket"))
                        {
                            strPNR = SendRetrievePNR(ttAA);

                            if (strPNR.Contains("INVALID") || strPNR.Contains("Error"))
                            {
                                strPNR = SendRetrivePNRbyRL(ttAA, strRead);
                            }
                        }

                        strResponse = strResponse.Contains("OK TICKETED") || strResponse.Contains("OK ETICKET")
                            ? strResponse.Replace("</DocIssuance_IssueTicketReply>", String.Format("{0}</DocIssuance_IssueTicketReply>", strPNR))
                            : strResponse.Replace("</Command_CrypticReply>", String.Format("{0}</Command_CrypticReply>", strPNR));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error sending messages to Amadeus.\r\n{ex.Message}");
                }

                //***************************************************************** 
                // Transform Native Amadeus IssueTicket Response into OTA Response* 
                //***************************************************************** 

                try
                {
                    var tagToReplace = strRequest.Contains("DocIssuance_IssueTicketReply") ? "</DocIssuance_IssueTicketReply>" : "</Command_CrypticReply>";
                    if (inSession)
                        strResponse = strResponse.Replace(tagToReplace, $"<ConversationID>{ConversationID}</ConversationID>{tagToReplace}");

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_IssueTicketSessionedRS.xsl");
                    strResponse = strResponse.Replace("999999999999", ConversationID);
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
                        ConversationID = null;
                        ttAA = null;
                    }
                }

                if (ttProviderSystems.LogNative)
                {
                    var strmsg = Message;
                    TripXMLTools.TripXMLLog.LogMessage("IssueTicket", ref strmsg, RequestTime, DateTime.Now, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }

                return strResponse;
            }
            catch (Exception exx)
            {
                //TODO: ReEngeneer Arror Log Collection
                //addLog($"<M>{mstrRequest}<BL/>", ttProviderSystems.UserID);
                throw exx;
            }
            finally
            {
                GC.Collect();
            }
        }

        public string DisplayTicket()
        {
            string strResponse = "";
            string strFinalResp = "";
            try
            {
                XmlDocument oDocTicket = null;
                XmlElement oRootTicket = null;
                DateTime RequestTime = DateTime.Now;
                string strRequest = SetRequest("AmadeusWS_DisplayTicketRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");
                XmlDocument oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                XmlElement oRoot = oDoc.DocumentElement;

                //********************** 
                // Create Session * 
                //********************** 
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);
                try
                {
                    foreach (XmlNode nd in oRoot.SelectNodes("Ticket"))
                    {
                        string tktNumber = nd.SelectSingleNode("RTT/Ticket_ProcessETicket/ticketInfoGroup/ticket/documentDetails/number").InnerText;
                        strRequest = nd.SelectSingleNode("RTT").InnerXml;
                        strResponse = SendETicketProcess(ttAA, strRequest);

                        if (strResponse.Contains("<Error>"))
                        {
                            strResponse = inSession
                            ? Version == "v03_"
                                        ? $"<TT_DisplayTicketRS Version=\"v03\"><Success/>{strResponse}<ConversationID>{ConversationID}</ConversationID></TT_DisplayTicketRS>"
                                        : $"<TT_DisplayTicketRS Version=\"1.0\"><Success/>{strResponse}<ConversationID>{ConversationID}</ConversationID></TT_DisplayTicketRS>"
                            : Version == "v03_"
                                        ? $"<TT_DisplayTicketRS Version=\"v03\"><Success/>{strResponse}</TT_DisplayTicketRS>"
                                        : $"<TT_DisplayTicketRS Version=\"1.0\"><Success/>{strResponse}</TT_DisplayTicketRS>";

                            return strResponse;
                        }

                        Message += strResponse;

                        oDocTicket = new XmlDocument();
                        oDocTicket.LoadXml(strResponse);
                        oRootTicket = oDocTicket.DocumentElement;

                        if (oRootTicket.SelectSingleNode("msgActionDetails").SelectSingleNode("responseType").InnerText == "3")
                        {
                            if (Version == "v03_")
                            {
                                strFinalResp += $"<Ticket Number=\"{nd.SelectSingleNode("RTT").SelectSingleNode("Ticket_ProcessETicket").SelectSingleNode("ticketInfoGroup").SelectSingleNode("ticket").SelectSingleNode("documentDetails").SelectSingleNode("number").InnerText}\">";

                                foreach (XmlNode tkt in oRootTicket.SelectNodes($"documentGroup/ticketInfoGroup[ticketInfo/documentDetails/number='{tktNumber}']/couponInfoGroup"))
                                {
                                    if (tkt.SelectSingleNode("couponInfo/couponDetails/cpnStatus") == null)
                                    {
                                        strFinalResp += "<Coupon Status=\"NotAvailable\"/>";
                                    }
                                    else
                                    {
                                        strFinalResp += $"<Coupon Airline=\"{tkt.SelectSingleNode("flightInfo/companyDetails/marketingCompany").InnerText}\" ";
                                        strFinalResp += $"Flight=\"{tkt.SelectSingleNode("flightInfo/flightIdentification/flightNumber").InnerText}\" ";
                                        strFinalResp += $"Departure=\"{tkt.SelectSingleNode("flightInfo/boardPointDetails/trueLocationId").InnerText}\" ";

                                        if (tkt.SelectSingleNode("flightInfo/flightIdentification/flightNumber").InnerText != "OPEN")
                                        {
                                            string depdate = $"20{tkt.SelectSingleNode("flightInfo/flightDate/departureDate").InnerText.Substring(4)}-{tkt.SelectSingleNode("flightInfo/flightDate/departureDate").InnerText.Substring(2, 2) + "-" + tkt.SelectSingleNode("flightInfo/flightDate/departureDate").InnerText.Substring(0, 2) + "T" + tkt.SelectSingleNode("flightInfo/flightDate/departureTime").InnerText.Substring(0, 2)}:{tkt.SelectSingleNode("flightInfo/flightDate/departureTime").InnerText.Substring(2)}:00";
                                            strFinalResp += $"DepartureDateTime=\"{depdate}\" ";
                                        }

                                        strFinalResp += $"Arrival=\"{tkt.SelectSingleNode("flightInfo/offpointDetails/trueLocationId").InnerText}\" ";

                                        string strStatus = tkt.SelectSingleNode("couponInfo/couponDetails/cpnStatus").InnerText;

                                        if (strStatus == "AL")
                                            strStatus = "AirportControl";
                                        else if (strStatus == "B")
                                            strStatus = "FlownUsed";
                                        else if (strStatus == "BD")
                                            strStatus = "Boarded";
                                        else if (strStatus == "CK")
                                            strStatus = "CheckedIn";
                                        else if (strStatus == "E")
                                            strStatus = "ExchangedReissued";
                                        else if (strStatus == "I")
                                            strStatus = "OriginalIssue";
                                        else if (strStatus == "IO")
                                            strStatus = "IrregularOperations";
                                        else if (strStatus == "OK")
                                            strStatus = "Confirmed";
                                        else if (strStatus == "PE")
                                            strStatus = "PrintExchange";
                                        else if (strStatus == "PR")
                                            strStatus = "Printed";
                                        else if (strStatus == "RF")
                                            strStatus = "Refunded";
                                        else if (strStatus == "RQ")
                                            strStatus = "Requested";
                                        else if (strStatus == "V")
                                            strStatus = "Void";
                                        else
                                            strStatus = "NotAvailable";

                                        strFinalResp += $"Status=\"{strStatus}\"/>";
                                    }
                                }

                                strFinalResp += "</Ticket>";
                            }
                            else
                            {
                                strFinalResp += $"<Ticket Number=\"{nd.SelectSingleNode("RTT").SelectSingleNode("Ticket_ProcessETicket").SelectSingleNode("ticketInfoGroup").SelectSingleNode("ticket").SelectSingleNode("documentDetails").SelectSingleNode("number").InnerText}\" Status=\"";

                                if (oRootTicket.SelectSingleNode("documentGroup/ticketInfoGroup/couponInfoGroup/couponInfo/couponDetails/cpnStatus") != null)
                                {
                                    string strStatus = oRootTicket.SelectSingleNode("documentGroup/ticketInfoGroup/couponInfoGroup/couponInfo/couponDetails/cpnStatus").InnerText;

                                    if (strStatus == "AL")
                                        strStatus = "AirportControl";
                                    else if (strStatus == "B")
                                        strStatus = "FlownUsed";
                                    else if (strStatus == "BD")
                                        strStatus = "Boarded";
                                    else if (strStatus == "CK")
                                        strStatus = "CheckedIn";
                                    else if (strStatus == "E")
                                        strStatus = "ExchangedReissued";
                                    else if (strStatus == "I")
                                        strStatus = "OriginalIssue";
                                    else if (strStatus == "IO")
                                        strStatus = "IrregularOperations";
                                    else if (strStatus == "OK")
                                        strStatus = "Confirmed";
                                    else if (strStatus == "PE")
                                        strStatus = "PrintExchange";
                                    else if (strStatus == "PR")
                                        strStatus = "Printed";
                                    else if (strStatus == "RF")
                                        strStatus = "Refunded";
                                    else if (strStatus == "RQ")
                                        strStatus = "Requested";
                                    else if (strStatus == "V")
                                        strStatus = "Void";
                                    else
                                        strStatus = "NotAvailable";

                                    strFinalResp += $"{strStatus}\"/>";
                                }
                                else
                                {
                                    strFinalResp += "NotFound\"/>";
                                }
                            }
                        }
                        else
                        {
                            strFinalResp += $"<Ticket Number=\"{nd.SelectSingleNode("RTT").SelectSingleNode("Ticket_ProcessETicket").SelectSingleNode("ticketInfoGroup").SelectSingleNode("ticket").SelectSingleNode("documentDetails").SelectSingleNode("number").InnerText}\" Status=\"NotFound\"/>";
                        }
                    }

                    //****************************************************************** 
                    // Transform Native Amadeus IssueTicket Response into OTA Response * 
                    //****************************************************************** 

                    strResponse = inSession
                        ? Version == "v03_"
                                    ? $"<TT_DisplayTicketRS Version=\"v03\"><Success/>{strFinalResp}<ConversationID>{ConversationID}</ConversationID></TT_DisplayTicketRS>"
                                    : $"<TT_DisplayTicketRS Version=\"1.0\"><Success/>{strFinalResp}<ConversationID>{ConversationID}</ConversationID></TT_DisplayTicketRS>"
                        : Version == "v03_"
                                    ? $"<TT_DisplayTicketRS Version=\"v03\"><Success/>{strFinalResp}</TT_DisplayTicketRS>"
                                    : $"<TT_DisplayTicketRS Version=\"1.0\"><Success/>{strFinalResp}</TT_DisplayTicketRS>";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (!inSession)
                    {
                        ttAA.CloseSession(ConversationID);
                        ConversationID = null;
                        ttAA = null;
                    }
                }

                

                if (ttProviderSystems.LogNative)
                {
                    var strmsg = Message;
                    TripXMLTools.TripXMLLog.LogMessage("IssueTicket", ref strmsg, RequestTime, DateTime.Now, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }

                return strResponse;
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                throw exx;
            }
            finally
            {
                GC.Collect();
            }
        }

        public string StoredFareBuild()
        {
            string strResponse = "";

            //***************************************************************** 
            // Transform OTA StoredFareBuild Request into Native Amadeus Request * 
            //***************************************************************** 
            try
            {
                string strSFResponse = string.Empty;
                string strResponseTST = string.Empty;
                string strRequest = SetRequest("AmadeusWS_PNRReadRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // ******************* 
                // Create Session * 
                // ******************* 
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //**************************************** 
                // Send Retreive PNR request to Amadeus * 
                //**************************************** 

                var strPNRReply = SendRetrivePNRbyRL(ttAA, strRequest);
                strRequest = CoreLib.TransformXML(Request, XslPath, $"{Version}AmadeusWS_StoredFareBuildRQ.xsl", false);
                strSFResponse = SendRequestCryptically(ttAA, strRequest);

                if (strSFResponse.Contains("<MessagesOnly_Reply>"))
                {
                    strRequest = "<PNR_AddMultiElements><pnrActions><optionCode>11</optionCode></pnrActions><dataElementsMaster><marker1></marker1><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";
                    strResponse = SendAddMultiElements(ttAA, strRequest);
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_TB_Errors.xsl", false);

                    if (!string.IsNullOrEmpty(strResponse))
                    {
                        if (strResponse.IndexOf("<Error") >= 0)
                        {
                            // Fatal Error 
                            strSFResponse = $"<Errors>{strResponse}</Errors>";
                        }
                    }

                    strResponseTST = SendDisplayTST(ttAA);
                }

                //************************************************************************** 
                // Transform Native Amadeus StoredFareBuild Response into OTA Response * 
                //************************************************************************** 
                try
                {
                    strResponseTST = inSession 
                        ? strResponseTST.Replace("</Ticket_DisplayTSTReply>", $"{strPNRReply}<ConversationID>{ConversationID}</ConversationID></Ticket_DisplayTSTReply>")
                        : strResponseTST.Replace("</Ticket_DisplayTSTReply>", $"{strPNRReply}</Ticket_DisplayTSTReply>");

                    strResponse = CoreLib.TransformXML(strResponseTST, XslPath, $"{Version}AmadeusWS_StoredFareBuildRS.xsl", false);
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
                        ConversationID = null;
                        ttAA = null;
                    }
                }
            }
            catch (Exception exx)
            {
                throw exx;
            }
            finally
            {
                GC.Collect();
            }

            return strResponse;
        }

        public string StoredFareUpdate()
        {
            string ConversationID = "";

            //***************************************************************** 
            // Transform OTA StoredFareUpdate Request into Native Amadeus Request * 
            //***************************************************************** 
            try
            {
                string strRequest = SetRequest("AmadeusWS_PNRReadRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                // ******************* 
                // Create Session * 
                // ******************* 
                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //**************************************** 
                // Send Retreive PNR request to Amadeus * 
                //**************************************** 
                string strSFResponse = SendRetrivePNRbyRL(ttAA, strRequest);
                string strNativePNRReply = strSFResponse;


                string strResponse = "";
                if (!strSFResponse.Contains("<MessagesOnly_Reply>"))
                {
                    string strResponseTST = "";
                    if (strSFResponse.Contains("<longFreetext>--- TST "))
                    {
                        strResponseTST = SendDisplayTST(ttAA);
                    }

                    try
                    {
                        strRequest = strRequest.Replace("</OTA_StoredFareUpdateRQ>", $"{strSFResponse}{strResponseTST}</OTA_StoredFareUpdateRQ>");
                        strRequest = CoreLib.TransformXML(strRequest, XslPath, $"{Version}AmadeusWS_StoredFareUpdateRQ.xsl", false);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error Transforming OTA Request. {ex.Message}");
                    }

                    if (string.IsNullOrEmpty(strRequest))
                        throw new Exception("Transformation produced empty xml.");

                    //********************************************** 
                    // Send stored fare build request to Amadeus * 
                    //********************************************** 
                    if (strRequest.StartsWith("<Ticket_UpdateTST"))
                    {
                        strSFResponse = SendUpdateTST(ttAA, strRequest);
                    }
                    else
                    {
                        strSFResponse = SendRequestCryptically(ttAA, strRequest);
                        strSFResponse = SendDisplayTST(ttAA);
                    }

                    strResponse = CoreLib.TransformXML(strSFResponse, XslPath, $"{Version}AmadeusWS_TB_Errors.xsl", false);

                    //******************** 
                    //* Check for Errors * 
                    //******************** 
                    if (strResponse.Length > 0)
                    {
                        if (strResponse.IndexOf("<Error") >= 0)
                        {
                            // Fatal Error 
                            return BuildOTAResponse(strResponse);
                        }
                        else if (strResponse.IndexOf("<Warning>") >= 0)
                        {
                            Warnings += strResponse;
                        }
                    }

                    //********************************* 
                    //* Send End Transaction Request * 
                    //********************************* 
                    string strEndTransaction = "<PNR_AddMultiElements><pnrActions><optionCode>10</optionCode></pnrActions><dataElementsMaster><marker1></marker1><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRAVELTALK</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";
                    strResponse = SendAddMultiElements(ttAA, strEndTransaction);
                    native += strEndTransaction;

                    // Check for Errors 
                    strEndTransaction = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_TB_Errors.xsl", false);
                    if (strEndTransaction.Length > 0)
                    {
                        if (strEndTransaction.Contains("<Error"))
                        {
                            // Fatal Error 
                            return BuildOTAResponse(strEndTransaction);
                        }
                        else if (strEndTransaction.Contains("<Warning>"))
                        {
                            Warnings += strEndTransaction;
                        }
                    }

                    var oDocResp = new XmlDocument();
                    oDocResp.LoadXml(strResponse);
                    XmlElement oRootResp = oDocResp.DocumentElement;
                    XmlNode oNodeResp = oRootResp.SelectSingleNode("pnrHeader/reservationInfo/reservation/controlNumber");

                    if (oNodeResp != null)
                    {
                        string RecordLocator = oNodeResp.InnerText;
                        if (!string.IsNullOrEmpty(RecordLocator))
                        {
                            // Send Retreive Request 
                            string strRTV = $"<PNR_RetrieveByRecLoc><sbrRecLoc><reservation><controlNumber>{RecordLocator}</controlNumber></reservation></sbrRecLoc></PNR_RetrieveByRecLoc>";

                            Thread.Sleep(1000);
                            //double dTime = DateTime.Now.TimeOfDay.TotalSeconds;
                            //while (!(DateTime.Now.TimeOfDay.TotalSeconds - dTime > 1))
                            //{
                            //}

                            strResponse = SendRetrivePNRbyRL(ttAA, strRTV);
                            native += $"{strRTV}{strResponse}";
                        }
                    }


                    if (strResponse.Contains("<longFreetext>--- TST "))
                    {
                        strResponseTST = SendDisplayTST(ttAA);
                        native += $"{strRequest}{strResponseTST}";
                    }

                    //**************************************************************************** 
                    // Add Previous Errors and Warnings To Amadeus Native End Transact Response * 
                    //**************************************************************************** 
                    strNativePNRReply = strResponse.Replace("</PNR_RetrieveByRecLocReply>", $"{Errors}{Warnings}{strResponseTST}{Request}</PNR_RetrieveByRecLocReply>");
                    oDocResp = null;
                }

                //***************************************************************** 
                // Transform Native Amadeus StoredFareUpdate Response into OTA Response * 
                //***************************************************************** 
                try
                {
                    Version = "v03_";
                    strNativePNRReply = inSession
                        ? strNativePNRReply.Replace("</PNR_RetrieveByRecLocReply>", $"<ConversationID>{ConversationID}</ConversationID></PNR_RetrieveByRecLocReply>")
                        : strNativePNRReply;

                    strResponse = CoreLib.TransformXML(strNativePNRReply, XslPath, $"{Version}AmadeusWS_PNRReadRS.xsl", false);

                    return strResponse;
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
                        ConversationID = null;
                        ttAA = null;
                    }
                }
            }
            catch (Exception exx)
            {
                throw exx;
            }
            finally
            {
                GC.Collect();
            }
        }

        public string Update()
        {
            string strResponse = "";
            string strSegments = "";
            string strRTV = "";

            try
            {
                //********************* 
                //* Create Session * 
                //********************* 
                DateTime RequestTime = DateTime.Now;
                string strResponseTST = "";
                
                XmlDocument otaDoc = new XmlDocument();
                otaDoc.LoadXml(Request);
                XmlElement otaElement = otaDoc.DocumentElement;
                string strRequest = SetRequest("AmadeusWS_PNRReadRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);

                //******************************************** 
                //* Get Amadeus Native PNR Retrieve response * 
                //******************************************** 
                Message = strRequest;
                string strNativePNRReply = SendRetrivePNRbyRL(ttAA, strRequest);
                Message += strNativePNRReply;
                if (strNativePNRReply.StartsWith("<PNR_RetrieveByRecLocReply>"))
                {
                    //******************************* 
                    //Load OTA Modify XML document * 
                    //******************************* 
                    XmlDocument oDoc = new XmlDocument();
                    oDoc.LoadXml(Request);
                    XmlElement oRoot = oDoc.DocumentElement;
                    var oDocResp = new XmlDocument();
                    XmlElement oRootTemp = null;
                    if (oRoot.SelectSingleNode("Position/Element[@Operation='delete']") != null)
                    {
                        //******************************** 
                        //* Build PNR Retrieve xml msg * 
                        //******************************** 
                        //******************************************************************* 
                        //* Transform OTA Modify Request into Amadeus Native Delete Request * 
                        //******************************************************************* 
                        strRequest = CoreLib.TransformXML($"<UpdateDelete>{Request}{strNativePNRReply}</UpdateDelete>", XslPath, $"{Version}AmadeusWS_UpdateDeleteRQ.xsl", false);

                        if (strRequest.IndexOf("<Error>") != -1)
                        {
                            return BuildOTAResponse(strRequest.Substring(strRequest.IndexOf("<Error>"), (strRequest.IndexOf("</Error>") + 8) - strRequest.IndexOf("<Error>")));
                        }

                        //************************************** 
                        //* Send Amadeus Native Delete Request * 
                        //************************************** 
                        var oDocTemp = new XmlDocument();
                        oDocTemp.LoadXml(strRequest);
                        oRootTemp = oDocTemp.DocumentElement;

                        string strErrorResp = SendRequestSegment(ttAA, oRootTemp.SelectSingleNode("Cancel").InnerXml, "Delete", ttProviderSystems.AmadeusWSSchema[PNR_Cancel], ttProviderSystems.AmadeusWSSchema[PNR_Reply]);
                        strNativePNRReply = nativeResp.Replace("PNR_Reply", "PNR_RetrieveByRecLocReply");

                        //******************** 
                        //* Check for Errors * 
                        //******************** 
                        if (strErrorResp.Length > 0)
                        {
                            if (strErrorResp.IndexOf("<Error") >= 0)
                            {
                                // Fatal Error 
                                return BuildOTAResponse(strErrorResp);
                            }
                            else if (strErrorResp.IndexOf("<Warning>") >= 0)
                            {
                                Warnings += strErrorResp;
                            }
                        }

                        strErrorResp = SendRequestSegment(ttAA, oRootTemp.SelectSingleNode("RF").InnerXml, "ReceivedFrom", ttProviderSystems.AmadeusWSSchema[PNR_AddMultiElements], ttProviderSystems.AmadeusWSSchema[PNR_Reply]);
                    }

                    XmlElement oRootResp = null;
                    if (oRoot.SelectSingleNode("Position/Element[@Operation='insert']") != null)
                    {
                        //******************************************************************* 
                        //* Transform OTA Modify Request into Amadeus Native Insert Request * 
                        //******************************************************************* 
                        strRequest = CoreLib.TransformXML($"<UpdateInsert>{Request}{strNativePNRReply}</UpdateInsert>", XslPath, $"{Version}AmadeusWS_UpdateInsertRQ.xsl", false);

                        oDocResp = new XmlDocument();
                        oDocResp.LoadXml(strNativePNRReply);
                        oRootResp = oDocResp.DocumentElement;

                        //******************** 
                        // Get All Requests * 
                        //******************** 
                        try
                        {
                            var oDocTemp = new XmlDocument();
                            oDocTemp.LoadXml(strRequest);
                            oRootTemp = oDocTemp.DocumentElement;

                            string strRF = oRootTemp.SelectSingleNode("RF").InnerXml;

                            // insert flights segments if any in request 
                            if (oRootTemp.SelectSingleNode("MasterPricer") != null)
                                strSegments = oRootTemp.SelectSingleNode("MasterPricer").InnerXml;

                            if (!string.IsNullOrEmpty(strSegments))
                            {
                                strResponse = SendRequestSegment(ttAA, strSegments, "Air", ttProviderSystems.AmadeusWSSchema[Air_SellFromRecommendation], ttProviderSystems.AmadeusWSSchema[Air_SellFromRecommendationReply]);
                                // Fatal Error 
                                if (strResponse.Length > 0)
                                {
                                    strResponse = BuildOTAResponse(strResponse);
                                    return strResponse;
                                }

                                if (oRootTemp.SelectSingleNode("MCT") != null)
                                {
                                    string strMCT = oRootTemp.SelectSingleNode("MCT").InnerXml;
                                    strResponse = SendRequestSegment(ttAA, strMCT, "MCT", ttProviderSystems.AmadeusWSSchema[Command_Cryptic], ttProviderSystems.AmadeusWSSchema[Command_CrypticReply]);

                                    // Fatal Error 
                                    if (strResponse.Length > 0)
                                    {
                                        strResponse = BuildOTAResponse(strResponse);
                                        return strResponse;
                                    }
                                }
                            }

                            if (oRootTemp.SelectSingleNode("CrypticCommand") != null)
                            {
                                string strCryptic = oRootTemp.SelectSingleNode("CrypticCommand").InnerXml;
                                strResponse = SendRequestSegment(ttAA, strCryptic, "FO", ttProviderSystems.AmadeusWSSchema[Command_Cryptic], ttProviderSystems.AmadeusWSSchema[Command_CrypticReply]);

                                // Fatal Error 
                                if (strResponse.Length > 0)
                                {
                                    strResponse = BuildOTAResponse(strResponse);
                                    return strResponse;
                                }
                            }
                            if (oRootTemp.SelectSingleNode("ValidAfterBefore") != null)
                            {

                                string strValidateAfterBefore = oRootTemp.SelectSingleNode("ValidAfterBefore").InnerXml;
                                string strResponseTST1 = SendDisplayTST(ttAA);

                                var xmlTST = new XmlDocument();
                                xmlTST.LoadXml(strResponseTST1);
                                XmlElement tstRoot;
                                tstRoot = xmlTST.DocumentElement;

                                string strdate = "";
                                foreach (XmlNode nd in oRoot.SelectNodes("Position"))
                                {
                                    if ((nd.SelectSingleNode("Element").Attributes["Operation"].Value.ToLower() == "insert") && (nd.SelectSingleNode("Element").Attributes["Child"].Value.ToLower() == "farevalidities"))
                                    {
                                        foreach (XmlNode nd1 in nd.SelectSingleNode("Element").SelectSingleNode("FareValidities").SelectNodes("FareValidity"))
                                        {
                                            if (nd1.Attributes["ValidityReason"].Value == "Before")
                                            {
                                                strdate = $"{nd1.Attributes["ValidityDate"].Value.Substring(8, 2)}{getMonth(Int32.Parse(nd1.Attributes["ValidityDate"].Value.Substring(5, 2)))}{strdate}";
                                            }
                                            if (nd1.Attributes["ValidityReason"].Value == "After")
                                            {
                                                strdate = $"{strdate}{nd1.Attributes["ValidityDate"].Value.Substring(8, 2)}{getMonth(Int32.Parse(nd1.Attributes["ValidityDate"].Value.Substring(5, 2)))}";
                                            }
                                        }
                                    }
                                }
                                strdate = $"V{strdate}";
                                strValidateAfterBefore = strValidateAfterBefore.Replace("ValidDate", strdate);
                                int ifareList = 1;
                                string strValidateAfterBeforeTmp = "";
                                foreach (XmlNode nd in tstRoot.SelectNodes("fareList"))
                                {
                                    strValidateAfterBeforeTmp = strValidateAfterBefore.Replace("Tx", $"T{ifareList}");
                                    strResponse = SendRequestSegment(ttAA, strValidateAfterBeforeTmp, "ValidBeforeAfter", ttProviderSystems.AmadeusWSSchema[Command_Cryptic], ttProviderSystems.AmadeusWSSchema[Command_CrypticReply]);

                                    // Fatal Error 
                                    if (strResponse.Length > 0)
                                    {
                                        strResponse = BuildOTAResponse(strResponse);
                                        return strResponse;
                                    }
                                    ifareList++;
                                }

                                // Fatal Error 
                                if (strResponse.Length > 0)
                                {
                                    strResponse = BuildOTAResponse(strResponse);
                                    return strResponse;
                                }
                            }

                            // insert other elements if any 
                            if (oRootTemp.SelectSingleNode("MultiElements") != null)
                            {
                                string strMultiElements = oRootTemp.SelectSingleNode("MultiElements").InnerXml;
                                strResponse = SendRequestSegment(ttAA, strMultiElements, "MultiElements", ttProviderSystems.AmadeusWSSchema[PNR_AddMultiElements], ttProviderSystems.AmadeusWSSchema[PNR_Reply]);

                                // Fatal Error 
                                if (strResponse.Length > 0)
                                {
                                    strResponse = BuildOTAResponse(strResponse);
                                    return strResponse;
                                }
                            }

                            else
                            {
                                strResponse = SendRequestSegment(ttAA, strRF, "ReceivedFrom", ttProviderSystems.AmadeusWSSchema[PNR_AddMultiElements], ttProviderSystems.AmadeusWSSchema[PNR_Reply]);
                            }

                            strNativePNRReply = nativeResp;
                            strNativePNRReply = nativeResp.Replace("PNR_Reply", "PNR_RetrieveByRecLocReply");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error Loading Transformed Request XML Document.\r\n{ex.Message}");
                        }
                    }

                    if (oRoot.SelectSingleNode("Position/Element[@Operation='modify']") != null)
                    {
                        //******************************** 
                        //* Build PNR Retrieve xml msg * 
                        //******************************** 
                        //******************************************************************* 
                        //* Transform OTA Modify Request into Amadeus Native Delete Request * 
                        //******************************************************************* 
                        strRequest = CoreLib.TransformXML($"<UpdateModify>{Request}{strNativePNRReply}</UpdateModify>", XslPath, $"{Version}AmadeusWS_UpdateModifyRQ.xsl", false);

                        oDocResp.LoadXml(strRequest);
                        oRootResp = oDocResp.DocumentElement;

                        //************************************** 
                        //* Send Amadeus Native Delete Request * 
                        //************************************** 
                        foreach (XmlNode oNodeResp1 in oRootResp.ChildNodes)
                        {
                            //strErrorResp = SendRequestSegment(oNodeResp1.OuterXml, "Modify");
                            if (oNodeResp1.OuterXml.StartsWith("<Command_Cryptic"))
                                strResponse = SendRequestSegment(ttAA, oNodeResp1.OuterXml, "PNR", ttProviderSystems.AmadeusWSSchema[Command_Cryptic], ttProviderSystems.AmadeusWSSchema[Command_CrypticReply]);
                            else if (oNodeResp1.OuterXml.StartsWith("<PNR_Cancel"))
                                strResponse = SendRequestSegment(ttAA, oNodeResp1.OuterXml, "PNR", ttProviderSystems.AmadeusWSSchema[PNR_Cancel], ttProviderSystems.AmadeusWSSchema[PNR_Reply]);
                            else if (oNodeResp1.OuterXml.StartsWith("<PNR_AddMultiElements"))
                                strResponse = SendRequestSegment(ttAA, oNodeResp1.OuterXml, "PNRModifyAddSeg", ttProviderSystems.AmadeusWSSchema[PNR_AddMultiElements], ttProviderSystems.AmadeusWSSchema[PNR_Reply]);


                            //******************** 
                            //* Check for Errors * 
                            //******************** 
                            if (strResponse.Length > 0)
                            {
                                if (strResponse.IndexOf("<Error") >= 0)
                                {
                                    // Fatal Error 
                                    return BuildOTAResponse(strResponse);
                                }
                                else if (strResponse.IndexOf("<Warning>") >= 0)
                                {
                                    Warnings += strResponse;
                                }
                            }

                        }
                    }

                    //***************************** 
                    //* Build Native ET xml msg * 
                    //***************************** 
                    string strEndTransaction = "<PNR_AddMultiElements><pnrActions><optionCode>10</optionCode></pnrActions><dataElementsMaster><marker1/><dataElementsIndiv><elementManagementData><segmentName>RF</segmentName></elementManagementData><freetextData><freetextDetail><subjectQualifier>3</subjectQualifier><type>P22</type></freetextDetail><longFreetext>TRIPXML</longFreetext></freetextData></dataElementsIndiv></dataElementsMaster></PNR_AddMultiElements>";

                    //********************************* 
                    //* Send End Transaction Request * 
                    //********************************* 
                    Message += strEndTransaction;
                    strResponse = SendAddMultiElements(ttAA, strEndTransaction);
                    Message += strResponse;
                    native += $"{strEndTransaction}{strResponse}";

                    // Check for Errors 
                    strEndTransaction = CoreLib.TransformXML(strResponse, XslPath, $"{Version}AmadeusWS_TB_Errors.xsl", false);

                    if (strEndTransaction.Length > 0)
                    {
                        if (strEndTransaction.Contains("<Error"))
                        {
                            // Fatal Error 
                            return BuildOTAResponse(strEndTransaction);
                        }
                        else if (strEndTransaction.Contains("<Warning>"))
                        {
                            Warnings += strEndTransaction;
                        }
                    }

                    oDocResp = new XmlDocument();
                    oDocResp.LoadXml(strResponse);
                    oRootResp = oDocResp.DocumentElement;
                    XmlNode oNodeResp = oRootResp.SelectSingleNode("pnrHeader/reservationInfo/reservation/controlNumber");

                    if (oNodeResp != null)
                    {
                        string RecordLocator = oNodeResp.InnerText;

                        if (!string.IsNullOrEmpty(RecordLocator))
                        {
                            // Send Retreive Request 
                            strRTV = $"<PNR_RetrieveByRecLoc><sbrRecLoc><reservation><controlNumber>{RecordLocator}</controlNumber></reservation></sbrRecLoc></PNR_RetrieveByRecLoc>";

                            /*** OLD CODE *****
                            double dTime = 0;
                            dTime = DateTime.Now.TimeOfDay.TotalSeconds;
                            while (!(DateTime.Now.TimeOfDay.TotalSeconds - dTime > 1))
                            {
                            }
                            ****************/
                            Thread.Sleep(1000);
                            Message += strRTV;
                            strResponse = SendRetrivePNRbyRL(ttAA, strRTV);
                            Message += strResponse;
                            native += $"{strRTV}{strResponse}";
                        }
                    }

                    if (strResponse.IndexOf("<longFreetext>--- TST ") != -1)
                    {
                        Message += strRequest;
                        strResponseTST = SendDisplayTST(ttAA);
                        Message += strResponseTST;
                        native += $"{strRTV}{strResponseTST}";
                    }
                    else
                    {
                        strResponseTST = "";
                    }

                    //**************************************************************************** 
                    // Add Previous Errors and Warnings To Amadeus Native End Transact Response * 
                    //**************************************************************************** 

                    strNativePNRReply = strResponse.Replace("</PNR_RetrieveByRecLocReply>", $"{Errors}{Warnings}{strResponseTST}{Request}</PNR_RetrieveByRecLocReply>");
                }

                try
                {
                    Version = "v03_";

                    strNativePNRReply = inSession
                        ? strNativePNRReply.Replace("</PNR_RetrieveByRecLocReply>", $"<ConversationID>{ConversationID}</ConversationID></PNR_RetrieveByRecLocReply>")
                        : strNativePNRReply;

                    strResponse = CoreLib.TransformXML(strNativePNRReply, XslPath, $"{Version}AmadeusWS_PNRReadRS.xsl", false);
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
                        ConversationID = null;
                        ttAA = null;
                    }
                }

                if (ttProviderSystems.LogNative)
                {
                    var strmsg = Message;
                    TripXMLTools.TripXMLLog.LogMessage("Update", ref strmsg, RequestTime, DateTime.Now, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }
            }
            catch (Exception exx)
            {
                addLog($"<M>{Request}<BL/>", ttProviderSystems.UserID);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRRead, exx.Message, ttProviderSystems);
            }
            finally
            {
                GC.Collect();
            }

            return strResponse;
        }

        public string UpdateSessioned()
        {
            string strResponse = "";
            string strSegments = "";
            string strErrEvent = "";

            try
            {
                DateTime RequestTime = DateTime.Now;
                XmlDocument otaDoc = new XmlDocument();
                XmlElement otaElement;
                otaDoc.LoadXml(Request);
                otaElement = otaDoc.DocumentElement;

                var strRequest = SetRequest("");
                XmlDocument oDoc = new XmlDocument();
                oDoc.LoadXml(strRequest);
                XmlElement oRoot = oDoc.DocumentElement;

                var ttAA = SetAdapter();
                bool inSession = SetConversationID(ttAA);


                //**************************** 
                // Retrieve existing PNR * 
                //**************************** 
                strErrEvent = "Error Transforming OTA PNRRead Request.";
                

                //******************************************** 
                //* Get Amadeus Native PNR Retrieve response * 
                //******************************************** 
                Message = "<PNR_Retrieve><retrievalFacts><retrieve><type>1</type></retrieve></retrievalFacts></PNR_Retrieve>";
                string strNativePNRReply = SendRetrievePNR(ttAA);
                strNativePNRReply = strNativePNRReply.Replace("PNR_Reply", "PNR_RetrieveByRecLocReply");
                Message += strNativePNRReply;
                strErrEvent = "Error sending Amadeus PNR Retrieve Request.";

                //******************************* 
                // Modify PNR - Delete elements * 
                //******************************* 
                strErrEvent = "Modify PNR - Delete elements Error.";

                XmlDocument oDocTemp = null;
                XmlElement oRootTemp = null;
                string strErrorResp = "";
                if (oRoot.SelectSingleNode("Position/Element[@Operation='delete']") != null)
                {
                    //******************************** 
                    //* Build PNR Retrieve xml msg * 
                    //******************************** 

                    //******************************************************************* 
                    //* Transform OTA Modify Request into Amadeus Native Delete Request * 
                    //******************************************************************* 
                    strRequest = CoreLib.TransformXML($"<UpdateDelete>{Request}{strNativePNRReply}</UpdateDelete>", XslPath, $"{Version}AmadeusWS_UpdateDeleteRQ.xsl", false);

                    if (strRequest.Contains("<Error>"))
                    {
                        return BuildOTAResponse(strRequest.Substring(strRequest.IndexOf("<Error>"), strRequest.IndexOf("</Error>") + 8 - strRequest.IndexOf("<Error>")));
                    }

                    //************************************** 
                    //* Send Amadeus Native Delete Request * 
                    //************************************** 
                    oDocTemp = new XmlDocument();
                    oDocTemp.LoadXml(strRequest);
                    oRootTemp = oDocTemp.DocumentElement;

                    strErrorResp = SendRequestSegment(ttAA, oRootTemp.SelectSingleNode("Cancel").InnerXml, "Delete", ttProviderSystems.AmadeusWSSchema[PNR_Cancel], ttProviderSystems.AmadeusWSSchema[PNR_Reply]);
                    strNativePNRReply = nativeResp.Replace("PNR_Reply", "PNR_RetrieveByRecLocReply");

                    //******************** 
                    //* Check for Errors * 
                    //******************** 
                    if (strErrorResp.Length > 0)
                    {
                        if (strErrorResp.IndexOf("<Error") >= 0)
                        {
                            // Fatal Error 
                            return BuildOTAResponse(strErrorResp);
                        }
                        else if (strErrorResp.IndexOf("<Warning>") >= 0)
                        {
                            Warnings += strErrorResp;
                        }
                    }

                    strErrorResp = SendRequestSegment(ttAA, oRootTemp.SelectSingleNode("RF").InnerXml, "ReceivedFrom", ttProviderSystems.AmadeusWSSchema[PNR_AddMultiElements], ttProviderSystems.AmadeusWSSchema[PNR_Reply]);

                    oDocTemp = null;
                }

                //******************************* 
                // Modify PNR - Insert elements * 
                //******************************* 
                strErrEvent = "Modify PNR - Insert elements Error.";

                XmlDocument oDocResp = null;
                XmlElement oRootResp = null;
                if (oRoot.SelectSingleNode("Position/Element[@Operation='insert']") != null)
                {
                    //******************************************************************* 
                    //* Transform OTA Modify Request into Amadeus Native Insert Request * 
                    //******************************************************************* 
                    strRequest = CoreLib.TransformXML($"<UpdateInsert>{Request}{strNativePNRReply}</UpdateInsert>", XslPath, $"{Version}AmadeusWS_UpdateInsertRQ.xsl", false);

                    oDocResp = new XmlDocument();
                    oDocResp.LoadXml(strNativePNRReply);
                    oRootResp = oDocResp.DocumentElement;

                    //******************** 
                    // Get All Requests * 
                    //******************** 

                    try
                    {
                        oDocTemp = new XmlDocument();
                        oDocTemp.LoadXml(strRequest);
                        oRootTemp = oDocTemp.DocumentElement;

                        string strRF = oRootTemp.SelectSingleNode("RF").InnerXml;

                        // insert flights segments if any in request 
                        if (oRootTemp.SelectSingleNode("MasterPricer") != null)
                            strSegments = oRootTemp.SelectSingleNode("MasterPricer").InnerXml;

                        if (!string.IsNullOrEmpty(strSegments))
                        {
                            strResponse = SendRequestSegment(ttAA, strSegments, "Air", ttProviderSystems.AmadeusWSSchema[Air_SellFromRecommendation], ttProviderSystems.AmadeusWSSchema[Air_SellFromRecommendationReply]);
                            // Fatal Error 
                            if (strResponse.Length > 0)
                            {
                                strResponse = BuildOTAResponse(strResponse);
                                return strResponse;
                            }

                            if (oRootTemp.SelectSingleNode("MCT") != null)
                            {
                                string strMCT = oRootTemp.SelectSingleNode("MCT").InnerXml;
                                strResponse = SendRequestSegment(ttAA, strMCT, "MCT", ttProviderSystems.AmadeusWSSchema[Command_Cryptic], ttProviderSystems.AmadeusWSSchema[Command_CrypticReply]);

                                // Fatal Error 
                                if (strResponse.Length > 0)
                                {
                                    strResponse = BuildOTAResponse(strResponse);
                                    return strResponse;
                                }
                            }
                        }

                        // insert other elements if any 
                        if (oRootTemp.SelectSingleNode("MultiElements") != null)
                        {
                            string strMultiElements = oRootTemp.SelectSingleNode("MultiElements").InnerXml;

                            //****************************************************************************
                            // The below given line's second string parameter was diffrence in loacl code
                            //***************************************************************************

                            strResponse = SendRequestSegment(ttAA, strMultiElements, "MultiElementsInsert", ttProviderSystems.AmadeusWSSchema[PNR_AddMultiElements], ttProviderSystems.AmadeusWSSchema[PNR_Reply]);

                            // Fatal Error 
                            //***************************************************
                            // Following if condition was diffrent in local code
                            //***************************************************
                            if (strResponse.Length > 0 && !strResponse.Contains("IS WAIT LIST"))
                            {
                                strResponse = BuildOTAResponse(strResponse);
                                return strResponse;
                            }
                            else
                                strResponse = "";
                        }
                        else
                        {
                            strResponse = SendRequestSegment(ttAA, strRF, "ReceivedFrom", ttProviderSystems.AmadeusWSSchema[PNR_AddMultiElements], ttProviderSystems.AmadeusWSSchema[PNR_Reply]);
                        }

                        strNativePNRReply = nativeResp.Replace("PNR_Reply", "PNR_RetrieveByRecLocReply");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error Loading Transformed Request XML Document.\r\n{ex.Message}");
                    }
                    finally
                    {
                        oDocResp = null;
                        oDocTemp = null;
                    }
                }

                //******************************* 
                // Modify PNR - Modify elements * 
                //******************************* 
                strErrEvent = "Modify PNR - Modify elements Error.";

                if (oRoot.SelectSingleNode("Position/Element[@Operation='modify']") != null)
                {
                    //******************************** 
                    //* Build PNR Retrieve xml msg * 
                    //******************************** 
                    //******************************************************************* 
                    //* Transform OTA Modify Request into Amadeus Native Delete Request * 
                    //******************************************************************* 
                    strRequest = CoreLib.TransformXML($"<UpdateModify>{Request}{strNativePNRReply}</UpdateModify>", XslPath, $"{Version}AmadeusWS_UpdateModifyRQ.xsl", false);

                    oDocResp = new XmlDocument();
                    oDocResp.LoadXml(strRequest);
                    oRootResp = oDocResp.DocumentElement;

                    //************************************** 
                    //* Send Amadeus Native Delete Request * 
                    //************************************** 
                    foreach (XmlNode oNodeResp1 in oRootResp.ChildNodes)
                    {
                        if (oNodeResp1.OuterXml.Contains("Command_Cryptic"))
                        {
                            strErrorResp = SendRequestSegment(ttAA, oNodeResp1.OuterXml, "Air", ttProviderSystems.AmadeusWSSchema[Command_Cryptic], ttProviderSystems.AmadeusWSSchema[Command_Cryptic]);
                        }
                        else if (oNodeResp1.OuterXml.Contains("PNR_Cancel"))
                        {
                            strErrorResp = SendRequestSegment(ttAA, oNodeResp1.OuterXml, "Air", ttProviderSystems.AmadeusWSSchema[PNR_Cancel], ttProviderSystems.AmadeusWSSchema[PNR_Cancel]);
                        }
                        else
                        {
                            strErrorResp = SendRequestSegment(ttAA, oNodeResp1.OuterXml, "Air", ttProviderSystems.AmadeusWSSchema[PNR_AddMultiElements], ttProviderSystems.AmadeusWSSchema[PNR_AddMultiElements]);
                        }

                        //******************** 
                        //* Check for Errors * 
                        //******************** 
                        if (strErrorResp.Length > 0)
                        {
                            if (strErrorResp.IndexOf("<Error") >= 0)
                            {
                                // Fatal Error 
                                return BuildOTAResponse(strErrorResp);
                            }
                            else if (strErrorResp.IndexOf("<Warning>") >= 0)
                            {
                                Warnings += strErrorResp;
                            }
                        }

                    }
                }

                strResponse = SendRetrievePNR(ttAA);
                Message += strResponse;
                native += $"{strRequest}{strResponse}";
                //strResponse = strNativePNRReply;

                string strResponseTST = "";
                if (strResponse.IndexOf("<longFreetext>--- TST ") != -1)
                {
                    strErrEvent = "Ticket_DisplayTST Error.";
                    Message += strRequest;
                    strResponseTST = SendDisplayTST(ttAA);
                    Message += strResponseTST;
                    native += $"{strRequest}{strResponseTST}";
                }

                #region GetPricingOptions

                var GetPricingOptionsTST = string.Empty;
                if (strResponse.Contains("<longFreetext>--- TST ") && strResponseTST.Contains("<referenceType>TST</referenceType>"))
                {
                    var oDocTST = new XmlDocument();
                    oDocTST.LoadXml(strResponseTST);
                    XmlElement oRootTST = oDocTST.DocumentElement;
                    var xmlNodeList = oRootTST?.SelectNodes("fareList");

                    if (xmlNodeList != null)
                        foreach (XmlNode oNodeTST in xmlNodeList)
                        {
                            string tstPricingOption = string.Empty;
                            string tstNum = oNodeTST.SelectSingleNode("fareReference/uniqueReference").InnerText;
                            strRequest = $"<Ticket_GetPricingOptions xmlns=\"http://xml.amadeus.com/TPORRQ_14_1_1A\"><documentSelection><referenceType>TST</referenceType><uniqueReference>{tstNum}</uniqueReference></documentSelection></Ticket_GetPricingOptions>";
                            tstPricingOption = SendGetPricingOptions(ttAA, strRequest);

                            if (tstPricingOption.Contains("<Error>"))
                                throw new Exception(tstPricingOption);

                            GetPricingOptionsTST += tstPricingOption;
                        }
                }

                #endregion

                //**************************************************************************** 
                // Add Previous Errors and Warnings To Amadeus Native End Transact Response * 
                //**************************************************************************** 
                strNativePNRReply = strResponse.Replace("</PNR_Reply>", $"{Errors}{Warnings}{strResponseTST}{GetPricingOptionsTST}{strRequest}</PNR_Reply>");
                
                //***************************************************************** 
                // Transform Native Amadeus TravelBuild Response into OTA Response * 
                //***************************************************************** 
                try
                {
                    strErrEvent = "AmadeusWS_PNRReadRS.xsl Error.";
                    Version = "v03";//strRequest.Contains("v03") ? "v03_" : "v04_";
                    
                    strNativePNRReply = inSession
                        ? strNativePNRReply.Replace("</PNR_Reply>", $"<ConversationID>{ConversationID}</ConversationID></PNR_Reply>")
                        : strNativePNRReply;

                    CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", String.Format("Final response size for version {0}", Version), strResponse.Length.ToString(CultureInfo.InvariantCulture), ttProviderSystems.LogUUID);
                    if (strNativePNRReply.Length > 5500)
                    {
                        CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response I", strNativePNRReply.Substring(0, strNativePNRReply.Length / 2), ttProviderSystems.LogUUID);
                        CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response II", strNativePNRReply.Substring(strNativePNRReply.Length / 2), ttProviderSystems.LogUUID);
                    }
                    else
                    {
                        CoreLib.SendTrace(ttProviderSystems.UserID, "PNRRead", "Final response I", strResponse, ttProviderSystems.LogUUID);
                    }

                    strResponse = CoreLib.TransformXML(strNativePNRReply, XslPath, $"{Version}AmadeusWS_PNRReadRS.xsl");
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
                        ConversationID = null;
                        ttAA = null;
                    }
                    oDocResp = null;
                }

                if (ttProviderSystems.LogNative)
                {
                    var strmsg = Message;
                    TripXMLTools.TripXMLLog.LogMessage("IssueTicket", ref strmsg, RequestTime, DateTime.Now, "Native", ttProviderSystems.Provider, ttProviderSystems.System, ttProviderSystems.UserName);
                }
            }
            catch (Exception exx)
            {
                throw new Exception($"{strErrEvent}\r\n{exx.Message}");
            }
            finally
            {
                GC.Collect();
            }

            return strResponse;
        }

        private void LogBlackList(string blackList)
        {
            try
            {
                var bl = new wsBlackList.wsFlightBlackList();
                string airline = "";
                string flno = "";
                string cos = "";
                DateTime dep;

                string strError = "";
                string sPath = "";

                var oDoc = new XmlDocument();
                oDoc.LoadXml($"<Errors>{blackList}</Errors>");
                var oRoot = oDoc.DocumentElement;

                foreach (XmlNode oNode in oRoot.ChildNodes)
                {
                    if (oNode.InnerText.IndexOf("Flight ") != -1)
                    {
                        strError = oNode.InnerText;
                        airline = strError.Substring(7, 2);
                        flno = strError.Substring(9, strError.IndexOf("Class") - 1 - 9);
                        cos = strError.Substring(strError.IndexOf("Class") + 6, 1);
                        dep = Convert.ToDateTime(strError.Substring(strError.IndexOf("Date") + 5, 10));
                        bl.wmFlightAdd(airline, flno, dep, cos);
                    }
                }

                strError = CoreLib.TransformXML(nativeResp, XslPath, $"{Version}SellIssuesRQ.xsl");

                sPath = ConfigurationManager.AppSettings["TripXMLFolder"];//System.Configuration.ConfigurationSettings.AppSettings.Get("TripXMLFolder");
                sPath += "\\Xsl\\AmadeusWS\\";

                oDoc = new XmlDocument();
                oDoc.LoadXml(strError);
                oRoot = oDoc.DocumentElement;

                var sw = new StreamWriter($"{sPath}FailedBookingsList.xls", true, Encoding.ASCII);

                lock (sw)
                {
                    foreach (XmlNode oNode in oRoot.ChildNodes)
                    {
                        sw.WriteLine(oNode.InnerText);
                    }

                    sw.Close();
                }
            }
            catch (Exception exx)
            {
                addLog($"<Error>{exx.Message}</Error>", ttProviderSystems.UserID);
            }
            finally
            {
                GC.Collect();
            }
        }

        public void SavePNRToAdmin()
        {

            try
            {
                ConnectorService.ConnectorClient cli = new ConnectorService.ConnectorClient();
                ////********************************************
                // 'bSendEmail' is was not in local code
                // *********************************************
                bool bSendEmail = false;
                List<string> portalSession = saveDbData.portalSession.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                CoreLib.SendTrace(ttProviderSystems.UserID, "SavePNRToAdmin", $"SiteID:{portalSession[0]}", saveDbData.TravelBuildRS, ttProviderSystems.LogUUID);

                // *****************************************
                // Following if block was not in local code
                //******************************************

                if (ttProviderSystems.SendEmailToAgency)
                    bSendEmail = true;

                //******************************************************************************
                //the following method call was done without bSendEmail parameter in local code
                //******************************************************************************
                cli.CreateBooking(Convert.ToInt16(portalSession[0]), 1, saveDbData.TravelBuildRS, bSendEmail);

            }
            catch (Exception ex)
            {
                CoreLib.SendTrace(ttProviderSystems.UserID, "SavePNRToAdmin", "Exception error", ex.Message, ttProviderSystems.LogUUID);
            }

            //*************************************************
            //the following if condotion was not there in local code, but content was there
            //*************************************************

            if (ttProviderSystems.CreateInRHAdmin)
            {
                StreamWriter oWriter = null;
                HttpWebRequest mHttpRequest;

                mHttpRequest = (HttpWebRequest)WebRequest.Create("http://admin.globalreservation.com/import/rastko.php");
                mHttpRequest.Method = "POST";
                mHttpRequest.ContentType = "text/xml ; charset=utf-8";
                mHttpRequest.KeepAlive = false;
                mHttpRequest.Timeout = 15000;
                mHttpRequest.ContentLength = saveDbData.TravelBuildRS.Length;
                try
                {
                    oWriter = new StreamWriter(mHttpRequest.GetRequestStream());
                    oWriter.Write(saveDbData.TravelBuildRS);
                }
                catch (Exception ex)
                {
                    CoreLib.SendTrace(ttProviderSystems.UserID, "SavePNRToAdmin", "Exception error", ex.Message, ttProviderSystems.LogUUID);
                }
                finally
                {
                    if (!(oWriter == null))
                    {
                        oWriter.Close();
                    }
                }

                try
                {
                    HttpWebResponse oHttpResponse = ((HttpWebResponse)(mHttpRequest.GetResponse()));
                    Stream stream = oHttpResponse.GetResponseStream();
                    StreamReader oReader = new StreamReader(stream);
                    string adminResp = oReader.ReadToEnd();
                    CoreLib.SendTrace(ttProviderSystems.UserID, "SavePNRToAdmin", "Admin response", adminResp, ttProviderSystems.LogUUID);
                }
                catch (Exception ex)
                {
                    CoreLib.SendTrace(ttProviderSystems.UserID, "SavePNRToAdmin", "Exception error", ex.Message, ttProviderSystems.LogUUID);
                }
            }
        }

        private void SendMail(string Username)
        {
            string result = DecodeTravelBuild(saveDbData.TravelBuildRS, "");
            //result = DecodeTravelBuild(result, "");
            StringReader reader = new StringReader(result);
            XmlDocument TravelBuildResult = new XmlDocument();
            string PNR = "";
            string LName = "";
            string FName = "";
            string ContactName = "";
            string CCNo = "";
            double BaseAmount = 0;
            double TaxAmount = 0;
            double totAmount = 0;
            string address1 = "";
            string address2 = "";
            string city = "";
            string state = "";
            string country = "";
            string Title = "";
            string PType = "";
            string DOB = "";
            string zip = "";

            if (result.IndexOf("<Success />") == -1)
            {
                return;
            }

            try
            {
                TravelBuildResult.Load(reader);
                XmlNodeList node = TravelBuildResult.GetElementsByTagName("OTA_TravelItineraryRS");
                PNR = node[0].SelectSingleNode("TravelItinerary").SelectSingleNode("ItineraryRef").Attributes["ID"].Value.ToString();

                XmlNodeList TravNodes = node[0].SelectSingleNode("TravelItinerary").SelectSingleNode("CustomerInfos").SelectNodes("CustomerInfo");
                StringBuilder sbTrav = new StringBuilder();
                sbTrav = sbTrav.Append("<table width=\"70%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr><th width=\"60%\" scope=\"col\">Passenger Name</th><th width=\"40%\" scope=\"col\">Passenger Type</th></tr>");
                foreach (XmlNode nd in TravNodes)
                {
                    LName = "";
                    FName = "";
                    FName = nd.SelectSingleNode("Customer").SelectSingleNode("PersonName").SelectSingleNode("GivenName").InnerText;

                    if (FName.IndexOf(" ") != -1)
                    {
                        Title = FName.Substring(FName.IndexOf(" ") + 1, FName.Length - (FName.IndexOf(" ") + 1));
                        FName = FName.Substring(0, FName.IndexOf(" "));
                    }

                    LName = nd.SelectSingleNode("Customer").SelectSingleNode("PersonName").SelectSingleNode("Surname").InnerText;
                    PType = nd.SelectSingleNode("Customer").SelectSingleNode("PersonName").Attributes["NameType"].Value;


                    if (nd.Attributes["RPH"].Value == "1")
                    {
                        if (Title.Length > 0)
                            ContactName = Title + "." + FName + " " + LName;
                        else
                            ContactName = FName + " " + LName;
                        if (nd.SelectSingleNode("Customer").SelectNodes("Address") != null
                            && nd.SelectSingleNode("Customer").SelectNodes("Address")[0] != null
                            && nd.SelectSingleNode("Customer").SelectNodes("Address")[0].SelectSingleNode("StreetNmbr") != null
                            )
                        {
                            address1 = nd.SelectSingleNode("Customer").SelectNodes("Address")[0].SelectSingleNode("StreetNmbr").InnerText;
                            if (nd.SelectSingleNode("Customer").SelectNodes("Address")[0].SelectSingleNode("CityName") != null)
                            {
                                city = nd.SelectSingleNode("Customer").SelectNodes("Address")[0].SelectSingleNode("CityName").InnerText;
                            }
                            if (nd.SelectSingleNode("Customer").SelectNodes("Address")[0].SelectSingleNode("PostalCode") != null)
                            {
                                city = nd.SelectSingleNode("Customer").SelectNodes("Address")[0].SelectSingleNode("PostalCode").InnerText;
                            }
                            if (nd.SelectSingleNode("Customer").SelectNodes("Address")[0].SelectSingleNode("CountryName") != null)
                            {
                                country = nd.SelectSingleNode("Customer").SelectNodes("Address")[0].SelectSingleNode("CountryName").Attributes["Code"].Value;
                            }
                            if (nd.SelectSingleNode("Customer").SelectNodes("Address")[0].SelectSingleNode("StateProv") != null)
                            {
                                state = nd.SelectSingleNode("Customer").SelectNodes("Address")[0].SelectSingleNode("StateProv").InnerText;
                            }

                        }

                    }
                    sbTrav = sbTrav.Append("<tr><td>").Append(LName).Append(", ").Append(FName).Append("</td><td align=\"center\">").Append(PType).Append("</td></tr>");


                }
                sbTrav = sbTrav.Append("</table>");
                if (node[0].SelectSingleNode("TravelItinerary").SelectSingleNode("TravelCost") != null
                    && node[0].SelectSingleNode("TravelItinerary").SelectSingleNode("TravelCost").SelectSingleNode("FormOfPayment") != null
                    && node[0].SelectSingleNode("TravelItinerary").SelectSingleNode("TravelCost").SelectNodes("FormOfPayment")[0].SelectSingleNode("TPA_Extensions") != null
                    && node[0].SelectSingleNode("TravelItinerary").SelectSingleNode("TravelCost").SelectNodes("FormOfPayment")[0].SelectSingleNode("TPA_Extensions").Attributes["FOPType"] != null
                    && node[0].SelectSingleNode("TravelItinerary").SelectSingleNode("TravelCost").SelectNodes("FormOfPayment")[0].SelectSingleNode("TPA_Extensions").Attributes["FOPType"].Value == "CC"
                    )
                {
                    CCNo = node[0].SelectSingleNode("TravelItinerary").SelectSingleNode("TravelCost").SelectNodes("FormOfPayment")[0].SelectSingleNode("PaymentCard").Attributes["CardNumber"].Value;

                }
                else
                {
                    CCNo = "N/A";
                }
                StringBuilder sbPayment = new StringBuilder();
                sbPayment = sbPayment.Append("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
                if (node[0].SelectSingleNode("TravelItinerary").SelectNodes("ItineraryInfo")[0].SelectSingleNode("ReservationItems").SelectSingleNode("ItemPricing") != null
                    && node[0].SelectSingleNode("TravelItinerary").SelectNodes("ItineraryInfo")[0].SelectSingleNode("ReservationItems").SelectSingleNode("ItemPricing").SelectSingleNode("AirFareInfo") != null
                    && node[0].SelectSingleNode("TravelItinerary").SelectNodes("ItineraryInfo")[0].SelectSingleNode("ReservationItems").SelectSingleNode("ItemPricing").SelectSingleNode("AirFareInfo").SelectSingleNode("ItinTotalFare") != null
                    && node[0].SelectSingleNode("TravelItinerary").SelectNodes("ItineraryInfo")[0].SelectSingleNode("ReservationItems").SelectSingleNode("ItemPricing").SelectSingleNode("AirFareInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare") != null
                )
                {


                    BaseAmount = double.Parse(node[0].SelectSingleNode("TravelItinerary").SelectNodes("ItineraryInfo")[0].SelectSingleNode("ReservationItems").SelectSingleNode("ItemPricing").SelectSingleNode("AirFareInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["Amount"].Value);
                    double decplaces = double.Parse(node[0].SelectSingleNode("TravelItinerary").SelectNodes("ItineraryInfo")[0].SelectSingleNode("ReservationItems").SelectSingleNode("ItemPricing").SelectSingleNode("AirFareInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["DecimalPlaces"].Value);
                    BaseAmount = BaseAmount / Math.Pow(10, decplaces);
                    TaxAmount = double.Parse(node[0].SelectSingleNode("TravelItinerary").SelectNodes("ItineraryInfo")[0].SelectSingleNode("ReservationItems").SelectSingleNode("ItemPricing").SelectSingleNode("AirFareInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").Attributes["Amount"].Value);
                    decplaces = double.Parse(node[0].SelectSingleNode("TravelItinerary").SelectNodes("ItineraryInfo")[0].SelectSingleNode("ReservationItems").SelectSingleNode("ItemPricing").SelectSingleNode("AirFareInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").Attributes["DecimalPlaces"].Value);
                    TaxAmount = TaxAmount / Math.Pow(10, decplaces);
                }
                if (BaseAmount > 0)
                {
                    sbPayment = sbPayment.Append("<tr><td>Base Fare</td><td align=\"right\">").Append(String.Format("{0:0.00}", BaseAmount)).Append(" ").Append(node[0].SelectSingleNode("TravelItinerary").SelectNodes("ItineraryInfo")[0].SelectSingleNode("ReservationItems").SelectSingleNode("ItemPricing").SelectSingleNode("AirFareInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("BaseFare").Attributes["CurrencyCode"].Value).Append("</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
                }
                else
                {
                    sbPayment = sbPayment.Append("<tr><td>Base Fare</td><td align=\"right\">").Append("N/A").Append(" ").Append("</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
                }
                if (TaxAmount > 0)
                {
                    sbPayment = sbPayment.Append("<tr><td>Total Taxes</td><td align=\"right\">").Append(String.Format("{0:0.00}", TaxAmount)).Append(" ").Append(node[0].SelectSingleNode("TravelItinerary").SelectNodes("ItineraryInfo")[0].SelectSingleNode("ReservationItems").SelectSingleNode("ItemPricing").SelectSingleNode("AirFareInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").Attributes["CurrencyCode"].Value).Append("</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
                }
                else
                {
                    sbPayment = sbPayment.Append("<tr><td>Total Taxes</td><td align=\"right\">").Append("N/A").Append(" ").Append("</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
                }
                totAmount = TaxAmount + BaseAmount;
                if (totAmount > 0)
                {
                    sbPayment = sbPayment.Append("<tr><td>Total Fare</td><td align=\"right\">").Append(String.Format("{0:0.00}", totAmount)).Append(" ").Append(node[0].SelectSingleNode("TravelItinerary").SelectNodes("ItineraryInfo")[0].SelectSingleNode("ReservationItems").SelectSingleNode("ItemPricing").SelectSingleNode("AirFareInfo").SelectSingleNode("ItinTotalFare").SelectSingleNode("Taxes").Attributes["CurrencyCode"].Value).Append("</td><td>&nbsp;</td><td>&nbsp;</td></tr></table>");
                }
                else
                {
                    sbPayment = sbPayment.Append("<tr><td>Total Fare</td><td align=\"right\">").Append("N/A").Append(" ").Append("</td><td>&nbsp;</td><td>&nbsp;</td></tr></table>");
                }


                XmlNodeList AirNodes = node[0].SelectSingleNode("TravelItinerary").SelectSingleNode("ItineraryInfo").SelectSingleNode("ReservationItems").SelectNodes("Item");
                StringBuilder sbAir = new StringBuilder();
                sbAir = sbAir.Append("<table border=\"0\" cellspacing=\"1\" cellpadding=\"0\" width=\"100%\">");
                foreach (XmlNode nd in AirNodes)
                {
                    if (nd.SelectNodes("Air").Count > 0)
                    {
                        sbAir = sbAir.Append("<tr>");
                        sbAir = sbAir.Append("<td valign=\"top\"><p><strong>Flight#:</strong>").Append(nd.SelectNodes("Air")[0].SelectSingleNode("MarketingAirline").Attributes["Code"].Value).Append(nd.SelectNodes("Air")[0].Attributes["FlightNumber"].Value).Append("</p></td>");
                        sbAir = sbAir.Append("<td valign=\"top\"><p align=\"center\">from </p></td>");
                        sbAir = sbAir.Append("<td valign=\"top\"><p>").Append(nd.SelectNodes("Air")[0].SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value).Append("<br />");
                        sbAir = sbAir.Append("Departs: ").Append(nd.SelectNodes("Air")[0].Attributes["DepartureDateTime"].Value.Replace("T", " ")).Append("</p></td>");
                        sbAir = sbAir.Append("<td valign=\"top\"><p align=\"center\">to </p></td>");
                        sbAir = sbAir.Append("<td valign=\"top\"><p>").Append(nd.SelectNodes("Air")[0].SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value).Append("<br />");
                        sbAir = sbAir.Append("Arrive: ").Append(nd.SelectNodes("Air")[0].Attributes["ArrivalDateTime"].Value.Replace("T", " ")).Append("</p></td>");
                        sbAir = sbAir.Append("</tr>");
                        sbAir = sbAir.Append("<tr>");
                        sbAir = sbAir.Append("<td></td>");
                        sbAir = sbAir.Append("<td colspan=\"3\" valign=\"top\"><p>This flight is operated by ").Append(nd.SelectNodes("Air")[0].SelectSingleNode("OperatingAirline").Attributes["Code"].Value).Append("</p></td>");
                        sbAir = sbAir.Append("</tr>");
                    }
                }
                sbAir = sbAir.Append("</table>");

                string StrBody = "";
                System.IO.StreamReader SR = null;
                try
                {
                    //SR = new System.IO.StreamReader(System.Web.HttpContext.Current.Server.MapPath("~") + "\\MailTemplates\\BookingEmail.html");
                    SR = new System.IO.StreamReader("C:\\tripxml\\MailTemplates\\BookingEmail.html");

                    StrBody = SR.ReadToEnd();

                }
                catch (Exception Exc)
                {

                }
                finally
                {
                    SR.Close();
                }

                StrBody = StrBody.Replace("@@Name@@", ContactName);
                StrBody = StrBody.Replace("@@Air@@", sbAir.ToString());
                //sbAir = sbAir.Remove(0, sbAir.Length);
                StrBody = StrBody.Replace("@@Trav@@", sbTrav.ToString());
                //sbTrav = sbTrav.Remove(0, sbTrav.Length);
                StrBody = StrBody.Replace("@@Price@@", sbPayment.ToString());
                //sbPayment = sbPayment.Remove(0, sbPayment.Length);

                StrBody = StrBody.Replace("@@Amount@@", totAmount.ToString());
                StrBody = StrBody.Replace("@@PNR@@", PNR);
                StrBody = StrBody.Replace("@@CCNo@@", CCNo);
                StrBody = StrBody.Replace("@@Address1@@", address1);
                StrBody = StrBody.Replace("@@Address2@@", address2);
                StrBody = StrBody.Replace("@@City@@", city);
                StrBody = StrBody.Replace("@@ZIP@@", zip);
                StrBody = StrBody.Replace("@@Country@@", country);
                StrBody = StrBody.Replace("@@State@@", state);


                // removed and send using the method on  CoreLib.

                //System.Net.Mail.MailMessage objMessage = new System.Net.Mail.MailMessage();
                //objMessage.Body = StrBody;
                //objMessage.Subject = "Booking Confirmation";
                //objMessage.From = new System.Net.Mail.MailAddress("info@globalreservation.us");
                ////objMessage.To.Add("agent@globalreservation.us");
                //objMessage.To.Add("shashin@thomalex.com");
                //objMessage.CC.Add("shashinw@gmail.com");
                //SmtpClient client = new SmtpClient();
                //objMessage.IsBodyHtml = true;

                //client.Send(objMessage);


                // use this when credentials are there (username & password)
                string strFrom = Username;

                if (Username == "morqua")
                    strFrom = "anywayanyday";

                //CoreLib.SendEmail("Booking Confirmation from " + strFrom + " PNR:" + PNR, StrBody, Username);

            }
            catch (Exception er)
            {
                string err = er.Message;
            }

        }

        private string DecodeTravelBuild(string strResponse, string UserID)
        {
            try
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.LoadXml(strResponse);
                XmlElement oRoot = oDoc.DocumentElement;

                DataView ttAirports = (DataView)System.Web.HttpContext.Current.Application.Get("ttAirports");
                DataView ttAirlines = (DataView)System.Web.HttpContext.Current.Application.Get("ttAirlines");
                DataView ttEquipments = (DataView)System.Web.HttpContext.Current.Application.Get("ttEquipments");

                foreach (XmlNode oNode in oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item/Air"))
                {
                    // *******************
                    // Decode Airports   *
                    // *******************
                    string tempStr = "";
                    if ((oNode.SelectSingleNode("DepartureAirport") != null))
                    {
                        tempStr = oNode.SelectSingleNode("DepartureAirport").Attributes["LocationCode"].Value;
                        oNode.SelectSingleNode("DepartureAirport").InnerText = GetDecodeValue(ref ttAirports, ref tempStr);
                    }
                    if ((oNode.SelectSingleNode("ArrivalAirport") != null))
                    {
                        tempStr = oNode.SelectSingleNode("ArrivalAirport").Attributes["LocationCode"].Value;
                        oNode.SelectSingleNode("ArrivalAirport").InnerText = GetDecodeValue(ref ttAirports, ref tempStr);
                    }

                    // *******************
                    // Decode Airlines   *
                    // *******************
                    if ((oNode.SelectSingleNode("OperatingAirline") != null) & (oNode.SelectSingleNode("OperatingAirline").Attributes["Code"] != null))
                    {
                        if (!string.IsNullOrEmpty(oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value))
                        {
                            tempStr = oNode.SelectSingleNode("OperatingAirline").Attributes["Code"].Value;
                            oNode.SelectSingleNode("OperatingAirline").InnerText = GetDecodeValue(ref ttAirlines, ref tempStr);
                        }
                    }

                    if ((oNode.SelectSingleNode("MarketingAirline") != null))
                    {
                        if ((oNode.SelectSingleNode("MarketingAirline").Attributes["Code"] != null))
                        {
                            tempStr = oNode.SelectSingleNode("MarketingAirline").Attributes["Code"].Value;
                            oNode.SelectSingleNode("MarketingAirline").InnerText = GetDecodeValue(ref ttAirlines, ref tempStr);
                        }
                    }
                    // *******************
                    // Decode Equipments *
                    // *******************
                    if ((oNode.SelectSingleNode("Equipment") != null))
                    {
                        if (oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"] != null)
                        {
                            tempStr = oNode.SelectSingleNode("Equipment").Attributes["AirEquipType"].Value;
                            oNode.SelectSingleNode("Equipment").InnerText = GetDecodeValue(ref ttEquipments, ref tempStr);
                        }
                    }
                }

                strResponse = oDoc.OuterXml;

            }
            catch (Exception ex)
            {
                CoreLib.SendTrace(ttProviderSystems.UserID, "wsTravelServices", "Error *** Decoding TravelBuild Response", ex.Message, ttProviderSystems.LogUUID);
            }

            return strResponse;

        }

        private string GetDecodeValue(ref DataView oDV, ref string strCode)
        {
            int i = oDV.Find(strCode);
            return i > -1 ? oDV[i]["Name"].ToString() : "";
        }

        private string getMonth(int month)
        {
            return ((enMonth)month).ToString();
        }

    }

}