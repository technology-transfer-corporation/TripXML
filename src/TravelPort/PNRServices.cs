using System.Xml;
using TripXMLMain;
using System.Text.RegularExpressions;
using System;
using System.Globalization;
using System.IO;

namespace Travelport
{
    public class PNRServices : TravelportBase
    {
        public string PNRRead()
        {
            string strResponse;
            //*****************************************************************
            // Transform OTA PNRRead Request into Native Amadeus Request     *
            //***************************************************************** 

            try
            {
                DateTime RequestTime = DateTime.Now;

                string strRetrieve;
                string strSearch;
                string strImport;

                #region Get Tracer ID
                branch = ProviderSystems.Profile.Text;
                if (!Request.Contains("<RequestorID Type=\"21\" Instance"))
                    Request = Request.Replace("RequestorID Type=\"21\" ", $"RequestorID Type=\"21\" Instance=\"{branch}\" ");

                string strRequest = SetRequest("Travelport_PNRReadRQ.xsl");
                CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Request", strRequest, ProviderSystems.LogUUID);
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                XmlDocument oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                XmlElement oRoot = oDoc.DocumentElement;
                var recordLocator = oRoot.SelectSingleNode("UniqueID/@ID") != null
                    ? oRoot.SelectSingleNode("UniqueID/@ID").Value
                    : string.Empty;

                if (oRoot.HasAttribute("Target"))
                {
                    switch (oRoot.Attributes["Target"].Value)
                    {
                        case "WSP":
                            host = "1P";
                            break;
                        case "GAL":
                            host = "1G";
                            break;
                        default:
                            host = "1V";
                            break;
                    }
                }

                #endregion

                oDoc.LoadXml(strRequest);
                var natElement = oDoc.DocumentElement;

                strRetrieve = natElement.SelectSingleNode("RetrieveReq").InnerXml;
                strSearch = natElement.SelectSingleNode("SearchReq").InnerXml;
                strImport = natElement.SelectSingleNode("ImportReq").InnerXml;

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                //*******************************************************************************
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                //******************************************************************************* 

                var ttProviderSystems = ProviderSystems;
                TravelPortWSAdapter ttTP = SetAdapter(ttProviderSystems);
                bool inSession = SetConversationID(ttTP, host, branch);



                // send retrieve universal record (UR)
                strResponse = ttTP.SendMessage(strRetrieve, TravelPortWSAdapter.enRequestType.UniversalRecordService);

                if (strResponse.Contains("Record locator not found"))
                {
                    // search UR by GDS locator
                    strResponse = ttTP.SendMessage(strSearch, TravelPortWSAdapter.enRequestType.UniversalRecordService);

                    if (strResponse.Contains("No matching records found for the given parameters"))
                    {
                        // GDS locator does not exist as UR, so import it
                        strResponse = ttTP.SendMessage(strImport, TravelPortWSAdapter.enRequestType.UniversalRecordService);
                    }
                    //------------------------------------------------------------
                    // It appears that no need for any prior manipulations with message
                    //------------------------------------------------------------
                    //else
                    //{
                    //    // get UR locator and retrieve it
                    //    XmlDocument otaDoc = new XmlDocument();
                    //    otaDoc.LoadXml(strResponse);
                    //    var otaElement = otaDoc.DocumentElement;

                    //    var nsmgr = new XmlNamespaceManager(otaDoc.NameTable);
                    //    nsmgr.AddNamespace("un", "http://www.travelport.com/schema/universal_v27_0");

                    //    var strURRecLoc = otaElement.SelectSingleNode("un:UniversalRecordSearchResult/@UniversalRecordLocatorCode", nsmgr).InnerText;

                    //    strRetrieve = strRetrieve.Replace(strProviderRecLoc, strURRecLoc);

                    //    strResponse = ttTP.SendMessage(strRetrieve, TravelPortWSAdapter.enRequestType.UniversalRecordService);
                    //}
                }

                if (strResponse.StartsWith("<Errors"))
                    strResponse = $"<universal:UniversalRecordRetrieveRsp>{strResponse}</universal:UniversalRecordRetrieveRsp>";

                //strResponse = strResponse.Replace(" xmlns=\"http://xml.amadeus.com/" + ttProviderSystems.TravelportSchema.PNR_RetrieveByRecLocReply + "\"", "");

                //*****************************************************************
                // Transform Native Amadeus PNRRead Response into OTA Response   *
                //***************************************************************** 
                try
                {
                    var strToReplace = "</universal:UniversalRecordRetrieveRsp>";
                    if (inSession)
                        strResponse = strResponse.Replace(strToReplace, $"<ConversationID>{ConversationID}</ConversationID>{strToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "PNRRead", "Final response", strResponse, ProviderSystems.LogUUID);

                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Travelport_PNRReadRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
            }
            catch (Exception exx)
            {
                AddLog($"<M>{Request}<BL/>", ProviderSystems);
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRRead, exx.Message, ProviderSystems);
            }
            return strResponse;
        }

        public string PNRReprice()
        {
            string response;
            DateTime RequestTime = DateTime.Now;
            //******************************************************************
            // Transform OTA PNRReprice Request into Native Travelport Request * 
            //****************************************************************** 
            var oDoc = new XmlDocument();
            try
            {
                branch = ProviderSystems.Profile.Text;
                TravelPortWSAdapter ttTP = SetAdapter(ProviderSystems);

                if (!Request.Contains("<RequestorID Type=\"21\" Instance"))
                    Request = Request.Replace("RequestorID Type=\"21\" ", $"RequestorID Type=\"21\" Instance=\"{branch}\" ");
                Request = Request.Replace("</ConversationID>", $"</ConversationID><UUID>{ProviderSystems.LogUUID}</UUID> ");

                string strRequest = SetRequest("Travelport_PNRRepriceRQ.xsl");

                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                oDoc.LoadXml(Request);
                XmlElement oRoot = oDoc.DocumentElement;
                var recordLocator = oRoot.SelectSingleNode("UniqueID/@ID") != null
                    ? oRoot.SelectSingleNode("UniqueID/@ID").Value
                    : string.Empty;

                //var oldSessionID = oRoot.SelectSingleNode("UniqueID/@ID") != null
                //    ? oRoot.SelectSingleNode("ConversationID").Value
                //    : string.Empty;

                var isPricePublished = Request.Contains(" FareType=\"Published\" ") || Request.Contains("Markup") || Request.Contains("TourCode") || Request.Contains("Endorsement");
                var withBrands = Request.Contains("BrandedFares");

                if (oRoot.HasAttribute("Target"))
                {
                    switch (oRoot.Attributes["Target"].Value)
                    {
                        case "WSP":
                            host = "1P";
                            break;
                        case "GAL":
                            host = "1G";
                            break;
                        default:
                            host = "1V";
                            break;
                    }
                }

                //*******************************************************************************
                // Send Transformed Request to the Amadeus Adapter and Getting Native Response  *
                //******************************************************************************* 
                string strRetrieve;

                bool inSession = SetConversationID(ttTP, host);
                //ttTP.TracerID = ConversationID;

                // send retrieve universal record (UR)                
                response = ttTP.SendMessage(strRequest, TravelPortWSAdapter.enRequestType.UniversalRecordService);

                strRetrieve = response;

                if (response.Contains("universal:UniversalRecord LocatorCode="))
                {
                    // create and send pricing message
                    strRequest = Request.Replace("</OTA_PNRRepriceRQ>", $"<Response>{response}</Response></OTA_PNRRepriceRQ>");
                    var airRQ = CoreLib.TransformXML(strRequest, XslPath, $"{Version}Travelport_PNRRepriceRQ.xsl", false);
                    var airRS = ttTP.SendMessage(airRQ, TravelPortWSAdapter.enRequestType.AirService);
                    CoreLib.SendTrace(ProviderSystems.UserID, "PNRReprice", "Price Request", airRS, ProviderSystems.LogUUID);
                    
                    //Repeat call if response is empty
                    if (airRS.Contains("Unexpected system error."))
                    { 
                        airRS = ttTP.SendMessage(airRQ, TravelPortWSAdapter.enRequestType.AirService);
                        CoreLib.SendTrace(ProviderSystems.UserID, "PNRReprice", "Repeated Price Request", airRQ, ProviderSystems.LogUUID);
                    }

                    strRetrieve = strRetrieve.Replace("</universal:UniversalRecordRetrieveRsp>", $"{airRS}</universal:UniversalRecordRetrieveRsp>");
                    
                    //Check RePrice response for correct FareBasis Codes
                    var _FBCheck = CheckFareBasisCodes(airRS);
                    if (!_FBCheck.IsChecked)
                    {
                        strRetrieve = strRetrieve.Replace("</universal:UniversalRecordRetrieveRsp>", $"<Errors>{_FBCheck.Message}</Errors></universal:UniversalRecordRetrieveRsp>");
                    }
                    else
                    {
                        if (Request.Contains("StoreFare=\"true\""))
                        {
                            string modRS = string.Empty; //response;

                            // store new pricing in UR                        
                            var modRQ = strRequest.Replace("</OTA_PNRRepriceRQ>", $"<NewPrice>{airRS}</NewPrice></OTA_PNRRepriceRQ>");
                            modRQ = CoreLib.TransformXML(modRQ, XslPath, $"{Version}Travelport_PNRRepriceRQ.xsl", false);
                            modRS = ttTP.SendMessage(modRQ, TravelPortWSAdapter.enRequestType.UniversalRecordService);
                            strRetrieve = strRetrieve.Replace("</universal:UniversalRecordRetrieveRsp>", $"{modRS}</universal:UniversalRecordRetrieveRsp>");

                            if (isPricePublished && (oRoot.SelectNodes("StoredFare[TourCode or Endorsement or Markup]").Count > 0))
                            {
                                //&& oRoot.SelectNodes("StoredFare[@FareType='Published']").Count > 0)
                                //<Response>{modRS}</Response>
                                var universalRecord = modRS.Contains("AirPricingInfo") ? modRS : response;
                                universalRecord = UpdateRQVesion(universalRecord, modRS);
                                modRQ = Request.Replace("</OTA_PNRRepriceRQ>", $"<Response>{universalRecord}</Response></OTA_PNRRepriceRQ>").Replace("</OTA_PNRRepriceRQ>", $"<UpdatePrice>{airRS}</UpdatePrice></OTA_PNRRepriceRQ>");
                                CoreLib.SendTrace(ProviderSystems.UserID, "PNRReprice", "UpdatePrice RQ", modRQ, ProviderSystems.LogUUID);
                                modRQ = CoreLib.TransformXML(modRQ, XslPath, $"{Version}Travelport_PNRRepriceRQ.xsl", false);
                                CoreLib.SendTrace(ProviderSystems.UserID, "PNRReprice", "Store Price RS", modRQ, ProviderSystems.LogUUID);
                                modRS = ttTP.SendMessage(modRQ, TravelPortWSAdapter.enRequestType.UniversalRecordService);
                                strRetrieve = strRetrieve.Replace("</universal:UniversalRecordRetrieveRsp>", $"{modRS}</universal:UniversalRecordRetrieveRsp>");
                            }
                        }
                    }
                }

                //********************************************************************
                // Transform Native Travelport PNRReprice Response into OTA Response *
                //******************************************************************** 
                try
                {
                    var strToReplace = "</universal:UniversalRecordRetrieveRsp>";
                    if (inSession)
                        strRetrieve = strRetrieve.Replace(strToReplace, $"<ConversationID>{ConversationID}</ConversationID>{strToReplace}");

                    CoreLib.SendTrace(ProviderSystems.UserID, "PNRReprice", "Final response", strRetrieve, ProviderSystems.LogUUID);
                    response = CoreLib.TransformXML(strRetrieve, XslPath, $"{Version}Travelport_PNRRepriceRS.xsl");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
                finally
                {
                    if (!inSession)
                    {
                        ttTP.CloseTerminalSession(branch, host, ConversationID);
                        ConversationID = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                response = modCore.FormatErrorMessage(modCore.ttServices.PNRReprice, ex.Message, ProviderSystems);
            }
            return response;
        }

        private (bool IsChecked, string Message) CheckFareBasisCodes(string airRS)
        {
            try
            {
                var oDoc = new XmlDocument();
                oDoc.LoadXml(Request);
                XmlElement oRoot = oDoc.DocumentElement;
                var storedFares = oRoot.SelectNodes("StoredFare");

                foreach (XmlNode fare in storedFares)
                {
                    var bExists = false;
                    var fareBasis = fare.SelectNodes("FareSegments");
                    var brands = fare.SelectNodes("BrandedFares");
                    if (fareBasis.Count.Equals(0))
                        continue;

                    if (brands.Count > 0)
                    {
                        var index = 0;
                        foreach (XmlNode fb in fareBasis)
                        {
                            var fareB = fb.SelectSingleNode("AirSegments").InnerText;

                            /********************************************************
                            //According to Irina we will check only Adult FareBasis, since they usually do not containes additional last two letters.
                            if (fareB.Length > 7)
                                fareB = fareB.Substring(0, 7);
                            ********************************************************/

                            var brand = brands.Item(index).SelectSingleNode("FareFamily").InnerText;
                            bExists = string.IsNullOrEmpty(brand) 
                                    ? airRS.Contains($"FareBasis=\"{fareB}\"") 
                                    : airRS.Contains($"FareBasis=\"{fareB}\"") && airRS.ToUpper().Contains($"NAME=\"{brand}\"");
                            index++;
                        }
                    }
                    else 
                    {
                        foreach (XmlNode fb in fareBasis)
                        {
                            bExists = airRS.Contains($"FareBasis=\"{fb.SelectSingleNode("AirSegments").InnerText}\"");
                        }
                    }

                    if (bExists)
                        return (true, string.Empty);
                }

                throw new Exception("Returned price do not match to requested FareBasis.");
            }
            catch (Exception ex)
            {
                return (false, $"<Error>{ex.Message}</Error>");
            }
        }

        private string UpdateRQVesion(string universalRecord, string modRS)
        {
            var version = Regex.Match(modRS, @"<universal:UniversalRecord.*(\sVersion=""\d+"")").Groups[1];
            return Regex.Replace(universalRecord, @"\sVersion=""\d+""", version.Value);
        }

        public string Queue()
        {
            string strResponse = String.Empty;
            //*****************************************************************
            // Transform OTA Queue Request into Native TravelPort Request     *
            //***************************************************************** 

            try
            {
                var oDoc = new XmlDocument();
                string strRequest = SetRequest("Travelport_QueueRQ.xsl");
                if (string.IsNullOrEmpty(strRequest))
                    throw new Exception("Transformation produced empty xml.");

                oDoc.LoadXml(Request);
                XmlElement oRoot = oDoc.DocumentElement;

                if (oRoot.HasAttribute("Target"))
                {
                    switch (oRoot.Attributes["Target"].Value)
                    {
                        case "WSP":
                            host = "1P";
                            break;
                        case "GAL":
                            host = "1G";
                            break;
                        default:
                            host = "1V";
                            break;
                    }
                }

                branch = oDoc.SelectSingleNode("POS/Source/@PseudoCityCode").InnerText;

                var ttProviderSystems = ProviderSystems;
                //*******************************************************************************
                // Send Transformed Request to the TravelPort Adapter and Getting Native Response  *
                //******************************************************************************* 

                var ttTP = SetAdapter(ttProviderSystems);
                bool inSession = SetConversationID(ttTP, host, branch);

                if (Request.IndexOf("ListQueue") != -1)
                {
                    if (strRequest.Contains("QLD"))
                    {
                        // process Worldspan queue
                        ConversationID = ttTP.CreateTerminalSession(branch, host);
                        strResponse = ttTP.SubmitTerminalTransaction(strRequest, branch, host, ConversationID);

                        if (strResponse.Contains(")"))
                        {
                            int i = 0;
                            var strResponseQL = strResponse;

                            while (i < 10 && strResponse.Contains(")"))
                            {
                                strResponse = ttTP.SubmitTerminalTransaction("MD", branch, host, ConversationID);
                                strResponseQL += strResponse;
                                i++;
                            }
                            strResponse = strResponseQL;
                        }

                        strResponse = $"<ListQueue>{strResponse}</ListQueue>";
                        ConversationID = ttTP.CloseTerminalSession(branch, host, ConversationID);
                    }
                    else
                    {
                        // process Galileo queue (TBD)
                    }
                }
                else if (Request.IndexOf("PlaceQueue") != -1)
                {
                    if (strRequest.Contains("QEP/"))
                    {
                        // process Worldspan queue
                        ConversationID = ttTP.CreateTerminalSession(branch, host);

                        // send *pnrloc
                        strResponse = ttTP.SubmitTerminalTransaction(strRequest.Substring(0, 7), branch, host, ConversationID);

                        if (!strResponse.Contains("INVALID") && !strResponse.Contains("INVLD ADDRESS"))
                        {
                            strResponse = ttTP.SubmitTerminalTransaction(strRequest.Substring(7), branch, host, ConversationID);
                        }

                        strResponse = $"<PlaceQueue>{strResponse}</PlaceQueue>";
                        ConversationID = ttTP.CloseTerminalSession(branch, host, ConversationID);
                    }
                    else
                    {
                        // process Galileo queue (TBD)
                    }
                }
                else if (Request.IndexOf("RemoveQueue") != -1)
                {
                    if (strRequest.Contains("QRQ/"))
                    {
                        // process Worldspan queue
                        ConversationID = ttTP.CreateTerminalSession(branch, host);

                        // send *pnrloc
                        strResponse = ttTP.SubmitTerminalTransaction(strRequest.Substring(0, 7), branch, host, ConversationID);

                        if (!strResponse.Contains("INVALID") && !strResponse.Contains("INVLD ADDRESS"))
                        {
                            strResponse = ttTP.SubmitTerminalTransaction(strRequest.Substring(7), branch, host, ConversationID);
                        }

                        strResponse = $"<RemoveQueue>{strResponse}</RemoveQueue>";

                        ConversationID = ttTP.CloseTerminalSession(branch, host, ConversationID);
                    }
                    else
                    {
                        // process Galileo queue (TBD)
                    }
                }

                //*******************************************************************************
                // check if message is queue lists and if need need to scroll the response      *
                //*******************************************************************************             
                ConversationID = "";
                ttTP = null;

                try
                {
                    strResponse = CoreLib.TransformXML(strResponse, XslPath, $"{Version}Travelport_QueueRS.xsl", false);
                    return strResponse;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error Transforming Native Response.\r\n{ex.Message}");
                }
            }
            catch (Exception ex)
            {
                strResponse = modCore.FormatErrorMessage(modCore.ttServices.PNRReprice, ex.Message, ProviderSystems);
            }
            return strResponse;

        }

        public static void addLog(string msg, string username)
        {
            try
            {
                string FilePath = $"log\\{username}_{DateTime.Today.ToString("dd-MM-yyyy")}";
                string DirPath = "C:\\TripXML\\log";
                FilePath = $"C:\\TripXML\\{FilePath}.txt";

                if (!Directory.Exists(DirPath))
                {
                    Directory.CreateDirectory(DirPath);
                }
                if (!File.Exists(FilePath))
                {
                    using (StreamWriter sw = File.CreateText(FilePath))
                    {
                        sw.WriteLine($"created On - {DateTime.Now}\r\n");
                        sw.Flush();
                        sw.Close();
                    }
                }
                using (StreamWriter sw = File.AppendText(FilePath))
                {
                    DateTimeFormatInfo myDTFI = new CultureInfo("en-US", true).DateTimeFormat;
                    sw.WriteLine($"{DateTime.UtcNow.ToString(myDTFI).Substring(11)} GMT - {msg}\r\n");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
